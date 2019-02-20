using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Suyeong.Lib.DB.MsSql
{
    public static class MsSqlDB
    {
        public static string GetDbConStr(string id, string password, string serverName, string dbName)
        {
            return $"Persist Security Info=False;User ID={id};Password={password};Server={serverName};Database={dbName};";
        }

        public static string GetDataSingle(string conStr, string query, SqlParameter[] parameters = null)
        {
            object scalar = null;

            using (SqlConnection conn = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (SqlParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                scalar = cmd.ExecuteScalar();
            }

            return scalar?.ToString();
        }

        public static DataTable GetDataTable(string conStr, string query, SqlParameter[] parameters = null)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (SqlParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;
        }

        public static DataSet GetDataSet(string conStr, string query, SqlParameter[] parameters = null)
        {
            DataSet dataSet = new DataSet();

            using (SqlConnection conn = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (SqlParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataSet);
                }
            }

            return dataSet;
        }

        public static bool SetQuery(string conStr, string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (SqlParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                cmd.ExecuteNonQuery();
            }

            return true;
        }

        public static bool SetQuery(string conStr, DataTable table)
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName = table.TableName;

                    foreach (DataColumn column in table.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }

                    bulkCopy.WriteToServer(table);
                }
            }

            return true;
        }

        async public static Task<string> GetDataSingleAsync(string conStr, string query, SqlParameter[] parameters = null)
        {
            object scalar = null;

            using (SqlConnection conn = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (SqlParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                scalar = await cmd.ExecuteScalarAsync();
            }

            return scalar?.ToString();
        }

        async public static Task<DataTable> GetDataTableAsync(string conStr, string query, SqlParameter[] parameters = null)
        {
            return await Task.Run<DataTable>(() => GetDataTable(conStr: conStr, query: query, parameters: parameters));
        }

        async public static Task<DataSet> GetDataSetAsync(string conStr, string query, SqlParameter[] parameters = null)
        {
            return await Task.Run<DataSet>(() => GetDataSet(conStr: conStr, query: query, parameters: parameters));
        }

        async public static Task<bool> SetQueryAsync(string conStr, string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (SqlParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                await cmd.ExecuteNonQueryAsync();
            }

            return true;
        }

        async public static Task<bool> SetQueryAsync(string conStr, DataTable table)
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName = table.TableName;

                    foreach (DataColumn column in table.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }

                    await bulkCopy.WriteToServerAsync(table);
                }
            }

            return true;
        }
    }
}
