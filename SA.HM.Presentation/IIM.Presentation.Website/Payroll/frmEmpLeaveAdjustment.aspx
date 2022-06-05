<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpLeaveAdjustment.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpLeaveAdjustment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Leave Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Leave Adjustment</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });        
    </script>
    <div id="EntryPanel" class="panel panel-default">       
        <div class="panel-heading">Instead Leave</div>
        <div class="panel-body">
                    <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblProcessDate" runat="server" class="control-label required-field" Text="Leave Month"></asp:Label>                    
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlEffectedMonth" CssClass="form-control" runat="server">
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblEmpId" runat="server" class="control-label" Text="Employee ID"></asp:Label>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtSrcEmpCode" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>                    
                </div>
                <div class="col-md-2">
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClick="btnSearch_Click" />
                </div>
            </div>           
            <div class="form-group" runat="server" id="EmployeeNameDiv">
                <div class="col-md-2">
                    <asp:Label ID="lblEmployeeNameCaption" runat="server" class="control-label" Text="Employee Name"></asp:Label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                </div>
            </div>           
            <div id="SearchPanel" class="panel panel-default">               
                <div class="panel-heading">Search Information</div>
                <div class="panel-body">
                    <asp:GridView ID="gvLeaveInformation" Width="100%" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="5"
                        TabIndex="13" OnRowCommand="gvLeaveInformation_RowCommand" OnRowDataBound="gvLeaveInformation_RowDataBound"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="Date" ItemStyle-Width="30%">
                                <ItemTemplate>
                                    <asp:Label ID="lblOverTimeDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("OverTimeDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="50%">
                                <ItemTemplate>
                                    <asp:DropDownList ID="dpdListEstatus" runat="server" Width="150px">
                                        <asp:ListItem>--- Please Select ---</asp:ListItem>
                                        <asp:ListItem>Payment</asp:ListItem>
                                        <asp:ListItem>Substitute Leave</asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;<asp:ImageButton ID="ImgSave" runat="server" CausesValidation="False" CommandName="CmdSave"
                                        CommandArgument='<%# bind("OverTimeId") %>' ImageUrl="~/Images/save.png" Text=""
                                        AlternateText="Save" ToolTip="Save" OnClientClick="return confirm('Do you want to Save?');" />
                                    <%--      &nbsp;<asp:ImageButton ID="ImgLeave" runat="server" CausesValidation="False" CommandName="CmdLeave"
                                        CommandArgument='<%# bind("OverTimeId") %>' ImageUrl="~/Images/save.png" Text=""
                                        AlternateText="Leave" ToolTip="Leave" OnClientClick="return confirm('Do you want to Save as Leave?');" />
                                   &nbsp;&nbsp;<asp:ImageButton ID="ImgPayment" runat="server" CausesValidation="False" CommandName="CmdPayment"
                                        CommandArgument='<%# bind("OverTimeId") %>' ImageUrl="~/Images/save.png" Text=""
                                        AlternateText="Payment" ToolTip="Payment" OnClientClick="return confirm('Do you want to Save as Payment?');" />--%>
                                    <%--&nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("OverTimeId") %>' ImageUrl="~/Images/delete.png" Text=""
                                        AlternateText="Cancel" ToolTip="Cancel" OnClientClick="return confirm('Do you want to Cancel Night Audit?');" />--%>
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
        </div>
        </div>
    </div>
</asp:Content>
