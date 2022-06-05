using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class EmpBonusDA : BaseService
    {
        public Boolean SaveEmpPeriodicBonusInfo(EmpBonusBO bonusBO, out int tmpId)
        {
            Boolean status = false;
            tmpId = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpBonusInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@BonusType", DbType.String, bonusBO.BonusType);
                        dbSmartAspects.AddInParameter(command, "@BonusAmount", DbType.Decimal, bonusBO.BonusAmount);
                        dbSmartAspects.AddInParameter(command, "@AmountType", DbType.String, bonusBO.AmountType);
                        dbSmartAspects.AddInParameter(command, "@DependsOnHead", DbType.Int32, bonusBO.DependsOnHead);
                        dbSmartAspects.AddInParameter(command, "@EffectivePeriod", DbType.Int32, bonusBO.EffectivePeriod);
                        dbSmartAspects.AddInParameter(command, "@BonusDate", DbType.DateTime, bonusBO.BonusDate);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, bonusBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@BonusSettingId", DbType.Int32, bonusBO.BonusSettingId);
                        //dbSmartAspects.AddInParameter(command, "@CreatedDate", DbType.DateTime, bonusBO.CreatedDate);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpId = Convert.ToInt32(command.Parameters["@BonusSettingId"].Value);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public Boolean UpdateEmpPeriodicBonusInfo(EmpBonusBO bonusBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpBonusInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@BonusSettingId", DbType.Int32, bonusBO.BonusSettingId);
                        dbSmartAspects.AddInParameter(command, "@BonusType", DbType.String, bonusBO.BonusType);
                        dbSmartAspects.AddInParameter(command, "@BonusAmount", DbType.Decimal, bonusBO.BonusAmount);
                        dbSmartAspects.AddInParameter(command, "@AmountType", DbType.String, bonusBO.AmountType);
                        dbSmartAspects.AddInParameter(command, "@DependsOnHead", DbType.Int32, bonusBO.DependsOnHead);
                        dbSmartAspects.AddInParameter(command, "@EffectivePeriod", DbType.Int32, bonusBO.EffectivePeriod);
                        dbSmartAspects.AddInParameter(command, "@BonusDate", DbType.DateTime, bonusBO.BonusDate);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, bonusBO.LastModifiedBy);
                        //dbSmartAspects.AddInParameter(command, "@LastModifiedDate", DbType.DateTime, bonusBO.LastModifiedDate);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public Boolean SaveOrDeleteEmpFestivalBonusInfo(List<EmpBonusBO> deleteList, List<EmpBonusBO> addList)
        {
            Boolean status = false;
            int addcount = 0;
            int deleteCount = 0;
            try
            {
                if (addList.Count > 0)
                {
                    using (DbConnection conn = dbSmartAspects.CreateConnection())
                    {
                        foreach (EmpBonusBO bonusBO in addList)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpBonusInfo_SP"))
                            {

                                dbSmartAspects.AddInParameter(command, "@BonusType", DbType.String, bonusBO.BonusType);
                                dbSmartAspects.AddInParameter(command, "@BonusAmount", DbType.Decimal, bonusBO.BonusAmount);
                                dbSmartAspects.AddInParameter(command, "@AmountType", DbType.String, bonusBO.AmountType);
                                dbSmartAspects.AddInParameter(command, "@DependsOnHead", DbType.Int32, bonusBO.DependsOnHead);
                                dbSmartAspects.AddInParameter(command, "@EffectivePeriod", DbType.Int32, bonusBO.EffectivePeriod);
                                dbSmartAspects.AddInParameter(command, "@BonusDate", DbType.DateTime, bonusBO.BonusDate);
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, bonusBO.CreatedBy);
                                //dbSmartAspects.AddInParameter(command, "@CreatedDate", DbType.DateTime, bonusBO.CreatedDate);
                                addcount += dbSmartAspects.ExecuteNonQuery(command);
                                // status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                            }
                        }
                        if (addcount == addList.Count)
                        {
                            status = true;
                        }
                        else
                        {
                            status = false;
                        }
                    }
                }
                if (deleteList.Count > 0)
                {
                    using (DbConnection conn = dbSmartAspects.CreateConnection())
                    {
                        foreach (EmpBonusBO bonusBO in deleteList)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, "PayrollBonusSetting");
                                dbSmartAspects.AddInParameter(command, "@TablePKField", DbType.String, "BonusSettingId");
                                dbSmartAspects.AddInParameter(command, "@TablePKId", DbType.String, bonusBO.BonusSettingId);

                                //dbSmartAspects.AddInParameter(command, "@BonusSettingId", DbType.Int32, bonusBO.BonusSettingId);
                                deleteCount += dbSmartAspects.ExecuteNonQuery(command);
                            }
                        }
                        if (deleteCount == deleteList.Count)
                        {
                            status = true;
                        }
                        else
                        {
                            status = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        //public Boolean UpdateEmpFestivalBonusInfo(List<EmpBonusBO> bonusList)
        //{
        //    Boolean status = false;
        //    int count = 0;
        //    try
        //    {
        //        using (DbConnection conn = dbSmartAspects.CreateConnection())
        //        {
        //            foreach (EmpBonusBO bonusBO in bonusList)
        //            {
        //                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpBonusInfo_SP"))
        //                {
        //                    dbSmartAspects.AddInParameter(command, "@BonusSettingId", DbType.Int32, bonusBO.BonusSettingId);
        //                    dbSmartAspects.AddInParameter(command, "@BonusType", DbType.String, bonusBO.BonusType);
        //                    dbSmartAspects.AddInParameter(command, "@BonusAmount", DbType.Decimal, bonusBO.BonusAmount);
        //                    dbSmartAspects.AddInParameter(command, "@AmountType", DbType.String, bonusBO.AmountType);
        //                    dbSmartAspects.AddInParameter(command, "@DependsOnHead", DbType.Int32, bonusBO.DependsOnHead);
        //                    dbSmartAspects.AddInParameter(command, "@EffectivePeriod", DbType.Int32, bonusBO.EffectivePeriod);
        //                    dbSmartAspects.AddInParameter(command, "@BonusDate", DbType.DateTime, bonusBO.BonusDate);
        //                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, bonusBO.CreatedBy);
        //                    //dbSmartAspects.AddInParameter(command, "@CreatedDate", DbType.DateTime, bonusBO.CreatedDate);
        //                    count += dbSmartAspects.ExecuteNonQuery(command);
        //                    // status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

        //                }
        //            }
        //            if (count == bonusList.Count)
        //            {
        //                status = true;
        //            }
        //            else
        //            {
        //                status = false;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    return status;
        //}

        public List<EmpBonusBO> GetBonusList(string bonusType)
        {
            List<EmpBonusBO> bonusList = new List<EmpBonusBO>();

            DataSet bonusDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpBonusInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BonusType", DbType.String, bonusType);

                    dbSmartAspects.LoadDataSet(cmd, bonusDS, "BonusList");
                    DataTable table = bonusDS.Tables["BonusList"];

                    bonusList = table.AsEnumerable().Select(r =>
                                   new EmpBonusBO
                                   {
                                       BonusSettingId = r.Field<int>("BonusSettingId"),
                                       BonusType = r.Field<string>("BonusType"),
                                       BonusAmount = r.Field<decimal>("BonusAmount"),
                                       AmountType = r.Field<string>("AmountType"),
                                       DependsOnHead = r.Field<int>("DependsOnHead"),
                                       ViewBonusDate = r.Field<string>("BonusDate"),
                                       EffectivePeriod = r.Field<byte?>("EffectivePeriod")

                                   }).ToList();
                }
            }

            return bonusList;
        }
    }
}
