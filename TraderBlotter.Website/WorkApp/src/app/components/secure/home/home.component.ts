import { Component, OnInit, ViewChild } from '@angular/core';
import { first } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { AgGridAngular } from 'ag-grid-angular';

import { User } from '../../../shared/models/user';
import { UserService } from '../../../shared/services/user.service';
import { BlotterService } from '../../../shared/services/blotterService'
import { Observable } from 'rxjs/internal/Observable';
import { GridOptions } from 'ag-grid-community';
import { DOCUMENT } from '@angular/common';

@Component({ templateUrl: 'home.component.html' })
export class HomeComponent {
    private gridApi;
    private gridColumnApi;
    public gridOptions: GridOptions;
    loading = false;
    users: User[];
    rowData: any[];
    columnDefs = [
        { field: 'tradeViewId' },
        { field: 'brokerId' },
        { field: 'tradeId' },
        { field: 'userId' },
        { field: 'exchangeUser' },
        { field: 'branchId' },
        { field: 'proClient' },
        { field: 'nNFCode' },
        { field: 'tokenNo' },
        { field: 'symbolName' },
        { field: 'stockName' },
        { field: 'expiryDate' },
        { field: 'strikePrice' },
        { field: 'optionType' },
        { field: 'orderType' },
        { field: 'buySell' },
        { field: 'tradePrice' },
        { field: 'tradeQty' },
        { field: 'tradeTime' },
        { field: 'tradeDate' },
        { field: 'exchangeTime' },
        { field: 'exchangeOrderId' },
        { field: 'lotSize' },
        { field: 'exchangeName' },
        { field: 'participantId' },
        { field: 'marketType' },
        { field: 'clientCode' },
        { field: 'source' },
        { field: 'tradeModifyFlag' }
    ];



    defaultColDef = {
        resizable: true,
        sortable: true,
        filter: true,
        maxWidth : 95
    };

    rowHeight = 32;

    constructor(private userService: UserService, private http: HttpClient, private blotterService: BlotterService) {
    }

    onFirstDataRendered(params) {
        console.log('onFirstDataRendered');
        this.gridApi = params.api;

        params.api.sizeColumnsToFit();
        params.api.resetRowHeights(20);

        //Set column width to data
        var allColumnIds: string[] = [];
        params.columnApi.getAllColumns().forEach(function (column) {
            allColumnIds.push(column.colId);
        });

        params.columnApi.autoSizeColumns(allColumnIds, true);        
    }

      onGridReady(params) {
        console.log('onGridReady');
        // this.gridApi = params.api;
        // this.gridColumnApi = params.columnApi;
        // params.api.sizeColumnsToFit();
        // params.api.resetRowHeights(20);
      }

    onFilterTextBoxChanged() {
        this.gridApi.setQuickFilter((<HTMLInputElement>document.getElementById("filter-text-box")).value);
    }


    ngOnInit() {

        this.loading = true;
        this.userService.getAll().pipe(first()).subscribe(users => {
            this.loading = false;
            this.users = users;
        });

        this.blotterService.getAllTrades().subscribe(
            (data) => {
                this.rowData = data;
            }
        )
    }


}