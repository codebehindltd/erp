using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.Inventory;

namespace HotelManagement.Data.HMCommon
{
    public class HMCommonSetupDA : BaseService
    {
        public bool SaveOrUpdateCommonConfiguration(HMCommonSetupBO commonSetupBO, out int tmpFloorId)
        {
            bool status = false;
            tmpFloorId = 0;

            if (commonSetupBO.SetupId == 0 || commonSetupBO.SetupId.ToString() == "")
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCommonConfiguration_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@SetupName", DbType.String, commonSetupBO.SetupName);
                        dbSmartAspects.AddInParameter(command, "@TypeName", DbType.String, commonSetupBO.TypeName);
                        dbSmartAspects.AddInParameter(command, "@SetupValue", DbType.String, commonSetupBO.SetupValue);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, commonSetupBO.Description);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, commonSetupBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@SetupId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpFloorId = Convert.ToInt32(command.Parameters["@SetupId"].Value);
                    }
                }
            }
            else
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCommonConfiguration_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@SetupId", DbType.Int32, commonSetupBO.SetupId);
                        dbSmartAspects.AddInParameter(command, "@SetupName", DbType.String, commonSetupBO.SetupName);
                        dbSmartAspects.AddInParameter(command, "@TypeName", DbType.String, commonSetupBO.TypeName);
                        dbSmartAspects.AddInParameter(command, "@SetupValue", DbType.String, commonSetupBO.SetupValue);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, commonSetupBO.Description);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, commonSetupBO.LastModifiedBy);
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            return status;
        }
        public HMCommonSetupBO GetCommonConfigurationInfo(string typeName, string setupName)
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {                
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCommonConfigurationInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TypeName", DbType.String, typeName);
                    dbSmartAspects.AddInParameter(cmd, "@SetupName", DbType.String, setupName);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                commonSetupBO.SetupId = Convert.ToInt32(reader["SetupId"]);
                                commonSetupBO.TypeName = reader["TypeName"].ToString();
                                commonSetupBO.SetupName = reader["SetupName"].ToString();
                                
                                if(string.IsNullOrWhiteSpace(reader["SetupValue"].ToString()))
                                {
                                    commonSetupBO.SetupValue = "0";
                                }
                                else
                                {
                                    commonSetupBO.SetupValue = reader["SetupValue"].ToString();
                                }                                
                                commonSetupBO.Description = reader["Description"].ToString();
                            }
                        }
                    }
                }
            }
            return commonSetupBO;
        }
        public List<HMCommonSetupBO> GetAllCommonConfigurationInfo()
        {
            List<HMCommonSetupBO> commonSetupBOList = new List<HMCommonSetupBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllCommonConfigurationInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                                commonSetupBO.SetupId = Convert.ToInt32(reader["SetupId"]);
                                commonSetupBO.TypeName = reader["TypeName"].ToString();
                                commonSetupBO.SetupName = reader["SetupName"].ToString();
                                if (string.IsNullOrWhiteSpace(reader["SetupValue"].ToString()))
                                {
                                    commonSetupBO.SetupValue = "0";
                                }
                                else
                                {
                                    commonSetupBO.SetupValue = reader["SetupValue"].ToString();
                                }
                                commonSetupBO.Description = reader["Description"].ToString();
                                commonSetupBOList.Add(commonSetupBO);
                            }
                        }
                    }
                }
            }
            return commonSetupBOList;
        }
        public bool SaveOrUpdateInvItemClassificationConfiguration(string discountCategoryBOList, int createdBy)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveInvDefaultClassificationConfiguration_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ClassificationIdList", DbType.String, discountCategoryBOList);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }

            return status;
        }
        public bool UpdateSoftwareExpireDate(string expireDate)
        {
            bool status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateSoftwareExpireDate_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ExpireDateInfo", DbType.String, expireDate);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<HMCommonSetupBO> GetAllCommonPaymentModeInfo()
        {
            List<HMCommonSetupBO> commonSetupBOList = new List<HMCommonSetupBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllCommonPaymentModeInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                                commonSetupBO.PaymentModeId = Convert.ToInt32(reader["PaymentModeId"]);
                                commonSetupBO.PaymentMode = reader["PaymentMode"].ToString();
                                commonSetupBO.DisplayName = reader["DisplayName"].ToString();
                                commonSetupBO.PaymentAccountsPostingId = Convert.ToInt32(reader["PaymentAccountsPostingId"]);
                                commonSetupBO.ReceiveAccountsPostingId = Convert.ToInt32(reader["ReceiveAccountsPostingId"]);

                                commonSetupBOList.Add(commonSetupBO);
                            }
                        }
                    }
                }
            }
            return commonSetupBOList;
        }
        public bool SaveOrUpdatePaymentModeInfo(List<HMCommonSetupBO> paymentModeItemListBO)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                foreach (HMCommonSetupBO row in paymentModeItemListBO)
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdatePaymentModeInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@PaymentModeId", DbType.Int32, row.PaymentModeId);
                        dbSmartAspects.AddInParameter(command, "@PaymentAccountsPostingId", DbType.Int32, row.PaymentAccountsPostingId);
                        dbSmartAspects.AddInParameter(command, "@ReceiveAccountsPostingId", DbType.Int32, row.ReceiveAccountsPostingId);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, row.CreatedBy);
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }

            return status;
        }
        public bool SaveOrUpdateCommonCustomFieldDataInfo(List<HMCommonSetupBO> commonSetupBO)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                foreach (HMCommonSetupBO row in commonSetupBO)
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateCommonCustomFieldDataInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@FieldId", DbType.Int32, row.FieldId);
                        dbSmartAspects.AddInParameter(command, "@FieldType", DbType.String, row.FieldType);
                        dbSmartAspects.AddInParameter(command, "@FieldValue", DbType.String, row.FieldValue);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, row.Description);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, row.ActiveStat);
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }

            return status;
        }
    }
}
