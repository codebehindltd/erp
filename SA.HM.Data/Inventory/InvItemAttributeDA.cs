using HotelManagement.Entity.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.Inventory
{
    public class InvItemAttributeDA : BaseService
    {
        public List<InvItemAttributeBO> GetInvItemAttributeByItemIdAndAttributeType(int itemId, string attributeType)
        {
            List<InvItemAttributeBO> headList = new List<InvItemAttributeBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemAttributeByItemIdAndAttributeType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    dbSmartAspects.AddInParameter(cmd, "@AttributeType", DbType.String, attributeType);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "UnitHead");
                    DataTable Table = ds.Tables["UnitHead"];

                    headList = Table.AsEnumerable().Select(r => new InvItemAttributeBO
                    {
                        Id = r.Field<long>("Id"),
                        Name = r.Field<string>("Name"),
                        Description = r.Field<string>("Description"),
                        SetupType = r.Field<string>("SetupType"),
                        Status = r.Field<bool?>("Status"),
                        CreatedBy = r.Field<int?>("CreatedBy"),
                        CreatedDate = r.Field<DateTime?>("CreatedDate")
                    }).ToList();
                }
            }
            return headList;
        }
        public List<InvItemAttributeBO> GetInvItemAttributeSetupBySearchCriteria(string name, string setupType, bool? status, int recordPerPage, int pageIndex, out int totalRecords)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<InvItemAttributeBO> SupportNCaseList = new List<InvItemAttributeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemAttributeSetupInfoForPaging_SP"))
                {
                    if (!string.IsNullOrEmpty(name))
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, name);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(setupType))
                        dbSmartAspects.AddInParameter(cmd, "@SetupType", DbType.String, setupType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SetupType", DbType.String, DBNull.Value);

                    if (status != null)
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.Boolean, status);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.Boolean, DBNull.Value);



                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemAttributeBO SupportNCase = new InvItemAttributeBO();
                                SupportNCase.Id = Convert.ToInt64(reader["Id"]);
                                SupportNCase.Name = reader["Name"].ToString();
                                SupportNCase.Description = reader["Description"].ToString();
                                SupportNCase.SetupType = reader["SetupType"].ToString();
                                SupportNCase.Status = Convert.ToBoolean(reader["Status"]);
                                SupportNCaseList.Add(SupportNCase);
                            }
                        }
                    }

                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                }
            }


            return SupportNCaseList;
        }
        public Boolean SaveOrUpdateSetupInfo(InvItemAttributeBO InvItemAttribute, out int id)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateInvItemAttributeSetup_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, InvItemAttribute.Id);
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, InvItemAttribute.Name);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, InvItemAttribute.Description);
                        dbSmartAspects.AddInParameter(command, "@SetupType", DbType.String, InvItemAttribute.SetupType);
                        dbSmartAspects.AddInParameter(command, "@Status", DbType.Boolean, InvItemAttribute.Status);                        

                        dbSmartAspects.AddInParameter(command, "@UserId", DbType.Int32, InvItemAttribute.CreatedBy); ;
                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));

                        status = (dbSmartAspects.ExecuteNonQuery(command) > 0);

                        id = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public InvItemAttributeBO GetInvItemAttributeSetupById(long id)
        {
            InvItemAttributeBO support = new InvItemAttributeBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemAttributeById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, id);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                support.Id = Convert.ToInt64(reader["Id"]);
                                support.SetupType = (reader["SetupType"].ToString());
                                support.Name = reader["Name"].ToString();
                                support.Status = Convert.ToBoolean(reader["Status"]);
                                support.Description = reader["Description"].ToString();
                            }
                        }
                    }
                }
            }
            return support;
        }
    }
}
