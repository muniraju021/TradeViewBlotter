import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({ providedIn: 'root' })
export class BlotterService {
  constructor(private http: HttpClient) {}

  getAllTrades(userName: string): Observable<any> {
    return this.http
      .get<any>(
        `${environment.apiUrl}/v1/TradeView/getAllTrades?userName=` + userName
      )
      .pipe(
        map((response: Response) => {
          return response;
        })
      );
  }

  getNetPositionViewDetails(userName: string): Observable<any> {
    return this.http
      .get<any>(
        `${environment.apiUrl}/v1/NetPosition/getNetPositionViewDetails?userName=` +
          userName
      )
      .pipe(
        map((response: Response) => {
          return response;
        })
      );
  }

  getAllTradesCount(): Observable<any> {
    return this.http
      .get<any>(`${environment.apiUrl}/v1/TradeView/getAllTradesCount`)
      .pipe(
        map((response: Response) => {
          return response;
        })
      );
  }

  getTradesCount(): Observable<any> {
    return this.http
      .get<any>(`${environment.apiUrl}/v1/Dashboard/getTradesCount`)
      .pipe(
        map((response: Response) => {
          return response;
        })
      );
  }

  getHealthCheckStats(): Observable<any> {
    return this.http
      .get<any>(`${environment.apiUrl}/v1/Dashboard/getHealthCheckStats`)
      .pipe(
        map((response: Response) => {
          return response;
        })
      );
  }

  syncNseCm(): Observable<any> {
    try {
      return this.http
        .get<any>(`${environment.apiUrl}/v1/LoadTradeView/SyncNseCmAllTrades`)
        .pipe(
          map((response: Response) => {
            return response;
          })
        );
    } catch (e) {
      console.log(e);
    }
    return null;
  }

  syncNseFo(): Observable<any> {
    try {
      return this.http
        .get<any>(`${environment.apiUrl}/v1/LoadTradeView/SyncNseFoAllTrades`)
        .pipe(
          map((response: Response) => {
            return response;
          })
        );
    } catch (e) {
      console.log(e);
    }
    return null;
  }

  syncBseCm(): Observable<any> {
    try {
      return this.http
        .get<any>(`${environment.apiUrl}/v1/LoadTradeView/SyncBseCmAllTrades`)
        .pipe(
          map((response: Response) => {
            return response;
          })
        );
    } catch (e) {
      console.log(e);
    }
    return null;
  }

  syncGreekNseCm(): Observable<any> {
    try {
      return this.http
        .get<any>(
          `${environment.apiUrl}/v1/LoadTradeView/SyncGreekNseCmAllTrades`
        )
        .pipe(
          map((response: Response) => {
            return response;
          })
        );
    } catch (e) {
      console.log(e);
    }
    return null;
  }

  syncGreekNseFo(): Observable<any> {
    try {
      return this.http
        .get<any>(
          `${environment.apiUrl}/v1/LoadTradeView/SyncGreekNseFoAllTrades`
        )
        .pipe(
          map((response: Response) => {
            return response;
          })
        );
    } catch (e) {
      console.log(e);
    }
    return null;
  }

  syncGreekBseCm(): Observable<any> {
    try {
      return this.http
        .get<any>(
          `${environment.apiUrl}/v1/LoadTradeView/SyncGreekBseCmAllTrades`
        )
        .pipe(
          map((response: Response) => {
            return response;
          })
        );
    } catch (e) {
      console.log(e);
    }
    return null;
  }

  healthCheck(): Observable<any> {
    try {
      return this.http.get<any>(`${environment.apiUrl}/v1/HealthCheck`).pipe(
        map((response: Response) => {
          return response;
        })
      );
    } catch (e) {
      console.log(e);
    }
    return null;
  }
}
