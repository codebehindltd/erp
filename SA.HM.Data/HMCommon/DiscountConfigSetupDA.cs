using HotelManagement.Entity.HMCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.HMCommon
{
    public class DiscountConfigSetupDA: BaseService
    {
        public DiscountConfigSetupBO GetDiscountConfigSetup()
        {
            DiscountConfigSetupBO discountConfigs = new DiscountConfigSetupBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDiscountConfigSetupByConfigurationId_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                discountConfigs.IsDiscountApplicableIndividually = Convert.ToBoolean(reader["IsDiscountApplicableIndividually"]);
                                discountConfigs.IsDiscountApplicableMaxOneWhenMultiple = Convert.ToBoolean(reader["IsDiscountApplicableMaxOneWhenMultiple"]);
                                discountConfigs.IsDiscountOptionShowsWhenMultiple = Convert.ToBoolean(reader["IsDiscountOptionShowsWhenMultiple"]);
                                discountConfigs.IsDiscountAndMembershipDiscountApplicableTogether = Convert.ToBoolean(reader["IsDiscountAndMembershipDiscountApplicableTogether"]);
                                discountConfigs.IsBankDiscount = Convert.ToBoolean(reader["IsBankDiscount"]);
                                discountConfigs.ConfigurationId = Convert.ToInt64(reader["ConfigurationId"]);
                                
                            }
                        }
                    }
                }
            }

            return discountConfigs;
        }
        public bool SaveDiscountConfigSetup(DiscountConfigSetupBO itemSave, int createdBy, out Int64 tmpId)
        {
            int status = 0;
            bool returnVal = false;
            tmpId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection()) 
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        
                            using (DbCommand commandAdd = dbSmartAspects.GetStoredProcCommand("SaveDiscountConfigSetup_SP"))
                            {
                                
                                    commandAdd.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandAdd, "@IsDiscountApplicableIndividually", DbType.Boolean, itemSave.IsDiscountApplicableIndividually);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsDiscountApplicableMaxOneWhenMultiple", DbType.Boolean, itemSave.IsDiscountApplicableMaxOneWhenMultiple);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsDiscountOptionShowsWhenMultiple", DbType.Boolean, itemSave.IsDiscountOptionShowsWhenMultiple);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsDiscountAndMembershipDiscountApplicableTogether", DbType.Boolean, itemSave.IsDiscountAndMembershipDiscountApplicableTogether);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsBankDiscount", DbType.Boolean, itemSave.IsBankDiscount);
                                    dbSmartAspects.AddInParameter(commandAdd, "@CreatedBy", DbType.Int32, createdBy);
                                    dbSmartAspects.AddOutParameter(commandAdd, "@ConfigurationId", DbType.Int64, sizeof(Int64));

                                    status = dbSmartAspects.ExecuteNonQuery(commandAdd, transction);

                                    tmpId = Convert.ToInt64(commandAdd.Parameters["@ConfigurationId"].Value);
                                
                            }
                        
                        //else
                        //{
                        //    status = 1;
                        //}
                        //if (status > 0 && DiscountConfigSetupListEdit.Count > 0)
                        //{
                        //    using (DbCommand commandAdd = dbSmartAspects.GetStoredProcCommand("UpdateDiscountConfigSetup_SP"))
                        //    {
                        //        foreach (var item in DiscountConfigSetupListEdit)
                        //        {
                        //            commandAdd.Parameters.Clear();
                        //            dbSmartAspects.AddInParameter(commandAdd, "@ConfigurationId", DbType.Int64, item.ConfigurationId);
                        //            dbSmartAspects.AddInParameter(commandAdd, "@IsDiscountApplicableIndividually", DbType.Int64, item.IsDiscountApplicableIndividually);
                        //            dbSmartAspects.AddInParameter(commandAdd, "@IsDiscountApplicableMaxOneWhenMultiple", DbType.Int64, item.IsDiscountApplicableMaxOneWhenMultiple);
                        //            dbSmartAspects.AddInParameter(commandAdd, "@IsDiscountOptionShowsWhenMultiple", DbType.Int64, item.IsDiscountOptionShowsWhenMultiple);
                        //            dbSmartAspects.AddInParameter(commandAdd, "@IsDiscountAndMembershipDiscountApplicableTogether", DbType.Int64, item.IsDiscountAndMembershipDiscountApplicableTogether);
                        //            dbSmartAspects.AddInParameter(commandAdd, "@IsBankDiscount", DbType.Int64, item.IsBankDiscount);
                        //            dbSmartAspects.AddInParameter(commandAdd, "@LastModifiedBy", DbType.Int32, createdBy);


                        //            status = dbSmartAspects.ExecuteNonQuery(commandAdd, transction);
                        //        }
                                
                        //    }
                        //}
                        if (status > 0)
                        {
                            returnVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            returnVal = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

                return returnVal;
        }
        public bool UpdateDiscountConfigSetup(DiscountConfigSetupBO itemUpdate, int createdBy)
        {
            int status = 0;
            bool returnVal = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandAdd = dbSmartAspects.GetStoredProcCommand("UpdateDiscountConfigSetup_SP"))
                        {
                            commandAdd.Parameters.Clear();
                            dbSmartAspects.AddInParameter(commandAdd, "@ConfigurationId", DbType.Boolean, itemUpdate.ConfigurationId);
                            dbSmartAspects.AddInParameter(commandAdd, "@IsDiscountApplicableIndividually", DbType.Boolean, itemUpdate.IsDiscountApplicableIndividually);
                            dbSmartAspects.AddInParameter(commandAdd, "@IsDiscountApplicableMaxOneWhenMultiple", DbType.Boolean, itemUpdate.IsDiscountApplicableMaxOneWhenMultiple);
                            dbSmartAspects.AddInParameter(commandAdd, "@IsDiscountOptionShowsWhenMultiple", DbType.Boolean, itemUpdate.IsDiscountOptionShowsWhenMultiple);
                            dbSmartAspects.AddInParameter(commandAdd, "@IsDiscountAndMembershipDiscountApplicableTogether", DbType.Boolean, itemUpdate.IsDiscountAndMembershipDiscountApplicableTogether);
                            dbSmartAspects.AddInParameter(commandAdd, "@IsBankDiscount", DbType.Boolean, itemUpdate.IsBankDiscount);
                            dbSmartAspects.AddInParameter(commandAdd, "@LastModifiedBy", DbType.Int32, createdBy);


                            status = dbSmartAspects.ExecuteNonQuery(commandAdd, transction);
                        }
                        if (status > 0)
                        {
                            returnVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            returnVal = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

                return returnVal;
        }
    }
    

}
