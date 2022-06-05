using HotelManagement.Entity.LCManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.LCManagement
{
    public class LcCnfInfoDA : BaseService
    {
        public bool SaveCNF(LCCnfInfoBO infoBO, out int outId)
        {
            Boolean status = false;
            outId = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCNF_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@SupplierId", DbType.Int32, infoBO.SupplierId);
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, infoBO.Name);
                        dbSmartAspects.AddInParameter(command, "@Code", DbType.String, infoBO.Code);
                        dbSmartAspects.AddInParameter(command, "@Address", DbType.String, infoBO.Address);
                        dbSmartAspects.AddInParameter(command, "@Phone", DbType.String, infoBO.Phone);
                        dbSmartAspects.AddInParameter(command, "@Fax", DbType.String, infoBO.Fax);
                        dbSmartAspects.AddInParameter(command, "@Email", DbType.String, infoBO.Email);
                        dbSmartAspects.AddInParameter(command, "@WebAddress", DbType.String, infoBO.WebAddress);
                        dbSmartAspects.AddInParameter(command, "@ContactPerson", DbType.String, infoBO.ContactPerson);
                        dbSmartAspects.AddInParameter(command, "@ContactEmail", DbType.String, infoBO.ContactEmail);
                        dbSmartAspects.AddInParameter(command, "@ContactPhone", DbType.String, infoBO.ContactPhone);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, infoBO.Remarks);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, infoBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        outId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return status;
        }
        public List<LCCnfInfoBO> GetAllCNFInfoList()
        {
            List<LCCnfInfoBO> cnfInfoList = new List<LCCnfInfoBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllLCCnfInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                LCCnfInfoBO cnf = new LCCnfInfoBO();
                                cnf.SupplierId = Convert.ToInt32(reader["SupplierId"]);
                                cnf.Name = reader["Name"].ToString();
                                cnf.Code = reader["Code"].ToString();
                                cnfInfoList.Add(cnf);
                            }
                        }
                    }
                }
            }
            return cnfInfoList;
        }

        public List<LCCnfInfoBO> GetCNFInformation(string name, string code, string email, string phone, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<LCCnfInfoBO> infoBOs = new List<LCCnfInfoBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCNFInfoBySearchCriteriaForPaging_SP"))
                    {
                        if (!string.IsNullOrEmpty(name))
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, name);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(code))
                            dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, code);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(email))
                            dbSmartAspects.AddInParameter(cmd, "@Email", DbType.String, email);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Email", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(phone))
                            dbSmartAspects.AddInParameter(cmd, "@Phone", DbType.String, phone);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Phone", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    LCCnfInfoBO bO = new LCCnfInfoBO();
                                    bO.SupplierId = Convert.ToInt32(reader["SupplierId"]);
                                    bO.Name = Convert.ToString(reader["Name"]);
                                    bO.Code = Convert.ToString(reader["Code"]);
                                    bO.ContactEmail = Convert.ToString(reader["ContactEmail"]);
                                    bO.ContactPhone = Convert.ToString(reader["ContactPhone"]);

                                    infoBOs.Add(bO);
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

            return infoBOs;
        }

        public LCCnfInfoBO GetCNFInformationById(int id)
        {
            LCCnfInfoBO infoBO = new LCCnfInfoBO();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCNFInformationById_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, id);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    infoBO.SupplierId = Convert.ToInt32(reader["SupplierId"]);
                                    infoBO.Name = reader["Name"].ToString();
                                    infoBO.Code = reader["Code"].ToString();
                                    infoBO.Email = Convert.ToString(reader["Email"]);
                                    infoBO.Phone = reader["Phone"].ToString();
                                    infoBO.Fax = Convert.ToString(reader["Fax"]);
                                    infoBO.Address = Convert.ToString(reader["Address"]);
                                    infoBO.ContactEmail = reader["ContactEmail"].ToString();
                                    infoBO.ContactPerson = (reader["ContactPerson"]).ToString();
                                    infoBO.ContactPhone = reader["ContactPhone"].ToString();
                                    infoBO.WebAddress = reader["WebAddress"].ToString();
                                    infoBO.Remarks = reader["Remarks"].ToString();
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
            return infoBO;
        }
    }
}
