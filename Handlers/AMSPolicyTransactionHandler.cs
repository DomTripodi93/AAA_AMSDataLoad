
using System.Data;
using Microsoft.Extensions.Configuration;
using AMSDataLoad.Models;
using AMSDataLoad.Handlers;
using System.Net;
using System.Security;
using Dapper;
using System.Text;
using AMSDataLoad.Data;
using System.Text.Json;
using AMSDataLoad.Helpers;

namespace AMSDataLoad.Handlers
{
    public class AMSPolicyTransactionHandler
    {
        private readonly LoggingHandler _logging;
        private readonly DataContextDapper _dapper;
        private readonly DateTime _startTime;
        // private readonly HttpClient _filterDate;
        private readonly Helper _helper = new Helper();
        private readonly RootReqHandler _rootReqHandler;
        private readonly string _dataset = "PolicyTransaction";
		private string _lastResponse = "";

        public AMSPolicyTransactionHandler(LoggingHandler logging, IConfiguration config, RootReqHandler rootReqHandler, DateTime startTime)
        {
            _logging = logging;
            _dapper = new DataContextDapper(config);
            _startTime = startTime;
            _rootReqHandler = rootReqHandler;
        }

        public async Task<string> GetAndInsertPolicyTransaction(string rootRequestUrl, string filterDateString)
        {
            string emailBody = "";
            await Task.Run(() =>
            {
                string errorPage = "";
                string activeStep = "";
                try
                {
                    string nextPage = "";
                    int topValue = 1000;
                    string tableName = "AFW_PolicyTransaction?";
                    bool moreRecordsLeft = true;

                    _dapper.ExecuteSql("EXEC Staging.AMS_PolicyTransaction_Load");
                    _dapper.ExecuteSql("TRUNCATE TABLE RawData.AMS_PolicyTransaction");

                    while (moreRecordsLeft)
                    {
                        string dataUrl = rootRequestUrl + tableName
                        + "limit=" + topValue
                        + "&schema=public&select=*"
                        + (nextPage != "" ? "&starting_token=" + nextPage : "");
                        // + "&"
                        // + "searchStartDate=" + filterDateString" + topValue 
                        // + "&"
                        // + "$skip=" + skipValue
                        // + "&"
                        // + "$count=true";

                        errorPage = nextPage;

                        activeStep = "Getting " + _dataset;
                        _logging.WriteLogForModel(activeStep, _dataset);
                        _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
                        _logging.WriteLogForModel(dataUrl, _dataset);
                        string dataString = _rootReqHandler.GetStringResult(dataUrl);
						_lastResponse = dataString;


                        Console.WriteLine(dataString.Substring(0, 200 < dataString.Length ? 200 : dataString.Length));
                        APIResponse<AMSPolicyTransaction> apiResponse = JsonSerializer.Deserialize<APIResponse<AMSPolicyTransaction>>(dataString) ?? new APIResponse<AMSPolicyTransaction>();
                        activeStep = "Loading " + _dataset;
                        // emailBody = _logging.LogServerError(dataString, emailBody, activeStep, errorPage);
                        
                        
                        if (apiResponse.Result.Count() < topValue) { moreRecordsLeft = false; }
                        if (apiResponse.Result.Count() > 0)
                        {
                        CheckAndInsertPolicyTransactions(apiResponse.Result, activeStep);
                        _logging.WriteLogForModel("Loaded to RawData", _dataset);
                        }
                        

                        nextPage = apiResponse.StartingToken;
                    }
                    _dapper.ExecuteSql("EXEC Staging.AMS_PolicyTransaction_Load");

                    _logging.WriteLogForModel("PolicyTransaction Staging Load Completed", _dataset);
                    _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
                }
                catch (Exception exception)
                {
                    _logging.WriteLogForModel(exception.Message, _dataset);
                    _logging.WriteLogForModel(exception.StackTrace != null ? exception.StackTrace : "", _dataset);
                    if (_lastResponse != "")
                    {
                        _logging.WriteLogForModel("-----Begin Previous Response:-----", _dataset);
                        _logging.WriteLogForModel(_lastResponse, _dataset);
                        _logging.WriteLogForModel("-----End Previous Response-----", _dataset);
                    }
                    _logging.WriteLogForModel("On Step " + activeStep + ": \n" + exception.Message, _dataset, true);
                    emailBody += "On Step " + activeStep + ", On Skip Value " + errorPage + ": \n" + exception.Message + "\n";
                }
            });
            return emailBody;
        }

