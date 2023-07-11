toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": true,
    "progressBar": true,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "10000",
    "timeOut": "6000",
    "extendedTimeOut": "10000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}

jconfirm.defaults = {
    title: 'Confirmation!',
    titleClass: '',
    type: 'default',
    typeAnimated: true,
    draggable: true,
    dragWindowGap: 15,
    dragWindowBorder: true,
    animateFromElement: true,
    smoothContent: true,
    content: 'Are you sure to continue?',
    buttons: {},
    defaultButtons: {
        ok: {
            action: function () {
            }
        },
        close: {
            action: function () {
            }
        },
    },
    contentLoaded: function (data, status, xhr) {
    },
    icon: '',
    lazyOpen: false,
    bgOpacity: null,
    theme: 'bootstrap',
    animation: 'zoom',
    closeAnimation: 'zoom',
    animationSpeed: 400,
    animationBounce: 1,
    rtl: false,
    container: 'body',
    containerFluid: false,
    backgroundDismiss: false,
    backgroundDismissAnimation: 'glow',
    autoClose: false,
    closeIcon: null,
    closeIconClass: false,
    watchInterval: 100,
    columnClass: 'col-md-4 col-md-offset-4 col-sm-6 col-sm-offset-3 col-xs-10 col-xs-offset-1',
    boxWidth: '50%',
    scrollToPreviousElement: true,
    scrollToPreviousElementAnimate: true,
    useBootstrap: true,
    offsetTop: 40,
    offsetBottom: 40,
    bootstrapClasses: {
        container: 'container',
        containerFluid: 'container-fluid',
        row: 'row',
    },
    onContentReady: function () { },
    onOpenBefore: function () { },
    onOpen: function () { },
    onClose: function () { },
    onDestroy: function () { },
    onAction: function () { }
};

function Disable_FuntionKey(e) {
    if ((e.which || e.keyCode) == 116) {
        e.preventDefault();
    }
    else if ((e.which || e.keyCode) == 117) {
        e.preventDefault();
    }
}

function toFixed(num, precision) {
    return parseFloat((+(Math.round(+(num + 'e' + precision)) + 'e' + -precision)).toFixed(precision));
}

function GetDateTimeFromString(inputDate) {
    var array = inputDate.split("/");
    var date = new Date(array[2], array[0] - 1, array[1]);
    return date;
}
function GetStringFromDateTime(inputDate) {
    //var format = "dd/MM/yyyy";
    var innboardFormat = innBoarDateFormat.replace('mm', 'MM');
    innboardFormat = innboardFormat.replace('yy', 'yyyy');
    var dateTime = new Date(inputDate);
    return dateTime.format(innboardFormat);
}
function GetTimeFromDateTime(inputDate) {
    var dateTime = new Date(inputDate);
    return dateTime.format(UserInfoFromDB.TimeFormat);
}
$(document).ready(function () {
    //alert(innBoarDateFormat);
    $('.datepicker').datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "-70:+30",
        dateFormat: innBoarDateFormat
    });
});

function CheckDateTimeValidation(ValidationType, ValidationControl, messageText) {
    if (ValidationType == "Date") {

    }
    else if (ValidationType == "Time") {
        var a, b, c, f, err = 0;
        a = $(ValidationControl).val();
        if (a.length != 5 && is12HourFormat == false) err = 1;
        b = a.substring(0, 2);
        c = a.substring(2, 3);
        f = a.substring(3, 5);
        if (/\D/g.test(b)) err = 1; //not a number
        if (/\D/g.test(f)) err = 1;
        if (b < 0 || b > 23) err = 1;
        if (f < 0 || f > 59) err = 1;
        if (c != ':') err = 1;
        if (err == 1) {
            toastr.warning("That is not a valid " + messageText + ".\nPlease re-enter in format XX:XX ");
            $(ValidationControl).focus();
            return false;
        }
    }

    return true;
}


function CheckDiscountValidationForFixedRPercentage(discountType, discountControl, compareAmountControl, messageText) {
    if ($(discountType).val() == "Fixed") {
        if (parseFloat($(discountControl).val()) > parseFloat($(compareAmountControl).val())) {
            toastr.warning(messageText + " Can Not Greater Than " + $(compareAmountControl).val() + " Amount.");
            $(discountControl).val("0");
            return false;
        }
    }
    else if ($(discountType).val() == "Percentage") {
        if (parseFloat($(discountControl).val()) > 100.00) {
            toastr.warning(messageText + " Can Not Greater Than 100%.");
            $(discountControl).val("0");
            return false;
        }
        else if (parseFloat($(discountControl).val()) < 0) {
            toastr.warning(messageText + " Can Not Less Than Zero.");
            $(discountControl).val("0");
            return false;
        }
    }

    return true;
}
function PopulateControlWithValueNTextField(list, control, defaultSelectedValue, TextField = "Text", ValueField = "Value") {
    if (list.length > 0) {
        control.removeAttr("disabled");
        control.empty().append('<option selected="selected" value="0">' + defaultSelectedValue + '</option>');
        $.each(list, function () {
            control.append($("<option></option>").val(this[ValueField]).html(this[TextField]));
        });
    }
    else {
        control.empty().append('<option selected="selected" value="0">Not available<option>');
    }
}
function PopulateControl(list, control, defaultSelectedValue) {
    if (list.length > 0) {
        control.removeAttr("disabled");
        if(list.length > 1)
        {
            control.empty().append('<option selected="selected" value="0">' + defaultSelectedValue + '</option>');
        }
        else
        {
            control.empty();
        }
        
        $.each(list, function () {
            control.append($("<option></option>").val(this['Value']).html(this['Text']));
        });
    }
    else {
        control.empty().append('<option selected="selected" value="0">Not available<option>');
    }
}
function PopulateControlWithOutDefault(list, control, defaultSelectedValue) {
    if (list.length > 0) {
        control.empty().append('<option selected="selected" value="0">' + defaultSelectedValue + '</option>');
        $.each(list, function () {
            control.append($("<option></option>").val(this['Value']).html(this['Text']));
        });
    }
    else {
        control.empty().append('<option selected="selected" value="0">Not available</option>');
    }
}
function alert(message, title, buttonText) {

    buttonText = (buttonText == undefined) ? "Ok" : buttonText;
    title = (title == undefined) ? "The page says:" : title;

    var div = $('<div>');
    div.html(message);
    div.attr('title', title);
    div.dialog({
        autoOpen: true,
        modal: true,
        draggable: true,
        overlayOpacity: .01,
        overlayColor: '#FFF',
        resizable: false,
        zIndex: 100000,
        buttons: [{
            text: buttonText,
            "class": 'TransactionalButton btn btn-primary',
            width: 90,
            click: function () {
                $(this).dialog("close");
                div.remove();
            }
        }]
    });
}

//function confirm(message, title) {
//    title = (title == undefined) ? "The page says:" : title;

//    $(document.createElement('div'))
//                .attr({ title: title, class: 'confirm' })
//                .html(message)
//                .dialog({
//                    buttons: {
//                        "Confirm": function () {
//                            $(this).dialog("close");
//                            return true;
//                        },
//                        "Cancel": function () {
//                            $(this).dialog("close");
//                            return false;
//                        }
//                    },
//                    close: function () {
//                        $(this).remove();
//                    },
//                    class: 'TransactionalButton btn btn-primary',
//                    draggable: true,
//                    modal: true,
//                    resizable: false,
//                    width: '200'
//                });
//}

function CommonHelper() { }

CommonHelper.ExactMatch = function () {
    $.extend($.expr[':'], {
        'textEquals': function (elem, i, match, array) {
            return $(elem).text().toLowerCase().match("^" + match[3].toLowerCase() + "$");
        }
    });
};

CommonHelper.MessageType = {
    Info: "Info",
    Success: "Success",
    Error: "Error"
};

CommonHelper.AlertMessage = function (messageModel) {

    var timeToDisplay = messageModel.TimeToDisplay;

    if (timeToDisplay == 0)
        timeToDisplay = 5000;

    if (messageModel.AlertType == "success") {
        toastr.success(messageModel.Message, "", { timeOut: timeToDisplay });
    }
    else if (messageModel.AlertType == "info") {
        toastr.info(messageModel.Message, "", { timeOut: timeToDisplay });
    }
    else if (messageModel.AlertType == "warning") {
        toastr.warning(messageModel.Message, "", { timeOut: timeToDisplay });
    }
    else if (messageModel.AlertType == "error") {
        toastr.error(messageModel.Message, "", { timeOut: timeToDisplay });
    }
    else { toastr.info(messageModel.Message, "", { timeOut: timeToDisplay }); }
}

CommonHelper.SpinnerOpen = function () {
    var $loading = $('#spinner').hide();
    $loading.show();
}

CommonHelper.SpinnerClose = function () {
    var $loading = $('#spinner').show();
    $loading.hide();
}

CommonHelper.SplitPascalCaseToWord = function (word) {
    var wordRe = /($[a-z])|[A-Z][^A-Z]+/g;
    return word.match(wordRe).join(" ");
}

CommonHelper.ApplyIntigerValidation = function () {
    $('.quantity').keydown(function (event) {

        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9
            || event.keyCode == 27 || event.keyCode == 13
            || (event.keyCode == 65 && event.ctrlKey === true)
            || (event.keyCode >= 35 && event.keyCode <= 39)) {
            return;
        } else {

            if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                event.preventDefault();
            }
        }
    });
};

CommonHelper.ApplyDecimalValidation = function () {
    $('.quantitydecimal').keypress(function (event) {
        if ((event.which != 46 || $(this).val().indexOf('.') != -1) &&
            ((event.which < 48 || event.which > 57) &&
                (event.which != 0 && event.which != 8))) {
            event.preventDefault();
        }

        var text = $(this).val();

        if ((text.indexOf('.') != -1) &&
            (text.substring(text.indexOf('.')).length > 2) &&
            (event.which != 0 && event.which != 8) &&
            ($(this)[0].selectionStart >= text.length - 2)) {
            event.preventDefault();
        }
    });
};

CommonHelper.ApplyDecimalValidationWithNegetiveValue = function () {
    $('.quantitynegativedecimal').keypress(function (event) {
        if ((event.which != 46 || $(this).val().indexOf('.') != -1) &&
            (event.which != 45 || $(this).val().indexOf('-') != -1) &&
            ((event.which < 48 || event.which > 57) &&
                (event.which != 0 && event.which != 8))) {
            event.preventDefault();
        }

        var text = $(this).val();

        if ((text.indexOf('.') != -1) &&
            (text.substring(text.indexOf('.')).length > 2) &&
            (event.which != 0 && event.which != 8) &&
            ($(this)[0].selectionStart >= text.length - 2)) {
            event.preventDefault();
        }
        if (event.which == 45 &&
            (event.which != 0 && event.which != 8) &&
            ($(this)[0].selectionStart >= 1)) {
            event.preventDefault();
        }
        
    });
};
CommonHelper.ApplyIntValidation = function () {
    $('.quantityint').keypress(function (event) {
        if ((event.which != 46 || $(this).val().indexOf('.') != -1) &&
            ((event.which < 48 || event.which > 57) &&
                (event.which != 0 && event.which != 8))) {
            event.preventDefault();
        }
    });
};

CommonHelper.IsVaildDate = function (date) {
    if (Date.parse(date)) {
        return true;
    }
    else
        return false;
}

CommonHelper.IsNumeric = function (input) {

    if ($.trim(input) == "")
        return false;

    return (input - 0) == input && input.length > 0;
}

CommonHelper.IsInt = function (n) {

    if ($.trim(n) == "")
        return false;

    var er = /^[0-9]+$/;
    return (er.test(n)) ? true : false;
}

CommonHelper.IsDecimal = function (n) {
    if ($.trim(n) == "")
        return false;

    var er = /^\d*\.?\d*$/;
    return (er.test(n)) ? true : false;
}

CommonHelper.IsDecimal2FloatPoint = function (n) {

    if ($.trim(n) == "")
        return false;

    var er = /^\d*\.?\d{0,2}$/;
    return (er.test(n)) ? true : false;
}

