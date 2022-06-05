<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmLCApproval.aspx.cs" Inherits="HotelManagement.Presentation.Website.LCManagement.frmLCApproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Purchase</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Product PO Approval</li>";
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

                tr += "<td style='width:30%;'>" + result[row].ItemName + "</td>";
                tr += "<td style='width:20%;'>" + result[row].Quantity + "</td>";
                tr += "<td style='width:20%;'>" + result[row].StockBy + "</td>";
                tr += "<td style='width:15%;'>" + result[row].PurchasePrice + "</td>";
                tr += "<td style='width:15%;'>" + ((result[row].PurchasePrice) * (result[row].Quantity)) + "</td>";

                grandTotal += ((result[row].PurchasePrice) * (result[row].Quantity));

                tr += "</tr>";

                $("#DetailsPOGrid tbody").append(tr);
                tr = "";
            }

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
                title: "LC Details Information",
                show: 'slide'
            });

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnLoadPODetailsFailed() { CommonHelper.SpinnerClose(); }
    </script>
    <asp:HiddenField ID="hfIsPurchaseOrderCheckedByEnable" runat="server" Value="0" />
    <div id="DetailsPOGridContaiiner" style="display: none;">
        <table id="DetailsPOGrid" class="table table-bordered table-condensed table-responsive"
            style="width: 100%;">
            <thead>
                <tr style="color: White; background-color: #44545E; text-align: left; font-weight: bold;">
                    <th style="width: 30%;">
                        Product Name
                    </th>
                    <th style="width: 15%;">
                        Quantity
                    </th>
                    <th style="width: 15%;">
                        Stock By
                    </th>
                    <th style="width: 20%;">
                        Purchase Price
                    </th>
                    <th style="width: 20%;">
                        Total Price
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
            <tfoot>
                <tr style='color: White; background-color: #62737D; text-align: left; font-weight: bold;'>
                    <td colspan="4">
                        Total:
                    </td>
                    <td style="width: 20%;">
                    </td>
                </tr>
            </tfoot>
        </table>        
    </div>
    <div id="InfoPanel" class="panel panel-default">        
        <div class="panel-heading">LC Information</div>
        <div class="panel-body"> 
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblSPONumber" runat="server" class="control-label" Text="LC Number"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtSPONumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                </div>
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
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                        TabIndex="4" OnClick="btnSearch_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClientClick="javascript: return PerformClearAction();" TabIndex="5" />
                </div>
            </div>
        </div>
        </div>
    </div>
    <div id="Div1" class="panel panel-default">
        <div class="panel-heading">
            Search Information</div>
        <div class="panel-body">
            <asp:GridView ID="gvOrderInfo" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" OnRowCommand="gvOrderInfo_RowCommand"
                OnPageIndexChanging="gvOrderInfo_PageIndexChanging" TabIndex="9" OnRowDataBound="gvOrderInfo_RowDataBound"
                CssClass="table table-bordered table-condensed table-responsive">
                <RowStyle BackColor="#E3EAEB" />
                <Columns>
                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("LCId") %>'></asp:Label>
                            <asp:Label ID="lblSupplierId" runat="server" Text='<%#Eval("SupplierId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="LCNumber" HeaderText="LC Number" ItemStyle-Width="35%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="LC Open Date" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblgvOrderDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("LCOpenDate"))) %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="LC Mature Date" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblgvReceivedByDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("LCMatureDate"))) %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="20%">
                        <ItemTemplate>
                            <asp:Label ID="lblApprovedStatus" runat="server" Text='<%#Eval("ApprovedStatus") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="15%">
                        <ItemTemplate>
                            &nbsp;
                            <asp:ImageButton ID="ImgDetailsApproved" runat="server" CausesValidation="False"
                                CommandName="CmdItemPOApproved" CommandArgument='<%# bind("LCId") %>' OnClientClick='<%#String.Format("return ProductReceiveApproved({0})", Eval("LCId")) %>'
                                ImageUrl="~/Images/approved.png" Text="" AlternateText="Details" ToolTip="Approve Purchase Order" />
                            &nbsp;&nbsp;<asp:ImageButton ID="ImgBtnCancelPO" runat="server" CausesValidation="False"
                                CommandName="CmdItemPOCancel" CommandArgument='<%# bind("LCId") %>' OnClientClick='<%#String.Format("return CancelReceivedItem({0})", Eval("LCId")) %>'
                                ImageUrl="~/Images/cancel.png" Text="" AlternateText="Details" ToolTip="Cancel Purchase Order" />
                            <%--&nbsp;&nbsp;<asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False"
                                CommandName="CmdEdit" CommandArgument='<%# bind("LCId") %>' ImageUrl="~/Images/edit.png"
                                Text="" AlternateText="Edit" ToolTip="Edit Purchase Order" />--%>
                            &nbsp;&nbsp;<asp:ImageButton ID="ImgDetailsPO" runat="server" CausesValidation="False"
                                CommandName="CmdPODetails" CommandArgument='<%# bind("LCId") %>' OnClientClick='<%#String.Format("return ProductPODetails({0})", Eval("LCId")) %>'
                                ImageUrl="~/Images/detailsInfo.png" Text="" AlternateText="Details" ToolTip="Purchase Order Details" />
                            <%--&nbsp;&nbsp;<asp:ImageButton ID="ImgReportPO" runat="server" CausesValidation="False"
                                CommandName="CmdReportPO" CommandArgument='<%# bind("LCId") %>' ImageUrl="~/Images/ReportDocument.png"
                                Text="" AlternateText="Invoice" ToolTip="Purchase Order Invoice" />--%>
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
