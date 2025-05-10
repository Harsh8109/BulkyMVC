using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //Whenever we add a controller, it must have Controller as name at the end which in this case is CategoryController.cs. That way the program will know that this is a controller.
    public class CategoryController : Controller
    {
        // Now that we have 3 records in categories table how can we retrieve and display them in index view for our category
        // In order to pass that to the view we have to retrieve that to our controller because whatever we pass in View() will be used and display in UI
        // Whenever we have to work on **retrieve, add or update data** we have to work on ApplicationDBContext
        // Since we are working with .Net Core, here what happens is when we add something to the services container in Program.cs that way we are adding to the dependency injection
        // This way we do not have to work with creating any objects, what we can do is we can directly tell our app that hey give me an implementation of ApplicationDBContext, I know you have that because I registered that inside the services
        // We will get the implementation of AppDBContext inside the constructor so I will write (ctor + enter)
        // Also we will create a private readonly field of ApplicationDBContext and we will call it as _db
        // We are creating this local variable because we want to use it inside any other action method

        //private readonly ApplicationDBContext _db;
        // private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork _unitOfWork;

        //public CategoryController(ApplicationDBContext db)
        //public CategoryController(ICategoryRepository db)
        //This is dependency injection, we are injecting the implementation of ICategoryRepository inside the constructor and we are assigning it to our local variable
        public CategoryController(IUnitOfWork unitOfWork)
        {
            // Whatever implementation we get inside the constructor, we will assign it to our local variable
            _unitOfWork = unitOfWork;

            // This way we can use it to any other action method
        }
        // Next we want to do is in the index here we want to retrieve all the categories, and we do not have to use any SQL for that, using entity framework we acn retrieve all of that
        public IActionResult Index()
        {
            // Here we will call this as var objCategoryList = _db.Categories.ToList(); so in _db. here we can access all the db sets that we added, but this time we want to retrieve the Categories and there we have a mentod .ToList that will convert to a list and assign it to objCategoryList
            // Also we can use List<Category> to explicit that the list of categories will be retrieved from this command if you don't wanna use var
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();

            // Now we have to pass the objCategoryList to the view and and then in the view we have to fetch that and extract and display all the categories, so in the index.cshtml will create a table
            // So if we change anything in the controller we will have to rebuild the project but if our views are updated and we do something there i.e., in html css or JavaScript it should automatically reflect those changes and we do not have to re-lode our application if the hot relode on file save is enabled
            // To pass the categories in the view we simply going to insert objCategoryList in View()
            // Now we have to capture of retrive that in the view, so in the Index.cshtml we will write @model in lowercase bcz when we have to access model it will be different but when we are retrieving or defining the model at the top, everything should be in lowercase
            // after that we have to define what will be the type of this model in index.html which is List<Category> so, once we define it, we have to 
            return View(objCategoryList);
        }

        // Now we are creating a new Category inside a new view but to do that we need to create a new action method here which will be invoked when we call it and it will call the view. We will also create a viw=ew after this.
        // IActionResult is return type and Create() is the name of our action method
        // We can navigate to the View->Category and create a view or we can just right click on Create() and give Add View, we can add and empty razor view and it's name should be similar as our action method
        public IActionResult Create()
        {
            // for now all this will do is return the view
            return View();
        }
        // In the create category form, when we hit the button it will hit the same endpoint as post request for which we have to create another action method over here which will be of the same name as Create category
        // But this will be of the type HTTPPOST, so whenever any category is been posted this endpoint will invoked and there we have to add the category
        // Also when we are posting something, in the Create() we will be getting the Category object bcz in the Create.cshtml if we examine we have the Category method and in form we have those properties
        // So when the value is posted it will provide a form in the Category Controller and let us call it as obj, and once we have that object which will have the value of posted category which needs to be added
        [HttpPost]
        public IActionResult Create(Category obj)
        {

            //If we want to add a validation that name and display order cannot be same, we would have to add a check
            // But we already have custom validation so we don't need this
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }
            //if (obj.Name != null && obj.Name.ToLower() == "test")
            //{
            //    ModelState.AddModelError("", "Test is an invalid value.");
            //}

            //Here we will create a category just like we are retrieving it above. Add is the method provided by entity framework core which will help us adding the category in the db, inside it we have to pass an object which is of type Category
            //But if we just write _db.Categories.Add() we are just telling that these are the Categories we would like to add in the db, it won't be added until we save it to DB.
            //At some point we will be adding more than one category but we do not want to add it one by one bcz that way we will be going to the db every time
            //So, Add method will keep track of what are all the changes i have to do in the db

            //This if condition is for validation, if all conditions meets then only the category will be added, for more info about validation, go to Category.cs
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();

                // TempData is a dictionary which is used to store data for the duration of an HTTP Request (TempData keep the information for the time of an HTTP Request)
                // It stays for the time of the request and then if we referesh the page, it goes away
                TempData["success"] = "Category created successfully";

                //once the category is added, we want to redirect to the category index view where they can see all the categories, now we cannot go to the view but we can go to the Inx=dex action, there it will re-lode the categories bcz when a category is added we have to re-lode and pass that to the view
                //So, rather then returning to the view, we have something called as RedirectToAction(), if you are in the same controller you can only write the action name inside the RedirectToAction() which is Index and that will work, but if you have to go to a diff controller, you can write the controller name here as well, just like RedirectToAction("Index", "Category") to be explicite
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Find only works on primary key which is Id in this case
            // ? means nullable, so if the category is not found it will return null
            // FirstOrDefault will return the first category that matches the id or null if not found, it works on primary key and also on other fields
            Category? categoryFromDB = _unitOfWork.Category.Get(u => u.Id == id);
            // Category? categoryFromDB1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            // if we need to do some complex query or calculation, we can use where
            // Category? categoryFromDB2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            if (categoryFromDB == null)
            {
                return NotFound();
            }
            return View(categoryFromDB);
        }
        // here we are posting the edited category and retriving the category object 'obj' which is posted and then we are updating the category in the db
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            //if (obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            //}

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        // this is a get request to delete the category
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDB = _unitOfWork.Category.Get(u => u.Id == id);

            if (categoryFromDB == null)
            {
                return NotFound();
            }
            return View(categoryFromDB);
        }

        // here we are explicitly mentioning that this is a post request and we are deleting the category from the db
        // we are using the same id which is passed in the delete action method and if we use same name as get request, we have to use ActionName attribute
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");

        }
    }
}


