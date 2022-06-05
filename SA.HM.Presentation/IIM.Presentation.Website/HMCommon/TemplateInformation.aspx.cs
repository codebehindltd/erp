using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class TemplateInformation : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTemplateType();
                LoadTemplateFor();
                CheckPermission();
            }
        }
        private void CheckPermission()
        {
            //hfViewPermission.Value = isViewPermission ? "1" : "0";
            //hfSavePermission.Value = isSavePermission ? "1" : "0";
            //hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            //hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }
        private void LoadTemplateType()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("TemplateType");

            ddlType.DataSource = fields;
            ddlType.DataTextField = "FieldValue";
            ddlType.DataValueField = "FieldValue";
            ddlType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlType.Items.Insert(0, item);
        }
        private void LoadTemplateFor()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("TemplateFor");

            ddlTemplateFor.DataSource = fields;
            ddlTemplateFor.DataTextField = "FieldValue";
            ddlTemplateFor.DataValueField = "Description";
            ddlTemplateFor.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlTemplateFor.Items.Insert(0, item);
        }
        [WebMethod]
        public static GridViewDataNPaging<TemplateInformationBO, GridPaging> LoadSearch(string name, string typeId, string templateForId, string subject, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<TemplateInformationBO, GridPaging> myGridData = new GridViewDataNPaging<TemplateInformationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<TemplateInformationBO> templateInformation = new List<TemplateInformationBO>();
            TemplateInfoDA DA = new TemplateInfoDA();
            templateInformation = DA.GetTemplateInformationWithGrid(name, typeId, templateForId, subject, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(templateInformation, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo DeleteData(long Id)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            TemplateInfoDA DA = new TemplateInfoDA();
            status = DA.DeleteData(Id);
            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                               EntityTypeEnum.EntityType.TemplateInformation.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.HMCommon.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.TemplateInformation));
            }
            return rtninf;
        }
    }
}