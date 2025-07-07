using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Project._BusinessLayer.Interfaces;
using MVC_Project._BusinessLayer.Services;
using MVC_Project._DataLayer.DTOs;
using MVC_Project._DataLayer.Models;
using MVC_Project.ViewModels.Income;

namespace MVC_Project.Controllers
{
    public class IncomeController(IIncomeService incomeService, ICategoryService categoryService) : Controller
    {
        public async Task<IActionResult> Overview(int? categoryId, decimal? minValue, decimal? maxValue, string? description) //asp-action tek Shared/_Layout tek <li>Incomes</li> duhet te jete i njejte me emr i metodes qe te funksionoj
                                        //asp-controller duhet te jete i njejte me emr e controllerit ku ndodhet metoda
        {
            var incomes = await incomeService.GetAllIncomesAsync(categoryId, minValue, maxValue, description);
            var categories = await categoryService.GetAllCategoriesAsync(_DataLayer.Models.Type.Income);

            var data = new IncomeOverviewVM()
            {
                Incomes = incomes?.ToList(),
                CategoryListItems = new SelectList(categories, "Id", "Name"),
                SelectedCategory = categories.SingleOrDefault(x => x.Id == categoryId),
                MinValue = minValue,
                MaxValue = maxValue,
                Description = description
            };

            return View(data);
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            var categories = await categoryService.GetAllCategoriesAsync(_DataLayer.Models.Type.Income, true);
            ViewData["Categories"] = new SelectList(categories, "Id", "Name");

            if (id != null)
            {
                var income = await incomeService.GetIncomeByIdAsync(id.Value);

                return View(income);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpsertForm(IncomeDTO payload)
        {
            if (payload != null)
            {
                if (payload.Id == 0)
                {
                    var createdExpense = await incomeService.CreateIncomeAsync(payload);
                    return RedirectToAction("Overview");
                }
                else
                {
                    var updatedExpense = await incomeService.ModifyIncomeAsync(payload, payload.Id);
                    return RedirectToAction("Overview");
                }

            }

            return BadRequest("Unable to upsert form!");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var income = await incomeService.GetIncomeByIdAsync(id.Value);
                return View(income);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Income payload)
        {
            if(payload != null && payload.Id > 0)
            {
                var deleted = await incomeService.DeleteIncomeAsync(payload.Id);

                if (!deleted) throw new ArgumentException("Unable to delete");

                return RedirectToAction("Overview");
            }

            return View();
        }
    }
}
