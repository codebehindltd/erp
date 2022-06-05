using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

using HotelManagement.Entity.HMCommon;

using HotelManagement.Entity.SalesManagment;

namespace HotelManagement.Data.SalesManagment
{
    public class SalesBandwidthInfoDA : BaseService
    {
        public bool SaveSalesBandwidthInfo(SalesBandwidthInfoBO salesBandwidthInfoBO, out int tmpBandwidthId)
        {
            bool status = false;

            using (DbConnection con = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveSalesBandwidthInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BandwidthType", DbType.String, salesBandwidthInfoBO.BandwidthType);
                    dbSmartAspects.AddInParameter(cmd, "@BandwidthName", DbType.String, salesBandwidthInfoBO.BandwidthName);
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, salesBandwidthInfoBO.ActiveStat);
                    dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, salesBandwidthInfoBO.CreatedBy);

                    dbSmartAspects.AddOutParameter(cmd, "@BandwidthInfoId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;

                    tmpBandwidthId = Convert.ToInt32(cmd.Parameters["@BandwidthInfoId"].Value);
                }
            }

            return status;
        }

        public bool UpdateSalesBandwidthInfoBO(SalesBandwidthInfoBO salesBandwidthInfoBO)
        {
            bool status = false;

            using (DbConnection con = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateSalesBandwidthInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BandwidthInfoId", DbType.Int32, salesBandwidthInfoBO.BandwidthInfoId);
                    dbSmartAspects.AddInParameter(cmd, "@BandwidthType", DbType.String, salesBandwidthInfoBO.BandwidthType);
                    dbSmartAspects.AddInParameter(cmd, "@BandwidthName", DbType.String, salesBandwidthInfoBO.BandwidthName);
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, salesBandwidthInfoBO.ActiveStat);
                    dbSmartAspects.AddInParameter(cmd, "@LastModifiedBy", DbType.Int32, salesBandwidthInfoBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
                }
            }

            return status;
        }

        public SalesBandwidthInfoBO GetSalesBandwidthInfoById(int bandwidthId)
        {
            SalesBandwidthInfoBO salesBandwidthInfoBO = new SalesBandwidthInfoBO();
            DataSet bandwidhtInfoSet = new DataSet();

            using (DbConnection con = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesBandwidthInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BandwidthInfoId", DbType.Int32, bandwidthId);

                    dbSmartAspects.LoadDataSet(cmd, bandwidhtInfoSet, "BandwidthInfo");
                    DataTable table = bandwidhtInfoSet.Tables["BandwidthInfo"];

                    salesBandwidthInfoBO = table.AsEnumerable().Select(r => new SalesBandwidthInfoBO
                    {
                        BandwidthInfoId = r.Field<int>("BandwidthInfoId"),
                        BandwidthType = r.Field<string>("BandwidthType"),
                        BandwidthName = r.Field<string>("BandwidthName"),
                        ActiveStat = r.Field<bool>("ActiveStat"),
                        ActiveStatus = r.Field<string>("ActiveStatus")

                    }).FirstOrDefault();
                }
            }

            return salesBandwidthInfoBO;
        }

        public List<SalesBandwidthInfoBO> GetSalesBandwidthInfoBySearchCriteria(string bandwidthName, string bandwidthType, bool status)
        {
            string searchCriteria = string.Empty;
            List<SalesBandwidthInfoBO> salesBandwidthInfoList = new List<SalesBandwidthInfoBO>();

            searchCriteria = GenerateWhereCondition(bandwidthName, bandwidthType, status);

            DataSet bandwidthInfoSet = new DataSet();

            using (DbConnection con = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesBandwidthInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, searchCriteria);

                    dbSmartAspects.LoadDataSet(cmd, bandwidthInfoSet, "BandwidthInfo");
                    DataTable table = bandwidthInfoSet.Tables["BandwidthInfo"];

                    salesBandwidthInfoList = table.AsEnumerable().Select(r => new SalesBandwidthInfoBO
                    {
                        BandwidthInfoId = r.Field<Int32>("BandwidthInfoId"),
                        BandwidthType = r.Field<string>("BandwidthType"),
                        BandwidthName = r.Field<string>("BandwidthName"),
                        ActiveStat = r.Field<bool>("ActiveStat"),
                        ActiveStatus = r.Field<string>("ActiveStatus")

                    }).ToList();

                }
            }

            return salesBandwidthInfoList;
        }

        public List<SalesBandwidthInfoBO> GetSalesBandwidthInfoByBandwidthType(string bandwidthType)
        {
            List<SalesBandwidthInfoBO> salesBandwidthInfoList = new List<SalesBandwidthInfoBO>();
            DataSet bandwidthInfoSet = new DataSet();

            using (DbConnection con = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesBandwidthInfoByBandwidthType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BandwidthType", DbType.String, bandwidthType);

                    dbSmartAspects.LoadDataSet(cmd, bandwidthInfoSet, "BandwidthInfo");
                    DataTable table = bandwidthInfoSet.Tables["BandwidthInfo"];

                    salesBandwidthInfoList = table.AsEnumerable().Select(r => new SalesBandwidthInfoBO
                    {
                        BandwidthInfoId = r.Field<Int32>("BandwidthInfoId"),
                        BandwidthName = r.Field<string>("BandwidthName")

                    }).ToList();

                }
            }

            return salesBandwidthInfoList;
        }

        private string GenerateWhereCondition(string bandwidthName, string bandwidthType, bool status)
        {
            string Where = string.Empty, Condition = string.Empty;

            if (!string.IsNullOrEmpty(bandwidthType))
            {
                Condition = "BandwidthType = '" + bandwidthType + "'";
            }

            if (!string.IsNullOrEmpty(bandwidthName))
            {
                if (string.IsNullOrEmpty(Condition))
                {
                    Condition = "BandwidthName LIKE '%" + bandwidthName + "%'";
                }
                else
                {
                    Condition += " AND BandwidthName LIKE '%" + bandwidthName + "%'";
                }
            }

            if (!string.IsNullOrEmpty(Condition))
            {
                Condition += " AND ActiveStat = " + (!status ? 0 : 1);
            }
            else
            {
                Condition = "ActiveStat = " + (!status ? 0 : 1);
            }

            if (!string.IsNullOrEmpty(Condition))
            {
                Where += "WHERE " + Condition;
            }

            return Where;
        }

    }
}
