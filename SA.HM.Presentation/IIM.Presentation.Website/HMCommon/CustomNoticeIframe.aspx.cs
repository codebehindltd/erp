using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Text;
using System.IO;
using iTextSharp.text.html;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp.tool.xml;
using ListItem = System.Web.UI.WebControls.ListItem;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class CustomNoticeIframe : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDepartment();
            }
        }
        //private void LoadGetAssignTo()
        //{
        //    EmployeeDA EmpDA = new EmployeeDA();
        //    List<EmployeeBO> EmpBO = new List<EmployeeBO>();
        //    EmpBO = EmpDA.GetEmployeeInfo();

        //    ddlAssignTo.DataSource = EmpBO;
        //    ddlAssignTo.DataTextField = "DisplayName";
        //    ddlAssignTo.DataValueField = "EmpId";
        //    ddlAssignTo.DataBind();

        //}
        private void LoadDepartment()
        {
            DepartmentDA DA = new DepartmentDA();
            List<DepartmentBO> depBO = new List<DepartmentBO>();
            depBO = DA.GetDepartmentInfo();

            ddlEmpDepartment.DataSource = depBO;
            ddlEmpDepartment.DataTextField = "Name";
            ddlEmpDepartment.DataValueField = "DepartmentId";
            ddlEmpDepartment.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlEmpDepartment.Items.Insert(0, item);

        }
        private static void SavePdf(CustomNoticeBO notice, int Id)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            HMUtility hmUtility = new HMUtility();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            byte[] bytes;
            StringBuilder sb = new StringBuilder();
            sb.Append(notice.Content);
            StringReader sr = new StringReader(notice.Content);

            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                htmlparser.Parse(sr);
                pdfDoc.Close();

                bytes = memoryStream.ToArray();
                memoryStream.Close();


                string fileName = string.Empty, fileNamePrint = string.Empty;
                DateTime dateTime = DateTime.Now;
                fileName = notice.NoticeName + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/HMCommon/CustomNotice/" + fileName), FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();

                List<DocumentsBO> docList = new List<DocumentsBO>();
                DocumentsBO document = new DocumentsBO();
                DocumentsDA docDA = new DocumentsDA();

                document.OwnerId = Id;
                document.Name = fileName;
                document.Path = @"/HMCommon/CustomNotice/";
                document.Extention = ".pdf";
                document.DocumentType = "Image";
                document.DocumentCategory = "CustomNoticeDocument";
                document.CreatedBy = userInformationBO.UserInfoId;
                docList.Add(document);

                List<DocumentsBO> predocList = docDA.GetDocumentsInfoByDocCategoryAndOwnerId("CustomNoticeDocument", Id);
                Boolean status = false;

                if (predocList.Count == 0)
                {
                    status = docDA.SaveDocumentsInfo(docList);
                }
                else
                {
                    document.DocumentId = predocList[0].DocumentId;
                    status = docDA.UpdateDocumentsInfoByOwnerId(document);
                }
            }
        }
        private static void SavePdfTest(CustomNoticeBO notice, int Id)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append("<body>");
            sb.Append(notice.Content);
            //sb.Append("</body>");
            HMCommonDA hmCommonDA = new HMCommonDA();
            HMUtility hmUtility = new HMUtility();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            string fileName = string.Empty, fileNamePrint = string.Empty;
            DateTime dateTime = DateTime.Now;
            fileName = notice.NoticeName + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";
            //Create our document object
            Document Doc = new Document(PageSize.A4);


            //Create our file stream
            using (FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/HMCommon/CustomNotice/" + fileName), FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                //Bind PDF writer to document and stream
                PdfWriter writer = PdfWriter.GetInstance(Doc, fs);

                //Open document for writing
                Doc.Open();


                //Add a page
                Doc.NewPage();

                MemoryStream msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(sb.ToString()));
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, Doc, msHtml, null, Encoding.UTF8, new UnicodeFontFactory());

                //Close the PDF
                Doc.Close();
            }
            List<DocumentsBO> docList = new List<DocumentsBO>();
            DocumentsBO document = new DocumentsBO();
            DocumentsDA docDA = new DocumentsDA();

            document.OwnerId = Id;
            document.Name = fileName;
            document.Path = @"/HMCommon/CustomNotice/";
            document.Extention = ".pdf";
            document.DocumentType = "Image";
            document.DocumentCategory = "CustomNoticeDocument";
            document.CreatedBy = userInformationBO.UserInfoId;
            docList.Add(document);

            List<DocumentsBO> predocList = docDA.GetDocumentsInfoByDocCategoryAndOwnerId("CustomNoticeDocument", Id);
            Boolean status = false;

            if (predocList.Count == 0)
            {
                status = docDA.SaveDocumentsInfo(docList);
            }
            else
            {
                document.DocumentId = predocList[0].DocumentId;
                status = docDA.UpdateDocumentsInfoByOwnerId(document);
            }
        }
            //private static void SavePdfTest(CustomNoticeBO notice, int Id)
            //{
            //    Document doc = new Document(PageSize.LETTER);
            //    HMCommonDA hmCommonDA = new HMCommonDA();
            //    HMUtility hmUtility = new HMUtility();

            //    UserInformationBO userInformationBO = new UserInformationBO();
            //    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            //    string fileName = string.Empty, fileNamePrint = string.Empty;
            //    DateTime dateTime = DateTime.Now;
            //    fileName = notice.NoticeName + String.Format("{0:ddMMMyyyyHHmmssffff}", dateTime) + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

            //    using (FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/HMCommon/CustomNotice/" + fileName), FileMode.Create, FileAccess.Write, FileShare.Read))
            //    {
            //        PdfWriter.GetInstance(doc, fs);
            //        doc.Open();

            //        //Sample HTML
            //        StringBuilder stringBuilder = new StringBuilder();
            //        stringBuilder.Append(notice.Content);
            //        string arialuniTff = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), HttpContext.Current.Server.MapPath("ARIALUNI.TTF"));
            //        //Register the font with iTextSharp
            //        iTextSharp.text.FontFactory.Register(arialuniTff);

            //        //Create a new stylesheet
            //        iTextSharp.text.html.simpleparser.StyleSheet ST = new iTextSharp.text.html.simpleparser.StyleSheet();
            //        //Set the default body font to our registered font's internal name
            //        ST.LoadTagStyle(HtmlTags.BODY, HtmlTags.FONT, "Arial Unicode MS");
            //        //Set the default encoding to support Unicode characters

            //        List<IElement> list = HTMLWorker.ParseToList(new StringReader(stringBuilder.ToString()), ST);

            //        //Loop through each element, don't bother wrapping in P tags
            //        foreach (var element in list)
            //        {
            //            doc.Add(element);
            //        }
            //        doc.Close();
            //    }
            //}

        //public void ConvertHtmlToImage(CustomNoticeBO notice, int Id)
        //{
        //    Bitmap m_Bitmap = new Bitmap(400, 600);
        //    PointF point = new PointF(0, 0);
        //    SizeF maxSize = new System.Drawing.SizeF(500, 500);
        //    HtmlRenderer.HtmlRender.Render(Graphics.FromImage(m_Bitmap),
        //                                            "<html><body><p>This is a shitty html code</p>"
        //                                            + "<p>This is another html line</p></body>",
        //                                             point, maxSize);

            //    m_Bitmap.Save(@"C:\Test.png", ImageFormat.Png);
            //}

        [WebMethod]
        public static CustomNoticeBO GetNoticeInfoById(int Id)
        {
            CustomNoticeBO noticeBO = new CustomNoticeBO();
            CustomNoticeDA noticeDA = new CustomNoticeDA();
            noticeBO = noticeDA.GetNoticeInfoById(Id);
            noticeBO.EmployeeList = noticeDA.GetNoticeAssignedEmployeeById(noticeBO.Id);
            return noticeBO;
        }
        [WebMethod]
        public static ReturnInfo SaveOrUpdateNotice(CustomNoticeBO notice, string EmpList)
        {
            ReturnInfo info = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            long outId = 0;
            CustomNoticeDA noticeDA = new CustomNoticeDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            notice.CreatedBy = userInformationBO.UserInfoId;

            try
            {
                status = noticeDA.SaveOrUpdateNotice(notice, EmpList, out outId);

                if (status)
                {

                    SavePdfTest(notice, Convert.ToInt32(outId));
                    info.IsSuccess = true;
                    if (notice.Id == 0)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.SMTask.ToString(), outId, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        info.Data = 0;
                    }
                    else
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.SMTask.ToString(), notice.Id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealStage));
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }
                else
                {
                    info.IsSuccess = false;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }

            }
            catch (Exception ex)
            {
                info.IsSuccess = false;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }
            return info;
        }

        [WebMethod]
        public static List<EmployeeBO> LoadEmployeeByGroup(int groupId)
        {
            EmployeeDA DA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = DA.GetEmployeeByDepartment(groupId);
            return empList;
        }
        [WebMethod]
        public static List<EmployeeBO> LoadAllEmployee()
        {
            EmployeeDA DA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = DA.GetEmployeeInfo();
            return empList;
        }
        [WebMethod]
        public static List<EmployeeBO> GetEmployeeInformationAutoSearch(string searchString)
        {
            EmployeeDA DA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = DA.GetEmpInformationForAutoSearch(searchString);
            return empList;
        }
        public class UnicodeFontFactory : FontFactoryImp
        {
            private static readonly string FontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
          HttpContext.Current.Server.MapPath("CustomNoticeFont/ARIALUNI.TTF"));

            private readonly BaseFont _baseFont;

            public UnicodeFontFactory()
            {
                _baseFont = BaseFont.CreateFont(FontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            }

            public override iTextSharp.text.Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color, bool cached)
            {
                return new iTextSharp.text.Font(_baseFont, size, style, color);
            }
        }
    }
}