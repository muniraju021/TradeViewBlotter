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
    public class TradeViewNseCmReposiotry : ITradeViewNseCmRepository
    {
        private readonly IGenericRepository<object> _tradeViewGenericRepo;
        private readonly ITradeViewGenericRepository _tradeViewRepo;
        private readonly ITradeViewRepository _tradeViewRepositoryEf;
        private readonly ITradeViewRefRepository _tradeViewRefRepository;
        private readonly IMapper _mapper;
        private readonly string Source = "OMNE";
        private readonly string _connectionName = "OmneDataSource";
        private readonly string LotSize = "1";
        private readonly string BrokerId = "12562";
        private readonly string ClientCodeConst = "12562";        
        private static ILog _log = LogService.GetLogger(typeof(TradeViewNseCmReposiotry));
        private static IConfiguration _configuration;
        private readonly string _chunkSize;

        public TradeViewNseCmReposiotry(IGenericRepository<object> tradeViewGenericRepo,
            ITradeViewRepository tradeViewRepositoryEf,
            IMapper mapper, ITradeViewRefRepository tradeViewRefRepository, ITradeViewGenericRepository tradeViewGenericRepository, IConfiguration configuration)
        {
            _tradeViewGenericRepo = tradeViewGenericRepo;
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
                query = string.Format($"SELECT FillId,UserId,ExchUser,BranchId,mnmLocationId,Symbol,SymbolName,TransactionType,FillPrice,FillSize," +
                                        $"ExchOrdId,ExecutingBroker,ExchAccountId,Source,ReportType,TradeDateTime FROM NSE_CM {whereCond} order by TradeDateTime desc");

                _log.Info($"TradeViewNseCmRepository: LoadTradeviewFromSource: NseCM Data Request- From: '{dtInputFrom}' Till: '{dtInputTill}'");

                var resultSet = new List<TradeViewNseCm>();
                using (var reader = await _tradeViewGenericRepo.GetDataReaderAsync(query, connectionName: _connectionName))
                {
                    var colNames = reader.GetColumnNames();
                    while (reader.Read())
                    {
                        var dt = new Dictionary<string, string>();
                        foreach (var item in colNames)
                        {
                            dt.Add(item, reader[item].ToString());
                        }
                        resultSet.Add(JsonConvert.DeserializeObject<TradeViewNseCm>(JsonConvert.SerializeObject(dt)));
                    }
                }
                _log.Info($"TradeViewNseCmRepository: LoadTradeviewFromSource: NseCM Data Request- Data Count:{resultSet?.Count} From: '{dtInputFrom}' Till: '{dtInputTill}'");

                if(resultSet.Count > 0)
                    await LoadTradeviewRefTable(resultSet);

                _log.Info($"TradeViewNseCmRepository: LoadTradeviewFromSource: NseCM Data Request Complete- From: '{dtInputFrom}' Till: '{dtInputTill}'");


            }
            else
            {
                int chunkIndex = 0;
                var dateValueStr = dateVal.ToString("yyyy-MM-dd 00:00:00");
                while (!isChunkingEnded)
                {
                    query = string.Format($"SELECT FillId,UserId,ExchUser,BranchId,mnmLocationId,Symbol,SymbolName,TransactionType,FillPrice,FillSize," +
                                        $"ExchOrdId,ExecutingBroker,ExchAccountId,Source,ReportType,TradeDateTime FROM NSE_CM WHERE STR_TO_DATE(TradeDateTime, '%d %M %Y %H:%i:%s') >= '{dateValueStr}'" +
                                        $" order by TradeDateTime desc Limit {chunkIndex},{_chunkSize}");

                    _log.Info($"TradeViewNseCmRepository: LoadTradeviewFromSource - NseCM Full Data requested - Chunk {chunkIndex}");

                    var resultSet = new List<TradeViewNseCm>();
                    using (var reader = await _tradeViewGenericRepo.GetDataReaderAsync(query, connectionName: _connectionName))
                    {
                        var colNames = reader.GetColumnNames();
                        while (reader.Read())
                        {
                            var dt = new Dictionary<string, string>();
                            foreach (var item in colNames)
                            {
                                dt.Add(item, reader[item].ToString());
                            }
                            resultSet.Add(JsonConvert.DeserializeObject<TradeViewNseCm>(JsonConvert.SerializeObject(dt)));
                        }
                    }

                    _log.Info($"TradeViewNseCmRepository: LoadTradeviewFromSource - NseCM Full Data requested - ChunkIndex:{chunkIndex} - DataCount:{resultSet?.Count}");

                    if (resultSet.Count <= 0)
                        isChunkingEnded = true;
                    chunkIndex += Convert.ToInt32(_chunkSize);
                    await LoadTradeviewRefTable(resultSet);

                    _log.Info($"TradeViewNseCmRepository: LoadTradeviewFromSource - NseCM Full Data Request Complete - ChunkIndex {chunkIndex}");


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
                    query = string.Format($"SELECT FillId,UserId,ExchUser,BranchId,mnmLocationId,Symbol,SymbolName,TransactionType,FillPrice,FillSize," +
                                        $"ExchOrdId,ExecutingBroker,ExchAccountId,Source,ReportType,TradeDateTime FROM NSE_CM " +
                                        $" order by TradeDateTime desc Limit {chunkIndex},{_chunkSize}");

                    _log.Info($"TradeViewNseCmRepository: LoadTradeviewFulDataFromSource - NseCM Full Data requested - Chunk {chunkIndex}");

                    var resultSet = new List<TradeViewNseCm>();
                    using (var reader = await _tradeViewGenericRepo.GetDataReaderAsync(query, connectionName: _connectionName))
                    {
                        var colNames = reader.GetColumnNames();
                        while (reader.Read())
                        {
                            var dt = new Dictionary<string, string>();
                            foreach (var item in colNames)
                            {
                                dt.Add(item, reader[item].ToString());
                            }
                            resultSet.Add(JsonConvert.DeserializeObject<TradeViewNseCm>(JsonConvert.SerializeObject(dt)));
                        }
                    }

                    _log.Info($"TradeViewNseCmRepository: LoadTradeviewFulDataFromSource - NseCM Full Data requested - ChunkIndex:{chunkIndex} - DataCount:{resultSet?.Count}");

                    if (resultSet.Count <= 0)
                        isChunkingEnded = true;
                    chunkIndex += Convert.ToInt32(_chunkSize);
                    await LoadTradeviewRefTable(resultSet);

                    _log.Info($"TradeViewNseCmRepository: LoadTradeviewFulDataFromSource - NseCM Full Data Request Complete - ChunkIndex {chunkIndex}");
                }
            }
            catch (Exception ex)
            {
                _log.Error($"TradeViewNseCmRepository: LoadTradeviewFulDataFromSource - Error in NseCM Full Data Request ", ex);
                throw ex;
            }
        }

        private async Task LoadTradeviewRefTable(List<TradeViewNseCm> tradeViewNseCms)
        {

            if (tradeViewNseCms?.Count > 0)
            {

                var output = new List<TradeViewRef>();
                foreach (var item in tradeViewNseCms)
                    output.Add(_mapper.Map<TradeViewRef>(item));
                var guid = Guid.NewGuid().ToString();

                output = output.Select(i =>
                {
                    i.LotSize = LotSize;
                    i.BrokerId = BrokerId;
                    i.StockName = i.SymbolName;
                    i.ProClient = i.ParticipantId != ClientCodeConst ? "CLI" : "PRO";
                    i.ExchangeName = Constants.NseCmExchangeName;
                    i.TradeDate = i.TradeDateTime;
                    i.TradeTime = i.TradeDateTime;
                    i.BuySell = i.BuySell == "1" ? "Buy" : "Sell";
                    //if (i.OrderType == "L")
                    //    i.OrderType = "LMT";
                    //else if (i.OrderType == "M")
                    //    i.OrderType = "MKT";
                    //else if (i.OrderType == "SL")
                    //    i.OrderType = "SL";
                    //else if (i.OrderType == "SL-M")
                    //    i.OrderType = "SL-MKT";
                    i.Source = Source;
                    i.Guid = guid;
                    i.ComputeTotalPriceValue();
                    return i;
                }
                ).ToList();


                //await _tradeViewRepositoryEf.AddTradeView(output.ToCollection<TradeView>());
                //_tradeViewRepositoryEf.MergeTradeView(output.ToCollection<TradeView>());

                //Insert into Ref Table
                _log.Info($"TradeViewNseCmRepository: LoadTradeviewRefTable Ref table Insertion Starting - {output.Count}");
                await _tradeViewRefRepository.AddTradeView(output.ToCollection<TradeViewRef>());
                _log.Info($"TradeViewNseCmRepository: LoadTradeviewRefTable Ref table Finished Starting - {output.Count}");

                //Sync with main table
                await _tradeViewRepo.SyncWithTradeViewRefTable(guid);

                _log.Info($"TradeViewNseCmRepository: LoadTradeviewRefTable - Nse CM Syncing Completed - {output.Count}");
            }
        }

    }
}
