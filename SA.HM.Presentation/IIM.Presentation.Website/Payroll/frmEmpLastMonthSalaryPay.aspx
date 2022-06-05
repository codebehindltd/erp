<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmEmpLastMonthSalaryPay.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpLastMonthSalaryPay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Last Month Benefit Pay</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });

        $(document).ready(function () {

            $("#ContentPlaceHolder1_txtLeaveDays").change(function () {

                var empId = $("#ContentPlaceHolder1_hfEmpId").val();
                var leaveDay = $("#ContentPlaceHolder1_txtLeaveDays").val();
                var selectedMonthRange = $("#ContentPlaceHolder1_ddlEffectedMonth").val();
                var salaryYear = $("#ContentPlaceHolder1_ddlYear").val();

                PageMethods.GetLeaveBalanceAmount(empId, salaryYear, selectedMonthRange, leaveDay, OnLoadLeaveBalanceSucceed, OnLoadLeaveBalanceFailed);
            });

        });

        function SaveEmployeeBenifits() {

            var benifitId = 0;
            var empId = $("#ContentPlaceHolder1_hfEmpId").val();
            var asf = $("#ContentPlaceHolder1_txtASF").val();
            var nss17 = $("#ContentPlaceHolder1_txtNSSF17").val();
            var nss8 = $("#ContentPlaceHolder1_txtNSSF8").val();
            var leaveDays = $("#ContentPlaceHolder1_txtLeaveDays").val();
            var leaveBalance = $("#ContentPlaceHolder1_txtLeaveBalanceAmount").val();
            var selectedMonthRange = $("#ContentPlaceHolder1_ddlEffectedMonth").val();
            var salaryYear = $("#ContentPlaceHolder1_ddlYear").val();

            if (asf == "") {
                alert("Please Give ASF");
                return false;
            }
            else if (nss17 == "") {
                alert("Please Give NSS 17%");
                return false;
            }
            else if (nss8 == "") {
                alert("Please Give NSS 8%");
                return false;
            }

            if ($.trim(leaveDays) == "")
                leaveBalance = "0";

            if ($.trim(leaveBalance) == "")
                leaveBalance = "0";

            benifitId = parseInt($("#ContentPlaceHolder1_hfBenifitId").val());

            var EmpLastMonthBenifitsPaymentBO = {
                BenifitId: benifitId,
                EmpId: empId,
                AfterServiceBenefit: asf,
                EmployeePFContribution: nss8,
                CompanyPFContribution: nss17,
                LeaveBalanceDays: leaveDays,
                LeaveBalanceAmount: leaveBalance
            };

            if (benifitId == 0)
                PageMethods.SaveUpdateEmpBenefit(EmpLastMonthBenifitsPaymentBO, salaryYear, selectedMonthRange, OnLoadSaveEmpBenefitsSucceed, OnLoadSaveEmpBenefitsFailed);
            else
                PageMethods.SaveUpdateEmpBenefit(EmpLastMonthBenifitsPaymentBO, salaryYear, selectedMonthRange, OnLoadUpdateEmpBenefitsSucceed, OnLoadUpdateEmpBenefitsFailed);

            return false;
        }

        function OnLoadSaveEmpBenefitsSucceed(result) {
            if (result == true) {
                toastr.success("Save Operation Successfull.");
                $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
                $("#BenefitsDialog").dialog("close");
                ClearBenifitForm();
            }
            else {
                toastr.error("Save Operation Un-Successfull.");
            }
        }
        function OnLoadUpdateEmpBenefitsFailed(error) { }

        function OnLoadUpdateEmpBenefitsSucceed(result) {
            if (result == true) {
                toastr.success("Update Operation Successfull.");
                $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
                $("#BenefitsDialog").dialog("close");
                ClearBenifitForm();
            }
            else {
                toastr.error("Update Operation Un-Successfull.");
            }
        }
        function OnLoadSaveEmpBenefitsFailed(error) { }

        function PaymentDetails(empId) {

            ClearBenifitForm();

            $("#ContentPlaceHolder1_hfEmpId").val(empId);
            var selectedMonthRange = $("#ContentPlaceHolder1_ddlEffectedMonth").val();
            var salaryYear = $("#ContentPlaceHolder1_ddlYear").val();

            PageMethods.GetLastMonthSalaryEmployeeBenifits(empId, salaryYear, selectedMonthRange, OnLoadLastMonthSalaryEmployeeSucceed, OnLoadLastMonthSalaryEmployeeFailed);

            $("#BenefitsDialog").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Last Month Payment",
                show: 'slide'
            });
            return false;
        }

        function OnLoadLastMonthSalaryEmployeeSucceed(result) {
            $("#ContentPlaceHolder1_txtASF").val(result.AfterServiceBenefit);
            $("#ContentPlaceHolder1_txtNSSF17").val(result.CompanyPFContribution);
            $("#ContentPlaceHolder1_txtNSSF8").val(result.EmployeePFContribution);

            $("#ContentPlaceHolder1_hfBenifitId").val(result.BenifitId);

            if (result.BenifitId != 0) {
                $("#<%=btnSaveBenifits.ClientID%>").val('Update');
            }
            else {
                $("#<%=btnSaveBenifits.ClientID%>").val('Save');
            }

            if (result.LeaveBalanceDays != 0)
                $("#ContentPlaceHolder1_txtLeaveDays").val(result.LeaveBalanceDays);

            if (result.LeaveBalanceAmount != 0)
                $("#ContentPlaceHolder1_txtLeaveBalanceAmount").val(result.LeaveBalanceAmount);

        }
        function OnLoadLastMonthSalaryEmployeeFailed(error) {

        }

        function OnLoadLeaveBalanceSucceed(result) {
            $("#ContentPlaceHolder1_txtLeaveBalanceAmount").val(result.LeaveBalanceAmount);
        }
        function OnLoadLeaveBalanceFailed(error) { }

        function CloseBenifitsDialog() {
            $("#BenefitsDialog").dialog("close");
            return false;
        }

        function ClearBenifitForm() {

            $("#ContentPlaceHolder1_hfBenifitId").val("0");
            $("#ContentPlaceHolder1_hfEmpId").val("");

            $("#ContentPlaceHolder1_txtASF").val("");
            $("#ContentPlaceHolder1_txtNSSF17").val("");
            $("#ContentPlaceHolder1_txtNSSF8").val("");

            $("#ContentPlaceHolder1_txtLeaveDays").val("");
            $("#ContentPlaceHolder1_txtLeaveBalanceAmount").val("");

            $("#<%=btnSaveBenifits.ClientID%>").val('Save');
        }
    </script>
    <asp:HiddenField ID="hfEmpId" runat="server" />
    <asp:HiddenField ID="hfBenifitId" runat="server" />

    <div id="BenefitsDialog" style="display: none;">
        <div id="BenefitsAmount" class="form-horizontal">
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblAsf" CssClass="control-label" runat="server" Text="ASF"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtASF" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="Label1" runat="server" CssClass="control-label" Text="NSSF 17%"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtNSSF17" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="Label4" CssClass="control-label" runat="server" Text="NSSF 8%"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtNSSF8" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-2">
                </div>
                <div class="col-md-4">
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="Label5" CssClass="control-label" runat="server" Text="Leave Balane Days"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtLeaveDays" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="Label6" CssClass="control-label" runat="server" Text="Amount"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtLeaveBalanceAmount" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-12">
                    <asp:Button ID="btnSaveBenifits" runat="server" OnClientClick="javascript:return SaveEmployeeBenifits()"
                        Text="Save" CssClass="btn btn-primary" />
                    <asp:Button ID="btnCancel" runat="server" Text="Close" OnClientClick="javascript:return CloseBenifitsDialog()"
                        CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>

    <div class="panel panel-default">
        <div class="panel-heading">Employee List For Last Month Pay</div>
        <div class="panel-body">
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="Label3" runat="server" CssClass="control-label required-field" Text="Process Month"></asp:Label>
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
            <div class="form-group">
                <div class="col-md-12">
                    <asp:Button ID="btnLastMonthSalaryEmployeeLoad" runat="server" Text="Search Employee"
                        CssClass="btn btn-primary" OnClick="btnLastMonthSalaryEmployeeLoad_Click" />
                </div>
            </div>
        </div>
    </div>

    <div id="SearchPanel" class="form-group">
        <div class="col-md-12">
            <asp:GridView ID="gvLastMonthSalaryEmployee" Width="100%" runat="server" AllowPaging="True"
                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                ForeColor="#333333" PageSize="30" CssClass="table table-bordered table-condensed table-responsive">
                <RowStyle BackColor="#E3EAEB" />
                <Columns>
                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("EmpId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="EmpCode" HeaderText="Code" ItemStyle-Width="10%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="EmployeeName" HeaderText="Name" ItemStyle-Width="20%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="EmpType" HeaderText="Type" ItemStyle-Width="10%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Location" HeaderText="Location" ItemStyle-Width="20%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Position" HeaderText="Position" ItemStyle-Width="15%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Project" HeaderText="Project" ItemStyle-Width="10%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                CommandArgument='<%# bind("EmpId") %>' ImageUrl="~/Images/edit.png" Text="" AlternateText="Payment"
                                ToolTip="Payment Details" OnClientClick='<%# Eval("EmpId", "return PaymentDetails({0})") %>' />
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

    <script type="text/javascript">
        var xNewAdd = '<%=isNewAddButtonEnable%>';
        if (xNewAdd > -1) {
            NewAddButtonPanelShow();
        }
        else {
            NewAddButtonPanelHide();
        }
    </script>
</asp:Content>
