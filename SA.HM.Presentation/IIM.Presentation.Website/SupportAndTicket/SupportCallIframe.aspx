<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="SupportCallIframe.aspx.cs" Inherits="HotelManagement.Presentation.Website.SupportAndTicket.SupportCallIframe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .CallSummaryDetailsType {
            float: left;
            margin: 6px;
            padding-left: 4px;
            display: block;
            border: 2px solid white;
        }
    </style>
    <script>
        var ClientSelected = null;
        var ItemSelected = null;
        var SupportItem = new Array();
        var SupportItemDeleted = new Array();

        var ItemSelectedForSupportDetails = null;
        var SupportItemForSupportDetails = new Array();
        var SupportItemDeletedForSupportDetails = new Array();
        var isClose;

        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;

        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;

            CommonHelper.ApplyDecimalValidation();

            $("#ContentPlaceHolder1_ddlSupportStage").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlSupportCategory").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlCase").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlCaseOwner").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlCategoryForSupport").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlSupportForwardTo").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlSupportPriority").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlSupportType").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $('#ContentPlaceHolder1_txtDeadLineDate').keypress(function (event) {
                event.preventDefault();
                return false;
            });

            $('#ContentPlaceHolder1_txtDeadLineDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: 0,
                dateFormat: innBoarDateFormat,
                minDate: 0
            }).datepicker("setDate", 0);

            $("#ContentPlaceHolder1_ddlSupportSource").change(function () {
                var Term = $(this).val();

                if (Term == "Other") {
                    $("#OtherSourceDiv").show();
                }
                else {
                    $("#OtherSourceDiv").hide();
                    $("#ContentPlaceHolder1_txtOtherSource").val('');
                }
            });

            $("#ContentPlaceHolder1_txtClientName").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SupportAndTicket/SupportCallIframe.aspx/ClientSearch',
                        data: JSON.stringify({ searchTerm: request.term }),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.CompanyName,
                                    value: m.CompanyId,
                                    CompanyId: m.CompanyId
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

                    ClientSelected = ui.item;

                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfClientId").val(ui.item.value);
                    LoadComapnyCallDetails(ui.item.value, 5);
                }
            });

            $("#ContentPlaceHolder1_txtItem").autocomplete({
                source: function (request, response) {
                    var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SupportAndTicket/SupportCallIframe.aspx/ItemSearch',
                        data: JSON.stringify({ searchTerm: request.term, categoryId: categoryId, IsCustomerItem: 1, IsSupplierItem: 1 }),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.Name,
                                    value: m.ItemId,
                                    ItemName: m.Name,
                                    ItemId: m.ItemId,
                                    CategoryId: m.CategoryId,
                                    ProductType: m.ProductType,
                                    StockBy: m.StockBy,
                                    UnitHead: m.UnitHead
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

                    ItemSelected = ui.item;

                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_txtCurrentStockBy").val(ui.item.UnitHead);
                }
            });
            $("#ContentPlaceHolder1_txtItemForSupport").autocomplete({
                source: function (request, response) {
                    var costCenterId = $("#ContentPlaceHolder1_hfCostcenterId").val();
                    var categoryId = $("#ContentPlaceHolder1_ddlCategoryForSupport").val();

                    var clientId = $("#ContentPlaceHolder1_hfClientId").val();
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SupportAndTicket/SupportCallIframe.aspx/ItemSearchWithClient',
                        data: JSON.stringify({ costCenterId: costCenterId, searchTerm: request.term, categoryId: categoryId, ClientId: clientId }),
                        dataType: "json",
                        async: false,
                        success: function (data) {
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
                                    UnitPrice: m.UnitPrice,
                                    VatAmount: m.VatAmount
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

                    ItemSelectedForSupportDetails = ui.item;

                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_txtUnitPriceForSupport").val(ui.item.UnitPrice);
                    $("#ContentPlaceHolder1_txtCurrentStockByForSupport").val(ui.item.UnitHead);
                    $("#ContentPlaceHolder1_txtUnitQuantityForSupport").val("1");
                    $("#txtVat").val(ui.item.VatAmount);
                }
            });

            var supportCallId = $.trim(CommonHelper.GetParameterByName("sc"));
            var supportCallType = $.trim(CommonHelper.GetParameterByName("sct"));
            var taskId = $.trim(CommonHelper.GetParameterByName("tid"));

            $("#ContentPlaceHolder1_hfSpportCallType").val(supportCallType);
            $("#ContentPlaceHolder1_hfTaskId").val(taskId);

            if ($("#ContentPlaceHolder1_hfSpportCallType").val() == "Details") {
                $("#SupportDetailsDiv").show();
            }
            else {
                $("#SupportDetailsDiv").hide();
            }

            if (supportCallId != "") {
                PerformEdit(supportCallId);
            }
            else {
                Clear();
            }
        });

        // Documents Div
        function UploadComplete() {
            var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var id = +$("#ContentPlaceHolder1_hfSupportId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();

            PageMethods.LoadContactDocument(id, randomId, deletedDoc, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }
        function OnLoadDocumentSucceeded(result) {
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

                guestDocumentTable += "<td align='left' style='width: 50%; cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + guestDoc[row].Name + "</td>";

                if (guestDoc[row].Path != "") {
                    if (guestDoc[row].Extention == ".jpg" || guestDoc[row].Extention == ".png" || guestDoc[row].Extention == ".PNG" || guestDoc[row].Extention == ".JPG")
                        imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    else
                        imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                guestDocumentTable += "<td align='left' style='width: 30%;cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + imagePath + "</td>";

                guestDocumentTable += "<td align='left' style='width: 20%'>";
                guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteGuestDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                guestDocumentTable += "</td>";
                guestDocumentTable += "</tr>";
            }
            guestDocumentTable += "</table>";
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

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
        }

        function LoadDocumentFromImplementation() {
            var id = +$("#ContentPlaceHolder1_hfTaskId").val();

            PageMethods.LoadImplementationDocument(id, OnLoadDocumentFromImplementationSucceeded, OnLoadDocumentFromImplementationFailed);
            return false;
        }

        function OnLoadDocumentFromImplementationSucceeded(result) {
            var guestDoc = result;
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";

            guestDocumentTable += "<table id='DocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            guestDocumentTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th></tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }

                guestDocumentTable += "<td align='left' style='width: 50%; cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + guestDoc[row].Name + "</td>";

                if (guestDoc[row].Path != "") {
                    if (guestDoc[row].Extention == ".jpg" || guestDoc[row].Extention == ".png" || guestDoc[row].Extention == ".PNG" || guestDoc[row].Extention == ".JPG")
                        imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    else
                        imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                guestDocumentTable += "<td align='left' style='width: 30%;cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + imagePath + "</td>";

                guestDocumentTable += "</tr>";
            }
            guestDocumentTable += "</table>";
            $("#DocumentInfo_ImplementationCenter").html(guestDocumentTable);
        }

        function OnLoadDocumentFromImplementationFailed(error) {
            toastr.error(error.get_message());
        }

        function LoadDocumentFromFeedback() {
            var id = +$("#ContentPlaceHolder1_hfTaskId").val();

            PageMethods.LoadFeedbackDocument(id, OnLoadDocumentFromFeedbackSucceeded, OnLoadDocumentFromFeedbackFailed);
            return false;
        }

        function OnLoadDocumentFromFeedbackSucceeded(result) {
            var guestDoc = result;
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";

            guestDocumentTable += "<table id='DocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            guestDocumentTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th></tr>";

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }

                guestDocumentTable += "<td align='left' style='width: 50%; cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + guestDoc[row].Name + "</td>";

                if (guestDoc[row].Path != "") {
                    if (guestDoc[row].Extention == ".jpg" || guestDoc[row].Extention == ".png" || guestDoc[row].Extention == ".PNG" || guestDoc[row].Extention == ".JPG")
                        imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    else
                        imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                guestDocumentTable += "<td align='left' style='width: 30%;cursor: pointer; cursor: hand;'><a javascript:void();' onclick= \"ShowDocument('" + result[row].Path + result[row].Name + "','" + result[row].Name + "');\">" + imagePath + "</td>";

                guestDocumentTable += "</tr>";
            }
            guestDocumentTable += "</table>";
            $("#DocumentInfo_Feedback").html(guestDocumentTable);
        }

        function OnLoadDocumentFromFeedbackFailed(error) {
            toastr.error(error.get_message());
        }

        function CloseDialog() {
            $("#DocumentDialouge").dialog('close');
            return false;
        }

        function AttachFile() {
            var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var path = "/SupportAndTicket/Images/";
            var category = "SupportAndTicketDoc";
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

        function PerformEdit(id) {
            //debugger;
            PageMethods.GetSupportById(id, OnSuccessLoading, OnFailLoading);
            return false;
        }

        function LoadComapnyCallDetails(companyId, totalDetails) {
            PageMethods.LoadComapnyCallDetails(companyId, totalDetails, OnSuccessLoadComapnyCallDetailsLoading, OnFailLoadComapnyCallDetailsLoading)
            return false;
        }

        function OnSuccessLoadComapnyCallDetailsLoading(result) {
            var subContent = "";
            for (var i = 0; i < result.length; i++) {
                subContent += "<div class='CallSummaryDetailsType col-md-12'>";
                subContent += "<div class='form-group'> <b>Ticket Number :</b> " + result[i].CaseNumber + "  </div>";
                subContent += "<div class='form-group'> <b>Case Name :</b> " + result[i].CaseName + "  </div>";
                subContent += "<div class='form-group'> <b>Case Details :</b> " + result[i].CaseDetails + "  </div>";
                subContent += "<div class='form-group'> <b>Ticket Date :</b> " + CommonHelper.DateFromDateTimeToDisplay(result[i].CreatedDate, innBoarDateFormat) + "  </div>";
                subContent += "<div class='form-group'> <b>Assigned To :</b> " + result[i].AssignedTo + "  </div>";
                if (result[i].CaseCloseByName != "") {
                    subContent += "<div class='form-group'> <b>Ticket Close By :</b> " + result[i].CaseCloseByName + "  </div>";
                }
                if (result[i].CaseCloseDateDisplay != "") {
                    subContent += "<div class='form-group'> <b>Ticket Close Date :</b> " + result[i].CaseCloseDateDisplay + "  </div>";
                }
                subContent += "</div>";
            }

            $("#CallDetailsDiv").html(subContent);

            return false;
        }

        function ShowCaseDetailsInformation(id) {
            PageMethods.GetSupportCaseInternalNotesDetailsInformationById(id, OnSuccessShowCaseDetailsInformationResult, OnFailLoading);
            return false;
        }

        function OnSuccessShowCaseDetailsInformationResult(result) {
            $("#CaseHistoryDetailsInfoDiv").html(result.InternalNotesDetails);
        }

        function OnFailLoadComapnyCallDetailsLoading(error) {
            toastr.error(error.get_message());
            return false;
        }

        function LoadEmployeeDetails() {
        }

        function OnSuccessLoading(result) {
            FillForm(result);
            return false;
        }

        function OnSuccessLoadingSupportCaseDetailsHistory(result) {
            var tr = "";
            debugger;
            for (var i = 0; i < result.length; i++) {
                tr += "<tr style='cursor: pointer; cursor: hand;' onclick= \"ShowCaseDetailsInformation('" + result[i].Id + "');\">";
                tr += "<td style='width:5%; cursor: pointer; cursor: hand;'>" + result[i].LogNumber + "</td>";
                tr += "<td style='width:45%; cursor: pointer; cursor: hand;''>" + result[i].ShortInternalNotesDetails + "</td>";
                tr += "<td style='width:30%; display: none;'>" + result[i].InternalNotesDetails + "</td>";
                tr += "</tr>";
            }

            $("#SupportCaseDetailsHistoryTbl tbody").prepend(tr);
        }

        function FillForm(Result) {
            Clear();

            $("#ContentPlaceHolder1_hfSupportId").val(Result.Id);
            $("#ContentPlaceHolder1_ddlCaseOwner").val(Result.CaseOwnerId).trigger('change');
            $("#ContentPlaceHolder1_ddlSupportCategory").val(Result.SupportCategoryId).trigger('change');
            $("#ContentPlaceHolder1_ddlSupportSource").val(Result.SupportSource).trigger('change');
            $("#ContentPlaceHolder1_ddlCase").val(Result.CaseId).trigger('change');
            $("#ContentPlaceHolder1_ddlSupportStage").val(Result.SupportStageId).trigger('change');
            $("#ContentPlaceHolder1_txtClientName").val(Result.CompanyNameWithCode);

            $("#ContentPlaceHolder1_hfClientId").val(Result.ClientId);
            LoadComapnyCallDetails(Result.ClientId, 5);
            LoadEmployeeDetails();

            $("#ContentPlaceHolder1_txtOtherSource").val(Result.SupportSourceOtherDetails);
            $("#ContentPlaceHolder1_txtCaseDeltails").val(Result.CaseDetails);
            $("#ContentPlaceHolder1_txtDisableCaseDeltails").val(Result.CaseDetails);
            $("#ContentPlaceHolder1_txtItemDetails").val(Result.ItemOrServiceDetails);

            var tr = "";
            for (var i = 0; i < Result.STSupportDetails.length; i++) {
                if (Result.STSupportDetails[i].Type == "Support") {
                    tr += "<tr>";
                    tr += "<td style='width:50%;'>" + Result.STSupportDetails[i].ItemName + "</td>";
                    tr += "<td style='width:30%;'>" + Result.STSupportDetails[i].HeadName + "</td>";
                    tr += "<td style='width:20%;'>";
                    tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                    tr += "</td>";
                    tr += "<td style='display:none;'>" + Result.STSupportDetails[i].ItemId + "</td>";
                    tr += "<td style='display:none;'>" + Result.STSupportDetails[i].CategoryId + "</td>";
                    tr += "<td style='display:none;'>" + Result.STSupportDetails[i].StockBy + "</td>";
                    tr += "<td style='display:none;'>" + Result.STSupportDetails[i].STSupportDetailsId + "</td>";
                    tr += "</tr>";

                    SupportItem.push({
                        ItemId: parseInt(Result.STSupportDetails[i].ItemId, 10),
                        CategoryId: parseInt(Result.STSupportDetails[i].CategoryId, 10),
                        StockBy: parseInt(Result.STSupportDetails[i].StockBy, 10),
                        Type: Result.STSupportDetails[i].Type,
                        STSupportDetailsId: Result.STSupportDetails[i].STSupportDetailsId,
                        STSupportId: Result.STSupportDetails[i].STSupportId
                    });
                }
            }

            $("#SupportItemTbl tbody").prepend(tr);
            //$("#ContentPlaceHolder1_txtInternalNotes").val(Result.InternalNotesDetails);
            $("#ContentPlaceHolder1_txtDisableInternalNotes").val(Result.InternalNotesDetails);

            if (Result.SupportTypeId != null)
                $("#ContentPlaceHolder1_ddlSupportType").val(Result.SupportTypeId).trigger('change');
            if (Result.SupportPriorityId != null)
                $("#ContentPlaceHolder1_ddlSupportPriority").val(Result.SupportPriorityId).trigger('change');
            if (Result.SupportForwardToId != null)
                $("#ContentPlaceHolder1_ddlSupportForwardTo").val(Result.SupportForwardToId).trigger('change');
            if (Result.BillConfirmation != null && Result.BillConfirmation != "")
                $("#ContentPlaceHolder1_ddlBillConfirmation").val(Result.BillConfirmation).trigger('change');
            if (Result.SupportDeadline != null)
                $("#ContentPlaceHolder1_txtDeadLineDate").val(GetStringFromDateTime(Result.SupportDeadline));

            tr = "";
            for (var i = 0; i < Result.STSupportDetails.length; i++) {
                if (Result.STSupportDetails[i].Type == "SupportDetails") {
                    tr += "<tr>";
                    tr += "<td style='width:35%;'>" + Result.STSupportDetails[i].ItemName + "</td>";
                    tr += "<td style='width:10%;'>" + Result.STSupportDetails[i].HeadName + "</td>";
                    tr += "<td style='width:10%;'>" +
                        "<input type='text' value='" + Result.STSupportDetails[i].UnitPrice + "' id='pp" + Result.STSupportDetails[i].ItemName + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoqDetails(this)'  />" +
                        "</td>";

                    tr += "<td style='width:15%;'>" +
                        "<input type='text' value='" + Result.STSupportDetails[i].UnitQuantity + "' id='pi" + Result.STSupportDetails[i].ItemName + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoqDetails(this)'  />" +
                        "</td>";
                    tr += "<td style='width:10%;'>" + (Result.STSupportDetails[i].VatAmount) + "</td>";
                    tr += "<td style='width:15%;'>" + ((parseFloat(Result.STSupportDetails[i].UnitPrice) * parseFloat(Result.STSupportDetails[i].UnitQuantity)) + Result.STSupportDetails[i].VatAmount).toFixed(2) + "</td>";

                    tr += "<td style='width:5%;'>";
                    tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItemForSupportDetails(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                    tr += "</td>";
                    tr += "<td style='display:none;'>" + Result.STSupportDetails[i].ItemId + "</td>";
                    tr += "<td style='display:none;'>" + Result.STSupportDetails[i].CategoryId + "</td>";
                    tr += "<td style='display:none;'>" + Result.STSupportDetails[i].StockBy + "</td>";
                    tr += "<td style='display:none;'>" + Result.STSupportDetails[i].STSupportDetailsId + "</td>";
                    tr += "<td style='display:none;'>" + Result.STSupportDetails[i].VatRate + "</td>";
                    tr += "</tr>";

                    CommonHelper.ApplyDecimalValidation();
                    SupportItemForSupportDetails.push({
                        ItemId: parseInt(Result.STSupportDetails[i].ItemId, 10),
                        CategoryId: parseInt(Result.STSupportDetails[i].CategoryId, 10),
                        StockBy: parseInt(Result.STSupportDetails[i].StockBy, 10),
                        UnitPrice: parseFloat(Result.STSupportDetails[i].UnitPrice),

                        UnitQuantity: parseFloat(Result.STSupportDetails[i].UnitQuantity),
                        VatRate: parseFloat(Result.STSupportDetails[i].VatRate),
                        VatAmount: parseFloat(Result.STSupportDetails[i].VatAmount),

                        TotalPrice: parseFloat((Result.STSupportDetails[i].UnitPrice * Result.STSupportDetails[i].UnitQuantity) + Result.STSupportDetails[i].VatAmount),

                        Type: Result.STSupportDetails[i].Type,
                        STSupportDetailsId: Result.STSupportDetails[i].STSupportDetailsId,
                        STSupportId: Result.STSupportDetails[i].STSupportId
                    });
                }
            }

            $("#SupportItemTblForSupport tbody").prepend(tr);
            CommonHelper.ApplyDecimalValidation();

            if (IsCanEdit) {
                $("#btnSave").val('Update').show();
                $("#DisableCaseDeltailsDiv").show();
                $("#CaseDeltailsDiv").hide();
            }
            else {
                $("#btnSave").hide();
                $("#CaseDeltailsDiv").show();
                $("#DisableCaseDeltailsDiv").hide();
            }

            UploadComplete();
            LoadDocumentFromImplementation();
            LoadDocumentFromFeedback();

            PageMethods.GetSupportCaseInternalNotesDetailsHistoryById(Result.Id, OnSuccessLoadingSupportCaseDetailsHistory, OnFailLoading);
            return false;
        }

        function OnFailLoading(error) {
            toastr.error(error.get_message());
            return false;
        }

        function AddItem() {
            if (ItemSelected == null) {
                toastr.warning("Select a Item.");
                $("#ContentPlaceHolder1_txtItem").focus();
                return false;
            }

            var ItemOBj = _.findWhere(SupportItem, { ItemId: parseInt(ItemSelected.value, 10) });
            if (ItemOBj != null) {
                toastr.warning("It's already added.");
                $("#ContentPlaceHolder1_txtItem").focus();
                return false;
            }

            var tr = "";
            tr += "<tr>";
            tr += "<td style='width:50%;'>" + ItemSelected.label + "</td>";
            tr += "<td style='width:30%;'>" + ItemSelected.UnitHead + "</td>";
            tr += "<td style='width:20%;'>";
            tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
            tr += "</td>";
            tr += "<td style='display:none;'>" + ItemSelected.value + "</td>";
            tr += "<td style='display:none;'>" + ItemSelected.CategoryId + "</td>";
            tr += "<td style='display:none;'>" + ItemSelected.StockBy + "</td>";
            tr += "<td style='display:none;'>" + 0 + "</td>";
            tr += "</tr>";

            $("#SupportItemTbl tbody").prepend(tr);
            tr = "";
            CommonHelper.ApplyDecimalValidation();
            SupportItem.push({
                ItemId: parseInt(ItemSelected.value, 10),
                CategoryId: parseInt(ItemSelected.CategoryId, 10),
                StockBy: parseInt(ItemSelected.StockBy, 10),
                Type: 'Support',
                STSupportDetailsId: 0,
                STSupportId: 0
            });

            ClearSupportItemContainer();
        }
        function AddItemForSupport() {
            debugger;
            if (ItemSelectedForSupportDetails == null) {
                toastr.warning("Select a Item.");
                $("#ContentPlaceHolder1_txtItemForSupport").focus();
                return false;
            }

            var ItemOBj = _.findWhere(SupportItemForSupportDetails, { ItemId: parseInt(ItemSelectedForSupportDetails.value, 10) });
            if (ItemOBj != null) {
                toastr.warning("It's already added.");
                $("#ContentPlaceHolder1_txtItemForSupport").focus();
                return false;
            }

            var UnitPriceForSupport = $("#ContentPlaceHolder1_txtUnitPriceForSupport").val();
            if (UnitPriceForSupport == "" || UnitPriceForSupport == "0") {
                toastr.warning("Unit Price Cannot Be Zero Or Empty.");
                $("#ContentPlaceHolder1_txtUnitPriceForSupport").focus();
                return false;
            }
            var UnitQuantityForSupport = $("#ContentPlaceHolder1_txtUnitQuantityForSupport").val();
            if (UnitQuantityForSupport == "" || UnitQuantityForSupport == "0") {
                toastr.warning("Unit Quantity Cannot Be Zero Or Empty.");
                $("#ContentPlaceHolder1_txtUnitQuantityForSupport").focus();
                return false;
            }
            
            var calculatedVatAmount = 0, calculatedTotalAmount = 0;
            var VatAmount = $("#txtVat").val();
            if (VatAmount > 0) {
                if ($("#ContentPlaceHolder1_ddlInclusiveOrExclusive").val() == "Exclusive") {
                    if ($("#ContentPlaceHolder1_cbTPVatAmount").is(":checked")) {
                        calculatedVatAmount = ((parseFloat(UnitPriceForSupport) * parseFloat(UnitQuantityForSupport))) * parseFloat(VatAmount / 100);
                        calculatedTotalAmount = ((parseFloat(UnitPriceForSupport) * parseFloat(UnitQuantityForSupport)) + calculatedVatAmount);
                    }
                    else {
                        calculatedVatAmount = 0;
                        calculatedTotalAmount = ((parseFloat(UnitPriceForSupport) * parseFloat(UnitQuantityForSupport)));
                    }
                }
                else {
                    calculatedVatAmount = parseFloat(((parseFloat(UnitPriceForSupport) * parseFloat(UnitQuantityForSupport))) * parseFloat(VatAmount / (100 + parseFloat(VatAmount))));
                    UnitPriceForSupport = ((parseFloat(UnitPriceForSupport) * parseFloat(UnitQuantityForSupport)) - calculatedVatAmount);
                    calculatedTotalAmount = ((parseFloat(UnitPriceForSupport) * parseFloat(UnitQuantityForSupport)) + calculatedVatAmount);
                }
            }
            else {
                calculatedVatAmount = 0;
                calculatedTotalAmount = ((parseFloat(UnitPriceForSupport) * parseFloat(UnitQuantityForSupport)));
            }

            var tr = "";
            tr += "<tr>";
            tr += "<td style='width:35%;'>" + ItemSelectedForSupportDetails.label + "</td>";
            tr += "<td style='width:10%;'>" + ItemSelectedForSupportDetails.UnitHead + "</td>";
            tr += "<td style='width:10%;'>" +
                "<input type='text' value='" + UnitPriceForSupport.toFixed(2) + "' id='pp" + ItemSelectedForSupportDetails.label + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoqDetails(this)'  />" +
                "</td>";
            tr += "<td style='width:15%;'>" +
                "<input type='text' value='" + UnitQuantityForSupport + "' id='pi" + ItemSelectedForSupportDetails.label + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoqDetails(this)'  />" +
                "</td>";
            tr += "<td style='width:10%;'>" + (calculatedVatAmount).toFixed(2) + "</td>";
            //tr += "<td style='width:15%;'>" + ((parseFloat(UnitPriceForSupport) * parseFloat(UnitQuantityForSupport)) + calculatedVatAmount).toFixed(2) + "</td>";
            tr += "<td style='width:15%;'>" + calculatedTotalAmount.toFixed(2) + "</td>";
            tr += "<td style='width:5%;'>";
            tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItemForSupportDetails(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
            tr += "</td>";
            tr += "<td style='display:none;'>" + ItemSelectedForSupportDetails.value + "</td>";
            tr += "<td style='display:none;'>" + ItemSelectedForSupportDetails.CategoryId + "</td>";
            tr += "<td style='display:none;'>" + ItemSelectedForSupportDetails.StockBy + "</td>";
            tr += "<td style='display:none;'>" + 0 + "</td>";
            tr += "<td style='display:none;'>" + (VatAmount) + "</td>";
            tr += "</tr>";

            $("#SupportItemTblForSupport tbody").prepend(tr);
            tr = "";
            CommonHelper.ApplyDecimalValidation();
            SupportItemForSupportDetails.push({
                ItemId: parseInt(ItemSelectedForSupportDetails.value, 10),
                CategoryId: parseInt(ItemSelectedForSupportDetails.CategoryId, 10),
                StockBy: parseInt(ItemSelectedForSupportDetails.StockBy, 10),
                UnitPrice: parseFloat(UnitPriceForSupport),
                UnitQuantity: parseFloat(UnitQuantityForSupport),
                //TotalPrice: parseFloat((UnitPriceForSupport * UnitQuantityForSupport)).toFixed(2),
                VatRate: (VatAmount),
                VatAmount: (calculatedVatAmount),
                TotalPrice: parseFloat(calculatedTotalAmount).toFixed(2),
                Type: 'SupportDetails',
                STSupportDetailsId: 0,
                STSupportId: 0
            });

            ClearSupportItemContainerForSupport();
            $("#ContentPlaceHolder1_txtItemForSupport").focus();
        }

        function CalculateTotalForAdhoqDetails(control) {
            debugger;
            var calculatedVatAmount = 0, calculatedTotalAmount = 0;
            var tr = $(control).parent().parent();

            var reAmount = $.trim($(tr).find("td:eq(2)").find("input").val());
            // debugger;

            if (reAmount == "" || reAmount == "0") {
                toastr.info("Amount Cannot Be Zero Or Empty.");
                return false;
            }

            var reQuantity = $.trim($(tr).find("td:eq(3)").find("input").val());
            // debugger;

            if (reQuantity == "" || reQuantity == "0") {
                toastr.info("Qantity Cannot Be Zero Or Empty.");
                return false;
            }

            var VatAmount = parseFloat($.trim($(tr).find("td:eq(11)").text()), 10);
            if (VatAmount > 0) {
                if ($("#ContentPlaceHolder1_ddlInclusiveOrExclusive").val() == "Exclusive") {
                    if ($("#ContentPlaceHolder1_cbTPVatAmount").is(":checked")) {
                        calculatedVatAmount = ((parseFloat(reAmount) * parseFloat(reQuantity))) * parseFloat(VatAmount / 100);
                        calculatedTotalAmount = ((parseFloat(reAmount) * parseFloat(reQuantity)) + calculatedVatAmount);
                    }
                    else {
                        calculatedVatAmount = 0;
                        calculatedTotalAmount = ((parseFloat(reAmount) * parseFloat(reQuantity)));
                    }
                }
                else {
                    calculatedVatAmount = parseFloat(((parseFloat(reAmount) * parseFloat(reQuantity))) * parseFloat(VatAmount / (100 + parseFloat(VatAmount))));
                    UnitPriceForSupport = ((parseFloat(reAmount) * parseFloat(reQuantity)) - calculatedVatAmount);
                    calculatedTotalAmount = ((parseFloat(reAmount) * parseFloat(reQuantity)) + calculatedVatAmount);
                }
            }
            else {
                calculatedVatAmount = 0;
                calculatedTotalAmount = ((parseFloat(reAmount) * parseFloat(reQuantity)));
            }

            $(tr).find("td:eq(4)").text(calculatedVatAmount.toFixed(2));
            $(tr).find("td:eq(5)").text(calculatedTotalAmount.toFixed(2));
            //$(tr).find("td:eq(5)").text((parseFloat(reAmount) * parseFloat(reQuantity)).toFixed(2));

            var itemId = parseInt($.trim($(tr).find("td:eq(7)").text()), 10);
            var ItemIdObj = _.findWhere(SupportItemForSupportDetails, { ItemId: itemId });
            var index = _.indexOf(SupportItemForSupportDetails, ItemIdObj);

            SupportItemForSupportDetails[index].UnitPrice = parseFloat(reAmount);
            SupportItemForSupportDetails[index].UnitQuantity = parseFloat(reQuantity);
            SupportItemForSupportDetails[index].VatRate = parseFloat(VatAmount);
            SupportItemForSupportDetails[index].VatAmount = parseFloat(calculatedVatAmount);
            SupportItemForSupportDetails[index].TotalPrice = calculatedTotalAmount; //(parseFloat(reAmount) * parseFloat(reQuantity)).toFixed(2);

        }

        function CalculateTotalForAdhoq(control) {
            //debugger;
            var tr = $(control).parent().parent();

            var reAmount = $.trim($(tr).find("td:eq(2)").find("input").val());
            // debugger;

            if (reAmount == "" || reAmount == "0") {
                toastr.info("Amount Cannot Be Zero Or Empty.");
                return false;
            }
            var itemId = parseInt($.trim($(tr).find("td:eq(5)").text()), 10);
            var ItemIdObj = _.findWhere(SupportItemForSupportDetails, { ItemId: itemId });
            var index = _.indexOf(SupportItemForSupportDetails, ItemIdObj);

            SupportItemForSupportDetails[index].UnitPrice = parseFloat(reAmount);

        }

        function SaveNClose() {
            //debugger;
            isClose = true;
            //SaveOrUpdateTask();
            $.when(SaveOrUpdateSupport()).done(function () {
                if (isClose) {
                    if (typeof parent.CloseDialog === "function") {
                        parent.CloseDialog();
                    }
                }
            });
            return false;
        }

        function SaveOrUpdateSupport() {
            //debugger;
            var id = +$("#ContentPlaceHolder1_hfSupportId").val();
            var caseOwnerId = $("#ContentPlaceHolder1_ddlCaseOwner").val();
            if (caseOwnerId == "0") {
                isClose = false;
                toastr.warning("Please Select Ticket Owner.");
                $("#ContentPlaceHolder1_ddlCaseOwner").focus();
                return false;
            }

            var clientId = $("#ContentPlaceHolder1_hfClientId").val();
            if (clientId == "0") {
                isClose = false;
                toastr.warning("Select a client.");
                $("#ContentPlaceHolder1_txtClientName").focus();
                return false;
            }

            var supportCategoryId = $("#ContentPlaceHolder1_ddlSupportCategory").val();
            if (supportCategoryId == "0") {
                isClose = false;
                toastr.warning("Select a support category.");
                $("#ContentPlaceHolder1_ddlSupportCategory").focus();
                return false;
            }

            var supportSource = $("#ContentPlaceHolder1_ddlSupportSource").val();
            var supportSourceOtherDetails = $("#ContentPlaceHolder1_txtOtherSource").val();

            if (supportSource == "Other") {
                if (supportSourceOtherDetails == "") {
                    isClose = false;
                    toastr.warning("Write Other Source Details.");
                    $("#ContentPlaceHolder1_txtOtherSource").focus();
                    return false;
                }
            }

            var caseId = $("#ContentPlaceHolder1_ddlCase").val();
            if (caseId == "0") {
                isClose = false;
                toastr.warning("Please Select Case.");
                $("#ContentPlaceHolder1_ddlCase").focus();
                return false;
            }

            var caseDeltails = $("#ContentPlaceHolder1_txtCaseDeltails").val();
            var itemDeltails = $("#ContentPlaceHolder1_txtItemDetails").val();
            var supportStageId = $("#ContentPlaceHolder1_ddlSupportStage").val();
            if (supportStageId == "0") {
                isClose = false;
                toastr.warning("Please Select Support Stage.");
                $("#ContentPlaceHolder1_ddlSupportStage").focus();
                return false;
            }

            var internalNotesDeltails, supportTypeId, supportPriorityId, billConfirmation, deadLineDate, supportForwardToId;

            deadLineDate = $("#ContentPlaceHolder1_txtDeadLineDate").val();
            var supportCallType = $("#ContentPlaceHolder1_hfSpportCallType").val();

            if (supportCallType == "Details") {
                internalNotesDeltails = $("#ContentPlaceHolder1_txtInternalNotes").val();
                supportTypeId = $("#ContentPlaceHolder1_ddlSupportType").val();
                if (supportTypeId == "0") {
                    isClose = false;
                    toastr.warning("Please Select Support Type.");
                    $("#ContentPlaceHolder1_ddlSupportType").focus();
                    return false;
                }

                supportPriorityId = $("#ContentPlaceHolder1_ddlSupportPriority").val();
                if (supportPriorityId == "0") {
                    isClose = false;
                    toastr.warning("Please Select Support Priority.");
                    $("#ContentPlaceHolder1_ddlSupportPriority").focus();
                    return false;
                }

                supportForwardToId = $("#ContentPlaceHolder1_ddlSupportForwardTo").val();
                //if (supportForwardToId == "0") {
                //    isClose = false;
                //    toastr.warning("Select a support forward to.");
                //    $("#ContentPlaceHolder1_ddlSupportForwardTo").focus();
                //    return false;
                //}

                billConfirmation = $("#ContentPlaceHolder1_ddlBillConfirmation").val();
                if (billConfirmation == "") {
                    isClose = false;
                    toastr.warning("Please Select Bill Confirmation.");
                    $("#ContentPlaceHolder1_ddlBillConfirmation").focus();
                    return false;
                }

                if (deadLineDate == "") {
                    isClose = false;
                    toastr.warning("Please Select Deadline");
                    $("#ContentPlaceHolder1_txtDeadLineDate").focus();
                    return false;
                }

                deadLineDate = CommonHelper.DateFormatToMMDDYYYY(deadLineDate, '/');

                var itemCountForSupportDetails = SupportItemForSupportDetails.length;
                //if (itemCountForSupportDetails == 0) {
                //    isClose = false;
                //    toastr.warning("Add some item or service for support details.");
                //    $("#ContentPlaceHolder1_ddlCategoryForSupport").focus();
                //    return false;
                //}
            }

            var Support = {
                Id: id,
                CaseOwnerId: caseOwnerId,
                ClientId: clientId,
                SupportCategoryId: supportCategoryId,
                SupportSource: supportSource,
                SupportSourceOtherDetails: supportSourceOtherDetails,
                CaseId: caseId,
                CaseDetails: caseDeltails,
                ItemOrServiceDetails: itemDeltails,
                SupportStageId: supportStageId,
                InternalNotesDetails: internalNotesDeltails,
                SupportTypeId: supportTypeId,
                SupportPriorityId: supportPriorityId,
                SupportForwardToId: supportForwardToId,
                SupportDeadline: deadLineDate,
                BillConfirmation: billConfirmation
            }

            var hfRandom = $("#<%=RandomProductId.ClientID %>").val();
            var deletedDocuments = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();

            PageMethods.SaveOrUpdateSupport(Support, SupportItem, SupportItemDeleted, SupportItemForSupportDetails, SupportItemDeletedForSupportDetails, supportCallType, hfRandom, deletedDocuments, OnSuccessSaveOrUpdate, OnFailSaveOrUpdate);
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
        function DeleteAdhoqItem(control) {
            if (!confirm("Do you want to delete?")) { return false; }
            var tr = $(control).parent().parent();
            var itemId = parseInt($.trim($(tr).find("td:eq(7)").text()), 10);
            var detailsId = parseInt($.trim($(tr).find("td:eq(10)").text()), 10);

            var ItemOBj = _.findWhere(SupportItem, { ItemId: itemId });
            var index = _.indexOf(SupportItem, ItemOBj);

            if (parseInt(detailsId, 10) > 0)
                SupportItemDeleted.push(JSON.parse(JSON.stringify(ItemOBj)));

            SupportItem.splice(index, 1);
            $(tr).remove();
        }

        function DeleteAdhoqItemForSupportDetails(control) {
            if (!confirm("Do you want to delete?")) { return false; }
            var tr = $(control).parent().parent();
            var itemId = parseInt($.trim($(tr).find("td:eq(7)").text()), 10);
            var detailsId = parseInt($.trim($(tr).find("td:eq(10)").text()), 10);

            var ItemOBj = _.findWhere(SupportItemForSupportDetails, { ItemId: itemId });
            var index = _.indexOf(SupportItemForSupportDetails, ItemOBj);

            if (parseInt(detailsId, 10) > 0)
                SupportItemDeletedForSupportDetails.push(JSON.parse(JSON.stringify(ItemOBj)));

            SupportItemForSupportDetails.splice(index, 1);
            $(tr).remove();
        }

        function ClearSupportItemContainer() {
            $("#ContentPlaceHolder1_ddlCategory").val("0").change();
            $("#ContentPlaceHolder1_txtItem").val("");
            $("#ContentPlaceHolder1_txtCurrentStockBy").val("");
            $("#btnAdd").val("Add");
            ItemSelected = null;
        }

        function ClearSupportItemContainerForSupport() {
            $("#ContentPlaceHolder1_ddlCategoryForSupport").val("0").change();
            $("#ContentPlaceHolder1_txtItemForSupport").val("");
            $("#ContentPlaceHolder1_txtCurrentStockByForSupport").val("");
            $("#ContentPlaceHolder1_txtUnitPriceForSupport").val("");
            $("#ContentPlaceHolder1_txtUnitQuantityForSupport").val("");
            $("#btnAdd").val("Add");

            ItemSelectedForSupportDetails = null;
        }

        function Clear() {
            $("#ContentPlaceHolder1_ddlSupportCategory").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlSupportSource").val("Phone").trigger('change');
            $("#ContentPlaceHolder1_ddlCase").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlSupportStage").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtClientName").val('');
            $("#ContentPlaceHolder1_txtOtherSource").val('');
            $("#ContentPlaceHolder1_txtCaseDeltails").val('');
            $("#ContentPlaceHolder1_txtItemDetails").val('');
            $("#ContentPlaceHolder1_hfClientId").val("0");
            $("#ContentPlaceHolder1_hfSupportId").val("0");
            ClearSupportItemContainer();
            if (IsCanSave) {
                $("#btnSave").val('Save').show();
            }
            else {
                $("#btnSave").hide();
            }
            isClose = false;
            return false;
        }
    </script>
    <asp:HiddenField ID="hfSpportCallType" runat="server" Value="" />
    <asp:HiddenField ID="hfTaskId" runat="server" Value="0" />
    <asp:HiddenField ID="hfClientId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfSupportId" Value="0" runat="server" />
    <asp:HiddenField ID="hfCostcenterId" runat="server" />
    <asp:HiddenField ID="hfIsVatEnable" runat="server" />
    <asp:HiddenField ID="hfRestaurantVatAmount" runat="server" />
    <asp:HiddenField ID="hfIsRestaurantBillInclusive" runat="server" />
    <div>
        <div style="padding: 10px 30px 10px 30px">
            <div id="SupportInformationNCompanyNKnowledgeDiv">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-8" id="leftSideDiv">
                            <div id="SupportInformationDiv" class="panel panel-default ">
                                <div class="panel-heading">
                                    Ticket Information
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <label class="control-label required-field">Ticket Owner</label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:DropDownList runat="server" ID="ddlCaseOwner" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <label class="control-label required-field">Client Name</label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:TextBox ID="txtClientName" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <label class="control-label required-field">Category</label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList runat="server" ID="ddlSupportCategory" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <label class="control-label required-field">Source</label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList runat="server" ID="ddlSupportSource" CssClass="form-control">
                                                    <asp:ListItem Text="Phone" Value="Phone"></asp:ListItem>
                                                    <asp:ListItem Text="Mail" Value="Mail"></asp:ListItem>
                                                    <asp:ListItem Text="SMS" Value="SMS"></asp:ListItem>
                                                    <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div id="OtherSourceDiv" style="display: none;">
                                            <div class="form-group">
                                                <div class="col-md-2">
                                                    <label class="control-label required-field">Other Source</label>
                                                </div>
                                                <div class="col-md-10">
                                                    <asp:TextBox ID="txtOtherSource" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <label class="control-label required-field">Case</label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:DropDownList runat="server" ID="ddlCase" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Case Deltails"></asp:Label>
                                            </div>
                                            <div class="col-md-10" style="display: none;" id="DisableCaseDeltailsDiv">
                                                <asp:TextBox ID="txtDisableCaseDeltails" runat="server" Enabled="false" Height="250px" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-10" id="CaseDeltailsDiv">
                                                <asp:TextBox ID="txtCaseDeltails" runat="server" Height="250px" placeholder="Case Deltails" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="panel panel-default" style="display: none;">
                                            <div class="panel-heading">Item/Service Information</div>
                                            <div class="panel-body">
                                                <div class="form-horizontal">
                                                    <div class="form-group">
                                                        <div class="col-md-2">
                                                            <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category"></asp:Label>
                                                        </div>
                                                        <div class="col-md-10">
                                                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="col-md-2">
                                                            <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Item"></asp:Label>
                                                        </div>
                                                        <div class="col-md-10">
                                                            <asp:TextBox ID="txtItem" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="col-md-2">
                                                            <asp:Label ID="Label6" runat="server" class="control-label" Text="Stock Unit"></asp:Label>
                                                        </div>
                                                        <div class="col-md-10">
                                                            <asp:TextBox ID="txtCurrentStockBy" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group" style="padding-top: 10px;">
                                                        <div class="col-md-12">
                                                            <input id="btnAdd" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" onclick="AddItem()" />
                                                            <input id="btnCancelOrder" type="button" value="Cancel" onclick="ClearSupportItemContainer()"
                                                                class="TransactionalButton btn btn-primary btn-sm" />
                                                        </div>
                                                    </div>
                                                    <div style="height: 250px; overflow-y: scroll;">
                                                        <table id="SupportItemTbl" class="table table-bordered table-condensed table-hover">
                                                            <thead>
                                                                <tr>
                                                                    <th style="width: 50%;">Item</th>
                                                                    <th style="width: 30%;">Stock Unit</th>
                                                                    <th style="width: 20%;">Action</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody></tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group" style="display: none;">
                                            <div class="col-md-2">
                                                <label class="control-label">Item/Service Details</label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:TextBox ID="txtItemDetails" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <label class="control-label required-field">Support Stage</label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:DropDownList runat="server" ID="ddlSupportStage" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div id="SupportDetailsDiv">
                                            <div class="panel panel-default">
                                                <div class="panel-heading">Item/Service Information For Ticket Details</div>
                                                <div class="panel-body">
                                                    <div class="form-horizontal">
                                                        <div class="form-group">
                                                            <div class="col-md-2">
                                                                <asp:Label ID="lblCategoryForSupport" runat="server" class="control-label" Text="Category"></asp:Label>
                                                            </div>
                                                            <div class="col-md-10">
                                                                <asp:DropDownList ID="ddlCategoryForSupport" runat="server" CssClass="form-control">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-md-2">
                                                                <asp:Label ID="lblItemForSupport" runat="server" class="control-label required-field" Text="Item"></asp:Label>
                                                            </div>
                                                            <div class="col-md-10">
                                                                <asp:TextBox ID="txtItemForSupport" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-md-2">
                                                                <asp:Label ID="lblCurrentStockByForSupport" runat="server" class="control-label" Text="Stock Unit"></asp:Label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtCurrentStockByForSupport" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <label for="Vat">
                                                                    Vat Amount</label>
                                                           <%-- </div>
                                                            <div class="col-md-1">--%>
                                                                <asp:CheckBox ID="cbTPVatAmount" runat="server" onclick="javascript: return IsVatEnableCheckOrUncheck(this);" />
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:DropDownList ID="ddlInclusiveOrExclusive" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Value="Inclusive">Inc.</asp:ListItem>
                                                                    <asp:ListItem Value="Exclusive">Exc.</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <input type="text" class="form-control" id="txtVat" placeholder="Vat Amount" disabled="disabled" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-md-2">
                                                                <asp:Label ID="lblUnitQuantityForSupport" runat="server" class="control-label required-field" Text="Unit Quantity"></asp:Label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtUnitQuantityForSupport" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:Label ID="lblUnitPriceForSupport" runat="server" class="control-label required-field" Text="Unit Price"></asp:Label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtUnitPriceForSupport" runat="server" CssClass="form-control quantitydecimal"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group" style="padding-top: 10px;">
                                                            <div class="col-md-12">
                                                                <input id="btnAddForSupport" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" onclick="AddItemForSupport()" />
                                                                <input id="btnCancelOrderForSupport" type="button" value="Cancel" onclick="ClearSupportItemContainerForSupport()"
                                                                    class="TransactionalButton btn btn-primary btn-sm" />
                                                            </div>
                                                        </div>
                                                        <div style="height: 250px; overflow-y: scroll;">
                                                            <table id="SupportItemTblForSupport" class="table table-bordered table-condensed table-hover">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="width: 35%;">Item</th>
                                                                        <th style="width: 10%;">Stock Unit</th>
                                                                        <th style="width: 10%;">Unit Price</th>
                                                                        <th style="width: 15%;">Unit Quantity</th>
                                                                        <th style="width: 10%;">Vat</th>
                                                                        <th style="width: 15%;">Total Price</th>
                                                                        <th style="width: 5%;">Action</th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody></tbody>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="form-group">
                                                    <div class="col-md-4" style="height: 300px; overflow-y: scroll;">
                                                        <table id="SupportCaseDetailsHistoryTbl" class="table table-bordered table-condensed table-hover">
                                                            <thead>
                                                                <tr>
                                                                    <th style="width: 5%;">#</th>
                                                                    <th style="width: 45%;">Internal Notes History</th>
                                                                    <th style="width: 30%; display: none;">Internal Notes</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody></tbody>
                                                        </table>
                                                    </div>
                                                    <div class="col-md-8">
                                                        <div>
                                                            <asp:TextBox ID="txtDisableInternalNotes" Enabled="false" runat="server" Height="150px" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div>
                                                            <asp:TextBox ID="txtInternalNotes" runat="server" Height="150px" placeholder="Internal Notes" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <label class="control-label required-field">Support Type</label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList runat="server" ID="ddlSupportType" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2 ">
                                                <label class="control-label required-field">Deadline</label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtDeadLineDate" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <label class="control-label required-field">Support Priority</label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList runat="server" ID="ddlSupportPriority" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <label class="control-label">Bill Confirmation</label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList runat="server" ID="ddlBillConfirmation" CssClass="form-control">
                                                    <asp:ListItem Text="No" Value="NO"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="YES"></asp:ListItem>
                                                    <asp:ListItem Text="TBA" Value="TBA"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group" style="display: none;">
                                            <div class="col-md-2">
                                                <label class="control-label ">Forward To</label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:DropDownList runat="server" ID="ddlSupportForwardTo" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:HiddenField ID="RandomProductId" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="hfGuestDeletedDoc" runat="server" Value=""></asp:HiddenField>
                                            <div class="col-md-2">
                                                <label class="control-label">Attachment</label>
                                            </div>
                                            <div class="col-md-10">
                                                <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Attach" onclick="AttachFile()" />
                                            </div>
                                        </div>
                                        <div id="DocumentInfo">
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <label class="control-label">Attachment (Implementation Center)</label>
                                            </div>

                                        </div>
                                        <div id="DocumentInfo_ImplementationCenter">
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <label class="control-label">Attachment (Feedback)</label>
                                            </div>
                                        </div>
                                        <div id="DocumentInfo_Feedback">
                                        </div>
                                        <div id="DocumentDialouge" style="display: none;">
                                            <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
                                                clientidmode="static" scrolling="yes"></iframe>
                                        </div>
                                        <div id="ShowDocumentDiv" style="display: none;">
                                            <iframe id="fileIframe" name="IframeName" width="100%" height="100%" runat="server"
                                                clientidmode="static" scrolling="yes"></iframe>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                                    <div class="col-md-12">
                                        <input id="btnSave" type="button" class="TransactionalButton btn btn-primary btn-sm"
                                            onclick="SaveNClose()" value="Save" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4" id="rightSideDiv">
                        <div id="CompanyDiv" class="panel panel-default " style="height: 960px; overflow-y: scroll">
                            <div class="panel-heading">
                                Last Ticket Details
                            </div>
                            <div class="panel-body">
                                <div id="CallDetailsDiv">
                                </div>
                            </div>
                        </div>
                        <div id="CaseHistoryDetailDiv" class="panel panel-default " style="height: 300px; overflow-y: scroll">
                            <div class="panel-heading">
                                Internal Note Details
                            </div>
                            <div class="panel-body">
                                <div id="CaseHistoryDetailsInfoDiv">
                                </div>
                            </div>
                        </div>
                        <div id="KnowledgeDiv" class="panel panel-default " style="height: 400px; display: none;">
                            <div class="panel-heading">
                                Knowledge Information
                            </div>
                            <div class="panel-body">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </div>
</asp:Content>
