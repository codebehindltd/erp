<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="RepairAndMaintenance.aspx.cs" Inherits="HotelManagement.Presentation.Website.HouseKeeping.RepairAndMaintenance" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var flag = 0;
        $(document).ready(function () {
            $("#fixedAsset").show();
            $("#nonFixedAsset").hide();
            $("#fixedAssetSrc").hide();
            $("#nonFixedAssetSrc").hide();
            $("#<%=ddlMaintenanceType.ClientID %>").change(function () {
                var type = $("#ContentPlaceHolder1_ddlMaintenanceType").val();
                if (type == 'Fixed Asset') {
                    $("#fixedAsset").show("slow");
                    $("#nonFixedAsset").hide("slow");
                }
                else {
                    $("#nonFixedAsset").show("slow");
                    $("#fixedAsset").hide("slow");
                }
            });
            $("#<%=ddlMaintenanceTypeSrc.ClientID %>").change(function () {
                var type = $("#ContentPlaceHolder1_ddlMaintenanceTypeSrc").val();
                if (type == 'Fixed Asset') {
                    $("#fixedAssetSrc").show("slow");
                    $("#nonFixedAssetSrc").hide("slow");
                }
                else if (type == '0') {
                    $("#nonFixedAssetSrc").hide("slow");
                    $("#fixedAssetSrc").hide("slow");
                }
                else {
                    $("#nonFixedAssetSrc").show("slow");
                    $("#fixedAssetSrc").hide("slow");
                }
            });
            $("#<%=txtFromDateSrc.ClientID %>").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $("#<%=txtToDateSrc.ClientID %>").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $("#ContentPlaceHolder1_txtExpectedDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);

            $("#<%=txtExpectedTime.ClientID%>").timepicker({
                showPeriod: is12HourFormat,
            })
            $("#<%=ddlFixedItemSrc.ClientID %>").select2({
                width: "100%",
                tags: true
            });
            $("#<%=ddlFixedItem.ClientID %>").select2({
                width: "100%",
                tags: true,
                dropdownParent: $("#CreateNewDialog")
            });
            $("#<%=ddlTransectionType.ClientID%>").change(function () {
                var type = $("#ContentPlaceHolder1_ddlTransectionType").val();
                ChangeTransectionType(type, 1);
            });
            $("#<%=ddlTransectionTypeSrc.ClientID%>").change(function () {
                var type = $("#ContentPlaceHolder1_ddlTransectionTypeSrc").val();
                ChangeTransectionType(type, 0);
            });
            GridPaging(1, 1);
        });
        function ChangeTransectionType(type, flag) {
            if (type == "Room") {
                //divRoomRest otherAreaDiv lblTransectionId
                if (flag == 1) {
                    $("#divRoomRest").show("slow");
                    $("#<%=lblTransectionId.ClientID%>").text("Room");
                    $("#otherAreaDiv").hide("slow");
                }
                else {
                    $("#divRoomRestSrc").show("slow");
                    $("#<%=lblTransectionIdSrc.ClientID%>").text("Room");
                }
                $.ajax({

                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../HouseKeeping/HKLostFound.aspx/LoadRoom',
                    async: false,
                    data: "{'type':'" + type.trim() + "'}",
                    dataType: "json",
                    success: function (data) {
                        OnLoadRoomSucceed(data.d, flag);
                    },
                    error: function (result) {
                        CommonHelper.AlertMessage(result.d.AlertMessage);
                    }
                });
            }
            else if (type == "Restaurant") {
                if (flag == 1) {
                    $("#divRoomRest").show("slow");
                    $("#<%=lblTransectionId.ClientID%>").text("Restaurant");
                    $("#otherAreaDiv").hide("slow");
                }
                else {
                    $("#divRoomRestSrc").show("slow");
                    $("#<%=lblTransectionIdSrc.ClientID%>").text("Restaurant");
                }
                $.ajax({

                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../HouseKeeping/HKLostFound.aspx/LoadRestaurant',
                    async: false,
                    data: "{'type':'" + type.trim() + "'}",
                    dataType: "json",
                    success: function (data) {
                        OnLoadRestaurantSucceed(data.d, flag);
                    },
                    error: function (result) {
                        CommonHelper.AlertMessage(result.d.AlertMessage);
                    }
                });
            }
            else if (type == "Banquet") {
                if (flag == 1) {
                    $("#divRoomRest").show("slow");
                    $("#<%=lblTransectionId.ClientID%>").text("Banquet");
                    $("#otherAreaDiv").hide("slow");
                }
                else {
                    $("#divRoomRestSrc").show("slow");
                    $("#<%=lblTransectionIdSrc.ClientID%>").text("Banquet");
                }
                $.ajax({

                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: '../HouseKeeping/HKLostFound.aspx/LoadBanquet',
                    async: false,
                    data: "{'type':'" + type.trim() + "'}",
                    dataType: "json",
                    success: function (data) {
                        OnLoadBanquetSucceed(data.d, flag);
                    },
                    error: function (result) {
                        CommonHelper.AlertMessage(result.d.AlertMessage);
                    }
                });
            }
            else if (type == "Others") {
                if (flag == 1) {
                    $("#divRoomRest").hide("slow");
                    $("#otherAreaDiv").show("slow");
                    $("#<%=lblTransectionId.ClientID%>").text("");
                }
                else {
                    $("#divRoomRestSrc").hide("slow");
                    $("#<%=lblTransectionIdSrc.ClientID%>").text("");
                }
            }
            else {
                if (flag == 1) {
                    $("#divRoomRest").hide("slow");
                    $("#otherAreaDiv").hide("slow");
                    $("#<%=lblTransectionId.ClientID%>").text("");
                }
                else {
                    $("#divRoomRestSrc").hide("slow");
                    $("#<%=lblTransectionIdSrc.ClientID%>").text("");
                }
            }
        }
        function OnLoadRoomSucceed(result, flag) {
            var list = result;
            var controlId = "";
            if (flag == 1) {
                controlId = '<%=ddlTransectionId.ClientID%>';
            }
            else {
                controlId = '<%=ddlTransectionIdSrc.ClientID%>';
            }
            var control = $('#' + controlId);
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].RoomNumber + '" value="' + list[i].RoomId + '">' + list[i].RoomNumber + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }
            return false;
        }
        function OnLoadRestaurantSucceed(result, flag) {
            var list = result;
            var controlId = "";
            if (flag == 1) {
                controlId = '<%=ddlTransectionId.ClientID%>';
            }
            else {
                controlId = '<%=ddlTransectionIdSrc.ClientID%>';
            }
            var control = $('#' + controlId);
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].CostCenter + '" value="' + list[i].CostCenterId + '">' + list[i].CostCenter + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }
            return false;
        }
        function OnLoadBanquetSucceed(result, flag) {
            var list = result;
            var controlId = "";
            if (flag == 1) {
                controlId = '<%=ddlTransectionId.ClientID%>';
            }
            else {
                controlId = '<%=ddlTransectionIdSrc.ClientID%>';
            }
            var control = $('#' + controlId);
            control.empty();
            if (list != null) {
                if (list.length > 0) {
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                    for (i = 0; i < list.length; i++) {
                        control.append('<option title="' + list[i].Name + '" value="' + list[i].Id + '">' + list[i].Name + '</option>');
                    }
                }
                else {
                    control.removeAttr("disabled");
                    control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                }
            }
            return false;
        }
        function CreateRepairAndMaintenance() {
            PerformClearAction();
            $("#<%=btnSaveClose.ClientID %>").val("Save");

            $("#CreateNewDialog").dialog({
                autoOpen: true,
                modal: true,
                width: 1000,
                height: 500,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Create New Repair And Maintenance",
                show: 'slide'
            });
            return false;
        }
        function AttachFile() {
            $("#implementationDocuments").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Repair And Maintenance Documents",
                show: 'slide'
            });
        }
        function UploadComplete() {
            var randomId = $("#ContentPlaceHolder1_RandomDocId").val();
            ShowUploadedDocument(randomId);
        }

        function ShowUploadedDocument(randomId) {
            var id = $("#ContentPlaceHolder1_hfId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();
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

                DocTable += "<td align='left' style='width: 50%'>" + result[row].Name + "</td>";

                if (result[row].Path != "") {
                    imagePath = "<img src='" + result[row].Path + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                DocTable += "<td align='left' style='width: 30%'>" + imagePath + "</td>";

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
            var deletedDoc = $("#<%=hfDeletedDoc.ClientID %>").val();

            if (deletedDoc != "")
                deletedDoc += "," + docId;
            else
                deletedDoc = docId;

            $("#<%=hfDeletedDoc.ClientID %>").val(deletedDoc);

            $("#trdoc" + rowIndex).remove();
        }
        function OnGetUploadedDocByWebMethodFailed(error) {
            alert(error.get_message());
        }
        function SaveAndClose() {
            var id = $(<%=hfId.ClientID%>).val();
            var itemId = 0;
            var itemName = '';
            var maintenanceType = $("#<%=ddlMaintenanceType.ClientID%>").val();
            if (maintenanceType == 'Fixed Asset') {
                itemId = $("#<%=ddlFixedItem.ClientID%>").val();
                itemName = '';
            }
            else {
                itemId = 0;
                itemName = $("#<%=txtItemName.ClientID%>").val();
            }
            if (itemId == "0" && itemName == "") {
                toastr.warning("Enter Item Name");
                return false;
            }
            var details = $("#<%=txtDetails.ClientID%>").val();
            var maintenanceArea = $("#<%=ddlTransectionType.ClientID%>").val();
            var transectionId = $("#<%=ddlTransectionId.ClientID%>").val();
            if (transectionId == "0") {
                var transectionLbl = $("#<%=lblTransectionId.ClientID%>").text();
                if (maintenanceArea != "Lobby" || maintenanceArea != "Others") {
                    toastr.warning("Select " + transectionLbl);
                    $("#<%=ddlTransectionId.ClientID%>").focus();
                    return false;
                }
            }
            var isEmergency = $("#<%=ddlEmergencyType.ClientID%>").val() == '1' ? true : false;
            var expectedDate = $("#<%=txtExpectedDate.ClientID%>").val();
            if (expectedDate == "") {
                toastr.warning("Enter Expected Date");
                $("#<%=txtExpectedDate.ClientID%>").focus();
                return false;
            }
            expectedDate = CommonHelper.DateFormatToMMDDYYYY(expectedDate, '/');
            var expectedTime = $("#<%=txtExpectedTime.ClientID%>").val();
            if (expectedTime == "") {
                toastr.warning("Enter Expected Time");
                $("#<%=txtExpectedTime.ClientID%>").focus();
                return false;
            }
            expectedTime = moment(expectedTime, ["h:mm A"]).format("HH:mm");
            var requestedById = 0;
            var requestedByName = "";
            if ($("#<%=hfRequestedBy.ClientID%>").val() == "1") {
                requestedById = $("#<%=ddlRequestedBy.ClientID%>").val();
                requestedByName = "";
            }
            else {
                requestedById = "0";
                requestedByName = $("#<%=txtRequestedBy.ClientID%>").val();
            }
            if (requestedById == "0" && requestedByName == "") {
                toastr.warning("Enter Requested By");
                return false;
            }
            var HotelRepairNMaintenanceBO = {
                Id: id,
                MaintenanceType: maintenanceType,
                ItemName: itemName,
                ItemId: itemId,
                Details: details,
                MaintenanceArea: maintenanceArea,
                TransectionId: transectionId,
                IsEmergency: isEmergency,
                ExpectedDate: expectedDate,
                ExpectedTime: expectedTime,
                RequestedById: requestedById,
                RequestedByName: requestedByName,
            }
            var randomDocId = $("#ContentPlaceHolder1_RandomDocId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../HouseKeeping/RepairAndMaintenance.aspx/SaveOrUpdateRepairNMaintenance',

                data: JSON.stringify({ HotelRepairNMaintenanceBO: HotelRepairNMaintenanceBO, randomDocId: randomDocId, deletedDoc: deletedDoc }),
                dataType: "json",
                success: function (data) {
                    $("#ContentPlaceHolder1_RandomDocId").val(data.d.Data);
                    GridPaging(1, 1);
                    ChangeRandomId();
                    PerformClearAction();
                    CommonHelper.AlertMessage(data.d.AlertMessage);
                    if (flag == 1) {
                        $('#CreateNewDialog').dialog('close');
                    }
                    flag = 0;
                },
                error: function (result) {

                }
            });
            return false;
        }
        function PerformClearAction() {

            $(<%=hfId.ClientID%>).val("0");
            $("#<%=ddlMaintenanceType.ClientID%>").val('Fixed Asset');
            $("#<%=ddlFixedItem.ClientID%>").val("0").trigger('change');
            $("#<%=txtItemName.ClientID%>").val("");
            $("#<%=txtDetails.ClientID%>").val("");
            $("#<%=ddlTransectionType.ClientID%>").val('Lobby');
            $("#<%=ddlEmergencyType.ClientID%>").val("1");
            $("#<%=txtExpectedDate.ClientID%>").val("");
            $("#<%=txtExpectedTime.ClientID%>").val("");
            if ($("#<%=hfRequestedBy.ClientID%>").val() == "1") {
                requestedById = $("#<%=ddlRequestedBy.ClientID%>").val("0");
            }
            $("#<%=txtRequestedBy.ClientID%>").val("");

        }
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadGrid(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function LoadGrid(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#CNFTransactionTable tbody tr").length;
            var fromDate = $("#ContentPlaceHolder1_txtFromDateSrc").val();
            var toDate = $("#ContentPlaceHolder1_txtToDateSrc").val();
            var maintenanceType = $("#ContentPlaceHolder1_ddlMaintenanceTypeSrc").val();
            var itemId = $("#ContentPlaceHolder1_ddlFixedItemSrc").val();
            var itemName = $("#ContentPlaceHolder1_txtItemNameSrc").val();
            var transectionId = $("#ContentPlaceHolder1_ddlTransectionIdSrc").val();
            if (maintenanceType != "0") {
                if (maintenanceType == "Fixed Asset") {
                    itemName = "";
                }
                else {
                    itemId = 0;
                }
            }
            else {
                itemName = "";
                itemId = 0;
            }
            var maintenanceArea = $("#ContentPlaceHolder1_ddlTransectionTypeSrc").val();
            if (maintenanceArea == "0" || maintenanceArea == "Lobby" || maintenanceArea == "Others") {
                transectionId = 0;
            }
            var isEmergency = $("#ContentPlaceHolder1_ddlEmergencyTypeSrc").val();
            var requestedById = $("#ContentPlaceHolder1_ddlRequestedBySrc").val();
            var requestedByName = $("#ContentPlaceHolder1_txtRequestedBySrc").val();
            if ($("#<%=hfRequestedBy.ClientID%>").val() == "0") {
                requestedById = 0;
            }
            else
                requestedByName = "";
            if (fromDate == "") {
                fromDate = "01/01/1970";
            }
            if (toDate == "") {
                toDate = new Date();
                toDate = ((toDate.getMonth() > 8) ? (toDate.getMonth() + 1) : ('0' + (toDate.getMonth() + 1))) + '/' + ((toDate.getDate() > 9) ? toDate.getDate() : ('0' + toDate.getDate())) + '/' + toDate.getFullYear();

            }
            else
                toDate = CommonHelper.DateFormatToMMDDYYYY(toDate, '/');
            fromDate = CommonHelper.DateFormatToMMDDYYYY(fromDate, '/');


            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../HouseKeeping/RepairAndMaintenance.aspx/LoadRepairNMaintenance',

                data: JSON.stringify({
                    fromDate: fromDate, toDate: toDate, maintenanceType: maintenanceType, itemId: itemId, itemName: itemName,
                    maintenanceArea: maintenanceArea, isEmergency: isEmergency, transectionId: transectionId, requestedById: requestedById,
                    requestedByName: requestedByName, gridRecordsCount: gridRecordsCount, pageNumber: pageNumber, IsCurrentOrPreviousPage: IsCurrentOrPreviousPage
                }),
                dataType: "json",
                success: function (data) {
                    LoadTable(data);
                },
                error: function (result) {
                    PerformClearAction();
                }
            });
            return false;
        }
        function LoadTable(data) {

            $("#RepairNMaintenanceTable tbody").empty();
            $("#GridPagingContainer ul").empty();
            i = 0;

            $.each(data.d.GridData, function (count, gridObject) {

                var tr = "";

                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }
                var itemName = gridObject.MaintenanceType == "Fixed Asset" ? gridObject.FixedItemName : gridObject.ItemName;
                tr += "<td style='width:15%;text-align: left'>" + itemName + "</td>";
                tr += "<td style='width:12%;text-align: center'>" + moment(gridObject.CreateDate).format("DD/MM/YYYY") + "</td>";
                tr += "<td style='width:12%;text-align: center'>" + moment(gridObject.ExpectedDate).format("DD/MM/YYYY") + "</td>";
                tr += "<td style='width:15%;text-align: center '>" + moment(gridObject.ExpectedTime).format('hh:mm A') + "</td>";
                tr += "<td style='width:15%;text-align: center'>" + gridObject.MaintenanceArea + "</td>";
                var IsEmergency = gridObject.IsEmergency == true ? "Emergency" : "Not Emergency"
                tr += "<td style='width:15%;'>" + IsEmergency + "</td>";

                tr += "<td style=\"text-align: center; width:16%; cursor:pointer;\">";
                tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return FillFormEdit(" + gridObject.Id + ");\" alt='Edit'  title='Edit' border='0' />";
                tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return DeleteAction(" + gridObject.Id + ");\" alt='Delete'  title='Delete' border='0' />";
                tr += "&nbsp;&nbsp;<img src='../Images/RepainNMaintenance.png' onClick= \"javascript:return TaskAssign(" + gridObject.Id + ");\" alt='Delete'  title='Delete' border='0'  width=\"16\" height=\"16\" />";

                //if (gridObject.IsCanEdit) {
                //    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return FillFormEdit(" + gridObject.Id + ");\" alt='Edit'  title='Edit' border='0' />";
                //}

                //if (gridObject.IsCanDelete) {
                //    tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return DeleteTransaction(" + gridObject.Id + ");\" alt='Delete'  title='Delete' border='0' />";
                //}

                //if (gridObject.IsCanChecked) {
                //    tr += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return TransactionApprovalWithConfirmation('" + 'Checked' + "'," + gridObject.Id + ")\" alt='Checked'  title='Checked' border='0' />";

                //}

                //if (gridObject.IsCanApproved) {
                //    tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return TransactionApprovalWithConfirmation('" + 'Approved' + "', " + gridObject.Id + ")\" alt='Approved'  title='Approved' border='0' />";
                //}
                tr += "</td>";
                tr += "<td style='display:none'>" + gridObject.Id + "</td>";


                tr += "</tr>";

                $("#RepairNMaintenanceTable tbody").append(tr);

                tr = "";
                i++;
            });
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(data.d.GridPageLinks.NextButton);
            return false;
        }
        function FillFormEdit(id) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../HouseKeeping/RepairAndMaintenance.aspx/FillForm',

                data: "{ 'id':'" + id + "'}",
                dataType: "json",
                success: function (data) {
                    $("#CreateNewDialog").dialog({
                        autoOpen: true,
                        modal: true,
                        width: '75%',
                        closeOnEscape: true,
                        resizable: false,
                        height: 'auto',
                        fluid: true,
                        title: "Create New Repair And Maintenance",
                        show: 'slide'
                    });
                    $(<%=hfId.ClientID%>).val(data.d.Id);
                    $("#<%=ddlMaintenanceType.ClientID%>").val(data.d.MaintenanceType);
                    $("#<%=ddlFixedItem.ClientID%>").val(data.d.ItemId).trigger('change');
                    $("#<%=txtItemName.ClientID%>").val(data.d.ItemName);
                    $("#<%=txtDetails.ClientID%>").val(data.d.Details);
                    $("#<%=ddlTransectionType.ClientID%>").val(data.d.MaintenanceArea).trigger('change');
                    if (data.d.MaintenanceArea != "Lobby" || data.d.MaintenanceArea != "Others") {
                        $("#<%=ddlTransectionId.ClientID%>").val(data.d.TransectionId);
                    }
                    $("#<%=ddlEmergencyType.ClientID%>").val(data.d.isEmergency == true ? "1" : "0");
                    $("#<%=txtExpectedDate.ClientID%>").val(moment(data.d.ExpectedDate).format("DD/MM/YYYY"));
                    $("#<%=txtExpectedTime.ClientID%>").val(moment(data.d.ExpectedTime).format("hh:mm A"));
                    if ($("#<%=hfRequestedBy.ClientID%>").val() == "1") {
                        requestedById = $("#<%=ddlRequestedBy.ClientID%>").val(data.d.RequestedById);
                    }
                    $("#<%=txtRequestedBy.ClientID%>").val(data.d.RequestedByName);
                    UploadComplete();
                },
                error: function (result) {
                    PerformClearAction();
                }
            });
            return false;
        }
        function ChangeRandomId() {
            //$('#DocumentInfo tbody tr').each(function (i, row) {
            //    $(this).find("td:eq(2) img").trigger('click')
            //});

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../HouseKeeping/RepairAndMaintenance.aspx/ChangeRandomId',
                dataType: "json",
                async: false,
                success: function (data) {
                    $("#ContentPlaceHolder1_RandomDocId").val(data.d);
                },
                error: function (error) {
                }
            });
        }
        function DeleteAction(id) {
            if (!confirm("Do you want to Delete?")) {
                return false;
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../HouseKeeping/RepairAndMaintenance.aspx/DeleteAction',
                data: "{'Id':'" + id + "'}",
                dataType: "json",
                success: function (data) {
                    CommonHelper.AlertMessage(data.d.AlertMessage);
                    GridPaging($("#GridPagingContainer").find("li.active").index(), 1);
                },
                error: function (result) {

                }
            });
            return false;
        }
        function TaskAssign(Id) {
            if (!confirm("Want to Assign Employee?")) {
                return false;
            }
            var iframeid = 'frmPrint';
            var url = "../TaskManagement/AssignTaskIFrame.aspx?tid=" + Id + "&tType=Internal";
            document.getElementById(iframeid).src = url;
            $("#SalesNoteDialog").dialog({
                autoOpen: true,
                modal: true,
                width: "82%",
                height: 600,
                closeOnEscape: false,
                resizable: false,
                fluid: true,
                title: "Task Assign",
                show: 'slide'
            });
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }
        function CloseDialog() {
            $("#SalesNoteDialog").dialog('close');
            return false;
        }
    </script>

    <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="tempDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfParentDoc" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />

    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>

    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfRequestedBy" runat="server" Value="0"></asp:HiddenField>
    <div id="SalesNoteDialog" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <div id="implementationDocuments" style="display: none;">
        <label for="Attachment" class="control-label col-md-2">
            Attachment</label>
        <div class="col-md-4">
            <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
                <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                    FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
            </asp:Panel>
        </div>
    </div>
    <div id="InfoPanel" class="panel panel-default">
        <div class="panel-heading">
            Repair & Maintenance Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label runat="server" class="control-label " Text="Maintenance Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlMaintenanceTypeSrc" runat="server" CssClass="form-control">
                            <asp:ListItem Text="--- ALL ---" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Fixed Asset" Value="Fixed Asset"></asp:ListItem>
                            <asp:ListItem Text="Non Fixed Asset" Value="Non Fixed Asset"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div id="nonFixedAssetSrc">
                        <div class="col-md-2">
                            <asp:Label runat="server" class="control-label" Text="Item Name"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtItemNameSrc" runat="server" CssClass="form-control">                            
                            </asp:TextBox>
                        </div>
                    </div>
                    <div id="fixedAssetSrc">
                        <div class="col-md-2">
                            <asp:Label runat="server" class="control-label" Text="Item Name"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlFixedItemSrc" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label runat="server" class="control-label" Text="Maintenance Area"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTransectionTypeSrc" runat="server" CssClass="form-control" TabIndex="1">
                            <asp:ListItem Text="--- ALL ---" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Lobby" Value="Lobby"></asp:ListItem>
                            <asp:ListItem Text="Room" Value="Room"></asp:ListItem>
                            <asp:ListItem Text="Restaurant" Value="Restaurant"></asp:ListItem>
                            <asp:ListItem Text="Banquet" Value="Banquet"></asp:ListItem>
                            <asp:ListItem Text="Others" Value="Others"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="divRoomRestSrc" style="display: none">
                        <div>
                            <asp:Label ID="lblTransectionIdSrc" runat="server" class="control-label col-md-2" Text="Room/Restaurant"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlTransectionIdSrc" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDateSrc" runat="server" class="control-label" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFromDateSrc" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblToDateSrc" runat="server" class="control-label" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtToDateSrc" CssClass="form-control" runat="server" autoComplete="off"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label" Text="Emergency Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlEmergencyTypeSrc" runat="server" CssClass="form-control">
                            <asp:ListItem Text="--- All ---" Value="All"></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label runat="server" class="control-label" Text="Requested By"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtRequestedBySrc" runat="server" CssClass="form-control">                            
                        </asp:TextBox>
                        <asp:DropDownList ID="ddlRequestedBySrc" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="javascript: return GridPaging(1,1);"
                            CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />
                        <asp:Button ID="btnAdd" runat="server" Text="New Repair & Maintenance" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateRepairAndMaintenance();" TabIndex="6" />
                    </div>
                </div>
            </div>
        </div>
        <div id="SearchPanel" class="panel panel-default">
            <div class="panel-heading">
                Search Information
            </div>
            <div class="panel-body">
                <div class="form-group" id="RepairNMaintenanceContainer" style="overflow: scroll;">
                    <table class="table table-bordered table-condensed table-responsive" id="RepairNMaintenanceTable"
                        style="width: 100%;">
                        <thead>
                            <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                <th style="width: 15%; text-align: center">Item Name
                                </th>
                                <th style="width: 12%; text-align: center">Request Date
                                </th>
                                <th style="width: 12%; text-align: center">Expected Finished Date
                                </th>
                                <th style="width: 15%; text-align: center">Expected Finished Time
                                </th>
                                <th style="width: 15%; text-align: center">Maintenance Area
                                </th>
                                <th style="width: 15%; text-align: center">Emergency Type
                                </th>
                                <th style="width: 16%; text-align: center">Action
                                </th>
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
    </div>
    <div id="CreateNewDialog" style="display: none">
        <div id="AddPanel" class="panel panel-default">
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label runat="server" class="control-label " Text="Maintenance Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlMaintenanceType" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Fixed Asset" Value="Fixed Asset"></asp:ListItem>
                                <asp:ListItem Text="Non Fixed Asset" Value="Non Fixed Asset"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div id="nonFixedAsset">
                            <div class="col-md-2">
                                <asp:Label runat="server" class="control-label" Text="Item Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtItemName" runat="server" CssClass="form-control">                            
                                </asp:TextBox>
                            </div>
                        </div>
                        <div id="fixedAsset">
                            <div class="col-md-2">
                                <asp:Label runat="server" class="control-label" Text="Item Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlFixedItem" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label runat="server" class="control-label" Text="Details"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtDetails" runat="server" Rows="4" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label runat="server" class="control-label" Text="Maintenance Area"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlTransectionType" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Lobby" Value="Lobby"></asp:ListItem>
                                <asp:ListItem Text="Room" Value="Room"></asp:ListItem>
                                <asp:ListItem Text="Restaurant" Value="Restaurant"></asp:ListItem>
                                <asp:ListItem Text="Banquet" Value="Banquet"></asp:ListItem>
                                <asp:ListItem Text="Others" Value="Others"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div id="divRoomRest" style="display: none">
                            <div>
                                <asp:Label ID="lblTransectionId" runat="server" class="control-label col-md-2" Text="Room/Restaurant"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlTransectionId" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label1" runat="server" class="control-label" Text="Emergency Type"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlEmergencyType" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblExpectedDate" runat="server" class="control-label" Text="Expected Date"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtExpectedDate" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblExpectedTime" runat="server" class="control-label" Text="Expected Time"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtExpectedTime" CssClass="form-control" runat="server" autoComplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
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
                        <div class="col-md-2">
                            <asp:Label runat="server" class="control-label" Text="Requested By"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtRequestedBy" runat="server" CssClass="form-control">                            
                            </asp:TextBox>
                            <asp:DropDownList ID="ddlRequestedBy" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnSaveClose" runat="server" Text="Save" OnClientClick="javascript:return SaveAndClose();"
                                CssClass="TransactionalButton btn btn-primary btn-sm" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript: return PerformClearAction();" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
