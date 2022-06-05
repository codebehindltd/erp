<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmEmpLoanHoldup.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpLoanHoldup" %>

<%@ Register TagPrefix="UserControl" TagName="EmployeeSearchLoanHoldup" Src="~/HMCommon/UserControl/EmployeeSearchWithoutEmployeeType.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            $("#myTabs").tabs();

            $("#ContentPlaceHolder1_txtApprovedDate").datepicker("option", {
                changeMonth: true,
                changeYear: true,
                yearRange: "-100:+0",
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtHoldupDate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_txtHoldupDate").datepicker("option", {
                changeMonth: true,
                changeYear: true,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtApprovedDate").datepicker("option", "maxDate", selectedDate);
                }
            });
        });

        function SaveLoanHoldup() {

            var holdUpDuration = $("#<%=ddlHoldupDuration.ClientID %>").val();
            var loanHoldupForMonthOrYear = $("#<%=ddlLoanHoldupForMonthOrYear.ClientID %>").val();
            var holdupDate = $("#<%=txtHoldupDate.ClientID %>").val();
            var loanStatus = ($("#<%=ddlLoanStatus.ClientID %>").val()) == "1" ? true : false;
            var empId = $("#ContentPlaceHolder1_approvedByEmployee_hfEmployeeId").val();
            var approvedDate = $("#<%=txtApprovedDate.ClientID %>").val();
            var remarks = $("#<%=txtRemarks.ClientID %>").val();
            var approvedById = $("#ContentPlaceHolder1_approvedByEmployee_hfEmployeeId").val();
            var loanId = $("#<%=hfLoanId.ClientID %>").val();
            var loanHolpupId = $.trim($("#<%=hfLoanHoldupId.ClientID %>").val());

            if ($.trim(holdupDate) == "") {
                $("#<%=lblMessage.ClientID %>").text('Please Give holpup date');
                MessagePanelShow();
                return false;
            }

            var loanHoldup = {
                LoanHoldupId: (loanHolpupId == "" ? "0" : loanHolpupId),
                LoanId: loanId,
                EmpId: empId,
                LoanHoldupDate: holdupDate,
                DurationForLoanHoldup: holdUpDuration,
                HoldForMonthOrYear: loanHoldupForMonthOrYear,
                InstallmentNumberWhenLoanHoldup: 0,
                DueAmount: 0,
                OverDueAmount: 0,
                LoanStatus: loanStatus,
                ApprovedBy: approvedById,
                ApprovedDate: approvedDate,
                Remarks: remarks
            };

            if (loanHolpupId == "")
                PageMethods.SaveLoanHoldUp(loanHoldup, OnSaveLoanHoldUpSucceed, OnSaveLoanHoldUpFailed);
            else
                PageMethods.UpdateEmpLoan(loanHoldup, OnSaveLoanHoldUpSucceed, OnSaveLoanHoldUpFailed);

            return false;
        }

        function OnSaveLoanHoldUpSucceed(result) {
            if (result) {
                alert('Operation Successfull');
                //window.close();
                //                $("#<%=lblMessage.ClientID %>").text('Operation Successfull');
                //                $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
                //                MessagePanelShow();
            }
            else {
                alert('operation Un-Successfull');
                //                $("#<%=lblMessage.ClientID %>").text('Save operation Un-Successfull');
                //                MessagePanelShow();
            }
        }

        function OnSaveLoanHoldUpFailed(error) {

        }

        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }

        function CleanForm() {
            $("#frmHotelManagement")[0].reset();
        }

        function WorkAfterSearchEmployee() { }

    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <asp:HiddenField ID="hfLoanId" runat="server" />
    <asp:HiddenField ID="hfLoanHoldupId" runat="server" />
    <div id="EmpLoan" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Loan Information</a>
        <div class="HMBodyContainer">
            <div class="divClear">
            </div>
            <fieldset>
                <legend>Loan Info</legend>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="Label1" runat="server" Text="Loan Amount"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtLoanAmount" ReadOnly="true" runat="server"></asp:TextBox>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="lblPerInstallAmount" runat="server" Text="Installment Amount"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:TextBox ID="txtPerInstallAmount" ReadOnly="true" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblInstallmentNumberWhenLoanHoldup" runat="server" Text="Installment Number"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtInstallmentNumberWhenLoanHoldup" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="lblDueAmount" runat="server" Text="Due Amount"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:TextBox ID="txtDueAmount" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblOverDueAmount" runat="server" Text="Over Due Amount"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtOverDueAmount" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblDurationForLoanHoldup" runat="server" Text="Holdup Duration"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlHoldupDuration" runat="server" CssClass="customLargeDropDownSize">
                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                            <asp:ListItem Text="2" Value="2"> </asp:ListItem>
                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                            <asp:ListItem Text="4" Value="4"> </asp:ListItem>
                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                            <asp:ListItem Text="6" Value="6"> </asp:ListItem>
                            <asp:ListItem Text="7" Value="7"></asp:ListItem>
                            <asp:ListItem Text="8" Value="8"> </asp:ListItem>
                            <asp:ListItem Text="9" Value="9"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"> </asp:ListItem>
                            <asp:ListItem Text="11" Value="11"> </asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="lblLoanTakenForMonthOrYear" runat="server" Text="Holdup For Month/Year"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:DropDownList ID="ddlLoanHoldupForMonthOrYear" runat="server" CssClass="customLargeDropDownSize">
                            <asp:ListItem Text="Month" Value="month"></asp:ListItem>
                            <asp:ListItem Text="Year" Value="year"> </asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblLoanHoldupDate" runat="server" Text="Holdup Date"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtHoldupDate" runat="server" CssClass="datepicker"></asp:TextBox>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="Label2" runat="server" Text="Status"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:DropDownList ID="ddlLoanStatus" runat="server" CssClass="customLargeDropDownSize">
                            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                            <asp:ListItem Text="In-Active" Value="0"> </asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </fieldset>
            <div class="divClear">
            </div>
            <fieldset>
                <legend>Approved By</legend>
                <UserControl:EmployeeSearchLoanHoldup ID="approvedByEmployee" runat="server" />
            </fieldset>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblApprovedDate" runat="server" Text="Approved Date"></asp:Label>
                    <span class="MandatoryField">*</span>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtApprovedDate" runat="server" CssClass="datepicker"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblRemarks" runat="server" Text="Description"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="ThreeColumnTextBox" TextMode="MultiLine"
                        TabIndex="12"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="HMContainerRowButton">
                <asp:Button ID="btnSave" runat="server" OnClientClick="javascript:return SaveLoanHoldup()"
                    Text="Save" CssClass="TransactionalButton btn btn-primary" />
                <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary" />
            </div>
            <div class="divClear">
            </div>
        </div>
    </div>
    <div class="divClear">
    </div>
</asp:Content>
