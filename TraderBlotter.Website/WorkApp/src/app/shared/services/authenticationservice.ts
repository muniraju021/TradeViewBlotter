import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../../environments/environment';
import { User } from '../models/user';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private currentUserSubject: BehaviorSubject<User>;
    public currentUser: Observable<User>;
    roleId: string = '';
    userLogin: string = '';
    authToken: string = '';
    userLastLogin: string = '';

    constructor(private http: HttpClient) {
        this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
        this.currentUser = this.currentUserSubject.asObservable();
        if (this.currentUserSubject && this.currentUserSubject.value) {
            this.roleId = this.currentUserSubject.value["roleId"];
            this.userLogin = this.currentUserSubject.value["loginName"];
            this.authToken = this.currentUserSubject.value["token"];
            this.userLastLogin = this.currentUserSubject.value["lastLogin"];
        }
    }

    public get currentUserValue(): User {
        return this.currentUserSubject.value;
    }

    login(username: string, password: string) {

        return this.http.post<any>(`${environment.apiUrl}/v1/Authentication/Login`, { LoginName: username, Password: password })
            .pipe(map(user => {
                // store user details and basic auth credentials in local storage to keep user logged in between page refreshes
                user.authdata = window.btoa(username + ':' + password);
                localStorage.setItem('currentUser', JSON.stringify(user));
                this.currentUserSubject.next(user);
                this.roleId = JSON.parse(localStorage.getItem('currentUser'))["roleId"];
                this.userLogin = JSON.parse(localStorage.getItem('currentUser'))["loginName"];
                this.authToken = JSON.parse(localStorage.getItem('currentUser'))["token"];
                this.userLastLogin = JSON.parse(localStorage.getItem('currentUser'))["lastLogin"];
                return user;
            }));
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
        this.currentUserSubject.next(null);
    }
}