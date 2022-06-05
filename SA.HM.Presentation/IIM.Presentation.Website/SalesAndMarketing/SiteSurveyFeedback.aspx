<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="SiteSurveyFeedback.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.SiteSurveyFeedback" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var ItemSelected = null;
        var AddedItem = new Array();
        var EditedItem = new Array();
        var DeletedItem = new Array();
        var siteSurveyId;
        var id = 0;
        var AllFlag = false;
        $(document).ready(function () {
            siteSurveyId = $.trim(CommonHelper.GetParameterByName("ssid"));
            $("#ContentPlaceHolder1_txtItem").focus();

            if (siteSurveyId != "" || siteSurveyId > 0) {

                PageMethods.CheckFeedback(siteSurveyId, CheckFeedbackSucceed, CheckFeedbackFailed)
            }
            function CheckFeedbackSucceed(result) {
                if (result != 0) {
                    PageMethods.LoadFeedbackDetailsById(result, LoadFeedbackDetailsByIdSucceed, LoadFeedbackDetailsByIdFailed)
                    return false;
                }
            }
            function LoadFeedbackDetailsByIdSucceed(result) {

                $('#ContentPlaceHolder1_btnSave').val("Update");
                $('#ContentPlaceHolder1_ddlEngineerName').val(result.SMSiteSurveyEngineerBOList).trigger('change');

                LoadTableForEdit(result.SMSiteSurveyFeedbackDetailsBOList);
                siteSurveyId = result.SMSiteSurveyFeedbackBO.SiteSurveyNoteId;
                $('#ContentPlaceHolder1_hfSiteSurveyFeedbackId').val(result.SMSiteSurveyFeedbackBO.Id);
                $('#ContentPlaceHolder1_txtSurveyFeedback').val(result.SMSiteSurveyFeedbackBO.SurveyFeedback);
                return false;
            }
            function CheckFeedbackFailed(error) {
                CommonHelper.AlertMessage(error.AlertMessage);
            }
            function LoadFeedbackDetailsByIdFailed(error) {
                CommonHelper.AlertMessage(error.AlertMessage);
            }
            CommonHelper.ApplyDecimalValidation();
            $('#ContentPlaceHolder1_ddlEngineerName').select2({
                tags: false,
                allowClear: true,
                width: "99.75%",
            });
            $("#ContentPlaceHolder1_ddlEngineerName").change(function () {

                var len = $('#ContentPlaceHolder1_ddlEngineerName option:selected').length;
                if (len > 1 ) {
                    var list = $('#ContentPlaceHolder1_ddlEngineerName').val();
                    if (jQuery.inArray("0", list) > -1) {
                        AllFlag = true;
                        $("#ContentPlaceHolder1_ddlEngineerName").val("0").trigger("change");
                        
                    }
                }
                
            });
            $("#ContentPlaceHolder1_txtItem").autocomplete({

                source: function (request, response) {
                    CommonHelper.SpinnerOpen();
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({ searchTerm: request.term }),
                        url: '../SalesAndMarketing/SiteSurveyFeedback.aspx/ItemSearch',
                        dataType: "json",
                        success: function (data) {

                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {

                                    label: m.Name,
                                    value: m.ItemId,
                                    ItemName: m.Name,
                                    ItemId: m.ItemId,
                                    StockBy: m.StockBy,
                                    UnitHead: m.UnitHead,
                                    StockQuantity: m.StockQuantity
                                };
                            });
                            response(searchData);
                        },
                        error: function (result) {
                            CommonHelper.AlertMessage(result.d.AlertMessage);
                        }
                    });
                },
                focus: function (event, ui) {
                    event.preventDefault();

                },
                select: function (event, ui) {
                    event.preventDefault();

                    ItemSelected = ui.item;
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_hfItemId").val(ui.item.value);
                    $("#ContentPlaceHolder1_lblUnitHead").html(ui.item.UnitHead);
                    CommonHelper.SpinnerClose();
                }
            });
        });
        function AddItemForFeedback() {

            var itemName = ($("#ContentPlaceHolder1_txtItem").val());
            var itemId = parseInt($("#ContentPlaceHolder1_hfItemId").val());

            if (itemName == "") {
                toastr.warning("Please Select an Item");
                $("#ContentPlaceHolder1_txtItem").focus();
                return false;
            }
            if ($("#ContentPlaceHolder1_txtQuantity").val() == "0") {
                toastr.warning("Please Add Quentity");
                $("#ContentPlaceHolder1_txtQuantity").focus();
                return false;
            }
            AddItem();
            return false;
        }
        function AddItem() {
            var Additm = _.findWhere(AddedItem, { ItemId: ItemSelected.ItemId });
            var Edititm = _.findWhere(EditedItem, { ItemId: ItemSelected.ItemId });

            if (Additm != null || Edititm != null) {
                toastr.warning("Same Item has Already Added. Duplicate Item Is Not Accepted.");
                $("#ContentPlaceHolder1_txtItem").focus();
                return false;
            }
            var unitHead = $("#ContentPlaceHolder1_lblUnitHead").html();
            var quantity = $("#ContentPlaceHolder1_txtQuantity").val();
            var tr = "";
            tr += "<tr>";

            tr += "<td style='width:35%;'>" + ItemSelected.ItemName + "</td>";

            tr += "<td style='width:25%;'>" + unitHead + "</td>";
            tr += "<td style='width:20%;'>" +
                "<input type='text' value='" + quantity + "' id='pp" + ItemSelected.ItemId + "' class='form-control quantitydecimal' onblur='ChangeQuantity(this)' />" +
                "</td>";
            tr += "<td style='width:20%;'>" +

                "<a href='javascript:void()' onclick= 'DeleteItem(this)' ><img alt='Delete' src='../Images/delete.png' title='Delete' /></a>";

            tr += "</td>";

            tr += "<td style='display:none;'>" + ItemSelected.ItemId + "</td>";
            tr += "<td style='display:none;'>0</td>";

            tr += "</tr>";

            $("#ItemForFeedBackTbl tbody").prepend(tr);
            tr = "";
            AddedItem.push({
                ItemId: parseInt(ItemSelected.ItemId, 10),
                Quantity: parseFloat(quantity),
                Id: 0
            });
            CommonHelper.ApplyDecimalValidation();
            CancelItemForFeedback();
            $("#ContentPlaceHolder1_txtItem").focus();
        }
        function ChangeQuantity(control) {

            var tr = $(control).parent().parent();
            var quantity = $.trim($(tr).find("td:eq(2)").find("input").val());

            if (quantity == "" || quantity == "0") {
                toastr.info("Quantity Cannot Be Zero Or Empty.");
                return false;
            }
            var itemId = parseInt($.trim($(tr).find("td:eq(4)").text()), 10);
            var SiteSurveyFeedbackDetailsId = parseInt($.trim($(tr).find("td:eq(5)").text()), 10);
            if (parseInt(SiteSurveyFeedbackDetailsId, 10) > 0) {
                var item = _.findWhere(EditedItem, { ItemId: itemId });
                var index = _.indexOf(EditedItem, item);
                EditedItem[index].Quantity = parseFloat(quantity)
            }
            else {
                item = _.findWhere(AddedItem, { ItemId: itemId });
                index = _.indexOf(AddedItem, item);
                AddedItem[index].Quantity = parseFloat(quantity);
            }

        }
        function DeleteItem(control) {

            if (!confirm("Do you want to delete item?")) { return false; }

            var tr = $(control).parent().parent();

            var itemId = parseInt($.trim($(tr).find("td:eq(4)").text()), 10);
            var SiteSurveyFeedbackDetailsId = parseInt($.trim($(tr).find("td:eq(5)").text()), 10);
            if (parseInt(SiteSurveyFeedbackDetailsId, 10) > 0) {
                var item = _.findWhere(EditedItem, { ItemId: itemId });
                var index = _.indexOf(EditedItem, item);
                DeletedItem.push(JSON.parse(JSON.stringify(item)));
                EditedItem.splice(index, 1);
            }
            else {
                AddedItem.splice(index, 1);
            }
            $(tr).remove();
        }

        function PerformSave() {
            var length = $("#ItemForFeedBackTbl tbody tr").length;
            id = $('#ContentPlaceHolder1_hfSiteSurveyFeedbackId').val();
            var EmpId = $('#ContentPlaceHolder1_ddlEngineerName').val();
            if (length < 1) {
                toastr.warning("Please Add Atleast one Item");
                $("#ContentPlaceHolder1_txtItem").focus();
                return false;
            }
            var EmpList = $('#ContentPlaceHolder1_hfSelectedEngineer').val(EmpId).val();
            var Feedback = $('#ContentPlaceHolder1_txtSurveyFeedback').val();
            if (EmpList == "") {
                toastr.warning("Please Add Atleast one Engineer");
                $("#ContentPlaceHolder1_ddlEngineerName").focus();
                return false;
            }
            PageMethods.SaveFeedback(id, siteSurveyId, Feedback, EmpList, AddedItem, EditedItem, DeletedItem, OnSaveFeedbackSucceeded, OnSaveFeedbackFailed);
            return false;
        }
        function OnSaveFeedbackSucceeded(result) {
            parent.ShowAlert(result.AlertMessage);
            parent.CloseDialog();
            if (typeof parent.GridPaging === "function")
                parent.GridPaging(1, 1);
        }
        function OnSaveFeedbackFailed(error) {
            CommonHelper.AlertMessage(error.AlertMessage);
        }
        function CancelItemForFeedback() {
            $("#ContentPlaceHolder1_lblUnitHead").html("");
            $("#ContentPlaceHolder1_txtQuantity").val("");
            $("#ContentPlaceHolder1_txtItem").val("").trigger('Change');
        }
        function LoadTableForEdit(data) {
            var tr = "";
            var totalRow = data.length, row = 0;
            var tr = "";

            for (row = 0; row < totalRow; row++) {
                tr += "<tr>";

                tr += "<td style='width:35%;'>" + data[row].ItemName + "</td>";

                tr += "<td style='width:25%;'>" + data[row].UnitHead + "</td>";
                tr += "<td style='width:20%;'>" +
                    "<input type='text' value='" + data[row].Quantity + "' id='pp" + data[row].ItemId + "' class='form-control quantitydecimal' onblur='ChangeQuantity(this)' />" +
                    "</td>";
                tr += "<td style='width:20%;'>" +

                    "<a href='javascript:void()' onclick= 'DeleteItem(this)' ><img alt='Delete' src='../Images/delete.png' title='Delete' /></a>";

                tr += "</td>";

                tr += "<td style='display:none;'>" + data[row].ItemId + "</td>";
                tr += "<td style='display:none;'>" + data[row].Id + "</td>";


                tr += "</tr>";

                $("#ItemForFeedBackTbl tbody").prepend(tr);
                tr = "";
                EditedItem.push({
                    ItemId: parseInt(data[row].ItemId, 10),
                    Quantity: parseFloat(data[row].Quantity),
                    Id: parseInt(data[row].Id, 10),
                });
                CommonHelper.ApplyDecimalValidation();
            }
        }
        //function Clean() {
        //    $("#ContentPlaceHolder1_txtSurveyFeedback").val("");
        //    $("#ContentPlaceHolder1_ddlEngineerName").val(null).trigger('change');
        //    $('#ContentPlaceHolder1_hfSiteSurveyFeedbackId').val("0");
        //    $('#ContentPlaceHolder1_btnSave').val("Save");
        //     $("#ItemForFeedBackTbl tbody").html("");
        //    return false;
        //}
    </script>

    <asp:HiddenField ID="hfItemId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSiteSurveyNoteId" runat="server" Value="0" />
    <asp:HiddenField ID="hfSelectedEngineer" runat="server" Value="" />
    <asp:HiddenField ID="hfSiteSurveyFeedbackId" runat="server" Value="0" />

    <div class="panel panel-default">
        <div class="panel-heading">Receive Info</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblItem" runat="server" class="control-label required-field" Text="Item Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtItem" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Quantity</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="quantitydecimal form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="required-field">Unit Head</label>
                    </div>
                    <div class="col-md-4">
                        <asp:Label ID="lblUnitHead" runat="server" class="form-control" Text=""></asp:Label>
                    </div>
                </div>
                <div class="form-group" style="padding-top: 10px;">
                    <div class="col-md-12">
                        <input id="btnAdd" type="button" value="Add" class="TransactionalButton btn btn-primary btn-sm" onclick="AddItemForFeedback()" />
                        <input id="btnCancel" type="button" value="Cancel" onclick="CancelItemForFeedback()"
                            class="TransactionalButton btn btn-primary btn-sm" />
                    </div>
                </div>
                <div id="Item" style="overflow-y: scroll;">
                    <table id="ItemForFeedBackTbl" class="table table-bordered table-condensed table-hover">
                        <thead>
                            <tr>
                                <th style="width: 35%;">Item Name</th>
                                <th style="width: 25%;">Unit Head</th>
                                <th style="width: 20%;">Quantity</th>
                                <th style="width: 20%;">Action</th>
                                <th style="display: none;">ItemId</th>
                                <th style="display: none;">SiteSurveyFeedbackDetailsId</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                        <tfoot></tfoot>

                    </table>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblRemarks" runat="server" class="control-label" Text="Survey Feedback"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtSurveyFeedback" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblEngineerName" runat="server" class="control-label" Text="Engineer Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlEngineerName" name="states[]" multiple="multiple" CssClass="form-control" runat="server" Style="width: 100%;"></asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return PerformSave();" CssClass="btn btn-primary btn-sm" />
                        <%--<asp:Button ID="btnClean" runat="server" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return Clean();" />--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
