using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TraderBlotter.Api.Models.Dto
{
    public class TradeViewDto
    {
        public long TradeViewId { get; set; }
        public long BrokerId { get; set; }
        public int ExchangeTradeNo { get; set; }
        public string BseUserid { get; set; }
        public string BranchId { get; set; }
        public string ProClient { get; set; }
        public string NNFCode { get; set; }
        public string TokenNo { get; set; }
        public string StockName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double StrikePrice { get; set; }
        public string BsePriceType { get; set; }
        public string TransactionType { get; set; }
        public double TradePrice { get; set; }
        public int TradeQty { get; set; }
        public DateTime TradeTime { get; set; }
        public long ExchangeOrderId { get; set; }
        public int LotSize { get; set; }
        public string TradeType { get; set; }
        public string MarketType { get; set; }
        public string OrderType { get; set; }
    }
}
