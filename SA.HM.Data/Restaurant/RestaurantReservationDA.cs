using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using HotelManagement.Entity.Restaurant;
using System.Data;
using System.Collections;

namespace HotelManagement.Data.Restaurant
{
    public class RestaurantReservationDA : BaseService
    {
        public Boolean SaveRestaurantReservationInfo(RestaurantReservationBO tableReservation, out int tmpReservationId, List<RestaurantReservationTableDetailBO> tableAddList, out string currentReservationNumber, List<RestaurantReservationItemDetailBO> itemAddList)
        {
            bool retVal = false;
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveRestaurantReservationInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@DateIn", DbType.DateTime, tableReservation.DateIn);
                        dbSmartAspects.AddInParameter(commandMaster, "@DateOut", DbType.DateTime, tableReservation.DateOut);
                        dbSmartAspects.AddInParameter(commandMaster, "@ConfirmationDate", DbType.DateTime, tableReservation.ConfirmationDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservedCompany", DbType.String, tableReservation.ReservedCompany);
                        //dbSmartAspects.AddInParameter(commandMaster, "@GuestId", DbType.Int32, tableReservation.GuestId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ContactAddress", DbType.String, tableReservation.ContactAddress);
                        dbSmartAspects.AddInParameter(commandMaster, "@ContactPerson", DbType.String, tableReservation.ContactPerson);
                        dbSmartAspects.AddInParameter(commandMaster, "@ContactNumber", DbType.String, tableReservation.ContactNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@MobileNumber", DbType.String, tableReservation.MobileNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@FaxNumber", DbType.String, tableReservation.FaxNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@ContactEmail", DbType.String, tableReservation.ContactEmail);
                        dbSmartAspects.AddInParameter(commandMaster, "@TotalTableNumber", DbType.Int32, tableReservation.TotalTableNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservationMode", DbType.String, tableReservation.ReservationMode);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservationType", DbType.String, tableReservation.ReservationType);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservationStatus", DbType.String, tableReservation.ReservationStatus);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsListedCompany", DbType.Boolean, tableReservation.IsListedCompany);
                        dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, tableReservation.CompanyId);
                        dbSmartAspects.AddInParameter(commandMaster, "@BusinessPromotionId", DbType.Int32, tableReservation.BusinessPromotionId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReferenceId", DbType.Int32, tableReservation.ReferenceId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PaymentMode", DbType.String, tableReservation.PaymentMode);
                        dbSmartAspects.AddInParameter(commandMaster, "@PayFor", DbType.Int32, tableReservation.PayFor);
                        dbSmartAspects.AddInParameter(commandMaster, "@CurrencyType", DbType.Int32, tableReservation.CurrencyType);
                        dbSmartAspects.AddInParameter(commandMaster, "@ConversionRate", DbType.Decimal, tableReservation.ConversionRate);
                        dbSmartAspects.AddInParameter(commandMaster, "@Reason", DbType.String, tableReservation.Reason);
                        dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, tableReservation.Remarks);
                        dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, tableReservation.CreatedBy);

                        //dbSmartAspects.AddInParameter(commandMaster, "@tempResId", DbType.Int32, tableReservation.ReservationId);
                        dbSmartAspects.AddOutParameter(commandMaster, "@ReservationId", DbType.Int32, sizeof(Int32));
                        dbSmartAspects.AddOutParameter(commandMaster, "@ReservationNumber", DbType.String, 50);

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        tmpReservationId = Convert.ToInt32(commandMaster.Parameters["@ReservationId"].Value);
                        currentReservationNumber = (commandMaster.Parameters["@ReservationNumber"].Value).ToString();

