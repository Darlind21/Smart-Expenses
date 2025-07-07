using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Project._DataLayer.Models;

namespace MVC_Project.ViewModels.Income
{
    public class IncomeOverviewVM
    {
        public List<_DataLayer.Models.Income>? Incomes { get; set; }
        public IEnumerable<SelectListItem>? CategoryListItems { get; set; }

        public Category? SelectedCategory { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
        public string? Description { get; set; }
    }
}
