<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpSearch.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Employee Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Search Employee</li>";
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

            $('#ContentPlaceHolder1_ddlDivision').change(function () {
                LoadDistrict();
                LoadThana();
            });

            $('#ContentPlaceHolder1_ddlDistrict').change(function () {
                LoadThana();
            });

            $('#ContentPlaceHolder1_txtFromDOB').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDOB').datepicker("option", "minDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtToDOB').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDOB').datepicker("option", "maxDate", selectedDate);
                }
            });
        });

        function LoadDistrict() {
            var divisionId = $("#<%=ddlDivision.ClientID %>").val();
            PageMethods.LoadDistrict(divisionId, OnLoadDivisionSucceeded, OnLoadDivisionFailed);
            return false;
        }

        function LoadThana() {
            var districtId = $("#<%=ddlDistrict.ClientID %>").val();
            PageMethods.LoadThana(districtId, OnLoadDistrictSucceeded, OnLoadDistrictFailed);
            return false;
        }

        function OnLoadDivisionSucceeded(result) {
            var list = result;
            var controlId = '<%=ddlDistrict.ClientID%>';
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
            var controlId = '<%=ddlThana.ClientID%>';
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
            var grade = $("#<%=ddlGradeId.ClientID %>").val();
            var bloodGroup = $("#<%=ddlBloodGroup.ClientID %>").val();
            var fromDOB = $("#<%=txtFromDOB.ClientID %>").val();
            var toDOB = $("#<%=txtToDOB.ClientID %>").val();
            var divisionId = $("#<%=ddlDivision.ClientID %>").val();
            var districtId = $("#<%=ddlDistrict.ClientID %>").val();
            var thanaId = $("#<%=ddlThana.ClientID %>").val();

            PageMethods.SearchEmployeeResume(empType, department, positionName, workStation, ageFrom, ageTo, jobLengthFrom, jobLengthTo, expFrom, expTo, grade, bloodGroup, fromDOB, toDOB, divisionId, districtId, thanaId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
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
                tr += "<td align='left' style=\"width:53%; cursor:pointer;\">" + gridObject.DisplayName + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.EmpCode + "</td>";
                tr += "<td align='right' style=\"width:10%; cursor:pointer;\"><img src='../Images/ReportDocument.png' onClick= \"javascript:return GenerateResume('" + gridObject.EmpId + "')\" alt='Resume Review' ToolTip='Resume Review' border='0' /></td>";
                tr += "<td align='right' style=\"width:10%; cursor:pointer;\"><img src='../Images/ReportDocument.png' onClick= \"javascript:return DocumentsDetails('" + gridObject.EmpId + "')\" alt='Documents Review' ToolTip='Documents Review' border='0' /></td>";
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
            window.open(url, popup_window, "width=760,height=780,left=300,top=50,resizable=yes");
        }

        function DocumentsDetails(empId) {
            LoadApplicantsDocuments(empId);

            $("#TouchKeypad").dialog({
                autoOpen: true,
                modal: true,
                width: 800,
                height: 400,
                closeOnEscape: true,
                resizable: false,
                title: "Documents",
                show: 'slide'
            });

            //popup(1, 'TouchKeypad', '', 900, 666);
            //$("#popUpDiv").css('height', '657px');
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

    </script>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfddlDistrictId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfddlThanaId" runat="server"></asp:HiddenField>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div id="EmployeeSearchPanel">
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
                            <asp:Label ID="lblDesignationId" runat="server" class="control-label" Text="Designation Name"></asp:Label>
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
                            <asp:TextBox ID="txtAgeFrom" runat="server" CssClass="form-control" Width="70"></asp:TextBox>
                        </div>
                        <div class="col-md-1" style="text-align: center;">
                            <asp:Label ID="Label1" runat="server" class="control-label" Text="to"></asp:Label>
                        </div>
                        <div class="col-md-1">
                            <asp:TextBox ID="txtAgeTo" runat="server" CssClass="form-control" Width="70"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <asp:Label ID="Label2" runat="server" class="control-label" Text="years"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblJobLength" runat="server" class="control-label" Text="Job Length"></asp:Label>
                        </div>
                        <div class="col-md-1">
                            <asp:TextBox ID="txtJobLengthFrom" runat="server" CssClass="form-control" Width="70"></asp:TextBox>
                        </div>
                        <div class="col-md-1" style="text-align: center;">
                            <asp:Label ID="Label3" runat="server" class="control-label" Text="to"></asp:Label>
                        </div>
                        <div class="col-md-1">
                            <asp:TextBox ID="txtJobLengthTo" runat="server" CssClass="form-control" Width="70"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <asp:Label ID="Label4" runat="server" class="control-label" Text="years"></asp:Label>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblEmpExperience" runat="server" class="control-label" Text="Experience"></asp:Label>
                        </div>
                        <div class="col-md-1">
                            <asp:TextBox ID="txtExperienceFrom" runat="server" CssClass="form-control" Width="70"></asp:TextBox>                          
                        </div>
                        <div class="col-md-1" style="text-align: center;">
                            <asp:Label ID="Label5" runat="server" class="control-label" Text="to"></asp:Label>
                        </div>
                        <div class="col-md-1">
                            <asp:TextBox ID="txtExperienceTo" runat="server" CssClass="form-control" Width="70"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <asp:Label ID="Label6" runat="server" class="control-label" Text="years"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblGrade" runat="server" class="control-label" Text="Grade"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlGradeId" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
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
                            <asp:Label ID="lblFromDOB" runat="server" class="control-label" Text="From DOB"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtFromDOB" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblToDOB" runat="server" class="control-label" Text="To DOB"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtToDOB" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblDivision" runat="server" class="control-label" Text="Division"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblDistrict" runat="server" class="control-label" Text="District"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlDistrict" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblThana" runat="server" class="control-label" Text="Thana/Upazilla"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlThana" runat="server" CssClass="form-control">
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
            </div>
        </div>
        <div id="SearchOutput">
            <div class="panel-body">
                <table id='gvResumeSearch' class="table table-bordered table-condensed table-responsive"
                    width="100%">
                    <colgroup>
                        <col style="width: 7%;" />
                        <col style="width: 53%;" />
                        <col style="width: 20%;" />
                        <col style="width: 10%;" />
                        <col style="width: 10%;" />
                    </colgroup>
                    <thead>
                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                            <th>
                                <%--<input type="checkbox" id="checkAllResume" title="Select All Applicant" />--%>
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
    </div>
    <div id="TouchKeypad" style="display: none;">
        <div id="PopTabPanel">
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
