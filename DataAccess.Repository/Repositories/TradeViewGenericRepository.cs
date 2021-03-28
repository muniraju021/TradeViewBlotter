using DataAccess.Repository.Data;
using DataAccess.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Repositories
{
    public class TradeViewGenericRepository : ITradeViewGenericRepository
    {
        private readonly IGenericRepository<TradeView> _tradeViewRepo;

        public TradeViewGenericRepository(IGenericRepository<TradeView> tradeViewRepo)
        {
            _tradeViewRepo = tradeViewRepo;
        }

        public async Task<IEnumerable<TradeView>> GetAllTradeViewsByPageIndex(int pageIndex, int pageSize)
        {
            //var query = $"SELECT * FROM tradeview order by TradeDate desc LIMIT {pageIndex}, {pageSize}";
            var query = $"SELECT * FROM tradeview order by TradeDate desc";
            var res = await _tradeViewRepo.GetAllEntityAsync(query);
            return res;
        }
    } 
}
