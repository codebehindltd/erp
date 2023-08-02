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
        public bool SaveRoomReservationInfoForMobileApps(HotelRoomReservationMobileAppsBO roomReservationInfo, out long tmpReservationId)
        {
            SqlParameter transactionType = new SqlParameter("@TransactionType", roomReservationInfo.TransactionType);
            SqlParameter fromDate = new SqlParameter("@FromDate", roomReservationInfo.FromDate);
            SqlParameter toDate = new SqlParameter("@ToDate", roomReservationInfo.ToDate);
            SqlParameter roomTypeId = new SqlParameter("@RoomTypeId", roomReservationInfo.RoomTypeId);
            SqlParameter paxQuantity = new SqlParameter("@PaxQuantity", roomReservationInfo.PaxQuantity);
            SqlParameter childQuantity = new SqlParameter("@ChildQuantity", roomReservationInfo.ChildQuantity);
            SqlParameter extraBedQuantity = new SqlParameter("@ExtraBedQuantity", roomReservationInfo.ExtraBedQuantity);
            SqlParameter guestName = new SqlParameter("@GuestName", roomReservationInfo.GuestName);
            SqlParameter phoneNumber = new SqlParameter("@PhoneNumber", roomReservationInfo.PhoneNumber);
            SqlParameter guestNotes = new SqlParameter("@GuestNotes", roomReservationInfo.GuestNotes);

            SqlParameter pOutReservationId = new SqlParameter("@ReservationId", SqlDbType.Int);
            pOutReservationId.Direction = ParameterDirection.Output;

            int result = InnboardDBContext.Database.ExecuteSqlCommand("EXEC [dbo].[SaveRoomReservationInfoForMobileApps_SP] @TransactionType, @FromDate, @ToDate, @RoomTypeId, @PaxQuantity, @ChildQuantity, @ExtraBedQuantity, @GuestName, @PhoneNumber, @GuestNotes, @ReservationId OUT", transactionType, fromDate, toDate, roomTypeId, paxQuantity, childQuantity, extraBedQuantity, guestName, phoneNumber, guestNotes, pOutReservationId);

            tmpReservationId = (long)pOutReservationId.Value;

            if (result > 0)
            {
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
