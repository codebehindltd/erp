using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Restaurant;
using System.Data.Common;
using System.Data;
using System.Collections;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;

namespace HotelManagement.Data.Restaurant
{
    public class RestaurantComboItemDA : BaseService
    {
        public List<RestaurantComboItemBO> GetRestaurantComboItemInfo(int costCenterId)
        {
            List<RestaurantComboItemBO> roomOwnerList = new List<RestaurantComboItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantComboInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantComboItemBO comboItemBO = new RestaurantComboItemBO();
                                comboItemBO.ComboId = Convert.ToInt32(reader["ComboId"]);
                                comboItemBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                comboItemBO.ComboName = reader["ComboName"].ToString();
                                comboItemBO.ComboPrice = Convert.ToDecimal(reader["ComboPrice"]);
                                comboItemBO.Code = reader["Code"].ToString();
                                comboItemBO.ImageName = reader["ImageName"].ToString();
                                
                                roomOwnerList.Add(comboItemBO);
                            }
                        }
                    }
                }
            }
            return roomOwnerList;
        }
        public Boolean SaveRestaurantComboItemInfo(InvItemBO invItemBO, List<InvItemCostCenterMappingBO> costCenterList, out int tmpItemId, List<InvItemDetailsBO> detailBO)
        {
            bool retVal = false;
            Boolean status = false;
            //int tmpRegistrationId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveInvItemInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@ItemType", DbType.String, invItemBO.ItemType);
                        dbSmartAspects.AddInParameter(commandMaster, "@Name", DbType.String, invItemBO.Name);
                        dbSmartAspects.AddInParameter(commandMaster, "@Code", DbType.String, invItemBO.Code);
                        dbSmartAspects.AddInParameter(commandMaster, "@Description", DbType.String, invItemBO.Description);
                        dbSmartAspects.AddInParameter(commandMaster, "@CategoryId", DbType.Int32, invItemBO.CategoryId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ManufacturerId", DbType.Int32, invItemBO.ManufacturerId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ProductType", DbType.String, invItemBO.ProductType);
                        dbSmartAspects.AddInParameter(commandMaster, "@PurchasePrice", DbType.Decimal, invItemBO.PurchasePrice);
                        dbSmartAspects.AddInParameter(commandMaster, "@SellingLocalCurrencyId", DbType.Int32, invItemBO.SellingLocalCurrencyId);
                        dbSmartAspects.AddInParameter(commandMaster, "@UnitPriceLocal", DbType.Decimal, invItemBO.UnitPriceLocal);
                        dbSmartAspects.AddInParameter(commandMaster, "@SellingUsdCurrencyId", DbType.Int32, invItemBO.SellingUsdCurrencyId);
                        dbSmartAspects.AddInParameter(commandMaster, "@UnitPriceUsd", DbType.Decimal, invItemBO.UnitPriceUsd);
                        dbSmartAspects.AddInParameter(commandMaster, "@StockType", DbType.String, invItemBO.StockType);
                        dbSmartAspects.AddInParameter(commandMaster, "@StockBy", DbType.Int32, invItemBO.StockBy);
                        dbSmartAspects.AddInParameter(commandMaster, "@ClassificationId", DbType.Int32, invItemBO.ClassificationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceWarranty", DbType.Int32, invItemBO.ServiceWarranty);
                        dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, invItemBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(commandMaster, "@ItemId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(commandMaster) > 0 ? true : false;
                        tmpItemId = Convert.ToInt32(commandMaster.Parameters["@ItemId"].Value);

                        //dbSmartAspects.AddInParameter(commandMaster, "@CategoryId", DbType.Int32, comboItemBO.CategoryId);
                        //dbSmartAspects.AddInParameter(commandMaster, "@ComboName", DbType.String, comboItemBO.ComboName);
                        //dbSmartAspects.AddInParameter(commandMaster, "@ComboPrice", DbType.Decimal, comboItemBO.ComboPrice);
                        //dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, comboItemBO.CreatedBy);
                        //dbSmartAspects.AddOutParameter(commandMaster, "@ComboId", DbType.Int32, sizeof(Int32));
                        //dbSmartAspects.AddInParameter(commandMaster, "@Code", DbType.String, comboItemBO.Code);
                        //dbSmartAspects.AddInParameter(commandMaster, "@ImageName", DbType.String, comboItemBO.ImageName);

                        //status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        //tmpComboId = Convert.ToInt32(commandMaster.Parameters["@ComboId"].Value);

                        if (status)
                        {
                            int count = 0;

                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveInvItemDetailsInfo_SP"))
                            {
                                foreach (InvItemDetailsBO comboDetailBO in detailBO)
                                {
                                    if (comboDetailBO.ItemId == 0)
                                    {
                                        commandDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemId", DbType.Int32, tmpItemId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemDetailId", DbType.Int32, comboDetailBO.ItemDetailId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemUnit", DbType.Decimal, comboDetailBO.ItemUnit);

                                        count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }

                            if (count == detailBO.Count)
                            {
                                int costCount = costCenterList.Count;
                                for (int i = 0; i < costCount; i++)
                                {
                                    costCenterList[i].ItemId = tmpItemId;
                                }

                                if (costCount > 0)
                                {
                                    Boolean costStatus = SaveCostCenterList(costCenterList);
                                }

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
                            retVal = false;
                        }
                    }
                }
            }
            retVal = SaveItemCategoryImage(tmpItemId, invItemBO.RandomItemId);
            return retVal;
        }
        private bool SaveCostCenterList(List<InvItemCostCenterMappingBO> costCenterList)
        {
            bool status = false;
            InvItemCostCenterMappingDA costDA = new InvItemCostCenterMappingDA();
            int costCount = costCenterList.Count;
            int mappingId;
            for (int i = 0; i < costCount; i++)
            {
                status = costDA.SaveInvItemCostCenterMappingInfo(costCenterList[i], out mappingId);
            }

            return status;
        }
        public Boolean UpdateRestaurantComboInfo(InvItemBO invItemBO, List<InvItemCostCenterMappingBO> costCenterList, List<InvItemDetailsBO> detailBO, ArrayList arrayDelete)
        {
            bool retVal = false;
            Boolean status = false;
            int tmpItemId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateInvItemInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@ItemId", DbType.Int32, invItemBO.ItemId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ItemType", DbType.String, invItemBO.ItemType);
                        dbSmartAspects.AddInParameter(commandMaster, "@Name", DbType.String, invItemBO.Name);
                        dbSmartAspects.AddInParameter(commandMaster, "@Code", DbType.String, invItemBO.Code);
                        dbSmartAspects.AddInParameter(commandMaster, "@Description", DbType.String, invItemBO.Description);
                        dbSmartAspects.AddInParameter(commandMaster, "@CategoryId", DbType.Int32, invItemBO.CategoryId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ManufacturerId", DbType.Int32, invItemBO.ManufacturerId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ProductType", DbType.String, invItemBO.ProductType);
                        dbSmartAspects.AddInParameter(commandMaster, "@PurchasePrice", DbType.Decimal, invItemBO.PurchasePrice);
                        dbSmartAspects.AddInParameter(commandMaster, "@SellingLocalCurrencyId", DbType.Int32, invItemBO.SellingLocalCurrencyId);
                        dbSmartAspects.AddInParameter(commandMaster, "@UnitPriceLocal", DbType.Decimal, invItemBO.UnitPriceLocal);
                        dbSmartAspects.AddInParameter(commandMaster, "@SellingUsdCurrencyId", DbType.Int32, invItemBO.SellingUsdCurrencyId);
                        dbSmartAspects.AddInParameter(commandMaster, "@UnitPriceUsd", DbType.Decimal, invItemBO.UnitPriceUsd);
                        dbSmartAspects.AddInParameter(commandMaster, "@StockType", DbType.String, invItemBO.StockType);
                        dbSmartAspects.AddInParameter(commandMaster, "@StockBy", DbType.Int32, invItemBO.StockBy);
                        dbSmartAspects.AddInParameter(commandMaster, "@ClassificationId", DbType.Int32, invItemBO.ClassificationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceWarranty", DbType.Int32, invItemBO.ServiceWarranty);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, invItemBO.LastModifiedBy);
                        status = dbSmartAspects.ExecuteNonQuery(commandMaster) > 0 ? true : false;
                        //dbSmartAspects.AddInParameter(commandMaster, "@ComboId", DbType.Int32, comboItemBO.ComboId);
                        //dbSmartAspects.AddInParameter(commandMaster, "@CategoryId", DbType.Int32, comboItemBO.CategoryId);
                        //dbSmartAspects.AddInParameter(commandMaster, "@ComboName", DbType.String, comboItemBO.ComboName);
                        //dbSmartAspects.AddInParameter(commandMaster, "@ComboPrice", DbType.Decimal, comboItemBO.ComboPrice);
                        //dbSmartAspects.AddInParameter(commandMaster, "@Code", DbType.String, comboItemBO.Code);
                        //dbSmartAspects.AddInParameter(commandMaster, "@ImageName", DbType.String, comboItemBO.ImageName);
                        //dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, comboItemBO.LastModifiedBy);

                        //status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        tmpItemId = invItemBO.ItemId;

                        if (status)
                        {
                            int count = 0;
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveInvItemDetailsInfo_SP"))
                            {
                                foreach (InvItemDetailsBO comboDetailBO in detailBO)
                                {
                                    if (comboDetailBO.ItemId == 0)
                                    {
                                        commandDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemId", DbType.Int32, tmpItemId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemDetailId", DbType.Int32, comboDetailBO.ItemDetailId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemUnit", DbType.Decimal, comboDetailBO.ItemUnit);
                                        count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }

                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateInvItemDetailsInfo_SP"))
                            {
                                foreach (InvItemDetailsBO comboDetailBO in detailBO)
                                {
                                    if (comboDetailBO.ItemId != 0)
                                    {
                                        commandDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandDetails, "@DetailId", DbType.Int32, comboDetailBO.DetailId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemId", DbType.Int32, tmpItemId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemDetailId", DbType.Int32, comboDetailBO.ItemDetailId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemUnit", DbType.Decimal, comboDetailBO.ItemUnit);

                                        count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }

                            if (count == detailBO.Count)
                            {
                                int statusDelete = 0;
                                if (arrayDelete.Count > 0)
                                {
                                    foreach (int delId in arrayDelete)
                                    {
                                        using (DbCommand commandDelete = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                        {
                                            dbSmartAspects.AddInParameter(commandDelete, "@TableName", DbType.String, "InvItemDetails");
                                            dbSmartAspects.AddInParameter(commandDelete, "@TablePKField", DbType.String, "DetailId");
                                            dbSmartAspects.AddInParameter(commandDelete, "@TablePKId", DbType.String, delId);

                                            statusDelete = dbSmartAspects.ExecuteNonQuery(commandDelete);
                                        }
                                    }
                                }
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
                            retVal = false;
                        }
                    }
                }
            }
            //  retVal = SaveItemCategoryImage(comboItemBO.ComboId, comboItemBO.RandomComboId);

            Boolean costStatus = UpdateRestaurantComboCostCenterMappingInfo(costCenterList);
            return retVal;
        }
        private bool UpdateRestaurantComboCostCenterMappingInfo(List<InvItemCostCenterMappingBO> costCenterList)
        {
            InvItemCostCenterMappingDA costDA = new InvItemCostCenterMappingDA();
            List<InvItemCostCenterMappingBO> dbCostList = new List<InvItemCostCenterMappingBO>();
            dbCostList = costDA.GetInvItemCostCenterMappingByItemId(costCenterList[0].ItemId);

            // Both In
            List<int> idList = new List<int>();
            int mappingId;
            for (int i = 0; i < costCenterList.Count; i++)
            {

                int succ = IsAvailableInDb(dbCostList, costCenterList[i]);
                if (succ > 0)
                {
                    //Update
                    costCenterList[i].MappingId = succ;
                    bool status = costDA.UpdateInvItemCostCenterMappingInfo(costCenterList[i]);
                    idList.Add(succ);
                }
                else
                {
                    //Insert
                    bool status = costDA.SaveInvItemCostCenterMappingInfo(costCenterList[i], out mappingId);
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

            Boolean deleteStatus = costDA.DeleteAllInvItemCostCenterMappingInfoWithoutMappingIdList(costCenterList[0].ItemId, saveAndUpdatedIdList);

            return true;
        }
        private int IsAvailableInDb(List<InvItemCostCenterMappingBO> dbCostList, InvItemCostCenterMappingBO costBO)
        {
            int isInDB = 0;
            for (int j = 0; j < dbCostList.Count; j++)
            {
                if (dbCostList[j].ItemId == costBO.ItemId && costBO.CostCenterId == dbCostList[j].CostCenterId)
                {
                    isInDB = dbCostList[j].MappingId;
                }
            }
            return isInDB;
        }
        public Boolean SaveItemCategoryImage(int categoryId, int randomCategoryId)
        {
            Boolean status = false;
            status = DeletePreviousCategoryImage(categoryId);
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandDocument = dbSmartAspects.GetStoredProcCommand("UpdateEmployeeDocumentAndSignature_SP"))
                {
                    commandDocument.Parameters.Clear();
                    dbSmartAspects.AddInParameter(commandDocument, "@EmpId", DbType.Int32, categoryId);
                    dbSmartAspects.AddInParameter(commandDocument, "@RandomId", DbType.String, randomCategoryId);
                    status = dbSmartAspects.ExecuteNonQuery(commandDocument) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean DeletePreviousCategoryImage(int categoryId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteTemporaryImage_Sp"))
                {
                    dbSmartAspects.AddInParameter(command, "@Id", DbType.String, categoryId);
                    dbSmartAspects.AddInParameter(command, "@Type", DbType.String, "RestaurentCombo");
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                }
            }
            return status;
        }
        public RestaurantComboItemBO GetRestaurantComboInfoById(int comboId)
        {
            RestaurantComboItemBO comboItemBO = new RestaurantComboItemBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantComboInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ComboId", DbType.Int32, comboId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                comboItemBO.ComboId = Convert.ToInt32(reader["ComboId"]);
                                comboItemBO.ComboName = reader["ComboName"].ToString();
                                comboItemBO.ComboPrice = Convert.ToDecimal(reader["ComboPrice"]);
                                comboItemBO.Code = reader["Code"].ToString();
                                comboItemBO.ImageName = reader["ImageName"].ToString();
                                comboItemBO.ComboPrice = Convert.ToDecimal(reader["ComboPrice"]);
                            }
                        }
                    }
                }
            }
            return comboItemBO;
        }
        public Boolean DeleteComboDetailInfoByComboId(int comboId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteComboDetailInfoByComboId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ComboId", DbType.Int32, comboId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<RestaurantComboItemBO> GetRestaurantComboItemInfoBySearchCriteria(string ComboName, string Code, int CostCenterId)
        {
            string searchCriteria = string.Empty;
            searchCriteria = GenarateWhereCondition(ComboName, Code, CostCenterId);
            List<RestaurantComboItemBO> roomOwnerList = new List<RestaurantComboItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantComboItemInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, searchCriteria);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantComboItemBO comboItemBO = new RestaurantComboItemBO();
                                comboItemBO.ComboId = Convert.ToInt32(reader["ComboId"]);
                                comboItemBO.ComboName = reader["ComboName"].ToString();
                                comboItemBO.ComboPrice = Convert.ToDecimal(reader["ComboPrice"]);
                                comboItemBO.Code = reader["Code"].ToString();
                                comboItemBO.ImageName = reader["ImageName"].ToString();
                                roomOwnerList.Add(comboItemBO);
                            }
                        }
                    }
                }
            }
            return roomOwnerList;
        }
        public string GenarateWhereCondition(string ComboName, string Code, int CostCenterId)
        {
            string Where = string.Empty;
            string Condition = string.Empty;
            if (!string.IsNullOrEmpty(ComboName))
            {
                Condition += " ComboName =" + "'" + ComboName + "'" + "";
            }
            if (!string.IsNullOrEmpty(Code))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND Code=" + "'" + Code + "'" + "";
                }
                else
                {
                    Condition += " Code=" + "'" + Code + "'" + "";
                }
            }
            //if (!string.IsNullOrEmpty(CostCenterId.ToString()))
            //{
            //    if (CostCenterId != 0)
            //    {
            //        if (!string.IsNullOrEmpty(Condition))
            //        {
            //            Condition += " AND CostCenterId=" + "'" + CostCenterId + "'" + "";
            //        }
            //        else
            //        {
            //            Condition += " CostCenterId=" + "'" + CostCenterId + "'" + "";
            //        }
            //    }
            //}
            if (!string.IsNullOrWhiteSpace(Condition))
            {
                Where = " WHERE " + Condition;
            }
            else
            {
                Where = Condition;
            }
            return Where;
        }

    }
}
