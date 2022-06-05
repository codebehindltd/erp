// According to http://msdn2.microsoft.com/en-us/library/system.web.httppostedfile.aspx
// "Files are uploaded in MIME multipart/form-data format. 
// By default, all requests, including form fields and uploaded files, 
// larger than 256 KB are buffered to disk, rather than held in server memory."
// So we can use an HttpHandler to handle uploaded files and not have to worry
// about the server recycling the request do to low memory. 
// don't forget to increase the MaxRequestLength in the web.config.
// If you server is still giving errors, then something else is wrong.
// I've uploaded a 1.3 gig file without any problems. One thing to note, 
// when the SaveAs function is called, it takes time for the server to 
// save the file. The larger the file, the longer it takes.
// So if a progress bar is used in the upload, it may read 100%, but the upload won't
// be complete until the file is saved.  So it may look like it is stalled, but it
// is not.

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Net;
using System.Text;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using System.Data;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using System.Web.SessionState;
using HotelManagement.Data.DocumentManagement;
using HotelManagement.Entity.DocumentManagement;

/// <summary>
/// Upload handler for uploading files.
/// </summary>
public class Upload : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public Upload()
    {
        
    }
    #region IHttpHandler Members
    public bool IsReusable
    {
        get { return true; }
    }
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string queryString = "";
        int CustomerId = -1;

        if (context.Request.Files.Count > 0)
        {
            try  //
            {
                
                string BanquetSeatingId = context.Request.QueryString["BanquetSeatingId"];
                int banquetSeatingId = -1;
                if (!string.IsNullOrEmpty(BanquetSeatingId))
                {
                    banquetSeatingId = Convert.ToInt32(BanquetSeatingId);
                }

                string buffetId = context.Request.QueryString["BuffetId"];
                int BuffetId = -1;
                if (!string.IsNullOrEmpty(buffetId))
                {
                    BuffetId = Convert.ToInt32(buffetId);
                }

                string comboId = context.Request.QueryString["ComboId"];
                int ComboId = -1;
                if (!string.IsNullOrEmpty(comboId))
                {
                    ComboId = Convert.ToInt32(comboId);
                }


                string restaurentItemId = context.Request.QueryString["RestaurentItemId"];
                int RestaurentItemId = -1;
                if (!string.IsNullOrEmpty(restaurentItemId))
                {
                    RestaurentItemId = Convert.ToInt32(restaurentItemId);
                }

                string categoryId = context.Request.QueryString["CategoryId"];
                int CategoryId = -1;
                if (!string.IsNullOrEmpty(categoryId))
                {
                    CategoryId = Convert.ToInt32(categoryId);
                }

                string guestIdString = context.Request.QueryString["guestId"];
                int tempOwnerId = Convert.ToInt32(guestIdString);
                if (!string.IsNullOrEmpty(context.Request.QueryString["employeeId"]))
                {
                    queryString = context.Request.QueryString["employeeId"].ToString();
                }

                if (!string.IsNullOrEmpty(context.Request.QueryString["CustomerId"]))
                {
                    CustomerId = Int32.Parse(context.Request.QueryString["CustomerId"]);
                }


                string strInventoryProductId = context.Request.QueryString["InventoryProductId"];

                //string inventoryProductId = strInventoryProductId.Split('~')[0].ToString();
                //string inventoryProductName = strInventoryProductId.Split('~')[1].ToString();
                string inventoryProductId = context.Request.QueryString["InventoryProductId"];
                int InventoryProductId = -1;
                if (!string.IsNullOrEmpty(inventoryProductId))
                {
                    InventoryProductId = Convert.ToInt32(inventoryProductId);
                }

                string vacantImageId = context.Request.QueryString["VacantImageId"];
                int VacantImageId = -1;
                if (!string.IsNullOrEmpty(vacantImageId))
                {
                    VacantImageId = Convert.ToInt32(vacantImageId);
                }
                string occupiedImageId = context.Request.QueryString["OccupiedImageId"];
                int OccupiedImageId = -1;
                if (!string.IsNullOrEmpty(occupiedImageId))
                {
                    OccupiedImageId = Convert.ToInt32(occupiedImageId);
                }


                string companyDocId = context.Request.QueryString["CompanyDocId"];
                string contactDocId = context.Request.QueryString["ContactDocId"];
                int CompanyDocId = -1;
                int ContactDocId = -1; 
                if (!string.IsNullOrEmpty(companyDocId))
                {
                    CompanyDocId = Convert.ToInt32(companyDocId);
                }
                if (!string.IsNullOrEmpty(contactDocId))
                {
                    ContactDocId = Convert.ToInt32(contactDocId);
                }

                string lcDocId = context.Request.QueryString["LCDocId"];
                int LCDocId = -1;
                if (!string.IsNullOrEmpty(lcDocId))
                {
                    LCDocId = Convert.ToInt32(lcDocId);
                }
                string overHeadDocId = context.Request.QueryString["OverHeadDocId"];
                int OverHeadDocId = -1;
                if (!string.IsNullOrEmpty(overHeadDocId))
                {
                    OverHeadDocId = Convert.ToInt32(overHeadDocId);
                }

                string billVoucherDocId = context.Request.QueryString["BillVoucherDocId"];
                int BillVoucherDocId = -1;
                if (!string.IsNullOrEmpty(billVoucherDocId))
                {
                    BillVoucherDocId = Convert.ToInt32(billVoucherDocId);
                }

                string requestForQuotationFeedbackDocId = context.Request.QueryString["RequestForQuotationFeedbackDocId"];
                int RequestForQuotationFeedbackDocId = -1;
                if (!string.IsNullOrEmpty(requestForQuotationFeedbackDocId))
                {
                    RequestForQuotationFeedbackDocId = Convert.ToInt32(requestForQuotationFeedbackDocId);
                }

                string requestForQuotationDocId = context.Request.QueryString["RequestForQuotationDocId"];
                int RequestForQuotationDocId = -1;
                if (!string.IsNullOrEmpty(requestForQuotationDocId))
                {
                    RequestForQuotationDocId = Convert.ToInt32(requestForQuotationDocId);
                }

                string requestForQuotationNSupplierDocId = context.Request.QueryString["RequestForQuotationNSupplierDocId"];
                int RequestForQuotationNSupplierDocId = -1;
                if (!string.IsNullOrEmpty(requestForQuotationNSupplierDocId))
                {
                    RequestForQuotationNSupplierDocId = Convert.ToInt32(requestForQuotationNSupplierDocId);
                }

                string lCInformationDocId = context.Request.QueryString["LCInformationDocId"];
                int LCInformationDocId = -1;
                if (!string.IsNullOrEmpty(lCInformationDocId))
                {
                    LCInformationDocId = Convert.ToInt32(lCInformationDocId);
                }

                string cashRequisitionApprovalDocId = context.Request.QueryString["CashRequisitionApprovalDocId"];
                int CashRequisitionApprovalDocId = -1;
                if (!string.IsNullOrEmpty(cashRequisitionApprovalDocId))
                {
                    CashRequisitionApprovalDocId = Convert.ToInt32(cashRequisitionApprovalDocId);
                }

                string InventoryProductCategoryId = context.Request.QueryString["InventoryProductCategoryId"];
                int inventoryProductCategoryId = -1;
                if (!string.IsNullOrEmpty(InventoryProductCategoryId))
                {
                    inventoryProductCategoryId = Convert.ToInt32(InventoryProductCategoryId);
                }

                string SiteSurveyDocId = context.Request.QueryString["SiteSurveyDocId"];
                int siteSurveyDocId = -1;
                if (!string.IsNullOrEmpty(SiteSurveyDocId))
                {
                    siteSurveyDocId = Convert.ToInt32(SiteSurveyDocId);
                }


                string queryDealId = context.Request.QueryString["DealId"];
                string queryDealFeedbackId = context.Request.QueryString["DealFeedbackId"];
                int dealId = 0;
                int dealFeedbackId = 0;

                if (!string.IsNullOrEmpty(queryDealId))
                {
                    dealId = Convert.ToInt32(HttpContext.Current.Session["DealId"]);
                    //dealId = Convert.ToInt32(queryDealId);
                }

                if (!string.IsNullOrEmpty(queryDealFeedbackId))
                {
                    dealFeedbackId = Convert.ToInt32(queryDealFeedbackId);
                }

                string TaskAssignDocId = context.Request.QueryString["TaskAssignDocId"];
                int TaskAssignId = 0;

                if (!string.IsNullOrEmpty(TaskAssignDocId))
                {
                    TaskAssignId = Convert.ToInt32(TaskAssignDocId);
                }

                string ReceiveOrderDocId = context.Request.QueryString["ReceiveOrderDocId"];
                int ReceiveOrderId = 0;

                if (!string.IsNullOrEmpty(ReceiveOrderDocId))
                {
                    ReceiveOrderId = Convert.ToInt32(HttpContext.Current.Session["ReceiveOrderDocId"]); 
                }

                string SupplierDocId = context.Request.QueryString["SupplierDocId"];
                int SupplierId = 0;

                if (!string.IsNullOrEmpty(SupplierDocId))
                {
                    SupplierId = Convert.ToInt32(HttpContext.Current.Session["SupplierDocId"]); 
                }

                string LostItemDocId = context.Request.QueryString["LostItemDocId"];
                int LostItemId = 0;

                if (!string.IsNullOrEmpty(LostItemDocId))
                {
                    LostItemId = Convert.ToInt32(HttpContext.Current.Session["LostItemDocId"]); 
                }

                string LostItemReturnDocId = context.Request.QueryString["LostItemReturnDocId"];
                int LostItemReturnId = 0;

                if (!string.IsNullOrEmpty(LostItemReturnDocId))
                {
                    LostItemReturnId = Convert.ToInt32(HttpContext.Current.Session["LostItemReturnDocId"]); 
                }

                string ProjectDocId = context.Request.QueryString["ProjectId"];
                int ProjectId = 0;

                if (!string.IsNullOrEmpty(ProjectDocId))
                {
                    ProjectId = Convert.ToInt32(ProjectDocId);
                }

                string queryLedgerMasterId = context.Request.QueryString["LedgerMasterId"];
                int LedgerMasterId = 0;

                if (!string.IsNullOrEmpty(queryLedgerMasterId))
                {
                    LedgerMasterId = Convert.ToInt32(HttpContext.Current.Session["LedgerMasterId"]);
                }

                string DocumentDocId = context.Request.QueryString["DocumentDocId"];
                int DocumentId = 0;

                if (!string.IsNullOrEmpty(DocumentDocId))
                {
                    DocumentId = Convert.ToInt32(DocumentDocId);
                }

                string RepairNMaintenanceDocId = context.Request.QueryString["RepairNMaintenanceId"];
                int RepairNMaintenanceId = 0;

                if (!string.IsNullOrEmpty(RepairNMaintenanceDocId))
                {
                    RepairNMaintenanceId = Convert.ToInt32(HttpContext.Current.Session["RepairNMaintenanceId"]);
                }
                string empTransferDocId = context.Request.QueryString["EmpTransferDocuments"];
                int EmpTransferDocId = 0;

                if (!string.IsNullOrEmpty(empTransferDocId))
                {
                    EmpTransferDocId = Convert.ToInt32(HttpContext.Current.Session["EmpTransferDocuments"]);
                } 
                
                string companyBillReceiveDocId = context.Request.QueryString["CompanyBillReceive"];
                int CompanyBillReceiveDocId = 0;

                if (!string.IsNullOrEmpty(companyBillReceiveDocId))
                {
                    CompanyBillReceiveDocId = Convert.ToInt32(HttpContext.Current.Session["CompanyBillReceive"]);
                }

                if (tempOwnerId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();
                    for (int j = 0; j < context.Request.Files.Count; j++)
                    {
                        HttpPostedFile uploadFile = context.Request.Files[j];
                        if (uploadFile.ContentLength > 0)
                        {
                            DocumentsBO signatureBO = new DocumentsBO();
                            string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\HotelManagement\\Image\\");
                            string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                            string extension = Path.GetExtension(uploadFile.FileName);

                            fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                            uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                            string fileName = uploadFile.FileName;
                            signatureBO.OwnerId = tempOwnerId;
                            signatureBO.Name = fileNames;
                            signatureBO.Path = @"/HotelManagement/Image/";
                            signatureBO.Extention = extension;
                            signatureBO.DocumentType = "Image";
                            signatureBO.DocumentCategory = "Guest";
                            signatureBO.CreatedBy = userBO.UserInfoId;
                            docList.Add(signatureBO);
                            Boolean status = docDA.SaveDocumentsInfo(docList);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(queryString))
                {
                    string[] separators = { "-" };
                    string[] words = queryString.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    string Id = words[0];
                    string Type = words[1];

                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();
                    string uploadFolder = "";

                    HttpPostedFile uploadFile = context.Request.Files[0];
                    if (uploadFile.ContentLength > 0)
                    {
                        DocumentsBO signatureBO = new DocumentsBO();
                        if (Type == "Employee Signature")
                        {
                            uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\Payroll\\Images\\Signature");
                            signatureBO.Path = @"/Payroll/Images/Signature/";
                        }
                        else if (Type == "Applicant Signature")
                        {
                            uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\Recruitment\\Images\\Signature");
                            signatureBO.Path = @"/Recruitment/Images/Signature/";
                        }
                        else if (Type == "Employee Document")
                        {
                            uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\Payroll\\Images\\Documents");
                            signatureBO.Path = @"/Payroll/Images/Documents/";
                        }
                        else if (Type == "Applicant Document")
                        {
                            uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\Recruitment\\Images\\Documents");
                            signatureBO.Path = @"/Recruitment/Images/Documents/";
                        }
                        else if (Type == "Employee Other Documents")
                        {
                            uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\Payroll\\Images\\Others");
                            signatureBO.Path = @"/Payroll/Images/Others/";
                        }
                        else if (Type == "Applicant Other Documents")
                        {
                            uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\Recruitment\\Images\\Others");
                            signatureBO.Path = @"/Recruitment/Images/Others/";
                        }
                        string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                        string extension = Path.GetExtension(uploadFile.FileName);

                        fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                        uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                        signatureBO.OwnerId = Int32.Parse(Id);
                        signatureBO.Name = fileNames;
                        signatureBO.Extention = extension;
                        signatureBO.DocumentType = "Image";
                        signatureBO.DocumentCategory = Type;
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }

                }
                else if (CustomerId != -1)
                {

                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();
                    for (int j = 0; j < context.Request.Files.Count; j++)
                    {
                        HttpPostedFile uploadFile = context.Request.Files[j];
                        if (uploadFile.ContentLength > 0)
                        {
                            DocumentsBO signatureBO = new DocumentsBO();
                            string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\SalesManagment\\Image\\");

                            string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                            string extension = Path.GetExtension(uploadFile.FileName);

                            fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                            uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                            string fileName = uploadFile.FileName;
                            signatureBO.OwnerId = CustomerId;
                            signatureBO.Name = fileNames;
                            signatureBO.Path = @"/SalesManagment/Image/";
                            signatureBO.Extention = extension;
                            signatureBO.DocumentType = "Image";
                            signatureBO.DocumentCategory = "Sales Contact Details";
                            signatureBO.CreatedBy = userBO.UserInfoId;
                            docList.Add(signatureBO);
                            Boolean status = docDA.SaveDocumentsInfo(docList);
                        }
                    }

                }
                else if (RestaurentItemId != -1)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();

                        HttpPostedFile uploadFile = context.Request.Files[0];
                        if (uploadFile.ContentLength > 0)
                        {
                            DocumentsBO signatureBO = new DocumentsBO();
                            string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\Restaurant\\CategoryImages\\");

                            uploadFile.SaveAs(Path.Combine(uploadFolder, uploadFile.FileName));
                            string extension = Path.GetExtension(uploadFile.FileName);

                            string fileName = uploadFile.FileName;
                            signatureBO.OwnerId = RestaurentItemId;
                            signatureBO.Name = uploadFile.FileName;
                            signatureBO.Path = @"/Restaurant/CategoryImages/";
                            signatureBO.Extention = extension;
                            signatureBO.DocumentType = "Image";
                            signatureBO.DocumentCategory = "RestaurentItem";
                            signatureBO.CreatedBy = userBO.UserInfoId;
                            docList.Add(signatureBO);
                            Boolean status = docDA.SaveDocumentsInfo(docList);
                        }
               
                }
                else if (CategoryId != -1)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();

                    HttpPostedFile uploadFile = context.Request.Files[0];
                    if (uploadFile.ContentLength > 0)
                    {
                        DocumentsBO signatureBO = new DocumentsBO();
                        string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\Restaurant\\ItemImages\\");

                        uploadFile.SaveAs(Path.Combine(uploadFolder, uploadFile.FileName));
                        string extension = Path.GetExtension(uploadFile.FileName);

                        string fileName = uploadFile.FileName;
                        signatureBO.OwnerId = CategoryId;
                        signatureBO.Name = uploadFile.FileName;
                        signatureBO.Path = @"/Restaurant/ItemImages/";
                        signatureBO.Extention = extension;
                        signatureBO.DocumentType = "Image";
                        signatureBO.DocumentCategory = "ItemCategory";
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }

                }
                else if (ComboId != -1)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();

                    HttpPostedFile uploadFile = context.Request.Files[0];
                    if (uploadFile.ContentLength > 0)
                    {
                        DocumentsBO signatureBO = new DocumentsBO();
                        string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\Restaurant\\ComboImages\\");

                        uploadFile.SaveAs(Path.Combine(uploadFolder, uploadFile.FileName));
                        string extension = Path.GetExtension(uploadFile.FileName);

                        string fileName = uploadFile.FileName;
                        signatureBO.OwnerId = ComboId;
                        signatureBO.Name = uploadFile.FileName;
                        signatureBO.Path = @"/Restaurant/ComboImages/";
                        signatureBO.Extention = extension;
                        signatureBO.DocumentType = "Image";
                        signatureBO.DocumentCategory = "RestaurantCombo";
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }
                }
                else if (BuffetId != -1)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();

                    HttpPostedFile uploadFile = context.Request.Files[0];
                    if (uploadFile.ContentLength > 0)
                    {
                        DocumentsBO signatureBO = new DocumentsBO();
                        string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\Restaurant\\BuffetImages\\");

                        uploadFile.SaveAs(Path.Combine(uploadFolder, uploadFile.FileName));
                        string extension = Path.GetExtension(uploadFile.FileName);
                        string fileName = uploadFile.FileName;
                        signatureBO.OwnerId = BuffetId;
                        signatureBO.Name = uploadFile.FileName;
                        signatureBO.Path = @"/Restaurant/BuffetImages/";
                        signatureBO.Extention = extension;
                        signatureBO.DocumentType = "Image";
                        signatureBO.DocumentCategory = "RestaurantBuffet";
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }
                }
                else if (banquetSeatingId != -1)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();

                    HttpPostedFile uploadFile = context.Request.Files[0];
                    if (uploadFile.ContentLength > 0)
                    {
                        DocumentsBO signatureBO = new DocumentsBO();
                        string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\Banquet\\SeatingPlanImage\\");

                        uploadFile.SaveAs(Path.Combine(uploadFolder, uploadFile.FileName));
                        string extension = Path.GetExtension(uploadFile.FileName);
                        string fileName = uploadFile.FileName;
                        signatureBO.OwnerId = banquetSeatingId;
                        signatureBO.Name = uploadFile.FileName;
                        signatureBO.Path = @"/Banquet/SeatingPlanImage/";
                        signatureBO.Extention = extension;
                        signatureBO.DocumentType = "Image";
                        signatureBO.DocumentCategory = "BanquetSeatingPlan";
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }

                }
                else if (InventoryProductId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();

                    HttpPostedFile uploadFile = context.Request.Files[0];
                    if (uploadFile.ContentLength > 0)
                    {
                        DocumentsBO signatureBO = new DocumentsBO();
                        string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\Inventory\\Images\\Product\\");

                        uploadFile.SaveAs(Path.Combine(uploadFolder, uploadFile.FileName));
                        string extension = Path.GetExtension(uploadFile.FileName);
                        string fileName = uploadFile.FileName;
                        signatureBO.OwnerId = InventoryProductId;
                        signatureBO.Name = uploadFile.FileName;
                        signatureBO.Path = @"/Inventory/Images/Product/";
                        signatureBO.Extention = extension;
                        signatureBO.DocumentType = "Image";
                        signatureBO.DocumentCategory = "InventoryProduct";
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }               
                }
                else if (inventoryProductCategoryId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();
                    HttpPostedFile uploadFile = context.Request.Files[0];
                    if (uploadFile.ContentLength > 0)
                    {
                        DocumentsBO signatureBO = new DocumentsBO();
                        string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\Inventory\\Images\\Category\\");

                        uploadFile.SaveAs(Path.Combine(uploadFolder, uploadFile.FileName));
                        string extension = Path.GetExtension(uploadFile.FileName);
                        string fileName = uploadFile.FileName;
                        signatureBO.OwnerId = inventoryProductCategoryId;
                        signatureBO.Name = uploadFile.FileName;
                        signatureBO.Path = @"/Inventory/Images/Category/";
                        signatureBO.Extention = extension;
                        signatureBO.DocumentType = "Image";
                        signatureBO.DocumentCategory = "InventoryProductCategory";
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }               
                }
                else if (siteSurveyDocId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();
                    HttpPostedFile uploadFile = context.Request.Files[0];
                    if (uploadFile.ContentLength > 0)
                    {
                        DocumentsBO signatureBO = new DocumentsBO();
                        string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\SalesAndMarketing\\Images\\SiteSurvey\\");

                        uploadFile.SaveAs(Path.Combine(uploadFolder, uploadFile.FileName));
                        string extension = Path.GetExtension(uploadFile.FileName);
                        string fileName = uploadFile.FileName;
                        signatureBO.OwnerId = siteSurveyDocId;
                        signatureBO.Name = uploadFile.FileName;
                        signatureBO.Path = @"/SalesAndMarketing/Images/SiteSurvey/";
                        signatureBO.Extention = extension;
                        if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".psd" || extension == ".tiff" || extension == ".indd" || extension == ".ai")
                        {
                            signatureBO.DocumentType = "Image";
                        }
                        else
                        {
                            signatureBO.DocumentType = "Document";
                        }
                        signatureBO.DocumentCategory = "SiteSurveyDoc";
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }
                }
                else if (VacantImageId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();

                    HttpPostedFile uploadFile = context.Request.Files[0];
                    if (uploadFile.ContentLength > 0)
                    {
                        DocumentsBO signatureBO = new DocumentsBO();
                        string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\POS\\Images\\VacantTable\\");

                        uploadFile.SaveAs(Path.Combine(uploadFolder, uploadFile.FileName));
                        string extension = Path.GetExtension(uploadFile.FileName);

                        string fileName = uploadFile.FileName;
                        signatureBO.OwnerId = VacantImageId;
                        signatureBO.Name = uploadFile.FileName;
                        signatureBO.Path = @"/POS/Images/VacantTable/";
                        signatureBO.Extention = extension;
                        signatureBO.DocumentType = "Image";
                        signatureBO.DocumentCategory = "VacantTableDocument";
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }
                }
                else if (OccupiedImageId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();

                    HttpPostedFile uploadFile = context.Request.Files[0];
                    if (uploadFile.ContentLength > 0)
                    {
                        DocumentsBO signatureBO = new DocumentsBO();
                        string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\POS\\Images\\OccupiedTable\\");

                        uploadFile.SaveAs(Path.Combine(uploadFolder, uploadFile.FileName));
                        string extension = Path.GetExtension(uploadFile.FileName);

                        string fileName = uploadFile.FileName;
                        signatureBO.OwnerId = OccupiedImageId;
                        signatureBO.Name = uploadFile.FileName;
                        signatureBO.Path = @"/POS/Images/OccupiedTable/";
                        signatureBO.Extention = extension;
                        signatureBO.DocumentType = "Image";
                        signatureBO.DocumentCategory = "OccupiedTableDocument";
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }
                }
                else if (CompanyDocId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();

                    HttpPostedFile uploadFile = context.Request.Files[0];
                    if (uploadFile.ContentLength > 0)
                    {
                        DocumentsBO signatureBO = new DocumentsBO();
                        string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\SalesAndMarketing\\Images\\Company\\");

                        uploadFile.SaveAs(Path.Combine(uploadFolder, uploadFile.FileName));
                        string extension = Path.GetExtension(uploadFile.FileName);

                        string fileName = uploadFile.FileName;
                        signatureBO.OwnerId = CompanyDocId;
                        signatureBO.Name = uploadFile.FileName;
                        signatureBO.Path = @"/SalesAndMarketing/Images/Company/";
                        signatureBO.Extention = extension;
                        signatureBO.DocumentType = "Image";
                        signatureBO.DocumentCategory = "CompanyDocument";
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }
                }
                else if (ContactDocId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();

                    HttpPostedFile uploadFile = context.Request.Files[0];
                    if (uploadFile.ContentLength > 0)
                    {
                        DocumentsBO signatureBO = new DocumentsBO();
                        string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\SalesAndMarketing\\Images\\Contact\\");

                        string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                        string extension = Path.GetExtension(uploadFile.FileName);

                        fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                        uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));
                        
                        string fileName = uploadFile.FileName;
                        signatureBO.OwnerId = ContactDocId;
                        signatureBO.Name = fileNames;
                        signatureBO.Path = @"/SalesAndMarketing/Images/Contact/";
                        signatureBO.Extention = extension;
                        signatureBO.DocumentType = "Image";
                        signatureBO.DocumentCategory = "ContactDocument";
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }
                }
                
                else if (BillVoucherDocId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();

                    HttpPostedFile uploadFile = context.Request.Files[0];
                    if (uploadFile.ContentLength > 0)
                    {
                        DocumentsBO signatureBO = new DocumentsBO();
                        string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\Payroll\\Images\\BillVoucher\\");

                        string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                        string extension = Path.GetExtension(uploadFile.FileName);

                        fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                        uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));
                        
                        string fileName = uploadFile.FileName;
                        signatureBO.OwnerId = BillVoucherDocId;
                        signatureBO.Name = fileNames;
                        signatureBO.Path = @"/Payroll/Images/BillVoucher/";
                        signatureBO.Extention = extension;
                        signatureBO.DocumentType = "Image";
                        signatureBO.DocumentCategory = "BillVoucherDoc";
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }
                }
                else if (RequestForQuotationDocId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();

                    HttpPostedFile uploadFile = context.Request.Files[0];
                    if (uploadFile.ContentLength > 0)
                    {
                        DocumentsBO signatureBO = new DocumentsBO();
                        string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\PurchaseManagment\\Images\\RequestForQuotation\\");

                        string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                        string extension = Path.GetExtension(uploadFile.FileName);

                        fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                        uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                        string fileName = uploadFile.FileName;
                        signatureBO.OwnerId = RequestForQuotationDocId;
                        signatureBO.Name = fileNames;
                        signatureBO.Path = @"/PurchaseManagment/Images/RequestForQuotation/";
                        signatureBO.Extention = extension;
                        signatureBO.DocumentType = "Image";
                        signatureBO.DocumentCategory = "RequestForQuotationDoc";
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }
                }

                else if (RequestForQuotationFeedbackDocId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();

                    HttpPostedFile uploadFile = context.Request.Files[0];
                    if (uploadFile.ContentLength > 0)
                    {
                        DocumentsBO signatureBO = new DocumentsBO();
                        string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\PurchaseManagment\\Images\\RequestForQuotation\\");

                        string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                        string extension = Path.GetExtension(uploadFile.FileName);

                        fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                        uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                        string fileName = uploadFile.FileName;
                        signatureBO.OwnerId = RequestForQuotationFeedbackDocId;
                        signatureBO.Name = fileNames;
                        signatureBO.Path = @"/PurchaseManagment/Images/RequestForQuotation/";
                        signatureBO.Extention = extension;
                        signatureBO.DocumentType = "Image";
                        signatureBO.DocumentCategory = "RequestForQuotationFeedbackDoc";
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }
                }
                else if (RequestForQuotationNSupplierDocId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();

                    HttpPostedFile uploadFile = context.Request.Files[0];
                    if (uploadFile.ContentLength > 0)
                    {
                        DocumentsBO signatureBO = new DocumentsBO();
                        string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\PurchaseManagment\\Images\\RequestForQuotation\\");

                        string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                        string extension = Path.GetExtension(uploadFile.FileName);

                        fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                        uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                        string fileName = uploadFile.FileName;
                        signatureBO.OwnerId = RequestForQuotationNSupplierDocId;
                        signatureBO.Name = fileNames;
                        signatureBO.Path = @"/PurchaseManagment/Images/RequestForQuotation/";
                        signatureBO.Extention = extension;
                        signatureBO.DocumentType = "Image";
                        signatureBO.DocumentCategory = "RequestForQuotationNSupplierDoc";
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }
                }

                

                else if (LCInformationDocId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();

                    HttpPostedFile uploadFile = context.Request.Files[0];
                    if (uploadFile.ContentLength > 0)
                    {
                        DocumentsBO signatureBO = new DocumentsBO();
                        string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\LCManagement\\Images\\");

                        string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                        string extension = Path.GetExtension(uploadFile.FileName);

                        fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                        uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                        string fileName = uploadFile.FileName;
                        signatureBO.OwnerId = LCInformationDocId;
                        signatureBO.Name = fileNames;
                        signatureBO.Path = @"/LCManagement/Images/";
                        signatureBO.Extention = extension;
                        signatureBO.DocumentType = "Image";
                        signatureBO.DocumentCategory = "LCInformationDoc";
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }
                }
                else if (CashRequisitionApprovalDocId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();

                    HttpPostedFile uploadFile = context.Request.Files[0];
                    if (uploadFile.ContentLength > 0)
                    {
                        DocumentsBO signatureBO = new DocumentsBO();
                        string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\Payroll\\Images\\CashRequisition\\");

                        string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                        string extension = Path.GetExtension(uploadFile.FileName);

                        fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                        uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                        string fileName = uploadFile.FileName;
                        signatureBO.OwnerId = CashRequisitionApprovalDocId;
                        signatureBO.Name = fileNames;
                        signatureBO.Path = @"/Payroll/Images/CashRequisition/";
                        signatureBO.Extention = extension;
                        signatureBO.DocumentType = "Image";
                        signatureBO.DocumentCategory = "CashRequisitionApprovalDoc";
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }
                }
                else if (LCDocId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();

                    HttpPostedFile uploadFile = context.Request.Files[0];
                    if (uploadFile.ContentLength > 0)
                    {
                        DocumentsBO signatureBO = new DocumentsBO();
                        string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\LCManagement\\Images\\");

                        uploadFile.SaveAs(Path.Combine(uploadFolder, uploadFile.FileName));
                        string extension = Path.GetExtension(uploadFile.FileName);

                        string fileName = uploadFile.FileName;
                        signatureBO.OwnerId = LCDocId;
                        signatureBO.Name = uploadFile.FileName;
                        signatureBO.Path = @"/LCManagement/Images/";
                        signatureBO.Extention = extension;
                        signatureBO.DocumentType = "Image";
                           
                        signatureBO.DocumentCategory = "LCDoc";
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }
                }
                else if (OverHeadDocId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();

                    HttpPostedFile uploadFile = context.Request.Files[0];
                    if (uploadFile.ContentLength > 0)
                    {
                        DocumentsBO signatureBO = new DocumentsBO();
                        string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\LCManagement\\Images\\");

                        uploadFile.SaveAs(Path.Combine(uploadFolder, uploadFile.FileName));
                        string extension = Path.GetExtension(uploadFile.FileName);

                        string fileName = uploadFile.FileName;
                        signatureBO.OwnerId = OverHeadDocId;
                        signatureBO.Name = uploadFile.FileName;
                        signatureBO.Path = @"/LCManagement/Images/";
                        signatureBO.Extention = extension;
                        signatureBO.DocumentType = "Image";
                           
                        signatureBO.DocumentCategory = "OverHeadDoc";
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }
                }
                else if (dealId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();
                    for (int j = 0; j < context.Request.Files.Count; j++)
                    {
                        HttpPostedFile uploadFile = context.Request.Files[j];
                        if (uploadFile.ContentLength > 0)
                        {
                            DocumentsBO signatureBO = new DocumentsBO();
                            string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\SalesAndMarketing\\Images\\Deal\\");

                            string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                            string extension = Path.GetExtension(uploadFile.FileName);

                            fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                            uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                            string fileName = uploadFile.FileName;
                            signatureBO.OwnerId = dealId;
                            signatureBO.Name = fileNames;
                            signatureBO.Path = @"/SalesAndMarketing/Images/Deal/";
                            signatureBO.Extention = extension;
                            signatureBO.DocumentType = "Image";
                            signatureBO.DocumentCategory = "SalesDealDocuments";
                            signatureBO.CreatedBy = userBO.UserInfoId;
                            docList.Add(signatureBO);
                            Boolean status = docDA.SaveDocumentsInfo(docList);
                        }
                    }
                }
                else if (TaskAssignId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();
                    for (int j = 0; j < context.Request.Files.Count; j++)
                    {
                        HttpPostedFile uploadFile = context.Request.Files[j];
                        if (uploadFile.ContentLength > 0)
                        {
                            DocumentsBO signatureBO = new DocumentsBO();
                            string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\TaskManagement\\Images\\TaskAssign\\");

                            string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                            string extension = Path.GetExtension(uploadFile.FileName);

                            fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                            uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                            string fileName = uploadFile.FileName;
                            signatureBO.OwnerId = TaskAssignId;
                            signatureBO.Name = fileNames;
                            signatureBO.Path = @"/TaskManagement/Images/TaskAssign/";
                            signatureBO.Extention = extension;
                            signatureBO.DocumentType = "Image";
                            signatureBO.DocumentCategory = "TaskAssignDocuments";
                            signatureBO.CreatedBy = userBO.UserInfoId;
                            docList.Add(signatureBO);
                            Boolean status = docDA.SaveDocumentsInfo(docList);
                        }
                    }
                }
                else if (ReceiveOrderId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();
                    for (int j = 0; j < context.Request.Files.Count; j++)
                    {
                        HttpPostedFile uploadFile = context.Request.Files[j];
                        if (uploadFile.ContentLength > 0)
                        {
                            DocumentsBO signatureBO = new DocumentsBO();
                            string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\Inventory\\Images\\Receive");

                            string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                            string extension = Path.GetExtension(uploadFile.FileName);

                            fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                            uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                            string fileName = uploadFile.FileName;
                            signatureBO.OwnerId = ReceiveOrderId;
                            signatureBO.Name = fileNames;
                            signatureBO.Path = @"/Inventory/Images/Receive";
                            signatureBO.Extention = extension;
                            signatureBO.DocumentType = "Image";
                            signatureBO.DocumentCategory = "ReceiveOrderDocuments";
                            signatureBO.CreatedBy = userBO.UserInfoId;
                            docList.Add(signatureBO);
                            Boolean status = docDA.SaveDocumentsInfo(docList);
                        }
                    }
                }
                else if (SupplierId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();
                    for (int j = 0; j < context.Request.Files.Count; j++)
                    {
                        HttpPostedFile uploadFile = context.Request.Files[j];
                        if (uploadFile.ContentLength > 0)
                        {
                            DocumentsBO signatureBO = new DocumentsBO();
                            string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\PurchaseManagment\\Images\\Supplier\\");

                            string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                            string extension = Path.GetExtension(uploadFile.FileName);

                            fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                            uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                            string fileName = uploadFile.FileName;
                            signatureBO.OwnerId = SupplierId;
                            signatureBO.Name = fileNames;
                            signatureBO.Path = @"/PurchaseManagment/Images/Supplier/";
                            signatureBO.Extention = extension;
                            signatureBO.DocumentType = "Image";
                            signatureBO.DocumentCategory = "SupplierDocuments";
                            signatureBO.CreatedBy = userBO.UserInfoId;
                            docList.Add(signatureBO);
                            Boolean status = docDA.SaveDocumentsInfo(docList);
                        }
                    }
                    
                    
                }
                else if (LostItemId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();
                    for (int j = 0; j < context.Request.Files.Count; j++)
                    {
                        HttpPostedFile uploadFile = context.Request.Files[j];
                        if (uploadFile.ContentLength > 0)
                        {
                            DocumentsBO signatureBO = new DocumentsBO();
                            string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\HouseKeeping\\Images\\LostNFound\\");

                            string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                            string extension = Path.GetExtension(uploadFile.FileName);

                            fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                            uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                            string fileName = uploadFile.FileName;
                            signatureBO.OwnerId = LostItemId;
                            signatureBO.Name = fileNames;
                            signatureBO.Path = @"/HouseKeeping/Images/LostNFound/";
                            signatureBO.Extention = extension;
                            signatureBO.DocumentType = "Image";
                            signatureBO.DocumentCategory = "LostItemDocuments";
                            signatureBO.CreatedBy = userBO.UserInfoId;
                            docList.Add(signatureBO);
                            Boolean status = docDA.SaveDocumentsInfo(docList);
                        }
                    }
                    
                    
                }
                else if (LostItemReturnId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();
                    for (int j = 0; j < context.Request.Files.Count; j++)
                    {
                        HttpPostedFile uploadFile = context.Request.Files[j];
                        if (uploadFile.ContentLength > 0)
                        {
                            DocumentsBO signatureBO = new DocumentsBO();
                            string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\HouseKeeping\\Images\\LostNFound\\");

                            string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                            string extension = Path.GetExtension(uploadFile.FileName);

                            fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                            uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                            string fileName = uploadFile.FileName;
                            signatureBO.OwnerId = LostItemReturnId;
                            signatureBO.Name = fileNames;
                            signatureBO.Path = @"/HouseKeeping/Images/LostNFound/";
                            signatureBO.Extention = extension;
                            signatureBO.DocumentType = "Image";
                            signatureBO.DocumentCategory = "LostItemReturnDocuments";
                            signatureBO.CreatedBy = userBO.UserInfoId;
                            docList.Add(signatureBO);
                            Boolean status = docDA.SaveDocumentsInfo(docList);
                        }
                    }
                    
                    
                }
                else if (dealFeedbackId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();
                    for (int j = 0; j < context.Request.Files.Count; j++)
                    {
                        HttpPostedFile uploadFile = context.Request.Files[j];
                        if (uploadFile.ContentLength > 0)
                        {
                            DocumentsBO signatureBO = new DocumentsBO();
                            string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\SalesAndMarketing\\Images\\Deal\\");

                            string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                            string extension = Path.GetExtension(uploadFile.FileName);

                            fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                            uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                            string fileName = uploadFile.FileName;
                            signatureBO.OwnerId = dealFeedbackId;
                            signatureBO.Name = fileNames;
                            signatureBO.Path = @"/SalesAndMarketing/Images/Deal/";
                            signatureBO.Extention = extension;
                            signatureBO.DocumentType = "Image";
                            signatureBO.DocumentCategory = "SalesDealFeedbackDocuments";
                            signatureBO.CreatedBy = userBO.UserInfoId;
                            docList.Add(signatureBO);
                            Boolean status = docDA.SaveDocumentsInfo(docList);
                        }
                    }
                }
                else if (ProjectId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();
                    for (int j = 0; j < context.Request.Files.Count; j++)
                    {
                        HttpPostedFile uploadFile = context.Request.Files[j];
                        if (uploadFile.ContentLength > 0)
                        {
                            DocumentsBO signatureBO = new DocumentsBO();
                            string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\GeneralLedger\\File\\GLProject\\");

                            string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                            string extension = Path.GetExtension(uploadFile.FileName);

                            fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                            uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                            string fileName = uploadFile.FileName;
                            signatureBO.OwnerId = ProjectId;
                            signatureBO.Name = fileNames;
                            signatureBO.Path = @"/GeneralLedger/File/GLProject/";
                            signatureBO.Extention = extension;
                            signatureBO.DocumentType = "Image";
                            signatureBO.DocumentCategory = "GLProjectDocument";
                            signatureBO.CreatedBy = userBO.UserInfoId;
                            docList.Add(signatureBO);
                            Boolean status = docDA.SaveDocumentsInfo(docList);
                        }
                    }
                }
                else if (LedgerMasterId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();
                    for (int j = 0; j < context.Request.Files.Count; j++)
                    {
                        HttpPostedFile uploadFile = context.Request.Files[j];
                        if (uploadFile.ContentLength > 0)
                        {
                            DocumentsBO signatureBO = new DocumentsBO();
                            string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\GeneralLedger\\File\\Voucher\\");

                            string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                            string extension = Path.GetExtension(uploadFile.FileName);

                            fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                            uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                            string fileName = uploadFile.FileName;
                            signatureBO.OwnerId = LedgerMasterId;
                            signatureBO.Name = fileNames;
                            signatureBO.Path = @"/GeneralLedger/File/Voucher/";
                            signatureBO.Extention = extension;
                            signatureBO.DocumentType = "Image";
                            signatureBO.DocumentCategory = "GLVoucherDocuments";
                            signatureBO.CreatedBy = userBO.UserInfoId;
                            docList.Add(signatureBO);
                            Boolean status = docDA.SaveDocumentsInfo(docList);
                        }
                    }
                }
                else if (DocumentId > 0)
                {
                    DocumentsForDocManagementDA docDA = new DocumentsForDocManagementDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsForDocManagementBO> docList = new List<DocumentsForDocManagementBO>();
                    for (int j = 0; j < context.Request.Files.Count; j++)
                    {
                        HttpPostedFile uploadFile = context.Request.Files[j];
                        if (uploadFile.ContentLength > 0)
                        {
                            DocumentsForDocManagementBO DocumentBO = new DocumentsForDocManagementBO();
                            string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\DocumentManagement\\Images\\Documents\\");

                            string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                            string extension = Path.GetExtension(uploadFile.FileName);

                            fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                            uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                            string fileName = uploadFile.FileName;
                            DocumentBO.OwnerId = DocumentId;
                            DocumentBO.Name = fileNames;
                            DocumentBO.Path = @"/DocumentManagement/Images/Documents/";
                            DocumentBO.Extention = extension;
                            DocumentBO.DocumentType = "doc";
                            DocumentBO.DocumentCategory = "DocumentsDoc";
                            DocumentBO.CreatedBy = userBO.UserInfoId;
                            docList.Add(DocumentBO);
                            Boolean status = docDA.SaveDocumentsInfo(docList);
                        }
                    }
                }                
                else if (RepairNMaintenanceId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();
                    for (int j = 0; j < context.Request.Files.Count; j++)
                    {
                        HttpPostedFile uploadFile = context.Request.Files[j];
                        if (uploadFile.ContentLength > 0)
                        {
                            DocumentsBO DocBO = new DocumentsBO();
                            string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\HouseKeeping\\Images\\RepairNMaintenance\\");

                            string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                            string extension = Path.GetExtension(uploadFile.FileName);

                            fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                            uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                            string fileName = uploadFile.FileName;
                            DocBO.OwnerId = RepairNMaintenanceId;
                            DocBO.Name = fileNames;
                            DocBO.Path = @"/HouseKeeping/Images/RepairNMaintenance/";
                            DocBO.Extention = extension;
                            DocBO.DocumentType = "Image";
                            DocBO.DocumentCategory = "RepairNMaintenanceDoc";
                            DocBO.CreatedBy = userBO.UserInfoId;
                            docList.Add(DocBO);
                            Boolean status = docDA.SaveDocumentsInfo(docList);
                        }
                    }


                }
                else if (EmpTransferDocId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();
                    for (int j = 0; j < context.Request.Files.Count; j++)
                    {
                        HttpPostedFile uploadFile = context.Request.Files[j];
                        if (uploadFile.ContentLength > 0)
                        {
                            DocumentsBO DocBO = new DocumentsBO();
                            string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\Payroll\\Images\\Transfer\\");

                            string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                            string extension = Path.GetExtension(uploadFile.FileName);

                            fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                            uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                            string fileName = uploadFile.FileName;
                            DocBO.OwnerId = RepairNMaintenanceId;
                            DocBO.Name = fileNames;
                            DocBO.Path = @"/Payroll/Images/Transfer/";
                            DocBO.Extention = extension;
                            DocBO.DocumentType = "Image";
                            DocBO.DocumentCategory = "EmpTransferDocuments";
                            DocBO.CreatedBy = userBO.UserInfoId;
                            docList.Add(DocBO);
                            Boolean status = docDA.SaveDocumentsInfo(docList);
                        }
                    }


                }
                else if (CompanyBillReceiveDocId > 0)
                {
                    DocumentsDA docDA = new DocumentsDA();
                    UserInformationBO userBO = new UserInformationBO();
                    List<DocumentsBO> docList = new List<DocumentsBO>();
                    for (int j = 0; j < context.Request.Files.Count; j++)
                    {
                        HttpPostedFile uploadFile = context.Request.Files[j];
                        if (uploadFile.ContentLength > 0)
                        {
                            DocumentsBO DocBO = new DocumentsBO();
                            string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\HotelManagement\\Image\\");

                            string fileNames = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                            string extension = Path.GetExtension(uploadFile.FileName);

                            fileNames = fileNames + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;
                            uploadFile.SaveAs(Path.Combine(uploadFolder, fileNames));

                            string fileName = uploadFile.FileName;
                            DocBO.OwnerId = RepairNMaintenanceId;
                            DocBO.Name = fileNames;
                            DocBO.Path = @"/HotelManagement/Image/";
                            DocBO.Extention = extension;
                            DocBO.DocumentType = "Image";
                            DocBO.DocumentCategory = "CompanyBillReceive";
                            DocBO.CreatedBy = userBO.UserInfoId;
                            docList.Add(DocBO);
                            Boolean status = docDA.SaveDocumentsInfo(docList);
                        }
                    }


                }
                else
                {
                    HttpPostedFile uploadFile = context.Request.Files[0];
                    CompanyBO companyBO = new CompanyBO();
                    CompanyDA companyDA = new CompanyDA();
                    string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + "\\HotelManagement\\Image\\");
                    uploadFile.SaveAs(Path.Combine(uploadFolder, uploadFile.FileName));
                    string extension = Path.GetExtension(uploadFile.FileName);
                    string fileName = uploadFile.FileName;
                    companyBO.ImageName = uploadFile.FileName;
                    companyBO.ImagePath = @"/HotelManagement/Image/";
                    Boolean status = companyDA.SaveOrUpdateCompanyDocuments(companyBO);
                }
            }
            catch (Exception ex)
            {
                //Log.Write(Log.Level.Error, "App_Code", "Upload handler", ex.ToString());
                throw ex;
            }
        }
        // Used as a fix for a bug in mac flash player that makes the 
        // onComplete event not fire
        HttpContext.Current.Response.Write(" ");
    }
    #endregion
}