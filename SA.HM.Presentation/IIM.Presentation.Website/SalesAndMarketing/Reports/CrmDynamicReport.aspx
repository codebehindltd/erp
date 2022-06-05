<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="CrmDynamicReport.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.Reports.CrmDynamicReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var joinTable = [];
        var colNames = new Array;
        var selectedArr = new Array;
        var tableAll = new Array;
        $(document).ready(function () {
            $('#ContentPlaceHolder1_txtSrcFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSrcToDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtSrcToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSrcFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });
            var isDeal = false;
            var isContact = false;
            var isCompany = false;
            $("#ContentPlaceHolder1_ddlSegment").change(function () {
                var segement = $("#ContentPlaceHolder1_ddlSegment").val();
                if (segement != "0") {
                    if (segement == "SMDeal") {
                        isDeal = true;
                    }
                    else if (segement == "SMContactInformation" || segement == "HotelGuestCompany") {
                        isContact = true;
                    }
                    //else if (segement == "HotelGuestCompany") {
                    //    isCompany = true;
                    //}

                    if (segement == "SMLifeCycleStage" && (!isContact)) {
                        toastr.warning("Please select column from Company Or Contact first.");
                        return false;
                    }
                    else if (segement == "SMDealStage" && (!isDeal)) {
                        toastr.warning("Please select column from Deal first.");
                        return false;
                    }
                    else
                        LoadFields(segement);
                }
            });
            $("#btnUp").click(function () {
                var options = $("#ContentPlaceHolder1_lstSelectedFields option:selected");
                if (options.length <= 0) {
                    toastr.warning("Please select a field first.");
                    return false;
                }
                if (options.length > 0) {
                    listboxMove("ContentPlaceHolder1_lstSelectedFields", "up");
                }
            });
            $("#btnDown").click(function () {
                var options = $("#ContentPlaceHolder1_lstSelectedFields option:selected");
                if (options.length <= 0) {
                    toastr.warning("Please select a field first.");
                    return false;
                }
                if (options.length > 0) {
                    listboxMove("ContentPlaceHolder1_lstSelectedFields", "down");
                }
            });
            $("#btnUp2").click(function () {
                var options = $("#ContentPlaceHolder1_lstOrderBy option:selected");
                if (options.length <= 0) {
                    toastr.warning("Please select a field first.");
                    return false;
                }
                if (options.length > 0) {
                    listboxMove("ContentPlaceHolder1_lstOrderBy", "up");
                }
            });
            $("#btnDown2").click(function () {
                var options = $("#ContentPlaceHolder1_lstOrderBy option:selected");
                if (options.length <= 0) {
                    toastr.warning("Please select a field first.");
                    return false;
                }
                if (options.length > 0) {
                    listboxMove("ContentPlaceHolder1_lstOrderBy", "down");
                }
            });
            $("#btnTransferRight").click(function () {
                debugger;
                var options = $("#ContentPlaceHolder1_lstAvailableFields option:selected");
                if (options.length <= 0) {
                    toastr.warning("Please select a field first.");
                    return false;
                }
                var tempText = options[0].text;
                var tempValue = options[0].value;
                var splitStr = options[0].value.split('.');
                //var table = $("#ContentPlaceHolder1_ddlSegment").val();
                var checkTab = _.contains(joinTable, splitStr[0]);//join table names
                if (!checkTab) {
                    joinTable.push(splitStr[0]);
                }
                var checkCol = _.contains(selectedArr, options[0].value);//join col names

                var splitStr2 = "";
                for (var i = 0; i < selectedArr.length; i++) {
                    splitStr2 = selectedArr[i].split('.');
                    if (splitStr2[1] == splitStr[1]) {
                        toastr.warning("Same named column cannot be added.");
                        return false;
                    }
                    splitStr2 = "";
                }

                if (options.length > 0) {
                    if (!checkCol) {
                        for (var i = 0; i < options.length; i++) {
                            $("#ContentPlaceHolder1_lstSelectedFields").append($(options[i]));

                        }
                        $('#ContentPlaceHolder1_lstOrderBy').append($('<option>', {
                            value: tempValue,
                            text: tempText
                        }));
                        //$("#ContentPlaceHolder1_lstOrderBy").append($(temp[0]));
                        selectedArr.push(options[0].value);
                        tableAll.push(splitStr[0]);
                    }
                    else {
                        toastr.warning("Column already added.");
                        return false;
                    }
                }
            });
            $("#btnDelete").click(function () {
                
                var options = $("#ContentPlaceHolder1_lstSelectedFields option:selected");
                var list = document.getElementById("ContentPlaceHolder1_lstOrderBy");
                if (options.length <= 0) {
                    toastr.warning("Please select a field first.");
                    return false;
                }
                //var len = listb.options.length;
                var splitStr = options[0].value.split('.');
                var count = 0;

                if (options.length > 0) {
                    var indexCol = _.indexOf(selectedArr, options[0].value);
                    if (indexCol > -1) {
                        selectedArr.splice(indexCol, 1);
                    }
                    var checkTab = _.contains(joinTable, splitStr[0]);

                    if (checkTab) {

                        for (var i = 0; i < selectedArr.length; i++) {
                            var splitSelected = selectedArr[i].split('.');
                            if (splitSelected[0] == splitStr[0]) {
                                count++;
                            }
                        }

                        if (count <= 0) {
                            var index = _.indexOf(joinTable, splitStr[0]);
                            if (index > -1) {
                                joinTable.splice(index, 1);
                            }
                        }

                    }

                    for (var i = 0; i < list.length; i++) {
                        //var chkIsPresent = _.contains(list.options[i].value, options[0].value);
                        if (list.options[i].value == options[0].value) {
                            list.options[i].remove();
                        }
                    }
                    options[0].remove();
                    //listb.options.remove(i);
                }

            });
            $("#btnDelete2").click(function () {
                var options = $("#ContentPlaceHolder1_lstOrderBy option:selected");
                if (options.length <= 0) {
                    toastr.warning("Please select a field first.");
                    return false;
                }
                if (options.length > 0) {
                    options[0].remove();
                }
            });
            $("#btnDeleteAll").click(function () {
                var options = $("#ContentPlaceHolder1_lstOrderBy option");
                var selected = $("#ContentPlaceHolder1_lstSelectedFields option");
                if (options.length <= 0) {
                    toastr.warning("Please select a field first.");
                    return false;
                }
                if (options.length > 0) {
                    for (var i = 0; i < options.length; i++) {
                        options[i].remove();
                    }
                }
                if (selected.length > 0) {
                    for (var i = 0; i < selected.length; i++) {
                        selected[i].remove();
                    }
                }
            });
            var IsSave = false;
            $("#btnSave_Generate").click(function () {
                IsSave = true;
                CheckValidation(IsSave);
            });
            $("#btnGenerate").click(function () {
                CheckValidation(IsSave);
            });
        });
        function CheckValidation(IsSave) {
            var today = new Date();
            //var dd = String(today.getDate()).padStart(2, '0');
            //var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
            //var yyyy = today.getFullYear();

            //today = mm + '/' + dd + '/' + yyyy;
            //today = CommonHelper.DateFormatMMDDYYY(today);

            var fromDate = $("#<%=txtSrcFromDate.ClientID %>").val();
            var toDate = $("#<%=txtSrcToDate.ClientID %>").val();
            var reportType = $("#<%=ddlReportType.ClientID %>").val();
            var filter = $("#<%=ddlFilters.ClientID %>").val();
            var sequence = $("#<%=ddlSequence.ClientID %>").val();
            var reportName = $("#<%=txtReportName.ClientID %>").val();
            <%--var templateName = $("#<%=txtTemplateName.ClientID %>").val();--%>
            if (IsSave) {
                if (reportName == "") {
                    toastr.warning("Please inseert name for report.");
                    $("#<%=txtReportName.ClientID %>").focus();
                    return false;
                }
            }
            if (selectedArr.length <= 0) {
                toastr.warning("Please select fields for report.");
                return false;
            }
            //var sql = "";
            //sql += "SELECT ";
            //for (var i = 0; i < selectedArr.length; i++) {
            //    sql += selectedArr[i];
            //    sql += (i < selectedArr.length - 1) ? " , " : "";
            //}
            //sql += " FROM ";
            //for (var i = 0; i < joinTable.length; i++) {
            //    sql += joinTable[i];
            //    sql += (i < joinTable.length - 1) ? " , " : "";
            //}
            var list = document.getElementById("ContentPlaceHolder1_lstOrderBy");
            var sequenceArr = new Array;
            for (var i = 0; i < list.options.length; i++) {
                sequenceArr.push(list.options[i].value);
            }
            if (fromDate == "") {
                $("#<%=txtSrcFromDate.ClientID %>").val(today.getDate());
            }
            if (toDate == "") {
                $("#<%=txtSrcToDate.ClientID %>").val(today.getDate());

            }
            //sql += " ORDER BY ";
            //for (var i = 0; i < sequenceArr.length; i++) {
            //    var splitSelected = sequenceArr[i].split('.');
            //    sql += splitSelected[1];
            //    sql += (i < sequenceArr.length - 1) ? " , " : "";
            //}
            //sql += "  " + sequence;
            //debugger;
            PageMethods.SaveAndGenerateReport(IsSave, selectedArr, joinTable, sequenceArr, fromDate, toDate, reportType, reportName, filter, sequence, OnSucceed, OnFailed);
        }
        function OnSucceed(result) {
            var reportName = $("#<%=txtReportName.ClientID %>").val();
            var iframeid = 'IframeReport';
            var url = "./CrmDynamicIframe.aspx?name="+reportName;
            parent.document.getElementById(iframeid).src = url;
            //$("#ReportPanel").show();
            $("#ReportPanel").dialog({
                autoOpen: true,
                modal: true,
                width: 1200,
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "",
                show: 'slide'
            });
        }
        function OnFailed(error) {

        }
        function listboxMove(listID, direction) {
            var listbox = document.getElementById(listID);
            var selIndex = listbox.selectedIndex;
            if (listID == "ContentPlaceHolder1_lstSelectedFields") {

            }
            if (-1 == selIndex) {
                alert("Please select an option to move.");
                return;

            }
            var increment = -1;

            if (direction == 'up')
                increment = -1;
            else
                increment = 1;

            if ((selIndex + increment) < 0 ||
                (selIndex + increment) > (listbox.options.length - 1)) {
                return;
            }

            var temp = selectedArr[selIndex];
            selectedArr[selIndex] = selectedArr[selIndex + increment];
            selectedArr[selIndex + increment] = temp;
            
            var selValue = listbox.options[selIndex].value;

            var selText = listbox.options[selIndex].text;

            listbox.options[selIndex].value = listbox.options[selIndex + increment].value

            listbox.options[selIndex].text = listbox.options[selIndex + increment].text

            listbox.options[selIndex + increment].value = selValue;

            listbox.options[selIndex + increment].text = selText;

            listbox.selectedIndex = selIndex + increment;
        }
        function LoadFields(segement) {
            PageMethods.GetFieldsFromTable(segement, SucceedGetFieldsFromTable, FailedGetFieldsFromTable);
            return false;
        }
        function SucceedGetFieldsFromTable(result) {
            if (result.length > 0) {
                var prevFields = $('#ContentPlaceHolder1_lstAvailableFields option');
                var prevSelected = $('#ContentPlaceHolder1_lstSelectedFields option');
                var table = $("#ContentPlaceHolder1_ddlSegment").val();
                if (prevFields.length > 0) {
                    $("#ContentPlaceHolder1_lstAvailableFields").empty();
                }
                //if (prevSelected.length > 0) {
                //    $("#ContentPlaceHolder1_lstSelectedFields").empty();
                //}
                $.each(result, function (i, item) {
                    $('#ContentPlaceHolder1_lstAvailableFields').append($('<option>', {
                        value: table + "." + item.ColumnName,
                        text: item.ColumnName
                    }));
                });
            }

        }
        function FailedGetFieldsFromTable(error) {

        }
    </script>
    <div style="display: none;">
        <iframe id="frmPrint" name="frmPrint" width="0" height="0" runat="server" style="left: -1000; top: 2000;"
            clientidmode="static"></iframe>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            CRM Dynamic Report
        </div>
        <div class=" panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Select Segment"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSegment" runat="server" CssClass="form-control" TabIndex="2">
                            <asp:ListItem Value="0">Please Select</asp:ListItem>
                            <asp:ListItem Value="SMDeal">Deal</asp:ListItem>
                            <asp:ListItem Value="SMDealStage">Deal Stage</asp:ListItem>
                            <asp:ListItem Value="SMLifeCycleStage">Life Cycle Stage</asp:ListItem>
                            <asp:ListItem Value="HotelGuestCompany">Company</asp:ListItem>
                            <asp:ListItem Value="SMContactInformation">Contacts</asp:ListItem>
                            <asp:ListItem Value="SMQuotation">Quotation</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-4">
                        <asp:Label ID="availabel" runat="server" class="control-label" Text="Available Fields"></asp:Label>
                    </div>

                    <div class="col-md-6" style="padding-left: 185px;">
                        <asp:Label ID="selected" runat="server" class="control-label" Text="Selected Fields"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-5">
                        <asp:ListBox ID="lstAvailableFields" runat="server" Width="100%"
                            Height="200px"></asp:ListBox>
                    </div>
                    <div class="col-md-1">
                        <div style="padding-top: 100px;">
                            <input type="button" id="btnTransferRight" value=">>" style="" class="btn btn-primary" />
                        </div>
                        <%--<div style="padding-top: 2px;">
                            <input type="button" id="btnTransferLeft" value="<<" style="" class="btn btn-primary" />
                        </div>--%>
                    </div>
                    <div class="col-md-5">
                        <asp:ListBox ID="lstSelectedFields" runat="server" Width="100%"
                            Height="200px"></asp:ListBox>
                    </div>
                    <div class="col-md-1">
                        <div style="padding-top: 90px;">
                            <input type="button" id="btnUp" value="Up" style="" class="btn btn-primary" />
                        </div>
                        <div style="padding-top: 2px;">
                            <input type="button" id="btnDown" value="Down" style="" class="btn btn-primary" />
                        </div>
                        <div style="padding-top: 2px;">
                            <input type="button" id="btnDelete" value="X" style="" class="btn btn-primary" />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <%--<div class=" col-md-offset-8">
                    </div>--%>

                    <div class="col-md-12" style="padding-left: 530px;">
                        <asp:Label ID="Label10" runat="server" class="control-label" Text="Order By"></asp:Label>
                    </div>
                </div>
                <div class="form-group" id="divReportType" style="padding-top: 5px">
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label" Text="Report Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control" TabIndex="2">
                            <asp:ListItem Value="TablularReport" Selected="True">Tablular Report</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-5">
                        <asp:ListBox ID="lstOrderBy" runat="server" Width="100%"
                            Height="200px"></asp:ListBox>
                    </div>
                    <div class="col-md-1">
                        <div style="padding-top: 90px;">
                            <input type="button" id="btnUp2" value="Up" style="" class="btn btn-primary" />
                        </div>
                        <div style="padding-top: 2px;">
                            <input type="button" id="btnDown2" value="Down" style="" class="btn btn-primary" />
                        </div>
                        <div style="padding-top: 2px;">
                            <input type="button" id="btnDelete2" value="X" style="" class="btn btn-primary" />
                        </div>
                    </div>
                </div>
                <div class="form-group" id="divFilters" style="padding-top: 5px">
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label" Text="Filters"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlFilters" runat="server" CssClass="form-control" TabIndex="2">
                            <asp:ListItem Value="DateRange" Selected="True">Date Range</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">

                    </div>
                    <div class="col-md-4">
                        <input type="button" id="btnDeleteAll" value="Clear All" title="Delete All" style="" class="btn btn-primary" />
                    </div>
                </div>
                <div class="form-group" id="divDateRange" style="padding-top: 5px">

                    <div class="col-md-2">
                        <asp:Label ID="Label4" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSrcFromDate" runat="server" CssClass="form-control"
                            TabIndex="5"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label5" runat="server" class="control-label" Text="To Date"></asp:Label>
                    </div>

                    <div class="col-md-4">
                        <asp:TextBox ID="txtSrcToDate" runat="server" CssClass="form-control"
                            TabIndex="6"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" id="divSequence" style="padding-top: 5px">
                    <div class="col-md-2">
                        <asp:Label ID="Label6" runat="server" class="control-label" Text="Sequence"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSequence" runat="server" CssClass="form-control" TabIndex="2">
                            <asp:ListItem Value="ASC" Selected="True">Ascending</asp:ListItem>
                            <asp:ListItem Value="DESC">Descending</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group" style="padding-top: 5px;">
                    <div class="col-md-2">
                        <asp:Label ID="Label7" runat="server" class="control-label" Text="Report Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtReportName" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" style="padding-top: 5px">
                    <%--<div class="col-md-2 col-lg-offset-2">
                        <input type="button" id="btnSave_Generate" value="Save as Template & Generate" style="" class="btn btn-primary" />
                    </div>--%>
                    <div class="col-md-2">
                        <input type="button" id="btnGenerate" value="Generate Report" style="" class="btn btn-primary" />
                    </div>
                </div>
                <%--<div style="padding-top: 5px" id="divTemplateName">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label8" runat="server" class="control-label" Text="Template Name"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtTemplateName" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <input type="button" id="btnOkay" value="Okay" style="" class="btn btn-primary" />
                            &nbsp; &nbsp;
                            <input type="button" id="btnCancel" value="Cancel" style="" class="btn btn-primary" />
                        </div>

                    </div>

                </div>--%>
            </div>
        </div>
    </div>
    <div id="ReportPanel" class="panel panel-default" style="display: none;">
        <%--<iframe  id="IframeReport" name="IframeName" runat="server" clientidmode="static" 
            style="height: 100vh; width: 100%; overflow:hidden" frameborder="0"></iframe>--%>
        <iframe id="IframeReport" name="printDoc" width="100%" height="800" frameborder="0" style="overflow: hidden;"></iframe>
    </div>

</asp:Content>
