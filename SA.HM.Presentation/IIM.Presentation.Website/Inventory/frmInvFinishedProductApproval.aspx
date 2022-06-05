<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmInvFinishedProductApproval.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.frmInvFinishedProductApproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Finished Product Approval</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            var txtStartDate = '<%=txtFromDate.ClientID%>'
            var txtEndDate = '<%=txtToDate.ClientID%>'

            $('#' + txtStartDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtEndDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtEndDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtStartDate).datepicker("option", "maxDate", selectedDate);
                }
            });
        });

        function FinishProductDetails(finishProductId) {
            PageMethods.GetFinishProductDetails(finishProductId, OnFinishProductLoadSucceeded, OnFinishProductLoadFailed);
            return false;
        }
        function OnFinishProductLoadSucceeded(result) {

            $("#DetailsFinishProductGrid tbody").html("");
            var totalRow = result.length, row = 0;

            var tr = "";

            for (row = 0; row < totalRow; row++) {

                if (row % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:40%;'>" + result[row].ProductName + "</td>";
                tr += "<td style='width:25%;'>" + result[row].StockBy + "</td>";
                tr += "<td style='width:25%;'>" + result[row].Quantity + "</td>";

                tr += "</tr>";

                $("#DetailsFinishProductGrid tbody").append(tr);
                tr = "";
            }

            $("#DetailsFinishProductGridContainer").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 800,
                maxWidth: 900,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Finish Item Details",
                show: 'slide'
            });
        }
        function OnFinishProductLoadFailed() { }

        function ClearFinishGoodsProduct() {

            $("#ContentPlaceHolder1_ddlSearchCostCenter").val("0");
            $("#ContentPlaceHolder1_txtFromDate").val("");
            $("#ContentPlaceHolder1_txtToDate").val("");

            return false;
        }

    </script>
    <div id="DetailsFinishProductGridContainer" style="display: none;">
        <table id="DetailsFinishProductGrid" class="table table-bordered table-condensed table-responsive"
            style="width: 100%;">
            <thead>
                <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                    <th style="width: 40%;">
                        Item Name
                    </th>
                    <th style="width: 25%;">
                        Stock By
                    </th>
                    <th style="width: 25%;">
                        Quantity
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
    <asp:HiddenField ID="hfItemId" runat="server" Value="0" />
    <asp:HiddenField ID="hfFinishProductId" runat="server" Value="0" />
    <div id="SearchEntry" class="panel panel-default">
        <div class="panel-heading">
            Finished Item Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label" Text="Cost Center"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlSearchCostCenter" runat="server" CssClass="form-control"
                            TabIndex="1">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
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
                            OnClientClick="javascript: return ClearFinishGoodsProduct();" TabIndex="6" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information</div>
        <div class="panel-body">
            <asp:GridView ID="gvFinishedProductInfo" Width="100%" runat="server" AutoGenerateColumns="False"
                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" TabIndex="9"
                OnRowCommand="gvFinishedProductInfo_RowCommand" OnRowDataBound="gvFinishedProductInfo_RowDataBound">
                <RowStyle BackColor="#E3EAEB" CssClass="table table-bordered table-condensed table-responsive" />
                <Columns>
                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("FinishProductId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Order Date" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblgvVoucherDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("OrderDate"))) %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="CostCenter" HeaderText="Cost Center" ItemStyle-Width="45%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ApprovedStatus" HeaderText="Status" ItemStyle-Width="15%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="15%">
                        <ItemTemplate>
                            &nbsp;<asp:ImageButton ID="ImgDetailsFinishGoods" runat="server" CausesValidation="False"
                                CommandName="CmdDetails" CommandArgument='<%# bind("OrderDate") %>' OnClientClick='<%#String.Format("return FinishProductDetails({0})", Eval("FinishProductId")) %>'
                                ImageUrl="~/Images/detailsInfo.png" Text="" AlternateText="Details" ToolTip="Product Details" />
                            &nbsp;<asp:ImageButton ID="ImgDetailsApproved" OnClientClick="return confirm('Do you want to approve?');" runat="server" CausesValidation="False"
                                CommandName="CmdItemApproved" CommandArgument='<%# bind("FinishProductId") %>'
                                ImageUrl="~/Images/approved.png" Text="" AlternateText="Details" ToolTip="Approve Item" />
                            &nbsp;&nbsp;<asp:ImageButton ID="ImgBtnCancel" OnClientClick="return confirm('Do you want to cancel?');" runat="server" CausesValidation="False"
                                CommandName="CmdItemCancel" CommandArgument='<%# bind("FinishProductId") %>'
                                ImageUrl="~/Images/cancel.png" Text="" AlternateText="Details" ToolTip="Cancel Item" />
                            &nbsp;<asp:ImageButton ID="ImgUpdate" OnClientClick="return confirm('Do you want to edit?');" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                CommandArgument='<%# bind("FinishProductId") %>' ImageUrl="~/Images/edit.png"
                                Text="" AlternateText="Edit" ToolTip="Edit" />
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
        </div>
    </div>
</asp:Content>
