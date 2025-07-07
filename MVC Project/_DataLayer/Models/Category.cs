using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Project._DataLayer.Models
{
    public class Category : BaseModel
    {
        public int Id { get; set; }

        public Type Type { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(300)]
        public string? Description { get; set; } = null!;

        [Required]
        public bool IsActive { get; set; }



        public List<Income>? Incomes { get; set; }
        public List<Expense>? Expenses { get; set; }
    }

    public enum Type
    {
        [Display(Name = "Income")]
        Income = 1,

        [Display(Name = "Expense")]
        Expense = 2
    }

}
