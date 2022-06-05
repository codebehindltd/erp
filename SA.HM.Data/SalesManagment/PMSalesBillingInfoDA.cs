using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.SalesManagment;

namespace HotelManagement.Data.SalesManagment
{
   public class PMSalesBillingInfoDA :BaseService
    {
       public List<PMSalesBillingInfoBO> GetAllPMSalesBillingInfo()
        {
            List<PMSalesBillingInfoBO> List = new List<PMSalesBillingInfoBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllPMSalesBillingInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMSalesBillingInfoBO billingBO = new PMSalesBillingInfoBO();
                                billingBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                billingBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                                billingBO.BillingContactPerson = Convert.ToString(reader["BillingContactPerson"]);
                                billingBO.BillingInfoId = Convert.ToInt32(reader["BillingInfoId"]);
                                billingBO.BillingPersonDepartment = Convert.ToString(reader["BillingPersonDepartment"]);
                                billingBO.BillingPersonEmail = Convert.ToString(reader["BillingPersonEmail"]);
                                billingBO.BillingPersonPhone = reader["BillingPersonPhone"].ToString();
                                billingBO.CustomerId = Convert.ToInt32(reader["CustomerId"]);
                                List.Add(billingBO);
                            }
                        }
                    }
                }
            }
            return List;
        }
       public PMSalesBillingInfoBO GetAllPMSalesBillingInfoBillingInfoId(int BillingInfoId)
        {
            PMSalesBillingInfoBO billingBO = new PMSalesBillingInfoBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllPMSalesBillingInfoBillingInfoId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BillingInfoId", DbType.Int32, BillingInfoId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                billingBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                billingBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                                billingBO.BillingContactPerson = Convert.ToString(reader["BillingContactPerson"]);
                                billingBO.BillingInfoId = Convert.ToInt32(reader["BillingInfoId"]);
                                billingBO.BillingPersonDepartment = Convert.ToString(reader["BillingPersonDepartment"]);
                                billingBO.BillingPersonEmail = Convert.ToString(reader["BillingPersonEmail"]);
                                billingBO.BillingPersonPhone = reader["BillingPersonPhone"].ToString();
                                billingBO.BillingPersonDesignation = reader["BillingPersonDesignation"].ToString();
                            }
                        }
                    }
                }
            }
            return billingBO;
        }

       public bool SavePMSalesBillingInfo(PMSalesBillingInfoBO billingBO, out int tmpCustomerId)
       {
           Boolean status = false;
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePMSalesBillingInfo_SP"))
               {
                   dbSmartAspects.AddInParameter(command, "@CustomerId", DbType.Int32, billingBO.CustomerId);
                   dbSmartAspects.AddInParameter(command, "@BillingContactPerson", DbType.String, billingBO.BillingContactPerson);
                   dbSmartAspects.AddInParameter(command, "@BillingPersonDepartment", DbType.String, billingBO.BillingPersonDepartment);
                   dbSmartAspects.AddInParameter(command, "@BillingPersonDesignation", DbType.String, billingBO.BillingPersonDesignation);
                   dbSmartAspects.AddInParameter(command, "@BillingPersonPhone", DbType.String, billingBO.BillingPersonPhone);
                   dbSmartAspects.AddInParameter(command, "@BillingPersonEmail", DbType.String, billingBO.BillingPersonEmail);

                   dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, billingBO.CreatedBy);
                   dbSmartAspects.AddOutParameter(command, "@BillingInfoId", DbType.Int32, sizeof(Int32));
                   status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                   tmpCustomerId = Convert.ToInt32(command.Parameters["@BillingInfoId"].Value);
               }
           }
           return status;
       }

       public bool UpdatePMSalesBillingInfo(PMSalesBillingInfoBO billingBO)
       {
           Boolean status = false;
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePMSalesBillingInfo_SP"))
               {
                   dbSmartAspects.AddInParameter(command, "@BillingInfoId", DbType.Int32, billingBO.BillingInfoId);
                   dbSmartAspects.AddInParameter(command, "@CustomerId", DbType.Int32, billingBO.CustomerId);
                   dbSmartAspects.AddInParameter(command, "@BillingContactPerson", DbType.String, billingBO.BillingContactPerson);
                   dbSmartAspects.AddInParameter(command, "@BillingPersonDepartment", DbType.String, billingBO.BillingPersonDepartment);
                   dbSmartAspects.AddInParameter(command, "@BillingPersonDesignation", DbType.String, billingBO.BillingPersonDesignation);
                   dbSmartAspects.AddInParameter(command, "@BillingPersonPhone", DbType.String, billingBO.BillingPersonPhone);
                   dbSmartAspects.AddInParameter(command, "@BillingPersonEmail", DbType.String, billingBO.BillingPersonEmail);


                   dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, billingBO.LastModifiedBy);
                   status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
               }
           }
           return status;
       }



    }
}
