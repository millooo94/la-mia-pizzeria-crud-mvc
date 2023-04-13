using Microsoft.AspNetCore.Mvc;
using la_mia_pizzeria_static.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static.Controllers
{
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
                .SingleOrDefault(p => p.Id == id);

            if (pizza is null)
            {
                return NotFound($"Pizza with id {id} not found.");
            }

            return View(pizza);
        }
        public IActionResult Create()
        {
            var formModel = new PizzaFormModel()
            {
                Categories = _context.Categories.ToArray(),
            };

            return View(formModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaFormModel form)
        {
            if (!ModelState.IsValid)
            {
                form.Categories = _context.Categories.ToArray();
				return View(form);
            }

			_context.Pizzas.Add(form.Pizza);
			_context.SaveChanges();

			return RedirectToAction("Index");   
        }

        public ActionResult Update(int id)
        {
			var pizza = _context.Pizzas.SingleOrDefault(p => p.Id == id);

			if (pizza is null)
			{
				return NotFound($"Pizza with id {id} not found.");
			}

			var formModel = new PizzaFormModel
			{
				Pizza = pizza,
				Categories = _context.Categories.ToArray()
			};

			return View(formModel);

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Update(int id, PizzaFormModel form)
		{
            if (!ModelState.IsValid)
            {
				form.Categories = _context.Categories.ToArray();
				return View(form);
            }

			//lo faccio per fare savedPizza = form.Pizza;
			//e anche per il controllo di esistenza del record
			var savedPizza = _context.Pizzas.AsNoTracking().FirstOrDefault(p => p.Id == id);

			if (savedPizza is null)
				{
				return NotFound($"Pizza with id {id} not found.");
			}

			//savedPizza.Name = form.Pizza.Name;
			savedPizza = form.Pizza;
			savedPizza.Id = id; // mi serve per fare il tracking del record altrimenti lo faccio su id = 0

			

			_context.Pizzas.Update(savedPizza);

			_context.SaveChanges();
		

			return RedirectToAction("Index");

		}

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
