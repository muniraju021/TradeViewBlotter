using Dapper;
using DataAccess.Repository.Data;
using DataAccess.Repository.Infrastructure;
using MySql.Data.MySqlClient.Memcached;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Repositories
{
    public class TradeViewGenericRepository : ITradeViewGenericRepository
    {
        private readonly IGenericRepository<TradeView> _tradeViewRepo;

        public TradeViewGenericRepository(IGenericRepository<TradeView> tradeViewRepo)
        {
            _tradeViewRepo = tradeViewRepo;
        }

        public async Task<IEnumerable<TradeView>> GetAllTradeViewsByPageIndex()
        {
            //var query = $"SELECT * FROM tradeview order by TradeDate desc LIMIT {pageIndex}, {pageSize}";
            var query = $"SELECT * FROM tradeview order by TradeDate desc";
            var res = await _tradeViewRepo.GetAllEntityAsync(query);
            return res;
        }

        public async Task<IEnumerable<TradeView>> GetAllTradeViewsByClientCodes(List<string> clientCodes)
        {
            var clientCodesVal = clientCodes.Select(i => { i = "'" + i + "'"; return i; }).ToList();
            var inputParam = string.Join(",", clientCodesVal);
            var query = $"SELECT * FROM tradeview where ClientCode in ({inputParam}) order by TradeDate desc";
            var res = await _tradeViewRepo.GetAllEntityAsync(query);
            return res;
        }

        public async Task<List<string>> GetClientCodesByGroupName(string groupName)
        {
            var inputParams = new DynamicParameters();
            inputParams.Add("grpName", groupName);
            var lst = await _tradeViewRepo.GetAllEntityAsync<string>("getClientCodesByGroupName", inputParams, CommandType.StoredProcedure);
            return lst.ToList();
        }

        public async Task<List<string>> GetClientCodesByDealerCode(string dealerCode)
        {
            var inputParams = new DynamicParameters();
            inputParams.Add("dealerCd", dealerCode);
            var lst = await _tradeViewRepo.GetAllEntityAsync<string>("getClientCodeByDealer", inputParams, CommandType.StoredProcedure);
            return lst.ToList();
        }
    } 
}
