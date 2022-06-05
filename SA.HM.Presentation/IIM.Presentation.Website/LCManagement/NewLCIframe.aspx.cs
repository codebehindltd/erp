using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.LCManagement;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.LCManagement;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.LCManagement
{
    public partial class NewLCIframe : BasePage
    {
        public NewLCIframe() : base("LCInformation")
        {

        }
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                IsLCNumberAutoGenerate();
                companyProjectUserControl.ddlFirstValueVar = "select";
                LoadSupplierInfoByOrderType();
                LoadCurrency();
                LoadAccountHead();
                CheckPermission();

                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomProductId.Value = seatingId.ToString();

                FileUpload();
            }
        }
        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }
        private void IsLCNumberAutoGenerate()
        {
            LCNumberLabel.Visible = true;
            LCNumberControl.Visible = true;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
            homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsLCNumberAutoGenerate", "IsLCNumberAutoGenerate");
            if (homePageSetupBO != null)
            {
                if (homePageSetupBO.SetupId > 0)
                {
                    hfIsLCNumberAutoGenerate.Value = homePageSetupBO.SetupValue;
                    if (homePageSetupBO.SetupValue == "1")
                    {
                        LCNumberLabel.Visible = false;
                        LCNumberControl.Visible = false;
                    }
                }
            }
        }
        private void LoadAccountHead()
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            NodeMatrixDA entityDA = new NodeMatrixDA();
            List <NodeMatrixBO> list = entityDA.GetNodeMatrixInfoByAncestorNodeId(16).Where(x => x.IsTransactionalHead == true).ToList();
            this.ddlLCManageAccount.DataSource = list;
            this.ddlLCManageAccount.DataTextField = "HeadWithCode";
            this.ddlLCManageAccount.DataValueField = "NodeId";
            this.ddlLCManageAccount.DataBind();

            this.ddlAccountHead.DataSource = list;
            this.ddlAccountHead.DataTextField = "HeadWithCode";
            this.ddlAccountHead.DataValueField = "NodeId";
            this.ddlAccountHead.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlLCManageAccount.Items.Insert(0, item);
            this.ddlAccountHead.Items.Insert(0, item);
        }
        public void LoadSupplierInfoByOrderType()
        {
            string supplierTypeId = "Foreign";
            Data.PurchaseManagment.PMSupplierDA entityDA = new Data.PurchaseManagment.PMSupplierDA();
            List<PMSupplierBO> supplierBOList = new List<PMSupplierBO>();

            supplierBOList = entityDA.GetPMSupplierInfoByOrderType(supplierTypeId);

            if (supplierBOList.Count == 1)
            {
                ddlSupplier.DataSource = supplierBOList;
                ddlSupplier.DataTextField = "Name";
                ddlSupplier.DataValueField = "SupplierId";
                ddlSupplier.DataBind(); ListItem itemSupplier = new ListItem();
                itemSupplier.Value = "0";
                itemSupplier.Text = hmUtility.GetDropDownFirstValue();
                ddlSupplier.Items.Insert(0, itemSupplier);

            }
            else if(supplierBOList.Count > 1)
            {
                ddlSupplier.DataSource = supplierBOList;
                ddlSupplier.DataTextField = "Name";
                ddlSupplier.DataValueField = "SupplierId";
                ddlSupplier.DataBind();
                ListItem itemSupplier = new ListItem();
                itemSupplier.Value = "0";
                itemSupplier.Text = hmUtility.GetDropDownFirstValue();
                ddlSupplier.Items.Insert(0, itemSupplier);
            }

        }
        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("All");

            ddlCurrency.DataSource = currencyListBO;
            ddlCurrency.DataTextField = "CurrencyName";
            ddlCurrency.DataValueField = "CurrencyId";
            ddlCurrency.DataBind();

            hfCurrencyAll.Value = JsonConvert.SerializeObject(currencyListBO);
        }
        
        [WebMethod]
        public static List<LCInformationDetailBO> LoadPurchaseOrderDetails(int pOrderId)
        {
            List<PMPurchaseOrderDetailsBO> pd = new List<PMPurchaseOrderDetailsBO>();
            PMPurchaseOrderDA orderDetailDA = new PMPurchaseOrderDA();
            List<LCInformationDetailBO> ld = new List<LCInformationDetailBO>();

            pd = orderDetailDA.GetPMPurchaseOrderDetailByOrderId(pOrderId);

            for(int i =0; i< pd.Count; i++)
            {
                LCInformationDetailBO DetailsBO = new LCInformationDetailBO();

                DetailsBO.LCDetailId = 0;
                DetailsBO.LCId = 0;
                DetailsBO.POrderId = pd[i].POrderId;
                DetailsBO.CostCenterId = pd[i].CostCenterId;
                DetailsBO.StockById = pd[i].StockById;
                DetailsBO.ProductId = pd[i].ItemId;
                DetailsBO.ItemName = pd[i].ItemName;
                if (pd[i].CurrencyId == 1)
                {
                    DetailsBO.PurchasePrice = pd[i].PurchasePrice;
                }
                else
                {
                    DetailsBO.PurchasePrice = pd[i].PurchasePrice * pd[i].ConvertionRate;
                }
                
                DetailsBO.Quantity = pd[i].Quantity;
                DetailsBO.StockBy = pd[i].StockBy;


                ld.Add(DetailsBO);
            }

            

            return ld;
        }

        [WebMethod]
        public static ReturnInfo SaveOrUpdateLCInformation(LCInformationViewBO LCInformationViewBOForAdded, LCInformationViewBO LCInformationViewBOForDeleted, string hfRandom, string deletedDocument)
        {
            ReturnInfo info = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            LCInformationDA lcDA = new LCInformationDA();
            LCInformationViewBOForAdded.LCInformation.CreatedBy = userInformationBO.UserInfoId;
            long id = 0;
            long OwnerIdForDocuments = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            try
            {
                status = lcDA.SaveOrUpdateLCInformation(LCInformationViewBOForAdded, LCInformationViewBOForDeleted, out id);


                if (status)
                {
                    info.IsSuccess = true;
                    if (LCInformationViewBOForAdded.LCInformation.LCId == 0)
                    {
                        OwnerIdForDocuments = id;
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        info.Data = 0;
                    }
                    else
                    {
                        OwnerIdForDocuments = id;
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }

                    DocumentsDA documentsDA = new DocumentsDA();
                    if (!string.IsNullOrEmpty(deletedDocument))
                    {
                        bool delete = documentsDA.DeleteDocument(deletedDocument);
                    }
                    Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(hfRandom));
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
        public static List<PMPurchaseOrderBO> LoadPO(int supplierId)
        {
            PMPurchaseOrderDA entityDA = new PMPurchaseOrderDA();
            List<PMPurchaseOrderBO> boList = entityDA.GetPMPurchaseOrderInfoBySupplierId(supplierId);
            return boList;
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

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("LCInformationDoc", randomId);

            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("LCInformationDoc", id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));

            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }

        [WebMethod]
        public static CommonCurrencyConversionBO LoadCurrencyConversionRate(int currecyType)
        {
            CommonCurrencyDA commonCurrencyDA = new CommonCurrencyDA();
            CommonCurrencyConversionBO conversionBO = new CommonCurrencyConversionBO();
            conversionBO = commonCurrencyDA.GetCurrencyConversionRate(currecyType);
            return conversionBO;
        }

        [WebMethod]
        public static LCInformationViewBO GetLcById(int id)
        {
            LCInformationViewBO ViewBO = new LCInformationViewBO();
            LCInformationDA lcDA = new LCInformationDA();

            ViewBO = lcDA.GetLCInformationALlDetailsByLCNumber(id);

            return ViewBO;
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
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("LCDoc", randomId);
            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("LCDoc", (int)id));

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
        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "LoadDetailGridInformation", jscript, true);

            // flashUpload.QueryParameters = "LCInformationDocId=" + Server.UrlEncode(RandomProductId.Value);

            //flashUpload.QueryParameters = "InventoryProductId=" + Server.UrlEncode(RandomProductId.Value) + "~" + txtCode.Text;
        }
    }
}