using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmHMPaidService : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadCurrency();
                string editId = Request.QueryString["editId"];
                if (!string.IsNullOrWhiteSpace(editId))
                {
                    int Id = Convert.ToInt32(editId);
                    if (Id > 0)
                    {
                        FillForm(Id);
                    }
                }
                LoadCostCentre();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFrmValid())
            {
                return;
            }
            lblMessage.Text = string.Empty;

            HMPaidServiceBO paidServiceBO = new HMPaidServiceBO();
            HMPaidServiceDA paidServiceDA = new HMPaidServiceDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            paidServiceBO.ServiceName = this.txtServiceName.Text;
            paidServiceBO.Description = this.txtDescription.Text;
            paidServiceBO.ServiceType = this.ddlServiceType.SelectedIndex == 1 ? "Daily" : "PerStay";
            if (!string.IsNullOrWhiteSpace(this.txtSellingPriceLocal.Text))
            {
                paidServiceBO.UnitPriceLocal = Convert.ToDecimal(this.txtSellingPriceLocal.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.txtSellingPriceUsd.Text))
            {
                paidServiceBO.UnitPriceUsd = Convert.ToDecimal(this.txtSellingPriceUsd.Text);
            }
            paidServiceBO.ActiveStat = this.ddlActiveStat.SelectedIndex == 0 ? true : false;
            paidServiceBO.CostCenterId = Convert.ToInt32(this.ddlCostCentre.SelectedItem.Value);

            if (String.IsNullOrWhiteSpace(txtPaidServiceId.Value))
            {
                paidServiceBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = paidServiceDA.SaveHMPaidServiceInfo(paidServiceBO);
                if (status)
                {
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.HMPaidService.ToString(),paidServiceBO.PaidServiceId,ProjectModuleEnum.ProjectModule.FrontOffice.ToString(),hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HMPaidService));
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Saved Operation Successfull";
                    this.Cancel();
                }
            }
            else
            {
                paidServiceBO.PaidServiceId = Convert.ToInt32(txtPaidServiceId.Value);
                paidServiceBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = paidServiceDA.UpdateHMPaidServiceInfo(paidServiceBO);
                if (status)
                {
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.HMPaidService.ToString(), paidServiceBO.PaidServiceId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HMPaidService));
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Update Operation Successfull";
                    this.Cancel();
                }
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.txtServiceName.Text = string.Empty;
            this.txtDescription.Text = string.Empty;
            this.txtSellingPriceLocal.Text = string.Empty;
            this.txtSellingPriceUsd.Text = string.Empty;
            this.ddlCostCentre.SelectedIndex = 0;
            this.ddlServiceType.SelectedIndex = 0;
            this.ddlActiveStat.SelectedIndex = 0;
        }
        //************************ User Defined Function ********************//
        private void LoadCurrency()
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
            this.ddlSellingPriceLocal.DataSource = fields;
            this.ddlSellingPriceLocal.DataTextField = "FieldValue";
            this.ddlSellingPriceLocal.DataValueField = "FieldId";
            this.ddlSellingPriceLocal.DataBind();
            this.ddlSellingPriceLocal.SelectedIndex = 0;
            this.lblSellingPriceLocal.Text = "Unit Price(" + this.ddlSellingPriceLocal.SelectedItem.Text + ")";


            this.ddlSellingPriceUsd.DataSource = fields;
            this.ddlSellingPriceUsd.DataTextField = "FieldValue";
            this.ddlSellingPriceUsd.DataValueField = "FieldId";
            this.ddlSellingPriceUsd.DataBind();
            this.ddlSellingPriceUsd.SelectedIndex = 1;
            this.lblSellingPriceUsd.Text = "Unit Price(" + this.ddlSellingPriceUsd.SelectedItem.Text + ")";
        }
        private void LoadCostCentre()
        {
            CostCentreTabDA apprMarksIndDA = new CostCentreTabDA();
            this.ddlCostCentre.DataSource = apprMarksIndDA.GetAllCostCentreTabInfo();
            this.ddlCostCentre.DataTextField = "CostCenter";
            this.ddlCostCentre.DataValueField = "CostCenterId";
            this.ddlCostCentre.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCostCentre.Items.Insert(0, item);            
        }
        public void FillForm(int editId)
        {
            HMPaidServiceBO paidServiceBO = new HMPaidServiceBO();
            HMPaidServiceDA paidServiceDA = new HMPaidServiceDA();

            paidServiceBO = paidServiceDA.GetPaidServiceInfoById(editId);
            ddlCostCentre.SelectedValue = paidServiceBO.CostCenterId.ToString();
            txtServiceName.Text = paidServiceBO.ServiceName.ToString();
            txtDescription.Text = paidServiceBO.Description.ToString();
            ddlServiceType.SelectedValue = paidServiceBO.ServiceType.ToString();
            txtSellingPriceLocal.Text = paidServiceBO.UnitPriceLocal.ToString();
            txtSellingPriceUsd.Text = paidServiceBO.UnitPriceUsd.ToString();
            //ddlActiveStat.SelectedIndex = paidServiceBO.ActiveStat == true ? 1 : 2;
            ddlActiveStat.SelectedValue = (paidServiceBO.ActiveStat == true ? 0 : 1).ToString();

            txtPaidServiceId.Value = paidServiceBO.PaidServiceId.ToString();
            btnSave.Text = "Update";
        }
        private bool IsFrmValid()
        {
            bool flag = true;

            if (txtServiceName.Text == "")
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please Provide Service Name";
                flag = false;
                txtServiceName.Focus();
            }
            else if (ddlServiceType.SelectedIndex == 0)
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please Provide Service Type";
                flag = false;
                ddlServiceType.Focus();
            }
            else if (ddlCostCentre.SelectedIndex == 0)
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please Provide Cost Center";
                flag = false;
                ddlCostCentre.Focus();
            }
            return flag;
        }
        private void Cancel()
        {
            this.btnSave.Text = "Save";
            this.txtServiceName.Text = string.Empty;
            this.txtDescription.Text = string.Empty;
            this.txtSellingPriceLocal.Text = string.Empty;
            this.txtSellingPriceUsd.Text = string.Empty;
            this.ddlCostCentre.SelectedIndex = 0;
            this.ddlServiceType.SelectedIndex = 0;
            this.ddlActiveStat.SelectedIndex = 0;
        }
        //************************ User Defined WebMethod ********************//
        [WebMethod]
        public static GridViewDataNPaging<HMPaidServiceBO, GridPaging> SearchPaidServiceAndLoadGridInformation(string serviceName, string serviceType, string activeStat, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<HMPaidServiceBO, GridPaging> myGridData = new GridViewDataNPaging<HMPaidServiceBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            HMPaidServiceDA paidServiceDA = new HMPaidServiceDA();
            List<HMPaidServiceBO> paidServiceList = new List<HMPaidServiceBO>();
            paidServiceList = paidServiceDA.GetPaidServiceInfoBySearchCriteriaForPagination(serviceName, serviceType, activeStat, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<HMPaidServiceBO> distinctItems = new List<HMPaidServiceBO>();
            distinctItems = paidServiceList.GroupBy(test => test.PaidServiceId).Select(group => group.First()).ToList();


            //myGridData.GridPagingProcessing(guestInfoList, totalRecords);
            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static string DeletePaidServiceById(int sEmpId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            try
            {
                HMPaidServiceDA paidServiceDA = new HMPaidServiceDA();
                Boolean status = paidServiceDA.DeletePaidServiceById(sEmpId);
                if (status)
                {
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.HMPaidService.ToString(), sEmpId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HMPaidService));
                    result = "success";
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = "Data Deleted Failed.";
                throw ex;
            }

            return result;
        }
    }
}