using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Restaurant;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Restaurant
{
    public class RestaurantTokenDA : BaseService
    {
        public string GenarateRestaurantTokenNumber(int costCenterId, int bearerId)
        {
            string TokenNumber = string.Empty;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GenarateRestaurantToken_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@BearerId", DbType.Int32, bearerId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                TokenNumber = Convert.ToInt32(reader["TokenNumber"]).ToString();
                            }
                        }
                    }
                }
            }
            return TokenNumber;
        }
        public string GenarateTokenNumberByKot(int kotId)
        {
            string tokenNumber = string.Empty, query = string.Empty;

            query = "SELECT DBO.FnDisplayTokenNumber(" + kotId.ToString() + ")";

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    cmd.Connection = conn;
                    tokenNumber = cmd.ExecuteScalar().ToString();
                }
            }
            return tokenNumber;
        }

        public List<RestaurantTokenBO> GetRestaurantTokenInfo()
        {
            List<RestaurantTokenBO> token = new List<RestaurantTokenBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantTokenDetails_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RestaurantTokenInfo");
                    DataTable Table = ds.Tables["RestaurantTokenInfo"];

                    token = Table.AsEnumerable().Select(r => new RestaurantTokenBO
                    {
                        KotId = r.Field<Int32>("KotId"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        SourceId = r.Field<Int32>("KotId"),
                        TokenNumber = r.Field<string>("TokenNumber"),
                        IsBillHoldup = r.Field<bool>("IsBillHoldup")

                    }).ToList();
                }
            }

            return token;
        }
    }
}
