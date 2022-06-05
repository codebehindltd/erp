<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmServiceChargeDistribution.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmServiceChargeDistribution" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Payroll Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Service Charge Distribution</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });

    </script>
    <div id="EntryPanel" class="panel panel-default">       
            <div class="panel-heading">Service Amount Distribution</div>
        <div class="panel-body">
        <div class="form-horizontal">   
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblProcessDate" runat="server" class="control-label" Text="Salary Month"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlProcessMonth" runat="server" CssClass="form-control"
                        TabIndex="2">
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Process Year"></asp:Label>                    
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlYear" CssClass="form-control" runat="server">
                    </asp:DropDownList>
                </div>
            </div>            
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="Label5" runat="server" class="control-label" Text="Service Amount"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtServiceAmount" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>            
            <div class="row">
 <div class="col-md-12">
                <asp:Button ID="btnSave" runat="server" TabIndex="4" Text="Save" CssClass="TransactionalButton btn btn-primary"
                    OnClick="btnSave_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" TabIndex="5" CssClass="TransactionalButton btn btn-primary"
                    OnClick="btnClear_Click" OnClientClick="return confirm('Do you want to Clear?');"/>
            </div>
            </div>
        </div>
        </div>
    </div>    
</asp:Content>
