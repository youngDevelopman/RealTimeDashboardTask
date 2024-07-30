import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import Chart from 'chart.js/auto';
import 'chartjs-adapter-moment';

@Component({
  selector: 'app-active-users-chart',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './active-users-chart.component.html',
  styleUrl: './active-users-chart.component.css'
})
export class ActiveUsersChartComponent implements OnInit {
  lastUpdatedInfo: { activeUsers: number, utcUpdatedTimestamp: string };
  title = 'ng-chart';
  chart: any;
 
  ngOnInit(): void {
    this.chart = new Chart('canvas', 
    {
      type: 'line',
      data: {
        labels: [], // Initial labels (timestamps)
        datasets: [{
          label: 'Active Users',
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

  updateActiveUsersChart(data: string): void {
    const json = JSON.parse(data);
    const activeUsers = json.activeUsers;
    const time = json.utcUpdatedTime;

    this.chart.data.labels.push(time);
    this.chart.data.datasets[0].data.push(activeUsers);

    this.chart.update();

    this.lastUpdatedInfo = json;
  }
}
