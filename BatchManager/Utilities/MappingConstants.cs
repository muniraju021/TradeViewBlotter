using System;
using System.Collections.Generic;
using System.Text;

namespace BatchManager.Utilities
{
    public class MappingConstants
    {
        public static Dictionary<string, string> BseCmColMappings = new Dictionary<string, string>
        {
            { "TraderViewId","" },
            { "BrokerId","3107" },
            { "TraderId","FillId" },
            { "UserId","UserId" },
            { "ExchangeUser","ExchUser" },
            { "BranchId","BranchId" },
            { "ProClient","" },
            { "NNFCode","mnmLocationId" },
            { "TokenNo","Symbol" },
            { "SymbolName","SymbolName" },
            { "StockName","SymbolName" },
            { "ExpiryDate","" },
            { "StrikePrice","" },
            { "OptionType","" },
            { "OrderType","PriceType" },
            { "BuySell","TransactionType" },
            { "TradePrice","FillPrice" },
            { "TradeQty","FillSize" },
            { "TradeDate","FillDate" },
            { "ExchangeOrderId","ExchOrdId" },
            { "LotSize","1" },
            { "ExchangeName","BSE_CM" },
            { "ParticipantId","ExecutingBroker" },
            { "ClientCode","ExchAccountId" },
            { "Source","Source" },
            { "TradeModifyFlag","ExchAccountId" }
        };
    }
}
