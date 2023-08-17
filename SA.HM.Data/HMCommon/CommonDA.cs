using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace HotelManagement.Data.HMCommon
{
    public class CommonDA : BaseService
    {
        public List<T> GetAllTableRow<T>(string tableName, string tableFieldName = "",
                                            string tableFieldValue = "", bool checkIsActive = false,
                                                    bool checkIsDeleted = false) where T : class
        {
            List<T> tableDataList = Activator.CreateInstance<List<T>>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllTableRow_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TableName", DbType.String, tableName);
                    dbSmartAspects.AddInParameter(cmd, "@TableFieldName", DbType.String, tableFieldName);
                    dbSmartAspects.AddInParameter(cmd, "@TableFieldValue", DbType.String, tableFieldValue);
                    dbSmartAspects.AddInParameter(cmd, "@CheckIsActive", DbType.Boolean, checkIsActive);
                    dbSmartAspects.AddInParameter(cmd, "@CheckIsDeleted", DbType.Boolean, checkIsDeleted);

                    DataSet ds = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, ds, "TableData");
                    DataTable Table = ds.Tables["TableData"];

                    tableDataList = Table.AsEnumerable().Select(r => UtilityDA.ConvertDataRowToObjet<T>(r)
                    ).ToList();
                }
            }
            return tableDataList;
        }

        public T GetTableRow<T>(string tableName, string tableFieldName = "",
                                            string tableFieldValue = "", bool checkIsActive = false,
                                                    bool checkIsDeleted = false) where T : class
        {
            T tableData = Activator.CreateInstance<T>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllTableRow_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TableName", DbType.String, tableName);
                    dbSmartAspects.AddInParameter(cmd, "@TableFieldName", DbType.String, tableFieldName);
                    dbSmartAspects.AddInParameter(cmd, "@TableFieldValue", DbType.String, tableFieldValue);
                    dbSmartAspects.AddInParameter(cmd, "@CheckIsActive", DbType.Boolean, checkIsActive);
                    dbSmartAspects.AddInParameter(cmd, "@CheckIsDeleted", DbType.Boolean, checkIsDeleted);

                    DataSet ds = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, ds, "TableData");
                    DataTable Table = ds.Tables["TableData"];

                    tableData = Table.AsEnumerable().Select(r => UtilityDA.ConvertDataRowToObjet<T>(r)
                    ).FirstOrDefault();
                }
            }
            return tableData;
        }
        public string JoinTable(string replacedBy, string tableName, bool hasCreatedBy, bool hasLastModifieddBy)
        {
            string join = string.Empty;
            if (tableName == "PayrollEmployee")
            {
                if (replacedBy == "DepartmentId")
                {
                    join = " LEFT JOIN PayrollDepartment PD ON  PD.DepartmentId = PayrollEmployee.DepartmentId ";
                }
                else if (replacedBy == "EmpTypeId")
                {
                    join = " LEFT JOIN PayrollEmpType PET  ON  PET.TypeId = PayrollEmployee.EmpTypeId ";
                }
                else if (replacedBy == "DesignationId")
                {
                    join = " LEFT JOIN PayrollDesignation PDES  ON  PDES.DesignationId = PayrollEmployee.DesignationId ";
                }
                else if (replacedBy == "BankName")
                {
                    join = " LEFT OUTER JOIN PayrollEmpBankInfo PEBI  ON  EmpId = PEBI.EmpI ";
                    join = " LEFT OUTER JOIN CommonBank CB ON  PEBI.BankId = CB.BankId ";
                }
                else if (replacedBy == "GradeId")
                {
                    join = " LEFT JOIN PayrollEmpGrade PEG ON  PayrollEmployee.GradeId = PEG.GradeId ";
                }
                else if (replacedBy == "CountryId")
                {
                    join = " LEFT JOIN CommonCountries CCOU	            ON  PayrollEmployee.CountryId = CCOU.CountryId ";
                }
                else if (replacedBy == "DistrictId")
                {
                    join = " LEFT JOIN PayrollEmpDistrict PDIS	            ON  PayrollEmployee.DistrictId = PDIS.DistrictId ";
                }
                else if (replacedBy == "DivisionId")
                {
                    join = " LEFT JOIN PayrollEmpDivision PDIV	            ON  PayrollEmployee.DivisionId = PDIV.DivisionId ";
                }
                else if (replacedBy == "ThanaId")
                {
                    join = " LEFT JOIN PayrollEmpThana PTH	            ON  PayrollEmployee.ThanaId = PTH.ThanaId ";
                }
                else if (replacedBy == "WorkStationId")
                {
                    join = " LEFT OUTER JOIN PayrollEmpWorkStation PEWS	            ON  PayrollEmployee.WorkStationId = PEWS.WorkStationId ";
                }
                else if (replacedBy == "EmployeeStatusId")
                {
                    join = " LEFT JOIN PayrollEmployeeStatus PESt		ON PayrollEmployee.EmployeeStatusId = PESt.EmployeeStatusId ";
                }
                else if (replacedBy == "GlCompanyId")
                {
                    join = " LEFT JOIN GLCompany GLC				ON PayrollEmployee.GlCompanyId = GLC.CompanyId ";
                }
                else if (replacedBy == "PayrollCurrencyId")
                {
                    join = " LEFT JOIN CommonCurrency CCUR		ON PayrollEmployee.PayrollCurrencyId = CCUR.CurrencyId ";
                }
                else if (replacedBy == "CostCenterId" || replacedBy == "WorkingCostCenterId")
                {
                    join = " LEFT JOIN CommonCostCenter CCC   ON PayrollEmployee.WorkingCostCenterId = CCC.CostCenterId ";
                }
                else if (replacedBy == "RepotingTo")
                {
                    join = " LEFT JOIN PayrollDesignation PDES1	            ON  PayrollEmployee.RepotingTo = PDES1.GradeId ";
                }
                else if (replacedBy == "RepotingTo2")
                {
                    join = " LEFT JOIN PayrollDesignation PDES2	            ON  PayrollEmployee.RepotingTo = PDES2.GradeId ";
                }
                else
                {
                    join = "";
                }
                if (hasCreatedBy)
                {
                    join = " LEFT JOIN SecurityUserInformation ON SecurityUserInformation.UserInfoId = EMP.CreatedBy ";

                }
                if (hasLastModifieddBy)
                {
                    join = " LEFT JOIN SecurityUserInformation SUI ON SUI.UserInfoId = EMP.LastModifiedBy ";

                }
            }
            return join;
        }

       

        //public bool SendSMSbySSLGateway(int templateId, string subject, string body, string EmpList)
        //{
        //    bool status = false;
        //        SMS sMS;
        //       HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
        //       HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();


        //     commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendSMS", "SendSMSConfiguration");
        //    string mainString = commonSetupBO.Description;
        //    List<int> empIds = new List<int>();
        //    foreach (var s in EmpList.Split(','))
        //    {
        //        int num;
        //        if (int.TryParse(s, out num))
        //            empIds.Add(num);
        //    }
        //    List<EmployeeBO> employeeBOs = new List<EmployeeBO>();
        //    EmployeeBO bo = new EmployeeBO();
        //    EmployeeDA da = new EmployeeDA();
        //    if (empIds.Count > 0)
        //    {
        //        foreach (var item in empIds)
        //        {
        //            bo = da.GetEmployeeInfoById(item);
        //            if (bo != null)
        //            {
        //                employeeBOs.Add(bo);
        //            }
        //        }
        //    }

        //}
        public string GenerateModifiedBody(long templateId, EmployeeBO employeeBOs)
        {
            TemplateInfoDA DA = new TemplateInfoDA();
            TemplateInformationBO templateBo = new TemplateInformationBO();
            templateBo = DA.GetTemplateInformationById(templateId);
            List<TemplateInformationDetailBO> detailsBOs = new List<TemplateInformationDetailBO>();
            detailsBOs = DA.GetTemplateInformationDetail(templateId);
            string modifiedBody = string.Empty;
            List<TemporaryReplaceBO> replaceBOList = new List<TemporaryReplaceBO>();



            if (detailsBOs.Count > 0)
            {
                foreach (var item2 in detailsBOs)
                {
                    TemporaryReplaceBO replaceBO = new TemporaryReplaceBO();
                    var pk = "";
                    if (item2.TableName == "PayrollEmployee")
                    {
                        pk = "EmpId";
                    }
                    else if (item2.TableName == "SMContactInformation")
                    {
                        pk = "Id";
                    }
                    else if (item2.TableName == "HotelGuestCompany")
                    {
                        pk = "CompanyId";
                    }
                    else if (item2.TableName == "PMSupplier")
                    {
                        pk = "SupplierId";
                    }
                    replaceBO.Id = employeeBOs.EmpId;
                    replaceBO.BodyText = item2.BodyText;
                    replaceBO.ReplacedBy = item2.ReplacedBy;
                    replaceBO.ReplaceByValue = DA.GetReplaceByValue(employeeBOs.EmpId, item2.ReplacedBy, item2.TableName, pk);

                    replaceBOList.Add(replaceBO);
                }
                StringBuilder builder = new StringBuilder(templateBo.Body);
                modifiedBody = "";
                foreach (var item in replaceBOList)
                {
                    if (templateBo.Body.Contains(item.BodyText))
                    {
                        //builder.Replace(item.BodyText, item.ReplaceByValue);
                        //modifiedBody = builder.ToString();
                        //templateBo.Body = templateBo.Body.Replace(item.BodyText, item.ReplaceByValue);
                        //modifiedBody = templateBo.Body.Replace(item.BodyText, item.ReplaceByValue);
                        var regex = new Regex(@"(?<![\w])" + item.BodyText + @"(?![\w])", RegexOptions.IgnoreCase);
                        templateBo.Body = regex.Replace(templateBo.Body, item.ReplaceByValue);
                    }
                }
                modifiedBody = templateBo.Body;
            }
            else
            {
                modifiedBody = templateBo.Body;
            }
            return modifiedBody;
        }

        public Boolean AutoCompanyBillGenerationProcess(string billType, Int64 billId, int userInfoId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("AutoCompanyBillGenerationProcess_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, billType);
                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int64, billId);
                    dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int64, userInfoId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public Boolean BillCalculationProcessForCompanyDiscountAndCashIncentive_SP(Int64 billId, int userInfoId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("BillCalculationProcessForCompanyDiscountAndCashIncentive_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int64, billId);
                    dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int64, userInfoId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
    }
}
