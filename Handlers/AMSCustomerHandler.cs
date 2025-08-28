
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
    public class AMSCustomerHandler
    {
        private readonly LoggingHandler _logging;
        private readonly DataContextDapper _dapper;
        private readonly DateTime _startTime;
        // private readonly HttpClient _filterDate;
        private readonly Helper _helper = new Helper();
        private readonly RootReqHandler _rootReqHandler;
        private readonly string _dataset = "Customer";
		private string _lastResponse = "";

        public AMSCustomerHandler(LoggingHandler logging, IConfiguration config, RootReqHandler rootReqHandler, DateTime startTime)
        {
            _logging = logging;
            _dapper = new DataContextDapper(config);
            _startTime = startTime;
            _rootReqHandler = rootReqHandler;
        }

        public async Task<string> GetAndInsertCustomer(string rootRequestUrl, string filterDateString)
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
                    string tableName = "AFW_Customer?";
                    bool moreRecordsLeft = true;

                    _dapper.ExecuteSql("EXEC Staging.AMS_Customer_Load");
                    _dapper.ExecuteSql("TRUNCATE TABLE RawData.AMS_Customer");

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
                        APIResponse<AMSCustomer> apiResponse = JsonSerializer.Deserialize<APIResponse<AMSCustomer>>(dataString) ?? new APIResponse<AMSCustomer>();
                        activeStep = "Loading " + _dataset;
                        // emailBody = _logging.LogServerError(dataString, emailBody, activeStep, errorPage);
                        
                        
                        if (apiResponse.Result.Count() < topValue) { moreRecordsLeft = false; }
                        if (apiResponse.Result.Count() > 0)
                        {
                        CheckAndInsertCustomers(apiResponse.Result, activeStep);
                        _logging.WriteLogForModel("Loaded to RawData", _dataset);
                        }
                        

                        nextPage = apiResponse.StartingToken;
                    }
                    _dapper.ExecuteSql("EXEC Staging.AMS_Customer_Load");

                    _logging.WriteLogForModel("Customer Staging Load Completed", _dataset);
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

        public void CheckAndInsertCustomers(IEnumerable<AMSCustomer> amsCustomers, string activeStep)
        {
            if (amsCustomers != null && amsCustomers.Count() > 0)
            {
                _logging.WriteLogForModel(activeStep, _dataset);
                _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
                InsertCustomers(amsCustomers);
            }
            else
            {
                _logging.WriteLogForModel("No Customers Found in Date Range", _dataset);
            }
        }

        public void InsertCustomers(IEnumerable<AMSCustomer> amsCustomers)
        {

            // string insertRoot = "INSERT INTO RawData.AMS_Customer(CustomerChecksum,Datasource,Deleted,Sequencenumber,AcquisitionCode,Active,Addr1,Addr2,ANotId,ARClosedDate,ARClosedStatus,AutoApplyCR,AutoApplyPay,BillAddr1,BillAddr2,BillCity,BillCounty,BillName,BillState,BillZip,BrokerCode,BusAreaCode,BusExt,BusFullPhone,BusOriginCode,BusPhone,BusSince,ChangedBy,ChangedDate,City,CollectLetter,ContactMethod,Country,County,CsrCode,CustAddedDate,CustId,CustNo,DBA,DOB,DriversLicense,DUNSNo,ElectronicDelivery,EMail,EMail2,EnteredDate,FaxAreaCode,FaxExt,FaxFullPhone,FaxPhone,FedIdNo,FirmNameCust,FirstName,FormalSalutation,GLBrnchCode,GLCode,GLDeptCode,GLDivCode,GLGrpCode,GroupingOption,InformalSalutation,IsBillAddrSameAsCust,IsBillNameSameAsCust,IsBrokCust,IsDeriveAttrFlagsCust,IsExclDelete,IsForeign,IsPrintAgencyBill,IsPrintDirectBill,IsSecured,JoinCriteria,KnownSince,LastName,LateCharge,LatitudeCust,LLId,LongitudeCust,MarineAreaCode,MarineExt,MarineFullPhone,MarinePhone,MarketingSolicitation,Married,MasterCustId,MasterSubTrack,MasterSubType,MethodOfDistribution,MidName,MktgFlag,NAICS,NonPC,Notes,Occupation,OtherAreaCode,OtherExt,OtherFullPhone,OtherPhone,PagerAreaCode,PagerExt,PagerFullPhone,PagerPhone,PermAttrFlagsCust,PremOption,PrintCustStmt,Prod1Code,ReferralLocationCode,ReferralNameCode,ReportOption,ResAreaCode,ResExt,ResFullPhone,ResPhone,Sex,SIC,SortName,SSN,CustomerState,StatePrintGroup,TypeCust,TypeEntity,TypeName,TypeOfBusiness,UserAttrFlagsCust,WebAddr,YearEmployed,ZipCode,EducationLevel,InsertDate) VALUES ";
            string insertRoot = "INSERT INTO RawData.AMS_Customer VALUES ";
            string sqlQuery = insertRoot;

            int rowsInserted = 0;
            int countForCurrent = 0;
            DynamicParameters sqlParameters = new DynamicParameters();
            foreach (AMSCustomer amsCustomer in amsCustomers)
            {
                countForCurrent += 1;
                string sqlToAdd = "(@CustomerChecksum" + countForCurrent.ToString() +
					",@Datasource" + countForCurrent.ToString() +
					",@Deleted" + countForCurrent.ToString() +
					",@Sequencenumber" + countForCurrent.ToString() +
					",@AcquisitionCode" + countForCurrent.ToString() +
					",@Active" + countForCurrent.ToString() +
					",@Addr1" + countForCurrent.ToString() +
					",@Addr2" + countForCurrent.ToString() +
					",@ANotId" + countForCurrent.ToString() +
					",@ARClosedDate" + countForCurrent.ToString() +
					",@ARClosedStatus" + countForCurrent.ToString() +
					",@AutoApplyCR" + countForCurrent.ToString() +
					",@AutoApplyPay" + countForCurrent.ToString() +
					",@BillAddr1" + countForCurrent.ToString() +
					",@BillAddr2" + countForCurrent.ToString() +
					",@BillCity" + countForCurrent.ToString() +
					",@BillCounty" + countForCurrent.ToString() +
					",@BillName" + countForCurrent.ToString() +
					",@BillState" + countForCurrent.ToString() +
					",@BillZip" + countForCurrent.ToString() +
					",@BrokerCode" + countForCurrent.ToString() +
					",@BusAreaCode" + countForCurrent.ToString() +
					",@BusExt" + countForCurrent.ToString() +
					",@BusFullPhone" + countForCurrent.ToString() +
					",@BusOriginCode" + countForCurrent.ToString() +
					",@BusPhone" + countForCurrent.ToString() +
					",@BusSince" + countForCurrent.ToString() +
					",@ChangedBy" + countForCurrent.ToString() +
					",@ChangedDate" + countForCurrent.ToString() +
					",@City" + countForCurrent.ToString() +
					",@CollectLetter" + countForCurrent.ToString() +
					",@ContactMethod" + countForCurrent.ToString() +
					",@Country" + countForCurrent.ToString() +
					",@County" + countForCurrent.ToString() +
					",@CsrCode" + countForCurrent.ToString() +
					",@CustAddedDate" + countForCurrent.ToString() +
					",@CustId" + countForCurrent.ToString() +
					",@CustNo" + countForCurrent.ToString() +
					",@DBA" + countForCurrent.ToString() +
					",@DOB" + countForCurrent.ToString() +
					",@DriversLicense" + countForCurrent.ToString() +
					",@DUNSNo" + countForCurrent.ToString() +
					",@ElectronicDelivery" + countForCurrent.ToString() +
					",@EMail" + countForCurrent.ToString() +
					",@EMail2" + countForCurrent.ToString() +
					",@EnteredDate" + countForCurrent.ToString() +
					",@FaxAreaCode" + countForCurrent.ToString() +
					",@FaxExt" + countForCurrent.ToString() +
					",@FaxFullPhone" + countForCurrent.ToString() +
					",@FaxPhone" + countForCurrent.ToString() +
					",@FedIdNo" + countForCurrent.ToString() +
					",@FirmNameCust" + countForCurrent.ToString() +
					",@FirstName" + countForCurrent.ToString() +
					",@FormalSalutation" + countForCurrent.ToString() +
					",@GLBrnchCode" + countForCurrent.ToString() +
					",@GLCode" + countForCurrent.ToString() +
					",@GLDeptCode" + countForCurrent.ToString() +
					",@GLDivCode" + countForCurrent.ToString() +
					",@GLGrpCode" + countForCurrent.ToString() +
					",@GroupingOption" + countForCurrent.ToString() +
					",@InformalSalutation" + countForCurrent.ToString() +
					",@IsBillAddrSameAsCust" + countForCurrent.ToString() +
					",@IsBillNameSameAsCust" + countForCurrent.ToString() +
					",@IsBrokCust" + countForCurrent.ToString() +
					",@IsDeriveAttrFlagsCust" + countForCurrent.ToString() +
					",@IsExclDelete" + countForCurrent.ToString() +
					",@IsForeign" + countForCurrent.ToString() +
					",@IsPrintAgencyBill" + countForCurrent.ToString() +
					",@IsPrintDirectBill" + countForCurrent.ToString() +
					",@IsSecured" + countForCurrent.ToString() +
					",@JoinCriteria" + countForCurrent.ToString() +
					",@KnownSince" + countForCurrent.ToString() +
					",@LastName" + countForCurrent.ToString() +
					",@LateCharge" + countForCurrent.ToString() +
					",@LatitudeCust" + countForCurrent.ToString() +
					",@LLId" + countForCurrent.ToString() +
					",@LongitudeCust" + countForCurrent.ToString() +
					",@MarineAreaCode" + countForCurrent.ToString() +
					",@MarineExt" + countForCurrent.ToString() +
					",@MarineFullPhone" + countForCurrent.ToString() +
					",@MarinePhone" + countForCurrent.ToString() +
					",@MarketingSolicitation" + countForCurrent.ToString() +
					",@Married" + countForCurrent.ToString() +
					",@MasterCustId" + countForCurrent.ToString() +
					",@MasterSubTrack" + countForCurrent.ToString() +
					",@MasterSubType" + countForCurrent.ToString() +
					",@MethodOfDistribution" + countForCurrent.ToString() +
					",@MidName" + countForCurrent.ToString() +
					",@MktgFlag" + countForCurrent.ToString() +
					",@NAICS" + countForCurrent.ToString() +
					",@NonPC" + countForCurrent.ToString() +
					",@Notes" + countForCurrent.ToString() +
					",@Occupation" + countForCurrent.ToString() +
					",@OtherAreaCode" + countForCurrent.ToString() +
					",@OtherExt" + countForCurrent.ToString() +
					",@OtherFullPhone" + countForCurrent.ToString() +
					",@OtherPhone" + countForCurrent.ToString() +
					",@PagerAreaCode" + countForCurrent.ToString() +
					",@PagerExt" + countForCurrent.ToString() +
					",@PagerFullPhone" + countForCurrent.ToString() +
					",@PagerPhone" + countForCurrent.ToString() +
					",@PermAttrFlagsCust" + countForCurrent.ToString() +
					",@PremOption" + countForCurrent.ToString() +
					",@PrintCustStmt" + countForCurrent.ToString() +
					",@Prod1Code" + countForCurrent.ToString() +
					",@ReferralLocationCode" + countForCurrent.ToString() +
					",@ReferralNameCode" + countForCurrent.ToString() +
					",@ReportOption" + countForCurrent.ToString() +
					",@ResAreaCode" + countForCurrent.ToString() +
					",@ResExt" + countForCurrent.ToString() +
					",@ResFullPhone" + countForCurrent.ToString() +
					",@ResPhone" + countForCurrent.ToString() +
					",@Sex" + countForCurrent.ToString() +
					",@SIC" + countForCurrent.ToString() +
					",@SortName" + countForCurrent.ToString() +
					",@SSN" + countForCurrent.ToString() +
					",@CustomerState" + countForCurrent.ToString() +
					",@StatePrintGroup" + countForCurrent.ToString() +
					",@TypeCust" + countForCurrent.ToString() +
					",@TypeEntity" + countForCurrent.ToString() +
					",@TypeName" + countForCurrent.ToString() +
					",@TypeOfBusiness" + countForCurrent.ToString() +
					",@UserAttrFlagsCust" + countForCurrent.ToString() +
					",@WebAddr" + countForCurrent.ToString() +
					",@YearEmployed" + countForCurrent.ToString() +
					",@ZipCode" + countForCurrent.ToString() +
					",@EducationLevel" + countForCurrent.ToString() +
					",GETDATE()),";


                if ((sqlQuery + sqlToAdd).Length > 4000)
                {
                    rowsInserted += _dapper.ExecuteSqlWithParameters(sqlQuery.Trim(','), sqlParameters);
                    sqlParameters = new DynamicParameters();
					sqlToAdd = string.Join("0,", sqlToAdd.Split(countForCurrent.ToString() + ","));
                    sqlQuery = insertRoot;
                    countForCurrent = 0;
                }


				sqlParameters.Add("@CustomerChecksum" + countForCurrent.ToString(), amsCustomer.CustomerChecksum, DbType.String);
				sqlParameters.Add("@Datasource" + countForCurrent.ToString(), amsCustomer.Datasource, DbType.String);
				sqlParameters.Add("@Deleted" + countForCurrent.ToString(), amsCustomer.Deleted, DbType.Boolean);
				sqlParameters.Add("@Sequencenumber" + countForCurrent.ToString(), amsCustomer.Sequencenumber, DbType.Decimal);
				sqlParameters.Add("@AcquisitionCode" + countForCurrent.ToString(), amsCustomer.AcquisitionCode, DbType.String);
				sqlParameters.Add("@Active" + countForCurrent.ToString(), amsCustomer.Active, DbType.String);
				sqlParameters.Add("@Addr1" + countForCurrent.ToString(), amsCustomer.Addr1, DbType.String);
				sqlParameters.Add("@Addr2" + countForCurrent.ToString(), amsCustomer.Addr2, DbType.String);
				sqlParameters.Add("@ANotId" + countForCurrent.ToString(), amsCustomer.ANotId, DbType.String);
				sqlParameters.Add("@ARClosedDate" + countForCurrent.ToString(), _helper.FormatDate(amsCustomer.ARClosedDate + ""), DbType.DateTime);
				sqlParameters.Add("@ARClosedStatus" + countForCurrent.ToString(), amsCustomer.ARClosedStatus, DbType.String);
				sqlParameters.Add("@AutoApplyCR" + countForCurrent.ToString(), amsCustomer.AutoApplyCR, DbType.String);
				sqlParameters.Add("@AutoApplyPay" + countForCurrent.ToString(), amsCustomer.AutoApplyPay, DbType.String);
				sqlParameters.Add("@BillAddr1" + countForCurrent.ToString(), amsCustomer.BillAddr1, DbType.String);
				sqlParameters.Add("@BillAddr2" + countForCurrent.ToString(), amsCustomer.BillAddr2, DbType.String);
				sqlParameters.Add("@BillCity" + countForCurrent.ToString(), amsCustomer.BillCity, DbType.String);
				sqlParameters.Add("@BillCounty" + countForCurrent.ToString(), amsCustomer.BillCounty, DbType.String);
				sqlParameters.Add("@BillName" + countForCurrent.ToString(), amsCustomer.BillName, DbType.String);
				sqlParameters.Add("@BillState" + countForCurrent.ToString(), amsCustomer.BillState, DbType.String);
				sqlParameters.Add("@BillZip" + countForCurrent.ToString(), amsCustomer.BillZip, DbType.String);
				sqlParameters.Add("@BrokerCode" + countForCurrent.ToString(), amsCustomer.BrokerCode, DbType.String);
				sqlParameters.Add("@BusAreaCode" + countForCurrent.ToString(), amsCustomer.BusAreaCode, DbType.String);
				sqlParameters.Add("@BusExt" + countForCurrent.ToString(), amsCustomer.BusExt, DbType.String);
				sqlParameters.Add("@BusFullPhone" + countForCurrent.ToString(), amsCustomer.BusFullPhone, DbType.String);
				sqlParameters.Add("@BusOriginCode" + countForCurrent.ToString(), amsCustomer.BusOriginCode, DbType.String);
				sqlParameters.Add("@BusPhone" + countForCurrent.ToString(), amsCustomer.BusPhone, DbType.String);
				sqlParameters.Add("@BusSince" + countForCurrent.ToString(), amsCustomer.BusSince, DbType.String);
				sqlParameters.Add("@ChangedBy" + countForCurrent.ToString(), amsCustomer.ChangedBy, DbType.String);
				sqlParameters.Add("@ChangedDate" + countForCurrent.ToString(), _helper.FormatDate(amsCustomer.ChangedDate + ""), DbType.DateTime);
				sqlParameters.Add("@City" + countForCurrent.ToString(), amsCustomer.City, DbType.String);
				sqlParameters.Add("@CollectLetter" + countForCurrent.ToString(), amsCustomer.CollectLetter, DbType.String);
				sqlParameters.Add("@ContactMethod" + countForCurrent.ToString(), amsCustomer.ContactMethod, DbType.String);
				sqlParameters.Add("@Country" + countForCurrent.ToString(), amsCustomer.Country, DbType.String);
				sqlParameters.Add("@County" + countForCurrent.ToString(), amsCustomer.County, DbType.String);
				sqlParameters.Add("@CsrCode" + countForCurrent.ToString(), amsCustomer.CsrCode, DbType.String);
				sqlParameters.Add("@CustAddedDate" + countForCurrent.ToString(), _helper.FormatDate(amsCustomer.CustAddedDate + ""), DbType.DateTime);
				sqlParameters.Add("@CustId" + countForCurrent.ToString(), amsCustomer.CustId, DbType.String);
				sqlParameters.Add("@CustNo" + countForCurrent.ToString(), amsCustomer.CustNo, DbType.Int32);
				sqlParameters.Add("@DBA" + countForCurrent.ToString(), amsCustomer.DBA, DbType.String);
				sqlParameters.Add("@DOB" + countForCurrent.ToString(), _helper.FormatDate(amsCustomer.DOB + ""), DbType.DateTime);
				sqlParameters.Add("@DriversLicense" + countForCurrent.ToString(), amsCustomer.DriversLicense, DbType.String);
				sqlParameters.Add("@DUNSNo" + countForCurrent.ToString(), amsCustomer.DUNSNo, DbType.String);
				sqlParameters.Add("@ElectronicDelivery" + countForCurrent.ToString(), amsCustomer.ElectronicDelivery, DbType.String);
				sqlParameters.Add("@EMail" + countForCurrent.ToString(), amsCustomer.EMail, DbType.String);
				sqlParameters.Add("@EMail2" + countForCurrent.ToString(), amsCustomer.EMail2, DbType.String);
				sqlParameters.Add("@EnteredDate" + countForCurrent.ToString(), _helper.FormatDate(amsCustomer.EnteredDate + ""), DbType.DateTime);
				sqlParameters.Add("@FaxAreaCode" + countForCurrent.ToString(), amsCustomer.FaxAreaCode, DbType.String);
				sqlParameters.Add("@FaxExt" + countForCurrent.ToString(), amsCustomer.FaxExt, DbType.String);
				sqlParameters.Add("@FaxFullPhone" + countForCurrent.ToString(), amsCustomer.FaxFullPhone, DbType.String);
				sqlParameters.Add("@FaxPhone" + countForCurrent.ToString(), amsCustomer.FaxPhone, DbType.String);
				sqlParameters.Add("@FedIdNo" + countForCurrent.ToString(), amsCustomer.FedIdNo, DbType.String);
				sqlParameters.Add("@FirmNameCust" + countForCurrent.ToString(), amsCustomer.FirmNameCust, DbType.String);
				sqlParameters.Add("@FirstName" + countForCurrent.ToString(), amsCustomer.FirstName, DbType.String);
				sqlParameters.Add("@FormalSalutation" + countForCurrent.ToString(), amsCustomer.FormalSalutation, DbType.String);
				sqlParameters.Add("@GLBrnchCode" + countForCurrent.ToString(), amsCustomer.GLBrnchCode, DbType.String);
				sqlParameters.Add("@GLCode" + countForCurrent.ToString(), amsCustomer.GLCode, DbType.String);
				sqlParameters.Add("@GLDeptCode" + countForCurrent.ToString(), amsCustomer.GLDeptCode, DbType.String);
				sqlParameters.Add("@GLDivCode" + countForCurrent.ToString(), amsCustomer.GLDivCode, DbType.String);
				sqlParameters.Add("@GLGrpCode" + countForCurrent.ToString(), amsCustomer.GLGrpCode, DbType.String);
				sqlParameters.Add("@GroupingOption" + countForCurrent.ToString(), amsCustomer.GroupingOption, DbType.String);
				sqlParameters.Add("@InformalSalutation" + countForCurrent.ToString(), amsCustomer.InformalSalutation, DbType.String);
				sqlParameters.Add("@IsBillAddrSameAsCust" + countForCurrent.ToString(), amsCustomer.IsBillAddrSameAsCust, DbType.String);
				sqlParameters.Add("@IsBillNameSameAsCust" + countForCurrent.ToString(), amsCustomer.IsBillNameSameAsCust, DbType.String);
				sqlParameters.Add("@IsBrokCust" + countForCurrent.ToString(), amsCustomer.IsBrokCust, DbType.String);
				sqlParameters.Add("@IsDeriveAttrFlagsCust" + countForCurrent.ToString(), amsCustomer.IsDeriveAttrFlagsCust, DbType.String);
				sqlParameters.Add("@IsExclDelete" + countForCurrent.ToString(), amsCustomer.IsExclDelete, DbType.String);
				sqlParameters.Add("@IsForeign" + countForCurrent.ToString(), amsCustomer.IsForeign, DbType.Boolean);
				sqlParameters.Add("@IsPrintAgencyBill" + countForCurrent.ToString(), amsCustomer.IsPrintAgencyBill, DbType.String);
				sqlParameters.Add("@IsPrintDirectBill" + countForCurrent.ToString(), amsCustomer.IsPrintDirectBill, DbType.String);
				sqlParameters.Add("@IsSecured" + countForCurrent.ToString(), amsCustomer.IsSecured, DbType.String);
				sqlParameters.Add("@JoinCriteria" + countForCurrent.ToString(), amsCustomer.JoinCriteria, DbType.String);
				sqlParameters.Add("@KnownSince" + countForCurrent.ToString(), amsCustomer.KnownSince, DbType.String);
				sqlParameters.Add("@LastName" + countForCurrent.ToString(), amsCustomer.LastName, DbType.String);
				sqlParameters.Add("@LateCharge" + countForCurrent.ToString(), amsCustomer.LateCharge, DbType.String);
				sqlParameters.Add("@LatitudeCust" + countForCurrent.ToString(), amsCustomer.LatitudeCust, DbType.Decimal);
				sqlParameters.Add("@LLId" + countForCurrent.ToString(), amsCustomer.LLId, DbType.String);
				sqlParameters.Add("@LongitudeCust" + countForCurrent.ToString(), amsCustomer.LongitudeCust, DbType.Decimal);
				sqlParameters.Add("@MarineAreaCode" + countForCurrent.ToString(), amsCustomer.MarineAreaCode, DbType.String);
				sqlParameters.Add("@MarineExt" + countForCurrent.ToString(), amsCustomer.MarineExt, DbType.String);
				sqlParameters.Add("@MarineFullPhone" + countForCurrent.ToString(), amsCustomer.MarineFullPhone, DbType.String);
				sqlParameters.Add("@MarinePhone" + countForCurrent.ToString(), amsCustomer.MarinePhone, DbType.String);
				sqlParameters.Add("@MarketingSolicitation" + countForCurrent.ToString(), amsCustomer.MarketingSolicitation, DbType.String);
				sqlParameters.Add("@Married" + countForCurrent.ToString(), amsCustomer.Married, DbType.String);
				sqlParameters.Add("@MasterCustId" + countForCurrent.ToString(), amsCustomer.MasterCustId, DbType.String);
				sqlParameters.Add("@MasterSubTrack" + countForCurrent.ToString(), amsCustomer.MasterSubTrack, DbType.String);
				sqlParameters.Add("@MasterSubType" + countForCurrent.ToString(), amsCustomer.MasterSubType, DbType.String);
				sqlParameters.Add("@MethodOfDistribution" + countForCurrent.ToString(), amsCustomer.MethodOfDistribution, DbType.String);
				sqlParameters.Add("@MidName" + countForCurrent.ToString(), amsCustomer.MidName, DbType.String);
				sqlParameters.Add("@MktgFlag" + countForCurrent.ToString(), amsCustomer.MktgFlag, DbType.String);
				sqlParameters.Add("@NAICS" + countForCurrent.ToString(), amsCustomer.NAICS, DbType.String);
				sqlParameters.Add("@NonPC" + countForCurrent.ToString(), amsCustomer.NonPC, DbType.String);
				sqlParameters.Add("@Notes" + countForCurrent.ToString(), amsCustomer.Notes, DbType.String);
				sqlParameters.Add("@Occupation" + countForCurrent.ToString(), amsCustomer.Occupation, DbType.String);
				sqlParameters.Add("@OtherAreaCode" + countForCurrent.ToString(), amsCustomer.OtherAreaCode, DbType.String);
				sqlParameters.Add("@OtherExt" + countForCurrent.ToString(), amsCustomer.OtherExt, DbType.String);
				sqlParameters.Add("@OtherFullPhone" + countForCurrent.ToString(), amsCustomer.OtherFullPhone, DbType.String);
				sqlParameters.Add("@OtherPhone" + countForCurrent.ToString(), amsCustomer.OtherPhone, DbType.String);
				sqlParameters.Add("@PagerAreaCode" + countForCurrent.ToString(), amsCustomer.PagerAreaCode, DbType.String);
				sqlParameters.Add("@PagerExt" + countForCurrent.ToString(), amsCustomer.PagerExt, DbType.String);
				sqlParameters.Add("@PagerFullPhone" + countForCurrent.ToString(), amsCustomer.PagerFullPhone, DbType.String);
				sqlParameters.Add("@PagerPhone" + countForCurrent.ToString(), amsCustomer.PagerPhone, DbType.String);
				sqlParameters.Add("@PermAttrFlagsCust" + countForCurrent.ToString(), amsCustomer.PermAttrFlagsCust, DbType.String);
				sqlParameters.Add("@PremOption" + countForCurrent.ToString(), amsCustomer.PremOption, DbType.String);
				sqlParameters.Add("@PrintCustStmt" + countForCurrent.ToString(), amsCustomer.PrintCustStmt, DbType.String);
				sqlParameters.Add("@Prod1Code" + countForCurrent.ToString(), amsCustomer.Prod1Code, DbType.String);
				sqlParameters.Add("@ReferralLocationCode" + countForCurrent.ToString(), amsCustomer.ReferralLocationCode, DbType.String);
				sqlParameters.Add("@ReferralNameCode" + countForCurrent.ToString(), amsCustomer.ReferralNameCode, DbType.String);
				sqlParameters.Add("@ReportOption" + countForCurrent.ToString(), amsCustomer.ReportOption, DbType.String);
				sqlParameters.Add("@ResAreaCode" + countForCurrent.ToString(), amsCustomer.ResAreaCode, DbType.String);
				sqlParameters.Add("@ResExt" + countForCurrent.ToString(), amsCustomer.ResExt, DbType.String);
				sqlParameters.Add("@ResFullPhone" + countForCurrent.ToString(), amsCustomer.ResFullPhone, DbType.String);
				sqlParameters.Add("@ResPhone" + countForCurrent.ToString(), amsCustomer.ResPhone, DbType.String);
				sqlParameters.Add("@Sex" + countForCurrent.ToString(), amsCustomer.Sex, DbType.String);
				sqlParameters.Add("@SIC" + countForCurrent.ToString(), amsCustomer.SIC, DbType.String);
				sqlParameters.Add("@SortName" + countForCurrent.ToString(), amsCustomer.SortName, DbType.String);
				sqlParameters.Add("@SSN" + countForCurrent.ToString(), amsCustomer.SSN, DbType.String);
				sqlParameters.Add("@CustomerState" + countForCurrent.ToString(), amsCustomer.CustomerState, DbType.String);
				sqlParameters.Add("@StatePrintGroup" + countForCurrent.ToString(), amsCustomer.StatePrintGroup, DbType.String);
				sqlParameters.Add("@TypeCust" + countForCurrent.ToString(), amsCustomer.TypeCust, DbType.String);
				sqlParameters.Add("@TypeEntity" + countForCurrent.ToString(), amsCustomer.TypeEntity, DbType.String);
				sqlParameters.Add("@TypeName" + countForCurrent.ToString(), amsCustomer.TypeName, DbType.String);
				sqlParameters.Add("@TypeOfBusiness" + countForCurrent.ToString(), amsCustomer.TypeOfBusiness, DbType.String);
				sqlParameters.Add("@UserAttrFlagsCust" + countForCurrent.ToString(), amsCustomer.UserAttrFlagsCust, DbType.String);
				sqlParameters.Add("@WebAddr" + countForCurrent.ToString(), amsCustomer.WebAddr, DbType.String);
				sqlParameters.Add("@YearEmployed" + countForCurrent.ToString(), amsCustomer.YearEmployed, DbType.String);
				sqlParameters.Add("@ZipCode" + countForCurrent.ToString(), amsCustomer.ZipCode, DbType.String);
				sqlParameters.Add("@EducationLevel" + countForCurrent.ToString(), amsCustomer.EducationLevel, DbType.String);

                sqlQuery += sqlToAdd;
            }
            int lastInsertCount = _dapper.ExecuteSqlWithParameters(sqlQuery.Trim(','), sqlParameters);
            if (lastInsertCount > 0)
            {
                rowsInserted += lastInsertCount;
                _logging.WriteLogForModel("Customers Load Completed: " + rowsInserted.ToString() + " Rows Inserted", _dataset);
                _logging.WriteLogForModel((DateTime.Now - _startTime).TotalSeconds.ToString(), _dataset);
            }
        }
    }
}