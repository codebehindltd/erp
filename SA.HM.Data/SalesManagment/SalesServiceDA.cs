using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.SalesManagment;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.SalesManagment
{
    public class SalesServiceDA : BaseService
    {
        public bool SaveSalesServiceInfo(Entity.SalesManagment.SalesServiceBO serviceBO, out int tmpServiceId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveSalesServiceInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@Name", DbType.String, serviceBO.Name);
                    dbSmartAspects.AddInParameter(command, "@Code", DbType.String, serviceBO.Code);
                    dbSmartAspects.AddInParameter(command, "@CategoryId", DbType.Int32, serviceBO.CategoryId);
                    dbSmartAspects.AddInParameter(command, "@Frequency", DbType.String, serviceBO.Frequency);

                    dbSmartAspects.AddInParameter(command, "@PurchasePrice", DbType.Decimal, serviceBO.PurchasePrice);
                    dbSmartAspects.AddInParameter(command, "@SellingLocalCurrencyId", DbType.Int32, serviceBO.SellingLocalCurrencyId);
                    dbSmartAspects.AddInParameter(command, "@UnitPriceLocal", DbType.Decimal, serviceBO.UnitPriceLocal);
                    dbSmartAspects.AddInParameter(command, "@SellingUsdCurrencyId", DbType.Int32, serviceBO.SellingUsdCurrencyId);
                    dbSmartAspects.AddInParameter(command, "@UnitPriceUsd", DbType.Decimal, serviceBO.UnitPriceUsd);

                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, serviceBO.Description);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, serviceBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@ServiceId", DbType.Int32, sizeof(Int32));

                    dbSmartAspects.AddInParameter(command, "@BandwidthType", DbType.Int32, serviceBO.BandwidthType);
                    dbSmartAspects.AddInParameter(command, "@Bandwidth", DbType.Int32, serviceBO.Bandwidth);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpServiceId = Convert.ToInt32(command.Parameters["@ServiceId"].Value);
                }
            }
            return status;
        }
        public bool UpdateSalesServiceInfo(Entity.SalesManagment.SalesServiceBO serviceBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateSalesServiceInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ServiceId", DbType.Int32, serviceBO.ServiceId);
                    dbSmartAspects.AddInParameter(command, "@Name", DbType.String, serviceBO.Name);
                    dbSmartAspects.AddInParameter(command, "@Code", DbType.String, serviceBO.Code);
                    dbSmartAspects.AddInParameter(command, "@CategoryId", DbType.Int32, serviceBO.CategoryId);
                    dbSmartAspects.AddInParameter(command, "@Frequency", DbType.String, serviceBO.Frequency);

                    dbSmartAspects.AddInParameter(command, "@PurchasePrice", DbType.Decimal, serviceBO.PurchasePrice);
                    dbSmartAspects.AddInParameter(command, "@SellingLocalCurrencyId", DbType.Int32, serviceBO.SellingLocalCurrencyId);
                    dbSmartAspects.AddInParameter(command, "@UnitPriceLocal", DbType.Decimal, serviceBO.UnitPriceLocal);
                    dbSmartAspects.AddInParameter(command, "@SellingUsdCurrencyId", DbType.Int32, serviceBO.SellingUsdCurrencyId);
                    dbSmartAspects.AddInParameter(command, "@UnitPriceUsd", DbType.Decimal, serviceBO.UnitPriceUsd);

                    dbSmartAspects.AddInParameter(command, "@BandwidthType", DbType.Int32, serviceBO.BandwidthType);
                    dbSmartAspects.AddInParameter(command, "@Bandwidth", DbType.Int32, serviceBO.Bandwidth);

                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, serviceBO.Description);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, serviceBO.LastModifiedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<SalesServiceBO> GetSaleServicInfoBySearchCriteria(string Name, string Code)
        {
            string SearchCriteria = string.Empty;
            SearchCriteria = GenerateWhereCondition(Name, Code);

            List<SalesServiceBO> serviceList = new List<SalesServiceBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSaleServicInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, SearchCriteria);

                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "SaleService");
                    DataTable Table = SaleServiceDS.Tables["SaleService"];

                    serviceList = Table.AsEnumerable().Select(r => new SalesServiceBO
                    {

                        ServiceId = r.Field<int>("serviceId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        Frequency = r.Field<string>("Frequency"),
                        PurchasePrice = r.Field<decimal>("PurchasePrice"),
                        SellingLocalCurrencyId = r.Field<int>("SellingLocalCurrencyId"),
                        UnitPriceLocal = r.Field<decimal>("UnitPriceLocal"),
                        SellingUsdCurrencyId = r.Field<int>("SellingUsdCurrencyId"),
                        UnitPriceUsd = r.Field<decimal>("UnitPriceUsd"),
                        Description = r.Field<string>("Description"),
                        CategoryName = r.Field<string>("CategoryName")

                    }).ToList();


                    //if (reader != null)
                    //{
                    //    while (reader.Read())
                    //    {
                    //        SalesServiceBO serviceBO = new SalesServiceBO();
                    //        ServiceId = Convert.ToInt32(r.Field<int>("serviceId"]);
                    //        Name = r.Field<int>("Name"].ToString();
                    //        Code = r.Field<int>("Code"].ToString();
                    //        CategoryId = Int32.Parse(r.Field<int>("CategoryId"].ToString());
                    //        Frequency = r.Field<int>("Frequency"].ToString();
                    //        PurchasePrice = Convert.ToDecimal(r.Field<int>("PurchasePrice"]);
                    //        SellingPriceLocal = Int32.Parse(r.Field<int>("SellingPriceLocal"].ToString());
                    //        UnitPriceLocal = Convert.ToDecimal(r.Field<int>("UnitPriceLocal"]);
                    //        SellingPriceUsd = Int32.Parse(r.Field<int>("SellingPriceUsd"].ToString());
                    //        UnitPriceUsd = Convert.ToDecimal(r.Field<int>("UnitPriceUsd"]);
                    //        Description = r.Field<int>("Description"].ToString();
                    //        CategoryName = r.Field<int>("CategoryName"].ToString();
                    //        serviceList.Add(serviceBO);
                    //    }
                    //}

                }
            }
            return serviceList;
        }
        public List<SalesServiceBO> GetSaleServicInfo()
        {
            List<SalesServiceBO> serviceList = new List<SalesServiceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSaleServicInfo_SP"))
                {

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SalesServiceBO serviceBO = new SalesServiceBO();
                                serviceBO.ServiceId = Convert.ToInt32(reader["serviceId"]);
                                serviceBO.Name = reader["Name"].ToString();
                                serviceBO.Code = reader["Code"].ToString();
                                serviceBO.CategoryId = Int32.Parse(reader["CategoryId"].ToString());
                                serviceBO.Frequency = reader["Frequency"].ToString();
                                serviceBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]);
                                serviceBO.SellingLocalCurrencyId = Int32.Parse(reader["SellingLocalCurrencyId"].ToString());
                                serviceBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                serviceBO.SellingUsdCurrencyId = Int32.Parse(reader["SellingUsdCurrencyId"].ToString());
                                serviceBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                serviceBO.Description = reader["Description"].ToString();
                                serviceBO.CategoryName = reader["CategoryName"].ToString();
                                serviceList.Add(serviceBO);
                            }
                        }
                    }
                }
            }
            return serviceList;
        }
        public List<SalesServiceBO> GetSaleServicInfoByCategoryId(int categoryId)
        {
            List<SalesServiceBO> serviceList = new List<SalesServiceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSaleServicInfoByCategoryId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SalesServiceBO serviceBO = new SalesServiceBO();
                                serviceBO.ServiceId = Convert.ToInt32(reader["serviceId"]);
                                serviceBO.Name = reader["Name"].ToString();
                                serviceBO.Code = reader["Code"].ToString();
                                serviceBO.CategoryId = Int32.Parse(reader["CategoryId"].ToString());
                                serviceBO.Frequency = reader["Frequency"].ToString();
                                serviceBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]);
                                serviceBO.SellingLocalCurrencyId = Int32.Parse(reader["SellingLocalCurrencyId"].ToString());
                                serviceBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                serviceBO.SellingUsdCurrencyId = Int32.Parse(reader["SellingUsdCurrencyId"].ToString());
                                serviceBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                serviceBO.Description = reader["Description"].ToString();
                                serviceBO.CategoryName = reader["CategoryName"].ToString();
                                serviceList.Add(serviceBO);
                            }
                        }
                    }
                }
            }
            return serviceList;
        }
        public List<SalesServiceBO> GetSaleServicInfoByFrequency(string frequency)
        {
            List<SalesServiceBO> serviceList = new List<SalesServiceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSaleServicInfoByFrequency_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Frequency", DbType.String, frequency);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SalesServiceBO serviceBO = new SalesServiceBO();
                                serviceBO.ServiceId = Convert.ToInt32(reader["serviceId"]);
                                serviceBO.Name = reader["Name"].ToString();
                                serviceBO.Code = reader["Code"].ToString();
                                serviceBO.CategoryId = Int32.Parse(reader["CategoryId"].ToString());
                                serviceBO.Frequency = reader["Frequency"].ToString();
                                serviceBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]);
                                serviceBO.SellingLocalCurrencyId = Int32.Parse(reader["SellingLocalCurrencyId"].ToString());
                                serviceBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                serviceBO.SellingUsdCurrencyId = Int32.Parse(reader["SellingUsdCurrencyId"].ToString());
                                serviceBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                serviceBO.Description = reader["Description"].ToString();
                                serviceBO.CategoryName = reader["CategoryName"].ToString();
                                serviceList.Add(serviceBO);
                            }
                        }
                    }
                }
            }
            return serviceList;
        }
        public SalesServiceBO GetSalesServiceInfoByServiceId(int serviceId)
        {
            SalesServiceBO serviceBO = new SalesServiceBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesServiceInfoByServiceId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ServiceId", DbType.Int32, serviceId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                serviceBO.ServiceId = Convert.ToInt32(reader["serviceId"]);
                                serviceBO.Name = reader["Name"].ToString();
                                serviceBO.Code = reader["Code"].ToString();
                                serviceBO.CategoryId = Int32.Parse(reader["CategoryId"].ToString());
                                serviceBO.Frequency = reader["Frequency"].ToString();

                                serviceBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]);
                                serviceBO.SellingLocalCurrencyId = Int32.Parse(reader["SellingLocalCurrencyId"].ToString());
                                serviceBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                serviceBO.SellingUsdCurrencyId = Int32.Parse(reader["SellingUsdCurrencyId"].ToString());
                                serviceBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);

                                serviceBO.Description = reader["Description"].ToString();
                                serviceBO.CategoryName = reader["CategoryName"].ToString();

                                serviceBO.Bandwidth = Convert.ToInt32(reader["Bandwidth"].ToString());
                                serviceBO.BandwidthType = Int32.Parse(reader["BandwidthType"].ToString());

                            }
                        }
                    }
                }
            }
            return serviceBO;
        }

        public List<SalesServiceViewBO> GetServiceSalesDetailsInfoForReport(int itemId, DateTime fromDate, DateTime toDate, string customerId, int customerStatus)
        {
            string WhereCondition = GenerateWhereConditionForServiceSalesDetailsReport(itemId, fromDate, toDate, customerId, customerStatus);
            List<SalesServiceViewBO> serviceList = new List<SalesServiceViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetServiceSalesDetailsInfoForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@WhereCondition", DbType.String, WhereCondition);                   

                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "SaleService");
                    DataTable Table = SaleServiceDS.Tables["SaleService"];

                    serviceList = Table.AsEnumerable().Select(r => new SalesServiceViewBO
                    {

                        SubscriberName = r.Field<string>("SubscriberName"),
                        PostalOrPhysicalAddress = r.Field<string>("PostalOrPhysicalAddress"),
                        ContactPersont1 = r.Field<string>("ContactPersont1"),
                        ContactPersont2 = r.Field<string>("ContactPersont2"),
                        TechnicalContactPerson = r.Field<string>("TechnicalContactPerson"),
                        ClientId = r.Field<string>("ClientId"),
                        ConnectionType = r.Field<string>("ConnectionType"),
                        SiteId = r.Field<string>("SiteId"),
                        SiteName = r.Field<string>("SiteName"),
                        SiteAddress = r.Field<string>("SiteAddress"),
                        ContactPersonInSite = r.Field<string>("ContactPersonInSite"),
                        Category = r.Field<string>("Category"),
                        ItemId = r.Field<int>("ItemId"),
                        BandwidthType = r.Field<string>("BandwidthType"),
                        Bandwidth = r.Field<string>("Bandwidth"),
                        ActivationDate = r.Field<DateTime>("ActivationDate"),
                        BillExpireDate = r.Field<DateTime>("BillExpireDate"),
                        ItemUnit = r.Field<decimal>("ItemUnit"),
                        UnitPrice = r.Field<decimal>("UnitPrice"),
                        EquipmentBalance = r.Field<decimal>("EquipmentBalance"),
                        InstallationFee = r.Field<int>("InstallationFee"),
                        Remarks = r.Field<string>("Remarks")

                    }).ToList();
                }
            }
            return serviceList;
        }

        private string GenerateWhereCondition(string Name, string Code)
        {
            string Where = string.Empty;

            if (!string.IsNullOrWhiteSpace(Name))
            {
                Where = "SS.Name LIKE '%" + Name + "%'";
            }

            if (!string.IsNullOrWhiteSpace(Code))
            {
                if (string.IsNullOrEmpty(Where))
                {
                    Where = "SS.Code LIKE '%" + Code + "%'";
                }
                else
                {
                    Where += " AND SS.Code LIKE '%" + Code + "%'";
                }
            }

            if (!string.IsNullOrEmpty(Where))
            {
                Where = " WHERE " + Where;
            }

            return Where;
        }

        private string GenerateWhereConditionForServiceSalesDetailsReport(int itemId, DateTime fromDate, DateTime toDate, string customerId, int customerStatus)
        {
            string Where = string.Empty;

            if (itemId != 0)
            {
                Where = " AND pd.ItemId = " + itemId;
            }

            if (customerId != "0")
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += " AND sc.CustomerId = " + customerId;
                }
                else
                {
                    Where = " AND sc.CustomerId = " + customerId;
                }
            }

            //if (customerStatus != -1)
            //{
            //    if (!string.IsNullOrEmpty(Where))
            //    {
            //        Where += " AND sc.CustomerStatus = " + customerStatus;
            //    }
            //    else
            //    {
            //        Where = "sc.CustomerStatus = " + customerStatus;
            //    }
            //}

            Where += " AND dbo.FnDate(ps.SalesDate) >= dbo.FnDate('" + fromDate.Date.ToString("yyyy-MM-dd") + "')";
            Where += " AND dbo.FnDate(ps.SalesDate) <= dbo.FnDate('" + toDate.Date.ToString("yyyy-MM-dd") + "')";

            return Where;

        }
    }
}