        public void CheckAndInsertPolicyTransactions(IEnumerable<AMSPolicyTransaction> amsPolicyTransactions, string activeStep)
        {
            if (amsPolicyTransactions != null && amsPolicyTransactions.Count() > 0)
            {
                _logging.WriteLogForModel(activeStep, _dataset);
                _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
                InsertPolicyTransactions(amsPolicyTransactions);
            }
            else
            {
                _logging.WriteLogForModel("No PolicyTransactions Found in Date Range", _dataset);
            }
        }

        public void InsertPolicyTransactions(IEnumerable<AMSPolicyTransaction> amsPolicyTransactions)
        {

            // string insertRoot = "INSERT INTO RawData.AMS_PolicyTransaction(PolicyTransactionChecksum,Datasource,Deleted,Sequencenumber,BilledNonPrem,BillMethodPolT,BinderReplacePolTEffDate,ChangedBy,ChangedDate,Description,EffDate,EnteredDate,EstRevenuePercent,InstDayPolT,IsPosted,IsUploaded,PayPId,PolId,PremOnEffDate,ReasonPolT,ReplaceDatePolT,Source,TranType,Annualizedestrevenue,Annualizedpremium,InsertDate) VALUES ";
            string insertRoot = "INSERT INTO RawData.AMS_PolicyTransaction VALUES ";
            string sqlQuery = insertRoot;

            int rowsInserted = 0;
            int countForCurrent = 0;
            DynamicParameters sqlParameters = new DynamicParameters();
            foreach (AMSPolicyTransaction amsPolicyTransaction in amsPolicyTransactions)
            {
                countForCurrent += 1;
                string sqlToAdd = "(@PolicyTransactionChecksum" + countForCurrent.ToString() +
					",@Datasource" + countForCurrent.ToString() +
					",@Deleted" + countForCurrent.ToString() +
					",@Sequencenumber" + countForCurrent.ToString() +
					",@BilledNonPrem" + countForCurrent.ToString() +
					",@BillMethodPolT" + countForCurrent.ToString() +
					",@BinderReplacePolTEffDate" + countForCurrent.ToString() +
					",@ChangedBy" + countForCurrent.ToString() +
					",@ChangedDate" + countForCurrent.ToString() +
					",@Description" + countForCurrent.ToString() +
					",@EffDate" + countForCurrent.ToString() +
					",@EnteredDate" + countForCurrent.ToString() +
					",@EstRevenuePercent" + countForCurrent.ToString() +
					",@InstDayPolT" + countForCurrent.ToString() +
					",@IsPosted" + countForCurrent.ToString() +
					",@IsUploaded" + countForCurrent.ToString() +
					",@PayPId" + countForCurrent.ToString() +
					",@PolId" + countForCurrent.ToString() +
					",@PremOnEffDate" + countForCurrent.ToString() +
					",@ReasonPolT" + countForCurrent.ToString() +
					",@ReplaceDatePolT" + countForCurrent.ToString() +
					",@Source" + countForCurrent.ToString() +
					",@TranType" + countForCurrent.ToString() +
					",@Annualizedestrevenue" + countForCurrent.ToString() +
					",@Annualizedpremium" + countForCurrent.ToString() +
					",GETDATE()),";


                if ((sqlQuery + sqlToAdd).Length > 4000)
                {
                    rowsInserted += _dapper.ExecuteSqlWithParameters(sqlQuery.Trim(','), sqlParameters);
                    sqlParameters = new DynamicParameters();
					sqlToAdd = string.Join("0,", sqlToAdd.Split(countForCurrent.ToString() + ","));
                    sqlQuery = insertRoot;
                    countForCurrent = 0;
                }


				sqlParameters.Add("@PolicyTransactionChecksum" + countForCurrent.ToString(), amsPolicyTransaction.PolicyTransactionChecksum, DbType.String);
				sqlParameters.Add("@Datasource" + countForCurrent.ToString(), amsPolicyTransaction.Datasource, DbType.String);
				sqlParameters.Add("@Deleted" + countForCurrent.ToString(), amsPolicyTransaction.Deleted, DbType.Boolean);
				sqlParameters.Add("@Sequencenumber" + countForCurrent.ToString(), amsPolicyTransaction.Sequencenumber, DbType.Decimal);
				sqlParameters.Add("@BilledNonPrem" + countForCurrent.ToString(), amsPolicyTransaction.BilledNonPrem, DbType.Decimal);
				sqlParameters.Add("@BillMethodPolT" + countForCurrent.ToString(), amsPolicyTransaction.BillMethodPolT, DbType.String);
				sqlParameters.Add("@BinderReplacePolTEffDate" + countForCurrent.ToString(), _helper.FormatDate(amsPolicyTransaction.BinderReplacePolTEffDate + ""), DbType.DateTime);
				sqlParameters.Add("@ChangedBy" + countForCurrent.ToString(), amsPolicyTransaction.ChangedBy, DbType.String);
				sqlParameters.Add("@ChangedDate" + countForCurrent.ToString(), _helper.FormatDate(amsPolicyTransaction.ChangedDate + ""), DbType.DateTime);
				sqlParameters.Add("@Description" + countForCurrent.ToString(), amsPolicyTransaction.Description, DbType.String);
				sqlParameters.Add("@EffDate" + countForCurrent.ToString(), _helper.FormatDate(amsPolicyTransaction.EffDate + ""), DbType.DateTime);
				sqlParameters.Add("@EnteredDate" + countForCurrent.ToString(), _helper.FormatDate(amsPolicyTransaction.EnteredDate + ""), DbType.DateTime);
				sqlParameters.Add("@EstRevenuePercent" + countForCurrent.ToString(), amsPolicyTransaction.EstRevenuePercent, DbType.Decimal);
				sqlParameters.Add("@InstDayPolT" + countForCurrent.ToString(), amsPolicyTransaction.InstDayPolT, DbType.Int16);
				sqlParameters.Add("@IsPosted" + countForCurrent.ToString(), amsPolicyTransaction.IsPosted, DbType.String);
				sqlParameters.Add("@IsUploaded" + countForCurrent.ToString(), amsPolicyTransaction.IsUploaded, DbType.String);
				sqlParameters.Add("@PayPId" + countForCurrent.ToString(), amsPolicyTransaction.PayPId, DbType.String);
				sqlParameters.Add("@PolId" + countForCurrent.ToString(), amsPolicyTransaction.PolId, DbType.String);
				sqlParameters.Add("@PremOnEffDate" + countForCurrent.ToString(), amsPolicyTransaction.PremOnEffDate, DbType.Decimal);
				sqlParameters.Add("@ReasonPolT" + countForCurrent.ToString(), amsPolicyTransaction.ReasonPolT, DbType.String);
				sqlParameters.Add("@ReplaceDatePolT" + countForCurrent.ToString(), _helper.FormatDate(amsPolicyTransaction.ReplaceDatePolT + ""), DbType.DateTime);
				sqlParameters.Add("@Source" + countForCurrent.ToString(), amsPolicyTransaction.Source, DbType.String);
				sqlParameters.Add("@TranType" + countForCurrent.ToString(), amsPolicyTransaction.TranType, DbType.String);
				sqlParameters.Add("@Annualizedestrevenue" + countForCurrent.ToString(), amsPolicyTransaction.Annualizedestrevenue, DbType.String);
				sqlParameters.Add("@Annualizedpremium" + countForCurrent.ToString(), amsPolicyTransaction.Annualizedpremium, DbType.String);

                sqlQuery += sqlToAdd;
            }
            int lastInsertCount = _dapper.ExecuteSqlWithParameters(sqlQuery.Trim(','), sqlParameters);
            if (lastInsertCount > 0)
            {
                rowsInserted += lastInsertCount;
                _logging.WriteLogForModel("PolicyTransactions Load Completed: " + rowsInserted.ToString() + " Rows Inserted", _dataset);
                _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
            }
        }
    }
}