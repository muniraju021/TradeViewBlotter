import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ArbitragePositionComponent } from './arbitrage-position.component';

describe('ArbitragePositionComponent', () => {
  let component: ArbitragePositionComponent;
  let fixture: ComponentFixture<ArbitragePositionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ArbitragePositionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ArbitragePositionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
