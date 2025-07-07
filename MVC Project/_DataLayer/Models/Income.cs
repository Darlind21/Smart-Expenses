using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Project._DataLayer.Models
{
    public class Income : BaseModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }
        public string? Description { get; set; }

        [Required]
        public DateOnly IncomeDate { get; set; }


        public Category? Category { get; set; }

        [ForeignKey(nameof(Category))]
        public int? CategoryId { get; set; }
    }
}
