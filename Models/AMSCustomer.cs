using System;
    using System.Text.Json.Serialization;
    
    namespace AMSDataLoad.Models
    {
        public partial class AMSCustomer
    	{ 
    		[JsonPropertyName("__$checksum")]
		public string? CustomerChecksum { get; set; }
		[JsonPropertyName("__$datasource")]
		public string? Datasource { get; set; }
		[JsonPropertyName("__$deleted")]
		public bool? Deleted { get; set; }
		[JsonPropertyName("__$sequencenumber")]
		public decimal? Sequencenumber { get; set; }
		[JsonPropertyName("acquisitioncode")]
		public string? AcquisitionCode { get; set; }
		[JsonPropertyName("active")]
		public string? Active { get; set; }
		[JsonPropertyName("addr1")]
		public string? Addr1 { get; set; }
		[JsonPropertyName("addr2")]
		public string? Addr2 { get; set; }
		[JsonPropertyName("anotid")]
		public string? ANotId { get; set; }
		[JsonPropertyName("arcloseddate")]
		public string? ARClosedDate { get; set; }
		[JsonPropertyName("arclosedstatus")]
		public string? ARClosedStatus { get; set; }
		[JsonPropertyName("autoapplycr")]
		public string? AutoApplyCR { get; set; }
		[JsonPropertyName("autoapplypay")]
		public string? AutoApplyPay { get; set; }
		[JsonPropertyName("billaddr1")]
		public string? BillAddr1 { get; set; }
		[JsonPropertyName("billaddr2")]
		public string? BillAddr2 { get; set; }
		[JsonPropertyName("billcity")]
		public string? BillCity { get; set; }
		[JsonPropertyName("billcounty")]
		public string? BillCounty { get; set; }
		[JsonPropertyName("billname")]
		public string? BillName { get; set; }
		[JsonPropertyName("billstate")]
		public string? BillState { get; set; }
		[JsonPropertyName("billzip")]
		public string? BillZip { get; set; }
		[JsonPropertyName("brokercode")]
		public string? BrokerCode { get; set; }
		[JsonPropertyName("busareacode")]
		public string? BusAreaCode { get; set; }
		[JsonPropertyName("busext")]
		public string? BusExt { get; set; }
		[JsonPropertyName("busfullphone")]
		public string? BusFullPhone { get; set; }
		[JsonPropertyName("busorigincode")]
		public string? BusOriginCode { get; set; }
		[JsonPropertyName("busphone")]
		public string? BusPhone { get; set; }
		[JsonPropertyName("bussince")]
		public string? BusSince { get; set; }
		[JsonPropertyName("changedby")]
		public string? ChangedBy { get; set; }
		[JsonPropertyName("changeddate")]
		public string? ChangedDate { get; set; }
		[JsonPropertyName("city")]
		public string? City { get; set; }
		[JsonPropertyName("collectletter")]
		public string? CollectLetter { get; set; }
		[JsonPropertyName("contactmethod")]
		public string? ContactMethod { get; set; }
		[JsonPropertyName("country")]
		public string? Country { get; set; }
		[JsonPropertyName("county")]
		public string? County { get; set; }
		[JsonPropertyName("csrcode")]
		public string? CsrCode { get; set; }
		[JsonPropertyName("custaddeddate")]
		public string? CustAddedDate { get; set; }
		[JsonPropertyName("custid")]
		public string? CustId { get; set; }
		[JsonPropertyName("custno")]
		public int? CustNo { get; set; }
		[JsonPropertyName("dba")]
		public string? DBA { get; set; }
		[JsonPropertyName("dob")]
		public string? DOB { get; set; }
		[JsonPropertyName("driverslicense")]
		public string? DriversLicense { get; set; }
		[JsonPropertyName("dunsno")]
		public string? DUNSNo { get; set; }
		[JsonPropertyName("electronicdelivery")]
		public string? ElectronicDelivery { get; set; }
		[JsonPropertyName("email")]
		public string? EMail { get; set; }
		[JsonPropertyName("email2")]
		public string? EMail2 { get; set; }
		[JsonPropertyName("entereddate")]
		public string? EnteredDate { get; set; }
		[JsonPropertyName("faxareacode")]
		public string? FaxAreaCode { get; set; }
		[JsonPropertyName("faxext")]
		public string? FaxExt { get; set; }
		[JsonPropertyName("faxfullphone")]
		public string? FaxFullPhone { get; set; }
		[JsonPropertyName("faxphone")]
		public string? FaxPhone { get; set; }
		[JsonPropertyName("fedidno")]
		public string? FedIdNo { get; set; }
		[JsonPropertyName("firmnamecust")]
		public string? FirmNameCust { get; set; }
		[JsonPropertyName("firstname")]
		public string? FirstName { get; set; }
		[JsonPropertyName("formalsalutation")]
		public string? FormalSalutation { get; set; }
		[JsonPropertyName("glbrnchcode")]
		public string? GLBrnchCode { get; set; }
		[JsonPropertyName("glcode")]
		public string? GLCode { get; set; }
		[JsonPropertyName("gldeptcode")]
		public string? GLDeptCode { get; set; }
		[JsonPropertyName("gldivcode")]
		public string? GLDivCode { get; set; }
		[JsonPropertyName("glgrpcode")]
		public string? GLGrpCode { get; set; }
		[JsonPropertyName("groupingoption")]
		public string? GroupingOption { get; set; }
		[JsonPropertyName("informalsalutation")]
		public string? InformalSalutation { get; set; }
		[JsonPropertyName("isbilladdrsameascust")]
		public string? IsBillAddrSameAsCust { get; set; }
		[JsonPropertyName("isbillnamesameascust")]
		public string? IsBillNameSameAsCust { get; set; }
		[JsonPropertyName("isbrokcust")]
		public string? IsBrokCust { get; set; }
		[JsonPropertyName("isderiveattrflagscust")]
		public string? IsDeriveAttrFlagsCust { get; set; }
		[JsonPropertyName("isexcldelete")]
		public string? IsExclDelete { get; set; }
		[JsonPropertyName("isforeign")]
		public bool? IsForeign { get; set; }
		[JsonPropertyName("isprintagencybill")]
		public string? IsPrintAgencyBill { get; set; }
		[JsonPropertyName("isprintdirectbill")]
		public string? IsPrintDirectBill { get; set; }
		[JsonPropertyName("issecured")]
		public string? IsSecured { get; set; }
		[JsonPropertyName("joincriteria")]
		public string? JoinCriteria { get; set; }
		[JsonPropertyName("knownsince")]
		public string? KnownSince { get; set; }
		[JsonPropertyName("lastname")]
		public string? LastName { get; set; }
		[JsonPropertyName("latecharge")]
		public string? LateCharge { get; set; }
		[JsonPropertyName("latitudecust")]
		public decimal? LatitudeCust { get; set; }
		[JsonPropertyName("llid")]
		public string? LLId { get; set; }
		[JsonPropertyName("longitudecust")]
		public decimal? LongitudeCust { get; set; }
		[JsonPropertyName("marineareacode")]
		public string? MarineAreaCode { get; set; }
		[JsonPropertyName("marineext")]
		public string? MarineExt { get; set; }
		[JsonPropertyName("marinefullphone")]
		public string? MarineFullPhone { get; set; }
		[JsonPropertyName("marinephone")]
		public string? MarinePhone { get; set; }
		[JsonPropertyName("marketingsolicitation")]
		public string? MarketingSolicitation { get; set; }
		[JsonPropertyName("married")]
		public string? Married { get; set; }
		[JsonPropertyName("mastercustid")]
		public string? MasterCustId { get; set; }
		[JsonPropertyName("mastersubtrack")]
		public string? MasterSubTrack { get; set; }
		[JsonPropertyName("mastersubtype")]
		public string? MasterSubType { get; set; }
		[JsonPropertyName("methodofdistribution")]
		public string? MethodOfDistribution { get; set; }
		[JsonPropertyName("midname")]
		public string? MidName { get; set; }
		[JsonPropertyName("mktgflag")]
		public string? MktgFlag { get; set; }
		[JsonPropertyName("naics")]
		public string? NAICS { get; set; }
		[JsonPropertyName("nonpc")]
		public string? NonPC { get; set; }
		[JsonPropertyName("notes")]
		public string? Notes { get; set; }
		[JsonPropertyName("occupation")]
		public string? Occupation { get; set; }
		[JsonPropertyName("otherareacode")]
		public string? OtherAreaCode { get; set; }
		[JsonPropertyName("otherext")]
		public string? OtherExt { get; set; }
		[JsonPropertyName("otherfullphone")]
		public string? OtherFullPhone { get; set; }
		[JsonPropertyName("otherphone")]
		public string? OtherPhone { get; set; }
		[JsonPropertyName("pagerareacode")]
		public string? PagerAreaCode { get; set; }
		[JsonPropertyName("pagerext")]
		public string? PagerExt { get; set; }
		[JsonPropertyName("pagerfullphone")]
		public string? PagerFullPhone { get; set; }
		[JsonPropertyName("pagerphone")]
		public string? PagerPhone { get; set; }
		[JsonPropertyName("permattrflagscust")]
		public string? PermAttrFlagsCust { get; set; }
		[JsonPropertyName("premoption")]
		public string? PremOption { get; set; }
		[JsonPropertyName("printcuststmt")]
		public string? PrintCustStmt { get; set; }
		[JsonPropertyName("prod1code")]
		public string? Prod1Code { get; set; }
		[JsonPropertyName("referrallocationcode")]
		public string? ReferralLocationCode { get; set; }
		[JsonPropertyName("referralnamecode")]
		public string? ReferralNameCode { get; set; }
		[JsonPropertyName("reportoption")]
		public string? ReportOption { get; set; }
		[JsonPropertyName("resareacode")]
		public string? ResAreaCode { get; set; }
		[JsonPropertyName("resext")]
		public string? ResExt { get; set; }
		[JsonPropertyName("resfullphone")]
		public string? ResFullPhone { get; set; }
		[JsonPropertyName("resphone")]
		public string? ResPhone { get; set; }
		[JsonPropertyName("sex")]
		public string? Sex { get; set; }
		[JsonPropertyName("sic")]
		public string? SIC { get; set; }
		[JsonPropertyName("sortname")]
		public string? SortName { get; set; }
		[JsonPropertyName("ssn")]
		public string? SSN { get; set; }
		[JsonPropertyName("state")]
		public string? CustomerState { get; set; }
		[JsonPropertyName("stateprintgroup")]
		public string? StatePrintGroup { get; set; }
		[JsonPropertyName("typecust")]
		public string? TypeCust { get; set; }
		[JsonPropertyName("typeentity")]
		public string? TypeEntity { get; set; }
		[JsonPropertyName("typename")]
		public string? TypeName { get; set; }
		[JsonPropertyName("typeofbusiness")]
		public string? TypeOfBusiness { get; set; }
		[JsonPropertyName("userattrflagscust")]
		public string? UserAttrFlagsCust { get; set; }
		[JsonPropertyName("webaddr")]
		public string? WebAddr { get; set; }
		[JsonPropertyName("yearemployed")]
		public string? YearEmployed { get; set; }
		[JsonPropertyName("zipcode")]
		public string? ZipCode { get; set; }
		[JsonPropertyName("educationlevel")]
		public string? EducationLevel { get; set; }

	}
}