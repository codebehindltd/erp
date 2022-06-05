<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="DealStageWiseDashboardIframe.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.DealStageWiseDashboardIframe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            var dealStageId = "", companyId = "", contactId = "", ownerId = "", dealName = "";
            var dateType = "";
            var fromDate = "";
            var toDate = "";
            if ($.trim(CommonHelper.GetParameterByName("cid")) != "")
                companyId = parseInt($.trim(CommonHelper.GetParameterByName("cid")), 10);
            else
                companyId = "";

            if ($.trim(CommonHelper.GetParameterByName("sid")) != "")
                dealStageId = parseInt($.trim(CommonHelper.GetParameterByName("sid")), 10);
            else
                dealStageId = "";

            if ($.trim(CommonHelper.GetParameterByName("oid")) != "")
                ownerId = parseInt($.trim(CommonHelper.GetParameterByName("oid")), 10);
            else
                ownerId = "";
            if ($.trim(CommonHelper.GetParameterByName("dname")) != "")
                dealName = $.trim(CommonHelper.GetParameterByName("dname"));
            else
                dealName = "";
            if ($.trim(CommonHelper.GetParameterByName("dty")) != "")
                dateType = $.trim(CommonHelper.GetParameterByName("dty"));
            else
                dateType = "";
            if ($.trim(CommonHelper.GetParameterByName("fd")) != "")
                fromDate = $.trim(CommonHelper.GetParameterByName("fd"));
            else
                fromDate = "";
            if ($.trim(CommonHelper.GetParameterByName("td")) != "")
                toDate = $.trim(CommonHelper.GetParameterByName("td"));
            else
                toDate = "";

            var iframeid = 'frmPrint';
            var url = "./DealStageWiseDashboard.aspx?sid=" + dealStageId + "&cid=" + companyId + "&oid=" + ownerId + "&dname=" + dealName + "&dty=" + dateType + "&fd=" + fromDate + "&td=" + toDate;
            parent.document.getElementById(iframeid).src = url;
            $("#SalesNoteDialog").show();

            $("#btnGoBack").click(function () {
                var isFirefox = typeof InstallTrigger !== 'undefined';
                if (isFirefox) {
                    window.history.go(-2);
                }
                else {
                    window.history.back(-1);
                }

                return false;
            });
        });
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
            setTimeout(ReloadFrame, 4000);
        }
        function ReloadFrame() {
            document.getElementById('frmPrint').src = document.getElementById('frmPrint').src;

        }
        function GoToDetails(dealId, companyId, contactId) {
            window.location.href = "./DealInformation.aspx?did=" + dealId + "&cid=" + companyId;
        }
    </script>
    <div id="SalesNoteDialog" style="display: none;">
        <div class="form-group">
            <div class="col-md-2 col-md-offset-11" style="padding-bottom: 5px;">
                <input class="TransactionalButton btn btn-primary btn-sm" type="button" value="Go Back" id="btnGoBack" title="Go Back" />
            </div>
        </div>
        <iframe id="frmPrint" name="IframeName" runat="server"
            clientidmode="static" style="height: 100vh; width: 100%"></iframe>
    </div>
</asp:Content>
