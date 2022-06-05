using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class TestSMSPushAPI : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            //Procedure one
            //String sid = "STAKEHOLDER";
            //String user = "USER NAME";
            //String pass = "PASSWORD";
            //string sms_url = "http://sms.sslwireless.com/pushapi/dynamic/server.php?";
            //String myParameters = "user=" + user + "&pass=" + pass + "&msisdn=" + "880XXXXXXXXXX" + "&sms=" + System.Web.HttpUtility.UrlEncode("Test SMS1\nTest sms2\nTest sms API3") + "&csmsid=" + "1234567890" + "&sid=" + sid;
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sms_url + myParameters);
            //request.AutomaticDecompression = DecompressionMethods.GZip;
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //Stream stream = response.GetResponseStream();
            //StreamReader reader = new StreamReader(stream);
            //response = reader.ReadToEnd();
           
            //return response;
        }

        

        public string convertBanglatoUnicode(string banglaText)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in banglaText)
            {
                sb.AppendFormat("{1:x4}", c, (int)c);
            }
            string unicode = sb.ToString().ToUpper();

            return unicode;
        }

        protected void btnSendMail_Click(object sender, EventArgs e)
        {
            bool status = false;

            SMS sMS = new SMS();
            sMS.sid = "ezzygroup";
            sMS.user = "EzzyGroup";
            sMS.pass = "90<4V22c";
            sMS.URI = "http://sms.sslwireless.com/pushapi/dynamic/server.php";

            String cell1 = "01677400731";
            String cell2 = "8801521441219";
            String text1 = "Test SMS 1 from DGL.বল কেমন আছো";
            String text2 = "Test SMS 2 from DGL";
            String clientRefId = "1234567890";

            //bool isAscii = false;
            //foreach (var item in text1)
            //{
            //    isAscii = item < 128;
            //    if (isAscii)
            //    {
            //        return;
            //    }
            //}
            //if (isAscii)
            //{
            //    text1 = convertBanglatoUnicode(text1);
            //}
            
            string myParameters = "user=" + sMS.user + "&pass=" + sMS.pass + "&sms[0][0]=" + cell1 + "&sms[0][1]=" + text1 + "&sms[0][2]=" + clientRefId + "&sms[1][0]=" + cell2 + "&SMS[1][1]=" + text2 + "&sms[1][2]=" + clientRefId + "&sid=" + sMS.sid;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sMS.URI);
            byte[] data = Encoding.ASCII.GetBytes(myParameters);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            String responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Response.Write(responseString);
            XmlDocument xmltest = new XmlDocument();
            xmltest.LoadXml(responseString);

            XmlNodeList elemlist = xmltest.GetElementsByTagName("PERMITTED");

            string result = elemlist[0].InnerXml;
            if (result == "OK")
            {
                status = true;
            }
            else
            {
                status = false;
            }
            if (status)
            {
                CommonHelper.AlertInfo(innboardMessage, "SMS Send Successful", AlertType.Success);
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "SMS Send Failed", AlertType.Warning);
            }
        }
    }

}