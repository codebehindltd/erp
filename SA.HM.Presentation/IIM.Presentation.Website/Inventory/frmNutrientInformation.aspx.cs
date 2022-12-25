using HotelManagement.Data.Inventory;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Inventory
{
    public partial class frmNutrientInformation : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadNutritionType();
            }
        }

        private void LoadNutritionType()
        {
            InvNutrientInfoDA nutritionInfoDa = new InvNutrientInfoDA();
            List<InvNutrientInfoBO> nutritionInfoBo = new List<InvNutrientInfoBO>();
            nutritionInfoBo = nutritionInfoDa.GetNutritionType();

            ddlNutritionType.DataSource = nutritionInfoBo;
            ddlNutritionType.DataTextField = "Name";
            ddlNutritionType.DataValueField = "NutritionTypeId";
            ddlNutritionType.DataBind();

            ddlNutritionTypeSearch.DataSource = nutritionInfoBo;
            ddlNutritionTypeSearch.DataTextField = "Name";
            ddlNutritionTypeSearch.DataValueField = "NutritionTypeId";
            ddlNutritionTypeSearch.DataBind();

            ListItem itemSearch = new ListItem();
            itemSearch.Value = "0";
            itemSearch.Text = hmUtility.GetDropDownFirstValue();
            ddlNutritionType.Items.Insert(0, itemSearch);
            ddlNutritionTypeSearch.Items.Insert(0, itemSearch);
        }

        [WebMethod]
        public static ReturnInfo SaveNutritionTyepInfo(InvNutrientInfoBO NutritionTypeInfo)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                InvNutrientInfoDA niDa = new InvNutrientInfoDA();

                NutritionTypeInfo.CreatedBy = userInformationBO.UserInfoId;

                Boolean IsUpdate = false;
                if (NutritionTypeInfo.IsEdit)
                {
                    IsUpdate = true;
                }

                status = niDa.SaveNutritionTyepInfo(NutritionTypeInfo);
                if (status)
                {
                    rtninfo.IsSuccess = true;
                    if (IsUpdate)
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                    else
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                }

                if (!status)
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;

        }

        [WebMethod]
        public static ReturnInfo SaveNutrientInfo(InvNutrientInfoBO nutrientInfo)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                InvNutrientInfoDA niDa = new InvNutrientInfoDA();

                nutrientInfo.CreatedBy = userInformationBO.UserInfoId;

                Boolean IsUpdate = false;
                if (nutrientInfo.IsEdit)
                {
                    IsUpdate = true;
                }

                status = niDa.SaveNutrientInfo(nutrientInfo);
                if (status)
                {
                    rtninfo.IsSuccess = true;
                    if (IsUpdate)
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                    else
                    {
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                }

                if (!status)
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;

        }

        
        [WebMethod]
        public static GridViewDataNPaging<InvNutrientInfoBO, GridPaging> SearchNutrientInformation(InvNutrientInfoBO nutrientInfo, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage
                                                                                         )
        {
            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<InvNutrientInfoBO, GridPaging> myGridData = new GridViewDataNPaging<InvNutrientInfoBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            InvNutrientInfoDA inDA = new InvNutrientInfoDA();
            List<InvNutrientInfoBO> nutrientList = new List<InvNutrientInfoBO>();

            nutrientList = inDA.GetNutrientInformationForSearch(nutrientInfo, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(nutrientList, totalRecords);
            return myGridData;
        }

        
        [WebMethod]
        public static InvNutrientInfoBO NutrientInfoEdit(long NutrientId, long NutritionTypeId)
        {
            InvNutrientInfoBO viewBo = new InvNutrientInfoBO();
            InvNutrientInfoDA inDA = new InvNutrientInfoDA();
            viewBo = inDA.GetNutrientInfoById(NutrientId, NutritionTypeId);
            return viewBo;
        }

        [WebMethod]
        public static ReturnInfo NutrientInfoDelete(long NutrientId, long NutritionTypeId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            try
            {

                InvNutrientInfoDA inDa = new InvNutrientInfoDA();
                status = inDa.NutrientInfoDelete(NutrientId, NutritionTypeId);
                if (status)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }

                if (!status)
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;

        }
    }
}