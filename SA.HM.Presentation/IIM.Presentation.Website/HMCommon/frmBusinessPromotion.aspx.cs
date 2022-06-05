using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using System.Web.Services;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class frmBusinessPromotion : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected int isBankInformationDivEnable = -1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
            if (!string.IsNullOrWhiteSpace(DeleteSuccess))
            {               
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
            }

            if (!IsPostBack)
            {
                DateTime mamun1 = hmUtility.SuprimaIntToDateTime(1441299267);
                DateTime mamun2 = hmUtility.SuprimaIntToDateTime(1441299324);
                DateTime mamun3 = hmUtility.SuprimaIntToDateTime(1441300003);

                this.LoadCurrentDate();
                this.LoadBank();
                this.LoadBankCardTypeInfo();
                SetDefaulTime();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridView();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            DateTime resultDate;
            if (string.IsNullOrWhiteSpace(this.txtBPHead.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Name.", AlertType.Warning);
                this.txtBPHead.Focus();
            }
            else if (String.IsNullOrEmpty(txtPeriodFrom.Text) || !DateTime.TryParse(txtPeriodFrom.Text,out resultDate))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Date From.", AlertType.Warning);
                this.txtPeriodFrom.Focus();
            
            }
            else if (String.IsNullOrEmpty(txtPeriodTo.Text) || !DateTime.TryParse(txtPeriodTo.Text, out resultDate))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Date To.", AlertType.Warning);
                this.txtPeriodTo.Focus();

            }
            else if (string.IsNullOrWhiteSpace(this.txtPercentAmount.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Percent Amount.", AlertType.Warning);
                this.txtPercentAmount.Focus();
            }
            else
            {
                BusinessPromotionBO bpBO = new BusinessPromotionBO();
                BusinessPromotionDA bpDA = new BusinessPromotionDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                bpBO.BPHead = this.txtBPHead.Text;

                //int pFMin = !string.IsNullOrWhiteSpace(this.txtPrbFromMinute.Text) ? Convert.ToInt32(this.txtPrbFromMinute.Text) : 0;
                //int pFHour = this.ddlPrbFromAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtPrbFromHour.Text) % 12) : ((Convert.ToInt32(this.txtPrbFromHour.Text) % 12) + 12);
                string FromTime = (txtPrbFromHour.Text.Replace("AM", "")).Replace("PM", "");
                string[] fTime = FromTime.Split(':');
                int pFHour = Convert.ToInt32(fTime[0]);
                int pFMin = Convert.ToInt32(fTime[1]);
                bpBO.PeriodFrom = hmUtility.GetDateTimeFromString(this.txtPeriodFrom.Text, userInformationBO.ServerDateFormat).AddHours(pFHour).AddMinutes(pFMin);

                //int pTMin = !string.IsNullOrWhiteSpace(this.txtPrbToMinute.Text) ? Convert.ToInt32(this.txtPrbToMinute.Text) : 0;
                //int pTHour = this.ddlPrbToAMPM.SelectedIndex == 0 ? (Convert.ToInt32(this.txtPrbToHour.Text) % 12) : ((Convert.ToInt32(this.txtPrbToHour.Text) % 12) + 12);
                string ToTime = (txtPrbToHour.Text.Replace("AM", "")).Replace("PM", "");
                string[] tTime = ToTime.Split(':');
                int pTHour = Convert.ToInt32(tTime[0]);
                int pTMin = Convert.ToInt32(tTime[1]);
                bpBO.PeriodTo = hmUtility.GetDateTimeFromString(this.txtPeriodTo.Text, userInformationBO.ServerDateFormat).AddHours(pTHour).AddMinutes(pTMin);

                //bpBO.PeriodFrom = hmUtility.GetDateTimeFromString(this.txtPeriodFrom.Text, userInformationBO.ServerDateFormat);
                //bpBO.PeriodTo = hmUtility.GetDateTimeFromString(this.txtPeriodTo.Text, userInformationBO.ServerDateFormat);
                bpBO.PercentAmount = Convert.ToDecimal(this.txtPercentAmount.Text);
                bpBO.TransactionType = this.ddlTransactionType.SelectedValue;
                bpBO.IsBPPublic = this.chkIsBPPublic.Checked == true ? true : false;
                bpBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;

                List<BusinessPromotionDetailBO> businessPromotionDetailBO = new List<BusinessPromotionDetailBO>();
                if (bpBO.TransactionType == "Bank")
                {
                    BusinessPromotionDetailBO bankInfo = new BusinessPromotionDetailBO();
                    bankInfo.TransactionType = "Bank";
                    bankInfo.TransactionId = Convert.ToInt32(this.ddlBankId.SelectedValue);
                    businessPromotionDetailBO.Add(bankInfo);

                    BusinessPromotionDetailBO cardTypeInfo = new BusinessPromotionDetailBO();
                    cardTypeInfo.TransactionType = "CardType";
                    cardTypeInfo.TransactionId = Convert.ToInt32(this.ddlCardType.SelectedValue);
                    businessPromotionDetailBO.Add(cardTypeInfo);
                }
                
                if (string.IsNullOrWhiteSpace(txtBusinessPromotionId.Value))
                {
                    int tmpBPId = 0;
                    bpBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = bpDA.SaveBusinessPromotionInfo(bpBO, businessPromotionDetailBO, out tmpBPId);
                    if (status)
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.BussinessPromotion.ToString(), tmpBPId,
                        ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BussinessPromotion));
                        this.Cancel();
                    }
                }
                else
                {
                    bpBO.BusinessPromotionId = Convert.ToInt32(txtBusinessPromotionId.Value);
                    bpBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = bpDA.UpdateBusinessPromotionInfo(bpBO, businessPromotionDetailBO);
                    if (status)
                    {                        
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.BussinessPromotion.ToString(), bpBO.BusinessPromotionId,
                            ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BussinessPromotion));
                        this.Cancel();
                    }
                }
                if (gvGuestHouseService.Rows.Count>0)
                {
                    this.LoadGridView();
                }
                this.SetTab("EntryTab");
            }
        }
        protected void gvGuestHouseService_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {            
            this.gvGuestHouseService.PageIndex = e.NewPageIndex;
            this.CheckObjectPermission();
            this.LoadGridView();
        }
        protected void gvGuestHouseService_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                //  imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                //  imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                //imgUpdate.Visible = isSavePermission;
                //imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvGuestHouseService_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int BPId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(BPId);
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("CommonBusinessPromotion", "BusinessPromotionId", BPId);
                if (status)
                {                    
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.BussinessPromotion.ToString(), BPId,
                       ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.BussinessPromotion));
                }
                LoadGridView();
                this.SetTab("SearchTab");
            }
        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission()
        {
            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            //ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            //objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmBank.ToString());

            //isSavePermission = objectPermissionBO.IsSavePermission;
            //isDeletePermission = objectPermissionBO.IsDeletePermission;
            //btnSave.Visible = isSavePermission;
            //if (!isSavePermission)
            //{
            //    isNewAddButtonEnable = -1;
            //}
        }
        private void LoadBank()
        {
            BankDA bankDA = new BankDA();
            List<BankBO> entityBOList = new List<BankBO>();
            entityBOList = bankDA.GetBankInfo();

            this.ddlBankId.DataSource = entityBOList;
            this.ddlBankId.DataTextField = "BankName";
            this.ddlBankId.DataValueField = "BankId";
            this.ddlBankId.DataBind();

            ListItem itemBank = new ListItem();
            itemBank.Value = "0";
            itemBank.Text = hmUtility.GetDropDownFirstValue();
            this.ddlBankId.Items.Insert(0, itemBank);
        }
        private void LoadBankCardTypeInfo()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("BankCardType", hmUtility.GetDropDownFirstAllValue());

            this.ddlCardType.DataSource = fields;
            this.ddlCardType.DataTextField = "FieldValue";
            this.ddlCardType.DataValueField = "FieldId";
            this.ddlCardType.DataBind();
            this.ddlCardType.SelectedIndex = 0;
        }
        private void LoadGridView()
        {

            string BPHead = txtSName.Text;
            Boolean ActiveStat = ddlSActiveStat.SelectedIndex == 0 ? true : false;
            this.CheckObjectPermission();

            BusinessPromotionDA da = new BusinessPromotionDA();
            List<BusinessPromotionBO> files = da.GetBusinessPromotionInfoBySearchCriteria(BPHead, ActiveStat);

            this.gvGuestHouseService.DataSource = files;
            this.gvGuestHouseService.DataBind();
            SetTab("SearchTab");
        }
        private void SetTab(string TabName)
        {
            if (TabName == "SearchTab")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "EntryTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtPeriodFrom.Text = hmUtility.GetStringFromDateTime(dateTime);
            this.txtPeriodTo.Text = hmUtility.GetStringFromDateTime(dateTime);
        }
        private void Cancel()
        {
            this.LoadCurrentDate();
            this.txtBPHead.Text = string.Empty;
            this.txtPercentAmount.Text = "0";
            this.ddlActiveStat.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            this.txtBusinessPromotionId.Value = string.Empty;
            this.ddlTransactionType.SelectedValue = "Others";
            this.txtBPHead.Focus();
            txtPrbFromHour.Text = string.Empty;
            //txtPrbFromMinute.Text = string.Empty;
            //ddlPrbFromAMPM.SelectedIndex = 0;
            txtPrbToHour.Text = string.Empty;
            //txtPrbToMinute.Text = string.Empty;
            //ddlPrbToAMPM.SelectedIndex = 0;
        }
        public void FillForm(int EditId)
        {
            BusinessPromotionBO bpBO = new BusinessPromotionBO();
            BusinessPromotionDA bpDA = new BusinessPromotionDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            bpBO = bpDA.GetBusinessPromotionInfoById(EditId);
            this.txtBusinessPromotionId.Value = bpBO.BusinessPromotionId.ToString();
            this.ddlActiveStat.SelectedValue = (bpBO.ActiveStat == true ? 0 : 1).ToString();
            this.txtBPHead.Text = bpBO.BPHead;

            this.txtPeriodFrom.Text = hmUtility.GetStringFromDateTime(bpBO.PeriodFrom);
            this.txtPrbFromHour.Text = bpBO.PeriodFrom.ToString(userInformationBO.TimeFormat);
            //this.txtPrbFromHour.Text = Convert.ToInt32(bpBO.PeriodFrom.ToString("%h")) == 0 ? "12" : bpBO.PeriodFrom.ToString("%h");
            //this.txtPrbFromMinute.Text = bpBO.PeriodFrom.ToString("mm");            

            //DateTime FromTime = Convert.ToDateTime(bpBO.PeriodFrom);
            //string S = FromTime.ToString("tt");
            //this.ddlPrbFromAMPM.SelectedIndex = S == "AM" ? 0 : 1;

            this.txtPeriodTo.Text = hmUtility.GetStringFromDateTime(bpBO.PeriodTo);
            this.txtPrbToHour.Text = bpBO.PeriodTo.ToString(userInformationBO.TimeFormat);
            //this.txtPrbToHour.Text = Convert.ToInt32(bpBO.PeriodTo.ToString("%h")) == 0 ? "12" : bpBO.PeriodTo.ToString("%h");
            //this.txtPrbToMinute.Text = bpBO.PeriodTo.ToString("mm");

            //DateTime ToTime = Convert.ToDateTime(bpBO.PeriodTo);
            //string SS = ToTime.ToString("tt");
            //this.ddlPrbToAMPM.SelectedIndex = SS == "AM" ? 0 : 1;

            //this.txtPeriodFrom.Text = hmUtility.GetStringFromDateTime(bpBO.PeriodFrom);
            //this.txtPeriodTo.Text = hmUtility.GetStringFromDateTime(bpBO.PeriodTo);
            this.txtPercentAmount.Text = bpBO.PercentAmount.ToString();
            this.ddlTransactionType.SelectedValue = bpBO.TransactionType;
            if (bpBO.IsBPPublic == true)
            {
                chkIsBPPublic.Checked = true;
            }
            else
            {
                chkIsBPPublic.Checked = false;
            }

            if (bpBO.TransactionType == "Bank")
            {
                isBankInformationDivEnable = 1;
                BusinessPromotionDA commonDA = new BusinessPromotionDA();
                BusinessPromotionBO businessPromotionBO = new BusinessPromotionBO();
                List<BusinessPromotionBO> businessPromotionBOList = commonDA.LoadBusinessPromotionRelatedInformation(bpBO.BusinessPromotionId);
                if (businessPromotionBOList != null)
                {
                    if (businessPromotionBOList.Count == 1)
                    {
                        this.ddlBankId.SelectedValue = businessPromotionBOList[0].TransactionId.ToString();
                    }
                    else if (businessPromotionBOList.Count == 2)
                    {
                        this.ddlBankId.SelectedValue = businessPromotionBOList[0].TransactionId.ToString();
                        this.ddlCardType.SelectedValue = businessPromotionBOList[1].TransactionId.ToString();
                    }
                }
            }

        }
        private void SetDefaulTime()
        {
            this.txtPrbFromHour.Text = "12:00";
            this.txtPrbToHour.Text = "12:00";            
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
    }
}