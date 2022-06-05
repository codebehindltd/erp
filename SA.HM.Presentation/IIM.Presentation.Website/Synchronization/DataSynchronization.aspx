<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="DataSynchronization.aspx.cs" Inherits="HotelManagement.Presentation.Website.Synchronization.DataSynchronization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var baseUrl = "";
        var registrationData = [];
        var registrationIdList = [];
        var newRegistrationList = [];
        var syncedRegistrationList = [];
        var newAddedRestaurantIdList = [];
        var newAddedServiceBillIdList = [];
        var newAddedBanquetBillIdList = [];
        var temporarySyncData = [];

        $(document).ready(function () {

            $("#myTabs").tabs({ disabled: [1, 2, 3, 4] });
            $("#myTabs").tabs({ active: [0] });
            $("#myTabs").tabs();

            baseUrl = "http://" + $("#ContentPlaceHolder1_hfSyncApiUrl").val();
            LogIn();
            LoadLastSyncDateTime();
            //setInterval(function(){alert("Hello")},3000);

            $.ajaxSetup({
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Accept", "application/vvv.website+json;version=1");
                    xhr.setRequestHeader("Authorization", "Bearer " + localStorage.getItem('token'));
                }

            });

            $("#ContentPlaceHolder1_txtToDate").datepicker({
                dateFormat: innBoarDateFormat,
                defaultDate: DayOpenDate,
                setDate: DayOpenDate,
                changeMonth: true,
                changeYear: true,
                minDate: $("#ContentPlaceHolder1_lblLastSyncDateTime").text(),
                maxDate: DayOpenDate
                //onChange: function () { CheckSyncTime(); $("#tbRegistrationInformation tbody tr").remove(); ClearAmount(); }
            }).datepicker('setDate', DayOpenDate);

            $("#ContentPlaceHolder1_txtToDate").change(function () {

                CheckSyncTime();
                $("#tbRegistrationInformation tbody tr").remove();
                ClearAmount();
            });


            $("[id=chkAll]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#tbRegistrationInformation tbody tr").find("td:eq(0)").find("input:enabled").prop("checked", true);
                }
                else {
                    $("#tbRegistrationInformation tbody tr").find("td:eq(0)").find("input:enabled").prop("checked", false);
                }
                UpdateRegistrationGrandTotal();
            });

            $("[id=chkAllr]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#tbRestaurantInformation tbody tr").find("td:eq(0)").find("input:enabled").prop("checked", true);
                }
                else {
                    $("#tbRestaurantInformation tbody tr").find("td:eq(0)").find("input:enabled").prop("checked", false);
                }
                UpdateRestaurantGrandTotal();
            });

            $("[id=chkAllService]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#tbServiceBillInformation tbody tr").find("td:eq(0)").find("input:enabled").prop("checked", true);
                }
                else {
                    $("#tbServiceBillInformation tbody tr").find("td:eq(0)").find("input:enabled").prop("checked", false);
                }
                UpdateServiceGrandTotal();
            });

            $("[id=chkAllBanquet]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#tbBanquetBillInformation tbody tr").find("td:eq(0)").find("input:enabled").prop("checked", true);
                }
                else {
                    $("#tbBanquetBillInformation tbody tr").find("td:eq(0)").find("input:enabled").prop("checked", false);
                }
                UpdateBanquetGrandTotal();
            });

        });

        function CheckSyncTime() {
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();

            var fromDate = $("#ContentPlaceHolder1_lblLastSyncDateTime").text();

            var dateDifference = CalculateDateDiff(fromDate, toDate);

            fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');
            toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            if (dateDifference < 0) {
                $('#ContentPlaceHolder1_txtToDate').datepicker('setDate', DayOpenDate);
                toastr.warning("Cannot select previous Date of last sync Date");
            }
            else {
                if (Date.parse(toDate) > Date.parse(CommonHelper.DateFormatToMMDDYYYY(DayOpenDate, '/'))) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker('setDate', DayOpenDate);
                    toastr.warning("Cannot select future date & time");
                }
            }
        }
        function LoadLastSyncDateTime() {
            $.ajax({
                type: "POST",
                url: "./DataSynchronization.aspx/LoadLastSyncDateTime",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    var date = CommonHelper.DateFormatToMMDDYYYY(result.d, '/');
                    $("#ContentPlaceHolder1_lblLastSyncDateTime").text(date);
                },
                error: function (result) {
                }
            });

        }

        function SearchRegistration() {
            ProcessRoomRate();
            ClearAmount();
            newRegistrationList = [];
            newAddedRestaurantIdList = [];
            newAddedServiceBillIdList = [];
            newAddedBanquetBillIdList = [];
            temporarySyncData = [];
            LoadLastSyncDateTime();
            LoadTemporarySyncData();

            $.when(LoadRegistrationDataforSync()).then(function (response) {

                registrationData = response.d;
                registrationIdList = response.d.map(l => l.RegistrationId);

                $.each(response.d, function (i, item) {
                    if (item.GuiId != null)
                        syncedRegistrationList.push(item);
                });
                LoadRegistrationTable(response.d);

            });
            $("#myTabs").tabs({ disabled: [1, 2, 3, 4] });
            $("#myTabs").tabs({ active: [0] });
            $("#ContentPlaceHolder1_lblLastSyncDateTime").attr('disabled', false);
        }

        function LoadTemporarySyncData() {
            $.ajax({
                type: "POST",
                url: "./DataSynchronization.aspx/GetTemporarySyncData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    temporarySyncData = result.d;
                    newRegistrationList = temporarySyncData.filter(data => data.BillType == "Registration").map(l => l.BillId);
                    newAddedRestaurantIdList = temporarySyncData.filter(data => data.BillType == "Restaurant").map(l => l.BillId);
                    newAddedServiceBillIdList = temporarySyncData.filter(data => data.BillType == "Service").map(l => l.BillId);
                    newAddedBanquetBillIdList = temporarySyncData.filter(data => data.BillType == "Banquet").map(l => l.BillId);
                },
                error: function (result) {
                    toastr.error("Contact with admin.");
                }
            });
        }

        function LogIn() {

            var pass = prompt("Please enter Password:");
            var obj = {
                UserPassword: pass
            }

            if (pass == null || pass == "")
                window.location.href = "/HMCommon/frmInnboardDashboard.aspx";
            else {
                if (baseUrl == "") {
                    toastr.warning("Api url not found");
                    setTimeout(function () { window.location.href = "/HMCommon/frmInnboardDashboard.aspx"; }, 1000);

                }
                else {
                    $.when(LoginToServer(obj)).done(function (response) {
                        localStorage.setItem('token', response.access_token);
                        //ProcessRoomRate();
                        //SearchRegistration();
                    });
                }

            }

        }

        function ProcessRoomRate() {
            $.ajax({
                type: "POST",
                url: "./DataSynchronization.aspx/ProcessRoomRate",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    if (result.d)
                        toastr.success("Room rate process successful");
                    else
                        toastr.error("Room rate process unsuccessful");
                },
                error: function (result) {
                    toastr.error("Room rate process successful");
                }
            });
        }

        function LoginToServer(obj) {

            return $.ajax({
                type: 'POST',
                url: baseUrl + '/Account/Login',
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    toastr.success("Log in Successfull");

                },
                error: function (error) {
                    if (error.responseJSON != undefined)
                        toastr.warning(error.responseJSON.error_description);
                    else
                        toastr.warning("The password is incorrect.");
                    setTimeout(function () {
                        window.location.href = "/HMCommon/frmInnboardDashboard.aspx";
                    }, 1000);

                }
            });
        }

        function LoadRegistrationDataforSync() {

            var toDate = $("#ContentPlaceHolder1_txtToDate").val();
            toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            var param = JSON.stringify({ toDate: toDate });

            return $.ajax({
                type: 'POST',
                url: "/Synchronization/DataSynchronization.aspx/GetRegistrationListToSync",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: param,
                success: function (data) {

                    //LoadDataforSync();
                },
                error: function (request, error) {
                    //alert("Request: " + JSON.stringify(request));
                    toastr.warning(request.responseJSON.Message);
                    $("#ContentPlaceHolder1_txtToDate").val(DayOpenDate);
                    return false;
                }

            });
        }

        function LoadRegistrationTable(data) {
            $("#tbRegistrationInformation tbody tr").remove();

            var tr = "", totalRow = 0;
            var isExist = false, isTransferedRoomSyncing = false;
            //var editPermission = true, deletePermission = true, approvalPermission = true;

            if (data == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"9\" >No Data Found</td> </tr>";
                $("#tbRegistrationInformation tbody ").append(emptyTr);
                return false;
            }
            var isRatePlusPlus = 1;

            var vat = parseFloat($("#ContentPlaceHolder1_hfGuestHouseVat").val());
            var serviceCharge = parseFloat($("#ContentPlaceHolder1_hfGuestHouseServiceCharge").val());

            if ($("#ContentPlaceHolder1_hfIsRatePlusPlus").val() != "")
                isRatePlusPlus = parseInt($("#ContentPlaceHolder1_hfIsRatePlusPlus").val(), 10);

            var additionalChargeType = $("#ContentPlaceHolder1_hfAdditionalChargeType").val();
            var inclusiveBill = parseInt($("#ContentPlaceHolder1_hfInclusiveHotelManagementBill").val(), 10);
            var isVatEnableOnGuestHouseCityCharge = parseInt($("#ContentPlaceHolder1_hfIsVatEnableOnGuestHouseCityCharge").val(), 10);
            var cityCharge = parseFloat($("#ContentPlaceHolder1_hfCityCharge").val());
            var additionalCharge = parseFloat($("#ContentPlaceHolder1_hfAdditionalCharge").val());

            var isVatAmountEnable = 1, isServiceChargeEnable = 1, isCityChargeEnable = 1, isAdditionalChargeEnable = 1;
            var nights;

            var toDate = $("#ContentPlaceHolder1_txtToDate").val();


            var fromDate = $("#ContentPlaceHolder1_lblLastSyncDateTime").text();

            var rowFromDateTime, rowToDateTime;

            $.each(data, function (count, gridObject) {
                totalRow = $("#tbRegistrationInformation tbody tr").length;

                isExist = newRegistrationList.some(data => data == gridObject.RegistrationId);

                isTransferedRoomSyncing = registrationData.some(data => (data.BillPaidBy == gridObject.RegistrationId || data.RegistrationId == gridObject.BillPaidBy) && data.GuidId != null);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                if (gridObject.GuidId == null && isExist && !isTransferedRoomSyncing)
                    tr += "<td align='left' style=\"width:5%;\">" + "<input onclick=\"javascript:return CheckTransferedRoomNUpdateUpdateRegistrationGrandTotal(this)\" checked= 'checked' type='checkbox'/>" + "</td>";
                else if (gridObject.GuidId != null || isTransferedRoomSyncing)
                    tr += "<td align='left' style=\"width:5%;\">" + "<input onclick=\"javascript:return CheckTransferedRoomNUpdateUpdateRegistrationGrandTotal(this)\" checked= 'checked' disabled='disabled' type='checkbox'/>" + "</td>";
                else
                    tr += "<td align='left' style=\"width:5%;\">" + "<input onclick=\"javascript:return CheckTransferedRoomNUpdateUpdateRegistrationGrandTotal(this)\" type='checkbox'/>" + "</td>";

                tr += "<td align='left' style=\"width:15%;\">" + gridObject.RegistrationNumber + '<br>Room Number#' + gridObject.RoomNumber + "</td>";
                tr += "<td align='left' style=\"width:20%;\">" + gridObject.GuestName + "</td>";
                tr += "<td align='left' style=\"width:10%\">" + gridObject.DisplayArriveDate + "</td>";
                tr += "<td align='left' style=\"width:15%\">" + gridObject.DisplayCheckOut + "</td>";
                tr += "<td align='left' style=\"width:10%\">" + gridObject.RoomRate + "</td>";

                tr += "<td align='left' style=\"display:none\">" + gridObject.RegistrationId + "</td>";
                tr += "<td align='left' style=\"display:none\">" + gridObject.GuidId + "</td>";
                tr += "<td align='left' style=\"display:none\">" + gridObject.IsSyncCompleted + "</td>";

                isVatAmountEnable = gridObject.IsVatAmountEnable ? 1 : 0;
                isServiceChargeEnable = gridObject.IsServiceChargeEnable ? 1 : 0;
                isCityChargeEnable = gridObject.IsCityChargeEnable ? 1 : 0;
                isAdditionalChargeEnable = gridObject.IsAdditionalChargeEnable ? 1 : 0;

                vatAmount = gridObject.IsVatAmountEnable ? vat : 0;
                serviceChargeAmount = gridObject.IsServiceChargeEnable ? serviceCharge : 0;
                cityChargeAmount = gridObject.IsCityChargeEnable ? cityCharge : 0;
                additionalChargeAmount = gridObject.IsAdditionalChargeEnable ? additionalCharge : 0;


                var InnboardRateCalculationInfo = CommonHelper.GetRackRateServiceChargeVatInformation(gridObject.RoomRate, serviceChargeAmount, cityChargeAmount,
                    vatAmount, additionalChargeAmount, additionalChargeType, inclusiveBill, isRatePlusPlus, isVatEnableOnGuestHouseCityCharge,
                    isVatAmountEnable, isServiceChargeEnable, isCityChargeEnable, isAdditionalChargeEnable, 0, "Fixed", 0);


                tr += "<td align='left' style=\"display:none\">" + (gridObject.Nights * InnboardRateCalculationInfo.VatAmount).toFixed(2) + "</td>";

                tr += "<td align='left' style=\"width:10%\">" + gridObject.Nights + "</td>";
                tr += "<td align='left' style=\"width:15%\">" + (gridObject.Nights * gridObject.RoomRate).toFixed(2) + "</td>";
                //tr += "<td align='left' style=\"width:10%\">" + nights + "</td>";

                tr += "<td align='center' style=\"width:10%;\"> ";
                tr += "<img src=\"../Images/ReportDocument.png\" title=\"Invoice Preview\" alt=\"Invoice Preview\" onclick=\"javascript: return ViewInvoice('RoomRegistration', " + gridObject.RegistrationId + ")\" style=\"cursor: pointer; \"/>";

                tr += "</td>";
                tr += "<td align='left' style=\"display:none\">" + gridObject.BillPaidBy + "</td>";
                tr += "</tr>";

                $("#tbRegistrationInformation tbody").append(tr);

                tr = "";


            });
            CheckTransferedRoomWithDetails();
            UpdateRegistrationGrandTotal();

        }

        function SaveRegistrationIdTemporary() {
            newRegistrationList = new Array();

            $("#tbRegistrationInformation tbody tr").each(function (index, item) {

                var registrationId = 0, guidId = null, IsSyncCompleted;

                if ($(this).find("td:eq(0)").find("input").is(':checked')) {

                    registrationId = parseInt($.trim($(item).find("td:eq(6)").text()));

                    guidId = $.trim($(item).find("td:eq(7)").text());
                    IsSyncCompleted = ($.trim($(item).find("td:eq(8)").text()) == 'true');

                    if (!IsSyncCompleted)
                        newRegistrationList.push(registrationId);

                }
            });
        }

        function SaveServiceBillIdTemporary() {
            newAddedServiceBillIdList = new Array();

            $("#tbServiceBillInformation tbody tr").each(function (index, item) {

                var billId = 0, IsSyncCompleted;

                if ($(this).find("td:eq(0)").find("input").is(':checked')) {

                    billId = parseInt($.trim($(item).find("td:eq(6)").text()));
                    IsSyncCompleted = ($.trim($(item).find("td:eq(9)").text()) == 'true');

                    if (!IsSyncCompleted)
                        newAddedServiceBillIdList.push(billId);

                }
            });

        }

        function SaveRestaurantIdTemporary() {
            newAddedRestaurantIdList = new Array();
            $("#tbRestaurantInformation tbody tr").each(function (index, item) {

                var billId = 0, IsSyncCompleted;

                if ($(this).find("td:eq(0)").find("input").is(':checked')) {

                    billId = parseInt($.trim($(item).find("td:eq(6)").text()));
                    IsSyncCompleted = ($.trim($(item).find("td:eq(9)").text()) == 'true');

                    if (!IsSyncCompleted)
                        newAddedRestaurantIdList.push(billId);

                }
            });
        }

        function SaveBanquetIdTemporary() {
            newAddedBanquetBillIdList = new Array();
            $("#tbBanquetBillInformation tbody tr").each(function (index, item) {

                var billId = 0, IsSyncCompleted;

                if ($(this).find("td:eq(0)").find("input").is(':checked')) {

                    billId = parseInt($.trim($(item).find("td:eq(6)").text()));
                    IsSyncCompleted = ($.trim($(item).find("td:eq(9)").text()) == 'true');

                    if (!IsSyncCompleted)
                        newAddedBanquetBillIdList.push(billId);

                }
            });
        }

        function SyncData() {
            SyncRegistrationData();
            SyncServiceBillData();
            SyncRestaurantData();
            SyncBanquetData();
            if (newRegistrationList.length > 0 || newAddedRestaurantIdList.length > 0 || newAddedServiceBillIdList.length > 0 ||
                newAddedBanquetBillIdList.length > 0)
                UpdateSyncDateTime();
            SearchRegistration();

        }

        function SyncLater(type) {
            var billList = new Array();
            if (type == "Registration")
                SaveRegistrationIdTemporary();
            if (type == "Service")
                SaveServiceBillIdTemporary();
            if (type == "Restaurant")
                SaveRestaurantIdTemporary();
            if (type == "Banquet")
                SaveBanquetIdTemporary();

            if (newRegistrationList.length > 0) {
                newRegistrationList.forEach((reg) => {
                    billList.push({
                        BillId: reg,
                        BillType: "Registration"
                    });
                });
            }
            if (newAddedRestaurantIdList.length > 0) {
                newAddedRestaurantIdList.forEach((reg) => {
                    billList.push({
                        BillId: reg,
                        BillType: "Restaurant"
                    });
                });
            }
            if (newAddedServiceBillIdList.length > 0) {
                newAddedServiceBillIdList.forEach((reg) => {
                    billList.push({
                        BillId: reg,
                        BillType: "Service"
                    });
                });
            }
            if (newAddedBanquetBillIdList.length > 0) {
                newAddedBanquetBillIdList.forEach((reg) => {
                    billList.push({
                        BillId: reg,
                        BillType: "Banquet"
                    });
                });
            }
            if (billList.length > 0) {
                $.ajax({
                    type: "POST",
                    url: "./DataSynchronization.aspx/SaveSyncDataForSyncLater",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ billList: billList }),
                    success: function (result) {
                        CommonHelper.AlertMessage(result.d.AlertMessage);
                    },
                    error: function (result) {
                        CommonHelper.AlertMessage(result.d.AlertMessage);
                    }
                });
            }
            SearchRegistration();
        }

        function SyncRegistrationData() {

            var registration;
            if (newRegistrationList.length > 0) {
                for (var i = (newRegistrationList.length - 1); i >= 0; i--) {
                    registration = _.findWhere(registrationData, { RegistrationId: newRegistrationList[i] });

                    $.when(GetRegistrationRelatedDataToSync(newRegistrationList[i])).done(function (response) {
                        //response.d.RoomRegistration.

                        if (response.d != null) {

                            $.when(SendRegistrationDataToAPI(response.d))
                                .done(function (response) {
                                    if (response.Success) {
                                        SaveRegistrationSyncInformation(response.GuidId, response.IsSyncCompleted);

                                    }
                                    else
                                        toastr.info("Sync Operation failed for: " + registration.RegistrationNumber);
                                });
                        }
                        else {
                            toastr.info("No new Data found to Sync for: " + registration.RegistrationNumber);
                        }
                    });
                }
            }
        }

        function SyncServiceBillData() {

            if (newAddedServiceBillIdList.length > 0) {
                for (var i = (newAddedServiceBillIdList.length - 1); i >= 0; i--) {

                    $.when(GetServiceBillRelatedDataToSync(newAddedServiceBillIdList[i]))
                        .done(function (response) {

                            if (response.d != null) {
                                $.when(SendServiceBillDataToAPI(response.d))
                                    .done(function (responseFromApi) {
                                        if (responseFromApi.Success) {
                                            SaveServiceBillSyncInformation(responseFromApi.GuidId, responseFromApi.IsSyncCompleted);

                                        }

                                    });
                            }
                        });
                }
            }
        }

        function SyncRestaurantData() {
            if (newAddedRestaurantIdList.length > 0) {
                for (var i = (newAddedRestaurantIdList.length - 1); i >= 0; i--) {

                    $.when(GetRestaurantRelatedDataToSync(newAddedRestaurantIdList[i]))
                        .done(function (response) {

                            if (response.d != null) {
                                $.when(SendRestaurantDataToAPI(response.d))
                                    .done(function (responseFromApi) {
                                        if (responseFromApi.Success) {
                                            SaveRestaurantSyncInformation(responseFromApi.GuidId, responseFromApi.IsSyncCompleted);

                                        }

                                    });
                            }
                        });
                }
            }
        }

        function SyncBanquetData() {
            if (newAddedBanquetBillIdList.length > 0) {
                for (var i = (newAddedBanquetBillIdList.length - 1); i >= 0; i--) {

                    $.when(GetBanquetRelatedDataToSync(newAddedBanquetBillIdList[i]))
                        .done(function (response) {

                            if (response.d != null) {
                                $.when(SendBanquetDataToAPI(response.d))
                                    .done(function (responseFromApi) {
                                        if (responseFromApi.Success) {
                                            SaveBanquetSyncInformation(responseFromApi.GuidId, responseFromApi.IsSyncCompleted);

                                        }
                                    });
                            }
                        });

                }
            }
        }

        function GetRegistrationRelatedDataToSync(registrationId) {
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();
            toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            var param = JSON.stringify({ toDate: toDate });

            return $.ajax({
                type: 'POST',
                url: "/Synchronization/DataSynchronization.aspx/GetRegistrationRelatedDataToSync",
                data: JSON.stringify({ registrationId: registrationId, toDate: toDate }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {

                },
                error: function (request, error) {
                    alert("Request: " + JSON.stringify(request));
                    return false;
                }

            });

        }

        function GetServiceBillRelatedDataToSync(serviceBillId) {

            return $.ajax({
                type: 'POST',
                url: "/Synchronization/DataSynchronization.aspx/GetServiceBillRelatedDataToSync",
                data: JSON.stringify({ serviceBillId: serviceBillId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {

                },
                error: function (request, error) {
                    alert("Request: " + JSON.stringify(request));
                    return false;
                }

            });
        }

        function GetRestaurantRelatedDataToSync(billId) {
            return $.ajax({
                type: 'POST',
                url: "/Synchronization/DataSynchronization.aspx/GetRestaurantRelatedDataToSync",
                data: JSON.stringify({ billId: billId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {

                },
                error: function (request, error) {
                    alert("Request: " + JSON.stringify(request));
                    return false;
                }

            });
        }

        function GetBanquetRelatedDataToSync(reservationId) {
            return $.ajax({
                type: 'POST',
                url: "/Synchronization/DataSynchronization.aspx/GetBanquetRelatedDataToSync",
                data: JSON.stringify({ id: reservationId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {

                },
                error: function (request, error) {
                    alert("Request: " + JSON.stringify(request));
                    return false;
                }

            });
        }

        function SendRegistrationDataToAPI(registration) {

            return $.ajax({
                type: 'POST',
                url: baseUrl + '/Sync',
                data: JSON.stringify(registration),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {

                },
                error: function () {
                    toastr.info('Contact With Admin');
                }

            });
        }

        function SendServiceBillDataToAPI(serviceBill) {

            return $.ajax({
                type: 'POST',
                url: baseUrl + '/Sync/SyncServiceBill',
                data: JSON.stringify(serviceBill),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {

                },
                error: function () {
                    toastr.info('Contact With Admin');
                }

            });
        }

        function SendRestaurantDataToAPI(restaurant) {

            return $.ajax({
                type: 'POST',
                url: baseUrl + '/Sync/SyncRestaurant',
                data: JSON.stringify(restaurant),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {

                },
                error: function () {
                    toastr.info('Contact With Admin');
                }

            });
        }

        function SendBanquetDataToAPI(banquet) {

            return $.ajax({
                type: 'POST',
                url: baseUrl + '/Sync/SyncBanquetBill',
                data: JSON.stringify(banquet),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {

                },
                error: function () {
                    toastr.info('Contact With Admin');
                }

            });
        }

        function SaveRegistrationSyncInformation(guidId, IsSyncCompleted) {

            var param = JSON.stringify({ id: guidId, IsSyncCompleted: IsSyncCompleted });
            return $.ajax({
                type: 'POST',
                url: "/Synchronization/DataSynchronization.aspx/SaveRegistrationSyncInformation",
                data: param,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    toastr.success("Registration Sync Successfull");
                    //LoadDataforSync();
                },
                error: function (request, error) {
                    alert("Request: " + JSON.stringify(request));
                    return false;
                }

            });
        }

        function SaveServiceBillSyncInformation(guidId, IsSyncCompleted) {

            var param = JSON.stringify({ id: guidId, IsSyncCompleted: IsSyncCompleted });
            return $.ajax({
                type: 'POST',
                url: "/Synchronization/DataSynchronization.aspx/SaveServiceBillSyncInformation",
                data: param,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    toastr.success("Service bill Sync Successfull");
                    //LoadDataforSync();
                },
                error: function (request, error) {
                    alert("Request: " + JSON.stringify(request));
                    return false;
                }

            });
        }

        function SaveRestaurantSyncInformation(guidId, IsSyncCompleted) {

            var param = JSON.stringify({ id: guidId, IsSyncCompleted: IsSyncCompleted });
            return $.ajax({
                type: 'POST',
                url: "/Synchronization/DataSynchronization.aspx/SaveRestaurantSyncInformation",
                data: param,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    toastr.success("Restuarant Sync Successfull");
                    //LoadDataforSync();
                },
                error: function (request, error) {
                    alert("Request: " + JSON.stringify(request));
                    return false;
                }

            });
        }

        function SaveBanquetSyncInformation(guidId, IsSyncCompleted) {

            var param = JSON.stringify({ id: guidId, IsSyncCompleted: IsSyncCompleted });
            return $.ajax({
                type: 'POST',
                url: "/Synchronization/DataSynchronization.aspx/SaveBanquetSyncInformation",
                data: param,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    toastr.success("Banquet Sync Successfull");
                    //LoadDataforSync();
                },
                error: function (request, error) {
                    alert("Request: " + JSON.stringify(request));
                    return false;
                }

            });
        }

        function NextTab() {
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();

            if (toDate == "") {
                toastr.info("Select To Date");
                $("#ContentPlaceHolder1_txtToDate").focus();
                return false;
            }


            $("#ContentPlaceHolder1_lblLastSyncDateTime").attr('disabled', true);
            $("#myTabs").tabs({ disabled: [0, 2, 3, 4] });
            $("#myTabs").tabs({ active: [1] });
            SaveRegistrationIdTemporary();
            LoadServiceBillForSync();
            return false;
        }

        function NextTab2() {
            $("#ContentPlaceHolder1_lblLastSyncDateTime").attr('disabled', true);
            LoadResturantBillsForSync();
            SaveServiceBillIdTemporary();
            $("#myTabs").tabs({ disabled: [0, 1, 3, 4] });
            $("#myTabs").tabs({ active: [2] });
        }

        function NextTab3() {
            $("#ContentPlaceHolder1_lblLastSyncDateTime").attr('disabled', true);
            $("#myTabs").tabs({ disabled: [0, 1, 2, 4] });
            $("#myTabs").tabs({ active: [3] });

            SaveRestaurantIdTemporary();
            LoadBanquetBillsForSync();
        }

        function NextTab4() {
            $("#ContentPlaceHolder1_lblLastSyncDateTime").attr('disabled', true);
            $("#myTabs").tabs({ disabled: [0, 1, 2, 3] });
            $("#myTabs").tabs({ active: [4] });

            SaveBanquetIdTemporary();
            UpdateFinalGrandTotal();
        }

        function LoadResturantBillsForSync() {
            $.when(GetRestaurantDataForSync()).then(function (response) {

                LaodRestaurantTable(response.d);

            });
        }

        function LoadServiceBillForSync() {
            $.when(GetServiceBillDataForSync()).then(function (response) {

                LaodServiceBillTable(response.d);

            });
        }

        function LoadBanquetBillsForSync() {
            $.when(GetBanquetDataForSync()).then(function (response) {

                LaodBanquetTable(response.d);

            });
        }

        function GetServiceBillDataForSync() {
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();
            toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            var param = JSON.stringify({ registrationIdList: registrationIdList, toDate: toDate });

            return $.ajax({
                type: 'POST',
                url: "/Synchronization/DataSynchronization.aspx/GetServiceBillListToSync",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: param,
                success: function (data) {

                },
                error: function (request, error) {
                    alert("Request: " + JSON.stringify(request));
                    return false;
                }

            });
        }

        function GetRestaurantDataForSync() {
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();
            toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            var param = JSON.stringify({ registrationIdList: registrationIdList, toDate: toDate });

            return $.ajax({
                type: 'POST',
                url: "/Synchronization/DataSynchronization.aspx/GetRestaurantListToSync",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: param,
                success: function (data) {

                    //LoadDataforSync();
                },
                error: function (request, error) {
                    alert("Request: " + JSON.stringify(request));
                    return false;
                }

            });
        }

        function GetBanquetDataForSync() {
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();
            toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            var param = JSON.stringify({ registrationIdList: registrationIdList, toDate: toDate });

            return $.ajax({
                type: 'POST',
                url: "/Synchronization/DataSynchronization.aspx/GetBanquetBillListToSync",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: param,
                success: function (data) {

                    //LoadDataforSync();
                },
                error: function (request, error) {
                    alert("Request: " + JSON.stringify(request));
                    return false;
                }

            });
        }

        function LaodServiceBillTable(data) {
            $("#tbServiceBillInformation tbody tr").remove();
            //$("#GridPagingContainer ul").html("");
            var tr = "", totalRow = 0;
            var isSynced, isNewlyAdded;
            var isExist = false;

            if (data == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"6\" >No Data Found</td> </tr>";
                $("#tbServiceBillInformation tbody ").append(emptyTr);
                return false;
            }

            $.each(data, function (count, gridObject) {
                isNewlyAdded = newRegistrationList.includes(gridObject.RegistrationId);
                isSynced = syncedRegistrationList.includes(gridObject.RegistrationId);
                totalRow = $("#tbServiceBillInformation tbody tr").length;
                isExist = newAddedServiceBillIdList.some(data => data == gridObject.BillId);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                if (gridObject.IsSyncCompleted)
                    tr += "<td align='left' style=\"width:5%;\">" + "<input onclick=\"javascript:return UpdateServiceGrandTotal()\" type='checkbox' checked= 'checked' disabled='disabled'/>" + "</td>";
                else if (gridObject.RegistrationId > 0) {
                    if (isNewlyAdded || isSynced)
                        tr += "<td align='left' style=\"width:5%;\">" + "<input onclick=\"javascript:return UpdateServiceGrandTotal()\" type='checkbox' checked= 'checked' disabled='disabled'/>" + "</td>";
                    else
                        tr += "<td align='left' style=\"width:5%;\">" + "<input onclick=\"javascript:return UpdateServiceGrandTotal()\" type='checkbox' disabled='disabled'/>" + "</td>";
                }
                else if (isExist)
                    tr += "<td align='left' style=\"width:5%;\">" + "<input onclick=\"javascript:return UpdateServiceGrandTotal()\" checked= 'checked' type='checkbox'/>" + "</td>";
                else
                    tr += "<td align='left' style=\"width:5%;\">" + "<input onclick=\"javascript:return UpdateServiceGrandTotal()\" type='checkbox'/>" + "</td>";

                tr += "<td align='left' style=\"width:20%;\">" + gridObject.BillNumber + "</td>";
                tr += "<td align='left' style=\"width:25%;\">" + gridObject.CostCenter + "</td>";
                tr += "<td align='left' style=\"width:15%;\">" + gridObject.BillAmount + "</td>";
                tr += "<td align='left' style=\"width:25%;\">" + gridObject.PaymentDescription + "</td>";

                tr += "<td align='center' style=\"width:10%;\"> ";
                tr += "<img src=\"../Images/ReportDocument.png\" title=\"Invoice Preview\" alt=\"Invoice Preview\" onclick=\"javascript: return ViewInvoice('GuestHouseService', " + gridObject.BillId + ")\" style=\"cursor: pointer; \"/>";
                tr += "</td>";

                tr += "<td align='left' style=\"display:none\">" + gridObject.BillId + "</td>";
                tr += "<td align='left' style=\"display:none\">" + gridObject.RegistrationId + "</td>";
                tr += "<td align='left' style=\"display:none\">" + gridObject.GuidId + "</td>";
                tr += "<td align='left' style=\"display:none\">" + gridObject.IsSyncCompleted + "</td>";
                tr += "<td align='left' style=\"display:none\">" + gridObject.VatAmount + "</td>";

                tr += "</tr>";

                $("#tbServiceBillInformation tbody").append(tr);
                tr = "";
            });
            UpdateServiceGrandTotal();
        }

        function LaodRestaurantTable(data) {

            $("#tbRestaurantInformation tbody tr").remove();
            $("#GridPagingContainer ul").html("");
            var tr = "", totalRow = 0;
            var isSynced, isNewlyAdded;
            var isExist = false;

            if (data == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"6\" >No Data Found</td> </tr>";
                $("#tbRestaurantInformation tbody ").append(emptyTr);
                return false;
            }

            $.each(data, function (count, gridObject) {
                isNewlyAdded = newRegistrationList.includes(gridObject.RegistrationId);
                isSynced = syncedRegistrationList.includes(gridObject.RegistrationId);
                totalRow = $("#tbRestaurantInformation tbody tr").length;
                isExist = newAddedRestaurantIdList.some(data => data == gridObject.BillId);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                if (gridObject.IsSyncCompleted)
                    tr += "<td align='left' style=\"width:5%;\">" + "<input onclick=\"javascript:return UpdateRestaurantGrandTotal()\" type='checkbox' checked= 'checked' disabled='disabled'/>" + "</td>";
                else if (gridObject.RegistrationId > 0) {
                    if (isNewlyAdded || isSynced)
                        tr += "<td align='left' style=\"width:5%;\">" + "<input onclick=\"javascript:return UpdateRestaurantGrandTotal()\" type='checkbox' checked= 'checked' disabled='disabled'/>" + "</td>";
                    else
                        tr += "<td align='left' style=\"width:5%;\">" + "<input onclick=\"javascript:return UpdateRestaurantGrandTotal()\" type='checkbox' disabled='disabled'/>" + "</td>";
                }
                else if (isExist)
                    tr += "<td align='left' style=\"width:5%;\">" + "<input onclick=\"javascript:return UpdateRestaurantGrandTotal()\" checked= 'checked' type='checkbox'/>" + "</td>";
                else
                    tr += "<td align='left' style=\"width:5%;\">" + "<input onclick=\"javascript:return UpdateRestaurantGrandTotal()\" type='checkbox'/>" + "</td>";

                tr += "<td align='left' style=\"width:20%;\">" + gridObject.BillNumber + "</td>";
                tr += "<td align='left' style=\"width:25%;\">" + gridObject.CostCenter + "</td>";
                tr += "<td align='left' style=\"width:20%;\">" + gridObject.BillAmount + "</td>";
                tr += "<td align='left' style=\"width:20%;\">" + gridObject.PaymentDescription + "</td>";

                tr += "<td align='center' style=\"width:10%;\"> ";
                tr += "<img src=\"../Images/ReportDocument.png\" title=\"Invoice Preview\" alt=\"Invoice Preview\" onclick=\"javascript: return ViewInvoice('RestaurantService', " + gridObject.BillId + ")\" style=\"cursor: pointer; \"/>";
                tr += "</td>";

                tr += "<td align='left' style=\"display:none\">" + gridObject.BillId + "</td>";
                tr += "<td align='left' style=\"display:none\">" + gridObject.RegistrationId + "</td>";
                tr += "<td align='left' style=\"display:none\">" + gridObject.GuidId + "</td>";
                tr += "<td align='left' style=\"display:none\">" + gridObject.IsSyncCompleted + "</td>";
                tr += "<td align='left' style=\"display:none\">" + gridObject.VatAmount + "</td>";

                tr += "</tr>";

                $("#tbRestaurantInformation tbody").append(tr);
                tr = "";
            });
            UpdateRestaurantGrandTotal();
        }

        function LaodBanquetTable(data) {
            $("#tbBanquetBillInformation tbody tr").remove();
            //$("#GridPagingContainer ul").html("");
            var tr = "", totalRow = 0;
            var isSynced, isNewlyAdded;
            var isExist = false;

            if (data == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"6\" >No Data Found</td> </tr>";
                $("#tbBanquetBillInformation tbody ").append(emptyTr);
                return false;
            }

            $.each(data, function (count, gridObject) {
                isNewlyAdded = newRegistrationList.includes(gridObject.RegistrationId);
                isSynced = syncedRegistrationList.includes(gridObject.RegistrationId);
                totalRow = $("#tbBanquetBillInformation tbody tr").length;
                isExist = newAddedBanquetBillIdList.some(data => data == gridObject.BillId);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                if (gridObject.IsSyncCompleted)
                    tr += "<td align='left' style=\"width:5%;\">" + "<input onclick=\"javascript:return UpdateBanquetGrandTotal()\" type='checkbox' checked= 'checked' disabled='disabled'/>" + "</td>";
                else if (gridObject.RegistrationId > 0) {
                    if (isNewlyAdded || isSynced)
                        tr += "<td align='left' style=\"width:5%;\">" + "<input onclick=\"javascript:return UpdateBanquetGrandTotal()\" type='checkbox' checked= 'checked' disabled='disabled'/>" + "</td>";
                    else
                        tr += "<td align='left' style=\"width:5%;\">" + "<input onclick=\"javascript:return UpdateBanquetGrandTotal()\" type='checkbox' disabled='disabled'/>" + "</td>";
                }
                else if (isExist)
                    tr += "<td align='left' style=\"width:5%;\">" + "<input onclick=\"javascript:return UpdateBanquetGrandTotal()\" checked= 'checked' type='checkbox'/>" + "</td>";
                else
                    tr += "<td align='left' style=\"width:5%;\">" + "<input onclick=\"javascript:return UpdateBanquetGrandTotal()\" type='checkbox'/>" + "</td>";

                tr += "<td align='left' style=\"width:20%;\">" + gridObject.BillNumber + "</td>";
                tr += "<td align='left' style=\"width:20%;\">" + gridObject.CostCenter + "</td>";
                tr += "<td align='left' style=\"width:25%;\">" + gridObject.BillAmount + "</td>";
                tr += "<td align='left' style=\"width:20%;\">" + gridObject.PaymentDescription + "</td>";

                tr += "<td align='center' style=\"width:10%;\"> ";
                tr += "<img src=\"../Images/ReportDocument.png\" title=\"Invoice Preview\" alt=\"Invoice Preview\" onclick=\"javascript: return ViewInvoice('BanquetService', " + gridObject.BillId + ")\" style=\"cursor: pointer; \"/>";
                tr += "</td>";

                tr += "<td align='left' style=\"display:none\">" + gridObject.BillId + "</td>";
                tr += "<td align='left' style=\"display:none\">" + gridObject.RegistrationId + "</td>";
                tr += "<td align='left' style=\"display:none\">" + gridObject.GuidId + "</td>";
                tr += "<td align='left' style=\"display:none\">" + gridObject.IsSyncCompleted + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + gridObject.VatAmount + "</td>";

                tr += "</tr>";

                $("#tbBanquetBillInformation tbody").append(tr);
                tr = "";
            });
            UpdateBanquetGrandTotal();
        }

        function UpdateSyncDateTime() {

            var toDate = $("#ContentPlaceHolder1_txtToDate").val();
            toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');

            var param = JSON.stringify({ toDate: toDate });

            $.ajax({
                type: 'POST',
                url: "/Synchronization/DataSynchronization.aspx/UpdateSyncDateTime",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: param,
                async: false,
                success: function (data) {

                    //LoadDataforSync();
                },
                error: function (request, error) {
                    alert("Request: " + JSON.stringify(request));
                    return false;
                }

            });
        }

        function ViewInvoice(serviceType, serviceBillId) {
            var url = "";
            var popup_window = "Invoice Preview";
            if (serviceType == "RoomRegistration") {
                LoadRegistrationIdToSession(serviceBillId);
                url = "/HotelManagement/Reports/frmReportGuestBillPreview.aspx";
            }
            else if (serviceType == "GuestHouseService") {
                url = "/HotelManagement/Reports/frmReportServiceBillInfo.aspx?billID=" + serviceBillId;
            }
            else if (serviceType == "RestaurantService") {
                url = "/POS/Reports/frmReportBillInfo.aspx?billID=" + serviceBillId;
            }
            else if (serviceType == "BanquetService") {
                url = "/Banquet/Reports/frmReportReservationBillInfo.aspx?Id=" + serviceBillId;
            }

            window.open(url, popup_window, "width=800,height=680,left=300,top=50,resizable=yes");
        }

        function LoadRegistrationIdToSession(registrationId) {

            var param = JSON.stringify({ registrationId: registrationId });

            $.ajax({
                type: 'POST',
                url: "/Synchronization/DataSynchronization.aspx/LoadRegistrationIdToSession",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: param,
                async: false,
                success: function (data) {

                    //LoadDataforSync();
                },
                error: function (request, error) {
                    alert("Request: " + JSON.stringify(request));
                    return false;
                }

            });
        }

        function CheckTransferedRoomWithDetails() {
            $("#tblTransferRoomDetails").empty();
            var guidId = 0, billPaidBy = 0, isTransferedRoomSyncing = false;

            $("#tbRegistrationInformation tbody tr").each(function (index, item) {

                guidId = $(this).find("td:eq(7)").text();
                billPaidBy = parseInt($(this).find("td:eq(13)").text());

                isTransferedRoomSyncing = registrationIdList.some(data => data == billPaidBy);

                if (isTransferedRoomSyncing)
                    CheckTransferedRoom($(this).find("td:eq(0)").find("input"));
            });
        }

        function CheckTransferedRoomNUpdateUpdateRegistrationGrandTotal(row) {
            CheckTransferedRoom(row);
            UpdateRegistrationGrandTotal();

        }

        function UpdateRegistrationGrandTotal() {
            var toDate = $("#ContentPlaceHolder1_txtToDate").val();

            var fromDate = $("#ContentPlaceHolder1_lblLastSyncDateTime").text();

            //var dateDifference = CommonHelper.DateDifferenceInDays(fromDate, toDate);
            //var dateDifference = CalculateDateDiff(fromDate, toDate) + 1;
            var nights = 0;
            var grandTotal = 0.00, vatTotal = 0.00, isSyncCompleted, guidId;

            $("#tbRegistrationInformation tbody tr").each(function (index, item) {

                if ($(this).find("td:eq(0)").find("input").is(':checked')) {

                    isSyncCompleted = ($.trim($(item).find("td:eq(8)").text()) == 'true');
                    guidId = $.trim($(item).find("td:eq(7)").text());

                    if (!isSyncCompleted) {
                        //if (dateDifference > 1 || guidId == "null") {
                        nights = parseInt($.trim($(item).find("td:eq(10)").text()), 2);
                        // nights = nights + dateDifference;
                        grandTotal = grandTotal + parseFloat($.trim($(item).find("td:eq(11)").text()));
                        vatTotal = vatTotal + parseFloat($.trim($(item).find("td:eq(9)").text()));
                        //}
                    }
                }
            });

            //$("#lblNights").text(dateDifference);
            $("#lblRegistrationGrandTotal").text(grandTotal.toFixed(2));
            $("#lblRegistrationVatTotal").text(vatTotal.toFixed(2));
        }

        function CheckTransferedRoom(row) {
            var registrationId = 0;

            var billPaidById = parseInt($(row).parent().parent().find("td:eq(13)").text().trim());
            var rowRegistrationId = parseInt($(row).parent().parent().find("td:eq(6)").text().trim());

            var TransferedRoom = registrationData.find(data => data.BillPaidBy == rowRegistrationId);
            var PaidByRoom = registrationData.find(data => data.RegistrationId == rowRegistrationId);
            if (PaidByRoom.BillPaidBy > 0) {
                TransferedRoom = PaidByRoom;
                PaidByRoom = registrationData.find(data => data.RegistrationId == TransferedRoom.BillPaidBy);
            }
            var transferedRegistrationId = TransferedRoom != undefined ? TransferedRoom.RegistrationId : 0;
            var billPaidById = PaidByRoom != undefined ? PaidByRoom.RegistrationId : 0;

            //var isTransferedRoomSyncing = newRegistrationList.some(data => data == regisatration);
            var isChecked = $(row).is(':checked');
            var isDisabled = $(row).is(':disabled');
            var totalRow = 0;
            //$("#tblTransferRoomDetails").empty();

            $("#tbRegistrationInformation tbody tr").each(function (index, item) {

                totalRow = $("#tblTransferRoomDetails tr").length;
                registrationId = parseInt($(this).find("td:eq(6)").text().trim());

                if (registrationId == billPaidById || registrationId == transferedRegistrationId) {
                    if (isChecked) {
                        if (isDisabled)
                            $(this).find("td:eq(0)").find("input").prop('checked', true).prop('disabled', true);
                        else
                            $(this).find("td:eq(0)").find("input").prop('checked', true).prop('disabled', false);
                        var isNotExist = false;

                        if (billPaidById > 0 && transferedRegistrationId > 0) {

                            isNotExist = IsNotExistInDetailstable(billPaidById, transferedRegistrationId);

                            if (isNotExist) {

                                var tr = "";
                                tr += "<tr style=\"background-color:#FFFFFF;\">";
                                //if ((totalRow % 2) == 0) {
                                //    tr += "<tr style=\"background-color:#E3EAEB;\">";
                                //}
                                //else {
                                //    tr += "<tr style=\"background-color:#FFFFFF;\">";
                                //}
                                tr += "<td align='left' style=\"display:none;\">" + billPaidById + "</td>";
                                tr += "<td align='left' style=\"display:none;\">" + transferedRegistrationId + "</td>";
                                tr += "<td align='left' style=\"width:20%;\"> Room : " + TransferedRoom.RoomNumber + " (" + TransferedRoom.RegistrationNumber + ")  bill transfered to  Room : " + PaidByRoom.RoomNumber + " (" + PaidByRoom.RegistrationNumber + ")" + "</td>";
                                tr += "</tr>";

                                $("#tblTransferRoomDetails").append(tr);
                                tr = "";
                            }
                        }

                    }
                    else {
                        $(this).find("td:eq(0)").find("input").prop('checked', false);
                        if (billPaidById > 0 && transferedRegistrationId > 0) {

                            isNotExist = IsNotExistInDetailstable(billPaidById, transferedRegistrationId);

                            if (!isNotExist) {

                                var detailPaidById = 0, detailtransferedRegistrationId = 0;
                                $("#tblTransferRoomDetails tr").each(function (index, item) {

                                    detailPaidById = parseInt($(this).find("td:eq(0)").text().trim());
                                    detailtransferedRegistrationId = parseInt($(this).find("td:eq(1)").text().trim());

                                    if (detailPaidById == billPaidById && detailtransferedRegistrationId == transferedRegistrationId) {
                                        $(this).remove();
                                        return false;
                                    }

                                });
                            }
                        }
                    }

                }

            });

        }


        function IsNotExistInDetailstable(billPaidById, transferedRegistrationId) {
            var detailPaidById = 0, detailtransferedRegistrationId = 0;
            var isNotExist = true;
            $("#tblTransferRoomDetails tr").each(function (index, item) {

                detailPaidById = parseInt($(this).find("td:eq(0)").text().trim());
                detailtransferedRegistrationId = parseInt($(this).find("td:eq(1)").text().trim());

                if (detailPaidById == billPaidById && detailtransferedRegistrationId == transferedRegistrationId) {
                    isNotExist = false;
                    return false;
                }

            });
            return isNotExist;
        }
        function CalculateDateDiff(dateFrom, dateTo) {
            //dateFrom = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(dateFrom, innBoarDateFormat);
            // dateTo = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(dateTo, innBoarDateFormat);
            dateFrom = CommonHelper.DateFormatToMMDDYYYY(dateFrom, '/');
            dateTo = CommonHelper.DateFormatToMMDDYYYY(dateTo, '/');

            //var date1 = new Date(dateFrom);
            //var date2 = new Date(dateTo);
            var dt1 = new Date(dateFrom);
            var dt2 = new Date(dateTo);

            //var timeDiff = Math.abs(date2.getTime() - date1.getTime());
            //var diffDays = Math.floor(timeDiff / (1000 * 3600 * 24));
            return Math.floor((Date.UTC(dt2.getFullYear(), dt2.getMonth(), dt2.getDate()) - Date.UTC(dt1.getFullYear(), dt1.getMonth(), dt1.getDate())) / (1000 * 60 * 60 * 24));
            //return diffDays;
        }

        function UpdateServiceGrandTotal() {
            var grandTotal = 0.00, vatTotal = 0.00, isSyncCompleted;
            $("#tbServiceBillInformation tbody tr").each(function (index, item) {

                if ($(this).find("td:eq(0)").find("input").is(':checked')) {
                    isSyncCompleted = ($.trim($(item).find("td:eq(9)").text()) == 'true');
                    if (!isSyncCompleted) {
                        grandTotal = grandTotal + parseFloat($.trim($(item).find("td:eq(3)").text()));
                        vatTotal = vatTotal + parseFloat($.trim($(item).find("td:eq(10)").text()));
                    }
                }
            });
            $("#lblServiceGrandTotal").text(grandTotal.toFixed(2));
            $("#lblServiceVatTotal").text(vatTotal.toFixed(2));
        }

        function UpdateRestaurantGrandTotal() {
            var grandTotal = 0.00, vatTotal = 0.00, isSyncCompleted;
            $("#tbRestaurantInformation tbody tr").each(function (index, item) {

                if ($(this).find("td:eq(0)").find("input").is(':checked')) {
                    isSyncCompleted = ($.trim($(item).find("td:eq(9)").text()) == 'true');
                    if (!isSyncCompleted) {
                        grandTotal = grandTotal + parseFloat($.trim($(item).find("td:eq(3)").text()));
                        vatTotal = vatTotal + parseFloat($.trim($(item).find("td:eq(10)").text()));
                    }
                }
            });
            $("#lblRestaurantGrandTotal").text(grandTotal.toFixed(2));
            $("#lblRestaurantVatTotal").text(vatTotal.toFixed(2));
        }

        function UpdateBanquetGrandTotal() {
            var grandTotal = 0.00, vatTotal = 0.00;
            $("#tbBanquetBillInformation tbody tr").each(function (index, item) {

                if ($(this).find("td:eq(0)").find("input").is(':checked')) {
                    isSyncCompleted = ($.trim($(item).find("td:eq(9)").text()) == 'true');
                    if (!isSyncCompleted) {
                        grandTotal = grandTotal + parseFloat($.trim($(item).find("td:eq(3)").text()));
                        vatTotal = vatTotal + parseFloat($.trim($(item).find("td:eq(10)").text()));
                    }
                }
            });
            $("#lblBanquetGrandTotal").text(grandTotal.toFixed(2));
            $("#lblBanquetVatTotal").text(vatTotal.toFixed(2));
        }

        function UpdateFinalGrandTotal() {
            var roomGrandTotal = parseFloat($("#lblRegistrationGrandTotal").text());
            var rooVatTotal = parseFloat($("#lblRegistrationVatTotal").text());
            var serviceGrandTotal = parseFloat($("#lblServiceGrandTotal").text());
            var serviceVatTotal = parseFloat($("#lblServiceVatTotal").text());
            var restaurantGrandTotal = parseFloat($("#lblRestaurantGrandTotal").text());
            var restaurantVatTotal = parseFloat($("#lblRestaurantVatTotal").text());
            var banquetGrandTotal = parseFloat($("#lblBanquetGrandTotal").text());
            var banquetVatTotal = parseFloat($("#lblBanquetVatTotal").text());

            var finalGrandTotal = parseFloat((roomGrandTotal + serviceGrandTotal + restaurantGrandTotal + banquetGrandTotal), 2);
            var finalVatTotal = parseFloat((rooVatTotal + serviceVatTotal + restaurantVatTotal + banquetVatTotal), 2);

            $("#lblSRoomGrandTotal").text(roomGrandTotal);
            $("#lblSRoomVatTotal").text(rooVatTotal);
            $("#lblSServiceGrandTotal").text(serviceGrandTotal);
            $("#lblSServiceVatTotal").text(serviceVatTotal);
            $("#lblSRestaurantGrandTotal").text(restaurantGrandTotal);
            $("#lblSRestaurantVatTotal").text(restaurantVatTotal);
            $("#lblSBanquetGrandTotal").text(banquetGrandTotal);
            $("#lblSBanquetVatTotal").text(banquetVatTotal);
            $("#lblFinalGrandTotal").text(finalGrandTotal.toFixed(2));
            $("#lblFinalVatTotal").text(finalVatTotal.toFixed(2));
        }
        function ClearAmount() {
            $("#lblRegistrationGrandTotal").text('0');
            $("#lblRegistrationVatTotal").text('0');
            $("#lblServiceGrandTotal").text('0');
            $("#lblServiceVatTotal").text('0');
            $("#lblRestaurantGrandTotal").text('0');
            $("#lblRestaurantVatTotal").text('0');
            $("#lblBanquetGrandTotal").text('0');
            $("#lblBanquetVatTotal").text('0');

            $("#lblSRoomGrandTotal").text('0');
            $("#lblSRoomVatTotal").text('0');
            $("#lblSServiceGrandTotal").text('0');
            $("#lblSServiceVatTotal").text('0');
            $("#lblSRestaurantGrandTotal").text('0');
            $("#lblSRestaurantVatTotal").text('0');
            $("#lblSBanquetGrandTotal").text('0');
            $("#lblSBanquetVatTotal").text('0');
            $("#lblFinalGrandTotal").text('0');
            $("#lblFinalVatTotal").text('0');
        }

        function SyncSetupData() {

            GetSetupData();
        }

        function GetSetupData() {
            return $.ajax({
                type: "POST",
                url: "./DataSynchronization.aspx/GetSetupData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    SendSentupDataToApi(result.d);
                },
                error: function (result) {
                }
            });
        }

        function SendSentupDataToApi(data) {
            return $.ajax({
                type: 'POST',
                url: baseUrl + '/Sync/SyncLocation',
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.Success)
                        toastr.success(data.AlertMessage);
                    else
                        toastr.info('Contact With Admin.');
                },
                error: function () {
                    toastr.info('Contact With Admin');
                }

            });
        }
    </script>
    <asp:HiddenField ID="hfAdditionalCharge" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCityCharge" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsRatePlusPlus" runat="server" />
    <asp:HiddenField ID="hfAdditionalChargeType" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfInclusiveHotelManagementBill" runat="server" />
    <asp:HiddenField ID="hfIsVatEnableOnGuestHouseCityCharge" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfGuestHouseVat" runat="server" />
    <asp:HiddenField ID="hfGuestHouseServiceCharge" runat="server" />
    <asp:HiddenField ID="hfLastSyncDate" runat="server" />
    <asp:HiddenField ID="hfSyncApiUrl" runat="server" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Registration</a></li>
            <li id="Li1" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Service Bill</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-3">Restaurant Bill</a></li>
            <li id="C" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-4">Banquet</a></li>
            <li id="D" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-5">Final</a></li>
        </ul>
        <div id="tab-1">
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Seach Data
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label">Last Sync Date Time :</label>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblLastSyncDateTime" Style="font-weight: bold" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label required-field">To Date</label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtToDate" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <input type="button" value="Search" id="btnSearch" onclick="SearchRegistration()" class="btn btn-primary btn-sm" />
                                <input type="button" value="Sync Setup Data" id="btnSyncSetupData" onclick="SyncSetupData()" class="btn btn-primary btn-sm" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-heading">
                    Registration Information
                </div>
                <div class="panel-body">
                    <table id='tbRegistrationInformation' class="table table-bordered table-condensed table-responsive" width="100%">
                        <colgroup>
                            <col style="width: 5%;" />
                            <col style="width: 15%;" />
                            <col style="width: 20%;" />
                            <col style="width: 10%;" />
                            <col style="width: 10%;" />
                            <col style="width: 10%;" />
                            <col style="width: 10%;" />
                            <col style="width: 10%;" />
                            <col style="width: 10%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>
                                    <input id="chkAll" type="checkbox" />
                                </td>
                                <td>Registration No
                                </td>
                                <td>Guest Name
                                </td>
                                <td>Check In
                                </td>
                                <td>Check Out
                                </td>
                                <td>Room Rate
                                </td>
                                <td style="display: none">RegistrationId
                                </td>
                                <td style="display: none">Guid Id
                                </td>
                                <td style="display: none">IsSyncedCompleted
                                </td>
                                <td style="display: none">Vat Amount
                                </td>
                                <td>Night
                                </td>
                                <td>Total
                                </td>
                                <td>Action
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <%--<div class="row">
                        <div class="col-md-2 totalAmout">
                            Nights  :
                        </div>
                        <div class="col-md-2">
                            <label id="lblNights">0</label>
                        </div>
                    </div>--%>
                    <div class="col-md-6">
                        <div class="row">
                            <div class="col-md-4 totalAmout">
                                <label>Grand Total  :</label>
                            </div>
                            <div class="col-md-2">
                                <label id="lblRegistrationGrandTotal">0</label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4 totalAmout">
                                Vat Total  :
                            </div>
                            <div class="col-md-2">
                                <label id="lblRegistrationVatTotal">0</label>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <table id="tblTransferRoomDetails" class="table table-bordered table-condensed table-responsive"></table>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <input type="button" value="Sync Later" class="btn btn-primary btn-sm"
                                onclick="SyncLater('Registration')" />
                            <asp:Button ID="btnNext" runat="server" TabIndex="4" Text="Next" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="return NextTab()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Service Bill
                </div>
                <div class="panel-body">
                    <table id='tbServiceBillInformation' class="table table-bordered table-condensed table-responsive" width="100%">
                        <colgroup>
                            <col style="width: 5%;" />
                            <col style="width: 20%;" />
                            <col style="width: 25%;" />
                            <col style="width: 15%;" />
                            <col style="width: 25%;" />
                            <col style="width: 10%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>
                                    <input id="chkAllService" type="checkbox" />
                                </td>
                                <td>BillNumber
                                </td>
                                <td>Cost Center
                                </td>
                                <td>Bill Amount
                                </td>
                                <td>Payment Description
                                </td>
                                <td>Action
                                </td>
                                <td style="display: none">BillId
                                </td>
                                <td style="display: none">RegistrationId
                                </td>
                                <td style="display: none">GuidId
                                </td>
                                <td style="display: none">IsSyncedCompleted
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <div class="row">
                        <div class="col-md-2 totalAmout">
                            Grand Total  :
                        </div>
                        <div class="col-md-2">
                            <label id="lblServiceGrandTotal">0</label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2 totalAmout">
                            Vat Total  :
                        </div>
                        <div class="col-md-2">
                            <label id="lblServiceVatTotal">0</label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <input type="button" value="Sync Later" class="btn btn-primary btn-sm"
                                onclick="SyncLater('Service')" />
                            <input type="button" value="Next" class="TransactionalButton btn btn-primary btn-sm" onclick="NextTab2()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-3">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Restaurant
                </div>
                <div class="panel-body">
                    <table id='tbRestaurantInformation' class="table table-bordered table-condensed table-responsive" width="100%">
                        <colgroup>
                            <col style="width: 5%;" />
                            <col style="width: 20%;" />
                            <col style="width: 25%;" />
                            <col style="width: 20%;" />
                            <col style="width: 20%;" />
                            <col style="width: 10%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>
                                    <input id="chkAllr" type="checkbox" />
                                </td>
                                <td>BillNumber
                                </td>
                                <td>Cost Center
                                </td>
                                <td>Bill Amount
                                </td>
                                <td>Payment Description
                                </td>
                                <td>Action
                                </td>
                                <td style="display: none">BillId
                                </td>
                                <td style="display: none">RegistrationId
                                </td>
                                <td style="display: none">GuidId
                                </td>
                                <td style="display: none">IsSyncedCompleted
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <div class="row">
                        <div class="col-md-2 totalAmout">
                            Grand Total  :
                        </div>
                        <div class="col-md-2">
                            <label id="lblRestaurantGrandTotal">0</label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2 totalAmout">
                            Vat Total  :
                        </div>
                        <div class="col-md-2">
                            <label id="lblRestaurantVatTotal">0</label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <input type="button" value="Sync Later" class="btn btn-primary btn-sm"
                                onclick="SyncLater('Restaurant')" />
                            <input type="button" value="Next" class="TransactionalButton btn btn-primary btn-sm" onclick="NextTab3()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-4">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Banquet
                </div>
                <div class="panel-body">
                    <table id='tbBanquetBillInformation' class="table table-bordered table-condensed table-responsive" width="100%">
                        <colgroup>
                            <col style="width: 5%;" />
                            <col style="width: 20%;" />
                            <col style="width: 20%;" />
                            <col style="width: 25%;" />
                            <col style="width: 20%;" />
                            <col style="width: 10%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>
                                    <input id="chkAllBanquet" type="checkbox" />
                                </td>
                                <td>BillNumber
                                </td>
                                <td>Cost Center
                                </td>
                                <td>Bill Amount
                                </td>
                                <td>Payment Description
                                </td>
                                <td>Action
                                </td>
                                <td style="display: none">BillId
                                </td>
                                <td style="display: none">RegistrationId
                                </td>
                                <td style="display: none">GuidId
                                </td>
                                <td style="display: none">IsSyncedCompleted
                                </td>
                                <td style="display: none">VatAmount
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <div class="row">
                        <div class="col-md-2 totalAmout">
                            Grand Total  :
                        </div>
                        <div class="col-md-2">
                            <label id="lblBanquetGrandTotal">0</label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2 totalAmout">
                            Vat Total  :
                        </div>
                        <div class="col-md-2">
                            <label id="lblBanquetVatTotal">0</label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <input type="button" value="Sync Later" class="btn btn-primary btn-sm"
                                onclick="SyncLater('Banquet')" />
                            <input type="button" value="Next" class="TransactionalButton btn btn-primary btn-sm" onclick="NextTab4()" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-5">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Final
                </div>
                <div class="panel-body">
                    <table id="tblFinalinfo" class="table table-hover">
                        <tbody>
                            <tr>
                                <td class="col-md-4 totalAmout">
                                    <label>Room Grand Total</label>
                                </td>
                                <td class="col-md-1" style="font-weight: bold;">:
                                </td>
                                <td class="col-md-4">
                                    <label id="lblSRoomGrandTotal">0</label>
                                </td>
                            </tr>
                            <tr>
                                <td class="col-md-4 totalAmout">
                                    <label>Room Vat Total</label>
                                </td>
                                <td class="col-md-1" style="font-weight: bold;">:
                                </td>
                                <td class="col-md-4">
                                    <label id="lblSRoomVatTotal">0</label>
                                </td>
                            </tr>
                            <tr>
                                <td class="col-md-4 totalAmout">
                                    <label>Service Bill Grand Total</label>
                                </td>
                                <td class="col-md-1" style="font-weight: bold;">:
                                </td>
                                <td class="col-md-4">
                                    <label id="lblSServiceGrandTotal">0</label>
                                </td>
                            </tr>
                            <tr>
                                <td class="col-md-4 totalAmout">
                                    <label>Service Bill Vat Total</label>
                                </td>
                                <td class="col-md-1" style="font-weight: bold;">:
                                </td>
                                <td class="col-md-4">
                                    <label id="lblSServiceVatTotal">0</label>
                                </td>
                            </tr>
                            <tr>
                                <td class="col-md-4 totalAmout">
                                    <label>Restaurant Grand Total</label>
                                </td>
                                <td class="col-md-1" style="font-weight: bold;">:
                                </td>
                                <td class="col-md-4">
                                    <label id="lblSRestaurantGrandTotal">0</label>
                                </td>
                            </tr>
                            <tr>
                                <td class="col-md-4 totalAmout">
                                    <label>Restaurant Vat Total</label>
                                </td>
                                <td class="col-md-1" style="font-weight: bold;">:
                                </td>
                                <td class="col-md-4">
                                    <label id="lblSRestaurantVatTotal">0</label>
                                </td>
                            </tr>
                            <tr>
                                <td class="col-md-4 totalAmout">
                                    <label>Banquet Bill Grand Total</label>
                                </td>
                                <td class="col-md-1" style="font-weight: bold;">:
                                </td>
                                <td class="col-md-4">
                                    <label id="lblSBanquetGrandTotal">0</label>
                                </td>
                            </tr>
                            <tr>
                                <td class="col-md-4 totalAmout">
                                    <label>Banquet Bill Vat Total</label>
                                </td>
                                <td class="col-md-1" style="font-weight: bold;">:
                                </td>
                                <td class="col-md-4">
                                    <label id="lblSBanquetVatTotal">0</label>
                                </td>
                            </tr>
                            <tr>
                                <td class="col-md-4 totalAmout">
                                    <label>Final Grand Total</label>
                                </td>
                                <td class="col-md-1" style="font-weight: bold;">:
                                </td>
                                <td class="col-md-4">
                                    <label id="lblFinalGrandTotal">0</label>
                                </td>
                            </tr>
                            <tr>
                                <td class="col-md-4 totalAmout">
                                    <label>Final Vat Total</label>
                                </td>
                                <td class="col-md-1" style="font-weight: bold;">:
                                </td>
                                <td class="col-md-4">
                                    <label id="lblFinalVatTotal">0</label>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="row">
                        <div class="col-md-12">
                            <input type="button" id="btnSaveTemporary" value="Sync Later" class="btn btn-primary btn-sm"
                                onclick="SyncLater('All')" />
                            <input id="btnSync" type="button" class="TransactionalButton btn btn-primary btn-sm"
                                value="Sync" onclick="SyncData()" />

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
