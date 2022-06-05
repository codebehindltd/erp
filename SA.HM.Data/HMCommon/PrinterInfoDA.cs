using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HMCommon
{
    public class PrinterInfoDA : BaseService
    {
        public List<PrinterInfoBO> GetRestaurentItemTypeInfo()
        {
            List<PrinterInfoBO> itemList = new List<PrinterInfoBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPrinterInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PrinterInfoBO itemType = new PrinterInfoBO();

                                itemType.ActiveStatus = reader["ActiveStatus"].ToString();
                                itemType.PrinterInfoId = Convert.ToInt32(reader["PrinterInfoId"]);
                                itemType.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                itemType.StockType = reader["StockType"].ToString();
                                itemType.PrinterName = reader["PrinterName"].ToString();
                                itemType.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                itemList.Add(itemType);
                            }
                        }
                    }
                }
            }
            return itemList;
        }
        public Boolean SavePrinterInfo(PrinterInfoBO itemTypeBO, out int tmpTypeId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePrinterInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, itemTypeBO.CostCenterId);
                    dbSmartAspects.AddInParameter(command, "@StockType", DbType.String, itemTypeBO.StockType);
                    dbSmartAspects.AddInParameter(command, "@KitchenId", DbType.Int32, itemTypeBO.KitchenId);
                    dbSmartAspects.AddInParameter(command, "@PrinterName", DbType.String, itemTypeBO.PrinterName);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, itemTypeBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, itemTypeBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@TypeId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpTypeId = Convert.ToInt32(command.Parameters["@TypeId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdatePrinterInfo(PrinterInfoBO itemBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePrinterInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@PrinterInfoId", DbType.Int32, itemBO.PrinterInfoId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, itemBO.CostCenterId);
                    dbSmartAspects.AddInParameter(command, "@StockType", DbType.String, itemBO.StockType);
                    dbSmartAspects.AddInParameter(command, "@KitchenId", DbType.Int32, itemBO.KitchenId);
                    dbSmartAspects.AddInParameter(command, "@PrinterName", DbType.String, itemBO.PrinterName);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, itemBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, itemBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public PrinterInfoBO GetPrinterInfoById(int printerInfoId)
        {
            PrinterInfoBO typeBO = new PrinterInfoBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPrinterInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PrinterInfoId", DbType.Int32, printerInfoId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                typeBO.PrinterInfoId = Convert.ToInt32(reader["PrinterInfoId"]);
                                typeBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                typeBO.StockType = reader["StockType"].ToString();
                                typeBO.KitchenId = Convert.ToInt32(reader["KitchenId"]);
                                typeBO.PrinterName = reader["PrinterName"].ToString();
                                typeBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                typeBO.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return typeBO;
        }
        public List<PrinterInfoBO> GetRestaurentItemTypeInfoByKotId(int kotId)
        {
            List<PrinterInfoBO> itemList = new List<PrinterInfoBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurentItemTypeInfoByKotId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PrinterInfoBO typeBO = new PrinterInfoBO();

                                //typeBO.PrinterInfoId = Convert.ToInt32(reader["PrinterInfoId"]);
                                typeBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                typeBO.CostCenter = reader["CostCenter"].ToString();
                                typeBO.KitchenId = Convert.ToInt32(reader["KitchenId"]);
                                typeBO.KitchenOrStockName = reader["KitchenOrStockName"].ToString();
                                typeBO.PrinterName = reader["PrinterName"].ToString();
                                typeBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                typeBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                typeBO.StockType = reader["StockType"].ToString();
                                typeBO.IsChanged = Convert.ToBoolean(reader["IsChanged"]);
                                typeBO.DefaultView = reader["DefaultView"].ToString();
                                itemList.Add(typeBO);
                            }
                        }
                    }
                }
            }
            return itemList;
        }
        public List<PrinterInfoBO> GetRestaurentItemTypeInfoByKotIdForReprint(int kotId)
        {
            List<PrinterInfoBO> itemList = new List<PrinterInfoBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurentItemTypeInfoByKotIdForReprint_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PrinterInfoBO typeBO = new PrinterInfoBO();

                                //typeBO.PrinterInfoId = Convert.ToInt32(reader["PrinterInfoId"]);
                                typeBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                typeBO.CostCenter = reader["CostCenter"].ToString();
                                typeBO.KitchenId = Convert.ToInt32(reader["KitchenId"]);
                                typeBO.KitchenOrStockName = reader["KitchenOrStockName"].ToString();
                                typeBO.PrinterName = reader["PrinterName"].ToString();
                                typeBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                typeBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                typeBO.StockType = reader["StockType"].ToString();
                                typeBO.PrintFlag = Convert.ToBoolean(reader["PrintFlag"]);
                                typeBO.DefaultView = reader["DefaultView"].ToString();
                                //itemType.ActiveStatus = reader["ActiveStatus"].ToString();
                                //itemType.TypeId = Convert.ToInt32(reader["TypeId"]);
                                //itemType.TypeName = reader["TypeName"].ToString();
                                //itemType.PrinterName = reader["PrinterName"].ToString();
                                //itemType.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                itemList.Add(typeBO);
                            }
                        }
                    }
                }
            }
            return itemList;
        }
        public List<PrinterInfoBO> GetRestaurentItemTypeInfoBySearchCriteria(int CostCenterId, string PrinterName, bool ActiveStat)
        {
            List<PrinterInfoBO> itemList = new List<PrinterInfoBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPrinterInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, CostCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@PrinterName", DbType.String, PrinterName);
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, ActiveStat);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PrinterInfoBO itemType = new PrinterInfoBO();
                                itemType.ActiveStatus = reader["ActiveStatus"].ToString();
                                itemType.PrinterInfoId = Convert.ToInt32(reader["PrinterInfoId"]);
                                itemType.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                itemType.CostCenter = reader["CostCenter"].ToString();
                                itemType.StockType = reader["StockType"].ToString();
                                itemType.PrinterName = reader["PrinterName"].ToString();
                                itemType.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                itemList.Add(itemType);
                            }
                        }
                    }
                }
            }
            return itemList;
        }
        public PrinterInfoBO GetPrinterInfoByCostCenterNType(int costCenterId, string printerType)
        {
            PrinterInfoBO printerBO = new PrinterInfoBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPrinterInfoByCostCenterNType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@PrinterType", DbType.String, printerType);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                printerBO.PrinterInfoId = Convert.ToInt32(reader["PrinterInfoId"]);
                                printerBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                printerBO.StockType = reader["StockType"].ToString();
                                printerBO.KitchenId = Convert.ToInt32(reader["KitchenId"]);
                                printerBO.PrinterName = reader["PrinterName"].ToString();
                                printerBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                printerBO.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return printerBO;
        }
        public List<PrinterInfoBO> GetPrinterInfoByPrintType(string printType)
        {
            List<PrinterInfoBO> typeBO = new List<PrinterInfoBO>();
            PrinterInfoBO printBo;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPrinterInfoByPrintType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PrintyType", DbType.String, printType);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                printBo = new PrinterInfoBO();

                                printBo.PrinterInfoId = Convert.ToInt32(reader["PrinterInfoId"]);
                                printBo.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                printBo.StockType = reader["StockType"].ToString();
                                printBo.KitchenId = Convert.ToInt32(reader["KitchenId"]);
                                printBo.PrinterName = reader["PrinterName"].ToString();
                                printBo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                printBo.ActiveStatus = reader["ActiveStatus"].ToString();

                                typeBO.Add(printBo);
                            }
                        }
                    }
                }
            }
            return typeBO;
        }
        public List<PrinterInfoBO> GetPrinterInfoByCostCenterPrintType(int costCenterId, string printType)
        {
            List<PrinterInfoBO> typeBO = new List<PrinterInfoBO>();
            PrinterInfoBO printBo;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPrinterInfoByCostcenterPrintType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@PrintyType", DbType.String, printType);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                printBo = new PrinterInfoBO();

                                printBo.PrinterInfoId = Convert.ToInt32(reader["PrinterInfoId"]);
                                printBo.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                printBo.StockType = reader["StockType"].ToString();
                                printBo.KitchenId = Convert.ToInt32(reader["KitchenId"]);
                                printBo.PrinterName = reader["PrinterName"].ToString();
                                printBo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                printBo.ActiveStatus = reader["ActiveStatus"].ToString();

                                typeBO.Add(printBo);
                            }
                        }
                    }
                }
            }
            return typeBO;
        }
    }
}
