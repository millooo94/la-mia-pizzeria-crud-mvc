using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace la_mia_pizzeria_static.Models
{
    public class PizzaContext : IdentityDbContext<IdentityUser>
    {
		public PizzaContext(DbContextOptions<PizzaContext> options) : base(options) { }
		public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Category> Categories { get; set; }
		public DbSet<Ingredient> Ingredients { get; set; }

		public void Seed()
        {
            var pizzaSeed = new Pizza[]
            {
                new Pizza
                {
                    Img = "/img/margherita.jpg",
                    Name = "Margherita",
                    Description = "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Mollitia quis est adipisci incidunt rem nostrum ipsam fuga ratione tempora eveniet!",
                    Price = 5
                },
                new Pizza
                {
                    Img =  "/img/brontese.png",
                    Name = "Brontese",
                    Description = "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Mollitia quis est adipisci incidunt rem nostrum ipsam fuga ratione tempora eveniet!",
                    Price = 13
                },
                new Pizza
                {
                    Img = "/img/pizza-fritta.jpg",
                    Name = "Pizza fritta",
                    Description = "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Mollitia quis est adipisci incidunt rem nostrum ipsam fuga ratione tempora eveniet!",
                    Price = 7
                }
            };

            if (!Pizzas.Any())
            {
                Pizzas.AddRange(pizzaSeed);
            }

            if (!Categories.Any())
            {
                var categorySeed = new Category[]
                {
                    new Category
                    {
                        Name = "Pizze classiche",
                        Pizzas = pizzaSeed
                    },
                    new Category
                    {
                        Name = "Pizze bianche",
                    },
                    new Category
                    {
                        Name = "Pizze vegetariane",
                    },
                    new Category
                    {
                        Name = "Pizze di mare",
                    }
                };

                Categories.AddRange(categorySeed);
            }

			if (!Ingredients.Any())
			{
				var seed = new Ingredient[]
				{
					new()
					{
						Name = "Tomato"
					},
					new()
					{
						Name = "Mozzarella"
					},
					new()
					{
						Name = "EVO oil",
						Pizzas = pizzaSeed
					}
				};

				Ingredients.AddRange(seed);
			}

			if (!Roles.Any())
			{
				var seed = new IdentityRole[]
				{
					new("Admin"),
					new("User")
				};

				Roles.AddRange(seed);
			}

			if (Users.Any(u => u.Email == "admin@dev.com" || u.Email == "user@dev.com")
				&& !UserRoles.Any())
			{
				var admin = Users.First(u => u.Email == "admin@dev.com");
				var user = Users.First(u => u.Email == "user@dev.com");

				var adminRole = Roles.First(r => r.Name == "Admin");
				var userRole = Roles.First(r => r.Name == "User");

				var seed = new IdentityUserRole<string>[]
				{
					new()
					{
						UserId = admin.Id,
						RoleId = adminRole.Id
					},
					new()
					{
						UserId = user.Id,
						RoleId = userRole.Id
					}
				};

				UserRoles.AddRange(seed);
			}

			SaveChanges();
        }
    }
}
