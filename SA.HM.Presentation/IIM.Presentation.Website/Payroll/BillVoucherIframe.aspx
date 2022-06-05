<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="BillVoucherIframe.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.BillVoucherIframe" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>

<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;
        var isClose;
        var GlobalProjectId = 0;
        var ItemEdited = ""; var indexEdited = -1;

        var GlobalApproveStatus = "";

        var RequisitionForBillVoucher = new Array();
        var RequisitionForBillVoucherDeleted = new Array();

        $(document).ready(function () {

            $("#ContentPlaceHolder1_companyProjectUserControl_hfDropdownFirstValue").val("select");

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            var billVoucherId = $.trim(CommonHelper.GetParameterByName("bvid"));
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
            $("#ContentPlaceHolder1_ddlTransactionFromAccountHeadId").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlAssignEmployee").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });


            if (billVoucherId != "") {
                PerformEdit(billVoucherId);
            }
            else {
                Clear();
            }
        });

        function ClearBillContainer() {
            $("#ContentPlaceHolder1_ddlAccountExpenseHead").val("0").change();
            $("#ContentPlaceHolder1_txtAmount").val("");
            $("#ContentPlaceHolder1_txtRemarks").val("");
            $("#btnAdd").val("Add");

            ItemEdited = ""; indexEdited = -1;

            //$("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").attr("disabled", true);
            //$("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").attr("disabled", true);
        }


        function PerformEdit(id) {
            //debugger;
            PageMethods.GetRequsitionById(id, OnSuccessLoading, OnFailLoading)
            return false;
        }

        function OnSuccessLoading(result) {
            FillForm(result);
            return false;
        }
        function FillForm(Result) {
            //debugger;
            Clear();

            GlobalApproveStatus = Result[0].ApprovedStatus;
            //debugger;
            $("#ContentPlaceHolder1_hfBillVoucherId").val(Result[0].Id);
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val(Result[0].CompanyId).trigger('change');
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val(Result[0].ProjectId).trigger('change');
            $("#ContentPlaceHolder1_ddlAssignEmployee").val(Result[0].EmployeeId).change();

            $("#ContentPlaceHolder1_ddlTransactionFromAccountHeadId").val(Result[0].TransactionFromAccountHeadId).change();
            $("#ContentPlaceHolder1_txtFinalRemarks").val(Result[0].Remarks);
            GlobalProjectId = Result[0].ProjectId;
            var tr = "";
            for (var i = 0; i < Result.length; i++) {
                tr += "<tr>";

                tr += "<td style='width:55%;'>" + Result[i].RequisitionForHeadName + "</td>";

                tr += "<td style='width:15%;'>" +
                    "<input type='text' value='" + Result[i].RequsitionAmount + "' id='pp" + Result[i].RequisitionForHeadId + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoq(this)'  />" +
                    "</td>";
                tr += "<td style='width:15%;'>" +
                    "<textarea id='ppr" + Result[i].RequisitionForHeadId + "' onblur='CalculateTotalForAdhoq(this)' style='width:100%;'>" + Result[i].IndividualRemarks + "</textarea>" +
                    //"<input type='text' value='" + Result[i].IndividualRemarks + "' id='ppr" + Result[i].RequisitionForHeadId + "' class='form-control' onblur='CalculateTotalForAdhoq(this)' />" +
                    "</td>";

                tr += "<td style='width:10%;'>";
                tr += "<a href='javascript:void()' onclick= 'EditAdhoqItem(this)' ><img alt='Delete' src='../Images/edit.png' /></a>";
                tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                tr += "</td>";
                tr += "<td style='display:none;'>" + Result[i].RequisitionForHeadId + "</td>";
                tr += "<td style='display:none;'>" + Result[i].DetailId + "</td>";

                tr += "</tr>";

                RequisitionForBillVoucher.push({
                    CostCenterId: parseInt(Result[i].RequisitionForHeadId, 10),
                    RequsitionAmount: Result[i].RequsitionAmount,
                    Remarks: Result[i].IndividualRemarks,
                    CompanyId: Result[i].CompanyId,
                    ProjectId: Result[i].ProjectId,
                    DetailId: Result[i].DetailId,
                    Id: Result[i].Id,
                    CostCenterName: Result[i].RequisitionForHeadName
                });
            }

            $("#RequisitionForBillVoucherTbl tbody").prepend(tr);

            if (IsCanEdit)
                $("#btnSave").val('Update').show();
            else
                $("#btnSave").hide();
            $("#btnClear").hide();
            UploadComplete();
            return false;
        }

        function OnFailLoading(error) {
            toastr.error(error.get_message());
            return false;
        }

        function AddItem() {
            //debugger;
            var costCenter = $("#ContentPlaceHolder1_ddlAccountExpenseHead option:selected").val();
            var costCenterName = $("#ContentPlaceHolder1_ddlAccountExpenseHead option:selected").text();
            if (costCenter == "0") {
                toastr.warning("Select a Cost Center For Requisition For.");
                $("#ContentPlaceHolder1_ddlAccountExpenseHead").focus();
                return false;
            }
            var CostCenterOBJ = _.findWhere(RequisitionForBillVoucher, { CostCenterId: parseInt(costCenter, 10) });
            if (CostCenterOBJ != null) {
                toastr.warning("It's already added.");
                $("#ContentPlaceHolder1_ddlAccountExpenseHead").focus();
                return false;


            }
            var requsitionAmount = $("#ContentPlaceHolder1_txtAmount").val();
            if (requsitionAmount == "") {
                toastr.warning("Add Requsition Amount.");
                $("#ContentPlaceHolder1_txtAmount").focus();
                return false;
            }

            var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            if (companyId == "0") {
                isClose = false;
                toastr.warning("Select a company.");
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").focus();
                return false;
            }
            var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
            if (projectId == "0") {
                isClose = false;
                toastr.warning("Select a project.");
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").focus();
                return false;
            }
            var transactionFromAccountHeadId = $("#ContentPlaceHolder1_ddlTransactionFromAccountHeadId").val();
            if (transactionFromAccountHeadId == "0") {
                isClose = false;
                toastr.warning("Select a Transaction From.");
                $("#ContentPlaceHolder1_ddlTransactionFromAccountHeadId").focus();
                return false;
            }

            if (costCenter == transactionFromAccountHeadId) {
                isClose = false;
                toastr.warning("Transaction From And Requisition For can not be same.");
                $("#ContentPlaceHolder1_ddlAccountExpenseHead").focus();
                return false;
            }

            var remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            //if (remarks == "") {
            //    toastr.warning("Add remarks.");
            //    $("#ContentPlaceHolder1_txtRemarks").focus();
            //    return false;
            //}
            debugger;



            if (ItemEdited == "" && $("#btnAdd").val() != "Update") {
                var tr = "";
                tr += "<tr>";

                tr += "<td style='width:55%;'>" + costCenterName + "</td>";

                tr += "<td style='width:15%;'>" +
                    "<input type='text' value='" + requsitionAmount + "' id='pp" + costCenter + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoq(this)'  />" +
                    "</td>";
                tr += "<td style='width:15%;'>" +
                    "<textarea id='ppr" + costCenter + "' onblur='CalculateTotalForAdhoq(this)' style='width:100%;'>" + remarks + "</textarea>" +
                    //"<input type='text' value='" + remarks + "' id='ppr" + costCenter + "' class='form-control' onblur='CalculateTotalForAdhoq(this)' />" +
                    "</td>";

                tr += "<td style='width:10%;'>";
                tr += "<a href='javascript:void()' onclick= 'EditAdhoqItem(this)' ><img alt='Delete' src='../Images/edit.png' /></a>";
                tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                tr += "</td>";
                tr += "<td style='display:none;'>" + costCenter + "</td>";
                tr += "<td style='display:none;'>" + 0 + "</td>";

                tr += "</tr>";

                $("#RequisitionForBillVoucherTbl tbody").prepend(tr);
                tr = "";
                CommonHelper.ApplyDecimalValidation();
                RequisitionForBillVoucher.push({
                    CostCenterId: parseInt(costCenter, 10),
                    RequsitionAmount: requsitionAmount,
                    Remarks: remarks,
                    CompanyId: companyId,
                    ProjectId: projectId,
                    DetailId: 0,
                    Id: 0,
                    CostCenterName: costCenterName
                });
            }
            else {
                $(ItemEdited).find("td:eq(1)").find("input").val(requsitionAmount);
                $(ItemEdited).find("td:eq(2)").find("textarea").val(remarks);
                $(ItemEdited).find("td:eq(0)").text(costCenterName);
                $(ItemEdited).find("td:eq(4)").text(costCenter);


                RequisitionForBillVoucher[indexEdited].RequsitionAmount = parseFloat(requsitionAmount);
                RequisitionForBillVoucher[indexEdited].Remarks = (remarks);
                RequisitionForBillVoucher[indexEdited].CostCenterId = parseInt(costCenter, 10);
                RequisitionForBillVoucher[indexEdited].CostCenterName = costCenterName;

            }

            ClearBillContainer();
            $("#ContentPlaceHolder1_ddlAccountExpenseHead").focus();
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").attr("disabled", true);
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").attr("disabled", true);
        }

        function CalculateTotalForAdhoq(control) {

            //debugger;
            var tr = $(control).parent().parent();

            var reAmount = $.trim($(tr).find("td:eq(1)").find("input").val());
            var reMarks = $.trim($(tr).find("td:eq(2)").find("textarea").val());
            // debugger;

            if (reAmount == "" || reAmount == "0") {
                toastr.info("Amount Cannot Be Zero Or Empty.");
                return false;
            }

            if (reMarks == "") {
                toastr.info("Purpose Cannot Be Empty.");
                return false;
            }

            var costCenterId = parseInt($.trim($(tr).find("td:eq(4)").text()), 10);

            var CostCenter = _.findWhere(RequisitionForBillVoucher, { CostCenterId: costCenterId });
            var index = _.indexOf(RequisitionForBillVoucher, CostCenter);

            RequisitionForBillVoucher[index].RequsitionAmount = parseFloat(reAmount);
            RequisitionForBillVoucher[index].Remarks = (reMarks);
        }

        function DeleteAdhoqItem(control) {

            if (!confirm("Do you want to delete?")) { return false; }

            //debugger;

            var tr = $(control).parent().parent();

            var costCenter = parseInt($.trim($(tr).find("td:eq(4)").text()), 10);
            var detailsId = parseInt($.trim($(tr).find("td:eq(5)").text()), 10);

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

            var costCenter = parseInt($.trim($(tr).find("td:eq(4)").text()), 10);
            var detailsId = parseInt($.trim($(tr).find("td:eq(5)").text()), 10);

            var CostCenter = _.findWhere(RequisitionForBillVoucher, { CostCenterId: costCenter });
            indexEdited = _.indexOf(RequisitionForBillVoucher, CostCenter);

            $("#ContentPlaceHolder1_txtAmount").val(CostCenter.RequsitionAmount);
            $("#ContentPlaceHolder1_txtRemarks").val(CostCenter.Remarks);
            $("#ContentPlaceHolder1_ddlAccountExpenseHead").val(CostCenter.CostCenterId).trigger('change');

            $("#btnAdd").val("Update");


        }

        function Clear() {


            $("#ContentPlaceHolder1_ddlTransactionFromAccountHeadId").val("0").trigger('change');

            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val("0").trigger('change');
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val("0").trigger('change');
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
            var id = +$("#ContentPlaceHolder1_hfBillVoucherId").val();
            var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            if (companyId == "0") {
                isClose = false;
                toastr.warning("Select a company.");
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").focus();
                return false;
            }
            var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
            if (projectId == "0") {
                isClose = false;
                toastr.warning("Select a project.");
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").focus();
                return false;
            }

            var employeeId = $("#ContentPlaceHolder1_ddlAssignEmployee").val();
            if (employeeId == "0") {
                isClose = false;
                toastr.warning("Select a employee.");
                $("#ContentPlaceHolder1_ddlAssignEmployee").focus();
                return false;
            }


            var transactionFromAccountHeadId = $("#ContentPlaceHolder1_ddlTransactionFromAccountHeadId").val();
            if (transactionFromAccountHeadId == "0") {
                isClose = false;
                toastr.warning("Select a Transaction From.");
                $("#ContentPlaceHolder1_ddlTransactionFromAccountHeadId").focus();
                return false;
            }

            var remarks = $("#ContentPlaceHolder1_txtFinalRemarks").val();
            if (remarks == "") {
                isClose = false;
                toastr.warning("Please Enter Purpose");
                $("#ContentPlaceHolder1_txtFinalRemarks").focus();
                return false;
            }

            var totalAmount = 0.0;
            var RequisitionForBillVoucherNewlyAdded = new Array();
            RequisitionForBillVoucherNewlyAdded = RequisitionForBillVoucher;
            var row = 0, rowCount = RequisitionForBillVoucherNewlyAdded.length;
            for (row = 0; row < rowCount; row++) {
                if (parseInt(transactionFromAccountHeadId, 10) == RequisitionForBillVoucherNewlyAdded[row].CostCenterId) {
                    isClose = false;
                    toastr.warning("Transaction From And Requisition For can not be same.");
                    $("#ContentPlaceHolder1_ddlTransactionFromAccountHeadId").focus();
                    return false;
                }
                if (RequisitionForBillVoucherNewlyAdded[row].RequsitionAmount == "" || RequisitionForBillVoucherNewlyAdded[row].RequsitionAmount == "0") {
                    toastr.warning("Requsition Amount Cannot NUll OR Empty In " + RequisitionForBillVoucherNewlyAdded[row].CostCenterName);
                    break;
                }
                totalAmount += parseFloat(RequisitionForBillVoucherNewlyAdded[row].RequsitionAmount);
            }
            if (row == 0) {
                isClose = false;
                toastr.warning("Add some data for Bill");
                return false;
            }

            if (row != rowCount) {
                return false;
            }
            var Requisition = {
                Id: id,
                CompanyId: companyId,
                ProjectId: projectId,
                EmployeeId: employeeId,
                TransactionFromAccountHeadId: transactionFromAccountHeadId,
                Amount: totalAmount,
                Remarks: remarks,
                TransactionType: "Bill Voucher",
                ApprovedStatus: (GlobalApproveStatus == "" ? "Pending" : GlobalApproveStatus),
                RefId: 0
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
            var id = +$("#ContentPlaceHolder1_hfBillVoucherId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();

            PageMethods.LoadContactDocument(id, randomId, deletedDoc, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
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

                guestDocumentTable += "<td align='left' style='width: 50%; cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + guestDoc[row].Name + "</td>";

                if (guestDoc[row].Path != "") {
                    if (guestDoc[row].Extention == ".jpg" || guestDoc[row].Extention == ".png" || guestDoc[row].Extention == ".PNG" || guestDoc[row].Extention == ".JPG")
                        imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    else
                        imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                guestDocumentTable += "<td align='left' style='width: 30%;cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + imagePath + "</td>";

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

        function CloseDialog() {
            $("#DocumentDialouge").dialog('close');
            return false;
        }

        function AttachFile() {

            var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var path = "/Payroll/Images/BillVoucher/";
            var category = "BillVoucherDoc";
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
            //    width: 900,
            //    closeOnEscape: true,
            //    resizable: false,
            //    title: "Add Documents",
            //    show: 'slide'
            //});
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

    </script>

    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfBillVoucherId" Value="0" runat="server" />
    <div id="ShowDocumentDiv" style="display: none;">
        <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div>
        <div style="padding: 10px 30px 10px 30px">

            <div class="form-horizontal">
                <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblAssignEmployee" runat="server" class="control-label required-field" Text="Employee"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlAssignEmployee" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Transaction From"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlTransactionFromAccountHeadId" runat="server" CssClass="form-control">
                            <asp:ListItem Text="---Please Select---" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>


                <div id="BillContainer">
                    <hr />
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblAccountExpenseHead" runat="server" class="control-label required-field" Text="Requisition For"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlAccountExpenseHead" runat="server" CssClass="form-control">
                                <asp:ListItem Text="---Please Select---" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Requisition Amount</label>
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
                                    <th style="width: 40%;">Requisition For</th>
                                    <th style="width: 20%;">Requisition Amount</th>
                                    <th style="width: 30%;">Purpose</th>
                                    <th style="width: 10%;">Action</th>
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
                        <asp:TextBox TextMode="MultiLine" ID="txtFinalRemarks" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                    <div class="col-md-12">
                        <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                            onclick="SaveNClose()" value="Save" />
                        <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                            onclick="javascript: return Clear();" />
                    </div>
                </div>


            </div>
        </div>
    </div>

</asp:Content>
