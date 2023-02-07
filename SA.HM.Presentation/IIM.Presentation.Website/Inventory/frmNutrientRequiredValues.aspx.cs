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
    public partial class frmNutrientRequiredValues : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadItemName();
                LoadNutrientInfo();
            }
        }
        private void LoadItemName()
        {
            InvItemDA invItemDa = new InvItemDA();
            List<InvItemBO> invItemBo = new List<InvItemBO>();
            invItemBo = invItemDa.GetInvFinishedItemInformation();

            ddlItemName.DataSource = invItemBo;
            ddlItemName.DataTextField = "CodeAndName";
            ddlItemName.DataValueField = "ItemId";
            ddlItemName.DataBind();

            ddlItemNameSearch.DataSource = invItemBo;
            ddlItemNameSearch.DataTextField = "CodeAndName";
            ddlItemNameSearch.DataValueField = "ItemId";
            ddlItemNameSearch.DataBind();

            ListItem itemSearch = new ListItem();
            itemSearch.Value = "0";
            itemSearch.Text = hmUtility.GetDropDownFirstValue();
            ddlItemName.Items.Insert(0, itemSearch);
            ddlItemNameSearch.Items.Insert(0, itemSearch);
        }
        
        private void LoadNutrientInfo()
        {
            InvNutrientInfoDA invNutrientDa = new InvNutrientInfoDA();
            List<InvNutrientInfoBO> invNutrientBo = new List<InvNutrientInfoBO>();
            invNutrientBo = invNutrientDa.GetNutrientInformations();

            ddlNutrient.DataSource = invNutrientBo;
            ddlNutrient.DataTextField = "Name";
            ddlNutrient.DataValueField = "NutrientId";
            ddlNutrient.DataBind();

            ListItem itemSearch = new ListItem();
            itemSearch.Value = "0";
            itemSearch.Text = hmUtility.GetDropDownFirstValue();
            ddlNutrient.Items.Insert(0, itemSearch);
        }
        
        [WebMethod]
        public static ReturnInfo SaveNutrientRequiredValues(InvNutrientInfoBO NutrientRequiredMasterInfo, List<InvNutrientInfoBO> AddedNutrientRequiredValueInfo, List<int> deletedNutrientRequiredValueList)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                InvNutrientInfoDA nutrientInfoDa = new InvNutrientInfoDA();
                status = nutrientInfoDa.SaveNutrientRequiredValues(NutrientRequiredMasterInfo, AddedNutrientRequiredValueInfo, deletedNutrientRequiredValueList, userInformationBO.UserInfoId);
                if (status)
                {
                    rtninfo.IsSuccess = true;
                    if(NutrientRequiredMasterInfo.Id > 0)
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
        public static GridViewDataNPaging<InvNutrientInfoBO, GridPaging> SearchNutrientRequiredValues(int itemId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage
                                                                                         )
        {
            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<InvNutrientInfoBO, GridPaging> myGridData = new GridViewDataNPaging<InvNutrientInfoBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            InvNutrientInfoDA nutrientInfoDa = new InvNutrientInfoDA();
            List<InvNutrientInfoBO> invNutrientList = new List<InvNutrientInfoBO>();

            invNutrientList = nutrientInfoDa.GetNutrientRequiredValuesForSearch(itemId, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(invNutrientList, totalRecords);
            return myGridData;
        }

        [WebMethod]
        public static NRVViewBO NutrientRequiredValuesEdit(long Id)
        {
            NRVViewBO viewBo = new NRVViewBO();
            InvNutrientInfoDA nutrientDa = new InvNutrientInfoDA();

            viewBo.NRVMasterInfo = nutrientDa.GetNRVMasterInfo(Id);
            viewBo.NRVDetails = nutrientDa.GetNRVDetailsById(Id);
            return viewBo;
        }

        [WebMethod]
        public static ReturnInfo NutrientRequiredValuesDelete(long Id)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            try
            {

                InvNutrientInfoDA nutrientInfoDa = new InvNutrientInfoDA();
                status = nutrientInfoDa.NutrientRequiredValuesDelete(Id);
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