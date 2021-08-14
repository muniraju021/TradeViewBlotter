using DataAccess.Repository.Data;
using DataAccess.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Repositories
{
    public interface ITradeViewGenericRepository
    {
        Task<IEnumerable<TradeView>> GetAllTradeViewsByPageIndex(int offset);
        Task<List<string>> GetClientCodesByGroupName(string groupName);
        Task<List<string>> GetClientCodesByDealerCode(string groupName);
        Task<IEnumerable<TradeView>> GetAllTradeViewsByClientCodes(List<string> clientCodes);
        Task<int> ArchiveAndPurgeTradeView(string exchangeName);
        Task<int> SyncWithTradeViewRefTable(string guid);
        Task<IEnumerable<NetPositionView>> GetNetPositionView();
        Task<long> GetAllTradesCount();
    }
}
