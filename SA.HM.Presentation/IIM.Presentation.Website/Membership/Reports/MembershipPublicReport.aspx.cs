using HotelManagement.Data.Membership;
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

namespace HotelManagement.Presentation.Website.Membership.Reports
{
    public partial class MembershipPublicReport : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMemberType();
            }
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

        [WebMethod]
        public static GridViewDataNPaging<OnlineMemberBO, GridPaging> SearchNLoadMemberInformation(int typeId, string MembershipNo, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int totalRecords = 0;
            GridViewDataNPaging<OnlineMemberBO, GridPaging> myGridData = new GridViewDataNPaging<OnlineMemberBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            MemMemberBasicDA memberBasicDA = new MemMemberBasicDA();
            List<OnlineMemberBO> memberList = new List<OnlineMemberBO>();
            memberList = memberBasicDA.GetOnlineMemberInfoBySearchCriteriaForReport(typeId, MembershipNo, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<OnlineMemberBO> distinctItems = new List<OnlineMemberBO>();
            distinctItems = memberList.GroupBy(test => test.MemberId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
    }
}