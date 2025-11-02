import { Component, OnInit } from '@angular/core';
import { FinanceService } from '../../shared/services/finance.service';
import { DashboardSummary } from '../../shared/models/dashboard-summary';
import { TransactionType } from '../../shared/models/transaction';


@Component({
  standalone: false,
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  summary!: DashboardSummary;
  isLoading = true;

  // ✅ Expor o enum para o template
  TransactionType = TransactionType;

  constructor(private financeService: FinanceService) { }

  ngOnInit(): void {
    this.loadDashboardSummary();
  }

  loadDashboardSummary(): void {
    this.financeService.getDashboardSummary().subscribe({
      next: (data) => {
        this.summary = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading dashboard:', error);
        this.isLoading = false;
      }
    });
  }
}