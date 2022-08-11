<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="SupportCallDetailsIframe.aspx.cs" Inherits="HotelManagement.Presentation.Website.SupportAndTicket.SupportCallDetailsIframe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

            $("#txtSearchBank").val("");

            $("#ContentPlaceHolder1_hfPaymentId").val("");
            $("#txtBranchName").val("");
            $("#txtAccountName").val("");
            $("#txtAccountNumber").val("");
            $("#txtAccountType").val("");



            $("#ContentPlaceHolder1_hfContactId").val("");
            $("#txtSearchContact").val("");
            $("#txtDesignation").val("");
            $("#txtDepartment").val("");

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
            
            $("#txtSearchContact").autocomplete({
                source: function (request, response) {
                    var companyId = $("#ContentPlaceHolder1_hfClientId").val() == "" ? 0 : +$("#ContentPlaceHolder1_hfClientId").val();
                    if (companyId == 0) {
                        $("#txtSearchContact").val("");
                        toastr.warning("Select A Company First.");
                        $("#txtEstimatedTaskDoneDate").focus();
                        return false;
                    }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: 'SupportCallDetailsIframe.aspx/GetContactInfoForAutoComplete',
                        data: "{'contactName':'" + request.term + "', 'companyId':'" + companyId + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    JobTitle: m.JobTitle,
                                    Department: m.Department,
                                    label: m.Name,
                                    value: m.Id
                                };
                            });
                            response(searchData);
                        },
                        error: function (xhr, err) {
                            toastr.error(xhr.responseText);
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
                    $(this).val(ui.item.label);

                    $("#ContentPlaceHolder1_hfContactId").val(ui.item.value);
                    $("#txtDesignation").val(ui.item.JobTitle);
                    $("#txtDepartment").val(ui.item.Department);
                }
            });

            $("#txtSearchBank").autocomplete({
                source: function (request, response) {
                    var companyId = $("#ContentPlaceHolder1_hfClientId").val() == "" ? 0 : +$("#ContentPlaceHolder1_hfClientId").val();
                    if (companyId == 0) {
                        $("#txtSearchBank").val("");
                        toastr.warning("Select A Company First.");
                        $("#txtEstimatedTaskDoneDate").focus();
                        return false;

                    }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: 'SupportCallDetailsIframe.aspx/GetBankInfoForAutoComplete',
                        data: "{'bankName':'" + request.term + "'}",
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    AccountName: m.AccountName,
                                    BranchName: m.BranchName,
                                    AccountNumber: m.AccountNumber,
                                    AccountType: m.AccountType,
                                    Description: m.Description,
                                    label: m.BankName,
                                    value: m.BankId
                                };
                            });
                            response(searchData);
                        },
                        error: function (xhr, err) {
                            toastr.error(xhr.responseText);
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
                    $(this).val(ui.item.label);

                    $("#ContentPlaceHolder1_hfPaymentId").val(ui.item.value);
                    $("#txtBranchName").val(ui.item.BranchName);
                    $("#txtAccountName").val(ui.item.AccountName);
                    $("#txtAccountNumber").val(ui.item.AccountNumber);
                    $("#txtAccountType").val(ui.item.AccountType);
                }
            });

            $("#ContentPlaceHolder1_txtClientName").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SupportAndTicket/SupportCallDetailsIframe.aspx/ClientSearch',
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
                    var categoryId = $("#ContentPlaceHolder1_ddlCategoryForSupport").val();

                    var clientId = $("#ContentPlaceHolder1_hfClientId").val();
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SupportAndTicket/SupportCallIframe.aspx/ItemSearchWithClient',
                        data: JSON.stringify({ searchTerm: request.term, categoryId: categoryId, IsCustomerItem: 1, IsSupplierItem: 1, ClientId: clientId }),
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
                                    UnitPrice: m.UnitPrice
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

                }
            });

            var supportCallId = $.trim(CommonHelper.GetParameterByName("sc"));
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
                //guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteGuestDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
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

        function CloseDialog() {
            $("#DocumentDialouge").dialog('close');
            return false;
        }

        function AttachFile() {
            var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
            var path = "/SupportAndTicket/Images/SupportAndTicket/";
            var category = "SupportAndTicketDoc";
            var iframeid = 'frmPrint';
            
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
            PageMethods.GetSupportById(id, OnSuccessLoading, OnFailLoading);

            return false;
        }

        function OnSuccessLoading(result) {
            FillForm(result);
            return false;
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
            $("#ContentPlaceHolder1_txtOtherSource").val(Result.SupportSourceOtherDetails);
            $("#ContentPlaceHolder1_txtCaseDeltails").val(Result.CaseDetails);
            $("#ContentPlaceHolder1_txtItemDetails").val(Result.ItemOrServiceDetails);

            var tr = "";
            for (var i = 0; i < Result.STSupportDetails.length; i++) {

                if (Result.STSupportDetails[i].Type == "Support") {

                    tr += "<tr>";
                    tr += "<td style='width:50%;'>" + Result.STSupportDetails[i].ItemName + "</td>";
                    tr += "<td style='width:30%;'>" + Result.STSupportDetails[i].HeadName + "</td>";
                    tr += "<td style='width:20%;'>";
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
            $("#ContentPlaceHolder1_txtInternalNotes").val(Result.InternalNotesDetails);
            debugger;

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
                    tr += "<td style='width:15%;'>" + Result.STSupportDetails[i].HeadName + "</td>";
                    tr += "<td style='width:15%;'>" + Result.STSupportDetails[i].UnitPrice + "</td>";
                    tr += "<td style='width:15%;'>" + Result.STSupportDetails[i].UnitQuantity  + "</td>";

                    tr += "<td style='width:10%;'>" + (Result.STSupportDetails[i].VatAmount) + "</td>";
                    tr += "<td style='width:10%;'>" + (Result.STSupportDetails[i].TotalPrice) + "</td>";

                    //tr += "<td style='width:15%;'>" + ((parseFloat(Result.STSupportDetails[i].UnitPrice) * parseFloat(Result.STSupportDetails[i].UnitQuantity)) + Result.STSupportDetails[i].VatAmount).toFixed(2) + "</td>";

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

                        VatRate: parseFloat(Result.STSupportDetails[i].VatRate),
                        VatAmount: parseFloat(Result.STSupportDetails[i].VatAmount),

                        TotalPrice: parseFloat(Result.STSupportDetails[i].TotalPrice),
                        //TotalPrice: parseFloat((Result.STSupportDetails[i].UnitPrice * Result.STSupportDetails[i].UnitQuantity) + Result.STSupportDetails[i].VatAmount),

                        Type: Result.STSupportDetails[i].Type,
                        STSupportDetailsId: Result.STSupportDetails[i].STSupportDetailsId,
                        STSupportId: Result.STSupportDetails[i].STSupportId
                    });
                }
            }

            $("#SupportItemTblForSupport tbody").prepend(tr);

            CommonHelper.ApplyDecimalValidation();

            if (IsCanSave)
                $("#btnGenerate").val('Generate').show();
            else
                $("#btnGenerate").hide();

            UploadComplete();

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

            var tr = "";
            tr += "<tr>";
            tr += "<td style='width:40%;'>" + ItemSelectedForSupportDetails.label + "</td>";
            tr += "<td style='width:25%;'>" + ItemSelectedForSupportDetails.UnitHead + "</td>";
            tr += "<td style='width:25%;'>" +
                "<input type='text' value='" + UnitPriceForSupport + "' id='pp" + ItemSelectedForSupportDetails.label + "' class='form-control quantitydecimal' onblur='CalculateTotalForAdhoq(this)'  />" +
                "</td>";
            tr += "<td style='width:20%;'>";
            tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItemForSupportDetails(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
            tr += "</td>";
            tr += "<td style='display:none;'>" + ItemSelectedForSupportDetails.value + "</td>";
            tr += "<td style='display:none;'>" + ItemSelectedForSupportDetails.CategoryId + "</td>";
            tr += "<td style='display:none;'>" + ItemSelectedForSupportDetails.StockBy + "</td>";
            //tr += "<td style='display:none;'>" + 0 + "</td>";
            tr += "</tr>";

            $("#SupportItemTblForSupport tbody").prepend(tr);
            tr = "";
            CommonHelper.ApplyDecimalValidation();
            SupportItemForSupportDetails.push({
                ItemId: parseInt(ItemSelectedForSupportDetails.value, 10),
                CategoryId: parseInt(ItemSelectedForSupportDetails.CategoryId, 10),
                StockBy: parseInt(ItemSelectedForSupportDetails.StockBy, 10),
                UnitPrice: parseFloat(UnitPriceForSupport),
                Type: 'SupportDetails',
                STSupportDetailsId: 0,
                STSupportId: 0
            });

            ClearSupportItemContainerForSupport();
        }

        function CalculateTotalForAdhoq(control) {
            var tr = $(control).parent().parent();
            var reAmount = $.trim($(tr).find("td:eq(2)").find("input").val());

            if (reAmount == "" || reAmount == "0") {
                toastr.info("Amount Cannot Be Zero Or Empty.");
                return false;
            }

            var itemId = parseInt($.trim($(tr).find("td:eq(4)").text()), 10);

            var ItemIdObj = _.findWhere(SupportItemForSupportDetails, { ItemId: itemId });
            var index = _.indexOf(SupportItemForSupportDetails, ItemIdObj);

            SupportItemForSupportDetails[index].UnitPrice = parseFloat(reAmount);
        }

        function GenerateNClose() {
            isClose = true;
            $.when(GenerateSupportBill()).done(function () {
                if (isClose) {

                    if (typeof parent.CloseDialog === "function") {
                        parent.CloseDialog();
                    }

                }
            });
            return false;
        }

        function GenerateSupportBill() {
            var id = +$("#ContentPlaceHolder1_hfSupportId").val();
            var contactId = $("#ContentPlaceHolder1_hfContactId").val();
            var paymentInstructionId = $("#ContentPlaceHolder1_hfPaymentId").val();
            var rows = new Array();
            rows.push(id);

            PageMethods.GenerateSupportBill(rows, contactId, paymentInstructionId, OnSuccessGenerateSupportBill, OnFailGenerateSupportBill);
            return false;
        }        

        function OnSuccessGenerateSupportBill(result) {
            if (result.IsSuccess) {
                if (result.IsSuccess) {
                    parent.ShowAlert(result.AlertMessage);
                    parent.SearchInformation(1, 1);
                }
                Clear();
            }
        }

        function OnFailGenerateSupportBill(error) {
            isClose = false;
            toastr.error(error.get_message());
            return false;
        }
        function DeleteAdhoqItem(control) {
            if (!confirm("Do you want to delete?")) { return false; }
            var tr = $(control).parent().parent();

            var itemId = parseInt($.trim($(tr).find("td:eq(3)").text()), 10);
            var detailsId = parseInt($.trim($(tr).find("td:eq(6)").text()), 10);

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

            var itemId = parseInt($.trim($(tr).find("td:eq(3)").text()), 10);
            var detailsId = parseInt($.trim($(tr).find("td:eq(6)").text()), 10);

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
                $("#btnGenerate").val('Generate').show();
            }
            else {
                $("#btnGenerate").hide();
            }
            isClose = false;
            return false;
        }
    </script>

    <asp:HiddenField ID="hfSpportCallType" runat="server" Value="" />
    <asp:HiddenField ID="hfClientId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfSupportId" Value="0" runat="server" />
    <%-- <asp:HiddenField ID="hfCompanyId" runat="server" Value=""></asp:HiddenField>--%>
    <asp:HiddenField ID="hfPaymentId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfContactId" runat="server" Value="0"></asp:HiddenField>
    <div>
        <div style="padding: 10px 30px 10px 30px">
            <div id="SupportInformationNCompanyNKnowledgeDiv">
                <div class="form-horizontal">
                    <div class="form-group">
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
                                            <asp:DropDownList runat="server" ID="ddlCaseOwner" CssClass="form-control" Enabled="false">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <label class="control-label required-field">Client Name</label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="txtClientName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <label class="control-label required-field">Support Category</label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList runat="server" ID="ddlSupportCategory" CssClass="form-control" Enabled="false">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <label class="control-label required-field">Support Source</label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList runat="server" ID="ddlSupportSource" CssClass="form-control" Enabled="false">
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
                                                <asp:TextBox ID="txtOtherSource" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <label class="control-label required-field">Case</label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:DropDownList runat="server" ID="ddlCase" CssClass="form-control" Enabled="false">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <label class="control-label">Case Deltails</label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="txtCaseDeltails" runat="server" TextMode="MultiLine" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="panel panel-default" style="display:none;">
                                        <div class="panel-heading">Item/Service Information</div>
                                        <div class="panel-body">
                                            <div class="form-horizontal">
                                                <div style="display: none;">
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
                                    <div class="form-group" style="display:none;">
                                        <div class="col-md-2">
                                            <label class="control-label">Item/Service Details</label>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="txtItemDetails" runat="server" TextMode="MultiLine" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2">
                                            <label class="control-label required-field">Support Stage</label>
                                        </div>
                                        <div class="col-md-10"><asp:DropDownList runat="server" ID="ddlSupportStage" CssClass="form-control" Enabled="false">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div id="SupportDetailsDiv">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">Item/Service Information For Support Details</div>
                                            <div class="panel-body">
                                                <div class="form-horizontal">
                                                    <div style="display: none;">
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
                                                            <div class="col-md-10">
                                                                <asp:TextBox ID="txtCurrentStockByForSupport" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-md-2">
                                                                <asp:Label ID="lblUnitPriceForSupport" runat="server" class="control-label required-field" Text="Unit Price"></asp:Label>
                                                            </div>
                                                            <div class="col-md-10">
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
                                                    </div>
                                                    <div style="height: 250px; overflow-y: scroll;">
                                                        <table id="SupportItemTblForSupport" class="table table-bordered table-condensed table-hover">
                                                            <thead>
                                                                <tr>
                                                                    <th style="width: 35%;">Item</th>
                                                                    <th style="width: 15%;">Stock Unit</th>
                                                                    <th style="width: 10%;">Unit Price</th>                                                                    
                                                                    <th style="width: 10%;">Unit Quantity</th>
                                                                    <th style="width: 10%;">Vat</th>
                                                                    <th style="width: 15%;">Total Price</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody></tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <label class="control-label">Internal Notes</label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:TextBox ID="txtInternalNotes" runat="server" TextMode="MultiLine" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <label class="control-label required-field">Support Type</label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList runat="server" ID="ddlSupportType" CssClass="form-control" Enabled="false">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2 ">
                                                <label class="control-label required-field">Support Deadline</label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtDeadLineDate" runat="server" CssClass="form-control" TabIndex="1" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <label class="control-label required-field">Support Priority</label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList runat="server" ID="ddlSupportPriority" CssClass="form-control" Enabled="false">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <label class="control-label">Bill Confirmation</label>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList runat="server" ID="ddlBillConfirmation" CssClass="form-control" Enabled="false">
                                                    <asp:ListItem Text="No" Value="NO"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="YES"></asp:ListItem>
                                                    <asp:ListItem Text="TBA" Value="TBA"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group" style="display:none;">
                                            <div class="col-md-2">
                                                <label class="control-label required-field">Support Forward To</label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:DropDownList runat="server" ID="ddlSupportForwardTo" CssClass="form-control" Enabled="false">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:HiddenField ID="RandomProductId" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="hfGuestDeletedDoc" runat="server" Value=""></asp:HiddenField>
                                            <div class="col-md-2">
                                                <label class="control-label">Attachment</label>
                                            </div>
                                            <div style="display: none;">
                                                <div class="col-md-10">
                                                    <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Attach" onclick="AttachFile()" />
                                                </div>
                                            </div>
                                        </div>
                                        <div id="DocumentInfo">
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

 
                                <div class="form-horizontal">
                                 <div id="AttentionDiv" class="panel panel-default col-sm-6" style="height: 333px;">
                                <div class="panel-heading">
                                    Attention Detail
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <asp:Label ID="Label1" runat="server" Text="Contact" CssClass="control-label col-sm-3"></asp:Label>
                                            <div class="col-sm-9">
                                                <input type="text" class="form-control" id="txtSearchContact" />
                                            </div>
                                        </div>
                                        <fieldset>
                                            <legend>Contact Details</legend>
                                            <div class="form-group">
                                                <asp:Label ID="Label2" runat="server" Text="Designation" CssClass="control-label col-sm-3"></asp:Label>
                                                <div class="col-sm-9">
                                                    <input type="text" class="form-control" id="txtDesignation" readonly="readonly" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="Label3" runat="server" Text="Department" CssClass="control-label col-sm-3"></asp:Label>
                                                <div class="col-sm-9">
                                                    <input type="text" class="form-control" id="txtDepartment" readonly="readonly" />
                                                </div>
                                            </div>

                                        </fieldset>
                                    </div>
                                </div>
                            </div>
                            <div id="PaymentInstructionDiv" class="panel panel-default col-sm-6" style="height: 333px;">
                                <div class="panel-heading">
                                    Payment Instruction
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <asp:Label ID="Label5" runat="server" Text="Bank" CssClass="control-label col-sm-3"></asp:Label>
                                            <div class="col-sm-9">
                                                <input type="text" class="form-control" id="txtSearchBank" />
                                            </div>
                                        </div>
                                        <fieldset>
                                            <legend>Payment Details</legend>
                                            <div class="form-group">
                                                <asp:Label ID="Label7" runat="server" Text="Branch Name" CssClass="control-label col-sm-3"></asp:Label>
                                                <div class="col-sm-9">
                                                    <input type="text" class="form-control" id="txtBranchName" readonly="readonly" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="Label8" runat="server" Text="Account Name" CssClass="control-label col-sm-3"></asp:Label>
                                                <div class="col-sm-9">
                                                    <input type="text" class="form-control" id="txtAccountName" readonly="readonly" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="Label9" runat="server" Text="Account Number" CssClass="control-label col-sm-3"></asp:Label>
                                                <div class="col-sm-9">
                                                    <input type="text" class="form-control" id="txtAccountNumber" readonly="readonly" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="Label10" runat="server" Text="Account Type" CssClass="control-label col-sm-3"></asp:Label>
                                                <div class="col-sm-9">
                                                    <input type="text" class="form-control" id="txtAccountType" readonly="readonly" />
                                                </div>
                                            </div>
                                        </fieldset>
                                    </div>
                                </div>
                            </div>

                                    </div>

                                <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                                    <div class="col-md-12">
                                        <input id="btnGenerate" type="button" class="TransactionalButton btn btn-primary btn-sm"
                                            onclick="GenerateNClose()" value="Generate" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
