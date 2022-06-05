using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HMCommon.UserControl
{
    public partial class CompanyProjectUserControlVertical : System.Web.UI.UserControl
    {
        HMUtility hmUtility = new HMUtility();
        public string ConnfigurationID { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckIntegrateWithAccounts();
            }

        }
        private void CheckIntegrateWithAccounts()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO bo = new HMCommonSetupBO();
            bo = commonSetupDA.GetCommonConfigurationInfo(ConnfigurationID, ConnfigurationID);

            if (bo.ActiveStat != false)
            {
                if (bo.SetupValue == "1")
                {
                    LoadGLCompany();
                }
                else
                    LoadDefaultGLCompanyNProject();
            }
            else
            {
                LoadGLCompany();
            }
        }

        private void LoadDefaultGLCompanyNProject()
        {
            GLCompanyDA entityDA = new GLCompanyDA();
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            companyList.Add(entityDA.GetAllGLCompanyInfo().FirstOrDefault());

            ddlGLCompany.DataSource = companyList;
            ddlGLCompany.DataTextField = "Name";
            ddlGLCompany.DataValueField = "CompanyId";
            ddlGLCompany.DataBind();

            hfCompanyAll.Value = JsonConvert.SerializeObject(companyList);

            hfIsSingle.Value = "1";
            if (companyList.Count > 0)
            {
                GLProjectDA projectDA = new GLProjectDA();
                List<GLProjectBO> projectList = new List<GLProjectBO>();
                projectList.Add(projectDA.GetGLProjectInfoByGLCompany(companyList[0].CompanyId).FirstOrDefault());

                ddlGLProject.DataSource = projectList;
                ddlGLProject.DataTextField = "Name";
                ddlGLProject.DataValueField = "ProjectId";
                ddlGLProject.DataBind();
            }
        }

        private void LoadGLCompany()
        {
            hfIsSingle.Value = "0";
            GLCompanyDA entityDA = new GLCompanyDA();
            //var List = entityDA.GetAllGLCompanyInfoByUserGroupId(userGroupId);
            List<GLCompanyBO> GLCompanyBOList = new List<GLCompanyBO>();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO.UserInfoId == 1)
            {
                GLCompanyBOList = entityDA.GetAllGLCompanyInfo();
            }
            else
            {
                GLCompanyBOList = entityDA.GetAllGLCompanyInfoByUserGroupId(userInformationBO.UserGroupId);
            }

            hfCompanyAll.Value = JsonConvert.SerializeObject(GLCompanyBOList);

            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            if (GLCompanyBOList.Count == 1)
            {
                companyList.Add(GLCompanyBOList[0]);
                ddlGLCompany.DataSource = companyList;
                ddlGLCompany.DataTextField = "Name";
                ddlGLCompany.DataValueField = "CompanyId";
                ddlGLCompany.DataBind();
                hfGLCompanyId.Value = GLCompanyBOList[0].CompanyId.ToString();
                LoadGLProjectByCompany(companyList[0].CompanyId);
            }
            else
            {
                ddlGLCompany.DataSource = GLCompanyBOList;
                ddlGLCompany.DataTextField = "Name";
                ddlGLCompany.DataValueField = "CompanyId";
                ddlGLCompany.DataBind();
                System.Web.UI.WebControls.ListItem itemCompany = new System.Web.UI.WebControls.ListItem();
                itemCompany.Value = "0";
                if (hfDropdownFirstValue.Value == "all")
                {
                    itemCompany.Text = hmUtility.GetDropDownFirstAllValue();
                }
                else
                {
                    itemCompany.Text = hmUtility.GetDropDownFirstValue();
                }
                ddlGLCompany.Items.Insert(0, itemCompany);
            }
        }
        private void LoadGLProjectByCompany(int companyId)
        {
            GLProjectDA entityDA = new GLProjectDA();
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            var List = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(Convert.ToInt32(companyId), Convert.ToInt32(userInformationBO.UserGroupId)).Where(x => x.IsFinalStage == false).ToList();
            
            ddlGLProject.DataSource = List;
            ddlGLProject.DataTextField = "Name";
            ddlGLProject.DataValueField = "ProjectId";
            ddlGLProject.DataBind();

            if (List.Count > 1)
            {
                hfIsSingle.Value = "0";
                System.Web.UI.WebControls.ListItem itemProject = new System.Web.UI.WebControls.ListItem();
                itemProject.Value = "0";
                itemProject.Text = hmUtility.GetDropDownFirstAllValue();
                ddlGLProject.Items.Insert(0, itemProject);
            }
            else
            {
                hfIsSingle.Value = "1";
                if (List.Count == 1)
                    hfGLProjectId.Value = List[0].ProjectId.ToString();
            }
        }
    }
}