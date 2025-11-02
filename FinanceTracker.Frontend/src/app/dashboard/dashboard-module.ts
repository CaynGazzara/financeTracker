import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatListModule } from '@angular/material/list';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { DashboardComponent } from './dashboard/dashboard.component';
import { BalanceChartComponent } from '../components/balance-chart/balance-chart.component';
import { BaseChartDirective } from 'ng2-charts';  

@NgModule({
  declarations: [
    DashboardComponent,
    BalanceChartComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: '', component: DashboardComponent }]),
    MatCardModule,
    MatGridListModule,
    MatListModule,
    MatProgressSpinnerModule,
    BaseChartDirective 
  ]
})
export class DashboardModule { }