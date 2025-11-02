// Models/Category.cs
namespace FinanceTracker.API.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = "#000000";
        public TransactionType Type { get; set; }
    }
}