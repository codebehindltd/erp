<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmployeeBillGeneration.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmployeeBillGeneration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#myTabs").tabs();

            $("#ContentPlaceHolder1_ddlEmployee").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlEmployeeForSearch").select2({
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

        });

        function SearchEmployeeBill() {
            var employeeId = $("#ContentPlaceHolder1_ddlEmployee").val();
            $("#BillInfo tbody").html("");

            if (employeeId == "0") {
                toastr.warning("Please Select Employee.");
                return false;
            }

            CommonHelper.SpinnerOpen();
            $("#BillInfo tbody").html("");
            PageMethods.EmployeeBillBySearch(employeeId, OnLoadEmployeeBillSucceeded, OnEmployeeBillFailed);
        }

        function OnLoadEmployeeBillSucceeded(result) {

            var row = 0, tr = "", chk = "checked='checked'", isChecked = 0;

            for (row = 0; row < result.length; row++) {

                isChecked = result[row].EmployeeBillDetailsId > 0 ? "1" : "0";

                if ((row + 1) % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width: 10%'> ";

                if (isChecked == "1") {
                    tr += "<input type='checkbox' id='pay" + result[row].EmployeePaymentId + "'" + chk + " onclick='CalculateBill()' />";
                }
                else {
                    tr += "<input type='checkbox' id='pay" + result[row].EmployeePaymentId + "' onclick='CalculateBill()' />";
                }

                tr += "</td>";

                //tr += "<td style='width: 20%'>" + result[row].ModuleName + "</td>";
                tr += "<td style='width: 10%'>" + GetStringFromDateTime(result[row].PaymentDate) + "</td>";
                tr += "<td style='width: 25%'>" + result[row].BillNumber + "</td>";
                tr += "<td style='width: 20%'>" + result[row].DueAmount + "</td>";
                tr += "<td style='width: 10%'>" + result[row].UsdConversionRate + "</td>";
                tr += "<td style='width: 25%'>" + result[row].UsdBillAmount + "</td>";
                tr += "<td style='display:none;'>" + result[row].EmployeeBillDetailsId + "</td>";
                tr += "<td style='display:none;'>" + result[row].EmployeePaymentId + "</td>";
                tr += "<td style='display:none;'>" + result[row].BillId + "</td>";

                tr += "</tr>";

                $("#BillInfo tbody").append(tr);
                tr = "";
            }

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnEmployeeBillFailed(error) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        function GenerateEmployeeBill() {

            var employeeBillId = $("#ContentPlaceHolder1_hfEmployeeBillId").val();
            var billCurrencyId = $("#ContentPlaceHolder1_ddlCurrency").val();

            if ($("#ContentPlaceHolder1_ddlEmployee").val() == "0") {
                toastr.warning("Please Select Employee.");
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

            var EmployeeBillGeneration = {
                EmployeeBillId: employeeBillId,
                EmployeeId: $("#ContentPlaceHolder1_ddlEmployee").val(),
                BillDate: CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtPaymentDate").val(), '/'),
                BillCurrencyId: billCurrencyId,
                Remarks: $("#ContentPlaceHolder1_txtRemarks").val()
            };
            
            var detailsId = 0;

            $("#BillInfo tbody tr").each(function () {

                detailsId = parseInt($(this).find("td:eq(6)").text(), 10);

                if ($(this).find("td:eq(0)").find("input").is(":checked") == true && detailsId == 0) {
                    BillGenerationDetails.push({
                        EmployeeBillDetailsId: detailsId,
                        EmployeeBillId: employeeBillId,
                        EmployeePaymentId: $(this).find("td:eq(7)").text(),
                        BillId: $(this).find("td:eq(8)").text(),
                        Amount: $(this).find("td:eq(3)").text()
                    });
                }
                else if ($(this).find("td:eq(0)").find("input").is(":checked") == true && detailsId > 0) {

                    BillGenerationDetailsEdited.push({
                        EmployeeBillDetailsId: detailsId,
                        EmployeeBillId: employeeBillId,
                        EmployeePaymentId: $(this).find("td:eq(7)").text(),
                        BillId: $(this).find("td:eq(8)").text(),
                        Amount: $(this).find("td:eq(3)").text()
                    });
                }
                else if ($(this).find("td:eq(0)").find("input").is(":checked") == false && detailsId > 0) {

                    BillGenerationDetailsDeleted.push({
                        EmployeeBillDetailsId: detailsId,
                        EmployeeBillId: employeeBillId,
                        EmployeePaymentId: $(this).find("td:eq(7)").text(),
                        BillId: $(this).find("td:eq(8)").text(),
                        Amount: $(this).find("td:eq(3)").text()
                    });
                }
            });

            PageMethods.GenerateEmployeeBill(EmployeeBillGeneration, BillGenerationDetails, BillGenerationDetailsEdited, BillGenerationDetailsDeleted, OnGenerateCompanyBillSucceeded, OnGenerateCompanyBillFailed);
        }

        function OnGenerateCompanyBillSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);

                var employeeId = $("#ContentPlaceHolder1_ddlEmployee").val();
                PerformClearAction();

                var url = "/Payroll/Reports/frmReportEmployeePaymentInvoice.aspx?eid=" + employeeId + "&ebi=" + result.Pk;
                var popup_window = "Employee Invoice";
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

            var dateFrom = null, dateTo = null, employeeId;

            if ($("#ContentPlaceHolder1_txtFromDate").val() != "")
                dateFrom = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtFromDate").val(), '/');

            if ($("#ContentPlaceHolder1_txtToDate").val() != "")
                dateTo = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtToDate").val(), '/');

            employeeId = $("#ContentPlaceHolder1_ddlEmployeeForSearch").val();

            $("#BillInfoSearch tbody").html("");
            PageMethods.GetEmployeeBillGenerationBySearch(dateFrom, dateTo, employeeId, OnSearchPaymentSucceeded, OnSearchPaymentFailed);

            return false;
        }

        function OnSearchPaymentSucceeded(result) {
            $("#BillInfoSearch tbody").html("");
            var row = 0, tr = "";

            for (row = 0; row < result.length; row++) {

                if ((row + 1) % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width: 15%'>" + result[row].EmployeeBillNumber + "</td>";
                tr += "<td style='width: 10%'>" + GetStringFromDateTime(result[row].BillDate) + "</td>";
                tr += "<td style='width: 10%'>" + result[row].CurrencyName + "</td>";
                tr += "<td style='width: 25%'>" + result[row].EmployeeName + "</td>";
                tr += "<td style='width: 30%'>" + result[row].Remarks + "</td>";

                if (result[row].ApprovedStatus == null) {
                    tr += "<td style='width:10%;'>";
                    tr += "<a href='javascript:void();' onclick= 'javascript:return ShowEmployeeBill(" + result[row].EmployeeId + "," + result[row].EmployeeBillId + ")' ><img alt='bill' title='Generate Bill' src='../Images/ReportDocument.png' /></a>";
                    tr += "&nbsp;&nbsp;";
                    tr += "<a onclick=\"javascript:return FIllForEdit(" + result[row].EmployeeBillId + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                    tr += "&nbsp;&nbsp;";
                    tr += "<a href='javascript:void();' onclick= 'javascript:return DeleteEmployeeBillGeneration(" + result[row].EmployeeBillId + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                    tr += "</td>";
                }
                else {
                    tr += "<td style='width:10%;'>";
                    tr += "</td>";
                }

                tr += "<td style=display:none;'>" + result[row].EmployeeBillId + "</td>";
                tr += "</tr>";

                $("#BillInfoSearch tbody").append(tr);
                tr = "";
            }
        }
        function OnSearchPaymentFailed() { }

        function PerformClearAction() {
            $("#BillInfo tbody").html("");
            $("#ContentPlaceHolder1_ddlEmployee").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtRemarks").val("");
            $("#ContentPlaceHolder1_hfEmployeeBillId").val("0");
            $("#TotalAmount").text("");
            $("#TotalAmountUsd").text("");
            $('#ContentPlaceHolder1_txtPaymentDate').datepicker().datepicker("setDate", DayOpenDate);
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

        function FIllForEdit(employeeBillId) {
            $("#BillInfo tbody").html("");
            PageMethods.FillForm(employeeBillId, OnFillFormSucceed, OnFillFormFailed);
            return false;
        }
        function OnFillFormSucceed(result) {

            $("#ContentPlaceHolder1_hfEmployeeBillId").val(result.BillGeneration.EmployeeBillId);
            $("#ContentPlaceHolder1_ddlEmployee").val(result.BillGeneration.EmployeeId + "").trigger('change');
            $("#ContentPlaceHolder1_txtRemarks").val(result.BillGeneration.Remarks);
            $("#ContentPlaceHolder1_txtPaymentDate").val(GetStringFromDateTime(result.BillGeneration.BillDate));
            $("#ContentPlaceHolder1_ddlCurrency").val(result.BillGeneration.BillCurrencyId);

            //------------ Details

            var row = 0, tr = "", chk = "checked='checked'", isChecked = 0;
            var totalPaymentAmount = 0.00;

            for (row = 0; row < result.EmployeeBill.length; row++) {

                var pd = _.findWhere(result.BillGenerationDetails, { EmployeeBillId: result.EmployeeBill[row].BillGenerationId });

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
                    tr += "<input type='checkbox' id='pay" + result.EmployeeBill[row].EmployeeBillDetailsId + "'" + chk + " onclick='CalculateBill(this)' />";
                    totalPaymentAmount += result.EmployeeBill[row].DueAmount;
                }
                else {
                    tr += "<input type='checkbox' id='pay" + result.EmployeeBill[row].EmployeeBillDetailsId + "' onclick='CalculateBill(this)' />";
                }

                tr += "</td>";

                tr += "<td style='width: 10%'>" + GetStringFromDateTime(result.EmployeeBill[row].PaymentDate) + "</td>";
                tr += "<td style='width: 25%'>" + result.EmployeeBill[row].BillNumber + "</td>";
                tr += "<td style='width: 20%'>" + result.EmployeeBill[row].DueAmount + "</td>";
                tr += "<td style='width: 10%'>" + result.EmployeeBill[row].UsdConversionRate + "</td>";
                tr += "<td style='width: 25%'>" + result.EmployeeBill[row].UsdBillAmount + "</td>";

                if (pd != null)
                    tr += "<td style='display:none;'>" + pd.EmployeeBillDetailsId + "</td>";
                else
                    tr += "<td style='display:none;'>0</td>";

                tr += "<td style='display:none;'>" + result.EmployeeBill[row].EmployeePaymentId + "</td>";
                tr += "<td style='display:none;'>" + result.EmployeeBill[row].BillId + "</td>";

                tr += "</tr>";

                $("#BillInfo tbody").append(tr);
                tr = "";
                isChecked = "0";
            }

            $("#TotalAmount").text(totalPaymentAmount);
            CommonHelper.ApplyDecimalValidation();
            $("#ContentPlaceHolder1_btnSave").val("Update");

            CalculateBill();
            $("#myTabs").tabs({ active: 0 });
        }
        function OnFillFormFailed(error) {
            toastr.error(error.get_message());
        }

        function ShowEmployeeBill(employeeId, employeeBillId) {

            var url = "/Payroll/Reports/frmReportEmployeePaymentInvoice.aspx?eid=" + employeeId + "&ebi=" + employeeBillId;
            var popup_window = "Employee Invoice";
            window.open(url, popup_window, "width=800,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
        }

        function DeleteEmployeeBillGeneration(employeeBillId) {
            PageMethods.DeleteEmployeeBillGeneration(employeeBillId, OnDeleteCompanyBillSucceeded, OnDeleteCompanyBillFailed);
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
    <asp:HiddenField ID="hfEmployeeBillId" Value="0" runat="server" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Employee Bill</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Employee Bill</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Employee Bill Generation
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPaymentDate" runat="server" class="control-label" Text="Employee"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <input type="button" class="btn btn-primary" value="Search" onclick="SearchEmployeeBill()" />
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
                                            <th style="width: 25%;">Bill Number</th>
                                            <th style="width: 20%;">Amount</th>
                                            <th style="width: 10%;">C.Rate</th>
                                            <th style="width: 25%;">USD Amount</th>
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
                                <div class="col-md-5 text-right" style="font-weight: bold;">USD: &nbsp;&nbsp;</div><label id="TotalAmountUsd">0.00</label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Payment Date"></asp:Label>
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
                                <input type="button" class="btn btn-primary" value="Generate Bill" onclick="GenerateEmployeeBill()" />
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
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Employee"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlEmployeeForSearch" runat="server" CssClass="form-control">
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
