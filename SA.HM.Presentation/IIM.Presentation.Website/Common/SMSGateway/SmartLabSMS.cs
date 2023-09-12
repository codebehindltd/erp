using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace HotelManagement.Presentation.Website.Common
{
    public class SmartLabSMS
    {
        public bool SendSingleSms(APIRequestForSmartLabSMS requestData)
        {
            bool status = false;
            var mobileNumber = string.Empty;
            if (!requestData.msisdn.StartsWith("88")){
                mobileNumber = "88" +requestData.msisdn;
            }
            else
            {
                mobileNumber = requestData.msisdn;
            }
            
            string smsURL = "https://labapi.smartlabsms.com/smsapiv3?apikey=" + requestData.api_key + "&sender=" + requestData.sender_id + "&msisdn=" + mobileNumber + "&smstext=" + requestData.sms;

            this.HttpGet(smsURL);
            return status;
        }

        public string HttpGet(string uri)
        {
            string content = null;

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader sr = new StreamReader(stream))
            {
                content = sr.ReadToEnd();
            }

            return content;
        }
    }

    public class APIRequestForSmartLabSMS
    {
        public string api_key { get; set; }
        public string msisdn { get; set; }
        public string sms { get; set; }
        public string sender_id { get; set; }
    }
}
