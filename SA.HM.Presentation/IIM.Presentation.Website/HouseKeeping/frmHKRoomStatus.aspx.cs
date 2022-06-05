using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.HouseKeeping;
using HotelManagement.Data.HouseKeeping;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Entity;
using HotelManagement.Data;
using HotelManagement.Entity.HMCommon;
using System.Drawing;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.UserInformation;
using System.Data;

namespace HotelManagement.Presentation.Website.HouseKeeping
{
    public partial class frmHKRoomStatus : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        protected int IsSuccess = -1;
        HMUtility hmUtility = new HMUtility();
        List<RoomNumberBO> roomNumberListBO;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            
            if (!IsPostBack)
            {
                LoadCurrentDate();
                LoadFORoomStatus();
                LoadHKRoomStatus();
                MakeOutofOrderRoomVacant();
                LoadFloor();
                LoadFloorBlock();
                LoadStatusForAll();
            }
        }
        protected void gvRoomStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvRoomStatus.PageIndex = e.NewPageIndex;
        }
        protected void gvRoomStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblRoomId = (Label)e.Row.FindControl("lblRoomId");
                ImageButton btnPreference = (ImageButton)e.Row.FindControl("btnPreference");
                btnPreference.Attributes["onclick"] = "javascript:return GetGuestIdFMRoomId('" + lblRoomId.Text + "');";

                Label lblLasCleanDate = (Label)e.Row.FindControl("lblLastCleanDate");

                ImageButton btnDetails = (ImageButton)e.Row.FindControl("btnDetails");
                btnDetails.Attributes["onclick"] = "javascript:return GetReservationIdFMRoomId('" + lblRoomId.Text + "');";

                DropDownList ddlHKRoomStatus = (DropDownList)e.Row.FindControl("ddlHKRoomStatus");
                Label lblFORoomStatus = (Label)e.Row.FindControl("lblFORoomStatus");
                Label lblReservationStatus = (Label)e.Row.FindControl("lblReservationStatus");


                HKRoomStatusDA HKRoomStatusDA = new HKRoomStatusDA();
                List<HKRoomStatusBO> files = HKRoomStatusDA.GetHKRoomStatusType().Where(x => x.HKRoomStatusId != 1 && x.HKRoomStatusId != 2).ToList();

                ddlHKRoomStatus.DataSource = files;
                ddlHKRoomStatus.DataTextField = "StatusName";
                ddlHKRoomStatus.DataValueField = "HKRoomStatusId";
                ddlHKRoomStatus.DataBind();

                int rowIndex = e.Row.RowIndex;
                //string fromDate = (e.Row.FindControl("lblOutofOrderDate") as Label).Text;
                //string fromDate = (e.Row.FindControl("hfTxtFromDate") as HiddenField).Value;
                //string fromTime = (e.Row.FindControl("hfTxtFromTime") as HiddenField).Value;
                //string toDate = (e.Row.FindControl("hfTxtToDate") as HiddenField).Value;
                //string toTime = (e.Row.FindControl("hfTxtToTime") as HiddenField).Value;

                string reason = (e.Row.FindControl("lblOutoforderReason") as Label).Text;

                ddlHKRoomStatus.Attributes["onclick"] = "javascript:return ShowHideDetails('" + rowIndex  + "', '" + reason + "');";

                string hkRoomStatus = (e.Row.FindControl("lblHKRoomStatus") as Label).Text;
                //if (!string.IsNullOrWhiteSpace(hfHouseKeepingMorningDirtyHour.Value))
                //{
                //    UserInformationBO userInformationBO = new UserInformationBO();
                //    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                //    //DateTime lastCleanDate = hmUtility.GetDateTimeFromString(lblLasCleanDate.Text, userInformationBO.ServerDateFormat);
                //    DateTime lastCleanDate = Convert.ToDateTime(lblLasCleanDate.Text);

                //    if (lastCleanDate.Date != DateTime.Now.Date)
                //    {
                //        string fTime = hfHouseKeepingMorningDirtyHour.Value.Split('~')[0].ToString();
                //        string tTime = hfHouseKeepingMorningDirtyHour.Value.Split('~')[1].ToString();
                //        int fromTime = !string.IsNullOrWhiteSpace(fTime) ? Convert.ToInt32(fTime) : 6;
                //        int toTime = !string.IsNullOrWhiteSpace(tTime) ? Convert.ToInt32(tTime) : 12;
                //        int currentHour = !string.IsNullOrWhiteSpace(DateTime.Now.ToString("HH")) ? Convert.ToInt32(DateTime.Now.ToString("HH")) : 0;
                //        if (currentHour >= fromTime && currentHour <= toTime)
                //        {
                //            hkRoomStatus = "Dirty";
                //        }
                //    }
                //}

                if (!string.IsNullOrEmpty(hkRoomStatus))
                {
                    if (hkRoomStatus == "Vacant")
                    {
                        ddlHKRoomStatus.Items.FindByText("Clean").Selected = true;
                    }
                    else if (hkRoomStatus == "Occupied")
                    {
                        ddlHKRoomStatus.Items.FindByText("Dirty").Selected = true;
                    }
                    else
                    {
                        ddlHKRoomStatus.Items.FindByText(hkRoomStatus).Selected = true;
                    }
                }

                if (lblFORoomStatus.Text == "Occupied")
                {
                    btnPreference.Visible = true;
                    ddlHKRoomStatus.Items.Remove(ddlHKRoomStatus.Items.FindByText("Out of Order"));
                    ddlHKRoomStatus.Items.Remove(ddlHKRoomStatus.Items.FindByText("Out of Service"));
                }
                else
                {
                    btnPreference.Visible = false;
                }
                if (lblReservationStatus.Text == "Exp. Arrival")
                {
                    //btnDetails.Visible = true;
                    btnDetails.Visible = false;
                    ddlHKRoomStatus.Items.Remove(ddlHKRoomStatus.Items.FindByText("Out of Order"));
                    ddlHKRoomStatus.Items.Remove(ddlHKRoomStatus.Items.FindByText("Out of Service"));
                    ddlHKRoomStatus.Items.Remove(ddlHKRoomStatus.Items.FindByText("Turn Down"));
                }
                else
                {
                    btnDetails.Visible = false;
                }
            }

            ///Commented By MR:Not necessary to load the rooms for each data row; rather getting this from global variable  
            //RoomNumberDA roomNumberDA = new RoomNumberDA();
            //List<RoomNumberBO> roomNumberListBO = new List<RoomNumberBO>();
            //roomNumberListBO = roomNumberDA.GetRoomNumberInfoByRoomType(0);

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                HKRoomStatusViewBO RoomStatusViewBO = (HKRoomStatusViewBO)e.Row.DataItem;
                TextBox fromDate = e.Row.FindControl("txtFromDate") as TextBox;
                TextBox fromTime = e.Row.FindControl("txtFromTime") as TextBox;

                TextBox toDate = e.Row.FindControl("txtToDate") as TextBox;
                TextBox toTime = e.Row.FindControl("txtToTime") as TextBox;

                HiddenField hfTxtFromDate = e.Row.FindControl("hfTxtFromDate") as HiddenField;
                HiddenField hfTxtFromTime = e.Row.FindControl("hfTxtFromTime") as HiddenField;
                HiddenField hfTxtToDate = e.Row.FindControl("hfTxtToDate") as HiddenField;
                HiddenField hfTxtToTime = e.Row.FindControl("hfTxtToTime") as HiddenField;

                fromDate.Text = RoomStatusViewBO.FromDateTime == null ? "" : Convert.ToDateTime(RoomStatusViewBO.FromDateTime).ToString(userInformationBO.ServerDateFormat);
                fromTime.Text = RoomStatusViewBO.FromDateTime == null ? "" : Convert.ToDateTime(RoomStatusViewBO.FromDateTime).ToString(userInformationBO.TimeFormat);

                toDate.Text = RoomStatusViewBO.ToDateTime == null ? "" : Convert.ToDateTime(RoomStatusViewBO.ToDateTime).ToString(userInformationBO.ServerDateFormat);
                toTime.Text = RoomStatusViewBO.ToDateTime == null ? "" : Convert.ToDateTime(RoomStatusViewBO.ToDateTime).ToString(userInformationBO.TimeFormat);

                hfTxtFromDate.Value= RoomStatusViewBO.FromDateTime == null ? "" : Convert.ToDateTime(RoomStatusViewBO.FromDateTime).ToString(userInformationBO.ServerDateFormat);
                hfTxtFromTime.Value= RoomStatusViewBO.FromDateTime == null ? "" : Convert.ToDateTime(RoomStatusViewBO.FromDateTime).ToString(userInformationBO.TimeFormat);
                hfTxtToDate.Value = RoomStatusViewBO.ToDateTime == null ? "" : Convert.ToDateTime(RoomStatusViewBO.ToDateTime).ToString(userInformationBO.ServerDateFormat); 
                hfTxtToTime.Value = RoomStatusViewBO.ToDateTime == null ? "" : Convert.ToDateTime(RoomStatusViewBO.ToDateTime).ToString(userInformationBO.TimeFormat);

                if (chkAllActiveReservation.Checked)
                {
                    Label lblRoomId = (Label)e.Row.FindControl("lblRoomId");
                    int roomId = !string.IsNullOrWhiteSpace(lblRoomId.Text) ? Convert.ToInt32(lblRoomId.Text) : 0;
                    string colorCode = roomNumberListBO.Where(x => x.RoomId == roomId).FirstOrDefault().ColorCodeName;

                    foreach (TableCell cell in e.Row.Cells)
                    {
                        cell.BackColor = Color.FromName(colorCode);
                        cell.ForeColor = Color.White;
                    }
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ddlAllHKSelect.SelectedValue = "0";
            this.LoadDetailsInformation();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int count = 0;
            string statusMsg = string.Empty;
            List<RoomNumberBO> roomList = new List<RoomNumberBO>();
            int rows = gvRoomStatus.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                CheckBox cb = (CheckBox)gvRoomStatus.Rows[i].FindControl("chkIsSavePermission");
                if (cb.Checked == true)
                {
                    count++;
                    RoomNumberBO bo = new RoomNumberBO();

                    Label lblRoomId = (Label)gvRoomStatus.Rows[i].FindControl("lblRoomId");
                    DropDownList ddlHKRoomStatus = (DropDownList)gvRoomStatus.Rows[i].FindControl("ddlHKRoomStatus");
                    TextBox txtFromDate = (TextBox)gvRoomStatus.Rows[i].FindControl("txtFromDate");
                    TextBox txtFromTime = (TextBox)gvRoomStatus.Rows[i].FindControl("txtFromTime");
                    TextBox txtToDate = (TextBox)gvRoomStatus.Rows[i].FindControl("txtToDate");
                    TextBox txtToTime = (TextBox)gvRoomStatus.Rows[i].FindControl("txtToTime");
                    TextBox txtReason = (TextBox)gvRoomStatus.Rows[i].FindControl("txtReason");
                    if(ddlAllHKSelect.SelectedValue=="3"|| ddlAllHKSelect.SelectedValue == "4")
                    {
                        if (txtFromDate.Text == "" && txtFromDateForAll.Text == "")
                        {
                            IsSuccess = 1;
                            CommonHelper.AlertInfo(innboardMessage, "Please confirm From Date for " + ddlAllHKSelect.SelectedItem.Text, AlertType.Warning);
                            return;
                        }
                        else if (txtFromTime.Text == "" && txtFromTimeForAll.Text == "")
                        {
                            IsSuccess = 1;
                            CommonHelper.AlertInfo(innboardMessage, "Please select From Time for " + ddlAllHKSelect.SelectedItem.Text, AlertType.Warning);
                            return;
                        }
                        else if (txtToDate.Text == "" && txtToDateForAll.Text == "")
                        {
                            IsSuccess = 1;
                            CommonHelper.AlertInfo(innboardMessage, "Please select To Date for " + ddlAllHKSelect.SelectedItem.Text, AlertType.Warning);
                            return;
                        }

                        else if (txtToTime.Text == "" && txtToTimeForAll.Text == "")
                        {
                            IsSuccess = 1;
                            CommonHelper.AlertInfo(innboardMessage, "Please select To Time for " + ddlAllHKSelect.SelectedItem.Text, AlertType.Warning);
                            return;
                        }
                    }
                    DateTime fromDateVal = DateTime.Now;
                    DateTime toDateVal = DateTime.Now;

                    bo.RoomId = Convert.ToInt32(lblRoomId.Text);
                    bo.HKRoomStatusId = Convert.ToInt64(ddlHKRoomStatus.SelectedValue);
                    bo.LastCleanDate2 = DateTime.Now;
                    bo.HKRoomStatusName = ddlHKRoomStatus.SelectedItem.Text;
                    statusMsg = bo.HKRoomStatusName;
                    if (ddlHKRoomStatus.SelectedItem.Text == "Out of Order" || ddlHKRoomStatus.SelectedItem.Text == "Out of Service")
                    {
                        if (!string.IsNullOrEmpty(txtFromDate.Text))
                        {
                            fromDateVal =hmUtility.GetDateTimeFromString(txtFromDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                            bo.FromDate = hmUtility.GetDateTimeFromString(txtFromDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                        }        
                        else
                        {
                            if (string.IsNullOrWhiteSpace(txtFromDateForAll.Text))
                            {
                                IsSuccess = 1;
                                CommonHelper.AlertInfo(innboardMessage, "Please confirm From Date", AlertType.Warning);
                                return;
                            }
                            else
                            {
                                bo.FromDate = hmUtility.GetDateTimeFromString(txtFromDateForAll.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                            }
                        }
                            

                        if (!string.IsNullOrEmpty(txtFromTime.Text))
                        {
                            
                            bo.FromDate = Convert.ToDateTime(bo.FromDate.ToString("yyyy-MM-dd") + " " + txtFromTime.Text);
                        }                           
                        else
                        {
                            if (string.IsNullOrWhiteSpace(txtFromTimeForAll.Text))
                            {
                                IsSuccess = 1;
                                CommonHelper.AlertInfo(innboardMessage, "Please confirm From Time", AlertType.Warning);

                                return;
                            }
                            else
                            {
                                bo.FromDate = Convert.ToDateTime(bo.FromDate.ToString("yyyy-MM-dd") + " " + txtFromTimeForAll.Text);
                            }
                        }
                        //bo.FromDate = Convert.ToDateTime(bo.FromDate.ToString("yyyy-MM-dd") + " " + txtFromTimeForAll.Text);

                        if (!string.IsNullOrEmpty(txtToDate.Text))
                        {
                            
                            toDateVal = hmUtility.GetDateTimeFromString(txtToDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                            //toDateVal = DateTime.Parse(txtToDate.Text);
                            if (fromDateVal.Date > toDateVal.Date)
                            {
                                IsSuccess = 1;
                                CommonHelper.AlertInfo(innboardMessage, "Please Give a Valid To Date", AlertType.Warning);
                                return;
                            }
                            else
                                bo.ToDate = hmUtility.GetDateTimeFromString(txtToDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                        }
                           
                        else
                        {
                            if (string.IsNullOrWhiteSpace(txtToDateForAll.Text))
                            {
                                IsSuccess = 1;
                                CommonHelper.AlertInfo(innboardMessage, "Please confirm To Date", AlertType.Warning);
                                return;
                            }
                            else
                            {
                                bo.ToDate = hmUtility.GetDateTimeFromString(txtToDateForAll.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                            }
                        }
                        //bo.ToDate = hmUtility.GetDateTimeFromString(txtToDateForAll.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);


                        if (!string.IsNullOrEmpty(txtToTime.Text))
                            bo.ToDate = Convert.ToDateTime(bo.ToDate.ToString("yyyy-MM-dd") + " " + txtToTime.Text);
                        else
                        {
                            if (string.IsNullOrWhiteSpace(txtToTimeForAll.Text))
                            {
                                IsSuccess = 1;
                                CommonHelper.AlertInfo(innboardMessage, "Please confirm To Time", AlertType.Warning);
                                return;
                            }
                            else
                            {
                                bo.ToDate = Convert.ToDateTime(bo.ToDate.ToString("yyyy-MM-dd") + " " + txtToTimeForAll.Text);
                            }
                        }
                        //bo.ToDate = Convert.ToDateTime(bo.ToDate.ToString("yyyy-MM-dd") + " " + txtToTimeForAll.Text);

                        if (!string.IsNullOrWhiteSpace(txtReason.Text))
                            bo.Remarks = txtReason.Text;
                        else if (!string.IsNullOrWhiteSpace(txtRemarks.Text))
                        {
                            bo.Remarks = txtRemarks.Text;
                        }
                        else
                        {
                            //bo.Remarks = txtRemarks.Text;
                            IsSuccess = 1;
                            CommonHelper.AlertInfo(innboardMessage, "Please Give Valid Reason", AlertType.Warning);
                            return;
                        }
                            

                    }
                    else
                    {
                        bo.FromDate = DateTime.Now;
                        bo.ToDate = DateTime.Now;
                        bo.Remarks = "";
                    }
                    roomList.Add(bo);
                }
            }
            if (count == 0)
            {
                IsSuccess = 1;
                CommonHelper.AlertInfo(innboardMessage, "Please select at least one room.", AlertType.Warning);
                return;
            }

            RoomNumberDA roomDA = new RoomNumberDA();
            bool status = false;
            if (roomList.Count > 0)
            {
                status = roomDA.UpdateRoomFromHk(roomList);

                // //---------Activity Log Transaction---------------
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RoomClean.ToString(), 0,
                    ProjectModuleEnum.ProjectModule.HouseKeeping.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomClean));
                ClearForm();
            }

            if (status)
            {
                LoadDetailsInformation();
                CommonHelper.AlertInfo(innboardMessage,statusMsg+" Operation Successful", AlertType.Success);
            }
        }
        protected void ddlAllHKSelect_Change(object sender, EventArgs e)
        {
            this.LoadDetailsInformation();
            string hkStatus = ddlAllHKSelect.SelectedValue;
            int rows = gvRoomStatus.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                Label lblFORoomStatus = (Label)gvRoomStatus.Rows[i].FindControl("lblFORoomStatus");
                DropDownList ddlHKRoomStatus = (DropDownList)gvRoomStatus.Rows[i].FindControl("ddlHKRoomStatus");
                if (lblFORoomStatus.Text != "Occupied")
                {
                    if (hkStatus != "0")
                    {
                        //if (hkStatus == "3")
                        //{
                        //    ListItem item = ddlHKRoomStatus.Items.FindByText("Out of Order");
                        //    if (item != null)
                        //    {
                        //        ddlHKRoomStatus.SelectedValue = hkStatus;
                        //    }
                        //}
                        //else if (hkStatus == "4")
                        //{
                        //    ListItem item = ddlHKRoomStatus.Items.FindByText("Out of Service");
                        //    if (item != null)
                        //    {
                        //        ddlHKRoomStatus.SelectedValue = hkStatus;
                        //    }
                        //}

                        ddlHKRoomStatus.SelectedValue = hkStatus;
                    }
                }
                else
                {
                    if (hkStatus != "0" && hkStatus != "3" && hkStatus != "4")
                    {
                        ddlHKRoomStatus.SelectedValue = hkStatus;
                    }
                }
            }
            IsSuccess = 1;
        }

        //************************ User Defined Function ********************//
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtCurrentDate.Value = hmUtility.GetStringFromDateTime(dateTime);
        }
        ////private void LoadHouseKeepingMorningDirtyHour()
        ////{
        ////    //string url = "../DataSyncronizeScheduleTask/SyncronizeData.aspx?Type=HKDirty";
        ////    //string sPopUp = "window.open('" + url + "', 'popup_window', 'width=715,height=780,left=300,top=50,resizable=yes, scrollbars=1');";
        ////    //ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);


        ////    HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
        ////    HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
        ////    commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("HouseKeepingMorningDirtyHour", "HouseKeepingMorningDirtyHour");
        ////    if (commonSetupBO != null)
        ////    {
        ////        if (commonSetupBO.SetupId > 0)
        ////        {
        ////            hfHouseKeepingMorningDirtyHour.Value = commonSetupBO.SetupValue;
        ////        }
        ////    }

        ////}
        private void LoadFloor()
        {
            HMFloorDA floorDA = new HMFloorDA();
            ddlFloorId.DataSource = floorDA.GetActiveHMFloorInfo();
            ddlFloorId.DataTextField = "FloorName";
            ddlFloorId.DataValueField = "FloorId";
            ddlFloorId.DataBind();

            //RoomNumberDA roomNumberDA = new RoomNumberDA();
            //List<RoomNumberBO> roomNumberBOList = roomNumberDA.GetRoomNumberInfo();

            //int totalRoomCount = 100;
            //if (roomNumberBOList != null)
            //{
            //    totalRoomCount = roomNumberBOList.Count();
            //}

            ListItem item = new ListItem();
            item.Value = "0";
            //if (totalRoomCount > 100)
            //{
            //    hfIsFloorNameValidationActive.Value = "1";
            //    item.Text = hmUtility.GetDropDownFirstValue();
            //}
            //else
            //{
                hfIsFloorNameValidationActive.Value = "0";
                item.Text = hmUtility.GetDropDownFirstAllValue();
            //}

            ddlFloorId.Items.Insert(0, item);
        }
        private void LoadFloorBlock()
        {
            FloorBlockDA floorDA = new FloorBlockDA();
            List<FloorBlockBO> floorList = new List<FloorBlockBO>();
            floorList = floorDA.GetActiveFloorBlockInfo();

            ddlFloorBlock.DataSource = floorList;
            ddlFloorBlock.DataTextField = "BlockName";
            ddlFloorBlock.DataValueField = "BlockId";
            ddlFloorBlock.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlFloorBlock.Items.Insert(0, item);
        }
        private void LoadFORoomStatus()
        {
            RoomStatusDA FORoomStatusDA = new RoomStatusDA();
            List<RoomStatusBO> files1 = new List<RoomStatusBO>();
            List<RoomStatusBO> files2 = new List<RoomStatusBO>();
            List<RoomStatusBO> files = FORoomStatusDA.GetRoomStatusInfo();

            if (files.Count > 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    files1.Add(files[i]);
                }
                for (int i = 4; i < files.Count; i++)
                {
                    files2.Add(files[i]);
                }
            }
            else
            {
                files1 = files.ToList();
            }

            this.chkFORoomStatus1.DataSource = files1;
            this.chkFORoomStatus1.DataTextField = "StatusName";
            this.chkFORoomStatus1.DataValueField = "StatusId";
            this.chkFORoomStatus1.DataBind();

            this.chkFORoomStatus2.DataSource = files2;
            this.chkFORoomStatus2.DataTextField = "StatusName";
            this.chkFORoomStatus2.DataValueField = "StatusId";
            this.chkFORoomStatus2.DataBind();

            for (int i = 0; i < chkFORoomStatus1.Items.Count; i++)
            {
                chkFORoomStatus1.Items[i].Selected = true;
            }
            for (int i = 0; i < chkFORoomStatus2.Items.Count; i++)
            {
                chkFORoomStatus2.Items[i].Selected = true;
            }

            List<RoomStatusBO> reservationStatusBOList = new List<RoomStatusBO>();

            RoomStatusBO longStayingStatusBO = new RoomStatusBO();
            longStayingStatusBO.StatusId = 101;
            longStayingStatusBO.StatusName = "Long Staying";
            reservationStatusBOList.Add(longStayingStatusBO);

            RoomStatusBO expectedArrivalStatusBO = new RoomStatusBO();
            expectedArrivalStatusBO.StatusId = 104;
            expectedArrivalStatusBO.StatusName = "Exp. Arrival";
            reservationStatusBOList.Add(expectedArrivalStatusBO);

            RoomStatusBO todaysArrivalStatusBO = new RoomStatusBO();
            todaysArrivalStatusBO.StatusId = 102;
            todaysArrivalStatusBO.StatusName = "Today's Checked In";
            reservationStatusBOList.Add(todaysArrivalStatusBO);

            RoomStatusBO expectedDepartureStatusBO = new RoomStatusBO();
            expectedDepartureStatusBO.StatusId = 103;
            expectedDepartureStatusBO.StatusName = "Exp. Departure";
            reservationStatusBOList.Add(expectedDepartureStatusBO);

            this.chkReservationStatus.DataSource = reservationStatusBOList;
            this.chkReservationStatus.DataTextField = "StatusName";
            this.chkReservationStatus.DataValueField = "StatusId";
            this.chkReservationStatus.DataBind();
        }
        private void LoadHKRoomStatus()
        {
            HKRoomStatusDA HKRoomStatusDA = new HKRoomStatusDA();
            List<HKRoomStatusBO> files1 = new List<HKRoomStatusBO>();
            List<HKRoomStatusBO> files2 = new List<HKRoomStatusBO>();
            List<HKRoomStatusBO> files = HKRoomStatusDA.GetHKRoomStatusType().Where(x => x.HKRoomStatusId != 1 && x.HKRoomStatusId != 2).ToList();

            if (files.Count > 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    files1.Add(files[i]);
                }
                for (int i = 4; i < files.Count; i++)
                {
                    files2.Add(files[i]);
                }
            }
            else
            {
                files1 = files.ToList();
            }

            this.chkHKRoomStatus1.DataSource = files1;
            this.chkHKRoomStatus1.DataTextField = "StatusName";
            this.chkHKRoomStatus1.DataValueField = "HKRoomStatusId";
            this.chkHKRoomStatus1.DataBind();

            this.chkHKRoomStatus2.DataSource = files2;
            this.chkHKRoomStatus2.DataTextField = "StatusName";
            this.chkHKRoomStatus2.DataValueField = "HKRoomStatusId";
            this.chkHKRoomStatus2.DataBind();

            ddlAllHKSelect.DataSource = files;
            ddlAllHKSelect.DataTextField = "StatusName";
            ddlAllHKSelect.DataValueField = "HKRoomStatusId";
            ddlAllHKSelect.DataBind();

            ListItem FirstItem = new ListItem();
            FirstItem.Value = "0";
            FirstItem.Text = hmUtility.GetDropDownFirstValue();
            this.ddlAllHKSelect.Items.Insert(0, FirstItem);
        }
        private void LoadStatusForAll()
        {
            HKRoomStatusDA HKRoomStatusDA = new HKRoomStatusDA();
            List<HKRoomStatusBO> files = HKRoomStatusDA.GetHKRoomStatusType().Where(x => x.HKRoomStatusId != 1 && x.HKRoomStatusId != 2).ToList();
            ddlAllHKSelect.DataSource = files;
            ddlAllHKSelect.DataTextField = "StatusName";
            ddlAllHKSelect.DataValueField = "HKRoomStatusId";
            ddlAllHKSelect.DataBind();

            ListItem FirstItem = new ListItem();
            FirstItem.Value = "0";
            FirstItem.Text = hmUtility.GetDropDownFirstValue();
            this.ddlAllHKSelect.Items.Insert(0, FirstItem);
        }
        private void ClearCheckList()
        {
            for (int i = 0; i < chkFORoomStatus1.Items.Count; i++)
            {
                chkFORoomStatus1.Items[i].Selected = false;
            }
            for (int i = 0; i < chkFORoomStatus2.Items.Count; i++)
            {
                chkFORoomStatus2.Items[i].Selected = false;
            }

            for (int i = 0; i < chkHKRoomStatus1.Items.Count; i++)
            {
                chkHKRoomStatus1.Items[i].Selected = false;
            }
            for (int i = 0; i < chkHKRoomStatus2.Items.Count; i++)
            {
                chkHKRoomStatus2.Items[i].Selected = false;
            }
        }
        private void ClearForm()
        {
            if (chkAllActiveReservation.Checked)
            {
                this.chkAllActiveReservation.Checked = true;
                
            }
            else
            {
                this.chkAllActiveReservation.Checked = false;
            }
            
            this.ddlAllHKSelect.SelectedValue = "0";
            this.txtRemarks.Text = string.Empty;
            this.txtFromDateForAll.Text = string.Empty;
            this.txtFromTimeForAll.Text = string.Empty;
            this.txtToDateForAll.Text = string.Empty;
            this.txtToTimeForAll.Text = string.Empty;
        }
        private void MakeOutofOrderRoomVacant()
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<RoomNumberBO> roomList = new List<RoomNumberBO>();
            roomList = roomNumberDA.GetRoomNumberInfo();

            foreach (RoomNumberBO roombo in roomList)
            {
                if (roombo.StatusId == 3)
                {
                    List<RoomNumberBO> temproomList = new List<RoomNumberBO>();
                    RoomLogFileBO bo = new RoomLogFileBO();
                    bool status = false;

                    bo = roomNumberDA.GetRoomLogFileInfo(roombo.RoomId);
                    if (bo != null)
                    {
                        if (bo.ToDate.Date == DateTime.Now.AddDays(-1).Date)
                        {
                            roombo.HKRoomStatusId = 5;
                            roombo.HKRoomStatusName = "Clean";
                            roombo.LastCleanDate2 = DateTime.Now;
                            roombo.Remarks = "";
                            roombo.ToDate = DateTime.Now;
                            roombo.FromDate = bo.FromDate;
                            temproomList.Add(roombo);
                            status = roomNumberDA.UpdateRoomFromHk(temproomList);

                            // //---------Activity Log Transaction---------------
                            Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RoomClean.ToString(), 0,
                                ProjectModuleEnum.ProjectModule.HouseKeeping.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomClean));
                        }
                    }
                }
            }
        }
        public static string GetReservationInformationView(RoomReservationBO reservationBO)
        {
            HMUtility hmUtility = new HMUtility();
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' width='100%' id='TableReservationInformation'>";
            strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Reservation Number:</td>";
            strTable += "<td align='left' style='width: 25%'>: " + reservationBO.ReservationNumber + "</td>";
            strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Reservation Date</td>";
            strTable += "<td align='left' style='width: 25%'>: " + hmUtility.GetStringFromDateTime(reservationBO.ReservationDate) + "</td>";
            strTable += "</tr>";

            strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Contact Person:</td>";
            strTable += "<td align='left' style='width: 25%'>: " + reservationBO.ContactPerson + "</td>";
            strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Contact Number:</td>";
            strTable += "<td align='left' style='width: 25%'>: " + reservationBO.ContactNumber + "</td>";
            strTable += "</tr>";

            strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Check In:</td>";
            strTable += "<td align='left' style='width: 25%'>: " + hmUtility.GetStringFromDateTime(reservationBO.DateIn) + "</td>";
            strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Check Out:</td>";
            strTable += "<td align='left' style='width: 25%'>: " + hmUtility.GetStringFromDateTime(reservationBO.DateOut) + "</td>";
            strTable += "</tr>";

            strTable += "<tr style='background-color:#E3EAEB;'>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'>Company</td>";
            strTable += "<td align='left' style='width: 25%'>: " + reservationBO.CompanyName + "</td>";
            strTable += "<td align='left' style='width: 10%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 25%;font-weight:bold'></td>";
            strTable += "<td align='left' style='width: 25%'></td>";
            strTable += "</tr>";

            strTable += "</table>";
            return strTable;
        }
        public static string GetGuestInformationView(List<GuestInformationBO> List)
        {
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' width='100%' id='TableGuestInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";


            strTable += "<th align='left' scope='col'>Guest Name</th><th align='left' scope='col'>Email</th> <th align='left' scope='col'>Phone</th><th align='left' scope='col'>Address</th> <th align='left' scope='col'>Country</th></tr>";
            int counter = 0;

            foreach (GuestInformationBO item in List)
            {
                counter++;
                strTable += "<tr style='background-color:#E3EAEB;'>";
                strTable += "<td align='left' style='width: 20%'>" + item.GuestName + "</td>";
                strTable += "<td align='left' style='width: 20%'>" + item.GuestEmail + "</td>";
                strTable += "<td align='left' style='width: 20%'>" + item.GuestPhone + "</td>";
                strTable += "<td align='left' style='width: 20%'>" + item.GuestAddress1 + "</td>";
                strTable += "<td align='left' style='width: 20%'>" + item.CountryName + "</td>";
                strTable += "</tr>";
            }


            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            return strTable;
        }
        private void LoadDetailsInformation()
        {   
            if (hfIsFloorNameValidationActive.Value == "1")
            {
                if (ddlFloorId.SelectedValue == "0")
                {
                    ddlFloorId.Focus();
                    CommonHelper.AlertInfo(innboardMessage, "Please Select Floor Name.", AlertType.Warning);
                    return;
                }
            }
            
            //HKRoomStatusDA HKRoomStatusDA = new HKRoomStatusDA();
            //List<HKRoomStatusBO> files = HKRoomStatusDA.GetHKRoomStatusType().Where(x => x.HKRoomStatusId != 1 && x.HKRoomStatusId != 2).ToList();
            //ddlAllHKSelect.DataSource = files;
            //ddlAllHKSelect.DataTextField = "StatusName";
            //ddlAllHKSelect.DataValueField = "HKRoomStatusId";
            //ddlAllHKSelect.DataBind();

            //ListItem FirstItem = new ListItem();
            //FirstItem.Value = "0";
            //FirstItem.Text = hmUtility.GetDropDownFirstValue();
            //this.ddlAllHKSelect.Items.Insert(0, FirstItem);

            string foRoomStatusId = string.Empty;
            string hkRoomStatusId = string.Empty;

            for (int i = 0; i < chkFORoomStatus1.Items.Count; i++)
            {
                if (chkFORoomStatus1.Items[i].Selected)
                {
                    if (string.IsNullOrEmpty(foRoomStatusId))
                    {
                        foRoomStatusId = chkFORoomStatus1.Items[i].Value;
                    }
                    else foRoomStatusId += "," + chkFORoomStatus1.Items[i].Value;
                }
            }
            for (int i = 0; i < chkFORoomStatus2.Items.Count; i++)
            {
                if (chkFORoomStatus2.Items[i].Selected)
                {
                    if (string.IsNullOrEmpty(foRoomStatusId))
                    {
                        foRoomStatusId = chkFORoomStatus2.Items[i].Value;
                    }
                    else foRoomStatusId += "," + chkFORoomStatus2.Items[i].Value;
                }
            }

            for (int i = 0; i < chkHKRoomStatus1.Items.Count; i++)
            {
                if (chkHKRoomStatus1.Items[i].Selected)
                {
                    if (string.IsNullOrEmpty(hkRoomStatusId))
                    {
                        hkRoomStatusId = chkHKRoomStatus1.Items[i].Value;
                        if (hkRoomStatusId == "5")
                        {
                            //hkRoomStatusId += "," + "1";
                            hkRoomStatusId += ",1,2";
                        }
                        else if (hkRoomStatusId == "6")
                        {
                            hkRoomStatusId += ",1,2";
                        }
                    }
                    else
                    {
                        hkRoomStatusId += "," + chkHKRoomStatus1.Items[i].Value;
                        if (chkHKRoomStatus1.Items[i].Value == "5")
                        {
                            hkRoomStatusId += ",1,2";
                        }
                        else if (chkHKRoomStatus1.Items[i].Value == "6")
                        {
                            hkRoomStatusId += ",1,2";
                        }
                    }
                }
            }
            for (int i = 0; i < chkHKRoomStatus2.Items.Count; i++)
            {
                if (chkHKRoomStatus2.Items[i].Selected)
                {
                    if (string.IsNullOrEmpty(hkRoomStatusId))
                    {
                        hkRoomStatusId = chkHKRoomStatus2.Items[i].Value;
                        if (hkRoomStatusId == "5")
                        {
                            hkRoomStatusId += ",1,2";
                        }
                        else if (hkRoomStatusId == "6")
                        {
                            hkRoomStatusId += ",1,2";
                        }
                    }
                    else
                    {
                        hkRoomStatusId += "," + chkHKRoomStatus2.Items[i].Value;
                        if (chkHKRoomStatus1.Items[i].Value == "5")
                        {
                            hkRoomStatusId += ",1,2";
                        }
                        else if (chkHKRoomStatus1.Items[i].Value == "6")
                        {
                            hkRoomStatusId += ",1,2";
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(foRoomStatusId) && string.IsNullOrEmpty(hkRoomStatusId))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please select some status.", AlertType.Warning);
                return;
            }

            HKRoomStatusDA hkRoomStatusDA = new HKRoomStatusDA();
            List<HKRoomStatusViewBO> roomStatusBO = new List<HKRoomStatusViewBO>();

            roomStatusBO = hkRoomStatusDA.GetHKRoomStatusInfo(foRoomStatusId, hkRoomStatusId, Convert.ToInt32(ddlFloorId.SelectedValue), Convert.ToInt32(ddlFloorBlock.SelectedValue));
            foreach (HKRoomStatusViewBO bo in roomStatusBO)
            {
                if (bo.FORoomStatus != "Occupied")
                {
                    bo.DateIn = "";
                    bo.DateOut = "";
                }
            }

            List<RoomCalenderBO> calenderMasterListBO = new List<RoomCalenderBO>();
            RoomCalenderDA calenderDA = new RoomCalenderDA();
            calenderMasterListBO = calenderDA.GetRoomInfoForCalender(DateTime.Now, DateTime.Now.AddDays(1)).ToList();

            RoomNumberDA roomNumberDA = new RoomNumberDA();
            roomNumberListBO = new List<RoomNumberBO>();
            roomNumberListBO = roomNumberDA.GetRoomNumberInfoByRoomType(0);
            for (int iRoomNumber = 0; iRoomNumber < roomNumberListBO.Count; iRoomNumber++)
            {
                var isRoom = (from rs in roomStatusBO where rs.RoomId == roomNumberListBO[iRoomNumber].RoomId select rs).FirstOrDefault();
                if (isRoom != null)
                {
                    if (roomNumberListBO[iRoomNumber].StatusId == 4)
                    {
                        isRoom.ReservationId = "";
                        isRoom.ReservationStatus = "";
                    }
                    else if (roomNumberListBO[iRoomNumber].StatusId == 3)
                    {
                        isRoom.ReservationId = "";
                        isRoom.ReservationStatus = "";
                    }
                    else if (roomNumberListBO[iRoomNumber].StatusId == 2)
                    {
                        if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomLongStayingDiv")
                        {
                            isRoom.ReservationId = "101";
                            isRoom.ReservationStatus = "Long Staying";
                        }
                        else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomTodaysCheckInDiv")
                        {
                            isRoom.ReservationId = "102";
                            isRoom.ReservationStatus = "Today's Checked In";
                        }
                        else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomPossibleVacantDiv")
                        {
                            isRoom.ReservationId = "103";
                            isRoom.ReservationStatus = "Exp. Departure";
                        }
                        else
                        {
                            isRoom.ReservationId = "";
                            isRoom.ReservationStatus = "";
                        }
                    }
                    else if (roomNumberListBO[iRoomNumber].StatusId == 1)
                    {
                        if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomVacantDiv")
                        {
                            isRoom.ReservationId = "";
                            isRoom.ReservationStatus = "";
                        }
                        else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomVacantDirtyDiv")
                        {
                            isRoom.ReservationId = "";
                            isRoom.ReservationStatus = "";

                            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
                            //calenderList = calenderMasterListBO.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId & x.TransectionStatus == "Reservation").ToList();
                            calenderList = calenderMasterListBO.Where(x => x.RoomId == roomNumberListBO[iRoomNumber].RoomId & x.TransectionStatus == "Reservation" & x.CheckIn.Date == DateTime.Now.Date).ToList();
                            if (calenderList != null)
                            {
                                if (calenderList.Count > 0)
                                {
                                    isRoom.ReservationId = "104";
                                    isRoom.ReservationStatus = "Exp. Arrival";
                                }
                            }
                        }
                        else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomReservedDiv")
                        {
                            isRoom.ReservationId = "104";
                            isRoom.ReservationStatus = "Exp. Arrival";
                        }
                    }
                }
            }

            List<string> reservationStatusIdList = new List<string>();
            for (int i = 0; i < chkReservationStatus.Items.Count; i++)
            {
                if (chkReservationStatus.Items[i].Selected)
                {
                    reservationStatusIdList.Add(chkReservationStatus.Items[i].Value.ToString());
                }
            }

            if (reservationStatusIdList.Count == 0)
            {
                this.gvRoomStatus.DataSource = roomStatusBO;
                this.gvRoomStatus.DataBind();
            }
            else
            {
                this.gvRoomStatus.DataSource = roomStatusBO.Where(x => reservationStatusIdList.Contains(x.ReservationId)).ToList();
                this.gvRoomStatus.DataBind();
            }

            if (roomStatusBO.Count > 0)
            {
                IsSuccess = 1;
            }
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static string LoadGuestPreferenceInfo()
        {
            List<GuestPreferenceBO> guestReferenceList = new List<GuestPreferenceBO>();
            GuestPreferenceDA guestReferenceDA = new GuestPreferenceDA();

            string HTML = string.Empty;

            guestReferenceList = guestReferenceDA.GetActiveGuestPreferenceInfo();
            HTML = GetHTMLGuestReferenceGridView(guestReferenceList);

            return HTML;
        }
        public static string GetHTMLGuestReferenceGridView(List<GuestPreferenceBO> List)
        {
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' id='GuestPreferenceInformation' width='100%' border: '1px solid #cccccc'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='center' scope='col'>Select</th><th align='left' scope='col'>Preference</th></tr>";

            int counter = 0;
            foreach (GuestPreferenceBO dr in List)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:White;'>";
                }

                strTable += "<td style='display:none;'>" + dr.PreferenceId + "</td>";
                strTable += "<td align='center' style='width: 20px'>";
                strTable += "&nbsp;<input type='checkbox'  id='" + dr.PreferenceId + "' name='" + dr.PreferenceName + "' value='" + dr.PreferenceId + "' >";
                strTable += "</td><td align='left' style='width: 138px'>" + dr.PreferenceName + "</td></tr>";
            }

            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Preference Available !</td></tr>";
            }

            strTable += "<div style='margin-top:12px;'>";
            strTable += "   <button type='button' onClick='javascript:return GetCheckedGuestPreference()' id='btnAddRoomId' class='btn btn-primary'> OK</button>";
            strTable += "</div>";
            return strTable;
        }
        [WebMethod]
        public static GuestRegistrationMappingBO GetGuestIdFMRoomId(int roomId)
        {
            GuestRegistrationMappingBO guestInfo = new GuestRegistrationMappingBO();
            GuestInformationDA guestInfoDA = new GuestInformationDA();
            guestInfo = guestInfoDA.GetGuestIdFMRoomId(roomId);
            return guestInfo;
        }
        [WebMethod]
        public static string GetGuestSavedPreference(int guestId)
        {
            string guestPreferenceId = string.Empty;
            GuestPreferenceDA preferenceDA = new GuestPreferenceDA();
            List<GuestPreferenceBO> preferenceList = new List<GuestPreferenceBO>();
            preferenceList = preferenceDA.GetGuestPreferenceInfoByGuestId(Convert.ToInt32(guestId));

            if (preferenceList.Count > 0)
            {
                foreach (GuestPreferenceBO preference in preferenceList)
                {
                    if (!string.IsNullOrEmpty(guestPreferenceId))
                    {
                        guestPreferenceId += "," + preference.PreferenceId;
                    }
                    else
                    {
                        guestPreferenceId = preference.PreferenceId.ToString();
                    }
                }
            }

            return guestPreferenceId;
        }
        [WebMethod]
        public static bool SaveGuestPreference(string selectedPreferenceId, int guestId)
        {
            bool status = false;
            GuestPreferenceDA guestPreferenceDA = new GuestPreferenceDA();
            status = guestPreferenceDA.SaveGuestPreferenceMappingInfo(selectedPreferenceId, guestId);

            return status;
        }
        [WebMethod]
        public static GuestReservationMappingBO GetReservationIdFMRoomId(int roomId)
        {
            GuestReservationMappingBO guestInfo = new GuestReservationMappingBO();
            GuestInformationDA guestInfoDA = new GuestInformationDA();
            guestInfo = guestInfoDA.GetReservationIdFMRoomId(roomId);
            return guestInfo;
        }
        [WebMethod]
        public static string GetReservationformationByReservationId(int ReservationId)
        {
            RoomReservationBO reservationBO = new RoomReservationBO();
            RoomReservationDA reservationDA = new RoomReservationDA();
            reservationBO = reservationDA.GetRoomReservationInfoById(ReservationId);
            return GetReservationInformationView(reservationBO);
        }
        [WebMethod]
        public static string GetReservationGuestInformationByReservationId(int ReservationId)
        {
            List<GuestInformationBO> guestInformationList = new List<GuestInformationBO>();
            GuestInformationDA guestInformationDA = new GuestInformationDA();
            guestInformationList = guestInformationDA.GetGuestInformationDetailByResId(Convert.ToInt32(ReservationId), false);

            return GetGuestInformationView(guestInformationList);
        }
    }
}