CommonHelper.IsLeadingZeroContains = function (n) {

    if ($.trim(n) == "")
        return false;

    var er = /^0+/;
    return (er.test(n)) ? true : false;
}

CommonHelper.IsLettersNumbersSpaceCommasPeriods = function (n) {

    if ($.trim(n) == "")
        return false;

    var er = /^[a-zA-Z0-9,.!? ]*$/;
    return (er.test(n)) ? true : false;
}

CommonHelper.WhiteSpaceRemove = function (removeFor) {
    removeFor = removeFor.replace(/ /g, '');
    return removeFor;
}

CommonHelper.SpecialCharacterRemove = function (removeFor) {
    removeFor = removeFor.replace(/[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi, '');;
    return removeFor;
}

CommonHelper.Decimal2Point = function (num) {
    return Number(num.toString().match(/^\d+(?:\.\d{0,2})?/))
};

CommonHelper.padLeft = function (nr, n, str) {
    return Array(n - String(nr).length + 1).join(str || '0') + nr;
}

CommonHelper.DateDifferenceInDays = function (dateFrom, dateTo) {
    dateFrom = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(dateFrom, innBoarDateFormat);
    dateTo = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(dateTo, innBoarDateFormat);

    var date1 = new Date(dateFrom);
    var date2 = new Date(dateTo);

    var timeDiff = Math.abs(date2.getTime() - date1.getTime());
    var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));

    return diffDays;
}

CommonHelper.DaysAdd = function (date, days) {

    if (date == "" || days == "") return;
    else if (CommonHelper.IsInt(days) == false) return;
    else if (CommonHelper.IsVaildDate(date) == false) return;

    Date.prototype.addDays = function (num) {
        var value = this.valueOf();
        value += 86400000 * num;
        return new Date(value);
    }

    var newDate = new Date(date);
    var resultDate = newDate.addDays(days);

    return resultDate;
}

CommonHelper.SecondsAdd = function (date, seconds) {

    if (date == "" || seconds == "") return;
    else if (CommonHelper.IsInt(days) == false) return;
    else if (CommonHelper.IsVaildDate(date) == false) return;

    Date.prototype.addSeconds = function (num) {
        var value = this.valueOf();
        value += 1000 * num;
        return new Date(value);
    }

    var newDate = new Date(date);
    var resultDate = newDate.addSeconds(seconds);

    return resultDate;
}

CommonHelper.MinutesAdd = function (date, minutes) {

    if (date == "" || minutes == "") return;
    else if (CommonHelper.IsInt(days) == false) return;
    else if (CommonHelper.IsVaildDate(date) == false) return;

    Date.prototype.addMinutes = function (num) {
        var value = this.valueOf();
        value += 60000 * num;
        return new Date(value);
    }

    var newDate = new Date(date);
    var resultDate = newDate.addMinutes(minutes);

    return resultDate;
}

CommonHelper.HoursAdd = function (date, hours) {

    if (date == "" || hours == "") return;
    else if (CommonHelper.IsInt(days) == false) return;
    else if (CommonHelper.IsVaildDate(date) == false) return;

    Date.prototype.addHours = function (num) {
        var value = this.valueOf();
        value += 3600000 * num;
        return new Date(value);
    }

    var newDate = new Date(date);
    var resultDate = newDate.addHours(hours);

    return resultDate;
}

CommonHelper.MonthsAdd = function (date, months) {

    if (date == "" || months == "") return;
    else if (CommonHelper.IsInt(days) == false) return;
    else if (CommonHelper.IsVaildDate(date) == false) return;

    Date.prototype.addMonths = function (num) {
        var value = new Date(this.valueOf());

        var mo = this.getMonth();
        var yr = this.getYear();

        mo = (mo + num) % 12;
        if (0 > mo) {
            yr += (this.getMonth() + num - mo - 12) / 12;
            mo += 12;
        }
        else
            yr += ((this.getMonth() + num - mo) / 12);

        value.setMonth(mo);
        value.setYear(yr);
        return value;
    }

    var newDate = new Date(date);
    var resultDate = newDate.addMonths(months);

    return resultDate;
}

CommonHelper.DateFormatToMMDDYYYY = function (strDate, delimeter) {

    var s = strDate.split(delimeter);

    if (innBoarDateFormat == 'dd/mm/yy') {
        strDate = s[1] + '/' + s[0] + '/' + s[2];
    }

    return strDate;
}
// format for moment JS 
CommonHelper.DateFormatToYYYYMMDD = function (strDate, delimeter) {

    var s = strDate.split(delimeter);
    var dateLength = s[0].length;
    var monthLength = s[1].length;
    var date = parseInt(s[0], 10);
    var month = parseInt(s[1], 10);
    if (date < 10 && dateLength == 1) {
        s[0] = '0' + s[0];
    }
    if (month < 10 && monthLength == 1) {
        s[1] = '0' + s[1];
    }
    if (innBoarDateFormat == 'dd/mm/yy') {
        strDate = s[2] + '-' + s[1] + '-' + s[0];
    }
    else if (innBoarDateFormat == 'mm/dd/yy') {
        strDate = s[2] + '-' + s[0] + '-' + s[1];
    }
    return strDate;
}

CommonHelper.DateFormatDDMMYYY = function (strDate) {

    var d = new Date(strDate);
    var curr_date = d.getDate();
    var curr_month = d.getMonth() + 1;
    var curr_year = d.getFullYear();

    var dt = curr_date + "/" + curr_month + "/" + curr_year;

    return dt;
}

CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY = function (strDate, dateFormat) {

    var currenDate = new Date();

    if (dateFormat == 'dd/mm/yy') {
        var datePart = strDate.split('/');
        currenDate = datePart[1] + "/" + datePart[0] + "/" + datePart[2];
    }
    else {
        currenDate = strDate;
    }

    return currenDate;
}

CommonHelper.DateFromStringToDisplay = function (strDate, dateFormat) {

    var dateStr = new Date(JSON.parse(strDate.replace('/Date(', '').replace(')/', '')));
    var dates = ((dateStr.getMonth() + 1) + '/' + dateStr.getDate() + '/' + dateStr.getFullYear());

    var currenDate = "";

    if (dateFormat == 'dd/mm/yy') {
        var datePart = dates.split('/');
        currenDate = datePart[1] + "/" + datePart[0] + "/" + datePart[2];
    }
    else {
        currenDate = dates;
    }

    return currenDate;
}

CommonHelper.DateFromDateTimeToDisplay = function (strDate, dateFormat) {

    var d = new Date(strDate);
    var dates = ((d.getMonth() + 1) + '/' + d.getDate() + '/' + d.getFullYear());

    var currenDate = "";

    if (dateFormat == 'dd/mm/yy') {
        var datePart = dates.split('/');
        currenDate = datePart[1] + "/" + datePart[0] + "/" + datePart[2];
    }
    else {
        currenDate = dates;
    }

    return currenDate;
}

CommonHelper.DateFormatDDMMMYYY = function (strDate) {
    var dateStr = new Date(JSON.parse(strDate.replace('/Date(', '').replace(')/', '')));
    var dates = (dateStr.getDate() + '/' + dateStr.getMonth() + '/' + dateStr.getFullYear());

    return dates;
}


CommonHelper.DateFormatMMDDYYY = function (strDate) {

    var d = new Date(strDate);
    var curr_date = d.getDate();
    var curr_month = d.getMonth() + 1;
    var curr_year = d.getFullYear();

    var dt = curr_month + "/" + curr_date + "/" + curr_year;

    return dt;
}

CommonHelper.DateFormatMMMDDYYY = function (strDate) {
    var dateStr = new Date(JSON.parse(strDate.replace('/Date(', '').replace(')/', '')));
    var dates = (dateStr.getMonth() + '/' + dateStr.getDate() + '/' + dateStr.getFullYear());

    return dates;
}

CommonHelper.GetReportViewerControlId = function (barTable) {
    var reportViewerId = barTable.attr("id");
    var v = $("#" + reportViewerId + "_fixedTable tbody tr:eq(2)").find("td").find('div');
    var barControlId = v.attr("id");

    return barControlId;
}

//InclusiveBill, Vat, ServiceCharge, CitySDCharge, AdditionalCharge,
//additionalChargeType, txtRoomRateVal, parseInt(cbServiceChargeVal, 10),
//parseInt(cbVatAmountVal, 10), parseInt(isInvoiceCitySDChargeEnable),
//parseInt(isInvoiceAdditionalChargeEnable
CommonHelper.GetDefaultRackRateServiceChargeVatCalculation = function (InclusiveHotelManagementBill, GuestHouseVatAmount, GuestHouseServiceCharge, GuestHouseCityCharge, NegotiatedRoomRate, IsServiceChargeEnable, IsCityChargeEnable, IsVatEnable) {
    var ServiceCharge = 0.0, VatAmount = 0.0, RackRate = 0.0;
    //alert(NegotiatedRoomRate);

    if (NegotiatedRoomRate > 0) {
        if (InclusiveHotelManagementBill == 0) {
            ServiceCharge = toFixed(parseFloat(NegotiatedRoomRate * (GuestHouseServiceCharge / 100) * IsServiceChargeEnable), 2);
            VatAmount = toFixed(parseFloat((NegotiatedRoomRate + parseFloat(ServiceCharge)) * (GuestHouseVatAmount / 100) * IsVatEnable), 2);
            RackRate = toFixed(parseFloat(NegotiatedRoomRate + parseFloat(ServiceCharge) + parseFloat(VatAmount)), 2);
        }
        else {
            VatAmount = toFixed(parseFloat(NegotiatedRoomRate * ((GuestHouseVatAmount / (100 + GuestHouseVatAmount)) * IsVatEnable)), 2);
            ServiceCharge = toFixed(parseFloat((NegotiatedRoomRate - (VatAmount * IsVatEnable)) * IsServiceChargeEnable * (GuestHouseServiceCharge / (100 + GuestHouseServiceCharge))), 2);
            RackRate = toFixed(parseFloat(NegotiatedRoomRate - ServiceCharge - VatAmount), 2);
        }
    }

    var InnboardGuestHouseBillInfo = {
        RackRate: RackRate,
        ServiceCharge: ServiceCharge,
        VatAmount: VatAmount,
        TotalRoomRate: 0.0,
        IsInclusive: InclusiveHotelManagementBill
    };

    return InnboardGuestHouseBillInfo;
}

