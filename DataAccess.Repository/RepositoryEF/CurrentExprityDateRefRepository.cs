using DataAccess.Repository.Data;
using DataAccess.Repository.Models;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DataAccess.Repository.RepositoryEF
{
    public class CurrentExprityDateRefRepository : ICurrentExprityDateRefRepository
    {
        private readonly ApplicationDbContext _db;
      
        public CurrentExprityDateRefRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public ICollection<CurrentExpirtyRefView> GetRoles()
        {
            return _db.CurrentExpirtyRefViews.ToList();
        }
    }
}
