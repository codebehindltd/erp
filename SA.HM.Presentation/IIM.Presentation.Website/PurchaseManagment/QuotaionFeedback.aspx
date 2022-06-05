<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="QuotaionFeedback.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.QuotaionFeedback" %>


<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>


        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;

        var RFQuotationTable;
        var RFQuotation = null;
        var ItemsFeedbackListInfo = new Array();

        $(document).ready(function () {

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            if (IsCanSave) {
                $('#btnSupmitSupplierComments').show();
                $('#btnAddSupplierComments').show();
            } else {
                $('#btnSupmitSupplierComments').hide();
                $('#btnAddSupplierComments').hide();
            }


            RFQuotationTable = $("#tblRFQuotation").DataTable({
                data: [],
                columns: [
                    { title: "", "data": "RFQId", visible: false },
                    { title: "Expire Date", "data": "ExpireDateTime", sWidth: '15%' },
                    { title: "Indent Name", "data": "IndentName", sWidth: '40%' },
                    { title: "Remaining Time", "data": null, sWidth: '30%' },
                    { title: "Action", "data": null, sWidth: '15%' }
                ], columnDefs: [
                    {
                        "targets": 1,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            //debugger;
                            return CommonHelper.DateFromDateTimeToDisplay(data, innBoarDateFormat);
                        }
                    }],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {


                    var row = '';
                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }

                    // Set the date we're counting down to
                    var countDownDate = aData.ExpireDateTime.getTime();

                    // Update the count down every 1 second
                    var x = setInterval(function () {

                        // Get today's date and time
                        var now = new Date().getTime();

                        // Find the distance between now and the count down date
                        var distance = countDownDate - now;

                        // Time calculations for days, hours, minutes and seconds
                        var days = Math.floor(distance / (1000 * 60 * 60 * 24));
                        var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                        var seconds = Math.floor((distance % (1000 * 60)) / 1000);

                        var strDay = days > 1 ? "Days" : "Day";
                        var strHour = hours > 1 ? "Hours" : "Hour";
                        var strMin = minutes > 1 ? "Minutes" : "Minute";
                        var strSec = seconds > 1 ? "Seconds" : "Second";
                        // Output the result in an element with id="demo"
                        var timeString = days + " " + strDay + " " + hours + " " + strHour + " "
                            + minutes + " " + strMin + " " + seconds + " " + strSec + " Left To Expire";

                        $('td:eq(' + (nRow.children.length - 2) + ')', nRow).html(timeString);

                        // If the count down is over, write some text 
                        if (distance < 0) {
                            clearInterval(x);
                            $('td:eq(' + (nRow.children.length - 2) + ')', nRow).html("Expired");
                        }
                    }, 1000);




                    //if (IsCanEdit && aData.IsCanEdit) {
                    //    if (aData.TransactionType == "Bill Voucher") {
                    //        row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformBillEdit('" + aData.Id + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";

                    //    }
                    //    else {
                    //        row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformEdit('" + aData.Id + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";
                    //    }
                    //}
                    //if (aData.IsCanDelete && IsCanDelete) {
                    //    row += "&nbsp;&nbsp;<a href='javascript:void();' onclick= \"DeleteCashRequisition('" + aData.Id + "');\"> <img alt='Delete' src='../Images/delete.png' title='Delete' /></a>";
                    //}
                    //if (aData.ApprovedStatus == "Pending" && aData.IsCanCheck) {
                    //    row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return Approval('" + aData.Id + "','" + aData.ApprovedStatus + "');\"><img alt=\"Check\" src=\"../Images/checked.png\" title='Check' /></a>";
                    //}
                    //else if (aData.ApprovedStatus == "Checked" && aData.IsCanApprove) {
                    //    row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return Approval('" + aData.Id + "','" + aData.ApprovedStatus + "');\"><img alt=\"Approve\" src=\"../Images/approved.png\" title='Approve' /></a>";

                    //}

                    //if (aData.TransactionType == "Cash Requisition" && aData.ApprovedStatus == "Approved") {

                    //    if (!aData.HaveCashRequisitionAdjustment) {
                    //        row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformAdustment('" + aData.Id + "');\"><img alt=\"Adjustment\" src=\"../Images/detailsInfo.png\" title='Adjustment' /></a>";
                    //    }
                    //    else {
                    //        row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformAdustmentDetails('" + aData.Id + "');\"><img alt=\"Adjustment\" src=\"../Images/detailsInfo.png\" title='Adjustment' /></a>";
                    //    }

                    //}

                    var now = new Date().getTime();

                    var distance = countDownDate - now;
                    if (distance >= 0) {
                        row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return ShowItemsForQuotation('" + aData.RFQId + "','" + aData.IndentName + "');\"> <img alt=\"Quotation\" src=\"../Images/quotation.png\" title='Quotation' /> </a>";
                    }
                    $('td:eq(' + (nRow.children.length - 1) + ')', nRow).html(row);
                },
                pageLength: UserInfoFromDB.GridViewPageSize,
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

            $('#ContentPlaceHolder1_txtSearchFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                // defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchToDate').datepicker("option", "minDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);


            CommonHelper.ApplyDecimalValidation();

            $('#ContentPlaceHolder1_txtSearchToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchFromDate').datepicker("option", "maxDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);
        });

        function SearchInformation(pageNumber, IsCurrentOrPreviousPage) {
            debugger;
            var gridRecordsCount = RFQuotationTable.data().length;


            var quotationType = $("#ContentPlaceHolder1_ddlQuotationType").val();

            var fromDate = $("#ContentPlaceHolder1_txtSearchFromDate").val();
            var toDate = $("#ContentPlaceHolder1_txtSearchToDate").val();
            if (fromDate == "")
                fromDate = null;
            if (toDate == "")
                toDate = null;
            if (fromDate != "")
                fromDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(fromDate, innBoarDateFormat);
            if (toDate != "")
                toDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(toDate, innBoarDateFormat);

            PageMethods.GetRFQuotation(quotationType, fromDate, toDate, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnRFQuotationLoadingSucceed, OnRFQuotationLoadingFailed);
            return false;
        }

        function AttachFile() {
            //$("#documentsDiv").dialog({
            //    autoOpen: true,
            //    modal: true,
            //    width: 900,
            //    closeOnEscape: true,
            //    resizable: false,
            //    title: "Add Documents",
            //    show: 'slide'
            //});

            var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            //var id = +$("#ContentPlaceHolder1_hfRFQSupplierId").val();
            //var deletedDoc = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();

            //var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var path = "/PurchaseManagment/Images/RequestForQuotation/";
            var category = "RequestForQuotationFeedbackDoc";
            var iframeid = 'frmPrint';
            //var url = "/HMCommon/FileUploadTest.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            var url = "/Common/FileUpload.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            document.getElementById(iframeid).src = url;

            $("#documentsDivNew").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 300,
                closeOnEscape: false,
                resizable: false,
                title: "Documents Upload",
                show: 'slide'
            });

        }

        function UploadComplete() {
            debugger;
            var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var id = +$("#ContentPlaceHolder1_hfRFQSupplierId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();

            PageMethods.LoadContactDocument(id, randomId, deletedDoc, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }
        function OnLoadDocumentSucceeded(result) {

            debugger;
            var guestDoc = result;
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";

            guestDocumentTable += "<table id='DocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            guestDocumentTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }

                guestDocumentTable += "<td align='left' style='width: 50%'>" + guestDoc[row].Name + "</td>";

                if (guestDoc[row].Path != "") {
                    if (guestDoc[row].Extention == ".jpg" || guestDoc[row].Extention == ".png" || guestDoc[row].Extention == ".PNG" || guestDoc[row].Extention == ".JPG")
                        imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    else
                        imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                guestDocumentTable += "<td align='left' style='width: 30%'>" + imagePath + "</td>";

                guestDocumentTable += "<td align='left' style='width: 20%'>";
                guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteGuestDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                guestDocumentTable += "</td>";
                guestDocumentTable += "</tr>";
            }
            guestDocumentTable += "</table>";

            // docc = guestDocumentTable;

            $("#DocumentInfo").html(guestDocumentTable);
        }

        function DeleteGuestDoc(docId, rowIndex) {
            if (confirm("Want to delete?")) {
                var deletedDoc = $("#<%=hfGuestDeletedDoc.ClientID %>").val();

                if (deletedDoc != "")
                    deletedDoc += "," + docId;
                else
                    deletedDoc = docId;

                $("#<%=hfGuestDeletedDoc.ClientID %>").val(deletedDoc);

                $("#trdoc" + rowIndex).remove();
            }
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }

        function OnRFQuotationLoadingSucceed(result) {
            //debugger;
            RFQuotationTable.clear();
            RFQuotationTable.rows.add(result.GridData);
            RFQuotationTable.draw();

            $("#GridPagingContainer ul").html("");
            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
            return false;
        }

        function OnRFQuotationLoadingFailed(error) {
            toastr.error(error.get_message());
            return false;
        }

        function ShowItemsForQuotation(RFQId, IndentName) {

            debugger;
            $("#ItemsForQuotationDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 700,
                closeOnEscape: true,
                resizable: false,
                title: "" + IndentName,
                show: 'slide'
            });
            $("#secondDialog").hide();
            $("#firstDialog").show();

            PageMethods.GetItemsForQuotation(RFQId, OnGetItemsForQuotationLoadingSucceed, OnGetItemsForQuotationLoadingFailed);

        }

        function UnitPriceBlur(indexNum) {
            debugger;

            var unitPrice = $("#WithoutVatAit_text" + indexNum).val() == "" ? 0.0 : $("#WithoutVatAit_text" + indexNum).val();

            var discount = $("#Discount_text" + indexNum).val() == "" ? 0.0 : $("#Discount_text" + indexNum).val();

            if (parseFloat(discount) > 100) {
                toastr.warning("Discount Amount Can Not Be Greater than 100.");
                discount = 0.0;
                $("#Discount_text" + indexNum).val(0.0);
            }

            var unitPriceWithOutVatAit = (parseFloat(unitPrice) - (parseFloat(unitPrice) * parseFloat(discount) / 100)).toFixed(2);

            var vat = parseFloat($("#VAT_text" + indexNum).val());

            var ait = parseFloat($("#AIT_text" + indexNum).val());

            var unitPriceWithVatAit = (parseFloat(unitPriceWithOutVatAit) + (parseFloat(unitPriceWithOutVatAit) * parseFloat(vat) / 100) + (parseFloat(unitPriceWithOutVatAit) * parseFloat(ait) / 100)).toFixed(2);

            var totalNum = parseFloat($("#Quantity_text" + indexNum).val());

            var totalPrice = (totalNum * unitPriceWithVatAit).toFixed(2);

            $("#OfferedUnitPriceWithoutVatAit_text" + indexNum).val(unitPriceWithOutVatAit);
            $("#OfferedUnitPriceWithVatAit_text" + indexNum).val(unitPriceWithVatAit);
            $("#BillingAmountWithVatAit_text" + indexNum).val(totalPrice);


        }

        function OnGetItemsForQuotationLoadingSucceed(result) {
            debugger;
            $("#DocumentInfo").html("");
            $("#RFQuotationItemsContainer").html("");
            RFQuotation = result;
            var quotationdivstr = "";
            quotationdivstr = "<div id=\"quotationaccordion\" style=\"width:100%;\" >";
            $("#ContentPlaceHolder1_txtRemarksSupplier").val("");
            $("#ContentPlaceHolder1_txtIndentBy").val(result.IndentBy);

            $("#ContentPlaceHolder1_txtRemarks").val(result.Description);
            $("#ContentPlaceHolder1_txtNoOfItems").val(result.RFQuotationItems.length);

            var payment = result.PaymentTerm;
            if (payment == "Credit")
                payment += "(" + result.CreditDays + " Days)";

            var itemlen = result.RFQuotationItems.length;
            var i = 0;
            for (i = 0; i < itemlen; i++) {
                quotationdivstr += "<h3 id=\"h" + i + "\"> Item Name: " + result.RFQuotationItems[i].ItemName + "</h3>";
                quotationdivstr += "<div><span id='mi" + i + "' style='display:none;'>" + result.RFQuotationItems[i].RFQId + "</span>" +
                    "<span id='apw" + i + "' style='display:none;'>" + result.RFQuotationItems[i].RFQItemId + "</span>";
                //quotationdivstr += "<p id='input_text" + i + i + "_wrapper'><label for=id1>Checkbox</label></p>";

                quotationdivstr += '<div class=\"form-group\"><div class=\"col-md-2 \"><label for="PaymentTerm' + i + '"> Payment Term </label></div>';
                quotationdivstr += '<div class=\"col-md-4\"><input type=\'text\' class=\'input_text form-control\'  id=\'PaymentTerm_text' + i + '\' placeholder=\'\' value=\'' + payment + '\' disabled=\'disabled\'></div>';

                quotationdivstr += '<div class=\"col-md-2 \"><label for="Quantity' + i + '"> Quantity </label></div>';
                quotationdivstr += '<div class=\"col-md-4\"><input type=\'text\' class=\'input_text form-control\' id=\'Quantity_text' + i + '\' placeholder=\'\' value=\'' + result.RFQuotationItems[i].Quantity + '\' disabled=\'disabled\'></div></div>';

                quotationdivstr += '<div class=\"form-group\">';

                quotationdivstr += '<div class=\"col-md-2 \"><label for="UnitHead' + i + '"> Unit Head </label></div>';
                quotationdivstr += '<div class=\"col-md-4\"><input type=\'text\' class=\'input_text form-control\' id=\'UnitHead_text' + i + '\' placeholder=\'\' value=\'' + result.RFQuotationItems[i].StockUnitName + '\' disabled=\'disabled\'></div></div>';


                quotationdivstr += '<div class=\"form-group\"><div class=\"col-md-2 \"><label for="VAT' + i + '"> VAT </label></div>';
                quotationdivstr += '<div class=\"col-md-4\"><input type=\'text\' class=\'input_text form-control\'  id=\'VAT_text' + i + '\' placeholder=\'\' value=\'' + result.VAT + '\' disabled=\'disabled\'></div>';

                quotationdivstr += '<div class=\"col-md-2 \"><label for="AIT' + i + '"> AIT </label></div>';
                quotationdivstr += '<div class=\"col-md-4\"><input type=\'text\' class=\'input_text form-control\' id=\'AIT_text' + i + '\' placeholder=\'\' value=\'' + result.AIT + '\' disabled=\'disabled\'></div></div>';

                quotationdivstr += '<div class=\"form-group\"><div class=\"col-md-3 \"><label for="WithoutVatAit' + i + '"> Unit Price (Without VAT & AIT) </label></div>';
                quotationdivstr += '<div class=\"col-md-9\"><input type=\'text\' class=\'input_text form-control quantitydecimal\'  id=\'WithoutVatAit_text' + i + '\' placeholder=\'\' onblur=\'UnitPriceBlur("' + i + '")\'></div></div>';

                quotationdivstr += '<div class=\"form-group\"><div class=\"col-md-3 \"><label for="Discount' + i + '"> Discount (%) </label></div>';
                quotationdivstr += '<div class=\"col-md-9\"><input type=\'text\' class=\'input_text form-control quantitydecimal\'  id=\'Discount_text' + i + '\' placeholder=\'\' onblur=\'UnitPriceBlur("' + i + '")\' ></div></div>';

                quotationdivstr += '<div class=\"form-group\"><div class=\"col-md-3 \"><label for="OfferedUnitPriceWithoutVatAit' + i + '"> Offered Unit Price (Without VAT & AIT) </label></div>';
                quotationdivstr += '<div class=\"col-md-9\"><input type=\'text\' class=\'input_text form-control quantitydecimal\'  id=\'OfferedUnitPriceWithoutVatAit_text' + i + '\' placeholder=\'\' disabled=\'disabled\' ></div></div>';

                quotationdivstr += '<div class=\"form-group\"><div class=\"col-md-3 \"><label for="OfferedUnitPriceWithVatAit' + i + '"> Offered Unit Price (With VAT & AIT) </label></div>';
                quotationdivstr += '<div class=\"col-md-9\"><input type=\'text\' class=\'input_text form-control quantitydecimal\'  id=\'OfferedUnitPriceWithVatAit_text' + i + '\' placeholder=\'\' disabled=\'disabled\' ></div></div>';

                quotationdivstr += '<div class=\"form-group\"><div class=\"col-md-3 \"><label for="BillingAmountWithVatAit' + i + '"> Billing Amount (With VAT & AIT) </label></div>';
                quotationdivstr += '<div class=\"col-md-9\"><input type=\'text\' class=\'input_text form-control quantitydecimal\'  id=\'BillingAmountWithVatAit_text' + i + '\' placeholder=\'\' disabled=\'disabled\' ></div></div>';

                quotationdivstr += '<div class=\"form-group\"><div class=\"col-md-3 \"><label for="DeliveryDuration' + i + '"> Delivery Duration (In Days) </label></div>';
                quotationdivstr += '<div class=\"col-md-9\"><input type=\'text\' class=\'input_text form-control quantity\'  id=\'DeliveryDuration_text' + i + '\' placeholder=\'\' ></div></div>';

                quotationdivstr += '<div class=\"form-group\"><div class=\"col-md-3 \"><label for="AdvanceAmount' + i + '"> Advance Amount </label></div>';
                quotationdivstr += '<div class=\"col-md-9\"><input type=\'text\' class=\'input_text form-control quantitydecimal\'  id=\'AdvanceAmount_text' + i + '\' placeholder=\'\' ></div></div>';

                quotationdivstr += '<div class=\"form-group\"><div class=\"col-md-3 \"><label for="OfferValidation' + i + '"> Offer Validation (In Days) </label></div>';
                quotationdivstr += '<div class=\"col-md-9\"><input type=\'text\' class=\'input_text form-control quantity\'  id=\'OfferValidation_text' + i + '\' placeholder=\'\' ></div></div>';

                //
                var strTable = "";
                strTable += '<div class=\"form-group\">';
                strTable = "<table id=\"specificationTbl" + i + "\" class=\"table table-bordered table-condensed table-hover\" style=\"width: 100%;\" cellspacing=\"0\" cellpadding=\"4\">" +
                    "<thead>" +
                    "<tr style=\"color: White; background-color: #44545E; font-weight: bold;\">" +
                    "<th style=\"text-align: center; width: 33%;\">" +
                    "Specification" +
                    "</th>" +
                    "<th style=\"width: 33%;\">" +
                    "Buyer Specification" +
                    "</th>" +
                    "<th style=\"width: 34%;\">" +
                    "Supplier Specification" +
                    "</th>" +
                    "</tr>" +
                    "</thead>" +
                    "<tbody>";
                var j = 0;
                var tableLen = result.RFQuotationItems[i].RFQuotationItemSpecifications.length;

                for (j = 0; j < tableLen; j++) {
                    if (j % 2 == 0) {
                        alternateColor = "style=\"background-color:#E3EAEB;\"";
                    }
                    else
                        alternateColor = "style=\"background-color:#FFFFFF;\"";

                    strTable += "<tr " + alternateColor + ">" +
                        "<td style=\"display:none;\">" +
                        result.RFQuotationItems[i].RFQuotationItemSpecifications[j].Id +
                        "</td>" +
                        "<td style=\"display:none;\">" + result.RFQuotationItems[i].RFQuotationItemSpecifications[j].RFQItemId +
                        "</td>" +
                        "<td style=\" text-align: center; width: 33%;\">" +
                        result.RFQuotationItems[i].RFQuotationItemSpecifications[j].Title +
                        "</td>" +
                        "<td style=\"width: 33%; \">" +
                        result.RFQuotationItems[i].RFQuotationItemSpecifications[j].Value +
                        "</td>" +
                        "<td style=\"width: 34%; \">" +
                        '<input type=\'text\' class=\'input_text form-control\'  id=\'SupplierSpecification_text' + i + j + '\' placeholder=\'\' value=\'As Required\'  >' +
                        "</td>" +

                        "</tr>";
                }
                strTable += " </tbody> </table>";
                //strTable += '</div>';
                //
                quotationdivstr += strTable;

                quotationdivstr += '<div class=\"form-group\"><div class=\"col-md-3 \"><label for="ItemRemarks' + i + '"> Item Remarks </label></div>';
                quotationdivstr += '<div class=\"col-md-9\"><textarea class="form-control" id=\'ItemRemarks_text' + i + '\' placeholder=\'\' ></textarea> </div></div>';

                quotationdivstr += '</div>';
            }

            //function add_field() {
            //    var total_text = document.getElementsByClassName("input_text");
            //    total_text = total_text.length + 1;
            //    document.getElementById("field_div").innerHTML = document.getElementById("field_div").innerHTML +
            //        "<p id='input_text" + total_text + "_wrapper'><input type='text' class='input_text' id='input_text" + total_text + "' placeholder='Enter Text'><input type='button' value='Remove' onclick=remove_field('input_text" + total_text + "');></p>";
            //}

            quotationdivstr += "</div>";

            //$("#pa").html("Hello <b>world!</b>");
            $("#RFQuotationItemsContainer").html(quotationdivstr);
            CommonHelper.ApplyDecimalValidation();
            CommonHelper.ApplyIntigerValidation();

            $("#quotationaccordion").accordion();

            return false;
        }

        function OnGetItemsForQuotationLoadingFailed(error) {
            toastr.error(error.get_message());
            return false;
        }

        function DeliveryCostBlur() {
            debugger;
            var DeliveryCost = parseFloat($("#txtDeliveryCost").val());
            var QuotedAmount = parseFloat($("#ContentPlaceHolder1_txtQuotedAmount").val());
            var ApplicableVatAit = parseFloat($("#ContentPlaceHolder1_txtApplicableVatAit").val());


            $("#ContentPlaceHolder1_txtTotalBillingAmount").val(QuotedAmount + ApplicableVatAit + DeliveryCost);

        }

        function SubmitAndClose() {

            debugger;


            var RFQuotationBO = {
                RFQSupplierId: 0,
                TotalItemQuoted: +$("#ContentPlaceHolder1_txtTotalItemQuoted").val(),
                QuotedAmount: parseFloat($("#ContentPlaceHolder1_txtQuotedAmount").val()),
                ApplicableVatAit: parseFloat($("#ContentPlaceHolder1_txtApplicableVatAit").val()),
                DeliveryCost: parseFloat($("#txtDeliveryCost").val()),
                TotalBillingAmount: parseFloat($("#ContentPlaceHolder1_txtTotalBillingAmount").val()),
                AdditionalInformation: $("#ContentPlaceHolder1_txtRemarksSupplier").val(),

                RFQId: RFQuotation.RFQId,
                RFQuotationItemsFeedback: ItemsFeedbackListInfo

            };

            var hfRandom = $("#<%=RandomProductId.ClientID %>").val();
            var deletedDocuments = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();

            PageMethods.SaveOrUpdateRFQFeedback(RFQuotationBO, hfRandom, deletedDocuments, OnSaveOrUpdateRFQFeedbackSucceeded, OnSaveOrUpdateRFQFeedbackFailed);



        }
        function OnSaveOrUpdateRFQFeedbackSucceeded(result) {

            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                //if (typeof parent.GridPaging === "function")
                //    parent.GridPaging(1, 1);
                $("#ItemsForQuotationDialog").dialog('close');

            }
            debugger;


        }

        function OnSaveOrUpdateRFQFeedbackFailed(error) {
            toastr.error(error.get_message());
            //CommonHelper.SpinnerClose();
        }

        function GoToTheSecondDialog() {
            debugger;
            var totalItem = RFQuotation.RFQuotationItems.length;

            var flag = 0;
            var TotalUnitPriceWithoutVatAit = 0.0;
            var TotalUnitPriceWithVatAit = 0.0;
            ItemsFeedbackListInfo = new Array();
            for (var i = 0; i < totalItem; i++) {



                var UnitPriceWithoutVatAit = parseFloat($("#OfferedUnitPriceWithoutVatAit_text" + i).val() == "" ? 0.0 : $("#OfferedUnitPriceWithoutVatAit_text" + i).val());
                var UnitPriceWithVatAit = parseFloat($("#OfferedUnitPriceWithVatAit_text" + i).val() == "" ? 0.0 : $("#OfferedUnitPriceWithVatAit_text" + i).val());
                var ItemQuantity = parseFloat($("#Quantity_text" + i).val());
                if (UnitPriceWithoutVatAit != 0 || UnitPriceWithVatAit != 0) {
                    flag += 1;
                    var ItemSpecificationsFeedback = new Array();

                    var totalItemSpecifications = RFQuotation.RFQuotationItems[i].RFQuotationItemSpecifications.length;
                    for (var j = 0; j < totalItemSpecifications; j++) {

                        ItemSpecificationsFeedback.push({
                            FeedbackId: 0,
                            RFQuotationItemDetailsId: RFQuotation.RFQuotationItems[i].RFQuotationItemSpecifications[j].Id,
                            Feedback: $("#SupplierSpecification_text" + i + j).val(),
                            RFQSupplierItemId: 0,
                            RFQSupplierId: 0

                        });

                    }

                    ItemsFeedbackListInfo.push({
                        RFQSupplierItemId: 0,
                        RFQSupplierId: 0,
                        ItemId: RFQuotation.RFQuotationItems[i].ItemId,
                        RFQItemId: RFQuotation.RFQuotationItems[i].RFQItemId,
                        UnitPrice: parseFloat($("#WithoutVatAit_text" + i).val() == "" ? 0 : $("#WithoutVatAit_text" + i).val()),
                        Discount: parseFloat($("#Discount_text" + i).val() == "" ? 0 : $("#Discount_text" + i).val()),
                        OfferedUnitPrice: parseFloat($("#OfferedUnitPriceWithoutVatAit_text" + i).val() == "" ? 0 : $("#OfferedUnitPriceWithoutVatAit_text" + i).val()),
                        OfferedUnitPriceWithVatAit: parseFloat($("#OfferedUnitPriceWithVatAit_text" + i).val() == "" ? 0 : $("#OfferedUnitPriceWithVatAit_text" + i).val()),
                        BillingAmount: parseFloat($("#BillingAmountWithVatAit_text" + i).val() == "" ? 0 : $("#BillingAmountWithVatAit_text" + i).val()),
                        AdvanceAmount: parseFloat($("#AdvanceAmount_text" + i).val() == "" ? 0 : $("#AdvanceAmount_text" + i).val()),
                        OfferValidation: parseInt(($("#OfferValidation_text" + i).val() == "" ? 0 : $("#OfferValidation_text" + i).val()), 10),
                        DeliveryDuration: parseInt(($("#DeliveryDuration_text" + i).val() == "" ? 0 : $("#DeliveryDuration_text" + i).val()), 10),
                        ItemRemarks: $("#ItemRemarks_text" + i).val(),
                        RFQuotationItemSpecificationsFeedback: ItemSpecificationsFeedback,
                        Quantity: RFQuotation.RFQuotationItems[i].Quantity,
                        StockUnit: RFQuotation.RFQuotationItems[i].StockUnit
                    });
                }

                TotalUnitPriceWithoutVatAit += (UnitPriceWithoutVatAit * ItemQuantity);
                TotalUnitPriceWithVatAit += (UnitPriceWithVatAit * ItemQuantity);


            }





            if (flag > 0) {

                $("#secondDialog").show();
                $("#firstDialog").hide();
                $("#ContentPlaceHolder1_txtTotalItemQuoted").val(flag);
                $("#ContentPlaceHolder1_txtQuotedAmount").val(TotalUnitPriceWithoutVatAit.toFixed(2));
                $("#ContentPlaceHolder1_txtApplicableVatAit").val((TotalUnitPriceWithVatAit - TotalUnitPriceWithoutVatAit).toFixed(2));
                $("#txtDeliveryCost").val(0);
                $("#ContentPlaceHolder1_txtTotalBillingAmount").val(TotalUnitPriceWithVatAit.toFixed(2));


            }
            else {
                toastr.info("Quote Some Items First.");

            }
            return false;

        }



    </script>

    <div class="panel panel-default">
        <div class="panel-heading">
            Quotaion Feedback
        </div>
        <div class="panel-body">
            <div class="form-horizontal">

                <asp:HiddenField ID="hfRFQSupplierId" runat="server" Value="0"></asp:HiddenField>

                <asp:HiddenField ID="RandomProductId" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="tempId" runat="server" Value="0"></asp:HiddenField>
                <asp:HiddenField ID="hfGuestDeletedDoc" runat="server" Value=""></asp:HiddenField>
                <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
                <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
                <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
                <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />

                <div class="form-group">
                    <div class="col-md-2 ">
                        <asp:Label ID="lblQuotationType" runat="server" class="control-label" Text="Quotation Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList runat="server" ID="ddlQuotationType" CssClass="form-control">
                            <asp:ListItem Text="Unquoted" Value="Unquoted"></asp:ListItem>
                            <asp:ListItem Text="Expired Unquoted" Value="ExpiredUnquoted"></asp:ListItem>
                            <asp:ListItem Text="Quoted" Value="Quoted"></asp:ListItem>
                            <asp:ListItem Text="Expired Quoted" Value="ExpiredQuoted"></asp:ListItem>
                            <asp:ListItem Text="Closed" Value="Closed"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="dvSearchDateTime" class="form-group">
                    <div class="col-md-2 ">
                        <label class="control-label ">From Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchFromDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                    <div class="col-md-2 ">
                        <label class="control-label">To Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchToDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" TabIndex="3" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return SearchInformation(1,1);" />

                    </div>

                </div>

                <div>
                    <table id="tblRFQuotation" class="table table-bordered table-condensed table-responsive">
                    </table>
                    <div class="childDivSection">
                        <div class="text-center" id="GridPagingContainer">
                            <ul class="pagination">
                            </ul>
                        </div>
                    </div>
                </div>

            </div>
            <div id="ItemsForQuotationDialog" style="display: none">
                <div style="padding: 10px 30px 10px 30px">
                    <div class="form-horizontal">

                        <div id="firstDialog">
                            <div class="form-group">

                                <div class="col-md-2 ">
                                    <label class="control-label">Indent By</label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtIndentBy" runat="server" CssClass=" form-control" Enabled="false"></asp:TextBox>
                                </div>


                            </div>

                            <div class="form-group">

                                <div class="col-md-2 ">
                                    <label class="control-label">No Of Items</label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtNoOfItems" runat="server" CssClass=" form-control" Enabled="false"></asp:TextBox>
                                </div>


                            </div>

                            <div class="form-group">
                                <div id="RFQuotationItemsContainer">
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-md-2 ">
                                    <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Buyer Remarks"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine" Enabled="false"
                                        TabIndex="11"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-md-2 ">
                                    <asp:Label ID="lblRemarksSupplier" runat="server" class="control-label" Text="Supplier Remarks"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtRemarksSupplier" runat="server" CssClass="form-control" TextMode="MultiLine"
                                        TabIndex="11"></asp:TextBox>
                                </div>
                            </div>


                            <div class="form-group">
                                <div class="col-md-2 ">
                                    <label class="control-label">Attachment</label>
                                </div>
                                <%--<div class="col-md-10">
                                    <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Documents..." onclick="AttachFile()" />
                                </div>--%>

                                <div class="col-md-10">
                                    <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Attach" onclick="AttachFile()" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div id="documentsDiv" style="display: none;">
                                    <label for="Attachment" class="control-label col-md-2">
                                        Attachment</label>
                                    <div class="col-md-4">
                                        <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
                                            <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                                                FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
                                        </asp:Panel>
                                    </div>
                                </div>
                                <div id="documentsDivNew" style="display: none;">
                                    <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
                                        clientidmode="static" scrolling="yes"></iframe>
                                </div>
                            </div>
                            <div id="DocumentInfo">
                            </div>
                            <div>
                                <input id="btnAddSupplierComments" type="button" value="Next" class="TransactionalButton btn btn-primary btn-sm" onclick="GoToTheSecondDialog()" />

                            </div>
                        </div>
                        <div id="secondDialog" style="display: none">
                            <div class="form-group">

                                <div class="col-md-3 ">
                                    <label class="control-label">Total Item Quoted</label>
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="txtTotalItemQuoted" runat="server" CssClass=" form-control quantitydecimal" Enabled="false"></asp:TextBox>
                                </div>


                            </div>
                            <div class="form-group">

                                <div class="col-md-3 ">
                                    <label class="control-label">Quoted Amount (Without VAT & AIT)</label>
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="txtQuotedAmount" runat="server" CssClass=" form-control quantitydecimal" Enabled="false"></asp:TextBox>
                                </div>


                            </div>
                            <div class="form-group">

                                <div class="col-md-3 ">
                                    <label class="control-label">Applicable VAT & AIT</label>
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="txtApplicableVatAit" runat="server" CssClass=" form-control quantitydecimal" Enabled="false"></asp:TextBox>
                                </div>


                            </div>
                            <div class="form-group">

                                <div class="col-md-3 ">
                                    <label class="control-label">Delivery Cost</label>
                                </div>
                                <div class="col-md-9">
                                    <%--<asp:TextBox ID="txtDeliveryCost" runat="server" CssClass=" form-control quantitydecimal"></asp:TextBox>--%>
                                    <input type="text" class="input_text form-control quantitydecimal" id="txtDeliveryCost" onblur="DeliveryCostBlur()" />
                                </div>


                            </div>
                            <div class="form-group">

                                <div class="col-md-3 ">
                                    <label class="control-label">Total Billing Amount</label>
                                </div>
                                <div class="col-md-9">
                                    <asp:TextBox ID="txtTotalBillingAmount" runat="server" CssClass=" form-control quantitydecimal" Enabled="false"></asp:TextBox>
                                </div>


                            </div>

                            <div>
                                <input id="btnSupmitSupplierComments" type="button" value="Submit" class="TransactionalButton btn btn-primary btn-sm" onclick="SubmitAndClose()" />

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
