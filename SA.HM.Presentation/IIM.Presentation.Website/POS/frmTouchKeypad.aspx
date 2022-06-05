<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmTouchKeypad.aspx.cs"
    Inherits="HotelManagement.Presentation.Website.POS.frmTouchKeypad" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en">
<head>
    <meta charset="utf-8">
    <title>Hotel Management Admin</title>
    <meta content="IE=edge,chrome=1" http-equiv="X-UA-Compatible">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <script src="/StyleSheetOld/lib/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="/StyleSheetOld/lib/bootstrap/css/bootstrap.css">
    <link rel="stylesheet" type="text/css" href="/StyleSheetOld/css/theme.css">
    <link rel="stylesheet" type="text/css" href="/StyleSheetOld/css/HMStyleSheet.css">
</head>
<script type="text/javascript">
    function myFunction(val) {
        var existingValue = $('#txtTouchKeypad').val();
        if (val != 99) {
            $('#txtTouchKeypad').val(existingValue + val);
        }
        else {
            if (existingValue.length > 0) {
                var m = existingValue.substring(0, existingValue.length - 1);
                $('#txtTouchKeypad').val(m);
            }
        }
    }


    $(document).ready(function () {
        /*var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
        var formName = "<span class='divider'>/</span><li class='active'>Touch Keypad</li>";
        var breadCrumbs = moduleName + formName;
        $("#ltlBreadCrumbsInformation").html(breadCrumbs);*/
    });

</script>
<body class="">
    <form id="form1" runat="server" class="search form-inline">
    <div id="TouchKeypad" class="block" style="width: 410px;">
        <input type="text" name="txtTouchKeypad" id="txtTouchKeypad" placeholder="Item Quantity"
            style="width: 396px;" />
        <div class='block-body collapse in'>
            <div class='DivNumericContainer'>
                <%-- <a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='ContentPlaceHolder1_img1' onclick='myFunction(0)' src="/Images/0.jpg" /></div>
                <%--  </a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%-- <a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='ContentPlaceHolder1_img2' onclick='myFunction(1)' src='/Images/1.jpg' /></div>
                <%--</a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%-- <a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='ContentPlaceHolder1_img3' onclick='myFunction(2)' src='/Images/2.jpg' /></div>
                <%--</a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%-- <a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='Img1' onclick='myFunction(3)' src='/Images/3.jpg' /></div>
                <%--</a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%--<a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='Img2' onclick='myFunction(4)' src='/Images/4.jpg' /></div>
                <%--</a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%--<a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='Img3' onclick='myFunction(5)' src='/Images/5.jpg' /></div>
                <%--</a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%--<a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='Img4' onclick='myFunction(6)' src='/Images/6.jpg' /></div>
                <%--</a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%-- <a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='Img5' onclick='myFunction(7)' src='/Images/7.jpg' /></div>
                <%-- </a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%--<a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='Img8' onclick='myFunction(8)' src='/Images/8.jpg' /></div>
                <%-- </a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%--<a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='Img9' onclick='myFunction(9)' src='/Images/9.jpg' /></div>
                <%--</a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%--<a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='Img8' onclick='myFunction(99)' src='/Images/Backspace.jpg' /></div>
                <%-- </a>--%>
            </div>
            <div class='DivNumericContainer'>
                <%--<a onclick='myFunction()' href='#'>--%>
                <div class='NumericItemDiv'>
                    <img id='ImgOk' onclick='myFunction(1)' src='/Images/Ok.jpg' /></div>
                <%--</a>--%>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
