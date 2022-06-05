<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="LogActivity.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.LogActivity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtFromDate, #txtToDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $("#filterArea").toggle();
            LoadLogActivity();

            $("#ContentPlaceHolder1_ddlLogOwner").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

        });

        function LoadLogActivity() {

            var logType = null, userId = null, dateFrom = null, dateTo = null;
            var companyId = null, contactId = null, dealId = null, salesCallEntryId = 0;

            if ($.trim(CommonHelper.GetParameterByName("cid")) != "")
                companyId = $.trim(CommonHelper.GetParameterByName("cid"));
            if ($.trim(CommonHelper.GetParameterByName("conid")) != "")
                contactId = $.trim(CommonHelper.GetParameterByName("conid"));
            if ($.trim(CommonHelper.GetParameterByName("did")) != "")
                dealId = $.trim(CommonHelper.GetParameterByName("did"));
            if ($.trim(CommonHelper.GetParameterByName("sceid")) != "")
                salesCallEntryId = $.trim(CommonHelper.GetParameterByName("sceid"));
            
            if ($("#ContentPlaceHolder1_ddlActivity").val() != "0") { logType = $("#ContentPlaceHolder1_ddlActivity").val(); }
            if ($("#ContentPlaceHolder1_ddlLogOwner").val() != "0") { userId = $("#ContentPlaceHolder1_ddlLogOwner").val(); }
            if ($("#txtFromDate").val() != "") { dateFrom = CommonHelper.DateFormatToMMDDYYYY($("#txtFromDate").val(), '/'); }
            if ($("#txtToDate").val() != "") { dateTo = CommonHelper.DateFormatToMMDDYYYY($("#txtToDate").val(), '/'); }

            CommonHelper.SpinnerOpen();
            PageMethods.LoadLoggedDetails(companyId, contactId, dealId, logType, userId, dateFrom, dateTo, salesCallEntryId, OnSaveSalesLogSucceed, OnSaveSalesLogFailed);

            return false;
        }
        function OnSaveSalesLogSucceed(result) {

            $("#loggedDetails").html(result);
            $("#filterArea").hide();
            CommonHelper.SpinnerClose();
            return false;
        }
        function OnSaveSalesLogFailed(error) {
            CommonHelper.SpinnerClose();
            toastr.error(error);
        }

        function EditLog(logId) {
            debugger;
            $(parent.document.getElementById("hfLogId")).val(logId);
            $(parent.document.getElementById("btnLogEntry")).trigger("click");
            return false;
        }

        function DeleteLog(logId) {

            if (!confirm("Do you want to delete?")) { return false; }

            var iframeid = 'logDoc';
            var url = "../SalesAndMarketing/SalesCallEntry.aspx?id=" + logId + "&t=d";
            parent.document.getElementById(iframeid).src = url;
        }

        function ToggleFilter() {
            $("#filterArea").toggle();
            return false;
        }

    </script>

    <div class="row">
        <div class="col-md-12" style="padding: 0px 30px 0px 30px; font-size: 14px; font-weight: bold;">
            <a href="javascript:void()" onclick="javascript: return ToggleFilter();">Filter Option</a>
            <hr style="margin: 2px 0px 1px 0px;" />
        </div>
    </div>

    <div class="panel panel-default" id="filterArea">
        <div class="panel panel-title" style="padding: 10px 15px 10px 15px;">
            Filter Options
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="row">
                    <div class="col-md-10 col-sm-10">
                        <div class="form-group">
                            <div class="col-sm-2 col-md-2">
                                <asp:Label ID="lblContactOwner" runat="server" class="control-label" Text="User"></asp:Label>
                            </div>
                            <div class="col-sm-4 col-md-4">
                                <asp:DropDownList ID="ddlLogOwner" runat="server" CssClass="form-control" TabIndex="3">
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-2 col-md-2">
                                <asp:Label ID="lblCompany" runat="server" class="control-label" Text="Activity"></asp:Label>
                            </div>
                            <div class="col-sm-4 col-md-4">
                                <asp:DropDownList ID="ddlActivity" runat="server" CssClass="form-control" TabIndex="3">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-sm-2 col-md-2">
                                <label class="control-label">Date From</label>
                            </div>
                            <div class="col-sm-4 col-md-4">
                                <input type="text" id="txtFromDate" class="form-control" />
                            </div>
                            <div class="col-sm-2 col-md-2">
                                <label class="control-label">To</label>
                            </div>
                            <div class="col-sm-4 col-md-4">
                                <input type="text" id="txtToDate" class="form-control" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-2 col-md-2">
                        <input type="button" value="Search" class="btn btn-primary" onclick="LoadLogActivity()" />
                    </div>
                </div>


            </div>
        </div>
    </div>

    <div id="loggedDetails">
        
    </div>

    <div id="LogEntryPage" style="display: none;">
        <iframe id="logDoc" name="logDoc" width="100%" height="650" frameborder="0" style="overflow: hidden;"></iframe>
    </div>

    <%-- 
    <div class="panel panel-default">
        <div class="panel panel-title" style="padding: 10px 15px 10px 15px;">
            <div class="row">
                <div class="col-md-6">Logged an email</div>
                <div class="col-md-3 text-right" style="font-size:13px;">
                    <a href="javascript:void()" title="Edit" onclick="javascript:return EditLog()">Edit</a>&nbsp;&nbsp;&nbsp;&nbsp;
                    <a href="javascript:void()" title="Delete" onclick="javascript:return DeleteLog()">Delete</a>
                </div>
                <div class="col-md-3 text-right">March 14 2019 5:24PM</div>
            </div>
        </div>
        <div class="panel-body">
            hkj ggg  cas da s d as d  asd as das das d ad a d as d ad a d a d  a da  da d a das d asd ad ad j hkj ggg  cas da s d as d  asd as das das d ad a d as d ad a d a d  a da  da d a das d asd ad ad j hkj ggg  cas da s d as d  asd as das das d ad a d as d ad a d a d  a da  da d a das d asd ad ad j
        </div>
    </div>--%>
</asp:Content>
