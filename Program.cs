
using System.Text.Json;
using System.Text.Json.Serialization;
using AMSDataLoad.Handlers;
using AMSDataLoad.Helpers;
using AMSDataLoad.Models;
using Microsoft.Extensions.Configuration;

// string filePath = @"test.json";
// string fileContents = File.ReadAllText(filePath);
// APIResponse<AMSCustomer>? test = JsonSerializer.Deserialize<APIResponse<AMSCustomer>>(fileContents);




DateTime startTime = DateTime.Now;
bool running = true;

string activeStep = "";

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appSettings.json")
    .Build();

bool devRun = config.GetSection("IsDev").Get<bool>();
string rootFolder = "C:/Users/BreadFTP/Desktop/AMSDataLoad/logs/";
if (devRun)
{
    rootFolder = "./logs/";
}
LoggingHandler logging = new LoggingHandler(rootFolder);



Helper helper = new Helper();

string encryptedClientId = config.GetSection("EncryptedClientId").Get<string>() + "";
string encryptedClientSecret = config.GetSection("EncryptedSecret").Get<string>() + "";

string encryptionPW = "AMS123!@#";
string decryptedClientId = helper.DecryptString(encryptionPW, encryptedClientId);
string decryptedClientSecret = helper.DecryptString(encryptionPW, encryptedClientSecret);

// Console.WriteLine(decryptedClientId);
// Console.WriteLine(decryptedClientSecret);

logging.WriteLog("Starting Prep");
logging.WriteLog((DateTime.Now - startTime).TotalSeconds.ToString());

string lookBackDaysString = config.GetSection("LookBackDays").Get<string>() + "";
int lookBackDays = Int32.Parse(lookBackDaysString != null ? lookBackDaysString : "7");
DateTime filterDate = DateTime.Today.AddDays(-lookBackDays);
string filterDateString = filterDate.ToString();

string lastResponse = "";
// int lastPage = 0;

Dictionary<string, bool> modelsToRun = config.GetSection("ModelsToRun").Get<Dictionary<string, bool>>() ?? new Dictionary<string, bool>();



// HttpClient authClient = new HttpClient();
string rootRequestUrl = "https://api.reporting.apps.vertafore.com/consumer/v1/ams360/table/";

RootReqHandler rootReqHandler = new RootReqHandler(
    logging, config, startTime, decryptedClientId, decryptedClientSecret
);

List<Task<string>> taskList = new List<Task<string>>();


// if (tablesToLoad.Contains("ECOOpr"))
if (modelsToRun["Customer"])
{
    AMSCustomerHandler dataHandlerHeader = new AMSCustomerHandler(logging, config, rootReqHandler, startTime);
    taskList.Add(dataHandlerHeader.GetAndInsertCustomer(rootRequestUrl, filterDateString));
}


string[] results = await Task.WhenAll(taskList);
taskList = new List<Task<string>>();

foreach (string result in results)
{
    Console.WriteLine(result);
}

Console.WriteLine("Complete");
