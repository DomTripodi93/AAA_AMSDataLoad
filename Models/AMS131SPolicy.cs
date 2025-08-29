using System;
    using System.Text.Json.Serialization;
    
    namespace AMSDataLoad.Models
    {
        public partial class AMS131SPolicy
    	{ 
    		[JsonPropertyName("__$checksum")]
		public string? SPolicyChecksum { get; set; }
		[JsonPropertyName("__$datasource")]
		public string? Datasource { get; set; }
		[JsonPropertyName("__$deleted")]
		public bool? Deleted { get; set; }
		[JsonPropertyName("__$sequencenumber")]
		public decimal? Sequencenumber { get; set; }
		[JsonPropertyName("changedby")]
		public string? ChangedBy { get; set; }
		[JsonPropertyName("changeddate")]
		public string? ChangedDate { get; set; }
		[JsonPropertyName("curretrodate")]
		public string? CurRetroDate { get; set; }
		[JsonPropertyName("effdate")]
		public string? EffDate { get; set; }
		[JsonPropertyName("entereddate")]
		public string? EnteredDate { get; set; }
		[JsonPropertyName("expirpolno")]
		public string? ExpirPolNo { get; set; }
		[JsonPropertyName("isdollaryes")]
		public string? IsDollarYes { get; set; }
		[JsonPropertyName("isumbrella")]
		public string? IsUmbrella { get; set; }
		[JsonPropertyName("lobid")]
		public string? LOBId { get; set; }
		[JsonPropertyName("polid")]
		public string? PolId { get; set; }
		[JsonPropertyName("propretrodate")]
		public string? PropRetroDate { get; set; }
		[JsonPropertyName("status")]
		public string? SPolicyStatus { get; set; }
		[JsonPropertyName("upolid")]
		public string? UPolId { get; set; }

	}
}