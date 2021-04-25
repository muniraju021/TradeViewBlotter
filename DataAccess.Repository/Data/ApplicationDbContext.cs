using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repository.Data;
using DataAccess.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<TradeView> TradeViews { get; set; }

        public DbSet<UserView> UserViews { get; set; }

        public DbSet<RoleView> RoleViews { get; set; }

        public DbSet<GroupView> GroupViews { get; set; }

        public DbSet<DealerView> DealerViews { get; set; }

        public DbSet<ClientView> ClientViews { get; set; }

        public DbSet<DealerClientMappingView> DealerClientMappingViews { get; set; }
        public DbSet<GroupDealerMappingView> GroupDealerMappingViews { get; set; }



    }
}