CommonHelper.GetRackRateServiceChargeVatInformation = function (TransactionalAmount, ServiceChargeConfig, SDCityChargeConfig, VatConfig,
    AdditionalChargeConfig, AdditionalChargeType, IsInclusive,
    IsRatePlusPlus, IsVatOnSD, isCitySDChargeEnableOnServiceCharge, IsVatEnable, IsServiceChargeEnable,
    IsSDChargeEnable, IsAdditionalChargeEnable, isDiscountApplicableOnRackRate,
    DiscountType, Discount) {


    var
        RackRateConfig = 100.00,
        PercentageTotal = 0.00,
        RackRate = 0.00,
        ServiceCharge = 0.00,
        SDCityCharge = 0.00,
        VatAmount = 0.00,
        AdditionalCharge = 0.00,
        CalculatedAmount = 0.00,
        DiscountAmount = 0.00,
        DiscountedAmount = 0.00;

    if (isDiscountApplicableOnRackRate == 0) {
        DiscountAmount = DiscountType == 'Percentage' ? (TransactionalAmount * (Discount / 100.00)) : Discount;
        TransactionalAmount = TransactionalAmount - DiscountAmount;
        DiscountedAmount = TransactionalAmount;
    }
    else if (isDiscountApplicableOnRackRate == 1) {//--** Discount When applicable on Rackrate. Find Out the Maximum Discount     
        DiscountedAmount = TransactionalAmount;
    }

    //Inclusive Calculation
    if (IsInclusive == 1) {

        if (AdditionalChargeType == 'Fixed') {
            AdditionalCharge = AdditionalChargeConfig;
            AdditionalChargeConfig = 0;
            TransactionalAmount -= AdditionalCharge;
        }

        //Not Flat Rate
        if (IsRatePlusPlus == 1) {
            if (SDCityChargeConfig > 0) {
                if (isCitySDChargeEnableOnServiceCharge == 1) {
                    SDCityChargeConfig += ServiceChargeConfig * (SDCityChargeConfig / 100);
                }
                else {
                    SDCityChargeConfig += (SDCityChargeConfig / 100);
                }
            }

            if (IsVatOnSD == 1) {
                VatConfig += (ServiceChargeConfig + SDCityChargeConfig) * (VatConfig / 100);
            }
            else {
                VatConfig += ServiceChargeConfig * (VatConfig / 100);
            }
        } // Not Flat Rate End

        PercentageTotal = RackRateConfig + ServiceChargeConfig + SDCityChargeConfig + VatConfig + AdditionalChargeConfig;

        ServiceCharge = (ServiceChargeConfig / PercentageTotal) * TransactionalAmount;
        SDCityCharge = (SDCityChargeConfig / PercentageTotal) * TransactionalAmount;
        VatAmount = (VatConfig / PercentageTotal) * TransactionalAmount;

        RackRate = TransactionalAmount - (ServiceCharge + SDCityCharge + VatAmount);

        if (AdditionalChargeConfig > 0) {
            AdditionalCharge = (AdditionalChargeConfig / PercentageTotal) * TransactionalAmount;
            RackRate -= AdditionalCharge;
        }

        if (isDiscountApplicableOnRackRate == 1) {
            DiscountAmount = DiscountType == 'Percentage' ? RackRate * (Discount / 100.00) : Discount;
            RackRate = RackRate - DiscountAmount;
            DiscountedAmount = IsInclusive == 1 ? (DiscountedAmount - DiscountAmount) : RackRate;
        }

        CalculatedAmount = RackRate + ServiceCharge + SDCityCharge + VatAmount + AdditionalCharge;

    }//Inclusive Calculation Ended
    else {

        RackRate = TransactionalAmount;
        CalculatedAmount = RackRate;

        if (ServiceChargeConfig > 0) {
            ServiceCharge = RackRate * (ServiceChargeConfig / 100.00);
            CalculatedAmount += ServiceCharge;
        }

        if (SDCityChargeConfig > 0) {
            // Not Flat Rate
            if (IsRatePlusPlus == 1) {
                if (isCitySDChargeEnableOnServiceCharge == 1) {
                    SDCityCharge = CalculatedAmount * (SDCityChargeConfig / 100.00);
                }
                else {
                    SDCityCharge = RackRate * (SDCityChargeConfig / 100.00);
                }
            }
            else { // Flat Rate
                SDCityCharge = RackRate * (SDCityChargeConfig / 100.00);
            }

            CalculatedAmount += SDCityCharge;
        }

        if (VatConfig > 0) {

            if (IsRatePlusPlus == 0) // Flat Rate
            {
                VatAmount = RackRate * (VatConfig / 100.00);
            }
            else if (IsVatOnSD == 0) //Vat Not Applicable To SD Charge
            {
                VatAmount = (RackRate + ServiceCharge) * (VatConfig / 100.00);
            }
            else {
                VatAmount = CalculatedAmount * (VatConfig / 100.00);
            }

            CalculatedAmount += VatAmount;
        }

        if (AdditionalChargeType == 'Fixed') {
            AdditionalCharge = AdditionalChargeConfig;
        }
        else {
            AdditionalCharge = RackRate * (AdditionalChargeConfig / 100.00);
        }

        CalculatedAmount += AdditionalCharge;

        if (isDiscountApplicableOnRackRate == 1) {
            DiscountAmount = DiscountType == 'Percentage' ? RackRate * (Discount / 100.00) : Discount;
            RackRate = RackRate - DiscountAmount
            CalculatedAmount -= DiscountAmount
        }
    }

    var BillPaymentDetails = {

        RackRate: toFixed(RackRate, 2),
        DiscountAmount: toFixed(DiscountAmount, 2),
        DiscountedAmount: toFixed(DiscountedAmount, 2),
        ServiceCharge: toFixed(ServiceCharge, 2),
        SDCityCharge: toFixed(SDCityCharge, 2),
        VatAmount: toFixed(VatAmount, 2),
        AdditionalCharge: toFixed(AdditionalCharge, 2),
        CalculatedAmount: Math.round(CalculatedAmount).toFixed(2),
        GrandTotal: Math.round(CalculatedAmount).toFixed(2)
    };

    return BillPaymentDetails;
}

CommonHelper.GetKotWiseVatNSChargeNDiscountNComplementary = function (itemTableId, discountType, discount,
    ServiceChargeConfig, SDCityChargeConfig, VatConfig, AdditionalChargeConfig,
    AdditionalChargeType, IsInclusive, IsRatePlusPlus, IsVatOnSD, isCitySDChargeEnableOnServiceCharge, IsVatEnable, IsServiceChargeEnable,
    IsSDChargeEnable, IsAdditionalChargeEnable, isDiscountApplicableOnRackRate, isComplementaryOrNonChargeable
) {

    var
        RackRateConfig = 100.00,
        PercentageTotal = 0.00,
        RackRate = 0.00,
        ServiceCharge = 0.00,
        SDCityCharge = 0.00,
        VatAmount = 0.00,
        AdditionalCharge = 0.00,
        CalculatedAmount = 0.00,
        TransactionalAmount = 0.00;

    var
        RackRateTotal = 0.00,
        ServiceChargeTotal = 0.00,
        SDCityChargeTotal = 0.00,
        VatAmountTotal = 0.00,
        AdditionalChargeTotal = 0.00,
        CalculatedAmountTotal = 0.00,
        DiscountedAmountTotal = 0.00,
        DiscountedAmount = 0.00,
        ActualDiscountTotal = 0.00;

    var itemId, classificationId, itemWiseDiscountType, itemWiseIndividualDiscount;
    var classificationWiseDiscount = 0.00, itemWiseDiscount = 0.00, totalItemAmount = 0.00, totalItemSalesAmount = 0.00;
    var totalSalesAmount = 0.00, classificationDiscount = 0.00, fixedDiscountRatio = 0.00;
    var itemUnit = 0.00, actualDiscount = 0.00, grandTotal = 0.00;

    if (discountType == 'Fixed') {
        $("#" + itemTableId + " tbody tr").each(function () {
            totalItemSalesAmount = totalItemSalesAmount + parseFloat($(this).find("td:eq(3)").text());
        });

        fixedDiscountRatio = parseFloat(discount) / totalItemSalesAmount;
    }

    $("#" + itemTableId + " tbody tr").each(function () {

        itemId = parseInt($(this).find("td:eq(5)").text(), 10);
        classificationId = parseInt($(this).find("td:eq(6)").text(), 10);
        itemWiseDiscountType = $.trim($(this).find("td:eq(7)").text());
        itemWiseIndividualDiscount = parseFloat($(this).find("td:eq(8)").text());
        totalItemAmount = parseFloat($(this).find("td:eq(3)").text());
        itemUnit = parseFloat($(this).find("td:eq(1)").text());

        totalItemAmount = isComplementaryOrNonChargeable == 0 ? totalItemAmount : 0.00;

        ServiceChargeConfig = parseFloat($(this).find("td:eq(9)").text());
        SDCityChargeConfig = parseFloat($(this).find("td:eq(10)").text());
        VatConfig = parseFloat($(this).find("td:eq(11)").text());
        AdditionalChargeConfig = parseFloat($(this).find("td:eq(12)").text());

        if (!IsServiceChargeEnable) { ServiceChargeConfig = 0.00; }
        if (!IsSDChargeEnable) { SDCityChargeConfig = 0.00; }
        if (!IsVatEnable) { VatConfig = 0.00; }
        if (!IsAdditionalChargeEnable) { AdditionalChargeConfig = 0.00; }

        totalSalesAmount += totalItemAmount;

        var calssifiedItem = _.findWhere(AddedClassificationList, { ClassificationId: classificationId });

        if (calssifiedItem != null) {
            classificationDiscount = parseFloat(calssifiedItem.DiscountAmount);
        }

        //--** Discount When not applicable on Rackrate. It is applicable on Main/Discounted Amount.
        //--** Find out the Maximum Discount Amount
        if (isDiscountApplicableOnRackRate == 0) {

            if (calssifiedItem != null) {
                if (discountType == "Percentage") {
                    classificationWiseDiscount = (totalItemAmount * (classificationDiscount / 100.00));
                }
            }

            if (itemWiseIndividualDiscount > 0) {

                if (itemWiseDiscountType == 'Percentage') {
                    itemWiseDiscount = (totalItemAmount * (itemWiseIndividualDiscount / 100.00));
                }
                else if (itemWiseDiscountType == 'Fixed') {
                    itemWiseDiscount = (itemUnit * itemWiseIndividualDiscount);
                }
            }

            if (itemWiseDiscount > classificationWiseDiscount) {
                //toastr.info("Individual Item wise discount is Greater Than Calssification Wise Discount. Bigger Discount must Apply.");
                actualDiscount = itemWiseDiscount;
            }
            else {
                actualDiscount = classificationWiseDiscount;
            }

            if (itemWiseDiscount == 0 && classificationWiseDiscount == 0) {

                if (discountType == "Percentage") {
                    actualDiscount = ((totalItemAmount * discount) / (100.00));
                }
                else {
                    actualDiscount = fixedDiscountRatio * totalItemAmount;
                }
            }
            else if (itemWiseDiscount > 0 && classificationWiseDiscount == 0 && itemWiseDiscount > discount) {
                actualDiscount = itemWiseDiscount;
            }
            else if (itemWiseDiscount == 0 && classificationWiseDiscount > 0 && classificationWiseDiscount > discount) {
                actualDiscount = classificationWiseDiscount;
            }

            TransactionalAmount = totalItemAmount - actualDiscount;
            DiscountedAmount = totalItemAmount - actualDiscount;

        }//--** Ended Discount When not applicable on Rackrate. It is applicable on Main/Discounted Amount

        else if (isDiscountApplicableOnRackRate == 1) {//--** Discount When applicable on Rackrate. Find Out the Maximum Discount       

            if (itemWiseIndividualDiscount == 0 && classificationDiscount == 0) {

                if (discountType == "Percentage") {
                    discount = discount;
                }
                else {
                    discount = fixedDiscountRatio;
                }
            }
            else if (itemWiseIndividualDiscount > 0 && classificationDiscount == 0 && itemWiseIndividualDiscount > discount) {
                discount = itemWiseIndividualDiscount;
                discountType = itemWiseDiscountType;
            }
            else if (itemWiseIndividualDiscount == 0 && classificationDiscount > 0 && classificationDiscount > discount) {
                discount = classificationDiscount;
            }

            TransactionalAmount = totalItemAmount;
            DiscountedAmount = totalItemAmount;

        }//--** Ended Discount When applicable on Rackrate. Find Out the Maximum Discount  

        //Inclusive Calculation
        if (IsInclusive == 1) {

            if (AdditionalChargeType == 'Fixed') {
                AdditionalCharge = AdditionalChargeConfig;
                AdditionalChargeConfig = 0;
                TransactionalAmount -= AdditionalCharge;
            }

            //Not Flat Rate
            if (IsRatePlusPlus == 1) {
                if (SDCityChargeConfig > 0) {
                    if (isCitySDChargeEnableOnServiceCharge == 1) {
                        SDCityChargeConfig += ServiceChargeConfig * (SDCityChargeConfig / 100);
                    }
                    else {
                        SDCityChargeConfig += (SDCityChargeConfig / 100);
                    }
                }

                if (IsVatOnSD == 1) {
                    VatConfig += (ServiceChargeConfig + SDCityChargeConfig) * (VatConfig / 100);
                }
                else {
                    VatConfig += ServiceChargeConfig * (VatConfig / 100);
                }
            } // Not Flat Rate End

            PercentageTotal = RackRateConfig + ServiceChargeConfig + SDCityChargeConfig + VatConfig + AdditionalChargeConfig;

            ServiceCharge = toFixed((ServiceChargeConfig / PercentageTotal) * TransactionalAmount, 2);
            SDCityCharge = toFixed((SDCityChargeConfig / PercentageTotal) * TransactionalAmount, 2);
            VatAmount = toFixed((VatConfig / PercentageTotal) * TransactionalAmount, 2);

            RackRate = TransactionalAmount - (ServiceCharge + SDCityCharge + VatAmount);

            if (AdditionalChargeConfig > 0) {
                AdditionalCharge = (AdditionalChargeConfig / PercentageTotal) * TransactionalAmount;
                RackRate -= AdditionalCharge;
            }

            if (isDiscountApplicableOnRackRate == 1) {
                if (discountType == "Percentage") {
                    actualDiscount = ((RackRate * discount) / (100.00));
                }
                else {
                    actualDiscount = fixedDiscountRatio * RackRate;
                }

                RackRate = RackRate - actualDiscount
                DiscountedAmount = IsInclusive == 1 ? (DiscountedAmount - actualDiscount) : RackRate;
            }

            CalculatedAmount = RackRate + ServiceCharge + SDCityCharge + VatAmount + AdditionalCharge;

            //toastr.info(RackRate + ',' + DiscountedAmount + ',' + CalculatedAmount);

        }//Inclusive Calculation Ended

        else { // Exclusive Calculation

            RackRate = TransactionalAmount;
            CalculatedAmount = RackRate;

            if (ServiceChargeConfig > 0) {
                ServiceCharge = RackRate * (ServiceChargeConfig / 100.00);
                CalculatedAmount += ServiceCharge;
            }

            if (SDCityChargeConfig > 0) {
                // Not Flat Rate
                if (IsRatePlusPlus == 1) {
                    if (isCitySDChargeEnableOnServiceCharge == 1) {
                        SDCityCharge = CalculatedAmount * (SDCityChargeConfig / 100.00);
                    }
                    else {
                        SDCityCharge = RackRate * (SDCityChargeConfig / 100.00);
                    }
                }
                else { // Flat Rate
                    SDCityCharge = RackRate * (SDCityChargeConfig / 100.00);
                }

                CalculatedAmount += SDCityCharge;
            }

            if (VatConfig > 0) {

                if (IsRatePlusPlus == 0) // Flat Rate
                {
                    VatAmount = RackRate * (VatConfig / 100.00);
                }
                else if (IsVatOnSD == 0) //Vat Not Applicable To SD Charge
                {
                    VatAmount = (RackRate + ServiceCharge) * (VatConfig / 100.00);
                }
                else {
                    VatAmount = CalculatedAmount * (VatConfig / 100.00);
                }

                CalculatedAmount += VatAmount;
            }

            if (AdditionalChargeType == 'Fixed') {
                AdditionalCharge = AdditionalChargeConfig;
            }
            else {
                AdditionalCharge = RackRate * (AdditionalChargeConfig / 100.00);
            }

            CalculatedAmount += AdditionalCharge;

            if (isDiscountApplicableOnRackRate == 1) {

                if (discountType == "Percentage") {
                    actualDiscount = ((RackRate * discount) / (100.00));
                }
                else {
                    actualDiscount = fixedDiscountRatio * RackRate;
                }

                //toastr.info(IsInclusive);

                RackRate = RackRate - actualDiscount;
                DiscountedAmount = IsInclusive == 1 ? (RackRate - actualDiscount) : RackRate;
                CalculatedAmount = CalculatedAmount - actualDiscount;

                //toastr.info(RackRate + ',' + DiscountedAmount + ',' + CalculatedAmount);
            }
        }

        RackRateTotal += RackRate;
        ServiceChargeTotal += ServiceCharge;
        SDCityChargeTotal += SDCityCharge;
        VatAmountTotal += VatAmount;
        AdditionalChargeTotal += AdditionalCharge;
        CalculatedAmountTotal += CalculatedAmount;
        DiscountedAmountTotal += DiscountedAmount;
        ActualDiscountTotal += actualDiscount;

        totalItemAmount = 0.00;
        RackRate = 0.00;
        CalculatedAmount = 0.00;
        TransactionalAmount = 0.00;
        DiscountedAmount = 0.00;
        actualDiscount = 0.00;
        AdditionalCharge = 0.00;
        classificationDiscount = 0;
        classificationWiseDiscount = 0;
        VatAmount = 0.00;
        SDCityCharge = 0.00;
    });

    var BillPaymentDetails = {

        TotalSalesAmount: toFixed(totalSalesAmount, 2),
        DiscountAmount: toFixed(ActualDiscountTotal, 2),
        DiscountedAmount: toFixed(DiscountedAmountTotal, 2),

        RackRate: RackRateTotal,
        ServiceCharge: ServiceChargeTotal,
        SDCityCharge: SDCityChargeTotal,
        VatAmount: VatAmountTotal,
        AdditionalCharge: AdditionalChargeTotal,
        CalculatedAmount: CalculatedAmountTotal,
        GrandTotal: CalculatedAmountTotal
    };

    return BillPaymentDetails;
}

