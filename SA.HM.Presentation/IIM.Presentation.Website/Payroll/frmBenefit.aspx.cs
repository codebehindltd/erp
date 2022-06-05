using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmBenefit : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {                
            }
            checkObjectPermission();
        }
        protected void checkObjectPermission()
        {
            btnSave.Visible = isSavePermission;
        }

        protected void gvBenefit_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
        }
        protected void gvBenefit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");

                //if (Convert.ToInt32(lblValue.Text) > 2)
                //{
                //    imgUpdate.Visible = isSavePermission;
                //    imgDelete.Visible = isDeletePermission;
                //}
                //else
                //{
                //    imgUpdate.Visible = false;
                //    imgDelete.Visible = false;
                //}
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;

            }
        }
        protected void gvBenefit_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long benefitId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(benefitId);
                this.btnSave.Visible = isUpdatePermission;
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("PayrollBenefitHead", "BenefitHeadId", benefitId);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                    EntityTypeEnum.EntityType.Benefit.ToString(), benefitId,
                    ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                    hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Benefit));
                }
                SearchBenefit();
                this.SetTab("SearchTab");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean status = false;
                BenefitHeadBO benefitBO = new BenefitHeadBO();
                BenefitDA benefitDA = new BenefitDA();

                benefitBO.BenefitName = txtBenefitName.Text;

                if (string.IsNullOrWhiteSpace(hfBenefitId.Value))
                {
                    long tmpBenefitId = 0;
                    status = benefitDA.SaveBenefitInfo(benefitBO, out tmpBenefitId);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.Benefit.ToString(), tmpBenefitId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Benefit));
                        this.Cancel();
                    }
                }
                else
                {
                    benefitBO.BenefitHeadId = Convert.ToInt32(hfBenefitId.Value);
                    status = benefitDA.UpdateBenefitInfo(benefitBO);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.Benefit.ToString(), benefitBO.BenefitHeadId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Benefit));
                        this.Cancel();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchBenefit();
            this.SetTab("SearchTab");
        }

        //************************ User Defined Function ********************//
        private void Cancel()
        {
            this.txtBenefitName.Text = string.Empty;
            this.btnSave.Visible = isSavePermission;
            this.btnSave.Text = "Save";
            this.hfBenefitId.Value = string.Empty;
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
        private void FillForm(long EditId)
        {
            BenefitHeadBO benefitBO = new BenefitHeadBO();
            BenefitDA benefitDA = new BenefitDA();
            benefitBO = benefitDA.GetBenefitInfoById(EditId);

            hfBenefitId.Value = benefitBO.BenefitHeadId.ToString();
            txtBenefitName.Text = benefitBO.BenefitName.ToString();
        }
        private void SearchBenefit()
        {
            List<BenefitHeadBO> benefitList = new List<BenefitHeadBO>();
            BenefitDA benefitDA = new BenefitDA();
            benefitList = benefitDA.GetAllBenefit();

            gvBenefit.DataSource = benefitList;
            gvBenefit.DataBind();
        }
    }
}