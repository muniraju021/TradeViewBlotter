using DataAccess.Repository.LogServices;
using DataAccess.Repository.Repositories;
using log4net;
using Org.BouncyCastle.Crypto.Modes.Gcm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BatchManager.Services
{
    public class AutoSyncService : IAutoSyncService
    {
        private static ILoadTradeviewData _loadTradeviewData;
        private readonly ILog _logger = LogService.GetLogger(typeof(AutoSyncService));
        private readonly ILoadTradeviewDataNseFo _loadTradeviewDataNseFo;
        private readonly ILoadTradeviewDataNseCm _loadTradeviewDataNseCm;

        public AutoSyncService(ILoadTradeviewData loadTradeviewData, ILoadTradeviewDataNseFo loadTradeviewDataNseFo, ILoadTradeviewDataNseCm loadTradeviewDataNseCm)
        {
            _loadTradeviewData = loadTradeviewData;
            _loadTradeviewDataNseFo = loadTradeviewDataNseFo;
            _loadTradeviewDataNseCm = loadTradeviewDataNseCm;
        }

        public async Task StartAutoSyncFromSource()
        {
            try
            {
                var tasklst = new List<Task>();
                var isSyncDataStarted = true;
                var cts = new CancellationTokenSource();
                _logger.Info("AutoSyncService: StartAutoSyncFromSource Initated");

                while (isSyncDataStarted)
                {                    
                    try
                    {                        
                        await _loadTradeviewData.LoadBseCmDataFromSourceDb();
                        await _loadTradeviewDataNseFo.LoadNseFoDataFromSourceDb();
                        await _loadTradeviewDataNseCm.LoadNseCmDataFromSourceDb();
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Exception in LoadNseCmDataFromSourceDb ", ex);
                    }
                    await Task.Delay(30000, cts.Token);
                }

                //tasklst.Add(_loadTradeviewData.LoadBseCmDataFromSourceDb());
                //_logger.Info($"Auto Sync of BSE CM Data Started");

                //tasklst.Add(_loadTradeviewDataNseFo.LoadNseFoDataFromSourceDb());
                //_logger.Info($"Auto Sync of NSE FO Data Started");

                //Task.WaitAll(tasklst.ToArray());

                _logger.Info($"AutoSyncService - Exiting");

            }
            catch (Exception ex)
            {
                _logger.Error($"AutoSyncService: Auto sync Data Initalization Failed ", ex);
            }
        }
    }
}
