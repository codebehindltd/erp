using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Restaurant;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Restaurant
{
    public class RestaurantConfigurationDA : BaseService
    {

        //--------*** Daily Sales Statement Configuration
        public Boolean SaveDailySalesStatementConfiguration(RestaurantDailySalesStatementConfigurationBO dailySalesStatementConfiguration, out int dailySalesStatementId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveDailySalesStatementConfiguration_SP"))
                {
                    command.Parameters.Clear();

                    dbSmartAspects.AddInParameter(command, "@DateFrom", DbType.Date, dailySalesStatementConfiguration.DateFrom);
                    dbSmartAspects.AddInParameter(command, "@DateTo", DbType.Date, dailySalesStatementConfiguration.DateTo);
                    dbSmartAspects.AddInParameter(command, "@PercentageAmount", DbType.Decimal, dailySalesStatementConfiguration.PercentageAmount);
                    dbSmartAspects.AddInParameter(command, "@AmountInPercentage", DbType.Decimal, dailySalesStatementConfiguration.AmountInPercentage);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Decimal, dailySalesStatementConfiguration.CreatedBy);

                    dbSmartAspects.AddOutParameter(command, "@DailySalesStatementId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    dailySalesStatementId = Convert.ToInt32(command.Parameters["@DailySalesStatementId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateDailySalesStatementConfiguration(RestaurantDailySalesStatementConfigurationBO dailySalesStatementConfiguration)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateDailySalesStatementConfiguration_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@DailySalesStatementId", DbType.Int32, dailySalesStatementConfiguration.DailySalesStatementId);
                        dbSmartAspects.AddInParameter(command, "@DateFrom", DbType.Date, dailySalesStatementConfiguration.DateFrom);
                        dbSmartAspects.AddInParameter(command, "@DateTo", DbType.Date, dailySalesStatementConfiguration.DateTo);
                        dbSmartAspects.AddInParameter(command, "@PercentageAmount", DbType.Decimal, dailySalesStatementConfiguration.PercentageAmount);
                        dbSmartAspects.AddInParameter(command, "@AmountInPercentage", DbType.Decimal, dailySalesStatementConfiguration.AmountInPercentage);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, dailySalesStatementConfiguration.LastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(command);
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
            }
            return retVal;
        }
        public RestaurantDailySalesStatementConfigurationBO GetDailySalesStatementConfigurationById(int dailySalesStatementId)
        {
            RestaurantDailySalesStatementConfigurationBO cnf = new RestaurantDailySalesStatementConfigurationBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDailySalesStatementConfigurationById"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DailySalesStatementId", DbType.Int32, dailySalesStatementId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                cnf.DailySalesStatementId = Convert.ToInt32(reader["DailySalesStatementId"].ToString());
                                cnf.DateFrom = Convert.ToDateTime(reader["DateFrom"]);
                                cnf.DateTo = Convert.ToDateTime(reader["DateTo"]);
                                cnf.PercentageAmount = Convert.ToDecimal(reader["PercentageAmount"]);
                                cnf.AmountInPercentage = Convert.ToDecimal(reader["AmountInPercentage"]);
                            }
                        }
                    }
                }
            }
            return cnf;
        }
        public List<RestaurantDailySalesStatementConfigurationBO> GetDailySalesStatementConfigurationBySearchCriteria(DateTime? FromDate, DateTime? ToDate, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<RestaurantDailySalesStatementConfigurationBO> dailySalesStatementConfiguration = new List<RestaurantDailySalesStatementConfigurationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDailySalesStatementConfigurationBySearchCriteria_SP"))
                {

                    if (FromDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.Date, FromDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.Date, DBNull.Value);
                    }

                    if (ToDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.Date, ToDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.Date, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet BearerDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, BearerDS, "DailySalesStatement");
                    DataTable Table = BearerDS.Tables["DailySalesStatement"];

                    if (Table != null)
                    {
                        dailySalesStatementConfiguration = Table.AsEnumerable().Select(r => new RestaurantDailySalesStatementConfigurationBO
                        {
                            DailySalesStatementId = r.Field<Int32>("DailySalesStatementId"),
                            DateFrom = r.Field<DateTime>("DateFrom"),
                            DateTo = r.Field<DateTime>("DateTo"),
                            PercentageAmount = r.Field<decimal>("PercentageAmount"),
                            AmountInPercentage = r.Field<decimal>("AmountInPercentage")

                        }).ToList();
                    }

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return dailySalesStatementConfiguration;
        }
        //--------*** Daily Sales Statement Configuration

    }
}
