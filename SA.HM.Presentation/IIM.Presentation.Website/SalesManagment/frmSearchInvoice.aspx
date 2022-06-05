<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmSearchInvoice.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesManagment.frmSearchInvoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Sales Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Search Invoice</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });

    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="EntryPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Invoice
        </a>
        <div class="HMBodyContainer">
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblInvoiceNumber" runat="server" Text="Invoice Number"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtInvoiceNumber" TabIndex="1" runat="server"> </asp:TextBox>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblCustomerId" runat="server" Text="Customer Id"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:TextBox ID="txtCustomerCode" TabIndex="2" runat="server"> </asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="HMContainerRowButton">
                <%--Right Left--%>
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                    OnClientClick="aspnetForm.target ='_blank';" CssClass="TransactionalButton btn btn-primary"
                    TabIndex="3" />
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Sales Invoice Information
        </a>
        <div class="divClear">
        </div>
        <div class="block-body collapse in">
            <asp:GridView ID="gvSalesInvoice" Width="100%" runat="server" AllowPaging="True"
                AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True"
                ForeColor="#333333" OnRowDataBound="gvSalesInvoice_RowDataBound" OnRowCommand="gvSalesInvoice_RowCommand">
                <RowStyle BackColor="#E3EAEB" />
                <Columns>
                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("SalesId") %>'></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="SalesDate" HeaderText="Sales Date" DataFormatString="{0:dd/MM/yyyy}"
                        ItemStyle-Width="10%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>--%>
                    <asp:TemplateField HeaderText="Sales Date " ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblFromDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("SalesDate")))%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="InvoiceNumber" HeaderText="Invoice Number" ItemStyle-Width="15%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomerCode" HeaderText="Customer ID" ItemStyle-Width="15%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-Width="40%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <%-- <asp:BoundField DataField="GrandTotal" HeaderText="Grand Total" ItemStyle-Width="15%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>--%>
                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImgBillPreview" runat="server" CausesValidation="False" CommandArgument='<%# String.Format("{0} , {1}", Eval("SalesId"), Eval("InvoiceId")) %>'
                                CommandName="CmdInvoice" ImageUrl="~/Images/ReportDocument.png" Text="" AlternateText="Bill Preview"
                                ToolTip="Bill Preview" />
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
        <div class="divClear">
        </div>
    </div>
    <script type="text/javascript">
        var x = '<%=isMessageBoxEnable%>';
        if (x > -1) {

            if (x == 2) {
                $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
            }
        }
        else {

        }

    </script>
</asp:Content>
