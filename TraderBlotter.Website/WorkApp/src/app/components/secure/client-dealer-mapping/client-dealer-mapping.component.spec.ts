import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientDealerMappingComponent } from './client-dealer-mapping.component';

describe('ClientDealerMappingComponent', () => {
  let component: ClientDealerMappingComponent;
  let fixture: ComponentFixture<ClientDealerMappingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClientDealerMappingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientDealerMappingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
