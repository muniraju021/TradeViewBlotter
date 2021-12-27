using DataAccess.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository.RepositoryEF.IRepositoryEF
{
    public interface ICurrentExprityDateRefRepository
    {
        ICollection<CurrentExpirtyRefView> GetRoles();
    }
}
