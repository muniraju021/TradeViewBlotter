using DataAccess.Repository.LogServices;
using DataAccess.Repository.Repositories;
using log4net;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BatchManager.Services
{
    public class AutoSyncService : IAutoSyncService
    {
        private static ILoadTradeviewData _loadTradeviewData;
        private readonly ILog _logger = LogService.GetLogger(typeof(AutoSyncService));
        private readonly ILoadTradeviewDataNseFo _loadTradeviewDataNseFo;

        public AutoSyncService(ILoadTradeviewData loadTradeviewData, ILoadTradeviewDataNseFo loadTradeviewDataNseFo)
        {
            _loadTradeviewData = loadTradeviewData;
            _loadTradeviewDataNseFo = loadTradeviewDataNseFo;
        }

        public async Task StartAutoSyncFromSource()
        {
            try
            {
                Task.Run(() =>
                {
                    _loadTradeviewData.LoadBseCmDataFromSourceDb();
                    _logger.Info($"Auto Sync of BSE CM Data Started");
                });

                Task.Run(() =>
                {
                     _loadTradeviewDataNseFo.LoadNseFoDataFromSourceDb();
                    _logger.Info($"Auto Sync of NSE FO Data Started");
                });
               
            }
            catch (Exception ex)
            {
                _logger.Error($"AutoSyncService: Auto sync Data Initalization Failed ", ex);
            }
        }
    }
}
