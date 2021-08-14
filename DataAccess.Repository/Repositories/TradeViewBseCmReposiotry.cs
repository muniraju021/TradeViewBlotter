using AutoMapper;
using DataAccess.Repository.Data;
using DataAccess.Repository.LogServices;
using DataAccess.Repository.Models;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using log4net;
using Microsoft.Extensions.Configuration;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.Repository.Repositories
{
    public class TradeViewBseCmReposiotry : ITradeViewBseCmRepository
    {
        private readonly IGenericRepository<object> _tradeViewBseCmRepo;
        private readonly ITradeViewGenericRepository _tradeViewRepo;
        private readonly ITradeViewRepository _tradeViewRepositoryEf;
        private readonly ITradeViewRefRepository _tradeViewRefRepository;
        private readonly IMapper _mapper;
        private readonly string _connectionName = "OmneDataSource";
        private readonly string LotSize = "1";
        private readonly string BrokerId = "3107";
        private readonly string ClientCode = "12562";
        private static ILog _log = LogService.GetLogger(typeof(TradeViewBseCmReposiotry));
        private static IConfiguration _configuration;
        private readonly string _chunkSize;

        public TradeViewBseCmReposiotry(IGenericRepository<object> tradeViewBseCmRepo,
            ITradeViewRepository tradeViewRepositoryEf,
            IMapper mapper, ITradeViewRefRepository tradeViewRefRepository, ITradeViewGenericRepository tradeViewGenericRepository, IConfiguration configuration)
        {
            _tradeViewBseCmRepo = tradeViewBseCmRepo;
            _tradeViewRepositoryEf = tradeViewRepositoryEf;
            _tradeViewRefRepository = tradeViewRefRepository;
            _tradeViewRepo = tradeViewGenericRepository;
            _mapper = mapper;
            _configuration = configuration;
            _chunkSize = _configuration.GetSection("SynchChunkSize")?.Value ?? "10000";
        }

        public async Task LoadTradeviewFromSource(DateTime dateVal = default(DateTime), bool isDeltaLoadRequested = false)
        {
            string query = string.Empty;
            bool isChunkingEnded = false;
           
            if (isDeltaLoadRequested)
            {
                var dtInputFrom = DateTime.Now.AddMinutes(-2).ToString("yyyy-MM-dd HH:mm:00");
                var dtInputTill = DateTime.Now.ToString("yyyy-MM-dd HH:mm:00");

                var whereCond = $" where STR_TO_DATE(TradeDateTime,'%d %M %Y %H:%i:%s') >= '{dtInputFrom}' and STR_TO_DATE(TradeDateTime,'%d %M %Y %H:%i:%s') <= '{dtInputTill}' ";
                query = string.Format($"SELECT FillId,UserId,ExchUser,BranchId,mnmLocationId,Symbol,SymbolName,PriceType,TransactionType,FillPrice,FillSize,FillTime,FillDate,ExchangeTime,ExchOrdId,ExecutingBroker," +
                                        $"ExchAccountId,Source,ReportType,TradeDateTime FROM BSE_CM {whereCond} order by FillTime desc");

                _log.Info($"TradeViewBseCmRepository: LoadTradeviewFromSource: BseCM Data Request- From: '{dtInputFrom}' Till: '{dtInputTill}'");

                var resultSet = new List<TradeViewBseCm>();
                using (var reader = await _tradeViewBseCmRepo.GetDataReaderAsync(query, connectionName: _connectionName))
                {
                    var colNames = reader.GetColumnNames();
                    while (reader.Read())
                    {
                        var dt = new Dictionary<string, string>();
                        foreach (var item in colNames)
                        {
                            dt.Add(item, reader[item].ToString());
                        }
                        resultSet.Add(JsonConvert.DeserializeObject<TradeViewBseCm>(JsonConvert.SerializeObject(dt)));
                    }
                }
                _log.Info($"TradeViewBseCmRepository: LoadTradeviewFromSource: BseCM Data Request- Data Count:{resultSet?.Count} From: '{dtInputFrom}' Till: '{dtInputTill}'");

                if(resultSet.Count > 0)
                    await LoadTradeviewRefTable(resultSet);

                _log.Info($"TradeViewBseCmRepository: LoadTradeviewFromSource: BseCM Data Request Complete- From: '{dtInputFrom}' Till: '{dtInputTill}'");


            }
            else
            {
                int chunkIndex = 0;
                var dateValueStr = dateVal.ToString("yyyy-MM-dd 00:00:00");
                while (!isChunkingEnded)
                {
                    query = string.Format($"SELECT FillId,UserId,ExchUser,BranchId,mnmLocationId,Symbol,SymbolName,PriceType,TransactionType,FillPrice,FillSize,FillTime,FillDate,ExchangeTime,ExchOrdId,ExecutingBroker," +
                                        $"ExchAccountId,Source,ReportType,TradeDateTime FROM BSE_CM WHERE STR_TO_DATE(TradeDateTime, '%d %M %Y %H:%i:%s') >= '{dateValueStr}'" +
                                        $" order by FillTime desc Limit {chunkIndex},{_chunkSize}");

                    _log.Info($"TradeViewBseCmRepository: LoadTradeviewFromSource - BseCM Full Data requested - Chunk {chunkIndex}");

                    var resultSet = new List<TradeViewBseCm>();
                    using (var reader = await _tradeViewBseCmRepo.GetDataReaderAsync(query, connectionName: _connectionName))
                    {
                        var colNames = reader.GetColumnNames();
                        while (reader.Read())
                        {
                            var dt = new Dictionary<string, string>();
                            foreach (var item in colNames)
                            {
                                dt.Add(item, reader[item].ToString());
                            }
                            resultSet.Add(JsonConvert.DeserializeObject<TradeViewBseCm>(JsonConvert.SerializeObject(dt)));
                        }
                    }

                    _log.Info($"TradeViewBseCmRepository: LoadTradeviewFromSource - BseCM Full Data requested - ChunkIndex:{chunkIndex} - DataCount:{resultSet?.Count}");

                    if (resultSet.Count <= 0)
                        isChunkingEnded = true;
                    chunkIndex += Convert.ToInt32(_chunkSize);
                    await LoadTradeviewRefTable(resultSet);

                    _log.Info($"TradeViewBseCmRepository: LoadTradeviewFromSource - BseCM Full Data Request Complete - ChunkIndex {chunkIndex}");


                }
            }
            return;
        }

        public async Task LoadTradeviewFulDataFromSource()
        {
            try
            {
                int chunkIndex = 0;
                string query = string.Empty;
                bool isChunkingEnded = false;
                while (!isChunkingEnded)
                {
                    query = string.Format($"SELECT FillId,UserId,ExchUser,BranchId,mnmLocationId,Symbol,SymbolName,PriceType,TransactionType,FillPrice,FillSize,FillTime,FillDate,ExchangeTime,ExchOrdId,ExecutingBroker," +
                                        $"ExchAccountId,Source,ReportType,TradeDateTime FROM BSE_CM " +
                                        $" order by FillTime desc Limit {chunkIndex},{_chunkSize}");

                    _log.Info($"TradeViewBseCmRepository: LoadTradeviewFulDataFromSource - BseCM Full Data requested - Chunk {chunkIndex}");

                    var resultSet = new List<TradeViewBseCm>();
                    using (var reader = await _tradeViewBseCmRepo.GetDataReaderAsync(query, connectionName: _connectionName))
                    {
                        var colNames = reader.GetColumnNames();
                        while (reader.Read())
                        {
                            var dt = new Dictionary<string, string>();
                            foreach (var item in colNames)
                            {
                                dt.Add(item, reader[item].ToString());
                            }
                            resultSet.Add(JsonConvert.DeserializeObject<TradeViewBseCm>(JsonConvert.SerializeObject(dt)));
                        }
                    }

                    _log.Info($"TradeViewBseCmRepository: LoadTradeviewFulDataFromSource - BseCM Full Data requested - ChunkIndex:{chunkIndex} - DataCount:{resultSet?.Count}");

                    if (resultSet.Count <= 0)
                        isChunkingEnded = true;
                    chunkIndex += Convert.ToInt32(_chunkSize);
                    await LoadTradeviewRefTable(resultSet);

                    _log.Info($"TradeViewBseCmRepository: LoadTradeviewFulDataFromSource - BseCM Full Data Request Complete - ChunkIndex {chunkIndex}");
                }
            }
            catch (Exception ex)
            {
                _log.Error($"TradeViewBseCmRepository: LoadTradeviewFulDataFromSource - Error in BseCM Full Data Request ", ex);
                throw ex;
            }
        }

        private async Task LoadTradeviewRefTable(List<TradeViewBseCm> tradeViewBseCms)
        {

            if (tradeViewBseCms?.Count > 0)
            {

                var output = new List<TradeViewRef>();
                foreach (var item in tradeViewBseCms)
                    output.Add(_mapper.Map<TradeViewRef>(item));
                var guid = Guid.NewGuid().ToString();

                output = output.Select(i =>
                {
                    i.LotSize = LotSize;
                    i.BrokerId = BrokerId;
                    i.StockName = i.SymbolName;
                    i.ProClient = i.ClientCode == ClientCode ? "PRO" : "CLI";
                    i.ExchangeName = Constants.BseCmExchangeName;
                    i.BuySell = i.BuySell == "B" ? "Buy" : "Sell";
                    if (i.OrderType == "L")
                        i.OrderType = "LMT";
                    else if (i.OrderType == "M")
                        i.OrderType = "MKT";
                    else if (i.OrderType == "SL")
                        i.OrderType = "SL";
                    else if (i.OrderType == "SL-M")
                        i.OrderType = "SL-MKT";
                    i.Guid = guid;
                    i.ComputeTotalPriceValue();
                    return i;
                }
                ).ToList();


                //await _tradeViewRepositoryEf.AddTradeView(output.ToCollection<TradeView>());
                //_tradeViewRepositoryEf.MergeTradeView(output.ToCollection<TradeView>());

                //Insert into Ref Table
                _log.Info($"TradeViewBseCmRepository: LoadTradeviewRefTable Ref table Insertion Starting - {output.Count}");
                await _tradeViewRefRepository.AddTradeView(output.ToCollection<TradeViewRef>());
                _log.Info($"TradeViewBseCmRepository: LoadTradeviewRefTable Ref table Finished Starting - {output.Count}");

                //Sync with main table
                await _tradeViewRepo.SyncWithTradeViewRefTable(guid);

                _log.Info($"TradeViewBseCmRepository: LoadTradeviewRefTable - Bse CM Syncing Completed - {output.Count}");
            }
        }

    }
}
