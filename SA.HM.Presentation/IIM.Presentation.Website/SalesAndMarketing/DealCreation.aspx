<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="DealCreation.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.DealCreation" %>

<%--<%@ Register TagPrefix="UserControl" TagName="Deal" Src="~/SalesAndMarketing/Deal.ascx" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var minEndDate;
        $(document).ready(function () {
            GridPaging(1, 1);

            $("#ContentPlaceHolder1_ddlSearchCompany").select2({
                tags: false,
                allowClear: true,
                placeholder:"",
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlSearchDealOwner").select2({
                tags: false,
                allowClear: true,
                placeholder:"",
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlSearchDealStage").select2({
                tags: false,
                allowClear: true,
                placeholder:"",
                width: "99.75%"
            });

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            });

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            });
        });

        function CreateNewDeal() {
            var iframeid = 'frmPrint';
            var url = "./Deal.aspx";
            parent.document.getElementById(iframeid).src = url;

            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: '100%',
                height: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "New Deal",
                show: 'slide'
            });
        }

        function PerformClearAction() {
            $("#ContentPlaceHolder1_txtSearchDealName").val('');
            $("#ContentPlaceHolder1_txtDealNumber").val('');
            $("#ContentPlaceHolder1_ddlSearchDealStage").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlSearchCompany").val("0").trigger('change');

            return false;
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var id = 0, dealName = "", contactId = 0, companyId = 0, dealStageId = 0;

            var gridRecordsCount = $("#DealsGrid tbody tr").length;
            var ownerId = $("#ContentPlaceHolder1_ddlSearchDealOwner").val();
            var dealNumber = $("#ContentPlaceHolder1_txtDealNumber").val();
            dealName = $("#ContentPlaceHolder1_txtSearchDealName").val();
            dealStageId = $("#ContentPlaceHolder1_ddlSearchDealStage").val();
            companyId = $("#ContentPlaceHolder1_ddlSearchCompany").val();
            //contactId = $("#ContentPlaceHolder1_ddlSearchContact").val();

            if (ownerId == null) {
                toastr.warning("Select Account Manager.");
                $("#ContentPlaceHolder1_ddlSearchDealOwner").focus();
                return false;
            }
            if (dealStageId == null) {
                toastr.warning("Select Deal Stage.");
                $("#ContentPlaceHolder1_ddlSearchDealStage").focus();
                return false;
            }
            if (companyId == null) {
                toastr.warning("Select Company.");
                $("#ContentPlaceHolder1_ddlSearchCompany").focus();
                return false;
            }

            var dateType = $("#ContentPlaceHolder1_ddlDateType").val();
            var fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();

            if (fromDate != "") {
                fromDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtFromDate").val(), '/');
            }
            if (toDate != "") {
                fromDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtToDate").val(), '/');
            }

            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/DealCreation.aspx/LoadGridPaging',
                data: JSON.stringify({ ownerId: ownerId, dealNumber: dealNumber, name: dealName, stageId: dealStageId, companyId: companyId, dateType: dateType, fromDate: fromDate, toDate: toDate, gridRecordsCount: gridRecordsCount, pageNumber: pageNumber, IsCurrentOrPreviousPage: IsCurrentOrPreviousPage }),
                dataType: "json",
                success: function (data) {

                    LoadTable(data);
                },
                error: function (result) {
                }
            });
            return false;
        }

        function LoadTable(searchData) {

            var rowLength = $("#DealsGrid tbody tr").length;
            var dataLength = searchData.length;
            $("#DealsGrid tbody").empty();
            $("#GridPagingContainer ul").empty();
            i = 0;

            if (searchData.d.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"8\" >No Data Found</td> </tr>";
                $("#DealsGrid tbody ").append(emptyTr);
                return false;
            }

            $.each(searchData.d.GridData, function (count, gridObject) {
                var tr = "";

                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {

                    tr = "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='display:none'>" + gridObject.Id + "</td>";
                tr += "<td style='width:10%'>" + gridObject.DealNumber + "</td>";
                tr += "<td style='width:15%'>" + "<a title='Deal Details' href='javascript:void();'style='color:#333333;' onclick= 'javascript:return GoToDetails(" + gridObject.Id + "," + gridObject.CompanyId + ")' >" + gridObject.Name + "</a>" + "</td>";
                tr += "<td style='width:5%;'>" + gridObject.Amount + "</td>";
                tr += "<td style='width:15%;'>" + gridObject.Stage + "</td>";
                tr += "<td style='width:20%;'>" + (gridObject.CompanyId != 0 ? gridObject.Company : gridObject.ContactName) + "</td>";
                tr += "<td style='width:10%;'>" + CommonHelper.DateFromStringToDisplay(gridObject.StartDate, innBoarDateFormat) + "</td>";
                tr += "<td style='width:10%;'>" + (gridObject.CloseDate != null ? CommonHelper.DateFromStringToDisplay(gridObject.CloseDate, innBoarDateFormat) : "") + "</td>";
                tr += "<td style='width:15%;cursor:pointer;'>";

                if (!gridObject.IsEditNDeleteDisable) {
                    tr += '<a href="javascript:void();" onclick="javascript:return FillFormEdit(' + gridObject.Id + ",\'" + gridObject.Name + '\');"' + "title='Edit' ><img src='../Images/edit.png' alt='Edit'></a>"
                    if (!gridObject.IsCanDelete == false) {
                        tr += '&nbsp;&nbsp;<a href="javascript:void();" onclick= "javascript:return DeleteDeal(' + gridObject.Id + ",\'" + gridObject.Name + '\');" ><img alt="Delete" src="../Images/delete.png" /></a>';
                    }
                }

                if (gridObject.IsQuotationReview) {
                    if (gridObject.QuotationId == 0)
                        tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'javascript:return CreateQuotation(" + gridObject.Id + "," + gridObject.CompanyId + "," + gridObject.ContactId + ");' title='Create Quotation' ><img alt='Create Quotation' src='../Images/salesQuotation.png' /></a>";
                    tr += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'javascript:return SearchQuotation(" + gridObject.Id + ");' title='Search Quotation' ><img style='width:16px;height:16px;' alt='Search Quotation' src='../Images/SearchItem.png' /></a>";
                }
                if (gridObject.IsSiteSurvey) {
                    tr += '&nbsp;&nbsp;<a href="javascript:void();" onclick= "javascript:return SiteSurveyNote(' + gridObject.Id + "," + gridObject.CompanyId + "," + gridObject.ContactId + ",\'" + gridObject.Name + '\');" title="Create Site Survey"><img style="width:16px;height:16px;" alt="Documents" src="../Images/detailsInfo.png" /></a>';
                    tr += '&nbsp;&nbsp;<a href="javascript:void();" onclick= "javascript:return SiteSurveySearch(' + gridObject.Id + "," + gridObject.CompanyId + "," + gridObject.ContactId + ",\'" + gridObject.Name + '\');" title="Site Survey Search" ><img style="width:16px;height:16px;" alt="Site Survey Search" src="../Images/SearchItem.png" /></a>';
                }

                tr += '&nbsp;&nbsp;<a href="javascript:void();" onclick= "javascript:return ShowDealDocuments(' + gridObject.Id + ');" title="Documents"><img style="width:16px;height:16px;" alt="Documents" src="../Images/document.png" /></a>';
                tr += "</td>";
                tr += "<td style='display:none'>" + gridObject.OwnerId + "</td>";
                tr += "<td style='display:none'>" + gridObject.CompanyId + "</td>";
                tr += "</tr>";

                $("#DealsGrid tbody").append(tr);

                tr = "";
                i++;
            });

            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.NextButton);
            return false;
        }
        function SiteSurveyNote(dealId, compantId, contactId, name) {
            var iframeid = 'frmPrint';
            var url = "./SiteSurveyNote.aspx?did=" + dealId + "&cid=" + compantId + "&conid=" + contactId;
            parent.document.getElementById(iframeid).src = url;

            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "100%",
                height: 600,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Site Survey Note - " + name,
                show: 'slide'
            });

            return false;
        }

        function SiteSurveySearch(deal, company, Contact, name) {

            var iframeid = 'frmPrint';
            var url = "./SiteSurveyInformation.aspx?dId=" + deal + "&cId=" + company + "&conId=" + Contact;
            parent.document.getElementById(iframeid).src = url;

            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "100%",
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,

                title: "Search Site Survey Note - " + name,
                show: 'slide'
            });

            return false;
        }

        function FillFormEdit(id, name) {
            if (!confirm("Do you want to edit - " + name + "?")) {
                return false;
            }
            var iframeid = 'frmPrint';
            var url = "./Deal.aspx?did=" + id;
            parent.document.getElementById(iframeid).src = url;

            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "100%",
                height: 600,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Update - " + name,
                show: 'slide'
            });

            return false;
        }

        function DeleteDeal(id, name) {
            if (confirm("Want to delete " + name + " ?")) {
                $.ajax({

                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../SalesAndMarketing/DealCreation.aspx/DeleteDeal',
                    data: JSON.stringify({ Id: id }),
                    dataType: "json",
                    success: function (data) {
                        CommonHelper.AlertMessage(data.d.AlertMessage);
                        GridPaging(1, 1);
                    },
                    error: function (result) {

                    }
                });
            }
            return false;
        }

        function CreateQuotation(dealId, companyId, contactId) {
            var iframeid = 'frmPrint';
            var url = "./frmSMQuotation.aspx?did=" + dealId + "&cid=" + companyId + "&conid=" + contactId;
            parent.document.getElementById(iframeid).src = url;
            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "100%",
                height: 580,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Create Quotation",
                show: 'slide',
                
            });
            return false;
        }

        function SearchQuotation(dealId) {
            var iframeid = 'frmPrint';
            var url = "./SMQuotationSearch.aspx?did=" + dealId;
            parent.document.getElementById(iframeid).src = url;
            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "100%",
                height: 700,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Search Quotation",
                show: 'slide'
            });
            return false;
        }
        function CreateSalesNote(QuotationId) {
            var iframeid = 'frmPrint';
            var url = "./SalesNoteEntry.aspx?qid=" + QuotationId;
            parent.document.getElementById(iframeid).src = url;
            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "100%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Sales Note",
                show: 'slide'
            });
        }
        function CloseDialog() {
            $("#SalesNoteDialog").dialog('close');
            return false;
        }
        function GoToDetails(dealId, companyId, contactId) {
            window.location.href = "./DealInformation.aspx?did=" + dealId + "&cid=" + companyId;
            return false;
        }
        function ShowInvoice(quotationId) {
            var url = "/SalesAndMarketing/Reports/SMQuotationDetailsReport.aspx?id=" + quotationId;
            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=800,height=680,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
        }

        function ShowDealDocuments(id) {
            PageMethods.LoadDealDocument(id, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }
        function OnLoadDocumentSucceeded(result) {
            $("#imageDiv").html(result);

            $("#dealDocuments").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                minHeight: 400,
                closeOnEscape: true,
                resizable: false,
                title: "Deal Documents",
                show: 'slide'
            });

            return false;
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }
        function LoadDealManagement() {
            var id = 0, dealName = "", contactId = 0, companyId = 0, dealStageId = 0;
            var ownerId = $("#ContentPlaceHolder1_ddlSearchDealOwner").val();
            dealName = $("#ContentPlaceHolder1_txtSearchDealName").val();
            dealStageId = $("#ContentPlaceHolder1_ddlSearchDealStage").val();
            companyId = $("#ContentPlaceHolder1_ddlSearchCompany").val();
            //contactId = $("#ContentPlaceHolder1_ddlSearchContact").val();

            var dateType = $("#ContentPlaceHolder1_ddlDateType").val();
            var fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();

            if (fromDate != "") {
                fromDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtFromDate").val(), '/');
            }
            if (toDate != "") {
                fromDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtToDate").val(), '/');
            }
            window.location.href = "./DealStageWiseDashboardIframe.aspx?sid=" + dealStageId + "&cid=" + companyId + "&oid=" + ownerId + "&dname=" + dealName + "&dty=" + dateType + "&fd=" + fromDate + "&td=" + toDate;
        }
    </script>
    <div id="dealDocuments" style="display: none;">
        <div id="imageDiv"></div>
    </div>
    <asp:HiddenField ID="hfContactPersonId" runat="server" Value="0"></asp:HiddenField>

    <button id="btnCloseDialog" onclick="return CloseDialog()" style="display: none;"></button>
    <div id="SalesNoteDialog" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server" 
            clientidmode="static"></iframe>
    </div>

    <div class="panel panel-default">
        <div class="panel-heading">
            Deal Information
            
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblDealOwner" runat="server" class="control-label required-field" Text="Account Manager"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSearchDealOwner" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label" Text="Deal Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtDealNumber" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                    </div>

                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblDealName" runat="server" class="control-label" Text="Deal Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchDealName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblDealStage" runat="server" class="control-label required-field" Text="Deal Stage"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSearchDealStage" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Date Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlDateType" runat="server" CssClass="form-control" TabIndex="6">
                            <asp:ListItem Text="Start Date" Value="StartDate" />
                            <asp:ListItem Text="End Date" Value="EndDate" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblCompany" runat="server" class="control-label required-field" Text="Company"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSearchCompany" runat="server" CssClass="form-control" TabIndex="6">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label required-field" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDate" runat="server" class="control-label required-field" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group" style="display: none;">
                    <div class="col-md-2">
                        <asp:Label ID="lblContact" runat="server" class="control-label" Text="Contact Person"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlContact" runat="server" CssClass="form-control" TabIndex="7">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm" value="Search" onclick="javascript: return GridPaging(1, 1);" />

                        <input type="button" id="btnSearchCancel" class="TransactionalButton btn btn-primary btn-sm" value="Clear" onclick="javascript: return PerformClearAction();" />
                        <input type="button" id="btnNewDealInfo" class="TransactionalButton btn btn-primary btn-sm" value="New Deal" onclick="javascript: return CreateNewDeal();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Search Information
            <a style="float: right; padding: 0px;" href='javascript:void();' onclick='javascript:return LoadDealManagement()' title='Deal Management'>
                <img style='width: 22px; height: 20px;' alt='Search Quotation' src='../Images/management.png' /></a>
        </div>
        <div class="panel-body">
            <table id="DealsGrid" class="table table-bordered table-condensed table-responsive" width="100%">
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                        <th style="display: none">Id
                        </th>
                        <th style="width: 10%;">Deal Number
                        </th>
                        <th style="width: 15%;">Deal Name
                        </th>
                        <th style="width: 5%;">Amount
                        </th>
                        <th style="width: 15%;">Deal Stage
                        </th>
                        <th style="width: 20%;">Company / Contact person
                        </th>
                        <th style="width: 10%;">Start Date
                        </th>
                        <th style="width: 10%;">End Date
                        </th>
                        <th style="width: 15%;">Action
                        </th>
                        <th style="display: none">DealOwnerID
                        </th>
                        <th style="display: none">CompanyId
                        </th>
                        <th style="display: none">ContactId
                        </th>
                        <th style="display: none">DealStageID
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

</asp:Content>
