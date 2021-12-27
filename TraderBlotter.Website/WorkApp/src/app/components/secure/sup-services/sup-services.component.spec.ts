import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupServicesComponent } from './sup-services.component';

describe('SupServicesComponent', () => {
  let component: SupServicesComponent;
  let fixture: ComponentFixture<SupServicesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SupServicesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SupServicesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
