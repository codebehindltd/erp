using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Restaurant;
using System.Web.Services;
using HotelManagement.Entity.Banquet;
using HotelManagement.Data.Banquet;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmTableCalendar : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadCostCenter();
            }
        }
        private void LoadCostCenter()
        {
            CostCentreTabDA entityDA = new CostCentreTabDA();
            this.ddlCostCenter.DataSource = entityDA.GetAllRestaurantTypeCostCentreTabInfo();
            this.ddlCostCenter.DataTextField = "CostCenter";
            this.ddlCostCenter.DataValueField = "CostCenterId";
            this.ddlCostCenter.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCostCenter.Items.Insert(0, item);
        }
        protected void btnViewCalender_Click(object sender, EventArgs e)
        {
            DateTime reserveDate = DateTime.Now;
            int costCenterId = Convert.ToInt32(ddlCostCenter.SelectedValue);
            List<RestaurantTableBO> restaurantTableList = new List<RestaurantTableBO>();
            RestaurantTableDA restaurantTableDA = new RestaurantTableDA();
            restaurantTableList = restaurantTableDA.GetTableInfoByCostCentre(costCenterId);

            if (ddlCostCenter.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Cost Center.", AlertType.Warning);
                return;
            }
            if (!string.IsNullOrWhiteSpace(txtCurrentDate.Text))
            {
                //reserveDate = Convert.ToDateTime(txtCurrentDate.Text);
                reserveDate = CommonHelper.DateTimeToMMDDYYYY(txtCurrentDate.Text);
            }
            else {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Start Date.", AlertType.Warning);
                txtCurrentDate.Focus();
                return;
            }

            List<TableReservationForCalendarViewBO> tableReservationList = new List<TableReservationForCalendarViewBO>();
            tableReservationList = restaurantTableDA.GetAllReservedTableInfoForCalendar(reserveDate, costCenterId);

            this.GenerateHTMLGuestGridView(restaurantTableList, tableReservationList);
        }
        private void GenerateHTMLGuestGridView(List<RestaurantTableBO> restaurantTableList, List<TableReservationForCalendarViewBO> tableReservationList)
        {
            DateTime date = DateTime.Now;
            int starthour = date.Hour;
            int endhour = 24;
            if (this.ddlDuration.SelectedIndex > 0)
            {
                starthour = 0;
            }
            if (!string.IsNullOrWhiteSpace(this.txtCurrentDate.Text))
            {
                //DateTime reserveDate = Convert.ToDateTime(this.txtCurrentDate.Text);
                DateTime reserveDate = CommonHelper.DateTimeToMMDDYYYY(txtCurrentDate.Text);
                if (reserveDate > DateTime.Now)
                    starthour = 0;
            }

            string strTable = "";
            int allocatedTableId = 0, hour = 0, entranceHour = 0, departureHour = 0;

            strTable += "<table cellspacing='0'border='1' cellpadding='4' id='TableWiseItemInformation'>";
            strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<td color='white' bgcolor='#457EA4' align='left' style='width: 150px;cursor:pointer' ><font color='white'><b>Table No.</b></font> </td>";
            //strTable += "<td bgcolor='#457EA4' align='left' style='width: 100px;cursor:pointer' ><font color='white'><b>Capacity</b></font></td>";

            for (hour = starthour; hour < endhour; hour++)
            {
                if (hour == 0)
                    strTable += "<td bgcolor='#457EA4' align='left' style='width: 100px;cursor:pointer' ><font color='white'><b>" + '1' + '2' + ' ' + 'A' + 'M' + "</b></font></td>";
                else if (hour <= 11)
                    strTable += "<td bgcolor='#457EA4' align='left' style='width: 100px;cursor:pointer' ><font color='white'><b>" + hour + ' ' + 'A' + 'M' + "</b></font></td>";
                else if (hour == 12)
                    strTable += "<td bgcolor='#457EA4' align='left' style='width: 100px;cursor:pointer' ><font color='white'><b>" + '1' + '2' + ' ' + 'P' + 'M' + "</b></font></td>";
                else
                {
                    int hourpm = hour % 12;
                    strTable += "<td bgcolor='#457EA4' align='left' style='width: 100px;cursor:pointer' ><font color='white'><b>" + hourpm + ' ' + 'P' + 'M' + "</b></font></td>";
                }
            }

            strTable += "</tr>";

            foreach (RestaurantTableBO br in restaurantTableList)
            {
                strTable += "<tr style='background-color:#e3eaeb;'>";

                strTable += "<td align='left' style='width: 150px;cursor:pointer' >" + br.TableNumber + "</td>";
                //strtable += "<td align='left' style='width: 100px;cursor:pointer'>" + br.capacity + "</td>";

                allocatedTableId = 0;
                entranceHour = 0;
                departureHour = 0;

                var table = tableReservationList.Where(b => b.TableId == br.TableId).FirstOrDefault();

                if (table != null)
                {
                    allocatedTableId = table.TableId;
                    entranceHour = table.ArriveHour;
                    departureHour = table.DepartHour;
                }

                for (hour = starthour; hour < endhour; hour++)
                {
                    if (table != null)
                    {
                        if (hour >= entranceHour && hour < departureHour)
                        {
                            strTable += "<td class='Reservation' align='left' style='width: 100px;cursor:pointer' onClick='javascript:return RedirectToDetails(" + table.ReservationId + ")'>"+table.ContactPerson+"</td>";
                        }
                        else
                            strTable += "<td align='left' style='width: 100px;cursor:pointer'></td>";
                    }
                    else
                    {
                        strTable += "<td align='left' style='width: 100px;cursor:pointer'></td>";
                    }
                }
                strTable += "</tr>";
            }
            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
            }

            this.ltlCalenderControl.InnerHtml = strTable;
        }
        public static string GetReservationInformationView(RestaurantReservationBO reservationBO)
        {
            HMUtility hmUtility = new HMUtility();
            string strTable = "";
            strTable += "<table cellspacing='0' width='100%' cellpadding='4' id='TableReservationInformation'>";
            strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Reservation Number:</td>";
            strTable += "<td align='left' style='width: 25%'>: " + reservationBO.ReservationNumber + "</td>";
            strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            //strTable += "<td align='left' style='width: 25%;font-weight:bold'>Reservation Date</td>";
            //strTable += "<td align='left' style='width: 25%'>: " + reservationBO.ContactPerson + "</td>";
            strTable += "</tr>";

            strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Contact Person:</td>";
            strTable += "<td align='left' style='width: 25%'>: " + reservationBO.ContactPerson + "</td>";
            strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            //strTable += "<td align='left' style='width: 25%;font-weight:bold'>Contact Number:</td>";
            //strTable += "<td align='left' style='width: 25%'>: " + reservationBO.RefferenceId + "</td>";
            strTable += "</tr>";

            //strTable += "<tr style='background-color:#E3EAEB;'>";
            //strTable += "<td align='left' style='width: 25%;font-weight:bold'>Check In:</td>";
            //strTable += "<td align='left' style='width: 25%'>: " + hmUtility.GetStringFromDateTime(reservationBO.DateIn) + "</td>";
            //strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            //strTable += "<td align='left' style='width: 25%;font-weight:bold'>Check Out:</td>";
            //strTable += "<td align='left' style='width: 25%'>: " + hmUtility.GetStringFromDateTime(reservationBO.DateOut) + "</td>";
            //strTable += "</tr>";

            //strTable += "<tr style='background-color:#E3EAEB;'>";
            //strTable += "<td align='left' style='width: 25%;font-weight:bold'>Company</td>";
            //strTable += "<td align='left' style='width: 25%'>: " + reservationBO.CompanyName + "</td>";
            //strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            //strTable += "<td align='left' style='width: 25%;font-weight:bold'></td>";
            //strTable += "<td align='left' style='width: 25%'></td>";
            //strTable += "</tr>";

            strTable += "</table>";
            return strTable;
        }
        [WebMethod]
        public static string GetReservationInformationByReservationId(int ReservationId)
        {
            RestaurantReservationBO reservationBO = new RestaurantReservationBO();
            RestaurantReservationDA reservationDA = new RestaurantReservationDA();
            reservationBO = reservationDA.GetRestaurantReservationInfoById(ReservationId);
            return GetReservationInformationView(reservationBO);
        }
    }
}