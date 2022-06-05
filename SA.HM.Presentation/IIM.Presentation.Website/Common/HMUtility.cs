using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.Payroll;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using System.Globalization;
using System.Reflection;
using System.ComponentModel;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using Microsoft.Reporting.WebForms;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity;

namespace HotelManagement.Presentation.Website.Common
{
    public class HMUtility
    {
        public string[] formatListM = new string[] { "M/d/yyyy", "MM/dd/yyyy", "MM-dd-yyyy", "M-d-yyyy", "MMM dd yyyy", "MM dd yyyy" };
        public string[] formatListD = new string[] { "d/M/yyyy", "dd/MM/yyyy", "dd-MM-yyyy", "d-M-yyyy", "dd MMM yyyy", "dd MM yyyy" };
        
        public HMUtility()
        {
        }
        
        private string[] GetFormatList()
        {
            string format = GetFormat(true);
            string Sub = format.Substring(0, 1).ToLower();
            if (Sub == "m")
            {
                return formatListM;
            }
            else
            {
                return formatListD;
            }
        }
        public string GetFormat(bool isServer)
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            UserInformationBO userInformationBO = new UserInformationBO();
            if (string.IsNullOrWhiteSpace(userInformationBO.ServerDateFormat))
            {
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InnboardCalender", "InnboardCalenderFormat");

                string[] formats = Regex.Split(commonSetupBO.SetupValue, "~");
                userInformationBO.ServerDateFormat = formats[0];
                userInformationBO.ClientDateFormat = formats[1];
            }
            if (isServer)
                commonSetupBO.SetupValue = userInformationBO.ServerDateFormat;
            else
                commonSetupBO.SetupValue = userInformationBO.ClientDateFormat;
            return commonSetupBO.SetupValue;
        }

