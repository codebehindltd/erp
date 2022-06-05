<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="CashRequisitionApprovalIframe.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.CashRequisitionApprovalIframe" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;
        var isClose;
        var GlobalProjectId = 0;var GlobalProjectAdjustmentId = 0;
        var GlobalApproveStatus = "";
        var ItemEdited = ""; var indexEdited = -1;

        var RequisitionForBillVoucher = new Array();
        var RequisitionForBillVoucherDeleted = new Array();

        $(document).ready(function () {
            //debugger;

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            //debugger;

            if ($("#ContentPlaceHolder1_hfIsCashRequisitionAdjustmentWithDifferentCompanyOrProjects").val() == '0') {

                $("#showCompanyAndProjectDivForEdit").hide();
                document.getElementById("companyCol").style.display = "none";
                document.getElementById("projectCol").style.display = "none";
            }
            else {
                $("#showCompanyAndProjectDivForEdit").show();
            }

            var CashRequisitionAdjustmentId = $.trim(CommonHelper.GetParameterByName("craid"));
            var CashRequisitionAdjustmentIdForEdit = $.trim(CommonHelper.GetParameterByName("craeid"));

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;

            CommonHelper.ApplyDecimalValidation();

            $("#ContentPlaceHolder1_ddlAccountExpenseHead").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            
            $("#ContentPlaceHolder1_ddlSupplierId").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlTransactionFromAccountHeadId").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlAssignEmployee").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlCompany").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlProject").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlAccountExpenseHead").change(function () {
                debugger;
                $("#SupplierInfoDiv").hide();
                var supplierAccountsHeadId = parseInt($("#ContentPlaceHolder1_hfSupplierAccountsHeadId").val(), 10);
                var expenseHeadId = parseInt($("#ContentPlaceHolder1_ddlAccountExpenseHead option:selected").val().trim(), 10);
                if (supplierAccountsHeadId == expenseHeadId)
                {
                    $("#SupplierInfoDiv").show();
                }
            });

            $("#ContentPlaceHolder1_ddlGLCompany").change(function () {
                //debugger;

                var CompanyId = parseInt($("#ContentPlaceHolder1_ddlGLCompany option:selected").val().trim(), 10);
                PageMethods.LoadProjectByCompanyId(CompanyId, OnLoadProjectByCompanyIdSucceed, OnLoadProjectByCompanyIdFailed);
                return false;
            });

            $("#ContentPlaceHolder1_ddlCompany").change(function () {
                // debugger;
                var CompanyId = parseInt($("#ContentPlaceHolder1_ddlCompany option:selected").val().trim());
                PageMethods.LoadProjectByCompanyId(CompanyId, OnLoadProjectForChangeByCompanyIdSucceed, OnLoadProjectByCompanyIdFailed);
                return false;
            });


            if (CashRequisitionAdjustmentId != "") {
                PerformEdit(CashRequisitionAdjustmentId);
            }

            if (CashRequisitionAdjustmentIdForEdit != "") {

                PerformEditAdjustment(CashRequisitionAdjustmentIdForEdit);
            }

        });

        function OnLoadProjectByCompanyIdSucceed(result) {

            // debugger;
            typesList = [];
            $("#ContentPlaceHolder1_ddlGLProject").empty();
            var i = 0, fieldLength = result.length;

            if (fieldLength > 0) {
                typesList = result;
                $('<option value="' + 0 + '">' + '--- Please Select ---' + '</option>').appendTo('#ContentPlaceHolder1_ddlGLProject');
                for (i = 0; i < fieldLength; i++) {
                    $('<option value="' + result[i].ProjectId + '">' + result[i].Name + '</option>').appendTo('#ContentPlaceHolder1_ddlGLProject');
                }

                if (GlobalProjectId > 0) {

                    $("#ContentPlaceHolder1_ddlGLProject").val(GlobalProjectId).trigger('change');
                    GlobalProjectId = 0;
                }
                //$("#ContentPlaceHolder1_ddlGLProject").attr("Enabled", true);
            }
            else {
                $("<option value='0'>--No Projects Found--</option>").appendTo("#ContentPlaceHolder1_ddlGLProject");
                var company = $("#ContentPlaceHolder1_ddlGLCompany").val();
                //$("#ContentPlaceHolder1_ddlGLProject").attr("Enabled", false);


            }

            return false;

        }

        function OnLoadProjectForChangeByCompanyIdSucceed(result) {

            // debugger;
            typesList = [];
            $("#ContentPlaceHolder1_ddlProject").empty();
            var i = 0, fieldLength = result.length;

            if (fieldLength > 0) {
                typesList = result;
                $('<option value="' + 0 + '">' + '--- Please Select ---' + '</option>').appendTo('#ContentPlaceHolder1_ddlProject');
                for (i = 0; i < fieldLength; i++) {
                    $('<option value="' + result[i].ProjectId + '">' + result[i].Name + '</option>').appendTo('#ContentPlaceHolder1_ddlProject');
                }

                if (GlobalProjectAdjustmentId > 0) {

                    $("#ContentPlaceHolder1_ddlProject").val(GlobalProjectAdjustmentId).trigger('change');
                    GlobalProjectAdjustmentId = 0;
                }


                //$("#ContentPlaceHolder1_ddlGLProject").attr("Enabled", true);
            }
            else {
                $("<option value='0'>--No Projects Found--</option>").appendTo("#ContentPlaceHolder1_ddlProject");

            }

            return false;

        }

        function ClearBillContainer() {
            $("#ContentPlaceHolder1_ddlCompany").val("0").change();
            $("#ContentPlaceHolder1_ddlProject").val("0").change();
            $("#ContentPlaceHolder1_ddlAccountExpenseHead").val("0").change();
            $("#ContentPlaceHolder1_txtAmount").val("");
            $("#ContentPlaceHolder1_txtRemarks").val("");
            $("#btnAdd").val("Add");

            ItemEdited = ""; indexEdited = -1;
        }

        function OnLoadProjectByCompanyIdFailed(xhr, err) {
            toastr.error(xhr.responseText);
        }
        function PerformEdit(id) {
            // debugger;
            PageMethods.GetRequsitionById(id, OnSuccessLoading, OnFailLoading)
            return false;
        }

        function PerformEditAdjustment(id) {
            // debugger;
            PageMethods.GetAdjustmentDetailsById(id, OnSuccessAdjustmentLoading, OnFailAdjustmentLoading)
            return false;
        }

        function OnSuccessAdjustmentLoading(Result) {
            // debugger;
            $("#ContentPlaceHolder1_hfCashRequisitionAdjustmentId").val(Result[0].Id);

            var tr = "";
            for (var i = 0; i < Result.length; i++) {
                tr += "<tr>";



                if ($("#ContentPlaceHolder1_hfIsCashRequisitionAdjustmentWithDifferentCompanyOrProjects").val() == '1') {
                    tr += "<td style='width:20%;'>" + Result[i].RequisitionForHeadName + "</td>";
                    tr += "<td style='width:20%;'>" + Result[i].IndividualCompanyName + "</td>";
                    tr += "<td style='width:20%;'>" + Result[i].IndividualProjectName + "</td>";
                }
                else {
                    tr += "<td style='width:20%;'>" + Result[i].RequisitionForHeadName + "</td>";

                    document.getElementById("companyCol").style.display = "none";
                    document.getElementById("projectCol").style.display = "none";

                    tr += "<td style='display:none; width:20%;'>" + Result[i].IndividualCompanyName + "</td>";
                    tr += "<td style='display:none; width:20%;'>" + Result[i].IndividualProjectName + "</td>";
                }

                tr += "<td style='width:15%;'>" +
                    "<input type='text' value='" + Result[i].RequsitionAmount + "' id='pp" + Result[i].RequisitionForHeadId + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoq(this)'  />" +
                    "</td>";
                tr += "<td style='width:15%;'>" +
                    //"<input type='text' value='" + Result[i].IndividualRemarks + "' id='ppr" + Result[i].RequisitionForHeadId + "' class='form-control' onblur='CalculateTotalForAdhoq(this)' />" +
                    "<textarea name='ppr" + Result[i].RequisitionForHeadId + "' id='ppr" + Result[i].RequisitionForHeadId + "' onblur='CalculateTotalForAdhoq(this)' style='width:100%;'>" + Result[i].IndividualRemarks + "</textarea>" +
                    "</td>";

                tr += "<td style='width:5%;'>";
                tr += "<a href='javascript:void()' onclick= 'EditAdhoqItem(this)' ><img alt='Delete' src='../Images/edit.png' /></a>";
                tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                tr += "</td>";
                tr += "<td style='display:none;'>" + Result[i].RequisitionForHeadId + "</td>";
                tr += "<td style='display:none;'>" + Result[i].DetailId + "</td>";
                tr += "<td style='display:none;'>" + Result[i].IndividualCompanyId + "</td>";
                tr += "<td style='display:none;'>" + Result[i].IndividualProjectId + "</td>";
                tr += "<td style='display:none;'>" + Result[i].SupplierId + "</td>";

                tr += "</tr>";

                RequisitionForBillVoucher.push({
                    CostCenterId: parseInt(Result[i].RequisitionForHeadId, 10),
                    RequsitionAmount: Result[i].RequsitionAmount,
                    Remarks: Result[i].IndividualRemarks,
                    CompanyId: Result[i].IndividualCompanyId,
                    ProjectId: Result[i].IndividualProjectId,
                    SupplierId: Result[i].SupplierId,
                    DetailId: Result[i].DetailId,
                    Id: Result[i].Id,
                    CostCenterName: Result[i].RequisitionForHeadName
                });
            }

            $("#RequisitionForBillVoucherTbl tbody").prepend(tr);
            CommonHelper.ApplyDecimalValidation();
            UploadComplete();

            return false;
        }
        function OnFailAdjustmentLoading(error) {
            toastr.error(error.get_message());
            return false;
        }

        function OnSuccessLoading(result) {
            FillForm(result);
            return false;
        }
        function FillForm(Result) {
            //debugger;
            Clear();
            //debugger;
            $("#ContentPlaceHolder1_hfCashRequisitionRefId").val(Result.Id);
            GlobalApproveStatus = Result.ApprovedStatus;
            $("#ContentPlaceHolder1_ddlGLCompany").val(Result.CompanyId).trigger('change');
            // debugger;
            //$("#ContentPlaceHolder1_ddlGLProject").val(Result.ProjectId);
            $("#ContentPlaceHolder1_ddlAssignEmployee").val(Result.EmployeeId).change();
            $("#ContentPlaceHolder1_ddlTransactionFromAccountHeadId").val(Result.TransactionFromAccountHeadId).change();
            // debugger;
            $("#ContentPlaceHolder1_txtFinalRemarks").val(Result.Remarks);
            GlobalProjectId = Result.ProjectId;
            $("#ContentPlaceHolder1_txtRemainingBalance").val(Result.RemainingAmount);

            var tr = "";


            $("#RequisitionForBillVoucherTbl tbody").prepend(tr);






            if (IsCanEdit)
                $("#btnSave").val('Adjust').show();
            else
                $("#btnSave").hide();
            //UploadComplete();
            return false;
        }

        function OnFailLoading(error) {
            toastr.error(error.get_message());
            return false;
        }

        function AddItem() {
            var company = 0, project = 0;
            var companyName = "", projectName = "";

            if ($("#ContentPlaceHolder1_hfIsCashRequisitionAdjustmentWithDifferentCompanyOrProjects").val() == '1') {

                company = $("#ContentPlaceHolder1_ddlCompany option:selected").val();
                companyName = $("#ContentPlaceHolder1_ddlCompany option:selected").text();
                if (company == "0") {
                    toastr.warning("Please Select a Company.");
                    $("#ContentPlaceHolder1_ddlCompany").focus();
                    return false;
                }

                project = $("#ContentPlaceHolder1_ddlProject option:selected").val();
                projectName = $("#ContentPlaceHolder1_ddlProject option:selected").text();
                if (project == "0") {
                    toastr.warning("Please Select a Project.");
                    $("#ContentPlaceHolder1_ddlProject").focus();
                    return false;
                }


            }

            var costCenter = $("#ContentPlaceHolder1_ddlAccountExpenseHead option:selected").val();
            var costCenterName = $("#ContentPlaceHolder1_ddlAccountExpenseHead option:selected").text();
            if (costCenter == "0") {
                toastr.warning("Select For Adjustment For.");
                $("#ContentPlaceHolder1_ddlAccountExpenseHead").focus();
                return false;
            }

            $("#SupplierInfoDiv").hide();
            var supplierId = 0;
            var supplierAccountsHeadId = parseInt($("#ContentPlaceHolder1_hfSupplierAccountsHeadId").val(), 10);
            var expenseHeadId = parseInt($("#ContentPlaceHolder1_ddlAccountExpenseHead option:selected").val().trim(), 10);
            if (supplierAccountsHeadId == expenseHeadId) {
                $("#SupplierInfoDiv").show();
                supplierId = $("#ContentPlaceHolder1_ddlSupplierId option:selected").val();
            }

            var CostCenterOBJ = _.findWhere(RequisitionForBillVoucher, { CostCenterId: parseInt(costCenter, 10), CompanyId: parseInt(company, 10), ProjectId: parseInt(project, 10), SupplierId: parseInt(supplierId, 10) });
            if (CostCenterOBJ != null) {
                toastr.warning("It's already added.");
                $("#ContentPlaceHolder1_ddlAccountExpenseHead").focus();
                return false;
            }

            var transactionFromAccountHeadId = $("#ContentPlaceHolder1_ddlTransactionFromAccountHeadId").val();
            if (transactionFromAccountHeadId == "0") {
                isClose = false;
                toastr.warning("Select a Transaction From.");
                $("#ContentPlaceHolder1_ddlTransactionFromAccountHeadId").focus();
                return false;
            }

            //if (costCenter == transactionFromAccountHeadId) {
            //    isClose = false;
            //    toastr.warning("Transaction From And Adjustment For can not be same.");
            //    $("#ContentPlaceHolder1_ddlAccountExpenseHead").focus();
            //    return false;
            //}

            var requsitionAmount = $("#ContentPlaceHolder1_txtAmount").val();
            if (requsitionAmount == "") {
                toastr.warning("Add Adjustment Amount.");
                $("#ContentPlaceHolder1_txtAmount").focus();
                return false;
            }

            
            

            var reaminningBalance = parseFloat($("#ContentPlaceHolder1_txtRemainingBalance").val());
            //if (reaminningBalance < requsitionAmount) {
            //    toastr.warning("Can Not Add Greater Than Remaining Balance.");
            //    $("#ContentPlaceHolder1_txtAmount").focus();
            //    return false;
            //}
            var totalAmount = 0.0;
            var RequisitionForBillVoucherNewlyAdded = new Array();
            RequisitionForBillVoucherNewlyAdded = RequisitionForBillVoucher;
            var row = 0, rowCount = RequisitionForBillVoucherNewlyAdded.length;
            for (row = 0; row < rowCount; row++) {

                totalAmount += parseFloat(RequisitionForBillVoucherNewlyAdded[row].RequsitionAmount);
            }
            totalAmount += parseFloat(requsitionAmount);
            //if (reaminningBalance < totalAmount) {
            //    toastr.warning("Can Not Add Greater Than Remaining Balance.");
            //    $("#ContentPlaceHolder1_txtAmount").focus();
            //    return false;
            //}


            var remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            //if (remarks == "") {
            //    toastr.warning("Add remarks.");
            //    $("#ContentPlaceHolder1_txtRemarks").focus();
            //    return false;
            //}

            if (ItemEdited == "" && $("#btnAdd").val() != "Update") {
                var tr = "";
                tr += "<tr>";



                if ($("#ContentPlaceHolder1_hfIsCashRequisitionAdjustmentWithDifferentCompanyOrProjects").val() == '1') {
                    tr += "<td style='width:20%;'>" + costCenterName + "</td>";
                    tr += "<td style='width:20%;'>" + companyName + "</td>";
                    tr += "<td style='width:20%;'>" + projectName + "</td>";
                }
                else {
                    tr += "<td style='width:20%;'>" + costCenterName + "</td>";

                    document.getElementById("companyCol").style.display = "none";
                    document.getElementById("projectCol").style.display = "none";

                    tr += "<td style='display:none; width:20%;'>" + companyName + "</td>";
                    tr += "<td style='display:none; width:20%;'>" + projectName + "</td>";
                }

                tr += "<td style='width:15%;'>" +
                    "<input type='text' value='" + requsitionAmount + "' id='pp" + costCenter + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoq(this)'  />" +
                    "</td>";
                tr += "<td style='width:15%;'>" +
                    //"<input type='text' value='" + remarks + "' id='ppr" + costCenter + "' class='form-control' onblur='CalculateTotalForAdhoq(this)' />" +
                    "<textarea name='ppr" + costCenter + "' id='ppr" + costCenter + "' onblur='CalculateTotalForAdhoq(this)' style='width:100%;'>" + remarks + "</textarea>" +
                    "</td>";

                tr += "<td style='width:5%;'>";
                tr += "<a href='javascript:void()' onclick= 'EditAdhoqItem(this)' ><img alt='Delete' src='../Images/edit.png' /></a>";
                tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                tr += "</td>";
                tr += "<td style='display:none;'>" + costCenter + "</td>";
                tr += "<td style='display:none;'>" + 0 + "</td>";
                tr += "<td style='display:none;'>" + parseInt(company, 10) + "</td>";
                tr += "<td style='display:none;'>" + parseInt(project, 10) + "</td>";

                tr += "<td style='display:none;'>" + supplierId + "</td>";

                tr += "</tr>";

                $("#RequisitionForBillVoucherTbl tbody").prepend(tr);
                tr = "";
                CommonHelper.ApplyDecimalValidation();
                RequisitionForBillVoucher.push({
                    CostCenterId: parseInt(costCenter, 10),
                    RequsitionAmount: requsitionAmount,
                    Remarks: remarks,
                    CompanyId: parseInt(company, 10),
                    ProjectId: parseInt(project, 10),
                    SupplierId: parseInt(supplierId, 10),
                    DetailId: 0,
                    Id: 0,
                    CostCenterName: costCenterName
                });

            }
            else {
                $(ItemEdited).find("td:eq(3)").find("input").val(requsitionAmount);
                $(ItemEdited).find("td:eq(4)").find("textarea").val(remarks);
                $(ItemEdited).find("td:eq(0)").text(costCenterName);
                $(ItemEdited).find("td:eq(6)").text(costCenter);
                $(ItemEdited).find("td:eq(8)").text(parseInt(company, 10));
                $(ItemEdited).find("td:eq(9)").text(parseInt(project, 10));
                $(ItemEdited).find("td:eq(10)").text(parseInt(supplierId, 10));

                CommonHelper.ApplyDecimalValidation();
                RequisitionForBillVoucher[indexEdited].RequsitionAmount = parseFloat(requsitionAmount);
                RequisitionForBillVoucher[indexEdited].Remarks = (remarks);
                RequisitionForBillVoucher[indexEdited].CostCenterId = parseInt(costCenter, 10);
                RequisitionForBillVoucher[indexEdited].CostCenterName = costCenterName;
                RequisitionForBillVoucher[indexEdited].CompanyId = parseInt(company, 10);
                RequisitionForBillVoucher[indexEdited].ProjectId = parseInt(project, 10);
                RequisitionForBillVoucher[indexEdited].SupplierId = parseInt(supplierId, 10);

            }
            ClearBillContainer();
            $("#ContentPlaceHolder1_ddlAccountExpenseHead").focus();
        }

        function CalculateTotalForAdhoq(control) {

            //debugger;
            var tr = $(control).parent().parent();

            var reAmount = $.trim($(tr).find("td:eq(3)").find("input").val());
            //var reMarks = $.trim($(tr).find("td:eq(4)").find("input").val());
            var reMarks = $.trim($(tr).find("td:eq(4)").find("textarea").val());
            debugger;

            if (reAmount == "" || reAmount == "0") {
                toastr.info("Amount Cannot Be Zero Or Empty.");
                return false;
            }

            //if (reMarks == "") {
            //    toastr.info("Remarks Cannot Be Empty.");
            //    return false;
            //}

            var costCenterId = parseInt($.trim($(tr).find("td:eq(6)").text()), 10);

            var company = parseInt($.trim($(tr).find("td:eq(8)").text()), 10);
            var project = parseInt($.trim($(tr).find("td:eq(9)").text()), 10);

            var CostCenter = _.findWhere(RequisitionForBillVoucher, { CostCenterId: costCenterId, CompanyId: parseInt(company, 10), ProjectId: parseInt(project, 10) });
            var index = _.indexOf(RequisitionForBillVoucher, CostCenter);

            RequisitionForBillVoucher[index].RequsitionAmount = parseFloat(reAmount);
            RequisitionForBillVoucher[index].Remarks = (reMarks);

            var totalAmount = 0.0;
            var RequisitionForBillVoucherNewlyAdded = new Array();
            RequisitionForBillVoucherNewlyAdded = RequisitionForBillVoucher;
            var row = 0, rowCount = RequisitionForBillVoucherNewlyAdded.length;
            for (row = 0; row < rowCount; row++) {

                totalAmount += parseFloat(RequisitionForBillVoucherNewlyAdded[row].RequsitionAmount);
            }

            var reaminningBalance = parseFloat($("#ContentPlaceHolder1_txtRemainingBalance").val());
            //if (reaminningBalance < totalAmount) {
            //    $(tr).find("td:eq(3)").find("input").val(0);
            //    RequisitionForBillVoucher[index].RequsitionAmount = 0;
            //    toastr.warning("Can Not Add Greater Than Remaining Balance.");
            //    return false;
            //}
        }

        function DeleteAdhoqItem(control) {

            if (!confirm("Do you want to delete?")) { return false; }

            //debugger;

            var tr = $(control).parent().parent();

            var costCenter = parseInt($.trim($(tr).find("td:eq(6)").text()), 10);
            var detailsId = parseInt($.trim($(tr).find("td:eq(7)").text()), 10);

            var CostCenter = _.findWhere(RequisitionForBillVoucher, { CostCenterId: costCenter });
            var index = _.indexOf(RequisitionForBillVoucher, CostCenter);

            if (parseInt(detailsId, 10) > 0)
                RequisitionForBillVoucherDeleted.push(JSON.parse(JSON.stringify(CostCenter)));

            RequisitionForBillVoucher.splice(index, 1);
            $(tr).remove();

        }
        function EditAdhoqItem(control) {

            if (!confirm("Do you want to Edit?")) { return false; }

            //debugger;

            var tr = $(control).parent().parent();
            ItemEdited = $(control).parent().parent();

            var costCenter = parseInt($.trim($(tr).find("td:eq(6)").text()), 10);
            var detailsId = parseInt($.trim($(tr).find("td:eq(7)").text()), 10);

            var CostCenter = _.findWhere(RequisitionForBillVoucher, { CostCenterId: costCenter });
            indexEdited = _.indexOf(RequisitionForBillVoucher, CostCenter);

            $("#ContentPlaceHolder1_txtAmount").val(CostCenter.RequsitionAmount);
            $("#ContentPlaceHolder1_txtRemarks").val(CostCenter.Remarks);
            debugger;
            $("#ContentPlaceHolder1_ddlCompany").val(CostCenter.CompanyId).trigger('change');
            //$("#ContentPlaceHolder1_ddlProject").val(CostCenter.ProjectId).trigger('change');
            GlobalProjectAdjustmentId = CostCenter.ProjectId;
            $("#ContentPlaceHolder1_ddlAccountExpenseHead").val(CostCenter.CostCenterId).trigger('change');
            $("#ContentPlaceHolder1_ddlSupplierId").val(CostCenter.SupplierId).trigger('change');

            $("#btnAdd").val("Update");


        }

        function Clear() {

            $("#ContentPlaceHolder1_ddlGLCompany").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlGLProject").val("0").trigger('change');
            //$("#ContentPlaceHolder1_ddlAssignEmployee").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtFinalRemarks").val('');
            ClearBillContainer();

            if (IsCanSave) {
                $("#btnSave").val('Save').show();
            }
            else {
                $("#btnSave").hide();
            }
            isClose = false;
            $("#btnClear").show();
            return false;
        }

        function SaveNClose() {
            //debugger;
            isClose = true;
            //SaveOrUpdateTask();
            $.when(SaveOrUpdateBillVoucher()).done(function () {
                if (isClose) {

                    if (typeof parent.CloseDialog === "function") {
                        parent.CloseDialog();
                    }

                }
            });
            return false;
        }

        function SaveOrUpdateBillVoucher() {
            //debugger;
            var RefId = +$("#ContentPlaceHolder1_hfCashRequisitionRefId").val();
            var companyId = $("#ContentPlaceHolder1_ddlGLCompany").val();
            if (companyId == "0") {
                isClose = false;
                toastr.warning("Select a company.");
                $("#ContentPlaceHolder1_ddlGLCompany").focus();
                return false;
            }
            var projectId = $("#ContentPlaceHolder1_ddlGLProject").val();
            if (projectId == "0") {
                isClose = false;
                toastr.warning("Select a project.");
                $("#ContentPlaceHolder1_ddlGLProject").focus();
                return false;
            }

            var employeeId = $("#ContentPlaceHolder1_ddlAssignEmployee").val();
            if (employeeId == "0") {
                isClose = false;
                toastr.warning("Select a employee.");
                $("#ContentPlaceHolder1_ddlAssignEmployee").focus();
                return false;
            }

            var remarks = $("#ContentPlaceHolder1_txtFinalRemarks").val();
            if (remarks == "") {
                isClose = false;
                toastr.warning("Enter Remarks");
                $("#ContentPlaceHolder1_txtFinalRemarks").focus();
                return false;
            }

            var transactionFromAccountHeadId = $("#ContentPlaceHolder1_ddlTransactionFromAccountHeadId").val();
            if (transactionFromAccountHeadId == "0") {
                isClose = false;
                toastr.warning("Select a Transaction From.");
                $("#ContentPlaceHolder1_ddlTransactionFromAccountHeadId").focus();
                return false;
            }

            var totalAmount = 0.0;
            var RequisitionForBillVoucherNewlyAdded = new Array();
            RequisitionForBillVoucherNewlyAdded = RequisitionForBillVoucher;
            var row = 0, rowCount = RequisitionForBillVoucherNewlyAdded.length;
            for (row = 0; row < rowCount; row++) {
                //if (parseInt(transactionFromAccountHeadId, 10) == RequisitionForBillVoucherNewlyAdded[row].CostCenterId) {
                //    isClose = false;
                //    toastr.warning("Transaction From And Adjustment For can not be same.");
                //    $("#ContentPlaceHolder1_ddlTransactionFromAccountHeadId").focus();
                //    return false;
                //}
                if (RequisitionForBillVoucherNewlyAdded[row].RequsitionAmount == "" || RequisitionForBillVoucherNewlyAdded[row].RequsitionAmount == "0") {
                    toastr.warning("Requsition Amount Cannot NUll OR Empty In " + RequisitionForBillVoucherNewlyAdded[row].CostCenterName);
                    return false;
                }
                totalAmount += parseFloat(RequisitionForBillVoucherNewlyAdded[row].RequsitionAmount);
            }
            if (row == 0) {
                isClose = false;
                toastr.warning("Add some data for adjustment");
                return false;
            }

            //var reaminningBalance = parseFloat($("#ContentPlaceHolder1_txtRemainingBalance").val());
            //if (reaminningBalance < totalAmount) {
            //    isClose = false;
            //    toastr.warning("Can Not Adjust Greater Than Remaining Balance.");
            //    return false;
            //}

            if (row != rowCount) {
                return false;
            }
            var id = +$("#ContentPlaceHolder1_hfCashRequisitionAdjustmentId").val();
            var Requisition = {
                Id: id,
                CompanyId: companyId,
                ProjectId: projectId,
                EmployeeId: employeeId,
                Amount: totalAmount,
                Remarks: remarks,
                TransactionType: "Cash Requisition Adjustment",
                //ApprovedStatus:( GlobalApproveStatus == "" ? "Pending" : GlobalApproveStatus),
                ApprovedStatus: "Pending",
                RefId: RefId
            }
            GlobalApproveStatus = "";
            var hfRandom = $("#<%=RandomProductId.ClientID %>").val();
            var deletedDocuments = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();
            PageMethods.SaveOrUpdateBillVoucher(Requisition, RequisitionForBillVoucherNewlyAdded, RequisitionForBillVoucherDeleted, hfRandom, deletedDocuments, OnSuccessSaveOrUpdate, OnFailSaveOrUpdate);
            return false;



        }

        function OnSuccessSaveOrUpdate(result) {
            //debugger;
            if (result.IsSuccess) {
                if (result.IsSuccess) {
                    debugger;
                    parent.ShowAlert(result.AlertMessage, result.PrimaryKeyValue, result.TransactionNo, result.TransactionType, result.TransactionStatus);
                    parent.SearchInformation(1, 1);
                }
                //if (typeof parent.GridPaging === "function")
                //    parent.GridPaging(1, 1);
                Clear();
            }
        }

        

        function OnFailSaveOrUpdate(error) {
            isClose = false;
            toastr.error(error.get_message());
            return false;
        }


        // Documents Div
        function UploadComplete() {
            var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var id = +$("#ContentPlaceHolder1_hfCashRequisitionAdjustmentId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();

            PageMethods.LoadDocument(id, randomId, deletedDoc, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }
        function OnLoadDocumentSucceeded(result) {

            var guestDoc = result;
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";

            guestDocumentTable += "<table id='DocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            guestDocumentTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }

                guestDocumentTable += "<td align='left' style='width: 50%'>" + guestDoc[row].Name + "</td>";

                if (guestDoc[row].Path != "") {
                    if (guestDoc[row].Extention == ".jpg" || guestDoc[row].Extention == ".png" || guestDoc[row].Extention == ".PNG" || guestDoc[row].Extention == ".JPG")
                        imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    else
                        imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                guestDocumentTable += "<td align='left' style='width: 30%'>" + imagePath + "</td>";

                guestDocumentTable += "<td align='left' style='width: 20%'>";
                guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteGuestDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                guestDocumentTable += "</td>";
                guestDocumentTable += "</tr>";
            }
            guestDocumentTable += "</table>";

            // docc = guestDocumentTable;

            $("#DocumentInfo").html(guestDocumentTable);
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }

        function AttachFile() {

            var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var path = "/Payroll/Images/CashRequisition/";
            var category = "CashRequisitionApprovalDoc";
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


            //$("#documentsDiv").dialog({
            //    autoOpen: true,
            //    modal: true,
            //    width: 700,
            //    closeOnEscape: true,
            //    resizable: false,
            //    title: "Add Documents",
            //    show: 'slide'
            //});
        }

        function CloseDialog() {
            $("#DocumentDialouge").dialog('close');
            return false;
        }
        function DeleteGuestDoc(docId, rowIndex) {
            if (confirm("Want to delete?")) {
                var deletedDoc = $("#<%=hfGuestDeletedDoc.ClientID %>").val();

                if (deletedDoc != "")
                    deletedDoc += "," + docId;
                else
                    deletedDoc = docId;

                $("#<%=hfGuestDeletedDoc.ClientID %>").val(deletedDoc);

                $("#trdoc" + rowIndex).remove();
            }
        }

    </script>

    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfCashRequisitionAdjustmentId" Value="0" runat="server" />
    <asp:HiddenField ID="hfCashRequisitionRefId" Value="0" runat="server" />
    <asp:HiddenField ID="hfSupplierAccountsHeadId" Value="0" runat="server" />
    <asp:HiddenField ID="hfIsCashRequisitionAdjustmentWithDifferentCompanyOrProjects" runat="server" Value="0" />

    <div>
        <div style="padding: 10px 30px 10px 30px">

            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblGLCompany" runat="server" class="control-label required-field" Text="Company"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlGLCompany" runat="server" CssClass="form-control" Enabled="false">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblGLProject" runat="server" class="control-label required-field" Text="Project"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlGLProject" runat="server" CssClass="form-control" Enabled="false">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblAssignEmployee" runat="server" class="control-label required-field" Text="Employee"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlAssignEmployee" CssClass="form-control" runat="server" Enabled="false">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Transaction From"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlTransactionFromAccountHeadId" runat="server" CssClass="form-control" Enabled="false">
                            <asp:ListItem Text="---Please Select---" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Remaining Balance"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtRemainingBalance" runat="server" CssClass="quantitydecimal form-control" Enabled="false"></asp:TextBox>
                    </div>
                </div>

                <div id="BillContainer">
                    <hr />
                    <div id="showCompanyAndProjectDivForEdit">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCompany" runat="server" class="control-label required-field" Text="Company"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblProject" runat="server" class="control-label required-field" Text="Project"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblAccountExpenseHead" runat="server" class="control-label required-field" Text="Adjustment For"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlAccountExpenseHead" runat="server" CssClass="form-control">
                                <asp:ListItem Text="---Please Select---" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group" id="SupplierInfoDiv" style="display:none;">
                        <div class="col-md-2">
                            <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Supplier"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlSupplierId" runat="server" CssClass="form-control">
                                <asp:ListItem Text="---Please Select---" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Adjustment Amount</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="quantitydecimal form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Purpose"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtRemarks" runat="server" TabIndex="6" CssClass="form-control"
                                TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group" style="padding-top: 10px;">
                        <div class="col-md-12">
                            <input id="btnAdd" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" onclick="AddItem()" />
                            <input id="btnCancelOrder" type="button" value="Cancel" onclick="ClearBillContainer()"
                                class="TransactionalButton btn btn-primary btn-sm" />
                        </div>
                    </div>

                    <div style="height: 250px; overflow-y: scroll;">
                        <table id="RequisitionForBillVoucherTbl" class="table table-bordered table-condensed table-hover">
                            <thead>
                                <tr>
                                    <th style="width: 20%;">Adjustment For</th>
                                    <th id="companyCol" style="width: 20%;">Company</th>
                                    <th id="projectCol" style="width: 20%;">Project</th>
                                    <th style="width: 15%;">Adjustment Amount</th>
                                    <th style="width: 15%;">Purpose</th>
                                    <th style="width: 5%;">Action</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>

                    </div>
                </div>
                <%--document div --%>
                <hr />
                <div id="documentsDiv" style="display: none;">
                    <label for="Attachment" class="control-label col-md-2">
                        Attachment</label>
                    <div class="col-md-4">
                        <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
                            <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                                FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
                        </asp:Panel>
                    </div>
                </div>

                <div id="DocumentDialouge" style="display: none;">
                    <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
                        clientidmode="static" scrolling="yes"></iframe>
                </div>

                <div class="form-group">
                    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>
                    <asp:HiddenField ID="RandomProductId" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="tempId" runat="server" Value="0"></asp:HiddenField>
                    <asp:HiddenField ID="hfGuestDeletedDoc" runat="server" Value=""></asp:HiddenField>
                    <div class="col-md-2">
                        <label class="control-label">Attachment</label>
                    </div>
                    <div class="col-md-10">
                        <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Attach" onclick="AttachFile()" />
                    </div>
                </div>
                <div id="DocumentInfo">
                </div>
                <hr />
                <div class="form-group">

                    <div class="col-md-2">
                        <label class="control-label required-field">Remarks</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox TextMode="MultiLine" ID="txtFinalRemarks" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                    <div class="col-md-12">
                        <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                            onclick="SaveNClose()" value="Save" />
                        <%--<input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                            onclick="javascript: return Clear();" />--%>
                    </div>
                </div>


            </div>
        </div>
    </div>

</asp:Content>
