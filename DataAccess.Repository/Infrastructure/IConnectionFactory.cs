using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataAccess.Repository.Infrastructure
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection(string connectionName = null);
    }
}
