using System;
    using System.Text.Json.Serialization;
    
    namespace AMSDataLoad.Models
    {
        public partial class AMS130Policy
    	{ 
    		[JsonPropertyName("__$checksum")]
		public string? PolicyChecksum { get; set; }
		[JsonPropertyName("__$datasource")]
		public string? Datasource { get; set; }
		[JsonPropertyName("__$deleted")]
		public bool? Deleted { get; set; }
		[JsonPropertyName("__$sequencenumber")]
		public decimal? Sequencenumber { get; set; }
		[JsonPropertyName("addinfo")]
		public string? AddInfo { get; set; }
		[JsonPropertyName("changedby")]
		public string? ChangedBy { get; set; }
		[JsonPropertyName("changeddate")]
		public string? ChangedDate { get; set; }
		[JsonPropertyName("dividendplan")]
		public string? DividendPlan { get; set; }
		[JsonPropertyName("effdate")]
		public string? EffDate { get; set; }
		[JsonPropertyName("employerno")]
		public string? EmployerNo { get; set; }
		[JsonPropertyName("entereddate")]
		public string? Entereddate { get; set; }
		[JsonPropertyName("includeexclude")]
		public string? IncludeExclude { get; set; }
		[JsonPropertyName("ispart1")]
		public string? IsPart1 { get; set; }
		[JsonPropertyName("isparticipating")]
		public string? IsParticipating { get; set; }
		[JsonPropertyName("issafetygroup")]
		public string? IsSafetyGroup { get; set; }
		[JsonPropertyName("lobid")]
		public string? LOBId { get; set; }
		[JsonPropertyName("nccino")]
		public string? NCCINo { get; set; }
		[JsonPropertyName("otherno")]
		public string? OtherNo { get; set; }
		[JsonPropertyName("polid")]
		public string? PolId { get; set; }
		[JsonPropertyName("ratingdate")]
		public string? RatingDate { get; set; }
		[JsonPropertyName("retroplan")]
		public string? RetroPlan { get; set; }
		[JsonPropertyName("retroyrs")]
		public string? RetroYrs { get; set; }
		[JsonPropertyName("state")]
		public string? PolicyState { get; set; }
		[JsonPropertyName("status")]
		public string? PolicyStatus { get; set; }
		[JsonPropertyName("wpolid")]
		public string? WPolId { get; set; }

	}
}