using HotelManagement.Data.HMCommon;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class DynamicEmployeeReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        HMUtility hmUtility = new HMUtility();
        HMCommonDA hmCommonDA = new HMCommonDA();

        [WebMethod]
        public static ReturnInfo SaveAndGenerateReport(bool isSave, List<string> columns, List<string> tables, List<string> orderBy, List<string> columnsText, List<string> orderByText, string fromDate, string toDate, string reportType, string reportName, string filter, string sequence)
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollReportingTo", "PayrollReportingTo");
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

            CommonDA commonDA = new CommonDA();

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
            //if (orderBy.Count > 0)
            //{
            //    sqlComm += " ROW_NUMBER() OVER( ORDER BY ";
            //    for (var i = 0; i < orderBy.Count; i++)
            //    {
            //        sqlComm += orderBy[i];
            //        sqlComm += (i < orderBy.Count - 1) ? " , " : "";
            //    }
            //    sqlComm += " " + sequence + " ) RowId, ";

            //}
            //else
            //{
            //    sqlComm += " ROW_NUMBER() OVER( ORDER BY ";
            //    for (var i = 0; i < columns.Count; i++)
            //    {
            //        sqlComm += columns[i];
            //        sqlComm += (i < columns.Count - 1) ? " , " : "";
            //    }
            //    sqlComm += " " + sequence + " ) RowId, ";
            //}
            sqlComm += " ROW_NUMBER() OVER( ORDER BY EMP.EmpCode ";
            sqlComm += " " + sequence + " ) RowId, ";

            //var showImg = true;

            for (var i = 0; i < columns.Count; i++)
            {
                string[] tokens = columns[i].Split('.');
                if (hasCreatedBy && (tokens[1].Contains("CreatedBy")))
                {
                    sqlComm += " SecurityUserInformation.UserName AS CreatedByName ";

                    columns.Remove(columns[i]);
                    columnsText.Remove(columnsText[i]);
                    i--;
                }
                else if (hasLastModifieddBy && tokens[1].Contains("LastModifiedBy"))
                {
                    sqlComm += " SUI.UserName AS LastModifiedByName ";

                    columns.Remove(columns[i]);
                    columnsText.Remove(columnsText[i]);
                    i--;
                }
                else if (columns[i].Contains("Date"))
                {
                    sqlComm += " dbo.FnGenerateDateFormat( "+ columns[i] + ", 'DateStamp') "+ columnsText[i].Replace(" ", "");
                }
                else if (columns[i] == "EMP.DepartmentId")
                {
                    sqlComm += " PD.[Name] AS Department ";
                }
                else if (columns[i] == "EMP.EmpTypeId")
                {
                    sqlComm += " PET.[Name] AS " + columnsText[i].Replace(" ", "") + " ";
                }
                else if (columns[i] == "EMP.DesignationId")
                {
                    sqlComm += " PDES.[Name] AS " + columnsText[i].Replace(" ", "") + " ";
                }
                else if (columns[i] == "EMP.BankName")
                {
                    sqlComm += " CB.BankName AS " + columnsText[i].Replace(" ", "") + " ";
                }
                else if (columns[i] == "EMP.GradeId")
                {
                    sqlComm += " PEG.Name " + columnsText[i].Replace(" ", "") + " ";
                }
                else if (columns[i] == "EMP.CountryId")
                {
                    sqlComm += " CCOU.CountryName " + columnsText[i].Replace(" ", "") + " ";
                }
                else if (columns[i] == "EMP.DistrictId")
                {
                    sqlComm += " PDIS.DistrictName " + columnsText[i].Replace(" ", "") + " ";
                }
                else if (columns[i] == "EMP.DivisionId")
                {
                    sqlComm += " PDIV.DivisionName " + columnsText[i].Replace(" ", "") + " ";
                }
                else if (columns[i] == "EMP.ThanaId")
                {
                    sqlComm += " PTH.ThanaName " + columnsText[i].Replace(" ", "") + " ";
                }
                else if (columns[i] == "EMP.WorkStationId")
                {
                    sqlComm += " PEWS.WorkStationName " + columnsText[i].Replace(" ", "") + " ";
                }
                else if (columns[i] == "EMP.EmployeeStatusId")
                {
                    sqlComm += " PESt.EmployeeStatus " + columnsText[i].Replace(" ", "") + " ";
                }
                else if (columns[i] == "EMP.GlCompanyId")
                {
                    sqlComm += " GLC.Name AS  " + columnsText[i].Replace(" ", "") + " ";
                }
                else if (columns[i] == "EMP.PayrollCurrencyId")
                {
                    sqlComm += " CCUR.CurrencyName AS Currency ";
                }
                else if (columns[i] == "EMP.CostCenterId" || columns[i] == "EMP.WorkingCostCenterId")
                {
                    sqlComm += " CCC.CostCenter ";
                }
                else if (columns[i] == "EMP.RepotingTo")
                {
                    if (commonSetupBO.SetupValue == "2")
                        sqlComm += " peOne.DisplayName ReportingTo ";
                    else 
                        sqlComm += " PDES1.Name ReportingTo ";
                }
                else if (columns[i] == "EMP.RepotingTo2")
                {
                    if (commonSetupBO.SetupValue == "2")
                        sqlComm += " peTwo.DisplayName ReportingTo2 ";
                    else
                        sqlComm += " PDES2.Name ReportingTo2 ";
                }
                //else if (showImg == true)
                //{
                //    sqlComm += " CD.Path + '' + CD.Name AS ImageUrl ";
                //    showImg = false;
                //}
                else
                {
                    sqlComm += columns[i] + " AS " + columnsText[i].Replace(" ", "");
                }
                    //sqlComm += columns[i] + " AS " + columnsText[i].Replace(" ", "");
                sqlComm += (i < columns.Count - 1) ? " , " : " ";
            }
            sqlComm += " INTO ##TT ";
            sqlComm += " FROM PayrollEmployee EMP ";
            //foreach (var item in tables)
            //{
            //    if (item == "SMQuotation")
            //    {
            //        tempTable.Add(item);
            //    }
            //    else if (item == "SMDeal")
            //    {
            //        tempTable.Add(item);
            //    }
            //    else if (item == "SMContactInformation")
            //    {
            //        tempTable.Add(item);
            //    }
            //    else if (item == "HotelGuestCompany")
            //    {
            //        tempTable.Add(item);
            //    }
            //    else if (item == "SMLifeCycleStage")
            //    {
            //        tempTable.Add(item);
            //    }
            //    else if (item == "SMDealStage")
            //    {
            //        tempTable.Add(item);
            //    }
            //}

            sqlComm += JoinTable(columns, hasCreatedBy, hasLastModifieddBy);

            //foreach (var item in columns)
            //{
            //    if (item.Contains("Date"))
            //    {
            //        
            //    }
            //}

            if (fromDate != "" && toDate !="")
            {
                sqlComm += " WHERE ( EMP.JoinDate BETWEEN '" + FromDate + "' AND '" + ToDate + "' ) ";
                //sqlComm += " AND ( CD.DocumentCategory = 'Employee Document') ";
            }
            else
            {
                //sqlComm += " WHERE CD.DocumentCategory = 'Employee Document' "; 
            }
            

            sqlComm += " ORDER BY ";
            for (var i = 0; i < orderBy.Count; i++)
            {
                //if (orderBy[i] == "EMP.DepartmentId")
                //{
                //    sqlComm += " EMP.DepartmentId ";
                //}
                //else if (orderBy[i] == "EMP.EmpTypeId")
                //{
                //    sqlComm += " EMP.EmpTypeId ";
                //}
                //else if (orderBy[i] == "EMP.DesignationId")
                //{
                //    sqlComm += " EMP.DesignationId ";
                //}
                //else if (orderBy[i] == "EMP.GradeId")
                //{
                //    sqlComm += " EMP.GradeId ";
                //}
                //else if (orderBy[i] == "EMP.CountryId")
                //{
                //    sqlComm += " EMP.CountryId ";
                //}
                //else if (orderBy[i] == "EMP.DistrictName")
                //{
                //    sqlComm += " EMP.DistrictId ";
                //}
                //else if (orderBy[i] == "EMP.DivisionName")
                //{
                //    sqlComm += " EMP.DivisionId ";
                //}
                //else if (orderBy[i] == "EMP.ThanaName")
                //{
                //    sqlComm += " EMP.ThanaId ";
                //}
                //else if (orderBy[i] == "EMP.WorkStation")
                //{
                //    sqlComm += " EMP.WorkStationId ";
                //}
                //else if (orderBy[i] == "EMP.EmployeeStatus")
                //{
                //    sqlComm += " PESt.EmployeeStatusId ";
                //}
                //else if (orderBy[i] == "EMP.GlCompany")
                //{
                //    sqlComm += " EMP.GlCompanyId ";
                //}
                //else if (orderBy[i] == "EMP.PayrollCurrency")
                //{
                //    sqlComm += " EMP.PayrollCurrencyId ";
                //}
                //else if (orderBy[i] == "EMP.CostCenter" || columns[i] == "EMP.WorkingCostCenter")
                //{
                //    sqlComm += " EMP.WorkingCostCenterId ";
                //}
                //else if (orderBy[i] == "EMP.RepotingTo")
                //{
                //    sqlComm += " EMP.RepotingTo ";
                //}
                //else if (orderBy[i] == "EMP.RepotingTo2")
                //{
                //    sqlComm += " EMP.RepotingTo2 ";
                //}
                //else
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

        [WebMethod]
        public static ArrayList GetFieldsFromTable(string segment)
        {
            List<DynamicReportBO> dynamicReports = new List<DynamicReportBO>();
            List<DynamicReportBO> dynamicRearranged = new List<DynamicReportBO>();
            DynamicReportDA reportDA = new DynamicReportDA();
            //|| (x.ColumnName != "QuotationId") || (x.ColumnName != "CompanyId")
            dynamicReports = reportDA.GetFieldsFromTable(segment);
            foreach (var item in dynamicReports.ToList())
            {
                if (item.DataType == "bit")
                {
                    dynamicReports.Remove(item);
                }
                else if (item.ColumnName.Equals("EmpId"))
                {
                    dynamicReports.Remove(item);
                }
                else if (item.ColumnName.Equals("EmpPassword"))
                {
                    dynamicReports.Remove(item);
                }
                else if (item.ColumnName.Equals("CostCenterId"))
                {
                    dynamicReports.Remove(item);
                }
                else if (item.ColumnName.Equals("AttendanceDeviceEmpId"))
                {
                    dynamicReports.Remove(item);
                }
                else if (item.ColumnName.Equals("DonorId"))
                {
                    dynamicReports.Remove(item);
                }
                else if (item.ColumnName.Equals("NodeId"))
                {
                    dynamicReports.Remove(item);
                }
                else if (item.ColumnName.Equals("Title"))
                {
                    dynamicReports.Remove(item);
                }else if (item.ColumnName.Equals("FirstName"))
                {
                    dynamicReports.Remove(item);
                }else if (item.ColumnName.Equals("LastName"))
                {
                    dynamicReports.Remove(item);
                }else if (item.ColumnName.Equals("ActivityCode"))
                {
                    dynamicReports.Remove(item);
                }
            }

            foreach (var item in dynamicReports)
            {
                string theString = item.ColumnName;

                StringBuilder builder = new StringBuilder();
                foreach (char c in theString)
                {
                    if (Char.IsUpper(c) && builder.Length > 0) builder.Append(' ');
                    builder.Append(c);
                }
                theString = builder.ToString();

                DynamicReportBO bO = new DynamicReportBO();
                bO.ColumnName = theString;
                bO.DataType = item.DataType;
                dynamicRearranged.Add(bO);
            }

            foreach (var item in dynamicRearranged.ToList())
            {
                if (item.ColumnName.Equals("Emp Code"))
                {
                    item.ColumnName = "Employee Code";
                }
                else if (item.ColumnName.Equals("Display Name"))
                {
                    item.ColumnName = "Employee Name";
                }else if (item.ColumnName.Equals("Emp Type Id"))
                {
                    item.ColumnName = "Employee Type";
                }else if (item.ColumnName.Equals("Probable P F Eligibility Date"))
                {
                    item.ColumnName = "Probable Provident Fund Eligibility Date";
                }else if (item.ColumnName.Equals("P F Eligibility Date"))
                {
                    item.ColumnName = "Provident Fund Eligibility Date";
                }
                else if (item.ColumnName.Equals("P F Eligibility Date"))
                {
                    item.ColumnName = "Provident Fund Eligibility Date";
                }else if (item.ColumnName.Equals("P F Terminate Date"))
                {
                    item.ColumnName = "Provident Fund Terminate Date";
                }else if (item.ColumnName.Equals("P Issue Place"))
                {
                    item.ColumnName = "Passport Issue Place";
                }else if (item.ColumnName.Equals("P Issue Date"))
                {
                    item.ColumnName = "Passport Issue Date";
                }else if (item.ColumnName.Equals("P Expire Date"))
                {
                    item.ColumnName = "Passport Expire Date";
                }else if (item.ColumnName.Equals("Emp Date Of Birth"))
                {
                    item.ColumnName = "Date Of Birth";
                }
                else if ((item.ColumnName.Contains("Id")) && (item.ColumnName != "National Id"))
                {
                    item.ColumnName = item.ColumnName.Substring(0, item.ColumnName.Length - 2);
                }
            }

            //dynamicReports = dynamicReports.Where(x => (x.ColumnName != "DisplaySequence")).ToList();


            ArrayList arr = new ArrayList();
            arr.Add(new { RawColumns = dynamicReports, Rearranged = dynamicRearranged });

            return arr;
        }

        private static string JoinTable(List<string> columns, bool hasCreatedBy, bool hasLastModifieddBy)
        {
            string sqlComm = string.Empty;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("PayrollReportingTo", "PayrollReportingTo");

            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i] == "EMP.DepartmentId")
                {
                    sqlComm += " LEFT JOIN PayrollDepartment PD ON  PD.DepartmentId = EMP.DepartmentId ";
                }
                else if (columns[i] == "EMP.EmpTypeId")
                {
                    sqlComm += " LEFT JOIN PayrollEmpType PET  ON  PET.TypeId = EMP.EmpTypeId ";
                }
                else if (columns[i] == "EMP.DesignationId")
                {
                    sqlComm += " LEFT JOIN PayrollDesignation PDES  ON  PDES.DesignationId = EMP.DesignationId ";
                }
                else if (columns[i] == "EMP.BankName")
                {
                    sqlComm += " LEFT OUTER JOIN PayrollEmpBankInfo PEBI  ON  EMP.EmpId = PEBI.EmpI ";
                    sqlComm += " LEFT OUTER JOIN CommonBank CB ON  PEBI.BankId = CB.BankId ";
                }
                else if (columns[i] == "EMP.GradeId")
                {
                    sqlComm += " LEFT JOIN PayrollEmpGrade PEG	            ON  EMP.GradeId = PEG.GradeId ";
                }
                else if (columns[i] == "EMP.CountryId")
                {
                    sqlComm += " LEFT JOIN CommonCountries CCOU	            ON  EMP.CountryId = CCOU.CountryId ";
                }
                else if (columns[i] == "EMP.DistrictId")
                {
                    sqlComm += " LEFT JOIN PayrollEmpDistrict PDIS	            ON  EMP.DistrictId = PDIS.DistrictId ";
                }
                else if (columns[i] == "EMP.DivisionId")
                {
                    sqlComm += " LEFT JOIN PayrollEmpDivision PDIV	            ON  EMP.DivisionId = PDIV.DivisionId ";
                }
                else if (columns[i] == "EMP.ThanaId")
                {
                    sqlComm += " LEFT JOIN PayrollEmpThana PTH	            ON  EMP.ThanaId = PTH.ThanaId ";
                }
                else if (columns[i] == "EMP.WorkStationId")
                {
                    sqlComm += " LEFT OUTER JOIN PayrollEmpWorkStation PEWS	            ON  EMP.WorkStationId = PEWS.WorkStationId ";
                }
                else if (columns[i] == "EMP.EmployeeStatusId")
                {
                    sqlComm += " LEFT JOIN PayrollEmployeeStatus PESt		ON emp.EmployeeStatusId = PESt.EmployeeStatusId ";
                }
                else if (columns[i] == "EMP.GlCompanyId")
                {
                    sqlComm += " LEFT JOIN GLCompany GLC				ON emp.GlCompanyId = GLC.CompanyId ";
                }
                else if (columns[i] == "EMP.PayrollCurrencyId")
                {
                    sqlComm += " LEFT JOIN CommonCurrency CCUR		ON emp.PayrollCurrencyId = CCUR.CurrencyId ";
                }
                else if (columns[i] == "EMP.CostCenterId" || columns[i] == "EMP.WorkingCostCenterId")
                {
                    sqlComm += " LEFT JOIN CommonCostCenter CCC   ON EMP.WorkingCostCenterId = CCC.CostCenterId ";
                }
                else if (columns[i] == "EMP.RepotingTo")
                {
                    if (commonSetupBO.SetupValue == "2")
                        sqlComm += " LEFT JOIN 	PayrollEmployee peOne            ON  EMP.RepotingTo = peOne.EmpId ";
                    else
                        sqlComm += " LEFT JOIN PayrollDesignation PDES1 ON EMP.RepotingTo = PDES1.DesignationId ";

                }
                else if (columns[i] == "EMP.RepotingTo2")
                {
                    if (commonSetupBO.SetupValue == "2")
                        sqlComm += " LEFT JOIN   PayrollEmployee peTwo          ON  EMP.RepotingTo2 = peTwo.EmpId ";
                    else
                        sqlComm += " LEFT JOIN PayrollDesignation PDES2	  ON EMP.RepotingTo2 = PDES2.DesignationId  ";
                }
                else
                {
                    sqlComm += "";
                }

                if (hasCreatedBy)
                {
                    sqlComm += " LEFT JOIN SecurityUserInformation ON SecurityUserInformation.UserInfoId = EMP.CreatedBy ";
                    hasCreatedBy = false;
                }
                if (hasLastModifieddBy)
                {
                    sqlComm += " LEFT JOIN SecurityUserInformation SUI ON SUI.UserInfoId = EMP.LastModifiedBy ";
                    hasLastModifieddBy = false;
                }

                //sqlComm += " LEFT JOIN CommonDocuments CD  ON  EMP.EmpId = CD.OwnerId ";

            }


            return sqlComm;
        }
    }
}