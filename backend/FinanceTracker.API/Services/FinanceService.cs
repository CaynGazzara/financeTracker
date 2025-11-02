using FinanceTracker.API.Models;

namespace FinanceTracker.API.Services
{
    public class FinanceService : IFinanceService
    {
        private readonly List<Transaction> _transactions = new();
        private readonly List<Category> _categories = new();
        private int _nextTransactionId = 1;
        private readonly object _lock = new object(); // ✅ Lock para thread safety

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
            lock (_lock)
            {
                _transactions.AddRange(new[]
                {
                    new Transaction { Id = _nextTransactionId++, Description = "Salário", Amount = 3000, Date = DateTime.Now.AddDays(-5), Type = TransactionType.Income, CategoryId = 1 },
                    new Transaction { Id = _nextTransactionId++, Description = "Mercado", Amount = 250, Date = DateTime.Now.AddDays(-3), Type = TransactionType.Expense, CategoryId = 4 },
                    new Transaction { Id = _nextTransactionId++, Description = "Uber", Amount = 35, Date = DateTime.Now.AddDays(-1), Type = TransactionType.Expense, CategoryId = 5 },
                    new Transaction { Id = _nextTransactionId++, Description = "Freelance", Amount = 800, Date = DateTime.Now.AddDays(-2), Type = TransactionType.Income, CategoryId = 3 }
                });
            }
        }

        public List<Transaction> GetAllTransactions()
        {
            lock (_lock)
            {
                return _transactions.OrderByDescending(t => t.Date).Select(t => new Transaction
                {
                    Id = t.Id,
                    Description = t.Description,
                    Amount = t.Amount,
                    Date = t.Date,
                    Type = t.Type,
                    CategoryId = t.CategoryId,
                    Category = _categories.FirstOrDefault(c => c.Id == t.CategoryId)
                }).ToList();
            }
        }

        public Transaction? GetTransactionById(int id)
        {
            lock (_lock)
            {
                var transaction = _transactions.FirstOrDefault(t => t.Id == id);
                if (transaction == null) return null;

                return new Transaction
                {
                    Id = transaction.Id,
                    Description = transaction.Description,
                    Amount = transaction.Amount,
                    Date = transaction.Date,
                    Type = transaction.Type,
                    CategoryId = transaction.CategoryId,
                    Category = _categories.FirstOrDefault(c => c.Id == transaction.CategoryId)
                };
            }
        }

        public Transaction CreateTransaction(Transaction transaction)
        {
            lock (_lock)
            {
                var newTransaction = new Transaction
                {
                    Id = _nextTransactionId++,
                    Description = transaction.Description,
                    Amount = transaction.Amount,
                    Date = transaction.Date,
                    Type = transaction.Type,
                    CategoryId = transaction.CategoryId,
                    Category = _categories.FirstOrDefault(c => c.Id == transaction.CategoryId)
                };

                _transactions.Add(newTransaction);
                return newTransaction;
            }
        }

        public Transaction? UpdateTransaction(int id, Transaction transaction)
        {
            lock (_lock)
            {
                var existingTransactionIndex = _transactions.FindIndex(t => t.Id == id);
                if (existingTransactionIndex == -1) return null;

                var updatedTransaction = new Transaction
                {
                    Id = id,
                    Description = transaction.Description,
                    Amount = transaction.Amount,
                    Date = transaction.Date,
                    Type = transaction.Type,
                    CategoryId = transaction.CategoryId,
                    Category = _categories.FirstOrDefault(c => c.Id == transaction.CategoryId)
                };

                _transactions[existingTransactionIndex] = updatedTransaction;
                return updatedTransaction;
            }
        }

        public bool DeleteTransaction(int id)
        {
            lock (_lock)
            {
                var transactionIndex = _transactions.FindIndex(t => t.Id == id);
                if (transactionIndex == -1) return false;

                _transactions.RemoveAt(transactionIndex);
                return true;
            }
        }

        public List<Category> GetCategories()
        {
            return _categories.ToList();
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