using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TraderBlotter.Api.Models.Dto
{
    public class TradeStatsDto
    {
        public int NseCount { get; set; }
        public int BseCount { get; set; }
        public int TotalCount { get; set; }
    }
}
