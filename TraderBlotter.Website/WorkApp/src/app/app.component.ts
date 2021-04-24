import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from '../app/shared/services/authenticationservice';
import { User } from '../app/shared/models/user';

@Component({ selector: 'app-root', templateUrl: 'app.component.html' })
export class AppComponent {
    currentUser: User;
    userLoginName: string = ''

    constructor(
        private router: Router,
        private authenticationService: AuthenticationService
    ) {
        this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
        // if (JSON.parse(localStorage.getItem('currentUser'))) {
        //     this.userLoginName = JSON.parse(localStorage.getItem('currentUser'))["loginName"];
        // }
    }

    logout() {
        this.authenticationService.logout();
        this.router.navigate(['/login']);
    }
}