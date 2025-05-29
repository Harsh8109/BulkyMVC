using Bulky.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Data
{
    //DBContext is a root class of entity framework core using which we can access entity framework
    //Now our ApplicationDBContext now basically implements/inherits from DBContext class which is a built in class inside entity frameowrk core nuget package
    public class ApplicationDBContext : IdentityDbContext<IdentityUser>
    {
        //This is a constructor where we have to pass the connection string, we have connection string in appsettings.json which we need to pass to DBContext
        //When we will inject/configure the ApplicationDBContext we will get that connection string as a parameter in constructor as DBContextOption which we will be passing to the base class
        //Note - when we will register ApplicationDBContext we will be adding some configuration and whatever configuration we add here we want to pass that to DBContext class
        //If we have to pass that in C#, we will write base and we will pass options that way all the options we configure will pass on to our base class which is DBContext
        //Now we have to register our ApplicationDBContext, and we always register anything in Program.cs
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        //When we have to create a Table here, we have to create something called as DB set inside ApplicationDBContext 
        //Here also we can use keywork "prop" and press tab to get a public class
        //Inside DeSet we have to define the entity which is a class, we want to create a category table so we write Category
        //Then my property will be the table name that you want for a table, inside SQL server if you want to call that categories then you can write categories after DeSet<Category> Categories and that will be the table that will be created inside the table
        //The below single line will create a table inside DB and this is the power of Entity Framework core
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        // On the Category list page we want to list all the categories, if we go to database we can edit top 200 rows and add the categories but rather than that Entity Framework Core provides us with some helper functions on if you have to seed some entities in your database
        // Here below the DbSet we will override the default function which is OnModelCreating and that expects some model builder
        // Now OnModelCreating is a default function which is already there in DB Context and we have to override the default behavior so we write the override keyword

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // In order to seed data we will be using model builder and there we have entity and we want to create or work on Category entity
            // We want to add some data inside the Category so we have a method HasData()
            // Inside HasData it expects a Category array an we want to create one object inside it
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }

                // **Must Remember** whenever anything needs to be updated in database we have to add a migration by using = add-migration SeedCategoryTable and after that to reflect the data in DB we will use = update-database
                );

            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1, Name = "Sam Financial", StreetAddress = "83 Park Avenue", City="Tech City", PostalCode="14254", State="IL", PhoneNumber="5567842544" },
                new Company { Id = 2, Name = "Vivid Books", StreetAddress = "99 Vivid St", City = "Vid City", PostalCode = "66542", State = "AT", PhoneNumber = "2221215825" },
                new Company { Id = 3, Name = "Readers Club", StreetAddress = "54 City Street", City = "Arizona", PostalCode = "33215", State = "FS", PhoneNumber = "5524614643" }

                // **Must Remember** whenever anything needs to be updated in database we have to add a migration by using = add-migration SeedCategoryTable and after that to reflect the data in DB we will use = update-database
                );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Fortune of Time",
                    Author = "Billy Spark",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "SWD9999001",
                    ListPrice = 99,
                    Price = 90,
                    Price50 = 85,
                    Price100 = 80,
                    CategoryId = 1, // Only use CategoryId which already exist, in our case we have 1,2,3 which is defined upside in seed category
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 2,
                    Title = "Dark Skies",
                    Author = "Nancy Hoover",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "CAW777777701",
                    ListPrice = 40,
                    Price = 30,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 3,
                    Title = "Vanish in the Sunset",
                    Author = "Julian Button",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "RITO5555501",
                    ListPrice = 55,
                    Price = 50,
                    Price50 = 40,
                    Price100 = 35,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 4,
                    Title = "Cotton Candy",
                    Author = "Abby Muscles",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "WS3333333301",
                    ListPrice = 70,
                    Price = 65,
                    Price50 = 60,
                    Price100 = 55,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 5,
                    Title = "Rock in the Ocean",
                    Author = "Ron Parker",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "SOTJ1111111101",
                    ListPrice = 30,
                    Price = 27,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 6,
                    Title = "Leaves and Wonders",
                    Author = "Laura Phantom",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "FOT000000001",
                    ListPrice = 25,
                    Price = 23,
                    Price50 = 22,
                    Price100 = 20,
                    CategoryId = 3,
                    ImageUrl = ""
                }
                );
        }

    }
}
