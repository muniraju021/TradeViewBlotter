using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TraderBlotter.Api.Models.Dto
{
    public class TradeViewDto
    {
        public long TradeViewId { get; set; }
        public string BrokerId { get; set; }
        public string TradeId { get; set; }
        public string UserId { get; set; }
        public string ExchangeUser { get; set; }
        public string BranchId { get; set; }
        public string ProClient { get; set; }
        public string NNFCode { get; set; }
        public string TokenNo { get; set; }
        public string SymbolName { get; set; }
        public string StockName { get; set; }
        public string ExpiryDate { get; set; }
        public double StrikePrice { get; set; }
        public string OptionType { get; set; }
        public string OrderType { get; set; }
        public string BuySell { get; set; }
        public double TradePrice { get; set; }
        public string TradeQty { get; set; }
        public string TradeTime { get; set; }
        public string TradeDate { get; set; }
        public DateTime ExchangeTime { get; set; }
        public string ExchangeOrderId { get; set; }
        public string LotSize { get; set; }
        public string ExchangeName { get; set; }
        public string ParticipantId { get; set; }
        public double Currency1 { get; set; }
        public double Currency2 { get; set; }
        public double Currency3 { get; set; }
        public string MarketType { get; set; }
        public string ClientCode { get; set; }
        public string Source { get; set; }
        public string TradeModifyFlag { get; set; }
    }
}
