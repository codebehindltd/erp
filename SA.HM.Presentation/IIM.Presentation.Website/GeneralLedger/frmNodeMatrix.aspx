<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="frmNodeMatrix.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.frmNodeMatrix" %>

<%@ Register Assembly="HotelManagement.Presentation.Website" Namespace="HotelManagement.Presentation.Website.Common"
    TagPrefix="cmsn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //Bread Crumbs Information-------------
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Accounts</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Chart Of Accounts</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            var txtFocusTabControl = '<%=txtFocusTabControl.ClientID%>'
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
            }

            SearchText();
            $("#txtSearch").focus();
            var ddlNodeId = '<%=ddlNodeId.ClientID%>'

            if ($('#' + ddlNodeId).val() != '0') {
                $("#txtSearch").val($('#' + ddlNodeId).find('option').filter(':selected').text());
            }
            else {
                $("#txtSearch").val('');
            }


            var txtNodeHeadText = '<%=txtNodeHeadText.ClientID%>'
            var ddlNodeIdForEdit = '<%=ddlNodeIdForEdit.ClientID%>'
            $("#txtSearch").blur(function () {
                if ($.trim($("#txtSearch").val()).length > 0) {
                    SearchTextById();
                }
            });

            SearchTextForEdit();
            $("#txtAccHead").focus();
            $("#txtAccHead").blur(function () {
                if ($.trim($("#txtAccHead").val()).length > 0) {
                    SearchTextForEditById();
                }
            });

        });
        $(function () {
            $("#myTabs").tabs();
        });
        //---------------------------
        function SearchTextById() {
            var vdata = document.getElementById('txtSearch').value;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/GeneralLedger/frmNodeMatrix.aspx/FillForm",
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
                            url: "/GeneralLedger/frmNodeMatrix.aspx/GetAutoCompleteData1",
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
                    url: "/GeneralLedger/frmNodeMatrix.aspx/FillForm",
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



                function SearchTextForEdit() {
                    $('.SearchAccountHeadTextBoxForEdit').autocomplete({
                        source: function (request, response) {
                            var vdata = document.getElementById('txtAccHead').value;
                            $.ajax({
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: "/GeneralLedger/frmNodeMatrix.aspx/GetAutoCompleteData1",
                                data: "{'searchText':'" + FilteringSearchText(vdata) + "'}",
                                dataType: "json",
                                success: function (data) {
                                    //alert('Edit:'+ data.d);
                                    response(data.d);
                                },
                                error: function (result) {
                                    //alert("Error");
                                }
                            });
                        }
                    });
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
    </script>
    <div id="divNodeMatrix" style="display: none;">
        <div id="myTabs">
            <ul id="tabPage" class="ui-style">
                <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-1">Chart Of Accounts</a></li>
                <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-2">Search Account</a></li>
                <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-3">Tree View</a></li>
            </ul>
            <div id="tab-1" class="panel panel-default">
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group" style="display: none;">
                            <div class="col-md-2 label-align">
                                <asp:HiddenField ID="txtFocusTabControl" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtEditNodeId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtAncestorNodeId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblNodeId" runat="server" Text="Account Head"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlNodeId" class="control-label" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSrcNodeHead" class="control-label" runat="server" Text="Account Head"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <input type="text" id="txtSearch" class="form-control SearchAccountHeadTextBox"
                                    tabindex="1" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblNodeHead" class="control-label required-field" runat="server" Text="New Head"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtNodeHead" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div id="code" runat="server">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblNodeNumber" class="control-label required-field" runat="server" Text="Code Number"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNodeNumber" runat="server" CssClass="form-control"
                                        TabIndex="3"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblActiveStat" class="control-label required-field" runat="server" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="form-control"
                                    TabIndex="4">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <asp:Panel ID="pnlCoaConfiguration" runat="server">
                            <div class="collapse in">
                                <div class="divFullSectionWithOneDvie">
                                    <div class="divFullSectionWithOneDvie">
                                        <div id="AccountTypeInformation" class="block">
                                            <a href="#page-stats" class="block-heading" data-toggle="collapse">Account Type Information
                                            </a>
                                            <div class="HMBodyContainer">
                                                <div class="divSectionGLAccountType">
                                                    <div style="float: left; text-align: center;">
                                                        <asp:CheckBoxList ID="chkAccountType" runat="server" RepeatDirection="Horizontal"
                                                            TabIndex="5">
                                                            <asp:ListItem Value="CP">Cash Payment (CP)</asp:ListItem>
                                                            <asp:ListItem Value="BP">Bank Payment (BP)</asp:ListItem>
                                                            <asp:ListItem Value="CR">Cash Receive (CR)</asp:ListItem>
                                                            <asp:ListItem Value="BR">Bank Receive (BR)</asp:ListItem>
                                                            <asp:ListItem Value="JV">Journal Voucher (JV)</asp:ListItem>
                                                            <asp:ListItem Value="CBC">Cash Bank Contra (CBC)</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                                <div class="divClear">
                                                </div>
                                            </div>
                                            <div class="divClear">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="divClear">
                                    </div>
                                    <div class="divFullSectionWithTwoDvie">
                                        <div class="divBox divSectionLeftRightSameWidth">
                                            <div id="CashFlowInformation" class="block">
                                                <a href="#page-stats" class="block-heading" data-toggle="collapse">Cash Flow Information
                                                </a>
                                                <div class="HMBodyContainer">
                                                    <div class="divSection">
                                                        <div class="divBox divSectionLeftLeft">
                                                            <asp:HiddenField ID="txtCashFlowInformation" runat="server"></asp:HiddenField>
                                                            <asp:Label ID="lblHeadId" runat="server" Text="Head Name"></asp:Label>
                                                        </div>
                                                        <div class="divBox divSectionLeftRight">
                                                            <cmsn:CustomDropDownList ID="ddlHeadId" runat="server" TabIndex="6">
                                                            </cmsn:CustomDropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="divClear">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="divBox divSectionLeftRightSameWidth">
                                            <div id="ProfitLossInformation" class="block">
                                                <a href="#page-stats" class="block-heading" data-toggle="collapse">Profit & Loss Information
                                                </a>
                                                <div class="HMBodyContainer">
                                                    <div class="divSection">
                                                        <div class="divBox divSectionLeftLeft">
                                                            <asp:HiddenField ID="txtProfitLossInformation" runat="server"></asp:HiddenField>
                                                            <asp:Label ID="lblPLHeadId" runat="server" Text="Head Name"></asp:Label>
                                                        </div>
                                                        <div class="divBox divSectionLeftRight">
                                                            <asp:DropDownList ID="ddlPLHeadId" runat="server" TabIndex="7">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="divClear">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="divClear">
                                    </div>
                                    <div class="divFullSectionWithTwoDvie">
                                        <div class="divBox divSectionLeftRightSameWidth">
                                            <div id="BalanceSheetInformation" class="block">
                                                <a href="#page-stats" class="block-heading" data-toggle="collapse">Balance Sheet Information
                                                </a>
                                                <div class="HMBodyContainer">
                                                    <div class="divSection">
                                                        <div class="divBox divSectionLeftLeft">
                                                            <asp:HiddenField ID="txtBalanceSheetInformation" runat="server"></asp:HiddenField>
                                                            <asp:Label ID="lblBSHeadId" runat="server" Text="Head Name"></asp:Label>
                                                        </div>
                                                        <div class="divBox divSectionLeftRight">
                                                            <asp:DropDownList ID="ddlBSHeadId" runat="server" TabIndex="7">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="divClear">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                        </asp:Panel>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary"
                                    TabIndex="8" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="9" Text="Clear" CssClass="btn btn-primary"
                                    OnClick="btnClear_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="tab-2">
                <div id="Div1" class="panel panel-default">
                    <div class="panel-heading">
                        Search Panel
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group" style="display: none;">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblNodeId2" class="control-label" runat="server" Text="Account Head"></asp:Label>
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
                                    <asp:Label ID="lblAccHead" class="control-label" runat="server" Text="Account Head"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <input type="text" id="txtAccHead" class="form-control SearchAccountHeadTextBoxForEdit"
                                        tabindex="2" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" CssClass="btn btn-primary"
                                        OnClick="btnSearch_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="Div2" class="panel panel-default">
                    <div class="panel-heading">
                        Search Information
                    </div>
                    <div class="panel-body">
                        <asp:GridView ID="gvChartOfAccout" Width="100%" runat="server" AllowPaging="True"
                            AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                            ForeColor="#333333" PageSize="30" OnPageIndexChanging="gvChartOfAccout_PageIndexChanging"
                            OnRowCommand="gvChartOfAccout_RowCommand" OnRowDataBound="gvChartOfAccout_RowDataBound" CssClass="table table-bordered table-condensed table-responsive">
                            <RowStyle BackColor="#E3EAEB" />
                            <Columns>
                                <asp:TemplateField HeaderText="IDNO" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblid" runat="server" Text='<%#Eval("NodeId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="NodeHead" HeaderText="Node Head" ItemStyle-Width="50%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NodeNumber" HeaderText="Code" ItemStyle-Width="35%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                            CommandArgument='<%# bind("NodeId") %>' ImageUrl="~/Images/edit.png" Text=""
                                            AlternateText="Edit" ToolTip="Edit" />
                                        &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                            CommandArgument='<%# bind("NodeId") %>' ImageUrl="~/Images/delete.png" Text=""
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
                        Tree View: Chart Of Accounts
                    </div>
                    <div class="panel-body">
                        <asp:TreeView ID="tvLocations" runat="server" NodeStyle-CssClass="treeNode" SelectedNodeStyle-CssClass="treeNodeSelected"
                            HoverNodeStyle-CssClass="treeNodeSelected" Style="font-size: 13px; font-family: Tahoma; font-weight: bold; text-align: left;"
                            ShowLines="true">
                            <Nodes>
                                <asp:TreeNode></asp:TreeNode>
                            </Nodes>
                        </asp:TreeView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
