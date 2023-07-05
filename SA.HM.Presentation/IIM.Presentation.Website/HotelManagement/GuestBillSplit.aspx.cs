using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class GuestBillSplit : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        HMCommonDA hmCommonDA = new HMCommonDA();        
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadCurrentDate();
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                CheckObjectPermission();
                string strRegistrationId = Request.QueryString["RId"];
                if (!string.IsNullOrEmpty(strRegistrationId))
                {
                    this.LoadCheckBoxListServiceInformation(strRegistrationId);
                }
            }
        }
        protected void gvIndividualServiceInformationForBillSplit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ((CheckBox)e.Row.FindControl("chkIsSelected")).Checked = true;
            }
        }
        protected void gvGroupServiceInformationForBillSplit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ((CheckBox)e.Row.FindControl("chkIsSelected")).Checked = true;
            }
        }

        //************************ User Defined Function ********************//
        private void CheckObjectPermission()
        {
            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            //ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            //objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmBank.ToString());

            //isSavePermission = objectPermissionBO.IsSavePermission;
            //isDeletePermission = objectPermissionBO.IsDeletePermission;

            //btnSave.Visible = isSavePermission;

            //if (isSavePermission)
            //{
            //    hfSavePermission.Value = "1";
            //}
            //else
            //{
            //    hfSavePermission.Value = "0";
            //}

            //if (isDeletePermission)
            //{
            //    hfDeletePermission.Value = "1";
            //}
            //else
            //{
            //    hfDeletePermission.Value = "0";
            //}

        }
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtStartDate.Text = hmUtility.GetStringFromDateTime(dateTime.AddYears(-12));
            this.txtEndDate.Text = hmUtility.GetStringFromDateTime(dateTime.AddDays(1));
        }
        private void LoadCheckBoxListServiceInformation(string strRegistrationId)
        {
            string registrationIdList = string.Empty;
            if (!string.IsNullOrWhiteSpace(strRegistrationId))
            {
                txtConversionRateHiddenField.Value = "0.00";
                txtSrcRegistrationId.Value = strRegistrationId;
                registrationIdList = strRegistrationId;

                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();

                roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(Convert.ToInt32(strRegistrationId));
                if (roomRegistrationBO != null)
                {
                    if (roomRegistrationBO.RegistrationId > 0)
                    {
                        txtConversionRateHiddenField.Value = roomRegistrationBO.ConversionRate.ToString("0.00");
                    }
                }

                HMCommonDA hmCommonDA = new HMCommonDA();
                registrationIdList = hmCommonDA.GetRegistrationIdList(registrationIdList);

                GuestBillSplitDA entityDA = new GuestBillSplitDA();
                List<GuestServiceBillApprovedBO> entityBOList = new List<GuestServiceBillApprovedBO>();

                int commaCount = registrationIdList.Count(x => x == ',');
                if (commaCount == 1)
                {
                    if (string.IsNullOrWhiteSpace(registrationIdList.Split(',')[1]))
                    {
                        registrationIdList = registrationIdList.Replace(",", "");
                    }
                }

                txtSrcRegistrationIdList.Value = registrationIdList;

                // // --------------Room Related Sales Information Process ----------------------
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                RoomRegistrationDA roomregistrationDA = new RoomRegistrationDA();
                roomregistrationDA.RoomNightAuditProcess(registrationIdList, DateTime.Now.AddDays(1), 0, userInformationBO.UserInfoId);
                // // --------------Room Related Sales Information Process ----------------------END

                entityBOList = entityDA.GetGuestServiceInfoByRegistrationId(registrationIdList);

                var ServiceList = entityBOList.Where(x => x.ServiceType == "GuestHouseService" || x.ServiceType == "RestaurantService" || x.ServiceType == "BanquetService").ToList();
                var RoomList = entityBOList.Where(x => x.ServiceType == "GuestRoomService").ToList();
                var GuestPayment = entityBOList.Where(x => x.ServiceType == "GuestPaymentStatement").ToList();

                this.chkBillSpliteRoomItem.DataSource = RoomList;
                this.chkBillSpliteRoomItem.DataTextField = "ServiceName";
                this.chkBillSpliteRoomItem.DataValueField = "ServiceId";
                this.chkBillSpliteRoomItem.DataBind();

                this.chkCompanyPaymentBillSpliteRoomItem.DataSource = RoomList;
                this.chkCompanyPaymentBillSpliteRoomItem.DataTextField = "ServiceName";
                this.chkCompanyPaymentBillSpliteRoomItem.DataValueField = "ServiceId";
                this.chkCompanyPaymentBillSpliteRoomItem.DataBind();

                this.chkBillSpliteServiceItem.DataSource = ServiceList;
                this.chkBillSpliteServiceItem.DataTextField = "ServiceName";
                this.chkBillSpliteServiceItem.DataValueField = "ServiceId";
                this.chkBillSpliteServiceItem.DataBind();

                this.chkCompanyPaymentBillSpliteServiceItem.DataSource = ServiceList;
                this.chkCompanyPaymentBillSpliteServiceItem.DataTextField = "ServiceName";
                this.chkCompanyPaymentBillSpliteServiceItem.DataValueField = "ServiceId";
                this.chkCompanyPaymentBillSpliteServiceItem.DataBind();

                this.chkBillSplitePaymentItem.DataSource = GuestPayment;
                this.chkBillSplitePaymentItem.DataTextField = "ServiceName";
                this.chkBillSplitePaymentItem.DataValueField = "ServiceId";
                this.chkBillSplitePaymentItem.DataBind();

                if (GuestPayment != null)
                {
                    if (GuestPayment.Count > 0)
                    {
                        List<GuestServiceBillApprovedBO> companyEntityBOList = new List<GuestServiceBillApprovedBO>();
                        GuestServiceBillApprovedBO companyEntityBO = new GuestServiceBillApprovedBO();
                        companyEntityBO.ServiceName = "Total Payment";
                        companyEntityBO.ServiceId = -100;
                        companyEntityBOList.Add(companyEntityBO);
                        this.chkCompanyPaymentBillSplitePaymentItem.DataSource = companyEntityBOList;
                        this.chkCompanyPaymentBillSplitePaymentItem.DataTextField = "ServiceName";
                        this.chkCompanyPaymentBillSplitePaymentItem.DataValueField = "ServiceId";
                        this.chkCompanyPaymentBillSplitePaymentItem.DataBind();
                    }
                }

                List<GuestServiceBillApprovedBO> individualEntityBOList = new List<GuestServiceBillApprovedBO>();
                individualEntityBOList = entityDA.GetGuestIndividualServiceInfoByRegistrationId(registrationIdList);

                var individualServiceList = individualEntityBOList.Where(x => x.ServiceType == "GuestHouseService" || x.ServiceType == "RestaurantService" || x.ServiceType == "BanquetService").ToList();
                var individualRoomList = individualEntityBOList.Where(x => x.ServiceType == "GuestRoomService").ToList();
                var individualPaymentList = individualEntityBOList.Where(x => x.ServiceType == "GuestPaymentStatement").ToList();
                var individualTransferedPaymentList = individualEntityBOList.Where(x => x.ServiceType == "OthersGuestPaymentStatement").ToList();

                this.chkBillSpliteIndividualRoomItem.DataSource = individualRoomList;
                this.chkBillSpliteIndividualRoomItem.DataTextField = "ServiceName";// "ServiceName" + "ServiceType";
                this.chkBillSpliteIndividualRoomItem.DataValueField = "ApprovedId";
                this.chkBillSpliteIndividualRoomItem.DataBind();

                this.chkBillSpliteIndividualServiceItem.DataSource = individualServiceList;
                this.chkBillSpliteIndividualServiceItem.DataTextField = "ServiceName";
                this.chkBillSpliteIndividualServiceItem.DataValueField = "ApprovedId";
                this.chkBillSpliteIndividualServiceItem.DataBind();

                this.chkBillSpliteIndividualPaymentItem.DataSource = individualPaymentList;
                this.chkBillSpliteIndividualPaymentItem.DataTextField = "ServiceName";
                this.chkBillSpliteIndividualPaymentItem.DataValueField = "ApprovedId";
                this.chkBillSpliteIndividualPaymentItem.DataBind();

                this.chkBillSpliteIndividualTransferedPaymentItem.DataSource = individualTransferedPaymentList;
                this.chkBillSpliteIndividualTransferedPaymentItem.DataTextField = "ServiceName";
                this.chkBillSpliteIndividualTransferedPaymentItem.DataValueField = "ApprovedId";
                this.chkBillSpliteIndividualTransferedPaymentItem.DataBind();

            }            
        }

        //************************ User Defined WebMethod ********************//
        [WebMethod(EnableSession = true)]
        public static string PerformBillSplitePrintPreview(string currencyRate, string isIsplite, string serviceType, string SelectdIndividualTransferedPaymentId, string SelectdPaymentId, string SelectdIndividualPaymentId, string SelectdIndividualServiceId, string SelectdIndividualRoomId, string SelectdServiceId, string SelectdRoomId, string StartDate, string EndDate, string ddlRegistrationId, string txtSrcRegistrationIdList)
        {
            InnboardWebUtility.BillPrintPreviewDynamicallyForReport(currencyRate, isIsplite, serviceType, SelectdIndividualTransferedPaymentId, SelectdPaymentId, SelectdIndividualPaymentId, SelectdIndividualServiceId, SelectdIndividualRoomId, SelectdServiceId, SelectdRoomId, StartDate, EndDate, ddlRegistrationId, txtSrcRegistrationIdList);
            return "";
        }

    }
}