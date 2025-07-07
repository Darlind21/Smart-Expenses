namespace MVC_Project._DataLayer.Models
{
    public abstract class BaseModel
    {
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedOn { get; set; }
    }
}
