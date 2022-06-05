<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmSalesContractDetails.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesManagment.frmSalesContractDetails" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {


            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Sales</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Sales Contact</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            var txtStartDate = '<%=txtSigningDate.ClientID%>'
            var txtEndDate = '<%=txtExpiryDate.ClientID%>'

            var ddlCustomerId = '<%=ddlCustomerId.ClientID%>'
            var customerId = $('#' + ddlCustomerId).val();
            LoadGuestImage(customerId);

            $('#' + ddlCustomerId).change(function () {

                var customerId = $('#' + ddlCustomerId).val();
                LoadGuestImage(customerId);
            });

            $('#' + txtStartDate).datepicker("option", {
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtEndDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtEndDate).datepicker("option", {
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtStartDate).datepicker("option", "maxDate", selectedDate);
                }
            });
        });

        function LoadGuestImage(guestId) {
            PageMethods.GetDocumentsByUserTypeAndUserId(guestId, OnLoadImagesSucceeded, OnLoadImagesFailed);
            return false;
        }
        function OnLoadImagesSucceeded(result) {
            $("#ImageDiv").html(result);
            return false;
        }
        function OnLoadImagesFailed(error) {
            alert(error.get_message());
        }
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

        function LoadImageUploader() {
            //alert('Item: ' + val);
            popup(1, 'popUpImage', '', 600, 300);
            return false;
        }

        function UploadComplete() {
            popup(-1);

            var RandomOwnerId = '<%=RandomOwnerId.ClientID%>'
            var id = $('#' + RandomOwnerId).val();
            LoadGuestImage(id);
        }

        function DeleteImage(ownerId) {
            PageMethods.DeleteCustomerImage(ownerId, OnDeleteCustomerImageSucceed, OnDeleteCustomerImageFailed);
        }
        function OnDeleteCustomerImageSucceed(result) {
            MessagePanelShow();
            $("#ContentPlaceHolder1_lblMessage").text(result);
            $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
            $("#ImageDiv").html("");
        }
        function OnDeleteCustomerImageFailed(error) {
            MessagePanelHide();
        }

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
                href="#tab-1">Sales Contact</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Sales Contact Details
                </a>
                <div class="HMBodyContainer">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:HiddenField ID="RandomOwnerId" runat="server" />
                            <asp:HiddenField ID="txtCompanyId" runat="server"></asp:HiddenField>
                            <asp:Label ID="lblCustomerId" runat="server" Text="Customer Name"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlCustomerId" runat="server" CssClass="ThreeColumnTextBox"
                                TabIndex="1">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblSigningDate" runat="server" Text="Signing Date"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtSigningDate" runat="server" CssClass="datepicker" TabIndex="5"></asp:TextBox>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblExpiryDate" runat="server" Text="Expiry Date"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="datepicker" TabIndex="3"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblDocumentName" runat="server" Text="Document"></asp:Label>
                            <span class="MandatoryField">*</span>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <input id="btnImageUp" type="button" onclick="javascript: return LoadImageUploader();"
                                class="TransactionalButton btn btn-primary" value="Upload Company Image" />
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="HMContainerRowButton">
                        <%--Right Left--%>
                        <asp:Button ID="btnSaveCompany" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                            TabIndex="8" OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                            TabIndex="9" Visible="false" />
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div id="ImageDiv">
                </div>
                <div class="divClear">
                </div>
            </div>
        </div>
        <div id="popUpImage" style="display: none">
            <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
                <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                    FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
            </asp:Panel>
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
