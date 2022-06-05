using HotelManagement.Entity.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.Inventory
{
    public class InvServicePackageDA : BaseService
    {
        public List<InvServicePackage> GetAllServicePackageWithPagination(string name, bool isActive, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<InvServicePackage> servicePackages = new List<InvServicePackage>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllServicePackageWithPagination_SP"))
                    {
                        if (!string.IsNullOrEmpty(name))
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, name);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);
                        dbSmartAspects.AddInParameter(cmd, "@IsActive", DbType.Boolean, isActive);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));


                        DataSet dataSet = new DataSet();

                        dbSmartAspects.LoadDataSet(cmd, dataSet, "Stage");
                        DataTable Table = dataSet.Tables["Stage"];

                        servicePackages = Table.AsEnumerable().Select(r => new InvServicePackage
                        {
                            ServicePackageId = r.Field<int>("ServicePackageId"),
                            PackageName = r.Field<string>("PackageName"),
                            IsActive = r.Field<bool?>("IsActive"),
                            CreatedBy = r.Field<int?>("CreatedBy"),
                            CreatedDate = r.Field<DateTime?>("CreatedDate"),
                            LastModifiedBy = r.Field<int?>("LastModifiedBy"),
                            LastModifiedDate= r.Field<DateTime?>("LastModifiedDate")
                        }).ToList();

                        totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return servicePackages;
        }

        public bool SaveOrUpdatePackage(InvServicePackage package, out int id)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateServicePackage_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ServicePackageId", DbType.Int32, package.ServicePackageId);
                        dbSmartAspects.AddInParameter(command, "@PackageName", DbType.String, package.PackageName);
                        dbSmartAspects.AddInParameter(command, "@IsActive", DbType.Boolean, package.IsActive);
                        dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, package.CreatedBy);
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

    }
}
