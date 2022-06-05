using InnboardAPI.DataAccesses;
using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using InnboardDomain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDataAccess.DataAccesses
{
    public class BanquetReservationDataAccess : GenericDataAccess<BanquetReservation>, IBanquetReservation
    {
        public BanquetReservationDataAccess() 
        {
        }

        

        public new BanquetReservation Save(BanquetReservation banquetReservation)
        {
            string query = string.Format(@"SELECT dbo.FnBanquetReservationNumber() as ReservationNumber");

            string reservationNumber = InnboardDBContext.Database.SqlQuery<string>(query).FirstOrDefault();

            banquetReservation.ReservationNumber = reservationNumber;

            return base.Save(banquetReservation);
        }
        public BanquetBillDataSync Sync(BanquetBillDataSync banquet)
        {
            using (var transaction = InnboardDBContext.Database.BeginTransaction())
            {
                string query = string.Format(@"SELECT dbo.FnBanquetReservationNumber() as ReservationNumber");
                string reservationNumber = InnboardDBContext.Database.SqlQuery<string>(query).FirstOrDefault();

                banquet.BanquetReservation.ReservationNumber = reservationNumber;

                base.Save(banquet.BanquetReservation);

                HotelCompanyPaymentLedger ledger = new HotelCompanyPaymentLedger();

                foreach (var bill in banquet.GuestBillPayments)
                {
                    HotelGuestBillPaymentDataAccess hotelGuestBillDA = new HotelGuestBillPaymentDataAccess();
                   
                    ledger = banquet.CompanyPayments.Where(l => l.PaymentId == bill.PaymentId).FirstOrDefault();

                    bill.ServiceBillId = (int)banquet.BanquetReservation.Id;
                    
                    if (bill.RegistrationId > 0)
                    {
                        HotelRoomRegistrationDataAccess roomDataAccess = new HotelRoomRegistrationDataAccess();

                        bill.RegistrationId = roomDataAccess.GetByGuiId(bill.RegistrationGuiId).RegistrationId;
                        //registrationId = banquetReservation.RegistrationId;
                    }
                    //first save guest bill
                    hotelGuestBillDA.Save(bill);

                    //then map with company payment if payment mode is company  
                    if (ledger != null && bill.PaymentMode == "Company")
                    {
                        HotelCompanyPaymentLedgerDataAccess ledgerDataAccess = new HotelCompanyPaymentLedgerDataAccess();

                        ledger.PaymentId = bill.PaymentId;
                        ledger.BillId = (int?)banquet.BanquetReservation.Id;
                        ledger.BillNumber = banquet.BanquetReservation.ReservationNumber;
                        ledgerDataAccess.Save(ledger);
                    }
                }
                SaveChanges();
                transaction.Commit();
                banquet.IsSyncCompleted = true;
            }            

            return banquet;
        }
    }
}
