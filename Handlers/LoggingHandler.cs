
using System.Text;
using AMSDataLoad.Models;
using System.Text.Json;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using MimeKit;
using Google.Apis.Gmail.v1.Data;
using System.Collections;

namespace AMSDataLoad.Handlers
{
    public class LoggingHandler
    {
        private readonly string _rootFolder;
        private string _storedError = "";
        public LoggingHandler(string rootFolder)
        {
            _rootFolder = rootFolder;
        }
        private int errorCount = 0;
        private string[] criticalErrorSubstrings = {"Critical error thrown",
        "Could not find stored procedure",
        "Must declare the scalar variable",
        "There are fewer columns in the INSERT statement than values specified in the VALUES clause.",
        "String or binary data would be truncated."};
        public String emailErrorMessage = "";
        public void WriteLog(String logText, bool failure = false)
        {   //systems sometimes don't support relative paths for the log folder
            //because the log folder is a necessary step to run the program
            //you'll have to declare the path from the root directory to the log dir in order for it to work.

            // string rootFolder = "logs/"; 
            // string rootFolder = "C:/Install/EpicorLoader/logs/"; //"logs/"; "C:/Install/EpicorLoader/logs/";
            string dateStamp = DateTime.Now.ToString("yyyy-MM-dd");

            Console.WriteLine(logText);
            _storedError += logText;

            if (!System.IO.Directory.Exists(_rootFolder))
            {
                System.IO.Directory.CreateDirectory("logs");
            }

            string logLocation = _rootFolder + dateStamp + ".txt";
            if (!System.IO.File.Exists(logLocation))
            {
                System.IO.File.WriteAllText(logLocation, logText);
            }
            else
            {
                using StreamWriter openFile = new(logLocation, append: true);

                openFile.WriteLine(logText + "\n");

                openFile.Close();
            }

            if (failure)
            {

            }
        }

        public void WriteLogForModel(string logText, string model, bool failure = false)
        {
            Console.WriteLine(logText);
            string dateStamp = DateTime.Now.ToString("yyyy-MM-dd");

            if (!System.IO.Directory.Exists(_rootFolder))
            {
                System.IO.Directory.CreateDirectory("logs");
            }

            string logLocation = _rootFolder + model + dateStamp + ".txt";
            if (!System.IO.File.Exists(logLocation))
            {
                System.IO.File.WriteAllText(logLocation, logText);
            }
            else
            {
                using StreamWriter openFile = new(logLocation, append: true);

                openFile.WriteLine(logText + "\n");

                openFile.Close();
            }

            if (failure)
            {

            }
        }

        public void SendEmail(string sendTo, string subject, string body, bool dev = false)
        {
            string fromEmail = "support@abeerconsulting.com";
            DateTime emailSendTime = DateTime.Now;
            subject += " @" + emailSendTime.ToString();

            string serviceAccountKeyFilePath = "C:/inetpub/AgencyZoom/service-account-key.json"; //"Assets/service-account-key.json"; "C:/Install/EpicorLoader/service-account-key.json";
            if (dev){
                serviceAccountKeyFilePath = "Assets/service-account-key.json";
            }
            string serviceJSON = File.ReadAllText(serviceAccountKeyFilePath);

            var credential = GoogleCredential.FromJson(serviceJSON)
                .CreateScoped(new[] { GmailService.Scope.GmailSend })
                .CreateWithUser(fromEmail);

            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "EmailClient",
            });

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Abeer Support", fromEmail));
            emailMessage.To.Add(new MailboxAddress(sendTo, sendTo));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain")
            {
                Text = body
            };

            var message = new Message
            {
                Raw = Base64UrlEncode(emailMessage.ToString())
            };

            //emailMessage.Attachments.Append(new Attachment("first_file/path", "second_file/path"));
            //emailMessage.Attachments.Append(new Attachment("first_file/path"));

