using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Banquet;
using System.Data.Common;
using System.Data;
using System.Collections;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Data.Banquet
{
    public class BanquetReservationDA : BaseService
    {
        public bool SaveBanquetReservationInfo(BanquetReservationBO reservationBO, List<BanquetReservationDetailBO> addList, List<BanquetReservationClassificationDiscountBO> discountLst, out long tmpReservationId)
        {
            bool retVal = false;
            int status = 0;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        try
                        {
                            using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveBanquetReservationInfo_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandMaster, "@IsListedCompany", DbType.Boolean, reservationBO.IsListedCompany);
                                dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, reservationBO.CompanyId);
                                dbSmartAspects.AddInParameter(commandMaster, "@Name", DbType.String, reservationBO.Name);
                                dbSmartAspects.AddInParameter(commandMaster, "@BanquetId", DbType.Int64, reservationBO.BanquetId);
                                dbSmartAspects.AddInParameter(commandMaster, "@CostCenterId", DbType.Int32, reservationBO.CostCenterId);
                                dbSmartAspects.AddInParameter(commandMaster, "@ReservationMode", DbType.String, reservationBO.ReservationMode);
                                dbSmartAspects.AddInParameter(commandMaster, "@CityName", DbType.String, reservationBO.CityName);
                                dbSmartAspects.AddInParameter(commandMaster, "@ZipCode", DbType.String, reservationBO.ZipCode);
                                dbSmartAspects.AddInParameter(commandMaster, "@CountryId", DbType.Int32, reservationBO.CountryId);
                                dbSmartAspects.AddInParameter(commandMaster, "@EmailAddress", DbType.String, reservationBO.EmailAddress);
                                dbSmartAspects.AddInParameter(commandMaster, "@PhoneNumber", DbType.String, reservationBO.PhoneNumber);
                                dbSmartAspects.AddInParameter(commandMaster, "@BookingFor", DbType.String, reservationBO.BookingFor);
                                dbSmartAspects.AddInParameter(commandMaster, "@ContactPerson", DbType.String, reservationBO.ContactPerson);
                                dbSmartAspects.AddInParameter(commandMaster, "@ContactPhone", DbType.String, reservationBO.ContactPhone);
                                dbSmartAspects.AddInParameter(commandMaster, "@ContactEmail", DbType.String, reservationBO.ContactEmail);
                                dbSmartAspects.AddInParameter(commandMaster, "@Address", DbType.String, reservationBO.Address);
                                dbSmartAspects.AddInParameter(commandMaster, "@ArriveDate", DbType.DateTime, reservationBO.ArriveDate);
                                dbSmartAspects.AddInParameter(commandMaster, "@DepartureDate", DbType.DateTime, reservationBO.DepartureDate);
                                dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, reservationBO.Remarks);
                                dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonAdult", DbType.Decimal, reservationBO.NumberOfPersonAdult);
                                dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonChild", DbType.Decimal, reservationBO.NumberOfPersonChild);
                                dbSmartAspects.AddInParameter(commandMaster, "@OccessionTypeId", DbType.Int64, reservationBO.OccessionTypeId);
                                dbSmartAspects.AddInParameter(commandMaster, "@SeatingId", DbType.Int64, reservationBO.SeatingId);
                                dbSmartAspects.AddInParameter(commandMaster, "@RefferenceId", DbType.Int64, reservationBO.RefferenceId);
                                dbSmartAspects.AddInParameter(commandMaster, "@BanquetRate", DbType.Decimal, reservationBO.BanquetRate);

                                dbSmartAspects.AddInParameter(commandMaster, "@InvoiceServiceRate", DbType.Decimal, reservationBO.InvoiceServiceRate);
                                dbSmartAspects.AddInParameter(commandMaster, "@IsInvoiceServiceChargeEnable", DbType.Boolean, reservationBO.IsInvoiceServiceChargeEnable);
                                dbSmartAspects.AddInParameter(commandMaster, "@InvoiceServiceCharge", DbType.Decimal, reservationBO.InvoiceServiceCharge);
                                dbSmartAspects.AddInParameter(commandMaster, "@IsInvoiceCitySDChargeEnable", DbType.Boolean, reservationBO.IsInvoiceCitySDChargeEnable);
                                dbSmartAspects.AddInParameter(commandMaster, "@InvoiceCitySDCharge", DbType.Decimal, reservationBO.InvoiceCitySDCharge);
                                dbSmartAspects.AddInParameter(commandMaster, "@IsInvoiceVatAmountEnable", DbType.Boolean, reservationBO.IsInvoiceVatAmountEnable);
                                dbSmartAspects.AddInParameter(commandMaster, "@InvoiceVatAmount", DbType.Decimal, reservationBO.InvoiceVatAmount);
                                dbSmartAspects.AddInParameter(commandMaster, "@IsInvoiceAdditionalChargeEnable", DbType.Boolean, reservationBO.IsInvoiceAdditionalChargeEnable);
                                dbSmartAspects.AddInParameter(commandMaster, "@AdditionalChargeType", DbType.String, reservationBO.AdditionalChargeType);
                                dbSmartAspects.AddInParameter(commandMaster, "@InvoiceAdditionalCharge", DbType.Decimal, reservationBO.InvoiceAdditionalCharge);

                                dbSmartAspects.AddInParameter(commandMaster, "@TotalAmount", DbType.Decimal, reservationBO.TotalAmount);
                                dbSmartAspects.AddInParameter(commandMaster, "@DiscountType", DbType.String, reservationBO.DiscountType);
                                dbSmartAspects.AddInParameter(commandMaster, "@DiscountAmount", DbType.Decimal, reservationBO.DiscountAmount);
                                dbSmartAspects.AddInParameter(commandMaster, "@DiscountedAmount", DbType.Decimal, reservationBO.DiscountedAmount);
                                dbSmartAspects.AddInParameter(commandMaster, "@GrandTotal", DbType.Decimal, reservationBO.GrandTotal);

                                dbSmartAspects.AddInParameter(commandMaster, "@IsReturnedClient", DbType.Boolean, reservationBO.IsReturnedClient);

                                dbSmartAspects.AddInParameter(commandMaster, "@GLCompanyId", DbType.Int32, reservationBO.GLCompanyId);
                                dbSmartAspects.AddInParameter(commandMaster, "@GLProjectId", DbType.Int32, reservationBO.GLProjectId);
                                dbSmartAspects.AddInParameter(commandMaster, "@EventType", DbType.String, reservationBO.EventType);
                                dbSmartAspects.AddInParameter(commandMaster, "@EventTitle", DbType.String, reservationBO.EventTitle);

                                dbSmartAspects.AddInParameter(commandMaster, "@ActiveStatus", DbType.Int32, 1);
                                dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int64, reservationBO.CreatedBy);

                                dbSmartAspects.AddOutParameter(commandMaster, "@ReservationId", DbType.Int64, sizeof(Int64));
                                dbSmartAspects.AddInParameter(commandMaster, "@MeetingAgenda", DbType.String, reservationBO.MeetingAgenda);

                                status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                                tmpReservationId = Convert.ToInt64(commandMaster.Parameters["@ReservationId"].Value);
                            }

                            if (status > 0 && !string.IsNullOrEmpty(reservationBO.PerticipantFromOfficeCommaSeperatedIds))
                            {
                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveBanquetReservationPerticipantFromOffice_SP"))
                                {
                                    dbSmartAspects.AddInParameter(commandDetails, "@BanquetReservationId", DbType.Int32, tmpReservationId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@EmpIdsCommaSeperated", DbType.String, reservationBO.PerticipantFromOfficeCommaSeperatedIds);

                                    dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }

                            if (status > 0 && addList.Count > 0)
                            {
                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveBanquetReservationDetailInfo_SP"))
                                {
                                    foreach (BanquetReservationDetailBO banquetDetailBO in addList)
                                    {
                                        if (banquetDetailBO.ReservationId == 0)
                                        {
                                            commandDetails.Parameters.Clear();
                                            dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int64, tmpReservationId);
                                            dbSmartAspects.AddInParameter(commandDetails, "@ItemTypeId", DbType.Int64, banquetDetailBO.ItemTypeId);
                                            dbSmartAspects.AddInParameter(commandDetails, "@ItemType", DbType.String, banquetDetailBO.ItemType);
                                            dbSmartAspects.AddInParameter(commandDetails, "@ItemId", DbType.Int64, banquetDetailBO.ItemId);
                                            dbSmartAspects.AddInParameter(commandDetails, "@ItemName", DbType.String, banquetDetailBO.ItemName);
                                            dbSmartAspects.AddInParameter(commandDetails, "@ItemUnit", DbType.Decimal, banquetDetailBO.ItemUnit);
                                            dbSmartAspects.AddInParameter(commandDetails, "@ItemArrivalTime", DbType.DateTime, banquetDetailBO.ItemArrivalTime);
                                            dbSmartAspects.AddInParameter(commandDetails, "@ItemDescription", DbType.String, banquetDetailBO.ItemDescription);

                                            if (banquetDetailBO.TotalPrice == 0)
                                                banquetDetailBO.IsComplementary = true;

                                            dbSmartAspects.AddInParameter(commandDetails, "@IsComplementary", DbType.Boolean, banquetDetailBO.IsComplementary);

                                            dbSmartAspects.AddInParameter(commandDetails, "@UnitPrice", DbType.Decimal, banquetDetailBO.UnitPrice);
                                            dbSmartAspects.AddInParameter(commandDetails, "@TotalPrice", DbType.Decimal, banquetDetailBO.TotalPrice);

                                            status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                        }
                                    }
                                }
                            }

                            if (status > 0)
                            {
                                using (DbCommand commandDiscount = dbSmartAspects.GetStoredProcCommand("SaveBanquetReservationClassificationDiscount_SP"))
                                {
                                    foreach (BanquetReservationClassificationDiscountBO dis in discountLst)
                                    {
                                        commandDiscount.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDiscount, "@ReservationId", DbType.Int64, tmpReservationId);
                                        dbSmartAspects.AddInParameter(commandDiscount, "@CategoryId", DbType.String, dis.CategoryId);

                                        status = dbSmartAspects.ExecuteNonQuery(commandDiscount, transction);
                                    }
                                }
                            }

                            if (status > 0)
                            {
                                if (reservationBO.DiscountAmount > 0)
                                {
                                    using (DbCommand commandDiscount = dbSmartAspects.GetStoredProcCommand("BanquetServiceRateNVatNServiceChargeProcess_SP"))
                                    {
                                        commandDiscount.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDiscount, "@ReservationId", DbType.Int64, tmpReservationId);
                                        dbSmartAspects.AddInParameter(commandDiscount, "@CategoryIdList", DbType.Int32, DBNull.Value);
                                        dbSmartAspects.AddInParameter(commandDiscount, "@DiscountType", DbType.String, reservationBO.DiscountType);
                                        dbSmartAspects.AddInParameter(commandDiscount, "@DiscountAmount", DbType.String, reservationBO.DiscountAmount);
                                        dbSmartAspects.AddInParameter(commandDiscount, "@CostCenterId", DbType.String, reservationBO.CostCenterId);

                                        status = dbSmartAspects.ExecuteNonQuery(commandDiscount, transction);
                                    }
                                }
                            }

                            if (status > 0)
                            {
                                transction.Commit();
                                retVal = true;
                            }
                            else
                            {
                                transction.Dispose();
                                retVal = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            transction.Dispose();
                            retVal = false;
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retVal;
        }
        public bool UpdateBanquetReservationInfo(BanquetReservationBO reservationBO, List<BanquetReservationDetailBO> addList, List<BanquetReservationDetailBO> editList, List<BanquetReservationDetailBO> deleteList, ArrayList arrayDelete, List<BanquetReservationClassificationDiscountBO> discountLst, List<BanquetReservationClassificationDiscountBO> discountDeletedLst)
        {
            bool retVal = false;
            int status = 0;
            long tmpReservationId = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        try
                        {
                            using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateBanquetReservationInfo_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandMaster, "@ReservationId", DbType.Int64, reservationBO.Id);
                                dbSmartAspects.AddInParameter(commandMaster, "@IsListedCompany", DbType.Boolean, reservationBO.IsListedCompany);
                                dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, reservationBO.CompanyId);
                                dbSmartAspects.AddInParameter(commandMaster, "@Name", DbType.String, reservationBO.Name);
                                dbSmartAspects.AddInParameter(commandMaster, "@BanquetId", DbType.Int64, reservationBO.BanquetId);
                                dbSmartAspects.AddInParameter(commandMaster, "@CostCenterId", DbType.Int32, reservationBO.CostCenterId);
                                dbSmartAspects.AddInParameter(commandMaster, "@ReservationMode", DbType.String, reservationBO.ReservationMode);
                                dbSmartAspects.AddInParameter(commandMaster, "@CityName", DbType.String, reservationBO.CityName);
                                dbSmartAspects.AddInParameter(commandMaster, "@ZipCode", DbType.String, reservationBO.ZipCode);
                                dbSmartAspects.AddInParameter(commandMaster, "@CountryId", DbType.Int32, reservationBO.CountryId);
                                dbSmartAspects.AddInParameter(commandMaster, "@EmailAddress", DbType.String, reservationBO.EmailAddress);
                                dbSmartAspects.AddInParameter(commandMaster, "@PhoneNumber", DbType.String, reservationBO.PhoneNumber);
                                dbSmartAspects.AddInParameter(commandMaster, "@BookingFor", DbType.String, reservationBO.BookingFor);
                                dbSmartAspects.AddInParameter(commandMaster, "@ContactPerson", DbType.String, reservationBO.ContactPerson);
                                dbSmartAspects.AddInParameter(commandMaster, "@ContactPhone", DbType.String, reservationBO.ContactPhone);
                                dbSmartAspects.AddInParameter(commandMaster, "@ContactEmail", DbType.String, reservationBO.ContactEmail);
                                dbSmartAspects.AddInParameter(commandMaster, "@Address", DbType.String, reservationBO.Address);
                                dbSmartAspects.AddInParameter(commandMaster, "@DepartureDate", DbType.DateTime, reservationBO.DepartureDate);
                                dbSmartAspects.AddInParameter(commandMaster, "@ArriveDate", DbType.DateTime, reservationBO.ArriveDate);
                                dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, reservationBO.Remarks);
                                dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonAdult", DbType.Decimal, reservationBO.NumberOfPersonAdult);
                                dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonChild", DbType.Decimal, reservationBO.NumberOfPersonChild);
                                dbSmartAspects.AddInParameter(commandMaster, "@OccessionTypeId", DbType.Int64, reservationBO.OccessionTypeId);
                                dbSmartAspects.AddInParameter(commandMaster, "@SeatingId", DbType.Int64, reservationBO.SeatingId);
                                dbSmartAspects.AddInParameter(commandMaster, "@RefferenceId", DbType.Int64, reservationBO.RefferenceId);

                                dbSmartAspects.AddInParameter(commandMaster, "@InvoiceServiceRate", DbType.Decimal, reservationBO.InvoiceServiceRate);
                                dbSmartAspects.AddInParameter(commandMaster, "@IsInvoiceServiceChargeEnable", DbType.Boolean, reservationBO.IsInvoiceServiceChargeEnable);
                                dbSmartAspects.AddInParameter(commandMaster, "@InvoiceServiceCharge", DbType.Decimal, reservationBO.InvoiceServiceCharge);
                                dbSmartAspects.AddInParameter(commandMaster, "@IsInvoiceCitySDChargeEnable", DbType.Boolean, reservationBO.IsInvoiceCitySDChargeEnable);
                                dbSmartAspects.AddInParameter(commandMaster, "@InvoiceCitySDCharge", DbType.Decimal, reservationBO.InvoiceCitySDCharge);
                                dbSmartAspects.AddInParameter(commandMaster, "@IsInvoiceVatAmountEnable", DbType.Boolean, reservationBO.IsInvoiceVatAmountEnable);
                                dbSmartAspects.AddInParameter(commandMaster, "@InvoiceVatAmount", DbType.Decimal, reservationBO.InvoiceVatAmount);
                                dbSmartAspects.AddInParameter(commandMaster, "@IsInvoiceAdditionalChargeEnable", DbType.Boolean, reservationBO.IsInvoiceAdditionalChargeEnable);
                                dbSmartAspects.AddInParameter(commandMaster, "@AdditionalChargeType", DbType.String, reservationBO.AdditionalChargeType);
                                dbSmartAspects.AddInParameter(commandMaster, "@InvoiceAdditionalCharge", DbType.Decimal, reservationBO.InvoiceAdditionalCharge);

                                dbSmartAspects.AddInParameter(commandMaster, "@TotalAmount", DbType.Decimal, reservationBO.TotalAmount);
                                dbSmartAspects.AddInParameter(commandMaster, "@DiscountType", DbType.String, reservationBO.DiscountType);
                                dbSmartAspects.AddInParameter(commandMaster, "@DiscountAmount", DbType.Decimal, reservationBO.DiscountAmount);
                                dbSmartAspects.AddInParameter(commandMaster, "@DiscountedAmount", DbType.Decimal, reservationBO.DiscountedAmount);
                                dbSmartAspects.AddInParameter(commandMaster, "@GrandTotal", DbType.Decimal, reservationBO.GrandTotal);

                                dbSmartAspects.AddInParameter(commandMaster, "@BanquetRate", DbType.Decimal, reservationBO.BanquetRate);

                                //dbSmartAspects.AddInParameter(commandMaster, "@IsServiceChargeEnable", DbType.Boolean, reservationBO.IsInvoiceServiceChargeEnable);
                                //dbSmartAspects.AddInParameter(commandMaster, "@ServiceCharge", DbType.Decimal, reservationBO.InvoiceServiceCharge);
                                //dbSmartAspects.AddInParameter(commandMaster, "@IsVatAmountEnable", DbType.Boolean, reservationBO.IsInvoiceVatAmountEnable);
                                //dbSmartAspects.AddInParameter(commandMaster, "@VatAmount", DbType.Decimal, reservationBO.InvoiceVatAmount);
                                //dbSmartAspects.AddInParameter(commandMaster, "@TotalAmount", DbType.Decimal, reservationBO.TotalAmount);
                                //dbSmartAspects.AddInParameter(commandMaster, "@DiscountType", DbType.String, reservationBO.DiscountType);
                                //dbSmartAspects.AddInParameter(commandMaster, "@DiscountAmount", DbType.Decimal, reservationBO.DiscountAmount);
                                //dbSmartAspects.AddInParameter(commandMaster, "@DiscountedAmount", DbType.Decimal, reservationBO.DiscountedAmount);

                                dbSmartAspects.AddInParameter(commandMaster, "@IsReturnedClient", DbType.Boolean, reservationBO.IsReturnedClient);
                                dbSmartAspects.AddInParameter(commandMaster, "@GLCompanyId", DbType.Int32, reservationBO.GLCompanyId);
                                dbSmartAspects.AddInParameter(commandMaster, "@GLProjectId", DbType.Int32, reservationBO.GLProjectId);
                                dbSmartAspects.AddInParameter(commandMaster, "@EventType", DbType.String, reservationBO.EventType);
                                dbSmartAspects.AddInParameter(commandMaster, "@EventTitle", DbType.String, reservationBO.EventTitle);

                                dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int64, reservationBO.LastModifiedBy);
                                dbSmartAspects.AddInParameter(commandMaster, "@MeetingAgenda", DbType.String, reservationBO.MeetingAgenda);

                                status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                                tmpReservationId = reservationBO.Id;
                            }

                            if (status > 0 && !string.IsNullOrEmpty(reservationBO.PerticipantFromOfficeCommaSeperatedIds))
                            {
                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveBanquetReservationPerticipantFromOffice_SP"))
                                {
                                    dbSmartAspects.AddInParameter(commandDetails, "@BanquetReservationId", DbType.Int32, tmpReservationId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@EmpIdsCommaSeperated", DbType.String, reservationBO.PerticipantFromOfficeCommaSeperatedIds);

                                    dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }

                            if (status > 0)
                            {
                                if (addList.Count > 0)
                                {
                                    using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveBanquetReservationDetailInfo_SP"))
                                    {
                                        foreach (BanquetReservationDetailBO detailBO in addList)
                                        {
                                            if (detailBO.Id == 0)
                                            {
                                                commandDetails.Parameters.Clear();

                                                dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int64, tmpReservationId);
                                                dbSmartAspects.AddInParameter(commandDetails, "@ItemTypeId", DbType.Int64, detailBO.ItemTypeId);
                                                dbSmartAspects.AddInParameter(commandDetails, "@ItemType", DbType.String, detailBO.ItemType);
                                                dbSmartAspects.AddInParameter(commandDetails, "@ItemId", DbType.Int64, detailBO.ItemId);
                                                dbSmartAspects.AddInParameter(commandDetails, "@ItemName", DbType.String, detailBO.ItemName);
                                                dbSmartAspects.AddInParameter(commandDetails, "@ItemUnit", DbType.Decimal, detailBO.ItemUnit);
                                                dbSmartAspects.AddInParameter(commandDetails, "@ItemArrivalTime", DbType.DateTime, detailBO.ItemArrivalTime);
                                                dbSmartAspects.AddInParameter(commandDetails, "@ItemDescription", DbType.String, detailBO.ItemDescription);
                                                dbSmartAspects.AddInParameter(commandDetails, "@IsComplementary", DbType.Boolean, detailBO.IsComplementary);
                                                dbSmartAspects.AddInParameter(commandDetails, "@UnitPrice", DbType.Decimal, detailBO.UnitPrice);
                                                dbSmartAspects.AddInParameter(commandDetails, "@TotalPrice", DbType.Decimal, detailBO.TotalPrice);
                                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                            }
                                        }
                                    }
                                }
                            }

                            if (editList.Count >0 && status > 0)
                            {
                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateBanquetReservationDetailsInfo_SP"))
                                {
                                    foreach (BanquetReservationDetailBO detailBO in editList)
                                    {
                                        commandDetails.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDetails, "@Id", DbType.Int32, detailBO.Id);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemId", DbType.Int32, detailBO.ItemId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemName", DbType.String, detailBO.ItemName);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemTypeId", DbType.String, detailBO.ItemTypeId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemType", DbType.String, detailBO.ItemType);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemUnit", DbType.Decimal, detailBO.ItemUnit);
                                        dbSmartAspects.AddInParameter(commandDetails, "@IsComplementary", DbType.Boolean, detailBO.IsComplementary);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemArrivalTime", DbType.DateTime, detailBO.ItemArrivalTime);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ItemDescription", DbType.String, detailBO.ItemDescription);
                                        dbSmartAspects.AddInParameter(commandDetails, "@UnitPrice", DbType.Decimal, detailBO.UnitPrice);
                                        dbSmartAspects.AddInParameter(commandDetails, "@TotalPrice", DbType.Decimal, detailBO.TotalPrice);
                                        status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }

                            if (deleteList.Count > 0)
                            {
                                foreach (BanquetReservationDetailBO detailBO in deleteList)
                                {
                                    using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                    {
                                        dbSmartAspects.AddInParameter(commandEducation, "@TableName", DbType.String, "BanquetReservationDetail");
                                        dbSmartAspects.AddInParameter(commandEducation, "@TablePKField", DbType.String, "Id");
                                        dbSmartAspects.AddInParameter(commandEducation, "@TablePKId", DbType.String, detailBO.Id);
                                        status = dbSmartAspects.ExecuteNonQuery(commandEducation);
                                    }
                                }

                            }

                            if (status > 0 && discountLst.Count > 0)
                            {
                                using (DbCommand commandDiscount = dbSmartAspects.GetStoredProcCommand("SaveBanquetReservationClassificationDiscount_SP"))
                                {
                                    foreach (BanquetReservationClassificationDiscountBO dis in discountLst)
                                    {
                                        if (dis.Id == 0)
                                        {
                                            commandDiscount.Parameters.Clear();
                                            dbSmartAspects.AddInParameter(commandDiscount, "@ReservationId", DbType.Int32, tmpReservationId);
                                            dbSmartAspects.AddInParameter(commandDiscount, "@CategoryId", DbType.String, dis.CategoryId);

                                            status = dbSmartAspects.ExecuteNonQuery(commandDiscount, transction);
                                        }
                                    }
                                }
                            }

                            if (status > 0 && discountDeletedLst.Count > 0)
                            {
                                using (DbCommand commandDiscount = dbSmartAspects.GetStoredProcCommand("DeleteBanquetReservationClassificationDiscount_SP"))
                                {
                                    foreach (BanquetReservationClassificationDiscountBO dis in discountDeletedLst)
                                    {
                                        if (dis.Id == 0)
                                        {
                                            commandDiscount.Parameters.Clear();
                                            dbSmartAspects.AddInParameter(commandDiscount, "@ReservationId", DbType.Int32, tmpReservationId);
                                            dbSmartAspects.AddInParameter(commandDiscount, "@DiscountId", DbType.String, dis.Id);

                                            status = dbSmartAspects.ExecuteNonQuery(commandDiscount, transction);
                                        }
                                    }
                                }
                            }

                            if (status > 0 && reservationBO.DiscountAmount > 0)
                            {
                                using (DbCommand commandDiscount = dbSmartAspects.GetStoredProcCommand("BanquetServiceRateNVatNServiceChargeProcess_SP"))
                                {
                                    commandDiscount.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDiscount, "@ReservationId", DbType.Int64, tmpReservationId);
                                    dbSmartAspects.AddInParameter(commandDiscount, "@CategoryIdList", DbType.Int32, DBNull.Value);
                                    dbSmartAspects.AddInParameter(commandDiscount, "@DiscountType", DbType.String, reservationBO.DiscountType);
                                    dbSmartAspects.AddInParameter(commandDiscount, "@DiscountAmount", DbType.String, reservationBO.DiscountAmount);
                                    dbSmartAspects.AddInParameter(commandDiscount, "@CostCenterId", DbType.Int32, reservationBO.CostCenterId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDiscount, transction);
                                }
                            }

                            if (status > 0)
                            {
                                transction.Commit();
                                retVal = true;
                            }
                            else
                            {
                                retVal = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            transction.Dispose();
                            retVal = false;
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        public bool UpdateBanquetReservationInfoForAddMoreItem(BanquetReservationBO reservationBO, List<BanquetReservationDetailBO> addList, List<BanquetReservationDetailBO> deleteList, ArrayList arrayDelete, List<BanquetReservationClassificationDiscountBO> discountLst, List<BanquetReservationClassificationDiscountBO> discountDeletedLst)
        {
            bool retVal = false;
            int status = 0;
            long tmpReservationId = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        try
                        {
                            using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateBanquetReservationInfoForAddMoreItem_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandMaster, "@ReservationId", DbType.Int64, reservationBO.Id);
                                dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonAdult", DbType.Decimal, reservationBO.NumberOfPersonAdult);
                                dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonChild", DbType.Decimal, reservationBO.NumberOfPersonChild);
                                dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int64, reservationBO.LastModifiedBy);

                                status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                                tmpReservationId = reservationBO.Id;
                            }

                            if (status > 0)
                            {
                                if (addList != null)
                                {
                                    int count = 0;
                                    using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveBanquetReservationDetailInfo_SP"))
                                    {
                                        foreach (BanquetReservationDetailBO detailBO in addList)
                                        {
                                            if (detailBO.Id == 0)
                                            {
                                                commandDetails.Parameters.Clear();

                                                dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int64, tmpReservationId);
                                                dbSmartAspects.AddInParameter(commandDetails, "@ItemTypeId", DbType.Int64, detailBO.ItemTypeId);
                                                dbSmartAspects.AddInParameter(commandDetails, "@ItemType", DbType.String, detailBO.ItemType);
                                                dbSmartAspects.AddInParameter(commandDetails, "@ItemId", DbType.Int64, detailBO.ItemId);
                                                dbSmartAspects.AddInParameter(commandDetails, "@ItemName", DbType.String, detailBO.ItemName);
                                                dbSmartAspects.AddInParameter(commandDetails, "@ItemUnit", DbType.Decimal, detailBO.ItemUnit);
                                                dbSmartAspects.AddInParameter(commandDetails, "@ItemArrivalTime", DbType.DateTime, detailBO.ItemArrivalTime);
                                                dbSmartAspects.AddInParameter(commandDetails, "@ItemDescription", DbType.String, detailBO.ItemDescription);
                                                dbSmartAspects.AddInParameter(commandDetails, "@IsComplementary", DbType.Boolean, detailBO.IsComplementary);
                                                dbSmartAspects.AddInParameter(commandDetails, "@UnitPrice", DbType.Decimal, detailBO.UnitPrice);
                                                dbSmartAspects.AddInParameter(commandDetails, "@TotalPrice", DbType.Decimal, detailBO.TotalPrice);
                                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                            }
                                        }
                                    }
                                }
                            }

                            if (deleteList != null)
                            {
                                foreach (BanquetReservationDetailBO detailBO in deleteList)
                                {
                                    using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                    {
                                        dbSmartAspects.AddInParameter(commandEducation, "@TableName", DbType.String, "BanquetReservationDetail");
                                        dbSmartAspects.AddInParameter(commandEducation, "@TablePKField", DbType.String, "DetailId");
                                        dbSmartAspects.AddInParameter(commandEducation, "@TablePKId", DbType.String, detailBO.Id);
                                        status = dbSmartAspects.ExecuteNonQuery(commandEducation);
                                    }
                                }

                            }

                            if (status > 0)
                            {
                                using (DbCommand commandDiscount = dbSmartAspects.GetStoredProcCommand("SaveBanquetReservationClassificationDiscount_SP"))
                                {
                                    foreach (BanquetReservationClassificationDiscountBO dis in discountLst)
                                    {
                                        if (dis.Id == 0)
                                        {
                                            commandDiscount.Parameters.Clear();
                                            dbSmartAspects.AddInParameter(commandDiscount, "@ReservationId", DbType.Int64, tmpReservationId);
                                            dbSmartAspects.AddInParameter(commandDiscount, "@CategoryId", DbType.String, dis.CategoryId);

                                            status = dbSmartAspects.ExecuteNonQuery(commandDiscount, transction);
                                        }
                                    }
                                }
                            }

                            if (status > 0)
                            {
                                using (DbCommand commandDiscount = dbSmartAspects.GetStoredProcCommand("DeleteBanquetReservationClassificationDiscount_SP"))
                                {
                                    foreach (BanquetReservationClassificationDiscountBO dis in discountDeletedLst)
                                    {
                                        if (dis.Id == 0)
                                        {
                                            commandDiscount.Parameters.Clear();
                                            dbSmartAspects.AddInParameter(commandDiscount, "@ReservationId", DbType.Int64, tmpReservationId);
                                            dbSmartAspects.AddInParameter(commandDiscount, "@DiscountId", DbType.String, dis.Id);

                                            status = dbSmartAspects.ExecuteNonQuery(commandDiscount, transction);
                                        }
                                    }
                                }
                            }

                            if (status > 0)
                            {
                                using (DbCommand commandDiscount = dbSmartAspects.GetStoredProcCommand("BanquetServiceRateNVatNServiceChargeProcess_SP"))
                                {
                                    commandDiscount.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDiscount, "@ReservationId", DbType.Int64, tmpReservationId);
                                    dbSmartAspects.AddInParameter(commandDiscount, "@CategoryIdList", DbType.Int32, DBNull.Value);
                                    dbSmartAspects.AddInParameter(commandDiscount, "@DiscountType", DbType.String, reservationBO.DiscountType);
                                    dbSmartAspects.AddInParameter(commandDiscount, "@DiscountAmount", DbType.String, reservationBO.DiscountAmount);
                                    dbSmartAspects.AddInParameter(commandDiscount, "@CostCenterId", DbType.String, reservationBO.CostCenterId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDiscount, transction);
                                }
                            }

                            if (status > 0)
                            {
                                transction.Commit();
                                retVal = true;
                            }
                            else
                            {
                                retVal = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            transction.Dispose();
                            retVal = false;
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        public List<BanquetReservationBO> GetAllBanquetReservationInformationBySearchCriteria(string Name, string Email, string ReservationNumber, string Phone, int? BanquetId, DateTime? ArraiveDate, DateTime? DepartureDate)
        {
            List<BanquetReservationBO> salesList = new List<BanquetReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllBanquetReservationInformationBySearchCriteria_SP"))
                {
                    if (!string.IsNullOrEmpty(Name))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, Name);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(Email))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Email", DbType.String, Email);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Email", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(ReservationNumber))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReservationNumber", DbType.String, ReservationNumber);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReservationNumber", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(Phone))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Phone", DbType.String, Phone);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Phone", DbType.String, DBNull.Value);
                    }

                    if (BanquetId != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@BanquetId", DbType.Int64, BanquetId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@BanquetId", DbType.Int64, DBNull.Value);
                    }

                    if (ArraiveDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ArriveDate", DbType.DateTime, ArraiveDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ArriveDate", DbType.DateTime, DBNull.Value);
                    }

                    if (DepartureDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartureDate", DbType.DateTime, DepartureDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartureDate", DbType.DateTime, DBNull.Value);
                    }

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetReservationBO salesBO = new BanquetReservationBO();
                                salesBO.Id = Convert.ToInt64(reader["Id"]);
                                salesBO.ReservationNumber = reader["ReservationNumber"].ToString();
                                salesBO.Name = reader["Name"].ToString();
                                salesBO.EmailAddress = reader["EmailAddress"].ToString();
                                //salesBO.CustomerName = reader["CustomerName"].ToString();
                                //salesBO.InvoiceNumber = reader["InvoiceNumber"].ToString();
                                salesBO.ActiveStatus = Convert.ToBoolean(reader["ActiveStatus"]);
                                salesList.Add(salesBO);
                            }
                        }
                    }
                }
            }
            return salesList;
        }
        private string GenerateWhereCondition(string Name, string Email, string ReservationNumber, string Phone, int BanquetId, DateTime ArraiveDate, DateTime DepartureDate)
        {

            string Where = string.Empty, Condition = string.Empty;

            if (!string.IsNullOrEmpty(Name))
            {
                Condition = " BR.NAME = '" + Name + "'";
            }

            if (!string.IsNullOrEmpty(Email))
            {
                if (string.IsNullOrEmpty(Condition))
                {
                    Condition = " BR.Email = '" + Email + "'";
                }
                else
                {
                    Condition += " AND BR.Email = '" + Email + "'";
                }
            }

            if (!string.IsNullOrEmpty(ReservationNumber))
            {
                if (string.IsNullOrEmpty(Condition))
                {
                    Condition = " BR.ReservationNumber = '" + ReservationNumber + "'";
                }
                else
                {
                    Condition += " AND BR.ReservationNumber = '" + ReservationNumber + "'";
                }
            }

            if (!string.IsNullOrEmpty(Phone))
            {
                if (string.IsNullOrEmpty(Condition))
                {
                    Condition = " BR.Phone = '" + Phone + "'";
                }
                else
                {
                    Condition += " AND BR.Phone = '" + Phone + "'";
                }
            }

            if (!string.IsNullOrEmpty(BanquetId.ToString()))
            {
                if (string.IsNullOrEmpty(Condition))
                {
                    Condition = " BR.BanquetId = '" + BanquetId + "'";
                }
                else
                {
                    Condition += " AND BR.BanquetId = '" + BanquetId + "'";
                }
            }


            if (string.IsNullOrEmpty(Condition))
            {
                Condition += "OR  ( dbo.FnDate(BR.ArriveDate) >= dbo.FnDate('" + ArraiveDate.ToString("yyyy-MM-dd") + "'";
                Condition += ") AND dbo.FnDate(BR.DepartureDate) <= dbo.FnDate('" + DepartureDate.ToString("yyyy-MM-dd") + "'" + ") )";

            }

            if (!string.IsNullOrEmpty(Condition))
            {
                Where += " WHERE " + Condition;
            }
            return Where;
        }
        public bool DeleteBanquetReservationInfoById(long _reservationId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteBanquetReservationInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int64, _reservationId);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public BanquetReservationBO GetBanquetReservationInfoById(long ReservationId)
        {
            BanquetReservationBO reservationBO = new BanquetReservationBO();
            
            
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetReservationInfoById_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int64, ReservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            if (reader.Read())
                            {
                                reservationBO.Id = Convert.ToInt64(reader["Id"]);
                                reservationBO.IsListedCompany = Convert.ToBoolean(reader["IsListedCompany"]);
                                reservationBO.CompanyId = Convert.ToInt32(reader["CompanyId"].ToString());
                                reservationBO.BanquetId = Convert.ToInt64(reader["BanquetId"]);
                                reservationBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"].ToString());
                                reservationBO.ReservationMode = reader["ReservationMode"].ToString();
                                reservationBO.ReservationNumber = reader["ReservationNumber"].ToString();
                                reservationBO.Name = reader["Name"].ToString();
                                reservationBO.CityName = reader["CityName"].ToString();
                                reservationBO.ZipCode = reader["ZipCode"].ToString();
                                reservationBO.EmailAddress = reader["EmailAddress"].ToString();
                                reservationBO.CountryId = Convert.ToInt32(reader["CountryId"].ToString());
                                reservationBO.PhoneNumber = reader["PhoneNumber"].ToString();
                                reservationBO.BookingFor = reader["BookingFor"].ToString();
                                reservationBO.ContactPerson = reader["ContactPerson"].ToString();
                                reservationBO.ContactPhone = reader["ContactPhone"].ToString();
                                reservationBO.PhoneNumber = reader["PhoneNumber"].ToString();
                                reservationBO.ContactEmail = reader["ContactEmail"].ToString();
                                reservationBO.Address = reader["Address"].ToString();
                                reservationBO.Remarks = reader["Comments"].ToString();
                                reservationBO.DepartureDate = Convert.ToDateTime(reader["DepartureDate"].ToString());
                                reservationBO.ArriveDate = Convert.ToDateTime(reader["ArriveDate"].ToString());
                                reservationBO.IsReturnedClient = Convert.ToBoolean(reader["IsReturnedClient"]);
                                reservationBO.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"].ToString());
                                reservationBO.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"].ToString());
                                reservationBO.OccessionTypeId = Convert.ToInt64(reader["OccessionTypeId"].ToString());
                                reservationBO.SeatingId = Convert.ToInt64(reader["SeatingId"].ToString());
                                if (reader["RefferenceId"] != DBNull.Value)
                                    reservationBO.RefferenceId = Convert.ToInt64(reader["RefferenceId"].ToString());
                                reservationBO.TotalAmount = Convert.ToDecimal(reader["TotalAmount"].ToString());
                                reservationBO.ReservationDiscountType = reader["ReservationDiscountType"].ToString();
                                reservationBO.ReservationDiscountAmount = Convert.ToDecimal(reader["ReservationDiscountAmount"].ToString());
                                reservationBO.DiscountType = reader["DiscountType"].ToString();
                                reservationBO.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"].ToString());
                                reservationBO.DiscountedAmount = Convert.ToDecimal(reader["DiscountedAmount"].ToString());
                                reservationBO.BanquetRate = Convert.ToDecimal(reader["BanquetRate"].ToString());

                                reservationBO.InvoiceServiceRate = Convert.ToDecimal(reader["InvoiceServiceRate"].ToString());
                                reservationBO.IsInvoiceServiceChargeEnable = Convert.ToBoolean(reader["IsInvoiceServiceChargeEnable"]);
                                reservationBO.IsInvoiceCitySDChargeEnable = Convert.ToBoolean(reader["IsInvoiceCitySDChargeEnable"]);
                                reservationBO.IsInvoiceVatAmountEnable = Convert.ToBoolean(reader["IsInvoiceVatAmountEnable"]);
                                reservationBO.IsInvoiceAdditionalChargeEnable = Convert.ToBoolean(reader["IsInvoiceAdditionalChargeEnable"]);

                                reservationBO.InvoiceServiceCharge = Convert.ToDecimal(reader["InvoiceServiceCharge"].ToString());
                                reservationBO.InvoiceCitySDCharge = Convert.ToDecimal(reader["InvoiceCitySDCharge"].ToString());
                                reservationBO.InvoiceVatAmount = Convert.ToDecimal(reader["InvoiceVatAmount"].ToString());
                                reservationBO.InvoiceAdditionalCharge = Convert.ToDecimal(reader["InvoiceAdditionalCharge"].ToString());
                                reservationBO.GrandTotal = Convert.ToDecimal(reader["GrandTotal"].ToString());
                                reservationBO.IsBillSettlement = Convert.ToBoolean(reader["IsBillSettlement"]);

                                reservationBO.GLProjectId = Convert.ToInt32(reader["GLProjectId"]);
                                reservationBO.GLCompanyId = Convert.ToInt32(reader["GLCompanyId"]);
                                reservationBO.EventType = reader["EventType"].ToString();
                                reservationBO.EventTitle = reader["EventTitle"].ToString();
                                reservationBO.CancellationReason = reader["CancellationReason"].ToString();
                                reservationBO.ReferenceName = reader["ReferenceName"].ToString();
                                reservationBO.CreatedBy = Convert.ToInt64(reader["CreatedBy"].ToString());
                                reservationBO.MeetingAgenda = reader["MeetingAgenda"].ToString();
                                //reservationBO.PerticipantFromOffice = reader["PerticipantFromOffice"]
                                
                            }
                        }
                    }
                }


                List<EmployeeBO> EmployeeIdList = new List<EmployeeBO>();
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeIdByBanquetReservationId_sp"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BanquetReservationId", DbType.Int64, ReservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmployeeBO employeeBO = new EmployeeBO();
                                employeeBO.EmpId = int.Parse(reader["EmployeeId"].ToString());
                                EmployeeIdList.Add(employeeBO);
                            }
                        }
                    }
                }
                reservationBO.PerticipantFromOffice = EmployeeIdList;
            }
            return reservationBO;
        }


        public BanquetReservationBO GetBanquetReservationInfoByReservationNo(string searchType, string ReservationNumber)
        {
            BanquetReservationBO reservationBO = new BanquetReservationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetReservationInfoByReservationNo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchType", DbType.String, searchType);
                    dbSmartAspects.AddInParameter(cmd, "@ReservationNo", DbType.String, ReservationNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                reservationBO.Id = Convert.ToInt64(reader["Id"]);
                                reservationBO.BanquetId = Convert.ToInt64(reader["BanquetId"]);
                                reservationBO.ReservationNumber = reader["ReservationNumber"].ToString();
                                reservationBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                reservationBO.Name = reader["Name"].ToString();
                                reservationBO.CityName = reader["CityName"].ToString();
                                reservationBO.ZipCode = reader["ZipCode"].ToString();
                                reservationBO.EmailAddress = reader["EmailAddress"].ToString();
                                reservationBO.CountryId = Convert.ToInt32(reader["CountryId"].ToString());
                                reservationBO.PhoneNumber = reader["PhoneNumber"].ToString();
                                reservationBO.BookingFor = reader["BookingFor"].ToString();
                                reservationBO.ContactPerson = reader["ContactPerson"].ToString();
                                reservationBO.ContactPhone = reader["ContactPhone"].ToString();
                                reservationBO.PhoneNumber = reader["PhoneNumber"].ToString();
                                reservationBO.ContactEmail = reader["ContactEmail"].ToString();
                                reservationBO.Address = reader["Address"].ToString();
                                reservationBO.DepartureDate = Convert.ToDateTime(reader["DepartureDate"].ToString());
                                reservationBO.ArriveDate = Convert.ToDateTime(reader["ArriveDate"].ToString());
                                reservationBO.IsReturnedClient = Convert.ToBoolean(reader["IsReturnedClient"]);
                                reservationBO.NumberOfPersonAdult = Convert.ToInt32(reader["NumberOfPersonAdult"].ToString());
                                reservationBO.NumberOfPersonChild = Convert.ToInt32(reader["NumberOfPersonChild"].ToString());
                                reservationBO.OccessionTypeId = Convert.ToInt64(reader["OccessionTypeId"].ToString());
                                reservationBO.SeatingId = Convert.ToInt64(reader["SeatingId"].ToString());
                                if (reader["RefferenceId"] != DBNull.Value)
                                    reservationBO.RefferenceId = Convert.ToInt64(reader["RefferenceId"].ToString());

                                reservationBO.InvoiceServiceRate = Convert.ToDecimal(reader["InvoiceServiceRate"].ToString());
                                reservationBO.IsInvoiceServiceChargeEnable = Convert.ToBoolean(reader["IsInvoiceServiceChargeEnable"]);
                                reservationBO.IsInvoiceCitySDChargeEnable = Convert.ToBoolean(reader["IsInvoiceCitySDChargeEnable"]);
                                reservationBO.IsInvoiceVatAmountEnable = Convert.ToBoolean(reader["IsInvoiceVatAmountEnable"]);
                                reservationBO.IsInvoiceAdditionalChargeEnable = Convert.ToBoolean(reader["IsInvoiceAdditionalChargeEnable"]);

                                reservationBO.InvoiceServiceCharge = Convert.ToDecimal(reader["InvoiceServiceCharge"].ToString());
                                reservationBO.InvoiceCitySDCharge = Convert.ToDecimal(reader["InvoiceCitySDCharge"].ToString());
                                reservationBO.InvoiceVatAmount = Convert.ToDecimal(reader["InvoiceVatAmount"].ToString());
                                reservationBO.InvoiceAdditionalCharge = Convert.ToDecimal(reader["InvoiceAdditionalCharge"].ToString());
                                reservationBO.GrandTotal = Convert.ToDecimal(reader["GrandTotal"].ToString());

                                reservationBO.TotalAmount = Convert.ToDecimal(reader["TotalAmount"].ToString());
                                reservationBO.DiscountType = reader["DiscountType"].ToString();
                                reservationBO.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"].ToString());
                                reservationBO.DiscountedAmount = Convert.ToDecimal(reader["DiscountedAmount"].ToString());
                                reservationBO.BanquetRate = Convert.ToDecimal(reader["BanquetRate"].ToString());

                                reservationBO.ActiveStatus = Convert.ToBoolean(reader["ActiveStatus"]);
                                reservationBO.ReferenceName = reader["ReferenceName"].ToString();
                                reservationBO.EventType = reader["EventType"].ToString();
                                reservationBO.CostCenterId = Convert.ToInt32(reader["CostcenterId"].ToString());
                            }
                        }
                    }
                }
                List<EmployeeBO> EmployeeIdList = new List<EmployeeBO>();
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeIdByBanquetReservationId_sp"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BanquetReservationId", DbType.Int64, reservationBO.Id);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmployeeBO employeeBO = new EmployeeBO();
                                employeeBO.EmpId = int.Parse(reader["EmployeeId"].ToString());
                                EmployeeIdList.Add(employeeBO);
                            }
                        }
                    }
                }
                reservationBO.PerticipantFromOffice = EmployeeIdList;
            }
            return reservationBO;
        }
        public List<BanquetReservationBO> GetAllBanquetReservationInfo()
        {
            List<BanquetReservationBO> salesList = new List<BanquetReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllBanquetReservationInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetReservationBO salesBO = new BanquetReservationBO();
                                salesBO.Id = Convert.ToInt64(reader["Id"]);
                                salesBO.ReservationNumber = reader["ReservationNumber"].ToString();
                                salesBO.Name = reader["Name"].ToString();
                                salesBO.EmailAddress = reader["EmailAddress"].ToString();
                                //salesBO.CustomerName = reader["CustomerName"].ToString();
                                //salesBO.InvoiceNumber = reader["InvoiceNumber"].ToString();

                                salesList.Add(salesBO);
                            }
                        }
                    }
                }
            }
            return salesList;
        }
        public bool UpdateBanquetReservationStatusInfo(BanquetReservationBO reservationBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateBanquetReservationStatusInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int64, reservationBO.Id);
                    dbSmartAspects.AddInParameter(command, "@Reason", DbType.String, reservationBO.Reason);
                    //dbSmartAspects.AddInParameter(command, "@Status", DbType.String, reservationBO.Status);
                    dbSmartAspects.AddInParameter(command, "@ActiveStatus", DbType.Int32, 0);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int64, reservationBO.LastModifiedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<BanquetReservationForCalendarViewBO> GetAllReservedBanquetInfoForCalendar(DateTime currentDate)
        {
            List<BanquetReservationForCalendarViewBO> entityBOList = new List<BanquetReservationForCalendarViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReservedBanquetInfoForCalender_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CurrentDate", DbType.DateTime, currentDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetReservationForCalendarViewBO entityBO = new BanquetReservationForCalendarViewBO();
                                entityBO.ReservationId = Convert.ToInt64(reader["ReservationId"]);
                                entityBO.BanquetId = Convert.ToInt64(reader["BanquetId"]);
                                entityBO.GuestName = reader["Name"].ToString();
                                entityBO.ArriveHour = Convert.ToInt32(reader["ArriveHour"]);
                                entityBO.DepartureHour = Convert.ToInt32(reader["DepartureHour"]);
                                entityBO.IsBillSettlement = Convert.ToBoolean(reader["IsBillSettlement"]);

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<BanquetReservationBO> GetBanquetReservationInfoForRegistrationSearch(string reservationNumber, string guestName)
        {
            List<BanquetReservationBO> banquetReservationList = new List<BanquetReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetReservationInfoForRegistrationSearch_SP"))
                {
                    //dbSmartAspects.AddInParameter(cmd, "@IsAllActiveReservation", DbType.Int32, IsAllActiveReservation);

                    if (!string.IsNullOrWhiteSpace(reservationNumber))
                        dbSmartAspects.AddInParameter(cmd, "@ReservationNumber", DbType.String, reservationNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReservationNumber", DbType.String, DBNull.Value);

                    if (!string.IsNullOrWhiteSpace(guestName))
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, guestName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetReservationBO banquetReservation = new BanquetReservationBO();

                                banquetReservation.Id = Convert.ToInt64(reader["Id"]);
                                banquetReservation.Name = reader["Name"].ToString();
                                banquetReservation.ArriveDate = Convert.ToDateTime(reader["ArriveDate"]);
                                banquetReservation.DepartureDate = Convert.ToDateTime(reader["DepartureDate"]);
                                banquetReservation.ReservationNumber = reader["ReservationNumber"].ToString();
                                banquetReservation.ContactPerson = reader["ContactPerson"].ToString();

                                banquetReservationList.Add(banquetReservation);
                            }
                        }
                    }
                }
            }
            return banquetReservationList;
        }
        public Boolean SettlementBanquetReservationInfo(BanquetReservationBO reservationBO, List<GuestBillPaymentBO> guestPaymentDetailList, out long tmpReservationId)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SettlementBanquetReservationInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@ReservationId", DbType.Int64, reservationBO.Id);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsBillSettlement", DbType.Boolean, reservationBO.IsBillSettlement);
                            dbSmartAspects.AddInParameter(commandMaster, "@RebateRemarks", DbType.String, reservationBO.RebateRemarks);
                            dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int64, reservationBO.CreatedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@MeetingDiscussion", DbType.String, reservationBO.MeetingDiscussion);
                            dbSmartAspects.AddInParameter(commandMaster, "@CallToAction", DbType.String, reservationBO.CallToAction);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsUnderCompany", DbType.Boolean, reservationBO.IsUnderCompany);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            tmpReservationId = Convert.ToInt64(commandMaster.Parameters["@ReservationId"].Value);
                        }

                        if (status > 0 && !string.IsNullOrEmpty(reservationBO.PerticipantFromOfficeCommaSeperatedIds))
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveBanquetReservationPerticipantFromOffice_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@BanquetReservationId", DbType.Int32, tmpReservationId);
                                dbSmartAspects.AddInParameter(commandDetails, "@EmpIdsCommaSeperated", DbType.String, reservationBO.PerticipantFromOfficeCommaSeperatedIds);

                                dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                            }
                        }

                        if (status > 0 && !string.IsNullOrEmpty(reservationBO.PerticipantFromClientCommaSeperatedIds))
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveBanquetReservationParticipantFromClient_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@BanquetReservationId", DbType.Int32, tmpReservationId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ClientIdsCommaSeperated", DbType.String, reservationBO.PerticipantFromClientCommaSeperatedIds);

                                dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                            }
                        }


                        if (status > 0)
                        {
                            if (guestPaymentDetailList != null)
                            {
                                using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoForBanquet_SP"))
                                {
                                    foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
                                    {
                                        commandGuestBillPayment.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Banquet");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        if (guestBillPaymentBO.PaymentMode == "Other Room")
                                        {
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                        }
                                        else
                                        {
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, 0);
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, reservationBO.Id);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RefundAccountHead", DbType.Int32, guestBillPaymentBO.RefundAccountHead);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@Remarks", DbType.String, guestBillPaymentBO.Remarks);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, reservationBO.CreatedBy);
                                        dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));

                                        status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                        //countGuestBillPaymentList = countGuestBillPaymentList + 1;
                                    }
                                }
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("BanquetReservationBillSettlement_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandMaster, "@ReservationId", DbType.Int64, reservationBO.Id);
                                dbSmartAspects.AddInParameter(commandMaster, "@RebateRemarks", DbType.String, reservationBO.RebateRemarks);
                                dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int64, reservationBO.CreatedBy);
                                dbSmartAspects.AddInParameter(commandMaster, "@BillStatus", DbType.String, DBNull.Value);

                                status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand commandDiscount = dbSmartAspects.GetStoredProcCommand("BanquetServiceRateNVatNServiceChargeProcess_SP"))
                            {
                                commandDiscount.Parameters.Clear();
                                dbSmartAspects.AddInParameter(commandDiscount, "@ReservationId", DbType.Int64, reservationBO.Id);
                                dbSmartAspects.AddInParameter(commandDiscount, "@CategoryIdList", DbType.Int32, DBNull.Value);
                                dbSmartAspects.AddInParameter(commandDiscount, "@DiscountType", DbType.String, reservationBO.DiscountType);
                                dbSmartAspects.AddInParameter(commandDiscount, "@DiscountAmount", DbType.String, reservationBO.DiscountAmount);
                                dbSmartAspects.AddInParameter(commandDiscount, "@CostCenterId", DbType.String, reservationBO.CostCenterId);

                                status = dbSmartAspects.ExecuteNonQuery(commandDiscount, transction);
                            }
                        }

                        if (status > 0 && reservationBO.IsBillReSettlement)
                        {
                            if (status > 0)
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("BanquetBillItemRollBackAtReSettlementTime_SP"))
                                {
                                    command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int64, reservationBO.Id);
                                    dbSmartAspects.AddInParameter(command, "@CostcenterId", DbType.Int32, reservationBO.CostCenterId);
                                    dbSmartAspects.AddInParameter(command, "@BillStatus", DbType.String, DBNull.Value);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                    status = status == 0 ? 1 : status;
                                }
                            }

                            if (status > 0)
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("BanquetReservationBillSettlement_SP"))
                                {
                                    command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                    dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int64, reservationBO.Id);
                                    dbSmartAspects.AddInParameter(command, "@RebateRemarks", DbType.String, reservationBO.Remarks);
                                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int64, reservationBO.CreatedBy);
                                    dbSmartAspects.AddInParameter(command, "@BillStatus", DbType.String, DBNull.Value);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }

                            if (status > 0)
                            {
                                using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("BanquetBillReSettlementLog_SP"))
                                {
                                    dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ReservationId", DbType.Int64, reservationBO.Id);
                                    dbSmartAspects.AddInParameter(commandPercentageDiscount, "@UserInfoId", DbType.Int32, reservationBO.CreatedBy);

                                    status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                                    status = status == 0 ? 1 : status;
                                }
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("BanquetSettlementProcessForInternal_SP"))
                            {
                                command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, reservationBO.Id);
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int64, reservationBO.CreatedBy);

                                //status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;

                        throw ex;
                    }
                }
            }

            return retVal;
        }

        

        public List<BanquetReservationBO> GetBanquetReservationInfoForBillSearch(DateTime? startDate, DateTime? endDate, string reservationNumber, int recordPerPage, int pageIndex, out int totalRecords)
        {
            string Where = GenerateReservationSearchWhereCondition(startDate, endDate, reservationNumber);
            List<BanquetReservationBO> billList = new List<BanquetReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetReservationInfoForBillSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, Where);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "BillInfo");
                    DataTable Table = ds.Tables["BillInfo"];

                    billList = Table.AsEnumerable().Select(r => new BanquetReservationBO
                    {
                        Id = r.Field<long>("Id"),
                        EventType = r.Field<string>("EventType"),
                        BanquetId = r.Field<long>("BanquetId"),
                        ReservationNumber = r.Field<string>("ReservationNumber"),
                        Name = r.Field<string>("Name"),
                        CityName = r.Field<string>("CityName"),
                        ZipCode = r.Field<string>("ZipCode"),
                        EmailAddress = r.Field<string>("EmailAddress"),
                        CountryId = r.Field<int>("CountryId"),
                        PhoneNumber = r.Field<string>("PhoneNumber"),
                        BookingFor = r.Field<string>("BookingFor"),
                        ContactPerson = r.Field<string>("ContactPerson"),
                        ContactPhone = r.Field<string>("ContactPhone"),
                        ContactEmail = r.Field<string>("ContactEmail"),
                        Address = r.Field<string>("Address"),
                        DepartureDate = r.Field<DateTime>("DepartureDate"),
                        ArriveDate = r.Field<DateTime>("ArriveDate"),
                        IsReturnedClient = r.Field<Boolean>("IsReturnedClient"),
                        NumberOfPersonAdult = r.Field<int>("NumberOfPersonAdult"),
                        NumberOfPersonChild = r.Field<int>("NumberOfPersonChild"),
                        OccessionTypeId = r.Field<long>("OccessionTypeId"),
                        SeatingId = r.Field<long>("SeatingId"),
                        RefferenceId = r.Field<long>("RefferenceId"),
                        TotalAmount = r.Field<decimal>("TotalAmount"),
                        DiscountAmount = r.Field<decimal>("DiscountAmount"),
                        DiscountedAmount = r.Field<decimal>("DiscountedAmount"),
                        BanquetRate = r.Field<decimal>("BanquetRate"),
                        IsInvoiceServiceChargeEnable = r.Field<Boolean>("IsInvoiceServiceChargeEnable"),
                        IsInvoiceVatAmountEnable = r.Field<Boolean>("IsInvoiceVatAmountEnable"),
                        BillStatus = r.Field<string>("BillStatus"),
                        IsDayClosed = r.Field<int>("IsDayClosed")
                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return billList;
        }
        public string GenerateReservationSearchWhereCondition(DateTime? startDate, DateTime? endDate, string reservationNumber)
        {
            string Where = string.Empty;
            reservationNumber = reservationNumber.Trim();

            if (startDate != null && endDate != null)
            {
                string strStartDate = startDate.HasValue ? startDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                string strEndDate = endDate.HasValue ? endDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                Where = " AND (dbo.FnDate(br.ArriveDate) BETWEEN dbo.FnDate('" + strStartDate + "') AND dbo.FnDate('" + strEndDate + "') )";
            }

            //Reservation Number
            if (!string.IsNullOrWhiteSpace(reservationNumber))
            {
                Where += " AND br.ReservationNumber LIKE '%" + reservationNumber + "%'";
            }

            return Where;
        }
        public List<BanquetReservationBO> GetBanquetReservationInfoForDuplicateChecking(int editId, int banquetId, DateTime arriveFromDate, DateTime departToDate)
        {
            List<BanquetReservationBO> billList = new List<BanquetReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetReservationInfoForDuplicateCheckingNew_SP"))
                {
                    //if (arriveTime.Length == 2)
                    //{ arriveTime = arriveTime + ":00:01"; }


                    dbSmartAspects.AddInParameter(cmd, "@BanquetId", DbType.Int64, banquetId);
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int64, editId);
                    dbSmartAspects.AddInParameter(cmd, "@ArriveFromDate", DbType.DateTime, arriveFromDate);
                    dbSmartAspects.AddInParameter(cmd, "@DepartToDate", DbType.DateTime, departToDate);

                    //dbSmartAspects.AddInParameter(cmd, "@ArriveTime", DbType.String, arriveTime);
                    //dbSmartAspects.AddInParameter(cmd, "@DepartTime", DbType.String, departTime);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "BillInfo");
                    DataTable Table = ds.Tables["BillInfo"];

                    billList = Table.AsEnumerable().Select(r => new BanquetReservationBO
                    {
                        Id = r.Field<long>("Id"),
                        BanquetId = r.Field<long>("BanquetId"),
                        ReservationNumber = r.Field<string>("ReservationNumber"),
                        Name = r.Field<string>("Name"),
                        CityName = r.Field<string>("CityName"),
                        ZipCode = r.Field<string>("ZipCode"),
                        EmailAddress = r.Field<string>("EmailAddress"),
                        CountryId = r.Field<int>("CountryId"),
                        PhoneNumber = r.Field<string>("PhoneNumber"),
                        BookingFor = r.Field<string>("BookingFor"),
                        ContactPerson = r.Field<string>("ContactPerson"),
                        ContactPhone = r.Field<string>("ContactPhone"),
                        //PhoneNumber = r.Field<string>("PhoneNumber"),
                        ContactEmail = r.Field<string>("ContactEmail"),
                        Address = r.Field<string>("Address"),
                        DepartureDate = r.Field<DateTime>("DepartureDate"),
                        ArriveDate = r.Field<DateTime>("ArriveDate"),
                        IsReturnedClient = r.Field<Boolean>("IsReturnedClient"),
                        NumberOfPersonAdult = r.Field<int>("NumberOfPersonAdult"),
                        NumberOfPersonChild = r.Field<int>("NumberOfPersonChild"),
                        OccessionTypeId = r.Field<long>("OccessionTypeId"),
                        SeatingId = r.Field<long>("SeatingId"),
                        RefferenceId = r.Field<long>("RefferenceId"),
                        TotalAmount = r.Field<decimal>("TotalAmount"),
                        DiscountAmount = r.Field<decimal>("DiscountAmount"),
                        DiscountedAmount = r.Field<decimal>("DiscountedAmount"),
                        BanquetRate = r.Field<decimal>("BanquetRate"),
                        IsInvoiceServiceChargeEnable = r.Field<Boolean>("IsInvoiceServiceChargeEnable"),
                        IsInvoiceCitySDChargeEnable = r.Field<Boolean>("IsInvoiceCitySDChargeEnable"),
                        IsInvoiceVatAmountEnable = r.Field<Boolean>("IsInvoiceVatAmountEnable"),
                        IsInvoiceAdditionalChargeEnable = r.Field<Boolean>("IsInvoiceAdditionalChargeEnable"),
                    }).ToList();
                }
            }
            return billList;
        }
        public List<BanquetReservationBO> GetBanquetReservationInfoForDuplicateChecking(int banquetId, DateTime arriveFromDate, DateTime departToDate)
        {
            List<BanquetReservationBO> billList = new List<BanquetReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetReservationInfoForDuplicateChecking_SP"))
                {
                    //if (arriveTime.Length == 2)
                    //{ arriveTime = arriveTime + ":00:01"; }


                    dbSmartAspects.AddInParameter(cmd, "@BanquetId", DbType.Int64, banquetId);
                    dbSmartAspects.AddInParameter(cmd, "@ArriveFromDate", DbType.DateTime, arriveFromDate);
                    dbSmartAspects.AddInParameter(cmd, "@DepartToDate", DbType.DateTime, departToDate);

                    //dbSmartAspects.AddInParameter(cmd, "@ArriveTime", DbType.String, arriveTime);
                    //dbSmartAspects.AddInParameter(cmd, "@DepartTime", DbType.String, departTime);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "BillInfo");
                    DataTable Table = ds.Tables["BillInfo"];

                    billList = Table.AsEnumerable().Select(r => new BanquetReservationBO
                    {
                        Id = r.Field<long>("Id"),
                        BanquetId = r.Field<long>("BanquetId"),
                        ReservationNumber = r.Field<string>("ReservationNumber"),
                        Name = r.Field<string>("Name"),
                        CityName = r.Field<string>("CityName"),
                        ZipCode = r.Field<string>("ZipCode"),
                        EmailAddress = r.Field<string>("EmailAddress"),
                        CountryId = r.Field<int>("CountryId"),
                        PhoneNumber = r.Field<string>("PhoneNumber"),
                        BookingFor = r.Field<string>("BookingFor"),
                        ContactPerson = r.Field<string>("ContactPerson"),
                        ContactPhone = r.Field<string>("ContactPhone"),
                        //PhoneNumber = r.Field<string>("PhoneNumber"),
                        ContactEmail = r.Field<string>("ContactEmail"),
                        Address = r.Field<string>("Address"),
                        DepartureDate = r.Field<DateTime>("DepartureDate"),
                        ArriveDate = r.Field<DateTime>("ArriveDate"),
                        IsReturnedClient = r.Field<Boolean>("IsReturnedClient"),
                        NumberOfPersonAdult = r.Field<int>("NumberOfPersonAdult"),
                        NumberOfPersonChild = r.Field<int>("NumberOfPersonChild"),
                        OccessionTypeId = r.Field<long>("OccessionTypeId"),
                        SeatingId = r.Field<long>("SeatingId"),
                        RefferenceId = r.Field<long>("RefferenceId"),
                        TotalAmount = r.Field<decimal>("TotalAmount"),
                        DiscountAmount = r.Field<decimal>("DiscountAmount"),
                        DiscountedAmount = r.Field<decimal>("DiscountedAmount"),
                        BanquetRate = r.Field<decimal>("BanquetRate"),
                        IsInvoiceServiceChargeEnable = r.Field<Boolean>("IsInvoiceServiceChargeEnable"),
                        IsInvoiceCitySDChargeEnable = r.Field<Boolean>("IsInvoiceCitySDChargeEnable"),
                        IsInvoiceVatAmountEnable = r.Field<Boolean>("IsInvoiceVatAmountEnable"),
                        IsInvoiceAdditionalChargeEnable = r.Field<Boolean>("IsInvoiceAdditionalChargeEnable"),
                    }).ToList();
                }
            }
            return billList;
        }

        public List<BanquetReservationBO> GetAllReservationList(string name, string reservationNumber, string email, string phone, int? banquetId, DateTime? arriveDate, DateTime? departureDate, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<BanquetReservationBO> reservationList = new List<BanquetReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllBanquetReservationList_SP"))
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, name);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(email))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Email", DbType.String, email);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Email", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(reservationNumber))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReservationNumber", DbType.String, reservationNumber);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReservationNumber", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(phone))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Phone", DbType.String, phone);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Phone", DbType.String, DBNull.Value);
                    }

                    if (banquetId != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@BanquetId", DbType.Int64, banquetId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@BanquetId", DbType.Int32, DBNull.Value);
                    }

                    if (arriveDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ArriveDate", DbType.DateTime, arriveDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ArriveDate", DbType.DateTime, DBNull.Value);
                    }

                    if (departureDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartureDate", DbType.DateTime, departureDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartureDate", DbType.DateTime, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetReservationBO reservationBO = new BanquetReservationBO();
                                reservationBO.Id = Convert.ToInt64(reader["Id"]);
                                reservationBO.ReservationNumber = reader["ReservationNumber"].ToString();
                                reservationBO.Name = reader["Name"].ToString();
                                reservationBO.ContactEmail = reader["ContactEmail"].ToString();
                                //salesBO.CustomerName = reader["CustomerName"].ToString();
                                //salesBO.InvoiceNumber = reader["InvoiceNumber"].ToString();
                                reservationBO.ActiveStatus = Convert.ToBoolean(reader["ActiveStatus"]);
                                reservationBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                reservationBO.Status = reader["Status"].ToString();
                                reservationBO.EventType = reader["EventType"].ToString();
                                reservationBO.EventTitle = reader["EventTitle"].ToString();
                                reservationBO.IsBillSettlement = Convert.ToBoolean(reader["IsBillSettlement"]);
                                reservationList.Add(reservationBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return reservationList;
        }
        public List<BanquetReservationReportViewBO> GetBanquetInfoForReport(DateTime fromDate, DateTime toDate, long banquetId)
        {
            List<BanquetReservationReportViewBO> banquetList = new List<BanquetReservationReportViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllBanquetReservationInfoForReport_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@BanquetId", DbType.Int64, banquetId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "BanquetReservationReportInfo");
                    DataTable Table = ds.Tables["BanquetReservationReportInfo"];

                    banquetList = Table.AsEnumerable().Select(r => new BanquetReservationReportViewBO
                    {
                        //ReservationDate = r.Field<DateTime>("ArriveDate"),
                        BanquetName = r.Field<string>("BanquetName"),
                        ReservationNo = r.Field<string>("ReservationNo"),
                        ContactPerson = r.Field<string>("ContactPerson"),
                        ContactNo = r.Field<string>("ContactNo"),
                        Email = r.Field<string>("Email"),
                        BookingDate = r.Field<string>("BookingDate"),
                        PartyStartDate = r.Field<string>("PartyStartDate"),
                        PartyEndDate = r.Field<string>("PartyEndDate"),
                        NoOfPersonAdult = r.Field<int>("NoOfPersonAdult"),
                        NoOfPersonChild = r.Field<int>("NoOfPersonChild"),
                        Reference = r.Field<string>("Reference"),
                        Status = r.Field<string>("Status")
                    }).ToList();
                }
            }
            return banquetList;
        }
        public List<BanquetSalesInfoReportViewBO> GetBanquetSalesInfoForReport(DateTime fromDate, DateTime toDate, int categoryidId, int itemId, string referNo, string transtype)
        {
            List<BanquetSalesInfoReportViewBO> salesList = new List<BanquetSalesInfoReportViewBO>();
            //DataSet customerDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetSalesInfoForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@FilterBy", DbType.String, referNo);
                    dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryidId);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceId", DbType.Int32, itemId);
                    dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, transtype);
                    dbSmartAspects.AddInParameter(cmd, "@InformationType", DbType.String, "SalesInfo");

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RestaurantSales");
                    DataTable table = ds.Tables["RestaurantSales"];
                    salesList = table.AsEnumerable().Select(r =>
                                new BanquetSalesInfoReportViewBO
                                {
                                    ServiceDate = r.Field<DateTime>("ServiceDate"),
                                    ServiceDisplayDate = r.Field<string>("ServiceDisplayDate"),
                                    ReferenceNo = r.Field<string>("ReferenceNo"),
                                    CategoryId = r.Field<int?>("CategoryId"),
                                    CategoryName = r.Field<string>("CategoryName"),
                                    ServiceId = r.Field<int?>("ServiceId"),
                                    ServiceName = r.Field<string>("ServiceName"),
                                    RoomNumber = r.Field<string>("RoomNumber"),
                                    ItemQuantity = r.Field<decimal?>("ItemQuantity"),
                                    UnitRate = r.Field<decimal?>("UnitRate"),
                                    TotalAmount = r.Field<decimal?>("TotalAmount"),
                                    DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                                    ServiceRate = r.Field<decimal?>("ServiceRate"),
                                    ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                                    CitySDCharge = r.Field<decimal?>("CitySDCharge"),
                                    VatAmount = r.Field<decimal?>("VatAmount"),
                                    AdditionalCharge = r.Field<decimal?>("AdditionalCharge"),
                                    TotalSalesAmount = r.Field<decimal?>("TotalSalesAmount"),
                                    ItemCost = r.Field<decimal?>("ItemCost"),
                                    ProfitNLossAmount = r.Field<decimal?>("ProfitNLossAmount"),
                                    SalesType = r.Field<string>("SalesType"),
                                    IsDiscountHead = r.Field<int?>("IsDiscountHead")
                                }).ToList();
                }
            }
            return salesList;
        }
        public BanquetReservationBO GetBanquetForDuplicateReservationValidation(long banquetId, DateTime startDate, DateTime endDate)
        {
            BanquetReservationBO reservationBO = new BanquetReservationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetForDuplicateReservationValidation_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@BanquetId", DbType.Int64, banquetId);
                    dbSmartAspects.AddInParameter(cmd, "@StartDate", DbType.DateTime, startDate);
                    dbSmartAspects.AddInParameter(cmd, "@EndDate", DbType.DateTime, endDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ReservationValidation");
                    DataTable table = ds.Tables["ReservationValidation"];
                    reservationBO = table.AsEnumerable().Select(r =>
                                new BanquetReservationBO
                                {
                                    Id = r.Field<long>("Id"),
                                    BanquetId = r.Field<long>("BanquetId")
                                }).FirstOrDefault();
                }
            }
            return reservationBO;
        }
        public List<BanquetReservationClassificationDiscountBO> GetBanquetReservationClassificationDiscount(int reservationId)
        {
            List<BanquetReservationClassificationDiscountBO> discountLst = new List<BanquetReservationClassificationDiscountBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetReservationClassificationDiscount_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int64, reservationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ReservationValidation");
                    DataTable table = ds.Tables["ReservationValidation"];

                    discountLst = table.AsEnumerable().Select(r =>
                                new BanquetReservationClassificationDiscountBO
                                {
                                    Id = r.Field<int>("Id"),
                                    ReservationId = r.Field<int>("ReservationId"),
                                    CategoryId = r.Field<int>("CategoryId")

                                }).ToList();
                }
            }
            return discountLst;
        }
        public Boolean UpdateBanquetReservationRegistrationIdInfoForOtherRoomPayment(int reservationId, string remarks)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateBanquetReservationRegistrationIdInfoForOtherRoomPayment_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int32, reservationId);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, remarks);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool UpdateBanquetReservationBillStatus(BanquetReservationBO banquetReservationBO)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateBanquetReservationBillStatus_SP"))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int64, banquetReservationBO.Id);
                            dbSmartAspects.AddInParameter(cmd, "@BillStatus", DbType.String, banquetReservationBO.BillStatus);
                            dbSmartAspects.AddInParameter(cmd, "@Remarks", DbType.String, banquetReservationBO.Remarks);
                            dbSmartAspects.AddInParameter(cmd, "@BillVoidBy", DbType.Int32, banquetReservationBO.BillVoidBy);
                            dbSmartAspects.AddInParameter(cmd, "@LastModifiedBy", DbType.Int64, banquetReservationBO.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(cmd, transction);
                        }

                        if (status > 0 && banquetReservationBO.BillStatus == "Void")
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("BanquetBillItemRollBackAtReSettlementTime_SP"))
                            {
                                command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, banquetReservationBO.Id);
                                dbSmartAspects.AddInParameter(command, "@CostcenterId", DbType.Int32, banquetReservationBO.CostCenterId);
                                dbSmartAspects.AddInParameter(command, "@BillStatus", DbType.String, "Void");
                                dbSmartAspects.ExecuteNonQuery(command, transction);
                                //status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                status = 1;
                            }
                        }

                        if (status > 0 && banquetReservationBO.BillStatus == "Active")
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("BanquetReservationBillSettlement_SP"))
                            {
                                command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int32, banquetReservationBO.Id);
                                dbSmartAspects.AddInParameter(command, "@RebateRemarks", DbType.String, banquetReservationBO.Remarks);
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int64, banquetReservationBO.LastModifiedBy);
                                dbSmartAspects.AddInParameter(command, "@BillStatus", DbType.String, "Void");

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);

                            }
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                        }
                        else
                        {
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;
                    }
                }

            }

            if (status > 0)
                return true;
            else
                return false;
        }
        public int SaveUpdateBanquetBill(DateTime restaurantBillDate, List<GuestBillPaymentBO> guestPaymentDetailList, string deletedPaymentIds, string deletedTransferedPaymentIds, string deletedAchievementPaymentIds, int billID)
        {
            int status = 0;
            bool isWorkAny = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    status = 1;

                    if (guestPaymentDetailList != null)
                    {
                        using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                        {
                            foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
                            {
                                if (guestBillPaymentBO.PaymentMode != "Other Room")
                                {
                                    if (guestBillPaymentBO.ServiceBillId == null) // guestBillPaymentBO.PaidServiceId == 0
                                    {
                                        commandGuestBillPayment.Parameters.Clear();
                                        guestBillPaymentBO.PaymentDate = DateTime.Now;

                                        int companyId = 0;
                                        if (guestBillPaymentBO.CompanyId != null)
                                        {
                                            companyId = guestBillPaymentBO.CompanyId;
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, restaurantBillDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Banquet");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillId", DbType.Int32, billID);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, billID);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CompanyId", DbType.Int32, companyId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillNumber", DbType.String, string.Empty);

                                        if (guestBillPaymentBO.PaymentMode == "Card")
                                        {
                                            if (guestBillPaymentBO.ExpireDate != null)
                                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                                        dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));

                                        if (status > 0)
                                        {
                                            status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                            isWorkAny = true;
                                        }
                                    }
                                }

                                if (guestBillPaymentBO.PaymentMode == "Other Room" && status > 0)
                                {
                                    using (DbCommand commandGuestBillPayment2 = dbSmartAspects.GetStoredProcCommand("SaveGuestExtraServiceBillInfo_SP"))
                                    {
                                        if (guestBillPaymentBO.ServiceBillId == null)
                                        {
                                            commandGuestBillPayment.Parameters.Clear();
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment2, "@ModuleName", DbType.String, "Banquet");
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment2, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment2, "@ServiceBillId", DbType.Int32, billID);
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment2, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment2, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment2, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment2, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                                            dbSmartAspects.AddOutParameter(commandGuestBillPayment2, "@PaymentId", DbType.Int32, sizeof(Int32));

                                            if (status > 0)
                                            {
                                                status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment2);
                                                isWorkAny = true;
                                            }
                                        }
                                    }
                                }

                                //using (DbCommand commandGuestBillPaymentDesc = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantBillPaymentDescriptionInfo_SP"))
                                //{
                                //    commandGuestBillPaymentDesc.Parameters.Clear();
                                //    dbSmartAspects.AddInParameter(commandGuestBillPaymentDesc, "@BillId", DbType.Int32, billID);
                                //    dbSmartAspects.AddInParameter(commandGuestBillPaymentDesc, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);

                                //    if (status > 0)
                                //    {
                                //        status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPaymentDesc);
                                //        isWorkAny = true;
                                //    }
                                //}
                            }
                        }

                        if (!string.IsNullOrEmpty(deletedPaymentIds) && status > 0)
                        {
                            string[] paymentId = deletedPaymentIds.Split(',');

                            using (DbCommand commandDeletePayment = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                for (int i = 0; i < paymentId.Count(); i++)
                                {
                                    commandDeletePayment.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDeletePayment, "@TableName", DbType.String, "HotelGuestBillPayment");
                                    dbSmartAspects.AddInParameter(commandDeletePayment, "@TablePKField", DbType.String, "PaymentId");
                                    dbSmartAspects.AddInParameter(commandDeletePayment, "@TablePKId", DbType.String, paymentId[i]);
                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePayment, transction);
                                    isWorkAny = true;
                                }
                            }
                        }

                        //-----------------------------------Delete from Room Payment------------------------------------------
                        if (!string.IsNullOrEmpty(deletedTransferedPaymentIds) && status > 0)
                        {
                            string[] paymentId = deletedTransferedPaymentIds.Split(',');

                            using (DbCommand commandDeletePayment = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                for (int i = 0; i < paymentId.Count(); i++)
                                {
                                    commandDeletePayment.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDeletePayment, "@TableName", DbType.String, "HotelGuestExtraServiceBillApproved");
                                    dbSmartAspects.AddInParameter(commandDeletePayment, "@TablePKField", DbType.String, "ApprovedId");
                                    dbSmartAspects.AddInParameter(commandDeletePayment, "@TablePKId", DbType.String, paymentId[i]);
                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePayment, transction);
                                    isWorkAny = true;
                                }
                            }
                        }

                        //// -----------------------------------------Delete Paid Service Achievement ----------------------------------------
                        //if (!string.IsNullOrEmpty(deletedAchievementPaymentIds) && status > 0)
                        //{
                        //    string[] paymentId = deletedAchievementPaymentIds.Split(',');

                        //    using (DbCommand commandDeletePayment = dbSmartAspects.GetStoredProcCommand("UpdateHotelGuestServiceBillApprovedForNotAchievement_SP"))
                        //    {
                        //        for (int i = 0; i < paymentId.Count(); i++)
                        //        {
                        //            commandDeletePayment.Parameters.Clear();
                        //            dbSmartAspects.AddInParameter(commandDeletePayment, "@ApprovedId", DbType.Int32, Convert.ToInt32(paymentId[i]));
                        //            status = dbSmartAspects.ExecuteNonQuery(commandDeletePayment, transction);
                        //            isWorkAny = true;
                        //        }
                        //    }
                        //}
                    }

                    if (status > 0 && isWorkAny)
                    {
                        transction.Commit();
                    }
                    else
                    {
                        if (isWorkAny)
                            transction.Rollback();
                    }
                }
            }

            return status;
        }
        public Boolean UpdateBanquetBillRegistrationIdInfoForOtherRoomPayment(int billId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateBanquetBillRegistrationIdInfoForOtherRoomPayment_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, billId);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool DeleteBanquetSettlementPaymentInfoById(long reservationId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteBanquetSettlementPaymentInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int64, reservationId);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public bool BanquetBillReSettlementLog(Int64 reservationId, int userInfoId)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("BanquetBillReSettlementLog_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ReservationId", DbType.Int64, reservationId);
                            dbSmartAspects.AddInParameter(commandPercentageDiscount, "@UserInfoId", DbType.Int32, userInfoId);

                            status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }

            return retVal;
        }

        public List<BanquetBillReSettlementLogReportBO> GetBanquetBillReSettlementLogReport(int costCenterId, string billNumber, DateTime dateFrom, DateTime dateTo)
        {
            List<BanquetBillReSettlementLogReportBO> entityBOList = new List<BanquetBillReSettlementLogReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("BanquetBillReSettlementLogReport_SP"))
                {
                    if (costCenterId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(billNumber))
                        dbSmartAspects.AddInParameter(cmd, "@ReservationNumber", DbType.String, billNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReservationNumber", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.Date, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.Date, dateTo);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "KotBill");
                    DataTable Table = ds.Tables["KotBill"];

                    entityBOList = Table.AsEnumerable().Select(r => new BanquetBillReSettlementLogReportBO
                    {
                        ResettlementHistoryId = r.Field<Int64>("ResettlementHistoryId"),
                        ReservationId = r.Field<Int64>("ReservationId"),
                        ReservationNumber = r.Field<string>("ReservationNumber"),
                        ResettlementDate = r.Field<DateTime>("ResettlementDate"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        CostCenter = r.Field<string>("CostCenter"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        UserName = r.Field<string>("UserName"),
                        CalculatedDiscountAmount = r.Field<decimal>("CalculatedDiscountAmount"),
                        ItemId = r.Field<Int32>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        ItemUnit = r.Field<decimal>("ItemUnit"),
                        UnitRate = r.Field<decimal>("UnitRate"),
                        Amount = r.Field<decimal>("Amount"),
                        Remarks = r.Field<string>("Remarks")

                    }).ToList();
                }
            }

            return entityBOList;
        }

        public bool ActivateBanquetReservation(long id, long lastModifiedBy)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ActivateBanquetReservation_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int64, id);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int64, lastModifiedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
    }
}
