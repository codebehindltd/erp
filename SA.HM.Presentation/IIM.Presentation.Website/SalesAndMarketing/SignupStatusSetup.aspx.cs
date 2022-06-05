using HotelManagement.Data.HMCommon;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class SignupStatusSetup : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        HMCommonDA hmCommonDA = new HMCommonDA();
        CompanySignupStatusDA statusDA = new CompanySignupStatusDA();
        HiddenField innboardMessage;

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SMCompanySignupStatus signupStatus = new SMCompanySignupStatus();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            signupStatus.Id = Convert.ToInt32(hfStatusId.Value);
            signupStatus.Status = txtName.Text;
            signupStatus.IsActive = (ddlActiveStatus.SelectedValue == "0");

            int id = 0;
            signupStatus.CreatedBy = userInformationBO.UserInfoId;

            if (signupStatus.Id == 0)
            {
                bool isExist = hmCommonDA.DuplicateCheckDynamicaly("SMCompanySignupStatus", "Status", signupStatus.Status,0,"Id",signupStatus.Id.ToString()) > 0;

                if (isExist)
                {
                    CommonHelper.AlertInfo(innboardMessage, string.Format("Your entered {0} {1} is already exist.", lblName.Text, signupStatus.Status), AlertType.Warning);
                    this.Cancel();
                    return;
                }
            }
            else
            {
                bool isExist = hmCommonDA.DuplicateCheckDynamicaly("SMCompanySignupStatus", "Status", signupStatus.Status, 1, "Id", signupStatus.Id.ToString()) > 0;

                if (isExist)
                {
                    CommonHelper.AlertInfo(innboardMessage, string.Format("Your entered {0} {1} is already exist.", lblName.Text, signupStatus.Status), AlertType.Warning);
                    this.Cancel();
                    return;
                }
            }
            try
            {
                Boolean status = statusDA.SaveOrUpdateSignupStatus(signupStatus, out id);

                if (status)
                {
                    if (signupStatus.Id == 0)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.SMCompanySignupStatus.ToString(), id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(),
                                hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMCompanySignupStatus));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    }
                    else
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.SMCompanySignupStatus.ToString(), signupStatus.Id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(),
                                hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMCompanySignupStatus));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        LoadStatusGrid();
                    }
                    this.Cancel();
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                throw ex;
            }
            SetTab("EntryTab");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadStatusGrid();
            SetTab("SearchTab");
        }

        protected void gvSignupStatus_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "CmdEdit")
            {
                FillForm(id);
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = false;
                try
                {
                    status = hmCommonDA.DeleteInfoById("SMCompanySignupStatus", "Id", id);
                }
                catch (SqlException ex)
                {
                    if (ex.Errors[0].Number == 547)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.ForeignKeyValidation, AlertType.Warning);
                    }
                    else
                        throw ex;
                }
                catch (Exception ex)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    throw ex;
                }
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.SMCompanySignupStatus.ToString(), id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(),
                                hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMCompanySignupStatus));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);

                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
                LoadStatusGrid();
                this.SetTab("SearchTab");
            }
        }

        protected void gvSignupStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");

                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");

                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;

                SMCompanySignupStatus rowData = (SMCompanySignupStatus)e.Row.DataItem;

                e.Row.Cells[2].Text = rowData.IsActive ? "Active" : "Inactive";
            }
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
        private void Cancel()
        {
            hfStatusId.Value = "0";
            this.txtName.Text = string.Empty;
            this.ddlActiveStatus.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            this.txtName.Focus();
        }
        private void LoadStatusGrid()
        {
            string name = txtSearchName.Text == "" ? null : txtSearchName.Text;
            int isActive = Convert.ToInt32(ddlSearchActiveStatus.SelectedValue);

            List<SMCompanySignupStatus> signupStatus = statusDA.GetSignupStatusBySearchCriteria(name, isActive);

            gvSignupStatus.DataSource = signupStatus;
            gvSignupStatus.DataBind();
        }
        private void FillForm(int id)
        {
            SMCompanySignupStatus signupStatus = statusDA.GetSignupStatusById(id);

            txtName.Text = signupStatus.Status;
            ddlActiveStatus.SelectedValue = signupStatus.IsActive ? "0" : "1";
            hfStatusId.Value = signupStatus.Id.ToString();
        }
    }
}