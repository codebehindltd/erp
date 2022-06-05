<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmEmpSalaryProcess.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpSalaryProcess" %>

<%@ Register TagPrefix="UserControl" TagName="companyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>
<%@ Register TagPrefix="UserControl" TagName="srcCompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Monthly Salary Process</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#myTabs").tabs();

        });
        function CheckValidation() {

            var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

            if (companyId == null) {
                companyId = "1";
            }

            if (projectId == null) {
                projectId = "1";
            }

            if (projectId == "0") {
                projectId = "1";
            }

            if (companyId == "0") {
                if ($("#ContentPlaceHolder1_hfIsCompanyProjectWiseEmployeeSalaryProcessEnable").val() == "1") {
                    toastr.warning("Please Select Company.");
                }

                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").focus();
                return false;
            }

            if (companyId != "0") {
                if (projectId == "0") {
                    if ($("#ContentPlaceHolder1_hfIsCompanyProjectWiseEmployeeSalaryProcessEnable").val() == "1") {
                        toastr.warning("Please Select Payroll Project.");
                    }
                    $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").focus();
                    return false;
                }
            }

            $("#ContentPlaceHolder1_hfCompanyId").val(companyId);
            $("#ContentPlaceHolder1_hfProjectId").val(projectId);

            if ($("#ContentPlaceHolder1_ddlEffectedMonth").val() == "0") {
                toastr.warning("Please Select Process Month");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlYear").val() == "0") {
                toastr.warning("Please Select Process Year");
                return false;
            }

            return true;
        }

        function ValidateSearch() {
            var companyId = $("#ContentPlaceHolder1_srcCompanyProjectUserControl_ddlGLCompany").val();
            var projectId = $("#ContentPlaceHolder1_srcCompanyProjectUserControl_ddlGLProject").val();

            if (companyId == null) {
                companyId = "1";
            }

            if (projectId == null) {
                projectId = "1";
            }

            if (projectId == "0") {
                projectId = "1";
            }

            if (companyId == "0") {
                if ($("#ContentPlaceHolder1_hfIsCompanyProjectWiseEmployeeSalaryProcessEnable").val() == "1") {
                    toastr.warning("Please Select Company.");
                }

                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").focus();
                return false;
            }

            if (companyId != "0") {
                if (projectId == "0") {
                    if ($("#ContentPlaceHolder1_hfIsCompanyProjectWiseEmployeeSalaryProcessEnable").val() == "1") {
                        toastr.warning("Please Select Payroll Project.");
                    }
                    $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").focus();
                    return false;
                }
            }

            $("#ContentPlaceHolder1_hfSrcCompanyId").val(companyId);
            $("#ContentPlaceHolder1_hfSrcProjectId").val(projectId);

            if ($("#ContentPlaceHolder1_ddlSEffectedMonth").val() == "0") {
                toastr.warning("Please Select Process Month");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlSYear").val() == "0") {
                toastr.warning("Please Select Process Year");
                return false;
            }

            return true;
        }

    </script>   
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0" />
    <asp:HiddenField ID="hfProjectId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSrcCompanyId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSrcProjectId" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsCompanyProjectWiseEmployeeSalaryProcessEnable" runat="server" Value="0" />    
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Salary Process</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Salary Process</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Employee Salary Process Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <UserControl:companyProjectUserControl ID="companyProjectUserControl" runat="server" />
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblProcessDate" runat="server" class="control-label required-field" Text="Process Month"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlEffectedMonth" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Process Year"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlYear" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>                        
                        <div class="row">
                            <div class="col-md-12">
                                <%--Right Left--%>
                                <asp:Button ID="btnSalryProcess" runat="server" Text="Salary Process" CssClass="btn btn-primary"
                                    OnClick="btnSalryProcess_Click" OnClientClick="javascript:return CheckValidation();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Search Salary Process
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <UserControl:srcCompanyProjectUserControl ID="srcCompanyProjectUserControl" runat="server" />
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Process Month"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSEffectedMonth" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Process Year"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSYear" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="Label5" runat="server" class="control-label" Text="Currency Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCurrencyType" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Individual Currency" Value="IndividualCurrency"></asp:ListItem>
                                    <asp:ListItem Text="Local Currency" Value="LocalCurrency"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <%--Right Left--%>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnSearch_Click" OnClientClick="javascript:return ValidateSearch()"
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
                    <asp:GridView ID="gvSalaryFormula" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True"
                        ForeColor="#333333" OnRowDataBound="gvSalaryFormula_RowDataBound" OnRowCommand="gvSalaryFormula_RowCommand"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("ProcessId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Process Date" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="Label115" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ProcessDate"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Salary Date From" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="Label15" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("SalaryDateFrom"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Salary Date To" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="Label225" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("SalaryDateTo"))) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ProcessSequence" HeaderText="Process Sequence" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgApproved" runat="server" CausesValidation="False" CommandName="CmdApproved"
                                        CommandArgument='<%# bind("ProcessId") %>' ImageUrl="~/Images/approved.png" OnClientClick="return confirm('Do you want to Approved?');"
                                        AlternateText="Approved" ToolTip="Approved Salary Process" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ImgShowStatement" runat="server" CausesValidation="False"
                                        CommandName="CmdShowStatement" CommandArgument='<%# bind("ProcessId") %>' ImageUrl="~/Images/ReportDocument.png"
                                        AlternateText="Statement" ToolTip="Salary Statement" />
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
</asp:Content>
