using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Data.HMCommon
{
    public class DocumentsDA : BaseService
    {
        public Boolean UpdateDocumentsInfo(DocumentsBO docBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateDocumentsInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@DocumentId", DbType.Int32, docBO.DocumentId);
                    dbSmartAspects.AddInParameter(command, "@OwnerId", DbType.Int32, docBO.OwnerId);
                    dbSmartAspects.AddInParameter(command, "@DocumentCategory", DbType.String, docBO.DocumentCategory);
                    dbSmartAspects.AddInParameter(command, "@DocumentType", DbType.String, docBO.DocumentType);
                    dbSmartAspects.AddInParameter(command, "@Extention", DbType.String, docBO.Extention);
                    dbSmartAspects.AddInParameter(command, "@Name", DbType.String, docBO.Name);
                    dbSmartAspects.AddInParameter(command, "@Path", DbType.String, docBO.Path);
                    dbSmartAspects.AddInParameter(command, "@LastModified", DbType.Int32, docBO.LastModified);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean SaveDocumentsInfo(List<DocumentsBO> docList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveDocumentsInfo_SP"))
                {
                    foreach (DocumentsBO docBO in docList)
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
        public List<DocumentsBO> GetDocumentsInfoByDocCategoryAndOwnerId(string Category, long ownerId)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDocumentsInfoByDocCategoryAndOwnerId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@DocumentCategory", DbType.String, Category);
                    dbSmartAspects.AddInParameter(cmd, "@OwnerId", DbType.Int64, ownerId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                DocumentsBO docs = new DocumentsBO();

                                docs.DocumentId = Convert.ToInt32(reader["DocumentId"]);
                                docs.OwnerId = Convert.ToInt32(reader["OwnerId"]);
                                docs.DocumentType = reader["DocumentType"].ToString();
                                docs.DocumentCategory = reader["DocumentCategory"].ToString();
                                docs.Name = reader["Name"].ToString();
                                docs.Path = reader["Path"].ToString();
                                docs.Extention = reader["Extention"].ToString();
                                docs.ImageUrl = reader["ImageUrl"].ToString();
                                docList.Add(docs);
                            }
                        }
                    }
                }
            }
            return docList;
        }
        public List<DocumentsBO> GetLastOneDocumentsInfoByDocCategoryAndOwnerId(string Category, long ownerId)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLastOneDocumentsInfoByDocCategoryAndOwnerId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@DocumentCategory", DbType.String, Category);
                    dbSmartAspects.AddInParameter(cmd, "@OwnerId", DbType.Int64, ownerId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                DocumentsBO docs = new DocumentsBO();

                                docs.DocumentId = Convert.ToInt32(reader["DocumentId"]);
                                docs.OwnerId = Convert.ToInt32(reader["OwnerId"]);
                                docs.DocumentType = reader["DocumentType"].ToString();
                                docs.DocumentCategory = reader["DocumentCategory"].ToString();
                                docs.Name = reader["Name"].ToString();
                                docs.Path = reader["Path"].ToString();
                                docs.Extention = reader["Extention"].ToString();
                                docs.ImageUrl = reader["ImageUrl"].ToString();
                                docList.Add(docs);
                            }
                        }
                    }
                }
            }
            return docList;
        }
        public List<DocumentsBO> GetLeaveEmployeeByDate(DateTime date)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLeaveEmployeeByDate_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@Date", DbType.DateTime, date);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                DocumentsBO docs = new DocumentsBO();

                                docs.EmployeeName = reader["EmployeeName"].ToString();
                                docs.DocumentId = reader["DocumentId"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["DocumentId"]);
                                docs.OwnerId = reader["OwnerId"] == System.DBNull.Value ? default(int) : Convert.ToInt32(reader["OwnerId"]);
                                docs.DocumentType = reader["DocumentType"].ToString();
                                docs.DocumentCategory = reader["DocumentCategory"].ToString();
                                docs.Name = reader["Name"].ToString();
                                docs.Path = reader["Path"].ToString();
                                docs.Extention = reader["Extention"].ToString();
                                docs.ImageUrl = reader["ImageUrl"].ToString();
                                docList.Add(docs);
                            }
                        }
                    }
                }
            }
            return docList;
        }
        public DocumentsBO GetDocumentsInfoByDocumentId(int documentId)
        {
            DocumentsBO docBO = new DocumentsBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDocumentsInfoByDocumentId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@DocumentId", DbType.Int32, documentId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                docBO.DocumentId = Convert.ToInt32(reader["DocumentId"]);
                                docBO.OwnerId = Convert.ToInt32(reader["OwnerId"]);
                                docBO.Name = reader["Name"].ToString();
                                docBO.Path = reader["Path"].ToString();
                                docBO.DocumentCategory = reader["DocumentCategory"].ToString();
                                docBO.DocumentType = reader["DocumentType"].ToString();
                                docBO.Extention = reader["Extention"].ToString();
                                docBO.Instruction = reader["Instruction"].ToString();
                            }
                        }
                    }
                }
            }
            return docBO;
        }
        public List<DocumentsBO> GetDocumentsByUserTypeAndUserId(string DocumentCategory, long OwnerId)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();
            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDocumentInfoByCategoryAndOwnerId_SP"))
            {
                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                dbSmartAspects.AddInParameter(cmd, "@OwnerId", DbType.Int64, OwnerId);
                dbSmartAspects.AddInParameter(cmd, "@DocumentCategory", DbType.String, DocumentCategory);
                using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            DocumentsBO docs = new DocumentsBO();
                            docs.DocumentId = Convert.ToInt32(reader["DocumentId"]);
                            docs.OwnerId = Convert.ToInt32(reader["OwnerId"]);
                            docs.DocumentType = reader["DocumentType"].ToString();
                            docs.DocumentCategory = reader["DocumentCategory"].ToString();
                            docs.Name = reader["Name"].ToString();
                            docs.Path = reader["Path"].ToString();
                            docs.Extention = reader["Extention"].ToString();
                            docList.Add(docs);
                        }
                    }
                }
            }
            return docList;
        }
        public List<DocumentsBO> GetDocumentsInfoByOwnerId(int ownerId)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDocumentsInfoByOwnerId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@OwnerId", DbType.Int32, ownerId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                DocumentsBO docs = new DocumentsBO();

                                docs.DocumentId = Convert.ToInt32(reader["DocumentId"]);
                                docs.OwnerId = Convert.ToInt32(reader["OwnerId"]);
                                docs.DocumentType = reader["DocumentType"].ToString();
                                docs.DocumentCategory = reader["DocumentCategory"].ToString();
                                docs.Name = reader["Name"].ToString();
                                docs.Path = reader["Path"].ToString();
                                docs.Extention = reader["Extention"].ToString();
                                docList.Add(docs);
                            }
                        }
                    }
                }
            }
            return docList;
        }
        public Boolean UpdateDocumentsInfoByOwnerId(DocumentsBO docBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateDocumentsInfoByOwnerId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@DocumentId", DbType.Int32, docBO.DocumentId);
                    dbSmartAspects.AddInParameter(command, "@OwnerId", DbType.Int32, docBO.OwnerId);
                    dbSmartAspects.AddInParameter(command, "@DocumentCategory", DbType.String, docBO.DocumentCategory);
                    dbSmartAspects.AddInParameter(command, "@DocumentType", DbType.String, docBO.DocumentType);
                    dbSmartAspects.AddInParameter(command, "@Extention", DbType.String, docBO.Extention);
                    dbSmartAspects.AddInParameter(command, "@Name", DbType.String, docBO.Name);
                    dbSmartAspects.AddInParameter(command, "@Path", DbType.String, docBO.Path);
                    dbSmartAspects.AddInParameter(command, "@LastModified", DbType.Int32, docBO.LastModified);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool UpdateDocumentsInfo(List<DocumentsBO> list)
        {
            Boolean status = false;
            int success = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                {
                    dbSmartAspects.AddInParameter(commandEducation, "@TableName", DbType.String, "CommonDocuments");
                    dbSmartAspects.AddInParameter(commandEducation, "@TablePKField", DbType.String, "OwnerId");
                    dbSmartAspects.AddInParameter(commandEducation, "@TablePKId", DbType.String, list[0].OwnerId);
                    success = dbSmartAspects.ExecuteNonQuery(commandEducation);
                }

                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateDocumentsInfoByDelete_SP"))
                {
                    foreach (DocumentsBO docBO in list)
                    {
                        command.Parameters.Clear();
                        dbSmartAspects.AddInParameter(command, "@OwnerId", DbType.Int32, docBO.OwnerId);
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
        public bool DeleteDocumentsByDocumentTypeNOwnerId(List<DocumentsBO> documentBoList, int guestId)
        {
            Boolean status = false;
            int success = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                foreach (DocumentsBO doc in documentBoList)
                {
                    if (doc.DocumentType == "Image")
                    {
                        using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("DeleteDocumentsByDocumentTypeNOwnerId_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandEducation, "@TableName", DbType.String, "CommonDocuments");
                            dbSmartAspects.AddInParameter(commandEducation, "@TablePKField", DbType.String, "OwnerId");
                            dbSmartAspects.AddInParameter(commandEducation, "@TablePKId", DbType.String, guestId);
                            dbSmartAspects.AddInParameter(commandEducation, "@DocumentType", DbType.String, documentBoList[0].DocumentType);

                            success = dbSmartAspects.ExecuteNonQuery(commandEducation);
                        }
                    }
                }
            }
            return status;
        }
        public bool DeleteDocumentsByDocumentId(long documentId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("DeleteDocumentsByDocumentId_SP"))
                {
                    dbSmartAspects.AddInParameter(commandEducation, "@DocumentId", DbType.Int64, documentId);

                    status = dbSmartAspects.ExecuteNonQuery(commandEducation) > 0;

                }
                return status;
            }

        }
        public Boolean DeleteDocument(string deletedDocumentId)
        {
            bool status = false;
            if (!string.IsNullOrEmpty(deletedDocumentId))
            {

                string[] documentId = deletedDocumentId.Split(',');

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        try
                        {
                            using (DbCommand commandDelete = dbSmartAspects.GetStoredProcCommand("DeleteDocumentsInfoByDocId_SP"))
                            {
                                for (int i = 0; i < documentId.Count() && int.Parse(documentId[i])!=0; i++)
                                {
                                    commandDelete.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDelete, "@DocumentId", DbType.Int32, int.Parse(documentId[i]));

                                    status = dbSmartAspects.ExecuteNonQuery(commandDelete, transction) > 0 ? true : false;

                                    if (!status)
                                        break;
                                }
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
                        catch (Exception ex)
                        {
                            transction.Rollback();
                            throw ex;
                        }
                    }
                    conn.Close();
                }

            }

            return status;
        }
        public bool DeleteDocumentsByDocumentIdListString(string documentIdList)
        {
            string[] documentId = documentIdList.Split(',');

            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandDelete = dbSmartAspects.GetStoredProcCommand("DeleteDocumentsByDocumentId_SP"))
                        {
                            for (int i = 0; i < documentId.Count() && documentId[i] != "0"; i++)
                            {
                                commandDelete.Parameters.Clear();

                                dbSmartAspects.AddInParameter(commandDelete, "@DocumentId", DbType.Int64, documentId[i]);

                                status = dbSmartAspects.ExecuteNonQuery(commandDelete, transction) > 0 ? true : false;

                                if (!status)
                                    break;
                            }
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
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;
                    }
                }
                conn.Close();
            }
            return status;
        }
        public Boolean UpdateRandomDocumentOwnwerIdWithOwnerId(long ownerId, long randomOwnerId)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandDocument = dbSmartAspects.GetStoredProcCommand("UpdateRandomDocumentOwnwerIdWithOwnerId"))
                {
                    commandDocument.Parameters.Clear();
                    dbSmartAspects.AddInParameter(commandDocument, "@OwnerId", DbType.Int64, ownerId);
                    dbSmartAspects.AddInParameter(commandDocument, "@RandomId", DbType.Int64, randomOwnerId);
                    status = dbSmartAspects.ExecuteNonQuery(commandDocument) > 0 ? true : false;
                }
            }

            return status;
        }
        public List<DocumentsBO> GetPaymentInstuctionInfo()
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPaymentInstuctionInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                DocumentsBO docs = new DocumentsBO();

                                docs.DocumentId = Convert.ToInt32(reader["DocumentId"]);
                                docs.OwnerId = Convert.ToInt32(reader["OwnerId"]);
                                docs.DocumentType = reader["DocumentType"].ToString();
                                docs.DocumentCategory = reader["DocumentCategory"].ToString();
                                docs.Name = reader["Name"].ToString();
                                docs.Path = reader["Path"].ToString();
                                docs.Extention = reader["Extention"].ToString();
                                docs.Instruction = reader["Instruction"].ToString();
                                docList.Add(docs);
                            }
                        }
                    }
                }
            }
            return docList;
        }
        public bool SavePaymentInstructionInfo(int DocumentId, int OwnerId, string ImagePath, string Instruction, int CreatedBy)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("PaymentInstruction_Save_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@DocumentId", DbType.Int32, DocumentId);
                    dbSmartAspects.AddInParameter(command, "@OwnerId", DbType.Int32, OwnerId);
                    dbSmartAspects.AddInParameter(command, "@Instruction", DbType.String, Instruction);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, CreatedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool UpdatePaymentInstructionInfo(int DocumentId, int OwnerId, string ImagePath, string Instruction, int CreatedBy)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("PaymentInstruction_Update_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@DocumentId", DbType.Int32, DocumentId);
                    dbSmartAspects.AddInParameter(command, "@OwnerId", DbType.Int32, OwnerId);
                    dbSmartAspects.AddInParameter(command, "@Instruction", DbType.String, Instruction);
                    dbSmartAspects.AddInParameter(command, "@UpdatedBy", DbType.Int32, CreatedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool DeletePaymentInstructionInfo(int DocumentId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("PaymentInstruction_Delete_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@DocumentId", DbType.Int32, DocumentId);
                    
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
    }
}
