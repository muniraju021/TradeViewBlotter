using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository.Data
{
    [Table("tradeview_ref")]
    public class TradeViewRef
    {
        [Key]
        public int Id { get; set; }
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
        public string StrikePrice { get; set; }
        public string OptionType { get; set; }
        public string OrderType { get; set; }
        public string BuySell { get; set; }
        public string TradePrice { get; set; }
        public string TradeQty { get; set; }
        public string TradeTime { get; set; }
        public string TradeDate { get; set; }
        public string ExchangeOrderId { get; set; }
        public string LotSize { get; set; }
        public string ExchangeName { get; set; }
        public string ParticipantId { get; set; }
        public string ClientCode { get; set; }
        public string Source { get; set; }
        public string TradeModifyFlag { get; set; }
        public string TradeDateTime { get; set; }
        public string Guid { get; set; }

        private string _totalBuySellValueTotalValue;
        public string TotalBuySellTotalValue
        {
            get
            {
                return _totalBuySellValueTotalValue;
            }
            set
            {
                _totalBuySellValueTotalValue = value;
            }
        }

        public void ComputeTotalPriceValue()
        {
            decimal price, qty;
            decimal.TryParse(TradePrice, out price);
            decimal.TryParse(TradeQty, out qty);
            decimal val = price * qty;
            _totalBuySellValueTotalValue = Math.Round(val, 2).ToString();
        }
    }
}
