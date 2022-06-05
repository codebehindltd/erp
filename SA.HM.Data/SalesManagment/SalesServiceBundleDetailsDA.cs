using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.SalesManagment;
using System.Data.Common;
using System.Data;
using System.Collections;

namespace HotelManagement.Data.SalesManagment
{
    public class SalesServiceBundleDetailsDA : BaseService
    {
        public bool SaveSalesServiceBundleInfo(SalesServiceBundleBO bundleBO, out int tmpBundleId, List<SalesServiceBundleDetailsBO> detailBO)
        {
            bool retVal = false;
            int status = 0;
            //int tmpRegistrationId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveSalesServiceBundleInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@BundleName", DbType.String, bundleBO.BundleName);
                        dbSmartAspects.AddInParameter(commandMaster, "@BundleCode", DbType.String, bundleBO.BundleCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@Frequency", DbType.String, bundleBO.Frequency);
                        dbSmartAspects.AddInParameter(commandMaster, "@SellingPriceLocal", DbType.Int32, bundleBO.SellingPriceLocal);
                        dbSmartAspects.AddInParameter(commandMaster, "@UnitPriceLocal", DbType.Decimal, bundleBO.UnitPriceLocal);
                        dbSmartAspects.AddInParameter(commandMaster, "@SellingPriceUsd", DbType.Int32, bundleBO.SellingPriceUsd);
                        dbSmartAspects.AddInParameter(commandMaster, "@UnitPriceUsd", DbType.Decimal, bundleBO.UnitPriceUsd);
                        dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, bundleBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(commandMaster, "@BundleId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        tmpBundleId = Convert.ToInt32(commandMaster.Parameters["@BundleId"].Value);

                        if (status > 0)
                        {
                            int count = 0;

                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveSalesServiceBundleDetailsInfo_SP"))
                            {
                                foreach (SalesServiceBundleDetailsBO bundleDetailBO in detailBO)
                                {
                                    if (bundleDetailBO.BundleId == 0)
                                    {
                                        commandDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandDetails, "@BundleId", DbType.Int32, tmpBundleId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@IsProductOrService", DbType.String, bundleDetailBO.IsProductOrService);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ProductId", DbType.Int32, bundleDetailBO.ProductId);
                                        //dbSmartAspects.AddInParameter(commandDetails, "@ServiceId", DbType.Int32, bundleDetailBO.ServiceId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@Quantity", DbType.Decimal, bundleDetailBO.Quantity);
                                        dbSmartAspects.AddInParameter(commandDetails, "@UnitPrice", DbType.Decimal, bundleDetailBO.UnitPrice);
                                        count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }

                            if (count == detailBO.Count)
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
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }
        public bool UpdateSalesServiceBundleInfo(SalesServiceBundleBO bundleBO, List<SalesServiceBundleDetailsBO> detailBO, ArrayList arrayDelete)
        {
            bool retVal = false;
            int status = 0;
            int tmpBundleId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateSalesServiceBundleInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@BundleId", DbType.Int32, bundleBO.BundleId);
                        dbSmartAspects.AddInParameter(commandMaster, "@BundleName", DbType.String, bundleBO.BundleName);
                        dbSmartAspects.AddInParameter(commandMaster, "@BundleCode", DbType.String, bundleBO.BundleCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@Frequency", DbType.String, bundleBO.Frequency);
                        dbSmartAspects.AddInParameter(commandMaster, "@SellingPriceLocal", DbType.Int32, bundleBO.SellingPriceLocal);
                        dbSmartAspects.AddInParameter(commandMaster, "@UnitPriceLocal", DbType.Decimal, bundleBO.UnitPriceLocal);
                        dbSmartAspects.AddInParameter(commandMaster, "@SellingPriceUsd", DbType.Int32, bundleBO.SellingPriceUsd);
                        dbSmartAspects.AddInParameter(commandMaster, "@UnitPriceUsd", DbType.Decimal, bundleBO.UnitPriceUsd); ;
                        dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, bundleBO.LastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        tmpBundleId = bundleBO.BundleId;
                        if (status > 0)
                        {
                            int count = 0;
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveSalesServiceBundleDetailsInfo_SP"))
                            {
                                foreach (SalesServiceBundleDetailsBO bundleDetailBO in detailBO)
                                {
                                    if (bundleDetailBO.DetailsId == 0)
                                    {
                                        commandDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandDetails, "@BundleId", DbType.Int32, tmpBundleId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@IsProductOrService", DbType.String, bundleDetailBO.IsProductOrService);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ProductId", DbType.Int32, bundleDetailBO.ProductId);
                                        //dbSmartAspects.AddInParameter(commandDetails, "@ServiceId", DbType.Int32, bundleDetailBO.ServiceId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@Quantity", DbType.Decimal, bundleDetailBO.Quantity);
                                        dbSmartAspects.AddInParameter(commandDetails, "@UnitPrice", DbType.Decimal, bundleDetailBO.UnitPrice);
                                        count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }
                            count = 0;
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateSalesServiceBundleDetailsInfo_SP"))
                            {
                                foreach (SalesServiceBundleDetailsBO bundleDetailBO in detailBO)
                                {
                                    if (bundleDetailBO.BundleId != 0)
                                    {
                                        commandDetails.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDetails, "@DetailsId", DbType.Int32, bundleDetailBO.DetailsId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@IsProductOrService", DbType.String, bundleDetailBO.IsProductOrService);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ProductId", DbType.Int32, bundleDetailBO.ProductId);
                                        //dbSmartAspects.AddInParameter(commandDetails, "@ServiceId", DbType.Int32, bundleDetailBO.ServiceId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@Quantity", DbType.Decimal, bundleDetailBO.Quantity);
                                        dbSmartAspects.AddInParameter(commandDetails, "@UnitPrice", DbType.Decimal, bundleDetailBO.UnitPrice);
                                        count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }

                            if (count == detailBO.Count)
                            {
                                if (arrayDelete.Count > 0)
                                {
                                    foreach (int delId in arrayDelete)
                                    {
                                        using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                        {
                                            dbSmartAspects.AddInParameter(commandEducation, "@TableName", DbType.String, "SalesServiceBundleDetails");
                                            dbSmartAspects.AddInParameter(commandEducation, "@TablePKField", DbType.String, "DetailsId");
                                            dbSmartAspects.AddInParameter(commandEducation, "@TablePKId", DbType.String, delId);
                                            status = dbSmartAspects.ExecuteNonQuery(commandEducation);
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
            return retVal;
        }
        public List<SalesServiceBundleBO> GetSalesServiceBundleInfo()
        {
            List<SalesServiceBundleBO> bundleList = new List<SalesServiceBundleBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesServiceBundleInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SalesServiceBundleBO bundleBO = new SalesServiceBundleBO();
                                bundleBO.BundleId = Convert.ToInt32(reader["BundleId"]);
                                bundleBO.BundleName = reader["BundleName"].ToString();
                                bundleBO.BundleCode = reader["BundleCode"].ToString();
                                bundleBO.Frequency = reader["Frequency"].ToString();
                                bundleBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                bundleBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                bundleBO.SellingPriceLocal = Int32.Parse(reader["SellingPriceLocal"].ToString());
                                bundleBO.SellingPriceUsd = Int32.Parse(reader["SellingPriceUsd"].ToString());
                                bundleList.Add(bundleBO);
                            }
                        }
                    }
                }
            }
            return bundleList;
        }
        public SalesServiceBundleBO GetSalesServiceBundleInfoByBundleId(int bundleId)
        {
            SalesServiceBundleBO bundleBO = new SalesServiceBundleBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesServiceBundleInfoByBundleId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BundleId", DbType.Int32, bundleId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                bundleBO.BundleId = Int32.Parse(reader["BundleId"].ToString());
                                bundleBO.BundleName = reader["BundleName"].ToString();
                                bundleBO.BundleCode = reader["BundleCode"].ToString();
                                bundleBO.Frequency = reader["Frequency"].ToString();
                                bundleBO.SellingPriceUsd = Int32.Parse(reader["SellingPriceUsd"].ToString());
                                bundleBO.SellingPriceLocal = Int32.Parse(reader["SellingPriceLocal"].ToString());
                                bundleBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                bundleBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                            }
                        }
                    }
                }
            }
            return bundleBO;
        }
        public bool DeleteServiceBundleDetailsInfoByBundleId(int bundleId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteServiceBundleDetailsInfoByBundleId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@SalesId", DbType.Int32, bundleId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<SalesServiceBundleDetailsBO> GetBundleDetailsByBundleId(int bundleId)
        {

            List<SalesServiceBundleDetailsBO> detailList = new List<SalesServiceBundleDetailsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBundleDetailsByBundleId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BundleId", DbType.Int32, bundleId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SalesServiceBundleDetailsBO detail = new SalesServiceBundleDetailsBO();

                                //detail.ServiceId = Int32.Parse(reader["ServiceId"].ToString());
                                detail.ProductId = Int32.Parse(reader["ProductId"].ToString());
                                detail.IsProductOrService = (reader["IsProductOrService"].ToString());
                                detail.Quantity = Convert.ToDecimal(reader["Quantity"].ToString());
                                detail.DetailsId = Int32.Parse(reader["DetailsId"].ToString());
                                detail.BundleId = Int32.Parse(reader["BundleId"].ToString());
                                detailList.Add(detail);
                            }
                        }
                    }
                }
            }
            return detailList;

        }

        public List<SalesServiceBundleBO> GetSalesServiceBundleInfoBySearchCriteria(string Name, string Code)
        {
            string SearchCriteria = GenerateWhereCondition(Name, Code);
            List<SalesServiceBundleBO> bundleList = new List<SalesServiceBundleBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesServiceBundleInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, SearchCriteria);

                    DataSet ServiceBundleDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ServiceBundleDS, "ServiceBundle");
                    DataTable Table = ServiceBundleDS.Tables["ServiceBundle"];

                    bundleList = Table.AsEnumerable().Select(r => new SalesServiceBundleBO
                    {
                        BundleId = r.Field<int>("BundleId"),
                        BundleName = r.Field<string>("BundleName"),
                        BundleCode = r.Field<string>("BundleCode"),
                        Frequency = r.Field<string>("Frequency"),
                        UnitPriceLocal = r.Field<decimal>("UnitPriceLocal"),
                        UnitPriceUsd = r.Field<decimal>("UnitPriceUsd"),
                        SellingPriceLocal = r.Field<int>("SellingPriceLocal"),
                        SellingPriceUsd = r.Field<int>("SellingPriceUsd")

                    }).ToList();
                }
            }
            return bundleList;
        }

        public List<SalesServiceBundleBO> GetSalesServiceBundleInfoByFrequency(string frequency)
        {
            List<SalesServiceBundleBO> bundleList = new List<SalesServiceBundleBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesServiceBundleInfoByFrequency_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Frequency", DbType.String, frequency);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SalesServiceBundleBO bundleBO = new SalesServiceBundleBO();
                                bundleBO.BundleId = Convert.ToInt32(reader["BundleId"]);
                                bundleBO.BundleName = reader["BundleName"].ToString();
                                bundleBO.BundleCode = reader["BundleCode"].ToString();
                                bundleBO.Frequency = reader["Frequency"].ToString();
                                bundleBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                bundleBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                bundleBO.SellingPriceLocal = Int32.Parse(reader["SellingPriceLocal"].ToString());
                                bundleBO.SellingPriceUsd = Int32.Parse(reader["SellingPriceUsd"].ToString());
                                bundleList.Add(bundleBO);
                            }
                        }
                    }
                }
            }
            return bundleList;
        }
        public List<SalesServiceBundleDetailsBO> GetBundleDetailsInformationByBundleId(int bundleId)
        {
            List<SalesServiceBundleDetailsBO> detailList = new List<SalesServiceBundleDetailsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBundleDetailsInformationByBundleId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BundleId", DbType.Int32, bundleId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SalesServiceBundleDetailsBO detail = new SalesServiceBundleDetailsBO();

                                detail.ItemName = reader["Name"].ToString();
                                detail.ProductId = Int32.Parse(reader["ProductId"].ToString());
                                detail.IsProductOrService = (reader["IsProductOrService"].ToString());
                                detail.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"].ToString());
                                detail.TotalUnitPriceLocal = Convert.ToDecimal(reader["TotalUnitPriceLocal"].ToString());
                                detail.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"].ToString());
                                detail.TotalUnitPriceUsd = Convert.ToDecimal(reader["TotalUnitPriceUsd"].ToString());

                                detail.Quantity = Convert.ToDecimal(reader["Quantity"].ToString());
                                detail.DetailsId = Int32.Parse(reader["DetailsId"].ToString());
                                detail.BundleId = Int32.Parse(reader["BundleId"].ToString());
                                detailList.Add(detail);
                            }
                        }
                    }
                }
            }
            return detailList;
        }


        private string GenerateWhereCondition(string Name, string Code)
        {
            string Where = string.Empty;

            if (!string.IsNullOrWhiteSpace(Name))
            {
                Where = "BundleName LIKE '%" + Name + "%'";
            }

            if (!string.IsNullOrWhiteSpace(Code))
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += " AND BundleCode LIKE '%" + Code + "%'";
                }
                else
                {
                    Where = "BundleCode LIKE '%" + Code + "%'";
                }
            }

            if (!string.IsNullOrEmpty(Where))
            {
                Where = " WHERE " + Where;
            }

            return Where;
        }
    }
}
