<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmPMSupplier.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.frmPMSupplier" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var ContactPersonsForSupplier = new Array();
        var ContactPersonsForSupplierDeleted = new Array();
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Purchase</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Supplier</li>";
            var breadCrumbs = moduleName + formName;

            if ($("#ContentPlaceHolder1_hfContactPersons").val() != '') {
                var persons = JSON.parse($("#ContentPlaceHolder1_hfContactPersons").val());
                if (persons != "") {
                    FillContactsInForm(persons);
                }
            }
            if ($("#ContentPlaceHolder1_hfClearContactForSupplierTbl").val() == '1') {

                $("#ContactForSupplierTbl > tbody").html("");
                $("#ContentPlaceHolder1_hfClearContactForSupplierTbl").val('0');

            }

            if ($("#ContentPlaceHolder1_hfIsSupplierCodeAutoGenerate").val() == '1') {

                $("#CodeDiv").hide();
            }
            else {
                $("#CodeDiv").show();
            }

            if ($("#ContentPlaceHolder1_hfIsSupplierUserPanalEnable").val() == '0') {

                $("#userPanal").hide();
            }
            else {
                $("#userPanal").show();
            }



            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            var editing = $("#<%=hfIsEdit.ClientID %>").val();
            if (editing != "0") {
                UploadComplete();
                <%--ShowUploadedDocument($("#<%=RandomDocId.ClientID %>").val());--%>
            }
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            <%--$("#<%=txtEmail.ClientID %>").blur(function () {
                var isValid = IsEmail($("#<%=txtEmail.ClientID %>").val());
                if (isValid != true) {
                    toastr.warning("Email is not in correct format.");
                }
            });--%>

            $("#<%=txtContactEmail.ClientID %>").blur(function () {
                var contactEmailValue = $("#<%=txtContactEmail.ClientID %>").val().length;
                if (contactEmailValue > 0) {
                    var isCEmailValid = IsEmail($("#<%=txtContactEmail.ClientID %>").val());
                    if (isCEmailValid != true) {
                        toastr.warning("Contact Email is not in correct format.");
                    }
                }
            });
        });

        $(document).ready(function () {

            $("[id=ContentPlaceHolder1_glCompany_ChkCreate]").on("click", function () {
                var topCheckBox = $(this);

                if ($(topCheckBox).is(":checked") == true) {
                    $("#ContentPlaceHolder1_glCompany tbody tr").find("td:eq(0) > span").find("input").prop("checked", true);
                }
                else {
                    $("#ContentPlaceHolder1_glCompany tbody tr").find("td:eq(0) > span").find("input").prop("checked", false);
                }
            });

            //$("[id=ContentPlaceHolder1_supplierInfo_ChkCreate]").on("click", function () {
            //    var topCheckBox = $(this);

            //    if ($(topCheckBox).is(":checked") == true) {
            //        $("#ContentPlaceHolder1_supplierInfo tbody tr").find("td:eq(0) > span").find("input").prop("checked", true);
            //    }
            //    else {
            //        $("#ContentPlaceHolder1_supplierInfo tbody tr").find("td:eq(0) > span").find("input").prop("checked", false);
            //    }
            //});

        });

        $(function () {
            $("#myTabs").tabs();
        });



        function IsEmail(email) {
            var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            return regex.test(email);
        }

        function validEmail(e) {
            var filter = /^\s*[\w\-\+_]+(\.[\w\-\+_]+)*\@[\w\-\+_]+\.[\w\-\+_]+(\.[\w\-\+_]+)*\s*$/;
            return String(e).search(filter) != -1;
        }


        function PerformClearActionOnButtonClick() {
            if (!confirm("Do you want To clear?")) {
                return false;
            }
            PerformClearAction();
        }
        function SaveValidation() {
            var supplierName = $("#<%=txtName.ClientID %>").val();
            if (supplierName == "") {
                toastr.warning("Please Insert Supplier Name.");


                $("#ContentPlaceHolder1_txtName").focus();
                return false;
            }
            var suppliertype = $("#<%=ddlSupplierType.ClientID %>").val();
            if (suppliertype == "0") {
                toastr.warning("Please Select Supplier Type.");


                $("#ContentPlaceHolder1_ddlSupplierType").focus();
                return false;
            }
            if ($("#ContentPlaceHolder1_hfIsSupplierCodeAutoGenerate").val() != '1') {
                var code = $("#<%=txtCode.ClientID %>").val();
                if (code == "") {
                    toastr.warning("Please Insert Supplier Code.");

                    $("#ContentPlaceHolder1_txtCode").focus();
                    return false;
                }
            }

            if ($("#ContentPlaceHolder1_hfIsSupplierUserPanalEnable").val() != '0') {
                var UserId = $("#<%=txtSupplierUserId.ClientID %>").val();
                if (UserId == "") {
                    toastr.warning("Please Insert Supplier Id.");
                    $("#ContentPlaceHolder1_txtSupplierUserId").focus();
                    return false;
                }
                var UserPassword = $("#<%=txtUserPassword.ClientID %>").val();
                if (UserPassword == "") {
                    toastr.warning("Please Insert Password.");
                    $("#ContentPlaceHolder1_txtUserPassword").focus();
                    return false;
                }
                if ($("#<%=txtUserPassword.ClientID %>").val() != $("#<%=txtUserConfirmPassword.ClientID %>").val()) {
                    toastr.warning('Wrong confirm password !');
                    $("#ContentPlaceHolder1_txtUserConfirmPassword").focus();
                    return false;
                }
            }


            <%--var email = $("#<%=txtEmail.ClientID %>").val();
            if (email == "") {
                toastr.warning("Please Insert Supplier Email Id.");

                $("#ContentPlaceHolder1_txtEmail").focus();
                return false;
            }--%>

            <%--var userName = "Shekhar Shete";
            //'<%Session["ContactPersonsForSupplier"] = "' + userName + '"; %>';

            '<%Session["ContactPersonsForSupplier"] = ContactPersonsForSupplier; %>';
            '<%Session["ContactPersonsForSupplierDeleted"] = "' + ContactPersonsForSupplierDeleted + '"; %>';--%>
            $("#<%=hfContactPersons.ClientID %>").val(JSON.stringify(ContactPersonsForSupplier));
            $("#<%=hfContactPersonsDeleted.ClientID %>").val(JSON.stringify(ContactPersonsForSupplierDeleted));

            return true;
        }
        function PerformClearAction() {
            $('#doctablelist tbody tr').each(function (i, row) {
                $(this).find("td:eq(2) img").trigger('click')
            });
            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './frmPMSupplier.aspx/ChangeRandomId',
                dataType: "json",
                async: false,
                success: function (data) {
                    $("#ContentPlaceHolder1_RandomDocId").val(data.d);
                },
                error: function (error) {
                }
            });
            $("#DocumentInfo").html("");
            $("#<%=txtSearchName.ClientID %>").val('');
            $("#<%=txtSearchCode.ClientID %>").val('');
            $("#<%=txtSearchEmail.ClientID %>").val('');
            $("#<%=txtSearchPhone.ClientID %>").val('');
            $("#<%=txtSearchContact.ClientID %>").val('');
            $("#<%=txtName.ClientID %>").val('');
            $("#<%=ddlSupplierType.ClientID %>").val('0');
            $("#<%=txtCode.ClientID %>").val('');
            $("#<%=txtSupplierId.ClientID %>").val('');
            $("#<%=txtSupplierUserInfoId.ClientID %>").val('');
            $("#<%=txtEmail.ClientID %>").val('');
            $("#<%=txtAddress.ClientID %>").val('');
            //$("#<%=txtContactPerson.ClientID %>").val('');
            $("#<%=txtPhone.ClientID %>").val('');

            $("#<%=txtFax.ClientID %>").val('');

            clearGridViewCheckboxes();

            $("#<%=txtRemarks.ClientID %>").val('');
            $("#ContentPlaceHolder1_hfContactPersons").val('');
            ClearContactContainer();
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }

        function clearGridViewCheckboxes() {
            var gridView = document.getElementById('<%=glCompany.ClientID %>');
            var Elements = gridView.getElementsByTagName('input');

            for (var i = 0; i < Elements.length; i++) {
                if (Elements[i].type == 'checkbox' && Elements[i].checked)
                    Elements[i].checked = false;
            }
        }

        function ClearContactContainer() {
            $("#<%=txtContactPerson.ClientID %>").val('');
            $("#<%=txtContactAddress.ClientID %>").val('');
            $("#<%=txtContactEmail.ClientID %>").val('');
            $("#<%=txtContactPhone.ClientID %>").val('');

            $("#ContentPlaceHolder1_ddlContactType").val("0").change();
        }
        function LoadDocUploader() {
            var randomId = +$("#ContentPlaceHolder1_RandomDocId").val();
            var path = "/PurchaseManagment/Images/Supplier/";
            var category = "SupplierDocuments";
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
            return false;
        }
        function UploadComplete() {
            var randomId = $("#ContentPlaceHolder1_RandomDocId").val();
            var id = $("#ContentPlaceHolder1_hfSupplierId").val();
            var deletedDoc = $("#ContentPlaceHolder1_hfDeletedDoc").val();
            PageMethods.GetUploadedDocByWebMethod(randomId, id, deletedDoc, OnGetUploadedDocByWebMethodSucceeded, OnGetUploadedDocByWebMethodFailed);
            return false;
            //ShowUploadedDocument(randomId);
        }

        function ShowUploadedDocument(Id) {
            //var id = $("#ContentPlaceHolder1_hfSupplierId").val();
            PageMethods.GetDocumentsByUserTypeAndUserId(Id, OnLoadImagesSucceeded, OnLoadImagesFailed);
            return false;
        }
        function OnLoadImagesSucceeded(result) {
            $("#imageDiv").html(result);

            $("#supplierDocuments").dialog({
                autoOpen: true,
                modal: true,
                width: 900,
                height: 300,
                closeOnEscape: true,
                resizable: false,
                title: "Supplier Documents",
                show: 'slide'
            });

            return false;
        }
        function OnLoadImagesFailed(error) {
            toastr.error(error.get_message());
        }
        function OnGetUploadedDocByWebMethodSucceeded(result) {
            //var totalDoc = result.length;
            //var row = 0;
            //var imagePath = "";
            //DocTable = "";

            //DocTable += "<table id='DocTableList' style='width:100%' class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            //DocTable += "<th align='left' scope='col'>Doc Name</th><th align='left' scope='col'>Display</th> <th align='left' scope='col'>Action</th></tr>";

            //for (row = 0; row < totalDoc; row++) {
            //    if (row % 2 == 0) {
            //        DocTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
            //    }
            //    else {
            //        DocTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
            //    }

            //    DocTable += "<td align='left' style='width: 50%'>" + result[row].Name + "</td>";

            //    if (result[row].Path != "") {
            //        imagePath = "<img src='" + result[row].Path + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
            //    }
            //    else
            //        imagePath = "";

            //    DocTable += "<td align='left' style='width: 30%'>" + imagePath + "</td>";

            //    DocTable += "<td align='left' style='width: 20%'>";
            //    DocTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteDoc('" + result[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
            //    DocTable += "</td>";
            //    DocTable += "</tr>";
            //}
            //DocTable += "</table>";

            //docc = DocTable;

            //$("#DocumentInfo").html(DocTable);
            var guestDoc = result;
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";

            guestDocumentTable += "<table id='contactDocList' style='width:100%' class='table table-bordered table-condensed table-responsive'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
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
                        imagePath = "<img src='" + guestDoc[row].Path + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    else
                        imagePath = "<img src='" + guestDoc[row].IconImage + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                guestDocumentTable += "<td align='left' style='width: 30%'>" + imagePath + "</td>";

                guestDocumentTable += "<td align='left' style='width: 20%'>";
                guestDocumentTable += "&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                guestDocumentTable += "</td>";
                guestDocumentTable += "</tr>";
            }
            guestDocumentTable += "</table>";

            // docc = guestDocumentTable;

            $("#DocumentInfo").html(guestDocumentTable);

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

        function AddItem() {
            debugger;


            var contactPerson = $("#ContentPlaceHolder1_txtContactPerson").val();
            if (contactPerson == "") {
                toastr.warning("Add Contact Person.");
                $("#ContentPlaceHolder1_txtContactPerson").focus();
                return false;
            }
            var ContactPersonOBJ = _.findWhere(ContactPersonsForSupplier, { ContactPerson: contactPerson });
            if (ContactPersonOBJ != null) {
                toastr.warning("It's already added.");
                $("#ContentPlaceHolder1_txtContactPerson").focus();
                return false;
            }

            var contactPhone = $("#ContentPlaceHolder1_txtContactPhone").val();
            if (contactPhone == "") {
                toastr.warning("Add Contact Phone.");
                $("#ContentPlaceHolder1_txtContactPhone").focus();
                return false;
            }

            var contactType = $("#ContentPlaceHolder1_ddlContactType option:selected").val();
            var contactTypeName = $("#ContentPlaceHolder1_ddlContactType option:selected").text();
            if (contactType == "0") {
                toastr.warning("Select a Contact Type.");
                $("#ContentPlaceHolder1_ddlContactType").focus();
                return false;
            }


            var contactEmailValue = $("#<%=txtContactEmail.ClientID %>").val().length;
            if (contactEmailValue > 0) {
                isCEmailValid = IsEmail($("#<%=txtContactEmail.ClientID %>").val());
                if (isCEmailValid != true) {
                    toastr.warning("Contact Email is not in correct format.");
                    return false;
                }
            }
            //ContentPlaceHolder1_txtWebAddress
            var contactAddress = $("#ContentPlaceHolder1_txtContactAddress").val();
            var contactEmail = $("#ContentPlaceHolder1_txtContactEmail").val();

            var tr = "";
            tr += "<tr>";

            tr += "<td style='width:25%;'>" + contactPerson + "</td>";

            tr += "<td style='width:25%;'>" +
                "<input type='text' value='" + contactAddress + "' id='cpa" + contactPerson + "' class='form-control' onblur='CalculateTotalForAdhoq(this)'  />" +
                "</td>";
            tr += "<td style='width:15%;'>" +
                "<input type='text' value='" + contactEmail + "' id='cpe" + contactPerson + "' class='form-control' onblur='CalculateTotalForAdhoq(this)'  />" +
                "</td>";
            tr += "<td style='width:15%;'>" +
                "<input type='text' value='" + contactPhone + "' id='cpp" + contactPerson + "' class='form-control' onblur='CalculateTotalForAdhoq(this)'  />" +
                "</td>";

            tr += "<td style='width:10%;'>" + contactTypeName + "</td>";

            tr += "<td style='width:10%;'>";
            tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
            tr += "</td>";
            tr += "<td style='display:none;'>" + contactType + "</td>";
            tr += "<td style='display:none;'>" + 0 + "</td>";

            tr += "</tr>";

            $("#ContactForSupplierTbl tbody").prepend(tr);
            tr = "";

            ContactPersonsForSupplier.push({
                ContactPerson: contactPerson,
                ContactAddress: contactAddress,
                ContactEmail: contactEmail,
                ContactPhone: contactPhone,
                ContactType: contactType,
                SupplierDetailsId: 0,
                SupplierId: 0
            });

            ClearContactContainer();
            $("#ContentPlaceHolder1_txtContactPerson").focus();
        }
        function confirmPass() {
            if ($("#<%=txtUserPassword.ClientID %>").val() != $("#<%=txtUserConfirmPassword.ClientID %>").val()) {
                toastr.warning('Wrong confirm password !');
                $("#ContentPlaceHolder1_txtUserConfirmPassword").focus();
                $("#<%=txtUserConfirmPassword.ClientID %>").val("");
                return false;
            }
        }
        function FillContactsInForm(Result) {

            var tr = "";
            for (var i = 0; i < Result.length; i++) {
                tr += "<tr>";

                tr += "<td style='width:25%;'>" + Result[i].ContactPerson + "</td>";

                tr += "<td style='width:25%;'>" +
                    "<input type='text' value='" + Result[i].ContactAddress + "' id='cpa" + Result[i].ContactPerson + "' class='form-control' onblur='CalculateTotalForAdhoq(this)'  />" +
                    "</td>";
                tr += "<td style='width:15%;'>" +
                    "<input type='text' value='" + Result[i].ContactEmail + "' id='cpe" + Result[i].ContactPerson + "' class='form-control' onblur='CalculateTotalForAdhoq(this)' />" +
                    "</td>";
                tr += "<td style='width:15%;'>" +
                    "<input type='text' value='" + Result[i].ContactPhone + "' id='cpp" + Result[i].ContactPerson + "' class='form-control' onblur='CalculateTotalForAdhoq(this)' />" +
                    "</td>";
                tr += "<td style='width:10%;'>" + Result[i].ContactType + "</td>";

                tr += "<td style='width:10%;'>";
                tr += "<a href='javascript:void()' onclick= 'DeleteAdhoqItem(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
                tr += "</td>";
                tr += "<td style='display:none;'>" + Result[i].ContactType + "</td>";
                tr += "<td style='display:none;'>" + Result[i].SupplierDetailsId + "</td>";

                tr += "</tr>";



                ContactPersonsForSupplier.push({
                    ContactPerson: Result[i].ContactPerson,
                    ContactAddress: Result[i].ContactAddress,
                    ContactEmail: Result[i].ContactEmail,
                    ContactPhone: Result[i].ContactPhone,
                    ContactType: Result[i].ContactType,
                    SupplierDetailsId: parseInt(Result[i].SupplierDetailsId, 10),
                    SupplierId: parseInt(Result[i].SupplierId, 10)
                });
            }
            $("#ContactForSupplierTbl tbody").prepend(tr);
            return false;

        }

        function CalculateTotalForAdhoq(control) {

            debugger;
            var tr = $(control).parent().parent();

            var contactAddress = $.trim($(tr).find("td:eq(1)").find("input").val());
            var contactEmail = $.trim($(tr).find("td:eq(2)").find("input").val());
            var contactPhone = $.trim($(tr).find("td:eq(3)").find("input").val());
            // debugger;
            var contactPerson = ($.trim($(tr).find("td:eq(0)").text()));

            var ContactPersonOBJ = _.findWhere(ContactPersonsForSupplier, { ContactPerson: contactPerson });
            var index = _.indexOf(ContactPersonsForSupplier, ContactPersonOBJ);


            var contactEmailValue = contactEmail.length;
            if (contactEmailValue > 0) {
                isCEmailValid = IsEmail(contactEmail);
                if (isCEmailValid != true) {
                    $(tr).find("td:eq(2)").find("input").val(ContactPersonsForSupplier[index].ContactEmail);
                    toastr.warning("Contact Email is not in correct format.");
                    return false;
                }
            }
            ContactPersonsForSupplier[index].ContactAddress = contactAddress;
            ContactPersonsForSupplier[index].ContactEmail = (contactEmail);
            ContactPersonsForSupplier[index].ContactPhone = (contactPhone);
        }

        function DeleteAdhoqItem(control) {

            if (!confirm("Do you want to delete?")) { return false; }

            //debugger;

            var tr = $(control).parent().parent();

            var contactPerson = ($.trim($(tr).find("td:eq(0)").text()));
            var detailsId = parseInt($.trim($(tr).find("td:eq(7)").text()), 10);

            var ContactPersonOBJ = _.findWhere(ContactPersonsForSupplier, { ContactPerson: contactPerson });
            var index = _.indexOf(ContactPersonsForSupplier, ContactPersonOBJ);

            if (parseInt(detailsId, 10) > 0)
                ContactPersonsForSupplierDeleted.push(JSON.parse(JSON.stringify(ContactPersonOBJ)));

            ContactPersonsForSupplier.splice(index, 1);
            $(tr).remove();

        }
    </script>
    <div id="supplierDocuments" style="display: none;">
        <div id="imageDiv"></div>
    </div>
    <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="tempDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfParentDoc" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfDeletedDoc" runat="server" Value="0" />
    <asp:HiddenField ID="hfSupplierId" runat="server" Value="0" />
    <asp:HiddenField ID="hfCompanyId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfIsEdit" runat="server" Value="0" />
    <asp:HiddenField ID="hfClearContactForSupplierTbl" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsSupplierDifferentWithGLCompany" runat="server" Value="0" />    
    <asp:HiddenField ID="hfIsSupplierCodeAutoGenerate" runat="server" Value="0" />
    <asp:HiddenField ID="hfIsSupplierUserPanalEnable" runat="server" Value="0" />
    <asp:HiddenField ID="hfContactPersons" runat="server" />
    <asp:HiddenField ID="hfContactPersonsDeleted" runat="server" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Supplier Info</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Supplier </a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Supplier Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:HiddenField ID="txtNodeId" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtSupplierId" runat="server"></asp:HiddenField>

                                <asp:HiddenField ID="txtSupplierUserInfoId" runat="server"></asp:HiddenField>

                                <asp:Label ID="lblName" runat="server" class="control-label required-field" Text="Supplier Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtName" runat="server" TabIndex="1" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label1" runat="server" class="control-label required-field" Text="Supplier Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList runat="server" ID="ddlSupplierType" CssClass="form-control">
                                    <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Local" Value="Local"></asp:ListItem>
                                    <asp:ListItem Text="Foreign" Value="Foreign"></asp:ListItem>
                                    <asp:ListItem Text="Both (Local & Foreign)" Value="Both"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div id="CodeDiv">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblCode" runat="server" class="control-label required-field" Text="Code"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div id="userPanal">
                            <div class="form-group ">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblUserId" runat="server" class="control-label required-field" Text="Supplier Id"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtSupplierUserId" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblUserPassword" runat="server" class="control-label required-field" Text="Password"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtUserPassword" runat="server" CssClass="form-control" TabIndex="4"
                                        TextMode="Password"></asp:TextBox>
                                </div>
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblUserConfirmPassword" runat="server" class="control-label required-field" Text="Confirm Pass."></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtUserConfirmPassword" runat="server" onblur="javascript: return confirmPass();"
                                        CssClass="form-control" TabIndex="5" TextMode="Password"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">

                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblPhone" runat="server" class="control-label" Text="Phone"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblFax" runat="server" class="control-label" Text="Fax"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFax" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">

                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblEmail" runat="server" class="control-label" Text="Email"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblAddress" runat="server" class="control-label" Text="Address"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="6"></asp:TextBox>
                            </div>
                        </div>

                        <div id="ContactContainer">
                            <hr />
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblContactPerson" runat="server" class="control-label required-field" Text="Contact Person"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblContactAddress" runat="server" class="control-label" Text="Contact Address"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtContactAddress" runat="server" CssClass="form-control" TabIndex="8"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblContactEmail" runat="server" class="control-label" Text="Contact Email"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtContactEmail" runat="server" CssClass="form-control" TabIndex="9"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblContactPhone" runat="server" class="control-label required-field" Text="Contact Phone"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtContactPhone" runat="server" CssClass="form-control" TabIndex="10"></asp:TextBox>
                                </div>

                                <div class="col-md-2 label-align">
                                    <asp:Label ID="lblContactType" runat="server" class="control-label required-field" Text="Contact Type"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList runat="server" ID="ddlContactType" CssClass="form-control">
                                        <%--<asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Support" Value="Support"></asp:ListItem>
                                        <asp:ListItem Text="Development" Value="Development"></asp:ListItem>--%>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group" style="padding-top: 10px;">
                                <div class="col-md-12">
                                    <input id="btnAdd" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" onclick="AddItem()" />
                                    <input id="btnCancelContact" type="button" value="Cancel" onclick="ClearContactContainer()"
                                        class="TransactionalButton btn btn-primary btn-sm" />
                                </div>
                            </div>

                            <div style="height: 250px; overflow-y: scroll;">
                                <table id="ContactForSupplierTbl" class="table table-bordered table-condensed table-hover">
                                    <thead>
                                        <tr>
                                            <th style="width: 25%;">Contact Person</th>
                                            <th style="width: 25%;">Address</th>
                                            <th style="width: 15%;">Email</th>
                                            <th style="width: 15%;">Phone</th>
                                            <th style="width: 10%;">Type</th>
                                            <th style="width: 10%;">Action</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>

                            </div>
                        </div>
                        <hr />

                        <div id="CompanyInformationDiv" class="panel panel-default col-md-12" runat="server">
                            <div class="panel-body">
                                <asp:GridView ID="glCompany" Width="100%" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                    ForeColor="#333333" PageSize="500000" CssClass="table table-bordered table-condensed table-responsive">
                                    <RowStyle BackColor="#E3EAEB" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompanyId" runat="server" Text='<%#Eval("CompanyId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Create/Update" ItemStyle-Width="05%">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ChkCreate" CssClass="ChkCreate" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSavePermission" CssClass="Chk_Create" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Company" ItemStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGlCompany" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                       
                                    </Columns>
                                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblRecordNotFound" runat="server" Text="Record Not Found."></asp:Label>
                                    </EmptyDataTemplate>
                                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#44545E" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#7C6F57" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </div>
                        </div>


                        <%-- <div id="SupplierInformationDiv" class="panel panel-default col-md-6">
                         <div class="panel-body">
                                <asp:GridView ID="supplierInfo" Width="100%" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                                    ForeColor="#333333" PageSize="500000" CssClass="table table-bordered table-condensed table-responsive">
                                    <RowStyle BackColor="#E3EAEB" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSupplierId" runat="server" Text='<%#Eval("SupplierId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Create/Update" ItemStyle-Width="05%">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ChkCreate" CssClass="ChkCreate" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSavePermission" CssClass="Chk_Create" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Supplier" ItemStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSupplier" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        
                                    </Columns>
                                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblRecordNotFound" runat="server" Text="Record Not Found."></asp:Label>
                                    </EmptyDataTemplate>
                                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#44545E" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#7C6F57" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </div>
                             </div>--%>

                        
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Description"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="11"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label class="control-label">Attachment</label>
                            </div>
                            <div class="col-md-4">
                                <input id="btnImageUp" type="button" onclick="javascript: return LoadDocUploader();"
                                    class="TransactionalButton btn btn-primary btn-sm" value="Others Document..." />
                            </div>
                        </div>
                        <%--<div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label3" runat="server" class="control-label" Text="Attachment"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <input id="btnImageUp" type="button" onclick="javascript: return LoadDocUploader();"
                                    class="TransactionalButton btn btn-primary btn-sm" value="Supplier Documents..." />
                            </div>
                        </div>--%>
                        <div id="DocumentInfo">
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="javascript: return SaveValidation();"
                                    OnClick="btnSave_Click" CssClass="btn btn-primary btn-sm" TabIndex="12" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="13" Text="Clear" CssClass="btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearActionOnButtonClick();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="InfoPanel" class="panel panel-default">
                <div class="panel-heading">
                    Supplier Information
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSearchName" runat="server" class="control-label" Text="Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSearchCode" runat="server" class="control-label" Text="Code"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchCode" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSearchEmail" runat="server" class="control-label" Text="Email"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchEmail" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSearchPhone" runat="server" class="control-label" Text="Phone"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchPhone" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSearchContact" runat="server" class="control-label" Text="Contact Person"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSearchContact" runat="server" CssClass="form-control" TabIndex="5"></asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="Label2" runat="server" class="control-label" Text="Supplier Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList runat="server" ID="ddlSrcSupplierType" CssClass="form-control">
                                    <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Local" Value="Local"></asp:ListItem>
                                    <asp:ListItem Text="Foreign" Value="Foreign"></asp:ListItem>
                                    <asp:ListItem Text="Both (Local & Foreign)" Value="Both"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <%--Right Left--%>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                    CssClass="btn btn-primary btn-sm" TabIndex="6" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearAction();" TabIndex="7" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information
                </div>
                <div class="panel-body">
                    <asp:GridView ID="gvSupplierInfo" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                        ForeColor="#333333" PageSize="50" OnPageIndexChanging="gvSupplierInfo_PageIndexChanging"
                        OnRowDataBound="gvSupplierInfo_RowDataBound" OnRowCommand="gvSupplierInfo_RowCommand"
                        CssClass="table table-bordered table-condensed table-responsive" TabIndex="9">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("SupplierId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Code" HeaderText="Code" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SupplierTypeId" HeaderText="Supplier Type" ItemStyle-Width="20%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" OnClientClick="return confirm('Do you want to edit?');" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# Bind("SupplierId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# Bind("SupplierId") %>' ImageUrl="~/Images/delete.png" Text=""
                                        AlternateText="Delete" ToolTip="Delete" OnClientClick="return confirm('Do you want to delete?');" />
                                    &nbsp;<asp:ImageButton ID="ImgShowDocuments" runat="server" CausesValidation="False" CommandName="CmdShowDocuments"
                                        CommandArgument='<%# Bind("SupplierId") %>' ImageUrl="~/Images/document.png" Text=""
                                        AlternateText="Documents" ToolTip="Documents" OnClientClick='<%# Eval("SupplierId", "ShowUploadedDocument(\"{0}\"); return false;") %>' />
                                </ItemTemplate>
                                <ControlStyle Font-Size="Small" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            <asp:Label ID="lblRecordNotFound" runat="server" Text="Record Not Found."></asp:Label>
                        </EmptyDataTemplate>
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#44545E" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#7C6F57" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </div>
            </div>

        </div>
        <div id="DocumentDialouge" style="display: none;">
            <iframe id="frmPrint" name="IframeName" width="100%" height="100%" runat="server"
                clientidmode="static" scrolling="yes"></iframe>
        </div>
    </div>

</asp:Content>
