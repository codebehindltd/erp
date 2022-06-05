using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Membership;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Membership
{
    public class MemMemberTypeDA : BaseService
    {
        public Boolean SaveMemberTypeInfo(MemMemberTypeBO memberTypeBO, out int tmpTypeId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveMemberTypeInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TypeName", DbType.String, memberTypeBO.Name);
                    dbSmartAspects.AddInParameter(command, "@Code", DbType.String, memberTypeBO.Code);
                    dbSmartAspects.AddInParameter(command, "@SubscriptionFee", DbType.String, memberTypeBO.SubscriptionFee);
                    dbSmartAspects.AddInParameter(command, "@DiscountPercent", DbType.String, memberTypeBO.DiscountPercent);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, memberTypeBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@TypeId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpTypeId = Convert.ToInt32(command.Parameters["@TypeId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateMemberTypeInfo(MemMemberTypeBO memberTypeBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateMemberTypeInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TypeId", DbType.Int32, memberTypeBO.TypeId);
                    dbSmartAspects.AddInParameter(command, "@TypeName", DbType.String, memberTypeBO.Name);
                    dbSmartAspects.AddInParameter(command, "@Code", DbType.String, memberTypeBO.Code);
                    dbSmartAspects.AddInParameter(command, "@SubscriptionFee", DbType.String, memberTypeBO.SubscriptionFee);
                    dbSmartAspects.AddInParameter(command, "@DiscountPercent", DbType.String, memberTypeBO.DiscountPercent);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, memberTypeBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public List<MemMemberTypeBO> GetMemberTypeInfoBySearchCriteriaForPaging(string typeName, string code, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<MemMemberTypeBO> memberTypeList = new List<MemMemberTypeBO>();
            DataSet memberTypeDS = new DataSet();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberTypeInfoBySearchCriteriaForPaging_SP"))
                {
                    if (!string.IsNullOrEmpty(typeName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@TypeName", DbType.String, typeName);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@TypeName", DbType.String, DBNull.Value);
                    if (!string.IsNullOrEmpty(code))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, code);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    dbSmartAspects.LoadDataSet(cmd, memberTypeDS, "MemberTypeInfo");
                    DataTable table = memberTypeDS.Tables["MemberTypeInfo"];

                    memberTypeList = table.AsEnumerable().Select(r =>
                                   new MemMemberTypeBO
                                   {
                                       TypeId = r.Field<int>("TypeId"),
                                       Name = r.Field<string>("Name"),
                                       Code = r.Field<string>("Code"),
                                       SubscriptionFee = r.Field<decimal?>("SubscriptionFee"),
                                       DiscountPercent = r.Field<decimal?>("DiscountPercent")
                                   }).ToList();
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return memberTypeList;
        }
        public MemMemberTypeBO GetMemberTypeInfoById(int memberTypeId)
        {
            MemMemberTypeBO memberTypeBO = new MemMemberTypeBO();
            DataSet memberTypeDS = new DataSet();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberTypeInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TypeId", DbType.Int32, memberTypeId);

                    dbSmartAspects.LoadDataSet(cmd, memberTypeDS, "MemberType");
                    DataTable table = memberTypeDS.Tables["MemberType"];

                    memberTypeBO = table.AsEnumerable().Select(r =>
                                   new MemMemberTypeBO
                                   {
                                       TypeId = r.Field<int>("TypeId"),
                                       Name = r.Field<string>("Name"),
                                       Code = r.Field<string>("Code"),
                                       SubscriptionFee = r.Field<decimal?>("SubscriptionFee"),
                                       DiscountPercent = r.Field<decimal?>("DiscountPercent")
                                   }).FirstOrDefault();
                }
            }
            return memberTypeBO;
        }
        public List<MemMemberTypeBO> GetAllMemberType()
        {
            List<MemMemberTypeBO> memberTypeList = new List<MemMemberTypeBO>();
            DataSet memberTypeDS = new DataSet();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllMemberType_SP"))
                {                    
                    dbSmartAspects.LoadDataSet(cmd, memberTypeDS, "MemberType");
                    DataTable table = memberTypeDS.Tables["MemberType"];

                    memberTypeList = table.AsEnumerable().Select(r =>
                                   new MemMemberTypeBO
                                   {
                                       TypeId = r.Field<int>("TypeId"),
                                       Name = r.Field<string>("Name"),
                                       Code = r.Field<string>("Code"),
                                       SubscriptionFee = r.Field<decimal?>("SubscriptionFee"),
                                       DiscountPercent = r.Field<decimal?>("DiscountPercent")
                                   }).ToList();
                }
            }
            return memberTypeList;
        }
    }
}
 