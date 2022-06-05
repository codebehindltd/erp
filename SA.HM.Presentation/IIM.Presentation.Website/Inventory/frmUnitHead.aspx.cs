using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class frmUnitHead : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (Session["StatusUpdate"] != null)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                Session["StatusUpdate"] = null;
            }
            if (!IsPostBack)
            {
                string editId = Request.QueryString["editId"];
                if (!string.IsNullOrWhiteSpace(editId))
                {
                    int Id = Convert.ToInt32(editId);
                    if (Id > 0)
                    {
                        FillForm(Id);
                    }
                }
                CheckPermission();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsFrmValid())
                {
                    return;
                }

                InvUnitHeadBO headBO = new InvUnitHeadBO();
                InvUnitHeadDA headDA = new InvUnitHeadDA();
                int tmpId;
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                headBO.HeadName = this.txtHeadName.Text;
                headBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;
                if (string.IsNullOrWhiteSpace(hfUnitHeadId.Value))
                {
                    headBO.CreatedBy = userInformationBO.UserInfoId;
                    bool status = headDA.SaveHeadInfo(headBO, out tmpId);
                    if (status)
                    {
                        DefaultUnitConversionRateSetup(tmpId);
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                       EntityTypeEnum.EntityType.ConversionHead.ToString(), tmpId,
                       ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                       hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ConversionHead));
                        this.Cancel();
                    }
                }
                else
                {
                    {
                        headBO.UnitHeadId = Convert.ToInt32(hfUnitHeadId.Value);
                        headBO.LastModifiedBy = userInformationBO.UserInfoId;
                        Boolean status = headDA.UpdateHeadInfo(headBO);
                        if (status)
                        {
                            bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.ConversionHead.ToString(), headBO.UnitHeadId,
                            ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ConversionHead));
                            this.Cancel();

                            var nvc = HttpUtility.ParseQueryString(Request.Url.Query);
                            nvc.Remove("editId");
                            string url = Request.Url.AbsolutePath + "?Status=Update" + nvc.ToString();
                            Session["StatusUpdate"] = url;
                            Response.Redirect(url);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        private void DefaultUnitConversionRateSetup(int UnitHeadId)
        {
            InvUnitConversionRateBO conversionBO = new InvUnitConversionRateBO();
            InvUnitConversionRateDA conversionDA = new InvUnitConversionRateDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            conversionBO.CreatedBy = userInformationBO.UserInfoId;
            conversionBO.LastModifiedBy = userInformationBO.UserInfoId;
            conversionBO.LastModifiedBy = userInformationBO.LastModifiedBy;
            conversionBO.FromUnitHeadId = UnitHeadId;
            conversionBO.ToUnitHeadId = UnitHeadId;
            conversionBO.ConversionRate = 1;

            int temconversionRateId = 0;
            Boolean status = conversionDA.SaveOrUpdateUnitConversionRate(conversionBO, out temconversionRateId);
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.txtHeadName.Text = string.Empty;
            this.ddlActiveStat.SelectedIndex = 0;
            this.hfUnitHeadId.Value = string.Empty;
        }
        public void FillForm(int EditId)
        {
            InvUnitHeadBO headBO = new InvUnitHeadBO();
            InvUnitHeadDA headDA = new InvUnitHeadDA();
            headBO = headDA.GetHeadInfoById(EditId);

            ddlActiveStat.SelectedValue = (headBO.ActiveStat == true ? 0 : 1).ToString();
            hfUnitHeadId.Value = headBO.UnitHeadId.ToString();
            txtHeadName.Text = headBO.HeadName.ToString();
            btnSave.Visible = isUpdatePermission;
            this.btnSave.Text = "Update";
        }
        private bool IsFrmValid()
        {
            bool flag = true;

            if (txtHeadName.Text == "")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Head Name.", AlertType.Warning);
                flag = false;
                txtHeadName.Focus();
            }
            return flag;
        }
        private void Cancel()
        {
            this.txtHeadName.Text = string.Empty;
            this.ddlActiveStat.SelectedIndex = 0;
            this.btnSave.Text = "Save";
        }


        [WebMethod]
        public static GridViewDataNPaging<InvUnitHeadBO, GridPaging> SearchHeadAndLoadGridInformation(string headName, Boolean activeStat, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<InvUnitHeadBO, GridPaging> myGridData = new GridViewDataNPaging<InvUnitHeadBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            InvUnitHeadDA headDA = new InvUnitHeadDA();
            List<InvUnitHeadBO> headList = new List<InvUnitHeadBO>();
            headList = headDA.GetHeadInformationBySearchCriteriaForPaging(headName, activeStat, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<InvUnitHeadBO> distinctItems = new List<InvUnitHeadBO>();
            distinctItems = headList.GroupBy(test => test.UnitHeadId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo DeleteData(int sEmpId)
        {
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("InvUnitHead", "UnitHeadId", sEmpId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                       EntityTypeEnum.EntityType.ConversionHead.ToString(), sEmpId,
                       ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(),
                       hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ConversionHead));
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninf;
        }

        private void CheckPermission()
        {
            btnSave.Visible = isSavePermission;
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }
    }
}