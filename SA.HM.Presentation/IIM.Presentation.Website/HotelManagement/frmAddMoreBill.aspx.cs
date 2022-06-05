using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using System.Globalization;
using HotelManagement.Data.HMCommon;
using System.Web.Services;
using HotelManagement.Data;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmAddMoreBill : System.Web.UI.Page
    {
        protected int isMessageBoxEnable = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.chkIsGroupPayment.Checked = false;
                this.LoadCurrentDate();
                this.LoadRoomNumber();

                string queryStringId = Request.QueryString["AddMoreBill"];
                int queryCurrentRegistrationId = 0;
                if (!string.IsNullOrEmpty(queryStringId))
                {
                    queryCurrentRegistrationId = Convert.ToInt32(queryStringId.Split('~')[1]);
                    Session["queryCurrentRegistrationId"] = queryCurrentRegistrationId;
                    this.ddlPaidByRoomId.SelectedValue = queryStringId.Split('~')[0];
                    //--Remove Paid By RoomId from Room Number List----------------------------------------------
                    this.ddlRoomId.Items.Remove(ddlRoomId.Items.FindByValue(this.ddlPaidByRoomId.SelectedValue));

                    if (Session["AddedExtraRoomInformation"] != null)
                    {
                        string totalAddedRoomIdList = Session["AddedExtraRoomInformation"].ToString();                                                
                        List<RoomRegistrationBO> entityBOList = new List<RoomRegistrationBO>();
                        RoomRegistrationDA entityDA = new RoomRegistrationDA();
                        entityBOList = entityDA.GetRoomRegistrationInfoByRegistrationIdList(totalAddedRoomIdList);

                        foreach (RoomRegistrationBO row in entityBOList)
                        {
                            this.ddlRoomId.Items.Remove(ddlRoomId.Items.FindByValue(row.RoomId.ToString()));
                        }
                    }
                }
                else
                {
                    Session["queryCurrentRegistrationId"] = null;
                }

                if (this.ddlPaidByRoomId.SelectedIndex != -1)
                {
                    this.LoadMultipleGuestRoomList(queryCurrentRegistrationId, Convert.ToInt32(queryStringId.Split('~')[0]));
                }

                string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
                if (!string.IsNullOrWhiteSpace(DeleteSuccess))
                {
                    this.isMessageBoxEnable = 2;
                    this.lblMessage.Text = "Data Deleted successfully.";
                }

                this.LoadRelatedInformation();
            }

            this.EntryValidationForSameRoom();

        }
        protected void btnBackToCheckOutForm_Click(object sender, EventArgs e)
        {
            if (Session["AddedExtraRoomInformation"] != null)
            {
                string totalAddedRoomIdList = Session["AddedExtraRoomInformation"].ToString();

                if (!string.IsNullOrWhiteSpace(totalAddedRoomIdList))
                {
                    Session["AddedExtraRoomInformation"] = totalAddedRoomIdList + "," + this.txtAddedRoomId.Value;
                }
                else
                {
                    Session["AddedExtraRoomInformation"] = this.txtAddedRoomId.Value;
                }
            }
            else
            {
                Session["AddedExtraRoomInformation"] = this.txtAddedRoomId.Value.ToString();
            }

            Response.Redirect("../HotelManagement/frmRoomCheckOut.aspx?BackFromServiceBill=" + this.ddlPaidByRoomId.SelectedValue);
        }
        protected void gvGHServiceBill_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvGHServiceBill.PageIndex = e.NewPageIndex;
            //this.LoadGridView();
        }
        protected void gvGHServiceBill_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";

            }
        }
        protected void ddlRoomId_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadRelatedInformation();
            this.EntryValidationForSameRoom();


            //New Developed-----------------------------------------------------
            this.LoadRelatedInformation();
            this.CalculateGrandTotal();
        }
        protected void btnGroupSave_Click(object sender, EventArgs e)
        {
            this.isMessageBoxEnable = 2;
            lblMessage.Text = "Need to Fix------------------------------------.";

            //int counter = 0;
            //foreach (GridViewRow row in gvGroupWiseGuestHouseRoomList.Rows)
            //{
            //    counter = counter + 1;
            //    bool isAccept = ((CheckBox)row.FindControl("CheckBoxAccept")).Checked;
            //    if (isAccept)
            //    {
            //        GuestHouseCheckOutBO guestHouseCheckOutBO = new GuestHouseCheckOutBO();
            //        GuestHouseCheckOutDA guestHouseCheckOutDA = new GuestHouseCheckOutDA();

            //        guestHouseCheckOutBO.RegistrationId = Convert.ToInt32(this.ddlRegistrationId.SelectedValue);

            //        DateTime _CheckOutDate;
            //        if (DateTime.TryParseExact(this.txtPaidDate.Text, "dd/MM/yyyy", null, DateTimeStyles.None, out _CheckOutDate))
            //        {
            //            guestHouseCheckOutBO.CheckOutDate = _CheckOutDate;
            //        }

            //        guestHouseCheckOutBO.PayMode = string.Empty;
            //        guestHouseCheckOutBO.TotalAmount = Convert.ToDecimal(((Label)row.FindControl("lblgvAmount")).Text);

            //        // Need to Fix------------------------------------------------------------------------------------------------
            //        //guestHouseCheckOutBO.VatAmount = Convert.ToDecimal(((Label)row.FindControl("lblgvVatAmount")).Text);
            //        //guestHouseCheckOutBO.DiscountAmount = Convert.ToDecimal(((Label)row.FindControl("lblgvDiscountAmount")).Text);
            //        //guestHouseCheckOutBO.GrandTotal = Convert.ToDecimal(((Label)row.FindControl("lblgvGrandTotal")).Text);
            //        //guestHouseCheckOutBO.ReceivedAmount = Convert.ToDecimal(((Label)row.FindControl("lblgvAmount")).Text);
            //        //guestHouseCheckOutBO.DueAmount = Convert.ToDecimal(0);
            //        guestHouseCheckOutBO.BillPaidBy = Convert.ToInt32(this.ddlPaidByRegistrationId.SelectedValue);

            //        int tmpRoomTypeId = 0;
            //        List<AvailableGuestListBO> entityDetailBOList = new List<AvailableGuestListBO>(); // only for SaveGuestHouseCheckOutInfo send empty value
            //        Boolean status = guestHouseCheckOutDA.SaveGuestHouseCheckOutInfo(guestHouseCheckOutBO, entityDetailBOList, out tmpRoomTypeId);
            //    }
            //}

            //if (gvGroupWiseGuestHouseRoomList.Rows.Count == counter)
            //{
            //    this.isMessageBoxEnable = 2;
            //    lblMessage.Text = "Saved Operation Successfull.";
            //}
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (chkIsGroupPayment.Checked)
            {
                int counter = 0;
                foreach (GridViewRow row in gvGroupWiseGuestHouseRoomList.Rows)
                {
                    counter = counter + 1;
                    bool isAccept = ((CheckBox)row.FindControl("CheckBoxAccept")).Checked;
                    if (isAccept)
                    {
                        string groupRegistrationId = ((Label)row.FindControl("lblGroupRegistrationId")).Text;
                        if (!string.IsNullOrWhiteSpace(this.txtAddedRoomId.Value))
                        {
                            this.txtAddedRoomId.Value = this.txtAddedRoomId.Value + "," + groupRegistrationId;
                        }
                        else
                        {
                            this.txtAddedRoomId.Value = groupRegistrationId;
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(this.txtAddedRoomId.Value))
                {
                    this.txtAddedRoomId.Value = this.txtAddedRoomId.Value + "," + this.ddlRegistrationId.SelectedValue.ToString();
                }
                else
                {
                    this.txtAddedRoomId.Value = this.ddlRegistrationId.SelectedValue.ToString();
                }
            }
            this.isMessageBoxEnable = 2;
            lblMessage.Text = "Add Operation Successfull.";
        }
        //************************ User Defined Function ********************//
        private void EntryValidationForSameRoom()
        {
            if (this.ddlPaidByRoomId.SelectedValue == this.ddlRoomId.SelectedValue)
            {
                this.btnSave.Visible = false;
            }
            else
            {
                this.btnSave.Visible = true;
            }
        }
        private void LoadRoomNumber()
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();

            List<RoomNumberBO> TodaysExpectedCheckOutRoomNumberInfoBO = new List<RoomNumberBO>();
            TodaysExpectedCheckOutRoomNumberInfoBO = roomNumberDA.GetTodaysExpectedCheckOutRoomNumberInfo();

            this.ddlPaidByRoomId.DataSource = TodaysExpectedCheckOutRoomNumberInfoBO;
            this.ddlPaidByRoomId.DataTextField = "RoomNumber";
            this.ddlPaidByRoomId.DataValueField = "RoomId";
            this.ddlPaidByRoomId.DataBind();

            this.ddlRoomId.DataSource = TodaysExpectedCheckOutRoomNumberInfoBO;
            this.ddlRoomId.DataTextField = "RoomNumber";
            this.ddlRoomId.DataValueField = "RoomId";
            this.ddlRoomId.DataBind();

            ListItem itemRoomId = new ListItem();
            itemRoomId.Value = "0";
            itemRoomId.Text = hmUtility.GetDropDownFirstValue();
            this.ddlRoomId.Items.Insert(0, itemRoomId);
        }
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtPaidDate.Text =hmUtility.GetStringFromDateTime( dateTime);
        }
        private void Cancel()
        {
            ////this.ddlRegistrationId.SelectedIndex = 0;
            //this.LoadCurrentDate();
            ////this.ddlServiceId.SelectedIndex = 0;
            //this.txtServiceRate.Text = string.Empty;
            //this.txtServiceQuantity.Text = string.Empty;
            //this.txtDiscountAmount.Text = string.Empty;
            //this.btnSave.Text = "Save";
            //this.ddlRegistrationId.Focus();
        }
        public bool isValidForm()
        {
            bool status = true;
            if (string.IsNullOrWhiteSpace(this.txtPaidDate.Text))
            {
                status = false;
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please provide Paid Date.";
                this.txtPaidDate.Focus();
            }

            return status;

        }
        private void LoadFormByRegistrationNumber()
        {
            // Need to Fix------------------------------------------------------------------
            if (this.ddlRegistrationId.SelectedIndex != -1)
            {
                lblMessage.Text = "";
                //this.pnlMulltipleRoomBillDetails.Visible = true;
                List<GuestHouseCheckOutBO> guestHouseCheckOutBO = new List<GuestHouseCheckOutBO>();
                GuestHouseCheckOutDA guestHouseCheckOutDA = new GuestHouseCheckOutDA();

                guestHouseCheckOutBO = guestHouseCheckOutDA.GetGuestHouseCheckOutInfoByPaidBy(this.ddlRegistrationId.SelectedValue.ToString(), Convert.ToInt32(this.ddlRegistrationId.SelectedValue));
                if (guestHouseCheckOutBO.Count > 0)
                {
                    this.gvExtraRoomDetail.DataSource = guestHouseCheckOutBO;
                    this.gvExtraRoomDetail.DataBind();
                }
                else
                {
                    this.gvExtraRoomDetail.DataSource = null;
                    this.gvExtraRoomDetail.DataBind();
                    //this.pnlMulltipleRoomBillDetails.Visible = false;
                }
            }
            else
            {
                this.gvExtraRoomDetail.DataSource = null;
                this.gvExtraRoomDetail.DataBind();
                //this.pnlMulltipleRoomBillDetails.Visible = false;
            }

            this.CalculateExtraRoomBillAmountTotal();
        }
        //private void LoadRelatedInformation()
        //{
        //    if (ddlPaidByRoomId.SelectedIndex != -1)
        //    {
        //        int roomId = Convert.ToInt32(this.ddlPaidByRoomId.SelectedValue);
        //        this.LoadPaidByRegistrationNumber(roomId);
        //        this.LoadFormByRegistrationNumber();

        //    }

        //    if (ddlRoomId.SelectedIndex != -1)
        //    {
        //        int roomId = Convert.ToInt32(this.ddlRoomId.SelectedValue);
        //        this.LoadRegistrationNumber(roomId);
        //        this.LoadServiceGridView();
        //    }
        //}
        private void LoadPaidByRegistrationNumber(int roomId)
        {
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            this.ddlPaidByRegistrationId.DataSource = roomRegistrationDA.GetRoomRegistrationInfoByRoomId(roomId);
            this.ddlPaidByRegistrationId.DataTextField = "RegistrationNumber";
            this.ddlPaidByRegistrationId.DataValueField = "RegistrationId";
            this.ddlPaidByRegistrationId.DataBind();
        }
        private void LoadRegistrationNumber(int roomId)
        {
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            this.ddlRegistrationId.DataSource = roomRegistrationDA.GetRoomRegistrationInfoByRoomId(roomId);
            this.ddlRegistrationId.DataTextField = "RegistrationNumber";
            this.ddlRegistrationId.DataValueField = "RegistrationId";
            this.ddlRegistrationId.DataBind();
        }
        //private void LoadServiceGridView()
        //{
        //    if (ddlRegistrationId.SelectedIndex != -1)
        //    {
        //        GuestHouseCheckOutDA da = new GuestHouseCheckOutDA();
        //        int registrationId = Convert.ToInt32(this.ddlRegistrationId.SelectedValue);
        //        //GuestHouseCheckOutDetailBO files = da.GetGuestHouseBillSummery(registrationId);
        //        //// Need to Fix------------------------------------------------------------------
        //        ////this.txtTotalAmount.Text = files.Amount.ToString();
        //        ////this.txtVatAmount.Text = files.VatAmount.ToString();
        //        ////this.txtDiscountAmount.Text = files.DiscountAmount.ToString();
        //        ////this.txtGrandTotal.Text = ((files.Amount + files.VatAmount) - files.DiscountAmount).ToString();
        //        List<GuestHouseCheckOutDetailBO> files = da.GetGuestHouseBill(registrationId, "GuestService");
        //        this.CalculateGuestServiceAmountTotal();
        //    }
        //    else
        //    {
        //        //this.txtTotalAmount.Text = "0";
        //        //this.txtDiscountAmount.Text = "0";
        //        //this.txtVatAmount.Text = "0";
        //        //this.txtGrandTotal.Text = "0";
        //    }
        //}
        private void LoadMultipleGuestRoomList(int currentRegistrationId, int roomId)
        {
            GuestHouseCheckOutDA da = new GuestHouseCheckOutDA();
            this.gvGroupWiseGuestHouseRoomList.DataSource = da.GetGuestHouseBillListWiseSummery(currentRegistrationId, roomId);
            this.gvGroupWiseGuestHouseRoomList.DataBind();
        }
        [WebMethod]
        public static string DeleteData(int sEmpId)
        {
            string result = string.Empty;
            HMUtility hMUtility = new HMUtility();
            try
            {
                GuestHouseCheckOutDA hmCommonDA = new GuestHouseCheckOutDA();

                Boolean status = hmCommonDA.DeleteGuestHouseCheckOutInfoById(sEmpId);
                if (status)
                {
                    result = "Success";
                    hMUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),EntityTypeEnum.EntityType.GuestHouseCheckOut.ToString(),sEmpId,ProjectModuleEnum.ProjectModule.FrontOffice.ToString(),hMUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestHouseCheckOut));
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = "Data Deleted Failed.";
                throw ex;
            }

            return result;
        }
        [WebMethod]
        public static GHServiceBillBO FillForm(int EditId)
        {
            GHServiceBillBO ghServiceBillBO = new GHServiceBillBO();
            GHServiceBillDA ghServiceBillDA = new GHServiceBillDA();

            ghServiceBillBO = ghServiceBillDA.GetGuestServiceBillInfoById(EditId);

            return ghServiceBillBO;
        }



        //--New Developed------------------------------------------
        private void LoadRelatedInformation()
        {
            if (ddlPaidByRoomId.SelectedIndex != -1)
            {
                int roomId = Convert.ToInt32(this.ddlPaidByRoomId.SelectedValue);
                this.LoadPaidByRegistrationNumber(roomId);
                this.LoadFormByRegistrationNumber();

            }
            if (ddlRoomId.SelectedIndex != -1)
            {
                int roomId = Convert.ToInt32(this.ddlRoomId.SelectedValue);
                this.LoadRegistrationNumber(roomId);
                this.LoadRoomGridView();
                this.LoadServiceGridView();
                this.LoadRestaurantGridView();
                this.LoadPaymentInformation();
                this.LoadFormByRegistrationNumber();
            }
        }
        private void LoadRoomGridView()
        {
            this.gvRoomDetail.DataSource = null;
            this.gvRoomDetail.DataBind();
            this.txtIndividualRoomDiscountAmount.Text = "0";
            this.txtIndividualRoomVatAmount.Text = "0";
            this.txtIndividualRoomServiceCharge.Text = "0";
            this.txtIndividualRoomGrandTotal.Text = "0";

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (ddlRegistrationId.SelectedIndex != -1)
            {
                GuestHouseCheckOutDA da = new GuestHouseCheckOutDA();
                int registrationId = Convert.ToInt32(this.ddlRegistrationId.SelectedValue);
                List<GuestHouseCheckOutDetailBO> files = da.GetGuestHouseBill(registrationId.ToString(), "GuestRoom", userInformationBO.UserInfoId);

                //foreach (GuestHouseCheckOutDetailBO row in files)
                //{
                //    //decimal roomRate = row.ServiceRate;
                //    //row.ServiceRate = roomRate + row.VatAmount + row.ServiceCharge;
                //    //row.DiscountAmount = roomRate + row.VatAmount + row.ServiceCharge;
                //    //row.TotalAmount = row.ServiceRate + row.VatAmount + row.ServiceCharge - row.DiscountAmount;


                //    decimal roomRate = row.ServiceRate;
                //    row.ServiceRate = roomRate;// +row.VatAmount + row.ServiceCharge;
                //    if (row.TotalAmount > 0)
                //    {
                //        row.DiscountAmount = 0;//roomRate + row.VatAmount + row.ServiceCharge;
                //        row.TotalAmount = row.ServiceRate + row.VatAmount + row.ServiceCharge - row.DiscountAmount;
                //    }
                //    else
                //    {
                //        row.DiscountAmount = (-1) * row.TotalAmount ;//roomRate + row.VatAmount + row.ServiceCharge;
                //        row.TotalAmount = row.ServiceRate + row.VatAmount + row.ServiceCharge - row.DiscountAmount;
                //    }

                //    //if (row.NightAuditApproved.ToString().ToUpper() == "N")
                //    //{
                //    //    RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();

                //    //    roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(Convert.ToInt32(this.ddlRegistrationId.SelectedValue));
                //    //    if (roomRegistrationBO != null)
                //    //    {
                //    //        if (roomRegistrationBO.RegistrationId > 0)
                //    //        {
                //    //            if (roomRegistrationBO.IsCompanyGuest)
                //    //            {
                //    //                //row.ServiceRate = serviceAmountForCompanyGuest;
                //    //                //row.VatAmount = serviceAmountForCompanyGuest;
                //    //                //row.ServiceCharge = serviceAmountForCompanyGuest;
                //    //                //row.TotalAmount = serviceAmountForCompanyGuest;
                //    //                //row.DiscountAmount = row.ServiceRate;

                //    //                decimal roomRate = row.ServiceRate;

                //    //                row.ServiceRate = roomRate + row.VatAmount + row.ServiceCharge;
                //    //                row.DiscountAmount = roomRate + row.VatAmount + row.ServiceCharge;
                                    

                //    //            }
                //    //        }
                //    //    }
                //    //}

                //}

                this.gvRoomDetail.DataSource = files;
                this.gvRoomDetail.DataBind();

                if (files.Count > 0)
                {
                    this.CalculateGuestRoomAmountTotal();
                }
            }
            //else
            //{
            //    this.gvRoomDetail.DataSource = null;
            //    this.gvRoomDetail.DataBind();
            //    this.txtIndividualRoomDiscountAmount.Text = "0";
            //    this.txtIndividualRoomVatAmount.Text = "0";
            //    this.txtIndividualRoomGrandTotal.Text = "0";
            //}
        }
        private void LoadServiceGridView()
        {
            this.gvServiceDetail.DataSource = null;
            this.gvServiceDetail.DataBind();
            this.txtIndividualServiceDiscountAmount.Text = "0";
            this.txtIndividualRoomVatAmount.Text = "0";
            this.txtIndividualServiceServiceCharge.Text = "0";
            this.txtIndividualServiceGrandTotal.Text = "0";

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (ddlRegistrationId.SelectedIndex != -1)
            {
                GuestHouseCheckOutDA da = new GuestHouseCheckOutDA();
                int registrationId = Convert.ToInt32(this.ddlRegistrationId.SelectedValue);
                List<GuestHouseCheckOutDetailBO> files = da.GetGuestHouseBill(registrationId.ToString(), "GuestService", userInformationBO.UserInfoId);

                this.gvServiceDetail.DataSource = files;
                this.gvServiceDetail.DataBind();

                if (files.Count > 0)
                {
                    this.CalculateGuestServiceAmountTotal();
                }
            }
            //else
            //{
            //    this.gvServiceDetail.DataSource = null;
            //    this.gvServiceDetail.DataBind();
            //    this.txtIndividualServiceDiscountAmount.Text = "0";
            //    this.txtIndividualRoomVatAmount.Text = "0";
            //    this.txtIndividualServiceGrandTotal.Text = "0";
            //}
        }
        private void LoadRestaurantGridView()
        {
            //this.gvRestaurantDetail.DataSource = null;
            //this.gvRestaurantDetail.DataBind();
            //this.txtIndividualRestaurantDiscountAmount.Text = "0";
            //this.txtIndividualRestaurantVatAmount.Text = "0";
            //this.txtIndividualRestaurantServiceCharge.Text = "0";
            //this.txtIndividualRestaurantGrandTotal.Text = "0";
            //if (ddlRegistrationId.SelectedIndex != -1)
            //{
            //    GuestHouseCheckOutDA da = new GuestHouseCheckOutDA();
            //    int registrationId = Convert.ToInt32(this.ddlRegistrationId.SelectedValue);
            //    //string registrationId = txtSrcRegistrationIdList.Value;
            //    List<KotBillDetailBO> files = da.GetRestaurantBillForGuestHouseBill(registrationId.ToString());

            //    this.gvRestaurantDetail.DataSource = files;
            //    this.gvRestaurantDetail.DataBind();

            //    if (files.Count > 0)
            //    {
            //        this.CalculateRestaurantAmountTotal();
            //    }
            //}
            ////else
            ////{
            ////    this.gvRestaurantDetail.DataSource = null;
            ////    this.gvRestaurantDetail.DataBind();
            ////    this.txtIndividualRestaurantDiscountAmount.Text = "0";
            ////    this.txtIndividualRestaurantVatAmount.Text = "0";
            ////    this.txtIndividualRestaurantServiceCharge.Text = "0";
            ////    this.txtIndividualRestaurantGrandTotal.Text = "0";
            ////}
        }
        public void CalculateRestaurantAmountTotal()
        {
            //decimal AmtTotal = 0, AmtTmp;
            //decimal AmtDisTotal = 0, AmtDisTmp;
            //decimal AmtVatTotal = 0, AmtVatTmp;
            //decimal AmtSChargeTotal = 0, AmtSChargeTmp;
            //decimal AmtTotalAmount = 0, AmtTotalAmountTmp;

            //for (int i = 0; i < gvRestaurantDetail.Rows.Count; i++)
            //{
            //    AmtTmp = 0;
            //    AmtDisTmp = 0;
            //    AmtVatTmp = 0;
            //    AmtSChargeTmp = 0;
            //    AmtTotalAmountTmp = 0;

            //    if (decimal.TryParse(((Label)gvRestaurantDetail.Rows[i].FindControl("lblServiceRate")).Text, out AmtTmp))
            //        AmtTotal += AmtTmp;
            //    if (decimal.TryParse(((Label)gvRestaurantDetail.Rows[i].FindControl("lblVatAmount")).Text, out AmtVatTmp))
            //        AmtVatTotal += AmtVatTmp;
            //    if (decimal.TryParse(((Label)gvRestaurantDetail.Rows[i].FindControl("lblServiceCharge")).Text, out AmtSChargeTmp))
            //        AmtSChargeTotal += AmtSChargeTmp;
            //    if (decimal.TryParse(((Label)gvRestaurantDetail.Rows[i].FindControl("lblDiscountAmount")).Text, out AmtDisTmp))
            //        AmtDisTotal += AmtDisTmp;
            //    if (decimal.TryParse(((Label)gvRestaurantDetail.Rows[i].FindControl("lblTotalAmount")).Text, out AmtTotalAmountTmp))
            //        AmtTotalAmount += AmtTotalAmountTmp;
            //}

            //this.txtIndividualRestaurantVatAmount.Text = AmtVatTotal.ToString("#0.00");
            //this.txtIndividualRestaurantServiceCharge.Text = AmtSChargeTotal.ToString("#0.00");
            //this.txtIndividualRestaurantDiscountAmount.Text = AmtDisTotal.ToString("#0.00");
            //this.txtIndividualRestaurantGrandTotal.Text = AmtTotalAmount.ToString("#0.00");
        }
        public void CalculateGuestRoomAmountTotal()
        {
            decimal AmtTotal = 0, AmtTmp;
            decimal AmtDisTotal = 0, AmtDisTmp;
            decimal AmtVatTotal = 0, AmtVatTmp;
            decimal AmtSChargeTotal = 0, AmtSChargeTmp;
            decimal AmtTotalAmount = 0, AmtTotalAmountTmp;

            for (int i = 0; i < gvRoomDetail.Rows.Count; i++)
            {
                AmtTmp = 0;
                AmtDisTmp = 0;
                AmtVatTmp = 0;
                AmtSChargeTmp = 0;
                AmtTotalAmountTmp = 0;

                if (decimal.TryParse(((Label)gvRoomDetail.Rows[i].FindControl("lblServiceRate")).Text, out AmtTmp))
                    AmtTotal += AmtTmp;
                if (decimal.TryParse(((Label)gvRoomDetail.Rows[i].FindControl("lblVatAmount")).Text, out AmtVatTmp))
                    AmtVatTotal += AmtVatTmp;
                if (decimal.TryParse(((Label)gvRoomDetail.Rows[i].FindControl("lblServiceCharge")).Text, out AmtSChargeTmp))
                    AmtSChargeTotal += AmtSChargeTmp;
                if (decimal.TryParse(((Label)gvRoomDetail.Rows[i].FindControl("lblDiscountAmount")).Text, out AmtDisTmp))
                    AmtDisTotal += AmtDisTmp;
                if (decimal.TryParse(((Label)gvRoomDetail.Rows[i].FindControl("lblTotalAmount")).Text, out AmtTotalAmountTmp))
                    AmtTotalAmount += AmtTotalAmountTmp;
            }

            this.txtIndividualRoomVatAmount.Text = AmtVatTotal.ToString("#0.00");
            this.txtIndividualRoomServiceCharge.Text = AmtSChargeTotal.ToString("#0.00");
            this.txtIndividualRoomDiscountAmount.Text = AmtDisTotal.ToString("#0.00");
            this.txtIndividualRoomGrandTotal.Text = AmtTotalAmount.ToString("#0.00");
        }
        public void CalculateGuestServiceAmountTotal()
        {
            decimal AmtTotal = 0, AmtTmp;
            decimal AmtDisTotal = 0, AmtDisTmp;
            decimal AmtVatTotal = 0, AmtVatTmp;
            decimal AmtSChargeTotal = 0, AmtSChargeTmp;
            decimal AmtTotalAmount = 0, AmtTotalAmountTmp;

            for (int i = 0; i < gvServiceDetail.Rows.Count; i++)
            {
                AmtTmp = 0;
                AmtDisTmp = 0;
                AmtVatTmp = 0;
                AmtSChargeTmp = 0;
                AmtTotalAmountTmp = 0;

                if (decimal.TryParse(((Label)gvServiceDetail.Rows[i].FindControl("lblServiceRate")).Text, out AmtTmp))
                    AmtTotal += AmtTmp;
                if (decimal.TryParse(((Label)gvServiceDetail.Rows[i].FindControl("lblVatAmount")).Text, out AmtVatTmp))
                    AmtVatTotal += AmtVatTmp;
                if (decimal.TryParse(((Label)gvServiceDetail.Rows[i].FindControl("lblServiceCharge")).Text, out AmtSChargeTmp))
                    AmtSChargeTotal += AmtSChargeTmp;
                if (decimal.TryParse(((Label)gvServiceDetail.Rows[i].FindControl("lblDiscountAmount")).Text, out AmtDisTmp))
                    AmtDisTotal += AmtDisTmp;
                if (decimal.TryParse(((Label)gvServiceDetail.Rows[i].FindControl("lblTotalAmount")).Text, out AmtTotalAmountTmp))
                    AmtTotalAmount += AmtTotalAmountTmp;
            }

            this.txtIndividualServiceVatAmount.Text = AmtVatTotal.ToString("#0.00");
            this.txtIndividualServiceServiceCharge.Text = AmtSChargeTotal.ToString("#0.00");
            this.txtIndividualServiceDiscountAmount.Text = AmtDisTotal.ToString("#0.00");
            this.txtIndividualServiceGrandTotal.Text = AmtTotalAmount.ToString("#0.00");
        }
        public void CalculateExtraRoomBillAmountTotal()
        {
            decimal AmtDisTotal = 0, AmtDisTmp;
            decimal AmtVatTotal = 0, AmtVatTmp;
            decimal AmtSChargeTotal = 0, AmtSChargeTmp;
            decimal AmtTotalAmount = 0, AmtTotalAmountTmp;

            for (int i = 0; i < gvExtraRoomDetail.Rows.Count; i++)
            {
                AmtDisTmp = 0;
                AmtVatTmp = 0;
                AmtSChargeTmp = 0;
                AmtTotalAmountTmp = 0;

                if (decimal.TryParse(((Label)gvExtraRoomDetail.Rows[i].FindControl("lblExtraVatAmount")).Text, out AmtVatTmp))
                    AmtVatTotal += AmtVatTmp;
                if (decimal.TryParse(((Label)gvExtraRoomDetail.Rows[i].FindControl("lblExtraServiceCharge")).Text, out AmtSChargeTmp))
                    AmtSChargeTotal += AmtSChargeTmp;
                if (decimal.TryParse(((Label)gvExtraRoomDetail.Rows[i].FindControl("lblExtraDiscountAmount")).Text, out AmtDisTmp))
                    AmtDisTotal += AmtDisTmp;
                if (decimal.TryParse(((Label)gvExtraRoomDetail.Rows[i].FindControl("lblExtraTotalAmount")).Text, out AmtTotalAmountTmp))
                    AmtTotalAmount += AmtTotalAmountTmp;
            }

            this.txtIndividualExtraRoomVatAmount.Text = AmtVatTotal.ToString("#0.00");
            this.txtIndividualExtraRoomServiceCharge.Text = AmtSChargeTotal.ToString("#0.00");
            this.txtIndividualExtraRoomDiscountAmount.Text = AmtDisTotal.ToString("#0.00");
            this.txtIndividualExtraRoomGrandTotal.Text = AmtTotalAmount.ToString("#0.00");
        }
        public void CalculateGrandTotal()
        {

            // Vat Total Calculation-----------
            decimal calculatedGuestRoomVatTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRoomVatAmount.Text) ? Convert.ToDecimal(this.txtIndividualRoomVatAmount.Text) : 0;
            decimal calculatedGuestServiceVatTotal = !string.IsNullOrWhiteSpace(this.txtIndividualServiceVatAmount.Text) ? Convert.ToDecimal(this.txtIndividualServiceVatAmount.Text) : 0;
            decimal calculatedRestaurantVatTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRestaurantVatAmount.Text) ? Convert.ToDecimal(this.txtIndividualRestaurantVatAmount.Text) : 0;
            decimal calculatedExtraRoomVatTotal = !string.IsNullOrWhiteSpace(this.txtIndividualExtraRoomVatAmount.Text) ? Convert.ToDecimal(this.txtIndividualExtraRoomVatAmount.Text) : 0;

            decimal calculatedVatTotal = calculatedGuestRoomVatTotal + calculatedGuestServiceVatTotal + calculatedRestaurantVatTotal + calculatedExtraRoomVatTotal;
            this.txtVatTotal.Text = calculatedVatTotal.ToString("#0.00");

            // Service Charge Total Calculation-----------
            decimal calculatedGuestRoomServiceChargeTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRoomServiceCharge.Text) ? Convert.ToDecimal(this.txtIndividualRoomServiceCharge.Text) : 0;
            decimal calculatedGuestServiceServiceChargeTotal = !string.IsNullOrWhiteSpace(this.txtIndividualServiceServiceCharge.Text) ? Convert.ToDecimal(this.txtIndividualServiceServiceCharge.Text) : 0;
            decimal calculatedRestaurantServiceChargeTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRestaurantServiceCharge.Text) ? Convert.ToDecimal(this.txtIndividualRestaurantServiceCharge.Text) : 0;
            decimal calculatedExtraRoomServiceChargeTotal = !string.IsNullOrWhiteSpace(this.txtIndividualExtraRoomServiceCharge.Text) ? Convert.ToDecimal(this.txtIndividualExtraRoomServiceCharge.Text) : 0;

            decimal calculatedServiceChargeTotal = calculatedGuestRoomServiceChargeTotal + calculatedGuestServiceServiceChargeTotal + calculatedRestaurantServiceChargeTotal + calculatedExtraRoomServiceChargeTotal;
            this.txtServiceChargeTotal.Text = calculatedServiceChargeTotal.ToString("#0.00");

            // Discount Total Calculation-----------
            decimal calculatedGuestRoomDiscountTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRoomDiscountAmount.Text) ? Convert.ToDecimal(this.txtIndividualRoomDiscountAmount.Text) : 0;
            decimal calculatedGuestServiceDiscountTotal = !string.IsNullOrWhiteSpace(this.txtIndividualServiceDiscountAmount.Text) ? Convert.ToDecimal(this.txtIndividualServiceDiscountAmount.Text) : 0;
            decimal calculatedRestaurantDiscountTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRestaurantDiscountAmount.Text) ? Convert.ToDecimal(this.txtIndividualRestaurantDiscountAmount.Text) : 0;
            decimal calculatedExtraRoomDiscountTotal = !string.IsNullOrWhiteSpace(this.txtIndividualExtraRoomDiscountAmount.Text) ? Convert.ToDecimal(this.txtIndividualExtraRoomDiscountAmount.Text) : 0;

            decimal calculatedDiscountTotal = calculatedGuestRoomDiscountTotal + calculatedGuestServiceDiscountTotal + calculatedRestaurantDiscountTotal + calculatedExtraRoomDiscountTotal;
            this.txtDiscountAmountTotal.Text = calculatedDiscountTotal.ToString("#0.00");

            // Grand Total Calculation-----------
            decimal calculatedGuestRoomTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRoomGrandTotal.Text) ? Convert.ToDecimal(this.txtIndividualRoomGrandTotal.Text) : 0;
            decimal calculatedGuestServiceTotal = !string.IsNullOrWhiteSpace(this.txtIndividualServiceGrandTotal.Text) ? Convert.ToDecimal(this.txtIndividualServiceGrandTotal.Text) : 0;
            decimal calculatedRestaurantTotal = !string.IsNullOrWhiteSpace(this.txtIndividualRestaurantGrandTotal.Text) ? Convert.ToDecimal(this.txtIndividualRestaurantGrandTotal.Text) : 0;
            decimal calculatedExtraRoomTotal = !string.IsNullOrWhiteSpace(this.txtIndividualExtraRoomGrandTotal.Text) ? Convert.ToDecimal(this.txtIndividualExtraRoomGrandTotal.Text) : 0;
            decimal calculatedAdvancePaymentAmountTotal = !string.IsNullOrWhiteSpace(this.txtAdvancePaymentAmount.Text) ? Convert.ToDecimal(this.txtAdvancePaymentAmount.Text) : 0;

            decimal calculatedGrandTotal = (calculatedGuestRoomTotal + calculatedGuestServiceTotal + calculatedRestaurantTotal + calculatedExtraRoomTotal) - calculatedAdvancePaymentAmountTotal;
            this.txtGrandTotal.Text = calculatedGrandTotal.ToString("#0.00");
        }
        private void LoadPaymentInformation()
        {
            if (ddlRegistrationId.SelectedIndex != -1)
            {
                PaymentSummaryBO paymentSummaryBO = new PaymentSummaryBO();
                GuestBillPaymentDA paymentSummaryDA = new GuestBillPaymentDA();
                int registrationId = Convert.ToInt32(this.ddlRegistrationId.SelectedValue);
                paymentSummaryBO = paymentSummaryDA.GetGuestBillPaymentSummaryInfoByRegiId(registrationId);
                //Session["TotalPaymentSummary"] = paymentSummaryBO.TotalPayment;
                this.txtAdvancePaymentAmount.Text = paymentSummaryBO.TotalPayment.ToString();
            }
            else
            {
                this.txtAdvancePaymentAmount.Text = "0";
            }
        }

        protected string GetStringFromDateTime(DateTime dateTime)
        {

            return dateTime.ToString(hmUtility.GetFormat(true));
        }
    }
}