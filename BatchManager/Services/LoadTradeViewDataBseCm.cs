using DataAccess.Repository.LogServices;
using DataAccess.Repository.Repositories;
using log4net;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BatchManager.Services
{
    public class LoadTradeViewDataBseCm : ILoadTradeviewData
    {
        private readonly ITradeViewBseCmRepository _tradeViewBseCmRepository;
        private readonly ILog _logger = LogService.GetLogger(typeof(LoadTradeViewDataBseCm));

        public LoadTradeViewDataBseCm(ITradeViewBseCmRepository tradeViewBseCmRepository)
        {
            _tradeViewBseCmRepository = tradeViewBseCmRepository;
            LoadBseCmDataFromSourceDb();
        }

        public Task LoadBseCmDataFromSourceDb()
        {
            try
            {
                var cts = new CancellationTokenSource();
                Task t = Task.Factory.StartNew(
                async () => {
                    while (true)
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        try
                        {
                            await _tradeViewBseCmRepository.LoadTradeviewFromSource(true);
                            await Task.Delay(1000, cts.Token);
                        }
                        catch (TaskCanceledException ex) 
                        {
                            _logger.Error($"Exception in LoadNseCmDataFromSourceDb ", ex);
                        }
                    }
                }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default).Unwrap();
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception in LoadNseCmDataFromSourceDb", ex);
            }
            return default(Task);
        }
    }
}
