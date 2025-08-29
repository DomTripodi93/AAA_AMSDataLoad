using System;
    using System.Text.Json.Serialization;
    
    namespace AMSDataLoad.Models
    {
        public partial class AMSCustContact
    	{ 
    		[JsonPropertyName("__$checksum")]
		public string? CustContactChecksum { get; set; }
		[JsonPropertyName("__$datasource")]
		public string? Datasource { get; set; }
		[JsonPropertyName("__$deleted")]
		public bool? Deleted { get; set; }
		[JsonPropertyName("__$sequencenumber")]
		public decimal? Sequencenumber { get; set; }
		[JsonPropertyName("address1")]
		public string? Address1 { get; set; }
		[JsonPropertyName("address2")]
		public string? Address2 { get; set; }
		[JsonPropertyName("busareacode")]
		public string? BusAreaCode { get; set; }
		[JsonPropertyName("busext")]
		public string? BusExt { get; set; }
		[JsonPropertyName("busphone")]
		public string? BusPhone { get; set; }
		[JsonPropertyName("ccntid")]
		public string? CCntId { get; set; }
		[JsonPropertyName("changedby")]
		public string? ChangedBy { get; set; }
		[JsonPropertyName("changeddate")]
		public string? ChangedDate { get; set; }
		[JsonPropertyName("city")]
		public string? City { get; set; }
		[JsonPropertyName("contactmethod")]
		public string? ContactMethod { get; set; }
		[JsonPropertyName("contactname")]
		public string? ContactName { get; set; }
		[JsonPropertyName("country")]
		public string? Country { get; set; }
		[JsonPropertyName("county")]
		public string? County { get; set; }
		[JsonPropertyName("custcenterdisplay")]
		public string? CustCenterDisplay { get; set; }
		[JsonPropertyName("custid")]
		public string? CustId { get; set; }
		[JsonPropertyName("email")]
		public string? EMail { get; set; }
		[JsonPropertyName("entereddate")]
		public string? EnteredDate { get; set; }
		[JsonPropertyName("faxareacode")]
		public string? FaxAreaCode { get; set; }
		[JsonPropertyName("faxext")]
		public string? FaxExt { get; set; }
		[JsonPropertyName("faxphone")]
		public string? FaxPhone { get; set; }
		[JsonPropertyName("formalsalutation")]
		public string? FormalSalutation { get; set; }
		[JsonPropertyName("informalsalutation")]
		public string? InformalSalutation { get; set; }
		[JsonPropertyName("isdirector")]
		public string? Isdirector { get; set; }
		[JsonPropertyName("isforeign")]
		public bool? IsForeign { get; set; }
		[JsonPropertyName("isofficer")]
		public string? IsOfficer { get; set; }
		[JsonPropertyName("mobileareacode")]
		public string? MobileAreaCode { get; set; }
		[JsonPropertyName("mobileext")]
		public string? MobileExt { get; set; }
		[JsonPropertyName("mobilephone")]
		public string? MobilePhone { get; set; }
		[JsonPropertyName("notes")]
		public string? Notes { get; set; }
		[JsonPropertyName("pagerareacode")]
		public string? PagerAreaCode { get; set; }
		[JsonPropertyName("pagerext")]
		public string? PagerExt { get; set; }
		[JsonPropertyName("pagerphone")]
		public string? PagerPhone { get; set; }
		[JsonPropertyName("resareacode")]
		public string? ResAreaCode { get; set; }
		[JsonPropertyName("resext")]
		public string? ResExt { get; set; }
		[JsonPropertyName("resphone")]
		public string? ResPhone { get; set; }
		[JsonPropertyName("sortorder")]
		public short? SortOrder { get; set; }
		[JsonPropertyName("state")]
		public string? CustContactState { get; set; }
		[JsonPropertyName("title")]
		public string? Title { get; set; }
		[JsonPropertyName("zip")]
		public string? Zip { get; set; }

	}
}