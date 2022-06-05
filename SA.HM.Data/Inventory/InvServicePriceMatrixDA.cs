using HotelManagement.Entity.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace HotelManagement.Data.Inventory
{
    public class InvServicePriceMatrixDA : BaseService
    {
        public bool SaveOrUpdatePriceMatrix(ServicePriceMatrixBO priceMatrix, out int id)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdatePriceMatrix_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ServicePriceMatrixId", DbType.Int32, priceMatrix.ServicePriceMatrixId);
                        dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, priceMatrix.ItemId);
                        dbSmartAspects.AddInParameter(command, "@PackageName", DbType.String, priceMatrix.PackageName);
                        dbSmartAspects.AddInParameter(command, "@UplinkFrequencyId", DbType.Int32, priceMatrix.UplinkFrequencyId);
                        dbSmartAspects.AddInParameter(command, "@UplinkFrequencyUnit", DbType.String, priceMatrix.UplinkFrequencyUnit);
                        dbSmartAspects.AddInParameter(command, "@DownlinkFrequencyId", DbType.Int32, priceMatrix.DownlinkFrequencyId);
                        dbSmartAspects.AddInParameter(command, "@DownlinkFrequencyUnit", DbType.String, priceMatrix.DownlinkFrequencyUnit);
                        dbSmartAspects.AddInParameter(command, "@ShareRatio", DbType.String, priceMatrix.ShareRatio);
                        dbSmartAspects.AddInParameter(command, "@UnitPrice", DbType.Decimal, priceMatrix.UnitPrice);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, priceMatrix.Description);
                        dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, priceMatrix.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));

                        status = (dbSmartAspects.ExecuteNonQuery(command) > 0);

                        id = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public List<ServicePriceMatrixBO> GetServicePackagesByItemIdNPackageNameWithPagination(int itemId, string packageName, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<ServicePriceMatrixBO> productList = new List<ServicePriceMatrixBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetServicePackagesByItemIdNPackageName_SP"))
                {
                    if (itemId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrWhiteSpace(packageName))
                        dbSmartAspects.AddInParameter(cmd, "@PackageName", DbType.String, packageName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PackageName", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ServicePriceMatrixBO productBO = new ServicePriceMatrixBO();

                                productBO.ServicePriceMatrixId = Convert.ToInt32(reader["ServicePriceMatrixId"]);
                                productBO.PackageName = reader["PackageName"].ToString();
                                productBO.ItemName = reader["ItemName"].ToString();
                                productBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);

                                productList.Add(productBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return productList;
        }

        public ServicePriceMatrixBO GetServicePriceMatrixById(int servicePriceMatrixId)
        {
            ServicePriceMatrixBO ServicePriceMatrix = new ServicePriceMatrixBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetServicePriceMatrixById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ServicePriceMatrixId", DbType.Int32, servicePriceMatrixId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PriceMatrix");
                    DataTable Table = ds.Tables["PriceMatrix"];

                    ServicePriceMatrix = Table.AsEnumerable().Select(r => new ServicePriceMatrixBO
                    {
                        ServicePriceMatrixId = r.Field<int>("ServicePriceMatrixId"),
                        ItemId = r.Field<int>("ItemId"),
                        UnitPrice = r.Field<decimal>("UnitPrice"),
                        UplinkFrequency = r.Field<int>("UplinkFrequency"),
                        DownlinkFrequency = r.Field<int>("DownlinkFrequency"),

                    }).FirstOrDefault();
                }
            }
            return ServicePriceMatrix;
        }
    }
}
