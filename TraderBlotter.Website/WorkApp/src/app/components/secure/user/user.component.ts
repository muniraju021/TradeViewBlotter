import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UserService } from 'src/app/shared/services/user.service';
import { User, UserRole } from 'src/app/shared/models/user';
import { Response } from 'selenium-webdriver/http';
import { GridOptions } from 'ag-grid-community';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {
  userForm: FormGroup
  userRoles: any = [];
  userCodes: any = [];
  submitted = false;
  userRole: any = 'SuperAdmin';
  userCode: any = '';
  error = '';
  success = '';
  loading = false;
  rowHeight: number;
  headerHeight: number;
  private gridApi;
  private gridColumnApi;
  public gridOptions: GridOptions;
  rowData: any[];
  columnDefs: any[];

  defaultColDef = {
    resizable: true,
    sortable: true,
    filter: true
};

  constructor(
    private formBuilder: FormBuilder, private userService: UserService
  ) { }


  ngOnInit(): void {
    this.userForm = this.formBuilder.group({
      loginname: ['', Validators.required],
      password: ['', Validators.required],
      inputEmail: ['', [Validators.required, Validators.email]]
    });

    this.userService.getUserRoles().subscribe(
      (data) => {
        this.userRoles = data.map(({ roleName }) => roleName)
      },
      () => { }
    );
  }

  get f() { return this.userForm.controls; }

  CreateUser() {
    this.error = null;
    this.submitted = true;

    if (this.userForm.invalid) {
      return;
    }

    this.loading=true;

    this.loading = true;

    let user = new User();
    user.LoginName = this.f.loginname.value,
      user.Password = this.f.password.value,
      user.EmailId = this.f.inputEmail.value;
    user.RoleName = this.userRole;
    user.UserCode = this.userCode;

    this.userService.addUser(user).subscribe((data) => {
      this.userForm.reset();
      this.loading=false;
    },
      error => {
        this.error = error;
        this.loading = false;
      }
    );

  }

  onRoleSelected(value: string) {
    this.userRole = value;

    this.userService.getUserCodes(this.userRole).subscribe(
      (data) => {
        this.userCodes = data.map(({ clientCode }) => clientCode)
      },
      error => {
        this.error = error;
      });
  }

  onCodeSelected(value: string) {
    console.log("the selected value is " + value);
    this.userCode = value;
  }

  onFirstDataRendered(params) {
  }

  onGridReady(params) {
  }

  onFilterChanged(params) {
}

}
