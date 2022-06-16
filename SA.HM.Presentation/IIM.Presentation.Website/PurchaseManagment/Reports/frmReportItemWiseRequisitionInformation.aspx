<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="frmReportItemWiseRequisitionInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.Reports.frmReportItemWiseRequisitionInformation" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            var reportType = $("#ContentPlaceHolder1_ddlReportType").val();

            var txtStartDate = '<%=txtStartDate.ClientID%>'
            var txtEndDate = '<%=txtEndDate.ClientID%>'

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Purchase</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Item Wise Requisition</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            $("#ContentPlaceHolder1_hfProductId").val("0");
            $("#ContentPlaceHolder1_ddlReportType").change(function () {

                var reportType;
                reportType = $("#ContentPlaceHolder1_ddlReportType").val();
                if (reportType == "Date Wise") {
                    $("#fromCostCenter").show();
                    $("#toCostCenter").show();
                    $("#requisitionNo").show();
                    $("#requisitionStatus").show();
                    $("#item").hide();
                    $("#category").hide();
                   
                }
                else if (reportType == "Item Wise") {
                    $("#fromCostCenter").show();
                    $("#toCostCenter").show();
                    $("#requisitionNo").show();
                    $("#requisitionStatus").show();
                    $("#item").show();
                    $("#category").show();
                    
                }
                else if (reportType == "Costcenter Wise") {
                    $("#fromCostCenter").show();
                    $("#toCostCenter").show();
                    $("#requisitionNo").show();
                    $("#requisitionStatus").show();
                    $("#item").hide();
                    $("#category").hide();
                    
                }
                else if (reportType == "Category Wise") {
                    $("#fromCostCenter").show();
                    $("#toCostCenter").show();
                    $("#requisitionNo").show();
                    $("#requisitionStatus").show();
                    $("#item").hide();
                    $("#category").show();
                    
                }
                else if (reportType == "Requisition Number Wise") {
                    $("#fromCostCenter").show();
                    $("#toCostCenter").show();
                    $("#requisitionNo").show();
                    $("#requisitionStatus").show();
                    $("#item").hide();
                    $("#category").hide();
                 
                }
                else if (reportType == "Invoice Wise") {
                    $("#fromCostCenter").show();
                    $("#toCostCenter").show();
                    $("#requisitionNo").hide();
                    $("#requisitionStatus").hide();
                    $("#item").hide();
                    $("#category").hide();
                
                }
                //else if (reportType == "CompanyProject") {
                //    $("#fromCostCenter").show();
                //    $("#toCostCenter").show();
                //    $("#requisitionNo").show();
                //    $("#requisitionStatus").show();
                //    $("#item").hide();
                //    $("#category").hide();
                //    $("#CompanyProjectDiv").show();
                //}
            });
            $("#ContentPlaceHolder1_ddlReportType").val(reportType).trigger('change');
            //$("#ContentPlaceHolder1_txtItemName").autocomplete({

            //    source: function (request, response) {

            //        var costCenterId = $("#ContentPlaceHolder1_ddlFromCostCenter").val();
            //        var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
            //        //debugger;
            //        $.ajax({
            //            type: "POST",
            //            contentType: "application/json; charset=utf-8",
            //            url: './frmReportItemWiseRequisitionInformation.aspx/ItemSearch',
            //            data: "{'searchTerm':'" + request.term + "', 'costCenterId':'" + costCenterId + "', 'categoryId':'" + categoryId + "'}",
            //            dataType: "json",
            //            success: function (data) {

            //                var searchData = data.error ? [] : $.map(data.d, function (m) {
            //                    return {
            //                        label: m.Name,
            //                        value: m.ItemId
            //                    };
            //                });
            //                response(searchData);
            //            },
            //            error: function (result) {
            //                //alert("Error");
            //            }
            //        });
            //    },
            //    focus: function (event, ui) {
            //        // prevent autocomplete from updating the textbox
            //        event.preventDefault();
            //    },
            //    select: function (event, ui) {
            //        // prevent autocomplete from updating the textbox
            //        event.preventDefault();
            //        // manually update the textbox and hidden field
            //        $(this).val(ui.item.label);                    
            //        $("#ContentPlaceHolder1_hfProductId").val(ui.item.value)
            //    }
            //});

            $('#' + txtStartDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtEndDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtEndDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtStartDate).datepicker("option", "maxDate", selectedDate);
                }
            });

            $("#ContentPlaceHolder1_ddlProductId").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlFromCostCenter").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlToCostCenter").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlItemName").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlSrcGLProject").change(function () {
                var id = $("#ContentPlaceHolder1_ddlSrcGLProject").val();
                var name = $("#ContentPlaceHolder1_ddlSrcGLProject").text();
                $("#ContentPlaceHolder1_hfProjectId").val(id); 
                $("#ContentPlaceHolder1_hfProjectName").val(name);
            });
            var single = $("#ContentPlaceHolder1_hfIsSingle").val();
            if (single == "1") {
                $('#CompanyProjectDiv').hide();
                //$('#SearchTypePanel').show();
            }
            else {
                $('#CompanyProjectDiv').show();
                //$('#SearchTypePanel').hide();
            }
        });

        //Div Visible True/False-------------------
        function EntryPanelVisibleFalse() {
            $('#ReportPanel').show();
        }
        function PopulateProjectsSrc(control) {
            let companyId = $(control).val();

            $("#ContentPlaceHolder1_ddlSrcGLProject").empty().append('<option selected="selected" value="0">Loading...</option>');

            $.ajax({
                type: "POST",
                url: "frmReportItemWiseRequisitionInformation.aspx/GetGLProjectByGLCompanyId",
                data: JSON.stringify({ companyId: companyId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (response) {
                    PopulateControlWithValueNTextField(response.d, $("#ContentPlaceHolder1_ddlSrcGLProject"), $("#<%=CommonDropDownHiddenField.ClientID %>").val(), "Name", "ProjectId");
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
        }
    </script>
    <asp:HiddenField ID="hfIsSingle" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfProjectId" Value="0" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfProjectName" Value="" runat="server"></asp:HiddenField>
       <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfProductId" runat="server" Value="0"></asp:HiddenField>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblReportType" runat="server" class="control-label" Text="Report Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control" TabIndex="6">
                            <asp:ListItem Value="Date Wise">Date Wise</asp:ListItem>
                            <asp:ListItem Value="Item Wise">Item Wise</asp:ListItem>
                            <asp:ListItem Value="Costcenter Wise">Cost Center Wise</asp:ListItem>
                            <asp:ListItem Value="Category Wise">Category Wise</asp:ListItem>
                            <asp:ListItem Value="Requisition Number Wise">Requisition Number Wise</asp:ListItem>
                            <asp:ListItem Value="Invoice Format Wise">Invoice Wise Requisition</asp:ListItem>
                             <%--<asp:ListItem Value="CompanyProject">Company & Project</asp:ListItem>--%>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server"></asp:TextBox><input
                            type="hidden" id="hidFromDate" />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server"></asp:TextBox>
                        <input type="hidden" id="hidToDate" />
                    </div>
                </div>

                <div class="form-group">
                    <div id="fromCostCenter">
                        <div class="col-md-2">
                            <asp:Label ID="lblFromCostCenter" runat="server" class="control-label" Text="From Cost Center"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlFromCostCenter" runat="server" CssClass="form-control" TabIndex="6">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="toCostCenter">
                        <div class="col-md-2">
                            <asp:Label ID="lblToCostCenter" runat="server" class="control-label" Text="To Cost Center"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlToCostCenter" runat="server" CssClass="form-control" TabIndex="6">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div id="requisitionNo">
                        <div class="col-md-2">
                            <asp:Label ID="lblPMNumber" runat="server" class="control-label" Text="Requisition No"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtPMNumber" runat="server" CssClass="form-control" TabIndex="6">
                            </asp:TextBox>
                        </div>
                    </div>
                    <div id="requisitionStatus">
                        <div class="col-md-2">
                            <asp:Label ID="Label1" runat="server" class="control-label" Text="Requisition Status"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlPOApprovalStatus" runat="server" CssClass="form-control" TabIndex="6">
                                <asp:ListItem Value="0">--- All ---</asp:ListItem>
                                <asp:ListItem Value="1">Submitted</asp:ListItem>
                                <asp:ListItem Value="2">Approved</asp:ListItem>
                                <asp:ListItem Value="3">Checked</asp:ListItem>
                                <asp:ListItem Value="4">Canceled</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div id="category" style="display: none">
                        <div class="col-md-2">
                            <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category Name"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="item" style="display: none">
                        <div class="col-md-2">
                            <asp:Label ID="lblBundleName" runat="server" class="control-label" Text="Item Name"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlItemName" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group" id="CompanyProjectDiv">
                    <div class="col-md-2">
                        <asp:Label ID="Label6" runat="server" class="control-label"
                            Text="Company"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSrcGLCompany" runat="server" CssClass="form-control" onchange="PopulateProjectsSrc(this);">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label7" runat="server" class="control-label"
                            Text="Project"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSrcGLProject" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary btn-sm"
                            OnClick="btnGenarate_Click" />
                    </div>
                </div>
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
    <div id="ReportPanel" class="panel panel-default" style="display: none;">
        <div class="panel-heading">
            Report:: Requisition Information
        </div>
        <div class="panel-body">
            <asp:Panel ID="pnlRoomCalender" runat="server" ScrollBars="Both" Height="800px">
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
        });

        function PrintDocumentFunc(ss) {
            $('#btnPrintReportFromClient').trigger('click');
            return true;
        }

        var x = '<%=_RoomStatusInfoByDate%>';
        if (x > -1)
            EntryPanelVisibleFalse();
    </script>
</asp:Content>

