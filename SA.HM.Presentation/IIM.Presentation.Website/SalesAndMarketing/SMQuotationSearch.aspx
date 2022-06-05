<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="SMQuotationSearch.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.SMQuotationSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            CommonHelper.AutoSearchClientDataSource("txtSCompanySearch", "ContentPlaceHolder1_ddlSCompany", "ContentPlaceHolder1_hfSCompanyId");

            $('#txtSCompanySearch').blur(function () {
                if ($(this).val() != "") {
                    var cmpId = $("#ContentPlaceHolder1_hfSCompanyId").val();
                    if (cmpId != "") {
                        LoadSiteBySCompanyId(cmpId);
                    }
                }
            });

            $("#SearchPanel").hide();
            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });
            var dealId = +$.trim(CommonHelper.GetParameterByName("did"));
            $("#ContentPlaceHolder1_ddlDeal").val(dealId);
            $("#btnSearch").trigger('click');
            $(".pnlDetails").hide();
        });
        function LoadSiteBySCompanyId(companyId) {
            PageMethods.LoadCompanySite(companyId, OnLoadSiteBySCompanySucceeded, OnLoadSiteBySCompanyFailed);
        }
        function OnLoadSiteBySCompanySucceeded(result) {
            var list = result;
            var control = $('#ContentPlaceHolder1_ddlSCompanySite');

            control.empty();
            if (list != null) {
                if (list.length > 0) {

                    control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].SiteId + '">' + list[i].SiteName + '</option>');
                    }
                }
                else {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }

            // control.val($("#ContentPlaceHolder1_hfCompanySiteId").val());
            return false;
        }
        function OnLoadSiteBySCompanyFailed() { }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#gvSalesCallInformation tbody tr").length;
            var companyId = 0, dealId = 0;

            var companyName = $("#txtSCompanySearch").val();
            if (companyName == '') {
                companyId = '0';
            }
            else {
                companyId = $("#<%=ddlSCompany.ClientID %>").val();
            }
            dealId = $("#<%=ddlDeal.ClientID %>").val();

            PageMethods.GetQuotationForPagination(companyId, dealId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchSuccess, OnSearchFail);
            return false;
        }
        function OnSearchSuccess(result) {

            var format = innBoarDateFormat.replace('mm', 'MM');
            var format = format.replace('yy', 'yyyy');

            $("#gvSalesCallInformation tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"6\" >No Data Found</td> </tr>";
                $("#gvSalesCallInformation tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvSalesCallInformation tbody tr").length;

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:15%; \">" + gridObject.QuotationNo + "</td>";
                tr += "<td align='left' style=\"width:10%; \">" + GetStringFromDateTime(gridObject.ProposalDate) + "</td>";
                tr += "<td align='left' style=\"width:25%; \">" + gridObject.CompanyName + "</td>";
                //tr += "<td align='right' style=\"width:15%; cursor:pointer;\"><img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.QuotationId + "')\" alt='Edit Information' border='0' /></td>";
                tr += "<td align='center' style=\"width:15%; cursor:pointer;\">";
                if (gridObject.IsAccepted != 1) {
                    tr += "<img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.QuotationId + "','" + gridObject.QuotationNo + "')\" alt='Update Quotation' border='0' />";
                    tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return PerformApproval('" + gridObject.QuotationId + "','" + gridObject.DealId + "')\" alt='Approve'  title='Approve' border='0' />";
                    tr += "&nbsp;&nbsp;<img src='../Images/copy.png' onClick= \"javascript:return CopyQuotation('" + gridObject.QuotationId + "','" + gridObject.QuotationNo + "')\" title='Copy Quotaion' alt='Copy Quotaion' border='0' />";
                }
                else if (UserInfoFromDB.IsAdminUser)
                    tr += "&nbsp;&nbsp;<img src='../Images/cancel.png' onClick= \"javascript:return CancelQuotation('" + gridObject.QuotationId + "','" + gridObject.QuotationNo + "','" + gridObject.DealId + "')\" title='Canecl Quotaion' alt='Cancel Quotaion' border='0' />";
                tr += "&nbsp;&nbsp;<img src='../Images/detailsInfo.png' onClick= \"javascript:return ShowQuotationDetails(this,'" + gridObject.QuotationId + "')\" title='Quotaion Details' alt='Quotaion Details' border='0' />";
                tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png' onClick= \"javascript:return ShowQuotationInvoice('" + gridObject.QuotationId + "')\" title='Quotation Invoice' alt='Quotation Invoice' border='0' />";
                tr += "</td>";
                tr += "</tr>";

                $("#gvSalesCallInformation tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            return false;
        }
        function OnSearchFail(error) {
            toastr.error(error.get_message());
        }

        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }

        function ShowQuotationInvoice(quotationId) {

            var url = "/SalesAndMarketing/Reports/SMQuotationDetailsReport.aspx?id=" + quotationId + "&frmacc=1";
            var popup_window = "Print Preview";
            window.open(url, popup_window, "width=800,height=680,left=300,top=50,location=no,toolbar=no,menubar=no,resizable=yes, scrollbars=1");
        }

        function PerformEditAction(quotationId, QuotationNo) {
            if (!confirm("Do you want to edit - " + QuotationNo + "?")) {
                return false;
            }
            var iframeid = 'printDoc';
            var url = "./frmSMQuotation.aspx?Id=" + quotationId;
            //window.location = url;
            document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 1200,
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Update Quotation",
                show: 'slide'
            });
        }
        function CopyQuotation(quotationId, QuotationNo) {
            if (!confirm("Do you want to copy - " + QuotationNo + "?")) {
                return false;
            }
            var iframeid = 'printDoc';
            var url = "./frmSMQuotation.aspx?Id=" + quotationId + "&isCopy=1";
            //window.location = url;
            document.getElementById(iframeid).src = url;

            $("#displayBill").dialog({
                autoOpen: true,
                modal: true,
                width: 1200,
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Edit Quotation",
                show: 'slide'
            });
        }

        function CancelQuotation(quotationId, QuotationNo, dealId) {
            if (!confirm("Do you want to cancel - " + QuotationNo + "?")) {
                return false;
            }

            PageMethods.CancelQuotation(quotationId, dealId, OnLoadApprovalSucceed, OnLoadApprovalFailed);
            return false;
        }

        function PerformApproval(quotationId, dealId) {
            PageMethods.PerformApproval(quotationId, dealId, OnLoadApprovalSucceed, OnLoadApprovalFailed);
            return false;
        }
        function OnLoadApprovalSucceed(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                $(window.parent.document.getElementById("btnSearch")).trigger('click');
                GridPaging($("#GridPagingContainer").find("li.active").index(), 1);
                if (result.Data != null)
                    ViewInvoice(result.Data.DealId, result.Data.QuotationId);
            }
            else {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            return false;
        }
        function OnLoadApprovalFailed(error) {
            CommonHelper.AlertMessage(error.AlertMessage);
        }

        function ViewInvoice(dealId, quotationId) {
            var url = "";
            var popup_window = "Invoice Preview";

            url = "/SalesAndMarketing/Reports/SMQuotationDetailsReport.aspx?id=" + quotationId + "&did=" + dealId + "&frmacc=1";

            window.open(url, popup_window, "width=800,height=680,left=300,top=50,resizable=yes");
        }
        function OnLoadQuotationSucceeded(result) {

            $("#ContentPlaceHolder1_hfCompanySiteId").val(result.Quotation.SiteId);

            $("#ContentPlaceHolder1_hfQuotationId").val(result.Quotation.QuotationId);
            $("#ContentPlaceHolder1_hfQDealId").val(result.Quotation.QuotationId);
            $("#ContentPlaceHolder1_hfCompanyId").val(result.Quotation.CompanyId);

            $("#ContentPlaceHolder1_txtProposalDate").val(GetStringFromDateTime(result.Quotation.ProposalDate));
            $("#ContentPlaceHolder1_ddlServiceType").val(result.Quotation.ServiceTypeId);
            $("#ContentPlaceHolder1_ddlLocation").val(result.Quotation.LocationId);
            $("#ContentPlaceHolder1_ddlCompanySite").val(result.Quotation.SiteId);
            $("#ContentPlaceHolder1_txtDeviceOrUser").val(result.Quotation.TotalDeviceOrUser);
            $("#ContentPlaceHolder1_ddlContractPeriod").val(result.Quotation.ContractPeriodId);
            $("#ContentPlaceHolder1_ddlBillingPeriod").val(result.Quotation.BillingPeriodId);
            $("#ContentPlaceHolder1_ddlDeliveryBy").val(result.Quotation.ItemServiceDeliveryId);
            $("#ContentPlaceHolder1_ddlCurrentVendor").val(result.Quotation.CurrentVendorId);
            $("#ContentPlaceHolder1_txtRemarks").val(result.Quotation.Remarks);
            if (result.Quotation.CompanyId > 0) {
                LoadCompanyInfo(result.Quotation.CompanyId);
                LoadSiteByCompanyId(result.Quotation.CompanyId);
            }

            var tr = "", deleteLink = "<a href=\"javascript:void()\" onclick= 'DeleteItem(this)' ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";
            QuotationItemDetails = result.QuotationItemDetails;
            QuotationServiceDetails = result.QuotationServiceDetails;

            var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00;
            $.each(result.QuotationItemDetails, function (index, itm) {

                if ((index % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }
                tr += "<td align='left' style=\"display:none;\">" + itm.QuotationDetailsId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ItemId + "</td>";
                tr += "<td align='left' style=\"width:30%; text-align:Left;\">" + itm.ItemName + "</td>";
                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.HeadName + "</td>";

                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" +
                    " <input type=\"text\" disabled='disabled' id='itm" + itm.ItemId + "' value='" + itm.Quantity + "' class=\"form-control\" onblur='EditQuantity(this)' />" +
                    "</td>";

                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.UnitPrice + "</td>";
                tr += "<td align='left' style='width: 15%;'>" + itm.TotalPrice + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.StockBy + "</td>";
                tr += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

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

            if (ContactPeriod.length != 0) {
                var cp = _.findWhere(ContactPeriod, { ContractPeriodId: result.Quotation.ContractPeriodId });
                contractPeriodValue = cp.ContractPeriodValue;
            }

            serviceType = $("#ContentPlaceHolder1_ddlBandwidthServiceType option:selected").text();
            packageName = $("#ContentPlaceHolder1_ddlPackage option:selected").text();

            tr = ""; totalQuantity = 0.00; totalUnitCost = 0.00; totalCost = 0.00;
            $("#QuotationServicenformation thead tr:eq(0)").find("th:eq(6)").text("Price/ " + contractPeriodValue + " Month");

            $.each(result.QuotationServiceDetails, function (index, itm) {
                if ((index % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                deleteLink = "<a href=\"javascript:void()\" onclick= \"DeleteServiceItem(this," + itm.ItemId + "," + itm.ServicePackageId + "," + itm.ServiceTypeId + ")\" ><img alt=\"Delete\" src=\"../Images/delete.png\" /></a>";

                tr += "<td align='left' style=\"display:none;\">" + itm.QuotationDetailsId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ItemId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.StockById + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.Quantity + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ServicePackageId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ServiceBandWidthId + "</td>";
                tr += "<td align='left' style=\"display:none;\">" + itm.ServiceTypeId + "</td>";

                //tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + itm.ServiceType + "</td>";
                tr += "<td align='left' style=\"width:50%; text-align:Left;\">" + itm.ItemName + "</td>";
                //tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.PackageName + "</td>";
                //tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + itm.BandWidthName + "</td>";
                tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + itm.Quantity + "</td>";

                //tr += "<td align='left' style=\"width:10%; text-align:Left;\">" +
                //    " <input type=\"text\" disabled='disabled' id='itm" + itm.ItemId + "' value='" + itm.UpLink + "' class=\"form-control\" />" +
                //    "</td>";

                tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.UnitPrice + "</td>";
                //tr += "<td align='left' style='width: 15%;'>" + itm.TotalPrice + "</td>";
                tr += "<td align='left' style='width: 30%;'>" + (itm.Quantity * itm.UnitPrice) + "</td>";
                //tr += "<td align='center' style=\"width:10%; cursor:pointer;\">" + deleteLink + "</td>";

                tr += "</tr>";

                $("#QuotationServicenformation tbody").append(tr);

                totalQuantity = totalQuantity + itm.Quantity;
                totalUnitCost = totalUnitCost + itm.UnitPrice;
                //totalCost = totalCost + itm.TotalPrice;
                totalCost = totalCost + (itm.Quantity * itm.UnitPrice);
                tr = "";
            });

            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(1)").text(totalUnitCost);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(2)").text(totalCost);

            $("#myTabs").tabs({ active: 0 });

            return false;
        }
        function OnLoadQuotationFailed(error) {
            toastr.error(error);
        }
        function CloseDialog() {
            $("#displayBill").dialog('close');
        }

        function ShowQuotationDetails(row, quotationId) {
            $.each($(row).parent().parent().parent().find('tr'), function (index, gridObject) {

                if ((index % 2) == 0) {
                    $(gridObject).css("background-color", "#E3EAEB");
                }
                else {
                    $(gridObject).css("background-color", "#FFFFFF");
                }

            });

            $(row).parent().parent().css("background-color", "#e6ffe6");

            PageMethods.GetQuotationDetailById(quotationId, OnLoadQuotationSucceeded, OnLoadQuotationFailed);
            return false;
        }
        function OnLoadQuotationSucceeded(result) {
            $(".pnlDetails").show();
            $("#ContentPlaceHolder1_lblProposalDate").text(GetStringFromDateTime(result.Quotation.ProposalDate));
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
            if (result.QuotationItemDetails.length > 0) {
                $("#lblQuotationItem").show();
                $("#dvQuotationItem").show();
            }
            else {
                $("#lblQuotationItem").hide();
                $("#dvQuotationItem").hide();
            }
            $("#QuotationItemInformation tbody").html("");
            var totalCost = 0.00, totalQuantity = 0.00, totalUnitCost = 0.00;
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
            if (result.QuotationServiceDetails.length > 0) {
                $("#lblQuotationService").show();
                $("#dvQuotationService").show();
            }
            else {
                $("#lblQuotationService").hide();
                $("#dvQuotationService").hide();
            }
            $("#QuotationServicenformation tbody").html("");

            $.each(result.QuotationServiceDetails, function (index, itm) {
                if ((index % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }


                //tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + itm.ServiceType + "</td>";
                tr += "<td align='left' style=\"width:50%; text-align:Left;\">" + itm.ItemName + "</td>";
                //tr += "<td align='left' style=\"width:15%; text-align:Left;\">" + itm.PackageName + "</td>";
                //tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + itm.Downlink + "</td>";

                //tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + itm.Uplink + "</td>";
                tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + itm.Quantity + "</td>";

                tr += "<td align='left' style=\"width:10%; text-align:Left;\">" + itm.UnitPrice + "</td>";
                //tr += "<td align='left' style='width: 15%;'>" + itm.TotalPrice + "</td>";
                tr += "<td align='left' style='width: 30%;'>" + (itm.Quantity * itm.UnitPrice) + "</td>";

                tr += "</tr>";

                $("#QuotationServicenformation tbody").append(tr);

                totalQuantity = totalQuantity + itm.Quantity;
                totalUnitCost = totalUnitCost + itm.UnitPrice;
                //totalCost = totalCost + itm.TotalPrice;
                totalCost = totalCost + (itm.Quantity * itm.UnitPrice);
                tr = "";
            });

            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(1)").text(totalUnitCost);
            $("#QuotationServicenformation tfoot tr:eq(0)").find("td:eq(2)").text(totalCost);
            LoadDiscountItem(result.QuotationDetails);
            return false;
        }
        function OnLoadQuotationFailed(error) {
            toastr.error(error);
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
            
            _(QuotationDetails).each((val,index ) => {
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
                    OutLetName :"All",
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
                tr += "<td align='left' style=\"width:30%; text-align:Left;\">" + (itm.TypeName||itm.Type) + "</td>";
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
                
                tr += "<td align='left' style=\"width:30%; text-align:Left;\">" + (itm.TypeName ||itm.Type)+ "</td>";
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
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <div id="displayBill" style="display: none;">
        <iframe id="printDoc" name="printDoc" width="1200" height="700" style="overflow: hidden;"></iframe>
        <div id="bottomPrint">
        </div>
    </div>
    <div id="SearchEntry" class="panel panel-default" style="display: none;">
        <div class="panel-heading">Search Information</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblSCompany" runat="server" class="control-label" Text="Company Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <input id="txtSCompanySearch" type="text" class="form-control" />
                        <div style="display: none;">
                            <asp:DropDownList ID="ddlSCompany" TabIndex="1" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblStatus" runat="server" class="control-label" Text="Site"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSCompanySite" runat="server" TabIndex="1" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label24" runat="server" class="control-label" Text="Deal"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlDeal" runat="server" TabIndex="1" CssClass="form-control">
                            <%--<asp:ListItem Value="0">Please Select</asp:ListItem>--%>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <button type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm">
                            Search</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-md-6">
        <div id="SearchPanel" class="panel panel-default">
            <div class="panel-heading">Search Information</div>
            <div class="panel-body">
                <table id='gvSalesCallInformation' class="table table-bordered table-condensed table-responsive">
                    <thead>
                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                            <td style="width: 15%;">Quotation No
                            </td>
                            <td style="width: 10%;">Quotation Date
                            </td>
                            <td style="width: 25%;">Company Name
                            </td>
                            <td style="width: 15%;">Action
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <div class="childDivSection">
                    <div class="text-center" id="GridPagingContainer">
                        <ul class="pagination">
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-md-6">
        <div class="panel panel-default" style="height: 600px; overflow-y: scroll;">
            <div class="panel-heading">
                Quotation Details
            </div>
            <div class="panel-body">
            </div>
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
                        <tr id="DeviceNUser" runat="server">
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
                        <tr id="Delivery" runat="server">
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
            <div id="lblQuotationItem" class="pnlDetails panel-heading">Item Information</div>
            <div id="dvQuotationItem" class="pnlDetails panel-body">
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
            <div id="lblQuotationService" class="pnlDetails panel-heading">Service Information</div>
            <div id="dvQuotationService" class="pnlDetails panel-body">
                <table id="QuotationServicenformation" class="table table-condensed table-bordered table-responsive">
                    <thead>
                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                            <%--<th style="width: 10%;">Service</th>--%>
                            <th style="width: 50%;">Service Name</th>
                            <%--<th style="width: 15%;">Package/s</th>--%>
                            <%--<th style="/*width: 10%;*/">Downlink</th>--%>
                            <th style="width: 10%;">Quantity</th>
                            <%--<th style="width: 10%;">Uplink</th>--%>
                            <th style="width: 10%;">Unit Cost</th>
                            <th style="width: 30%;">Total Cost</th>
                            <%--<th style="width: 15%;">Price/ 3 Month</th>--%>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="2" style="width: 45%; padding-right: 5px; text-align: right;">Total:</td>
                            <td style="width: 20%;"></td>
                            <td style="width: 20%;"></td>
                        </tr>
                    </tfoot>
                </table>
            </div>
            <div id="lblQuotaionBanquet" style="display:none;" class="pnlDetails panel-heading">Banquet Information</div>
            <div id="dvQuotaionBanquet" style="display:none;" class="pnlDetails panel-body">
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
            <div id="lblQuotaionRestaurant" style="display:none;" class="pnlDetails panel-heading">Restaurant Information</div>
            <div id="dvQuotaionRestaurant" style="display:none;" class="pnlDetails panel-body">
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
            <div id="lblQuotaionRoom" style="display:none;" class="pnlDetails panel-heading">Guest Room Information</div>
            <div id="dvQuotaionRoom" style="display:none;" class="pnlDetails panel-body">
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
            <div id="lblQuotaionServiceOutLet" style="display:none;" class="pnlDetails panel-heading">Service Outlet Information</div>
            <div id="dvQuotaionServiceOutLet" style="display:none;" class="pnlDetails panel-body">
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
        </div>
    </div>

</asp:Content>
