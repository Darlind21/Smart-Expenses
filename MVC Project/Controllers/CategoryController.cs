using Microsoft.AspNetCore.Mvc;
using MVC_Project._BusinessLayer.Interfaces;
using MVC_Project._BusinessLayer.Services;
using MVC_Project._DataLayer.DTOs;
using MVC_Project._DataLayer.Models;
using MVC_Project._DataLayer.Models.Enums;

namespace MVC_Project.Controllers
{
    public class CategoryController(ICategoryService categoryService) : Controller
    {
        public async Task<IActionResult> Overview()
        {
            var category = await categoryService.GetAllCategoriesAsync();
            return View(category);
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            if (id != null)
            {
                var category = await categoryService.GetCategoryByIdAsync(id.Value);
                return View(category);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpsertForm(Category payload)
        {
            if (payload != null)
            {
                if (payload.Id == 0)
                {
                    var createdCategory = await categoryService.CreateCategoryAsync(payload.Name, payload.Type, payload.Description);
                    return RedirectToAction("Overview");
                }
                else
                {
                    var createdCategory = await categoryService.ModifyCategoryAsync(payload);
                    return RedirectToAction("Overview");
                }

            }

            return BadRequest("Unable to upsert form!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var income = await categoryService.GetCategoryByIdAsync(id.Value);
                return View(income);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Income payload)
        {
            if (payload != null && payload.Id > 0)
            {
                var deleted = await categoryService.DeleteCategoryAsync(payload.Id);

                if (!deleted) throw new ArgumentException("Unable to delete");

                return RedirectToAction("Overview");
            }

            return View();
        }
    }
}
