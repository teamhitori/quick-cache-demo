import { Component } from '@angular/core';
import { ApiService } from './api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'quick-cache-web-ui';
  apiResponse: any;
  gaugeType = "semi";
  gaugeValue = 28.3;
  gaugeLabel = "Speed";
  gaugeAppendText = "m/s";

  constructor(private apiService: ApiService) {}

  startRedisTest() {
    this.apiService.startRedisTest().subscribe(
      (response) => {
        this.apiResponse = response;
      },
      (error) => {
        console.error('Error fetching data: ', error);
      }
    );
  }

  getMetrics() {
    this.apiService.getMetrics().subscribe(
      (response) => {
        this.apiResponse = response;
      },
      (error) => {
        console.error('Error fetching data: ', error);
      }
    );
  }
}