CommonHelper.GetBanquetVatNSChargeNDiscountNComplementary = function (itemTableId, discountType, discount, vatAmount, serviceChargeAmount, IsServiceChargeEnable, IsVatEnable, IsInclusiveBill) {

    var itemId, classificationId = 0, itemWiseDiscountType, itemWiseIndividualDiscount;
    var classificationWiseDiscount = 0.00, itemWiseDiscount = 0.00, totalItemAmount = 0.00;
    var totalSalesAmount = 0.00;
    var itemUnit = 0.00, actualDiscount = 0.00;

    $("#" + itemTableId + " tbody tr").each(function () {

        itemId = parseInt($(this).find("td:eq(4)").text(), 10);
        classificationId = parseInt($(this).find("td:eq(2)").text(), 10);
        totalItemAmount = parseFloat($(this).find("td:eq(8)").text());
        itemUnit = parseFloat($(this).find("td:eq(7)").text());

        totalSalesAmount += totalItemAmount;

        var calssifiedItem = _.findWhere(categoryWiseDiscount, { CategoryId: classificationId });

        if (calssifiedItem != null) {

            if (discountType == "Percentage") {
                classificationWiseDiscount += (totalItemAmount * (discount / 100.00));
            }
            else {
                classificationWiseDiscount += (discount * itemUnit);
            }
        }
    });

    var calssifiedItem = _.findWhere(categoryWiseDiscount, { CategoryId: 100001 });

    if (calssifiedItem != null) {

        totalItemAmount = parseFloat($("#ContentPlaceHolder1_txtBanquetRate").val());
        itemUnit = 1.00;
        totalSalesAmount += totalItemAmount;

        if (discountType == "Percentage") {
            classificationWiseDiscount += (totalItemAmount * (discount / 100.00));
        }
        else {
            classificationWiseDiscount += (discount * itemUnit);
        }
    }
    else {
        totalItemAmount = parseFloat($("#ContentPlaceHolder1_txtBanquetRate").val());
        totalSalesAmount += totalItemAmount;
    }

    if (classificationWiseDiscount == 0) {

        if (discountType == "Percentage") {
            actualDiscount = (totalSalesAmount * (discount / 100.00));
        }
        else {
            actualDiscount = discount;
        }
    }
    else if (classificationWiseDiscount > 0) { //&& classificationWiseDiscount > discount
        actualDiscount = classificationWiseDiscount;
    }

    var ServiceCharge = 0.00, Vat = 0.00, ServiceRate = 0.00, discountedAmount = 0.00;

    discountedAmount = totalSalesAmount - actualDiscount;

    if (totalSalesAmount > 0) {
        if (IsInclusiveBill == 0) {
            ServiceCharge = toFixed(parseFloat(discountedAmount * (serviceChargeAmount / 100) * IsServiceChargeEnable), 2);
            Vat = toFixed(parseFloat((discountedAmount + parseFloat(ServiceCharge)) * (vatAmount / 100) * IsVatEnable), 2);

            ServiceRate = toFixed(parseFloat(discountedAmount), 2);
        }
        else {
            Vat = toFixed(parseFloat(discountedAmount * ((vatAmount / (100 + vatAmount)) * IsVatEnable)), 2);
            ServiceCharge = toFixed(parseFloat((discountedAmount - (Vat)) * IsServiceChargeEnable * (serviceChargeAmount / (100 + serviceChargeAmount))), 2);

            ServiceRate = toFixed(parseFloat(discountedAmount - ServiceCharge - Vat), 2);
        }
    }

    var BillPaymentDetails = {
        TotalSalesAmount: toFixed(totalSalesAmount, 2),
        DiscountAmount: toFixed(actualDiscount, 2),
        DiscountedAmount: toFixed(discountedAmount, 2),
        ServiceRate: toFixed(ServiceRate, 2),
        ServiceCharge: toFixed(ServiceCharge, 2),
        VatAmount: toFixed(Vat, 2),
        GrandTotal: toFixed((parseFloat(ServiceRate) + parseFloat(ServiceCharge) + parseFloat(Vat)), 2)
    };
    return BillPaymentDetails;
}

CommonHelper.Age = function (birthdate) {

    //date of birth format must be dd/mm/yyyy 

    if (innboardFormat == "mm/dd/yy") {

        var innboardFormat = "dd/MM/yyyy";
        var dateTime = new Date(birthdate);
        birthdate = dateTime.format(innboardFormat);
    }

    splitBirthdayParts = birthdate.split('/');
    myDay = splitBirthdayParts[0];
    myMonth = splitBirthdayParts[1];
    myYear = splitBirthdayParts[2];

    var age;
    var today = new Date();
    var todaysDay = today.getDate();
    var todaysMonth = today.getMonth() + 1;

    var todaysYear = today.getYear();
    age = todaysYear - myYear;

    if (todaysMonth < myMonth) age--;
    if (todaysMonth == myMonth) {
        if (todaysDay < myDay) age--;
    }

    if (isNaN(age)) age = "???";
    return age;
}

CommonHelper.AutoSearch = function (searchControl, dataSource, valueFieldControl) {

    $(document).ready(function () {
        $("#" + searchControl).autocomplete({
            source: function (request, response) {

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: dataSource,
                    data: "{'searchText':'" + request.term + "'}",
                    dataType: "json",
                    success: function (data) {

                        var searchData = data.error ? [] : $.map(data.d, function (m) {
                            return {
                                label: m.DisplayField,
                                value: m.ValueField
                            };
                        });
                        response(searchData);
                    },
                    error: function (result) {
                        //alert("Error");
                    }
                });
            },
            focus: function (event, ui) {
                // prevent autocomplete from updating the textbox
                event.preventDefault();
                // manually update the textbox
                $(this).val(ui.item.label);
            },
            select: function (event, ui) {
                // prevent autocomplete from updating the textbox
                event.preventDefault();
                // manually update the textbox and hidden field
                $(this).val(ui.item.label);
                $("#" + valueFieldControl).val(ui.item.value);
            }
        });
    });
}

CommonHelper.BrowserType = function () {
    var browser = {
        chrome: false,
        mozilla: false,
        opera: false,
        msie: false,
        safari: false
    };
    var sUsrAg = navigator.userAgent;
    if (sUsrAg.indexOf("Chrome") > -1) {
        browser.chrome = true;
    } else if (sUsrAg.indexOf("Safari") > -1) {
        browser.safari = true;
    } else if (sUsrAg.indexOf("Opera") > -1) {
        browser.opera = true;
    } else if (sUsrAg.indexOf("Firefox") > -1) {
        browser.mozilla = true;
    } else if (sUsrAg.indexOf("MSIE") > -1) {
        browser.msie = true;
    }

    return browser;
}


CommonHelper.GetParameterByName = function (name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null) return "";
    else return decodeURIComponent(results[1].replace(/\+/g, " "));
}

