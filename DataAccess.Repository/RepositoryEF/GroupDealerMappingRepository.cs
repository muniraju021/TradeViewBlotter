﻿using DataAccess.Repository.Data;
using DataAccess.Repository.Models;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repository.RepositoryEF
{
    public class GroupDealerMappingRepository : IGroupDealerMappingRepository
    {
        private readonly ApplicationDbContext _db;

        public GroupDealerMappingRepository(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }

        public void MergeGroupDealerMapping(ICollection<GroupDealerMappingView> lstGroupDealerMappping)
        {
            var existingMaps = _db.GroupDealerMappingViews.Where(i => i.GroupName == lstGroupDealerMappping.ToList()[0].GroupName).ToList();

            _db.GroupDealerMappingViews.RemoveRange(existingMaps);
            var val = _db.SaveChanges();

            _db.BulkMerge(lstGroupDealerMappping, options => options.ColumnPrimaryKeyExpression = c => new { c.GroupName, c.DealerCode });
        }
    }
}
