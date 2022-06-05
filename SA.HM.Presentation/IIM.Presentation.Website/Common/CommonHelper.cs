using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using HotelManagement.Entity.UserInformation;
using System.Web.UI;
using System.Reflection;
using Microsoft.Reporting.WebForms;
using System.Text.RegularExpressions;
using HotelManagement.Entity.HMCommon;
using System.Globalization;
using System.Collections;

namespace HotelManagement.Presentation.Website.Common
{
    public static class CommonHelper
    {
        public const string SaveMessage = "Saved Operation Successfull.";
        public const string UpdateMessage = "Update Operation Successfull.";
        public const string DeleteMessage = "Delete Operation Successfull.";
        public const string ErrorMessage = "Operation Failed.";

        public enum MessageTpe
        {
            Info,
            Success,
            Error
        }
        public enum ReportImportFormat
        {
            Excel,
            Word,
            PDF
        }
        public enum QuotationItemType
        {
            Item,
            Service
        }
        public enum CostAnalysisItemType
        {
            Item,
            Service
        }

        public static Alert AlertInfo(string message, string alertType)
        {
            Alert alert = new Alert();
            alert.Message = message;
            alert.AlertType = alertType;

            return alert;
        }
        public static void AlertInfo(HiddenField hfInboardMessage, string message, string alertType, int timeToDisplay = 0, string rederictUrl = "")
        {
            Alert alert = new Alert();
            alert.Message = message;
            alert.AlertType = alertType;
            alert.TimeToDisplay = timeToDisplay;
            alert.RederictUrl = rederictUrl;

            if (alertType == AlertType.Error)
            {
                alert.IsSuccess = 0;
            }
            else
            {
                alert.IsSuccess = 1;
            }

            hfInboardMessage.Value = JsonConvert.SerializeObject(alert);
        }

        public static MessageUtility MessageInfo(string messageType, string message)
        {
            MessageUtility util = new MessageUtility();

            util.MessageType = messageType;
            util.Message = message;

            return util;
        }
        public static MessageUtility MessageInfo(string messageType, string message, int id = 0)
        {
            MessageUtility util = new MessageUtility();
            util.MessageType = messageType;
            util.Message = message;
            util.Id = id;

            return util;
        }
        public static string AgeCalculation(DateTime dateOfBirth)
        {
            DateTime adtCurrentDate = DateTime.Now;
            int ageNoOfYears, ageNoOfMonths, ageNoOfDays;
            string age = string.Empty;

            ageNoOfDays = adtCurrentDate.Day - dateOfBirth.Day;
            ageNoOfMonths = adtCurrentDate.Month - dateOfBirth.Month;
            ageNoOfYears = adtCurrentDate.Year - dateOfBirth.Year;

            if (ageNoOfDays < 0)
            {
                ageNoOfDays += DateTime.DaysInMonth(adtCurrentDate.Year, adtCurrentDate.Month);
                ageNoOfDays--;
            }

            if (ageNoOfMonths < 0)
            {
                ageNoOfMonths += 12;
                ageNoOfMonths--;
            }

            age = ageNoOfYears.ToString() + " years, " + ageNoOfMonths.ToString() + " months, " + ageNoOfDays.ToString() + " days";

            return age;

            // Antoher way
            //DateTime dob = Convert.ToDateTime("18 Feb 1987");
            //DateTime PresentYear = DateTime.Now;
            //TimeSpan ts = PresentYear - dob;
            //DateTime Age = DateTime.MinValue.AddDays(ts.Days);
            //MessageBox.Show(string.Format(" {0} Years {1} Month {2} Days", Age.Year - 1, Age.Month - 1, Age.Day - 1));
        }
        public static int? GetFilterUser(UserInformationBO userInformationBO)
        {
            int? userId = null;

            if (userInformationBO.UserInfoId != 1)
            {
                userId = userInformationBO.UserInfoId;
            }

            return userId;
        }
        public static string ComparisionValue(string compareOperator)
        {
            ComaprisionOperator op = new ComaprisionOperator();

            if (compareOperator == HMConstants.ComapreString.EQ.ToString())
            {
                return op.EQ;
            }
            else if (compareOperator == HMConstants.ComapreString.GT.ToString())
            {
                return op.GT;
            }
            else if (compareOperator == HMConstants.ComapreString.LT.ToString())
            {
                return op.LT;
            }
            else if (compareOperator == HMConstants.ComapreString.GE.ToString())
            {
                return op.GE;
            }
            else if (compareOperator == HMConstants.ComapreString.LE.ToString())
            {
                return op.LE;
            }
            else
            {
                return op.EQ;
            }
        }
        public static string SalaryDateFrom(string monthRange, string salaryYear)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int daysInAMonth = 30;
            string[] salaryMonthRange = monthRange.Split('-');
            int startMonth = Convert.ToInt32(salaryMonthRange[0]), endMonth = Convert.ToInt32(salaryMonthRange[1]);
            int salaryMonthStartDateDay = Convert.ToInt32(salaryMonthRange[2]);

