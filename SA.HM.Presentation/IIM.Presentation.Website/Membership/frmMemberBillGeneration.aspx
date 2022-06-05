<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmMemberBillGeneration.aspx.cs" Inherits="HotelManagement.Presentation.Website.Membership.frmMemberBillGeneration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#myTabs").tabs();

            $("#ContentPlaceHolder1_ddlMember").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlMemberForSearch").select2({
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

        function SearchMemberBill() {
            var memberId = $("#ContentPlaceHolder1_ddlMember").val();
            $("#TotalAmount").text("0.00");
            $("#BillInfo tbody").html("");

            if (memberId == "0") {
                toastr.warning("Please Select Member.");
                return false;
            }

            CommonHelper.SpinnerOpen();
            $("#BillInfo tbody").html("");
            PageMethods.MemberBillBySearch(memberId, OnLoadMemberBillSucceeded, OnMemberBillFailed);
        }

        function OnLoadMemberBillSucceeded(result) {

            var row = 0, tr = "", chk = "checked='checked'", isChecked = false;

            for (row = 0; row < result.length; row++) {

                isChecked = result[row].MemberBillDetailsId > 0 ? true : false;

                if ((row + 1) % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width: 10%'> ";

                if (isChecked) {
                    tr += "<input type='checkbox' id='pay" + result[row].MemberPaymentId + "'" + chk + " onclick='CalculateBill()' />";
                }
                else {
                    tr += "<input type='checkbox' id='pay" + result[row].MemberPaymentId + "' onclick='CalculateBill()' />";
                }

                tr += "</td>";

                //tr += "<td style='width: 20%'>" + result[row].ModuleName + "</td>";
                tr += "<td style='width: 20%'>" + GetStringFromDateTime(result[row].PaymentDate) + "</td>";
                tr += "<td style='width: 25%'>" + result[row].BillNumber + "</td>";
                tr += "<td style='width: 25%'>" + result[row].DueAmount + "</td>";
                tr += "<td style='width: 10%'>" + result[row].UsdConversionRate + "</td>";
                tr += "<td style='width: 20%'>" + result[row].UsdBillAmount + "</td>";
                tr += "<td style=display:none;'>" + result[row].MemberBillDetailsId + "</td>";
                tr += "<td style=display:none;'>" + result[row].MemberPaymentId + "</td>";
                tr += "<td style=display:none;'>" + result[row].BillId + "</td>";

                tr += "</tr>";

                $("#BillInfo tbody").append(tr);
                tr = "";
            }

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnMemberBillFailed(error) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        function GenerateMemberBill() {

            var MemberBillId = $("#ContentPlaceHolder1_hfMemberBillId").val();
            var billCurrencyId = $("#ContentPlaceHolder1_ddlCurrency").val();
            if ($("#ContentPlaceHolder1_ddlMember").val() == "0") {
                toastr.warning("Please Select Member.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtPaymentDate").val() == "") {
                toastr.warning("Please Give Bill Generation Date.");
                return false;
            }
            else if ($("#BillInfo tbody tr").find("td:eq(0)").find("input").is(":checked") == false) {
                toastr.warning("Please Select Member Bill To Generate.");
                return false;
            }

            var BillGenerationDetails = new Array();
            var BillGenerationDetailsEdited = new Array();
            var BillGenerationDetailsDeleted = new Array();

            CommonHelper.SpinnerOpen();

            var MemberBillGeneration = {
                MemberBillId: MemberBillId,
                MemberId: $("#ContentPlaceHolder1_ddlMember").val(),
                BillDate: CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtPaymentDate").val(), '/'),
                BillCurrencyId: billCurrencyId,
                Remarks: $("#ContentPlaceHolder1_txtRemarks").val()
            };
            var detailsId = 0;
            debugger;
            $("#BillInfo tbody tr").each(function () {

                detailsId = parseInt($(this).find("td:eq(6)").text(), 10);

                if ($(this).find("td:eq(0)").find("input").is(":checked") == true && detailsId == 0) {
                    BillGenerationDetails.push({
                        MemberBillDetailsId: detailsId,
                        MemberBillId: MemberBillId,
                        MemberPaymentId: $(this).find("td:eq(7)").text(),
                        BillId: $(this).find("td:eq(8)").text(),
                        Amount: $(this).find("td:eq(3)").text()
                    });
                }
                else if ($(this).find("td:eq(0)").find("input").is(":checked") == true && detailsId > 0) {

                    BillGenerationDetailsEdited.push({
                        MemberBillDetailsId: detailsId,
                        MemberBillId: MemberBillId,
                        MemberPaymentId: $(this).find("td:eq(7)").text(),
                        BillId: $(this).find("td:eq(8)").text(),
                        Amount: $(this).find("td:eq(3)").text()
                    });
                }
                else if ($(this).find("td:eq(0)").find("input").is(":checked") == false && detailsId > 0) {

                    BillGenerationDetailsDeleted.push({
                        MemberBillDetailsId: detailsId,
                        MemberBillId: MemberBillId,
                        MemberPaymentId: $(this).find("td:eq(7)").text(),
                        BillId: $(this).find("td:eq(8)").text(),
                        Amount: $(this).find("td:eq(3)").text()
                    });
                }
            });

            PageMethods.GenerateMemberBill(MemberBillGeneration, BillGenerationDetails, BillGenerationDetailsEdited, BillGenerationDetailsDeleted, OnGenerateMemberBillSucceeded, OnGenerateMemberBillFailed);
        }

        function OnGenerateMemberBillSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);

                MemberId = $("#ContentPlaceHolder1_ddlMember").val();
                PerformClearAction();

                var url = "/Membership/Reports/frmReportMemberPaymentInvoice.aspx?CId=" + MemberId + "&cbi=" + result.Pk;
                var popup_window = "Member Invoice";
                window.open(url, popup_window, "width=800,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");

            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            CommonHelper.SpinnerClose();
        }
        function OnGenerateMemberBillFailed(error) {
            toastr.info(error.get_message());
            CommonHelper.SpinnerClose();
            return false;
        }

        function SearchPayment() {

            var dateFrom = null, dateTo = null, MemberId;

            if ($("#ContentPlaceHolder1_txtFromDate").val() != "")
                dateFrom = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtFromDate").val(), '/');

            if ($("#ContentPlaceHolder1_txtToDate").val() != "")
                dateTo = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtToDate").val(), '/');

            MemberId = $("#ContentPlaceHolder1_ddlMemberForSearch").val();

            $("#BillInfoSearch tbody").html("");
            PageMethods.GetMemberBillGenerationBySearch(dateFrom, dateTo, MemberId, OnSearchPaymentSucceeded, OnSearchPaymentFailed);

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

                tr += "<td style='width: 15%'>" + result[row].MemberBillNumber + "</td>";
                tr += "<td style='width: 10%'>" + GetStringFromDateTime(result[row].BillDate) + "</td>";
                tr += "<td style='width: 30%'>" + result[row].MemberName + "</td>";
                tr += "<td style='width: 35%'>" + result[row].Remarks + "</td>";

                if (result[row].ApprovedStatus == null) {
                    tr += "<td style='width:10%;'>";
                    tr += "<a href='javascript:void();' onclick= 'javascript:return ShowMemberBill(" + result[row].MemberId + "," + result[row].MemberBillId + ")' ><img alt='bill' title='Generate Bill' src='../Images/ReportDocument.png' /></a>";
                    tr += "&nbsp;&nbsp;";
                    tr += "<a onclick=\"javascript:return FIllForEdit(" + result[row].MemberBillId + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                    tr += "&nbsp;&nbsp;";
                    tr += "<a href='javascript:void();' onclick= 'javascript:return DeleteMemberBillGeneration(" + result[row].MemberBillId + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                    tr += "</td>";
                }
                else {
                    tr += "<td style='width:10%;'>";
                    tr += "<a href='javascript:void();' onclick= 'javascript:return ShowMemberBill(" + result[row].MemberId + "," + result[row].MemberBillId + ")' ><img alt='bill' title='Generate Bill' src='../Images/ReportDocument.png' /></a>";
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
            $("#ContentPlaceHolder1_ddlMember").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtRemarks").val("");
            $("#TotalAmount").text("");
            $('#ContentPlaceHolder1_txtPaymentDate').datepicker().datepicker("setDate", DayOpenDate);
        }

        function CalculateBill() {

            var totalPayment = 0.00;

            $("#BillInfo tbody tr").each(function () {

                if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {
                    totalPayment += parseFloat($(this).find("td:eq(3)").text());
                }
            });

            $("#TotalAmount").text(toFixed((totalPayment), 2));
        }

        function FIllForEdit(MemberBillId) {
            $("#BillInfo tbody").html("");
            PageMethods.FillForm(MemberBillId, OnFillFormSucceed, OnFillFormFailed);
            return false;
        }
        function OnFillFormSucceed(result) {

            $("#ContentPlaceHolder1_hfMemberBillId").val(result.BillGeneration.MemberBillId);
            $("#ContentPlaceHolder1_ddlMember").val(result.BillGeneration.MemberId + "").trigger('change');
            $("#ContentPlaceHolder1_txtRemarks").val(result.BillGeneration.Remarks);
            $("#ContentPlaceHolder1_txtPaymentDate").val(GetStringFromDateTime(result.BillGeneration.BillDate));
            $("#ContentPlaceHolder1_ddlCurrency").val(result.BillGeneration.BillCurrencyId);
            //------------ Details

            var row = 0, tr = "", chk = "checked='checked'", isChecked = 0;
            var totalPaymentAmount = 0.00;

            for (row = 0; row < result.MemberBill.length; row++) {

                var pd = _.findWhere(result.BillGenerationDetails, { MemberBillId: result.MemberBill[row].BillGenerationId });

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
                    tr += "<input type='checkbox' id='pay" + result.MemberBill[row].MemberBillDetailsId + "'" + chk + " onclick='CalculateBill(this)' />";
                    totalPaymentAmount += result.MemberBill[row].DueAmount;
                }
                else {
                    tr += "<input type='checkbox' id='pay" + result.MemberBill[row].MemberBillDetailsId + "' onclick='CalculateBill(this)' />";
                }

                tr += "</td>";

                tr += "<td style='width: 20%'>" + GetStringFromDateTime(result.MemberBill[row].PaymentDate) + "</td>";
                tr += "<td style='width: 25%'>" + result.MemberBill[row].BillNumber + "</td>";
                tr += "<td style='width: 25%'>" + result.MemberBill[row].DueAmount + "</td>";
                tr += "<td style='width: 10%'>" + result.MemberBill[row].UsdConversionRate + "</td>";
                tr += "<td style='width: 20%'>" + result.MemberBill[row].UsdBillAmount + "</td>";

                if (pd != null)
                    tr += "<td style=display:none;'>" + pd.MemberBillDetailsId + "</td>";
                else
                    tr += "<td style=display:none;'>0</td>";

                tr += "<td style=display:none;'>" + result.MemberBill[row].MemberPaymentId + "</td>";
                tr += "<td style=display:none;'>" + result.MemberBill[row].BillId + "</td>";

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

        function ShowMemberBill(MemberId, MemberBillId) {

            var url = "/Membership/Reports/frmReportMemberPaymentInvoice.aspx?CId=" + MemberId + "&cbi=" + MemberBillId;
            var popup_window = "Member Invoice";
            window.open(url, popup_window, "width=800,height=780,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
        }

        function DeleteMemberBillGeneration(MemberBillId) {
            if (confirm("Do you want to delete this record ?")) {
                PageMethods.DeleteMemberBillGeneration(MemberBillId, OnDeleteMemberBillSucceeded, OnDeleteMemberBillFailed);
                return false;
            }
            
        }

        function OnDeleteMemberBillSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchPayment();
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            return false;
        }
        function OnDeleteMemberBillFailed(result) {

        }

    </script>
    <asp:HiddenField ID="hfMemberBillId" Value="0" runat="server" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Member Bill</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Member Bill</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Member Bill Generation
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblPaymentDate" runat="server" class="control-label" Text="Member"></asp:Label>
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlMember" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <input type="button" class="btn btn-primary" value="Search" onclick="SearchMemberBill()" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <table id="BillInfo" class="table table-bordered table-condensed table-hover table-responsive">
                                    <thead>
                                        <tr>
                                            <th style="width: 10%;">Select</th>
                                            <%--<th style="width: 20%;">Payment Mode</th>--%>
                                            <th style="width: 20%;">Bill Date</th>
                                            <th style="width: 25%;">Bill Number</th>
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
                            <div class="col-md-9 text-right" style="font-weight: bold;">Total Amount:&nbsp;&nbsp;</div>
                            <div class="col-md-3">
                                <label id="TotalAmount">0.00</label>
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
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCurrency" TabIndex="6" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <input type="button" class="btn btn-primary" value="Generate Bill" onclick="GenerateMemberBill()" />
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
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Member"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlMemberForSearch" runat="server" CssClass="form-control">
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
                                    <th style="width: 30%;">Member Name</th>
                                    <th style="width: 35%;">Remarks</th>
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
