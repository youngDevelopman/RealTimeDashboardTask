import { Injectable } from '@angular/core';
import { Observable, Observer } from 'rxjs';
@Injectable({
 providedIn: 'root',
})
export class webSocketService {
    private ws: WebSocket;

    private baseUrl = 'ws://localhost:6969';

    getActiveUsers(): Observable<string> {
      return this.connect(this.baseUrl + '/ws/active-users');
    }

    getTotalSales(): Observable<string> {
        return this.connect(this.baseUrl + '/ws/total-sales');
    }

    getTopSellingProducts(): Observable<string> {
        return this.connect(this.baseUrl + '/ws/top-selling-products');
    }
  
    connect(url: string): Observable<string> {
      return new Observable((observer: Observer<string>) => {
        this.ws = new WebSocket(url);
  
        this.ws.onmessage = (event) => observer.next(event.data);
        this.ws.onerror = (event) => observer.error(event);
        this.ws.onclose = (event) => observer.complete();
  
        return () => this.ws.close();
      });
    }
}