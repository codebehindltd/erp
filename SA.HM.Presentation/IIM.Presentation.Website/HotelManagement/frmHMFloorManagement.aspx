<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmHMFloorManagement.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmHMFloorManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Floor Management</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            if ($("#ContentPlaceHolder1_hfNewEntryShow").val() == "1") {
                EntryPanelVisibleTrue();
            }
        });

        function GenerateCoordinates() {
            var fullStringValue = '';
            $("#<%=txtFloorWiseRoomAllocationInfo.ClientID %>").val('');
            $('#FloorRoomAllocation').children().each(function () {
                if ($(this).hasClass('draggable')) {
                    var id = this.id;
                    var divX = $('#' + id).position().left;
                    var divY = $('#' + id).position().top;
                    var divWidth = $('#' + id).width();
                    var divHeight = $('#' + id).height();

                    //var strValue = 'id:' + id + ' X:' + divX + ' Y:' + divY + ' W:' + divWidth + ' H:' + divHeight + '|';
                    var strValue = id + ',' + divX + ',' + divY + ',' + divWidth + ',' + divHeight + '|';

                    fullStringValue = $("#<%=txtFloorWiseRoomAllocationInfo.ClientID %>").val() + strValue;
                    $("#<%=txtFloorWiseRoomAllocationInfo.ClientID %>").val(fullStringValue);
                }
            });
            return true;
        }

        function EntryPanelVisibleTrue() {
            $('#EntryPanel').show();
            $("#ContentPlaceHolder1_hfNewEntryShow").val("1");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#EntryPanel').hide();
            $("#ContentPlaceHolder1_hfNewEntryShow").val("0");
            return false;
        }

        function RoomInformationShow() {

            $("#RoomInformation").dialog({
                width: 1100,
                height: 600,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Room Information",
                show: 'slide'
            });

            return false;
        }

    </script>

    <div class="row">
        <div class="col-md-12">
            <input type="button" class="btn btn-primary" value="Add More Room" onclick="EntryPanelVisibleTrue()" />
        </div>
    </div>

    <div id="EntryPanel" style="display:none;">
        <div class="panel panel-default">
            <div class="panel-heading">Floor Management Information</div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2 text-right">
                            <asp:Label ID="lblFloorId" runat="server" CssClass="control-label" Text="Floor Name"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlFloorId" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlFloorId_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2 text-right">
                            <asp:Label ID="Label2" runat="server" CssClass="control-label" Text="Block"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlFloorBlock" CssClass="form-control" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFloorBlock_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>


                    <div class="row">
                        <div class="col-md-12">
                            <asp:GridView ID="gvHMFloorManagement" Width="100%" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                ForeColor="#333333" PageSize="50" OnPageIndexChanging="gvHMFloorManagement_PageIndexChanging"
                                OnRowDataBound="gvHMFloorManagement_RowDataBound" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFloorManagementId" runat="server" Text='<%#Eval("FloorManagementId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Room Number">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvRoomId" runat="server" Text='<%# Bind("RoomId") %>' Visible="False"></asp:Label>
                                            <asp:Label ID="lblgvRoomNumber" runat="server" Text='<%# Bind("RoomNumber") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Room Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvRoomType" runat="server" Text='<%# Bind("RoomType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Added Status" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsActiveStatus" runat="server" />
                                            <asp:Label ID="lblchkIsActiveStatus" runat="server" Text='<%#Eval("ActiveStatus") %>'
                                                Visible="False"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
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

                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnSaveAll" runat="server" Text="Save" CssClass="btn btn-primary"
                                OnClick="btnSaveAll_Click" />
                            <input type="button" id="btnCancel" class="btn btn-primary" value="Close" onclick="EntryPanelVisibleFalse()" />
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfNewEntryShow" runat="server" Value="0" />
    <div class="panel panel-default" id="GraphicalPanel">
        <div class="panel-heading">Floor Management Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2 text-right">
                        <asp:HiddenField ID="txtFloorWiseRoomAllocationInfo" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblSrcFloorId" runat="server" CssClass="control-label" Text="Floor Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSrcFloorId" CssClass="form-control" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSrcFloorId_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2 text-right">
                        <asp:Label ID="Label1" runat="server" CssClass="control-label" Text="Block"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSrcFloorBlock" CssClass="form-control" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSrcFloorBlock_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSave" runat="server" Text="Save Design" CssClass="btn btn-primary"
                            OnClick="btnSave_Click" OnClientClick="javascript:return GenerateCoordinates()" />
                    </div>
                </div>

            </div>
            <br />
            <div class="row">
                <div class="col-md-12 FloorRoomAllocationBGImage" style="height: 800px; overflow-y: scroll;">
                    <asp:Literal ID="ltlRoomTemplate" runat="server">
                    </asp:Literal>
                </div>
            </div>

        </div>
    </div>

    <script type="text/javascript">
        $(function () {
            $(".draggable").draggable().resizable();
            $(".droppable, #droppable-inner").droppable({
                drop: function (event, ui) {
                    //alert(ui.draggable.attr('id') + ' was dropped from ' + ui.draggable.parent().attr('id'));
                    return false;
                }
            });
        });

        $(document).ready(function () {
            $(".draggable").mousedown(function () {
                $(this).css('border-style', 'dashed');
            });
            $(".draggable").mouseup(function () {
                $(this).parent().find('.draggable').css('border-style', 'solid');
            });
        });

    </script>
</asp:Content>
