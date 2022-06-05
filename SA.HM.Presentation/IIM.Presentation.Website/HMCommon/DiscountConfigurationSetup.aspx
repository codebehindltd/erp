<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="DiscountConfigurationSetup.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.DiscountConfigurationSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
        });
        function SaveDiscountValidation() {
            var DisIndividual = $("#ContentPlaceHolder1_rbDisIndividual").is(":checked");
            var DisMaxOneForMulti = $("#ContentPlaceHolder1_rbDisMaxOneForMulti").is(":checked");
            var ShowDisForMulti = $("#ContentPlaceHolder1_rbShowDisForMulti").is(":checked");
            if (DisIndividual == false && DisMaxOneForMulti == false && ShowDisForMulti == false) {
                toastr.warning("Please select one discount from Discount Applicable");
                return false;
            }
            return true;
        }
    </script>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Discount Configuration Setup
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <fieldset class="col-md-6">
                    <legend>Discount Applicable</legend>
                    <div>
                        <div class="form-group" style="display:none;">
                            <div class="col-md-12">
                                <asp:RadioButton ID="rbDisIndividual" GroupName="DicountPolicy" runat="server" />
                                &nbsp;
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="Is Discount Applicable Individually "></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:RadioButton ID="rbDisMaxOneForMulti" GroupName="DicountPolicy" runat="server" />&nbsp;
                                <asp:Label ID="Label1" runat="server" class="control-label" Text="Is Discount Applicable Max One When Multiple "></asp:Label>
                            </div>
                        </div>
                        <div class="form-group" style="display:none;">
                            <div class="col-md-12">
                                <asp:RadioButton ID="rbShowDisForMulti" GroupName="DicountPolicy" runat="server" />&nbsp;
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Is Discount Option Show When Multiple"></asp:Label>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <div class="form-group">
                    <div class="col-md-12">
                        <asp:CheckBox ID="chkDisWithMembership" runat="server" />
                        <asp:Label ID="Label4" runat="server" class="control-label" Text="Is Discount And Membership Discount Applicable Together"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-12">
                        <asp:CheckBox ID="chkDisForBank" runat="server" />
                        <asp:Label ID="Label5" runat="server" class="control-label" Text="Is Bank Discount Applicable"></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-12">
                        <asp:Button ID="btnSaveDiscount" runat="server" Text="Save" TabIndex="2"
                            CssClass="TransactionalButton btn btn-primary btn-sm" OnClick="btnSave_Click" OnClientClick="javascript:return SaveDiscountValidation()" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

