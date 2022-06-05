using HotelManagement.Entity.HotelManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.HotelManagement
{
    public class HotelGuestBillApprovedDA : BaseService
    {
        public List<HotelGuestBillApproved> GetHotelGuestApprovedBillByRegistrationId(int registrationId)
        {
            List<HotelGuestBillApproved> approvedBillList = new List<HotelGuestBillApproved>();


            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHotelGuestApprovedBillByRegistrationId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "HotelGuestApproved");
                    DataTable Table = ds.Tables["HotelGuestApproved"];

                    approvedBillList = Table.AsEnumerable().Select(r => new HotelGuestBillApproved
                    {

                        ApprovedId = r.Field<long>("ApprovedId"),
                        RegistrationId = r.Field<long?>("RegistrationId"),
                        ServiceDate = r.Field<DateTime?>("ServiceDate"),
                        ApprovedDate = r.Field<DateTime?>("ApprovedDate"),
                        RoomType = r.Field<string>("RoomType"),
                        RoomId = r.Field<int?>("RoomId"),
                        RoomNumber = r.Field<string>("RoomNumber"),
                        ServiceName = r.Field<string>("ServiceName"),
                        TotalRoomCharge = r.Field<decimal?>("TotalRoomCharge"),
                        UnitPrice = r.Field<decimal?>("UnitPrice"),
                        BPPercentAmount = r.Field<decimal?>("BPPercentAmount"),
                        DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                        RoomRate = r.Field<decimal?>("RoomRate"),
                        VatAmount = r.Field<decimal?>("VatAmount"),
                        ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                        CitySDCharge = r.Field<decimal?>("CitySDCharge"),
                        AdditionalCharge = r.Field<decimal?>("AdditionalCharge"),
                        InvoiceRackRate = r.Field<decimal?>("InvoiceRackRate"),
                        IsServiceChargeEnable = r.Field<bool?>("IsServiceChargeEnable"),
                        InvoiceServiceCharge = r.Field<decimal?>("InvoiceServiceCharge"),
                        IsCitySDChargeEnable = r.Field<bool?>("IsCitySDChargeEnable"),
                        InvoiceCitySDCharge = r.Field<decimal?>("InvoiceCitySDCharge"),
                        IsVatAmountEnable = r.Field<bool?>("IsVatAmountEnable"),
                        InvoiceVatAmount = r.Field<decimal?>("InvoiceVatAmount"),
                        IsAdditionalChargeEnable = r.Field<bool?>("IsAdditionalChargeEnable"),
                        InvoiceAdditionalCharge = r.Field<decimal?>("InvoiceAdditionalCharge"),
                        CurrencyExchangeRate = r.Field<decimal?>("CurrencyExchangeRate"),
                        ReferenceSalesCommission = r.Field<decimal?>("ReferenceSalesCommission"),
                        IsBillHoldUp = r.Field<bool?>("IsBillHoldUp"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        TotalCalculatedAmount = r.Field<decimal?>("TotalCalculatedAmount"),
                        CreatedBy = r.Field<int?>("CreatedBy"),
                        CreatedDate = r.Field<DateTime?>("CreatedDate"),
                        LastModifiedBy = r.Field<int?>("LastModifiedBy"),
                        LastModifiedDate = r.Field<DateTime?>("LastModifiedDate"),
                        IsExtraBedCharge = r.Field<bool>("IsExtraBedCharge")


                    }).ToList();
                }
            }
            return approvedBillList;
        }
    }
}
