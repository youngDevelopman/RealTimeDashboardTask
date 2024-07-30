import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TotalSalesChartComponent } from './total-sales-chart.component';

describe('TotalSalesChartComponent', () => {
  let component: TotalSalesChartComponent;
  let fixture: ComponentFixture<TotalSalesChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TotalSalesChartComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TotalSalesChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
