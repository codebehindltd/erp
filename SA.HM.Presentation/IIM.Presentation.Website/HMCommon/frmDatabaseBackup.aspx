<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmDatabaseBackup.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.frmDatabaseBackup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Company Information</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Database Backup</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

        });
    </script>
    <div id="EntryPanel" class="panel panel-default">        
        <div class="panel-heading">Database Backup</div> 
        <div class="panel-body">          
            <div class="form-horizontal">
                <asp:Button ID="btnDatabaseBackup" runat="server" TabIndex="3" Text="Database Backup"
                    CssClass="TransactionalButton btn btn-primary btn-sm" OnClick="btnDatabaseBackup_Click" />
            </div>
        </div>
    </div>   
</asp:Content>
