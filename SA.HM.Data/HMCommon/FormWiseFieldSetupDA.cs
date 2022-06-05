using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using HotelManagement.Entity.Security;
using HotelManagement.Entity.HMCommon;
using System.Data.SqlClient;

namespace HotelManagement.Data.HMCommon
{
    public class FormWiseFieldSetupDA : BaseService
    {
        public List<MenuLinksBO> GetAllFormIdAndName()
        {
            List<MenuLinksBO> FormList = new List<MenuLinksBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFormInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                MenuLinksBO Form = new MenuLinksBO();
                                Form.MenuLinksId = Convert.ToInt32(reader["MenuLinksId"]);
                                Form.PageName = reader["PageName"].ToString();
                                Form.PageId = reader["PageId"].ToString();
                                FormList.Add(Form);
                            }
                        }
                    }
                }
            }

            return FormList;
        }

        public List<FormWiseFieldSetupBO> GetFieldsByMenuLinkID(int formId)
        {
            List<FormWiseFieldSetupBO> formWiseFields = new List<FormWiseFieldSetupBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFieldsByFormId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@formID", DbType.Int32, formId);
                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "FormFields");
                    DataTable Table = SaleServiceDS.Tables["FormFields"];
                    formWiseFields = Table.AsEnumerable().Select(r => new FormWiseFieldSetupBO
                    {
                        Id = r.Field<Int32>("Id"),
                        PageId = r.Field<Int32>("PageId"),
                        FieldId = r.Field<string>("FieldId"),
                        FieldName = r.Field<string>("FieldName"),
                        IsMandatory = r.Field<bool>("IsMandatory")

                    }).ToList();

                }


            }
            return formWiseFields;
        }

        public Boolean UpdateIsMandatoryFields(DataTable dt, int CreatedBy )
        {
            bool status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateIsMandatoryFields_SP"))
                    {
                        SqlParameter p = new SqlParameter("MandatoryFieldsTable", dt);
                        p.SqlDbType = SqlDbType.Structured;
                        cmd.Parameters.Add(p);

                        SqlParameter c = new SqlParameter("LastModifiedBy", CreatedBy);
                        c.SqlDbType = SqlDbType.Int;
                        cmd.Parameters.Add(c);

                        status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public List<MenuLinksBO> GetAllMenuLinksBasedOnFieldSetup()
        {
            List<MenuLinksBO> menuLinks = new List<MenuLinksBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllMenuLinksBasedOnFieldSetup_SP"))
                {
                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "MenuLinks");
                    DataTable Table = SaleServiceDS.Tables["MenuLinks"];
                    menuLinks = Table.AsEnumerable().Select(r => new MenuLinksBO
                    {
                        MenuLinksId = r.Field<Int64>("MenuLinksId"),
                        ModuleId = r.Field<Int32>("ModuleId"),
                        PageId = r.Field<string>("PageId"),
                        PageName = r.Field<string>("PageName"),
                        PageDisplayCaption = r.Field<string>("PageDisplayCaption"),
                        PageExtension = r.Field<string>("PageExtension"),
                        PagePath = r.Field<string>("PagePath"),
                        PageType = r.Field<string>("PageType"),
                        ActiveStat = r.Field<bool>("ActiveStat")

                    }).ToList();

                }


            }
            return menuLinks;
        }

        public bool SaveFormWiseField(FormWiseFieldSetupBO FormWiseField)
        {
            bool retVal = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveFormWiseFieldSetup_SP"))
                {
                    dbSmartAspects.AddInParameter(commandMaster, "@PageId", DbType.Int32, FormWiseField.PageId);
                    dbSmartAspects.AddInParameter(commandMaster, "@FieldId", DbType.String, FormWiseField.FieldId);
                    dbSmartAspects.AddInParameter(commandMaster, "@FieldName", DbType.String, FormWiseField.FieldName);
                    dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, FormWiseField.CreatedBy);
                    dbSmartAspects.ExecuteNonQuery(commandMaster);
                    retVal = true;
                }
            }
            return retVal;
        }

        public List<FormWiseFieldSetupBO> GetFormWiseFieldSetupBySearchCriteria(int pageId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<FormWiseFieldSetupBO> FieldList = new List<FormWiseFieldSetupBO>();
            totalRecords = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFormWiseFieldSetupInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PageId", DbType.Int32, pageId);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                FormWiseFieldSetupBO FieldBO = new FormWiseFieldSetupBO();
                                FieldBO.Id = Int32.Parse(reader["Id"].ToString());
                                FieldBO.PageId = Int32.Parse(reader["PageId"].ToString());
                                FieldBO.FieldId = reader["FieldId"].ToString();
                                FieldBO.FieldName = reader["FieldName"].ToString();
                                FieldList.Add(FieldBO);
                            }
                        }
                    }
                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                }
            }
            return FieldList;
        }

        public FormWiseFieldSetupBO GetFormWiseFieldInfoById(int Id)
        {
            FormWiseFieldSetupBO FieldBO = new FormWiseFieldSetupBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFormWiseFieldInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, Id);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                FieldBO.Id = Convert.ToInt32(reader["Id"]);
                                FieldBO.PageId = Convert.ToInt32(reader["PageId"]);
                                FieldBO.FieldName = reader["FieldName"].ToString();
                                FieldBO.FieldId = reader["FieldId"].ToString();
                            }
                        }
                    }
                }
            }
            return FieldBO;
        }

        public bool UpdateFormWiseFieldInfo(FormWiseFieldSetupBO FormWiseField)
        {
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateFormWiseFieldInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(commandMaster, "@Id", DbType.Int32, FormWiseField.Id);
                    dbSmartAspects.AddInParameter(commandMaster, "@PageId", DbType.Int32, FormWiseField.PageId);
                    dbSmartAspects.AddInParameter(commandMaster, "@FieldId", DbType.String, FormWiseField.FieldId);
                    dbSmartAspects.AddInParameter(commandMaster, "@FieldName", DbType.String, FormWiseField.FieldName);
                    dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, FormWiseField.LastModifiedBy);
                    dbSmartAspects.ExecuteNonQuery(commandMaster);
                    retVal = true;
                }
            }
            return retVal;
        }

        // activity log details related DA

        public bool UpdateActivitySaveFields(List<FormWiseFieldSetupBO> formWiseFields, int userId)
        {
            bool retVal = false;
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        if (formWiseFields.Count > 0)
                        {
                            using (DbCommand commandAdd = dbSmartAspects.GetStoredProcCommand("UpdateActivitySaveFields_SP"))
                            {
                                foreach (FormWiseFieldSetupBO item in formWiseFields)
                                {
                                    commandAdd.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandAdd, "@Id", DbType.Int64, item.Id);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsSaveActivity", DbType.Boolean, item.IsSaveActivity);
                                    dbSmartAspects.AddInParameter(commandAdd, "@LastModifiedBy", DbType.Int32, userId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandAdd, transction);
                                }
                            }
                        }
                        if (status > 0)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
                return retVal;
        }

        public bool UpdateFormWiseFieldActivityLogSetup(FormWiseFieldSetupBO FormWiseField)
        {
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateFormWiseFieldActivityLogSetup_SP"))
                {
                    dbSmartAspects.AddInParameter(commandMaster, "@Id", DbType.Int32, FormWiseField.Id);
                    dbSmartAspects.AddInParameter(commandMaster, "@PageIdStr", DbType.String, FormWiseField.PageIdStr);
                    dbSmartAspects.AddInParameter(commandMaster, "@FieldName", DbType.String, FormWiseField.FieldName);
                    dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, FormWiseField.LastModifiedBy);
                    dbSmartAspects.ExecuteNonQuery(commandMaster);
                    retVal = true;
                }
            }
            return retVal;
        }
        public List<FormWiseFieldSetupBO> GetFieldsByPageId(string pageId)
        {
            List<FormWiseFieldSetupBO> formWiseFields = new List<FormWiseFieldSetupBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFieldsByPageId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PageId", DbType.String, pageId);
                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "FormFields");
                    DataTable Table = SaleServiceDS.Tables["FormFields"];
                    formWiseFields = Table.AsEnumerable().Select(r => new FormWiseFieldSetupBO
                    {
                        Id = r.Field<Int64>("Id"),
                        PageIdStr = r.Field<string>("PageId"),
                        FieldName = r.Field<string>("FieldName"),
                        IsSaveActivity = r.Field<bool>("IsSaveActivity")
                    }).ToList();

                }


            }
            return formWiseFields;
        }

        public List<MenuLinksBO> GetAllMenuLinksBasedOnActivityLogSetup()
        {
            List<MenuLinksBO> menuLinks = new List<MenuLinksBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllMenuLinksBasedOnActivityLogSetup_SP"))
                {
                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "MenuLinks");
                    DataTable Table = SaleServiceDS.Tables["MenuLinks"];
                    menuLinks = Table.AsEnumerable().Select(r => new MenuLinksBO
                    {
                        MenuLinksId = r.Field<Int64>("MenuLinksId"),
                        ModuleId = r.Field<Int32>("ModuleId"),
                        PageId = r.Field<string>("PageId"),
                        PageName = r.Field<string>("PageName"),
                        PageDisplayCaption = r.Field<string>("PageDisplayCaption"),
                        PageExtension = r.Field<string>("PageExtension"),
                        PagePath = r.Field<string>("PagePath"),
                        PageType = r.Field<string>("PageType"),
                        ActiveStat = r.Field<bool>("ActiveStat")

                    }).ToList();

                }


            }
            return menuLinks;
        }

        public FormWiseFieldSetupBO GetActivityLogFormWiseFieldInfoById(int Id)
        {
            FormWiseFieldSetupBO FieldBO = new FormWiseFieldSetupBO(); 

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActivityLogFormWiseFieldInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, Id);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                FieldBO.Id = Convert.ToInt32(reader["Id"]);
                                FieldBO.PageIdStr = reader["PageId"].ToString();
                                FieldBO.FieldName = reader["FieldName"].ToString();
                                //FieldBO.FieldId = reader["FieldId"].ToString();
                            }
                        }
                    }
                }
            }
            return FieldBO;
        }

        public bool SaveFormWiseFieldForActivityLog(FormWiseFieldSetupBO FormWiseField)
        {
            bool retVal = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveFormWiseFieldForActivityLog_SP"))
                {
                    dbSmartAspects.AddInParameter(commandMaster, "@PageIdStr", DbType.String, FormWiseField.PageIdStr);
                    dbSmartAspects.AddInParameter(commandMaster, "@FieldName", DbType.String, FormWiseField.FieldName);
                    dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, FormWiseField.CreatedBy);

                    retVal = dbSmartAspects.ExecuteNonQuery(commandMaster) > 0 ? true : false;
                }
            }
            return retVal;
        }

        public List<FormWiseFieldSetupBO> GetFormWiseFieldsetupForActivityLog(string pageId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<FormWiseFieldSetupBO> FieldList = new List<FormWiseFieldSetupBO>();
            totalRecords = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFormWiseFieldsetupForActivityLog_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PageId", DbType.String, pageId);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                FormWiseFieldSetupBO FieldBO = new FormWiseFieldSetupBO();
                                FieldBO.Id = Int32.Parse(reader["Id"].ToString());
                                FieldBO.PageIdStr = reader["PageId"].ToString();
                                FieldBO.PageName = reader["PageName"].ToString();
                                FieldBO.IsSaveActivity = Convert.ToBoolean(reader["IsSaveActivity"]);
                                FieldBO.FieldName = reader["FieldName"].ToString();
                                FieldList.Add(FieldBO);
                            }
                        }
                    }
                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                }
            }
            return FieldList;
        }
    }
}
