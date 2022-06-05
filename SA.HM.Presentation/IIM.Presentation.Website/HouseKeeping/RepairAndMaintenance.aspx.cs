using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HouseKeeping;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HouseKeeping;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HouseKeeping
{
    public partial class RepairAndMaintenance : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FileUpload();
                LoadProduct();
                RequestedBy();
            }

        }
        private void RequestedBy()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollIntegrateWithInventory", "IsPayrollIntegrateWithInventory");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                if (setUpBO.SetupValue == "0")
                {
                    hfRequestedBy.Value = "0";
                    txtRequestedBy.Visible = true;
                    ddlRequestedBy.Visible = false;
                    txtRequestedBySrc.Visible = true;
                    ddlRequestedBySrc.Visible = false;
                }
                else
                {
                    hfRequestedBy.Value = "1";
                    txtRequestedBy.Visible = false;
                    ddlRequestedBy.Visible = true;
                    txtRequestedBySrc.Visible = false;
                    ddlRequestedBySrc.Visible = true;

                    EmployeeDA entityDA = new EmployeeDA();
                    List<EmployeeBO> EmpList = new List<EmployeeBO>();
                    EmpList = entityDA.GetEmployeeInfo();
                    this.ddlRequestedBy.DataSource = EmpList;
                    this.ddlRequestedBy.DataTextField = "DisplayName";
                    this.ddlRequestedBy.DataValueField = "EmpId";
                    this.ddlRequestedBy.DataBind();

                    this.ddlRequestedBySrc.DataSource = EmpList;
                    this.ddlRequestedBySrc.DataTextField = "DisplayName";
                    this.ddlRequestedBySrc.DataValueField = "EmpId";
                    this.ddlRequestedBySrc.DataBind();

                    ListItem itemEmpId = new ListItem();
                    itemEmpId.Value = "0";
                    itemEmpId.Text = hmUtility.GetDropDownFirstValue();
                    ListItem itemEmpIdSrc = new ListItem();
                    itemEmpIdSrc.Value = "0";
                    itemEmpIdSrc.Text = hmUtility.GetDropDownFirstAllValue();
                    this.ddlRequestedBy.Items.Insert(0, itemEmpId);
                    this.ddlRequestedBySrc.Items.Insert(0, itemEmpIdSrc);

                }
            }
        }
        private void LoadProduct()
        {
            List<InvItemBO> items = new List<InvItemBO>();
            InvItemDA productDA = new InvItemDA();
            items = productDA.GetInvItemInfo();
            items = items.Where(p => p.ServiceType == "FixedAsset").ToList();
            this.ddlFixedItem.DataSource = items;
            this.ddlFixedItem.DataTextField = "Name";
            this.ddlFixedItem.DataValueField = "ItemId";
            this.ddlFixedItem.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlFixedItem.Items.Insert(0, item);

            this.ddlFixedItemSrc.DataSource = items;
            this.ddlFixedItemSrc.DataTextField = "Name";
            this.ddlFixedItemSrc.DataValueField = "ItemId";
            this.ddlFixedItemSrc.DataBind();
            ListItem itemSrc = new ListItem();
            itemSrc.Value = "0";
            itemSrc.Text = hmUtility.GetDropDownFirstAllValue();
            ddlFixedItemSrc.Items.Insert(0, itemSrc);

        }

        private void FileUpload()
        {
            Random rd = new Random();
            int seatingId = rd.Next(100000, 999999);
            RandomDocId.Value = seatingId.ToString();
            tempDocId.Value = seatingId.ToString();
            HttpContext.Current.Session["RepairNMaintenanceId"] = RandomDocId.Value;
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            flashUpload.QueryParameters = "RepairNMaintenanceId=" + Server.UrlEncode(RandomDocId.Value);
        }

        [WebMethod]
        public static ReturnInfo SaveOrUpdateRepairNMaintenance(HotelRepairNMaintenanceBO HotelRepairNMaintenanceBO, int randomDocId, string deletedDoc)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            var OwnerIdForDocuments = 0;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (HotelRepairNMaintenanceBO.Id == 0)
            {
                HotelRepairNMaintenanceBO.CreatedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }
            else
            {
                HotelRepairNMaintenanceBO.LastModifiedBy = Convert.ToInt32(userInformationBO.UserInfoId);
            }

            int OutId;
            HotelRepairNMaintenanceDA DA = new HotelRepairNMaintenanceDA();
            status = DA.SaveOrUpdateRepairNMaintenance(HotelRepairNMaintenanceBO, out OutId);
            if (status)
                OwnerIdForDocuments = Convert.ToInt32(OutId);
            if (status)
            {
                DocumentsDA documentsDA = new DocumentsDA();
                if (!string.IsNullOrEmpty(deletedDoc))
                {
                    bool delete = documentsDA.DeleteDocument(deletedDoc);
                }
                Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(randomDocId));

                if (HotelRepairNMaintenanceBO.Id == 0)
                {

                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.HotelRepairNMaintenance.ToString(), OutId,
                            ProjectModuleEnum.ProjectModule.HouseKeeping.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HotelRepairNMaintenance));

                }
                else
                {

                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.HotelRepairNMaintenance.ToString(), OutId,
                           ProjectModuleEnum.ProjectModule.HouseKeeping.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HotelRepairNMaintenance));
                }


            }
            else
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }
            Random rd = new Random();
            int randomId = rd.Next(100000, 999999);
            rtninfo.Data = randomId;
            return rtninfo;
        }
        [WebMethod]
        public static GridViewDataNPaging<HotelRepairNMaintenanceBO, GridPaging> LoadRepairNMaintenance(DateTime fromDate, DateTime toDate, string maintenanceType, int itemId, string itemName,
                                                                            string maintenanceArea, string isEmergency,int transectionId, int requestedById, 
                                                                            string requestedByName,int gridRecordsCount, int pageNumber, int IsCurrentOrPreviousPage)
        {
            int totalRecords = 0;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<HotelRepairNMaintenanceBO, GridPaging> myGridData = new GridViewDataNPaging<HotelRepairNMaintenanceBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, IsCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            List<HotelRepairNMaintenanceBO> RepairNMaintenanceList = new List<HotelRepairNMaintenanceBO>();
            HotelRepairNMaintenanceDA DA = new HotelRepairNMaintenanceDA();

            RepairNMaintenanceList = DA.GetRepairNMaintenanceForSearchCriteria(fromDate,  toDate,  maintenanceType,  itemId,  itemName,maintenanceArea,  isEmergency,  transectionId,  requestedById,
                                                                             requestedByName, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(RepairNMaintenanceList, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static HotelRepairNMaintenanceBO FillForm(int id)
        {
            HotelRepairNMaintenanceBO RepairNMaintenance = new HotelRepairNMaintenanceBO();
            HotelRepairNMaintenanceDA DA = new HotelRepairNMaintenanceDA();
            RepairNMaintenance = DA.GetHotelRepairNMaintenanceById(id);

            return RepairNMaintenance;
        }
        [WebMethod]
        public static ReturnInfo DeleteAction(int Id)
        {
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            HMCommonDA DA = new HMCommonDA();
            status = DA.DeleteInfoById("HotelRepairNMaintenance", "Id", Id);
            if (status)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                               EntityTypeEnum.EntityType.HotelRepairNMaintenance.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.HouseKeeping.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.HotelRepairNMaintenance));
            }
            return rtninfo;
        }
        [WebMethod]
        public static int ChangeRandomId()
        {
            Random rd = new Random();
            int randomId = rd.Next(100000, 999999);
            HttpContext.Current.Session["RepairNMaintenanceId"] = randomId;
            return randomId;
        }
        [WebMethod]
        public static List<DocumentsBO> GetUploadedDocByWebMethod(int randomId, int id, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }
            List<DocumentsBO> docList = new List<DocumentsBO>();
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("RepairNMaintenanceDoc", randomId);
            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("RepairNMaintenanceDoc", id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));
            foreach (DocumentsBO dc in docList)
            {

                if (dc.DocumentType == "Image")
                    dc.Path = (dc.Path + dc.Name);

                dc.Name = dc.Name.Remove(dc.Name.LastIndexOf('.'));
            }
            docList = new HMCommonDA().GetDocumentListWithIcon(docList).ToList();
            return docList;
        }

    }
}