<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="Mamun.Presentation.Website.Common.Footer" %>
<asp:Panel ID="Footer1" runat="server" Height="120px" Width="100%">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td colspan="2">
                <hr style="border-style: solid; border-color: #CCCCCC" />
            </td>
        </tr>
        <tr>
            <td style="text-align: left">
                Current User:&nbsp;<b><asp:Label ID="lblCurrentUser" runat="server" Text=""></asp:Label></b>
            </td>
            <td style="text-align: right">
                All right reserved <b>SmartAspects<sup><i style="font-size: 10px">TM</i></sup></b>
                &copy; 2013
            </td>
        </tr>
    </table>
</asp:Panel>
