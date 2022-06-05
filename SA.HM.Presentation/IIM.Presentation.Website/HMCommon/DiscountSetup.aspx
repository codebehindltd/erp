<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="DiscountSetup.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.DiscountSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var Table, DiscountTable, SearchTable;
        var deletedItemList;
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;

        $(document).ready(function () {
            deletedItemList = new Array();

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;

            if (IsCanSave)
                $("#btnSave").show();
            else
                $("#btnSave").hide();

            $("#myTabs").tabs();
           

            $('#ContentPlaceHolder1_txtFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                minDate: DayOpenDate,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtToDate').datepicker("option", "minDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                minDate: DayOpenDate,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtFromDate').datepicker("option", "maxDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtSearchFromDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchToDate').datepicker("option", "minDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $('#ContentPlaceHolder1_txtSearchToDate').datepicker({
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $('#ContentPlaceHolder1_txtSearchFromDate').datepicker("option", "maxDate", selectedDate);
                }
            }).datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_ddlDiscountAppliedOn").change(function () {

                var type = $(this).val();

                if (type == "0") {
                    $("#dvCategory").hide();
                    $("#dvRoom").hide();
                    $("#dvCostcenter").hide();
                }
                else if (type == "Item") {
                    $("#dvCategory").show();
                    $("#dvRoom").hide();
                    $("#dvCostcenter").show();
                }
                else if (type == "Room") {
                    $("#dvCategory").hide();
                    $("#dvRoom").show();
                    $("#dvCostcenter").hide();
                }
                else {
                    $("#dvCostcenter").show();
                    if (type == "RoomType")
                        $("#dvCostcenter").hide();
                    $("#dvCategory").hide();
                    $("#dvRoom").hide();
                }
                Table.clear().draw();
                LoadDiscountItems();
            });

            $("#ContentPlaceHolder1_ddlCategory").change(function () {
                $("#ContentPlaceHolder1_ddlDiscountAppliedOn").trigger('change');
            });

            $("#ContentPlaceHolder1_ddlRoomType").change(function () {
                $("#ContentPlaceHolder1_ddlDiscountAppliedOn").trigger('change');
            });

            $("#ContentPlaceHolder1_ddlCostCenter").change(function () {
                $("#ContentPlaceHolder1_ddlDiscountAppliedOn").trigger('change');
            });

            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#ContentPlaceHolder1_ddlRoomType").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            CommonHelper.ApplyDecimalValidation();

            Table = $("#tblItems").DataTable({
                data: [],
                columns: [
                    { title: "<input id='chkAllItem' type='checkbox'></input>", "data": null, sWidth: '5%' },
                    { title: "", "data": "DiscountForName", sWidth: '35%' },
                    { title: "Discount Type", "data": null, sWidth: '25%' },
                    { title: "Discount", "data": null, sWidth: '35%' },
                    { title: "", "data": "DiscountForId", visible: false }
                ],
                columnDefs: [
                    {
                        "targets": 0,
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            return '<input type="checkbox" />';
                        }
                    },
                    {
                        "targets": 2,
                        "render": function (data, type, full, meta) {
                            return '<select class="form-control">' +
                                '<option value="0">---Please Select---</option>' +
                                '<option value="Fixed">Fixed</option>' +
                                '<option value="Percentage">Percentage</option>' +
                                '</select > ';
                        }
                    },
                    {
                        "targets": 3,
                        "render": function (data, type, full, meta) {
                            return '<input type="text" class="form-control quantitydecimal" />';
                        }
                    },
                    {
                        targets: [0, 1],
                        orderable: false
                    }
                ],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }
                },
                pageLength: UserInfoFromDB.GridViewPageSize,
                filter: true,
                ordering: false,
                processing: true,
                retrieve: true,
                bAutoWidth: false,
                bLengthChange: false,
                bInfo: false,
                bPaginate: false,
                scrollX: "0px",
                scrollY: "300px",
                scrollCollapse: true,
                language: {
                    emptyTable: "No Data Found"
                },
                bJQueryUI: true,
                sScrollXInner: "100%",
                fnInitComplete: function () {
                    this.css("visibility", "visible");
                },
                bScrollAutoCss: true
            });

            DiscountTable = $("#tblDiscountItems").DataTable({
                data: [],
                columns: [
                    { title: "Item", "data": "DiscountForName", sWidth: '35%' },
                    { title: "Discount Type", "data": "DiscountType", sWidth: '30%' },
                    { title: "Discount", "data": "Discount", sWidth: '25%' },
                    { title: "Action", "data": null, sWidth: '10%' },
                    { title: "", "data": "DiscountForId", visible: false },
                    { title: "", "data": "Id", visible: false },
                    { title: "", "data": "IsEdited", visible: false, "defaultContent": "0" }
                ],
                columnDefs: [
                    {
                        "targets": 1,
                        "render": function (data, type, full, meta) {

                            return '<select class="form-control" onchange="UpdateEditFlag(this)">' +
                                '<option value="Fixed">Fixed</option>' +
                                '<option value="Percentage">Percentage</option>' +
                                '</select > ';

                        }
                    },
                    {
                        "targets": 2,
                        "render": function (data, type, full, meta) {
                            return '<input type="text" class="form-control quantitydecimal" value="' + data + '" onchange="UpdateEditFlag(this)" />';
                        }
                    }

                ],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }
                    $('td:eq(1) select', nRow).val(aData.DiscountType);
                    $('td:eq(3)', nRow).html("&nbsp;&nbsp;<a href='javascript:void();' onclick= 'DeleteItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>");
                },
                pageLength: UserInfoFromDB.GridViewPageSize,
                filter: true,
                info: false,
                ordering: false,
                processing: true,
                retrieve: true,
                bAutoWidth: false,
                bLengthChange: false,
                bInfo: false,
                bPaginate: false,
                scrollY: "300px",
                scrollCollapse: true,
                language: {
                    emptyTable: " "
                }
            });

            SearchTable = $("#tblSearchDiscount").DataTable({
                data: [],
                columns: [
                    { title: "DisCount Name", "data": "DiscountName", sWidth: '30%' },
                    { title: "Cost Center", "data": "CostCenter", sWidth: '25%' },
                    { title: "Discount For", "data": "DiscountFor", sWidth: '15%' },
                    { title: "From", "data": "FromDate", sWidth: '10%' },
                    { title: "To", "data": "Todate", sWidth: '10%' },
                    { title: "Action", "data": null, sWidth: '10%' },
                    { title: "", "data": "Id", visible: false }
                ],

                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    var row = '';
                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }
                    if (IsCanEdit)
                        row += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return PerformEdit('" + aData.Id + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";
                    if (IsCanDelete)
                        row += "&nbsp;&nbsp;<a href='#' onclick= \"DeleteDiscount('" + aData.Id + "');\"> <img alt='Delete' src='../Images/delete.png' /></a>";

                    row += "&nbsp;&nbsp;<a href='#' onclick= \"GetDiscountDetails('" + aData.Id + "');\"> <img alt='Delete' src='../Images/detailsInfo.png' /></a>";

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

            //$(window).bind('resize', function () {
            //    Table.fnAdjustColumnSizing();
            //    DiscountTable.fnAdjustColumnSizing();
            //});

            $('#tblItems_filter input').addClass('form-control');
            $('#tblDiscountItems_filter input').addClass('form-control');

            $("[id=chkAllItem]").on("change", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#tblItems tbody tr").find("td:eq(0)").find("input").prop("checked", true);
                }
                else {
                    $("#tblItems tbody tr").find("td:eq(0) ").find("input").prop("checked", false);
                }
            });
        });

        //function LoadDiscountItems(pageNumber, IsCurrentOrPreviousPage) {

        //    var gridRecordsCount = Table.data().length;

        //    var appliedOn = $("#ContentPlaceHolder1_ddlDiscountAppliedOn").val();
        //    var type = 0;
        //    var costCenter = +$("#ContentPlaceHolder1_ddlCostCenter").val();

        //    if (appliedOn == "Item")
        //        type = +$("#ContentPlaceHolder1_ddlCategory").val();
        //    else if (appliedOn == "Room")
        //        type = +$("#ContentPlaceHolder1_ddlRoomType").val();

        //    PageMethods.GetDiscountItemList(appliedOn, type, costCenter, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
        //    return false;
        //}

        function LoadDiscountItems() {

            var appliedOn = $("#ContentPlaceHolder1_ddlDiscountAppliedOn").val();
            var type = 0;
            var costCenterId = +$("#ContentPlaceHolder1_ddlCostCenter").val();

            if (appliedOn == "Item") {
                type = +$("#ContentPlaceHolder1_ddlCategory").val();

                if (costCenterId == 0 || type == 0)
                    return false;
            }
            else if (appliedOn == "Room")
                type = +$("#ContentPlaceHolder1_ddlRoomType").val();

            PageMethods.GetDiscountItemList(appliedOn, type, costCenterId, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }

        function OnLoadObjectSucceeded(result) {

            //$("#GridPagingContainer ul").html("");

            var type = $("#ContentPlaceHolder1_ddlDiscountAppliedOn").val();
            Table.clear();

            if (type == "0") {
                Table.columns(1).header().to$().text("Item Name");
            }
            else if (type == "Item") {
                Table.columns(1).header().to$().text("Item Name");
            }
            else if (type == "Room") {
                Table.columns(1).header().to$().text("Room Number");
            }
            else if (type == "RoomType") {
                Table.columns(1).header().to$().text("Room Type");
            }
            else
                Table.columns(1).header().to$().text("Category Name");
            //result.GridData
            Table.rows.add(result);
            Table.draw();

            //$("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            //$("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            //$("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            CommonHelper.ApplyDecimalValidation();
            return false;
        }
        function OnLoadObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function AddNewItem() {

            var length = $("#tblDiscountItems tbody tr").length;
            var rows = new Array();
            if (length > 0)
                $("#ContentPlaceHolder1_ddlDiscountAppliedOn").attr('disabled', true);

            $("#tblItems tbody tr").each(function (index, item) {

                var name, discountType, discount, discountForId;

                if ($(this).find("td:eq(0)").find("input").is(':checked')) {

                    name = $.trim($(item).find("td:eq(1)").text());
                    discountType = ($(item).find("td:eq(2) select").val() == "0" ? $("#ContentPlaceHolder1_ddlAllDiscountType").val() : $(item).find("td:eq(2) select").val());
                    discount = parseFloat($.trim($(item).find("td:eq(3) input").val()));
                    discountForId = Table.row($(this)).data().DiscountForId;

                    if (isNaN(discount))
                        discount = (isNaN(parseFloat($("#ContentPlaceHolder1_txtAllDiscount").val())) ? '' : parseFloat($("#ContentPlaceHolder1_txtAllDiscount").val()));

                    var duplicte = _.where(DiscountTable.data(), { DiscountForId: discountForId });

                    if (duplicte.length > 0) {
                        toastr.info(name + ' already exist cannot be added');
                    }

                    else {
                        rows.push({
                            DiscountForName: name,
                            DiscountType: discountType,
                            Discount: discount,
                            DiscountForId: discountForId,
                            Id: 0,
                            IsEdited: 1
                        });
                    }


                }
            });

            DiscountTable.rows.add(rows);
            DiscountTable.columns.adjust().draw();

            CommonHelper.ApplyDecimalValidation();
        }

        function DeleteItem(item) {

            var row = $(item).parents('tr');

            var id = DiscountTable.row(row).data().Id;
            DiscountTable.row(row).remove().draw(false);
            if (id > 0)
                deletedItemList.push(id);

            if (DiscountTable.data().length == 0)
                $("#ContentPlaceHolder1_ddlDiscountAppliedOn").attr('disabled', false);
        }
        function SearchDiscount() {
            var name, costCenterId, fromDate, toDate;

            name = $("#ContentPlaceHolder1_txtSearchDiscountName").val();
            costCenterId = +$("#ContentPlaceHolder1_ddlSearchCostCenter").val();

            if ($("#ContentPlaceHolder1_txtSearchFromDate").val() == "")
                fromDate = new Date();
            else
                fromDate = $("#ContentPlaceHolder1_txtSearchFromDate").val();

            if ($("#ContentPlaceHolder1_txtSearchToDate").val() == "")
                toDate = new Date();
            else
                toDate = $("#ContentPlaceHolder1_txtSearchToDate").val();

            fromDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(fromDate, innBoarDateFormat);
            toDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(toDate, innBoarDateFormat);

            PageMethods.GetAllDiscount(fromDate, toDate, costCenterId, name, OnSearchSucceeded, OnSeacrhFailed);
            return false;
        }

        function OnSearchSucceeded(result) {

            var type = $("#ContentPlaceHolder1_ddlDiscountAppliedOn").val();

            SearchTable.clear();

            $.each(result, function (key, value) {

                value.FromDate = GetStringFromDateTime(value.FromDate);
                value.Todate = GetStringFromDateTime(value.Todate);
            });

            SearchTable.rows.add(result);
            SearchTable.draw();

            return false;
        }
        function OnSeacrhFailed(error) {
            toastr.error(error.get_message());
        }

        function PerformEdit(id) {
            if (confirm("Do you want to edit?")) {

                PageMethods.GetDiscount(id, OnLoadDiscountSuccess, OnLoadDiscountFailed);
            }
            return false;
        }

        function OnLoadDiscountSuccess(result) {

            $("#ContentPlaceHolder1_ddlCostCenter").val(result.CostCenterId);
            $("#ContentPlaceHolder1_ddlDiscountAppliedOn").val(result.DiscountFor).trigger('change');
            $("#ContentPlaceHolder1_txtDiscountName").val(result.DiscountName);
            $("#ContentPlaceHolder1_txtFromDate").val(GetStringFromDateTime(result.FromDate));
            $("#ContentPlaceHolder1_txtToDate").val(GetStringFromDateTime(result.Todate));
            $("#ContentPlaceHolder1_txtRemarks").val(result.Remarks);
            $("#ContentPlaceHolder1_hfDiscountMasterId").val(result.Id);

            DiscountTable.clear();
            DiscountTable.rows.add(result.DiscountDetails);
            DiscountTable.draw();

            var length = $("#tblDiscountItems tbody tr").length;

            if (length > 0)
                $("#ContentPlaceHolder1_ddlDiscountAppliedOn").attr('disabled', true);

            $("#myTabs").tabs({ active: 0 });
            $("#btnSave").val("Update");
        }
        function OnLoadDiscountFailed(error) {
            toastr.error(error.get_message());
        }

        function SaveOrUpdateDiscount() {

            var rows = new Array();
            var name, discountType, discount, discountForId, discountDetailsId, isEdited;

            for (var i = 0; i < DiscountTable.data().length; i++) {

                name = $.trim(DiscountTable.cell(i, 0).nodes().to$().text());
                discountType = $.trim(DiscountTable.cell(i, 1).nodes().to$().find('select').val());
                discount = parseInt(DiscountTable.cell(i, 2).nodes().to$().find('input').val());
                discountForId = DiscountTable.row(i).data().DiscountForId;
                discountDetailsId = DiscountTable.row(i).data().Id;
                isEdited = (DiscountTable.row(i).data().IsEdited == 1);

                if (isEdited) {
                    if (isNaN(discount)) {
                        DiscountTable.cell(i, 2).nodes().to$().find('input').focus();
                        toastr.warning("Enter Discount");
                        return false;
                    }
                    else
                        rows.push({
                            DiscountForName: name,
                            DiscountType: discountType,
                            Discount: discount,
                            DiscountForId: discountForId,
                            Id: discountDetailsId
                        });
                }

            }

            if (DiscountTable.data().length == 0) {
                toastr.warning("Add Discount Item");
                return false;
            }
            var discountFor = $("#ContentPlaceHolder1_ddlDiscountAppliedOn").val();
            var discountName = $("#ContentPlaceHolder1_txtDiscountName").val();
            var costCenterId = +$("#ContentPlaceHolder1_ddlCostCenter").val();
            var fromDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY($("#ContentPlaceHolder1_txtFromDate").val(), innBoarDateFormat);
            var toDate = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY($("#ContentPlaceHolder1_txtToDate").val(), innBoarDateFormat);
            var remarks = $("#ContentPlaceHolder1_txtRemarks").val();
            var masterId = +$("#ContentPlaceHolder1_hfDiscountMasterId").val();

            if (discountName == "") {
                $("#ContentPlaceHolder1_txtDiscountName").focus();
                toastr.warning("Select Discount Name");
                return false;
            }
            else if (discountFor == "0") {
                $("#ContentPlaceHolder1_ddlDiscountAppliedOn").focus();
                toastr.warning("Select Discount Applied On");
                return false;
            }

            else if (costCenterId == 0 && (discountFor == "Item" || discountFor == "Category")) {
                $("#ContentPlaceHolder1_ddlCostCenter").focus();
                toastr.warning("Select Cost Center");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtFromDate").val() == "") {
                $("#ContentPlaceHolder1_txtFromDate").focus();
                toastr.warning("Select From Date");
                return false;
            }
            else if ($("#ContentPlaceHolder1_txtToDate").val() == "") {
                $("#ContentPlaceHolder1_txtToDate").focus();
                toastr.warning("Select To Date");
                return false;
            }
            var discountObj = {
                Id: masterId,
                FromDate: fromDate,
                Todate: toDate,
                DiscountFor: discountFor,
                Remarks: remarks,
                DiscountName: discountName,
                CostCenterId: costCenterId,
                DiscountDetails: rows
            };

            if (confirm("Want to Save?")) {
                PageMethods.SaveOrUpdateDiscount(discountObj, deletedItemList, OnSaveOrUpdateDiscountSuccess, OnSaveOrUpdateDiscountFailed);
            }
            return false;
        }
        function OnSaveOrUpdateDiscountSuccess(result) {
            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
            }
            Clear();
        }
        function OnSaveOrUpdateDiscountFailed(error) {
            toastr.error(error.get_message());
        }

        function DeleteDiscount(id) {
            if (confirm("Want to Delete?")) {
                PageMethods.DeleteDiscount(id, OnDeleteDiscountSuccess, OnDeleteDiscountFailed);
            }

            return false;
        }

        function OnDeleteDiscountSuccess(result) {

            if (result.IsSuccess) {
                CommonHelper.AlertMessage(result.AlertMessage);
                SearchDiscount();
            }

        }
        function OnDeleteDiscountFailed(error) {
            toastr.error(error.get_message());
        }

        function UpdateEditFlag(row) {
           
            var row = $(row).parents('tr');
            DiscountTable.cell(row, 6).data("1");
            var value = DiscountTable.cell(row, 1).nodes().to$().find('select').val();
            DiscountTable.cell(row, 1).data(value).draw();
        }

        function GetDiscountDetails(requisitionId) {
            CommonHelper.SpinnerOpen();
            $("#ContentPlaceHolder1_hfRequisitionId").val(requisitionId);
            PageMethods.GetDiscountDetails(requisitionId, OnFillDiscountDetailsSucceed, OnFillDiscountDetailsFailed);
            return false;
        }

        function OnFillDiscountDetailsSucceed(result) {

            $("#DiscountDetailsGridContainer").html(result);

            $("#DiscountDetailsDialog").dialog({
                autoOpen: true,
                modal: true,
                minWidth: 700,
                maxWidth: 900,
                maxHeight: 400,
                closeOnEscape: true,
                resizable: false,
                height: 'auto',
                fluid: true,
                title: "Discount Details",
                show: 'slide'
            });
            CommonHelper.SpinnerClose();
        }
        function OnFillDiscountDetailsFailed(error) {
            toastr.error(error.get_message());
            CommonHelper.SpinnerClose();
        }

        //function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
        //    LoadDiscountItems(pageNumber, IsCurrentOrPreviousPage);
        //}

        function Clear() {

            deletedItemList = [];
            $("#ContentPlaceHolder1_ddlCostCenter").val("0");
            $("#ContentPlaceHolder1_ddlDiscountAppliedOn").val("0").attr('disabled', false).trigger('change');
            $("#ContentPlaceHolder1_txtDiscountName").val('');
            $("#ContentPlaceHolder1_ddlAllDiscountType").val("Fixed");
            $("#ContentPlaceHolder1_txtAllDiscount").val('');
            //$("#ContentPlaceHolder1_txtFromDate").val();
            //$("#ContentPlaceHolder1_txtToDate").val();
            $("#ContentPlaceHolder1_txtRemarks").val('');
            $("#ContentPlaceHolder1_hfDiscountMasterId").val('0');
            $("#chkAllItem").prop("checked", false);
            $("#ContentPlaceHolder1_ddlCategory").val("0").trigger('change');
            $("#ContentPlaceHolder1_ddlRoomType").val("0").trigger('change');
            Table.clear().draw();
            DiscountTable.clear().draw();
            SearchTable.clear().draw();
            if (IsCanSave)
                $("#btnSave").val('Save').show();
            else
                $("#btnSave").hide();
        }
    </script>
    <style type="text/css">
        table.dataTable thead {
            color: White;
            background-color: #44545E;
            font-weight: bold;
        }

            table.dataTable thead th,
            table.dataTable thead td {
                border-bottom: 1px solid #ddd;
            }

        table.dataTable.no-footer, .dataTables_wrapper.no-footer .dataTables_scrollBody {
            border-bottom: 1px solid #ddd;
        }

        .dataTables_filter {
            width: 50%;
            float: left !important;
            text-align: left !important;
        }

        .dataTables_scrollHeadInner {
            width: 100% !important;
        }

        div.dataTables_scrollHead table.dataTable {
            width: 100% !important;
        }

        table.dataTable td.dataTables_empty {
            text-align: left;
            background-color: #E3EAEB !important;
        }

        table.dataTable {
            box-sizing: inherit;
        }
    </style>

    <asp:HiddenField ID="hfDiscountMasterId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />

    <div id="DiscountDetailsDialog" style="display: none;">
        <div id="DiscountDetailsGridContainer">
        </div>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Discount Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Discount</a></li>
        </ul>
        <div id="tab-1">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Discount Entry
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label required-field">
                                    Discount Name
                                </label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDiscountName" runat="server" CssClass="form-control" placeholder="Enter Discount Name" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label required-field">
                                    Discount Applied On
                                </label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlDiscountAppliedOn" runat="server" CssClass="form-control" TabIndex="2">
                                    <asp:ListItem Value="Category">Category</asp:ListItem>
                                    <asp:ListItem Value="Item">Item</asp:ListItem>
                                    <asp:ListItem Value="RoomType">RoomType</asp:ListItem>
                                    <asp:ListItem Value="Room">Room</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="dvCostcenter" style="display: none">
                            <div class="col-md-2">
                                <label class="control-label required-field">
                                    Cost Center
                                </label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="form-control" TabIndex="3"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="dvCategory" style="display: none">
                            <div class="col-md-2">
                                <label class="control-label">
                                    Category
                                </label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" TabIndex="4"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="dvRoom" style="display: none">
                            <div class="col-md-2">
                                <label class="control-label">
                                    Room Type
                                </label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlRoomType" runat="server" CssClass="form-control" TabIndex="5"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label required-field">
                                    From Date
                                </label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <label class="control-label required-field">
                                    To Date
                                </label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label">
                                    Discount Type
                                </label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlAllDiscountType" runat="server" CssClass="form-control" TabIndex="8">
                                    <asp:ListItem Value="Fixed">Fixed</asp:ListItem>
                                    <asp:ListItem Value="Percentage">Percentage</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <label class="control-label">
                                    Discount
                                </label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtAllDiscount" runat="server" placeholder="For All Item" CssClass="form-control quantitydecimal" TabIndex="9"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label">
                                    Remarks
                                </label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine" TabIndex="10"></asp:TextBox>
                            </div>
                        </div>
                        <div id="dvItem" class="panel panel-default">
                            <div class="panel-heading">
                                Item List
                            </div>
                            <div class="panel-body">
                                <div>
                                    <table id="tblItems" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
                                    </table>
                                </div>
                            </div>
                            <div class="row" style="padding-top: 10px;">
                                <div class="col-md-12">
                                    <input type="button" id="btnAdd" class="TransactionalButton btn btn-primary btn-sm" tabindex="11" value="Add" onclick="AddNewItem()" />
                                </div>
                            </div>
                        </div>
                        <div id="dvDiscountItems" class="panel panel-default">
                            <div class="panel-heading">
                                Discount Item List
                            </div>
                            <div class="panel-body">
                                <table id="tblDiscountItems" class="table table-bordered table-condensed table-responsive" style="width: 100%;">
                                </table>
                            </div>
                        </div>
                        <div class="row" style="padding-top: 10px;">
                            <div class="col-md-12">
                                <input type="button" id="btnSave" class="TransactionalButton btn btn-primary btn-large" tabindex="12" value="Save" onclick="SaveOrUpdateDiscount()" />
                                <input type="button" id="btnClear" class="TransactionalButton btn btn-primary btn-large" tabindex="13" value="Clear" onclick='if (confirm("Are you sure?")) { Clear(); }' />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Search Discount
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label">
                                    Discount Name
                                </label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchDiscountName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label">
                                    Cost Center
                                </label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSearchCostCenter" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label">
                                    From Date
                                </label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <label class="control-label">
                                    To Date
                                </label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchToDate" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row" style="padding-top: 10px;">
                            <div class="col-md-12">
                                <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm" tabindex="9" value="Search" onclick="SearchDiscount()" />
                                <input type="button" id="btnSearchClear" class="TransactionalButton btn btn-primary btn-sm" tabindex="10" value="Clear" />
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <div class="panel panel-default" style="overflow-y: scroll;">
                <div class="panel-heading">
                    Discount Item List
                </div>
                <div class="panel-body">
                    <table id="tblSearchDiscount" class="table table-bordered table-condensed table-responsive">
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
