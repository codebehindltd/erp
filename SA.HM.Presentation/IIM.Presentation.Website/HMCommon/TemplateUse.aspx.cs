using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static HotelManagement.Presentation.Website.HMCommon.CustomNoticeIframe;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class TemplateUse : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTemplateType();
                LoadTemplateFor();
                LoadTemplateInformation();
                LoadDepartment();
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

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlType.Items.Insert(0, item);
        }
        private void LoadDepartment()
        {
            DepartmentDA DA = new DepartmentDA();
            List<DepartmentBO> depBO = new List<DepartmentBO>();
            depBO = DA.GetDepartmentInfo();

            ddlEmpDepartment.DataSource = depBO;
            ddlEmpDepartment.DataTextField = "Name";
            ddlEmpDepartment.DataValueField = "DepartmentId";
            ddlEmpDepartment.DataBind();
            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlEmpDepartment.Items.Insert(0, item);

        }
        private void LoadTemplateFor()
        {
            //HMCommonDA commonDA = new HMCommonDA();
            //CustomFieldBO customField = new CustomFieldBO();
            //List<CustomFieldBO> fields = new List<CustomFieldBO>();
            //fields = commonDA.GetCustomField("TemplateFor");

            //ddlSenderType.DataSource = fields;
            //ddlSenderType.DataTextField = "FieldValue";
            //ddlSenderType.DataValueField = "Description";
            //ddlSenderType.DataBind();

            //ListItem item = new ListItem();
            //item.Value = "0";
            //item.Text = hmUtility.GetDropDownFirstValue();
            //ddlSenderType.Items.Insert(0, item);

        }
        private void LoadTemplateInformation()
        {
            TemplateInfoDA DA = new TemplateInfoDA();

            List<TemplateInformationBO> BOs = new List<TemplateInformationBO>();

            BOs = DA.GetTemplateInformationForDdl();

            ddlTemplate.DataSource = BOs;
            ddlTemplate.DataTextField = "Name";
            ddlTemplate.DataValueField = "Id";
            ddlTemplate.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlTemplate.Items.Insert(0, item);
        }
        [WebMethod]
        public static TemplateInformationBO GetTemplateInfo(long id)
        {
            TemplateInformationBO informationBO = new TemplateInformationBO();
            TemplateInfoDA DA = new TemplateInfoDA();

            informationBO = DA.GetTemplateInformationById(id);

            return informationBO;
        }
        [WebMethod]
        public static GridViewDataNPaging<TemplateEmailBO, GridPaging> LoadSearch(string name, string type, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<TemplateEmailBO, GridPaging> myGridData = new GridViewDataNPaging<TemplateEmailBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            List<TemplateEmailBO> templateInformation = new List<TemplateEmailBO>();
            TemplateInfoDA DA = new TemplateInfoDA();
            templateInformation = DA.GetTemplateUsedWithGrid(name, type, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(templateInformation, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static List<TemplateInformationBO> GetTemplateInfoByType(string type)
        {
            List<TemplateInformationBO> informationBO = new List<TemplateInformationBO>();
            TemplateInfoDA DA = new TemplateInfoDA();

            informationBO = DA.GetTemplateInformationByType(type);

            return informationBO;
        }
        [WebMethod]
        public static TemplateEmailBO GetTemplateEmailInfoById(long Id)
        {
            TemplateEmailBO informationBO = new TemplateEmailBO();
            TemplateInfoDA DA = new TemplateInfoDA();

            informationBO = DA.GetTemplateEmailInfoById(Id);
            informationBO.EmployeeList = DA.GetTemplateUsedEmployeeByDetailId(informationBO.Id);
            return informationBO;
        }
        [WebMethod]
        public static ReturnInfo DeleteData(int Id)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("TemplateEmail", "Id", Id);

                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.TemplateInformation.ToString(), Id, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.TemplateInformation));
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
        public static ReturnInfo SaveTemplateUse(long id, string type, int templateId, string subject, string body, string empList, string AssignType)
        {
            ReturnInfo info = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            CommonDA commonDA = new CommonDA();
            long outId = 0;
            TemplateInfoDA DA = new TemplateInfoDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            TemplateEmailBO templateEmailBO = new TemplateEmailBO
            {
                Id = id,
                TemplateBody = body,
                TemplateType = type,
                AssignType = AssignType,
                TemplateId = templateId,
                CreatedBy = userInformationBO.UserInfoId
            };

            status = DA.SaveTemplateEmail(templateEmailBO, empList, type, templateId, out outId);
            if (status)
            {
                info.IsSuccess = true;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                Boolean logStatus = hmUtility.CreateActivityLogEntity("TemplateEamil Saved",
                                EntityTypeEnum.EntityType.TemplateInformation.ToString(), outId,
                            ProjectModuleEnum.ProjectModule.HMCommon.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.TemplateInformation));
                if (type == "Email")
                {
                    var sentMail = SendMail(templateId, subject, body, empList);
                    if (!sentMail)
                    {
                        info.AlertMessage = CommonHelper.AlertInfo("Save operation successful but has some problem with mail send", AlertType.Warning);
                    }
                    else
                    {
                        info.AlertMessage = CommonHelper.AlertInfo("Save operation successful with mail send", AlertType.Success);
                    }
                }
                else if (type == "Letter")
                {
                    //var deleteStatus = DA.DeleteTemplatefromAssignedemployee(templateId);
                }
                else if (type == "SMS")
                {
                    var sentSMS = SendSMSbySSLGateway(templateId, subject, body, empList);
                    if (!sentSMS)
                    {
                        info.AlertMessage = CommonHelper.AlertInfo("Save operation successful but has some problem with SMS send", AlertType.Warning);
                    }
                    else
                    {
                        info.AlertMessage = CommonHelper.AlertInfo("Save operation successful with SMS send", AlertType.Success);
                    }
                }

            }
            else
            {
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Warning);
            }
            //save mail templater with employee id

            return info;
        }

       
        public static bool SendSMSbySSLGateway(int templateId, string subject, string body, string EmpList)
        {
            bool status = false;
            SmsView sms;
            try
            {
                HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendSMS", "SendSMSConfiguration");
                CommonDA commonDA = new CommonDA();
                RoomReservationBO reservationBO = new RoomReservationBO();
                RoomReservationDA reservationDA = new RoomReservationDA();
                string mainString = commonSetupBO.Description;
                string[] dataArray = mainString.Split('~');
                var smsGetway = dataArray[0];
                HMUtility hmUtility = new HMUtility();
                //Employee Block
                List<int> empIds = new List<int>();
                foreach (var s in EmpList.Split(','))
                {
                    int num;
                    if (int.TryParse(s, out num))
                        empIds.Add(num);
                }
                List<EmployeeBO> employeeBOs = new List<EmployeeBO>();
                EmployeeBO bo = new EmployeeBO();
                EmployeeDA da = new EmployeeDA();
                if (empIds.Count > 0)
                {
                    foreach (var item in empIds)
                    {
                        bo = da.GetEmployeeInfoById(item);
                        if (bo != null)
                        {
                            employeeBOs.Add(bo);
                        }
                    }
                }

                // Config Block
                if (!string.IsNullOrEmpty(mainString))
                {
                    List<string> mobileNumbers = new List<string>();

                    foreach (var item in employeeBOs)
                    {
                        //var modifiedBody = string.Empty;

                        //mobileNumbers.Add(item.PresentPhone);

                        sms = new SmsView
                        {
                            TempleteName = HMConstants.SMSTemplates.ReservationConfirmation
                        };

                        var singletoken = new Dictionary<string, string>
                        {
                        {"COMPANY", hmUtility.GetHMCompanyProfile()},
                        {"COMPANYADDRESS", hmUtility.GetHMCompanyAddress()},
                        {"ReservationNumber", reservationBO.ReservationNumber },
                        {"ArrivalDate", reservationBO.ArrivalTime.ToString()},
                        {"DepartureDate", reservationBO.DepartureTime.ToString()},
                        {"RoomType", reservationBO.RoomType },
                        {"RoomNumber", reservationBO.RoomNumber }
                        };
                        status = SmsHelper.SendSmsSingle(sms, singletoken, smsGetway, item.PresentPhone);
                    }

                    

                    for (int i = 0; i < employeeBOs.Count; i++)
                    {
                        
                        mobileNumbers.Add(employeeBOs[i].PresentPhone);
                    }

                    //var modifiedBody = string.Empty;
                    //if (templateId != 0)
                    //{
                    //    modifiedBody = commonDA.GenerateModifiedBody(templateId, employeeBOs[i]);
                    //}
                    //else
                    //{
                    //    modifiedBody = body;
                    //}
                    sms = new SmsView
                    {
                        TempleteName = HMConstants.SMSTemplates.ReservationConfirmation
                    };

                    var token = new Dictionary<string, string>
                        {
                        {"COMPANY", hmUtility.GetHMCompanyProfile()},
                        {"COMPANYADDRESS", hmUtility.GetHMCompanyAddress()},
                        {"ReservationNumber", reservationBO.ReservationNumber },
                        {"ArrivalDate", reservationBO.ArrivalTime.ToString()},
                        {"DepartureDate", reservationBO.DepartureTime.ToString()},
                        {"RoomType", reservationBO.RoomType },
                        {"RoomNumber", reservationBO.RoomNumber }
                        };
                    try
                    {
                        //Bulk Sms
                        status = SmsHelper.SendSmsBulk(sms, token, smsGetway, mobileNumbers);

                        //Single Sms
                        //status = SmsHelper.SendSmsSingle(sms, token, smsGetway, mobileNumbers);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }



        private static bool SendMail(int templateId, string subject, string body, string EmpList)
        {
            HMUtility hmUtility = new HMUtility();
            Email email;

            //employee block
            List<int> empIds = new List<int>();
            foreach (var s in EmpList.Split(','))
            {
                int num;
                if (int.TryParse(s, out num))
                    empIds.Add(num);
            }
            List<EmployeeBO> employeeBOs = new List<EmployeeBO>();
            EmployeeBO bo = new EmployeeBO();
            EmployeeDA da = new EmployeeDA();
            if (empIds.Count > 0)
            {
                foreach (var item in empIds)
                {
                    bo = da.GetEmployeeInfoById(item);
                    if (bo != null)
                    {
                        employeeBOs.Add(bo);
                    }
                }
            }

            //template block 

            

            //send mail after modify

            bool status = false;

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            CommonDA commonDA = new CommonDA();

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
            string mainString = commonSetupBO.SetupValue;

            //string urlHead1 = System.Web.Configuration.WebConfigurationManager.AppSettings["ClientURL"].ToString();

            //var Id1 = 2;

            if (!string.IsNullOrEmpty(mainString))
            {
                string[] dataArray = mainString.Split('~');
                for (int i = 0; i < employeeBOs.Count; i++)
                {
                    var modifiedBody = string.Empty;
                    if (templateId != 0)
                    {
                        modifiedBody = commonDA.GenerateModifiedBody(templateId, employeeBOs[i]);
                    }
                    else
                    {
                        modifiedBody = body;
                    }
                    email = new Email
                    {
                        From = dataArray[0],
                        Password = dataArray[1],
                        To = employeeBOs[i].OfficialEmail,
                        Host = dataArray[2],
                        Port = dataArray[3],
                        Subject = subject,
                        TempleteName = HMConstants.EmailTemplates.TemplateCustomEmail
                    };

                    var token = new Dictionary<string, string>
                    {
                    {"BODY", modifiedBody},
                    {"COMPANY", hmUtility.GetHMCompanyProfile()},
                    {"COMPANYADDRESS", hmUtility.GetHMCompanyAddress()},
                    };

                    try
                    {
                        status = EmailHelper.SendEmail(email, token);
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
        public static string ShowLetter(long id, int empId)
        {
            TemplateEmailBO BOs = new TemplateEmailBO();
            TemplateInfoDA DA = new TemplateInfoDA();
            BOs = DA.GetTemplateEmailById(id, "Letter");
            CommonDA commonDA = new CommonDA();
            List<EmployeeBO> employeeBOs = new List<EmployeeBO>();
            EmployeeBO bo = new EmployeeBO();
            EmployeeDA da = new EmployeeDA();
            if (empId > 0)
            {
                bo = da.GetEmployeeInfoById(empId);
            }
            var modifiedBody = string.Empty;
            if (BOs.TemplateId != 0)
            {
                modifiedBody = commonDA.GenerateModifiedBody(BOs.TemplateId, bo);
            }
            else
            {
                modifiedBody = BOs.TemplateBody;
            }

            HMCommonDA hmCommonDA = new HMCommonDA();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            string fileName = string.Empty, fileNamePrint = string.Empty, path = string.Empty;
            DateTime dateTime = DateTime.Now;
            if (BOs.LastModifiedDate != null)
            {
                fileName = bo.EmpId.ToString() + "_" + BOs.Id.ToString() + String.Format("{0:ddMMMyyyyHHmmssffff}", BOs.LastModifiedDate) + "_" + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";
            }
            else
                fileName = bo.EmpId.ToString() + "_" + BOs.Id.ToString() + String.Format("{0:ddMMMyyyyHHmmssffff}", BOs.CreatedDate) + "_" + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

            if (File.Exists(HttpContext.Current.Server.MapPath("../HMCommon/CustomTemplate/" + fileName)))
            {
                return "../HMCommon/CustomTemplate/" + fileName;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(modifiedBody);

                //Create our document object
                Document Doc = new Document(PageSize.A4);

                //Create our file stream
                using (FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("../HMCommon/CustomTemplate/" + fileName), FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    //Bind PDF writer to document and stream
                    PdfWriter writer = PdfWriter.GetInstance(Doc, fs);

                    //Open document for writing
                    Doc.Open();

                    //Add a page
                    Doc.NewPage();

                    MemoryStream msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(sb.ToString()));
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, Doc, msHtml, null, Encoding.UTF8, new UnicodeFontFactory());

                    Doc.CloseDocument();
                    //Close the PDF
                    //Doc.Close();
                }
                return "../HMCommon/CustomTemplate/" + fileName;
            }
        }

    }
    
}