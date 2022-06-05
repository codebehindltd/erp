using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Common;
using System.Data;
using HotelManagement.Entity.Inventory;

namespace HotelManagement.Data.Inventory
{
    public class InvManufacturerDA : BaseService
    {
        public bool SaveManufacturerInfo(InvManufacturerBO PMManufacturerBO, out int tmpManInfoId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveManufacturerInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@Name", DbType.String, PMManufacturerBO.Name);
                    dbSmartAspects.AddInParameter(command, "@Code", DbType.String, PMManufacturerBO.Code);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, PMManufacturerBO.Remarks);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, PMManufacturerBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@ManufacturerId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpManInfoId = Convert.ToInt32(command.Parameters["@ManufacturerId"].Value);
                }
            }
            return status;
        }

        public bool UpdateManufactureInfo(InvManufacturerBO PMManufacturerBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateManufactureInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ManufacturerId", DbType.Int32, PMManufacturerBO.ManufacturerId);
                    dbSmartAspects.AddInParameter(command, "@Name", DbType.String, PMManufacturerBO.Name);
                    dbSmartAspects.AddInParameter(command, "@Code", DbType.String, PMManufacturerBO.Code);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, PMManufacturerBO.Remarks);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, PMManufacturerBO.LastModifiedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public bool CheckItemReference(int ManufacturerId)
        {
            int number = 0;
            string query = string.Format("SELECT COUNT(ItemId) AS number  from InvItem where ManufacturerId =  {0}", ManufacturerId);

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
                                    number = Convert.ToInt32(reader["number"]);
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

            if (number > 0)
            {
                return false;
            }
            else
                return true;
  
        }
        public List<InvManufacturerBO> GetManufacturerInfo()
        {
            List<InvManufacturerBO> manufacturerList = new List<InvManufacturerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvManufacturerInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvManufacturerBO manufacturerBO = new InvManufacturerBO();
                                manufacturerBO.ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]);
                                manufacturerBO.Name = reader["Name"].ToString();
                                manufacturerBO.Code = reader["Code"].ToString();
                                manufacturerBO.Remarks = reader["Remarks"].ToString();
                                manufacturerBO.CreatedDate = reader["CreatedDate"].ToString();
                                manufacturerList.Add(manufacturerBO);
                            }
                        }
                    }
                }
            }
            return manufacturerList;
        }

        public InvManufacturerBO GetManufacturerInfoById(int ManufacturerId)
        {
            InvManufacturerBO manufacturerBO = new InvManufacturerBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetManufacturerInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ManufacturerId", DbType.Int32, ManufacturerId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                manufacturerBO.ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]);
                                manufacturerBO.Name = reader["Name"].ToString();
                                manufacturerBO.Code = reader["Code"].ToString();
                                manufacturerBO.Remarks = reader["Remarks"].ToString();
                            }
                        }
                    }
                }
            }
            return manufacturerBO;
        }

        public List<InvManufacturerBO> GetManufacturerInfoBySearchCriteria(string Code, string Name)
        {
            string SearchCriteria = GenerateWhereCondition(Code, Name);
            List<InvManufacturerBO> manufacturerList = new List<InvManufacturerBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetManufacturerInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, SearchCriteria);

                    DataSet ManufacturerDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ManufacturerDS, "Manufacturer");

                    DataTable Table = ManufacturerDS.Tables["Manufacturer"];
                    manufacturerList = Table.AsEnumerable().Select(r => new InvManufacturerBO
                    {

                        ManufacturerId = r.Field<Int64>("ManufacturerId"),
                        Name = r.Field<string>("Name"),
                        Remarks = r.Field<string>("Remarks"),
                        Code = r.Field<string>("Code")

                    }).ToList();
                }
            }
            return manufacturerList;
        }

        public List<InvManufacturerBO> GetManufacturerInfoByServiceType(string SeviceType)
        {
            List<InvManufacturerBO> manufacturerList = new List<InvManufacturerBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetManufacturerInfoByServiceType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SeviceType", DbType.String, SeviceType);


                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvManufacturerBO manufacturerBO = new InvManufacturerBO();
                                manufacturerBO.ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]);
                                manufacturerBO.Name = reader["Name"].ToString();
                                manufacturerBO.Code = reader["Code"].ToString();
                                manufacturerList.Add(manufacturerBO);
                            }
                        }
                    }
                }
            }
            return manufacturerList;
        }

        private string GenerateWhereCondition(string Code, string Name)
        {
            string Where = string.Empty;

            if (!string.IsNullOrWhiteSpace(Code))
            {
                Where = "Code LIKE '%" + Code + "%'";
            }

            if (!string.IsNullOrWhiteSpace(Name))
            {
                if (string.IsNullOrEmpty(Where))
                {
                    Where = "Name LIKE '%" + Name + "%'";
                }
                else
                {
                    Where += " AND Name LIKE '%" + Name + "%'";
                }
            }

            if (!string.IsNullOrEmpty(Where))
                Where = " WHERE " + Where;

            return Where;
        }
    }
}
