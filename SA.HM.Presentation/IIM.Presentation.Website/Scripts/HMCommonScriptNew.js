CommonHelper.ApplyDecimalValidationWithFivePrecision = function () {
    $('.quantitydecimalWithFivePrecision').keypress(function (event) {
        debugger;
        if ((event.which != 46 || $(this).val().indexOf('.') != -1) &&
            ((event.which < 48 || event.which > 57) &&
                (event.which != 0 && event.which != 8))) {
            event.preventDefault();
        }

        var text = $(this).val();

        if ((text.indexOf('.') != -1) &&
            (text.substring(text.indexOf('.')).length > 5) &&
            (event.which != 0 && event.which != 8) &&
            ($(this)[0].selectionStart >= text.length - 5)) {
            event.preventDefault();
        }
    });
};
CommonHelper.IsValidTime = function (time) {
    var regex = /^(?:(?:0?\d|1[0-2]):[0-5]\d)+$/;

    if ((time.includes("AM")) || (time.includes("PM"))) {
        time = time.slice(0, -3);
    }

    var isValidTime = regex.test(time);

    if (!isValidTime) {
        return false;
    } else {
        return true;
    }
};