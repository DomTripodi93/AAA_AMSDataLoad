
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
    public class AMS131SPolicyHandler
    {
        private readonly LoggingHandler _logging;
        private readonly DataContextDapper _dapper;
        private readonly DateTime _startTime;
        // private readonly HttpClient _filterDate;
        private readonly Helper _helper = new Helper();
        private readonly RootReqHandler _rootReqHandler;
        private readonly string _dataset = "131SPolicy";
		private string _lastResponse = "";

        public AMS131SPolicyHandler(LoggingHandler logging, IConfiguration config, RootReqHandler rootReqHandler, DateTime startTime)
        {
            _logging = logging;
            _dapper = new DataContextDapper(config);
            _startTime = startTime;
            _rootReqHandler = rootReqHandler;
        }

        public async Task<string> GetAndInsert131SPolicy(string rootRequestUrl, string filterDateString)
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
                    string tableName = "AFW_131SPolicy?";
                    bool moreRecordsLeft = true;

                    _dapper.ExecuteSql("EXEC Staging.AMS_131SPolicy_Load");
                    _dapper.ExecuteSql("TRUNCATE TABLE RawData.AMS_131SPolicy");

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
                        APIResponse<AMS131SPolicy> apiResponse = JsonSerializer.Deserialize<APIResponse<AMS131SPolicy>>(dataString) ?? new APIResponse<AMS131SPolicy>();
                        activeStep = "Loading " + _dataset;
                        // emailBody = _logging.LogServerError(dataString, emailBody, activeStep, errorPage);
                        
                        
                        if (apiResponse.Result.Count() < topValue) { moreRecordsLeft = false; }
                        if (apiResponse.Result.Count() > 0)
                        {
                        CheckAndInsert131SPolicys(apiResponse.Result, activeStep);
                        _logging.WriteLogForModel("Loaded to RawData", _dataset);
                        }
                        

                        nextPage = apiResponse.StartingToken;
                    }
                    _dapper.ExecuteSql("EXEC Staging.AMS_131SPolicy_Load");

                    _logging.WriteLogForModel("131SPolicy Staging Load Completed", _dataset);
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

        public void CheckAndInsert131SPolicys(IEnumerable<AMS131SPolicy> ams131SPolicys, string activeStep)
        {
            if (ams131SPolicys != null && ams131SPolicys.Count() > 0)
            {
                _logging.WriteLogForModel(activeStep, _dataset);
                _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
                Insert131SPolicys(ams131SPolicys);
            }
            else
            {
                _logging.WriteLogForModel("No 131SPolicys Found in Date Range", _dataset);
            }
        }

        public void Insert131SPolicys(IEnumerable<AMS131SPolicy> ams131SPolicys)
        {

            // string insertRoot = "INSERT INTO RawData.AMS_131SPolicy(SPolicyChecksum,Datasource,Deleted,Sequencenumber,ChangedBy,ChangedDate,CurRetroDate,EffDate,EnteredDate,ExpirPolNo,IsDollarYes,IsUmbrella,LOBId,PolId,PropRetroDate,SPolicyStatus,UPolId,InsertDate) VALUES ";
            string insertRoot = "INSERT INTO RawData.AMS_131SPolicy VALUES ";
            string sqlQuery = insertRoot;

            int rowsInserted = 0;
            int countForCurrent = 0;
            DynamicParameters sqlParameters = new DynamicParameters();
            foreach (AMS131SPolicy ams131SPolicy in ams131SPolicys)
            {
                countForCurrent += 1;
                string sqlToAdd = "(@SPolicyChecksum" + countForCurrent.ToString() +
					",@Datasource" + countForCurrent.ToString() +
					",@Deleted" + countForCurrent.ToString() +
					",@Sequencenumber" + countForCurrent.ToString() +
					",@ChangedBy" + countForCurrent.ToString() +
					",@ChangedDate" + countForCurrent.ToString() +
					",@CurRetroDate" + countForCurrent.ToString() +
					",@EffDate" + countForCurrent.ToString() +
					",@EnteredDate" + countForCurrent.ToString() +
					",@ExpirPolNo" + countForCurrent.ToString() +
					",@IsDollarYes" + countForCurrent.ToString() +
					",@IsUmbrella" + countForCurrent.ToString() +
					",@LOBId" + countForCurrent.ToString() +
					",@PolId" + countForCurrent.ToString() +
					",@PropRetroDate" + countForCurrent.ToString() +
					",@SPolicyStatus" + countForCurrent.ToString() +
					",@UPolId" + countForCurrent.ToString() +
					",GETDATE()),";


                if ((sqlQuery + sqlToAdd).Length > 4000)
                {
                    rowsInserted += _dapper.ExecuteSqlWithParameters(sqlQuery.Trim(','), sqlParameters);
                    sqlParameters = new DynamicParameters();
					sqlToAdd = string.Join("0,", sqlToAdd.Split(countForCurrent.ToString() + ","));
                    sqlQuery = insertRoot;
                    countForCurrent = 0;
                }


				sqlParameters.Add("@SPolicyChecksum" + countForCurrent.ToString(), ams131SPolicy.SPolicyChecksum, DbType.String);
				sqlParameters.Add("@Datasource" + countForCurrent.ToString(), ams131SPolicy.Datasource, DbType.String);
				sqlParameters.Add("@Deleted" + countForCurrent.ToString(), ams131SPolicy.Deleted, DbType.Boolean);
				sqlParameters.Add("@Sequencenumber" + countForCurrent.ToString(), ams131SPolicy.Sequencenumber, DbType.Decimal);
				sqlParameters.Add("@ChangedBy" + countForCurrent.ToString(), ams131SPolicy.ChangedBy, DbType.String);
				sqlParameters.Add("@ChangedDate" + countForCurrent.ToString(), _helper.FormatDate(ams131SPolicy.ChangedDate + ""), DbType.DateTime);
				sqlParameters.Add("@CurRetroDate" + countForCurrent.ToString(), ams131SPolicy.CurRetroDate, DbType.String);
				sqlParameters.Add("@EffDate" + countForCurrent.ToString(), _helper.FormatDate(ams131SPolicy.EffDate + ""), DbType.DateTime);
				sqlParameters.Add("@EnteredDate" + countForCurrent.ToString(), _helper.FormatDate(ams131SPolicy.EnteredDate + ""), DbType.DateTime);
				sqlParameters.Add("@ExpirPolNo" + countForCurrent.ToString(), ams131SPolicy.ExpirPolNo, DbType.String);
				sqlParameters.Add("@IsDollarYes" + countForCurrent.ToString(), ams131SPolicy.IsDollarYes, DbType.String);
				sqlParameters.Add("@IsUmbrella" + countForCurrent.ToString(), ams131SPolicy.IsUmbrella, DbType.String);
				sqlParameters.Add("@LOBId" + countForCurrent.ToString(), ams131SPolicy.LOBId, DbType.String);
				sqlParameters.Add("@PolId" + countForCurrent.ToString(), ams131SPolicy.PolId, DbType.String);
				sqlParameters.Add("@PropRetroDate" + countForCurrent.ToString(), ams131SPolicy.PropRetroDate, DbType.String);
				sqlParameters.Add("@SPolicyStatus" + countForCurrent.ToString(), ams131SPolicy.SPolicyStatus, DbType.String);
				sqlParameters.Add("@UPolId" + countForCurrent.ToString(), ams131SPolicy.UPolId, DbType.String);

                sqlQuery += sqlToAdd;
            }
            int lastInsertCount = _dapper.ExecuteSqlWithParameters(sqlQuery.Trim(','), sqlParameters);
            if (lastInsertCount > 0)
            {
                rowsInserted += lastInsertCount;
                _logging.WriteLogForModel("131SPolicys Load Completed: " + rowsInserted.ToString() + " Rows Inserted", _dataset);
                _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
            }
        }
    }
}