<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmReportItemWiseStock.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.Reports.frmReportItemWiseStock" %>

<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Inventory</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Item Wise Stock</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            $('#ContentPlaceHolder1_txtStockDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            });

            if ($("#ContentPlaceHolder1_ddlReportType").val() == "ItemWiseStock") {
                $("#CostcenterWise").hide();
                $("#LocationWise").hide();
                //$("#CompanyAndProjectDiv").hide();
                $("#ContentPlaceHolder1_ddlLocation").val("0");
            }
            else if ($("#ContentPlaceHolder1_ddlReportType").val() == "CategoryWiseStock") {
                $("#CostcenterWise").hide();
                $("#LocationWise").hide();
                //$("#CompanyAndProjectDiv").hide();
                $("#ContentPlaceHolder1_ddlLocation").val("0");
            }
            else if ($("#ContentPlaceHolder1_ddlReportType").val() == "ProjectWiseStock") {
                $("#LocationWise").show();
                //$("#CompanyAndProjectDiv").show();
            }
            else if ($("#ContentPlaceHolder1_ddlReportType").val() == "LocationWiseStock") {
                $("#LocationWise").show();
                //$("#CompanyAndProjectDiv").hide();
            }

            var ddlCategory = '<%=ddlCategory.ClientID%>'
            var category = $('#' + ddlCategory).val();
            var ddlItem = '<%=ddlItem.ClientID%>'

            var txtHiddenItemId = '<%=txtHiddenItemId.ClientID%>'
            $('#' + ddlItem).change(function () {

                $('#' + txtHiddenItemId).val($('#' + ddlItem).val());
                if ($(this).val() == "0")
                    $("#dvStockType").show();
                else
                    $("#dvStockType").hide();
            });

            if ($('#' + txtHiddenItemId).val() > 0) {
                $('#' + ddlItem).val($('#' + txtHiddenItemId).val());
            }

            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlBrand").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlClassification").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlItem").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%",
                tags: false
            });


            $("#ContentPlaceHolder1_ddlColor").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlSize").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlStyle").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlReportType").change(function () {
                if ($(this).val() == "ItemWiseStock") {
                    $("#CostcenterWise").hide();
                    $("#LocationWise").hide();
                    //$("#CompanyAndProjectDiv").hide();
                    $("#ContentPlaceHolder1_ddlCategory").val("0");
                    $("#ContentPlaceHolder1_ddlProduct").val("0");
                    $("#ContentPlaceHolder1_ddlLocation").val("0");
                }
                else if ($(this).val() == "CategoryWiseStock") {
                    $("#CostcenterWise").hide();
                    $("#LocationWise").hide();
                    //$("#CompanyAndProjectDiv").hide();
                    $("#ContentPlaceHolder1_ddlCategory").val("0");
                    $("#ContentPlaceHolder1_ddlProduct").val("0");
                    $("#ContentPlaceHolder1_ddlLocation").val("0");
                }
                else if ($(this).val() == "LocationWiseStock") {
                    $("#ContentPlaceHolder1_ddlCategory").val("0");
                    $("#ContentPlaceHolder1_ddlProduct").val("0");
                    $("#ContentPlaceHolder1_ddlLocation").val("0");
                    //$("#CompanyAndProjectDiv").hide();
                    $("#CostcenterWise").show();
                    $("#LocationWise").show();
                }
                else if ($(this).val() == "ProjectWiseStock") {
                    $("#ContentPlaceHolder1_ddlCategory").val("0");
                    $("#ContentPlaceHolder1_ddlProduct").val("0");
                    $("#ContentPlaceHolder1_ddlLocation").val("0");
                    //$("#CompanyAndProjectDiv").show();
                    $("#CostcenterWise").show();
                    $("#LocationWise").show();
                }
            });
        });

        function LoadProductItem(itemType) {
            PageMethods.GetServiceByCriteria(itemType, OnFillServiceSucceeded, OnFillServiceFailed);
            return false;
        }

        function OnFillServiceSucceeded(result) {
            var list = result;
            var ddlItem = '<%=ddlItem.ClientID%>';
            var control = $('#' + ddlItem);
            control.empty();

            if (list != null) {
                if (list.length > 0) {
                    control.removeAttr("disabled");
                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].ItemId + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }
            else {
                control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
            }
            return false;
        }

        function OnFillServiceFailed(error) {
            alert(error.get_message());
        }

        function ValidationProcess() {
            var company = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            var project = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();

            $("#ContentPlaceHolder1_hfCompanyId").val(company);
            $("#ContentPlaceHolder1_hfProjectId").val(project);
            $("#ContentPlaceHolder1_hfCompanyName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany option:selected").text());
            $("#ContentPlaceHolder1_hfProjectName").val($("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject option:selected").text());

        }
    </script>
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfProjectId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyName" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfProjectName" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfIsItemAttributeEnable" runat="server" Value=""></asp:HiddenField>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Report Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="ItemWiseStock" Text="Item Wise Stock"></asp:ListItem>
                            <asp:ListItem Value="CategoryWiseStock" Text="Category Wise Stock"></asp:ListItem>
                            <asp:ListItem Value="LocationWiseStock" Text="Location Wise Stock"></asp:ListItem>
                            <asp:ListItem Value="ProjectWiseStock" Text="Project Wise Stock"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label6" runat="server" class="control-label" Text="Stock Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStockDate" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div id="LocationWise">
                        <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                        <div class="col-md-2">
                            <asp:Label ID="lblLocation" runat="server" class="control-label" Text="Location"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                        <asp:HiddenField ID="txtHiddenItemId" runat="server" />
                        <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlCategory" CssClass="form-control" runat="server"
                            OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblItem" runat="server" class="control-label" Text="Item Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlItem" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" runat="server" id="ColorSizeDiv">
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label"
                            Text="Color"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlColor" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label4" runat="server" class="control-label"
                            Text="Size"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSize" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" runat="server" id="StyleDiv">
                    <div class="col-md-2">
                        <asp:Label ID="Label5" runat="server" class="control-label"
                            Text="Style"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlStyle" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" id="dvStockType">
                    <div class="col-md-2">
                        <asp:Label ID="lblStockType" runat="server" class="control-label"
                            Text="Stock Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlStockType" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label" Text="Show Transaction"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlShowTransaction" CssClass="form-control" runat="server">
                            <asp:ListItem Value="WithoutZero">Without Zero Quantity</asp:ListItem>
                            <asp:ListItem Value="WithZero">With Zero Quantity</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblBrand" runat="server" class="control-label"
                            Text="Manufacturer/Brand"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlBrand" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblClassification" runat="server" class="control-label"
                            Text="Item Classification"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlClassification" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Model Number</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtModel" runat="server" CssClass="form-control "></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblSerial" runat="server" class="control-label"
                            Text="Serial Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSerial" runat="server" CssClass="form-control">
                            <asp:ListItem Value="WithoutSerial">Without Serial</asp:ListItem>
                            <asp:ListItem Value="WithSerial">With Serial</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <%--<div class="form-group" id="CompanyAndProjectDiv" style="display: none;">
                    <div class="col-md-12">--%>

                <%--    </div>
                </div>--%>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary btn-sm"
                            OnClick="btnGenarate_Click" OnClientClick="javascript:return ValidationProcess();" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="columnRight">
                <asp:TextBox ID="txtCompanyName" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtCompanyAddress" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtCompanyWeb" runat="server" Visible="False"></asp:TextBox>
            </div>
        </div>
    </div>
    <div style="display: none;">
        <asp:Button ID="btnPrintReportFromClient" runat="server" Text="Button" OnClick="btnPrintReportFromClient_Click"
            ClientIDMode="Static" />
    </div>
    <div style="display: none;">
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000; top: 2000;"
            clientidmode="static"></iframe>
    </div>
    <div id="ReportPanel" class="panel panel-default">
        <div class="panel-heading">
            Report:: Current Stock Information
        </div>
        <div class="panel-body">
            <asp:Panel ID="pnlReporContainer" runat="server" ScrollBars="Both" Height="700px">
                <rsweb:ReportViewer ShowFindControls="false" ShowWaitControlCancelLink="false" ID="rvTransaction"
                    PageCountMode="Actual" SizeToReportContent="true" ShowPrintButton="true" runat="server"
                    Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" Width="950px" Height="820px">
                </rsweb:ReportViewer>
            </asp:Panel>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            if (CommonHelper.BrowserType().mozilla || CommonHelper.BrowserType().chrome) {

                var barControlId = CommonHelper.GetReportViewerControlId($("#<%=rvTransaction.ClientID %>"));

                var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                var innerTable = '<table title="Print" onclick="PrintDocumentFunc(\'' + barControlId + '\'); return false;" id="ff_print" style="cursor: default;">' + innerTbody + '</table>'
                var outerDiv = '<div style="display: inline-block; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + '</td></tr></tbody></table></div>';

                $("#" + barControlId + " > div").append(outerDiv);
            }
            if ($("#ContentPlaceHolder1_companyProjectUserControl_hfIsSingle").val() != "1") {


                var c = $("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val();
                var p = $("#ContentPlaceHolder1_hfProjectId").val();

                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val($("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val()).trigger("change");
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val($("#ContentPlaceHolder1_hfProjectId").val()).trigger("change");

            }
        });

        function PrintDocumentFunc(ss) {
            $('#btnPrintReportFromClient').trigger('click');
            return true;
        }
    </script>
</asp:Content>
