using System;
    using System.Text.Json.Serialization;
    
    namespace AMSDataLoad.Models
    {
        public partial class AMSBasicPolInfo
    	{ 
    		[JsonPropertyName("__$checksum")]
		public string? BasicPolInfoChecksum { get; set; }
		[JsonPropertyName("__$datasource")]
		public string? Datasource { get; set; }
		[JsonPropertyName("__$deleted")]
		public bool? Deleted { get; set; }
		[JsonPropertyName("__$sequencenumber")]
		public decimal? Sequencenumber { get; set; }
		[JsonPropertyName("agcybusclass")]
		public string? AgcyBusClass { get; set; }
		[JsonPropertyName("anotid")]
		public string? ANotId { get; set; }
		[JsonPropertyName("auditflag")]
		public string? AuditFlag { get; set; }
		[JsonPropertyName("auditperiod")]
		public string? AuditPeriod { get; set; }
		[JsonPropertyName("billacctno")]
		public string? BillAcctNo { get; set; }
		[JsonPropertyName("billedstmtprem")]
		public decimal? BilledStmtPrem { get; set; }
		[JsonPropertyName("billmethod")]
		public string? BillMethod { get; set; }
		[JsonPropertyName("brokercode")]
		public string? BrokerCode { get; set; }
		[JsonPropertyName("busorigincode")]
		public string? BusOriginCode { get; set; }
		[JsonPropertyName("carrierstatus")]
		public string? CarrierStatus { get; set; }
		[JsonPropertyName("changedby")]
		public string? ChangedBy { get; set; }
		[JsonPropertyName("changeddate")]
		public string? ChangedDate { get; set; }
		[JsonPropertyName("cocode")]
		public string? CoCode { get; set; }
		[JsonPropertyName("compcustno")]
		public string? CompCustNo { get; set; }
		[JsonPropertyName("cotype")]
		public string? CoType { get; set; }
		[JsonPropertyName("csrcode")]
		public string? CsrCode { get; set; }
		[JsonPropertyName("custid")]
		public string? CustId { get; set; }
		[JsonPropertyName("descriptionbpol")]
		public string? DescriptionBPol { get; set; }
		[JsonPropertyName("entereddate")]
		public string? EnteredDate { get; set; }
		[JsonPropertyName("excludefrmdownload")]
		public string? ExcludeFrmDownload { get; set; }
		[JsonPropertyName("execcode")]
		public string? ExecCode { get; set; }
		[JsonPropertyName("firstwrittendate")]
		public string? FirstWrittenDate { get; set; }
		[JsonPropertyName("flatamount")]
		public decimal? FlatAmount { get; set; }
		[JsonPropertyName("fulltermpremium")]
		public decimal? FullTermPremium { get; set; }
		[JsonPropertyName("glbrnchcode")]
		public string? GLBrnchCode { get; set; }
		[JsonPropertyName("gldeptcode")]
		public string? GLDeptCode { get; set; }
		[JsonPropertyName("gldivcode")]
		public string? GLDivCode { get; set; }
		[JsonPropertyName("glgrpcode")]
		public string? GLGrpCode { get; set; }
		[JsonPropertyName("instday")]
		public short? InstDay { get; set; }
		[JsonPropertyName("iscontinuous")]
		public string? IsContinuous { get; set; }
		[JsonPropertyName("isexcldelete")]
		public string? IsExclDelete { get; set; }
		[JsonPropertyName("isfiltered")]
		public string? IsFiltered { get; set; }
		[JsonPropertyName("isfinanced")]
		public string? IsFinanced { get; set; }
		[JsonPropertyName("ismultientity")]
		public string? IsMultiEntity { get; set; }
		[JsonPropertyName("isnewbpol")]
		public string? IsNewBPol { get; set; }
		[JsonPropertyName("isposted")]
		public string? IsPosted { get; set; }
		[JsonPropertyName("isprodcredrequire100")]
		public string? IsProdCredRequire100 { get; set; }
		[JsonPropertyName("isproductioncreditenabled")]
		public string? IsProductionCreditEnabled { get; set; }
		[JsonPropertyName("isreinsuranceenabled")]
		public string? IsReinsuranceEnabled { get; set; }
		[JsonPropertyName("issuedstate")]
		public string? IssuedState { get; set; }
		[JsonPropertyName("issuspended")]
		public string? IsSuspended { get; set; }
		[JsonPropertyName("istid")]
		public string? Istid { get; set; }
		[JsonPropertyName("masteragent")]
		public string? MasterAgent { get; set; }
		[JsonPropertyName("method")]
		public string? Method { get; set; }
		[JsonPropertyName("methodofpayments")]
		public string? MethodOfPayments { get; set; }
		[JsonPropertyName("multientityarflag")]
		public string? MultiEntityARFlag { get; set; }
		[JsonPropertyName("natlprodcode")]
		public long? NatlProdCode { get; set; }
		[JsonPropertyName("negcommvaliddate")]
		public string? NegCommValidDate { get; set; }
		[JsonPropertyName("notrenewable")]
		public string? NotRenewable { get; set; }
		[JsonPropertyName("numofpayments")]
		public short? NumOfPayments { get; set; }
		[JsonPropertyName("paypid")]
		public string? PayPId { get; set; }
		[JsonPropertyName("percentage")]
		public decimal? Percentage { get; set; }
		[JsonPropertyName("polchangeddate")]
		public string? PolChangedDate { get; set; }
		[JsonPropertyName("poleffdate")]
		public string? PolEffDate { get; set; }
		[JsonPropertyName("polexpdate")]
		public string? PolExpDate { get; set; }
		[JsonPropertyName("polid")]
		public string? PolId { get; set; }
		[JsonPropertyName("polno")]
		public string? PolNo { get; set; }
		[JsonPropertyName("polsubtype")]
		public string? PolSubType { get; set; }
		[JsonPropertyName("poltype")]
		public string? PolType { get; set; }
		[JsonPropertyName("poltypelob")]
		public string? PolTypeLOB { get; set; }
		[JsonPropertyName("premadj")]
		public string? PremAdj { get; set; }
		[JsonPropertyName("priorpolicy")]
		public string? PriorPolicy { get; set; }
		[JsonPropertyName("priorpolid")]
		public string? Priorpolid { get; set; }
		[JsonPropertyName("renewallist")]
		public string? RenewalList { get; set; }
		[JsonPropertyName("renewalrptflag")]
		public string? RenewalRptFlag { get; set; }
		[JsonPropertyName("shortpolno")]
		public string? ShortPolNo { get; set; }
		[JsonPropertyName("sourcepolid")]
		public string? SourcePolId { get; set; }
		[JsonPropertyName("status")]
		public string? BasicPolInfoStatus { get; set; }
		[JsonPropertyName("subagent")]
		public string? SubAgent { get; set; }
		[JsonPropertyName("ticomid")]
		public string? TiComId { get; set; }
		[JsonPropertyName("typeofbus")]
		public short? TypeOfBus { get; set; }
		[JsonPropertyName("underwriter")]
		public string? Underwriter { get; set; }
		[JsonPropertyName("writingcocode")]
		public string? WritingCoCode { get; set; }
		[JsonPropertyName("renewaltermcount")]
		public short? RenewalTermCount { get; set; }

	}
}