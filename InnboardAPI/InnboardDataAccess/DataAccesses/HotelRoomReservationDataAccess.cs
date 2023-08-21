using InnboardAPI;
using InnboardAPI.DataAccesses;
using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDataAccess.DataAccesses
{
    public class HotelRoomReservationDataAccess : GenericDataAccess<HotelRoomReservationMobileAppsBO>
    {
        public async Task<List<HotelRoomReservation>> GetRoomReservationInformationForMobileApps(int propertyId, string transactionType, int transactionId, DateTime fromDate, DateTime toDate)
        {
            SqlParameter paramPropertyId = new SqlParameter("@PropertyId", propertyId);
            SqlParameter paramTransactionType = new SqlParameter("@TransactionType", transactionType);
            SqlParameter paramTransactionId = new SqlParameter("@TransactionId", transactionId);
            SqlParameter paramFromDate = new SqlParameter("@FromDate", fromDate);
            SqlParameter paramToDate = new SqlParameter("@ToDate", toDate);
            var result = await InnboardDBContext.Database.SqlQuery<HotelRoomReservation>("EXEC [dbo].[GetRoomReservationInformationForMobileApps_SP] @PropertyId, @TransactionType, @TransactionId, @FromDate, @ToDate", paramPropertyId, paramTransactionType, paramTransactionId, paramFromDate, paramToDate).ToListAsync();
            return result;
        }
        public bool SaveRoomReservationInfoForMobileApps(HotelRoomReservationMobileAppsBO roomReservationInfo, out int tmpGuestId, out long tmpReservationId)
        {
            SqlParameter guestSourceId = new SqlParameter("@GuestSourceId", roomReservationInfo.GuestSourceId);
            SqlParameter transactionType = new SqlParameter("@TransactionType", roomReservationInfo.TransactionType);
            SqlParameter memberId = new SqlParameter("@MemberId", roomReservationInfo.MemberId);
            SqlParameter fromDate = new SqlParameter("@FromDate", roomReservationInfo.FromDate);
            SqlParameter toDate = new SqlParameter("@ToDate", roomReservationInfo.ToDate);
            SqlParameter guestName = new SqlParameter("@GuestName", roomReservationInfo.GuestName);
            SqlParameter phoneNumber = new SqlParameter("@PhoneNumber", roomReservationInfo.PhoneNumber);
            SqlParameter guestRemarks = new SqlParameter("@GuestRemarks", roomReservationInfo.GuestRemarks);

            SqlParameter pOutGuestId = new SqlParameter("@GuestId", SqlDbType.Int);
            pOutGuestId.Direction = ParameterDirection.Output;

            SqlParameter pOutReservationId = new SqlParameter("@ReservationId", SqlDbType.BigInt);
            pOutReservationId.Direction = ParameterDirection.Output;

            int result = InnboardDBContext.Database.ExecuteSqlCommand("EXEC [dbo].[SaveRoomReservationInfoForMobileApps_SP] @GuestSourceId, @TransactionType, @MemberId, @FromDate, @ToDate, @GuestName, @PhoneNumber, @GuestRemarks, @GuestId OUT, @ReservationId OUT", guestSourceId, transactionType, memberId, fromDate, toDate, guestName, phoneNumber, guestRemarks, pOutGuestId, pOutReservationId);

            tmpGuestId = (int)pOutGuestId.Value;
            tmpReservationId = (long)pOutReservationId.Value;

            if (result > 0)
            {
                foreach (HotelRoomReservationDetailsMobileAppsBO row in roomReservationInfo.HotelRoomReservationDetails)
                {
                    SqlParameter reservationId = new SqlParameter("@ReservationId", tmpReservationId);
                    SqlParameter roomTypeId = new SqlParameter("@RoomTypeId", row.RoomTypeId);
                    SqlParameter roomQuantity = new SqlParameter("@RoomQuantity", row.RoomQuantity);
                    SqlParameter paxQuantity = new SqlParameter("@PaxQuantity", row.PaxQuantity);
                    SqlParameter childQuantity = new SqlParameter("@ChildQuantity", row.ChildQuantity);
                    SqlParameter extraBedQuantity = new SqlParameter("@ExtraBedQuantity", row.ExtraBedQuantity);
                    SqlParameter guestNotes = new SqlParameter("@GuestNotes", row.GuestNotes);

                    int result2 = InnboardDBContext.Database.ExecuteSqlCommand("EXEC [dbo].[SaveRoomReservationDetailInfoForMobileApps_SP] @ReservationId, @RoomTypeId, @RoomQuantity, @PaxQuantity, @ChildQuantity, @ExtraBedQuantity, @GuestNotes", reservationId, roomTypeId, roomQuantity, paxQuantity, childQuantity, extraBedQuantity, guestNotes);
                }

                return true;

            }
            else
            {
                return false;
            }

        }

        public bool SaveRoomReservationPaymentInfoForMobileApps(HotelRoomReservationMobileAppsBO roomReservationPaymentInfo)
        {
            SqlParameter reservationId = new SqlParameter("@ReservationId", roomReservationPaymentInfo.ReservationId);
            SqlParameter transactionType = new SqlParameter("@TransactionType", roomReservationPaymentInfo.TransactionType);
            SqlParameter transactionId = new SqlParameter("@TransactionId", roomReservationPaymentInfo.TransactionId);
            SqlParameter transactionAmount = new SqlParameter("@TransactionAmount", roomReservationPaymentInfo.TransactionAmount);
            SqlParameter transactionDetails = new SqlParameter("@TransactionDetails", roomReservationPaymentInfo.TransactionDetails);
            SqlParameter createdBy = new SqlParameter("@CreatedBy", roomReservationPaymentInfo.CreatedBy);

            int result = InnboardDBContext.Database.ExecuteSqlCommand("EXEC [dbo].[SaveRoomReservationPaymentInfoForMobileApps_SP] @ReservationId, @TransactionType, @TransactionId, @TransactionAmount, @TransactionDetails, @CreatedBy", reservationId, transactionType, transactionId, transactionAmount, transactionDetails, createdBy);

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
