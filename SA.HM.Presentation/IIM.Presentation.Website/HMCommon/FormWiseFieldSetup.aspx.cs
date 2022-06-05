using HotelManagement.Entity.Security;
using HotelManagement.Data.HMCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class FormWiseFieldSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFormName();
            }
           
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

        }
        private void LoadFormName()
        {
            HMUtility hmUtility = new HMUtility();
            List<MenuLinksBO> list = new List<MenuLinksBO>();
            FormWiseFieldSetupDA DA = new FormWiseFieldSetupDA();
            list = DA.GetAllFormIdAndName();
            ddlFormName.DataSource = list;
            ddlFormName.DataTextField = "PageName";
            ddlFormName.DataValueField = "MenuLinksId";
            ddlFormName.DataBind();

            list = DA.GetAllMenuLinksBasedOnFieldSetup();
            ddlSearchFormName.DataSource = list;
            ddlSearchFormName.DataTextField = "PageName";
            ddlSearchFormName.DataValueField = "MenuLinksId";
            ddlSearchFormName.DataBind();



            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlFormName.Items.Insert(0, item);
            this.ddlSearchFormName.Items.Insert(0, item);


        }

        [WebMethod]
        public static ReturnInfo SaveFormWiseField(FormWiseFieldSetupBO FormWiseField)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                int _userId = Convert.ToInt32(hmUtility.GetCurrentApplicationUserInfo().UserInfoId);
                FormWiseFieldSetupDA _formWiseFieldSetupDA = new FormWiseFieldSetupDA();
                if (FormWiseField.Id == 0)
                {
                    FormWiseField.CreatedBy = _userId;
                    status = _formWiseFieldSetupDA.SaveFormWiseField(FormWiseField);
                    if (status)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                }
                else
                {

                    FormWiseField.LastModifiedBy = _userId;
                    status = _formWiseFieldSetupDA.UpdateFormWiseFieldInfo(FormWiseField);
                    if (status)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return rtninf;
        }

        [WebMethod]
        public static FormWiseFieldSetupBO EditFormWiseField(int Id)
        {
            FormWiseFieldSetupBO FormWiseField = new FormWiseFieldSetupBO();

            try
            {

                FormWiseFieldSetupDA _formWiseFieldSetupDA = new FormWiseFieldSetupDA();

                FormWiseField = _formWiseFieldSetupDA.GetFormWiseFieldInfoById(Id);

            }
            catch (Exception)
            {

                throw;
            }

            return FormWiseField;
        }
        [WebMethod]
        public static GridViewDataNPaging<FormWiseFieldSetupBO, GridPaging> SearchFormWiseField(int pageId, int gridRecordsCount, int pageNumber,
                                                                                           int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<FormWiseFieldSetupBO, GridPaging> myGridData = new GridViewDataNPaging<FormWiseFieldSetupBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            FormWiseFieldSetupDA DA = new FormWiseFieldSetupDA();
            List<FormWiseFieldSetupBO> FieldList = new List<FormWiseFieldSetupBO>();
            FieldList = DA.GetFormWiseFieldSetupBySearchCriteria(pageId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(FieldList, totalRecords);
            return myGridData;
        }

    }
}