using Microsoft.AspNetCore.Mvc;
using ProductSolutions.Repository;
using ProductSolutions.Dtos;
using ProductSolutions.Models;
using ProductSolutions.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text;
using System.Globalization;

namespace ProductSolutions.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _db;

        public ProductController(IProductRepository productRepository, IWebHostEnvironment env, ApplicationDbContext db)
        {
            _productRepository = productRepository;
            _env = env;
            _db = db;
        }
        public async Task<IActionResult> Index(string? q, int page = 1, int pageSize = 10)
        {
            var result = await _productRepository.GetAllProducts(q, page, pageSize);
            return View(result);
        }

        // Export CSV for current query
        public async Task<IActionResult> ExportCsv(string? q)
        {
            var items = await _productRepository.GetProductsForExport(q);

            var sb = new StringBuilder();
            // Header
            sb.AppendLine("Id,Name,Brand,Categories,Price,Description,ImageUrl,CreatedAt");

            string Escape(string? s)
            {
                if (string.IsNullOrEmpty(s)) return string.Empty;
                var escaped = s.Replace("\"", "\"\"");
                if (escaped.Contains(',') || escaped.Contains('"') || escaped.Contains('\n') || escaped.Contains('\r'))
                {
                    return '"' + escaped + '"';
                }
                return escaped;
            }

            foreach (var p in items)
            {
                var line = string.Join(",",
                    p.Id.ToString(CultureInfo.InvariantCulture),
                    Escape(p.Name),
                    Escape(p.Brand),
                    Escape(string.Join(";", p.Categories.Select(c => c.Name))),
                    p.Price.ToString(CultureInfo.InvariantCulture),
                    Escape(p.Description),
                    Escape(p.ImageUrl),
                    p.CreatedAt.ToString("o", CultureInfo.InvariantCulture)
                );
                sb.AppendLine(line);
            }

            var bytes = Encoding.UTF8.GetBytes('\uFEFF' + sb.ToString()); // BOM for Excel
            var fileName = $"product_{DateTime.UtcNow:yyyyMMddHHmmss}.csv";
            return File(bytes, "text/csv", fileName);
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        public IActionResult Create()
        {
            // provide categories for selection
            ViewBag.Categories = new SelectList(_db.Categories.OrderBy(c => c.Name).ToList(), "Id", "Name");
            return View();
        }
        public async Task<IActionResult> CreateData(CreateProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_db.Categories.OrderBy(c => c.Name).ToList(), "Id", "Name", dto.CategoryIds);
                return View(dto);
            }
            var productData = new Product
            {
                Name = dto.Name,
                Brand = dto.Brand,
                Description = dto.Description,
                Price = dto.Price,
                CreatedAt = DateTime.UtcNow
            };

            // assign selected categories
            if (dto.CategoryIds != null && dto.CategoryIds.Any())
            {
                var cats = await _db.Categories.Where(c => dto.CategoryIds.Contains(c.Id)).ToListAsync();
                foreach (var c in cats) productData.Categories.Add(c);
            }

            // Handle uploaded image file
            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                var uploadsRoot = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", "products");
                Directory.CreateDirectory(uploadsRoot);

                var ext = Path.GetExtension(dto.ImageFile.FileName);
                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploadsRoot, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }

                // Set public URL path
                productData.ImageUrl = $"/uploads/products/{fileName}";
            }

            await _productRepository.CreateProduct(productData);
            return RedirectToAction(nameof(Index));
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null) return NotFound();

            var dto = new UpdateProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryIds = product.Categories?.Select(c => c.Id).ToList() ?? new List<int>()
            };

            ViewBag.Categories = new SelectList(_db.Categories.OrderBy(c => c.Name).ToList(), "Id", "Name", dto.CategoryIds);

            return View(dto);
        }

        // POST: Product/EditData
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditData(UpdateProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", dto);
            }
            // Load product with categories using the controller's DbContext to ensure tracked changes
            var existing = await _db.Products.Include(p => p.Categories).FirstOrDefaultAsync(p => p.Id == dto.Id);
            if (existing == null) return NotFound();

            existing.Name = dto.Name;
            existing.Brand = dto.Brand;
            existing.Description = dto.Description;
            existing.Price = dto.Price;

            // Handle image replacement
            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                var uploadsRoot = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", "products");
                Directory.CreateDirectory(uploadsRoot);

                // delete old file if present and under uploads/products
                if (!string.IsNullOrEmpty(existing.ImageUrl) && existing.ImageUrl.StartsWith("/uploads/products/"))
                {
                    var oldFile = existing.ImageUrl.Substring("/uploads/products/".Length);
                    var oldPath = Path.Combine(uploadsRoot, oldFile);
                    if (System.IO.File.Exists(oldPath))
                    {
                        try { System.IO.File.Delete(oldPath); } catch { }
                    }
                }

                var ext = Path.GetExtension(dto.ImageFile.FileName);
                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploadsRoot, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }

                existing.ImageUrl = $"/uploads/products/{fileName}";
            }

            // update categories selection: load selected categories and replace collection
            if (dto.CategoryIds != null)
            {
                var cats = await _db.Categories.Where(c => dto.CategoryIds.Contains(c.Id)).ToListAsync();
                existing.Categories.Clear();
                foreach (var c in cats) existing.Categories.Add(c);
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = dto.Id });
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null) return NotFound();

            // Remove image file if stored under uploads/products
            if (!string.IsNullOrEmpty(product.ImageUrl) && product.ImageUrl.StartsWith("/uploads/products/"))
            {
                var uploadsRoot = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", "products");
                var fileName = product.ImageUrl.Substring("/uploads/products/".Length);
                var filePath = Path.Combine(uploadsRoot, fileName);
                if (System.IO.File.Exists(filePath))
                {
                    try { System.IO.File.Delete(filePath); } catch { }
                }
            }

            var removed = await _productRepository.DeleteProduct(id);
            if (!removed)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
