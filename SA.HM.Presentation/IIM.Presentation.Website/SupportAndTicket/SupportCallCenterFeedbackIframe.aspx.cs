using HotelManagement.Data.HMCommon;
using HotelManagement.Data.SupportAndTicket;
using HotelManagement.Entity.HMCommon;
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
    public partial class SupportCallCenterFeedbackIframe : BasePage
    {

        public SupportCallCenterFeedbackIframe() : base("SupportCallInformation")
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                CheckPermission();

                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomProductId.Value = seatingId.ToString();
            }
        }

        private void CheckPermission()
        {

            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }

        [WebMethod]
        public static STSupportBO GetSupportFeedbackById(long id)
        {
            STSupportBO BO = new STSupportBO();
            SupportNCaseSetupDA supportDA = new SupportNCaseSetupDA();

            BO = supportDA.GetSupportFeedbackById(id);

            return BO;
        }

        [WebMethod]
        public static ReturnInfo SaveOrUpdateSupportFeedback(STSupportBO support, string hfRandom, string deletedDocument)
        {
            ReturnInfo info = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            SupportNCaseSetupDA supportDA = new SupportNCaseSetupDA();
            support.CreatedBy = userInformationBO.UserInfoId;
            long id = 0;
            try
            {
                status = supportDA.SaveOrUpdateSupportFeedback(support, out id);


                if (status)
                {
                    info.IsSuccess = true;
                    if (support.Id == 0)
                    {
                        info.PrimaryKeyValue = id.ToString();
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        info.Data = 0;
                    }
                    else
                    {
                        info.PrimaryKeyValue = support.Id.ToString();
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }

                    DocumentsDA documentsDA = new DocumentsDA();
                    if (!string.IsNullOrEmpty(deletedDocument))
                    {
                        bool delete = documentsDA.DeleteDocument(deletedDocument);
                    }

                    HMCommonDA hmCommonDA = new HMCommonDA();
                    Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(id, Convert.ToInt32(hfRandom));
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

        // Documents //
        [WebMethod]
        public static List<DocumentsBO> LoadContactDocument(long id, int randomId, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }

            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("SupportAndTicketFeedbackDoc", randomId);

            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("SupportAndTicketFeedbackDoc", id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));

            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            return docList;
        }
    }
}