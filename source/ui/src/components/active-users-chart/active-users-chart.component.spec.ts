import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ActiveUsersChartComponent } from './active-users-chart.component';

describe('ActiveUsersChartComponent', () => {
  let component: ActiveUsersChartComponent;
  let fixture: ComponentFixture<ActiveUsersChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ActiveUsersChartComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ActiveUsersChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
