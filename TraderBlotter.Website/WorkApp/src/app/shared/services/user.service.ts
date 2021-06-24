import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../../environments/environment';
import { User } from '../models/user';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class UserService {
    constructor(private http: HttpClient) { }

    getAll() {
        return this.http.get<User[]>(`${environment.apiUrl}/users`);
    }

    getUserRoles(): Observable<any> {
        return this.http.get<any>(`${environment.apiUrl}/v1/UserManagement/getAllRoles`)
            .pipe(map((response: Response) => {
                return response;
            }));
    }


    getUserCodes(role: string): Observable<any> {
        return this.http.get<any>(`${environment.apiUrl}/v1/UserManagement/getUserCodes?role=` + role)
            .pipe(map((response: Response) => {
                return response;
            }));
    }

    addUser(user: User) {
        return this.http.post<any>(`${environment.apiUrl}/v1/UserManagement/addUser`, {LoginName: user.LoginName, 
            Password:user.Password, EmailId: user.EmailId, RoleName : user.RoleName, UserCode :user.UserCode, IsActive: user.IsActive })
            .pipe(map(response => {
                return response;
            }));
    }

    getUsers(): Observable<any> {
        return this.http.get<any>(`${environment.apiUrl}/v1/UserManagement/getUsers`)
            .pipe(map((response: Response) => {
                return response;
            }));
    }

updateUser(user: User): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}/v1/UserManagement/updateUser`, {LoginName: user.LoginName, 
         EmailId: user.EmailId, RoleName : user.RoleName, UserCode :user.UserCode, IsActive: user.IsActive })
        .pipe(map(response => {
            return response;
        }));
}

deleteUser(user: User): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}/v1/UserManagement/deleteUser`, {LoginName: user.LoginName})
       .pipe(map(response => {
           return response;
       }));
}

getUserByLoginName(userName: string): Observable<any> {
    return this.http.get<any>(`${environment.apiUrl}/v1/UserManagement/getUserByLoginName?loginName=` + userName)
        .pipe(map((response: Response) => {
            return response;
        }));
}


}