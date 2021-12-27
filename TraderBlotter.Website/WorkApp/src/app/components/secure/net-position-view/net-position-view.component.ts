import { Component, OnInit } from '@angular/core';
import { GridOptions, RowNode } from 'ag-grid-community';
import { BlotterService } from 'src/app/shared/services/blotterService';

@Component({
  selector: 'app-net-position-view',
  templateUrl: './net-position-view.component.html',
  styleUrls: ['./net-position-view.component.scss'],
})
export class NetPositionViewComponent implements OnInit {
  rowHeight: number;
  userLoginName: string;
  public gridOptions: GridOptions;
  columnDefs = [
    { field: 'exchangeName', headerName: 'Exchange Name' },
    { field: 'stockName', headerName: 'Stock Name' },
    { field: 'optionType', headerName: 'Option Type' },
    {
      field: 'buyQuantity',
      headerName: 'Buy Qty',
      cellStyle: { color: 'navy' },
    },
    {
      field: 'sellQuantity',
      headerName: 'Sell Qty',
      cellStyle: { color: 'red' },
    },
    { field: 'buyTotalValue', headerName: 'Buy Value' },
    { field: 'sellTotalValue', headerName: 'Sell Value' },
    { field: 'buyAvg', headerName: 'Buy Avg' },
    { field: 'sellAvg', headerName: 'Sell Avg' },
    { field: 'netQuantity', headerName: 'Net Qty' },
    { field: 'expiryDate', headerName: 'Expiry' },
    {
      field: 'profit',
      headerName: 'Profit',
      cellStyle: (params) => {
        if (params.value > 0) {
          return { color: 'green' };
        }
        return { color: 'red' };
      },
    },
    { field: 'userId', headerName: 'User Id' },
    { field: 'clientCode', headerName: 'Client Id' },
  ];
  rowData: any;

  defaultColDef = {
    resizable: true,
    sortable: true,
    filter: true,
    minWidth: 100,
  };
  gridApi: any;
  gridColumnApi: any;
  buyValue: string = '';
  sellValue: string = '';
  buyQty: string = '';
  sellQty: string = '';
  totalNetQty: string = '';
  loading = false;
  bap: string = '';
  sap: string = '';
  pnl: string = '';
  tradesCount: string = '';
  usd: any = 0.013;
  euro: any = 0.011;
  uk: any = 0.0097;

  constructor(private blotterService: BlotterService) {
    this.rowHeight = 28;
    this.gridOptions = {
      columnDefs: this.columnDefs,
      rowData: this.rowData,
    };
  }

  onGridReady(params) {}

  onFirstDataRendered(params) {
    this.gridApi = params.api;
    this.gridColumnApi = params.columnApi;

    params.api.sizeColumnsToFit();

    this.calculations();
  }

  ngOnInit(): void {
    this.userLoginName = JSON.parse(localStorage.getItem('currentUser'))[
      'loginName'
    ];
    this.blotterService
      .getNetPositionViewDetails(this.userLoginName)
      .subscribe((data) => {
        this.rowData = data;
      });
  }

  onFilterTextBoxChanged() {
    this.gridApi.setQuickFilter(
      (<HTMLInputElement>document.getElementById('filter-text-box')).value
    );
  }

  onFilterChanged(params) {
    this.calculations();
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
      columnSeparator: ',',
    };
  }

  getRowStyle(params) {}

  calculations() {
    this.bap =
      this.sap =
      this.buyValue =
      this.sellValue =
      this.buyQty =
      this.sellQty =
      this.totalNetQty =
      this.pnl =
      this.tradesCount =
        '';
    let buyAvgPrice = 0;
    let sellAvgPrice = 0;
    let buyValue = 0;
    let sellValue = 0;
    let buyQty = 0;
    let sellQty = 0;
    let netQty = 0;
    let pAndL = 0;

    let columnsWithAggregation = ['buyAvg'];
    columnsWithAggregation.forEach((element) => {
      this.gridApi.forEachNodeAfterFilter((rowNode: RowNode) => {
        buyAvgPrice += Number(rowNode.data[element]);
      });
    });

    columnsWithAggregation = ['sellAvg'];
    columnsWithAggregation.forEach((element) => {
      this.gridApi.forEachNodeAfterFilter((rowNode: RowNode) => {
        sellAvgPrice += Number(rowNode.data[element]);
      });
    });

    columnsWithAggregation = ['buyTotalValue'];
    columnsWithAggregation.forEach((element) => {
      this.gridApi.forEachNodeAfterFilter((rowNode: RowNode) => {
        buyValue += Number(rowNode.data[element]);
      });
    });

    columnsWithAggregation = ['sellTotalValue'];
    columnsWithAggregation.forEach((element) => {
      this.gridApi.forEachNodeAfterFilter((rowNode: RowNode) => {
        sellValue += Number(rowNode.data[element]);
      });
    });

    columnsWithAggregation = ['buyQuantity'];
    columnsWithAggregation.forEach((element) => {
      this.gridApi.forEachNodeAfterFilter((rowNode: RowNode) => {
        buyQty += Number(rowNode.data[element]);
      });
    });

    columnsWithAggregation = ['sellQuantity'];
    columnsWithAggregation.forEach((element) => {
      this.gridApi.forEachNodeAfterFilter((rowNode: RowNode) => {
        sellQty += Number(rowNode.data[element]);
      });
    });

    columnsWithAggregation = ['netQuantity'];
    columnsWithAggregation.forEach((element) => {
      this.gridApi.forEachNodeAfterFilter((rowNode: RowNode) => {
        netQty += Number(rowNode.data[element]);
      });
    });

    if (buyAvgPrice) this.bap = `${buyAvgPrice.toFixed(2)}`;
    if (sellAvgPrice) this.sap = `${sellAvgPrice.toFixed(2)}`;
    if (buyValue) this.buyValue = `${buyValue.toFixed(2)}`;
    if (sellValue) this.sellValue = `${sellValue.toFixed(2)}`;
    if (buyQty) this.buyQty = `${buyQty.toFixed(2)}`;
    if (sellQty) this.sellQty = `${sellQty.toFixed(2)}`;
    if (netQty) this.totalNetQty = `${netQty.toFixed(2)}`;

    if (buyValue || sellValue) {
      pAndL = sellValue - buyValue;
      this.pnl = `${pAndL.toFixed(2)}`;
    }

    this.blotterService.getAllTradesCount().subscribe((data) => {
      this.tradesCount = data;
    });
  }
}
