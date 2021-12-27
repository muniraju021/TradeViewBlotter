using DataAccess.Repository.LogServices;
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
    public class LoadTradeViewDataNseCm : ILoadTradeviewDataNseCm
    {
        private readonly ITradeViewNseCmRepository _tradeViewNseCmRepository;
        private readonly ILog _logger = LogService.GetLogger(typeof(LoadTradeViewDataNseCm));
        public static bool isSyncDataStarted = false;
        private readonly IGreekNseCmRepository _greekNseCmRepository;

        public LoadTradeViewDataNseCm(ITradeViewNseCmRepository tradeViewNseCmRepository, IGreekNseCmRepository greekNseCmRepository)
        {
            _tradeViewNseCmRepository = tradeViewNseCmRepository;
            _greekNseCmRepository = greekNseCmRepository;
        }

        public async Task LoadNseCmDataFromSourceDb()
        {
            try
            {
                _logger.Info($"Inside LoadNseCmDataFromSourceDb");
                await _tradeViewNseCmRepository.LoadTradeviewFromSource(isDeltaLoadRequested: true);
            }
            catch (Exception ex)
            {
                _logger.Error($"LoadTradeViewDataNseCm: LoadNseCmDataFromSourceDb - Exception in LoadNseCmDataFromSourceDb - {ex}");
            }
        }

        public async Task LoadNseCmDataFromGreek()
        {
            try
            {
                _logger.Info($"LoadTradeViewDataNseCm: LoadNseCmDataFromGreek - Loading NSE CM Data from Greek File");
                await _greekNseCmRepository.LoadTradeviewFromSource(isDeltaLoadRequested: true);
            }
            catch (Exception ex)
            {
                _logger.Error($"LoadTradeViewDataNseCm: LoadNseCmDataFromGreek - Exception in LoadNseCmDataFromSourceDb - {ex}");
            }
        }
    }
}
