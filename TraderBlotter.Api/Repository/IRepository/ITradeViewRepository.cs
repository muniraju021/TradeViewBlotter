using DataAccess.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TraderBlotter.Api.Models;

namespace TraderBlotter.Api.Repository.IRepository
{
    public interface ITradeViewRepository
    {
        ICollection<TradeView> GetTradeViews();
    }
}
