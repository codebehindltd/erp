using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Security;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class PageWiseActivityLogDetailsSetup : System.Web.UI.Page
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
            menuLinks = formWiseFieldSetupDA.GetAllMenuLinksBasedOnActivityLogSetup();

            ddlFormSetupName.DataSource = menuLinks;
            ddlFormSetupName.DataTextField = "PageName";
            ddlFormSetupName.DataValueField = "pageId";
            ddlFormSetupName.DataBind();

            ddlSearchFormName.DataSource = menuLinks;
            ddlSearchFormName.DataTextField = "PageName";
            ddlSearchFormName.DataValueField = "pageId";
            ddlSearchFormName.DataBind();

            List<MenuLinksBO> list = new List<MenuLinksBO>();
            list = formWiseFieldSetupDA.GetAllFormIdAndName();
            ddlFormName.DataSource = list;
            ddlFormName.DataTextField = "PageName";
            ddlFormName.DataValueField = "PageId";
            ddlFormName.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlFormSetupName.Items.Insert(0, item);
            ddlFormName.Items.Insert(0, item);
            ddlSearchFormName.Items.Insert(0, item);

        }
        [WebMethod]
        public static int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate, string pkId)
        {
            string tableName = "ActivityLogDetailsSetup";
            string pkFieldName = "PageId";
            string pkFieldValue = pkId;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }
        [WebMethod]
        public static List<FormWiseFieldSetupBO> GetFieldsByFormId(string formId)
        {
            FormWiseFieldSetupDA formWiseFieldSetupDA = new FormWiseFieldSetupDA();
            List<FormWiseFieldSetupBO> formWiseFields = new List<FormWiseFieldSetupBO>();
            formWiseFields = formWiseFieldSetupDA.GetFieldsByPageId(formId);

            return formWiseFields;
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
                    status = _formWiseFieldSetupDA.SaveFormWiseFieldForActivityLog(FormWiseField);
                    if (status)
                    {
                        rtninf.IsSuccess = true;
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                }
                else
                {

                    FormWiseField.LastModifiedBy = _userId;
                    status = _formWiseFieldSetupDA.UpdateFormWiseFieldActivityLogSetup(FormWiseField);
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

                FormWiseField = _formWiseFieldSetupDA.GetActivityLogFormWiseFieldInfoById(Id);

            }
            catch (Exception)
            {
                throw;
            }

            return FormWiseField;
        }
        [WebMethod]
        public static ReturnInfo DeleteFormWiseField(int Id)
        {
            FormWiseFieldSetupBO FormWiseField = new FormWiseFieldSetupBO();
            ReturnInfo info = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("ActivityLogDetailsSetup", "Id", Id);
                if (status)
                {
                    info.IsSuccess = true;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    info.IsSuccess = false;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }

            }
            catch (Exception)
            {
                throw;
            }

            return info;
        }

        [WebMethod]
        public static ReturnInfo UpdateSelectedFields(List<FormWiseFieldSetupBO> formWiseFieldsList)
        {
            ReturnInfo rtninf = new ReturnInfo();
            try
            {

                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                FormWiseFieldSetupDA formWiseFieldSetupDA = new FormWiseFieldSetupDA();
                bool temp = formWiseFieldSetupDA.UpdateActivitySaveFields(formWiseFieldsList, userInformationBO.UserInfoId);


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

        [WebMethod]
        public static GridViewDataNPaging<FormWiseFieldSetupBO, GridPaging> SearchFormWiseField(string pageId, int gridRecordsCount, int pageNumber,
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
            FieldList = DA.GetFormWiseFieldsetupForActivityLog(pageId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(FieldList, totalRecords);
            return myGridData;
        }
    }
}