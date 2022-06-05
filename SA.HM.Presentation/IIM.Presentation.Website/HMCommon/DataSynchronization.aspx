<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="DataSynchronization.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.DataSynchronization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var VoucherList = [];
        var RestaurantBill = [];
        $(document).ready(function () {
            $("#txtDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat
            }).datepicker("setDate", DayOpenDate);
        });
        function ChangeType(control) {
            var type = $(control).val();
            if (type == "Accounts") {
                $("#lblCompany").show();
                $("#lblProject").show();
                $("#tblVoucher").show();
                $("#tblRestaurantBill").hide();
                $("#ContentPlaceHolder1_ddlCompany").show();
                $("#ContentPlaceHolder1_ddlProject").show();
            }
            else if (type == "Restaurant") {
                $("#lblCompany").hide();
                $("#lblProject").hide();
                $("#tblVoucher").hide();
                $("#tblRestaurantBill").show();
                $("#ContentPlaceHolder1_ddlCompany").hide();
                $("#ContentPlaceHolder1_ddlProject").hide();
            }

        }

        function CheckAllVoucher() {
            if ($("#voucherCheck").is(":checked")) {
                $("#tblVoucher tbody tr").find("td:eq(0)").find("input:enabled").prop("checked", true);
            }
            else {
                $("#tblVoucher tbody tr").find("td:eq(0)").find("input:enabled").prop("checked", false);
            }
            if ($("#chekAllRestaurantBill").is(":checked")) {
                $("#tblRestaurantBill tbody tr").find("td:eq(0)").find("input:enabled").prop("checked", true);
            }
            else {
                $("#tblRestaurantBill tbody tr").find("td:eq(0)").find("input:enabled").prop("checked", false);
            }
        }

        function PopulateProjects(control) {
            let companyId = $(control).val();

            $("#ContentPlaceHolder1_ddlProject").empty().append('<option selected="selected" value="0">Loading...</option>');

            $.ajax({
                type: "POST",
                url: "./DataSynchronization.aspx/GetGLProjectByGLCompanyId",
                data: JSON.stringify({ companyId: companyId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    PopulateControlWithValueNTextField(response.d, $("#ContentPlaceHolder1_ddlProject"), $("#<%=CommonDropDownHiddenField.ClientID %>").val(), "Name", "ProjectId");
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
        }

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#tblVoucher tbody tr").length;
            var type = $("#ContentPlaceHolder1_ddlSynchronizationType").val();
            var companyId = $("#ContentPlaceHolder1_ddlCompany").val();
            var projectId = $("#ContentPlaceHolder1_ddlProject").val();

            var date = $("#txtDate").val();
            if (type == "0") {
                $("#ContentPlaceHolder1_ddlSynchronizationType").focus();
                toastr.warning("Please Select Synchronization Type.");
                return false;
            }
            if (type == "Accounts" && companyId == "0") {
                $("#ContentPlaceHolder1_ddlCompany").focus();
                toastr.warning("Please Select Company.");
                return false;
            }
            if (type == "Accounts" && projectId == "0") {
                $("#ContentPlaceHolder1_ddlProject").focus();
                toastr.warning("Please Select Project.");
                return false;
            }
            if (date == "") {
                $("#txtDate").focus();
                toastr.warning("Please Select Date.");
                return false;
            }
            date = CommonHelper.DateFormatToMMDDYYYY(date, '/');
            CommonHelper.SpinnerOpen();
            if (type == "Accounts")
                PageMethods.GetVoucherBySearchCriteria(companyId, projectId, date, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            else
                PageMethods.GetRestaurantBillBySearchCriteria(date, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadRestaurantBillSucceeded, OnLoadRestaurantBillFailed);
            return false;
        }

        function OnLoadObjectSucceeded(result) {

            $("#tblVoucher tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"6\" >No Data Found</td> </tr>";
                $("#tblVoucher tbody ").append(emptyTr);
                CommonHelper.SpinnerClose();
                return false;
            }

            var tr = "";
            let isChecked = false;
            $.each(result.GridData, function (count, gridObject) {
                isChecked = VoucherList.includes(gridObject.LedgerMasterId);
                tr += "<tr>";

                if (!gridObject.IsSynced) {
                    if (isChecked)
                        tr += "<td style=\"text-align:center; width:5%;\">  <input type='checkbox' checked='checked' value='" + gridObject.LedgerMasterId + "' /> </td>";
                    else
                        tr += "<td style=\"text-align:center; width:5%;\">  <input type='checkbox' value='" + gridObject.LedgerMasterId + "' /> </td>";
                }
                else {
                    tr += "<td style=\"width:5%;\"></td>";
                }

                //tr += "<td style=\"width:15%;\">" + new Date(gridObject.VoucherDate).format(innBoarDateFormat) + "</td>";
                tr += "<td style=\"width:20%;\">" + gridObject.VoucherDateString + "</td>";
                tr += "<td style=\"width:20%;\">" + gridObject.VoucherNo + "</td>";
                tr += "<td style=\"width:20%;\">" + gridObject.VoucherType + "</td>";
                tr += "<td style=\"width:20%;\">" + gridObject.GLStatus + "</td>";
                tr += "<td style=\"text-align: center; width:15%; cursor:pointer;\">";
                tr += "</td>";
                //if (gridObject.GLStatus == 'Submit' || gridObject.GLStatus == 'Pending') {
                //    tr += "<td style=\"text-align: center; width:8%; cursor:pointer;\"><img src='../Images/approved.png' onClick= \"javascript:return ApprovedIndividualVoucher('" + gridObject.LedgerMasterId + "', this)\" alt='Approved'  title='Approved' border='0' />";
                //    tr += "&nbsp;&nbsp;<img src='../Images/detailsInfo.png'  onClick= \"javascript:return VoucherDetails('" + gridObject.LedgerMasterId + "')\" alt='Details'  title='Details' border='0' />";
                //    tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return VoucherPreviewOption('" + gridObject.LedgerMasterId + "')\" alt='Voucher' title='Voucher' border='0' />";
                //    tr += "</td>";
                //}
                //else {
                //    tr += "<td style=\"text-align: center; width:8%; cursor:pointer;\">";
                //    tr += "&nbsp;&nbsp;<img src='../Images/detailsInfo.png'  onClick= \"javascript:return VoucherDetails('" + gridObject.LedgerMasterId + "')\" alt='Details' title='Details' border='0' />";
                //    tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return VoucherPreviewOption('" + gridObject.LedgerMasterId + "')\" alt='Voucher' title='Voucher' border='0' />";
                //    tr += "</td>";
                //}

                tr += "<td align='right' style=\"display:none;\">" + gridObject.LedgerMasterId + "</td>";
                tr += "<td align='right' style=\"display:none;\">" + gridObject.CreatedBy + "</td>";

                tr += "</tr>"

                $("#tblVoucher tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            CommonHelper.SpinnerClose();
            return false;
        }

        function OnLoadObjectFailed(error) {
            toastr.error(error);
            CommonHelper.SpinnerClose();
        }

        function OnLoadRestaurantBillSucceeded(result) {

            $("#tblRestaurantBill tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"6\" >No Data Found</td> </tr>";
                $("#tblRestaurantBill tbody ").append(emptyTr);
                CommonHelper.SpinnerClose();
                return false;
            }

            var tr = "";
            let isChecked = false;
            $.each(result.GridData, function (count, gridObject) {
                isChecked = RestaurantBill.includes(gridObject.BillId);
                tr += "<tr>";

                if (!gridObject.IsSynced) {
                    if (isChecked)
                        tr += "<td style=\"text-align:center; width:5%;\">  <input type='checkbox' checked='checked' value='" + gridObject.LedgerMasterId + "' /> </td>";
                    else
                        tr += "<td style=\"text-align:center; width:5%;\">  <input type='checkbox' value='" + gridObject.LedgerMasterId + "' /> </td>";
                }
                else {
                    tr += "<td style=\"width:5%;\"></td>";
                }

                tr += "<td style=\"width:25%;\">" + gridObject.BillNumber + "</td>";
                tr += "<td style=\"width:25%;\">" + gridObject.CostCenter + "</td>";
                tr += "<td style=\"width:25%;\">" + gridObject.RoundedGrandTotal + "</td>";
                tr += "<td style=\"text-align: center; width:20%; cursor:pointer;\">";
                tr += "</td>";

                tr += "<td align='right' style=\"display:none;\">" + gridObject.BillId + "</td>";

                tr += "</tr>"

                $("#tblRestaurantBill tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            CommonHelper.SpinnerClose();
            return false;
        }

        function OnLoadRestaurantBillFailed(error) {
            toastr.error(error);
            CommonHelper.SpinnerClose();
        }

        function SaveVoucherId(pageNumber, IsCurrentOrPreviousPage) {
            SaveVoucherIdTemporary();
            GridPaging(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }
        function SaveRestaurentBillId(pageNumber, IsCurrentOrPreviousPage) {
            SaveRestaurantBillIdTemporary();
            GridPaging(pageNumber, IsCurrentOrPreviousPage);
            return false;
        }

        function SaveVoucherIdTemporary() {
            var voucherId = 0, isDuplicate = false;

            $("#tblVoucher tbody tr").each(function (index, item) {

                if ($(this).find("td:eq(0)").find("input:enabled").is(':checked')) {
                    voucherId = parseInt($.trim($(item).find("td:eq(6)").text()));
                    isDuplicate = VoucherList.includes(voucherId);

                    if (!isDuplicate)
                        VoucherList.push(voucherId);
                }
            });
        }
        function SaveRestaurantBillIdTemporary() {
            var billId = 0, isDuplicate = false;

            $("#tblRestaurantBill tbody tr").each(function (index, item) {

                if ($(this).find("td:eq(0)").find("input:enabled").is(':checked')) {
                    billId = parseInt($.trim($(item).find("td:eq(5)").text()));
                    isDuplicate = RestaurantBill.includes(billId);

                    if (!isDuplicate)
                        RestaurantBill.push(billId);
                }
            });
        }

        function SyncData() {
            SaveVoucherIdTemporary();
            SaveRestaurantBillIdTemporary();

            if ($("#ContentPlaceHolder1_txtServerIp").val() == "") {
                toastr.warning("Enter Server IP.");
                $("#ContentPlaceHolder1_txtServerIp").focus();
                return false;
            }
            VoucherList.forEach(function (value, index) {
                $.when(GetVoucherInformation(value)).done(function (data) {

                    if (data.d != null) {

                        $.when(SyncVoucherInformation(data.d))
                            .done(function (response) {
                                if (response.Success) {
                                    UpdateVoucherSyncInformation(value, response.VoucherNo);
                                }
                                else
                                    toastr.info("Sync Operation failed for: " + data.d.VoucherNo);
                            });
                    }
                    else {
                        toastr.info("Data not found for: " + value);
                    }
                });
            });

            RestaurantBill.forEach(function (value, index) {
                $.when(GetRestaurantBillInformation(value)).done(function (data) {

                    if (data.d != null) {

                        $.when(SyncRestaurantBillInformation(data.d))
                            .done(function (response) {
                                if (response.Success) {
                                    UpdateRestaurantBillSyncInformation(value, response.BillNumber);
                                }
                                else
                                    toastr.info("Sync Operation failed for: " + data.d.BillNumber);
                            });
                    }
                    else {
                        toastr.info("Data not found for: " + value);
                    }
                });
            });

        }

        function GetVoucherInformation(voucherId) {
            return $.ajax({
                type: "POST",
                url: "./DataSynchronization.aspx/GetVoucherInformationById",
                data: JSON.stringify({ voucherId: voucherId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {

                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
        }

        function GetRestaurantBillInformation(billId) {
            return $.ajax({
                type: "POST",
                url: "./DataSynchronization.aspx/GetRestaurantBilllInformationById",
                data: JSON.stringify({ billId: billId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {

                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
        }

        function LoginToServer(obj) {
            let baseUrl = "http://" + $("#ContentPlaceHolder1_txtServerIp").val() + "/api";
            return $.ajax({
                type: 'POST',
                url: baseUrl + '/Account/Login',
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    toastr.success("Log in Successfull");

                },
                error: function (error) {
                    if (error.responseJSON != undefined)
                        toastr.warning(error.responseJSON.error_description);
                    else
                        toastr.warning("The password is incorrect.");
                    //setTimeout(function () {
                    // window.location.href = "/HMCommon/frmInnboardDashboard.aspx";
                    //}, 1000);

                }
            });
        }

        function SyncVoucherInformation(data) {

            let baseUrl = "http://" + $("#ContentPlaceHolder1_txtServerIp").val() + "/api";
            return $.ajax({
                type: 'POST',
                url: baseUrl + '/GeneralLedger/Sync',
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                },
                error: function (error) {
                    if (error.responseJSON != undefined)
                        toastr.warning(error.responseJSON.error_description);
                    else
                        toastr.warning("Sync Unsuccessful.");
                    //setTimeout(function () {
                    // window.location.href = "/HMCommon/frmInnboardDashboard.aspx";
                    //}, 1000);

                }
            });
        }
        function SyncRestaurantBillInformation(data) {

            let baseUrl = "http://" + $("#ContentPlaceHolder1_txtServerIp").val() + "/api";
            return $.ajax({
                type: 'POST',
                url: baseUrl + '/RestaurantBill/Sync',
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                },
                error: function (error) {
                    if (error.responseJSON != undefined)
                        toastr.warning(error.responseJSON.error_description);
                    else
                        toastr.warning("Sync Unsuccessful.");
                }
            });
        }

        function UpdateVoucherSyncInformation(id, VoucherNo) {
            return $.ajax({
                type: "POST",
                url: "./DataSynchronization.aspx/UpdateVoucherSyncInformation",
                data: JSON.stringify({ id: id, voucherNo: VoucherNo }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    CommonHelper.AlertMessage(response.d.AlertMessage);
                    let pageNumber = parseInt($("#GridPagingContainer ul li.active a").text());
                    GridPaging(pageNumber, 1);
                    VoucherList = [];
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
        }

        function UpdateRestaurantBillSyncInformation(id, billNumber) {
            return $.ajax({
                type: "POST",
                url: "./DataSynchronization.aspx/UpdateRestaurantBillSyncInformation",
                data: JSON.stringify({ id: id, billNumber: billNumber }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    CommonHelper.AlertMessage(response.d.AlertMessage);
                    let pageNumber = parseInt($("#GridPagingContainer ul li.active a").text());
                    GridPaging(pageNumber, 1);
                    RestaurantBill = [];
                },
                failure: function (response) {
                    toastr.error(response.d);
                }
            });
        }

    </script>
    <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
    <div class="panel panel-default">
        <div class="panel-heading">
            Data Synchronization
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Synchronization Type</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlSynchronizationType" runat="server" CssClass="form-control" TabIndex="1" onchange="ChangeType(this)">
                            <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                            <asp:ListItem Value="Accounts">Accounts</asp:ListItem>
                            <asp:ListItem Value="Restaurant">Restaurant</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label id="lblCompany" class="control-label required-field">Company</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control" TabIndex="2" onchange="PopulateProjects(this)">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label required-field">Date</label>
                    </div>
                    <div class="col-md-4">
                        <input type="text" id="txtDate" class="form-control" />
                    </div>
                    <div class="col-md-2">
                        <label id="lblProject" class="control-label required-field">Project</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control" TabIndex="3">
                        </asp:DropDownList>
                    </div>

                </div>
                <div class="row">
                    <div class="col-md-12">
                        <input type="button" value="Search" class="TransactionalButton btn btn-primary btn-sm" onclick="GridPaging(1, 1)" tabindex="5" />
                    </div>
                </div>
                <div class="row" style="padding-top: 10px;">
                    <div class="col-md-12">
                        <div style="overflow-x: scroll;">
                            <table id="tblVoucher" style="display:none;" class="table table-hover table-bordered table-condensed table-responsive">
                                <thead>
                                    <tr>
                                        <td style="text-align: center; width: 5%;">
                                            <input type="checkbox" value="" id="voucherCheck" onclick="CheckAllVoucher()" />
                                        </td>
                                        <td style="width: 20%;">Voucher Date
                                        </td>
                                        <td style="width: 20%;">Voucher No
                                        </td>
                                        <td style="width: 20%;">Voucher Type
                                        </td>
                                        <td style="width: 20%;">Status
                                        </td>
                                        <td style="width: 15%;">Action
                                        </td>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                            <table id="tblRestaurantBill" style="display:none;" class="table table-hover table-bordered table-condensed table-responsive">
                                <thead>
                                    <tr>
                                        <td style="text-align: center; width: 5%;">
                                            <input type="checkbox" value="" id="chekAllRestaurantBill" onclick="CheckAllVoucher()" />
                                        </td>
                                        <td style="width: 25%;">Invoice No
                                        </td>
                                        <td style="width: 25%;">Outlate Name
                                        </td>
                                        <td style="width: 25%;">Bill Amount
                                        </td>
                                        <td style="width: 20%;">Action
                                        </td>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                            <div class="text-center" id="GridPagingContainer">
                                <ul class="pagination">
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" style="padding-top: 10px;">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Sync Server IP</label>
                    </div>
                    <div class="col-md-4">
                        <input disabled="disabled" type="text" id="txtServerIp" class="form-control" runat="server" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <input type="button" value="Data Sync" class="TransactionalButton btn btn-primary btn-sm" onclick="SyncData()" tabindex="6" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
