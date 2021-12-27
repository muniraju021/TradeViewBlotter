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

        private string _buyTotalValue;
        public string BuyTotalValue 
        {
            get
            {
                decimal val;
                decimal.TryParse(_buyTotalValue, out val);
                return Math.Round(val, 2).ToString();
            }
            set { _buyTotalValue = value; }
        }

        private string _sellTotalValue;
        public string SellTotalValue 
        { 
            get
            {
                decimal val;
                decimal.TryParse(_sellTotalValue, out val);
                return Math.Round(val, 2).ToString();
            }
            set { _sellTotalValue = value; }
        }

        private string _buyAvg;
        public string BuyAvg
        {
            get
            {
                decimal val;
                decimal.TryParse(_buyAvg, out val);
                return Math.Round(val, 2).ToString();
            }
            set { _buyAvg = value; }
        }

        private string _sellAvg;
        public string SellAvg 
        { 
            get
            {
                decimal val;
                decimal.TryParse(_sellAvg, out val);
                return Math.Round(val, 2).ToString();
            }
            set { _sellAvg = value; }
        }

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
        public string ExpiryDate { get; set; }
        public string Profit
        {
            get 
            {
                decimal sellPrice, buyPrice;
                decimal.TryParse(SellTotalValue, out sellPrice);
                decimal.TryParse(BuyTotalValue, out buyPrice);                
                return Math.Round((sellPrice - buyPrice),2).ToString();
            }
        }
        public string UserId { get; set; }
        public string ClientCode { get; set; }
        public string ExchangeName { get; set; }

        public string OptionType { get; set; }
    }
}
