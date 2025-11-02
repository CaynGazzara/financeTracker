// Services/FinanceService.cs
using FinanceTracker.API.Models;

namespace FinanceTracker.API.Services
{
    public class FinanceService : IFinanceService
    {
        private readonly List<Transaction> _transactions = new();
        private readonly List<Category> _categories = new();
        private int _nextTransactionId = 1;

        public FinanceService()
        {
            InitializeCategories();
            SeedSampleData();
        }

        private void InitializeCategories()
        {
            _categories.AddRange(new[]
            {
                new Category { Id = 1, Name = "Salário", Type = TransactionType.Income, Color = "#10B981" },
                new Category { Id = 2, Name = "Investimentos", Type = TransactionType.Income, Color = "#059669" },
                new Category { Id = 3, Name = "Freelance", Type = TransactionType.Income, Color = "#047857" },
                new Category { Id = 4, Name = "Alimentação", Type = TransactionType.Expense, Color = "#EF4444" },
                new Category { Id = 5, Name = "Transporte", Type = TransactionType.Expense, Color = "#F59E0B" },
                new Category { Id = 6, Name = "Moradia", Type = TransactionType.Expense, Color = "#8B5CF6" },
                new Category { Id = 7, Name = "Lazer", Type = TransactionType.Expense, Color = "#EC4899" },
                new Category { Id = 8, Name = "Saúde", Type = TransactionType.Expense, Color = "#06B6D4" }
            });
        }

        private void SeedSampleData()
        {
            _transactions.AddRange(new[]
            {
                new Transaction { Id = _nextTransactionId++, Description = "Salário", Amount = 3000, Date = DateTime.Now.AddDays(-5), Type = TransactionType.Income, CategoryId = 1 },
                new Transaction { Id = _nextTransactionId++, Description = "Mercado", Amount = 250, Date = DateTime.Now.AddDays(-3), Type = TransactionType.Expense, CategoryId = 4 },
                new Transaction { Id = _nextTransactionId++, Description = "Uber", Amount = 35, Date = DateTime.Now.AddDays(-1), Type = TransactionType.Expense, CategoryId = 5 },
                new Transaction { Id = _nextTransactionId++, Description = "Freelance", Amount = 800, Date = DateTime.Now.AddDays(-2), Type = TransactionType.Income, CategoryId = 3 }
            });
        }

        public List<Transaction> GetAllTransactions()
        {
            return _transactions.OrderByDescending(t => t.Date).ToList();
        }

        public Transaction? GetTransactionById(int id)
        {
            return _transactions.FirstOrDefault(t => t.Id == id);
        }

        public Transaction CreateTransaction(Transaction transaction)
        {
            transaction.Id = _nextTransactionId++;
            transaction.Category = _categories.FirstOrDefault(c => c.Id == transaction.CategoryId);
            _transactions.Add(transaction);
            return transaction;
        }

        public Transaction? UpdateTransaction(int id, Transaction transaction)
        {
            var existingTransaction = GetTransactionById(id);
            if (existingTransaction == null) return null;

            existingTransaction.Description = transaction.Description;
            existingTransaction.Amount = transaction.Amount;
            existingTransaction.Date = transaction.Date;
            existingTransaction.Type = transaction.Type;
            existingTransaction.CategoryId = transaction.CategoryId;
            existingTransaction.Category = _categories.FirstOrDefault(c => c.Id == transaction.CategoryId);

            return existingTransaction;
        }

        public bool DeleteTransaction(int id)
        {
            var transaction = GetTransactionById(id);
            if (transaction == null) return false;

            return _transactions.Remove(transaction);
        }

        public List<Category> GetCategories()
        {
            return _categories;
        }

        public DashboardSummary GetDashboardSummary()
        {
            var transactions = GetAllTransactions();
            var totalIncome = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            var totalExpenses = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
            var recentTransactions = transactions.Take(5).ToList();

            return new DashboardSummary
            {
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                RecentTransactions = recentTransactions
            };
        }
    }
}