﻿using DataAccess.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TraderBlotter.Api.Data;
using TraderBlotter.Api.Models;
using TraderBlotter.Api.Repository.IRepository;

namespace TraderBlotter.Api.Repository
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

        public ICollection<TradeView> GetTradeViews()
        {
            return _db.TradeViews.OrderByDescending(i => i.TradeTime).ToList();
        }

    }
}
