using System.Text.Json.Serialization;
namespace AMSDataLoad.Models
{
    public class TokenObject
    {
        [JsonPropertyName("access_token")]
        public string JWT { get; set; } = "";
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}