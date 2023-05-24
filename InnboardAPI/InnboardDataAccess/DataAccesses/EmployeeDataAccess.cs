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
using InnboardDomain.Models.Payroll;

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

        public async  Task<List<PayrollEmpTracking>> GetEmpTrackingList(int? EmpId, DateTime FromDate, DateTime ToDate)
        {
            string query;
            if (EmpId != null)
            {
                query = string.Format($@"Select * from [dbo].[PayrollEmpLocationTracking] WHERE EmpId='{EmpId}' AND AttDateTime between '{FromDate.ToShortDateString()}' AND '{ToDate.ToShortDateString()}'
	                        order by [CreatedDate] desc");
            }
            else
            {
                query = string.Format(@"SELECT TOP(50) * FROM [dbo].[PayrollEmpLocationTracking] ORDER BY [CreatedDate] DESC");
            }
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

        public async Task<List<LeaveTakenNBalanceBO>> GetLeaveType(int? empId, DateTime currentDate)
        {
            SqlParameter param1 = new SqlParameter("@EmpId", empId);
            SqlParameter param2 = new SqlParameter("@CurrentDate", currentDate);
            var result = await InnboardDBContext.Database.SqlQuery<LeaveTakenNBalanceBO>("EXEC [dbo].[GetLeaveTakenNBalanceByEmployee_SP] @EmpId, @CurrentDate", param1, param2).ToListAsync();
            return result;
        }

        public async Task<List<PayrollEmpLocationInfo>> GetActiveEmployees()
        {
            var result = await InnboardDBContext.Database.SqlQuery<PayrollEmpLocationInfo>("EXEC [dbo].[GetActiveEmployeeInfo_SP]").ToListAsync();
            return result;
        }

        public bool LeaveApplication(LeaveInformationBO model)
        {
            SqlParameter empId = new SqlParameter("@EmpId", model.EmpId);
            SqlParameter leaveMode = new SqlParameter("@LeaveMode ", model.LeaveMode);
            SqlParameter leaveTypeId = new SqlParameter("@leaveTypeId ", model.LeaveTypeId);
            SqlParameter fromDate = new SqlParameter("@FromDate ", model.FromDate);
            SqlParameter toDate = new SqlParameter("@ToDate ", model.ToDate);
            SqlParameter transactionType = new SqlParameter("@TransactionType ", model.TransactionType);
            if (model.TransactionType == null)
            {
                transactionType.Value = DBNull.Value;
            }
            SqlParameter noOfDays = new SqlParameter("@NoOfDays ", model.NoOfDays);
            SqlParameter workHandover = new SqlParameter("@WorkHandover ", model.WorkHandover);
            SqlParameter expireDate = new SqlParameter("@ExpireDate ", model.ExpireDate);
            if(model.ExpireDate == null)
            {
                expireDate.Value = DBNull.Value;
            }
            SqlParameter leaveStatus = new SqlParameter("@LeaveStatus ", model.LeaveStatus);
            SqlParameter reason = new SqlParameter("@Reason ", model.Reason);
            SqlParameter reportingTo = new SqlParameter("@ReportingTo ", model.ReportingTo);

            SqlParameter createdBy = new SqlParameter("@CreatedBy ", model.CreatedBy);
            SqlParameter leaveId = new SqlParameter("@LeaveId ", model.LeaveId);
            int result = InnboardDBContext.Database.ExecuteSqlCommand("EXEC [dbo].[SaveEmpLeaveInformation_SP]  @EmpId , @LeaveMode, @LeaveTypeId, @FromDate, @ToDate, @TransactionType, @NoOfDays, @WorkHandover, @ExpireDate, @LeaveStatus, @Reason, @ReportingTo, @CreatedBy, @LeaveId ", empId, leaveMode, leaveTypeId, fromDate, toDate, transactionType, noOfDays, workHandover, expireDate, leaveStatus, reason, reportingTo, createdBy, leaveId);

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<LeaveInformationBO>> GetLeaveInformationBySearchCriteria(LeaveInformationBO model)
        {
            List<LeaveInformationBO> leaveInformationBOList = new List<LeaveInformationBO>();

            //var isAdminUser = 0;
            //var leaveType = string.IsNullOrEmpty(criteriaDto.TypeName) ? "null" : "'" + criteriaDto.TypeName + "'";
            //var leaveMode = string.IsNullOrEmpty(criteriaDto.LeaveMode) ? "null" : "'" + criteriaDto.LeaveMode + "'";
            //var leaveFromDate = criteriaDto.FromDate == null ? "null" : "'" + criteriaDto.FromDate + "'";
            //var leaveToDate = criteriaDto.ToDate == null ? "null" : "'" + criteriaDto.ToDate + "'";
            //var leaveStatus = string.IsNullOrEmpty(criteriaDto.LeaveStatus) ? "null" : "'" + criteriaDto.LeaveStatus + "'";

            //string query = $@"EXEC [dbo].[GetEmpLeaveInformationByEmpIdForPaging_SP] "+ isAdminUser + $" {criteriaDto.CreatedBy}, {criteriaDto.EmpId}, {criteriaDto.TypeName}, {criteriaDto.LeaveMode}, " + leaveFromDate + $", " + leaveToDate + $", " + leaveStatus + $", {criteriaDto.pageParams.PageSize}, {criteriaDto.pageParams.PageNumber}, NULL";
            //leaveInformationBOList = await InnboardDBContext.Database.SqlQuery<LeaveInformationBO>(query).ToListAsync();
            int adminId = 0;
            SqlParameter isAdminUser = new SqlParameter("@IsAdminUser", adminId);
            SqlParameter userId = new SqlParameter("@UserId", model.CreatedBy);
            SqlParameter empId = new SqlParameter("@EmployeeID", model.EmployeeId);
            if (model.EmployeeId == null)
            {
                empId.Value = DBNull.Value;
            }
            SqlParameter leaveStatus = new SqlParameter("@LeaveStatus", model.LeaveStatus);
            if (model.LeaveStatus == null)
            {
                leaveStatus.Value = DBNull.Value;
            }
            SqlParameter leaveType = new SqlParameter("@LeaveType", model.LeaveType);
            if (model.LeaveType == null)
            {
                leaveType.Value = DBNull.Value;
            }

            SqlParameter fromDate = new SqlParameter("@LeaveFromDate", model.LeaveFromDate);
            if (model.LeaveFromDate == null)
            {
                fromDate.Value = DBNull.Value;
            }
            SqlParameter toDate = new SqlParameter("@LeaveToDate", model.LeaveToDate);
            if (model.LeaveToDate == null)
            {
                toDate.Value = DBNull.Value;
            }
            SqlParameter leaveMode = new SqlParameter("@LeaveMode", model.LeaveMode);
            if (model.LeaveMode == null)
            {
                leaveMode.Value = DBNull.Value;
            }
            SqlParameter perPage = new SqlParameter("@RecordPerPage", model.pageParams.PageSize);
            SqlParameter pageIndex = new SqlParameter("@PageIndex", model.pageParams.PageNumber);
            SqlParameter recordCount = new SqlParameter("@RecordCount", 100);

            var result = await InnboardDBContext.Database.SqlQuery<LeaveInformationBO>("EXEC [dbo].[GetEmpLeaveInformationByEmpIdForPaging_SP] @IsAdminUser, @UserId, @EmployeeID, @LeaveStatus, @LeaveType, @LeaveFromDate, @LeaveToDate, @LeaveMode, @RecordPerPage, @PageIndex, @RecordCount", isAdminUser, userId, empId, leaveStatus, leaveType, fromDate, toDate, leaveMode, perPage, pageIndex, recordCount).ToListAsync();
            return result;
        }

        public async Task<List<LeaveInformationBO>> GetLeaveInformation(int userId, int leaveId)
        {
            SqlParameter param1 = new SqlParameter("@UserInfoId", userId);
            SqlParameter param2 = new SqlParameter("@LeaveId", leaveId);
         
            var result = await InnboardDBContext.Database.SqlQuery<LeaveInformationBO>("EXEC [dbo].[GetEmpLeaveInformationByIdForApps_SP_SP] @UserInfoId, @LeaveId", param1, param2).ToListAsync();
            return result;
        }

        public bool LeaveApproval(LeaveInformationBO model)
        {
            SqlParameter leaveId = new SqlParameter("@LeaveId", model.LeaveId);
            SqlParameter leaveStatus = new SqlParameter("@LeaveStatus", model.LeaveStatus);
            SqlParameter cancelReason = new SqlParameter("@CancelReason", model.CancelReason);
            if(model.CancelReason == null)
            {
                cancelReason.Value = DBNull.Value;
            }
            SqlParameter lastModifiedBy = new SqlParameter("@LastModifiedBy", model.LastModifiedBy);
            
            int result = InnboardDBContext.Database.ExecuteSqlCommand("EXEC [dbo].[UpdateEmpLeaveStatus_SP]  @LeaveId, @LeaveStatus, @CancelReason, @LastModifiedBy", leaveId, leaveStatus, cancelReason, lastModifiedBy);

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
