using HotelManagement.Entity.DocumentManagement;
using HotelManagement.Entity.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.DocumentManagement
{
    public class DocumentsForDocManagementDA : BaseService
    {
        public List<DocumentsForDocManagementBO> GetDocumentsByUserTypeAndUserId(string DocumentCategory, long OwnerId)
        {
            List<DocumentsForDocManagementBO> docList = new List<DocumentsForDocManagementBO>();
            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDMDocumentInfoByCategoryAndOwnerId_SP"))
            {
                dbSmartAspects.AddInParameter(cmd, "@OwnerId", DbType.Int64, OwnerId);
                dbSmartAspects.AddInParameter(cmd, "@DocumentCategory", DbType.String, DocumentCategory);
                using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            DocumentsForDocManagementBO docs = new DocumentsForDocManagementBO();

                            docs.DocumentId = Convert.ToInt32(reader["DocumentId"]);
                            docs.OwnerId = Convert.ToInt32(reader["OwnerId"]);
                            docs.DocumentType = reader["DocumentType"].ToString();
                            docs.DocumentCategory = reader["DocumentCategory"].ToString();
                            docs.Name = reader["Name"].ToString();
                            docs.DocumentName = reader["DocumentName"].ToString();

                            docs.Path = reader["Path"].ToString();
                            docs.Extention = reader["Extention"].ToString();
                            docList.Add(docs);
                        }
                    }
                }
            }
            return docList;
        }
        public List<DocumentsForDocManagementBO> GetDocumentListWithIcon(List<DocumentsForDocManagementBO> docList)
        {
            for (int i = 0; i < docList.Count; i++)
            {
                if (docList[i].Extention.ToLower() == ".doc" || docList[i].Extention.ToLower() == ".docx")
                {
                    docList[i].IconImage = "/Images/FileType/doc.png";
                }
                else if (docList[i].Extention.ToLower() == ".flv")
                {
                    docList[i].IconImage = "/Images/FileType/flv.png";
                }
                else if (docList[i].Extention.ToLower() == ".html")
                {
                    docList[i].IconImage = "/Images/FileType/html.png";
                }
                else if (docList[i].Extention.ToLower() == ".pdf")
                {
                    docList[i].IconImage = "/Images/FileType/pdf.png";
                }
                else if (docList[i].Extention.ToLower() == ".xls")
                {
                    docList[i].IconImage = "/Images/FileType/xls.png";
                }
                else if (docList[i].Extention.ToLower() == ".xlsx")
                {
                    docList[i].IconImage = "/Images/FileType/xlsx.png";
                }
                else if (docList[i].Extention.ToLower() == ".zip")
                {
                    docList[i].IconImage = "/Images/FileType/zip.png";
                }
                else if (docList[i].Extention.ToLower() == ".xml")
                {
                    docList[i].IconImage = "/Images/FileType/xml.png";
                }
                else
                {
                    docList[i].IconImage = "/Images/FileType/Unknown.png";
                }
            }
            return docList;
        }
        public Boolean SaveDocumentsInfo(List<DocumentsForDocManagementBO> docList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveDMDocumentsInfo_SP"))
                {
                    foreach (DocumentsForDocManagementBO docBO in docList)
                    {
                        command.Parameters.Clear();
                        dbSmartAspects.AddInParameter(command, "@OwnerId", DbType.Int64, docBO.OwnerId);
                        dbSmartAspects.AddInParameter(command, "@DocumentCategory", DbType.String, docBO.DocumentCategory);
                        dbSmartAspects.AddInParameter(command, "@DocumentType", DbType.String, docBO.DocumentType);
                        dbSmartAspects.AddInParameter(command, "@Extention", DbType.String, docBO.Extention);
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, docBO.Name);
                        dbSmartAspects.AddInParameter(command, "@Path", DbType.String, docBO.Path);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, docBO.CreatedBy);
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            return status;
        }
        public bool SaveOrUpdateDocument(DMDocumentBO document, string empId, out long id)
        {
            Boolean status = false;
            bool retVal = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateDocument_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, document.Id);
                            dbSmartAspects.AddInParameter(command, "@DocumentName", DbType.String, document.DocumentName);
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, document.Description);
                            dbSmartAspects.AddInParameter(command, "@EmailReminderType", DbType.String, document.EmailReminderType);
                            dbSmartAspects.AddInParameter(command, "@EmailReminderDate", DbType.DateTime, document.EmailReminderDate);
                            dbSmartAspects.AddInParameter(command, "@EmailReminderTime", DbType.Time, document.EmailReminderTime);
                            dbSmartAspects.AddInParameter(command, "@CallToAction", DbType.String, document.CallToAction);
                            dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, document.CreatedBy);
                            dbSmartAspects.AddInParameter(command, "@AssignType", DbType.String, document.AssignType);
                            if (document.EmpDepartment!=0)
                                dbSmartAspects.AddInParameter(command, "@EmpDepartment", DbType.Int32, document.EmpDepartment);
                            else
                                dbSmartAspects.AddInParameter(command, "@EmpDepartment", DbType.Int32, DBNull.Value);
                            dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));

                            status = (dbSmartAspects.ExecuteNonQuery(command, transction) > 0);

                            id = Convert.ToInt64(command.Parameters["@OutId"].Value);
                        }
                        if (status && empId != "")
                        {
                            using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveUpdateDocumentAssignedEmployee_SP"))
                            {
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@DocumentId", DbType.Int64, id);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@EmployeeList", DbType.String, empId);

                                status = (dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction) > 0);
                            }
                        }
                        if (status)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                            status = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public Boolean UpdateUploadedDocumentsInformationByOwnerId(long ownerId, string docName, long randomId)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandDocument = dbSmartAspects.GetStoredProcCommand("UpdateUploadedDMDocumentsInfoByOwnerId_SP"))
                {
                    commandDocument.Parameters.Clear();
                    dbSmartAspects.AddInParameter(commandDocument, "@OwnerId", DbType.Int64, ownerId);
                    dbSmartAspects.AddInParameter(commandDocument, "@RandomId", DbType.Int64, randomId);
                    dbSmartAspects.AddInParameter(commandDocument, "@DocumentName", DbType.String, docName);
                    status = dbSmartAspects.ExecuteNonQuery(commandDocument) > 0 ? true : false;
                }
            }

            return status;
        }
        public List<DMDocumentBO> GetDocumentBySearchCriteria(string documentName, string assignedEmp, DateTime fromDate, DateTime toDate, int recordPerPage, int pageIndex, out int totalRecords)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<DMDocumentBO> documentList = new List<DMDocumentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAssignDocumentForPaging_SP"))
                {
                    if (!string.IsNullOrEmpty(documentName))
                        dbSmartAspects.AddInParameter(cmd, "@DocumentName", DbType.String, documentName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DocumentName", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);

                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    if (!string.IsNullOrEmpty(assignedEmp))
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeList", DbType.String, assignedEmp);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeList", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                DMDocumentBO document = new DMDocumentBO();
                                document.Id = Convert.ToInt64(reader["Id"]);
                                document.DocumentName = reader["DocumentName"].ToString();
                                document.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                document.Description = reader["Description"].ToString();
                                documentList.Add(document);
                            }
                        }
                    }
                    //            DataSet dataSet = new DataSet();

                    //dbSmartAspects.LoadDataSet(cmd, dataSet, "Document");
                    //DataTable Table = dataSet.Tables["Document"];

                    //documentList = Table.AsEnumerable().Select(r => new SMDocument
                    //{
                    //    Id = r.Field<long>("Id"),
                    //    DocumentName = r.Field<string>("DocumentName"),
                    //    DueDate = r.Field<DateTime>("DueDate"),
                    //    DueTime = r.Field<DateTime>("DueTime"),
                    //    DocumentType = r.Field<string>("DocumentType"),
                    //    Description = r.Field<string>("Description"),
                    //    AssignTo = r.Field<int>("AssignTo"),
                    //    EmailReminderDate = r.Field<DateTime?>("EmailReminderDate"),
                    //    EmailReminderTime = r.Field<DateTime?>("EmailReminderTime"),
                    //    CreatedBy = r.Field<int>("CreatedBy"),
                    //    IsCompleted = r.Field<bool>("IsCompleted")

                    //}).ToList();
                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                }
            }


            return documentList;
        }
        public DMDocumentBO GetDocumentById(long id)
        {
            DMDocumentBO document = new DMDocumentBO();
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            string query = string.Format("SELECT * FROM DMDocumentMaster WHERE Id = {0}", id);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                document.Id = Convert.ToInt64(reader["Id"]);
                                document.DocumentName = reader["DocumentName"].ToString();
                                if (reader["Description"] != DBNull.Value)
                                {
                                    document.Description = reader["Description"].ToString();
                                }
                                document.EmailReminderType = reader["EmailReminderType"].ToString();
                                if (reader["EmailReminderDate"] != DBNull.Value)
                                    document.EmailReminderDate = Convert.ToDateTime(reader["EmailReminderDate"]);
                                if (reader["EmailReminderTime"] != DBNull.Value)
                                    document.EmailReminderTime = Convert.ToDateTime(currentDate + " " + reader["EmailReminderTime"]);
                                if (reader["CallToAction"] != DBNull.Value)
                                {
                                    document.CallToAction = reader["CallToAction"].ToString();
                                }
                                if (reader["AssignType"] != DBNull.Value)
                                {
                                    document.AssignType = reader["AssignType"].ToString();
                                }
                                if (reader["EmpDepartment"] != DBNull.Value)
                                {
                                    document.EmpDepartment = Convert.ToInt32(reader["EmpDepartment"]);
                                }
                                document.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);

                            }
                        }
                    }

                }
            }
            return document;
        }
        public List<EmployeeBO> GetDocumentAssignedEmployeeById(long id)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<EmployeeBO> documentList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAssignedEmployeeByDocumentId"))
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
        public List<DMDocumentBO> GetAssignedDocumentByEmpId(int empId)
        {
            List<DMDocumentBO> documentList = new List<DMDocumentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDocumentsByEmpId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.String, empId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                DMDocumentBO documentBO = new DMDocumentBO();
                                documentBO.Id = Convert.ToInt32(reader["Id"]);
                                documentBO.DocumentName = reader["DocumentName"].ToString();
                                documentBO.CallToAction = reader["CallToAction"].ToString();
                                documentBO.Description = reader["Description"].ToString();

                                documentList.Add(documentBO);
                            }
                        }
                    }

                }
            }
            return documentList;
        }
        public List<DMDocumentBO> GetDocumentForReport(string documentName, string assignedEmp, DateTime fromDate, DateTime toDate)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<DMDocumentBO> documentList = new List<DMDocumentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAssignDocumentForReport_SP"))
                {
                    if (!string.IsNullOrEmpty(documentName))
                        dbSmartAspects.AddInParameter(cmd, "@DocumentName", DbType.String, documentName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DocumentName", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);

                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    if (!string.IsNullOrEmpty(assignedEmp))
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeList", DbType.String, assignedEmp);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeList", DbType.String, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                DMDocumentBO document = new DMDocumentBO();
                                document.Id = Convert.ToInt64(reader["Id"]);
                                document.DocumentName = reader["DocumentName"].ToString();
                                document.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                document.Description = reader["Description"].ToString();
                                document.EmployeeNameList = reader["EmployeeNameList"].ToString();
                                document.CreatedByName = reader["CreatedByName"].ToString();
                                documentList.Add(document);
                            }
                        }
                    }                    
                }
            }


            return documentList;
        }

    }
}
