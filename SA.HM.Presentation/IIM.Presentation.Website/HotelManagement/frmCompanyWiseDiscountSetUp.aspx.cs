using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data;
using System.Web.Services;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmCompanyWiseDiscountSetUp : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadCompany();
                LoadRoomType();
                CheckPermission();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsFormValid())
            {
                return;
            }
            else
            {
                CompanyWiseDiscountPolicyBO discountPolicyBO = new CompanyWiseDiscountPolicyBO();
                CompanyWiseDiscountPolicyDA discountPolicyDA = new CompanyWiseDiscountPolicyDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                discountPolicyBO.CompanyId = Convert.ToInt32(ddlCompany.SelectedValue);
                discountPolicyBO.RoomTypeId = Convert.ToInt32(ddlRoomType.SelectedValue);
                discountPolicyBO.DiscountType = ddlDiscountType.SelectedValue;
                discountPolicyBO.DiscountAmount = Convert.ToDecimal(txtDiscountAmount.Text);
                discountPolicyBO.ActiveStat = ddlActiveStatus.SelectedIndex == 0 ? true : false;

                if (string.IsNullOrWhiteSpace(hfDiscountPolicyId.Value))
                {
                    int tmpDiscountPolicyId = 0;
                    discountPolicyBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = discountPolicyDA.SaveDiscountPolicy(discountPolicyBO, out tmpDiscountPolicyId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.DiscountPolicy.ToString(), tmpDiscountPolicyId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DiscountPolicy));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        this.Cancel();
                    }
                }
                else
                {
                    discountPolicyBO.CompanyWiseDiscountId = Convert.ToInt32(hfDiscountPolicyId.Value);
                    discountPolicyBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = discountPolicyDA.UpdateDiscountPolicy(discountPolicyBO);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.DiscountPolicy.ToString(), discountPolicyBO.CompanyWiseDiscountId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DiscountPolicy));
                        this.Cancel();
                    }
                }

                this.SetTab("EntryTab");
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.Cancel();
        }

        //************************ User Defined Function ********************//
        private void CheckPermission()
        {
            hfIsSavePermission.Value = isSavePermission ? "1" : "0";
            hfIsUpdatePermission.Value = isUpdatePermission ? "1" : "0";
            hfIsDeletePermission.Value = isDeletePermission ? "1" : "0";
        }
        private void LoadCompany()
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> files = guestCompanyDA.GetAffiliatedGuestCompanyInfo();
            ddlCompany.DataSource = files;
            ddlCompany.DataTextField = "CompanyName";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();

            ddlSCompany.DataSource = files;
            ddlSCompany.DataTextField = "CompanyName";
            ddlSCompany.DataValueField = "CompanyId";
            ddlSCompany.DataBind();

            ListItem itemReference = new ListItem();
            itemReference.Value = "0";
            itemReference.Text = hmUtility.GetDropDownFirstValue();
            ddlCompany.Items.Insert(0, itemReference);
            ddlSCompany.Items.Insert(0, itemReference);
        }
        private void LoadRoomType()
        {
            RoomTypeDA roomTypeDA = new RoomTypeDA();
            ddlRoomType.DataSource = roomTypeDA.GetRoomTypeInfo();
            ddlRoomType.DataTextField = "RoomType";
            ddlRoomType.DataValueField = "RoomTypeId";
            ddlRoomType.DataBind();

            ListItem itemRoomType = new ListItem();
            itemRoomType.Value = "0";
            itemRoomType.Text = hmUtility.GetDropDownFirstValue();
            ddlRoomType.Items.Insert(0, itemRoomType);
        }
        private void Cancel()
        {
            ddlCompany.SelectedIndex = 0;
            ddlRoomType.SelectedIndex = 0;
            ddlDiscountType.SelectedIndex = 0;
            txtDiscountAmount.Text = string.Empty;
            ddlActiveStatus.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            this.hfDiscountPolicyId.Value = string.Empty;            
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
        private bool IsFormValid()
        {
            bool flag = true;

            if (ddlCompany.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Company.", AlertType.Warning);
                ddlCompany.Focus();
                flag = false;
            }
            else if(ddlRoomType.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Room Type.", AlertType.Warning);
                ddlRoomType.Focus();
                flag = false;
            }            
            else if(string.IsNullOrEmpty(txtDiscountAmount.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Discount Amount.", AlertType.Warning);
                txtDiscountAmount.Focus();
                flag = false;
            }

            return flag;
        }

        //************************ User Defined Web Function ********************//
        [WebMethod]
        public static GridViewDataNPaging<CompanyWiseDiscountPolicyBO, GridPaging> SearchDiscountPolicyInfo(int companyId, bool activeStat, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<CompanyWiseDiscountPolicyBO, GridPaging> myGridData = new GridViewDataNPaging<CompanyWiseDiscountPolicyBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            CompanyWiseDiscountPolicyDA discountPolicyDA = new CompanyWiseDiscountPolicyDA();
            List<CompanyWiseDiscountPolicyBO> discountPolicyBO = new List<CompanyWiseDiscountPolicyBO>();
            discountPolicyBO = discountPolicyDA.SearchDiscountPolicyInfo(companyId, activeStat, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<CompanyWiseDiscountPolicyBO> distinctItems = new List<CompanyWiseDiscountPolicyBO>();
            distinctItems = discountPolicyBO.GroupBy(test => test.CompanyWiseDiscountId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static CompanyWiseDiscountPolicyBO LoadDiscountPolicyInfo(long companyWiseDiscountId)
        {
            CompanyWiseDiscountPolicyDA discountPolicyDA = new CompanyWiseDiscountPolicyDA();
            CompanyWiseDiscountPolicyBO discountPolicyBO = new CompanyWiseDiscountPolicyBO();
            discountPolicyBO = discountPolicyDA.GetDiscountPolicyById(companyWiseDiscountId);
            return discountPolicyBO;
        }        
    }
}