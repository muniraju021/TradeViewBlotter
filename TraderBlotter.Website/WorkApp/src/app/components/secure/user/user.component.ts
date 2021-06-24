import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormGroupDirective, NgForm } from '@angular/forms';
import { UserService } from 'src/app/shared/services/user.service';
import { User, UserRole } from 'src/app/shared/models/user';
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
  userRole: any = '';
  userCode: any = '';
  error = '';
  success = '';
  rowHeight: number;
  headerHeight: number;
  private gridApi;
  private gridColumnApi;
  public gridOptions: GridOptions;
  rowData: any[];
  columnDefs: any[];
  defaultColDef: any;
  emailRegex: string = '[A-Za-z0-9._%-]+@[A-Za-z0-9._%-]+\\.[a-z]{2,3}';

  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild('formDirective') private formDirective: NgForm;

  constructor(
    private formBuilder: FormBuilder, private userService: UserService
  ) {
    this.rowHeight = 35;
    this.columnDefs = [
      { field: 'loginName', headerName: 'Login Name', editable: false },
      { field: 'emailId', headerName: 'Email Id' },
      { field: 'roleName', headerName: 'Role Name' },
      { field: 'roleCode', headerName: 'Role Code' },
      { field: 'isActive', headerName: 'Is Active' }
    ];

    this.defaultColDef = {
      resizable: true,
      sortable: true,
      filter: false,
      editable: true
    };
  }


  ngOnInit(): void {
    this.userForm = this.formBuilder.group({
      loginname: ['', Validators.required],
      password: ['', Validators.required],
      inputEmail: ['', [Validators.required]],
      ddCode: []
    });

    this.userService.getUserRoles().subscribe(
      (data) => {
        this.userRoles = data.map(({ roleName }) => roleName)
      },
      error => {
        this.success = '';
        this.error = 'An error occurred while fetching User roles';
      }
    );

    this.userService.getUsers().subscribe(
      (data) => {
        this.rowData = data;
      },
      error => {
        this.success = '';
        this.error = 'An error occurred while fetching Users';
      }
    )
  }

  get f() { return this.userForm.controls; }

  CreateUser() {
    this.submitted = true;

    if (this.userForm.invalid) {
      return;
    }

    if (!this.userRole) {
      this.success = ''
      this.error = 'Please select a Role';
      return;
    }

    if (this.userRole && this.userRole != 'SuperAdmin' && !(this.userCode)) {
      this.success = '';
      this.error = 'Please select a Code';
      return;
    }

    this.createUser();

    this.userService.getUsers().subscribe(
      (data) => {
        this.rowData = data;
      },
      error => {
        this.success = '';
        this.error = 'An error occurred while fetching Users';
      }
    )
  }

  onRoleSelected(value: string) {
    this.submitted = false;
    this.userRole = value;
    this.error = '';

    if ((this.userRole) && !(this.userRole == 'SuperAdmin')) {
      this.userService.getUserCodes(this.userRole).subscribe(
        (data) => {
          if ((this.userRole) && this.userRole == 'Dealer')
            this.userCodes = data.map(({ dealerCode }) => dealerCode)
          else if ((this.userRole) && this.userRole == 'GroupUser')
            this.userCodes = data.map(({ groupName }) => groupName)
          else if ((this.userRole) && this.userRole == 'Client')
            this.userCodes = data.map(({ clientCode }) => clientCode)
          else
            return;
        },
        error => {
          this.success = ''
          this.error = 'Error while fetching Codes';
        });
    }
  }

  onCodeSelected(value: string) {
    this.userCode = value;
  }

  onFirstDataRendered(params) {
    this.gridApi = params.api;
    this.gridColumnApi = params.columnApi;

    params.api.sizeColumnsToFit();
  }

  onGridReady(params) {
  }

  clearForm() {
    this.submitted = false;
    this.userRole = ''
    this.userForm.reset();
  }

  createUser() {
    let user = new User();
    if (this.f.loginname.value)
      user.LoginName = this.f.loginname.value;
    if (this.f.password.value)
      user.Password = this.f.password.value;

    if (this.f.inputEmail.value) {
      let res = this.f.inputEmail.value.match(this.emailRegex);

      if (res && res.length > 0)
        user.EmailId = this.f.inputEmail.value;

      else {
        this.success = '';
        this.error = 'Please enter a valid email';
        return;
      }
    }

    if (this.userRole)
      user.RoleName = this.userRole;

    if (<HTMLSelectElement>document.getElementById('ddCode'))
      user.UserCode = (<HTMLSelectElement>document.getElementById('ddCode')).value;
    user.IsActive = true;

    this.userService.addUser(user).subscribe((data) => {
      this.refreshGrid();
      this.error = '';
      this.success = 'User successfully created';
      this.clearForm();
    },
      error => {
        this.success = '';
        this.error = 'Error in creating user: ' + error;
      }
    );

  }

  refreshGrid() {
    this.userService.getUsers().subscribe(
      (data) => {
        this.rowData = data;
        this.gridOptions.api.setRowData(this.rowData);
      },
      error => {
        this.success = '';
        this.error = 'An error occurred while fetching Users';
      }
    )
  }

}
