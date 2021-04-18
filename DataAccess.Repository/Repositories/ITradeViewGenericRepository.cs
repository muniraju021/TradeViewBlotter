using DataAccess.Repository.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Repositories
{
    public interface ITradeViewGenericRepository
    {
        Task<IEnumerable<TradeView>> GetAllTradeViewsByPageIndex();
        Task<List<string>> GetClientCodesByGroupName(string groupName);
        Task<List<string>> GetClientCodesByDealerCode(string groupName);
        Task<IEnumerable<TradeView>> GetAllTradeViewsByClientCodes(List<string> clientCodes);
        Task<int> ArchiveAndPurgeTradeView();
    }
}
