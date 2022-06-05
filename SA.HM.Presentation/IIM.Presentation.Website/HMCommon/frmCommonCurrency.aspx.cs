using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class frmCommonCurrency : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (Session["StatusUpdate"] != null)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                Session["StatusUpdate"] = null;
            }
            //Session["StatusUpdate"] = null;
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
            }

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFrmValid())
            {
                return;
            }

            CommonCurrencyBO headBO = new CommonCurrencyBO();
            CommonCurrencyDA headDA = new CommonCurrencyDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            headBO.CurrencyName = this.txtHeadName.Text;
            headBO.ActivaStat = ddlActiveStat.SelectedIndex == 0 ? true : false;
            if (string.IsNullOrWhiteSpace(txtConHeadId.Value))
            {
                int tmpConversionId = 0;
                headBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = headDA.SaveHeadInfo(headBO, out tmpConversionId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.ConversionHead.ToString(), tmpConversionId,
               ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ConversionHead));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    this.Cancel();
                }
            }
            else
            {
                {
                    headBO.CurrencyId = Convert.ToInt32(txtConHeadId.Value);
                    headBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = headDA.UpdateHeadInfo(headBO);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.ConversionHead.ToString(), headBO.CurrencyId,
               ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ConversionHead));
                        this.Cancel();
                        var nvc = HttpUtility.ParseQueryString(Request.Url.Query);
                        nvc.Remove("editId");
                        string url = Request.Url.AbsolutePath + "?Status=Update" + nvc.ToString();
                        Session["StatusUpdate"] = url;
                        Response.Redirect(url);

                        //this.isMessageBoxEnable = 2;
                        //lblMessage.Text = "Update Operation Successfull";
                    }
                }
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.txtHeadName.Text = string.Empty;
            this.ddlActiveStat.SelectedIndex = 0;
            this.txtConHeadId.Value = string.Empty;
            btnSave.Text = "Save";
        }
        //************************ User Defined Function ********************//
        public void FillForm(int EditId)
        {
            CommonCurrencyBO headBO = new CommonCurrencyBO();
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            headBO = headDA.GetCommonCurrencyInfoById(EditId);

            ddlActiveStat.SelectedValue = (headBO.ActivaStat == true ? 0 : 1).ToString();
            txtConHeadId.Value = headBO.CurrencyId.ToString();
            txtHeadName.Text = headBO.CurrencyName.ToString();

            this.btnSave.Text = "Update";
        }
        private bool IsFrmValid()
        {
            bool flag = true;

            if (txtHeadName.Text == "")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Currency Name.", AlertType.Warning);
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
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static GridViewDataNPaging<CommonCurrencyBO, GridPaging> SearchHeadAndLoadGridInformation(string headName, Boolean activeStat, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<CommonCurrencyBO, GridPaging> myGridData = new GridViewDataNPaging<CommonCurrencyBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> headList = new List<CommonCurrencyBO>();
            headList = headDA.GetHeadInformationBySearchCriteriaForPaging(headName, activeStat, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<CommonCurrencyBO> distinctItems = new List<CommonCurrencyBO>();
            distinctItems = headList.GroupBy(test => test.CurrencyId).Select(group => group.First()).ToList();


            //myGridData.GridPagingProcessing(guestInfoList, totalRecords);
            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo DeleteHeadById(int sEmpId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("CommonConversionHead", "ConversionHeadId", sEmpId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.ConversionHead.ToString(), sEmpId,
               ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ConversionHead));
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rtninf;
        }
    }
}