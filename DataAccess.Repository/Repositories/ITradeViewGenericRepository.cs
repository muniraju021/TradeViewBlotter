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
        Task<IEnumerable<TradeView>> GetAllTradeViewsByClientCodes(List<string> clientCodes, int offset = 0);
        Task<IEnumerable<TradeView>> GetAllTradeViewsByDealerCodeClientCodes(List<string> clientCodes, string dealerCode, int offset = 0);
        Task<IEnumerable<TradeView>> GetAllTradeViewsByDealerCodesClientCodes(List<string> clientCodes, List<string> dealerCodes, int offset = 0);
        Task<IEnumerable<TradeView>> GetTradesByDealerCode(List<string> dealerCodes, int offset = 0);
        Task<int> ArchiveAndPurgeTradeView(string exchangeName);
        Task<int> SyncWithTradeViewRefTable(string guid);

        Task<IEnumerable<NetPositionView>> GetNetPositionView();
        Task<IEnumerable<NetPositionView>> GetNetPositionViewByDealerClients(List<string> lstDealers, List<string> clients);
        Task<IEnumerable<NetPositionView>> GetNetPositionViewByClients(List<string> clients);
        Task<IEnumerable<NetPositionView>> GetNetPoistionViewByDealerCodes(List<string> lstDealers);

        Task<long> GetAllTradesCount();
        Task<TradeStats> GetTradeStats();

        Task<long> GetAllTradesCountByClientCode(List<string> clientCodeCsv);
        Task<long> GetAllTradesCountByDealerClientCode(List<string> clientCodeCsv, string dealerCode);
        Task<long> GetAllTradesCountByDealersAndClientCodes(List<string> lstclietnCodes, List<string> lstDealerCodes);
        Task<long> GetTadesCountByDealerCode(List<string> lstDealerCodes);
    }
}
