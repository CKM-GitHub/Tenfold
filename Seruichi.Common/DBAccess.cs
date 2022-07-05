using System;
using System.Data;
using System.Data.SqlClient;

namespace Seruichi.Common
{
    public class DBAccess
    {
        private string connectionString = StaticCache.DBInfo.SQLConnString;
        private int commandTimeout = StaticCache.DBInfo.CommandTimeout;

        public DBAccess()
        {
        }

        public DataTable SelectDatatable(string sSQL, params SqlParameter[] para)
        {
            DataTable dt = new DataTable();

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var adapt = new SqlDataAdapter(sSQL, conn))
                {
                    conn.Open();
                    adapt.SelectCommand.CommandTimeout = commandTimeout;
                    adapt.SelectCommand.CommandType = CommandType.StoredProcedure;

                    if (para != null)
                    {
                        para = ChangeToDBNull(para);
                        adapt.SelectCommand.Parameters.AddRange(para);
                    }

                    adapt.Fill(dt);
                    conn.Close();
                }

                return dt;
            }
            catch (Exception ex)
            {
                WriteLog(ex, sSQL);
                throw ex;
            }
        }

        public DataSet SelectDataSet(string sSQL, params SqlParameter[] para)
        {
            DataSet dt = new DataSet();

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var adapt = new SqlDataAdapter(sSQL, conn))
                {
                    conn.Open();
                    adapt.SelectCommand.CommandTimeout = commandTimeout;
                    adapt.SelectCommand.CommandType = CommandType.StoredProcedure;

                    if (para != null)
                    {
                        para = ChangeToDBNull(para);
                        adapt.SelectCommand.Parameters.AddRange(para);
                    }

                    adapt.Fill(dt);
                    conn.Close();
                }

                return dt;
            }
            catch (Exception ex)
            {
                WriteLog(ex, sSQL);
                throw ex;
            }
        }
        public bool InsertUpdateDeleteData(string sSQL, bool useOptimisticExclusion, params SqlParameter[] para)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(sSQL, conn))
                {
                    conn.Open();
                    cmd.CommandTimeout = commandTimeout;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (para != null)
                    {
                        para = ChangeToDBNull(para);
                        cmd.Parameters.AddRange(para);
                        if (useOptimisticExclusion)
                        {
                            cmd.Parameters.Add(new SqlParameter("@OutExclusionError", SqlDbType.TinyInt) {
                                Direction = ParameterDirection.Output });
                        }
                    }

                    var transaction = conn.BeginTransaction();
                    cmd.Transaction = transaction;
                    try
                    {
                        int ret = cmd.ExecuteNonQuery();
                        if (useOptimisticExclusion)
                        {
                            if (cmd.Parameters["@OutExclusionError"].Value.ToInt16(0) > 0)
                                throw new ExclusionException();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                    //conn.Close();
                }
                return true;
            }
            catch (ExclusionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                WriteLog(ex, sSQL);
                return false;
            }
        }

        private SqlParameter[] ChangeToDBNull(SqlParameter[] para)
        {
            foreach (var p in para)
            {
                if (p.Value == null)
                {
                    p.Value = DBNull.Value;
                    p.SqlValue = DBNull.Value;
                }
                else if (p.SqlDbType == SqlDbType.VarChar)
                {
                    if (string.IsNullOrWhiteSpace(p.Value.ToString()))
                    {
                        p.Value = DBNull.Value;
                        p.SqlValue = DBNull.Value;
                    }
                    else
                    {
                        p.Value = p.Value.ToString().Replace("\t", string.Empty);
                    }
                }
            }
            return para;
        }

        private void WriteLog(Exception ex, string sql)
        {
            Logger.GetInstance().Error(ex, sql);
        }
    }
}
