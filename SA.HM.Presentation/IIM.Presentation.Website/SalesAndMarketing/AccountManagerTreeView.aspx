<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="AccountManagerTreeView.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.AccountManagerTreeView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Tree View: Account Manager
        </div>
        <div class="panel-body">
            <asp:TreeView ID="tvLocations" runat="server" NodeStyle-CssClass="treeNode" SelectedNodeStyle-CssClass="treeNodeSelected"
                HoverNodeStyle-CssClass="treeNodeSelected" Style="font-size: 13px; font-family: Tahoma; font-weight: bold; text-align: left;"
                ShowLines="true">
                <Nodes>
                    <asp:TreeNode></asp:TreeNode>
                </Nodes>
            </asp:TreeView>
        </div>
    </div>
</asp:Content>
