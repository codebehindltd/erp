<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmSalesOrderApproval.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.frmSalesOrderApproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Sales & Marketing</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Product Sales Order Approval</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

        });

        $(document).ready(function () {
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


        function ProductPODetails(pOrderId) {
            CommonHelper.SpinnerOpen();
            PageMethods.PerformLoadPMProductDetailOnDisplayMode(pOrderId, OnLoadPODetailsSucceeded, OnLoadPODetailsFailed);
            return false;
        }

        function OnLoadPODetailsSucceeded(result) {
            $("#DetailsPOGrid tbody").html("");
            var totalRow = result.length, row = 0, status = "";
            var tr = "", grandTotal = 0.0;
            var orderRemarksDisplay = "";

            status = result[row].Status;

            if (status == "Approved") {
                $("#buttonContainer").hide();
            }
            else {
                $("#buttonContainer").show();
            }

            for (row = 0; row < totalRow; row++) {
                orderRemarksDisplay = result[row].OrderRemarks;
                if (row % 2 == 0) {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:30%;'>" + result[row].ProductName + "</td>";
                tr += "<td style='width:20%;'>" + result[row].Quantity + "</td>";
                tr += "<td style='width:20%;'>" + result[row].StockBy + "</td>";
                tr += "<td style='width:15%;'>" + result[row].PurchasePrice + "</td>";
                tr += "<td style='width:15%;'>" + ((result[row].PurchasePrice) * (result[row].Quantity)) + "</td>";

                grandTotal += ((result[row].PurchasePrice) * (result[row].Quantity));

                tr += "</tr>";

                $("#DetailsPOGrid tbody").append(tr);
                tr = "";
            }
            $('#ContentPlaceHolder1_lblOrderRemarksDisplay').text("Order Remarks: " + orderRemarksDisplay);
            $("#DetailsPOGrid tfoot tr:eq(0)").find("td:eq(1)").text(grandTotal);


            $("#DetailsPOGridContaiiner").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 800,
                maxWidth: 900,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Sales Order Details for the SO Number: " + result[0].SONumber,
                show: 'slide'
            });

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnLoadPODetailsFailed() { CommonHelper.SpinnerClose(); }
        function CreateNewSalesOrder() {
            //PerformClearAction();
            var iframeid = 'frmPrint';
            var url = "./frmSalesOrder.aspx";
            parent.document.getElementById(iframeid).src = url;

            $("#SalesOrderDialogue").dialog({
                autoOpen: true,
                modal: true,
                width: 1100,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "New Sales Order",
                show: 'slide'
            });
            return false;
        }
        function CloseSalesOrderDialog() {
            $('#SalesOrderDialogue').dialog('close');
            return false;
        }
    </script>
    <asp:HiddenField ID="hfIsPurchaseOrderCheckedByEnable" runat="server" Value="0" />
    <div id="SalesOrderDialogue" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="DetailsPOGridContaiiner" style="display: none;">
        <table id="DetailsPOGrid" class="table table-bordered table-condensed table-responsive"
            style="width: 100%;">
            <thead>
                <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                    <th style="width: 30%;">Product Name
                    </th>
                    <th style="width: 15%;">Quantity
                    </th>
                    <th style="width: 15%;">Stock By
                    </th>
                    <th style="width: 20%;">Unit Price
                    </th>
                    <th style="width: 20%;">Total Price
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
            <tfoot>
                <tr style='color: White; background-color: #62737D; text-align: left; font-weight: bold;'>
                    <td colspan="4">Total:
                    </td>
                    <td style="width: 20%;"></td>
                </tr>
            </tfoot>
        </table>
        <div class="divClear">
        </div>
        <asp:Label ID="lblOrderRemarksDisplay" runat="server" class="control-label" Text=""></asp:Label>
    </div>
    <div id="InfoPanel" class="panel panel-default">
        <div class="panel-heading">Order Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSPONumber" runat="server" class="control-label" Text="SO Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSPONumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblSrcCostCenter" runat="server" class="control-label" Text="Cost Center"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSrcCostCenter" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server" TabIndex="2"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Panel ID="pnlStatus" runat="server">
                        <div class="col-md-2">
                            <asp:Label ID="lblStatus" runat="server" class="control-label" Text="Status"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="1" CssClass="form-control">
                                <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                <asp:ListItem Text="Submitted" Value="Pending"></asp:ListItem>
                                <asp:ListItem Text="Checked" Value="Checked"></asp:ListItem>
                                <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </asp:Panel>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            TabIndex="4" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return PerformClearAction();" TabIndex="5" />
                        <asp:Button ID="btnAdd" runat="server" Text="New Sales Order" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateNewSalesOrder();" TabIndex="6" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="Div1" class="panel panel-default">
        <div class="panel-heading">
            Search Information
        </div>
        <div class="panel-body">
            <asp:GridView ID="gvOrderInfo" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" OnRowCommand="gvOrderInfo_RowCommand"
                OnPageIndexChanging="gvOrderInfo_PageIndexChanging" TabIndex="9" OnRowDataBound="gvOrderInfo_RowDataBound"
                CssClass="table table-bordered table-condensed table-responsive">
                <RowStyle BackColor="#E3EAEB" />
                <Columns>
                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("POrderId") %>'></asp:Label>
                            <asp:Label ID="lblSupplierId" runat="server" Text='<%#Eval("SupplierId") %>'></asp:Label>
                            <asp:Label ID="lblDeliveryStatus" runat="server" Text='<%#Eval("DeliveryStatus") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PONumber" HeaderText="SO Number" ItemStyle-Width="10%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CostCenter" HeaderText="Cost Center" ItemStyle-Width="15%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SupplierName" HeaderText="Company" ItemStyle-Width="30%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Order Date" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblgvOrderDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("CreatedDate"))) %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delivery Date" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblgvReceivedByDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ReceivedByDate"))) %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="lblApprovedStatus" runat="server" Text='<%#Eval("ApprovedStatus") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="20%">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImgDetailsApproved" runat="server" CausesValidation="False"
                                CommandName="CmdItemPOApproved" CommandArgument='<%# bind("POrderId") %>' OnClientClick='<%#String.Format("return ProductReceiveApproved({0})", Eval("POrderId")) %>'
                                ImageUrl="~/Images/approved.png" Text="" AlternateText="Details" ToolTip="Approve Sales Order" />
                            &nbsp;&nbsp;<asp:ImageButton ID="ImgBtnCancelPO" runat="server" CausesValidation="False"
                                CommandName="CmdItemPOCancel" CommandArgument='<%# bind("POrderId") %>'
                                ImageUrl="~/Images/cancel.png" Text="" AlternateText="Delete" ToolTip="Delete Sales Order" OnClientClick="return confirm('Do you want to Delete?');" />
                            <%--&nbsp;&nbsp;<asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False"
                                CommandName="CmdEdit" CommandArgument='<%# bind("POrderId") %>' ImageUrl="~/Images/edit.png"
                                Text="" AlternateText="Edit" ToolTip="Edit Sales Order" />--%>
                            <%--&nbsp;&nbsp;<asp:ImageButton ID="ImgDetailsPO" runat="server" CausesValidation="False"
                                CommandName="CmdPODetails" CommandArgument='<%# bind("POrderId") %>' OnClientClick='<%#String.Format("return ProductPODetails({0})", Eval("POrderId")) %>'
                                ImageUrl="~/Images/detailsInfo.png" Text="" AlternateText="Details" ToolTip="Sales Order Details" />--%>
                            &nbsp;&nbsp;<asp:ImageButton ID="ImgReportPO" runat="server" CausesValidation="False"
                                CommandName="CmdReportPO" CommandArgument='<%# bind("POrderId") %>' ImageUrl="~/Images/ReportDocument.png"
                                Text="" AlternateText="Invoice" ToolTip="Sales Order Invoice" />
                            &nbsp;&nbsp;<asp:ImageButton ID="ImgBillStatus" runat="server" CausesValidation="False"
                                CommandName="CmdBillStatus" CommandArgument='<%# bind("POrderId") %>'
                                ImageUrl="~/Images/checked.png" Text="" AlternateText="Details" ToolTip="Already Billed" />
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
