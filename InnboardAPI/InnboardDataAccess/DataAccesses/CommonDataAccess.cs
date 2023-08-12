using InnboardAPI.DataAccesses;
using InnboardDomain.Models;
using InnboardDomain.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDataAccess.DataAccesses
{
    public class CommonDataAccess : GenericDataAccess<CustomFieldBO>
    {
        public async Task<List<CommonCompanyProfileBO>> GetCompanyInfo()
        {
            var result = await InnboardDBContext.Database.SqlQuery<CommonCompanyProfileBO>("EXEC [dbo].[GetCompanyInfo_SP]").ToListAsync();
            return result;
        }

        public async Task<List<CustomFieldBO>> GetCustomField(string fieldType)
        {
            SqlParameter param1 = new SqlParameter("@FieldName", fieldType);
            var result = await InnboardDBContext.Database.SqlQuery<CustomFieldBO>("EXEC [dbo].[GetCustomField_SP] @FieldName", param1).ToListAsync();
            return result;
        }

        public async Task<List<PropertyInformationBO>> GetPropertyInformation(string transactionType, int transactionId)
        {
            SqlParameter paramTransactionType = new SqlParameter("@TransactionType", transactionType);
            SqlParameter paramTransactionId = new SqlParameter("@TransactionId", transactionId);
            var result = await InnboardDBContext.Database.SqlQuery<PropertyInformationBO>("EXEC [dbo].[GetPropertyInformationForApps_SP] @TransactionType, @TransactionId", paramTransactionType, paramTransactionId).ToListAsync();
            return result;
        }

        public async Task<GuestOrMemberProfileBO> GetGuestOrMemberProfileInformation(string transactionType, int transactionId)
        {
            SqlParameter paramTransactionType = new SqlParameter("@TransactionType", transactionType);
            SqlParameter paramTransactionId = new SqlParameter("@TransactionId", transactionId);
            var result = await InnboardDBContext.Database.SqlQuery<GuestOrMemberProfileBO>("EXEC [dbo].[GetGuestOrMemberProfileInformation_SP] @TransactionType, @TransactionId", paramTransactionType, paramTransactionId).ToListAsync();
            return result.FirstOrDefault();
        }

        public async Task<List<GuestOrMemberPromotionalOfferBO>> GetGuestOrMemberPromotionalOffer(string transactionType, int transactionId)
        {
            SqlParameter paramTransactionType = new SqlParameter("@TransactionType", transactionType);
            SqlParameter paramTransactionId = new SqlParameter("@TransactionId", transactionId);
            var result = await InnboardDBContext.Database.SqlQuery<GuestOrMemberPromotionalOfferBO>("EXEC [dbo].[GetGuestOrMemberPromotionalOffer_SP] @TransactionType, @TransactionId", paramTransactionType, paramTransactionId).ToListAsync();
            return result;
        }
    }
}
