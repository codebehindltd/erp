using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using Newtonsoft.Json;
using HotelManagement.Data;
using HotelManagement.Entity;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.UserInformation;
using Mamun.Presentation.Website.Common;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class frmInnboardDashboard : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                if (!IsPostBack)
                {
                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    if (userInformationBO != null)
                    {
                        UserGroupBO userGroupBO = new UserGroupBO();
                        UserGroupDA userGroupDA = new UserGroupDA();
                        userGroupBO = userGroupDA.GetUserGroupInfoById(userInformationBO.UserGroupId);
                        if (userGroupBO != null)
                        {
                            if (userGroupBO.DefaultHomePageId > 0)
                            {
                                Response.Redirect(userGroupBO.DefaultHomePage);
                            }
                        }
                    }
                }
                LoadCurrentDate();
                SetDefaulCleanTime();
            }
        }
        protected void lbtnDashboardManagement_Click(object sender, EventArgs e)
        {
            Response.Redirect("/HMCommon/frmDashboardManagement.aspx");
        }
        protected void btnPaxInRateUpdate_Click(object sender, EventArgs e)
        {
            int registrationId = !string.IsNullOrWhiteSpace(this.hfPaxInRegistrationId.Value) ? Convert.ToInt32(this.hfPaxInRegistrationId.Value) : 0;
            int guestId = !string.IsNullOrWhiteSpace(this.hfPaxInGuestId.Value) ? Convert.ToInt32(this.hfPaxInGuestId.Value) : 0;
            decimal paxInRate = !string.IsNullOrWhiteSpace(this.txtPaxInRate.Text) ? Convert.ToDecimal(this.txtPaxInRate.Text) : 0;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (registrationId > 0 && guestId > 0 && paxInRate > 0)
            {
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                Boolean status = roomRegistrationDA.UpdateGuestPaxInRateInfo(registrationId, guestId, paxInRate, userInformationBO.UserInfoId);
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
            }
        }
        protected void btnBillPreview_Click(object sender, EventArgs e)
        {
            string url = "/HotelManagement/Reports/frmReportGuestBillPreview.aspx";
            string s = "window.open('" + url + "', 'popup_window', 'width=750,height=680,left=300,top=50,resizable=yes');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
        //************************ User Defined Function ********************//
        //private void RoomStatisticsInfo()
        //{
        //    UserInformationBO userInformationBO = new UserInformationBO();
        //    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
        //    if (!userInformationBO.IsMenuSearchRoomSearchRoomStatisticsInfoEnable)
        //    {
        //        pnlMenuSearchRoomSearchRoomStatisticsInfo.Visible = false;
        //    }
        //    else
        //    {
        //        HMCommonDA roomStatisticsInfoDA = new HMCommonDA();
        //        RoomStatisticsInfoBO roomStatisticsInfoBO = new RoomStatisticsInfoBO();
        //        roomStatisticsInfoBO = roomStatisticsInfoDA.GetRoomStatisticsInfo();
        //        if (roomStatisticsInfoBO != null)
        //        {
        //            lblExpectedArrival.Text = roomStatisticsInfoBO.ExpectedArrival.ToString();
        //            lblExpectedDeparture.Text = roomStatisticsInfoBO.ExpectedDeparture.ToString();
        //            lblCheckInRoom.Text = roomStatisticsInfoBO.CheckInRoom.ToString();
        //            lblWalkInRoom.Text = roomStatisticsInfoBO.WalkInRoom.ToString();
        //            lblRoomToSell.Text = roomStatisticsInfoBO.RoomToSell.ToString();
        //            lblRegisterComplaint.Text = roomStatisticsInfoBO.RegisterComplaint.ToString();
        //            lblInhouseRoomOrGuest.Text = roomStatisticsInfoBO.InhouseRoomOrGuest.ToString();
        //            lblExtraAdultsOrChild.Text = roomStatisticsInfoBO.ExtraAdultsOrChild.ToString();
        //            lblInhouseForeigners.Text = roomStatisticsInfoBO.InhouseForeigners.ToString();
        //            lblGuestBlock.Text = roomStatisticsInfoBO.GuestBlock.ToString();
        //            lblAirportPickupDrop.Text = roomStatisticsInfoBO.AirportPickupDrop.ToString();
        //        }
        //    }            
        //}
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtApprovedDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            // this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
        }
        private void SetDefaulCleanTime()
        {
            this.txtProbableCleanTime.Text = "12:00";
        }
        private void LoadExpGroupResvArrivNtftn()
        {
            string todaysdate = DateTime.Now.ToShortDateString();

            List<RoomReservationBO> reservationList = new List<RoomReservationBO>();
            RoomReservationDA roomreservationDA = new RoomReservationDA();
            reservationList = roomreservationDA.GetRoomReservationInfo();
            reservationList = reservationList.Where(a => a.ReservedMode == "Group" && a.ShowDateIn == todaysdate).ToList();
            string msgbody = string.Empty;
            foreach (RoomReservationBO bo in reservationList)
            {
                if (string.IsNullOrEmpty(msgbody))
                {
                    msgbody = bo.ReservationNumber;
                }
                else msgbody += ", " + bo.ReservationNumber;
            }

            UserInformationDA userInformationDA = new UserInformationDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            List<UserInformationBO> userInfoList = new List<UserInformationBO>();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            userInfoList = userInformationDA.GetAllUserInformation();

            if (userInfoList != null)
            {
                if (userInfoList.Count > 0)
                {
                    foreach (UserInformationBO bo in userInfoList)
                    {
                        UserInformationBO participant = new UserInformationBO();
                        participant = userInformationDA.GetUserInformationByEmpId(bo.EmpId);


                        bool IsMessageSendAllGroupUser = false;

                        CommonMessageDA messageDa = new CommonMessageDA();
                        CommonMessageBO message = new CommonMessageBO();
                        CommonMessageDetailsBO detailBO = new CommonMessageDetailsBO();
                        List<CommonMessageDetailsBO> messageDetails = new List<CommonMessageDetailsBO>();

                        message.Subjects = "Group Reservation Arrival Notification";
                        message.MessageBody = "Group Reservation of " + msgbody + " will arrive today.";
                        message.MessageFrom = userInformationBO.UserInfoId;
                        message.MessageFromUserId = userInformationBO.UserId;
                        message.MessageDate = DateTime.Now;
                        message.Importance = "Normal";

                        detailBO.MessageTo = bo.UserInfoId;
                        detailBO.UserId = bo.UserId;
                        messageDetails.Add(detailBO);

                        bool status = messageDa.SaveMessage(message, messageDetails, IsMessageSendAllGroupUser);
                        if (status)
                        {
                            (this.Master as HM).MessageCount();
                        }
                    }
                }
            }
        }
        private void SaveNotificationForEffectiveBenefit()
        {
            UserInformationDA userInformationDA = new UserInformationDA();
            UserInformationBO fromBO = new UserInformationBO();
            UserInformationBO toBO = new UserInformationBO();
            List<UserInformationBO> toList = new List<UserInformationBO>();
            fromBO = hmUtility.GetCurrentApplicationUserInfo();
            toList = userInformationDA.GetAllUserInformation();
            toBO = toList.Where(a => a.UserId == "superadmin").FirstOrDefault();

            BenefitDA benefitDA = new BenefitDA();
            List<PayrollEmpBenefitBO> benList = new List<PayrollEmpBenefitBO>();
            benList = benefitDA.GetEmpEffectiveBenefitInfo(fromBO.EmpId);
            string empName = string.Empty;
            string benefitName = string.Empty;
            foreach (PayrollEmpBenefitBO bo in benList)
            {
                empName = bo.EmpName;

                if (string.IsNullOrEmpty(benefitName))
                    benefitName = bo.BenefitName;
                else benefitName += ", " + bo.BenefitName;
            }

            bool IsMessageSendAllGroupUser = false;

            CommonMessageDA messageDa = new CommonMessageDA();
            CommonMessageBO messageBO = new CommonMessageBO();
            CommonMessageDetailsBO detailBO = new CommonMessageDetailsBO();
            List<CommonMessageDetailsBO> messageDetails = new List<CommonMessageDetailsBO>();

            messageBO.Subjects = "Effective Benefit";
            messageBO.MessageBody = "Today " + empName + " has " + benefitName + " benefits to be effective.";
            messageBO.MessageFrom = fromBO.UserInfoId;
            messageBO.MessageFromUserId = fromBO.UserId;
            messageBO.MessageDate = DateTime.Now;
            messageBO.Importance = "Normal";



            detailBO.MessageTo = toBO.UserInfoId;
            detailBO.UserId = toBO.UserId;
            messageDetails.Add(detailBO);

            messageDa.SaveMessage(messageBO, messageDetails, IsMessageSendAllGroupUser);
        }
        private void GetEmpForGratuityEligibleNotification()
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetEmployeeInfo();
            empList = empList.Where(a => a.ShowProbableGratuityEligibilityDate == DateTime.Now.Date.ToString("d")).ToList();
            string empName = string.Empty;
            foreach (EmployeeBO bo in empList)
            {
                if (string.IsNullOrEmpty(empName))
                    empName = bo.DisplayName;
                else empName += ", " + bo.DisplayName;
            }

            UserInformationDA userInformationDA = new UserInformationDA();
            UserInformationBO fromBO = new UserInformationBO();
            UserInformationBO toBO = new UserInformationBO();
            List<UserInformationBO> toList = new List<UserInformationBO>();
            fromBO = hmUtility.GetCurrentApplicationUserInfo();
            toList = userInformationDA.GetAllUserInformation();
            toBO = toList.Where(a => a.UserId == "superadmin").FirstOrDefault();

            bool IsMessageSendAllGroupUser = false;

            CommonMessageDA messageDa = new CommonMessageDA();
            CommonMessageBO messageBO = new CommonMessageBO();
            CommonMessageDetailsBO detailBO = new CommonMessageDetailsBO();
            List<CommonMessageDetailsBO> messageDetails = new List<CommonMessageDetailsBO>();

            messageBO.Subjects = "Gratuity Eligibility";
            messageBO.MessageBody = "Today is the Gratuity eligibility date of " + empName + ".";
            messageBO.MessageFrom = fromBO.UserInfoId;
            messageBO.MessageFromUserId = fromBO.UserId;
            messageBO.MessageDate = DateTime.Now;
            messageBO.Importance = "Normal";



            detailBO.MessageTo = toBO.UserInfoId;
            detailBO.UserId = toBO.UserId;
            messageDetails.Add(detailBO);

            messageDa.SaveMessage(messageBO, messageDetails, IsMessageSendAllGroupUser);
        }
        private void GetEmpForPFEligibleNotification()
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetEmployeeInfo();
            empList = empList.Where(a => a.ShowProbablePFEligibilityDate == DateTime.Now.Date.ToString("d")).ToList();
            string empName = string.Empty;
            foreach (EmployeeBO bo in empList)
            {
                if (string.IsNullOrEmpty(empName))
                    empName = bo.DisplayName;
                else empName += ", " + bo.DisplayName;
            }

            UserInformationDA userInformationDA = new UserInformationDA();
            UserInformationBO fromBO = new UserInformationBO();
            UserInformationBO toBO = new UserInformationBO();
            List<UserInformationBO> toList = new List<UserInformationBO>();
            fromBO = hmUtility.GetCurrentApplicationUserInfo();
            toList = userInformationDA.GetAllUserInformation();
            toBO = toList.Where(a => a.UserId == "superadmin").FirstOrDefault();

            bool IsMessageSendAllGroupUser = false;

            CommonMessageDA messageDa = new CommonMessageDA();
            CommonMessageBO messageBO = new CommonMessageBO();
            CommonMessageDetailsBO detailBO = new CommonMessageDetailsBO();
            List<CommonMessageDetailsBO> messageDetails = new List<CommonMessageDetailsBO>();

            messageBO.Subjects = "PF Eligibility";
            messageBO.MessageBody = "Today is the PF eligibility date of " + empName + ".";
            messageBO.MessageFrom = fromBO.UserInfoId;
            messageBO.MessageFromUserId = fromBO.UserId;
            messageBO.MessageDate = DateTime.Now;
            messageBO.Importance = "Normal";



            detailBO.MessageTo = toBO.UserInfoId;
            detailBO.UserId = toBO.UserId;
            messageDetails.Add(detailBO);

            messageDa.SaveMessage(messageBO, messageDetails, IsMessageSendAllGroupUser);
        }
        public static string GetReservationInformationView(RoomReservationBO reservationBO)
        {
            HMUtility hmUtility = new HMUtility();
            string strTable = "";
            strTable += "<table width='100%' id='TableReservationInformation' class='table table-bordered table-condensed table-responsive'>";
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
        private static string GetUserDetailHtml(List<GuestInformationBO> registrationDetailListBO)
        {
            string strTable = "";
            strTable += "<table style='width:100%' class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";


            strTable += "<th align='left' scope='col'>Guest Name</th><th align='left' scope='col'>Email</th> <th align='left' scope='col'>Action</th></tr>";
            int counter = 0;



            foreach (GuestInformationBO dr in registrationDetailListBO)
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

                strTable += "<td align='left' style='width: 50%'>" + dr.GuestName + "</td>";
                strTable += "<td align='left' style='width: 30%'>" + dr.GuestEmail + "</td>";
                strTable += "<td align='left' style='width: 20%'>";
                strTable += "&nbsp;<img src='../Images/select.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return PerformViewActionForGuestDetail('" + dr.GuestId + "')\" alt='Edit Information' border='0' />";

                strTable += "</td>";
                strTable += "</tr>";
            }

            // strTable += "<tr style='background-color:#E3EAEB;'>";
            // strTable += "<td align='left' colspan='3' style='width: 100%'>" + GuestTemidPrint + "</td>";
            // strTable += "</tr>";

            strTable += "</table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            return strTable;
            //string strTable = "";
            //strTable += "<table cellspacing='0' cellpadding='4' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";


            //strTable += "<th align='center' scope='col'>Guest Name</th><th align='left' scope='col'>Date Of Birth</th> <th align='left' scope='col'>Email</th> <th align='left' scope='col'>Action</th></tr>";
            //int counter = 0;



            //foreach (GuestInformationBO dr in registrationDetailListBO)
            //{
            //    counter++;
            //    if (counter % 2 == 0)
            //    {
            //        // It's even
            //        strTable += "<tr style='background-color:#E3EAEB;'>";
            //    }
            //    else
            //    {
            //        // It's odd
            //        strTable += "<tr style='background-color:White;'>";
            //    }

            //    strTable += "<td align='left' style='width: 160px'>" + dr.GuestName + "</td>";
            //    strTable += "<td align='left' style='width: 160px'>" + dr.GuestDOB + "</td>";
            //    strTable += "<td align='left' style='width: 160px'>" + dr.GuestEmail + "</td>";
            //    strTable += "<td align='left' style='width: 160px'>";
            //    strTable += "&nbsp;<img src='../Images/edit.png' onClick='javascript:return PerformEditActionForGuestDetail(" + dr.GuestId + ")' alt='Edit Information' border='0' />";
            //    strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformDeleteActionForGuestDetail(" + dr.GuestId + ")' alt='Delete Information' border='0' />";
            //    strTable += "</td>";
            //    strTable += "</tr>";
            //}


            //strTable += "</table>";
            //if (strTable == "")
            //{
            //    strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            //}

            //return strTable;
        }
        public static List<RoomCalenderBO> GetRoomCalenderList(DateTime StartDate, DateTime EndDate)
        {
            List<RoomCalenderBO> calenderList = new List<RoomCalenderBO>();
            RoomCalenderDA calenderDA = new RoomCalenderDA();
            calenderList = calenderDA.GetRoomInfoForCalender(StartDate, EndDate);
            return calenderList;
        }
        
        public static string GetGuestInformationView(List<GuestInformationBO> List)
        {
            string strTable = "";
            strTable += "<table width='100%' id='TableGuestInformation' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";


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
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static string LoadTrainingList()
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            EmpTrainingDA empTrainingDA = new EmpTrainingDA();
            List<EmpTrainingBO> viewList = new List<EmpTrainingBO>();
            viewList = empTrainingDA.GetTrainingListForDepartmentHead(userInformationBO.EmpId);

            string topPart = @"<div class='panel panel-default'><div class='panel-heading'>Upcoming Training List</div>";

            string strTable = string.Empty;
            int counter = 0;

            strTable += "<table id='TrainingList' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='display:none'></th><th align='left' scope='col' style='width: 30%;'>Training Name</th><th align='left' scope='col' style='width: 10%;'>Start Date</th><th align='center' scope='col' style='width: 10%;'>End Date</th><th align='left' scope='col' style='width: 20%;'>Organizer</th><th align='left' scope='col' style='width: 20%;'>Trainer</th><th align='left' scope='col' style='width: 10%;'>Location</th></tr></thead>";
            strTable += "<tbody>";

            foreach (EmpTrainingBO empTraing in viewList)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:White;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }

                strTable += "<td align='left' style=\"display:none;\">" + empTraing.TrainingId + "</td>";
                strTable += "<td align='left' style='width: 30%;'>" + empTraing.TrainingName + "</td>";
                strTable += "<td align='left' style='width: 10%;'>" + empTraing.FromDate + "</td>";
                strTable += "<td align='left' style='width: 10%;'>" + empTraing.ToDate + "</td>";
                strTable += "<td align='left' style='width: 20%;'>" + empTraing.Organizer + "</td>";
                strTable += "<td align='left' style='width: 20%;'>" + empTraing.Trainer + "</td>";
                strTable += "<td align='left' style='width: 10%;'>" + empTraing.Location + "</td>";

                strTable += "</tr>";
            }

            strTable += "</tbody></table></div>";

            string endPart = @"<div style='padding:5px; text-align:right; text-decoration: underline;'> <a href='/Payroll/Reports/frmReportEmpTraining.aspx' style='color:black; font-weight:bold;'>Click for Details</a></div>";

            strTable = topPart + strTable + endPart;
            return strTable;
        }
        [WebMethod]
        public static string LoadLeaveBalance()
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            EmployeeYearlyLeaveDA leaveDA = new EmployeeYearlyLeaveDA();
            List<LeaveTakenNBalanceBO> leaveInformationList = new List<LeaveTakenNBalanceBO>();
            leaveInformationList = leaveDA.GetDepartmentWiseTotalLeaveBalance(userInformationBO.EmpId);

            string topPart = @"<div class='panel panel-default'><div class='panel-heading'>Leave Balance Info</div>";

            string strTable = string.Empty;
            int counter = 0;

            strTable += "<table id='LeaveBalance' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='left' scope='col' style='width: 40%;'>Leave Type</th><th align='left' scope='col' style='width: 20%;'>Total Balance</th><th align='center' scope='col' style='width: 20%;'>Taken</th><th align='left' scope='col' style='width: 20%;'>Remaining</th></tr></thead>";
            strTable += "<tbody>";

            foreach (LeaveTakenNBalanceBO leaveBO in leaveInformationList)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:White;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }

                strTable += "<td align='left' style='width: 40%;'>" + leaveBO.LeaveTypeName + "</td>";
                strTable += "<td align='left' style='width: 20%;'>" + leaveBO.TotalLeave + "</td>";
                strTable += "<td align='left' style='width: 20%;'>" + leaveBO.TotalTakenLeave + "</td>";
                strTable += "<td align='left' style='width: 20%;'>" + leaveBO.RemainingLeave + "</td>";

                strTable += "</tr>";
            }

            strTable += "</tbody></table></div>";

            string endPart = @"<div style='padding:5px; text-align:right; text-decoration: underline;'> <a href='/Payroll/Reports/frmEmpLeaveReport.aspx' style='color:black; font-weight:bold;'>Click for Details</a></div>";

            strTable = topPart + strTable + endPart;

            return strTable;
        }
        [WebMethod]
        public static string ExpectedArrivalList()
        {
            DateTime fromDate = DateTime.Now, toDate = DateTime.Now;
            string reportType = "ArrivalList", guestCompany = "0";

            List<GuestHouseInfoForReportBO> guestHouseInfo = new List<GuestHouseInfoForReportBO>();
            GuestInformationDA guestInfoDa = new GuestInformationDA();
            guestHouseInfo = guestInfoDa.GetGuestHouseInfoForReport(fromDate, toDate, reportType, guestCompany);

            var aa = guestHouseInfo.GroupBy(x => x.RRNumber).ToList();
            List<GuestHouseInfoForReportBO> bb = new List<GuestHouseInfoForReportBO>();
            for (int i = 0; i < aa.Count; i++)
            {
                GuestHouseInfoForReportBO b = new GuestHouseInfoForReportBO();
                b = guestHouseInfo.Where(x => x.RRNumber == aa[i].Key).FirstOrDefault();
                bb.Add(b);
            }

            var topList = bb.Take(5);

            string topPart = @"<div class='panel panel-default'><div class='panel-heading'>Expected Arrival List</div>";

            string strTable = string.Empty;
            int counter = 0;

            strTable += "<table id='LeaveBalance' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='left' scope='col' style='width: 15%;'>Reservation No</th><th align='left' scope='col' style='width: 45%;'>Guest Name</th><th align='left' scope='col' style='width: 40%;'>Company Name</th></tr></thead>";
            strTable += "<tbody>";

            foreach (GuestHouseInfoForReportBO bo in topList)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:White;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }

                strTable += "<td align='left' style='width: 15%;'>" + bo.RRNumber + "</td>";
                strTable += "<td align='left' style='width: 45%;'>" + bo.GuestName + "</td>";
                strTable += "<td align='left' style='width: 40%;'>" + bo.GuestCompany + "</td>";

                strTable += "</tr>";
            }

            strTable += "</tbody></table></div>";
            string endPart = @"<div style='padding:5px; text-align:right; text-decoration: underline;'> <a href='/HotelManagement/Reports/frmReportGuestHouseInfo.aspx' style='color:black; font-weight:bold;'>Click for Details</a></div>";

            strTable = topPart + strTable + endPart;

            return strTable;
        }
        [WebMethod]
        public static string ExpectedDepartureList()
        {
            DateTime fromDate = DateTime.Now, toDate = DateTime.Now;
            string reportType = "ProbablyVacantList", guestCompany = "0";

            List<GuestHouseInfoForReportBO> guestHouseInfo = new List<GuestHouseInfoForReportBO>();
            GuestInformationDA guestInfoDa = new GuestInformationDA();
            guestHouseInfo = guestInfoDa.GetGuestHouseInfoForReport(fromDate, toDate, reportType, guestCompany);

            var topList = guestHouseInfo.Take(5);

            string topPart = @"<div class='panel panel-default'><div class='panel-heading'>Expected Departure List</div>";

            string strTable = string.Empty;
            int counter = 0;

            strTable += "<table id='LeaveBalance' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='left' scope='col' style='width: 30%;'>Guest Name</th><th align='left' scope='col' style='width: 30%;'>Company</th><th align='center' scope='col' style='width: 20%;'>Checked In</th><th align='left' scope='col' style='width: 20%;'>Room No.</th></tr></thead>";
            strTable += "<tbody>";

            foreach (GuestHouseInfoForReportBO bo in topList)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:White;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }

                strTable += "<td align='left' style='width: 30%;'>" + bo.GuestName + "</td>";
                strTable += "<td align='left' style='width: 30%;'>" + bo.GuestCompany + "</td>";
                strTable += "<td align='left' style='width: 20%;'>" + bo.DateIn + "</td>";
                strTable += "<td align='left' style='width: 20%;'>" + bo.RoomNumberList + "</td>";

                strTable += "</tr>";
            }

            strTable += "</tbody></table></div>";
            string endPart = @"<div style='padding:5px; text-align:right; text-decoration: underline;'> <a href='/HotelManagement/Reports/frmReportGuestHouseInfo.aspx' style='color:black; font-weight:bold;'>Click for Details</a></div>";

            strTable = topPart + strTable + endPart;

            return strTable;
        }
        [WebMethod]
        public static string CheckedInList()
        {
            DateTime fromDate = DateTime.Now, toDate = DateTime.Now;
            string reportType = "CheckInList", guestCompany = "0";

            List<GuestHouseInfoForReportBO> guestHouseInfo = new List<GuestHouseInfoForReportBO>();
            GuestInformationDA guestInfoDa = new GuestInformationDA();
            guestHouseInfo = guestInfoDa.GetGuestHouseInfoForReport(fromDate, toDate, reportType, guestCompany);

            var topList = guestHouseInfo.Take(5);

            string topPart = @"<div class='panel panel-default'><div class='panel-heading'>Checked In List</div>";

            string strTable = string.Empty;
            int counter = 0;

            strTable += "<table id='LeaveBalance' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='left' scope='col' style='width: 30%;'>Room No.</th><th align='left' scope='col' style='width: 50%;'>Guest Name</th><th align='center' scope='col' style='width: 20%;'>Checked In</th></tr></thead>";
            strTable += "<tbody>";

            foreach (GuestHouseInfoForReportBO bo in topList)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:White;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }

                strTable += "<td align='left' style='width: 30%;'>" + bo.RoomNumberOrPickInfo + "</td>";
                strTable += "<td align='left' style='width: 50%;'>" + bo.GuestName + "</td>";
                strTable += "<td align='left' style='width: 20%;'>" + bo.DateIn + "</td>";
                //strTable += "<td align='left' style='width: 20%;'>" + bo.DateOut + "</td>";

                strTable += "</tr>";
            }

            strTable += "</tbody></table></div>";
            string endPart = @"<div style='padding:5px; text-align:right; text-decoration: underline;'> <a href='/HotelManagement/Reports/frmReportGuestHouseInfo.aspx' style='color:black; font-weight:bold;'>Click for Details</a></div>";

            strTable = topPart + strTable + endPart;

            return strTable;
        }
        [WebMethod]
        public static string CheckedOutList()
        {
            DateTime fromDate = DateTime.Now, toDate = DateTime.Now;
            string reportType = "CheckOutList", guestCompany = "0";

            List<GuestHouseInfoForReportBO> guestHouseInfo = new List<GuestHouseInfoForReportBO>();
            GuestInformationDA guestInfoDa = new GuestInformationDA();
            guestHouseInfo = guestInfoDa.GetGuestHouseInfoForReport(fromDate, toDate, reportType, guestCompany);

            var topList = guestHouseInfo.Take(5);

            string topPart = @"<div class='panel panel-default'><div class='panel-heading'>Checked Out List</div>";

            string strTable = string.Empty;
            int counter = 0;

            strTable += "<table id='LeaveBalance' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='left' scope='col' style='width: 15%;'>Room No.</th><th align='left' scope='col' style='width: 30%;'>Guest Name</th><th align='left' scope='col' style='width: 25%;'>Company</th><th align='center' scope='col' style='width: 15%;'>Checked In</th><th align='left' scope='col' style='width: 15%;'>Expected Departure</th></tr></thead>";
            strTable += "<tbody>";

            foreach (GuestHouseInfoForReportBO bo in topList)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:White;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }

                strTable += "<td align='left' style='width: 15%;'>" + bo.RoomNumberList + "</td>";
                strTable += "<td align='left' style='width: 30%;'>" + bo.GuestName + "</td>";
                strTable += "<td align='left' style='width: 25%;'>" + bo.GuestCompany + "</td>";
                strTable += "<td align='left' style='width: 15%;'>" + bo.DateIn + "</td>";
                strTable += "<td align='left' style='width: 15%;'>" + bo.DateOut + "</td>";


                strTable += "</tr>";
            }

            strTable += "</tbody></table></div>";
            string endPart = @"<div style='padding:5px; text-align:right; text-decoration: underline;'> <a href='/HotelManagement/Reports/frmReportGuestHouseInfo.aspx' style='color:black; font-weight:bold;'>Click for Details</a></div>";

            strTable = topPart + strTable + endPart;

            return strTable;
        }
        [WebMethod]
        public static string InHouseGuestList()
        {
            DateTime fromDate = DateTime.Now, toDate = DateTime.Now;
            string reportType = "InHouseGustList", guestCompany = "0";

            List<GuestHouseInfoForReportBO> guestHouseInfo = new List<GuestHouseInfoForReportBO>();
            GuestInformationDA guestInfoDa = new GuestInformationDA();
            guestHouseInfo = guestInfoDa.GetGuestHouseInfoForReport(fromDate, toDate, reportType, guestCompany);

            var topList = guestHouseInfo.Take(5);

            string topPart = @"<div class='panel panel-default'><div class='panel-heading'>In House Guest List</div>";

            string strTable = string.Empty;
            int counter = 0;

            strTable += "<table id='LeaveBalance' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='left' scope='col' style='width: 20%;'>Room No.</th><th align='left' scope='col' style='width: 50%;'>Guest Name</th><th align='center' scope='col' style='width: 15%;'>Checked In</th><th align='left' scope='col' style='width: 15%;'>Expected Departure</th></tr></thead>";
            strTable += "<tbody>";

            foreach (GuestHouseInfoForReportBO bo in topList)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:White;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }

                strTable += "<td align='left' style='width: 20%;'>" + bo.RoomNumberOrPickInfo + "</td>";
                strTable += "<td align='left' style='width: 50%;'>" + bo.GuestName + "</td>";
                strTable += "<td align='left' style='width: 15%;'>" + bo.DateIn + "</td>";
                strTable += "<td align='left' style='width: 15%;'>" + bo.DateOut + "</td>";
                //strTable += "<td align='left' style='width: 20%;'>" + bo.UserName + "</td>";

                strTable += "</tr>";
            }

            strTable += "</tbody></table></div>";
            string endPart = @"<div style='padding:5px; text-align:right; text-decoration: underline;'> <a href='/HotelManagement/Reports/frmReportGuestHouseInfo.aspx' style='color:black; font-weight:bold;'>Click for Details</a></div>";

            strTable = topPart + strTable + endPart;

            return strTable;
        }
        [WebMethod]
        public static string AirportPickUpDropList()
        {
            DateTime fromDate = DateTime.Now, toDate = DateTime.Now;

            GuestAirportPickUpDropDA guestAirportPDDA = new GuestAirportPickUpDropDA();
            List<GuestAirportPickUpDropReportViewBO> guestAipportPDBO = new List<GuestAirportPickUpDropReportViewBO>();
            guestAipportPDBO = guestAirportPDDA.GetGuestAirportPickupDropInfo(fromDate, toDate, 0).Where(x => x.TransactionType == "Reservation").ToList();

            var topList = guestAipportPDBO.OrderBy(a => a.FromDate).Take(5);

            string topPart = @"<div class='panel panel-default'><div class='panel-heading'>Airport Pick Up/Drop</div>";

            string strTable = string.Empty;
            int counter = 0;

            strTable += "<table id='LeaveBalance' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='left' scope='col' style='width: 20%;'>Room No.</th><th align='left' scope='col' style='width: 25%;'>Reservation No.</th><th align='left' scope='col' style='width: 25%;'>Guest Name</th><th align='left' scope='col' style='width: 30%;'>Flight Number</th></tr></thead>";
            strTable += "<tbody>";

            foreach (GuestAirportPickUpDropReportViewBO bo in topList)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:White;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }

                strTable += "<td align='left' style='width: 20%;'>" + bo.RoomNumber + "</td>";
                strTable += "<td align='left' style='width: 25%;'>" + bo.TransactionNumber + "</td>";
                strTable += "<td align='left' style='width: 25%;'>" + bo.GuestName + "</td>";
                strTable += "<td align='left' style='width: 30%;'>" + bo.FlightNumber + "</td>";

                strTable += "</tr>";
            }

            strTable += "</tbody></table></div>";
            string endPart = @"<div style='padding:5px; text-align:right; text-decoration: underline;'> <a href='/HotelManagement/Reports/frmReportGuestAirportPickupDropInfo.aspx' style='color:black; font-weight:bold;'>Click for Details</a></div>";

            strTable = topPart + strTable + endPart;

            return strTable;
        }
        [WebMethod]
        public static string VIPGuestList()
        {
            DateTime fromDate = DateTime.Now, toDate = DateTime.Now;
            string guestName = string.Empty, status = "All";
            int countryId = 0, companyId = 0;

            AllReportDA reportDA = new AllReportDA();
            List<VIPGuestReportViewBO> guestList = new List<VIPGuestReportViewBO>();
            guestList = reportDA.GetVIPGuestInfo(fromDate, toDate, guestName, countryId, companyId, status);

            var topList = guestList.Take(5);

            string topPart = @"<div class='panel panel-default'><div class='panel-heading'>VIP Guest List</div>";

            string strTable = string.Empty;
            int counter = 0;

            strTable += "<table id='LeaveBalance' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='left' scope='col' style='width: 20%;'>Room No.</th><th align='left' scope='col' style='width: 30%;'>Guest Name</th><th align='left' scope='col' style='width: 30%;'>Company</th><th align='center' scope='col' style='width: 30%;'>Country</th></tr></thead>";
            strTable += "<tbody>";

            foreach (VIPGuestReportViewBO bo in topList)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:White;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }

                strTable += "<td align='left' style='width: 20%;'>" + bo.RoomNumber + "</td>";
                strTable += "<td align='left' style='width: 30%;'>" + bo.GuestName + "</td>";
                strTable += "<td align='left' style='width: 30%;'>" + bo.CompanyName + "</td>";
                strTable += "<td align='left' style='width: 20%;'>" + bo.CountryName + "</td>";

                strTable += "</tr>";
            }

            strTable += "</tbody></table></div>";
            string endPart = @"<div style='padding:5px; text-align:right; text-decoration: underline;'> <a href='/HotelManagement/Reports/frmReportVIPGuestInfo.aspx' style='color:black; font-weight:bold;'>Click for Details</a></div>";

            strTable = topPart + strTable + endPart;

            return strTable;
        }
        [WebMethod]
        public static string DailyStatisticalReport()
        {
            DateTime reportDate = DateTime.Now;

            AllReportDA reportDA = new AllReportDA();
            List<DailyStatisticalReportViewBO> list = new List<DailyStatisticalReportViewBO>();
            list = reportDA.GetDailyStatisticalReportInfo(reportDate);

            var topList = list.Take(5);

            string topPart = @"<div class='panel panel-default'><div class='panel-heading'>Daily Statistical Report</div>";

            string strTable = string.Empty;
            int counter = 0;

            strTable += "<table id='DailyStatistics' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='left' scope='col' style='width: 60%;'>Details</th><th align='left' scope='col' style='width: 40%;'>Total No.</th></tr></thead>";
            strTable += "<tbody>";

            foreach (DailyStatisticalReportViewBO bo in topList)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:White;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }

                strTable += "<td align='left' style='width: 60%;'>" + bo.Details + "</td>";
                strTable += "<td align='left' style='width: 40%;'>" + bo.TotalNo + "</td>";

                strTable += "</tr>";
            }

            strTable += "</tbody></table></div>";
            //string endPart = @"<div style='padding:5px; text-align:right; text-decoration: underline;'> <a href='/HotelManagement/Reports/frmReportVIPGuestInfo.aspx' style='color:black; font-weight:bold;'>Click for Details</a></div>";

            strTable = topPart + strTable;

            return strTable;
        }
        [WebMethod]
        public static List<LeaveTakenNBalanceBO> LoadDepartmentWiseLeaveBalance()
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            //int departmentId = userInformationBO

            EmployeeYearlyLeaveDA leaveDa = new EmployeeYearlyLeaveDA();
            List<LeaveTakenNBalanceBO> leaveInformationList = new List<LeaveTakenNBalanceBO>();
            leaveInformationList = leaveDa.GetDepartmentWiseTotalLeaveBalance(userInformationBO.EmpId);

            return leaveInformationList;
        }
        [WebMethod]
        public static List<DailyStatisticalReportViewBO> DailyStatisticalPieChartReport()
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            //int departmentId = userInformationBO
            DateTime reportDate = DateTime.Now;
            AllReportDA reportDA = new AllReportDA();
            List<DailyStatisticalReportViewBO> list = new List<DailyStatisticalReportViewBO>();
            list = reportDA.GetDailyStatisticalReportInfo(reportDate);
            //return list;

            List<DailyStatisticalReportViewBO> list5Data = new List<DailyStatisticalReportViewBO>();
            var topList = list.Take(5);

            foreach (DailyStatisticalReportViewBO bo in topList)
            {
                list5Data.Add(bo);
            }

            return list5Data;
        }
        [WebMethod]
        public static List<EmpTypeWiseEmpNoViewBO> LoadEmpTypeWiseEmpNo()
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            EmpTypeDA emptypeDA = new EmpTypeDA();
            List<EmpTypeWiseEmpNoViewBO> viewList = new List<EmpTypeWiseEmpNoViewBO>();
            viewList = emptypeDA.GetEmpTypeWiseEmp(userInformationBO.EmpId);

            return viewList;
        }
        [WebMethod]
        public static string TodaysRoomStatus()
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<RoomNumberBO> roomNumberListBO = new List<RoomNumberBO>();


            List<RoomNumberBO> roomNumberList = new List<RoomNumberBO>();

            RoomTypeDA roomTypeDA = new RoomTypeDA();
            List<RoomTypeBO> RoomTypeInfoList = new List<RoomTypeBO>();
            RoomTypeInfoList = roomTypeDA.GetRoomTypeInfo();

            string strTable = string.Empty;
            string fullContent = string.Empty;

            int RoomVacantDirtyDiv = 0;
            int RoomTodaysCheckInDiv = 0;
            int RoomLongStayingDiv = 0;
            int RoomPossibleVacantDiv = 0;
            int RoomOccupaiedDiv = 0;
            int RoomReservedDiv = 0;
            int RoomVacantDiv = 0;
            int RoomOutOfServiceDiv = 0;
            int RoomOutOfOrderDiv = 0;

            string roomSummary = string.Empty;

            //            string topPart = @"<div class='divClear'>
            //                            </div>
            //                            <div id='SearchPanel' class='block'>
            //                                <a href='#' class='block-heading' data-toggle='collapse'>";

            string topPart = @"<div class='panel panel-default'><div class='panel-heading'>Todays Room Status</div>";
            //string topPart = @"<a href='#page-stats' class='block-heading' data-toggle='collapse'>Todays Room Status</a>";

            string topTemplatePart = @"</a>
                                <div class='panel-body' style = 'overflow-y: scroll; height: 280px;'>           
                                ";
            string groupNamePart = string.Empty;

            string endTemplatePart = @"</div>
                            </div></div>";

            //for (int statusInfo = 0; statusInfo < RoomTypeInfoList.Count; statusInfo++)
            //{
            //roomNumberListBO = roomNumberDA.GetRoomNumberInfoByRoomType(RoomTypeInfoList[statusInfo].RoomTypeId);
            roomNumberListBO = roomNumberDA.GetRoomNumberInfoByRoomType(0);

            string subContent = string.Empty;

            //for (int iRoomNumber = 0; iRoomNumber < roomNumberListBO.Count; iRoomNumber++)
            //{
            //    if (roomNumberListBO[iRoomNumber].StatusId == 3)
            //    {
            //        subContent += @"<div class='DivRoomContainerHeight61'><div class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
            //        RoomOutOfServiceDiv = RoomOutOfServiceDiv + 1;
            //    }
            //    else if (roomNumberListBO[iRoomNumber].StatusId == 2)
            //    {
            //        if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomPossibleVacantDiv")
            //        {
            //            subContent += "<div class='DivRoomContainerHeight61'><div class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
            //            RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;
            //        }
            //        else
            //        {
            //            subContent += "<div class='DivRoomContainerHeight61'><div class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
            //            RoomOccupaiedDiv = RoomOccupaiedDiv + 1;
            //        }
            //    }
            //    else if (roomNumberListBO[iRoomNumber].StatusId == 1)
            //    {
            //        if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomVacantDiv")
            //        {
            //            //string Content1 = "<div class='DivRoomContainerHeight61'><a href='/HotelManagement/frmRoomRegistration.aspx?SelectedRoomNumber=" + roomNumberListBO[iRoomNumber].RoomId + "&source=Registration";
            //            //string Content1 = "<div class='DivRoomContainerHeight61'>";

            //            string Content1 = "<div class='DivRoomContainerHeight61'><a href='/HotelManagement/frmRoomRegistration.aspx?SelectedRoomNumber=" + roomNumberListBO[iRoomNumber].RoomId + "&source=Registration";
            //            string Content2 = @"'><div class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

            //            subContent += Content1 + Content2;
            //            RoomVacantDiv = RoomVacantDiv + 1;
            //        }
            //        else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomVacantDirtyDiv")
            //        {
            //            //string Content1 = "<div class='DivRoomContainer'><a href='/HotelManagement/frmRoomRegistration.aspx?SelectedRoomNumber=" + roomNumberListBO[iRoomNumber].RoomId;
            //            //string Content2 = @"'><div class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'></div></a><div class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

            //            string Content1 = "<div class='DivRoomContainerHeight61'>";
            //            string Content2 = @"<div class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

            //            subContent += Content1 + Content2;
            //            RoomVacantDirtyDiv = RoomVacantDirtyDiv + 1;
            //        }
            //        else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomReservedDiv")
            //        {
            //            string Content1 = "<div class='DivRoomContainerHeight61'> <div class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div>";
            //            string Content2 = @"<div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

            //            subContent += Content1 + Content2;
            //            RoomReservedDiv = RoomReservedDiv + 1;
            //        }
            //    }
            //}

            for (int iRoomNumber = 0; iRoomNumber < roomNumberListBO.Count; iRoomNumber++)
            {
                if (roomNumberListBO[iRoomNumber].StatusId == 4)
                {
                    subContent += @"<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "' onclick='RoomOutOfServiceDiv(" + roomNumberListBO[iRoomNumber].RoomNumber + ")'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                    RoomOutOfServiceDiv = RoomOutOfServiceDiv + 1;
                }
                else if (roomNumberListBO[iRoomNumber].StatusId == 3)
                {
                    subContent += @"<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "' onclick='RoomOutOfOrderDiv(" + roomNumberListBO[iRoomNumber].RoomNumber + ")'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                    RoomOutOfOrderDiv = RoomOutOfOrderDiv + 1;
                }
                else if (roomNumberListBO[iRoomNumber].StatusId == 2)
                {
                    if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomTodaysCheckInDiv")
                    {
                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "' onclick='RoomTodaysCheckInDiv(" + roomNumberListBO[iRoomNumber].RoomNumber + ")'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                        RoomTodaysCheckInDiv = RoomTodaysCheckInDiv + 1;
                    }
                    else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomLongStayingDiv")
                    {
                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "' onclick='RoomTodaysCheckInDiv(" + roomNumberListBO[iRoomNumber].RoomNumber + ")'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                        RoomLongStayingDiv = RoomLongStayingDiv + 1;
                    }
                    else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomPossibleVacantDiv")
                    {
                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "' onclick='RoomPossibleVacantDiv(" + roomNumberListBO[iRoomNumber].RoomNumber + ")'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                        RoomPossibleVacantDiv = RoomPossibleVacantDiv + 1;
                    }
                    else
                    {
                        subContent += "<div class='DivRoomContainerHeight61'><div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "' onclick='RoomOccupaiedDiv(" + roomNumberListBO[iRoomNumber].RoomNumber + ")'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                        RoomOccupaiedDiv = RoomOccupaiedDiv + 1;
                    }
                }
                else if (roomNumberListBO[iRoomNumber].StatusId == 1)
                {
                    if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomVacantDiv")
                    {
                        //string Content1 = "<div class='DivRoomContainerHeight61'><a href='/HotelManagement/frmRoomRegistration.aspx?SelectedRoomNumber=" + roomNumberListBO[iRoomNumber].RoomId + "&source=Registration";
                        //string Content2 = @"'><div class='" + roomNumberListBO[iRoomNumber].CSSClassName + "'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";
                        string Content1 = "<div class='DivRoomContainerHeight61'>";
                        string Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "' onclick='RoomVacantDiv(" + roomNumberListBO[iRoomNumber].RoomNumber + ")'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                        subContent += Content1 + Content2;
                        RoomVacantDiv = RoomVacantDiv + 1;
                    }
                    else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomVacantDirtyDiv")
                    {
                        string Content1 = "<div class='DivRoomContainerHeight61'>";
                        string Content2 = @"<div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "' onclick='RoomVacantDirtyDiv(" + roomNumberListBO[iRoomNumber].RoomNumber + ")'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div></a><div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                        subContent += Content1 + Content2;
                        RoomVacantDirtyDiv = RoomVacantDirtyDiv + 1;
                    }
                    else if (roomNumberListBO[iRoomNumber].CSSClassName == "RoomReservedDiv")
                    {
                        string Content1 = "<div class='DivRoomContainerHeight61'> <div style='background-color:" + roomNumberListBO[iRoomNumber].ColorCodeName + "' class='" + roomNumberListBO[iRoomNumber].CSSClassName + "' onclick='RoomReservedDiv(" + roomNumberListBO[iRoomNumber].RoomNumber + ")'><div style='height:25px; margin-top:17px; text-align:center; color:#fff; font-weight:bold;'> " + roomNumberListBO[iRoomNumber].RoomNumber + " </div></div>";
                        string Content2 = @"<div style='display:none;' class='RoomNumberDiv'>" + roomNumberListBO[iRoomNumber].RoomNumber + "</div></div>";

                        subContent += Content1 + Content2;
                        RoomReservedDiv = RoomReservedDiv + 1;
                    }
                }
            }

            //roomSummary = "Vacant: " + RoomVacantDiv + ", Vacant Dirty: " + RoomVacantDirtyDiv + ", Reserved: " + RoomReservedDiv + ", Today's Checked In: " + RoomTodaysCheckInDiv + ", Occupied: " + RoomOccupaiedDiv + ", Expected Departure: " + RoomPossibleVacantDiv + ", Out of Order: " + RoomOutOfServiceDiv + ".";
            roomSummary = "Vacant: " + RoomVacantDiv + ", Vacant Dirty: " + RoomVacantDirtyDiv + ", Reserved: " + RoomReservedDiv + ", Today's Checked In: " + RoomTodaysCheckInDiv + ", Occupied: " + (RoomOccupaiedDiv + RoomPossibleVacantDiv + RoomTodaysCheckInDiv + RoomLongStayingDiv) + ", Long Staying: " + RoomLongStayingDiv + ", Expected Departure: " + RoomPossibleVacantDiv + ", Out of Order: " + RoomOutOfOrderDiv + ", Out of Service: " + RoomOutOfServiceDiv + ".";

            //groupNamePart =  "Status" + roomSummary;

            groupNamePart = "<div style='border:1px solid black; background-color: lightblue; padding:5px; font-weight: bold;'> " + roomSummary + "</div>";

            fullContent += subContent;
            string endPart = @"<div style='padding:5px; text-align:right; text-decoration: underline;'> <a href='/HotelManagement/frmRoomStatusInfo.aspx' style='color:black; font-weight:bold;'>Click for Details</a></div>";

            //}

            //this.ltlRoomTemplate.InnerHtml = topPart + groupNamePart + topTemplatePart + fullContent + endTemplatePart;

            strTable = topPart + groupNamePart + topTemplatePart + fullContent + endTemplatePart + endPart;
            return strTable;
        }
        [WebMethod]
        public static List<UserDashboardItemMappingBO> GenerateManagement()
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            List<UserDashboardItemMappingBO> boList = new List<UserDashboardItemMappingBO>();
            DashboardDA dashboardDA = new DashboardDA();

            if (userInformationBO != null)
            {
                if (userInformationBO.UserInfoId > 0)
                {
                    //boList = dashboardDA.GetItemList(userInformationBO.UserGroupId);
                    boList = dashboardDA.GetItemList(userInformationBO.UserInfoId);
                }
            }
            return boList;
        }
        [WebMethod]
        public static void SaveManagement(string[] column1, string[] column2)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            List<DashboardManagementBO> list1 = new List<DashboardManagementBO>();
            DashboardManagementBO bo1 = new DashboardManagementBO();

            for (int i = 0; i < column1.Count(); i++)
            {
                if (column1[i] != null)
                {
                    string[] s = column1[i].Split('S');
                    string divName = s[0], itemId = s[1];
                    list1.Add(new DashboardManagementBO() { UserId = userInformationBO.UserInfoId, Panel = 1, DivName = column1[i], ItemId = Convert.ToInt64(itemId) });
                    //list1.Add(new DashboardManagementBO() { UserId = userInformationBO.UserInfoId, Panel = 1, DivName = divName, ItemId = Convert.ToInt64(column1[i].Substring(column1[i].Length - 1, 1)) });
                }
            }

            for (int i = 0; i < column2.Count(); i++)
            {
                if (column2[i] != null)
                {
                    string[] s = column2[i].Split('S');
                    string divName = s[0], itemId = s[1];
                    list1.Add(new DashboardManagementBO() { UserId = userInformationBO.UserInfoId, Panel = 2, DivName = column2[i], ItemId = Convert.ToInt64(itemId) });
                    //list1.Add(new DashboardManagementBO() { UserId = userInformationBO.UserInfoId, Panel = 2, DivName = divName, ItemId = Convert.ToInt64(column2[i].Substring(column2[i].Length - 1, 1)) });
                }
            }

            DashboardDA dashboardDA = new DashboardDA();

            bool deleteStatus = dashboardDA.DeleteDashboardManagement(userInformationBO.UserInfoId);

            bool status = dashboardDA.SaveDashboardManagement(list1);
        }
        [WebMethod]
        public static List<DashboardManagementBO> GetManagement()
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            DashboardDA dashboardDA = new DashboardDA();
            List<DashboardManagementBO> list = new List<DashboardManagementBO>();
            //list = dashboardDA.GetDashboardManagement(userInformationBO.UserGroupId);
            list = dashboardDA.GetDashboardManagement(userInformationBO.UserInfoId);

            return list;
        }
        //Todays Room Status- Room click options
        [WebMethod]
        public static string LoadVacantDirtyPossiblePath(string RoomNember, string PageTitle)
        {
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumber(RoomNember);
            HMCommonDA hmCommonDA = new HMCommonDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            List<RoomStatusPossiblePathViewBO> list = new List<RoomStatusPossiblePathViewBO>();
            list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "VacantDirtyPossiblePath");

            string strTable = string.Empty;
            strTable += "<div style='padding:10px'>";
            for (int i = 0; i < list.Count; i++)
            {
                strTable += "<div style='float:left;padding-left:30px; padding-bottom:30px;'>";
                strTable += "<input type='button' style='width:150px' value='" + list[i].DisplayText + "' class='TransactionalButton btn btn-primary btn-sm'";

                if (list[i].DisplayText == "Details")
                {
                    strTable += " onclick=\"return CountTotalNumberOfGuestByRoomNumber('" + RoomNember + "' );\"  />";
                }
                else
                {
                    strTable += " onclick=\"return LoadCleanUpInfo('" + numberBO.RoomId + "');\"  />";
                }

                strTable += "</div>";
            }

            strTable += "</div>";

            return strTable;
        }
        [WebMethod]
        public static string LoadReservedPossiblePath(string RoomNember, string PageTitle)
        {
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumber(RoomNember);
            HMCommonDA hmCommonDA = new HMCommonDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            List<RoomStatusPossiblePathViewBO> list = new List<RoomStatusPossiblePathViewBO>();
            list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "ReservedPossiblePath");

            RoomCalenderBO calenderBO = GetRoomCalenderList(DateTime.Now, DateTime.Now.AddDays(1)).Where(x => x.RoomId == numberBO.RoomId && x.TransectionStatus == "Reservation").FirstOrDefault();

            string strTable = string.Empty;
            strTable += "<div style='padding:10px'>";
            for (int i = 0; i < list.Count; i++)
            {
                strTable += "<div style='float:left;padding-left:30px; padding-bottom:30px;'>";

                strTable += "<input type='button' style='width:150px' value='" + list[i].DisplayText + "' class='TransactionalButton btn btn-primary btn-sm'";

                if (list[i].DisplayText == "Details")
                {
                    strTable += " onclick=\"return LoadReservationDetails('" + calenderBO.TransectionId + "' );\"  />";
                }
                else if (list[i].DisplayText.Trim() == "Reservation")
                {
                    strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?editId=" + calenderBO.TransectionId + "';\"  />";
                }
                else if (list[i].DisplayText.Trim() == "Registration")
                {
                    strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?SelectedRoomNumber=" + numberBO.RoomId + "&source=Reservation';\"  />";
                }
                else if (list[i].DisplayText.Trim() == "Reservation Payment")
                {
                    strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?rsvnId=" + calenderBO.TransectionId + "&rId=" + numberBO.RoomId + "';\"  />";
                }

                strTable += "</div>";
            }

            strTable += "</div>";

            return strTable;
        }
        [WebMethod(EnableSession = true)]
        public static string LoadOccupiedPossiblePath(string RoomNember, string PageTitle)
        {
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumber(RoomNember);
            HMCommonDA hmCommonDA = new HMCommonDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            List<RoomStatusPossiblePathViewBO> list = new List<RoomStatusPossiblePathViewBO>();
            list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "OccupiedPossiblePath");

            string strTable = string.Empty;
            strTable += "<div style='padding:10px'>";
            for (int i = 0; i < list.Count; i++)
            {
                strTable += "<div style='float:left;padding-left:30px; padding-bottom:30px;'>";
                strTable += "<input type='button' style='width:150px' value='" + list[i].DisplayText + "' class='TransactionalButton btn btn-primary btn-sm'";

                if (list[i].DisplayText == "Details")
                {
                    strTable += " onclick=\"return CountTotalNumberOfGuestByRoomNumber('" + RoomNember + "' );\"  />";
                }
                else if (list[i].DisplayText == "Bill Preview")
                {
                    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                    RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                    roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(RoomNember);
                    if (roomAllocationBO.RoomId > 0)
                    {
                        HMUtility hmUtility = new HMUtility();
                        HttpContext.Current.Session["CheckOutRegistrationIdList"] = roomAllocationBO.RegistrationId.ToString();
                        //string currencyRate = "0.00";
                        //string isIsplite = "0";
                        //string serviceType = "Preview";
                        //string SelectdIndividualTransferedPaymentId = "0";
                        //string SelectdPaymentId = "0";
                        //string SelectdIndividualPaymentId = "0";
                        //string SelectdIndividualServiceId = "0";
                        //string SelectdIndividualRoomId = "0";
                        //string SelectdServiceId = "0";
                        //string SelectdRoomId = "0";
                        string StartDate = hmUtility.GetFromDate();
                        string EndDate = hmUtility.GetToDate();
                        string ddlRegistrationId = roomAllocationBO.RegistrationId.ToString();
                        string txtSrcRegistrationIdList = roomAllocationBO.RegistrationId.ToString();

                        HttpContext.Current.Session["IsBillSplited"] = "0";
                        HttpContext.Current.Session["GuestBillFromDate"] = hmUtility.GetFromDate();
                        HttpContext.Current.Session["GuestBillToDate"] = hmUtility.GetToDate();
                        //InnboardWebUtility.BillPrintPreviewDynamicallyForReport(currencyRate, isIsplite, serviceType, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdIndividualServiceId, SelectdIndividualRoomId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, ddlRegistrationId, txtSrcRegistrationIdList);
                        strTable += " onclick='LoadBillPreview()' />";
                    }
                }
                else
                {
                    strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?RoomId=" + numberBO.RoomId + "';\"  />";
                }

                strTable += "</div>";
            }

            strTable += "</div>";

            return strTable;
        }
        [WebMethod]
        public static string LoadExpectedDeparturePath(string RoomNember, string PageTitle)
        {
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumber(RoomNember);
            HMCommonDA hmCommonDA = new HMCommonDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            List<RoomStatusPossiblePathViewBO> list = new List<RoomStatusPossiblePathViewBO>();
            list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "ExpectedDeparturePath");

            string strTable = string.Empty;
            strTable += "<div style='padding:10px'>";
            for (int i = 0; i < list.Count; i++)
            {
                strTable += "<div style='float:left;padding-left:30px; padding-bottom:30px;'>";
                strTable += "<input type='button' style='width:150px' value='" + list[i].DisplayText + "' class='TransactionalButton btn btn-primary btn-sm'";

                if (list[i].DisplayText == "Details")
                {
                    strTable += " onclick=\"return CountTotalNumberOfGuestByRoomNumber('" + RoomNember + "' );\"  />";
                }
                else if (list[i].DisplayText == "Bill Preview")
                {
                    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                    RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                    roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(RoomNember);
                    if (roomAllocationBO.RoomId > 0)
                    {
                        HMUtility hmUtility = new HMUtility();
                        HttpContext.Current.Session["CheckOutRegistrationIdList"] = roomAllocationBO.RegistrationId.ToString();
                        //string currencyRate = "0.00";
                        //string isIsplite = "0";
                        //string serviceType = "Preview";
                        //string SelectdIndividualTransferedPaymentId = "0";
                        //string SelectdPaymentId = "0";
                        //string SelectdIndividualPaymentId = "0";
                        //string SelectdIndividualServiceId = "0";
                        //string SelectdIndividualRoomId = "0";
                        //string SelectdServiceId = "0";
                        //string SelectdRoomId = "0";
                        string StartDate = hmUtility.GetFromDate();
                        string EndDate = hmUtility.GetToDate();
                        string ddlRegistrationId = roomAllocationBO.RegistrationId.ToString();
                        string txtSrcRegistrationIdList = roomAllocationBO.RegistrationId.ToString();

                        HttpContext.Current.Session["IsBillSplited"] = "0";
                        HttpContext.Current.Session["GuestBillFromDate"] = hmUtility.GetFromDate();
                        HttpContext.Current.Session["GuestBillToDate"] = hmUtility.GetToDate();
                        //InnboardWebUtility.BillPrintPreviewDynamicallyForReport(currencyRate, isIsplite, serviceType, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdIndividualServiceId, SelectdIndividualRoomId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, ddlRegistrationId, txtSrcRegistrationIdList);

                        strTable += " onclick='LoadBillPreview()' />";
                    }
                }
                else
                {
                    strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?RoomId=" + numberBO.RoomId + "';\"  />";
                }

                strTable += "</div>";
            }

            strTable += "</div>";

            return strTable;
        }
        [WebMethod]
        public static string LoadOutOfOrderPossiblePath(string RoomNember, string PageTitle)
        {
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumber(RoomNember);
            HMCommonDA hmCommonDA = new HMCommonDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            List<RoomStatusPossiblePathViewBO> list = new List<RoomStatusPossiblePathViewBO>();
            list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "OutOfOrderPossiblePath");

            string strTable = string.Empty;
            strTable += "<div style='padding:10px'>";
            for (int i = 0; i < list.Count; i++)
            {
                strTable += "<div style='float:left;padding-left:30px; padding-bottom:30px;'>";

                strTable += "<input type='button' style='width:150px' value='" + list[i].DisplayText + "' class='TransactionalButton btn btn-primary btn-sm'";

                if (list[i].DisplayText == "Details")
                {
                    strTable += " onclick=\"return ShowOutOfServiceRoomInformation('" + RoomNember + "' );\"  />";
                }
                else if (list[i].DisplayText == "Room Status Change")
                {
                    strTable += " onclick=\"return LoadCleanUpInfo('" + numberBO.RoomId + "');\"  />";
                }
                else
                {
                    strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?RoomId=" + numberBO.RoomId + "';\"  />";
                }

                strTable += "</div>";
            }

            strTable += "</div>";

            return strTable;
        }
        [WebMethod]
        public static string LoadOutOfServicePossiblePath(string RoomNember, string PageTitle)
        {
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumber(RoomNember);
            HMCommonDA hmCommonDA = new HMCommonDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            List<RoomStatusPossiblePathViewBO> list = new List<RoomStatusPossiblePathViewBO>();
            list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "OutOfOrderPossiblePath");

            string strTable = string.Empty;
            strTable += "<div style='padding:10px'>";
            for (int i = 0; i < list.Count; i++)
            {
                strTable += "<div style='float:left;padding-left:30px; padding-bottom:30px;'>";

                strTable += "<input type='button' style='width:150px' value='" + list[i].DisplayText + "' class='TransactionalButton btn btn-primary btn-sm'";

                if (list[i].DisplayText == "Details")
                {
                    strTable += " onclick=\"return ShowOutOfServiceRoomInformation('" + RoomNember + "' );\"  />";
                }
                else if (list[i].DisplayText == "Room Status Change")
                {
                    strTable += " onclick=\"return LoadCleanUpInfo('" + numberBO.RoomId + "');\"  />";
                }
                else
                {
                    strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?RoomId=" + numberBO.RoomId + "';\"  />";
                }

                strTable += "</div>";
            }

            strTable += "</div>";

            return strTable;
        }
        [WebMethod]
        public static string LoadVacantPossiblePath(string RoomNember, string PageTitle)
        {
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = numberDA.GetRoomInformationByRoomNumber(RoomNember);
            HMCommonDA hmCommonDA = new HMCommonDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            List<RoomStatusPossiblePathViewBO> list = new List<RoomStatusPossiblePathViewBO>();
            list = hmCommonDA.GetRoomStatusPossiblePath(userInformationBO.UserGroupId, "VacantPossiblePath");

            string strTable = string.Empty;
            strTable += "<div style='padding:10px'>";
            for (int i = 0; i < list.Count; i++)
            {
                strTable += "<div style='float:left;padding-left:30px; padding-bottom:30px;'>";

                strTable += "<input type='button' style='width:150px' value='" + list[i].DisplayText + "' class='TransactionalButton btn btn-primary btn-sm'";

                if (list[i].DisplayText == "Room Status Change")
                {
                    strTable += " onclick=\"return LoadCleanUpInfo('" + numberBO.RoomId + "');\"  />";
                }
                else if (list[i].DisplayText.Trim() == "Registration")
                {
                    strTable += " onclick=\" location.href='" + list[i].PossiblePath + "?SelectedRoomNumber=" + numberBO.RoomId + "&source=Registration';\"  />";
                }

                strTable += "</div>";
            }
            strTable += "</div>";
            return strTable;
        }
        [WebMethod]
        public static GuestInformationBO CountTotalNumberOfGuestByRoomNumber(string roomNumer)
        {
            GuestInformationBO guestBO = new GuestInformationBO();
            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumer);
            GuestInformationDA guestDA = new GuestInformationDA();
            int count = guestDA.CountNumberOfGuestByRegistrationId(allocationBO.RegistrationId);
            guestBO.NumberOfGuest = count;
            guestBO.RoomNumber = roomNumer;
            return guestBO;
        }
        [WebMethod]
        public static string GetRegistrationInformationListByRoomNumber(string roomNumer)
        {
            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumer);
            List<GuestInformationBO> list = new List<GuestInformationBO>();
            GuestInformationDA guestDA = new GuestInformationDA();
            list = guestDA.GetGuestInformationByRegistrationId(allocationBO.RegistrationId);
            return GetUserDetailHtml(list);
        }
        [WebMethod]
        public static GuestInformationBO GetRegistrationInformationForSingleGuestByRoomNumber(string roomNumer)
        {
            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(roomNumer);
            List<GuestInformationBO> list = new List<GuestInformationBO>();
            GuestInformationDA guestDA = new GuestInformationDA();
            list = guestDA.GetGuestInformationByRegistrationId(allocationBO.RegistrationId);
            foreach (GuestInformationBO row in list)
            {
                row.RoomRate = allocationBO.RoomRate;
                row.CurrencyTypeHead = allocationBO.CurrencyTypeHead;
                row.ArriveDate = row.ArriveDate;
                row.ExpectedCheckOutDate = allocationBO.ExpectedCheckOutDate;
            }
            return list[0];
        }
        [WebMethod]
        public static string GetDocumentsByUserTypeAndUserId(string GuestId)
        {
            string UserType = "";
            int UserId = 0;
            List<DocumentsBO> docList = new List<DocumentsBO>();
            DocumentsDA docDA = new DocumentsDA();
            docList = docDA.GetDocumentsByUserTypeAndUserId("Guest", Int64.Parse(GuestId));
            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            string strTable = "";
            strTable += "<div style='color: White; background-color: #44545E;width:750px;'>";
            int counter = 0;
            foreach (DocumentsBO dr in docList)
            {
                if (dr.Extention == ".jpg")
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:250px; height:250px; float:left;padding:30px'>";
                    strTable += "<a target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img style='width: 200px; height: 200px;' src='" + ImgSource + "'  alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
                else
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:100px; height:100px; float:left;padding:30px'>";
                    strTable += "<a target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img style='width: 100px; height: 100px;' src='" + dr.IconImage + "' alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
            }
            strTable += "</div>";
            if (strTable == "")
            {
                strTable = "<tr><td align='center'>No Record Available!</td></tr>";
            }
            return strTable;

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
        [WebMethod]
        public static RoomNumberBO ShowOutOfServiceRoomInformation(string roomNumber)
        {
            RoomNumberDA numberDA = new RoomNumberDA();
            RoomNumberBO numberBO = new RoomNumberBO();
            numberBO = numberDA.GetRoomInfoByRoomNumber(roomNumber);
            return numberBO;
        }
        [WebMethod]
        public static RoomNumberBO GetRoomCleanUpInfo(int EditId)
        {
            RoomNumberBO roomNumberList = new RoomNumberBO();
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            roomNumberList = roomNumberDA.GetRoomNumberInfoById(EditId);

            return roomNumberList;
        }
        [WebMethod]
        public static GuestInformationBO PerformViewActionForGuestDetail(int guestId)
        {
            GuestInformationBO guestBO = new GuestInformationBO();
            GuestInformationDA guestDA = new GuestInformationDA();
            guestBO = guestDA.GetGuestInformationByGuestId(guestId);

            RoomAlocationBO allocationBO = new RoomAlocationBO();
            RoomRegistrationBO registrationBO = new RoomRegistrationBO();
            RoomRegistrationDA registrationDA = new RoomRegistrationDA();
            registrationBO = registrationDA.GetGuestLastRegistrationByGuestId(guestId);
            allocationBO = registrationDA.GetActiveRegistrationInfoByRoomNumber(registrationBO.RoomNumber.ToString());
            guestBO.RoomRate = allocationBO.RoomRate;
            guestBO.CurrencyTypeHead = allocationBO.CurrencyTypeHead;
            guestBO.ArriveDate = registrationBO.ArriveDate;
            guestBO.ExpectedCheckOutDate = allocationBO.ExpectedCheckOutDate;
            guestBO.GuestId = registrationBO.GuestId;
            guestBO.RegistrationId = registrationBO.RegistrationId;
            guestBO.PaxInRate = registrationBO.PaxInRate;
            guestBO.CountryName = registrationBO.CountryName;
            return guestBO;
        }
        [WebMethod]
        public static ReturnInfo ChangeRoomStatus(int roomId, string cleanupStatus, string remarks, string fromDate, string toDate, string cleanDate, string cleanTime, string lastCleanDate)
        {
            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            RoomNumberBO roomNumberBO = new RoomNumberBO();
            RoomNumberDA roomNumberDA = new RoomNumberDA();

            roomNumberBO.RoomId = Convert.ToInt32(roomId);

            RoomNumberBO roomNumberList = new RoomNumberBO();
            roomNumberList = roomNumberDA.GetRoomNumberInfoById(roomNumberBO.RoomId);
            roomNumberBO.StatusId = roomNumberList.StatusId;

            string clnTime = cleanTime;
            string[] clntime = clnTime.Split(':');
            int hour = Convert.ToInt32(clntime[0]);
            int min = Convert.ToInt32(clntime[1]);

            if (cleanupStatus == "Cleaned")
            {
                roomNumberBO.CleanupStatus = "Cleaned";
                roomNumberBO.CleanDate = Convert.ToString((hmUtility.GetDateTimeFromString(cleanDate, userInformationBO.ServerDateFormat).AddHours(hour).AddMinutes(min)));
                roomNumberBO.LastCleanDate = Convert.ToString(hmUtility.GetDateTimeFromString(cleanDate, userInformationBO.ServerDateFormat));
                roomNumberBO.FromDate = !string.IsNullOrWhiteSpace(fromDate) ? hmUtility.GetDateTimeFromString(fromDate, userInformationBO.ServerDateFormat) : DateTime.Now;
                roomNumberBO.ToDate = !string.IsNullOrWhiteSpace(toDate) ? hmUtility.GetDateTimeFromString(toDate, userInformationBO.ServerDateFormat) : DateTime.Now;
            }
            else if (cleanupStatus == "Dirty")
            {
                roomNumberBO.CleanupStatus = "Dirty";
                roomNumberBO.CleanDate = Convert.ToString(hmUtility.GetDateTimeFromString(cleanDate, userInformationBO.ServerDateFormat).AddHours(hour).AddMinutes(min));
                if (!string.IsNullOrWhiteSpace(cleanDate))
                {
                    roomNumberBO.LastCleanDate = cleanDate;
                }
                else
                {
                    roomNumberBO.LastCleanDate = DateTime.Now.ToString();
                }
                roomNumberBO.FromDate = !string.IsNullOrWhiteSpace(fromDate) ? hmUtility.GetDateTimeFromString(fromDate, userInformationBO.ServerDateFormat) : DateTime.Now;
                roomNumberBO.ToDate = !string.IsNullOrWhiteSpace(toDate) ? hmUtility.GetDateTimeFromString(toDate, userInformationBO.ServerDateFormat) : DateTime.Now;
            }
            else if (cleanupStatus == "Available")
            {
                roomNumberBO.CleanupStatus = "Available";
                roomNumberBO.FromDate = !string.IsNullOrWhiteSpace(fromDate) ? hmUtility.GetDateTimeFromString(fromDate, userInformationBO.ServerDateFormat) : DateTime.Now;
                roomNumberBO.ToDate = !string.IsNullOrWhiteSpace(toDate) ? hmUtility.GetDateTimeFromString(toDate, userInformationBO.ServerDateFormat) : DateTime.Now;
                roomNumberBO.CleanDate = DateTime.Now.ToString();
                roomNumberBO.LastCleanDate = DateTime.Now.ToString();
            }
            else if (cleanupStatus == "OutOfOrder")
            {
                roomNumberBO.CleanupStatus = "OutOfOrder";
                roomNumberBO.FromDate = !string.IsNullOrWhiteSpace(fromDate) ? hmUtility.GetDateTimeFromString(fromDate, userInformationBO.ServerDateFormat) : DateTime.Now;
                roomNumberBO.ToDate = !string.IsNullOrWhiteSpace(toDate) ? hmUtility.GetDateTimeFromString(toDate, userInformationBO.ServerDateFormat) : DateTime.Now;
                roomNumberBO.CleanDate = DateTime.Now.ToString();
                roomNumberBO.LastCleanDate = DateTime.Now.ToString();
            }
            roomNumberBO.Remarks = remarks;
            roomNumberBO.LastModifiedBy = userInformationBO.UserInfoId;

            Boolean status = roomNumberDA.UpdateRoomCleanInfo(roomNumberBO);
            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
            }
            else
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninf;
        }
        [WebMethod]
        public static string LoadGuestPreferences(int guestId)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            GuestPreferenceDA guestPrefDA = new GuestPreferenceDA();
            List<GuestPreferenceBO> gstPrefList = new List<GuestPreferenceBO>();

            gstPrefList = guestPrefDA.GetGuestPreferenceInfoByGuestId(guestId);
            return hmCommonDA.GetPreferenceListView(gstPrefList);
        }
        [WebMethod]
        public static GuestAirportPickUpDropReportViewBO LoadGuestAirportDrop(int registrationId)
        {
            GuestAirportPickUpDropDA hmCommonDA = new GuestAirportPickUpDropDA();
            List<GuestAirportPickUpDropReportViewBO> gstPrefList = new List<GuestAirportPickUpDropReportViewBO>();
            GuestAirportPickUpDropReportViewBO guestAirportPickUpDropReportViewBO = new GuestAirportPickUpDropReportViewBO();

            gstPrefList = hmCommonDA.GetGuestAirportDropInfoByRegistrationId(registrationId);

            foreach (GuestAirportPickUpDropReportViewBO row in gstPrefList)
            {
                guestAirportPickUpDropReportViewBO = row;
            }

            return guestAirportPickUpDropReportViewBO;
        }
    }
}