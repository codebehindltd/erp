<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmPMOutProductReceive.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.frmPMOutProductReceive" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Out Product Receive</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#btnSearch").click(function () {
                var outId = $("#ContentPlaceHolder1_ddlPONumber").val();
                ProductOutDetails(outId);
            });
        });

        function ProductOutDetails(outId) {
            PageMethods.GetProductOutDetails(outId, OnProductOutDetailsLoadSucceeded, OnSaveProductOutDetailsLoadFailed);
            return false;
        }
        function OnProductOutDetailsLoadSucceeded(result) {
            $("#ContentPlaceHolder1_hfOutId").val(result[0].OutId);
            $("#DetailsOutGrid tbody").html("");
            var totalRow = result.length, row = 0;

            var tr = "";

            for (row = 0; row < totalRow; row++) {
                if (row % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:15%;'>" + result[row].ProductName + "</td>";

                if (result[row].SerialNumber == null)
                    tr += "<td style='width:12%;'>" + '' + "</td>";
                else
                    tr += "<td style='width:12%;'>" + result[row].SerialNumber + "</td>";

                tr += "<td style='width:8%;'>" + result[row].Quantity + "</td>";
                tr += "<td style='width:15%;'>" + result[row].CostCenterFrom + "</td>";
                tr += "<td style='width:13%;'>" + result[row].LocationFrom + "</td>";
                tr += "<td style='width:15%;'>" + result[row].CostCenterTo + "</td>";
                tr += "<td style='width:13%;'>" + result[row].LocationTo + "</td>";
                tr += "<td style='width:8%;'>" + result[row].StockBy + "</td>";

                tr += "</tr>";

                $("#DetailsOutGrid tbody").append(tr);
                tr = "";
            }

            $("#DetailsOutGridContaiiner").show();
        }
        function OnSaveProductOutDetailsLoadFailed(error) {
        }
    </script>
    <asp:HiddenField ID="hfOutId" runat="server" Value="0"></asp:HiddenField>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Product Out for Receive</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblPONo" runat="server" class="control-label" Text="Product Out Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlPONumber" runat="server" CssClass="form-control" TabIndex="3">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <button type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm">
                            Search</button>
                    </div>
                </div>
                <div class="form-group" style="padding-top: 10px;">
                    <div id="DetailsOutGridContaiiner" style="display: none;">
                        <table id="DetailsOutGrid" class="table table-bordered table-condensed table-responsive"
                            style="width: 100%;">
                            <thead>
                                <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                                    <th style="width: 15%;">
                                        Product Name
                                    </th>
                                    <th style="width: 12%;">
                                        Serial Number
                                    </th>
                                    <th style="width: 8%;">
                                        Quantity
                                    </th>
                                    <th style="width: 15%;">
                                        From Cost Center
                                    </th>
                                    <th style="width: 13%;">
                                        From Location
                                    </th>
                                    <th style="width: 15%;">
                                        To Cost Center
                                    </th>
                                    <th style="width: 13%;">
                                        To Location
                                    </th>
                                    <th style="width: 8%;">
                                        Stock By
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                        <div class="row" style="padding-left:10px;">
                            <div class="col-md-12">
                                <asp:Button ID="btnReceive" runat="server" Text="Receive" TabIndex="7" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnReceive_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
