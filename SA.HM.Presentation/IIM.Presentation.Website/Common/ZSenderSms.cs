using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace HotelManagement.Presentation.Website.Common
{
    public class ZSenderSms
    {
        public bool SendSingleSms(APIRequestForZsender requestData)
        {
            bool status = false;
            //HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            //HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            //commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendSMS", "SendSMSConfiguration");
            //string mainString = commonSetupBO.Description;
            var mobileNumber = string.Empty;
            if (!requestData.msisdn.StartsWith("88")){
                mobileNumber = "88" +requestData.msisdn;
            }
            else
            {
                mobileNumber = requestData.msisdn;
            }
            string smsURL = "https://api.zsender.com/v1/sms.php?api_id="+requestData.api_id+"&key="+requestData.api_key+"&msisdn="+mobileNumber+"&message="+requestData.sms+"&sender_id="+requestData.sender_id+"&trx_id="+requestData.trx_id;

            this.HttpGet(smsURL);
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(smsURL);
            //request.ContentType = /*"application/x-www-form-urlencoded";*/ "application/json";
            //request.Method = "POST";
            //APIResponse apiResponse = null;
            //var settings = new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Ignore,
            //    MissingMemberHandling = MissingMemberHandling.Ignore
            //};

            

            //string[] dataArray = mainString.Split('~');

            //using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            //{
            //    /*APIRequest requestData = new APIRequest
            //    {
            //        api_token = dataArray[0],
            //        sid = dataArray[1],
            //        csms_id = dataArray[2],
            //        msisdn = mobileNumber,
            //        sms = messageBody
            //    };*/

            //    string json = JsonConvert.SerializeObject(requestData, settings);

            //    streamWriter.Write(json);

            //}

            //var httpResponse = (HttpWebResponse)request.GetResponse();
            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //    var result = streamReader.ReadToEnd();
            //    apiResponse = JsonConvert.DeserializeObject<APIResponse>(result, settings);
            //    if (apiResponse.status == "success")
            //    {
            //        status = true;
            //    }
            //}
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

        //public bool SendBulkSms(APIRequestForBulk apiRequest)
        //{
        //    bool status = false;
        //    string smsURL = "https://smsplus.sslwireless.com/api/v3/send-sms/bulk";
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(smsURL);
        //    request.ContentType = "application/json";
        //    request.Method = "POST";

        //    APIResponse apiResponse = null;

        //    var settings = new JsonSerializerSettings
        //    {
        //        NullValueHandling = NullValueHandling.Ignore,
        //        MissingMemberHandling = MissingMemberHandling.Ignore
        //    };

        //    //string[] dataArray = mainString.Split('~');
        //    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        //    {
        //        /*APIRequestForBulk apiRequest = new APIRequestForBulk()
        //        {
        //            api_token = dataArray[0],
        //            sid = dataArray[1],
        //            msisdn = mobileNumberList,
        //            sms = messageBody,
        //            batch_csms_id = dataArray[2]
        //        };*/

        //        string json = JsonConvert.SerializeObject(apiRequest, settings);

        //        streamWriter.Write(json);

        //    }

        //var httpResponse = (HttpWebResponse)request.GetResponse();
        //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //{
        //    var result = streamReader.ReadToEnd();
        //    apiResponse = JsonConvert.DeserializeObject<APIResponse>(result, settings);
        //    if (apiResponse.status == "success")
        //    {
        //        status = true;
        //    }
        //}
        //return status;
        //}
    }

    public class APIRequestForZsender
    {
        public string api_key { get; set; }
        public string api_id { get; set; }
        public string trx_id { get; set; }
        public string msisdn { get; set; }
        public string sms { get; set; }
        public string sender_id { get; set; }
    }

    //public class Smsinfo
    //{
    //    public string sms_status { get; set; }
    //    public string status_message { get; set; }
    //    public string msisdn { get; set; }
    //    public string sms_type { get; set; }
    //    public string sms_body { get; set; }
    //    public string csms_id { get; set; }
    //    public string reference_id { get; set; }
    //}

    //public class APIResponse
    //{
    //    public string status { get; set; }
    //    public int status_code { get; set; }
    //    public string error_message { get; set; }
    //    public List<Smsinfo> smsinfo { get; set; }
    //}

    //public class APIRequestForBulk
    //{
    //    public string api_token { get; set; }
    //    public string sid { get; set; }
    //    public List<string> msisdn { get; set; }
    //    public string sms { get; set; }
    //    public string batch_csms_id { get; set; }
    //}
}
