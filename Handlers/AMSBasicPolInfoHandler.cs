
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
    public class AMSBasicPolInfoHandler
    {
        private readonly LoggingHandler _logging;
        private readonly DataContextDapper _dapper;
        private readonly DateTime _startTime;
        // private readonly HttpClient _filterDate;
        private readonly Helper _helper = new Helper();
        private readonly RootReqHandler _rootReqHandler;
        private readonly string _dataset = "BasicPolInfo";
		private string _lastResponse = "";

        public AMSBasicPolInfoHandler(LoggingHandler logging, IConfiguration config, RootReqHandler rootReqHandler, DateTime startTime)
        {
            _logging = logging;
            _dapper = new DataContextDapper(config);
            _startTime = startTime;
            _rootReqHandler = rootReqHandler;
        }

        public async Task<string> GetAndInsertBasicPolInfo(string rootRequestUrl, string filterDateString)
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
                    string tableName = "AFW_BasicPolInfo?";
                    bool moreRecordsLeft = true;

                    _dapper.ExecuteSql("EXEC Staging.AMS_BasicPolInfo_Load");
                    _dapper.ExecuteSql("TRUNCATE TABLE RawData.AMS_BasicPolInfo");

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
                        APIResponse<AMSBasicPolInfo> apiResponse = JsonSerializer.Deserialize<APIResponse<AMSBasicPolInfo>>(dataString) ?? new APIResponse<AMSBasicPolInfo>();
                        activeStep = "Loading " + _dataset;
                        // emailBody = _logging.LogServerError(dataString, emailBody, activeStep, errorPage);
                        
                        
                        if (apiResponse.Result.Count() < topValue) { moreRecordsLeft = false; }
                        if (apiResponse.Result.Count() > 0)
                        {
                        CheckAndInsertBasicPolInfos(apiResponse.Result, activeStep);
                        _logging.WriteLogForModel("Loaded to RawData", _dataset);
                        }
                        

                        nextPage = apiResponse.StartingToken;
                    }
                    _dapper.ExecuteSql("EXEC Staging.AMS_BasicPolInfo_Load");

                    _logging.WriteLogForModel("BasicPolInfo Staging Load Completed", _dataset);
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

        public void CheckAndInsertBasicPolInfos(IEnumerable<AMSBasicPolInfo> amsBasicPolInfos, string activeStep)
        {
            if (amsBasicPolInfos != null && amsBasicPolInfos.Count() > 0)
            {
                _logging.WriteLogForModel(activeStep, _dataset);
                _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
                InsertBasicPolInfos(amsBasicPolInfos);
            }
            else
            {
                _logging.WriteLogForModel("No BasicPolInfos Found in Date Range", _dataset);
            }
        }

        public void InsertBasicPolInfos(IEnumerable<AMSBasicPolInfo> amsBasicPolInfos)
        {

            // string insertRoot = "INSERT INTO RawData.AMS_BasicPolInfo(BasicPolInfoChecksum,Datasource,Deleted,Sequencenumber,AgcyBusClass,ANotId,AuditFlag,AuditPeriod,BillAcctNo,BilledStmtPrem,BillMethod,BrokerCode,BusOriginCode,CarrierStatus,ChangedBy,ChangedDate,CoCode,CompCustNo,CoType,CsrCode,CustId,DescriptionBPol,EnteredDate,ExcludeFrmDownload,ExecCode,FirstWrittenDate,FlatAmount,FullTermPremium,GLBrnchCode,GLDeptCode,GLDivCode,GLGrpCode,InstDay,IsContinuous,IsExclDelete,IsFiltered,IsFinanced,IsMultiEntity,IsNewBPol,IsPosted,IsProdCredRequire100,IsProductionCreditEnabled,IsReinsuranceEnabled,IssuedState,IsSuspended,Istid,MasterAgent,Method,MethodOfPayments,MultiEntityARFlag,NatlProdCode,NegCommValidDate,NotRenewable,NumOfPayments,PayPId,Percentage,PolChangedDate,PolEffDate,PolExpDate,PolId,PolNo,PolSubType,PolType,PolTypeLOB,PremAdj,PriorPolicy,Priorpolid,RenewalList,RenewalRptFlag,ShortPolNo,SourcePolId,BasicPolInfoStatus,SubAgent,TiComId,TypeOfBus,Underwriter,WritingCoCode,RenewalTermCount,InsertDate) VALUES ";
            string insertRoot = "INSERT INTO RawData.AMS_BasicPolInfo VALUES ";
            string sqlQuery = insertRoot;

            int rowsInserted = 0;
            int countForCurrent = 0;
            DynamicParameters sqlParameters = new DynamicParameters();
            foreach (AMSBasicPolInfo amsBasicPolInfo in amsBasicPolInfos)
            {
                countForCurrent += 1;
                string sqlToAdd = "(@BasicPolInfoChecksum" + countForCurrent.ToString() +
					",@Datasource" + countForCurrent.ToString() +
					",@Deleted" + countForCurrent.ToString() +
					",@Sequencenumber" + countForCurrent.ToString() +
					",@AgcyBusClass" + countForCurrent.ToString() +
					",@ANotId" + countForCurrent.ToString() +
					",@AuditFlag" + countForCurrent.ToString() +
					",@AuditPeriod" + countForCurrent.ToString() +
					",@BillAcctNo" + countForCurrent.ToString() +
					",@BilledStmtPrem" + countForCurrent.ToString() +
					",@BillMethod" + countForCurrent.ToString() +
					",@BrokerCode" + countForCurrent.ToString() +
					",@BusOriginCode" + countForCurrent.ToString() +
					",@CarrierStatus" + countForCurrent.ToString() +
					",@ChangedBy" + countForCurrent.ToString() +
					",@ChangedDate" + countForCurrent.ToString() +
					",@CoCode" + countForCurrent.ToString() +
					",@CompCustNo" + countForCurrent.ToString() +
					",@CoType" + countForCurrent.ToString() +
					",@CsrCode" + countForCurrent.ToString() +
					",@CustId" + countForCurrent.ToString() +
					",@DescriptionBPol" + countForCurrent.ToString() +
					",@EnteredDate" + countForCurrent.ToString() +
					",@ExcludeFrmDownload" + countForCurrent.ToString() +
					",@ExecCode" + countForCurrent.ToString() +
					",@FirstWrittenDate" + countForCurrent.ToString() +
					",@FlatAmount" + countForCurrent.ToString() +
					",@FullTermPremium" + countForCurrent.ToString() +
					",@GLBrnchCode" + countForCurrent.ToString() +
					",@GLDeptCode" + countForCurrent.ToString() +
					",@GLDivCode" + countForCurrent.ToString() +
					",@GLGrpCode" + countForCurrent.ToString() +
					",@InstDay" + countForCurrent.ToString() +
					",@IsContinuous" + countForCurrent.ToString() +
					",@IsExclDelete" + countForCurrent.ToString() +
					",@IsFiltered" + countForCurrent.ToString() +
					",@IsFinanced" + countForCurrent.ToString() +
					",@IsMultiEntity" + countForCurrent.ToString() +
					",@IsNewBPol" + countForCurrent.ToString() +
					",@IsPosted" + countForCurrent.ToString() +
					",@IsProdCredRequire100" + countForCurrent.ToString() +
					",@IsProductionCreditEnabled" + countForCurrent.ToString() +
					",@IsReinsuranceEnabled" + countForCurrent.ToString() +
					",@IssuedState" + countForCurrent.ToString() +
					",@IsSuspended" + countForCurrent.ToString() +
					",@Istid" + countForCurrent.ToString() +
					",@MasterAgent" + countForCurrent.ToString() +
					",@Method" + countForCurrent.ToString() +
					",@MethodOfPayments" + countForCurrent.ToString() +
					",@MultiEntityARFlag" + countForCurrent.ToString() +
					",@NatlProdCode" + countForCurrent.ToString() +
					",@NegCommValidDate" + countForCurrent.ToString() +
					",@NotRenewable" + countForCurrent.ToString() +
					",@NumOfPayments" + countForCurrent.ToString() +
					",@PayPId" + countForCurrent.ToString() +
					",@Percentage" + countForCurrent.ToString() +
					",@PolChangedDate" + countForCurrent.ToString() +
					",@PolEffDate" + countForCurrent.ToString() +
					",@PolExpDate" + countForCurrent.ToString() +
					",@PolId" + countForCurrent.ToString() +
					",@PolNo" + countForCurrent.ToString() +
					",@PolSubType" + countForCurrent.ToString() +
					",@PolType" + countForCurrent.ToString() +
					",@PolTypeLOB" + countForCurrent.ToString() +
					",@PremAdj" + countForCurrent.ToString() +
					",@PriorPolicy" + countForCurrent.ToString() +
					",@Priorpolid" + countForCurrent.ToString() +
					",@RenewalList" + countForCurrent.ToString() +
					",@RenewalRptFlag" + countForCurrent.ToString() +
					",@ShortPolNo" + countForCurrent.ToString() +
					",@SourcePolId" + countForCurrent.ToString() +
					",@BasicPolInfoStatus" + countForCurrent.ToString() +
					",@SubAgent" + countForCurrent.ToString() +
					",@TiComId" + countForCurrent.ToString() +
					",@TypeOfBus" + countForCurrent.ToString() +
					",@Underwriter" + countForCurrent.ToString() +
					",@WritingCoCode" + countForCurrent.ToString() +
					",@RenewalTermCount" + countForCurrent.ToString() +
					",GETDATE()),";


                if ((sqlQuery + sqlToAdd).Length > 4000)
                {
                    rowsInserted += _dapper.ExecuteSqlWithParameters(sqlQuery.Trim(','), sqlParameters);
                    sqlParameters = new DynamicParameters();
					sqlToAdd = string.Join("0,", sqlToAdd.Split(countForCurrent.ToString() + ","));
                    sqlQuery = insertRoot;
                    countForCurrent = 0;
                }


				sqlParameters.Add("@BasicPolInfoChecksum" + countForCurrent.ToString(), amsBasicPolInfo.BasicPolInfoChecksum, DbType.String);
				sqlParameters.Add("@Datasource" + countForCurrent.ToString(), amsBasicPolInfo.Datasource, DbType.String);
				sqlParameters.Add("@Deleted" + countForCurrent.ToString(), amsBasicPolInfo.Deleted, DbType.Boolean);
				sqlParameters.Add("@Sequencenumber" + countForCurrent.ToString(), amsBasicPolInfo.Sequencenumber, DbType.Decimal);
				sqlParameters.Add("@AgcyBusClass" + countForCurrent.ToString(), amsBasicPolInfo.AgcyBusClass, DbType.String);
				sqlParameters.Add("@ANotId" + countForCurrent.ToString(), amsBasicPolInfo.ANotId, DbType.String);
				sqlParameters.Add("@AuditFlag" + countForCurrent.ToString(), amsBasicPolInfo.AuditFlag, DbType.String);
				sqlParameters.Add("@AuditPeriod" + countForCurrent.ToString(), amsBasicPolInfo.AuditPeriod, DbType.String);
				sqlParameters.Add("@BillAcctNo" + countForCurrent.ToString(), amsBasicPolInfo.BillAcctNo, DbType.String);
				sqlParameters.Add("@BilledStmtPrem" + countForCurrent.ToString(), amsBasicPolInfo.BilledStmtPrem, DbType.Decimal);
				sqlParameters.Add("@BillMethod" + countForCurrent.ToString(), amsBasicPolInfo.BillMethod, DbType.String);
				sqlParameters.Add("@BrokerCode" + countForCurrent.ToString(), amsBasicPolInfo.BrokerCode, DbType.String);
				sqlParameters.Add("@BusOriginCode" + countForCurrent.ToString(), amsBasicPolInfo.BusOriginCode, DbType.String);
				sqlParameters.Add("@CarrierStatus" + countForCurrent.ToString(), amsBasicPolInfo.CarrierStatus, DbType.String);
				sqlParameters.Add("@ChangedBy" + countForCurrent.ToString(), amsBasicPolInfo.ChangedBy, DbType.String);
				sqlParameters.Add("@ChangedDate" + countForCurrent.ToString(), _helper.FormatDate(amsBasicPolInfo.ChangedDate + ""), DbType.DateTime);
				sqlParameters.Add("@CoCode" + countForCurrent.ToString(), amsBasicPolInfo.CoCode, DbType.String);
				sqlParameters.Add("@CompCustNo" + countForCurrent.ToString(), amsBasicPolInfo.CompCustNo, DbType.String);
				sqlParameters.Add("@CoType" + countForCurrent.ToString(), amsBasicPolInfo.CoType, DbType.String);
				sqlParameters.Add("@CsrCode" + countForCurrent.ToString(), amsBasicPolInfo.CsrCode, DbType.String);
				sqlParameters.Add("@CustId" + countForCurrent.ToString(), amsBasicPolInfo.CustId, DbType.String);
				sqlParameters.Add("@DescriptionBPol" + countForCurrent.ToString(), amsBasicPolInfo.DescriptionBPol, DbType.String);
				sqlParameters.Add("@EnteredDate" + countForCurrent.ToString(), _helper.FormatDate(amsBasicPolInfo.EnteredDate + ""), DbType.DateTime);
				sqlParameters.Add("@ExcludeFrmDownload" + countForCurrent.ToString(), amsBasicPolInfo.ExcludeFrmDownload, DbType.String);
				sqlParameters.Add("@ExecCode" + countForCurrent.ToString(), amsBasicPolInfo.ExecCode, DbType.String);
				sqlParameters.Add("@FirstWrittenDate" + countForCurrent.ToString(), _helper.FormatDate(amsBasicPolInfo.FirstWrittenDate + ""), DbType.DateTime);
				sqlParameters.Add("@FlatAmount" + countForCurrent.ToString(), amsBasicPolInfo.FlatAmount, DbType.Decimal);
				sqlParameters.Add("@FullTermPremium" + countForCurrent.ToString(), amsBasicPolInfo.FullTermPremium, DbType.Decimal);
				sqlParameters.Add("@GLBrnchCode" + countForCurrent.ToString(), amsBasicPolInfo.GLBrnchCode, DbType.String);
				sqlParameters.Add("@GLDeptCode" + countForCurrent.ToString(), amsBasicPolInfo.GLDeptCode, DbType.String);
				sqlParameters.Add("@GLDivCode" + countForCurrent.ToString(), amsBasicPolInfo.GLDivCode, DbType.String);
				sqlParameters.Add("@GLGrpCode" + countForCurrent.ToString(), amsBasicPolInfo.GLGrpCode, DbType.String);
				sqlParameters.Add("@InstDay" + countForCurrent.ToString(), amsBasicPolInfo.InstDay, DbType.Int16);
				sqlParameters.Add("@IsContinuous" + countForCurrent.ToString(), amsBasicPolInfo.IsContinuous, DbType.String);
				sqlParameters.Add("@IsExclDelete" + countForCurrent.ToString(), amsBasicPolInfo.IsExclDelete, DbType.String);
				sqlParameters.Add("@IsFiltered" + countForCurrent.ToString(), amsBasicPolInfo.IsFiltered, DbType.String);
				sqlParameters.Add("@IsFinanced" + countForCurrent.ToString(), amsBasicPolInfo.IsFinanced, DbType.String);
				sqlParameters.Add("@IsMultiEntity" + countForCurrent.ToString(), amsBasicPolInfo.IsMultiEntity, DbType.String);
				sqlParameters.Add("@IsNewBPol" + countForCurrent.ToString(), amsBasicPolInfo.IsNewBPol, DbType.String);
				sqlParameters.Add("@IsPosted" + countForCurrent.ToString(), amsBasicPolInfo.IsPosted, DbType.String);
				sqlParameters.Add("@IsProdCredRequire100" + countForCurrent.ToString(), amsBasicPolInfo.IsProdCredRequire100, DbType.String);
				sqlParameters.Add("@IsProductionCreditEnabled" + countForCurrent.ToString(), amsBasicPolInfo.IsProductionCreditEnabled, DbType.String);
				sqlParameters.Add("@IsReinsuranceEnabled" + countForCurrent.ToString(), amsBasicPolInfo.IsReinsuranceEnabled, DbType.String);
				sqlParameters.Add("@IssuedState" + countForCurrent.ToString(), amsBasicPolInfo.IssuedState, DbType.String);
				sqlParameters.Add("@IsSuspended" + countForCurrent.ToString(), amsBasicPolInfo.IsSuspended, DbType.String);
				sqlParameters.Add("@Istid" + countForCurrent.ToString(), amsBasicPolInfo.Istid, DbType.String);
				sqlParameters.Add("@MasterAgent" + countForCurrent.ToString(), amsBasicPolInfo.MasterAgent, DbType.String);
				sqlParameters.Add("@Method" + countForCurrent.ToString(), amsBasicPolInfo.Method, DbType.String);
				sqlParameters.Add("@MethodOfPayments" + countForCurrent.ToString(), amsBasicPolInfo.MethodOfPayments, DbType.String);
				sqlParameters.Add("@MultiEntityARFlag" + countForCurrent.ToString(), amsBasicPolInfo.MultiEntityARFlag, DbType.String);
				sqlParameters.Add("@NatlProdCode" + countForCurrent.ToString(), amsBasicPolInfo.NatlProdCode, DbType.Int64);
				sqlParameters.Add("@NegCommValidDate" + countForCurrent.ToString(), _helper.FormatDate(amsBasicPolInfo.NegCommValidDate + ""), DbType.DateTime);
				sqlParameters.Add("@NotRenewable" + countForCurrent.ToString(), amsBasicPolInfo.NotRenewable, DbType.String);
				sqlParameters.Add("@NumOfPayments" + countForCurrent.ToString(), amsBasicPolInfo.NumOfPayments, DbType.Int16);
				sqlParameters.Add("@PayPId" + countForCurrent.ToString(), amsBasicPolInfo.PayPId, DbType.String);
				sqlParameters.Add("@Percentage" + countForCurrent.ToString(), amsBasicPolInfo.Percentage, DbType.Decimal);
				sqlParameters.Add("@PolChangedDate" + countForCurrent.ToString(), _helper.FormatDate(amsBasicPolInfo.PolChangedDate + ""), DbType.DateTime);
				sqlParameters.Add("@PolEffDate" + countForCurrent.ToString(), _helper.FormatDate(amsBasicPolInfo.PolEffDate + ""), DbType.DateTime);
				sqlParameters.Add("@PolExpDate" + countForCurrent.ToString(), _helper.FormatDate(amsBasicPolInfo.PolExpDate + ""), DbType.DateTime);
				sqlParameters.Add("@PolId" + countForCurrent.ToString(), amsBasicPolInfo.PolId, DbType.String);
				sqlParameters.Add("@PolNo" + countForCurrent.ToString(), amsBasicPolInfo.PolNo, DbType.String);
				sqlParameters.Add("@PolSubType" + countForCurrent.ToString(), amsBasicPolInfo.PolSubType, DbType.String);
				sqlParameters.Add("@PolType" + countForCurrent.ToString(), amsBasicPolInfo.PolType, DbType.String);
				sqlParameters.Add("@PolTypeLOB" + countForCurrent.ToString(), amsBasicPolInfo.PolTypeLOB, DbType.String);
				sqlParameters.Add("@PremAdj" + countForCurrent.ToString(), amsBasicPolInfo.PremAdj, DbType.String);
				sqlParameters.Add("@PriorPolicy" + countForCurrent.ToString(), amsBasicPolInfo.PriorPolicy, DbType.String);
				sqlParameters.Add("@Priorpolid" + countForCurrent.ToString(), amsBasicPolInfo.Priorpolid, DbType.String);
				sqlParameters.Add("@RenewalList" + countForCurrent.ToString(), amsBasicPolInfo.RenewalList, DbType.String);
				sqlParameters.Add("@RenewalRptFlag" + countForCurrent.ToString(), amsBasicPolInfo.RenewalRptFlag, DbType.String);
				sqlParameters.Add("@ShortPolNo" + countForCurrent.ToString(), amsBasicPolInfo.ShortPolNo, DbType.String);
				sqlParameters.Add("@SourcePolId" + countForCurrent.ToString(), amsBasicPolInfo.SourcePolId, DbType.String);
				sqlParameters.Add("@BasicPolInfoStatus" + countForCurrent.ToString(), amsBasicPolInfo.BasicPolInfoStatus, DbType.String);
				sqlParameters.Add("@SubAgent" + countForCurrent.ToString(), amsBasicPolInfo.SubAgent, DbType.String);
				sqlParameters.Add("@TiComId" + countForCurrent.ToString(), amsBasicPolInfo.TiComId, DbType.String);
				sqlParameters.Add("@TypeOfBus" + countForCurrent.ToString(), amsBasicPolInfo.TypeOfBus, DbType.Int16);
				sqlParameters.Add("@Underwriter" + countForCurrent.ToString(), amsBasicPolInfo.Underwriter, DbType.String);
				sqlParameters.Add("@WritingCoCode" + countForCurrent.ToString(), amsBasicPolInfo.WritingCoCode, DbType.String);
				sqlParameters.Add("@RenewalTermCount" + countForCurrent.ToString(), amsBasicPolInfo.RenewalTermCount, DbType.Int16);

                sqlQuery += sqlToAdd;
            }
            int lastInsertCount = _dapper.ExecuteSqlWithParameters(sqlQuery.Trim(','), sqlParameters);
            if (lastInsertCount > 0)
            {
                rowsInserted += lastInsertCount;
                _logging.WriteLogForModel("BasicPolInfos Load Completed: " + rowsInserted.ToString() + " Rows Inserted", _dataset);
                _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
            }
        }
    }
}