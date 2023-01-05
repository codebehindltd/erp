using HotelManagement.Entity.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.Inventory
{
    public class InvNutrientInfoDA : BaseService
    {
        public bool SaveNutritionTyepInfo(InvNutrientInfoBO NutritionTypeInfo)
        {
            Int64 status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        if(NutritionTypeInfo.NutritionTypeId > 0)
                        {
                            using (DbCommand cmdSave = dbSmartAspects.GetStoredProcCommand("UpdateNutritionTyepInfo_SP"))
                            {
                                cmdSave.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmdSave, "@NutritionTypeId", DbType.Int64, NutritionTypeInfo.NutritionTypeId);
                                dbSmartAspects.AddInParameter(cmdSave, "@Code", DbType.String, NutritionTypeInfo.Code);
                                dbSmartAspects.AddInParameter(cmdSave, "@Name", DbType.String, NutritionTypeInfo.Name);
                                dbSmartAspects.AddInParameter(cmdSave, "@Remarks", DbType.String, NutritionTypeInfo.Remarks);
                                dbSmartAspects.AddInParameter(cmdSave, "@ActiveStat", DbType.Boolean, NutritionTypeInfo.ActiveStat);
                                dbSmartAspects.AddInParameter(cmdSave, "@CreatedBy", DbType.Int32, NutritionTypeInfo.CreatedBy);

                                status = dbSmartAspects.ExecuteNonQuery(cmdSave, transction);
                            }
                        }
                        else
                        {
                            using (DbCommand cmdSave = dbSmartAspects.GetStoredProcCommand("SaveNutritionTyepInfo_SP"))
                            {
                                cmdSave.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmdSave, "@Code", DbType.String, NutritionTypeInfo.Code);
                                dbSmartAspects.AddInParameter(cmdSave, "@Name", DbType.String, NutritionTypeInfo.Name);
                                dbSmartAspects.AddInParameter(cmdSave, "@Remarks", DbType.String, NutritionTypeInfo.Remarks);
                                dbSmartAspects.AddInParameter(cmdSave, "@ActiveStat", DbType.Boolean, NutritionTypeInfo.ActiveStat);
                                dbSmartAspects.AddInParameter(cmdSave, "@CreatedBy", DbType.Int32, NutritionTypeInfo.CreatedBy);

                                status = dbSmartAspects.ExecuteNonQuery(cmdSave, transction);
                            }
                        }

                        if (status > 0)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                        }
                    }
                    catch
                    {
                        retVal = false;
                        transction.Rollback();
                    }
                }
            }

            return retVal;
        }
        
        public bool SaveNutrientInfo(InvNutrientInfoBO nutrientInfo)
        {
            Int64 status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        if(nutrientInfo.NutrientId > 0)
                        {
                            using (DbCommand cmdSave = dbSmartAspects.GetStoredProcCommand("UpdateNutrientInfo_SP"))
                            {
                                cmdSave.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmdSave, "@NutrientId", DbType.Int64, nutrientInfo.NutrientId);
                                dbSmartAspects.AddInParameter(cmdSave, "@NutritionTypeId", DbType.Int64, nutrientInfo.NutritionTypeId);
                                dbSmartAspects.AddInParameter(cmdSave, "@Code", DbType.String, nutrientInfo.Code);
                                dbSmartAspects.AddInParameter(cmdSave, "@Name", DbType.String, nutrientInfo.Name);
                                dbSmartAspects.AddInParameter(cmdSave, "@Remarks", DbType.String, nutrientInfo.Remarks);
                                dbSmartAspects.AddInParameter(cmdSave, "@ActiveStat", DbType.Boolean, nutrientInfo.ActiveStat);
                                dbSmartAspects.AddInParameter(cmdSave, "@CreatedBy", DbType.Int32, nutrientInfo.CreatedBy);

                                status = dbSmartAspects.ExecuteNonQuery(cmdSave, transction);
                            }
                        }
                        else
                        {
                            using (DbCommand cmdSave = dbSmartAspects.GetStoredProcCommand("SaveNutrientInfo_SP"))
                            {
                                cmdSave.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmdSave, "@NutritionTypeId", DbType.Int64, nutrientInfo.NutritionTypeId);
                                dbSmartAspects.AddInParameter(cmdSave, "@Code", DbType.String, nutrientInfo.Code);
                                dbSmartAspects.AddInParameter(cmdSave, "@Name", DbType.String, nutrientInfo.Name);
                                dbSmartAspects.AddInParameter(cmdSave, "@Remarks", DbType.String, nutrientInfo.Remarks);
                                dbSmartAspects.AddInParameter(cmdSave, "@ActiveStat", DbType.Boolean, nutrientInfo.ActiveStat);
                                dbSmartAspects.AddInParameter(cmdSave, "@CreatedBy", DbType.Int32, nutrientInfo.CreatedBy);

                                status = dbSmartAspects.ExecuteNonQuery(cmdSave, transction);
                            }
                        }

                        if (status > 0)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                        }
                    }
                    catch
                    {
                        retVal = false;
                        transction.Rollback();
                    }
                }
            }

            return retVal;
        }
        
        public bool SaveNutrientsAmount(List<InvNutrientInfoBO> AddedNutrients, int userInfoId)
        {
            Int64 status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (InvNutrientInfoBO ni in AddedNutrients)
                        {
                            if(ni.NutrientAmount > 0)
                            {
                                using (DbCommand cmdSave = dbSmartAspects.GetStoredProcCommand("SaveNutrientsAmount_SP"))
                                {
                                    cmdSave.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdSave, "@ItemId", DbType.Int32, ni.ItemId);
                                    dbSmartAspects.AddInParameter(cmdSave, "@NutritionTypeId", DbType.Int32, ni.NutritionTypeId);
                                    dbSmartAspects.AddInParameter(cmdSave, "@NutrientId", DbType.Int32, ni.NutrientId);
                                    dbSmartAspects.AddInParameter(cmdSave, "@NutrientAmount", DbType.Decimal, ni.NutrientAmount);
                                    dbSmartAspects.AddInParameter(cmdSave, "@Formula", DbType.String, ni.Formula);
                                    dbSmartAspects.AddInParameter(cmdSave, "@CreatedBy", DbType.Int32, userInfoId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdSave, transction);
                                }
                            }
                            else
                            {
                                if(ni.Formula == null)
                                {
                                    using (DbCommand cmdSave = dbSmartAspects.GetStoredProcCommand("SaveOrDeleteNutrientsAmount_SP"))
                                    {
                                        cmdSave.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(cmdSave, "@ItemId", DbType.Int32, ni.ItemId);
                                        dbSmartAspects.AddInParameter(cmdSave, "@NutritionTypeId", DbType.Int32, ni.NutritionTypeId);
                                        dbSmartAspects.AddInParameter(cmdSave, "@NutrientId", DbType.Int32, ni.NutrientId);
                                        dbSmartAspects.AddInParameter(cmdSave, "@NutrientAmount", DbType.Decimal, ni.NutrientAmount);
                                        dbSmartAspects.AddInParameter(cmdSave, "@Formula", DbType.String, ni.Formula);
                                        dbSmartAspects.AddInParameter(cmdSave, "@CreatedBy", DbType.Int32, userInfoId);

                                        status = dbSmartAspects.ExecuteNonQuery(cmdSave, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                        }
                    }
                    catch
                    {
                        retVal = false;
                        transction.Rollback();
                    }
                }
            }

            return retVal;
        }
        public List<InvNutrientInfoBO> GetNutritionType()
        {
            List<InvNutrientInfoBO> nutritionTypeList = new List<InvNutrientInfoBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNutritionType_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvNutrientInfoBO nutritionType = new InvNutrientInfoBO();

                                nutritionType.NutritionTypeId = Convert.ToInt64(reader["NutritionTypeId"]);
                                nutritionType.Code = reader["Code"].ToString();
                                nutritionType.Name = reader["Name"].ToString();
                                nutritionType.Remarks = reader["Remarks"].ToString();
                                nutritionTypeList.Add(nutritionType);
                            }
                        }
                    }
                }
            }
            return nutritionTypeList;
        }
        public List<InvNutrientInfoBO> GetNutrientInformations()
        {
            List<InvNutrientInfoBO> nutrientList = new List<InvNutrientInfoBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNutrientInformations_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvNutrientInfoBO nutrient = new InvNutrientInfoBO();

                                nutrient.NutrientId = Convert.ToInt64(reader["NutrientId"]);
                                nutrient.NutritionTypeId = Convert.ToInt64(reader["NutritionTypeId"]);
                                nutrient.Code = reader["Code"].ToString();
                                nutrient.Name = reader["Name"].ToString();
                                nutrient.Remarks = reader["Remarks"].ToString();
                                nutrient.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                nutrientList.Add(nutrient);
                            }
                        }
                    }
                }
            }
            return nutrientList;
        }
        
        public List<InvNutrientInfoBO> GetNutrientAmounts()
        {
            List<InvNutrientInfoBO> nutrientAmountList = new List<InvNutrientInfoBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNutrientAmounts_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvNutrientInfoBO nutrientAmount = new InvNutrientInfoBO();

                                nutrientAmount.ItemId = Convert.ToInt32(reader["RecipeItemId"]);
                                nutrientAmount.NutrientId = Convert.ToInt32(reader["NutrientsId"]);
                                nutrientAmount.NutritionTypeId = Convert.ToInt32(reader["NTypeId"]);
                                nutrientAmount.Formula = reader["Formula"].ToString();
                                nutrientAmount.NutrientAmount = Convert.ToDecimal(reader["FormulaValue"]);
                                nutrientAmountList.Add(nutrientAmount);
                            }
                        }
                    }
                }
            }
            return nutrientAmountList;
        }

        public List<InvNutrientInfoBO> GetNutrientInformationForSearch(InvNutrientInfoBO nutrientInfo, Int32 userInfoId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<InvNutrientInfoBO> nutritionInfo = new List<InvNutrientInfoBO>();
            totalRecords = 0;
            if(nutrientInfo.SetupTypeId == 1)
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {                    
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNutritionTypeInformationForSearch_SP"))
                    {
                        if (!string.IsNullOrEmpty(nutrientInfo.Code))
                            dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, nutrientInfo.Code);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(nutrientInfo.Name))
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, nutrientInfo.Name);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, nutrientInfo.ActiveStat);
                        dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    InvNutrientInfoBO nutritionType = new InvNutrientInfoBO();

                                    nutritionType.NutritionTypeId = Convert.ToInt64(reader["Id"]);
                                    nutritionType.Code = reader["Code"].ToString();
                                    nutritionType.Name = reader["Name"].ToString();
                                    nutritionType.Remarks = reader["Remarks"].ToString();
                                    nutritionType.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                    nutritionType.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                    nutritionInfo.Add(nutritionType);
                                }
                            }
                        }

                        totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value.ToString());
                    }
                }
            }
            else if (nutrientInfo.SetupTypeId == 2)
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNutrientInformation_SP"))
                    {
                        if (nutrientInfo.NutritionTypeId == 0)
                        {
                            dbSmartAspects.AddInParameter(cmd, "@NutritionTypeId", DbType.Int32, DBNull.Value);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@NutritionTypeId", DbType.Int64, nutrientInfo.NutritionTypeId);
                        }

                        if (!string.IsNullOrEmpty(nutrientInfo.Code))
                            dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, nutrientInfo.Code);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(nutrientInfo.Name))
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, nutrientInfo.Name);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, nutrientInfo.ActiveStat);
                        dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    InvNutrientInfoBO nutritionType = new InvNutrientInfoBO();

                                    nutritionType.NutrientId = Convert.ToInt64(reader["Id"]);
                                    nutritionType.NutritionTypeId = Convert.ToInt64(reader["NTypeId"]);
                                    nutritionType.Code = reader["Code"].ToString();
                                    nutritionType.Name = reader["Name"].ToString();
                                    nutritionType.Remarks = reader["Remarks"].ToString();
                                    nutritionType.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                    nutritionType.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                    nutritionInfo.Add(nutritionType);
                                }
                            }
                        }

                        totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value.ToString());
                    }
                }
            }
            return nutritionInfo;
        }

        public InvNutrientInfoBO GetNutrientInfoById(long NutrientId, long NutritionTypeId)
        {
            InvNutrientInfoBO inInfo = new InvNutrientInfoBO();
            if(NutrientId == 0)
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNutrientInfoByNutritionTypeId_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@NutritionTypeId", DbType.Int64, NutritionTypeId);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    inInfo.NutritionTypeId = Convert.ToInt64(reader["Id"]);
                                    inInfo.Code = reader["Code"].ToString();
                                    inInfo.Name = reader["Name"].ToString();
                                    inInfo.Remarks = reader["Remarks"].ToString();
                                    inInfo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNutrientInfoByNutrientId_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@NutrientId", DbType.Int64, NutrientId);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    inInfo.NutrientId = Convert.ToInt64(reader["Id"]);
                                    inInfo.NutritionTypeId = Convert.ToInt64(reader["NTypeId"]);                                    
                                    inInfo.Code = reader["Code"].ToString();
                                    inInfo.Name = reader["Name"].ToString();
                                    inInfo.Remarks = reader["Remarks"].ToString();
                                    inInfo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                }
                            }
                        }
                    }
                }
            }
            
            return inInfo;
        }

        public bool NutrientInfoDelete(long NutrientId, long NutritionTypeId)
        {
            Int64 status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand cmdDelete = dbSmartAspects.GetStoredProcCommand("NutrientInfoDelete_SP"))
                        {
                            cmdDelete.Parameters.Clear();

                            dbSmartAspects.AddInParameter(cmdDelete, "@NutrientId", DbType.Int64, NutrientId);
                            dbSmartAspects.AddInParameter(cmdDelete, "@NutritionTypeId", DbType.Int64, NutritionTypeId);

                            status = dbSmartAspects.ExecuteNonQuery(cmdDelete, transction);
                        }


                        if (status > 0)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                        }
                    }
                    catch
                    {
                        retVal = false;
                        transction.Rollback();
                    }
                }
            }

            return retVal;
        }
    }
}
