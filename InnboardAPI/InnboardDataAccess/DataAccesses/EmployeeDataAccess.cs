using InnboardAPI.DataAccesses;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using InnboardDomain.ViewModel;
using InnboardDomain.Utility;
using InnboardDomain.CriteriaDtoModel;

namespace InnboardDataAccess.DataAccesses
{
    public class EmployeeDataAccess : GenericDataAccess<PayrollEmpTracking>
    {
        public async Task<bool> EmpLocationTrackingSave(PayrollEmpTracking model)
        {
            SqlParameter param1 = new SqlParameter("@EmpId", model.EmpId);
            SqlParameter param2 = new SqlParameter("@Latitude", SqlDbType.Float);
            param2.Value = Convert.ToDecimal(model.Latitude);
            SqlParameter param3 = new SqlParameter("@Longitude", SqlDbType.Float);
            param3.Value = Convert.ToDecimal(model.Longitude);
            SqlParameter param4 = new SqlParameter("@AttDateTime", model.AttDateTime);
            SqlParameter param5 = new SqlParameter("@DeviceInfo", model.DeviceInfo);
            SqlParameter param6 = new SqlParameter("@GoogleMapUrl", model.GoogleMapUrl);
            SqlParameter param7 = new SqlParameter("@CreatedBy", model.CreatedBy);

            int result = InnboardDBContext.Database.ExecuteSqlCommand("EXEC [dbo].[SavePayrollEmpLocationTracking_SP] @EmpId,@Latitude,@Longitude,@AttDateTime,@DeviceInfo,@GoogleMapUrl,@CreatedBy", param1, param2, param3, param4, param5, param6, param7);

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async  Task<List<PayrollEmpTracking>> GetEmpTrackingList()
        {
            string query = string.Format(@"Select TOP (50) * from [dbo].[PayrollEmpLocationTracking] order by [CreatedDate] desc");

            var data = await InnboardDBContext.Database.SqlQuery<PayrollEmpTracking>(query).ToListAsync();           

            return data;
        }

        public async Task<List<PayrollEmpLocationInfo>> GetPayrollEmpListWithLocation(PayrollEmpCriteriaDto criteriaDto)
        {
            if (criteriaDto == null)
            {
                criteriaDto = new PayrollEmpCriteriaDto();
            }          
            
            string query = string.Format($@"EXEC [dbo].[GetPayrollEmpLocationTracking_SP] {criteriaDto.UserInfoId}, '{criteriaDto.pageParams.SearchKey}', {criteriaDto.pageParams.PageNumber}, {criteriaDto.pageParams.PageSize}");
            var data = await InnboardDBContext.Database.SqlQuery<PayrollEmpLocationInfo>(query).ToListAsync();            
            return data;
        }
    }
}
