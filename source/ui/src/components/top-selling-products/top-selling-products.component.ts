import { Component } from '@angular/core';
import Chart from 'chart.js/auto';
import 'chartjs-adapter-moment';

@Component({
  selector: 'app-top-selling-products',
  standalone: true,
  imports: [],
  templateUrl: './top-selling-products.component.html',
  styleUrl: './top-selling-products.component.css'
})
export class TopSellingProductsComponent {
  title = 'ng-chart';
  chart: any;
 
  ngOnInit(): void {
    this.chart = new Chart('top-selling-product-chart', 
    {
      type: 'bar',
      data: {
        labels: [], // Initial labels (timestamps)
        datasets: [{
          label: 'Top selling products',
          data: [], // Initial data points
          borderColor: 'rgba(75, 192, 192, 1)',
          borderWidth: 2,
        }]
      },
    });
  }

  updateTopSellingProductsChart(data: string): void {
    console.log('data received ' + data);
    const jsonArr = JSON.parse(data);
    const names: string[] = [];
    const salesFigures: number[] = [];

    jsonArr.forEach((element: any) => {
      names.push(element.Name);
      salesFigures.push(element.Sales);
    });

    // Replace the existing data with the new data
    this.chart.data.labels = names;
    this.chart.data.datasets[0].data = salesFigures;

    // Update the chart
    this.chart.update();
  }
}
