using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Transactions;
using log4net;

namespace DAL
{
	public class DataAccess
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static SqlConnection CreateConnection(string connString)
		{
			return new SqlConnection(connString);
		}

		public static object ExecuteScalar(string connString, string sql, CommandType commandType = CommandType.Text, params SqlParameter[] parameters)
		{

			SqlConnection cn = CreateConnection(connString);

			using (cn)
			{
				cn.Open();

				SqlTransaction transaction = cn.BeginTransaction();

				using (transaction)
				{
					try
					{
						SqlCommand cmd = new SqlCommand(sql, cn, transaction);
						cmd.CommandType = commandType;
						if (parameters.Length > 0)
							cmd.Parameters.AddRange(parameters);
						object oRet = cmd.ExecuteScalar();
						transaction.Commit();
						return oRet;
					}
					catch (TransactionException ex)
					{
						transaction.Rollback();
						log.Error(ex.Message, ex);
						throw new Exception(ex.Message);
					}
					catch (SqlException ex)
					{
						transaction.Rollback();
						log.Error(ex.Message, ex);
						throw new Exception(ex.Message);
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						log.Error(ex.Message, ex);
						throw new Exception(ex.Message);
					}


				}
			}
		}



		public static int ExecuteNonQuery(string connString, string sql, CommandType commandType = CommandType.Text, params SqlParameter[] parameters)
		{
			SqlConnection cn = CreateConnection(connString);

			using (cn)
			{
				cn.Open();

				SqlTransaction transaction = cn.BeginTransaction();

				using (transaction)
				{
					try
					{
						SqlCommand cmd = new SqlCommand(sql, cn, transaction);
						cmd.CommandType = commandType;
						if (parameters != null && parameters.Any())
							cmd.Parameters.AddRange(parameters);
						var iRet = cmd.ExecuteNonQuery();
						transaction.Commit();
						return iRet;
					}
					catch (TransactionException ex)
					{
						transaction.Rollback();
						log.Error(ex.Message, ex);
						throw new Exception(ex.Message);
					}
					catch (SqlException ex)
					{
						transaction.Rollback();
						log.Error(ex.Message, ex);
						throw new Exception(ex.Message);
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						log.Error(ex.Message, ex);
						throw new Exception(ex.Message);
					}

				}
			}
		}

		public static DataSet CreateDataSet(string connString, string sql, CommandType commandType = CommandType.Text, string datasetName = null, params SqlParameter[] parameters)
		{
			try
			{
				SqlConnection cn = CreateConnection(connString);
				SqlDataAdapter adapter = new SqlDataAdapter(sql, cn);
				adapter.SelectCommand.CommandType = commandType;
				if (parameters != null && parameters.Any())
					adapter.SelectCommand.Parameters.AddRange(parameters);
				DataSet ds = new DataSet();
				if (!string.IsNullOrEmpty(datasetName)) ds.DataSetName = datasetName;
				adapter.Fill(ds);
				return ds;
			}
			catch (SqlException ex)
			{
				log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}
			catch (Exception ex)
			{
				log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}

		}


	}
}
