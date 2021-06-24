import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class MappingService {
    constructor(private http: HttpClient) { }

    getDealers(): Observable<any> {
        return this.http.get<any>(`${environment.apiUrl}/v1/UserManagement/getDealers`)
            .pipe(map((response: Response) => {
                return response;
            }));
    }



    

}