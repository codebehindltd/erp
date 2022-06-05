using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Membership;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Membership;
using HotelManagement.Entity.Payroll;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Membership
{
    public partial class DeviceMemberMapping : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMemberDepartment();
            }
        }
        private void LoadMemberDepartment()
        {
            DepartmentDA departmentDA = new DepartmentDA();
            EmployeeMappingDA employeeMappingDA = new EmployeeMappingDA();
            MemMemberTypeDA memberTypeDA = new MemMemberTypeDA();
            ddlMemberType.DataSource = memberTypeDA.GetAllMemberType();
            ddlMemberType.DataTextField = "Name";
            ddlMemberType.DataValueField = "TypeId";
            ddlMemberType.DataBind();

            ddlMappingMemberDepartment.DataSource = employeeMappingDA.GetMappingDepartmentInfo();
            ddlMappingMemberDepartment.DataTextField = "Name";
            ddlMappingMemberDepartment.DataValueField = "DepartmentId";
            ddlMappingMemberDepartment.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlMemberType.Items.Insert(0, item);
            ddlMappingMemberDepartment.Items.Insert(0, item);
        }
        [WebMethod]
        public static MemberViewBO GetMemberListByDepartmentId(int departmentId)
        {
            MemberViewBO employeeMappingList = new MemberViewBO();
            EmployeeMappingDA employeeDA = new EmployeeMappingDA();
            MemberMappingDA memberMappingDA = new MemberMappingDA();
            employeeMappingList.Members = memberMappingDA.GetMemberByType(departmentId);
            employeeMappingList.MappingMembers = memberMappingDA.GetMappingMemberList();
            return employeeMappingList;
        }
        [WebMethod]
        public static ReturnInfo UpdateMapping(List<MemberViewBO> memeberList)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo returninfo = new ReturnInfo();
            MemberMappingDA memberDA = new MemberMappingDA();
            try
            {
                bool status = memberDA.UpdateMemberMapping(memeberList);
                if (status)
                {
                    returninfo.IsSuccess = true;
                    returninfo.ObjectList = new ArrayList(memeberList);
                    returninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    foreach (var item in memeberList)
                    {
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.MembershipInfo.ToString(), item.MemberId, 
                            ProjectModuleEnum.ProjectModule.MembershipManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.MembershipInfo));
                    }
                }
                else
                {
                    returninfo.IsSuccess = false;
                    returninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                returninfo.IsSuccess = false;
                returninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return returninfo;
        }
    }
}