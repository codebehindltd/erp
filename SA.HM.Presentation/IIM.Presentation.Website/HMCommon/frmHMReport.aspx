<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HMReport.Master" AutoEventWireup="true" CodeBehind="frmHMReport.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.frmHMReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <script type="text/javascript">
     //Bread Crumbs Information-------------
     $(document).ready(function () {
         var moduleName = "";
         var formName = "<li class='active'>Report</li>";
         var breadCrumbs = moduleName + formName;
         $("#ltlBreadCrumbsInformation").html(breadCrumbs);
     });
    </script>
</asp:Content>
