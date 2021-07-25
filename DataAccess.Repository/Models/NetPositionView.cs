using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository.Models
{
    public class NetPositionView
    {
        public string StockName { get; set; }
        public string BuyQuantity { get; set; }
        public string SellQuantity { get; set; }
        public string BuyTotalValue { get; set; }
        public string SellTotalValue { get; set; }
        public string BuyAvg { get; set; }
        public string SellAvg { get; set; }
        public string NetQuantity
        {
            get
            {
                long sellQty, buyQty;
                long.TryParse(SellQuantity, out sellQty);
                long.TryParse(BuyQuantity, out buyQty);
                return (buyQty - sellQty).ToString();
            }
        }
        public string ExpriyDate { get; set; }
        public string Profit
        {
            get 
            {
                decimal sellPrice, buyPrice;
                decimal.TryParse(SellTotalValue, out sellPrice);
                decimal.TryParse(BuyTotalValue, out buyPrice);
                return (sellPrice - buyPrice).ToString();
            }
        }
        public string UserId { get; set; }
        public string ClientCode { get; set; }
        public string ExchangeName { get; set; }

    }
}
