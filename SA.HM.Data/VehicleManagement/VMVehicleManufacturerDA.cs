using HotelManagement.Entity.VehicleManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.VehicleManagement
{
    public class VMVehicleManufacturerDA:BaseService
    {
        public Boolean SaveUpdateManufacturerInformation(VMManufacturerBO VMManufacturerBO, out long OutId)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateVehicleManufacturerInformation_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, VMManufacturerBO.Id);

                        if (VMManufacturerBO.BrandName != "")
                            dbSmartAspects.AddInParameter(command, "@BrandName", DbType.String, VMManufacturerBO.BrandName);
                        else
                            dbSmartAspects.AddInParameter(command, "@BrandName", DbType.String, DBNull.Value);

                        if (VMManufacturerBO.Description != "")
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, VMManufacturerBO.Description);
                        else
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@Status", DbType.String, VMManufacturerBO.Status);

                        if (VMManufacturerBO.Id == 0)
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, VMManufacturerBO.CreatedBy);
                        else
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, VMManufacturerBO.LastModifiedBy);

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
        public List<VMManufacturerBO> GetManufacturerInformationPagination(string brandName, bool Status, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<VMManufacturerBO> ManufacturerInformationList = new List<VMManufacturerBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetVehicleManufacturerInformationForPaging_SP"))
                    {

                        if ((brandName) != "")
                            dbSmartAspects.AddInParameter(cmd, "@BrandName", DbType.String, brandName);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@BrandName", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.Boolean, Status);


                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    VMManufacturerBO ManufacturerInformation = new VMManufacturerBO();

                                    ManufacturerInformation.Id = Convert.ToInt64(reader["Id"]);
                                    ManufacturerInformation.BrandName = (reader["BrandName"].ToString());

                                    ManufacturerInformationList.Add(ManufacturerInformation);
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
            return ManufacturerInformationList;
        }
        public VMManufacturerBO GetManufacturerInformationBOById(long id)
        {
            VMManufacturerBO ManufacturerInformation = new VMManufacturerBO();
            string query = string.Format("select * from VMManufacturer  where Id = {0}", id);

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

                                    ManufacturerInformation.Id = Convert.ToInt64(reader["Id"]);
                                    ManufacturerInformation.BrandName = (reader["BrandName"].ToString());
                                    ManufacturerInformation.Description = (reader["Description"].ToString());
                                    if (reader["Status"] != DBNull.Value)
                                    {
                                        ManufacturerInformation.Status = Convert.ToBoolean(reader["Status"]);
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

            return ManufacturerInformation;

        }
        public bool DeleteManufacturer(long Id)
        {
            bool status = false;
            try
            {
                string query = string.Format("DELETE FROM VMManufacturer WHERE Id = {0}", Id);

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
        public List<VMManufacturerBO> GetManufacturerInfoForDDL()
        {
            List<VMManufacturerBO> vMManufacturers = new List<VMManufacturerBO>();
            string query = string.Format("SELECT Id, BrandName FROM VMManufacturer  WHERE Status = '1'");
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
                                VMManufacturerBO vMManufacturer = new VMManufacturerBO()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    BrandName = reader["BrandName"].ToString()
                                };
                                vMManufacturers.Add(vMManufacturer);
                            }
                        }
                    }
                }
            }

            return vMManufacturers;
        }
    }
}
