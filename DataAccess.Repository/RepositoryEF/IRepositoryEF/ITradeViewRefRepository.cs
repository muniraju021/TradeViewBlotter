using DataAccess.Repository.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.RepositoryEF.IRepositoryEF
{
    public interface ITradeViewRefRepository
    {
        Task AddTradeView(ICollection<TradeViewRef> lstTradeViews);

        Task BulkInsert(ICollection<TradeViewRef> lstTradeViews);
    }
}
