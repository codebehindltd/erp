using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.SupportAndTicket;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.SupportAndTicket;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SupportAndTicket
{
    public partial class SupportPriceMatrixSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadCategory();
            }
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            //List = da.GetAllInvItemCatagoryInfoByServiceType("Product");
            List = da.GetAllActiveInvItemCatagoryInfoByServiceType("All");
            ddlCategory.DataSource = List;
            ddlCategory.DataTextField = "MatrixInfo";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "---All---";
            ddlCategory.Items.Insert(0, item);
        }
        [WebMethod]
        public static GridViewDataNPaging<STSupportPriceMatrixSetupBO, GridPaging> SearchStage(string company, string item, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<STSupportPriceMatrixSetupBO, GridPaging> myGridData = new GridViewDataNPaging<STSupportPriceMatrixSetupBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<STSupportPriceMatrixSetupBO> stageinfo = new List<STSupportPriceMatrixSetupBO>();
            SupportNCaseSetupDA DA = new SupportNCaseSetupDA();
            stageinfo = DA.GetPriceMatrixSetupBySearchCriteria(company, item, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(stageinfo, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static List<InvItemAutoSearchBO> ItemSearch(string searchTerm,  int categoryId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemByCategoryAutoSearch(searchTerm, categoryId, true);

            return itemInfo;
        }
        [WebMethod]
        public static List<GuestCompanyBO> CompanySearch(string searchTerm)
        {
            List<GuestCompanyBO> companyInfo = new List<GuestCompanyBO>();
            GuestCompanyDA itemDa = new GuestCompanyDA();
            companyInfo = itemDa.GetGuestCompanyInfo(searchTerm);

            return companyInfo;
        }
        [WebMethod]
        public static ReturnInfo SaveOrUpdatePriceMatrix(List<STSupportPriceMatrixSetupBO> SupportPriceMatrixLIst)
        {
            ReturnInfo info = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            SupportNCaseSetupDA DA = new SupportNCaseSetupDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                        
            int id = 0;

            try
            {

                info.IsSuccess = DA.SaveOrUpdateSupportPriceMatrixSetupInfo(SupportPriceMatrixLIst);

                if (info.IsSuccess)
                {
                    if (SupportPriceMatrixLIst[0].Id == 0)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.SMDealStage.ToString(), id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        info.Data = 0;
                    }
                    else
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.SMDealStage.ToString(), SupportPriceMatrixLIst[0].Id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }
                else
                {
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;
        }
        [WebMethod]
        public static STSupportPriceMatrixSetupBO GetStageById(int id)
        {
            STSupportPriceMatrixSetupBO PriceMatrix = new STSupportPriceMatrixSetupBO();
            SupportNCaseSetupDA DA = new SupportNCaseSetupDA();

            PriceMatrix = DA.GetPriceMatrixSetupById(id);

            return PriceMatrix;
        }

        [WebMethod]
        public static ReturnInfo PerformDelete(long id)
        {
            ReturnInfo info = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();

            try
            {
                status = hmCommonDA.DeleteInfoById("STSupportPriceMatrixSetup", "Id", id);

                if (status)
                {
                    info.IsSuccess = true;
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.SMDealStage.ToString(), id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    info.IsSuccess = false;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                info.IsSuccess = false;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;
        }
        [WebMethod]
        public static int DuplicateCheckDynamicaly(string fieldName1, string fieldValue1, string fieldName2, string fieldValue2, int isUpdate, string id)
        {
            string tableName = "STSupportPriceMatrixSetup";
            string pkFieldName = "Id";
            string pkFieldValue = id;
            int IsDuplicate = 0;
            if (!string.IsNullOrWhiteSpace(pkFieldValue))
            {
                isUpdate = 1;
            }
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName1, fieldValue1, fieldName2, fieldValue2, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }


    }
}