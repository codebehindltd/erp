<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="frmInvLocation.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.frmInvLocation" %>

<%@ Register Assembly="HotelManagement.Presentation.Website" Namespace="HotelManagement.Presentation.Website.Common"
    TagPrefix="cmsn" %>
<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //Bread Crumbs Information-------------
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Stock Location Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $('#ContentPlaceHolder1_gvCategoryCostCenterInfo_ChkCreate').click(function () {
                if ($('#ContentPlaceHolder1_gvCategoryCostCenterInfo_ChkCreate').is(':checked')) {
                    CheckAllCheckBoxCreate()
                }
                else {
                    UnCheckAllCheckBoxCreate();
                }
            });

           <%-- var txtFocusTabControl = '<%=txtFocusTabControl.ClientID%>'
            var A = '<%=A.ClientID%>'
            var B = '<%=B.ClientID%>'
            var C = '<%=C.ClientID%>'
            if ($('#' + txtFocusTabControl).val() == '2') {
                $('#' + B).addClass('ui-state-default ui-corner-top ui-tabs-active ui-state-active');
                $('#' + A).addClass('ui-state-default ui-corner-top');
                $('#' + C).addClass('ui-state-default ui-corner-top');
                $('#divNodeMatrix').show();
            }
            else {
                $('#' + A).addClass('ui-state-default ui-corner-top ui-tabs-active ui-state-active');
                $('#' + B).addClass('ui-state-default ui-corner-top');
                $('#' + C).addClass('ui-state-default ui-corner-top');
                $('#divNodeMatrix').show();
            }--%>

            //SearchText();
            $("#txtSearch").focus();
            var ddlNodeId = '<%=ddlNodeId.ClientID%>'

            if ($('#' + ddlNodeId).val() != '0') {
                $("#txtSearch").val($('#' + ddlNodeId).find('option').filter(':selected').text());
            }
            else {
                $("#txtSearch").val('');
            }

            var ddlNodeId = '<%=ddlNodeId.ClientID%>'
            var txtNodeHead = '<%=txtNodeHead.ClientID%>'

            $('#' + txtNodeHead).blur(function () {
                $("#CostCenterInformationDiv").show("Slow");
                var search = $('#txtSearch').val();
                //alert(search);
                if (search == "") {
                    ddlNodeId.val(0);
                }
                if ($.trim($('#' + txtNodeHead)).length > 0) {
                    if ($('#' + ddlNodeId).val() > 0) {
                        $("#CostCenterInformationDiv").hide("Slow");
                    }
                }
            });

            var txtNodeHeadText = '<%=txtNodeHeadText.ClientID%>'
            var ddlNodeIdForEdit = '<%=ddlNodeIdForEdit.ClientID%>'
            $("#txtSearch").blur(function () {
                if ($.trim($("#txtSearch").val()).length > 0) {
                    SearchTextById();
                }
            });

            //SearchTextForEdit();
            $("#txtAccHead").focus();
            $("#txtAccHead").blur(function () {
                if ($.trim($("#txtAccHead").val()).length > 0) {
                    //SearchTextForEditById();
                }
            });

        });
        $(function () {
            $("#myTabs").tabs();
        });
        function CheckAllCheckBoxCreate() {
            $('.Chk_Create input[type=checkbox]').each(function () {
                $(this).prop("checked", true);
            });
        }

        function UnCheckAllCheckBoxCreate() {
            $('.Chk_Create input[type=checkbox]').each(function () {
                $(this).prop("checked", false);
            });
        }
        //---------------------------
        function SearchTextById() {
            var vdata = document.getElementById('txtSearch').value;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Inventory/frmInvLocation.aspx/FillForm",
                data: "{'searchText':'" + FilteringSearchText(vdata) + "'}",
                dataType: "json",
                success: function (data) {
                    var ddlNodeId = '<%=ddlNodeId.ClientID%>'
                    $('#' + ddlNodeId).val(data.d);
                },
                error: function (result) {
                    //alert("Error");
                }
            });
        }

        function FilteringSearchText(str) {
            return str.replace(/[']/g, escape).replace(/\*/g, "%2A");
        }

        function fixedEncodeURIComponent(str) {
            return encodeURIComponent(str).replace(/[!'()]/g, escape).replace(/\*/g, "%2A");
        }


        //---------------------------
        function SearchText() {
            $('.SearchAccountHeadTextBox').autocomplete({
                source: function (request, response) {
                    var vdata = document.getElementById('txtSearch').value;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "/Inventory/frmInvLocation.aspx/GetAutoCompleteData1",
                        data: "{'searchText':'" + FilteringSearchText(vdata) + "'}",
                        dataType: "json",
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                }
            });
        }


        function SearchTextForEditById() {
            var vdata = document.getElementById('txtAccHead').value;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Inventory/frmInvLocation.aspx/FillForm",
                data: "{'searchText':'" + FilteringSearchText(vdata) + "'}",
                dataType: "json",
                success: function (data) {
                    var ddlNodeId = '<%=ddlNodeId.ClientID%>'
                    $('#' + ddlNodeId).val(data.d);
                },
                error: function (result) {
                    //alert("Error");
                }
            });
        }



        //function SearchTextForEdit() {
        //    $('.SearchAccountHeadTextBoxForEdit').autocomplete({
        //        source: function (request, response) {
        //            var vdata = document.getElementById('txtAccHead').value;
        //            $.ajax({
        //                type: "POST",
        //                contentType: "application/json; charset=utf-8",
        //                url: "/Inventory/frmInvLocation.aspx/GetAutoCompleteData1",
        //                data: "{'searchText':'" + FilteringSearchText(vdata) + "'}",
        //                dataType: "json",
        //                success: function (data) {
        //                    //alert('Edit:'+ data.d);
        //                    response(data.d);
        //                },
        //                error: function (result) {
        //                    //alert("Error");
        //                }
        //            });
        //        }
        //    });
        //}

        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }

        ///utility functions
        function OnTreeClick(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var nodeClick = src.tagName.toLowerCase() == "a";
            if (nodeClick) {
                selectedNode = $(src).parents('table:first');
                //innerText works in IE but fails in Firefox (I'm sick of browser anomalies), so use innerHTML as well
                var nodeText = src.innerText || src.innerHTML;
                var nodeValue = GetNodeValue(src);

                $("#<%=tvLocations.ClientID %>").find("td").each(function () {
                    var tdClass = $(this).attr('class');
                    if (tdClass.indexOf('treeNodeSelected') > -1)
                        $(this).attr('class', tdClass.replace('treeNodeSelected', ''))
                });

                $("#" + src.id).parent().attr('class', $("#" + src.id).parent().attr('class') + ' treeNodeSelected');
                //$("#"+src.id).parent().className = "treeNodeSelected"
                //alert("Text: " + nodeText + "," + "Value: " + nodeValue);
                $(txtLocationId).val(nodeValue);
                OpenItem(nodeValue);
                return false; //comment this if you want postback on node click
            }
            else if (src.tagName.toLowerCase() == "td" && $(src).attr("class").indexOf("treeNode") > -1) {
                selectedNode = $(src).parents('table:first');
                //innerText works in IE but fails in Firefox (I'm sick of browser anomalies), so use innerHTML as well
                //var nodeText = $(src).children("a").innerText || $(src).children("a").innerHTML;
                var nodeValue = GetNodeValue($(src).children("a"));

                $("#<%=tvLocations.ClientID %>").find("td").each(function () {
                    var tdClass = $(this).attr('class');
                    if (tdClass.indexOf('treeNodeSelected') > -1)
                        $(this).attr('class', tdClass.replace('treeNodeSelected', ''))
                });

                $(src).attr('class', $(src).attr('class') + ' treeNodeSelected');
                $(txtLocationId).val(nodeValue);
                OpenItem(nodeValue);
                return false; //comment this if you want postback on node click
            }
            nodeClick = src.tagName.toLowerCase() == "img" && $(src).attr("src").indexOf("house.png") > 0
            if (nodeClick) {
                selectedNode = $(src).parents('table:first');
                return false; //comment this if you want postback on node click
            }
        }

        function GetNodeValue(node) {
            var nodeValue = "";
            var hrefValue = node.href == null ? node.attr('href') : node.href;
            var nodePath = hrefValue.substring(hrefValue.indexOf(",") + 2, hrefValue.length - 2);
            var nodeValues = nodePath.split("\\");
            if (nodeValues.length > 1)
                nodeValue = nodeValues[nodeValues.length - 1];
            else
                nodeValue = nodeValues[0].substr(1);
            return nodeValue;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewAccountHead').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewAccountHead').show("slow");
            $('#EntryPanel').hide("slow");
            return false;
        }
        function CheckLength() {

            var search = $('#txtSearch').val();
            // alert(search);
            if (search == "") {
                $("#CostCenterInformationDiv").show("Slow");
            }
            else {
                $("#CostCenterInformationDiv").hide("Slow");
            }
        }
    </script>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="divNodeMatrix">
        <div id="myTabs">
            <ul id="tabPage" class="ui-style">
                <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-1">Location</a></li>
                <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-2">Search Location</a></li>
                <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-3">Tree View</a></li>
                <%--<li id="C" runat="server"><a href="#tab-3">Title 3</a></li>--%>
            </ul>
            <div id="tab-1">
                <div id="EntryPanel" class="panel panel-default">
                    <%--<a href="#page-stats" class="block-heading" data-toggle="collapse">Chart Of Accounts</a>--%>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group" style="display: none;">
                                <div class="col-md-2 label-align">
                                    <asp:HiddenField ID="txtFocusTabControl" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="txtEditNodeId" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="txtAncestorNodeId" runat="server"></asp:HiddenField>
                                    <asp:Label ID="lblNodeId" runat="server" class="control-label" Text="Parent Location"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlNodeId" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblSrcNodeHead" runat="server" class="control-label" Text="Parent Location"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <input type="text" id="txtSearch" class="form-control SearchAccountHeadTextBox"
                                        tabindex="1" onblur="CheckLength()" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:HiddenField ID="txtLocationId" runat="server"></asp:HiddenField>
                                    <asp:Label ID="lblNodeHead" runat="server" class="control-label required-field" Text="New Location"></asp:Label>                                    
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtNodeHead" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:HiddenField ID="tempLocationId" runat="server"></asp:HiddenField>
                                    <asp:Label ID="lblCodeNumber" runat="server" class="control-label required-field" Text="Code Number"></asp:Label>                                    
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCodeNumber" runat="server" CssClass="form-control"
                                        TabIndex="3"></asp:TextBox>
                                </div>
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblIsStoreLocation" runat="server" class="control-label" Text="Is Store Location"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlIsStoreLocation" runat="server" CssClass="form-control"
                                        TabIndex="4">
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="form-control"
                                        TabIndex="4">
                                        <asp:ListItem Value="0">Active</asp:ListItem>
                                        <asp:ListItem Value="1">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblDescription" runat="server" class="control-label" Text="Description"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine"
                                        TabIndex="4"></asp:TextBox>
                                </div>
                            </div>
                            <div id="CategoryCostCenterInformationDiv">
                                <div class="panel-body">                                    
                                        <asp:GridView ID="gvCategoryCostCenterInfo" Width="100%" runat="server" AllowPaging="True"
                                            AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                            ForeColor="#333333" PageSize="200" CssClass="table table-bordered table-condensed table-responsive">
                                            <RowStyle BackColor="#E3EAEB" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="IDNO" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCostCentreId" runat="server" Text='<%#Eval("CostCenterId") %>'></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Create/Update" ItemStyle-Width="05%">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="ChkCreate" CssClass="ChkCreate" runat="server" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIsSavePermission" CssClass="Chk_Create" runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cost Center Information" ItemStyle-Width="55%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvCostCentre" runat="server" Text='<%# Bind("CostCenter") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
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
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="TransactionalButton btn btn-primary"
                                    TabIndex="8" />
                                <asp:Button ID="btnClear" OnClientClick="return confirm('Do you want to clear?');" runat="server" TabIndex="9" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                                    OnClick="btnClear_Click" />
                                    </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="tab-2">
                <div id="Div1" class="panel panel-default">
                    <div class="panel-heading">
                        Search Panel</div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group" style="display: none;">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblNodeId2" runat="server" class="control-label" Text="Location"></asp:Label>
                                    <asp:HiddenField ID="txtNodeHeadText" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="txtEditNodeId2" runat="server"></asp:HiddenField>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlNodeIdForEdit" TabIndex="1" CssClass="form-control"
                                        runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div style="padding-top: 15px;">
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblAccHead" runat="server" class="control-label" Text="Location Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtAccHead" runat="server" class="form-control SearchAccountHeadTextBoxForEdit" ClientIDMode="Static"></asp:TextBox>                                 
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" CssClass="TransactionalButton btn btn-primary"
                                        OnClick="btnSearch_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="Div2" class="panel panel-default">
                    <div class="panel-heading">
                        Search Information</div>
                    <div class="panel-body">
                        <asp:GridView ID="gvChartOfAccout" Width="100%" runat="server" AllowPaging="True"
                            AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                            ForeColor="#333333" PageSize="30" OnPageIndexChanging="gvChartOfAccout_PageIndexChanging"
                            OnRowCommand="gvChartOfAccout_RowCommand" OnRowDataBound="gvChartOfAccout_RowDataBound"
                            CssClass="table table-bordered table-condensed table-responsive">
                            <RowStyle BackColor="#E3EAEB" />
                            <Columns>
                                <asp:TemplateField HeaderText="IDNO" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblid" runat="server" Text='<%#Eval("LocationId") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name" HeaderText="Location" ItemStyle-Width="50%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgUpdate" OnClientClick="return confirm('Do you want to edit?');" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                            CommandArgument='<%# bind("LocationId") %>' ImageUrl="~/Images/edit.png" Text=""
                                            AlternateText="Edit" ToolTip="Edit" />
                                        &nbsp;<asp:ImageButton ID="ImgDelete" OnClientClick="return confirm('Do you want to delete?');" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                            CommandArgument='<%# bind("LocationId") %>' ImageUrl="~/Images/delete.png" Text=""
                                            AlternateText="Delete" ToolTip="Delete" />
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
            <div id="tab-3">
                <div id="SearchPanel" class="panel panel-default">
                    <div class="panel-heading">
                        Tree View: Location</div>
                    <div class="panel-body">
                        <asp:TreeView ID="tvLocations" runat="server" NodeStyle-CssClass="treeNode" SelectedNodeStyle-CssClass="treeNodeSelected"
                            HoverNodeStyle-CssClass="treeNodeSelected" Style="font-size: 13px; font-family: Tahoma;
                            font-weight: bold; text-align: left;" ShowLines="true">
                            <Nodes>
                                <asp:TreeNode></asp:TreeNode>
                            </Nodes>
                        </asp:TreeView>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
       

        $("#txtSearch").focus();

        var x = '<%=isMessageBoxEnable%>';
        if (x > -1) {
            MessagePanelShow();
            if (x == 2) {
                $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
            }
        } else { MessagePanelHide(); }

        $("#txtSearch").focus();
    </script>
</asp:Content>
