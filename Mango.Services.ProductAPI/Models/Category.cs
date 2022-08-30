using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ProductAPI.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        
        [Required]
        public string Name { get; set; }
    }
}
