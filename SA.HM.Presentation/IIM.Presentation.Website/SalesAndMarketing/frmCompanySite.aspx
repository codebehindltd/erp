<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmCompanySite.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.frmCompanySite" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {

            var lblMessage = '<%=lblMessage.ClientID%>'

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Sales & Marketing</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Company Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {

            $("#<%=txtSiteName.ClientID %>").val(result.txtSiteName);
            $("#<%=txtBusinessContactName.ClientID %>").val(result.BusinessContactName);
            $("#<%=txtBusinessContactEmail.ClientID %>").val(result.BusinessContactEmail);
            $("#<%=txtBusinessContactPhone.ClientID %>").val(result.BusinessContactPhone);
            $("#<%=txtBillingContactName.ClientID %>").val(result.BillingContactName);
            $("#<%=txtBillingContactEmail.ClientID %>").val(result.BillingContactEmail);
            $("#<%=txtBillingContactPhone.ClientID %>").val(result.BillingContactPhone);
            $("#<%=txtTechnicalContactName.ClientID %>").val(result.TechnicalContactName);
            $("#<%=txtTechnicalContactEmail.ClientID %>").val(result.TechnicalContactEmail);
            $("#<%=txtTechnicalContactPhone .ClientID %>").val(result.TechnicalContactPhone);

            $("#<%=txtRemarks.ClientID %>").val(result.Remarks);

            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewGuestCompany').hide("slow");
            $('#EntryPanel').show("slow");
        }

        function OnFillFormObjectFailed(error) {
            alert(error.get_message());
        }

        //For Delete-------------------------        
        function PerformDeleteAction(actionId) {
            var answer = confirm("Do you want to delete this record?")
            if (answer) {
                PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            }
            return false;
        }

        function OnDeleteObjectSucceeded(result) {
            window.location = "frmGuestCompany.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            alert(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {

            $("#<%=txtSiteName.ClientID %>").val('');
            $("#<%=txtBusinessContactName.ClientID %>").val('');
            $("#<%=txtBusinessContactEmail.ClientID %>").val('');
            $("#<%=txtBusinessContactPhone.ClientID %>").val('');
            $("#<%=txtBillingContactName.ClientID %>").val('');
            $("#<%=txtBillingContactEmail.ClientID %>").val('');
            $("#<%=txtBillingContactPhone.ClientID %>").val('');
            $("#<%=txtTechnicalContactName.ClientID %>").val('');
            $("#<%=txtTechnicalContactEmail.ClientID %>").val('');
            $("#<%=txtTechnicalContactPhone .ClientID %>").val('');

            return false;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewGuestCompany').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewGuestCompany').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }
        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }
        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewGuestCompany').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewGuestCompany').hide("slow");
        }
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
                title: "", // TODO add title
                show: 'slide'
            });
            return false;
        }

        function UploadComplete() {
            var id = $("#ContentPlaceHolder1_RandomProductId").val();
            ShowUploadedDocument(id);
        }

        function ShowUploadedDocument(id) {
            PageMethods.GetUploadedImageByWebMethod(id, "CompanyDoc", OnGetUploadedImageByWebMethodSucceeded, OnGetUploadedImageByWebMethodFailed);
            return false;
        }

        function OnGetUploadedImageByWebMethodSucceeded(result) {
            if (result != "") {
                $('#CompanyDocumentInfo').show();
            }
            else {
                $('#CompanyDocumentInfo').hide();
            }
            $('#CompanyDocumentInfo').html(result);
            return false;
        }
        function OnGetUploadedImageByWebMethodFailed(error) {
            toastr.error("Please Contact With Admin. Upload Failed.");
        }

        function ShowCompanyDocuments(companyId) {
            PageMethods.GetDocumentsByUserTypeAndUserId(companyId, OnLoadImagesSucceeded, OnLoadImagesFailed);
            return false;
        }
        function OnLoadImagesSucceeded(result) {
            $("#imageDiv").html(result);

            $("#companyDocuments").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Company Documents",
                show: 'slide'
            });

            return false;
        }
        function OnLoadImagesFailed(error) {
            toastr.error(error.get_message());
        }
        function ConfirmEdit(SiteName) {
            if (!confirm("Do you want to edit - " + SiteName + "?")) {
                return false;
            }
        }
    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <asp:HiddenField ID="txtNodeId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfSiteId" runat="server"></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Site Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Site </a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Site Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="SiteName" class="control-label col-md-2 required-field">Company</label>
                            <div class="col-sm-4">
                                <asp:DropDownList ID="ddlCompanyId" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <label for="SiteName" class="control-label col-md-2 required-field">Site Name</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtSiteName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="BusinessContactName" class="control-label col-md-2 required-field">Business Contact Name</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtBusinessContactName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="BusinessContactEmail" class="control-label col-md-2">Business Contact Email</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtBusinessContactEmail" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                            <label for="BusinessContactPhone" class="control-label col-md-2">Business Contact Phone</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtBusinessContactPhone" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="TechnicalContactName" class="control-label col-md-2 required-field">Technical Contact Name</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtTechnicalContactName" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="TechnicalContactEmail" class="control-label col-md-2 required-field">Technical Contact Email</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtTechnicalContactEmail" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                            </div>
                            <label for="TechnicalContactPhone" class="control-label col-md-2 required-field">Technical Contact Phone</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtTechnicalContactPhone" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="BillingContactName" class="control-label col-md-2 required-field">Billing Contact Name</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtBillingContactName" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="BillingContactEmail" class="control-label col-md-2 required-field">Billing Contact Email</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtBillingContactEmail" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                            </div>
                            <label for="BillingContactPhone" class="control-label col-md-2 required-field">Billing Contact Phone</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtBillingContactPhone" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="Remarks" class="control-label col-md-2">Remarks</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="9"></asp:TextBox>
                            </div>
                        </div>

                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary btn-sm"
                        TabIndex="11" />
                    <asp:Button ID="btnClear" TabIndex="12" runat="server" Text="Clear" CssClass="btn btn-primary btn-sm"
                        OnClientClick="javascript: return PerformClearAction();" />
                </div>
            </div>

        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Site Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="SiteName" class="control-label col-md-2">Site Name</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtSSiteName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="CompanyEmail" class="control-label col-md-2">Business Contact Name</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtSBusinessContactName" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="BusinessContactEmail" class="control-label col-md-2">Business Contact Email</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtSBusinessContactEmail" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                            </div>
                            <label for="ContactPerson" class="control-label col-md-2">Business Contact Phone</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtSBusinessContactPhone" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                    CssClass="btn btn-primary btn-sm" TabIndex="9" />
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
                    <asp:GridView ID="gvGuestCompany" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                        ForeColor="#333333" PageSize="20" OnPageIndexChanging="gvGuestCompany_PageIndexChanging"
                        OnRowDataBound="gvGuestCompany_RowDataBound" OnRowCommand="gvGuestCompany_RowCommand"
                        CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("SiteId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SiteName" HeaderText="Site Name" ItemStyle-Width="35%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BusinessContactName" HeaderText="Business Contact Name" ItemStyle-Width="10%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BusinessContactEmail" HeaderText="BusinessContactEmail" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BusinessContactPhone" HeaderText="BusinessContactPhone" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" OnClientClick='<%# string.Format("return ConfirmEdit(\"{0}\");", Eval("SiteName")) %>' CommandName="CmdEdit"
                                        CommandArgument='<%# bind("SiteId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("SiteId") %>' ImageUrl="~/Images/delete.png" Text=""
                                        AlternateText="Delete" ToolTip="Delete" OnClientClick="return confirm('Do you want to Delete?');" />
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

    <div id="companyDocuments" style="display: none;">
        <div id="imageDiv"></div>
    </div>

    <script type="text/javascript">
        var xNewAdd = '<%=isNewAddButtonEnable%>';

        if (xNewAdd > -1) {

            NewAddButtonPanelShow();
            if (parseInt(xNewAdd) == 2) {
                $('#btnNewGuestCompany').hide();
                $('#EntryPanel').show();
            }
        }
        else {
            NewAddButtonPanelHide();
        }

    </script>
</asp:Content>