                        if (status > 0)
                        {
                            int tableCount = 0;

                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveRestaurantReservationTableDetailInfo_SP"))
                            {
                                foreach (RestaurantReservationTableDetailBO reservationDetailBO in tableAddList)
                                {
                                    commandDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int64, tmpReservationId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@CostCentreId", DbType.Int32, reservationDetailBO.CostCenterId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@TableId", DbType.Int32, reservationDetailBO.TableId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@DiscountAmount", DbType.Decimal, reservationDetailBO.Amount);
                                    dbSmartAspects.AddInParameter(commandDetails, "@DiscountType", DbType.String, reservationDetailBO.DiscountType);

                                    tableCount += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                            //transction.Commit();
                            //retVal = true;
                            if (tableCount == tableAddList.Count && itemAddList != null)
                            {
                                int itemCount = 0;

                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveRestaurantReservationItemDetailInfo_SP"))
                                {
                                    foreach (RestaurantReservationItemDetailBO itemDetailBO in itemAddList)
                                    {
                                        if (itemDetailBO.ReservationId == 0)
                                        {
                                            commandDetails.Parameters.Clear();
                                            dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int64, tmpReservationId);
                                            dbSmartAspects.AddInParameter(commandDetails, "@ItemTypeId", DbType.Int32, itemDetailBO.ItemTypeId);
                                            dbSmartAspects.AddInParameter(commandDetails, "@ItemType", DbType.String, itemDetailBO.ItemType);
                                            dbSmartAspects.AddInParameter(commandDetails, "@ItemId", DbType.Int32, itemDetailBO.ItemId);
                                            dbSmartAspects.AddInParameter(commandDetails, "@ItemName", DbType.String, itemDetailBO.ItemName);
                                            dbSmartAspects.AddInParameter(commandDetails, "@ItemUnit", DbType.Decimal, itemDetailBO.ItemUnit);
                                            dbSmartAspects.AddInParameter(commandDetails, "@IsComplementary", DbType.Boolean, itemDetailBO.IsComplementary);
                                            dbSmartAspects.AddInParameter(commandDetails, "@UnitPrice", DbType.Decimal, itemDetailBO.UnitPrice);
                                            dbSmartAspects.AddInParameter(commandDetails, "@TotalPrice", DbType.Decimal, itemDetailBO.TotalPrice);
                                            itemCount += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                        }
                                    }
                                }

                                if (itemCount == itemAddList.Count)
                                {
                                    transction.Commit();
                                    retVal = true;
                                }
                            }
                        }

                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }
        public Boolean UpdateRestaurantReservationInfo(RestaurantReservationBO tableReservation, List<RestaurantReservationTableDetailBO> addTableList, List<RestaurantReservationTableDetailBO> deleteTableList, List<RestaurantReservationTableDetailBO> editTableList, List<RestaurantReservationItemDetailBO> deleteItemList, List<RestaurantReservationItemDetailBO> addItemList, out string currentReservationNumber)
        {
            bool retVal = false;
            int status = 0;
            long tmpReservationId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantReservationInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservationId", DbType.Int64, tableReservation.ReservationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@DateIn", DbType.DateTime, tableReservation.DateIn);
                        dbSmartAspects.AddInParameter(commandMaster, "@DateOut", DbType.DateTime, tableReservation.DateOut);
                        dbSmartAspects.AddInParameter(commandMaster, "@ConfirmationDate", DbType.DateTime, tableReservation.ConfirmationDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservedCompany", DbType.String, tableReservation.ReservedCompany);
                        //dbSmartAspects.AddInParameter(commandMaster, "@GuestId", DbType.Int32, tableReservation.GuestId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ContactAddress", DbType.String, tableReservation.ContactAddress);
                        dbSmartAspects.AddInParameter(commandMaster, "@ContactPerson", DbType.String, tableReservation.ContactPerson);
                        dbSmartAspects.AddInParameter(commandMaster, "@ContactNumber", DbType.String, tableReservation.ContactNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@MobileNumber", DbType.String, tableReservation.MobileNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@FaxNumber", DbType.String, tableReservation.FaxNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@ContactEmail", DbType.String, tableReservation.ContactEmail);
                        dbSmartAspects.AddInParameter(commandMaster, "@TotalTableNumber", DbType.Int32, tableReservation.TotalTableNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservationMode", DbType.String, tableReservation.ReservationMode);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservationType", DbType.String, tableReservation.ReservationType);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservationStatus", DbType.String, tableReservation.ReservationStatus);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsListedCompany", DbType.Boolean, tableReservation.IsListedCompany);
                        dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, tableReservation.CompanyId);
                        dbSmartAspects.AddInParameter(commandMaster, "@BusinessPromotionId", DbType.Int32, tableReservation.BusinessPromotionId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReferenceId", DbType.Int32, tableReservation.ReferenceId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PaymentMode", DbType.String, tableReservation.PaymentMode);
                        dbSmartAspects.AddInParameter(commandMaster, "@PayFor", DbType.Int32, tableReservation.PayFor);
                        dbSmartAspects.AddInParameter(commandMaster, "@CurrencyType", DbType.Int32, tableReservation.CurrencyType);
                        dbSmartAspects.AddInParameter(commandMaster, "@ConversionRate", DbType.Decimal, tableReservation.ConversionRate);
                        dbSmartAspects.AddInParameter(commandMaster, "@Reason", DbType.String, tableReservation.Reason);
                        dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, tableReservation.Remarks);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, tableReservation.LastModifiedBy);

                        //dbSmartAspects.AddInParameter(commandMaster, "@tempResId", DbType.Int32, tableReservation.ReservationId);
                        //dbSmartAspects.AddOutParameter(commandMaster, "@ReservationId", DbType.Int32, sizeof(Int32));
                        dbSmartAspects.AddOutParameter(commandMaster, "@ReservationNumber", DbType.String, 50);

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        tmpReservationId = tableReservation.ReservationId;
                        currentReservationNumber = (commandMaster.Parameters["@ReservationNumber"].Value).ToString();

                        if (status > 0)
                        {
                            if (addTableList.Count != 0)
                            {
                                using (DbCommand addTableDetails = dbSmartAspects.GetStoredProcCommand("SaveRestaurantReservationTableDetailInfo_SP"))
                                {
                                    foreach (RestaurantReservationTableDetailBO detailBO in addTableList)
                                    {
                                        addTableDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(addTableDetails, "@ReservationId", DbType.Int64, tmpReservationId);
                                        dbSmartAspects.AddInParameter(addTableDetails, "@CostCentreId", DbType.Int32, detailBO.CostCenterId);
                                        dbSmartAspects.AddInParameter(addTableDetails, "@TableId", DbType.Int32, detailBO.TableId);
                                        dbSmartAspects.AddInParameter(addTableDetails, "@DiscountAmount", DbType.Decimal, detailBO.Amount);
                                        dbSmartAspects.AddInParameter(addTableDetails, "@DiscountType", DbType.String, detailBO.DiscountType);

                                        status = dbSmartAspects.ExecuteNonQuery(addTableDetails, transction);
                                    }
                                }
                            }

                            if (deleteTableList.Count != 0)
                            {
                                using (DbCommand deleteTableDetail = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                {
                                    foreach (RestaurantReservationTableDetailBO detailBO in deleteTableList)
                                    {
                                        deleteTableDetail.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(deleteTableDetail, "@TableName", DbType.String, "RestaurantReservationTableDetail");
                                        dbSmartAspects.AddInParameter(deleteTableDetail, "@TablePKField", DbType.String, "TableDetailId");
                                        dbSmartAspects.AddInParameter(deleteTableDetail, "@TablePKId", DbType.String, detailBO.TableDetailId);
                                        status = dbSmartAspects.ExecuteNonQuery(deleteTableDetail);
                                    }
                                }                                
                            }

                            if (editTableList.Count != 0)
                            {
                                using (DbCommand editTableDetail = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantReservationTableDetailInfo_SP"))
                                {
                                    foreach (RestaurantReservationTableDetailBO detailBO in editTableList)
                                    {
                                        editTableDetail.Parameters.Clear();

                                        //dbSmartAspects.AddInParameter(editTableDetail, "@TableDetailId", DbType.Int64, detailBO.TableDetailId);
                                        dbSmartAspects.AddInParameter(editTableDetail, "@ReservationId", DbType.Int64, tmpReservationId);
                                        dbSmartAspects.AddInParameter(editTableDetail, "@CostCenterId", DbType.Int32, detailBO.CostCenterId);
                                        dbSmartAspects.AddInParameter(editTableDetail, "@TableId", DbType.Int32, detailBO.TableId);
                                        dbSmartAspects.AddInParameter(editTableDetail, "@DiscountType", DbType.String, detailBO.DiscountType);
                                        dbSmartAspects.AddInParameter(editTableDetail, "@Amount", DbType.Decimal, detailBO.Amount);
                                        status = dbSmartAspects.ExecuteNonQuery(editTableDetail, transction);
                                    }
                                }
                            }

                            if (addItemList.Count != 0)
                            {
                                //int count = 0;
                                using (DbCommand addItemDetails = dbSmartAspects.GetStoredProcCommand("SaveRestaurantReservationItemDetailInfo_SP"))
                                {
                                    foreach (RestaurantReservationItemDetailBO detailBO in addItemList)
                                    {
                                        addItemDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(addItemDetails, "@ReservationId", DbType.Int64, tmpReservationId);
                                        dbSmartAspects.AddInParameter(addItemDetails, "@ItemTypeId", DbType.Int32, detailBO.ItemTypeId);
                                        dbSmartAspects.AddInParameter(addItemDetails, "@ItemType", DbType.String, detailBO.ItemType);
                                        dbSmartAspects.AddInParameter(addItemDetails, "@ItemId", DbType.Int32, detailBO.ItemId);
                                        dbSmartAspects.AddInParameter(addItemDetails, "@ItemName", DbType.String, detailBO.ItemName);
                                        dbSmartAspects.AddInParameter(addItemDetails, "@ItemUnit", DbType.Decimal, detailBO.ItemUnit);
                                        dbSmartAspects.AddInParameter(addItemDetails, "@IsComplementary", DbType.Boolean, detailBO.IsComplementary);
                                        dbSmartAspects.AddInParameter(addItemDetails, "@UnitPrice", DbType.Decimal, detailBO.UnitPrice);
                                        dbSmartAspects.AddInParameter(addItemDetails, "@TotalPrice", DbType.Decimal, detailBO.TotalPrice);
                                        status = dbSmartAspects.ExecuteNonQuery(addItemDetails, transction);
                                    }
                                }
                            }
                            if (deleteItemList.Count != 0)
                            {
                                using (DbCommand deleteItemDetails = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                {
                                    foreach (RestaurantReservationItemDetailBO detailBO in deleteItemList)
                                    {
                                        dbSmartAspects.AddInParameter(deleteItemDetails, "@TableName", DbType.String, "RestaurantItemReservationDetail");
                                        dbSmartAspects.AddInParameter(deleteItemDetails, "@TablePKField", DbType.String, "ItemDetailId");
                                        dbSmartAspects.AddInParameter(deleteItemDetails, "@TablePKId", DbType.String, detailBO.ItemDetailId);
                                        status = dbSmartAspects.ExecuteNonQuery(deleteItemDetails);
                                    }
                                }

                            }
                        }
                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }

                    }

                    //retVal = true;
                }
            }
            return retVal;
        }
        //public bool UpdateBanquetReservationInfo(RestaurantTableReservationBO reservationBO, List<RestaurantTableReservationDetailBO> detailList, ArrayList arrayDelete)
        //{
        //    bool retVal = false;
        //    int status = 0;
        //    int tmpSalesId = 0;
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateBanquetReservationInfo_SP"))
        //        {
        //            conn.Open();
        //            using (DbTransaction transction = conn.BeginTransaction())
        //            {
        //                dbSmartAspects.AddInParameter(commandMaster, "@ReservationId", DbType.Int32, reservationBO.ReservationId);
        //                dbSmartAspects.AddInParameter(commandMaster, "@Name", DbType.String, reservationBO.Name);
        //                dbSmartAspects.AddInParameter(commandMaster, "@BanquetId", DbType.Int32, reservationBO.BanquetId);
        //                dbSmartAspects.AddInParameter(commandMaster, "@CityName", DbType.String, reservationBO.CityName);
        //                dbSmartAspects.AddInParameter(commandMaster, "@ZipCode", DbType.String, reservationBO.ZipCode);
        //                dbSmartAspects.AddInParameter(commandMaster, "@CountryId", DbType.Int32, reservationBO.CountryId);
        //                dbSmartAspects.AddInParameter(commandMaster, "@EmailAddress", DbType.String, reservationBO.EmailAddress);
        //                dbSmartAspects.AddInParameter(commandMaster, "@PhoneNumber", DbType.String, reservationBO.PhoneNumber);
        //                dbSmartAspects.AddInParameter(commandMaster, "@BookingFor", DbType.String, reservationBO.BookingFor);
        //                dbSmartAspects.AddInParameter(commandMaster, "@ContactPerson", DbType.String, reservationBO.ContactPerson);
        //                dbSmartAspects.AddInParameter(commandMaster, "@ContactPhone", DbType.String, reservationBO.ContactPhone);
        //                dbSmartAspects.AddInParameter(commandMaster, "@ContactEmail", DbType.String, reservationBO.ContactEmail);
        //                dbSmartAspects.AddInParameter(commandMaster, "@Address", DbType.String, reservationBO.Address);
        //                dbSmartAspects.AddInParameter(commandMaster, "@DepartureDate", DbType.DateTime, reservationBO.DepartureDate);
        //                dbSmartAspects.AddInParameter(commandMaster, "@ArriveDate", DbType.DateTime, reservationBO.ArriveDate);

        //                dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonAdult", DbType.Decimal, reservationBO.NumberOfPersonAdult);
        //                dbSmartAspects.AddInParameter(commandMaster, "@NumberOfPersonChild", DbType.Decimal, reservationBO.NumberOfPersonChild);
        //                dbSmartAspects.AddInParameter(commandMaster, "@OccessionTypeId", DbType.Int32, reservationBO.OccessionTypeId);
        //                dbSmartAspects.AddInParameter(commandMaster, "@SeatingId", DbType.Int32, reservationBO.SeatingId);
        //                dbSmartAspects.AddInParameter(commandMaster, "@RefferenceId", DbType.Int32, reservationBO.RefferenceId);

        //                dbSmartAspects.AddInParameter(commandMaster, "@TotalAmount", DbType.Decimal, reservationBO.TotalAmount);
        //                dbSmartAspects.AddInParameter(commandMaster, "@DiscountAmount", DbType.Decimal, reservationBO.DiscountAmount);
        //                dbSmartAspects.AddInParameter(commandMaster, "@VatAmount", DbType.Decimal, reservationBO.VatAmount);
        //                dbSmartAspects.AddInParameter(commandMaster, "@GrandTotal", DbType.Decimal, reservationBO.GrandTotal);
        //                dbSmartAspects.AddInParameter(commandMaster, "@IsReturnedClient", DbType.Boolean, reservationBO.IsReturnedClient);
        //                dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int64, reservationBO.LastModifiedBy);

        //                status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
        //                tmpSalesId = reservationBO.ReservationId;
        //                if (status > 0)
        //                {
        //                    int count = 0;
        //                    using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveBanquetReservationDetailInfo_SP"))
        //                    {
        //                        foreach (BanquetReservationDetailBO detailBO in detailList)
        //                        {
        //                            if (detailBO.ReservationId == 0)
        //                            {
        //                                commandDetails.Parameters.Clear();

        //                                dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int32, tmpSalesId);
        //                                dbSmartAspects.AddInParameter(commandDetails, "@ItemTypeId", DbType.String, detailBO.ItemTypeId);
        //                                dbSmartAspects.AddInParameter(commandDetails, "@ItemType", DbType.String, detailBO.ItemType);
        //                                dbSmartAspects.AddInParameter(commandDetails, "@ItemId", DbType.Int32, detailBO.ItemId);
        //                                dbSmartAspects.AddInParameter(commandDetails, "@ItemUnit", DbType.Decimal, detailBO.ItemUnit);
        //                                dbSmartAspects.AddInParameter(commandDetails, "@IsComplementary", DbType.Boolean, detailBO.IsComplementary);
        //                                dbSmartAspects.AddInParameter(commandDetails, "@UnitPrice", DbType.Decimal, detailBO.UnitPrice);
        //                                dbSmartAspects.AddInParameter(commandDetails, "@TotalPrice", DbType.Decimal, detailBO.TotalPrice);
        //                                count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
        //                            }
        //                        }
        //                    }

        //                    using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateBanquetReservationDetailsInfo_SP"))
        //                    {
        //                        foreach (BanquetReservationDetailBO detailBO in detailList)
        //                        {
        //                            if (detailBO.ReservationId != 0)
        //                            {
        //                                commandDetails.Parameters.Clear();
        //                                dbSmartAspects.AddInParameter(commandDetails, "@DetailId", DbType.Int32, detailBO.DetailId);
        //                                dbSmartAspects.AddInParameter(commandDetails, "@ItemId", DbType.Int32, detailBO.ItemId);
        //                                dbSmartAspects.AddInParameter(commandDetails, "@ItemTypeId", DbType.String, detailBO.ItemTypeId);
        //                                dbSmartAspects.AddInParameter(commandDetails, "@ItemType", DbType.String, detailBO.ItemType);
        //                                dbSmartAspects.AddInParameter(commandDetails, "@ItemUnit", DbType.Decimal, detailBO.ItemUnit);
        //                                dbSmartAspects.AddInParameter(commandDetails, "@IsComplementary", DbType.Boolean, detailBO.IsComplementary);
        //                                dbSmartAspects.AddInParameter(commandDetails, "@UnitPrice", DbType.Decimal, detailBO.UnitPrice);
        //                                dbSmartAspects.AddInParameter(commandDetails, "@TotalPrice", DbType.Decimal, detailBO.TotalPrice);
        //                                count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
        //                            }
        //                        }
        //                    }
        //                    if (count == detailList.Count)
        //                    {
        //                        if (arrayDelete != null)
        //                        {
        //                            if (arrayDelete.Count > 0)
        //                            {
        //                                foreach (int delId in arrayDelete)
        //                                {
        //                                    using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
        //                                    {
        //                                        dbSmartAspects.AddInParameter(commandEducation, "@TableName", DbType.String, "BanquetReservationDetail");
        //                                        dbSmartAspects.AddInParameter(commandEducation, "@TablePKField", DbType.String, "DetailId");
        //                                        dbSmartAspects.AddInParameter(commandEducation, "@TablePKId", DbType.String, delId);
        //                                        status = dbSmartAspects.ExecuteNonQuery(commandEducation);
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        transction.Commit();
        //                        retVal = true;
        //                    }
        //                    else
        //                    {
        //                        retVal = false;
        //                    }
        //                }
        //                else
        //                {
        //                    retVal = false;
        //                }
        //            }
        //        }
        //    }
        //    return retVal;
        //}
        public RestaurantReservationBO GetRestaurantReservationInfoById(int reservationId)
        {
            RestaurantReservationBO tableReservation = new RestaurantReservationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantReservationInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                tableReservation.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                tableReservation.ReservationNumber = reader["ReservationNumber"].ToString();
                                tableReservation.ReservationDate = Convert.ToDateTime(reader["ReservationDate"]);
                                tableReservation.DateIn = Convert.ToDateTime(reader["DateIn"]);
                                tableReservation.DateOut = Convert.ToDateTime(reader["DateOut"]);
                                tableReservation.ConfirmationDate = Convert.ToDateTime(reader["ConfirmationDate"]);
                                tableReservation.ReservedCompany = reader["ReservedCompany"].ToString();
                                //tableReservation.GuestId = Int32.Parse(reader["GuestId"].ToString());
                                tableReservation.ContactAddress = reader["ContactAddress"].ToString();
                                tableReservation.ContactPerson = reader["ContactPerson"].ToString();
                                tableReservation.ContactNumber = reader["ContactNumber"].ToString();

                                tableReservation.MobileNumber = reader["MobileNumber"].ToString();
                                tableReservation.FaxNumber = reader["FaxNumber"].ToString();

                                tableReservation.ContactEmail = reader["ContactEmail"].ToString();
                                tableReservation.TotalTableNumber = Convert.ToInt32(reader["TotalTableNumber"]);
                                tableReservation.ReservationMode = reader["ReservationMode"].ToString();
                                tableReservation.ReservationType = reader["ReservationType"].ToString();
                                tableReservation.ReservationStatus = reader["ReservationStatus"].ToString();
                                tableReservation.IsListedCompany = Convert.ToBoolean(reader["IsListedCompany"]);
                                tableReservation.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                tableReservation.BusinessPromotionId = Int32.Parse(reader["BusinessPromotionId"].ToString());
                                tableReservation.Reason = reader["Reason"].ToString();
                                tableReservation.ReferenceId = Convert.ToInt32(reader["ReferenceId"]);
                                tableReservation.PaymentMode = reader["PaymentMode"].ToString();
                                tableReservation.PayFor = Convert.ToInt32(reader["PayFor"].ToString());
                                tableReservation.CurrencyType = Convert.ToInt32(reader["CurrencyType"].ToString());
                                tableReservation.ConversionRate = Convert.ToDecimal(reader["ConversionRate"].ToString());
                                tableReservation.Remarks = reader["Remarks"].ToString();
                                //tableReservation.DiscountType = reader["DiscountType"].ToString();
                                //tableReservation.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"].ToString());
                            }
                        }
                    }
                }
            }
            return tableReservation;
        }
        public List<RestaurantReservationBO> GetRestaurantReservationInfoBySearchCriteriaForPaging(DateTime? fromDate, DateTime? toDate, string contactPerson, string reserveNo, int recordPerPage, int pageIndex, out int totalRecords)
        {
            string Where = GenarateWhereCondition(fromDate, toDate, contactPerson, reserveNo);
            List<RestaurantReservationBO> tableReservationList = new List<RestaurantReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantReservationInfoBySearchCriteriaForPagination_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, Where);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ReservationDetail");
                    DataTable Table = ds.Tables["ReservationDetail"];

                    tableReservationList = Table.AsEnumerable().Select(r => new RestaurantReservationBO
                    {
                        ReservationId = r.Field<long>("ReservationId"),
                        ReservationNumber = r.Field<string>("ReservationNumber"),
                        ReservationDate = r.Field<DateTime>("ReservationDate"),
                        DateIn = r.Field<DateTime>("DateIn"),
                        DateOut = r.Field<DateTime>("DateOut"),
                        ContactPerson = r.Field<string>("ContactPerson"),
                        ReservationMode = r.Field<string>("ReservationMode")
                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return tableReservationList;
        }
        public Boolean DeleteTableReservationDetailInfoById(int tableReservationId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteTableReservationDetailInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ReservationId", DbType.Int32, tableReservationId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public string GenarateWhereCondition(DateTime? fromDate, DateTime? toDate, string contactPerson, string reservationNumber)
        {
            string Where = string.Empty;

            if (!string.IsNullOrWhiteSpace(reservationNumber))
            {
                reservationNumber = reservationNumber.Trim();
            }
            else
            {
                reservationNumber = null;
            }
            if (!string.IsNullOrWhiteSpace(contactPerson))
            {
                contactPerson = contactPerson.Trim();
            }
            else
            {
                contactPerson = null;
            }
            if (fromDate != null && toDate != null)
            {
                if (!string.IsNullOrWhiteSpace(reservationNumber))
                {
                    if (!string.IsNullOrWhiteSpace(contactPerson))
                    {
                        Where = "(dbo.FnDate(DateIn) BETWEEN dbo.FnDate('" + fromDate + "') AND dbo.FnDate('" + toDate + "'))";
                        Where += " AND ReservationNumber ='" + reservationNumber + "'";
                        Where += " AND ContactPerson LIKE '%" + contactPerson + "%'";
                    }
                    else
                    {
                        Where = "(dbo.FnDate(DateIn) BETWEEN dbo.FnDate('" + fromDate + "') AND dbo.FnDate('" + toDate + "'))";
                        Where += " AND ReservationNumber ='" + reservationNumber + "'";
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(contactPerson))
                    {
                        Where = "(dbo.FnDate(DateIn) BETWEEN dbo.FnDate('" + fromDate + "') AND dbo.FnDate('" + toDate + "'))";
                        Where += " AND ContactPerson LIKE '%" + contactPerson + "%'";
                    }
                    else
                    {
                        Where = "(dbo.FnDate(DateIn) BETWEEN dbo.FnDate('" + fromDate + "') AND dbo.FnDate('" + toDate + "'))";
                    }
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(reservationNumber))
                {
                    if (!string.IsNullOrWhiteSpace(contactPerson))
                    {
                        Where += "ReservationNumber ='" + reservationNumber + "'";
                        Where += " AND ContactPerson LIKE '%" + contactPerson + "%'";
                    }
                    else
                    {
                        Where += "ReservationNumber ='" + reservationNumber + "'";
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(contactPerson))
                    {
                        Where += "ContactPerson LIKE '%" + contactPerson + "%'";
                    }
                    else
                    {
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(Where))
            {
                Where = " WHERE " + Where;
            }

            return Where;
        }

    }
}
