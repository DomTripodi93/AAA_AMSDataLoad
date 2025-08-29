using System;
    using System.Text.Json.Serialization;
    
    namespace AMSDataLoad.Models
    {
        public partial class AMSInvoiceTransaction
    	{ 
    		[JsonPropertyName("__$checksum")]
		public string? InvoiceTransactionChecksum { get; set; }
		[JsonPropertyName("__$datasource")]
		public string? Datasource { get; set; }
		[JsonPropertyName("__$deleted")]
		public bool? Deleted { get; set; }
		[JsonPropertyName("__$sequencenumber")]
		public decimal? Sequencenumber { get; set; }
		[JsonPropertyName("backinvtpid")]
		public string? BackInvTPId { get; set; }
		[JsonPropertyName("bhid")]
		public string? BHId { get; set; }
		[JsonPropertyName("billmethod")]
		public string? BillMethod { get; set; }
		[JsonPropertyName("billseqid")]
		public string? BillSeqId { get; set; }
		[JsonPropertyName("billtranid")]
		public string? BillTranId { get; set; }
		[JsonPropertyName("binderpolteffdate")]
		public string? BinderPolTEffDate { get; set; }
		[JsonPropertyName("binderpostmethod")]
		public string? BinderPostMethod { get; set; }
		[JsonPropertyName("binderstatus")]
		public string? BinderStatus { get; set; }
		[JsonPropertyName("brokercode")]
		public string? BrokerCode { get; set; }
		[JsonPropertyName("changedby")]
		public string? ChangedBy { get; set; }
		[JsonPropertyName("changeddate")]
		public string? ChangedDate { get; set; }
		[JsonPropertyName("chargecat")]
		public string? ChargeCat { get; set; }
		[JsonPropertyName("chargecode")]
		public string? Chargecode { get; set; }
		[JsonPropertyName("cocode")]
		public string? CoCode { get; set; }
		[JsonPropertyName("commpaytype")]
		public string? CommPayType { get; set; }
		[JsonPropertyName("cotype")]
		public string? CoType { get; set; }
		[JsonPropertyName("cshid")]
		public string? CSHId { get; set; }
		[JsonPropertyName("custid")]
		public string? CustId { get; set; }
		[JsonPropertyName("description")]
		public string? Description { get; set; }
		[JsonPropertyName("entereddate")]
		public string? EnteredDate { get; set; }
		[JsonPropertyName("fulltermpremamt")]
		public decimal? FullTermPremAmt { get; set; }
		[JsonPropertyName("gldate")]
		public string? GLDate { get; set; }
		[JsonPropertyName("grossamt")]
		public decimal? GrossAmt { get; set; }
		[JsonPropertyName("inveffdate")]
		public string? InvEffDate { get; set; }
		[JsonPropertyName("invid")]
		public string? InvId { get; set; }
		[JsonPropertyName("invtpid")]
		public string? InvTPId { get; set; }
		[JsonPropertyName("iscancelled")]
		public string? IsCancelled { get; set; }
		[JsonPropertyName("isinstallment")]
		public string? IsInstallment { get; set; }
		[JsonPropertyName("isposted")]
		public string? IsPosted { get; set; }
		[JsonPropertyName("journaltranid")]
		public string? JournalTranId { get; set; }
		[JsonPropertyName("lineofbus")]
		public string? LineOfBus { get; set; }
		[JsonPropertyName("newinvtpid")]
		public string? NewInvTPId { get; set; }
		[JsonPropertyName("nonprrecipient")]
		public string? NonPrRecipient { get; set; }
		[JsonPropertyName("oldinvtpid")]
		public string? OldInvTPId { get; set; }
		[JsonPropertyName("plantype")]
		public string? PlanType { get; set; }
		[JsonPropertyName("polteffdate")]
		public string? PolTEffDate { get; set; }
		[JsonPropertyName("poltpid")]
		public string? Poltpid { get; set; }
		[JsonPropertyName("rbbkoutid")]
		public string? RBBkOutId { get; set; }
		[JsonPropertyName("replacedate")]
		public string? ReplaceDate { get; set; }
		[JsonPropertyName("trantype")]
		public string? TranType { get; set; }
		[JsonPropertyName("writingcocode")]
		public string? WritingCoCode { get; set; }

	}
}