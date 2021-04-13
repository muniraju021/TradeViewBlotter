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

        public ICollection<GroupView> GetGroups()
        {
            return _db.GroupViews.ToList();
        }

        public ICollection<DealerView> GetDealers()
        {
            return _db.DealerViews.ToList();
        }

        public ICollection<ClientView> GetClientViews()
        {
            return _db.ClientViews.ToList();
        }

        public UserView ValidateLogin(string loginName,string password)
        {
            return _db.UserViews.Where(i => i.LoginName == loginName && i.Password == password && i.IsActive).FirstOrDefault();
        }

        public async Task AddUserAsync(UserView userView)
        {
            await _db.UserViews.AddAsync(userView);
            _db.SaveChanges();
        }

        public void UpdateUserAsync(UserView userView)
        {
             _db.UserViews.Update(userView);
            _db.SaveChanges();
        }
    }
}
