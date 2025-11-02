import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FinanceService } from '../../shared/services/finance.service';
import { Transaction, TransactionType } from '../../shared/models/transaction';
import { Category } from '../../shared/models/category';

@Component({
  standalone: false,
  selector: 'app-transaction-form',
  templateUrl: './transaction-form.component.html',
  styleUrls: ['./transaction-form.component.css']
})
export class TransactionFormComponent implements OnInit {
  transactionForm: FormGroup;
  categories: Category[] = [];
  filteredCategories: Category[] = [];
  
  // ✅ Usar números diretamente
  transactionTypes = [
    { value: 0, label: 'Receita' },
    { value: 1, label: 'Despesa' }
  ];
  
  isEditMode = false;
  transactionId?: number;
  isLoading = true;

  constructor(
    private fb: FormBuilder,
    private financeService: FinanceService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.transactionForm = this.createForm();
  }

  ngOnInit(): void {
    this.loadCategories();
    this.checkEditMode();
    
    this.transactionForm.get('type')?.valueChanges.subscribe((type: number) => {
      this.updateFilteredCategories(type);
    });
  }

  createForm(): FormGroup {
    return this.fb.group({
      description: ['', [Validators.required, Validators.minLength(3)]],
      amount: ['', [Validators.required, Validators.min(0.01)]],
      date: [new Date(), Validators.required],
      type: [1, Validators.required], // ✅ Número
      categoryId: ['', Validators.required]
    });
  }

  loadCategories(): void {
    this.financeService.getCategories().subscribe({
      next: (data) => {
        this.categories = data;
        const initialType = this.transactionForm.get('type')?.value;
        this.updateFilteredCategories(initialType);
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading categories:', error);
        this.isLoading = false;
      }
    });
  }

  updateFilteredCategories(type: number): void {
    // ✅ Agora ambos são numbers, então a comparação funciona
    this.filteredCategories = this.categories.filter(category => category.type === type);
    
    const currentCategoryId = this.transactionForm.get('categoryId')?.value;
    const currentCategory = this.categories.find(c => c.id === currentCategoryId);
    
    if (currentCategory && currentCategory.type !== type) {
      this.transactionForm.patchValue({ categoryId: '' });
    }
  }

  checkEditMode(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.transactionId = +id;
      this.loadTransaction(this.transactionId);
    }
  }

  loadTransaction(id: number): void {
    this.financeService.getTransaction(id).subscribe({
      next: (transaction) => {
        console.log('Loaded transaction for edit:', transaction);
        
        this.transactionForm.patchValue({
          description: transaction.description,
          amount: transaction.amount,
          date: transaction.date,
          type: transaction.type, // ✅ Já é number
          categoryId: transaction.categoryId
        });
      },
      error: (error) => {
        console.error('Error loading transaction:', error);
        this.router.navigate(['/transactions']);
      }
    });
  }

  onSubmit(): void {
    if (this.transactionForm.valid) {
      const formValue = this.transactionForm.value;
      
      const transaction: Transaction = {
        id: this.transactionId || 0,
        description: formValue.description,
        amount: parseFloat(formValue.amount),
        date: new Date(formValue.date).toISOString(),
        type: parseInt(formValue.type), // ✅ Number
        categoryId: parseInt(formValue.categoryId) // ✅ Number
      };

      console.log('Sending transaction to backend:', transaction);

      if (this.isEditMode && this.transactionId) {
        this.financeService.updateTransaction(this.transactionId, transaction).subscribe({
          next: () => {
            console.log('Transaction updated successfully');
            this.router.navigate(['/transactions']);
          },
          error: (error) => {
            console.error('Error updating transaction:', error);
            console.error('Error details:', error.error);
            alert('Erro ao atualizar transação. Verifique o console.');
          }
        });
      } else {
        this.financeService.createTransaction(transaction).subscribe({
          next: () => {
            console.log('Transaction created successfully');
            this.router.navigate(['/transactions']);
          },
          error: (error) => {
            console.error('Error creating transaction:', error);
            console.error('Error details:', error.error);
            alert('Erro ao criar transação. Verifique o console.');
          }
        });
      }
    } else {
      console.log('Form invalid:', this.transactionForm.errors);
      console.log('Form values:', this.transactionForm.value);
    }
  }

  onCancel(): void {
    this.router.navigate(['/transactions']);
  }
}