using HotelManagement.Data.HMCommon;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.HMCommon;
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
    public partial class QuotaionFeedback : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

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


        [WebMethod]
        public static List<DocumentsBO> LoadContactDocument(long id, int randomId, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }

            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("RequestForQuotationFeedbackDoc", randomId);

            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("RequestForQuotationFeedbackDoc", id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));

            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }

        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "LoadDetailGridInformation", jscript, true);

            flashUpload.QueryParameters = "RequestForQuotationFeedbackDocId=" + Server.UrlEncode(RandomProductId.Value);



            //flashUpload.QueryParameters = "InventoryProductId=" + Server.UrlEncode(RandomProductId.Value) + "~" + txtCode.Text;
        }


        [WebMethod]
        public static GridViewDataNPaging<RFQuotationBO, GridPaging> GetRFQuotation(string quotationType, DateTime? fromDate, DateTime? toDate, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            List<RFQuotationBO> RFQuotationBOList = new List<RFQuotationBO>();
            GridViewDataNPaging<RFQuotationBO, GridPaging> myGridData = new GridViewDataNPaging<RFQuotationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);



            int supplierId = userInformationBO.SupplierId;

            PMRFQuotationDA rfqDA = new PMRFQuotationDA();

            RFQuotationBOList = rfqDA.GetRFQuotationForGridPaging(supplierId, quotationType, fromDate, toDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);



            myGridData.GridPagingProcessing(RFQuotationBOList, totalRecords);
            return myGridData;
        }

        [WebMethod]
        public static RFQuotationBO GetItemsForQuotation(int RFQId)
        {

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            RFQuotationBO rFQuotationBO = new RFQuotationBO();


            PMRFQuotationDA rfqDA = new PMRFQuotationDA();

            rFQuotationBO = rfqDA.GetItemsForQuotation(RFQId);
            



            return rFQuotationBO;
        }

        [WebMethod]
        public static ReturnInfo SaveOrUpdateRFQFeedback(RFQuotationFeedbackBO QuotationFeedbackInfo, string hfRandom, string deletedDocument)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            int OutId = 0;
            int itemId = 0;
            long OwnerIdForDocuments = 0;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            QuotationFeedbackInfo.SupplierId = userInformationBO.SupplierId;
            PMRFQuotationDA rfqDA = new PMRFQuotationDA();
            try
            {
                QuotationFeedbackInfo.CreatedBy = userInformationBO.UserInfoId;
                rtninfo.IsSuccess = rfqDA.SaveOrUpdateRFQFeedback(QuotationFeedbackInfo, out OutId, out itemId);

                if (rtninfo.IsSuccess)
                {
                    if (QuotationFeedbackInfo.RFQSupplierId == 0)
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
                        OwnerIdForDocuments = QuotationFeedbackInfo.RFQSupplierId;
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
                    
                    Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(hfRandom));

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return rtninfo;
        }

        
    }

}

