// Services/IFinanceService.cs
using FinanceTracker.API.Models;

namespace FinanceTracker.API.Services
{
    public interface IFinanceService
    {
        List<Transaction> GetAllTransactions();
        Transaction? GetTransactionById(int id);
        Transaction CreateTransaction(Transaction transaction);
        Transaction? UpdateTransaction(int id, Transaction transaction);
        bool DeleteTransaction(int id);
        List<Category> GetCategories();
        DashboardSummary GetDashboardSummary();
    }
}