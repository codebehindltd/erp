using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using System.Web.Services;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class frmInboardED : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserInformationBOSession"] != null)
            {
                if (!IsPostBack)
                {
                    this.pnlInnboardCheckingInformation.Visible = true;
                    this.pnlInnboardDetailInformation.Visible = false;
                }
            }
            else
            {
                Session["UserInformationBOSession"] = null;
                Response.Redirect("/Login.aspx");
            }
        }

        protected void btnInnboardHeadCheck_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            UserInformationDA userInformationDA = new UserInformationDA();
            //superadminInnb0@rd!nnb0@rdERP@2010
            string enteredValue = this.txtInnboardHead.Text;
            if (!string.IsNullOrWhiteSpace(this.txtInnboardHead.Text))
            {
                if (this.txtInnboardHead.Text.Length == 34)
                {
                    string[] separators = { "Innb0@rd" };
                    string[] wordList = enteredValue.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    userInformationBO.UserId = wordList[0];
                    userInformationBO.UserPassword = wordList[1];

                    UserInformationBO userInformation = userInformationDA.GetUserInformationByUserNameNId(userInformationBO.UserId, userInformationBO.UserPassword);
                    if (userInformation != null)
                    {
                        if (userInformation.UserInfoId > 0)
                        {
                            this.pnlInnboardCheckingInformation.Visible = false;
                            this.pnlInnboardDetailInformation.Visible = true;
                        }
                    }
                }
                else
                {
                    Session["UserInformationBOSession"] = null;
                    Response.Redirect("/Login.aspx");
                }
            }
            else
            {
                Session["UserInformationBOSession"] = null;
                Response.Redirect("/Login.aspx");
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.txtHeadName.Text))
            {
                if (ddlType.SelectedValue == "CS")
                {
                    if (this.ddlActiveStat.SelectedValue == "DE")
                    {
                        this.txtHeadValue.Text = CryptographyHelper.Encrypt(this.txtHeadName.Text);
                    }
                    else if (this.ddlActiveStat.SelectedValue == "ED")
                    {
                        this.txtHeadValue.Text = CryptographyHelper.Decrypt(this.txtHeadName.Text);
                    }
                    else if (this.ddlActiveStat.SelectedValue == "Clear")
                    {
                        this.txtHeadName.Text = string.Empty;
                        this.ddlActiveStat.SelectedIndex = 0;
                        this.txtHeadValue.Text = string.Empty;
                    }
                }
                else if (ddlType.SelectedValue == "ED")
                {
                    EncryptionHelper encryptionHelper = new EncryptionHelper();
                    if (this.ddlActiveStat.SelectedValue == "DE")
                    {
                        this.txtHeadValue.Text = encryptionHelper.Encrypt(this.txtHeadName.Text);
                        if (ddlIsExipreDateUpdate.SelectedValue == "1")
                        {
                            if (!string.IsNullOrWhiteSpace(txtHeadValue.Text))
                            {
                                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                                Boolean status = commonSetupDA.UpdateSoftwareExpireDate(txtHeadValue.Text);
                            }
                        }
                    }
                    else if (this.ddlActiveStat.SelectedValue == "ED")
                    {
                        this.txtHeadValue.Text = encryptionHelper.Decrypt(this.txtHeadName.Text);
                    }
                    else if (this.ddlActiveStat.SelectedValue == "Clear")
                    {
                        this.txtHeadName.Text = string.Empty;
                        this.ddlActiveStat.SelectedIndex = 0;
                        this.txtHeadValue.Text = string.Empty;
                    }
                }
            }
        }
    }
}