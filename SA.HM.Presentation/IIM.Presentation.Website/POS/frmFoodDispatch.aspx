<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmFoodDispatch.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmFoodDispatch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            /*var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Food Dispatch</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);*/

            $('.RoomOccupaiedDiv').on('click', function (e) {
                pageTitle = "Food Dispatch Possible Path";
                var kotId = $(this).next(".RoomNumberDiv").text();

                PageMethods.LoadFoodDispatchPath(kotId, pageTitle, OnLoadFoodDispatchPathSucceeded, OnLoadFoodDispatchPathFailed);
                return false;
            });

            $("#btnBackToDecisionMaker").click(function () {
                popup(-1);

                $("#serviceDecider").dialog("close");

                $("#serviceDecider").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 600,
                    closeOnEscape: true,
                    resizable: false,
                    title: "Food Dispatch Possible Path",
                    show: 'slide'
                });

            });
        });

        function GoBack() {
            popup(-1);

            $("#serviceDecider").dialog("close");

            $("#serviceDecider").dialog({
                autoOpen: true,
                modal: true,
                width: 600,
                closeOnEscape: true,
                resizable: false,
                title: "Food Dispatch Possible Path",
                show: 'slide'
            });
        }

        function OnLoadFoodDispatchPathSucceeded(result) {
            $('#serviceDeciderHtml').html(result);

            $("#serviceDecider").dialog({
                autoOpen: true,
                modal: true,
                width: 600,
                closeOnEscape: true,
                resizable: false,
                title: pageTitle,
                show: 'slide'
            });
        }

        function OnLoadFoodDispatchPathFailed(error) {
        }

        function DispatchFoodByKotId(kotId) {
            popup(-1);
            var yes = confirm('Do you want to dispatch?');
            if (yes) {
                PageMethods.DispatchFoodByKotId(kotId, OnDispatchFoodSucceeded, OnDispatchFoodFailed);
            }
        }

        function OnDispatchFoodSucceeded(data) {
            toastr.info('Food Dispatched Successfully.');
            window.location = "frmFoodDispatch.aspx";
        }

        function OnDispatchFoodFailed(data) {
            CommonHelper.AlertMessage(data.AlertMessage);
        }

        function GetKotBillDetailListByKotId(kotId) {
            PageMethods.GetKotBillDetailListByKotId(kotId, OnGetKotBillDetailListByKotIdSucceeded, OnGetKotBillDetailListByKotIdFailed);
            $("#<%=hfKotId.ClientID %>").val(kotId);
        }

        function OnGetKotBillDetailListByKotIdSucceeded(result) {
            popup(-1);
            $('#GuestList').html(result);
            popup(1, 'GuestListingPopUp', '', 900, 300);
            $("#popUpDiv").css('height', '657px');
            return false;
        }

        function OnGetKotBillDetailListByKotIdFailed() {
        }

        function DispatchFoodByKotDetailId(kotDetailId) {
            var yes = confirm('Do you want to dispatch?');
            if (yes) {
                PageMethods.DispatchFoodByKotDetailId('Dispatch', kotDetailId, OnDispatchFoodByKotDetailIdSucceeded, OnDispatchFoodByKotDetailIdFailed);
            }
        }

        function OnDispatchFoodByKotDetailIdSucceeded(result) {
            toastr.info('Food Dispatched Successfully');
            var kotId = $("#<%=hfKotId.ClientID %>").val();
            GetKotBillDetailListByKotId(kotId);
        }

        function OnDispatchFoodByKotDetailIdFailed(data) {
            CommonHelper.AlertMessage(data.AlertMessage);
        }

        function UnDispatchFoodByKotDetailId(kotDetailId) {
            var yes = confirm('Do you want to undispatch?');
            if (yes) {
                PageMethods.DispatchFoodByKotDetailId('UnDispatch', kotDetailId, OnUnDispatchFoodByKotDetailIdSucceeded, OnUnDispatchFoodByKotDetailIdFailed);
            }
        }

        function OnUnDispatchFoodByKotDetailIdSucceeded(data) {
            toastr.info('Food UnDispatched Successfully');
            var kotId = $("#<%=hfKotId.ClientID %>").val();
            GetKotBillDetailListByKotId(kotId);
        }

        function OnUnDispatchFoodByKotDetailIdFailed(data) {
            CommonHelper.AlertMessage(data.AlertMessage);
        }        
    </script>
    <div>
        <div class="btn-toolbar;" style="text-align: right;">
            <asp:HiddenField ID="hfCostCenterId" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hfKotId" runat="server"></asp:HiddenField>
        </div>
    </div>
    <div id="SearchPanel" class="block">
        <div class="block-body collapse in">
            <asp:Literal ID="ltlFoodDispatchTemplate" runat="server">
            </asp:Literal>
        </div>
    </div>
    <div id="serviceDecider" style="display: none;">
        <div id="serviceDeciderHtml">
        </div>
    </div>
    <div id="GuestListingPopUp" style="display: none;">
        <div id="GuestList">
        </div>
    </div>
</asp:Content>
