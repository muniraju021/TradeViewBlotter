using DataAccess.Repository.Data;
using DataAccess.Repository.Models;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repository.RepositoryEF
{
    public class RolesViewRepository : IRoleViewRepository
    {
        private readonly ApplicationDbContext _db;
        public RolesViewRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public RoleView GetRoleById(int roleId)
        {
            return _db.RoleViews.Where(i => i.RoleId == roleId).FirstOrDefault();
        }

        public ICollection<RoleView> GetRoles()
        {
            return _db.RoleViews.ToList();
        }
    }
}
