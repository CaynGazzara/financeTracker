import { Transaction } from './transaction';

export interface DashboardSummary {
  totalIncome: number;
  totalExpenses: number;
  balance: number;
  recentTransactions: Transaction[];
}