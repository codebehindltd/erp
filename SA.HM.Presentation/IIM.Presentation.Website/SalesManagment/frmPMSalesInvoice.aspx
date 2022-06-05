<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmPMSalesInvoice.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesManagment.frmPMSalesInvoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Sales Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Bill Generate</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            var txtEndDate = '<%=txtToDate.ClientID%>'
            $('#' + txtEndDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
        });


        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }

        $(function () {
            $("#myTabs").tabs();
        });


    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div style="height: 45px">
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Monthly Invoice</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Individual Invoice</a></li>
        </ul>
        <div id="tab-1">
            <div id="SearchPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Sales Bill Generate
                    Information </a>
                <div class="HMContainerRowButton">
                    <%--Right Left--%>
                    <asp:Button ID="btnBillPreview" runat="server" TabIndex="3" Text="Bill Preview" CssClass="TransactionalButton btn btn-primary"
                        OnClick="btnBillPreview_Click" />
                </div>
                <div class="divClear">
                </div>
                <div class="block-body collapse in">
                    <asp:GridView ID="gvSalesBillGenerate" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True"
                        ForeColor="#333333" OnRowDataBound="gvSalesBillGenerate_RowDataBound" OnRowCommand="gvSalesBillGenerate_RowCommand">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("SalesId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-Width="30%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CustomerCode" HeaderText="Customer ID" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BillNumber" HeaderText="Sales Order" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DueAmount" HeaderText="Due/ Advance" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgBillPreview" runat="server" CausesValidation="False" CommandArgument='<%# bind("SalesId") %>'
                                        CommandName="CmdBillGenarate" ImageUrl="~/Images/ReportDocument.png" Text=""
                                        AlternateText="Bill Preview" ToolTip="Bill Preview" />
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
                <div class="HMContainerRowButton">
                    <%--Right Left--%>
                    <asp:Button ID="btnBillGenerate" runat="server" TabIndex="3" Text="Bill Generate"
                        CssClass="TransactionalButton btn btn-primary" OnClick="btnBillGenerate_Click" />
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="divClear">
            </div>
            <div id="EntryPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Bill Receive Information
                </a>
                <div class="HMBodyContainer">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblInvoiceNumber" runat="server" Text="Invoice Number"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtInvoiceNumber" runat="server"> </asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblCustomerId" runat="server" Text="Customer Id"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:HiddenField ID="txtCustomerId" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="txtInvoiceId" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="txtPaymentId" runat="server"></asp:HiddenField>
                            <asp:TextBox ID="txtCustomerCode" runat="server"> </asp:TextBox>
                            <asp:HiddenField ID="txtHiddenFieldId" runat="server"></asp:HiddenField>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="HMContainerRowButton">
                        <%--Right Left--%>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                            CssClass="TransactionalButton btn btn-primary" TabIndex="4" />
                    </div>
                </div>
                <div class="divClear">
                </div>
                <asp:Panel ID="pnlCustomerInformation" runat="server" Height="162px">
                    <div class="divFullSectionWithTwoDvie">
                        <div class="divBox divSectionLeftRightSameWidth">
                            
                            <div id="Div2" class="block">
                                <a href="#page-stats" class="block-heading" data-toggle="collapse">Customer Information
                                </a>
                                <div class="HMBodyContainer">
                                    <div class="divSection">
                                        <div class="divBox divSectionLeftLeft">
                                            <asp:Label ID="lblHCustomerName" runat="server" Text="Customer Name"></asp:Label>
                                        </div>
                                        <div class="divBox divSectionLeftRight">
                                            <asp:Label ID="lblCustomerName" runat="server" Font-Bold="True"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="divClear">
                                    </div>
                                    <div class="divSection">
                                        <div class="divBox divSectionLeftLeft">
                                            <asp:Label ID="lblHCode" runat="server" Text="CustomerCode"></asp:Label>
                                        </div>
                                        <div class="divBox divSectionLeftRight">
                                            <asp:Label ID="lblCode" runat="server" Font-Bold="True"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="divClear">
                                    </div>
                                    <div class="divSection">
                                        <div class="divBox divSectionLeftLeft">
                                            <asp:Label ID="lblHBillForm" runat="server" Text="Bill Form"></asp:Label>
                                        </div>
                                        <div class="divBox divSectionLeftRight">
                                            <asp:Label ID="lblBillForm" runat="server" Font-Bold="True"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="divClear">
                                    </div>
                                    <div class="divSection">
                                        <div class="divBox divSectionLeftLeft">
                                            <asp:Label ID="lblHBillTo" runat="server" Text="Bill To"></asp:Label>
                                        </div>
                                        <div class="divBox divSectionLeftRight">
                                            <asp:Label ID="lblBillTo" runat="server" Font-Bold="True"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="divClear">
                                    </div>
                                    <div class="divSection">
                                        <div class="divBox divSectionLeftLeft">
                                            <asp:Label ID="lblHInvoiceAmount" runat="server" Text="Invoice Amount"></asp:Label>
                                        </div>
                                        <div class="divBox divSectionLeftRight">
                                            <asp:Label ID="lblInvoiceAmount" runat="server" Font-Bold="True"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="divClear">
                                    </div>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                        </div>
                    </div>
                    <div class="divBox divSectionLeftRightSameWidth">
                        <%--<div id="ProfitLossInformation" class="block">
                        <a href="#page-stats" class="block-heading" data-toggle="collapse">Bill Preview Information</a>--%>
                        <div class="HMBodyContainerGridView">
                            <div class="HMBodyContainer">
                                <div class="divSection">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                    <asp:Label ID="lblFromDateValue" runat="server" Text="From Date"></asp:Label>
                                    </div>
                                </div>
                                <div class="divClear">
                                </div>
                                <div class="divSection">
                                    <div class="divBox divSectionLeftLeft">
                                        <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>
                                    </div>
                                    <div class="divBox divSectionLeftRight">
                                        <asp:TextBox ID="txtToDate" runat="server"> </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="HMContainerRowButton">
                                <%--Right Left--%>
                                <asp:Button ID="btnIndividualBillPreview" runat="server" TabIndex="3" Text="Bill Preview"
                                    CssClass="TransactionalButton btn btn-primary" 
                                    onclick="btnIndividualBillPreview_Click" />
                            </div>
                        </div>
                        <%--</div>--%>
                    </div>
                </asp:Panel>
                <div class="divClear">
                </div>
                <div class="block-body collapse in">
                    <asp:GridView ID="gvIndividualSalesBillGenerate" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True"
                        ForeColor="#333333" OnRowDataBound="gvIndividualSalesBillGenerate_RowDataBound" OnRowCommand="gvIndividualSalesBillGenerate_RowCommand">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("SalesId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-Width="30%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CustomerCode" HeaderText="Customer ID" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BillNumber" HeaderText="Sales Order" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DueAmount" HeaderText="Due/ Advance" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" ItemStyle-Width="15%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgBillPreview" runat="server" CausesValidation="False" CommandArgument='<%# bind("SalesId") %>'
                                        CommandName="CmdBillGenarate" ImageUrl="~/Images/ReportDocument.png" Text=""
                                        AlternateText="Bill Preview" ToolTip="Bill Preview" />
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
                <div class="HMContainerRowButton">
                    <%--Right Left--%>
                    <asp:Button ID="btnIndividualBillGenerate" runat="server" TabIndex="3" Text="Bill Generate"
                        CssClass="TransactionalButton btn btn-primary" 
                        onclick="btnIndividualBillGenerate_Click" />
                </div>
            </div>
            <div class="divClear">
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var x = '<%=isMessageBoxEnable%>';
        if (x > -1) {
            MessagePanelShow();
            if (x == 2) {
                $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
            }
        }
        else {
            MessagePanelHide();
        }

    </script>
</asp:Content>
