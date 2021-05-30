using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Repositories
{
    public interface ITradeViewNseFoRepository
    {
        Task LoadTradeviewFromSource(DateTime dateTimeVal = default(DateTime), bool isDeltaLoadRequested = false);
    }
}
