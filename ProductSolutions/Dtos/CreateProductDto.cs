using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ProductSolutions.Dtos
{
    public class CreateProductDto
    {
        [Required, MinLength(2), MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required, MinLength(2), MaxLength(100)]
        public string Brand { get; set; } = string.Empty;
        [Required, MinLength(2), MaxLength(100)]
        public string Category { get; set; } = string.Empty;
        [Required, MinLength(2), MaxLength(250)]
        public string Description { get; set; } = string.Empty;
        [Required, Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        // Optional image upload. If provided, the controller will save the file and set ImageUrl on the product.
        [DataType(DataType.Upload)]
        public IFormFile? ImageFile { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
