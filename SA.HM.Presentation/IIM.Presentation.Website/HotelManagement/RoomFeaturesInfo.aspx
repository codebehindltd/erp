<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="RoomFeaturesInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.RoomFeaturesInfo" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }
            $("#pnlAllFeatures").hide();

            //$("#btnSaveFeatures").click(function () {
            //    toString.info("Hi");
            //});

            $("#ContentPlaceHolder1_btnSearch").click(function () {
                var roomNumb = $("#ContentPlaceHolder1_txtRoomNumber").val();


                if (roomNumb == "") {
                    toastr.warning("Please enter a valid room number");
                    return false;
                }
                else {
                    $('#ContentPlaceHolder1_lstSelectedFeat').find('option').remove();
                    $('#ContentPlaceHolder1_lstAllFeatures').find('option').remove();
                    GetRoomInfo();
                }
                return false;
            });

            //$("#btnsave").click(function () {
            //    var features = document.getelementbyid('contentplaceholder1_lstselectedfeat');
            //    var itemid = "";
            //    var saveitems = [];

            //    for (var i = 0; i < features.length; i++) {
            //        itemid += features.options[i].value;
            //        saveitems.push(itemid);
            //    }
            //    savefeaturesinfo(saveitems);
            //    toastr.info(saveitems.length);
            //});
            //$("#ContentPlaceHolder1_lstAllFeatures").blur(function () {
                
            //    $("#ContentPlaceHolder1_lstAllFeatures").find("option").attr("selected", false);
            //});

            $("#btnTransferRight").click(function () {
                var options = $("#ContentPlaceHolder1_lstAllFeatures option:selected");


                for (var i = 0; i < options.length; i++) {
                    $("#ContentPlaceHolder1_lstSelectedFeat").append($(options[i]));
                }
            });

            $("#btnTransferLeft").click(function () {
                var options = $("#ContentPlaceHolder1_lstSelectedFeat option:selected");

                //var optionToRemove = $("#ContentPlaceHolder1_lstSelectedFeat option:selected");

                //var allSelectedFeatures = $('#ContentPlaceHolder1_lstSelectedFeat option');

                //var allFeatures = $('#ContentPlaceHolder1_lstAllFeatures option');

                //var values = $.map(allFeatures, function (option) {
                //    return option.value;
                //});

                for (var i = 0; i < options.length; i++) {
                    $("#ContentPlaceHolder1_lstAllFeatures").append($(options[i]));
                }

                //for (var i = 0; i < values.length; i++) {
                //    if (optionToRemove.value == values[i].value) {

                //    }
                //}

            });
        });

        // for room search-------------------
        function GetRoomInfo() {
            var roomNumb = $("#ContentPlaceHolder1_txtRoomNumber").val();

            PageMethods.GetRoomInfoByRoomNumber(roomNumb, OnLoadSearchSucceed, OnLoadSearchFailed);
            return false;
        }
        function OnLoadSearchSucceed(result) {
            if (result.RoomNumber != null) {
                $("#<%=hfRoomId.ClientID%>").val(result.RoomId);
                $("#<%=txtRoomNumber.ClientID%>").val(result.RoomNumber);
                $("#<%=txtRoomtype.ClientID%>").val(result.RoomType);
                $("#<%=lblRoomFeatures.ClientID%>").text(result.RoomNumber + " Features");
                $("#pnlAllFeatures").show('slow');
                LoadRoomFeatures();
                LoadAlreadySavedRoomFt();
            }
            else {
                $("#pnlAllFeatures").hide();
                toastr.warning("No room found, Please enter a valid room number.");
            }
            return false;
        }
        function OnLoadSearchFailed(error) {
            toastr.error(error.get_message());
        }

        // get all features------------
        function LoadRoomFeatures() {
            var roomId = $("#ContentPlaceHolder1_hfRoomId").val();

            PageMethods.GetActiveRoomFeatures(roomId, OnLoadFtSucceed, OnLoadFtFailed);

        }
        function OnLoadFtSucceed(result) {
            var roomFtList = result;

            $.each(result, function (i, item) {
                $('#ContentPlaceHolder1_lstAllFeatures').append($('<option>', {
                    value: item.Id,
                    text: item.Features
                }));
            });
        }
        function OnLoadFtFailed(error) {
            toastr.error(error.get_message());
        }

        //get already saved room features-----------------
        function LoadAlreadySavedRoomFt() {
            var roomId = $("#ContentPlaceHolder1_hfRoomId").val();

            PageMethods.GetRoomFeaturesByRoomId(roomId, OnLoadFeaturesSucceed, OnLoadFeaturesFailed);
        }
        function OnLoadFeaturesSucceed(result) {
            $.each(result, function (i, item) {
                $('#ContentPlaceHolder1_lstSelectedFeat').append($('<option>', {
                    value: item.Id,
                    text: item.Features
                }));
            });
        }
        function OnLoadFeaturesFailed(error) {
            toastr.error(error.get_message());
        }

        //For Save data-------------
        function SaveFeaturesInfo() {

            var selectedOptions = $('#ContentPlaceHolder1_lstSelectedFeat option');
            var allFeatures = $('#ContentPlaceHolder1_lstAllFeatures option');
            var roomNumb = $("#ContentPlaceHolder1_txtRoomNumber").val();

            var selectedValues = $.map(selectedOptions, function (option) {
                return option.value;
            });

            //var allFeaturesValues = $.map(allFeatures, function (option) {
            //    return option.value;
            //});

            if (selectedValues.length == 0) {
                toastr.warning("Please add some features for the room first");
                return false;
            }
            //toastr.info(values.length);

            var roomId = $("#ContentPlaceHolder1_hfRoomId").val();

            PageMethods.SaveRoomFeaturesInfo(selectedValues, roomNumb, roomId, OnSaveRoomFeatureSucceed, OnSaveRoomFeatureFailed);

            return false;
        }
        function OnSaveRoomFeatureSucceed(result) {
            if (result.IsSuccess) {

                CommonHelper.AlertMessage(result.AlertMessage);
                performclearaction();

            }
            else {

                CommonHelper.AlertMessage(result.AlertMessage);
            }

        }
        function OnSaveRoomFeatureFailed() {

        }

        function performclearaction() {
            $("#form1")[0].reset();
            $("#ContentPlaceHolder1_lblRoomFeatures").text("Features");
            $('#ContentPlaceHolder1_lstSelectedFeat').find('option').remove();
            $('#ContentPlaceHolder1_lstAllFeatures').find('option').remove();
            //$("#pnlAllFeatures").hide();
        }

    </script>

    <div id="RoomSearch" class="panel panel-default">

        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:HiddenField ID="hfRoomId" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hfId" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblRoomNumber" runat="server" class="control-label required-field" Text="Room Number"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtRoomNumber" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" TabIndex="2" CssClass="TransactionalButton btn btn-primary btn-sm" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblRoomType" runat="server" class="control-label" Text="Room Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtRoomtype" runat="server" CssClass="form-control" TabIndex="3" disabled="disabled"></asp:TextBox>
                    </div>
                </div>
            </div>

        </div>

    </div>
    <div id="pnlAllFeatures" class="panel panel-default">
        <div class="panel-heading" style="font-size: small;">Feature Selection</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-4" style="padding-left: 50px; font-size: small;">
                        <asp:Label ID="lblAllFeatures" runat="server" class="control-label" Text="All Features of the Room"></asp:Label>
                    </div>

                    <div class="col-md-6" style="padding-left: 300px; font-size: small;">
                        <asp:Label ID="lblRoomFeatures" runat="server" class="control-label" Text="Selected Features"></asp:Label>
                    </div>
                </div>
                <div style="padding-bottom: 20px;">
                    <div class="row no-gutters">
                        <div class="col-sm-5">
                            <asp:ListBox ID="lstAllFeatures" runat="server" Width="100%"
                                Height="200px"></asp:ListBox>

                        </div>
                        <div class="col-sm-2">
                            <div style="padding-top: 100px;">
                                <input type="button" id="btnTransferRight" value=">>" style="width: 95px;" class="btn btn-primary" />
                            </div>
                            <div style="padding-top: 2px;">
                                <input type="button" id="btnTransferLeft" value="<<" style="width: 95px;" class="btn btn-primary" />
                            </div>
                        </div>
                        <div class="col-sm-5">
                            <asp:ListBox ID="lstSelectedFeat" runat="server" Width="100%"
                                Height="200px"></asp:ListBox>
                            <%--<div style="margin-top: 5px;">
                            <input type="button" id="btnRightBillSplitPrintPreview" value="Print Preview" class="btn btn-primary" />
                        </div>--%>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div style="margin-top: 5px; padding-left: 30px;" class="col-md-2">
                        <asp:Button ID="btnSaveFeatures" runat="server" TabIndex="4" Text="Save Features"
                            CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript:return SaveFeaturesInfo()" />
                        <%--<input type="button" id="btnSave" value="Save Features" class="btn btn-primary" onclick="return SaveFeaturesInfo()" />--%>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
