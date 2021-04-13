import { Component, OnInit, ViewChild } from '@angular/core';
import { first } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { AgGridAngular } from 'ag-grid-angular';

import { User } from '../../../shared/models/user';
import { UserService } from '../../../shared/services/user.service';
import { BlotterService } from '../../../shared/services/blotterService'
import { Observable } from 'rxjs/internal/Observable';
import { GridOptions, RowNode } from 'ag-grid-community';
import { DOCUMENT } from '@angular/common';
import { element } from 'protractor';

@Component({ templateUrl: 'home.component.html', styleUrls: ['./home.component.scss'] })
export class HomeComponent {
    buyValue: string = '';
    sellValue: string = '';
    buyQty: string = '';
    sellQty: string = '';
    avgPriceBuy: string = '';
    avgPriceSell: string = '';
    totalNetQty: string = '';
    private gridApi;
    private gridColumnApi;
    public gridOptions: GridOptions;
    loading = false;
    users: User[];
    rowData: any[];
    columnDefs = [
        { field: 'tradeViewId', headerName: 'Blotter ID' },
        { field: 'tradeId', headerName: 'Exch Trade ID' },
        { field: 'symbolName', headerName: 'Instrument Name' },
        { field: 'stockName' },
        { field: 'tradePrice', headerName: 'Price' },
        { field: 'tradeQty', headerName: 'Qty' },
        { field: 'expiryDate', headerName: 'Expiry' },
        { field: 'strikePrice', headerName: 'Strike' },
        { field: 'optionType', headerName: 'CE / PE' },
        { field: 'buySell', headerName: 'Buy / Sell' },
        { field: 'tradeTime', headerName: 'Time' },
        { field: 'exchangeTime', headerName: 'Exch Timestamp' },
        { field: 'exchangeOrderId', headerName: 'Exch Order ID' },
        { field: 'orderType' },
        { field: 'tokenNo' },
        { field: 'exchangeName', headerName: 'Exch Name' },
        { field: 'brokerId' },       
        { field: 'userId' },//dealer id
        { field: 'exchangeUser', headerName: 'CTCL ID' },
        { field: 'branchId' },
        { field: 'proClient', headerName: 'Pro / Client' },
        { field: 'nNFCode' },
        { field: 'lotSize' },
        { field: 'participantId' },
        { field: 'clientCode' },        
        { field: 'source', headerName:'Data Source' },
        { field: 'tradeModifyFlag', headerName: 'Trade Modified' }
    ];

    defaultColDef = {
        resizable: true,
        sortable: true,
        filter: true,
        //maxWidth : 95,
    };

    rowHeight = 28;

    constructor(private userService: UserService, private http: HttpClient, private blotterService: BlotterService) {
    }


    onFirstDataRendered(params) {
        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;

        params.api.sizeColumnsToFit();

        //Set column width to data
        var allColumnIds: string[] = [];
        this.gridColumnApi.getAllColumns().forEach(function (column) {
            allColumnIds.push(column.colId);
        });
        this.gridColumnApi.autoSizeColumns(allColumnIds, true);

        this.tradePricecalculations();
        this.tradeQtycalculations();
        this.avgCalculations();
    }

    onGridReady(params) {
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

    onFilterChanged(params) {
        this.tradePricecalculations();
        this.tradeQtycalculations();
        this.avgCalculations();
    }


    tradePricecalculations() {
        let columnsWithAggregation = ['tradePrice']
        let tradePriceBuy: number = 0;
        let tradePriceSell: number = 0;
        this.buyValue = '';
        this.sellValue = '';

        columnsWithAggregation.forEach(element => {
            this.gridApi.forEachNodeAfterFilter((rowNode: RowNode) => {
                if (rowNode.data['buySell'] == 'B' && rowNode.data[element])
                    tradePriceBuy += Number(rowNode.data[element]);
                if (rowNode.data['buySell'] == 'S' && rowNode.data[element])
                    tradePriceSell += Number(rowNode.data[element]);
            });
        })
        if (tradePriceBuy)
            this.buyValue = `${tradePriceBuy.toFixed(2)}`;
        if (tradePriceSell)
            this.sellValue = `${tradePriceSell.toFixed(2)}`;
    }

    tradeQtycalculations() {
        let columnsWithAggregation = ['tradeQty']
        let tradeQtyBuy: number = 0;
        let tradeQtySell: number = 0;
        let totalNetQuantity: number = 0;
        this.buyQty = '';
        this.sellQty = '';
        this.totalNetQty = '';

        columnsWithAggregation.forEach(element => {
            this.gridApi.forEachNodeAfterFilter((rowNode: RowNode) => {
                if (rowNode.data['buySell'] == 'B' && rowNode.data[element])
                    tradeQtyBuy += Number(rowNode.data[element]);
                if (rowNode.data['buySell'] == 'S' && rowNode.data[element])
                    tradeQtySell += Number(rowNode.data[element]);
            });
        })
        if (tradeQtyBuy)
            this.buyQty = `${tradeQtyBuy}`;
        if (tradeQtySell)
            this.sellQty = `${tradeQtySell}`;

        totalNetQuantity = Number(this.buyQty) - Number(this.sellQty)

        if (totalNetQuantity)
            this.totalNetQty = `${totalNetQuantity}`;
    }

    avgCalculations() {
        this.avgPriceBuy = '';
        this.avgPriceSell = '';
        let averageBuy: number = 0;
        let averageSell: number = 0;

        if(this.buyValue && this.buyQty)
        averageBuy = Number(this.buyValue) / Number(this.buyQty);

        if(this.sellValue && this.sellQty)
        averageSell = Number(this.sellValue) / Number(this.sellQty);

        if (averageBuy)
            this.avgPriceBuy = `${averageBuy.toFixed(2)}`;
        if (averageSell)
            this.avgPriceSell = `${averageSell.toFixed(2)}`;
    }
}