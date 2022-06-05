using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using HotelManagement.Entity.Restaurant;
using System.Data;

namespace HotelManagement.Data.Restaurant
{
    public class RestaurantTableReservationDA : BaseService
    {
        public Boolean SaveTableReservationInfo(RestaurantTableReservationBO tableReservation, out int tmpReservationId, List<RestaurantTableReservationDetailBO> detailBO, out string currentReservationNumber)
        {
            bool retVal = false;
            int status = 0;            
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveRestaurantTableReservationInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@DateIn", DbType.DateTime, tableReservation.DateIn);
                        dbSmartAspects.AddInParameter(commandMaster, "@DateOut", DbType.DateTime, tableReservation.DateOut);
                        dbSmartAspects.AddInParameter(commandMaster, "@ConfirmationDate", DbType.DateTime, tableReservation.ConfirmationDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservedCompany", DbType.String, tableReservation.ReservedCompany);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestId", DbType.Int32, tableReservation.GuestId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ContactAddress", DbType.String, tableReservation.ContactAddress);
                        dbSmartAspects.AddInParameter(commandMaster, "@ContactPerson", DbType.String, tableReservation.ContactPerson);
                        dbSmartAspects.AddInParameter(commandMaster, "@ContactNumber", DbType.String, tableReservation.ContactNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@MobileNumber", DbType.String, tableReservation.MobileNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@FaxNumber", DbType.String, tableReservation.FaxNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@ContactEmail", DbType.String, tableReservation.ContactEmail);
                        dbSmartAspects.AddInParameter(commandMaster, "@TotalTableNumber", DbType.Int32, tableReservation.TotalTableNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservedMode", DbType.String, tableReservation.ReservedMode);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservationType", DbType.String, tableReservation.ReservationType);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservationMode", DbType.String, tableReservation.ReservationMode);
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

                        dbSmartAspects.AddInParameter(commandMaster, "@tempResId", DbType.Int32, tableReservation.ReservationId);
                        dbSmartAspects.AddOutParameter(commandMaster, "@ReservationId", DbType.Int32, sizeof(Int32));
                        dbSmartAspects.AddOutParameter(commandMaster, "@ReservationNumber", DbType.String, 50);

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        tmpReservationId = Convert.ToInt32(commandMaster.Parameters["@ReservationId"].Value);
                        currentReservationNumber = (commandMaster.Parameters["@ReservationNumber"].Value).ToString();

                        if (status > 0)
                        {
                            int count = 0;

                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveRestaurantTableReservationDetailInfo_SP"))
                            {
                                foreach (RestaurantTableReservationDetailBO reservationDetailBO in detailBO)
                                {
                                    commandDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDetails, "@ReservationId", DbType.Int32, tmpReservationId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@CostCentreId", DbType.Int32, reservationDetailBO.CostCenterId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@TableId", DbType.Int32, reservationDetailBO.TableId);                                    
                                    dbSmartAspects.AddInParameter(commandDetails, "@Amount", DbType.Decimal, reservationDetailBO.Amount);
                                    dbSmartAspects.AddInParameter(commandDetails, "@DiscountType", DbType.String, reservationDetailBO.DiscountType);

                                    count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                            transction.Commit();
                            retVal = true;
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
        public RestaurantTableReservationBO GetTableReservationInfoById(int reservationId)
        {
            RestaurantTableReservationBO tableReservation = new RestaurantTableReservationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantTableReservationInfoById_SP"))
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
                                //tableReservation.ReservationDate = Convert.ToDateTime(reader["ReservationDate"]);
                                tableReservation.DateIn = Convert.ToDateTime(reader["DateIn"]);
                                tableReservation.DateOut = Convert.ToDateTime(reader["DateOut"]);
                                tableReservation.ConfirmationDate = Convert.ToDateTime(reader["ConfirmationDate"]);
                                tableReservation.ReservedCompany = reader["ReservedCompany"].ToString();
                                tableReservation.GuestId = Int32.Parse(reader["GuestId"].ToString());
                                tableReservation.ContactAddress = reader["ContactAddress"].ToString();
                                tableReservation.ContactPerson = reader["ContactPerson"].ToString();
                                tableReservation.ContactNumber = reader["ContactNumber"].ToString();

                                tableReservation.MobileNumber = reader["MobileNumber"].ToString();
                                tableReservation.FaxNumber = reader["FaxNumber"].ToString();

                                tableReservation.ContactEmail = reader["ContactEmail"].ToString();
                                tableReservation.TotalTableNumber = Convert.ToInt32(reader["TotalTableNumber"]);
                                tableReservation.ReservedMode = reader["ReservedMode"].ToString();
                                tableReservation.ReservationType = reader["ReservationType"].ToString();
                                tableReservation.ReservationMode = reader["ReservationMode"].ToString();
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
        public List<RestaurantTableReservationBO> GetTableReservationInformationBySearchCriteriaForPaging(DateTime fromDate, DateTime toDate, string guestName, string reserveNo, int recordPerPage, int pageIndex, out int totalRecords)
        {
            string Where = GenarateWhereCondition(fromDate, toDate, guestName, reserveNo);
            List<RestaurantTableReservationBO> roomreservationList = new List<RestaurantTableReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTableReservationInfoBySearchCriteriaForpagination_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, Where);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantTableReservationBO roomreservationDetail = new RestaurantTableReservationBO();

                                roomreservationDetail.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                //roomreservationDetail.GuestName = reader["GuestName"].ToString();
                                roomreservationDetail.ReservationNumber = reader["ReservationNumber"].ToString();
                                //string reservedate = Convert.ToDateTime(reader["ReservationDate"].ToString()).ToString("dd-MMM-yyyy");
                                //roomreservationDetail.ReservationDate = Convert.ToDateTime(reservedate);
                                //string dateIn = Convert.ToDateTime(reader["DateIn"].ToString()).ToString("dd-MMM-yyyy");
                                //roomreservationDetail.DateIn = Convert.ToDateTime(dateIn);
                                //string dateOut = Convert.ToDateTime(reader["DateOut"].ToString()).ToString("dd-MMM-yyyy");
                                //roomreservationDetail.DateOut = Convert.ToDateTime(dateOut);
                                // roomreservationDetail.DateOut = Convert.ToDateTime(reader["DateOut"].ToString()).ToString("dd-MMM-yyyy");
                                roomreservationDetail.ReservationNumber = roomreservationDetail.ReservationNumber.ToString();
                                roomreservationDetail.ReservationMode = reader["ReservationMode"].ToString();

                                roomreservationList.Add(roomreservationDetail);
                            }
                        }
                    }

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return roomreservationList;
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
        public string GenarateWhereCondition(DateTime FromDate, DateTime ToDate, string reservationNumber, string reservationGuest)
        {
            string Where = string.Empty;

            reservationNumber = reservationNumber.Trim();
            reservationGuest = reservationGuest.Trim();
            if (!string.IsNullOrEmpty(reservationNumber))
            {
                Where = "(dbo.FnDate(DateIn) BETWEEN dbo.FnDate('" + FromDate + "') AND dbo.FnDate('" + ToDate + "') )";
                if (!string.IsNullOrEmpty(reservationNumber))
                {
                    if (!string.IsNullOrEmpty(Where))
                    {
                        Where += " AND ReservationNumber LIKE '%" + reservationNumber + "%'";
                    }
                }
                //Where += " ORDER BY ReservationId DESC";
            }
            else
            {
                if (!string.IsNullOrEmpty(reservationGuest))
                {

                    Where = "(dbo.FnDate(DateIn) BETWEEN dbo.FnDate('" + FromDate + "') AND dbo.FnDate('" + ToDate + "') )";
                    if (!string.IsNullOrEmpty(reservationGuest))
                    {
                        if (!string.IsNullOrEmpty(Where))
                        {
                            Where += " AND dbo.FnGuestInformationListWithCommaSeperator(ReservationId, 'Reservation') LIKE '%" + reservationGuest + "%'";
                        }
                    }
                    //Where += " ORDER BY ReservationId DESC";
                }
                else
                {
                    Where = "dbo.FnDate(DateIn) BETWEEN dbo.FnDate('" + FromDate + "') AND dbo.FnDate('" + ToDate + "')";
                    if (!string.IsNullOrEmpty(reservationNumber))
                    {
                        if (!string.IsNullOrEmpty(Where))
                        {
                            Where += " OR  ReservationNumber LIKE '%" + reservationGuest + "%'";
                            Where += " OR  dbo.FnGetFirstGuestNameForReservation(ReservationId) LIKE '%" + reservationGuest + "%'";
                        }
                    }
                    //Where += " ORDER BY ReservationId DESC";
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
