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
    public class RestaurantBillDataAccess : GenericDataAccess<RestaurantBill>, IRestaurantBill
    {
        public RestaurantBillDataAccess()
        {
        }

        public new RestaurantBill Save(RestaurantBill restaurant)
        {
            string query = string.Format(@"SELECT dbo.FnRestaurantBillNumber('{0}') as billNumber", restaurant.CostCenterId);

            string billNumber = InnboardDBContext.Database.SqlQuery<string>(query).FirstOrDefault();
            
            restaurant.BillNumber = billNumber;

            //context.Entry(roomRegistration).Property(x => x.RegistrationNumber).IsModified = true; 
            return base.Save(restaurant);

        }

        public RestaurantBill GetByGuiId(Guid? Id)
        {
            RestaurantBill roomRegistration = new RestaurantBill();

            return InnboardDBSet.Where(r => r.GuidId == Id).FirstOrDefault();
            //return InnboardDBSet.Where(r=>prop.GetValue(r).Equals(Id)).FirstOrDefault();
        }

        public RestaurantDataSync Sync(RestaurantDataSync restaurant)
        {
            using (var transaction = InnboardDBContext.Database.BeginTransaction())
            {
                string query = string.Format(@"SELECT dbo.FnRestaurantBillNumber({0}) as BillNumber",restaurant.Bill.CostCenterId);
                string billNumber = InnboardDBContext.Database.SqlQuery<string>(query).FirstOrDefault();

                if (restaurant.Bill.RegistrationId > 0)
                {
                    HotelRoomRegistrationDataAccess roomDataAccess = new HotelRoomRegistrationDataAccess();
                    var response = roomDataAccess.GetByGuiId(restaurant.Bill.RegistrationGuidId);

                    restaurant.Bill.RegistrationId = response != null ? response.RegistrationId : 0;
                }
                restaurant.Bill.ReferenceBillNumber = restaurant.Bill.BillNumber;
                restaurant.Bill.BillNumber = billNumber;
                Save(restaurant.Bill);
                SaveChanges();

                List<RestaurantKotBillDetail> restaurantKotBillDetails = new List<RestaurantKotBillDetail>();
                RestaurantKotBillDetail restaurantKotBillDetail = new RestaurantKotBillDetail();

                List<RestaurantKotSpecialRemarksDetail> remarksDetails = new List<RestaurantKotSpecialRemarksDetail>();
                RestaurantKotSpecialRemarksDetail remarksDetail = new RestaurantKotSpecialRemarksDetail();

                foreach (var kotBillMaster in restaurant.KotBillMasters)
                {
                    restaurantKotBillDetails = restaurant.KotBillDetails.Where(r => r.KotId == kotBillMaster.KotId).ToList();
                    remarksDetails = restaurant.KotSpecialRemarksDetails.Where(r => r.KotId == kotBillMaster.KotId).ToList();

                    InnboardDBContext.RestaurantKotBillMaster.Add(kotBillMaster);
                    SaveChanges();

                    InnboardDBContext.RestaurantBillDetail.Add(new RestaurantBillDetail() { BillId = restaurant.Bill.BillId, KotId = kotBillMaster.KotId });
                    SaveChanges();
                    restaurantKotBillDetails.Select(i =>
                    {
                        i.KotId = kotBillMaster.KotId;
                        return i;
                    }).ToList();
                    InnboardDBContext.RestaurantKotBillDetail.AddRange(restaurantKotBillDetails);
                    SaveChanges();
                    foreach (var remark in remarksDetails)
                    {
                        remark.KotId = kotBillMaster.KotId;
                        InnboardDBContext.RestaurantKotSpecialRemarksDetail.Add(remark);
                        SaveChanges();
                    }

                }

                restaurant.BillClassificationDiscounts.Select(i => { i.BillId = restaurant.Bill.BillId; return i; }).ToList();
                InnboardDBContext.RestaurantBillClassificationDiscount.AddRange(restaurant.BillClassificationDiscounts);

                HotelCompanyPaymentLedger ledger = new HotelCompanyPaymentLedger();

                foreach (var item in restaurant.GuestBillPayments)
                {
                    HotelGuestBillPaymentDataAccess hotelGuestBillDA = new HotelGuestBillPaymentDataAccess();

                    ledger = restaurant.CompanyPayments.Where(l => l.PaymentId == item.PaymentId).FirstOrDefault();

                    item.ServiceBillId = restaurant.Bill.BillId;

                    if (item.RegistrationId > 0)
                    {
                        item.RegistrationId = restaurant.Bill.RegistrationId;
                    }
                    //first save guest bill
                    hotelGuestBillDA.Save(item);

                    //then map with company payment if payment mode is company  

                    if (ledger != null && item.PaymentMode == "Company")
                    {
                        HotelCompanyPaymentLedgerDataAccess ledgerDataAccess = new HotelCompanyPaymentLedgerDataAccess();

                        ledger.PaymentId = item.PaymentId;
                        ledger.BillId = restaurant.Bill.BillId;
                        ledger.BillNumber = restaurant.Bill.BillNumber;
                        ledgerDataAccess.Save(ledger);
                    }
                }

                restaurant.GuestExtraServiceApprovedBills.Select(i =>
                {
                    i.ServiceBillId = restaurant.Bill.BillId;
                    i.RegistrationId = restaurant.Bill.RegistrationId;
                    return i;
                }).ToList();
                InnboardDBContext.HotelGuestExtraServiceBillApproved.AddRange(restaurant.GuestExtraServiceApprovedBills);
                restaurant.IsSyncCompleted = true;
                SaveChanges();
                transaction.Commit();
            }
            return restaurant;
        }
    }
}
