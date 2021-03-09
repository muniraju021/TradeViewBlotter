using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repository.Data;
using Microsoft.EntityFrameworkCore;
using TraderBlotter.Api.Models;

namespace TraderBlotter.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<TradeView> TradeViews { get; set; }
    }
}
