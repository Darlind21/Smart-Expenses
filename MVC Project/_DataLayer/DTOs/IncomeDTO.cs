namespace MVC_Project._DataLayer.DTOs
{
    public class IncomeDTO
    {
        public int Id { get; set; } = 0;
        public string? Name { get; set; }
        public decimal? Value { get; set; }
        public string? Description { get; set; }
        public DateOnly? IncomeDate { get; set; }

        public int CategoryId { get; set; }
    }
}
