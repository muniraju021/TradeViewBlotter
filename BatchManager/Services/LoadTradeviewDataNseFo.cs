using DataAccess.Repository.LogServices;
using DataAccess.Repository.Repositories;
using log4net;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BatchManager.Services
{
    public class LoadTradeviewDataNseFo : ILoadTradeviewDataNseFo
    {
        private readonly ITradeViewNseFoRepository _tradeViewNseFoRepository;
        private readonly ILog _logger = LogService.GetLogger(typeof(LoadTradeviewDataNseFo));
        public static bool isSyncDataStarted = false;

        public LoadTradeviewDataNseFo(ITradeViewNseFoRepository tradeViewNseFoRepository)
        {
            _tradeViewNseFoRepository = tradeViewNseFoRepository;
        }

        public Task LoadNseFoDataFromSourceDb()
        {
            try
            {
                _logger.Info($"Inside LoadNseFoDataFromSourceDb");
                var cts = new CancellationTokenSource();
                if (!LoadTradeviewDataNseFo.isSyncDataStarted)
                {
                    _logger.Info($"Intializing AutoSync");
                    LoadTradeviewDataNseFo.isSyncDataStarted = true;
                    Task t = Task.Factory.StartNew(
                    async () =>
                    {
                        while (LoadTradeviewDataNseFo.isSyncDataStarted)
                        {
                            cts.Token.ThrowIfCancellationRequested();
                            try
                            {
                                await _tradeViewNseFoRepository.LoadTradeviewFromSource(isDeltaLoadRequested: true);
                                await Task.Delay(60000, cts.Token);
                            }
                            catch (TaskCanceledException ex)
                            {
                                _logger.Error($"Exception in LoadNseCmDataFromSourceDb ", ex);
                            }
                        }
                        _logger.Info($"LoadNseFoDataFromSourceDb: Auto Sync BSE_CM data Stopped");
                    }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default).Unwrap();
                    return t;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception in LoadNseCmDataFromSourceDb", ex);
            }
            return default(Task);
        }
    }
}
