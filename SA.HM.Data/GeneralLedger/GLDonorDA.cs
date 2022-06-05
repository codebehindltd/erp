using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.GeneralLedger;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.GeneralLedger
{
    public class GLDonorDA : BaseService
   {

        public List<GLDonorBO> GetAllGLDonorInfo()
       {
           List<GLDonorBO> projectList = new List<GLDonorBO>();
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllGLDonorInfo_SP"))
               {
                   using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                   {
                       if (reader != null)
                       {
                           while (reader.Read())
                           {
                               GLDonorBO projectBO = new GLDonorBO();
                               projectBO.DonorId = Convert.ToInt32(reader["DonorId"]);
                               //projectBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                               projectBO.Name = reader["Name"].ToString();
                               projectBO.Code = reader["Code"].ToString();
                               projectBO.ShortName = reader["ShortName"].ToString();
                               projectBO.Description = reader["Description"].ToString();
                               projectList.Add(projectBO);
                           }
                       }
                   }
               }
           }
           return projectList;
       }
        public bool SaveGLDonorInfo(GLDonorBO projectBO, out int tmpDonorId)
       {
           Boolean status = false;
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGLDonorInfo_SP"))
               {
                   dbSmartAspects.AddInParameter(command, "@Name", DbType.String, projectBO.Name);
                   //dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, projectBO.CompanyId);
                   dbSmartAspects.AddInParameter(command, "@Code", DbType.String, projectBO.Code);
                   dbSmartAspects.AddInParameter(command, "@ShortName", DbType.String, projectBO.ShortName);
                   dbSmartAspects.AddInParameter(command, "@Description", DbType.String, projectBO.Description);
                   dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, projectBO.CreatedBy);
                   dbSmartAspects.AddOutParameter(command, "@DonorId", DbType.Int32, sizeof(Int32));
                   status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                   tmpDonorId = Convert.ToInt32(command.Parameters["@DonorId"].Value);
               }
           }
           return status;
       }
        public bool UpdateGLDonorInfo(GLDonorBO projectBO)
       {
           Boolean status = false;
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGLDonorInfo_SP"))
               {
                   dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int32, projectBO.DonorId);
                   dbSmartAspects.AddInParameter(command, "@Name", DbType.String, projectBO.Name);
                   //dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, projectBO.CompanyId);
                   dbSmartAspects.AddInParameter(command, "@Code", DbType.String, projectBO.Code);
                   dbSmartAspects.AddInParameter(command, "@ShortName", DbType.String, projectBO.ShortName);
                   dbSmartAspects.AddInParameter(command, "@Description", DbType.String, projectBO.Description);
                   dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, projectBO.LastModifiedBy);
                   status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
               }
           }
           return status;
       }
        public GLDonorBO GetGLDonorInfoById(int DonorId)
       {
           GLDonorBO projectBO = new GLDonorBO();
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLDonorInfoById_SP"))
               {
                   dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int32, DonorId);

                   using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                   {
                       if (reader != null)
                       {
                           while (reader.Read())
                           {
                               //projectBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                               projectBO.DonorId = Convert.ToInt32(reader["DonorId"]);
                               projectBO.Name = reader["Name"].ToString();
                               projectBO.Code = reader["Code"].ToString();
                               projectBO.ShortName = reader["ShortName"].ToString();
                               projectBO.Description = reader["Description"].ToString();
                           }
                       }
                   }
               }
           }
           return projectBO;
       }
       // public List<GLDonorBO> GetDonorByCompanyId(int CompanyId)
       //{
       //    List<GLDonorBO> List = new List<GLDonorBO>();
       //    using (DbConnection conn = dbSmartAspects.CreateConnection())
       //    {
       //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLProjectInfoByCompanyId_SP"))
       //        {
       //            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, CompanyId);

       //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
       //            {
       //                if (reader != null)
       //                {
       //                    while (reader.Read())
       //                    {
       //                        GLDonorBO projectBO = new GLDonorBO();
       //                        projectBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
       //                        projectBO.DonorId = Convert.ToInt32(reader["DonorId"]);
       //                        projectBO.Name = reader["Name"].ToString();
       //                        projectBO.Code = reader["Code"].ToString();
       //                        projectBO.ShortName = reader["ShortName"].ToString();
       //                        projectBO.Description = reader["Description"].ToString();
       //                        List.Add(projectBO);
       //                    }
       //                }
       //            }
       //        }
       //    }
       //    return List;
       //}
       // public List<GLDonorBO> GetGLDonorInfoByGLCompany(int companyId)
       //{
       //    List<GLDonorBO> projectList = new List<GLDonorBO>();
       //    using (DbConnection conn = dbSmartAspects.CreateConnection())
       //    {
       //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLProjectInfoByGLCompany_SP"))
       //        {
       //            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
       //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
       //            {
       //                if (reader != null)
       //                {
       //                    while (reader.Read())
       //                    {
       //                        GLDonorBO projectBO = new GLDonorBO();
       //                        projectBO.DonorId = Convert.ToInt32(reader["DonorId"]);
       //                        //projectBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
       //                        projectBO.Name = reader["Name"].ToString();
       //                        projectBO.Code = reader["Code"].ToString();
       //                        projectBO.ShortName = reader["ShortName"].ToString();
       //                        projectBO.Description = reader["Description"].ToString();
       //                        projectList.Add(projectBO);
       //                    }
       //                }
       //            }
       //        }
       //    }
       //    return projectList;
       //}
        public List<GLDonorBO> GetAllGLDonorInfoBySearchCriteria(string DonorName, string ShortName, string DonorCode, int CompanyId)
       {
           List<GLDonorBO> projectList = new List<GLDonorBO>();
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllGLDonorInfoBySearchCriteria_SP"))
               {

                   dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DonorName);
                   dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, DonorCode);
                   //dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, CompanyId);
                   dbSmartAspects.AddInParameter(cmd, "@ShortName", DbType.String, ShortName);
                   using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                   {
                       if (reader != null)
                       {
                           while (reader.Read())
                           {
                               GLDonorBO projectBO = new GLDonorBO();
                               projectBO.DonorId = Convert.ToInt32(reader["DonorId"]);
                               //projectBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                               projectBO.Name = reader["Name"].ToString();
                               projectBO.Code = reader["Code"].ToString();
                               projectBO.ShortName = reader["ShortName"].ToString();
                               projectBO.Description = reader["Description"].ToString();
                               projectList.Add(projectBO);
                           }
                       }
                   }
               }
           }
           return projectList;
       }
   }
}
