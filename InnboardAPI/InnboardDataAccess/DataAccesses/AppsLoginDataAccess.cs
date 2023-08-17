﻿using InnboardAPI.DataAccesses;
using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using InnboardDomain.ViewModel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace InnboardDataAccess.DataAccesses
{
    public class AppsLoginDataAccess : GenericDataAccess<AppsLoginModel>, IAppsLogin
    {
        public UserInfoModel LoginUser(AppsLoginModel loginModel)
        {
            string query = string.Format(@"EXEC [dbo].[GetUserInformationByUserIdNHashPassword_SP] ");

            SqlParameter param1 = new SqlParameter("@UserId", loginModel.UserName);
            SqlParameter param2 = new SqlParameter("@UserPassword", loginModel.Password);

            UserInfoModel userInfo = InnboardDBContext.Database.SqlQuery<UserInfoModel>("EXEC [dbo].[GetUserInformationByUserNameNId_SP] @UserId, @UserPassword", param1, param2).FirstOrDefault();
            return userInfo;
        }
        public UserInfoModel GetGuestOrMemberInformationByUserIdNPassword(string userType, string userId, string userPassword)
        {
            SqlParameter paramUserType = new SqlParameter("@UserType", userType);
            SqlParameter paramUserId = new SqlParameter("@UserId", userId);
            SqlParameter paramUserPassword = new SqlParameter("@UserPassword", userPassword);

            UserInfoModel userInfo = InnboardDBContext.Database.SqlQuery<UserInfoModel>("EXEC [dbo].[GetGuestOrMemberInformationByUserIdNPassword_SP] @UserType, @UserId, @UserPassword", paramUserType, paramUserId, paramUserPassword).FirstOrDefault();
            return userInfo;
        }
        public UserInfoModel LoginUserByMasterUserInfoId(int masterUserInfoId)
        {
            SqlParameter paramMasterUserInfoId = new SqlParameter("@MasterUserInfoId", masterUserInfoId);
            UserInfoModel userInfo = InnboardDBContext.Database.SqlQuery<UserInfoModel>("EXEC [dbo].[GetUserInformationByMasterUserInfoId_SP] @MasterUserInfoId", paramMasterUserInfoId).FirstOrDefault();
            return userInfo;
        }

        public bool AppUserAttendanceSave(AppAttendanceModel appAttModel)
        {
            SqlParameter param1 = new SqlParameter("@EmpId", appAttModel.EmpId);
            SqlParameter param2 = new SqlParameter("@Latitude", SqlDbType.Float);
            param2.Value = Convert.ToDecimal(appAttModel.Latitude);
            SqlParameter param3 = new SqlParameter("@Longitude", SqlDbType.Float);
            param3.Value = Convert.ToDecimal(appAttModel.Longitude);
            SqlParameter param4 = new SqlParameter("@AttDateTime", appAttModel.AttDateTime);
            SqlParameter param5 = new SqlParameter("@IntAttDateTime", appAttModel.IntAttDateTime);
            SqlParameter param6 = new SqlParameter("@Image", appAttModel.Image); 
            SqlParameter param7 = new SqlParameter("@GoogleMapUrl", appAttModel.GoogleMapUrl); 

            //string reservationNumber = InnboardDBContext.Database.SqlQuery<string>("EXEC [dbo].[PayrollEmpAttendanceLogMobileApp_Insert_SP] @EmpId,@Latitude,@Longitude, @AttDateTime, @Image", param1, param2, param3, param4, param5);
            int result = InnboardDBContext.Database.ExecuteSqlCommand("EXEC [dbo].[PayrollEmpAttendanceLogMobileApp_Insert_SP] @EmpId,@Latitude,@Longitude, @AttDateTime, @IntAttDateTime, @Image, @GoogleMapUrl", param1, param2, param3, param4, param5, param6, param7);
            
            if(result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SaveUpdateEmpAttendenceInfoForMobileApps(AppAttendanceModel appAttModel)
        {
            SqlParameter empId = new SqlParameter("@EmpId", appAttModel.EmpId);            
            SqlParameter attendanceDate = new SqlParameter("@AttendanceDate", appAttModel.AttendanceDate);
            SqlParameter entryTime = new SqlParameter("@EntryTime", appAttModel.EntryTime);
            if (appAttModel.EntryTime == null)
            {
                entryTime.Value = DBNull.Value;
            }
            SqlParameter exitTime = new SqlParameter("@ExitTime", appAttModel.ExitTime);
            if (appAttModel.ExitTime == null)
            {
                exitTime.Value = DBNull.Value;
            }
            SqlParameter remark = new SqlParameter("@Remark", appAttModel.Remark);
            SqlParameter userInfoId = new SqlParameter("@UserInfoId", appAttModel.UserInfoId);

            //string reservationNumber = InnboardDBContext.Database.SqlQuery<string>("EXEC [dbo].[PayrollEmpAttendanceLogMobileApp_Insert_SP] @EmpId,@Latitude,@Longitude, @AttDateTime, @Image", param1, param2, param3, param4, param5);
            int result = InnboardDBContext.Database.ExecuteSqlCommand("EXEC [dbo].[SaveUpdateEmpAttendenceInfoForMobileApps_SP] @EmpId,@AttendanceDate,@EntryTime, @ExitTime, @Remark, @UserInfoId", empId, attendanceDate, entryTime, exitTime, remark, userInfoId);

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
