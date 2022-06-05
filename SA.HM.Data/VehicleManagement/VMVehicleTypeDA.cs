using HotelManagement.Entity.VehicleManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.VehicleManagement
{
    public class VMVehicleTypeDA :BaseService
    {
        public Boolean SaveUpdateTypeInformation(VMVehicleTypeBO vMVehicleTypeBO, out long OutId)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateVehicleTypeInformation_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, vMVehicleTypeBO.Id);

                        if (vMVehicleTypeBO.TypeName != "")
                            dbSmartAspects.AddInParameter(command, "@TypeName", DbType.String, vMVehicleTypeBO.TypeName);
                        else
                            dbSmartAspects.AddInParameter(command, "@TypeName", DbType.String, DBNull.Value);

                        if (vMVehicleTypeBO.Description != "")
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, vMVehicleTypeBO.Description);
                        else
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@Status", DbType.String, vMVehicleTypeBO.Status);

                        if (vMVehicleTypeBO.Id == 0)
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, vMVehicleTypeBO.CreatedBy);
                        else
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, vMVehicleTypeBO.LastModifiedBy);

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
        public List<VMVehicleTypeBO> GetTypeInformationPagination(string typeName, bool Status, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<VMVehicleTypeBO> TypeInformationList = new List<VMVehicleTypeBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetVehicleTypeInformationForPaging_SP"))
                    {

                        if ((typeName) != "")
                            dbSmartAspects.AddInParameter(cmd, "@TypeName", DbType.String, typeName);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@TypeName", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, Status);


                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    VMVehicleTypeBO TypeInformation = new VMVehicleTypeBO();

                                    TypeInformation.Id = Convert.ToInt64(reader["Id"]);
                                    TypeInformation.TypeName = (reader["TypeName"].ToString());

                                    TypeInformationList.Add(TypeInformation);
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
            return TypeInformationList;
        }
        public VMVehicleTypeBO GetTypeInformationBOById(long id)
        {
            VMVehicleTypeBO TypeInformation = new VMVehicleTypeBO();
            string query = string.Format("select * from VMVehicleType  where Id = {0}", id);

            try
            {
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

                                    TypeInformation.Id = Convert.ToInt64(reader["Id"]);
                                    TypeInformation.TypeName = (reader["TypeName"].ToString());
                                    TypeInformation.Description = (reader["Description"].ToString());
                                    if (reader["Status"] != DBNull.Value)
                                    {
                                        TypeInformation.Status = Convert.ToBoolean(reader["Status"]);
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

            return TypeInformation;

        }
        
        public bool DeleteType(long Id)
        {
            bool status = false;
            try
            {
                string query = string.Format("DELETE FROM VMVehicleType WHERE Id = {0}", Id);

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
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
        public List<VMVehicleTypeBO> GetTypeInfoForDDL()
        {
            List<VMVehicleTypeBO> vMTypes = new List<VMVehicleTypeBO>();
            string query = string.Format("SELECT Id, TypeName  from VMVehicleType  where Status = '1'");
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
                                VMVehicleTypeBO vMType = new VMVehicleTypeBO()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    TypeName = reader["TypeName"].ToString()
                                };
                                vMTypes.Add(vMType);
                            }
                        }
                    }
                }
            }

            return vMTypes;
        }
    }
}
