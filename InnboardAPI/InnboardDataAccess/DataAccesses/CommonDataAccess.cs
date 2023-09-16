using InnboardAPI.DataAccesses;
using InnboardDataAccess.DataAccesses.common;
using InnboardDomain.Models;
using InnboardDomain.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Linq;

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

        public async Task<List<GuestOrMemberPromotionalOfferBO>> GetMemberRoomNightInformation(int transactionId)
        {
            var offerList = await getMemberOfferList("MemberRoomReservation", transactionId);
            return offerList;

        }

        public async Task<List<GuestOrMemberPromotionalOfferBO>> GetMemberRoomNightPromotionalOffer(int transactionId)
        {


            SqlParameter paramTransactionId = new SqlParameter("@TransactionId", transactionId);
            var result = await InnboardDBContext.Database.SqlQuery<GuestOrMemberPromotionalOfferBO>("EXEC [dbo].[GetMemberRoomNightPromotionalOffer_SP] @TransactionId", paramTransactionId).ToListAsync();
            return result;
        }
        

        public async Task<List<GuestOrMemberPromotionalOfferBO>> GetGuestOrMemberPromotionalOffer(string transactionType, int transactionId)
        {
            
            SqlParameter paramTransactionType = new SqlParameter("@TransactionType", transactionType);
            SqlParameter paramTransactionId = new SqlParameter("@TransactionId", transactionId);
            var result = await InnboardDBContext.Database.SqlQuery<GuestOrMemberPromotionalOfferBO>("EXEC [dbo].[GetGuestOrMemberPromotionalOffer_SP] @TransactionType, @TransactionId", paramTransactionType, paramTransactionId).ToListAsync();
            return result;
        }

        private async Task<List<GuestOrMemberPromotionalOfferBO>> getMemberOfferList(string transactionType, int transactionId)
        {
            List<GuestOrMemberPromotionalOfferBO> offerList = new List<GuestOrMemberPromotionalOfferBO>();
            var propertyList = await GetPropertyInformation(transactionType, transactionId);
            if (propertyList != null)
            {
                foreach (PropertyInformationBO row in propertyList)
                {
                    if(row.EndPointIp != null)
                    {
                        var data = HTTPRequest.HttpGet(row.EndPointIp + "api/Common/GetMemberRoomNightPromotionalOffer?transactionId=" + transactionId);

                        // prepare jsn data
                        JavaScriptSerializer j = new JavaScriptSerializer();
                        List<GuestOrMemberPromotionalOfferBO> a = (List<GuestOrMemberPromotionalOfferBO>)j.Deserialize(data, typeof(List<GuestOrMemberPromotionalOfferBO>));


                        offerList.AddRange(a);
                    }
                    

                }
            }
            return offerList;


        }
    }
}
