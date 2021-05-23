using DataAccess.Repository.Data;
using DataAccess.Repository.Models;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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

        public UserView ValidateLogin(string loginName, string password)
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
            _db.UserViews.Attach(userView);
            _db.Entry(userView).Property(i => i.Password).IsModified = false;
            _db.Entry(userView).Property(i => i.EmailId).IsModified = true;
            _db.Entry(userView).Property(i => i.RoleId).IsModified = true;
            _db.Entry(userView).Property(i => i.IsActive).IsModified = true;
            _db.Entry(userView).Property(i => i.DealerCode).IsModified = true;
            _db.Entry(userView).Property(i => i.ClientCode).IsModified = true;
            _db.Entry(userView).Property(i => i.GroupName).IsModified = true;

            //_db.UserViews.Update(userView);
            _db.SaveChanges();
        }

        public void DeleteUser(UserView userView)
        {
            var current = _db.UserViews.Find(userView.LoginName);
            if (current != null)
            {
                current.IsActive = userView.IsActive;
                _db.UserViews.Update(current);
            }
            int res = _db.SaveChanges();
        }

        public ICollection<DealerView> GetDealersByGroupName(string groupName)
        {
            return _db.GroupDealerMappingViews.Where(i => i.GroupName == groupName).ToList().Select(j => new DealerView { DealerCode = j.DealerCode }).ToList();
        }

        public ICollection<ClientView> GetClientCodesByDealerCode(string dealerCode)
        {
            return _db.DealerClientMappingViews.Where(i => i.DealerCode == dealerCode).Select(j => new ClientView { ClientCode = j.ClientCode }).ToList();
        }
    }
}
