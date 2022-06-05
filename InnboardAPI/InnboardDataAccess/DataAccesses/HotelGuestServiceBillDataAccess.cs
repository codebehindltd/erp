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
    public class HotelGuestServiceBillDataAccess : GenericDataAccess<HotelGuestServiceBill>, IHotelGuestServiceBill
    {
        public new HotelGuestServiceBill Save (HotelGuestServiceBill hotelGuestServiceBill)
        {
            string query = string.Format(@"SELECT dbo.FnCommonBillNumber('ServiceBill') as BillNumber");

            string billNumber = InnboardDBContext.Database.SqlQuery<string>(query).FirstOrDefault();

            hotelGuestServiceBill.BillNumber = billNumber;

            return base.Save(hotelGuestServiceBill);
        }

        public ServiceBillDataSync Sync(ServiceBillDataSync hotelGuestServiceBill)
        {
            using (var transaction = InnboardDBContext.Database.BeginTransaction())
            {
                string query = string.Format(@"SELECT dbo.FnCommonBillNumber('ServiceBill') as BillNumber");
                string billNumber = InnboardDBContext.Database.SqlQuery<string>(query).FirstOrDefault();

                if (hotelGuestServiceBill.ServiceBill.RegistrationId > 0)
                {
                    HotelRoomRegistrationDataAccess roomDataAccess = new HotelRoomRegistrationDataAccess();

                    hotelGuestServiceBill.ServiceBill.RegistrationId = roomDataAccess.GetByGuiId(hotelGuestServiceBill.ServiceBill.RegistrationGuidId).RegistrationId;
                }
                hotelGuestServiceBill.ServiceBill.BillNumber = billNumber;
                Save(hotelGuestServiceBill.ServiceBill);
                SaveChanges();

                HotelCompanyPaymentLedger ledger = new HotelCompanyPaymentLedger();

                foreach (var item in hotelGuestServiceBill.GuestBillPayments)
                {
                    HotelGuestBillPaymentDataAccess hotelGuestBillDA = new HotelGuestBillPaymentDataAccess();

                    ledger = hotelGuestServiceBill.CompanyPayments.Where(l => l.PaymentId == item.PaymentId).FirstOrDefault();

                    item.ServiceBillId = hotelGuestServiceBill.ServiceBill.ServiceBillId;

                    if (item.RegistrationId > 0)
                    {
                        item.RegistrationId = hotelGuestServiceBill.ServiceBill.RegistrationId;
                    }
                    //first save guest bill
                    hotelGuestBillDA.Save(item);

                    //then map with company payment if payment mode is company  

                    if (ledger != null && item.PaymentMode == "Company")
                    {
                        HotelCompanyPaymentLedgerDataAccess ledgerDataAccess = new HotelCompanyPaymentLedgerDataAccess();

                        ledger.PaymentId = item.PaymentId;
                        ledger.BillId = hotelGuestServiceBill.ServiceBill.ServiceBillId;
                        ledger.BillNumber = hotelGuestServiceBill.ServiceBill.BillNumber;
                        ledgerDataAccess.Save(ledger);
                    }
                }
                SaveChanges();
                transaction.Commit();
            }

            return hotelGuestServiceBill;
        }

        public HotelGuestServiceBill GetByGuiId(Guid? Id)
        {
            HotelGuestServiceBill roomRegistration = new HotelGuestServiceBill();

            return InnboardDBSet.Where(r => r.GuidId == Id).FirstOrDefault();
            //return InnboardDBSet.Where(r=>prop.GetValue(r).Equals(Id)).FirstOrDefault();
        }
    }
}
