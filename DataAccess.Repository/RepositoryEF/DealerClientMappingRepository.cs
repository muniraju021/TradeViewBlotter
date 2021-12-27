using DataAccess.Repository.Data;
using DataAccess.Repository.Models;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.RepositoryEF
{
    public class DealerClientMappingRepository : IDealerClientMappingRepository
    {
        private readonly ApplicationDbContext _db;

        public DealerClientMappingRepository(ApplicationDbContext applicationDbContext)
        {
            this._db = applicationDbContext;
        }

        public void MergeDealerClientMapping(ICollection<DealerClientMappingView> lstDealerClientMappping)
        {
            var existingMaps = _db.DealerClientMappingViews.Where(i => i.DealerCode == lstDealerClientMappping.ToList()[0].DealerCode).ToList();

            _db.DealerClientMappingViews.RemoveRange(existingMaps);
            if(lstDealerClientMappping?.Count > 0)
                _db.DealerClientMappingViews.AddRange(lstDealerClientMappping);
            var val = _db.SaveChanges();

            //_db.BulkMerge(lstDealerClientMappping, options => options.ColumnPrimaryKeyExpression = c => new { c.DealerCode, c.ClientCode });
        }
    }
}
