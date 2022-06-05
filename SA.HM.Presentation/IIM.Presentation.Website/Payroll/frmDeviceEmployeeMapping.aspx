<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmDeviceEmployeeMapping.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmDeviceEmployeeMapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        "use strict";
        var mappingEmployeeList = null;
        var EmployeeList = null;
        var mappingDeviceType = "";
        $(document).ready(function () {

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#<%=ddlEmployeeDepartment.ClientID%>").change(function () {
                let employeeDepartmentId = $("#<%=ddlEmployeeDepartment.ClientID%>").val();
                LoadEmployeeMappingTable(employeeDepartmentId);
            });

            $("#<%=ddlMappingEmployeeDepartment.ClientID %>").change(function () {
                let employeeDepartmentId = parseInt($("#<%=ddlMappingEmployeeDepartment.ClientID%>").val());
                LoadMappingEmployeeDropdown(employeeDepartmentId);
            });

        });


        function LoadEmployeeMappingTable(employeeDepartmentId) {

            var deviceId = $("#ContentPlaceHolder1_ddlDevice").val();
            if (deviceId == "0") {
                return false;
            }

            PageMethods.GetEmployeeListByDepartmentId(employeeDepartmentId, deviceId, OnSuccesEmployeeLoad, OnFailedEmployeeLoad);
        }
        function OnSuccesEmployeeLoad(result) {
            $("#tbEmployeeMapping tbody tr").remove();
            let tr = "";
            let options = "", totalRow = 0;

            if (result.Employees.length == 0) {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Employee Found</td> </tr>";
                $("#tbEmployeeMapping tbody ").append(emptyTr);
                return false;
            }
            options = "<select class='form-control' id=\"\">";
            options += "<option value=\"" + "0" + "\">" + "---Please Select---" + "</option>";
            options += " </select>";
            var DeviceId = $("#ContentPlaceHolder1_ddlDevice").val();

            EmployeeList = result.Employees;

            $.each(result.Employees, function (count, gridObject) {

                totalRow = $("#tbEmployeeMapping tbody tr").length;
                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td style=\"width:30%;\">" + gridObject.EmpCode + " - " + gridObject.DisplayName + "</td>";
                tr += "<td style=\"width:30%;\">" + options + "</td>";
                tr += "<td align='right' style=\"display:none;\">" + gridObject.EmpId + "</td>";
                tr += "<td align='right' style=\"display:none;\">" + gridObject.AttendanceDeviceEmpCode + "</td>";

                tr += "</tr>"

                $("#tbEmployeeMapping tbody").append(tr);

                tr = "";
            });
            $("#tbEmployeeMapping tbody tr").find('td:eq(1)').find('select').select2({
                tags: false,
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });


            //var DeviceList = JSON.parse($("#ContentPlaceHolder1_hfDeviceEmployee").val());
            var DeviceList = result.Devices;
            var mappingDevice = _(DeviceList).find(i => i.DeviceId == DeviceId);
            mappingEmployeeList = mappingDevice.Employees;

            if (mappingDeviceType == "Suprima") {
                LoadMappingEmployeeDropdown($("#<%=ddlMappingEmployeeDepartment.ClientID %>").val());
            }
            else {
                LoadMappingEmployeeDropdown(0);
            }
        }
        function OnFailedEmployeeLoad(error) {
            let options = "";
            options += "<option value=\"" + "0" + "\">" + "---Please Select---" + "</option>";
        }

        function LoadMappingEmployeeDropdown(employeeDepartmentId) {
            var list = new Array();
            if (employeeDepartmentId > 0)
                list = _.where(mappingEmployeeList, { DepartmentId: employeeDepartmentId });
            else
                list = mappingEmployeeList;
            let options = "";
            options += "<option value=\"" + "0" + "\">" + "---Please Select---" + "</option>";
            $.each(list, function (count, itm) {
                options += "<option value=\"" + itm.EmpCode + "\">" + itm.DisplayName + "</option>";
            });
            $("#tbEmployeeMapping tbody tr").each(function () {
                $(this).find('td:eq(1)').find('select').html(options);
                var flag = $(this).find('td:eq(3)').text();
                //check employee had mappingId
                if (flag != "0") {
                    $(this).find('td:eq(1)').find('select').val(flag);
                    //if mapping department changed then select initial value
                    if (flag != $(this).find('td:eq(1)').find('select').val())
                        $(this).find('td:eq(1)').find('select').val("0");
                }
                else {
                    $(this).find('td:eq(1)').find('select').val("0");
                }
            });
        }

        function UpdateMapping() {
            var employeeList = new Array();
            var emp, mapEmp, tag, checkDuplicate, checkMapped;
            var currentMappingId, selectedMappingId;
            //check if department is not selected
            if ($("#<%=ddlEmployeeDepartment.ClientID%>").val() == 0) {
                var row = $("#tbEmployeeMapping tbody tr").length;
                if (row < 0) {
                    toastr.warning("Select Department");
                    return false;
                }

            }
            var DeviceId = $("#ContentPlaceHolder1_ddlDevice").val();

            //check if mapping department is not selected
           <%-- if ($("#<%=ddlMappingEmployeeDepartment.ClientID%>").val() == 0 && mappingDeviceType == "Suprima") {
                toastr.warning("Select Mapping Department");
                return false;
            }--%>
            $("#tbEmployeeMapping tbody tr").each(function () {

                currentMappingId = $(this).find('td:eq(3)').text();
                selectedMappingId = $(this).find('td:eq(1)').find('select').val();

                //check if mapping id is changed or not
                if (selectedMappingId != currentMappingId) {
                    if (selectedMappingId != "0") {
                        //check duplicate in same click event
                        checkDuplicate = _.findWhere(employeeList, { MappingEmployeeCode: selectedMappingId, DeviceId });
                        //check if mapping Employee is already mapped or not
                        checkMapped = _.findWhere(EmployeeList, { AttendanceDeviceEmpCode: selectedMappingId });
                    }

                    if (checkDuplicate != null) {
                        tag = $(this).find('td:eq(1)').find('select option:selected').text();
                        toastr.warning(tag + " is selected for multiple Employee.");
                        $(this).find('td:eq(1)').find('select').val("0").focus();
                        return false;
                    }

                    if (checkMapped != null) {
                        tag = $(this).find('td:eq(1)').find('select option:selected').text();
                        $(this).find('td:eq(1)').find('select').val("0").focus();
                        if (!confirm((tag + " is already Mapped with " + checkMapped.DisplayName + ".")))
                            return false;
                    }

                    emp = parseInt($(this).find('td:eq(2)').text());
                    employeeList.push({
                        DeviceId,
                        EmployeeId: emp,
                        MappingEmployeeCode: selectedMappingId
                    });
                }
            });
            if (employeeList.length > 0) {
                PageMethods.UpdateMapping(employeeList, OnSuccessUpdating, OnErrorUpdating);
            }
            //else {
            //    toastr.warning("Nothing to Update.");
            //}
            return false;
        }

        function Clear() {
            $("#<%=ddlEmployeeDepartment.ClientID%>").val("0");
            $("#<%=ddlMappingEmployeeDepartment.ClientID%>").val("0");
            $("#tbEmployeeMapping tbody tr").remove();
        }
        function OnSuccessUpdating(returninfo) {

            $("#tbEmployeeMapping tbody tr").each(function () {

                //update left employee colmn in client side
                let empId = parseInt($(this).find('td:eq(2)').text());
                for (var i = 0; i < returninfo.ObjectList.length; i++) {
                    if (empId == returninfo.ObjectList[i].EmployeeId)
                        $(this).find('td:eq(3)').text(returninfo.ObjectList[i].MappingEmployeeCode);
                }
            });

            //update left Employee list colmn in client side
            for (var i = 0; i < returninfo.ObjectList.length; i++) {
                var match = _.find(EmployeeList, function (item) { return item.EmpId === returninfo.ObjectList[i].EmployeeId });

                if (match) {
                    match.AttendanceDeviceEmpCode = returninfo.ObjectList[i].MappingEmployeeCode;
                }
            }
            if (returninfo.IsSuccess)
                toastr.success(returninfo.AlertMessage.Message);
            else
                toastr.error(returninfo.AlertMessage.Message);
        }
        function OnErrorUpdating(error) {
            toastr.error(error.get_message());
        }

        function LoadEmployee(control) {
            var deviceId = $(control).val();

            var DeviceList = JSON.parse($("#ContentPlaceHolder1_hfDevice").val());
            var mappingDevice = _(DeviceList).find(i => i.DeviceId == deviceId);
            mappingDeviceType = mappingDevice.DeviceType;

            if (mappingDeviceType == "Suprima") {

                $("#dvMappingDepartment").show();
            }
            else {
                $("#dvMappingDepartment").hide();
                $("#ContentPlaceHolder1_ddlEmployeeDepartment").val("0").trigger('change');
            }
            return true;
        }
    </script>
    <asp:HiddenField runat="server" ID="hfDeviceEmployee" />
    <asp:HiddenField runat="server" ID="hfDevice" />
    <div id="EntryPanel" class="panel panel-default">
        <div class="form-horizontal">
            <div id="deviceEmployeeMappingDiv" class="panel panel-default">
                <div class="panel-heading">
                    Device Employee Mapping
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label Text="Device" runat="server" class="control-label required-field"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlDevice" runat="server" CssClass="form-control" onchange="LoadEmployee(this)">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group" id="dvDepartment">
                        <div class="col-md-2">
                            <asp:Label Text="Department" runat="server" class="control-label required-field"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlEmployeeDepartment" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group" id="dvMappingDepartment">
                        <div class="col-md-2">
                            <asp:Label Text="Mapping Department" runat="server" class="control-label required-field"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlMappingEmployeeDepartment" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <table id="tbEmployeeMapping" class="table table-bordered table-condensed table-responsive">
                        <thead>
                            <tr style="background-color: #44545E">
                                <th style="width: 30%;">Display Name</th>
                                <th style="width: 30%;">Mapping Name</th>
                                <th style="display: none;">Id</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnUpdate" Text="Update" CssClass="TransactionalButton btn btn-primary btn-sm" runat="server" OnClientClick="return UpdateMapping(); " />
                    <asp:Button ID="btnGetDeviceEmployee" Text="Sync Device Employee" CssClass="TransactionalButton btn btn-primary btn-sm" runat="server" OnClick="btnGetDeviceEmployee_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
