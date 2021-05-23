using AutoMapper;
using DataAccess.Repository.Data;
using DataAccess.Repository.LogServices;
using DataAccess.Repository.Models;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        private readonly IMapper _mapper;
        private readonly string _connectionName = "OmneDataSource";
        private readonly string BrokerId = "12562";
        private readonly string ClientCode = "12562";
        private readonly string ExchangeName = "NSE_FO";
        private readonly string Source = "OMNE";
        private static ILog _log = LogService.GetLogger(typeof(TradeViewNseFoRepository));

        public TradeViewNseFoRepository(IGenericRepository<object> tradeViewNseFoRepo, ITradeViewRepository tradeViewRepositoryEf, IMapper mapper)
        {
            _tradeViewNseFoRepo = tradeViewNseFoRepo;
            _tradeViewRepositoryEf = tradeViewRepositoryEf;
            _mapper = mapper;
        }

        public async Task LoadTradeviewFromSource(bool isDeltaLoadRequested = false)
        {
            string query = string.Empty;

            try
            {
                if (isDeltaLoadRequested)
                {
                    var dtInputFrom = DateTime.Now.AddMinutes(-2).ToString("dd MMM yyyy HH:mm:00");
                    var dtInputTill = DateTime.Now.ToString("dd MMM yyyy HH:mm:00");
                    var whereCond = $" where TradeDateTime >= '{dtInputFrom}' and TradeDateTime <= '{dtInputTill}' ";
                    query = string.Format($"select FillId,UserId,ExchUser,BranchId,mnmLocationId,Symbol,SymbolName,TradingSymbol,ExpiryDate,StrikePrice,OptionType,OrderType,TransactionType," +
                            $"FillPrice,FillSize,TradeDateTime,ExchOrdId,mnmLotSize,ExecutingBroker,ExchAccountId,ReportType from NSE_FO {whereCond} order by TradeDateTime desc");

                    _log.Info($"NseFO Data requested from '{dtInputFrom}' till '{dtInputTill}'");
                }
                else
                {
                    query = string.Format($"select FillId,UserId,ExchUser,BranchId,mnmLocationId,Symbol,SymbolName,TradingSymbol,ExpiryDate,StrikePrice,OptionType,OrderType," +
                        $"TransactionType,FillPrice,FillSize,TradeDateTime,ExchOrdId,mnmLotSize,ExecutingBroker,ExchAccountId,ReportType from NSE_FO order by TradeDateTime desc");

                    _log.Info($"NseFO Data Full Load Requested");
                }

                List<TradeViewNseFo> resultSet = new List<TradeViewNseFo>();
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

                if (resultSet?.Count > 0)
                {

                    var output = new List<TradeView>();
                    foreach (var item in resultSet)
                        output.Add(_mapper.Map<TradeView>(item));

                    output = output.Select(i =>
                    {
                        i.BrokerId = BrokerId;
                        i.ProClient = i.ClientCode == ClientCode ? "PRO" : "CLI";
                        i.ExchangeName = ExchangeName;
                        i.Source = Source;
                        i.TradeDate = i.TradeDateTime;
                        i.TradeTime = i.TradeDateTime;
                        return i;
                    }).ToList();

                    //var temp = output.Where(i => i.TradeId == "7433200").ToList();
                    //temp[0].UserId = "OTRD10";
                    //temp[0].TradeId = "7433201";


                    //await _tradeViewRepositoryEf.AddTradeView(output.ToCollection<TradeView>());

                    _log.Info($"Nse FO to be Processed Records - {output.Count}");
                    _tradeViewRepositoryEf.MergeTradeView(output.ToCollection<TradeView>());
                    _log.Info($"Nse FO Processed Records - {output.Count}");
                }
                else
                    _log.Info($"Nse FO - No Records fetched");
            }
            catch (Exception ex)
            {
                _log.Error($"Exception in LoadTradeviewFromSource ", ex);
                throw ex;
            }           

            return;
        }
    }
}
