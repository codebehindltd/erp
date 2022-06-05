<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmCompany.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.frmCompany" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Company Information</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Company Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            if ($("#<%=btnSaveCompany.ClientID %>").val() == "Update") {
                $('#ViewImageDiv').show();
                $('#UpImageDiv').show();
            }
            else {
                $('#ViewImageDiv').hide();
                $('#UpImageDiv').hide();
            }
            GetUploadedImage();
        });

        $(function () {
            $("#myTabs").tabs();
        });

        function LoadImageUploader() {
            //alert('Item: ' + val);
            //popup(1, 'popUpImage', '', 600, 300);
            $("#popUpImage").dialog({
                width: 650,
                height: 300,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "", //TODO add title
                show: 'slide'
            });
            return false;
        }


        function PerformClearActionCompany() {
            $("#<%=txtCompanyName.ClientID %>").val('');
            $("#<%=txtCompanyAddress.ClientID %>").val('');
            $("#<%=txtEmailAddress.ClientID %>").val('');
            $("#<%=txtWebAddress.ClientID %>").val('');
            $("#<%=txtContactPerson.ClientID %>").val('');
            $("#<%=txtContactNumber.ClientID %>").val('');
            $("#<%=txtRemarks.ClientID %>").val('');
            return false;
        }
        function PerformClearAction() {
            $("#<%=txtBankName.ClientID %>").val('');
            $("#<%=txtBranchName.ClientID %>").val('');
            $("#<%=txtSwiftCode.ClientID %>").val('');
            $("#<%=txtAccountName.ClientID %>").val('');
            $("#<%=txtAccountNo1.ClientID %>").val('');
            $("#<%=txtAccountNo2.ClientID %>").val('');
            if ($("#<%=txtBankId.ClientID %>").val() != "") {
                $("#<%=btnSaveCompanyBank.ClientID %>").val("Update");
            }
            else {
                $("#<%=btnSaveCompanyBank.ClientID %>").val("Save");
            }
            return false;
        }

        function GetUploadedImage() {
            PageMethods.GetCompanyProfileImage(GetCompanyProfileImageObjectSucceeded, OnGetCompanyProfileImageObjectFailed);
            return false;
        }


        function GetCompanyProfileImageObjectSucceeded(result) {
            $('#ImageDiv').html(result);
            return false;
        }
        function OnGetCompanyProfileImageObjectFailed(error) {
            alert(error.get_message());
        }

    </script>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Company Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:HiddenField ID="txtCompanyId" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblCompanyName" runat="server" class="control-label required-field"
                            Text="Company Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblCompanyCode" runat="server" class="control-label" Text="Company Code"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCompanyCode" runat="server" CssClass="form-control" TabIndex="36"
                            MaxLength="20"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblCompanyAddress" runat="server" class="control-label required-field"
                            Text="Address"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtCompanyAddress" runat="server" CssClass="form-control" TextMode="MultiLine"
                            TabIndex="2"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblEmailAddress" runat="server" class="control-label" Text="Company Email"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblWebAddress" runat="server" class="control-label" Text="Web Address"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtWebAddress" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblContactPerson" runat="server" class="control-label" Text="Contact Person"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblContactNumber" runat="server" class="control-label" Text="Contact Number"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtContactNumber" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblVatRegistrationNo" runat="server" class="control-label" Text="BIN Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtVatRegistrationNo" runat="server" CssClass="form-control" TabIndex="36"
                            MaxLength="30"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblTinNumber" runat="server" class="control-label" Text="TIN Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtTinNumber" runat="server" CssClass="form-control" TabIndex="36"
                            MaxLength="20"></asp:TextBox>
                    </div>                    
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                            TabIndex="7"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" id="UpImageDiv">
                    <div class="col-md-2">
                        <asp:Label ID="lbComImageImage" runat="server" class="control-label" Text="Company Logo"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <input id="btnImageUp" type="button" tabindex="8;" onclick="javascript: return LoadImageUploader();"
                            class="TransactionalButton btn btn-primary btn-sm" value="Upload Company Image" />
                    </div>
                </div>
                <div class="form-group" id="ViewImageDiv">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-10">
                        <div id="ImageDiv" style="width: 150px; height: 150px">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSaveCompany" runat="server" Text="Save" OnClick="btnSaveCompany_Click"
                            CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="8" />
                        <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return PerformClearActionCompany();" TabIndex="9"
                            Visible="false" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="popUpImage" style="display: none">
        <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
            <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
        </asp:Panel>
    </div>
    <div style="display: none;">
        <div id="myTabs">
            <ul id="tabPage" class="ui-style">
                <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-1">Company Information</a></li>
                <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-2">Company Bank Information </a></li>
            </ul>
            <div id="tab-1">
            </div>
            <div id="tab-2">
                <div id="EntryPanelBank" class="panel panel-default">
                    <div class="panel-heading">
                        Bank Information</div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:HiddenField ID="txtBankId" runat="server"></asp:HiddenField>
                                    <asp:Label ID="lblBankName" runat="server" class="control-label" Text="Bank Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblBranchName" runat="server" class="control-label" Text="Branch Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtBranchName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblSwiftCode" runat="server" class="control-label" Text="Swift Code"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtSwiftCode" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblAccountName" runat="server" class="control-label" Text="Account Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="lblAccountNo1" runat="server" class="control-label" Text="Account No"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtAccountNo1" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                                </div>
                                <div style="display: none;">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblAccountNo2" runat="server" class="control-label" Text="Account No2"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtAccountNo2" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btnSaveCompanyBank" runat="server" TabIndex="7" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                        OnClick="btnSaveCompanyBank_Click" />
                                    <asp:Button ID="btnClear" runat="server" TabIndex="8" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                        OnClientClick="javascript: return PerformClearAction();" Visible="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
