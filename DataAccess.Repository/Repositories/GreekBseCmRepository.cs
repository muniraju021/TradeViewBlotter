﻿using DataAccess.Repository.LogServices;
using DataAccess.Repository.Models;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using DataAccess.Repository.Utilities;
using log4net;
using Microsoft.Extensions.Configuration;
using ServiceStack;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using DataAccess.Repository.Data;
using AutoMapper;

namespace DataAccess.Repository.Repositories
{
    public class GreekBseCmRepository : GreekRepository<TradeViewGreekBseCm>, IGreekBseCmRepository
    {
        private readonly IGenericRepository<object> _tradeViewGenericRepo;
        private readonly ITradeViewGenericRepository _tradeViewRepo;
        private readonly ITradeViewRefRepository _tradeViewRefRepository;
        private static ILog _log = LogService.GetLogger(typeof(GreekNseCmRepository));
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        private const string sourceFileConfigKey = "GreekFileBseCmSourceFilePath";
        private const string destinationFileConfigKey = "GreekFileBseCmFilePath";
        private const string BrokerId = "12562";
        private readonly string ClientCodeConst = "12562";
        private const string Source = "GREEK_NSE_CM";

        public GreekBseCmRepository(IGenericRepository<object> tradeViewGenericRepo, ITradeViewGenericRepository tradeViewRepo,
            ITradeViewRefRepository tradeViewRefRepository, IConfiguration configuration, IFileHelper fileHelper, IMapper mapper)
            : base(fileHelper,_log)
        {
            _tradeViewGenericRepo = tradeViewGenericRepo;
            _tradeViewRepo = tradeViewRepo;
            _tradeViewRefRepository = tradeViewRefRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task LoadTradeviewFromSource(DateTime dateVal = default, bool isDeltaLoadRequested = false)
        {
            _log.Info($"GreekBseCmRepository: LoadTradeviewFromSource Starting Params: isDeltaLoadRequested: {isDeltaLoadRequested}");
            var sourceFilePath = _configuration.GetValue<string>(sourceFileConfigKey).ToString();
            var destinationFilePath = _configuration.GetValue<string>(destinationFileConfigKey).ToString();

            if (isDeltaLoadRequested)
            {
                var lst = GetDataFromSource(sourceFilePath, destinationFilePath);
                await LoadTradeViewRefTable(lst);
                _log.Info($"GreekBseCmRepository: LoadTradeviewFromSource Finished Params- isDeltaRequested:{isDeltaLoadRequested} Result-{lst?.Count}");
            }

            _log.Info($"GreekBseCmRepository: LoadTradeviewFromSource Finsihed Params: isDeltaLoadRequested:{isDeltaLoadRequested}");
        }
        
        public async Task LoadTradeviewFulDataFromSource()
        {
            _log.Info($"GreekBseCmRepository: LoadTradeviewFulDataFromSource Starting");
            var sourceFilePath = _configuration.GetValue<string>(sourceFileConfigKey).ToString();
            var destinationFilePath = _configuration.GetValue<string>(destinationFileConfigKey).ToString();
            var lst = GetDataFromSource(sourceFilePath, destinationFilePath,true);
            await LoadTradeViewRefTable(lst);
            _log.Info($"GreekBseCmRepository: LoadTradeviewFulDataFromSource Finished Result-{lst.Count}");
        }

        private async Task LoadTradeViewRefTable(List<TradeViewGreekBseCm> tradeViewGreekBseCms)
        {
            if(tradeViewGreekBseCms?.Count > 0)
            {
                var output = new List<TradeViewRef>();
                foreach (var item in tradeViewGreekBseCms)
                {
                    output.Add(_mapper.Map<TradeViewRef>(item));
                }
                var guid = Guid.NewGuid().ToString();

                output = output.Select(i =>
                {
                    i.BrokerId = BrokerId;
                    i.StockName = i.SymbolName;
                    i.ProClient = i.ParticipantId != ClientCodeConst ? "CLI" : "PRO";
                    i.ExchangeName = Constants.GreekBseCmExchangeName;
                    i.TradeDate = i.TradeDateTime;
                    i.TradeTime = i.TradeDateTime;
                    i.BuySell = i.BuySell == "1" ? "Buy" : "Sell";                    
                    i.Source = Source;
                    i.Guid = guid;
                    i.ComputeTotalPriceValue();
                    return i;
                }
               ).ToList();

                _log.Info($"GreekBseCmRepository: LoadTradeviewRefTable Ref table Insertion Starting - {output.Count}");
                await _tradeViewRefRepository.AddTradeView(output.ToCollection<TradeViewRef>());
                _log.Info($"GreekBseCmRepository: LoadTradeviewRefTable Ref table Finished Starting - {output.Count}");

                await _tradeViewRepo.SyncWithTradeViewRefTable(guid);
                _log.Info($"GreekBseCmRepository: LoadTradeviewRefTable - Greek Nse CM Syncing Completed - {output.Count}");
            }
        }
    }
}
