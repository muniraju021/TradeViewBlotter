import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({ providedIn: 'root' })
export class BlotterService {
    constructor(private http: HttpClient) { }

    getAllTrades(userName: string): Observable<any> {
        return this.http.get<any>(`${environment.apiUrl}/v1/TradeView/getAllTrades?userName=`+userName)
            .pipe(map((response: Response) => {
                return response;
            }));
    }

    getNetPositionViewDetails(): Observable<any> {
        return this.http.get<any>(`${environment.apiUrl}/v1/NetPosition/getNetPositionViewDetails`)
            .pipe(map((response: Response) => {
                return response;
            }));
    }

    getAllTradesCount(): Observable<any> {
        return this.http.get<any>(`${environment.apiUrl}/v1/TradeView/getAllTradesCount`)
            .pipe(map((response: Response) => {
                return response;
            }));
    }
}