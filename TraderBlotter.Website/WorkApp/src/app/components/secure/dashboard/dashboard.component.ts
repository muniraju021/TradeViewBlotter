import { Component, OnInit } from '@angular/core';
import { ChartType } from 'chart.js';
import { MultiDataSet, Label } from 'ng2-charts';
import { BlotterService } from '../../../shared/services/blotterService'

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  public nseCount: any;
  public bseCount: any;
  public totalCount: number = 0;
  public doughnutChartLabels: Label[] = ['NSE Trade Count', 'BSE Trade Count'];
  public doughnutChartData: MultiDataSet = [];

  public doughnutChartType: ChartType = 'doughnut';

  public doughnutChartOptions: any = {
    aspectRatio: 1
  }

  //   public doughnutChartColors: any[] = 
  // [
  //     {
  //         backgroundColor: 'rgba(177,200,84,0.2)',
  //         borderColor: 'rgba(106,185,236,1)'
  //     }
  // ]


  constructor(public blotterService: BlotterService) { }

  //this.doughnutChartData = [];

  ngOnInit(): void {
    this.blotterService.getTradesCount().subscribe(
      (data) => {
        this.nseCount = data.nseCount;
        this.bseCount = data.bseCount;
        this.totalCount = data.totalCount;

        // this.nseCount = 70;
        // this.bseCount = 30;
        // this.totalCount = 100;

        this.doughnutChartData = [[0, 0], [this.nseCount, this.bseCount]];

      }
    )
  }

}
