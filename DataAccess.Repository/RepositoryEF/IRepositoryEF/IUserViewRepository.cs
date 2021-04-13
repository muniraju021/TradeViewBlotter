using DataAccess.Repository.Models;
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

        ICollection<GroupView> GetGroups();
        ICollection<DealerView> GetDealers();
        ICollection<ClientView> GetClientViews();
        Task AddUserAsync(UserView userView);
        void UpdateUserAsync(UserView userView);
    }
}
