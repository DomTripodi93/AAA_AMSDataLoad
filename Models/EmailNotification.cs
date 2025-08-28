using System;

namespace AMSDataLoad.Models
{
    public partial class EmailNotification
    {
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string RequestType { get; set; }
    }
}
