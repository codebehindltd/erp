using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.GeneralLedger;
using System.Data;
using System.Data.Common;

namespace HotelManagement.Data.GeneralLedger
{
    public class GLCompanyDA : BaseService
    {
        public bool SaveGLCompanyInfo(GLCompanyBO companyBO, out int tmpCompanyId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGLCompanyInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@Name", DbType.String, companyBO.Name);
                    dbSmartAspects.AddInParameter(command, "@Code", DbType.String, companyBO.Code);
                    dbSmartAspects.AddInParameter(command, "@ShortName", DbType.String, companyBO.ShortName);
                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, companyBO.Description);
                    dbSmartAspects.AddInParameter(command, "@CompanyAddress", DbType.String, companyBO.CompanyAddress);
                    dbSmartAspects.AddInParameter(command, "@WebAddress", DbType.String, companyBO.WebAddress);
                    dbSmartAspects.AddInParameter(command, "@BinNumber", DbType.String, companyBO.BinNumber);
                    dbSmartAspects.AddInParameter(command, "@TinNumber", DbType.String, companyBO.TinNumber);
                    dbSmartAspects.AddInParameter(command, "@BudgetType", DbType.String, companyBO.BudgetType);
                    dbSmartAspects.AddInParameter(command, "@InterCompanyTransactionHeadId", DbType.Int32, companyBO.InterCompanyTransactionHeadId);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, companyBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@CompanyId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpCompanyId = Convert.ToInt32(command.Parameters["@CompanyId"].Value);
                }
            }
            return status;
        }


        public List<GLCompanyBO> GetCompanyBySupplierId(int supplierId)
        {
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyBySupplierId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLCompanyBO companyBO = new GLCompanyBO();
                                companyBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                companyList.Add(companyBO);
                            }
                        }
                    }
                }
            }
            return companyList;
        }

        public bool UpdateGLCompanyInfo(GLCompanyBO companyBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGLCompanyInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, companyBO.CompanyId);
                    dbSmartAspects.AddInParameter(command, "@Name", DbType.String, companyBO.Name);
                    dbSmartAspects.AddInParameter(command, "@Code", DbType.String, companyBO.Code);
                    dbSmartAspects.AddInParameter(command, "@ShortName", DbType.String, companyBO.ShortName);
                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, companyBO.Description);
                    dbSmartAspects.AddInParameter(command, "@CompanyAddress", DbType.String, companyBO.CompanyAddress);
                    dbSmartAspects.AddInParameter(command, "@WebAddress", DbType.String, companyBO.WebAddress);
                    dbSmartAspects.AddInParameter(command, "@BinNumber", DbType.String, companyBO.BinNumber);
                    dbSmartAspects.AddInParameter(command, "@TinNumber", DbType.String, companyBO.TinNumber);
                    dbSmartAspects.AddInParameter(command, "@BudgetType", DbType.String, companyBO.BudgetType);
                    dbSmartAspects.AddInParameter(command, "@InterCompanyTransactionHeadId", DbType.Int32, companyBO.InterCompanyTransactionHeadId);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, companyBO.LastModifiedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public List<GLCompanyBO> GetAllGLCompanyInfo()
        {
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllGLCompanyInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLCompanyBO companyBO = new GLCompanyBO();
                                companyBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                companyBO.Name = reader["Name"].ToString();
                                companyBO.Code = reader["Code"].ToString();
                                companyBO.ShortName = reader["ShortName"].ToString();
                                companyBO.Description = reader["Description"].ToString();
                                companyBO.IsProfitableOrganization = Convert.ToBoolean(reader["IsProfitableOrganization"]);
                                companyList.Add(companyBO);
                            }
                        }
                    }
                }
            }
            return companyList;
        }

        public List<GLCompanyBO> GetSupplierCompanyInfo()
        {
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupplierCompanyInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLCompanyBO companyBO = new GLCompanyBO();
                                companyBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                companyBO.Name = reader["Name"].ToString();
                                companyList.Add(companyBO);
                            }
                        }
                    }
                }
            }
            return companyList;
        }

        public List<GLCompanyBO> GetAllGLCompanyInfoByUserGroupId(int userGroupId)
        {
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllGLCompanyInfoByUserGroupId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLCompanyBO companyBO = new GLCompanyBO();
                                companyBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                companyBO.Name = reader["Name"].ToString();
                                companyBO.Code = reader["Code"].ToString();
                                companyBO.ShortName = reader["ShortName"].ToString();
                                companyBO.Description = reader["Description"].ToString();
                                companyBO.IsProfitableOrganization = Convert.ToBoolean(reader["IsProfitableOrganization"]);
                                companyList.Add(companyBO);
                            }
                        }
                    }
                }
            }
            return companyList;
        }
        public GLCompanyBO GetGLCompanyInfoById(int CompanyId)
        {
            GLCompanyBO companyBO = new GLCompanyBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLCompanyInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, CompanyId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                companyBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                companyBO.Name = reader["Name"].ToString();
                                companyBO.Code = reader["Code"].ToString();
                                companyBO.ShortName = reader["ShortName"].ToString();
                                companyBO.Description = reader["Description"].ToString();
                                companyBO.IsProfitableOrganization = Convert.ToBoolean(reader["IsProfitableOrganization"]);
                                companyBO.InterCompanyTransactionHeadId = Convert.ToInt32(reader["InterCompanyTransactionHeadId"]);
                                companyBO.CompanyAddress = reader["CompanyAddress"].ToString();
                                companyBO.WebAddress = reader["WebAddress"].ToString();
                                companyBO.BinNumber = reader["BinNumber"].ToString();
                                companyBO.TinNumber = reader["TinNumber"].ToString();
                                companyBO.BudgetType = reader["BudgetType"].ToString();
                                companyBO.ImageName = reader["ImageName"].ToString();
                            }
                        }
                    }
                }
            }
            return companyBO;
        }

        public List<GLCompanyBO> GetAllGLCompanyInfoBySearchCriteria(string Name, string Code, string ShortName)
        {
            string SearchCriteria = GenerateWhereCondition(Name, Code, ShortName);
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllGLCompanyInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, SearchCriteria);

                    DataSet CompanyDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, CompanyDS, "Company");
                    DataTable Table = CompanyDS.Tables["Company"];

                    companyList = Table.AsEnumerable().Select(r => new GLCompanyBO
                    {
                        CompanyId = r.Field<int>("CompanyId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code"),
                        ShortName = r.Field<string>("ShortName"),
                        Description = r.Field<string>("Description")

                    }).ToList();
                }
            }
            return companyList;
        }

        private string GenerateWhereCondition(string Name, string Code, string ShortName)
        {
            string Where = string.Empty;

            if (!string.IsNullOrWhiteSpace(Name))
            {
                Where = "Name LIKE '%" + Name + "%'";
            }

            if (!string.IsNullOrWhiteSpace(Code))
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += " AND Code LIKE '%" + Code + "%'";
                }
                else
                {
                    Where = "Code LIKE '%" + Code + "%'";
                }
            }

            if (!string.IsNullOrWhiteSpace(ShortName))
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += " AND ShortName LIKE '%" + ShortName + "%'";
                }
                else
                {
                    Where = "ShortName LIKE '%" + ShortName + "%'";
                }
            }

            if (!string.IsNullOrEmpty(Where))
            {
                Where = " WHERE " + Where;
            }

            return Where;
        }

        ////--------------Donor
        //public List<GLDonorBO> GetAllDonorInfo()
        //{
        //    List<GLDonorBO> donorList = new List<GLDonorBO>();
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllGLDonorInfo_SP"))
        //        {
        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        GLDonorBO donor = new GLDonorBO();
        //                        donor.DonorId = Convert.ToInt32(reader["DonorId"]);
        //                        donor.DonorName = reader["DonorName"].ToString();
        //                        donor.DonorCode = reader["DonorCode"].ToString();
        //                        donorList.Add(donor);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return donorList;
        //}

    }
}
