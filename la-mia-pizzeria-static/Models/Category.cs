using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Name must have less than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        public IEnumerable<Pizza>? Pizzas { get; set;}
    }
}
