import { Component, ElementRef, ViewChild } from '@angular/core';
import { ApiService } from './api.service';
import Chart from 'chart.js/auto';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'quick-cache-web-ui';

  apiRedisResponse: any;
  gaugeRedisType = "semi";
  gaugeRedisValue = 0;
  gaugeRedisLabel = "Average";
  gaugeRedisAppendText = "ms";

  apiQuickResponse: any;
  gaugeQuickType = "semi";
  gaugeQuickValue = 0;
  gaugeQuickLabel = "Average";
  gaugeQuickAppendText = "ms";

  @ViewChild('redisChartCanvas')
  redisChartCanvas!: ElementRef<HTMLCanvasElement>;
  chartRedis: Chart | undefined;

  @ViewChild('quickChartCanvas')
  quickChartCanvas!: ElementRef<HTMLCanvasElement>;
  chartQuick: Chart | undefined;

  constructor(private apiService: ApiService) {}

  ngAfterViewInit() {
    this.chartRedis = new Chart(this.redisChartCanvas.nativeElement, {
      type: 'line',
      data: {
        labels: Array.from(Array(1000).keys()), // Example labels from 0 to 9
        datasets: [{
          label: 'r/w speed in ms over time',
          backgroundColor: 'rgb(255, 99, 132)',
          borderColor: 'rgb(255, 99, 132)',
          data: [], // Example data
        }]
      },
      options: {
        responsive: true,
        aspectRatio:3
      }
    });

    this.chartQuick = new Chart(this.quickChartCanvas.nativeElement, {
      type: 'line',
      data: {
        labels: Array.from(Array(1000).keys()), // Example labels from 0 to 9
        datasets: [{
          label: 'r/w speed in ms over time',
          backgroundColor: 'rgb(255, 99, 132)',
          borderColor: 'rgb(255, 99, 132)',
          data: [], // Example data
        }]
      },
      options: {
        responsive: true,
        aspectRatio:3
      }
    });
  }

  _testActive = false;
  _redisTestActive = false;
  _quickTestActive = false;


  average(array:number[]): number {

    if(!array.length) return 0;

    return array.reduce((a, b) => a + b) / array.length

  }

  async startTest(testIndex: number) {

    if(!this._testActive){
      this._testActive = true;
      this._redisTestActive = true;
      this._quickTestActive = true;

      var startJson = await this.apiService.startRedisTest(testIndex);

      while(this._redisTestActive) {
        var respoonse = await this.apiService.getRedisResults();

        //console.log(respoonse);

        this._redisTestActive = respoonse.isRunning

        this.gaugeRedisValue = this.average(respoonse.results);

        if (this.chartRedis) {
          //
          this.chartRedis.data.datasets[0].data = respoonse.results; // Update the data
          this.chartRedis.update(); // Important: call this to refresh the chart
        }

        await this.sleep(500);
      }

      var startJson = await this.apiService.startQuickTest(testIndex);

      while(this._quickTestActive) {
        var respoonse = await this.apiService.getQuickResults();

        console.log(respoonse);

        this._quickTestActive = respoonse.isRunning

        this.gaugeQuickValue = this.average(respoonse.results);

        if (this.chartQuick) {
          //
          this.chartQuick.data.datasets[0].data = respoonse.results; // Update the data
          this.chartQuick.update(); // Important: call this to refresh the chart
        }
        await this.sleep(500);
      }

      this._testActive = false;
    }
  }

  async sleep(msec: number): Promise<void> {
      return new Promise(resolve => setTimeout(resolve, msec));
  }

  // startRedisTest() {
  //   this.apiService.startRedisTest().subscribe(
  //     (response) => {
  //       this.apiRedisResponse = JSON.parse(response);
  //     },
  //     (error) => {
  //       console.error('Error fetching data: ', error);
  //     }
  //   );
  // }

  // getRedisResults() {
  //   this.apiService.getRedisResults().subscribe(
  //     (response) => {
  //       this.apiRedisResponse = response;

  //       console.log(response.results);

  //       const average = (array:number[]) => array.reduce((a, b) => a + b) / array.length;

  //       this.gaugeRedisValue = average(response.results);

  //       if (this.chartRedis) {
  //         //
  //         this.chartRedis.data.datasets[0].data = response.results; // Update the data
  //         this.chartRedis.update(); // Important: call this to refresh the chart
  //       }
  //     },
  //     (error) => {
  //       console.error('Error fetching data: ', error);
  //     }
  //   );
  // }

  // getRedisMetrics() {
  //   this.apiService.getRedisMetrics().subscribe(
  //     (response) => {
  //       this.apiRedisResponse = response;
  //     },
  //     (error) => {
  //       console.error('Error fetching data: ', error);
  //     }
  //   );
  // }

  // startQuickTest() {
  //   this.apiService.startQuickTest().subscribe(
  //     (response) => {
  //       this.apiQuickResponse = JSON.parse(response);
  //     },
  //     (error) => {
  //       console.error('Error fetching data: ', error);
  //     }
  //   );
  // }

  // getQuickResults() {
  //   this.apiService.getQuickResults().subscribe(
  //     (response) => {
  //       this.apiQuickResponse = response;

  //       console.log(response.results);

  //       const average = (array:number[]) => array.reduce((a, b) => a + b) / array.length;

  //       this.gaugeQuickValue = average(response.results);

  //       if (this.chartQuick) {
  //         //
  //         this.chartQuick.data.datasets[0].data = response.results; // Update the data
  //         this.chartQuick.update(); // Important: call this to refresh the chart
  //       }
  //     },
  //     (error) => {
  //       console.error('Error fetching data: ', error);
  //     }
  //   );
  // }

  // getQuickMetrics() {
  //   this.apiService.getQuickMetrics().subscribe(
  //     (response) => {
  //       this.apiQuickResponse = response;
  //     },
  //     (error) => {
  //       console.error('Error fetching data: ', error);
  //     }
  //   );
  // }
}
