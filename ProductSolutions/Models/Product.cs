using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProductSolutions.Models
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = "";
        [MaxLength(100)]
        public string Brand { get; set; } = "";
        // Categories - many-to-many relationship
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        [Precision(18, 2)]
        public decimal Price { get; set; }
        [MaxLength(250)]
        public string Description { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}
