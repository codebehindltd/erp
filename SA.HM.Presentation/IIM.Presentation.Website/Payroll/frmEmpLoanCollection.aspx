<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmEmpLoanCollection.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpLoanCollection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            
            var hiddenId = $("#<%=hfLoanCollectionId.ClientID %>").val();
            if (hiddenId > 0) {
                $("#<%=btnSave.ClientID %>").val("Update");
            }
        });

        function SaveLoanCollection() {

            var loanId = $("#<%=hfLoanId.ClientID %>").val();
            var installmentNumber = $("#<%=txtInstallmentNumber.ClientID %>").val();
            var collectionDate = $("#<%=txtCollectDate.ClientID %>").val();
            var collectedLoanAmount = $("#<%=txtPerInstallLoanAmount.ClientID %>").val();
            var collectedinterestAmount = $("#<%=txtPerInstallInterestAmount.ClientID %>").val();
            var empId = $("#<%= hfEmployeeId.ClientID %>").val();

            var collectionId = $("#<%=hfLoanCollectionId.ClientID %>").val();
            if (collectionId == "")
                collectionId = "0";

            var loanCollection = {
                CollectionId: collectionId,
                LoanId: loanId,
                EmpId: empId,
                InstallmentNumber: installmentNumber,
                CollectionDate: collectionDate,
                CollectedLoanAmount: collectedLoanAmount,
                CollectedInterestAmount: collectedinterestAmount
            };

            if (collectionId != "0") {
                PageMethods.UpdateLoanCollection(loanCollection, OnUpdateLoanCollectionSucceed, OnUpdateLoanCollectionFailed);
            }
            else {
                PageMethods.SaveLoanCollection(loanCollection, OnSaveLoanCollectionSucceed, OnSaveLoanCollectionFailed);
            }

            return false;
        }

        function OnSaveLoanCollectionSucceed(result) {
            $("#<%=btnSave.ClientID %>").val("Update");
        }

        function OnSaveLoanCollectionFailed(error) {

        }

        function OnUpdateLoanCollectionSucceed(result) {

        }

        function OnUpdateLoanCollectionFailed(error) {

        }
               
    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <asp:HiddenField ID="hfLoanCollectionId" runat="server" Value="" />
    <asp:HiddenField ID="hfLoanId" runat="server" Value="" />
    <asp:HiddenField ID="hfEmployeeId" runat="server" Value="" />
    <div id="EmpLoan" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Loan Collection Information</a>
        <div class="HMBodyContainer">
            <div class="divClear">
            </div>
            <fieldset>
                <legend>Loan Collection Info</legend>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblInstallmentNumber" runat="server" Text="Installment Number"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtInstallmentNumber" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="Label1" runat="server" Text="Loan Amount"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:TextBox ID="txtLoanAmount" ReadOnly="true" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblPerInstallAmount" runat="server" Text="Installment Loan Amount"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtPerInstallLoanAmount" ReadOnly="true" runat="server"></asp:TextBox>
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
                        <asp:Label ID="Label2" runat="server" Text="Installment Interest Amount"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtPerInstallInterestAmount" ReadOnly="true" runat="server"></asp:TextBox>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="lblOverDueAmount" runat="server" Text="Over Due Amount"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:TextBox ID="txtOverDueAmount" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
            </fieldset>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblCollectAmount" runat="server" Text="Total Collection Amount"></asp:Label>
                    <span class="MandatoryField">*</span>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtCollectAmount" runat="server" TabIndex="3"></asp:TextBox>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblCollectDate" runat="server" Text="Collection Date"></asp:Label>
                    <span class="MandatoryField">*</span>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:TextBox ID="txtCollectDate" runat="server" CssClass="datepicker" TabIndex="2"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="HMContainerRowButton">
                <asp:Button ID="btnSave" runat="server" OnClientClick="javascript:return SaveLoanCollection()"
                    Text="Save" CssClass="TransactionalButton btn btn-primary" OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                    OnClick="btnCancel_Click" />
            </div>
            <div class="divClear">
            </div>
        </div>
    </div>
    <div class="divClear">
    </div>
</asp:Content>
