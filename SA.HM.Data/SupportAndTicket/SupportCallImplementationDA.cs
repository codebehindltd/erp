using HotelManagement.Entity.SupportAndTicket;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.SupportAndTicket
{
    public class SupportCallImplementationDA : BaseService
    {
        public List<STSupportBO> GetSupportCallInformationForGridPaging(int clientId, int caseId, string caseNumber, string fromDate, string toDate, string status, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<STSupportBO> STSupportBOList = new List<STSupportBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupportCallInformationForImplementationSearch_SP"))
                    {

                        if (clientId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@ClientId", DbType.Int32, clientId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ClientId", DbType.Int32, DBNull.Value);

                        if (!string.IsNullOrEmpty(caseNumber))
                            dbSmartAspects.AddInParameter(cmd, "@CaseNumber", DbType.String, caseNumber);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@CaseNumber", DbType.String, DBNull.Value);

                        if (caseId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@CaseId", DbType.Int32, caseId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@CaseId", DbType.Int32, DBNull.Value);

                        if (!string.IsNullOrEmpty(fromDate))
                            dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, Convert.ToDateTime(fromDate));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                        if (!string.IsNullOrEmpty(toDate))
                            dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, Convert.ToDateTime(toDate));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                        if (!string.IsNullOrEmpty(status))
                            dbSmartAspects.AddInParameter(cmd, "@Status", DbType.DateTime, status);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Status", DbType.DateTime, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        DataSet SupportDS = new DataSet();

                        dbSmartAspects.LoadDataSet(cmd, SupportDS, "Support");
                        DataTable Table = SupportDS.Tables["Support"];

                        STSupportBOList = Table.AsEnumerable().Select(r => new STSupportBO
                        {
                            Id = r.Field<Int64>("Id"),
                            CaseNumber = r.Field<string>("CaseNumber"),
                            CaseOwnerId = r.Field<int>("CaseOwnerId"),
                            ClientId = r.Field<int>("ClientId"),
                            CompanyName = r.Field<string>("CompanyName"),
                            SupportCategoryId = r.Field<int>("SupportCategoryId"),
                            SupportSource = r.Field<string>("SupportSource"),
                            SupportSourceOtherDetails = r.Field<string>("SupportSourceOtherDetails"),
                            CaseId = r.Field<int>("CaseId"),
                            CaseDetails = r.Field<string>("CaseDetails"),
                            ItemOrServiceDetails = r.Field<string>("ItemOrServiceDetails"),
                            CreatedBy = r.Field<int>("CreatedBy"),
                            CreatedByName = r.Field<string>("CreatedByName"),
                            CreatedDate = r.Field<DateTime>("CreatedDate"),
                            SerialNumber = r.Field<Int64>("SerialNumber"),
                            SupportStageId = r.Field<int>("SupportStageId"),
                            TaskId = r.Field<long?>("TaskId"),
                            IsCompleted = r.Field<bool>("IsCompleted"),
                            CaseName = r.Field<string>("CaseName"),
                            TaskStatus = r.Field<string>("TaskStatus"),
                            BillStatus = r.Field<string>("BillStatus"),
                            SupportStatus = r.Field<string>("SupportStatus"),
                            PassDay = r.Field<int>("PassDay")
                        }).ToList();

                        totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return STSupportBOList;
        }


    }
}
