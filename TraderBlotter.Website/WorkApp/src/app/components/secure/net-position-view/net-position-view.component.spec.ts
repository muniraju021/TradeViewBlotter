import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NetPositionViewComponent } from './net-position-view.component';

describe('NetPositionViewComponent', () => {
  let component: NetPositionViewComponent;
  let fixture: ComponentFixture<NetPositionViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NetPositionViewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NetPositionViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
