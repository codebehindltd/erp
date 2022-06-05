using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
    public class RoomReservationDetailsForMailDA : BaseService
    {

        public List<RoomReservationDetailsForMailBO> GetOnlineReservationDetailsInformationForMail(int ReservationNumber, string CompanyName, string CompanyAddress, string CompanyWeb)
        {
            List<RoomReservationDetailsForMailBO> detailList = new List<RoomReservationDetailsForMailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("RoomReservationBillGenerate"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationNumber", DbType.Int32, ReservationNumber);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, CompanyName);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyAddress", DbType.String, CompanyAddress);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyWeb", DbType.String, CompanyWeb);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                RoomReservationDetailsForMailBO datailBO = new RoomReservationDetailsForMailBO();
                                datailBO.CompanyName = reader["CompanyName"].ToString();
                                datailBO.CompanyAddress = reader["CompanyAddress"].ToString();
                                datailBO.WebAddress = reader["WebAddress"].ToString();
                                datailBO.GuestName = reader["GuestName"].ToString();
                                datailBO.GuestCompanyName = reader["GuestCompanyName"].ToString();
                                datailBO.GuestCompanyAddress = reader["GuestCompanyAddress"].ToString();
                                datailBO.ContactPerson = reader["ContactPerson"].ToString();
                                datailBO.ContactDesignation = reader["ContactDesignation"].ToString();
                                datailBO.TelephoneNumber = reader["TelephoneNumber"].ToString();
                                datailBO.ContactNumber = reader["ContactNumber"].ToString();
                                datailBO.FaxNumber = reader["FaxNumber"].ToString();
                                datailBO.ContactEmail = reader["ContactEmail"].ToString();
                                datailBO.ReservationNumber = reader["ReservationNumber"].ToString();
                                datailBO.ReferencePerson = reader["ReferencePerson"].ToString();
                                datailBO.ReferenceDesignation = reader["ReferenceDesignation"].ToString();
                                datailBO.ReferenceOrganization = reader["ReferenceOrganization"].ToString();
                                datailBO.ReferenceTelephone = reader["ReferenceTelephone"].ToString();
                                datailBO.ReferenceCellNumber = reader["ReferenceCellNumber"].ToString();
                                datailBO.ReferenceEmail = reader["ReferenceEmail"].ToString();
                                datailBO.MethodOfPayment = reader["MethodOfPayment"].ToString();
                                datailBO.RoomType = reader["RoomType"].ToString();
                                datailBO.ArrivalFlightName = reader["ArrivalFlightName"].ToString();
                                datailBO.ArrivalFlightNumber = reader["ArrivalFlightNumber"].ToString();
                                datailBO.CurrencyType = reader["CurrencyType"].ToString();
                                datailBO.DepartureFlightName = reader["DepartureFlightName"].ToString();
                                datailBO.DepartureFlightNumber = reader["DepartureFlightNumber"].ToString();
                                datailBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"].ToString());
                                datailBO.RoomRate = Convert.ToDecimal(reader["RoomRate"].ToString());
                                datailBO.ArrivalDate = Convert.ToDateTime(reader["ArrivalDate"].ToString());
                                if (!string.IsNullOrWhiteSpace(reader["ArrivalTime"].ToString()))
                                {
                                    datailBO.ArrivalTime = Convert.ToDateTime(reader["ArrivalTime"].ToString());
                                }
                                datailBO.DepartureDate = Convert.ToDateTime(reader["DepartureDate"].ToString());
                                if (!string.IsNullOrWhiteSpace(reader["DepartureTime"].ToString()))
                                {
                                    datailBO.DepartureTime = Convert.ToDateTime(reader["DepartureTime"].ToString());
                                }
                                datailBO.TypeWiseTotalRooms = Convert.ToDecimal(reader["TypeWiseTotalRooms"].ToString());
                                datailBO.TotalNumberOfRooms = Convert.ToDecimal(reader["TotalNumberOfRooms"].ToString());
                                datailBO.ConversionRate = Convert.ToDecimal(reader["ConversionRate"].ToString());
                                datailBO.ReservationDate = Convert.ToDateTime(reader["CreatedDate"]);
                                detailList.Add(datailBO);

                            }
                        }
                    }
                }
                return detailList;
            }
        }
    }
}
