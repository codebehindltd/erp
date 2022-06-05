using InnboardDomain;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardAPI
{
    public partial class InnboardDbContext : DbContext
    {
        public InnboardDbContext() : base(Cryptography.Decrypt(System.Configuration.ConfigurationManager.ConnectionStrings["InnboardConnectionString"].ConnectionString))
        {

        }

        public virtual DbSet<BanquetReservationClassificationDiscount> BanquetReservationClassificationDiscount { get; set; }
        public virtual DbSet<BanquetReservation> BanquetReservation { get; set; }
        public virtual DbSet<BanquetReservationDetail> BanquetReservationDetail { get; set; }
        public virtual DbSet<CommonCostCenter> CommonCostCenter { get; set; }
        public virtual DbSet<GLLedgerMaster> GLLedgerMaster { get; set; }
        public virtual DbSet<GLLedgerDetails> GLLedgerDetails { get; set; }
        public virtual DbSet<GLVoucherApprovedInfo> GLVoucherApprovedInfo { get; set; }
        public virtual DbSet<HotelCompanyPaymentLedger> HotelCompanyPaymentLedger { get; set; }
        public virtual DbSet<HotelGuestBillApproved> HotelGuestBillApproved { get; set; }
        public virtual DbSet<HotelGuestBillPayment> HotelGuestBillPayment { get; set; }
        public virtual DbSet<HotelGuestCompany> HotelGuestCompany { get; set; }
        public virtual DbSet<HotelGuestDayLetCheckOut> HotelGuestDayLetCheckOut { get; set; }
        public virtual DbSet<HotelGuestExtraServiceBillApproved> HotelGuestExtraServiceBillApproved { get; set; }
        public virtual DbSet<HotelGuestHouseCheckOut> HotelGuestHouseCheckOut { get; set; }
        public virtual DbSet<HotelGuestInformation> HotelGuestInformation { get; set; }
        public virtual DbSet<HotelGuestInformationOnline> HotelGuestInformationOnline { get; set; }
        public virtual DbSet<HotelGuestRegistration> HotelGuestRegistration { get; set; }
        public virtual DbSet<HotelGuestServiceBill> HotelGuestServiceBill { get; set; }
        public virtual DbSet<HotelOnlineRoomReservation> HotelOnlineRoomReservation { get; set; }
        public virtual DbSet<HotelRoomReservationOnline> HotelRoomReservationOnline { get; set; }
        public virtual DbSet<HotelRoomReservationDetailOnline> HotelRoomReservationDetailOnline { get; set; }
        public virtual DbSet<HotelRegistrationAireportPickupDrop> HotelRegistrationAireportPickupDrop { get; set; }
        public virtual DbSet<HotelRoomNumber> HotelRoomNumber { get; set; }
        public virtual DbSet<HotelRoomRegistration> HotelRoomRegistration { get; set; }
        public virtual DbSet<HotelRoomType> HotelRoomType { get; set; }
        public virtual DbSet<InvCategoryCostCenterMapping> InvCategoryCostCenterMapping { get; set; }
        public virtual DbSet<InvCategory> InvCategory { get; set; }
        public virtual DbSet<InvCogsAccountVsItemCategoryMappping> InvCogsAccountVsItemCategoryMappping { get; set; }
        public virtual DbSet<InvInventoryAccountVsItemCategoryMappping> InvInventoryAccountVsItemCategoryMappping { get; set; }
        public virtual DbSet<InvItemClassificationCostCenterMapping> InvItemClassificationCostCenterMapping { get; set; }
        public virtual DbSet<InvItemClassification> InvItemClassification { get; set; }
        public virtual DbSet<InvItemCostCenterMapping> InvItemCostCenterMapping { get; set; }
        public virtual DbSet<InvItem> InvItem { get; set; }
        public virtual DbSet<InvLocation> InvLocation { get; set; }
        public virtual DbSet<InvLocationCostCenterMapping> InvLocationCostCenterMapping { get; set; }
        public virtual DbSet<InvUnitConversion> InvUnitConversion { get; set; }
        public virtual DbSet<InvUnitHead> InvUnitHead { get; set; }
        public virtual DbSet<RestaurantBillClassificationDiscount> RestaurantBillClassificationDiscount { get; set; }
        public virtual DbSet<RestaurantBill> RestaurantBill { get; set; }
        public virtual DbSet<RestaurantBillDetail> RestaurantBillDetail { get; set; }
        public virtual DbSet<RestaurantKotBillDetail> RestaurantKotBillDetail { get; set; }
        public virtual DbSet<RestaurantKotBillMaster> RestaurantKotBillMaster { get; set; }
        public virtual DbSet<RestaurantKotSpecialRemarksDetail> RestaurantKotSpecialRemarksDetail { get; set; }
        public virtual DbSet<RestaurantRecipeDetail> RestaurantRecipeDetail { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BanquetInformation>()
                .HasMany(e => e.BanquetReservation)
                .WithOptional(e => e.BanquetInformation)
                .HasForeignKey(e => e.BanquetId);

            modelBuilder.Entity<BanquetOccessionType>()
                .HasMany(e => e.BanquetReservation)
                .WithOptional(e => e.BanquetOccessionType)
                .HasForeignKey(e => e.OccessionTypeId);

            modelBuilder.Entity<BanquetRefference>()
                .HasMany(e => e.BanquetReservation)
                .WithOptional(e => e.BanquetRefference)
                .HasForeignKey(e => e.RefferenceId);

            modelBuilder.Entity<BanquetReservation>()
                .HasMany(e => e.BanquetBillPayment)
                .WithOptional(e => e.BanquetReservation)
                .HasForeignKey(e => e.ReservationId);

            modelBuilder.Entity<BanquetReservation>()
                .HasMany(e => e.BanquetReservationBillPayment)
                .WithOptional(e => e.BanquetReservation)
                .HasForeignKey(e => e.ReservationId);

            modelBuilder.Entity<BanquetReservation>()
               .HasMany(e => e.BanquetReservationClassificationDiscount)
               .WithOptional(e => e.BanquetReservation)
               .HasForeignKey(e => e.ReservationId);

            modelBuilder.Entity<BanquetReservation>()
               .HasMany(e => e.BanquetReservationDetail)
               .WithOptional(e => e.BanquetReservation)
               .HasForeignKey(e => e.ReservationId);

            modelBuilder.Entity<BanquetSeatingPlan>()
               .HasMany(e => e.BanquetReservation)
               .WithOptional(e => e.BanquetSeatingPlan)
               .HasForeignKey(e => e.SeatingId);
            

        }


    }
}
