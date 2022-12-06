using InnboardAPI.DataAccesses;
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

            UserInfoModel userInfo = InnboardDBContext.Database.SqlQuery<UserInfoModel>("EXEC [dbo].[GetUserInformationByUserNameNId_SP] @UserId,@UserPassword", param1, param2).FirstOrDefault();
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


    }
    
}
