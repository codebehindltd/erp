using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class SalesBill : BasePage
    {
        HMUtility hmUtility = new HMUtility();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                companyProjectUserControl.ConnfigurationID = "IsDealIntegrateWithAccounts";

                LoadCompany();
            }
        }

        private void LoadCompany()
        {
            GuestCompanyDA guestCompanyDA = new GuestCompanyDA();
            List<GuestCompanyBO> files = new List<GuestCompanyBO>();
            files = guestCompanyDA.GetALLGuestCompanyInfo();


            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            this.ddlCompany.DataSource = files;
            this.ddlCompany.DataTextField = "CompanyName";
            this.ddlCompany.DataValueField = "CompanyId";
            this.ddlCompany.DataBind();

            this.ddlCompany.Items.Insert(0, item);
        }
        private static bool SendMail()
        {
            Email email;
            bool status = false;

            List<UserGroupBO> userGroupBO = new List<UserGroupBO>();
            UserGroupDA userGroupDA = new UserGroupDA();
            userGroupBO = userGroupDA.GetUserGroupInfo();

            UserGroupBO userGroup = userGroupBO.Where(i => i.UserGroupType == ConstantHelper.UserGroupType.Inventory.ToString()).FirstOrDefault();

            string emailAddress = userGroup != null ? userGroup.Email : string.Empty;

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
            string mainString = commonSetupBO.SetupValue;

            if (!string.IsNullOrEmpty(mainString))
            {
                string[] dataArray = mainString.Split('~');

                if (!string.IsNullOrWhiteSpace(emailAddress))
                {
                    email = new Email()
                    {
                        From = dataArray[0],
                        Password = dataArray[1],
                        To = emailAddress,
                        Subject = "Item Out For Sales",
                        Body = "Please Out Product From Inventory.",
                        Host = dataArray[2],
                        Port = dataArray[3],
                        TempleteName = HMConstants.EmailTemplates.BirthdayWish
                    };

                    var tokens = new Dictionary<string, string>
                         {
                             {"COMPANYNAME",null}
                };
                    try
                    {
                        status = EmailHelper.SendEmail(email, tokens);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }


                HMCommonSetupBO setUpBO = new HMCommonSetupBO();
                setUpBO = commonSetupDA.GetCommonConfigurationInfo("TechnicalDepartment", "TechnicalDepartment");

                if (setUpBO != null)
                {
                    userGroup = userGroupBO.Where(i => i.UserGroupId == Convert.ToInt32(setUpBO.SetupValue) || i.UserGroupType == ConstantHelper.UserGroupType.Technical.ToString()).FirstOrDefault();
                    emailAddress = userGroup != null ? userGroup.Email : string.Empty;
                }
                if (!string.IsNullOrWhiteSpace(emailAddress))
                {
                    email = new Email()
                    {
                        From = dataArray[0],
                        Password = dataArray[1],
                        To = emailAddress,
                        Subject = "Item Out For Sales",
                        Body = "Please Receive Your Product From Inventory.",
                        Host = dataArray[2],
                        Port = dataArray[3],
                        TempleteName = HMConstants.EmailTemplates.BirthdayWish
                    };

                    var tokens = new Dictionary<string, string>
                         {
                             {"COMPANYNAME",null}
                };
                    try
                    {
                        status = EmailHelper.SendEmail(email, tokens);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            return status;
        }
        [WebMethod]
        public static GridViewDataNPaging<SMQuotationViewBO, GridPaging> SearchQuotation(string quotationNumber, int companyId, DateTime? fromDate, DateTime? toDate, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<SMQuotationViewBO, GridPaging> myGridData = new GridViewDataNPaging<SMQuotationViewBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            SalesQuotationNBillingDA invItemDA = new SalesQuotationNBillingDA();
            List<SMQuotationViewBO> invItemList = new List<SMQuotationViewBO>();
            invItemList = invItemDA.GetQuotationForSalesBillOrSalesNote(quotationNumber, companyId, fromDate, toDate, "SalesBill", userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(invItemList, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo ApproveQuotation(long quotationId, long dealId, long glCompanyId, long glProjectId)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninf = new ReturnInfo();
            SalesQuotationEditBO quotationv = new SalesQuotationEditBO();
            SalesQuotationNBillingDA salesDA = new SalesQuotationNBillingDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            try
            {
                SMDeal previousBO = new DealDA().GetDealInfoBOById(dealId);
                SalesMarketingLogType<SMDeal> logDA = new SalesMarketingLogType<SMDeal>();
                rtninf.IsSuccess = salesDA.UpdateApprovalFromSalesBilling(quotationId, glCompanyId, glProjectId, userInformationBO.UserInfoId);

                if (rtninf.IsSuccess)
                {
                    SMDeal deal = new DealDA().GetDealInfoBOById(dealId);
                    logDA.Log(ConstantHelper.SalesandMarketingLogType.DealActivity, deal, previousBO);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.SMQuotation.ToString(), quotationId,
                           ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMQuotation));
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                    //SendMail();
                }
                else
                {
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