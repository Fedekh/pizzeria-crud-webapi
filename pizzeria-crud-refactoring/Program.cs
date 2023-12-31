using la_mia_pizzeria_crud_mvc.CustomLogger;
using pizzeria_mvc.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using pizzeria_crud_refactoring.Database.Repository;
using pizzeria_crud_refactoring.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PizzaContext>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PizzaContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

//codice per json in modo che ignori le dipendenza cicliche di eventuali relazioni N:N o 1:N
builder.Services.AddControllers().AddJsonOptions(x =>
x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// implementazione Dipendency injection del DB + Custom FileLogger
builder.Services.AddScoped<ICustomLog, CustomFileLogger>();
builder.Services.AddScoped<PizzaContext, PizzaContext>();
builder.Services.AddScoped<IRepositoryPizza, RepositoryPizza>();


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
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pizza}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
