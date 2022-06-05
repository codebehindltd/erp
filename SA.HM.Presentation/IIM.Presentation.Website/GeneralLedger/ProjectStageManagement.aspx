<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="ProjectStageManagement.aspx.cs" Inherits="HotelManagement.Presentation.Website.GeneralLedger.ProjectStageManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var iframeid = 'frmPrint';
            var url = "./ProjectStageManagementIframe.aspx";
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
