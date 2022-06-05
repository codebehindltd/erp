$(document).ready(function () {

    $('#MenuExpandable > ul > li:has(ul)').addClass("has-sub");

    $('#MenuExpandable > ul > li > a').click(function () {
        var checkElement = $(this).next();

        $('#MenuExpandable li').removeClass('active');
        $(this).closest('li').addClass('active');


        if ((checkElement.is('ul')) && (checkElement.is(':visible'))) {
            $(this).closest('li').removeClass('active');
            checkElement.slideUp('normal');
        }

        if ((checkElement.is('ul')) && (!checkElement.is(':visible'))) {
            $('#MenuExpandable ul ul:visible').slideUp('normal');
            checkElement.slideDown('normal');
        }

        if (checkElement.is('ul')) {
            return false;
        } else {
            return true;
        }
    });

});