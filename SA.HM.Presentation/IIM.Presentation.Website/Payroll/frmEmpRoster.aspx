<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpRoster.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpRoster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Attendance & Time Keeping</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Roster</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });

        $(document).ready(function () {
            var ddlRosterId = '<%=ddlRosterId.ClientID%>'
            var txtSrcRoomNumber = '<%=txtSrcRoomNumber.ClientID%>'
            var ddlEmpId = '<%=ddlEmpId.ClientID%>'
            var txtEmployeeNameInfo = '<%=txtEmployeeNameInfo.ClientID%>'
            $('#' + ddlRosterId).change(function () {
                var ddlRosterIdSelectedIndex = parseFloat($('#' + ddlRosterId).prop("selectedIndex"));
                if (ddlRosterIdSelectedIndex != 0) {
                    $('#' + txtSrcRoomNumber).val("")
                    $('#' + ddlEmpId).val("0")
                    $('#' + txtEmployeeNameInfo).val("")
                    $('#EmpRosterGridInformation').hide();
                }
                else {
                    $('#EmpRosterGridInformation').show();
                }
            });
        });
        function CheckValidity() {
            var grid = document.getElementById("<%=gvEmpRoster.ClientID %>");

            for (var i = 0; i < grid.rows.length - 1; i++) {
                var slab1 = $("#ContentPlaceHolder1_gvEmpRoster_ItemDropDown_" + i).val()
                var slab2 = $("#ContentPlaceHolder1_gvEmpRoster_SecondTimeSlabId_" + i).val()
                if (slab1 != "0" && slab2 != "0") {
                    if (slab1 == slab2) {
                        toastr.warning("Same Time Slab Cannot Be Given");
                        return false;
                    }
                }
                
            }
            return true;
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=ddlEmpId.ClientID %>").val(0);
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }        
    </script>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Employee Roster Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:HiddenField ID="txtEmpRosterId" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblRoster" runat="server" class="control-label required-field" Text="Roster"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlRosterId" runat="server" TabIndex="1" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblEmpId" runat="server" class="control-label required-field" Text="Employee ID"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSrcRoomNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                        <asp:DropDownList ID="ddlEmpId" runat="server" CssClass="form-control" AutoPostBack="True"
                            TabIndex="1" OnSelectedIndexChanged="ddlEmpId_SelectedIndexChanged" Visible="false">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnSrcEmployees" runat="server" Text="Search" TabIndex="2" CssClass="TransactionalButton btn btn-primary"
                            OnClick="btnSrcEmployees_Click" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Employee Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtEmployeeNameInfo" runat="server" CssClass="form-control" TabIndex="1"
                            Enabled="False"></asp:TextBox>
                    </div>
                </div>
                <div id="EmpRosterGridInformation" style="display: none;">
                    <div class="panel-body">
                        <asp:GridView ID="gvEmpRoster" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True" ForeColor="#333333"
                            OnRowDataBound="gvEmpRoster_RowDataBound" CssClass="table table-bordered table-condensed table-responsive">
                            <RowStyle BackColor="#E3EAEB" />
                            <Columns>
                                <%--<asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("DepartmentId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Schedule Date" ItemStyle-Width="50%" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblgvShowRosterDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("RosterDate").ToString())).ToString("dddd") +", "+ GetStringFromDateTime(Convert.ToDateTime(Eval("RosterDate").ToString())).ToString("MMMM d, yyyy") %>'></asp:Label>--%>
                                        <asp:Label ID="lblgvShowRosterDate" runat="server" Text='<%# DateTime.Parse(Eval("RosterDate").ToString()).ToString("dddd") +", "+ DateTime.Parse(Eval("RosterDate").ToString()).ToString("MMMM d, yyyy") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Schedule Date" ItemStyle-Width="50%" ItemStyle-HorizontalAlign="Left"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvRosterDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("RosterDate"))) %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Time Slab 1" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ItemDropDown" CssClass="form-control" runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Time Slab 2" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="SecondTimeSlabId" CssClass="form-control" runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
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
                    <div class="row">
                        <div class="col-md-12">
                            <%--Right Left--%>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                                OnClick="btnSave_Click" OnClientClick="javascript:return CheckValidity()"/>
                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                                OnClientClick="javascript: return PerformClearAction();" Visible="False" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--</div>--%>
    <script type="text/javascript">
        var isEnableEmpRosterGrid = '<%=isEnableEmpRosterGrid%>';
        if (isEnableEmpRosterGrid > -1) {
            $('#EmpRosterGridInformation').show();
        }
        else {
            $('#EmpRosterGridInformation').hide();
        }
    </script>
</asp:Content>
