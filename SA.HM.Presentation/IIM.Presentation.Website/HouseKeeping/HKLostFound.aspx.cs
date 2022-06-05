using HotelManagement.Data;
using HotelManagement.Data.Banquet;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HouseKeeping;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity;
using HotelManagement.Entity.Banquet;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HouseKeeping;
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
    public partial class HKLostFound : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        bool IsPayrollIntegrateWithFrontOffice = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FileUpload();
                CheckObjectPermission();
                LoadCommonDropDownHiddenField();
                LoadIsPayrollIntegrateWithFrontOffice();
                if (IsPayrollIntegrateWithFrontOffice)
                {
                    LoadEmployee();
                    
                }
                
            }
        }

        //Load Methods
        private void CheckObjectPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadEmployee()
        {
            EmployeeDA employeeDA = new EmployeeDA();
            ddlFoundPerson.DataSource = employeeDA.GetEmployeeInfo();
            ddlFoundPerson.DataTextField = "EmployeeName";
            ddlFoundPerson.DataValueField = "EmpId";
            ddlFoundPerson.DataBind();

            ddlFoundPersonSrc.DataSource = employeeDA.GetEmployeeInfo();
            ddlFoundPersonSrc.DataTextField = "EmployeeName";
            ddlFoundPersonSrc.DataValueField = "EmpId";
            ddlFoundPersonSrc.DataBind();

            ListItem itemEmployee = new ListItem();
            itemEmployee.Value = "0";
            itemEmployee.Text = "--Please Select--";
            ddlFoundPerson.Items.Insert(0, itemEmployee);
            ddlFoundPersonSrc.Items.Insert(0, itemEmployee);

        }
        private void LoadIsPayrollIntegrateWithFrontOffice()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollIntegrateWithFrontOffice", "IsPayrollIntegrateWithFrontOffice");
            hfIsPayrollIntegrateWithFrontOffice.Value = "0";
            if (commonSetupBO != null)
            {
                if (commonSetupBO.SetupId > 0)
                {
                    if (commonSetupBO.SetupValue == "1")
                    {
                        hfIsPayrollIntegrateWithFrontOffice.Value = "1";
                        IsPayrollIntegrateWithFrontOffice = true;
                    }
                    else
                    {
                        hfIsPayrollIntegrateWithFrontOffice.Value = "0";
                        IsPayrollIntegrateWithFrontOffice = false;
                    }
                }
            }
        }

        // Client Call
        [WebMethod]
        public static List<RoomNumberBO> LoadRoom()
        {
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            List<RoomNumberBO> roomNumberBO = new List<RoomNumberBO>();
            roomNumberBO = roomNumberDA.GetRoomNumberInfo();
            return roomNumberBO;
        }
        [WebMethod]
        public static List<CostCentreTabBO> LoadRestaurant()
        {
            CostCentreTabDA DA = new CostCentreTabDA();
            List<CostCentreTabBO> BO = new List<CostCentreTabBO>();
            BO = DA.GetAllRestaurantTypeCostCentreTabInfo().Where( x => x.IsRestaurant == true).ToList();
            return BO;
        }
        [WebMethod]
        public static List<BanquetInformationBO> LoadBanquet()
        {
            BanquetInformationDA DA = new BanquetInformationDA();
            List<BanquetInformationBO> banquets= new List<BanquetInformationBO>();
            banquets = DA.GetAllBanquetInformation();
            return banquets;
        }

        [WebMethod]
        public static ReturnInfo SaveUpdate(LostFoundBO lostFoundBO, string hfRandom, string deletedDocument)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            int OutId = 0;
            long OwnerIdForDocuments = 0;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            LostFoundDA foundDA = new LostFoundDA();
            try
            {
                lostFoundBO.CreatedBy = userInformationBO.UserInfoId;
                rtninfo.IsSuccess = foundDA.SaveLostNFound(lostFoundBO, out OutId);

                if (rtninfo.IsSuccess)
                {
                    if (lostFoundBO.Id == 0)
                    {
                        OwnerIdForDocuments = OutId;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                                EntityTypeEnum.EntityType.LostNFound.ToString(), OutId,
                            ProjectModuleEnum.ProjectModule.HouseKeeping.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LostNFound));
                    }
                    else
                    {
                        OwnerIdForDocuments = lostFoundBO.Id;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                                EntityTypeEnum.EntityType.LostNFound.ToString(), lostFoundBO.Id,
                            ProjectModuleEnum.ProjectModule.HouseKeeping.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LostNFound));
                    }
                    DocumentsDA documentsDA = new DocumentsDA();
                    if (!string.IsNullOrEmpty(deletedDocument))
                    {
                        bool delete = documentsDA.DeleteDocument(deletedDocument);
                    }
                    Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(hfRandom));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return rtninfo;
        }
        [WebMethod]
        public static GridViewDataNPaging<LostFoundBO, GridPaging> SearchGridPaging(string itemNameSrc, string itemTypeSrc, string transectionTypeSrc, int transectionIdSrc, string foundDateSrc, int foundPersonId, string foundPersonName, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            LostFoundDA foundDA = new LostFoundDA();
            List<LostFoundBO> infoBOs = new List<LostFoundBO>();
            int totalRecords = 0;

            GridViewDataNPaging<LostFoundBO, GridPaging> myGridData = new GridViewDataNPaging<LostFoundBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            infoBOs = foundDA.GetLostFoundInfoGridding(itemNameSrc, itemTypeSrc, transectionTypeSrc, transectionIdSrc, foundDateSrc, foundPersonId, foundPersonName, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);
            myGridData.GridPagingProcessing(infoBOs, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static LostFoundBO FillForm(int Id)
        {
            LostFoundDA foundDA = new LostFoundDA();
            LostFoundBO infoBO = new LostFoundBO();
            infoBO = foundDA.GetLostFoundInfoById(Id);

            return infoBO;
        }
        [WebMethod]
        public static ReturnInfo DeleteData(long Id)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            HMCommonDA DA = new HMCommonDA();
            status = DA.DeleteInfoById("HotelLostNFound", "Id", Id);
            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                               EntityTypeEnum.EntityType.LostNFound.ToString(), Id,
                               ProjectModuleEnum.ProjectModule.HouseKeeping.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LostNFound));
            }
            return rtninf;
        }
        // Docuemnts
        private void FileUpload()
        {
            Random rd = new Random();
            int seatingId = rd.Next(100000, 999999);
            RandomDocId.Value = seatingId.ToString();
            tempDocId.Value = seatingId.ToString();
            HttpContext.Current.Session["LostItemDocId"] = RandomDocId.Value;
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            flashUpload.QueryParameters = "LostItemDocId=" + Server.UrlEncode(RandomDocId.Value);
        }
        
        [WebMethod]
        public static int ChangeRandomId()
        {
            Random rd = new Random();
            int randomId = rd.Next(100000, 999999);
            HttpContext.Current.Session["LostItemDocId"] = randomId;
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
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("LostItemDocuments", randomId);
            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("LostItemDocuments", (int)id));

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