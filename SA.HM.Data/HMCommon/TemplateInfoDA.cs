using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.HMCommon
{
    public class TemplateInfoDA : BaseService
    {
        public TemplateInformationBO GetTemplateInformationById(long id)
        {
            TemplateInformationBO templateInformationBO = new TemplateInformationBO();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTemplateInformationById_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, id);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    templateInformationBO.Id = Convert.ToInt64(reader["Id"]);
                                    //templateInformationBO.TypeId = Convert.ToInt32(reader["TypeId"]);
                                    templateInformationBO.Type = Convert.ToString(reader["Type"]);
                                    //templateInformationBO.TemplateForId = Convert.ToInt32(reader["TemplateForId"]);
                                    templateInformationBO.TemplateFor = Convert.ToString(reader["TemplateFor"]);

                                    templateInformationBO.Name = reader["Name"].ToString();
                                    templateInformationBO.Subject = reader["Subject"].ToString();
                                    templateInformationBO.Body = reader["Body"].ToString();


                                    //if (reader["CreatedDate"] != DBNull.Value)
                                    //{
                                    //    templateInformationBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);

                                    //}
                                    //if (reader["LastModifiedDate"] != DBNull.Value)
                                    //{
                                    //    templateInformationBO.LastModifiedDate = Convert.ToDateTime(reader["LastModifiedDate"]);

                                    //}

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return templateInformationBO;
        }
        public List<TemplateEmailBO> GetTemplateInfoForEmployeeById(long id)
        {
            List<TemplateEmailBO> templateEmailBOs = new List<TemplateEmailBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTemplateInfoForEmployeeById_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, id);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    TemplateEmailBO emailBO = new TemplateEmailBO();
                                    emailBO.Id = Convert.ToInt64(reader["Id"]);
                                    emailBO.Name = Convert.ToString(reader["Name"]);
                                    emailBO.TemplateBody = Convert.ToString(reader["TemplateBody"]);
                                    emailBO.DisplayName = Convert.ToString(reader["DisplayName"]);
                                    if (reader["CreatedDate"] != DBNull.Value)
                                    {
                                        emailBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                    }
                                    templateEmailBOs.Add(emailBO);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return templateEmailBOs;
        }
        public List<TemplateInformationBO> GetTemplateInformationForDdl()
        {
            List<TemplateInformationBO> templateInfoDetailsList = new List<TemplateInformationBO>();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTemplateInformationForDdl_SP"))
                    {

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    TemplateInformationBO templateInformationBO = new TemplateInformationBO();

                                    templateInformationBO.Id = Convert.ToInt64(reader["Id"]);
                                    //templateInformationBO.TypeId = Convert.ToInt32(reader["TypeId"]);
                                    templateInformationBO.Type = Convert.ToString(reader["Type"]);
                                    //templateInformationBO.TemplateForId = Convert.ToInt32(reader["TemplateForId"]);
                                    templateInformationBO.TemplateFor = Convert.ToString(reader["TemplateFor"]);

                                    templateInformationBO.Name = reader["Name"].ToString();
                                    templateInformationBO.Subject = reader["Subject"].ToString();
                                    templateInformationBO.Body = reader["Body"].ToString();


                                    templateInfoDetailsList.Add(templateInformationBO);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return templateInfoDetailsList;
        }
        public List<TemplateInformationBO> GetTemplateInformationByType(string type)
        {
            List<TemplateInformationBO> templateInfoDetailsList = new List<TemplateInformationBO>();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTemplateInformationByType_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Type", DbType.String, type);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    TemplateInformationBO templateInformationBO = new TemplateInformationBO();

                                    templateInformationBO.Id = Convert.ToInt64(reader["Id"]);
                                    //templateInformationBO.TypeId = Convert.ToInt32(reader["TypeId"]);
                                    templateInformationBO.Type = Convert.ToString(reader["Type"]);
                                    //templateInformationBO.TemplateForId = Convert.ToInt32(reader["TemplateForId"]);
                                    templateInformationBO.TemplateFor = Convert.ToString(reader["TemplateFor"]);

                                    templateInformationBO.Name = reader["Name"].ToString();
                                    templateInformationBO.Subject = reader["Subject"].ToString();
                                    templateInformationBO.Body = reader["Body"].ToString();


                                    templateInfoDetailsList.Add(templateInformationBO);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return templateInfoDetailsList;
        }

        public Boolean SaveTemplateInformation(TemplateInformationBO templateInformationBO, out long OutId)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveTemplateInformation_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, templateInformationBO.Id);


                        if (!string.IsNullOrEmpty(templateInformationBO.Name))
                            dbSmartAspects.AddInParameter(command, "@Name", DbType.String, templateInformationBO.Name);
                        else
                            dbSmartAspects.AddInParameter(command, "@Name", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(templateInformationBO.Type))
                            dbSmartAspects.AddInParameter(command, "@Type", DbType.String, templateInformationBO.Type);
                        else
                            dbSmartAspects.AddInParameter(command, "@Type", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(templateInformationBO.TemplateFor))
                            dbSmartAspects.AddInParameter(command, "@TemplateFor", DbType.String, templateInformationBO.TemplateFor);
                        else
                            dbSmartAspects.AddInParameter(command, "@TemplateFor", DbType.String, DBNull.Value);



                        if (!string.IsNullOrEmpty(templateInformationBO.Subject))
                            dbSmartAspects.AddInParameter(command, "@Subject", DbType.String, templateInformationBO.Subject);
                        else
                            dbSmartAspects.AddInParameter(command, "@Subject", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(templateInformationBO.Body))
                            dbSmartAspects.AddInParameter(command, "@Body", DbType.String, templateInformationBO.Body);
                        else
                            dbSmartAspects.AddInParameter(command, "@Body", DbType.String, DBNull.Value);



                        if ((templateInformationBO.CreatedBy) != 0)
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, templateInformationBO.CreatedBy);
                        else
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, DBNull.Value);

                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int64, sizeof(Int64));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        OutId = Convert.ToInt32(command.Parameters["@OutId"].Value);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public Boolean SaveTemplateEmail(TemplateEmailBO bO, string empList, string type, int templateId, out long OutId)
        {
            Boolean status = false;


            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {

                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveTemplateEmail_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, bO.Id);
                            dbSmartAspects.AddInParameter(command, "@TemplateId", DbType.Int64, templateId);

                            if (!string.IsNullOrEmpty(bO.TemplateBody))
                                dbSmartAspects.AddInParameter(command, "@EmailBody", DbType.String, bO.TemplateBody);
                            else
                                dbSmartAspects.AddInParameter(command, "@EmailBody", DbType.String, DBNull.Value);
                            if (!string.IsNullOrEmpty(bO.TemplateType))
                                dbSmartAspects.AddInParameter(command, "@TemplateType", DbType.String, bO.TemplateType);
                            else
                                dbSmartAspects.AddInParameter(command, "@TemplateType", DbType.String, DBNull.Value);
                            
                            if (!string.IsNullOrEmpty(bO.AssignType))
                                dbSmartAspects.AddInParameter(command, "@AssignType", DbType.String, bO.AssignType);
                            else
                                dbSmartAspects.AddInParameter(command, "@AssignType", DbType.String, DBNull.Value);

                            if ((bO.CreatedBy) != 0)
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, bO.CreatedBy);
                            else
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                            OutId = Convert.ToInt64(command.Parameters["@OutId"].Value);

                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;
                    }
                    if (status)
                    {

                        if (status && empList != "")
                        {
                            using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveEmailDetails_SP"))
                            {
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@TemplateEmailId", DbType.Int64, OutId);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@EmployeeList", DbType.String, empList);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@TemplateType", DbType.String, type);

                                status = (dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction) > 0);
                            }
                        }
                        if (status)
                        {
                            transction.Commit();
                        }
                        else
                        {
                            transction.Rollback();
                            status = false;
                        }
                    }
                    else
                    {
                        transction.Rollback();
                        status = false;
                    }
                }
            }
            return status;
        }
        public TemplateEmailBO GetTemplateEmailById(long id, string type)
        {
            TemplateEmailBO emailBO = new TemplateEmailBO();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTemplateEmailById_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, id);
                        dbSmartAspects.AddInParameter(cmd, "@Type", DbType.String, type);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    emailBO.Id = Convert.ToInt64(reader["Id"]);
                                    emailBO.TemplateId = Convert.ToInt64(reader["TemplateId"]);
                                    emailBO.TemplateBody = Convert.ToString(reader["TemplateBody"]);
                                    if (reader["CreatedDate"] != DBNull.Value)
                                    {
                                        emailBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                    }
                                    if (reader["LastModifiedDate"] != DBNull.Value)
                                    {
                                        emailBO.LastModifiedDate = Convert.ToDateTime(reader["LastModifiedDate"]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return emailBO;
        }
        public string GetReplaceByValue(long id, string replacedBy, string tableName, string pk)
        {
            //TemporaryReplaceBO detailsBO = new TemporaryReplaceBO();
            string replaceByValue = "";
            string tempData = "";
            string rawData = "";
            string join = "";
            CommonDA commonDA = new CommonDA();
            rawData = replacedBy;
            tempData = replacedBy;
            if ((tempData.Contains("Id")) && (tempData != "National Id"))
            {
                tempData = tempData.Substring(0, tempData.Length - 2);
            }

            if (replacedBy.Contains("Date") && tableName != "CommonDataForTemplate")
            {
                replacedBy = " dbo.FnGenerateDateFormat( " + replacedBy + ", 'DateStamp') " + replacedBy;
            }
            else if (replacedBy == "DepartmentId")
            {
                replacedBy = " PD.[Name] AS Department ";
            }
            else if (replacedBy == "EmpTypeId")
            {
                replacedBy = " PET.[Name] EmpType ";
            }
            else if (replacedBy == "DesignationId")
            {
                replacedBy = " PDES.[Name] Designation ";
            }
            else if (replacedBy == "BankName")
            {
                replacedBy = " CB.BankName ";
            }
            else if (replacedBy == "GradeId")
            {
                replacedBy = " PEG.Name Grade ";
            }
            else if (replacedBy == "CountryId")
            {
                replacedBy = " CCOU.CountryName Country ";
            }
            else if (replacedBy == "DistrictId")
            {
                replacedBy = " PDIS.DistrictName District ";
            }
            else if (replacedBy == "DivisionId")
            {
                replacedBy = " PDIV.DivisionName Division ";
            }
            else if (replacedBy == "ThanaId")
            {
                replacedBy = " PTH.ThanaName Thana ";
            }
            else if (replacedBy == "WorkStationId")
            {
                replacedBy = " PEWS.WorkStationName WorkStation ";
            }
            else if (replacedBy == "EmployeeStatusId")
            {
                replacedBy = " PESt.EmployeeStatus EmployeeStatus ";
            }
            else if (replacedBy == "GlCompanyId")
            {
                replacedBy = " GLC.Name GlCompany ";
            }
            else if (replacedBy == "PayrollCurrencyId")
            {
                replacedBy = " CCUR.CurrencyName PayrollCurrency ";
            }
            else if (replacedBy == "CostCenterId" || replacedBy == "WorkingCostCenterId")
            {
                replacedBy = " CCC.CostCenter CostCenter ";
            }
            else if (replacedBy == "RepotingTo")
            {
                replacedBy = " PDES1.Name ReportingTo ";
            }
            else if (replacedBy == "RepotingTo2")
            {
                replacedBy = " PDES2.Name ReportingTo2 ";
            }
            join = commonDA.JoinTable(rawData, tableName, false, false);
            string query = string.Empty;

            if (pk != "")
            {
                query = string.Format(@"SELECT {0}
                                            FROM {1} {4}
                                            WHERE {2} = {3}
                                         ", replacedBy, tableName, pk, id, join);
            }
            else
            {
                if (replacedBy == "CurrentDate(DD/MM/YYYY)")
                {
                    tempData = "CurrentDate";
                    query = string.Format(@"SELECT dbo.FnGenerateDateFormat(GETDATE(), 'DateStamp') CurrentDate ");
                }
                else if (replacedBy == "CurrentDate(MMM-DD-YYYY)")
                {
                    tempData = "CurrentDate";
                    query = string.Format(@"SELECT dbo.FnGenerateDateFormat(GETDATE(), 'MMM-DD-YYYY') CurrentDate ");
                }
                else if (replacedBy == "CurrentMonth")
                {
                    tempData = "CurrentMonth";
                    query = string.Format(@"SELECT DATENAME(month, getdate()) AS CurrentMonth ");
                }
                else if (replacedBy == "CurrentYear")
                {
                    tempData = "CurrentYear";
                    query = string.Format(@"SELECT DATENAME(year, getdate()) AS CurrentYear ");
                }
            }

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    if (!string.IsNullOrEmpty(query))
                    {
                        using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                        {
                            //dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, empId);
                            //dbSmartAspects.AddInParameter(cmd, "@BodyText", DbType.Int32, bodyText);
                            //dbSmartAspects.AddInParameter(cmd, "@ReplacedBy", DbType.Int32, replacedBy);
                            //dbSmartAspects.AddInParameter(cmd, "@TableName", DbType.Int32, tableName);
                            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                            {
                                if (reader != null)
                                {
                                    while (reader.Read())
                                    {
                                        replaceByValue = reader[tempData].ToString();
                                    }
                                }
                            }
                        }
                    }
                    
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return replaceByValue;
        }
        
        public Boolean SaveDetails(List<TemplateInformationDetailBO> newAdded, List<TemplateInformationDetailBO> delatedItems, long parentId)
        {
            bool status = false;
            bool status2 = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        if (newAdded.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveTemplateInformationDetails_SP"))
                            {
                                foreach (var item in newAdded)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@TemplateId", DbType.Int64, parentId);

                                    dbSmartAspects.AddInParameter(command, "@BodyText", DbType.String, item.BodyText);
                                    dbSmartAspects.AddInParameter(command, "@ReplacedBy", DbType.String, item.ReplacedBy);
                                    dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, item.TableName);


                                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                                }

                            }



                        }
                        if (delatedItems.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteTemplateInformationDetailItems_SP"))
                            {
                                foreach (var item in delatedItems)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, item.Id);
                                    dbSmartAspects.AddInParameter(command, "@TemplateId", DbType.Int64, item.TemplateId);

                                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;

                    }
                    if (status)
                    {
                        transction.Commit();
                    }
                    else
                    {
                        transction.Rollback();
                        status = false;
                    }
                }
            }
            return status;
        }
        public List<TemplateInformationDetailBO> GetTemplateInformationDetail(long parentId)
        {
            List<TemplateInformationDetailBO> templateInfoDetailsList = new List<TemplateInformationDetailBO>();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTemplateDetails_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@TemplateId", DbType.Int32, parentId);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    TemplateInformationDetailBO detailsBO = new TemplateInformationDetailBO();

                                    detailsBO.Id = Convert.ToInt64(reader["Id"]);
                                    detailsBO.TemplateId = Convert.ToInt64(reader["TemplateId"]);
                                    detailsBO.BodyText = reader["BodyText"].ToString();
                                    detailsBO.ReplacedBy = reader["ReplacedBy"].ToString();
                                    detailsBO.TableName = reader["TableName"].ToString();


                                    templateInfoDetailsList.Add(detailsBO);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return templateInfoDetailsList;
        }

        public List<TemplateInformationBO> GetTemplateInformationWithGrid(string name, string typeId, string templateForId, string subject, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<TemplateInformationBO> templateInformationBOList = new List<TemplateInformationBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTemplateInformationBySearchCriteriaForPaging_SP"))
                    {
                        if (!string.IsNullOrEmpty(name))
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, name);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(subject))
                            dbSmartAspects.AddInParameter(cmd, "@Subject", DbType.String, subject);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Subject", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(typeId))
                            dbSmartAspects.AddInParameter(cmd, "@Type", DbType.String, typeId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Type", DbType.String, DBNull.Value);
                        if (!string.IsNullOrEmpty(templateForId))
                            dbSmartAspects.AddInParameter(cmd, "@TemplateFor", DbType.String, templateForId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@TemplateFor", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    TemplateInformationBO templateInformationBO = new TemplateInformationBO();

                                    templateInformationBO.Id = Convert.ToInt64(reader["Id"]);
                                    //templateInformationBO.TypeId = Convert.ToInt32(reader["TypeId"]);
                                    templateInformationBO.Type = Convert.ToString(reader["Type"]);
                                    //templateInformationBO.TemplateForId = Convert.ToInt32(reader["TemplateForId"]);
                                    templateInformationBO.TemplateFor = Convert.ToString(reader["TemplateFor"]);

                                    templateInformationBO.Name = reader["Name"].ToString();
                                    templateInformationBO.Subject = reader["Subject"].ToString();
                                    templateInformationBO.Body = reader["Body"].ToString();

                                    templateInformationBOList.Add(templateInformationBO);
                                }
                            }
                        }
                        totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return templateInformationBOList;
        }

        public List<TemplateEmailBO> GetTemplateUsedWithGrid(string name, string type, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<TemplateEmailBO> templateInformationBOList = new List<TemplateEmailBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTemplateUseBySearchCriteriaForPaging_SP"))
                    {
                        if (!string.IsNullOrEmpty(name))
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, name);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(type))
                            dbSmartAspects.AddInParameter(cmd, "@Type", DbType.String, type);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Type", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    TemplateEmailBO templateInformationBO = new TemplateEmailBO();

                                    templateInformationBO.Id = Convert.ToInt64(reader["Id"]);
                                    if(reader["TemplateId"] != DBNull.Value)
                                    templateInformationBO.TemplateId = Convert.ToInt64(reader["TemplateId"]);

                                    templateInformationBO.TemplateType = Convert.ToString(reader["TemplateType"]);
                                    templateInformationBO.TemplateFor = Convert.ToString(reader["TemplateFor"]);

                                    templateInformationBO.TemplateBody = Convert.ToString(reader["TemplateBody"]);

                                    templateInformationBO.Name = reader["Name"].ToString();
                                    templateInformationBO.Subject = reader["Subject"].ToString();
                                    templateInformationBO.AssignType = reader["AssignType"].ToString();

                                    templateInformationBOList.Add(templateInformationBO);
                                }
                            }
                        }
                        totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return templateInformationBOList;
        }
        public TemplateEmailBO GetTemplateEmailInfoById(long id)
        {
            TemplateEmailBO templateInformationBO = new TemplateEmailBO();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTemplateEmailInfoById_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, id);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    

                                    templateInformationBO.Id = Convert.ToInt64(reader["Id"]);
                                    templateInformationBO.TemplateId = Convert.ToInt64(reader["TemplateId"]);

                                    templateInformationBO.TemplateType = Convert.ToString(reader["TemplateType"]);
                                    templateInformationBO.TemplateFor = Convert.ToString(reader["TemplateFor"]);

                                    templateInformationBO.TemplateBody = Convert.ToString(reader["TemplateBody"]);

                                    templateInformationBO.Name = reader["Name"].ToString();
                                    templateInformationBO.Subject = reader["Subject"].ToString();
                                    templateInformationBO.AssignType = reader["AssignType"].ToString();

                                    
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return templateInformationBO;
        }
        public List<EmployeeBO> GetTemplateUsedEmployeeByDetailId(long id)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<EmployeeBO> documentList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTemplateUsedEmployeeById"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, id);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmployeeBO Emp = new EmployeeBO();
                                Emp.EmpId = Convert.ToInt32(reader["EmpId"]);
                                Emp.DisplayName = reader["DisplayName"].ToString();
                                Emp.EmpCode = reader["EmpCode"].ToString();
                                Emp.Department = reader["Department"].ToString();
                                documentList.Add(Emp);
                            }
                        }
                    }
                }
            }

            return documentList;
        }
        public bool DeleteData(long Id)
        {
            bool status;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteTemplate_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, Id);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;
                    }
                    if (status)
                    {
                        transction.Commit();
                    }
                    else
                    {
                        transction.Rollback();
                    }
                }

            }
            return status;

        }
        public Boolean DeleteTemplatefromAssignedemployee(int Id)
        {
            bool status = false;

            string query = string.Format("Delete From CustomNotice WHERE Id = {0} ", Id);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                        {
                            status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
                        }

                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;
                    }
                    if (status)
                    {
                        transction.Commit();
                    }
                    else
                    {
                        transction.Rollback();
                    }
                }

            }




            return status;

        }
    }
}
