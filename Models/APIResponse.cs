
using System.Text.Json.Serialization;

namespace AMSDataLoad.Models
{
    public partial class APIResponse<T>
    {

        [JsonPropertyName("content")]
        public IEnumerable<T> Result { get; set; } = new List<T>();
        [JsonPropertyName("starting_token")]
        public string StartingToken { get; set; } = "";
        [JsonPropertyName("recordCount")]
        public int RecordCount { get; set; }

    }
}
