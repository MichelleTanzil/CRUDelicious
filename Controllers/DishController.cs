using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using crudelicious.Models;
using System.Linq;
using System.Collections.Generic;
using System;

namespace crudelicious.Controllers
{
    public class DishController : Controller
    {
        private MyContext dbContext;

        // here we can "inject" our context service into the constructor
        public DishController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            List<Dish> AllDishes = dbContext.Dishes
            .OrderByDescending(d => d.CreatedAt)
            .Take(5)
            .ToList();

            return View(AllDishes);
        }

        [HttpGet("new")]
        public IActionResult AddDishView()
        {
            return View();
        }

        [HttpPost("newdish")]
        public IActionResult AddDish(Dish newDish)
        {
            if(ModelState.IsValid)
            {
                dbContext.Dishes.Add(newDish);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else{
                return View("AddDishView");
            }
        }
        [HttpGet]
        [Route("{dishid}")]
        public IActionResult DishView(int dishid)
        {
            Dish oneDish = dbContext.Dishes.Single(d => d.DishId == dishid);
            return View(oneDish);
        }
        [HttpPost("deletedish")]
        public IActionResult DeleteDish(int dishid)
        {
            Dish oneDish = dbContext.Dishes.Single(d => d.DishId == dishid);
            dbContext.Dishes.Remove(oneDish);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet("edit/{dishid}")]
        public IActionResult EditDishView(int dishid)
        {
            Dish oneDish = dbContext.Dishes.Single(d => d.DishId == dishid);
            return View(oneDish);
        }

        [HttpPost("editDish/{dishid}")]
        public IActionResult EditDish(Dish retrivedDish, int dishid)
        {
            if(ModelState.IsValid)
            {
            Dish dishToUpdate = dbContext.Dishes.FirstOrDefault(d => d.DishId == dishid);
            dishToUpdate.Name = retrivedDish.Name;
            dishToUpdate.Chef = retrivedDish.Chef;
            dishToUpdate.Tastiness = retrivedDish.Tastiness;
            dishToUpdate.Calories = retrivedDish.Calories;
            dishToUpdate.Description = retrivedDish.Description;
            dishToUpdate.UpdatedAt = DateTime.Now;

            dbContext.SaveChanges();
            return RedirectToAction("DishView", "Dish", new {dishid});
            }
            else
            {
                return View("EditDishView", retrivedDish);
            }
        }
    }
}