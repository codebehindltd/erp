using HotelManagement.Entity.SalesAndMarketing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.SalesAndMarketing
{
    public class CompanySignupStatusDA : BaseService
    {
        public Boolean SaveOrUpdateSignupStatus(SMCompanySignupStatus signupStatus, out int id)
        {
            Boolean status = false;
            try {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateSignupStatus_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, signupStatus.Id);
                        dbSmartAspects.AddInParameter(command, "@Status", DbType.String, signupStatus.Status);
                        dbSmartAspects.AddInParameter(command, "@IsActive", DbType.Boolean, signupStatus.IsActive);
                        dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, signupStatus.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));

                        status = (dbSmartAspects.ExecuteNonQuery(command) > 0);

                        id = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public List<SMCompanySignupStatus> GetAllSignupStatus()
        {
            List<SMCompanySignupStatus> statuseList = new List<SMCompanySignupStatus>();

            string query = string.Format("SELECT * FROM SMCompanySignupStatus WHERE IsActive = '1'");

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet dataSet = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, dataSet, "Status");
                    DataTable Table = dataSet.Tables["Status"];

                    statuseList = Table.AsEnumerable().Select(r => new SMCompanySignupStatus
                    {
                        Id = r.Field<int>("Id"),
                        Status = r.Field<string>("Status"),
                        IsActive = r.Field<bool>("IsActive")

                    }).ToList();
                }
            }
            return statuseList;
        }
        public List<SMCompanySignupStatus> GetSignupStatusBySearchCriteria(string name, int isActive)
        {
            List<SMCompanySignupStatus> statusList = new List<SMCompanySignupStatus>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSignupStatusBySearchCriteria_SP"))
                {
                    if (!String.IsNullOrEmpty(name))
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, name);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, DBNull.Value);
                    dbSmartAspects.AddInParameter(cmd, "@IsActive", DbType.Int32, isActive);

                    DataSet dataSet = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, dataSet, "Status");
                    DataTable Table = dataSet.Tables["Status"];

                    statusList = Table.AsEnumerable().Select(r => new SMCompanySignupStatus
                    {
                        Id = r.Field<int>("Id"),
                        Status = r.Field<string>("Status"),
                        IsActive = r.Field<bool>("IsActive")

                    }).ToList();
                }
            }
            return statusList;
        }
        public SMCompanySignupStatus GetSignupStatusById(int id)
        {
            SMCompanySignupStatus status = new SMCompanySignupStatus();
            string query = string.Format("SELECT * FROM SMCompanySignupStatus WHERE Id = {0}", id);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet dataSet = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, dataSet, "Status");
                    DataTable Table = dataSet.Tables["Status"];

                    status = Table.AsEnumerable().Select(r => new SMCompanySignupStatus
                    {
                        Id = r.Field<int>("Id"),
                        Status = r.Field<string>("Status"),
                        IsActive = r.Field<bool>("IsActive")

                    }).FirstOrDefault();
                }
            }
            return status;
        }
    }
}
