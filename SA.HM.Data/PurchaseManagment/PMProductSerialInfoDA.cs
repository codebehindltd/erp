using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Inventory;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.PurchaseManagment
{
    public class PMProductSerialInfoDA : BaseService
    {
        public List<PMProductSerialInfoBO> GetPMProductInfoByCategoryId(int productId)
        {
            List<PMProductSerialInfoBO> productList = new List<PMProductSerialInfoBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("PMProductSerialInfoByProductId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProductId", DbType.Int32, productId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMProductSerialInfoBO productBO = new PMProductSerialInfoBO();
                                productBO.SerialId = Convert.ToInt32(reader["SerialId"]);
                                productBO.ProductId = Convert.ToInt32(reader["ProductId"]);
                                productBO.SerialNumber = reader["SerialNumber"].ToString();
                                productBO.ProductName = reader["ProductName"].ToString();
                                productBO.ProductCode = reader["ProductCode"].ToString();
                                productList.Add(productBO);
                            }
                        }
                    }
                }
            }
            return productList;
        }
        public List<PMProductSerialInfoBO> GetSerialInfoByProductNOutId(int productId, int outId)
        {
            List<PMProductSerialInfoBO> productList = new List<PMProductSerialInfoBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSerialInfoByProductNOutId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProductId", DbType.Int32, productId);
                    dbSmartAspects.AddInParameter(cmd, "@OutId", DbType.Int32, outId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMProductSerialInfoBO productBO = new PMProductSerialInfoBO();
                                productBO.SerialId = Convert.ToInt32(reader["SerialId"]);
                                productBO.ProductId = Convert.ToInt32(reader["ProductId"]);
                                productBO.SerialNumber = reader["SerialNumber"].ToString();
                                productBO.ProductName = reader["ProductName"].ToString();
                                productBO.ProductCode = reader["ProductCode"].ToString();
                                productList.Add(productBO);
                            }
                        }
                    }
                }
            }
            return productList;
        }
        public PMProductSerialInfoBO GetPMProductSerialInfoBySerialNPurchseOrder(int pOrderId, string serialNumber)
        {
            PMProductSerialInfoBO productBO = new PMProductSerialInfoBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMProductSerialInfoBySerialNPurchaseOrder_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@POrderId", DbType.Int32, pOrderId);
                    dbSmartAspects.AddInParameter(cmd, "@SerialNumber", DbType.String, serialNumber);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                productBO.SerialId = Convert.ToInt32(reader["SerialId"]);
                                productBO.ProductId = Convert.ToInt32(reader["ProductId"]);
                                productBO.SerialNumber = reader["SerialNumber"].ToString();
                                productBO.ProductName = reader["ProductName"].ToString();
                                productBO.ProductCode = reader["ProductCode"].ToString();
                            }
                        }
                    }
                }
            }
            return productBO;
        }
        public PMProductSerialInfoBO GetPMProductSerialInfoBySerialNumber(string serialNumber)
        {
            PMProductSerialInfoBO productBO = new PMProductSerialInfoBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMProductSerialInfoBySerialNumber_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SerialNumber", DbType.String, serialNumber);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                productBO.SerialId = Convert.ToInt32(reader["SerialId"]);
                                productBO.ProductId = Convert.ToInt32(reader["ProductId"]);
                                productBO.SerialNumber = reader["SerialNumber"].ToString();
                                productBO.ProductName = reader["ProductName"].ToString();
                                productBO.ProductCode = reader["ProductCode"].ToString();
                            }
                        }
                    }
                }
            }
            return productBO;
        }
        public PMProductSerialInfoBO GetPMProductSerialInfoBySerialNumberForSale(int productId, string serialNumber)
        {
            PMProductSerialInfoBO productBO = new PMProductSerialInfoBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMProductSerialInfoBySerialNumberForSale_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProductId", DbType.Int32, productId);
                    dbSmartAspects.AddInParameter(cmd, "@SerialNumber", DbType.String, serialNumber);
                    DataSet ProductSerialDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, ProductSerialDS, "ProductSerial");
                    DataTable Table = ProductSerialDS.Tables["ProductSerial"];

                    productBO = Table.AsEnumerable().Select(r => new PMProductSerialInfoBO
                    {
                        SerialId = r.Field<int>("SerialId"),
                        ProductId = r.Field<int>("ProductId"),
                        SerialNumber = r.Field<string>("SerialNumber"),
                        ProductName = r.Field<string>("ProductName"),
                        ProductCode = r.Field<string>("ProductCode")

                    }).FirstOrDefault();
                }
            }
            return productBO;
        }
        public PMProductSerialInfoBO GetPMProductSerialInfoBySerialNumberForSaleReturn(int salesId, int productId, string serialNumber)
        {
            PMProductSerialInfoBO productBO = new PMProductSerialInfoBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMProductSerialInfoBySerialNumberForSaleReturn_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SalesId", DbType.Int32, salesId);
                    dbSmartAspects.AddInParameter(cmd, "@ProductId", DbType.Int32, productId);
                    dbSmartAspects.AddInParameter(cmd, "@SerialNumber", DbType.String, serialNumber);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                productBO.SerialId = Convert.ToInt32(reader["SerialId"]);
                                productBO.ProductId = Convert.ToInt32(reader["ProductId"]);
                                productBO.SerialNumber = reader["SerialNumber"].ToString();
                                productBO.ProductName = reader["ProductName"].ToString();
                                productBO.ProductCode = reader["ProductCode"].ToString();
                            }
                        }
                    }
                }
            }
            return productBO;
        }
    }
}
