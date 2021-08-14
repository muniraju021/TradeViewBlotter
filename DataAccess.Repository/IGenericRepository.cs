using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        Task<IEnumerable<TEntity>> GetAllEntityAsync(string spName, object parameters = null, CommandType cmdType = CommandType.Text, string connectionName = null);

        Task<IEnumerable<T>> GetAllEntityAsync<T>(string spName, object parameters = null, CommandType cmdType = CommandType.Text, string connectionName = null);

        Task<T> GetEntityAsync<T>(string spName, object parameters = null, CommandType cmdType = CommandType.Text, string connectionName = null);

        void Add(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        Task<IDataReader> GetDataReaderAsync(string spName, object parameters = null, CommandType cmdType = CommandType.Text, string connectionName = null);

        Task<int> ExcecuteNonQueryAsync(string spName, object parameters = null, CommandType cmdType = CommandType.Text, string connectionName = null, int commandTimeout = 30);
    }
}
