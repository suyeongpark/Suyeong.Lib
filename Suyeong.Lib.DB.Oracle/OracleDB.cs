using System.Data;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace Suyeong.Lib.DB.Oracle
{
    public static class OracleDB
    {
        public static string GetDbConStr(string id, string password, string dataSource)
        {
            return $"Persist Security Info=False;User ID={id};Password={password};Data Source={dataSource};Min Pool Size=10;Max Pool Size=50;Connection Lifetime=120;";
        }

        public static string GetDataSingle(string conStr, string query, OracleParameter[] parameters = null)
        {
            object scalar = null;

            using (OracleConnection conn = new OracleConnection(conStr))
            using (OracleCommand cmd = new OracleCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (OracleParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                scalar = cmd.ExecuteScalar();
            }

            return scalar?.ToString();
        }

        public static DataTable GetDataTable(string conStr, string query, OracleParameter[] parameters = null)
        {
            DataTable dataTable = new DataTable();

            using (OracleConnection conn = new OracleConnection(conStr))
            using (OracleCommand cmd = new OracleCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (OracleParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                using (OracleDataAdapter adapter = new OracleDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;
        }

        public static DataSet GetDataSet(string conStr, string query, OracleParameter[] parameters = null)
        {
            DataSet dataSet = new DataSet();

            using (OracleConnection conn = new OracleConnection(conStr))
            using (OracleCommand cmd = new OracleCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (OracleParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                using (OracleDataAdapter adapter = new OracleDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataSet);
                }
            }

            return dataSet;
        }

        public static bool SetQuery(string conStr, string query, OracleParameter[] parameters = null, int bindCount = 0)
        {
            using (OracleConnection conn = new OracleConnection(conStr))
            using (OracleCommand cmd = new OracleCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (OracleParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                if (bindCount > 0)
                {
                    cmd.ArrayBindCount = bindCount;
                }

                cmd.ExecuteNonQuery();
            }

            return true;
        }

        async public static Task<string> GetDataSingleAsync(string conStr, string query, OracleParameter[] parameters = null)
        {
            object scalar = null;

            using (OracleConnection conn = new OracleConnection(conStr))
            using (OracleCommand cmd = new OracleCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (OracleParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                scalar = await cmd.ExecuteScalarAsync();
            }

            return scalar?.ToString();
        }

        async public static Task<DataTable> GetDataTableAsync(string conStr, string query, OracleParameter[] parameters = null)
        {
            return await Task.Run<DataTable>(() => GetDataTable(conStr: conStr, query: query, parameters: parameters));
        }

        async public static Task<DataSet> GetDataSetAsync(string conStr, string query, OracleParameter[] parameters = null)
        {
            return await Task.Run<DataSet>(() => GetDataSet(conStr: conStr, query: query, parameters: parameters));
        }

        async public static Task<bool> SetQueryAsync(string conStr, string query, OracleParameter[] parameters = null, int bindCount = 0)
        {
            using (OracleConnection conn = new OracleConnection(conStr))
            using (OracleCommand cmd = new OracleCommand(query, conn))
            {
                conn.Open();

                if (parameters != null)
                {
                    foreach (OracleParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }

                if (bindCount > 0)
                {
                    cmd.ArrayBindCount = bindCount;
                }

                await cmd.ExecuteNonQueryAsync();
            }

            return true;
        }
    }
}
