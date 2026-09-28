using System.ComponentModel.DataAnnotations;

namespace ProductSolutions.Dtos
{
    public class UpdateProductDto
    {
        public int Id { get; set; }

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

        [DataType(DataType.Upload)]
        public Microsoft.AspNetCore.Http.IFormFile? ImageFile { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
    }
}
