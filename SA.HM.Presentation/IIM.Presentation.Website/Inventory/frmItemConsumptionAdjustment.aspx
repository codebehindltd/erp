<%@ Page Title="Item Consumption Adjustment" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmItemConsumptionAdjustment.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.frmItemConsumptionAdjustment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            $("#<%=btnAdjustment.ClientID%>").hide();
        });


        function SearchConsumption() {
            var issueNumber = $("#<%=txtIssueNumber.ClientID%>").val();
            if (issueNumber == "") {
                toastr.warning("Please Enter Issue Number");
                return false;
            }
            PageMethods.GetOutIdByIssueNumber(issueNumber, OnSuccess, OnFailed);
            return false;
        }

        function OnSuccess(returnInfo) {
            if (returnInfo.Data == null) {
                $("#tbLocation tbody tr").remove();
                $("#tbCosumptionItemDetails tbody tr").remove();
                $("#<%=btnAdjustment.ClientID%>").hide();
                toastr.warning("Please Enter Valid Issue Number");
                return false;
            }
            GetConsumptionDetails(returnInfo.Data);
        }

        function OnFailed(error) {
            toastr.error(error.get_message());
        }
        function GetConsumptionDetails(outId) {
            PageMethods.GetConsumptionDetails(outId, OnSuccessLoading, OnFailedLoading);
        }
        function OnSuccessLoading(adjustments) {
            var tablestr = "", options = "", tr = "", i = 0, j = 0, k = 1;
            $("#tbCosumptionItemDetails tbody tr").remove();
            if (adjustments.ProductOutDetails == null) {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#tbCosumptionItemDetails tbody").html(emptyTr);
                return false;
            }
            else {
                $("#<%=btnAdjustment.ClientID%>").show();
                //Generate location table 
                tr += "<tr style=\"background-color:#E3EAEB;\">";
                if (adjustments.ProductOutDetails[0].CostCenterFrom!=null)
                    tr += "<td style=\"width:30%;\">" + adjustments.ProductOutDetails[0].CostCenterFrom + " - " + adjustments.ProductOutDetails[0].LocationFrom + "</td>";
                if (adjustments.ProductOutDetails[0].CostCenterTo!=null)
                    tr += "<td style=\"width:30%;\">" + adjustments.ProductOutDetails[0].CostCenterTo + " - " + adjustments.ProductOutDetails[0].LocationTo + "</td>";
                tr += "</tr>";

                $("#tbLocation tbody").html(tr);
                tr = "";

                //Generate Unithead dropdown
                options = "<select id='##' class='form-control' id=\"\">";
                options += "<option value=\"" + "0" + "\">" + "-Please Select-" + "</option>";

                $.each(adjustments.AllUnitHeads, function (count, headList) {
                    options += "<option value=\"" + headList.UnitHeadId + "\">" + headList.HeadName + "</option>";
                });
                options += " </select>";

                $.each(adjustments.ProductOutDetails, function (count, gridObject) {
                    i++;

                    totalRow = $("#tbCosumptionItemDetails tbody tr").length;
                    if ((totalRow % 2) == 0) {
                        tr += "<tr style=\"background-color:#E3EAEB;\">";
                    }
                    else {
                        tr += "<tr style=\"background-color:#FFFFFF;\">";
                    }


                    tr += "<td style=\"width:30%;\">" + gridObject.ProductName + "</td>";
                    tr += "<td style=\"width:30%;\">" + gridObject.Quantity + " " + gridObject.StockBy + "</td>";

                    tr += "<td style=\"width:40%;\">";

                    tr += "<div class=\"col-md-6\">";
                    if(gridObject.AdjustmentQuantity!=null)
                        tr += "<input value='" +  gridObject.AdjustmentQuantity + "' placeholder=\"Enter Quantity\" class=\"form-control quantitydecimal\" id ='q" + gridObject.OutDetailsId + "' />";
                    else
                        tr += "<input value='" + "" + "' placeholder=\"Enter Quantity\" class=\"form-control quantitydecimal\" id ='q" + gridObject.OutDetailsId + "' />";
                    tr += "</div>";

                    tr += "<div class=\"col-md-6\">" + options.replace('##', ('s' + gridObject.OutDetailsId)) + "</div>";
                    tr += "</td>";

                    tr += "<td style=\"display:none;\">" + gridObject.OutDetailsId + "</td>";
                    if (gridObject.AdjustmentQuantity!=null)
                        tr += "<td style=\"display:none;\">" + gridObject.AdjustmentQuantity + "</td>";
                    else
                        tr += "<td style=\"display:none;\">" + "" + "</td>";
                    if (gridObject.AdjustmentStockById!=null)
                        tr += "<td style=\"display:none;\">" + gridObject.AdjustmentStockById + "</td>";
                    else
                        tr += "<td style=\"display:none;\">" +"0"+ "</td>";

                    tr += "</tr>";

                    $("#tbCosumptionItemDetails tbody").append(tr);
                    if (gridObject.AdjustmentStockById!=null)
                        $("#s" + gridObject.OutDetailsId).val(gridObject.AdjustmentStockById);
                    else
                        $("#s" + gridObject.OutDetailsId).val(0);

                    tr = "";
                    
                });

                CommonHelper.ApplyDecimalValidation();
            }
        }
        function OnFailedLoading(error) {

            toastr.error(error.get_message());
        }

        function AdjustConsumption() {
            var ProductOutDetails = new Array();
            var adjustmentQuantity, adjustmentStockById;
            var preAdjustmentQuantity, preAdjustmentStockById;
            var inputId = "#q", selectId = "#s", outDetailsId;
            var adval, stockval;
            var validity = true;

            $("#tbCosumptionItemDetails tbody tr").each(function () {
                
                outDetailsId = $(this).find('td:eq(3)').text();
                inputId += outDetailsId;
                selectId += outDetailsId;

                preAdjustmentQuantity = $(this).find('td:eq(4)').text();
                preAdjustmentStockById = $(this).find('td:eq(5)').text();

                adval = $(this).find('td:eq(2)').find(inputId).val();
                stockval = $(this).find('td:eq(2)').find(selectId).val();

                if (adval != "")
                {
                    adjustmentQuantity = parseInt(adval);
                    if (stockval == 0)
                    {
                        validity = false;
                        toastr.warning("Select Unit Head");
                        $(this).find('td:eq(2)').find(selectId).focus();
                        return false;
                    }
                    adjustmentStockById = parseInt(stockval);
                }   
                else
                {
                    adjustmentQuantity = "";
                    adjustmentStockById = "";
                }

                //check value with previous value
                if (preAdjustmentQuantity != adval || preAdjustmentStockById != stockval)
                {
                    ProductOutDetails.push({
                        OutDetailsId: outDetailsId,
                        AdjustmentQuantity: adjustmentQuantity,
                        AdjustmentStockById: adjustmentStockById
                    });
                }
  
                inputId = "#q";
                selectId = "#s";
            });
            if (ProductOutDetails.length > 0 && validity==true)
                PageMethods.AdjustConsumption(ProductOutDetails, OnSuccessAdjust, OnFailedAdjust);
            return false;
        }

        function OnSuccessAdjust(returnInfo) {
            if (returnInfo.IsSuccess)
                toastr.success(returnInfo.AlertMessage.Message);
            else
                toastr.error(returnInfo.AlertMessage.Message);
            SearchConsumption();  
        }
        function OnFailedAdjust(error) {
            toastr.error(error.get_message());
        }

    </script>
    <div class="panel panel-default">
        <div class="form-horizontal">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Item Consumption Adjustment
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label CssClass="control-label required-field" Text="Issue Number" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtIssueNumber" CssClass="form-control" TabIndex="1" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <asp:Button ID="btnSearch" Text="Search" CssClass="TransactionalButton btn btn-primary btn-sm" runat="server" OnClientClick="return SearchConsumption();" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-6">
                            <table id="tbLocation" class="table table-bordered table-condensed table-responsive">
                                <thead>
                                    <tr style="background-color: #44545E">
                                        <th style="width: 30%;">From</th>
                                        <th style="width: 30%;">To</th>

                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>

                    </div>
                    <div class="form-group">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                Consumption Details
                            </div>
                            <div class="panel-body" id="cosumptionItemDiv">
                                <table style="width: 80%" id="tbCosumptionItemDetails" class="table table-bordered table-condensed table-responsive">
                                    <thead>
                                        <tr style="background-color: #44545E">
                                            <th style="width: 30%;">Item Name</th>
                                            <th style="width: 30%;">Consumption Quantity</th>
                                            <th style="width: 40%;">Adjustment Quantity</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnAdjustment" Text="Adjust" CssClass="TransactionalButton btn btn-primary btn-sm" runat="server" OnClientClick="return AdjustConsumption()" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
