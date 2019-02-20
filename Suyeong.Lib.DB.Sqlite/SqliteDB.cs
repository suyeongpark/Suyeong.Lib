using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace Suyeong.Lib.DB.Sqlite
{
    public static class SqliteDB
    {
        public static string GetDbConStr(string dataSource, string password)
        {
            return $"Data Source={dataSource};Version=3;Pooling=True;Max Pool Size=100;Password={password};";
        }

        public static string GetDataSingle(string conStr, string query, SQLiteParameter[] parameters = null)
        {
            object scalar = null;

            using (SQLiteConnection conn = new SQLiteConnection(conStr))
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (SQLiteParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                scalar = cmd.ExecuteScalar();
            }

            return scalar?.ToString();
        }

        public static DataTable GetDataTable(string conStr, string query, SQLiteParameter[] parameters = null)
        {
            DataTable dataTable = new DataTable();

            using (SQLiteConnection conn = new SQLiteConnection(conStr))
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (SQLiteParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;
        }

        public static DataSet GetDataSet(string conStr, string query, SQLiteParameter[] parameters = null)
        {
            DataSet dataSet = new DataSet();

            using (SQLiteConnection conn = new SQLiteConnection(conStr))
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (SQLiteParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataSet);
                }
            }

            return dataSet;
        }

        public static bool SetQuery(string conStr, string query, SQLiteParameter[] parameters = null)
        {
            using (SQLiteConnection conn = new SQLiteConnection(conStr))
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (SQLiteParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                cmd.ExecuteNonQuery();
            }

            return true;
        }

        async public static Task<string> GetDataSingleAsync(string conStr, string query, SQLiteParameter[] parameters = null)
        {
            object scalar = null;

            using (SQLiteConnection conn = new SQLiteConnection(conStr))
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (SQLiteParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                scalar = await cmd.ExecuteScalarAsync();
            }

            return scalar?.ToString();
        }

        async public static Task<DataTable> GetDataTableAsync(string conStr, string query, SQLiteParameter[] parameters = null)
        {
            return await Task.Run<DataTable>(() => GetDataTable(conStr: conStr, query: query, parameters: parameters));
        }

        async public static Task<DataSet> GetDataSetAsync(string conStr, string query, SQLiteParameter[] parameters = null)
        {
            return await Task.Run<DataSet>(() => GetDataSet(conStr: conStr, query: query, parameters: parameters));
        }

        async public static Task<bool> SetQueryAsync(string conStr, string query, SQLiteParameter[] parameters = null)
        {
            using (SQLiteConnection conn = new SQLiteConnection(conStr))
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (SQLiteParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                await cmd.ExecuteNonQueryAsync();
            }

            return true;
        }
    }
}
