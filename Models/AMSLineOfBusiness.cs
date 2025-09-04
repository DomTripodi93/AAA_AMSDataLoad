using System;
    using System.Text.Json.Serialization;
    
    namespace AMSDataLoad.Models
    {
        public partial class AMSLineOfBusiness
    	{ 
    		[JsonPropertyName("__$checksum")]
		public string? LineOfBusinessChecksum { get; set; }
		[JsonPropertyName("__$datasource")]
		public string? Datasource { get; set; }
		[JsonPropertyName("__$deleted")]
		public bool? Deleted { get; set; }
		[JsonPropertyName("__$sequencenumber")]
		public decimal? Sequencenumber { get; set; }
		[JsonPropertyName("appcreateddate")]
		public string? AppCreatedDate { get; set; }
		[JsonPropertyName("changedby")]
		public string? ChangedBy { get; set; }
		[JsonPropertyName("changeddate")]
		public string? ChangedDate { get; set; }
		[JsonPropertyName("description")]
		public string? Description { get; set; }
		[JsonPropertyName("effdate")]
		public string? EffDate { get; set; }
		[JsonPropertyName("elfformverid")]
		public string? ElfFormVerId { get; set; }
		[JsonPropertyName("entereddate")]
		public string? EnteredDate { get; set; }
		[JsonPropertyName("expdate")]
		public string? ExpDate { get; set; }
		[JsonPropertyName("insertseqno")]
		public int? InsertSeqNo { get; set; }
		[JsonPropertyName("lineofbus")]
		public string? LineOfBus { get; set; }
		[JsonPropertyName("lobchangeddate")]
		public string? LOBChangedDate { get; set; }
		[JsonPropertyName("lobid")]
		public string? LOBId { get; set; }
		[JsonPropertyName("plantype")]
		public string? PlanType { get; set; }
		[JsonPropertyName("polid")]
		public string? PolId { get; set; }
		[JsonPropertyName("sortno")]
		public short? SortNo { get; set; }
		[JsonPropertyName("stateplantype")]
		public string? StatePlanType { get; set; }
		[JsonPropertyName("uicodelobs")]
		public string? UICodeLOBS { get; set; }
		[JsonPropertyName("writingcocode")]
		public string? WritingCoCode { get; set; }

	}
}