            string fromSalaryDate = string.Empty, toSalaryDate = string.Empty;

            if (startMonth == endMonth)
            {
                daysInAMonth = System.DateTime.DaysInMonth(Convert.ToInt32(salaryYear), Convert.ToInt32(startMonth));

                //fromSalaryDate = salaryYear + "-" + startMonth.ToString().PadLeft(2, '0') + "-" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                //toSalaryDate = salaryYear + "-" + (endMonth).ToString().PadLeft(2, '0') + "-" + daysInAMonth.ToString().PadLeft(2, '0');
                if (userInformationBO.ServerDateFormat == "dd/MM/yyyy")
                {
                    fromSalaryDate = salaryYear + "/" + startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                    toSalaryDate = salaryYear + "/" + endMonth.ToString().PadLeft(2, '0') + "/" + daysInAMonth.ToString().PadLeft(2, '0');
                }
                else if (userInformationBO.ServerDateFormat == "MM/dd/yyyy")
                {
                    fromSalaryDate = salaryYear + "/" + startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                    toSalaryDate = salaryYear + "/" + endMonth.ToString().PadLeft(2, '0') + "/" + daysInAMonth.ToString().PadLeft(2, '0');

                    //fromSalaryDate = startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + salaryYear;
                    //toSalaryDate = (endMonth).ToString().PadLeft(2, '0') + "/" + daysInAMonth.ToString().PadLeft(2, '0') + "/" + salaryYear;
                }
            }
            else
            {
                if (startMonth < endMonth && Convert.ToInt32(salaryYear) <= DateTime.Now.Year)
                {
                    //fromSalaryDate = salaryYear + "-" + startMonth.ToString().PadLeft(2, '0') + "-" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                    //toSalaryDate = salaryYear + "-" + endMonth.ToString().PadLeft(2, '0') + "-" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                    if (userInformationBO.ServerDateFormat == "dd/MM/yyyy")
                    {
                        fromSalaryDate = salaryYear + "/" + startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                        toSalaryDate = salaryYear + "/" + endMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');

                        //fromSalaryDate = startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + salaryYear;
                        //toSalaryDate = salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + endMonth.ToString().PadLeft(2, '0') + "/" + salaryYear;
                    }
                    else if (userInformationBO.ServerDateFormat == "MM/dd/yyyy")
                    {
                        fromSalaryDate = salaryYear + "/" + startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                        toSalaryDate = salaryYear + "/" + endMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');

                        //fromSalaryDate = startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + salaryYear;
                        //toSalaryDate = endMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + salaryYear;
                    }
                }
                else if (endMonth < startMonth && DateTime.Now.Year > Convert.ToInt32(salaryYear))
                {
                    //fromSalaryDate = salaryYear + "-" + startMonth.ToString().PadLeft(2, '0') + "-" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                    //toSalaryDate = (Convert.ToInt32(salaryYear) + 1).ToString() + "-" + endMonth.ToString().PadLeft(2, '0') + "-" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                    if (userInformationBO.ServerDateFormat == "dd/MM/yyyy")
                    {
                        fromSalaryDate = salaryYear + "/" + startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                        toSalaryDate = (Convert.ToInt32(salaryYear) + 1).ToString() + "/" + endMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');

                        //fromSalaryDate = startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + salaryYear;
                        //toSalaryDate = salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + endMonth.ToString().PadLeft(2, '0') + "/" + (Convert.ToInt32(salaryYear) + 1).ToString();
                    }
                    else if (userInformationBO.ServerDateFormat == "MM/dd/yyyy")
                    {
                        fromSalaryDate = salaryYear + "/" + startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                        toSalaryDate = (Convert.ToInt32(salaryYear) + 1).ToString() + "/" + endMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                    }
                }
            }

