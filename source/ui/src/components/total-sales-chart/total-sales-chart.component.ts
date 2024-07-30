import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import Chart from 'chart.js/auto';
import 'chartjs-adapter-moment';

@Component({
  selector: 'app-total-sales-chart',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './total-sales-chart.component.html',
  styleUrl: './total-sales-chart.component.css'
})
export class TotalSalesChartComponent {
  lastUpdatedInfo: { totalSales: number, utcUpdatedTimestamp: string };
  title = 'ng-chart';
  chart: any;
 
  ngOnInit(): void {
    this.chart = new Chart('total-sales-chart', 
    {
      type: 'line',
      data: {
        labels: [], // Initial labels (timestamps)
        datasets: [{
          label: 'Total sales',
          data: [], // Initial data points
          borderColor: 'rgba(75, 192, 192, 1)',
          borderWidth: 2,
          fill: false
        }]
      },
      options: {
        scales: {
          x: {
            type: 'time',
            time: {
              unit: 'second'
            }
          },
          y: {
            beginAtZero: true
          }
        }
      }
    });
  }

  updateTotalSalesChart(data: string): void {
    const json = JSON.parse(data);
    const activeUsers = json.totalSales;
    const time = json.utcUpdatedTime;

    this.chart.data.labels.push(time);
    this.chart.data.datasets[0].data.push(activeUsers);

    this.chart.update();

    this.lastUpdatedInfo = json;
  }
}
