<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="VMDriverInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.VehicleManagement.VMDriverInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var payrollIntegrated = "0";
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            CommonHelper.ApplyIntigerValidation();
            GridPaging(1, 1);
            payrollIntegrated = $("#<%=hfIsPayrollIntegrateWithVehicleManagement.ClientID %>").val();

            if (payrollIntegrated == "1") {
                //// employeeLoadDivSrc employeeTxtDivSrc
                $("#employeeLoadDivSrc").show();
                $("#employeeLoadDiv").show();

                $("#NameDivSrc").hide();
                $("#NameDiv").hide();
            }
            else {
                $("#employeeLoadDivSrc").hide("slow");
                $("#employeeLoadDiv").hide("slow");

                $("#NameDiv").show("slow");
                $("#NameDivSrc").show("slow");
            }

            $("#<%=ddlEmployee.ClientID %>").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#<%=ddlEmployeeSrc.ClientID %>").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#<%=txtDoB.ClientID %>").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
        });
        function CreateNew() {
            PerformClearAction();
            $("#<%=btnSaveClose.ClientID %>").val("Save");

            $("#AddNewDiv").dialog({
                autoOpen: true,
                modal: true,
                width: '85%',
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Create New",
                show: 'slide'
            });
            return false;
        }
        function SaveAndClose() {
            var id = $(<%=hfId.ClientID%>).val();
            var driverName = "";
            var employeeId = 0;
            if (payrollIntegrated == '1') {
                driverName = $("#ContentPlaceHolder1_ddlEmployee :selected").text();
                employeeId = $(<%=ddlEmployee.ClientID%>).val();
                if (employeeId == "0") {
                    toastr.warning("Select Employee");
                    $("#ContentPlaceHolder1_ddlEmployee").focus();
                }
            }
            else {
                driverName = $(<%=txtDriverName.ClientID%>).val();
                if (driverName == "") {
                    toastr.warning("Enter Driver Name");
                    $("#ContentPlaceHolder1_txtDriverName").focus();
                }
            }
            var drivingLicenceNumber = $(<%=txtDrivingLicence.ClientID%>).val();
            if (drivingLicenceNumber == "") {
                toastr.warning("Enter Driving Licence No");
                $("#ContentPlaceHolder1_txtDrivingLicence").focus();
            }
            var dateOfBirth = $(<%=txtDoB.ClientID%>).val();
            var NID = $(<%=txtNID.ClientID%>).val();
            var phone = $(<%=txtPhone.ClientID%>).val();
            var email = $(<%=txtEmail.ClientID%>).val();
            var emergancyContactPerson = $(<%=txtEmergencyContactName.ClientID%>).val();
            var emergancyContactNumber = $(<%=txtEmergencyContactNum.ClientID%>).val();
            dateOfBirth = CommonHelper.DateFormatToMMDDYYYY(dateOfBirth, '/');

            var VMDriverInformationBO = {
                Id: id,
                DriverName: driverName,
                DrivingLicenceNumber: drivingLicenceNumber,
                DateOfBirth: dateOfBirth,
                NID: NID,
                Phone: phone,
                Email: email,
                EmergancyContactPerson: emergancyContactPerson,
                EmergancyContactNumber: emergancyContactNumber,
                EmployeeId: employeeId
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './VMDriverInformation.aspx/SaveUpdate',
                async: false,
                data: JSON.stringify({ VMDriverInformationBO: VMDriverInformationBO }),
                dataType: "json",
                success: function (data) {
                    GridPaging(1, 1);
                    PerformClearAction();
                    CommonHelper.AlertMessage(data.d.AlertMessage);
                    if (flag == 1) {
                        $('#CreateNewDialog').dialog('close');
                    }
                    flag = 0;
                },
                error: function (result) {

                }
            });
        }

        function PerformClearAction() {
            $(<%=hfId.ClientID%>).val("0");
            $(<%=ddlEmployee.ClientID%>).val("0").trigger('change');
            $(<%=txtDriverName.ClientID%>).val("");
            $(<%=txtDrivingLicence.ClientID%>).val("");
            $(<%=txtDoB.ClientID%>).val("");
            $(<%=txtNID.ClientID%>).val("");
            $(<%=txtPhone.ClientID%>).val("");
            $(<%=txtEmail.ClientID%>).val("");
            $(<%=txtEmergencyContactName.ClientID%>).val("");
            $(<%=txtEmergencyContactNum.ClientID%>).val("");
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#DriverTable tbody tr").length;
            var driverName = "";
            var employeeId = 0;
            if (payrollIntegrated == '1') {
                driverName = $("#ContentPlaceHolder1_ddlEmployeeSrc :selected").text();
                employeeId = $(<%=ddlEmployeeSrc.ClientID%>).val();
            }
            else {
                driverName = $(<%=txtDriverNameSrc.ClientID%>).val();
            }
            var licenceNumber = $("#<%=txtDrivingLicenceSrc.ClientID %>").val();
            var phone = $("#<%=txtPhoneSrc.ClientID %>").val();

            PageMethods.SearchGridPaging(driverName, licenceNumber, phone, employeeId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {
            $("#DriverTable tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#DriverTable tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#DriverTable tbody tr").length;
                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.DriverName + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.DrivingLicenceNumber + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.Phone + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.EmergancyContactNumber + "</td>";

                tr += "<td style=\"text-align: center; width:20%; cursor:pointer;\">";
                tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction(" + gridObject.Id + ");\" alt='Edit'  title='Edit' border='0' />";
                tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'PerformDeleteAction(" + gridObject.Id + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                tr += "</td>";
                tr += "<td align='right' style=\"width:5%; display:none;\">" + gridObject.Id + "</td>";
                tr += "</tr>"

                $("#DriverTable tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            return false;
        }
        function OnLoadObjectFailed() {

        }
        function PerformEditAction(Id) {
            FillForm(Id);
        }
        function FillForm(Id) {
            $("#ContentPlaceHolder1_btnClear").hide();

            CommonHelper.SpinnerOpen();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './VMDriverInformation.aspx/FillForm',

                data: "{'Id':'" + Id + "'}",
                dataType: "json",
                success: function (data) {
                    if (!confirm("Do you want to edit ")) {
                        return false;
                    }
                    $("#AddNewDiv").dialog({
                        autoOpen: true,
                        modal: true,
                        width: '85%',
                        closeOnEscape: true,
                        resizable: false,
                        height: 'auto',
                        fluid: true,
                        title: "Update Driver Information",
                        show: 'slide'
                    });
                    $("#btnSave").val("Update And Close");
                    $("#btnClear").hide();
                    $("#btnSaveNContinue").hide();
                    $(<%=hfId.ClientID%>).val(data.d.Id);
                    $(<%=ddlEmployee.ClientID%>).val(data.d.EmployeeId).trigger('change');
                    $(<%=txtDriverName.ClientID%>).val(data.d.DriverName);
                    $(<%=txtDrivingLicence.ClientID%>).val(data.d.DrivingLicenceNumber);
                    $(<%=txtDoB.ClientID%>).val(moment(data.d.ExpenseDate).format("DD/MM/YYYY"));
                    $(<%=txtNID.ClientID%>).val(data.d.NID);
                    $(<%=txtPhone.ClientID%>).val(data.d.Phone);
                    $(<%=txtEmail.ClientID%>).val(data.d.Email);
                    $(<%=txtEmergencyContactName.ClientID%>).val(data.d.EmergancyContactPerson);
                    $(<%=txtEmergencyContactNum.ClientID%>).val(data.d.EmergancyContactNumber);
                    CommonHelper.SpinnerClose();
                },
                error: function (result) {

                }
            });
            return false;
        }

        function PerformDeleteAction() {
            if (!confirm("Do you want to Delete?")) {
                return false;
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './VMDriverInformation.aspx/DeleteData',
                data: "{'Id':'" + Id + "'}",
                dataType: "json",
                success: function (data) {
                    CommonHelper.AlertMessage(data.d.AlertMessage);
                    GridPaging(1, 1);
                },
                error: function (data) {

                }
            });
            return false;
        }
    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsPayrollIntegrateWithVehicleManagement" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>

    <div id="InfoPanel" class="panel panel-default">
        <div class="panel-heading">
            Driver Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label runat="server" class="control-label required-field" Text="Driver Name"></asp:Label>
                    </div>
                    <div class="col-md-4" id="NameDivSrc">
                        <asp:TextBox ID="txtDriverNameSrc" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-10" id="employeeLoadDivSrc" style="display: none">
                        <asp:DropDownList ID="ddlEmployeeSrc" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label ">Driving Licence No.</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtDrivingLicenceSrc" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Phone</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPhoneSrc" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Email</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEmailSrc" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                    </div>

                </div>
                &nbsp;
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="javascript: return GridPaging(1,1);"
                            CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />
                        <asp:Button ID="btnAdd" runat="server" Text="New Lost & Found" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateNew();" TabIndex="6" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information
        </div>
        <div class="panel-body">
            <div class="form-group" id="LostFoundTableContainer" style="overflow: scroll;">
                <table class="table table-bordered table-condensed table-responsive" id="DriverTable"
                    style="width: 100%;">
                    <thead>
                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                            <th style="width: 20%;">Name
                            </th>
                            <th style="width: 20%;">Licence No.
                            </th>
                            <th style="width: 20%;">Contact No.
                            </th>
                            <th style="width: 20%;">Emergancy Contact No.
                            </th>
                            <th style="width: 20%;">Action
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
    <div id="AddNewDiv" style="display: none">
        <div id="AddPanel" class="panel panel-default">
            <%--<div class="panel-heading">
                New CNF
            </div>--%>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">

                        <div class="col-md-2">
                            <asp:Label runat="server" class="control-label" Text="Driver Name"></asp:Label>
                        </div>
                        <div class="col-md-4" style="display: none" id="employeeLoadDiv">
                            <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control" TabIndex="1">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4" id="NameDiv">
                            <asp:TextBox ID="txtDriverName" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                        <div class="col-md-2">
                            <label class="control-label ">Driving Licence No.</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDrivingLicence" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label ">Date of Birth</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDoB" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label ">NID</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtNID" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Phone</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label">Email</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group" id="emergencyDiv">
                        <div class="col-md-2">
                            <label class="control-label">Emergency Contact Name</label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtEmergencyContactName" CssClass="form-control" runat="server"></asp:TextBox>

                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Emergency Contact Number</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtEmergencyContactNum" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <%--<div class="form-group">
                        <div class="col-md-2">
                            <label for="Attachment" class="control-label">Attachment</label>
                        </div>
                        <div class="col-md-10">
                            <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Attach" onclick="AttachFile()" />
                        </div>
                    </div>
                    <div id="DocumentInfo">
                    </div>--%>
                    
                    &nbsp;
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnSaveClose" runat="server" Text="Save" OnClientClick="javascript:return SaveAndClose();"
                                CssClass="TransactionalButton btn btn-primary btn-sm" />
                            <%--<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript: return PerformClearAction();" />--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
