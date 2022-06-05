using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.SalesManagment;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.SalesManagment
{
   public class PMSalesSiteInfoDA :BaseService
   {

       public List<PMSalesSiteInfoBO> GetAllPMSalesSiteInfo()
       {
           List<PMSalesSiteInfoBO> List = new List<PMSalesSiteInfoBO>();
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllPMSalesSiteInfo_SP"))
               {
                   using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                   {
                       if (reader != null)
                       {
                           while (reader.Read())
                           {
                               PMSalesSiteInfoBO siteBO = new PMSalesSiteInfoBO();
                               siteBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                               siteBO.CustomerId = Convert.ToInt32(reader["CustomerId"]);
                               siteBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                               siteBO.SiteAddress = Convert.ToString(reader["SiteAddress"]);
                               siteBO.SiteContactPerson = Convert.ToString(reader["SiteContactPerson"]);
                               siteBO.SiteEmail = Convert.ToString(reader["SiteEmail"]);
                               siteBO.SiteId = reader["SiteId"].ToString();

                               siteBO.SiteName = reader["SiteName"].ToString();
                               siteBO.SitePhoneNumber = reader["SitePhoneNumber"].ToString();
                               siteBO.SiteEmail = reader["SiteEmail"].ToString();
                               siteBO.SiteInfoId =Convert.ToInt32( reader["SiteInfoId"].ToString());
                               List.Add(siteBO);
                           }
                       }
                   }
               }
           }
           return List;
       }

       public PMSalesSiteInfoBO GetAllPMSalesSiteInfoBySiteInfoId(int SiteInfoId)
       {
           PMSalesSiteInfoBO siteBO = new PMSalesSiteInfoBO();
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllPMSalesSiteInfoBySiteInfoId_SP"))
               {
                   dbSmartAspects.AddInParameter(cmd, "@SiteInfoId", DbType.Int32, SiteInfoId);
                   using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                   {
                       if (reader != null)
                       {
                           while (reader.Read())
                           {
                               siteBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                               siteBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                               siteBO.SiteAddress = Convert.ToString(reader["SiteAddress"]);
                               siteBO.SiteContactPerson = Convert.ToString(reader["SiteContactPerson"]);
                               siteBO.SiteEmail = Convert.ToString(reader["SiteEmail"]);
                               siteBO.SiteId = reader["SiteId"].ToString();

                               siteBO.SiteName = reader["SiteName"].ToString();
                               siteBO.SitePhoneNumber = reader["SitePhoneNumber"].ToString();
                               siteBO.SiteEmail = reader["SiteEmail"].ToString();
                               siteBO.SiteInfoId =Convert.ToInt32( reader["SiteInfoId"].ToString());

                           }
                       }
                   }
               }
           }
           return siteBO;
       }


       public bool SavePMSalesSiteInfo(PMSalesSiteInfoBO siteBO, out int tmpCustomerId)
       {
           Boolean status = false;
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePMSalesSiteInfo_SP"))
               {
                   dbSmartAspects.AddInParameter(command, "@SiteId", DbType.String, siteBO.SiteId);
                   dbSmartAspects.AddInParameter(command, "@CustomerId", DbType.Int32, siteBO.CustomerId);
                   dbSmartAspects.AddInParameter(command, "@SiteName", DbType.String, siteBO.SiteName);
                   dbSmartAspects.AddInParameter(command, "@SiteAddress", DbType.String, siteBO.SiteAddress);
                   dbSmartAspects.AddInParameter(command, "@SiteContactPerson", DbType.String, siteBO.SiteContactPerson);
                   dbSmartAspects.AddInParameter(command, "@SitePhoneNumber", DbType.String, siteBO.SitePhoneNumber);
                   dbSmartAspects.AddInParameter(command, "@SiteEmail", DbType.String, siteBO.SiteEmail);

                   dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, siteBO.CreatedBy);
                   dbSmartAspects.AddOutParameter(command, "@SiteInfoId", DbType.Int32, sizeof(Int32));
                   status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                   tmpCustomerId = Convert.ToInt32(command.Parameters["@SiteInfoId"].Value);
               }
           }
           return status;
       }

       public bool UpdatePMSalesSiteInfo(PMSalesSiteInfoBO siteBO)
       {
           Boolean status = false;
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePMSalesSiteInfo_SP"))
               {
                   dbSmartAspects.AddInParameter(command, "@SiteInfoId", DbType.Int32, siteBO.SiteInfoId); 
                   dbSmartAspects.AddInParameter(command, "@SiteId", DbType.String, siteBO.SiteId);
                   dbSmartAspects.AddInParameter(command, "@CustomerId", DbType.Int32, siteBO.CustomerId);
                   dbSmartAspects.AddInParameter(command, "@SiteName", DbType.String, siteBO.SiteName);
                   dbSmartAspects.AddInParameter(command, "@SiteAddress", DbType.String, siteBO.SiteAddress);
                   dbSmartAspects.AddInParameter(command, "@SiteContactPerson", DbType.String, siteBO.SiteContactPerson);
                   dbSmartAspects.AddInParameter(command, "@SitePhoneNumber", DbType.String, siteBO.SitePhoneNumber);
                   dbSmartAspects.AddInParameter(command, "@SiteEmail", DbType.String, siteBO.SiteEmail);
                   dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, siteBO.LastModifiedBy);
                   status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
               }
           }
           return status;
       }
   }
}
