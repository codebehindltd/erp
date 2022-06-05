<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmSupplierBillGeneration.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.frmSupplierBillGeneration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_ddlSupplier").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
        });

        function SearchSupplierBill() {

            CommonHelper.SpinnerOpen();
            var supplierId = $("#ContentPlaceHolder1_ddlSupplier").val();
            $("#BillInfo tbody").html("");
            PageMethods.SupplierBillBySearch(supplierId, OnLoadSupplierBillSucceeded, OnSupplierBillFailed);
        }

        function OnLoadSupplierBillSucceeded(result) {

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
                    tr += "<input type='checkbox' id='pay" + result[row].SupplierPaymentId + "'" + chk + " />";
                }
                else {
                    tr += "<input type='checkbox' id='pay" + result[row].SupplierPaymentId + "' />";
                }

                tr += "</td>";

                tr += "<td style='width: 20%'>" + GetStringFromDateTime(result[row].PaymentDate) + "</td>";
                tr += "<td style='width: 45%'>" + result[row].BillNumber + "</td>";
                tr += "<td style='width: 25%'>" + result[row].DueAmount + "</td>";
                tr += "<td style=display:none;'>" + result[row].SupplierPaymentId + "</td>";
                tr += "<td style=display:none;'>" + result[row].BillId + "</td>";
                tr += "<td style=display:none;'>" + isChecked + "</td>";

                tr += "</tr>";

                $("#BillInfo tbody").append(tr);
                tr = "";
            }

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnSupplierBillFailed(error) {
            toastr.info("Error On Bill Search.");
            CommonHelper.SpinnerClose();
        }

        function GenerateSupplierBill() {
            var SupplierBill = new Array();
            CommonHelper.SpinnerOpen();

            $("#BillInfo tbody tr").each(function () {

                if ($(this).find("td:eq(0)").find("input").is(":checked") == true) {
                    SupplierBill.push({
                        SupplierPaymentId: $(this).find("td:eq(4)").text(),
                        BillId: $(this).find("td:eq(5)").text(),
                        IsBillGenerated: true
                    });
                }
                else if ($(this).find("td:eq(6)").text() == "1") {

                    SupplierBill.push({
                        SupplierPaymentId: $(this).find("td:eq(4)").text(),
                        BillId: $(this).find("td:eq(5)").text(),
                        IsBillGenerated: false
                    });
                }
            });

            PageMethods.GenerateSuplierBill(SupplierBill, OnGenerateCompanyBillSucceeded, OnGenerateCompanyBillFailed);
        }

        function OnGenerateCompanyBillSucceeded(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
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

        function PerformClearAction() {
            $("#BillInfo tbody").html("");
            $('#ContentPlaceHolder1_ddlSupplier').val("0").trigger('change');
        }

    </script>

    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Supplier Bill Search
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblPaymentDate" runat="server" class="control-label" Text="Supplier"></asp:Label>
                    </div>
                    <div class="col-md-8">
                        <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <input type="button" class="btn btn-primary" value="Search" onclick="SearchSupplierBill()" />
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-12">
                        <table id="BillInfo" class="table table-bordered table-condensed table-hover table-responsive">
                            <thead>
                                <tr>
                                    <th style="width: 10%;">Select</th>
                                    <th style="width: 20%;">Bill Date</th>
                                    <th style="width: 45%;">Bill Number</th>
                                    <th style="width: 25%;">Amount</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-12">
                        <input type="button" class="btn btn-primary" value="Generate Bill" onclick="GenerateSupplierBill()" />
                    </div>
                </div>

            </div>
        </div>
    </div>


</asp:Content>
