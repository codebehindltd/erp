<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="frmSupplierCompanyBalanceTransfer.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.frmSupplierCompanyBalanceTransfer" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            $("#myTabs").tabs();
            $("#ContentPlaceHolder1_ddlTransactionType").change(function () {
                var transactionTypeId = $("#ContentPlaceHolder1_ddlTransactionType").val();
                if (transactionTypeId == "0") {
                    $("#SupplierToCompanyDiv").hide();
                } else {
                    $("#SupplierToCompanyDiv").show();
                }

                PageMethods.GetSupplierCompanyList(transactionTypeId, OnGetSupplierCompanyListSucceed, OnGetSupplierCompanyListFailed);
            });

            $("#ContentPlaceHolder1_ddlFromSearch").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlToSearch").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlFrom").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlTo").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            if ($("#ContentPlaceHolder1_ddlTransactionType").val() == "0") {
                $("#SupplierToCompanyDiv").hide();
            } else {
                $("#SupplierToCompanyDiv").show();
            }
        });
        function OnGetSupplierCompanyListSucceed(result) {
            $("#ContentPlaceHolder1_lblFrom").text("From " + result[2]);
            $("#ContentPlaceHolder1_lblTo").text("To " + result[3]);
            $('#ContentPlaceHolder1_ddlFrom').empty();
            $('#ContentPlaceHolder1_ddlTo').empty();
            var fromId = $("#ContentPlaceHolder1_hfEditFromTransaction").val();
            $('#ContentPlaceHolder1_ddlFrom').append($('<option/>', {
                value: 0,
                text: "--- Please Select ---"
            }));
            $.each(result[0], function (key, value) {
                if (value.CompanyId) {
                    $('#ContentPlaceHolder1_ddlFrom').append($('<option/>', {
                        value: value.CompanyId,
                        text: value.Name
                    }));
                } else if (value.SupplierId) {
                    $('#ContentPlaceHolder1_ddlFrom').append($('<option/>', {
                        value: value.SupplierId,
                        text: value.Name
                    }));
                }
            });
            $('#ContentPlaceHolder1_ddlFrom option[value="' + fromId + '"]').attr("selected", "selected");
            var toId = $("#ContentPlaceHolder1_hfEditToTransaction").val();
            $('#ContentPlaceHolder1_ddlTo').append($('<option/>', {
                value: 0,
                text: "--- Please Select ---"
            }));
            $.each(result[1], function (key, value) {
                if (value.CompanyId) {
                    $('#ContentPlaceHolder1_ddlTo').append($('<option/>', {
                        value: value.CompanyId,
                        text: value.Name
                    }));
                } else if (value.SupplierId) {
                    $('#ContentPlaceHolder1_ddlTo').append($('<option/>', {
                        value: value.SupplierId,
                        text: value.Name
                    }));
                }
            });
            $('#ContentPlaceHolder1_ddlTo option[value="' + toId + '"]').attr("selected", "selected");
        }

        function OnGetSupplierCompanyListFailed() {

        }

        function Clear() {
            $("#ContentPlaceHolder1_ddlTransactionType").val("0");
            $("#ContentPlaceHolder1_ddlFrom").val("1");
            $("#ContentPlaceHolder1_ddlTo").val("1");
            $("#ContentPlaceHolder1_txtAmount").val("");
            $("#ContentPlaceHolder1_txtRemarks").val("");
        }

        function SaveTransferInfo() {
            var transactionType = "", fromTransactionId = "0", toTransactionId = "0", amount = 0.00, editId = "", remarks = "";
            transactionType = $("#ContentPlaceHolder1_ddlTransactionType :selected").text();
            transactionTypeId = $("#ContentPlaceHolder1_ddlTransactionType").val();
            fromTransactionId = $("#ContentPlaceHolder1_ddlFrom").val();
            fromTransactionText = $("#ContentPlaceHolder1_ddlFrom :selected").text();
            toTransactionId = $("#ContentPlaceHolder1_ddlTo").val();
            toTransactionText = $("#ContentPlaceHolder1_ddlTo :selected").text();
            amount = $("#ContentPlaceHolder1_txtAmount").val();
            remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            if ($("#ContentPlaceHolder1_ddlFrom").val() == "0") {
                if (transactionTypeId == 1 || transactionTypeId == 3) {
                    toastr.warning("Please Provide From Supplier.");
                }
                else if (transactionTypeId == 2 || transactionTypeId == 4) {
                    toastr.warning("Please Provide From Company.");
                }
                $("#ContentPlaceHolder1_ddlFrom").focus();
                return false;
            }
            if ($("#ContentPlaceHolder1_ddlTo").val() == "0") {
                if (transactionTypeId == 2 || transactionTypeId == 3) {
                    toastr.warning("Please Provide To Supplier.");
                }
                else if (transactionTypeId == 1 || transactionTypeId == 4) {
                    toastr.warning("Please Provide To Company.");
                }
                $("#ContentPlaceHolder1_ddlTo").focus();
                return false;
            }
            if ($("#ContentPlaceHolder1_txtAmount").val() == "") {
                toastr.warning("Please Provide Amount.");
                $("#ContentPlaceHolder1_txtAmount").focus();
                return false;
            }
            if ($("#ContentPlaceHolder1_txtRemarks").val() == "") {
                toastr.warning("Please Provide Description.");
                $("#ContentPlaceHolder1_txtRemarks").focus();
                return false;
            }
            if (transactionTypeId == "0") {
                toastr.info("Please Select Transaction Type");
                return false;
            }
            else if (amount == 0.00) {
                toastr.info("Insert an Amount");
                return false;
            }
            editId = $("#ContentPlaceHolder1_hfUpdateId").val();

            if (fromTransactionText == toTransactionText) {
                if (transactionTypeId == "3") {
                    toastr.info("You can't transfer balance to the Same Supplier.");
                } else {
                    toastr.info("You can't transfer balace to the same Company.");
                }
            } else {
                PageMethods.SaveSupplierCompanyBalanceTransfer(editId, transactionType, fromTransactionId, toTransactionId, amount, remarks, OnSaveSupplierCompanyBalanceTransferSucceed, OnSaveSupplierCompanyBalanceTransferFailed);
            }
            return false;
        }
        function OnSaveSupplierCompanyBalanceTransferSucceed(result) {
            var editId = "";
            editId = $("#ContentPlaceHolder1_hfUpdateId").val();
            if (result) {
                if (editId == "") {
                    toastr.success("Balance Successfully Transfered.");
                } else {
                    toastr.success("Successfully Updated.");
                    $("#ContentPlaceHolder1_btnSave").val("Save");
                }
                
                $("#ContentPlaceHolder1_ddlTransactionType").val("0");
                $("#ContentPlaceHolder1_txtAmount").val("");
                if ($("#ContentPlaceHolder1_ddlTransactionType").val() == "0") {
                    $("#SupplierToCompanyDiv").hide();
                } else {
                    $("#SupplierToCompanyDiv").show();
                }
                $("#ContentPlaceHolder1_txtRemarks").val("");
            }
            else {
                toastr.warning("Balance Transfer Failed.");
            }
        }
        function OnSaveSupplierCompanyBalanceTransferFailed() {
            
        }
    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="1" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="1" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="1" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="1" />
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfProjectIdList" runat="server" />
    <asp:HiddenField ID="txtMonthSetupId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfUpdateId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfEditFromTransaction" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfEditToTransaction" runat="server"></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Balance Transfer</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search</a></li>
        </ul>
        <div id="tab-1">
            <div id="SupplierCompanyBalanceTransfer" class="panel panel-default">
                <div class="panel-heading">
                    Balance Transfer
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblTransactionType" runat="server" class="control-label" Text="Transaction Type"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlTransactionType" runat="server" CssClass="form-control"
                                    TabIndex="1">
                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="1">Supplier To Company</asp:ListItem>
                                    <asp:ListItem Value="2">Company To Supplier</asp:ListItem>
                                    <asp:ListItem Value="3">Supplier To Supplier</asp:ListItem>
                                    <asp:ListItem Value="4">Company To Company</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="SupplierToCompanyDiv" style="display: none;">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblFrom" runat="server" class="control-label required-field" Text="From"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlFrom" runat="server" CssClass="form-control" TabIndex="2">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblTo" runat="server" class="control-label required-field" Text="To"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlTo" runat="server" CssClass="form-control" TabIndex="3">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblAmount" runat="server" class="control-label required-field" Text="Amount"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblRemarks" runat="server" class="control-label required-field" Text="Description"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtRemarks" CssClass="form-control" TextMode="MultiLine" runat="server"
                                        TabIndex="11"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-12">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="5"
                                        CssClass="TransactionalButton btn btn-primary btn-sm" OnClientClick="javascript:return SaveTransferInfo()" />
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="6"
                                        CssClass="TransactionalButton btn btn-primary btn-sm" OnClientClick="javascript:return Clear()" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchSupplierCompanyBalanceTransfer" class="panel panel-default">
                <div class="panel-heading">
                    Search Balance Transfer
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblTransactionTypeSearch" runat="server" class="control-label" Text="Transaction Type"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlTransactionTypeSearch" runat="server" CssClass="form-control"
                                    TabIndex="1">
                                    <asp:ListItem Value="0">--- All ---</asp:ListItem>
                                    <asp:ListItem Value="1">Supplier To Company</asp:ListItem>
                                    <asp:ListItem Value="2">Company To Supplier</asp:ListItem>
                                    <asp:ListItem Value="3">Supplier To Supplier</asp:ListItem>
                                    <asp:ListItem Value="4">Company To Company</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="TransactionTypeSearchDiv">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblFromSearch" runat="server" class="control-label" Text="From"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlFromSearch" runat="server" CssClass="form-control" TabIndex="2">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblToSearch" runat="server" class="control-label" Text="To"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlToSearch" runat="server" CssClass="form-control" TabIndex="3">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <input type="button" class="btn btn-primary btn-sm" value="Search" onclick="SearchTransaction()" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchResult" class="panel panel-default">
                    <div class="panel-heading">
                        Search Information
                    </div>
                    <div class="panel-body">
                        <table id="TransactionInfoSearch" class="table table-bordered table-condensed table-hover table-responsive">
                            <thead>
                                <tr>
                                    <th style="width: 10%;">Transaction Type</th>
                                    <th style="width: 15%;">From Transaction</th>
                                    <th style="width: 15%;">To Transaction</th>
                                    <th style="width: 10%;">Amount</th>
                                    <th style="width: 30%">Description</th>
                                    <th style="width: 10%">Status</th>
                                    <th style="width: 10%;">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
        </div>
    </div>
    <script>
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            $("#ContentPlaceHolder1_ddlTransactionTypeSearch").change(function () {
                
                var transactionTypeSearchId = $("#ContentPlaceHolder1_ddlTransactionTypeSearch").val();
                if (transactionTypeSearchId == "0") {
                    $("#TransactionTypeSearchDiv").hide();
                } else {
                    $("#TransactionTypeSearchDiv").show();
                }
                PageMethods.GetSupplierCompanyList(transactionTypeSearchId, OnSearchSupplierCompanyListSucceed, OnSearchSupplierCompanyListFailed);
            });

            if ($("#ContentPlaceHolder1_ddlTransactionTypeSearch").val() == "0") {
                $("#ContentPlaceHolder1_ddlTransactionTypeSearch :selected").text("--- All ---");
                $("#TransactionTypeSearchDiv").hide();
            } else {
                $("#TransactionTypeSearchDiv").show();
            }

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_txtFromDate").blur(function () {
                var date = $("#ContentPlaceHolder1_txtFromDate").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#ContentPlaceHolder1_txtFromDate").focus();
                        $("#ContentPlaceHolder1_txtFromDate").val(DayOpenDate);
                        return false;
                    }
                }

            });
            $("#ContentPlaceHolder1_txtToDate").blur(function () {
                var date = $("#ContentPlaceHolder1_txtToDate").val();
                if (date != "") {
                    date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                    var isValid = CommonHelper.IsVaildDate(date);
                    if (!isValid) {
                        toastr.warning("Invalid Date");
                        $("#ContentPlaceHolder1_txtToDate").focus();
                        $("#ContentPlaceHolder1_txtToDate").val(DayOpenDate);
                        return false;
                    }
                }

            });
        });

        function OnSearchSupplierCompanyListSucceed(result) {
            $("#ContentPlaceHolder1_lblFromSearch").text("From " + result[2]);
            $("#ContentPlaceHolder1_lblToSearch").text("To " + result[3]);
            $('#ContentPlaceHolder1_ddlFromSearch').empty();
            $('#ContentPlaceHolder1_ddlToSearch').empty();

            $('#ContentPlaceHolder1_ddlFromSearch').append($('<option/>', {
                value: 0,
                text: "--- All ---"
            }));
            $.each(result[0], function (key, value) {
                if (value.CompanyId) {
                    $('#ContentPlaceHolder1_ddlFromSearch').append($('<option/>', {
                        value: value.CompanyId,
                        text: value.Name
                    }));
                } else if (value.SupplierId) {
                    $('#ContentPlaceHolder1_ddlFromSearch').append($('<option/>', {
                        value: value.SupplierId,
                        text: value.Name
                    }));
                }
            });

            $('#ContentPlaceHolder1_ddlToSearch').append($('<option/>', {
                value: 0,
                text: "--- All ---"
            }));
            $.each(result[1], function (key, value) {
                if (value.CompanyId) {
                    $('#ContentPlaceHolder1_ddlToSearch').append($('<option/>', {
                        value: value.CompanyId,
                        text: value.Name
                    }));
                } else if (value.SupplierId) {
                    $('#ContentPlaceHolder1_ddlToSearch').append($('<option/>', {
                        value: value.SupplierId,
                        text: value.Name
                    }));
                }
            });
            
        }

        function OnSearchSupplierCompanyListFailed() {

        }

        function SearchTransaction() {
            var dateFrom = null, dateTo = null, transactionTypeSearch, fromTransaction, toTransaction;

            if ($("#ContentPlaceHolder1_txtFromDate").val() != "")
                dateFrom = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtFromDate").val(), '/');

            if ($("#ContentPlaceHolder1_txtToDate").val() != "")
                dateTo = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtToDate").val(), '/');
            transactionTypeSearch = $("#ContentPlaceHolder1_ddlTransactionTypeSearch :selected").text();
            
            fromTransaction = $("#ContentPlaceHolder1_ddlFromSearch").val();
            toTransaction = $("#ContentPlaceHolder1_ddlToSearch").val();
            if (transactionTypeSearch == "--- All ---") {
                fromTransaction = 0;
                toTransaction = 0;
            }
           
            $("#TransactionInfoSearch tbody").html("");
            PageMethods.GetTransactionsBySearch(transactionTypeSearch, fromTransaction, toTransaction, dateFrom, dateTo, OnGetTransactionsBySearchSucceeded, OnGetTransactionsBySearchFailed);

            return false;
        }

        function OnGetTransactionsBySearchSucceeded(result) {
            var tr = "";
            $.each(result, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style='width:10%;'>" + gridObject.TransactionType + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.FromTransactionText + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.ToTransactionText + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.Amount + "</td>";

                if (gridObject.Remarks != null)
                    tr += "<td style='width:30%;'>" + gridObject.Remarks + "</td>";
                else
                    tr += "<td style='width:30%;'></td>";
                tr += "<td style='width:10%;'>" + (gridObject.ApprovedStatus == 'Pending' ? 'Pending' : gridObject.ApprovedStatus) + "</td>";
                tr += "<td style=\"text-align: center; width:10%; cursor:pointer;\">";
                if (gridObject.IsCanEdit && IsCanEdit) {
                    tr += "<a onclick=\"javascript:return FIllForEdit(" + gridObject.Id + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                }

                if (gridObject.IsCanDelete && IsCanDelete) {
                    tr += "<a href='javascript:void();' onclick= 'javascript:return DeleteTransactionInfo(" + gridObject.Id + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                }

                if (gridObject.IsCanChecked && IsCanSave) {
                    tr += "<a href='javascript:void();' onclick= 'javascript:return CheckedTransfer(" + gridObject.Id + ")' ><img alt='Checked' src='../Images/checked.png' /></a>";
                }

                if (gridObject.IsCanApproved && IsCanSave) {
                    tr += "<a href='javascript:void();' onclick= 'javascript:return ApprovedTransfer(" + gridObject.Id + ")' ><img alt='approved' src='../Images/approved.png' /></a>";
                }

                //tr += "&nbsp;&nbsp;";

                //tr += "<a href='javascript:void();' onclick= 'javascript:return ShowReport(" + gridObject.Id + ")' ><img alt='approved' src='../Images/ReportDocument.png' /></a>";

                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.Id + "</td>";

                tr += "</tr>";

                $("#TransactionInfoSearch tbody").append(tr);
                tr = "";
            });
        }
        function OnGetTransactionsBySearchFailed() { }

        function FIllForEdit(Id) {
            if (!confirm("Do you want to edit?")) {
                return false;
            }
            $("#TransactionInfoSearch tbody").html("");
            PageMethods.FillForm(Id, OnFillFormSucceed, OnFillFormFailed);
            return false;
        }
        function OnFillFormSucceed(result) {
            var editTransactionId;
            if (result.TransactionType == "Supplier To Company") {
                editTransactionId = "1";
            } else if (result.TransactionType == "Company To Supplier") {
                editTransactionId = "2";
            } else if (result.TransactionType == "Supplier To Supplier") {
                editTransactionId = "3";
            } else if (result.TransactionType == "Company To Company") {
                editTransactionId = "4";
            }
            $("#ContentPlaceHolder1_hfEditFromTransaction").val(result.FromTransactionId);
            $("#ContentPlaceHolder1_hfEditToTransaction").val(result.ToTransactionId);

            $("#ContentPlaceHolder1_ddlTransactionType").val(editTransactionId).trigger("change");
            $("#ContentPlaceHolder1_txtAmount").val(result.Amount);
            $("#ContentPlaceHolder1_txtRemarks").val(result.Remarks);
            $("#ContentPlaceHolder1_btnSave").val("Update");
            $("#ContentPlaceHolder1_hfUpdateId").val(result.Id);
            
            $("#myTabs").tabs({ active: 0 });
        }

        function OnFillFormFailed(error) {
            toastr.error(error.get_message());
        }

        function DeleteTransactionInfo(Id) {
            if (confirm("Do you want to delete?")) {
                PageMethods.DeleteTransactionInfo(Id, OnDeleteTransactionInfoSucceeded, OnDeleteTransactionInfoFailed);
            }
        }

        function OnDeleteTransactionInfoSucceeded(result) {
            if (result) {
                toastr.success("Deleted Successfully.");
                SearchTransaction();
            }
            else {
                toastr.warning("Deletion Failed.");
            }
        }
        function OnDeleteTransactionInfoFailed(result) {

        }

        function CheckedTransfer(Id) {
            if (confirm("Do You Want to Check? ")) {
                PageMethods.CheckedTransfer(Id, OnCheckedSucceed, OnCheckedFailed);
                return false;
            }
        }
        function OnCheckedSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchTransaction();
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnCheckedFailed(error) {
            toastr.error(error.get_message());
        }

        function ApprovedTransfer(Id) {
            if (confirm("Do You Want to Approve? ")) {
                PageMethods.ApprovedTransfer(Id, OnApprovedSucceed, OnApprovedFailed);
                return false;
            }
        }
        function OnApprovedSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchTransaction();
                PerformClearAction();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnApprovedFailed(error) {
            toastr.error(error.get_message());
        }
    </script>
</asp:Content>
