<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="frmInvCategory.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.frmInvCategory" %>

<%@ Register Assembly="HotelManagement.Presentation.Website" Namespace="HotelManagement.Presentation.Website.Common"
    TagPrefix="cmsn" %>
<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //Bread Crumbs Information-------------
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Category Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_ddlInventoryAccounts").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlCogsAccounts").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlFixedAssetAccounts").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlDepreciationAccounts").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#myTabs").tabs();

            $('#ImagePanel').hide();
            UploadComplete();

            var txtFocusTabControl = '<%=txtFocusTabControl.ClientID%>'
            var A = '<%=A.ClientID%>'
            var B = '<%=B.ClientID%>'
            var C = '<%=C.ClientID%>'

            if ($('#' + txtFocusTabControl).val() == '1') {
                $('#myTabs').tabs({ selected: 0 });
                $('#divNodeMatrix').show();
            }
            else if ($('#' + txtFocusTabControl).val() == '2') {
                $('#myTabs').tabs({ selected: 1 });
                $('#divNodeMatrix').show();
            }
            else {
                $('#myTabs').tabs({ selected: 2 });
                $('#divNodeMatrix').show();
            }

            var ddlServiceType = '<%=ddlServiceType.ClientID%>'
            if ($('#' + ddlServiceType).val() == 'FixedAsset') {
                $('#FixedAssetInformation').show();
                $('#InventoryInformation').hide();
            }
            else {
                $('#FixedAssetInformation').hide();
                $('#InventoryInformation').show();
            }

            $('#' + ddlServiceType).change(function () {
                if ($('#' + ddlServiceType).val() == 'FixedAsset') {
                    $('#FixedAssetInformation').show();
                    $('#InventoryInformation').hide();
                }
                else {
                    $('#FixedAssetInformation').hide();
                    $('#InventoryInformation').show();
                }
            });

            SearchText();
            var ddlNodeId = '<%=ddlNodeId.ClientID%>'

            if ($('#' + ddlNodeId).val() != '0') {
                $("#txtSearch").val($('#' + ddlNodeId).find('option').filter(':selected').text());
            }
            else {
                $("#txtSearch").val('');
            }

            var txtNodeHeadText = '<%=txtNodeHeadText.ClientID%>'
            var ddlNodeIdForEdit = '<%=ddlNodeIdForEdit.ClientID%>'
            $("#txtSearch").change(function () {
                if ($.trim($("#txtSearch").val()).length > 0) {
                    SearchTextById();
                    $("#ContentPlaceHolder1_hfParentCategory").val("0");
                }
                else if ($("#txtSearch").val() == "") {
                    $("#ContentPlaceHolder1_hfParentCategory").val("1");
                }
            });

            //SearchTextForEdit();
            $("#txtAccHead").focus();
            $("#txtAccHead").blur(function () {
                if ($.trim($("#txtAccHead").val()).length > 0) {
                    SearchTextForEditById();
                }
            });


            $("#ContentPlaceHolder1_gvCategoryCostCenterInfo_ChkCreate").change(function () {
                var isChecked = $("#ContentPlaceHolder1_gvCategoryCostCenterInfo_ChkCreate").is(":checked");

                if ($(this).is(":checked") == true) {
                    $("#ContentPlaceHolder1_gvCategoryCostCenterInfo tbody tr").find("td:eq(0) > span").find("input").prop("checked", true);
                }
                else {
                    $("#ContentPlaceHolder1_gvCategoryCostCenterInfo tbody tr").find("td:eq(0) > span").find("input").prop("checked", false);
                }
            });

        });

        //---------------------------
        function SearchTextById() {

            var vdata = document.getElementById('txtSearch').value;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Inventory/frmInvCategory.aspx/FillForm",
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

        function UploadComplete() {
            var id = $("#ContentPlaceHolder1_RandomCategoryId").val();
            ShowUploadedDocument(id);
        }

        function ShowUploadedDocument(id) {
            PageMethods.GetUploadedImageByWebMethod(id, "InventoryProductCategory", OnGetUploadedImageByWebMethodSucceeded, OnGetUploadedImageByWebMethodFailed);
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
            alert(error.get_message());
        }
        function LoadImageUploader() {
            //$("#popUpImage").dialog({
            //    width: 650,
            //    height: 300,
            //    autoOpen: true,
            //    modal: true,
            //    closeOnEscape: true,
            //    resizable: false,
            //    fluid: true,
            //    title: "", // TODO add title
            //    show: 'slide'
            //});
            var randomId = +$("#ContentPlaceHolder1_RandomCategoryId").val();
            var path = "/Inventory/Images/Category/";
            var category = "InventoryProductCategory";
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

        //---------------------------
        function SearchText() {

            $('.SearchAccountHeadTextBox').autocomplete({
                source: function (request, response) {
                    //debugger;
                    var vdata = document.getElementById('txtSearch').value;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "/Inventory/frmInvCategory.aspx/GetAutoCompleteData1",
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
                url: "/Inventory/frmInvCategory.aspx/FillForm",
                data: "{'searchText':'" + FilteringSearchText(vdata) + "'}",
                dataType: "json",
                success: function (data) {
                    var ddlNodeIdForEdit = '<%=ddlNodeIdForEdit.ClientID%>'
                    $('#' + ddlNodeIdForEdit).val(data.d);
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
        //                url: "/Inventory/frmInvCategory.aspx/GetAutoCompleteData1",
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
                    $(txtLocationId).val(nodeValue);
                    OpenItem(nodeValue);
                    return false; //comment this if you want postback on node click
                }
                else if (src.tagName.toLowerCase() == "td" && $(src).attr("class").indexOf("treeNode") > -1) {
                    selectedNode = $(src).parents('table:first');
                    //innerText works in IE but fails in Firefox (I'm sick of browser anomalies), so use innerHTML as well
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
    <asp:HiddenField ID="hfIsInvCategoryCodeAutoGenerate" runat="server" />
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="divNodeMatrix" style="display: none;">
        <div id="myTabs">
            <ul id="tabPage" class="ui-style">
                <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-1">Category</a></li>
                <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-2">Search Category</a></li>
                <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                    href="#tab-3">Tree View</a></li>
            </ul>
            <div id="tab-1" class="panel panel-default">
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group" style="display: none;">
                            <div class="col-md-2 label-align">
                                <asp:HiddenField ID="hfIsInventoryIntegrateWithAccounts" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtFocusTabControl" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtEditNodeId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtAncestorNodeId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hfParentCategory" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblNodeId" runat="server" class="control-label" Text="Parent Category"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlNodeId" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSrcNodeHead" runat="server" class="control-label" Text="Parent Category"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <input type="text" id="txtSearch" class="form-control SearchAccountHeadTextBox" tabindex="1" />
                                <%--<asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>--%>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:HiddenField ID="txtCategoryId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblNodeHead" runat="server" class="control-label required-field" Text="New Category"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtNodeHead" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div id="code" runat="server">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblNodeNumber" runat="server" class="control-label required-field"
                                        Text="Category Code"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNodeNumber" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblServiceType" runat="server" class="control-label required-field"
                                    Text="Category Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="form-control" TabIndex="3">
                                    <asp:ListItem Value="None">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="Product">Product</asp:ListItem>
                                    <asp:ListItem Value="Service">Service</asp:ListItem>
                                    <asp:ListItem Value="FixedAsset">Fixed Asset</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="UpImageDiv">
                            <div class="col-md-2 label-align">
                                <asp:HiddenField ID="RandomCategoryId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hfCategoryId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="tempCategoryId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblCategoryImage" runat="server" class="control-label" Text="Category Image"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <input id="btnImageUp" tabindex="3" type="button" onclick="javascript: return LoadImageUploader();"
                                    class="btn btn-primary" value="Category Image..." />
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="form-control" TabIndex="4">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <asp:Panel ID="pnlIsInventoryIntegrateWithAccounts" runat="server">
                            <div id="InventoryInformation">
                                <div class="form-group" id="AccountForInventory">
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Inventory Head"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlInventoryAccounts" runat="server" CssClass="form-control"
                                            TabIndex="6">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group" id="AccountForCogs">
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Expense Head"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlCogsAccounts" runat="server" CssClass="form-control" TabIndex="6">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div id="FixedAssetInformation">
                                <div class="form-group" id="AccountForNonCurrentAsset">
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Fixed Asset Head"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlFixedAssetAccounts" runat="server" CssClass="form-control"
                                            TabIndex="6">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group" id="AccountForDepreciation">
                                    <div class="col-md-2 label-align">
                                        <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Depreciation Head"></asp:Label>
                                    </div>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="ddlDepreciationAccounts" runat="server" CssClass="form-control" TabIndex="6">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="form-group" id="ImagePanel">
                            <div class="col-md-2 label-align">
                            </div>
                            <div class="col-md-4">
                                <div id="SigDiv" style="width: 150px; height: 150px">
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblDescription" runat="server" class="control-label" Text="Description"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="4"></asp:TextBox>
                            </div>
                        </div>
                        <div id="CategoryCostCenterInformationDiv" class="panel panel-default">
                            <div class="panel-body">
                                <asp:GridView ID="gvCategoryCostCenterInfo" Width="100%" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                    ForeColor="#333333" PageSize="200" CssClass="table table-bordered table-condensed table-responsive">
                                    <RowStyle BackColor="#E3EAEB" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCostCentreId" runat="server" Text='<%#Eval("CostCenterId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Create/Update" ItemStyle-Width="05%">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ChkCreate" CssClass="ChkCreate" runat="server" onc />
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
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary"
                                    TabIndex="8" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="9" Text="Clear" OnClientClick="return confirm('Do you want to clear?');" CssClass="btn btn-primary"
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
                                    <asp:Label ID="lblNodeId2" runat="server" class="control-label" Text="Category"></asp:Label>
                                    <asp:HiddenField ID="txtNodeHeadText" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="txtEditNodeId2" runat="server"></asp:HiddenField>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlNodeIdForEdit" TabIndex="1" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div style="padding-top: 15px;">
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblAccHead" runat="server" class="control-label" Text="Category Name"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtAccHead" runat="server" class="form-control SearchAccountHeadTextBoxForEdit" ClientIDMode="Static"></asp:TextBox>
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
                        <asp:GridView ID="gvChartOfAccout" Width="100%" runat="server"
                            AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                            ForeColor="#333333" PageSize="30" OnRowCommand="gvChartOfAccout_RowCommand" OnRowDataBound="gvChartOfAccout_RowDataBound"
                            CssClass="table table-bordered table-condensed table-responsive">
                            <RowStyle BackColor="#E3EAEB" />
                            <Columns>
                                <asp:TemplateField HeaderText="IDNO" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblid" runat="server" Text='<%#Eval("CategoryId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name" HeaderText="Category" ItemStyle-Width="50%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Code" HeaderText="Code" ItemStyle-Width="35%">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgUpdate" OnClientClick="return confirm('Do you want to edit?');" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                            CommandArgument='<%# bind("CategoryId") %>' ImageUrl="~/Images/edit.png" Text=""
                                            AlternateText="Edit" ToolTip="Edit" />
                                        &nbsp;<asp:ImageButton ID="ImgDelete" OnClientClick="return confirm('Do you want to delete?');" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                            CommandArgument='<%# bind("CategoryId") %>' ImageUrl="~/Images/delete.png" Text=""
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
                        Tree View: Category
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
    <div id="popUpImage" style="display: none">
        <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
            <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
        </asp:Panel>
    </div>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
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
