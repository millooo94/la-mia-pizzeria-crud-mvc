using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;

namespace la_mia_pizzeria_static.Models
{
    public class PizzaFormModel
    {
        public Pizza Pizza { get; set; } = new Pizza { Img = "https://picsum.photos/200/300" };
        public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();

		public IEnumerable<SelectListItem> Ingredients { get; set; } = Enumerable.Empty<SelectListItem>();
		public List<string> SelectedIngredients { get; set; } = new();
	}
}
