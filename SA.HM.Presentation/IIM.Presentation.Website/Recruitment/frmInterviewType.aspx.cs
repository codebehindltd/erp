using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.Recruitment
{
    public partial class frmInterviewType : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        private Boolean isSavePermission = true;
        private Boolean isDeletePermission = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.LoadGridView();
                this.SetTab("EntryTab");
            }
        }

        protected void gvInterviewType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvInterviewType.PageIndex = e.NewPageIndex;
            //this.CheckObjectPermission();
            this.LoadGridView();
        }
        protected void gvInterviewType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                //  imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                //  imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isSavePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvInterviewType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int interviewTypeId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(interviewTypeId);
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                try
                {
                    Boolean status = hmCommonDA.DeleteInfoById("PayrollInterviewType", "InterviewTypeId", interviewTypeId);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                    }
                    LoadGridView();
                    this.SetTab("SearchTab");
                }
                catch (Exception ex)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtInterviewType.Text))
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Interview Type.", AlertType.Warning);
                    txtInterviewType.Focus();
                }
                else if (string.IsNullOrWhiteSpace(txtMarks.Text))
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Marks.", AlertType.Warning);
                    txtMarks.Focus();
                }
                else
                {
                    InterviewTypeBO typeBO = new InterviewTypeBO();
                    InterviewTypeDA interviewTypeDA = new InterviewTypeDA();

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    typeBO.InterviewName = txtInterviewType.Text;
                    typeBO.Marks = Convert.ToDecimal(txtMarks.Text);
                    typeBO.Remarks = txtRemarks.Text;

                    if (string.IsNullOrWhiteSpace(hfInterviewTypeId.Value))
                    {
                        int tmpInterviewTypeId = 0;
                        typeBO.CreatedBy = userInformationBO.UserInfoId;
                        Boolean status = interviewTypeDA.SaveInterviewTypeInfo(typeBO, out tmpInterviewTypeId);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                            this.LoadGridView();
                            this.Cancel();
                        }
                    }
                    else
                    {
                        typeBO.InterviewTypeId = Convert.ToInt16(hfInterviewTypeId.Value);
                        typeBO.LastModifiedBy = userInformationBO.UserInfoId;
                        Boolean status = interviewTypeDA.UpdateInterviewTypeInfo(typeBO);
                        if (status)
                        {
                            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                            this.LoadGridView();
                            this.Cancel();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }

        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtInterviewType.Text = string.Empty;
            txtMarks.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            this.btnSave.Text = "Save";
            hfInterviewTypeId.Value = string.Empty;
        }

        private void LoadGridView()
        {
            //this.CheckObjectPermission();

            List<InterviewTypeBO> interviewTypeList = new List<InterviewTypeBO>();
            InterviewTypeDA interviewTypeDA = new InterviewTypeDA();

            interviewTypeList = interviewTypeDA.GetInterviewTypeList();

            this.gvInterviewType.DataSource = interviewTypeList;
            this.gvInterviewType.DataBind();
            SetTab("SearchTab");
        }
        public void FillForm(int editId)
        {
            InterviewTypeBO interviewTypeBO = new InterviewTypeBO();
            InterviewTypeDA interviewTypeDA = new InterviewTypeDA();
            interviewTypeBO = interviewTypeDA.GetDisciplinaryActionTypeById(editId);
            txtInterviewType.Text = interviewTypeBO.InterviewName;
            txtMarks.Text = interviewTypeBO.Marks.ToString();
            txtRemarks.Text = interviewTypeBO.Remarks;
            hfInterviewTypeId.Value = interviewTypeBO.InterviewTypeId.ToString();
            btnSave.Text = "Update";
        }
        private void Cancel()
        {
            txtInterviewType.Text = string.Empty;
            txtMarks.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            this.btnSave.Text = "Save";
            hfInterviewTypeId.Value = string.Empty;
            this.SetTab("EntryTab");
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
    }
}