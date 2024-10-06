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
    public partial class DataRepository : DataJSONServices, IDataRepository
    {
        public IQueryPattern _queryPattern;
        public virtual Dictionary<string, string> AddParameter(string[] values = null)
        {
            var parameter = new Dictionary<string, string>();
            int i = 1;
            if (values.Length > 0)
            {
                foreach (var data in values)
                {
                    parameter.Add($"@param{i}", data);
                    i++;
                }
            }

            return parameter;
        } 
        public virtual IQueryPattern AddQuery(string query, Dictionary<string, string> parameters)
        {
            _queryPattern = new QueryPattern();
            _queryPattern.Query = query;
            _queryPattern.Parameters.Add(parameters);
            return _queryPattern;
        }
        public virtual T GetDataOneRowColum<T>(string query, Dictionary<string, string> parameter = null)
        {
            try
            {
                var data = "";
                using (SqlConnection connection = new SqlConnection(_databaseConnection))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
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
            catch (Exception)
            {
                throw;
            }
        }
        public virtual DataRow GetDataRow(string query, Dictionary<string, string> parameter = null)
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
                         dataAdapter.Fill(dataTable);

                        DataRow row = dataTable.Rows[0];
                        return row;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public virtual DataSet GetDataSet(string query, Dictionary<string, string> parameter)
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
                        dataAdapter.Fill(dataSet);
                        return dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public virtual DataTable GetDataTable(string query, Dictionary<string, string> parameter=null)
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
                        dataAdapter.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual (DataTable DataTable, int TotalCount) GetDataTable(string query, Dictionary<string, string> parameter = null, SqlParameter totalCountParameter = null)
        {
            try
            {
                using (SqlConnection obcon = new SqlConnection(_databaseConnection))
                {
                    using (SqlCommand command = new SqlCommand(query, obcon))
                    {
                        // Add the parameters to the command
                        if (parameter != null && parameter.Count > 0)
                        {
                            foreach (var item in parameter)
                            {
                                command.Parameters.AddWithValue(item.Key, item.Value);
                            }
                        }

                        // Add the output parameter if provided
                        if (totalCountParameter != null)
                        {
                            command.Parameters.Add(totalCountParameter);
                        }

                        // Create a data adapter
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            dataAdapter.Fill(dt);
                            return (dt, totalCountParameter != null ? (int)totalCountParameter.Value : 0); // Return the DataTable and the total count
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveChanges(List<IQueryPattern> queryPatterns)
        {
            try
            {
                using (SqlConnection  connection = new SqlConnection(_databaseConnection))
                {
                    connection.Open();
                    SqlTransaction transaction;
                    transaction = connection.BeginTransaction("transection");
                    try
                    {
                        using (SqlCommand cmd = connection.CreateCommand())
                        {
                            cmd.Transaction = transaction;
                            foreach(var data in queryPatterns)
                            {
                                cmd.CommandText = data.Query;
                                if(data.Parameters.Count>0 && data.Parameters != null)
                                {
                                    cmd.Parameters.Clear();
                                    foreach (var parameter in data.Parameters)
                                    {
                                        foreach(var item in parameter)
                                        {
                                            cmd.Parameters.AddWithValue(item.Key, item.Value);
                                        }

                                    }
                                }
                                // HACK: THIS CODE USE ONLY GENERATE RAW QUERY WHEN ANYOUNE NEED
                              //  string query = cmd.CommandText;
                               // foreach (SqlParameter p in cmd.Parameters)
                              //  {
                              //      query = query.Replace(p.ParameterName, p.Value.ToString());
                              //  }
                                cmd.ExecuteNonQuery();
                            }
                            // Attempt to commit the transection
                            transaction.Commit();
                            connection.Close();
                        }
                    }
                    catch(Exception ex1)
                    {
                        transaction.Rollback();
                        throw ex1;
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
