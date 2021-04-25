using DataAccess.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.RepositoryEF.IRepositoryEF
{
    public interface IDealerClientMappingRepository
    {
        void MergeDealerClientMapping(ICollection<DealerClientMappingView> lstDealerClientMappping);
    }
}
