﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class frmCurrencyTransaction : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        protected int isNewAddButtonEnable = 1;
        protected int isSearchPanelEnable = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                LoadConversionHead();
            }
        }

        //private void LoadFromConversionHead()
        //{
        //    HMUtility hmUtility = new HMUtility();
        //    HMCommonDA hmCommonDA = new HMCommonDA();
        //    List<CustomFieldBO> list = new List<CustomFieldBO>();
        //    list = hmCommonDA.GetCustomFieldList("CurrencyConversionHead");
        //    this.ddlFromConversion.DataSource = list;
        //    this.ddlFromConversion.DataTextField = "FieldValue";
        //    this.ddlFromConversion.DataValueField = "FieldId";
        //    this.ddlFromConversion.DataBind();

        //    ListItem itemSelect = new ListItem();
        //    itemSelect.Value = "0";
        //    itemSelect.Text = hmUtility.GetDropDownFirstValue();
        //    this.ddlFromConversion.Items.Insert(0, itemSelect);
        //}
        //private void ToFromConversionHead()
        //{
        //    HMUtility hmUtility = new HMUtility();
        //    HMCommonDA hmCommonDA = new HMCommonDA();
        //    List<CustomFieldBO> list = new List<CustomFieldBO>();
        //    list = hmCommonDA.GetCustomFieldList("CurrencyConversionHead");
        //    this.ddlToConversion.DataSource = list;
        //    this.ddlToConversion.DataTextField = "FieldValue";
        //    this.ddlToConversion.DataValueField = "FieldId";
        //    this.ddlToConversion.DataBind();

        //    ListItem itemSelect = new ListItem();
        //    itemSelect.Value = "0";
        //    itemSelect.Text = hmUtility.GetDropDownFirstValue();
        //    this.ddlToConversion.Items.Insert(0, itemSelect);
        //}
        protected void gvCurrencyConversion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvCurrencyConversion.PageIndex = e.NewPageIndex;
            this.LoadGridView();
        }
        protected void gvCurrencyConversion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                ImageButton imgPreview = (ImageButton)e.Row.FindControl("ImgPaymentPreview");
                //imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                //imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                //imgUpdate.Visible = isSavePermission;
                //imgDelete.Visible = isDeletePermission;
                //imgDelete.Visible = false;
                imgPreview.Visible = false;
            }
        }
        protected void gvCurrencyConversion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int editId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(editId);
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
                //isSearchPanelEnable++;
            }
            else if (e.CommandName == "CmdPaymentPreview")
            {
                string url = "/HMCommon/Reports/frmReportCommonCurrencyConversion.aspx?ConversionIdList=" + Convert.ToInt32(e.CommandArgument.ToString());
                string sPopUp = "window.open('" + url + "', 'popup_window', 'width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);

                SearchLoadGridView();
                // this.SearchInformation();
                //this.SetTab("SearchTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("CommonCurrencyTransaction", "CurrencyConversionId", editId);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);                    
                }
                SearchLoadGridView();
                this.SetTab("SearchTab");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFrmValid())
            {
                return;
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            CommonCurrencyTransactionDA cmncurrencyconvDA = new CommonCurrencyTransactionDA();
            CommonCurrencyTransactionBO cmncurrencyconvBO = new CommonCurrencyTransactionBO();

            cmncurrencyconvBO.FromConversionHeadId = Convert.ToInt32(this.ddlFromConversion.SelectedValue);
            cmncurrencyconvBO.ToConversionHeadId = Convert.ToInt32(this.ddlToConversion.SelectedValue);
            cmncurrencyconvBO.ConversionAmount = Convert.ToDecimal(this.txtMoneyAmount.Text);
            cmncurrencyconvBO.ConversionRate = Convert.ToDecimal(this.txtConversionRate.Text);
            cmncurrencyconvBO.ConvertedAmount = Convert.ToDecimal(Request.Form[txtTotalAmount.UniqueID]);
            if (string.IsNullOrWhiteSpace(txtCommonConversionId.Value))
            {
                int temCurrencyConversionId = 0;
                cmncurrencyconvBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = cmncurrencyconvDA.SaveCommonCurrencyTransaction(cmncurrencyconvBO, out temCurrencyConversionId);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.CurrencyConversion.ToString(), temCurrencyConversionId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CurrencyConversion));
                    Cancel();
                    //this.LoadGridView();
                    //string url = "/HMCommon/Reports/frmReportCommonCurrencyConversion.aspx?ConversionIdList=" + temCurrencyConversionId;
                    //string sPopUp = "window.open('" + url + "', 'popup_window', 'width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1');";
                    //ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
                }
            }
            else
            {
                cmncurrencyconvBO.CurrencyConversionId = Convert.ToInt32(txtCommonConversionId.Value);
                cmncurrencyconvBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = cmncurrencyconvDA.UpdateCommonCurrencyConversion(cmncurrencyconvBO);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.CurrencyConversion.ToString(), cmncurrencyconvBO.CurrencyConversionId,
                ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CurrencyConversion));
                    Cancel();
                    //this.LoadGridView();
                    //string url = "/HMCommon/Reports/frmReportCommonCurrencyConversion.aspx?ConversionIdList=" + cmncurrencyconvBO.CurrencyConversionId;
                    //string sPopUp = "window.open('" + url + "', 'popup_window', 'width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1');";
                    //ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
                }
            }

            this.SetTab("EntryTab");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchLoadGridView();
            // isSearchPanelEnable++;
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            string paymentIdList = string.Empty;

            foreach (GridViewRow row in gvConversionInfo.Rows)
            {
                bool isSelect = ((CheckBox)row.FindControl("ChkSelect")).Checked;

                if (isSelect)
                {
                    Label lblObjectTabIdValue = (Label)row.FindControl("lblid");
                    if (string.IsNullOrWhiteSpace(paymentIdList.Trim()))
                    {
                        paymentIdList = paymentIdList + Convert.ToInt32(lblObjectTabIdValue.Text);
                    }
                    else
                    {
                        paymentIdList = paymentIdList + "," + Convert.ToInt32(lblObjectTabIdValue.Text);
                    }
                }
            }


            if (!string.IsNullOrWhiteSpace(paymentIdList))
            {
                string url = "/HMCommon/Reports/frmReportCommonCurrencyConversion.aspx?ConversionIdList=" + paymentIdList;
                string sPopUp = "window.open('" + url + "', 'popup_window', 'width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
            }

        }
        private bool IsFrmValid()
        {
            bool flag = true;

            if (this.ddlFromConversion.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "From Conversion.", AlertType.Warning);
                flag = false;
                ddlFromConversion.Focus();
            }
            else if (this.ddlToConversion.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "To Conversion.", AlertType.Warning);
                flag = false;
                ddlSToConversion.Focus();
            }
            else if (string.IsNullOrWhiteSpace(this.txtMoneyAmount.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Amount.", AlertType.Warning);
                flag = false;
                txtMoneyAmount.Focus();
            }
            else if (string.IsNullOrWhiteSpace(this.txtConversionRate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Conversion Rate.", AlertType.Warning);
                flag = false;
                txtConversionRate.Focus();
            }
            return flag;
        }
        private void LoadConversionHead()
        {
            //CommonCurrencyDA headDA = new CommonCurrencyDA();
            //this.ddlFromConversion.DataSource = headDA.GetConversionHeadInfoByType("Foreign");
            //this.ddlFromConversion.DataTextField = "ConversionHeadName";
            //this.ddlFromConversion.DataValueField = "ConversionHeadId";
            //this.ddlFromConversion.DataBind();

            //ListItem item = new ListItem();
            //item.Value = "0";
            //item.Text = hmUtility.GetDropDownFirstAllValue();
            //this.ddlFromConversion.Items.Insert(0, item);

            //this.ddlToConversion.DataSource = headDA.GetConversionHeadInfoByType("Local");
            //this.ddlToConversion.DataTextField = "ConversionHeadName";
            //this.ddlToConversion.DataValueField = "ConversionHeadId";
            //this.ddlToConversion.DataBind();
            //this.ddlToConversion.Items.Insert(0, item);

            //this.ddlSFromConversion.DataSource = headDA.GetConversionHeadInfoByType("Foreign");
            //this.ddlSFromConversion.DataTextField = "ConversionHeadName";
            //this.ddlSFromConversion.DataValueField = "ConversionHeadId";
            //this.ddlSFromConversion.DataBind();
            //this.ddlSFromConversion.Items.Insert(0, item);

            //this.ddlSToConversion.DataSource = headDA.GetConversionHeadInfoByType("Local");
            //this.ddlSToConversion.DataTextField = "ConversionHeadName";
            //this.ddlSToConversion.DataValueField = "ConversionHeadId";
            //this.ddlSToConversion.DataBind();
            //this.ddlSToConversion.Items.Insert(0, item);

            CommonCurrencyDA headDA = new CommonCurrencyDA();
            this.ddlFromConversion.DataSource = headDA.GetConversionHeadInfoByType("NLocal");
            this.ddlFromConversion.DataTextField = "CurrencyName";
            this.ddlFromConversion.DataValueField = "CurrencyId";
            this.ddlFromConversion.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlFromConversion.Items.Insert(0, item);

            this.ddlToConversion.DataSource = headDA.GetConversionHeadInfoByType("Local");
            this.ddlToConversion.DataTextField = "CurrencyName";
            this.ddlToConversion.DataValueField = "CurrencyId";
            this.ddlToConversion.DataBind();
            this.ddlToConversion.Items.Insert(0, item);

            this.ddlSFromConversion.DataSource = headDA.GetConversionHeadInfoByType("NLocal");
            this.ddlSFromConversion.DataTextField = "CurrencyName";
            this.ddlSFromConversion.DataValueField = "CurrencyId";
            this.ddlSFromConversion.DataBind();
            this.ddlSFromConversion.Items.Insert(0, item);

            this.ddlSToConversion.DataSource = headDA.GetConversionHeadInfoByType("Local");
            this.ddlSToConversion.DataTextField = "CurrencyName";
            this.ddlSToConversion.DataValueField = "CurrencyId";
            this.ddlSToConversion.DataBind();
            this.ddlSToConversion.Items.Insert(0, item);
        }

        private void LoadGridView()
        {
            //this.CheckObjectPermission();
            CommonCurrencyTransactionDA cmncurrencyconvDA = new CommonCurrencyTransactionDA();
            List<CommonCurrencyTransactionBO> files = cmncurrencyconvDA.GetCommonCurrencyConversion();

            isSearchPanelEnable = 1;
            this.gvCurrencyConversion.DataSource = files;
            this.gvCurrencyConversion.DataBind();


            //this.btnPaymentPreview.Visible = true;
        }
        private void SearchLoadGridView()
        {
            DateTime? fromDate = null;
            DateTime? toDate = null;
            //this.CheckObjectPermission();
            int fromId = Convert.ToInt32(ddlSFromConversion.SelectedValue);
            int toId = Convert.ToInt32(ddlSToConversion.SelectedValue);
            if (!string.IsNullOrWhiteSpace(txtFromDate.Text))
            {
                fromDate = hmUtility.GetDateTimeFromString(txtFromDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            if (!string.IsNullOrWhiteSpace(txtToDate.Text))
            {
                toDate = hmUtility.GetDateTimeFromString(txtToDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            CommonCurrencyTransactionDA cmncurrencyconvDA = new CommonCurrencyTransactionDA();
            List<CommonCurrencyTransactionBO> files = cmncurrencyconvDA.GetAllCommonCurrencyConversion(fromId, toId, fromDate, toDate);

            isSearchPanelEnable = 1;
            this.gvCurrencyConversion.DataSource = files;
            this.gvCurrencyConversion.DataBind();

            this.gvConversionInfo.DataSource = files;
            this.gvConversionInfo.DataBind();
            SetTab("SearchTab");
            this.btnPreview.Visible = true;
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
        public void FillForm(int EditId)
        {
            CommonCurrencyTransactionDA cmncurrencyconvDA = new CommonCurrencyTransactionDA();
            CommonCurrencyTransactionBO cmncurrencyconvBO = new CommonCurrencyTransactionBO();
            cmncurrencyconvBO = cmncurrencyconvDA.GetCommonCurrencyConversionById(EditId);

            txtCommonConversionId.Value = cmncurrencyconvBO.CurrencyConversionId.ToString();
            txtMoneyAmount.Text = cmncurrencyconvBO.ConversionAmount.ToString();
            txtConversionRate.Text = cmncurrencyconvBO.ConversionRate.ToString();
            txtTotalAmount.Text = cmncurrencyconvBO.ConvertedAmount.ToString();
            ddlFromConversion.SelectedValue = cmncurrencyconvBO.FromConversionHeadId.ToString();
            ddlToConversion.SelectedValue = cmncurrencyconvBO.ToConversionHeadId.ToString();
            this.btnSave.Text = "Update";
        }
        private void Cancel()
        {
            ddlFromConversion.SelectedIndex = 0;
            ddlToConversion.SelectedIndex = 0;
            txtMoneyAmount.Text = string.Empty;
            txtConversionRate.Text = string.Empty;
            txtTotalAmount.Text = string.Empty;
        }

        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmHMConvertCurrency.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        //private void SetTab(string TabName)
        //{
        //    if (TabName == "SearchTab")
        //    {
        //        B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
        //        A.Attributes.Add("class", "ui-state-default ui-corner-top");
        //    }
        //    else if (TabName == "EntryTab")
        //    {
        //        A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
        //        B.Attributes.Add("class", "ui-state-default ui-corner-top");
        //    }
        //}
        [WebMethod]
        public static CommonCurrencyConversionBO GetConversionRateByHeadId(string FromHeadId, string ToHeadId)
        {
            CommonCurrencyConversionBO setupBO = new CommonCurrencyConversionBO();
            CommonCurrencyConversionDA setupDA = new CommonCurrencyConversionDA();
            setupBO = setupDA.GetCurrencyConversionInfoByCurrencyId(Int32.Parse(FromHeadId), Int32.Parse(ToHeadId));

            if (setupBO.ConversionRate == 0)
            {
                setupBO.ConversionRate = 1;
            }
            return setupBO;
        }


    }
}