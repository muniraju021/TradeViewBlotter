using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository.Data
{
    [Table("tradeview")]
    public class TradeView
    {
        [Key]
        public long TradeViewId { get; set; }
        public long BrokerId { get; set; }
        public int TradeId { get; set; }
        public string UserId { get; set; }
        public string ExchangeUser { get; set; }
        public string BranchId { get; set; }
        public string ProClient { get; set; }
        public string NNFCode { get; set; }
        public string TokenNo { get; set; }
        public string SymbolName { get; set; }
        public string StockName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double StrikePrice { get; set; }
        public string OptionType { get; set; }
        public string OrderType { get; set; }
        public string BuySell { get; set; }
        public double TradePrice { get; set; }
        public int TradeQty { get; set; }
        public DateTime TradeTime { get; set; }
        public DateTime TradeDate { get; set; }
        public long ExchangeOrderId { get; set; }
        public int LotSize { get; set; }
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