            return fromSalaryDate;
        }
        public static string SalaryDateTo(string monthRange, string salaryYear)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int daysInAMonth = 30;
            string[] salaryMonthRange = monthRange.Split('-');
            int startMonth = Convert.ToInt32(salaryMonthRange[0]), endMonth = Convert.ToInt32(salaryMonthRange[1]);
            int salaryMonthStartDateDay = Convert.ToInt32(salaryMonthRange[2]);

            string fromSalaryDate = string.Empty, toSalaryDate = string.Empty;

            if (startMonth == endMonth)
            {
                daysInAMonth = System.DateTime.DaysInMonth(Convert.ToInt32(salaryYear), Convert.ToInt32(startMonth));

                //fromSalaryDate = salaryYear + "-" + startMonth.ToString().PadLeft(2, '0') + "-" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                //toSalaryDate = salaryYear + "-" + (endMonth).ToString().PadLeft(2, '0') + "-" + daysInAMonth.ToString().PadLeft(2, '0');
                if (userInformationBO.ServerDateFormat == "dd/MM/yyyy")
                {
                    fromSalaryDate = salaryYear + "/" + startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                    toSalaryDate = salaryYear + "/" + endMonth.ToString().PadLeft(2, '0') + "/" + daysInAMonth.ToString().PadLeft(2, '0');
                }
                else if (userInformationBO.ServerDateFormat == "MM/dd/yyyy")
                {
                    fromSalaryDate = salaryYear + "/" + startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                    toSalaryDate = salaryYear + "/" + endMonth.ToString().PadLeft(2, '0') + "/" + daysInAMonth.ToString().PadLeft(2, '0');

                    //fromSalaryDate = startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + salaryYear;
                    //toSalaryDate = (endMonth).ToString().PadLeft(2, '0') + "/" + daysInAMonth.ToString().PadLeft(2, '0') + "/" + salaryYear;
                }
            }
            else
            {
                if (startMonth < endMonth && Convert.ToInt32(salaryYear) <= DateTime.Now.Year)
                {
                    //fromSalaryDate = salaryYear + "-" + startMonth.ToString().PadLeft(2, '0') + "-" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                    //toSalaryDate = salaryYear + "-" + endMonth.ToString().PadLeft(2, '0') + "-" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                    if (userInformationBO.ServerDateFormat == "dd/MM/yyyy")
                    {
                        fromSalaryDate = salaryYear + "/" + startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                        toSalaryDate = salaryYear + "/" + endMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');

                        //fromSalaryDate = startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + salaryYear;
                        //toSalaryDate = salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + endMonth.ToString().PadLeft(2, '0') + "/" + salaryYear;
                    }
                    else if (userInformationBO.ServerDateFormat == "MM/dd/yyyy")
                    {
                        fromSalaryDate = salaryYear + "/" + startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                        toSalaryDate = salaryYear + "/" + endMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');

                        //fromSalaryDate = startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + salaryYear;
                        //toSalaryDate = endMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + salaryYear;
                    }
                }
                else if (endMonth < startMonth && DateTime.Now.Year > Convert.ToInt32(salaryYear))
                {
                    //fromSalaryDate = salaryYear + "-" + startMonth.ToString().PadLeft(2, '0') + "-" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                    //toSalaryDate = (Convert.ToInt32(salaryYear) + 1).ToString() + "-" + endMonth.ToString().PadLeft(2, '0') + "-" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                    if (userInformationBO.ServerDateFormat == "dd/MM/yyyy")
                    {
                        fromSalaryDate = salaryYear + "/" + startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                        toSalaryDate = (Convert.ToInt32(salaryYear) + 1).ToString() + "/" + endMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');

                        //fromSalaryDate = startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + salaryYear;
                        //toSalaryDate = salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + endMonth.ToString().PadLeft(2, '0') + "/" + (Convert.ToInt32(salaryYear) + 1).ToString();
                    }
                    else if (userInformationBO.ServerDateFormat == "MM/dd/yyyy")
                    {
                        fromSalaryDate = salaryYear + "/" + startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                        toSalaryDate = (Convert.ToInt32(salaryYear) + 1).ToString() + "/" + endMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
                    }
                }
            }

