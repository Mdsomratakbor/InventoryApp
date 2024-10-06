using DataReadWriteFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReadWriteFramework
{
    public partial class DataRepository : IDataAsyncRepository
    {
        private readonly string _databaseConnection;

        public DataRepository(string connectionString)
        {
            this._databaseConnection = connectionString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual async Task<T> GetDataOneRowColumAsync<T>(string query, Dictionary<string, string> parameter = null)
        {
            try
            {
                var data = "";
                using (SqlConnection connection = new SqlConnection(_databaseConnection))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {

                        if (parameter != null && parameter.Count > 0)
                        {
                            foreach (var item in parameter)
                            {
                                cmd.Parameters.AddWithValue(item.Key, item.Value);
                            }
                        }
                        SqlDataReader reader = await Task.Run(()=> cmd.ExecuteReader());
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                data = Convert.ToString(reader[0]);
                            }
                            reader.Close();
                            connection.Close();
                        }
                        return (T)Convert.ChangeType(data, typeof(T));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual async Task<DataRow> GetDataRowAsync(string query, Dictionary<string, string> parameter = null)
        {
            try
            {
                using (SqlConnection obcon = new SqlConnection(_databaseConnection))
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, obcon))
                    {
                        if (parameter != null && parameter.Count > 0)
                        {
                            foreach (var item in parameter)
                            {
                                dataAdapter.SelectCommand.Parameters.AddWithValue(item.Key, item.Value);
                            }
                        }
                        DataTable dataTable = new DataTable();
                        await Task.Run(() => dataAdapter.Fill(dataTable));

                        DataRow row = dataTable.Rows[0];
                        return row;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual async  Task<DataSet> GetDataSetAsync(string query, Dictionary<string, string> parameter = null)
        {
            try
            {
                using (SqlConnection obcon = new SqlConnection(_databaseConnection))
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, obcon))
                    {
                        if (parameter != null && parameter.Count > 0)
                        {
                            foreach (var item in parameter)
                            {
                                dataAdapter.SelectCommand.Parameters.AddWithValue(item.Key, item.Value);
                            }
                        }
                        DataSet dataSet = new DataSet();
                        await Task.Run(() => dataAdapter.Fill(dataSet));
                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual async Task<DataTable> GetDataTableAsync(string query, Dictionary<string, string> parameter = null)
        {
            try
            {
                using (SqlConnection obcon = new SqlConnection(_databaseConnection))
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, obcon))
                    {

                        if (parameter != null && parameter.Count > 0)
                        {
                            foreach (var item in parameter)
                            {
                                dataAdapter.SelectCommand.Parameters.AddWithValue(item.Key, item.Value);
                            }
                        }
                        DataTable dt = new DataTable();
                        await Task.Run(() => dataAdapter.Fill(dt));
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> SaveChangesAsync(List<IQueryPattern> queryPatterns)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_databaseConnection))
                {
                    connection.Open();
                    SqlTransaction transaction;
                    transaction = connection.BeginTransaction("transection");
                    try
                    {
                        using (SqlCommand cmd = connection.CreateCommand())
                        {
                            cmd.Transaction = transaction;
                            foreach (var data in queryPatterns)
                            {
                                cmd.CommandText = data.Query;
                                if (data.Parameters.Count > 0 && data.Parameters != null)
                                {
                                    cmd.Parameters.Clear();
                                    foreach (var parameter in data.Parameters)
                                    {
                                        foreach (var item in parameter)
                                        {
                                            cmd.Parameters.AddWithValue(item.Key, item.Value);
                                        }

                                    }
                                }
                                // HACK: THIS CODE USE ONLY GENERATE RAW QUERY WHEN ANYOUNE NEED
                                //string query = cmd.CommandText;
                                //foreach (SqlParameter p in cmd.Parameters)
                                //{
                                //    query = query.Replace(p.ParameterName, p.Value.ToString());
                                //}
                                await cmd.ExecuteNonQueryAsync();
                            }
                            // Attempt to commit the transection
                            transaction.Commit();
                            connection.Close();
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
