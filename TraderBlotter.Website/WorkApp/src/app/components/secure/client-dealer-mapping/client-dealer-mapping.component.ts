import { Component, OnInit } from '@angular/core';
import {CdkDragDrop, moveItemInArray, transferArrayItem} from '@angular/cdk/drag-drop';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-client-dealer-mapping',
  templateUrl: './client-dealer-mapping.component.html',
  styleUrls: ['./client-dealer-mapping.component.scss']
})
export class ClientDealerMappingComponent implements OnInit {
dealer:string = '';
dealers: any =[];
userForm: FormGroup;
error = '';
success = '';


  todo = [
    'Mapping 1',
    'Mapping 2',
    'Mapping 3',
    'Mapping 4'
  ];

  done = [
    'Mapping 5',
    'Mapping 6',
    'Mapping 7',
    'Mapping 8',
    'Mapping 9'
  ];

  drop(event: CdkDragDrop<string[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(event.previousContainer.data,
                        event.container.data,
                        event.previousIndex,
                        event.currentIndex);
    }
  }

  constructor() { }

  ngOnInit(): void {
    
  }

  onDealerSelected(value:string)
  {

  }

  createMapping()
  {}

}
