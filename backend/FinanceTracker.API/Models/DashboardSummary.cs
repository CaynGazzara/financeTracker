// Models/DashboardSummary.cs
namespace FinanceTracker.API.Models
{
    public class DashboardSummary
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal Balance => TotalIncome - TotalExpenses;
        public List<Transaction> RecentTransactions { get; set; } = new();
    }
}