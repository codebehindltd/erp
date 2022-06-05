<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="LogActivityInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.LogActivityInformation" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            });

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            });
            $("#ContentPlaceHolder1_ddlCompanyOwner").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_txtCompanyForSrc").autocomplete({
                source: function (request, response) {
                    var accountManager = $("#ContentPlaceHolder1_ddlCompanyOwner").val();
                    var industry = $("#ContentPlaceHolder1_hfIndustryId").val();
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/LogActivityInformation.aspx/GetCompanyByAutoSearchAndAccountManagerId',
                        data: JSON.stringify({ searchTerm: request.term, accountManagerId: accountManager, industryId: industry }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.CompanyName,
                                    value: m.CompanyId
                                };
                            });
                            response(searchData);
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                },
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfCompanyId").val(ui.item.value);
                }
            });
            $("#ContentPlaceHolder1_txtIndustry").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/LogActivityInformation.aspx/GetIndustryInfoBySearchCriteria',
                        data: JSON.stringify({ searchTerm: request.term }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.IndustryName,
                                    value: m.IndustryId
                                };
                            });
                            response(searchData);
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                },
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfCompanyId").val(ui.item.value);
                }
            });
            $("#ContentPlaceHolder1_txtContact").autocomplete({
                source: function (request, response) {
                    //debugger;
                    var accountManager = $("#ContentPlaceHolder1_ddlCompanyOwner").val();
                    var company = $("#ContentPlaceHolder1_hfCompanyId").val();
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/LogActivityInformation.aspx/GetContactByAccountManagerNCompany',
                        data: JSON.stringify({ searchTerm: request.term, accountManagerId: accountManager, companyId: company }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.Id
                                };
                            });
                            response(searchData);
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                },
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfContactId").val(ui.item.value);
                }
            });
            $("#ContentPlaceHolder1_txtDeal").autocomplete({
                source: function (request, response) {
                    //debugger;
                    var accountManager = $("#ContentPlaceHolder1_ddlCompanyOwner").val();
                    var company = $("#ContentPlaceHolder1_hfCompanyId").val();
                    var contact = $("#ContentPlaceHolder1_hfContactId").val();
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/LogActivityInformation.aspx/GetDealByCompanyIdContactIdNAccountManager',
                        data: JSON.stringify({ searchText: request.term, contactId: contact, companyId: company, accountManagerId: accountManager }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.Id
                                };
                            });
                            response(searchData);
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                },
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfDealId").val(ui.item.value);
                }
            });

            //GridPaging(1, 1);
        });
        function CreateNew() {
            var logId = $("#hfLogId").val();
            var iframeid = 'frmPrint';
            var url = "../SalesAndMarketing/SalesCallEntry.aspx?id=" + logId + "&from=MenuLinks";
            document.getElementById(iframeid).src = url;
            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "95%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                title: "Log Entry",
                show: 'slide'
            });
            return false;
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }
        function LoadCompanyDetails(companyId) {
            var url = "./CompanyInformation.aspx?id=" + companyId;
            window.location = url;
            return true;
        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadGrid(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function LoadGrid(pageNumber, IsCurrentOrPreviousPage) {
            var company, industry, contact, accountManager, deal;
            var gridRecordsCount = $("#LogTable tbody tr").length;

            var logType = $("#ContentPlaceHolder1_ddlLogType").val();
            var fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();

            if ($("#ContentPlaceHolder1_txtCompanyForSrc").val() == '') {
                company = '0'
            } else {
                company = $("#ContentPlaceHolder1_hfCompanyId").val();
            }

            if ($("#ContentPlaceHolder1_txtIndustry").val() == '') {
                industry = '0';
            } else {
                industry = $("#ContentPlaceHolder1_hfIndustryId").val();
            }

            if ($("#ContentPlaceHolder1_txtContact").val() == '') {
                contact = '0';
            } else {
                contact = $("#ContentPlaceHolder1_hfContactId").val();
            }

            accountManager = $("#ContentPlaceHolder1_ddlCompanyOwner").val();

            if ($("#ContentPlaceHolder1_txtDeal").val() == '') {
                deal = '0';
            } else {
                deal = $("#ContentPlaceHolder1_hfDealId").val();
            }


            if (fromDate != "")
                fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');
            //fromDate = new Date();
            if (toDate != "")
                toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/LogActivityInformation.aspx/LoadLogActivityInformationForSearch',

                data: "{'company':'" + company + "','accountManager':'" + accountManager + "','logtype':'" + logType + "','deal':'" + deal + "','fromDate':'"
                    + fromDate + "','toDate':'" + toDate + "', 'industry':'" + industry + "', 'contact':'" + contact + "', 'gridRecordsCount':'" + gridRecordsCount +
                    "', 'pageNumber':'" + pageNumber + "', 'isCurrentOrPreviousPage':'" + IsCurrentOrPreviousPage + "'}",
                dataType: "json",
                success: function (data) {
                    LoadTable(data);
                },
                error: function (result) {
                    //PerformClearAction();
                }
            });
            return false;
        }
        function LoadTable(data) {
            var isCompanyHyperlinkEnableFromGrid = $("#ContentPlaceHolder1_hfIsCompanyHyperlinkEnableFromGrid").val();
            $("#LogTable tbody").empty();
            $("#GridPagingContainer ul").empty();
            i = 0;

            $.each(data.d.GridData, function (count, gridObject) {

                var tr = "";

                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:10%;'>" + moment(gridObject.LogDate).format("DD/MM/YYYY") + "</td>";
                //tr += "<td style='width:10%;'>" + gridObject.CompanyName + "</td>";

                if (isCompanyHyperlinkEnableFromGrid == "1") {
                    tr += "<td align='left'  style='width: 10%;cursor:pointer' title='Company Information details' onClick='javascript:return LoadCompanyDetails(" + gridObject.CompanyId + ")'>" + gridObject.CompanyName + "</td>";
                }
                else {
                    tr += "<td align='left' style='width:10%;'>" + gridObject.CompanyName + "</td>";
                }


                tr += "<td style='width:10%;'>" + gridObject.IndustryName + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.LogType + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.ParticipantFromParty + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.CompanyParticipant + "</td>";
                tr += "<td style='width:10%;'>" + gridObject.AccountManager + "</td>";

                tr += "<td style='width:10%;'> <a onclick=\"javascript:return FillFormEdit(" + gridObject.Id + ");\" title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'DeleteLog(" + gridObject.Id + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'ShowLogDetails(" + gridObject.Id + ")' ><img alt='Delete' src='../Images/detailsInfo.png' /></a>";

                tr += "<td style='display:none'>" + gridObject.Id + "</td>";


                tr += "</tr>";

                $("#LogTable tbody").append(tr);

                tr = "";
                i++;
            });
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.NextButton);
            return false;
        }
        function ShowLogDetails(id) {
            var iframeid = 'logDetails';
            var url = "./LogActivity.aspx?sceid=" + id;
            parent.document.getElementById(iframeid).src = url;
            $("#LogDetailsDiv").dialog({
                autoOpen: true,
                modal: true,
                width: "90%",
                height: 500,
                closeOnEscape: false,
                resizable: false,
                title: "Log Details",
                show: 'slide'
            });
            return false;
        }
        function DeleteLog(logId) {

            if (!confirm("Do you want to delete?")) { return false; }

            var iframeid = 'logDoc';
            var url = "../SalesAndMarketing/SalesCallEntry.aspx?id=" + logId + "&t=d";
            parent.document.getElementById(iframeid).src = url;
        }
        function FillFormEdit(id) {
            var logId = $("#hfLogId").val();
            var iframeid = 'frmPrint';
            var url = "../SalesAndMarketing/SalesCallEntry.aspx?id=" + id + "&from=MenuLinks";
            document.getElementById(iframeid).src = url;
            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "90%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                title: "Log Entry",
                show: 'slide'
            });
            return false;
        }
        function OpenReport() {
            GridPaging(1, 1);
            var company = $("#ContentPlaceHolder1_txtCompanyForSrc").val();
            var accountManager = $("#ContentPlaceHolder1_ddlCompanyOwner").val();
            var logType = $("#ContentPlaceHolder1_ddlLogType").val();
            var fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();
            var industry = $("#ContentPlaceHolder1_txtIndustry").val();

            var iframeid = 'frmPrint';
            var url = "../SalesAndMarketing/Reports/LogActivityEmptyMasterPageReport.aspx?company=" + company + "&accountManager=" + accountManager + "&logtype=" + logType + "&fromDate=" + fromDate + "&toDate=" + toDate + "&industry=" + industry;
            document.getElementById(iframeid).src = url;
            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "90%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                title: "Log Entry Report",
                show: 'slide'
            });
            return false;
        }
        function CloseLog() {
            $("#SalesNoteDialog").dialog('close');
            return false;
        }

    </script>
    <input id="hfLogId" type="hidden" value="0" />
    <asp:HiddenField ID="hfIsCompanyHyperlinkEnableFromGrid" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCompanyId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfIndustryId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfContactId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfDealId" runat="server" Value="0"></asp:HiddenField>

    <div id="LogEntryPage" style="display: none;">
        <iframe id="logDoc" name="logDoc" width="100%" height="650" frameborder="0" style="overflow: hidden;"></iframe>
    </div>
    <div id="SalesNoteDialog" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Log Activity Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Log Type</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlLogType" TabIndex="1" CssClass="form-control" runat="server">
                            <asp:ListItem Text="--- All ---" Value=""></asp:ListItem>
                            <asp:ListItem Text="Log a Call" Value="Log a call"></asp:ListItem>
                            <asp:ListItem Text="Log a Message" Value="Log a message"></asp:ListItem>
                            <asp:ListItem Text="Log an Email" Value="Log an email"></asp:ListItem>
                            <asp:ListItem Text="Log a Meeting" Value="Log a meeting"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Account Manager</label>
                    </div>
                    <div class="col-sm-4">
                        <asp:DropDownList ID="ddlCompanyOwner" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">From Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">To Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Industry</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtIndustry" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Company</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCompanyForSrc" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Contact</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtContact" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Deal</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtDeal" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return GridPaging(1,1);" />
                        <asp:Button ID="btnClean" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return Clean();" />
                        <asp:Button ID="btnCreateNew" runat="server" Text="New Log Entry" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateNew();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information
            <input style="float: right; padding: 0px 20px 0px 20px;" id="btnReport" type="button" class="TransactionalButton btn btn-primary btn-sm"
                value="Log Activity Report" onclick="OpenReport()" />
        </div>
        <div class="panel-body">
            <div class="form-group" id="LogTableContainer">
                <table class="table table-bordered table-condensed table-responsive" id="LogTable"
                    style="width: 100%;">
                    <thead>
                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                            <th style="width: 10%;">Date
                            </th>
                            <th style="width: 10%;">Company
                            </th>
                            <th style="width: 10%;">Industry
                            </th>
                            <th style="width: 10%;">Log Type
                            </th>
                            <th style="width: 20%;">Participants
                            </th>
                            <th style="width: 20%;">Team Member
                            </th>
                            <th style="width: 10%;">Account Manager
                            </th>
                            <th style="width: 10%;">Action
                            </th>
                            <th style="display: none">Id
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <div class="childDivSection">
                    <div class="text-center" id="GridPagingContainer">
                        <ul class="pagination">
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="LogDetailsDiv" style="display: none">
        <iframe id="logDetails" name="logDetails" width="100%" height="700" style="overflow: hidden; border: none;"></iframe>
    </div>
</asp:Content>
