using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft;
using Newtonsoft.Json;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class frmFiscalYearClosing : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected bool isSingle = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                isSingle = hmUtility.GetSingleProjectAndCompany();
                if (!IsPostBack)
                {
                    if (isSingle == true)
                    {
                        LoadSingleProjectAndCompany();
                    }
                    else
                    {
                        LoadGLCompany(false);
                        LoadGLProject(false);
                    }
                    LoadCommonDropDownHiddenField();
                    LoadFiscalYear();
                    LoadGLDonor();
                }
            }
        }

        private void LoadGLCompany(bool isSingle)
        {
            GLCompanyDA entityDA = new GLCompanyDA();
            var List = entityDA.GetAllGLCompanyInfo();

            hfCompanyAll.Value = JsonConvert.SerializeObject(List);

            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            if (isSingle == true)
            {
                companyList.Add(List[0]);
                ddlGLCompany.DataSource = companyList;
                ddlGLCompany.DataTextField = "Name";
                ddlGLCompany.DataValueField = "CompanyId";
                ddlGLCompany.DataBind();
            }
            else
            {
                ddlGLCompany.DataSource = List;
                ddlGLCompany.DataTextField = "Name";
                ddlGLCompany.DataValueField = "CompanyId";
                ddlGLCompany.DataBind();
                System.Web.UI.WebControls.ListItem itemCompany = new System.Web.UI.WebControls.ListItem();
                itemCompany.Value = "0";
                itemCompany.Text = hmUtility.GetDropDownFirstValue();
                ddlGLCompany.Items.Insert(0, itemCompany);
            }
        }
        private void LoadGLProject(bool isSingle)
        {
            GLProjectDA entityDA = new GLProjectDA();
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            var List = entityDA.GetAllGLProjectInfo();
            if (isSingle == true)
            {
                projectList.Add(List[0]);
                ddlGLProject.DataSource = projectList;
                ddlGLProject.DataTextField = "Name";
                ddlGLProject.DataValueField = "ProjectId";
                ddlGLProject.DataBind();
            }
            else
            {
                ddlGLProject.DataSource = List;
                ddlGLProject.DataTextField = "Name";
                ddlGLProject.DataValueField = "ProjectId";
                ddlGLProject.DataBind();

                System.Web.UI.WebControls.ListItem itemProject = new System.Web.UI.WebControls.ListItem();
                itemProject.Value = "0";
                itemProject.Text = hmUtility.GetDropDownFirstValue();
                ddlGLProject.Items.Insert(0, itemProject);
            }
            SingleprojectId.Value = List[0].ProjectId.ToString();
        }
        private void LoadGLProject(string companyId)
        {
            GLProjectDA entityDA = new GLProjectDA();
            ddlGLProject.DataSource = entityDA.GetGLProjectInfoByGLCompany(Convert.ToInt32(companyId));
            ddlGLProject.DataTextField = "Name";
            ddlGLProject.DataValueField = "ProjectId";
            ddlGLProject.DataBind();

            System.Web.UI.WebControls.ListItem itemProject = new System.Web.UI.WebControls.ListItem();
            itemProject.Value = "0";
            itemProject.Text = hmUtility.GetDropDownFirstValue();
            ddlGLProject.Items.Insert(0, itemProject);
        }
        private void LoadGLDonor()
        {
            GLDonorDA entityDA = new GLDonorDA();
            List<GLDonorBO> donorList = new List<GLDonorBO>();
            donorList = entityDA.GetAllGLDonorInfo();

            ddlDonor.DataSource = donorList;
            ddlDonor.DataTextField = "Name";
            ddlDonor.DataValueField = "DonorId";
            ddlDonor.DataBind();

            System.Web.UI.WebControls.ListItem itemDonor = new System.Web.UI.WebControls.ListItem();
            itemDonor.Value = "0";
            itemDonor.Text = hmUtility.GetDropDownFirstValue();
            ddlDonor.Items.Insert(0, itemDonor);

        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadSingleProjectAndCompany()
        {
            LoadGLCompany(true);
            LoadGLProject(true);
        }
        private void LoadFiscalYear()
        {
            GLFiscalYearDA fiscalYear = new GLFiscalYearDA();

            ddlFiscalYear.DataSource = fiscalYear.GetAllFiscalYear();
            ddlFiscalYear.DataTextField = "FiscalYearName";
            ddlFiscalYear.DataValueField = "FiscalYearId";
            ddlFiscalYear.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlFiscalYear.Items.Insert(0, item);
        }
        protected void btinFiscalYearCLosing_Click(object sender, EventArgs e)
        {
            int fiscalYearId = 0;
            bool status = false;

            try
            {
                if (ddlFiscalYear.SelectedValue != "")
                {
                    fiscalYearId = Convert.ToInt32(ddlFiscalYear.SelectedValue);
                }

                int companyId = 0, projectId = 0, donorId = 0;

                if (ddlGLCompany.SelectedValue != "0" || ddlGLCompany.SelectedValue != "")
                {
                    companyId = Convert.ToInt32(ddlGLCompany.SelectedValue);
                }

                if (ddlGLProject.SelectedValue != "0" || ddlGLProject.SelectedValue != "")
                {
                    projectId = Convert.ToInt32(ddlGLProject.SelectedValue);
                }

                if (ddlDonor.SelectedValue != "0" || ddlDonor.SelectedValue != "")
                {
                    donorId = Convert.ToInt32(ddlDonor.SelectedValue);
                }

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                GLFiscalYearDA fiscalYear = new GLFiscalYearDA();
                status = fiscalYear.FiscalYearClosingBalanceProcessing(fiscalYearId, companyId, projectId, donorId, userInformationBO.UserInfoId);

                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Closing Balance Process Operation Successfull.", AlertType.Success);
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, "Closing Balance Process Operation Failed.", AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, ex.Message.ToString(), AlertType.Error);
            }
        }
    }
}