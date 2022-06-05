<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="CashRequisitionIframe.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.CashRequisitionIframe" %>

<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;
        var isClose;
        var GlobalProjectId = 0;

        var GlobalApproveStatus = "";

        $(document).ready(function () {

            $("#ContentPlaceHolder1_companyProjectUserControl_hfDropdownFirstValue").val("select");


            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            var cashRequsitionId = $.trim(CommonHelper.GetParameterByName("crid"));
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            $("#ContentPlaceHolder1_ddlAssignEmployee").select2({
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
            CommonHelper.ApplyDecimalValidation();

            $('#ContentPlaceHolder1_txtRequireDate').datepicker({
                changeMonth: true,
                changeYear: true,
                minDate: DayOpenDate,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);


            if (cashRequsitionId != "") {
                PerformEdit(cashRequsitionId);
            }
            else {
                Clear();
            }
        });
        function PerformEdit(id) {
            // debugger;
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

            GlobalApproveStatus = Result.ApprovedStatus;
            $("#ContentPlaceHolder1_hfCashRequsitionId").val(Result.Id);
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val(Result.CompanyId).trigger('change');
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val(Result.ProjectId).trigger('change');
            $("#ContentPlaceHolder1_ddlAssignEmployee").val(Result.EmployeeId).change();

            $("#ContentPlaceHolder1_ddlTransactionFromAccountHeadId").val(Result.TransactionFromAccountHeadId).change();
            $("#ContentPlaceHolder1_txtRemarks").val(Result.Remarks);
            $("#ContentPlaceHolder1_txtAmount").val(Result.Amount);
            $("#ContentPlaceHolder1_txtRequireDate").val(CommonHelper.DateFromDateTimeToDisplay(Result.RequireDate, innBoarDateFormat));
            GlobalProjectId = Result.ProjectId;





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

        function SaveOrUpdateCashRequisition() {
            debugger;
            var id = +$("#ContentPlaceHolder1_hfCashRequsitionId").val();
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

            var amount = $("#ContentPlaceHolder1_txtAmount").val();
            if (amount == "") {
                isClose = false;
                toastr.warning("Add Amount");
                $("#ContentPlaceHolder1_txtAmount").focus();
                return false;
            }
            debugger;

            var requireDate = $("#ContentPlaceHolder1_txtRequireDate").val();
            if (requireDate == "") {
                isClose = false;
                toastr.warning("Select Require Date");
                $("#ContentPlaceHolder1_txtRequireDate").focus();
                return false;
            }

            var remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            if (remarks == "") {
                isClose = false;
                toastr.warning("Enter Remarks");
                $("#ContentPlaceHolder1_txtRemarks").focus();
                return false;
            }

            var cashRequisition = {
                Id: id,
                CompanyId: companyId,
                ProjectId: projectId,
                EmployeeId: employeeId,
                TransactionFromAccountHeadId: transactionFromAccountHeadId,
                Amount: amount,
                RequireDate: CommonHelper.DateFormatToMMDDYYYY(requireDate, '/'),
                Remarks: remarks,
                TransactionType: "Cash Requisition",
                ApprovedStatus: (GlobalApproveStatus == "" ? "Pending" : GlobalApproveStatus),
                RefId: 0
            }
            GlobalApproveStatus = "";
            var hfRandom = $("#<%=RandomProductId.ClientID %>").val();
            var deletedDocuments = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();

            PageMethods.SaveOrUpdateCashRequisition(cashRequisition, hfRandom, deletedDocuments, OnSuccessSaveOrUpdate, OnFailSaveOrUpdate);
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
        function Clear() {

            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val("0").trigger('change');
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val("0").trigger('change');

            $("#ContentPlaceHolder1_ddlTransactionFromAccountHeadId").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtAmount").val('');
            $("#ContentPlaceHolder1_txtRemarks").val('');

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
            $.when(SaveOrUpdateCashRequisition()).done(function () {
                if (isClose) {

                    if (typeof parent.CloseDialog === "function") {
                        parent.CloseDialog();
                    }

                }
            });
            return false;
        }

        function UploadComplete() {
            var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var id = +$("#ContentPlaceHolder1_hfCashRequsitionId").val();
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

                guestDocumentTable += "<td align='left' style='width: 50%; cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + guestDoc[row].Name + "</td>";

                if (guestDoc[row].Path != "") {
                    if (guestDoc[row].Extention == ".jpg" || guestDoc[row].Extention == ".png" || guestDoc[row].Extention == ".PNG" || guestDoc[row].Extention == ".JPG")
                        imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    else
                        imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                guestDocumentTable += "<td align='left' style='width: 30%'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + imagePath + "</td>";

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
            var category = "CashRequisitionDoc";
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
    <asp:HiddenField ID="hfCashRequsitionId" Value="0" runat="server" />
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
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Amount</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtAmount" runat="server" CssClass="quantitydecimal form-control"></asp:TextBox>
                    </div>

                    <div class="col-md-2">
                        <label class="control-label required-field">Require Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtRequireDate" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Remarks</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox TextMode="MultiLine" ID="txtRemarks" runat="server" CssClass="form-control"></asp:TextBox>
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
</asp:Content>
