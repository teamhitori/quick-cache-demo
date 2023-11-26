import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) {}

  public startRedisTest(): Observable<any> {
    const url = `${environment.apiUrl}/api/LoadTest/0`; // Replace with your API URL
    return this.http.get<any>(url);
  }

  public getMetrics(): Observable<any> {
    const url = `${environment.prometheusUrl}/api/v1/query?query=microsoft_aspnetcore_hosting_current_requests`; // Replace with your API URL
    return this.http.get<any>(url);
  }
}