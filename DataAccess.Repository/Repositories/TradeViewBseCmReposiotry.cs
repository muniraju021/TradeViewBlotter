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
        private readonly IMapper _mapper;
        private readonly string _connectionName = "OmneDataSource";
        private readonly string LotSize = "1";
        private readonly string BrokerId = "3107";
        private readonly string ClientCode = "12562";
        private readonly string ExchangeName = "BSE_CM";
        private static ILog _log = LogService.GetLogger(typeof(TradeViewBseCmReposiotry));

        public TradeViewBseCmReposiotry(IGenericRepository<object> tradeViewBseCmRepo, ITradeViewRepository tradeViewRepositoryEf, IMapper mapper)
        {
            _tradeViewBseCmRepo = tradeViewBseCmRepo;
            _tradeViewRepositoryEf = tradeViewRepositoryEf;
            _mapper = mapper;
        }

        public async Task LoadTradeviewFromSource(bool isDeltaLoadRequested = false)
        {

            string query = string.Empty;

            if (isDeltaLoadRequested)
            {
                var dtInputFrom = DateTime.Now.AddMinutes(-2).ToString("dd MMM yyyy HH:mm:00");
                var dtInputTill = DateTime.Now.ToString("dd MMM yyyy HH:mm:00");
                var whereCond = $" where TradeDateTime >= '{dtInputFrom}' and TradeDateTime <= '{dtInputTill}' ";
                query = string.Format($"SELECT FillId,UserId,ExchUser,BranchId,mnmLocationId,Symbol,SymbolName,PriceType,TransactionType,FillPrice,FillSize,FillTime,FillDate,ExchangeTime,ExchOrdId,ExecutingBroker," +
                                        $"ExchAccountId,Source,ReportType,TradeDateTime FROM BSE_CM {whereCond} order by FillTime desc");

                _log.Info($"BseCM Data requested from '{dtInputFrom}' till '{dtInputTill}'");
            }
            else
            {
                query = string.Format($"SELECT FillId,UserId,ExchUser,BranchId,mnmLocationId,Symbol,SymbolName,PriceType,TransactionType,FillPrice,FillSize,FillTime,FillDate,ExchangeTime,ExchOrdId,ExecutingBroker," +
                                        $"ExchAccountId,Source,ReportType,TradeDateTime FROM BSE_CM order by FillTime desc");
            }

            List<TradeViewBseCm> resultSet = new List<TradeViewBseCm>();
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

            if (resultSet?.Count > 0)
            {

                var output = new List<TradeView>();
                foreach (var item in resultSet)
                    output.Add(_mapper.Map<TradeView>(item));

                output = output.Select(i =>
                    {
                        i.LotSize = LotSize;
                        i.BrokerId = BrokerId;
                        i.StockName = i.SymbolName;
                        i.ProClient = i.ClientCode == ClientCode ? "PRO" : "CLI";
                        i.ExchangeName = ExchangeName;
                        if (i.OrderType == "L")
                            i.OrderType = "LMT";
                        else if (i.OrderType == "M")
                            i.OrderType = "MKT";
                        else if (i.OrderType == "SL")
                            i.OrderType = "SL";
                        else if (i.OrderType == "SL-M")
                            i.OrderType = "SL-MKT";
                        return i;
                    }
                ).ToList();

                //var temp = output.Where(i => i.TradeId == "7433200").ToList();
                //temp[0].UserId = "OTRD10";
                //temp[0].TradeId = "7433201";


                //await _tradeViewRepositoryEf.AddTradeView(output.ToCollection<TradeView>());
                await _tradeViewRepositoryEf.MergeTradeView(output.ToCollection<TradeView>());
                _log.Info($"Bse CM Processed Records - {output.Count}");
            }

            return;
        }

        public static void SyncTradeViewDataFromSource()
        {

        }

    }
}
