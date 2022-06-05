<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="VMVehicleInformation.aspx.cs" Inherits="HotelManagement.Presentation.Website.VehicleManagement.VMVehicleInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ContentPlaceHolder1_ddlAccountHeadId").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            CommonHelper.ApplyIntigerValidation(); // quantity
            CommonHelper.ApplyDecimalValidation(); // quantitydecimal
            GridPaging(1, 1);
        });
        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {
            LoadSearch(pageNumber, IsCurrentOrPreviousPage);
        }
        function LoadSearch(pageNumber, IsCurrentOrPreviousPage) {
            //string , string , string , int , int , int , string 
             var vehicleName = $("#<%=txtVehicleNameSrc.ClientID %>").val();
            var model = $("#<%=txtModelNameSrc.ClientID %>").val();
            var airConditioning = $("#<%=ddlAirConditionSrc.ClientID %>").val();
            var manufacturerId = $("#<%=ddlManufacturerIdSrc.ClientID %>").val();
            var accountHeadId = $("#<%=ddlAccountHeadIdSrc.ClientID %>").val();
            var vehicleTypeId = $("#<%=ddlVehicleTypeSrc.ClientID %>").val();
            var status = $("#<%=ddlStatusSrc.ClientID %>").val();
            var gridRecordsCount = $("#VehicleTable tbody tr").length;
            $.ajax({

                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './VMVehicleInformation.aspx/SearchGridPaging',

                data: "{'vehicleName':'" + vehicleName.trim() + "', 'model':'" + model.trim() + "', 'airConditioning':'" + airConditioning.trim() + "','manufacturerId':'" + manufacturerId.trim() + "','accountHeadId':'" + accountHeadId.trim() + "','vehicleTypeId':'" + vehicleTypeId.trim() + "','status':'" + status.trim() + "', 'gridRecordsCount':'" + gridRecordsCount + "', 'pageNumber':'" + pageNumber + "', 'isCurrentOrPreviousPage':'" + IsCurrentOrPreviousPage + "'}",
                dataType: "json",
                success: function (data) {

                    LoadTable(data);
                },
                error: function (result) {
                    //CommonHelper.AlertMessage(result.d.AlertMessage);
                }
            });
            return false;
        }
        function LoadTable(searchData) {
            var rowLength = $("#VehicleTable tbody tr").length;
            var dataLength = searchData.length;
            $("#VehicleTable tbody").empty();
            $("#GridPagingContainer ul").empty();
            var i = 0;

            if (searchData.d.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"5\" >No Data Found</td> </tr>";
                $("#VehicleTable tbody ").append(emptyTr);
                return false;
            }

            $.each(searchData.d.GridData, function (count, gridObject) {
                var tr = "";
                if (i % 2 == 0) {
                    tr = "<tr style='background-color:#FFFFFF;'>";
                }
                else {
                    tr = "<tr style='background-color:#E3EAEB;'>";
                }
                tr += "<td style='width:20%;'>" + gridObject.VehicleName + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.NumberPlate + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.ManufacturerName + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.ModelName + "</td>";

                tr += "<td style='width:20%;'> <a onclick=\"javascript:return FillFormEdit(" + gridObject.Id + ",\'" + gridObject.VehicleName + '\');"' + "title='Edit' href='javascript:void();'><img src='../Images/edit.png' alt='Edit'></a>"
                tr += "&nbsp;&nbsp;<a href='#' onclick= 'DeleteData(" + gridObject.Id + ")' ><img alt='Delete' src='../Images/delete.png' /></a>";
                tr += "</td>";

                tr += "<td style='display:none'>" + gridObject.Id + "</td>";
                tr += "</tr>";

                $("#VehicleTable tbody").append(tr);

                tr = "";
                i++;
            });
            //PerformCancleAction();
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(searchData.d.GridPageLinks.NextButton);
            return false;
        }
        function SaveAndClose() {
            var id =  $("#<%=hfId.ClientID %>").val();
            var vehicleName =  $("#<%=txtVehicleName.ClientID %>").val();
            var vehicleTypeId =  $("#<%=ddlVehicleType.ClientID %>").val();
            var manuFactureId =  $("#<%=ddlManufacturerId.ClientID %>").val();
            var numberPlate =  $("#<%=txtNumberPlate.ClientID %>").val();
            var taxNumber =  $("#<%=txtTaxNumber.ClientID %>").val();
            var taxYear =  $("#<%=txtTaxValidationYear.ClientID %>").val();
            var modelName =  $("#<%=txtModelName.ClientID %>").val();
            var modelYear =  $("#<%=txtModelYear.ClientID %>").val();
            var engineType =  $("#<%=txtEngineType.ClientID %>").val();
            var engineCapacity =  $("#<%=txtEngineCapacity.ClientID %>").val();
            var passengerCapacity =  $("#<%=txtPassengerCapacity.ClientID %>").val();
            var fare =  $("#<%=txtFare.ClientID %>").val();
            var fuelType =  $("#<%=txtFuelType.ClientID %>").val();
            var fuelTankCapacity = $("#<%=txtFuelTankCapacity.ClientID %>").val();
            var insuranceNo = $("#<%=txtInsuranceNumber.ClientID %>").val();
            var status = $("#<%=ddlStatus.ClientID %>").val();
            var bodyType = $("#<%=txtBodyType.ClientID %>").val();
            var mileage = $("#<%=txtMileage.ClientID %>").val();
            var isAirBag = $("#<%=ddlIsAirBagAvailable.ClientID %>").val();
            var airCondition = $("#<%=ddlAirCondition.ClientID %>").val();
            var accountHeadId = $("#<%=ddlAccountHeadId.ClientID %>").val();
            var isABSavail = $("#<%=ddlIsABSAvailable.ClientID %>").val();

            if (vehicleName == "") {
                toastr.warning("Please insert vehicle name.");
                $("#<%=txtVehicleName.ClientID %>").focus();
                return false;
            }
            else if (vehicleTypeId == "0") {
                toastr.warning("Please select vehicle type.");
                $("#<%=ddlVehicleType.ClientID %>").focus();
                return false;
            }
            else if (manuFactureId == "0") {
                toastr.warning("Please select vehicle manufacturer.");
                $("#<%=ddlManufacturerId.ClientID %>").focus();
                return false;
            }
            else if (numberPlate == "") {
                toastr.warning("Please insert vehicle number plate.");
                $("#<%=txtNumberPlate.ClientID %>").focus();
                return false;
            }

            var VehicleBO = {
                Id: id,
                VehicleName: vehicleName,
                VehicleTypeId: vehicleTypeId,
                AccountHeadId: accountHeadId,
                ManufacturerId: manuFactureId,
                NumberPlate: numberPlate,
                TaxNumber: taxNumber,
                TaxValidationYear: taxYear,
                ModelName: modelName,
                ModelYear: modelYear,
                EngineType: engineType,
                EngineCapacity: engineCapacity,
                Fare: fare,
                PassengerCapacity: passengerCapacity,
                FuelType: fuelType,
                FuelTankCapacity: fuelTankCapacity,
                InsuranceNumber: insuranceNo,
                Status: status,
                BodyType: bodyType,
                Mileage: mileage,
                IsAirBagAvailable: isAirBag,
                AirConditioningType: airCondition,
                IsABSEnable: isABSavail
            }

             PageMethods.SaveUpdate(VehicleBO,OnSaveSucceed, OnFailed);
            return false;
        }
        
        function OnSaveSucceed(result) {
            if (result.IsSuccess == true) {
                CommonHelper.SpinnerClose();
                CommonHelper.AlertMessage(result.AlertMessage);
                PerformClearAction();
                $("#AddNewDiv").dialog('close');
                GridPaging(1, 1);

            }
            return false;
        }
        function OnFailed(error) {
            toastr.error(error);
        }
        function FillFormEdit(id, name) {

            if (!confirm("Do you want to edit ?")) {
                return false;
            }
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
                title: "Update - " + name,
                show: 'slide'
            });
            CommonHelper.SpinnerOpen();
            PageMethods.FillForm(id, OnFillFormSucceed, OnFailed);
            return false;
        }
        function OnFillFormSucceed(result) {
            PerformClearAction();

            $("#<%=hfId.ClientID %>").val(result.Id);
            $("#<%=txtVehicleName.ClientID %>").val(result.VehicleName);
            $("#<%=ddlVehicleType.ClientID %>").val(result.VehicleTypeId);
            $("#<%=ddlManufacturerId.ClientID %>").val(result.ManufacturerId);
            $("#<%=txtNumberPlate.ClientID %>").val(result.NumberPlate);
            $("#<%=txtTaxNumber.ClientID %>").val(result.TaxNumber);
            $("#<%=txtTaxValidationYear.ClientID %>").val(result.TaxValidationYear);
            $("#<%=txtModelName.ClientID %>").val(result.ModelName);
            $("#<%=txtModelYear.ClientID %>").val(result.ModelYear);
            $("#<%=txtEngineType.ClientID %>").val(result.EngineCapacity);
            $("#<%=txtEngineCapacity.ClientID %>").val(result.EngineCapacity);
            $("#<%=txtPassengerCapacity.ClientID %>").val(result.PassengerCapacity);
            $("#<%=txtFare.ClientID %>").val(result.Fare);
            $("#<%=txtFuelType.ClientID %>").val(result.FuelType);
            $("#<%=txtFuelTankCapacity.ClientID %>").val(result.FuelTankCapacity);
            $("#<%=txtInsuranceNumber.ClientID %>").val(result.InsuranceNumber);
            var status = result.Status + '';
            $("#<%=ddlStatus.ClientID %>").val(status);
            $("#<%=txtBodyType.ClientID %>").val(result.BodyType);
            $("#<%=txtMileage.ClientID %>").val(result.Mileage);
            var IsAirBagAvailable = result.IsAirBagAvailable + '';
            $("#<%=ddlIsAirBagAvailable.ClientID %>").val(IsAirBagAvailable);
            var AirConditioningType = result.AirConditioningType + '';
            $("#<%=ddlAirCondition.ClientID %>").val(AirConditioningType);

            $("#<%=ddlAccountHeadId.ClientID %>").val(result.AccountHeadId).trigger('change');
            var IsABSEnable = result.IsABSEnable + '';
            $("#<%=ddlIsABSAvailable.ClientID %>").val(IsABSEnable);

            $("#<%=btnSaveClose.ClientID %>").val("Update");
            CommonHelper.SpinnerClose();
        }
        function CreateNew() {
            PerformClearAction();

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
                title: "Create New",
                show: 'slide'
            });
            return false;
        }
        function PerformClearAction() {
            $("#<%=hfId.ClientID %>").val("0");
            $("#<%=txtVehicleName.ClientID %>").val("");
            $("#<%=ddlVehicleType.ClientID %>").val("0");
            $("#<%=ddlManufacturerId.ClientID %>").val("0");
            $("#<%=txtNumberPlate.ClientID %>").val("");
            $("#<%=txtTaxNumber.ClientID %>").val("");
            $("#<%=txtTaxValidationYear.ClientID %>").val("");
            $("#<%=txtModelName.ClientID %>").val("");
            $("#<%=txtModelYear.ClientID %>").val("");
            $("#<%=txtEngineType.ClientID %>").val("");
            $("#<%=txtEngineCapacity.ClientID %>").val("");
            $("#<%=txtPassengerCapacity.ClientID %>").val("");
            $("#<%=txtFare.ClientID %>").val("");
            $("#<%=txtFuelType.ClientID %>").val("");
            $("#<%=txtFuelTankCapacity.ClientID %>").val("");
            $("#<%=txtInsuranceNumber.ClientID %>").val("");
            $("#<%=ddlStatus.ClientID %>").val();
            $("#<%=txtBodyType.ClientID %>").val("");
            $("#<%=txtMileage.ClientID %>").val("");
            $("#<%=ddlIsAirBagAvailable.ClientID %>").val("0");
            $("#<%=ddlAirCondition.ClientID %>").val("0");
            $("#<%=ddlAccountHeadId.ClientID %>").val("0");
            $("#<%=ddlIsABSAvailable.ClientID %>").val("0");

            $("#<%=btnSaveClose.ClientID %>").val("Save");
        }
        function DeleteData(id) {
            if (!confirm("Do you want to delete?")) {
                return;
            }
            PageMethods.DeleteData(id, OnDeleteSucceed, OnFailed);
            return false;
        }
        function OnDeleteSucceed(result) {
            LoadSearch(1, 1);
            CommonHelper.AlertMessage(result.AlertMessage);
            return false;
        }
    </script>
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>

    <div id="InfoPanel" class="panel panel-default">
        <div class="panel-heading">
            Driver Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label ">Vehicle Name</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtVehicleNameSrc" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Number Plate</label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtNumberPlateSrc" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label ">Manufacturer</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlManufacturerIdSrc" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label ">Type</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlVehicleTypeSrc" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                            <label class="control-label">Model Name</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtModelNameSrc" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                    <div class="col-md-2">
                            <label class="control-label">Air Conditioning</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlAirConditionSrc" runat="server" CssClass="form-control" TabIndex="1">
                                <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                <asp:ListItem Text="AC" Value="AC"></asp:ListItem>
                                <asp:ListItem Text="Non-AC" Value="NonAC"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <label class="control-label">Account Head</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlAccountHeadIdSrc" runat="server" CssClass="form-control" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label ">Status</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlStatusSrc" runat="server" CssClass="form-control" TabIndex="1">
                            <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Active" Value="true"></asp:ListItem>
                            <asp:ListItem Text="In Active" Value="false"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                &nbsp;
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClientClick="javascript: return GridPaging(1,1);"
                            CssClass="TransactionalButton btn btn-primary btn-sm" TabIndex="5" />
                        <asp:Button ID="btnAdd" runat="server" Text="New Vehicle" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return CreateNew();" TabIndex="6" />
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
            <div class="form-group" id="TableContainer" style="overflow: scroll;">
                <table class="table table-bordered table-condensed table-responsive" id="VehicleTable"
                    style="width: 100%;">
                    <thead>
                        <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'>
                            <th style="width: 20%;">Vehicle Name
                            </th>
                            <th style="width: 20%;">Number Plate
                            </th>
                            <th style="width: 20%;">Manufacturer
                            </th>
                            <th style="width: 20%;">Model
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
    <div id="AddNewDiv" style="display: none">
        <div id="AddPanel" class="panel panel-default">
            <%--<div class="panel-heading">
                New CNF
            </div>--%>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div id="NameDiv">
                            <div class="col-md-2">
                                <label class="control-label ">Vehicle Name</label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtVehicleName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>

                        </div>
                        <div class="col-md-2">
                            <label class="control-label ">Type</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlVehicleType" runat="server" CssClass="form-control" TabIndex="1">
                            </asp:DropDownList>
                        </div>

                    </div>
                    <div class="form-group" id="otherAreaDiv">
                        <div class="col-md-2">
                            <label class="control-label ">Manufacturer</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlManufacturerId" runat="server" CssClass="form-control" TabIndex="1">
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-2">
                            <label class="control-label">Number Plate</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtNumberPlate" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Tax Reg. Number</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtTaxNumber" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label ">Tax Validation Year</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtTaxValidationYear" CssClass="form-control quantity" runat="server"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Model Name</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtModelName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                        
                        <div class="col-md-2">
                            <label class="control-label">Model Year</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtModelYear" runat="server" CssClass="form-control quantity" TabIndex="2"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Engine Type</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtEngineType" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label">Engine Capacity (cc)</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtEngineCapacity" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Passenger Capacity</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtPassengerCapacity" CssClass="form-control quantity" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label">Fare</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtFare" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Fuel Type</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtFuelType" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label">Fuel Tank Capacity (L)</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtFuelTankCapacity" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">

                        <div class="col-md-2">
                            <label class="control-label ">Insurance Number</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtInsuranceNumber" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label ">Status</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" TabIndex="1">
                                <asp:ListItem Text="Active" Value="true"></asp:ListItem>
                                <asp:ListItem Text="InActive" Value="false"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Body Type</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtBodyType" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label">Mileage (Km/L)</label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtMileage" CssClass="form-control quantitydecimal" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Is Air Bag Available</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlIsAirBagAvailable" runat="server" CssClass="form-control" TabIndex="1">
                                <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                <asp:ListItem Text="No" Value="false"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <label class="control-label">Air Conditioning</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlAirCondition" runat="server" CssClass="form-control" TabIndex="1">
                                <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                <asp:ListItem Text="AC" Value="AC"></asp:ListItem>
                                <asp:ListItem Text="Non-AC" Value="NonAC"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="control-label">Account Head</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlAccountHeadId" runat="server" CssClass="form-control" TabIndex="1">
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-2">
                            <label class="control-label">Is ABS Avilable</label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlIsABSAvailable" runat="server" CssClass="form-control" TabIndex="1">
                                <asp:ListItem Text="--- Please Select ---" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                <asp:ListItem Text="No" Value="false"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <%--<div class="form-group">
                        <div class="col-md-2">
                            <label for="Attachment" class="control-label">Attachment</label>
                        </div>
                        <div class="col-md-10">
                            <input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Attach" onclick="AttachFile()" />
                        </div>
                    </div>
                    <div id="DocumentInfo">
                    </div>--%>
                    
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
