<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmResumeSearch.aspx.cs" Inherits="HotelManagement.Presentation.Website.Recruitment.frmResumeSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var vcc = [];

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Employee Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Search Resume Bank</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#btnSearch").click(function () {
                $("#SearchOutput").show('slow');
                GridPaging(1, 1);
            });

            $("#PopMyTabs").tabs();
            $("#btnBackToDecisionMaker").click(function () {
                //popup(-1);
                $("#TouchKeypad").dialog("close");
            });

            $("#ContentPlaceHolder1_ddlSearchType").change(function () {
                var searchType = $("#<%=ddlSearchType.ClientID %>").val();
                if (searchType == "Applicants") {
                    $("#ApplicantSearchPanel").show();
                    $("#EmployeeSearchPanel").hide();
                }
                else if (searchType == "Employees") {
                    $("#ApplicantSearchPanel").hide();
                    $("#EmployeeSearchPanel").show();
                }
            });

            $("#checkAllResume").click(function () {
                if ($(this).is(":checked") == true) {
                    $("#gvResumeSearch tbody tr").find("td:eq(0)").find("input").prop('checked', true);
                }
                else {
                    $("#gvResumeSearch tbody tr").find("td:eq(0)").find("input").prop('checked', false);
                }
            });


            $("#chkbSelectAllJob").click(function () {
                if ($(this).is(":checked") == true) {
                    $("#JobCircularTbl tbody tr").find("td:eq(0)").find("input").prop('checked', true);
                }
                else {
                    $("#JobCircularTbl tbody tr").find("td:eq(0)").find("input").prop('checked', false);
                }
            });

            $('#ContentPlaceHolder1_ddlEmpDivision').change(function () {
                LoadDistrict();
                LoadThana();
            });

            $('#ContentPlaceHolder1_ddlEmpDistrict').change(function () {
                LoadThana();
            });

            $('#ContentPlaceHolder1_ddlAppDivision').change(function () {
                LoadDistrict();
                LoadThana();
            });

            $('#ContentPlaceHolder1_ddlAppDistrict').change(function () {
                LoadThana();
            });

        });

        function LoadDistrict() {
            var empType = $("#<%=ddlSearchType.ClientID %>").val();
            if (empType == 'Applicants') {
                var divisionId = $("#<%=ddlAppDivision.ClientID %>").val();
            }
            else var divisionId = $("#<%=ddlEmpDivision.ClientID %>").val();
            PageMethods.LoadDistrict(divisionId, OnLoadDivisionSucceeded, OnLoadDivisionFailed);
            return false;
        }

        function LoadThana() {
            var empType = $("#<%=ddlSearchType.ClientID %>").val();
            if (empType == 'Applicants') {
                var districtId = $("#<%=ddlAppDistrict.ClientID %>").val();
            }
            else var districtId = $("#<%=ddlEmpDistrict.ClientID %>").val();
            PageMethods.LoadThana(districtId, OnLoadDistrictSucceeded, OnLoadDistrictFailed);
            return false;
        }

        function OnLoadDivisionSucceeded(result) {
            var list = result;
            var empType = $("#<%=ddlSearchType.ClientID %>").val();
            if (empType == 'Applicants') {
                var controlId = '<%=ddlAppDistrict.ClientID%>';
            }
            else var controlId = '<%=ddlEmpDistrict.ClientID%>';
            var control = $('#' + controlId);
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].DistrictName + '" value="' + list[i].DistrictId + '">' + list[i].DistrictName + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }
        }
        function OnLoadDivisionFailed(error) {
        }

        function OnLoadDistrictSucceeded(result) {
            var list = result;
            var empType = $("#<%=ddlSearchType.ClientID %>").val();
            if (empType == 'Applicants') {
                var controlId = '<%=ddlAppThana.ClientID%>';
            }
            else var controlId = '<%=ddlEmpThana.ClientID%>';
            var control = $('#' + controlId);
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].ThanaName + '" value="' + list[i].ThanaId + '">' + list[i].ThanaName + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }
        }
        function OnLoadDistrictFailed(error) {
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#gvResumeSearch tbody tr").length;
            var searchType = $("#<%=ddlSearchType.ClientID %>").val();

            var appName = $("#<%=txtApplicantName.ClientID %>").val();
            var appId = $("#<%=txtApplicantCode.ClientID %>").val();
            var lookingFor = $("#<%=ddlJobLevel.ClientID %>").val();
            var availableFor = $("#<%=ddlAvailableType.ClientID %>").val();
            var preSalFrom = $("#<%=txtPreSalFrom.ClientID %>").val();
            var preSalTo = $("#<%=txtpreSalTo.ClientID %>").val();
            var expSalFrom = $("#<%=txtExpSalFrom.ClientID %>").val();
            var expSalTo = $("#<%=txtExpSalTo.ClientID %>").val();
            var currency = $("#<%=ddlCurrency.ClientID %>").val();
            var jobCategory = $("#<%=ddlJobCategory.ClientID %>").val();
            var organizationType = $("#<%=ddlOrganizationType.ClientID %>").val();
            var jobLocation = $("#<%=ddlPrfJobLocation.ClientID %>").val();
            var expYrFrom = $("#<%=ddlExpFrom.ClientID %>").val();
            var expYrTo = $("#<%=ddlExpTo.ClientID %>").val();
            var appDivId = $("#<%=ddlAppDivision.ClientID %>").val();
            var appDistId = $("#<%=ddlAppDistrict.ClientID %>").val();
            var appThaId = $("#<%=ddlAppThana.ClientID %>").val();

            var empType = $("#<%=ddlEmpCategoryId.ClientID %>").val();
            var department = $("#<%=ddlDepartmentId.ClientID %>").val();
            var positionName = $("#<%=ddlDesignationId.ClientID %>").val();
            var workStation = $("#<%=ddlWorkStation.ClientID %>").val();
            var ageFrom = $("#<%=txtAgeFrom.ClientID %>").val();
            var ageTo = $("#<%=txtAgeTo.ClientID %>").val();
            var jobLengthFrom = $("#<%=txtJobLengthFrom.ClientID %>").val();
            var jobLengthTo = $("#<%=txtJobLengthTo.ClientID %>").val();
            var expFrom = $("#<%=txtExperienceFrom.ClientID %>").val();
            var expTo = $("#<%=txtExperienceTo.ClientID %>").val();
            var bloodGroup = $("#<%=ddlBloodGroup.ClientID %>").val();
            var empDivId = $("#<%=ddlEmpDivision.ClientID %>").val();
            var empDistId = $("#<%=ddlEmpDistrict.ClientID %>").val();
            var empThaId = $("#<%=ddlEmpThana.ClientID %>").val();

            if (searchType == "Applicants") {
                PageMethods.SearchApplicantResume(appName, appId, lookingFor, availableFor, preSalFrom, preSalTo, expSalFrom, expSalTo, currency, jobCategory, organizationType, jobLocation, expYrFrom, expYrTo, appDivId, appDistId, appThaId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            }
            else if (searchType == "Employees") {
                PageMethods.SearchEmployeeResume(empType, department, positionName, workStation, ageFrom, ageTo, jobLengthFrom, jobLengthTo, expFrom, expTo, bloodGroup, empDivId, empDistId, empThaId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            }
            return false;
        }

        function OnLoadObjectSucceeded(result) {
            $("#gvResumeSearch tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvResumeSearch tbody ").append(emptyTr);
                return false;
            }

            var ddlSearchTypeVal = $("#<%=ddlSearchType.ClientID %>").val();

            if (ddlSearchTypeVal == "Applicants") {
                $("#AddasEmployeeTh").show();
            }
            else {
                $("#AddasEmployeeTh").hide();
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvResumeSearch tbody tr").length;
                //totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td style = 'text-align: center; width:7%;' > <input type='checkbox' id='chk" + gridObject.EmpId + "' /> </td>";
                tr += "<td align='left' style=\"width:43%; cursor:pointer;\">" + gridObject.DisplayName + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.EmpCode + "</td>";
                tr += "<td align='right' style=\"width:10%; cursor:pointer;\"><img src='../Images/ReportDocument.png' onClick= \"javascript:return GenerateResume('" + gridObject.EmpId + "')\" alt='Resume Review' ToolTip='Resume Review' border='0' /></td>";
                tr += "<td align='right' style=\"width:10%; cursor:pointer;\"><img src='../Images/ReportDocument.png' onClick= \"javascript:return DocumentsDetails('" + gridObject.EmpId + "')\" alt='Documents Review' ToolTip='Documents Review' border='0' /></td>";
                if (gridObject.IsApplicantRecruitment) {
                    tr += "<td align='right' style=\"width:10%; cursor:pointer;\"><img src='../Images/job.png' onClick= \"javascript:return AddAsEmployee('" + gridObject.EmpId + "')\" alt='Resume Review' ToolTip='Resume Review' border='0' /></td>";
                }
                tr += "<td align='left' style=\"display:none;\">" + gridObject.EmpId + "</td>";


                //tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.Department + "</td>";
                //tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.Designation + "</td>";
                //tr += "<td align='right' style=\"width:10%; cursor:pointer;\"><img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.EmpId + "')\" alt='Edit Information' border='0' /></td>";               

                tr += "</tr>"

                $("#gvResumeSearch tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            return false;
        }

        function OnLoadObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function GenerateResume(empId) {
            var url = "/Recruitment/Reports/frmApplicantResumePreview.aspx?EmpId=" + empId;
            var popup_window = "Generate Resume";
            window.open(url, popup_window, "width=785,height=780,left=300,top=50,resizable=yes");
        }

        function DocumentsDetails(empId) {
            LoadApplicantsDocuments(empId);
            //popup(1, 'TouchKeypad', '', 900, 666);
            $("#TouchKeypad").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                height: 666,
                closeOnEscape: true,
                resizable: false,
                title: "", ////TODO add title
                show: 'slide'
            });
            $("#popUpDiv").css('height', '757px');
            return false;
        }

        function LoadApplicantsDocuments(empId) {
            PageMethods.GetDocumentsByUserTypeAndUserId(empId, OnLoadImagesSucceeded, OnLoadImagesFailed);
            return false;
        }
        function OnLoadImagesSucceeded(result) {
            $("#imageDiv").html(result);
            return false;
        }
        function OnLoadImagesFailed(error) {
            alert(error.get_message());
        }

        function JobOpening() {
            PageMethods.GetJobCircular(OnLoadJobCircularSucceeded, OnLoadJobCircularFailed);
            return false;
        }

        function OnLoadJobCircularSucceeded(result) {

            var tr = "", totalRow = 1, editLink = "", deleteLink = "";
            $("#JobCircularTbl tbody").html("");

            $.each(result, function (count, gridObject) {

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td style = 'text-align: center; width:7%;' > <input type='checkbox' id='chk" + gridObject.JobCircularId + "' /> </td>";
                tr += "<td align='left' style=\"width:25%; cursor:pointer;\">" + gridObject.JobTitle + "</td>";
                tr += "<td align='left' style=\"width:14%; cursor:pointer;\">" + gridObject.DepartmentName + "</td>";
                tr += "<td align='left' style=\"width:12%; cursor:pointer;\">" + gridObject.JobTypeName + "</td>";
                tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.NoOfVancancie + "</td>";
                tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.AgeRangeFrom + " - " + gridObject.AgeRangeTo + "</td>";
                tr += "<td align='left' style=\"width:12%; cursor:pointer;\">" + gridObject.Gender + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + gridObject.JobCircularId + "</td>";

                tr += "</tr>"

                $("#JobCircularTbl tbody").append(tr);
                tr = "";
                totalRow += 1;
            });

            $("#JobCircularContainer").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 800,
                maxWidth: 900,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Applicant Assign To Open Job",
                show: 'slide'
            });
        }
        function OnLoadJobCircularFailed(error) {
        }

        function AssignApplicantToJob() {

            var jobId = '', empId = '';
            var job = new Array(), emp = new Array();

            var applicantType = $("#ContentPlaceHolder1_ddlSearchType").val();

            $("#JobCircularTbl tbody tr").each(function () {
                if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {
                    jobId = $(this).find("td:eq(7)").text();
                    job.push(jobId);
                }
            });

            $("#gvResumeSearch tbody tr").each(function () {
                if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {
                    empId = $(this).find("td:eq(5)").text();
                    emp.push(empId);
                }
            });
            debugger;
            PageMethods.AssignApplicantToJob(applicantType, emp, job, OnJobAssignSucceeded, OnJobAssignFailed);
            return false;
        }

        function OnJobAssignSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $("#JobCircularContainer").dialog("close");
                $("#JobCircularTbl tbody tr").find("td:eq(0)").find("input").prop('checked', false);
                $("#chkbSelectAllJob").prop('checked', false);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }

        function OnJobAssignFailed(error) {

        }

        function AddAsEmployee(empId) {
            window.location = "/Payroll/frmEmployee.aspx?EmpId=" + empId;
        }

    </script>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <div id="JobCircularContainer" style="display: none;">
        <table id="JobCircularTbl" class="table table-bordered table-condensed table-responsive"
            style="width: 100%;">
            <colgroup>
                <col style="width: 7%;" />
                <col style="width: 25%;" />
                <col style="width: 14%;" />
                <col style="width: 12%;" />
                <col style="width: 15%;" />
                <col style="width: 15%;" />
                <col style="width: 12%;" />
            </colgroup>
            <thead>
                <tr style="color: White; background-color: #44545E; font-weight: bold;">
                    <th style="text-align:center">
                        <input type="checkbox" id="chkbSelectAllJob" title="Assign To All Job" />
                    </th>
                    <th style="text-align: left;">
                        Job Title
                    </th>
                    <th style="text-align: left;">
                        Department
                    </th>
                    <th style="text-align: left;">
                        Job Type
                    </th>
                    <th style="text-align: left;">
                        Vacancies
                    </th>
                    <th style="text-align: left;">
                        Age Range
                    </th>
                    <th style="text-align: left;">
                        Gender
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
        <div class="row">
            <div class="col-md-12">
                <button type="button" id="Button1" onclick="AssignApplicantToJob()" class="TransactionalButton btn btn-primary">
                    Save</button>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSearchType" runat="server" class="control-label" Text="Search In"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSearchType" runat="server" CssClass="form-control" TabIndex="2">
                            <asp:ListItem Value="Applicants">Outside Applicants</asp:ListItem>
                            <asp:ListItem Value="Employees">Inhouse Employees</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="ApplicantSearchPanel">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblApplicantName" runat="server" class="control-label" Text="Applicant Name"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtApplicantName" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblApplicantCode" runat="server" class="control-label" Text="Applicant ID"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtApplicantCode" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblJobLevel" runat="server" class="control-label" Text="Looking for"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlJobLevel" runat="server" CssClass="form-control" TabIndex="2">
                                <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                <asp:ListItem Value="Entry Level">Entry Level</asp:ListItem>
                                <asp:ListItem Value="Mid Level">Mid Level</asp:ListItem>
                                <asp:ListItem Value="Top Level">Top Level</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblAvailableType" runat="server" class="control-label" Text="Available for"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlAvailableType" runat="server" CssClass="form-control" TabIndex="2">
                                <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                <asp:ListItem Value="Full Time">Full Time</asp:ListItem>
                                <asp:ListItem Value="Part Time">Part Time</asp:ListItem>
                                <asp:ListItem Value="Contractual">Contractual</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblPresentSalary" runat="server" class="control-label" Text="Present Salary"></asp:Label>
                        </div>
                        <div class="col-md-1">
                            <asp:TextBox ID="txtPreSalFrom" runat="server" CssClass="form-control" Width="88"></asp:TextBox>
                        </div>
                        <div class="col-md-1" style="text-align: right;">
                            <asp:Label ID="Label1" runat="server" class="control-label" Text="to"></asp:Label>
                        </div>
                        <div class="col-md-1">
                            <asp:TextBox ID="txtpreSalTo" runat="server" CssClass="form-control" Width="88"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblExpectedSlary" runat="server" class="control-label" Text="Expected Salary"></asp:Label>
                        </div>
                        <div class="col-md-1">
                            <asp:TextBox ID="txtExpSalFrom" runat="server" CssClass="form-control" Width="88"></asp:TextBox>
                        </div>
                        <div class="col-md-1" style="text-align: right;">
                            <asp:Label ID="Label2" runat="server" class="control-label" Text="to"></asp:Label>
                        </div>
                        <div class="col-md-1">
                            <asp:TextBox ID="txtExpSalTo" runat="server" CssClass="form-control" Width="88"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblCurrency" runat="server" class="control-label" Text="Currency"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCurrency" CssClass="form-control" runat="server" TabIndex="4">
                                <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                <asp:ListItem Value="SSP">SSP</asp:ListItem>
                                <asp:ListItem Value="USD">USD</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblPrfJobCategory" runat="server" class="control-label" Text="Prefered Job Category"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlJobCategory" runat="server" CssClass="form-control" TabIndex="2">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblPrfOrganizationType" runat="server" class="control-label" Text="Prefered Organization Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlOrganizationType" runat="server" CssClass="form-control"
                                TabIndex="2">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblPrfJobLocation" runat="server" class="control-label" Text="Prefered Job Location"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlPrfJobLocation" runat="server" CssClass="form-control" TabIndex="2">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblExperience" runat="server" class="control-label" Text="Experience"></asp:Label>
                        </div>
                        <div class="col-md-1">
                            <asp:DropDownList ID="ddlExpFrom" runat="server" CssClass="form-control" TabIndex="2"
                                Width="85">
                                <asp:ListItem Value="0">0</asp:ListItem>
                                <asp:ListItem Value="1">1</asp:ListItem>
                                <asp:ListItem Value="2">2</asp:ListItem>
                                <asp:ListItem Value="3">3</asp:ListItem>
                                <asp:ListItem Value="4">4</asp:ListItem>
                                <asp:ListItem Value="5">5</asp:ListItem>
                                <asp:ListItem Value="6">6</asp:ListItem>
                                <asp:ListItem Value="7">7</asp:ListItem>
                                <asp:ListItem Value="8">8</asp:ListItem>
                                <asp:ListItem Value="9">9</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                            </asp:DropDownList>                           
                        </div>
                        <div class="col-md-1" style="text-align: right;">
                            <asp:Label ID="Label3" runat="server" class="control-label" Text="to"></asp:Label>
                        </div>
                        <div class="col-md-1">
                            <asp:DropDownList ID="ddlExpTo" runat="server" CssClass="form-control" TabIndex="2"
                                Width="85">
                                <asp:ListItem Value="0">0</asp:ListItem>
                                <asp:ListItem Value="1">1</asp:ListItem>
                                <asp:ListItem Value="2">2</asp:ListItem>
                                <asp:ListItem Value="3">3</asp:ListItem>
                                <asp:ListItem Value="4">4</asp:ListItem>
                                <asp:ListItem Value="5">5</asp:ListItem>
                                <asp:ListItem Value="6">6</asp:ListItem>
                                <asp:ListItem Value="7">7</asp:ListItem>
                                <asp:ListItem Value="8">8</asp:ListItem>
                                <asp:ListItem Value="9">9</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-1" style="text-align: right;">
                            <asp:Label ID="Label4" runat="server" class="control-label" Text="years"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                        </div>
                        <div class="col-md-4">
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblAppDivision" runat="server" class="control-label" Text="Division"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlAppDivision" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblAppDistrict" runat="server" class="control-label" Text="District"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlAppDistrict" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblAppThana" runat="server" class="control-label" Text="Thana/Upazilla"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlAppThana" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div id="EmployeeSearchPanel" style="display: none">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblEmpCategoryId" runat="server" class="control-label" Text="Employee Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlEmpCategoryId" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblDepartmentId" runat="server" class="control-label" Text="Department"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlDepartmentId" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblDesignationId" runat="server" class="control-label" Text="Position Name"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlDesignationId" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblWorkStation" runat="server" class="control-label" Text="Work Station"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlWorkStation" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblAge" runat="server" class="control-label" Text="Age"></asp:Label>
                        </div>
                        <div class="col-md-1">
                            <asp:TextBox ID="txtAgeFrom" runat="server" CssClass="form-control" Width="88"></asp:TextBox>                           
                        </div>
                        <div class="col-md-1" style="text-align: right;">
                            <asp:Label ID="Label5" runat="server" class="control-label" Text="to"></asp:Label>
                        </div>
                        <div class="col-md-1" style="text-align: right;">
                            <asp:TextBox ID="txtAgeTo" runat="server" CssClass="form-control" Width="88"></asp:TextBox>
                        </div>
                        <div class="col-md-1" style="text-align: right;">
                            <asp:Label ID="Label7" runat="server" class="control-label" Text="years"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblJobLength" runat="server" class="control-label" Text="Job Length"></asp:Label>
                        </div>
                        <div class="col-md-1">
                            <asp:TextBox ID="txtJobLengthFrom" runat="server" CssClass="form-control" Width="88"></asp:TextBox>                           
                        </div>
                        <div class="col-md-1" style="text-align: right;">
                            <asp:Label ID="Label6" runat="server" class="control-label" Text="to"></asp:Label>
                        </div>
                        <div class="col-md-1" style="text-align: right;">
                            <asp:TextBox ID="txtJobLengthTo" runat="server" CssClass="form-control" Width="88"></asp:TextBox>
                        </div>
                        <div class="col-md-1" style="text-align: right;">
                            <asp:Label ID="Label9" runat="server" class="control-label" Text="years"></asp:Label>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblEmpExperience" runat="server" class="control-label" Text="Experience"></asp:Label>
                        </div>
                        <div class="col-md-1">
                            <asp:TextBox ID="txtExperienceFrom" runat="server" CssClass="form-control" Width="88"></asp:TextBox>                           
                        </div>
                        <div class="col-md-1" style="text-align: right;">
                            <asp:Label ID="Label10" runat="server" class="control-label" Text="to"></asp:Label>
                        </div>
                        <div class="col-md-1" style="text-align: right;">
                            <asp:TextBox ID="txtExperienceTo" runat="server" CssClass="form-control" Width="88"></asp:TextBox>
                        </div>
                        <div class="col-md-1" style="text-align: right;">
                            <asp:Label ID="Label12" runat="server" class="control-label" Text="years"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblBloodGroup" runat="server" class="control-label" Text="Blood Group"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlBloodGroup" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblEmpDivision" runat="server" class="control-label" Text="Division"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlEmpDivision" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblEmpDistrict" runat="server" class="control-label" Text="District"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlEmpDistrict" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblEmpThana" runat="server" class="control-label" Text="Thana/Upazilla"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlEmpThana" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <button type="button" id="btnSearch" class="TransactionalButton btn btn-primary">
                            Search</button>
                    </div>
                </div>
                <div id="SearchOutput">
                    <div class="panel-body">
                        <table id='gvResumeSearch' class="table table-bordered table-condensed table-responsive"
                            width="100%">
                            <colgroup>
                                <col style="width: 7%;" />
                                <col style="width: 43%;" />
                                <col style="width: 20%;" />
                                <col style="width: 10%;" />
                                <col style="width: 10%;" />
                                <col style="width: 10%;" />
                            </colgroup>
                            <thead>
                                <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                    <th style="text-align:center">
                                        <input type="checkbox"  id="checkAllResume" title="Select All Applicant" />
                                    </th>
                                    <th style="text-align: left;">
                                        Name
                                    </th>
                                    <th style="text-align: left;">
                                        Code
                                    </th>
                                    <th style="text-align: right;">
                                        Resume
                                    </th>
                                    <th style="text-align: right;">
                                        Documents
                                    </th>
                                    <th style="text-align: right;" id="AddasEmployeeTh">
                                        Add as Employee
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
                <div class="row">
                    <div class="col-md-12">
                        <button type="button" id="btnAssignApplicantToOpenJob" onclick="JobOpening()" class="TransactionalButton btn btn-primary">
                            Assign Applicant To Job</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="TouchKeypad" style="display: none;">
        <div id="PopTabPanel" style="width: 900px">
            <div id="PopMyTabs">
                <ul id="PoptabPage" class="ui-style">
                    <li id="PopA" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none">
                        <a href="#Poptab-1">Applicant Documents</a></li>
                </ul>
                <div id="Poptab-1">
                    <div id="imageDiv">
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-4" style="margin-top: 10px; text-align: right; float: right;">
                        <input type="button" value="Back" class="btn btn-primary" id="btnBackToDecisionMaker" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
