import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from '../app/shared/services/authenticationservice';
import { User } from '../app/shared/models/user';

@Component({ selector: 'app-root', templateUrl: 'app.component.html' })
export class AppComponent {
    currentUser: User;    

    constructor(
        private router: Router,
        public authenticationService: AuthenticationService
    ) {
        this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
    }


    logout() {
        this.authenticationService.logout();
        this.router.navigate(['/login']);
    }
}