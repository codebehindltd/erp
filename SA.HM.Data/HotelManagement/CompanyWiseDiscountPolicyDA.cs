using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
    public class CompanyWiseDiscountPolicyDA : BaseService
    {
        public CompanyWiseDiscountPolicyBO GetDiscountPolicyByCompanyNRoomType(int companyId, int roomTypeId, bool activeStatus)
        {
            CompanyWiseDiscountPolicyBO discountPolicy = new CompanyWiseDiscountPolicyBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDiscountPolicyByCompanyNRoomType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    dbSmartAspects.AddInParameter(cmd, "@RoomTypeId", DbType.Int32, roomTypeId);
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, activeStatus);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "DiscountPolicy");
                    DataTable Table = ds.Tables["DiscountPolicy"];

                    discountPolicy = Table.AsEnumerable().Select(r => new CompanyWiseDiscountPolicyBO
                    {
                        CompanyWiseDiscountId = r.Field<Int64>("CompanyWiseDiscountId"),
                        CompanyId = r.Field<Int32>("CompanyId"),
                        RoomTypeId = r.Field<Int32>("RoomTypeId"),
                        DiscountType = r.Field<string>("DiscountType"),
                        DiscountAmount = r.Field<decimal>("DiscountAmount"),
                        ActiveStat = r.Field<bool>("ActiveStat"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        CreatedDate = r.Field<DateTime>("CreatedDate"),
                        LastModifiedBy = r.Field<Int32?>("LastModifiedBy"),
                        LastModifiedDate = r.Field<DateTime?>("LastModifiedDate")

                    }).FirstOrDefault();
                }
            }
            return discountPolicy;
        }
        public Boolean SaveDiscountPolicy(CompanyWiseDiscountPolicyBO discountPolicyBO, out int tmpDiscountPolicyId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveDiscountPolicyInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, discountPolicyBO.CompanyId);
                    dbSmartAspects.AddInParameter(command, "@RoomTypeId", DbType.Int32, discountPolicyBO.RoomTypeId);
                    dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, discountPolicyBO.DiscountType);
                    dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, discountPolicyBO.DiscountAmount);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, discountPolicyBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, discountPolicyBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@CompanyWiseDiscountId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpDiscountPolicyId = Convert.ToInt32(command.Parameters["@CompanyWiseDiscountId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateDiscountPolicy(CompanyWiseDiscountPolicyBO discountPolicyBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateDiscountPolicyInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CompanyWiseDiscountId", DbType.Int32, discountPolicyBO.CompanyWiseDiscountId);
                    dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, discountPolicyBO.CompanyId);
                    dbSmartAspects.AddInParameter(command, "@RoomTypeId", DbType.Int32, discountPolicyBO.RoomTypeId);
                    dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, discountPolicyBO.DiscountType);
                    dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, discountPolicyBO.DiscountAmount);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, discountPolicyBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, discountPolicyBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<CompanyWiseDiscountPolicyBO> SearchDiscountPolicyInfo(int companyId, bool activeStat, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<CompanyWiseDiscountPolicyBO> discountPolicyList = new List<CompanyWiseDiscountPolicyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDiscountPolicyInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, activeStat);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "DiscountPolicy");
                    DataTable Table = ds.Tables["DiscountPolicy"];

                    discountPolicyList = Table.AsEnumerable().Select(r => new CompanyWiseDiscountPolicyBO
                    {
                        CompanyWiseDiscountId = r.Field<Int64>("CompanyWiseDiscountId"),
                        CompanyId = r.Field<int>("CompanyId"),
                        RoomTypeId = r.Field<int>("RoomTypeId"),
                        RoomType = r.Field<string>("RoomType"),
                        DiscountType = r.Field<string>("DiscountType"),
                        DiscountAmount = r.Field<decimal>("DiscountAmount"),
                        ActiveStat = r.Field<bool>("ActiveStat"),
                        ActiveStatus = r.Field<string>("ActiveStatus")

                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return discountPolicyList;
        }
        public CompanyWiseDiscountPolicyBO GetDiscountPolicyById(long companyWiseDiscountId)
        {
            CompanyWiseDiscountPolicyBO discountBO = new CompanyWiseDiscountPolicyBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDiscountPolicyById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyWiseDiscountId", DbType.Int64, companyWiseDiscountId);

                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "DiscountPolicy");
                    DataTable Table = SaleServiceDS.Tables["DiscountPolicy"];

                    discountBO = Table.AsEnumerable().Select(r => new CompanyWiseDiscountPolicyBO
                    {
                        CompanyWiseDiscountId = r.Field<Int64>("CompanyWiseDiscountId"),
                        CompanyId = r.Field<Int32>("CompanyId"),
                        RoomTypeId = r.Field<Int32>("RoomTypeId"),
                        DiscountType = r.Field<string>("DiscountType"),
                        DiscountAmount = r.Field<decimal>("DiscountAmount"),                        
                        ActiveStat = r.Field<bool>("ActiveStat"),
                        ActiveStatus = r.Field<string>("ActiveStatus")

                    }).FirstOrDefault();
                }
            }
            return discountBO;
        }
    }
}
