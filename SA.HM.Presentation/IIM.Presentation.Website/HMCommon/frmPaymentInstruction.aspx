<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmPaymentInstruction.aspx.cs" Inherits="HotelManagement.Presentation.Website.PurchaseManagment.frmPaymentInstruction" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmPaymentInstruction.aspx' class='inActive'>Payment Instruction</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Payment Instruction</li>";
            var breadCrumbs = moduleName + formName;

            UploadComplete();

            PageMethods.GetPaymentInstructionsByWebMethod(OnGetPaymentInstructionsByWebMethodSucceeded, OnGetPaymentInstructionsByWebMethodFailed);
            
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);
            
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }            
        });

        
        function SaveInstructions() {

            var buttonStatus = $("#btnSave").val();

            var instruction = $("#<%=txtInstruction.ClientID %>").val();
            if (instruction == "") {
                toastr.warning("Please Insert Payment Instruction.");

                $("#ContentPlaceHolder1_instruction").focus();
                return false;
            }

            var UserId = $("#ContentPlaceHolder1_CurrentUserId").val();
            

            if (buttonStatus == "Save") {

                var documentId = $("#documentId").val();
                var ownerId = $("#ownerId").val();
                var imagePath = $("#ImagePath").val();

                $.ajax({
                    type: "POST",
                    url: "frmPaymentInstruction.aspx/SaveInstructions",
                    data: "{'DocumentId':'" + documentId + "', 'OwnerId':'" + ownerId + "', 'ImagePath':'" + imagePath + "','Instruction':'" + instruction + "','UserId':'" + UserId + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        alert(msg.d);
                        AddItem();
                        ClearContactContainer();
                    }
                });
            }
            else
            {
                var documentId = $("#<%=DocumentId.ClientID %>").val();
                var ownerId = $("#<%=OwnerId.ClientID %>").val();
                var imagePath = $("#<%=ImagePath.ClientID %>").val();
                
                $.ajax({
                    type: "POST",
                    url: "frmPaymentInstruction.aspx/UpdateInstructions",
                    data: "{'DocumentId':'" + documentId + "', 'OwnerId':'" + ownerId + "', 'ImagePath':'" + imagePath + "','Instruction':'" + instruction + "','UserId':'" + UserId + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        alert(msg.d);
                        location.reload();
                    }
                });
            }
            return false;
        }
        function PerformClearAction() {
            
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
            
            return false;
        }

        function LoadLogoUploader() {
            var randomId = +$("#ContentPlaceHolder1_RandomDocId").val();
            var path = "/HMCommon/Images/PaymentGatewayLogo/";
            var category = "PaymentInstruction";
            var iframeid = 'ifrmPaymentInstruction';
            var url = "/Common/FileUpload.aspx?Path=" + path + "&OwnerId=" + randomId + "&Category=" + category;
            document.getElementById(iframeid).src = url;

            $("#DocumentDialouge").dialog({
                autoOpen: true,
                modal: true,
                width: "40%",
                height: 300,
                closeOnEscape: false,
                resizable: false,
                title: "Documents Upload",
                show: 'slide'
            });
            return false;
        }

        function OnGetPaymentInstructionsByWebMethodSucceeded(result)
        {
            var guestDoc = result;
            var totalDoc = result.length;
            var row = 0;
            var imagePath = "";
            var guestDocumentTable = "";

            guestDocumentTable += "<table id = 'paymentInstructionList' style = 'width:100%' class='table table-bordered table-condensed table-responsive'>";
            guestDocumentTable += "<thead>";
            guestDocumentTable += "<tr style = 'color: White; background-color: #44545E; font-weight: bold;' >";
            guestDocumentTable += "<th align='left' scope='col' style='width: 10%'>Logo</th>";
            guestDocumentTable += "<th align = 'left' scope= 'col' style= 'width: 70%' >Instruction</th>";
            guestDocumentTable += "<th align = 'left' scope= 'col' style= 'width: 10%' >Action</th >";
            guestDocumentTable += "</tr>";
            guestDocumentTable += "</thead>";
            guestDocumentTable += "<tbody>";

            var UserId = $("#ContentPlaceHolder1_CurrentUserId").val();

            for (row = 0; row < totalDoc; row++) {
                if (row % 2 == 0) {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:#E3EAEB;'>";
                }
                else {
                    guestDocumentTable += "<tr id='trdoc" + row + "' style='background-color:White;'>";
                }

                if (guestDoc[row].Path != "") {
                    imagePath = "<img src='" + guestDoc[row].Path + guestDoc[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                }
                else
                    imagePath = "";

                guestDocumentTable += "<td align='left' style='width: 10%'>" + imagePath + "</td>";
                guestDocumentTable += "<td align='left' style='width: 70%'>" + guestDoc[row].Instruction + "</td>";

                guestDocumentTable += "<td align='left' style='width: 10%'>";
                guestDocumentTable += "&nbsp;<img src='../Images/edit.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return EditDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Edit Information' border='0' />";
                guestDocumentTable += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteDoc('" + guestDoc[row].DocumentId + "', '" + row + "')\" alt='Delete Information' border='0' />";
                guestDocumentTable += "</td>";
                guestDocumentTable += "</tr>";

            }

            guestDocumentTable += "</tbody>";
            guestDocumentTable += "</table>";

            
            $("#ContentPlaceHolder1_PaymentInstructionList").html(guestDocumentTable);

            return false;
        }

        function OnGetPaymentInstructionsByWebMethodFailed(error)
        {

        }

        function UploadComplete() {
            var randomId = $("#ContentPlaceHolder1_RandomDocId").val();
            PageMethods.GetUploadedDocByWebMethod(randomId, OnGetUploadedDocByWebMethodSucceeded, OnGetUploadedDocByWebMethodFailed);
            return false;
        }

        
        function OnGetUploadedDocByWebMethodSucceeded(result) {
            $("#ContentPlaceHolder1_DocumentInfo").html(result);
            $("#btnSave").val("Save");

            return false;
        }

        function DeleteDoc(docId, rowIndex) {

            if (!confirm("Do you want to delete the instruction?")) {
                return false;
            }

            $.ajax({
                type: "POST",
                url: "frmPaymentInstruction.aspx/DeleteInstructions",
                data: "{'DocumentId':'" + docId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    //alert(msg.d);
                    $("#trdoc" + rowIndex).remove();
                }
            });
        }

        function EditDoc(docId, rowIndex) {

            if (!confirm("Do you want to edit the instruction?")) {
                return false;
            }

            $.ajax({
                type: "POST",
                url: "frmPaymentInstruction.aspx/GetInstructions",
                data: "{'DocumentId':'" + docId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if(msg.d != null)
                    {
                        if(msg.d.DocumentId > 0)
                        {
                            $("#<%=DocumentId.ClientID %>").val(msg.d.DocumentId);
                            $("#<%=OwnerId.ClientID %>").val(msg.d.OwnerId);
                            $("#<%=ImagePath.ClientID %>").val(msg.d.ImagePath);
                            $("#<%=txtInstruction.ClientID %>").val(msg.d.Instruction);
                            
                            var imagePath = "<img src='" + msg.d.Path + msg.d.Name + "' style=\"width:170px; height: 140px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                            $("#ContentPlaceHolder1_DocumentInfo").html(imagePath);

                            $("#btnSave").val("Update");
                        }
                    }
                }
            });
        }

        function OnGetUploadedDocByWebMethodFailed(error) {
            alert(error.get_message());
        }

        function AddItem() {
            var documentId = $("#documentId").val();
            var ownerId = $("#ownerId").val();
            var UserId = $("#ContentPlaceHolder1_CurrentUserId").val();

            var instruction = $("#ContentPlaceHolder1_txtInstruction").val();
            if (instruction == "") {
                toastr.warning("Add Instruction.");
                $("#ContentPlaceHolder1_txtInstruction").focus();
                return false;
            }

            var imagePath = $("#ImagePath").val();
            var table = document.getElementById("paymentInstructionList");
            var totalRowCount = $('#tableId tbody tr').length;
            console.log(totalRowCount);
            
            var tr = "";
            tr += "<tr id='trdoc" + totalRowCount + "' style='background-color:#E3EAEB;'>";

            tr += "<td align='left' style='width: 10%'><img src='" + imagePath + "' style='width:40px; height: 40px; cursor: pointer; cursor: hand;' alt='Document Image' border='0'></td>";
            tr += "<td align='left' style='width: 70%'>" + instruction + "</td>";
            tr += "<td align='left' style='width: 10%'>";
            
            tr += "&nbsp;<img src='../Images/edit.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return EditDoc('" + documentId + "', '" + totalRowCount + "')\" alt='Edit Information' border='0' />";
            tr += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img src='../Images/delete.png' style=\"cursor: pointer; cursor: hand;\" onClick=\"javascript:return DeleteDoc('" + documentId + "', '" + totalRowCount + "')\" alt='Delete Information' border='0' />";
            tr += "</td>";
            tr += "</td>";
            
            tr += "</tr>";

            $("#paymentInstructionList tbody").prepend(tr);
        }
        
        function ClearContactContainer()
        {
            $("#documentId").val("");
            $("#ownerId").val("");
            $("#imagePath").val("");

            $("#ContentPlaceHolder1_DocumentId").val("");
            $("#ContentPlaceHolder1_OwnerId").val("");
            $("#ContentPlaceHolder1_ImagePath").val("");

            $("#ContentPlaceHolder1_txtInstruction").val("");
            $("#ContentPlaceHolder1_DocumentInfo").html("");
            $("#btnSave").val("Save");

            PerformClearAction();
        }


    </script>
    <div id="supplierDocuments" style="display: none;">
        <div id="imageDiv"></div>
    </div>
    <asp:HiddenField ID="RandomDocId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="CurrentUserId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="DocumentId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="OwnerId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="ImagePath" runat="server"></asp:HiddenField>
            
        <div id="EntryPanel" class="panel panel-default">
            <div class="panel-heading">
                Payment Instruction
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="row">
                        <div class="form-group">
                            <div class="col-md-1 text-right">
                                <asp:Label ID="lblPaymentImage" runat="server" class="control-label" Text="Logo"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <input id="btnLogoUp" type="button" onclick="javascript: return LoadLogoUploader();"
                                    class="TransactionalButton btn btn-primary btn-sm" value="Upload Logo" />
                            </div>
                            <div class="col-md-8 text-left">
                                <asp:Label ID="lblName" runat="server" class="control-label required-field" Text="Instruction"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group">
                            <div class="col-md-1">
                                
                            </div>
                            <div class="col-md-3">
                                <div id="DocumentInfo" runat="server" class="col-md-12 border border-white" style="min-height:150px; min-width: 100px; background-color: ghostwhite; border-radius: 5px;"></div>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtInstruction" runat="server" TabIndex="1" CssClass="form-control" TextMode="MultiLine" Height="150"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group" style="padding-top: 10px;">
                            <div class="col-md-12 text-right">
                                <input id="btnSave" type="button" value="Save" class="TransactionalButton btn btn-primary btn-sm" onclick="SaveInstructions()" />
                                <input id="btnCancelContact" type="button" value="Cancel" onclick="ClearContactContainer()"
                                    class="TransactionalButton btn btn-primary btn-sm" />
                            </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div id="PaymentInstructionList" runat="server" class="col-md-12">
                                
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="DocumentDialouge" style="display: none;">
            <iframe id="ifrmPaymentInstruction" name="IframeName" width="100%" height="100%" runat="server"
                clientidmode="static" scrolling="yes"></iframe>
        </div>
</asp:Content>