import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from 'src/app/shared/models/user';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-manage-user',
  templateUrl: './manage-user.component.html',
  styleUrls: ['./manage-user.component.scss']
})
export class ManageUserComponent implements OnInit {
  userForm: FormGroup
  user: string = '';
  users: any = [];
  userRoles: any = [];
  userCodes: any = [];
  submitted = false;
  userRole: any = '';
  userCode: any = '';
  error = '';
  success = '';
  currentUserDetails: any;
  currentUserLogin: string = '';
  xyz: string = '';
  emailRegex: string = '[A-Za-z0-9._%-]+@[A-Za-z0-9._%-]+\\.[a-z]{2,3}';

  constructor(
    private formBuilder: FormBuilder, private userService: UserService
  ) { }

  ngOnInit(): void {
    this.userForm = this.formBuilder.group({
      inputEmail: ['', [Validators.required]],
      ddUser: [],
      ddRole: [],
      ddCode: [],
      isActive: []
    });

    this.userService.getUsers().subscribe(
      (data) => {
        this.users = data.map(({ loginName }) => loginName)
      },
      error => {
        this.success = '';
        this.error = 'An error occurred while fetching Users';
      }
    )

    this.userService.getUserRoles().subscribe(
      (data) => {
        this.userRoles = data.map(({ roleName }) => roleName)
      },
      error => {
        this.success = '';
        this.error = 'An error occurred while fetching User roles';
      }
    );

  }

  get f() { return this.userForm.controls; }

  onUserSelected(value: string) {
    this.submitted = false;
    this.error = '';
    this.success = '';
    this.currentUserLogin = value;

    this.userService.getUserByLoginName(value).subscribe(
      (data) => {
        this.currentUserDetails = data;
        this.setCurrentUserDetails();
      },
      error => {
        this.success = '';
        this.error = 'An error occurred while fetching User Details';
      }
    )
  }


  UpdateUser() {
    this.submitted = true;

    if (!this.currentUserLogin) {
      this.success = ''
      this.error = 'Please select User to update';
      return;
    }

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

    this.updateUser();

  }

  onRoleSelected(value: string) {
    this.submitted = false;
    this.userRole = value;
    this.userCode = '';
    this.error = '';

    this.fetchUserCodes();
  }

  onCodeSelected(value: string) {
    this.userCode = value;
  }

  setCurrentUserDetails() {
    this.userForm.get('inputEmail').setValue(this.currentUserDetails["emailId"]);
    this.userForm.get('isActive').setValue(this.currentUserDetails["isActive"]);

    this.userRole = this.currentUserDetails["roleName"];

    if (!(this.userRole == 'SuperAdmin')) {
      this.fetchUserCodes();
      this.userCode = this.currentUserDetails["roleCode"];
    }
    else {
      this.userCode = '';
    }
  }

  // clearForm() {
  //   this.submitted = false;
  //   this.userRole = ''
  //   this.userForm.reset();
  // }

  updateUser() {
    let newUser = new User();
    newUser.LoginName = this.currentUserLogin;
    newUser.RoleName = this.userRole;
    newUser.UserCode = this.userCode;

    if (this.userForm.get('isActive'))
      newUser.IsActive = this.userForm.get('isActive').value;

    if (this.userForm.get('inputEmail')) {
      let res = this.userForm.get('inputEmail').value.match(this.emailRegex);

      if (res && res.length > 0)
        newUser.EmailId = this.userForm.get('inputEmail').value;

      else {
        this.success = '';
        this.error = 'Please enter a valid email';
        return;
      }
    }

    this.userService.updateUser(newUser).subscribe((data) => {
      this.error = '';
      this.success = 'User successfully updated';
    },
      error => {
        this.success = '';
        this.error = 'Error while updating user: ' + newUser.LoginName;
      }
    );

  }

  fetchUserCodes() {
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

}
