using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataAccess.Repository.Infrastructure
{
    public interface IConnectionFactory
    {
        public IDbConnection GetConnection{ get; }
    }
}
