using Dapper;
using DataAccess.Repository.Data;
using DataAccess.Repository.Infrastructure;
using DataAccess.Repository.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient.Memcached;
using Org.BouncyCastle.Bcpg.OpenPgp;
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
        private readonly IConfiguration _configuration;
        private readonly bool _blnArchiveDataEnabled;

        public TradeViewGenericRepository(IGenericRepository<TradeView> tradeViewRepo, IConfiguration configuration)
        {
            _tradeViewRepo = tradeViewRepo;
            _configuration = configuration;
            _blnArchiveDataEnabled = _configuration.GetSection("ArchiveEnabled")?.Value != null ? Convert.ToBoolean(_configuration.GetSection("ArchiveEnabled")?.Value) : false;
        }

        public async Task<IEnumerable<TradeView>> GetAllTradeViewsByPageIndex(int offset)
        {
            var query = $"SELECT * FROM tradeview where STR_TO_DATE(TradeDateTime,'%d %M %Y %H:%i:%s') >= curdate() order by TradeDateTime desc LIMIT {offset}, {Constants.ChunkCount}";
            //var query = $"SELECT * FROM tradeview order by TradeDateTime desc";
            var res = await _tradeViewRepo.GetAllEntityAsync(query, commandTimeout: 240);
            return res;
        }

        public async Task<IEnumerable<TradeView>> GetAllTradeViewsByDealerCodeClientCodes(List<string> clientCodes, string dealerCode, int offset = 0)
        {
            var clientCodesVal = clientCodes.Select(i => { i = "'" + i + "'"; return i; }).ToList();
            if (clientCodes?.Count > 0)
            {
                var inputParam = string.Join(",", clientCodesVal);
                var query = $"SELECT * FROM tradeview where UserId = '{dealerCode}' and ClientCode in ({inputParam}) order by TradeDate desc LIMIT {offset}, {Constants.ChunkCount}";
                var res = await _tradeViewRepo.GetAllEntityAsync(query, commandTimeout: 240);
                return res;
            }
            return default(IEnumerable<TradeView>);
        }

        public async Task<IEnumerable<TradeView>> GetAllTradeViewsByClientCodes(List<string> clientCodes, int offset = 0)
        {
            var clientCodesVal = clientCodes.Select(i => { i = "'" + i + "'"; return i; }).ToList();
            if (clientCodes?.Count > 0)
            {
                var inputParam = string.Join(",", clientCodesVal);
                var query = $"SELECT * FROM tradeview where STR_TO_DATE(TradeDateTime,'%d %M %Y %H:%i:%s') >= curdate() and (ClientCode in ({inputParam}) or exchangeName in ('GREEK_NSE_CM','GREEK_NSE_FO','GREEK_BSE_CM')) " +
                    $"order by TradeDateTime desc LIMIT {offset}, {Constants.ChunkCount}";
                var res = await _tradeViewRepo.GetAllEntityAsync(query, commandTimeout: 240);
                return res;
            }
            return default(IEnumerable<TradeView>);
        }

        public async Task<IEnumerable<TradeView>> GetAllTradeViewsByDealerCodesClientCodes(List<string> clientCodes, List<string> dealerCodes, int offset = 0)
        {
            var clientCodesVal = clientCodes.Select(i => { i = "'" + i + "'"; return i; }).ToList();
            var deakerCodesVal = dealerCodes.Select(i => { i = "'" + i + "'"; return i; }).ToList();
            if (clientCodes?.Count > 0)
            {
                var inputParam = string.Join(",", clientCodesVal);
                var dealerCodeConcat = string.Join(",", deakerCodesVal);
                var query = $"SELECT * FROM tradeview where UserId in ({dealerCodeConcat}) and ClientCode in ({inputParam}) order by TradeDate desc LIMIT {offset}, {Constants.ChunkCount}";
                var res = await _tradeViewRepo.GetAllEntityAsync(query, commandTimeout: 240);
                return res;
            }
            return default(IEnumerable<TradeView>);
        }

        public async Task<IEnumerable<TradeView>> GetTradesByDealerCode(List<string> dealerCodes, int offset=0)
        {
            var deakerCodesVal = dealerCodes.Select(i => { i = "'" + i + "'"; return i; }).ToList();
            if (dealerCodes?.Count > 0)
            {
                var dealerCodeConcat = string.Join(",", deakerCodesVal);
                var query = $"SELECT * FROM tradeview where STR_TO_DATE(TradeDateTime,'%d %M %Y %H:%i:%s') > curdate() and (UserId in ({dealerCodeConcat}) or exchangeName in ('GREEK_NSE_CM','GREEK_NSE_FO','GREEK_BSE_CM'))" +
                    $" order by TradeDateTime desc LIMIT {offset}, {Constants.ChunkCount}";
                var res = await _tradeViewRepo.GetAllEntityAsync(query, commandTimeout: 240);
                return res;
            }
            return default(IEnumerable<TradeView>);
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

        public async Task<int> ArchiveAndPurgeTradeView(string exchangeName)
        {
            if (_blnArchiveDataEnabled)
            {
                var inputParams = new DynamicParameters();
                inputParams.Add("exchangeName", exchangeName);
                var res = await _tradeViewRepo.ExcecuteNonQueryAsync("archiveTradeViewData", parameters: inputParams, cmdType: CommandType.StoredProcedure, commandTimeout: 240);
                return res;
            }
            return -1;
        }

        public async Task<int> SyncWithTradeViewRefTable(string guid)
        {
            var inputParams = new DynamicParameters();
            inputParams.Add("guidKey", guid);
            var res = await _tradeViewRepo.ExcecuteNonQueryAsync("SyncTradeViewWithRefTable", parameters: inputParams, cmdType: CommandType.StoredProcedure, commandTimeout: 240);
            return res;
        }

        public async Task<long> GetAllTradesCount()
        {
            var res = await _tradeViewRepo.GetEntityAsync<long>(spName: "GetAllTradesCount", cmdType: CommandType.StoredProcedure, commandTimeout: 240);
            return res;
        }

        public async Task<TradeStats> GetTradeStats()
        {
            var res = await _tradeViewRepo.GetEntityAsync<TradeStats>(spName: "getNseBseCount", cmdType: CommandType.StoredProcedure, commandTimeout: 180);
            return res;
        }

        public async Task<long> GetAllTradesCountByDealerClientCode(List<string> lstclietnCodes, string dealerCode)
        {
            var inputParams = new DynamicParameters();
            inputParams.Add("clientCod", string.Join(",", lstclietnCodes));
            inputParams.Add("dealerCode", dealerCode);
            var count = await _tradeViewRepo.GetEntityAsync<long>("GetAllTradesCountByDealerClientCode", inputParams, CommandType.StoredProcedure, commandTimeout: 240);
            return count;
        }


        public async Task<long> GetAllTradesCountByClientCode(List<string> lstclietnCodes)
        {
            var inputParams = new DynamicParameters();
            inputParams.Add("clientCod", string.Join(",", lstclietnCodes));
            var count = await _tradeViewRepo.GetEntityAsync<long>("GetAllTradesCountByClientCode", inputParams, CommandType.StoredProcedure, commandTimeout: 240);
            return count;
        }

        public async Task<long> GetAllTradesCountByDealersAndClientCodes(List<string> lstclietnCodes, List<string> lstDealerCodes)
        {
            var inputParams = new DynamicParameters();
            inputParams.Add("clientCod", string.Join(",", lstclietnCodes));
            inputParams.Add("dealerCode", string.Join(",", lstDealerCodes));
            var count = await _tradeViewRepo.GetEntityAsync<long>("GetAllTradesCountByDealerCodesClientCodes", inputParams, CommandType.StoredProcedure, commandTimeout: 240);
            return count;
        }

        public async Task<long> GetTadesCountByDealerCode(List<string> lstDealerCodes)
        {
            var inputParams = new DynamicParameters();
            inputParams.Add("dealerCode", string.Join(",", lstDealerCodes));
            var count = await _tradeViewRepo.GetEntityAsync<long>("GetTradesCountByDealerCodes", inputParams, CommandType.StoredProcedure, commandTimeout: 240);
            return count;
        }

        #region NetPositionViews
        public async Task<IEnumerable<NetPositionView>> GetNetPositionView()
        {
            var res = await _tradeViewRepo.GetAllEntityAsync<NetPositionView>(spName: "getNetPositionByStockName", cmdType: CommandType.StoredProcedure);
            foreach (var item in res)
            {
                item.SellQuantity = item.SellQuantity ?? "0";
                item.BuyQuantity = item.BuyQuantity ?? "0";
            }

            return res;
        }

        public async Task<IEnumerable<NetPositionView>> GetNetPositionViewByDealerClients(List<string> lstDealers, List<string> lstClients)
        {
            var inputParams = new DynamicParameters();
            inputParams.Add("clientCod", string.Join(",", lstClients));
            inputParams.Add("dealerCode", string.Join(",", lstDealers));
            var res = await _tradeViewRepo.GetAllEntityAsync<NetPositionView>("getNetPositionByStockNameByDealerAndClients", inputParams, CommandType.StoredProcedure, commandTimeout: 240);
            foreach (var item in res)
            {
                item.SellQuantity = item.SellQuantity ?? "0";
                item.BuyQuantity = item.BuyQuantity ?? "0";
            }
            return res;
        }

        public async Task<IEnumerable<NetPositionView>> GetNetPoistionViewByDealerCodes(List<string> lstDealers)
        {
            var inputParams = new DynamicParameters();
            inputParams.Add("dealerCode", string.Join(",", lstDealers));
            var res = await _tradeViewRepo.GetAllEntityAsync<NetPositionView>("getNetPositionByStockNameByDealerCode", inputParams, CommandType.StoredProcedure, commandTimeout: 240);
            foreach (var item in res)
            {
                item.SellQuantity = item.SellQuantity ?? "0";
                item.BuyQuantity = item.BuyQuantity ?? "0";
            }
            return res;
        }

        public async Task<IEnumerable<NetPositionView>> GetNetPositionViewByClients(List<string> lstClients)
        {
            var inputParams = new DynamicParameters();
            inputParams.Add("clientCod", string.Join(",", lstClients));
            var res = await _tradeViewRepo.GetAllEntityAsync<NetPositionView>("getNetPositionByStockNameByClients", inputParams, CommandType.StoredProcedure, commandTimeout: 240);
            foreach (var item in res)
            {
                item.SellQuantity = item.SellQuantity ?? "0";
                item.BuyQuantity = item.BuyQuantity ?? "0";
            }
            return res;
        }
        #endregion

        public Task<IEnumerable<NetPositionView>> GetNetPositionViewByDealerClients(string dealerCode, List<string> clients)
        {
            throw new NotImplementedException();
        }
    }
}
