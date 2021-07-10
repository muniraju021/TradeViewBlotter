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
import { single } from 'rxjs/internal/operators/single';

@Component({ templateUrl: 'home.component.html', styleUrls: ['./home.component.scss'] })
export class HomeComponent {
    buyValue: string = '';
    sellValue: string = '';
    buyQty: string = '';
    sellQty: string = '';
    avgPriceBuy: string = '';
    avgPriceSell: string = '';
    totalNetQty: string = '';
    rowSelection: string = '';
    rowHeight: number;
    headerHeight: number;
    private gridApi;
    private gridColumnApi;
    public gridOptions: GridOptions;
    loading = false;
    users: User[];
    user: string = '';
    rowData: any[];
    userLoginName: string = ''
    columnDefs = [
        { field: 'exchangeName', headerName: 'Exch Name' },
        { field: 'clientCode' },
        { field: 'proClient', headerName: 'Pro / Client' },
        { field: 'stockName', headerName: 'Stock Name' },
        { field: 'expiryDate', headerName: 'Expiry' },
        { field: 'strikePrice', headerName: 'Strike' },
        { field: 'optionType', headerName: 'CE / PE' },
        { field: 'buySell', headerName: 'Buy / Sell' },
        { field: 'tradeQty', headerName: 'Qty' },
        { field: 'tradePrice', headerName: 'Price' },
        { field: 'tradeTime', headerName: 'Time' },
        { field: 'participantId' },
        { field: 'userId', headerName: 'User Id' },
        { field: 'tradeViewId', headerName: 'Blotter ID' },
        { field: 'tradeId', headerName: 'Exch Trade ID' },
        { field: 'symbolName', headerName: 'Instrument Name' },  
        { field: 'exchangeTime', headerName: 'Exch Timestamp' },
        { field: 'exchangeOrderId', headerName: 'Exch Order ID' },
        { field: 'orderType' },
        { field: 'tokenNo' },
        { field: 'brokerId' },
        { field: 'exchangeUser', headerName: 'CTCL ID' },
        { field: 'branchId' },
        { field: 'nNFCode' },
        { field: 'lotSize' },
        { field: 'source', headerName: 'Data Source' },
        { field: 'tradeModifyFlag', headerName: 'Trade Modified' }
    ];

    defaultColDef = {
        resizable: true,
        sortable: true,
        filter: true,
        minWidth: 100
    };

    constructor(private userService: UserService, private http: HttpClient, private blotterService: BlotterService) {
        this.rowHeight = 28;
        this.gridOptions = {
            columnDefs: this.columnDefs,
            rowData: this.rowData
        }

    }

    getRowStyle(params) {
        if (params.data.buySell === 'Buy') {
            //return { 'color': 'blue' }
        }
        if (params.data.buySell === 'Sell') {
            //return { 'color': 'red' }
        }
        return null;
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

        this.user = JSON.parse(localStorage.getItem('currentUser'))["emailId"];
        this.userLoginName = JSON.parse(localStorage.getItem('currentUser'))["loginName"];

        this.blotterService.getAllTrades(this.userLoginName).subscribe(
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
                if (rowNode.data['buySell'] == 'Buy' && rowNode.data[element])
                    tradePriceBuy += Number(rowNode.data[element]);
                if (rowNode.data['buySell'] == 'Sell' && rowNode.data[element])
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
                if (rowNode.data['buySell'] == 'Buy' && rowNode.data[element])
                    tradeQtyBuy += Number(rowNode.data[element]);
                if (rowNode.data['buySell'] == 'Sell' && rowNode.data[element])
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

        if (this.buyValue && this.buyQty)
            averageBuy = Number(this.buyValue) / Number(this.buyQty);

        if (this.sellValue && this.sellQty)
            averageSell = Number(this.sellValue) / Number(this.sellQty);

        if (averageBuy)
            this.avgPriceBuy = `${averageBuy.toFixed(2)}`;
        if (averageSell)
            this.avgPriceSell = `${averageSell.toFixed(2)}`;
    }

    onBtnExport() {
        var params = this.getParams();
        if (params.suppressQuotes || params.columnSeparator) {
            
        }
        this.gridOptions.api.exportDataAsCsv(params);
    }

    getParams() {
        return {
            suppressQuotes: true,
            columnSeparator: ','
        };
    }
}