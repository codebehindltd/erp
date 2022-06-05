using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System.Collections;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using System.IO;

namespace HotelManagement.Presentation.Website.SalesManagment
{
    public partial class frmSalesContractDetails : System.Web.UI.Page
    {
        int UserId = 1;
        ArrayList arrayDelete;
        protected int _OrderId;
        protected int isMessageBoxEnable = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int RegistrationId;
                Random rd = new Random();
                RegistrationId = rd.Next(100000, 999999);
                RandomOwnerId.Value = RegistrationId.ToString();
                LoadCustomer();
            }
            string jscript = "function UploadComplete(){};";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            flashUpload.QueryParameters = "CustomerId=" + Server.UrlEncode(Convert.ToString(RandomOwnerId.Value));
        }

        private void LoadCustomer()
        {
            SalesCustomerDA salesCustomerDA = new SalesCustomerDA();
            List<SalesCustomerBO> customerList = new List<SalesCustomerBO>();
            customerList = salesCustomerDA.GetAllSalesCustomerInfo();
            ddlCustomerId.DataSource = customerList;
            ddlCustomerId.DataTextField = "Name";
            ddlCustomerId.DataValueField = "CustomerId";
            ddlCustomerId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCustomerId.Items.Insert(0, item);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (!isFormValid())
            {
                return;
            }
            SalesContractDetailsBO contactBO = new SalesContractDetailsBO();
            SalesContractDetailsDA contactDA = new SalesContractDetailsDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            contactBO.SigningDate = hmUtility.GetDateTimeFromString(txtSigningDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            contactBO.ExpiryDate = hmUtility.GetDateTimeFromString(txtExpiryDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            contactBO.CustomerId = Int32.Parse(ddlCustomerId.SelectedValue);
            contactBO.tempContractDetailsId = Int32.Parse(RandomOwnerId.Value);
            if (this.btnSaveCompany.Text.Equals("Save"))
            {
                int tmpOrderId = 0;
                contactBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = contactDA.SaveSalesContractDetailsInfo(contactBO, out tmpOrderId);
                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Saved Operation Successfull";
                    this.Cancel();
                }
            }
            else
            {
                contactBO.ContractDetailsId = Convert.ToInt32(Session["_OrderId"]);
                contactBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = contactDA.UpdateSalesContractDetailsInfo(contactBO);
                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Update Operation Successfull";
                    this.Cancel();
                }
            }


        }

        private void Cancel()
        {

            ddlCustomerId.SelectedIndex = 0;
            txtExpiryDate.Text = "";
            txtSigningDate.Text = "";

        }

        private bool isFormValid()
        {
            bool status = true;
            if (ddlCustomerId.SelectedIndex == 0)
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please Select Customer Name.";
                this.ddlCustomerId.Focus();
                status = false;
            }
            else if (string.IsNullOrEmpty(txtSigningDate.Text))
            {
                this.isMessageBoxEnable = 1;
                this.txtSigningDate.Focus();
                this.lblMessage.Text = "Please Enter Signing Date.";
                status = false;
            }
            else if (string.IsNullOrEmpty(txtExpiryDate.Text))
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please Enter Expiry Date.";
                this.txtExpiryDate.Focus();
                status = false;
            }
            return status;
        }

        
        [WebMethod]
        public static string GetDocumentsByUserTypeAndUserId(string GuestId)
        {
            string UserType = "";
            int UserId = 0;
            List<DocumentsBO> docList = new List<DocumentsBO>();
            DocumentsDA docDA = new DocumentsDA();
            docList = docDA.GetDocumentsByUserTypeAndUserId("Sales Contact Details", Int32.Parse(GuestId));
            docList = new HMCommonDA().GetDocumentListWithIcon(docList);
            string strTable = "";
            strTable += "<div style='color:#ffffff; background-color: #44545E;width:750px;'>";
            int counter = 0;
            foreach (DocumentsBO dr in docList)
            {
                if (dr.Extention == ".jpg")
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:250px; height:250px; float:left;padding:30px'>";
                    strTable += "<a target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img id= style='width: 100px; height: 100px;' src='" + ImgSource + "'  alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div> <div class=\"divClear\">  </div>";
                    strTable += " <div style=' width:20px; height:20px; float:left;padding:5px' id=\"deleteImage\"> <a href=\"javascript:void()\" title='Delete This Image' onclick=\"return DeleteImage('" + GuestId + "');\">  <img src='../Images/delete.png' alt=\"delte image\" /></a> </div>";
                }
                else
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:100px; height:100px; float:left;padding:30px'>";
                    strTable += "<a target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img id= style='width: 100px; height: 100px;' src='" + dr.IconImage + "' alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
            }
            strTable += "</div>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
            }
            return strTable;

        }

        [WebMethod]
        public static string DeleteCustomerImage(int GuestId)
        {
            string message = "Delete Operation Successfull";

            List<DocumentsBO> docList = new List<DocumentsBO>();
            DocumentsDA docDA = new DocumentsDA();
            docList = docDA.GetDocumentsByUserTypeAndUserId("Sales Contact Details", GuestId);

            docDA.DeleteDocumentsByDocumentTypeNOwnerId(docList, GuestId);

            return message;
        }
    }
}