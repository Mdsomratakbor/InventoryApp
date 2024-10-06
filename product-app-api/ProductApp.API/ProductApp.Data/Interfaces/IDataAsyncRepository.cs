using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReadWriteFramework.Interfaces
{
    public interface IDataAsyncRepository
    {
        /// <summary>
        /// This Method Return Asynchronous Single DataTable.
        /// </summary>
        /// <param name="query">Pass SQL Query</param>
        /// <param name="parameter">SQL Query all parameter pass</param>
        /// <returns>Return DataSet</returns>
        Task<DataTable> GetDataTableAsync(string query, Dictionary<string, string> parameter = null);

        /// <summary>
        /// This Method Return Asynchronous Multiple DataTable. If you need work with multiple data table on one method then use this method.
        /// </summary>
        /// <param name="query">Pass SQL Query</param>
        /// <param name="parameter">SQL Query all parameter pass</param>
        /// <returns>Return DataSet</returns>
        Task<DataSet> GetDataSetAsync(string query, Dictionary<string, string> parameter = null);

        /// <summary>
        /// This Method Return Asynchronous Single DataTable Row. If you need work with Single Row  then use this method.
        /// </summary>
        /// <param name="query">Pass SQL Query</param>
        /// <param name="parameter">SQL Query all parameter pass</param>
        /// <returns>Return DataSet</returns>
        Task<DataRow> GetDataRowAsync(string query, Dictionary<string, string> parameter = null);

        /// <summary>
        /// This Method Return Asynchronous One Row and One Column.
        /// </summary>
        /// <param name="query">Pass SQL Query</param>
        /// <param name="parameter">SQL Query all parameter pass</param>
        /// <returns>Return DataSet</returns>
        Task<T> GetDataOneRowColumAsync<T>(string query, Dictionary<string, string> parameter = null);

        /// <summary>
        ///  All Command sql query pass this method.
        /// </summary>
        /// <param name="queryPatterns">Many query and parameter pass this parameter</param>
        /// <returns>This method return bool value True or False</returns>
        Task<bool> SaveChangesAsync(List<IQueryPattern> queryPatterns);

    }
}