            this.WriteLog("Sending email :" + subject);
            try
            {
                service.Users.Messages.Send(message, "me").Execute();
                this.WriteLog("Email Sent Succesfully :" + subject);
            }
            catch (Exception e)
            {
                this.WriteLog("Failed to send email :" + subject);
            }

        }

        static string Base64UrlEncode(string input)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input))
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }
        public void WriteJSONExample(string logText, string model, bool failure = false)
        {
            string rootFolder = "JSONExamples/";

            string logLocation = rootFolder + model + ".json";
            System.IO.File.WriteAllText(logLocation, logText);
        }

        /// <summary>
        /// dumps exception to the log. Attempts to log the error message, stack trace, inner exception, base exception, and any more information that's available
        /// </summary>
        /// <param name="ex"> the exception being logged</param>
        public void logException(Exception ex, string model = "NoModel")
        {
            Func<string, string> summaryHeader = (text) =>
            {
                string bar = "----------";
                return bar + text + bar + "\n";
            };
            this.WriteLogForModel(summaryHeader("Error #" + this.errorCount.ToString()), model);
            this.WriteLogForModel(summaryHeader(DateTime.Now.ToString()), model);
            this.errorCount++;
            this.WriteLogForModel(summaryHeader("Begin Dump of Error"), model);

            this.WriteLogForModel(this.dumpException(ex, "Top Level Exception"), model);
            if (ex.InnerException != null)
            {
                this.WriteLogForModel(this.dumpException(ex.InnerException, "Inner Exception"), model);
            }
            if (ex.GetBaseException() != null)
            {
                this.WriteLogForModel(this.dumpException(ex.GetBaseException(), "Base Exception"), model);
            }

            this.WriteLogForModel(summaryHeader("End Dump of Error"), model);
            foreach (string substring in this.criticalErrorSubstrings)
            {
                if (ex.Message.Contains(substring))
                {
                    emailErrorMessage = ex.Message + "\n" + "The last error was a critical error, the program was ended";
                    this.WriteLogForModel(summaryHeader("The last error was a critical error, the program was ended and an email was sent @" + DateTime.Now.ToString()), model);
                    this.SendEmail("dtripodi@abeerconsulting.com", "Error on AgencyZoom Loader", this.emailErrorMessage);
                    throw new Exception("Critical error thrown");
                }
            }
        }

        /// <summary>
        /// dumps exception to the string. Attempts to get the string values of a number of common exception properties
        /// </summary>
        /// <param name="ex"> the exception being dumped</param>
        /// <param name="exceptionName"> name for exception, used in dumped string</param>
        /// <returns>
        /// string of concatenated dumped exception values
        /// </returns>
        public string dumpException(Exception ex, string exceptionName)
        {
            string emailMessage = "An error occured on the loader\nError message follows:\n\n";
            string emailSubject = "Error on AgencyZoom Loader";
            string dumpedException = "-----" + exceptionName + ":\n";
            Func<string, string, string> section = (propertyName, text) =>
            {
                return propertyName + ":\n" + text + "\n\n";
            };

            if (ex.Source != null)
            {
                dumpedException += section("Source", ex.Source);
            }
            if (ex.Message != null)
            {
                dumpedException += section("Message", ex.Message);
                emailMessage += ex.Message;

                if (this.emailErrorMessage.Length + ex.Message.Length > 1000)
                {
                    this.SendEmail("dtripodi@abeerconsulting.com", emailSubject, this.emailErrorMessage);
                    this.emailErrorMessage = ex.Message;
                }
                else
                {
                    this.emailErrorMessage += "\n" + ex.Message;
                }

            }
            if (ex.StackTrace != null)
            {
                dumpedException += section("Stack Trace", ex.StackTrace);
            }
            if (ex.Data != null)
            {
                foreach (DictionaryEntry errorExtraData in ex.Data)
                {
                    dumpedException += section(errorExtraData.Key.ToString() + "", errorExtraData.Value?.ToString() + "");
                }
            }
            if (ex.HResult > 0)
            {
                dumpedException += section("HResult", ex.HResult.ToString());
            }
            if (ex.HelpLink != null)
            {
                dumpedException += section("Help Link", ex.HelpLink);
            }


            return dumpedException + "\n\n";
        }

        public string LogServerError(string serverResponse, string emailBody, string activeStep, string errorPage)
        {
            // EpicorRESTAPIErrorMessage errorMessage = new EpicorRESTAPIErrorMessage();

            try
            {
                WriteLog("On Step " + activeStep + ": \n" + "Recieved error response from server", true);
                WriteLog("\n" + "Server Response as Follows:", true);
                WriteLog("\n" + serverResponse + "\n\n\n", true);
                return emailBody += "On Step " + activeStep + ", On Page " + errorPage + ": \n" + serverResponse + " \n\n\n";
            }
            catch (Exception e)
            {
                Console.WriteLine("On Step " + activeStep + ": \n" + e.Message, true);
                Console.WriteLine("\n" + "Server Response as Follows:", true);
                Console.WriteLine("\n" + serverResponse + "\n\n\n", true);
                Console.WriteLine(e.StackTrace ?? "\n\n", true);
                return emailBody += "On Step " + activeStep + ", On Page " + errorPage + ": \n" + serverResponse + ": \n";
            }
            // return emailBody;

        }

    }
}
