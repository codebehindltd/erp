using HotelManagement.Data.HMCommon;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing.Reports
{
    public partial class CrmDynamicReport : System.Web.UI.Page
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        HMCommonDA hmCommonDA = new HMCommonDA();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static ReturnInfo SaveAndGenerateReport(bool isSave, List<string> columns, List<string> tables, List<string> orderBy, string fromDate, string toDate, string reportType, string reportName, string filter, string sequence)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            string sqlComm = string.Empty;
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            if (fromDate != "")
            {
                FromDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                HttpContext.Current.Session["GetDynamicFromDate"] = FromDate.ToShortDateString();

            }
            if (toDate != "")
            {
                ToDate = hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                ToDate = ToDate.AddDays(1).AddSeconds(-1);
                HttpContext.Current.Session["GetDynamicToDate"] = ToDate;
            }

            List<DynamicReportBO> dynamicReports = new List<DynamicReportBO>();
            DynamicReportDA reportDA = new DynamicReportDA();
            List<string> tempTable = new List<string>();

            bool hasCreatedBy = false;
            bool hasLastModifieddBy = false;
            
            foreach (var item in columns)
            {
                string[] tokens = item.Split('.');

                if (tokens[1].Contains("CreatedBy"))
                {
                    hasCreatedBy = true;
                    //break;
                }
                if (tokens[1].Contains("LastModifiedBy"))
                {
                    hasLastModifieddBy = true;
                    //break;
                }
               
            }
            

            sqlComm += "SELECT ";
            //ROW_NUMBER() OVER(ORDER BY COmpanyId Desc) RowId
            if (orderBy.Count > 0)
            {
                sqlComm += " ROW_NUMBER() OVER( ORDER BY ";
                for (var i = 0; i < orderBy.Count; i++)
                {
                    sqlComm += orderBy[i];
                    sqlComm += (i < orderBy.Count - 1) ? " , " : "";
                }
                sqlComm += " " + sequence + " ) RowId, ";

            }
            else
            {
                sqlComm += " ROW_NUMBER() OVER( ORDER BY ";
                for (var i = 0; i < columns.Count; i++)
                {
                    sqlComm += columns[i];
                    sqlComm += (i < columns.Count - 1) ? " , " : "";
                }
                sqlComm += " " + sequence + " ) RowId, ";
            }
            for (var i = 0; i < columns.Count; i++)
            {
                string[] tokens = columns[i].Split('.');
                if (hasCreatedBy && (tokens[1].Contains("CreatedBy")))
                {
                    sqlComm += " SecurityUserInformation.UserName AS CreatedByName ";

                    columns.Remove(columns[i]);
                    i--;
                }
                else if (hasLastModifieddBy && tokens[1].Contains("LastModifiedBy"))
                {
                    sqlComm += " SUI.UserName AS LastModifiedByName ";

                    columns.Remove(columns[i]);
                    i--;
                }
                else
                    sqlComm += columns[i]; // + " AS " + columns[i].Replace(".", "")
                sqlComm += (i < columns.Count - 1) ? " , " : " ";
            }
            sqlComm += " INTO ##TT ";
            sqlComm += " FROM ";

            // rearranging tables with relations
            foreach (var item in tables)
            {
                if (item == "SMQuotation")
                {
                    tempTable.Add(item);
                }
                else if (item == "SMDeal")
                {
                    tempTable.Add(item);
                }
                else if (item == "SMContactInformation")
                {
                    tempTable.Add(item);
                }
                else if (item == "HotelGuestCompany")
                {
                    tempTable.Add(item);
                }
                else if (item == "SMLifeCycleStage")
                {
                    tempTable.Add(item);
                }
                else if (item == "SMDealStage")
                {
                    tempTable.Add(item);
                }
            }
            
            sqlComm += JoinTable(tempTable, hasCreatedBy, hasLastModifieddBy);

            foreach (var item in columns)
            {
                if (item.Contains("Date"))
                {
                    sqlComm += " WHERE " + item + " BETWEEN '" + FromDate + "' AND '" + ToDate + "' ";
                }
            }

            sqlComm += " ORDER BY ";
            for (var i = 0; i < orderBy.Count; i++)
            {
                sqlComm += orderBy[i];
                sqlComm += (i < orderBy.Count - 1) ? " , " : " ";
            }
            sqlComm += " " + sequence;
            dynamicReports = reportDA.GetDataForReport(sqlComm);
            HttpContext.Current.Session["GetDynamic"] = dynamicReports;

            if (isSave)// save now
            {

            }
            return returnInfo;
        }
        //SMContactInformation HotelGuestCompany SMQuotation SMLifeCycleStage SMDealStage SMDeal

        private static string JoinTable(List<string> tempTable, bool hasCreatedBy, bool hasLastModifieddBy)
        {
            string str = string.Empty;
            for (int i = 0; i < tempTable.Count; i++)
            {
                if (tempTable[i] == "SMQuotation")
                {
                    if (str == "")
                    {
                        str += " SMQuotation ";
                    }

                    for (int j = i + 1; j < tempTable.Count; j++)
                    {
                        if (tempTable[j] == "SMDeal")
                        {
                            str += " LEFT JOIN SMDeal ON SMQuotation.DealId = SMDeal.Id ";
                        }
                        if (tempTable[j] == "SMContactInformation")
                        {
                            str += " LEFT JOIN SMContactInformation ON SMQuotation.ContactId = SMContactInformation.Id ";
                        }
                        if (tempTable[j] == "HotelGuestCompany")
                        {
                            str += " LEFT JOIN HotelGuestCompany ON SMQuotation.CompanyId = HotelGuestCompany.CompanyId ";
                        }
                    }
                    if (hasCreatedBy)
                    {
                        str += " LEFT JOIN SecurityUserInformation ON SecurityUserInformation.UserInfoId = SMQuotation.CreatedBy ";
                        hasCreatedBy = false;
                    }
                    if (hasLastModifieddBy)
                    {
                        str += " LEFT JOIN SecurityUserInformation SUI ON SUI.UserInfoId = SMQuotation.LastModifiedBy ";
                        hasLastModifieddBy = false;
                    }

                }
                else if (tempTable[i] == "SMDeal")
                {
                    if (str == "")
                    {
                        str += " SMDeal ";
                    }

                    for (int j = i + 1; j < tempTable.Count; j++)
                    {
                        if (tempTable[j] == "SMContactInformation")
                        {
                            str += " LEFT JOIN SMDealWiseContactMap ON SMDeal.Id = SMDealWiseContactMap.DealId ";
                            str += " LEFT JOIN SMContactInformation ON SMContactInformation.Id = SMDealWiseContactMap.ContactId ";
                        }
                        if (tempTable[j] == "HotelGuestCompany")
                        {
                            str += " LEFT JOIN HotelGuestCompany ON SMDeal.CompanyId = HotelGuestCompany.CompanyId ";
                        }
                        if (tempTable[j] == "SMDealStage")
                        {
                            str += " LEFT JOIN SMDealStage on SMDeal.StageId = SMDealStage.Id ";
                        }
                    }
                    if (hasCreatedBy)
                    {
                        str += " LEFT JOIN SecurityUserInformation ON SecurityUserInformation.UserInfoId = SMDeal.CreatedBy ";
                        hasCreatedBy = false;
                    }
                    if (hasLastModifieddBy)
                    {
                        str += " LEFT JOIN SecurityUserInformation SUI ON SUI.UserInfoId = SMDeal.LastModifiedBy ";
                        hasLastModifieddBy = false;
                    }

                }
                else if (tempTable[i] == "SMContactInformation")
                {
                    if (str == "")
                    {
                        str += " SMContactInformation ";
                    }

                    for (int j = i + 1; j < tempTable.Count; j++)
                    {
                        if (tempTable[j] == "HotelGuestCompany")
                        {
                            str += " LEFT JOIN HotelGuestCompany ON SMContactInformation.CompanyId = HotelGuestCompany.CompanyId ";
                        }
                        if (tempTable[j] == "SMLifeCycleStage")
                        {
                            str += " LEFT JOIN SMLifeCycleStage ON SMContactInformation.LifeCycleId = SMLifeCycleStage.Id ";
                        }
                    }
                    if (hasCreatedBy)
                    {
                        str += " LEFT JOIN SecurityUserInformation ON SecurityUserInformation.UserInfoId = SMContactInformation.CreatedBy ";
                        hasCreatedBy = false;
                    }
                    if (hasLastModifieddBy)
                    {
                        str += " LEFT JOIN SecurityUserInformation SUI ON SUI.UserInfoId = SMContactInformation.LastModifiedBy ";
                        hasLastModifieddBy = false;
                    }

                }
                else if (tempTable[i] == "HotelGuestCompany")
                {
                    if (str == "")
                    {
                        str += " HotelGuestCompany ";
                    }

                    for (int j = i + 1; j < tempTable.Count; j++)
                    {

                        if (tempTable[j] == "SMLifeCycleStage")
                        {
                            str += " LEFT JOIN SMLifeCycleStage ON HotelGuestCompany.LifeCycleId = SMLifeCycleStage.Id ";
                        }
                    }
                    if (hasCreatedBy)
                    {
                        str += " LEFT JOIN SecurityUserInformation ON SecurityUserInformation.UserInfoId = HotelGuestCompany.CreatedBy ";
                        hasCreatedBy = false;
                    }
                    if (hasLastModifieddBy)
                    {
                        str += " LEFT JOIN SecurityUserInformation SUI ON SUI.UserInfoId = HotelGuestCompany.LastModifiedBy ";
                        hasLastModifieddBy = false;
                    }
                }

            }


            return str;
        }
        [WebMethod]
        public static List<DynamicReportBO> GetFieldsFromTable(string segment)
        {
            List<DynamicReportBO> dynamicReports = new List<DynamicReportBO>();
            DynamicReportDA reportDA = new DynamicReportDA();
            //|| (x.ColumnName != "QuotationId") || (x.ColumnName != "CompanyId")
            dynamicReports = reportDA.GetFieldsFromTable(segment);
            //if (segment == "SMDealStage" || segment == "SMLifeCycleStage" || segment == "SMContactInformation")
            //{
            //    dynamicReports = dynamicReports.Where(x => (x.ColumnName != "Id")).ToList();
            //}
            //else if (segment == "HotelGuestCompany")
            //{
            //    dynamicReports = dynamicReports.Where(x => (x.ColumnName != "CompanyId")).ToList();
            //}
            //else if (segment == "SMQuotation")
            //{
            //    dynamicReports = dynamicReports.Where(x => (x.ColumnName != "QuotationId")).ToList();
            //}

            dynamicReports = dynamicReports.Where(x => (x.ColumnName != "DisplaySequence")).ToList();
            dynamicReports.RemoveAll(u => u.ColumnName.Contains("Is") || u.ColumnName.Contains("Id"));

            return dynamicReports;
        }
    }
}