using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC_Project._BusinessLayer.Interfaces;
using MVC_Project._DataLayer.DTOs;
using MVC_Project._DataLayer.Models;
using MVC_Project.ViewModels.Home;

namespace MVC_Project.Controllers
{
    public class HomeController(IExpenseService expenseService, IIncomeService incomeService) : Controller
    {

        public async Task <IActionResult> Index()
        {
            var expensesTotal = await expenseService.GetTotalExpensesAsync(); 
            var incomesTotal = await incomeService.GetTotalIncomesAsync();
            var data = new HomeIndexVM
            {
                ExpensesTotal = expensesTotal,
                IncomesTotal = incomesTotal
            };

            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
