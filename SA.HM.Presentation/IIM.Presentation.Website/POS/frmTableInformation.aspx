<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmTableInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmTableInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            /*var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Restaurant Table</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);*/

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });

        function CheckValidation() {
            var tableNumber = $('#<%=txtTableNumber.ClientID%>').val();
            if ($.trim(tableNumber) == "") {
                toastr.warning("Table Numbe must not empty");
                return false;
            }
        }

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }

        function alert2(message, title, buttonText) {

            buttonText = (buttonText == undefined) ? "Ok" : buttonText;
            title = (title == undefined) ? "The page says:" : title;

            var div = $('<div>');
            div.html(message);
            div.attr('title', title);
            div.dialog({
                autoOpen: true,
                modal: true,
                draggable: false,
                resizable: false,
                buttons: [{
                    text: buttonText,
                    click: function () {
                        $(this).dialog("close");
                        div.remove();
                    }
                }]
            });
        }


        function OnFillFormObjectSucceeded(result) {
            if (result.StatusId != 2) {
                $("#<%=txtTableId.ClientID %>").val(result.TableId);
                $("#<%=txtTableNumber.ClientID %>").val(result.TableNumber);
                $("#<%=txtRemarks.ClientID %>").val(result.Remarks);
                $("#<%=btnSave.ClientID %>").val("Update");
                $('#btnNewTableNumber').hide("slow");
                $('#EntryPanel').show("slow");
            }
            else {
                toastr.info("This is not edditable.");
            }
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
            CommonHelper.AlertMessage(result.AlertMessage);
        }

        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearActionForButton() {
             if (!confirm("Do you want to clear?")) {
                return false;
            }
        }
        function PerformClearAction() {
            $("#<%=txtTableNumber.ClientID %>").val('');
            $("#<%=txtTableCapacity.ClientID %>").val('');
            $("#<%=txtTableId.ClientID %>").val('');
            $("#<%=txtRemarks.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewTableNumber').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewTableNumber').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }

        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewTableNumber').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewTableNumber').hide("slow");
        }
        $(function () {
            $("#myTabs").tabs();
        });
    </script>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Restaurant Table Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Table </a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Table Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <%--<div class="col-md-2">                                
                                <asp:Label ID="lblTableNumber" runat="server" class="control-label" Text="Table Number"></asp:Label>
                            </div>--%>
                            <asp:HiddenField ID="txtTableId" runat="server"></asp:HiddenField>
                            <label for="TableNumber" class="control-label col-md-2">Table Number</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTableNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <%--<div class="col-md-2">
                                <asp:Label ID="lblTableCapacity" runat="server" class="control-label" Text="Table Capacity"></asp:Label>
                            </div>--%>
                            <label for="TableCapacity" class="control-label col-md-2">Table Capacity</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTableCapacity" TabIndex="1" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <%--<div class="col-md-2">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Remarks"></asp:Label>
                            </div>--%>
                            <label for="Remarks" class="control-label col-md-2">Remarks</label>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" TabIndex="4" Text="Save" CssClass="btn btn-primary btn-sm"
                                    OnClientClick="javascript:return CheckValidation();" OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="5" CssClass="btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearActionForButton();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Search Table Number</div>
                <div class="panel-body">
                    <div class="HMBodyContainer">
                        <div class="form-horizontal">
                            <%--<div class="col-md-2">
                                <asp:Label ID="lblSTableNumber" runat="server" class="control-label" Text="Table Number"></asp:Label>
                            </div>--%>
                            <label for="TableNumber" class="control-label col-md-2">Table Number</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSTableNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btnSearch" runat="server" TabIndex="4" Text="Search" CssClass="btn btn-primary btn-sm"
                                        OnClick="btnSearch_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information</div>
                <div class="panel-body">
                    <asp:GridView ID="gvTableNumber" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="30"
                        OnPageIndexChanging="gvTableNumber_PageIndexChanging" OnRowDataBound="gvTableNumber_RowDataBound"
                        OnRowCommand="gvTableNumber_RowCommand" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("TableId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TableNumber" HeaderText="Table Number" ItemStyle-Width="50%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <%--<asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="35%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>--%>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" OnClientClick="return confirm('Do you want to edit?');" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("TableId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("TableId") %>' ImageUrl="~/Images/delete.png" Text=""
                                        AlternateText="Delete" ToolTip="Delete" OnClientClick="return confirm('Do you want to delete?');" />
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
    <script type="text/javascript">
        var xNewAdd = '<%=isNewAddButtonEnable%>';

        if (xNewAdd > -1) {

            NewAddButtonPanelShow();
            if (parseInt(xNewAdd) == 2) {
                $('#btnNewTableNumber').hide();
                $('#EntryPanel').show();
            }
        }
        else {
            NewAddButtonPanelHide();
        }
        
    </script>
</asp:Content>
