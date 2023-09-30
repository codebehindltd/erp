using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;

namespace HotelManagement.Data.HMCommon
{
    public class CostCentreTabDA : BaseService
    {

        public int CheckIsOnlyRetailPOS()
        {
            int IsOnlyRetailPOS = 0;
            
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("CheckIsOnlyRetailPOS_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                IsOnlyRetailPOS = Convert.ToInt32(reader["IsOnlyRetailPOS"]);
                            }
                        }
                    }
                }
            }
            return IsOnlyRetailPOS;
        }
        public List<CostCentreTabBO> GetCostCentreTabInfo()
        {
            int Index = 1;
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCostCenterInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CostCentreTabBO costCentreTabBO = new CostCentreTabBO();

                                costCentreTabBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                costCentreTabBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                costCentreTabBO.CompanyName = reader["CompanyName"].ToString();
                                costCentreTabBO.CostCenter = reader["CostCenter"].ToString();

                                costCentreTabBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                costCentreTabBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                costCentreTabBO.CitySDCharge = Convert.ToDecimal(reader["CitySDCharge"]);
                                costCentreTabBO.AdditionalChargeType = reader["AdditionalChargeType"].ToString();
                                costCentreTabBO.AdditionalCharge = Convert.ToDecimal(reader["AdditionalCharge"]);

                                costCentreTabBO.CostCenterType = reader["CostCenterType"].ToString();
                                costCentreTabBO.IsRestaurant = Convert.ToBoolean(reader["IsRestaurant"]);
                                costCentreTabBO.DefaultView = reader["DefaultView"].ToString();
                                costCentreTabBO.OutletType = Convert.ToInt32(reader["OutletType"]);
                                costCentreTabBO.DefaultStockLocationId = Convert.ToInt32(reader["DefaultStockLocationId"]);
                                costCentreTabBO.IsDefaultCostCenter = Convert.ToBoolean(reader["IsDefaultCostCenter"]);

                                costCentreTabBO.IsVatSChargeInclusive = Convert.ToInt32(reader["IsVatSChargeInclusive"]);
                                costCentreTabBO.IsServiceChargeEnable = Convert.ToBoolean(reader["IsServiceChargeEnable"]);
                                costCentreTabBO.IsVatEnable = Convert.ToBoolean(reader["IsVatEnable"]);
                                costCentreTabBO.IsRatePlusPlus = Convert.ToInt32(reader["IsRatePlusPlus"]);
                                costCentreTabBO.IsVatOnSDCharge = Convert.ToBoolean(reader["IsVatOnSDCharge"]);

                                costCentreTabBO.IsAdditionalChargeEnable = Convert.ToBoolean(reader["IsAdditionalChargeEnable"]);
                                costCentreTabBO.IsCitySDChargeEnable = Convert.ToBoolean(reader["IsCitySDChargeEnable"]);

                                costCentreTabBO.MappingId = 0;
                                costCentreTabBO.Index = Index++;

                                costCentreTabBOList.Add(costCentreTabBO);
                            }
                        }
                    }
                }
            }
            return costCentreTabBOList;
        }
        public List<CostCentreTabBO> GetCostCentreTabInfoByType(string costCenterType)
        {
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCostCentreTabInfoByType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterType", DbType.String, costCenterType);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CostCentreTabBO costCentreTabBO = new CostCentreTabBO();

                                costCentreTabBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                costCentreTabBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                costCentreTabBO.GLCompanyId = Convert.ToInt32(reader["GLCompanyId"]);
                                costCentreTabBO.CompanyName = reader["CompanyName"].ToString();
                                costCentreTabBO.CostCenter = reader["CostCenter"].ToString();
                                costCentreTabBO.CostCenterLogo = reader["CostCenterLogo"].ToString();

                                costCentreTabBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                costCentreTabBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                costCentreTabBO.CitySDCharge = Convert.ToDecimal(reader["CitySDCharge"]);
                                costCentreTabBO.AdditionalChargeType = reader["AdditionalChargeType"].ToString();
                                costCentreTabBO.AdditionalCharge = Convert.ToDecimal(reader["AdditionalCharge"]);

                                costCentreTabBO.CostCenterType = reader["CostCenterType"].ToString();
                                costCentreTabBO.IsRestaurant = Convert.ToBoolean(reader["IsRestaurant"]);
                                costCentreTabBO.DefaultView = reader["DefaultView"].ToString();
                                costCentreTabBO.OutletType = Convert.ToInt32(reader["OutletType"]);
                                costCentreTabBO.DefaultStockLocationId = Convert.ToInt32(reader["DefaultStockLocationId"]);
                                costCentreTabBO.IsDefaultCostCenter = Convert.ToBoolean(reader["IsDefaultCostCenter"]);

                                costCentreTabBO.IsVatSChargeInclusive = Convert.ToInt32(reader["IsVatSChargeInclusive"]);
                                costCentreTabBO.IsServiceChargeEnable = Convert.ToBoolean(reader["IsServiceChargeEnable"]);
                                costCentreTabBO.IsVatEnable = Convert.ToBoolean(reader["IsVatEnable"]);
                                costCentreTabBO.IsRatePlusPlus = Convert.ToInt32(reader["IsRatePlusPlus"]);
                                costCentreTabBO.IsVatOnSDCharge = Convert.ToBoolean(reader["IsVatOnSDCharge"]);
                                costCentreTabBO.IsCitySDChargeEnableOnServiceCharge = Convert.ToBoolean(reader["IsCitySDChargeEnableOnServiceCharge"]);

                                costCentreTabBO.IsAdditionalChargeEnable = Convert.ToBoolean(reader["IsAdditionalChargeEnable"]);
                                costCentreTabBO.IsCitySDChargeEnable = Convert.ToBoolean(reader["IsCitySDChargeEnable"]);
                                costCentreTabBO.IsDiscountApplicableOnRackRate = Convert.ToBoolean(reader["IsDiscountApplicableOnRackRate"]);

                                costCentreTabBO.InvoiceTemplate = Convert.ToInt32(reader["InvoiceTemplate"]);

                                costCentreTabBOList.Add(costCentreTabBO);
                            }
                        }
                    }
                }
            }
            return costCentreTabBOList;
        }
        public List<CostCentreTabBO> GetCostCentreTabInfoByUserGroupId(int userGroupId)
        {
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCostCentreTabInfoByUserGroupId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.String, userGroupId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CostCentreTabBO costCentreTabBO = new CostCentreTabBO();

                                costCentreTabBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                costCentreTabBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                costCentreTabBO.CompanyName = reader["CompanyName"].ToString();
                                costCentreTabBO.CostCenter = reader["CostCenter"].ToString();
                                costCentreTabBO.CostCenterType = reader["CostCenterType"].ToString();
                                costCentreTabBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                costCentreTabBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                costCentreTabBO.IsRestaurant = Convert.ToBoolean(reader["IsRestaurant"]);
                                costCentreTabBO.DefaultView = reader["DefaultView"].ToString();
                                costCentreTabBO.DefaultStockLocationId = Convert.ToInt32(reader["DefaultStockLocationId"]);
                                costCentreTabBO.OutletType = Convert.ToInt32(reader["OutletType"]);

                                costCentreTabBOList.Add(costCentreTabBO);
                            }
                        }
                    }
                }
            }
            return costCentreTabBOList;
        }
        public List<CostCentreTabBO> GetCostCentreTabInfoByUserGroupIdNCompanyId(int userGroupId , int CompanyId)
        {
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCostCentreTabInfoByUserGroupIdNCompanyId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.String, userGroupId);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.String, CompanyId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CostCentreTabBO costCentreTabBO = new CostCentreTabBO();

                                costCentreTabBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                costCentreTabBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                costCentreTabBO.CompanyName = reader["CompanyName"].ToString();
                                costCentreTabBO.CostCenter = reader["CostCenter"].ToString();
                                costCentreTabBO.CostCenterType = reader["CostCenterType"].ToString();
                                costCentreTabBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                costCentreTabBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                costCentreTabBO.IsRestaurant = Convert.ToBoolean(reader["IsRestaurant"]);
                                costCentreTabBO.DefaultView = reader["DefaultView"].ToString();
                                costCentreTabBO.DefaultStockLocationId = Convert.ToInt32(reader["DefaultStockLocationId"]);
                                costCentreTabBO.OutletType = Convert.ToInt32(reader["OutletType"]);
                                costCentreTabBO.CompanyType = reader["CompanyType"].ToString();

                                costCentreTabBOList.Add(costCentreTabBO);
                            }
                        }
                    }
                }
            }
            return costCentreTabBOList;
        }
        public List<CostCentreTabBO> GetUserWisePRPOCostCentreTabInfo(int userInfoId, string strPRPOType)
        {
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserWisePRPOCostCentreTabInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                    if (!string.IsNullOrEmpty(strPRPOType))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PRPOType", DbType.String, strPRPOType);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PRPOType", DbType.String, DBNull.Value);
                    }

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CostCentreTabBO costCentreTabBO = new CostCentreTabBO();

                                costCentreTabBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                costCentreTabBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                costCentreTabBO.CompanyName = reader["CompanyName"].ToString();
                                costCentreTabBO.CostCenter = reader["CostCenter"].ToString();
                                costCentreTabBO.CostCenterType = reader["CostCenterType"].ToString();
                                costCentreTabBO.OutletType = Convert.ToInt32(reader["OutletType"]);
                                costCentreTabBO.IsRestaurant = Convert.ToBoolean(reader["IsRestaurant"]);
                                costCentreTabBO.DefaultView = reader["DefaultView"].ToString();
                                costCentreTabBO.DefaultStockLocationId = Convert.ToInt32(reader["DefaultStockLocationId"]);
                                //costCentreTabBO.OutletType = Convert.ToInt32(reader["OutletType"]);

                                costCentreTabBOList.Add(costCentreTabBO);
                            }
                        }
                    }
                }
            }
            return costCentreTabBOList;
        }
        public List<CostCentreTabBO> GetAllRestaurantTypeCostCentreTabInfo()
        {
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllRestaurantTypeCostCentreTabInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CostCentreTabBO costCentreTabBO = new CostCentreTabBO();

                                costCentreTabBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                costCentreTabBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                costCentreTabBO.CompanyName = reader["CompanyName"].ToString();
                                costCentreTabBO.CostCenter = reader["CostCenter"].ToString();
                                costCentreTabBO.IsRestaurant = Convert.ToBoolean(reader["IsRestaurant"]);
                                costCentreTabBO.DefaultView = reader["DefaultView"].ToString();
                                //costCentreTabBO.OutletType = Convert.ToInt32(reader["OutletType"]);

                                costCentreTabBOList.Add(costCentreTabBO);
                            }
                        }
                    }
                }
            }
            return costCentreTabBOList;
        }
        public List<CostCentreTabBO> GetAllRestaurantTypeCostCentreInfoForStopChargePosting(int registrationId)
        {
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllRestaurantTypeCostCentreInfoForStopChargePosting_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CostCentreTabBO costCentreTabBO = new CostCentreTabBO();

                                costCentreTabBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                costCentreTabBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                costCentreTabBO.CompanyName = reader["CompanyName"].ToString();
                                costCentreTabBO.CostCenter = reader["CostCenter"].ToString();
                                costCentreTabBO.IsRestaurant = Convert.ToBoolean(reader["IsRestaurant"]);
                                costCentreTabBO.DefaultView = reader["DefaultView"].ToString();
                                costCentreTabBO.StopChargePostingStatus = Convert.ToInt32(reader["StopChargePostingStatus"]);
                                costCentreTabBOList.Add(costCentreTabBO);
                            }
                        }
                    }
                }
            }
            return costCentreTabBOList;
        }
        public List<CostCentreTabBO> GetUserWiseAllRestaurantTypeCostCentreTabInfo(int userInfoId)
        {
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserWiseAllRestaurantTypeCostCentreTabInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CostCentreTabBO costCentreTabBO = new CostCentreTabBO();

                                costCentreTabBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                costCentreTabBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                costCentreTabBO.CompanyName = reader["CompanyName"].ToString();
                                costCentreTabBO.CostCenter = reader["CostCenter"].ToString();
                                costCentreTabBO.IsRestaurant = Convert.ToBoolean(reader["IsRestaurant"]);
                                costCentreTabBO.DefaultView = reader["DefaultView"].ToString();
                                //costCentreTabBO.OutletType = Convert.ToInt32(reader["OutletType"]);

                                costCentreTabBOList.Add(costCentreTabBO);
                            }
                        }
                    }
                }
            }
            return costCentreTabBOList;
        }
        public List<CostCentreTabBO> GetAllCostCentreTabInfo()
        {
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllCostCentreTabInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CostCentreTabBO costCentreTabBO = new CostCentreTabBO();

                                costCentreTabBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                costCentreTabBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                costCentreTabBO.CompanyName = reader["CompanyName"].ToString();
                                costCentreTabBO.CostCenter = reader["CostCenter"].ToString();
                                costCentreTabBO.IsRestaurant = Convert.ToBoolean(reader["IsRestaurant"]);
                                costCentreTabBO.DefaultView = reader["DefaultView"].ToString();

                                costCentreTabBO.MappingId = 0;
                                //costCentreTabBO.OutletType = Convert.ToInt32(reader["OutletType"]);

                                costCentreTabBOList.Add(costCentreTabBO);
                            }
                        }
                    }
                }
            }
            return costCentreTabBOList;
        }
        public Boolean SaveCostCentreTabInfo(CostCentreTabBO costCentreTabBO, List<InvItemClassificationCostCenterMappingBO> mappingList, out int tmpCostCentreId)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCostCenterInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, costCentreTabBO.CompanyId);
                        dbSmartAspects.AddInParameter(command, "@GLCompanyId", DbType.Int32, costCentreTabBO.GLCompanyId);
                        dbSmartAspects.AddInParameter(command, "@CostCenter", DbType.String, costCentreTabBO.CostCenter);
                        dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, costCentreTabBO.ServiceCharge);
                        dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, costCentreTabBO.VatAmount);
                        dbSmartAspects.AddInParameter(command, "@CitySDCharge", DbType.Decimal, costCentreTabBO.CitySDCharge);
                        dbSmartAspects.AddInParameter(command, "@AdditionalChargeType", DbType.String, costCentreTabBO.AdditionalChargeType);
                        dbSmartAspects.AddInParameter(command, "@AdditionalCharge", DbType.Decimal, costCentreTabBO.AdditionalCharge);
                        dbSmartAspects.AddInParameter(command, "@IsVatSChargeInclusive", DbType.Int32, costCentreTabBO.IsVatSChargeInclusive);
                        dbSmartAspects.AddInParameter(command, "@IsRatePlusPlus", DbType.Int32, costCentreTabBO.IsRatePlusPlus);
                        dbSmartAspects.AddInParameter(command, "@IsVatOnSDCharge", DbType.Boolean, costCentreTabBO.IsVatOnSDCharge);
                        dbSmartAspects.AddInParameter(command, "@IsCitySDChargeEnableOnServiceCharge", DbType.Boolean, costCentreTabBO.IsCitySDChargeEnableOnServiceCharge);
                        dbSmartAspects.AddInParameter(command, "@CostCenterType", DbType.String, costCentreTabBO.CostCenterType);
                        dbSmartAspects.AddInParameter(command, "@IsRestaurant", DbType.Boolean, costCentreTabBO.IsRestaurant);
                        dbSmartAspects.AddInParameter(command, "@DefaultView", DbType.String, costCentreTabBO.DefaultView);
                        dbSmartAspects.AddInParameter(command, "@InvoiceTemplate", DbType.Int32, costCentreTabBO.InvoiceTemplate);
                        dbSmartAspects.AddInParameter(command, "@BillingStartTime", DbType.Int32, costCentreTabBO.BillingStartTime);
                        dbSmartAspects.AddInParameter(command, "@IsDefaultCostCenter", DbType.Boolean, costCentreTabBO.IsDefaultCostCenter);
                        dbSmartAspects.AddInParameter(command, "@OutletType", DbType.Int32, costCentreTabBO.OutletType);
                        dbSmartAspects.AddInParameter(command, "@BillNumberPrefix", DbType.String, costCentreTabBO.BillNumberPrefix);
                        dbSmartAspects.AddInParameter(command, "@IsServiceChargeEnable", DbType.Boolean, costCentreTabBO.IsServiceChargeEnable);
                        dbSmartAspects.AddInParameter(command, "@IsCitySDChargeEnable", DbType.Boolean, costCentreTabBO.IsCitySDChargeEnable);
                        dbSmartAspects.AddInParameter(command, "@IsVatEnable", DbType.Boolean, costCentreTabBO.IsVatEnable);
                        dbSmartAspects.AddInParameter(command, "@IsAdditionalChargeEnable", DbType.Boolean, costCentreTabBO.IsAdditionalChargeEnable);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, costCentreTabBO.CreatedBy);
                        dbSmartAspects.AddInParameter(command, "@PayrollDeptId", DbType.Int32, costCentreTabBO.PayrollDeptId);
                        dbSmartAspects.AddInParameter(command, "@IsDiscountEnable", DbType.Boolean, costCentreTabBO.IsDiscountEnable);
                        dbSmartAspects.AddInParameter(command, "@IsEnableItemAutoDeductFromStore", DbType.Boolean, costCentreTabBO.IsEnableItemAutoDeductFromStore);
                        dbSmartAspects.AddOutParameter(command, "@CostCenterId", DbType.Int32, sizeof(Int32));
                        

                        //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        status = dbSmartAspects.ExecuteNonQuery(command, transaction);

                        tmpCostCentreId = Convert.ToInt32(command.Parameters["@CostCenterId"].Value);


                        using (DbCommand commandClassification = dbSmartAspects.GetStoredProcCommand("SaveInvItemClassificationCostCenterMappingInfo_SP"))
                        {
                            foreach (InvItemClassificationCostCenterMappingBO classificationBO in mappingList)
                            {
                                commandClassification.Parameters.Clear();
                                dbSmartAspects.AddInParameter(commandClassification, "@ClassificationId", DbType.Int32, classificationBO.ClassificationId);
                                dbSmartAspects.AddInParameter(commandClassification, "@CostCenterId", DbType.Int32, tmpCostCentreId);
                                dbSmartAspects.AddOutParameter(commandClassification, "@MappingId", DbType.Int32, sizeof(Int32));
                                status = dbSmartAspects.ExecuteNonQuery(commandClassification, transaction);
                            }
                        }

                        //int costCount = mappingList.Count;
                        //for (int i = 0; i < costCount; i++)
                        //{
                        //    mappingList[i].CostCenterId = tmpCostCentreId;
                        //}

                        //if (costCount > 0)
                        //{
                        //    costStatus = SaveItemClassificationList(mappingList);
                        //}

                        if (status > 0)
                        {
                            transaction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }
        public Boolean UpdateCostCentreTabInfo(CostCentreTabBO costCentreTabBO, List<InvItemClassificationCostCenterMappingBO> mappingList)
        {
            Boolean status = false;
            Boolean costStatus = false;
            bool retVal = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCostCenterInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCentreTabBO.CostCenterId);
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, costCentreTabBO.CompanyId);
                        dbSmartAspects.AddInParameter(command, "@GLCompanyId", DbType.Int32, costCentreTabBO.GLCompanyId);
                        dbSmartAspects.AddInParameter(command, "@CostCenter", DbType.String, costCentreTabBO.CostCenter);
                        dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, costCentreTabBO.ServiceCharge);
                        dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, costCentreTabBO.VatAmount);
                        dbSmartAspects.AddInParameter(command, "@CitySDCharge", DbType.Decimal, costCentreTabBO.CitySDCharge);
                        dbSmartAspects.AddInParameter(command, "@AdditionalChargeType", DbType.String, costCentreTabBO.AdditionalChargeType);
                        dbSmartAspects.AddInParameter(command, "@AdditionalCharge", DbType.Decimal, costCentreTabBO.AdditionalCharge);
                        dbSmartAspects.AddInParameter(command, "@IsVatSChargeInclusive", DbType.Int32, costCentreTabBO.IsVatSChargeInclusive);
                        dbSmartAspects.AddInParameter(command, "@IsRatePlusPlus", DbType.Int32, costCentreTabBO.IsRatePlusPlus);
                        dbSmartAspects.AddInParameter(command, "@IsVatOnSDCharge", DbType.Boolean, costCentreTabBO.IsVatOnSDCharge);
                        dbSmartAspects.AddInParameter(command, "@IsCitySDChargeEnableOnServiceCharge", DbType.Boolean, costCentreTabBO.IsCitySDChargeEnableOnServiceCharge);
                        dbSmartAspects.AddInParameter(command, "@CostCenterType", DbType.String, costCentreTabBO.CostCenterType);
                        dbSmartAspects.AddInParameter(command, "@IsRestaurant", DbType.Boolean, costCentreTabBO.IsRestaurant);
                        dbSmartAspects.AddInParameter(command, "@DefaultView", DbType.String, costCentreTabBO.DefaultView);
                        dbSmartAspects.AddInParameter(command, "@InvoiceTemplate", DbType.Int32, costCentreTabBO.InvoiceTemplate);
                        dbSmartAspects.AddInParameter(command, "@BillingStartTime", DbType.Int32, costCentreTabBO.BillingStartTime);
                        dbSmartAspects.AddInParameter(command, "@IsDefaultCostCenter", DbType.Boolean, costCentreTabBO.IsDefaultCostCenter);
                        dbSmartAspects.AddInParameter(command, "@OutletType", DbType.Int32, costCentreTabBO.OutletType);
                        dbSmartAspects.AddInParameter(command, "@BillNumberPrefix", DbType.String, costCentreTabBO.BillNumberPrefix);
                        dbSmartAspects.AddInParameter(command, "@IsServiceChargeEnable", DbType.Boolean, costCentreTabBO.IsServiceChargeEnable);
                        dbSmartAspects.AddInParameter(command, "@IsCitySDChargeEnable", DbType.Boolean, costCentreTabBO.IsCitySDChargeEnable);
                        dbSmartAspects.AddInParameter(command, "@IsVatEnable", DbType.Boolean, costCentreTabBO.IsVatEnable);
                        dbSmartAspects.AddInParameter(command, "@IsAdditionalChargeEnable", DbType.Boolean, costCentreTabBO.IsAdditionalChargeEnable);
                        dbSmartAspects.AddInParameter(command, "@DefaultStockLocationId", DbType.Int32, costCentreTabBO.DefaultStockLocationId);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, costCentreTabBO.LastModifiedBy);
                        dbSmartAspects.AddInParameter(command, "@PayrollDeptId", DbType.Int32, costCentreTabBO.PayrollDeptId);
                        dbSmartAspects.AddInParameter(command, "@IsDiscountEnable", DbType.Boolean, costCentreTabBO.IsDiscountEnable);
                        dbSmartAspects.AddInParameter(command, "@IsEnableItemAutoDeductFromStore", DbType.Boolean, costCentreTabBO.IsEnableItemAutoDeductFromStore);


                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        if (mappingList.Count > 0)
                        {
                            costStatus = UpdateItemClassificationCostCenterMapping(mappingList);
                            if (status)
                                if (costStatus)
                                {
                                    transction.Commit();
                                    retVal = true;
                                }
                                else
                                {
                                    retVal = false;
                                }
                        }
                        else
                        {
                            if (status)
                                transction.Commit();
                            retVal = true;
                        }
                    }
                }
            }
            return retVal;
        }
        public CostCentreTabBO GetCostCentreTabInfoById(int costCenterId)
        {
            CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCostCenterInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                costCentreTabBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                costCentreTabBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                costCentreTabBO.GLCompanyId = Convert.ToInt32(reader["GLCompanyId"]);
                                costCentreTabBO.PayrollDeptId = Convert.ToInt32(reader["PayrollDeptId"]);
                                costCentreTabBO.CompanyName = reader["CompanyName"].ToString();
                                costCentreTabBO.CostCenter = reader["CostCenter"].ToString();
                                costCentreTabBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                costCentreTabBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                costCentreTabBO.CitySDCharge = Convert.ToDecimal(reader["CitySDCharge"]);
                                costCentreTabBO.AdditionalChargeType = reader["AdditionalChargeType"].ToString();
                                costCentreTabBO.AdditionalCharge = Convert.ToDecimal(reader["AdditionalCharge"]);
                                costCentreTabBO.CostCenterType = reader["CostCenterType"].ToString();
                                costCentreTabBO.IsRestaurant = Convert.ToBoolean(reader["IsRestaurant"]);
                                costCentreTabBO.InvoiceTemplate = Convert.ToInt32(reader["InvoiceTemplate"]);
                                costCentreTabBO.BillingStartTime = Convert.ToInt32(reader["BillingStartTime"]);
                                costCentreTabBO.DefaultView = reader["DefaultView"].ToString();
                                costCentreTabBO.OutletType = Convert.ToInt32(reader["OutletType"]);
                                costCentreTabBO.BillNumberPrefix = Convert.ToString(reader["BillNumberPrefix"]);
                                costCentreTabBO.DefaultStockLocationId = Convert.ToInt32(reader["DefaultStockLocationId"]);
                                costCentreTabBO.IsDefaultCostCenter = Convert.ToBoolean(reader["IsDefaultCostCenter"]);
                                costCentreTabBO.IsVatSChargeInclusive = Convert.ToInt32(reader["IsVatSChargeInclusive"]);
                                costCentreTabBO.IsServiceChargeEnable = Convert.ToBoolean(reader["IsServiceChargeEnable"]);
                                costCentreTabBO.IsVatEnable = Convert.ToBoolean(reader["IsVatEnable"]);
                                costCentreTabBO.IsRatePlusPlus = Convert.ToInt32(reader["IsRatePlusPlus"]);
                                costCentreTabBO.IsVatOnSDCharge = Convert.ToBoolean(reader["IsVatOnSDCharge"]);
                                costCentreTabBO.IsCitySDChargeEnableOnServiceCharge = Convert.ToBoolean(reader["IsCitySDChargeEnableOnServiceCharge"]);
                                costCentreTabBO.IsAdditionalChargeEnable = Convert.ToBoolean(reader["IsAdditionalChargeEnable"]);
                                costCentreTabBO.IsCitySDChargeEnable = Convert.ToBoolean(reader["IsCitySDChargeEnable"]);
                                costCentreTabBO.IsDiscountApplicableOnRackRate = Convert.ToBoolean(reader["IsDiscountApplicableOnRackRate"]);
                                costCentreTabBO.IsDiscountEnable = Convert.ToBoolean(reader["IsDiscountEnable"]);
                                costCentreTabBO.IsEnableItemAutoDeductFromStore = Convert.ToBoolean(reader["IsEnableItemAutoDeductFromStore"]);
                                costCentreTabBO.IsCostCenterNameShowOnInvoice = Convert.ToBoolean(reader["IsCostCenterNameShowOnInvoice"]);
                                costCentreTabBO.IsCustomerDetailsEnable = Convert.ToBoolean(reader["IsCustomerDetailsEnable"]);
                                costCentreTabBO.IsDeliveredByEnable = Convert.ToBoolean(reader["IsDeliveredByEnable"]);
                                costCentreTabBO.CompanyType = reader["CompanyType"].ToString();
                                costCentreTabBO.VatRegistrationNo = reader["VatRegistrationNo"].ToString();
                                costCentreTabBO.ContactNumber = reader["ContactNumber"].ToString();
                            }
                        }
                    }
                }
            }
            return costCentreTabBO;
        }
        public CostCentreTabBO GetCostCenterDetailInformation(string transactionType, int transactionId)
        {
            CostCentreTabBO costCentreTabBO = new CostCentreTabBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCostCenterDetailInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, transactionType);
                    dbSmartAspects.AddInParameter(cmd, "@TransactionId", DbType.Int32, transactionId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                costCentreTabBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                costCentreTabBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                costCentreTabBO.GLCompanyId = Convert.ToInt32(reader["GLCompanyId"]);
                                costCentreTabBO.CompanyName = reader["CompanyName"].ToString();
                                costCentreTabBO.CompanyAddress = reader["CompanyAddress"].ToString();
                                costCentreTabBO.CompanyType = reader["CompanyType"].ToString();
                                costCentreTabBO.CostCenterType = reader["CostCenterType"].ToString();
                                costCentreTabBO.CostCenter = reader["CostCenter"].ToString();
                                costCentreTabBO.IsServiceChargeEnable = Convert.ToBoolean(reader["IsServiceChargeEnable"]);
                                costCentreTabBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                costCentreTabBO.IsVatEnable = Convert.ToBoolean(reader["IsVatEnable"]);
                                costCentreTabBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                costCentreTabBO.IsCitySDChargeEnable = Convert.ToBoolean(reader["IsCitySDChargeEnable"]);
                                costCentreTabBO.CitySDCharge = Convert.ToDecimal(reader["CitySDCharge"]);
                                costCentreTabBO.IsRestaurant = Convert.ToBoolean(reader["IsRestaurant"]);
                                costCentreTabBO.DefaultView = reader["DefaultView"].ToString();
                                costCentreTabBO.InvoiceTemplate = Convert.ToInt32(reader["InvoiceTemplate"]);
                                costCentreTabBO.IsCostCenterNameShowOnInvoice = Convert.ToBoolean(reader["IsCostCenterNameShowOnInvoice"]);
                                costCentreTabBO.BillingStartTime = Convert.ToInt32(reader["BillingStartTime"]);
                            }
                        }
                    }
                }
            }
            return costCentreTabBO;
        }
        public List<CostCentreTabBO> GetRestaurantTypeCostCentreTabInfo(string userId, int userInfoIdOrEmpId, int isBearer)
        {
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantTypeCostCentreTabInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.String, userId);
                    dbSmartAspects.AddInParameter(cmd, "@IsBearer", DbType.Int32, isBearer);
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoIdOrEmpId", DbType.Int32, userInfoIdOrEmpId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CostCentreTabBO costCentreTabBO = new CostCentreTabBO();

                                costCentreTabBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                costCentreTabBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                costCentreTabBO.CompanyName = reader["CompanyName"].ToString();
                                costCentreTabBO.CostCenter = reader["CostCenter"].ToString();
                                costCentreTabBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                costCentreTabBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                costCentreTabBO.IsRestaurant = Convert.ToBoolean(reader["IsRestaurant"]);
                                costCentreTabBO.DefaultView = reader["DefaultView"].ToString();
                                costCentreTabBO.IsDefaultCostCenter = Convert.ToBoolean(reader["IsDefaultCostCenter"]);
                                costCentreTabBO.BillNumberPrefix = Convert.ToString(reader["BillNumberPrefix"]);

                                costCentreTabBOList.Add(costCentreTabBO);
                            }
                        }
                    }
                }
            }
            return costCentreTabBOList;
        }
        public List<CostCentreTabBO> GetRestaurantAndRetailPosTypeCostCentreTabInfo(string userId, int userInfoIdOrEmpId, int isBearer)
        {
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantAndRetailPosTypeCostCentreTabInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.String, userId);
                    dbSmartAspects.AddInParameter(cmd, "@IsBearer", DbType.Int32, isBearer);
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoIdOrEmpId", DbType.Int32, userInfoIdOrEmpId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CostCentreTabBO costCentreTabBO = new CostCentreTabBO();

                                costCentreTabBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                costCentreTabBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                costCentreTabBO.CompanyName = reader["CompanyName"].ToString();
                                costCentreTabBO.CostCenter = reader["CostCenter"].ToString();
                                costCentreTabBO.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                costCentreTabBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                costCentreTabBO.IsRestaurant = Convert.ToBoolean(reader["IsRestaurant"]);
                                costCentreTabBO.DefaultView = reader["DefaultView"].ToString();
                                costCentreTabBO.IsDefaultCostCenter = Convert.ToBoolean(reader["IsDefaultCostCenter"]);
                                costCentreTabBO.BillNumberPrefix = Convert.ToString(reader["BillNumberPrefix"]);
                                costCentreTabBO.CostCenterType = reader["CostCenterType"].ToString();
                                costCentreTabBOList.Add(costCentreTabBO);
                            }
                        }
                    }
                }
            }
            return costCentreTabBOList;
        }
        public Boolean GetIsDiscountEnableForCostCenter(int costCenterId)
        {
            Boolean IsDiscountEnable = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetIsDiscountEnableForCostCenter_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                IsDiscountEnable = Convert.ToBoolean(reader["IsDiscountEnable"]);
                            }
                        }
                    }
                }
            }
            return IsDiscountEnable;
        }
        private bool UpdateItemClassificationCostCenterMapping(List<InvItemClassificationCostCenterMappingBO> mappingList)
        {
            InvItemClassificationCostCenterMappingDA costDA = new InvItemClassificationCostCenterMappingDA();
            List<InvItemClassificationCostCenterMappingBO> dbList = new List<InvItemClassificationCostCenterMappingBO>();
            dbList = costDA.GetInvItemClassificationCostCenterMappingByCostCenterId((Int64)mappingList[0].CostCenterId);

            List<Int64> idList = new List<Int64>();
            Int64 mappingId;

            for (int i = 0; i < mappingList.Count; i++)
            {
                Int64 succ = IsAvailableInDb(dbList, mappingList[i]);
                if (succ > 0)
                {
                    //Update
                    mappingList[i].MappingId = succ;
                    bool status = costDA.UpdateInvItemClassificationCostCenterMappingInfo(mappingList[i]);
                    idList.Add(succ);
                }
                else
                {
                    //Insert
                    bool status = costDA.SaveInvItemClassificationCostCenterMappingInfo(mappingList[i], out mappingId);
                    idList.Add(mappingId);
                }
            }

            string saveAndUpdatedIdList = string.Empty;
            for (int j = 0; j < idList.Count; j++)
            {
                if (string.IsNullOrWhiteSpace(saveAndUpdatedIdList))
                {
                    saveAndUpdatedIdList = idList[j].ToString();
                }
                else
                {
                    saveAndUpdatedIdList = saveAndUpdatedIdList + "," + idList[j];
                }
            }
            Boolean deleteStatus = costDA.DeleteAllInvItemClassificationCostCenterMappingInfoWithoutMappingIdList((Int64)mappingList[0].CostCenterId, saveAndUpdatedIdList);
            return true;
        }
        private Int64 IsAvailableInDb(List<InvItemClassificationCostCenterMappingBO> dbList, InvItemClassificationCostCenterMappingBO costBO)
        {
            Int64 isInDB = 0;
            for (int j = 0; j < dbList.Count; j++)
            {
                if (dbList[j].ClassificationId == costBO.ClassificationId && costBO.CostCenterId == dbList[j].CostCenterId)
                {
                    isInDB = (Int64)dbList[j].MappingId;
                }
            }
            return isInDB;
        }
        public bool GetRoomStopChargePostingByRegistrationAndCostCenterId(int registrationId, int costCenterId)
        {
            string query = string.Format("SELECT ISNULL(StopChargePostingId, 0) StopChargePostingId FROM HotelRoomStopChargePosting WHERE RegistrationId = {0} AND CostCenterId = {1}", registrationId, costCenterId);
            int stopChargePostingId = 0;

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
                                stopChargePostingId = Convert.ToInt32(reader["StopChargePostingId"]);
                            }
                        }
                    }
                }
            }
            return stopChargePostingId > 0 ? true : false;
        }
        public bool CheckCostCenterName(string CostCenterName)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("CheckCostCenterName_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterName", DbType.String, CostCenterName);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                
                                status = Convert.ToInt32(reader["CostCenter"]);
                            }
                        }
                    }
                }
                retVal= status > 0 ? true : false;
            }
        return retVal;
        }
    }
}
