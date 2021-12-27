import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import {
  CdkDragDrop,
  moveItemInArray,
  transferArrayItem,
} from '@angular/cdk/drag-drop';
import { MappingService } from '../../../shared/services/mappingService';

@Component({
  selector: 'app-group-dealer-mapping',
  templateUrl: './group-dealer-mapping-component.html',
  styleUrls: ['./group-dealer-mapping-component.scss'],
})
export class GroupDealerMappingComponent implements OnInit {
  mappingForm: FormGroup;
  groupName: string = '';
  groupNames: any = [];
  dealerCode: string = '';
  userForm: FormGroup;
  error = '';
  success = '';
  currentGroup: string = '';

  availableMapping = [];
  currentMapping = [];

  constructor(
    private mappingService: MappingService,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    this.mappingForm = this.formBuilder.group({});

    this.mappingService.getGroups().subscribe((data) => {
      this.groupNames = data.map(({ groupName }) => groupName);
    });
  }

  onGroupNameSelected(value: string) {
    this.currentGroup = value;
    this.mappingService.getDealersByGroupName(value).subscribe((data) => {
      this.currentMapping = data.map(({ dealerCode }) => dealerCode);
    });

    this.mappingService
      .getDealerCodeNotMappedtoGroupName(value)
      .subscribe((data) => {
        this.availableMapping = data.map(({ dealerCode }) => dealerCode);
      });
  }

  drop(event: CdkDragDrop<string[]>) {
    if (event.previousContainer == event.container) {
      moveItemInArray(
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    }
  }

  onSubmit() {
    let lstMappings = [];

    this.currentMapping.forEach((element) => {
      lstMappings.push({ groupName: this.currentGroup, dealerCode: element });
    });

    if (lstMappings.length == 0) {
      lstMappings.push({
        groupName: this.groupName,
        dealercode: '',
      });
    }

    this.mappingService.addGroupDealerMapping(lstMappings).subscribe(
      (data) => {
        this.error = '';
        this.success = 'Group Mapping Details updated successfully';
      },
      () => {
        this.success = '';
        this.error = 'Error while trying to update Mapping';
      }
    );
  }
}
