<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmNightAudit.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmNightAudit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //var ab = {};
        var TotalRoomRateGlobalValue = 0;
        //For FillForm-------------------------
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }        
        function PerformFillFormForUpdateAction(actionData) {
            //toastr.info(actionData.split("~")[0]);            
            PageMethods.ApprovedNightAuditedDataForUpdateAction(actionData, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function PerformFillFormServiceAction(actionId) {
            PageMethods.FillServiceForm('GuestHouseService', actionId, OnFillServiceFormObjectSucceeded, OnFillServiceFormObjectFailed);
            return false;
        }
        function PerformFillFormRestaurantServiceAction(actionId) {
            PageMethods.FillServiceForm('RestaurantService', actionId, OnFillRestaurantServiceFormObjectSucceeded, OnFillRestaurantServiceFormObjectFailed);
            return false;
        }
        function PerformFillFormServiceForUpdateAction(actionId) {
            PageMethods.FillServiceFormUpdateAction('GuestHouseService', actionId, OnFillServiceFormObjectSucceeded, OnFillServiceFormObjectFailed);
            return false;
        }
        function PerformFillFormRestaurantServiceForUpdateAction(actionId) {
            PageMethods.BillResettlement(actionId, OnBillResettlementSucceeded, OnBillResettlementFailed);
            //PageMethods.FillServiceFormUpdateAction('RestaurantService', actionId, OnFillRestaurantServiceFormObjectSucceeded, OnFillServiceFormObjectFailed);
            return false;
        }
        function OnBillResettlementSucceeded(result) {
            if (result.IsSuccess == true) {
                window.location = result.RedirectUrl;
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
        }
        function OnBillResettlementFailed() { }
        $(document).ready(function () {
            var txtApprovedDate = '<%=txtApprovedDate.ClientID%>'
            $('#txtApprovedDate').click(function () {
            });

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Audit Process</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Night Audit</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            CommonHelper.ApplyDecimalValidation();

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $(function () {
                $("#myTabs").tabs();
            });

            $("#ContentPlaceHolder1_txtApprovedDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            var tabNumbers = $("#<%=hfNightAuditInformation.ClientID %>").val();
            var tab1 = 0, tab2 = 0, tab3 = 0, tab4 = 0, tab5 = 0, tab6 = 0, tab7 = 0, tab8 = 0, tab9 = 0, tab10 = 0, tab11 = 0, tab12 = 0;            
            for (var i = 0; i < tabNumbers.length; i++) {
                if (tabNumbers[i] == 1) {
                    tab1++;
                }
                else if (tabNumbers[i] == 2) {
                    tab2++;
                }
                else if (tabNumbers[i] == 3) {
                    tab3++;
                }
                else if (tabNumbers[i] == 4) {
                    tab4++;
                }
                else if (tabNumbers[i] == 5) {
                    tab5++;
                }
                else if (tabNumbers[i] == 6) {
                    tab6++;
                }
                else if (tabNumbers[i] == 7) {
                    tab7++;
                }
                else if (tabNumbers[i] == 8) {
                    tab8++;
                }
                else if (tabNumbers[i] == 9) {
                    tab9++;
                }
                else if (tabNumbers[i] == 10) {
                    tab10++;
                }
                else if (tabNumbers[i] == 11) {
                    tab11++;
                }
                else if (tabNumbers[i] == 12) {
                    tab12++;
                }
            }            
            if (tab3 == 1 && tab2 == 0) {
                $("#myTabs").tabs({
                    active: 2
                });
                $($("#myTabs").find("li")[2]).show();
                $("#SrcRoomInformationDiv").hide();
            }
            else if (tab3 == 1 || tab2 == 1) {
                $($("#myTabs").find("li")[2]).show();
                $("#SrcRoomInformationDiv").show();
            }
            if (tab2 == 1 || tab4 == 1 || tab5 == 1 || tab6 == 1 || tab7 == 1 || tab8 == 1 || tab9 == 1 || tab10 == 1 || tab11 == 1 || tab12 == 1) {
                $($("#myTabs").find("li")[0]).show();
                $($("#myTabs").find("li")[1]).show();
                $("#SrcRoomInformationDiv").show();
            }

            var txtTotalRoomRate = '<%=txtTotalRoomRate.ClientID%>'
            $('#' + txtTotalRoomRate).blur(function () {
                var hfInvoiceServiceCharge = '<%=hfInvoiceServiceCharge.ClientID%>'
                var hfGuestServiceChargeRate = '<%=hfGuestServiceChargeRate.ClientID%>'
                var NegotiatedRoomRate = $('#' + txtTotalRoomRate).val();
                var GuestHouseServiceCharge = $('#' + hfGuestServiceChargeRate).val();
                var IsServiceChargeEnable = "1";

                var hfIsVatAmountEnable = '<%=hfIsVatAmountEnable.ClientID%>'
                var hfGuestVatAmountRate = '<%=hfGuestVatAmountRate.ClientID%>'
                var GuestHouseVatAmount = $('#' + hfGuestVatAmountRate).val();
                var IsVatEnable = "1";

                var VatAmount = parseFloat(NegotiatedRoomRate) * parseFloat(parseFloat(GuestHouseVatAmount) / (100 + parseFloat(GuestHouseVatAmount)));
                var ServiceCharge = (parseFloat(NegotiatedRoomRate) - (VatAmount * 1)) * 1 * (parseFloat(GuestHouseServiceCharge) / (100 + parseFloat(GuestHouseServiceCharge)));

                var txtRoomRate = '<%=txtRoomRate.ClientID%>'
                var txtVatAmount = '<%=txtVatAmount.ClientID%>'
                var txtServiceCharge = '<%=txtServiceCharge.ClientID%>'
                $('#' + txtRoomRate).val((NegotiatedRoomRate - ServiceCharge - VatAmount).toFixed(2));
                $('#' + txtVatAmount).val(VatAmount.toFixed(2));
                $('#' + txtServiceCharge).val(ServiceCharge.toFixed(2));

                /*
                var txtVatAmountPercent = '<%=txtVatAmountPercent.ClientID%>'
                var txtServiceChargePercent = '<%=txtServiceChargePercent.ClientID%>'
                var txtReferenceSalesCommission = '<%=txtReferenceSalesCommission.ClientID%>'
                var txtReferenceSalesCommissionPercent = '<%=txtReferenceSalesCommissionPercent.ClientID%>'
                var txtCalculatedPercentAmount = '<%=txtCalculatedPercentAmount.ClientID%>'

                $('#' + txtCalculatedPercentAmount).val(100)

                var VatData = (parseFloat($('#' + txtRoomRate).val()) * parseFloat($('#' + txtVatAmountPercent).val())) / parseFloat($('#' + txtCalculatedPercentAmount).val());
                //var ServiceChargeData = (parseFloat($('#' + txtRoomRate).val()) * parseFloat($('#' + txtServiceChargePercent).val())) / parseFloat($('#' + txtCalculatedPercentAmount).val());
                var ServiceChargeData = (parseFloat($('#' + txtRoomRate).val()) * parseFloat($('#' + txtServiceChargePercent).val())) / parseFloat($('#' + txtCalculatedPercentAmount).val());
                var ReferenceSalesCommission = (parseFloat($('#' + txtRoomRate).val()) * parseFloat($('#' + txtReferenceSalesCommissionPercent).val()));
                var txtVatAmount = '<%=txtVatAmount.ClientID%>'
                var txtServiceCharge = '<%=txtServiceCharge.ClientID%>'
                $('#' + txtVatAmount).val(VatData)
                $('#' + txtServiceCharge).val(ServiceChargeData)
                $('#' + txtReferenceSalesCommission).val(ReferenceSalesCommission)
                */
            });


            var txtServiceRate = '<%=txtServiceRate.ClientID%>'
            $('#' + txtServiceRate).blur(function () {
                CalculationServiceInformation();
            });

            var txtServiceQuantity = '<%=txtServiceQuantity.ClientID%>'
            $('#' + txtServiceQuantity).blur(function () {
                CalculationServiceInformation();
            });

            var txtRestaurantServiceRate = '<%=txtRestaurantServiceRate.ClientID%>'
            $('#' + txtRestaurantServiceRate).blur(function () {
                CalculationRestaurantServiceInformation();
            });

            var txtRestaurantServiceQuantity = '<%=txtRestaurantServiceQuantity.ClientID%>'
            $('#' + txtRestaurantServiceQuantity).blur(function () {
                CalculationRestaurantServiceInformation();
            });
        });

        function CalculationServiceInformation() {
            var txtServiceRate = '<%=txtServiceRate.ClientID%>'
            var txtServiceQuantity = '<%=txtServiceQuantity.ClientID%>'
            var txtGuestServiceVatAmountPercent = '<%=txtGuestServiceVatAmountPercent.ClientID%>'
            var txtGuestServiceServiceChargePercent = '<%=txtGuestServiceServiceChargePercent.ClientID%>'
            var txtGuestServiceCalculatedPercentAmount = '<%=txtGuestServiceCalculatedPercentAmount.ClientID%>'

            $('#' + txtGuestServiceCalculatedPercentAmount).val(100)
            var VatData = (parseFloat($('#' + txtServiceRate).val() * $('#' + txtServiceQuantity).val()) * parseFloat($('#' + txtGuestServiceVatAmountPercent).val())) / parseFloat($('#' + txtGuestServiceCalculatedPercentAmount).val());
            var ServiceChargeData = (parseFloat($('#' + txtServiceRate).val() * $('#' + txtServiceQuantity).val()) * parseFloat($('#' + txtGuestServiceServiceChargePercent).val())) / parseFloat($('#' + txtGuestServiceCalculatedPercentAmount).val());
            var txtGuestServiceVatAmount = '<%=txtGuestServiceVatAmount.ClientID%>'
            var txtGuestServiceServiceCharge = '<%=txtGuestServiceServiceCharge.ClientID%>'
            $('#' + txtGuestServiceVatAmount).val(VatData.toFixed(2))
            $('#' + txtGuestServiceServiceCharge).val(ServiceChargeData.toFixed(2))
        };

        function CalculationRestaurantServiceInformation() {
            var txtRestaurantServiceRate = '<%=txtRestaurantServiceRate.ClientID%>'
            var txtRestaurantServiceQuantity = '<%=txtRestaurantServiceQuantity.ClientID%>'
            var txtRestaurantGuestServiceVatAmountPercent = '<%=txtRestaurantGuestServiceVatAmountPercent.ClientID%>'
            var txtRestaurantGuestServiceServiceChargePercent = '<%=txtRestaurantGuestServiceServiceChargePercent.ClientID%>'
            var txtRestaurantGuestServiceCalculatedPercentAmount = '<%=txtRestaurantGuestServiceCalculatedPercentAmount.ClientID%>'

            $('#' + txtRestaurantGuestServiceCalculatedPercentAmount).val(100)
            var VatData = (parseFloat($('#' + txtRestaurantServiceRate).val() * $('#' + txtRestaurantServiceQuantity).val()) * parseFloat($('#' + txtRestaurantGuestServiceVatAmountPercent).val())) / parseFloat($('#' + txtRestaurantGuestServiceCalculatedPercentAmount).val());
            var ServiceChargeData = (parseFloat($('#' + txtRestaurantServiceRate).val() * $('#' + txtRestaurantServiceQuantity).val()) * parseFloat($('#' + txtRestaurantGuestServiceServiceChargePercent).val())) / parseFloat($('#' + txtRestaurantGuestServiceCalculatedPercentAmount).val());
            var txtRestaurantGuestServiceVatAmount = '<%=txtRestaurantGuestServiceVatAmount.ClientID%>'
            var txtRestaurantGuestServiceServiceCharge = '<%=txtRestaurantGuestServiceServiceCharge.ClientID%>'
            $('#' + txtRestaurantGuestServiceVatAmount).val(VatData.toFixed(2))
            $('#' + txtRestaurantGuestServiceServiceCharge).val(ServiceChargeData.toFixed(2))
        };

        function OnFillFormObjectSucceeded(result) {
            
            $("#<%=hfApprovedServiceType.ClientID %>").val(result.ApprovedServiceType);
            $("#<%=hfRegistrationId.ClientID %>").val(result.RegistrationId);
            $("#<%=hfApprovedId.ClientID %>").val(result.ApprovedId);
            $("#<%=txtUnitPrice.ClientID %>").val(result.UnitPrice);
            $("#<%=txtCalculatedTotalRoomRate.ClientID %>").val(result.TotalCalculatedAmount);

            $("#<%=hfIsRatePlusPlus.ClientID %>").val(result.IsRatePlusPlusConfig);
            $("#<%=hfGuestHouseVat.ClientID %>").val(result.VatAmountConfig);
            $("#<%=hfGuestHouseServiceCharge.ClientID %>").val(result.ServiceChargeConfig);
            $("#<%=hfCityCharge.ClientID %>").val(result.CitySDChargeConfig);
            $("#<%=hfAdditionalCharge.ClientID %>").val(result.AdditionalChargeConfig);
            $("#<%=hfAdditionalChargeType.ClientID %>").val(result.AdditionalChargeTypeConfig);
            $("#<%=hfIsVatEnableOnGuestHouseCityCharge.ClientID %>").val(result.IsVatOnSDChargeConfig);
            $("#<%=hfIsDiscountApplicableOnRackRate.ClientID %>").val(result.IsDiscountApplicableOnRackRateConfig);

            CalculateRoomRateInclusively(result.IsServiceChargeEnable, result.IsCitySDChargeEnable, result.IsVatAmountEnable, result.IsAdditionalChargeEnable);
            
            <%--var guest = eval(result);
            $("#<%=ApprovedIdHiddenField.ClientID %>").val(guest.ApprovedId);
            $("#<%=txtRegistrationId.ClientID %>").val(guest.RegistrationId);
            $("#<%=txtRoomIdHiddenField.ClientID %>").val(guest.RoomId);
            $("#<%=txtRoomNumber.ClientID %>").val(guest.RoomNumber);
            $("#<%=txtGuestName.ClientID %>").val(result.GuestName);
            $("#<%=txtRoomType.ClientID %>").val(result.RoomType);
            $("#<%=txtPreviousRoomRate.ClientID %>").val(result.RoomRate);
            $("#<%=txtRoomRate.ClientID %>").val(result.CalculatedRoomRate);
            $("#<%= txtBPPercentAmount.ClientID %>").val(result.BPPercentAmount);
            $("#<%= txtDiscountAmount.ClientID %>").val(result.BPDiscountAmount);
            $("#<%=txtRoomNumber.ClientID %>").attr('readonly', true);
            $("#<%=txtGuestName.ClientID %>").attr('readonly', true);
            $("#<%=txtRoomType.ClientID %>").attr('readonly', true);
            $("#<%=txtPreviousRoomRate.ClientID %>").attr('readonly', true);
            $("#<%=txtVatAmount.ClientID %>").val(result.VatAmount);
            $("#<%=txtVatAmount.ClientID %>").attr('readonly', true);

            $("#<%=txtRoomRate.ClientID %>").attr('readonly', true);
            $("#<%=txtServiceCharge.ClientID %>").attr('readonly', true);

            $("#<%=txtVatAmountPercent.ClientID %>").val(result.VatAmountPercent);
            $("#<%=txtServiceChargePercent.ClientID %>").val(result.ServiceChargePercent);
            $("#<%=txtReferenceSalesCommission.ClientID %>").val(result.ReferenceSalesCommission);
            $("#<%=txtReferenceSalesCommissionPercent.ClientID %>").val(result.ReferenceSalesCommissionPercent);
            $("#<%=txtCalculatedPercentAmount.ClientID %>").val(result.CalculatedPercentAmount);

            $("#<%=txtTotalRoomRate.ClientID %>").val(result.TotalCalculatedAmount);
            $("#<%=hfIsBillInclusive.ClientID %>").val(result.IsBillInclusive);
            $("#<%=hfGuestServiceChargeRate.ClientID %>").val(result.GuestServiceChargeRate);
            $("#<%=hfGuestVatAmountRate.ClientID %>").val(result.GuestVatAmountRate);
            $("#<%=hfInvoiceServiceCharge.ClientID %>").val(result.InvoiceServiceCharge);
            $("#<%=hfIsVatAmountEnable.ClientID %>").val(result.IsVatAmountEnable);

            $('#EntryPanel').show("slow");

            if (guest.ApprovedId > 0) {
                $('#btnSaveButtonDiv').hide("slow");
                $('#btnUpdateRoomApprovedDataButtonDiv').show("slow");
            }
            else {
                $('#btnSaveButtonDiv').show("slow");
                $('#btnUpdateRoomApprovedDataButtonDiv').hide("slow");
            }--%>
        }

        function OnFillFormObjectFailed(error) {
            toastr.error(error);
        }
        

        function OnFillServiceFormObjectSucceeded(result) {
            //var guest = eval(result);
            var date = new Date(result.ServiceDate);
            $("#<%=ServiceApprovedIdHiddenField.ClientID %>").val(result.ApprovedId);
            $("#<%=txtServiceBillId.ClientID %>").val(result.ServiceBillId);
            $("#<%=txtRegistrationNumber.ClientID %>").val(result.RegistrationNumber);
            $("#<%=txtServiceDate.ClientID %>").val(GetStringFromDateTime(result.ServiceDate));
            $("#<%=txtServiceName.ClientID %>").val(result.ServiceName);
            $("#<%=txtServiceRoomNumber.ClientID %>").val(result.RoomNumber);
            $("#<%=txtServiceRegistrationId.ClientID %>").val(result.RegistrationId);
            $("#<%=txtServiceServiceId.ClientID %>").val(result.ServiceId);
            $("#<%=txtServiceRate.ClientID %>").val(result.ServiceRate);
            $("#<%=txtServiceQuantity.ClientID %>").val(result.ServiceQuantity);
            $("#<%=txtDiscountAmount.ClientID %>").val(result.DiscountAmount);
            $("#<%=txtServiceBillId.ClientID %>").val(result.ServiceBillId);
            $("#<%=txtServiceType.ClientID %>").val(result.ServiceType);
            $("#<%=txtPaymentMode.ClientID %>").val(result.RoomNumber);
            $("#<%=txtServiceRegistrationNumber.ClientID %>").val(result.RegistrationNumber);
            $("#<%=txtServiceServiceName.ClientID %>").val(result.ServiceName);
            $("#<%=txtServiceServiceDate.ClientID %>").val(GetStringFromDateTime(result.ServiceDate));

            $("#<%=txtGuestServiceVatAmount.ClientID %>").val(result.VatAmount);
            $("#<%=txtGuestServiceServiceCharge.ClientID %>").val(result.ServiceCharge);
            $("#<%=txtGuestServiceVatAmount.ClientID %>").attr('readonly', true);
            $("#<%=txtGuestServiceVatAmountPercent.ClientID %>").val(result.VatAmountPercent);
            $("#<%=txtGuestServiceServiceChargePercent.ClientID %>").val(result.ServiceChargePercent);
            $("#<%=txtGuestServiceCalculatedPercentAmount.ClientID %>").val(result.CalculatedPercentAmount);


            $('#ServiceApprovedDiv').show("slow");
            if (result.ApprovedId > 0) {
                $('#ServiceBillDiv').hide("slow");
                $('#ServiceBillApprovedDiv').show("slow");
            }
            else {
                $('#ServiceBillDiv').show("slow");
                $('#ServiceBillApprovedDiv').hide("slow");
            }
        }

        function OnFillRestaurantServiceFormObjectSucceeded(result) {
            //var guest = eval(result);
            var date = new Date(result.ServiceDate);
            $("#<%=RestaurantServiceApprovedIdHiddenField.ClientID %>").val(result.ApprovedId);
            $("#<%=txtRestaurantServiceBillId.ClientID %>").val(result.ServiceBillId);
            $("#<%=txtRestaurantRegistrationNumber.ClientID %>").val(result.RegistrationNumber);
            $("#<%=txtRestaurantServiceDate.ClientID %>").val(GetStringFromDateTime(result.ServiceDate));
            $("#<%=txtRestaurantServiceName.ClientID %>").val(result.ServiceName);
            $("#<%=txtRestaurantServiceRoomNumber.ClientID %>").val(result.RoomNumber);
            $("#<%=txtRestaurantServiceRegistrationId.ClientID %>").val(result.RegistrationId);
            $("#<%=txtRestaurantServiceServiceId.ClientID %>").val(result.ServiceId);
            $("#<%=txtRestaurantServiceRate.ClientID %>").val(result.ServiceRate);
            $("#<%=txtRestaurantServiceQuantity.ClientID %>").val(result.ServiceQuantity);
            $("#<%=txtRestaurantDiscountAmount.ClientID %>").val(result.DiscountAmount);
            $("#<%=txtRestaurantServiceBillId.ClientID %>").val(result.ServiceBillId);
            $("#<%=txtRestaurantServiceType.ClientID %>").val(result.ServiceType);
            $("#<%=txtRestaurantPaymentMode.ClientID %>").val(result.RoomNumber);
            $("#<%=txtRestaurantServiceRegistrationNumber.ClientID %>").val(result.RegistrationNumber);
            $("#<%=txtRestaurantServiceServiceName.ClientID %>").val(result.ServiceName);
            $("#<%=txtRestaurantServiceServiceDate.ClientID %>").val(GetStringFromDateTime(result.ServiceDate));

            $("#<%=txtRestaurantGuestServiceVatAmount.ClientID %>").val(result.VatAmount);
            $("#<%=txtRestaurantGuestServiceServiceCharge.ClientID %>").val(result.ServiceCharge);
            $("#<%=txtRestaurantGuestServiceVatAmount.ClientID %>").attr('readonly', true);
            $("#<%=txtRestaurantGuestServiceVatAmountPercent.ClientID %>").val(result.VatAmountPercent);
            $("#<%=txtRestaurantGuestServiceServiceChargePercent.ClientID %>").val(result.ServiceChargePercent);
            $("#<%=txtRestaurantGuestServiceCalculatedPercentAmount.ClientID %>").val(result.CalculatedPercentAmount);


            $('#RestaurantServiceApprovedDiv').show("slow");
            if (result.ApprovedId > 0) {
                $('#RestaurantServiceBillDiv').hide("slow");
                $('#RestaurantServiceBillApprovedDiv').show("slow");
            }
            else {
                $('#RestaurantServiceBillDiv').show("slow");
                $('#RestaurantServiceBillApprovedDiv').hide("slow");
            }
        }

        function OnFillServiceFormObjectFailed(error) {
            toastr.error(error);
        }

        function OnFillRestaurantServiceFormObjectSucceeded(result) {
            //var guest = eval(result);
            var date = new Date(result.ServiceDate);
            $("#<%=RestaurantServiceApprovedIdHiddenField.ClientID %>").val(result.ApprovedId);
            $("#<%=txtRestaurantServiceBillId.ClientID %>").val(result.ServiceBillId);
            $("#<%=txtRestaurantRegistrationNumber.ClientID %>").val(result.RegistrationNumber);
            $("#<%=txtRestaurantServiceDate.ClientID %>").val(GetStringFromDateTime(result.ServiceDate));
            $("#<%=txtRestaurantServiceName.ClientID %>").val(result.ServiceName);
            $("#<%=txtRestaurantServiceRoomNumber.ClientID %>").val(result.RoomNumber);
            $("#<%=txtRestaurantServiceRegistrationId.ClientID %>").val(result.RegistrationId);
            $("#<%=txtRestaurantServiceServiceId.ClientID %>").val(result.ServiceId);
            $("#<%=txtRestaurantServiceRate.ClientID %>").val(result.ServiceRate);
            $("#<%=txtRestaurantServiceQuantity.ClientID %>").val(result.ServiceQuantity);
            $("#<%=txtRestaurantDiscountAmount.ClientID %>").val(result.DiscountAmount);
            $("#<%=txtRestaurantServiceBillId.ClientID %>").val(result.ServiceBillId);
            $("#<%=txtRestaurantServiceType.ClientID %>").val(result.ServiceType);
            $("#<%=txtRestaurantPaymentMode.ClientID %>").val(result.RoomNumber);
            $("#<%=txtRestaurantServiceRegistrationNumber.ClientID %>").val(result.RegistrationNumber);
            $("#<%=txtRestaurantServiceServiceName.ClientID %>").val(result.ServiceName);
            $("#<%=txtRestaurantServiceServiceDate.ClientID %>").val(GetStringFromDateTime(result.ServiceDate));

            $("#<%=txtRestaurantGuestServiceVatAmount.ClientID %>").val(result.VatAmount);
            $("#<%=txtRestaurantGuestServiceServiceCharge.ClientID %>").val(result.ServiceCharge);
            $("#<%=txtRestaurantGuestServiceVatAmount.ClientID %>").attr('readonly', true);
            $("#<%=txtRestaurantGuestServiceVatAmountPercent.ClientID %>").val(result.VatAmountPercent);
            $("#<%=txtRestaurantGuestServiceServiceChargePercent.ClientID %>").val(result.ServiceChargePercent);
            $("#<%=txtRestaurantGuestServiceCalculatedPercentAmount.ClientID %>").val(result.CalculatedPercentAmount);


            $('#RestaurantServiceApprovedDiv').show("slow");
            if (result.ApprovedId > 0) {
                $('#RestaurantServiceBillDiv').hide("slow");
                $('#RestaurantServiceBillApprovedDiv').show("slow");
            }
            else {
                $('#RestaurantServiceBillDiv').show("slow");
                $('#RestaurantServiceBillApprovedDiv').hide("slow");
            }
        }

        function OnFillRestaurantServiceFormObjectFailed(error) {
            toastr.error(error);
        }

        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=lblMessage.ClientID %>").text('');
            $("#<%=txtRegistrationId.ClientID %>").val('');
            $("#<%=txtRoomIdHiddenField.ClientID %>").val('');
            $("#<%=txtRoomNumber.ClientID %>").val('');
            $("#<%=txtGuestName.ClientID %>").val('');
            $("#<%=txtRoomType.ClientID %>").val('');
            $("#<%=txtPreviousRoomRate.ClientID %>").val('');
            $("#<%= txtBPPercentAmount.ClientID %>").val('0');
            $("#<%=txtVatAmount.ClientID %>").val('0');
            $("#<%=txtServiceCharge.ClientID %>").val('0');
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }

        function PerformServiceClearAction() {
            $("#<%=lblMessage.ClientID %>").text('');
            var date = new Date();
            $("#<%=txtServiceDate.ClientID %>").val(GetStringFromDateTime(date));
            $("#<%=txtServiceRate.ClientID %>").val('0');
            $("#<%=txtServiceName.ClientID %>").val('');
            $("#<%=txtServiceRoomNumber.ClientID %>").val('');
            $("#<%=txtServiceQuantity.ClientID %>").val('0');
            $("#<%=txtDiscountAmount.ClientID %>").val('0');
            $("#<%=txtServiceBillId.ClientID %>").val('');
            //$("#<%=btnSave.ClientID %>").val("Save");
            $("#<%=txtServiceRegistrationNumber.ClientID %>").val('');
            $("#<%=txtServiceServiceName.ClientID %>").val('');
            $("#<%=txtServiceServiceDate.ClientID %>").val(GetStringFromDateTime(date));
            return false;
        }

        function PerformRestaurantServiceClearAction() {
            $("#<%=lblMessage.ClientID %>").text('');
            var date = new Date();
            $("#<%=txtRestaurantServiceDate.ClientID %>").val(GetStringFromDateTime(date));
            $("#<%=txtRestaurantServiceRate.ClientID %>").val('0');
            $("#<%=txtRestaurantServiceName.ClientID %>").val('');
            $("#<%=txtRestaurantServiceRoomNumber.ClientID %>").val('');
            $("#<%=txtRestaurantServiceQuantity.ClientID %>").val('0');
            $("#<%=txtRestaurantDiscountAmount.ClientID %>").val('0');
            $("#<%=txtRestaurantServiceBillId.ClientID %>").val('');
            //$("#<%=btnSave.ClientID %>").val("Save");
            $("#<%=txtRestaurantServiceRegistrationNumber.ClientID %>").val('');
            $("#<%=txtRestaurantServiceServiceName.ClientID %>").val('');
            $("#<%=txtRestaurantServiceServiceDate.ClientID %>").val(GetStringFromDateTime(date));
            return false;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }

        function EntryServicePanelVisibleTrue() {
            $('#ServiceApprovedDiv').show("slow");
            return false;
        }
        function EntryServicePanelVisibleFalse() {
            $('#ServiceApprovedDiv').hide("slow");
            PerformServiceClearAction();
            return false;
        }
        function EntryRestaurantServicePanelVisibleFalse() {
            $('#RestaurantServiceApprovedDiv').hide("slow");
            PerformRestaurantServiceClearAction();
            return false;
        }

        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }

        function OpenUpdatePanel() {
            $('#ServiceApprovedDiv').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }

        function ConfirmApproveMessege() {
            var approvedDate = $("#<%=txtApprovedDate.ClientID %>").val();
            //var today = GetDateTimeFromString(approvedDate);
            //var dateString = GetStringFromDateTime(today);
            var answer = confirm("Do you want to Approve Night Audit for the date " + "'" + approvedDate + "' ?")
            if (answer) {
                return true;
            }
            else {
                return false;
            }
        }


        function ConfirmApproveMessegeForUpdate() {
            var approvedDate = $("#<%=txtApprovedDate.ClientID %>").val();
            var today = GetDateTimeFromString(approvedDate);
            var dateString = GetStringFromDateTime(today);
            var answer = confirm("Do you want to Update Approved Night Audit data for the date " + "'" + dateString + "' ?")
            if (answer) {
                return true;
            }
            else {
                return false;
            }
        }

        function CalculateRoomRateInclusively(cbServiceCharge, cbCityCharge, cbVatAmount, cbAdditionalCharge) {
            $("#ContentPlaceHolder1_cbCalculateServiceCharge").prop("checked", cbCityCharge);
            $("#ContentPlaceHolder1_cbCalculateCityCharge").prop("checked", cbCityCharge);
            $("#ContentPlaceHolder1_cbCalculateVatCharge").prop("checked", cbVatAmount);
            $("#ContentPlaceHolder1_cbCalculateAdditionalCharge").prop("checked", cbAdditionalCharge);

            if (TotalRoomRateGlobalValue == 0) {
                TotalRoomRateGlobalValue = parseFloat($("#ContentPlaceHolder1_txtCalculatedTotalRoomRate").val());
            }
            CalculateRateInclusively("");
            $("#CalculateRackRateInclusivelyDialog").dialog({
                width: 935,
                height: 250,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Calculate Rate Inclusively",
                show: 'slide',
                open: function (event, ui) {
                    $('#CalculateRackRateInclusivelyDialog').css('overflow', 'hidden');
                }
            });
        }
        function CalculateRateInclusively(control) {

            var txtUnitPrice = '<%=txtCalculatedTotalRoomRate.ClientID%>'
            var txtRoomRate = '<%=txtRoomRate.ClientID%>'
            var cbServiceCharge = '<%=cbCalculateServiceCharge.ClientID%>'
            var cbCityCharge = '<%=cbCalculateCityCharge.ClientID%>'
            var cbVatAmount = '<%=cbCalculateVatCharge.ClientID%>'
            var cbAdditionalCharge = '<%=cbCalculateAdditionalCharge.ClientID%>'

            var inclusiveBill = 1, Vat = 0.00, ServiceCharge = 0.00, cityCharge = 0.00, additionalCharge = 0.00;
            var additionalChargeType = "Fixed", isRatePlusPlus = 1, isVatEnableOnGuestHouseCityCharge = 0;
            var cbVatAmountVal = 1, cbServiceChargeVal = 1, cbCityChargeVal = 1, cbAdditionalChargeVal = 1;
            var isDiscountApplicableOnRackRate = 0;

            if ($("#ContentPlaceHolder1_hfIsRatePlusPlus").val() != "")
            { isRatePlusPlus = parseInt($("#ContentPlaceHolder1_hfIsRatePlusPlus").val(), 10); }

            if ($("#<%=hfGuestHouseVat.ClientID %>").val() != "")
                Vat = parseFloat($("#<%=hfGuestHouseVat.ClientID %>").val());

            if ($("#<%=hfGuestHouseServiceCharge.ClientID %>").val() != "")
                ServiceCharge = parseFloat($("#<%=hfGuestHouseServiceCharge.ClientID %>").val());

            if ($("#<%=hfCityCharge.ClientID %>").val() != "")
                cityCharge = parseFloat($("#<%=hfCityCharge.ClientID %>").val());

            if ($("#<%=hfAdditionalCharge.ClientID %>").val() != "")
                additionalCharge = parseFloat($("#<%=hfAdditionalCharge.ClientID %>").val());


            if ($("#<%=hfAdditionalChargeType.ClientID %>").val() != "")
                additionalChargeType = $("#<%=hfAdditionalChargeType.ClientID %>").val();

            if ($("#<%=hfIsVatEnableOnGuestHouseCityCharge.ClientID %>").val() != "")
                isVatEnableOnGuestHouseCityCharge = parseInt($("#<%=hfIsVatEnableOnGuestHouseCityCharge.ClientID %>").val(), 10);

            if ($('#' + cbServiceCharge).is(':checked')) {
                cbServiceChargeVal = 1;
            }
            else {
                cbServiceChargeVal = 0;
                ServiceCharge = 0.00;
            }

            if ($('#' + cbCityCharge).is(':checked')) {
                cbCityChargeVal = 1;
            }
            else {
                cbCityChargeVal = 0;
                cityCharge = 0.00;
            }

            if ($('#' + cbVatAmount).is(':checked')) {
                cbVatAmountVal = 1;
            }
            else {
                cbVatAmountVal = 0;
                Vat = 0.00;
            }

            if ($('#' + cbAdditionalCharge).is(':checked')) {
                cbAdditionalChargeVal = 1;
            }
            else {
                cbAdditionalChargeVal = 0;
                additionalCharge = 0.00;
                additionalChargeType = "Percentage";
            }

            var txtRoomRateVal = parseFloat($('#' + txtUnitPrice).val());

            if ($("#<%=hfIsDiscountApplicableOnRackRate.ClientID %>").val() != "") {
                isDiscountApplicableOnRackRate = parseInt($("#<%=hfIsDiscountApplicableOnRackRate.ClientID %>").val(), 10);
            }

            var unitPrice = parseFloat($("#ContentPlaceHolder1_txtUnitPrice").val());
            var discountType = "";
            var discountAmount = 0;
            
            var RoomRateGlobal = CommonHelper.GetRackRateServiceChargeVatInformation(unitPrice, ServiceCharge, cityCharge,
                                                Vat, additionalCharge, additionalChargeType, 0, isRatePlusPlus, isVatEnableOnGuestHouseCityCharge,
                                                parseInt(cbVatAmountVal, 10), parseInt(cbServiceChargeVal, 10), parseInt(cbCityChargeVal, 10),
                                                parseInt(cbAdditionalChargeVal, 10), isDiscountApplicableOnRackRate, 'Fixed', 0.00);

            txtRoomRateVal = RoomRateGlobal.CalculatedAmount;  //parseFloat($('#' + txtUnitPrice).val());

            discountType = "Fixed";
            discountAmount = RoomRateGlobal.CalculatedAmount - parseFloat($('#' + txtUnitPrice).val());

            //toastr.info("unitPrice: "+ unitPrice + ", RoomRateGlobal.CalculatedAmount: " + RoomRateGlobal.CalculatedAmount + "discountAmount: " + discountAmount + "txtRoomRateVal :" + txtRoomRateVal);

            var RoomRate = CommonHelper.GetRackRateServiceChargeVatInformation(txtRoomRateVal, ServiceCharge, cityCharge,
                                            Vat, additionalCharge, additionalChargeType, inclusiveBill, isRatePlusPlus, isVatEnableOnGuestHouseCityCharge,
                                            parseInt(cbVatAmountVal, 10), parseInt(cbServiceChargeVal, 10), parseInt(cbCityChargeVal, 10),
                                            parseInt(cbAdditionalChargeVal, 10), isDiscountApplicableOnRackRate, discountType, discountAmount);

            //ab = RoomRate;

            if (RoomRate.RackRate > 0) {
                $("#ContentPlaceHolder1_txtCalculateServiceCharge").val(toFixed(RoomRate.ServiceCharge, 2));
                $("#ContentPlaceHolder1_txtCalculateVatCharge").val(toFixed(RoomRate.VatAmount, 2));
                $("#ContentPlaceHolder1_txtCalculateCityCharge").val(toFixed(RoomRate.SDCityCharge, 2));
                $("#ContentPlaceHolder1_txtCalculateAdditionalCharge").val(toFixed(RoomRate.AdditionalCharge, 2));
                $("#ContentPlaceHolder1_txtCalculateRackRate").val(RoomRate.RackRate);
                $("#ContentPlaceHolder1_txtCalculateDiscountAmount").val(RoomRate.DiscountAmount);
            }
            else {
                $("#ContentPlaceHolder1_txtCalculateServiceCharge").val('0');
                $("#ContentPlaceHolder1_txtCalculateVatCharge").val('0');
                $("#ContentPlaceHolder1_txtCalculateCityCharge").val('0');
                $("#ContentPlaceHolder1_txtCalculateAdditionalCharge").val('0');
                $("#ContentPlaceHolder1_txtCalculateDiscountAmount").val('0');
            }
        }
        function ApplyCalculateCharges() {

            var txtCalculatedTotalRoomRateData = $("#ContentPlaceHolder1_txtCalculatedTotalRoomRate").val();
            var txtCalculateRackRateData = $("#ContentPlaceHolder1_txtCalculateRackRate").val();
            var txtCalculateDiscountAmountData = $("#ContentPlaceHolder1_txtCalculateDiscountAmount").val();

            var cbCalculateServiceChargeData = $("#ContentPlaceHolder1_cbCalculateServiceCharge").is(":checked");
            var cbCalculateCityChargeData = $("#ContentPlaceHolder1_cbCalculateCityCharge").is(":checked");
            var cbCalculateVatChargeData = $("#ContentPlaceHolder1_cbCalculateVatCharge").is(":checked");
            var cbCalculateAdditionalChargeData = $("#ContentPlaceHolder1_cbCalculateAdditionalCharge").is(":checked");

            
            var registrationId = $("#<%=hfRegistrationId.ClientID %>").val();
            var approvedId = $("#<%=hfApprovedId.ClientID %>").val();
            var serviceType = $("#<%=hfApprovedServiceType.ClientID %>").val();
            PageMethods.ApprovedNightAuditedData(serviceType, registrationId, approvedId, txtCalculatedTotalRoomRateData, txtCalculateRackRateData, txtCalculateDiscountAmountData, cbCalculateServiceChargeData, cbCalculateCityChargeData, cbCalculateVatChargeData, cbCalculateAdditionalChargeData, OnApplyCalculateChargesSucceeded, OnApplyCalculateChargesFailed);
            return false;
            

            //var originalDiscount = toFixed((parseFloat($("#ContentPlaceHolder1_txtUnitPrice").val()) - parseFloat($("#ContentPlaceHolder1_txtCalculateRackRate").val())), 2);

            //$("#ContentPlaceHolder1_txtServiceCharge").val($("#ContentPlaceHolder1_txtCalculateServiceCharge").val());
            //$("#ContentPlaceHolder1_txtCityCharge").val($("#ContentPlaceHolder1_txtCalculateCityCharge").val());
            //$("#ContentPlaceHolder1_txtVatAmount").val($("#ContentPlaceHolder1_txtCalculateVatCharge").val());
            //$("#ContentPlaceHolder1_txtAdditionalCharge").val($("#ContentPlaceHolder1_txtCalculateAdditionalCharge").val());

            //$("#ContentPlaceHolder1_txtDiscountAmount").val(originalDiscount);
            //$("#ContentPlaceHolder1_ddlDiscountType").val("Fixed");

            //$("#ContentPlaceHolder1_txtTotalRoomRate").val($("#ContentPlaceHolder1_txtCalculatedTotalRoomRate").val());
            //$("#ContentPlaceHolder1_txtRoomRate").val($("#ContentPlaceHolder1_txtCalculateRackRate").val());
            //$("#CalculateRackRateInclusivelyDialog").dialog("close");

            //$("#ContentPlaceHolder1_cbServiceCharge").prop("checked", $("#ContentPlaceHolder1_cbCalculateServiceCharge").is(":checked"));
            //$("#ContentPlaceHolder1_cbCityCharge").prop("checked", $("#ContentPlaceHolder1_cbCalculateCityCharge").is(":checked"));
            //$("#ContentPlaceHolder1_cbVatAmount").prop("checked", $("#ContentPlaceHolder1_cbCalculateVatCharge").is(":checked"));
            //$("#ContentPlaceHolder1_cbAdditionalCharge").prop("checked", $("#ContentPlaceHolder1_cbCalculateAdditionalCharge").is(":checked"));
            //// //CalculateDiscount();
        }
        function OnApplyCalculateChargesSucceeded(result)
        {
            $("#CalculateRackRateInclusivelyDialog").dialog("close");
            toastr.info(result);
        }
        function OnApplyCalculateChargesFailed(error) {
            toastr.error(error);
        }
    </script>
    <!--Start Calculate Rack Rate Inclusively PopUp -->
    <asp:HiddenField ID="hfApprovedServiceType" runat="server" />
    <asp:HiddenField ID="hfRegistrationId" runat="server" />
    <asp:HiddenField ID="hfApprovedId" runat="server" />
    <asp:HiddenField ID="hfIsRatePlusPlus" runat="server" />
    <asp:HiddenField ID="hfGuestHouseVat" runat="server" />
    <asp:HiddenField ID="hfGuestHouseServiceCharge" runat="server" />
    <asp:HiddenField ID="hfCityCharge" runat="server" />
    <asp:HiddenField ID="hfAdditionalCharge" runat="server" />
    <asp:HiddenField ID="hfAdditionalChargeType" runat="server" />
    <asp:HiddenField ID="hfIsVatEnableOnGuestHouseCityCharge" runat="server" />
    <asp:HiddenField ID="hfIsDiscountApplicableOnRackRate" runat="server" />

    <div id="CalculateRackRateInclusivelyDialog" style="display: none;">

        <div class="form-horizontal">
            <div class="form-group">
                <label class="control-label col-md-2 required-field">
                    Total Room Rate</label>
                <div class="col-md-4">
                    <asp:TextBox ID="txtCalculatedTotalRoomRate" runat="server" CssClass="form-control" TabIndex="22"
                        onblur="CalculateRateInclusively(this)"></asp:TextBox>
                </div>
                <label class="control-label col-md-2 required-field">
                    Rack Rate</label>
                <div class="col-md-4">
                    <asp:HiddenField ID="txtUnitPrice" runat="server" />
                    <asp:TextBox ID="txtCalculateRackRate" runat="server" CssClass="form-control quantitydecimal" TabIndex="23" Enabled="false"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-2" style="text-align: right;">
                    <asp:Label ID="Label1" runat="server" CssClass="control-label required-field" Text="Service Charge"></asp:Label>
                </div>
                <div class="col-md-4">
                    <div class="input-group">
                        <asp:TextBox ID="txtCalculateServiceCharge" runat="server" TabIndex="22" CssClass="form-control"
                            Enabled="false"></asp:TextBox>
                        <span class="input-group-addon">
                            <asp:CheckBox ID="cbCalculateServiceCharge" runat="server" Text="" CssClass="customChkBox"
                                onclick="javascript: return CalculateRateInclusively(this);"
                                TabIndex="8" Checked="True" />
                        </span>
                    </div>
                </div>
                <div class="col-md-2" style="text-align: right;">
                    <asp:Label ID="Label3" runat="server" CssClass="control-label required-field" Text="City/SD Charge"></asp:Label>
                </div>
                <div class="col-md-4">
                    <div class="input-group">
                        <asp:TextBox ID="txtCalculateCityCharge" runat="server" TabIndex="22" CssClass="form-control"
                            Enabled="false"></asp:TextBox>
                        <span class="input-group-addon">
                            <asp:CheckBox ID="cbCalculateCityCharge" runat="server" Text="" CssClass="customChkBox"
                                onclick="javascript: return CalculateRateInclusively(this);"
                                TabIndex="8" Checked="True" />
                        </span>
                    </div>
                </div>

            </div>
            <div class="form-group">
                <div class="col-md-2" style="text-align: right;">
                    <asp:Label ID="Label2" runat="server" CssClass="control-label required-field" Text="Vat Amount"></asp:Label>
                </div>
                <div class="col-md-4">
                    <div class="input-group">
                        <asp:TextBox ID="txtCalculateVatCharge" runat="server" TabIndex="23" CssClass="form-control"
                            Enabled="false"></asp:TextBox>
                        <span class="input-group-addon">
                            <asp:CheckBox ID="cbCalculateVatCharge" runat="server" Text="" CssClass="customChkBox"
                                onclick="javascript: return CalculateRateInclusively(this);"
                                TabIndex="8" Checked="True" />
                        </span>
                    </div>
                </div>
                <div class="col-md-2" style="text-align: right;">
                    <asp:Label ID="Label4" runat="server" CssClass="control-label required-field" Text="Additional Charge"></asp:Label>
                </div>
                <div class="col-md-4">
                    <div class="input-group">
                        <asp:TextBox ID="txtCalculateAdditionalCharge" runat="server" TabIndex="22" CssClass="form-control"
                            Enabled="false"></asp:TextBox>
                        <span class="input-group-addon">
                            <asp:CheckBox ID="cbCalculateAdditionalCharge" runat="server" Text="" CssClass="customChkBox"
                                onclick="javascript: return CalculateRateInclusively(this);"
                                TabIndex="8" Checked="True" />
                        </span>
                    </div>

                </div>
            </div>

            <div class="form-group" style="display:none;">
                <%--<label for="DiscountType" class="control-label col-md-2">
                    Discount Type</label>
                <div class="col-md-4">
                    <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control" TabIndex="20">
                        <asp:ListItem>Fixed</asp:ListItem>
                        <asp:ListItem>Percentage</asp:ListItem>
                    </asp:DropDownList>
                </div>--%>
                <label for="DiscountAmount" class="control-label col-md-2">
                    Discount Amount</label>
                <div class="col-md-4">
                    <asp:TextBox ID="txtCalculateDiscountAmount" runat="server" CssClass="form-control quantitydecimal" Enabled="false" TabIndex="21"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-9"></div>
                <div class="col-md-2">
                    <input type="button" id="btnCalculatedDiscountOkey" value="Apply Rate"
                        class="col-sm-12 btn btn-primary btn-large" onclick="ApplyCalculateCharges()" />
                </div>
            </div>
        </div>
    </div>
    <!--End Calculate Rack Rate Inclusively PopUp -->
    <div id="MessageBox" class="alert alert-info" style="display: none">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <%--  <div style="height: 5px">
    </div>--%>
    <asp:HiddenField ID="hfNightAuditInformation" runat="server" />
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <label for="AuditDate" class="control-label col-md-2 required-field">
                        Audit Date</label>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtApprovedDate" runat="server" CssClass="form-control"
                            TabIndex="1"></asp:TextBox>
                    </div>
                    <div id="SrcRoomInformationDiv">
                        <label for="RoomNumber" class="control-label col-md-2">
                            Room Number</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSrcRoomNumber" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" TabIndex="3" runat="server" Text="Search Audit Information"
                            CssClass="btn btn-primary btn-sm" OnClick="btnSearch_Click" />
                    </div>
                </div>
            </div>
        </div>
        <div id="myTabs">
            <ul id="tabPage" class="ui-style">
                <li id="ARoomAudit" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none; display: none"><a href="#tab-1">Room Audit</a></li>
                <li id="BServiceAudit" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none; display: none"><a href="#tab-2">Service Audit </a></li>
                <li id="CRestaurantAudit" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none; display: none"><a href="#tab-3">Restaurant Audit </a></li>
            </ul>
            <div id="tab-1">
                <div id="EntryPanel" class="panel panel-default" style="display: none;">
                    <div class="panel-heading">
                        Approve Information
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label for="GuestName" class="control-label col-md-2">
                                    Guest Name</label>
                                <div class="col-md-10">
                                    <asp:HiddenField ID="ApprovedIdHiddenField" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="txtRegistrationId" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="txtRoomIdHiddenField" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="txtBPPercentAmount" runat="server"></asp:HiddenField>
                                    <asp:TextBox ID="txtGuestName" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="RoomNumber" class="control-label col-md-2">
                                    Room Number</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtRoomNumber" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                                </div>
                                <label for="RoomType" class="control-label col-md-2">
                                    Room Type</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtRoomType" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="TotalRoomRate" class="control-label col-md-2">
                                    Total Room Rate</label>
                                <div class="col-md-4">
                                    <asp:Label ID="lblPreviousRoomRate" runat="server" class="control-label" Text="Unit Price"
                                        Visible="false"></asp:Label>
                                    <asp:TextBox ID="txtTotalRoomRate" runat="server" CssClass="form-control quantitydecimal" TabIndex="7"></asp:TextBox>
                                    <asp:HiddenField ID="hfIsBillInclusive" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hfGuestServiceChargeRate" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hfGuestVatAmountRate" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hfInvoiceServiceCharge" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hfIsVatAmountEnable" runat="server"></asp:HiddenField>
                                    <asp:TextBox ID="txtPreviousRoomRate" runat="server" Visible="false" CssClass="form-control"
                                        TabIndex="7"></asp:TextBox>
                                </div>
                                <label for="RoomRate" class="control-label col-md-2">
                                    Room Rate</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtRoomRate" runat="server" CssClass="form-control" TabIndex="8"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="VatAmount" class="control-label col-md-2">
                                    Vat Amount</label>
                                <div class="col-md-4">
                                    <asp:HiddenField ID="txtVatAmountPercent" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="txtServiceChargePercent" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="txtReferenceSalesCommissionPercent" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="txtCalculatedPercentAmount" runat="server"></asp:HiddenField>
                                    <asp:TextBox ID="txtVatAmount" runat="server" CssClass="form-control" TabIndex="9"></asp:TextBox>
                                </div>
                                <label for="ServiceCharge" class="control-label col-md-2">
                                    Service Charge</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtServiceCharge" runat="server" CssClass="form-control" TabIndex="10"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="RefSalesCommission" class="control-label col-md-2">
                                    Ref. Sales Commission</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtReferenceSalesCommission" runat="server" CssClass="form-control quantitydecimal"
                                        TabIndex="9"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div id="btnSaveButtonDiv">
                                        <asp:Button ID="btnSave" runat="server" Text="Approve" TabIndex="9" CssClass="TransactionalButton btn btn-primary btn-sm"
                                            OnClick="btnSave_Click" OnClientClick="return ConfirmApproveMessege();" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Close" TabIndex="10" CssClass="btn btn-primary btn-sm"
                                            OnClientClick="javascript: return EntryPanelVisibleFalse();" OnClick="btnCancel_Click" />
                                    </div>
                                    <div id="btnUpdateRoomApprovedDataButtonDiv">
                                        <asp:Button ID="btnUpdateRoomApprovedData" runat="server" Text="Update" TabIndex="9"
                                            CssClass="TransactionalButton btn btn-primary btn-sm" OnClick="btnUpdateRoomApprovedData_Click" OnClientClick="return ConfirmApproveMessegeForUpdate();" />
                                        <asp:Button ID="btnCancelRoomApproved" runat="server" Text="Close" TabIndex="10"
                                            CssClass="btn btn-primary btn-sm" OnClientClick="javascript: return EntryPanelVisibleFalse();"
                                            OnClick="btnCancel_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="RoomAuditPanel" class="panel panel-default">
                    <div class="panel-heading">
                        Room Audit Information
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div>
                                <asp:GridView ID="gvAvailableGuestList" Width="100%" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                    ForeColor="#333333" PageSize="500" OnPageIndexChanging="gvAvailableGuestList_PageIndexChanging"
                                    OnRowDataBound="gvAvailableGuestList_RowDataBound" OnRowCommand="gvAvailableGuestList_RowCommand"
                                    CssClass="table table-bordered table-condensed table-responsive">
                                    <RowStyle BackColor="#E3EAEB" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="ApprovedId" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblApprovedId" runat="server" Text='<%#Eval("ApprovedId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblid" runat="server" Text='<%#Eval("RegistrationId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IDApprovedStatus" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblApprovedStatus" runat="server" Text='<%#Eval("ApprovedStatus") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IDIsGuestCheckedOut" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIsGuestCheckedOut" runat="server" Text='<%#Eval("IsGuestCheckedOut") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-Width="05%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBoxAccept" Checked="true" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RoomId" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRoomId" runat="server" Text='<%#Eval("RoomId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Room No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvRoomNumber" runat="server" Text='<%# Bind("RoomNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Guest Name" ItemStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvGuestName" runat="server" Text='<%# Bind("GuestName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service" ItemStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvServiceName" runat="server" Text='<%# Bind("ServiceName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Room Type" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvRoomType" runat="server" Text='<%# Bind("RoomType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Room Tariff">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvRoomRate" runat="server" Text='<%# Bind("RoomRate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Percent Amount" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvBPPercentAmount" runat="server" Text='<%# Bind("BPPercentAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount Amount" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvBPDiscountAmount" runat="server" Text='<%# Bind("BPDiscountAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvDisplayBPDiscountAmount" runat="server" Text=''></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Calculated Amount" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvCalculatedRoomRateAmount" runat="server" Text='<%# Bind("CalculatedRoomRate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="S. Charge">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvServiceCharge" runat="server" Text='<%# Bind("ServiceCharge") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="City Charge">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvCitySDCharge" runat="server" Text='<%# Bind("CitySDCharge") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vat Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvVatAmount" runat="server" Text='<%# Bind("VatAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Additional">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvAdditionalChargee" runat="server" Text='<%# Bind("AdditionalCharge") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sales Commission" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvReferenceSalesCommission" runat="server" Text='<%# Bind("ReferenceSalesCommission") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="VatAmountPercent" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvVatAmountPercent" runat="server" Text='<%# Bind("VatAmountPercent") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ServiceChargePercent" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvServiceChargePercent" runat="server" Text='<%# Bind("ServiceChargePercent") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CalculatedPercentAmount" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvCalculatedPercentAmount" runat="server" Text='<%# Bind("CalculatedPercentAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvTotalCalculatedAmount" runat="server" Text='<%# Bind("TotalCalculatedAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="USD Total" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvTotalCalculatedUsdAmount" runat="server" Text='<%# Bind("TotalCalculatedUsdAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                                    ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                                &nbsp;<asp:ImageButton ID="ImgApprove" runat="server" CausesValidation="False" CommandName="CmdApprove"
                                                    CommandArgument='<%# bind("RegistrationId") %>' ImageUrl="~/Images/save.png"
                                                    Text="" AlternateText="Approve" ToolTip="Approve" OnClientClick="return confirm('Do you want to Approve for entered Approve date?');" />
                                                &nbsp;<asp:ImageButton ID="ImgApproved" runat="server" CausesValidation="False" CommandName="CmdApproved"
                                                    ImageUrl="~/Images/select.png" Text="" AlternateText="Approved" ToolTip="Approved"
                                                    Enabled="False" />
                                                &nbsp;<asp:ImageButton ID="ImgEdit" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                                    ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                                &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                                    CommandArgument='<%# bind("RegistrationId") %>' ImageUrl="~/Images/delete.png"
                                                    Text="" AlternateText="Cancel" ToolTip="Cancel" OnClientClick="return confirm('Do you want to Cancel Night Audit?');" />
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
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btnSaveAll" runat="server" Text="Approve All" OnClientClick="return ConfirmApproveMessege();"
                                        CssClass="btn btn-primary btn-sm" OnClick="btnSaveAll_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="tab-2">
                <div id="Div1" class="panel panel-default">
                    <div id="ServiceApprovedDiv" class="panel panel-default" style="display: none;">
                        <div class="panel-heading">
                            Approve Information
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label for="RoomNumber" class="control-label col-md-2">
                                        Room Number</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtServiceRoomNumber" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </div>
                                    <label for="RegistrationNumber" class="control-label col-md-2">
                                        Registration Number</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtRegistrationNumber" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="Service" class="control-label col-md-2">
                                        Service</label>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtServiceName" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="ServiceDate" class="control-label col-md-2">
                                        Service Date</label>
                                    <div class="col-md-4">
                                        <asp:HiddenField ID="ServiceApprovedIdHiddenField" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="txtServiceBillId" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="txtServiceRegistrationId" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="txtServiceServiceId" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="txtServiceRegistrationNumber" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="txtServiceType" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="txtServiceServiceName" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="txtServiceServiceDate" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="txtPaymentMode" runat="server"></asp:HiddenField>
                                        <asp:TextBox ID="txtServiceDate" runat="server" CssClass="form-control" TabIndex="7"
                                            Enabled="false"></asp:TextBox>
                                    </div>
                                    <label for="ServiceRate" class="control-label col-md-2">
                                        Service Rate</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtServiceRate" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="VatAmount" class="control-label col-md-2">
                                        Vat Amount</label>
                                    <div class="col-md-4">
                                        <asp:HiddenField ID="txtGuestServiceVatAmountPercent" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="txtGuestServiceServiceChargePercent" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="txtGuestServiceCalculatedPercentAmount" runat="server"></asp:HiddenField>
                                        <asp:TextBox ID="txtGuestServiceVatAmount" runat="server" CssClass="form-control"
                                            TabIndex="9"></asp:TextBox>
                                    </div>
                                    <label for="Quantity" class="control-label col-md-2">
                                        Quantity</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtServiceQuantity" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="Service Charge" class="control-label col-md-2">
                                        Service Charge</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtGuestServiceServiceCharge" runat="server" CssClass="form-control"
                                            TabIndex="10"></asp:TextBox>
                                    </div>
                                    <label for="DiscountAmount" class="control-label col-md-2">
                                        Discount Amount</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtDiscountAmount" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="ServiceBillDiv">
                                            <asp:Button ID="btnServiceApproved" runat="server" Text="Approve" OnClientClick="return ConfirmApproveMessege();"
                                                TabIndex="9" CssClass="btn btn-primary btn-sm" OnClick="btnServiceApproved_Click" />
                                            <asp:Button ID="btnCloseServiceAudit" runat="server" Text="Close" TabIndex="10" CssClass="btn btn-primary btn-sm"
                                                OnClientClick="javascript: return EntryServicePanelVisibleFalse();" OnClick="btnCancel_Click" />
                                        </div>
                                        <div id="ServiceBillApprovedDiv">
                                            <asp:Button ID="btnUpdateApprovedService" runat="server" Text="Update" OnClientClick="return ConfirmApproveMessege();"
                                                TabIndex="9" CssClass="btn btn-primary btn-sm" OnClick="btnUpdateApprovedService_Click" />
                                            <asp:Button ID="btnCanceApprovedService" runat="server" Text="Close" TabIndex="10"
                                                CssClass="btn btn-primary btn-sm" OnClientClick="javascript: return EntryServicePanelVisibleFalse();"
                                                OnClick="btnCancel_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel-heading">
                        Service Details Information
                    </div>
                    <div class="panel-body">
                        <asp:GridView ID="gvGHServiceBill" Width="100%" runat="server" AllowPaging="True"
                            AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                            ForeColor="#333333" TabIndex="9" OnPageIndexChanging="gvGHServiceBill_PageIndexChanging"
                            OnRowDataBound="gvGHServiceBill_RowDataBound" OnRowCommand="gvGHServiceBill_RowCommand"
                            PageSize="500" CssClass="table table-bordered table-condensed table-responsive">
                            <RowStyle BackColor="#E3EAEB" />
                            <Columns>
                                <asp:TemplateField HeaderText="ApprovedId" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovedId" runat="server" Text='<%#Eval("ApprovedId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="IDNO" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblid" runat="server" Text='<%#Eval("ServiceBillId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RegiId" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegiId" runat="server" Text='<%#Eval("RegistrationId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ServiceApprovedStatus" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceApprovedStatus" runat="server" Text='<%#Eval("ApprovedStatus") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ServiceType" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceType" runat="server" Text='<%#Eval("ServiceType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ServiceId" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" ItemStyle-Width="05%">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBoxAccept" Checked="true" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvServiceDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ServiceDate"))) %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Room" ItemStyle-Width="6%" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRoomNumber" runat="server" Text='<%#Eval("RoomNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bill No." ItemStyle-Width="6%" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillNumber" runat="server" Text='<%#Eval("BillNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Service Name" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblServiceQuantity" runat="server" Text='<%#Eval("ServiceQuantity") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rate" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceRate" runat="server" Text='<%#Eval("ServiceRate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Disc." ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Left" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiscountAmount" runat="server" Text='<%#Eval("DiscountAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                <asp:TemplateField HeaderText="S. Charge" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvServiceCharge" runat="server" Text='<%# Bind("ServiceCharge") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SD. Charge" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvCitySDCharge" runat="server" Text='<%# Bind("CitySDCharge") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vat" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvVatAmount" runat="server" Text='<%# Bind("VatAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Additional" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvAdditionalCharge" runat="server" Text='<%# Bind("AdditionalCharge") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgvTotalCalculatedAmount" runat="server" Text='<%# Bind("TotalCalculatedAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="VatAmountPercent" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvVatAmountPercent" runat="server" Text='<%# Bind("VatAmountPercent") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ServiceChargePercent" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvServiceChargePercent" runat="server" Text='<%# Bind("ServiceChargePercent") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CalculatedPercentAmount" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvCalculatedPercentAmount" runat="server" Text='<%# Bind("CalculatedPercentAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                            ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                        &nbsp;<asp:ImageButton ID="ImgApprove" runat="server" CausesValidation="False" CommandName="CmdApprove"
                                            CommandArgument='<%# bind("ServiceBillId") %>' ImageUrl="~/Images/save.png" Text=""
                                            AlternateText="Approve" ToolTip="Approve" OnClientClick="return ConfirmApproveMessege();" />
                                        &nbsp;<asp:ImageButton ID="ImgApproved" runat="server" CausesValidation="False" CommandName="CmdApproved"
                                            ImageUrl="~/Images/select.png" Text="" AlternateText="Approved" ToolTip="Approved"
                                            Enabled="False" />
                                        &nbsp;<asp:ImageButton ID="ImgEdit" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                            ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                        &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                            CommandArgument='<%# bind("ServiceBillId") %>' ImageUrl="~/Images/delete.png"
                                            Text="" AlternateText="Cancel" ToolTip="Cancel" OnClientClick="return confirm('Do you want to Cancel Night Audit?');" />
                                        &nbsp;<asp:ImageButton ID="ImgBillPreview" runat="server" CausesValidation="False"
                                                CommandArgument='<%# bind("ServiceBillId") %>' CommandName="CmdPreview" ImageUrl="~/Images/ReportDocument.png"
                                                Text="" AlternateText="Preview" ToolTip="Preview" />
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
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnAllServiceBillApprove" runat="server" Text="Approve All" OnClientClick="return ConfirmApproveMessege();"
                                CssClass="btn btn-primary btn-sm" OnClick="btnAllServiceBillApproved_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="tab-3">
                <div id="IsRestaurantIntegrateWithFrontOfficeDiv" runat="server">
                    <div id="Div2" class="panel panel-default">
                        <div id="RestaurantServiceApprovedDiv" class="panel panel-default" style="display: none;">
                            <div class="panel-heading">
                                Approve Information
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label for="RoomNumber" class="control-label col-md-2">
                                            Room Number</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtRestaurantServiceRoomNumber" runat="server" CssClass="form-control"
                                                Enabled="false"></asp:TextBox>
                                        </div>
                                        <label for="RegistrationNumber" class="control-label col-md-2">
                                            Registration Number</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtRestaurantRegistrationNumber" runat="server" CssClass="form-control"
                                                Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="Service" class="control-label col-md-2">
                                            Service</label>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="txtRestaurantServiceName" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="ServiceDate" class="control-label col-md-2">
                                            Service Date</label>
                                        <div class="col-md-4">
                                            <asp:HiddenField ID="RestaurantServiceApprovedIdHiddenField" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="txtRestaurantServiceBillId" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="txtRestaurantServiceRegistrationId" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="txtRestaurantServiceServiceId" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="txtRestaurantServiceRegistrationNumber" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="txtRestaurantServiceType" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="txtRestaurantServiceServiceName" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="txtRestaurantServiceServiceDate" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="txtRestaurantPaymentMode" runat="server"></asp:HiddenField>
                                            <asp:TextBox ID="txtRestaurantServiceDate" runat="server" CssClass="form-control"
                                                TabIndex="7" Enabled="false"></asp:TextBox>
                                        </div>
                                        <label for="ServiceRate" class="control-label col-md-2">
                                            Service Rate</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtRestaurantServiceRate" runat="server" CssClass="form-control"
                                                TabIndex="4"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="Vat Amount" class="control-label col-md-2">
                                            Vat Amount</label>
                                        <div class="col-md-4">
                                            <asp:HiddenField ID="txtRestaurantGuestServiceVatAmountPercent" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="txtRestaurantGuestServiceServiceChargePercent" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="txtRestaurantGuestServiceCalculatedPercentAmount" runat="server"></asp:HiddenField>
                                            <asp:TextBox ID="txtRestaurantGuestServiceVatAmount" runat="server" CssClass="form-control"
                                                TabIndex="9"></asp:TextBox>
                                        </div>
                                        <label for="Quantity" class="control-label col-md-2">
                                            Quantity</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtRestaurantServiceQuantity" runat="server" CssClass="form-control"
                                                TabIndex="5"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="ServiceCharge" class="control-label col-md-2">
                                            Service Charge</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtRestaurantGuestServiceServiceCharge" runat="server" CssClass="form-control"
                                                TabIndex="10"></asp:TextBox>
                                        </div>
                                        <label for="DiscountAmount" class="control-label col-md-2">
                                            Discount Amount</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtRestaurantDiscountAmount" runat="server" CssClass="form-control"
                                                TabIndex="6"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div id="RestaurantServiceBillDiv">
                                                <asp:Button ID="btnRestaurantServiceApproved" runat="server" Text="Approve" OnClientClick="return ConfirmApproveMessege();"
                                                    TabIndex="9" CssClass="btn btn-primary btn-sm" OnClick="btnRestaurantServiceApproved_Click" />
                                                <asp:Button ID="btnCloseRestaurantServiceAudit" runat="server" Text="Close" TabIndex="10"
                                                    CssClass="btn btn-primary btn-sm" OnClientClick="javascript: return EntryRestaurantServicePanelVisibleFalse();"
                                                    OnClick="btnRestaurantCancel_Click" />
                                            </div>
                                            <div id="RestaurantServiceBillApprovedDiv">
                                                <asp:Button ID="btnUpdateRestaurantApprovedService" runat="server" Text="Update"
                                                    OnClientClick="return ConfirmApproveMessege();" TabIndex="9" CssClass="btn btn-primary btn-sm"
                                                    OnClick="btnUpdateRestaurantApprovedService_Click" />
                                                <asp:Button ID="btnCanceRestaurantApprovedService" runat="server" Text="Close" TabIndex="10"
                                                    CssClass="btn btn-primary btn-sm" OnClientClick="javascript: return EntryRestaurantServicePanelVisibleFalse();"
                                                    OnClick="btnRestaurantCancel_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel-heading">
                            Restaurant Details Information
                        </div>
                        <div class="panel-body">
                            <asp:GridView ID="gvRestaurantServiceBill" Width="100%" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                ForeColor="#333333" TabIndex="9" OnPageIndexChanging="gvRestaurantServiceBill_PageIndexChanging"
                                OnRowDataBound="gvRestaurantServiceBill_RowDataBound" OnRowCommand="gvRestaurantServiceBill_RowCommand"
                                PageSize="500" CssClass="table table-bordered table-condensed table-responsive">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="ApprovedId" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblApprovedId" runat="server" Text='<%#Eval("ApprovedId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IDNO" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%#Eval("ServiceBillId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RegiId" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRegiId" runat="server" Text='<%#Eval("RegistrationId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ServiceApprovedStatus" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblServiceApprovedStatus" runat="server" Text='<%#Eval("ApprovedStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ServiceType" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblServiceType" runat="server" Text='<%#Eval("ServiceType") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ServiceId" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-Width="05%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBoxAccept" Checked="true" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvServiceDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("ServiceDate"))) %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Room" ItemStyle-Width="6%" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRoomNumber" runat="server" Text='<%#Eval("RoomNumber") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bill No." ItemStyle-Width="6%" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBillNumber" runat="server" Text='<%#Eval("BillNumber") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Service Name" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblServiceRate" runat="server" Text='<%#Eval("ServiceRate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblServiceQuantity" runat="server" Text='<%#Eval("ServiceQuantity") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Disc." ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiscountAmount" runat="server" Text='<%#Eval("DiscountAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vat" ItemStyle-Width="6%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvVatAmount" runat="server" Text='<%# Bind("VatAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="S. Charge" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvServiceCharge" runat="server" Text='<%# Bind("ServiceCharge") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvTotalCalculatedAmount" runat="server" Text='<%# Bind("TotalCalculatedAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VatAmountPercent" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvVatAmountPercent" runat="server" Text='<%# Bind("VatAmountPercent") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ServiceChargePercent" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvServiceChargePercent" runat="server" Text='<%# Bind("ServiceChargePercent") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CalculatedPercentAmount" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvCalculatedPercentAmount" runat="server" Text='<%# Bind("CalculatedPercentAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IsPaidService" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgvIsPaidService" runat="server" Text='<%# Bind("IsPaidService") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                                ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgApprove" runat="server" CausesValidation="False" CommandName="CmdApprove"
                                                CommandArgument='<%# bind("ServiceBillId") %>' ImageUrl="~/Images/save.png" Text=""
                                                AlternateText="Approve" ToolTip="Approve" OnClientClick="return ConfirmApproveMessege();" />
                                            &nbsp;<asp:ImageButton ID="ImgApproved" runat="server" CausesValidation="False" CommandName="CmdApproved"
                                                ImageUrl="~/Images/select.png" Text="" AlternateText="Approved" ToolTip="Approved"
                                                Enabled="False" />
                                            &nbsp;<asp:ImageButton ID="ImgEdit" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                                ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit" ToolTip="Edit" />
                                            &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                                CommandArgument='<%# bind("ServiceBillId") %>' ImageUrl="~/Images/delete.png"
                                                Text="" AlternateText="Cancel" ToolTip="Cancel" OnClientClick="return confirm('Do you want to Cancel Night Audit?');" />
                                            &nbsp;<asp:ImageButton ID="ImgBillPreview" runat="server" CausesValidation="False"
                                                CommandArgument='<%# bind("ServiceBillId") %>' CommandName="CmdPreview" ImageUrl="~/Images/ReportDocument.png"
                                                Text="" AlternateText="Preview" ToolTip="Preview" />
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
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnAllRestaurantServiceBillApprove" runat="server" Text="Approve All"
                                    OnClientClick="return ConfirmApproveMessege();" CssClass="btn btn-primary btn-sm"
                                    OnClick="btnAllRestaurantServiceBillApprove_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
