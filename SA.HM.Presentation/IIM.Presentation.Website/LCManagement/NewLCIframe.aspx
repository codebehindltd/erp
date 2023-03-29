<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="NewLCIframe.aspx.cs" Inherits="HotelManagement.Presentation.Website.LCManagement.NewLCIframe" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var PODInformationTable;
        var Currency = new Array();
        var LCInformationDetails = new Array();
        var LCInformationDetailsDeleted = new Array();
        var LCPaymentDetails = new Array();
        var LCPaymentDetailsDeleted = new Array();

        var GlobalApproveStatus = "";
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;
        var isClose;


        $(document).ready(function () {

            var LCInformationId = $.trim(CommonHelper.GetParameterByName("lci"));
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;

            $("#ContentPlaceHolder1_companyProjectUserControl_hfDropdownFirstValue").val("select");


            if ($("#ContentPlaceHolder1_hfCurrencyAll").val() != "") {
                Currency = JSON.parse($("#ContentPlaceHolder1_hfCurrencyAll").val());
                $("#ContentPlaceHolder1_hfCurrencyAll").val("");

                var corncy = _.findWhere(Currency, { CurrencyId: parseInt($("#ContentPlaceHolder1_ddlCurrency").val()) });

                if (corncy != null) {

                    if (corncy.CurrencyType == "Local") {
                        $("#ContentPlaceHolder1_txtConversionRate").val("");
                        $("#convertionRateContainer").hide();
                    }
                    else {
                        $("#convertionRateContainer").show();
                    }
                }
            }

            $("#ContentPlaceHolder1_ddlLCManageAccount").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlSupplier").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlPODInformation").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlAccountHead").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlSupplier").change(function () {

                var SupplierId = parseInt($("#ContentPlaceHolder1_ddlSupplier").val().trim());

                //PageMethods.LoadPO(SupplierId, OnLoadPOSucceeded, OnLoadPOFailed);
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "./NewLCIframe.aspx/LoadPO",
                    data: "{'supplierId':'" + SupplierId + "'}",
                    dataType: "json",
                    async: false,
                    success: OnLoadPOSucceeded,
                    error: function (result) {
                        //alert("Error");
                    }
                });
                return false;

            });
            $("#ContentPlaceHolder1_ddlPODInformation").change(function () {
                //debugger;
                PODInformationTable.clear();
                PODInformationTable.draw();
                $("#ContentPlaceHolder1_txtTotalPurchaseAmount").val("");
                var poOrderId = parseInt($("#ContentPlaceHolder1_ddlPODInformation").val().trim());
                PurchaseOrderDetails(poOrderId);
                return false;

            });

            var single = $("#ContentPlaceHolder1_hfIsSingle").val();
            if (single == "1") {
                $('#CompanyProjectPanel').hide();
                //$('#SearchTypePanel').show();
            }
            else {
                $('#CompanyProjectPanel').show();
                //$('#SearchTypePanel').hide();
            }

            $("#ContentPlaceHolder1_ddlCurrency").change(function () {

                var corncy = _.findWhere(Currency, { CurrencyId: parseInt($(this).val()) });

                if (corncy != null) {

                    if (corncy.CurrencyType == "Local") {
                        $("#ContentPlaceHolder1_txtConversionRate").val("");
                        $("#convertionRateContainer").hide();
                    }
                    else {
                        $("#convertionRateContainer").show();
                        PageMethods.LoadCurrencyConversionRate(corncy.CurrencyId, OnLoadConversionRateSucceeded, OnLoadConversionRateFailed);
                    }
                }
            });

            PODInformationTable = $("#tblPODInformation").DataTable({
                data: [],
                columns: [
                    { title: "", "data": "LCDetailId", visible: false },
                    { title: "", "data": "LCId", visible: false },
                    { title: "", "data": "POrderId", visible: false },
                    { title: "", "data": "CostCenterId", visible: false },
                    { title: "", "data": "StockById", visible: false },

                    { title: "", "data": "ProductId", visible: false },
                    { title: "Item Name", "data": "ItemName", sWidth: '30%' },
                    { title: "Unit Price", "data": "PurchasePrice", sWidth: '15%' },
                    { title: "Unit", "data": "Quantity", sWidth: '15%' },
                    { title: "Unit Head", "data": "StockBy", sWidth: '15%' },
                    { title: "Total", "data": null, sWidth: '25%' }
                ],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var row = '';
                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }

                    $('td:eq(' + (nRow.children.length - 1) + ')', nRow).html((aData.Quantity * aData.PurchasePrice).toFixed(5));

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


            CommonHelper.ApplyDecimalValidation();
            CommonHelper.ApplyDecimalValidationWithFivePrecision();
            CommonHelper.ApplyIntigerValidation();
            $('#ContentPlaceHolder1_txtLCOpenDate').datepicker({
                changeMonth: true,
                changeYear: true,
                // defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtLCMatureDate').datepicker("option", "minDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtLCMatureDate').datepicker({
                changeMonth: true,
                changeYear: true,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtLCOpenDate').datepicker("option", "maxDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtPaymentDate').datepicker({
                changeMonth: true,
                changeYear: true,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);

            $("#ddlLCManageAccount").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ddlSupplier").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });


            if (LCInformationId != "") {
                PerformEdit(LCInformationId);
            }
            else {
                Clear();
            }
        });

        function PerformEdit(id) {
            //debugger;
            PageMethods.GetLcById(id, OnSuccessLoading, OnFailLoading)
            return false;
        }

        function OnSuccessLoading(result) {
            FillForm(result);
            return false;
        }
        function FillForm(Result) {
            //debugger;
            Clear();

            GlobalApproveStatus = Result.LCInformation.ApprovedStatus;
            //debugger;
            $("#ContentPlaceHolder1_hfLCInformationId").val(Result.LCInformation.LCId);


            $("#ContentPlaceHolder1_txtLCNumber").val(Result.LCInformation.LCNumber);
            $("#ContentPlaceHolder1_txtPINumber").val(Result.LCInformation.PINumber);
            $("#ContentPlaceHolder1_txtLCOpenDate").val(CommonHelper.DateFromDateTimeToDisplay(Result.LCInformation.LCOpenDate, innBoarDateFormat));

            $("#ContentPlaceHolder1_txtLCMatureDate").val(Result.LCInformation.LCMatureDate == null ? "" : CommonHelper.DateFromDateTimeToDisplay(Result.LCInformation.LCMatureDate, innBoarDateFormat));
            $("#ContentPlaceHolder1_txtLCValue").val(Result.LCInformation.LCValue);

            $("#ContentPlaceHolder1_ddlLCTypes").val(Result.LCInformation.LCTypes).trigger('change');
            $("#ContentPlaceHolder1_ddlIncoterms").val(Result.LCInformation.Incoterms).trigger('change');
            $("#ContentPlaceHolder1_ddlLCManageAccount").val(Result.LCInformation.LCManageAccountId).trigger('change');
            $("#ContentPlaceHolder1_ddlSupplier").val(Result.LCInformation.SupplierId).trigger('change');
            //debugger;
            $("#ContentPlaceHolder1_ddlPODInformation").val(Result.LCInformation.POorderId).trigger('change');
            $('#ContentPlaceHolder1_ddlSupplier').attr('disabled', 'disabled');
            $('#ContentPlaceHolder1_ddlPODInformation').attr('disabled', 'disabled');

            PODInformationTable.clear();
            PODInformationTable.rows.add(Result.LCInformationDetail);
            PODInformationTable.draw();
            LCInformationDetails = new Array();
            var total = 0.0;
            for (var i = 0; i < Result.LCInformationDetail.length; i++) {
                total += parseFloat(Result.LCInformationDetail[i].Quantity) * parseFloat(Result.LCInformationDetail[i].PurchasePrice);
                LCInformationDetails.push({
                    LCDetailId: parseInt(Result.LCInformationDetail[i].LCDetailId, 10),
                    LCId: Result.LCInformationDetail[i].LCId,
                    POrderId: Result.LCInformationDetail[i].POrderId,
                    CostCenterId: Result.LCInformationDetail[i].CostCenterId,
                    StockBy: Result.LCInformationDetail[i].StockBy,
                    StockById: Result.LCInformationDetail[i].StockById,
                    ProductId: Result.LCInformationDetail[i].ProductId,
                    PurchasePrice: Result.LCInformationDetail[i].PurchasePrice,
                    Quantity: Result.LCInformationDetail[i].Quantity
                });
            }
            $("#ContentPlaceHolder1_txtTotalPurchaseAmount").val(Math.ceil(total));


            var tr = "";
            for (var i = 0; i < Result.LCPayment.length; i++) {
                tr += "<tr>";

                tr += "<td style='width:15%;'>" + Result.LCPayment[i].AccountHeadName + "</td>";

                tr += "<td style='width:15%;'>" +
                    "<input type='text' value='" + Result.LCPayment[i].Amount + "'  class='form-control quantitydecimalWithFivePrecision' onblur='CalculateTotalForAdhoq(this)'  />" +
                    "</td>";
                tr += "<td style='width:10%;'>" + CommonHelper.DateFormatDDMMYYY(Result.LCPayment[i].PaymentDate) +
                    "</td>";

                tr += "<td style='width:10%;'>" + Result.LCPayment[i].CurrencyName + "</td>";
                tr += "<td style='width:10%;'>" +
                    "<input type='text' value='" + Result.LCPayment[i].ConvertionRate + "'  class='form-control quantitydecimalWithFivePrecision' onblur='CalculateTotalForAdhoq(this)'  />" +
                    "</td>";
                tr += "<td style='width:20%;'>" + (parseFloat(Result.LCPayment[i].Amount) * parseFloat(Result.LCPayment[i].ConvertionRate)).toFixed(5) + "</td>";

                tr += "<td style='width:15%;'>" +
                    "<input type='text' value='" + Result.LCPayment[i].Remarks + "'  class='form-control' onblur='CalculateTotalForAdhoq(this)'  />" +
                    "</td>";


                tr += "<td style='width:5%;'>";
                tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                tr += "</td>";
                tr += "<td style='display:none;'>" + Result.LCPayment[i].AccountHeadId + "</td>";
                tr += "<td style='display:none;'>" + Result.LCPayment[i].PaymentId + "</td>";

                tr += "</tr>";

                $("#tblPDInformation tbody").prepend(tr);
                tr = "";

                LCPaymentDetails.push({
                    PaymentId: parseInt(Result.LCPayment[i].PaymentId, 10),
                    LCId: Result.LCInformation.LCId,
                    AccountHeadId: parseInt(Result.LCPayment[i].AccountHeadId, 10),
                    AccountHeadName: Result.LCPayment[i].AccountHeadName,
                    CurrencyId: Result.LCPayment[i].CurrencyId,
                    ConvertionRate: Result.LCPayment[i].ConvertionRate,
                    Amount: Result.LCPayment[i].Amount,
                    Remarks: Result.LCPayment[i].Remarks,
                    PaymentDate: Result.LCPayment[i].PaymentDate
                });


                var PaymentAmount = $("#ContentPlaceHolder1_txtTotalPaymentAmount").val() == "" ? 0.0 : $("#ContentPlaceHolder1_txtTotalPaymentAmount").val();
                var totalPaymentAmount = parseFloat(PaymentAmount) + (parseFloat(Result.LCPayment[i].Amount) * parseFloat(Result.LCPayment[i].ConvertionRate));
                $("#ContentPlaceHolder1_txtTotalPaymentAmount").val(Math.ceil(totalPaymentAmount.toFixed(5)));
            }
            CommonHelper.ApplyDecimalValidationWithFivePrecision();


            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val(Result.LCInformation.CompanyId).trigger('change');
            $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val(Result.LCInformation.ProjectId).trigger('change');

            $('#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany').attr('disabled', 'disabled');
            $('#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject').attr('disabled', 'disabled');



            if (IsCanEdit)
                $("#btnSave").val('Update').show();
            else
                $("#btnSave").hide();
            $("#btnClear").hide();
            UploadComplete();

            return false;
        }

        function LoadDocUploader() {

            var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var path = "/LCManagement/Images/";
            var category = "LCDoc";
            var iframeid = 'frmPrint';
            //var url = "/HMCommon/FileUploadTest.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            var url = "/Common/FileUpload.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            document.getElementById(iframeid).src = url;

            $("#DocumentDialouge").dialog({
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

        function CloseDialog() {
            $("#DocumentDialouge").dialog('close');
            return false;
        }
        function UploadComplete() {
            var randomId = $("#ContentPlaceHolder1_RandomProductId").val();
            ShowUploadedDocument(randomId);
        }

        function ShowUploadedDocument(randomId) {
            var id = $("#ContentPlaceHolder1_hfLCInformationId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();
            PageMethods.GetUploadedDocByWebMethod(randomId, id, deletedDoc, OnGetUploadedDocByWebMethodSucceeded, OnGetUploadedDocByWebMethodFailed);
            return false;
        }

        function OnGetUploadedDocByWebMethodSucceeded(result) {
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            DocTable = "";

            DocTable += "<table id='DocTableList' style='width:100%' class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            DocTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    DocTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    DocTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }
                DocTable += "<td align='left' style='width: 50%;cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + "','" + result[row].Name + "');\">" + result[row].Name + "</td>";

                if (result[row].Path != "") {
                    imagePath = "<img src='" + result[row].Path + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                DocTable += "<td align='left' style='width: 30%'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + "','" + result[row].Name + "');\">" + imagePath + "</td>";

                DocTable += "<td align='left' style='width: 20%'>";
                DocTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteDoc('" + result[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                DocTable += "</td>";
                DocTable += "</tr>";
            }
            DocTable += "</table>";

            docc = DocTable;

            $("#DocumentInfo").html(DocTable);

            return false;
        }
        function DeleteDoc(docId, rowIndex) {
            var deletedDoc = $("#<%=hfGuestDeletedDoc.ClientID %>").val();

            if (deletedDoc != "")
                deletedDoc += "," + docId;
            else
                deletedDoc = docId;

            $("#<%=hfGuestDeletedDoc.ClientID %>").val(deletedDoc);

            $("#trdoc" + rowIndex).remove();
        }
        function OnGetUploadedDocByWebMethodFailed(error) {
            alert(error.get_message());
        }
        function ShowDocument(path, name) {
            var iframeid = 'fileIframe';
            document.getElementById(iframeid).src = path;
            $("#ShowDocumentDiv").dialog({
                autoOpen: true,
                modal: true,
                width: "82%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Document - " + name,
                show: 'slide'
            });
            return false;
        }

        function Clear() {

            //$("#ContentPlaceHolder1_ddlGLCompany").val("0").trigger('change');
            //$("#ContentPlaceHolder1_ddlGLProject").val("0").trigger('change');

            $("#ContentPlaceHolder1_txtLCNumber").val('');
            $("#ContentPlaceHolder1_txtPINumber").val('');

            $("#ContentPlaceHolder1_txtLCOpenDate").val(DayOpenDate);
            $("#ContentPlaceHolder1_txtLCMatureDate").val(DayOpenDate);

            $("#ContentPlaceHolder1_txtLCValue").val('');

            $("#ContentPlaceHolder1_ddlLCTypes").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlIncoterms").val("0").trigger('change');

            $("#ContentPlaceHolder1_ddlLCManageAccount").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlSupplier").val("0").trigger('change');

            //var Table = document.getElementById("tblPDInformation");
            //Table.innerHTML = "";

            Cancel();

            if (IsCanSave) {
                $("#btnSave").val('Save').show();
            }
            else {
                $("#btnSave").hide();
            }
            isClose = false;
            $("#btnClear").show();
            return false;
        }

        function OnLoadPOSucceeded(response) {
            //debugger;
            var result = response.d;
            var typesList = result;
            $("#ContentPlaceHolder1_ddlPODInformation").empty();
            PODInformationTable.clear();
            PODInformationTable.draw();
            $("#ContentPlaceHolder1_txtTotalPurchaseAmount").val("");

            var i = 0, fieldLength = result.length;
            if (fieldLength > 0) {
                typesList = result;
                $('<option value="' + 0 + '">' + '--- Please Select ---' + '</option>').appendTo('#ContentPlaceHolder1_ddlPODInformation');
                for (i = 0; i < fieldLength; i++) {
                    $('<option value="' + result[i].POrderId + '">' + result[i].PONumber + '</option>').appendTo('#ContentPlaceHolder1_ddlPODInformation');
                }
                //$("#ContentPlaceHolder1_ddlGLProject").attr("Enabled", true);
            }
            else {
                $("<option value='0'>--No Purchase Order Found--</option>").appendTo("#ContentPlaceHolder1_ddlPODInformation");

                //$("#ContentPlaceHolder1_ddlGLProject").attr("Enabled", false);


            }

            return false;
        }


        function PurchaseOrderDetails(POrderId) {
            //debugger;
            //PageMethods.LoadPurchaseOrderDetails(POrderId, OnLoadPurchaseOrderDetailsSucceed, OnLoadPurchaseOrderDetailsFailed);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "./NewLCIframe.aspx/LoadPurchaseOrderDetails",
                data: "{'pOrderId':'" + POrderId + "'}",
                dataType: "json",
                async: false,
                success: OnLoadPurchaseOrderDetailsSucceed,
                error: function (result) {
                    //alert("Error");
                }
            });
            return false;
        }
        function OnLoadPurchaseOrderDetailsSucceed(response) {
            //debugger;
            var result = response.d;
            PODInformationTable.clear();
            PODInformationTable.rows.add(result);
            PODInformationTable.draw();
            var total = 0.0;
            for (var i = 0; i < result.length; i++) {
                total += parseFloat(result[i].Quantity) * parseFloat(result[i].PurchasePrice);
                LCInformationDetails.push({
                    LCDetailId: parseInt(result[i].LCDetailId, 10),
                    LCId: result[i].LCId,
                    POrderId: result[i].POrderId,
                    CostCenterId: result[i].CostCenterId,
                    StockBy: result[i].StockBy,
                    StockById: result[i].StockById,
                    ProductId: result[i].ProductId,
                    PurchasePrice: result[i].PurchasePrice,
                    Quantity: result[i].Quantity
                });
            }
            $("#ContentPlaceHolder1_txtTotalPurchaseAmount").val(Math.ceil(total));


            //debugger;
        }
        function OnLoadPurchaseOrderDetailsFailed() {
            //toastr.info("No Convertion Rate Is Found.");
        }
        function OnFailLoading(error) {
            //debugger;
            alert(error.get_message());
            //toastr.info("No Convertion Rate Is Found.");
        }

        function AddPaymentDetail() {
            //var id = +$("#ContentPlaceHolder1_hfCashRequsitionId").val();
            //debugger;
            var accountId = $("#ContentPlaceHolder1_ddlAccountHead option:selected").val();

            //var PaymentOBJ = _.findWhere(LCPaymentDetails, { AccountHeadId: parseInt(accountId, 10) });
            //if (PaymentOBJ != null) {
            //    toastr.warning("It's already added.");
            //    $("#ContentPlaceHolder1_ddlAccountHead").focus();
            //    return false;
            //}

            var accountName = $("#ContentPlaceHolder1_ddlAccountHead option:selected").text();
            if (accountId == "0") {
                isClose = false;
                toastr.warning("Select a Account Head.");
                $("#ContentPlaceHolder1_ddlAccountHead").focus();
                return false;
            }
            var amount = $("#ContentPlaceHolder1_txtAmount").val();
            if (amount == "") {
                isClose = false;
                toastr.warning("Add amount.");
                $("#ContentPlaceHolder1_txtAmount").focus();
                return false;
            }

            var PaymentDate = $("#ContentPlaceHolder1_txtPaymentDate").val();
            if (PaymentDate == "") {
                isClose = false;
                toastr.warning("Add Payment Date.");
                $("#ContentPlaceHolder1_txtPaymentDate").focus();
                return false;
            }

            var currency = $("#ContentPlaceHolder1_ddlCurrency option:selected").val();
            var currencyName = $("#ContentPlaceHolder1_ddlCurrency option:selected").text();
            var convertionRate = $("#ContentPlaceHolder1_txtConversionRate").val() == "" ? 1 : $("#ContentPlaceHolder1_txtConversionRate").val();

            var PaymentAmount = $("#ContentPlaceHolder1_txtTotalPaymentAmount").val() == "" ? 0.0 : $("#ContentPlaceHolder1_txtTotalPaymentAmount").val();
            var totalPaymentAmount = parseFloat(PaymentAmount) + (parseFloat(amount) * parseFloat(convertionRate));
            var purchaseAmount = $("#ContentPlaceHolder1_txtTotalPurchaseAmount").val() == "" ? 0.0 : parseFloat($("#ContentPlaceHolder1_txtTotalPurchaseAmount").val());
            //if (totalPaymentAmount > purchaseAmount) {
            //    isClose = false;
            //    toastr.warning("Can not add greater than purchase amount.");
            //    $("#ContentPlaceHolder1_txtAmount").focus();
            //    return false;
            //}

            var remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            //if (remarks == "") {
            //    isClose = false;
            //    toastr.warning("Enter Remarks");
            //    $("#ContentPlaceHolder1_txtRemarks").focus();
            //    return false;
            //}


            var tr = "";
            tr += "<tr>";

            tr += "<td style='width:15%;'>" + accountName + "</td>";

            tr += "<td style='width:15%;'>" +
                "<input type='text' value='" + amount + "'  class='form-control quantitydecimalWithFivePrecision' onblur='CalculateTotalForAdhoq(this)'  />" +
                "</td>";
            tr += "<td style='width:10%;'>" + PaymentDate +
                "</td>";

            tr += "<td style='width:10%;'>" + currencyName + "</td>";
            tr += "<td style='width:10%;'>" +
                "<input type='text' value='" + convertionRate + "'  class='form-control quantitydecimalWithFivePrecision' onblur='CalculateTotalForAdhoq(this)'  />" +
                "</td>";
            tr += "<td style='width:20%;'>" + (parseFloat(amount) * parseFloat(convertionRate)) + "</td>";

            tr += "<td style='width:15%;'>" +
                "<input type='text' value='" + remarks + "'  class='form-control' onblur='CalculateTotalForAdhoq(this)'  />" +
                "</td>";


            tr += "<td style='width:5%;'>";
            tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
            tr += "</td>";
            tr += "<td style='display:none;'>" + accountId + "</td>";
            tr += "<td style='display:none;'>" + 0 + "</td>";

            tr += "</tr>";

            $("#tblPDInformation tbody").prepend(tr);
            tr = "";

            LCPaymentDetails.push({
                PaymentId: parseInt(0, 10),
                LCId: 0,
                AccountHeadId: parseInt(accountId, 10),
                AccountHeadName: accountName,
                CurrencyId: currency,
                ConvertionRate: convertionRate,
                Amount: amount,
                Remarks: remarks,
                PaymentDate: CommonHelper.DateFormatToMMDDYYYY(PaymentDate, '/')
            });

            var PaymentAmount = $("#ContentPlaceHolder1_txtTotalPaymentAmount").val() == "" ? 0.0 : $("#ContentPlaceHolder1_txtTotalPaymentAmount").val();
            var totalPaymentAmount = parseFloat(PaymentAmount) + (parseFloat(amount) * parseFloat(convertionRate));
            $("#ContentPlaceHolder1_txtTotalPaymentAmount").val(Math.ceil(totalPaymentAmount));

            CommonHelper.ApplyDecimalValidation();
            CommonHelper.ApplyDecimalValidationWithFivePrecision();
            Cancel();
            $("#ContentPlaceHolder1_ddlAccountHead").focus();
            return false;
        }
        function CalculateTotalForAdhoq(control) {

            //debugger;
            var tr = $(control).parent().parent();

            var Amount = $.trim($(tr).find("td:eq(1)").find("input").val());
            var ConversionRate = $.trim($(tr).find("td:eq(4)").find("input").val());
            var remarks = $.trim($(tr).find("td:eq(6)").find("input").val());
            // debugger;

            if (Amount == "" || Amount == "0") {
                toastr.info("Amount Cannot Be Zero Or Empty.");
                return false;
            }
            if (ConversionRate == "" || ConversionRate == "0") {
                toastr.info("Conversion Rate Cannot Be Zero Or Empty.");
                return false;
            }
            //if (remarks == "") {
            //    toastr.info("Remarks Cannot Be Empty.");
            //    return false;
            //}

            ($(tr).find("td:eq(5)").text((parseFloat(Amount) * parseFloat(ConversionRate)).toFixed(5)));

            //var costCenterId = parseInt($.trim($(tr).find("td:eq(4)").text()), 10);
            var accountId = parseInt($.trim($(tr).find("td:eq(8)").text()), 10);
            var detailsId = parseInt($.trim($(tr).find("td:eq(9)").text()), 10);

            var payment = _.findWhere(LCPaymentDetails, { AccountHeadId: accountId });
            var index = _.indexOf(LCPaymentDetails, payment);

            LCPaymentDetails[index].Amount = parseFloat(Amount);
            LCPaymentDetails[index].ConvertionRate = parseFloat(ConversionRate);
            LCPaymentDetails[index].Remarks = (remarks);

            var total = 0.0;
            for (var i = 0; i < LCPaymentDetails.length; i++) {
                total += (parseFloat(LCPaymentDetails[i].Amount) * parseFloat(LCPaymentDetails[i].ConvertionRate));
            }
            $("#ContentPlaceHolder1_txtTotalPaymentAmount").val(Math.ceil(total.toFixed(5)));


        }

        function DeleteAdhoqItem(control) {

            if (!confirm("Do you want to delete?")) { return false; }

            //debugger;

            var tr = $(control).parent().parent();

            var accountId = parseInt($.trim($(tr).find("td:eq(8)").text()), 10);
            var detailsId = parseInt($.trim($(tr).find("td:eq(9)").text()), 10);

            var payment = _.findWhere(LCPaymentDetails, { AccountHeadId: accountId });
            var index = _.indexOf(LCPaymentDetails, payment);

            if (parseInt(detailsId, 10) > 0)
                LCPaymentDetailsDeleted.push(JSON.parse(JSON.stringify(payment)));

            LCPaymentDetails.splice(index, 1);
            $(tr).remove();
            var total = 0.0;
            for (var i = 0; i < LCPaymentDetails.length; i++) {
                total += (parseFloat(LCPaymentDetails[i].Amount) * parseFloat(LCPaymentDetails[i].ConvertionRate));
            }
            $("#ContentPlaceHolder1_txtTotalPaymentAmount").val(Math.ceil(total));

        }

        function SaveNClose() {
            //debugger;
            isClose = true;
            //SaveOrUpdateTask();
            $.when(SaveOrUpdateLCInformation()).done(function () {
                if (isClose) {

                    if (typeof parent.CloseDialog === "function") {
                        parent.CloseDialog();
                    }

                }
            });
            return false;
        }

        function SaveOrUpdateLCInformation() {
            debugger;
            var id = +$("#ContentPlaceHolder1_hfLCInformationId").val();

            var LcNumber = $("#ContentPlaceHolder1_txtLCNumber").val();
            if (LcNumber == "") {
                var isLCNumberAutoGenerate = $("#<%=hfIsLCNumberAutoGenerate.ClientID %>").val();
                if (isLCNumberAutoGenerate == 0) {
                    isClose = false;
                    toastr.warning("Add LC Number.");
                    $("#ContentPlaceHolder1_txtLCNumber").focus();
                    return false;
                }
            }
            var PINumber = $("#ContentPlaceHolder1_txtPINumber").val();
            var LCOpenDate = $("#ContentPlaceHolder1_txtLCOpenDate").val();
            if (LCOpenDate == "") {
                isClose = false;
                toastr.warning("Add LC Open Date.");
                $("#ContentPlaceHolder1_txtLCOpenDate").focus();
                return false;
            }
            var LCMatureDate = $("#ContentPlaceHolder1_txtLCMatureDate").val();

            var LCValue = $("#ContentPlaceHolder1_txtLCValue").val();


            var LCTypesId = $("#ContentPlaceHolder1_ddlLCTypes").val();
            if (LCTypesId == "0") {
                isClose = false;
                toastr.warning("Select a LC Types.");
                $("#ContentPlaceHolder1_ddlLCTypes").focus();
                return false;
            }
            var IncotermsId = $("#ContentPlaceHolder1_ddlIncoterms").val();
            if (IncotermsId == "0") {
                isClose = false;
                toastr.warning("Select a Incoterms.");
                $("#ContentPlaceHolder1_ddlIncoterms").focus();
                return false;
            }

            var LCManageAccountId = $("#ContentPlaceHolder1_ddlLCManageAccount").val();
            if (LCManageAccountId == "0") {
                isClose = false;
                toastr.warning("Select a LC Manage Account.");
                $("#ContentPlaceHolder1_ddlLCManageAccount").focus();
                return false;
            }
            var SupplierId = $("#ContentPlaceHolder1_ddlSupplier").val();
            if (SupplierId == "0") {
                isClose = false;
                toastr.warning("Select a Supplier.");
                $("#ContentPlaceHolder1_ddlSupplier").focus();
                return false;
            }


            var companyId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val();
            if (companyId == "0") {
                isClose = false;
                toastr.warning("Select a company.");
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").focus();
                return false;
            }
            var projectId = $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").val();
            if (projectId == "0") {
                isClose = false;
                toastr.warning("Select a project.");
                $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLProject").focus();
                return false;
            }
            var PODId = $("#ContentPlaceHolder1_ddlPODInformation").val();
            if (PODId == "0") {
                isClose = false;
                toastr.warning("Select a Purchase Order.");
                $("#ContentPlaceHolder1_ddlPODInformation").focus();
                return false;
            }

            var LCInformation = {
                LCId: parseInt(id, 10),
                LCNumber: LcNumber,
                PINumber: PINumber,
                LCOpenDate: CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(LCOpenDate, innBoarDateFormat),
                LCMatureDate: LCMatureDate == "" ? null : CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(LCMatureDate, innBoarDateFormat),
                LCValue: LCValue,
                LCTypes: LCTypesId,
                Incoterms: IncotermsId,
                LCManageAccountId: LCManageAccountId,
                SupplierId: SupplierId,
                CompanyId: companyId,
                ProjectId: projectId,
                POorderId: PODId,
                ApprovedStatus: (GlobalApproveStatus == "" ? "Pending" : GlobalApproveStatus)
            }
            GlobalApproveStatus = "";


            var totalAmount = 0.0;
            var LCPaymentDetailsNewlyAdded = new Array();
            LCPaymentDetailsNewlyAdded = LCPaymentDetails;
            var row = 0, rowCount = LCPaymentDetailsNewlyAdded.length;
            for (row = 0; row < rowCount; row++) {
                if (LCPaymentDetailsNewlyAdded[row].Amount == "" || LCPaymentDetailsNewlyAdded[row].Amount == "0") {
                    toastr.warning("Payment Amount Cannot NUll OR Empty In " + LCPaymentDetailsNewlyAdded[row].AccountHeadName);
                    break;
                }
            }
            if (row == 0) {
                isClose = false;
                toastr.warning("Add some data for Payment");
                return false;
            }

            if (row != rowCount) {
                return false;
            }
            var LCInformationViewBOForAdded = {
                LCInformation: LCInformation,
                LCInformationDetail: LCInformationDetails,
                LCPayment: LCPaymentDetails
            }
            var LCInformationViewBOForDeleted = {
                LCInformationDetail: LCInformationDetailsDeleted,
                LCPayment: LCPaymentDetailsDeleted
            }
            var hfRandom = $("#<%=RandomProductId.ClientID %>").val();
            var deletedDocuments = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();


            PageMethods.SaveOrUpdateLCInformation(LCInformationViewBOForAdded, LCInformationViewBOForDeleted, hfRandom, deletedDocuments, OnSuccessSaveOrUpdate, OnFailSaveOrUpdate);
            return false;



        }
        function OnSuccessSaveOrUpdate(result) {
            //debugger;
            if (result.IsSuccess) {
                if (result.IsSuccess) {
                    parent.ShowAlert(result.AlertMessage);
                    parent.SearchInformation(1, 1);
                }
                //if (typeof parent.GridPaging === "function")
                //    parent.GridPaging(1, 1);
                Clear();
            }
        }

        function OnFailSaveOrUpdate(error) {
            isClose = false;
            toastr.error(error.get_message());
            return false;
        }

        function Cancel() {
            $("#ContentPlaceHolder1_ddlAccountHead").val("0").change();
            $("#ContentPlaceHolder1_txtAmount").val("");
            $("#ContentPlaceHolder1_txtPaymentDate").val(DayOpenDate);
            $("#ContentPlaceHolder1_ddlCurrency").val("1").change();
            $("#ContentPlaceHolder1_txtRemarks").val("");

        }

        function OnLoadConversionRateSucceeded(result) {
            if (result != null)
                $("#ContentPlaceHolder1_txtConversionRate").val(result.ConversionRate);
            else {
                toastr.info("No Convertion Rate Is Found.");
            }
        }
        function OnLoadConversionRateFailed() {
            toastr.info("No Convertion Rate Is Found.");
        }

    </script>
    <asp:HiddenField ID="hfIsSingle" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfCurrencyAll" runat="server" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfLCInformationId" Value="0" runat="server" />
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="RandomProductId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="tempId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfGuestDeletedDoc" runat="server" Value=""></asp:HiddenField>
    <asp:HiddenField ID="hfIsLCNumberAutoGenerate" runat="server" />
    <div>
        <div id="ShowDocumentDiv" style="display: none;">
            <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
                clientidmode="static" scrolling="yes"></iframe>
        </div>
        <div style="padding: 10px 30px 10px 30px">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2" id="LCNumberLabel" runat="server">
                        <label class="control-label required-field">LC Number</label>
                    </div>
                    <div class="col-md-4" id="LCNumberControl" runat="server">
                        <asp:TextBox ID="txtLCNumber" runat="server" CssClass=" form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">PI Number</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPINumber" runat="server" CssClass=" form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">LC Open Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtLCOpenDate" runat="server" CssClass=" form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label ">LC Mature Date</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtLCMatureDate" runat="server" CssClass=" form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label ">LC Value ($)</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtLCValue" runat="server" CssClass="quantitydecimalWithFivePrecision form-control"></asp:TextBox>
                    </div>

                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">LC Types</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlLCTypes" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                            <asp:ListItem Value="Confirmed">Confirmed</asp:ListItem>
                            <asp:ListItem Value="Unconfirmed">Unconfirmed</asp:ListItem>
                            <asp:ListItem Value="Deferred">Deferred</asp:ListItem>
                            <asp:ListItem Value="Sight">Sight</asp:ListItem>
                            <asp:ListItem Value="Revocable">Revocable</asp:ListItem>
                            <asp:ListItem Value="Irrevocable">Irrevocable</asp:ListItem>
                            <asp:ListItem Value="Transferable">Transferable</asp:ListItem>
                            <asp:ListItem Value="BackToBack">Back To Back</asp:ListItem>
                            <asp:ListItem Value="UPass">U Pass</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label required-field">Incoterms</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlIncoterms" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                            <asp:ListItem Value="N/A">N/A</asp:ListItem>
                            <asp:ListItem Value="EXW">EXW</asp:ListItem>
                            <asp:ListItem Value="FCA">FCA</asp:ListItem>
                            <asp:ListItem Value="CPT">CPT</asp:ListItem>
                            <asp:ListItem Value="CIP">CIP</asp:ListItem>
                            <asp:ListItem Value="DAP">DAP</asp:ListItem>
                            <asp:ListItem Value="DDP">DDP</asp:ListItem>
                            <asp:ListItem Value="FAS">FAS</asp:ListItem>
                            <asp:ListItem Value="FOB">FOB</asp:ListItem>
                            <asp:ListItem Value="CFR">CFR</asp:ListItem>
                            <asp:ListItem Value="CIF">CIF</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">LC Manage Account</label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlLCManageAccount" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Supplier</label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="PODInformation" class="panel panel-default">
                    <div class="panel-heading">
                        Purchase Order Detail Information 
                    </div>
                    <div class="panel-body">
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label">Purchase Order</label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPODInformation" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <table id="tblPODInformation" class="table table-bordered table-condensed table-responsive">
                        </table>
                        <div class="form-group">

                            <div class="col-md-2">
                                <label class="control-label">Total Purchase Amount</label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTotalPurchaseAmount" runat="server" CssClass="quantitydecimalWithFivePrecision form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="PDInformation" class="panel panel-default">
                    <div class="panel-heading">
                        Payment Detail Information
                    </div>
                    <div class="panel-body">
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label required-field">Account Head</label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlAccountHead" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label required-field">Amount</label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtAmount" runat="server" CssClass="quantitydecimalWithFivePrecision form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <label class="control-label ">Payment Date</label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPaymentDate" runat="server" CssClass=" form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label required-field">Currency</label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div id="convertionRateContainer">
                                <div class="col-md-2">
                                    <label class="control-label required-field">Conversion Rate</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtConversionRate" runat="server" CssClass="quantitydecimalWithFivePrecision form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label">Remarks</label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox TextMode="MultiLine" ID="txtRemarks" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" style="padding-top: 10px;">
                            <div class="col-md-12">
                                <input id="btnAdd" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" onclick="AddPaymentDetail()" />
                                <input id="btnCancel" type="button" value="Cancel"
                                    class="TransactionalButton btn btn-primary btn-sm" onclick="Cancel()" />
                            </div>
                        </div>
                        <div style="height: 250px; overflow-y: scroll;">
                            <table id="tblPDInformation" class="table table-bordered table-condensed table-hover">
                                <thead>
                                    <tr>
                                        <th style="width: 15%;">Account Head</th>
                                        <th style="width: 15%;">Amount</th>
                                        <th style="width: 10%;">Payment Date</th>
                                        <th style="width: 10%;">Currency</th>
                                        <th style="width: 10%;">Conversion Rate</th>
                                        <th style="width: 20%;">After Conversion (BDT)</th>
                                        <th style="width: 15%;">Remarks</th>
                                        <th style="width: 5%;">Action</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label">Total Payment Amount</label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTotalPaymentAmount" runat="server" CssClass="quantitydecimalWithFivePrecision form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Attachment</label>
                    </div>
                    <div class="col-md-4">
                        <input id="btnImageUp" type="button" onclick="javascript: return LoadDocUploader();"
                            class="TransactionalButton btn btn-primary btn-sm" value="LC Doc..." />
                    </div>
                </div>
                <div id="DocumentInfo">
                </div>
                <div class="form-group col-md-12" id="CompanyProjectPanel">

                    <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />
                </div>
                <div class="form-group" style="padding-top: 10px;">
                    <div class="col-md-12">
                        <input id="btnSave" type="button" onclick="SaveNClose()" value="Save" class="TransactionalButton btn btn-primary btn-sm" />
                        <input id="btnClear" type="button" value="Clear" onclick="javascript: return Clear();"
                            class="TransactionalButton btn btn-primary btn-sm" />
                    </div>
                </div>


            </div>
        </div>
    </div>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#ContentPlaceHolder1_companyProjectUserControl_hfIsSingle").val() != "1") {

                if ($("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val() == "0") {
                    $("#ContentPlaceHolder1_companyProjectUserControl_ddlGLCompany").val($("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val()).trigger("change");
                }
            }
        });
    </script>
</asp:Content>
