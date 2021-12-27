using DataAccess.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository.RepositoryEF.IRepositoryEF
{
    public interface IGroupDealerMappingRepository
    {
        void MergeGroupDealerMapping(ICollection<GroupDealerMappingView> lstGroupDealerMappping);
        ICollection<string> GetDealerByGroupName(string groupName);
    }
}
