<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmSalesCallAnalysis.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.frmSalesCallAnalysis" %>

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
                title: "Sales Order Details",
                show: 'slide'
            });

            CommonHelper.SpinnerClose();
            return false;
        }
        function OnLoadPODetailsFailed() { CommonHelper.SpinnerClose(); }
    </script>

    <div id="InfoPanel" class="panel panel-default">
        <div class="panel-heading">Sales Call Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSPONumber" runat="server" class="control-label" Text="Company"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="1" CssClass="form-control" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblStatus" runat="server" class="control-label" Text="Site"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCompanySite" runat="server" TabIndex="1" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Opportunity Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlOpportunityStatus" runat="server" TabIndex="1" CssClass="form-control">
                        </asp:DropDownList>
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
            Search Information
        </div>
        <div class="panel-body">
            <asp:GridView ID="gvOrderInfo" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" OnRowCommand="gvOrderInfo_RowCommand"
                TabIndex="9" CssClass="table table-bordered table-condensed table-responsive">
                <RowStyle BackColor="#E3EAEB" />
                <Columns>
                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("SalesCallId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CompanyName" HeaderText="Company Name" ItemStyle-Width="15%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SiteName" HeaderText="Site Name" ItemStyle-Width="15%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                     <asp:BoundField DataField="LocationName" HeaderText="Location Name" ItemStyle-Width="10%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CIType" HeaderText="CI Type" ItemStyle-Width="10%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ActionPlan" HeaderText="Action Plan" ItemStyle-Width="15%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OpportunityStatus" HeaderText="Opportunity Status" ItemStyle-Width="10%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>                  

                    <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="15%">
                        <ItemTemplate>
                            &nbsp;
                            <asp:ImageButton ID="ImgDetailsApproved" runat="server" CausesValidation="False"
                                CommandName="CmdItemPOApproved" CommandArgument='<%# bind("SalesCallId") %>' OnClientClick='<%#String.Format("return ProductReceiveApproved({0})", Eval("SalesCallId")) %>'
                                ImageUrl="~/Images/approved.png" Text="" AlternateText="Details" ToolTip="Approve Sales Order" />
                            &nbsp;&nbsp;<asp:ImageButton ID="ImgBtnCancelPO" runat="server" CausesValidation="False"
                                CommandName="CmdItemPOCancel" CommandArgument='<%# bind("SalesCallId") %>' OnClientClick='<%#String.Format("return CancelReceivedItem({0})", Eval("SalesCallId")) %>'
                                ImageUrl="~/Images/cancel.png" Text="" AlternateText="Details" ToolTip="Cancel Sales Order" />
                            &nbsp;&nbsp;<asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False"
                                CommandName="CmdEdit" CommandArgument='<%# bind("SalesCallId") %>' ImageUrl="~/Images/edit.png"
                                Text="" AlternateText="Edit" ToolTip="Edit Sales Order" />
                            &nbsp;&nbsp;<asp:ImageButton ID="ImgDetailsPO" runat="server" CausesValidation="False"
                                CommandName="CmdPODetails" CommandArgument='<%# bind("SalesCallId") %>' OnClientClick='<%#String.Format("return ProductPODetails({0})", Eval("SalesCallId")) %>'
                                ImageUrl="~/Images/detailsInfo.png" Text="" AlternateText="Details" ToolTip="Sales Order Details" />
                            &nbsp;&nbsp;<asp:ImageButton ID="ImgReportPO" runat="server" CausesValidation="False"
                                CommandName="CmdReportPO" CommandArgument='<%# bind("SalesCallId") %>' ImageUrl="~/Images/ReportDocument.png"
                                Text="" AlternateText="Invoice" ToolTip="Sales Order Invoice" />
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
