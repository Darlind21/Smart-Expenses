using MVC_Project._DataLayer.Models;

namespace MVC_Project._DataLayer.DTOs
{
    public class ExpenseDTO
    {
        public int Id { get; set; } = 0;
        public string? Name { get; set; }
        public decimal? Value { get; set; }
        public string? Description { get; set; }
        public DateOnly? ExpenseDate { get; set; }

        public int CategoryId { get; set; }
    }
}
