using DataAccess.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository.RepositoryEF.IRepositoryEF
{
    public class TradeViewRefRepository : ITradeViewRefRepository
    {
        private readonly ApplicationDbContext _db;

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
    }
}
