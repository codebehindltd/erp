<%@ Page Title="" Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="SalesNoteEntry.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.SalesNoteEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var ItemTable, ServiceTable;
        $(document).ready(function () {

            var quotationId = $.trim(CommonHelper.GetParameterByName("qid"));
            var isFromInventory = $.trim(CommonHelper.GetParameterByName("isInv")) == '1';

            if (quotationId != "")
                FillForm(quotationId);

            if (isFromInventory)
                $("#dvdeliverRemarks").show();
            else
                $("#dvdeliverRemarks").hide();

            var IsSalesNoteEnable = '<%=IsSalesNoteEnable%>' == 'True';

            ItemTable = $("#tblItem").DataTable({
                data: [],
                columns: [
                    { title: "Item Name", "data": "ItemName", sWidth: '40%' },
                    { title: "Quantity", "data": "Quantity", sWidth: '10%' },
                    { title: "Stock By", "data": "HeadName", sWidth: '10%' },
                    { title: "Sales Note", "data": "SalesNote", sWidth: '40%', visible: IsSalesNoteEnable },
                    { title: "", "data": "QuotationDetailsId", visible: false },
                    { title: "", "data": "IsEdited", visible: false, "defaultContent": "0" }
                ],
                columnDefs: [
                    {
                        "targets": 3,
                        "render": function (data, type, full, meta) {
                            return '<textarea onchange="UpdateItemEditFlag(this)" class="form-control" rows="2" cols="20"></textarea>';
                        }
                    }],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }
                    $('td:eq(4) textarea', nRow).val(aData.SalesNote);
                },
                //pageLength: UserInfoFromDB.GridViewPageSize,
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
            ServiceTable = $("#tblService").DataTable({
                data: [],
                columns: [
                    { title: "Item Name", "data": "ItemName", sWidth: '40%' },
                    { title: "Quantity", "data": "Quantity", sWidth: '10%' },
                    { title: "Stock By", "data": "HeadName", sWidth: '10%' },
                    { title: "Sales Note", "data": "SalesNote", sWidth: '40%', visible: IsSalesNoteEnable },
                    { title: "", "data": "QuotationDetailsId", visible: false },
                    { title: "", "data": "IsEdited", visible: false, "defaultContent": "0" }
                ],
                columnDefs: [
                    {
                        "targets": 3,
                        "render": function (data, type, full, meta) {
                            return '<textarea onchange="UpdateServiceEditFlag(this)" class="form-control" rows="2" cols="20"></textarea>';
                        }
                    }],
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

                    if (iDisplayIndex % 2 == 0) {
                        $('td', nRow).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', nRow).css('background-color', '#FFFFFF');
                    }
                    $('td:eq(4) textarea', nRow).val(aData.SalesNote);
                },
                //pageLength: UserInfoFromDB.GridViewPageSize,
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
            if (IsSalesNoteEnable)
                $("#btnDiv").show();
            else
                $("#btnDiv").hide();
        });

        function FillForm(id) {
            $("#ContentPlaceHolder1_hfQuotationId").val(id);
            PageMethods.LoadQuotation(id, OnLoadQuotationSucceeded, OnLoadQuotationFailed);
            return false;
        }

        function OnLoadQuotationSucceeded(result) {

            ItemTable.clear();
            ItemTable.rows.add(result.QuotationItemDetails);
            ItemTable.draw();

            ServiceTable.clear();
            ServiceTable.rows.add(result.QuotationServiceDetails);
            ServiceTable.draw();

            if (result.Quotation.IsSalesNoteFinal) {
                $("#tblItem").find('textarea').attr("disabled", true);
                $("#tblService").find('textarea').attr("disabled", true);
                $("#btnSave").hide();
                $("#btnClear").hide();
                $("#btnFinal").hide();
            }
            LoadDiscountItem(result.QuotationDetails);
            return false;
        }

        function OnLoadQuotationFailed(error) {
            toastr.error(error);
        }
        function UpdateSalesNote() {

            var id, note, isEdited;
            var notes = new Array();

            for (var i = 0; i < ItemTable.data().length; i++) {

                isEdited = (ItemTable.row(i).data().IsEdited == 1);
                if (isEdited) {
                    note = $.trim(ItemTable.cell(i, 4).nodes().to$().find('textarea').val());
                    id = ItemTable.row(i).data().QuotationDetailsId;

                    notes.push({
                        QuotationDetailsId: id,
                        SalesNote: note
                    });
                }

            }
            for (var i = 0; i < ServiceTable.data().length; i++) {

                isEdited = (ServiceTable.row(i).data().IsEdited == 1);
                if (isEdited) {
                    note = $.trim(ServiceTable.cell(i, 4).nodes().to$().find('textarea').val());
                    id = ServiceTable.row(i).data().QuotationDetailsId;

                    notes.push({
                        QuotationDetailsId: id,
                        SalesNote: note
                    });
                }

            }
            if (notes.length > 0) {
                PageMethods.UpdateSalesNote(notes, OnSuccessUpdate, OnFailedUpdate);
            }
            else {
                toastr.info("Nothing changed");
            }
            return false;
        }
        function UpdateItemEditFlag(row) {

            var row = $(row).parents('tr');
            ItemTable.cell(row, 6).data("1");
        }
        function UpdateServiceEditFlag(row) {

            var row = $(row).parents('tr');
            ServiceTable.cell(row, 6).data("1");
        }
        function OnSuccessUpdate(result) {
            if (result.IsSuccess) {
                //$("#CreateNewDialog").dialog('close');
                //$(window.parent.document.getElementById("btnCloseDialog")).trigger('click');
                CommonHelper.AlertMessage(result.AlertMessage);
                //Clear();
            }
        }

        function OnFailedUpdate(error) {
            toastr.error(error);
        }

        function Clear() {
            ItemTable.clear().draw();
            ServiceTable.clear().draw();
            return false;
        }

        function Finalize() {
            if (!confirm("Want to final sales note?"))
                return false;

            var id, note, isEdited;
            var notes = new Array();

            for (var i = 0; i < ItemTable.data().length; i++) {

                isEdited = (ItemTable.row(i).data().IsEdited == 1);
                if (isEdited) {
                    note = $.trim(ItemTable.cell(i, 4).nodes().to$().find('textarea').val());
                    id = ItemTable.row(i).data().QuotationDetailsId;

                    notes.push({
                        QuotationDetailsId: id,
                        SalesNote: note
                    });
                }

            }
            for (var i = 0; i < ServiceTable.data().length; i++) {

                isEdited = (ServiceTable.row(i).data().IsEdited == 1);
                if (isEdited) {
                    note = $.trim(ServiceTable.cell(i, 4).nodes().to$().find('textarea').val());
                    id = ServiceTable.row(i).data().QuotationDetailsId;

                    notes.push({
                        QuotationDetailsId: id,
                        SalesNote: note
                    });
                }

            }
            var id = +$("#ContentPlaceHolder1_hfQuotationId").val();
            PageMethods.FinalizeSalesNote(id, notes, OnApproveSucceeded, OnApproveFailed);
            return false;
        }
        function OnApproveSucceeded(result) {
            if (result.IsSuccess) {
                $("#tblItem").find('textarea').attr("disabled", true);
                $("#tblService").find('textarea').attr("disabled", true);
                $("#btnSave").hide();
                $("#btnClear").hide();
                $("#btnFinal").hide();
                if (typeof parent.SearchQuotationForSalesNote == "function")
                    parent.SearchQuotationForSalesNote();
                CommonHelper.AlertMessage(result.AlertMessage);
                var quotationId = $.trim(CommonHelper.GetParameterByName("qid"));
                SendMail(quotationId);
            }
        }
        function OnApproveFailed(error) {
            toastr.error(error);
        }
        var SendMail = (quotationId) => {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "./Reports/SMQuotationDetailsReport.aspx?id=" + quotationId + "&smail=1",
                //data: JSON.stringify({ quotationId: quotationId }),
                dataType: "json",
                success: (result) => {

                },
                error: (error) => {
                }
            });
        }
        function LoadDiscountItem(QuotationDetails) {
            $("#QuotaionBanquet").hide();
            $("#QuotaionRestaurant").hide();
            $("#QuotaionRoom").hide();
            $("#QuotaionServiceOutLet").hide();
            
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
            $("#QuotaionRoom").show();
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
            $("#QuotaionRestaurant").show();
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
            $("#QuotaionBanquet").show();
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
            $("#QuotaionServiceOutLet").show();
        }
    </script>
    <asp:HiddenField ID="hfQuotationId" Value="0" runat="server" />
    <div class="panel panel-default">
        <div class="panel-heading">
            Item Information
        </div>
        <div class="panel-body">
            <table id="tblItem" class="table table-bordered table-condensed table-responsive">
            </table>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Service Information
        </div>
        <div class="panel-body">
            <table id="tblService" class="table table-bordered table-condensed table-responsive">
            </table>

        </div>
    </div>
    <div id="QuotaionBanquet" class="panel panel-default" style="display: none;">
        <div class="pnlDetails panel-heading">Banquet Information</div>
        <div class="pnlDetails panel-body">
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
    </div>
    <div id="QuotaionRestaurant" style="display: none;" class="panel panel-default">
        <div  class="pnlDetails panel-heading">Restaurant Information</div>
        <div  class="pnlDetails panel-body">
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
    </div>
    <div id="QuotaionRoom" style="display: none;"  class="panel panel-default">
        <div class="pnlDetails panel-heading">Guest Room Information</div>
        <div class="pnlDetails panel-body">
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
    </div>
    <div id="QuotaionServiceOutLet" style="display: none;" class="panel panel-default">
        <div class="pnlDetails panel-heading">Service Outlet Information</div>
        <div class="pnlDetails panel-body">
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
    <div class="row" id="dvdeliverRemarks" style="display: none;">
        <div class="col-md-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Deliver Remarks</label>
                    </div>
                    <div class="col-md-4">
                        <textarea class="form-control" rows="2" cols="20"></textarea>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row" id="btnDiv">
        <div class="col-md-12">
            <input type="button" id="btnSave" class="TransactionalButton btn btn-primary btn-sm" value="Update" onclick="javascript: return UpdateSalesNote();" />
            <input type="button" id="btnClear" class="TransactionalButton btn btn-primary btn-sm" value="Clear" onclick="javascript: return Clear();" />
            <input type="button" id="btnFinal" class="TransactionalButton btn btn-primary btn-sm" value="Update & Final" onclick="javascript: return Finalize();" />
        </div>
    </div>
</asp:Content>
