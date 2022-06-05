<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmCompanyBillGeneration.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmCompanyBillGeneration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#myTabs").tabs();

            $("#ContentPlaceHolder1_ddlCompany").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlCompanyForSearch").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $('#ContentPlaceHolder1_txtPaymentDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);

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


            if ($("#ContentPlaceHolder1_hfIsSavePermission").val() == "0")
                $("#btnGenerate").hide();

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
            $("#ContentPlaceHolder1_txtPaymentDate").blur(function () {
                var date = $("#ContentPlaceHolder1_txtPaymentDate").val();
                 if (date != "") {
                     date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
                     var isValid = CommonHelper.IsVaildDate(date);
                     if (!isValid) {
                         toastr.warning("Invalid Date");
                         $("#ContentPlaceHolder1_txtPaymentDate").focus();
                         $("#ContentPlaceHolder1_txtPaymentDate").val(DayOpenDate);
                         return false;
                     }
                 }

             });
        });

        function SearchCompanyBill() {
            var companyId = $("#ContentPlaceHolder1_ddlCompany").val();
            $("#TotalAmount").text("0.00");
            $("#TotalAmountUsd").text("0.00");
            $("#BillInfo tbody").html("");

            if (companyId == "0") {
                toastr.warning("Please Select Company.");
                return false;
            }

            CommonHelper.SpinnerOpen();
            $("#BillInfo tbody").html("");
            PageMethods.CompanyBillBySearch(companyId, OnLoadCompanyBillSucceeded, OnCompanyBillFailed);
        }

        function OnLoadCompanyBillSucceeded(result) {

            var row = 0, tr = "", chk = "checked='checked'", isChecked = 0;

            for (row = 0; row < result.length; row++) {

                isChecked = result[row].IsBillGenerated == true ? "1" : "0";

                if ((row + 1) % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width: 10%'> ";

                if (result[row].IsBillGenerated) {
                    tr += "<input type='checkbox' id='pay" + result[row].CompanyPaymentId + "'" + chk + " onclick='CalculateBill()' />";
                }
                else {
                    tr += "<input type='checkbox' id='pay" + result[row].CompanyPaymentId + "' onclick='CalculateBill()' />";
                }

                tr += "</td>";

                //tr += "<td style='width: 20%'>" + result[row].ModuleName + "</td>";
                tr += "<td style='width: 10%'>" + GetStringFromDateTime(result[row].PaymentDate) + "</td>";
                tr += "<td style='width: 35%'>" + result[row].BillNumber + "</td>";
                tr += "<td style='width: 25%'>" + result[row].DueAmount + "</td>";
                tr += "<td style='width: 10%'>" + result[row].UsdConversionRate + "</td>";
                tr += "<td style='width: 20%'>" + result[row].UsdBillAmount + "</td>";
                tr += "<td style='display:none;'>" + result[row].CompanyBillDetailsId + "</td>";
                tr += "<td style='display:none;'>" + result[row].CompanyPaymentId + "</td>";
                tr += "<td style='display:none;'>" + result[row].BillId + "</td>";
                tr += "<td style='display:none;'>" + result[row].ModuleName + "</td>";

                tr += "</tr>";

                $("#BillInfo tbody").append(tr);
                tr = "";
            }

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnCompanyBillFailed(error) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        function GenerateCompanyBill() {

            var companyBillId = $("#ContentPlaceHolder1_hfCompanyBillId").val();
            var billCurrencyId = $("#ContentPlaceHolder1_ddlCurrency").val();

            if ($("#ContentPlaceHolder1_ddlCompany").val() == "0") {
                toastr.warning("Please Select Company.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtPaymentDate").val() == "") {
                toastr.warning("Please Give Bill Generation Date.");
                return false;
            }
            else if ($("#BillInfo tbody tr").find("td:eq(0)").find("input").is(":checked") == false) {
                toastr.warning("Please Select Company Bill To Generate.");
                return false;
            }

            var BillGenerationDetails = new Array();
            var BillGenerationDetailsEdited = new Array();
            var BillGenerationDetailsDeleted = new Array();

            CommonHelper.SpinnerOpen();

            var CompanyBillGeneration = {
                CompanyBillId: companyBillId,
                CompanyId: $("#ContentPlaceHolder1_ddlCompany").val(),
                BillDate: CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtPaymentDate").val(), '/'),
                BillCurrencyId: billCurrencyId,
                Remarks: $("#ContentPlaceHolder1_txtRemarks").val()
            };

            var detailsId = 0;

            $("#BillInfo tbody tr").each(function () {
                detailsId = parseInt($(this).find("td:eq(6)").text(), 10);
                if ($(this).find("td:eq(0)").find("input").is(":checked") == true && detailsId == 0) {
                    BillGenerationDetails.push({
                        CompanyBillDetailsId: detailsId,
                        CompanyBillId: companyBillId,
                        CompanyPaymentId: $(this).find("td:eq(7)").text(),
                        BillId: $(this).find("td:eq(8)").text(),
                        Amount: $(this).find("td:eq(3)").text(),
                        ModuleName: $(this).find("td:eq(9)").text()
                    });
                }
                else if ($(this).find("td:eq(0)").find("input").is(":checked") == true && detailsId > 0) {
                    BillGenerationDetailsEdited.push({
                        CompanyBillDetailsId: detailsId,
                        CompanyBillId: companyBillId,
                        CompanyPaymentId: $(this).find("td:eq(7)").text(),
                        BillId: $(this).find("td:eq(8)").text(),
                        Amount: $(this).find("td:eq(3)").text(),
                        ModuleName: $(this).find("td:eq(9)").text()
                    });
                }
                else if ($(this).find("td:eq(0)").find("input").is(":checked") == false && detailsId > 0) {
                    BillGenerationDetailsDeleted.push({
                        CompanyBillDetailsId: detailsId,
                        CompanyBillId: companyBillId,
                        CompanyPaymentId: $(this).find("td:eq(7)").text(),
                        BillId: $(this).find("td:eq(8)").text(),
                        Amount: $(this).find("td:eq(3)").text()
                    });
                }
            });

            PageMethods.GenerateCompanyBill(CompanyBillGeneration, BillGenerationDetails, BillGenerationDetailsEdited, BillGenerationDetailsDeleted, OnGenerateCompanyBillSucceeded, OnGenerateCompanyBillFailed);
        }

        function OnGenerateCompanyBillSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);

                companyId = $("#ContentPlaceHolder1_ddlCompany").val();
                PerformClearAction();

                var url = "/HotelManagement/Reports/frmReportCompanyPaymentInvoice.aspx?CId=" + companyId + "&cbi=" + result.Pk;
                var popup_window = "Company Invoice";
                window.open(url, popup_window, "width=800,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            CommonHelper.SpinnerClose();
        }
        function OnGenerateCompanyBillFailed(error) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        function SearchPayment() {

            var dateFrom = null, dateTo = null, companyId;

            if ($("#ContentPlaceHolder1_txtFromDate").val() != "")
                dateFrom = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtFromDate").val(), '/');

            if ($("#ContentPlaceHolder1_txtToDate").val() != "")
                dateTo = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtToDate").val(), '/');

            companyId = $("#ContentPlaceHolder1_ddlCompanyForSearch").val();

            $("#BillInfoSearch tbody").html("");
            PageMethods.GetCompanyBillGenerationBySearch(dateFrom, dateTo, companyId, OnSearchPaymentSucceeded, OnSearchPaymentFailed);

            return false;
        }

        function OnSearchPaymentSucceeded(result) {
            $("#BillInfoSearch tbody").html("");
            var row = 0, tr = "";
            var isUpdatepermission = false, isDeletePermission = false;

            if ($("#ContentPlaceHolder1_hfIsUpdatePermission").val() == "1")
                isUpdatepermission = true;
            if ($("#ContentPlaceHolder1_hfIsDeletePermission").val() == "1")
                isDeletePermission = true;

            for (row = 0; row < result.length; row++) {

                if ((row + 1) % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width: 15%'>" + result[row].CompanyBillNumber + "</td>";
                tr += "<td style='width: 10%'>" + GetStringFromDateTime(result[row].BillDate) + "</td>";
                tr += "<td style='width: 10%'>" + result[row].CurrencyName + "</td>";
                tr += "<td style='width: 25%'>" + result[row].CompanyName + "</td>";
                tr += "<td style='width: 30%'>" + result[row].Remarks + "</td>";

                if (result[row].ApprovedStatus == null) {
                    tr += "<td style='width:10%;'>";
                    tr += "<a href='javascript:void();' onclick= 'javascript:return ShowCompanyBill(" + result[row].CompanyId + "," + result[row].CompanyBillId + "," + result[row].BillCurrencyId + ")' ><img alt='bill' title='Generate Bill' src='../Images/ReportDocument.png' /></a>";
                    tr += "&nbsp;&nbsp;";
                    if (isUpdatepermission) {
                        tr += "<a onclick=\"javascript:return FIllForEdit(" + result[row].CompanyBillId + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                        tr += "&nbsp;&nbsp;";
                    }
                    if (isDeletePermission) {
                        tr += "<a href='javascript:void();' onclick= 'javascript:return DeleteCompanyBillGeneration(" + result[row].CompanyBillId + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                        tr += "</td>";
                    }

                }
                else {
                    tr += "<td style='width:10%;'>";
                    tr += "<a href='javascript:void();' onclick= 'javascript:return ShowCompanyBill(" + result[row].CompanyId + "," + result[row].CompanyBillId + "," + result[row].BillCurrencyId + ")' ><img alt='bill' title='Generate Bill' src='../Images/ReportDocument.png' /></a>";
                    tr += "</td>";
                }

                tr += "<td style=display:none;'>" + result[row].PaymentId + "</td>";
                tr += "</tr>";

                $("#BillInfoSearch tbody").append(tr);
                tr = "";
            }
        }
        function OnSearchPaymentFailed() { }

        function PerformClearAction() {
            $("#BillInfo tbody").html("");
            $("#ContentPlaceHolder1_ddlCompany").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtRemarks").val("");
            $("#ContentPlaceHolder1_hfCompanyBillId").val("0");
            $("#TotalAmount").text("");
            $("#TotalAmountUsd").text("");
            $('#ContentPlaceHolder1_txtPaymentDate').datepicker().datepicker("setDate", DayOpenDate);
            if ($("#ContentPlaceHolder1_hfIsSavePermission").val() == "0")
                $("#btnGenerate").hide();
        }

        function CalculateBill() {

            var totalPayment = 0.00, totalAmountUsd = 0.00;

            $("#BillInfo tbody tr").each(function () {

                if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {
                    totalPayment += parseFloat($(this).find("td:eq(3)").text());
                    totalAmountUsd += parseFloat($(this).find("td:eq(5)").text());
                }
            });

            $("#TotalAmount").text(toFixed((totalPayment), 2));
            $("#TotalAmountUsd").text(toFixed((totalAmountUsd), 2));
        }

        function FIllForEdit(companyBillId) {
            $("#BillInfo tbody").html("");
            PageMethods.FillForm(companyBillId, OnFillFormSucceed, OnFillFormFailed);
            return false;
        }
        function OnFillFormSucceed(result) {

            $("#ContentPlaceHolder1_hfCompanyBillId").val(result.BillGeneration.CompanyBillId);
            $("#ContentPlaceHolder1_ddlCompany").val(result.BillGeneration.CompanyId + "").trigger('change');
            $("#ContentPlaceHolder1_txtRemarks").val(result.BillGeneration.Remarks);
            $("#ContentPlaceHolder1_txtPaymentDate").val(GetStringFromDateTime(result.BillGeneration.BillDate));
            $("#ContentPlaceHolder1_ddlCurrency").val(result.BillGeneration.BillCurrencyId);

            //------------ Details

            var row = 0, tr = "", chk = "checked='checked'", isChecked = 0;
            var totalPaymentAmount = 0.00, totalAmountUsd = 0.00;

            for (row = 0; row < result.CompanyBill.length; row++) {

                var pd = _.findWhere(result.BillGenerationDetails, { CompanyBillId: result.CompanyBill[row].BillGenerationId });

                if (pd != null) {
                    isChecked = "1";
                }

                if ((row + 1) % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width: 10%'> ";

                if (isChecked == "1") {
                    tr += "<input type='checkbox' id='pay" + result.CompanyBill[row].CompanyBillDetailsId + "'" + chk + " onclick='CalculateBill(this)' />";
                    totalPaymentAmount += result.CompanyBill[row].DueAmount;
                    totalAmountUsd += result.CompanyBill[row].UsdBillAmount;
                }
                else {
                    tr += "<input type='checkbox' id='pay" + result.CompanyBill[row].CompanyBillDetailsId + "' onclick='CalculateBill(this)' />";
                }

                tr += "</td>";

                tr += "<td style='width: 10%'>" + GetStringFromDateTime(result.CompanyBill[row].PaymentDate) + "</td>";
                tr += "<td style='width: 35%'>" + result.CompanyBill[row].BillNumber + "</td>";
                tr += "<td style='width: 25%'>" + result.CompanyBill[row].DueAmount + "</td>";
                tr += "<td style='width: 10%'>" + result.CompanyBill[row].UsdConversionRate + "</td>";
                tr += "<td style='width: 20%'>" + result.CompanyBill[row].UsdBillAmount + "</td>";

                if (pd != null)
                    tr += "<td style='display:none;'>" + pd.CompanyBillDetailsId + "</td>";
                else
                    tr += "<td style='display:none;'>0</td>";

                tr += "<td style='display:none;'>" + result.CompanyBill[row].CompanyPaymentId + "</td>";
                tr += "<td style='display:none;'>" + result.CompanyBill[row].BillId + "</td>";

                if (pd != null)
                    tr += "<td style='display:none;'>" + pd.ModuleName + "</td>";
                else
                    tr += "<td style='display:none;'>" + result.CompanyBill[row].ModuleName + "</td>";

                tr += "</tr>";

                $("#BillInfo tbody").append(tr);
                tr = "";
                isChecked = "0";
            }

            $("#TotalAmount").text(totalPaymentAmount);
            $("#TotalAmountUsd").text(toFixed((totalAmountUsd), 2));
            CommonHelper.ApplyDecimalValidation();
            if ($("#ContentPlaceHolder1_hfIsUpdatePermission").val() == "1")
                $("#btnGenerate").val("Update").show();
            else
                $("#btnGenerate").hide();

            CalculateBill();
            $("#myTabs").tabs({ active: 0 });
        }
        function OnFillFormFailed(error) {
            toastr.error(error.get_message());
        }

        function ShowCompanyBill(companyId, companyBillId, billCurrencyId) {

            var url = "/HotelManagement/Reports/frmReportCompanyPaymentInvoice.aspx?CId=" + companyId + "&cbi=" + companyBillId + "&crnid=" + billCurrencyId;
            var popup_window = "Company Invoice";
            window.open(url, popup_window, "width=800,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
        }

        function DeleteCompanyBillGeneration(companyBillId) {
            PageMethods.DeleteCompanyBillGeneration(companyBillId, OnDeleteCompanyBillSucceeded, OnDeleteCompanyBillFailed);
        }

        function OnDeleteCompanyBillSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchPayment();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnDeleteCompanyBillFailed(result) {

        }

    </script>
    <asp:HiddenField ID="hfCompanyBillId" Value="0" runat="server" />
    <asp:HiddenField ID="hfIsSavePermission" runat="server" />
    <asp:HiddenField ID="hfIsUpdatePermission" runat="server" />
    <asp:HiddenField ID="hfIsDeletePermission" runat="server" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Company Bill</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Company Bill</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Company Bill Generation
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPaymentDate" runat="server" class="control-label" Text="Company"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <input type="button" class="btn btn-primary" value="Search" onclick="SearchCompanyBill()" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <table id="BillInfo" class="table table-bordered table-condensed table-hover table-responsive">
                                    <thead>
                                        <tr>
                                            <th style="width: 10%;">Select</th>
                                            <%--<th style="width: 20%;">Payment Mode</th>--%>
                                            <th style="width: 10%;">Bill Date</th>
                                            <th style="width: 35%;">Bill Number</th>
                                            <th style="width: 25%;">Amount</th>
                                            <th style="width: 10%;">C.Rate</th>
                                            <th style="width: 20%;">USD Amount</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-5 text-right" style="font-weight: bold;">Total Amount:&nbsp;&nbsp;</div>
                            <div class="col-md-2" style="text-align: right;">
                                <label id="TotalAmount">0.00</label>
                            </div>
                            <div class="col-md-2"></div>
                            <div class="col-md-2" style="text-align: right;">
                                <div class="col-md-5 text-right" style="font-weight: bold;">USD: &nbsp;&nbsp;</div>
                                <label id="TotalAmountUsd">0.00</label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Bill Generate Date"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPaymentDate" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" CssClass="form-control" TextMode="MultiLine" runat="server"
                                    TabIndex="11"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCurrency" runat="server" class="control-label required-field" Text="Bill Currency"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:DropDownList ID="ddlCurrency" TabIndex="6" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-7">
                                <input id="btnGenerate" type="button" class="btn btn-primary" value="Generate Bill" onclick="GenerateCompanyBill()" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Bill Generation Search
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Company"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCompanyForSearch" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
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
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return SearchPayment()" />
                            </div>
                        </div>
                    </div>
                </div>
                <div id="SearchResult" class="panel panel-default">
                    <div class="panel-heading">
                        Search Information
                    </div>
                    <div class="panel-body">
                        <table id="BillInfoSearch" class="table table-bordered table-condensed table-hover table-responsive">
                            <thead>
                                <tr>
                                    <th style="width: 15%;">Ledger Number</th>
                                    <th style="width: 10%;">Payment Date</th>
                                    <th style="width: 10%;">Bill Currency</th>
                                    <th style="width: 25%;">Company Name</th>
                                    <th style="width: 30%;">Remarks</th>
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
    </div>




</asp:Content>
