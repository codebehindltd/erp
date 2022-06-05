using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Security;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Security;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class PageWiseMandatoryField : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFormName();
            }

        }
        private void LoadFormName()
        {
            FormWiseFieldSetupDA formWiseFieldSetupDA = new FormWiseFieldSetupDA();
            List<MenuLinksBO> menuLinks = new List<MenuLinksBO>();
            menuLinks = formWiseFieldSetupDA.GetAllMenuLinksBasedOnFieldSetup();

            this.ddlFormName.DataSource = menuLinks;
            this.ddlFormName.DataTextField = "PageName";
            this.ddlFormName.DataValueField = "MenuLinksId";
            this.ddlFormName.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlFormName.Items.Insert(0, item);

        }

        [WebMethod]
        public static List<FormWiseFieldSetupBO> GetFieldsByFormId(int formId)
        {
            FormWiseFieldSetupDA formWiseFieldSetupDA = new FormWiseFieldSetupDA();
            List<FormWiseFieldSetupBO> formWiseFields = new List<FormWiseFieldSetupBO>();
            formWiseFields = formWiseFieldSetupDA.GetFieldsByMenuLinkID(formId);

            return formWiseFields;
        }

        [WebMethod]
        public static ReturnInfo UpdateMandatoryFields(List<UpdatedMandatoryFieldBO> formWiseFieldsList)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {

                DataTable formWiseFieldTable = new DataTable();

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                formWiseFieldTable = formWiseFieldsList.ListToDataTable<UpdatedMandatoryFieldBO>();
                FormWiseFieldSetupDA formWiseFieldSetupDA = new FormWiseFieldSetupDA();
                bool temp = formWiseFieldSetupDA.UpdateIsMandatoryFields(formWiseFieldTable, userInformationBO.UserGroupId);


                if (temp)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                throw ex;
            }
            return rtninf;
        }



    }
}