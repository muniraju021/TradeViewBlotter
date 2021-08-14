using AutoMapper;
using DataAccess.Repository.Data;
using DataAccess.Repository.LogServices;
using DataAccess.Repository.Models;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using log4net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Repositories
{
    public class TradeViewNseFoRepository : ITradeViewNseFoRepository
    {
        private readonly IGenericRepository<object> _tradeViewNseFoRepo;
        private readonly ITradeViewGenericRepository _tradeViewRepo;
        private readonly ITradeViewRepository _tradeViewRepositoryEf;
        private readonly ITradeViewRefRepository _tradeViewRefRepository;
        private readonly IMapper _mapper;
        private readonly string _connectionName = "OmneDataSource";
        private readonly string BrokerId = "12562";
        private readonly string ClientCode = "12562";
        private readonly string Source = "OMNE";
        private static ILog _log = LogService.GetLogger(typeof(TradeViewNseFoRepository));
        private static IConfiguration _configuration;
        private readonly string _chunkSize;

        public TradeViewNseFoRepository(IGenericRepository<object> tradeViewNseFoRepo, ITradeViewRepository tradeViewRepositoryEf,
            IMapper mapper, ITradeViewGenericRepository tradeViewRepo, ITradeViewRefRepository tradeViewRefRepository, IConfiguration configuration)
        {
            _tradeViewNseFoRepo = tradeViewNseFoRepo;
            _tradeViewRepositoryEf = tradeViewRepositoryEf;
            _tradeViewRepo = tradeViewRepo;
            _tradeViewRefRepository = tradeViewRefRepository;
            _mapper = mapper;
            _configuration = configuration;
            _chunkSize = _configuration.GetSection("SynchChunkSize")?.Value ?? "10000";
        }

        public async Task LoadTradeviewFromSource(DateTime dateTimeVal = default(DateTime), bool isDeltaLoadRequested = false)
        {
            string query = string.Empty;
            bool isChunkingEnded = false;

            try
            {
                if (isDeltaLoadRequested)
                {
                    //var dtInputFrom = DateTime.Now.AddMinutes(-2).ToString("dd MMM yyyy HH:mm:00");
                    //var dtInputTill = DateTime.Now.ToString("dd MMM yyyy HH:mm:00");

                    var dtInputFrom = DateTime.Now.AddMinutes(-2).ToString("yyyy-MM-dd HH:mm:00");
                    var dtInputTill = DateTime.Now.ToString("yyyy-MM-dd HH:mm:00");

                    var whereCond = $" where STR_TO_DATE(TradeDateTime,'%d %M %Y %H:%i:%s') >= '{dtInputFrom}' and STR_TO_DATE(TradeDateTime,'%d %M %Y %H:%i:%s') <= '{dtInputTill}' ";
                    query = string.Format($"select FillId,UserId,ExchUser,BranchId,mnmLocationId,Symbol,SymbolName,TradingSymbol,ExpiryDate,StrikePrice,OptionType,OrderType,TransactionType," +
                            $"FillPrice,FillSize,TradeDateTime,ExchOrdId,mnmLotSize,ExecutingBroker,ExchAccountId,ReportType from NSE_FO {whereCond} order by TradeDateTime desc");

                    _log.Info($"TradeViewNseFoRepository: LoadTradeviewFromSource: NseFo Data Request- From: '{dtInputFrom}' Till: '{dtInputTill}'");

                    var resultSet = new List<TradeViewNseFo>();
                    using (var reader = await _tradeViewNseFoRepo.GetDataReaderAsync(query, connectionName: _connectionName))
                    {
                        var colNames = reader.GetColumnNames();
                        while (reader.Read())
                        {
                            var dt = new Dictionary<string, string>();
                            foreach (var item in colNames)
                            {
                                dt.Add(item, reader[item].ToString());
                            }
                            resultSet.Add(JsonConvert.DeserializeObject<TradeViewNseFo>(JsonConvert.SerializeObject(dt)));
                        }
                    }
                    _log.Info($"TradeViewNseFoRepository: LoadTradeviewFromSource: NseFo Data Request- Data Count:{resultSet?.Count} From: '{dtInputFrom}' Till: '{dtInputTill}'");

                    if(resultSet.Count > 0)
                        await LoadTradeViewRefTable(resultSet);

                    _log.Info($"TradeViewNseFoRepository: LoadTradeviewFromSource: NseFo Data Request Complete- From: '{dtInputFrom}' Till: '{dtInputTill}'");
                }
                else
                {
                    int chunkIndex = 0;
                    var dateValueStr = dateTimeVal.ToString("yyyy-MM-dd 00:00:00");
                    while (!isChunkingEnded)
                    {
                        query = string.Format($"select FillId,UserId,ExchUser,BranchId,mnmLocationId,Symbol,SymbolName,TradingSymbol,ExpiryDate,StrikePrice,OptionType,OrderType," +
                                $"TransactionType,FillPrice,FillSize,TradeDateTime,ExchOrdId,mnmLotSize,ExecutingBroker,ExchAccountId,ReportType from NSE_FO " +
                                $"WHERE STR_TO_DATE(TradeDateTime, '%d %M %Y %H:%i:%s') >= '{dateValueStr}'" +
                                $"order by TradeDateTime desc LIMIT {chunkIndex},{_chunkSize}");

                        var resultSet = new List<TradeViewNseFo>();
                        using (var reader = await _tradeViewNseFoRepo.GetDataReaderAsync(query, connectionName: _connectionName))
                        {
                            var colNames = reader.GetColumnNames();
                            while (reader.Read())
                            {
                                var dt = new Dictionary<string, string>();
                                foreach (var item in colNames)
                                {
                                    dt.Add(item, reader[item].ToString());
                                }
                                resultSet.Add(JsonConvert.DeserializeObject<TradeViewNseFo>(JsonConvert.SerializeObject(dt)));
                            }
                        }
                        _log.Info($"TradeViewNseFoRepository: LoadTradeviewFromSource - NseFo Full Data requested - ChunkIndex:{chunkIndex} - DataCount:{resultSet?.Count}");

                        if (resultSet.Count <= 0)
                            isChunkingEnded = true;
                        chunkIndex += Convert.ToInt32(_chunkSize);
                        await LoadTradeViewRefTable(resultSet);

                        _log.Info($"TradeViewNseFoRepository: LoadTradeviewFromSource - NseFo Full Data Request Complete - ChunkIndex {chunkIndex}");
                    }

                }
            }
            catch (Exception ex)
            {
                _log.Error($"Exception in LoadTradeviewFromSource ", ex);
                throw ex;
            }           

            return;
        }

        private async Task LoadTradeViewRefTable(List<TradeViewNseFo> tradeViewNseFos)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            if (tradeViewNseFos?.Count > 0)
            {
                var output = new List<TradeViewRef>();
                foreach (var item in tradeViewNseFos)
                    output.Add(_mapper.Map<TradeViewRef>(item));

                var guid = Guid.NewGuid().ToString();

                output = output.Select(i =>
                {
                    i.BrokerId = BrokerId;
                    i.ProClient = i.ClientCode == ClientCode ? "PRO" : "CLI";
                    i.ExchangeName = Constants.NseFoExchangeName;
                    i.Source = Source;
                    i.TradeDate = !string.IsNullOrWhiteSpace(i.TradeDateTime) ? 
                                    DateTime.ParseExact(i.TradeDateTime, Constants.StrDateTimeFormat, provider).ToString(Constants.StrDateFormat) : null;
                    i.TradeTime = !string.IsNullOrWhiteSpace(i.TradeDateTime) ?
                                    DateTime.ParseExact(i.TradeDateTime, Constants.StrDateTimeFormat, provider).ToString(Constants.TimeFormat) : null; ;
                    i.Guid = guid;
                    i.BuySell = i.BuySell == "1" ? "Buy" : "Sell";
                    i.ComputeTotalPriceValue();
                    return i;
                }).ToList();

                //await _tradeViewRepositoryEf.AddTradeView(output.ToCollection<TradeView>());

                //_log.Info($"Nse FO to be Processed Records - {output.Count}");
                //_tradeViewRepositoryEf.MergeTradeView(output.ToCollection<TradeView>());

                //Insert into Ref Table
                _log.Info($"TradeViewNseFoRepository: LoadTradeViewFromSource Ref table Insertion Starting - {output.Count}");
                await _tradeViewRefRepository.AddTradeView(output.ToCollection<TradeViewRef>());
                _log.Info($"TradeViewNseFoRepository: LoadTradeViewFromSource Ref table Finished Starting - {output.Count}");

                //Sync with main table
                await _tradeViewRepo.SyncWithTradeViewRefTable(guid);
                _log.Info($"TradeViewNseFoRepository: LoadTradeviewRefTable - NSE FO Synching Completed - {output.Count}");
            }
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
                    query = string.Format($"select FillId,UserId,ExchUser,BranchId,mnmLocationId,Symbol,SymbolName,TradingSymbol,ExpiryDate,StrikePrice,OptionType,OrderType," +
                            $"TransactionType,FillPrice,FillSize,TradeDateTime,ExchOrdId,mnmLotSize,ExecutingBroker,ExchAccountId,ReportType from NSE_FO " +
                            $" order by TradeDateTime desc LIMIT {chunkIndex},{_chunkSize}");

                    var resultSet = new List<TradeViewNseFo>();
                    using (var reader = await _tradeViewNseFoRepo.GetDataReaderAsync(query, connectionName: _connectionName))
                    {
                        var colNames = reader.GetColumnNames();
                        while (reader.Read())
                        {
                            var dt = new Dictionary<string, string>();
                            foreach (var item in colNames)
                            {
                                dt.Add(item, reader[item].ToString());
                            }
                            resultSet.Add(JsonConvert.DeserializeObject<TradeViewNseFo>(JsonConvert.SerializeObject(dt)));
                        }
                    }
                    _log.Info($"TradeViewNseFoRepository: LoadTradeviewFulDataFromSource - NseFo Full Data requested - ChunkIndex:{chunkIndex} - DataCount:{resultSet?.Count}");

                    if (resultSet.Count <= 0)
                        isChunkingEnded = true;
                    chunkIndex += Convert.ToInt32(_chunkSize);
                    await LoadTradeViewRefTable(resultSet);

                    _log.Info($"TradeViewNseFoRepository: LoadTradeviewFulDataFromSource - NseFo Full Data Request Complete - ChunkIndex {chunkIndex}");
                }
            }
            catch (Exception ex)
            {
                _log.Error($"Exception in LoadTradeviewFulDataFromSource ", ex);
                throw ex;
            }
        }
    }
}
