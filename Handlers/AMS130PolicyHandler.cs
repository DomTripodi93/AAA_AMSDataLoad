
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
    public class AMS130PolicyHandler
    {
        private readonly LoggingHandler _logging;
        private readonly DataContextDapper _dapper;
        private readonly DateTime _startTime;
        // private readonly HttpClient _filterDate;
        private readonly Helper _helper = new Helper();
        private readonly RootReqHandler _rootReqHandler;
        private readonly string _dataset = "130Policy";
		private string _lastResponse = "";

        public AMS130PolicyHandler(LoggingHandler logging, IConfiguration config, RootReqHandler rootReqHandler, DateTime startTime)
        {
            _logging = logging;
            _dapper = new DataContextDapper(config);
            _startTime = startTime;
            _rootReqHandler = rootReqHandler;
        }

        public async Task<string> GetAndInsert130Policy(string rootRequestUrl, string filterDateString)
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
                    string tableName = "AFW_130Policy?";
                    bool moreRecordsLeft = true;

                    _dapper.ExecuteSql("EXEC Staging.AMS_130Policy_Load");
                    _dapper.ExecuteSql("TRUNCATE TABLE RawData.AMS_130Policy");

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
                        APIResponse<AMS130Policy> apiResponse = JsonSerializer.Deserialize<APIResponse<AMS130Policy>>(dataString) ?? new APIResponse<AMS130Policy>();
                        activeStep = "Loading " + _dataset;
                        // emailBody = _logging.LogServerError(dataString, emailBody, activeStep, errorPage);
                        
                        
                        if (apiResponse.Result.Count() < topValue) { moreRecordsLeft = false; }
                        if (apiResponse.Result.Count() > 0)
                        {
                        CheckAndInsert130Policys(apiResponse.Result, activeStep);
                        _logging.WriteLogForModel("Loaded to RawData", _dataset);
                        }
                        

                        nextPage = apiResponse.StartingToken;
                    }
                    _dapper.ExecuteSql("EXEC Staging.AMS_130Policy_Load");

                    _logging.WriteLogForModel("130Policy Staging Load Completed", _dataset);
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

        public void CheckAndInsert130Policys(IEnumerable<AMS130Policy> ams130Policys, string activeStep)
        {
            if (ams130Policys != null && ams130Policys.Count() > 0)
            {
                _logging.WriteLogForModel(activeStep, _dataset);
                _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
                Insert130Policys(ams130Policys);
            }
            else
            {
                _logging.WriteLogForModel("No 130Policys Found in Date Range", _dataset);
            }
        }

        public void Insert130Policys(IEnumerable<AMS130Policy> ams130Policys)
        {

            // string insertRoot = "INSERT INTO RawData.AMS_130Policy(PolicyChecksum,Datasource,Deleted,Sequencenumber,AddInfo,ChangedBy,ChangedDate,DividendPlan,EffDate,EmployerNo,Entereddate,IncludeExclude,IsPart1,IsParticipating,IsSafetyGroup,LOBId,NCCINo,OtherNo,PolId,RatingDate,RetroPlan,RetroYrs,PolicyState,PolicyStatus,WPolId,InsertDate) VALUES ";
            string insertRoot = "INSERT INTO RawData.AMS_130Policy VALUES ";
            string sqlQuery = insertRoot;

            int rowsInserted = 0;
            int countForCurrent = 0;
            DynamicParameters sqlParameters = new DynamicParameters();
            foreach (AMS130Policy ams130Policy in ams130Policys)
            {
                countForCurrent += 1;
                string sqlToAdd = "(@PolicyChecksum" + countForCurrent.ToString() +
					",@Datasource" + countForCurrent.ToString() +
					",@Deleted" + countForCurrent.ToString() +
					",@Sequencenumber" + countForCurrent.ToString() +
					",@AddInfo" + countForCurrent.ToString() +
					",@ChangedBy" + countForCurrent.ToString() +
					",@ChangedDate" + countForCurrent.ToString() +
					",@DividendPlan" + countForCurrent.ToString() +
					",@EffDate" + countForCurrent.ToString() +
					",@EmployerNo" + countForCurrent.ToString() +
					",@Entereddate" + countForCurrent.ToString() +
					",@IncludeExclude" + countForCurrent.ToString() +
					",@IsPart1" + countForCurrent.ToString() +
					",@IsParticipating" + countForCurrent.ToString() +
					",@IsSafetyGroup" + countForCurrent.ToString() +
					",@LOBId" + countForCurrent.ToString() +
					",@NCCINo" + countForCurrent.ToString() +
					",@OtherNo" + countForCurrent.ToString() +
					",@PolId" + countForCurrent.ToString() +
					",@RatingDate" + countForCurrent.ToString() +
					",@RetroPlan" + countForCurrent.ToString() +
					",@RetroYrs" + countForCurrent.ToString() +
					",@PolicyState" + countForCurrent.ToString() +
					",@PolicyStatus" + countForCurrent.ToString() +
					",@WPolId" + countForCurrent.ToString() +
					",GETDATE()),";


                if ((sqlQuery + sqlToAdd).Length > 4000)
                {
                    rowsInserted += _dapper.ExecuteSqlWithParameters(sqlQuery.Trim(','), sqlParameters);
                    sqlParameters = new DynamicParameters();
					sqlToAdd = string.Join("0,", sqlToAdd.Split(countForCurrent.ToString() + ","));
                    sqlQuery = insertRoot;
                    countForCurrent = 0;
                }


				sqlParameters.Add("@PolicyChecksum" + countForCurrent.ToString(), ams130Policy.PolicyChecksum, DbType.String);
				sqlParameters.Add("@Datasource" + countForCurrent.ToString(), ams130Policy.Datasource, DbType.String);
				sqlParameters.Add("@Deleted" + countForCurrent.ToString(), ams130Policy.Deleted, DbType.Boolean);
				sqlParameters.Add("@Sequencenumber" + countForCurrent.ToString(), ams130Policy.Sequencenumber, DbType.Decimal);
				sqlParameters.Add("@AddInfo" + countForCurrent.ToString(), ams130Policy.AddInfo, DbType.String);
				sqlParameters.Add("@ChangedBy" + countForCurrent.ToString(), ams130Policy.ChangedBy, DbType.String);
				sqlParameters.Add("@ChangedDate" + countForCurrent.ToString(), _helper.FormatDate(ams130Policy.ChangedDate + ""), DbType.DateTime);
				sqlParameters.Add("@DividendPlan" + countForCurrent.ToString(), ams130Policy.DividendPlan, DbType.String);
				sqlParameters.Add("@EffDate" + countForCurrent.ToString(), _helper.FormatDate(ams130Policy.EffDate + ""), DbType.DateTime);
				sqlParameters.Add("@EmployerNo" + countForCurrent.ToString(), ams130Policy.EmployerNo, DbType.String);
				sqlParameters.Add("@Entereddate" + countForCurrent.ToString(), _helper.FormatDate(ams130Policy.Entereddate + ""), DbType.DateTime);
				sqlParameters.Add("@IncludeExclude" + countForCurrent.ToString(), ams130Policy.IncludeExclude, DbType.String);
				sqlParameters.Add("@IsPart1" + countForCurrent.ToString(), ams130Policy.IsPart1, DbType.String);
				sqlParameters.Add("@IsParticipating" + countForCurrent.ToString(), ams130Policy.IsParticipating, DbType.String);
				sqlParameters.Add("@IsSafetyGroup" + countForCurrent.ToString(), ams130Policy.IsSafetyGroup, DbType.String);
				sqlParameters.Add("@LOBId" + countForCurrent.ToString(), ams130Policy.LOBId, DbType.String);
				sqlParameters.Add("@NCCINo" + countForCurrent.ToString(), ams130Policy.NCCINo, DbType.String);
				sqlParameters.Add("@OtherNo" + countForCurrent.ToString(), ams130Policy.OtherNo, DbType.String);
				sqlParameters.Add("@PolId" + countForCurrent.ToString(), ams130Policy.PolId, DbType.String);
				sqlParameters.Add("@RatingDate" + countForCurrent.ToString(), _helper.FormatDate(ams130Policy.RatingDate + ""), DbType.DateTime);
				sqlParameters.Add("@RetroPlan" + countForCurrent.ToString(), ams130Policy.RetroPlan, DbType.String);
				sqlParameters.Add("@RetroYrs" + countForCurrent.ToString(), ams130Policy.RetroYrs, DbType.String);
				sqlParameters.Add("@PolicyState" + countForCurrent.ToString(), ams130Policy.PolicyState, DbType.String);
				sqlParameters.Add("@PolicyStatus" + countForCurrent.ToString(), ams130Policy.PolicyStatus, DbType.String);
				sqlParameters.Add("@WPolId" + countForCurrent.ToString(), ams130Policy.WPolId, DbType.String);

                sqlQuery += sqlToAdd;
            }
            int lastInsertCount = _dapper.ExecuteSqlWithParameters(sqlQuery.Trim(','), sqlParameters);
            if (lastInsertCount > 0)
            {
                rowsInserted += lastInsertCount;
                _logging.WriteLogForModel("130Policys Load Completed: " + rowsInserted.ToString() + " Rows Inserted", _dataset);
                _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
            }
        }
    }
}