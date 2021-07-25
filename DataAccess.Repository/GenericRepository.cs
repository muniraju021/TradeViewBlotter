using Dapper;
using DataAccess.Repository.Infrastructure;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private IDbConnection GetDbConnection(string connectionName)
        {
            var con = string.IsNullOrWhiteSpace(connectionName) ? _connectionFactory.GetConnection() : _connectionFactory.GetConnection(connectionName);
            return con;
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

        public async Task<IEnumerable<TEntity>> GetAllEntityAsync(string spName, object parameters = null, CommandType cmdType = CommandType.Text, string connectionName = null)
        {
            using (var con = GetDbConnection(connectionName))
            {
                var res = await con.QueryAsync<TEntity>(spName, param: parameters, commandType: cmdType);
                return res;
            }
        }

        public async Task<IDataReader> GetDataReaderAsync(string spName, object parameters = null, CommandType cmdType = CommandType.Text, string connectionName = null)
        {
            var con = GetDbConnection(connectionName);
            var reader = await con.ExecuteReaderAsync(spName, commandType: cmdType);
            return reader;
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAllEntityAsync<T>(string spName, object parameters = null, CommandType cmdType = CommandType.Text, string connectionName = null)
        {
            using (var con = GetDbConnection(connectionName))
            {
                var res = await con.QueryAsync<T>(spName, param: parameters, commandType: cmdType);
                return res;
            }
        }

        public async Task<int> ExcecuteNonQueryAsync(string spName, object parameters = null, CommandType cmdType = CommandType.Text, string connectionName = null)
        {
            using (var con = GetDbConnection(connectionName))
            {
                var res = await con.ExecuteAsync(spName, param: parameters, commandType: cmdType);
                return res;
            }
        }
    }
}
