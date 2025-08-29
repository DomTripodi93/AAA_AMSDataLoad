using System;
    using System.Text.Json.Serialization;
    
    namespace AMSDataLoad.Models
    {
        public partial class AMSPolicyTransaction
    	{ 
    		[JsonPropertyName("__$checksum")]
		public string? PolicyTransactionChecksum { get; set; }
		[JsonPropertyName("__$datasource")]
		public string? Datasource { get; set; }
		[JsonPropertyName("__$deleted")]
		public bool? Deleted { get; set; }
		[JsonPropertyName("__$sequencenumber")]
		public decimal? Sequencenumber { get; set; }
		[JsonPropertyName("billednonprem")]
		public decimal? BilledNonPrem { get; set; }
		[JsonPropertyName("billmethodpolt")]
		public string? BillMethodPolT { get; set; }
		[JsonPropertyName("binderreplacepolteffdate")]
		public string? BinderReplacePolTEffDate { get; set; }
		[JsonPropertyName("changedby")]
		public string? ChangedBy { get; set; }
		[JsonPropertyName("changeddate")]
		public string? ChangedDate { get; set; }
		[JsonPropertyName("description")]
		public string? Description { get; set; }
		[JsonPropertyName("effdate")]
		public string? EffDate { get; set; }
		[JsonPropertyName("entereddate")]
		public string? EnteredDate { get; set; }
		[JsonPropertyName("estrevenuepercent")]
		public decimal? EstRevenuePercent { get; set; }
		[JsonPropertyName("instdaypolt")]
		public short? InstDayPolT { get; set; }
		[JsonPropertyName("isposted")]
		public string? IsPosted { get; set; }
		[JsonPropertyName("isuploaded")]
		public string? IsUploaded { get; set; }
		[JsonPropertyName("paypid")]
		public string? PayPId { get; set; }
		[JsonPropertyName("polid")]
		public string? PolId { get; set; }
		[JsonPropertyName("premoneffdate")]
		public decimal? PremOnEffDate { get; set; }
		[JsonPropertyName("reasonpolt")]
		public string? ReasonPolT { get; set; }
		[JsonPropertyName("replacedatepolt")]
		public string? ReplaceDatePolT { get; set; }
		[JsonPropertyName("source")]
		public string? Source { get; set; }
		[JsonPropertyName("trantype")]
		public string? TranType { get; set; }
		[JsonPropertyName("annualizedestrevenue")]
		public string? Annualizedestrevenue { get; set; }
		[JsonPropertyName("annualizedpremium")]
		public string? Annualizedpremium { get; set; }

	}
}