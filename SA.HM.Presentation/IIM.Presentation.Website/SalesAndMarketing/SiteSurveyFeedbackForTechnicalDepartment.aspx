<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="SiteSurveyFeedbackForTechnicalDepartment.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.SiteSurveyFeedbackForTechnicalDepartment" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var minEndDate;
        var dealId = 0;
        var companyId = 0;
        var contactId = 0;
        $(document).ready(function () {
            GridPaging(1, 1);

            $("#ContentPlaceHolder1_ddlSearchCompany").select2({
                tags: false,
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlSearchDealStatus").select2({
                tags: false,
                allowClear: true,
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

        function PerformClearAction() {
            $("#ContentPlaceHolder1_txtSearchDealName").val('');
            $("#ContentPlaceHolder1_ddlSearchDealStatus").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlSearchCompany").val("0").trigger('change');

            return false;
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var id = 0, dealName = "", contactId = 0, companyId = 0, dealStageId = 0;

            var gridRecordsCount = $("#DealsGrid tbody tr").length;
            var dealNumber = $("#ContentPlaceHolder1_txtSearchDealNumber").val();
            dealName = $("#ContentPlaceHolder1_txtSearchDealName").val();
            dealStatus = $("#ContentPlaceHolder1_ddlSearchDealStatus").val();
            companyId = $("#ContentPlaceHolder1_ddlSearchCompany").val();
            var dateType = $("#ContentPlaceHolder1_ddlDateType").val();
            var fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();

            if (fromDate != "") {
                fromDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtFromDate").val(), '/');
            }
            if (toDate != "") {
                fromDate = CommonHelper.DateFormatToMMDDYYYY($("#ContnentPlaceHolder1_txtToDate").val(), '/');
            }

            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/SiteSurveyFeedbackForTechnicalDepartment.aspx/LoadGridPaging',
                data: JSON.stringify({ dealNumber: dealNumber, name: dealName, dealStatus: dealStatus, companyId: companyId, dateType: dateType, fromDate: fromDate, toDate: toDate, gridRecordsCount: gridRecordsCount, pageNumber: pageNumber, IsCurrentOrPreviousPage: IsCurrentOrPreviousPage }),
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
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"7\" >No Data Found</td> </tr>";
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
                tr += "<td style='width:20%'>" + gridObject.Name + "</td>";
                tr += "<td style='width:20%;'>" + (gridObject.CompanyId != 0 ? gridObject.Company : gridObject.ContactName) + "</td>";
                tr += "<td style='width:10%;'>" + CommonHelper.DateFromStringToDisplay(gridObject.StartDate, innBoarDateFormat) + "</td>";
                tr += "<td style='width:10%;'>" + (gridObject.CloseDate != null ? CommonHelper.DateFromStringToDisplay(gridObject.CloseDate, innBoarDateFormat) : "") + "</td>";
                tr += "<td style='width:15%;cursor:pointer;'>";

                if (gridObject.IsSiteSurvey) {
                    tr += '&nbsp;&nbsp;<a href="javascript:void();" onclick= "javascript:return SiteSurveySearch(' + gridObject.Id + ", \'" + gridObject.Name + '\');" title="Site Survey Search" ><img style="width:16px;height:16px;" alt="Site Survey Search" src="../Images/SearchItem.png" /></a>';
                }
                tr += "</td>";
                tr += "<td style='display:none'>" + gridObject.NumberId + "</td>";
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

        function SiteSurveySearch(deal, name) {
            dealId = deal;
            LoadGridForNote(1, 1);

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

        function CloseDialog() {
            $("#SalesNoteDialog").dialog('close');
            return false;
        }
        function GoToDetails(dealId, companyId, contactId) {
            window.location.href = "./DealInformation.aspx?did=" + dealId + "&cid=" + companyId;
            return false;
        }

        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }

        function CloseDialog() {
            $("#SiteSurveyDialogue").dialog('close');
            LoadGridForNote(1, 1);
            return false;
        }
        //function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
        //    LoadGridForNote(pageNumber, IsCurrentOrPreviousPage);
        //    return false;
        //}
        function LoadGridForNote(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#SourceTable tbody tr").length;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/SiteSurveyFeedbackForTechnicalDepartment.aspx/LoadSiteSurveyForSearch',

                data: "{'dealId':'" + dealId + "', 'gridRecordsCount':'" + gridRecordsCount + "', 'pageNumber':'" + pageNumber + "', 'isCurrentOrPreviousPage':'" + IsCurrentOrPreviousPage + "'}",
                dataType: "json",
                success: function (data) {
                    LoadTableForNote(data);
                    var dealId = 0;
                },
                error: function (result) {
                }
            });
            return false;
        }
        function LoadTableForNote(data) {

            $("#SourceTable tbody").empty();
            $("#GridPagingContainer ul").empty();
            var i = 0;

            $.each(data.d.GridData, function (count, gridObject) {

                var tr = "";

                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:20%;'>" + gridObject.Date + "</td>";
                tr += "<td style='width:35%;'>" + gridObject.Company + "</td>";
                tr += "<td style='width:25%;'>" + gridObject.Status + "</td>";
                tr += "<td style='width:20%;'>";
                if (gridObject.IsDealNeedSiteSurvey) {
                    tr += "&nbsp;&nbsp;<a onclick=\"javascript:return FillFormFeedback(" + gridObject.Id + ");\" title='Feedback' href='javascript:void();'><img src='../Images/note.png' alt='Feedback'></a>"
                }
                if (gridObject.Status == "Done")
                    tr += "&nbsp;&nbsp;<a onclick=\"javascript:return ApproveNote(" + gridObject.DealId + ");\" title='approved' href='javascript:void();'><img src='../Images/approved.png' alt='approve'></a>";

                tr += "&nbsp;&nbsp;<a onclick=\"javascript:return ShowDocument(" + gridObject.Id + ");\" title='Document' href='javascript:void();'><img src='../Images/document.png' alt='Document'></a>"
                tr += "&nbsp;&nbsp;<a onclick=\"javascript:return ShowDetails(" + gridObject.Id + ");\" title='Details' href='javascript:void();'><img src='../Images/detailsInfo.png' alt='Details'></a>"

                tr += "<td style='display:none'>" + gridObject.Id + "</td>";
                tr += "<td style='display:none'>" + gridObject.CompanyId + "</td>";
                tr += "<td style='display:none'>" + gridObject.DealId + "</td>";


                tr += "</tr>";

                $("#SourceTable tbody").append(tr);

                tr = "";
                i++;
            });
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.NextButton);
            return false;
        }

        function FillFormFeedback(id) {
            var iframeid = 'frmPrint';

            var url = "./SiteSurveyFeedback.aspx?ssid=" + id;
            document.getElementById(iframeid).src = url;
            $("#SiteSurveyDialogue").dialog({
                autoOpen: true,
                modal: true,
                width: 1100,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Site Survey Feedback",
                show: 'slide'
            });

            return false;
        }
        function ShowDocument(id) {
            PageMethods.LoadSurveyDocument(id, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }
        function OnLoadDocumentSucceeded(result) {
            $("#imageDiv").html(result);

            $("#SurveyNoteDocuments").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Site Survey Note Documents",
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
        function ShowDetails(id) {
            $("#ItemForFeedBackTbl tbody").empty();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/SiteSurveyFeedbackForTechnicalDepartment.aspx/CheckFeedback',
                data: "{'id':'" + id + "'}",
                dataType: "json",
                async: false,
                success: function (data) {

                    CheckFeedbackSucceed(data.d);
                },
                error: function (result) {
                }
            });
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/SiteSurveyFeedbackForTechnicalDepartment.aspx/LoadSurveyDetailsById',
                data: "{'id':'" + id + "'}",
                dataType: "json",
                async: false,
                success: function (data) {

                    LoadDetails(data);

                },
                error: function (result) {
                }
            });
            return false;
        }
        function CheckFeedbackSucceed(result) {
            if (result != 0) {
                $("#pnlFeedbackItem").show();
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../SalesAndMarketing/SiteSurveyFeedbackForTechnicalDepartment.aspx/LoadFeedbackDetailsById',
                    data: "{'result':'" + result + "'}",
                    dataType: "json",
                    async: false,
                    success: function (data) {

                        LoadTableForEdit(data.d.SMSiteSurveyFeedbackDetailsBOList);
                    },
                    error: function (result) {
                    }
                });
                return false;
            }
            else {
                $("#pnlFeedbackItem").hide();
            }
        }
        function LoadTableForEdit(data) {

            var tr = "";
            var totalRow = data.length, row = 0;
            var tr = "";

            for (row = 0; row < totalRow; row++) {
                tr += "<tr>";

                tr += "<td style='width:35%;'>" + data[row].ItemName + "</td>";
                tr += "<td style='width:25%;'>" + data[row].UnitHead + "</td>";
                tr += "<td style='width:20%;'>" + data[row].Quantity + "</td>";

                tr += "</tr>";

                $("#ItemForFeedBackTbl tbody").prepend(tr);
                tr = "";
            }
        }
        function LoadDetails(data) {
            $("#lblStatus").html(data.d.SiteSurveyNote.Status)
            $("#lblDealName").html(data.d.SiteSurveyNote.Deal);
            $("#lblDescription").html(data.d.SiteSurveyNote.Description);
            $("#lblSegment").html(data.d.SiteSurveyNote.Segment);

            if (data.d.GuestCompany != null) {
                $("#pnlCompany").show();
                $("#pnlContact").hide();
                $("#ContentPlaceHolder1_lblCompanyName").html(data.d.GuestCompany.CompanyName);
                $("#ContentPlaceHolder1_lblBillingStreet").html(data.d.GuestCompany.BillingStreet);
                $("#ContentPlaceHolder1_lblBillingCity").html(data.d.GuestCompany.BillingCity);
                $("#ContentPlaceHolder1_lblBillingState").html(data.d.GuestCompany.BillingState);
                $("#ContentPlaceHolder1_lblBillingCountry").html(data.d.GuestCompany.BillingCountry);
                $("#ContentPlaceHolder1_lblBillingPostCode").html(data.d.GuestCompany.BillingPostCode);
            }
            if (data.d.ContactInformation != null) {
                $("#pnlContact").show();
                $("#pnlCompany").hide();

                $("#ContentPlaceHolder1_lblContactName").html(data.d.ContactInformation.Name);
                $("#ContentPlaceHolder1_lblEmail").html(data.d.ContactInformation.Email);
                $("#ContentPlaceHolder1_lblMobilePersonal").html(data.d.ContactInformation.MobilePersonal);
                $("#ContentPlaceHolder1_lblPhonePersonal").html(data.d.ContactInformation.PhonePersonal);
                $("#ContentPlaceHolder1_lblMobileWork").html(data.d.ContactInformation.MobileWork);
                $("#ContentPlaceHolder1_lblPhoneWork").html(data.d.ContactInformation.PhoneWork);
            }



        }
        function ApproveNote(dealId) {
            if (!confirm("Want to Approve?"))
                return false;
            PageMethods.ApproveSiteSurveyNote(dealId, OnApproveNoteSucceeded, OnApproveNoteFailed);
            return false;
        }
        function OnApproveNoteSucceeded(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            $("#SalesNoteDialog").dialog('close');
            GridPaging(parseInt($("#GridPagingContainer").find("ul li.active").text()), 1);
            return false;
        }
        function OnApproveNoteFailed(error) {
            parent.ShowAlert(error.AlertMessage);
        }
    </script>
    <asp:HiddenField ID="hfContactPersonId" runat="server" Value="0"></asp:HiddenField>

    <button id="btnCloseDialog" onclick="return CloseDialog()" style="display: none;"></button>
    <div id="SalesNoteDialog" style="display: none;">
        <div id="SurveyNoteDocuments" style="display: none;">
            <div id="imageDiv"></div>
        </div>

        <div id="SiteSurveyDialogue" style="display: none;">
            <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
                clientidmode="static" scrolling="yes"></iframe>
        </div>
        <div class="col-md-6">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <div class="form-group" id="SourceTableContainer">
                        <table class="table table-bordered table-condensed table-responsive" id="SourceTable"
                            style="width: 100%;">
                            <thead>
                                <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                    <th style="width: 20%;">Date
                                    </th>
                                    <th style="width: 35%;">Company Name/Contact
                                    </th>
                                    <th style="width: 25%;">Status
                                    </th>
                                    <th style="width: 20%;">Action
                                    </th>
                                    <th style="display: none">Id
                                    </th>
                                    <th style="display: none">CompanyId
                                    </th>
                                    <th style="display: none">DealId
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
        </div>
        <div class="col-md-6">
            <div class="form-horizontal">
                <div class="row">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-md-4" style="font-size: 14px; font-weight: bold;">
                                    <span id="lblDealName"></span>
                                </div>
                            </div>
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <label class="control-label">Site Survey Feedback Status</label>
                                    </div>
                                    <div class="col-md-8">
                                        <span style="font-weight: bold; border: none">:&nbsp;</span>
                                        <label id="lblStatus" class="control-label"></label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <label class="control-label">Description</label>
                                    </div>
                                    <div class="col-md-8">
                                        <span style="font-weight: bold; border: none">:&nbsp;</span>
                                        <label id="lblDescription" class="control-label" style="text-align: left;"></label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <label class="control-label">Site Survey For</label>
                                    </div>
                                    <div class="col-md-8">
                                        <span style="font-weight: bold; border: none">:&nbsp;</span>
                                        <label id="lblSegment" class="control-label"></label>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div id="pnlCompany" class="panel panel-default">
                            <div class="panel-heading">
                                Company Information
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-4">
                                            <label class="control-label">Company Name</label>
                                        </div>
                                        <div class="col-md-8">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label CssClass="control-label" ID="lblCompanyName" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-4">
                                            <label class="control-label">Street</label>
                                        </div>
                                        <div class="col-md-8">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label CssClass="control-label" ID="lblBillingStreet" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-4">
                                            <label class="control-label">City</label>
                                        </div>
                                        <div class="col-md-8">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label CssClass="control-label" ID="lblBillingCity" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-4">
                                            <label class="control-label">State</label>
                                        </div>
                                        <div class="col-md-8">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label CssClass="control-label" ID="lblBillingState" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-4">
                                            <label class="control-label">Country</label>
                                        </div>
                                        <div class="col-md-8">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label CssClass="control-label" ID="lblBillingCountry" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-4">
                                            <label class="control-label">Post Code</label>
                                        </div>
                                        <div class="col-md-8">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label CssClass="control-label" ID="lblBillingPostCode" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="pnlContact" class="panel panel-default" style="display: none;">
                            <div class="panel-heading">
                                Contact Information
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-4">
                                            <label class="control-label">Contact Name</label>
                                        </div>
                                        <div class="col-md-8">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label CssClass="control-label" ID="lblContactName" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-4">
                                            <label class="control-label">Email</label>
                                        </div>
                                        <div class="col-md-8">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label CssClass="control-label" ID="lblEmail" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-4">
                                            <label class="control-label">Mobile(Personal)</label>
                                        </div>
                                        <div class="col-md-8">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label CssClass="control-label" ID="lblMobilePersonal" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-4">
                                            <label class="control-label">Phone(Personal)</label>
                                        </div>
                                        <div class="col-md-8">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label CssClass="control-label" ID="lblPhonePersonal" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-4">
                                            <label class="control-label">Mobile(Work)</label>
                                        </div>
                                        <div class="col-md-8">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label CssClass="control-label" ID="lblMobileWork" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-4">
                                            <label class="control-label">Phone(Work)</label>
                                        </div>
                                        <div class="col-md-8">
                                            <span style="font-weight: bold; border: none">:&nbsp;</span>
                                            <asp:Label CssClass="control-label" ID="lblPhoneWork" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="pnlFeedbackItem" class="panel panel-default" style="display: none;">
                            <div class="panel-heading">
                                Feedback Item Information
                            </div>
                            <div id="Item" style="overflow-y: scroll;">
                                <table id="ItemForFeedBackTbl" class="table table-bordered table-condensed table-hover">
                                    <thead>
                                        <tr>
                                            <th style="width: 45%;">Item Name</th>
                                            <th style="width: 30%;">Unit Head</th>
                                            <th style="width: 25%;">Quantity</th>

                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                    <tfoot></tfoot>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Site Servey Feedback
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblDealNumber" runat="server" class="control-label" Text="Deal Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchDealNumber" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblDealName1" runat="server" class="control-label" Text="Deal Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchDealName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblDealStage" runat="server" class="control-label" Text="Feedback Status"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSearchDealStatus" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Pending" Value="Pending" />
                            <asp:ListItem Text="Done" Value="Done" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblCompany" runat="server" class="control-label" Text="Company"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSearchCompany" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Date Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlDateType" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Start Date" Value="StartDate" />
                            <asp:ListItem Text="End Date" Value="EndDate" />
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

                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Search Information
        </div>
        <div class="panel-body">
            <table id="DealsGrid" class="table table-bordered table-condensed table-responsive" width="100%">
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                        <th style="display: none">Id
                        </th>
                        <th style="width: 25%;">Deal Name
                        </th>
                        <th style="width: 30%;">Company / Contact person
                        </th>
                        <th style="width: 15%;">Start Date
                        </th>
                        <th style="width: 15%;">End Date
                        </th>
                        <th style="width: 15%;">Action
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
