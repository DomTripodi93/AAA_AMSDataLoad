
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
    public class AMSCustXRefHandler
    {
        private readonly LoggingHandler _logging;
        private readonly DataContextDapper _dapper;
        private readonly DateTime _startTime;
        // private readonly HttpClient _filterDate;
        private readonly Helper _helper = new Helper();
        private readonly RootReqHandler _rootReqHandler;
        private readonly string _dataset = "CustXRef";
		private string _lastResponse = "";

        public AMSCustXRefHandler(LoggingHandler logging, IConfiguration config, RootReqHandler rootReqHandler, DateTime startTime)
        {
            _logging = logging;
            _dapper = new DataContextDapper(config);
            _startTime = startTime;
            _rootReqHandler = rootReqHandler;
        }

        public async Task<string> GetAndInsertCustXRef(string rootRequestUrl, string filterDateString)
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
                    string tableName = "AFW_CustXRef?";
                    bool moreRecordsLeft = true;

                    _dapper.ExecuteSql("EXEC Staging.AMS_CustXRef_Load");
                    _dapper.ExecuteSql("TRUNCATE TABLE RawData.AMS_CustXRef");

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
                        APIResponse<AMSCustXRef> apiResponse = JsonSerializer.Deserialize<APIResponse<AMSCustXRef>>(dataString) ?? new APIResponse<AMSCustXRef>();
                        activeStep = "Loading " + _dataset;
                        // emailBody = _logging.LogServerError(dataString, emailBody, activeStep, errorPage);
                        
                        
                        if (apiResponse.Result.Count() < topValue) { moreRecordsLeft = false; }
                        if (apiResponse.Result.Count() > 0)
                        {
                        CheckAndInsertCustXRefs(apiResponse.Result, activeStep);
                        _logging.WriteLogForModel("Loaded to RawData", _dataset);
                        }
                        

                        nextPage = apiResponse.StartingToken;
                    }
                    _dapper.ExecuteSql("EXEC Staging.AMS_CustXRef_Load");

                    _logging.WriteLogForModel("CustXRef Staging Load Completed", _dataset);
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

        public void CheckAndInsertCustXRefs(IEnumerable<AMSCustXRef> amsCustXRefs, string activeStep)
        {
            if (amsCustXRefs != null && amsCustXRefs.Count() > 0)
            {
                _logging.WriteLogForModel(activeStep, _dataset);
                _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
                InsertCustXRefs(amsCustXRefs);
            }
            else
            {
                _logging.WriteLogForModel("No CustXRefs Found in Date Range", _dataset);
            }
        }

        public void InsertCustXRefs(IEnumerable<AMSCustXRef> amsCustXRefs)
        {

            // string insertRoot = "INSERT INTO RawData.AMS_CustXRef(CustXRefChecksum,Datasource,Deleted,Sequencenumber,AXRefId,ChangedBy,ChangedDate,CustId,CXRefId,EnteredDate,XReference,InsertDate) VALUES ";
            string insertRoot = "INSERT INTO RawData.AMS_CustXRef VALUES ";
            string sqlQuery = insertRoot;

            int rowsInserted = 0;
            int countForCurrent = 0;
            DynamicParameters sqlParameters = new DynamicParameters();
            foreach (AMSCustXRef amsCustXRef in amsCustXRefs)
            {
                countForCurrent += 1;
                string sqlToAdd = "(@CustXRefChecksum" + countForCurrent.ToString() +
					",@Datasource" + countForCurrent.ToString() +
					",@Deleted" + countForCurrent.ToString() +
					",@Sequencenumber" + countForCurrent.ToString() +
					",@AXRefId" + countForCurrent.ToString() +
					",@ChangedBy" + countForCurrent.ToString() +
					",@ChangedDate" + countForCurrent.ToString() +
					",@CustId" + countForCurrent.ToString() +
					",@CXRefId" + countForCurrent.ToString() +
					",@EnteredDate" + countForCurrent.ToString() +
					",@XReference" + countForCurrent.ToString() +
					",GETDATE()),";


                if ((sqlQuery + sqlToAdd).Length > 4000)
                {
                    rowsInserted += _dapper.ExecuteSqlWithParameters(sqlQuery.Trim(','), sqlParameters);
                    sqlParameters = new DynamicParameters();
					sqlToAdd = string.Join("0,", sqlToAdd.Split(countForCurrent.ToString() + ","));
                    sqlQuery = insertRoot;
                    countForCurrent = 0;
                }


				sqlParameters.Add("@CustXRefChecksum" + countForCurrent.ToString(), amsCustXRef.CustXRefChecksum, DbType.String);
				sqlParameters.Add("@Datasource" + countForCurrent.ToString(), amsCustXRef.Datasource, DbType.String);
				sqlParameters.Add("@Deleted" + countForCurrent.ToString(), amsCustXRef.Deleted, DbType.Boolean);
				sqlParameters.Add("@Sequencenumber" + countForCurrent.ToString(), amsCustXRef.Sequencenumber, DbType.Decimal);
				sqlParameters.Add("@AXRefId" + countForCurrent.ToString(), amsCustXRef.AXRefId, DbType.String);
				sqlParameters.Add("@ChangedBy" + countForCurrent.ToString(), amsCustXRef.ChangedBy, DbType.String);
				sqlParameters.Add("@ChangedDate" + countForCurrent.ToString(), _helper.FormatDate(amsCustXRef.ChangedDate + ""), DbType.DateTime);
				sqlParameters.Add("@CustId" + countForCurrent.ToString(), amsCustXRef.CustId, DbType.String);
				sqlParameters.Add("@CXRefId" + countForCurrent.ToString(), amsCustXRef.CXRefId, DbType.String);
				sqlParameters.Add("@EnteredDate" + countForCurrent.ToString(), _helper.FormatDate(amsCustXRef.EnteredDate + ""), DbType.DateTime);
				sqlParameters.Add("@XReference" + countForCurrent.ToString(), amsCustXRef.XReference, DbType.String);

                sqlQuery += sqlToAdd;
            }
            int lastInsertCount = _dapper.ExecuteSqlWithParameters(sqlQuery.Trim(','), sqlParameters);
            if (lastInsertCount > 0)
            {
                rowsInserted += lastInsertCount;
                _logging.WriteLogForModel("CustXRefs Load Completed: " + rowsInserted.ToString() + " Rows Inserted", _dataset);
                _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
            }
        }
    }
}