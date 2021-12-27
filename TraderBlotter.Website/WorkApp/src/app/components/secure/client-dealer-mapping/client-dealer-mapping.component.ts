import { Component, OnInit } from '@angular/core';
import {
  CdkDragDrop,
  moveItemInArray,
  transferArrayItem,
} from '@angular/cdk/drag-drop';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MappingService } from '../../../shared/services/mappingService';

@Component({
  selector: 'app-client-dealer-mapping',
  templateUrl: './client-dealer-mapping.component.html',
  styleUrls: ['./client-dealer-mapping.component.scss'],
})
export class ClientDealerMappingComponent implements OnInit {
  mappingForm: FormGroup;
  dealer: string = '';
  dealers: any = [];
  userForm: FormGroup;
  error = '';
  success = '';
  currentDealer: string = '';
  //lstMappings: any = [];

  availableMapping = [];

  currentMapping = [];

  drop(event: CdkDragDrop<string[]>) {
    if (event.previousContainer === event.container) {
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

  constructor(
    private mappingService: MappingService,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    this.mappingForm = this.formBuilder.group({});

    this.mappingService.getDealers().subscribe((data) => {
      this.dealers = data.map(({ dealerCode }) => dealerCode);
    });
  }

  onDealerSelected(value: string) {
    this.currentDealer = value;
    this.mappingService.getClientCodeByDealerCode(value).subscribe((data) => {
      this.currentMapping = data.map(({ clientCode }) => clientCode);
    });

    this.mappingService
      .getClientCodesNotMappedToDealerCode(value)
      .subscribe((data) => {
        this.availableMapping = data.map(({ clientCode }) => clientCode);
      });
  }

  onSubmit() {
    let lstMappings = [];

    this.currentMapping.forEach((element) => {
      lstMappings.push({ dealerCode: this.currentDealer, clientCode: element });
    });

    this.mappingService.addDealerClientMapping(lstMappings).subscribe(
      (data) => {
        this.error = '';
        this.success = 'User successfully updated';
      },
      () => {
        this.success = '';
        this.error = 'Error while trying to update Mappings';
      }
    );
  }
}