            //if (startMonth == endMonth)
            //{
            //    daysInAMonth = System.DateTime.DaysInMonth(Convert.ToInt32(salaryYear), Convert.ToInt32(startMonth));

            //    //fromSalaryDate = salaryYear + "-" + startMonth.ToString().PadLeft(2, '0') + "-" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
            //    //toSalaryDate = salaryYear + "-" + (endMonth).ToString().PadLeft(2, '0') + "-" + daysInAMonth.ToString().PadLeft(2, '0');
            //    if (userInformationBO.ServerDateFormat == "dd/MM/yyyy")
            //    {
            //        fromSalaryDate = startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + salaryYear;
            //        toSalaryDate = (endMonth).ToString().PadLeft(2, '0') + "/" + daysInAMonth.ToString().PadLeft(2, '0') + "/" + salaryYear;
            //    }
            //    else if (userInformationBO.ServerDateFormat == "MM/dd/yyyy")
            //    {
            //        fromSalaryDate = startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + salaryYear;
            //        toSalaryDate = (endMonth).ToString().PadLeft(2, '0') + "/" + daysInAMonth.ToString().PadLeft(2, '0') + "/" + salaryYear;
            //    }

            //}
            //else
            //{
            //    if (startMonth < endMonth && Convert.ToInt32(salaryYear) <= DateTime.Now.Year)
            //    {
            //        //fromSalaryDate = salaryYear + "-" + startMonth.ToString().PadLeft(2, '0') + "-" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
            //        //toSalaryDate = salaryYear + "-" + endMonth.ToString().PadLeft(2, '0') + "-" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
            //        if (userInformationBO.ServerDateFormat == "dd/MM/yyyy")
            //        {
            //            fromSalaryDate = startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + salaryYear;
            //            toSalaryDate = endMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + salaryYear;
            //        }
            //        else if (userInformationBO.ServerDateFormat == "MM/dd/yyyy")
            //        {
            //            fromSalaryDate = startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + salaryYear;
            //            toSalaryDate = endMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + salaryYear;
            //        }
            //    }
            //    else if (endMonth < startMonth && DateTime.Now.Year > Convert.ToInt32(salaryYear))
            //    {
            //        //fromSalaryDate = salaryYear + "-" + startMonth.ToString().PadLeft(2, '0') + "-" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
            //        //toSalaryDate = (Convert.ToInt32(salaryYear) + 1).ToString() + "-" + endMonth.ToString().PadLeft(2, '0') + "-" + salaryMonthStartDateDay.ToString().PadLeft(2, '0');
            //        if (userInformationBO.ServerDateFormat == "dd/MM/yyyy")
            //        {
            //            fromSalaryDate = startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + salaryYear;
            //            toSalaryDate = endMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + (Convert.ToInt32(salaryYear) + 1).ToString();
            //        }
            //        else if (userInformationBO.ServerDateFormat == "MM/dd/yyyy")
            //        {
            //            fromSalaryDate = startMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + salaryYear;
            //            toSalaryDate = endMonth.ToString().PadLeft(2, '0') + "/" + salaryMonthStartDateDay.ToString().PadLeft(2, '0') + "/" + (Convert.ToInt32(salaryYear) + 1).ToString();
            //        }
            //    }
            //}

