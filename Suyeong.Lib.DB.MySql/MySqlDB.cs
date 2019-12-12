using System;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Suyeong.Lib.DB.MySql
{
    public static class MySqlDB
    {
        // mysql bulk insert는 아래 링크 참조.
        // https://dev.mysql.com/doc/refman/5.7/en/insert.html
        // Oracle이나 MsSql과 달리 별도의 파라미터가 있지는 않고 그냥 values를 여러 번 넘긴다.
        // insert into TABLE_NAME (a,b,c) values (1,2,3),(4,5,6),(7,8,9);

        public static string GetDbConStr(string serverIP, string databaseName, string uid, string password)
        {
            return $"Server={serverIP};Database={databaseName};Uid={uid};Pwd={password};";
        }

        public static object GetDataSingle(string conStr, string query, MySqlParameter[] parameters = null)
        {
            object scalar = null;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString: conStr))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(cmdText: query, connection: connection))
                    {
                        if (parameters != null)
                        {
                            foreach (MySqlParameter parameter in parameters)
                            {
                                command.Parameters.Add(parameter);
                            }
                        }

                        scalar = command.ExecuteScalar();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return scalar;
        }

        async public static Task<object> GetDataSingleAsync(string conStr, string query, MySqlParameter[] parameters = null)
        {
            object scalar = null;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString: conStr))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(cmdText: query, connection: connection))
                    {

                        if (parameters != null)
                        {
                            foreach (MySqlParameter parameter in parameters)
                            {
                                command.Parameters.Add(parameter);
                            }
                        }

                        scalar = await command.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return scalar;
        }

        public static DataTable GetDataTable(string conStr, string query, MySqlParameter[] parameters = null)
        {
            DataTable table = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString: conStr))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(cmdText: query, connection: connection))
                    {
                        if (parameters != null)
                        {
                            foreach (MySqlParameter parameter in parameters)
                            {
                                command.Parameters.Add(parameter);
                            }
                        }

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter())
                        {
                            adapter.SelectCommand = command;
                            adapter.Fill(table);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return table;
        }

        async public static Task<DataTable> GetDataTableAsync(string conStr, string query, MySqlParameter[] parameters = null)
        {
            return await Task.Run<DataTable>(() => GetDataTable(conStr: conStr, query: query, parameters: parameters));
        }

        public static DataSet GetDataSet(string conStr, string query, MySqlParameter[] parameters = null)
        {
            DataSet dataSet = new DataSet();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString: conStr))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(cmdText: query, connection: connection))
                    {
                        if (parameters != null)
                        {
                            foreach (MySqlParameter parameter in parameters)
                            {
                                command.Parameters.Add(parameter);
                            }
                        }

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter())
                        {
                            adapter.SelectCommand = command;
                            adapter.Fill(dataSet);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dataSet;
        }

        async public static Task<DataSet> GetDataSetAsync(string conStr, string query, MySqlParameter[] parameters = null)
        {
            return await Task.Run<DataSet>(() => GetDataSet(conStr: conStr, query: query, parameters: parameters));
        }

        public static bool SetQuery(string conStr, string query, MySqlParameter[] parameters = null)
        {
            int result = 0;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString: conStr))
                {
                    connection.Open();

                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    using (MySqlCommand command = new MySqlCommand(cmdText: query, connection: connection, transaction: transaction))
                    {
                        try
                        {
                            if (parameters != null)
                            {
                                foreach (MySqlParameter parameter in parameters)
                                {
                                    command.Parameters.Add(parameter);
                                }
                            }

                            result = command.ExecuteNonQuery();

                            if (result > 0)
                            {
                                command.Transaction.Commit();
                            }
                        }
                        catch (Exception)
                        {
                            try
                            {
                                command.Transaction.Rollback();
                            }
                            catch (MySqlException)
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result > 0;
        }

        async public static Task<bool> SetQueryAsync(string conStr, string query, MySqlParameter[] parameters = null)
        {
            int result = 0;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString: conStr))
                {
                    connection.Open();

                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    using (MySqlCommand command = new MySqlCommand(cmdText: query, connection: connection, transaction: transaction))
                    {
                        try
                        {
                            if (parameters != null)
                            {
                                foreach (MySqlParameter parameter in parameters)
                                {
                                    command.Parameters.Add(parameter);
                                }
                            }

                            result = await command.ExecuteNonQueryAsync();

                            if (result > 0)
                            {
                                command.Transaction.Commit();
                            }
                        }
                        catch (Exception)
                        {
                            try
                            {
                                command.Transaction.Rollback();
                            }
                            catch (MySqlException)
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result > 0;
        }
    }
}
