using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Data.HotelManagement
{
    public class GuestReferenceDA :BaseService
    {
        public GuestReferenceBO GetGuestReferenceInfoById(int _referenceId)
        {

            GuestReferenceBO referenceBO = new GuestReferenceBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestReferenceInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReferenceId", DbType.Int32, _referenceId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                referenceBO.ReferenceId = Convert.ToInt32(reader["ReferenceId"]);
                                referenceBO.Name = reader["Name"].ToString();
                                referenceBO.SalesCommission = Convert.ToDecimal(reader["SalesCommission"].ToString());
                                referenceBO.Description = reader["Description"].ToString();
                                referenceBO.Email = reader["Email"].ToString();
                                referenceBO.Organization = reader["Organization"].ToString();
                                referenceBO.Designation = reader["Designation"].ToString();
                                referenceBO.CellNumber = reader["CellNumber"].ToString();
                                referenceBO.TelephoneNumber = reader["TelephoneNumber"].ToString();
                            }
                        }
                    }
                }
            }
            return referenceBO;
        }
        public List<GuestReferenceBO> GetGuestReferenceBySearchCriteria(string Name)
        {
            List<GuestReferenceBO> referenceList = new List<GuestReferenceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestReferenceBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, Name);


                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestReferenceBO referenceBO = new GuestReferenceBO();
                                referenceBO.ReferenceId = Convert.ToInt32(reader["ReferenceId"]);
                                referenceBO.Name = reader["Name"].ToString();
                                referenceBO.SalesCommission = Convert.ToDecimal(reader["SalesCommission"].ToString());
                                referenceBO.Description = reader["Description"].ToString();
                                referenceBO.Email = reader["Email"].ToString();
                                referenceBO.Organization = reader["Organization"].ToString();
                                referenceBO.Designation = reader["Designation"].ToString();
                                referenceBO.CellNumber = reader["CellNumber"].ToString();
                                referenceBO.TelephoneNumber = reader["TelephoneNumber"].ToString();
                                referenceList.Add(referenceBO);
                            }
                        }
                    }
                }
            }
            return referenceList;
        }
        public List<GuestReferenceBO> GetAllGuestRefference()
        {
            List<GuestReferenceBO> referenceList = new List<GuestReferenceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllGuestReference_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestReferenceBO referenceBO = new GuestReferenceBO();
                                referenceBO.ReferenceId = Convert.ToInt32(reader["ReferenceId"]);
                                referenceBO.Name = reader["Name"].ToString();
                                referenceBO.Description = reader["Description"].ToString();
                                referenceBO.SalesCommission = Convert.ToDecimal(reader["SalesCommission"].ToString());
                                referenceList.Add(referenceBO);
                            }
                        }
                    }
                }
            }
            return referenceList;
        }
        public bool SaveGuestReferenceInfo(GuestReferenceBO referenceBO, out int tmpReferenceId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGuestReferenceInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, referenceBO.Name);

                        dbSmartAspects.AddInParameter(command, "@Email", DbType.String, referenceBO.Email);
                        dbSmartAspects.AddInParameter(command, "@Organization", DbType.String, referenceBO.Organization);
                        dbSmartAspects.AddInParameter(command, "@Designation", DbType.String, referenceBO.Designation);
                        dbSmartAspects.AddInParameter(command, "@CellNumber", DbType.String, referenceBO.CellNumber);
                        dbSmartAspects.AddInParameter(command, "@TelephoneNumber", DbType.String, referenceBO.TelephoneNumber);

                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, referenceBO.Description);
                        dbSmartAspects.AddInParameter(command, "@SalesCommission", DbType.Decimal, referenceBO.SalesCommission);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, referenceBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@ReferenceId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpReferenceId = Convert.ToInt32(command.Parameters["@ReferenceId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public bool UpdateGuestReferenceInfo(GuestReferenceBO referenceBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGuestReferenceInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ReferenceId", DbType.Int32, referenceBO.ReferenceId);
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, referenceBO.Name);
                        dbSmartAspects.AddInParameter(command, "@Email", DbType.String, referenceBO.Email);
                        dbSmartAspects.AddInParameter(command, "@Organization", DbType.String, referenceBO.Organization);
                        dbSmartAspects.AddInParameter(command, "@Designation", DbType.String, referenceBO.Designation);
                        dbSmartAspects.AddInParameter(command, "@CellNumber", DbType.String, referenceBO.CellNumber);
                        dbSmartAspects.AddInParameter(command, "@TelephoneNumber", DbType.String, referenceBO.TelephoneNumber);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, referenceBO.Description);
                        dbSmartAspects.AddInParameter(command, "@SalesCommission", DbType.Decimal, referenceBO.SalesCommission);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, referenceBO.LastModifiedBy);
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
    }
}
