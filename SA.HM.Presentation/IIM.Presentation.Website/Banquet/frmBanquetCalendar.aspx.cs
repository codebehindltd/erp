using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.Banquet;
using HotelManagement.Data.Banquet;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.Banquet
{
    public partial class frmBanquetCalendar : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
        }

        [WebMethod]
        public static BanquetReservationBO GetReservationInfoById(int ReservationId)
        {
            BanquetReservationBO reservationBO = new BanquetReservationBO();
            BanquetReservationDA reservationDA = new BanquetReservationDA();
            reservationBO = reservationDA.GetBanquetReservationInfoById(ReservationId);
            return reservationBO;
        }
        protected void btnViewCalender_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtCurrentDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Start Date.", AlertType.Warning);
                return;
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            BanquetInformationBO banquetBO = new BanquetInformationBO();
            BanquetInformationDA banquetDA = new BanquetInformationDA();
            BanquetReservationDA banquetreservationDA = new BanquetReservationDA();
            var List = banquetDA.GetBanquetInfoForCalender();

            //DateTime reserveDate = Convert.ToDateTime(this.txtCurrentDate.Text);
            DateTime reserveDate = hmUtility.GetDateTimeFromString(this.txtCurrentDate.Text, userInformationBO.ServerDateFormat);
            List<BanquetReservationForCalendarViewBO> banquetViewBO = new List<BanquetReservationForCalendarViewBO>();
            banquetViewBO = banquetreservationDA.GetAllReservedBanquetInfoForCalendar(reserveDate);

            this.GenerateHTMLGuestGridView(List, banquetViewBO);
        }
        private void GenerateHTMLGuestGridView(List<BanquetInformationBO> allBanquetList, List<BanquetReservationForCalendarViewBO> reservedBanquetList)
        {
            DateTime date = DateTime.Today;
            int starthour = date.Hour;
            int endhour = 24;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (this.ddlDuration.SelectedIndex > 0)
            {
                starthour = 0;
            }
            if (!string.IsNullOrWhiteSpace(this.txtCurrentDate.Text))
            {
                DateTime reserveDate = hmUtility.GetDateTimeFromString(this.txtCurrentDate.Text, userInformationBO.ServerDateFormat);
                if (reserveDate > DateTime.Now)
                    starthour = 0;
            }

            string strTable = "";
            int hour = 0, entranceHour = 0, stayDuration = 0;

            strTable += "<table border='1' style='width:100%;' id='TableWiseItemInformation' class='table table-bordered table-condensed table-responsive'>";
            strTable += "<thead><tr style='background-color:#E3EAEB;'>";
            strTable += "<th color='white' bgcolor='#457EA4' align='left' style='width:15%;cursor:pointer' ><font color='white'><b>Hall Name</b></font> </th>";
            strTable += "<th bgcolor='#457EA4' align='left' style='width: 10%;cursor:pointer' ><font color='white'><b>Capacity</b></font></th>";

            for (hour = starthour; hour < endhour; hour++)
            {
                DateTime dynamicHour = DateTime.Now.Date.AddHours(hour);
                strTable += "<th bgcolor='#457EA4' align='left' style='width:20%;cursor:pointer' ><font color='white'><b>" + dynamicHour.ToString(userInformationBO.TimeFormat) + "</b></font></th>";
            }

            strTable += "</tr></thead><tbody>";

            foreach (BanquetInformationBO br in allBanquetList)
            {
                strTable += "<tr style='background-color:#E3EAEB;'>";

                strTable += "<td align='left' style='width:20%;cursor:pointer' >" + br.Name + "</td>";
                strTable += "<td align='left' style='width:20%;cursor:pointer'>" + br.Capacity + "</td>";

                entranceHour = 0;

                var ban = reservedBanquetList.Where(b => b.BanquetId == br.Id).ToList();
                                
                for (hour = starthour; hour < endhour; hour++)
                {
                    if (ban != null)
                    {
                        var bandits = ban.Where(b => b.ArriveHour == hour).FirstOrDefault();

                        if (bandits != null)
                        {
                            stayDuration = bandits.DepartureHour - bandits.ArriveHour;

                            for (entranceHour = 0; entranceHour < stayDuration; entranceHour++)
                            {
                                if (bandits.IsBillSettlement)
                                {
                                    strTable += "<td class='ReservationSettled' align='left' style='width:20%;;cursor:pointer' onClick='javascript:return RedirectToDetails(" + bandits.ReservationId + ")'>" + bandits.GuestName + "</td>";
                                }
                                else
                                {
                                    strTable += "<td class='Reservation' align='left' style='width:20%;cursor:pointer' onClick='javascript:return RedirectToDetails(" + bandits.ReservationId + ")'>" + bandits.GuestName + "</td>";
                                }
                            }

                            hour += (entranceHour - 1);
                            entranceHour = 0;
                        }
                        else
                        {
                            strTable += "<td align='left' style='width:8%;cursor:pointer'></td>";
                        }
                    }
                    else
                    {
                        strTable += "<td align='left' style='width:8%;cursor:pointer'></td>";
                    }
                }
                strTable += "</tr>";
            }
            strTable += "</tbody></table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
            }

            this.ltlCalenderControl.InnerHtml = strTable;
        }
        //private void GenerateHTMLGuestGridView(List<BanquetInformationBO> allBanquetList, List<BanquetReservationForCalendarViewBO> reservedBanquetList)
        //{
        //    DateTime date = DateTime.Today;
        //    int starthour = date.Hour;
        //    int endhour = 24;
        //    UserInformationBO userInformationBO = new UserInformationBO();
        //    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
        //    if (this.ddlDuration.SelectedIndex > 0)
        //    {
        //        starthour = 0;
        //    }
        //    if (!string.IsNullOrWhiteSpace(this.txtCurrentDate.Text))
        //    {
        //        DateTime reserveDate = hmUtility.GetDateTimeFromString(this.txtCurrentDate.Text, userInformationBO.ServerDateFormat);
        //        if (reserveDate > DateTime.Now)
        //            starthour = 0;
        //    }

        //    string strTable = "";
        //    int allocatedBanquetId = 0, hour = 0, entranceHour = 0, departureHpur = 0;

        //    strTable += "<table cellspacing='0'border='1' cellpadding='4' id='TableWiseItemInformation'>";
        //    strTable += "<tr style='background-color:#E3EAEB;'>";
        //    strTable += "<td color='white' bgcolor='#457EA4' align='left' style='width: 500px;cursor:pointer' ><font color='white'><b>Banquet</b></font> </td>";
        //    strTable += "<td bgcolor='#457EA4' align='left' style='width: 100px;cursor:pointer' ><font color='white'><b>Capacity</b></font></td>";

        //    for (hour = starthour; hour < endhour; hour++)
        //    {
        //        if (hour == 0)
        //            strTable += "<td bgcolor='#457EA4' align='left' style='width: 100px;cursor:pointer' ><font color='white'><b>" + '1' + '2' + ' ' + 'A' + 'M' + "</b></font></td>";
        //        else if (hour <= 11)
        //            strTable += "<td bgcolor='#457EA4' align='left' style='width: 100px;cursor:pointer' ><font color='white'><b>" + hour + ' ' + 'A' + 'M' + "</b></font></td>";
        //        else if (hour == 12)
        //            strTable += "<td bgcolor='#457EA4' align='left' style='width: 100px;cursor:pointer' ><font color='white'><b>" + '1' + '2' + ' ' + 'P' + 'M' + "</b></font></td>";
        //        else
        //        {
        //            int hourpm = hour % 12;
        //            strTable += "<td bgcolor='#457EA4' align='left' style='width: 100px;cursor:pointer' ><font color='white'><b>" + hourpm + ' ' + 'P' + 'M' + "</b></font></td>";
        //        }
        //    }

        //    strTable += "</tr>";

        //    foreach (BanquetInformationBO br in allBanquetList)
        //    {
        //        strTable += "<tr style='background-color:#E3EAEB;'>";

        //        strTable += "<td align='left' style='width: 500px;cursor:pointer' >" + br.Name + "</td>";
        //        strTable += "<td align='left' style='width: 100px;cursor:pointer'>" + br.Capacity + "</td>";

        //        allocatedBanquetId = 0;
        //        entranceHour = 0;
        //        departureHpur = 0;

        //        var ban = reservedBanquetList.Where(b => b.BanquetId == br.BanquetId).FirstOrDefault();

        //        if (ban != null)
        //        {
        //            allocatedBanquetId = ban.BanquetId;
        //            entranceHour = ban.ArriveHour;
        //            departureHpur = ban.DepartureHour;
        //        }

        //        for (hour = starthour; hour < endhour; hour++)
        //        {                                 
        //            if (ban != null)
        //            {                        
        //                if (hour >= entranceHour && hour < departureHpur)
        //                {
        //                    if (ban.IsBillSettlement)
        //                    {
        //                        strTable += "<td class='ReservationSettled' align='left' style='width: 100px;cursor:pointer' onClick='javascript:return RedirectToDetails(" + ban.ReservationId + ")'>" + ban.GuestName + "</td>";
        //                    }
        //                    else
        //                    {
        //                        strTable += "<td class='Reservation' align='left' style='width: 100px;cursor:pointer' onClick='javascript:return RedirectToDetails(" + ban.ReservationId + ")'>" + ban.GuestName + "</td>";
        //                    }
        //                }
        //                else
        //                    strTable += "<td align='left' style='width: 100px;cursor:pointer'></td>";                        
        //            }
        //            else
        //            {
        //                strTable += "<td align='left' style='width: 100px;cursor:pointer'></td>";
        //            }                    
        //        }
        //        strTable += "</tr>";
        //    }
        //    strTable += "</table>";
        //    if (strTable == "")
        //    {
        //        strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
        //    }

        //    this.ltlCalenderControl.InnerHtml = strTable;
        //}
        public static string GetReservationInformationView(BanquetReservationBO reservationBO)
        {
            HMUtility hmUtility = new HMUtility();
            
            string strTable = "";
            strTable += "<table width='100%' id='TableReservationInformation' class='table table-bordered table-condensed table-responsive'>";
            //strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<tr>";
            strTable += "<td align='left' style='width: 20%;font-weight:bold'>Reservation Number</td>";
            strTable += "<td align='left' style='width: 35%'> " + reservationBO.ReservationNumber + "</td>";
            //strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            if (reservationBO.EventType != "Internal")
            {
                strTable += "<td align='left' style='width: 20%;font-weight:bold'>Guest Name</td>";
                strTable += "<td align='left' style='width: 35%'> " + reservationBO.Name + "</td>";
            }
            
            strTable += "</tr>";

            //strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<tr>";
            strTable += "<td align='left' style='width: 20%;font-weight:bold'>Contact Person</td>";
            strTable += "<td align='left' style='width: 35%'> " + reservationBO.ContactPerson + "</td>";
            //strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 20%;font-weight:bold'>Contact Number</td>";
            strTable += "<td align='left' style='width: 35%'> " + reservationBO.ContactPhone + "</td>";
            strTable += "</tr>";


            strTable += "<tr>";
            strTable += "<td align='left' style='width: 20%;font-weight:bold'>Phone Number</td>";
            strTable += "<td align='left' style='width: 35%'> " + reservationBO.PhoneNumber + "</td>";
            //strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 20%;font-weight:bold'>Contact Email</td>";
            strTable += "<td align='left' style='width: 35%'> " + reservationBO.ContactEmail + "</td>";
            strTable += "</tr>";

            //strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<tr>";
            strTable += "<td align='left' style='width: 20%;font-weight:bold'>Number Of Adult</td>";
            strTable += "<td align='left' style='width: 35%'> " + reservationBO.NumberOfPersonAdult + "</td>";
            //strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 20%;font-weight:bold'>Number Of Child</td>";
            strTable += "<td align='left' style='width: 35%'> " + reservationBO.NumberOfPersonChild + "</td>";
            strTable += "</tr>";

            //strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<tr>";
            strTable += "<td align='left' style='width: 20%;font-weight:bold'>Remarks</td>";
            strTable += "<td colspan='3'>" + reservationBO.Remarks + "</td>";
            //////strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            ////strTable += "<td align='left' style='width: 20%;font-weight:bold'>Number Of Child</td>";
            //strTable += "<td align='left' style='width: 70%'> " + reservationBO.Remarks + "</td>";
            strTable += "</tr>";

            strTable += "</table>";
            return strTable;
        }

        [WebMethod]
        public static string GetReservationInformationByReservationId(int ReservationId)
        {
            BanquetReservationBO reservationBO = new BanquetReservationBO();
            BanquetReservationDA reservationDA = new BanquetReservationDA();
            reservationBO = reservationDA.GetBanquetReservationInfoById(ReservationId);
            return GetReservationInformationView(reservationBO);
        }
    }
}