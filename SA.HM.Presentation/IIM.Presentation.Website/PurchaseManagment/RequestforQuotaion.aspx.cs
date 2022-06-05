using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
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

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class RequestforQuotaion : BasePage
    {

        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadRequisitionOrder();
                LoadCategory();
                LoadAllCostCentreInfo();

                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomProductId.Value = seatingId.ToString();
                CheckPermission();

                FileUpload();
            }
        }

        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }


        private void LoadRequisitionOrder()
        {
            PMRequisitionDA entityDA = new PMRequisitionDA();
            List<PMRequisitionBO> requisitionBOList = entityDA.GetApprovedNNotDeliveredRequisitionInfo();

            ddlRequisitionNumber.DataSource = requisitionBOList;
            ddlRequisitionNumber.DataTextField = "PRNumber";
            ddlRequisitionNumber.DataValueField = "RequisitionId";
            ddlRequisitionNumber.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "---Please Select---";

            ddlRequisitionNumber.Items.Insert(0, item);

        }

        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetAllInvItemCatagoryInfoByServiceType("Product");
            ddlCategory.DataSource = List;
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = "---All---";
            ddlCategory.Items.Insert(0, item);
        }

        

        [WebMethod]
        public static List<DocumentsBO> LoadContactDocument(long id, int randomId, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }

            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("RequestForQuotationDoc", randomId);

            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("RequestForQuotationDoc", id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));

            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }

        [WebMethod]
        public static List<DocumentsBO> LoadContactDocumentForSupplier(long id, int randomId, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }

            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("RequestForQuotationNSupplierDoc", randomId);

            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("RequestForQuotationNSupplierDoc", id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));

            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }

        [WebMethod]
        public static ReturnInfo SaveOrUpdateRFQ(RFQuotationBO QuotationInfo, List<RFQuotationSupplierBO> SupplierList, string hfRandom, string deletedDocument, string deletedDocumentForSupplier)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            int OutId = 0;
            int itemId = 0;
            bool mailStatus = false;
            long OwnerIdForDocuments = 0;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            PMRFQuotationDA rfqDA = new PMRFQuotationDA();
            try
            {
                QuotationInfo.CreatedBy = userInformationBO.UserInfoId;
                rtninfo.IsSuccess = rfqDA.SaveOrUpdateRFQ(QuotationInfo, SupplierList, out OutId, out itemId);

                if (rtninfo.IsSuccess)
                {
                    
                    int i = 0;
                    for(i =0; i< SupplierList.Count; i++)
                    {
                        int sId = SupplierList[i].SupplierId == null ? 0 :  Convert.ToInt32(SupplierList[i].SupplierId);
                        if(sId > 0)
                        {
                            mailStatus = SendMail(sId);
                        }
                        
                        
                    }


                    if (QuotationInfo.RFQId == 0)
                    {
                        OwnerIdForDocuments = OutId;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        //Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                        //        EntityTypeEnum.EntityType.LostNFound.ToString(), OutId,
                        //    ProjectModuleEnum.ProjectModule.HouseKeeping.ToString(),
                        //    hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LostNFound));
                    }
                    else
                    {
                        OwnerIdForDocuments = QuotationInfo.RFQId;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                        //Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                        //        EntityTypeEnum.EntityType.LostNFound.ToString(), lostFoundBO.Id,
                        //    ProjectModuleEnum.ProjectModule.HouseKeeping.ToString(),
                        //    hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LostNFound));
                    }
                    DocumentsDA documentsDA = new DocumentsDA();
                    if (!string.IsNullOrEmpty(deletedDocument))
                    {
                        bool delete = documentsDA.DeleteDocument(deletedDocument);
                    }
                    if (!string.IsNullOrEmpty(deletedDocumentForSupplier))
                    {
                        bool delete = documentsDA.DeleteDocument(deletedDocumentForSupplier);
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

        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "LoadDetailGridInformation", jscript, true);

            flashUpload.QueryParameters = "RequestForQuotationDocId=" + Server.UrlEncode(RandomProductId.Value);


            flashUpload2.QueryParameters = "RequestForQuotationNSupplierDocId=" + Server.UrlEncode(RandomProductId.Value);

            //flashUpload.QueryParameters = "InventoryProductId=" + Server.UrlEncode(RandomProductId.Value) + "~" + txtCode.Text;
        }


        private void LoadAllCostCentreInfo()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            costCentreTabBOList = costCentreTabDA.GetCostCentreTabInfoByUserGroupId(userInformationBO.UserGroupId);

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            costCentreTabBOList = costCentreTabBOList.Where(o => o.OutletType == 2 && o.CostCenterType == "Inventory").ToList();

            ddlCostCentre.DataSource = costCentreTabBOList;
            ddlCostCentre.DataTextField = "CostCenter";
            ddlCostCentre.DataValueField = "CostCenterId";
            ddlCostCentre.DataBind();


            if (costCentreTabBOList.Count > 1)
                ddlCostCentre.Items.Insert(0, item);


        }

        private static bool SendMail(int supplierId)
        {
            HMUtility hmUtility = new HMUtility();
            Email email;
            Email email1;
            Email email2;

            bool status = false;
            bool status1 = false;
            bool status2 = false;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            //var accepted = "";
            //var rejected = "";
            //var deferred = "";

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
            string mainString = commonSetupBO.SetupValue;

            string urlHead1 = System.Web.Configuration.WebConfigurationManager.AppSettings["ClientURL"].ToString();

            UserInformationDA userInformationDA = new UserInformationDA();
            UserInformationBO userInformation = userInformationDA.GetUserInformationBySupplierId(supplierId);

            urlHead1 += "EmailTemplates/EmailLandingPage.aspx?t=" + "s" + "&u=" + userInformation.UserId + "&p=" + userInformation.UserPassword;

            PMSupplierBO supplierBO = new PMSupplierBO();
            PMSupplierDA supplierDA = new PMSupplierDA();
            supplierBO = supplierDA.GetPMSupplierInfoById(supplierId);

            if (!string.IsNullOrEmpty(mainString))
            {
                string[] dataArray = mainString.Split('~');
                email = new Email
                {
                    From = dataArray[0],
                    Password = dataArray[1],
                    To = supplierBO.Email,
                    Host = dataArray[2],
                    Port = dataArray[3],
                    Subject = "Request For Quotation",
                    TempleteName = HMConstants.EmailTemplates.RequestForQuotationTemplete
                };
                
                var token = new Dictionary<string, string>
                {
                    {"SupplierName", supplierBO.Name},
                    {"COMPANY", hmUtility.GetHMCompanyProfile()},
                    {"COMPANYADDRESS", hmUtility.GetHMCompanyAddress()},
                    {"LINK", urlHead1},
                };
                
                //var token3 = new Dictionary<string, string>
                //{
                //    {"NAME", onlineMember.FullName},
                //    {"MEMBERTYPE", onlineMember.TypeName},
                //    {"REJECTED", deferred},
                //    {"REMARKS", onlineMember.Remarks},
                //    {"COMPANY", hmUtility.GetHMCompanyProfile()},
                //    {"COMPANYADDRESS", hmUtility.GetHMCompanyAddress()},
                //};
                try
                {
                    status = EmailHelper.SendEmail(email, token);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return status;
        }

        [WebMethod]
        public static List<InvItemAutoSearchBO> ItemSearch(string searchTerm, int costCenterId, int categoryId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemDetailsForAutoSearchWithOutSupplier(searchTerm, costCenterId, ConstantHelper.CustomerSupplierAutoSearch.SupplierItem.ToString(), categoryId);

            return itemInfo;
        }

        [WebMethod]
        public static RequisitionViewBO GetRequisitionByRequsionId(int requisitionId)
        {
            PMRequisitionDA requisitionDA = new PMRequisitionDA();
            RequisitionViewBO viewBo = new RequisitionViewBO();

            viewBo.Requisition = requisitionDA.GetPMRequisitionInfoByID(requisitionId);
            viewBo.RequisitionDetails = requisitionDA.GetPMRequisitionDetailsByID(requisitionId);

            return viewBo;
        }

        [WebMethod]
        public static List<PMSupplierBO> GetPMSupplierInfoUsingItemList(string itemList )
        {

            PMSupplierDA costCentreTabDA = new PMSupplierDA();
            List<PMSupplierBO> files = costCentreTabDA.GetPMSupplierInfoUsingItemList(itemList );


            return files;
        }
    }







}