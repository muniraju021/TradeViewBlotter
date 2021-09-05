using DataAccess.Repository.Data;
using DataAccess.Repository.LogServices;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository.RepositoryEF.IRepositoryEF
{
    public class TradeViewRefRepository : ITradeViewRefRepository, IDisposable
    {
        private readonly ApplicationDbContext _db;
        private static ILog _log = LogService.GetLogger(typeof(TradeViewRefRepository));

        public TradeViewRefRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddTradeView(ICollection<TradeViewRef> lstTradeViews)
        {
            await _db.TradeViewRefs.AddRangeAsync(lstTradeViews);
            _db.SaveChanges();
        }

        public async Task BulkInsert(ICollection<TradeViewRef> lstTradeViews)
        {
            await _db.AddRangeAsync(lstTradeViews);
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _log.Info($"TradeViewRefRepository: Dispose Called.....");
        }
    }
}
