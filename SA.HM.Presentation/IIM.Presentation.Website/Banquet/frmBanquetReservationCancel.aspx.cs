using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Banquet;
using HotelManagement.Entity.Banquet;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Banquet
{
    public partial class frmBanquetReservationCancel : BasePage
    {
        HiddenField innboardMessage;
        protected int isNewAddButtonEnable = 1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.LoadBanquetReservation();
                this.LoadOccassion();
                this.LoadSeatingPlan();
                this.LoadBanquetInfo();
                CheckPermission();
            }
        }
        protected void btnSrcBillNumber_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.txtSrcBillNumber.Text))
            {
                BanquetReservationDA banquetReservationDA = new BanquetReservationDA();
                BanquetReservationBO banquetReservationBO = new BanquetReservationBO();
                List<BanquetReservationBO> banquetReservationBOList = new List<BanquetReservationBO>();

                banquetReservationBO = banquetReservationDA.GetBanquetReservationInfoByReservationNo("All", this.txtSrcBillNumber.Text);
                if (banquetReservationBO != null)
                {
                    if (banquetReservationBO.Id > 0)
                    {
                        if (banquetReservationBO.ActiveStatus == false)
                        {
                            CommonHelper.AlertInfo(innboardMessage, "This reservation is already canceled.", AlertType.Warning);
                            this.pnlBanquerReservationInfo.Visible = false;

                            List<BanquetReservationBO> blankBanquetReservationBOList = new List<BanquetReservationBO>();
                            this.ddlReservationId.DataSource = blankBanquetReservationBOList;
                            this.ddlReservationId.DataTextField = "ReservationNumber";
                            this.ddlReservationId.DataValueField = "Id";
                            this.ddlReservationId.DataBind();

                            ListItem item = new ListItem();
                            item.Value = "0";
                            item.Text = hmUtility.GetDropDownFirstValue();
                            this.ddlReservationId.Items.Insert(0, item);

                            this.txtSrcRegistrationIdList.Value = this.ddlReservationId.SelectedValue;
                        }
                        else
                        {
                            banquetReservationBOList.Add(banquetReservationBO);
                            this.ddlReservationId.DataSource = banquetReservationBOList;
                            this.ddlReservationId.DataTextField = "ReservationNumber";
                            this.ddlReservationId.DataValueField = "Id";
                            this.ddlReservationId.DataBind();

                            this.txtSrcRegistrationIdList.Value = this.ddlReservationId.SelectedValue;

                            this.pnlBanquerReservationInfo.Visible = true;
                            this.FillForm(banquetReservationBO.Id);
                        }
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + " Valid Reservation Number.", AlertType.Warning);
                        this.pnlBanquerReservationInfo.Visible = false;
                    }
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + " Valid Reservation Number.", AlertType.Warning);
                    this.pnlBanquerReservationInfo.Visible = false;
                }
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + " Valid Reservation Number.", AlertType.Warning);
                this.pnlBanquerReservationInfo.Visible = false;
            }
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Int32.Parse(ddlReservationId.SelectedValue) != 0)
            {
                if (string.IsNullOrWhiteSpace(txtReason.Text))
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Reason", AlertType.Warning);
                    return;
                }
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                BanquetReservationBO reservationBO = new BanquetReservationBO();
                BanquetReservationDA reservationDA = new BanquetReservationDA();
                reservationBO.Id = Int64.Parse(ddlReservationId.SelectedValue);
                string ReservationNumber = ddlReservationId.SelectedItem.Text;
                reservationBO.Reason = txtReason.Text;
                reservationBO.Status = "Cancel";
                reservationBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = reservationDA.UpdateBanquetReservationStatusInfo(reservationBO);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.BanquetReservation.ToString(), reservationBO.Id,
                        ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetReservation)+"Cancel");
                    CommonHelper.AlertInfo(innboardMessage, "Reservation Number " + ReservationNumber + " Canceled Successfully.", AlertType.Success);
                    this.Cancel();
                }
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Provide Valid Reservation Number.", AlertType.Warning);
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.Cancel();
        }
        //************************ User Defined Function ********************//
        private void LoadBanquetReservation()
        {
            BanquetReservationDA entityDA = new BanquetReservationDA();
            this.ddlReservationId.DataSource = entityDA.GetAllBanquetReservationInfo();
            this.ddlReservationId.DataTextField = "ReservationNumber";
            this.ddlReservationId.DataValueField = "Id";
            this.ddlReservationId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlReservationId.Items.Insert(0, item);
        }
        private void CheckPermission()
        {
            btnUpdate.Visible = isSavePermission;
        }
        private void LoadOccassion()
        {
            BanquetOccessionTypeBO occessionBO = new BanquetOccessionTypeBO();
            BanquetOccessionTypeDA occessionDA = new BanquetOccessionTypeDA();
            var List = occessionDA.GetAllBanquetTheme();
            this.ddlOccessionTypeId.DataSource = List;
            this.ddlOccessionTypeId.DataTextField = "Name";
            this.ddlOccessionTypeId.DataValueField = "Id";
            this.ddlOccessionTypeId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            //item.Value = "---None---";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlOccessionTypeId.Items.Insert(0, item);
        }
        private void LoadSeatingPlan()
        {
            BanquetSeatingPlanDA planDA = new BanquetSeatingPlanDA();
            var List = planDA.GetBanquetPlanInformation();
            this.ddlSeatingId.DataSource = List;
            this.ddlSeatingId.DataTextField = "Name";
            this.ddlSeatingId.DataValueField = "Id";
            this.ddlSeatingId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            //item.Value = "---None---";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlSeatingId.Items.Insert(0, item);

        }
        private void LoadBanquetInfo()
        {
            //ddlBanquetId
            BanquetInformationBO banquetBO = new BanquetInformationBO();
            BanquetInformationDA banquetDA = new BanquetInformationDA();
            var List = banquetDA.GetAllBanquetInformation();
            this.ddlBanquetId.DataSource = List;
            this.ddlBanquetId.DataTextField = "Name";
            this.ddlBanquetId.DataValueField = "Id";
            this.ddlBanquetId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            //item.Value = "---None---";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBanquetId.Items.Insert(0, item);
        }
        private void FillForm(long ReservationId)
        {
            BanquetReservationBO reservationBO = new BanquetReservationBO();
            BanquetReservationDA reservationDA = new BanquetReservationDA();
            reservationBO = reservationDA.GetBanquetReservationInfoById(ReservationId);

            //txtArriveDate.Text = reservationBO.ArriveDate.ToShortDateString();
            //txtDepartureDate.Text = reservationBO.DepartureDate.ToShortDateString();
            txtArriveDate.Text = hmUtility.GetStringFromDateTime(reservationBO.ArriveDate);
            txtProbableArrivalHour.Text = hmUtility.GetTimeFromDateTime(reservationBO.ArriveDate);

            //this.txtProbableArrivalHour.Text = Convert.ToInt32(reservationBO.ArriveDate.ToString("%h")) == 0 ? "12" : reservationBO.ArriveDate.ToString("%h");
            //this.txtProbableArrivalMinute.Text = reservationBO.ArriveDate.ToString("mm");

            //DateTime StartDateTime = Convert.ToDateTime(reservationBO.ArriveDate);
            //string S = StartDateTime.ToString("tt");
            //this.ddlProbableArrivalAMPM.SelectedIndex = S == "AM" ? 0 : 1;

            txtDepartureDate.Text = hmUtility.GetStringFromDateTime(reservationBO.DepartureDate);
            txtProbableDepartureHour.Text = hmUtility.GetTimeFromDateTime(reservationBO.DepartureDate);

            //this.txtProbableDepartureHour.Text = Convert.ToInt32(reservationBO.DepartureDate.ToString("%h")) == 0 ? "12" : reservationBO.DepartureDate.ToString("%h");
            //this.txtProbableDepartureMinute.Text = reservationBO.DepartureDate.ToString("mm");

            //DateTime DepartDateTime = Convert.ToDateTime(reservationBO.DepartureDate);
            //string D = DepartDateTime.ToString("tt");
            //this.ddlProbableDepartureAMPM.SelectedIndex = D == "AM" ? 0 : 1;

            txtName.Text = reservationBO.Name;
            ddlBanquetId.SelectedValue = reservationBO.BanquetId.ToString();
            if (reservationBO.ReservationMode != "0")
            {
                ddlReservationMode.SelectedValue = reservationBO.ReservationMode.ToString();
            }
            
            ddlOccessionTypeId.SelectedValue = reservationBO.OccessionTypeId.ToString();
            ddlSeatingId.SelectedValue = reservationBO.SeatingId.ToString();
            ddlBanquetId.SelectedValue = reservationBO.BanquetId.ToString();
            txtReason.Text = reservationBO.CancellationReason;

            //txtProbableArrivalHour.Enabled = false;
            //txtProbableArrivalMinute.Enabled = false;
            //ddlProbableArrivalAMPM.Enabled = false;
            //txtProbableDepartureHour.Enabled = false;
            //txtProbableDepartureMinute.Enabled = false;
            //ddlProbableDepartureAMPM.Enabled = false;

            if (ddlReservationMode.SelectedValue == "Company")
            {
                lblName.Text = "Company Name";
            }
            else
            {
                lblName.Text = "Person Name";
            }

            txtArriveDate.Enabled = false;
            txtDepartureDate.Enabled = false;
            txtName.Enabled = false;
            ddlBanquetId.Enabled = false;
            ddlReservationMode.Enabled = false;
            ddlOccessionTypeId.Enabled = false;
            ddlSeatingId.Enabled = false;
            ddlBanquetId.Enabled = false;

            txtProbableArrivalHour.Enabled = false;
            //txtProbableArrivalMinute.Enabled = false;
            //ddlProbableArrivalAMPM.Enabled = false;
            txtProbableDepartureHour.Enabled = false;
            //txtProbableDepartureMinute.Enabled = false;
            //ddlProbableDepartureAMPM.Enabled = false;
        }
        private void Cancel()
        {
            this.txtSrcBillNumber.Text = string.Empty;
            this.txtReason.Text = string.Empty;
            this.pnlBanquerReservationInfo.Visible = false;
        }
    }
}