using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.SalesAndMarketing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using HotelManagement.Entity.UserInformation;
using System.Text;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class TemplateInfoIframe : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTemplateType();
                LoadTemplateFor();
                LoadCommonDropDownHiddenField();
            }
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }

        private void LoadTemplateType()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("TemplateType");

            ddlType.DataSource = fields;
            ddlType.DataTextField = "FieldValue";
            ddlType.DataValueField = "FieldValue";
            ddlType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlType.Items.Insert(0, item);
        }
        private void LoadTemplateFor()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("TemplateFor");

            ddlTemplateFor.DataSource = fields;
            ddlTemplateFor.DataTextField = "FieldValue";
            ddlTemplateFor.DataValueField = "Description";
            ddlTemplateFor.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlTemplateFor.Items.Insert(0, item);
        }
        [WebMethod]
        public static ReturnInfo SaveUpdate(TemplateInformationBO bO, List<TemplateInformationDetailBO> newlyAddedItem, List<TemplateInformationDetailBO> deleteDbItem)
        {

            HMCommonDA hmCommonDA = new HMCommonDA();
            bool status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            rtninfo.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            try
            {
                bO.CreatedBy = userInformationBO.UserInfoId;
                long OutId;
                TemplateInfoDA DA = new TemplateInfoDA();
                status = DA.SaveTemplateInformation(bO, out OutId);

                if (status)
                {

                    if (bO.Id == 0)
                    {
                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                                EntityTypeEnum.EntityType.TemplateInformation.ToString(), OutId,
                            ProjectModuleEnum.ProjectModule.HMCommon.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.TemplateInformation));

                    }
                    else
                    {
                        rtninfo.IsSuccess = true;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                               EntityTypeEnum.EntityType.TemplateInformation.ToString(), OutId,
                               ProjectModuleEnum.ProjectModule.HMCommon.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.TemplateInformation));
                    }
                    bool detailsStatus = DA.SaveDetails(newlyAddedItem, deleteDbItem, OutId);

                }
                else
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo("save Fail", AlertType.Error);

                }
                return rtninfo;
            }
            catch (Exception ex)
            {
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }
            return rtninfo;
        }
        [WebMethod]
        public static ArrayList FillForm(long Id)
        {
            TemplateInfoDA DA = new TemplateInfoDA();
            TemplateInformationBO bO = new TemplateInformationBO();
            bO = DA.GetTemplateInformationById(Id);
            List<TemplateInformationDetailBO> detailsBOs = new List<TemplateInformationDetailBO>();
            detailsBOs = DA.GetTemplateInformationDetail(Id);

            ArrayList arr = new ArrayList();
            arr.Add(new { Details = detailsBOs, TemplateInformation = bO });

            return arr;
        }

        [WebMethod]
        public static List<CustomFieldBO> GetCommonFields(string segment) {
            HMCommonDA commonDA = new HMCommonDA();
            HMUtility hmUtility = new HMUtility();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField(segment);

            return fields;
        }
        [WebMethod]
        public static ArrayList GetFieldsFromTable(string segment)
        {
            List<DynamicReportBO> dynamicReports = new List<DynamicReportBO>();
            List<DynamicReportBO> dynamicRearranged = new List<DynamicReportBO>();
            DynamicReportDA reportDA = new DynamicReportDA();

            

            dynamicReports = reportDA.GetFieldsFromTable(segment);

            foreach (var item in dynamicReports.ToList())
            {
                if (item.DataType == "bit")
                {
                    dynamicReports.Remove(item);
                }
                else if (item.ColumnName.Equals("EmpId"))
                {
                    dynamicReports.Remove(item);
                }
                else if (item.ColumnName.Equals("EmpPassword"))
                {
                    dynamicReports.Remove(item);
                }
                else if (item.ColumnName.Equals("CostCenterId"))
                {
                    dynamicReports.Remove(item);
                }
                else if (item.ColumnName.Equals("AttendanceDeviceEmpId"))
                {
                    dynamicReports.Remove(item);
                }
                else if (item.ColumnName.Equals("DonorId"))
                {
                    dynamicReports.Remove(item);
                }
                else if (item.ColumnName.Equals("NodeId"))
                {
                    dynamicReports.Remove(item);
                }
                else if (item.ColumnName.Equals("Title"))
                {
                    dynamicReports.Remove(item);
                }
                else if (item.ColumnName.Equals("FirstName"))
                {
                    dynamicReports.Remove(item);
                }
                else if (item.ColumnName.Equals("LastName"))
                {
                    dynamicReports.Remove(item);
                }
                else if (item.ColumnName.Equals("ActivityCode"))
                {
                    dynamicReports.Remove(item);
                }
            }

            foreach (var item in dynamicReports)
            {
                string theString = item.ColumnName;

                StringBuilder builder = new StringBuilder();
                foreach (char c in theString)
                {
                    if (Char.IsUpper(c) && builder.Length > 0) builder.Append(' ');
                    builder.Append(c);
                }
                theString = builder.ToString();

                DynamicReportBO bO = new DynamicReportBO();
                bO.ColumnName = theString;
                bO.DataType = item.DataType;
                dynamicRearranged.Add(bO);
            }

            foreach (var item in dynamicRearranged.ToList())
            {
                if (item.ColumnName.Equals("Emp Code"))
                {
                    item.ColumnName = "Employee Code";
                }
                else if (item.ColumnName.Equals("Display Name"))
                {
                    item.ColumnName = "Employee Name";
                }
                else if (item.ColumnName.Equals("Emp Type Id"))
                {
                    item.ColumnName = "Employee Type";
                }
                else if (item.ColumnName.Equals("Probable P F Eligibility Date"))
                {
                    item.ColumnName = "Probable Provident Fund Eligibility Date";
                }
                else if (item.ColumnName.Equals("P F Eligibility Date"))
                {
                    item.ColumnName = "Provident Fund Eligibility Date";
                }
                else if (item.ColumnName.Equals("P F Eligibility Date"))
                {
                    item.ColumnName = "Provident Fund Eligibility Date";
                }
                else if (item.ColumnName.Equals("P F Terminate Date"))
                {
                    item.ColumnName = "Provident Fund Terminate Date";
                }
                else if (item.ColumnName.Equals("P Issue Place"))
                {
                    item.ColumnName = "Passport Issue Place";
                }
                else if (item.ColumnName.Equals("P Issue Date"))
                {
                    item.ColumnName = "Passport Issue Date";
                }
                else if (item.ColumnName.Equals("P Expire Date"))
                {
                    item.ColumnName = "Passport Expire Date";
                }
                else if (item.ColumnName.Equals("Emp Date Of Birth"))
                {
                    item.ColumnName = "Date Of Birth";
                }
                else if (item.ColumnName.Equals("Gl Company"))
                {
                    item.ColumnName = "Company Name";
                }
                else if ((item.ColumnName.Contains("Id")) && (item.ColumnName != "National Id"))
                {
                    item.ColumnName = item.ColumnName.Substring(0, item.ColumnName.Length - 2);
                }
            }
            ArrayList arr = new ArrayList();
            arr.Add(new { RawColumns = dynamicReports, Rearranged = dynamicRearranged });

            return arr;
        }
    }
}