<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="HKLostFound.aspx.cs" Inherits="HotelManagement.Presentation.Website.HouseKeeping.HKLostFound" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var payrollIntegrated = "0";
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false, IsCanView = false;
        $(document).ready(function () {
            CommonHelper.ApplyIntigerValidation();
            GridPaging(1, 1);
            payrollIntegrated = $("#<%=hfIsPayrollIntegrateWithFrontOffice.ClientID %>").val();
            if (payrollIntegrated == "1") {
                //// employeeLoadDivSrc employeeTxtDivSrc
                $("#employeeLoadDivSrc").show("slow");
                $("#employeeLoadDiv").show("slow");

                $("#employeeTxtDivSrc").hide("slow");
                $("#employeeTxtDiv").hide("slow");
            }
            else {
                $("#employeeLoadDivSrc").hide("slow");
                $("#employeeLoadDiv").hide("slow");

                $("#employeeTxtDivSrc").show("slow");
                $("#employeeTxtDiv").show("slow");
            }
            // ContentPlaceHolder1_ddlFoundPerson
            // ContentPlaceHolder1_ddlTransectionId
            $("#ContentPlaceHolder1_ddlFoundPersonSrc").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlFoundPerson").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlTransectionId").select2({
                tags: "true",
                //placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#<%=txtFoundDate.ClientID %>").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });
            $("#<%=txtFoundDateSrc.ClientID %>").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            });

            $("#<%=txtFoundTime.ClientID%>").timepicker({
                showPeriod: is12HourFormat,
            });
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;

            $("#<%=ddlTransectionType.ClientID%>").change(function () {
                var type = $("#ContentPlaceHolder1_ddlTransectionType").val();
                ChangeTransectionType(type, 1);
            });
            $("#<%=ddlTransectionTypeSrc.ClientID%>").change(function () {
                var type = $("#ContentPlaceHolder1_ddlTransectionTypeSrc").val();
                ChangeTransectionType(type, 0);
            });

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
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            var gridRecordsCount = $("#LostFoundTable tbody tr").length;
            var itemNameSrc = $("#<%=txtItemNameSrc.ClientID %>").val();
            var itemTypeSrc = $("#<%=ddlItemTypeSrc.ClientID %>").val();
            var transectionTypeSrc = $("#<%=ddlTransectionTypeSrc.ClientID %>").val();
            var transectionIdSrc = $("#<%=ddlTransectionIdSrc.ClientID %>").val();
            var foundDateSrc = $("#<%=txtFoundDateSrc.ClientID %>").val();
            var foundPersonId = $("#<%=ddlFoundPersonSrc.ClientID %>").val();
            var foundPersonName = $("#<%=txtFoundPersonSrc.ClientID %>").val();
            if (foundPersonId == null) {
                foundPersonId = 0;
            }
            if (transectionIdSrc == null) {
                transectionIdSrc = 0;
            }
            if (foundDateSrc != "") {
                foundDateSrc = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(foundDateSrc, innBoarDateFormat);
            }

            PageMethods.SearchGridPaging(itemNameSrc, itemTypeSrc, transectionTypeSrc, transectionIdSrc, foundDateSrc, foundPersonId, foundPersonName, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSearchSucceed, OnFailed);
            return false;
        }
        function OnSearchSucceed(searchData) {
            $("#LostFoundTable tbody").empty();
            $("#GridPagingContainer ul").empty();
            var i = 0;

            if (searchData.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#LostFoundTable tbody ").append(emptyTr);
                return false;
            }
            $.each(searchData.GridData, function (count, gridObject) {
                var tr = "";

                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }

                tr += "<td style='width:20%;'>" + gridObject.ItemName + "</td>";
                tr += "<td style='width:20%;'>" + CommonHelper.DateFromDateTimeToDisplay(gridObject.FoundDateTime, innBoarDateFormat) + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.TransectionType + "</td>";
                tr += "<td style='width:20%;'>" + (gridObject.HasItemReturned == true ? "Item Returned" : "") + "</td>";

                tr += "<td style='width:20%;'> <a onclick=\"javascript:return FillFormEdit(" + gridObject.Id + ",\'" + gridObject.ItemName + '\');"' + "title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                tr += "&nbsp;&nbsp;<a onclick=\"javascript:return ReturnItem(" + gridObject.Id + ",\'" + gridObject.ItemName + '\');"' + "title='Return' href='javascript:void();'><img src='../Images/return.png' alt='Return'></a>"

                tr += "&nbsp;&nbsp;<a title='Delete' href='#' onclick= 'DeleteData(" + gridObject.Id + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                tr += "</td>";

                tr += "<td style='display:none'>" + gridObject.Id + "</td>";
                tr += "</tr>";

                $("#LostFoundTable tbody").append(tr);

                tr = "";
                i++;
            });
            //PerformCancleAction();
            $("#GridPagingContainer ul").append(searchData.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(searchData.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(searchData.GridPageLinks.NextButton);
            return false;
        }
        function DeleteData(id) {
            if (!confirm("Do you want to delete?")) {
                return;
            }
            PageMethods.DeleteData(id, OnDeleteSucceed, OnFailed);
            return false;
        }
        function OnDeleteSucceed(result) {
            if (result.IsSuccess == true) {
                GridPaging(1, 1);
                CommonHelper.AlertMessage(result.AlertMessage);

            }
            return false;
        }
        function FillFormEdit(id, name) {
            if (!confirm("Do you want to edit ?")) {
                return false;
            }
            $("#AddNewDiv").dialog({
                autoOpen: true,
                modal: true,
                width: 1100,
                height: 600,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Update - " + name,
                show: 'slide'
            });
            CommonHelper.SpinnerOpen();
            PageMethods.FillForm(id, OnFillFormSucceed, OnFailed);
            return false;
        }
        function OnFillFormSucceed(result) {
            $("#<%=btnSaveClose.ClientID %>").val("Update");
            CommonHelper.SpinnerClose();
            var time = result.FoundDateTime.getHours() + ":" + result.FoundDateTime.getMinutes();
            ChangeTransectionType(result.TransectionType, 1);
            $("#<%=txtDescription.ClientID %>").val(result.Description);
            $("#<%=txtFoundDate.ClientID %>").val(CommonHelper.DateFromDateTimeToDisplay(result.FoundDateTime, innBoarDateFormat));
            $("#<%=txtFoundTime.ClientID %>").val(time);
            $("#<%=txtItemName.ClientID %>").val(result.ItemName);
            $("#<%=txtOtherArea.ClientID %>").val(result.OtherArea);
            $("#<%=txtFoundPerson.ClientID %>").val(result.WhoFoundItName);
            $("#<%=ddlTransectionId.ClientID %>").val(result.TransectionId).trigger("change");
            $("#<%=ddlItemType.ClientID %>").val(result.ItemType);
            $("#<%=ddlTransectionType.ClientID %>").val(result.TransectionType);
            $("#<%=ddlFoundPerson.ClientID %>").val(result.WhoFoundItId).trigger("change");
            $("#<%=hfId.ClientID %>").val(result.Id);
            ShowUploadedDocument($("#ContentPlaceHolder1_RandomDocId").val());
            return false;
        }
        function CreateLostFound() {
            PerformClearAction();
            $("#<%=btnSaveClose.ClientID %>").val("Save");

            $("#AddNewDiv").dialog({
                autoOpen: true,
                modal: true,
                width: 1000,
                height: 500,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Create New Lost and Found",
                show: 'slide'
            });
            return false;
        }
        function SaveAndClose() {
            var itemName = $("#<%=txtItemName.ClientID %>").val();
            var description = $("#<%=txtDescription.ClientID %>").val();
            var transectionType = $("#<%=ddlTransectionType.ClientID %>").val();
            var transectionId = $("#<%=ddlTransectionId.ClientID %>").val();
            var otherArea = $("#<%=txtOtherArea.ClientID %>").val();
            var itemType = $("#<%=ddlItemType.ClientID %>").val();
            var Date = $("#<%=txtFoundDate.ClientID %>").val();
            var Time = $("#<%=txtFoundTime.ClientID %>").val();
            var whoFoundItName = $("#<%=txtFoundPerson.ClientID %>").val();
            var whoFoundItId = $("#<%=ddlFoundPerson.ClientID %>").val();
            var id = $("#<%=hfId.ClientID %>").val();

            if (transectionType == "0") {
                toastr.warning("Please Select Where it Found");
                $("#<%=ddlTransectionType.ClientID %>").focus();
                return false;
            }
            else if (itemName == "") {
                toastr.warning("Please Insert Item Name");
                $("#<%=txtItemName.ClientID %>").focus();
                return false;
            }
            else if (itemType == "") {
                toastr.warning("Please Select Item Type");
                $("#<%=ddlItemType.ClientID %>").focus();
                return false;
            }
            else if (Date == "") {
                toastr.warning("Please Insert Found Date");
                $("#<%=ddlTransectionType.ClientID %>").focus();
                return false;
            }
            else if (whoFoundItId == "0" && payrollIntegrated) {
                toastr.warning("Please Select Who Found It");
                $("#<%=ddlFoundPerson.ClientID %>").focus();
                return false;
            }
            else if (payrollIntegrated == false && whoFoundItName == "") {
                toastr.warning("Please Insert Who Found It");
                $("#<%=txtFoundPerson.ClientID %>").focus();
                return false;
            }
            else if ((transectionType == "Room" || transectionType == "Restaurant" || transectionType == "Banquet") && transectionId == "0") {
                toastr.warning("Please Select Room / Restaurant");
                $("#<%=ddlTransectionId.ClientID %>").focus();
                return false;
            }

            if (Date != "") {
                Date = CommonHelper.DateFormatMMDDYYYYFromDDMMYYYY(Date, innBoarDateFormat);
            }
            if (Time != "") {
                var foundDateTime = Date + " " + Time;
            }
            var LostFoundBO = {
                Id: id,
                ItemName: itemName,
                Description: description,
                ItemType: itemType,
                FoundDateTime: foundDateTime,
                TransectionType: transectionType,
                TransectionId: transectionId,
                OtherArea: otherArea,
                WhoFoundIt: whoFoundItId,
                WhoFoundItName: whoFoundItName,
            }
            var hfRandom = $("#<%=RandomDocId.ClientID %>").val();
            var deletedDocuments = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            CommonHelper.SpinnerOpen();
            PageMethods.SaveUpdate(LostFoundBO, hfRandom, deletedDocuments, OnSaveSucceed, OnFailed);
            return false;
        }
        function OnSaveSucceed(result) {
            if (result.IsSuccess == true) {
                CommonHelper.SpinnerClose();
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
                $("#AddNewDiv").dialog('close');
                ChangeRandomId();
                GridPaging(1, 1);

            }
            return false;
        }
        function ShowAlert(Message) {
            CommonHelper.AlertMessage(Message);
        }
        function CloseReturnDialog() {
            $('#ReturnDialogue').dialog('close');
            return false;
        }
        function ReturnItem(id, name) {
            if (!confirm("Do you want to Return ?")) {
                return false;
            }
            var iframeid = 'frmPrint';
            var url = "./HKLostFoundReturnIframe.aspx?lid=" + id;
            parent.document.getElementById(iframeid).src = url;

            $("#ReturnDialogue").dialog({
                autoOpen: true,
                modal: true,
                width: 1100,
                height: 550,
                minWidth: 600,
                minHeight: 300,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Return - " + name,
                show: 'slide'
            });

            return false;
        }
        <%--function ReturnItem(id, name) {
            $("#<%=hfId.ClientID %>").val(id);
            $("#ReturnDiv").dialog({
                autoOpen: true,
                modal: true,
                width: 1000,
                height: 500,
                minWidth: 550,
                minHeight: 580,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Return " + name,
                show: 'slide'
            });
            return false;
        }--%>

        function PerformClearAction() {

            $('#DocumentInfo').html("");

            ChangeTransectionType("0", 1);
            $("#<%=btnSaveClose.ClientID %>").val("Save");

            $("#<%=txtDescription.ClientID %>").val("");
            $("#<%=txtFoundDate.ClientID %>").val("");
            $("#<%=txtFoundTime.ClientID %>").val("");
            $("#<%=txtItemName.ClientID %>").val("");
            $("#<%=txtOtherArea.ClientID %>").val("");
            $("#<%=txtFoundPerson.ClientID %>").val("");
            $("#<%=ddlTransectionId.ClientID %>").val("0").trigger("change");
            $("#<%=ddlItemType.ClientID %>").val("0");
            $("#<%=ddlTransectionType.ClientID %>").val("0");
            $("#<%=ddlFoundPerson.ClientID %>").val("0").trigger("change");
            $("#<%=hfId.ClientID %>").val("0");
            $("#<%=hfIsPayrollIntegrateWithFrontOffice.ClientID %>").val("0");

            return false;
            //IsPayrollIntegrateWithFrontOffice
        }

        function OnFailed(error) {
            toastr.warning(error);
            return false;
        }

        //Documents 
        function ChangeRandomId() {
            //$('#DocumentInfo tbody tr').each(function (i, row) {
            //    $(this).find("td:eq(2) img").trigger('click')
            //});

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './HKLostFound.aspx/ChangeRandomId',
                dataType: "json",
                async: false,
                success: function (data) {
                    $("#ContentPlaceHolder1_RandomDocId").val(data.d);
                },
                error: function (error) {
                }
            });
        }
        function AttachFile() {
            $("#popUpImage").dialog({
                width: 650,
                height: 300,
                autoOpen: true,
                modal: true,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "Lost Item Documets", // TODO add title
                show: 'slide'
            });
            return false;
        }
        function UploadComplete() {
            var randomId = $("#ContentPlaceHolder1_RandomDocId").val();
            ShowUploadedDocument(randomId);
        }

        function ShowUploadedDocument(randomId) {
            debugger;
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
    </script>

    <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="tempDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfParentDoc" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="" />

    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsPayrollIntegrateWithFrontOffice" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>
    <div id="ReturnDialogue" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>

    <div id="popUpImage" style="display: none">
        <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
            <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
        </asp:Panel>
    </div>
    <div id="InfoPanel" class="panel panel-default">
        <div class="panel-heading">
            Lost & Found Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label runat="server" class="control-label " Text="Item Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtItemNameSrc" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label runat="server" class="control-label" Text="Where It Found"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlTransectionTypeSrc" runat="server" CssClass="form-control" TabIndex="1">
                            <asp:ListItem Text="-- Please Select --" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Lobby" Value="Lobby"></asp:ListItem>
                            <asp:ListItem Text="Room" Value="Room"></asp:ListItem>
                            <asp:ListItem Text="Restaurant" Value="Restaurant"></asp:ListItem>
                            <asp:ListItem Text="Banquet" Value="Banquet"></asp:ListItem>
                            <asp:ListItem Text="Others" Value="Others"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="divRoomRestSrc" style="display: none">
                        <div>
                            <asp:Label ID="lblTransectionIdSrc" runat="server" class="control-label col-md-2"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlTransectionIdSrc" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>

                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Found Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFoundDateSrc" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="Label2" runat="server" class="control-label" Text="Item Type"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlItemTypeSrc" runat="server" CssClass="form-control" TabIndex="1">
                            <asp:ListItem Text="-- Please Select --" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Valuable" Value="Valuable"></asp:ListItem>
                            <asp:ListItem Text="Non-Valuable" Value="NonValuable"></asp:ListItem>
                            <asp:ListItem Text="Perishable" Value="Perishable"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label3" runat="server" class="control-label" Text="Who Found It"></asp:Label>
                    </div>
                    <div class="col-md-10" id="employeeLoadDivSrc" style="display: none">
                        <asp:DropDownList ID="ddlFoundPersonSrc" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-10" id="employeeTxtDivSrc">
                        <asp:TextBox ID="txtFoundPersonSrc" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                    </div>
                </div>
                &nbsp;
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="javascript: return GridPaging(1,1);"
                            CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />
                        <asp:Button ID="btnAdd" runat="server" Text="New Lost & Found" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateLostFound();" TabIndex="6" />
                    </div>
                </div>
            </div>
        </div>
        <div id="SearchPanel" class="panel panel-default">
            <div class="panel-heading">
                Search Information
            </div>
            <div class="panel-body">
                <div class="form-group" id="LostFoundTableContainer" style="overflow: scroll;">
                    <table class="table table-bordered table-condensed table-responsive" id="LostFoundTable"
                        style="width: 100%;">
                        <thead>
                            <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                                <th style="width: 20%;">Item Name
                                </th>
                                <th style="width: 20%;">Found Date
                                </th>
                                <th style="width: 20%;">Where It Found
                                </th>
                                <th style="width: 20%;">Return Info.
                                </th>
                                <th style="width: 20%;">Action
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
    <div id="AddNewDiv" style="display: none">
        <div id="AddPanel" class="panel panel-default">
            <%--<div class="panel-heading">
                New CNF
            </div>--%>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label runat="server" class="control-label required-field" Text="Where It Found"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlTransectionType" runat="server" CssClass="form-control" TabIndex="1">
                                <asp:ListItem Text="-- Please Select --" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Lobby" Value="Lobby"></asp:ListItem>
                                <asp:ListItem Text="Room" Value="Room"></asp:ListItem>
                                <asp:ListItem Text="Restaurant" Value="Restaurant"></asp:ListItem>
                                <asp:ListItem Text="Banquet" Value="Banquet"></asp:ListItem>
                                <asp:ListItem Text="Others" Value="Others"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div id="divRoomRest" style="display: none">
                            <div>
                                <asp:Label ID="lblTransectionId" runat="server" class="control-label col-md-2"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlTransectionId" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-group" id="otherAreaDiv" style="display: none">
                        <div class="col-md-2">
                            <label class="control-label ">Others Area Name</label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtOtherArea" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Item Name</label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtItemName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Item Description</label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtDescription" CssClass="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Item Type</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlItemType" runat="server" CssClass="form-control" TabIndex="1">
                                <asp:ListItem Text="-- Please Select --" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Valuable" Value="Valuable"></asp:ListItem>
                                <asp:ListItem Text="Non-Valuable" Value="NonValuable"></asp:ListItem>
                                <asp:ListItem Text="Perishable" Value="Perishable"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Found Date</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtFoundDate" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label ">Found Time</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtFoundTime" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                    <%--<div class="form-group">
                        

                    </div>--%>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label for="Attachment" class="control-label">Attachment</label>
                        </div>
                        <div class="col-md-10">
                            <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Attach" onclick="AttachFile()" />
                        </div>
                    </div>
                    <div id="DocumentInfo">
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label required-field">Who Found It</label>
                        </div>
                        <div class="col-md-10" id="employeeLoadDiv" style="display: none">
                            <asp:DropDownList ID="ddlFoundPerson" runat="server" CssClass="form-control" TabIndex="1">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-10" id="employeeTxtDiv">
                            <asp:TextBox ID="txtFoundPerson" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>

                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnSaveClose" runat="server" Text="Save" OnClientClick="javascript:return SaveAndClose();"
                                CssClass="TransactionalButton btn btn-primary btn-sm" />
                            <%--<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                OnClientClick="javascript: return PerformClearAction();" />--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
