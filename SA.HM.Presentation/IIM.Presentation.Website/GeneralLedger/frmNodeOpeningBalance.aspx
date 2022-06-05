<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmNodeOpeningBalance.aspx.cs"
    Inherits="HotelManagement.Presentation.Website.GeneralLedger.frmNodeOpeningBalance" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">

        var NodeMatrix = new Array();
        var FilterData = new Array();
        var SelectedNode = {};
        var CPBPCRBRNode = new Array();
        var VoucherCpBpCrBr = new Array();
        var VoucherList = new Array();

        $(document).ready(function () {

            if ($("#ContentPlaceHolder1_hfNodeMatrix").val() != "") {
                NodeMatrix = JSON.parse($("#ContentPlaceHolder1_hfNodeMatrix").val());
                $("#ContentPlaceHolder1_hfNodeMatrix").val("");
            }

            $('#ContentPlaceHolder1_txtVoucherDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $('#ContentPlaceHolder1_txtChequeDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#ContentPlaceHolder1_ddlVoucherType").change(function () {
                LoadCpBpCrBrAccountHead();

                var CpBpCrBrAccountCaption = "", accountTransactionType = "";

                if ($(this).val() == "CP") {
                    $("#cpcrbpbrMainHead").show();
                    $("#ContentPlaceHolder1_ddlLedgerMode").val("Cr");
                    $("#txtDrAmount").attr("disabled", false);
                    $("#txtCrAmount").attr("disabled", true);

                    CpBpCrBrAccountCaption = $(this).val() + ' Account Head';
                }
                else if ($(this).val() == "BP") {
                    $("#cpcrbpbrMainHead").show();
                    $("#ContentPlaceHolder1_ddlLedgerMode").val("Cr");
                    $("#txtDrAmount").attr("disabled", false);
                    $("#txtCrAmount").attr("disabled", true);
                    CpBpCrBrAccountCaption = $(this).val() + ' Account Head';
                }
                else if ($(this).val() == "CR") {
                    $("#cpcrbpbrMainHead").show();
                    $("#ContentPlaceHolder1_ddlLedgerMode").val("Dr");
                    $("#txtDrAmount").attr("disabled", true);
                    $("#txtCrAmount").attr("disabled", false);
                    CpBpCrBrAccountCaption = $(this).val() + ' Account Head';
                }
                else if ($(this).val() == "BR") {
                    $("#cpcrbpbrMainHead").show();
                    $("#ContentPlaceHolder1_ddlLedgerMode").val("Dr");
                    $("#txtDrAmount").attr("disabled", true);
                    $("#txtCrAmount").attr("disabled", false);
                    CpBpCrBrAccountCaption = $(this).val() + ' Account Head';
                }
                else {
                    $("#cpcrbpbrMainHead").hide();
                    $("#ContentPlaceHolder1_ddlLedgerMode").val("");
                    CpBpCrBrAccountCaption = 'Account Head';
                    $("#txtDrAmount").attr("disabled", false);
                    $("#txtCrAmount").attr("disabled", false);
                }

                $("#lblCpBpCrBr").text(CpBpCrBrAccountCaption);
            });

            $('#txtAccountName').autocomplete({
                source: function (request, response) {

                    if ($("#ContentPlaceHolder1_ddlVoucherType").val() == "") {
                        toastr.info("Please Select Voucher Type.");
                        return false;
                    }

                    CommonHelper.SpinnerOpen();
                    //var v = _.filter(NodeMatrix, function (q) { return q.HeadWithCode.match(/sal/i) });  //Like Search By reqular expression

                    FilterData = _.filter(NodeMatrix, function (obj) {
                        return ~obj.HeadWithCode.toLowerCase().indexOf((request.term).toLowerCase()); // like search. ~ with indexOf means contains
                    });

                    var searchData = $.map(FilterData, function (obj) {
                        return {
                            label: obj.HeadWithCode,
                            value: obj.NodeId
                        };
                    });
                    response(searchData);
                    CommonHelper.SpinnerClose();
                },
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    $(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);

                    SelectedNode = _.findWhere(FilterData, { NodeId: parseInt(ui.item.value, 10) });                   
                }
            });

        });

        function LoadCpBpCrBrAccountHead() {
            var projectId = $("#ContentPlaceHolder1_ddlGLProject").val();
            var voucherType = $("#ContentPlaceHolder1_ddlVoucherType").val();

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/GeneralLedger/frmVoucherEntry.aspx/AccountHeadForCPCRBPBR",
                data: "{'projectId':'" + projectId + "', 'voucherType':'" + voucherType + "'}",
                dataType: "json",
                success: OnChangeAccountHeadPopulated,
                error: function (result) {
                }
            });
        }
        function OnChangeAccountHeadPopulated(response) {
            PopulateControlForChangeAccountHead(response.d, $("#ContentPlaceHolder1_ddlCpBpCrBr"));
        }

        function PopulateControlForChangeAccountHead(list, control) {

            CPBPCRBRNode = list;

            if (list.length > 0) {
                control.empty();

                if ($(control).is(':disabled'))
                    control.removeAttr("disabled");

                if (list.length != 1) {
                }
                $.each(list, function () {
                    control.append($("<option></option>").val(this['NodeId']).html(this['NodeHead']));
                });
            }
            else {
                control.empty().append('<option selected="selected" value="0">Not Available<option>');
                control.attr("disabled", true);
            }
        }


        function AddVoucherNodeDetails() {

            var tr = "", voucherType = "", amountDr = "", amountCr = "", nodeNarration = "";
            var amountOtherDr = "", amountOtherCr = "", chequeNumber = null, chequeDate = null;
            var currencyTypeId = "", nodeId = 0;

            voucherType = $("#ContentPlaceHolder1_ddlVoucherType").val();
            currencyTypeId = $("#ContentPlaceHolder1_ddlCurrency").val();

            amountDr = $("#VoucherGrid thead tr:eq(1)").find("td:eq(1)").find("input").val();
            amountCr = $("#VoucherGrid thead tr:eq(1)").find("td:eq(2)").find("input").val();
            nodeNarration = $("#VoucherGrid thead tr:eq(1)").find("td:eq(3)").find("input").val();

            if (voucherType == "BR" || voucherType == "BP") {
                chequeNumber = $("#ContentPlaceHolder1_txtChequeNumber").val();
                chequeDate = $("#ContentPlaceHolder1_txtChequeDate").val();
            }

            if ((voucherType == "CP" || voucherType == "BP" || voucherType == "CR" || voucherType == "BR") && VoucherList.length == 0) {

                nodeId = parseInt($("#ContentPlaceHolder1_ddlCpBpCrBr").val(), 10);
                var frstNode = _.findWhere(CPBPCRBRNode, { NodeId: nodeId });

                VoucherList.push({
                    LedgerId: '0',
                    DealId: '0',
                    NodeId: frstNode.NodeId,
                    ChequeNumber: chequeNumber,
                    ChequeDate: chequeDate,
                    DrAmount: '0',
                    CrAmount: '0',
                    NodeNarration: '',
                    CostCenterId: '0',
                    FieldId: currencyTypeId,
                    CurrencyAmount: '0',
                    NodeType: frstNode.NodeType,
                    Hierarchy: frstNode.Hierarchy,
                    ParentId: '',
                    ParentLedgerId: ''
                });

                tr += "<tr>";
                tr += "<td style='width:40%;'>" + frstNode.NodeHead + "</td>";
                tr += "<td class='text-right' style='width:10%;'>" + 0.00 + "</td>";
                tr += "<td class='text-right' style='width:10%;'>" + 0.00 + "</td>";
                tr += "<td style='width:32%;'>" + "" + "</td>";
                tr += "<td style='width:8%;'></td>";
                tr += "</tr>";

                $('#VoucherGrid tbody').append(tr);
            }

            tr = "";

            VoucherList.push({
                LedgerId: '0',
                DealId: '0',
                NodeId: SelectedNode.NodeId,
                ChequeNumber: '',
                ChequeDate: '',
                DrAmount: amountDr,
                CrAmount: amountCr,
                NodeNarration: nodeNarration,
                CostCenterId: '0',
                FieldId: currencyTypeId,
                CurrencyAmount: '',
                NodeType: SelectedNode.NodeType,
                Hierarchy: SelectedNode.Hierarchy,
                ParentId: '',
                ParentLedgerId: ''
            });

            tr += "<tr>";
            tr += "<td style='width:40%;'>" + SelectedNode.NodeHead + "</td>";
            tr += "<td class='text-right' style='width:10%;'>" + amountDr + "</td>";
            tr += "<td class='text-right' style='width:10%;'>" + amountCr + "</td>";
            tr += "<td style='width:32%;'>" + nodeNarration + "</td>";
            tr += "<td style='width:8%;'></td>";
            tr += "</tr>";

            if ($('#VoucherGrid tbody tr').length > 0)
                $('#VoucherGrid tbody tr:first').after(tr);
            else
                $('#VoucherGrid tbody').append(tr);
            
            $("#VoucherGrid thead tr:eq(1)").find("td:eq(0)").find("input").val("");
            $("#VoucherGrid thead tr:eq(1)").find("td:eq(1)").find("input").val("");
            $("#VoucherGrid thead tr:eq(1)").find("td:eq(2)").find("input").val("");
            $("#VoucherGrid thead tr:eq(1)").find("td:eq(3)").find("input").val("");

            $("#VoucherGrid thead tr:eq(1)").find("td:eq(0)").find("input").focus();

            CalculateTotalDrCr();
        }

        function CalculateTotalDrCr() {

            var dr = "", cr = "", totalDr = 0.00, totalCr = 0.00;
            var drAmount = 0.00, crAmount = 0.00, voucherType = "";

            voucherType = $("#ContentPlaceHolder1_ddlVoucherType").val();

            $('#VoucherGrid tbody tr:not(:first)').each(function () {
                dr = $(this).find("td:eq(1)").text();
                cr = $(this).find("td:eq(2)").text();

                drAmount = dr == "" ? 0.00 : parseFloat(dr);
                crAmount = cr == "" ? 0.00 : parseFloat(cr);

                totalDr += drAmount;
                totalCr += crAmount;
            });

            if (voucherType == "CP" || voucherType == "BP") {
                $('#VoucherGrid tbody tr:eq(0)').find("td:eq(2)").text(totalDr);
                VoucherList[0].CrAmount = totalCr;
            }
            else if (voucherType == "CR" || voucherType == "BR") {
                $('#VoucherGrid tbody tr:eq(0)').find("td:eq(1)").text(totalCr);
                VoucherList[0].DrAmount = totalDr;
            }

            if (voucherType == "CP" || voucherType == "BP") {
                $("#lblTotalDrAmount").text(totalDr);
                $("#lblTotalCrAmount").text(totalDr);
            }
            else if (voucherType == "CR" || voucherType == "BR") {
                $("#lblTotalDrAmount").text(totalCr);
                $("#lblTotalCrAmount").text(totalCr);
            }
            else {
                $("#lblTotalDrAmount").text(totalDr);
                $("#lblTotalCrAmount").text(totalCr);
            }
        }

    </script>

    <asp:HiddenField ID="hfNodeMatrix" runat="server" />

    <div class="panel panel-default">
        <div class="panel-heading">Voucher Information</div>
        <div class="panel panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <label for="" class="control-label required-field col-md-2">Voucher Date</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtVoucherDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <label for="" class="control-label required-field col-md-2">Project</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlGLProject" runat="server" CssClass="form-control" TabIndex="5">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group">
                    <label for="" class="control-label required-field col-md-2">Currency Type</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control" TabIndex="3">
                        </asp:DropDownList>
                    </div>
                    <label for="" class="control-label required-field col-md-2">Conversion Rate</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtConversionRate" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <label for="" class="control-label required-field col-md-2">Voucher Type</label>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlVoucherType" runat="server" CssClass="form-control" TabIndex="2">
                            <asp:ListItem Value="">Select One</asp:ListItem>
                            <asp:ListItem Value="CP">Cash Payment (CP) </asp:ListItem>
                            <asp:ListItem Value="BP">Bank Payment (BP)</asp:ListItem>
                            <asp:ListItem Value="CR">Cash Receive (CR)</asp:ListItem>
                            <asp:ListItem Value="BR">Bank Receive (BR)</asp:ListItem>
                            <asp:ListItem Value="JV">Journal Voucher (JV)</asp:ListItem>
                            <asp:ListItem Value="CV">Contra Voucher (CV)</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                </div>

                <fieldset id="cpcrbpbrMainHead">
                    <legend></legend>
                    <div class="form-group">
                        <label for="" id="lblCpBpCrBr" class="control-label required-field col-md-2">Account Head</label>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCpBpCrBr" runat="server" CssClass="form-control" TabIndex="6">
                            </asp:DropDownList>
                        </div>
                        <label for="" class="control-label required-field col-md-2">Transection Mode</label>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlLedgerMode" runat="server" CssClass="form-control" disabled="disabled" TabIndex="7">
                                <asp:ListItem>Select One</asp:ListItem>
                                <asp:ListItem>Dr</asp:ListItem>
                                <asp:ListItem>Cr</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="" class="control-label required-field col-md-2">Cheque Number</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtChequeNumber" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                        </div>
                        <label for="" class="control-label required-field col-md-2">Cheque Date</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtChequeDate" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                        </div>
                    </div>

                </fieldset>

            </div>
        </div>
    </div>

    <div class="panel panel-default">
        <div class="panel-heading">Voucher Details</div>
        <div class="panel panel-body">
            <div class="row">
                <div class="col-md-12">

                    <table class="table table-hover table-condensed table-responsive">
                        <thead>
                            <tr>
                                <td style="width: 70%; text-align: right;">Total</td>
                                <td style="width: 10%; text-align: right;">DR Amount:</td>
                                <td style="width: 10%;">
                                    <label id="lblTotalDrAmount"></label>
                                </td>
                                <td style="width: 10%; text-align: right;">Cr Amount:</td>
                                <td style="width: 10%;">
                                    <label id="lblTotalCrAmount"></label>
                                </td>
                            </tr>
                        </thead>
                    </table>

                    <table id="VoucherGrid" class="table table-hover table-bordered table-condensed table-responsive">
                        <thead>
                            <tr>
                                <td style="width: 40%;">Account Name</td>
                                <td style="width: 10%;">DR Amount</td>
                                <td style="width: 10%;">Cr Amount</td>
                                <td style="width: 32%;">Narration</td>
                                <td style="width: 8%;">Action</td>
                            </tr>
                            <tr>
                                <td style="width: 40%;">
                                    <input type="text" class="form-control" id="txtAccountName" />
                                </td>
                                <td style="width: 10%;">
                                    <input type="text" class="form-control text-right" id="txtDrAmount" />
                                </td>
                                <td style="width: 10%;">
                                    <input type="text" class="form-control text-right" id="txtCrAmount" />
                                </td>
                                <td style="width: 35%;">
                                    <input type="text" class="form-control" id="txtNarration" /></td>
                                <td style="width: 5%;">
                                    <input type="button" class="btn btn-sm btn-primary" value="Add" onclick="AddVoucherNodeDetails()" /></td>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div>
            </div>

        </div>
    </div>


</asp:Content>
