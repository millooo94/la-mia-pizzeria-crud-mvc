using Microsoft.AspNetCore.Mvc;
using la_mia_pizzeria_static.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace la_mia_pizzeria_static.Controllers
{
	[Authorize(Roles = "Admin, User")]
	public class PizzaController : Controller
    {
		private readonly ILogger<PizzaController> _logger;
		private readonly PizzaContext _context;


		public PizzaController(ILogger<PizzaController> logger, PizzaContext context)
		{
			_logger = logger;
			_context = context;
			System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
			customCulture.NumberFormat.NumberDecimalSeparator = ".";
			System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
		}

		public IActionResult Index()
        {
            var pizzas = _context.Pizzas
                .Include(p=>p.Category)
                .ToArray();

            return View(pizzas);

        }

		public IActionResult Privacy()
        {
            return View();
        }

		public IActionResult Detail(int id)
        {
            var pizza = _context.Pizzas
                .Include (p => p.Category)
				.Include(p => p.Ingredients)
				.SingleOrDefault(p => p.Id == id);

            if (pizza is null)
            {
                return NotFound($"Pizza with id {id} not found.");
            }

            return View(pizza);
        }

		[Authorize(Roles = "Admin")]
		public IActionResult Create()
        {
            var formModel = new PizzaFormModel()
            {
                Categories = _context.Categories.ToArray(),
				Ingredients = _context.Ingredients.Select(i => new SelectListItem(i.Name, i.Id.ToString())).ToArray()
			};

            return View(formModel);
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaFormModel form)
        {
            if (!ModelState.IsValid)
            {
                form.Categories = _context.Categories.ToArray();
				form.Ingredients = _context.Ingredients.Select(i => new SelectListItem(i.Name, i.Id.ToString())).ToArray();

				return View(form);
            }

			form.Pizza.Ingredients = form.SelectedIngredients.Select(si => _context.Ingredients.First(i => i.Id == Convert.ToInt32(si))).ToList();

			_context.Pizzas.Add(form.Pizza);
			_context.SaveChanges();

			return RedirectToAction("Index");   
        }

		[Authorize(Roles = "Admin")]
		public ActionResult Update(int id)
        {
			var pizza = _context.Pizzas.Include(p => p.Ingredients).SingleOrDefault(p => p.Id == id);

			if (pizza is null)
			{
				return NotFound($"Pizza with id {id} not found.");
			}

			var formModel = new PizzaFormModel
			{
				Pizza = pizza,
				Categories = _context.Categories.ToArray(),
				Ingredients = _context.Ingredients.ToArray().Select(i => new SelectListItem(
					i.Name,
					i.Id.ToString(),
					pizza.Ingredients!.Any(_i => _i.Id == i.Id))
				).ToArray()
			};

			formModel.SelectedIngredients = formModel.Ingredients.Where(i => i.Selected).Select(i => i.Value).ToList();

			return View(formModel);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Update(int id, PizzaFormModel form)
		{
			if (!ModelState.IsValid)
			{
				form.Categories = _context.Categories.ToArray();
				form.Ingredients = _context.Ingredients.Select(i => new SelectListItem(i.Name, i.Id.ToString())).ToArray();

				return View(form);
			}

			var savedPizza = _context.Pizzas.Include(p => p.Ingredients).FirstOrDefault(i => i.Id == id);

			if (savedPizza is null)
			{
				return NotFound($"Pizza with id {id} not found.");
			}

			savedPizza.Name = form.Pizza.Name;
			savedPizza.Description = form.Pizza.Description;
			savedPizza.Img = form.Pizza.Img;
			savedPizza.CategoryId = form.Pizza.CategoryId;
			savedPizza.Ingredients = form.SelectedIngredients.Select(si => _context.Ingredients.First(i => i.Id == Convert.ToInt32(si))).ToList();

			_context.SaveChanges();

			return RedirectToAction("Index");

		}

		[Authorize(Roles = "ADMIN")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(int id)
		{
			var pizzaToDelete = _context.Pizzas.FirstOrDefault(p => p.Id == id);

			if (pizzaToDelete is null)
			{
				return NotFound($"Pizza with id {id} not found.");
			}

			_context.Pizzas.Remove(pizzaToDelete);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}


	}

}
