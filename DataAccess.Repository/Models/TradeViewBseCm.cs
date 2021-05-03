﻿using AutoMapper.Configuration.Conventions;
using DataAccess.Repository.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository.Models
{
    public class TradeViewBseCm
    {
        [MapTo(nameof(TradeView.TradeId))]
        public string FillId { get; set; }

        [MapTo(nameof(TradeView.UserId))]
        public string UserId { get; set; }

        [MapTo(nameof(TradeView.ExchangeUser))]
        public string ExchUser { get; set; }

        [MapTo(nameof(TradeView.BranchId))]
        public string BranchId { get; set; }

        [MapTo(nameof(TradeView.NNFCode))]
        public string MnmLocationId { get; set; }

        [MapTo(nameof(TradeView.TokenNo))]
        public string Symbol { get; set; }

        [MapTo(nameof(TradeView.SymbolName))]
        public string SymbolName { get; set; }

        [MapTo(nameof(TradeView.OrderType))]
        public string PriceType { get; set; }

        [MapTo(nameof(TradeView.BuySell))]
        public string TransactionType { get; set; }

        [MapTo(nameof(TradeView.TradePrice))]
        public string FillPrice { get; set; }

        [MapTo(nameof(TradeView.TradeQty))]
        public string FillSize { get; set; }

        [MapTo(nameof(TradeView.TradeTime))]
        public string FillTime { get; set; }

        [MapTo(nameof(TradeView.TradeDate))]
        public string FillDate { get; set; }              

        [MapTo(nameof(TradeView.ExchangeOrderId))]
        public string ExchOrdId { get; set; }

        [MapTo(nameof(TradeView.ParticipantId))]
        public string ExecutingBroker { get; set; }

        [MapTo(nameof(TradeView.ClientCode))]
        public string ExchAccountId { get; set; }

        [MapTo(nameof(TradeView.Source))]
        public string Source { get; set; }

        [MapTo(nameof(TradeView.TradeModifyFlag))]
        public string ReportType { get; set; }

        [MapTo(nameof(TradeView.TradeDateTime))]
        public string TradeDateTime { get; set; }

    }
}
