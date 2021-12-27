import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupDealerMappingComponent } from './group-dealer-mapping-component';

describe('GroupDealerMappingComponentComponent', () => {
  let component: GroupDealerMappingComponent;
  let fixture: ComponentFixture<GroupDealerMappingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GroupDealerMappingComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupDealerMappingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
