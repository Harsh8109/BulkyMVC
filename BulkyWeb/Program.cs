using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Whenever we want to register anything we do that in program.cs
//Here we can add our services before the line var app, and we want to tell here that we want to use entity framework.
//after AddDbContext we have to tell that which class has the implementation of DBContext
//We also want to configure some options, so in C# or .Net core when we have to configure options, we can call it with any name like "o=>" or to be specific "options=>" 
// => this sign is for "goes to" and then we can define whatever options we want to configure
//here we want to tell that DB context will be using SQL server, another thing is when we are using SQL server, we have to define connection string inside UseSqlServer which is inbuilt in Microsoft Dot Entity framework core nuget package
//We are using a section whit the name of connection string in appsettings.json which is a build in section
//So if we type builder.Configuration there we have a build in helper method which is GetConnectionString it is a short hand of getting a section with the name connection string

builder.Services.AddDbContext<ApplicationDBContext>(options=> 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Here we have told the .Net application that hey whenever someone ask for an implementation of AppDBContext, then this is the configuration that you have to do and based on that you have to create an object and provide that

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
