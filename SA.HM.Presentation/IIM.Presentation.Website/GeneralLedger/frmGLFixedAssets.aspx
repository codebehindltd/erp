<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmGLFixedAssets.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.frmGLFixedAssets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Accounts</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Fixed Asset Entry</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            SearchText();
            $("#ContentPlaceHolder1_txtAccountHead").focus();
            var ddlNodeId = '<%=ddlNodeId.ClientID%>'

            if ($('#' + ddlNodeId).val() != '0') {
                $("#ContentPlaceHolder1_txtAccountHead").val($('#' + ddlNodeId).find('option').filter(':selected').text());
            }
            else {
                $("#ContentPlaceHolder1_txtAccountHead").val('');
            }

            $("#ContentPlaceHolder1_txtAccountHead").blur(function () {
                if ($.trim($("#ContentPlaceHolder1_txtAccountHead").val()).length > 0) {
                    SearchTextById();
                }
            });

        });

        function SearchTextById() {
            var vdata = $('#ContentPlaceHolder1_txtAccountHead').val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/GeneralLedger/frmNodeMatrix.aspx/FillForm",
                data: "{'searchText':'" + FilteringSearchText(vdata) + "'}",
                dataType: "json",
                success: function (data) {
                    var ddlNodeId = '<%=ddlNodeId.ClientID%>'
                    $('#' + ddlNodeId).val(data.d);
                    GetBlockValueByNodeId(data.d)
                },
                error: function (result) {
                    //alert("Error");
                }
            });
        }

        function SearchText() {
            $('.SearchAccountHeadTextBox').autocomplete({
                source: function (request, response) {
                    var vdata = $('#ContentPlaceHolder1_txtAccountHead').val();
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

        function FilteringSearchText(str) {
            return str.replace(/[']/g, escape).replace(/\*/g, "%2A");
        }

        function fixedEncodeURIComponent(str) {
            return encodeURIComponent(str).replace(/[!'()]/g, escape).replace(/\*/g, "%2A");
        }

        function PerformClearAction() {
            var txtBlockB = '<%=txtBlockB.ClientID%>'
            var txtBlockE = '<%=txtBlockE.ClientID%>'
            var txtBlockF = '<%=txtBlockF.ClientID%>'
            var txtFixedAssetId = '<%=txtFixedAssetId.ClientID%>'
            $('#' + txtBlockB).val('');
            $('#' + txtBlockE).val('');
            $('#' + txtBlockF).val('');
            $('#' + txtFixedAssetId).val('');
            $("#<%=btnSave.ClientID %>").val("Save");
        }
        function GetBlockValueByNodeId(NodeId) {
            PageMethods.GetBlockValueByNodeIdByWebMethod(NodeId, OnGetBlockValueByNodeIdSucceeded, OnGetBlockValueByNodeIdFailed);
            return false;
        }
        function OnGetBlockValueByNodeIdSucceeded(result) {
            var txtBlockB = '<%=txtBlockB.ClientID%>'
            var txtBlockE = '<%=txtBlockE.ClientID%>'
            var txtBlockF = '<%=txtBlockF.ClientID%>'
            var txtFixedAssetId = '<%=txtFixedAssetId.ClientID%>'
            $('#' + txtBlockB).val(result.BlockB);
            $('#' + txtBlockE).val(result.BlockE);
            $('#' + txtBlockF).val(result.BlockF);
            $('#' + txtFixedAssetId).val(result.FixedAssetsId);
            if (result.FixedAssetsId != "") {
                $("#<%=btnSave.ClientID %>").val("Update");
            }
        }

        function OnGetBlockValueByNodeIdFailed(error) {
            alert(error.get_message());
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
    <div id="EntryPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Fixed Asset Entry
        </a>
        <div class="HMBodyContainer">
            <div class="divSection" style="display: none;">
                <div class="divBox divSectionLeftLeft">
                    <asp:HiddenField ID="txtFixedAssetId" runat="server"></asp:HiddenField>
                    <asp:Label ID="lblNodeId" runat="server" Text="Account Head"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlNodeId" CssClass="ThreeColumnDropDownList" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblAccountHead" runat="server" Text="Account Head"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtAccountHead" runat="server" CssClass="ThreeColumnTextBox SearchAccountHeadTextBox"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection" style="display:none">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblBlockB" runat="server" Text="Block B"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtBlockB" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblBlockE" runat="server" Text="Depreciation Rate(%)"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtBlockE" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblBlockF" runat="server" Text="Block F"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtBlockF" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="HMContainerRowButton">
                <%--Right Left--%>
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                    OnClick="btnSave_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                    OnClientClick="javascript: return PerformClearAction();" />
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

        var xNewAdd = '<%=isNewAddButtonEnable%>';
        if (xNewAdd > -1) {
            $("#ContentPlaceHolder1_txtAccountHead").Val('');
        }
        else {
            NewAddButtonPanelHide();
        }
        
    </script>
</asp:Content>
