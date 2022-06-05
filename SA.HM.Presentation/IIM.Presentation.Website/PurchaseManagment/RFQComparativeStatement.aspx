<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="RFQComparativeStatement.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.RFQComparativeStatement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        var RFQuotationItemsTable;
        var ItemList;
        $(document).ready(function () {

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            RFQuotationItemsTable = $("#tblRFQuotationItems").DataTable({
                data: [],
                columns: [
                    { title: "", "data": "RFQItemId", visible: false },
                    { title: "Item Name", "data": "ItemName", sWidth: '15%' },
                    { title: "Unit Name", "data": "StockUnitName", sWidth: '40%' },
                    { title: "Quantity", "data": "Quantity", sWidth: '30%' },
                    { title: "Action", "data": null, sWidth: '15%' }
                ],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {


                    var row = '';
                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }

                    row += "&nbsp;&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return viewComparativeStatement('" + aData.RFQItemId + "', '" + aData.ItemName + "');\"><img alt=\"Comparative Statement\" src=\"../Images/comparison.png\" title='Comparative Statement' /></a>";


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

            //$("#ContentPlaceHolder1_ddlIndentName").change(function () {

            //    var IndentId = parseInt($("#ContentPlaceHolder1_ddlIndentName").val().trim());

            //    PageMethods.LoadItemNameByIndentId(IndentId, OnLoadItemNameByIndentIdSucceed, OnLoadItemNameByIndentIdFailed);


            //    return false;

            //});


            $('#ContentPlaceHolder1_txtSearchToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                //defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchFromDate').datepicker("option", "maxDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_ddlIndentName").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            //$("#ContentPlaceHolder1_ddlItemName").select2({
            //    tags: "true",
            //    placeholder: "--- Please Select ---",
            //    allowClear: true,
            //    width: "99.75%"
            //});

        });

        //function OnLoadItemNameByIndentIdSucceed(result) {

        //    //debugger;
        //    typesList = [];
        //    $("#ContentPlaceHolder1_ddlItemName").empty();
        //    var i = 0, fieldLength = result.length;

        //    if (fieldLength > 0) {
        //        typesList = result;
        //        $('<option value="' + 0 + '">' + '--- Please Select ---' + '</option>').appendTo('#ContentPlaceHolder1_ddlItemName');
        //        for (i = 0; i < fieldLength; i++) {
        //            $('<option value="' + result[i].RFQItemId + '">' + result[i].ItemName + '</option>').appendTo('#ContentPlaceHolder1_ddlItemName');
        //        }
        //        //$("#ContentPlaceHolder1_ddlGLProject").attr("Enabled", true);
        //    }
        //    else {
        //        $("<option value='0'>--No Items Found--</option>").appendTo("#ContentPlaceHolder1_ddlItemName");



        //    }

        //    return false;

        //}

        function viewComparativeStatement(RFQItemId, ItemName) {


            $("#ComparativeStatementDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "83%",
                height: 700,
                closeOnEscape: true,
                resizable: false,
                title: "Comparative Statement" ,
                show: 'slide'
            });

            var ItemObject = _.findWhere(ItemList, { RFQItemId: parseInt(RFQItemId, 10) });
            if (ItemObject != null) {
                var time = ItemObject.CreatedDate.getHours() + ":" + (ItemObject.CreatedDate.getMinutes() < 10 ? "0" : "") + ItemObject.CreatedDate.getMinutes();
                $("#ContentPlaceHolder1_txtProductName").val(ItemObject.ItemName);
                $("#ContentPlaceHolder1_txtCreatedDate").val(CommonHelper.DateFromDateTimeToDisplay(ItemObject.CreatedDate, innBoarDateFormat) + " " + time);
                $("#ContentPlaceHolder1_txtPaymentMode").val(ItemObject.PaymentTerm == "Cash" ? ItemObject.PaymentTerm : ItemObject.PaymentTerm +"(Credit Pay Days : "+ ItemObject.CreditDays+")");
                $("#ContentPlaceHolder1_txtDeliveryTerms").val(ItemObject.DeliveryTerms == "OnSite" ? ItemObject.DeliveryTerms : ItemObject.DeliveryTerms + " (" + ItemObject.SiteAddress + ")");
                $("#ContentPlaceHolder1_txtQuantity").val(ItemObject.Quantity);
                $("#ContentPlaceHolder1_txtItemUnit").val(ItemObject.StockUnitName);
                $("#ContentPlaceHolder1_txtIndentTitle").val(ItemObject.IndentName);
                $("#ContentPlaceHolder1_txtCreatedBy").val(ItemObject.CreatedByName);
            }
            else {
                $("#ContentPlaceHolder1_txtProductName").val("");
                $("#ContentPlaceHolder1_txtCreatedDate").val("");
                $("#ContentPlaceHolder1_txtPaymentMode").val("");
                $("#ContentPlaceHolder1_txtDeliveryTerms").val("");
                $("#ContentPlaceHolder1_txtQuantity").val("");
                $("#ContentPlaceHolder1_txtItemUnit").val("");
                $("#ContentPlaceHolder1_txtIndentTitle").val("");
                $("#ContentPlaceHolder1_txtCreatedBy").val("");
            }

            

            PageMethods.GetRFQuotationItemFeedbackByRFQItemId(RFQItemId, OnGetRFQuotationItemFeedbackByRFQItemIdSucceed, OnGetRFQuotationItemFeedbackByRFQItemIdFailed);

        }


        function OnGetRFQuotationItemFeedbackByRFQItemIdSucceed(result) {
            debugger;

            $("#DivComparativeStatements").html("");

            if (result.length > 0) {
                var strTable = "";

                var QuoteDate = "";

                var UnitPrice = "";
                var Discount = "";


                var OfferedUnitPrice = "";
                var ApplicableVATAIT = "";
                var OfferedUnitPriceWithVATAIT = "";
                var TotalPrice = "";
                var DeliveryCharge = "";
                var Advance = "";
                var DeliveryDuration = "";
                var AdditionalInformation = "";
                var ItemRemarks = "";

                var OfferValidation = "";


                //var tableColWidth = parseFloat(100.0 / (result.length));

                strTable = "&nbsp;&nbsp;<table id=\"tblComparativeStatements\" class=\"table table-bordered table-condensed table-hover\" cellspacing=\"0\" cellpadding=\"4\">" +
                    "<thead>" +
                    "<tr style=\"color: White; background-color: #44545E; font-weight: bold;\">" +
                    "<th style=\"min-width: 200px;\">" +
                    "Name" +
                    "</th>";
                var i = 0;
                for (i = 0; i < result.length; i++) {

                    strTable += "<th style=\"min-width: 200px;\">" +
                        result[i].SupplierName +
                        "</th>";

                    var time = result[i].QuoteDate.getHours() + ":" + (result[i].QuoteDate.getMinutes() < 10 ? "0" : "") + result[i].QuoteDate.getMinutes() ;

                    QuoteDate += "<td style=\"  min-width: 200px;\">" +
                        CommonHelper.DateFromDateTimeToDisplay(result[i].QuoteDate, innBoarDateFormat) + " " + time +
                        "</td>";

                    OfferedUnitPrice += "<td style=\"  min-width: 200px;\">" +
                        result[i].OfferedUnitPrice +
                        "</td>";

                    ApplicableVATAIT += "<td style=\"  min-width: 200px;\">" +
                        (result[i].OfferedUnitPriceWithVatAit - result[i].OfferedUnitPrice).toFixed(2) +
                        "</td>";

                    OfferedUnitPriceWithVATAIT += "<td style=\"  min-width: 200px;\">" +
                        result[i].OfferedUnitPriceWithVatAit +
                        "</td>";
                    TotalPrice += "<td style=\"  min-width: 200px;\">" +
                        result[i].BillingAmount +
                        "</td>";
                    ItemRemarks += "<td style=\"  min-width: 200px;\">" +
                        result[i].ItemRemarks +
                        "</td>";
                    //DeliveryCharge += "<td style=\" text-align: center; min-width: 200px;\">" +
                    //    result[i].QuoteDate +
                    //    "</td>";
                    Advance += "<td style=\"  min-width: 200px;\">" +
                        result[i].AdvanceAmount +
                        "</td>";
                    DeliveryDuration += "<td style=\"  min-width: 200px;\">" +
                        result[i].DeliveryDuration +
                        "</td>";

                    OfferValidation += "<td style=\"  min-width: 200px;\">" +
                        result[i].OfferValidation +
                        "</td>";

                    UnitPrice += "<td style=\"  min-width: 200px;\">" +
                        result[i].UnitPrice +
                        "</td>";

                    Discount += "<td style=\"  min-width: 200px;\">" +
                        result[i].Discount +
                        "</td>";

                    

                }

                strTable += "</tr>" +
                    "</thead>" +
                    "<tbody>";
                alternateColorFirst = "style=\"background-color:#E3EAEB;\"";
                alternateColorSecond = "style=\"background-color:#FFFFFF;\"";


                strTable += "<tr " + alternateColorFirst + ">" +
                    "<td style=\"  min-width: 200px;\">" +
                    "Quote Date" +
                    "</td>" + QuoteDate +
                    "</tr>";

                strTable += "<tr " + alternateColorSecond + ">" +
                    "<td style=\"  min-width: 200px;\">" +
                    "Unit Price" +
                    "</td>" + UnitPrice +
                    "</tr>";

                strTable += "<tr " + alternateColorFirst + ">" +
                    "<td style=\"  min-width: 200px;\">" +
                    "Discount (%)" +
                    "</td>" + Discount +
                    "</tr>";

                strTable += "<tr " + alternateColorSecond + ">" +
                    "<td style=\"  min-width: 200px;\">" +
                    "Offered Unit Price(Without VAT & AIT)" +
                    "</td>" + OfferedUnitPrice +
                    "</tr>";

                strTable += "<tr " + alternateColorFirst + ">" +
                    "<td style=\"  min-width: 200px;\">" +
                    "Applicable VAT & AIT" +
                    "</td>" + ApplicableVATAIT +
                    "</tr>";

                strTable += "<tr " + alternateColorSecond + ">" +
                    "<td style=\"  min-width: 200px;\">" +
                    "Offered Unit Price (With VAT & AIT)" +
                    "</td>" + OfferedUnitPriceWithVATAIT +
                    "</tr>";

                strTable += "<tr " + alternateColorFirst + ">" +
                    "<td style=\"  min-width: 200px;\">" +
                    "Total Price (With VAT & AIT)" +
                    "</td>" + TotalPrice +
                    "</tr>";

                strTable += "<tr " + alternateColorSecond + ">" +
                    "<td style=\"  min-width: 200px;\">" +
                    "Advance" +
                    "</td>" + Advance +
                    "</tr>";

                strTable += "<tr " + alternateColorFirst + ">" +
                    "<td style=\"  min-width: 200px;\">" +
                    "Delivery Duration" +
                    "</td>" + DeliveryDuration +
                    "</tr>";

                strTable += "<tr " + alternateColorSecond + ">" +
                    "<td style=\"  min-width: 200px;\">" +
                    "Offer Validation" +
                    "</td>" + OfferValidation +
                    "</tr>";

                var strDetails = "";
                var specificationLen = result[0].RFQuotationItemSpecificationsFeedback.length;
                var alternateColor;

                for (i = 0; i < specificationLen; i++) {

                    if (i % 2 == 0) {
                        alternateColor = "style=\"background-color:#E3EAEB;\"";
                    }
                    else
                        alternateColor = "style=\"background-color:#FFFFFF;\"";

                    var j = 0;
                    strDetails += "<tr " + alternateColor + ">" +
                        "<td style=\"  min-width:  200px;\">" +
                        result[0].RFQuotationItemSpecificationsFeedback[i].Title + " : " + result[0].RFQuotationItemSpecificationsFeedback[i].Value +
                        "</td>";
                        
                    for(j = 0; j < result.length; j++) {

                        strDetails += "<td style=\"  min-width: 200px;\">" +
                            result[j].RFQuotationItemSpecificationsFeedback[i].Feedback +
                            "</td>";

                    }

                    strDetails += "</tr>";


                }

                strTable += strDetails;

                strTable += "<tr " + alternateColorFirst + ">" +
                    "<td style=\"  min-width: 200px;\">" +
                    "Item Remarks" +
                    "</td>" + ItemRemarks +
                    "</tr>"; 










                //"<th style=\"width: 33%;\">" +
                //    "Buyer Specification" +
                //    "</th>" +
                //    "<th style=\"width: 34%;\">" +
                //    "Supplier Specification" +
                //    "</th>";

                //var j = 0;
                //var tableLen = result.RFQuotationItems[i].RFQuotationItemSpecifications.length;

                //for (j = 0; j < tableLen; j++) {
                //    if (j % 2 == 0) {
                //        alternateColor = "style=\"background-color:#E3EAEB;\"";
                //    }
                //    else
                //        alternateColor = "style=\"background-color:#FFFFFF;\"";

                //    strTable += "<tr " + alternateColor + ">" +
                //        "<td style=\"display:none;\">" +
                //        result.RFQuotationItems[i].RFQuotationItemSpecifications[j].Id +
                //        "</td>" +
                //        "<td style=\"display:none;\">" + result.RFQuotationItems[i].RFQuotationItemSpecifications[j].RFQItemId +
                //        "</td>" +
                //        "<td style=\" text-align: center; width: 33%;\">" +
                //        result.RFQuotationItems[i].RFQuotationItemSpecifications[j].Title +
                //        "</td>" +
                //        "<td style=\"width: 33%; \">" +
                //        result.RFQuotationItems[i].RFQuotationItemSpecifications[j].Value +
                //        "</td>" +
                //        "<td style=\"width: 34%; \">" +
                //        '<input type=\'text\' class=\'input_text form-control\'  id=\'SupplierSpecification_text' + i + j + '\' placeholder=\'\' value=\'As Required\'  >' +
                //        "</td>" +

                //        "</tr>";
                //}
                strTable += " </tbody> </table>";


                $("#DivComparativeStatements").html(strTable);
            }

            return false;
        }


        function OnGetRFQuotationItemFeedbackByRFQItemIdFailed(error) {
            toastr.error(error.get_message());
            return false;
        }

        function SearchInformation(pageNumber, IsCurrentOrPreviousPage) {
            debugger;
            var gridRecordsCount = RFQuotationItemsTable.data().length;


            var IndentId = $("#ContentPlaceHolder1_ddlIndentName").val();
            if (IndentId == "0") {
                toastr.warning("Please Select Indent Name.");
                $("#ContentPlaceHolder1_ddlIndentName").focus();
                return false;
            }
            //var ItemId = $("#ContentPlaceHolder1_ddlItemName").val();
            //if (ItemId == "0") {
            //    toastr.warning("Please Select Item Name.");
            //    $("#ContentPlaceHolder1_ddlItemName").focus();
            //    return false;
            //}



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

            PageMethods.GetRFQuotationItems(IndentId, fromDate, toDate, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnRFQuotationLoadingSucceed, OnRFQuotationLoadingFailed);
            return false;
        }

        function OnRFQuotationLoadingSucceed(result) {
            debugger;
            RFQuotationItemsTable.clear();
            RFQuotationItemsTable.rows.add(result.GridData);
            RFQuotationItemsTable.draw();
            ItemList = result.GridData;

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

        //function OnLoadItemNameByIndentIdFailed(xhr, err) {
        //    toastr.error(xhr.responseText);
        //}
    </script>

    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />

    <div class="panel panel-default">
        <div class="panel-heading">
            Comparative Statement
        </div>
        <div class="panel-body">
            <div class="form-horizontal">

                <%--<div class="form-group" >
                    <div class="col-md-2 ">
                        <asp:Label ID="lblQuotationType" runat="server" class="control-label " Text="Quotation Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList runat="server" ID="ddlQuotationType" CssClass="form-control">
                            <asp:ListItem Text="Indent Wise" Value="IndentWise"></asp:ListItem>
                            <asp:ListItem Text="Item Wise" Value="ItemWise"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2 ">
                        <asp:Label ID="lblIndentOrItem" runat="server" class="control-label " Text="Indent / Item"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtIndentOrItem" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>--%>

                <div class="form-group">
                    <div class="col-md-2 ">
                        <asp:Label ID="lblIndentName" runat="server" class="control-label " Text="Indent Name"></asp:Label>
                    </div>
                    <div class="col-md-4">

                        <asp:DropDownList runat="server" ID="ddlIndentName" CssClass="form-control">
                        </asp:DropDownList>

                    </div>
                    <%--<div class="col-md-2 ">
                        <asp:Label ID="lblItemName" runat="server" class="control-label " Text="Item Name"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList runat="server" ID="ddlItemName" CssClass="form-control">
                        </asp:DropDownList>
                    </div>--%>
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
                    <table id="tblRFQuotationItems" class="table table-bordered table-condensed table-responsive">
                    </table>
                    <div class="childDivSection">
                        <div class="text-center" id="GridPagingContainer">
                            <ul class="pagination">
                            </ul>
                        </div>
                    </div>
                </div>
                <div id="ComparativeStatementDialog" style="display: none">
                    <div style="padding: 10px 30px 10px 30px">
                        <div class="form-horizontal">

                            <div class="form-group">

                                <div class="col-md-2 ">
                                    <label class="control-label">Product Name</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtProductName" runat="server" TextMode="MultiLine" CssClass=" form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-2 ">
                                    <label class="control-label">Indent Title</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtIndentTitle" runat="server" CssClass=" form-control" Enabled="false"></asp:TextBox>
                                </div>


                            </div>

                            <div class="form-group">

                                <div class="col-md-2 ">
                                    <label class="control-label">Created Date</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCreatedDate" runat="server" CssClass=" form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-2 ">
                                    <label class="control-label">Created By</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCreatedBy" runat="server" CssClass=" form-control" Enabled="false"></asp:TextBox>
                                </div>


                            </div>

                            <div class="form-group">

                                <div class="col-md-2 ">
                                    <label class="control-label">Payment Mode</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPaymentMode" runat="server" CssClass=" form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-2 ">
                                    <label class="control-label">Quantity</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtQuantity" runat="server" CssClass=" form-control" Enabled="false"></asp:TextBox>
                                </div>


                            </div>

                            <div class="form-group">

                                <div class="col-md-2 ">
                                    <label class="control-label">Delivery Terms</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDeliveryTerms" runat="server" CssClass=" form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-md-2 ">
                                    <label class="control-label">Item Unit</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtItemUnit" runat="server" CssClass=" form-control" Enabled="false"></asp:TextBox>
                                </div>


                            </div>

                            <div class="form-group" id="DivComparativeStatements" style="overflow-x: scroll">
                            </div>


                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>

</asp:Content>
