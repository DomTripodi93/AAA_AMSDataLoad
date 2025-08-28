
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Security;
using Dapper;
using System.Text;
using System.Text.Json;
using AMSDataLoad.Data;
using AMSDataLoad.Models;

namespace AMSDataLoad.Handlers
{
    public class RootReqHandler
    {
        private readonly LoggingHandler _logging;
        private readonly DataContextDapper _dapper;
        private HttpClient _client;
        private DateTime _startTime;
        string _decryptedClientId;
        string _decryptedClientSecret;
        IConfiguration _config;
        public RootReqHandler(LoggingHandler logging, IConfiguration config, DateTime startTime, string decryptedClientId, string decryptedClientSecret)
        {
            _logging = logging;
            _dapper = new DataContextDapper(config);
            _config = config;
            _startTime = startTime;
            _decryptedClientId = decryptedClientId;
            _decryptedClientSecret = decryptedClientSecret;
            _client = new HttpClient();
            if (_decryptedClientId != "" && _decryptedClientSecret != "")
            {
                // Console.WriteLine(_decryptedCredentials);
                _client = GetAuthenticatedClient();
            }
        }

        public Dictionary<string, string> GetRootUrls(string reqUrl)
        {
            var content = new StringContent("{}", Encoding.UTF8, "application/json");

            Dictionary<string, string> rootResult = new Dictionary<string, string>();

            var response = _client.GetAsync(new Uri(reqUrl)).Result;
            var contents = response.Content.ReadAsStringAsync().Result;

            //Console.WriteLine(contents);
            Dictionary<string, string>? userResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(contents);
            if (userResponse != null)
            {
                rootResult = userResponse;
            }

            return rootResult;
        }

        public int GetReqLimitPerMin()
        {

            // Get current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert to Central Time
            TimeZoneInfo centralTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            DateTime centralNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, centralTimeZone);

            // Extract the time component
            TimeSpan time = centralNow.TimeOfDay;

            // Check if time is between 10 PM (22:00) and 4 AM (04:00) the next day
            if ((time >= new TimeSpan(22, 0, 0)) || (time < new TimeSpan(4, 0, 0)))
            {
                return 60;
            }
            else
            {
                return 30;
            }
        }

        public HttpClient GetAuthenticatedClient()
        {
            HttpClient authClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://login.apps.vertafore.com/as/token.oauth2");
            // request.Headers.Add("Cookie", "PF=tKaGNBNucDUKg1FlGOdOIx;");
            var collection = new List<KeyValuePair<string, string>>
            {
                new("grant_type", "client_credentials"),
                new("client_id", _decryptedClientId),
                new("client_secret", _decryptedClientSecret)
            };

            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = authClient.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            string tokenResponseString = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine("" + tokenResponseString);



            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 10, 0);
            TokenObject tokenResponse = JsonSerializer.Deserialize<TokenObject>(tokenResponseString) ?? new TokenObject();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenResponse.JWT);
            // _token = tokenResponse.JWT;
            // Console.WriteLine(tokenResponse.JWT);
            return client;
        }

        public string GetStringResult(string reqUrl)
        {
            if ((DateTime.Now - _startTime).TotalSeconds > 3500)
            {
                _startTime = DateTime.Now;
                _client = GetAuthenticatedClient();
            }

            var request = _client.GetAsync(new Uri(reqUrl));
            var response = request.Result;
            string contents = response.Content.ReadAsStringAsync().Result;

            return contents;
        }

        public string GetStringFromPost(string reqUrl, string payload)
        {
            StringContent content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = _client.PostAsync(reqUrl, content).Result;
            //Console.WriteLine(response.ToString());
            string contents = response.Content.ReadAsStringAsync().Result;

            return contents;
        }

    }
}
