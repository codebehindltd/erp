<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmProductReceiveAccountsPostingApproval.aspx.cs" EnableEventValidation="false"
    Inherits="HotelManagement.Presentation.Website.PurchaseManagment.frmProductReceiveAccountsPostingApproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Product Receive Approval</li>";
            var breadCrumbs = moduleName + formName;


            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_btnCancel").click(function () {
                $('#ContentPlaceHolder1_ddlSearchCostCenterId').val("0");
                $("#ContentPlaceHolder1_ddlSearchPOrderId").val("0");
                $("#ContentPlaceHolder1_txtFromDate").val("");
                $("#ContentPlaceHolder1_txtToDate").val("");
                return false;
            });

            var txtFromDate = '<%=txtFromDate.ClientID%>'
            var txtToDate = '<%=txtToDate.ClientID%>'

            $('#' + txtFromDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtToDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtToDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtFromDate).datepicker("option", "maxDate", selectedDate);
                }
            });

            $("[id=ContentPlaceHolder1_gvProductReceiveInfo_ChkCreate]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#ContentPlaceHolder1_gvProductReceiveInfo tbody tr").find("td:eq(0) > span").find("input").prop("checked", true);
                }
                else {
                    $("#ContentPlaceHolder1_gvProductReceiveInfo tbody tr").find("td:eq(0) > span").find("input").prop("checked", false);
                }
            });

        });

        function ProductReceiveDetails(receivedId) {
            CommonHelper.SpinnerOpen();
            $("#ContentPlaceHolder1_hfReceivedId").val(receivedId);
            PageMethods.ReceivedDetails(receivedId, OnFillRequisitionDetailsSucceed, OnFillRequisitionDetailsFailed);
            return false;
        }
        function OnFillRequisitionDetailsSucceed(result) {

            $("#DetailsReceivedGrid tbody").html("");
            var totalRow = result.length, row = 0, status = "";
            var tr = "";

            status = result[row].Status;

            if (status == "Approved") {
                $("#buttonContainer").hide();
            }
            else {
                $("#buttonContainer").show();
            }

            for (row = 0; row < totalRow; row++) {

                if (row % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:20%;'>" + result[row].ItemName + "</td>";

                

                tr += "<td style='width:10%;'>" + result[row].Quantity + "</td>";
                tr += "<td style='width:10%;'>" + result[row].PurchasePrice + "</td>";
                tr += "<td style='width:10%;'>" + result[row].StockBy + "</td>";
                tr += "<td style='width:15%;'>" + result[row].LocationName + "</td>";
                tr += "<td style='width:15%;'>" + result[row].SupplierName + "</td>";

                tr += "</tr>";

                $("#DetailsReceivedGrid tbody").append(tr);
                tr = "";
            }

            $("#DetailsReceivedGridContaiiner").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 800,
                maxWidth: 900,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Received Item Details",
                show: 'slide'
            });
            CommonHelper.SpinnerClose();
        }
        function OnFillRequisitionDetailsFailed(error) {
            toastr.error(error.get_message());
            CommonHelper.SpinnerClose();
        }

        function ProductReceiveApproved(receivedId) {
            if (confirm("Are you want to approved?")) {
                $("#DetailsReceivedGridContaiiner").dialog("close");

                if (receivedId == "0") {
                    CommonHelper.SpinnerOpen();
                    receivedId = $("#ContentPlaceHolder1_hfReceivedId").val();
                    PageMethods.ApprovedReceivedDetails(receivedId, OnFillApprovedReceivedDetailsSucceed, OnFillApprovedReceivedDetailsFailed);
                    return false;
                }
                else {
                    return true;
                }
            }

            return false;
        }
        function OnFillApprovedReceivedDetailsSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $("#ContentPlaceHolder1_hfReceivedId").val("0");
                $("#ContentPlaceHolder1_btnSearch").trigger('click');
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnFillApprovedReceivedDetailsFailed(error) { CommonHelper.SpinnerClose(); }

        function EditReceivedItem() {
            receivedId = $("#ContentPlaceHolder1_hfReceivedId").val();
            PageMethods.EditReceivedDetails(receivedId, OnEditReceivedDetailsSucceed, OnEditReceivedDetailsFailed);
            return false;
        }
        function OnEditReceivedDetailsSucceed(result) {
            if (result.IsSuccess) {
                window.location = "/Inventory/frmPMProductReceive.aspx";
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnEditReceivedDetailsFailed(error) { }

        function CancelReceivedItem(receivedId) {

            if (confirm("Are you want to cancel order?")) {
                $("#DetailsReceivedGridContaiiner").dialog("close");

                if (receivedId == "0") {
                    CommonHelper.SpinnerOpen();
                    receivedId = $("#ContentPlaceHolder1_hfReceivedId").val();
                    PageMethods.CancelReceivedDetails(receivedId, OnCancelReceivedDetailsSucceed, OnCancelReceivedDetailsFailed);
                    return false;
                }
                else {
                    return true;
                }
            }

            return false;
        }
        function OnCancelReceivedDetailsSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $("#ContentPlaceHolder1_hfReceivedId").val("0");
                $("#ContentPlaceHolder1_btnSearch").trigger('click');
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnCancelReceivedDetailsFailed(error) { CommonHelper.SpinnerClose(); }

    </script>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfReceivedId" runat="server" Value="0" />

    <div id="DetailsReceivedGridContaiiner" style="display: none;">
        <table id="DetailsReceivedGrid" class="table table-bordered table-condensed table-responsive"
            style="width: 100%;">
            <thead>
                <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                    <th style="width: 20%;">Item Name
                    </th>
                    <th style="width: 10%;">Quantity
                    </th>
                    <th style="width: 10%;">Price
                    </th>
                    <th style="width: 10%;">Stock By
                    </th>
                    <th style="width: 15%;">Location
                    </th>
                    <th style="width: 15%;">Suppplier
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>       
    </div>
    <div id="InfoPanel" class="panel panel-default">
        <div class="panel-heading">
            Receive Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label" Text="Order Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtReceiveNumber" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:HiddenField ID="txtDealId" runat="server"></asp:HiddenField>
                        <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server" TabIndex="4"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                            CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary btn-sm"
                            TabIndex="6" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information
        </div>
        <div class="panel-body">
            <asp:GridView ID="gvProductReceiveInfo" Width="100%" runat="server" AutoGenerateColumns="False"
                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333"
                abIndex="9" CssClass="table table-bordered table-condensed table-responsive">
                <RowStyle BackColor="#E3EAEB" />
                <Columns>
                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblReceivedId" runat="server" Text='<%#Eval("ReceivedId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Create/Update" ItemStyle-Width="02%">
                        <HeaderTemplate>
                            <asp:CheckBox ID="ChkCreate" CssClass="ChkCreate" runat="server" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIsSavePermission" CssClass="Chk_Create" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="PONumber" HeaderText="PR Number" ItemStyle-Width="20%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Received Date" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblgvReceiveDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ReceivedDate"))) %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:BoundField DataField="ReceiveNumber" HeaderText="Receive Number" ItemStyle-Width="20%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="20%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="20%">
                        <ItemTemplate>
                            &nbsp;&nbsp;<asp:ImageButton ID="ImgDetailsRequisition" runat="server" CausesValidation="False"
                                CommandName="CmdRequisitionDetails" CommandArgument='<%# bind("ReceivedId") %>'
                                OnClientClick='<%#String.Format("return ProductReceiveDetails({0})", Eval("ReceivedId")) %>'
                                ImageUrl="~/Images/detailsInfo.png" Text="" AlternateText="Details" ToolTip="Receive Details" />
                            <%--&nbsp;&nbsp;<asp:ImageButton ID="ImgReportPR" runat="server" CausesValidation="False"
                                CommandName="CmdReportPR" CommandArgument='<%# bind("ReceivedId") %>' ImageUrl="~/Images/ReportDocument.png"
                                Text="" AlternateText="Report" ToolTip="Product Receive Report" />--%>
                        </ItemTemplate>
                        <ControlStyle Font-Size="Small" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <EmptyDataTemplate>
                    <asp:Label ID="lblRecordNotFound" runat="server" Text="Record Not Found."></asp:Label>
                </EmptyDataTemplate>
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#44545E" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#7C6F57" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>


            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnApprovedPosting" runat="server" Text="Approved Posting"
                        CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" OnClick="btnApprovedPosting_Click" />
                </div>
            </div>

        </div>
    </div>
</asp:Content>
