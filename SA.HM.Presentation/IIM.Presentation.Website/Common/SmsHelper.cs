using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HotelManagement.Presentation.Website.Common
{
    public class SmsHelper
    {
        public static bool SendSmsSingle(SmsView sms, Dictionary<string, string> tokens, string smsGetway, string mobileNumber)
        {
            bool status = false;
            var Body = SmsHelper.GetSmsBody(sms.TempleteName, tokens);
            try
            {
                if(smsGetway  == "SSLCommerce")
                {
                    HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                    commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendSMS", "SendSMSConfiguration");
                    string mainString = commonSetupBO.Description;
                    string[] dataArray = mainString.Split('~');
                    APIRequest requestData = new APIRequest
                    {
                        api_token = dataArray[1],
                        sid = dataArray[2],
                        csms_id = dataArray[3],
                        msisdn = mobileNumber,
                        sms = Body
                    };
                    SSLCommerceSmsSender sSLCommerceSmsSender = new SSLCommerceSmsSender();
                   status = sSLCommerceSmsSender.SendSingleSms(requestData);
                }

                if (smsGetway == "ZARSS")
                {
                    HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                    commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendSMS", "SendSMSConfiguration");
                    string mainString = commonSetupBO.Description;
                    string[] dataArray = mainString.Split('~');
                    APIRequestForZsender requestData = new APIRequestForZsender
                    {
                        api_id = dataArray[1],
                        api_key = dataArray[2],
                        trx_id = dataArray[3],
                        msisdn = mobileNumber,
                        sms = Body,
                        sender_id = "ZARSS"
                    };
                    ZSenderSms zsender = new ZSenderSms();
                    status = zsender.SendSingleSms(requestData);
                }
                return status;
            }
            catch (Exception ex)
            {
                string root = @"C:\Temp";
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                string strPath = root  + "\\some.txt";
                if (!File.Exists(strPath))
                {
                    File.Create(strPath).Dispose();
                }
                using (StreamWriter sw = File.AppendText(strPath))
                {
                    sw.WriteLine("=============Error Logging ===========");
                    sw.WriteLine("===========Start============= " + DateTime.Now);
                    sw.WriteLine("Error Message: " + ex.Message);
                    sw.WriteLine("Stack Trace: " + ex.StackTrace);
                    sw.WriteLine("===========End============= " + DateTime.Now);

                }

                //string readText = File.ReadAllText(@"C:\Temp\csc.txt");
                //Console.WriteLine(readText);
                //return false;

                using (StreamReader sr = new StreamReader(strPath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }

                return status;
            }
            finally
            {

            }

        }

        public static bool SendSmsBulk(SmsView sms, Dictionary<string, string> tokens, string smsGetway, List<string> mobileNumber)
        {
            bool status = false;
            var Body = SmsHelper.GetSmsBody(sms.TempleteName, tokens);
            try
            {
                if (smsGetway == "SSLCommerce")
                {
                    HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                    commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendSMS", "SendSMSConfiguration");
                    string mainString = commonSetupBO.Description;
                    string[] dataArray = mainString.Split('~');
                    APIRequestForBulk apiRequest = new APIRequestForBulk()
                    {
                        api_token = dataArray[1],
                        sid = dataArray[2],
                        msisdn = mobileNumber,
                        sms = Body,
                        batch_csms_id = dataArray[3]
                    };
                    SSLCommerceSmsSender sSLCommerceSmsSender = new SSLCommerceSmsSender();
                    status =  sSLCommerceSmsSender.SendBulkSms(apiRequest);
                }
                return status;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {

            }

        }
        /// <summary>
        /// Get Mail Body
        /// </summary>
        /// <param name="templateName">mail template name</param>
        /// <param name="tokens">Parameters for Email Templete</param>
        /// <returns></returns>

        //---Sms Dynamic body--//
        public static string GetSmsBody(string templateName, Dictionary<string, string> tokens)
        {
            string text = System.IO.File.ReadAllText(HttpContext.Current.Request.MapPath(string.Format("~/SMSTemplates/{0}", templateName)));
            return tokens.Aggregate(text, (current, token) => current.Replace(string.Format("##{0}##", token.Key), token.Value));
        }
    }
}