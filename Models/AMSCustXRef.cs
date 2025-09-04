using System;
    using System.Text.Json.Serialization;
    
    namespace AMSDataLoad.Models
    {
        public partial class AMSCustXRef
    	{ 
    		[JsonPropertyName("__$checksum")]
		public string? CustXRefChecksum { get; set; }
		[JsonPropertyName("__$datasource")]
		public string? Datasource { get; set; }
		[JsonPropertyName("__$deleted")]
		public bool? Deleted { get; set; }
		[JsonPropertyName("__$sequencenumber")]
		public decimal? Sequencenumber { get; set; }
		[JsonPropertyName("axrefid")]
		public string? AXRefId { get; set; }
		[JsonPropertyName("changedby")]
		public string? ChangedBy { get; set; }
		[JsonPropertyName("changeddate")]
		public string? ChangedDate { get; set; }
		[JsonPropertyName("custid")]
		public string? CustId { get; set; }
		[JsonPropertyName("cxrefid")]
		public string? CXRefId { get; set; }
		[JsonPropertyName("entereddate")]
		public string? EnteredDate { get; set; }
		[JsonPropertyName("xreference")]
		public string? XReference { get; set; }

	}
}