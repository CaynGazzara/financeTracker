// components/balance-chart/balance-chart.component.ts
import { Component, Input, OnInit } from '@angular/core';
import { ChartConfiguration } from 'chart.js';

@Component({ 
  standalone: false,
  selector: 'app-balance-chart',
  templateUrl: './balance-chart.component.html',
  styleUrls: ['./balance-chart.component.css']
})
export class BalanceChartComponent implements OnInit {
  @Input() income: number = 0;
  @Input() expenses: number = 0;

  public barChartLegend = true;
  public barChartPlugins = [];

  public barChartData: ChartConfiguration<'bar'>['data'] = {
    labels: ['Receitas vs Despesas'],
    datasets: [
      {
        data: [0],
        label: 'Receitas',
        backgroundColor: '#10B981'
      },
      {
        data: [0],
        label: 'Despesas',
        backgroundColor: '#EF4444'
      }
    ]
  };

public barChartOptions: ChartConfiguration<'bar'>['options'] = {
  responsive: true,
  plugins: {
    legend: {
      display: true,
    }
  },
  scales: {
    x: {
      display: true,
    },
    y: {
      display: true,
      beginAtZero: true
    }
  }
};

  ngOnInit(): void {
    this.updateChartData();
  }

  ngOnChanges(): void {
    this.updateChartData();
  }

  private updateChartData(): void {
    this.barChartData = {
      labels: ['Resumo Financeiro'],
      datasets: [
        {
          data: [this.income],
          label: 'Receitas',
          backgroundColor: '#10B981'
        },
        {
          data: [this.expenses],
          label: 'Despesas',
          backgroundColor: '#EF4444'
        }
      ]
    };
  }
}