CommonHelper.AutoSearchClientDataSource = function (searchControl, dataSource, valueFieldControl,dialogId="") {

    $(document).ready(function () {

        $.extend($.expr[':'], {
            'containsi': function (elem, i, match, array) {
                return (elem.textContent || elem.innerText || '').toLowerCase()
                    .indexOf((match[3] || "").toLowerCase()) >= 0;
            }
        });
        if (dialogId) {
            $("#" + searchControl).autocomplete({
                source: function (request, response) {

                    var options = [];
                    options = $("#" + dataSource).find('option:containsi(' + request.term + ')');
                    var searchData = $.map(options, function (m) {
                        return {
                            label: m.text,
                            value: m.value
                        };
                    });

                    if (searchData.length <= 0) { $("#" + valueFieldControl).val(''); }

                    response(searchData);
                },
                focus: function (event, ui) {
                    event.preventDefault();
                    $(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    event.preventDefault();
                    $(this).val(ui.item.label);
                    $("#" + valueFieldControl).val(ui.item.value);

                    var inputs = $(this).closest('form').find(':focusable');
                    inputs.eq(inputs.index(this) + 1).focus();
                }
            }).autocomplete("option", "appendTo", dialogId);
        }
        else {
            $("#" + searchControl).autocomplete({
                source: function (request, response) {

                    var options = [];
                    options = $("#" + dataSource).find('option:containsi(' + request.term + ')');
                    var searchData = $.map(options, function (m) {
                        return {
                            label: m.text,
                            value: m.value
                        };
                    });

                    if (searchData.length <= 0) { $("#" + valueFieldControl).val(''); }

                    response(searchData);
                },
                focus: function (event, ui) {
                    event.preventDefault();
                    $(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    event.preventDefault();
                    $(this).val(ui.item.label);
                    $("#" + valueFieldControl).val(ui.item.value);

                    var inputs = $(this).closest('form').find(':focusable');
                    inputs.eq(inputs.index(this) + 1).focus();
                }
            });
        }
    });
}

CommonHelper.TouchScreenKeyboard = function (keyBoardControl) {

    $("#" + keyBoardControl).keyboard({

        // set this to ISO 639-1 language code to override language set by the layout
        // http://en.wikipedia.org/wiki/List_of_ISO_639-1_codes
        // language defaults to "en" if not found
        language: null,
        rtl: false,

        // *** choose layout ***
        layout: 'qwerty',
        customLayout: { 'normal': ['{cancel}'] },

        position: {
            of: null, // optional - null (attach to input/textarea) or a jQuery object (attach elsewhere)
            my: 'center top',
            at: 'center top',
            at2: 'center bottom', // used when "usePreview" is false (centers keyboard at bottom of the input/textarea)
            collision: 'fit fit'
        },

        // allow jQuery position utility to reposition the keyboard on window resize
        reposition: true,

        // preview added above keyboard if true, original input/textarea used if false
        usePreview: false,

        // if true, the keyboard will always be visible
        alwaysOpen: false,

        // give the preview initial focus when the keyboard becomes visible
        initialFocus: true,

        // if true, keyboard will remain open even if the input loses focus.
        stayOpen: false,

        // *** change keyboard language & look ***
        display: {
            'a': '\u2714:Accept (Shift-Enter)', // check mark - same action as accept
            'accept': 'Accept:Accept (Shift-Enter)',
            'alt': 'AltGr:Alternate Graphemes',
            'b': '\u2190:Backspace',    // Left arrow (same as &larr;)
            'bksp': 'Bksp:Backspace',
            'c': '\u2716:Cancel (Esc)', // big X, close - same action as cancel
            'cancel': 'Cancel:Cancel (Esc)',
            'clear': 'C:Clear',             // clear num pad
            'combo': '\u00f6:Toggle Combo Keys',
            'dec': '.:Decimal',           // decimal point for num pad (optional), change '.' to ',' for European format
            'e': '\u21b5:Enter',        // down, then left arrow - enter symbol
            'enter': 'Enter:Enter',
            'left': '\u2190',              // left arrow (move caret)
            'lock': '\u21ea Lock:Caps Lock', // caps lock
            'next': 'Next',
            'prev': 'Prev',
            'right': '\u2192',              // right arrow (move caret)
            's': '\u21e7:Shift',        // thick hollow up arrow
            'shift': 'Shift:Shift',
            'sign': '\u00b1:Change Sign',  // +/- sign for num pad
            'space': '&nbsp;:Space',
            't': '\u21e5:Tab',          // right arrow to bar (used since this virtual keyboard works with one directional tabs)
            'tab': '\u21e5 Tab:Tab'       // \u21b9 is the true tab symbol (left & right arrows)
        },

        // Message added to the key title while hovering, if the mousewheel plugin exists
        wheelMessage: 'Use mousewheel to see other keys',

        css: {
            input: 'ui-widget-content ui-corner-all', // input & preview
            container: 'ui-widget-content ui-widget ui-corner-all ui-helper-clearfix', // keyboard container
            buttonDefault: 'ui-state-default ui-corner-all', // default state
            buttonHover: 'ui-state-hover',  // hovered button
            buttonAction: 'ui-state-active', // Action keys (e.g. Accept, Cancel, Tab, etc); replaces "actionClass"
            buttonDisabled: 'ui-state-disabled', // used when disabling the decimal button {dec}
            buttonEmpty: 'ui-keyboard-empty' // empty button class name {empty}
        },

        // *** Useability ***
        // Auto-accept content when clicking outside the keyboard (popup will close)
        autoAccept: false,

        // Prevents direct input in the preview window when true
        lockInput: false,

        // Prevent keys not in the displayed keyboard from being typed in
        restrictInput: false,

        // Check input against validate function, if valid the accept button is clickable;
        // if invalid, the accept button is disabled.
        acceptValid: true,

        // if acceptValid is true & the validate function returns a false, this option will cancel
        // a keyboard close only after the accept button is pressed
        cancelClose: true,

        // Use tab to navigate between input fields
        tabNavigation: false,

        // press enter (shift-enter in textarea) to go to the next input field
        enterNavigation: true,
        // mod key options: 'ctrlKey', 'shiftKey', 'altKey', 'metaKey' (MAC only)
        enterMod: 'altKey', // alt-enter to go to previous; shift-alt-enter to accept & go to previous

        // if true, the next button will stop on the last keyboard input/textarea; prev button stops at first
        // if false, the next button will wrap to target the first input/textarea; prev will go to the last
        stopAtEnd: true,

        // Set this to append the keyboard immediately after the input/textarea it is attached to.
        // This option works best when the input container doesn't have a set width and when the
        // "tabNavigation" option is true
        appendLocally: false,

        // Append the keyboard to a desired element. This can be a jQuery selector string or object
        appendTo: 'body',

        // If false, the shift key will remain active until the next key is (mouse) clicked on;
        // if true it will stay active until pressed again
        stickyShift: true,

        // caret places at the end of any text
        caretToEnd: false,

        // Prevent pasting content into the area
        preventPaste: false,

        // Set the max number of characters allowed in the input, setting it to false disables this option
        maxLength: false,

        // allow inserting characters '@@' caret when maxLength is set
        maxInsert: true,

        // Mouse repeat delay - when clicking/touching a virtual keyboard key, after this delay the key
        // will start repeating
        repeatDelay: 500,

        // Mouse repeat rate - after the repeatDelay, this is the rate (characters per second) at which the
        // key is repeated. Added to simulate holding down a real keyboard key and having it repeat. I haven't
        // calculated the upper limit of this rate, but it is limited to how fast the javascript can process
        // the keys. And for me, in Firefox, it's around 20.
        repeatRate: 20,

        // resets the keyboard to the default keyset when visible
        resetDefault: false,

        // Event (namespaced) on the input to reveal the keyboard. To disable it, just set it to ''.
        openOn: '', //focus

        // When the character is added to the input
        keyBinding: 'mousedown',

        // combos (emulate dead keys : http://en.wikipedia.org/wiki/Keyboard_layout#US-International)
        // if user inputs `a the script converts it to à, ^o becomes ô, etc.
        useCombos: true,

        // *** Methods ***
        // Callbacks - add code inside any of these callback functions as desired
        initialized: function (e, keyboard, el) { },
        visible: function (e, keyboard, el) { },
        change: function (e, keyboard, el) { },
        beforeClose: function (e, keyboard, el, accepted) { },
        accepted: function (e, keyboard, el) { },
        canceled: function (e, keyboard, el) { },
        hidden: function (e, keyboard, el) { },

        switchInput: null, // called instead of base.switchInput

        // this callback is called just before the "beforeClose" to check the value
        // if the value is valid, return true and the keyboard will continue as it should
        // (close if not always open, etc)
        // if the value is not value, return false and the clear the keyboard value
        // ( like this "keyboard.$preview.val('');" ), if desired
        // The validate function is called after each input, the "isClosing" value will be false;
        // when the accept button is clicked, "isClosing" is true
        validate: function (keyboard, value, isClosing) { return true; }

    });
}

CommonHelper.TouchScreenNumberKeyboardPayment = function (keyBoardControl, KeyBoardContainer) {

    $("." + keyBoardControl).keyboard({

        language: null,
        rtl: false,

        // *** Keyboard layout ***
        layout: 'custom',
        customLayout: {
            'default': [
                '10  7 8 9',
                '20  4 5 6',
                '50  1 2 3',
                '100 0 {dec} {b}',
                '500 {sp:1.96} {clear} {del}',
                '1000 00 {sp:0.72} Due-Amount'
            ]
        },

        position: {
            of: ($("#" + KeyBoardContainer)), // optional - null (attach to input/textarea) or a jQuery object (attach elsewhere)
            my: 'center top',
            at: 'center top',
            at2: 'center top', // used when "usePreview" is false (centers keyboard at bottom of the input/textarea)
            collision: 'fit fit'
        },

        // allow jQuery position utility to reposition the keyboard on window resize
        reposition: false,

        // preview added above keyboard if true, original input/textarea used if false
        usePreview: false,

        // if true, the keyboard will always be visible
        alwaysOpen: false,

        // give the preview initial focus when the keyboard becomes visible
        initialFocus: false,

        // if true, keyboard will remain open even if the input loses focus.
        stayOpen: true,

        // *** change keyboard language & look ***
        display: {
            'a': '\u2714:Accept (Shift-Enter)', // check mark - same action as accept
            'accept': 'Accept:Accept (Shift-Enter)',
            'alt': 'AltGr:Alternate Graphemes',
            'b': '\u2190:Backspace',    // Left arrow (same as &larr;)
            'bksp': 'Bksp:Backspace',
            'c': '\u2716:Cancel (Esc)', // big X, close - same action as cancel
            'cancel': 'Cancel:Cancel (Esc)',
            'clear': 'C:Clear',             // clear num pad
            'combo': '\u00f6:Toggle Combo Keys',
            'dec': '.:Decimal',           // decimal point for num pad (optional), change '.' to ',' for European format
            'e': '\u21b5:Enter',        // down, then left arrow - enter symbol
            'enter': 'Enter:Enter',
            'left': '\u2190',              // left arrow (move caret)
            'lock': '\u21ea Lock:Caps Lock', // caps lock
            'next': 'Next',
            'prev': 'Prev',
            'right': '\u2192',              // right arrow (move caret)
            's': '\u21e7:Shift',        // thick hollow up arrow
            'shift': 'Shift:Shift',
            'sign': '\u00b1:Change Sign',  // +/- sign for num pad
            'space': '&nbsp;:Space',
            't': '\u21e5:Tab',          // right arrow to bar (used since this virtual keyboard works with one directional tabs)
            'tab': '\u21e5 Tab:Tab'       // \u21b9 is the true tab symbol (left & right arrows)
        },

        // Message added to the key title while hovering, if the mousewheel plugin exists
        wheelMessage: 'Use mousewheel to see other keys',

        css: {
            input: 'ui-widget-content ui-corner-all', // input & preview
            container: 'ui-widget-content ui-widget ui-corner-all ui-helper-clearfix', // keyboard container
            buttonDefault: 'ui-state-default ui-corner-all', // default state
            buttonHover: 'ui-state-hover',  // hovered button
            buttonAction: 'ui-state-active', // Action keys (e.g. Accept, Cancel, Tab, etc); replaces "actionClass"
            buttonDisabled: 'ui-state-disabled', // used when disabling the decimal button {dec}
            buttonEmpty: 'ui-keyboard-empty' // empty button class name {empty}
        },

        // *** Useability ***
        // Auto-accept content when clicking outside the keyboard (popup will close)
        autoAccept: true,

        // Prevents direct input in the preview window when true
        lockInput: false,

        // Prevent keys not in the displayed keyboard from being typed in
        restrictInput: true,

        // Check input against validate function, if valid the accept button is clickable;
        // if invalid, the accept button is disabled.
        acceptValid: true,

        // if acceptValid is true & the validate function returns a false, this option will cancel
        // a keyboard close only after the accept button is pressed
        cancelClose: true,

        // Use tab to navigate between input fields
        tabNavigation: false,

        // press enter (shift-enter in textarea) to go to the next input field
        enterNavigation: true,
        // mod key options: 'ctrlKey', 'shiftKey', 'altKey', 'metaKey' (MAC only)
        enterMod: 'altKey', // alt-enter to go to previous; shift-alt-enter to accept & go to previous

        // if true, the next button will stop on the last keyboard input/textarea; prev button stops at first
        // if false, the next button will wrap to target the first input/textarea; prev will go to the last
        stopAtEnd: true,

        // Set this to append the keyboard immediately after the input/textarea it is attached to.
        // This option works best when the input container doesn't have a set width and when the
        // "tabNavigation" option is true
        appendLocally: false,

        // Append the keyboard to a desired element. This can be a jQuery selector string or object
        appendTo: ($("#" + KeyBoardContainer)),

        beforeInsert: function (e, keyboard, el, textToAdd) {

            if (textToAdd == "10" || textToAdd == "20" || textToAdd == "50" || textToAdd == "100" || textToAdd == "500" || textToAdd == "1000" ||
                textToAdd == "Due-Amount") {

                var newValue = 0.00, changeText = "";

                if (textToAdd == "Due-Amount") {
                    changeText = $("#ContentPlaceHolder1_lblTPChangeAmount").text();
                    textToAdd = $("#ContentPlaceHolder1_txtTPChangeAmount").val();

                    if (changeText == "Change Amount") { textToAdd = "0"; }

                    newValue = textToAdd == "0" ? "" : textToAdd;

                    if (parseFloat(el.value == "" ? 0.00 : el.value) > 0 && textToAdd != "0") {
                        newValue = parseFloat(textToAdd) + parseFloat(el.value == "" ? 0.00 : el.value);
                        el.value = "";
                    }
                }
                else {
                    newValue = parseFloat(textToAdd);
                    newValue += parseFloat(el.value == "" ? 0.00 : el.value);
                    el.value = "";
                }

                return newValue + '';
            }
            else {

                var tests = /^[0-9.,\b]+$/.test(textToAdd);
                if (!tests) {
                    return '';
                }

                var newText = $(el).val() + textToAdd;
                var t = newText.split('.').length;

                if (t > 2) { return ''; }

                return textToAdd;
            }
        },

        change: function (e, keyboard, el) {
            FocusedInputControl = $(el).attr("id");
            if ($(el).attr("id") != "ContentPlaceHolder1_txtTPDiscountAmount") {
                CalculatePayment();
            }
            //else if ($(el).attr("id") == "ContentPlaceHolder1_txtTPDiscountAmount") {
            //    //console.log('Change "' + el.value + '" was accepted!');
            //    //CalculateDiscountAmount();     

            //    keyboard.switchInput(true, kb.options.autoAccept);
            //    return false
            //}
        },

        // If false, the shift key will remain active until the next key is (mouse) clicked on;
        // if true it will stay active until pressed again
        stickyShift: true,

        // caret places at the end of any text
        caretToEnd: false,

        // Prevent pasting content into the area
        preventPaste: true,

        // Set the max number of characters allowed in the input, setting it to false disables this option
        maxLength: false,

        // allow inserting characters '@@' caret when maxLength is set
        maxInsert: true,

        // Mouse repeat delay - when clicking/touching a virtual keyboard key, after this delay the key
        // will start repeating
        repeatDelay: 500,

        // Mouse repeat rate - after the repeatDelay, this is the rate (characters per second) at which the
        // key is repeated. Added to simulate holding down a real keyboard key and having it repeat. I haven't
        // calculated the upper limit of this rate, but it is limited to how fast the javascript can process
        // the keys. And for me, in Firefox, it's around 20.
        repeatRate: 20,

        // resets the keyboard to the default keyset when visible
        resetDefault: false,

        // Event (namespaced) on the input to reveal the keyboard. To disable it, just set it to ''.
        openOn: 'focus', //focus

        // When the character is added to the input
        keyBinding: 'mousedown',

        // combos (emulate dead keys : http://en.wikipedia.org/wiki/Keyboard_layout#US-International)
        // if user inputs `a the script converts it to à, ^o becomes ô, etc.
        useCombos: false,

        // *** Methods ***
        // Callbacks - add code inside any of these callback functions as desired
        initialized: function (e, keyboard, el) { },
        visible: function (e, keyboard, el) { },

        beforeClose: function (e, keyboard, el, accepted) { },
        accepted: function (e, keyboard, el) { console.log('The content "' + el.value + '" was accepted!'); },
        canceled: function (e, keyboard, el) { },
        hidden: function (e, keyboard, el) { },

        switchInput: null, // called instead of base.switchInput

        // this callback is called just before the "beforeClose" to check the value
        // if the value is valid, return true and the keyboard will continue as it should
        // (close if not always open, etc)
        // if the value is not value, return false and the clear the keyboard value
        // ( like this "keyboard.$preview.val('');" ), if desired
        // The validate function is called after each input, the "isClosing" value will be false;
        // when the accept button is clicked, "isClosing" is true
        validate: function (keyboard, value, isClosing) { return true; }

    });
}

CommonHelper.TouchScreenNumberKeyboard = function (keyBoardControl, KeyBoardContainer) {

    $("." + keyBoardControl).keyboard({

        language: null,
        rtl: false,

        // *** Keyboard layout ***
        layout: 'custom',
        customLayout: {
            'default': [
                '7 8 9',
                '4 5 6',
                '1 2 3',
                '0 {dec} {b}',
                '{clear} {del}'
            ]
        },

        position: {
            of: ($("#" + KeyBoardContainer)), // optional - null (attach to input/textarea) or a jQuery object (attach elsewhere)
            my: 'center top',
            at: 'center top',
            at2: 'center top', // used when "usePreview" is false (centers keyboard at bottom of the input/textarea)
            collision: 'fit fit'
        },

        // allow jQuery position utility to reposition the keyboard on window resize
        reposition: false,

        // preview added above keyboard if true, original input/textarea used if false
        usePreview: false,

        // if true, the keyboard will always be visible
        alwaysOpen: false,

        // give the preview initial focus when the keyboard becomes visible
        initialFocus: true,

        // if true, keyboard will remain open even if the input loses focus.
        stayOpen: true,

        // *** change keyboard language & look ***
        display: {
            'a': '\u2714:Accept (Shift-Enter)', // check mark - same action as accept
            'accept': 'Accept:Accept (Shift-Enter)',
            'alt': 'AltGr:Alternate Graphemes',
            'b': '\u2190:Backspace',    // Left arrow (same as &larr;)
            'bksp': 'Bksp:Backspace',
            'c': '\u2716:Cancel (Esc)', // big X, close - same action as cancel
            'cancel': 'Cancel:Cancel (Esc)',
            'clear': 'C:Clear',             // clear num pad
            'combo': '\u00f6:Toggle Combo Keys',
            'dec': '.:Decimal',           // decimal point for num pad (optional), change '.' to ',' for European format
            'e': '\u21b5:Enter',        // down, then left arrow - enter symbol
            'enter': 'Enter:Enter',
            'left': '\u2190',              // left arrow (move caret)
            'lock': '\u21ea Lock:Caps Lock', // caps lock
            'next': 'Next',
            'prev': 'Prev',
            'right': '\u2192',              // right arrow (move caret)
            's': '\u21e7:Shift',        // thick hollow up arrow
            'shift': 'Shift:Shift',
            'sign': '\u00b1:Change Sign',  // +/- sign for num pad
            'space': '&nbsp;:Space',
            't': '\u21e5:Tab',          // right arrow to bar (used since this virtual keyboard works with one directional tabs)
            'tab': '\u21e5 Tab:Tab'       // \u21b9 is the true tab symbol (left & right arrows)
        },

        // Message added to the key title while hovering, if the mousewheel plugin exists
        wheelMessage: 'Use mousewheel to see other keys',

        css: {
            input: 'ui-widget-content ui-corner-all', // input & preview
            container: 'ui-widget-content ui-widget ui-corner-all ui-helper-clearfix', // keyboard container
            buttonDefault: 'ui-state-default ui-corner-all', // default state
            buttonHover: 'ui-state-hover',  // hovered button
            buttonAction: 'ui-state-active', // Action keys (e.g. Accept, Cancel, Tab, etc); replaces "actionClass"
            buttonDisabled: 'ui-state-disabled', // used when disabling the decimal button {dec}
            buttonEmpty: 'ui-keyboard-empty' // empty button class name {empty}
        },

        // *** Useability ***
        // Auto-accept content when clicking outside the keyboard (popup will close)
        autoAccept: true,

        // Prevents direct input in the preview window when true
        lockInput: false,

        // Prevent keys not in the displayed keyboard from being typed in
        restrictInput: true,

        // Check input against validate function, if valid the accept button is clickable;
        // if invalid, the accept button is disabled.
        acceptValid: true,

        // if acceptValid is true & the validate function returns a false, this option will cancel
        // a keyboard close only after the accept button is pressed
        cancelClose: true,

        // Use tab to navigate between input fields
        tabNavigation: false,

        // press enter (shift-enter in textarea) to go to the next input field
        enterNavigation: true,
        // mod key options: 'ctrlKey', 'shiftKey', 'altKey', 'metaKey' (MAC only)
        enterMod: 'altKey', // alt-enter to go to previous; shift-alt-enter to accept & go to previous

        // if true, the next button will stop on the last keyboard input/textarea; prev button stops at first
        // if false, the next button will wrap to target the first input/textarea; prev will go to the last
        stopAtEnd: true,

        // Set this to append the keyboard immediately after the input/textarea it is attached to.
        // This option works best when the input container doesn't have a set width and when the
        // "tabNavigation" option is true
        appendLocally: false,

        // Append the keyboard to a desired element. This can be a jQuery selector string or object
        appendTo: ($("#" + KeyBoardContainer)),

        // If false, the shift key will remain active until the next key is (mouse) clicked on;
        // if true it will stay active until pressed again
        stickyShift: true,

        // caret places at the end of any text
        caretToEnd: false,

        // Prevent pasting content into the area
        preventPaste: true,

        // Set the max number of characters allowed in the input, setting it to false disables this option
        maxLength: false,

        // allow inserting characters '@@' caret when maxLength is set
        maxInsert: true,

        // Mouse repeat delay - when clicking/touching a virtual keyboard key, after this delay the key
        // will start repeating
        repeatDelay: 500,

        // Mouse repeat rate - after the repeatDelay, this is the rate (characters per second) at which the
        // key is repeated. Added to simulate holding down a real keyboard key and having it repeat. I haven't
        // calculated the upper limit of this rate, but it is limited to how fast the javascript can process
        // the keys. And for me, in Firefox, it's around 20.
        repeatRate: 20,

        // resets the keyboard to the default keyset when visible
        resetDefault: false,

        // Event (namespaced) on the input to reveal the keyboard. To disable it, just set it to ''.
        openOn: 'focus', //focus

        // When the character is added to the input
        keyBinding: 'mousedown',

        // combos (emulate dead keys : http://en.wikipedia.org/wiki/Keyboard_layout#US-International)
        // if user inputs `a the script converts it to à, ^o becomes ô, etc.
        useCombos: false,

        // *** Methods ***
        // Callbacks - add code inside any of these callback functions as desired
        initialized: function (e, keyboard, el) { },
        visible: function (e, keyboard, el) { },
        change: function (e, keyboard, el) {
            FocusedInputControl = $(el).attr("id");
            CalculatePayment();
        },
        beforeClose: function (e, keyboard, el, accepted) { },
        accepted: function (e, keyboard, el) { },
        beforeInsert: function (e, keyboard, el, textToAdd) {

            var tests = /^[0-9.,\b]+$/.test(textToAdd);
            if (!tests) {
                return '';
            }
            else {

                var newText = $(el).val() + textToAdd;
                var t = newText.split('.').length;

                if (t > 2) { return ''; }

                return textToAdd;
            }

            //var wholeTextTest = /^[0-9.,\b]+$/.test($(el).val() + textToAdd);           
            //else if (!wholeTextTest) {
            //    return '';
            //}

        },
        canceled: function (e, keyboard, el) { },
        hidden: function (e, keyboard, el) { },

        switchInput: null, // called instead of base.switchInput

        // this callback is called just before the "beforeClose" to check the value
        // if the value is valid, return true and the keyboard will continue as it should
        // (close if not always open, etc)
        // if the value is not value, return false and the clear the keyboard value
        // ( like this "keyboard.$preview.val('');" ), if desired
        // The validate function is called after each input, the "isClosing" value will be false;
        // when the accept button is clicked, "isClosing" is true
        validate: function (keyboard, value, isClosing) { return true; }

    });
}

CommonHelper.TouchScreenNumberKeyboardWithoutDot = function (keyBoardControl, KeyBoardContainer) {

    $("." + keyBoardControl).keyboard({

        language: null,
        rtl: false,

        // *** Keyboard layout ***
        layout: 'custom',
        customLayout: {
            'default': [
                '7 8 9',
                '4 5 6',
                '1 2 3',
                '0 {b} {clear}'
            ]
        },

        position: {
            of: ($("#" + KeyBoardContainer)), // optional - null (attach to input/textarea) or a jQuery object (attach elsewhere)
            my: 'center top',
            at: 'center top',
            at2: 'center top', // used when "usePreview" is false (centers keyboard at bottom of the input/textarea)
            collision: 'fit fit'
        },

        // allow jQuery position utility to reposition the keyboard on window resize
        reposition: false,

        // preview added above keyboard if true, original input/textarea used if false
        usePreview: false,

        // if true, the keyboard will always be visible
        alwaysOpen: false,

        // give the preview initial focus when the keyboard becomes visible
        initialFocus: true,

        // if true, keyboard will remain open even if the input loses focus.
        stayOpen: true,

        // *** change keyboard language & look ***
        display: {
            'a': '\u2714:Accept (Shift-Enter)', // check mark - same action as accept
            'accept': 'Accept:Accept (Shift-Enter)',
            'alt': 'AltGr:Alternate Graphemes',
            'b': '\u2190:Backspace',    // Left arrow (same as &larr;)
            'bksp': 'Bksp:Backspace',
            'c': '\u2716:Cancel (Esc)', // big X, close - same action as cancel
            'cancel': 'Cancel:Cancel (Esc)',
            'clear': 'C:Clear',             // clear num pad
            'combo': '\u00f6:Toggle Combo Keys',
            'dec': '.:Decimal',           // decimal point for num pad (optional), change '.' to ',' for European format
            'e': '\u21b5:Enter',        // down, then left arrow - enter symbol
            'enter': 'Enter:Enter',
            'left': '\u2190',              // left arrow (move caret)
            'lock': '\u21ea Lock:Caps Lock', // caps lock
            'next': 'Next',
            'prev': 'Prev',
            'right': '\u2192',              // right arrow (move caret)
            's': '\u21e7:Shift',        // thick hollow up arrow
            'shift': 'Shift:Shift',
            'sign': '\u00b1:Change Sign',  // +/- sign for num pad
            'space': '&nbsp;:Space',
            't': '\u21e5:Tab',          // right arrow to bar (used since this virtual keyboard works with one directional tabs)
            'tab': '\u21e5 Tab:Tab'       // \u21b9 is the true tab symbol (left & right arrows)
        },

        // Message added to the key title while hovering, if the mousewheel plugin exists
        wheelMessage: 'Use mousewheel to see other keys',

        css: {
            input: 'ui-widget-content ui-corner-all', // input & preview
            container: 'ui-widget-content ui-widget ui-corner-all ui-helper-clearfix', // keyboard container
            buttonDefault: 'ui-state-default ui-corner-all', // default state
            buttonHover: 'ui-state-hover',  // hovered button
            buttonAction: 'ui-state-active', // Action keys (e.g. Accept, Cancel, Tab, etc); replaces "actionClass"
            buttonDisabled: 'ui-state-disabled', // used when disabling the decimal button {dec}
            buttonEmpty: 'ui-keyboard-empty' // empty button class name {empty}
        },

        // *** Useability ***
        // Auto-accept content when clicking outside the keyboard (popup will close)
        autoAccept: true,

        // Prevents direct input in the preview window when true
        lockInput: false,

        // Prevent keys not in the displayed keyboard from being typed in
        restrictInput: true,

        // Check input against validate function, if valid the accept button is clickable;
        // if invalid, the accept button is disabled.
        acceptValid: true,

        // if acceptValid is true & the validate function returns a false, this option will cancel
        // a keyboard close only after the accept button is pressed
        cancelClose: true,

        // Use tab to navigate between input fields
        tabNavigation: false,

        // press enter (shift-enter in textarea) to go to the next input field
        enterNavigation: true,
        // mod key options: 'ctrlKey', 'shiftKey', 'altKey', 'metaKey' (MAC only)
        enterMod: 'altKey', // alt-enter to go to previous; shift-alt-enter to accept & go to previous

        // if true, the next button will stop on the last keyboard input/textarea; prev button stops at first
        // if false, the next button will wrap to target the first input/textarea; prev will go to the last
        stopAtEnd: true,

        // Set this to append the keyboard immediately after the input/textarea it is attached to.
        // This option works best when the input container doesn't have a set width and when the
        // "tabNavigation" option is true
        appendLocally: false,

        // Append the keyboard to a desired element. This can be a jQuery selector string or object
        appendTo: ($("#" + KeyBoardContainer)),

        // If false, the shift key will remain active until the next key is (mouse) clicked on;
        // if true it will stay active until pressed again
        stickyShift: true,

        // caret places at the end of any text
        caretToEnd: false,

        // Prevent pasting content into the area
        preventPaste: true,

        // Set the max number of characters allowed in the input, setting it to false disables this option
        maxLength: false,

        // allow inserting characters '@@' caret when maxLength is set
        maxInsert: true,

        // Mouse repeat delay - when clicking/touching a virtual keyboard key, after this delay the key
        // will start repeating
        repeatDelay: 500,

        // Mouse repeat rate - after the repeatDelay, this is the rate (characters per second) at which the
        // key is repeated. Added to simulate holding down a real keyboard key and having it repeat. I haven't
        // calculated the upper limit of this rate, but it is limited to how fast the javascript can process
        // the keys. And for me, in Firefox, it's around 20.
        repeatRate: 20,

        // resets the keyboard to the default keyset when visible
        resetDefault: false,

        // Event (namespaced) on the input to reveal the keyboard. To disable it, just set it to ''.
        openOn: 'focus', //focus

        // When the character is added to the input
        keyBinding: 'mousedown',

        // combos (emulate dead keys : http://en.wikipedia.org/wiki/Keyboard_layout#US-International)
        // if user inputs `a the script converts it to à, ^o becomes ô, etc.
        useCombos: false,

        // *** Methods ***
        // Callbacks - add code inside any of these callback functions as desired
        initialized: function (e, keyboard, el) { },
        visible: function (e, keyboard, el) { },
        change: function (e, keyboard, el) { },
        beforeClose: function (e, keyboard, el, accepted) { },
        beforeInsert: function (e, keyboard, el, textToAdd) {
            var tests = /^[0-9\b]+$/.test(textToAdd);
            if (!tests) {
                return '';
            }
            else return textToAdd;
        },
        accepted: function (e, keyboard, el) { },
        canceled: function (e, keyboard, el) { },
        hidden: function (e, keyboard, el) { },

        switchInput: null, // called instead of base.switchInput

        // this callback is called just before the "beforeClose" to check the value
        // if the value is valid, return true and the keyboard will continue as it should
        // (close if not always open, etc)
        // if the value is not value, return false and the clear the keyboard value
        // ( like this "keyboard.$preview.val('');" ), if desired
        // The validate function is called after each input, the "isClosing" value will be false;
        // when the accept button is clicked, "isClosing" is true
        validate: function (keyboard, value, isClosing) { return true; }

    });
}

CommonHelper.TouchScreenUserKeyboard = function (keyBoardControl, KeyBoardContainer) {

    $("." + keyBoardControl).keyboard({

        language: null,
        rtl: false,

        // *** Keyboard layout ***
        layout: 'custom',
        customLayout: {
            'normal': [
                '` 1 2 3 4 5 6 7 8 9 0 - = {bksp}',
                '{tab} q w e r t y u i o p [ ] \\',
                'a s d f g h j k l ; \' {enter}',
                '{shift} z x c v b n m , . / {shift}',
                '{clear} {del} {space} {left} {right}'
            ],
            'shift': [
                '~ ! @ # $ % ^ & * ( ) _ + {bksp}',
                '{tab} Q W E R T Y U I O P { } |',
                'A S D F G H J K L : " {enter}',
                '{shift} Z X C V B N M < > ? {shift}',
                '{clear} {del} {space} {left} {right}'
            ]
        },

        position: {
            of: ($("#" + KeyBoardContainer)), // optional - null (attach to input/textarea) or a jQuery object (attach elsewhere)
            my: 'center top',
            at: 'center top',
            at2: 'center top', // used when "usePreview" is false (centers keyboard at bottom of the input/textarea)
            collision: 'fit fit'
        },

        // allow jQuery position utility to reposition the keyboard on window resize
        reposition: false,

        // preview added above keyboard if true, original input/textarea used if false
        usePreview: false,

        // if true, the keyboard will always be visible
        alwaysOpen: false,

        // give the preview initial focus when the keyboard becomes visible
        initialFocus: true,

        // if true, keyboard will remain open even if the input loses focus.
        stayOpen: true,

        // *** change keyboard language & look ***
        display: {
            'a': '\u2714:Accept (Shift-Enter)', // check mark - same action as accept
            'accept': 'Accept:Accept (Shift-Enter)',
            'alt': 'AltGr:Alternate Graphemes',
            'b': '\u2190:Backspace',    // Left arrow (same as &larr;)
            'bksp': 'Bksp:Backspace',
            'c': '\u2716:Cancel (Esc)', // big X, close - same action as cancel
            'cancel': 'Cancel:Cancel (Esc)',
            'clear': 'C:Clear',             // clear num pad
            'combo': '\u00f6:Toggle Combo Keys',
            'dec': '.:Decimal',           // decimal point for num pad (optional), change '.' to ',' for European format
            'e': '\u21b5:Enter',        // down, then left arrow - enter symbol
            'enter': 'Enter:Enter',
            'left': '\u2190',              // left arrow (move caret)
            'lock': '\u21ea Lock:Caps Lock', // caps lock
            'next': 'Next',
            'prev': 'Prev',
            'right': '\u2192',              // right arrow (move caret)
            's': '\u21e7:Shift',        // thick hollow up arrow
            'shift': 'Shift:Shift',
            'sign': '\u00b1:Change Sign',  // +/- sign for num pad
            'space': '&nbsp;:Space',
            't': '\u21e5:Tab',          // right arrow to bar (used since this virtual keyboard works with one directional tabs)
            'tab': '\u21e5 Tab:Tab'       // \u21b9 is the true tab symbol (left & right arrows)
        },

        // Message added to the key title while hovering, if the mousewheel plugin exists
        wheelMessage: 'Use mousewheel to see other keys',

        css: {
            input: 'ui-widget-content ui-corner-all', // input & preview
            container: 'ui-widget-content ui-widget ui-corner-all ui-helper-clearfix', // keyboard container
            buttonDefault: 'ui-state-default ui-corner-all', // default state
            buttonHover: 'ui-state-hover',  // hovered button
            buttonAction: 'ui-state-active', // Action keys (e.g. Accept, Cancel, Tab, etc); replaces "actionClass"
            buttonDisabled: 'ui-state-disabled', // used when disabling the decimal button {dec}
            buttonEmpty: 'ui-keyboard-empty' // empty button class name {empty}
        },

        // *** Useability ***
        // Auto-accept content when clicking outside the keyboard (popup will close)
        autoAccept: true,

        // Prevents direct input in the preview window when true
        lockInput: false,

        // Prevent keys not in the displayed keyboard from being typed in
        restrictInput: false,

        // Check input against validate function, if valid the accept button is clickable;
        // if invalid, the accept button is disabled.
        acceptValid: true,

        // if acceptValid is true & the validate function returns a false, this option will cancel
        // a keyboard close only after the accept button is pressed
        cancelClose: false,

        // Use tab to navigate between input fields
        tabNavigation: false,

        // press enter (shift-enter in textarea) to go to the next input field
        enterNavigation: true,
        // mod key options: 'ctrlKey', 'shiftKey', 'altKey', 'metaKey' (MAC only)
        enterMod: 'altKey', // alt-enter to go to previous; shift-alt-enter to accept & go to previous

        // if true, the next button will stop on the last keyboard input/textarea; prev button stops at first
        // if false, the next button will wrap to target the first input/textarea; prev will go to the last
        stopAtEnd: true,

        // Set this to append the keyboard immediately after the input/textarea it is attached to.
        // This option works best when the input container doesn't have a set width and when the
        // "tabNavigation" option is true
        appendLocally: false,

        // Append the keyboard to a desired element. This can be a jQuery selector string or object
        appendTo: ($("#" + KeyBoardContainer)),

        // If false, the shift key will remain active until the next key is (mouse) clicked on;
        // if true it will stay active until pressed again
        stickyShift: true,

        // caret places at the end of any text
        caretToEnd: false,

        // Prevent pasting content into the area
        preventPaste: false,

        // Set the max number of characters allowed in the input, setting it to false disables this option
        maxLength: false,

        // allow inserting characters '@@' caret when maxLength is set
        maxInsert: true,

        // Mouse repeat delay - when clicking/touching a virtual keyboard key, after this delay the key
        // will start repeating
        repeatDelay: 500,

        // Mouse repeat rate - after the repeatDelay, this is the rate (characters per second) at which the
        // key is repeated. Added to simulate holding down a real keyboard key and having it repeat. I haven't
        // calculated the upper limit of this rate, but it is limited to how fast the javascript can process
        // the keys. And for me, in Firefox, it's around 20.
        repeatRate: 20,

        // resets the keyboard to the default keyset when visible
        resetDefault: false,

        // Event (namespaced) on the input to reveal the keyboard. To disable it, just set it to ''.
        openOn: 'focus', //focus

        // When the character is added to the input
        keyBinding: 'mousedown',

        // combos (emulate dead keys : http://en.wikipedia.org/wiki/Keyboard_layout#US-International)
        // if user inputs `a the script converts it to à, ^o becomes ô, etc.
        useCombos: false,

        // *** Methods ***
        // Callbacks - add code inside any of these callback functions as desired
        initialized: function (e, keyboard, el) { },
        visible: function (e, keyboard, el) { },
        change: function (e, keyboard, el) {
            FocusedInputControl = $(el).attr("id");
            //CalculatePayment();
        },
        beforeClose: function (e, keyboard, el, accepted) { },
        accepted: function (e, keyboard, el) { },
        canceled: function (e, keyboard, el) { },
        hidden: function (e, keyboard, el) { },

        switchInput: null, // called instead of base.switchInput

        // this callback is called just before the "beforeClose" to check the value
        // if the value is valid, return true and the keyboard will continue as it should
        // (close if not always open, etc)
        // if the value is not value, return false and the clear the keyboard value
        // ( like this "keyboard.$preview.val('');" ), if desired
        // The validate function is called after each input, the "isClosing" value will be false;
        // when the accept button is clicked, "isClosing" is true
        validate: function (keyboard, value, isClosing) { return true; }

    });
}

CommonHelper.TouchScreenNumberKeyboardWithoutDotNContainer = function (keyBoardControl) {

    $("." + keyBoardControl).keyboard({

        language: null,
        rtl: false,

        // *** Keyboard layout ***
        layout: 'custom',
        customLayout: {
            'default': [
                '7 8 9',
                '4 5 6',
                '1 2 3',
                '0 {bksp} {clear}',
                '{accept} {cancel}'
            ]
        },

        position: {
            of: null, // optional - null (attach to input/textarea) or a jQuery object (attach elsewhere)
            my: 'center top',
            at: 'center top',
            at2: 'center bottom', // used when "usePreview" is false (centers keyboard at bottom of the input/textarea)
            collision: 'fit fit'
        },

        // allow jQuery position utility to reposition the keyboard on window resize
        reposition: false,

        // preview added above keyboard if true, original input/textarea used if false
        usePreview: false,

        // if true, the keyboard will always be visible
        alwaysOpen: false,

        // give the preview initial focus when the keyboard becomes visible
        initialFocus: false,

        // if true, keyboard will remain open even if the input loses focus.
        stayOpen: true,

        // *** change keyboard language & look ***
        display: {
            'a': '\u2714:Accept (Shift-Enter)', // check mark - same action as accept
            'accept': 'Accept:Accept (Shift-Enter)',
            'alt': 'AltGr:Alternate Graphemes',
            'b': '\u2190:Backspace',    // Left arrow (same as &larr;)
            'bksp': 'Bksp:Backspace',
            'c': '\u2716:Cancel (Esc)', // big X, close - same action as cancel
            'cancel': 'Cancel:Cancel (Esc)',
            'clear': 'C:Clear',             // clear num pad
            'combo': '\u00f6:Toggle Combo Keys',
            'dec': '.:Decimal',           // decimal point for num pad (optional), change '.' to ',' for European format
            'e': '\u21b5:Enter',        // down, then left arrow - enter symbol
            'enter': 'Enter:Enter',
            'left': '\u2190',              // left arrow (move caret)
            'lock': '\u21ea Lock:Caps Lock', // caps lock
            'next': 'Next',
            'prev': 'Prev',
            'right': '\u2192',              // right arrow (move caret)
            's': '\u21e7:Shift',        // thick hollow up arrow
            'shift': 'Shift:Shift',
            'sign': '\u00b1:Change Sign',  // +/- sign for num pad
            'space': '&nbsp;:Space',
            't': '\u21e5:Tab',          // right arrow to bar (used since this virtual keyboard works with one directional tabs)
            'tab': '\u21e5 Tab:Tab'       // \u21b9 is the true tab symbol (left & right arrows)
        },

        // Message added to the key title while hovering, if the mousewheel plugin exists
        wheelMessage: 'Use mousewheel to see other keys',

        css: {
            input: 'ui-widget-content ui-corner-all', // input & preview
            container: 'ui-widget-content ui-widget ui-corner-all ui-helper-clearfix', // keyboard container
            buttonDefault: 'ui-state-default ui-corner-all', // default state
            buttonHover: 'ui-state-hover',  // hovered button
            buttonAction: 'ui-state-active', // Action keys (e.g. Accept, Cancel, Tab, etc); replaces "actionClass"
            buttonDisabled: 'ui-state-disabled', // used when disabling the decimal button {dec}
            buttonEmpty: 'ui-keyboard-empty' // empty button class name {empty}
        },

        // *** Useability ***
        // Auto-accept content when clicking outside the keyboard (popup will close)
        autoAccept: true,

        // Prevents direct input in the preview window when true
        lockInput: false,

        // Prevent keys not in the displayed keyboard from being typed in
        restrictInput: true,

        // Check input against validate function, if valid the accept button is clickable;
        // if invalid, the accept button is disabled.
        acceptValid: true,

        // if acceptValid is true & the validate function returns a false, this option will cancel
        // a keyboard close only after the accept button is pressed
        cancelClose: true,

        // Use tab to navigate between input fields
        tabNavigation: false,

        // press enter (shift-enter in textarea) to go to the next input field
        enterNavigation: true,
        // mod key options: 'ctrlKey', 'shiftKey', 'altKey', 'metaKey' (MAC only)
        enterMod: 'altKey', // alt-enter to go to previous; shift-alt-enter to accept & go to previous

        // if true, the next button will stop on the last keyboard input/textarea; prev button stops at first
        // if false, the next button will wrap to target the first input/textarea; prev will go to the last
        stopAtEnd: true,

        // Set this to append the keyboard immediately after the input/textarea it is attached to.
        // This option works best when the input container doesn't have a set width and when the
        // "tabNavigation" option is true
        appendLocally: false,

        // Append the keyboard to a desired element. This can be a jQuery selector string or object
        appendTo: null,

        // If false, the shift key will remain active until the next key is (mouse) clicked on;
        // if true it will stay active until pressed again
        stickyShift: true,

        // caret places at the end of any text
        caretToEnd: false,

        // Prevent pasting content into the area
        preventPaste: true,

        // Set the max number of characters allowed in the input, setting it to false disables this option
        maxLength: false,

        // allow inserting characters '@@' caret when maxLength is set
        maxInsert: true,

        // Mouse repeat delay - when clicking/touching a virtual keyboard key, after this delay the key
        // will start repeating
        repeatDelay: 500,

        // Mouse repeat rate - after the repeatDelay, this is the rate (characters per second) at which the
        // key is repeated. Added to simulate holding down a real keyboard key and having it repeat. I haven't
        // calculated the upper limit of this rate, but it is limited to how fast the javascript can process
        // the keys. And for me, in Firefox, it's around 20.
        repeatRate: 20,

        // resets the keyboard to the default keyset when visible
        resetDefault: false,

        // Event (namespaced) on the input to reveal the keyboard. To disable it, just set it to ''.
        openOn: 'focus', //focus

        // When the character is added to the input
        keyBinding: 'mousedown',

        // combos (emulate dead keys : http://en.wikipedia.org/wiki/Keyboard_layout#US-International)
        // if user inputs `a the script converts it to à, ^o becomes ô, etc.
        useCombos: false,

        // *** Methods ***
        // Callbacks - add code inside any of these callback functions as desired
        initialized: function (e, keyboard, el) { },
        visible: function (e, keyboard, el) { },
        change: function (e, keyboard, el) { },
        beforeClose: function (e, keyboard, el, accepted) { },
        accepted: function (e, keyboard, el) { },
        beforeInsert: function (e, keyboard, el, textToAdd) {
            var tests = /^[0-9\b]+$/.test(textToAdd);
            if (!tests) {
                return '';
            }
            else return textToAdd;
        },
        canceled: function (e, keyboard, el) { },
        hidden: function (e, keyboard, el) { },

        switchInput: null, // called instead of base.switchInput

        // this callback is called just before the "beforeClose" to check the value
        // if the value is valid, return true and the keyboard will continue as it should
        // (close if not always open, etc)
        // if the value is not value, return false and the clear the keyboard value
        // ( like this "keyboard.$preview.val('');" ), if desired
        // The validate function is called after each input, the "isClosing" value will be false;
        // when the accept button is clicked, "isClosing" is true
        validate: function (keyboard, value, isClosing) { return true; }

    });
}

function SetMessegeType(type) {
    if (type == 1) {
        $('#MessageBox').addClass("alert alert-info").removeClass("alert-success-info");
    }
    else if (type == 2) {
        $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
    }
    else {
        $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
    }
}

CommonHelper.RestaurantBillPrintAndPreview =
    {
        NoPrintAndPreview: 0,
        BillPreviewOnly: 1,
        DirectPrint: 2,
        PrintAndPreview: 3
    }

CommonHelper.KotPrintAndPreview =
    {
        KotNotPrint: 0,
        KotPrint: 1
    }

CommonHelper.IsValidEmail = function (email) {
    var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!regex.test(email)) {
        return false;
    } else {
        return true;
    }

};


CommonHelper.IsValidPhone = function (inputtxt) {
    var regex = /^[+]*[(]{0,1}[0-9]{1,3}[)]{0,1}[-\s\./0-9]*$/;
    if (!regex.test(inputtxt)) {
        return false;
    } else {
        return true;
    }
};

