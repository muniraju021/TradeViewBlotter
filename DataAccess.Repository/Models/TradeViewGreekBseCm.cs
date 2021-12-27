using AutoMapper.Configuration.Conventions;
using DataAccess.Repository.Data;
using DataAccess.Repository.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DataAccess.Repository.Models
{
    public class TradeViewGreekBseCm
    {
        private string _column1;
        [MapTo(nameof(TradeView.BrokerId))]        
        public string Column1 { get { return _column1; } set { _column1 = value?.Trim(); } }

        private string _column2;    
        public string Column2 { get { return _column2; } set { _column2 = value?.Trim(); } }

        private string _column3;
        [MapTo(nameof(TradeView.TokenNo))]
        public string Column3 { get { return _column3; } set { _column3 = value?.Trim(); } }
                
        private string _column4;
        [MapTo(nameof(TradeView.SymbolName))]        
        public string Column4 { get { return _column4; } set { _column4 = value?.Trim(); } }
                        
        private string _column5;
        [MapTo(nameof(TradeView.TradePrice))]        
        public string Column5 { get { return _column5; } set { _column5 = value?.Trim(); } }

        private string _column6;
        [MapTo(nameof(TradeView.TradeQty))]        
        public string Column6 { get { return _column6; } set { _column6 = value?.Trim(); } }

        private string _column7;      
        public string Column7 { get { return _column7; } set { _column7 = value?.Trim(); } }

        private string _column8;    
        public string Column8 { get { return _column8; } set { _column8 = value?.Trim(); } }

        private string _column9;
        [MapTo(nameof(TradeView.TradeTime))]        
        public string Column9 { get { return _column9; } set { _column9 = value?.Trim(); } }

        private string _column10;
        [MapTo(nameof(TradeView.TradeDate))]        
        public string Column10 { get { return _column10; } set { _column10 = value?.Trim(); } }

        private string _column11;
        [MapTo(nameof(TradeView.ClientCode))]
        public string Column11 { get { return _column11; } set { _column11 = value?.Trim(); } }

        private string _column12;
        [MapTo(nameof(TradeView.ExchangeOrderId))]
        public string Column12 { get { return _column12; } set { _column12 = value?.Trim(); } }

        private string _column13;        
        public string Column13 { get { return _column13; } set { _column13 = value?.Trim(); } }

        private string _column14;
        [MapTo(nameof(TradeView.BuySell))]        
        public string Column14 { get { return _column14; } set { _column14 = value?.Trim(); } }

        private string _column15;
        public string Column15 { get { return _column15; } set { _column15 = value?.Trim(); } }

        private string _column16;
        [MapTo(nameof(TradeView.ProClient))]        
        public string Column16 { get { return _column16; } set { _column16 = value?.Trim(); } }

        private string _column17;        
        public string Column17 { get { return _column17; } set { _column17 = value?.Trim(); } }

        private string _column18;      
        public string Column18 { get { return _column18; } set { _column18 = value?.Trim(); } }

        private string _column19;    
        public string Column19 { get { return _column19; } set { _column19 = value?.Trim(); } }

        private string _column20;
        [MapTo(nameof(TradeView.TradeDateTime))]
        public string Column20 { get { return _column20; } set { _column20 = value?.Trim(); } }

        private string _column21;        
        public string Column21 { get { return _column21; } set { _column21 = value?.Trim(); } }

        private string _column22;
        [MapTo(nameof(TradeView.NNFCode))]        
        public string Column22 { get { return _column22; } set { _column22 = value?.Trim(); } }

        private string _column23;        
        public string Column23 { get { return _column23; } set { _column23 = value?.Trim(); } }

        private string _column24;        
        public string Column24 { get { return _column24; } set { _column24 = value?.Trim(); } }

        private string _column25;
        [MapTo(nameof(TradeView.ParticipantId))]
        public string Column25 { get { return _column25; } set { _column25 = value?.Trim(); } }

        private string _column26;
        public string Column26 { get { return _column26; } set { _column26 = value?.Trim(); } }

        private string _column27;
        public string Column27 { get { return _column27; } set { _column27 = value?.Trim(); } }

        private string _column28;
        public string Column28 { get { return _column28; } set { _column28 = value?.Trim(); } }

        private string _column29;
        public string Column29 { get { return _column29; } set { _column29 = value?.Trim(); } }

        public DateTime TradeDateTimeVal
        {
            get
            {
                DateTime dt;
                if (!string.IsNullOrWhiteSpace(Column20))
                {
                    if (DateTime.TryParseExact(Column20, Constants.StrDateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                        return dt;
                }
                return default(DateTime);
            }
        }
    }
}
