using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.SupportAndTicket;
using HotelManagement.Data.TaskManagment;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.SupportAndTicket;
using HotelManagement.Entity.TaskManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SupportAndTicket
{
    public partial class SupportCallImplementation :BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadCaseOwner();
            }
        }
        private void LoadCaseOwner()
        {
            List<STSupportNCaseSetupInfoBO> SupportNCaseSetupList = new List<STSupportNCaseSetupInfoBO>();
            SupportNCaseSetupDA DA = new SupportNCaseSetupDA();

            SupportNCaseSetupList = DA.SupportNCaseSetupInfoBySetupType("Case");

            ddlCase.DataSource = SupportNCaseSetupList;
            ddlCase.DataTextField = "Name";
            ddlCase.DataValueField = "Id";
            ddlCase.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCase.Items.Insert(0, item);

        }
        [WebMethod]
        public static GridViewDataNPaging<STSupportBO, GridPaging> GetSupportCallBySearchCriteria(int clientId, int caseId, string caseNumber, string fromDate, string toDate, string status, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<STSupportBO, GridPaging> myGridData = new GridViewDataNPaging<STSupportBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            List<STSupportBO> supportBO = new List<STSupportBO>();
            SupportCallImplementationDA DA = new SupportCallImplementationDA();
            if (clientId == 0 && caseId == 0 && string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate) && string.IsNullOrEmpty(caseNumber) && string.IsNullOrEmpty(status))
            {
                fromDate = DateTime.Now.ToShortDateString();
                toDate = DateTime.Now.ToShortDateString();
            }
            supportBO = DA.GetSupportCallInformationForGridPaging(clientId, caseId, caseNumber, fromDate, toDate, "", userInformationBO.GridViewPageSize, pageNumber, out totalRecords).Where(x => x.SupportStatus != "Done").ToList();
            supportBO = supportBO.Where(x => x.TaskStatus != "Done").ToList();
            myGridData.GridPagingProcessing(supportBO, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static List<GuestCompanyBO> ClientSearch(string searchTerm)
        { 

            List<GuestCompanyBO> companyInfo = new List<GuestCompanyBO>();
            GuestCompanyDA itemDa = new GuestCompanyDA();
            companyInfo = itemDa.GetGuestCompanyInfo(searchTerm);

            return companyInfo;
        }
        [WebMethod]
        public static string LoadVoucherDocumentById(long id)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("SupportAndTicketDoc", id);
            docList = new HMCommonDA().GetDocumentListWithIcon(docList);

            List<SMTask> taskList = new AssignTaskDA().GetAllTaskBySourceNameId((int)id);

            List<DocumentsBO> taskAssignDocuments = new List<DocumentsBO>();
            List<DocumentsBO> taskFeedbackDocuments = new List<DocumentsBO>();
            foreach (SMTask task in taskList)
            {
                taskAssignDocuments = new DocumentsDA().GetDocumentsByUserTypeAndUserId("TaskAssignDocuments", (int)task.Id);
                taskAssignDocuments = new HMCommonDA().GetDocumentListWithIcon(taskAssignDocuments);

                taskFeedbackDocuments = new DocumentsDA().GetDocumentsByUserTypeAndUserId("TaskFeedbackDocuments", (int)task.Id);
                taskFeedbackDocuments = new HMCommonDA().GetDocumentListWithIcon(taskFeedbackDocuments);
            }

            string strTable = "";
            if (docList.Count > 0)
            {
                strTable += "<div style='color: Black; background-color: #fff; width:750px; min-height:300px; float: left;'>";
                strTable += "<div style='width: 100 %; border-bottom: 1px solid #000; font-weight: bold;'>Documents from Call Center</div>";
                //int counter = 0;
                foreach (DocumentsBO dr in docList)
                {
                    if (dr.Extention == ".jpg" || dr.Extention == ".png")
                    {
                        string ImgSource = dr.Path + dr.Name;
                        SupportCallImplementation supportCallInformation = new SupportCallImplementation();

                        if (supportCallInformation.IsFileExist(ImgSource))
                        {
                            strTable += "<div style=' width:250px; height:250px; text-align:center; float:left;padding:30px'>";
                            strTable += "<a style='color:#333333;' target='_blank' href='" + ImgSource + "'>";
                            strTable += "<img style='width: 200px; height: 200px;' src='" + ImgSource + "'  alt='Image preview' title='Image Preview'/>";
                            strTable += "<span>'" + dr.Name + "'</span>";
                            strTable += "</a>";
                            strTable += "</div>";
                        }
                        else
                        {
                            strTable += "<div style=' width:250px; height:250px; text-align:center; float:left;'>";
                            //strTable += "<a style='color:#333333;' target='_blank' href='#'>";
                            //strTable += "<img style='width: 200px; height: 200px;' src='" + ImgSource + "'  alt='Image preview' />";
                            strTable += "<div style=' width:250px; height:200px; padding:10px'><svg version='1.0' id='Layer_1' xmlns:dc='http://purl.org/dc/elements/1.1/' xmlns:cc='http://creativecommons.org/ns#' xmlns:rdf='http://www.w3.org/1999/02/22-rdf-syntax-ns#' xmlns:svg='http://www.w3.org/2000/svg' xmlns:sodipodi='http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd' xmlns:inkscape='http://www.inkscape.org/namespaces/inkscape' inkscape:output_extension='org.inkscape.output.svg.inkscape' sodipodi:version='0.32' sodipodi:docname='No pub.svg' inkscape:version='0.46' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' x='0px' y='0px' width='220px' height='200px' viewBox='0 0 500 500' enable-background='new 0 0 500 500' xml:space='preserve'><defs><inkscape:perspective id='perspective54' inkscape:vp_z='500 : 250 : 1' inkscape:vp_y='0 : 1000 : 0' inkscape:vp_x='0 : 250 : 1' sodipodi:type='inkscape:persp3d' inkscape:persp3d-origin='250 : 166.66667 : 1'></inkscape:perspective><inkscape:perspective id='perspective2491' inkscape:vp_z='744.09448 : 526.18109 : 1' inkscape:vp_y='0 : 1000 : 0' inkscape:vp_x='0 : 526.18109 : 1' sodipodi:type='inkscape:persp3d' inkscape:persp3d-origin='372.04724 : 350.78739 : 1'></inkscape:perspective></defs><sodipodi:namedview id='base' inkscape:cy='173.43403' inkscape:cx='250' inkscape:zoom='1.3067333' showgrid='false' pagecolor='#ffffff' bordercolor='#666666' borderopacity='1.0' objecttolerance='10.0' gridtolerance='10.0' guidetolerance='10.0' inkscape:current-layer='Layer_1' inkscape:window-y='-8' inkscape:window-x='-8' inkscape:pageopacity='0.0' inkscape:pageshadow='2' inkscape:window-width='1680' inkscape:window-height='1000'></sodipodi:namedview><circle id='circle3' fill='#FFFFFF' stroke='#000000' cx='249.5' cy='249.5' r='205'/><path id='path2497' sodipodi:nodetypes='cccccccccccccccccccsssssc' d='M307.188,128.5c-58.837,42.757-120.586,83.151-186.938,113.563c-12.729,5.414-12.67,5.267-5.75,11.469c33.161,29.168,58.497,65.065,82,102.094c70.473-34.586,131.75-84.896,191.813-134.75c10.113-10.145,10.391-10.937,5.469-16.188C368.46,175.813,338.789,150.394,307.188,128.5z M374,243.5c-55.594,42.186-112.22,86.347-175.813,116.219l-4.625,1.906c-33.012-21.727-61.166-49.727-91.438-74.938C89.48,296.115,167.834,364.842,187,379.5c32.418-13.877,63.099-30.094,93.387-48.197l108.832-76.334L374,243.5z M120.094,267.563c-4.842,2.146-13.541,6.965-15.469,8.563l-2.5,2.063l2.906,2.625c1.602,1.437,8.927,8.329,16.25,15.313c17.096,16.303,36.074,32.656,55.656,47.969c8.562,6.695,10.5,7.986,10.5,6.969c0-3.563-19.934-32.285-32.938-47.469c-2.352-2.746-4.974-5.896-5.844-7C142.776,289.137,121.105,267.114,120.094,267.563z'/><g id='g5'><g id='g7'><g id='g9'><g id='g11'></g><g id='g15'><g id='g17'></g><g id='g21'></g><g id='g25'></g><g id='g29'></g></g></g><g id='g33'><rect id='rect35' x='273.441' y='89.786' fill='#FFFFFF' width='13.187' height='32.619'/></g></g><g id='g37'><line id='line39' fill='none' stroke='#FFFFFF' stroke-width='55' stroke-miterlimit='10' x1='393.35' y1='392.509' x2='110.112' y2='109.275'/><g id='g41'><path id='path43' fill='none' stroke='#FF0000' stroke-width='49' stroke-miterlimit='10' d='M250.029,446.658c-107.846,0-195.276-87.428-195.276-195.271c0-107.848,87.43-195.277,195.276-195.277c107.845,0,195.274,87.429,195.274,195.277C445.303,359.23,357.874,446.658,250.029,446.658z'/><path id='path45' fill='none' stroke='#000000' stroke-width='49' stroke-miterlimit='10' d='M250.029,251.387'/></g><line id='line47' fill='none' stroke='#FF0000' stroke-width='51.12' stroke-miterlimit='10' x1='392.868' y1='392.191' x2='109.63' y2='108.957'/></g></g></svg>";
                            strTable += "<span>'" + dr.Name + "'</span></div>";
                            //strTable += "</a>";
                            strTable += "</div>";
                        }

                    }
                    else
                    {
                        SupportCallImplementation supportCallInformation = new SupportCallImplementation();
                        string ImgSource = dr.Path + dr.Name;
                        if (!supportCallInformation.IsFileExist(ImgSource))
                        {
                            strTable += "<div style=' width:250px; height:250px; text-align:center; float:left;'>";
                            //strTable += "<a style='color:#333333;' target='_blank' href='#'>";
                            //strTable += "<img style='width: 200px; height: 200px;' src='" + ImgSource + "'  alt='Image preview' />";
                            strTable += "<div style=' width:250px; height:200px; padding:10px'><svg version='1.0' id='Layer_1' xmlns:dc='http://purl.org/dc/elements/1.1/' xmlns:cc='http://creativecommons.org/ns#' xmlns:rdf='http://www.w3.org/1999/02/22-rdf-syntax-ns#' xmlns:svg='http://www.w3.org/2000/svg' xmlns:sodipodi='http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd' xmlns:inkscape='http://www.inkscape.org/namespaces/inkscape' inkscape:output_extension='org.inkscape.output.svg.inkscape' sodipodi:version='0.32' sodipodi:docname='No pub.svg' inkscape:version='0.46' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' x='0px' y='0px' width='220px' height='200px' viewBox='0 0 500 500' enable-background='new 0 0 500 500' xml:space='preserve'><defs><inkscape:perspective id='perspective54' inkscape:vp_z='500 : 250 : 1' inkscape:vp_y='0 : 1000 : 0' inkscape:vp_x='0 : 250 : 1' sodipodi:type='inkscape:persp3d' inkscape:persp3d-origin='250 : 166.66667 : 1'></inkscape:perspective><inkscape:perspective id='perspective2491' inkscape:vp_z='744.09448 : 526.18109 : 1' inkscape:vp_y='0 : 1000 : 0' inkscape:vp_x='0 : 526.18109 : 1' sodipodi:type='inkscape:persp3d' inkscape:persp3d-origin='372.04724 : 350.78739 : 1'></inkscape:perspective></defs><sodipodi:namedview id='base' inkscape:cy='173.43403' inkscape:cx='250' inkscape:zoom='1.3067333' showgrid='false' pagecolor='#ffffff' bordercolor='#666666' borderopacity='1.0' objecttolerance='10.0' gridtolerance='10.0' guidetolerance='10.0' inkscape:current-layer='Layer_1' inkscape:window-y='-8' inkscape:window-x='-8' inkscape:pageopacity='0.0' inkscape:pageshadow='2' inkscape:window-width='1680' inkscape:window-height='1000'></sodipodi:namedview><circle id='circle3' fill='#FFFFFF' stroke='#000000' cx='249.5' cy='249.5' r='205'/><path id='path2497' sodipodi:nodetypes='cccccccccccccccccccsssssc' d='M307.188,128.5c-58.837,42.757-120.586,83.151-186.938,113.563c-12.729,5.414-12.67,5.267-5.75,11.469c33.161,29.168,58.497,65.065,82,102.094c70.473-34.586,131.75-84.896,191.813-134.75c10.113-10.145,10.391-10.937,5.469-16.188C368.46,175.813,338.789,150.394,307.188,128.5z M374,243.5c-55.594,42.186-112.22,86.347-175.813,116.219l-4.625,1.906c-33.012-21.727-61.166-49.727-91.438-74.938C89.48,296.115,167.834,364.842,187,379.5c32.418-13.877,63.099-30.094,93.387-48.197l108.832-76.334L374,243.5z M120.094,267.563c-4.842,2.146-13.541,6.965-15.469,8.563l-2.5,2.063l2.906,2.625c1.602,1.437,8.927,8.329,16.25,15.313c17.096,16.303,36.074,32.656,55.656,47.969c8.562,6.695,10.5,7.986,10.5,6.969c0-3.563-19.934-32.285-32.938-47.469c-2.352-2.746-4.974-5.896-5.844-7C142.776,289.137,121.105,267.114,120.094,267.563z'/><g id='g5'><g id='g7'><g id='g9'><g id='g11'></g><g id='g15'><g id='g17'></g><g id='g21'></g><g id='g25'></g><g id='g29'></g></g></g><g id='g33'><rect id='rect35' x='273.441' y='89.786' fill='#FFFFFF' width='13.187' height='32.619'/></g></g><g id='g37'><line id='line39' fill='none' stroke='#FFFFFF' stroke-width='55' stroke-miterlimit='10' x1='393.35' y1='392.509' x2='110.112' y2='109.275'/><g id='g41'><path id='path43' fill='none' stroke='#FF0000' stroke-width='49' stroke-miterlimit='10' d='M250.029,446.658c-107.846,0-195.276-87.428-195.276-195.271c0-107.848,87.43-195.277,195.276-195.277c107.845,0,195.274,87.429,195.274,195.277C445.303,359.23,357.874,446.658,250.029,446.658z'/><path id='path45' fill='none' stroke='#000000' stroke-width='49' stroke-miterlimit='10' d='M250.029,251.387'/></g><line id='line47' fill='none' stroke='#FF0000' stroke-width='51.12' stroke-miterlimit='10' x1='392.868' y1='392.191' x2='109.63' y2='108.957'/></g></g></svg>";
                            strTable += "<span>'" + dr.Name + "'</span></div>";
                            //strTable += "</a>";
                            strTable += "</div>";
                        }
                        else
                        {
                            strTable += "<div style=' width:250px; height:250px; text-align:center; float:left;padding:30px'>";
                            strTable += "<a style='color:#333333;' target='_blank' href='" + ImgSource + "'>";

                            string modifiedFileName = string.Empty;
                            if (dr.Name.Length > 12)
                            {
                                modifiedFileName = dr.Name.Substring(0, 12);
                                strTable += "<img style='width: 200px; height: 200px;' src='" + dr.IconImage + "' alt='" + dr.Name + "' title='" + dr.Name + "'/>";
                            }
                            else
                            {
                                modifiedFileName = dr.Name;
                            }

                            strTable += "<span>'" + modifiedFileName + "'</span>";
                            strTable += "</a>";
                            strTable += "</div>";
                        }
                    }
                }
                strTable += "</div>";
            }

            if (taskAssignDocuments.Count > 0)
            {
                strTable += "<div style='color: Black; background-color: #fff;width:750px; min-height:300px; float: left;'>";
                strTable += "<div style='width: 100 %; border-bottom: 1px solid #000; font-weight: bold;'>Documents from Implementation</div>";
                //int counter = 0;
                foreach (DocumentsBO dr in taskAssignDocuments)
                {
                    if (dr.Extention == ".jpg" || dr.Extention == ".png")
                    {
                        string ImgSource = dr.Path + dr.Name;

                        SupportCallImplementation supportCallInformation = new SupportCallImplementation();
                        if (supportCallInformation.IsFileExist(ImgSource))
                        {
                            strTable += "<div style=' width:250px; height:250px; text-align:center; float:left;padding:30px'>";
                            strTable += "<a style='color:#333333;' target='_blank' href='" + ImgSource + "'>";
                            strTable += "<img style='width: 200px; height: 200px;' src='" + ImgSource + "'  alt='Image preview' title='Image Preview'/>";
                            strTable += "<span>'" + dr.Name + "'</span>";
                            strTable += "</a>";
                            strTable += "</div>";
                        }
                        else
                        {
                            strTable += "<div style=' width:250px; height:250px; text-align:center; float:left;'>";
                            //strTable += "<a style='color:#333333;' target='_blank' href='#'>";
                            //strTable += "<img style='width: 200px; height: 200px;' src='" + ImgSource + "'  alt='Image preview' />";
                            strTable += "<div style=' width:250px; height:200px; padding:10px;'><svg version='1.0' id='Layer_1' xmlns:dc='http://purl.org/dc/elements/1.1/' xmlns:cc='http://creativecommons.org/ns#' xmlns:rdf='http://www.w3.org/1999/02/22-rdf-syntax-ns#' xmlns:svg='http://www.w3.org/2000/svg' xmlns:sodipodi='http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd' xmlns:inkscape='http://www.inkscape.org/namespaces/inkscape' inkscape:output_extension='org.inkscape.output.svg.inkscape' sodipodi:version='0.32' sodipodi:docname='No pub.svg' inkscape:version='0.46' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' x='0px' y='0px' width='220px' height='200px' viewBox='0 0 500 500' enable-background='new 0 0 500 500' xml:space='preserve'><defs><inkscape:perspective id='perspective54' inkscape:vp_z='500 : 250 : 1' inkscape:vp_y='0 : 1000 : 0' inkscape:vp_x='0 : 250 : 1' sodipodi:type='inkscape:persp3d' inkscape:persp3d-origin='250 : 166.66667 : 1'></inkscape:perspective><inkscape:perspective id='perspective2491' inkscape:vp_z='744.09448 : 526.18109 : 1' inkscape:vp_y='0 : 1000 : 0' inkscape:vp_x='0 : 526.18109 : 1' sodipodi:type='inkscape:persp3d' inkscape:persp3d-origin='372.04724 : 350.78739 : 1'></inkscape:perspective></defs><sodipodi:namedview id='base' inkscape:cy='173.43403' inkscape:cx='250' inkscape:zoom='1.3067333' showgrid='false' pagecolor='#ffffff' bordercolor='#666666' borderopacity='1.0' objecttolerance='10.0' gridtolerance='10.0' guidetolerance='10.0' inkscape:current-layer='Layer_1' inkscape:window-y='-8' inkscape:window-x='-8' inkscape:pageopacity='0.0' inkscape:pageshadow='2' inkscape:window-width='1680' inkscape:window-height='1000'></sodipodi:namedview><circle id='circle3' fill='#FFFFFF' stroke='#000000' cx='249.5' cy='249.5' r='205'/><path id='path2497' sodipodi:nodetypes='cccccccccccccccccccsssssc' d='M307.188,128.5c-58.837,42.757-120.586,83.151-186.938,113.563c-12.729,5.414-12.67,5.267-5.75,11.469c33.161,29.168,58.497,65.065,82,102.094c70.473-34.586,131.75-84.896,191.813-134.75c10.113-10.145,10.391-10.937,5.469-16.188C368.46,175.813,338.789,150.394,307.188,128.5z M374,243.5c-55.594,42.186-112.22,86.347-175.813,116.219l-4.625,1.906c-33.012-21.727-61.166-49.727-91.438-74.938C89.48,296.115,167.834,364.842,187,379.5c32.418-13.877,63.099-30.094,93.387-48.197l108.832-76.334L374,243.5z M120.094,267.563c-4.842,2.146-13.541,6.965-15.469,8.563l-2.5,2.063l2.906,2.625c1.602,1.437,8.927,8.329,16.25,15.313c17.096,16.303,36.074,32.656,55.656,47.969c8.562,6.695,10.5,7.986,10.5,6.969c0-3.563-19.934-32.285-32.938-47.469c-2.352-2.746-4.974-5.896-5.844-7C142.776,289.137,121.105,267.114,120.094,267.563z'/><g id='g5'><g id='g7'><g id='g9'><g id='g11'></g><g id='g15'><g id='g17'></g><g id='g21'></g><g id='g25'></g><g id='g29'></g></g></g><g id='g33'><rect id='rect35' x='273.441' y='89.786' fill='#FFFFFF' width='13.187' height='32.619'/></g></g><g id='g37'><line id='line39' fill='none' stroke='#FFFFFF' stroke-width='55' stroke-miterlimit='10' x1='393.35' y1='392.509' x2='110.112' y2='109.275'/><g id='g41'><path id='path43' fill='none' stroke='#FF0000' stroke-width='49' stroke-miterlimit='10' d='M250.029,446.658c-107.846,0-195.276-87.428-195.276-195.271c0-107.848,87.43-195.277,195.276-195.277c107.845,0,195.274,87.429,195.274,195.277C445.303,359.23,357.874,446.658,250.029,446.658z'/><path id='path45' fill='none' stroke='#000000' stroke-width='49' stroke-miterlimit='10' d='M250.029,251.387'/></g><line id='line47' fill='none' stroke='#FF0000' stroke-width='51.12' stroke-miterlimit='10' x1='392.868' y1='392.191' x2='109.63' y2='108.957'/></g></g></svg>";
                            strTable += "<span>'" + dr.Name + "'</span></div>";
                            //strTable += "</a>";
                            strTable += "</div>";
                        }

                    }
                    else
                    {
                        string ImgSource = dr.Path + dr.Name;

                        SupportCallImplementation supportCallInformation = new SupportCallImplementation();
                        if (!supportCallInformation.IsFileExist(ImgSource))
                        {
                            strTable += "<div style=' width:250px; height:250px; text-align:center; float:left;'>";
                            //strTable += "<a style='color:#333333;' target='_blank' href='#'>";
                            //strTable += "<img style='width: 200px; height: 200px;' src='" + ImgSource + "'  alt='Image preview' />";
                            strTable += "<div style=' width:250px; height:200px; padding:10px;'><svg version='1.0' id='Layer_1' xmlns:dc='http://purl.org/dc/elements/1.1/' xmlns:cc='http://creativecommons.org/ns#' xmlns:rdf='http://www.w3.org/1999/02/22-rdf-syntax-ns#' xmlns:svg='http://www.w3.org/2000/svg' xmlns:sodipodi='http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd' xmlns:inkscape='http://www.inkscape.org/namespaces/inkscape' inkscape:output_extension='org.inkscape.output.svg.inkscape' sodipodi:version='0.32' sodipodi:docname='No pub.svg' inkscape:version='0.46' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' x='0px' y='0px' width='220px' height='200px' viewBox='0 0 500 500' enable-background='new 0 0 500 500' xml:space='preserve'><defs><inkscape:perspective id='perspective54' inkscape:vp_z='500 : 250 : 1' inkscape:vp_y='0 : 1000 : 0' inkscape:vp_x='0 : 250 : 1' sodipodi:type='inkscape:persp3d' inkscape:persp3d-origin='250 : 166.66667 : 1'></inkscape:perspective><inkscape:perspective id='perspective2491' inkscape:vp_z='744.09448 : 526.18109 : 1' inkscape:vp_y='0 : 1000 : 0' inkscape:vp_x='0 : 526.18109 : 1' sodipodi:type='inkscape:persp3d' inkscape:persp3d-origin='372.04724 : 350.78739 : 1'></inkscape:perspective></defs><sodipodi:namedview id='base' inkscape:cy='173.43403' inkscape:cx='250' inkscape:zoom='1.3067333' showgrid='false' pagecolor='#ffffff' bordercolor='#666666' borderopacity='1.0' objecttolerance='10.0' gridtolerance='10.0' guidetolerance='10.0' inkscape:current-layer='Layer_1' inkscape:window-y='-8' inkscape:window-x='-8' inkscape:pageopacity='0.0' inkscape:pageshadow='2' inkscape:window-width='1680' inkscape:window-height='1000'></sodipodi:namedview><circle id='circle3' fill='#FFFFFF' stroke='#000000' cx='249.5' cy='249.5' r='205'/><path id='path2497' sodipodi:nodetypes='cccccccccccccccccccsssssc' d='M307.188,128.5c-58.837,42.757-120.586,83.151-186.938,113.563c-12.729,5.414-12.67,5.267-5.75,11.469c33.161,29.168,58.497,65.065,82,102.094c70.473-34.586,131.75-84.896,191.813-134.75c10.113-10.145,10.391-10.937,5.469-16.188C368.46,175.813,338.789,150.394,307.188,128.5z M374,243.5c-55.594,42.186-112.22,86.347-175.813,116.219l-4.625,1.906c-33.012-21.727-61.166-49.727-91.438-74.938C89.48,296.115,167.834,364.842,187,379.5c32.418-13.877,63.099-30.094,93.387-48.197l108.832-76.334L374,243.5z M120.094,267.563c-4.842,2.146-13.541,6.965-15.469,8.563l-2.5,2.063l2.906,2.625c1.602,1.437,8.927,8.329,16.25,15.313c17.096,16.303,36.074,32.656,55.656,47.969c8.562,6.695,10.5,7.986,10.5,6.969c0-3.563-19.934-32.285-32.938-47.469c-2.352-2.746-4.974-5.896-5.844-7C142.776,289.137,121.105,267.114,120.094,267.563z'/><g id='g5'><g id='g7'><g id='g9'><g id='g11'></g><g id='g15'><g id='g17'></g><g id='g21'></g><g id='g25'></g><g id='g29'></g></g></g><g id='g33'><rect id='rect35' x='273.441' y='89.786' fill='#FFFFFF' width='13.187' height='32.619'/></g></g><g id='g37'><line id='line39' fill='none' stroke='#FFFFFF' stroke-width='55' stroke-miterlimit='10' x1='393.35' y1='392.509' x2='110.112' y2='109.275'/><g id='g41'><path id='path43' fill='none' stroke='#FF0000' stroke-width='49' stroke-miterlimit='10' d='M250.029,446.658c-107.846,0-195.276-87.428-195.276-195.271c0-107.848,87.43-195.277,195.276-195.277c107.845,0,195.274,87.429,195.274,195.277C445.303,359.23,357.874,446.658,250.029,446.658z'/><path id='path45' fill='none' stroke='#000000' stroke-width='49' stroke-miterlimit='10' d='M250.029,251.387'/></g><line id='line47' fill='none' stroke='#FF0000' stroke-width='51.12' stroke-miterlimit='10' x1='392.868' y1='392.191' x2='109.63' y2='108.957'/></g></g></svg>";
                            strTable += "<span>'" + dr.Name + "'</span></div>";
                            //strTable += "</a>";
                            strTable += "</div>";
                        }
                        else
                        {
                            strTable += "<div style=' width:250px; height:250px; text-align:center; float:left;padding:30px'>";
                            strTable += "<a style='color:#333333;' target='_blank' href='" + ImgSource + "'>";

                            string modifiedFileName = string.Empty;
                            if (dr.Name.Length > 12)
                            {
                                modifiedFileName = dr.Name.Substring(0, 12);
                                strTable += "<img style='width: 200px; height: 200px;' src='" + dr.IconImage + "' alt='" + dr.Name + "' title='" + dr.Name + "'/>";
                            }
                            else
                            {
                                modifiedFileName = dr.Name;
                            }

                            strTable += "<span>'" + modifiedFileName + "'</span>";
                            strTable += "</a>";
                            strTable += "</div>";
                        }
                    }
                }
                strTable += "</div>";
            }

            if (taskFeedbackDocuments.Count > 0)
            {
                strTable += "<div style='color: Black; background-color: #fff;width:750px; min-height:300px; float: left;'>";
                strTable += "<div style='width: 100 %; border-bottom: 1px solid #000; font-weight: bold;'>Documents from Feedback</div>";
                foreach (DocumentsBO dr in taskFeedbackDocuments)
                {
                    if (dr.Extention == ".jpg" || dr.Extention == ".png")
                    {
                        string ImgSource = dr.Path + dr.Name;
                        SupportCallImplementation supportCallInformation = new SupportCallImplementation();

                        if (supportCallInformation.IsFileExist(ImgSource))
                        {
                            strTable += "<div style=' width:250px; height:250px; text-align:center; float:left;padding:30px'>";
                            strTable += "<a style='color:#333333;' target='_blank' href='" + ImgSource + "'>";
                            strTable += "<img style='width: 200px; height: 200px;' src='" + ImgSource + "'  alt='Image preview' title='Image Preview'/>";
                            strTable += "<span>'" + dr.Name + "'</span>";
                            strTable += "</a>";
                            strTable += "</div>";
                        }
                        else
                        {
                            strTable += "<div style=' width:250px; height:250px; text-align:center; float:left;'>";
                            //strTable += "<a style='color:#333333;' target='_blank' href='#'>";
                            //strTable += "<img style='width: 200px; height: 200px;' src='" + ImgSource + "'  alt='Image preview' />";
                            strTable += "<div style=' width:250px; height:200px; padding:10px;'><svg version='1.0' id='Layer_1' xmlns:dc='http://purl.org/dc/elements/1.1/' xmlns:cc='http://creativecommons.org/ns#' xmlns:rdf='http://www.w3.org/1999/02/22-rdf-syntax-ns#' xmlns:svg='http://www.w3.org/2000/svg' xmlns:sodipodi='http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd' xmlns:inkscape='http://www.inkscape.org/namespaces/inkscape' inkscape:output_extension='org.inkscape.output.svg.inkscape' sodipodi:version='0.32' sodipodi:docname='No pub.svg' inkscape:version='0.46' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' x='0px' y='0px' width='220px' height='200px' viewBox='0 0 500 500' enable-background='new 0 0 500 500' xml:space='preserve'><defs><inkscape:perspective id='perspective54' inkscape:vp_z='500 : 250 : 1' inkscape:vp_y='0 : 1000 : 0' inkscape:vp_x='0 : 250 : 1' sodipodi:type='inkscape:persp3d' inkscape:persp3d-origin='250 : 166.66667 : 1'></inkscape:perspective><inkscape:perspective id='perspective2491' inkscape:vp_z='744.09448 : 526.18109 : 1' inkscape:vp_y='0 : 1000 : 0' inkscape:vp_x='0 : 526.18109 : 1' sodipodi:type='inkscape:persp3d' inkscape:persp3d-origin='372.04724 : 350.78739 : 1'></inkscape:perspective></defs><sodipodi:namedview id='base' inkscape:cy='173.43403' inkscape:cx='250' inkscape:zoom='1.3067333' showgrid='false' pagecolor='#ffffff' bordercolor='#666666' borderopacity='1.0' objecttolerance='10.0' gridtolerance='10.0' guidetolerance='10.0' inkscape:current-layer='Layer_1' inkscape:window-y='-8' inkscape:window-x='-8' inkscape:pageopacity='0.0' inkscape:pageshadow='2' inkscape:window-width='1680' inkscape:window-height='1000'></sodipodi:namedview><circle id='circle3' fill='#FFFFFF' stroke='#000000' cx='249.5' cy='249.5' r='205'/><path id='path2497' sodipodi:nodetypes='cccccccccccccccccccsssssc' d='M307.188,128.5c-58.837,42.757-120.586,83.151-186.938,113.563c-12.729,5.414-12.67,5.267-5.75,11.469c33.161,29.168,58.497,65.065,82,102.094c70.473-34.586,131.75-84.896,191.813-134.75c10.113-10.145,10.391-10.937,5.469-16.188C368.46,175.813,338.789,150.394,307.188,128.5z M374,243.5c-55.594,42.186-112.22,86.347-175.813,116.219l-4.625,1.906c-33.012-21.727-61.166-49.727-91.438-74.938C89.48,296.115,167.834,364.842,187,379.5c32.418-13.877,63.099-30.094,93.387-48.197l108.832-76.334L374,243.5z M120.094,267.563c-4.842,2.146-13.541,6.965-15.469,8.563l-2.5,2.063l2.906,2.625c1.602,1.437,8.927,8.329,16.25,15.313c17.096,16.303,36.074,32.656,55.656,47.969c8.562,6.695,10.5,7.986,10.5,6.969c0-3.563-19.934-32.285-32.938-47.469c-2.352-2.746-4.974-5.896-5.844-7C142.776,289.137,121.105,267.114,120.094,267.563z'/><g id='g5'><g id='g7'><g id='g9'><g id='g11'></g><g id='g15'><g id='g17'></g><g id='g21'></g><g id='g25'></g><g id='g29'></g></g></g><g id='g33'><rect id='rect35' x='273.441' y='89.786' fill='#FFFFFF' width='13.187' height='32.619'/></g></g><g id='g37'><line id='line39' fill='none' stroke='#FFFFFF' stroke-width='55' stroke-miterlimit='10' x1='393.35' y1='392.509' x2='110.112' y2='109.275'/><g id='g41'><path id='path43' fill='none' stroke='#FF0000' stroke-width='49' stroke-miterlimit='10' d='M250.029,446.658c-107.846,0-195.276-87.428-195.276-195.271c0-107.848,87.43-195.277,195.276-195.277c107.845,0,195.274,87.429,195.274,195.277C445.303,359.23,357.874,446.658,250.029,446.658z'/><path id='path45' fill='none' stroke='#000000' stroke-width='49' stroke-miterlimit='10' d='M250.029,251.387'/></g><line id='line47' fill='none' stroke='#FF0000' stroke-width='51.12' stroke-miterlimit='10' x1='392.868' y1='392.191' x2='109.63' y2='108.957'/></g></g></svg>";
                            strTable += "<span>'" + dr.Name + "'</span></div>";
                            //strTable += "</a>";
                            strTable += "</div>";
                        }

                    }
                    else
                    {
                        string ImgSource = dr.Path + dr.Name;
                        SupportCallImplementation supportCallInformation = new SupportCallImplementation();
                        if (!supportCallInformation.IsFileExist(ImgSource))
                        {
                            strTable += "<div style=' width:250px; height:250px; text-align:center; float:left;'>";
                            //strTable += "<a style='color:#333333;' target='_blank' href='#'>";
                            //strTable += "<img style='width: 200px; height: 200px;' src='" + ImgSource + "'  alt='Image preview' />";
                            strTable += "<div style=' width:250px; height:200px; padding:10px;'><svg version='1.0' id='Layer_1' xmlns:dc='http://purl.org/dc/elements/1.1/' xmlns:cc='http://creativecommons.org/ns#' xmlns:rdf='http://www.w3.org/1999/02/22-rdf-syntax-ns#' xmlns:svg='http://www.w3.org/2000/svg' xmlns:sodipodi='http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd' xmlns:inkscape='http://www.inkscape.org/namespaces/inkscape' inkscape:output_extension='org.inkscape.output.svg.inkscape' sodipodi:version='0.32' sodipodi:docname='No pub.svg' inkscape:version='0.46' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' x='0px' y='0px' width='220px' height='200px' viewBox='0 0 500 500' enable-background='new 0 0 500 500' xml:space='preserve'><defs><inkscape:perspective id='perspective54' inkscape:vp_z='500 : 250 : 1' inkscape:vp_y='0 : 1000 : 0' inkscape:vp_x='0 : 250 : 1' sodipodi:type='inkscape:persp3d' inkscape:persp3d-origin='250 : 166.66667 : 1'></inkscape:perspective><inkscape:perspective id='perspective2491' inkscape:vp_z='744.09448 : 526.18109 : 1' inkscape:vp_y='0 : 1000 : 0' inkscape:vp_x='0 : 526.18109 : 1' sodipodi:type='inkscape:persp3d' inkscape:persp3d-origin='372.04724 : 350.78739 : 1'></inkscape:perspective></defs><sodipodi:namedview id='base' inkscape:cy='173.43403' inkscape:cx='250' inkscape:zoom='1.3067333' showgrid='false' pagecolor='#ffffff' bordercolor='#666666' borderopacity='1.0' objecttolerance='10.0' gridtolerance='10.0' guidetolerance='10.0' inkscape:current-layer='Layer_1' inkscape:window-y='-8' inkscape:window-x='-8' inkscape:pageopacity='0.0' inkscape:pageshadow='2' inkscape:window-width='1680' inkscape:window-height='1000'></sodipodi:namedview><circle id='circle3' fill='#FFFFFF' stroke='#000000' cx='249.5' cy='249.5' r='205'/><path id='path2497' sodipodi:nodetypes='cccccccccccccccccccsssssc' d='M307.188,128.5c-58.837,42.757-120.586,83.151-186.938,113.563c-12.729,5.414-12.67,5.267-5.75,11.469c33.161,29.168,58.497,65.065,82,102.094c70.473-34.586,131.75-84.896,191.813-134.75c10.113-10.145,10.391-10.937,5.469-16.188C368.46,175.813,338.789,150.394,307.188,128.5z M374,243.5c-55.594,42.186-112.22,86.347-175.813,116.219l-4.625,1.906c-33.012-21.727-61.166-49.727-91.438-74.938C89.48,296.115,167.834,364.842,187,379.5c32.418-13.877,63.099-30.094,93.387-48.197l108.832-76.334L374,243.5z M120.094,267.563c-4.842,2.146-13.541,6.965-15.469,8.563l-2.5,2.063l2.906,2.625c1.602,1.437,8.927,8.329,16.25,15.313c17.096,16.303,36.074,32.656,55.656,47.969c8.562,6.695,10.5,7.986,10.5,6.969c0-3.563-19.934-32.285-32.938-47.469c-2.352-2.746-4.974-5.896-5.844-7C142.776,289.137,121.105,267.114,120.094,267.563z'/><g id='g5'><g id='g7'><g id='g9'><g id='g11'></g><g id='g15'><g id='g17'></g><g id='g21'></g><g id='g25'></g><g id='g29'></g></g></g><g id='g33'><rect id='rect35' x='273.441' y='89.786' fill='#FFFFFF' width='13.187' height='32.619'/></g></g><g id='g37'><line id='line39' fill='none' stroke='#FFFFFF' stroke-width='55' stroke-miterlimit='10' x1='393.35' y1='392.509' x2='110.112' y2='109.275'/><g id='g41'><path id='path43' fill='none' stroke='#FF0000' stroke-width='49' stroke-miterlimit='10' d='M250.029,446.658c-107.846,0-195.276-87.428-195.276-195.271c0-107.848,87.43-195.277,195.276-195.277c107.845,0,195.274,87.429,195.274,195.277C445.303,359.23,357.874,446.658,250.029,446.658z'/><path id='path45' fill='none' stroke='#000000' stroke-width='49' stroke-miterlimit='10' d='M250.029,251.387'/></g><line id='line47' fill='none' stroke='#FF0000' stroke-width='51.12' stroke-miterlimit='10' x1='392.868' y1='392.191' x2='109.63' y2='108.957'/></g></g></svg>";
                            strTable += "<span>'" + dr.Name + "'</span></div>";
                            //strTable += "</a>";
                            strTable += "</div>";
                        }
                        else
                        {
                            strTable += "<div style=' width:250px; height:250px; text-align:center; float:left;padding:30px'>";
                            strTable += "<a style='color:#333333;' target='_blank' href='" + ImgSource + "'>";

                            string modifiedFileName = string.Empty;
                            if (dr.Name.Length > 12)
                            {
                                modifiedFileName = dr.Name.Substring(0, 12);
                                strTable += "<img style='width: 200px; height: 200px;' src='" + dr.IconImage + "' alt='" + dr.Name + "' title='" + dr.Name + "'/>";
                            }
                            else
                            {
                                modifiedFileName = dr.Name;
                            }

                            strTable += "<span>'" + modifiedFileName + "'</span>";
                            strTable += "</a>";
                            strTable += "</div>";
                        }
                    }
                }
                strTable += "</div>";
            }

            if (strTable == "")
            {
                strTable = "<tr><td align='center'>No Record Available!</td></tr>";
            }
            return strTable;

        }

        private bool IsFileExist(string fileName)
        {
            return File.Exists(Server.MapPath(fileName));

        }
    }
}