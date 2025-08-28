using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using AMSDataLoad.Models;
using AMSDataLoad.Handlers;

namespace AMSDataLoad.Data
{
    public class DataContextDapper
    {
        private string _connectionString = "";
        private string _emailConnectionString = "";

        private int timeout = 999999999;

        public DataContextDapper(IConfiguration config)
        {
            string? connectionStringResponse = config.GetConnectionString("DefaultConnection");
            string? emailConnectionStringResponse = config.GetConnectionString("EmailConnection");

            if (connectionStringResponse != null)
            {
                _connectionString = connectionStringResponse;
            }
            if (emailConnectionStringResponse != null)
            {
                _emailConnectionString = emailConnectionStringResponse;
            }

        }
        public IEnumerable<T> LoadData<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.Query<T>(sql, commandTimeout: this.timeout);
        }

        public T LoadDataSingle<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.QuerySingle<T>(sql, commandTimeout: this.timeout);
        }

        public bool ExecuteSql(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return (dbConnection.Execute(sql, commandTimeout: this.timeout) > 0);
        }

        public int ExecuteSqlWithRowCount(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.Execute(sql, commandTimeout: this.timeout);
        }


        public int ExecuteSqlWithParameters(string sql, DynamicParameters parameters)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.Execute(sql, parameters, commandTimeout: this.timeout);

        }

        public int ExecuteSqlWithParametersAsStoredProc(string sql, DynamicParameters parameters)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.Execute(sql, parameters, commandTimeout: this.timeout,commandType: CommandType.StoredProcedure);

        }

        public IEnumerable<T> LoadDataWithParameters<T>(string sql, DynamicParameters parameters)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.Query<T>(sql, parameters, commandTimeout: this.timeout);
        }
        public IEnumerable<T> LoadDataFromProcWithParameters<T>(string sql, DynamicParameters sqlParams)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                using (IDbTransaction tran = dbConnection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    var holdVal = dbConnection.Query<T>(sql, sqlParams, transaction: tran, commandTimeout: 999999999, commandType: CommandType.StoredProcedure);
                    dbConnection.Close();
                    return holdVal;
                }
            }
        }

        public T LoadDataSingleWithParameters<T>(string sql, DynamicParameters parameters)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.QuerySingle<T>(sql, parameters, commandTimeout: this.timeout);
        }

        public int ExecuteSendEmail(EmailNotification emailNotification, string sendTo)
        {
            string sql = "EXEC Emailer.spEmailQueue_Insert @EmailSubject='" + emailNotification.EmailSubject +
            "', @EmailBody='" + emailNotification.EmailBody +
            "', @SiteFrom='Inventory', @RequestType='" + emailNotification.RequestType +
            "', @EmailReadyStatus=1, @EmailTo='" + sendTo +
            "', @EmailBCC='support@abeerconsulting.com'";
            using (IDbConnection dbConnection = new SqlConnection(this._emailConnectionString))
            {
                return dbConnection.Execute(sql);
            }
        }

        public int ExecuteSendEmail(EmailNotification emailNotification, string sendTo, string format)
        {
            string sql = "EXEC Emailer.spEmailQueue_Insert @EmailSubject='" + emailNotification.EmailSubject +
            "', @EmailBody='" + emailNotification.EmailBody +
            "', @SiteFrom='Inventory', @RequestType='" + emailNotification.RequestType +
            "', @EmailReadyStatus=1, @EmailTo='" + sendTo +
            "', @EmailBCC='support@abeerconsulting.com', @EmailFormat='" + format + "'";
            using (IDbConnection dbConnection = new SqlConnection(this._emailConnectionString))
            {
                return dbConnection.Execute(sql, commandTimeout: this.timeout);
            }
        }

        /// <summary>
        /// retries parameterized sql query 5 times
        /// </summary>
        /// <param name="sql"> sql string to be executed </param>
        /// <param name="parameters"> dynamic parameters to be applied to query </param>
        /// <returns>
        /// count of changed records
        /// </returns>
        public int ExecuteSqlWithParametersRetry(string sql, DynamicParameters parameters)
        {
            bool success = false;
            int rowsAffected = 0;
            for (int i = 0; i < 5 && (!success); i++)
            {
                try
                {
                    rowsAffected = this.ExecuteSqlWithParameters(sql, parameters);
                }
                catch (Exception exception)
                {
                    Thread.Sleep(1 * 30 * 1000);
                    if (i == 4)
                    {
                        return 0;
                    }
                    continue;
                }
                success = true;
            }
            return rowsAffected;

        }

        public int ExecuteSqlWithParametersAsStoredProcRetry(string sql, DynamicParameters parameters)
        {
            bool success = false;
            int rowsAffected = 0;
            for (int i = 0; i < 5 && (!success); i++)
            {
                try
                {
                    rowsAffected = this.ExecuteSqlWithParametersAsStoredProc(sql, parameters);
                }
                catch (Exception exception)
                {
                    Thread.Sleep(1 * 30 * 1000);
                    if (i == 4)
                    {
                        return 0;
                    }
                    continue;
                }
                success = true;
            }
            return rowsAffected;

        }


        /// <summary>
        /// retries sql query 5 times
        /// </summary>
        /// <param name="sql"> sql string to be executed </param>
        /// <returns>
        /// bool if query was executed correctly
        /// </returns>
        public bool ExecuteSqlRetry(string sql)
        {
            bool success = false;
            bool queryExecuted = false;
            for (int i = 0; i < 5 && (!success); i++)
            {
                try
                {
                    queryExecuted = this.ExecuteSql(sql);
                }
                catch (Exception exception)
                {
                    Thread.Sleep(1 * 30 * 1000);
                    if (i == 4)
                    {
                        return false;
                    }
                    continue;
                }
                success = true;
            }
            return queryExecuted;

        }



    }
}