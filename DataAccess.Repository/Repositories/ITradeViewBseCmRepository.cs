using DataAccess.Repository.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Repositories
{
    public interface ITradeViewBseCmRepository
    {
        Task LoadTradeviewFromSource(DateTime dateVal = default(DateTime), bool isDeltaLoadRequested = false);

        Task LoadTradeviewFulDataFromSource();

        
    }
}
