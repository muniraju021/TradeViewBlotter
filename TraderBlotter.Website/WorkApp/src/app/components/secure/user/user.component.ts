import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormGroupDirective, NgForm } from '@angular/forms';
import { UserService } from 'src/app/shared/services/user.service';
import { User, UserRole } from 'src/app/shared/models/user';
import { Response } from 'selenium-webdriver/http';
import { GridOptions } from 'ag-grid-community';

function actionCellRenderer(params) {
  let eGui = document.createElement("div");

  let editingCells = params.api.getEditingCells();
  // checks if the rowIndex matches in at least one of the editing cells
  let isCurrentRowEditing = editingCells.some((cell) => {
    return cell.rowIndex === params.node.rowIndex;
  });

  if (isCurrentRowEditing) {
    eGui.innerHTML = `
<button  class="action-button update"  data-action="update"> Update  </button>
<button  class="action-button cancel"  data-action="cancel" > Cancel </button>
`;
  } else {
    eGui.innerHTML = `
<button class="action-button edit"  data-action="edit" > Edit  </button>
<button class="action-button delete" data-action="delete" > Delete </button>
`;
  }
  return eGui;
}

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
    this.columnDefs = [
      { field: 'loginName', headerName: 'Login Name', editable: false },
      { field: 'emailId', headerName: 'Email Id' },
      { field: 'roleName', headerName: 'Role Name' },
      { field: 'roleCode', headerName: 'Role Code' },
      { field: 'isActive', headerName: 'Is Active' },
      { headerName: "Action", cellRenderer: actionCellRenderer, editable: false, colId: "action" }
    ];

    function suppressEnter(params) {
      var KEY_ENTER = 13;
      var event = params.event;
      var key = event.which;
      var suppress = key === KEY_ENTER;
      return suppress;
    }

    this.defaultColDef = {
      suppressKeyboardEvent: function (params) {
        return suppressEnter(params);
      },
      resizable: true,
      sortable: true,
      filter: true,
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

  onCellClicked(params) {
    if (params.column.colId === "action" && params.event.target.dataset.action) {
      let action = params.event.target.dataset.action;
      if (action === "edit") {
        params.api.startEditingCell({
          rowIndex: params.node.rowIndex,
          colKey: params.columnApi.getDisplayedCenterColumns()[0].colId
        });
      }

      if (action === "delete") {
        this.deleteUser(params);
      }

      if (action === "update") {
        params.api.stopEditing(false);
        this.updateUser(params);
      }

      if (action === "cancel") {
        params.api.stopEditing(true);
      }
    }
  }

  onRowEditingStarted(params) {
    params.api.refreshCells({
      columns: ["action"],
      rowNodes: [params.node],
      force: true
    });
  }

  onRowEditingStopped(params) {
    params.api.refreshCells({
      columns: ["action"],
      rowNodes: [params.node],
      force: true
    });
  }

  clearForm() {
    this.submitted = false;
    this.userRole = ''
    this.userForm.reset();
  }


  updateUser(params: any) {
    let user = new User();

    if (params.data.loginName)
      user.LoginName = params.data.loginName;

    if (params.data.emailId) {
      let res = params.data.emailId.match(this.emailRegex);

      if (res && res.length > 0)
        user.EmailId = params.data.emailId;

      else {
        this.success = '';
        this.error = 'Please enter a valid email';
        this.refreshGrid();
        return;
      }
    }

    if (params.data.roleName)
      user.RoleName = params.data.roleName;
    if (params.data.roleCode)
      user.UserCode = params.data.roleCode;

    if (params.data.isActive) {
      if (params.data.isActive.toString() == "true" || params.data.isActive.toString() == "false")
        user.IsActive = params.data.isActive;
      else {
        this.success = '';
        this.error = 'Is Active can be true or false';
        this.refreshGrid();
        return;
      }
    }
    this.userService.updateUser(user).subscribe((data) => {
      //params.api.stopEditing(false);      
      this.error = '';
      this.success = 'User successfully updated';
    },
      error => {
        this.success = '';
        this.error = 'Error while updating user: ' + user.LoginName;
      }
    );
    this.refreshGrid();
  }

  deleteUser(params: any) {
    let user = new User();
    user.LoginName = params.data.loginName;

    this.userService.deleteUser(user).subscribe((data) => {
      // params.api.applyTransaction({
      //   remove: [params.node.data]
      // });     

      this.refreshGrid();

      this.error = '';
      this.success = 'User successfully deleted';
    },
      error => {
        this.success = '';
        this.error = 'Error while deleting user: ' + error;
      }
    );

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


    // if (this.f.inputEmail.value)
    //   user.EmailId = this.f.inputEmail.value;
    if (this.userRole)
      user.RoleName = this.userRole;
    // if (this.userCode)
    //   user.UserCode = this.userCode;
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
