using DataAccess.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository.RepositoryEF.IRepositoryEF
{
    public interface IRoleViewRepository
    {
        ICollection<RoleView> GetRoles();
        RoleView GetRoleById(int roleId);
    }
}
