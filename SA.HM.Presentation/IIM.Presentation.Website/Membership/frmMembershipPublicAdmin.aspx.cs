using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Membership;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Membership;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Membership
{
    public partial class frmMembershipPublicAdmin : System.Web.UI.Page
    {

        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadMemberType();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            OnlineMemberBO onlineMember = new OnlineMemberBO();
            MemMemberBasicDA basicDA = new MemMemberBasicDA();
            //UserInformationDA userInformationDA = new UserInformationDA();
            //UserInformationBO temp = userInformationDA.GetUserInformationByUserNameNId(userInformationBO.UserId, userInformationBO.UserPassword);
            onlineMember = basicDA.GetOnlineMemberInfoByMemNumber(userInformationBO.UserId);
            if (onlineMember != null)
            {
                hfIntroMemNo.Value = onlineMember.MemberId.ToString();
            }
            else
            {
                hfIntroMemNo.Value = "";
            }
            hfGroupName.Value = userInformationBO.GroupName;
        }

        private void LoadMemberType()
        {
            MemMemberBasicDA memberDA = new MemMemberBasicDA();
            List<MemMemberTypeBO> typeList = memberDA.GetMemMemberTypeList();
            ddlMemberType.DataSource = typeList;
            ddlMemberType.DataTextField = "Name";
            ddlMemberType.DataValueField = "TypeId";
            ddlMemberType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlMemberType.Items.Insert(0, item);
        }
        private static bool SendMail(int memberId)
        {
            HMUtility hmUtility = new HMUtility();
            Email email;
            Email email2;
            bool status = false;
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            var accepted = "";
            var rejected = "";
            var deferred = "";

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
            string mainString = commonSetupBO.SetupValue;

            OnlineMemberBO onlineMember = new OnlineMemberBO();
            MemMemberBasicDA basicDA = new MemMemberBasicDA();
            onlineMember = basicDA.GetOnlineMemberInfoById(memberId, 0, 0);
            if (onlineMember.IsAccepted == true)
            {
                accepted = "Your membership application has been accepted.";
            }
            else if (onlineMember.IsRejected == true)
            {
                rejected = "Your membership application has been Rejected.";
            }
            else if (onlineMember.IsDeferred)
            {
                deferred = "Your membership application has been Deferred.";
            }
            if (!string.IsNullOrEmpty(mainString))
            {
                string[] dataArray = mainString.Split('~');
                email = new Email
                {
                    From = dataArray[0],
                    Password = dataArray[1],
                    To = onlineMember.PersonalEmail,
                    Host = dataArray[2],
                    Port = dataArray[3],
                    Subject = "Membership Confirmation",
                    TempleteName = HMConstants.EmailTemplates.Membership
                };
                email2 = new Email
                {
                    From = dataArray[0],
                    Password = dataArray[1],
                    To = onlineMember.PersonalEmail,
                    Host = dataArray[2],
                    Port = dataArray[3],
                    Subject = "Membership Rejection",
                    TempleteName = HMConstants.EmailTemplates.MembershipRejected
                };
                var token1 = new Dictionary<string, string>
                {
                    {"NAME", onlineMember.FullName},
                    {"MEMBERTYPE", onlineMember.TypeName},
                    {"MEMBERSHIPNUMBER", onlineMember.MembershipNumber},
                    {"MEETINGDATE", onlineMember.MeetingDate.ToString()},
                    {"ACCEPTED", accepted},
                    {"COMPANY", hmUtility.GetHMCompanyProfile()},
                    {"COMPANYADDRESS", hmUtility.GetHMCompanyAddress()},

                };
                var token2 = new Dictionary<string, string>
                {
                    {"NAME", onlineMember.FullName},
                    {"MEMBERTYPE", onlineMember.TypeName},
                    {"REJECTED", rejected},
                    {"REMARKS", onlineMember.Remarks},
                    {"COMPANY", hmUtility.GetHMCompanyProfile()},
                    {"COMPANYADDRESS", hmUtility.GetHMCompanyAddress()},
                };
                var token3 = new Dictionary<string, string>
                {
                    {"NAME", onlineMember.FullName},
                    {"MEMBERTYPE", onlineMember.TypeName},
                    {"REJECTED", deferred},
                    {"REMARKS", onlineMember.Remarks},
                    {"COMPANY", hmUtility.GetHMCompanyProfile()},
                    {"COMPANYADDRESS", hmUtility.GetHMCompanyAddress()},
                };
                try
                {
                    if (onlineMember.IsRejected == true)
                    {
                        status = EmailHelper.SendEmail(email2, token2);
                    }
                    else if (onlineMember.IsDeferred == true)
                    {
                        status = EmailHelper.SendEmail(email2, token3);
                    }
                    else
                    {
                        status = EmailHelper.SendEmail(email, token1);
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return status;
        }
        [WebMethod]
        public static GridViewDataNPaging<OnlineMemberBO, GridPaging> SearchNLoadMemberInformation(int typeId, string name, string mobile, string email, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int totalRecords = 0;
            GridViewDataNPaging<OnlineMemberBO, GridPaging> myGridData = new GridViewDataNPaging<OnlineMemberBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            MemMemberBasicDA memberBasicDA = new MemMemberBasicDA();
            List<OnlineMemberBO> memberList = new List<OnlineMemberBO>();
            memberList = memberBasicDA.GetOnlineMemberInfoBySearchCriteriaForPaging(typeId, name, mobile, email, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<OnlineMemberBO> distinctItems = new List<OnlineMemberBO>();
            distinctItems = memberList.GroupBy(test => test.MemberId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static GridViewDataNPaging<OnlineMemberBO, GridPaging> GetMemberInfoByIntroducer(int introId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            MemMemberBasicDA basicDA = new MemMemberBasicDA();
            List<OnlineMemberBO> introducers = new List<OnlineMemberBO>();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            int totalRecords = 0;
            GridViewDataNPaging<OnlineMemberBO, GridPaging> myGridData = new GridViewDataNPaging<OnlineMemberBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            introducers = basicDA.GetOnlineMemberInfoByIntroducerId(introId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<OnlineMemberBO> distinctItems = new List<OnlineMemberBO>();
            distinctItems = introducers.GroupBy(test => test.MemberId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;


        }
        [WebMethod]
        public static OnlineMemberBO GetOnlineMemInfoByMemberIdNIntroId(int memberId, int introId1, int introId2)
        {
            OnlineMemberBO onlineMember = new OnlineMemberBO();
            MemMemberBasicDA basicDA = new MemMemberBasicDA();
            onlineMember = basicDA.GetOnlineMemberInfoById(memberId, introId1, introId2);
            return onlineMember;
        }
        [WebMethod]
        public static OnlineMemberBO GetOnlineMemInfoById(int memberId)
        {
            OnlineMemberBO onlineMember = new OnlineMemberBO();
            MemMemberBasicDA basicDA = new MemMemberBasicDA();
            onlineMember = basicDA.GetOnlineMemberInfoById(memberId, 0, 0);
            return onlineMember;
        }
        [WebMethod]
        public static List<OnlineMemberFamilyBO> GetOnlineFamilyInfoById(int memId)
        {
            MemMemberBasicDA basicDA = new MemMemberBasicDA();
            List<OnlineMemberFamilyBO> familyBOs = new List<OnlineMemberFamilyBO>();
            familyBOs = basicDA.GetOnlineMemFamilyMemberByMemberId(memId);

            return familyBOs;
        }
        [WebMethod]
        public static List<OnlineMemberEducationBO> GetOnlineEducationInfoById(int memId)
        {
            MemMemberBasicDA basicDA = new MemMemberBasicDA();
            List<OnlineMemberEducationBO> educationBOs = new List<OnlineMemberEducationBO>();
            educationBOs = basicDA.GetOnlineMemberEducationsById(memId);

            return educationBOs;
        }
        [WebMethod]
        public static ReturnInfo UpdateAndAcceptMemByIntroducer(int memberId, int introId)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            bool status = false;
            var tempId = 0;

            OnlineMemberBO onlineMember = new OnlineMemberBO();
            MemMemberBasicDA basicDA = new MemMemberBasicDA();
            onlineMember = basicDA.GetOnlineMemberInfoById(memberId, 0, 0);
            var intro1 = onlineMember.Introducer_1_id;
            var intro2 = onlineMember.Introducer_2_id;

            if (intro1 == introId)//first introducer
            {
                tempId = introId;
                onlineMember.IsAccepted1 = true;
                onlineMember.IsRejected1 = false;
                onlineMember.IsDeferred1 = false;
                onlineMember.Remarks1 = "Accepted";
            }
            else if (intro2 == introId)//2nd introducer
            {
                tempId = introId;
                onlineMember.IsAccepted2 = true;
                onlineMember.IsRejected2 = false;
                onlineMember.IsDeferred2 = false;
                onlineMember.Remarks2 = "Accepted";
            }
            returnInfo.IsSuccess = basicDA.UpdateAndAcceptMemByIntroducer(onlineMember, memberId, introId);
            if (returnInfo.IsSuccess)
            {
                returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
            }
            else
            {
                returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Warning);
            }
            return returnInfo;
        }
        [WebMethod]
        public static ReturnInfo UpdateAndRejectMemByIntroducer(int memberId, int introId, string remarks, bool isReject, bool isDefer)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            bool status = false;

            OnlineMemberBO onlineMember = new OnlineMemberBO();
            MemMemberBasicDA basicDA = new MemMemberBasicDA();
            onlineMember = basicDA.GetOnlineMemberInfoById(memberId, 0, 0);

            var intro1 = onlineMember.Introducer_1_id;
            var intro2 = onlineMember.Introducer_2_id;

            if (intro1 == introId)//first introducer
            {
                onlineMember.IsAccepted1 = false;
                onlineMember.IsRejected1 = isReject;
                onlineMember.IsDeferred1 = isDefer;
                onlineMember.Remarks1 = remarks;
            }
            else if (intro2 == introId)
            {
                onlineMember.IsAccepted2 = false;
                onlineMember.IsRejected2 = isReject;
                onlineMember.IsDeferred2 = isDefer;
                onlineMember.Remarks2 = remarks;
            }
            onlineMember.Remarks = remarks;

            returnInfo.IsSuccess = basicDA.UpdateAndRejectMemByIntroducer(onlineMember, memberId);
            if (returnInfo.IsSuccess == true)
            {
                returnInfo.AlertMessage = CommonHelper.AlertInfo("Rejection Operation successful.", AlertType.Success);
            }
            else
            {
                returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Warning);
            }
            return returnInfo;
        }
        [WebMethod]
        public static ReturnInfo UpdateAndAcceptMember(int memberId, DateTime MeetingDate, string MeetingDecision, DateTime MeetingDateEC, string MeetingDecisionEC)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            bool status = false, isCreatedUser = false;
            HMUtility hmUtility = new HMUtility();

            UserInformationBO userInformationBO = new UserInformationBO();
            UserInformationDA userInformationDA = new UserInformationDA();
            int tmpUserInfoId = 0;
            UserInformationBO currentUserInformationBO = new UserInformationBO();
            currentUserInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            OnlineMemberBO onlineMember = new OnlineMemberBO();
            OnlineMemberBO memberReg = new OnlineMemberBO();
            MemMemberBasicDA basicDA = new MemMemberBasicDA();
            onlineMember = basicDA.GetOnlineMemberInfoById(memberId, 0, 0);

            onlineMember.IsAccepted = true;
            onlineMember.IsRejected = false;
            onlineMember.IsDeferred = false;
            onlineMember.Remarks = "Accepted";

            onlineMember.MeetingDate = MeetingDate;
            onlineMember.MeetingDecision = MeetingDecision;
            onlineMember.MeetingDateEC = MeetingDateEC;
            onlineMember.MeetingDecisionEC = MeetingDecisionEC;

            returnInfo.IsSuccess = basicDA.UpdateAndAcceptMember(onlineMember);
            if (returnInfo.IsSuccess == true)
            {
                memberReg = basicDA.GetOnlineMemberInfoById(memberId, 0, 0);
                //member user create 
                UserGroupBO userGroup = new UserGroupBO();
                UserGroupDA userGroupDA = new UserGroupDA();
                userGroup = userGroupDA.GetUserGroupInfoByGroupName("Membership");

                userInformationBO.UserName = memberReg.FullName;
                userInformationBO.UserId = memberReg.MembershipNumber;
                userInformationBO.UserPassword = memberReg.MembershipNumber;
                userInformationBO.UserGroupId = userGroup.UserGroupId;

                userInformationBO.CreatedBy = currentUserInformationBO.UserInfoId;
                userInformationBO.ActiveStat = true;
                userInformationBO.IsAdminUser = false;

                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();

                isCreatedUser = userInformationDA.SaveUserInformation(userInformationBO, adminAuthorizationList, out tmpUserInfoId);

                status = SendMail(onlineMember.MemberId);
                if (status)
                {
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update + " Mail Sent", AlertType.Success);
                }
                else
                {
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                }

            }
            else
            {
                returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Warning);
            }
            return returnInfo;
        }
        [WebMethod]
        public static ReturnInfo UpdateAndRejectMember(int memberId, string remarks, bool isReject, bool isDefer, DateTime MeetingDate, string MeetingDecision, DateTime MeetingDateEC, string MeetingDecisionEC)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            bool status = false;

            OnlineMemberBO onlineMember = new OnlineMemberBO();
            MemMemberBasicDA basicDA = new MemMemberBasicDA();
            onlineMember = basicDA.GetOnlineMemberInfoById(memberId, 0, 0);

            onlineMember.IsAccepted = false;
            onlineMember.IsRejected = isReject;
            onlineMember.IsDeferred = isDefer;
            onlineMember.Remarks = remarks;

            onlineMember.MeetingDate = MeetingDate;
            onlineMember.MeetingDecision = MeetingDecision;
            onlineMember.MeetingDateEC = MeetingDateEC;
            onlineMember.MeetingDecisionEC = MeetingDecisionEC;

            returnInfo.IsSuccess = basicDA.UpdateAndRejectMember(onlineMember);
            if (returnInfo.IsSuccess == true)
            {
                status = SendMail(onlineMember.MemberId);
                returnInfo.AlertMessage = CommonHelper.AlertInfo("Rejection Operation successful.", AlertType.Success);
            }
            else
            {
                returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Warning);
            }
            return returnInfo;
        }
    }
}