        public string GetTimeFormat()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InnboardCalender", "InnboardTimeFormat");
            return commonSetupBO.SetupValue;
        }

        public DateTime GetDateTimeFromString(string Date, string format)
        {
            DateTime DateTime;
            DateTime.TryParseExact(Date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime);
            return DateTime;
        }
        public string GetUnivarsalDateTimeFromString(string Date, string format)
        {
            DateTime DateTime;
            DateTime.TryParseExact(Date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime);
            return DateTime.ToString("yyyy-MM-dd");
        }
        public DateTime SuprimaIntToDateTime(int intDate)
        {
            // According to Suprima Logic
            return new DateTime(1970, 1, 1).AddSeconds(intDate);
        }
        public string GetStringFromDateTime(DateTime dateTime)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = GetCurrentApplicationUserInfo();
            return dateTime.ToString(GetFormat(true));
        }

        public string GetDateTimeStringFromDateTime(DateTime dateTime)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = GetCurrentApplicationUserInfo();
            return dateTime.ToString(userInformationBO.ServerDateFormat + " " + userInformationBO.TimeFormat);
        }

        public string GetTimeFromDateTime(DateTime dateTime)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = GetCurrentApplicationUserInfo();
            return dateTime.ToString(userInformationBO.TimeFormat);
        }

        public string GetHourFromDateTime(DateTime dateTime)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = GetCurrentApplicationUserInfo();

            string hourFormat = "HH"; //Default Hour Format consider if not find any configuration
            string[] timeFormat = userInformationBO.TimeFormat.Split(':');
            if (timeFormat.Count() > 0)
            {
                hourFormat = timeFormat[0];
            }

            return dateTime.ToString(hourFormat);
        }

        public string GetFromDate()
        {
            string fromDate = string.Empty;
            fromDate = "01/01/2015";
            return fromDate;
        }
        public string GetToDate()
        {
            string toDate = string.Empty;
            toDate = "01/01/5000";
            return toDate;
        }
        public bool IsInnboardValidMail(string Email)
        {
            bool status = true;
            string expression = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            Match match = Regex.Match(Email, expression, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return status;
        }
        public string GetDropDownNoneValue()
        {
            string value = "--- None ---";
            return value;
        }
        public string GetDropDownFirstValue()
        {
            string value = "--- Please Select ---";
            return value;
        }
        public string GetDropDownFirstAllValue()
        {
            string value = "--- ALL ---";
            return value;
        }
        public UserInformationBO GetCurrentApplicationUserInfo()
        {
            if (System.Web.HttpContext.Current.Session["UserInformationBOSession"] == null)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Login.aspx");
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            return userInformationBO;
        }

        public UserInformationBO GetCurrentApplicationUserInfoForRestaurant()
        {
            if (System.Web.HttpContext.Current.Session["UserInformationBOSession"] == null)
            {
                System.Web.HttpContext.Current.Response.Redirect("/POS/Login.aspx");
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            return userInformationBO;
        }
        public ObjectPermissionBO ObjectPermission(int userId, string formName)
        {
            ObjectPermissionDA objectPermissionDA = new ObjectPermissionDA();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = objectPermissionDA.GetFormPermissionByUserIdNForm(userId, formName);
            if (objectPermissionBO.ObjectPermissionId > 0)
            {
                if (!objectPermissionBO.IsViewPermission)
                {
                    System.Web.HttpContext.Current.Response.Redirect("/HMCommon/frmHMHome.aspx");
                }

                return objectPermissionBO;
            }
            else
            {
                System.Web.HttpContext.Current.Response.Redirect("/HMCommon/frmHMHome.aspx");
            }

            return objectPermissionBO;
        }        
        public ObjectPermissionBO ObjectPermissionForBearer(int empId, string formName)
        {
            ObjectPermissionDA objectPermissionDA = new ObjectPermissionDA();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = objectPermissionDA.GetFormPermissionForBearerByForm(formName);
            if (objectPermissionBO.ObjectPermissionId > 0)
            {
                if (!objectPermissionBO.IsViewPermission)
                {
                    System.Web.HttpContext.Current.Response.Redirect("/POS/Login.aspx");
                }

                return objectPermissionBO;
            }
            else
            {
                System.Web.HttpContext.Current.Response.Redirect("/POS/Login.aspx");
            }

            return objectPermissionBO;
        }
        public bool IsNumber(String str)
        {
            try
            {
                Double.Parse(str);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public string sqlInjectionFilter(string Value, Boolean isSQLInjection)
        {
            if (Value != null)
            {
                //Level 0
                if (isSQLInjection)
                {
                    Value = Value.Replace("--", "");
                    Value = Value.Replace("[", "[[]");
                    Value = Value.Replace("%", "[%]");
                    Value = Value.Replace(" OR ", "");
                    Value = Value.Replace(" AND ", "");

                    //Level 1
                    string[] myArray = new string[] { "xp_ ", "update ", "insert ", "select ", "drop ", "alter ", "create ", "rename ", "delete ", "replace " };
                    int i = 0;
                    int i2 = 0;
                    int intLenghtLeft = 0;
                    for (i = 0; i < myArray.Length; i++)
                    {
                        string strWord = myArray[i];
                        Regex rx = new Regex(strWord, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        MatchCollection matches = rx.Matches(Value);
                        i2 = 0;
                        foreach (Match match in matches)
                        {
                            GroupCollection groups = match.Groups;
                            intLenghtLeft = groups[0].Index + myArray[i].Length + i2;
                            Value = Value.Substring(0, intLenghtLeft - 1) + "&nbsp;" + Value.Substring(Value.Length - (Value.Length - intLenghtLeft), Value.Length - intLenghtLeft);
                            i2 += 5;
                        }
                    }
                }
                else
                {
                    Value = Value.Replace("%27", "'"); // Most important one! This line alone can prevent most injection attacks
                }

                return Value;
            }
            else
            {
                return Value;
            }
        }
        public bool GetSingleProjectAndCompany()
        {
            bool isSingle = true;
            GLProjectDA entityDA = new GLProjectDA();
            List<GLProjectBO> projectListBO = entityDA.GetAllGLProjectInfo();
            if (projectListBO != null)
            {
                if (projectListBO.Count > 1)
                {
                    isSingle = false;
                }
            }
            return isSingle;

        }
        public bool GetIsIntegratedGeneralLedger()
        {
            bool isIntegrated = true;
            int integratedGeneralLedger = Int32.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["IntegratedGeneralLedger"].ToString());
            if (integratedGeneralLedger == 1)
            {
                isIntegrated = true;
            }
            else
            {
                isIntegrated = false;
            }
            return isIntegrated;
        }
        private bool CheckForValidDomain(String DomainName)
        {
            bool status = true;
            string url = "http://www." + DomainName.Trim();

            WebResponse response = null;
            string data = string.Empty;

            try
            {
                WebRequest request = WebRequest.Create(url);
                response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    data = reader.ReadToEnd();
                }

                // domain exists, this is valid domain
                status = true;
            }

            catch (WebException ee)
            {
                // return false, most likely this domain doesn't exists
                status = false;
            }

            catch (Exception ee)
            {
                // Some error occured, the domain might exists
                status = false;
            }

            finally
            {
                if (response != null) response.Close();
            }

            return status;
        }
        public bool CreateActivityLogEntity(string ActivityType, string EntityType, long EntityId, string Module, string Remarks)
        {
            UserInformationBO userBO = new UserInformationBO();
            userBO = GetCurrentApplicationUserInfo();
            ActivityLogsDA logDA = new ActivityLogsDA();
            ActivityLogsBO activityLog = new ActivityLogsBO();
            activityLog.ActivityType = ActivityType;
            activityLog.EntityType = EntityType;
            activityLog.EntityId = EntityId;
            activityLog.Remarks = Remarks;
            activityLog.Module = Module;
            activityLog.CreatedBy = userBO.UserInfoId;
            bool status = logDA.SaveActivityLogInformation(activityLog);
            return status;
        }
        public bool CreateActivityLogEntity(string ActivityType, string EntityType, long EntityId, string Module, string Remarks, out long activityId)
        {
            UserInformationBO userBO = new UserInformationBO();
            userBO = GetCurrentApplicationUserInfo();
            ActivityLogsDA logDA = new ActivityLogsDA();
            ActivityLogsBO activityLog = new ActivityLogsBO();
            activityLog.ActivityType = ActivityType;
            activityLog.EntityType = EntityType;
            activityLog.EntityId = EntityId;
            activityLog.Remarks = Remarks;
            activityLog.Module = Module;
            activityLog.CreatedBy = userBO.UserInfoId;
            bool status = logDA.SaveActivityLogInformation(activityLog, out activityId);
            return status;
        }
        public bool CreateActivityLogDetails(int activityId, string fieldName, string currentData, string previousData, string remarks, out long Id)
        {
            ActivityLogsDA logDA = new ActivityLogsDA();
            ActivityLogDetailsBO activityLog = new ActivityLogDetailsBO();
            activityLog.ActivityId = activityId;
            activityLog.FieldName = fieldName;
            activityLog.CurrentData = currentData;
            activityLog.PreviousData = previousData;
            activityLog.DetailDescription = remarks;

            bool status = logDA.SaveActivityLogDetails(activityLog, out Id);
            return status;
        }
        public string GetHMCompanyProfile()
        {
            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
            return files[0].CompanyName;
        }
        public string GetHMCompanyAddress()
        {
            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
            return files[0].CompanyAddress + "\n" + files[0].WebAddress;
        }
        public string GetPrintDate()
        {
            DateTime Today = DateTime.Now;
            return "Print Date : " + GetStringFromDateTime(Today);
        }
        public String GetFromDateAndToDate(DateTime FromDate, DateTime ToDate)
        {
            return " Date From : " + GetStringFromDateTime(FromDate) + "  Date To : " + GetStringFromDateTime(ToDate);
        }
        public String GetSearchDate(DateTime FromDate)
        {
            return " Search Date : " + GetStringFromDateTime(FromDate);
        }
        public List<string> GetReportYearList()
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("InnboardFromYearShow", GetDropDownFirstValue());

            int uptoYear = Int32.Parse(fields[1].FieldValue);
            int currentYear = Int32.Parse(DateTime.Now.Year.ToString());
            int previousYear = currentYear - uptoYear;
            List<string> yearList = new List<string>();
            for (int i = currentYear; i >= previousYear; i--)
            {
                yearList.Add(i.ToString());
            }
            return yearList;
        }
        public string GetExpireInformation(string searchType)
        {
            string returnString = string.Empty;
            if (searchType == "FieldType")
            {
                returnString = "Innb0@rdEncryption";
            }

            return returnString;
        }
        public string GetActivityTypeEnumDescription(ActivityTypeEnum.ActivityType value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        public string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        public string GetEntityTypeEnumDescription(EntityTypeEnum.EntityType value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        /// <summary>
        /// Converts Number to Word
        /// </summary>
        /// <param name="n">Non Negetive Integer Number</param>
        /// <returns></returns>
        public static string ConvertNumberToString(int n)
        {
            if (n < 0)
                throw new NotSupportedException("negative numbers not supported");
            if (n == 0)
                return "zero";
            if (n < 10)
                return ConvertDigitToString(n);
            if (n < 20)
                return ConvertTeensToString(n);
            if (n < 100)
                return ConvertHighTensToString(n);
            if (n < 1000)
                return ConvertBigNumberToString(n, (int)1e2, "hundred");
            if (n < 1e6)
                return ConvertBigNumberToString(n, (int)1e3, "thousand");
            if (n < 1e9)
                return ConvertBigNumberToString(n, (int)1e6, "million");
            //if (n < 1e12)
            return ConvertBigNumberToString(n, (int)1e9, "billion");
        }

        private static string ConvertDigitToString(int i)
        {
            switch (i)
            {
                case 0: return "";
                case 1: return "one";
                case 2: return "two";
                case 3: return "three";
                case 4: return "four";
                case 5: return "five";
                case 6: return "six";
                case 7: return "seven";
                case 8: return "eight";
                case 9: return "nine";
                default:
                    throw new IndexOutOfRangeException(String.Format("{0} not a digit", i));
            }
        }

        //assumes a number between 10 & 19
        private static string ConvertTeensToString(int n)
        {
            switch (n)
            {
                case 10: return "ten";
                case 11: return "eleven";
                case 12: return "twelve";
                case 13: return "thirteen";
                case 14: return "fourteen";
                case 15: return "fiveteen";
                case 16: return "sixteen";
                case 17: return "seventeen";
                case 18: return "eighteen";
                case 19: return "nineteen";
                default:
                    throw new IndexOutOfRangeException(String.Format("{0} not a teen", n));
            }
        }

        //assumes a number between 20 and 99
        private static string ConvertHighTensToString(int n)
        {
            int tensDigit = (int)(Math.Floor((double)n / 10.0));

            string tensStr;
            switch (tensDigit)
            {
                case 2: tensStr = "twenty"; break;
                case 3: tensStr = "thirty"; break;
                case 4: tensStr = "forty"; break;
                case 5: tensStr = "fifty"; break;
                case 6: tensStr = "sixty"; break;
                case 7: tensStr = "seventy"; break;
                case 8: tensStr = "eighty"; break;
                case 9: tensStr = "ninety"; break;
                default:
                    throw new IndexOutOfRangeException(String.Format("{0} not in range 20-99", n));
            }
            if (n % 10 == 0) return tensStr;
            string onesStr = ConvertDigitToString(n - tensDigit * 10);
            return tensStr + "-" + onesStr;
        }

        private static string ConvertBigNumberToString(int n, int baseNum, string baseNumStr)
        {
            // special case: use commas to separate portions of the number, unless we are in the hundreds
            string separator = (baseNumStr != "hundred") ? ", " : " ";

            // Strategy: translate the first portion of the number, then recursively translate the remaining sections.
            // Step 1: strip off first portion, and convert it to string:
            int bigPart = (int)(Math.Floor((double)n / baseNum));
            string bigPartStr = ConvertNumberToString(bigPart) + " " + baseNumStr;
            // Step 2: check to see whether we're done:
            if (n % baseNum == 0) return bigPartStr;
            // Step 3: concatenate 1st part of string with recursively generated remainder:
            int restOfNumber = n - bigPart * baseNum;
            return bigPartStr + separator + ConvertNumberToString(restOfNumber);
        }

        #region ReportError
        /// <summary>
        /// Send email to the on exception
        /// </summary>
        /// <param name="ex">Exception</param>
        public static void ReportError(Exception ex)
        {
            if (ex.Message == "File does not exist.")
            {
                return;
            }

            string errorEmailTo = System.Web.Configuration.WebConfigurationManager.AppSettings["ErrorReportMailTo"].ToString();
            string subject = System.Web.Configuration.WebConfigurationManager.AppSettings["ErrorMailSubject"].ToString();
            string mailBody = GetMailMessageBody(ex);

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
            string mainString = commonSetupBO.SetupValue;
            Email email;

            if (!string.IsNullOrEmpty(mainString))
            {
                string[] dataArray = mainString.Split('~');
                email = new Email()
                {
                    From = dataArray[0],
                    Password = dataArray[1],
                    To = errorEmailTo,
                    Subject = subject,
                    Body = mailBody,
                    Host = dataArray[2],
                    Port = dataArray[3]
                };

                try
                {
                    EmailHelper.SendEmail(email,null);
                }
                catch (Exception exx)
                {
                    throw exx;
                }
            }            
        }
        #endregion

        #region GetMailMessageBody
        /// <summary>
        /// Builds the error report mail message body
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>message body</returns>
        public static string GetMailMessageBody(Exception ex)
        {
            string messageBody = "";
            //create the email body
            messageBody += "<b>Browser: </b>" + HttpContext.Current.Request.Browser.Browser + " " + HttpContext.Current.Request.Browser.Version.ToString() + "<br/><br/>";
            messageBody += "<b>Date/Time: </b>" + DateTime.Now + "<br/><br/>";
            messageBody += "<b>Diagnostics: </b><br/>";
            if (ex.InnerException != null)
            {
                messageBody += "&nbsp;&nbsp;<b>Error message: </b>" + ex.InnerException.Message + "<br/>"; ;
                messageBody += "&nbsp;&nbsp;<b>Stack trace: </b>" + ex.InnerException.StackTrace + "<br/><br/>";
            }
            string referrer = "";
            if (HttpContext.Current.Request.UrlReferrer != null)
            {
                referrer = HttpContext.Current.Request.UrlReferrer.ToString();
            }
            else
            {
                referrer = "Self";
            }
            messageBody += "<b>Referrer: </b>" + referrer + "<br/>";
            messageBody += "<b>Url: </b>" + HttpContext.Current.Request.Url.ToString() + "<br/>";
            messageBody += "<b>Remote address: </b>" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

            return messageBody;
        }
        #endregion
        
        public string GetTooltipContainer(string RoomId, string RoomNumber, string RoomType, string RoomName)
        {
            string tooltipContent = "<div id='ContentDiv" + RoomId + "' class='ContentDiv' style='display: none;'>";
            tooltipContent += "<table class='ContentTable'>";
            tooltipContent += "<tr>";
            tooltipContent += "<td colspan='2' class='ModalTitle'>Room No : ";
            tooltipContent += RoomNumber;
            tooltipContent += "</td>";
            tooltipContent += "</tr>";
            tooltipContent += "<tr>";
            tooltipContent += "<td class='modal_label'>Type</td>";
            tooltipContent += "<td class='modal_data'>: " + RoomType + "</td>";
            tooltipContent += "</tr>";
            tooltipContent += "<tr>";
            tooltipContent += "<td class='modal_label'>Name</td>";
            tooltipContent += "<td class='modal_data'>: " + RoomName + "</td>";
            tooltipContent += "</tr>";
            tooltipContent += "<tr>";
            tooltipContent += "<td class='modal_label'>Number</td>";
            tooltipContent += "<td class='modal_data'>: " + RoomNumber + "</td>";
            tooltipContent += "</tr>";
            tooltipContent += "</table>";
            tooltipContent += "</div>";
            return tooltipContent;
        }
    }
}