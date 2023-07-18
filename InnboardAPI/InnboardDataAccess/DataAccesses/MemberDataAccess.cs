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
using InnboardDomain.Models.Membership;

namespace InnboardDataAccess.DataAccesses
{
    public class MemberDataAccess : GenericDataAccess<GetMembershipSetupDataBO>
    {
        public async Task<List<GetMembershipSetupDataBO>> GetMembershipSetupData()
        {
            var result = await InnboardDBContext.Database.SqlQuery<GetMembershipSetupDataBO>("EXEC [dbo].[GetMembershipSetupData_SP]").ToListAsync();
            return result;
        }

        public bool SaveMemMemberBasicInfoForMobileAppsRegistration(MemMemberBasics memberBasicInfo, out int outMemberId)
        {
            SqlParameter typeId = new SqlParameter("@TypeId", memberBasicInfo.TypeId);
            SqlParameter fullName = new SqlParameter("@FullName", memberBasicInfo.FullName);
            SqlParameter mobileNumber = new SqlParameter("@MobileNumber", memberBasicInfo.MobileNumber);
            SqlParameter personalEmail = new SqlParameter("@PersonalEmail", memberBasicInfo.PersonalEmail);            
            SqlParameter createdBy = new SqlParameter("@CreatedBy", memberBasicInfo.CreatedBy);

            SqlParameter pOutMemberId = new SqlParameter("@MemberId", SqlDbType.Int);
            pOutMemberId.Direction = ParameterDirection.Output;

            int result = InnboardDBContext.Database.ExecuteSqlCommand("EXEC [dbo].[SaveMemMemberBasicInfoForMobileAppsRegistration_SP] @TypeId, @FullName, @MobileNumber, @PersonalEmail, @CreatedBy, @MemberId OUT", typeId, fullName, mobileNumber, personalEmail, createdBy, pOutMemberId);

            outMemberId = (int)pOutMemberId.Value;

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool SaveMemberPaymentInfoForMobileAppsRegistration(MemMemberBasics memberBasicInfo)
        {
            SqlParameter memberId = new SqlParameter("@MemberId", memberBasicInfo.MemberId);
            SqlParameter transactionType = new SqlParameter("@TransactionType", memberBasicInfo.TransactionType);
            SqlParameter transactionId = new SqlParameter("@TransactionId", memberBasicInfo.TransactionId);
            SqlParameter securityDeposit = new SqlParameter("@SecurityDeposit", memberBasicInfo.SecurityDeposit);
            SqlParameter transactionDetails = new SqlParameter("@TransactionDetails", memberBasicInfo.TransactionDetails);
            SqlParameter createdBy = new SqlParameter("@CreatedBy", memberBasicInfo.CreatedBy);

            int result = InnboardDBContext.Database.ExecuteSqlCommand("EXEC [dbo].[SaveMemberPaymentInfoForMobileAppsRegistration_SP] @MemberId, @TransactionType, @TransactionId, @SecurityDeposit, @TransactionDetails, @CreatedBy", memberId, transactionType, transactionId, securityDeposit, transactionDetails, createdBy);

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
