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

namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class frmNodeOpeningBalance : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCurrency();
                LoadGLProject();
                LoadChartofAccounts();
            }
        }

        private void LoadCurrency()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();

            fields = commonDA.GetCustomField("Currency", hmUtility.GetDropDownFirstValue());
            if (fields != null)
            {
                if (fields.Count > 1)
                {
                    fields.RemoveAt(0);
                }
            }

            this.ddlCurrency.DataSource = fields;
            this.ddlCurrency.DataTextField = "FieldValue";
            this.ddlCurrency.DataValueField = "FieldId";
            this.ddlCurrency.DataBind();
        }

        private void LoadGLProject()
        {
            GLProjectDA entityDA = new GLProjectDA();
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            var List = entityDA.GetAllGLProjectInfo();

            if (List.Count == 1)
            {
                projectList.Add(List[0]);
                this.ddlGLProject.DataSource = projectList;
                this.ddlGLProject.DataTextField = "Name";
                this.ddlGLProject.DataValueField = "ProjectId";
                this.ddlGLProject.DataBind();
            }
            else
            {
                this.ddlGLProject.DataSource = List;
                this.ddlGLProject.DataTextField = "Name";
                this.ddlGLProject.DataValueField = "ProjectId";
                this.ddlGLProject.DataBind();

                ListItem itemProject = new ListItem();
                itemProject.Value = "0";
                itemProject.Text = hmUtility.GetDropDownFirstValue();
                this.ddlGLProject.Items.Insert(0, itemProject);
            }
        }

        private void LoadChartofAccounts()
        {
            NodeMatrixDA nmda = new NodeMatrixDA();
            List<NodeMatrixBO> nm = new List<NodeMatrixBO>();

            nm = nmda.GetNodeMatrixInfo();
            hfNodeMatrix.Value = JsonConvert.SerializeObject(nm);
        }

        [WebMethod]
        public static ArrayList AccountHeadForCPCRBPBR(string projectId, string voucherType)
        {
            ArrayList list = new ArrayList();
            List<GLAccountConfigurationBO> entityBOList = new List<GLAccountConfigurationBO>();

            if (!string.IsNullOrEmpty(projectId))
            {
                GLAccountConfigurationDA entityDA = new GLAccountConfigurationDA();
                entityBOList = entityDA.GetAllAccountConfigurationInfoByProjectIdNAccountType(Convert.ToInt32(projectId), voucherType);

                //foreach (GLAccountConfigurationBO acrows in entityBOList)
                //{
                //    NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
                //    NodeMatrixBO nodeMatrixBO = new NodeMatrixBO();

                //    nodeMatrixBO = nodeMatrixDA.GetNodeMatrixInfoById(acrows.NodeId);
                //    if (nodeMatrixBO != null)
                //    {
                //        list.Add(new ListItem(nodeMatrixBO.HeadWithCode.ToString(), nodeMatrixBO.NodeId.ToString()));
                //    }
                //}
            }
            return new ArrayList(entityBOList);
        }      
    }
}