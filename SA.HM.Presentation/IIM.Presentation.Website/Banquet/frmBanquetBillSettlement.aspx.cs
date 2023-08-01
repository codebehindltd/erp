using HotelManagement.Data;
using HotelManagement.Data.Banquet;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity;
using HotelManagement.Entity.Banquet;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmBanquetBillSettlement : BasePage
    {
        protected bool isSingle = true;
        HiddenField innboardMessage;
        string grandTotalForResettlement = "0.00";
        HMUtility hmUtility = new HMUtility();
        protected int isCompanyProjectPanelEnable = -1;
        protected int isIntegratedGeneralLedgerDiv = 1;
        protected int isBillReSettlement = 0;
        protected int LocalCurrencyId;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            isSingle = hmUtility.GetSingleProjectAndCompany();
            if (!IsPostBack)
            {
                LoadLocalCurrencyId();
                pnlBillPrintPreview.Visible = false;
                HttpContext.Current.Session["BillPreviewCurrencyRateInformation"] = null;
                LoadBank();
                LoadLabelForSalesTotal();
                ClearCommonSessionInformation();
                LoadRegisteredGuestCompanyInfo();
                hfGuestPaymentDetailsInformationDiv.Value = "1";
                string cardValidation = System.Web.Configuration.WebConfigurationManager.AppSettings["CardValidation"].ToString();
                txtCardValidation.Value = cardValidation.ToString();
                LoadRoomNumber(0);
                LoadCurrency();
                LoadIsConversionRateEditable();
                IsLocalCurrencyDefaultSelected();
                LoadCommonDropDownHiddenField();
                LoadAccountHeadInfo();
                LoadEmployeeInfo1();
                LoadEmployeeInfo();
                LoadCompany();
                LoadCompany1();
                GetContactInformationOfClientWithoutCompany();
                GetContactInformationOfClientWithoutCompany1();

                Session["TransactionDetailList"] = null;
                Session["GuestPaymentDetailListForGrid"] = null;

                string bReservationNumber = Request.QueryString["ReSettlement"];
                if (!string.IsNullOrEmpty(bReservationNumber))
                {
                    SrcBillNumberProcessForReSettlement(bReservationNumber);
                }
            }
        }


        private void LoadEmployeeInfo()
        {
            EmployeeDA empDa = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();

            empList = empDa.GetEmployeeInfo();
            ddltxtParticipantFromOffice.DataSource = empList;
            ddltxtParticipantFromOffice.DataTextField = "DisplayName";
            ddltxtParticipantFromOffice.DataValueField = "EmpId";
            ddltxtParticipantFromOffice.DataBind();


            ListItem FirstItem = new ListItem();
            FirstItem.Value = "0";
            FirstItem.Text = hmUtility.GetDropDownFirstValue();
            ddltxtParticipantFromOffice.Items.Insert(0, FirstItem);

        }

        private void LoadEmployeeInfo1()
        {
            EmployeeDA empDa = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();

            empList = empDa.GetEmployeeInfo();
            ddltxtParticipantFromOffice1.DataSource = empList;
            ddltxtParticipantFromOffice1.DataTextField = "DisplayName";
            ddltxtParticipantFromOffice1.DataValueField = "EmpId";
            ddltxtParticipantFromOffice1.DataBind();


            ListItem FirstItem = new ListItem();
            FirstItem.Value = "0";
            FirstItem.Text = hmUtility.GetDropDownFirstValue();
            ddltxtParticipantFromOffice1.Items.Insert(0, FirstItem);

        }

        public void LoadCompany()
        {
            List<GLCompanyBO> companyInfo = new List<GLCompanyBO>();
            GLCompanyDA da = new GLCompanyDA();
            companyInfo = da.GetAllGLCompanyInfo();
            ddlCompany.DataSource = companyInfo;
            ddlCompany.DataTextField = "Name";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();
            ListItem FirstItem = new ListItem();
            FirstItem.Value = "0";
            FirstItem.Text = hmUtility.GetDropDownFirstValue();
            ddlCompany.Items.Insert(0, FirstItem);

        }


        public void LoadCompany1()
        {
            List<GLCompanyBO> companyInfo = new List<GLCompanyBO>();
            GLCompanyDA da = new GLCompanyDA();
            companyInfo = da.GetAllGLCompanyInfo();
            ddlCompany1.DataSource = companyInfo;
            ddlCompany1.DataTextField = "Name";
            ddlCompany1.DataValueField = "CompanyId";
            ddlCompany1.DataBind();
            ListItem FirstItem = new ListItem();
            FirstItem.Value = "0";
            FirstItem.Text = hmUtility.GetDropDownFirstValue();
            ddlCompany1.Items.Insert(0, FirstItem);

        }

        private void GetContactInformationOfClientWithoutCompany()
        {
            List<ContactInformationBO> contactInformation = new List<ContactInformationBO>();

            ContactInformationDA da = new ContactInformationDA();
            List<ContactInformationBO> contact = new List<ContactInformationBO>();
            contact = da.GetContactInformationOfClientWithoutCompany(0);
            ddlParticipantFromClient.DataSource = contact;
            ddlParticipantFromClient.DataTextField = "Name";
            ddlParticipantFromClient.DataValueField = "Id";
            ddlParticipantFromClient.DataBind();
            ListItem FirstItem = new ListItem();
            FirstItem.Value = "0";
            FirstItem.Text = hmUtility.GetDropDownFirstValue();
            ddlParticipantFromClient.Items.Insert(0, FirstItem);
        }


        private void GetContactInformationOfClientWithoutCompany1()
        {
            List<ContactInformationBO> contactInformation = new List<ContactInformationBO>();

            ContactInformationDA da = new ContactInformationDA();
            List<ContactInformationBO> contact = new List<ContactInformationBO>();
            contact = da.GetContactInformationOfClientWithoutCompany(0);
            ddlParticipantFromClient1.DataSource = contact;
            ddlParticipantFromClient1.DataTextField = "Name";
            ddlParticipantFromClient1.DataValueField = "Id";
            ddlParticipantFromClient1.DataBind();
            ListItem FirstItem = new ListItem();
            FirstItem.Value = "0";
            FirstItem.Text = hmUtility.GetDropDownFirstValue();
            ddlParticipantFromClient1.Items.Insert(0, FirstItem);
        }


        [WebMethod]
        public static List<ContactInformationBO> GetContactInformationByCompanyId(int companyId)
        {


            List<ContactInformationBO> contactInformationList = new List<ContactInformationBO>();

            ContactInformationDA DA = new ContactInformationDA();
            List<ContactInformationBO> contactInfo = new List<ContactInformationBO>();

            contactInfo = DA.GetContactInformationByCompanyId(companyId);

            return contactInfo;

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            Response.Redirect("/Banquet/frmBanquetBillSettlement.aspx");
        }
        protected void gvGuestHouseCheckOut_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvGuestHouseCheckOut.PageIndex = e.NewPageIndex;
            //LoadGridView();
        }
        protected void gvGuestHouseCheckOut_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "CmdEdit")
            //{
            //    _BillId = Convert.ToInt32(e.CommandArgument.ToString());
            //    Session["_RoomTypeId"] = _BillId;
            //    FillForm(_BillId);
            //}
            //else if (e.CommandName == "CmdDelete")
            //{
            //    try
            //    {
            //        _BillId = Convert.ToInt32(e.CommandArgument.ToString());
            //        Session["_RoomTypeId"] = _BillId;
            //        DeleteData(_BillId);
            //        Cancel();
            //        LoadGridView();

            //    }
            //    catch
            //    {
            //    }
            //}
        }
        protected void ddlRoomId_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRelatedInformation();
        }
        protected void btnBillPreview_Click(object sender, EventArgs e)
        {
            if (ddlRoomId.SelectedValue != "0")
            {
                //GoToPrintPreviewReport("0");
                //Response.Redirect("/HotelManagement/Reports/frmReportGuestBillInfo.aspx");
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "a Room Number.", AlertType.Warning);
                txtSrcBillNumber.Focus();
            }
        }
        protected void btnSrcBillNumber_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            BanquetReservationBO reservationBO = new BanquetReservationBO();
            BanquetReservationDA reservationDA = new BanquetReservationDA();
            if (string.IsNullOrWhiteSpace(txtSrcBillNumber.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please provide a valid Reservation Number.", AlertType.Warning);
                txtSrcBillNumber.Focus();
                return;
            }

            SrcBillNumberProcess();
            string participantFromOfficeValue = hfparticipantFromOfficeValue.Value;
            reservationBO.PerticipantFromOfficeCommaSeperatedIds = participantFromOfficeValue;

        }


        protected void ddlPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAccountHeadInfo();
        }
        protected void gvIndividualServiceInformationForBillSplit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ((CheckBox)e.Row.FindControl("chkIsSelected")).Checked = true;
            }
        }
        protected void gvGroupServiceInformationForBillSplit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ((CheckBox)e.Row.FindControl("chkIsSelected")).Checked = true;
            }
        }
        protected void btnUpdateReservation_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            BanquetReservationBO reservationBO = new BanquetReservationBO();
            BanquetReservationDA reservationDA = new BanquetReservationDA();

            reservationBO.Id = Int64.Parse(ddlReservationId.SelectedValue);
            reservationBO.CostCenterId = !string.IsNullOrWhiteSpace(hfCostcenterId.Value) ? Convert.ToInt32(hfCostcenterId.Value) : 0;
            reservationBO.DiscountType = hfDiscountType.Value;
            reservationBO.DiscountAmount = !string.IsNullOrWhiteSpace(hfDiscountAmount.Value) ? Convert.ToDecimal(hfDiscountAmount.Value) : 0;
            reservationBO.NumberOfPersonAdult = !string.IsNullOrWhiteSpace(hfNumberOfPersonAdult.Value) ? Convert.ToInt32(hfNumberOfPersonAdult.Value) : 0;
            reservationBO.NumberOfPersonChild = !string.IsNullOrWhiteSpace(hfNumberOfPersonChild.Value) ? Convert.ToInt32(hfNumberOfPersonChild.Value) : 0;

            // -------------------------------------------------------------------------------------------------------------------------------------------
            List<BanquetReservationDetailBO> addList = new List<BanquetReservationDetailBO>();
            // List<BanquetReservationDetailBO> editList = new List<BanquetReservationDetailBO>();
            List<BanquetReservationDetailBO> deleteList = new List<BanquetReservationDetailBO>();
            List<BanquetReservationClassificationDiscountBO> discountLst = new List<BanquetReservationClassificationDiscountBO>();
            List<BanquetReservationClassificationDiscountBO> discountDeletedLst = new List<BanquetReservationClassificationDiscountBO>();

            addList = JsonConvert.DeserializeObject<List<BanquetReservationDetailBO>>(hfSaveObj.Value);
            //editList = JsonConvert.DeserializeObject<List<BanquetReservationDetailBO>>(hfEditObj.Value);
            deleteList = JsonConvert.DeserializeObject<List<BanquetReservationDetailBO>>(hfDeleteObj.Value);
            discountLst = JsonConvert.DeserializeObject<List<BanquetReservationClassificationDiscountBO>>(hfClassificationDiscountSave.Value);
            // -------------------------------------------------------------------------------------------------------------------------------------------

            Boolean status = reservationDA.UpdateBanquetReservationInfoForAddMoreItem(reservationBO, addList, deleteList, Session["arrayDelete"] as ArrayList, discountLst, discountDeletedLst);

            if (status)
            {
                CommonHelper.AlertInfo(innboardMessage, "Item Added Successfully.", AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.BanquetReservation.ToString(), reservationBO.Id,
                    ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BanquetReservation));

                string bReservationNumber = Request.QueryString["ReSettlement"];
                if (!string.IsNullOrEmpty(bReservationNumber))
                {
                    SrcBillNumberProcessForReSettlement(bReservationNumber);
                }
                else
                {
                    SrcBillNumberProcess();
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //if (!IsFrmValid())
            //{
            //    return;
            //}

            if (Int32.Parse(ddlReservationId.SelectedValue) != 0)
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                BanquetReservationBO reservationBO = new BanquetReservationBO();
                BanquetReservationDA reservationDA = new BanquetReservationDA();

                if (Request.QueryString["ReSettlement"] != null)
                { reservationBO.IsBillReSettlement = true; }
                else { reservationBO.IsBillReSettlement = false; }

                reservationBO.Id = Int64.Parse(ddlReservationId.SelectedValue);
                reservationBO.IsBillSettlement = true;
                reservationBO.CreatedBy = userInformationBO.UserInfoId;
                reservationBO.RebateRemarks = txtRebateRemarks.Text;
                reservationBO.Remarks = txtRemarks.Text;

                decimal totalSalesAmount = !string.IsNullOrWhiteSpace(HiddenFieldSalesTotal.Value) ? Convert.ToDecimal(HiddenFieldSalesTotal.Value) : 0;
                decimal grandTotalAmount = !string.IsNullOrWhiteSpace(HiddenFieldGrandTotal.Value) ? Convert.ToDecimal(HiddenFieldGrandTotal.Value) : 0;
                if (totalSalesAmount != grandTotalAmount)
                {
                    GuestCheckOutDiscount(Int32.Parse(ddlReservationId.SelectedValue));
                }

                BanquetReservationBO billBO = new BanquetReservationBO();
                billBO = reservationDA.GetBanquetReservationInfoById(reservationBO.Id);
                if (billBO.Id > 0)
                {
                    reservationBO.DiscountType = billBO.DiscountType;
                    reservationBO.DiscountAmount = billBO.DiscountAmount;
                    reservationBO.CostCenterId = billBO.CostCenterId;
                }

                if (btnSave.Text.Equals("Settlement"))
                {
                    List<GuestBillPaymentBO> guestPaymentDetailList = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : (HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>).Where(p => p.BillNumber == null).ToList();
                    List<GuestBillPaymentBO> deletedPaymentDetailListForGrid = HttpContext.Current.Session["DeletedGuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["DeletedGuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;

                    HMCommonDA hmCommonDA = new HMCommonDA();
                    long tmpSalesId = 0;
                    Boolean status = reservationDA.SettlementBanquetReservationInfo(reservationBO, guestPaymentDetailList, out tmpSalesId);
                    if (status)
                    {
                        bool deleteStatus = false;
                        foreach (var payment in deletedPaymentDetailListForGrid)
                        {
                            deleteStatus = hmCommonDA.DeleteInfoById("HotelGuestBillPayment", "PaymentId", payment.PaymentId);

                        }

                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.BanquetReservation.ToString(), billBO.Id, ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), "Banquet Reservation Bill Information Updated for Bill Settlement");
                        Clear();
                        txtSrcBillNumber.Text = string.Empty;
                        Session["HiddenFieldCompanyPaymentButtonInfo"] = null;
                        CommonHelper.AlertInfo(innboardMessage, "Bill Settlement Successfull.", AlertType.Success);

                        HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                        HMCommonSetupBO setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsSDCIntegrationEnable", "IsSDCIntegrationEnable");
                        string url = "/Banquet/Reports/frmReportReservationBillInfo.aspx?Id=" + reservationBO.Id;
                        if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
                        {
                            if (setUpBO.SetupValue == "1")
                            {
                                url = "/Banquet/Reports/frmReportReservationBillInfo.aspx?Id=" + reservationBO.Id + "&sdc=1";
                            }
                        }
                        string s = "window.open('" + url + "', 'popup_window', 'width=790,height=780,left=300,top=50,resizable=yes');";
                        ClientScript.RegisterStartupScript(GetType(), "script", s, true);
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Settlement Operation Failed.", AlertType.Warning);
                    }
                }
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Provide Valid Reservation Number.", AlertType.Warning);
            }

        }
        //************************ User Defined Function ********************//
        private void SrcBillNumberProcess()
        {
            ClearCommonSessionInformation();
            ClearUnCommonSessionInformation();
            Session["GuestPaymentDetailListForGrid"] = null;
            if (!string.IsNullOrWhiteSpace(txtSrcBillNumber.Text))
            {
                BanquetReservationDA banquetReservationDA = new BanquetReservationDA();
                BanquetReservationBO banquetReservationBO = new BanquetReservationBO();
                List<BanquetReservationBO> banquetReservationBOList = new List<BanquetReservationBO>();

                banquetReservationBO = banquetReservationDA.GetBanquetReservationInfoByReservationNo("Settlement", txtSrcBillNumber.Text);

                if (banquetReservationBO != null)
                {
                    if (banquetReservationBO.Id > 0)
                    {
                        if (banquetReservationBO.DepartureDate.Date > DateTime.Now.Date)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + " Valid Reservation Number for Settlement.", AlertType.Warning);
                            return;
                        }

                        banquetReservationBOList.Add(banquetReservationBO);
                        ddlReservationId.DataSource = banquetReservationBOList;
                        ddlReservationId.DataTextField = "ReservationNumber";
                        ddlReservationId.DataValueField = "Id";
                        ddlReservationId.DataBind();

                        txtSrcRegistrationIdList.Value = ddlReservationId.SelectedValue;
                        txtRemarks.Text = banquetReservationBO.Remarks;


                        hfEventType.Value = banquetReservationBO.EventType.ToString();
                        hfCostcenterId.Value = banquetReservationBO.CostCenterId.ToString();
                        hfReservationId.Value = banquetReservationBO.Id.ToString();
                        hfDiscountType.Value = banquetReservationBO.DiscountType;
                        hfDiscountAmount.Value = banquetReservationBO.DiscountAmount.ToString();
                        txtNumberOfPersonAdult.Text = banquetReservationBO.NumberOfPersonAdult.ToString();
                        txtNumberOfPersonChild.Text = banquetReservationBO.NumberOfPersonChild.ToString();
                        List<int> empIdList = new List<int>();
                        foreach (var employee in banquetReservationBO.PerticipantFromOffice)
                        {
                            empIdList.Add(employee.EmpId);
                        }
                        hfparticipantFromOfficeValue.Value = string.Join(",", empIdList.ToArray());

                        LoadRoomNumber(banquetReservationBO.CostCenterId);
                        LoadRelatedInformation();

                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + " Valid Reservation Number.", AlertType.Warning);
                        Clear();
                    }
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + " Valid Reservation Number.", AlertType.Warning);
                    Clear();
                }
            }
        }
        private void SrcBillNumberProcessForReSettlement(string bReservationNumber)
        {
            ClearCommonSessionInformation();
            ClearUnCommonSessionInformation();
            Session["GuestPaymentDetailListForGrid"] = null;
            if (!string.IsNullOrWhiteSpace(bReservationNumber))
            {
                BanquetReservationDA banquetReservationDA = new BanquetReservationDA();
                BanquetReservationBO banquetReservationBO = new BanquetReservationBO();
                List<BanquetReservationBO> banquetReservationBOList = new List<BanquetReservationBO>();

                banquetReservationBO = banquetReservationDA.GetBanquetReservationInfoByReservationNo("ReSettlement", bReservationNumber);
                if (banquetReservationBO != null)
                {
                    if (banquetReservationBO.Id > 0)
                    {
                        UserInformationBO userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
                        banquetReservationDA.BanquetBillReSettlementLog(banquetReservationBO.Id, userInformationBO.UserInfoId);

                        txtSrcBillNumber.Text = banquetReservationBO.ReservationNumber;
                        banquetReservationBOList.Add(banquetReservationBO);
                        ddlReservationId.DataSource = banquetReservationBOList;
                        ddlReservationId.DataTextField = "ReservationNumber";
                        ddlReservationId.DataValueField = "Id";
                        ddlReservationId.DataBind();

                        txtSrcRegistrationIdList.Value = ddlReservationId.SelectedValue;
                        txtRemarks.Text = banquetReservationBO.Remarks;
                        LoadRelatedInformation();
                        hfCostcenterId.Value = banquetReservationBO.CostCenterId.ToString();
                        hfReservationId.Value = banquetReservationBO.Id.ToString();
                        hfDiscountType.Value = banquetReservationBO.DiscountType;
                        hfDiscountAmount.Value = banquetReservationBO.DiscountAmount.ToString();
                        txtNumberOfPersonAdult.Text = banquetReservationBO.NumberOfPersonAdult.ToString();
                        txtNumberOfPersonChild.Text = banquetReservationBO.NumberOfPersonChild.ToString();
                        isBillReSettlement = 1;

                        grandTotalForResettlement = banquetReservationBO.GrandTotal.ToString("0.00");
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + " Valid Bill Number.", AlertType.Warning);
                        Clear();
                    }
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + " Valid Bill Number.", AlertType.Warning);
                    Clear();
                }
            }

            txtSrcBillNumber.Enabled = false;
            btnSrcBillNumber.Enabled = false;

        }
        private void LoadLabelForSalesTotal()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("Currency", hmUtility.GetDropDownFirstValue());
            if (fields != null)
            {
                if (fields.Count > 1)
                {
                    fields.RemoveAt(0);
                }
            }
            ddlSalesTotalLocal.DataSource = fields;
            ddlSalesTotalLocal.DataTextField = "FieldValue";
            ddlSalesTotalLocal.DataValueField = "FieldId";
            ddlSalesTotalLocal.DataBind();
            ddlSalesTotalLocal.SelectedIndex = 0;

            lblSalesTotalLocal.Text = "Sales Amount";
            lblGrandTotalLocal.Text = "Grand Total";

            ddlSalesTotalUsd.DataSource = fields;
            ddlSalesTotalUsd.DataTextField = "FieldValue";
            ddlSalesTotalUsd.DataValueField = "FieldId";
            ddlSalesTotalUsd.DataBind();
            ddlSalesTotalUsd.SelectedIndex = 1;
            lblSalesTotalUsd.Text = "Sales Amount (" + ddlSalesTotalUsd.SelectedItem.Text + ")";
            lblGrandTotalUsd.Text = "Grand Total (" + ddlSalesTotalUsd.SelectedItem.Text + ")";
        }
        private void LoadBank()
        {
            BankDA bankDA = new BankDA();
            List<BankBO> entityBOList = new List<BankBO>();
            entityBOList = bankDA.GetBankInfo();

            ddlBankId.DataSource = entityBOList;
            ddlBankId.DataTextField = "BankName";
            ddlBankId.DataValueField = "BankId";
            ddlBankId.DataBind();

            ddlChequeBankId.DataSource = entityBOList;
            ddlChequeBankId.DataTextField = "BankName";
            ddlChequeBankId.DataValueField = "BankId";
            ddlChequeBankId.DataBind();

            ddlMBankingBankId.DataSource = entityBOList;
            ddlMBankingBankId.DataTextField = "BankName";
            ddlMBankingBankId.DataValueField = "BankId";
            ddlMBankingBankId.DataBind();

            ListItem itemBank = new ListItem
            {
                Value = "0",
                Text = hmUtility.GetDropDownFirstValue()
            };
            ddlBankId.Items.Insert(0, itemBank);
            ddlChequeBankId.Items.Insert(0, itemBank);
            ddlMBankingBankId.Items.Insert(0, itemBank);
        }
        private void LoadRegisteredGuestCompanyInfo()
        {
            GuestCompanyDA companyDa = new GuestCompanyDA();
            ddlCompanyName.DataSource = companyDa.GetGuestCompanyInfo();
            ddlCompanyName.DataTextField = "CompanyName";
            ddlCompanyName.DataValueField = "NodeId";
            ddlCompanyName.DataBind();

            ListItem itemCompanyName = new ListItem
            {
                Value = "0",
                Text = hmUtility.GetDropDownFirstValue()
            };
            ddlCompanyName.Items.Insert(0, itemCompanyName);
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;
        }
        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("LocalNUsd");
            ddlCurrency.DataSource = currencyListBO;
            ddlCurrency.DataTextField = "CurrencyName";
            ddlCurrency.DataValueField = "CurrencyId";
            ddlCurrency.DataBind();

            ListItem item = new ListItem
            {
                Value = "0",
                Text = hmUtility.GetDropDownFirstValue()
            };
            ddlCurrency.Items.Insert(0, item);
        }
        private void LoadIsConversionRateEditable()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsConversionRateEditable", "IsConversionRateEditable");

            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        txtConversionRate.ReadOnly = true;
                        //isConversionRateEditable = true;
                    }
                    else
                    {
                        txtConversionRate.ReadOnly = false;
                        //isConversionRateEditable = false;
                    }
                }
            }
        }
        private void IsLocalCurrencyDefaultSelected()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsLocalCurrencyDefaultSelected", "IsLocalCurrencyDefaultSelected");

            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        CommonCurrencyDA headDA = new CommonCurrencyDA();
                        List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
                        currencyListBO = headDA.GetConversionHeadInfoByType("All");

                        CommonCurrencyBO currencyBO = currencyListBO.Where(x => x.CurrencyType == "Local").SingleOrDefault();
                        ddlCurrency.SelectedValue = currencyBO.CurrencyId.ToString();
                    }
                }
            }
        }
        private void LoadLocalCurrencyId()
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetLocalCurrencyInfo();

            LocalCurrencyId = commonCurrencyBO.CurrencyId;
            hfLocalCurrencyId.Value = commonCurrencyBO.CurrencyId.ToString();
        }
        private void LoadReservationNumber(int roomId)
        {
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<RoomRegistrationBO> entityBOList = new List<RoomRegistrationBO>();
            entityBOList = roomRegistrationDA.GetRoomRegistrationInfoByRoomId(roomId);
            txtSrcRegistrationIdList.Value = ddlReservationId.SelectedValue.ToString();

            if (!string.IsNullOrWhiteSpace(ddlReservationId.SelectedValue))
            {
                GuestCompanyBO guestCompanyInfo = new GuestCompanyBO();
                GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
                guestCompanyInfo = guestCompanyDA.GetGuestCompanyInfoByRegistrationId(Convert.ToInt32(ddlReservationId.SelectedValue));
                if (guestCompanyInfo != null)
                {
                    hfGuestCompanyInformation.Value = guestCompanyInfo.CompanyName;
                }
            }


        }
        private void LoadRoomNumber(int costCenterId)
        {
            ListItem itemRoom = new ListItem
            {
                Value = "0",
                Text = hmUtility.GetDropDownFirstValue()
            };

            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<RoomNumberBO> roomInfo = roomNumberDA.GetRoomNumberInfoWithStopChargePosting(costCenterId);

            ddlRoomId.DataSource = roomInfo;
            ddlRoomId.DataTextField = "RoomNumber";
            ddlRoomId.DataValueField = "RoomId";
            ddlRoomId.DataBind();
            ddlRoomId.Items.Insert(0, itemRoom);

            ddlPaidByRegistrationId.DataSource = roomInfo;
            ddlPaidByRegistrationId.DataTextField = "RoomNumber";
            ddlPaidByRegistrationId.DataValueField = "RoomId";
            ddlPaidByRegistrationId.DataBind();
            ddlPaidByRegistrationId.Items.Insert(0, itemRoom);
        }
        private void LoadItemAndRequisitesGridView()
        {
            if (ddlReservationId.SelectedIndex != -1)
            {
                BanquetBillPaymentDA banquetDa = new BanquetBillPaymentDA();
                List<BanquetReservationBillGenerateReportBO> BanquetReservationBill = new List<BanquetReservationBillGenerateReportBO>();
                List<BanquetReservationBillGenerateReportBO> BanquetItemDetail = new List<BanquetReservationBillGenerateReportBO>();
                List<BanquetReservationBillGenerateReportBO> BanquetRequisitesDetail = new List<BanquetReservationBillGenerateReportBO>();

                BanquetReservationBill = banquetDa.GetBanquetReservationBillGenerateReport(Convert.ToInt32(ddlReservationId.SelectedValue));
                if (BanquetReservationBill != null)
                {
                    if (BanquetReservationBill.Count > 0)
                    {
                        BanquetItemDetail = BanquetReservationBill.Where(test => test.ItemType != "Requisites" & test.ItemType != "Payments" & test.ItemName.Trim().Length != 0).ToList();
                        BanquetRequisitesDetail = BanquetReservationBill.Where(test => test.ItemType == "Requisites" & test.ItemName.Trim().Length != 0).ToList();

                        gvItemDetail.DataSource = BanquetItemDetail;
                        gvItemDetail.DataBind();

                        gvRequisitesDetail.DataSource = BanquetRequisitesDetail;
                        gvRequisitesDetail.DataBind();

                        // Service Charge Total Calculation-----------
                        decimal calculatedServiceChargeTotal = BanquetReservationBill[0].ServiceCharge;
                        txtServiceChargeTotal.Text = calculatedServiceChargeTotal.ToString("#0.00");


                        // City/ SD Charge Total Calculation-----------
                        decimal calculatedCitySDChargeTotal = BanquetReservationBill[0].CitySDCharge;
                        txtSDCharge.Text = calculatedCitySDChargeTotal.ToString("#0.00");


                        // Vat Total Calculation-----------
                        decimal calculatedVatTotal = BanquetReservationBill[0].VatAmount;
                        txtVatTotal.Text = calculatedVatTotal.ToString("#0.00");

                        // Additional Charge Total Calculation-----------
                        decimal calculatedAdditionalChargeTotal = BanquetReservationBill[0].AdditionalCharge;
                        txtAdditionalCharge.Text = calculatedAdditionalChargeTotal.ToString("#0.00");

                        // Discount Total Calculation-----------
                        decimal calculatedDiscountTotal = BanquetReservationBill[0].CalculatedDiscountAmount;
                        txtDiscountAmountTotal.Text = calculatedDiscountTotal.ToString("#0.00");
                        txtDiscountAmount.Text = "0.00";

                        decimal calculatedGuestAdvanceAmount = !string.IsNullOrWhiteSpace(hfAdvancePaymentAmount.Value) ? Convert.ToDecimal(hfAdvancePaymentAmount.Value) : 0;

                        ////// Sales Total Calculation-----------
                        ////txtSalesTotal.Text = Math.Round(BanquetReservationBill[0].TotalAmount - calculatedGuestAdvanceAmount - calculatedDiscountTotal).ToString("#0.00");
                        ////HiddenFieldSalesTotal.Value = Math.Round(BanquetReservationBill[0].TotalAmount - calculatedGuestAdvanceAmount - calculatedDiscountTotal).ToString("#0.00");

                        ////txtGrandTotal.Text = Math.Round(BanquetReservationBill[0].TotalAmount - calculatedGuestAdvanceAmount - calculatedDiscountTotal).ToString("#0.00");
                        ////HiddenFieldGrandTotal.Value = Math.Round(BanquetReservationBill[0].TotalAmount - calculatedGuestAdvanceAmount - calculatedDiscountTotal).ToString("#0.00");

                        //// Sales Total Calculation-----------
                        //txtSalesTotal.Text = Math.Round(BanquetReservationBill[0].GrandTotal - calculatedGuestAdvanceAmount - calculatedDiscountTotal).ToString("#0.00");
                        //HiddenFieldSalesTotal.Value = Math.Round(BanquetReservationBill[0].GrandTotal - calculatedGuestAdvanceAmount - calculatedDiscountTotal).ToString("#0.00");

                        //txtGrandTotal.Text = Math.Round(BanquetReservationBill[0].GrandTotal - calculatedGuestAdvanceAmount - calculatedDiscountTotal).ToString("#0.00");
                        //HiddenFieldGrandTotal.Value = Math.Round(BanquetReservationBill[0].GrandTotal - calculatedGuestAdvanceAmount - calculatedDiscountTotal).ToString("#0.00");

                        txtSalesTotal.Text = Math.Round(BanquetReservationBill[0].GrandTotal - calculatedGuestAdvanceAmount).ToString("#0.00");
                        HiddenFieldSalesTotal.Value = Math.Round(BanquetReservationBill[0].GrandTotal - calculatedGuestAdvanceAmount).ToString("#0.00");

                        txtGrandTotal.Text = Math.Round(BanquetReservationBill[0].GrandTotal - calculatedGuestAdvanceAmount).ToString("#0.00");
                        HiddenFieldGrandTotal.Value = Math.Round(BanquetReservationBill[0].GrandTotal - calculatedGuestAdvanceAmount).ToString("#0.00");

                        txtBanquetId.Text = BanquetReservationBill[0].BanquetName;
                        txtBanquetRate.Text = (BanquetReservationBill[0].BanquetRate).ToString("#0.00");
                        txtSeatingId.Text = BanquetReservationBill[0].SeatingName;

                        //--Remove Paid By RoomId from Room Number List----------------------------------------------
                        if (ddlPayMode.SelectedIndex != -1)
                        {
                            ddlPayMode.Items.Remove(ddlPayMode.Items.FindByValue("Company"));
                            //ddlPayMode.Items.Remove(ddlPayMode.Items.FindByValue("Cheque"));
                        }

                        if (BanquetReservationBill[0].CompanyId > 0)
                        {
                            GuestCompanyBO companyBO = new GuestCompanyBO();
                            GuestCompanyDA companyDA = new GuestCompanyDA();
                            companyBO = companyDA.GetGuestCompanyInfoById(BanquetReservationBill[0].CompanyId);
                            if (companyBO != null)
                            {
                                if (companyBO.CompanyId > 0)
                                {
                                    if (companyBO.ActiveStat)
                                    {
                                        GuestCompanyDA companyDa = new GuestCompanyDA();
                                        List<GuestCompanyBO> companyListBO = new List<GuestCompanyBO>();
                                        companyListBO = companyDa.GetGuestCompanyInfo().Where(x => x.CompanyId == BanquetReservationBill[0].CompanyId).ToList();
                                        ddlCompanyName.DataSource = companyListBO;
                                        ddlCompanyName.DataTextField = "CompanyName";
                                        ddlCompanyName.DataValueField = "NodeId";
                                        ddlCompanyName.DataBind();

                                        ListItem itemRoom = new ListItem
                                        {
                                            Value = "Company",
                                            Text = "Company"
                                        };
                                        ddlPayMode.Items.Insert(4, itemRoom);
                                    }
                                }
                            }
                        }

                        pnlBillPrintPreview.Visible = true;
                    }
                }
                else
                {
                    gvItemDetail.DataSource = null;
                    gvItemDetail.DataBind();

                    gvRequisitesDetail.DataSource = null;
                    gvRequisitesDetail.DataBind();

                    txtIndividualRoomVatAmount.Text = "0";
                    txtIndividualRoomServiceCharge.Text = "0";
                    txtIndividualRoomDiscountAmount.Text = "0";
                    txtIndividualRoomGrandTotal.Text = "0";
                }
            }
            else
            {
                gvItemDetail.DataSource = null;
                gvItemDetail.DataBind();

                gvRequisitesDetail.DataSource = null;
                gvRequisitesDetail.DataBind();

                txtIndividualRoomVatAmount.Text = "0";
                txtIndividualRoomServiceCharge.Text = "0";
                txtIndividualRoomDiscountAmount.Text = "0";
                txtIndividualRoomGrandTotal.Text = "0";
            }


        }
        public void CalculateSalesTotal()
        {
            /*
            // Vat Total Calculation-----------
            decimal calculatedGuestRoomVatTotal = !string.IsNullOrWhiteSpace(txtIndividualRoomVatAmount.Text) ? Convert.ToDecimal(txtIndividualRoomVatAmount.Text) : 0;
            decimal calculatedGuestServiceVatTotal = !string.IsNullOrWhiteSpace(txtIndividualServiceVatAmount.Text) ? Convert.ToDecimal(txtIndividualServiceVatAmount.Text) : 0;
            decimal calculatedRestaurantVatTotal = !string.IsNullOrWhiteSpace(txtIndividualRestaurantVatAmount.Text) ? Convert.ToDecimal(txtIndividualRestaurantVatAmount.Text) : 0;
            decimal calculatedExtraRoomVatTotal = !string.IsNullOrWhiteSpace(txtIndividualExtraRoomVatAmount.Text) ? Convert.ToDecimal(txtIndividualExtraRoomVatAmount.Text) : 0;

            decimal calculatedVatTotal = calculatedGuestRoomVatTotal + calculatedGuestServiceVatTotal + calculatedRestaurantVatTotal + calculatedExtraRoomVatTotal;
            txtVatTotal.Text = calculatedVatTotal.ToString("#0.00");

            // Service Charge Total Calculation-----------
            decimal calculatedGuestRoomServiceChargeTotal = !string.IsNullOrWhiteSpace(txtIndividualRoomServiceCharge.Text) ? Convert.ToDecimal(txtIndividualRoomServiceCharge.Text) : 0;
            decimal calculatedGuestServiceServiceChargeTotal = !string.IsNullOrWhiteSpace(txtIndividualServiceServiceCharge.Text) ? Convert.ToDecimal(txtIndividualServiceServiceCharge.Text) : 0;
            decimal calculatedRestaurantServiceChargeTotal = !string.IsNullOrWhiteSpace(txtIndividualRestaurantServiceCharge.Text) ? Convert.ToDecimal(txtIndividualRestaurantServiceCharge.Text) : 0;
            decimal calculatedExtraRoomServiceChargeTotal = !string.IsNullOrWhiteSpace(txtIndividualExtraRoomServiceCharge.Text) ? Convert.ToDecimal(txtIndividualExtraRoomServiceCharge.Text) : 0;

            decimal calculatedServiceChargeTotal = calculatedGuestRoomServiceChargeTotal + calculatedGuestServiceServiceChargeTotal + calculatedRestaurantServiceChargeTotal + calculatedExtraRoomServiceChargeTotal;
            txtServiceChargeTotal.Text = calculatedServiceChargeTotal.ToString("#0.00");

            // Discount Total Calculation-----------
            decimal calculatedGuestRoomDiscountTotal = !string.IsNullOrWhiteSpace(txtIndividualRoomDiscountAmount.Text) ? Convert.ToDecimal(txtIndividualRoomDiscountAmount.Text) : 0;
            decimal calculatedGuestServiceDiscountTotal = !string.IsNullOrWhiteSpace(txtIndividualServiceDiscountAmount.Text) ? Convert.ToDecimal(txtIndividualServiceDiscountAmount.Text) : 0;
            decimal calculatedRestaurantDiscountTotal = !string.IsNullOrWhiteSpace(txtIndividualRestaurantDiscountAmount.Text) ? Convert.ToDecimal(txtIndividualRestaurantDiscountAmount.Text) : 0;
            decimal calculatedExtraRoomDiscountTotal = !string.IsNullOrWhiteSpace(txtIndividualExtraRoomDiscountAmount.Text) ? Convert.ToDecimal(txtIndividualExtraRoomDiscountAmount.Text) : 0;

            decimal calculatedDiscountTotal = calculatedGuestRoomDiscountTotal + calculatedGuestServiceDiscountTotal + calculatedRestaurantDiscountTotal + calculatedExtraRoomDiscountTotal;
            txtDiscountAmountTotal.Text = calculatedDiscountTotal.ToString("#0.00");

            // Sales Total Calculation-----------
            decimal calculatedGuestRoomTotal = !string.IsNullOrWhiteSpace(txtIndividualRoomGrandTotal.Text) ? Convert.ToDecimal(txtIndividualRoomGrandTotal.Text) : 0;
            decimal calculatedGuestServiceTotal = !string.IsNullOrWhiteSpace(txtIndividualServiceGrandTotal.Text) ? Convert.ToDecimal(txtIndividualServiceGrandTotal.Text) : 0;
            decimal calculatedRestaurantTotal = !string.IsNullOrWhiteSpace(txtIndividualRestaurantGrandTotal.Text) ? Convert.ToDecimal(txtIndividualRestaurantGrandTotal.Text) : 0;
            //decimal calculatedExtraRoomTotal = !string.IsNullOrWhiteSpace(txtIndividualExtraRoomGrandTotal.Text) ? Convert.ToDecimal(txtIndividualExtraRoomGrandTotal.Text) : 0;
            decimal calculatedExtraRoomTotal = 0;
            decimal calculatedAdvancePaymentAmountTotal = !string.IsNullOrWhiteSpace(txtAdvancePaymentAmount.Text) ? Convert.ToDecimal(txtAdvancePaymentAmount.Text) : 0;

            //decimal calculatedSalesTotal = (calculatedGuestRoomTotal + calculatedGuestServiceTotal + calculatedRestaurantTotal + calculatedExtraRoomTotal) - calculatedAdvancePaymentAmountTotal - calculatedDiscountTotal;
            decimal calculatedSalesTotal = (calculatedGuestRoomTotal + calculatedGuestServiceTotal + calculatedRestaurantTotal + calculatedExtraRoomTotal) - calculatedAdvancePaymentAmountTotal;
            txtSalesTotal.Text = Math.Round(calculatedSalesTotal).ToString("#0.00");
            HiddenFieldSalesTotal.Value = Math.Round(calculatedSalesTotal).ToString("#0.00");

            txtDiscountAmount.Text = "0.00";
            txtGrandTotal.Text = Math.Round(calculatedSalesTotal).ToString("#0.00");
            HiddenFieldGrandTotal.Value = Math.Round(calculatedSalesTotal).ToString("#0.00");


            //decimal conversionRate = !string.IsNullOrWhiteSpace(txtConversionRate.Text) ? Convert.ToDecimal(txtConversionRate.Text) : 1;
            if (!string.IsNullOrWhiteSpace(ddlReservationId.SelectedValue))
            {
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                RoomRegistrationBO registrationBO = new RoomRegistrationBO();
                registrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(Convert.ToInt32(ddlReservationId.SelectedValue));
                txtConversionRate.Text = registrationBO.ConversionRate.ToString();
                txtConversionRateHiddenField.Value = registrationBO.ConversionRate.ToString();
                btnLocalBillPreview.Text = "Bill Preview" + " (" + registrationBO.LocalCurrencyHead + ")";
                btnUSDBillPreview.Text = "Bill Preview (USD)";
            }
            decimal conversionRate = !string.IsNullOrWhiteSpace(txtConversionRateHiddenField.Value) ? Convert.ToDecimal(txtConversionRateHiddenField.Value) : 1;
            if (conversionRate > 0)
            {
                lblSalesTotalUsd.Visible = true;
                txtSalesTotalUsd.Visible = true;
                lblGrandTotalUsd.Visible = true;
                txtGrandTotalUsd.Visible = true;
                txtSalesTotalUsd.Text = (calculatedSalesTotal / conversionRate).ToString("#0.00"); //Math.Round(calculatedSalesTotal / conversionRate).ToString("#0.00");
                hfTxtSalesTotalUsd.Value = txtSalesTotalUsd.Text;
                txtGrandTotalUsd.Text = txtSalesTotalUsd.Text;
                hfGrandTotalUsd.Value = txtSalesTotalUsd.Text;
            }
            else
            {
                txtSalesTotalUsd.Text = "0.00";
                lblSalesTotalUsd.Visible = false;
                txtSalesTotalUsd.Visible = false;
                lblGrandTotalUsd.Visible = false;
                txtGrandTotalUsd.Visible = false;
            }
             */
        }
        private void Cancel()
        {
            txtSrcRegistrationIdList.Value = string.Empty;
            hfGuestPaymentDetailsInformationDiv.Value = "1";
            ddlPayMode.SelectedIndex = 0;
            ddlChequeReceiveAccountsInfo.SelectedIndex = 0;
            txtChecqueNumber.Text = string.Empty;
            ddlCardPaymentAccountHeadId.SelectedIndex = 0;
            ddlCardType.SelectedIndex = 0;
            txtCardNumber.Text = string.Empty;
            txtCardHolderName.Text = string.Empty;
            txtExpireDate.Text = string.Empty;
            ddlBankId.SelectedValue = "0";
            btnSave.Text = "Save";
            ddlReservationId.Focus();
            GuestPaymentDetailGrid.InnerHtml = "";
            Session["GuestPaymentDetailListForGrid"] = null;
        }
        private bool IsFrmValid()
        {
            bool flag = true;

            decimal grandTotal = !string.IsNullOrWhiteSpace(HiddenFieldGrandTotal.Value) ? Convert.ToDecimal(HiddenFieldGrandTotal.Value) : 0;
            decimal totalPaid = !string.IsNullOrWhiteSpace(HiddenFieldTotalPaid.Value) ? Convert.ToDecimal(HiddenFieldTotalPaid.Value) : 0;
            if (grandTotal != totalPaid)
            {
                //isMessageBoxEnable = 1;
                //lblMessage.Text = "Grand Total and Guest Payment Amount is not Equal..";
                CommonHelper.AlertInfo(innboardMessage, "Grand Total and Guest Payment Amount is not Equal..", AlertType.Warning);
                ddlPayMode.Focus();
                flag = false;
                return flag;
            }


            if (string.IsNullOrWhiteSpace(txtSrcBillNumber.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a valid Bill Number.", AlertType.Warning);
                txtSrcBillNumber.Focus();
                flag = false;
            }
            else if (ddlRoomId.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "a valid Room Number.", AlertType.Warning);
                txtSrcBillNumber.Focus();
                flag = false;
            }
            else if (ddlPayMode.SelectedValue == "0")
            {
                if (grandTotal > 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Payment Mode.", AlertType.Warning);
                    ddlPayMode.Focus();
                    flag = false;
                }
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "", AlertType.Warning);
            }
            return flag;
        }
        private void LoadRelatedInformation()
        {
            if (ddlReservationId.SelectedIndex != -1)
            {
                LoadItemAndRequisitesGridView();
                LoadPaymentInformation();
            }
        }
        //private void LoadPaymentInformation()
        //{
        //    if (ddlReservationId.SelectedIndex != -1)
        //    {
        //        //HMCommonDA hmCommonDA = new HMCommonDA();
        //        string reservationId = ddlReservationId.SelectedValue.ToString();

        //        //PaymentSummaryBO paymentSummaryBO = new PaymentSummaryBO();
        //        //BanquetBillPaymentDA paymentSummaryDA = new BanquetBillPaymentDA();
        //        //paymentSummaryBO = paymentSummaryDA.GetGuestBillPaymentSummaryInfoByBanquetReservationId(reservationId, 0);
        //        //txtAdvancePaymentAmount.Text = Math.Round(paymentSummaryBO.TotalPayment).ToString();
        //        //hfAdvancePaymentAmount.Value = Math.Round(paymentSummaryBO.TotalPayment).ToString();

        //        LoadPaymentDetailsInformation(reservationId);

        //    }
        //    else
        //    {
        //        txtAdvancePaymentAmount.Text = "0";
        //        hfAdvancePaymentAmount.Value = "0";
        //    }
        //}
        private void LoadPaymentInformation()
        {
            decimal calculatePaidAmount = 0, dueAmountTotal = 0;
            string reservationId = ddlReservationId.SelectedValue.ToString();
            if (!string.IsNullOrEmpty(reservationId))
            {
                List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;

                GuestBillPaymentDA da = new GuestBillPaymentDA();
                guestPaymentDetailListForGrid = da.GetGuestBillPaymentInformationByServiceBillId("Banquet", Convert.ToInt32(reservationId)).Where(x => x.PaymentType != "Discount").ToList();
                HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = guestPaymentDetailListForGrid;

                string strTable = "";
                if (guestPaymentDetailListForGrid != null)
                {
                    strTable += "<table id='ReservationDetailGrid' class='table table-bordered table-condensed table-responsive' style='width:100%'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                    strTable += "<th align='left' scope='col'>Payment Mode</th><th align='left' scope='col'>Description</th><th align='left' scope='col'>Amount</th><th align='center' scope='col'>Action</th></tr>";
                    int counter = 0;
                    foreach (GuestBillPaymentBO dr in guestPaymentDetailListForGrid)
                    {
                        counter++;
                        if (counter % 2 == 0)
                        {
                            // It's even
                            strTable += "<tr style='background-color:#E3EAEB;'><td align='left' style='width: 40%;'>" + dr.PaymentMode + "</td>";
                        }
                        else
                        {
                            // It's odd
                            strTable += "<tr style='background-color:White;'><td align='left' style='width: 40%;'>" + dr.PaymentMode + "</td>";
                        }
                        strTable += "<td align='left' style='width: 40%;'>" + dr.PaymentDescription + "</td>";
                        strTable += "<td align='left' style='width: 20%;'>" + dr.PaymentAmount + "</td>";
                        strTable += "<td align='center' style='width: 15%;'>";
                        if (dr.BillNumber == null || dr.PaymentType != "Advance")
                            strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformGuestPaymentDetailDelete(" + dr.PaymentId + ")' alt='Delete Information' border='0' />";
                        strTable += "</td></tr>";

                        calculatePaidAmount = calculatePaidAmount + dr.PaymentAmount;
                    }
                    strTable += "</table>";
                    if (strTable == "")
                    {
                        strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
                    }
                }

                //txtAdvancePaymentAmount.Text = ((!string.IsNullOrWhiteSpace(txtAdvancePaymentAmount.Text) ? Convert.ToDecimal(txtAdvancePaymentAmount.Text) : 0) - alreadyRoomPaidAmount).ToString();

                BanquetReservationDA banquetReservationDA = new BanquetReservationDA();
                BanquetReservationBO banquetReservationBO = new BanquetReservationBO();

                banquetReservationBO = banquetReservationDA.GetBanquetReservationInfoById(Convert.ToInt32(reservationId));
                if (banquetReservationBO != null)
                {
                    if (banquetReservationBO.Id > 0)
                    {
                        if (banquetReservationBO.EventType == "Internal")
                        {
                            txtSalesTotal.Text = (Math.Round(banquetReservationBO.GrandTotal)).ToString();
                        }
                        txtGrandTotal.Text = (Math.Round(banquetReservationBO.GrandTotal)).ToString();

                        dueAmountTotal = Math.Round(banquetReservationBO.GrandTotal - calculatePaidAmount);

                        ddlDiscountType.SelectedValue = "Fixed";
                        txtDiscountAmount.Text = "0.00";
                    }
                }

                GuestPaymentDetailGrid.InnerHtml = strTable;
            }
            hfDueAmount.Value = dueAmountTotal.ToString();
            TotalPaid.InnerHtml = "Total Amount: " + calculatePaidAmount.ToString("0.00");
            dueTotal.InnerHtml = "Due Amount   :  " + dueAmountTotal.ToString("0.00");
        }
        private void LoadAccountHeadInfo()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            lblPaymentAccountHead.Text = "Payment Receive In";

            List<CommonPaymentModeBO> commonPaymentModeBOList = new List<CommonPaymentModeBO>();
            commonPaymentModeBOList = hmCommonDA.GetCommonPaymentModeInfo("All");

            CommonPaymentModeBO cashPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Cash").FirstOrDefault();
            CommonPaymentModeBO cardPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Card").FirstOrDefault();
            CommonPaymentModeBO chequePaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Cheque").FirstOrDefault();
            CommonPaymentModeBO companyPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Company").FirstOrDefault();
            CommonPaymentModeBO mBankingPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "M-Banking").FirstOrDefault();
            CommonPaymentModeBO refundPaymentModeInfo = commonPaymentModeBOList.Where(x => x.PaymentMode == "Refund").FirstOrDefault();

            ddlCashReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cashPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            ddlCashReceiveAccountsInfo.DataTextField = "NodeHead";
            ddlCashReceiveAccountsInfo.DataValueField = "NodeId";
            ddlCashReceiveAccountsInfo.DataBind();

            ddlCardReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cardPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            ddlCardReceiveAccountsInfo.DataTextField = "NodeHead";
            ddlCardReceiveAccountsInfo.DataValueField = "NodeId";
            ddlCardReceiveAccountsInfo.DataBind();

            ddlCardPaymentAccountHeadId.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + cardPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            ddlCardPaymentAccountHeadId.DataTextField = "NodeHead";
            ddlCardPaymentAccountHeadId.DataValueField = "NodeId";
            ddlCardPaymentAccountHeadId.DataBind();

            ddlChequeReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + chequePaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            ddlChequeReceiveAccountsInfo.DataTextField = "NodeHead";
            ddlChequeReceiveAccountsInfo.DataValueField = "NodeId";
            ddlChequeReceiveAccountsInfo.DataBind();

            ddlMBankingReceiveAccountsInfo.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + mBankingPaymentModeInfo.ReceiveAccountsPostingId.ToString() + ")");
            ddlMBankingReceiveAccountsInfo.DataTextField = "NodeHead";
            ddlMBankingReceiveAccountsInfo.DataValueField = "NodeId";
            ddlMBankingReceiveAccountsInfo.DataBind();

            ddlRefundAccountHead.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + refundPaymentModeInfo.PaymentAccountsPostingId.ToString() + ")");
            ddlRefundAccountHead.DataTextField = "NodeHead";
            ddlRefundAccountHead.DataValueField = "NodeId";
            ddlRefundAccountHead.DataBind();
        }
        public static string LoadGuestPaymentDetailGridViewByWM(string paymentDescription)
        {
            string strTable = "";
            List<GuestBillPaymentBO> detailList = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;
            if (detailList != null)
            {
                strTable += "<table id='ReservationDetailGrid' class='table table-bordered table-condensed table-responsive' style='width:100%'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                strTable += "<th align='left' scope='col'>Payment Mode</th><th align='left' scope='col'>Description</th><th align='left' scope='col'>Amount</th><th align='center' scope='col'>Action</th></tr>";
                int counter = 0;
                foreach (GuestBillPaymentBO dr in detailList)
                {
                    counter++;
                    if (counter % 2 == 0)
                    {
                        // It's even
                        strTable += "<tr style='background-color:#E3EAEB;'><td align='left' style='width: 40%;'>" + dr.PaymentMode + "</td>";
                    }
                    else
                    {
                        // It's odd
                        strTable += "<tr style='background-color:White;'><td align='left' style='width: 40%;'>" + dr.PaymentMode + "</td>";
                    }
                    strTable += "<td align='left' style='width: 40%;'>" + dr.PaymentDescription + "</td>";
                    strTable += "<td align='left' style='width: 20%;'>" + dr.PaymentAmount + "</td>";
                    strTable += "<td align='center' style='width: 15%;'>";
                    if (dr.BillNumber == null || dr.PaymentType != "Advance")
                        strTable += "&nbsp;<img src='../Images/delete.png' onClick='javascript:return PerformGuestPaymentDetailDelete(" + dr.PaymentId + ")' alt='Delete Information' border='0' />";
                    strTable += "</td></tr>";
                }
                strTable += "</table>";
                if (strTable == "")
                {
                    strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
                }
            }
            return strTable;
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        private void ClearCommonSessionInformation()
        {
            Session["TransactionDetailList"] = null;
            Session["GuestPaymentDetailListForGrid"] = null;
            Session["CheckOutPayMode"] = null;
            Session["CurrentRegistrationId"] = null;
            Session["IsCheckOutBillPreview"] = null;
            Session["GuestBillRoomIdParameterValue"] = null;
            Session["GuestBillServiceIdParameterValue"] = null;
            Session["GuestPaymentDetailListForGrid"] = null;
            Session["CompanyPaymentRoomIdList"] = null;
            Session["CompanyPaymentServiceIdList"] = null;
        }
        private void ClearUnCommonSessionInformation()
        {
            Session["txtStartDate"] = null;
            Session["IsBillSplited"] = null;
            Session["GuestBillFromDate"] = null;
            Session["GuestBillToDate"] = null;
            Session["AddedExtraRoomInformation"] = null;
            Session["CheckOutRegistrationIdList"] = null;
        }
        private void GuestCheckOutDiscount(int ddlReservationId)
        {
            HMUtility hmUtility = new HMUtility();
            int dynamicDetailId = 0;
            int ddlPaidByRegistrationId = 0;

            List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;

            //GuestBillPaymentBO singleEntityBOEdit = guestPaymentDetailListForGrid.Where(x => x.PaymentMode == ddlPayMode).FirstOrDefault();

            if (guestPaymentDetailListForGrid != null)
            {
                dynamicDetailId = guestPaymentDetailListForGrid.Count + 1;
            }

            GuestBillPaymentBO guestBillPaymentBO = new GuestBillPaymentBO();
            ddlPaidByRegistrationId = Convert.ToInt32(ddlReservationId);


            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            CustomFieldBO CashReceiveAccountsInfo = new CustomFieldBO();
            CashReceiveAccountsInfo = hmCommonDA.GetCustomFieldByFieldName("CashReceiveAccountsInfo");
            ddlCashPaymentAccountHeadForDiscount.DataSource = entityDA.GetNodeMatrixInfoByCustomString("WHERE  NodeId IN(" + CashReceiveAccountsInfo.FieldValue.ToString() + ")");
            ddlCashPaymentAccountHeadForDiscount.DataTextField = "NodeHead";
            ddlCashPaymentAccountHeadForDiscount.DataValueField = "NodeId";
            ddlCashPaymentAccountHeadForDiscount.DataBind();

            guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCashPaymentAccountHeadForDiscount.SelectedValue);
            guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPaymentAccountHeadForDiscount.SelectedValue);

            guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlReservationId);
            guestBillPaymentBO.PaymentType = "Discount";
            //}

            guestBillPaymentBO.BankId = Convert.ToInt32(0);
            guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlReservationId);
            //guestBillPaymentBO.FieldId = 45; // Convert.ToInt32(ddlCurrency);
            guestBillPaymentBO.FieldId = LocalCurrencyId;


            guestBillPaymentBO.ConvertionRate = 1;


            decimal totalSalesAmount = !string.IsNullOrWhiteSpace(HiddenFieldSalesTotal.Value) ? Convert.ToDecimal(HiddenFieldSalesTotal.Value) : 0;
            decimal grandTotalAmount = !string.IsNullOrWhiteSpace(HiddenFieldGrandTotal.Value) ? Convert.ToDecimal(HiddenFieldGrandTotal.Value) : 0;

            guestBillPaymentBO.CurrencyAmount = (totalSalesAmount - grandTotalAmount);
            guestBillPaymentBO.PaymentAmount = (totalSalesAmount - grandTotalAmount);


            guestBillPaymentBO.ChecqueDate = DateTime.Now;
            guestBillPaymentBO.PaymentMode = "Cash";
            guestBillPaymentBO.PaymentId = dynamicDetailId;
            guestBillPaymentBO.CardNumber = string.Empty;
            guestBillPaymentBO.CardType = string.Empty;
            //if (string.IsNullOrEmpty(txtExpireDate))
            //{
            guestBillPaymentBO.ExpireDate = null;
            //}
            //else
            //{
            //    guestBillPaymentBO.ExpireDate = hmUtility.GetDateTimeFromString(txtExpireDate);
            //}
            guestBillPaymentBO.ChecqueNumber = string.Empty;
            guestBillPaymentBO.CardHolderName = string.Empty;

            guestBillPaymentBO.PaymentDescription = string.Empty;


            guestBillPaymentBO.PaymentId = dynamicDetailId;

            guestPaymentDetailListForGrid.Add(guestBillPaymentBO);
            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = guestPaymentDetailListForGrid;
            //return LoadGuestPaymentDetailGridViewByWM(paymentDescription);
        }
        private void Clear()
        {
            txtVatTotal.Text = string.Empty;
            txtServiceChargeTotal.Text = string.Empty;
            txtDiscountAmountTotal.Text = string.Empty;
            txtAdvancePaymentAmount.Text = string.Empty;
            txtSalesTotal.Text = string.Empty;
            txtIndividualRoomVatAmount.Text = string.Empty;
            txtIndividualRoomServiceCharge.Text = string.Empty;
            txtIndividualRoomDiscountAmount.Text = string.Empty;
            txtIndividualRoomGrandTotal.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            txtRebateRemarks.Text = string.Empty;
            ddlReservationId.SelectedIndex = -1;
            pnlBillPrintPreview.Visible = false;

            txtSDCharge.Text = string.Empty;
            txtAdditionalCharge.Text = string.Empty;
            TotalPaid.InnerText = string.Empty;
            dueTotal.InnerText = string.Empty;
            btnSave.Enabled = false;
            txtSrcBillNumber.Text = string.Empty;

            txtBanquetId.Text = string.Empty;
            txtBanquetRate.Text = string.Empty;
            txtSeatingId.Text = string.Empty;
            txtGrandTotal.Text = string.Empty;
            txtMeetingDiscussion1.Value = string.Empty;
            txtCallToAction1.Value = string.Empty;
            ddlCompany1.SelectedIndex = -1;
            gvItemDetail.DataSource = null;
            gvItemDetail.DataBind();

            gvRequisitesDetail.DataSource = null;
            gvRequisitesDetail.DataBind();

            List<BanquetReservationBO> blankBanquetReservationBOList = new List<BanquetReservationBO>();
            ddlReservationId.DataSource = blankBanquetReservationBOList;
            ddlReservationId.DataTextField = "ReservationNumber";
            ddlReservationId.DataValueField = "ReservationId";
            ddlReservationId.DataBind();

            ListItem item = new ListItem
            {
                Value = "0",
                Text = hmUtility.GetDropDownFirstValue()
            };
            ddlReservationId.Items.Insert(0, item);

            txtSrcRegistrationIdList.Value = ddlReservationId.SelectedValue;
            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = null;
            HttpContext.Current.Session["DeletedGuestPaymentDetailListForGrid"] = null;
            GuestPaymentDetailGrid.InnerHtml = "";

            hfDueAmount.Value = "";
            hfEventType.Value = "0";
        }
        //************************ User Defined WebMethod ********************//       
        [WebMethod]
        public static string GetTotalBillAmountByWebMethod(string ddlReservationId, string SelectdRoomId, string SelectdServiceId, string StartDate, string EndDate)
        {
            GuestBillSplitDA entityDA = new GuestBillSplitDA();
            GuestServiceBillApprovedBO entityBO = new GuestServiceBillApprovedBO();

            HMUtility hmUtility = new HMUtility();
            string startDate = hmUtility.GetUnivarsalDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            string endDate = hmUtility.GetUnivarsalDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            entityBO = entityDA.GetGuestServiceTotalAmountInfo(ddlReservationId, SelectdRoomId, SelectdServiceId, startDate, endDate);
            return Math.Round(entityBO.ServiceTotalAmount).ToString("#0.00"); // entityBO.ServiceTotalAmount.ToString();
        }
        [WebMethod(EnableSession = true)]
        public static string PerformSaveGuestPaymentDetailsInformationByWebMethod(bool isEdit, string paymentDescription, string ddlCurrency, string currencyType, string localCurrencyId, string conversionRate, string ddlPayMode, string ddlBankId, string txtReceiveLeadgerAmount, string ddlReservationId, string ddlCashPaymentAccountHead, string txtCardNumber, string ddlCardType, string txtExpireDate, string txtCardHolderName, string txtChecqueNumber, string ddlChecquePaymentAccountHeadId, string ddlCardPaymentAccountHeadId, string ddlCompanyPaymentAccountHead, string ddlMBankingBankId, string ddlMBankingReceiveAccountsInfo, string ddlPaidByRoomId, string RefundAccountHead)
        {
            HMUtility hmUtility = new HMUtility();
            int dynamicDetailId = 0;
            int ddlPaidByRegistrationId = 0;

            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            CustomFieldBO banquetRoomSalesAccountHeadInfoBO = new CustomFieldBO();
            banquetRoomSalesAccountHeadInfoBO = hmCommonDA.GetCustomFieldByFieldName("BanquetRoomSalesAccountHeadInfo");
            int banquetRoomSalesAccountHeadInfo = !string.IsNullOrWhiteSpace(banquetRoomSalesAccountHeadInfoBO.FieldValue) ? Convert.ToInt32(banquetRoomSalesAccountHeadInfoBO.FieldValue) : 0;

            List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;

            GuestBillPaymentBO singleEntityBOEdit = guestPaymentDetailListForGrid.Where(x => x.PaymentMode == ddlPayMode).FirstOrDefault();

            if (guestPaymentDetailListForGrid != null)
            {
                dynamicDetailId = guestPaymentDetailListForGrid.Count + 1;
            }

            GuestBillPaymentBO guestBillPaymentBO = new GuestBillPaymentBO();

            if (ddlPayMode == "Other Room")
            {
                if (!string.IsNullOrWhiteSpace(ddlPaidByRoomId))
                {
                    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                    List<RoomRegistrationBO> billPaidByInfoList = new List<RoomRegistrationBO>();

                    billPaidByInfoList = roomRegistrationDA.GetRoomRegistrationInfoByRoomId(Convert.ToInt32(ddlPaidByRoomId));
                    if (billPaidByInfoList != null)
                    {
                        foreach (RoomRegistrationBO row in billPaidByInfoList)
                        {
                            ddlPaidByRegistrationId = row.RegistrationId;
                        }
                    }
                    else
                    {
                        ddlPaidByRegistrationId = Convert.ToInt32(ddlReservationId);
                    }
                }
                else
                {
                    ddlPaidByRegistrationId = Convert.ToInt32(ddlReservationId);
                }
            }
            else
            {
                ddlPaidByRegistrationId = Convert.ToInt32(ddlReservationId);
            }

            if (ddlPayMode == "Company")
            {
                guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCompanyPaymentAccountHead);
                guestBillPaymentBO.PaymentType = ddlPayMode;
                guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCompanyPaymentAccountHead);
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlReservationId);
                guestBillPaymentBO.RegistrationId = 0;
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlReservationId);
            }
            //else if (ddlPayMode == "Employee")
            //{
            //    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlEmployeePaymentAccountHead);
            //    guestBillPaymentBO.PaymentType = ddlPayMode;
            //    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlEmployeePaymentAccountHead);
            //    guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlPaidByRegistrationId);
            //    guestBillPaymentBO.BankId = Convert.ToInt32(ddlEmpId);
            //    guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlEmpId);
            //}
            else if (ddlPayMode == "Other Room")
            {
                guestBillPaymentBO.NodeId = banquetRoomSalesAccountHeadInfo;
                guestBillPaymentBO.PaymentType = ddlPayMode;
                guestBillPaymentBO.AccountsPostingHeadId = banquetRoomSalesAccountHeadInfo;
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlPaidByRegistrationId);
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlPaidByRegistrationId);
                guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlPaidByRegistrationId);
            }
            else if (ddlPayMode == "Refund")
            {
                ddlCurrency = localCurrencyId;
                conversionRate = "1";
                guestBillPaymentBO.RefundAccountHead = Int32.Parse(RefundAccountHead);
                guestBillPaymentBO.PaymentMode = "Refund";
                guestBillPaymentBO.CurrencyAmount = guestBillPaymentBO.CurrencyAmount * 1;
                guestBillPaymentBO.PaymentAmount = guestBillPaymentBO.PaymentAmount * 1;
                guestBillPaymentBO.PaymentType = "Refund";
                guestBillPaymentBO.AccountsPostingHeadId = Int32.Parse(RefundAccountHead);
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlReservationId);
                guestBillPaymentBO.RegistrationId = 0;
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlReservationId);
            }
            else
            {
                if (ddlPayMode == "Cash")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCashPaymentAccountHead);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPaymentAccountHead);
                    guestBillPaymentBO.RegistrationId = 0;
                }
                else if (ddlPayMode == "Card")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCardPaymentAccountHeadId);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCardPaymentAccountHeadId);
                    guestBillPaymentBO.RegistrationId = 0;
                }
                else if (ddlPayMode == "Cheque")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlChecquePaymentAccountHeadId);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlChecquePaymentAccountHeadId);
                    ddlCardType = string.Empty;
                    guestBillPaymentBO.RegistrationId = 0;
                }
                if (ddlPayMode == "M-Banking")
                {
                    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlMBankingReceiveAccountsInfo);
                    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlMBankingReceiveAccountsInfo);
                }
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlBankId);
                guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlReservationId);
                if (guestBillPaymentBO.BillNumber != null)
                    guestBillPaymentBO.PaymentType = "Advance";
                else
                    guestBillPaymentBO.PaymentType = "Payment";
            }

            guestBillPaymentBO.PaymentDescription = paymentDescription;
            guestBillPaymentBO.BankId = Convert.ToInt32(ddlBankId);
            if (ddlPayMode == "M-Banking")
            {
                guestBillPaymentBO.BankId = Convert.ToInt32(ddlMBankingBankId);
            }

            if (currencyType == "Local")
            {
                guestBillPaymentBO.IsUSDTransaction = false;
                guestBillPaymentBO.FieldId = Int32.Parse(ddlCurrency);
                guestBillPaymentBO.ConvertionRate = 1;
                guestBillPaymentBO.CurrencyAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
                guestBillPaymentBO.PaymentAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
            }
            else
            {
                guestBillPaymentBO.IsUSDTransaction = true;
                guestBillPaymentBO.FieldId = Int32.Parse(ddlCurrency);
                guestBillPaymentBO.ConvertionRate = !string.IsNullOrWhiteSpace(conversionRate) ? Convert.ToDecimal(conversionRate) : 1;
                guestBillPaymentBO.CurrencyAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
                guestBillPaymentBO.PaymentAmount = guestBillPaymentBO.CurrencyAmount * guestBillPaymentBO.ConvertionRate;
            }

            guestBillPaymentBO.ChecqueDate = DateTime.Now;
            guestBillPaymentBO.PaymentMode = ddlPayMode;
            guestBillPaymentBO.PaymentId = dynamicDetailId;
            guestBillPaymentBO.CardNumber = txtCardNumber;
            guestBillPaymentBO.CardType = ddlCardType;
            if (string.IsNullOrEmpty(txtExpireDate))
            {
                guestBillPaymentBO.ExpireDate = null;
            }
            else
            {
                guestBillPaymentBO.ExpireDate = hmUtility.GetDateTimeFromString(txtExpireDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            guestBillPaymentBO.ChecqueNumber = txtChecqueNumber;
            guestBillPaymentBO.CardHolderName = txtCardHolderName;

            guestBillPaymentBO.PaymentDescription = paymentDescription;

            guestBillPaymentBO.PaymentId = dynamicDetailId;

            guestPaymentDetailListForGrid.Add(guestBillPaymentBO);
            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = guestPaymentDetailListForGrid;
            return LoadGuestPaymentDetailGridViewByWM(paymentDescription);
        }
        [WebMethod(EnableSession = true)]
        public static string PerformDeleteGuestPaymentByWebMethod(int paymentId)
        {
            List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;
            GuestBillPaymentBO singleEntityBOEdit = guestPaymentDetailListForGrid.Where(x => x.PaymentId == paymentId).FirstOrDefault();
            if (guestPaymentDetailListForGrid.Contains(singleEntityBOEdit))
            {
                guestPaymentDetailListForGrid.Remove(singleEntityBOEdit);
                if (singleEntityBOEdit.BillNumber != null)
                {
                    List<GuestBillPaymentBO> deletedPaymentDetailListForGrid = HttpContext.Current.Session["DeletedGuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["DeletedGuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;
                    deletedPaymentDetailListForGrid.Add(singleEntityBOEdit);
                    HttpContext.Current.Session["DeletedGuestPaymentDetailListForGrid"] = deletedPaymentDetailListForGrid;
                }

            }
            HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = guestPaymentDetailListForGrid;
            HttpContext.Current.Session["CompanyPaymentRoomIdList"] = null;
            HttpContext.Current.Session["CompanyPaymentServiceIdList"] = null;

            return LoadGuestPaymentDetailGridViewByWM("");
        }
        [WebMethod(EnableSession = true)]
        public static string PerformGetTotalPaidAmountByWebMethod()
        {

            var List = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;
            decimal sum = 0;
            for (int i = 0; i < List.Count; i++)
            {
                if (List[i].PaymentMode == "Refund")
                {
                    sum = sum - Convert.ToDecimal(List[i].PaymentAmount);
                }
                else
                {
                    sum = sum + Convert.ToDecimal(List[i].PaymentAmount);
                }
            }
            return Math.Round(sum).ToString();
        }
        [WebMethod(EnableSession = true)]
        public static string PerformCompanyPayBill(string serviceType, string SelectdServiceApprovedId, string SelectdRoomApprovedId, string SelectdServiceId, string SelectdRoomId, string SelectdPaymentId, string StartDate, string EndDate, string ddlReservationId, string txtSrcRegistrationIdList)
        {
            HttpContext.Current.Session["CompanyPaymentRoomIdList"] = null;
            HttpContext.Current.Session["CompanyPaymentServiceIdList"] = null;
            HMUtility hmUtility = new HMUtility();
            //Room Information----------------
            List<GuestServiceBillApprovedBO> entityRoomBOList = new List<GuestServiceBillApprovedBO>();

            int totalRoomIdOut = SelectdRoomId.Split(',').Length - 1;
            for (int i = 0; i < totalRoomIdOut; i++)
            {
                GuestServiceBillApprovedBO entityRoomBO = new GuestServiceBillApprovedBO
                {
                    RegistrationId = Convert.ToInt32(ddlReservationId),
                    ServiceId = Convert.ToInt32(SelectdRoomId.Split(',')[i]),
                    ArriveDate = hmUtility.GetDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat),
                    CheckOutDate = hmUtility.GetDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat)
                };
                entityRoomBOList.Add(entityRoomBO);
            }
            HttpContext.Current.Session["CompanyPaymentRoomIdList"] = entityRoomBOList;

            //Service Information----------------
            List<GuestServiceBillApprovedBO> entityServiceBOList = new List<GuestServiceBillApprovedBO>();

            int totalServiceIdOut = SelectdServiceId.Split(',').Length - 1;
            for (int i = 0; i < totalServiceIdOut; i++)
            {
                GuestServiceBillApprovedBO entityServiceBO = new GuestServiceBillApprovedBO
                {
                    RegistrationId = Convert.ToInt32(ddlReservationId),
                    ServiceId = Convert.ToInt32(SelectdServiceId.Split(',')[i]),
                    ArriveDate = hmUtility.GetDateTimeFromString(StartDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat),
                    CheckOutDate = hmUtility.GetDateTimeFromString(EndDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat)
                };
                entityServiceBOList.Add(entityServiceBO);
            }

            HttpContext.Current.Session["CompanyPaymentServiceIdList"] = entityServiceBOList;

            decimal calculatedAmount = 0;
            if (!string.IsNullOrWhiteSpace(SelectdPaymentId))
            {
                //Payment Information----------------
                //List<GuestServiceBillApprovedBO> entityPaymentBOList = new List<GuestServiceBillApprovedBO>();
                List<GuestBillPaymentBO> guestPaymentBOList = new List<GuestBillPaymentBO>();
                GuestBillPaymentDA guestBillPaymentDA = new GuestBillPaymentDA();

                int paymentId = SelectdPaymentId.Split(',').Length;
                for (int i = 0; i < paymentId; i++)
                {
                    calculatedAmount = calculatedAmount + guestBillPaymentDA.GetGuestBillPaymentSummaryInfoByPaymentType(txtSrcRegistrationIdList, Convert.ToInt32(SelectdPaymentId.Split(',')[i]));
                }
            }

            //HttpContext.Current.Session["CompanyPaymentServiceIdList"] = entityServiceBOList;
            return calculatedAmount.ToString();
        }
        [WebMethod]
        public static CommonCurrencyBO LoadCurrencyType(int currecyType)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyBO commonCurrencyBO = new CommonCurrencyBO();
            commonCurrencyBO = commonCurrencyDA.GetCommonCurrencyInfoById(currecyType);
            return commonCurrencyBO;
        }
        [WebMethod]
        public static CommonCurrencyConversionBO LoadCurrencyConversionRate(int currecyType)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyConversionBO conversionBO = new CommonCurrencyConversionBO();
            conversionBO = commonCurrencyDA.GetCurrencyConversionRate(currecyType);
            return conversionBO;
        }
        [WebMethod]
        public static List<InvCategoryBO> LoadCategory(int costCenterId)
        {
            List<InvCategoryBO> categoryList = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            categoryList = da.GetCostCenterWiseInvItemCatagoryInfo("Product", costCenterId);

            List<InvCategoryBO> requisitesList = new List<InvCategoryBO>();
            InvCategoryBO requisitesBO = new InvCategoryBO
            {
                CategoryId = 100000,
                Name = "Requisites",
                MatrixInfo = "Requisites"
            };
            requisitesList.Add(requisitesBO);

            List<InvCategoryBO> List = new List<InvCategoryBO>();
            List.AddRange(requisitesList);
            List.AddRange(categoryList);

            return List;
        }
        [WebMethod]
        public static List<InvItemBO> GetInvItemByCategoryNCostCenter(int costCenterId, int CategoryId)
        {
            InvItemDA productDA = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();
            productList = productDA.GetDynamicallyItemInformationByCategoryId(costCenterId, CategoryId);

            return productList;
        }
        [WebMethod]
        public static InvItemViewBO GetProductDataByCriteria(int categoryId, int costCenterId, string ddlItemId)
        {
            InvItemViewBO viewBO = new InvItemViewBO();
            InvItemDA itemDA = new InvItemDA();
            var obj = itemDA.GetInvItemPriceForBanquet(categoryId, costCenterId, Int32.Parse(ddlItemId));
            viewBO.UnitPriceLocal = obj.UnitPrice;
            viewBO.ItemId = obj.ItemId;

            return viewBO;
        }
        [WebMethod(EnableSession = true)]
        public static string PerformGuestPaymentDetailsInformationForReSettlementByWebMethod(bool isEdit, string paymentDescription, string ddlCurrency, string currencyType, string localCurrencyId, string conversionRate, string ddlPayMode, string ddlBankId, string txtReceiveLeadgerAmount, string ddlReservationId, string ddlCashPaymentAccountHead, string txtCardNumber, string ddlCardType, string txtExpireDate, string txtCardHolderName, string txtChecqueNumber, string ddlChecquePaymentAccountHeadId, string ddlCardPaymentAccountHeadId, string ddlCompanyPaymentAccountHead, string ddlMBankingBankId, string ddlMBankingReceiveAccountsInfo, string ddlPaidByRoomId, string RefundAccountHead)
        {
            //HMUtility hmUtility = new HMUtility();
            //int dynamicDetailId = 0;
            //int ddlPaidByRegistrationId = 0;

            //HMCommonDA hmCommonDA = new HMCommonDA();
            //NodeMatrixDA entityDA = new NodeMatrixDA();
            //CustomFieldBO banquetRoomSalesAccountHeadInfoBO = new CustomFieldBO();
            //banquetRoomSalesAccountHeadInfoBO = hmCommonDA.GetCustomFieldByFieldName("BanquetRoomSalesAccountHeadInfo");
            //int banquetRoomSalesAccountHeadInfo = !string.IsNullOrWhiteSpace(banquetRoomSalesAccountHeadInfoBO.FieldValue) ? Convert.ToInt32(banquetRoomSalesAccountHeadInfoBO.FieldValue) : 0;

            //List<GuestBillPaymentBO> guestPaymentDetailListForGrid = HttpContext.Current.Session["GuestPaymentDetailListForGrid"] == null ? new List<GuestBillPaymentBO>() : HttpContext.Current.Session["GuestPaymentDetailListForGrid"] as List<GuestBillPaymentBO>;

            //GuestBillPaymentBO singleEntityBOEdit = guestPaymentDetailListForGrid.Where(x => x.PaymentMode == ddlPayMode).FirstOrDefault();

            //if (guestPaymentDetailListForGrid != null)
            //{
            //    dynamicDetailId = guestPaymentDetailListForGrid.Count + 1;
            //}

            //GuestBillPaymentBO guestBillPaymentBO = new GuestBillPaymentBO();

            //if (ddlPayMode == "Other Room")
            //{
            //    if (!string.IsNullOrWhiteSpace(ddlPaidByRoomId))
            //    {
            //        RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            //        List<RoomRegistrationBO> billPaidByInfoList = new List<RoomRegistrationBO>();

            //        billPaidByInfoList = roomRegistrationDA.GetRoomRegistrationInfoByRoomId(Convert.ToInt32(ddlPaidByRoomId));
            //        if (billPaidByInfoList != null)
            //        {
            //            foreach (RoomRegistrationBO row in billPaidByInfoList)
            //            {
            //                ddlPaidByRegistrationId = row.RegistrationId;
            //            }
            //        }
            //        else
            //        {
            //            ddlPaidByRegistrationId = Convert.ToInt32(ddlReservationId);
            //        }
            //    }
            //    else
            //    {
            //        ddlPaidByRegistrationId = Convert.ToInt32(ddlReservationId);
            //    }
            //}
            //else
            //{
            //    ddlPaidByRegistrationId = Convert.ToInt32(ddlReservationId);
            //}

            //if (ddlPayMode == "Company")
            //{
            //    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCompanyPaymentAccountHead);
            //    guestBillPaymentBO.PaymentType = ddlPayMode;
            //    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCompanyPaymentAccountHead);
            //    guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlReservationId);
            //    guestBillPaymentBO.RegistrationId = 0;
            //    guestBillPaymentBO.BankId = Convert.ToInt32(ddlReservationId);
            //}
            ////else if (ddlPayMode == "Employee")
            ////{
            ////    guestBillPaymentBO.NodeId = Convert.ToInt32(ddlEmployeePaymentAccountHead);
            ////    guestBillPaymentBO.PaymentType = ddlPayMode;
            ////    guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlEmployeePaymentAccountHead);
            ////    guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlPaidByRegistrationId);
            ////    guestBillPaymentBO.BankId = Convert.ToInt32(ddlEmpId);
            ////    guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlEmpId);
            ////}
            //else if (ddlPayMode == "Other Room")
            //{
            //    guestBillPaymentBO.NodeId = banquetRoomSalesAccountHeadInfo;
            //    guestBillPaymentBO.PaymentType = ddlPayMode;
            //    guestBillPaymentBO.AccountsPostingHeadId = banquetRoomSalesAccountHeadInfo;
            //    guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlPaidByRegistrationId);
            //    guestBillPaymentBO.BankId = Convert.ToInt32(ddlPaidByRegistrationId);
            //    guestBillPaymentBO.RegistrationId = Convert.ToInt32(ddlPaidByRegistrationId);
            //}
            //else if (ddlPayMode == "Refund")
            //{
            //    ddlCurrency = localCurrencyId;
            //    conversionRate = "1";
            //    guestBillPaymentBO.RefundAccountHead = Int32.Parse(RefundAccountHead);
            //    guestBillPaymentBO.PaymentMode = "Refund";
            //    guestBillPaymentBO.CurrencyAmount = guestBillPaymentBO.CurrencyAmount * 1;
            //    guestBillPaymentBO.PaymentAmount = guestBillPaymentBO.PaymentAmount * 1;
            //    guestBillPaymentBO.PaymentType = "Refund";
            //    guestBillPaymentBO.AccountsPostingHeadId = Int32.Parse(RefundAccountHead);
            //    guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlReservationId);
            //    guestBillPaymentBO.RegistrationId = 0;
            //    guestBillPaymentBO.BankId = Convert.ToInt32(ddlReservationId);
            //}
            //else
            //{
            //    if (ddlPayMode == "Cash")
            //    {
            //        guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCashPaymentAccountHead);
            //        guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCashPaymentAccountHead);
            //        guestBillPaymentBO.RegistrationId = 0;
            //    }
            //    else if (ddlPayMode == "Card")
            //    {
            //        guestBillPaymentBO.NodeId = Convert.ToInt32(ddlCardPaymentAccountHeadId);
            //        guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlCardPaymentAccountHeadId);
            //        guestBillPaymentBO.RegistrationId = 0;
            //    }
            //    else if (ddlPayMode == "Cheque")
            //    {
            //        guestBillPaymentBO.NodeId = Convert.ToInt32(ddlChecquePaymentAccountHeadId);
            //        guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlChecquePaymentAccountHeadId);
            //        ddlCardType = string.Empty;
            //        guestBillPaymentBO.RegistrationId = 0;
            //    }
            //    if (ddlPayMode == "M-Banking")
            //    {
            //        guestBillPaymentBO.NodeId = Convert.ToInt32(ddlMBankingReceiveAccountsInfo);
            //        guestBillPaymentBO.AccountsPostingHeadId = Convert.ToInt32(ddlMBankingReceiveAccountsInfo);
            //    }
            //    guestBillPaymentBO.BankId = Convert.ToInt32(ddlBankId);
            //    guestBillPaymentBO.BillPaidBy = Convert.ToInt32(ddlReservationId);
            //    guestBillPaymentBO.PaymentType = "Advance";
            //}

            //guestBillPaymentBO.PaymentDescription = paymentDescription;
            //guestBillPaymentBO.BankId = Convert.ToInt32(ddlBankId);
            //if (ddlPayMode == "M-Banking")
            //{
            //    guestBillPaymentBO.BankId = Convert.ToInt32(ddlMBankingBankId);
            //}

            //if (currencyType == "Local")
            //{
            //    guestBillPaymentBO.IsUSDTransaction = false;
            //    guestBillPaymentBO.FieldId = Int32.Parse(ddlCurrency);
            //    guestBillPaymentBO.ConvertionRate = 1;
            //    guestBillPaymentBO.CurrencyAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
            //    guestBillPaymentBO.PaymentAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
            //}
            //else
            //{
            //    guestBillPaymentBO.IsUSDTransaction = true;
            //    guestBillPaymentBO.FieldId = Int32.Parse(ddlCurrency);
            //    guestBillPaymentBO.ConvertionRate = !string.IsNullOrWhiteSpace(conversionRate) ? Convert.ToDecimal(conversionRate) : 1;
            //    guestBillPaymentBO.CurrencyAmount = !string.IsNullOrWhiteSpace(txtReceiveLeadgerAmount) ? Convert.ToDecimal(txtReceiveLeadgerAmount) : 0;
            //    guestBillPaymentBO.PaymentAmount = guestBillPaymentBO.CurrencyAmount * guestBillPaymentBO.ConvertionRate;
            //}

            //guestBillPaymentBO.ChecqueDate = DateTime.Now;
            //guestBillPaymentBO.PaymentMode = ddlPayMode;
            //guestBillPaymentBO.PaymentId = dynamicDetailId;
            //guestBillPaymentBO.CardNumber = txtCardNumber;
            //guestBillPaymentBO.CardType = ddlCardType;
            //if (string.IsNullOrEmpty(txtExpireDate))
            //{
            //    guestBillPaymentBO.ExpireDate = null;
            //}
            //else
            //{
            //    guestBillPaymentBO.ExpireDate = hmUtility.GetDateTimeFromString(txtExpireDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            //}
            //guestBillPaymentBO.ChecqueNumber = txtChecqueNumber;
            //guestBillPaymentBO.CardHolderName = txtCardHolderName;

            //guestBillPaymentBO.PaymentDescription = paymentDescription;

            //guestBillPaymentBO.PaymentId = dynamicDetailId;

            //guestPaymentDetailListForGrid.Add(guestBillPaymentBO);
            //HttpContext.Current.Session["GuestPaymentDetailListForGrid"] = guestPaymentDetailListForGrid;
            //return LoadGuestPaymentDetailGridViewByWM(paymentDescription);
            return string.Empty;
        }

        protected void btnFinish_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            BanquetReservationBO reservationBO = new BanquetReservationBO();
            BanquetReservationDA reservationDA = new BanquetReservationDA();
            BanquetInformationDA banInfoDa = new BanquetInformationDA();

            reservationBO.Id = Int64.Parse(ddlReservationId.SelectedValue);
            reservationBO = reservationDA.GetBanquetReservationInfoById(reservationBO.Id);

            //var hallName = reservationDA.GetHallName(reservationBO.ReservationNumber);

            BanquetInformationBO banquetInformationBO = banInfoDa.GetBanquetInformationById(reservationBO.BanquetId);




            //BanquetInformationBO banquet = new BanquetInformationBO();
            //BanquetInformationDA informationDA = new BanquetInformationDA();
            //banquet = informationDA.GetBanquetInformationById(reservationBO.Id);


            reservationBO.IsBillSettlement = true;
            reservationBO.CreatedBy = userInformationBO.UserInfoId;
            reservationBO.RebateRemarks = txtRebateRemarks.Text;
            reservationBO.MeetingDiscussion = txtMeetingDiscussion1.Value;

            reservationBO.CallToAction = txtCallToAction1.Value;

            int companyValue = Int32.Parse(ddlCompany1.SelectedValue);
            if (companyValue == 0)
            {
                reservationBO.IsUnderCompany = false;
            }
            else
            {
                reservationBO.IsUnderCompany = true;
            }

            string participantFromOfficeValue = hfparticipantFromOfficeValue.Value;
            reservationBO.PerticipantFromOfficeCommaSeperatedIds = participantFromOfficeValue;
            string participantFromClientValue = hfparticipantFromClientValue.Value;
            reservationBO.PerticipantFromClientCommaSeperatedIds = participantFromClientValue;

            //var participant = reservationBO.PerticipantFromOfficeCommaSeperatedIds.Split(new string[] { "," }, StringSplitOptions.None);
            var client = reservationBO.PerticipantFromClientCommaSeperatedIds.Split(new string[] { "," }, StringSplitOptions.None);


            BanquetReservationBO billBO = new BanquetReservationBO();
            billBO = reservationDA.GetBanquetReservationInfoById(reservationBO.Id);
            if (billBO.Id > 0)
            {
                reservationBO.DiscountType = billBO.DiscountType;
                reservationBO.DiscountAmount = billBO.DiscountAmount;
                reservationBO.CostCenterId = billBO.CostCenterId;
            }
            long tmpSalesId = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            List<GuestBillPaymentBO> guestPaymentDetailList = new List<GuestBillPaymentBO>();
            Boolean status = reservationDA.SettlementBanquetReservationInfo(reservationBO, guestPaymentDetailList, out tmpSalesId);
            if (status)
            {

                CommonHelper.AlertInfo(innboardMessage, "Your Program Finished Successfull.", AlertType.Success);
                hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.BanquetReservation.ToString(), reservationBO.Id, ProjectModuleEnum.ProjectModule.BanquetManagement.ToString(), "Banquet Reservation Bill Information Updated for Bill Settlement");
                Clear();
            }

            else
            {
                CommonHelper.AlertInfo(innboardMessage, "Operation Failed.", AlertType.Warning);
            }

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsBanquetInternalMeetingEmailSendEnable", "IsBanquetInternalMeetingEmailSendEnable");
            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (companyValue != 0)
                    {
                        ContactInformationDA emp = new ContactInformationDA();
                        CompanyDA companyDA = new CompanyDA();
                        CompanyBO companyBO = new CompanyBO();
                        companyBO = companyDA.GetCompanyInfoById(companyValue);
                        //companyBO = companyDA.GetCompanyInfoById(emp.GetContactInformationById);
                        HMCommonSetupBO SendEmailAddressBO = new HMCommonSetupBO();

                        bool send = false;

                        Email email;
                        SendEmailAddressBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
                        string mainString = SendEmailAddressBO.SetupValue;
                        EmployeeBO employeeBO = new EmployeeBO();
                        EmployeeDA empDa = new EmployeeDA();

                        int employeeId = Int32.Parse((reservationBO.RefferenceId).ToString());
                        var cordinator = empDa.GetEmployeeInfoById(employeeId);

                        foreach (var invitedEmployee in client)
                        {
                            var employee = emp.GetContactInformationById(int.Parse(invitedEmployee));
                            //int companyEmailId = (int)employee.CompanyId;
                            //var companyEmail = companyDA.GetCompanyInfoById(companyEmailId);

                            var tokens = new Dictionary<string, string>
                    {
                    {"Name",  employee.Name},
                    {"MeetingAgenda", reservationBO.MeetingAgenda },
                    {"MeetingDiscussion", reservationBO.MeetingDiscussion},
                    {"CallToAction", reservationBO.CallToAction },
                    {"Cordinator", cordinator.DisplayName },
                    {"CordinatorEmail", cordinator.OfficialEmail },
                    {"CordinatorPhone", cordinator.PresentPhone },
                    {"Date", reservationBO.ArriveDate.ToString() },
                    {"CompanyName", companyBO.CompanyName},
                    {"CompanyAddress", companyBO.CompanyAddress},
                    {"Email", companyBO.EmailAddress },
                    {"Venue",  banquetInformationBO.Name}
                    };

                            if (!string.IsNullOrEmpty(mainString))
                            {
                                string[] dataArray = mainString.Split('~');
                                email = new Email()
                                {
                                    From = dataArray[0],
                                    Password = dataArray[1],
                                    To = employee.Email,
                                    //FromDisplayName = ,
                                    //ToDisplayName = employee.Name,
                                    Subject = reservationBO.EventTitle,
                                    //Body = reservationBO.MeetingDiscussion,
                                    Host = dataArray[2],
                                    Port = dataArray[3],
                                    TempleteName = "ThanksEmployeeEmail.html"
                                };

                                try
                                {
                                    //Send Mail   
                                    send = EmailHelper.SendEmail(email, tokens);
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}