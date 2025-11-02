import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FinanceService } from '../../shared/services/finance.service';
import { Transaction, TransactionType } from '../../shared/models/transaction';
import { Category } from '../../shared/models/category';

@Component({
  standalone: false,
  selector: 'app-transaction-list',
  templateUrl: './transaction-list.component.html',
  styleUrls: ['./transaction-list.component.css']
})
export class TransactionListComponent implements OnInit {
  transactions: Transaction[] = [];
  categories: Category[] = [];
  displayedColumns: string[] = ['description', 'amount', 'category', 'date', 'actions'];

  constructor(
    private financeService: FinanceService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadTransactions();
    this.loadCategories();
  }

  loadTransactions(): void {
    this.financeService.getTransactions().subscribe({
      next: (data) => {
        this.transactions = data;
        console.log('Transactions loaded:', data); // ✅ Debug
      },
      error: (error) => {
        console.error('Error loading transactions:', error);
        alert('Erro ao carregar transações. Verifique se o backend está rodando.');
      }
    });
  }

  loadCategories(): void {
    this.financeService.getCategories().subscribe({
      next: (data) => {
        this.categories = data;
      },
      error: (error) => {
        console.error('Error loading categories:', error);
      }
    });
  }

  getCategoryName(categoryId: number): string {
    const category = this.categories.find(c => c.id === categoryId);
    return category ? category.name : 'N/A';
  }

  getCategoryColor(categoryId: number): string {
    const category = this.categories.find(c => c.id === categoryId);
    return category ? category.color : '#000000';
  }

  editTransaction(id: number): void {
    console.log('Editing transaction:', id); // ✅ Debug
    this.router.navigate(['/transactions/edit', id]);
  }

  deleteTransaction(id: number): void {
    console.log('Deleting transaction:', id); // ✅ Debug
    if (confirm('Tem certeza que deseja excluir esta transação?')) {
      this.financeService.deleteTransaction(id).subscribe({
        next: () => {
          console.log('Transaction deleted successfully'); // ✅ Debug
          this.loadTransactions(); // Recarrega a lista
        },
        error: (error) => {
          console.error('Error deleting transaction:', error);
          alert('Erro ao excluir transação. Verifique o console.');
        }
      });
    }
  }

  addTransaction(): void {
    this.router.navigate(['/transactions/new']);
  }
}