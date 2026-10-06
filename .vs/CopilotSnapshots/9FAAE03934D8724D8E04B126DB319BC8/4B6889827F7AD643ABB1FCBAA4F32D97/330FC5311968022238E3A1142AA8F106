using ProductSolutions.Models;

namespace ProductSolutions.Repository
{
    public interface IProductRepository
    {
        public Task<PagedResult<Product>> GetAllProducts(string? query = null, int page = 1, int pageSize = 10);
        public Task<Product?> GetProductById(int id);
        public Task<Product> CreateProduct(Product product);
        public Task<Product?> UpdateProduct(int id, Product product);
        public Task<bool> DeleteProduct(int id);
        // Return all matching products for exports (no paging)
        public Task<List<Product>> GetProductsForExport(string? query = null);
    }
}
