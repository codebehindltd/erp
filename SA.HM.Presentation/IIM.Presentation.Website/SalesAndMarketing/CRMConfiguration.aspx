<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="CRMConfiguration.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.CRMConfiguration" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .mycheckbox input[type="checkbox"] {
            margin-right: 10px;
            padding-top: 5px;
        }
    </style>
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            /*var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Configuration</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);*/

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

        });
        $(function () {
            $("#myTabs").tabs();
        });
        function CheckPrefixLength(control) {
            const charLength = $(control).val().length;
            if (charLength > 0 && charLength != 3) {
                toastr.warning("Enter 3 Character.");
                $(control).focus();
            }
        }
        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }
    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            CRM Configuration
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="col-md-6">
                    <div class="row">
                        <div class="col-md-12">
                            <div id="CommonInformation" class="panel panel-default" runat="server">
                                <div class="panel-heading">
                                    Common Information
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-md-5">
                                                <label class="control-label">Technical Department</label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:HiddenField ID="hfTechnicalDepartment" runat="server"></asp:HiddenField>
                                                <asp:DropDownList ID="TechnicalDepartment" runat="server" CssClass="form-control" TabIndex="3">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-5">
                                                <label class="control-label">Company Number Prefix</label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:HiddenField ID="hfSalesCompanyNumberPrefix" runat="server"></asp:HiddenField>
                                                <asp:TextBox ID="SalesCompanyNumberPrefix" runat="server" CssClass="form-control" onblur="CheckPrefixLength(this)"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-5">
                                                <label class="control-label">Contact Number Prefix</label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:HiddenField ID="hfSalesContactNumberPrefix" runat="server"></asp:HiddenField>
                                                <asp:TextBox ID="SalesContactNumberPrefix" runat="server" CssClass="form-control" onblur="CheckPrefixLength(this)"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-5">
                                                <label class="control-label">Deal Number Prefix</label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:HiddenField ID="hfSalesDealNumberPrefix" runat="server"></asp:HiddenField>
                                                <asp:TextBox ID="SalesDealNumberPrefix" runat="server" CssClass="form-control" onblur="CheckPrefixLength(this)"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsSalesNoteEnable" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsSalesNoteEnable" runat="server" CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Sales Note Enable?
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsCRMAreaFieldEnable" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsCRMAreaFieldEnable" runat="server" CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Area Field Enable?
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div id="DealStageConfig" class="panel panel-default" runat="server">
                                <div class="panel-heading">
                                    Deal Stage Configuration
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsDealStageCanChangeMoreThanOneStep" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsDealStageCanChangeMoreThanOneStep" runat="server" CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Deal Stage Change More Than One Step?
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsDealStageChangedToClosedWonAutomaticallyAfterApprovalFromAccounts" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsDealStageChangedToClosedWonAutomaticallyAfterApprovalFromAccounts" runat="server" CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Deal Stage Changed to Closed Won Automatically After Approval from Accounts?
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div id="ContactInformation" class="panel panel-default" runat="server">
                                <div class="panel-heading">
                                    Contact Information
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsContactInformationRestrictedForAllUsers" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsContactInformationRestrictedForAllUsers" runat="server" CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Contact Information Restricted For All Users?
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsContactHyperlinkEnableFromGrid" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsContactHyperlinkEnableFromGrid" runat="Server"
                                                        CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Contact Hyperlink Enable From Grid?
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div id="DealInformation" class="panel panel-default" runat="server">
                                <div class="panel-heading">
                                    Deal Information
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsSegmentNameWillShow" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsSegmentNameWillShow" runat="server" CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Segment Name will show?
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsProductInformationWillShow" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsProductInformationWillShow" runat="server" CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Product Information will show?
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsServiceInformationWillShow" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsServiceInformationWillShow" runat="server" CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Service Information will show?
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsDealRestrictedForAllUsers" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsDealRestrictedForAllUsers" runat="server" CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Deal Information Restricted For All Users?
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="row">
                        <div class="col-md-12">
                            <div id="CompanyInformation" class="panel panel-default" runat="server">
                                <div class="panel-heading">
                                    Company Information
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsShippingAddresswillshow" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsShippingAddresswillshow" runat="server" CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Shipping Address will show?
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsDiscountPercentageWillShow" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsDiscountPercentageWillShow" runat="server" CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Discount (%) will show?
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsCreditLimitWillShow" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsCreditLimitWillShow" runat="server" CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Credit Limit will show?
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsNumberOfEmployeeWillShow" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsNumberOfEmployeeWillShow" runat="server" CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Number Of Employee will show?
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsAnnualRevenueWillShow" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsAnnualRevenueWillShow" runat="server" CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Annual Revenue will show?
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsHotelGuestCompanyRestrictionForAllUsers" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsHotelGuestCompanyRestrictionForAllUsers" runat="Server"
                                                        CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Company Information Restricted For All Users?
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsCRMCompanyNumberEnable" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsCRMCompanyNumberEnable" runat="Server"
                                                        CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Company Number Enable?
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsCompanyHyperlinkEnableFromGrid" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsCompanyHyperlinkEnableFromGrid" runat="Server"
                                                        CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Company Hyperlink Enable From Grid?
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div id="QuotationInformation" class="panel panel-default" runat="server">
                                <div class="panel-heading">
                                    Quotation Information
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsDeviceOrUserWillShow" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsDeviceOrUserWillShow" runat="server" CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Device/User will show?
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsDeliveryWillShow" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsDeliveryWillShow" runat="server" CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Delivery will show?
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsQuotationCreateFromSiteServeyFeedback" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsQuotationCreateFromSiteServeyFeedback" runat="server" CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Quotation Create from Site Servey Feedback?
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div id="LifeCycleStageInformation" class="panel panel-default" runat="server">
                                <div class="panel-heading">
                                    Life Cycle Stage Information
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-12">
                                                    <asp:HiddenField ID="hfIsLifeCycleStageCanChangeMoreThanOneStep" runat="server"></asp:HiddenField>
                                                    <asp:CheckBox ID="IsLifeCycleStageCanChangeMoreThanOneStep" runat="server" CssClass="mycheckbox" />
                                                    &nbsp;&nbsp;Is Life Cycle Stage Can Change More Than One Step?
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-12">
                    <asp:Button ID="btnUpdate" runat="server" TabIndex="3" Text="Update" CssClass="TransactionalButton btn btn-primary btn-sm"
                        OnClick="btnUpdate_Click" />
                </div>
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
