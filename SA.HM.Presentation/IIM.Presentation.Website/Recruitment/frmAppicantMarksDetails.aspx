<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmAppicantMarksDetails.aspx.cs" Inherits="HotelManagement.Presentation.Website.Recruitment.frmAppicantMarksDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Employee Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Appointment Letter</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#myTabs").tabs();

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_txtReportingDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $('#ContentPlaceHolder1_txtArrivalHour').timepicker({
                showPeriod: is12HourFormat
            });

            $("#chkbApplicant").change(function () {

                if ($(this).is(":checked")) {
                    $("#ApplicantResultTbl tbody tr").find("td:eq(0)").find("input").prop("checked", true);
                }
                else {
                    $("#ApplicantResultTbl tbody tr").find("td:eq(0)").find("input").prop("checked", false);
                }
            });

            $("#btnSearch").click(function () {
                if ($("#ContentPlaceHolder1_ddlJobCircular").val() == "0") {
                    toastr.warning("Please Select Job Circular");
                    return false;
                }

                var jobCircularId = $("#ContentPlaceHolder1_ddlJobCircular").val();
                var departmentId = $("#ContentPlaceHolder1_ddlDepartments").val();

                PageMethods.GetApplicantMarks(jobCircularId, departmentId, OnApplicantMarksLoadSucceeded, OnApplicantMarksLoadFailed);
                return false;
            });

        });

        function OnApplicantMarksLoadSucceeded(result) {

            var tr = "", totalRow = 1, editLink = "", deleteLink = "";
            $("#ApplicantResultTbl tbody").html("");

            $.each(result, function (count, gridObject) {

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td style = 'text-align: center; width:7%;' > <input type='checkbox' id='chk" + gridObject.ApplicantId + "' /> </td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.DepartmentName + "</td>";
                tr += "<td align='left' style=\"width:25%; cursor:pointer;\">" + gridObject.JobTitle + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.EmployeeName + "</td>";
                tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.PresentPhone + "</td>";
                tr += "<td align='left' style=\"width:13%; cursor:pointer;\">" + gridObject.MarksObtain + "</td>";
                tr += "<td align='left' style=\"display:none\">" + gridObject.ApplicantId + "</td>";

                tr += "</tr>"

                $("#ApplicantResultTbl tbody").append(tr);
                tr = "";
                totalRow += 1;
            });

        }

        function OnApplicantMarksLoadFailed(error) { }

        function GenerateAppointmentLetter() {

            if ($("#ContentPlaceHolder1_ddlJobCircular").val() == "") {
                toastr.warning("Please Select Job Circular");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtReportingDate").val() == "") {
                toastr.warning("Please Provide Applicant Reporting Date");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtArrivalHour").val() == "") {
                toastr.warning("Please Provide Applicant Reporting Hour");
                return false;
            }

            if ($("#ApplicantResultTbl tbody tr").find("td:eq(0)").find("input").is(':checked') == false) {
                toastr.warning("Please Select Applicant To Generate Appointment Letter.");
                return false;
            }
            var check = $('#ApplicantResultTbl').find('input[type=checkbox]:checked').length;
            if (parseInt(check) > 1) {
                toastr.warning("Please Check only one item");
                return false;
            }
            
            var Applicant = new Array();

            $("#ApplicantResultTbl tbody > tr").each(function () {

                if ($(this).find("td:eq(0)").find("input").is(':checked') == true) {

                    Applicant.push({
                        ApplicantId: $(this).find("td:eq(6)").text()

                    });
                }
            });

            $("#<%=hfApplicantIds.ClientID %>").val(JSON.stringify(Applicant));
            $('#btnAppointmentLetterShow').trigger('click');

            false;
        }
    </script>
    <asp:HiddenField ID="hfJobCircularId" runat="server" Value="" />
    <asp:HiddenField ID="hfIsCurrentOrPreviousPage" runat="server" Value="" />
    <asp:HiddenField ID="hfApplicantIds" runat="server" Value="" />
    <div style="display: none;">
        <asp:Button ID="btnAppointmentLetterShow" runat="server" Text="Button" ClientIDMode="Static"
            OnClick="btnAppointmentLetterShow_Click" />
    </div>
    <div id="" class="panel panel-default">
        <div class="panel-heading">
            Job Circular Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblLoanType" runat="server" class="control-label required-field" Text="Job Circular"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlJobCircular" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Department"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlDepartments" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Reporting Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtReportingDate" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Reporting Time"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtArrivalHour" placeholder="12" CssClass="form-control" runat="server"
                            TabIndex="56"></asp:TextBox>
                        <%--<asp:TextBox ID="txtArrivalMin" placeholder="00" CssClass="CustomMinuteSize" TabIndex="57"
                        runat="server"></asp:TextBox>
                    <asp:DropDownList ID="ddlArrivalAmPm" CssClass="CustomAMPMSize" runat="server" TabIndex="58">
                        <asp:ListItem>AM</asp:ListItem>
                        <asp:ListItem>PM</asp:ListItem>
                    </asp:DropDownList>--%>
                    </div>
                </div>
                <div class="form-group">
                    <div id="ApplicantMarksContainer">
                        <table>
                        </table>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <button type="button" id="btnSearch" class="TransactionalButton btn btn-primary">
                            Search</button>
                        <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary" />
                    </div>
                </div>
                <div id="ApplicantResultGridContainer">
                    <table id="ApplicantResultTbl" class="table table-bordered table-condensed table-responsive"
                        style="width: 100%;">
                        <colgroup>
                            <col style="width: 7%;" />
                            <col style="width: 20%;" />
                            <col style="width: 25%;" />
                            <col style="width: 20%;" />
                            <col style="width: 15%;" />
                            <col style="width: 13%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <th class="text-center">
                                    <input type="checkbox" id="chkbApplicant" title="Select Applicant" />
                                </th>
                                <th style="text-align: left;">Department
                                </th>
                                <th style="text-align: left;">Position
                                </th>
                                <th style="text-align: left;">Applicant Name
                                </th>
                                <th style="text-align: left;">Phone No
                                </th>
                                <th style="text-align: left;">Obtain Marks
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <input type="button" id="btnGenerateAppointmentLetter" value="Generate Appointment Letter"
                            class="TransactionalButton btn btn-primary" onclick="return GenerateAppointmentLetter()" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
