using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data;
using HotelManagement.Entity;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using System.Web.Services;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Newtonsoft.Json;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Restaurant;


namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmMachineTest : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
            if (!string.IsNullOrWhiteSpace(DeleteSuccess))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
            }

            if (!IsPostBack)
            {
                this.LoadCostCenter();
                this.CheckObjectPermission(); 
            }
        }
        protected void gvGuestHouseService_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.CheckObjectPermission();
        }
        protected void gvGuestHouseService_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Visible = isSavePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvGuestHouseService_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int BankId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(BankId);
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("PPumpMachineTest", "TestId", BankId);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                }

                this.SetTab("SearchTab");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtTestDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Test Date.", AlertType.Warning);
                this.txtTestDate.Focus();
            }
            else if (this.ddlCostCenter.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Machine Name.", AlertType.Warning);
                this.ddlCostCenter.Focus();
            }
            else if (string.IsNullOrWhiteSpace(this.txtTestQuantity.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Test Quantity.", AlertType.Warning);
                this.txtTestQuantity.Focus();
            }
            else
            {
                MachineTestBO entityBO = new MachineTestBO();
                RestaurentBillDA entityDA = new RestaurentBillDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                entityBO.TestDate = hmUtility.GetDateTimeFromString(txtTestDate.Text, userInformationBO.ServerDateFormat);
                entityBO.MachineId = Convert.ToInt32(this.ddlCostCenter.SelectedValue);
                entityBO.TestQuantity = Convert.ToDecimal(this.txtTestQuantity.Text);
                entityBO.Remarks = this.txtRemarks.Text;

                if (string.IsNullOrWhiteSpace(txtTestId.Value))
                {
                    int tmpPkId = 0;
                    entityBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = entityDA.SaveMachineTestInfo(entityBO, out tmpPkId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.MachineTest.ToString(), tmpPkId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.MachineTest));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        this.Cancel();
                    }
                }
                //else
                //{
                //    entityBO.TestId = Convert.ToInt32(txtTestId.Value);
                //    entityBO.LastModifiedBy = userInformationBO.UserInfoId;
                //    Boolean status = entityDA.UpdateBankInfo(entityBO);
                //    if (status)
                //    {
                //        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                //        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.MachineTest.ToString(), entityBO.TestId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.MachineTest));
                //        this.Cancel();
                //    }
                //}

                this.SetTab("EntryTab");
            }
        }
        //************************ User Defined Function ********************//
        private void Cancel()
        {
            this.ddlCostCenter.SelectedIndex = 0;
            this.txtTestDate.Text = string.Empty;
            this.txtTestQuantity.Text = string.Empty;
            this.txtRemarks.Text = string.Empty;
            this.btnSave.Text = "Save";
            this.txtTestId.Value = string.Empty;
            this.ddlCostCenter.Focus();
        }
        public void LoadCostCenter()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            var List = costCentreTabDA.GetCostCentreTabInfo().Where(x => x.CostCenterType == "PetrolPump").ToList();
            this.ddlCostCenter.DataSource = List;
            this.ddlCostCenter.DataTextField = "CostCenter";
            this.ddlCostCenter.DataValueField = "CostCenterId";
            this.ddlCostCenter.DataBind();

            this.ddlSCostCenter.DataSource = List;
            this.ddlSCostCenter.DataTextField = "CostCenter";
            this.ddlSCostCenter.DataValueField = "CostCenterId";
            this.ddlSCostCenter.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCostCenter.Items.Insert(0, item);
            this.ddlSCostCenter.Items.Insert(0, item);
        }
        private void CheckObjectPermission()
        {
            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            //ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            //objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmMachineTest.ToString());
            //isSavePermission = objectPermissionBO.IsSavePermission;
            //isDeletePermission = objectPermissionBO.IsDeletePermission;
            //btnSave.Visible = isSavePermission;
            //if (!isSavePermission)
            //{
            //    isNewAddButtonEnable = -1;
            //}

            //if (isSavePermission)
            //{
            //    IsBankSavePermission.Value = "1";
            //}
            //else
            //{
            //    IsBankSavePermission.Value = "0";
            //}

            //if (isDeletePermission)
            //{
            //    IsBankDeletePermission.Value = "1";
            //}
            //else
            //{
            //    IsBankDeletePermission.Value = "0";
            //}

        }
        public void FillForm(int EditId)
        {
            //BankBO bankBO = new BankBO();
            //BankDA bankDA = new BankDA();
            //bankBO = bankDA.GetBankInfoById(EditId);
            //ddlActiveStat.SelectedValue = (bankBO.ActiveStat == true ? 0 : 1).ToString();
            //txtBankId.Value = bankBO.BankId.ToString();
            //txtBankName.Text = bankBO.BankName.ToString();
        }
        private void SetTab(string TabName)
        {
            if (TabName == "SearchTab")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "EntryTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static ReturnInfo DeleteData(int sEmpId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("CommonBank", "BankId", sEmpId);

                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.Bank.ToString(), sEmpId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Bank));
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rtninf;
        }
        [WebMethod]
        public static string LoadBreadCrumbsInformation()
        {
            string breadCrumbs = string.Empty;
            breadCrumbs = "<span class='divider'>/</span><a href='/HMCommon/frmBank.aspx'>Bank</a><span class='divider'></span>";
            return breadCrumbs;
        }
        [WebMethod]
        public static GridViewDataNPaging<MachineTestBO, GridPaging> SearchMachineTestAndLoadGridInformation(int costCenterId, string fromDate, string toDate, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                FromDate = hmUtility.GetDateTimeFromString(fromDate, userInformationBO.ServerDateFormat);
            }
            if (!string.IsNullOrWhiteSpace(toDate))
            {
                ToDate = hmUtility.GetDateTimeFromString(toDate, userInformationBO.ServerDateFormat);
            }

            GridViewDataNPaging<MachineTestBO, GridPaging> myGridData = new GridViewDataNPaging<MachineTestBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            RestaurentBillDA bankDA = new RestaurentBillDA();
            List<MachineTestBO> bankInfoList = new List<MachineTestBO>();
            bankInfoList = bankDA.GetMachineTestInformationBySearchCriteriaForPaging(costCenterId, FromDate, ToDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            //List<BankBO> distinctItems = new List<BankBO>();
            //distinctItems = bankInfoList.GroupBy(test => test.BankName).Select(group => group.First()).ToList();
            //distinctItems = bankInfoList.GroupBy(test => test.BankName).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(bankInfoList, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static BankBO LoadDetailInformation(int bankId)
        {
            BankDA bankDA = new BankDA();
            return bankDA.GetBankInfoById(bankId);
        }
    }
}