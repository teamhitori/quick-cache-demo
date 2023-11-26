import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  public getApiResponse(): Observable<any> {
    const url = `${this.apiUrl}/api/LoadTest/0`; // Replace with your API URL
    return this.http.get<any>(url);
  }
}