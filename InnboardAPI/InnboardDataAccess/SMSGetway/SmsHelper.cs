using InnboardDataAccess.DataAccesses;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace InnboardDataAccess.SMSGetway
{
    public class SmsHelper
    {
        public static bool SendSmsSingle(SMSView sms, Dictionary<string, string> tokens, string smsGetway, string mobileNumber, string commonSetupBODescription)
        //public static bool SendSmsSingle(string smsGetway, string mobileNumber)
        {
            bool status = false;
            var Body = SmsHelper.GetSmsBody(sms.TempleteName, tokens);
            //var Body = "Test SMS Body";
            try
            {
                //if (smsGetway == "SSLCommerce")
                //{
                //    HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                //    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                //    commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendSMS", "SendSMSConfiguration");
                //    string mainString = commonSetupBO.Description;
                //    string[] dataArray = mainString.Split('~');
                //    APIRequest requestData = new APIRequest
                //    {
                //        api_token = dataArray[1],
                //        sid = dataArray[2],
                //        csms_id = dataArray[3],
                //        msisdn = mobileNumber,
                //        sms = Body
                //    };
                //    SSLCommerceSmsSender sSLCommerceSmsSender = new SSLCommerceSmsSender();
                //    status = sSLCommerceSmsSender.SendSingleSms(requestData);
                //}

                //if (smsGetway == "ZARSS")
                //{
                //    HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                //    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                //    commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendSMS", "SendSMSConfiguration");
                //    string mainString = commonSetupBO.Description;
                //    string[] dataArray = mainString.Split('~');
                //    APIRequestForZsender requestData = new APIRequestForZsender
                //    {
                //        api_id = dataArray[1],
                //        api_key = dataArray[2],
                //        trx_id = dataArray[3],
                //        msisdn = mobileNumber,
                //        sms = Body,
                //        sender_id = "ZARSS"
                //    };
                //    ZSenderSms zsender = new ZSenderSms();
                //    status = zsender.SendSingleSms(requestData);
                //}

                if (smsGetway == "SmartLabSMS")
                {
                    
                    //HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                    //HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                    //commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendSMS", "SendSMSConfiguration");
                    //string mainString = commonSetupBO.Description;
                    string[] dataArray = commonSetupBODescription.Split('~');
                    APIRequestForSmartLabSMS requestData = new APIRequestForSmartLabSMS
                    {
                        api_key = dataArray[1],
                        sender_id = dataArray[2],
                        msisdn = mobileNumber,
                        sms = Body



                        //api_key = "9e5906e7a56c9a2ba6c1bc230c2de97c",
                        //sender_id = "DhaliResort",
                        //msisdn = mobileNumber,
                        //sms = Body

                    };

                    SmartLabSMS smartLabSMSsender = new SmartLabSMS();
                    status = smartLabSMSsender.SendSingleSms(requestData);
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
                string strPath = root + "\\some.txt";
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
