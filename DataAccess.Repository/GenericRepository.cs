using Dapper;
using DataAccess.Repository.Infrastructure;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IDbConnection connection;

        public GenericRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TEntity>> GetAllEntityAsync(string spName,object parameters=null,CommandType cmdType=CommandType.Text)
        {
            using(var conn = _connectionFactory.GetConnection)
            {
                var res = await conn.QueryAsync<TEntity>(spName, param: parameters, commandType: cmdType);
                return res;
            }
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
