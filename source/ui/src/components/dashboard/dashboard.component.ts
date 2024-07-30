import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { webSocketService } from '../../services/web-socket-service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit, OnDestroy {
  activeUsers: string;
  totalSales: string;
  topSellingProducts: any;

  private activeUsersSubscription: Subscription;
  private totalSalesSubscription: Subscription;
  private topSellingProductsSubscription: Subscription;

  constructor(private wsService: webSocketService) {}


  ngOnInit(): void {
    this.activeUsersSubscription = this.wsService.getActiveUsers().subscribe(
      data => this.activeUsers = data,
      err => console.error(err)
    );
    this.totalSalesSubscription = this.wsService.getTotalSales().subscribe(
      data => this.totalSales = data,
      err => console.error(err)
    );
    this.topSellingProductsSubscription = this.wsService.getTopSellingProducts().subscribe(
      data => this.topSellingProducts = JSON.parse(data),
      err => console.error(err)
    );
  }

  ngOnDestroy(): void {
    this.activeUsersSubscription.unsubscribe();
  }
}
