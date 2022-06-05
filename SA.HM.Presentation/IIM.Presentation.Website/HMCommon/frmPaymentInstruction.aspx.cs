using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;
using System.Web.Services;
using System.Text;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class frmPaymentInstruction : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                FileUpload();
                //LoadInstructions();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                CurrentUserId.Value = userInformationBO.UserInfoId.ToString();
            }
            
        }
        
        [WebMethod()]
        public static string SaveInstructions(string DocumentId, string OwnerId, string ImagePath, string Instruction, string UserId)
        {
            try
            {
                int docId = 0, ownId = 0, usrId = 0;

                int.TryParse(DocumentId, out docId);
                int.TryParse(OwnerId, out ownId);
                int.TryParse(UserId, out usrId);

                DocumentsDA docDa = new DocumentsDA();
                bool IsSaved = docDa.SavePaymentInstructionInfo(docId, ownId, ImagePath, Instruction, usrId);
                if (IsSaved)
                    return "Payment Instruction Saved Successfully!";
                else
                    return "Instruction Save Failed!";
            }
            catch(Exception exp)
            {
                return "Save Failed!";
            }
        }

        [WebMethod()]
        public static string UpdateInstructions(string DocumentId, string OwnerId, string ImagePath, string Instruction, string UserId)
        {
            try
            {
                int docId = 0, ownId = 0, usrId = 0;

                int.TryParse(DocumentId, out docId);
                int.TryParse(OwnerId, out ownId);
                int.TryParse(UserId, out usrId);

                DocumentsDA docDa = new DocumentsDA();
                bool IsSaved = docDa.UpdatePaymentInstructionInfo(docId, ownId, ImagePath, Instruction, usrId);
                if (IsSaved)
                    return "Payment Instruction Saved Successfully!";
                else
                    return "Instruction Save Failed!";
            }
            catch (Exception exp)
            {
                return "Save Failed!";
            }
        }

        [WebMethod()]
        public static string DeleteInstructions(string DocumentId)
        {
            try
            {
                int docId = 0;

                int.TryParse(DocumentId, out docId);

                DocumentsDA docDa = new DocumentsDA();
                bool IsDeleted = docDa.DeletePaymentInstructionInfo(docId);

                if (IsDeleted)
                    return "Instruction Deleted Successfully.";
                else
                    return "Failed to delete instruction!";
            }
            catch(Exception exp)
            {
                return "Delete Failed";
            }
        }

        [WebMethod()]
        public static DocumentsBO GetInstructions(string DocumentId)
        {
            try
            {
                int docId = 0;

                int.TryParse(DocumentId, out docId);

                DocumentsDA docDa = new DocumentsDA();
                DocumentsBO docBO = docDa.GetDocumentsInfoByDocumentId(docId);

                return docBO;
            }
            catch (Exception exp)
            {
                return null;
            }
        }

        
        [WebMethod]
        public static int ChangeRandomId()
        {
            Random rd = new Random();
            int randomId = rd.Next(100000, 999999);
            HttpContext.Current.Session["SupplierDocId"] = randomId;
            return randomId;
        }
        
        [WebMethod]
        public static List<DocumentsBO> GetPaymentInstructionsByWebMethod()
        {
            try
            {
                List<DocumentsBO> docList = new List<DocumentsBO>();
                DocumentsDA docDA = new DocumentsDA();

                docList = docDA.GetPaymentInstuctionInfo();
                docList = new HMCommonDA().GetDocumentListWithIcon(docList);

                docList = docList.OrderByDescending(Obj => Obj.DocumentId).ToList();

                StringBuilder strBuilder = new StringBuilder();

                strBuilder.Append("<table id = 'paymentInstructionList' style = 'width:100%' class='table table-bordered table-condensed table-responsive'>");
                strBuilder.Append("<thead>");
                strBuilder.Append("<tr style = 'color: White; background-color: #44545E; font-weight: bold;' >");
                strBuilder.Append("<th align='left' scope='col' style='width: 10%'>Payment Gateway Company Logo</th>");
                strBuilder.Append("<th align = 'left' scope= 'col' style= 'width: 70%'> Payment Instruction</th>");
                strBuilder.Append("<th align = 'left' scope= 'col' style= 'width: 10%'> Action </th >");
                strBuilder.Append("</tr >");
                strBuilder.Append("</thead>");


                string host = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;

                int index = 0;

                strBuilder.Append("<tbody>");
                foreach (DocumentsBO docBo in docList)
                {
                    if (index % 2 == 0)
                    {
                        strBuilder.Append("<tr id='trdoc" + index + "' style='background-color:#E3EAEB;'>");
                    }
                    else
                    {
                        strBuilder.Append("<tr id = 'trdoc" + index + "' style = 'background-color:White;' > ");
                    }

                    string imagePath = "";
                    if (docBo.Path != "")
                    {
                        imagePath = "<img src='" + host + "/" + docBo.Path + docBo.Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                        
                    }
                    else
                        imagePath = "";


                    strBuilder.Append("<td align='left' style='width: 10%'>" + imagePath + "</td>");
                    strBuilder.Append("<td align='left' style='width: 70%'>" + docBo.Instruction + "</td>");

                    strBuilder.Append("<td align='left' style='width: 10%'>");
                    strBuilder.Append("&nbsp;<img src='../Images/edit.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return EditDoc('" + docBo.DocumentId + "', '" + index + "')\" alt='Edit Information' border='0' />");
                    strBuilder.Append("&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteDoc('" + docBo.DocumentId + "', '" + index + "')\" alt='Delete Information' border='0' />");
                    strBuilder.Append("</td>");
                    strBuilder.Append("</tr>");

                    index++;
                }
                strBuilder.Append("</tbody>");
                strBuilder.Append("</table >");

                string strTable = strBuilder.ToString();

                //return strTable;
                return docList;
            }
            catch (Exception exp)
            {
                return null;
            }
        }

        [WebMethod]
        public static string GetUploadedDocByWebMethod(int randomId)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("PaymentInstruction", randomId);
            
            foreach (DocumentsBO dc in docList)
            {

                if (dc.DocumentType == "Image")
                    dc.Path = (dc.Path + dc.Name);

                dc.Name = dc.Name.Remove(dc.Name.LastIndexOf('.'));
            }
            docList = new HMCommonDA().GetDocumentListWithIcon(docList).ToList();
            if(docList.Count > 0)
            {
                string host = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;
                
                DocumentsBO docBo = docList[0];

                string imagePath = host + "/" + docBo.Path;

                string strImage = "<input type=\"hidden\" id=\"documentId\" name=\"documentId\" value=\""+ docBo.DocumentId +"\">";
                strImage += "<input type=\"hidden\" id=\"ownerId\" name=\"ownerId\" value=\""+ docBo.OwnerId + "\">";
                strImage += "<img src='" + imagePath + "' style='width: 100px; height: 100px;' alt='Company Logo'/>";
                strImage += "<input type=\"hidden\" id=\"ImagePath\" name=\"ImagePath\" value=\"" + imagePath + "\">";

                return strImage;
            }
            else
            {
                return "";
            }
        }
        private void FileUpload()
        {
            Random rd = new Random();
            int seatingId = rd.Next(100000, 999999);
            RandomDocId.Value = seatingId.ToString();
            
            HttpContext.Current.Session["SupplierDocId"] = RandomDocId.Value;
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
        }
        private void CheckObjectPermission()
        {
            //btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                
            }
        }
        
    }
}