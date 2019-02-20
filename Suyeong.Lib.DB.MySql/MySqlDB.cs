using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Suyeong.Lib.DB.MySql
{
    public static class MySqlDB
    {
        // mysql bulk insert는 아래 링크 참조.
        // https://dev.mysql.com/doc/refman/5.7/en/insert.html
        // Oracle이나 MsSql과 달리 별도의 파라미터가 있지는 않고 그냥 values를 여러 번 다넘긴.
        // INSERT INTO tbl_name (a,b,c) VALUES(1,2,3),(4,5,6),(7,8,9);

        public static string GetDbConStr(string server, string database, string uid, string password)
        {
            return $"Server={server};Database={database};Uid={uid};Pwd={password};";
        }

        public static string GetDataSingle(string conStr, string query, MySqlParameter[] parameters = null)
        {
            object scalar = null;

            using (MySqlConnection conn = new MySqlConnection(conStr))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (MySqlParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                scalar = cmd.ExecuteScalar();
            }

            return scalar?.ToString();
        }

        public static DataTable GetDataTable(string conStr, string query, MySqlParameter[] parameters = null)
        {
            DataTable dataTable = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(conStr))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (MySqlParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                using (MySqlDataAdapter adapter = new MySqlDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;
        }

        public static DataSet GetDataSet(string conStr, string query, MySqlParameter[] parameters = null)
        {
            DataSet dataSet = new DataSet();

            using (MySqlConnection conn = new MySqlConnection(conStr))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (MySqlParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                using (MySqlDataAdapter adapter = new MySqlDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataSet);
                }
            }

            return dataSet;
        }

        public static bool SetQuery(string conStr, string query, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection conn = new MySqlConnection(conStr))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (MySqlParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                cmd.ExecuteNonQuery();
            }

            return true;
        }

        async public static Task<string> GetDataSingleAsync(string conStr, string query, MySqlParameter[] parameters = null)
        {
            object scalar = null;

            using (MySqlConnection conn = new MySqlConnection(conStr))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (MySqlParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                scalar = await cmd.ExecuteScalarAsync();
            }

            return scalar?.ToString();
        }

        async public static Task<DataTable> GetDataTableAsync(string conStr, string query, MySqlParameter[] parameters = null)
        {
            return await Task.Run<DataTable>(() => GetDataTable(conStr: conStr, query: query, parameters: parameters));
        }

        async public static Task<DataSet> GetDataSetAsync(string conStr, string query, MySqlParameter[] parameters = null)
        {
            return await Task.Run<DataSet>(() => GetDataSet(conStr: conStr, query: query, parameters: parameters));
        }

        async public static Task<bool> SetQueryAsync(string conStr, string query, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection conn = new MySqlConnection(conStr))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (MySqlParameter param in parameters)
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
