using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BatchManager.Services
{
    public interface ILoadTradeviewDataNseCm
    {
        Task LoadNseCmDataFromSourceDb();
    }
}
