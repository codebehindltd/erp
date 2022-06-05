<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="SalesBill.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.SalesBill" %>

<%@ Register TagPrefix="UserControl" TagName="CompanyProjectUserControl" Src="~/HMCommon/UserControl/CompanyProjectUserControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        "use strict";
        let QuotationTable;

        $(document).ready(() => {
            QuotationTable = $("#tblQuotation").DataTable({
                data: [],
                columns: [
                    { title: "Quotation No.", data: "QuotationNo", width: "10%" },
                    { title: "Deal Name", data: "DealName", width: "30%" },
                    { title: "Company", data: "CompanyName", width: "35%" },
                    { title: "Proposal Date", data: "ProposalDate", width: "15%" },
                    { title: "Action", data: null, width: "10%" },
                    { title: "", data: "QuotationId", visible: false }
                ],
                rowCallback: (row, data, displayNum, displayIndex, dataIndex) => {
                    var tableRow = "";

                    if (displayIndex % 2 == 0) {
                        $('td', row).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', row).css('background-color', '#FFFFFF');
                    }
                    if (!data.IsApprovedFromBilling)
                        tableRow += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return GetQuotationDetails('" + data.QuotationId + "');\"> <img alt=\"Approve\" src=\"../Images/approved.png\" title='Approve' /> </a>";

                    tableRow += "&nbsp;&nbsp;<a href='javascript:void();' onclick= 'ShowQuotationInvoice(" + data.QuotationId + ")' title='Quotation Invoice' ><img style='width:16px;height:16px;' alt='Quotation Invoice' src='../Images/ReportDocument.png' /></a>";

                    $('td:eq(' + (row.children.length - 1) + ')', row).html(tableRow);
                },
                info: false,
                ordering: false,
                processing: false,
                paging: false,
                filter: false,
                language: {
                    emptyTable: "No Data Found"
                }
            });

            $("#ContentPlaceHolder1_txtFromDate").datepicker({
                dateFormat: innBoarDateFormat,
                defaultDate: DayOpenDate,
                changeMonth: true,
                changeYear: true
            });

            $("#ContentPlaceHolder1_txtToDate").datepicker({
                dateFormat: innBoarDateFormat,
                defaultDate: DayOpenDate,
                changeMonth: true,
                changeYear: true
            });

            $("#ContentPlaceHolder1_ddlCompany").select2({
                tags: false,
                allowClear: true,
                width: "99.75%"
            });

            SearchQuotation();
            var isSingle = $("#ContentPlaceHolder1_companyProjectUserControl_hfIsSingle").val()
            if (isSingle == "1") {
                $("#GLCompanyProjectDiv").hide();
            }
            else {
                $("#GLCompanyProjectDiv").show();
            }
        });

        var GridPaging = (pageNumber, isCurrentOrPreviousPage) => {
            let gridRecordsCount = QuotationTable.data().length;

            let quotationNumber = $("#ContentPlaceHolder1_txtQuotationNumber").val().trim();
            let companyId = $("#ContentPlaceHolder1_ddlCompany").val();
            let fromDate = $("#ContentPlaceHolder1_txtFromDate").val();
            let toDate = $("#ContentPlaceHolder1_txtToDate").val();

            if (fromDate == "" && toDate != "") {
                fromDate = CommonHelper.DateFormatToMMDDYYYY(DayOpenDate, '/');
            }
            if (fromDate != "" && toDate == "") {
                toDate = CommonHelper.DateFormatToMMDDYYYY(DayOpenDate, '/');
            }

            fromDate = fromDate != "" ? CommonHelper.DateFormatToMMDDYYYY(fromDate, '/') : null;
            toDate = toDate != "" ? CommonHelper.DateFormatToMMDDYYYY(toDate, '/') : null;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "./SalesBill.aspx/SearchQuotation",
                dataType: "json",
                data: JSON.stringify({ quotationNumber: quotationNumber, companyId: companyId, fromDate: fromDate, toDate: toDate, gridRecordsCount: gridRecordsCount, pageNumber: pageNumber, isCurrentOrPreviousPage: isCurrentOrPreviousPage }),
                async: false,
                success: (data) => {
                    GenerateQuotationTable(data);
                },
                error: (error) => {
                    toastr.error(error, "", { timeOut: 5000 });
                }
            });
            return false;
        }

        var SearchQuotation = () => {
            GridPaging(1, 1);
        }

        var GenerateQuotationTable = (result) => {
            result.d.GridData.map(row => { row.ProposalDate = CommonHelper.DateFromStringToDisplay(row.ProposalDate, innBoarDateFormat); });

            QuotationTable.clear();
            QuotationTable.rows.add(result.d.GridData);
            QuotationTable.draw();

            $("#GridPagingContainer ul").html("");

            $("#GridPagingContainer ul").append(result.d.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.d.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.d.GridPageLinks.NextButton);

            return true;
        }

        var ShowQuotationInvoice = (quotationId) => {
            var url = "/SalesAndMarketing/Reports/SMQuotationDetailsReport.aspx?id=" + quotationId + "&frmacc=1";
            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=800,height=680,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
            return false;
        }

        var GetQuotationDetails = (quotationId) => {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=UTF-8",
                url: "./SMQuotationSearch.aspx/GetQuotationDetailById",
                dataType: "json",
                data: JSON.stringify({ quotationId: quotationId }),
                async: false,
                success: (data) => {
                    LoadQuotationDetails(data.d);
                },
                error: (error) => {
                    toastr.error(error, "", { timeOut: 5000 });
                }
            });
            return false;
        }

        var LoadQuotationDetails = (result) => {
            $("#QuotationDetailsDialog").dialog({
                autoOpen: true,
                modal: true,
                width: 1300,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Quotation Details",
                show: 'slide'
            });

            $("#ContentPlaceHolder1_hfQuotationId").val(result.Quotation.QuotationId);
            $("#ContentPlaceHolder1_hfDealId").val(result.Quotation.DealId);
            $("#ContentPlaceHolder1_lblProposalDate").text(CommonHelper.DateFromStringToDisplay(result.Quotation.ProposalDate, innBoarDateFormat));
            $("#ContentPlaceHolder1_lblServiceType").text(result.Quotation.ServiceName);
            $("#ContentPlaceHolder1_lblDeviceOrUser").text(result.Quotation.TotalDeviceOrUser);
            $("#ContentPlaceHolder1_lblContractPeriod").text(result.Quotation.ContractPeriodName);
            $("#ContentPlaceHolder1_lblBillingPeriod").text(result.Quotation.BillingPeriodName);
            $("#ContentPlaceHolder1_lblDeliveryBy").text(result.Quotation.ItemServiceDeliveryName);
            $("#ContentPlaceHolder1_lblCurrentVendor").text(result.Quotation.CurrentVendorName);
            $("#ContentPlaceHolder1_lblRemarks").text(result.Quotation.Remarks);

            if (result.Quotation.CompanyId > 0) {
                $("#tblCompanyinfo").show();
                $("#tblContactinfo").hide();

                $("#ContentPlaceHolder1_txtCompanySearch").text(result.Company.CompanyName);
                $("#ContentPlaceHolder1_txtCompanyType").text(result.Company.TypeName);
                $("#ContentPlaceHolder1_lblIndustry").text(result.Company.IndustryName);
                $("#ContentPlaceHolder1_lblOwnership").text(result.Company.OwnershipName);
                $("#ContentPlaceHolder1_lblContactNo").text(result.Company.ContactNumber);
                $("#ContentPlaceHolder1_lblEMail").text(result.Company.EmailAddress);
                $("#ContentPlaceHolder1_lblWebsite").text(result.Company.WebAddress);
                $("#ContentPlaceHolder1_lblLifeCycleStage").text(result.Company.LifeCycleStage);
                $("#ContentPlaceHolder1_txtCompanyOrContactName").text(result.Company.CompanyName);

                var adress = "";
                adress += (result.Company.BillingStreet != null && result.Company.BillingStreet != "") ? result.Company.BillingStreet + " , " : "";
                adress += (result.Company.BillingState != null && result.Company.BillingStreet != "") ? result.Company.BillingState + " , " : "";
                adress += (result.Company.BillingCity != null && result.Company.BillingStreet != "") ? result.Company.BillingCity + " , " : "";
                adress += (result.Company.BillingPostCode != null && result.Company.BillingStreet != "") ? "PostCode: " + result.Company.BillingPostCode + " , " : "";
                adress += (result.BillingCountry != null && result.BillingStreet != "") ? result.BillingCountry + " , " : "";

                $("#ContentPlaceHolder1_lblCompanyAddress").val(adress);
            }
            else if (result.Quotation.ContactId > 0) {
                $("#tblContactinfo").show();
                $("#tblCompanyinfo").hide();
                $("#ContentPlaceHolder1_txtContactName").text(result.Contact.Name);
                $("#ContentPlaceHolder1_lblContactTitle").text(result.Contact.JobTitle);
                $("#ContentPlaceHolder1_lblDepartment").text(result.Contact.Department);
                $("#ContentPlaceHolder1_lblContactMobile").text(result.Contact.MobilePersonal);
                $("#ContentPlaceHolder1_lblContactPhone").text(result.Contact.PhonePersonal);
                $("#ContentPlaceHolder1_lblConatctEmail").text(result.Contact.EmailWork);
                $("#ContentPlaceHolder1_lblPersonalAddress").text(result.Contact.PersonalAddress);
                $("#ContentPlaceHolder1_lblContactLifeCycleStage").text(result.Contact.ContactLifeCycleStage);
            }

            var tr = "";
            $("#QuotationItemInformation tbody").html("");
            var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00;
            if (result.QuotationItemDetails.length > 0) {
                $("#lblItem").show();
                $("#dvItem").show();
            }
            else {
                $("#lblItem").hide();
                $("#dvItem").hide();
            }
            $.each(result.QuotationItemDetails, function (index, itm) {

                if ((index % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                tr += "<td align='left' style=\"width:30%; text-align:Left;\">" + itm.ItemName + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.HeadName + "</td>";

                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.Quantity + "</td>";

                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.UnitPrice + "</td>";
                tr += "<td align='left' style='width: 15%;'>" + itm.TotalPrice + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.StockBy + "</td>";

                tr += "</tr>";

                $("#QuotationItemInformation tbody").append(tr);
                tr = "";

                totalQuantity = totalQuantity + itm.Quantity;
                totalUnitCost = totalUnitCost + itm.UnitPrice;
                totalCost = totalCost + itm.TotalPrice;
            });

            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(1)").text(totalQuantity);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(2)").text(totalUnitCost);
            $("#QuotationItemInformation tfoot tr:eq(0)").find("td:eq(3)").text(totalCost);

            var contractPeriodValue = 0.00;

            tr = ""; totalQuantity = 0.00; totalUnitCost = 0.00; totalCost = 0.00;

            $("#QuotationServicenformation tbody").html("");
            if (result.QuotationServiceDetails.length > 0) {
                $("#lblService").show();
                $("#dvService").show();
            }
            else {
                $("#lblService").hide();
                $("#dvService").hide();
            }
            $.each(result.QuotationServiceDetails, function (index, itm) {
                if ((index % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }


                tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + itm.ServiceType + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.ItemName + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.PackageName + "</td>";
                tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + itm.Downlink + "</td>";
                tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + itm.Uplink + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.UnitPrice + "</td>";
                tr += "<td align='left' style='width: 15%;'>" + itm.TotalPrice + "</td>";

                tr += "</tr>";

                $("#QuotationServicenformation tbody").append(tr);

                totalQuantity = totalQuantity + itm.Quantity;
                totalUnitCost = totalUnitCost + itm.UnitPrice;
                totalCost = totalCost + itm.TotalPrice;
                tr = "";
            });

            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(1)").text(totalUnitCost);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(2)").text(totalCost);
            LoadDiscountItem(result.QuotationDetails);
        }

        var ApproveQuotation = () => {
            let quotationId = Math.trunc($("#ContentPlaceHolder1_hfQuotationId").val());
            let dealId = Math.trunc($("#ContentPlaceHolder1_hfDealId").val());

            let glCompanyId = Math.trunc($("#ContentPlaceHolder1_companyProjectUserControl_hfGLCompanyId").val());
            let glProjectId = Math.trunc($("#ContentPlaceHolder1_companyProjectUserControl_hfGLProjectId").val());

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "./SalesBill.aspx/ApproveQuotation",
                data: JSON.stringify({ quotationId: quotationId, dealId: dealId, glCompanyId: glCompanyId, glProjectId: glProjectId }),
                dataType: "json",
                async: false,
                success: (result) => {
                    if (result.d.IsSuccess) {
                        CommonHelper.AlertMessage(result.d.AlertMessage);
                        SearchQuotation();
                        $("#QuotationDetailsDialog").dialog('close');
                        //SendMail(quotationId);
                    }
                },
                error: (error) => {
                    toastr.error(error, "", { timeOut: 5000 });
                }
            });
        }
        var SendMail = (quotationId) => {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "./Reports/SMQuotationDetailsReport.aspx?id=" + quotationId + "&frmacc=1",
                //data: JSON.stringify({ quotationId: quotationId }),
                dataType: "json",
                success: (result) => {

                },
                error: (error) => {
                }
            });
        }

        function LoadDiscountItem(QuotationDetails) {
            $("#lblQuotaionBanquet").hide();
            $("#dvQuotaionBanquet").hide();
            $("#lblQuotaionRestaurant").hide();
            $("#dvQuotaionRestaurant").hide();
            $("#lblQuotaionRoom").hide();
            $("#dvQuotaionRoom").hide();
            $("#lblQuotaionServiceOutLet").hide();
            $("#dvQuotaionServiceOutLet").hide();

            _(QuotationDetails).each((val, index) => {
                if (val.ItemType == "GuestRoom")
                    LoadGuestRoomDiscount(val);
                else if (val.ItemType == "Restaurant")
                    LoadRestuarantDiscount(val);
                else if (val.ItemType == "Banquet")
                    LoadBanquetDiscount(val);
                else if (val.ItemType == "ServiceOutlet")
                    LoadServiceOutletDiscount(val);
            });
        }

        function LoadGuestRoomDiscount(data) {

            $("#QuotationRoominformation tbody").html("");
            var tr = "";
            if (data.QuotationDiscountDetails.length == 0 && data.IsDiscountForAll) {
                data.QuotationDiscountDetails.push({
                    TypeName: "All",
                    DiscountType: data.DiscountType,
                    DiscountAmount: data.DiscountAmount,
                    DiscountAmountUSD: data.DiscountAmountUSD
                });
            }
            $.each(data.QuotationDiscountDetails, function (index, itm) {

                if ((index % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:60%; text-align:Left;\">" + itm.TypeName + "</td>";
                tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + itm.DiscountType + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.DiscountAmount + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.DiscountAmountUSD + "</td>";

                tr += "</tr>";

                $("#QuotationRoominformation tbody").append(tr);
                tr = "";

            });
            $("#lblQuotaionRoom").show();
            $("#dvQuotaionRoom").show();
        }
        function LoadRestuarantDiscount(data) {
            $("#QuotationRestaurantinformation tbody").html("");
            var tr = "", groupMemberCount = 0, currentOutletId = 0;
            if (data.QuotationDiscountDetails.length == 0 && data.IsDiscountForAll) {
                data.QuotationDiscountDetails.push({
                    OutLetName: "All",
                    TypeName: "All",
                    DiscountType: data.DiscountType,
                    DiscountAmount: data.DiscountAmount,
                    DiscountAmountUSD: data.DiscountAmountUSD
                });
            }
            data.QuotationDiscountDetails = _(data.QuotationDiscountDetails).sortBy("OutLetName");
            $.each(data.QuotationDiscountDetails, function (index, itm) {

                if ((index % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                if (currentOutletId != itm.OutLetId) {
                    groupMemberCount = _(data.QuotationDiscountDetails).filter({ OutLetId: itm.OutLetId }).length;

                    currentOutletId = itm.OutLetId;
                    tr += "<td align='left' style=\"width:30%; text-align:Left;\" rowspan=\"" + groupMemberCount + "\">" + itm.OutLetName + "</td>";
                }
                tr += "<td align='left' style=\"width:30%; text-align:Left;\">" + (itm.TypeName || itm.Type) + "</td>";
                tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + itm.DiscountType + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.DiscountAmount + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.DiscountAmountUSD + "</td>";

                tr += "</tr>";

                $("#QuotationRestaurantinformation tbody").append(tr);
                tr = "";

            });
            $("#lblQuotaionRestaurant").show();
            $("#dvQuotaionRestaurant").show();
        }
        function LoadBanquetDiscount(data) {

            $("#QuotationBanquetinformation tbody").html("");
            var tr = "", groupMemberCount = 0, currentOutletId = 0;
            if (data.QuotationDiscountDetails.length == 0 && data.IsDiscountForAll) {
                data.QuotationDiscountDetails.push({
                    OutLetName: "All",
                    TypeName: "All",
                    DiscountType: data.DiscountType,
                    DiscountAmount: data.DiscountAmount,
                    DiscountAmountUSD: data.DiscountAmountUSD
                });
            }
            data.QuotationDiscountDetails = _(data.QuotationDiscountDetails).sortBy("OutLetName");
            $.each(data.QuotationDiscountDetails, function (index, itm) {

                if ((index % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                if (currentOutletId != itm.OutLetId) {
                    groupMemberCount = _(data.QuotationDiscountDetails).filter({ OutLetId: itm.OutLetId }).length;

                    currentOutletId = itm.OutLetId;
                    tr += "<td align='left' style=\"width:30%; text-align:Left;\" rowspan=\"" + groupMemberCount + "\">" + itm.OutLetName + "</td>";
                }

                tr += "<td align='left' style=\"width:30%; text-align:Left;\">" + (itm.TypeName || itm.Type) + "</td>";
                tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + itm.DiscountType + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.DiscountAmount + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.DiscountAmountUSD + "</td>";

                tr += "</tr>";

                $("#QuotationBanquetinformation tbody").append(tr);
                tr = "";

            });
            $("#lblQuotaionBanquet").show();
            $("#dvQuotaionBanquet").show();
        }
        function LoadServiceOutletDiscount(data) {

            $("#QuotationServiceOutLetinformation tbody").html("");
            var tr = "";
            if (data.QuotationDiscountDetails.length == 0 && data.IsDiscountForAll) {
                data.QuotationDiscountDetails.push({
                    TypeName: "All",
                    DiscountType: data.DiscountType,
                    DiscountAmount: data.DiscountAmount,
                    DiscountAmountUSD: data.DiscountAmountUSD
                });
            }
            $.each(data.QuotationDiscountDetails, function (index, itm) {

                if ((index % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:60%; text-align:Left;\">" + itm.TypeName + "</td>";
                tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + itm.DiscountType + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.DiscountAmount + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.DiscountAmountUSD + "</td>";

                tr += "</tr>";

                $("#QuotationServiceOutLetinformation tbody").append(tr);
                tr = "";

            });
            $("#lblQuotaionServiceOutLet").show();
            $("#dvQuotaionServiceOutLet").show();
        }
    </script>
    <asp:HiddenField ID="hfQuotationId" runat="server" Value="0" />
    <asp:HiddenField ID="hfDealId" runat="server" Value="0" />
    <%--    <asp:HiddenField ID="hfGLCompanyId" runat="server" Value="0" />
    <asp:HiddenField ID="hfGLProjectId" runat="server" Value="0" />--%>
    <div id="QuotationDetailsDialog" style="display: none;">

        <div class="panel panel-default">


            <div class="pnlDetails panel-heading">Customer Details</div>
            <div class="pnlDetails panel-body">
                <table id="tblCompanyinfo" class="table-hover">
                    <tbody>
                        <tr>
                            <td class="col-md-4">
                                <asp:Label ID="lblCompany" runat="server" class="control-label" Text="Company Name"></asp:Label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="txtCompanySearch" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <asp:Label ID="Label38" runat="server" class="control-label" Text="Company Type"></asp:Label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="txtCompanyType" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <asp:Label ID="Label39" runat="server" class="control-label" Text="Industry"></asp:Label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblIndustry" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <asp:Label ID="Label40" runat="server" class="control-label" Text="Ownership"></asp:Label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblOwnership" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <asp:Label ID="Label9" runat="server" class="control-label" Text="Phone"></asp:Label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblContactNo" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <asp:Label ID="Label8" runat="server" class="control-label" Text="E-mail"></asp:Label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblEMail" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <asp:Label ID="Label41" runat="server" class="control-label" Text="Website"></asp:Label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblWebsite" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <asp:Label ID="Label42" runat="server" class="control-label" Text="Life Cycle Stage"></asp:Label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblLifeCycleStage" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Address"></asp:Label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblCompanyAddress" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <table id="tblContactinfo" class="table-hover">
                    <tbody>
                        <tr>
                            <td class="col-md-4">
                                <asp:Label ID="Label28" runat="server" class="control-label" Text="Name"></asp:Label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="txtContactName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <asp:Label ID="Label29" runat="server" class="control-label" Text="Title"></asp:Label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblContactTitle" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <asp:Label ID="Label30" runat="server" class="control-label" Text="Department"></asp:Label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblDepartment" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <asp:Label ID="Label32" runat="server" class="control-label" Text="Mobile(Personal)"></asp:Label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblContactMobile" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <asp:Label ID="Label31" runat="server" class="control-label" Text="Phone(Personal)"></asp:Label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblContactPhone" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <asp:Label ID="Label36" runat="server" class="control-label" Text="Personal Address"></asp:Label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblPersonalAddress" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <asp:Label ID="Label33" runat="server" class="control-label" Text="Life Cycle Stage"></asp:Label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblContactLifeCycleStage" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="E-mail(Personal)"></asp:Label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblConatctEmail" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="pnlDetails panel-heading">Proposal References</div>
            <div class="pnlDetails panel-body">
                <table id="tblProposalInfo" class="table-hover">
                    <tbody>
                        <tr>
                            <td class="col-md-4">
                                <label>Proposal Date</label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblProposalDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <label>Proposal For</label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblServiceType" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <label>Device/User(Qtn)</label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblDeviceOrUser" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <label>Contract Period</label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblContractPeriod" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <label>Billing Period</label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblBillingPeriod" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <label>Delivery</label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblDeliveryBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <label>Present ISP</label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblCurrentVendor" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="col-md-4">
                                <label>Remarks</label>
                            </td>
                            <td class="col-md-1" style="font-weight: bold;">:
                            </td>
                            <td class="col-md-4">
                                <asp:Label ID="lblRemarks" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div id="lblItem" style="display: none;" class="pnlDetails panel-heading">Item Information</div>
            <div id="dvItem" style="display: none;" class="pnlDetails panel-body">
                <table id="QuotationItemInformation" class="table table-condensed table-bordered table-responsive">
                    <thead>
                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                            <th scope="col" style="width: 40%;">Item Name</th>
                            <th scope="col" style="width: 15%;">Stock By</th>
                            <th scope="col" style="width: 15%;">Quantity</th>
                            <th scope="col" style="width: 15%;">Unit Cost</th>
                            <th scope="col" style="width: 15%;">Total Cost</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="2" style="width: 45%; padding-right: 5px; text-align: right;">Total:</td>
                            <td style="width: 15%;"></td>
                            <td style="width: 15%;"></td>
                            <td style="width: 15%;"></td>
                        </tr>
                    </tfoot>
                </table>
            </div>
            <div id="lblService" style="display: none;" class="pnlDetails panel-heading">Service Information</div>
            <div id="dvService" style="display: none;" class="pnlDetails panel-body">
                <table id="QuotationServicenformation" class="table table-condensed table-bordered table-responsive">
                    <thead>
                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                            <th style="width: 10%;">Service</th>
                            <th style="width: 25%;">Prduct Details</th>
                            <th style="width: 15%;">Package/s</th>
                            <th style="width: 10%;">Downlink</th>
                            <th style="width: 10%;">Uplink</th>
                            <th style="width: 15%;">Price/ Month</th>
                            <th style="width: 15%;">Price/ 3 Month</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="5" style="width: 45%; padding-right: 5px; text-align: right;">Total:</td>
                            <td style="width: 20%;"></td>
                            <td style="width: 20%;"></td>
                        </tr>
                    </tfoot>
                </table>
            </div>
            <div id="lblQuotaionBanquet" style="display: none;" class="pnlDetails panel-heading">Banquet Information</div>
            <div id="dvQuotaionBanquet" style="display: none;" class="pnlDetails panel-body">
                <table id="QuotationBanquetinformation" class="table table-condensed table-bordered table-responsive">
                    <thead>
                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                            <th style="width: 30%;">Outlet</th>
                            <th style="width: 30%;">Discount Head</th>
                            <th style="width: 10%;">Discount Type</th>
                            <th style="width: 15%;">DiscountAmount (Local)</th>
                            <th style="width: 15%;">Discount Amount (USD)</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
            <div id="lblQuotaionRestaurant" style="display: none;" class="pnlDetails panel-heading">Restaurant Information</div>
            <div id="dvQuotaionRestaurant" style="display: none;" class="pnlDetails panel-body">
                <table id="QuotationRestaurantinformation" class="table table-condensed table-bordered table-responsive">
                    <thead>
                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                            <th style="width: 30%;">Outlet</th>
                            <th style="width: 30%;">Classification</th>
                            <th style="width: 10%;">Discount Type</th>
                            <th style="width: 15%;">DiscountAmount (Local)</th>
                            <th style="width: 15%;">Discount Amount (USD)</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
            <div id="lblQuotaionRoom" style="display: none;" class="pnlDetails panel-heading">Guest Room Information</div>
            <div id="dvQuotaionRoom" style="display: none;" class="pnlDetails panel-body">
                <table id="QuotationRoominformation" class="table table-condensed table-bordered table-responsive">
                    <thead>
                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                            <th style="width: 60%;">Room Type</th>
                            <th style="width: 10%;">Discount Type</th>
                            <th style="width: 15%;">DiscountAmount (Local)</th>
                            <th style="width: 15%;">Discount Amount (USD)</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
            <div id="lblQuotaionServiceOutLet" style="display: none;" class="pnlDetails panel-heading">Service Outlet Information</div>
            <div id="dvQuotaionServiceOutLet" style="display: none;" class="pnlDetails panel-body">
                <table id="QuotationServiceOutLetinformation" class="table table-condensed table-bordered table-responsive">
                    <thead>
                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                            <th style="width: 60%;">Service Name</th>
                            <th style="width: 10%;">Discount Type</th>
                            <th style="width: 15%;">Discount Amount (Local)</th>
                            <th style="width: 15%;">Discount Amount (USD)</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
            <div class="panel panel-default" id="GLCompanyProjectDiv">
                <div class="pnlDetails panel-heading">GL Details</div>
                <div class="pnlDetails panel-body">
                    <UserControl:CompanyProjectUserControl ID="companyProjectUserControl" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <input type="button" class="btn btn-primary btn-sm" value="Approve" onclick="ApproveQuotation()" />
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Search Quotation
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Quotaion Number</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtQuotationNumber" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Company</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCompany" runat="server" class="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Proposal Date From</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDate" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Proposal Date To</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDate" runat="server" class="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <input type="button" class="btn btn-primary btn-sm" value="Search" onclick="SearchQuotation()" />
                        <input type="button" class="btn btn-primary btn-sm" value="Clear" />
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-heading">
            Search Information
        </div>
        <div class="panel-body">
            <table id="tblQuotation" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
            </table>
            <div class="childDivSection">
                <div class="text-center" id="GridPagingContainer">
                    <ul class="pagination">
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
