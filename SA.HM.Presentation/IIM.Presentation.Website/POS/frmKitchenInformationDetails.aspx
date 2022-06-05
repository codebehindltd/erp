<%@ Page Title="" Language="C#" MasterPageFile="~/POS/RestaurantMM.Master" AutoEventWireup="true" CodeBehind="frmKitchenInformationDetails.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmKitchenInformationDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var KotInformationTable, KotInformationDetailsTable, SpecialInstructionsTable, RecipeDetailsTable;

        var UserInfoFromDB = null;
        //Page Refresh when idle for 10 seconds
        var time = new Date().getTime();
        $(document.body).bind("mousemove keypress", function (e) {
            time = new Date().getTime();
        });
        function refresh() {
            if (new Date().getTime() - time >= 30000)
                window.location.reload(true);
            else
                setTimeout(refresh, 30000);
        }
        setTimeout(refresh, 30000);

        //ended Page Refresh when idle for 10 seconds

        $(document).ready(function () {

            var itsPostBack = <%= Page.IsPostBack ? "true" : "false" %>;

            if (!itsPostBack) {
                //debugger;
                var previousAddress =  "/POS/frmCostCenterSelectionForAll.aspx";
                //document.referrer + "";
                $('#ContentPlaceHolder1_hfRefPreviousPage').val(previousAddress);
                var str = "<input type='button' id='btnGoBack' class='TransactionalButton btn btn-primary' value='Go Back' onclick=\"javascript:return GoBack('" + previousAddress + "')\" />";


                $('#DivGoBack').html(str);
            }
            else {
                //debugger;
                var previousAddress = $('#ContentPlaceHolder1_hfRefPreviousPage').val();
                var str = "<input type='button' id='btnGoBack' class='TransactionalButton btn btn-primary' value='Go Back' onclick=\"javascript:return GoBack('" + previousAddress + "')\" />";


                $('#DivGoBack').html(str);
            }

            //debugger;
            //if ($("#ContentPlaceHolder1_hfOrderType").val() == "Table") {
            //    debugger;
            //    LoadTableInfo($("#ContentPlaceHolder1_hfOrderCostcenterId").val());
            //}
            if ($("#hfUserInfoObj").val() !== "") {
                UserInfoFromDB = JSON.parse($("#hfUserInfoObj").val());
                $("#hfUserInfoObj").val("");
            }





            RecipeDetailsTable = $("#tblRecipeDetails").DataTable({
                data: [],
                columns: [
                    { title: "", "data": "ItemId", visible: false },
                    { title: "", "data": "RecipeItemId", visible: false },
                    { title: "", "data": "ItemCost", visible: false },
                    { title: "Recipe Item Name", "data": "RecipeItemName", sWidth: '50%' },
                    { title: "Unit", "data": "ItemUnit", sWidth: '25%' },
                    { title: "Head Name", "data": "HeadName", sWidth: '25%' }
                ],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var row = '';
                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }

                },
                pageLength: 10000,
                filter: false,
                info: false,
                ordering: false,
                processing: true,
                retrieve: true,
                bAutoWidth: false,
                bLengthChange: false,
                bInfo: false,
                bPaginate: false,
                language: {
                    emptyTable: "No Data Found"
                },

            });

            SpecialInstructionsTable = $("#tblSpecialInstructions").DataTable({
                data: [],
                columns: [
                    { title: "", "data": "RemarksDetailId", visible: false },
                    { title: "", "data": "SpecialRemarksId", visible: false },
                    { title: "Special Remarks", "data": "SpecialRemarks", sWidth: '100%' }
                ],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var row = '';
                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }

                },
                pageLength: 10000,
                filter: false,
                info: false,
                ordering: false,
                processing: true,
                retrieve: true,
                bAutoWidth: false,
                bLengthChange: false,
                bInfo: false,
                bPaginate: false,
                language: {
                    emptyTable: "No Data Found"
                },

            });
            KotInformationDetailsTable = $("#tblKotInformationDetails").DataTable({
                data: [],
                columns: [
                    { title: "", "data": "ItemId", visible: false },
                    { title: "Item Name", "data": "Name", sWidth: '50%' },
                    { title: "Item Unit", "data": "ItemUnit", sWidth: '20%' },
                    { title: "Action", "data": null, sWidth: '30%' }
                ],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var row = '';
                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }

                    row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return viewItemSpecialRemarkAndRecipeDetails('" + aData.ItemId + "', '" + aData.Name + "');\"><img alt=\"Details\" src=\"../Images/detailsInfo.png\" title='Details' /></a>";
                    row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return completeDelivery('" + aData.ItemId + "');\"><img alt=\"Delivery\" src=\"../Images/restaurantDelivery.png\"  title='Delivery' /></a>";


                    $('td:eq(' + (nRow.children.length - 1) + ')', nRow).html(row);

                },
                pageLength: 10000,
                filter: false,
                info: false,
                ordering: false,
                processing: true,
                retrieve: true,
                bAutoWidth: false,
                bLengthChange: false,
                bInfo: false,
                bPaginate: false,
                language: {
                    emptyTable: "No Data Found"
                },

            });
            KotInformationTable = $("#tblKotInformation").DataTable({
                data: [],
                columns: [
                    { title: "Kot No", "data": "KotId", sWidth: '30%' },
                    { title: "Outlet Name", "data": "CostCenter", sWidth: '50%' },
                    { title: "Action", "data": null, sWidth: '20%' }
                ],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var row = '';
                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }
                    //debugger;
                    //$('td:eq(0)').val(CommonHelper.DateFormatDDMMMYYY(aData.CreatedDate));
                    //$('td:eq(0)').val(CommonHelper.DateTimeClientFormatWiseConversionForDisplay(aData.CreatedDate));
                    //debugger;
                    //$('td:eq(' + (0) + ')', nRow).html(CommonHelper.DateFromStringToDisplay(aData.CreatedDate, innBoarDateFormat));
                    //CreatedDate = aData.CreatedDate;
                    //if (IsCanEdit && aData.IsCanEdit) {
                    //    row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformEditOperation('" + aData.Id + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";

                    //}
                    //if (aData.IsCanDelete && IsCanDelete) {
                    //    row += "&nbsp;&nbsp;<a href='javascript:void();' onclick= \"DeleteCashRequisition('" + aData.Id + "');\"> <img alt='Delete' src='../Images/delete.png' title='Delete' /></a>";
                    //}
                    //if (aData.ApprovedStatus == "Pending" && aData.IsCanCheck) {
                    //    row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return Approval('" + aData.Id + "','" + aData.ApprovedStatus + "','" + aData.Amount + "');\"><img alt=\"Check\" src=\"../Images/checked.png\" title='Check' /></a>";
                    //}
                    //else if (aData.ApprovedStatus == "Checked" && aData.IsCanApprove) {
                    //    row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return Approval('" + aData.Id + "','" + aData.ApprovedStatus + "','" + aData.Amount + "');\"><img alt=\"Approve\" src=\"../Images/approved.png\" title='Approve' /></a>";

                    //}



                    row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return viewKotDetailsForKitchen('" + aData.KotId + "');\"><img alt=\"Details\" src=\"../Images/detailsInfo.png\" title='Details' /></a>";
                    if (aData.IsCanDeliver != 0) {
                        row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return completeDeliveryKOT('" + aData.KotId + "');\"><img alt=\"Delivery\" src=\"../Images/restaurantDelivery.png\" title='Delivery' /></a>";
                    }

                    $('td:eq(' + (nRow.children.length - 1) + ')', nRow).html(row);

                    //$('td', row).eq(1).append(CommonHelper.DateTimeClientFormatWiseConversionForDisplay(aData.CreatedDate));


                },
                pageLength: 10000,
                filter: false,
                info: false,
                ordering: false,
                processing: true,
                retrieve: true,
                bAutoWidth: false,
                bLengthChange: false,
                bInfo: false,
                bPaginate: false,
                language: {
                    emptyTable: "No Data Found"
                },

            });

            var KitichenId = +$.trim(CommonHelper.GetParameterByName("kid"));
            var Kitchen = $.trim(CommonHelper.GetParameterByName("k"));
            if (KitichenId != 0) {
                LoadKitchenInfo(KitichenId, Kitchen)
            }
        });

        function GoBack(previousAddress) {
            if (confirm("Do you want to go back?")) {
                
                window.location = previousAddress;

            }

        }

        function LoadKitchenInfo(KitchenId, Kitchen) {
            var pageNumber = 1;
            var IsCurrentOrPreviousPage = 1;
            var gridRecordsCount = KotInformationTable.data().length;
            $("#ContentPlaceHolder1_hfRestaurantKitchen").val(Kitchen);

            $("#ContentPlaceHolder1_hfRestaurantKitchenId").val(KitchenId);
            $("#ContentPlaceHolder1_lblKitchen").text(Kitchen);

            PageMethods.LoadKitchenInfo(KitchenId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadKitchenInfoSucceeded, OnLoadKitchenInfoFailed);
            return false;
        }

        function completeDelivery(itemId) {
            if (!confirm("Do you want to Deliver this item?")) { return false; }
            var kotid = $("#ContentPlaceHolder1_hfRestaurantKOTId").val();
            PageMethods.completeDelivery(itemId, kotid, OnCompleteDeliverySucceeded, OncompleteDeliveryFailed);
            return false;

        }

        function OnCompleteDeliverySucceeded(result) {
            debugger;
            if (result.IsSuccess) {
                if (result.IsSuccess) {
                    CommonHelper.AlertMessage((result.AlertMessage));
                    var kotId = +$("#ContentPlaceHolder1_hfRestaurantKOTId").val();
                    viewKotDetailsForKitchen(kotId);
                }
            }
        }

        function OncompleteDeliveryFailed(error) {
            toastr.error(error.get_message());
            return false;
        }


        function viewItemSpecialRemarkAndRecipeDetails(itemId, itemName) {
           // debugger;
            var pageNumber = 1;
            var IsCurrentOrPreviousPage = 1;
            var gridRecordsCount = SpecialInstructionsTable.data().length;
            var kotid = $("#ContentPlaceHolder1_hfRestaurantKOTId").val();
            $("#ContentPlaceHolder1_hfItemName").val(itemName);

            PageMethods.GetSpecialRemarksDetails(kotid, itemId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnGetSpecialRemarksDetailsSucceeded, OnGetSpecialRemarksDetailsFailed);

            PageMethods.LoadRecipeByKotIdAndItemId(itemId, kotid, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadRecipeByKotIdAndItemIdSucceeded, OnLoadRecipeByKotIdAndItemIdFailed);

            return false;

        }

        function OnLoadRecipeByKotIdAndItemIdSucceeded(result) {
            //debugger;
            $("#ContentPlaceHolder1_lblRecipeDetails").text("Special Recipe Instructions For : " + $("#ContentPlaceHolder1_hfItemName").val());

            RecipeDetailsTable.clear();
            RecipeDetailsTable.rows.add(result.GridData);
            RecipeDetailsTable.draw();

            $("#GridPagingReceipe ul").html("");
            $("#GridPagingReceipe ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingReceipe ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingReceipe ul").append(result.GridPageLinks.NextButton);
        }
        function OnLoadRecipeByKotIdAndItemIdFailed() { }

        function OnGetSpecialRemarksDetailsSucceeded(result) {
           // debugger;

            $("#ContentPlaceHolder1_lblSpecialInstructions").text("Special Instructions For : " + $("#ContentPlaceHolder1_hfItemName").val());
            SpecialInstructionsTable.clear();
            SpecialInstructionsTable.rows.add(result.GridData);
            SpecialInstructionsTable.draw();

            $("#GridPaging ul").html("");
            $("#GridPaging ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPaging ul").append(result.GridPageLinks.Pagination);
            $("#GridPaging ul").append(result.GridPageLinks.NextButton);
        }
        function OnGetSpecialRemarksDetailsFailed() { }

        function completeDeliveryKOT(kotid) {
            if (!confirm("Do you want to deliver this KOT")) { return false; }
            PageMethods.completeDelivery(0, kotid, OnCompleteDeliveryKOTSucceeded, OncompleteDeliveryFailed);
            return false;
        }
        function OnCompleteDeliveryKOTSucceeded(result) {
            debugger;
            if (result.IsSuccess) {
                if (result.IsSuccess) {
                    CommonHelper.AlertMessage(result.AlertMessage);
                    var kitchenId = +$("#ContentPlaceHolder1_hfRestaurantKitchenId").val();
                    var kitchenName = $("#ContentPlaceHolder1_lblKitchen").text();
                    LoadKitchenInfo(kitchenId, kitchenName);
                }
            }
        }


        function viewKotDetailsForKitchen(KotId) {
            //debugger;
            var pageNumber = 1;
            var IsCurrentOrPreviousPage = 1;
            var gridRecordsCount = KotInformationDetailsTable.data().length;
            var KitchenId = $("#ContentPlaceHolder1_hfRestaurantKitchenId").val();
            $("#ContentPlaceHolder1_hfRestaurantKOTId").val(KotId);
            PageMethods.LoadKotDetailsForKitchen(KitchenId, KotId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnKotDetailsForKitchenSucceeded, OnKotDetailsForKitchenFailed);
            return false;
        }
        function OnKotDetailsForKitchenSucceeded(result) {
            //debugger;
            SpecialInstructionsTable.clear();
            SpecialInstructionsTable.draw();
            $("#ContentPlaceHolder1_lblSpecialInstructions").text("Special Instructions");
            RecipeDetailsTable.clear();
            RecipeDetailsTable.draw();
            $("#ContentPlaceHolder1_lblRecipeDetails").text("Special Recipe Instructions");

            $("#ContentPlaceHolder1_lblKotInformationDetails").text("Kot Information Details : " + $("#ContentPlaceHolder1_hfRestaurantKOTId").val());
            KotInformationDetailsTable.clear();
            KotInformationDetailsTable.rows.add(result.GridData);
            KotInformationDetailsTable.draw();

            $("#GridPagingContainerDetails ul").html("");
            $("#GridPagingContainerDetails ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainerDetails ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainerDetails ul").append(result.GridPageLinks.NextButton);
        }
        function OnKotDetailsForKitchenFailed() { }

        function OnLoadKitchenInfoSucceeded(result) {

            KotInformationDetailsTable.clear();
            KotInformationDetailsTable.draw();
            $("#ContentPlaceHolder1_lblKotInformationDetails").text("Kot Information Details");
            SpecialInstructionsTable.clear();
            SpecialInstructionsTable.draw();
            $("#ContentPlaceHolder1_lblSpecialInstructions").text("Special Instructions");
            RecipeDetailsTable.clear();
            RecipeDetailsTable.draw();
            $("#ContentPlaceHolder1_lblRecipeDetails").text("Special Recipe Instructions");
            KotInformationTable.clear();
            KotInformationTable.rows.add(result.GridData);
            KotInformationTable.draw();

            $("#GridPagingContainer ul").html("");
            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

        }
        function OnLoadKitchenInfoFailed() { }

    </script>
    <asp:HiddenField ID="hfUserInfoObj" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField ID="hfRestaurantKOTId" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfItemName" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfRefPreviousPage" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfRestaurantKitchen" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfRestaurantKitchenId" runat="server" Value=""></asp:HiddenField>
    <div class="panel panel-default" >
        <%--<div class="panel-heading">
            <asp:Label ID="lblKitchen" runat="server" Text=""></asp:Label>
        </div>--%>
        <div class="panel-body">
            <div id="KitchenInfoDialog">

                <div class="col-md-4" style="height: 700px; ">
                    <div class="form-group">
                        <h4><b><asp:Label ID="lblKitchen" runat="server" Text=""></asp:Label></b></h4>
                        <hr />
                    </div>
                    
                    <div id="SearchPanel" class="panel panel-default" style="height: 625px; overflow-y: scroll;">
                        <div class="panel-heading">KOT Information</div>
                        <div class="panel-body">

                            <table id="tblKotInformation" class="table table-bordered table-condensed table-responsive">
                            </table>
                            <div class="childDivSection" style='display:none;'>
                                <div class="text-center" id="GridPagingContainer">
                                    <ul class="pagination">
                                    </ul>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="col-md-8">
                    <div class="panel panel-default" style="height: 225px; overflow-y: scroll;">
                        <div class="panel-heading">
                            <asp:Label ID="lblKotInformationDetails" runat="server" Text="Kot Information Details"></asp:Label>
                        </div>
                        <div class="panel-body">

                            <table id="tblKotInformationDetails" class="table table-bordered table-condensed table-responsive">
                            </table>
                            <div class="childDivSection" style='display:none;'>
                                <div class="text-center" id="GridPagingContainerDetails">
                                    <ul class="pagination">
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default" style="height: 225px; overflow-y: scroll;">
                        <div class="panel-heading">

                            <asp:Label ID="lblSpecialInstructions" runat="server" Text="Special Instructions"></asp:Label>
                        </div>
                        <div class="panel-body">
                            <table id="tblSpecialInstructions" class="table table-bordered table-condensed table-responsive">
                            </table>
                            <div class="childDivSection" style='display:none;'>
                                <div class="text-center" id="GridPaging">
                                    <ul class="pagination">
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default" style="height: 225px; overflow-y: scroll;">
                        <div class="panel-heading">

                            <asp:Label ID="lblRecipeDetails" runat="server" Text="Recipe Details"></asp:Label>
                        </div>
                        <div class="panel-body">
                            <table id="tblRecipeDetails" class="table table-bordered table-condensed table-responsive">
                            </table>
                            <div class="childDivSection" style='display:none;'>
                                <div class="text-center" id="GridPagingReceipe">
                                    <ul class="pagination">
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
            <div class="row" id="SubmitButtonDiv" style="padding-top: 10px;">
                <div class="form-group">
                    <div class="col-md-1">
                        <div id="DivGoBack">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
