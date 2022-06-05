using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Membership;
using HotelManagement.Data.Membership;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.Membership
{
    public partial class frmMemMemberType : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
        }

        //**************************** Handlers ****************************//
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtTypeName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Type Name.", AlertType.Warning);
                this.txtTypeName.Focus();
                return;
            }

            MemMemberTypeBO memberTypeBO = new MemMemberTypeBO();
            MemMemberTypeDA memberTypeDA = new MemMemberTypeDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            memberTypeBO.Name = txtTypeName.Text;
            memberTypeBO.Code = txtTypeCode.Text;
            if (!string.IsNullOrEmpty(txtSubscriptionFee.Text))
            {
                memberTypeBO.SubscriptionFee = Convert.ToDecimal(txtSubscriptionFee.Text);
            }
            if (!string.IsNullOrEmpty(txtDiscountPercent.Text))
            {
                memberTypeBO.DiscountPercent = Convert.ToDecimal(txtDiscountPercent.Text);
            }

            if (string.IsNullOrWhiteSpace(hfMemberTypeId.Value))
            {
                int tmpTypeId = 0;
                memberTypeBO.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = memberTypeDA.SaveMemberTypeInfo(memberTypeBO, out tmpTypeId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.MembershipType.ToString(), tmpTypeId,
                        ProjectModuleEnum.ProjectModule.MembershipManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.MembershipType));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    this.Cancel();
                }
            }
            else
            {
                memberTypeBO.TypeId = Convert.ToInt32(hfMemberTypeId.Value);
                memberTypeBO.LastModifiedBy = userInformationBO.UserInfoId;
                Boolean status = memberTypeDA.UpdateMemberTypeInfo(memberTypeBO);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.MembershipType.ToString(), memberTypeBO.TypeId,
                        ProjectModuleEnum.ProjectModule.MembershipManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.MembershipType));
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                    this.Cancel();
                }
            }
        }


        //************************ User Defined Function ********************//
        private void Cancel()
        {
            txtTypeName.Text = string.Empty;
            txtTypeCode.Text = string.Empty;
            txtSubscriptionFee.Text = string.Empty;
            txtDiscountPercent.Text = string.Empty;
            hfMemberTypeId.Value = string.Empty;
            this.btnSave.Text = "Save";
        }

        //************************ User Defined Function ********************//
        [WebMethod]
        public static GridViewDataNPaging<MemMemberTypeBO, GridPaging> SearchMemberTypeNLoadGridInformation(string typeName, string code, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<MemMemberTypeBO, GridPaging> myGridData = new GridViewDataNPaging<MemMemberTypeBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            MemMemberTypeDA memberTypeDA = new MemMemberTypeDA();
            List<MemMemberTypeBO> memberTypeList = new List<MemMemberTypeBO>();
            memberTypeList = memberTypeDA.GetMemberTypeInfoBySearchCriteriaForPaging(typeName, code, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<MemMemberTypeBO> distinctItems = new List<MemMemberTypeBO>();
            distinctItems = memberTypeList.GroupBy(test => test.TypeId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static MemMemberTypeBO LoadDetailInformation(int memberTypeId)
        {
            MemMemberTypeDA memberTypeDA = new MemMemberTypeDA();
            return memberTypeDA.GetMemberTypeInfoById(memberTypeId);
        }
        [WebMethod]
        public static ReturnInfo DeleteData(int sMemberTypeId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("MemMemberType", "TypeId", sMemberTypeId);

                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.MembershipType.ToString(), sMemberTypeId,
                        ProjectModuleEnum.ProjectModule.MembershipManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.MembershipType));
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
    }
}