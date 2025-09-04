
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
    public class AMSLineOfBusinessHandler
    {
        private readonly LoggingHandler _logging;
        private readonly DataContextDapper _dapper;
        private readonly DateTime _startTime;
        // private readonly HttpClient _filterDate;
        private readonly Helper _helper = new Helper();
        private readonly RootReqHandler _rootReqHandler;
        private readonly string _dataset = "LineOfBusiness";
		private string _lastResponse = "";

        public AMSLineOfBusinessHandler(LoggingHandler logging, IConfiguration config, RootReqHandler rootReqHandler, DateTime startTime)
        {
            _logging = logging;
            _dapper = new DataContextDapper(config);
            _startTime = startTime;
            _rootReqHandler = rootReqHandler;
        }

        public async Task<string> GetAndInsertLineOfBusiness(string rootRequestUrl, string filterDateString)
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
                    string tableName = "AFW_LineOfBusiness?";
                    bool moreRecordsLeft = true;

                    _dapper.ExecuteSql("EXEC Staging.AMS_LineOfBusiness_Load");
                    _dapper.ExecuteSql("TRUNCATE TABLE RawData.AMS_LineOfBusiness");

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
                        APIResponse<AMSLineOfBusiness> apiResponse = JsonSerializer.Deserialize<APIResponse<AMSLineOfBusiness>>(dataString) ?? new APIResponse<AMSLineOfBusiness>();
                        activeStep = "Loading " + _dataset;
                        // emailBody = _logging.LogServerError(dataString, emailBody, activeStep, errorPage);
                        
                        
                        if (apiResponse.Result.Count() < topValue) { moreRecordsLeft = false; }
                        if (apiResponse.Result.Count() > 0)
                        {
                        CheckAndInsertLineOfBusinesss(apiResponse.Result, activeStep);
                        _logging.WriteLogForModel("Loaded to RawData", _dataset);
                        }
                        

                        nextPage = apiResponse.StartingToken;
                    }
                    _dapper.ExecuteSql("EXEC Staging.AMS_LineOfBusiness_Load");

                    _logging.WriteLogForModel("LineOfBusiness Staging Load Completed", _dataset);
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

        public void CheckAndInsertLineOfBusinesss(IEnumerable<AMSLineOfBusiness> amsLineOfBusinesss, string activeStep)
        {
            if (amsLineOfBusinesss != null && amsLineOfBusinesss.Count() > 0)
            {
                _logging.WriteLogForModel(activeStep, _dataset);
                _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
                InsertLineOfBusinesss(amsLineOfBusinesss);
            }
            else
            {
                _logging.WriteLogForModel("No LineOfBusinesss Found in Date Range", _dataset);
            }
        }

        public void InsertLineOfBusinesss(IEnumerable<AMSLineOfBusiness> amsLineOfBusinesss)
        {

            // string insertRoot = "INSERT INTO RawData.AMS_LineOfBusiness(LineOfBusinessChecksum,Datasource,Deleted,Sequencenumber,AppCreatedDate,ChangedBy,ChangedDate,Description,EffDate,ElfFormVerId,EnteredDate,ExpDate,InsertSeqNo,LineOfBus,LOBChangedDate,LOBId,PlanType,PolId,SortNo,StatePlanType,UICodeLOBS,WritingCoCode,InsertDate) VALUES ";
            string insertRoot = "INSERT INTO RawData.AMS_LineOfBusiness VALUES ";
            string sqlQuery = insertRoot;

            int rowsInserted = 0;
            int countForCurrent = 0;
            DynamicParameters sqlParameters = new DynamicParameters();
            foreach (AMSLineOfBusiness amsLineOfBusiness in amsLineOfBusinesss)
            {
                countForCurrent += 1;
                string sqlToAdd = "(@LineOfBusinessChecksum" + countForCurrent.ToString() +
					",@Datasource" + countForCurrent.ToString() +
					",@Deleted" + countForCurrent.ToString() +
					",@Sequencenumber" + countForCurrent.ToString() +
					",@AppCreatedDate" + countForCurrent.ToString() +
					",@ChangedBy" + countForCurrent.ToString() +
					",@ChangedDate" + countForCurrent.ToString() +
					",@Description" + countForCurrent.ToString() +
					",@EffDate" + countForCurrent.ToString() +
					",@ElfFormVerId" + countForCurrent.ToString() +
					",@EnteredDate" + countForCurrent.ToString() +
					",@ExpDate" + countForCurrent.ToString() +
					",@InsertSeqNo" + countForCurrent.ToString() +
					",@LineOfBus" + countForCurrent.ToString() +
					",@LOBChangedDate" + countForCurrent.ToString() +
					",@LOBId" + countForCurrent.ToString() +
					",@PlanType" + countForCurrent.ToString() +
					",@PolId" + countForCurrent.ToString() +
					",@SortNo" + countForCurrent.ToString() +
					",@StatePlanType" + countForCurrent.ToString() +
					",@UICodeLOBS" + countForCurrent.ToString() +
					",@WritingCoCode" + countForCurrent.ToString() +
					",GETDATE()),";


                if ((sqlQuery + sqlToAdd).Length > 4000)
                {
                    rowsInserted += _dapper.ExecuteSqlWithParameters(sqlQuery.Trim(','), sqlParameters);
                    sqlParameters = new DynamicParameters();
					sqlToAdd = string.Join("0,", sqlToAdd.Split(countForCurrent.ToString() + ","));
                    sqlQuery = insertRoot;
                    countForCurrent = 0;
                }


				sqlParameters.Add("@LineOfBusinessChecksum" + countForCurrent.ToString(), amsLineOfBusiness.LineOfBusinessChecksum, DbType.String);
				sqlParameters.Add("@Datasource" + countForCurrent.ToString(), amsLineOfBusiness.Datasource, DbType.String);
				sqlParameters.Add("@Deleted" + countForCurrent.ToString(), amsLineOfBusiness.Deleted, DbType.Boolean);
				sqlParameters.Add("@Sequencenumber" + countForCurrent.ToString(), amsLineOfBusiness.Sequencenumber, DbType.Decimal);
				sqlParameters.Add("@AppCreatedDate" + countForCurrent.ToString(), _helper.FormatDate(amsLineOfBusiness.AppCreatedDate + ""), DbType.DateTime);
				sqlParameters.Add("@ChangedBy" + countForCurrent.ToString(), amsLineOfBusiness.ChangedBy, DbType.String);
				sqlParameters.Add("@ChangedDate" + countForCurrent.ToString(), _helper.FormatDate(amsLineOfBusiness.ChangedDate + ""), DbType.DateTime);
				sqlParameters.Add("@Description" + countForCurrent.ToString(), amsLineOfBusiness.Description, DbType.String);
				sqlParameters.Add("@EffDate" + countForCurrent.ToString(), _helper.FormatDate(amsLineOfBusiness.EffDate + ""), DbType.DateTime);
				sqlParameters.Add("@ElfFormVerId" + countForCurrent.ToString(), amsLineOfBusiness.ElfFormVerId, DbType.String);
				sqlParameters.Add("@EnteredDate" + countForCurrent.ToString(), _helper.FormatDate(amsLineOfBusiness.EnteredDate + ""), DbType.DateTime);
				sqlParameters.Add("@ExpDate" + countForCurrent.ToString(), _helper.FormatDate(amsLineOfBusiness.ExpDate + ""), DbType.DateTime);
				sqlParameters.Add("@InsertSeqNo" + countForCurrent.ToString(), amsLineOfBusiness.InsertSeqNo, DbType.Int32);
				sqlParameters.Add("@LineOfBus" + countForCurrent.ToString(), amsLineOfBusiness.LineOfBus, DbType.String);
				sqlParameters.Add("@LOBChangedDate" + countForCurrent.ToString(), _helper.FormatDate(amsLineOfBusiness.LOBChangedDate + ""), DbType.DateTime);
				sqlParameters.Add("@LOBId" + countForCurrent.ToString(), amsLineOfBusiness.LOBId, DbType.String);
				sqlParameters.Add("@PlanType" + countForCurrent.ToString(), amsLineOfBusiness.PlanType, DbType.String);
				sqlParameters.Add("@PolId" + countForCurrent.ToString(), amsLineOfBusiness.PolId, DbType.String);
				sqlParameters.Add("@SortNo" + countForCurrent.ToString(), amsLineOfBusiness.SortNo, DbType.Int16);
				sqlParameters.Add("@StatePlanType" + countForCurrent.ToString(), amsLineOfBusiness.StatePlanType, DbType.String);
				sqlParameters.Add("@UICodeLOBS" + countForCurrent.ToString(), amsLineOfBusiness.UICodeLOBS, DbType.String);
				sqlParameters.Add("@WritingCoCode" + countForCurrent.ToString(), amsLineOfBusiness.WritingCoCode, DbType.String);

                sqlQuery += sqlToAdd;
            }
            int lastInsertCount = _dapper.ExecuteSqlWithParameters(sqlQuery.Trim(','), sqlParameters);
            if (lastInsertCount > 0)
            {
                rowsInserted += lastInsertCount;
                _logging.WriteLogForModel("LineOfBusinesss Load Completed: " + rowsInserted.ToString() + " Rows Inserted", _dataset);
                _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
            }
        }
    }
}