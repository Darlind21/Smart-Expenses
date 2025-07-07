using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Project._BusinessLayer.Interfaces;
using MVC_Project._BusinessLayer.Services;
using MVC_Project._DataLayer.DTOs;
using MVC_Project._DataLayer.Models;
using MVC_Project.ViewModels.Expense;

namespace MVC_Project.Controllers
{
    public class ExpenseController(IExpenseService expenseService, ICategoryService categoryService) : Controller
    {
        public async Task<IActionResult> Overview(int? categoryId, decimal? minValue, decimal? maxValue, string? description)
        {
            var expenses = await expenseService.GetAllExpensesAsync(categoryId, minValue, maxValue, description);
            var categories = await categoryService.GetAllCategoriesAsync(_DataLayer.Models.Type.Expense);

            var data = new ExpenseOverviewVM()
            {
                Expenses = expenses?.ToList(),
                CategoryListItems = new SelectList (categories, "Id", "Name"),
                SelectedCategory = categories.SingleOrDefault(x => x.Id == categoryId),
                MinValue = minValue,
                MaxValue = maxValue,
                Description = description
            };

            return View(data);
        }

        public async Task<IActionResult> Upsert(int? id) //Update or Insert = Upsert
        {
            var categories = await categoryService.GetAllCategoriesAsync(_DataLayer.Models.Type.Expense, true);
            ViewData["Categories"] = new SelectList(categories, "Id", "Name");

            if (id != null)
            {
                var expense = await expenseService.GetExpenseByIdAsync(id.Value);
                
                return View(expense);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpsertForm(ExpenseDTO payload)
        {
            if (payload != null)
            {
                if (payload.Id == 0)
                {
                    var createdExpense = await expenseService.CreateExpenseAsync(payload);
                    return RedirectToAction("Overview");
                }
                else
                {
                    var updatedExpense = await expenseService.ModifyExpenseAsync(payload, payload.Id);
                    return RedirectToAction("Overview");
                }

            }

            return BadRequest("Unable to upsert form!");
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var expense = await expenseService.GetExpenseByIdAsync(id.Value);
                return View(expense);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Expense payload)
        {
            if (payload != null && payload.Id > 0)
            {
                var deleted = await expenseService.DeleteExpenseAsync(payload.Id);

                if (!deleted) throw new ArgumentException("Unable to delete");

                return RedirectToAction("Overview");
            }

            return View();
        }

    }
}
