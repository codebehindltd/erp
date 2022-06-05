<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="CustomNoticeIframe.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.CustomNoticeIframe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/ckeditor/ckeditor.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <script>
        var isClose = false;
        var UserInformation = [];
        $(document).ready(function () {
            var noticeId = $.trim(CommonHelper.GetParameterByName("nId"));
            //$('txtContent.ckeditor').ckeditor();
            $("#ContentPlaceHolder1_ddlAssignTo").select2({
            });
            CKEDITOR.replace('txtContent');
            CKEDITOR.config.removePlugins = 'newpage,save,flash,iframe,about,print';
            $('#ContentPlaceHolder1_txtCloseDate').datepicker({
                changeMonth: true,
                changeYear: true,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                minDate: DayOpenDate
            }).datepicker();
            if (noticeId != "") {
                FillForm(noticeId);
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

        function FillForm(id) {
            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../HMCommon/CustomNoticeIframe.aspx/GetNoticeInfoById',
                data: JSON.stringify({ Id: id }),
                dataType: "json",
                async: false,
                success: function (data) {
                    $("#ContentPlaceHolder1_Id").val(data.d.Id);
                    $("#btnSave").val("Update");
                    $("#btnSaveNContinue").hide();
                    $("#ContentPlaceHolder1_txtNoticeName").val(data.d.NoticeName);
                    $('#ContentPlaceHolder1_ddlAssignTo').val(data.d.NoticeEmpMapBOs).trigger('change');
                    var date = moment(data.d.CloseDate).format("DD/MM/YYYY")
                    $("#ContentPlaceHolder1_txtCloseDate").val(moment(data.d.CloseDate).format("DD/MM/YYYY"));
                    //$("#ContentPlaceHolder1_txtCloseDate").val(moment(data.d.CloseDate).format("mm-dd-yyyy"));
                    CKEDITOR.instances.txtContent.setData(data.d.Content);

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
                },
                error: function (result) {
                }
            });
        }
        function PerformSave() {
            var EmpDepartment = 0;
            var id = $("#ContentPlaceHolder1_Id").val();
            var noticeName = $("#ContentPlaceHolder1_txtNoticeName").val();
            var EmpId = $('#ContentPlaceHolder1_ddlAssignTo').val();
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
            //var EmpList = $('#ContentPlaceHolder1_hfSelectedEmpId').val(EmpId).val();
                        
            var content = CKEDITOR.instances.txtContent.getData();
            if (noticeName == "") {
                toastr.warning("Enter Notice Name");
                $("#ContentPlaceHolder1_txtNoticeName").focus();
                return false;
            }
            var closeDate = $('#ContentPlaceHolder1_txtCloseDate').val();
            if (closeDate == "") {
                toastr.warning("Enter Close Date");
                $("#ContentPlaceHolder1_txtCloseDate").focus();
                return false;
            }
            var closeDate = CommonHelper.DateFormatToMMDDYYYY(closeDate, '/');
            if (content == "") {
                toastr.warning("Add Notice Content");
                return false;
            }
            if (EmpList.length <= 0) {
                toastr.warning("Add At least one employee");
                return false;
            }
            var notice = {
                Id: id,
                NoticeName: noticeName,
                Content: content,
                CloseDate: closeDate,
                AssignType: AssignType,
                EmpDepartment: EmpDepartment
            }
            PageMethods.SaveOrUpdateNotice(notice, EmpList, OnSuccessSaveOrUpdate, OnFailSaveOrUpdate);
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
            $("#ContentPlaceHolder1_Id").val("0");
            $("#ContentPlaceHolder1_txtNoticeName").val("");
            $('#ContentPlaceHolder1_ddlAssignTo').val("").trigger('change');
            $('#ContentPlaceHolder1_hfSelectedEmpId').val("0");
            $("#ContentPlaceHolder1_txtCloseDate").val('');
            CKEDITOR.instances.txtContent.setData('');
            $("#<%=ddlAssignType.ClientID%>").val('Individual').trigger('change');
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
        function SaveNClose() {
            isClose = true;
            //SaveOrUpdateTask();
            $.when(PerformSave()).done(function () {
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
    </script>
    <div>
        <asp:HiddenField ID="hfSelectedEmpId" runat="server" Value="0" />
        <asp:HiddenField ID="Id" runat="server" Value="0" />

        <div style="padding: 10px 30px 10px 30px">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Notice Name</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtNoticeName" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Close Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCloseDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
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
                <%--<div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblAssignTo" runat="server" class="control-label" Text="Assign To"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlAssignTo" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;"></asp:DropDownList>
                    </div>
                </div>--%>
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
                <div>
                    <textarea name="txtContent" id="txtContent"></textarea>
                </div>
                <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                    <div class="col-md-12">
                        <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="SaveNClose()" value="Save & Close" />
                        <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                            onclick="javascript: return Clear();" />
                        <input id="btnSaveNContinue" type="button" class="TransactionalButton btn btn-primary btn-sm"
                            onclick="PerformSave()" value="Save & Continue" />
                        
                    </div>
                </div>
            </div>
    </div>
    </div>
</asp:Content>
