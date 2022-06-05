<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="EmployeeDashboard.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.EmployeeDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        h4 {
            color: red;
        }

        h5 {
            color: red;
        }

        p {
            color: blue;
        }

        .LeaveTypeDiv {
            border: 2px solid black;
            float: left;
            width: 182px;
            height: 110px;
            margin: 6px;
            padding-left: 4px;
            display: block;
            background-color: #d5d8dc;
        }

        .TranningTypeDiv {
            float: left;
            margin: 6px;
            padding-left: 4px;
            display: block;
            border: 2px solid white;
        }



        .BirthdayTypeDiv {
            float: left;
            margin: 6px;
            padding-left: 4px;
            display: block;
            border: 2px solid white;
        }

        .SummaryTypeDiv {
            float: left;
            margin: 6px;
            padding-left: 4px;
            display: block;
            border: 2px solid white;
        }

        .TaskTypeDiv {
            float: left;
            margin: 6px;
            padding-left: 4px;
            display: block;
            border: 2px solid white;
        }

        .LeaveTodayDiv {
            float: left;
            margin: 6px;
            padding-left: 4px;
            display: block;
        }

        .middle {
            text-align: center;
        }

        .imgCircle {
            border-radius: 50%;
            border: 2px solid Black;
        }

        hr {
            display: block;
            margin-top: 0.5em;
            margin-bottom: 0.5em;
            margin-left: auto;
            margin-right: auto;
            border-style: inset;
            border-width: 1px;
        }
    </style>

    <script type="text/javascript">
        var empId = 0;
        $(document).ready(function () {
            $("#btnNewApplication").click(function () {
                RequestOff("Leave Application", "");
            });

            $("#ContentPlaceHolder1_ddlWorkHandover").select2({
                tags: "true",
                allowClear: true,
                width: "99.75%"
            });

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", $('#ContentPlaceHolder1_txtToDate').val());
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", $('#ContentPlaceHolder1_txtFromDate').val());

                    if ($('#ContentPlaceHolder1_txtToDate').val() != '')
                        $('#ContentPlaceHolder1_txtNoOfDays').val(CommonHelper.DateDifferenceInDays($('#ContentPlaceHolder1_txtFromDate').val(), $('#ContentPlaceHolder1_txtToDate').val()) + 1);
                },
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", $('#ContentPlaceHolder1_txtFromDate').val());

                    $('#ContentPlaceHolder1_txtNoOfDays').val(CommonHelper.DateDifferenceInDays($('#ContentPlaceHolder1_txtFromDate').val(), selectedDate) + 1);
                },
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            empId = $("#<%=hfEmployeeId.ClientID %>").val();
        });
        function ShowLetter(Id) {
            PageMethods.ShowLetter(Id, empId, OnSuccessShowLetter, OnFailShowLetter);
            return false;
        }
        function OnSuccessShowLetter(result) {
            debugger;
            if (result != "") {
                var iframeid = 'frmNotice';
                //var url = "/Payroll/Reports/CompanyWiseEmployeeList.aspx?cid=" + CompanyId;
                document.getElementById(iframeid).src = result;

                $("#ShowNoticeDiv").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 1100,
                    height: 600,
                    minWidth: 550,
                    minHeight: 580,
                    closeOnEscape: true,
                    resizable: false,
                    fluid: true,
                    //title: notice,
                    show: 'slide'
                });

                return false;
            }
        }
        function OnFailShowLetter(error) {

        }
        function ShowInvoice(Id) {
            var url = "/HMCommon/Reports/CustomeNoticeReport.aspx?nId=" + Id;
            var popup_window = "Notice Preview";
            window.open(url, popup_window, "width=745,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");

        }

        function PerformProjectDetails(taskId, taskName, taskFor, taskType, empId) {
            var iframeid = 'frmTask';
            if (taskFor == 'CRM') {
                var url = "../TaskManagement/TaskFeedbackForCRMIframe.aspx?tid=" + taskId + "&eid=" + empId + "&tType=" + taskType + "&tFor=" + taskFor;
            }
            else {
                var url = "../TaskManagement/TaskDetails.aspx?tid=" + taskId + "&eid=" + empId;
            }
            document.getElementById(iframeid).src = url;

            $("#TaskDialogue").dialog({
                autoOpen: true,
                modal: true,
                width: "95%",
                height: 800,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: taskName,
                show: 'slide'
            });

            return false;
        }

        function PerformEmployeeCompanyDetails(CompanyId, CompanyName) {
            var iframeid = 'frmCompany';
            var url = "/Payroll/Reports/CompanyWiseEmployeeList.aspx?cid=" + CompanyId;
            document.getElementById(iframeid).src = url;

            $("#ShowCompanyDiv").dialog({
                autoOpen: true,
                modal: true,
                width: 1100,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: CompanyName,
                show: 'slide'
            });

            return false;
        }

        function GoToDetaisPage(empId) {
            if (!confirm("Do you want to go the details page?")) {
                return false;
            }
            window.location = "/Payroll/frmEmployeeInformation.aspx?editId=" + empId;
        }
        function RequestOff(msg, status) {
            $("#EntryPanel").show();

            $("#EntryPanel").dialog({
                autoOpen: true,
                dialogClass: 'no-close',
                modal: true,
                width: 800,
                height: 500,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                open: function (event, ui) {
                    $('#EntryPanel').css('overflow', 'hidden');
                    $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                },
                title: msg,
                show: 'slide'
            });
            var drop = document.getElementById("ContentPlaceHolder1_ddlStatus");

            if (status == "Pending") {
                if (drop.options[2].value == "Approved") {
                    drop.options[2].hidden = true;
                }
            }
            else if (status == "Checked") {
                if (drop.options[1].value == "Checked") {
                    drop.options[1].hidden = true;
                }
            }
            else {
                $("#ApprovalDiv").hide;
            }
            return false;

        }
        function OnFailed(error) {

        }

        function PerformSave() {
            var leaveInfo = new Array;
            var employeeId = 0, leaveId = 0, leaveMode = 0;
            var leaveTypeId = 0, fromDate = "", toDate = "";
            var numberOfDays = 0, workHandover = 0, leaveStatus, description;

            <%--//leaveId = $("#<%=hfLeaveId.ClientID %>").val();--%>
            leaveTypeId = $("#<%=ddlLeaveTypeId.ClientID %>").val();
            leaveMode = $("#<%=ddlLeaveMode.ClientID %>").val();
            description = $("#<%=txtRemarks.ClientID %>").val();

            workHandover = $("#<%=ddlWorkHandover.ClientID %>").val();

            fromDate = $("#<%=txtFromDate.ClientID %>").val();
            if (fromDate != "") {
                fromDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(fromDate, innBoarDateFormat);
            }
            toDate = $("#<%=txtToDate.ClientID %>").val();
            if (toDate != "") {
                toDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(toDate, innBoarDateFormat);
            }
            numberOfDays = $("#<%=txtNoOfDays.ClientID %>").val();

            if (leaveTypeId == "0") {
                toastr.warning("Please select leave type.");
                return false;
            }
            else if (leaveMode == "0") {
                toastr.warning("Please select leave mode.");
                return false;
            }
            else if (numberOfDays == "") {
                toastr.warning("Please provide no of days.");
                return false;
            }
            else if (fromDate == "") {
                toastr.warning("Please provide from date.");
                return false;
            }
            else if (toDate == "") {
                toastr.warning("Please provide to date.");
                return false;
            }
            else if (description == "") {
                toastr.warning("Please provide description");
                return false;
            }

            leaveInfo = {
                LeaveId: leaveId,
                LeaveMode: leaveMode,
                LeaveTypeId: leaveTypeId,
                FromDate: fromDate,
                ToDate: toDate,
                NoOfDays: numberOfDays,
                WorkHandover: workHandover,
                Reason: description

            }
            PageMethods.SaveLeaveInformation(leaveInfo, OnSaveSucceed, OnFailed);
        }
        function OnSaveSucceed(result) {
            if (result.IsSuccess) {
                PerformClearClose();
                toastr.success('Request off operation successful.');


            }
            else {
                toastr.error('Request off operation unsuccessful.');
            }
        }

        function PerformClearClose() {
            $('#EntryPanel').dialog('close');
            $("#ApprovalDiv").hide();
            $("#saveBtnDiv ").show();
            $("#ContentPlaceHolder1_hfEmployeeId").val("0");

            //$("#<%=ddlLeaveMode.ClientID %>").val("0");

            $("#<%=ddlLeaveTypeId.ClientID %>").val("0");

            $("#<%=txtFromDate.ClientID %>").val("");
            $("#<%=txtToDate.ClientID %>").val("");

            $("#<%=txtNoOfDays.ClientID %>").val("");
            $("#<%=txtRemarks.ClientID %>").val("");


            var drop = document.getElementById("ContentPlaceHolder1_ddlStatus");

            $("#btnSave").val("Save");
        }
        function ShowDocument(id) {

            PageMethods.LoadDocument(id, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }
        function OnLoadDocumentSucceeded(result) {
            $("#DocumentDocuments").dialog({
                autoOpen: true,
                modal: true,
                width: "70%",
                height: 300,
                closeOnEscape: true,
                resizable: false,
                title: result.DocumentBO.DocumentName,
                show: 'slide'
            });
            var guestDoc = result.DocumentsForDocList;
            var totalDoc = result.DocumentsForDocList.length;
            var row = 0;
            var imagePath = "";
            var DocumentTable = "";

            if (totalDoc != 0) {


                DocumentTable += "<table id='DocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
                DocumentTable += "<th align='left' scope='col'>Document Name</th><th align='left' scope='col'>Document</th> </tr>";

                for (row = 0; row < totalDoc; row++) {
                    var ImgSource = guestDoc[row].Path + guestDoc[row].Name;
                    if (row % 2 == 0) {
                        DocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                    }
                    else {
                        DocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                    }

                    DocumentTable += "<td align='left' style='width: 75%'><a style='color:#333333;' target='_blank' href='" + ImgSource + "'>" + guestDoc[row].Name + "</a></td>";

                    if (guestDoc[row].Path != "") {
                        if (guestDoc[row].Extention == ".jpg" || guestDoc[row].Extention == ".png")
                            imagePath = "<a style='color:#333333;' target='_blank' href='" + ImgSource + "'> <img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /></a> ";
                        else
                            imagePath = "<a style='color:#333333;' target='_blank' href='" + ImgSource + "'><img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /></a> ";
                    }
                    else
                        imagePath = "";

                    DocumentTable += "<td align='left' style='width: 25%'>" + imagePath + "</td>";

                    DocumentTable += "</tr>";
                }
                DocumentTable += "</table>";

            }

            $("#ContentPlaceHolder1_DocumentInfo").html(DocumentTable);

            $('#OthersDocDiv').html(result.DocumentsForDocList);

            $("#DescriptionTable tbody").empty();
            var tr = "";

            tr = "<tr style='background-color:#FFFFFF;'>";

            tr += "<td style='width:75%;'>" + result.DocumentBO.Description + "</td>";

            $("#DescriptionTable tbody").append(tr);
            tr = "";
            return false;
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }
        function ShowAlertMessege(Message) {
            toastr.success("Success");
            return false;
        }
        function CloseTaskDialog() {
            $("#TaskDialogue").dialog('close');
            return false;
        }

        function ReloadTaskDialog() {
            location.reload(true);
        }

        function LoadCustomNotice(path, notice) {
            var iframeid = 'frmNotice';
            //var url = "/Payroll/Reports/CompanyWiseEmployeeList.aspx?cid=" + CompanyId;
            document.getElementById(iframeid).src = path;

            $("#ShowNoticeDiv").dialog({
                autoOpen: true,
                modal: true,
                width: 1100,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: notice,
                show: 'slide'
            });

            return false;
        }
        function ShowSummaryReport(empId, type) {
            var iframeid = 'frmNotice';
            var typeId;
            if (type == "Present List") {
                typeId = "P";
                var url = "/Payroll/Reports/TodaysSummaryReport.aspx?type=" + typeId;
                document.getElementById(iframeid).src = url;
            }
            else if (type == "Late Entry") {
                typeId = "L";
                var url = "/Payroll/Reports/TodaysSummaryReport.aspx?type=" + typeId;
                document.getElementById(iframeid).src = url;
            }
            else if (type == "Absent") {
                typeId = "A";
                var url = "/Payroll/Reports/TodaysSummaryReport.aspx?type=" + typeId;
                document.getElementById(iframeid).src = url;
            }
            else if (type.indexOf('Leave') != -1) {
                var url = "/Payroll/Reports/TodaysLeaveSummaryReport.aspx?type=" + type;
                document.getElementById(iframeid).src = url;
            }

            $("#ShowNoticeDiv").dialog({
                autoOpen: true,
                modal: true,
                width: 1100,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: type,
                show: 'slide'
            });
            return false;
        }
    </script>
    <asp:HiddenField ID="hfEmployeeId" Value="0" runat="server" />
    <div id="DocumentDocuments" style="display: none;">
        <div class="col-md-5" style="margin: 0; padding: 0;">
            <div class="form-group" id="DescriptionTableContainer">
                <table class="table table-bordered table-condensed table-responsive" id="DescriptionTable"
                    style="width: 100%;">
                    <thead>
                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                            <th style="width: 75%;">Document Description
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="col-md-7" style="margin: 0; padding: 0;">
            <div class="form-group">
                <div id="DocumentInfo" runat="server" class="col-md-12">
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            <asp:Label ID="lblEmployeeName" runat="server" class="control-label" Text="Employee Name"></asp:Label>
        </div>
        <div class="panel-body">
            <div id="dashboardHeader" class="col-md-12 form-group" style="height: 200px">
                <div class="form-group">
                    <div class="col-md-3">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Literal ID="literalImageTemplete" runat="server"> </asp:Literal>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="col-md-6">
                        <div>
                            <div class="form-group">
                                <div class="col-md-12">
                                    <asp:Label ID="lblDesignation" runat="server" class="control-label" Text="Designation"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-12">
                                    <asp:Label ID="lblDepartment" runat="server" class="control-label" Text="Department"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-12">
                                    <asp:Label ID="lblEmployeeType" runat="server" class="control-label" Text="Employee Type"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-12">
                                    <asp:Label ID="lblJoinDate" runat="server" class="control-label" Text="Join Date"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-12">
                                    <asp:Label ID="lblDateOfBirth" runat="server" class="control-label" Text="Date Of Birth"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3" style="text-align: center; height: 190px">
                        <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Literal ID="literalBestEmployeeImageTemplete" runat="server"> </asp:Literal>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-4 form-group" id="Div1" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Todays Summary
                        </div>
                        <div class="panel-body form-group" style="height: 185px; overflow-y: scroll">
                            <asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Literal ID="literalSummaryTemplete" runat="server"> </asp:Literal>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="col-md-8 form-group" id="Div2" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <asp:Label ID="lblLeaveInformation" runat="server" class="control-label text-left" Text="Leave Information"></asp:Label>
                            <input id="btnNewApplication" type="button"
                                class="TransactionalButton btn btn-primary col-md-offset-6" value="Leave Application" />
                        </div>
                        <div class="panel-body form-group" style="height: 170px; overflow-x: scroll">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Literal ID="literalLeaveTemplete" runat="server"> </asp:Literal>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-6 form-group" id="LeaveTodayTemplete" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Who's Out? (Today)
                        </div>
                        <div class="panel-body form-group" style="height: 250px; overflow-y: scroll">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Literal ID="literalLeaveTodayTemplete" runat="server"> </asp:Literal>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 form-group" id="LeaveTomorrowTemplete" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Who's Out? (Tomorrow)
                        </div>
                        <div class="panel-body form-group" style="height: 250px; overflow-y: scroll">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Literal ID="literalLeaveTomorrowTemplete" runat="server"> </asp:Literal>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-6 form-group" id="CelebrationsTemplete" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Celebrations
                        </div>
                        <div class="panel-body form-group" style="height: 250px; overflow-y: scroll">
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Literal ID="literalCelebrationsTemplete" runat="server"> </asp:Literal>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 form-group" id="ProvisionPeriodTemplete" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Probation Period
                        </div>
                        <div class="panel-body form-group" style="height: 250px; overflow-y: scroll">
                            <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Literal ID="literalPeriodTempleteTemplete" runat="server"> </asp:Literal>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-6 form-group" id="TranningTemplete" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Trainning
                        </div>
                        <div class="panel-body form-group" style="height: 250px; overflow-y: scroll">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Literal ID="literalTranningTemplete" runat="server"> </asp:Literal>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 form-group" id="CompanyWiseEmployeeCountTemplete" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Employee Distribution
                        </div>
                        <div class="panel-body form-group" style="height: 250px; overflow-y: scroll">
                            <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Literal ID="literalCompanyWiseEmployeeCountTemplete" runat="server"> </asp:Literal>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-6 form-group" id="HolidayInformationTemplete" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Holiday Information
                        </div>
                        <div class="panel-body form-group" style="height: 250px; overflow-y: scroll">
                            <asp:UpdatePanel ID="UpdatePanel14" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Literal ID="literalHolidayInformationTemplete" runat="server"> </asp:Literal>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 form-group" id="LetterTemplete" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Letter
                        </div>
                        <div class="panel-body form-group" style="height: 250px; overflow-y: scroll">
                            <asp:UpdatePanel ID="UpdatePanel16" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Literal ID="literalLetterTemplete" runat="server"> </asp:Literal>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-6 form-group" id="ReminderTemplete" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Reminder
                        </div>
                        <div class="panel-body form-group" style="height: 250px; overflow-y: scroll">
                            <asp:UpdatePanel ID="UpdatePanel17" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Literal ID="literalReminderTemplete" runat="server"> </asp:Literal>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-6 form-group" id="TaskTemplete" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Task
                        </div>
                        <div class="panel-body form-group" style="height: 250px; overflow-y: scroll">
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Literal ID="literalTaskTemplete" runat="server"> </asp:Literal>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div id="EntryPanel" class="panel panel-default" style="display: none">
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label runat="server" class="control-label required-field" Text="Leave Type"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlLeaveTypeId" runat="server" CssClass="form-control"
                                        TabIndex="4">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2" style="display: none;">
                                    <asp:Label runat="server" class="control-label required-field" Text="Leave Mode"></asp:Label>
                                </div>
                                <div class="col-md-4" style="display: none;">
                                    <asp:DropDownList ID="ddlLeaveMode" runat="server" CssClass="form-control"
                                        TabIndex="5">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblFromDate" runat="server" class="control-label required-field" Text="From Date"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblToDate" runat="server" class="control-label required-field" Text="To Date"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblNoOfDays" runat="server" class="control-label required-field" Text="No Of Days"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNoOfDays" runat="server" CssClass="form-control" TabIndex="8"
                                        ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label runat="server" class="control-label" Text="Work Handover"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlWorkHandover" runat="server" CssClass="form-control"
                                        TabIndex="5">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Description"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtRemarks" runat="server" Height="170px" TextMode="multiline" CssClass="form-control"
                                        TabIndex="8">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="row" id="saveBtnDiv">
                                <div class="col-md-12">
                                    <input type="button" value="Save" style="width: 100px;" id="btnSave" onclick="PerformSave()" class="TransactionalButton btn btn-primary btn-sm" />
                                    &nbsp;
                        <input type="button" value="Close" style="width: 100px;" id="btnClear" onclick="PerformClearClose()" class="TransactionalButton btn btn-primary btn-sm" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="TaskDialogue" style="display: none;">
                    <iframe id="frmTask" name="IframeName" width="100%" height="100%" runat="server"
                        clientidmode="static" scrolling="yes" style="height: 620px;"></iframe>
                </div>
                <div id="ShowCompanyDiv" style="display: none;">
                    <iframe id="frmCompany" name="IframeName" width="100%" height="100%" runat="server"
                        clientidmode="static" scrolling="yes" style="height: 620px;"></iframe>
                </div>
                <div id="ShowNoticeDiv" style="display: none;">
                    <iframe id="frmNotice" name="IframeName" width="100%" height="100%" runat="server"
                        clientidmode="static" scrolling="yes" style="height: 620px;"></iframe>
                </div>
                <div class="col-md-6 form-group" id="NoticeTemplete" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Notice
                        </div>
                        <div class="panel-body form-group" style="height: 250px; overflow-y: scroll">
                            <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Literal ID="literalNoticeTemplete" runat="server"> </asp:Literal>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 form-group" id="FixedAssetTemplete" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Fixed Assets
                        </div>
                        <div class="panel-body form-group" style="height: 250px; overflow-y: scroll">
                            <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Literal ID="literalFixedAssetTemplete" runat="server"> </asp:Literal>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 form-group" id="AssignedDocTemplete" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Documents
                        </div>
                        <div class="panel-body form-group" style="height: 250px; overflow-y: scroll">
                            <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Literal ID="literalAssignedDocTemplete" runat="server"> </asp:Literal>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
