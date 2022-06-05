<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmBothContribution.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmBothContribution" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearch" Src="~/HMCommon/UserControl/EmployeeSearchWithBasicInfo.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var pp = [], nn = "";
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Salary Formula</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $("#ContentPlaceHolder1_ddlDepartmentId").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlSalaryHeadId").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            if ($("#ContentPlaceHolder1_ddlBasicGrossTransactionType").val() == "Individual") {
                $("#EmplyeeWiseSalaryBasic").show();
                $("#GroupWiseSalaryBasic").hide();
                var depId = $("#ContentPlaceHolder1_ddlDepartmentId").val();
                if (depId != "0") {
                    LoadDepartment(depId);
                }
            }
            else if ($("#ContentPlaceHolder1_ddlBasicGrossTransactionType").val() == "Grade") {
                $("#EmplyeeWiseSalaryBasic").hide();
                $("#GroupWiseSalaryBasic").show();
                PageMethods.GetGradeWiseBasicSalary(OnGradeWiseBasicSalarySucceeded, OnGradeWiseBasicSalaryFailed);
            }
            else {
                $("#EmplyeeWiseSalaryBasic").hide();
                $("#GroupWiseSalaryBasic").hide();

            }

            if ($("#ContentPlaceHolder1_ddlTransactionType").val() == "Individual") {
                $("#EmplyeeWiseSalaryFormula").show();
                $("#GroupWiseSalaryFormula").hide();
            }
            else {
                $("#EmplyeeWiseSalaryFormula").hide();
                $("#GroupWiseSalaryFormula").show();
            }

            var txtAmount = '<%=txtAmount.ClientID%>'
            LoadDepandsOnHead();

            $('#' + txtAmount).blur(function () {
                var txtAmount = '<%=txtAmount.ClientID%>'
                var Amount = $('#' + txtAmount).val();
                var floatAmount = parseFloat(Amount);

                if (jQuery.trim(Amount) == '' || floatAmount <= 0) {
                    toastr.info("Amount is not in correct format.");
                }
            });

            var ddlSalaryHeadId = '<%=ddlSalaryHeadId.ClientID%>'
            $('#' + ddlSalaryHeadId).change(function () {
                LoadDepandsOnHead();
            });

            var ddlTransactionType = '<%=ddlTransactionType.ClientID%>'

            $('#ContentPlaceHolder1_ddlTransactionType').change(function () {
                if ($(this).val() == "Individual") {
                    $("#EmplyeeWiseSalaryFormula").show();
                    $("#GroupWiseSalaryFormula").hide();
                }
                else {
                    $("#EmplyeeWiseSalaryFormula").hide();
                    $("#GroupWiseSalaryFormula").show();
                }
            });

            $('#ContentPlaceHolder1_ddlBasicGrossTransactionType').change(function () {
                if ($(this).val() == "Individual") {
                    $("#EmplyeeWiseSalaryBasic").show();
                    $("#GroupWiseSalaryBasic").hide();

                    var departmentId = $("#ContentPlaceHolder1_ddlDepartmentId").val();
                    if (departmentId != "0") {
                        PageMethods.GetEmployeeBasicSalary(departmentId, OnEmployeeBasicSalarySucceeded, OnEmployeeBasicSalaryFailed);
                    }
                }
                else if ($(this).val() == "Grade") {
                    $("#EmplyeeWiseSalaryBasic").hide();
                    $("#GroupWiseSalaryBasic").show();
                    PageMethods.GetGradeWiseBasicSalary(OnGradeWiseBasicSalarySucceeded, OnGradeWiseBasicSalaryFailed);
                }
                else {
                    $("#EmplyeeWiseSalaryBasic").hide();
                    $("#GroupWiseSalaryBasic").hide();
                }
            });

            $("#ContentPlaceHolder1_ddlDepartmentId").change(function () {
                var departmentId = $(this).val();
                if (departmentId == "0") { toastr.warning("Please Select Department"); return; }
                PageMethods.GetEmployeeBasicSalary(departmentId, OnEmployeeBasicSalarySucceeded, OnEmployeeBasicSalaryFailed);
            });

            var grossTranType = $("#ContentPlaceHolder1_ddlBasicGrossTransactionType").val();
            if (true) {

            }
            $("#ContentPlaceHolder1_txtAmount").blur(function () {
                var amountType = $("#ContentPlaceHolder1_ddlAmountType").val();
                var amount = $("#ContentPlaceHolder1_txtAmount").val();
                if (amountType == 'Percentage' && parseFloat(amount) > 100) {
                    toastr.warning("Percentage amount couldn't be greater than 100");
                    $("#ContentPlaceHolder1_txtAmount").focus();
                    return false; 
                }
            });
            $("#ContentPlaceHolder1_ddlAmountType").change(function () {
                var amountType = $("#ContentPlaceHolder1_ddlAmountType").val();
                var amount = $("#ContentPlaceHolder1_txtAmount").val();
                if (amountType == 'Percentage' && parseFloat(amount) > 100) {
                    toastr.warning("Percentage amount couldn't be greater than 100");
                    $("#ContentPlaceHolder1_txtAmount").focus();
                    return false; 
                }
            });
            CommonHelper.ApplyDecimalValidation();
        });
        function LoadDepartment(departmentId) {
            PageMethods.GetEmployeeBasicSalary(departmentId, OnEmployeeBasicSalarySucceeded, OnEmployeeBasicSalaryFailed);
        }
        function OnGradeWiseBasicSalarySucceeded(result) {
            $("#GradeWiseSalaryBasicContainer").html(result);
        }
        function OnGradeWiseBasicSalaryFailed() {

        }

        function OnEmployeeBasicSalarySucceeded(result) {
            $("#EmployeeSalaryBasicContainer").html(result);
        }
        function OnEmployeeBasicSalaryFailed() {

        }

        function fixedlength(textboxID, keyEvent, maxlength) {
            //validation for digits upto 'maxlength' defined by caller function
            if (textboxID.value.length > maxlength) {
                textboxID.value = textboxID.value.substr(0, maxlength);
            }
            else if (textboxID.value.length < maxlength || textboxID.value.length == maxlength) {
                textboxID.value = textboxID.value.replace(/[^\d]+/g, '');
                return true;
            }
            else
                return false;
        }

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {
            $("#<%=txtAmount.ClientID %>").val(result.Amount);
            $("#<%=ddlActiveStat.ClientID %>").val(result.ActiveStat == true ? 0 : 1);
            $("#<%=ddlAmountType.ClientID %>").val(result.AmountType);
            $("#<%=ddlDependsOn.ClientID %>").val(result.DependsOn);
            $("#<%=ddlGradeId.ClientID %>").val(result.GradeId);
            $("#<%=ddlSalaryHeadId.ClientID %>").val(result.SalaryHeadId);
            $("#<%=txtFormulaId.ClientID %>").val(result.FormulaId);
            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewSalaryFormula').hide("slow");
            $('#EntryPanel').show("slow");
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
            window.location = "frmSalaryFormula.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=txtAmount.ClientID %>").val('');
            $("#ContentPlaceHolder1_ddlTransactionType").val('Grade').trigger('change');
            $("#<%=ddlActiveStat.ClientID %>").val(0);
            $("#<%=ddlAmountType.ClientID %>").val(0);
            $("#<%=ddlDependsOn.ClientID %>").val(0);
            $("#<%=ddlGradeId.ClientID %>").val(0);
            $("#<%=ddlSalaryHeadId.ClientID %>").val(0);
            $("#<%=txtFormulaId.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");

            $("#ContentPlaceHolder1_searchEmployee_hfEmployeeId").val('0');
            $("#ContentPlaceHolder1_searchEmployee_txtEmployeeName").val('');
            $("#ContentPlaceHolder1_searchEmployee_txtDepartment").val('');
            $("#ContentPlaceHolder1_searchEmployee_hfEmployeeDepartment").val('');
            $("#ContentPlaceHolder1_searchEmployee_hfEmployeeName").val('');
            $("#ContentPlaceHolder1_searchEmployee_txtDesignation").val('');
            $("#ContentPlaceHolder1_searchEmployee_txtGrade").val('');
            $("#ContentPlaceHolder1_searchEmployee_txtJoinDate").val('');
            $("#ContentPlaceHolder1_searchEmployee_txtWorkStation").val('');
            $("#ContentPlaceHolder1_searchEmployee_txtStatus").val('');
            $("#ContentPlaceHolder1_searchEmployee_txtMail").val('');
            $("#ContentPlaceHolder1_searchEmployee_txtCode").val('');
            $("#ContentPlaceHolder1_searchEmployee_txtSearchEmployee").val('');
            $("#SalaryHeadEmployeeWiseContaainer").html("");

            return false;
        }
        function PerformClearActionWithConfirmation() {
            if (!confirm("Do you Want To Clear?")) {
                return false;
            }
            PerformClearAction();
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewSalaryFormula').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewSalaryFormula').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }
        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewSalaryFormula').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewSalaryFormula').hide("slow");
        }
        $(function () {
            $("#myTabs").tabs();
        });

        function LoadDepandsOnHead() {
            var salaryHead = $("#<%=ddlSalaryHeadId.ClientID %>").val();
            PageMethods.GetDependsOnHeadList(salaryHead, OnFillDependsOnSucceeded, OnFillDependsOnFailed);
            return false;
        }

        function OnFillDependsOnSucceeded(result) {
            var list = result;
            var controlId = '<%=ddlDependsOn.ClientID%>';
            var control = $('#' + controlId);
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.attr("disabled", false);
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].ItemName + '" value="' + list[i].ItemId + '">' + list[i].ItemName + '</option>');
                    }
                }
                else {
                    control.attr("disabled", true);
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            var ddlDependsOn = '<%=ddlDependsOn.ClientID%>'
            var selectedDependsOn = '<%=selectedDependsOn.ClientID%>'
            var val = $('#' + selectedDependsOn).val();

            if (val != "") {
                $('#' + ddlDependsOn).val(val);
            }
            return false;
        }

        function OnFillDependsOnFailed(error) {
            toastr.error(error.get_message());
        }

        function ValidateFormula() {
            var amountType = $("#ContentPlaceHolder1_ddlAmountType").val();
            var amount = $("#ContentPlaceHolder1_txtAmount").val();
            if (amountType == 'Percentage' && parseFloat(amount) > 100) {
                toastr.warning("Percentage amount couldn't be greater than 100");
                $("#ContentPlaceHolder1_txtAmount").focus();
                return false;
            }
            if ($("#ContentPlaceHolder1_ddlTransactionType").val() == "Grade") {

                if ($("#ContentPlaceHolder1_ddlGradeId").val() == "0") {
                    toastr.warning("Please Select Grade.");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_ddlSalaryHeadId").val() == "0") {
                    toastr.warning("Please Select Salary Head.");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_ddlAmountType").val() == "0") {
                    toastr.warning("Please Select Amount Type.");
                    return false;
                }
                else if ($("#ContentPlaceHolder1_ddlAmountType").val() == "Percent(%)" && $("#ContentPlaceHolder1_ddlDependsOn").val() == "0") {
                    toastr.warning("Please Select Depends On.");
                    return false;
                }
                else if (CommonHelper.IsDecimal($.trim($("#ContentPlaceHolder1_txtAmount").val())) == false) {
                    toastr.warning("Please Give Valid Amount.");
                    return false;
                }

                return true;
            }
            else if ($("#ContentPlaceHolder1_ddlTransactionType").val() == "Individual") {
                var employeeId = $("#ContentPlaceHolder1_searchEmployee_hfEmployeeId").val();
                if (employeeId == "0") {
                    toastr.warning("Please Select Employee");
                    return false;
                }
                else if ($("#EmployeeWiseSalaryFormulatbl tbody tr").find("td:eq(2)").find("input").is(":checked") == false) {
                    toastr.warning("Please Select Salary Head.");
                    return false;
                }

                var formulaId = '0', salaryHeadId = '', dependsOn = '';
                var amountType = '', amount = 0.0, transactionType = '';
                var amountTypedb = '0', depndedOndb = '0', amountdb = '0';
                var salaryFormula = new Array(), salaryFormulaEdited = new Array(), salaryFormulaDeleted = new Array();

                transactionType = $("#ContentPlaceHolder1_ddlTransactionType").val();
                var gridLength = $("#EmployeeWiseSalaryFormulatbl tbody tr").length;
                var tr = "", row = 0;

                for (row = 0; row < gridLength; row++) {

                    tr = $("#EmployeeWiseSalaryFormulatbl tbody tr:eq(" + row + ")");

                    formulaId = $.trim($(tr).find("td:eq(0)").text());

                    if (formulaId == "0") {

                        if ($(tr).find("td:eq(2)").find("input").is(":checked") == true) {

                            salaryHeadId = $(tr).find("td:eq(1)").text();
                            amountType = $(tr).find("td:eq(5)").find("select").val();
                            dependsOn = $(tr).find("td:eq(6)").find("select").val();
                            amount = $(tr).find("td:eq(7)").find("input").val();

                            if (amountType == "0") {
                                toastr.info("Please Select Amount Type.");
                                return false;
                            }
                            else if (amountType == "Percent(%)" && dependsOn == "0") {
                                toastr.info("Please Select Depends On.");
                                return false;
                            }
                            else if ($.trim(amount) == "" || $.trim(amount) == "0") {
                                toastr.info("Please Give Amount.");
                                return false;
                            }
                            else if (CommonHelper.IsDecimal($.trim(amount)) == false) {
                                toastr.info("Please Give Valid Amount.");
                                return false;
                            }

                            salaryFormula.push({
                                FormulaId: formulaId,
                                TransactionType: transactionType,
                                GradeIdOrEmployeeId: employeeId,
                                SalaryHeadId: salaryHeadId,
                                DependsOn: (dependsOn == "0" ? null : dependsOn),
                                Amount: amount,
                                AmountType: amountType,
                                ActiveStat: true
                            });
                        }
                    }
                    else {
                        salaryHeadId = $(tr).find("td:eq(1)").text();
                        amountType = $(tr).find("td:eq(5)").find("select").val();
                        dependsOn = $(tr).find("td:eq(6)").find("select").val();
                        amount = $(tr).find("td:eq(7)").find("input").val();

                        if (amountType == "0") {
                            toastr.info("Please Select Amount Type.");
                            return false;
                        }
                        else if (amountType == "Percent(%)" && dependsOn == "0") {
                            toastr.info("Please Select Depends On.");
                            return false;
                        }
                        else if ($.trim(amount) == "" || $.trim(amount) == "0") {
                            toastr.info("Please Give Amount.");
                            return false;
                        }
                        else if (CommonHelper.IsDecimal($.trim(amount)) == false) {
                            toastr.info("Please Give Valid Amount.");
                            return false;
                        }

                        amountTypedb = $(tr).find("td:eq(8)").text();
                        depndedOndb = $(tr).find("td:eq(9)").text();
                        amountdb = $(tr).find("td:eq(10)").text();

                        if ($(tr).find("td:eq(2)").find("input").is(":checked") == true) {
                            salaryFormulaEdited.push({
                                FormulaId: formulaId,
                                TransactionType: transactionType,
                                GradeIdOrEmployeeId: employeeId,
                                SalaryHeadId: salaryHeadId,
                                DependsOn: (dependsOn == "0" ? null : dependsOn),
                                Amount: amount,
                                AmountType: amountType,
                                ActiveStat: true
                            });
                        }

                        if ($(tr).find("td:eq(2)").find("input").is(":checked") == false) {
                            salaryFormulaDeleted.push({
                                FormulaId: formulaId,
                                TransactionType: transactionType,
                                GradeIdOrEmployeeId: employeeId,
                                SalaryHeadId: salaryHeadId,
                                DependsOn: (dependsOn == "0" ? null : dependsOn),
                                Amount: amount,
                                AmountType: amountType,
                                ActiveStat: false
                            });
                        }
                    }
                }

                PageMethods.SaveEmployeeWiseSalaryFormula(salaryFormula, salaryFormulaEdited, salaryFormulaDeleted, OnSaveEmployeeWiseSalaryFormulaSucceeded, OnSaveEmployeeWiseSalaryFormulaFailed);
                return false;
            }

            return false;
        }
        function OnSaveEmployeeWiseSalaryFormulaSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                ResetForm();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnSaveEmployeeWiseSalaryFormulaFailed(error) {
            toastr.warning("Error On Load. Please Try Again.");
        }

        //function SaveEmployeeNGradeWiseBasicSalary() {
        //    var empid = '0', gradeId = '', amount = 0.0, transactionType = '', amountdb = '0';
        //    var gridLength = 0, tr = "", row = 0;
        //    var employee = new Array(), grade = new Array();

        //    transactionType = $("#ContentPlaceHolder1_ddlBasicGrossTransactionType").val();

        //    if (transactionType == "Individual") {

        //        gridLength = $("#EmployeeWiseSalaryBasicTbl tbody tr").length;

        //        for (row = 0; row < gridLength; row++) {

        //            tr = $("#EmployeeWiseSalaryBasicTbl tbody tr:eq(" + row + ")");

        //            empid = $.trim($(tr).find("td:eq(0)").text());
        //            amount = $(tr).find("td:eq(4)").find("input").val();

        //            if (CommonHelper.IsDecimal($.trim(amount)) == false) {
        //                toastr.info("Please Give Valid Amount.");
        //                return false;
        //            }

        //            employee.push({
        //                EmpId: empid,
        //                BasicAmount: amount
        //            });
        //        }
        //    }
        //    else if (transactionType == "Grade") {
        //        gridLength = $("#GradeWiseSalaryBasicContainer tbody tr").length;

        //        for (row = 0; row < gridLength; row++) {
        //            tr = $("#GradeWiseSalaryBasicContainer tbody tr:eq(" + row + ")");
        //            gradeId = $.trim($(tr).find("td:eq(0)").text());
        //            amount = $(tr).find("td:eq(2)").find("input").val();

        //            if (CommonHelper.IsDecimal($.trim(amount)) == false) {
        //                toastr.info("Please Give Valid Amount.");
        //                return false;
        //            }

        //            grade.push({
        //                GradeId: gradeId,
        //                BasicAmount: amount
        //            });
        //        }
        //    }

        //    PageMethods.UpdateEmployeeBasicSalary(grade, employee, transactionType, OnUpdateEmployeeBasicSalarySucceeded, OnUpdateEmployeeBasicSalaryFailed);
        //    return false;
        //}

        //function OnUpdateEmployeeBasicSalarySucceeded(result) {
        //    if (result.IsSuccess) {
        //        CommonHelper.AlertMessage(result.AlertMessage);
        //        ResetForm();
        //    }
        //    else {
        //        CommonHelper.AlertMessage(result.AlertMessage);
        //    }
        //}
        //function OnUpdateEmployeeBasicSalaryFailed() {

        //}

        function WorkAfterSearchEmployee() {
            PageMethods.EmployeeWiseSalaryFormulaHead(OnLodEmployeeWiseSalaryFormulaSucceeded, OnLodEmployeeWiseSalaryFormulaFailed);
        }
        function OnLodEmployeeWiseSalaryFormulaSucceeded(result) {
            $("#SalaryHeadEmployeeWiseContaainer").html(result);
            GetEmployeeWiseSalaryFormulaHead();
        }
        function OnLodEmployeeWiseSalaryFormulaFailed(error) {
            toastr.warning("Error On Load. Please Try Again.");
        }

        function GetEmployeeWiseSalaryFormulaHead() {
            var employeeId = $("#ContentPlaceHolder1_searchEmployee_hfEmployeeId").val();
            PageMethods.GetEmployeeWiseSalaryFormulaHead(employeeId, OnLodEmployeeSalaryFormulaSucceeded, OnLodEmployeeSalaryFormulaFailed);
        }
        function OnLodEmployeeSalaryFormulaSucceeded(result) {
            var individualFormula = result.EmployeeIndividualWise;
            var gradeWiseFormula = result.EmployeeGradeWise;

            var gridLength = $("#EmployeeWiseSalaryFormulatbl tbody tr").length;
            var tr = "", row = 0, id = "", md = {};

            if (gradeWiseFormula.length > 0) {

                md = {};

                for (row = 0; row < gridLength; row++) {
                    tr = $("#EmployeeWiseSalaryFormulatbl tbody tr:eq(" + row + ")");
                    id = $(tr).find("td:eq(1)").text();

                    md = _.findWhere(gradeWiseFormula, { SalaryHeadId: parseInt(id, 10) });

                    if (md != null) {
                        $("#amnttyp" + id).val(md.AmountType);
                        if (md.DependsOn != null)
                            $("#depnd" + id).val(md.DependsOn);
                        else
                            md.DependsOn = 0;

                        $("#amnt" + id).val(md.Amount);
                        $("#amtdb" + id).text(md.AmountType);
                        $("#depndb" + id).text(md.DependsOn);
                        $("#amntdb" + id).text(md.Amount);
                    }
                }
            }

            if (individualFormula.length > 0) {
                for (row = 0; row < gridLength; row++) {
                    tr = $("#EmployeeWiseSalaryFormulatbl tbody tr:eq(" + row + ")");
                    id = $(tr).find("td:eq(1)").text();

                    md = _.findWhere(individualFormula, { SalaryHeadId: parseInt(id, 10) });

                    if (md != null) {
                        $("#amnttyp" + id).val(md.AmountType);
                        $("#chk" + id).prop("checked", true);

                        if (md.DependsOn != null)
                            $("#depnd" + id).val(md.DependsOn);
                        else
                            md.DependsOn = 0;

                        $("#amnt" + id).val(md.Amount);
                        $("#frmula" + id).text(md.FormulaId);
                        $("#amtdb" + id).text(md.AmountType);
                        $("#depndb" + id).text(md.DependsOn);
                        $("#amntdb" + id).text(md.Amount);
                    }
                }
            }
        }

        function OnLodEmployeeSalaryFormulaFailed() { }

        function CheckInputValidation(amount) {
            if ($.trim($(amount).val()) == "") {
                return;
            }
            else if ($.trim($(amount).val()) == "0") {
                toastr.info("Please Give Valid Amount (0 is not acccepted).");
                $(amount).val("");
                return;
            }
            else if (CommonHelper.IsDecimal($(amount).val()) == false) {
                toastr.info("Please Give Valid Amount.");
                $(amount).val("");
            }
        }

        function ResetForm() {
            $("#ContentPlaceHolder1_searchEmployee_hfEmployeeId").val("0");
            $("#EmployeeWiseSalaryFormulatbl tbody").html("");
            $("#ContentPlaceHolder1_ddlTransactionType").val("Individual");
        }
    </script>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="selectedDependsOn" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtIsIndividual" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtEmployeeId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtFormulaId" runat="server"></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <%--<li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-3">Salary Basic/Gross Entry</a></li>--%>
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Contribution Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Contribution </a></li>
        </ul>
        <%--<div id="tab-3">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Transaction Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlBasicGrossTransactionType" runat="server" CssClass="form-control"
                            TabIndex="4">
                            <asp:ListItem Value="">--- Please Select ---</asp:ListItem>
                            <asp:ListItem Value="Grade">Grade</asp:ListItem>
                            <asp:ListItem Value="Individual">Individual</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="GroupWiseSalaryBasic">
                    <div class="form-group" style="margin: 10px 5px 0 5px;">
                        <div id="GradeWiseSalaryBasicContainer">
                        </div>
                    </div>
                </div>
                <div id="EmplyeeWiseSalaryBasic">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Department"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlDepartmentId" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group" style="margin: 10px 5px 0 5px;">
                        <div id="EmployeeSalaryBasicContainer">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="Button1" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                            TabIndex="10" OnClientClick="javascript:return SaveEmployeeNGradeWiseBasicSalary();" />
                    </div>
                </div>
            </div>
        </div>--%>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">Contribution Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblTransactionType" runat="server" class="control-label" Text="Setup Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlTransactionType" runat="server" CssClass="form-control"
                                    TabIndex="4">
                                    <asp:ListItem Value="Grade">Grade</asp:ListItem>
                                    <asp:ListItem Value="Individual">Individual</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="GroupWiseSalaryFormula">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblGrade" runat="server" class="control-label required-field" Text="Grade/Scale"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlGradeId" runat="server" CssClass="form-control"
                                        TabIndex="1">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="form-control"
                                        TabIndex="4">
                                        <asp:ListItem Value="0">Active</asp:ListItem>
                                        <asp:ListItem Value="1">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblSalaryHeadId" runat="server" class="control-label required-field" Text="Salary Head"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlSalaryHeadId" runat="server" CssClass="form-control"
                                        TabIndex="6">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblDependsOn" runat="server" class="control-label" Text="Depends On"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlDependsOn" runat="server" CssClass="form-control"
                                        TabIndex="7">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblAmount" runat="server" class="control-label required-field" Text="Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control quantitydecimal" TabIndex="8"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblAmountType" runat="server" class="control-label required-field" Text="Amount Type"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlAmountType" runat="server" CssClass="form-control"
                                        TabIndex="9">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div id="EmplyeeWiseSalaryFormula">
                            <UserControl:EmployeeSearch runat="server" ID="searchEmployee" />
                            <div class="form-group" style="margin-top: 10px;">
                                <div id="SalaryHeadEmployeeWiseContaainer">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                                    TabIndex="10" OnClick="btnSave_Click" OnClientClick="javascript:return ValidateFormula();" />
                                <input id="btnClear" type="button" class="TransactionalButton btn btn-primary" value="Clear" tabindex="11" onclick="PerformClearActionWithConfirmation()" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">Contribution Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSearchGrade" runat="server" class="control-label" Text="Grade/Scale"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSearchGrade" runat="server" CssClass="form-control"
                                    TabIndex="1">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSearchActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSearchActiveStat" runat="server" CssClass="form-control"
                                    TabIndex="3">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSearchSelaryHead" runat="server" class="control-label" Text="Salary Head"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlSearchSelaryHead" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnSearch_Click" TabIndex="6" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">Search Information</div>
                <div class="panel-body">
                    <asp:GridView ID="gvSalaryFormula" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True"
                        ForeColor="#333333" OnPageIndexChanging="gvSalaryFormula_PageIndexChanging" OnRowDataBound="gvSalaryFormula_RowDataBound"
                        OnRowCommand="gvSalaryFormula_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("FormulaId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SalaryHead" HeaderText="Salary Head" ItemStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SalaryType" HeaderText="Salary Type" ItemStyle-Width="11%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Grade" HeaderText="Grade/Employee" ItemStyle-Width="17%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AmountType" HeaderText="Amount Type" ItemStyle-Width="17%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%#Bind("FormulaId") %>' ImageUrl="~/Images/edit.png" AlternateText="Edit"
                                        ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%#Bind("FormulaId") %>' ImageUrl="~/Images/delete.png" AlternateText="Delete"
                                        ToolTip="Delete" OnClientClick="return confirm('Do you want to Delete?');" />
                                </ItemTemplate>
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
            </div>
        </div>
    </div>
    <script type="text/javascript">
</script>
</asp:Content>
