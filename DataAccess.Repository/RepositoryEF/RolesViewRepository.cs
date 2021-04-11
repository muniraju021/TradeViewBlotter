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
        public ICollection<RoleView> GetRoles()
        {
            return _db.RoleViews.ToList();
        }
    }
}
