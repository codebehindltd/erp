<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="EmployeeDetailsIframe.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.EmployeeDetailsIframe" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var text = "";
        $(document).ready(function () {
            $("#searchTableDiv").hide();
            $("#myTabs").hide();
            text = $.trim(CommonHelper.GetParameterByName("text"));
            if (text != "") {
                LoadCompanyForSearch(text, 1, 1);
            }
            var hfIsPayrollDependentHide = $("#<%=hfIsPayrollDependentHide.ClientID %>").val();
            if (hfIsPayrollDependentHide == 1) {
                $($("#myTabs").find("li")[4]).hide();
            }

            var hfIsPayrollBeneficiaryHide = $("#<%=hfIsPayrollBeneficiaryHide.ClientID %>").val();
            if (hfIsPayrollBeneficiaryHide == 1) {
                $($("#myTabs").find("li")[5]).hide();
            }
            var hfIsPayrollReferenceHide = $("#<%=hfIsPayrollReferenceHide.ClientID %>").val();
            if (hfIsPayrollReferenceHide == 1) {
                $($("#myTabs").find("li")[6]).hide();
            }

            var hfIsPayrollBenefitsHide = $("#<%=hfIsPayrollBenefitsHide.ClientID %>").val();
            if (hfIsPayrollBenefitsHide == 1) {
                $($("#myTabs").find("li")[7]).hide();
            }
        });
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadCompanyForSearch(text, pageNumber, IsCurrentOrPreviousPage);
        }
        function LoadCompanyForSearch(text, pageNumber, IsCurrentOrPreviousPage) {
            $("#searchTableDiv").show();


            var gridRecordsCount = $("#tblGuestInfo tbody tr").length;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../Payroll/EmployeeDetailsIframe.aspx/SearchNLoadEmpInformation',
                data: JSON.stringify({ text: text, gridRecordsCount: gridRecordsCount, pageNumber: pageNumber, isCurrentOrPreviousPage: IsCurrentOrPreviousPage }),
                dataType: "json",
                success: function (data) {
                    CommonHelper.SpinnerOpen();
                    LoadTable(data);
                },
                error: function (result) {
                    CommonHelper.AlertMessage(result.d.AlertMessage);
                }
            });
            return false;
        }
        function LoadTable(searchData) {

            CommonHelper.SpinnerClose();

           


            var gridRecordsCount = $("#tblGuestInfo tbody tr").length;
            if (gridRecordsCount < 0) {
                $("#tblGuestInfo tbody").html("");
            }
            else {
                $("#tblGuestInfo tbody tr").remove();
            }
            $("#GridPagingContainer ul").html("");
            var tr = "", totalRow = 0;
            var imagePath = "";
            var i = 0;
            if (searchData.d.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#tblGuestInfo tbody ").append(emptyTr);
                return false;
            }
            $.each(searchData.d.GridData, function (count, gridObject) {
                totalRow = $("#tblGuestInfo tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                if (gridObject.Path != "") {
                    if (gridObject.Extention == ".jpg" || gridObject.Extention == ".png" || gridObject.Extention == ".JPG" || gridObject.Extention == ".PNG" )
                        imagePath = "<img src='" + gridObject.Path + gridObject.Name + "' style=\"width:89px; height: 99px; cursor: pointer; cursor: hand;\"  alt='Employee Image' border='0' /> ";
                    else
                        imagePath = "<img src='/Images/FileType/Unknown.png' style=\"width:89px; height: 99px; cursor: pointer; cursor: hand;\"  alt='Employee Image' border='0' /> ";
                }
                else
                    imagePath = "";

                //tr += "<td align='left' style='width: 30%'>" + imagePath + "</td>";

                tr += "<td align='left'  style='width: 10%; cursor:pointer' title='Emnployee details' onClick='javascript:return LoadDetails(" + gridObject.EmpId + ")'>" + imagePath + "</td>";
                tr += "<td align='left'  style='width: 10%; cursor:pointer' title='Emnployee details' onClick='javascript:return LoadDetails(" + gridObject.EmpId + ")'>" + gridObject.EmpCode + "</td>";
                tr += "<td align='left'  style='width: 10%; cursor:pointer' title='Employee details' onClick='javascript:return LoadDetails(" + gridObject.EmpId + ")'>" + gridObject.EmployeeName + "</td>";
                tr += "<td align='left' style='width: 10%; cursor:pointer' title='Emnployee details' onClick='javascript:return LoadDetails(" + gridObject.EmpId + ")'>" + gridObject.EmpType + "</td>";
                tr += "<td align='left' style='width: 10%; cursor:pointer' title='Emnployee details' onClick='javascript:return LoadDetails(" + gridObject.EmpId + ")'>" + gridObject.Department + "</td>";
                tr += "<td align='left' style='width: 10%; cursor:pointer' title='Emnployee details' onClick='javascript:return LoadDetails(" + gridObject.EmpId + ")'>" + gridObject.Designation + "</td>";
                tr += "<td align='left' style='width: 5%; cursor:pointer' title='Emnployee details' onClick='javascript:return LoadDetails(" + gridObject.EmpId + ")'>" + gridObject.PresentPhone + "</td>";
                tr += "<td align='left' style='width: 10%;cursor:pointer' title='Emnployee details' onClick='javascript:return LoadDetails(" + gridObject.EmpId + ")'>" + gridObject.PersonalEmail + "</td>";
                tr += "<td align='left' style='width: 10%;cursor:pointer' title='Emnployee details' onClick='javascript:return LoadDetails(" + gridObject.EmpId + ")'>" + gridObject.OfficialEmail + "</td>";
                tr += "<td align='left' style='width: 10%;cursor:pointer' title='Emnployee details' onClick='javascript:return LoadDetails(" + gridObject.EmpId + ")'>" + gridObject.PresentAddress + "</td>";
                tr += "<td align='left' style='width: 5%;cursor:pointer' title='Emnployee details' onClick='javascript:return LoadDetails(" + gridObject.EmpId + ")'>" + gridObject.EmployeeStatus + "</td>";
                //tr += "<td style='width:10%;'> <a onclick=\"javascript:return FillFormEdit(" + gridObject.CompanyId + ",\'" + gridObject.CompanyName + '\');"' + "title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                //tr += "&nbsp;&nbsp;<a href='#' title='Delete' onclick= 'DeleteCompany(" + gridObject.CompanyId + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                //tr += '&nbsp;&nbsp;<a href="javascript:void();" onclick= "javascript:return ShowCompanyDocuments(' + gridObject.CompanyId + ');" title="Documents"><img style="width:16px;height:16px;" alt="Documents" src="../Images/document.png" /></a>';
                tr += "</td>";
                $("#tblGuestInfo tbody").append(tr);
                tr = "";
                imagePath = "";

            });
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.NextButton);

            CommonHelper.ApplyIntigerValidation();
            return false;
        }
        function LoadDetails(id) {
            var detailsEnable = $("#<%=hfIsEmpSearchDetailsFromDashboardEnable.ClientID %>").val();
            if (detailsEnable == "1") {
                if (id != "") {
                    //$("#searchTableDiv").hide();
                    $("#<%=hfEmpId.ClientID %>").val(id);

                    var iframeid = 'IframeReport';
                    var url = "/Payroll/EmployeeInfoIframe.aspx?empId=" + id;
                    document.getElementById(iframeid).src = url;
                    //$("#ReportPanel").show();
                    $("#ReportPanel").dialog({
                        autoOpen: true,
                        modal: true,
                        width: 1000,
                        height: 600,
                        closeOnEscape: false,
                        resizable: false,
                        fluid: true,
                        title: "",

                        show: 'slide'
                    });
                    //$(this).dialog(close);
                }
            }

        }
        function myfunction(id) {
            PageMethods.FillForm(id, OnFillFormSucceed, OnFillFormFailed);
            return false;
        }
        function OnFillFormSucceed(result) {

            $("#searchTableDiv").hide();
            //$("#myTabs").show();
            var EmployeeList = result[0].EmployeeList;
            var EmpEducation = result[0].EmpEducation;
            var EmpExperience = result[0].EmpExperience;
            var EmpDependent = result[0].EmpDependent;
            var EmpNominee = result[0].EmpNominee;
            var EmpReference = result[0].EmpReference;
            var BankInfo = result[0].BankInfo;



        }
        function LoadTables(result, tableName) {

        }
    </script>
    <asp:HiddenField ID="hfIsEmpSearchDetailsFromDashboardEnable" runat="server"></asp:HiddenField>
    <div id="ReportPanel" class="panel panel-default" style="display: none;">
        <%--<iframe  id="IframeReport" name="IframeName" runat="server" clientidmode="static" 
            style="height: 100vh; width: 100%; overflow:hidden" frameborder="0"></iframe>--%>
        <iframe id="IframeReport" name="printDoc" width="100%" height="800" frameborder="0" style="overflow-y:scroll"></iframe>
    </div>
    <div id="searchTableDiv" style="display: none">
        <div class="form-group">
            <table id="tblGuestInfo" style="width: 100%;" class="table table-bordered table-condensed table-responsive">
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold; text-align: left;">
                        <th style="width: 10%;">Image
                        </th>
                        <th style="width: 10%;">Employee ID
                        </th>
                        <th style="width: 10%;">Employee Name
                        </th>
                        <th style="width: 10%;">Employee Type
                        </th>
                        <th style="width: 10%;">Department
                        </th>
                        <th style="width: 10%;">Designation
                        </th>
                        <th style="width: 5%;">Phone
                        </th>
                        <th style="width: 10%;">Email
                        </th>
                        <th style="width: 10%;">Office Email
                        </th>
                        <th style="width: 10%;">Address
                        </th>
                        <th style="width: 5%;">Status
                        </th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
            <div class="childDivSection">
                <div class="text-center" id="GridPagingContainer">
                    <ul class="pagination">
                    </ul>
                </div>
            </div>
        </div>
    </div>



    <asp:HiddenField ID="hfEmpId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIsPayrollDependentHide" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsPayrollBeneficiaryHide" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsPayrollBenefitsHide" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsPayrollReferenceHide" runat="server"></asp:HiddenField>
    <div id="myTabs" style="display: none">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Official</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Details</a></li>
            <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-3">Education</a></li>
            <li id="D" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-4">Experience</a></li>
            <li id="E" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-5">Dependent</a></li>
            <li id="F" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-6">Beneficiary</a></li>
            <li id="L" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-7">Reference</a></li>
            <li id="K" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-11">Benefits</a></li>
            <li id="G" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-8">Bank Info</a></li>
            <li id="H" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-9">Documents</a></li>
            <%--<li id="J" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-10">Others...</a></li>--%>
        </ul>
        <div id="tab-1">
            <div id="EmpMaster" class="panel panel-default">
                <div class="panel-heading">
                    Employee Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <%--<div class="form-group" style="display: none;">
                            <div class="col-md-2">
                                <asp:Label ID="lblGoToScrolling" runat="server" class="control-label" Text="Go To Scrolling"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtGoToScrolling" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>--%>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblEmpCode" runat="server" class="control-label required-field" Text="Employee ID"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:HiddenField ID="hfEmpCode" runat="server" />
                                <asp:TextBox ID="txtEmpCode" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblTitle" runat="server" class="control-label" Text="Title"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTitle" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                <%--<asp:DropDownList ID="ddlTitle" runat="server" CssClass="form-control" TabIndex="2" Enabled="false">
                                    <asp:ListItem Value="">----Please Select----</asp:ListItem>
                                    <asp:ListItem Value="Mr">Mr.</asp:ListItem>
                                    <asp:ListItem Value="Mrs">Mrs.</asp:ListItem>
                                    <asp:ListItem Value="Miss">Miss</asp:ListItem>
                                </asp:DropDownList>--%>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFirstName" runat="server" class="control-label required-field"
                                    Text="First Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFirstName" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblLastName" runat="server" class="control-label required-field" Text="Last Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtLastName" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDisplayName" runat="server" class="control-label" Text="Full Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblEmpJoinDate" runat="server" class="control-label required-field"
                                    Text="Join Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmpJoinDate" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblDepartmentId" runat="server" class="control-label required-field"
                                    Text="Department"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDepartment" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                <%--<<%--asp:DropDownList ID="ddlDepartmentId" runat="server" CssClass="form-control" Enabled="false">
                                </asp:DropDownList>--%>--%>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblEmpCategoryId" runat="server" class="control-label required-field"
                                    Text="Employee Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmpCategort" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                <%--<asp:DropDownList ID="ddlEmpCategoryId" runat="server" CssClass="form-control" Enabled="false">
                                </asp:DropDownList>--%>
                            </div>
                            <div id="ContractEndDateDiv" style="display: none;">
                                <div class="col-md-2">
                                    <asp:Label ID="lblContractEndDate" runat="server" class="control-label required-field"
                                        Text="Contract End Date"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtContractEndDate" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                    <asp:HiddenField ID="hftxtContractEndDate" runat="server" />
                                </div>
                            </div>
                            <div id="ProvisionPeriodDiv" style="display: none;">
                                <div class="col-md-2">
                                    <asp:Label ID="lblProvisionPeriod" runat="server" class="control-label" Text="Probation Period"></asp:Label>
                                </div>
                                <div class="col-md-1">
                                    <asp:CheckBox ID="chkIsProvisionPeriod" runat="server" Text="" onclick="javascript: return ToggleFieldVisible();"
                                        TabIndex="9" Enabled="false" />
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtProvisionPeriod" CssClass="form-control" Width="190px" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblGradeId" runat="server" class="control-label required-field" Text="Employee Grade"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtGrade" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                <%--<asp:DropDownList ID="ddlGradeId" runat="server" CssClass="form-control" Enabled="false">
                                </asp:DropDownList>--%>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Employee Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmpStatus" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                <%--<asp:DropDownList ID="ddlEmployeeStatus" runat="server" CssClass="form-control" Enabled="false">
                                </asp:DropDownList>--%>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDesignationId" runat="server" class="control-label required-field"
                                    Text="Designation Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDesignation" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                <%--<asp:DropDownList ID="ddlDesignationId" runat="server" CssClass="form-control" Enabled="false">
                                </asp:DropDownList>--%>
                            </div>
                            <div id="glCompanyDiv">
                                <div class="col-md-2">
                                    <asp:HiddenField ID="hfIsSingle" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hfGLCompanyId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hfGLProjectId" runat="server" Value="0" />

                                    <asp:Label ID="lblGLCompany" runat="server" class="control-label required-field"
                                        Text="Company"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCompany" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                    <%-- <asp:DropDownList ID="ddlGLCompany" runat="server" CssClass="form-control" Enabled="false">
                                    </asp:DropDownList>--%>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" id="PayrollWorkStationDiv" runat="server">
                            <div class="col-md-2">
                                <asp:Label ID="lblWorkStation" runat="server" class="control-label" Text="Work Station"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtWorkStation" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                <%--<asp:DropDownList ID="ddlWorkStation" runat="server" CssClass="form-control" Enabled="false">
                                </asp:DropDownList>--%>
                            </div>
                        </div>
                        <div class="form-group" id="CostCenterDiv" runat="server">
                            <div class="col-md-2">
                                <asp:Label ID="lblCostCenter" runat="server" class="control-label" Text="Cost Center"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtCostCenter" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>

                                <%--<asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="form-control" Enabled="false">
                                </asp:DropDownList>--%>
                            </div>
                        </div>
                        <div class="form-group" id="PayrollDonorNameAndActivityCodeHideDiv" runat="server">
                            <div class="col-md-2">
                                <asp:Label ID="lblDonor" runat="server" class="control-label" Text="Donor Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDonor" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>

                                <%--<asp:DropDownList ID="ddlDonor" runat="server" CssClass="form-control" Enabled="false">
                                </asp:DropDownList>--%>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="lblActivityCode" runat="server" class="control-label" Text="Activity Code"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtActivityCode" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblReportingTo" runat="server" class="control-label" Text="Reporting To (1)"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRoprtingTo" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>

                                <%--<asp:DropDownList ID="ddlReportingTo" runat="server" CssClass="form-control" Enabled="false">
                                </asp:DropDownList>--%>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblReportingTo2" runat="server" class="control-label" Text="Reporting To (2)"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtReportingTo2" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>

                                <%--<asp:DropDownList ID="ddlReportingTo2" runat="server" CssClass="form-control" Enabled="false">
                                </asp:DropDownList>--%>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblOfficialEmail" runat="server" class="control-label"
                                    Text="Official Email"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtOfficialEmail" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblReferenceBy" runat="server" class="control-label" Text="Reference By"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtReferenceBy" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Job Description"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="Payroll Currency"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="PayrollCurrency" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                <%--<asp:DropDownList ID="ddlPayrollCurrencyId" runat="server" CssClass="form-control" TabIndex="2" Enabled="false">
                                </asp:DropDownList>--%>
                            </div>
                            <%--<div class="form-group" id="IsProvidentFundDeductHideDiv" runat="server">
                                <div class="col-md-3">
                                    <asp:Label ID="Label4" runat="server" class="control-label" Text="Is Provident Fund Deduct?"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlIsProvidentFundDeduct" runat="server" CssClass="form-control" TabIndex="3" Enabled="false">
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>--%>
                        </div>



                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">

            <div id="EmployeeDetailsInformation" class="panel panel-default">
                <div class="panel-heading">
                    Employee Details Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFathersName" runat="server" class="control-label" Text="Father's Name"></asp:Label>
                                <%--<span class="MandatoryField">*</span>--%>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtFathersName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblMothersName" runat="server" class="control-label" Text="Mother's Name"></asp:Label>
                                <%--<span class="MandatoryField">*</span>--%>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtMothersName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblEmpDateOfBirth" runat="server" class="control-label" Text="Date Of Birth"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmpDateOfBirth" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblGender" runat="server" class="control-label required-field" Text="Gender"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtGender" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>

                                <%-- <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                                </asp:DropDownList>--%>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblBloodGroup" runat="server" class="control-label" Text="Blood Group"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtBloodGroup" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>

                                <%--<asp:DropDownList ID="ddlBloodGroup" runat="server" CssClass="form-control">
                                </asp:DropDownList>--%>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblReligion" runat="server" class="control-label required-field" Text="Religion"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtReligion" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>

                                <%-- <asp:DropDownList ID="ddlReligion" runat="server" CssClass="form-control">
                                </asp:DropDownList>--%>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblHeight" runat="server" class="control-label" Text="Employee Height"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtHeight" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblMaritalStatus" runat="server" class="control-label required-field"
                                    Text="Marital Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtMaritialStatus" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>

                                <%--<asp:DropDownList ID="ddlMaritalStatus" runat="server" CssClass="form-control">
                                </asp:DropDownList>--%>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblNatinality" runat="server" class="control-label required-field"
                                    Text="Nationality"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCountry" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>

                                <%--<asp:DropDownList ID="ddlCountryId" runat="server" CssClass="form-control">
                                </asp:DropDownList>--%>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblNationalId" runat="server" class="control-label" Text="National Id/SSN"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNationalId" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <asp:Panel ID="pnlDivisionDistrictThana" runat="server">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblDivision" runat="server" class="control-label" Text="Division"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDivision" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>

                                    <%--<asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control">
                                    </asp:DropDownList>--%>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblDistrict" runat="server" class="control-label" Text="District"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDistrict" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>

                                    <%--<asp:DropDownList ID="ddlDistrict" runat="server" CssClass="form-control">
                                    </asp:DropDownList>--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblThana" runat="server" class="control-label" Text="Thana/Upazilla"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtThana" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>

                                    <%--<asp:DropDownList ID="ddlThana" runat="server" CssClass="form-control">
                                    </asp:DropDownList>--%>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblTinNumber" runat="server" class="control-label" Text="TIN Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtTinNumber" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPassportNumber" runat="server" class="control-label" Text="Passport Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPassportNumber" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPIssueDate" runat="server" class="control-label" Text="Pass Issue Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPIssueDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPIssuePlace" runat="server" class="control-label" Text="Pass Issue Place"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPIssuePlace" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPExpireDate" runat="server" class="control-label" Text="Pass. Expiry Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPExpireDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <%--<div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblCurrentLocation" runat="server" Text="Current Location"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCurrentLocation" runat="server" CssClass="ThreeColumnDropDownList"
                                TabIndex="2">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>--%>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPresentAddress" runat="server" class="control-label" Text="Present Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtPresentAddress" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPresentCity" runat="server" class="control-label" Text="Present City"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPresentCity" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPresentZipCode" runat="server" class="control-label" Text="Present Zip/ P.O. Box"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPresentZipCode" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPresentCountry" runat="server" class="control-label" Text="Present Country"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPresentCountry" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPresentPhone" runat="server" class="control-label" Text="Present Phone"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPresentPhone" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPermanentAddress" runat="server" class="control-label" Text="Permanent Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtPermanentAddress" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPermanentCity" runat="server" class="control-label" Text="Permanent City"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPermanentCity" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPermanentZipCode" runat="server" class="control-label" Text="Per. Zip/ P.O. Box"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPermanentZipCode" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPermanentCountry" runat="server" class="control-label" Text="Permanent Country"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPermanentCountry" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPermanentPhone" runat="server" class="control-label" Text="Permanent Phone"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPermanentPhone" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPersonalEmail" runat="server" class="control-label" Text="Personal Email"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtPersonalEmail" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAlternativeEmail" runat="server" class="control-label" Text="Alternative Email"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtAlternativeEmail" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label11" runat="server" class="control-label " Text="Emerg. Contact Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmergencyContactName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label13" runat="server" class="control-label" Text="Relationship"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmergencyContactRelationship" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label14" runat="server" class="control-label" Text="Emerg. Contact Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmergencyContactNumber" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label15" runat="server" class="control-label" Text="Emerg. Contact Email"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmergencyContactEmail" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-3">
            <div id="EducationInformation" class="panel panel-default">
                <div class="panel-heading">
                    Education Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblExamLevel" runat="server" class="control-label required-field"
                                    Text="Level"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlExamLevel" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblExamName" runat="server" class="control-label required-field" Text="Degree"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtExamName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblInstituteName" runat="server" class="control-label required-field"
                                    Text="Institute Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtInstituteName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSubjectName" runat="server" class="control-label" Text="Subject Name (Major)"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSubjectName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPassYear" runat="server" class="control-label required-field" Text="Passing Year"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPassYear" CssClass="form-control quantity" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblPassClass" runat="server" class="control-label required-field"
                                    Text="Result (Class/GPA)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPassClass" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <%--<div class="row">
                            <div class="col-md-12">
                                
                                <asp:Button ID="btnEmpEducation" runat="server" Text="Add" CssClass="btn btn-primary"
                                    OnClick="btnEmpEducation_Click" />
                                <asp:Label ID="hfEducationId" runat="server" class="control-label" Text='' Visible="False"></asp:Label>
                            </div>
                        </div>--%>
                        <div>
                            <%--<asp:GridView ID="gvEmpEducation" Width="100%" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                ForeColor="#333333" PageSize="5" OnRowCommand="gvEmpEducation_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("EducationId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ExamName" HeaderText="Exam Name" ItemStyle-Width="51%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PassYear" HeaderText="PassYear" ItemStyle-Width="17%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PassClass" HeaderText="PassClass" ItemStyle-Width="17%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# bind("EducationId") %>'
                                                CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit"
                                                ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# bind("EducationId") %>'
                                                CommandName="CmdDelete" ImageUrl="~/Images/delete.png" OnClientClick="return confirm('Do you want to save?');"
                                                Text="" AlternateText="Delete" ToolTip="Delete" />
                                        </ItemTemplate>
                                        <ControlStyle Font-Size="Small" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblRecordNotFound" runat="server" Text="Record Not Found."></asp:Label>
                                </EmptyDataTemplate>
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#44545E" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#7C6F57" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-4">
            <div id="ExperienceInformation" class="panel panel-default">
                <div class="panel-heading">
                    Experience Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCompanyName" runat="server" class="control-label required-field"
                                    Text="Company Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtCompanyName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCompanyUrl" runat="server" class="control-label" Text="Company Website"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtCompanyUrl" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblJoinDate" runat="server" class="control-label required-field" Text="Join Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtJoinDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblJoinDesignation" runat="server" class="control-label" Text="Join Designation"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtJoinDesignation" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblLeaveDate" runat="server" class="control-label required-field"
                                    Text="Leave Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtLeaveDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblLeaveDesignation" runat="server" class="control-label" Text="Leave Designation"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtLeaveDesignation" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAchievements" runat="server" class="control-label" Text="Achievements"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtAchievements" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <%--<div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnEmpExperience" runat="server" Text="Add" CssClass="btn btn-primary"
                                    OnClick="btnEmpExperience_Click" />
                                <asp:Label ID="hfExperienceId" runat="server" class="control-label" Visible="False"></asp:Label>
                            </div>
                        </div>--%>
                        <div>
                            <%-- <asp:GridView ID="gvExperience" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                                OnRowCommand="gvExperience_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("ExperienceId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CompanyName" HeaderText="Company" ItemStyle-Width="51%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShowJoinDate" HeaderText="Join Date" ItemStyle-Width="17%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShowLeaveDate" HeaderText="Leave Date" ItemStyle-Width="17%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# bind("ExperienceId") %>'
                                                CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit"
                                                ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# bind("ExperienceId") %>'
                                                CommandName="CmdDelete" ImageUrl="~/Images/delete.png" OnClientClick="return confirm('Do you want to save?');"
                                                Text="" AlternateText="Delete" ToolTip="Delete" />
                                        </ItemTemplate>
                                        <ControlStyle Font-Size="Small" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblRecordNotFound" runat="server" Text="Record Not Found."></asp:Label>
                                </EmptyDataTemplate>
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#44545E" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#7C6F57" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-5">
            <div id="DependentInformation" class="panel panel-default">
                <div class="panel-heading">
                    Dependent Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDependentName" runat="server" class="control-label required-field"
                                    Text="Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDependentName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDateOfBirth" runat="server" class="control-label" Text="Date Of Birth"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDateOfBirth" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Age"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:HiddenField ID="hfAge" runat="server" />
                                <asp:TextBox ID="txtAge" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRelationship" runat="server" class="control-label required-field"
                                    Text="Relationship"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtRelationship" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <%--<div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnEmpDependent" runat="server" Text="Add" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnEmpDependent_Click" OnClientClick="javascript:return CheckAgeEligibility()" />
                                <asp:Label ID="lblHiddenDependentId" runat="server" class="control-label" Text=''
                                    Visible="False"></asp:Label>
                            </div>
                        </div>--%>
                        <div>
                            <%--<asp:GridView ID="gvDependent" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                                OnRowCommand="gvDependent_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("DependentId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DependentName" HeaderText="Name" ItemStyle-Width="50%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Relationship" HeaderText="Relationship" ItemStyle-Width="35%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# bind("DependentId") %>'
                                                CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit"
                                                ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# bind("DependentId") %>'
                                                CommandName="CmdDelete" ImageUrl="~/Images/delete.png" OnClientClick="return confirm('Do you want to delete?');"
                                                Text="" AlternateText="Delete" ToolTip="Delete" />
                                        </ItemTemplate>
                                        <ControlStyle Font-Size="Small" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblRecordNotFound" runat="server" Text="Record Not Found."></asp:Label>
                                </EmptyDataTemplate>
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#44545E" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#7C6F57" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-6">
            <div id="Div3" class="panel panel-default">
                <div class="panel-heading">
                    Beneficiary Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label6" runat="server" class="control-label required-field" Text="Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtNomineeName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label8" runat="server" class="control-label" Text="Date Of Birth"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNomineeDateOfBirth" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label9" runat="server" class="control-label" Text="Age"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:HiddenField ID="hfNomineeAge" runat="server" />
                                <asp:TextBox ID="txtNomineeAge" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label10" runat="server" class="control-label required-field" Text="Relationship"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNomineeRelationship" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label12" runat="server" class="control-label required-field" Text="Percentage"></asp:Label>
                                <span>(%)</span>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPercentage" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <%--<div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnAddNominee" runat="server" Text="Add" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnAddNominee_Click" OnClientClick="javascript:return ValidateNomineePerentage()" />
                                <asp:Label ID="lblNomineeId" runat="server" Text='' Visible="False"></asp:Label>
                            </div>
                        </div>--%>
                        <div>
                            <%--<asp:GridView ID="gvNominee" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                                OnRowCommand="gvNominee_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("NomineeId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="NomineeName" HeaderText="Name" ItemStyle-Width="45%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Relationship" HeaderText="Relationship" ItemStyle-Width="25%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Percentage" HeaderText="Percentage" ItemStyle-Width="15%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NomineeId" HeaderText="NomineeId" ItemStyle-Width="15%" ItemStyle-CssClass="HideTag" HeaderStyle-CssClass="HideTag">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# bind("NomineeId") %>'
                                                CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit"
                                                ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# bind("NomineeId") %>'
                                                CommandName="CmdDelete" ImageUrl="~/Images/delete.png" OnClientClick="return confirm('Do you want to delete?');"
                                                Text="" AlternateText="Delete" ToolTip="Delete" />
                                        </ItemTemplate>
                                        <ControlStyle Font-Size="Small" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblRecordNotFound" runat="server" Text="Record Not Found."></asp:Label>
                                </EmptyDataTemplate>
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#44545E" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#7C6F57" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-7">
            <div id="Div7" class="panel panel-default">
                <div class="panel-heading">
                    Reference Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblReferenceName" runat="server" class="control-label required-field" Text="Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtReferenceName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblOrganization" runat="server" class="control-label" Text="Organization"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtOrganization" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDesignation" runat="server" class="control-label" Text="Designation"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRefDesignation" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAddress" runat="server" class="control-label" Text="Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtAddress" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblMobile" runat="server" class="control-label" Text="Mobile"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtMobile" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblEmail" runat="server" class="control-label" Text="Email"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblReferenceRelationship" runat="server" class="control-label required-field" Text="Relationship"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtReferenceRelationship" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDescription" runat="server" class="control-label" Text="Description"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox TextMode="MultiLine" ID="txtDescription" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <%--<div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnAddReference" runat="server" Text="Add" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    onclick="btnAddReference_Click" OnClientClick="javascript:return ValidateReferencePerson()" />
                                <asp:Label ID="lblReferenceId" runat="server" Text='' Visible="False"></asp:Label>
                            </div>
                        </div>--%>
                        <div>
                            <%-- <asp:GridView ID="gvReference" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                                OnRowCommand="gvReference_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("ReferenceId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="25%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Relationship" HeaderText="Relationship" ItemStyle-Width="25%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="30%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# bind("ReferenceId") %>'
                                                CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" OnClientClick="return confirm('Do you want to edit?');"
                                                ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# bind("ReferenceId") %>'
                                                CommandName="CmdDelete" ImageUrl="~/Images/delete.png" OnClientClick="return confirm('Do you want to delete?');"
                                                Text="" AlternateText="Delete" ToolTip="Delete" />
                                        </ItemTemplate>
                                        <ControlStyle Font-Size="Small" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblRecordNotFound" runat="server" Text="Record Not Found."></asp:Label>
                                </EmptyDataTemplate>
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#44545E" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#7C6F57" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-8">
            <div id="BankDiv" class="panel panel-default">
                <div class="panel-heading">
                    Bank Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblBankId" runat="server" class="control-label" Text="Bank Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control" TabIndex="13">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblBranchName" runat="server" class="control-label" Text="Branch Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtBranchName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAccountName" runat="server" class="control-label" Text="Account Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAccountName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblAccountNumber" runat="server" class="control-label" Text="Account Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAccountNumber" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAccountType" runat="server" class="control-label" Text="Account Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAccountType" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label7" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarksForBankInfo" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="12"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-9">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Employee Documents
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <%--<div class="form-group">
                            <asp:HiddenField ID="RandomEmpId" runat="server" />
                            <asp:HiddenField ID="tempEmpId" runat="server" />
                            <div class="col-md-2">
                                <asp:Label ID="lblSignatureImage" runat="server" class="control-label" Text="Employee Signature"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <input id="btnSignatureUp" type="button" onclick="javascript: return LoadSignatureUploader();"
                                    class="TransactionalButton btn btn-primary" value="Upload Signature" />
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblDocumentImage" runat="server" class="control-label" Text="Employee Picture"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <input id="btnDocumentUp" type="button" onclick="javascript: return LoadDocumentUploader();"
                                    class="TransactionalButton btn btn-primary" value="Upload Picture" />
                            </div>
                        </div>--%>
                        <div class="form-group">
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                                <div id="DocDiv" style="width: 150px; height: 150px" runat="server">
                                </div>
                            </div>
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                                <div id="SigDiv" style="width: 150px; height: 150px" runat="server">
                                </div>
                            </div>
                        </div>
                        <%--<div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label">Attachment</label>
                            </div>
                            <div class="col-md-4">
                                <input id="btnImageUp" type="button" onclick="javascript: return LoadOthersDocumentUploader();"
                                    class="TransactionalButton btn btn-primary btn-sm" value="Others Document..." />
                            </div>
                        </div>--%>
                    </div>

                    <div class="block-body collapse in">

                        <div class="form-group">
                            <div id="DocumentInfo" runat="server" class="col-md-12">
                            </div>
                        </div>


                    </div>

                </div>




            </div>
        </div>

    </div>
</asp:Content>
