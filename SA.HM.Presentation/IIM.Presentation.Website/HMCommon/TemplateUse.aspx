<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="TemplateUse.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.TemplateUse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/ckeditor/ckeditor.js"></script>
    <script type="text/javascript">
        var UserInformation = [];
        $(document).ready(function () {
            //document.getElementById("ContentPlaceHolder1_ddlSenderType").disabled = true;
            CKEDITOR.env.isCompatible = true;
            CKEDITOR.replace('txtBody');
            CKEDITOR.replace('txtBodyLetter');
            $("#<%=ddlTemplate.ClientID %>").change(function () {
                var id = $("#<%=ddlTemplate.ClientID %>").val();
                if (id == "0") {
                    Clear();
                }
                else {
                    LoadTemplate(id);
                }

            });
            $("#<%=ddlType.ClientID %>").change(function () {
                var id = $("#<%=ddlType.ClientID %>").val();
                LoadTemplateByType(id);
                ChangeType(id);
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
            $("#<%=ddlAssignType.ClientID %>").change(function () {

                //$("#IndividualMailUser tbody").html("");
                //$("#GrouplyMailUserGrid tbody").html("");

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
            $("#ContentPlaceHolder1_ddlEmpDepartment").change(function () {
                $("#ckhAll").prop("checked", false);
                $("#GrouplyAssignUserGrid tbody").html("");
                EmployeeLoadByGroup($(this).val());
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
            $("#myTabs").tabs();
            LoadSearch(1, 1);
        });
        function LoadTemplate(id) {
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './TemplateUse.aspx/GetTemplateInfo',
                data: JSON.stringify({ id: id }),
                dataType: "json",
                async: false,
                success: function (data) {
                    SucceedGetTemplete(data.d);
                },
                error: function (result) {
                    FailedGetTemplete(result.d);
                }
            });
        }
        function SucceedGetTemplete(result) {
            if (result.Id > 0) {
                $("#<%=hfId.ClientID %>").val(result.Id);
                $("#<%=ddlType.ClientID %>").val(result.Type);
                //document.getElementById("ContentPlaceHolder1_ddlType").disabled = true;
                //document.getElementById("txtBody").disabled = true;

                document.getElementById("ContentPlaceHolder1_txtSubject").disabled = true;
                if (result.Type == "Email") {
                    CKEDITOR.instances.txtBody.setData(result.Body);
                }
                else if (result.Type == "Letter") {
                    CKEDITOR.instances.txtBodyLetter.setData(result.Body);
                }
                else if (result.Type == "SMS") {
                    $("#<%=txtBodySMS.ClientID %>").val(result.Body);
                }

                //CKEDITOR.instances.txtBody.config.readOnly = true;

                $("#<%=txtSubject.ClientID %>").val(result.Subject);
                ChangeType(result.Type);
                //$("#divSenderType").show();

            }
            else {
                $("#<%=ddlType.ClientID %>").val("0");
                document.getElementById("ContentPlaceHolder1_ddlType").disabled = false;

                document.getElementById("ContentPlaceHolder1_txtSubject").disabled = false;
                CKEDITOR.instances.txtBody.setData("");
                $("#<%=txtSubject.ClientID %>").val("");
                $("#<%=hfId.ClientID %>").val("0");
            }
            return false;
        }
        function FailedGetTemplete(error) {

        }
        function LoadTemplateByType(type) {
            return $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './TemplateUse.aspx/GetTemplateInfoByType',
                data: JSON.stringify({ type: type }),
                dataType: "json",
                async: false,
                success: function (result) {
                    if (result.d.length > 0) {

                        var list = result.d;
                        var ddlItem = '<%=ddlTemplate.ClientID%>';
                        var control = $('#' + ddlItem);
                        control.empty();

                        if (list != null) {
                            if (list.length > 0) {
                                control.removeAttr("disabled");
                                control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                                for (i = 0; i < list.length; i++) {
                                    control.append('<option title="' + list[i].Name + '" value="' + list[i].Id + '">' + list[i].Name + '</option>');
                                }
                            }
                            else {
                                control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                            }
                        }
                        else {
                            control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                        }
                        return false;
                    }
                },
                error: function (result) {

                }
            });
        }
        function ChangeType(type) {

            if (type == "Email") {
                $("#divEmail").show();
                $("#btnSend").val("Send");
                $("#divSMS").hide();
            }
            else if (type == "Letter") {
                //$("#divLetter").show();
                $("#divLetter").hide();
                $("#btnSend").val("Save");
                $("#divEmail").hide();
                $("#divSMS").hide();

            }
            else if (type == "SMS") {
                $("#divEmail").hide();
                $("#divLetter").hide();
                $("#divSMS").show();
                $("#btnSend").val("Send");
            }
            else {
                $("#divEmail").hide();
                $("#divLetter").hide();
                $("#divSMS").hide();
                $("#btnSend").val("Send");
            }
        }
        function PerformSend() {
            //var EmpId = $('#ContentPlaceHolder1_ddlAssignTo').val();
            var templateId = $("#<%=ddlTemplate.ClientID%>").val();
            var type = $("#<%=ddlType.ClientID%>").val();
            var AssignType = $("#<%=ddlAssignType.ClientID%>").val();
            var subject = $("#<%=txtSubject.ClientID%>").val();
            var id = $("#<%=hfId.ClientID%>").val();
            var EmpId = [];
            if (type == "0") {
                toastr.warning("Select a Type.");
                $("#<%=ddlType.ClientID%>").focus();
                return false;
            }
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
            //var EmpList = $('#ContentPlaceHolder1_hfSelectedEmpId').val(EmpId).val();

            var body = "";
            if (type == "Email") {
                body = CKEDITOR.instances.txtBody.getData();
            }
            else if (type == "Letter") {
                body = CKEDITOR.instances.txtBodyLetter.getData();
            }
            else if (type == "SMS") {
                body = $("#<%=txtBodySMS.ClientID %>").val();
            }
            if (EmpList.length <= 0) {
                toastr.warning("Add At least one employee");
                return false;
            }
            CommonHelper.SpinnerOpen();
            PageMethods.SaveTemplateUse(id, type, templateId, subject, body, EmpList, AssignType, OnSuccessSend, OnFailSend);
            return false;
        }
        function OnSuccessSend(result) {
            if (result.IsSuccess) {
                CommonHelper.SpinnerClose();
                CommonHelper.AlertMessage(result.AlertMessage);
                Clear();
            }
            else {
                CommonHelper.SpinnerClose();
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            return false;
        }
        function OnFailSend(error) {
            toastr.warning(error);
            CommonHelper.SpinnerClose();
            return false;
        }
        function FillFormEdit(id) {
            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './TemplateUse.aspx/GetTemplateEmailInfoById',
                data: JSON.stringify({ Id: id }),
                dataType: "json",
                async: false,
                success: function (data) {
                    $("#ContentPlaceHolder1_Id").val(data.d.Id);
                    $("#btnSave").val("Update");
                    $("#<%=txtName.ClientID%>").val(data.d.Name);
                    $("#<%=ddlTemplate.ClientID%>").val(data.d.TemplateId).trigger('change');
                    $("#<%=ddlType.ClientID%>").val(data.d.TemplateType);
                    $("#<%=ddlAssignType.ClientID%>").val(data.d.AssignType);
                    $("#<%=txtSubject.ClientID%>").val(data.d.Subject);
                    $("#<%=hfId.ClientID%>").val(data.d.Id);
                    var type = data.d.TemplateType;
                    if (type == "Email") {
                        CKEDITOR.instances.txtBody.setData(data.d.TemplateBody);
                    }
                    else if (type == "Letter") {
                        CKEDITOR.instances.txtBodyLetter.setData(data.d.TemplateBody);
                    }
                    else if (type == "SMS") {
                        $("#<%=txtBodySMS.ClientID %>").val(data.d.TemplateBody);

                    }

                    if (data.d.AssignType == 'Individual') {
                        IndividualTableLoadOnFillForm(data.d.EmployeeList)
                    }
                    else if (data.d.AssignType == 'Department') {
                        $("#<%=ddlAssignType.ClientID%>").val(data.d.AssignType).trigger('change');
                        $("#<%=ddlEmpDepartment.ClientID%>").val(data.d.EmpDepartment).trigger('change');
                        $("#GrouplyAssignUserGrid tbody tr").each(function () {
                            for (i = 0; i < data.d.EmployeeList.length; i++) {
                                if (parseFloat($.trim($(this).find("td:eq(4)").text())) == data.d.EmployeeList[i].EmpId) {
                                    $(this).find("td:eq(0)").find("input").prop("checked", true);
                                }
                            }
                        });
                    }
                    else if (data.d.AssignType == 'All') {
                        $("#<%=ddlAssignType.ClientID%>").val(data.d.AssignType).trigger('change');
                        $("#AllAssignUserGrid tbody tr").each(function () {
                            for (i = 0; i < data.d.EmployeeList.length; i++) {
                                if (parseFloat($.trim($(this).find("td:eq(4)").text())) == data.d.EmployeeList[i].EmpId) {
                                    $(this).find("td:eq(0)").find("input").prop("checked", true);
                                }
                            }
                        });
                    }
                    $("#myTabs").tabs({ active: 0 });
                },
                error: function (result) {
                }
            });
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
        function DeleteItem(Id) {
            if (!confirm("Do you want to delete?")) {
                return;
            }
            $(Id).parent().parent().remove();
            PageMethods.DeleteData(Id, DeleteSucceed, DeleteFailed);
            return false;
        }
        function DeleteSucceed(result) {
            LoadSearch(1, 1);
            $("#myTabs").tabs({ active: 1 });
            CommonHelper.AlertMessage(result.AlertMessage);
            return false;
        }
        function DeleteFailed(error) {
            CommonHelper.AlertMessage(error.AlertMessage);
            return false;
        }
        //employee type functions
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

        function ShowDocumentReport(Id, empId) {
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
        function AddIndividualEmployee() {
            var templateId = $("#<%=ddlTemplate.ClientID%>").val();
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



            tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return ShowDocumentReport('" + templateId + "','" + UserInformation[0].EmpId + "')\" alt='Show Letter' title='Show Letter' border='0' />";



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
        function Clear() {
            $("#AllAssignContainer").hide();
            $("#GroupAssignContainer").hide();
            $("#IndividualAssignUser tbody").html("");
            $("#GrouplyAssignUserGrid tbody").html("");
            $("#AllAssignUserGrid tbody").html("");
            $("#ContentPlaceHolder1_hfId").val("0");

            $('#ContentPlaceHolder1_hfSelectedEmpId').val("0");
            CKEDITOR.instances.txtBody.setData('');
            CKEDITOR.instances.txtBodyLetter.setData('');
            $("#<%=txtBodySMS.ClientID %>").val('');
            $("#<%=ddlAssignType.ClientID%>").val('Individual').trigger('change');
            $("#<%=txtSubject.ClientID%>").val("");
            $("#<%=ddlTemplate.ClientID%>").val("0").trigger('click');
            $("#<%=ddlType.ClientID%>").val("0").trigger('click');
            document.getElementById("ContentPlaceHolder1_txtSubject").disabled = false;
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadSearch(pageNumber, IsCurrentOrPreviousPage);
        }
        function LoadSearch(pageNumber, IsCurrentOrPreviousPage) {
            var name = "", type = "", templateId = "", subject = "";
            var gridRecordsCount = $("#ContactTable tbody tr").length;
            name = $("#<%=txtName.ClientID %>").val();
            type = $("#<%=txtType.ClientID %>").val();
            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './TemplateUse.aspx/LoadSearch',
                data: JSON.stringify({ name: name, type: type, gridRecordsCount: gridRecordsCount, pageNumber: pageNumber, isCurrentOrPreviousPage: IsCurrentOrPreviousPage }),

                dataType: "json",
                success: function (data) {

                    LoadTable(data);
                },
                error: function (result) {
                    CommonHelper.AlertMessage(result.d.AlertMessage);
                }
            });
            return false;
        }
        function LoadTable(searchData) {

            var rowLength = $("#ContactTable tbody tr").length;
            var dataLength = searchData.length;
            $("#ContactTable tbody").empty();
            $("#GridPagingContainer ul").empty();
            i = 0;

            if (searchData.d.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"6\" >No Data Found</td> </tr>";
                $("#ContactTable tbody ").append(emptyTr);
                return false;
            }

            $.each(searchData.d.GridData, function (count, gridObject) {

                var tr = "";

                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }
                tr += "<td style='width:15%;'>" + gridObject.Name + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.TemplateType + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.TemplateFor + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.AssignType + "</td>";

                tr += "<td style='width:10%;'> <a onclick=\"javascript:return FillFormEdit(" + gridObject.Id + ",\'" + gridObject.Name + '\');"' + "title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                tr += "&nbsp;&nbsp;<a href='#' onclick= 'DeleteItem(" + gridObject.Id + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";


                tr += "<td style='display:none'>" + gridObject.Id + "</td>";
                tr += "</tr>";

                $("#ContactTable tbody").append(tr);

                tr = "";
                i++;
            });
            //PerformCancleAction();
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.NextButton);
            return false;
        }

    </script>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfSelectedEmpId" runat="server" Value="0" />
    <div id="ShowNoticeDiv" style="display: none;">
                    <iframe id="frmNotice" name="IframeName" width="100%" height="100%" runat="server"
                        clientidmode="static" scrolling="yes" style="height: 620px;"></iframe>
                </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Template Use</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Used Template</a></li>
        </ul>
        <div id="tab-1">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Template Use
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div>
                                <div class="col-md-2">
                                    <label class="control-label required-field">Communication Type</label>
                                </div>
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" TabIndex="1">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <label class="control-label">Template</label>
                            </div>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlTemplate" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>

                        </div>
                        <div class="form-group">
                            <%--<div id="divSenderType" style="display: none">
                        <div class="col-md-2">
                            <label class="control-label">Sender Type</label>
                        </div>
                        <div class="col-sm-4">
                            <asp:DropDownList ID="ddlSenderType" runat="server" CssClass="form-control" TabIndex="1">
                            </asp:DropDownList>
                        </div>
                    </div>--%>
                            <div class="col-md-2">
                                <label class="control-label">Employee Type</label>
                            </div>
                            <div class="col-sm-10">
                                <asp:DropDownList ID="ddlAssignType" runat="server" CssClass="form-control" TabIndex="1">
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
                    </div>
                </div>
                <div class="panel panel-default" id="divEmail" style="display: none">
                    <div class="panel-heading">
                        Email
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="control-label col-md-2 required-field">Subject</label>
                                <div class="col-sm-10">

                                    <asp:TextBox ID="txtSubject" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-md-2 required-field">Body</label>
                                <div class="col-sm-10">
                                    <textarea id="txtBody" name="txtBody"></textarea>
                                    <%--<asp:TextBox ID="txtBody" CssClass="form-control" runat="server" TextMode="MultiLine" Wrap="true"></asp:TextBox>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default" id="divLetter" style="display: none">
                    <div class="panel-heading">
                        Letter
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <%--<div class="form-group">
                        <label class="control-label col-md-2 required-field">Subject</label>
                        <div class="col-sm-10">

                            <asp:TextBox ID="TextBox1" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>--%>
                            <div class="form-group">
                                <label class="control-label col-md-2 required-field">Letter Body</label>
                                <div class="col-sm-10">
                                    <textarea id="txtBodyLetter" name="txtBody"></textarea>
                                    <%--<asp:TextBox ID="txtBody" CssClass="form-control" runat="server" TextMode="MultiLine" Wrap="true"></asp:TextBox>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default" id="divSMS" style="display: none">
                    <div class="panel-heading">
                        SMS
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="control-label col-md-2 required-field">SMS Body</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtBodySMS" CssClass="form-control" runat="server" TextMode="MultiLine" Wrap="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                    <div class="col-md-12">
                        <input id="btnSend" type="button" class="TransactionalButton btn btn-primary btn-sm"
                            onclick="PerformSend()" value="Send" />
                        <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                            onclick="javascript: return Clear();" />
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="AddPanel" class="panel panel-default">
                <div class="panel-heading">
                    Template Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">

                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label required-field">Template Name</label>
                            </div>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <label class="control-label required-field">Type</label>
                            </div>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtType" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>

                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="javascript: return LoadSearch(1,1);"
                                    CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />
                                <asp:Button ID="btnClearSrc" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearAction();" TabIndex="6" />
                            </div>
                        </div>
                    </div>
                </div>
                <div id="SearchPanel" class="panel panel-default">
                    <div class="panel-heading">
                        Search Information
                    </div>
                    <div class="panel-body">
                        <div class="form-group" id="ContactTableContainer" style="overflow: scroll;">
                            <table class="table table-bordered table-condensed table-responsive" id="ContactTable"
                                style="width: 100%;">
                                <thead>
                                    <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                        <th style="width: 20%;">Name
                                        </th>
                                        <th style="width: 20%;">Template Type
                                        </th>
                                        <th style="width: 20%;">Template For
                                        </th>
                                        <th style="width: 20%;">Assign Type
                                        </th>
                                        <th style="width: 10%;">Action
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
            </div>
        </div>
    </div>
</asp:Content>
