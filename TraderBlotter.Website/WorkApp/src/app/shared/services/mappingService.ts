import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class MappingService {
  constructor(private http: HttpClient) {}

  getDealers(): Observable<any> {
    return this.http
      .get<any>(`${environment.apiUrl}/v1/UserManagement/getDealers`)
      .pipe(
        map((response: Response) => {
          return response;
        })
      );
  }

  getGroups(): Observable<any> {
    return this.http
      .get<any>(`${environment.apiUrl}/v1/UserManagement/getGroups`)
      .pipe(
        map((response: Response) => {
          return response;
        })
      );
  }

  getClientCodeByDealerCode(dealerCode: string): Observable<any> {
    return this.http
      .get<any>(
        `${environment.apiUrl}/v1/UserManagement/getClientCodeByDealerCode?dealerCode=` +
          dealerCode
      )
      .pipe(
        map((response: Response) => {
          return response;
        })
      );
  }

  getDealersByGroupName(groupName: string): Observable<any> {
    return this.http
      .get<any>(
        `${environment.apiUrl}/v1/UserManagement/getDealersByGroupName?groupName=` +
          groupName
      )
      .pipe(
        map((response: Response) => {
          return response;
        })
      );
  }

  getDealerCodeNotMappedtoGroupName(groupName: string): Observable<any> {
    return this.http
      .get<any>(
        `${environment.apiUrl}/v1/UserManagement/getDealerNotMappedToGroupName?groupName=` +
          groupName
      )
      .pipe(
        map((response: Response) => {
          return response;
        })
      );
  }

  getClientCodesNotMappedToDealerCode(dealerCode: string): Observable<any> {
    return this.http
      .get<any>(
        `${environment.apiUrl}/v1/UserManagement/getClientCodesNotMappedToDealerCode?dealerCode=` +
          dealerCode
      )
      .pipe(
        map((response: Response) => {
          return response;
        })
      );
  }

  addDealerClientMapping(lstMappings: any[]): Observable<any> {
    return this.http
      .post<any>(
        `${environment.apiUrl}/v1/Mapping/addDealerClientMapping`,
        lstMappings
      )
      .pipe(
        map((response) => {
          return response;
        })
      );
  }

  addGroupDealerMapping(lstMappings: any[]): Observable<any> {
    return this.http
      .post<any>(
        `${environment.apiUrl}/v1/Mapping/addGroupDealerMapping`,
        lstMappings
      )
      .pipe(
        map((response) => {
          return response;
        })
      );
  }
}
