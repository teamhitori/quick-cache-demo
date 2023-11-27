import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, take, lastValueFrom } from 'rxjs';
import { environment } from '../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) {}

  public async startRedisTest(testIndex: number): Promise<any> {
    const url = `${environment.apiUrl}/api/RedisTest/${testIndex}`;
    const request$ = this.http.get<any>(url).pipe(take(1));
    return await lastValueFrom<any>(request$);
  }

  public async getRedisResults(): Promise<any> {
    const url = `${environment.apiUrl}/api/RedisTest/results`; // Replace with your API URL
    const request$ = this.http.get<any>(url).pipe(take(1));
    return await lastValueFrom<any>(request$);
  }

  public async getRedisMetrics(): Promise<any> {
    const url = `${environment.prometheusUrl}/api/v1/query?query=test_redis_duration_seconds_bucket`; // Replace with your API URL
    const request$ = this.http.get<any>(url).pipe(take(1));
    return await lastValueFrom<any>(request$);
  }

  public async startQuickTest(testIndex: number): Promise<any> {
    const url = `${environment.apiUrl}/api/QuickCacheTest/${testIndex}`; // Replace with your API URL
    const request$ = this.http.get<any>(url).pipe(take(1));
    return await lastValueFrom<any>(request$);
  }

  public async getQuickResults(): Promise<any> {
    const url = `${environment.apiUrl}/api/QuickCacheTest/results`; // Replace with your API URL
    const request$ = this.http.get<any>(url).pipe(take(1));
    return await lastValueFrom<any>(request$);
  }

  public async getQuickMetrics(): Promise<any> {
    const url = `${environment.prometheusUrl}/api/v1/query?query=test_quick_cache_duration_seconds_bucket`; // Replace with your API URL
    const request$ = this.http.get<any>(url).pipe(take(1));
    return await lastValueFrom<any>(request$);
  }
}