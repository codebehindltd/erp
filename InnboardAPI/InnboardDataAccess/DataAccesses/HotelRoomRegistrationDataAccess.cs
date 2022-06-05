
using InnboardDataAccess.DataAccesses;
using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using InnboardDomain.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace InnboardAPI.DataAccesses
{
    public class HotelRoomRegistrationDataAccess : GenericDataAccess<HotelRoomRegistration>, IHotelRoomRegistration
    {

        public HotelRoomRegistrationDataAccess()
        {

        }

        public new HotelRoomRegistration Save(HotelRoomRegistration roomRegistration)
        {
            string query = string.Format(@"SELECT dbo.FnRegistrationNumber() as RegNumber");

            string registrationNumber = InnboardDBContext.Database.SqlQuery<string>(query).FirstOrDefault();

            roomRegistration.RegistrationNumber = registrationNumber;
            roomRegistration.CheckOutDate = roomRegistration.CheckOutDateForAPI;
            return base.Save(roomRegistration);
        }

        public HotelRoomRegistration GetByGuiId(Guid? Id)
        {
            HotelRoomRegistration roomRegistration = new HotelRoomRegistration();

            return InnboardDBSet.Where(r => r.GuidId == Id).FirstOrDefault();
            //return InnboardDBSet.Where(r=>prop.GetValue(r).Equals(Id)).FirstOrDefault();
        }


        public int UpdateRegistration(HotelRoomRegistration roomRegistration)
        {
            InnboardDBContext.Entry(roomRegistration).State = EntityState.Modified;
            InnboardDBContext.Entry(roomRegistration).Property(p => p.RegistrationNumber).IsModified = true;
            return base.SaveChanges();

        }

        public RegistrationDataSync SyncRoomRegistrationData(RegistrationDataSync room)
        {
            using (var transaction = InnboardDBContext.Database.BeginTransaction())
            {
                var response = GetByGuiId(room.GuidId);
                HotelRoomNumberDataAccess hotelRoomNumberDataAccess = new HotelRoomNumberDataAccess();
                //New Registration Entry
                if (response == null)
                {
                    string query = string.Format(@"SELECT dbo.FnRegistrationNumber() as RegNumber");
                    string registrationNumber = InnboardDBContext.Database.SqlQuery<string>(query).FirstOrDefault();

                    room.RoomRegistration.RegistrationNumber = registrationNumber;
                    room.RoomRegistration.CheckOutDate = room.RoomRegistration.CheckOutDateForAPI;

                    InnboardDBContext.HotelRoomRegistration.Add(room.RoomRegistration);
                    SaveChanges();
                    HotelRoomNumber roomNumber = new HotelRoomNumber()
                    {
                        RoomId = (int)room.RoomRegistration.RoomId,
                        StatusId = 2,
                        HKRoomStatusId = 5
                    };
                    InnboardDBContext.HotelRoomNumber.Attach(roomNumber);
                    InnboardDBContext.Entry(roomNumber).Property(p => p.StatusId).IsModified = true;
                    InnboardDBContext.Entry(roomNumber).Property(p => p.HKRoomStatusId).IsModified = true;

                }
                else if (room.RoomRegistration != null)
                {
                    room.RoomRegistration.RegistrationId = response.RegistrationId;

                    //if room is changed
                    if (room.RoomRegistration.RoomId != response.RoomId)
                    {
                        HotelRoomNumber roomNumber = new HotelRoomNumber()
                        {
                            RoomId = (int)response.RoomId,
                            StatusId = 1,
                            HKRoomStatusId = 5,
                            CleanupStatus = "Not Cleaned"
                        };
                        InnboardDBContext.HotelRoomNumber.Attach(roomNumber);
                        InnboardDBContext.Entry(roomNumber).Property(p => p.StatusId).IsModified = true;
                        InnboardDBContext.Entry(roomNumber).Property(p => p.HKRoomStatusId).IsModified = true;
                        InnboardDBContext.Entry(roomNumber).Property(p => p.CleanupStatus).IsModified = true;

                        roomNumber = new HotelRoomNumber()
                        {
                            RoomId = (int)room.RoomRegistration.RoomId,
                            StatusId = 2,
                            HKRoomStatusId = 5
                        };
                        InnboardDBContext.HotelRoomNumber.Attach(roomNumber);
                        InnboardDBContext.Entry(roomNumber).Property(p => p.StatusId).IsModified = true;
                        InnboardDBContext.Entry(roomNumber).Property(p => p.HKRoomStatusId).IsModified = true;

                    }

                    var local = InnboardDBSet.Local.FirstOrDefault(r => r.RegistrationId == room.RoomRegistration.RegistrationId);
                    if (local != null)
                        InnboardDBContext.Entry(local).State = EntityState.Detached;

                    UpdateRegistration(room.RoomRegistration);
                    SaveChanges();
                }
                else
                {
                    room.RoomRegistration = response;
                }
                foreach (var guest in room.Guests)
                {
                    HotelGuestRegistration guestRegistration = room.GuestRegistrationMappings.Where(r => r.GuestId == guest.GuestId).FirstOrDefault();
                    //first save guest information
                    InnboardDBContext.HotelGuestInformation.Add(guest);
                    SaveChanges();

                    guestRegistration.RegistrationId = room.RoomRegistration.RegistrationId;
                    guestRegistration.GuestId = guest.GuestId;

                    //then map guest with registration
                    InnboardDBContext.HotelGuestRegistration.Add(guestRegistration);
                    SaveChanges();
                }

                HotelCompanyPaymentLedger ledger = new HotelCompanyPaymentLedger();

                if (room.GuestBillPayments.Count > 0)
                {
                    HotelGuestBillPaymentDataAccess hotelGuestBillDA = new HotelGuestBillPaymentDataAccess();

                    room.GuestBillPayments.Select(b => { b.RegistrationId = room.RoomRegistration.RegistrationId; return b; }).ToList();

                    foreach (var guestBill in room.GuestBillPayments)
                    {
                        //first save guest bill
                        hotelGuestBillDA.Save(guestBill);

                        ledger = room.CompanyPayments.Where(l => l.PaymentId == guestBill.PaymentId)
                                                    .FirstOrDefault();

                        //then map with company payment if payment mode is company  
                        if (ledger != null && guestBill.PaymentMode == "Company")
                        {
                            HotelCompanyPaymentLedgerDataAccess ledgerDataAccess = new HotelCompanyPaymentLedgerDataAccess();

                            ledger.PaymentId = guestBill.PaymentId;
                            ledger.BillId = guestBill.PaymentId;
                            ledger.BillNumber = room.RoomRegistration.RegistrationNumber;
                            ledgerDataAccess.Save(ledger);
                        }
                    }
                }

                if (room.ApprovedHotelGuestBills.Count > 0)
                {
                    room.ApprovedHotelGuestBills.Select(b => { b.RegistrationId = room.RoomRegistration.RegistrationId; return b; }).ToList();
                    InnboardDBContext.HotelGuestBillApproved.AddRange(room.ApprovedHotelGuestBills);
                }

                if (room.GuestServiceBills.Count > 0)
                {
                    room.GuestServiceBills.Select(b => { b.RegistrationId = room.RoomRegistration.RegistrationId; return b; }).ToList();
                    InnboardDBContext.HotelGuestServiceBill.AddRange(room.GuestServiceBills);
                }

                if (room.AirportPickupDrops.Count > 0)
                {
                    //Convert DateTime property to TimeSpan
                    List<HotelRegistrationAireportPickupDrop> pickupDrops = new List<HotelRegistrationAireportPickupDrop>();
                    pickupDrops = (from a in room.AirportPickupDrops
                                   select new HotelRegistrationAireportPickupDrop
                                   {
                                       APDId = a.APDId,
                                       RegistrationId = room.RoomRegistration.RegistrationId,
                                       ArrivalFlightName = a.ArrivalFlightName,
                                       ArrivalFlightNumber = a.ArrivalFlightNumber,
                                       ArrivalTime = Convert.ToDateTime(a.ArrivalTime).TimeOfDay,
                                       DepartureAirlineId = a.DepartureAirlineId,
                                       DepartureFlightName = a.DepartureFlightName,
                                       DepartureFlightNumber = a.DepartureFlightNumber,
                                       DepartureTime = Convert.ToDateTime(a.DepartureTime).TimeOfDay

                                   }).ToList();


                    InnboardDBContext.HotelRegistrationAireportPickupDrop.AddRange(pickupDrops);
                }
                HotelGuestHouseCheckOutDataAccess checkOutDataAccess = new HotelGuestHouseCheckOutDataAccess();

                if (room.GuestHouseCheckOut != null)
                {
                 
                    room.GuestHouseCheckOut.RegistrationId = room.RoomRegistration.RegistrationId;
                    if (room.GuestHouseCheckOut.BillPaidByGuidId == room.RoomRegistration.GuidId)
                        room.GuestHouseCheckOut.BillPaidBy = room.RoomRegistration.RegistrationId;
                    else
                    {
                        //if The room who paid is already Synced.
                        var billPaidByRoom = GetByGuiId(room.GuestHouseCheckOut.BillPaidByGuidId);
                        if (billPaidByRoom != null)
                            room.GuestHouseCheckOut.BillPaidBy = billPaidByRoom.RegistrationId;
                    }
                    checkOutDataAccess.Save(room.GuestHouseCheckOut);
                    
                    room.IsSyncCompleted = true;
                    //check HotelRoomNumber is already attached .if exist then dettach
                    var local = InnboardDBContext.HotelRoomNumber.Local.FirstOrDefault(r => r.RoomId == room.RoomRegistration.RoomId);

                    if (local != null)
                        InnboardDBContext.Entry(local).State = EntityState.Detached;

                    HotelRoomNumber roomNumber = new HotelRoomNumber()
                    {
                        RoomId = (int)room.RoomRegistration.RoomId,
                        StatusId = 1,
                        HKRoomStatusId = 2,
                        CleanupStatus = "Not Cleaned"
                    };
                    InnboardDBContext.HotelRoomNumber.Attach(roomNumber);
                    InnboardDBContext.Entry(roomNumber).Property(p => p.StatusId).IsModified = true;
                    InnboardDBContext.Entry(roomNumber).Property(p => p.HKRoomStatusId).IsModified = true;
                    InnboardDBContext.Entry(roomNumber).Property(p => p.CleanupStatus).IsModified = true;

                    SaveChanges();
                }
                else
                    room.IsSyncCompleted = false;

                //if The room who paid is Synced after bill trasferred Rooms synced.Update BillPaidBy
                foreach (var guidId in room.BillPaidForGuidId)
                {
                    var billPaidForRoom = GetByGuiId(guidId);
                    if (billPaidForRoom != null)
                    {
                        HotelGuestHouseCheckOut checkOut = checkOutDataAccess.GetAll().AsQueryable().Where(r => r.RegistrationId == billPaidForRoom.RegistrationId).FirstOrDefault();
                        if (checkOut != null)
                        {
                            checkOut.BillPaidBy = room.RoomRegistration.RegistrationId;
                            checkOutDataAccess.Update(checkOut);
                        }
                    }
                }

                if (room.HotelGuestDayLateCheckOut != null)
                {
                    room.HotelGuestDayLateCheckOut.RegistrationId = room.RoomRegistration.RegistrationId;
                    InnboardDBContext.HotelGuestDayLetCheckOut.Add(room.HotelGuestDayLateCheckOut);
                }

                SaveChanges();
                transaction.Commit();
            }

            return room;
        }
    }
}