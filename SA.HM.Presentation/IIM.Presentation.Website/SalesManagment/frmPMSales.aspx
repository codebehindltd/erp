<%@ Page Title="" Language="C#" MasterPageFile="~/Common/HM.Master" AutoEventWireup="true"
    CodeBehind="frmPMSales.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesManagment.frmPMSales"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">


        $(document).ready(function () {

            LoadTechnicalInformationDropDown();
            LoadSiteInformationDropDown();
            LoadSiteInformationDropDown();

            IsEnableSiteInformationCheckBox();
            var chkIsSiteEnable = '<%=chkIsSiteEnable.ClientID%>'
            if ($('#' + chkIsSiteEnable).is(':checked')) {
                $('#SitePanel').show();
            }
            else {
                $('#SitePanel').hide();
            }

            $('#' + chkIsSiteEnable).change(function () {
                if ($('#' + chkIsSiteEnable).is(':checked')) {

                    $('#SitePanel').show();
                }
                else {

                    $('#SitePanel').hide();
                }

            });


            var ddlSiteInformation = '<%=ddlSiteInformation.ClientID%>'
            var ddlTechnicalInformation = '<%=ddlTechnicalInformation.ClientID%>'
            var ddlBillingInformarion = '<%=ddlBillingInformarion.ClientID%>'

            $('#' + ddlSiteInformation).change(function () {
                var siteId = $('#' + ddlSiteInformation).val();
                PageMethods.GetSiteInformation(siteId, OnGetSiteInformationSucceeded, OnGetSiteInformationFailed);

            });



            function OnGetSiteInformationSucceeded(result) {
                var txtSiteName = '<%=txtSiteName.ClientID%>'
                var txtSiteAddress = '<%=txtSiteAddress.ClientID%>'
                var txtSiteContactPerson = '<%=txtSiteContactPerson.ClientID%>'
                var txtSitePhoneNumber = '<%=txtSitePhoneNumber.ClientID%>'
                var txtSiteEmail = '<%=txtSiteEmail.ClientID%>'
                var txtSiteAddress = '<%=txtSiteAddress.ClientID%>'
                $('#' + txtSiteName).val(result.SiteName);
                $('#' + txtSiteAddress).val(result.SiteAddress);
                $('#' + txtSiteContactPerson).val(result.SiteContactPerson);
                $('#' + txtSitePhoneNumber).val(result.SitePhoneNumber);
                $('#' + txtSiteEmail).val(result.SiteEmail);
            }

            function OnGetSiteInformationFailed(error) {

            }


            $('#' + ddlTechnicalInformation).change(function () {
                var technicalId = $('#' + ddlTechnicalInformation).val();
                PageMethods.GetTechnicalInformation(technicalId, OnGetTechnicalInformationSucceeded, OnGetTechnicalInformationFailed);

            });
            function OnGetTechnicalInformationFailed(error) {

            }
            function OnGetTechnicalInformationSucceeded(result) {
                var txtTechnicalContactPerson = '<%=txtTechnicalContactPerson.ClientID%>'
                var txtTechnicalPersonDesignation = '<%=txtTechnicalPersonDesignation.ClientID%>'
                var txtTechnicalPersonDepartment = '<%=txtTechnicalPersonDepartment.ClientID%>'
                var txtTechnicalPersonPhone = '<%=txtTechnicalPersonPhone.ClientID%>'
                var txtTechnicalPersonEmail = '<%=txtTechnicalPersonEmail.ClientID%>'
                $('#' + txtTechnicalContactPerson).val(result.TechnicalContactPerson);
                $('#' + txtTechnicalPersonDesignation).val(result.TechnicalPersonDesignation);
                $('#' + txtTechnicalPersonDepartment).val(result.TechnicalPersonDepartment);
                $('#' + txtTechnicalPersonPhone).val(result.TechnicalPersonPhone);
                $('#' + txtTechnicalPersonEmail).val(result.TechnicalPersonEmail);
            }


            $('#' + ddlBillingInformarion).change(function () {
                var billingId = $('#' + ddlBillingInformarion).val();
                PageMethods.GetBillingInformarion(billingId, OnGetBillingInformarionSucceeded, OnGetBillingInformarionFailed);
            });

            function OnGetBillingInformarionSucceeded(result) {
                var txtBillingContactPerson = '<%=txtBillingContactPerson.ClientID%>'
                var txtBillingPersonDepartment = '<%=txtBillingPersonDepartment.ClientID%>'
                var txtBillingPersonDesignation = '<%=txtBillingPersonDesignation.ClientID%>'
                var txtBillingPersonPhone = '<%=txtBillingPersonPhone.ClientID%>'
                var txtBillingPersonEmail = '<%=txtBillingPersonEmail.ClientID%>'
                $('#' + txtBillingContactPerson).val(result.BillingContactPerson);
                $('#' + txtBillingPersonDepartment).val(result.BillingPersonDepartment);
                $('#' + txtBillingPersonDesignation).val(result.BillingPersonDesignation);
                $('#' + txtBillingPersonEmail).val(result.BillingPersonEmail);
                $('#' + txtBillingPersonPhone).val(result.BillingPersonPhone);
            }

            function OnGetBillingInformarionFailed(error) {
            }





            var txtHiddenSalesId = '<%=txtHiddenSalesId.ClientID%>'
            var salesId = $('#' + txtHiddenSalesId).val();
            if (salesId != "") {
                $('#ContentPlaceHolder1_ddlFrequency').attr('disabled', true);
                $('#PaymentDetailsInformation').hide();
                $('#ContentPlaceHolder1_btnSave').attr('disabled', false);

            }
            else {

                $('#ContentPlaceHolder1_ddlFrequency').attr('disabled', false);
                $('#ContentPlaceHolder1_btnSave').attr('disabled', true);
            }

            var HiddenFrequencyId = '<%=HiddenFrequencyId.ClientID%>'
            var frequency = $('#ContentPlaceHolder1_ddlFrequency').val();
            $('#' + HiddenFrequencyId).val(frequency);

            LoadInitialGridView();
            var txtSalesAmount = '<%=txtSalesAmount.ClientID%>'
            if (parseFloat($('#' + txtSalesAmount).val()) == 0) {
                $('#btnBillPreview').hide();
            }
            else {
                $('#btnBillPreview').show();
            }
            $('#TotalPaid').hide();

            $('#ContentPlaceHolder1_AlartMessege').show();

            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Sales Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Sales</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            var ddlCustomerId = '<%=ddlCustomerId.ClientID%>'


            var ddlManufacturer = '<%=ddlManufacturer.ClientID%>'
            var ddlProductCategory = '<%=ddlProductCategory.ClientID%>'
            var ddlServiceType = '<%=ddlServiceType.ClientID%>'
            var serviceType = $('#' + ddlServiceType).val();
            LoadManufacturer(serviceType);
            LoadProductCategory(serviceType);
            $('#ContentPlaceHolder1_ddlProductId').attr('disabled', true);
            $('#ContentPlaceHolder1_ddlServiceId').attr('disabled', true);

            if (serviceType == "Product") {
                $('#ProductDropDownList').show();
                $('#ServiceDropDownList').hide();
                $('#ServiceBundleDropDownList').hide();
                $('#ProductPanel').show();
                $('#ItemNamePanel').show();
                $('#ServicePanel').hide();
                $('#BundlePanel').hide();
                $('#categoryPanel').show();
            }
            else if (serviceType == "Service") {
                $('#ProductDropDownList').hide();
                $('#ServiceDropDownList').show();
                $('#ServiceBundleDropDownList').hide();
                $('#ProductPanel').hide();
                $('#ItemNamePanel').hide();
                $('#ServicePanel').show();
                $('#BundlePanel').hide();
                $('#categoryPanel').show();
                var ddlServiceId = '<%=ddlServiceId.ClientID%>'
                $('#' + ddlServiceId).attr('disabled', true);
            }
            else {
                $('#ProductDropDownList').hide();
                $('#ServiceDropDownList').hide();
                $('#ServiceBundleDropDownList').show();
                $('#ProductPanel').hide();
                $('#ItemNamePanel').hide();
                $('#ServicePanel').hide();
                $('#BundlePanel').show();
                $('#categoryPanel').hide();
            }

            $('#' + ddlManufacturer).change(function () {

                var ddlServiceType = '<%=ddlServiceType.ClientID%>'
                var ddlProductCategory = '<%=ddlProductCategory.ClientID%>'
                var ddlManufacturer = '<%=ddlManufacturer.ClientID%>'
                var serviceType = $('#' + ddlServiceType).val();
                var CategoryId = $('#' + ddlProductCategory).val();
                var manufacturerId = $('#' + ddlManufacturer).val();
                LoadProductInformation(serviceType, CategoryId, manufacturerId);
            });

            $('#' + ddlProductCategory).change(function () {
                var ddlServiceType = '<%=ddlServiceType.ClientID%>'
                var ddlProductCategory = '<%=ddlProductCategory.ClientID%>'
                var ddlManufacturer = '<%=ddlManufacturer.ClientID%>'
                var serviceType = $('#' + ddlServiceType).val();
                var CategoryId = $('#' + ddlProductCategory).val();
                var manufacturerId = $('#' + ddlManufacturer).val();
                LoadProductInformation(serviceType, CategoryId, manufacturerId);
            });


            $('#ContentPlaceHolder1_ddlFrequency').change(function () {

                var txtUnitPriceLocal = '<%=txtUnitPriceLocal.ClientID%>'
                var txtUnitPriceUSD = '<%=txtUnitPriceUSD.ClientID%>'
                var hiddenUnitPriceLocal = '<%=hiddenUnitPriceLocal.ClientID%>'
                var hiddenUnitPriceUSD = '<%=hiddenUnitPriceUSD.ClientID%>'

                var UnitPriceLocal = $('#' + hiddenUnitPriceLocal).val()
                var UnitPriceUsd = $('#' + hiddenUnitPriceUSD).val()
                var HiddenFrequencyId = '<%=HiddenFrequencyId.ClientID%>'
                var frequency = $('#ContentPlaceHolder1_ddlFrequency').val();
                $('#' + HiddenFrequencyId).val(frequency);
                var multiplyer = 1;
                if (frequency == "One Time") {
                    multiplyer = 1;
                }
                else if (frequency == "Monthly") {
                    multiplyer = 1;
                }
                else if (frequency == "Quaterly") {
                    multiplyer = 3;
                }
                else if (frequency == "Half Yearly") {
                    multiplyer = 6;
                }
                else if (frequency == "Yearly") {
                    multiplyer = 12;
                }

                if ($('#' + txtUnitPriceLocal).val() != "") {
                    if (UnitPriceLocal != null && UnitPriceUsd != null) {
                        $('#' + txtUnitPriceLocal).val(UnitPriceLocal);
                        $('#' + txtUnitPriceUSD).val(UnitPriceUsd);
                    }
                    else {
                        $('#' + txtUnitPriceLocal).val("0");
                        $('#' + txtUnitPriceUSD).val("0");
                    }
                }
                SetServiceQuantityInformation();

            });


            $('#btnOwnerDetails').click(function () {
                var ddlServiceType = '<%=ddlServiceType.ClientID%>'
                var ServiceType = $('#' + ddlServiceType).val();

                var ddlProductId = '<%=ddlProductId.ClientID%>'
                var ProductId = $('#' + ddlProductId).val();

                var ddlServiceId = '<%=ddlServiceId.ClientID%>'
                var ServiceId = $('#' + ddlServiceId).val();
                var HiddenProductName = "";
                var HiddenProductId = "";
                var txtItemName = '<%=txtItemName.ClientID%>'
                $('#' + txtItemName).val();

                var ddlServiceBundleId = '<%=ddlServiceBundleId.ClientID%>'
                var ServiceBundleId = $('#' + ddlServiceBundleId).val();


                if (ServiceType == "Product") {

                    if (ProductId == 0 || ProductId == "" || isNaN(ProductId)) {
                        MessagePanelShow();
                        $('#ContentPlaceHolder1_lblMessage').text('Please Select Item Name.');
                        return;
                    }
                    ProductId = ProductId;
                    HiddenProductId = $("#ContentPlaceHolder1_ddlProductId option:selected").val();
                }
                else if (ServiceType == "Service") {

                    if (ServiceId == 0 || ServiceId == "" || isNaN(ServiceId)) {
                        MessagePanelShow();
                        $('#ContentPlaceHolder1_lblMessage').text('Please Select Item Name.');
                        return;
                    }
                    ProductId = ServiceId;

                    HiddenProductId = $("#ContentPlaceHolder1_ddlServiceId option:selected").val();
                }
                else {

                    if (ServiceBundleId == 0 || ServiceBundleId == "" || isNaN(ServiceBundleId)) {
                        MessagePanelShow();
                        $('#ContentPlaceHolder1_lblMessage').text('Please Select Item Name.');
                        return;
                    }
                    ProductId = ServiceBundleId;

                    HiddenProductId = $("#ContentPlaceHolder1_ddlServiceBundleId option:selected").val();
                }

                var txtSerialNumber = '<%=txtSerialNumber.ClientID%>'
                var serialNumber = $('#' + txtSerialNumber).val();




                HiddenProductName = $('#' + txtItemName).val();
                var txtUnitPriceLocal = '<%=txtUnitPriceLocal.ClientID%>'
                var UnitPriceLocal = $('#' + txtUnitPriceLocal).val();

                var txtUnit = '<%=txtUnit.ClientID%>'
                var Unit = $('#' + txtUnit).val();


                var detailId = '<%=detailId.ClientID%>'
                var detailId = $('#' + detailId).val();

                var ddlCurrency = '<%=ddlCurrency.ClientID%>'
                var Currency = $('#' + ddlCurrency).val();

                var txtUnitPriceUSD = '<%=txtUnitPriceUSD.ClientID%>'
                var UnitPriceUSD = $('#' + txtUnitPriceUSD).val();

                var hfIsSeparateSalesInventory = '<%=hfIsSeparateSalesInventory.ClientID%>'
                var isSeparateSalesInventory = $('#' + hfIsSeparateSalesInventory).val();

                var txtSerialNumber = '<%=txtSerialNumber.ClientID%>'
                var serialNumber = $('#' + txtSerialNumber).val();




                if ($("#hfIsSerializableProduct").val() == "Yes" && ServiceType == "Product") {
                    ValidateSerialNumberForProduct(ProductId, serialNumber);
                }
                else {
                    PageMethods.SaveProductDetails(ServiceType, ProductId, UnitPriceLocal, Unit, detailId, HiddenProductId, HiddenProductName, Currency, UnitPriceUSD, isSeparateSalesInventory, serialNumber, OnSaveProductDetailsSucceeded, OnSaveProductDetailsFailed);
                }



            });


            function ValidateSerialNumberForProduct(productId, quantity_Serial) {
                PageMethods.ValidateSerialNumber(productId, quantity_Serial, OnValidateSerialNumberSucceed, OnValidateSerialNumberFailed);
            }
            function OnValidateSerialNumberSucceed(result) {

                if (result == "") {
                    MessagePanelShow();
                    $("#<%=lblMessage.ClientID%>").text("Invalid Product Serial Number.");
                    return 0;
                }
                else {

                    $("#hfIsSerializableProduct").val("0");
                    var ddlServiceType = '<%=ddlServiceType.ClientID%>'
                    var ServiceType = $('#' + ddlServiceType).val();

                    var ddlProductId = '<%=ddlProductId.ClientID%>'
                    var ProductId = $('#' + ddlProductId).val();

                    var ddlServiceId = '<%=ddlServiceId.ClientID%>'
                    var ServiceId = $('#' + ddlServiceId).val();
                    var HiddenProductName = "";
                    var HiddenProductId = "";
                    var txtItemName = '<%=txtItemName.ClientID%>'
                    $('#' + txtItemName).val();

                    var ddlServiceBundleId = '<%=ddlServiceBundleId.ClientID%>'
                    var ServiceBundleId = $('#' + ddlServiceBundleId).val();


                    if (ServiceType == "Product") {

                        if (ProductId == 0 || ProductId == "" || isNaN(ProductId)) {
                            MessagePanelShow();
                            $('#ContentPlaceHolder1_lblMessage').text('Please Select Item Name.');
                            return;
                        }
                        ProductId = ProductId;
                        HiddenProductId = $("#ContentPlaceHolder1_ddlProductId option:selected").val();
                    }
                    else if (ServiceType == "Service") {

                        if (ServiceId == 0 || ServiceId == "" || isNaN(ServiceId)) {
                            MessagePanelShow();
                            $('#ContentPlaceHolder1_lblMessage').text('Please Select Item Name.');
                            return;
                        }
                        ProductId = ServiceId;

                        HiddenProductId = $("#ContentPlaceHolder1_ddlServiceId option:selected").val();
                    }
                    else {

                        if (ServiceBundleId == 0 || ServiceBundleId == "" || isNaN(ServiceBundleId)) {
                            MessagePanelShow();
                            $('#ContentPlaceHolder1_lblMessage').text('Please Select Item Name.');
                            return;
                        }
                        ProductId = ServiceBundleId;

                        HiddenProductId = $("#ContentPlaceHolder1_ddlServiceBundleId option:selected").val();
                    }

                    var txtSerialNumber = '<%=txtSerialNumber.ClientID%>'
                    var serialNumber = $('#' + txtSerialNumber).val();




                    HiddenProductName = $('#' + txtItemName).val();
                    var txtUnitPriceLocal = '<%=txtUnitPriceLocal.ClientID%>'
                    var UnitPriceLocal = $('#' + txtUnitPriceLocal).val();

                    var txtUnit = '<%=txtUnit.ClientID%>'
                    var Unit = $('#' + txtUnit).val();


                    var detailId = '<%=detailId.ClientID%>'
                    var detailId = $('#' + detailId).val();

                    var ddlCurrency = '<%=ddlCurrency.ClientID%>'
                    var Currency = $('#' + ddlCurrency).val();

                    var txtUnitPriceUSD = '<%=txtUnitPriceUSD.ClientID%>'
                    var UnitPriceUSD = $('#' + txtUnitPriceUSD).val();

                    var hfIsSeparateSalesInventory = '<%=hfIsSeparateSalesInventory.ClientID%>'
                    var isSeparateSalesInventory = $('#' + hfIsSeparateSalesInventory).val();

                    var txtSerialNumber = '<%=txtSerialNumber.ClientID%>'
                    var serialNumber = $('#' + txtSerialNumber).val();
                    PageMethods.SaveProductDetails(ServiceType, ProductId, UnitPriceLocal, Unit, detailId, HiddenProductId, HiddenProductName, Currency, UnitPriceUSD, isSeparateSalesInventory, serialNumber, OnSaveProductDetailsSucceeded, OnSaveProductDetailsFailed);


                }
            }
            function OnValidateSerialNumberFailed(error) {
            }






            $('#' + ddlCustomerId).change(function () {
                LoadTechnicalInformationDropDown();
                LoadSiteInformationDropDown();
                LoadBillingInformationDropDown();
                if ($('#' + ddlCustomerId).val() == "0") {
                    var txtName = '<%=txtName.ClientID%>'
                    var ddlCustomerType = '<%=ddlCustomerType.ClientID%>'
                    var txtPhone = '<%=txtPhone.ClientID%>'
                    var txtEmail = '<%=txtEmail.ClientID%>'

                    var txtWebAddress = '<%=txtWebAddress.ClientID%>'
                    var txtAddress = '<%=txtAddress.ClientID%>'

                    $('#' + txtName).val('');
                    $('#' + ddlCustomerType).val('0')
                    $('#' + txtPhone).val('');
                    $('#' + txtEmail).val('');
                    $('#' + txtWebAddress).val('');
                    $('#' + txtAddress).val('');

                }
                else {
                    var customerId = $('#' + ddlCustomerId).val();
                    PageMethods.LoadCustomerInformation(customerId, OnFillCustomerInfoSucceeded, OnFillCustomerInfoFailed);
                    return false;
                }
            });
        });



        $(function () {
            $("#myTabs").tabs();
        });
        $(document).ready(function () {
            $('#CreditPaymentAccountHeadDiv').hide();
            $("#btnBillPreview").click(function () {
                var customerInfo = $("#<%= txtName.ClientID %>").val();
                var url = "/SalesManagment/Reports/frmReportPMSalesInvoice.aspx?SalesId=" + "0" + "&From=TmpInvoice&InvoiceId=" + "0" + "&CustomerInfo=" + customerInfo;
                var popup_window = "Sales Preview";
                window.open(url, popup_window, "width=650,height=680,left=300,top=50,resizable=yes");
            });

            $(function () {
                var ddlPayMode = '<%=ddlPayMode.ClientID%>'
                var lblPaymentAccountHead = '<%=lblPaymentAccountHead.ClientID%>'
                $('#' + ddlPayMode).change(function () {

                    if ($('#' + ddlPayMode).val() == "Cash") {
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CreditPaymentAccountHeadDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').show();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).show();
                        $('#ComPaymentDiv').hide();
                        $('#PrintPreviewDiv').show();
                    }
                    else if ($('#' + ddlPayMode).val() == "Card") {
                        $('#CardPaymentAccountHeadDiv').show();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CreditPaymentAccountHeadDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).hide();
                        $('#ComPaymentDiv').hide();
                        $('#PrintPreviewDiv').show();
                    } //
                    else if ($('#' + ddlPayMode).val() == "Cheque") {
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').show();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CreditPaymentAccountHeadDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).hide();
                        $('#ComPaymentDiv').hide();
                        $('#PrintPreviewDiv').show();
                    }
                    else if ($('#' + ddlPayMode).val() == "Credit") {
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#CreditPaymentAccountHeadDiv').show();
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).hide();
                        $('#ComPaymentDiv').hide();
                        $('#PrintPreviewDiv').show();
                    }
                    else if ($('#' + ddlPayMode).val() == "Company") {
                        $('#PaidByOtherRoomDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').show();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#CreditPaymentAccountHeadDiv').hide();
                        $('#BankPaymentAccountHeadDiv').hide();
                        $('#lblPaymentAccountHead').hide();
                        $('#' + lblPaymentAccountHead).show();
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#ComPaymentDiv').show();
                        $('#PrintPreviewDiv').hide();
                        popup(1, 'BillSplitPopUpForm', '', 600, 518);
                    }
                    else if ($('#' + ddlPayMode).val() == "Other Room") {
                        $('#PaidByOtherRoomDiv').show();
                        $('#CardPaymentAccountHeadDiv').hide();
                        $('#CreditPaymentAccountHeadDiv').hide();
                        $('#ChecquePaymentAccountHeadDiv').hide();
                        $('#CompanyPaymentAccountHeadDiv').hide();
                        $('#CashPaymentAccountHeadDiv').hide();
                        $('#' + lblPaymentAccountHead).show();
                        $('#ComPaymentDiv').hide();
                        $('#PrintPreviewDiv').show();
                    }
                });
            });



            $("#btnAddDetailGuestPayment").click(function () {


                var txtReceiveLeadgerAmount = '<%=txtReceiveLeadgerAmount.ClientID%>'
                var txtCardNumber = '<%=txtCardNumber.ClientID%>'
                var amount = $('#' + txtReceiveLeadgerAmount).val();
                var number = $('#' + txtCardNumber).val()
                var isValid = ValidateForm();
                if (isValid == false) {
                    return;
                }
                else if (amount == "") {
                    MessagePanelShow();
                    $('#ContentPlaceHolder1_lblMessage').text('Please provide Receive Amount.');
                    return;
                }
                else {
                    SaveGuestPaymentDetailsInformationByWebMethod();
                }

            });


            //--Item Type Change----------------------------------
            var ddlServiceType = '<%=ddlServiceType.ClientID%>'

            var txtVatAmount = '<%=txtVatAmount.ClientID%>'
            var txtGrandTotal = '<%=txtGrandTotal.ClientID%>'
            var txtSalesAmount = '<%=txtSalesAmount.ClientID%>'
            var txtSalesDate = '<%=txtSalesDate.ClientID%>'
            var txtStartDate = '<%=txtFromDate.ClientID%>'
            var txtEndDate = '<%=txtToDate.ClientID%>'
            var ddlFrequency = '<%=ddlFrequency.ClientID%>'
            var ddlServiceType = '<%=ddlServiceType.ClientID%>'
            var ddlProductId = '<%=ddlProductId.ClientID%>';
            var ddlServiceId = '<%=ddlServiceId.ClientID%>';
            var ddlServiceBundleId = '<%=ddlServiceBundleId.ClientID%>';
            var ddlCurrency = '<%=ddlCurrency.ClientID%>';
            var txtCurrentId = '<%=txtCurrentId.ClientID%>';

            $('#' + txtCurrentId).val(0);
            if ($('#' + ddlCurrency).val() == 45) {

                $('#UnitPriceUSDDiv').hide();
                $('#UnitPriceLocalDiv').show();
            }
            else {
                $('#UnitPriceLocalDiv').hide();
                $('#UnitPriceUSDDiv').show();
            }

            var frequencyId = $('#' + ddlFrequency).val();
            var serviceType = $('#' + ddlServiceType).val();
            //LoadddlProductId(frequencyId, serviceType);


            $('#' + ddlCurrency).change(function () {
                var ddlCurrency = '<%=ddlCurrency.ClientID%>';
                if ($('#' + ddlCurrency).val() == 45) {

                    $('#UnitPriceUSDDiv').hide();
                    $('#UnitPriceLocalDiv').show();
                }
                else {
                    $('#UnitPriceLocalDiv').hide();
                    $('#UnitPriceUSDDiv').show();
                }

            });

            $('#' + txtSalesDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $('#' + txtStartDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtEndDate).datepicker("option", "minDate", selectedDate);
                }
            });

            $('#' + txtEndDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#' + txtStartDate).datepicker("option", "maxDate", selectedDate);
                }
            });


            var txtSalesDate = '<%=txtSalesDate.ClientID%>'
            $('#ProductDropDownList').show("slow");
            var myDate = new Date();
            var displayDate = (myDate.getMonth() + 1) + '/' + (myDate.getDate()) + '/' + myDate.getFullYear();
            $('#' + txtSalesDate).val(displayDate);

            $('#' + txtVatAmount).blur(function () {
                var vat = parseFloat($('#' + txtVatAmount).val());
                var GT = vat + (parseFloat($('#' + txtSalesAmount).val()));
                $('#' + txtGrandTotal).val(GT);

                var txtSalesAmount = '<%=txtSalesAmount.ClientID%>'
                if (parseFloat($('#' + txtSalesAmount).val()) == 0) {
                    $('#btnBillPreview').hide();
                }
                else {
                    $('#btnBillPreview').show();
                }
            });

            $('#' + ddlFrequency).change(function () {
                var frequencyId = $('#' + ddlFrequency).val();
                var serviceType = $('#' + ddlServiceType).val();

                //LoadddlProductId(frequencyId, serviceType);
            });

            $('#' + ddlServiceType).change(function () {
                var frequencyId = $('#' + ddlFrequency).val();
                var serviceType = $('#' + ddlServiceType).val();
                if (serviceType == "Product") {
                    $('#ProductDropDownList').show();
                    $('#ServiceDropDownList').hide();
                    $('#ServiceBundleDropDownList').hide();
                    $('#ProductPanel').show();
                    $('#ItemNamePanel').show();
                    $('#ServicePanel').hide();
                    $('#BundlePanel').hide();
                    $('#categoryPanel').show();
                }
                else if (serviceType == "Service") {
                    $('#ProductDropDownList').hide();
                    $('#ServiceDropDownList').show();
                    $('#ServiceBundleDropDownList').hide();
                    $('#ProductPanel').hide();
                    $('#ItemNamePanel').hide();
                    $('#ServicePanel').show();
                    $('#BundlePanel').hide();
                    $('#categoryPanel').show();
                    var ddlServiceId = '<%=ddlServiceId.ClientID%>'
                    $('#' + ddlServiceId).attr('disabled', true);
                }
                else {
                    $('#ProductDropDownList').hide();
                    $('#ServiceDropDownList').hide();
                    $('#ServiceBundleDropDownList').show();
                    $('#ProductPanel').hide();
                    $('#ItemNamePanel').hide();
                    $('#ServicePanel').hide();
                    $('#BundlePanel').show();
                    $('#categoryPanel').hide();
                }

                var ddlProductCategory = '<%=ddlProductCategory.ClientID%>'
                var ddlManufacturer = '<%=ddlManufacturer.ClientID%>'
                var CategoryId = $('#' + ddlProductCategory).val();
                var manufacturerId = $('#' + ddlManufacturer).val();
                //LoadddlProductId(frequencyId, serviceType);
                LoadManufacturer(serviceType);
                LoadProductCategory(serviceType);
            });


            $('#' + ddlProductId).change(function () {
                var frequencyId = $('#' + ddlFrequency).val();
                var serviceType = $('#' + ddlServiceType).val();
                var product = $('#' + ddlProductId).val();

                CheckProductIsSerializableOrNot(product);
                LoadProductData(frequencyId, serviceType, product);
            });

            function CheckProductIsSerializableOrNot(productId) {
                PageMethods.IsProductSerializable(productId, OnCheckProductIsSerializableOrNotSucceed, OnCheckProductIsSerializableOrNotError);
            }

            function OnCheckProductIsSerializableOrNotSucceed(result) {

                var lblQuantitySerial = $("#<%=lblSerialNumber.ClientID %>");
                var txtQuantitySerial = $("#<%=txtSerialNumber.ClientID %>");

                if (result == "1") {
                    lblQuantitySerial.text("Serial Number");
                    txtQuantitySerial.removeClass("CustomTextBox").addClass("ThreeColumnTextBox");
                    $("#hfIsSerializableProduct").val("1");
                }
                else {

                    lblQuantitySerial.text("Quantity");
                    txtQuantitySerial.removeClass("ThreeColumnTextBox").addClass("CustomTextBox");
                }
            }
            function OnCheckProductIsSerializableOrNotError(error) { }

            $('#' + ddlServiceId).change(function () {
                var frequencyId = $('#' + ddlFrequency).val();
                var serviceType = $('#' + ddlServiceType).val();
                var product = $('#' + ddlServiceId).val();
                LoadProductData(frequencyId, serviceType, product);
            });

            $('#' + ddlServiceBundleId).change(function () {
                var frequencyId = $('#' + ddlFrequency).val();
                var serviceType = $('#' + ddlServiceType).val();
                var product = $('#' + ddlServiceBundleId).val();
                LoadProductData(frequencyId, serviceType, product);
            });
        });


        // Load Site Technical And Billing Information

        function LoadSiteInformationDropDown() {

            var ddlCustomerId = '<%=ddlCustomerId.ClientID%>'
            var customerId = $('#' + ddlCustomerId).val();

            PageMethods.LoadSiteInformationDropDown(customerId, OnLoadSiteInformationDropDownSucceeded, OnLoadSiteInformationDropDownFailed);
            return false;
        }

        //
        function OnLoadSiteInformationDropDownSucceeded(result) {
            var list = result;
            var controlId;
            var control;
            controlId = '<%=ddlSiteInformation.ClientID%>';
            control = $('#' + controlId);
            control.empty();
            if (list != null) {
                for (i = 0; i < list.length; i++) {
                    control.append('<option title="' + list[i].ItemName + '" value="' + list[i].ItemId + '">' + list[i].Code + '</option>');
                }
            }

            return false;
        }
        function OnLoadSiteInformationDropDownFailed(error) {
        }

        function LoadTechnicalInformationDropDown() {

            var ddlCustomerId = '<%=ddlCustomerId.ClientID%>'
            var customerId = $('#' + ddlCustomerId).val();

            PageMethods.LoadTechnicalInformationDropDown(customerId, OnLoadTechnicalInformationDropDownSucceeded, OnLoadTechnicalInformationDropDownFailed);
            return false;
        }

        //
        function OnLoadTechnicalInformationDropDownSucceeded(result) {
            var list = result;
            var controlId;
            var control;
            controlId = '<%=ddlTechnicalInformation.ClientID%>';
            control = $('#' + controlId);
            control.empty();
            if (list != null) {
                for (i = 0; i < list.length; i++) {
                    control.append('<option title="' + list[i].ItemName + '" value="' + list[i].ItemId + '">' + list[i].Code + '</option>');
                }
            }

            return false;
        }
        function OnLoadTechnicalInformationDropDownFailed(error) {
        }

        function LoadBillingInformationDropDown() {

            var ddlCustomerId = '<%=ddlCustomerId.ClientID%>'
            var customerId = $('#' + ddlCustomerId).val();

            PageMethods.LoadBillingInformationDropDown(customerId, OnLoadBillingInformationDropDownSucceeded, OnLoadBillingInformationDropDownFailed);
            return false;
        }

        //
        function OnLoadBillingInformationDropDownSucceeded(result) {
            var list = result;
            var controlId;
            var control;
            controlId = '<%=ddlBillingInformarion.ClientID%>';
            control = $('#' + controlId);
            control.empty();
            if (list != null) {
                for (i = 0; i < list.length; i++) {
                    control.append('<option title="' + list[i].ItemName + '" value="' + list[i].ItemId + '">' + list[i].Code + '</option>');
                }
            }

            return false;
        }
        function OnLoadBillingInformationDropDownFailed(error) {
        }

        function LoadManufacturer(serviceType) {
            PageMethods.LoadProductManufacturerByService(serviceType, OnLoadProductManufacturerSucceeded, OnLoadProductManufacturerFailed);
            return false;
        }

        function OnLoadProductManufacturerSucceeded(result) {
            var ddlServiceType = '<%=ddlServiceType.ClientID%>'
            $('#ContentPlaceHolder1_ddlProductId').attr('disabled', true);
            var list = result;
            var controlId = '<%=ddlManufacturer.ClientID%>';
            var control = $('#' + controlId);
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].ManufacturerId + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            return false;
        }
        function OnLoadProductManufacturerFailed(error) {
        }

        function LoadProductCategory(serviceType) {
            PageMethods.LoadProductCategoryByService(serviceType, OnLoadProductCategoryByServiceSucceeded, OnLoadProductCategoryByServiceFailed);
            return false;
        }

        function OnLoadProductCategoryByServiceSucceeded(result) {
            var ddlServiceType = '<%=ddlServiceType.ClientID%>'
            if ($('#' + ddlServiceType).val() == 'Product') {
                var txtUnit = '<%=txtUnit.ClientID%>';
                $('#' + txtUnit).val('1');
            }
            var list = result;
            var ddlProductCategory = '<%=ddlProductCategory.ClientID%>';
            var control = $('#' + ddlProductCategory);
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].CategoryId + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }
            return false;
        }
        function OnLoadProductCategoryByServiceFailed(error) {
        }


        function PerformFillFormActionWAW(DetailsId, ServiceType, ItemName, ItemId, ItemUnit, ItemPrice) {

            LoadManufacturer(ServiceType);
            LoadProductCategory(ServiceType);
            var detailId = '<%=detailId.ClientID%>'
            $('#' + detailId).val(DetailsId);
            var txtCurrentId = '<%=txtCurrentId.ClientID%>'
            $('#' + txtCurrentId).val(ItemId);
            var txtUnitPriceLocal = '<%=txtUnitPriceLocal.ClientID%>'
            var txtUnitPriceUSD = '<%=txtUnitPriceUSD.ClientID%>'
            var ddlFrequency = '<%=ddlFrequency.ClientID%>'
            var ddlProductId = '<%=ddlProductId.ClientID%>';
            var ddlServiceType = '<%=ddlServiceType.ClientID%>'

            var ddlServiceId = '<%=ddlServiceId.ClientID%>';
            var ddlServiceBundleId = '<%=ddlServiceBundleId.ClientID%>';
            var txtUnit = '<%=txtUnit.ClientID%>'
            var ddlCurrency = '<%=ddlCurrency.ClientID%>';

            if (ServiceType == "Product") {
                $('#ProductDropDownList').show();
                $('#ServiceDropDownList').hide();
                $('#ServiceBundleDropDownList').hide();
                $('#ProductPanel').show();
                $('#ItemNamePanel').show();
                $('#ServicePanel').hide();
                $('#BundlePanel').hide();
                $('#categoryPanel').show();

            }
            else if (ServiceType == "Service") {
                $('#ProductDropDownList').hide();
                $('#ServiceDropDownList').show();
                $('#ServiceBundleDropDownList').hide();
                $('#ProductPanel').hide();
                $('#ItemNamePanel').hide();
                $('#ServicePanel').show();
                $('#BundlePanel').hide();
                $('#categoryPanel').show();
            }
            else {
                $('#ProductDropDownList').hide();
                $('#ServiceDropDownList').hide();
                $('#ServiceBundleDropDownList').show();
                $('#ProductPanel').hide();
                $('#ItemNamePanel').hide();
                $('#ServicePanel').hide();
                $('#BundlePanel').show();
                $('#categoryPanel').hide();
            }
            var frequencyId = $('#' + ddlFrequency).val();
            $('#' + ddlServiceType).val(ServiceType);

            var txtItemName = '<%=txtItemName.ClientID%>'
            $('#' + txtItemName).val(ItemName);

            if (ServiceType == "Product") {
                $('#' + ddlProductId).val(ItemId);
                $('#' + ddlProductId).attr('disabled', false);
            }
            else if (ServiceType == "Service") {
                $('#' + ddlServiceId).val(ItemId);
                $('#' + ddlServiceId).attr('disabled', false);
            }
            else {
                $('#' + ddlServiceBundleId).val(ItemId);
                $('#' + ddlServiceBundleId).attr('disabled', false);
            }

            $('#' + txtUnit).val(ItemUnit);
            $('#' + txtUnitPriceLocal).val();
            var unit = parseFloat(ItemUnit);
            var price = parseFloat(ItemPrice);
            var singlePrice = price / unit;
            if ($('#' + ddlCurrency).val() == 45) {
                $('#' + txtUnitPriceLocal).val(singlePrice);
            }
            else {
                $('#' + txtUnitPriceLocal).val(singlePrice);
            }
            $('#btnOwnerDetails').text("Edit");
            $('#btnOwnerDetails').val("Edit");
            return false;
        }

        /*-----------------------------*/

        function LoadProductData(frequencyId, serviceType, product) {
            PageMethods.LoadProductDataByCriteria(frequencyId, serviceType, product, OnFillProductSucceeded, OnFillproductFailed);
            return false;
        }

        function OnFillProductSucceeded(result) {
            var isSeparateSalesInventory = '<%=hfIsSeparateSalesInventory.ClientID%>'
            if ($('#' + isSeparateSalesInventory).val() == "Yes") {
                $('#IsSeparateSalesInventoryDiv').hide();
            }
            else {
                if (result.ProductType == "Serial Product") {
                    $('#IsSeparateSalesInventoryDiv').show();
                }
                else {
                    $('#IsSeparateSalesInventoryDiv').hide();
                }
            }
            
            var txtUnitPriceLocal = '<%=txtUnitPriceLocal.ClientID%>'
            var txtUnitPriceUSD = '<%=txtUnitPriceUSD.ClientID%>'
            var txtHiddenProductId = '<%=txtHiddenProductId.ClientID%>'
            var txtHiddenProductName = '<%=txtHiddenProductName.ClientID%>'
            var txtItemName = '<%=txtItemName.ClientID%>'
            var ddlFrequency = '<%=ddlFrequency.ClientID%>'

            var frequency = $('#' + ddlFrequency).val();
            $('#' + txtItemName).val(result.ItemName);
            $('#' + txtHiddenProductId).val(result.ItemId);
            $('#' + txtHiddenProductName).val(result.ItemName);

            var txtUnitPriceLocal = '<%=txtUnitPriceLocal.ClientID%>'
            var txtUnitPriceUSD = '<%=txtUnitPriceUSD.ClientID%>'
            var hiddenUnitPriceLocal = '<%=hiddenUnitPriceLocal.ClientID%>'
            var hiddenUnitPriceUSD = '<%=hiddenUnitPriceUSD.ClientID%>'
            $('#' + hiddenUnitPriceLocal).val(result.UnitPriceLocal)
            $('#' + hiddenUnitPriceUSD).val(result.UnitPriceUsd)
            var multiplyer = 1;
            if (frequency == "One Time") {
                multiplyer = 1;
            }
            else if (frequency == "Monthly") {
                multiplyer = 1;
            }
            else if (frequency == "Quaterly") {
                multiplyer = 3;
            }
            else if (frequency == "Half Yearly") {
                multiplyer = 6;
            }
            else if (frequency == "Yearly") {
                multiplyer = 12;
            }


            if (result.UnitPriceLocal != null && result.UnitPriceUsd != null) {
                $('#' + txtUnitPriceLocal).val(result.UnitPriceLocal);
                $('#' + txtUnitPriceUSD).val(result.UnitPriceUsd);
            }
            else {
                $('#' + txtUnitPriceLocal).val("0");
                $('#' + txtUnitPriceUSD).val("0");
            }

            SetServiceQuantityInformation();
            return false;
        }

        function OnFillproductFailed(error) {
            alert(error.get_message());
        }


        function SetServiceQuantityInformation() {
            var ddlServiceType = '<%=ddlServiceType.ClientID%>'
            var ddlServiceId = '<%=ddlServiceId.ClientID%>'
            var txtUnit = '<%=txtUnit.ClientID%>'
            var ddlFrequency = '<%=ddlFrequency.ClientID%>'

            var frequency = $('#' + ddlFrequency).val();
            var serviceId = $('#' + ddlServiceId).val();

            if ($('#' + ddlServiceType).val() == "Service") {
                PageMethods.LoadServiceQuantityInformation(frequency, serviceId, OnLoadServiceQuantityInformationSucceeded, OnFillproductFailed);
            }
            return false;

        }

        function OnLoadServiceQuantityInformationSucceeded(result) {
            var txtUnit = '<%=txtUnit.ClientID%>'
            $('#' + txtUnit).val(result);
        }


        function LoadProductInformation(ServiceType, CategoryId, manufacturerId) {
            PageMethods.LoadProductInformationByCategoryAndManufacturer(ServiceType, CategoryId, manufacturerId, OnLoadProductInformationSucceeded, OnLoadProductInformationFailed);
            return false;
        }

        function OnLoadProductInformationSucceeded(result) {
            var ddlServiceType = '<%=ddlServiceType.ClientID%>'
            var txtCurrentId = '<%=txtCurrentId.ClientID%>'
            var serviceType = $('#' + ddlServiceType).val();
            var list = result;
            var controlId;
            var control;
            if (serviceType == "Product") {
                controlId = '<%=ddlProductId.ClientID%>';
                $('#' + controlId).attr('disabled', true);
                control = $('#' + controlId);
            }
            else if (serviceType == "Service") {
                controlId = '<%=ddlServiceId.ClientID%>';
                $('#' + controlId).attr('disabled', true);
                control = $('#' + controlId);
            }
            else {
                controlId = '<%=ddlServiceBundleId.ClientID%>';
                control = $('#' + controlId);
            }

            var hfIsSeparateSalesInventory = '<%=hfIsSeparateSalesInventory.ClientID%>'
            if (result.ProductType == "Serial Product") {
                $("#hfIsSerializableProduct").val("1");
             }
            else {
                $("#hfIsSerializableProduct").val("");
                MessagePanelHide();
            }

            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {

                        if (serviceType == "Product") {
                            control.append('<option title="' + list[i].ItemName + '" value="' + list[i].ItemId + '">' + list[i].Code + '</option>');
                        }
                        else {
                            control.append('<option title="' + list[i].ItemName + '" value="' + list[i].ItemId + '">' + list[i].ItemName + '</option>');
                        }
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }
            var id = $('#' + txtCurrentId).val();
            $('#' + controlId).val(id);

            return false;
        }

        function OnLoadProductInformationFailed(error) {
        }


        function LoadddlProductId(frequencyId, serviceType) {
            PageMethods.GetServiceByCriteria(frequencyId, serviceType, OnFillServiceSucceeded, OnFillServiceFailed);
            return false;
        }


        function OnFillServiceSucceeded(result) {
            var ddlServiceType = '<%=ddlServiceType.ClientID%>'
            var txtCurrentId = '<%=txtCurrentId.ClientID%>'
            var serviceType = $('#' + ddlServiceType).val();
            var list = result;
            var controlId = '<%=ddlProductId.ClientID%>';
            var control = $('#' + controlId);
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Code + '" value="' + list[i].ItemId + '">' + list[i].Code + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            var id = $('#' + txtCurrentId).val();
            $('#' + controlId).val(id);

            return false;
        }


        function LoadInitialGridView() {
            var txtHiddenSalesId = '<%=txtHiddenSalesId.ClientID%>'
            var salesId = $('#' + txtHiddenSalesId).val();
            if (salesId != "") {
                $('#btnBillPreview').show();
                PageMethods.LoadSalesDetailGridView(salesId, OnLoadSalesDetailGridViewSucceeded, OnLoadSalesDetailGridViewFailed);
                return false;
            }
        }

        function OnFillServiceFailed(error) {
            alert(error.get_message());
        }

        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            $("#<%=txtItemName.ClientID %>").val("Save");
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {
            MessagePanelHide();

            $('#btnNew').hide("slow");
            $('#EntryPanel').show("slow");
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
            window.location = "frmPMSales.aspx?DeleteConfirmation=Deleted"
        }

        function OnDeleteObjectFailed(error) {
            alert(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {

            $("#<%=btnSave.ClientID %>").val("Save");
            MessagePanelHide();
            return false;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNew').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNew').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
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
            $('#btnNew').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNew').hide("slow");
        }



        function ValidateForm() {

            var isCardValid = validateCard();
            var isDateValid = ValidateExpireDate();
            if (isCardValid != true) {
                return false;
            }
            else if (isDateValid != true) {
                var lblMessage = '<%=lblMessage.ClientID%>'
                $('#' + lblMessage).text("Please fill the Expiry Date");
                MessagePanelShow();
                return false;
            }
            else {
                return true;
            }
        }
        function ValidateExpireDate() {
            var isValid = true;
            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            var txtExpireDate = '<%=txtExpireDate.ClientID%>'
            if ($('#' + ddlPayMode).val() == "Card") {
                if ($('#' + txtExpireDate).val() == "") {
                    isValid = false;
                }
            }
            return isValid;
        }


        function validateCard() {
            var txtCardValidation = '<%=txtCardValidation.ClientID%>'
            var ddlPayMode = '<%=ddlPayMode.ClientID%>'
            if ($('#' + ddlPayMode).val() != "Card") {
                return true;
            }

            if ($('#' + txtCardValidation).val() == 0) {
                return true;
            }


            var txtCardNumber = '<%=txtCardNumber.ClientID%>'
            var ddlCardType = '<%=ddlCardType.ClientID%>'
            var cardNumber = $('#' + txtCardNumber).val();
            var cardType = $('#' + ddlCardType).val();
            var isTrue = true;
            var messege = "";

            if (!cardType) {
                isTrue = false;
                messege = "Card number must not be empty.";
            }

            if (cardNumber.length == 0) {						//most of these checks are self explanitory

                //alert("Please enter a valid card number.");
                isTrue = false;
                messege = "Please enter a valid card number."

            }
            for (var i = 0; i < cardNumber.length; ++i) {		// make sure the number is all digits.. (by design)
                var c = cardNumber.charAt(i);


                if (c < '0' || c > '9') {

                    isTrue = false;
                    messege = "Please enter a valid card number. Use only digits. do not use spaces or hyphens.";
                }
            }
            var length = cardNumber.length; 		//perform card specific length and prefix tests

            switch (cardType) {
                case 'a':
                    if (length != 15) {

                        //alert("");
                        isTrue = false;
                        messege = "Please enter a valid American Express Card number.";
                    }
                    var prefix = parseInt(cardNumber.substring(0, 2));

                    if (prefix != 34 && prefix != 37) {


                        isTrue = false;
                        messege = "Please enter a valid American Express Card number.";
                    }
                    break;
                case 'd':

                    if (length != 16) {

                        //alert("");
                        isTrue = false;
                        messege = "Please enter a valid Discover Card number.";
                    }
                    var prefix = parseInt(cardNumber.substring(0, 4));

                    if (prefix != 6011) {

                        //alert("Please enter a valid Discover Card number.");
                        isTrue = false;
                        messege = "Please enter a valid Discover Card number.";
                    }
                    break;
                case 'm':

                    if (length != 16) {

                        //alert("");
                        isTrue = false;
                        messege = "Please enter a valid MasterCard number.";
                    }
                    var prefix = parseInt(cardNumber.substring(0, 2));

                    if (prefix < 51 || prefix > 55) {

                        isTrue = false;
                        messege = "Please enter a valid MasterCard number.";
                    }
                    break;
                case 'v':

                    if (length != 16 && length != 13) {

                        //alert("");
                        isTrue = false;
                        messege = "Please enter a valid Visa Card number.";
                    }
                    var prefix = parseInt(cardNumber.substring(0, 1));

                    if (prefix != 4) {
                        isTrue = false;
                        messege = "Please enter a valid Visa Card number.";
                    }
                    break;
            }
            if (!mod10(cardNumber)) {
                //alert("");
                isTrue = false;
                messege = "Sorry! this is not a valid credit card number.";
            }

            if (isTrue == false) {
                MessagePanelShow();
                var lblMessage = '<%=lblMessage.ClientID%>'
                $('#' + lblMessage).text(messege);
                alert(messege);
                return false;
            }
            else {
                MessagePanelHide();
                return true;
            }
        }


        function mod10(cardNumber) { // LUHN Formula for validation of credit card numbers.
            var ar = new Array(cardNumber.length);
            var i = 0, sum = 0;

            for (i = 0; i < cardNumber.length; ++i) {
                ar[i] = parseInt(cardNumber.charAt(i));
            }
            for (i = ar.length - 2; i >= 0; i -= 2) { // you have to start from the right, and work back.
                ar[i] *= 2; 						 // every second digit starting with the right most (check digit)
                if (ar[i] > 9) ar[i] -= 9; 		 // will be doubled, and summed with the skipped digits.
            } 									 // if the double digit is > 9, ADD those individual digits together 


            for (i = 0; i < ar.length; ++i) {
                sum += ar[i]; 					 // if the sum is divisible by 10 mod10 succeeds
            }
            return (((sum % 10) == 0) ? true : false);
        }


        function SaveGuestPaymentDetailsInformationByWebMethod() {

            var Amount = $("#<%=txtReceiveLeadgerAmount.ClientID %>").val();
            var floatAmout = parseFloat(Amount);
            if (floatAmout <= 0) {
                MessagePanelShow();
                $('#ContentPlaceHolder1_lblMessage').text('Receive Amount is not in correct format.');
                return;
            }
            else {
                MessagePanelHide();
                $('#ContentPlaceHolder1_lblMessage').text('');
            }


            var isEdit = false;
            if ($('#btnAddDetailGuestPayment').val() == "Edit") {
                $("#<%=lblHiddenIdDetailGuestPayment.ClientID %>").val("5");
                isEdit = true;
            }
            else {
                $("#<%=lblHiddenIdDetailGuestPayment.ClientID %>").val("0");
            }

            var ddlCreditAccountHead = $("#<%=ddlCreditAccountHead.ClientID %>").val();

            var ddlPayMode = $("#<%=ddlPayMode.ClientID %>").val();
            var txtReceiveLeadgerAmount = $("#<%=txtReceiveLeadgerAmount.ClientID %>").val();
            var ddlCashReceiveAccountsInfo = $("#<%=ddlCashReceiveAccountsInfo.ClientID %>").val();

            var txtCardNumber = $("#<%=txtCardNumber.ClientID %>").val();
            var ddlCardType = $("#<%=ddlCardType.ClientID %>").val();
            var txtExpireDate = $("#<%=txtExpireDate.ClientID %>").val();

            var ddlBankName = $("#<%=ddlBankName.ClientID %>").val();

            var txtCardHolderName = $("#<%=txtCardHolderName.ClientID %>").val();
            var txtChecqueNumber = $("#<%=txtChecqueNumber.ClientID %>").val();
            var ddlBankId = $("#<%=ddlBankId.ClientID %>").val();
            var ddlCompanyPaymentAccountHead = $("#<%=ddlCompanyPaymentAccountHead.ClientID %>").val();



            $('#btnAddDetailGuestPayment').val("Add");

            PageMethods.PerformSaveGuestPaymentDetailsInformationByWebMethod(isEdit, ddlPayMode, txtReceiveLeadgerAmount, ddlCashReceiveAccountsInfo, txtCardNumber, ddlCardType, txtExpireDate, txtCardHolderName, txtChecqueNumber, ddlBankId, ddlCompanyPaymentAccountHead, ddlCreditAccountHead, OnPerformSaveGuestPaymentDetailsInformationSucceeded, OnPerformSaveGuestPaymentDetailsInformationFailed)

            return false;
        }
        function OnPerformSaveGuestPaymentDetailsInformationFailed(error) {
            alert(error.get_message());
        }
        function OnPerformSaveGuestPaymentDetailsInformationSucceeded(result) {
            $("#GuestPaymentDetailGrid").html(result);
            ClearDetailsPart();
            GetTotalPaidAmount()
        }


        function GetTotalPaidAmount() {
            PageMethods.PerformGetTotalPaidAmountByWebMethod(OnPerformGetTotalPaidAmountSucceeded, PerformGetTotalPaidAmountFailed)
            return false;
        }

        function PerformGetTotalPaidAmountFailed(error) {
            alert(error.get_message());
        }
        function OnPerformGetTotalPaidAmountSucceeded(result) {
            var txtGrandTotal = $("#<%=txtGrandTotal.ClientID %>").val();

            var _grandTotal = parseFloat(txtGrandTotal);

            var GrandTotal = parseFloat(txtGrandTotal);
            var PaidTotal = parseFloat(result);


            if (_grandTotal == 0) {
                if (PaidTotal != _grandTotal) {

                    $('#ContentPlaceHolder1_btnSave').attr('disabled', true);
                    $('#ContentPlaceHolder1_AlartMessege').show();
          
                }
                else {
                    $('#ContentPlaceHolder1_btnSave').attr('disabled', false);
                    $('#ContentPlaceHolder1_AlartMessege').hide();
                }
            }
            else if (PaidTotal == GrandTotal) {
                $('#ContentPlaceHolder1_btnSave').attr('disabled', false);
                $('#ContentPlaceHolder1_AlartMessege').hide();
            }
            else {
                var txtHiddenSalesId = '<%=txtHiddenSalesId.ClientID%>'
                var salesId = $('#' + txtHiddenSalesId).val();
                if (salesId != "") {
                    $('#ContentPlaceHolder1_btnSave').attr('disabled', false);
                    $('#ContentPlaceHolder1_AlartMessege').hide();
                }
                else {
                    $('#ContentPlaceHolder1_btnSave').attr('disabled', true);
                    $('#ContentPlaceHolder1_AlartMessege').show();
                }


                

            }

            var dueAmountTotal = GrandTotal - PaidTotal;

            var dueFormatedText = "Due Amount   :  " + dueAmountTotal;
            $('#dueTotal').show();
            $('#dueTotal').text(dueFormatedText);

            var FormatedText = "Total Amount: " + PaidTotal;
            $('#TotalPaid').show();
            $('#TotalPaid').text(FormatedText);
        }


        function PerformGuestPaymentDetailDelete(paymentId) {
            PageMethods.PerformDeleteGuestPaymentByWebMethod(paymentId, OnPerformDeleteGuestPaymentDetailsSucceeded, OnPerformDeleteGuestPaymentDetailsFailed);
            return false;
        }
        function OnPerformDeleteGuestPaymentDetailsSucceeded(result) {
            $("#ReservationDetailGrid").html(result);
            GetTotalPaidAmount();
            return false;
        }
        function OnPerformDeleteGuestPaymentDetailsFailed(error) {
        }

        function ClearDetailsPart() {

            $("#<%=txtReceiveLeadgerAmount.ClientID %>").val('');

            $("#<%=txtCardNumber.ClientID %>").val('');
            $("#<%=ddlCardType.ClientID %>").val('a');
            $("#<%=txtExpireDate.ClientID %>").val('');
            $("#<%=txtCardHolderName.ClientID %>").val('');

            $("#<%=txtChecqueNumber.ClientID %>").val('');
        }

        function OnLoadSalesDetailGridViewSucceeded(result) {
            $('#productDetailGrid').html(result);
            GetTotalPaidAmount();
            return false;
        }

        function OnLoadSalesDetailGridViewFailed(error) {
        }

        function OnFillCustomerInfoSucceeded(result) {
            var txtName = '<%=txtName.ClientID%>'
            var ddlCustomerType = '<%=ddlCustomerType.ClientID%>'
            var txtPhone = '<%=txtPhone.ClientID%>'
            var txtEmail = '<%=txtEmail.ClientID%>'

            var txtWebAddress = '<%=txtWebAddress.ClientID%>'
            var txtAddress = '<%=txtAddress.ClientID%>'



            $('#' + txtName).val(result.Name);
            $('#' + ddlCustomerType).val(result.CustomerType)
            $('#' + txtPhone).val(result.Phone);
            $('#' + txtEmail).val(result.Email);
            $('#' + txtWebAddress).val(result.WebAddress);
            $('#' + txtAddress).val(result.Address);


            return false;
        }

        function OnFillCustomerInfoFailed(error) {
            alert(error.get_message());
        }

        function OnSaveProductDetailsSucceeded(result) {
            $('#productDetailGrid').html(result);


            var txtSerialNumber = '<%=txtSerialNumber.ClientID%>'
            $('#' + txtSerialNumber).val("");



            var rowCount = $('#ProductDetailGrid tr').length;
            if (rowCount > 1) {
                $('#ContentPlaceHolder1_ddlFrequency').attr('disabled', true);
            }
            else {
                $('#ContentPlaceHolder1_ddlFrequency').attr('disabled', false);
            }
            var ddlCurrency = '<%=ddlCurrency.ClientID%>'
            var Currency = $('#' + ddlCurrency).val();
            $('#btnOwnerDetails').val("Add");
            PageMethods.GetCalculatedGrandTotal(Currency, OnGetCalculatedGrandTotalSucceeded, OnGetCalculatedGrandTotalFailed);
        }
        function OnSaveProductDetailsFailed(error) {
            MessagePanelShow();
            $('#ContentPlaceHolder1_lblMessage').text('Please Select Item Name.');
        }

        function OnGetCalculatedGrandTotalSucceeded(result) {
            var txtSalesAmount = '<%=txtSalesAmount.ClientID%>'
            $('#' + txtSalesAmount).val(result);

            var txtGrandTotal = '<%=txtGrandTotal.ClientID%>'
            $('#' + txtGrandTotal).val(result);
            ShowOrHideBillPreveiw();
            PerformClearProductDetails();
            IsEnableSiteInformationCheckBox();

        }

        function OnGetCalculatedGrandTotalFailed(error) {
        }


        function PerformProductDetailDelete(detailId) {
            PageMethods.DeleteProductDetails(detailId, OnSaveProductDetailsSucceeded, OnSaveProductDetailsFailed);
        }


        function PerformClearProductDetails() {

            var ddlServiceType = '<%=ddlServiceType.ClientID%>'
            $('#' + ddlServiceType).val('0');

            var ddlProductId = '<%=ddlProductId.ClientID%>'
            $('#' + ddlProductId).val('0');

            var txtUnitPriceLocal = '<%=txtUnitPriceLocal.ClientID%>'
            $('#' + txtUnitPriceLocal).val('');

            var txtUnit = '<%=txtUnit.ClientID%>'
            $('#' + txtUnit).val('1');

            var detailId = '<%=detailId.ClientID%>'
            $('#' + detailId).val('');
            LoadManufacturer("Product");
            LoadProductCategory("Product");

            var txtItemName = '<%=txtItemName.ClientID%>'
            $('#' + txtItemName).val("");

            $('#ContentPlaceHolder1_ddlProductId').attr('disabled', true);
            $('#ContentPlaceHolder1_ddlServiceId').attr('disabled', true);

            $('#ContentPlaceHolder1_ddlProductId').val('0');
            $('#ContentPlaceHolder1_ddlServiceId').val('0');
            $('#ContentPlaceHolder1_ddlServiceBundleId').val('0');


            $('#ProductDropDownList').show();
            $('#ServiceDropDownList').hide();
            $('#ServiceBundleDropDownList').hide();
            $('#ProductPanel').show();
            $('#ItemNamePanel').show();
            $('#ServicePanel').hide();
            $('#BundlePanel').hide();
            $('#categoryPanel').show();


        }

        function ValidateMasterForm() {
            var isValid = true;
            var lblMessage = '<%=lblMessage.ClientID%>'


            var txtName = '<%=txtName.ClientID%>'
            var ddlCustomerType = '<%=ddlCustomerType.ClientID%>'
            var txtPhone = '<%=txtPhone.ClientID%>'
            var txtEmail = '<%=txtEmail.ClientID%>'
            var txtWebAddress = '<%=txtWebAddress.ClientID%>'
            var txtAddress = '<%=txtAddress.ClientID%>'


            var chkIsSiteEnable = '<%=chkIsSiteEnable.ClientID%>'
            var txtSiteName = '<%=txtSiteName.ClientID%>'
            var txtSiteContactPerson = '<%=txtSiteContactPerson.ClientID%>'
            var txtSitePhoneNumber = '<%=txtSitePhoneNumber.ClientID%>'
            var txtSiteEmail = '<%=txtSiteEmail.ClientID%>'






            if ($('#' + txtName).val() == "") {
                $('#' + lblMessage).text("Please Fill the Customer Name.");
                MessagePanelShow();
                isValid = false;
            }
            else if ($('#' + ddlCustomerType).val() == 0) {
                $('#' + lblMessage).text("Please Select the Customer Type.");
                MessagePanelShow();
                isValid = false;
            }
            else if ($('#' + txtPhone).val() == "") {
                $('#' + lblMessage).text("Please Fill the Customer Phone.");
                MessagePanelShow();
                isValid = false;
            }
            else if ($('#' + chkIsSiteEnable).is(':checked')) {

                if ($('#' + txtSiteName).val() == "") {
                    $('#' + lblMessage).text("Please Fill the Site Name.");
                    MessagePanelShow();
                    isValid = false;
                }
                else if ($('#' + txtSiteAddress).val() == "") {
                    $('#' + lblMessage).text("Please Fill the Site Address");
                    MessagePanelShow();
                    isValid = false;
                }
                else if ($('#' + txtSiteContactPerson).val() == "") {
                    $('#' + lblMessage).text("Please Fill the Site ContactPerson.");
                    MessagePanelShow();
                    isValid = false;
                }
                else if ($('#' + txtSitePhoneNumber).val() == "") {
                    $('#' + lblMessage).text("Please Fill the Site Phone Numbe.");
                    MessagePanelShow();
                    isValid = false;
                }
                else if ($('#' + txtSiteEmail).val() == "") {
                    $('#' + lblMessage).text("Please Fill the Site Email.");
                    MessagePanelShow();
                    isValid = false;
                }

            }

            if (isValid == true) {
                return true;
            }
            else {
                return false;
            }
        }


        function ShowOrHideBillPreveiw() {
            var txtGrandTotal = '<%=txtGrandTotal.ClientID%>'
            if ($('#' + txtGrandTotal).val() > 0) {
                $('#btnBillPreview').show();
            }
            else {
                $('#btnBillPreview').hide();
            }
        }

        function IsEnableSiteInformationCheckBox() {
            PageMethods.IsEnableSiteInformationCheckBox(IsEnableSiteInformationCheckBoxSucceeded, IsEnableSiteInformationCheckBoxFailed);
        }

        function IsEnableSiteInformationCheckBoxSucceeded(result) {
            if (result == 1) {

                $('#ContentPlaceHolder1_chkIsSiteEnable').attr('checked', true);
                $('#SitePanel').show();
            }
            else {
                $('#ContentPlaceHolder1_chkIsSiteEnable').attr('checked', false);

                $('#SitePanel').hide();
            }

            ShowAndHideSitePanelByBandwidth();

        }
        function IsEnableSiteInformationCheckBoxFailed(error) {

        }


        function ShowAndHideSitePanelByBandwidth() {
            var txtBandwidthType = '<%=txtBandwidthType.ClientID%>'
            if ($('#' + txtBandwidthType).val() == "True") {
             
                $('#SitePanel').show();
            }
            else {
        
                $('#SitePanel').hide();

            }
        }

    </script>
    <style>
        .mycheckbox input[type="checkbox"]
        {
            margin-right: 10px;
            padding-top: 5px;
        }
    </style>
    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
        <asp:HiddenField ID="txtCardValidation" runat="server"></asp:HiddenField>
    </div>
    <div style="height: 45px">
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Sales Info</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Sales</a></li>
            <%--<li id="C" runat="server"><a href="#tab-3">Title 3</a></li>--%>
        </ul>
        <div id="tab-1">
            <%-- <div id="EntryPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Sales Information</a>
                <div class="HMBodyContainer">--%>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <input type="hidden" value="0" id="hfIsSerializableProduct" />
                    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="HiddenFrequencyId" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="txtBandwidthType" runat="server"></asp:HiddenField>
                    <asp:Label ID="lblCustomerId" runat="server" Text="Customer Name"></asp:Label>
                    <span class="MandatoryField">*</span>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlCustomerId" runat="server" CssClass="customLargeDropDownSize"
                        TabIndex="1">
                    </asp:DropDownList>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblSalesDate" runat="server" Text="Start/Activation Date"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:TextBox ID="txtSalesDate" CssClass="customLargeTextBoxSize" runat="server" TabIndex="2"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div id="CustomerEntry">
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:HiddenField ID="txtCustomerId" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hiddenUnitPriceLocal" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hiddenUnitPriceUSD" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblName" runat="server" Text="Customer Name"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtName" TabIndex="3" runat="server" CssClass="ThreeColumnTextBox"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblCustomerType" runat="server" Text="Customer Type"></asp:Label>
                        <span class="MandatoryField">*</span>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:DropDownList ID="ddlCustomerType" runat="server" TabIndex="4">
                        </asp:DropDownList>
                    </div>
                    <div class="divBox divSectionRightLeft">
                        <asp:Label ID="lblPhone" runat="server" Text="Phone"></asp:Label>
                    </div>
                    <div class="divBox divSectionRightRight">
                        <asp:TextBox ID="txtPhone" runat="server" TabIndex="5"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtEmail" TabIndex="6" CssClass="ThreeColumnTextBox" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblWebAddress" runat="server" Text="Web Address"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtWebAddress" TabIndex="7" CssClass="ThreeColumnTextBox" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <div class="divSection">
                    <div class="divBox divSectionLeftLeft">
                        <asp:Label ID="lblAddress" runat="server" Text="Address"></asp:Label>
                    </div>
                    <div class="divBox divSectionLeftRight">
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="ThreeColumnTextBox" TextMode="MultiLine"
                            TabIndex="8"></asp:TextBox>
                    </div>
                </div>
                <div class="divClear">
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblCurrency" runat="server" Text="Preffered Currency"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="customSmallDropDownSize"
                        TabIndex="9">
                    </asp:DropDownList>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblFrequency" runat="server" Text="Frequency"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:DropDownList ID="ddlFrequency" runat="server" TabIndex="10">
                        <asp:ListItem Value="One Time">One Time</asp:ListItem>
                        <asp:ListItem>Monthly</asp:ListItem>
                        <asp:ListItem>Quaterly</asp:ListItem>
                        <asp:ListItem>Half Yearly</asp:ListItem>
                        <asp:ListItem>Yearly</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div id="Commissionformation" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Item Detail Information
                </a>
                <div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblItemType" runat="server" Text="Item Type"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="customLargeDropDownSize"
                                TabIndex="11">
                                <asp:ListItem>Product</asp:ListItem>
                                <asp:ListItem>Service</asp:ListItem>
                                <asp:ListItem>Service Bundle</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div id="categoryPanel">
                            <div class="divBox divSectionRightLeft">
                                <asp:Label ID="lblProductCategory" runat="server" Text="Category"></asp:Label>
                            </div>
                            <div class="divBox divSectionRightRight">
                                <asp:DropDownList ID="ddlProductCategory" runat="server" CssClass="customLargeDropDownSize"
                                    TabIndex="12">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection" id="ProductPanel">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblManufaturer" runat="server" Text="Manufaturer/Brand"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlManufacturer" runat="server" CssClass="customLargeDropDownSize"
                                TabIndex="13">
                            </asp:DropDownList>
                        </div>
                        <div>
                            <div class="divBox divSectionRightLeft">
                                <asp:Label ID="lblItemId" runat="server" Text="Code/ Model"></asp:Label>
                                <span class="MandatoryField">*</span>
                            </div>
                            <div class="divBox divSectionRightRight">
                                <div id="ProductDropDownList">
                                    <asp:HiddenField ID="txtHiddenProductId" runat="server" />
                                    <asp:HiddenField ID="txtCurrentId" runat="server" />
                                    <asp:HiddenField ID="txtHiddenProductName" runat="server" />
                                    <asp:DropDownList ID="ddlProductId" runat="server" CssClass="customLargeDropDownSize"
                                        TabIndex="14">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection" id="ServicePanel">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="Label2" runat="server" Text="Item Name"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <div id="ServiceDropDownList">
                                <asp:DropDownList ID="ddlServiceId" runat="server" CssClass="ThreeColumnDropDownList"
                                    TabIndex="15">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection" id="BundlePanel">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="Label6" runat="server" Text="Item Name"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <div id="ServiceBundleDropDownList">
                                <asp:DropDownList ID="ddlServiceBundleId" runat="server" CssClass="ThreeColumnDropDownList"
                                    TabIndex="16">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection" id="ItemNamePanel">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblItemName" runat="server" Text="Item Name"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtItemName" runat="server" CssClass="InnerThreeColumnDropDownList" TabIndex="17"
                                ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div id="IsSeparateSalesInventoryDiv" style="display: none;">
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:HiddenField ID="hfIsSeparateSalesInventory" runat="server" />
                                <asp:Label ID="lblSerialNumber" runat="server" Text="Serial Number"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtSerialNumber" TabIndex="18" runat="server" CssClass="ThreeColumnTextBox"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblUnitPriceLocal" runat="server" Text="Unit Price"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <div id="UnitPriceLocalDiv" style="display: none">
                                <asp:TextBox ID="txtUnitPriceLocal" CssClass="customLargeTextBoxSize" runat="server"
                                    TabIndex="19"></asp:TextBox>
                            </div>
                            <div id="UnitPriceUSDDiv" style="display: none">
                                <asp:TextBox ID="txtUnitPriceUSD" CssClass="customLargeTextBoxSize" runat="server"
                                    TabIndex="20"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblUnit" runat="server" Text="Quantity"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtUnit" CssClass="CustomTextBox" runat="server" TabIndex="21">1</asp:TextBox>
                            <input id="btnOwnerDetails" type="button" tabindex="22" value="Add" class="TransactionalButton btn btn-primary" />
                            <asp:Label ID="lblHiddenOwnerDetailtId" runat="server" Visible="False"></asp:Label>
                            <asp:HiddenField ID="detailId" runat="server" />
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divClear">
                    </div>
                    <div style="text-align: center;">
                        <div id="productDetailGrid">
                        </div>
                    </div>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblSalesAmount" runat="server" Text="Sales Amount"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtSalesAmount" runat="server" TabIndex="23" CssClass="customLargeTextBoxSize">0</asp:TextBox>
                </div>
                <div class="divBox divSectionRightLeft">
                    <asp:Label ID="lblVatAmount" runat="server" Text="Vat Amount"></asp:Label>
                </div>
                <div class="divBox divSectionRightRight">
                    <asp:TextBox ID="txtVatAmount" runat="server" TabIndex="24" CssClass="customLargeTextBoxSize" ReadOnly="True">0</asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtGrandTotal" TabIndex="25" runat="server" CssClass="customLargeTextBoxSize">0</asp:TextBox>
                </div>
                <div class="divBox divSectionRightLeft">
                </div>
                <div class="divBox divSectionRightRight">
                    <input type="button" id="btnBillPreview"  tabindex="26" value="Bill Preview" class="TransactionalButton btn btn-primary" />
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="divSection" id="CheckboxPanel" style="display: none">
                <div class="divBox divSectionLeftLeft">
                </div>
                <div class="divBox divSectionLeftRight">
                    <div style="float: left">
                        <asp:CheckBox ID="chkIsSiteEnable" runat="Server" TabIndex="27" Text="Enable Site Information."
                            Font-Bold="true" CssClass="mycheckbox" TextAlign="right" />
                    </div>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div id="SitePanel">
                <div id="SiteInformation" class="block">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Site Detail Information
                    </a>
                    <div class="HMBodyContainer">
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblSiteInformation" runat="server" Text="Site Information"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:DropDownList ID="ddlSiteInformation" runat="server" CssClass="ThreeColumnDropDownList"
                                    TabIndex="28">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblSiteName" runat="server" Text="Site Name"></asp:Label>
                                <span class="MandatoryField">*</span>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtSiteName" runat="server" TabIndex="29" CssClass="ThreeColumnTextBox">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblSiteAddress" runat="server" Text="Site Address"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtSiteAddress" runat="server" TabIndex="30" TextMode="MultiLine" CssClass="ThreeColumnTextBox">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblSiteContactPerson" runat="server" Text="Contact Person"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtSiteContactPerson" TabIndex="31" runat="server" CssClass="ThreeColumnTextBox">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblSitePhoneNumber" runat="server" Text="Phone Number"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtSitePhoneNumber" TabIndex="32" runat="server" CssClass="ThreeColumnTextBox">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblSiteEmail" runat="server" Text="Email"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:TextBox ID="txtSiteEmail" TabIndex="33" runat="server" CssClass="ThreeColumnTextBox">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
                <%--<div class="divFullSectionWithTwoDvie">--%>
                <div class="divBoxWithoutLeftRightPadding divSectionLeftRightSameWidthFor50Percent">
                    <div id="MonthlySalaryDateSchedulePanel" class="block">
                        <a href="#page-stats" class="block-heading" data-toggle="collapse">Billing Information
                        </a>
                        <div class="HMBodyContainer">
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblBillingInformarion" runat="server" Text="Billing Informarion"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:DropDownList ID="ddlBillingInformarion" runat="server" TabIndex="34">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblBillingContactPerson" runat="server" Text="Contact Person"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtBillingContactPerson" TabIndex="35" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblBillingPersonDepartment" runat="server" Text="Department"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtBillingPersonDepartment" TabIndex="36" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblBillingPersonDesignation" runat="server" Text="Designation"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtBillingPersonDesignation" TabIndex="37" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblBillingPersonPhone" runat="server" Text="Phone"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtBillingPersonPhone" TabIndex="38" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblBillingPersonEmail" runat="server" Text="E-Mail"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtBillingPersonEmail" TabIndex="39" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="divBoxWithoutLeftRightPadding divSectionLeftRightSameWidthFor50Percent">
                    <div id="EmployeeBasicPanel" class="block">
                        <a href="#page-stats" class="block-heading" data-toggle="collapse">Technical Information
                        </a>
                        <div class="HMBodyContainer">
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblTechnicalInformation" runat="server" Text="Technical Information"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:DropDownList ID="ddlTechnicalInformation" runat="server" TabIndex="40">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblTechnicalContactPerson" runat="server" Text="Contact Person"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtTechnicalContactPerson" TabIndex="41" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblTechnicalPersonDepartment" runat="server" Text="Department"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtTechnicalPersonDepartment" TabIndex="42" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblTechnicalPersonDesignation" runat="server" Text="Designation"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtTechnicalPersonDesignation" TabIndex="43" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblTechnicalPersonPhone" runat="server" Text="Phone"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtTechnicalPersonPhone" TabIndex="44" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblTechnicalPersonEmail" runat="server" Text="E-Mail"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtTechnicalPersonEmail" TabIndex="45" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                        </div>
                    </div>
                </div>
                <%-- </div>--%>
                <%--</div>
               </div>--%>
            </div>
            <div class="divClear">
            </div>
            <div class="divClear">
            </div>
            <div id="PaymentDetailsInformation">
                <div class="block" style="min-height: 160px">
                    <a href="#page-stats" class="block-heading" data-toggle="collapse">Payment Information
                    </a>
                    <div class="childDivSectionDivBlockBody">
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblPayMode" runat="server" Text="Payment Mode"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <asp:DropDownList ID="ddlPayMode" runat="server" TabIndex="46">
                                    <asp:ListItem>Cash</asp:ListItem>
                                    <asp:ListItem>Card</asp:ListItem>
                                    <asp:ListItem>Cheque</asp:ListItem>
                                    <asp:ListItem>Credit</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="divBox divSectionRightLeft">
                                <asp:Label ID="lblReceiveLeadgerAmount" runat="server" Text="Receive Amount"></asp:Label>
                            </div>
                            <div class="divBox divSectionRightRight">
                                <asp:TextBox ID="txtReceiveLeadgerAmount" runat="server" TabIndex="47"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection">
                            <div class="divBox divSectionLeftLeft">
                                <asp:Label ID="lblPaymentAccountHead" runat="server" Text="Account Head"></asp:Label>
                            </div>
                            <div class="divBox divSectionLeftRight">
                                <div id="CashPaymentAccountHeadDiv">
                                    <asp:DropDownList ID="ddlCashReceiveAccountsInfo" TabIndex="48" runat="server" CssClass="childDivSectionDivThreeColumnDropDownList">
                                    </asp:DropDownList>
                                </div>
                                <div id="BankPaymentAccountHeadDiv" style="display: none;">
                                    <asp:DropDownList ID="ddlCardReceiveAccountsInfo" runat="server" TabIndex="49" CssClass="childDivSectionDivThreeColumnDropDownList">
                                    </asp:DropDownList>
                                </div>
                                <div id="CompanyPaymentAccountHeadDiv" style="display: none;">
                                    <asp:DropDownList ID="ddlCompanyPaymentAccountHead" runat="server" TabIndex="50" CssClass="childDivSectionDivThreeColumnDropDownList">
                                    </asp:DropDownList>
                                </div>
                                <div id="PaidByOtherRoomDiv" style="display: none">
                                    <asp:DropDownList ID="ddlPaidByRegistrationId" runat="server" TabIndex="51" CssClass="childDivSectionDivThreeColumnDropDownList">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div id="ChecquePaymentAccountHeadDiv" style="display: none;">
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblBankId" runat="server" Text="Bank Name"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:DropDownList ID="ddlBankId" runat="server" AutoPostBack="True" TabIndex="52">
                                    </asp:DropDownList>
                                </div>
                                <div class="divBox divSectionRightLeft">
                                    <asp:Label ID="lblChecqueNumber" runat="server" Text="Cheque Number"></asp:Label>
                                </div>
                                <div class="divBox divSectionRightRight">
                                    <asp:TextBox ID="txtChecqueNumber" runat="server" TabIndex="53"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div id="CardPaymentAccountHeadDiv" style="display: none;">
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblBankName" runat="server" Text="Bank Name"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:DropDownList ID="ddlBankName" runat="server" TabIndex="54" CssClass="ThreeColumnDropDownList"
                                        AutoPostBack="false">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="Label3" runat="server" Text="Card Type"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:DropDownList ID="ddlCardType" runat="server" TabIndex="55" CssClass="tdLeftAlignWithSize">
                                        <asp:ListItem Value="a">American Express</asp:ListItem>
                                        <asp:ListItem Value="m">Master Card</asp:ListItem>
                                        <asp:ListItem Value="v">Visa Card</asp:ListItem>
                                        <asp:ListItem Value="d">Discover Card</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="divBox divSectionRightLeft">
                                    <asp:Label ID="lblCardNumber" runat="server" Text="Card Number"></asp:Label>
                                </div>
                                <div class="divBox divSectionRightRight">
                                    <asp:TextBox ID="txtCardNumber" TabIndex="56" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="Label4" runat="server" Text="Expiry Date"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:TextBox ID="txtExpireDate" TabIndex="57" CssClass="datepicker" runat="server"></asp:TextBox>
                                </div>
                                <div class="divBox divSectionRightLeft">
                                    <asp:Label ID="Label5" runat="server" Text="Card Holder Name"></asp:Label>
                                </div>
                                <div class="divBox divSectionRightRight">
                                    <asp:TextBox ID="txtCardHolderName" TabIndex="58" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="divClear">
                            </div>
                        </div>
                        <div id="CreditPaymentAccountHeadDiv">
                            <div class="divClear">
                            </div>
                            <div class="divSection">
                                <div class="divBox divSectionLeftLeft">
                                    <asp:Label ID="lblCreditAccountHead" runat="server" Text="Credit Account Head"></asp:Label>
                                </div>
                                <div class="divBox divSectionLeftRight">
                                    <asp:DropDownList ID="ddlCreditAccountHead" TabIndex="59" runat="server" CssClass="ThreeColumnDropDownList">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="divClear">
                        </div>
                        <div class="divSection" style="padding-left: 10px;">
                            <%--Right Left--%>
                            <input type="button" id="btnAddDetailGuestPayment" value="Add" tabindex="60" class="TransactionalButton btn btn-primary"
                                onclientclick="javascript: return ValidateForm();" />
                            <asp:Label ID="lblHiddenIdDetailGuestPayment" runat="server" Text='' Visible="False"></asp:Label>
                        </div>
                        <div class="divClear">
                        </div>
                        <div id="GuestPaymentDetailGrid" class="childDivSection">
                        </div>
                        <div id="TotalPaid" class="totalAmout">
                        </div>
                        <div class="divClear">
                        </div>
                        <div id="dueTotal" class="totalAmout">
                        </div>
                        <div class="divClear">
                        </div>
                        <div>
                            <asp:Label ID="AlartMessege" runat="server" Style="color: Red;" Text='Grand Total and Payment Amount is not Equal.'
                                CssClass="totalAmout"></asp:Label>
                        </div>
                        <div class="divClear">
                        </div>
                    </div>
                </div>
                <div class="divClear">
                </div>
            </div>
            <div class="divSection">
                <div class="divBox divSectionLeftLeft">
                    <asp:Label ID="Label1" runat="server" Text="Comments"></asp:Label>
                </div>
                <div class="divBox divSectionLeftRight">
                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="ThreeColumnTextBox" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
            <div class="divClear">
            </div>
            <div class="HMContainerRowButton">
                <%--Right Left--%>
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary"
                    TabIndex="79" OnClick="btnSave_Click" OnClientClick="javascript: return ValidateMasterForm();" />
                <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary"
                    TabIndex="80" OnClick="btnCancel_Click" />
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchPMSales" class="block" style="">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Sales Information</a>
                <div class="HMBodyContainer">
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:HiddenField ID="txtHiddenSalesId" runat="server" />
                            <asp:Label ID="lblCustomerName" runat="server" Text="Customer Name"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:DropDownList ID="ddlCustomerName" runat="server" CssClass="customLargeDropDownSize"
                                TabIndex="61">
                            </asp:DropDownList>
                        </div>
                        <div class="divBox divSectionRightLeft">
                            <asp:Label ID="lblBillNo" runat="server" Text="Invoice No."></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtBillNo" runat="server" TabIndex="62"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divSection">
                        <div class="divBox divSectionLeftLeft">
                            <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>
                        </div>
                        <div class="divBox divSectionLeftRight">
                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="63"></asp:TextBox>
                        </div>
                        <div class="divBox
    divSectionRightLeft">
                            <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>
                        </div>
                        <div class="divBox divSectionRightRight">
                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="64"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="divClear">
                    </div>
                    <div class="HMContainerRowButton">
                        <%--Right Left--%>
                        <asp:Button ID="btnSearch" runat="server" TabIndex="65" Text="Search" OnClick="btnSearch_Click"
                            CssClass="TransactionalButton btn btn-primary"  />
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="block">
                <a href="#page-stats" class="block-heading" data-toggle="collapse">Search Information
                </a>
                <div class="block-body collapse in">
                    <asp:GridView ID="gvRoomOwner" Width="100%" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" GridLines="None" AllowSorting="True" ForeColor="#333333" PageSize="20"
                        OnRowCommand="gvRoomOwner_RowCommand" OnRowDataBound="gvRoomOwner_RowDataBound">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("SalesId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sales Date" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvSalesDate" runat="server" Text='<%# GetStringFromDateTime(Convert.ToDateTime(Eval("SalesDate")))%>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="InvoiceNumber" HeaderText="Invoice Number" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-Width="45%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandArgument='<%# bind("SalesId") %>'
                                        CommandName="CmdEdit" ImageUrl="~/Images/edit.png" Text="" AlternateText="Edit"
                                        ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandArgument='<%# bind("SalesId") %>'
                                        CommandName="CmdDelete" ImageUrl="~/Images/delete.png" OnClientClick="return confirm('Do you want to save?');"
                                        Text="" AlternateText="Delete" ToolTip="Delete" />
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
        </div>
    </div>
    <script type="text/javascript">
        var ddlServiceType = '<%=ddlServiceType.ClientID%>'
        var x = '<%=isMessageBoxEnable%>';
        var IsService = '<%=IsService%>';

        if (x > -1) {
            MessagePanelShow();
            if (x == 2) {
                $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
            }
        }
        else {
            MessagePanelHide();
        }

        var hfIsSeparateSalesInventory = '<%=hfIsSeparateSalesInventory.ClientID%>'
        if ($('#' + hfIsSeparateSalesInventory).val() == "Yes") {
            $('#IsSeparateSalesInventoryDiv').hide();
        }
        else {
            $('#IsSeparateSalesInventoryDiv').show();
        }
    </script>
</asp:Content>
