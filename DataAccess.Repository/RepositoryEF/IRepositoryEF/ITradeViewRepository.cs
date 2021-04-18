using DataAccess.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository.RepositoryEF.IRepositoryEF
{
    public interface ITradeViewRepository
    {
        ICollection<TradeView> GetTradeViews();

        Task AddTradeView(ICollection<TradeView> lstTradeViews);

        Task MergeTradeView(ICollection<TradeView> lstTradeViews);

    }
}
