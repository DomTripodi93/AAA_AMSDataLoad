
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
    public class AMSCustContactHandler
    {
        private readonly LoggingHandler _logging;
        private readonly DataContextDapper _dapper;
        private readonly DateTime _startTime;
        // private readonly HttpClient _filterDate;
        private readonly Helper _helper = new Helper();
        private readonly RootReqHandler _rootReqHandler;
        private readonly string _dataset = "CustContact";
		private string _lastResponse = "";

        public AMSCustContactHandler(LoggingHandler logging, IConfiguration config, RootReqHandler rootReqHandler, DateTime startTime)
        {
            _logging = logging;
            _dapper = new DataContextDapper(config);
            _startTime = startTime;
            _rootReqHandler = rootReqHandler;
        }

        public async Task<string> GetAndInsertCustContact(string rootRequestUrl, string filterDateString)
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
                    string tableName = "AFW_CustContact?";
                    bool moreRecordsLeft = true;

                    _dapper.ExecuteSql("EXEC Staging.AMS_CustContact_Load");
                    _dapper.ExecuteSql("TRUNCATE TABLE RawData.AMS_CustContact");

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
                        APIResponse<AMSCustContact> apiResponse = JsonSerializer.Deserialize<APIResponse<AMSCustContact>>(dataString) ?? new APIResponse<AMSCustContact>();
                        activeStep = "Loading " + _dataset;
                        // emailBody = _logging.LogServerError(dataString, emailBody, activeStep, errorPage);
                        
                        
                        if (apiResponse.Result.Count() < topValue) { moreRecordsLeft = false; }
                        if (apiResponse.Result.Count() > 0)
                        {
                        CheckAndInsertCustContacts(apiResponse.Result, activeStep);
                        _logging.WriteLogForModel("Loaded to RawData", _dataset);
                        }
                        

                        nextPage = apiResponse.StartingToken;
                    }
                    _dapper.ExecuteSql("EXEC Staging.AMS_CustContact_Load");

                    _logging.WriteLogForModel("CustContact Staging Load Completed", _dataset);
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

        public void CheckAndInsertCustContacts(IEnumerable<AMSCustContact> amsCustContacts, string activeStep)
        {
            if (amsCustContacts != null && amsCustContacts.Count() > 0)
            {
                _logging.WriteLogForModel(activeStep, _dataset);
                _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
                InsertCustContacts(amsCustContacts);
            }
            else
            {
                _logging.WriteLogForModel("No CustContacts Found in Date Range", _dataset);
            }
        }

        public void InsertCustContacts(IEnumerable<AMSCustContact> amsCustContacts)
        {

            // string insertRoot = "INSERT INTO RawData.AMS_CustContact(CustContactChecksum,Datasource,Deleted,Sequencenumber,Address1,Address2,BusAreaCode,BusExt,BusPhone,CCntId,ChangedBy,ChangedDate,City,ContactMethod,ContactName,Country,County,CustCenterDisplay,CustId,EMail,EnteredDate,FaxAreaCode,FaxExt,FaxPhone,FormalSalutation,InformalSalutation,Isdirector,IsForeign,IsOfficer,MobileAreaCode,MobileExt,MobilePhone,Notes,PagerAreaCode,PagerExt,PagerPhone,ResAreaCode,ResExt,ResPhone,SortOrder,CustContactState,Title,Zip,InsertDate) VALUES ";
            string insertRoot = "INSERT INTO RawData.AMS_CustContact VALUES ";
            string sqlQuery = insertRoot;

            int rowsInserted = 0;
            int countForCurrent = 0;
            DynamicParameters sqlParameters = new DynamicParameters();
            foreach (AMSCustContact amsCustContact in amsCustContacts)
            {
                countForCurrent += 1;
                string sqlToAdd = "(@CustContactChecksum" + countForCurrent.ToString() +
					",@Datasource" + countForCurrent.ToString() +
					",@Deleted" + countForCurrent.ToString() +
					",@Sequencenumber" + countForCurrent.ToString() +
					",@Address1" + countForCurrent.ToString() +
					",@Address2" + countForCurrent.ToString() +
					",@BusAreaCode" + countForCurrent.ToString() +
					",@BusExt" + countForCurrent.ToString() +
					",@BusPhone" + countForCurrent.ToString() +
					",@CCntId" + countForCurrent.ToString() +
					",@ChangedBy" + countForCurrent.ToString() +
					",@ChangedDate" + countForCurrent.ToString() +
					",@City" + countForCurrent.ToString() +
					",@ContactMethod" + countForCurrent.ToString() +
					",@ContactName" + countForCurrent.ToString() +
					",@Country" + countForCurrent.ToString() +
					",@County" + countForCurrent.ToString() +
					",@CustCenterDisplay" + countForCurrent.ToString() +
					",@CustId" + countForCurrent.ToString() +
					",@EMail" + countForCurrent.ToString() +
					",@EnteredDate" + countForCurrent.ToString() +
					",@FaxAreaCode" + countForCurrent.ToString() +
					",@FaxExt" + countForCurrent.ToString() +
					",@FaxPhone" + countForCurrent.ToString() +
					",@FormalSalutation" + countForCurrent.ToString() +
					",@InformalSalutation" + countForCurrent.ToString() +
					",@Isdirector" + countForCurrent.ToString() +
					",@IsForeign" + countForCurrent.ToString() +
					",@IsOfficer" + countForCurrent.ToString() +
					",@MobileAreaCode" + countForCurrent.ToString() +
					",@MobileExt" + countForCurrent.ToString() +
					",@MobilePhone" + countForCurrent.ToString() +
					",@Notes" + countForCurrent.ToString() +
					",@PagerAreaCode" + countForCurrent.ToString() +
					",@PagerExt" + countForCurrent.ToString() +
					",@PagerPhone" + countForCurrent.ToString() +
					",@ResAreaCode" + countForCurrent.ToString() +
					",@ResExt" + countForCurrent.ToString() +
					",@ResPhone" + countForCurrent.ToString() +
					",@SortOrder" + countForCurrent.ToString() +
					",@CustContactState" + countForCurrent.ToString() +
					",@Title" + countForCurrent.ToString() +
					",@Zip" + countForCurrent.ToString() +
					",GETDATE()),";


                if ((sqlQuery + sqlToAdd).Length > 4000)
                {
                    rowsInserted += _dapper.ExecuteSqlWithParameters(sqlQuery.Trim(','), sqlParameters);
                    sqlParameters = new DynamicParameters();
					sqlToAdd = string.Join("0,", sqlToAdd.Split(countForCurrent.ToString() + ","));
                    sqlQuery = insertRoot;
                    countForCurrent = 0;
                }


				sqlParameters.Add("@CustContactChecksum" + countForCurrent.ToString(), amsCustContact.CustContactChecksum, DbType.String);
				sqlParameters.Add("@Datasource" + countForCurrent.ToString(), amsCustContact.Datasource, DbType.String);
				sqlParameters.Add("@Deleted" + countForCurrent.ToString(), amsCustContact.Deleted, DbType.Boolean);
				sqlParameters.Add("@Sequencenumber" + countForCurrent.ToString(), amsCustContact.Sequencenumber, DbType.Decimal);
				sqlParameters.Add("@Address1" + countForCurrent.ToString(), amsCustContact.Address1, DbType.String);
				sqlParameters.Add("@Address2" + countForCurrent.ToString(), amsCustContact.Address2, DbType.String);
				sqlParameters.Add("@BusAreaCode" + countForCurrent.ToString(), amsCustContact.BusAreaCode, DbType.String);
				sqlParameters.Add("@BusExt" + countForCurrent.ToString(), amsCustContact.BusExt, DbType.String);
				sqlParameters.Add("@BusPhone" + countForCurrent.ToString(), amsCustContact.BusPhone, DbType.String);
				sqlParameters.Add("@CCntId" + countForCurrent.ToString(), amsCustContact.CCntId, DbType.String);
				sqlParameters.Add("@ChangedBy" + countForCurrent.ToString(), amsCustContact.ChangedBy, DbType.String);
				sqlParameters.Add("@ChangedDate" + countForCurrent.ToString(), _helper.FormatDate(amsCustContact.ChangedDate + ""), DbType.DateTime);
				sqlParameters.Add("@City" + countForCurrent.ToString(), amsCustContact.City, DbType.String);
				sqlParameters.Add("@ContactMethod" + countForCurrent.ToString(), amsCustContact.ContactMethod, DbType.String);
				sqlParameters.Add("@ContactName" + countForCurrent.ToString(), amsCustContact.ContactName, DbType.String);
				sqlParameters.Add("@Country" + countForCurrent.ToString(), amsCustContact.Country, DbType.String);
				sqlParameters.Add("@County" + countForCurrent.ToString(), amsCustContact.County, DbType.String);
				sqlParameters.Add("@CustCenterDisplay" + countForCurrent.ToString(), amsCustContact.CustCenterDisplay, DbType.String);
				sqlParameters.Add("@CustId" + countForCurrent.ToString(), amsCustContact.CustId, DbType.String);
				sqlParameters.Add("@EMail" + countForCurrent.ToString(), amsCustContact.EMail, DbType.String);
				sqlParameters.Add("@EnteredDate" + countForCurrent.ToString(), _helper.FormatDate(amsCustContact.EnteredDate + ""), DbType.DateTime);
				sqlParameters.Add("@FaxAreaCode" + countForCurrent.ToString(), amsCustContact.FaxAreaCode, DbType.String);
				sqlParameters.Add("@FaxExt" + countForCurrent.ToString(), amsCustContact.FaxExt, DbType.String);
				sqlParameters.Add("@FaxPhone" + countForCurrent.ToString(), amsCustContact.FaxPhone, DbType.String);
				sqlParameters.Add("@FormalSalutation" + countForCurrent.ToString(), amsCustContact.FormalSalutation, DbType.String);
				sqlParameters.Add("@InformalSalutation" + countForCurrent.ToString(), amsCustContact.InformalSalutation, DbType.String);
				sqlParameters.Add("@Isdirector" + countForCurrent.ToString(), amsCustContact.Isdirector, DbType.String);
				sqlParameters.Add("@IsForeign" + countForCurrent.ToString(), amsCustContact.IsForeign, DbType.Boolean);
				sqlParameters.Add("@IsOfficer" + countForCurrent.ToString(), amsCustContact.IsOfficer, DbType.String);
				sqlParameters.Add("@MobileAreaCode" + countForCurrent.ToString(), amsCustContact.MobileAreaCode, DbType.String);
				sqlParameters.Add("@MobileExt" + countForCurrent.ToString(), amsCustContact.MobileExt, DbType.String);
				sqlParameters.Add("@MobilePhone" + countForCurrent.ToString(), amsCustContact.MobilePhone, DbType.String);
				sqlParameters.Add("@Notes" + countForCurrent.ToString(), amsCustContact.Notes, DbType.String);
				sqlParameters.Add("@PagerAreaCode" + countForCurrent.ToString(), amsCustContact.PagerAreaCode, DbType.String);
				sqlParameters.Add("@PagerExt" + countForCurrent.ToString(), amsCustContact.PagerExt, DbType.String);
				sqlParameters.Add("@PagerPhone" + countForCurrent.ToString(), amsCustContact.PagerPhone, DbType.String);
				sqlParameters.Add("@ResAreaCode" + countForCurrent.ToString(), amsCustContact.ResAreaCode, DbType.String);
				sqlParameters.Add("@ResExt" + countForCurrent.ToString(), amsCustContact.ResExt, DbType.String);
				sqlParameters.Add("@ResPhone" + countForCurrent.ToString(), amsCustContact.ResPhone, DbType.String);
				sqlParameters.Add("@SortOrder" + countForCurrent.ToString(), amsCustContact.SortOrder, DbType.Int16);
				sqlParameters.Add("@CustContactState" + countForCurrent.ToString(), amsCustContact.CustContactState, DbType.String);
				sqlParameters.Add("@Title" + countForCurrent.ToString(), amsCustContact.Title, DbType.String);
				sqlParameters.Add("@Zip" + countForCurrent.ToString(), amsCustContact.Zip, DbType.String);

                sqlQuery += sqlToAdd;
            }
            int lastInsertCount = _dapper.ExecuteSqlWithParameters(sqlQuery.Trim(','), sqlParameters);
            if (lastInsertCount > 0)
            {
                rowsInserted += lastInsertCount;
                _logging.WriteLogForModel("CustContacts Load Completed: " + rowsInserted.ToString() + " Rows Inserted", _dataset);
                _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
            }
        }
    }
}