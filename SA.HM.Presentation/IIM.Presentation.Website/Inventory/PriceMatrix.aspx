<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="PriceMatrix.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.PriceMatrix" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var PackageTable;
        var IsCanSave = false, IsCanEdit = false, IsCanDelete = false;
        $(document).ready(function () {
            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;

            $("#ContentPlaceHolder1_ddlServiceItem").select2({
                tags: false,
                allowClear: true,
                width: "99.75%",
                placeholder: "--- Please Select ---"
            });
            $("#ContentPlaceHolder1_ddlSearchServiceItem").select2({
                tags: false,
                allowClear: true,
                width: "99.75%",
                placeholder: "--- ALL ---"
            });
            PackageTable = $("#tblPackage").DataTable({
                data: [],
                columns: [
                    { title: "Package Name", data: "PackageName", width: "20%" },
                    { title: "Service Name", data: "ItemName", width: "20%" },
                    { title: "Price", data: "UnitPrice", width: "20%" },
                    { title: "Action", data: null, width: "20%" },
                    { title: "", data: "ServicePriceMatrixId", visible: false }
                ],
                rowCallback: (row, data, displayNum, displayIndex, dataIndex) => {

                    var tableRow = "";

                    if (displayIndex % 2 == 0) {
                        $('td', row).css('background-color', '#E3EAEB');
                    }
                    else {
                        $('td', row).css('background-color', '#FFFFFF');
                    }
                    if (IsCanEdit)
                        tableRow += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return EditPackage(" + data.ServicePriceMatrixId + ",'" + data.PackageName + "');\"> <img alt=\"Edit\" src=\"../Images/edit.png\" title='Edit' /> </a>";
                    if (IsCanDelete)
                        tableRow += "&nbsp;<a href=\"javascript:void();\" onclick=\"javascript:return DeletePackage(" + data.ServicePriceMatrixId + ",'" + data.PackageName + "');\"> <img alt=\"Delete\" src=\"../Images/delete.png\" title='Delete' /> </a>";

                    $('td:eq(' + (row.children.length - 1) + ')', row).html(tableRow);
                },
                info: false,
                ordering: false,
                processing: false,
                paging: false,
                filter: false,
                language: {
                    emptyTable: "No Data Found"
                }

            });
            SaveBandwidth($("#chkIsSaveBandwidth"));
            SearchPackage(1, 1);
        });

        function SearchPackage(pageNumber, isCurrentOrPreviousPage) {
            let gridRecordsCount = PackageTable.data().length;
            var packageName = $("#txtSearchPackageName").val();
            var itemId = $("#ContentPlaceHolder1_ddlSearchServiceItem").val();
            PageMethods.SearchPackage(itemId, packageName, gridRecordsCount, pageNumber, isCurrentOrPreviousPage, OnSearchPackageSucceeded, OnSearchPackageFailed);
            return false;
        }

        function OnSearchPackageSucceeded(result) {
            PackageTable.clear();
            PackageTable.rows.add(result.GridData);
            PackageTable.draw();

            $("#GridPagingContainer ul").html("");

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);
        }

        function EditPackage(servicePriceMatrixId, packageName) {
            if (!confirm(`Want to Edit ${packageName}?`))
                return true;

            PageMethods.GetPackageById(servicePriceMatrixId, OnGetPackageSucceeded, OnSearchPackageFailed);
            return false;
        }

        function OnGetPackageSucceeded(result) {

            $("#PackageEntry").dialog({
                autoOpen: true,
                modal: true,
                width: '100%',
                height: 480,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: `Update Package - ${result.PackageName}`,
                show: 'slide'
            });
            if (result.UplinkFrequencyId > 0)
                $("#chkIsSaveBandwidth").attr("checked", true).trigger('change');
            $("#ContentPlaceHolder1_hfServicePriceMatrixId").val(result.ServicePriceMatrixId);
            $("#ContentPlaceHolder1_ddlServiceItem").val(result.ItemId).trigger('change');
            $("#txtPackageName").val(result.PackageName);
            $("#ContentPlaceHolder1_txtUnitPrice").val(result.UnitPrice);
            $("#ContentPlaceHolder1_txtDescription").val(result.Description);
            $("#ContentPlaceHolder1_ddlUplinkFrequency").val(result.UplinkFrequencyId);
            $("#ContentPlaceHolder1_ddlUplinkFrequencyUnit").val(result.UplinkFrequencyUnit);
            $("#ContentPlaceHolder1_ddlDownlinkFrequency").val(result.DownlinkFrequencyId);
            $("#ContentPlaceHolder1_ddlDownlinkFrequencyUnit").val(result.DownlinkFrequencyUnit);
            $("#ContentPlaceHolder1_ddlShareRatio").val(result.ShareRatio);
            if (IsCanEdit)
                $("#btnSave").val("Update").show();
            else
                $("#btnSave").hide();
            return false;
        }

        function DeletePackage(servicePriceMatrixId, packageName) {
            if (!confirm(`Want to Delete ${packageName}?`))
                return true;
            PageMethods.DeletePriceMatrix(servicePriceMatrixId, OnSaveOrUpdateSucceed, OnSaveOrUpdateFailed);
            return false;
        }

        function OnSearchPackageFailed(error) {
            toastr.error(error, "", { timeOut: 5000 });
        }

        function Save() {
            var ServicePriceMatrixId = $("#ContentPlaceHolder1_hfServicePriceMatrixId").val();
            var ItemId = $("#ContentPlaceHolder1_ddlServiceItem").val();
            var PackageName = $("#txtPackageName").val();
            var UnitPrice = $("#ContentPlaceHolder1_txtUnitPrice").val();
            var Description = $("#ContentPlaceHolder1_txtDescription").val();
            var UplinkFrequencyId = $("#ContentPlaceHolder1_ddlUplinkFrequency").val();
            var UplinkFrequencyUnit = $("#ContentPlaceHolder1_ddlUplinkFrequencyUnit").val();
            var DownlinkFrequencyId = $("#ContentPlaceHolder1_ddlDownlinkFrequency").val();
            var DownlinkFrequencyUnit = $("#ContentPlaceHolder1_ddlDownlinkFrequencyUnit").val();
            var ShareRatio = $("#ContentPlaceHolder1_ddlShareRatio").val();

            if (ItemId == "0") {
                toastr.warning("Enter Service Name.");
                $("#ContentPlaceHolder1_ddlServiceItem").focus();
                return false;
            }
            if (!PackageName) {
                toastr.warning("Enter Package Name.");
                $("#txtPackageName").focus();
                return false;
            }

            if (!UnitPrice) {
                toastr.warning("Enter Service Rate.");
                $("#ContentPlaceHolder1_txtUnitPrice").focus();
                return false;
            }
            if ($("#chkIsSaveBandwidth").is(":checked")) {
                if (UplinkFrequencyId == "0") {
                    toastr.warning("Select Uplink.");
                    $("#ContentPlaceHolder1_ddlUplinkFrequency").focus();
                    return false;
                } if (UplinkFrequencyUnit == "0") {
                    toastr.warning("Select Select Unit.");
                    $("#ContentPlaceHolder1_ddlUplinkFrequencyUnit").focus();
                    return false;
                } if (DownlinkFrequencyId == "0") {
                    toastr.warning("Select Downlink.");
                    $("#ContentPlaceHolder1_ddlDownlinkFrequency").focus();
                    return false;
                } if (DownlinkFrequencyUnit == "0") {
                    toastr.warning("Enter Select Unit.");
                    $("#ContentPlaceHolder1_ddlDownlinkFrequencyUnit").focus();
                    return false;
                }
            }
            else {
                UplinkFrequencyId = "0";
                UplinkFrequencyUnit = "0";
                DownlinkFrequencyId = "0";
                DownlinkFrequencyUnit = "0";
                ShareRatio = "0";
            }
            var priceMatrix = {
                ServicePriceMatrixId,
                ItemId,
                PackageName,
                UplinkFrequencyId,
                UplinkFrequencyUnit,
                DownlinkFrequencyId,
                DownlinkFrequencyUnit,
                ShareRatio,
                UnitPrice,
                Description
            }

            PageMethods.SaveOrUpdatePriceMatrix(priceMatrix, OnSaveOrUpdateSucceed, OnSaveOrUpdateFailed);
            return false;
        }

        function OnSaveOrUpdateSucceed(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
            if (result.IsSuccess) {
                $("#PackageEntry").dialog('close');
                var activeLink = Math.trunc($("#GridPagingContainer").find("ul li.active").text());
                SearchPackage(activeLink, 1);
                Clear();
            }
            return false;
        }

        function OnSaveOrUpdateFailed(error) {
            toastr.error(error.get_message());
        }

        function Clear() {
            $("#ContentPlaceHolder1_hfServicePriceMatrixId").val("0");
            $("#ContentPlaceHolder1_ddlServiceItem").val("0").trigger('change');
            $("#txtPackageName").val("");
            $("#ContentPlaceHolder1_txtUnitPrice").val("");
            $("#ContentPlaceHolder1_txtDescription").val("");
            $("#ContentPlaceHolder1_ddlUplinkFrequency").val("0");
            $("#ContentPlaceHolder1_ddlUplinkFrequencyUnit").val("0");
            $("#ContentPlaceHolder1_ddlDownlinkFrequency").val("0");
            $("#ContentPlaceHolder1_ddlDownlinkFrequencyUnit").val("0");
            $("#ContentPlaceHolder1_ddlShareRatio").val("0");
            if (IsCanSave)
                $("#btnSave").val('Save').show();
            else
                $("#btnSave").hide();
        }

        function NewPackage() {
            Clear();
            $("#PackageEntry").dialog({
                autoOpen: true,
                modal: true,
                width: '100%',
                height: 480,
                closeOnEscape: true,
                resizable: false,
                fluid: true,
                title: "New Package",
                show: 'slide'
            });

            if (IsCanSave)
                $("#btnSave").show();
            else
                $("#btnSave").hide();
        }
        function SaveBandwidth(control) {
            if ($(control).is(":checked")) {
                $("#dvBandwidth").show();
                $("#dvContacts").show();
            }
            else {
                $("#dvBandwidth").hide();
                $("#dvContacts").hide();

            }
        }
    </script>
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfServicePriceMatrixId" runat="server" Value="0"></asp:HiddenField>
    <div id="PackageEntry" class="panel panel-default" style="display: none;">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2 ">
                        <asp:Label ID="Label2" runat="server" class="control-label required-field" Text="Service Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlServiceItem" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2 ">
                        <asp:Label ID="Label20" runat="server" class="control-label required-field" Text="Package Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtPackageName" CssClass="form-control" TabIndex="2" runat="server"
                            ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-4">
                        <input id="chkIsSaveBandwidth" type="checkbox" onchange="SaveBandwidth(this)" />
                        &nbsp;<label class="control-label">Save Bandwidth?</label>
                    </div>
                </div>
                <fieldset id="dvBandwidth">
                    <legend>Bandwidth</legend>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="Label4" runat="server" class="control-label required-field" Text="Uplink"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlUplinkFrequency" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="Label7" runat="server" class="control-label required-field" Text="Unit"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlUplinkFrequencyUnit" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label></label>
                            <asp:Label ID="Label3" runat="server" class="control-label required-field" Text="Downlink"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlDownlinkFrequency" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="Label6" runat="server" class="control-label required-field" Text="Unit"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlDownlinkFrequencyUnit" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 ">
                            <asp:Label ID="Label8" runat="server" class="control-label" Text="Share Ratio"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlShareRatio" runat="server" CssClass="form-control" TabIndex="3">
                            </asp:DropDownList>
                        </div>
                    </div>
                </fieldset>

                <div class="form-group">
                    <div class="col-md-2 ">
                        <asp:Label ID="Label23" runat="server" class="control-label required-field" Text="Service Rate"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtUnitPrice" TabIndex="4" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2 ">
                        <asp:Label ID="Label5" runat="server" class="control-label" Text="Description"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox TextMode="MultiLine" ID="txtDescription" runat="server" class="form-control" Text="" TabIndex="5"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row" style="padding-bottom: 0; padding-top: 10px;">
                <div class="col-md-12">
                    <input id="btnSave" type="button" value="Save" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return Save();" />
                    <input id="btnClear" type="button" value="Clear" class="TransactionalButton btn btn-primary btn-sm"
                        onclick="javascript: return Clear();" />
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Search Price Matrix
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2 ">
                        <asp:Label ID="Label9" runat="server" class="control-label" Text="Service Name"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlSearchServiceItem" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Package Name</label>
                    </div>
                    <div class="col-md-10">
                        <asp:TextBox ID="txtSearchPackageName" runat="server" class="form-control" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <input type="button" class="btn btn-primary btn-sm" value="Search" onclick="SearchPackage(1, 1)" />
                        <input type="button" class="btn btn-primary btn-sm" value="Clear" />
                        <input type="button" class="btn btn-primary btn-sm" value="New Package" onclick="NewPackage()" />
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-heading">
            Search Information
        </div>
        <div class="panel-body">
            <table id="tblPackage" class="table table-bordered table-condensed table-responsive">
            </table>
            <div class="childDivSection">
                <div class="text-center" id="GridPagingContainer">
                    <ul class="pagination">
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
