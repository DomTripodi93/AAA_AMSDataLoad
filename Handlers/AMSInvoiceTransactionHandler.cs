
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
    public class AMSInvoiceTransactionHandler
    {
        private readonly LoggingHandler _logging;
        private readonly DataContextDapper _dapper;
        private readonly DateTime _startTime;
        // private readonly HttpClient _filterDate;
        private readonly Helper _helper = new Helper();
        private readonly RootReqHandler _rootReqHandler;
        private readonly string _dataset = "InvoiceTransaction";
		private string _lastResponse = "";

        public AMSInvoiceTransactionHandler(LoggingHandler logging, IConfiguration config, RootReqHandler rootReqHandler, DateTime startTime)
        {
            _logging = logging;
            _dapper = new DataContextDapper(config);
            _startTime = startTime;
            _rootReqHandler = rootReqHandler;
        }

        public async Task<string> GetAndInsertInvoiceTransaction(string rootRequestUrl, string filterDateString)
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
                    string tableName = "AFW_InvoiceTransaction?";
                    bool moreRecordsLeft = true;

                    _dapper.ExecuteSql("EXEC Staging.AMS_InvoiceTransaction_Load");
                    _dapper.ExecuteSql("TRUNCATE TABLE RawData.AMS_InvoiceTransaction");

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
                        APIResponse<AMSInvoiceTransaction> apiResponse = JsonSerializer.Deserialize<APIResponse<AMSInvoiceTransaction>>(dataString) ?? new APIResponse<AMSInvoiceTransaction>();
                        activeStep = "Loading " + _dataset;
                        // emailBody = _logging.LogServerError(dataString, emailBody, activeStep, errorPage);
                        
                        
                        if (apiResponse.Result.Count() < topValue) { moreRecordsLeft = false; }
                        if (apiResponse.Result.Count() > 0)
                        {
                        CheckAndInsertInvoiceTransactions(apiResponse.Result, activeStep);
                        _logging.WriteLogForModel("Loaded to RawData", _dataset);
                        }
                        

                        nextPage = apiResponse.StartingToken;
                    }
                    _dapper.ExecuteSql("EXEC Staging.AMS_InvoiceTransaction_Load");

                    _logging.WriteLogForModel("InvoiceTransaction Staging Load Completed", _dataset);
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

        public void CheckAndInsertInvoiceTransactions(IEnumerable<AMSInvoiceTransaction> amsInvoiceTransactions, string activeStep)
        {
            if (amsInvoiceTransactions != null && amsInvoiceTransactions.Count() > 0)
            {
                _logging.WriteLogForModel(activeStep, _dataset);
                _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
                InsertInvoiceTransactions(amsInvoiceTransactions);
            }
            else
            {
                _logging.WriteLogForModel("No InvoiceTransactions Found in Date Range", _dataset);
            }
        }

        public void InsertInvoiceTransactions(IEnumerable<AMSInvoiceTransaction> amsInvoiceTransactions)
        {

            // string insertRoot = "INSERT INTO RawData.AMS_InvoiceTransaction(InvoiceTransactionChecksum,Datasource,Deleted,Sequencenumber,BackInvTPId,BHId,BillMethod,BillSeqId,BillTranId,BinderPolTEffDate,BinderPostMethod,BinderStatus,BrokerCode,ChangedBy,ChangedDate,ChargeCat,Chargecode,CoCode,CommPayType,CoType,CSHId,CustId,Description,EnteredDate,FullTermPremAmt,GLDate,GrossAmt,InvEffDate,InvId,InvTPId,IsCancelled,IsInstallment,IsPosted,JournalTranId,LineOfBus,NewInvTPId,NonPrRecipient,OldInvTPId,PlanType,PolTEffDate,Poltpid,RBBkOutId,ReplaceDate,TranType,WritingCoCode,InsertDate) VALUES ";
            string insertRoot = "INSERT INTO RawData.AMS_InvoiceTransaction VALUES ";
            string sqlQuery = insertRoot;

            int rowsInserted = 0;
            int countForCurrent = 0;
            DynamicParameters sqlParameters = new DynamicParameters();
            foreach (AMSInvoiceTransaction amsInvoiceTransaction in amsInvoiceTransactions)
            {
                countForCurrent += 1;
                string sqlToAdd = "(@InvoiceTransactionChecksum" + countForCurrent.ToString() +
					",@Datasource" + countForCurrent.ToString() +
					",@Deleted" + countForCurrent.ToString() +
					",@Sequencenumber" + countForCurrent.ToString() +
					",@BackInvTPId" + countForCurrent.ToString() +
					",@BHId" + countForCurrent.ToString() +
					",@BillMethod" + countForCurrent.ToString() +
					",@BillSeqId" + countForCurrent.ToString() +
					",@BillTranId" + countForCurrent.ToString() +
					",@BinderPolTEffDate" + countForCurrent.ToString() +
					",@BinderPostMethod" + countForCurrent.ToString() +
					",@BinderStatus" + countForCurrent.ToString() +
					",@BrokerCode" + countForCurrent.ToString() +
					",@ChangedBy" + countForCurrent.ToString() +
					",@ChangedDate" + countForCurrent.ToString() +
					",@ChargeCat" + countForCurrent.ToString() +
					",@Chargecode" + countForCurrent.ToString() +
					",@CoCode" + countForCurrent.ToString() +
					",@CommPayType" + countForCurrent.ToString() +
					",@CoType" + countForCurrent.ToString() +
					",@CSHId" + countForCurrent.ToString() +
					",@CustId" + countForCurrent.ToString() +
					",@Description" + countForCurrent.ToString() +
					",@EnteredDate" + countForCurrent.ToString() +
					",@FullTermPremAmt" + countForCurrent.ToString() +
					",@GLDate" + countForCurrent.ToString() +
					",@GrossAmt" + countForCurrent.ToString() +
					",@InvEffDate" + countForCurrent.ToString() +
					",@InvId" + countForCurrent.ToString() +
					",@InvTPId" + countForCurrent.ToString() +
					",@IsCancelled" + countForCurrent.ToString() +
					",@IsInstallment" + countForCurrent.ToString() +
					",@IsPosted" + countForCurrent.ToString() +
					",@JournalTranId" + countForCurrent.ToString() +
					",@LineOfBus" + countForCurrent.ToString() +
					",@NewInvTPId" + countForCurrent.ToString() +
					",@NonPrRecipient" + countForCurrent.ToString() +
					",@OldInvTPId" + countForCurrent.ToString() +
					",@PlanType" + countForCurrent.ToString() +
					",@PolTEffDate" + countForCurrent.ToString() +
					",@Poltpid" + countForCurrent.ToString() +
					",@RBBkOutId" + countForCurrent.ToString() +
					",@ReplaceDate" + countForCurrent.ToString() +
					",@TranType" + countForCurrent.ToString() +
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


				sqlParameters.Add("@InvoiceTransactionChecksum" + countForCurrent.ToString(), amsInvoiceTransaction.InvoiceTransactionChecksum, DbType.String);
				sqlParameters.Add("@Datasource" + countForCurrent.ToString(), amsInvoiceTransaction.Datasource, DbType.String);
				sqlParameters.Add("@Deleted" + countForCurrent.ToString(), amsInvoiceTransaction.Deleted, DbType.Boolean);
				sqlParameters.Add("@Sequencenumber" + countForCurrent.ToString(), amsInvoiceTransaction.Sequencenumber, DbType.Decimal);
				sqlParameters.Add("@BackInvTPId" + countForCurrent.ToString(), amsInvoiceTransaction.BackInvTPId, DbType.String);
				sqlParameters.Add("@BHId" + countForCurrent.ToString(), amsInvoiceTransaction.BHId, DbType.String);
				sqlParameters.Add("@BillMethod" + countForCurrent.ToString(), amsInvoiceTransaction.BillMethod, DbType.String);
				sqlParameters.Add("@BillSeqId" + countForCurrent.ToString(), amsInvoiceTransaction.BillSeqId, DbType.String);
				sqlParameters.Add("@BillTranId" + countForCurrent.ToString(), amsInvoiceTransaction.BillTranId, DbType.String);
				sqlParameters.Add("@BinderPolTEffDate" + countForCurrent.ToString(), _helper.FormatDate(amsInvoiceTransaction.BinderPolTEffDate + ""), DbType.DateTime);
				sqlParameters.Add("@BinderPostMethod" + countForCurrent.ToString(), amsInvoiceTransaction.BinderPostMethod, DbType.String);
				sqlParameters.Add("@BinderStatus" + countForCurrent.ToString(), amsInvoiceTransaction.BinderStatus, DbType.String);
				sqlParameters.Add("@BrokerCode" + countForCurrent.ToString(), amsInvoiceTransaction.BrokerCode, DbType.String);
				sqlParameters.Add("@ChangedBy" + countForCurrent.ToString(), amsInvoiceTransaction.ChangedBy, DbType.String);
				sqlParameters.Add("@ChangedDate" + countForCurrent.ToString(), _helper.FormatDate(amsInvoiceTransaction.ChangedDate + ""), DbType.DateTime);
				sqlParameters.Add("@ChargeCat" + countForCurrent.ToString(), amsInvoiceTransaction.ChargeCat, DbType.String);
				sqlParameters.Add("@Chargecode" + countForCurrent.ToString(), amsInvoiceTransaction.Chargecode, DbType.String);
				sqlParameters.Add("@CoCode" + countForCurrent.ToString(), amsInvoiceTransaction.CoCode, DbType.String);
				sqlParameters.Add("@CommPayType" + countForCurrent.ToString(), amsInvoiceTransaction.CommPayType, DbType.String);
				sqlParameters.Add("@CoType" + countForCurrent.ToString(), amsInvoiceTransaction.CoType, DbType.String);
				sqlParameters.Add("@CSHId" + countForCurrent.ToString(), amsInvoiceTransaction.CSHId, DbType.String);
				sqlParameters.Add("@CustId" + countForCurrent.ToString(), amsInvoiceTransaction.CustId, DbType.String);
				sqlParameters.Add("@Description" + countForCurrent.ToString(), amsInvoiceTransaction.Description, DbType.String);
				sqlParameters.Add("@EnteredDate" + countForCurrent.ToString(), _helper.FormatDate(amsInvoiceTransaction.EnteredDate + ""), DbType.DateTime);
				sqlParameters.Add("@FullTermPremAmt" + countForCurrent.ToString(), amsInvoiceTransaction.FullTermPremAmt, DbType.Decimal);
				sqlParameters.Add("@GLDate" + countForCurrent.ToString(), _helper.FormatDate(amsInvoiceTransaction.GLDate + ""), DbType.DateTime);
				sqlParameters.Add("@GrossAmt" + countForCurrent.ToString(), amsInvoiceTransaction.GrossAmt, DbType.Decimal);
				sqlParameters.Add("@InvEffDate" + countForCurrent.ToString(), _helper.FormatDate(amsInvoiceTransaction.InvEffDate + ""), DbType.DateTime);
				sqlParameters.Add("@InvId" + countForCurrent.ToString(), amsInvoiceTransaction.InvId, DbType.String);
				sqlParameters.Add("@InvTPId" + countForCurrent.ToString(), amsInvoiceTransaction.InvTPId, DbType.String);
				sqlParameters.Add("@IsCancelled" + countForCurrent.ToString(), amsInvoiceTransaction.IsCancelled, DbType.String);
				sqlParameters.Add("@IsInstallment" + countForCurrent.ToString(), amsInvoiceTransaction.IsInstallment, DbType.String);
				sqlParameters.Add("@IsPosted" + countForCurrent.ToString(), amsInvoiceTransaction.IsPosted, DbType.String);
				sqlParameters.Add("@JournalTranId" + countForCurrent.ToString(), amsInvoiceTransaction.JournalTranId, DbType.String);
				sqlParameters.Add("@LineOfBus" + countForCurrent.ToString(), amsInvoiceTransaction.LineOfBus, DbType.String);
				sqlParameters.Add("@NewInvTPId" + countForCurrent.ToString(), amsInvoiceTransaction.NewInvTPId, DbType.String);
				sqlParameters.Add("@NonPrRecipient" + countForCurrent.ToString(), amsInvoiceTransaction.NonPrRecipient, DbType.String);
				sqlParameters.Add("@OldInvTPId" + countForCurrent.ToString(), amsInvoiceTransaction.OldInvTPId, DbType.String);
				sqlParameters.Add("@PlanType" + countForCurrent.ToString(), amsInvoiceTransaction.PlanType, DbType.String);
				sqlParameters.Add("@PolTEffDate" + countForCurrent.ToString(), _helper.FormatDate(amsInvoiceTransaction.PolTEffDate + ""), DbType.DateTime);
				sqlParameters.Add("@Poltpid" + countForCurrent.ToString(), amsInvoiceTransaction.Poltpid, DbType.String);
				sqlParameters.Add("@RBBkOutId" + countForCurrent.ToString(), amsInvoiceTransaction.RBBkOutId, DbType.String);
				sqlParameters.Add("@ReplaceDate" + countForCurrent.ToString(), _helper.FormatDate(amsInvoiceTransaction.ReplaceDate + ""), DbType.DateTime);
				sqlParameters.Add("@TranType" + countForCurrent.ToString(), amsInvoiceTransaction.TranType, DbType.String);
				sqlParameters.Add("@WritingCoCode" + countForCurrent.ToString(), amsInvoiceTransaction.WritingCoCode, DbType.String);

                sqlQuery += sqlToAdd;
            }
            int lastInsertCount = _dapper.ExecuteSqlWithParameters(sqlQuery.Trim(','), sqlParameters);
            if (lastInsertCount > 0)
            {
                rowsInserted += lastInsertCount;
                _logging.WriteLogForModel("InvoiceTransactions Load Completed: " + rowsInserted.ToString() + " Rows Inserted", _dataset);
                _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
            }
        }
    }
}