<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="Deal.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.Deal" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var ProductInformation;
        var ContactList = [];
        var flagValidation = 0;
        $(document).ready(function () {
            var companyId = $.trim(CommonHelper.GetParameterByName("cid"));
            var dealId = $.trim(CommonHelper.GetParameterByName("did"));
            var contactId = $.trim(CommonHelper.GetParameterByName("conid"));

            $("#ContentPlaceHolder1_ddlIndependentContact").change(function () {

                var contactId = $(this).val();
                if (parseInt(contactId) > 0)
                    LoadDealTypeAsLifeCycleStage(0, contactId);
            });

            if (contactId != "") {
                ContactList.push(parseInt(contactId));
                if (companyId == "" || companyId == "0")
                    $("#ContentPlaceHolder1_ddlIndependentContact").val(ContactList).trigger('change');
            }

            if (dealId != "") {
                FillForm(dealId);
            }
            CommonHelper.ApplyDecimalValidation();
            
            $("#ContentPlaceHolder1_ddlDealOwner").select2({
                tags: false,
                allowClear: true,
                width: "99.75%",
            });
            $("#ContentPlaceHolder1_ddlCompany").select2({
                tags: false,
                allowClear: true,
                width: "99.75%",
            });
            $("#ContentPlaceHolder1_ddlIndependentContact").select2({
                tags: false,
                allowClear: true,
                width: "99.75%",
            });

            $("#ContentPlaceHolder1_ddlCompany").change(function () {
                var companyId = parseInt($("#ContentPlaceHolder1_ddlCompany").val().trim());

                //if (companyId == 0) {
                //    toastr.info("Please Select a Company.");
                //    return false;
                //}
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "../../../SalesAndMarketing/Deal.aspx/GetEmployeeByCompanyId",
                    dataType: "json",
                    data: JSON.stringify({ companyId: companyId }),
                    async: false,
                    success: (data) => {
                        OnLoadGetEmployeesSucceed(data.d);
                    },
                    error: (error) => {
                        toastr.error(error, "", { timeOut: 5000 });
                    }
                });

                LoadDealTypeAsLifeCycleStage(companyId, 0);
                return false;

            });

            $("#chkIsSaveUnderCompany").on('change', function () {
                SaveUnderCompany(this);
            });

            SaveUnderCompany($("#chkIsSaveUnderCompany"));

            if (companyId != "" && companyId != "0" && (dealId == "" || dealId == "0")) {
                $("#chkIsSaveUnderCompany").attr("checked", true).attr('disabled', true).trigger('change');
                $("#ContentPlaceHolder1_ddlCompany").val(companyId).attr('disabled', true).trigger('change');
            }
            else if (contactId != "" && contactId != "0" && (dealId == "" || dealId == "0")) {
                $("#chkIsSaveUnderCompany").attr("checked", false).attr('disabled', true).trigger('change');
            }
            $("#ContentPlaceHolder1_txtStartDate").datepicker({
                //defaultDate: "+1w",
                changeMonth: true,
                changeYear: true,
                defaultDate: DayOpenDate,
                dateFormat: innBoarDateFormat,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtEndDate").datepicker("option", "minDate", selectedDate);

                    var strDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtStartDate").val(), '/');
                    minEndDate = GetStringFromDateTime(CommonHelper.DaysAdd(strDate, 1));

                    $("#ContentPlaceHolder1_txtEndDate").datepicker("option", {
                        minDate: minEndDate
                    });
                }
            }).datepicker("setDate", DayOpenDate);

            $("#ContentPlaceHolder1_txtEndDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat,
                minDate: DayOpenDate,
                onClose: function (selectedDate) {
                    $("#ContentPlaceHolder1_txtStartDate").datepicker("option", "maxDate", selectedDate);

                }
            }).datepicker("setDate", DayOpenDate);

            ProductInformation = $("ProductInformation").DataTable({
                columns: [{ title: "Item Name", data: "QuotationNo", width: "20%" },
                { title: "Stock By", data: "DealName", width: "20%" },
                { title: "Action", data: null, width: "20%" },
                { title: "", data: "QuotationId", visible: false }]
            });

            $('#ContentPlaceHolder1_ddlContacts').select2();
            //Company Project Load
           <%-- var glCompanyId = $("#ContentPlaceHolder1_hfGLCompanyId").val();
            var glProjectId = $("#ContentPlaceHolder1_hfGLProjectId").val();
             if (glCompanyId != "0") {
                PopulateProjects(glCompanyId);
            }
            if (glProjectId != "0") {
                $("#<%=ddlGLProject.ClientID %>").val(glProjectId);
            }--%>

           <%-- var single = $("#<%=hfIsSingle.ClientID %>").val();--%>
            //if (single == "1") {
            //    $('#CompanyProjectPanel').hide();
            //}
            //else {
            //    $('#CompanyProjectPanel').show();
            //}
            <%--$("#ContentPlaceHolder1_ddlGLCompany").change(function () {
                var glCompanyId = $("#<%=ddlGLCompany.ClientID %>").val();
                if (glCompanyId != 0) {
                    PopulateProjects(glCompanyId);
                }
            });
            $("#ContentPlaceHolder1_ddlGLProject").change(function () {
                var id = $("#<%=ddlGLProject.ClientID %>").val();
                if (id != 0) {
                    $("#<%=hfGLProjectId.ClientID %>").val(id);
                }
            });--%>
        });

        function SaveUnderCompany(control) {
            if ($(control).is(":checked")) {
                $("#dvCompany").show();
                $("#dvContacts").show();
                $("#dvIndepentContacts").hide();
            }
            else {
                $("#dvCompany").hide();
                $("#dvContacts").hide();
                $("#dvIndepentContacts").show();
                var contactId = $("#ContentPlaceHolder1_ddlIndependentContact").val();
                if (ContactList.length == 0)
                    $("#ContentPlaceHolder1_ddlIndependentContact").val($("#ContentPlaceHolder1_ddlIndependentContact option:first").val()).trigger('change');
            }
        }

        function LoadDealTypeAsLifeCycleStage(companyId, contactId) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "../../../SalesAndMarketing/Deal.aspx/CheckCompanyIsClient",
                dataType: "json",
                data: JSON.stringify({ companyId: companyId, contactId: contactId }),
                async: false,
                success: (data) => {
                    LoadDealType(data.d);
                },
                error: (error) => {
                    toastr.error(error, "", { timeOut: 5000 });
                }
            });
        }

        function LoadDealType(isClient) {
            if (isClient) {
                $("#ContentPlaceHolder1_ddlDealType option[value = 'New Business']").hide();
                $("#ContentPlaceHolder1_ddlDealType").val("Existing Business");
            }
            else {
                $("#ContentPlaceHolder1_ddlDealType option[value = 'New Business']").show();
                $("#ContentPlaceHolder1_ddlDealType").val("New Business");
            }
        }

        function OnLoadGetEmployeesSucceed(results) {

            $("#ContentPlaceHolder1_ddlContacts").empty();

            var i = 0, fieldLength = results.length;

            if (fieldLength > 0) {
                for (i = 0; i < fieldLength; i++) {
                    $('<option value="' + results[i].Id + '">' + results[i].Name + '</option>').appendTo('#ContentPlaceHolder1_ddlContacts');
                }
            }
            else {
                $("<option value='0'>--No Contact Found--</option>").appendTo("#ContentPlaceHolder1_ddlContacts");
            }
            if (fieldLength == 1 && $("#ContentPlaceHolder1_hfId").val() != "0")
                $("#ContentPlaceHolder1_ddlContacts").val($("#ContentPlaceHolder1_ddlContacts option:first").val()).trigger('change');
            if (ContactList.length > 0)
                $("#ContentPlaceHolder1_ddlContacts").val(ContactList).trigger('change');
        }

        function OnLoadGetEmployeesFailed(error) {
            toastr.error(error);
        }

        function SaveAndClose() {
            flagValidation = 1;
            $.when(SaveOrUpdateDeal()).done(function () {
                if (flagValidation == 1) {

                    if (typeof parent.CloseDialog === "function") {
                        parent.CloseDialog();
                    }
                    if ($("#btnSave").val() == "Update and Close") {
                        $("#btnSave").val("Save And Close");
                        $("#btnSaveContinue").show();
                        $("#btnCancel").show();
                    }
                }
            });
        }

        function SaveOrUpdateDeal() {

            var id = $("#ContentPlaceHolder1_hfId").val();
            var name = $("#ContentPlaceHolder1_txtDealName").val();

            var ownerId = parseInt($("#ContentPlaceHolder1_ddlDealOwner").val().trim());
            var gLCompanyId = 0;
            var gLProjectId = 0;
            if (ownerId == 0) {
                flagValidation = 0;
                toastr.warning("Select Account Manager");
                $("#ContentPlaceHolder1_ddlDealOwner").focus();
                return false;
            }
            //if (gLCompanyId == 0) {
            //    flagValidation = 0;
            //    toastr.warning("Select Company");
            //    $("#ContentPlaceHolder1_ddlGLCompany").focus();
            //    return false;
            //}
            //if (gLProjectId == 0) {
            //    flagValidation = 0;
            //    toastr.warning("Select Project");
            //    $("#ContentPlaceHolder1_ddlGLProject").focus();
            //    return false;
            //}

            if (name == "") {
                flagValidation = 0;
                toastr.warning("Enter Deal Name");
                $("#ContentPlaceHolder1_txtDealName").focus();
                return false;
            }
            var stageId = parseInt($("#ContentPlaceHolder1_ddlDealStage").val().trim());

            if (stageId == 0) {
                flagValidation = 0;
                toastr.warning("Select Deal Stage");
                $("#ContentPlaceHolder1_ddlDealStage").focus();
                return false;
            }

            var companyId = parseInt($("#ContentPlaceHolder1_ddlCompany").val().trim());
            var isCompanyChecked = $("#chkIsSaveUnderCompany").is(":checked");

            if (isCompanyChecked && companyId == 0) {
                flagValidation = 0;
                toastr.warning("Select Company");
                $("#ContentPlaceHolder1_ddlCompany").focus();
                return false;
            }

            var amount = ($("#ContentPlaceHolder1_txtDealAmount").val());

            var startDate = $("#ContentPlaceHolder1_txtStartDate").val();

            if (startDate == "") {
                flagValidation = 0;
                toastr.warning("Select Start Date");
                $("#ContentPlaceHolder1_txtStartDate").focus();
                return false;
            }
            else {
                startDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtStartDate").val(), '/');
            }

            var endDate = ($("#ContentPlaceHolder1_txtEndDate").val());

            if (endDate != "") {
                endDate = CommonHelper.DateFormatToMMDDYYYY($("#ContentPlaceHolder1_txtEndDate").val(), '/');
            }

            var contactIds = $("#ContentPlaceHolder1_ddlContacts").val();

            if (isCompanyChecked && contactIds.length == 0) {
                flagValidation = 0;
                toastr.warning("Select Contact");
                $("#ContentPlaceHolder1_ddlContacts").focus();
                return false;
            }
            var independentContactId = $("#ContentPlaceHolder1_ddlIndependentContact").val() != "" && $("#ContentPlaceHolder1_ddlIndependentContact").val() != null ? parseInt($("#ContentPlaceHolder1_ddlIndependentContact").val()) : 0;

            if (!isCompanyChecked && independentContactId == 0) {
                flagValidation = 0;
                toastr.warning("Select Contact");
                $("#ContentPlaceHolder1_ddlIndependentContact").focus();
                return false;
            }

            var contacts = new Array();

            if (isCompanyChecked) {

                contactIds.forEach((r) => {
                    contacts.push({
                        ContactId: r,
                        Name: $("#ContentPlaceHolder1_ddlContacts").find(`option[value =${r}]`).text()
                    });
                });

            }
            else {
                companyId = 0;
                contacts.push({
                    ContactId: independentContactId,
                    Name: $("#ContentPlaceHolder1_ddlIndependentContact").find(`option[value =${independentContactId}]`).text()
                });

            }
            var type = $("#ContentPlaceHolder1_ddlDealType").val();
            var probabilityStageId = +$("#ContentPlaceHolder1_ddlProbabilityStage").val();

            var expectedRevenue = $("#ContentPlaceHolder1_txtExpectedRevenue").val().trim();

            if (expectedRevenue != "")
                expectedRevenue = parseFloat(expectedRevenue);
            var segmentId = 0;

            var segmentId = +$("#ContentPlaceHolder1_ddlSegment").val();
            if (isNaN(segmentId))
                segmentId = 0;
            var randomDealId = +$("#ContentPlaceHolder1_RandomDealId").val();
            var deletedDocuments = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();

            var description = $("#ContentPlaceHolder1_txtDescription").val();

            var deal = {
                Id: id,
                RandomDealId: randomDealId,
                Name: name,
                OwnerId: ownerId,
                StageId: stageId,
                CompanyId: companyId,
                Type: type,
                Amount: amount,
                StartDate: startDate,
                CloseDate: endDate,
                ProbabilityStageId: probabilityStageId,
                ExpectedRevenue: expectedRevenue,
                SegmentId: segmentId,
                Description: description,
                Owner: $("#ContentPlaceHolder1_ddlDealOwner option:selected").text(),
                Stage: $("#ContentPlaceHolder1_ddlDealStage option:selected").text(),
                Contacts: contacts
            }


            return $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/Deal.aspx/SaveOrUpdateDeal',
                data: JSON.stringify({ deal: deal, deletedDocument: deletedDocuments }),
                dataType: "json",
                async: false,
                success: function (data) {
                    OnSuccessSaveOrUpdate(data.d);
                },
                error: function (error) {
                    OnFailSaveOrUpdate(error.responseText);
                }
            });
            //PageMethods.SaveOrUpdateDeal(deal, deletedDocuments, OnSuccessSaveOrUpdate, OnFailSaveOrUpdate)
        }

        function OnSuccessSaveOrUpdate(result) {

            if (result.IsSuccess) {
                parent.ShowAlert(result.AlertMessage);
                var contactId = +$.trim(CommonHelper.GetParameterByName("conid"));
                if (typeof parent.GridPaging === "function") {
                    var activeLink = Math.trunc($(parent.WebForm_GetElementById("GridPagingContainer")).find("ul li.active").text());
                    parent.GridPaging(activeLink, 1);
                }

                if (typeof parent.LoadContactAllDeal == "function")
                    parent.LoadContactAllDeal();
                if (typeof parent.LoadLog == "function")
                    parent.LoadLog();
                if (typeof parent.GetDealInfoByCompanyId == "function" && contactId > 0)
                    parent.GetDealInfoByCompanyId();
                if (typeof parent.FillForm === "function" && contactId == 0) {
                    var dealId = $.trim(CommonHelper.GetParameterByName("did"));
                    parent.FillForm(dealId);
                }
                //if (result.Data == null)
                $("#ContentPlaceHolder1_RandomDealId").val(result.Data);
                PerformClearAction();
            }

            else {
                flagValidation = 0;
                if (result.DataStr)
                    $("#ContentPlaceHolder1_ddlDealStage").focus();
                parent.ShowAlert(result.AlertMessage);
            }
        }

        function PerformClearAction() {
            var companyId = $.trim(CommonHelper.GetParameterByName("cid"));
            var dealId = $.trim(CommonHelper.GetParameterByName("did"));
            var contactId = $.trim(CommonHelper.GetParameterByName("conid"));

            $("#ContentPlaceHolder1_hfId").val("0");
            $("#ContentPlaceHolder1_hfContactPersonId").val("0");
            $("#ContentPlaceHolder1_txtDealName").val('').focus();
            $("#ContentPlaceHolder1_ddlIndependentContact").val('0').trigger('change');
            $("#ContentPlaceHolder1_ddlDealStage").val("0");
            if (!$("#chkIsSaveUnderCompany").is(":disabled"))
                $("#ContentPlaceHolder1_ddlCompany").val("0").trigger('change');
            $("#ContentPlaceHolder1_txtStartDate").datepicker("setDate", DayOpenDate);
            $("#ContentPlaceHolder1_txtEndDate").datepicker("setDate", DayOpenDate);
            $("#ContentPlaceHolder1_txtDealAmount").val('');
            $("#ContentPlaceHolder1_ddlProbabilityStage").val('');
            $("#ContentPlaceHolder1_txtExpectedRevenue").val('');
            $("#ContentPlaceHolder1_ddlSegment").val('');
            $("#ContentPlaceHolder1_hfGuestDeletedDoc").val("");
            $("#DealDocumentInfo").html('');
            ContactList = [];
            //flagValidation = 0;
            return false;
        }

        function FillForm(dealId) {
            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: '../SalesAndMarketing/Deal.aspx/GetDealInfoById',
                data: JSON.stringify({ Id: dealId }),
                dataType: "json",
                success: function (data) {
                    PerformClearAction();
                    $("#btnSave").val("Update and Close");
                    $("#btnSaveContinue").hide();
                    $("#btnCancel").hide();
                    $("#ContentPlaceHolder1_txtDealName").val(data.d.Name);
                    $("#ContentPlaceHolder1_ddlDealOwner").val(data.d.OwnerId);
                    //$("#ContentPlaceHolder1_ddlGLCompany").val(data.d.GLCompanyId);
                    //$("#ContentPlaceHolder1_ddlGLProject").val(data.d.GLProjectId);

                    ContactList = data.d.Contacts.map((r) => r.ContactId);

                    if (data.d.CompanyId != 0) {
                        $("#chkIsSaveUnderCompany").attr("checked", true).trigger('change');
                        $("#ContentPlaceHolder1_ddlCompany").val(data.d.CompanyId).trigger('change');
                    }
                    else {
                        $("#ContentPlaceHolder1_ddlIndependentContact").val(data.d.Contacts[0].ContactId).trigger('change');
                        $("#chkIsSaveUnderCompany").attr("checked", false).trigger('change');
                    }

                    $("#ContentPlaceHolder1_ddlDealStage").val(data.d.StageId);

                    //$("#ContentPlaceHolder1_hfContactPersonId").val(data.d.ContactId);
                    $("#ContentPlaceHolder1_txtStartDate").val(CommonHelper.DateFromStringToDisplay(data.d.StartDate, innBoarDateFormat));
                    if (data.d.CloseDate != null)
                        $("#ContentPlaceHolder1_txtEndDate").val(CommonHelper.DateFromStringToDisplay(data.d.CloseDate, innBoarDateFormat));
                    else
                        $("#ContentPlaceHolder1_txtEndDate").val("");
                    $("#ContentPlaceHolder1_txtDealAmount").val(data.d.Amount);
                    $("#ContentPlaceHolder1_ddlProbabilityStage").val(data.d.ProbabilityStageId);
                    $("#ContentPlaceHolder1_txtExpectedRevenue").val(data.d.ExpectedRevenue);
                    $("#ContentPlaceHolder1_ddlSegment").val(data.d.SegmentId);
                    $("#ContentPlaceHolder1_txtDescription").val(data.d.Description);
                    $("#ContentPlaceHolder1_hfId").val(data.d.Id);
                    //$("#ContentPlaceHolder1_RandomDealId").val(data.d.RandomDealId);
                    UploadComplete();
                },
                error: function (result) {
                }
            });
        }

        function OnFailSaveOrUpdate(error) {
            toastr.error(error);
        }

        function UploadComplete() {
            var randomId = +$("#ContentPlaceHolder1_RandomDealId").val();
            var id = +$("#ContentPlaceHolder1_hfId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfGuestDeletedDoc").val();
            PageMethods.LoadDealDocument(id, randomId, deletedDoc, OnLoadDocumentSucceeded, OnLoadDocumentFailed);
            return false;
        }

        function OnLoadDocumentSucceeded(result) {
            var guestDoc = result;
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";

            guestDocumentTable += "<table id='dealDocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
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
                    if (guestDoc[row].Extention == ".jpg")
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

            $("#DealDocumentInfo").html(guestDocumentTable);
        }

        function OnLoadDocumentFailed(error) {
            toastr.error(error.get_message());
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
        function LoadDocUploader() {

            var randomId = +$("#ContentPlaceHolder1_RandomDealId").val();
            var path = "/SalesAndMarketing/Images/Deal/";
            var category = "SalesDealDocuments";
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
        function AttachFile() {
            $("#dealdocuments").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                closeOnEscape: true,
                resizable: false,
                title: "Deal Documents",
                show: 'slide'
            });
        }
        function PopulateProjects(companyId) {
            //let companyId = $(control).val();
            if (companyId == "0") {
                PopulateControlWithValueNTextField([], $("#ContentPlaceHolder1_ddlGLProject"), $("#<%=CommonDropDownHiddenField.ClientID %>").val(), "Name", "ProjectId");
            }
            $("#ContentPlaceHolder1_ddlGLProject").empty().append('<option selected="selected" value="0">Loading...</option>');

            $.ajax({
                type: "POST",
                url: "./Deal.aspx/GetGLProjectByGLCompanyId",
                data: JSON.stringify({ companyId: companyId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    PopulateControlWithValueNTextField(response.d, $("#ContentPlaceHolder1_ddlGLProject"), $("#<%=CommonDropDownHiddenField.ClientID %>").val(), "Name", "ProjectId");
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
        }
    </script>
    <div id="DocumentDialouge" style="display: none;">
        <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
            clientidmode="static" scrolling="yes"></iframe>
    </div>
    <%--<div id="dealdocuments" style="display: none;">
        <label for="Attachment" class="control-label col-md-2">
            Attachment</label>
        <div class="col-md-4">
            <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
                <cc1:ClientUploader ID="flashUpload" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                    FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
            </asp:Panel>
        </div>
    </div>--%>

    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfGuestDeletedDoc" runat="server" />
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>
    <div class="panel-body">
        <div class="form-horizontal">
            
            <fieldset id="CompanyProjectPanel" style="display:none">
                
                <asp:HiddenField ID="hfIsSingle" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hfCompanyAll" runat="server" />
                
            </fieldset>
            <div class="form-group">
                <div class="col-md-2">
                    <asp:Label ID="lblDealOwnerForCreation" runat="server" class="control-label required-field" Text="Account Manager"></asp:Label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlDealOwner" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label required-field">Deal Name</label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtDealName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-offset-2 col-md-4">
                    <input id="chkIsSaveUnderCompany" type="checkbox" />
                    &nbsp;<label class="control-label">Is Save Under Company ?</label>
                </div>
            </div>
            <div id="dvCompany" class="form-group under">
                <div class="col-md-2">
                    <label class="control-label required-field">Company</label>
                </div>
                <div class="col-md-10">
                    <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div id="dvContacts" class="form-group">
                <div class="col-md-2">
                    <label class="control-label required-field">Contacts</label>
                </div>
                <div class="col-md-10">
                    <asp:DropDownList ID="ddlContacts" runat="server" CssClass="form-control" multiple="multiple" name="states[]" Style="width: 100%;">
                    </asp:DropDownList>
                </div>
            </div>
            <div id="dvIndepentContacts" class="form-group">
                <div class="col-md-2">
                    <label class="control-label required-field">Contact</label>
                </div>
                <div class="col-md-10">
                    <asp:DropDownList ID="ddlIndependentContact" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label required-field">Deal Type</label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlDealType" runat="server" CssClass="form-control">
                        <asp:ListItem Value="New Business" Text="New Business"></asp:ListItem>
                        <asp:ListItem Value="Existing Business" Text="Existing Business"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <label class="control-label">Deal Amount</label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtDealAmount" runat="server" CssClass="quantitydecimal form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label required-field">Start Date</label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <label class="control-label">Exp. Close Date</label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label required-field">Deal Stage</label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlDealStage" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label">Probability Stage</label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlProbabilityStage" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <label class="control-label">Expected Revenue</label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtExpectedRevenue" runat="server" CssClass="quantitydecimal form-control"></asp:TextBox>
                </div>
            </div>
            <div id="SegmentDiv" class="form-group" runat="server">
                <div class="col-md-2">
                    <label class="control-label">Segment Name</label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlSegment" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>
            <div id="dvProductInfo" class="panel panel-default" runat="server">
                <div class="panel-heading">
                    Product Information
                </div>
                <div class="panel-body">
                    <%-- <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCategory" CssClass="form-control" runat="server" TabIndex="20">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblItemName" runat="server" class="control-label" Text="Item Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtItemName" CssClass="form-control" TabIndex="21" runat="server"
                                    ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblItemStockBy" runat="server" class="control-label" Text="Stock By"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlItemStockBy" runat="server" CssClass="form-control" TabIndex="22">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <button type="button" id="btnAddItem" tabindex="24" class="btn btn-primary" onclick="AddItem()">
                                    Add</button>
                            </div>
                        </div>
                        &nbsp;
                            <div class="form-group" style="padding: 0px;">
                                <div id="ItemTableContainer">
                                    <table id="ProductInformation" class="table table-condensed table-bordered table-responsive">
                                    </table>
                                </div>
                            </div>
                    </div>--%>
                </div>
            </div>
            <div id="dvServiceInfo" class="panel panel-default" runat="server">
                <div class="panel-heading">
                    Service Information
                </div>
                <div class="panel-body">
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-2">
                    <label class="control-label">Description</label>
                </div>
                <div class="col-md-10">
                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <asp:HiddenField ID="RandomDealId" runat="server"></asp:HiddenField>
                <div class="col-md-2">
                    <label class="control-label">Attachment</label>
                </div>
                <div class="col-md-10">
                    <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Attach" onclick="LoadDocUploader()" />
                </div>
            </div>
            <div class="form-group">
                <div id="DealDocumentInfo" class="col-md-12">
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <input type="button" id="btnSave" class="TransactionalButton btn btn-primary btn-sm" onclick="SaveAndClose()" value="Save And Close" />
                    <input type="button" id="btnSaveContinue" class="TransactionalButton btn btn-primary btn-sm" onclick="SaveOrUpdateDeal()" value="Save And Continue" />
                    <input type="button" id="btnCancel" class="btn btn-primary btn-sm" value="Clear" onclick="PerformClearAction()" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
