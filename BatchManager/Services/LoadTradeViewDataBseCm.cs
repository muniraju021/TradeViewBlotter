﻿using DataAccess.Repository.LogServices;
using DataAccess.Repository.Repositories;
using log4net;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace BatchManager.Services
{
    public class LoadTradeViewDataBseCm : ILoadTradeviewData
    {
        private readonly ITradeViewBseCmRepository _tradeViewBseCmRepository;
        private readonly ILog _logger = LogService.GetLogger(typeof(LoadTradeViewDataBseCm));
        public static bool isSyncDataStarted = false;
        private readonly IGreekBseCmRepository _greekBseCmRepository;

        public LoadTradeViewDataBseCm(ITradeViewBseCmRepository tradeViewBseCmRepository, IGreekBseCmRepository greekBseCmRepository)
        {
            _tradeViewBseCmRepository = tradeViewBseCmRepository;
            _greekBseCmRepository = greekBseCmRepository;
        }

        //public Task LoadBseCmDataFromSourceDb()
        //{
        //    try
        //    {
        //        _logger.Info($"Inside LoadBseCmDataFromSourceDb");
        //        var cts = new CancellationTokenSource();
        //        if (!LoadTradeViewDataBseCm.isSyncDataStarted)
        //        {
        //            _logger.Info($"Intializing AutoSync");
        //            LoadTradeViewDataBseCm.isSyncDataStarted = true;
        //            Task t = Task.Factory.StartNew(
        //            async () =>
        //            {
        //                while (LoadTradeViewDataBseCm.isSyncDataStarted)
        //                {
        //                    cts.Token.ThrowIfCancellationRequested();
        //                    try
        //                    {
        //                        await _tradeViewBseCmRepository.LoadTradeviewFromSource(isDeltaLoadRequested:true);
        //                        await Task.Delay(60000, cts.Token);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        _logger.Error($"Exception in LoadNseCmDataFromSourceDb ", ex);
        //                    }
        //                }
        //                _logger.Info($"LoadBseCmDataFromSourceDb: Auto Sync BSE_CM data Stopped");
        //            }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default).Unwrap();
        //            return t;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error($"Exception in LoadNseCmDataFromSourceDb", ex);
        //    }
        //    return default(Task);
        //}

        public async Task LoadBseCmDataFromSourceDb()
        {
            try
            {
                _logger.Info($"LoadBseCmDataFromSourceDb Started");
                await _tradeViewBseCmRepository.LoadTradeviewFromSource(isDeltaLoadRequested: true);
            }
            catch (Exception ex)
            {
                _logger.Error($"LoadTradeViewDataBseCm: Exception in LoadBseCmDataFromSourceDb", ex);
            }
        }

        public async Task LoadTradeviewFulDataFromSource()
        {
            try
            {
                _logger.Info($"LoadBseCmFullDataFromSourceDb Started");
                await _tradeViewBseCmRepository.LoadTradeviewFulDataFromSource();
            }
            catch (Exception ex)
            {
                _logger.Error($"LoadTradeViewDataBseCm: Exception in LoadTradeviewFulDataFromSource", ex);
            }
        }

        public async Task LoadBseCmDataFromGreek()
        {
            try
            {
                _logger.Info($"LoadBseCmDataFromGreek Started");
                await _greekBseCmRepository.LoadTradeviewFromSource(isDeltaLoadRequested: true);
            }
            catch (Exception ex)
            {

                _logger.Error($"LoadBseCmDataFromGreek: Exception in LoadBseCmDataFromGreek", ex);
            }
        }
    }
}
