import { Component, OnInit } from '@angular/core';
import { delay } from 'rxjs/operators';
import { BlotterService } from 'src/app/shared/services/blotterService';

@Component({
  selector: 'app-sup-services',
  templateUrl: './sup-services.component.html',
  styleUrls: ['./sup-services.component.scss'],
})
export class SupServicesComponent implements OnInit {
  constructor(private blotterService: BlotterService) {}
  progressBarVal = 20;
  nseCmError = '';
  nseCmSuccess = '';
  nseCmIsProgressBarHidden = true;

  nseFoError = '';
  nseFoSuccess = '';
  nseFoIsProgressBarHidden = true;

  bseCmError = '';
  bseCmSuccess = '';
  bseCmIsProgressBarHidden = true;

  gNseCmError = '';
  gNseCmSuccess = '';
  gNseCmIsProgressBarHidden = true;

  gNseFoError = '';
  gNseFoSuccess = '';
  gNseFoIsProgressBarHidden = true;

  gBseCmError = '';
  gBseCmSuccess = '';
  gBseCmIsProgressBarHidden = true;

  healthError = '';
  healthSuccess = '';
  healthIsProgressBarHidden = true;

  ngOnInit(): void {}

  SyncNseCmData() {
    try {
      console.log('Inside SyncNseCmData');
      this.progressBarVal = 40;
      this.nseCmIsProgressBarHidden = false;

      this.blotterService.syncNseCm().subscribe(
        (data) => {
          this.nseCmIsProgressBarHidden = true;
          this.nseCmSuccess = 'Sync of NSE CM Operation Successful..';
        },
        (error) => {
          this.nseCmIsProgressBarHidden = true;
          this.nseCmError = 'Sync of NSE CM Operation Failed..';
        }
      );
    } catch (e) {
      console.log(e);
    }
  }

  SyncNseFoData() {
    console.log('Inside SyncNseFoData');
    this.nseFoIsProgressBarHidden = false;

    this.blotterService.syncNseFo().subscribe(
      (data) => {
        this.nseFoIsProgressBarHidden = true;
        this.nseFoSuccess = 'Sync of NSE FO Operation Successful..';
      },
      (error) => {
        this.nseFoIsProgressBarHidden = true;
        this.nseFoError = 'Sync of NSE CM Operation Failed..';
      }
    );
  }

  SyncBseCmData() {
    console.log('Inside SyncBseCmData');
    this.bseCmIsProgressBarHidden = false;

    this.blotterService.syncBseCm().subscribe(
      (data) => {
        this.bseCmIsProgressBarHidden = true;
        this.bseCmSuccess = 'Sync of BSE CM Operation Successful..';
      },
      (error) => {
        this.bseCmIsProgressBarHidden = true;
        this.bseCmError = 'Sync of BSE CM Operation Failed..';
      }
    );
  }

  GreekSyncNseCmData() {
    console.log('Inside GreekSyncNseCmData');
    this.gNseCmIsProgressBarHidden = false;

    this.blotterService.syncGreekNseCm().subscribe(
      (data) => {
        this.gNseCmIsProgressBarHidden = true;
        this.gNseCmSuccess = 'Sync of Greek NSE CM Operation Successful..';
      },
      (error) => {
        this.gNseCmIsProgressBarHidden = true;
        this.gNseCmError = 'Sync of Greek NSE CM Operation Failed..';
      }
    );
  }

  GreekSyncNseFoData() {
    console.log('Inside GreekSyncNseFoData');
    this.gNseFoIsProgressBarHidden = false;

    this.blotterService.syncGreekNseFo().subscribe(
      (data) => {
        this.gNseFoIsProgressBarHidden = true;
        this.gNseFoSuccess = 'Sync of Greek NSE FO Operation Successful..';
      },
      (error) => {
        this.gNseFoIsProgressBarHidden = true;
        this.gNseFoError = 'Sync of Greek NSE FO Operation Failed..';
      }
    );
  }

  GreekSyncBseCmData() {
    console.log('Inside GreekSyncBseCmData');
    this.gBseCmIsProgressBarHidden = false;

    this.blotterService.syncGreekBseCm().subscribe(
      (data) => {
        this.gBseCmIsProgressBarHidden = true;
        this.gBseCmSuccess = 'Sync of Greek BSE CM Operation Successful..';
      },
      (error) => {
        this.gBseCmIsProgressBarHidden = true;
        this.gBseCmError = 'Sync of Greek BSE CM Operation Failed..';
      }
    );
  }

  HealthCheck() {
    console.log('Inside HealthCheck');
    this.healthIsProgressBarHidden = false;

    this.blotterService.healthCheck().subscribe(
      (data) => {
        this.healthIsProgressBarHidden = true;
        this.healthSuccess = 'Health Check Successful..';
      },
      (error) => {
        this.healthIsProgressBarHidden = true;
        this.healthError = 'Health Check Operation Failed..';
      }
    );
  }

  private async sleep() {
    await this.delay(5000);
    console.log('Trace 1');
  }
  private delay(ms: number) {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }
}
