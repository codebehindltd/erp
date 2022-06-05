﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true" CodeBehind="frmEmpPayScale.aspx.cs" Inherits="HotelManagement.Presentation.Website.Payroll.frmEmpPayScale" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>HR Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Employee Payscale</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
        });

        $(document).ready(function () {
            $("#<%= ddlGrade.ClientID %>").change(function () {
                var gradeValue = $("#<%= ddlGrade.ClientID %>").val();
                LoadOnDDLChange(gradeValue);
                EntryPanelVisibleTrue();
            });
        });
        function LoadOnDDLChange(gradeValue) {
            PageMethods.OnDDLChange(gradeValue, OnChangeSucceeded, OnFillFormObjectFailed);
            EntryPanelVisibleTrue();

            return false;
        }

        function OnChangeSucceeded(result) {

            if (result.PayScaleId > 0) {

                $("#<%=txtPayScaleId.ClientID %>").val(result.PayScaleId);
                $("#<%=btnSave.ClientID %>").val("Update");
                $("#<%= ddlGrade.ClientID %>").val(result.GradeId);
                $("#<%= txtBasicAmount.ClientID %>").val(result.BasicAmount);
            }
            return false;
        }



        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            EntryPanelVisibleTrue();
            return false;
        }
        function OnFillFormObjectSucceeded(result) {
            $("#<%=txtPayScaleId.ClientID %>").val(result.PayScaleId);
            $("#<%=btnSave.ClientID %>").val("Update");
            $("#<%= ddlGrade.ClientID %>").val(result.GradeId);
            $("#<%= txtBasicAmount.ClientID %>").val(result.BasicAmount);
            return false;
        }
        function EntryPanelVisibleTrue() {
            $('#btnNewPayScale').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }

        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }
        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewPayScale').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewPayScale').hide("slow");
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewPayScale').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }


        function PerformClearAction() {

            $("#<%=txtPayScaleId.ClientID %>").val("");
            $("#<%=ddlGrade.ClientID %>").val(0);

            $("#<%=txtBasicAmount.ClientID %>").val("");
            $("#<%=btnSave.ClientID %>").val("Save");
            MessagePanelHide();
            return false;
        }

        function OnFillFormObjectFailed(error) {
            alert(error.get_message());
        }
        //For Delete-------------------------        
        function PerformDeleteAction(actionId) {
            var answer = confirm("Do you want to delete this record?")
            if (answer) {
                PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            }
            return false;
        }

        function OnDeleteObjectSucceeded(result) {
            $("#<%=txtPayScaleId.ClientID %>").val("");
            window.location = "frmEmpPayScale.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            alert(error.get_message());
        }
</script>


    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>


    <div id="btnNewPayScale" class="btn-toolbar">
        <button onclick="javascript: return EntryPanelVisibleTrue();" class="btn btn-primary">
            <i class="icon-plus"></i>New Pay Scale</button>
        <div class="btn-group">
        </div>
    </div>


    <div id="EntryPanel" class="block" style="display: none;">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Leave Type Information
        </a>
        <div class="HMBodyContainer">
            <div class="block-body collapse in">

                <div class="HMContainerRow">
                    <div class="l-left">
                        <%--Left Left--%>
                        <asp:Label ID="lblGrade" runat="server" Text="Grade"></asp:Label>
                    </div>
                    <div class="r-left">
                        <%--Right Left--%>
                        <asp:DropDownList ID="ddlGrade" runat="server" CssClass="tdLeftAlignWithSize">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="divClear">
                </div>

                <div class="HMContainerRow">
                    <div class="l-left">
                        <%--Left Left--%>
                        <asp:HiddenField ID="txtPayScaleId" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblBasicAmount" runat="server" Text="Basic Amount"></asp:Label>
                    </div>
                    <div class="r-left">
                        <%--Right Left--%>
                        <asp:TextBox ID="txtBasicAmount" runat="server" CssClass="customLargeTextBoxSize"></asp:TextBox>
                    </div>
                </div>

                <div class="divClear">
                </div>
                <div class="HMContainerRowButton">
                    <%--Right Left--%>
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-primary"
                        OnClientClick="javascript: return PerformClearAction();" />
                    <asp:Button ID="btnCancel" runat="server" Text="Close" CssClass="btn btn-primary"
                        OnClientClick="javascript: return EntryPanelVisibleFalse();" />
                </div>
            </div>
        </div>
    </div>

    <div class="divClear">
    </div>
    <div id="SearchPanel" class="block">
        <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Information
        </a>

        <div class="block-body collapse in">
            <asp:GridView ID="gvEmpPayScale" Width="100%" runat="server" AllowPaging="True"
                AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True"
                ForeColor="#333333" OnPageIndexChanging="gvEmpPayScale_PageIndexChanging"
                OnRowDataBound="gvEmpPayScale_RowDataBound">
                <RowStyle BackColor="#E3EAEB" />
                <Columns>
                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("PayScaleId") %>'></asp:Label></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="BasicAmount" HeaderText="Basic Amount" ItemStyle-Width="50%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Grade" HeaderText="Grade" ItemStyle-Width="35%">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />

                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                ImageUrl="~/Images/delete.png" Text="" AlternateText="Delete" ToolTip="Delete" />
                        </ItemTemplate>
                        <ControlStyle Font-Size="Small" />
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <EmptyDataTemplate>
                    <asp:Label ID="lblRecordNotFound" runat="server" Text="Record Not Found."></asp:Label>
                </EmptyDataTemplate>
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#44545E" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#7C6F57" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
        </div>
    </div>

    <script type="text/javascript">
        var x = '<%=isMessageBoxEnable%>';
        if (x > -1) {
            MessagePanelShow();
            if (x == 2) {
                $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
            } 
        }
        else {
            MessagePanelHide();
        }

        var xNewAdd = '<%=isNewAddButtonEnable%>';
        if (xNewAdd > -1) {
            NewAddButtonPanelShow();
        }
        else {
            NewAddButtonPanelHide();
        }
        
    </script>
</asp:Content>
