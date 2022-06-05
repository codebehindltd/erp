using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HouseKeeping;
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
    public partial class HKLostFoundReturnIframe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FileUpload();
            }
        }
        private void FileUpload()
        {
            Random rd = new Random();
            int seatingId = rd.Next(100000, 999999);
            RandomDocId.Value = seatingId.ToString();
            tempDocId.Value = seatingId.ToString();
            HttpContext.Current.Session["LostItemReturnDocId"] = RandomDocId.Value;
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            flashUpload.QueryParameters = "LostItemReturnDocId=" + Server.UrlEncode(RandomDocId.Value);
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
        public static ReturnInfo PerformReturnUpdate(int id, string returnDate, string returnDescription, string whomToReturn, int hfRandom, string deletedDocument)
        {
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                ReturnInfo rtninfo = new ReturnInfo();
                rtninfo.IsSuccess = false;
                HMUtility hmUtility = new HMUtility();
                long OwnerIdForDocuments = 0;
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                LostFoundDA foundDA = new LostFoundDA();
                LostFoundBO lostFoundBO = new LostFoundBO();


                if (returnDate != "")
                {
                    lostFoundBO.ReturnDate = hmUtility.GetDateTimeFromString(returnDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                }
                if (id > 0)
                {
                    lostFoundBO.Id = id;
                }
                lostFoundBO.WhomToReturn = whomToReturn;
                lostFoundBO.ReturnDescription = returnDescription;
                lostFoundBO.HasItemReturned = true;
                lostFoundBO.LastModifiedBy = userInformationBO.UserInfoId;

                rtninfo.IsSuccess = foundDA.PerformReturn(lostFoundBO);
                if (rtninfo.IsSuccess)
                {
                    {
                        OwnerIdForDocuments = lostFoundBO.Id;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo("Lost item has returned successfully.", AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity("Lost item has returned",
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
                return rtninfo;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
            
        }
        [WebMethod]
        public static List<DocumentsBO> GetUploadedDocByWebMethod(int id, int randomId, string deletedDoc)
        {
            //List<int> delete = new List<int>();
            //if (!(String.IsNullOrEmpty(deletedDoc)))
            //{
            //    delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            //}
            //List<DocumentsBO> docList = new List<DocumentsBO>();
            //docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("LostItemReturnDocuments", randomId);
            //if (id > 0)
            //    docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("LostItemReturnDocuments", (int)id));

            //docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));
            //foreach (DocumentsBO dc in docList)
            //{

            //    if (dc.DocumentType == "Image")
            //        dc.Path = (dc.Path + dc.Name);

            //    dc.Name = dc.Name.Remove(dc.Name.LastIndexOf('.'));
            //}
            //docList = new HMCommonDA().GetDocumentListWithIcon(docList).ToList();
            //return docList;
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }

            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("LostItemReturnDocuments", randomId);

            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("LostItemReturnDocuments", id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));

            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }
    }
}