            return toSalaryDate;
        }
        public static DateTime DateTimeToMMDDYYYY(string strDate)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            return DateTimeToMMDDYYYY(strDate, userInformationBO.ClientDateFormat);
        }
        public static DateTime DateTimeToMMDDYYYY(string strDate, string dateFormat)
        {
            string[] salaryMonthRange = strDate.Split('/');
            string convertedDate = string.Empty;

            if (dateFormat == "dd/mm/yy")
            {
                convertedDate = salaryMonthRange[1] + "/" + salaryMonthRange[0] + "/" + salaryMonthRange[2];

            }
            else if (dateFormat == "mm/dd/yy")
            {
                convertedDate = strDate;
            }


            return Convert.ToDateTime(convertedDate);
        }

        public static string DateTimeClientFormatWiseConversionForSaveUpdate(DateTime date)
        {
            string convertedDate = string.Empty;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO.ClientDateFormat == "dd/mm/yy")
            {
                convertedDate = date.ToString("MM/dd/yyyy");
            }
            else if (userInformationBO.ClientDateFormat == "mm/dd/yy")
            {
                convertedDate = date.ToString("MM/dd/yyyy");
            }

            return convertedDate;
        }
        public static string DateTimeClientFormatWiseConversionForDisplay(DateTime date)
        {
            string convertedDate = string.Empty;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO.ClientDateFormat == "dd/mm/yy")
            {
                convertedDate = date.ToString("dd/MM/yyyy");
            }
            else if (userInformationBO.ClientDateFormat == "mm/dd/yy")
            {
                convertedDate = date.ToString("MM/dd/yyyy");
            }

            return convertedDate;
        }
        public static string DateTimeConvertion(DateTime date)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            return date.ToString(userInformationBO.ServerDateFormat);
        }
        public static List<DateTime> GetDateArrayBetweenTwoDates(DateTime StartDate, DateTime EndDate)
        {
            var dates = new List<DateTime>();

            for (var dt = StartDate; dt <= EndDate; dt = dt.AddDays(1))
            {
                //dates.Add(dt.AddDays(1).AddSeconds(-1));
                dates.Add(dt);
            }
            return dates;
        }
        public static void ClearControl(Control formAllControl)
        {
            foreach (Control formControl in formAllControl.Controls)
            {
                if (formControl is TextBox)
                {
                    (formControl as TextBox).Text = string.Empty;
                }
                else if (formControl is RadioButtonList)
                {
                    (formControl as RadioButtonList).ClearSelection();
                }
                else if (formControl.GetType() == typeof(RadioButton))
                {
                    (formControl as RadioButton).Checked = false;
                }
                else if (formControl is ListBox)
                {
                    (formControl as ListBox).ClearSelection();
                }
                else if (formControl is CheckBox)
                {
                    (formControl as CheckBox).Checked = false;
                }
                else if (formControl is DropDownList)
                {
                    (formControl as DropDownList).SelectedIndex = 0;
                }
                else if (formControl is HiddenField)
                {
                    (formControl as HiddenField).Value = string.Empty;
                }
            }
        }
        public static string ExceptionErrorMessage(Exception ex)
        {
            string message = string.Empty;

            if (ex.InnerException != null)
            {
                message = ex.InnerException.Message;
            }
            else
            {
                message = ex.Message;
            }

            return message;
        }
        public static string RestaurantOrderSourceType(string sourceType)
        {
            if (sourceType == "tkn")
            {
                sourceType = ConstantHelper.RestaurantBillSource.RestaurantToken.ToString();
            }
            else if (sourceType == "tbl")
            {
                sourceType = ConstantHelper.RestaurantBillSource.RestaurantTable.ToString();
            }
            else if (sourceType == "rom")
            {
                sourceType = ConstantHelper.RestaurantBillSource.GuestRoom.ToString();
            }

            return sourceType;
        }
        public static void DisableReportExportFormat(Microsoft.Reporting.WebForms.ReportViewer ReportViewerId, string ImportFormatName)
        {
            FieldInfo info;
            foreach (RenderingExtension extension in ReportViewerId.LocalReport.ListRenderingExtensions())
            {
                if (extension.Name.Trim().ToUpper() == ImportFormatName.Trim().ToUpper())
                {
                    info = extension.GetType().GetField("m_isVisible", BindingFlags.Instance | BindingFlags.NonPublic);
                    info.SetValue(extension, false);
                }
            }

        }

        public static string SentenceCaseConvertionTolowerCase(this string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
            //return Regex.Replace(str, "[a-z][A-Z]", m => $"{m.Value[0]} {char.ToLower(m.Value[1])}");
        }
        public static string SentenceCaseConvertionToUpperCase(this string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToUpper(m.Value[1]));
            //return Regex.Replace(str, "[a-z][A-Z]", m => $"{m.Value[0]} {char.ToLower(m.Value[1])}");
        }

        public static List<MonthYearBO> MonthGeneration(string format)
        {
            List<MonthYearBO> monthyear = new List<MonthYearBO>();
            DateTime dateFrom = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime dateTo = new DateTime(DateTime.Now.Year, 12, 31);

            for (var d = dateFrom; d <= dateTo; d = d.AddMonths(1))
            {
                monthyear.Add(new MonthYearBO
                {
                    MonthId = d.Month,
                    MonthName = d.ToString(format)
                });
            }

            return monthyear;
        }

        public static T ConvertDateTimePropertiesUTCtoLocalTime<T>(T entity) where T : class
        {
            var properties = entity.GetType().GetProperties();

            foreach (var property in properties)
            {
                var entityobject = property.GetValue(entity,null);

                if (entityobject == null || entityobject.GetType().IsPrimitive)
                    continue;
                else if (property.PropertyType == typeof(DateTime) || (property.PropertyType == typeof(DateTime?)
                                && property.GetValue(entity, null) != null) && property.CanWrite)
                {

                    property.SetValue(entity, ((DateTime)property.GetValue(entity, null)).ToLocalTime(), null);

                }
                else if (entityobject.GetType().Name == typeof(List<>).Name)
                {
                    var nestedList = property.GetValue(entity, null);

                    foreach (var item in (IList)nestedList)
                    {
                        var nestedProperties = item.GetType().GetProperties();

                        foreach (var prop in nestedProperties)
                        {
                            if (prop.PropertyType == typeof(DateTime) || (prop.PropertyType == typeof(DateTime?)
                                            && prop.GetValue(item, null) != null) && prop.CanWrite)
                                prop.SetValue(item, ((DateTime)prop.GetValue(item, null)).ToLocalTime(), null);
                        }
                    }
                }
                else
                {
                    var nestedProperties = entityobject.GetType().GetProperties();
                    foreach (var item in nestedProperties)
                    {
                        if (item.PropertyType == typeof(DateTime) || (item.PropertyType == typeof(DateTime?)
                                    && item.GetValue(entityobject, null) != null) && property.CanWrite)
                            item.SetValue(entityobject, ((DateTime)item.GetValue(entityobject, null)).ToLocalTime(), null);
                    }
                }
            }
            return entity;
        }
    }
}