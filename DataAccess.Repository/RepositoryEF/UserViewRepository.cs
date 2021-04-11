using DataAccess.Repository.Data;
using DataAccess.Repository.Models;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repository.RepositoryEF
{
    public class UserViewRepository : IUserViewRepository
    {
        private readonly ApplicationDbContext _db;

        public UserViewRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public UserView GetUserById(string loginName)
        {
            return _db.UserViews.Where(i => i.LoginName == loginName).FirstOrDefault();
        }

        public ICollection<UserView> GetUserViews()
        {
            return _db.UserViews.ToList();
        }
    }
}
