<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmBanquetSeatingPlan.aspx.cs" Inherits="HotelManagement.Presentation.Website.Banquet.frmBanquetSeatingPlan" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        oFReader = new FileReader(), rFilter = /^image\/(?:bmp|cis\-cod|gif|ief|jpeg|pipeg|png|svg\+xml|tiff|x\-cmu\-raster|x\-cmx|x\-icon|x\-portable\-anymap|x\-portable\-bitmap|x\-portable\-graymap|x\-portable\-pixmap|x\-rgb|x\-xbitmap|x\-xpixmap|x\-xwindowdump)$/i;

        oFReader.onload = function (oFREvent) {

            document.getElementById("uploadPreview").src = oFREvent.target.result;
        };

        function loadImageFile() {
            if (document.getElementById("ContentPlaceHolder1_fuItemImage").files.length === 0) { return; }
            var oFile = document.getElementById("ContentPlaceHolder1_fuItemImage").files[0];
            if (!rFilter.test(oFile.type)) { toastr.warning("You must select a valid image file!"); return; }
            oFReader.readAsDataURL(oFile);
        }

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Banquet Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Seating Plan</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            UploadComplete();
        });



        function UploadComplete() {
            var id = $("#ContentPlaceHolder1_RandomSeatingId").val();
            ShowUploadedDocument(id);
        }

        function ShowUploadedDocument(id) {
            PageMethods.GetUploadedImageByWebMethod(id, "BanquetSeatingPlan", OnGetUploadedImageByWebMethodSucceeded, OnGetUploadedImageByWebMethodFailed);
            return false;
        }

        function OnGetUploadedImageByWebMethodSucceeded(result) {
            if (result != "") {
                $('#ImagePanel').show();
            }
            else {
                $('#ImagePanel').hide();
            }
            $('#SigDiv').html(result);
            return false;
        }
        function OnGetUploadedImageByWebMethodFailed(error) {
            toastr.error(error);
        }

        function LoadDocUploader() {
            ////alert('Item: ' + val);
            ////popup(1, 'popUpImage', '', 600, 300);
            //$("#popUpImage").dialog({
            //    width: 640,
            //    height: 300,
            //    autoOpen: true,
            //    modal: true,
            //    closeOnEscape: true,
            //    resizable: false,
            //    fluid: true,
            //    title: "", ////TODO add title
            //    show: 'slide'
            //});
            var randomId = +$("#ContentPlaceHolder1_RandomSeatingId").val();
            var path = "/Banquet/SeatingPlanImage/";
            var category = "BanquetSeatingPlan";
            var iframeid = 'frmPrint';
            //var url = "/HMCommon/FileUploadTest.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            var url = "/Common/FileUpload.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            document.getElementById(iframeid).src = url;

            $("#DocumentDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 300,
                closeOnEscape: false,
                resizable: false,
                title: "Documents Upload",
                show: 'slide'
            });
            return false;
        }




        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function OnFillFormObjectSucceeded(result) {
            $("#<%=txtName.ClientID %>").val(result.Name);
            $("#<%=txtCode.ClientID %>").val(result.Code);
            $("#<%=ddlActiveStat.ClientID %>").val(result.ActiveStat == true ? 0 : 1);
            $("#<%=txtSeatingId.ClientID %>").val(result.Id);

            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNew').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }

        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
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
            window.location = "frmSeatingPlan.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=txtName.ClientID %>").val('');
            $("#<%=txtCode.ClientID %>").val('');
            $("#<%=ddlActiveStat.ClientID %>").val(0);
            $("#<%=txtSeatingId.ClientID %>").val('');
            $("#ContentPlaceHolder1_txtDescription").val('');

            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNew').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNew').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }
        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNew').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNew').hide("slow");
        }
        $(function () {
            $("#myTabs").tabs();
        });
    </script>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Seating Plan Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Seating Plan</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Seating Plan Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="RandomSeatingId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="tempSeatingId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtSeatingId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblName" runat="server" class="control-label required-field" Text="Plan Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCode" runat="server" class="control-label required-field" Text="Plan Code"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label">Attachment</label>
                            </div>
                            <div class="col-md-4">
                                <input id="btnImageUp" type="button" onclick="javascript: return LoadDocUploader();"
                                    class="TransactionalButton btn btn-primary btn-sm" value="Sitting Plan Image..." />
                            </div>
                        </div>
                        <div id="DocumentInfo">
                        </div>
                        <div class="form-group" id="ImagePanel">
                            <div class="col-md-2">
                            </div>
                            <div class="col-md-4">
                                <div id="SigDiv">
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblActiveStat" runat="server" class="control-label"
                                    Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="form-control" TabIndex="4">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblDescription" runat="server" class="control-label" Text="Description"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="5"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="6" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="7" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearAction();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Seating Plan Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSName" runat="server" class="control-label" Text="Category Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSCode" runat="server" class="control-label" Text="Code"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSCode" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSActiveStat" runat="server" CssClass="form-control" TabIndex="3">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" TabIndex="4" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSearch_Click" />
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
                    <asp:GridView ID="gvSeatingPlan" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True" ForeColor="#333333"
                        OnPageIndexChanging="gvSeatingPlan_PageIndexChanging" OnRowDataBound="gvSeatingPlan_RowDataBound"
                        OnRowCommand="gvSeatingPlan_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="51%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Code" HeaderText="Code" ItemStyle-Width="17%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActiveStatus" HeaderText="Status" ItemStyle-Width="17%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("Id") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("Id") %>' ImageUrl="~/Images/delete.png" Text=""
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
        <div id="popUpImage" style="display: none">
            <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
                <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                    FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
            </asp:Panel>
        </div>
    </div>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <script type="text/javascript">
        var xNewAdd = '<%=isNewAddButtonEnable%>';
        if (xNewAdd > -1) {

            NewAddButtonPanelShow();
            if (parseInt(xNewAdd) == 2) {
                $('#btnNew').hide();
                $('#EntryPanel').show();
            }
        }
        else {
            NewAddButtonPanelHide();
        }

    </script>
</asp:Content>
