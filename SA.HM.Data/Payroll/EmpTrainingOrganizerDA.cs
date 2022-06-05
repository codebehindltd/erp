using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class EmpTrainingOrganizerDA: BaseService
    {
        public Boolean SaveEmpTrainingOrganizerInfo(EmpTrainingOrganizerBO organizerInfo, out int tmpId)
        {
            Boolean status = false;
            tmpId = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpTrngOrganizerInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@OrganizerName", DbType.String, organizerInfo.OrganizerName);
                        dbSmartAspects.AddInParameter(command, "@Address", DbType.String, organizerInfo.Address);
                        dbSmartAspects.AddInParameter(command, "@Email", DbType.String, organizerInfo.Email);
                        dbSmartAspects.AddInParameter(command, "@ContactNo", DbType.String, organizerInfo.ContactNo);
                        dbSmartAspects.AddInParameter(command, "@TrainingType", DbType.String, organizerInfo.TrainingType);
                        dbSmartAspects.AddOutParameter(command, "@OrganizerId", DbType.Int32, organizerInfo.OrganizerId);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpId = Convert.ToInt32(command.Parameters["@OrganizerId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public Boolean UpdateEmpTrainingOrganizerInfo(EmpTrainingOrganizerBO organizerInfo)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpTrngOrganizerInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@OrganizerId", DbType.Int32, organizerInfo.OrganizerId);
                        dbSmartAspects.AddInParameter(command, "@OrganizerName", DbType.String, organizerInfo.OrganizerName);
                        dbSmartAspects.AddInParameter(command, "@Address", DbType.String, organizerInfo.Address);
                        dbSmartAspects.AddInParameter(command, "@Email", DbType.String, organizerInfo.Email);
                        dbSmartAspects.AddInParameter(command, "@ContactNo", DbType.String, organizerInfo.ContactNo);
                        dbSmartAspects.AddInParameter(command, "@TrainingType", DbType.String, organizerInfo.TrainingType);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public List<EmpTrainingOrganizerBO> GetOrganizerInfoBySearchCriteriaForPagination(string organizerName, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<EmpTrainingOrganizerBO> advtknList = new List<EmpTrainingOrganizerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTrngOrganizerInfoBySearchCriteriaForPagination_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@OrganizerName", DbType.String, organizerName);                                 

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollEmpTrainingOrganizer");
                    DataTable Table = ds.Tables["PayrollEmpTrainingOrganizer"];

                    advtknList = Table.AsEnumerable().Select(r => new EmpTrainingOrganizerBO
                    {
                        OrganizerId = r.Field<Int32>("OrganizerId"),
                        OrganizerName = r.Field<String>("OrganizerName"),
                        Address = r.Field<String>("Address"),
                        Email = r.Field<String>("Email"),
                        ContactNo = r.Field<String>("ContactNo"),
                        TrainingType = r.Field<String>("TrainingType")
                     
                    }).ToList();
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return advtknList;
        }

        public EmpTrainingOrganizerBO GetOrganizerInfoById(int organizerId)
        {
            EmpTrainingOrganizerBO organizerBO = new EmpTrainingOrganizerBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOrganizerInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@OrganizerId", DbType.Int32, organizerId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollEmpTrainingOrganizer");
                    DataTable Table = ds.Tables["PayrollEmpTrainingOrganizer"];

                    organizerBO = Table.AsEnumerable().Select(r => new EmpTrainingOrganizerBO
                    {
                        OrganizerId = r.Field<Int32>("OrganizerId"),
                        OrganizerName = r.Field<String>("OrganizerName"),
                        Address = r.Field<String>("Address"),
                        Email = r.Field<String>("Email"),
                        ContactNo = r.Field<String>("ContactNo"),
                        TrainingType = r.Field<String>("TrainingType")

                    }).FirstOrDefault();
                }
            }
            return organizerBO;
        }

        public Boolean DeleteOrganizerInfoById(int itemId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteOrganizerInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@OrganizerId", DbType.Int32, itemId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public List<EmpTrainingOrganizerBO> GetAllOrganizer()
        {
            List<EmpTrainingOrganizerBO> organizerBOList = new List<EmpTrainingOrganizerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllOrganizerInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpTrainingOrganizerBO organizerBO = new EmpTrainingOrganizerBO();

                                organizerBO.OrganizerId = Convert.ToInt32(reader["OrganizerId"]);
                                organizerBO.OrganizerName = reader["OrganizerName"].ToString();

                                organizerBOList.Add(organizerBO);
                            }
                        }
                    }
                }
            }
            return organizerBOList;
        }
    }
}
