<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="AddNewDocumentIFrame.aspx.cs" Inherits="HotelManagement.Presentation.Website.DocumentManagement.AddNewDocumentIFrame" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var DocumentTable, isClose;
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;
        var DocTable = "";
        var UserInformation = [];
        $(document).ready(function () {
            var documentId = $.trim(CommonHelper.GetParameterByName("docid"));
            IsCanSave = 1;//$('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = 1;//$('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = 1;//$('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            $("#ContentPlaceHolder1_ddlReminderType").change(function () {

                if ($(this).val() == "Once")
                    $("#dvReminderDateNTime").show();
                else
                    $("#dvReminderDateNTime").hide();
            });
            $('#ContentPlaceHolder1_txtEmailReminderDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);
            $('#ContentPlaceHolder1_txtEmailReminderTime').timepicker({
                showPeriod: is12HourFormat
            });
            $("#ContentPlaceHolder1_ddlAssignTo").select2();
            if (documentId != "0") {
                PerformEdit(documentId);
            }
            else {
                Clear();
            }
            $("#ContentPlaceHolder1_ddlAssignType").change(function () {

                $("#IndividualMailUser tbody").html("");
                $("#GrouplyMailUserGrid tbody").html("");

                if ($(this).val() == "Individual") {
                    $(".GroupAssignContainer").hide();
                    $(".IndividualAssignContainer").show();
                    $(".AllAssignContainer").hide();
                }
                else if ($(this).val() == "Department") {
                    $(".GroupAssignContainer").show();
                    $(".IndividualAssignContainer").hide();
                    $(".AllAssignContainer").hide();
                }
                else if ($(this).val() == "All") {
                    $(".AllAssignContainer").show();
                    $(".IndividualAssignContainer").hide();
                    $(".GroupAssignContainer").hide();
                    GetAllEmployee();
                }
            });
            $("#btnAddUser").click(function () {
                var a = 0;
                if (UserInformation.length == 0) {
                    toastr.warning("Please select an user.");
                    return false;
                }
                $("#IndividualAssignUser tbody tr").each(function () {
                    if (($.trim($(this).find("td:eq(4)").text()) == UserInformation[0].EmpId)
                    ) {
                        $("#txtEmployeeName").focus();
                        toastr.warning("This Employee is already added");
                        a = 1;
                        return false;
                    }
                });
                if (a == 0)
                    AddIndividualEmployee();
                else {

                    var length = UserInformation.length;
                    UserInformation[0] = UserInformation[length - 1];
                }

                return false;
            });

            $("#btnCancelUser").click(function () {
                $("#txtEmployeeName").val("");
                UserInformation = [];
            });
            $("#txtEmployeeName").autocomplete({
                source: function (request, response) {

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../DocumentManagement/AddNewDocumentIFrame.aspx/GetEmployeeInformationAutoSearch',
                        data: "{'searchString':'" + request.term + "'}",
                        dataType: "json",
                        async: false,
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.DisplayName,
                                    value: m.EmpId,
                                    Department: m.Department,
                                    EmpCode: m.EmpCode,
                                    EmpId: m.EmpId,
                                    DisplayName: m.DisplayName,

                                };
                            });
                            response(searchData);
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                },
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field

                    UserInformation[0] = ui.item;
                    $(this).val(ui.item.label);
                }
            });
            $("#ContentPlaceHolder1_ddlEmpDepartment").change(function () {
                $("#ckhAll").prop("checked", false);
                $("#GrouplyAssignUserGrid tbody").html("");
                EmployeeLoadByGroup($(this).val());
            });
        });
        function GetAllEmployee() {
            $("#AllAssignUserGrid tbody").html("");
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../DocumentManagement/AddNewDocumentIFrame.aspx/LoadAllEmployee',
                dataType: "json",
                async: false,
                success: function (data) {

                    var rowLength = data.d.length;

                    var tr = "", i = 0;

                    for (i = 0; i < rowLength; i++) {

                        if (i % 2 == 0) {
                            tr += "<tr style='background-color:#FFFFFF;'>";
                        }
                        else {
                            tr += "<tr style='background-color:#E3EAEB;'>";
                        }

                        tr += "<td style='width:7%; text-align:center;'>";
                        tr += "<input type='checkbox' value='' />";//0
                        tr += "</td>";
                        tr += "<td style='width:53%;'>" + data.d[i].DisplayName + "</td>";//1
                        tr += "<td style='width:20%;'>" + data.d[i].EmpCode + "</td>";//2
                        tr += "<td style='width:20%;'>" + data.d[i].Department + "</td>";//3

                        tr += "<td style='display:none'>" + data.d[i].EmpId + "</td>";//4
                        tr += "</tr>";

                        $("#AllAssignUserGrid tbody").append(tr);
                        tr = "";
                    }

                    return false;

                },
                error: function (result) {
                    //alert("Error");
                }
            });
        }
        function EmployeeLoadByGroup(groupId) {
            EmployeeLoading(groupId);
            return false;
        }
        function EmployeeLoading(groupId) {
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../DocumentManagement/AddNewDocumentIFrame.aspx/LoadEmployeeByGroup',
                data: "{'groupId':'" + groupId + "'}",
                dataType: "json",
                async: false,
                success: function (data) {

                    var rowLength = data.d.length;

                    var tr = "", i = 0;

                    for (i = 0; i < rowLength; i++) {

                        if (i % 2 == 0) {
                            tr += "<tr style='background-color:#FFFFFF;'>";
                        }
                        else {
                            tr += "<tr style='background-color:#E3EAEB;'>";
                        }

                        tr += "<td style='width:7%; text-align:center;'>";
                        tr += "<input type='checkbox' value='' />";//0
                        tr += "</td>";
                        tr += "<td style='width:53%;'>" + data.d[i].DisplayName + "</td>";//1
                        tr += "<td style='width:20%;'>" + data.d[i].EmpCode + "</td>";//2
                        tr += "<td style='width:20%;'>" + data.d[i].Department + "</td>";//3

                        tr += "<td style='display:none'>" + data.d[i].EmpId + "</td>";//4
                        tr += "</tr>";

                        $("#GrouplyAssignUserGrid tbody").append(tr);
                        tr = "";
                    }

                    return false;

                },
                error: function (result) {
                    //alert("Error");
                }
            });
        }
        function AddIndividualEmployee() {

            var rowLength = $("#IndividualAssignUser tbody tr").length;

            var tr = "";

            if (rowLength % 2 == 0) {
                tr += "<tr style='background-color:#FFFFFF;'>";
            }
            else {
                tr += "<tr style='background-color:#E3EAEB;'>";
            }

            tr += "<td style='width:50%;'>" + UserInformation[0].DisplayName + "</td>";
            tr += "<td style='width:20%;'>" + UserInformation[0].EmpCode + "</td>";
            tr += "<td style='width:20%;'>" + UserInformation[0].Department + "</td>";

            tr += "<td style='width:10%;'>";
            tr += "<a href='javascript:void();' onclick= 'javascript:return DeleteUser(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
            tr += "</td>";

            tr += "<td style='display:none'>" + UserInformation[0].EmpId + "</td>";


            tr += "</tr>";

            $("#IndividualAssignUser tbody").append(tr);
            $("#txtEmployeeName").val("");


            UserInformation = [];
        }
        function DeleteUser(deleteUser) {
            $(deleteUser).parent().parent().remove();
        }

        function ChangeReminder(element) {

            if ($(element).prop("checked") == true) {
                var id = +$("#ContentPlaceHolder1_ddlAssignTo").val();
                if (id != 0) {
                    //HasEmailAddress(id);
                    $("#dvEmail").show();
                }
                else
                    $("#dvEmail").show();
            }
            else
                $("#dvEmail").hide();
            return false;
        }
        function LoadDocUploader() {

            var randomId = +$("#ContentPlaceHolder1_RandomDocId").val();
            var path = "/DocumentManagement/Images/Documents/";
            var category = "DocumentsDoc";
            var iframeid = 'frmPrint';
            //var url = "/HMCommon/FileUploadTest.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            var url = "/Common/FileUpload.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            document.getElementById(iframeid).src = url;

            $("#DocumentDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 300,
                closeOnEscape: false,
                resizable: false,
                title: "Documents Upload",
                show: 'slide'
            });

        }
        function UploadComplete() {
            var randomId = $("#ContentPlaceHolder1_RandomDocId").val();
            ShowUploadedDocument(randomId);
        }

        function ShowUploadedDocument(randomId) {
            var id = $("#ContentPlaceHolder1_hfDocumentId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            PageMethods.GetUploadedDocByWebMethod(randomId, id, deletedDoc, OnGetUploadedDocByWebMethodSucceeded, OnGetUploadedDocByWebMethodFailed);
            return false;
        }

        function OnGetUploadedDocByWebMethodSucceeded(result) {
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            DocTable = "";

            DocTable += "<table id='DocTableList' style='width:100%' class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            DocTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    DocTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    DocTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }

                DocTable += "<td align='left' style='width: 50% ; cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + "','" + result[row].Name + "');\">" + result[row].Name + "</td>";

                if (result[row].Path != "") {
                    imagePath = "<img src='" + result[row].Path + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                DocTable += "<td align='left' style='width: 30%; cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + "','" + result[row].Name + "');\">" + imagePath + "</td>";

                DocTable += "<td align='left' style='width: 20%'>";
                DocTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteDoc('" + result[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                DocTable += "</td>";
                DocTable += "</tr>";
            }
            DocTable += "</table>";

            docc = DocTable;

            $("#DocumentInfo").html(DocTable);

            return false;
        }
        function DeleteDoc(docId, rowIndex) {
            var deletedDoc = $("#<%=hfDeletedDoc.ClientID %>").val();

            if (deletedDoc != "")
                deletedDoc += "," + docId;
            else
                deletedDoc = docId;

            $("#<%=hfDeletedDoc.ClientID %>").val(deletedDoc);

            $("#trdoc" + rowIndex).remove();
        }
        function OnGetUploadedDocByWebMethodFailed(error) {
            alert(error.get_message());
        }
        function SaveOrUpdateDocument() {
            var EmpDepartment = 0;
            var id = +$("#ContentPlaceHolder1_hfDocumentId").val();
            var DocumentName = $("#ContentPlaceHolder1_txtDocumentName").val();
            if (DocumentName == "") {
                isClose = false;
                toastr.warning("Enter Document Name");
                $("#ContentPlaceHolder1_txtDocumentName").focus();
                return false;
            }
            var description = $("#ContentPlaceHolder1_txtDescription").val();
            //var EmpId = $('#ContentPlaceHolder1_ddlAssignTo').val();
            var AssignType = $("#<%=ddlAssignType.ClientID%>").val();
            var EmpId = [];
            if (AssignType == "Individual") {
                $("#IndividualAssignUser tbody tr").each(function () {
                    EmpId.push($.trim($(this).find("td:eq(4)").text()));
                });
            }
            else if (AssignType == 'Department') {
                EmpDepartment = $("#<%=ddlEmpDepartment.ClientID%>").val();
                $("#GrouplyAssignUserGrid tbody tr").each(function () {
                    if ($(this).find("td:eq(0)").find("input").is(":checked")) {
                        EmpId.push($.trim($(this).find("td:eq(4)").text()));
                    }
                });
            }
            else if (AssignType == 'All') {
                $("#AllAssignUserGrid tbody tr").each(function () {
                    if ($(this).find("td:eq(0)").find("input").is(":checked")) {
                        EmpId.push($.trim($(this).find("td:eq(4)").text()));
                    }
                });
            }
            var EmpList = $('#ContentPlaceHolder1_hfSelectedEmpId').val(EmpId).val();

            if (jQuery.inArray("0", EmpId) > -1 || EmpList == "") {
                isClose = false;
                toastr.warning("Select Assign To");
                $("#ContentPlaceHolder1_ddlAssignTo").focus();
                return false;
            }
            var emailDate = null, emailTime = null;

            var reminderType = null;


            if ($("#ContentPlaceHolder1_IsEnableEmailReminder").prop("checked") == true) {

                reminderType = $("#ContentPlaceHolder1_ddlReminderType").val();

                if (reminderType == "Once") {
                    emailDate = $("#ContentPlaceHolder1_txtEmailReminderDate").val();

                    if (emailDate == "") {
                        isClose = false;
                        toastr.warning("Enter Email Reminder Date");
                        $("#ContentPlaceHolder1_txtEmailReminderDate").focus();
                        return false;
                    }
                    emailDate = CommonHelper.DateFormatToMMDDYYYY(emailDate, '/');
                    emailTime = $("#ContentPlaceHolder1_txtEmailReminderTime").val();

                    if (emailTime == "") {
                        isClose = false;
                        toastr.warning("Enter Email Reminder Time");
                        $("#ContentPlaceHolder1_txtEmailReminderTime").focus();
                        return false;
                    }
                    emailTime = moment(emailTime, ["h:mm A"]).format("HH:mm");
                }
                var callToAction = $("#ContentPlaceHolder1_txtCallToAction").val();

            }

            var document = {
                Id: id,
                DocumentName: DocumentName,
                Description: description,
                EmailReminderType: reminderType,
                EmailReminderDate: emailDate,
                EmailReminderTime: emailTime,
                CallToAction: callToAction,
                AssignType: AssignType,
                EmpDepartment: EmpDepartment

            }
            var randomDocId = $("#ContentPlaceHolder1_RandomDocId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();

            PageMethods.SaveOrUpdateDocument(document, EmpList, parseInt(randomDocId), deletedDoc, OnSuccessSaveOrUpdate, OnFailSaveOrUpdate);
            return false;
        }
        function OnSuccessSaveOrUpdate(result) {
            if (result.IsSuccess) {
                if (result.IsSuccess) {
                    parent.ShowAlert(result.AlertMessage);
                }
                if (typeof parent.GridPaging === "function")
                    parent.GridPaging(1, 1);
                $("#ContentPlaceHolder1_RandomDocId").val(result.Data);
                Clear();
            }
        }

        function OnFailSaveOrUpdate(error) {
            isClose = false;
            toastr.error(error.get_message());
            return false;
        }
        function Clear() {
            $("#AllAssignContainer").hide();
            $("#GroupAssignContainer").hide();
            $("#IndividualAssignUser tbody").html("");
            $("#ContentPlaceHolder1_txtDocumentName").val('');
            $("#ContentPlaceHolder1_txtDescription").val('');
            $("#DocumentInfo").html("");
            //$("#ContentPlaceHolder1_ddlAssignTo").val(null).trigger('change');
            // $("#ContentPlaceHolder1_ddlSearchAssignTo option:selected").removeAttr("selected").trigger('change');
            $("#ContentPlaceHolder1_IsEnableEmailReminder").prop("checked", false);
            ChangeReminder($("#ContentPlaceHolder1_IsEnableEmailReminder"));
            $("#ContentPlaceHolder1_txtEmailReminderDate").val(DayOpenDate);
            $("#ContentPlaceHolder1_txtEmailReminderTime").val('');
            $("#ContentPlaceHolder1_ddlReminderType").val("Once");
            $("#<%=ddlAssignType.ClientID%>").val('Individual').trigger('change');
            if (IsCanSave) {
                $("#btnSave").val('Save & Close').show();
                $("#btnSaveNContinue").show();
            }
            else {
                $("#btnSaveNContinue").show();
                $("#btnSave").hide();
            }
            //isClose = false;
            $("#btnClear").show();
            return false;
        }
        function PerformEdit(id) {
            PageMethods.GetDocumentId(id, OnSuccessLoading, OnFailLoading)
            return false;
        }

        function OnSuccessLoading(result) {
            FillForm(result);
            return false;
        }

        function OnFailLoading(result) {

            return false;
        }
        function FillForm(Result) {
            var i = 0;
            Clear();
            $("#NewDocumentDialog").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Edit Document - " + Result.DocumentName,
                show: 'slide'
            });
            $("#ContentPlaceHolder1_hfDocumentId").val(Result.Id);
            $("#ContentPlaceHolder1_txtDocumentName").val(Result.DocumentName);

            $("#ContentPlaceHolder1_txtDescription").val(Result.Description);
            //$("#ContentPlaceHolder1_ddlAssignTo").val(Result.EmployeeList).trigger('change');
            if (Result.AssignType == 'Individual') {
                IndividualTableLoadOnFillForm(Result.EmployeeList)
            }
            else if (Result.AssignType == 'Department') {
                $("#<%=ddlAssignType.ClientID%>").val(Result.AssignType).trigger('change');
                $("#<%=ddlEmpDepartment.ClientID%>").val(Result.EmpDepartment).trigger('change');
                $("#GrouplyAssignUserGrid tbody tr").each(function () {
                    for (i = 0; i < Result.EmployeeList.length; i++) {
                        if (parseFloat($.trim($(this).find("td:eq(4)").text())) == Result.EmployeeList[i].EmpId) {
                            $(this).find("td:eq(0)").find("input").prop("checked", true);
                        }
                    }
                });
            }
            else if (Result.AssignType == 'All') {
                $("#<%=ddlAssignType.ClientID%>").val(Result.AssignType).trigger('change');
                $("#AllAssignUserGrid tbody tr").each(function () {
                    for (i = 0; i < Result.EmployeeList.length; i++) {
                        if (parseFloat($.trim($(this).find("td:eq(4)").text())) == Result.EmployeeList[i].EmpId) {
                            $(this).find("td:eq(0)").find("input").prop("checked", true);
                        }
                    }
                });
            }

            if (Result.EmailReminderType != "") {

                $("#ContentPlaceHolder1_ddlReminderType").val(Result.EmailReminderType).trigger('change');
                $("#ContentPlaceHolder1_IsEnableEmailReminder").prop("checked", true);
                $("#ContentPlaceHolder1_txtCallToAction").val(Result.CallToAction)
                ChangeReminder($("#ContentPlaceHolder1_IsEnableEmailReminder"));
                if (Result.EmailReminderType == "Once") {
                    $("#ContentPlaceHolder1_txtEmailReminderDate").val(GetStringFromDateTime(Result.EmailReminderDate));
                    $("#ContentPlaceHolder1_txtEmailReminderTime").val(moment(Result.EmailReminderTime).format("h:mm A"));
                }

            }
            else
                $("#ContentPlaceHolder1_IsEnableEmailReminder").prop("checked", false);
            if (IsCanEdit)
                $("#btnSave").val('Update & Close').show();
            else
                $("#btnSave").hide();
            $("#btnClear").hide();
            $("#btnSaveNContinue").hide();
            ShowUploadedDocument($("#ContentPlaceHolder1_RandomDocId").val());
            return false;
        }
        function SaveNClose() {
            isClose = true;
            //SaveOrUpdateDocument();
            $.when(SaveOrUpdateDocument()).done(function () {
                if (isClose) {

                    if (typeof parent.CloseDialog === "function") {
                        parent.CloseDialog();
                    }
                    if ($("#btnSave").val() == "Update and Close") {
                        $("#btnSave").val("Save And Close");
                        $("#btnSaveNContinue").show();
                        $("#btnClear").show();
                    }
                }
            });
            return false;
        }
        function ShowDocument(path, name) {
            var iframeid = 'fileIframe';
            document.getElementById(iframeid).src = path;
            $("#ShowDocumentDiv").dialog({
                autoOpen: true,
                modal: true,
                width: "82%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Document - " + name,
                show: 'slide'
            });
            return false;
        }
        function CheckAllGroupUser(topCheckBox) {
            if ($(topCheckBox).is(":checked") == true) {
                $("#GrouplyAssignUserGrid tbody tr").find("td:eq(0)").find("input").prop("checked", true);
            }
            else {
                $("#GrouplyAssignUserGrid tbody tr").find("td:eq(0)").find("input").prop("checked", false);
            }
        }
        function CheckAllUser(topCheckBox) {
            if ($(topCheckBox).is(":checked") == true) {
                $("#AllAssignUserGrid tbody tr").find("td:eq(0)").find("input").prop("checked", true);
            }
            else {
                $("#AllAssignUserGrid tbody tr").find("td:eq(0)").find("input").prop("checked", false);
            }
        }
        function IndividualTableLoadOnFillForm(data) {
            $("#IndividualAssignUser tbody").html("");
            var rowLength = data.length;

            var tr = "", i = 0;

            for (i = 0; i < rowLength; i++) {
                if (i % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:50%;'>" + data[i].DisplayName + "</td>";
                tr += "<td style='width:20%;'>" + data[i].EmpCode + "</td>";
                tr += "<td style='width:20%;'>" + data[i].Department + "</td>";

                tr += "<td style='width:10%;'>";
                tr += "<a href='javascript:void();' onclick= 'javascript:return DeleteUser(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                tr += "</td>";

                tr += "<td style='display:none'>" + data[i].EmpId + "</td>";


                tr += "</tr>";

                $("#IndividualAssignUser tbody").append(tr);
                tr = "";
            }
        }
    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />

    <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="tempDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfParentDoc" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfSelectedEmpId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSelectedEmpIdForSearch" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />
    <asp:HiddenField ID="hfDocumentId" Value="0" runat="server" />
    <div id="ShowDocumentDiv" style="display: none;">
        <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div style="padding: 10px 30px 10px 30px">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label required-field">Document Name</label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtDocumentName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblMessageMode" runat="server" class="control-label" Text="Employee Type"></asp:Label>
                </div>
                <div class="col-md-10">
                    <asp:DropDownList ID="ddlAssignType" runat="server" CssClass="form-control"
                        TabIndex="1">
                        <asp:ListItem Value="Individual" Text="Individual Employee"></asp:ListItem>
                        <asp:ListItem Value="Department" Text="Employee Department"></asp:ListItem>
                        <asp:ListItem Value="All" Text="All"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="GroupAssignContainer" style="display: none">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Employee Department"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlEmpDepartment" runat="server" CssClass="form-control"
                            TabIndex="2">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="IndividualAssignContainer">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Employee Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtEmployeeName" runat="server" TabIndex="5" CssClass="form-control"
                            ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" style="padding: 5px 0 5px 0;">
                    <input id="btnAddUser" type="button" value="Add User" tabindex="8" class="TransactionalButton btn btn-primary btn-sm" />
                    <input id="btnCancelUser" type="button" value="Cancel User" tabindex="9" class="TransactionalButton btn btn-primary btn-sm" />
                </div>
            </div>
            <div class="IndividualAssignContainer" style="width: 100%;">
                <div class="form-group">
                    <table id="IndividualAssignUser" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
                        <thead>
                            <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                                <th style="width: 50%;">Employee Name
                                </th>
                                <th style="width: 20%;">Employee Code
                                </th>
                                <th style="width: 20%;">Department
                                </th>
                                <th style="width: 10%;">Action
                                </th>
                                <th style="display: none;">Employee Id
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="GroupAssignContainer" style="width: 100%; height: 300px; overflow-y: scroll; display: none">
                <div class="form-group">
                    <table id="GrouplyAssignUserGrid" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
                        <thead>
                            <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                                <th style="width: 7%; text-align: center;">
                                    <input type='checkbox' value='' id="ckhAll" onchange="CheckAllGroupUser(this)" style="padding: 0; margin: 3px;" />
                                </th>
                                <th style="width: 53%;">Employee Name
                                </th>
                                <th style="width: 20%;">Employee Code
                                </th>
                                <th style="width: 20%;">Department
                                </th>
                                <th style="display: none;">Employee Id
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="AllAssignContainer" style="width: 100%; height: 300px; overflow-y: scroll; display: none">
                <div class="form-group">
                    <table id="AllAssignUserGrid" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
                        <thead>
                            <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                                <th style="width: 7%; text-align: center;">
                                    <input type='checkbox' value='' onchange="CheckAllUser(this)" style="padding: 0; margin: 3px;" />
                                </th>
                                <th style="width: 53%;">Employee Name
                                </th>
                                <th style="width: 20%;">Employee Code
                                </th>
                                <th style="width: 20%;">Department
                                </th>
                                <th style="display: none;">Employee Id
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
            <%-- <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblAssignTo" runat="server" class="control-label required-field" Text="Assign To"></asp:Label>
                </div>
                <div class="col-md-10">
                    <asp:DropDownList ID="ddlAssignTo" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;"></asp:DropDownList>
                </div>
            </div>--%>
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label">Attachment</label>
                </div>
                <div class="col-md-4">
                    <input id="btnImageUp" type="button" onclick="javascript: return LoadDocUploader();"
                        class="TransactionalButton btn btn-primary btn-sm" value="Upload Document..." />
                </div>
            </div>
            <div id="DocumentInfo">
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label">Description</label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox TextMode="MultiLine" ID="txtDescription" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-8">
                    <asp:CheckBox ID="IsEnableEmailReminder" TabIndex="4" runat="Server" Text="Enable email reminder?" Font-Bold="true"
                        Checked="false" CssClass="mycheckbox" TextAlign="right" onclick="ChangeReminder(this)" />
                </div>
            </div>
            <div id="dvEmail" style="display: none;">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Reminder Type</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReminderType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="Once">Once</asp:ListItem>
                            <asp:ListItem Value="Daily">Daily</asp:ListItem>
                            <asp:ListItem Value="Weekly">Weekly</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="dvReminderDateNTime" class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Email Reminder Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEmailReminderDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Email Reminder Time</label>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtEmailReminderTime" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Call To Action</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox TextMode="MultiLine" ID="txtCallToAction" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row" style="padding-bottom: 0; padding-top: 10px;">
        <div class="col-md-12">
            <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                onclick="SaveNClose()" value="Save & Close" />
            <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                onclick="javascript: return Clear();" />
            <input id="btnSaveNContinue" type="button" value="Save & Continue" class="TransactionalButton btn btn-primary btn-sm"
                onclick="javascript: return SaveOrUpdateDocument();" />
        </div>
    </div>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
</asp:Content>
