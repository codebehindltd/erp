<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmMemberPaymentConfiguration.aspx.cs" Inherits="HotelManagement.Presentation.Website.Membership.frmMemberPaymentConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Membership</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Member Payment Configuration</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            CommonHelper.AutoSearchClientDataSource("txtMemberCode", "ContentPlaceHolder1_ddlMember", "ContentPlaceHolder1_hfMemberId");

            if ($("#ContentPlaceHolder1_ddlTransactionType").val() == "Individual") {
                $("#IndividualDiv1").show();
                $("#IndividualDiv2").show();
                $("#TypeDiv").hide();
            }
            else {
                $("#IndividualDiv1").hide();
                $("#IndividualDiv2").hide();
                $("#TypeDiv").show();
            }

            $('#ContentPlaceHolder1_ddlTransactionType').change(function () {

                if ($(this).val() == "Individual") {
                    $("#IndividualDiv1").show();
                    $("#IndividualDiv2").show();
                    $("#TypeDiv").hide();
                }
                else {
                    $("#IndividualDiv1").hide();
                    $("#IndividualDiv2").hide();
                    $("#TypeDiv").show();
                }
            });

            $('#ContentPlaceHolder1_ddlMemberType').change(function () {
                var transactionType = $("#<%=ddlTransactionType.ClientID %>").val();
                var typeId = $("#<%=ddlMemberType.ClientID %>").val();
                PageMethods.GetMemberPaymentConfigInfo(transactionType, typeId, OnLoadSucceeded, OnLoadFailed);
            });
            $('#txtMemberCode').blur(function () {
                $("#MemName").show();
                var transactionType = $("#<%=ddlTransactionType.ClientID %>").val();
                var memberId = $("#<%=hfMemberId.ClientID %>").val();
                PageMethods.GetMemberPaymentConfigInfo(transactionType, memberId, OnLoadSucceeded, OnLoadFailed);
            });

            $("#myTabs").tabs();

            $('#ContentPlaceHolder1_txtBillStartDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtDoorStartDate').datepicker("option", "minDate", selectedDate);
                }
            });
            $('#ContentPlaceHolder1_txtDoorStartDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtBillStartDate').datepicker("option", "maxDate", selectedDate);
                }
            });
        });

        function OnLoadSucceeded(result) {
            if (result != null) {
                $("#<%=hfPaymentConfigId.ClientID %>").val(result.MemPaymentConfigId);
                $("#<%=ddlBillingPeriod.ClientID %>").val(result.BillingPeriod);
                $("#<%=txtBillingAmount1.ClientID %>").val(result.BillingAmount);
                $("#<%=txtBillStartDate.ClientID %>").val(GetStringFromDateTime(result.BillingStartDate));
                $("#<%=txtDoorStartDate.ClientID %>").val(GetStringFromDateTime(result.DoorStartDate));
                $("#<%=txtMembername.ClientID %>").val(result.MemberName);
                $("#<%=btnSave.ClientID %>").val("Update");
            }
            else {
                $("#<%=hfPaymentConfigId.ClientID %>").val("");
                $("#<%=ddlBillingPeriod.ClientID %>").val("Weekly");
                $("#<%=txtBillingAmount1.ClientID %>").val("");
                $("#<%=txtBillStartDate.ClientID %>").val("");
                $("#<%=txtDoorStartDate.ClientID %>").val("");
                $("#<%=btnSave.ClientID %>").val("Save");

                var transactionType = $("#<%=ddlTransactionType.ClientID %>").val();
                if (transactionType == "Individual") {
                    var memberId = $("#<%=hfMemberId.ClientID %>").val();
                    PageMethods.GetMemberPaymentConfigInfoByMemberId(memberId, OnLoadDataSucceeded, OnLoadDataFailed);
                }
            }
        }

        function OnLoadFailed() {
        }

        function OnLoadDataSucceeded(result) {
            if (result != null) {
                $("#<%=ddlBillingPeriod.ClientID %>").val(result.BillingPeriod);
                $("#<%=txtBillingAmount1.ClientID %>").val(result.BillingAmount);
                $("#<%=txtMembername.ClientID %>").val(result.MemberName);
            }
        }
        function OnLoadDataFailed() {
        }
       
    </script>
    <asp:HiddenField ID="hfPaymentConfigId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfMemberId" runat="server"></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Payment Config. Entry</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Payment Configuration Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblTransactionType" runat="server" class="control-label" Text="Transaction Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlTransactionType" runat="server" CssClass="form-control"
                                    TabIndex="4">
                                    <asp:ListItem Value="MemberType">Member Type</asp:ListItem>
                                    <asp:ListItem Value="Individual">Individual</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="TypeDiv">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblMemberType" runat="server" class="control-label required-field"
                                        Text="Member Type"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlMemberType" runat="server" CssClass="form-control" TabIndex="1">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div id="IndividualDiv1">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblMember" runat="server" class="control-label required-field" Text="Member"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <input id="txtMemberCode" type="text" class="form-control" name="memCode" />
                                    <div style="display: none;">
                                        <asp:DropDownList ID="ddlMember" TabIndex="1" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div id="MemName" style="display:none;">
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtMembername" runat="server" CssClass="form-control" TabIndex="1" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblBillingPeriod" runat="server" class="control-label required-field"
                                    Text="BIlling Period"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlBillingPeriod" runat="server" CssClass="form-control" TabIndex="4">
                                    <asp:ListItem Value="Daily">Daily</asp:ListItem>
                                    <asp:ListItem Value="Weekly">Weekly</asp:ListItem>
                                    <asp:ListItem Value="Monthly">Monthly</asp:ListItem>
                                    <asp:ListItem Value="Quarterly">Quarterly</asp:ListItem>
                                    <asp:ListItem Value="HalfYearly">Half Yearly</asp:ListItem>
                                    <asp:ListItem Value="Yearly">Yearly</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblBillingAmount" runat="server" class="control-label required-field"
                                    Text="BIlling Amount"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtBillingAmount1" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div id="IndividualDiv2">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblBillStartDate" runat="server" class="control-label" Text="Billing Start Date"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtBillStartDate" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblDoorStartDate" runat="server" class="control-label" Text="Door Start Date"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDoorStartDate" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <%--Right Left--%>
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                                    TabIndex="10" OnClick="btnSave_Click" OnClientClick="javascript:return ValidateFormula();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
