<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpAllowanceDeduction.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpAllowanceDeduction" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithBasicInfo.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Allowance/ Deduction</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            if (IsCanSave) {
                $('#ContentPlaceHolder1_btnSave').show();
            } else {
                $('#ContentPlaceHolder1_btnSave').hide();
            }

            $("#myTabs").tabs();

            $("#IndividualEmployeee").hide();
            $("#DepartmentWiseAllowanceDeduct").hide();
            $("#IndividualSearch").hide();

            $("#ContentPlaceHolder1_ddlAllowDeductType").change(function () {

                if ($("#ContentPlaceHolder1_ddlAllowDeductType").val() == "AllEmployee") {

                    $("#IndividualEmployeee").hide();
                    $("#DepartmentWiseAllowanceDeduct").hide();

                }
                else if ($("#ContentPlaceHolder1_ddlAllowDeductType").val() == "DepartmentWise") {
                    $("#IndividualEmployeee").hide();
                    $("#DepartmentWiseAllowanceDeduct").show();
                }
                else if ($("#ContentPlaceHolder1_ddlAllowDeductType").val() == "Individual") {
                    $("#IndividualEmployeee").show();
                    $("#DepartmentWiseAllowanceDeduct").hide();
                }
            });

            $("#ContentPlaceHolder1_ddlSAllowDeductType").change(function () {

                $("#<%=btnSave.ClientID %>").val("Save");

                if ($("#ContentPlaceHolder1_ddlSAllowDeductType").val() == "AllEmployee") {
                    $("#IndividualSearch").hide();

                    $("#ContentPlaceHolder1_employeeSearchForSearch_hfEmployeeId").val("0");
                    $("#ContentPlaceHolder1_employeeSearchForSearch_txtSearchEmployee").val('');
                    $("#ContentPlaceHolder1_employeeSearchForSearch_txtEmployeeName").val('');
                    $("#ContentPlaceHolder1_employeeSearchForSearch_txtDepartment").val('');
                    $("#ContentPlaceHolder1_employeeSearchForSearch_txtDesignation").val('');
                    $("#ContentPlaceHolder1_employeeSearchForSearch_txtGrade").val('');
                }
                else if ($("#ContentPlaceHolder1_ddlSAllowDeductType").val() == "Individual") {
                    $("#IndividualSearch").show();
                }
            });

            $("#ContentPlaceHolder1_ddlDepartment").change(function () {
                if ($(this).val() == "0") {
                    $("#gvEmployee tbody").html("");
                    return;
                }

                LoadEmployeeDepartmentWise();
            });

            $("#checkAllEmployee").click(function () {
                if ($(this).is(":checked") == true) {
                    $("#gvEmployee tbody tr").find("td:eq(0)").find("input").prop('checked', true);
                }
                else {
                    $("#gvEmployee tbody tr").find("td:eq(0)").find("input").prop('checked', false);
                }
            });

            $("#ContentPlaceHolder1_ddlSalaryType").change(function () {
                PageMethods.GetSalaryHeadInfoByType($(this).val(), OnLoadSalaryHeadSucceeded, OnLoadSalaryHeadFailed);
                return false;
            });

            $("#ContentPlaceHolder1_ddlEffectedMonth").change(function () {

                if ($("#ContentPlaceHolder1_ddlAllowDeductType").val() == "DepartmentWise") {
                    var effectedMonth = $("#ContentPlaceHolder1_ddlEffectedMonth").val();
                    var departmentId = $("#ContentPlaceHolder1_ddlDepartment").val();
                    var effectiveYear = $("#ContentPlaceHolder1_ddlYear").val();

                    if (effectiveYear != "0")
                        PageMethods.GetEmpAllowanceDeductionInfoByDepartmentId(departmentId, effectedMonth, effectiveYear, OnLoadSalaryAddDeductSucceeded, OnLoadSalaryAddDeductFailed);

                    return false;
                }
                return false;
            });

            $("#ContentPlaceHolder1_ddlYear").change(function () {

                if ($("#ContentPlaceHolder1_ddlAllowDeductType").val() == "DepartmentWise") {
                    var effectedMonth = $("#ContentPlaceHolder1_ddlEffectedMonth").val();
                    var departmentId = $("#ContentPlaceHolder1_ddlDepartment").val();
                    var effectiveYear = $("#ContentPlaceHolder1_ddlYear").val();

                    if (effectedMonth != "0")
                        PageMethods.GetEmpAllowanceDeductionInfoByDepartmentId(departmentId, effectedMonth, effectiveYear, OnLoadSalaryAddDeductSucceeded, OnLoadSalaryAddDeductFailed);

                    return false;
                }
                return false;
            });


            $("#ContentPlaceHolder1_gvEmpAllowanceDeduction tbody tr:eq(1)").remove();

        });

        function OnLoadSalaryAddDeductSucceeded(result) {
            var gridLength = $("#gvEmployee tbody tr").length;
            var tr = "", row = 0, id = "", md = {};
            var salaryType = '', aldedHeadId = '', amount = '', remarks = '', amountType = '', dependson = '';

            if (result.length > 0) {

                for (row = 0; row < gridLength; row++) {
                    tr = $("#gvEmployee tbody tr:eq(" + row + ")");
                    id = $(tr).find("td:eq(4)").text();

                    if ($("#chk" + id).is(":checked", true) == true) {
                        $("#chk" + id).prop("checked", false);
                    }
                }

                for (row = 0; row < gridLength; row++) {
                    tr = $("#gvEmployee tbody tr:eq(" + row + ")");
                    id = $(tr).find("td:eq(4)").text();

                    md = _.findWhere(result, { EmpId: parseInt(id, 10) });

                    if (md != null) {
                        $("#chk" + id).prop("checked", true);
                        salaryType = md.SalaryType;
                        aldedHeadId = md.SalaryHeadId;
                        amount = md.AllowDeductAmount;
                        remarks = md.Remarks;
                        amountType = md.AmountType;
                        dependson = md.DependsOn;

                        $(tr).find("td:eq(5)").text(md.EmpAllowDeductId)
                    }
                }
            }

            if (salaryType != $("#ContentPlaceHolder1_ddlSalaryType").val()) {
                $("#ContentPlaceHolder1_hfSalaryType").val(salaryType);
                PageMethods.GetSalaryHeadInfoByType(salaryType, OnLoadSalaryHeadSucceeded, OnLoadSalaryHeadFailed);
                return false;
            }

            $("#ContentPlaceHolder1_ddlAmountType").val(amountType);
            $("#ContentPlaceHolder1_ddlSalaryType").val(salaryType);
            $("#ContentPlaceHolder1_ddlSalaryHeadId").val(aldedHeadId);
            $("#ContentPlaceHolder1_txtAllowDeductAmount").val(amount);
            $("#ContentPlaceHolder1_ddlDependsOn").val(dependson);
            $("#ContentPlaceHolder1_txtRemarks").val(remarks);

        }
        function OnLoadSalaryAddDeductFailed(error) {

        }

        function OnLoadSalaryHeadSucceeded(result) {
            var list = result;
            var controlId = '<%=ddlSalaryHeadId.ClientID%>';
            var control = $('#' + controlId);
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.empty().append('<option selected="selected" value="0">--- Please Select ---</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].SalaryHead + '" value="' + list[i].SalaryHeadId + '">' + list[i].SalaryHead + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">--- Please Select ---</option>');
                }

                if ($("#ContentPlaceHolder1_hfSalaryType").val() != "")
                    control.val($("#ContentPlaceHolder1_hfSalaryType").val());
            }
        }
        function OnLoadSalaryHeadFailed() { toastr.error("Please Contact With Admin."); }

        function LoadEmployeeDepartmentWise() {
            var departmentId = $("#ContentPlaceHolder1_ddlDepartment").val();
            PageMethods.LoadDepartmentalWiseEmployee(departmentId, OnLoadEmployeeSucceeded, OnLoadEmployeeFailed);
            return false;
        }

        function OnLoadEmployeeSucceeded(result) {
            $("#gvEmployee tbody").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvEmployee tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result, function (count, gridObject) {

                totalRow += 1;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td style = 'text-align: center; width:7%;' > <input type='checkbox' id='chk" + gridObject.EmpId + "' /> </td>";
                tr += "<td align='left' style=\"width:53%; cursor:pointer;\">" + gridObject.DisplayName + "</td>";
                tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.EmpCode + "</td>";
                tr += "<td align='left' style=\"width:25%; cursor:pointer;\">" + gridObject.Designation + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + gridObject.EmpId + "</td>";
                tr += "<td align='left' style=\"display:none;\"></td>";

                tr += "</tr>"

                $("#gvEmployee tbody ").append(tr);
                tr = "";
            });

            return false;
        }
        function OnLoadEmployeeFailed(error) {
            toastr.error(error.get_message());
        }

        function PerformSaveAction() {

            var empAllowDeductId = $("#ContentPlaceHolder1_hfEmpAllowDeductId").val();
            var allowDeductType = $("#ContentPlaceHolder1_ddlAllowDeductType").val();
            var employeeId = $("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val();
            var salaryHeadId = $("#ContentPlaceHolder1_ddlSalaryHeadId").val();
            var effectedMonthRange = $("#ContentPlaceHolder1_ddlEffectedMonth").val();
            var effectedYear = $("#ContentPlaceHolder1_ddlYear").val();
            var allowDeductAmount = $("#ContentPlaceHolder1_txtAllowDeductAmount").val();
            var amountType = $("#ContentPlaceHolder1_ddlAmountType").val();
            var remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            var dependsOn = $("#ContentPlaceHolder1_ddlDependsOn").val();

            var departmentId = $("#ContentPlaceHolder1_ddlDepartment").val();

            if (allowDeductType == "") {
                toastr.warning("Please Select Allow Deduction Type");
                return false;
            }
            else if (effectedMonthRange == "0") {
                toastr.warning("Please Select Effected Month");
                return false;
            }
            else if (effectedYear == "0") {
                toastr.warning("Please Select Effected Year");
                return false;
            }
            else if (salaryHeadId == "0") {
                toastr.warning("Please Select Allowance/Deduction");
                return false;
            }
            else if (allowDeductAmount == "" || allowDeductAmount == "0") {
                toastr.warning("Please Give Amount");
                return false;
            }
            else if (CommonHelper.IsDecimal(allowDeductAmount) == false) {
                toastr.warning("Please Give Valid Amount / Days");
                return false;
            }
            else if (amountType == "0") {
                toastr.warning("Please Select Amount Type");
                return false;
            }

            if ($("#ContentPlaceHolder1_ddlAmountType").val() == "Percent(%)") {

                if ($("#ContentPlaceHolder1_ddlDependsOn").val() == "0") {
                    toastr.warning("Please Select Depends On");
                    return false;
                }
            }

            if (allowDeductType == "DepartmentWise") {

                if (departmentId == "0") {
                    toastr.warning("Please Select Department");
                    return false;
                }
                else if ($("#gvEmployee tbody tr").find("td:eq(0)").find("input").is(":checked") == false) {
                    toastr.warning("Please Select Employee");
                    return false;
                }
            }

            if (allowDeductType == "Individual") {
                if ($("#ContentPlaceHolder1_employeeSearch_hfEmployeeId").val() == "0") {
                    toastr.warning("Please Select Employee");
                    return false;
                }
            }

            if (employeeId == "0")
                employeeId = null;

            if (departmentId == "0")
                departmentId = null;

            var salaryAddDeduction = {
                EmpAllowDeductId: empAllowDeductId,
                AllowDeductType: allowDeductType,
                DepartmentId: departmentId,
                EmpId: employeeId,
                SalaryHeadId: salaryHeadId,
                AmountType: amountType,
                DependsOn: dependsOn,
                AllowDeductAmount: allowDeductAmount,
                EffectiveYear: effectedYear,
                Remarks: remarks
            };

            if (allowDeductType != "DepartmentWise") {
                PageMethods.PerformAllowanceDeductionSaveAction(salaryAddDeduction, effectedMonthRange, OnSaveAllowanceDeductionSucceed, OnSaveAllowanceDeductionFailed);
            }
            else if (allowDeductType == "DepartmentWise") {
                var EmpLst = new Array(), EmpEditLst = new Array(), EmpDeletedLst = new Array();
                var empId = '', allowDeductId = '0';

                $("#gvEmployee tbody tr").each(function () {

                    empId = $(this).find("td:eq(4)").text();
                    allowDeductId = $(this).find("td:eq(5)").text();

                    if (allowDeductId == "")
                        allowDeductId = "0";

                    if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {

                        if (allowDeductId == "0") {
                            EmpLst.push({
                                EmpId: empId,
                                EmpAllowDeductId: allowDeductId
                            });
                        }
                        else {
                            EmpEditLst.push({
                                EmpId: empId,
                                EmpAllowDeductId: allowDeductId
                            });
                        }
                    }
                    else {

                        if (allowDeductId != "0") {
                            EmpDeletedLst.push({
                                EmpId: empId,
                                EmpAllowDeductId: allowDeductId
                            });
                        }
                    }
                });

                PageMethods.SaveAllowanceDeduction(salaryAddDeduction, EmpLst, EmpEditLst, EmpDeletedLst, effectedMonthRange, OnSaveAllowanceDeductionSucceed, OnSaveAllowanceDeductionFailed);
            }

            return false;
        }
        function OnSaveAllowanceDeductionSucceed(result) {

            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);

                var type = $("#ContentPlaceHolder1_ddlAllowDeductType").val();
                var headType = $("#ContentPlaceHolder1_ddlAllowDeductType").val();

                PerformClearAction();
                $("#frmHotelManagement")[0].reset();

                $("#ContentPlaceHolder1_ddlAllowDeductType").val(type);
                $("#ContentPlaceHolder1_ddlAllowDeductType").val(headType);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnSaveAllowanceDeductionFailed(error) {
            toastr.error(error.get_message());
        }

        function OnAllowanceDeductionSucceed(result) {

            $("#ContentPlaceHolder1_gvEmpAllowanceDeduction tr:not(:first-child)").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"5\" >No Data Found</td> </tr>";
                $("#ContentPlaceHolder1_gvEmpAllowanceDeduction tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#ContentPlaceHolder1_gvEmpAllowanceDeduction tbody tr").length + 1;
                totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:30%;\">" + gridObject.SalaryHead + "</td>";
                tr += "<td align='left' style=\"width:20%;\">" + gridObject.SalaryType + "</td>";
                tr += "<td align='left' style=\"width:20%;\">" + gridObject.AllowDeductAmount + "</td>";
                tr += "<td align='left' style=\"width:20%;\">" + gridObject.AmountType + "</td>";

                if (IsCanEdit) {
                    editLink = "<a href=\"javascript:void();\" onclick=\"javascript:return PerformFillFormAction('" + gridObject.EmpAllowDeductId + "', '" + result.GridPageLinks.CurrentPageNumber + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" /> </a>";
                }
                if (IsCanDelete) {
                    deleteLink = "<a href=\"javascript:void();\" onclick=\"javascript:return PerformDeleteAction('" + gridObject.EmpAllowDeductId + "', '" + result.GridPageLinks.CurrentPageNumber + "');\"><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
                }

                tr += "<td align='center' style=\"width:10%;\">" + editLink + deleteLink + "</td>";

                tr += "</tr>"

                $("#ContentPlaceHolder1_gvEmpAllowanceDeduction tbody ").append(tr);
                tr = "";
                editLink = "";
                deleteLink = "";

            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

        }
        function OnAllowanceDeductionFailed(error) {

        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#ContentPlaceHolder1_gvEmpAllowanceDeduction tbody tr").length - 1;
            var empId = $("#ContentPlaceHolder1_employeeSearchForSearch_hfEmployeeId").val();
            var effectedMonth = $("#ContentPlaceHolder1_ddlSEffectedMonth").val();
            var effectedYear = $("#ContentPlaceHolder1_ddlSYear").val();

            if ($("#ContentPlaceHolder1_ddlSAllowDeductType").val() == "") {
                toastr.info("Please Select Allowance Deduction Type");
                return false;
            }
            else if (effectedMonth == "0") {
                toastr.info("Please Select Effected Month");
                return false;
            }
            else if (effectedYear == "0") {
                toastr.info("Please Select Effected Year");
                return false;
            }

            if ($("#ContentPlaceHolder1_ddlSAllowDeductType").val() == "Individual") {
                if (empId == "0") {
                    toastr.info("Please Select Employee");
                    return false;
                }
            }

            var allowDeductType = $("#ContentPlaceHolder1_ddlSAllowDeductType").val();

            PageMethods.LoadEmployeeAllowanceDeduction(allowDeductType, empId, effectedMonth, effectedYear, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnAllowanceDeductionSucceed, OnAllowanceDeductionFailed);
            return false;
        }


        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {

            var dateEffectFrom = new Date(result.EffectFrom);
            var dateEffectTo = new Date(result.EffectTo);
            $("#<%=ddlEffectedMonth.ClientID %>").get(0).selectedIndex = (dateEffectFrom.getMonth() + 1);
            $("#ContentPlaceHolder1_ddlAllowDeductType").val(result.AllowDeductType);

            $("#<%=txtRemarks.ClientID %>").val(result.Remarks);
            $("#<%=txtAllowDeductAmount.ClientID %>").val(result.AllowDeductAmount);
            $("#<%=ddlSalaryHeadId.ClientID %>").val(result.SalaryHeadId);
            $("#<%=hfEmpAllowDeductId.ClientID %>").val(result.EmpAllowDeductId);
            $("#ContentPlaceHolder1_ddlAmountType").val(result.AmountType);
            $("#ContentPlaceHolder1_ddlYear").val(result.EffectiveYear);
            $("#ContentPlaceHolder1_ddlDependsOn").val(result.DependsOn);

            if (result.SalaryType != $("#ContentPlaceHolder1_ddlSalaryType").val()) {
                $("#ContentPlaceHolder1_ddlSalaryType").val(result.SalaryType);
                $("#ContentPlaceHolder1_hfSalaryType").val(result.SalaryType);

                PageMethods.GetSalaryHeadInfoByType(result.SalaryType, OnLoadSalaryHeadSucceeded, OnLoadSalaryHeadFailed);
                return false;
            }

            if (result.AllowDeductType == "Individual") {
                $("#IndividualEmployeee").show();
                $("#DepartmentWiseAllowanceDeduct").hide();

                $("#ContentPlaceHolder1_employeeSearch_txtSearchEmployee").val(result.EmpCode);
                $("#ContentPlaceHolder1_employeeSearch_btnSrcEmployees").trigger("click");
            }
            if (IsCanEdit) {
                $('#ContentPlaceHolder1_btnSave').show();
            } else {
                $('#ContentPlaceHolder1_btnSave').hide();
            }

            $("#<%=btnSave.ClientID %>").val("Update");

            $("#myTabs").tabs({ active: 0 });

            return false;
        }
        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
        }

        //For Delete-------------------------        
        function PerformDeleteAction(actionId) {

            var answer = confirm("Do you want to delete this record?")
            if (answer) {
                PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            }
            return false;
        }
        function OnDeleteObjectSucceeded(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            ReloadGrid(0);
            PerformClearAction();
        }
        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function PerformClearActionForButton() {

            if (!confirm("Do you want to clear?")) {
                return false;
            }

            PerformClearAction();
        }

        //For ClearForm-------------------------
        function PerformClearAction() {

            $("#frmHotelManagement")[0].reset();

            $("#<%=ddlEffectedMonth.ClientID %>").val("0");
            $("#<%=txtAllowDeductAmount.ClientID %>").val('');
            $("#<%=txtRemarks.ClientID %>").val('');
            $("#<%=ddlSalaryHeadId.ClientID %>").val('0');
            $("#<%=hfEmpAllowDeductId.ClientID %>").val('0');
            $("#ContentPlaceHolder1_hfSalaryType").val("");
            $("#ContentPlaceHolder1_ddlAmountType").val("0");

            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }

        function ReloadGrid(IsCurrentOrPreviousPage) {
            var currentPageNumber = $("#GridPagingContainer ul li[class='active']").text();

            if (currentPageNumber == "")
                currentPageNumber = 1;

            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }

        function WorkAfterSearchEmployee() {
        }
        

    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEmpAllowDeductId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfSalaryType" runat="server" Value=""></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Salary Add/Deduct Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Salary Add/Deduct </a></li>
        </ul>
        <div id="tab-1">
            <div id="IncrementInformation" class="panel panel-default" style="margin: 0;">
                <div class="panel-heading">
                    Allowance/Deduction
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Allow Deduction Type"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlAllowDeductType" TabIndex="2" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="--- Please Select ---"></asp:ListItem>
                                    <asp:ListItem Value="AllEmployee" Text="All Employee"></asp:ListItem>
                                    <asp:ListItem Value="DepartmentWise" Text="Department Wise"></asp:ListItem>
                                    <asp:ListItem Value="Individual" Text="Individual Employee"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="DepartmentWiseAllowanceDeduct">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Department"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlDepartment" TabIndex="2" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div id="DepartmentWiseEmployeeContainer" style="margin: 5px; margin-bottom: 10px; max-height: 300px; overflow-y: scroll;">
                                <table id='gvEmployee' class="table table-bordered table-condensed table-responsive" width="100%">
                                    <colgroup>
                                        <col style="width: 7%;" />
                                        <col style="width: 53%;" />
                                        <col style="width: 15%;" />
                                        <col style="width: 25%;" />
                                    </colgroup>
                                    <thead>
                                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                            <th>
                                                <input type="checkbox" id="checkAllEmployee" title="Select All Employee" />
                                            </th>
                                            <th style="text-align: left;">Name
                                            </th>
                                            <th style="text-align: left;">Code
                                            </th>
                                            <th style="text-align: left;">Designation
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div id="IndividualEmployeee">
                            <UserControl:EmployeeSearch runat="server" ID="employeeSearch" />
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblProcessDate" runat="server" class="control-label required-field" Text="Effected Month"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlEffectedMonth" TabIndex="4" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label5" runat="server" class="control-label required-field" Text="Process Year"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlYear" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSalaryType" runat="server" class="control-label" Text="Head Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSalaryType" runat="server" CssClass="form-control" TabIndex="3">
                                    <asp:ListItem Value="Allowance">Allowance</asp:ListItem>
                                    <asp:ListItem Value="Deduction">Deduction</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSalaryHeadId" runat="server" class="control-label required-field" Text="Allow/ Deduction"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSalaryHeadId" TabIndex="2" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAmountType" runat="server" class="control-label required-field" Text="Amount Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlAmountType" runat="server" CssClass="form-control"
                                    TabIndex="9">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label7" runat="server" class="control-label" Text="Depends On"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlDependsOn" runat="server" CssClass="form-control"
                                    TabIndex="9">
                                    <asp:ListItem Value="0" Text="--- Please Select ---"></asp:ListItem>
                                    <asp:ListItem Value="Basic" Text="Basic Salary"></asp:ListItem>
                                    <asp:ListItem Value="Gross" Text="Gross Salary"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblAllowDeductAmount" runat="server" class="control-label required-field" Text="Amount / Days"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAllowDeductAmount" TabIndex="3" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" TabIndex="5" CssClass="form-control"
                                    TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <%--Right Left--%>
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                                    TabIndex="6" OnClientClick="return PerformSaveAction();" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="7" Text="Clear" CssClass="btn btn-primary"
                                    OnClientClick="javascript: return PerformClearActionForButton();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchPanel" class="panel panel-default" style="margin: 0;">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Allow Deduction Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSAllowDeductType" TabIndex="2" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="" Text="--- Please Select ---"></asp:ListItem>
                                    <asp:ListItem Value="AllEmployee" Text="All Employee"></asp:ListItem>
                                    <asp:ListItem Value="Individual" Text="Individual Employee"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Effected Month"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSEffectedMonth" TabIndex="4" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label6" runat="server" class="control-label required-field" Text="Process Year"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSYear" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="IndividualSearch">
                            <UserControl:EmployeeSearch runat="server" ID="employeeSearchForSearch" />
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" TabIndex="7" Text="Search" CssClass="btn btn-primary"
                                    OnClientClick="javascript: return GridPaging(1, 1);" />
                            </div>
                        </div>
                        <div style="margin-top: 15px;">
                            <asp:GridView ID="gvEmpAllowanceDeduction" Width="100%" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True"
                                ForeColor="#333333" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:BoundField DataField="SalaryHead" HeaderText="Allowance/ Deduction" ItemStyle-Width="30%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SalaryType" HeaderText="Salary Type" ItemStyle-Width="20%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AllowDeductAmount" HeaderText="Amount / Day" ItemStyle-Width="20%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AllowDeductAmount" HeaderText="AmountType" ItemStyle-Width="20%">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="10%">
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
                            </asp:GridView>
                        </div>
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
</asp:Content>
