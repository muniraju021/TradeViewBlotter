using DataAccess.Repository.Models;
using MySql.Data.MySqlClient.Memcached;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.RepositoryEF.IRepositoryEF
{
    public interface IUserViewRepository
    {
        ICollection<UserView> GetUserViews();
        UserView GetUserById(string loginName);
        UserView ValidateLogin(string loginName, string password);
        Task UpdateUserLastLogin(string userName);
        ICollection<GroupView> GetGroups();
        ICollection<DealerView> GetDealers();
        ICollection<ClientView> GetClientViews();
        Task AddUserAsync(UserView userView);
        void UpdateUserAsync(UserView userView);
        void DeleteUser(UserView userView);
        ICollection<DealerView> GetDealersByGroupName(string groupName);
        ICollection<ClientView> GetClientCodesByDealerCode(string dealerCode);
        ICollection<ClientView> GetClientCodesNotMappedToDealerCode(string dealerCode);
        ICollection<DealerView> GetDealersNotMappedToGroupName(string groupName);
    }
}
