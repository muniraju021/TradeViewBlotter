using DataAccess.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository.RepositoryEF.IRepositoryEF
{
    public class TradeViewRepository : ITradeViewRepository
    {
        private readonly ApplicationDbContext _db;

        public TradeViewRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddTradeView(ICollection<TradeView> lstTradeViews)
        {
            await _db.TradeViews.AddRangeAsync(lstTradeViews);
            _db.SaveChanges();
        }

        public void MergeTradeView(ICollection<TradeView> lstTradeViews)
        {
            _db.BulkMerge(lstTradeViews, options => options.ColumnPrimaryKeyExpression = c => new { c.TradeId,c.BuySell});
        }

        public ICollection<TradeView> GetTradeViews()
        {
            return _db.TradeViews.OrderByDescending(i => i.TradeTime).ToList();
        }

        public void BulkInsert(ICollection<TradeView> lstTradeViews)
        {
            
        }

    }
}
