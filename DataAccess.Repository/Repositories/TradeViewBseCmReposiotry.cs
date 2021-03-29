using AutoMapper;
using DataAccess.Repository.Data;
using DataAccess.Repository.Models;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
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

        public TradeViewBseCmReposiotry(IGenericRepository<object> tradeViewBseCmRepo, ITradeViewGenericRepository tradeViewRepo, ITradeViewRepository tradeViewRepositoryEf, IMapper mapper)
        {
            _tradeViewBseCmRepo = tradeViewBseCmRepo;
            _tradeViewRepo = tradeViewRepo;
            _tradeViewRepositoryEf = tradeViewRepositoryEf;
            _mapper = mapper;
        }

        public async Task LoadTradeviewFromSource()
        {
            var query = $"SELECT FillId,UserId,ExchUser,BranchId,mnmLocationId,Symbol,SymbolName,PriceType,TransactionType,FillPrice,FillSize,FillTime,FillDate,ExchangeTime,ExchOrdId,ExecutingBroker," +
                $"ExchAccountId,Source,ReportType FROM bse_cm order by FillTime desc";

            List<TradeViewBseCm> resultSet = new List<TradeViewBseCm>();
            using(var reader = await _tradeViewBseCmRepo.GetDataReaderAsync(query,connectionName: _connectionName))
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
                    return i; 
                }
            ).ToList();

            //var temp = output.Where(i => i.TradeId == "7433200").ToList();
            //temp[0].UserId = "OTRD10";
            //temp[0].TradeId = "7433201";


            //await _tradeViewRepositoryEf.AddTradeView(output.ToCollection<TradeView>());
            await _tradeViewRepositoryEf.MergeTradeView(output.ToCollection<TradeView>());

            return;
        }

    }
}
