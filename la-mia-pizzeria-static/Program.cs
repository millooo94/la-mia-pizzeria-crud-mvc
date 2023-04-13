using la_mia_pizzeria_static.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSqlServer<PizzaContext>("Data Source=localhost;Initial Catalog=PizzasDb;Integrated Security=True; Encrypt=False");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pizza}/{action=Index}/{id?}");


using (var scope = app.Services.CreateScope())
using (var ctx = scope.ServiceProvider.GetService<PizzaContext>())
{
	ctx!.Seed();
}

app.Run();
