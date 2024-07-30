import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { webSocketService } from '../../services/web-socket-service';
import { CommonModule } from '@angular/common';
import { ActiveUsersChartComponent } from '../active-users-chart/active-users-chart.component';
import { TotalSalesChartComponent } from "../total-sales-chart/total-sales-chart.component";

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, ActiveUsersChartComponent, TotalSalesChartComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit, OnDestroy {
  totalSales: string;
  topSellingProducts: any;

  private activeUsersSubscription: Subscription;
  private totalSalesSubscription: Subscription;
  private topSellingProductsSubscription: Subscription;

  @ViewChild(ActiveUsersChartComponent) chartComponent: ActiveUsersChartComponent;
  @ViewChild(TotalSalesChartComponent) totalSalesChartComponent: TotalSalesChartComponent;

  constructor(private wsService: webSocketService) {}

  ngOnInit(): void {
    this.activeUsersSubscription = this.wsService.getActiveUsers().subscribe(
      data => { this.chartComponent.updateActiveUsersChart(data) },
      err => console.error(err)
    );
    this.totalSalesSubscription = this.wsService.getTotalSales().subscribe(
      data => this.totalSalesChartComponent.updateTotalSalesChart(data),
      err => console.error(err)
    );
    this.topSellingProductsSubscription = this.wsService.getTopSellingProducts().subscribe(
      data => this.topSellingProducts = JSON.parse(data),
      err => console.error(err)
    );
  }

  ngOnDestroy(): void {
    this.activeUsersSubscription.unsubscribe();
    this.totalSalesSubscription.unsubscribe();
    this.topSellingProductsSubscription.unsubscribe();
  }
}
