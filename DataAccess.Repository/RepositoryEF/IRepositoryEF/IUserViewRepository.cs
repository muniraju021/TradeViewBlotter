using DataAccess.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository.RepositoryEF.IRepositoryEF
{
    public interface IUserViewRepository
    {
        ICollection<UserView> GetUserViews();

        UserView GetUserById(string loginName);
    }
}
