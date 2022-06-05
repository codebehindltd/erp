<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="RequestforQuotaion.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.RequestforQuotaion" %>


<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        var SpecificationList = new Array();
        var ItemWithSpecificationList = new Array();
        var RequisitionItems = new Array();
        var ItemSelected = null;
        var itemid = 0;

        var RequestForQuotationItem = new Array();
        var SupplierListInfo = new Array();

        $(document).ready(function () {

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            //debugger;

            if (IsCanSave) {
                $('#ContentPlaceHolder1_btnSave').show();
                $('#ContentPlaceHolder1_btnGoToTheThirdPage').show();
            } else {
                $('#ContentPlaceHolder1_btnSave').hide();
                $('#ContentPlaceHolder1_btnGoToTheThirdPage').hide();
            }

            CommonHelper.ApplyDecimalValidation();
            CommonHelper.ApplyIntigerValidation();

            $("#ContentPlaceHolder1_ddlRequisitionNumber").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#<%=txtExpiryDate.ClientID %>").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#<%=txtExpiryTime.ClientID%>").timepicker({
                showPeriod: is12HourFormat,
            });



            $("#ContentPlaceHolder1_ddlRequisitionNumber").change(function () {

                var RequsitionId = parseInt($(this).val());

                if (RequsitionId != 0) {
                    PageMethods.GetRequisitionByRequsionId(RequsitionId, OnRequisitionItemSucceeded, OnRequisitionItemFailed);
                }

            });

            $("#ContentPlaceHolder1_ddlPaymentTerm").change(function () {

                var paymentTerm = $(this).val();

                if (paymentTerm == "Credit") {
                    $("#CreditDaysDiv").show();
                }
                else {
                    $("#CreditDaysDiv").hide();
                }
            });

            $("#ContentPlaceHolder1_ddlDeliveryTerms").change(function () {

                var DeliveryTerms = $(this).val();

                if (DeliveryTerms == "OnSite") {
                    $("#siteAddressDiv").hide();
                }
                else {
                    $("#siteAddressDiv").show();
                }
            });





            $("#ContentPlaceHolder1_ddlQuotationType").change(function () {

                var QuotationType = $(this).val();

                if (QuotationType == "AdHoc") {
                    $("#AdhoqRequest").show();
                    $("#RequisitionOrderContainer").hide();
                    $("#RequsitionWiseRequest").hide();

                }
                else {
                    $("#AdhoqRequest").hide();
                    $("#RequisitionOrderContainer").show();
                    $("#RequsitionWiseRequest").show();

                }



            });

            $("#ContentPlaceHolder1_ddlCostCentre").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });



            $("#ContentPlaceHolder1_txtItem").autocomplete({
                source: function (request, response) {

                    var costCenterId = $("#ContentPlaceHolder1_ddlCostCentre").val();
                    var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();




                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../PurchaseManagment/RequestforQuotaion.aspx/ItemSearch',
                        data: JSON.stringify({ searchTerm: request.term, costCenterId: costCenterId, categoryId: categoryId }),
                        dataType: "json",
                        success: function (data) {
                            debugger;
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.ItemId,
                                    ItemName: m.Name,
                                    ItemId: m.ItemId,
                                    CategoryId: m.CategoryId,
                                    ProductType: m.ProductType,
                                    StockBy: m.StockBy,
                                    UnitHead: m.UnitHead,
                                    StockQuantity: m.StockQuantity,
                                    PurchasePrice: m.PurchasePrice,
                                    LastPurchaseDate: m.LastPurchaseDate
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
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    debugger;
                    ItemSelected = ui.item;

                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfItemId").val(ui.item.value);

                    $("#ContentPlaceHolder1_txtCurrentStock").val(ui.item.StockQuantity);
                    $("#ContentPlaceHolder1_txtStockUnit").val(ui.item.UnitHead);
                }
            });

            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });

        });

        function CheckPercentage() {

            debugger;
            var vat = ($("#ContentPlaceHolder1_txtVAT").val());
            vat == "" ? 0 : vat;
            if (parseFloat(vat) > 100) {
                toastr.warning("VAT can not be grater than 100.");
                $("#ContentPlaceHolder1_txtVAT").val("0");
                $("#ContentPlaceHolder1_txtVAT").focus();
                return false;
            }

            var ait = ($("#ContentPlaceHolder1_txtAIT").val());
            ait == "" ? 0 : ait;
            if (parseFloat(ait) > 100) {
                toastr.warning("AIT can not be grater than 100.");
                $("#ContentPlaceHolder1_txtAIT").val("0");
                $("#ContentPlaceHolder1_txtAIT").focus();
                return false;
            }

            return false;

        }

        function GoToTheSecondPage() {

            if (RequestForQuotationItem.length > 0) {

                $("#SecondPage").show();
                $("#initialPage").hide();
                $("#ThirdPage").hide();

                debugger;
                var itemList = "";
                for (var j = 0; j < RequestForQuotationItem.length; j++) {
                    if (itemList == "") {
                        itemList = "" + RequestForQuotationItem[j].ItemId;
                    }
                    else {
                        itemList = itemList + "," + RequestForQuotationItem[j].ItemId;
                    }
                }

                PageMethods.GetPMSupplierInfoUsingItemList(itemList, OnPMSupplierInfoUsingItemListSucceeded, OnPMSupplierInfoUsingItemListFailed);


            }
            else {
                toastr.info("Add Some Item First.");

            }
            return false;

        }

        function OnPMSupplierInfoUsingItemListSucceeded(result) {


            $("#OrderCheckAll").prop("checked", true);
            $("#SupplierInformationTbl tbody").html("");
            var totalRow = result.length, row = 0, status = "";
            var tr = "";

            var rowLength = result.length;
            var row = 0;


            for (row = 0; row < rowLength; row++) {

                tr += "<tr>";

                tr += "<td style='width:10%; text-align: center;'>" +
                    "<input type='checkbox' checked='checked' id='check' " + result[row].SupplierId + " />" +
                    "</td>";


                tr += "<td style='width:90%;'>" + result[row].Name + "</td>";


                tr += "<td style='display:none;'>" + result[row].SupplierId + "</td>";
                tr += "<td style='display:none;'>" + result[row].Code + "</td>";

                tr += "</tr>";

                $("#SupplierInformationTbl tbody").append(tr);

                tr = "";
            }


        }

        function OnPMSupplierInfoUsingItemListFailed(error) {
            toastr.error(error.get_message());
            //CommonHelper.SpinnerClose();
        }

        function GoToTheThirdPage() {

            var itemId = "", tr = "";
            debugger;
            SupplierListInfo = new Array();

            var tableLength = $("#SupplierInformationTbl tbody tr").length;

            var rqRow = 0, message = "";
            for (rqRow = 0; rqRow < tableLength; rqRow++) {

                if ($("#SupplierInformationTbl tbody tr:eq(" + rqRow + ")").find("td:eq(0)").find("input").is(":checked")) {

                    var supplierId = $("#SupplierInformationTbl tbody tr:eq(" + rqRow + ")").find("td:eq(2)").text();


                    SupplierListInfo.push({
                        SupplierId: parseInt(supplierId, 10),
                        RFQId: 0
                    });
                }
            }

            if (SupplierListInfo.length > 0) {



                var IndentName = $("#ContentPlaceHolder1_txtIndentName").val();
                if (IndentName == "") {
                    toastr.warning("Add Indent Name.");
                    $("#ContentPlaceHolder1_txtIndentName").focus();
                    return false;
                }

                var ExpiryDate = $("#ContentPlaceHolder1_txtExpiryDate").val();
                if (ExpiryDate == "") {
                    toastr.warning("Add Expiry Date.");
                    $("#ContentPlaceHolder1_txtExpiryDate").focus();
                    return false;
                }
                var paymentTerm = $("#ContentPlaceHolder1_ddlPaymentTerm").val();

                if (paymentTerm == "Credit") {

                    var Days = $("#ContentPlaceHolder1_txtDays").val();
                    if (Days == "") {
                        toastr.warning("Add Credit Days.");
                        $("#ContentPlaceHolder1_txtDays").focus();
                        return false;
                    }
                }

                var DeliveryTerms = $("#ContentPlaceHolder1_ddlDeliveryTerms").val();

                if (DeliveryTerms == "Works") {

                    var Address = $("#ContentPlaceHolder1_txtSiteAddress").val();
                    if (Address == "") {
                        toastr.warning("Add Site Address.");
                        $("#ContentPlaceHolder1_txtSiteAddress").focus();
                        return false;
                    }
                }



                var Date = $("#<%=txtExpiryDate.ClientID %>").val();
                var Time = $("#<%=txtExpiryTime.ClientID %>").val();
                if (Date != "") {
                    Date = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(Date, innBoarDateFormat);
                }
                var expiryDateTime = Date;
                if (Time != "") {
                    expiryDateTime = Date + " " + Time;
                }
                var RFQuotationBO = {
                    RFQId: 0,
                    StoreID: +$("#ContentPlaceHolder1_ddlCostCentre").val(),
                    Description: $("#ContentPlaceHolder1_txtRemarks").val(),
                    IndentName: $("#ContentPlaceHolder1_txtIndentName").val(),
                    PaymentTerm: $("#ContentPlaceHolder1_ddlPaymentTerm").val(),
                    CreditDays: parseFloat($("#ContentPlaceHolder1_txtDays").val() == "" ? 0 : $("#ContentPlaceHolder1_txtDays").val()),
                    DeliveryTerms: $("#ContentPlaceHolder1_ddlDeliveryTerms").val(),
                    SiteAddress: $("#ContentPlaceHolder1_txtSiteAddress").val(),
                    ExpireDateTime: expiryDateTime,
                    VAT: parseFloat($("#ContentPlaceHolder1_txtVAT").val() == "" ? 0 : $("#ContentPlaceHolder1_txtVAT").val()),
                    AIT: parseFloat($("#ContentPlaceHolder1_txtAIT").val() == "" ? 0 : $("#ContentPlaceHolder1_txtAIT").val()),
                    IndentPurpose: $("#ContentPlaceHolder1_txtIndentPurpose").val(),
                    RFQuotationItems: RequestForQuotationItem
                };

                var hfRandom = $("#<%=RandomProductId.ClientID %>").val();
                var deletedDocuments = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();
                var deletedDocumentsForSupplier = $("#ContentPlaceHolder1_hfGuestDeletedDocForSupplier").val();


                PageMethods.SaveOrUpdateRFQ(RFQuotationBO, SupplierListInfo, hfRandom, deletedDocuments, deletedDocumentsForSupplier, OnSaveOrUpdateRFQSucceeded, OnSaveOrUpdateRFQFailed);








            }
            else {
                toastr.info("Select Supplier.");

            }
            return false;

        }

        function OnSaveOrUpdateRFQSucceeded(result) {

            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                //if (typeof parent.GridPaging === "function")
                //    parent.GridPaging(1, 1);
                $("#SecondPage").hide();
                $("#initialPage").hide();
                $("#ThirdPage").show();

            }
            debugger;


        }

        function OnSaveOrUpdateRFQFailed(error) {
            toastr.error(error.get_message());
            //CommonHelper.SpinnerClose();
        }



        function ClearSpecificationDiv() {
            $("#ContentPlaceHolder1_txtTitle").val("");
            $("#ContentPlaceHolder1_txtValue").val("");
        }
        function ClearSpecificationDivInDialoug() {
            $("#ContentPlaceHolder1_txtTitleInDialoug").val("");
            $("#ContentPlaceHolder1_txtValueInDialoug").val("");
        }

        function CalculateTotalForRequsition(control) {

            //debugger;
            var tr = $(control).parent().parent();

            var itemId = parseInt($.trim($(tr).find("td:eq(8)").text()), 10);

            var quantity = $.trim($(tr).find("td:eq(5)").find("input").val());
            // debugger;

            if (quantity == "" || quantity == "0") {
                toastr.info("Request Quantity Cannot Be Zero Or Empty.");
                return false;
            }


            var ItemOBJ = _.findWhere(RequisitionItems, { ItemId: itemId });
            var index = _.indexOf(RequisitionItems, ItemOBJ);

            RequisitionItems[index].Quantity = (quantity);
        }

        function CalculateTotalForAdhoq(control) {

            //debugger;
            var tr = $(control).parent().parent();

            var title = $(tr).find("td:eq(0)").text();

            var ValueText = $.trim($(tr).find("td:eq(1)").find("input").val());
            // debugger;

            if (ValueText == "") {
                toastr.info("Value Cannot Be Empty.");
                return false;
            }


            var TitleOBJ = _.findWhere(SpecificationList, { Title: title });
            var index = _.indexOf(SpecificationList, TitleOBJ);

            SpecificationList[index].Value = (ValueText);
        }

        function CalculateTotalForAdhoqItem(control) {

            debugger;
            var tr = $(control).parent().parent();

            var itemId = parseInt($.trim($(tr).find("td:eq(5)").text()), 10);

            var quantity = $.trim($(tr).find("td:eq(3)").find("input").val());
            // debugger;

            if (quantity == "" || quantity == "0") {
                toastr.info("Quantity Cannot Be Zero Or Empty.");
                return false;
            }


            var ItemOBJ = _.findWhere(RequestForQuotationItem, { ItemId: itemId });
            var index = _.indexOf(RequestForQuotationItem, ItemOBJ);

            RequestForQuotationItem[index].Quantity = (quantity);

        }



        function DeleteAdhoqItem(control) {

            if (!confirm("Do you want to delete?")) { return false; }

            debugger;

            var tr = $(control).parent().parent();

            var title = $(tr).find("td:eq(0)").text();
            //var detailsId = parseInt($.trim($(tr).find("td:eq(5)").text()), 10);

            var TitleOBJ = _.findWhere(SpecificationList, { Title: title });
            var index = _.indexOf(SpecificationList, TitleOBJ);

            //if (parseInt(detailsId, 10) > 0)
            //    RequisitionForBillVoucherDeleted.push(JSON.parse(JSON.stringify(CostCenter)));

            SpecificationList.splice(index, 1);
            $(tr).remove();

        }

        function DeleteAdhoqItemForQuotation(control) {

            if (!confirm("Do you want to delete?")) { return false; }

            debugger;

            var tr = $(control).parent().parent();
            var itemId = parseInt($.trim($(tr).find("td:eq(5)").val()), 10);
            //var detailsId = parseInt($.trim($(tr).find("td:eq(5)").text()), 10);

            var ItemOBJ = _.findWhere(RequestForQuotationItem, { ItemId: itemId });
            var index = _.indexOf(RequestForQuotationItem, ItemOBJ);

            //if (parseInt(detailsId, 10) > 0)
            //    RequisitionForBillVoucherDeleted.push(JSON.parse(JSON.stringify(CostCenter)));

            RequestForQuotationItem.splice(index, 1);
            $(tr).remove();

        }
        function AddItemInDialoug() {

            var title = $("#ContentPlaceHolder1_txtTitleInDialoug").val();
            if (title == "") {
                toastr.warning("Add Title.");
                $("#ContentPlaceHolder1_txtTitleInDialoug").focus();
                return false;
            }

            var valueText = $("#ContentPlaceHolder1_txtValueInDialoug").val();



            var tr = "";
            tr += "<tr>";

            tr += "<td style='width:45%;'>" + title + "</td>";

            tr += "<td style='width:45%;'>" +
                "<input type='text' value='" + valueText + "' id='pp" + title + "' class='form-control' onblur='CalculateTotalForAdhoq(this)'  />" +
                "</td>";


            tr += "<td style='width:10%;'>";
            tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
            tr += "</td>";

            tr += "<td style='display:none;'>" + 0 + "</td>";

            tr += "</tr>";

            $("#SpecificationForRequestTblInDialoug tbody").prepend(tr);
            tr = "";


            debugger;
            SpecificationList.push({
                Title: title,
                Value: valueText,
                RFQItemId: 0,
                Id: 0
            });

            ClearSpecificationDivInDialoug();
            $("#ContentPlaceHolder1_txtTitleInDialoug").focus();
        }

        function AddItem() {

            var title = $("#ContentPlaceHolder1_txtTitle").val();
            if (title == "") {
                toastr.warning("Add Title.");
                $("#ContentPlaceHolder1_txtTitle").focus();
                return false;
            }

            var valueText = $("#ContentPlaceHolder1_txtValue").val();



            var tr = "";
            tr += "<tr>";

            tr += "<td style='width:45%;'>" + title + "</td>";

            tr += "<td style='width:45%;'>" +
                "<input type='text' value='" + valueText + "' id='pp" + title + "' class='form-control' onblur='CalculateTotalForAdhoq(this)'  />" +
                "</td>";


            tr += "<td style='width:10%;'>";
            tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
            tr += "</td>";

            tr += "<td style='display:none;'>" + 0 + "</td>";

            tr += "</tr>";

            $("#SpecificationForRequestTbl tbody").prepend(tr);
            tr = "";
            debugger;

            SpecificationList.push({
                Title: title,
                Value: valueText,
                RFQItemId: 0,
                Id: 0
            });

            ClearSpecificationDiv();
            $("#ContentPlaceHolder1_txtTitle").focus();
        }
        function UploadComplete() {
            debugger;
            var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var id = +$("#ContentPlaceHolder1_hfRFQId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();

            PageMethods.LoadContactDocument(id, randomId, deletedDoc, OnLoadDocumentSucceeded, OnLoadDocumentFailed);

            UploadCompleteForSupplier();

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

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }

        function UploadCompleteForSupplier() {
            debugger;
            var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var id = +$("#ContentPlaceHolder1_hfRFQId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfGuestDeletedDocForSupplier").val();

            PageMethods.LoadContactDocumentForSupplier(id, randomId, deletedDoc, OnLoadDocumentSucceededForSupplier, OnLoadDocumentFailedForSupplier);
            return false;
        }
        function OnLoadDocumentSucceededForSupplier(result) {

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
                guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteGuestDocForSupplier('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                guestDocumentTable += "</td>";
                guestDocumentTable += "</tr>";
            }
            guestDocumentTable += "</table>";

            // docc = guestDocumentTable;

            $("#DocumentInfoForSupplier").html(guestDocumentTable);
        }

        function OnLoadDocumentFailedForSupplier(error) {
            toastr.error(error.get_message());
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
            //var id = +$("#ContentPlaceHolder1_hfRFQId").val();
            //var deletedDoc = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();


            //var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var path = "/PurchaseManagment/Images/RequestForQuotation/";
            var category = "RequestForQuotationDoc";
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
        function AttachFileForSupplier() {

            var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            //var id = +$("#ContentPlaceHolder1_hfRFQId").val();
            //var deletedDoc = $("#ContentPlaceHolder1_hfGuestDeletedDocForSupplier").val();

            var path = "/PurchaseManagment/Images/RequestForQuotation/";
            var category = "RequestForQuotationNSupplierDoc";
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


            //$("#documentsDivForSupplier").dialog({
            //    autoOpen: true,
            //    modal: true,
            //    width: 900,
            //    closeOnEscape: true,
            //    resizable: false,
            //    title: "Add Documents",
            //    show: 'slide'
            //});
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

        function DeleteGuestDocForSupplier(docId, rowIndex) {
            if (confirm("Want to delete?")) {
                var deletedDoc = $("#<%=hfGuestDeletedDocForSupplier.ClientID %>").val();

                if (deletedDoc != "")
                    deletedDoc += "," + docId;
                else
                    deletedDoc = docId;

                $("#<%=hfGuestDeletedDocForSupplier.ClientID %>").val(deletedDoc);

                $("#trdoc" + rowIndex).remove();
            }
        }

        function AddItemForQuotation() {

            if ($("#ContentPlaceHolder1_ddlQuotationType").val() == "AdHoc") {
                AddItemForAdhoqQuotation();
            }
            else {
                AddItemForQuotationFromRequisition();
            }
        }

        function AddItemForQuotationFromRequisition() {

            if ($("#ItemWithRequsitionForRequestTbl tbody tr").find("td:eq(0)").find("input").is(":checked") == false) {
                toastr.warning("Please Select Item Before Quotation.");
                return false;
            }
            else if ($("#ContentPlaceHolder1_ddlQuotationType").val() == 'RequisitionWise' && $("#ItemWithRequsitionForRequestTbl tbody tr").length == 0) {
                toastr.warning("Please Load Item From Requisition For Quotation.");
                return false;
            }
            debugger;

            var itemId = "", tr = "";

            var tableLength = $("#ItemWithRequsitionForRequestTbl tbody tr").length;

            var rqRow = 0, message = "";
            for (rqRow = 0; rqRow < tableLength; rqRow++) {

                if ($("#ItemWithRequsitionForRequestTbl tbody tr:eq(" + rqRow + ")").find("td:eq(0)").find("input").is(":checked")) {

                    itemId = $("#ItemWithRequsitionForRequestTbl tbody tr:eq(" + rqRow + ")").find("td:eq(8)").text();
                    var itm = _.findWhere(RequestForQuotationItem, { ItemId: parseInt(itemId, 10) });

                    if (itm != null) {
                        toastr.warning("Same Item Already Added. Duplicate Item Is Not Accepted.");
                        return false;
                    }

                    var ItemOBJ = _.findWhere(RequisitionItems, { ItemId: parseInt(itemId, 10) });
                    var index = _.indexOf(RequisitionItems, ItemOBJ);

                    //RequisitionItems[index].Quantity = (quantity);

                    RequestForQuotationItem.push({
                        ItemId: parseInt(RequisitionItems[index].ItemId, 10),
                        StockUnit: parseInt(RequisitionItems[index].StockById, 10),
                        Quantity: parseFloat(RequisitionItems[index].Quantity),
                        RFQuotationItemSpecifications: RequisitionItems[index].SpecificationList,
                        RFQItemId: 0,
                        RFQId: 0
                    });



                    tr = "";

                    tr += "<tr>";

                    tr += "<td style='width:30%;'>" + RequisitionItems[index].ItemName + "</td>";

                    tr += "<td style='width:15%;'>" + RequisitionItems[index].StockQuantity + "</td>";

                    tr += "<td style='width:15%;'>" + RequisitionItems[index].UnitHead + "</td>";

                    tr += "<td style='width:30%;'>" +
                        "<input type='text' value='" + RequisitionItems[index].Quantity + "' id='pp" + RequisitionItems[index].ItemId + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoqItem(this)' />" +
                        "</td>";


                    tr += "<td style='width:10%;'>" +
                        "<a href='javascript:void()' onclick= 'DeleteAdhoqItemForQuotation(this)' ><img alt='Delete' src='../Images/delete.png' /></a>" +
                        "<a href=\"javascript:void()\" onclick= \"ShowSpecifications(\'" + RequisitionItems[index].ItemId + "\',\'" + RequisitionItems[index].ItemName + "\')\" ><img alt='Specifications' src='../Images/detailsInfo.png' /></a>" +
                        "</td>";


                    tr += "<td style='display:none;'>" + RequisitionItems[index].ItemId + "</td>";
                    tr += "<td style='display:none;'>" + RequisitionItems[index].CategoryId + "</td>";
                    tr += "<td style='display:none;'>" + RequisitionItems[index].StockById + "</td>";

                    tr += "<td style='display:none;'>0</td>";

                    tr += "</tr>";

                    $("#ItemWithSpecificationForRequestTbl tbody").prepend(tr);
                    tr = "";


                }
            }

            CommonHelper.ApplyDecimalValidation();

            RequisitionItems = new Array();
            $("#ItemWithRequsitionForRequestTbl tbody").html("");
        }
        function AddItemForAdhoqQuotation() {
            debugger;

            if (ItemSelected == null) {
                toastr.warning("Please Select Item.");
                return false;
            }
            else if ($.trim($("#ContentPlaceHolder1_txtQuantity").val()) == "" || $.trim($("#ContentPlaceHolder1_txtQuantity").val()) == "0") {
                toastr.warning("Please Give Quantity.");
                $("#ContentPlaceHolder1_txtQuantity").focus();
                return false;
            }


            var itm = _.findWhere(RequestForQuotationItem, { ItemId: ItemSelected.ItemId });

            if (itm != null) {
                toastr.warning("Same Item Already Added. Duplicate Item Is Not Accepted.");
                return false;
            }


            var total = 0, unitPrice = 0, quantity = 0, tr = "", remarks = "", t = "";


            quantity = $("#ContentPlaceHolder1_txtQuantity").val();

            tr += "<tr>";

            tr += "<td style='width:30%;'>" + ItemSelected.ItemName + "</td>";

            tr += "<td style='width:15%;'>" + ItemSelected.StockQuantity + "</td>";

            tr += "<td style='width:15%;'>" + ItemSelected.UnitHead + "</td>";

            tr += "<td style='width:30%;'>" +
                "<input type='text' value='" + quantity + "' id='pp" + ItemSelected.ItemId + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoqItem(this)' />" +
                "</td>";


            tr += "<td style='width:10%;'>" +
                "<a href='javascript:void()' onclick= 'DeleteAdhoqItemForQuotation(this)' ><img alt='Delete' src='../Images/delete.png' /></a>" +
                "<a href=\"javascript:void()\" onclick= \"ShowSpecifications(\'" + ItemSelected.ItemId + "\',\'" + ItemSelected.ItemName + "\')\" ><img alt='Specifications' src='../Images/detailsInfo.png' /></a>" +
                "</td>";


            tr += "<td style='display:none;'>" + ItemSelected.ItemId + "</td>";
            tr += "<td style='display:none;'>" + ItemSelected.CategoryId + "</td>";
            tr += "<td style='display:none;'>" + ItemSelected.StockBy + "</td>";

            tr += "<td style='display:none;'>0</td>";

            tr += "</tr>";

            $("#ItemWithSpecificationForRequestTbl tbody").prepend(tr);
            tr = "";

            RequestForQuotationItem.push({
                ItemId: parseInt(ItemSelected.ItemId, 10),
                StockUnit: parseInt(ItemSelected.StockBy, 10),
                Quantity: parseFloat(quantity),
                RFQuotationItemSpecifications: SpecificationList,
                RFQItemId: 0,
                RFQId: 0
            });



            debugger;

            SpecificationList = new Array();




            CommonHelper.ApplyDecimalValidation();
            ClearAfterAdhoqQuotationItemAdded();
            $("#ContentPlaceHolder1_txtItem").focus();
        }

        function ShowSpecifications(Itemid, itemName) {

            debugger;
            $("#specificationDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 500,
                closeOnEscape: true,
                resizable: false,
                title: (Itemid > 0) ? ("Edit Specification For " + itemName) : ("Add Specification For " + itemName),
                show: 'slide'
            });
            itemid = Itemid;

            var ItemOBJ = _.findWhere(RequestForQuotationItem, { ItemId: parseInt(Itemid) });
            var index = _.indexOf(RequestForQuotationItem, ItemOBJ);

            SpecificationList = new Array();
            SpecificationList = RequestForQuotationItem[index].RFQuotationItemSpecifications;



            var i = 0;
            var tr = "";
            $("#SpecificationForRequestTblInDialoug tbody").html("");
            for (i = 0; i < SpecificationList.length; i++) {
                tr += "<tr>";

                tr += "<td style='width:45%;'>" + SpecificationList[i].Title + "</td>";

                tr += "<td style='width:45%;'>" +
                    "<input type='text' value='" + SpecificationList[i].Value + "' id='pp" + SpecificationList[i].Title + "' class='form-control' onblur='CalculateTotalForAdhoq(this)'  />" +
                    "</td>";


                tr += "<td style='width:10%;'>";
                tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                tr += "</td>";

                tr += "<td style='display:none;'>" + 0 + "</td>";

                tr += "</tr>";



            }
            $("#SpecificationForRequestTblInDialoug tbody").prepend(tr);
            tr = "";


            return false;
        }

        function AddSpecificationsToItems() {
            debugger;
            if (itemid > 0) {
                var ItemOBJ = _.findWhere(RequestForQuotationItem, { ItemId: parseInt(itemid) });
                var index = _.indexOf(RequestForQuotationItem, ItemOBJ);
                RequestForQuotationItem[index].RFQuotationItemSpecifications = SpecificationList;
                SpecificationList = new Array();
                itemid = 0;
            }



            $("#specificationDialog").dialog('close');
            return false;

        }

        function ClearAfterAdhoqQuotationItemAdded() {
            $("#ContentPlaceHolder1_ddlQuotationType").val("AdHoc").trigger('change');
            $("#ContentPlaceHolder1_ddlRequisitionNumber").val("0").trigger('change');
            $("#ItemWithRequsitionForRequestTbl tbody").html("");

            $("#ContentPlaceHolder1_txtItem").val("");
            $("#ContentPlaceHolder1_txtCurrentStock").val("");
            $("#ContentPlaceHolder1_txtStockUnit").val("");
            $("#ContentPlaceHolder1_txtQuantity").val("");

            ClearSpecificationDiv();
            SpecificationList = new Array();

            $("#SpecificationForRequestTbl tbody").html("");

            ItemSelected = null;
        }

        function CheckAllOrder() {
            if ($("#OrderCheck").is(":checked")) {
                $("#ItemWithRequsitionForRequestTbl tbody tr").find("td:eq(0)").find("input").prop("checked", true);
            }
            else {
                $("#ItemWithRequsitionForRequestTbl tbody tr").find("td:eq(0)").find("input").prop("checked", false);
            }
        }

        function CheckAllOrderAll() {
            if ($("#OrderCheckAll").is(":checked")) {
                $("#SupplierInformationTbl tbody tr").find("td:eq(0)").find("input").prop("checked", true);
            }
            else {
                $("#SupplierInformationTbl tbody tr").find("td:eq(0)").find("input").prop("checked", false);
            }
        }

        function OnRequisitionItemSucceeded(result) {

            RequisitionItems = new Array();

            $("#OrderCheck").prop("checked", true);
            $("#ItemWithRequsitionForRequestTbl tbody").html("");
            var totalRow = result.length, row = 0, status = "";
            var tr = "";

            var rowLength = result.RequisitionDetails.length;
            var row = 0;


            for (row = 0; row < rowLength; row++) {
                debugger;
                tr += "<tr>";

                tr += "<td style='width:5%;'>" +
                    "<input type='checkbox' checked='checked' id='chk' " + result.RequisitionDetails[row].ItemId + " />" +
                    "</td>";

                //if (result[row].SupplierId > 0) {
                //    tr += "<td style='width:5%;'>" +
                //        "<input type='checkbox' checked='checked' id='chk' " + result[row].ItemId + " />" +
                //        "</td>";
                //}
                //else {
                //    tr += "<td style='width:5%;'></td>";
                //}

                //tr += "<td style='width:5%;'></td>";

                tr += "<td style='width:25%;'>" + result.RequisitionDetails[row].ItemId + "-" + result.RequisitionDetails[row].ItemName + "</td>";
                tr += "<td style='width:15%;'>" + result.RequisitionDetails[row].CurrentStockFromStore + "</td>";
                tr += "<td style='width:15%;'>" + result.RequisitionDetails[row].ApprovedQuantity + "</td>";


                tr += "<td style='width:20%;'>" + result.RequisitionDetails[row].HeadName + "</td>";

                tr += "<td style='width:20%;'>" +
                    "<input type='text' value='" + result.RequisitionDetails[row].ApprovedQuantity + "' id='pq' " + result.RequisitionDetails[row].ItemId + " class='form-control quantitydecimal' onblur='CalculateTotalForRequsition(this)' />" +
                    "</td>";

                //tr += "<td style='width:8%;'>" +
                //    "<input type='text' value='" + result[row].PurchasePrice + "' id='pp' " + result[row].ItemId + " class='form-control quantitydecimal' onblur='CheckRequisitionWiseItemPrice(this)' />" +
                //    "</td>";

                //if (result[row].Remarks != null)
                //    tr += "<td style='width:10%;'>" + result[row].Remarks + "</td>";
                //else
                //    tr += "<td style='width:10%;'></td>";

                tr += "<td style='display:none;'>" + result.RequisitionDetails[row].RequisitionId + "</td>";
                tr += "<td style='display:none;'>" + result.RequisitionDetails[row].RequisitionDetailsId + "</td>";
                tr += "<td style='display:none;'>" + result.RequisitionDetails[row].ItemId + "</td>";
                tr += "<td style='display:none;'>" + result.RequisitionDetails[row].StockById + "</td>";

                tr += "</tr>";

                $("#ItemWithRequsitionForRequestTbl tbody").append(tr);
                RequisitionItems.push({
                    ItemId: parseInt(result.RequisitionDetails[row].ItemId, 10),
                    StockById: parseInt(result.RequisitionDetails[row].StockById, 10),
                    Quantity: parseFloat(result.RequisitionDetails[row].ApprovedQuantity),
                    SpecificationList: new Array(),
                    DetailId: 0,
                    ItemName: result.RequisitionDetails[row].ItemId + "-" + result.RequisitionDetails[row].ItemName,
                    StockQuantity: result.RequisitionDetails[row].CurrentStockFromStore,
                    UnitHead: result.RequisitionDetails[row].HeadName,
                    CategoryId: result.RequisitionDetails[row].CategoryId

                });
                tr = "";
            }

            CommonHelper.ApplyDecimalValidation();

        }

        function OnRequisitionItemFailed(error) {
            toastr.error(error.get_message());
            //CommonHelper.SpinnerClose();
        }





    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />

    <asp:HiddenField ID="hfItemId" runat="server" Value="0" />


    <asp:HiddenField ID="hfRFQId" runat="server" Value="0"></asp:HiddenField>

    <asp:HiddenField ID="RandomProductId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="tempId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfGuestDeletedDoc" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfGuestDeletedDocForSupplier" runat="server" Value=""></asp:HiddenField>


    <div class="panel panel-default">
        <div class="panel-heading">
            Request For Quotation
        </div>
        <div class="panel-body">
            <div class="form-horizontal">


                <div id="initialPage">
                    <div class="form-group">

                        <div class="col-md-2 ">
                            <asp:Label ID="lblSore" runat="server" class="control-label required-field" Text="Store From"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlCostCentre" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>

                    </div>


                    <div class="form-group">
                        <div class="col-md-2 ">
                            <asp:Label ID="lblQuotationType" runat="server" class="control-label" Text="Quotation Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList runat="server" ID="ddlQuotationType" CssClass="form-control">
                                <asp:ListItem Text="Ad Hoc" Value="AdHoc"></asp:ListItem>
                                <asp:ListItem Text="Requisition Wise" Value="RequisitionWise"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div id="RequisitionOrderContainer" style="display: none">
                            <div class="col-md-2 ">
                                <asp:Label ID="lblRequisitionNumber" runat="server" class="control-label" Text="Requisition Number"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList runat="server" ID="ddlRequisitionNumber" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>


                    </div>
                    <div id="AdhoqRequest">
                        <div class="form-group">

                            <div class="col-md-2 ">
                                <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList runat="server" ID="ddlCategory" CssClass="form-control">
                                </asp:DropDownList>
                            </div>

                        </div>

                        <div class="form-group">
                            <div class="col-md-2 ">
                                <asp:Label ID="lblItemName" runat="server" class="control-label required-field" Text="Item Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtItem" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">

                            <div class="col-md-2 ">
                                <label class="control-label ">Current Stock</label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCurrentStock" runat="server" CssClass=" form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col-md-2 ">
                                <label class="control-label">Stock Unit</label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtStockUnit" runat="server" CssClass=" form-control" Enabled="false"></asp:TextBox>
                            </div>

                        </div>
                        <div class="form-group">

                            <div class="col-md-2 ">
                                <label class="control-label required-field">Quantity</label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtQuantity" runat="server" CssClass=" form-control quantitydecimal"></asp:TextBox>
                            </div>


                        </div>

                        <div id="SpecificationDiv" class="panel panel-default">
                            <div class="panel-heading">
                                Specification
                            </div>
                            <div class="panel-body">

                                <div>
                                    <div class="form-group">

                                        <div class="col-md-2 ">
                                            <label class="control-label required-field">Title</label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="txtTitle" runat="server" CssClass=" form-control"></asp:TextBox>
                                        </div>


                                    </div>

                                    <div class="form-group">

                                        <div class="col-md-2 ">
                                            <label class="control-label">Value</label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="txtValue" runat="server" CssClass=" form-control"></asp:TextBox>
                                        </div>


                                    </div>

                                    <div class="form-group" style="padding-top: 10px;">
                                        <div class="col-md-12 ">
                                            <input id="btnAdd" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" onclick="AddItem()" />
                                            <input id="btnCancelContact" type="button" value="Cancel" onclick="ClearSpecificationDiv()"
                                                class="TransactionalButton btn btn-primary btn-sm" />
                                        </div>
                                    </div>

                                    <div style="height: 250px; overflow-y: scroll;">
                                        <table id="SpecificationForRequestTbl" class="table table-bordered table-condensed table-hover">
                                            <thead>
                                                <tr>
                                                    <th style="width: 45%;">Title</th>
                                                    <th style="width: 45%;">Value</th>
                                                    <th style="width: 10%;">Action</th>
                                                </tr>
                                            </thead>
                                            <tbody></tbody>
                                        </table>

                                    </div>

                                </div>

                            </div>
                        </div>

                        <div id="specificationDialog" style="display: none">
                            <div style="padding: 10px 30px 10px 30px">
                                <div class="form-horizontal">
                                    <div class="form-group">

                                        <div class="col-md-2 ">
                                            <label class="control-label required-field">Title</label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="txtTitleInDialoug" runat="server" CssClass=" form-control"></asp:TextBox>
                                        </div>


                                    </div>

                                    <div class="form-group">

                                        <div class="col-md-2 ">
                                            <label class="control-label">Value</label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="txtValueInDialoug" runat="server" CssClass=" form-control"></asp:TextBox>
                                        </div>


                                    </div>

                                    <div class="form-group" style="padding-top: 10px;">
                                        <div class="col-md-12 ">
                                            <input id="btnAddInDialoug" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" onclick="AddItemInDialoug()" />
                                            <input id="btnCancelContactInDialoug" type="button" value="Cancel" onclick="ClearSpecificationDivInDialoug()"
                                                class="TransactionalButton btn btn-primary btn-sm" />
                                        </div>
                                    </div>

                                    <br />
                                    <div class="form-group">
                                        <div style="height: 250px; overflow-y: scroll">
                                            <table id="SpecificationForRequestTblInDialoug" class="table table-bordered table-condensed table-hover">
                                                <thead>
                                                    <tr>
                                                        <th style="width: 45%;">Title</th>
                                                        <th style="width: 45%;">Value</th>
                                                        <th style="width: 10%;">Action</th>
                                                    </tr>
                                                </thead>
                                                <tbody></tbody>
                                            </table>

                                        </div>
                                    </div>
                                    <div id="UpdatedOrAddedDiv">
                                        <input id="btnAddNewSpecification" type="button" value="Update Specifications" class="TransactionalButton btn btn-primary btn-sm" onclick="AddSpecificationsToItems()" />

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>




                    <div id="RequsitionWiseRequest" style="display: none">

                        <table id="ItemWithRequsitionForRequestTbl" class="table table-bordered table-condensed table-hover">
                            <thead>
                                <tr>
                                    <th style="width: 5%; text-align: center;">Select
                                            <input type="checkbox" value="" checked="checked" id="OrderCheck" onclick="CheckAllOrder()" />
                                    </th>
                                    <th style="width: 25%;">Item Name</th>
                                    <th style="width: 15%;">Current Stock</th>
                                    <th style="width: 15%;">Requisition Quantity</th>
                                    <th style="width: 20%;">Stock Unit</th>
                                    <th style="width: 20%;">Request Quantity</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>

                    </div>
                    <div class="form-group" style="padding-top: 10px;">
                        <div class="col-md-12 ">
                            <input id="btnAddNewItem" type="button" value="Add New Item" class="TransactionalButton btn btn-primary btn-sm" onclick="AddItemForQuotation()" />
                            <input id="btnCancelNewItem" type="button" value="Cancel" onclick="ClearAfterAdhoqQuotationItemAdded()"
                                class="TransactionalButton btn btn-primary btn-sm" />
                        </div>
                    </div>

                    <div style="height: 250px; overflow-y: scroll;">
                        <table id="ItemWithSpecificationForRequestTbl" class="table table-bordered table-condensed table-hover">
                            <thead>
                                <tr>
                                    <th style="width: 30%;">Item Name</th>
                                    <th style="width: 15%;">Current Stock</th>
                                    <th style="width: 15%;">Stock Unit</th>
                                    <th style="width: 30%;">Quantity</th>
                                    <th style="width: 10%;">Action</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>

                    </div>


                    <div class="form-group">
                        <div class="col-md-2 ">
                            <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Description"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
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
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnSave" runat="server" Text="Next" OnClientClick="javascript: return GoToTheSecondPage();"
                                CssClass="btn btn-primary btn-sm" TabIndex="12" />
                        </div>
                    </div>

                </div>
                <div id="SecondPage" style="display: none;">

                    <div class="form-group">

                        <div class="col-md-2 ">
                            <asp:Label ID="lblIndentName" runat="server" class="control-label required-field" Text="Indent Name"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtIndentName" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                    </div>

                    <div class="form-group">

                        <div class="col-md-2 ">
                            <asp:Label ID="lblPaymentTerm" runat="server" class="control-label" Text="Payment Term"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList runat="server" ID="ddlPaymentTerm" CssClass="form-control">

                                <asp:ListItem Text="Cash" Value="Cash"></asp:ListItem>
                                <asp:ListItem Text="Credit" Value="Credit"></asp:ListItem>

                            </asp:DropDownList>
                        </div>

                        <div id="CreditDaysDiv" style="display: none;">

                            <div class="col-md-2 ">
                                <asp:Label ID="lblDays" runat="server" class="control-label required-field" Text="Days (Credit)"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDays" runat="server" CssClass="form-control quantity"></asp:TextBox>
                            </div>

                        </div>

                    </div>

                    <div class="form-group">

                        <div class="col-md-2 ">
                            <asp:Label ID="lblDeliveryTerms" runat="server" class="control-label" Text="Delivery Terms"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList runat="server" ID="ddlDeliveryTerms" CssClass="form-control">

                                <asp:ListItem Text="On Site" Value="OnSite"></asp:ListItem>
                                <asp:ListItem Text="Works" Value="Works"></asp:ListItem>

                            </asp:DropDownList>
                        </div>


                    </div>
                    <div id="siteAddressDiv" style="display: none">

                        <div class="form-group">
                            <div class="col-md-2 ">
                                <asp:Label ID="lblSiteAddress" runat="server" class="control-label required-field" Text="Site Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSiteAddress" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="11"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="form-group ">
                        <div class="col-md-2">
                            <label class="control-label required-field">Expiry Date</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                        <div class="col-md-2 ">
                            <label class="control-label ">Expiry Time</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtExpiryTime" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group">

                        <div class="col-md-2 ">
                            <label class="control-label">VAT</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtVAT" runat="server" onblur='CheckPercentage()' CssClass=" form-control quantitydecimal"></asp:TextBox>
                        </div>

                        <div class="col-md-2 ">
                            <label class="control-label">AIT</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtAIT" runat="server" onblur='CheckPercentage()' CssClass=" form-control quantitydecimal"></asp:TextBox>
                        </div>


                    </div>

                    <div class="form-group">
                        <div class="col-md-2 ">
                            <asp:Label ID="lblIndentPurpose" runat="server" class="control-label" Text="Indent Purpose"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtIndentPurpose" runat="server" CssClass="form-control" TextMode="MultiLine"
                                TabIndex="11"></asp:TextBox>
                        </div>
                    </div>

                    <table id="SupplierInformationTbl" class="table table-bordered table-condensed table-hover">
                        <thead>
                            <tr>
                                <th style="width: 10%; text-align: center;">
                                    <input type="checkbox" value="" checked="checked" id="OrderCheckAll" onclick="CheckAllOrderAll()" />
                                </th>
                                <th style="width: 90%;">Supplier Information</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>

                    <div class="form-group">
                        <div class="col-md-2 ">
                            <label class="control-label">Attachment</label>
                        </div>
                        <%--<div class="col-md-10">
                            <input type="button" id="btnAttachmentForSupplier" class="TransactionalButton btn btn-primary btn-sm" value="Documents..." onclick="AttachFileForSupplier()" />
                        </div>--%>
                        <div class="col-md-10">
                            <input type="button" id="btnAttachmentForSupplier" class="TransactionalButton btn btn-primary btn-sm" value="Attach" onclick="AttachFileForSupplier()" />
                        </div>
                    </div>

                    <div class="form-group">
                        <div id="documentsDivForSupplier" style="display: none;">
                            <label for="Attachment" class="control-label col-md-2">
                                Attachment</label>
                            <div class="col-md-4">
                                <asp:Panel ID="Panel1" runat="server" Style="text-align: center;">
                                    <cc2:ClientUploader ID="flashUpload2" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadCompleteForSupplier()"
                                        FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                    <div id="DocumentInfoForSupplier">
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnGoToTheThirdPage" runat="server" Text="Next" OnClientClick="javascript: return GoToTheThirdPage();"
                                CssClass="btn btn-primary btn-sm" TabIndex="12" />
                        </div>
                    </div>


                </div>

                <div id="ThirdPage" style="display: none;">
                </div>
            </div>
        </div>


    </div>

</asp:Content>
