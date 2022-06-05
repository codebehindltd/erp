using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.Maintenance;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.Maintenance;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Maintenance
{
    public partial class frmGatePass : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();

        HMCommonDA hmCommonDA = new HMCommonDA();

        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
            if (!string.IsNullOrWhiteSpace(DeleteSuccess))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
            }

            if (!IsPostBack)
            {
                LoadCommonDropDownHiddenField();
                LoadStockBy();
                LoadAllCostCentreTabInfo();
                LoadSupplierInfo();
                //this.CheckObjectPermission();
            }
        }

        protected void gvGatePassInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }
        protected void gvGatePassInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            
        }
        protected void gvGatePassInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
        }

        //************************ User Defined Function ********************//
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
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmPMRequisition.ToString());
            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }

        // need to code
        private void LoadGridview()
        {
            this.SetTab("SearchTab");
            GatePassDA gatePassDA = new GatePassDA();
            List<GatePassBO> gatePassList = new List<GatePassBO>();
            this.CheckObjectPermission();

            string startDate = string.Empty;
            string endDate = string.Empty;
            DateTime dateTime = DateTime.Now;

            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = this.txtToDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);


            gvGatePassInfo.DataSource = gatePassList;
            gvGatePassInfo.DataBind();
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadAllCostCentreTabInfo()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            var List = costCentreTabDA.GetCostCentreTabInfoByUserGroupId(userInformationBO.UserGroupId)
                                    .Where(o => o.CostCenterType == "Inventory").ToList();

            this.ddlCostCentre.DataSource = List;
            this.ddlCostCentre.DataTextField = "CostCenter";
            this.ddlCostCentre.DataValueField = "CostCenterId";
            this.ddlCostCentre.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            if (List.Count > 1)
            {
                this.ddlCostCentre.Items.Insert(0, item);
            }
        }
        private void LoadStockBy()
        {
            List<InvUnitHeadBO> headListBO = new List<InvUnitHeadBO>();
            InvUnitHeadDA da = new InvUnitHeadDA();
            headListBO = da.GetInvUnitHeadInfo();

            this.ddlStockBy.DataSource = headListBO;
            this.ddlStockBy.DataTextField = "HeadName";
            this.ddlStockBy.DataValueField = "UnitHeadId";
            this.ddlStockBy.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlStockBy.Items.Insert(0, item);
        }
        
        private void LoadSupplierInfo()
        {
            PMSupplierDA entityDA = new PMSupplierDA();
            List<PMSupplierBO> supplierBOList = new List<PMSupplierBO>();

            supplierBOList = entityDA.GetPMSupplierInfo();

            ddlSupplier.DataSource = supplierBOList;
            ddlSupplier.DataTextField = "Name";
            ddlSupplier.DataValueField = "SupplierId";
            ddlSupplier.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSupplier.Items.Insert(0, item);
        }

        //************************ Web Method Function ********************//
        [WebMethod]
        public static GatePassViewBO FillForm(int gatePassId)
        {
            //todo
            GatePassDA gatePassDA = new GatePassDA();
            GatePassViewBO viewBo = new GatePassViewBO();

            viewBo.GatePass = gatePassDA.GetGatePassInfoByID(gatePassId);
            viewBo.GatePassDetails = gatePassDA.GetGatePassDetailsByID(gatePassId);

            return viewBo;
        }
        [WebMethod]
        public static ReturnInfo SaveGatePass(int gatePassId, GatePassBO gatepass, List<GatePassItemBO> AddedItem, List<GatePassItemBO> EditedItem, List<GatePassItemBO> DeletedItem)
        {
            ReturnInfo rtninf = new ReturnInfo();
            long gatePassNewId = 0;
            string gatePassNumber = string.Empty;
            bool status = false;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                GatePassDA gatePassDA = new GatePassDA();

                if (gatePassId == 0)
                {
                    gatepass.CreatedBy = userInformationBO.UserInfoId;
                    status = gatePassDA.SaveGatePassInfo(gatepass, AddedItem, out gatePassNewId, out gatePassNumber);

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.GatePass.ToString(), gatePassNewId,
                                ProjectModuleEnum.ProjectModule.Maintenance.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GatePass));
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                }
                else
                {
                    gatepass.LastModifiedBy = userInformationBO.UserInfoId;
                    status = gatePassDA.UpdateGatePassInfo(gatepass, AddedItem, EditedItem, DeletedItem);

                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.GatePass.ToString(), gatePassId,
                                ProjectModuleEnum.ProjectModule.Maintenance.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GatePass));
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }

                //HMCommonDA commonDa = new HMCommonDA();
                //CustomFieldBO customFieldObject = new CustomFieldBO();
                //customFieldObject = commonDa.GetCustomFieldByFieldName("ItemRequisitionApprovedByEmail");

                //if (customFieldObject != null)
                //{
                //    var req = gatePassDA.GetPMRequisitionInfoByID(gatepass.RequisitionId);
                //    EmailHelper.SendEmail(string.Empty, customFieldObject.FieldValue.ToString(), "Approval Pending For Requisition No " + req.PRNumber,
                //        "Please Approved The Requisition.", string.Empty);
                //}

                if (!status)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }
        [WebMethod]
        public static List<GatePassItemBO> GatePassDetails(int gatePassId)
        {
            GatePassDA gatePassDA = new GatePassDA();
            List<GatePassItemBO> gatePassItems = new List<GatePassItemBO>();
            gatePassItems = gatePassDA.GetGatePassDetailsByID(gatePassId);

            return gatePassItems;
        }
        [WebMethod]
        public static List<InvItemAutoSearchBO> ItemSearch(string searchTerm, int costCenterId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();
            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemNameWiseItemDetailsForAutoSearch(searchTerm, costCenterId, "SupplierItem");

            return itemInfo;
        }
        [WebMethod]
        public static List<InvUnitHeadBO> LoadRelatedStockBy(int stockById)
        {
            InvUnitHeadDA unitHeadDA = new InvUnitHeadDA();
            List<InvUnitHeadBO> unitHeadList = new List<InvUnitHeadBO>();
            unitHeadList = unitHeadDA.GetRelatedStockBy(stockById);

            return unitHeadList;
        }
    }
}