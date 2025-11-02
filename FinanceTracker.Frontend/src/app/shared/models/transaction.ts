import { Category } from "./category";

export interface Transaction {
  id: number;
  description: string;
  amount: number;
  date: string;
  type: number;
  categoryId: number;
  category?: Category;
}

export enum TransactionType {
  Income = 0,
  Expense = 1
}