<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="frmNutritionValue.aspx.cs" Inherits="HotelManagement.Presentation.Website.Inventory.frmNutritionValue" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        
        $(document).ready(function () {

            IsCanSave = $('#ContentPlaceHolder1_hfSavePermission').val() == '1' ? true : false;
            IsCanEdit = $('#ContentPlaceHolder1_hfEditPermission').val() == '1' ? true : false;
            IsCanDelete = $('#ContentPlaceHolder1_hfDeletePermission').val() == '1' ? true : false;
            IsCanView = $('#ContentPlaceHolder1_hfViewPermission').val() == '1' ? true : false;
            $("#ContentPlaceHolder1_txtFormula").attr("disabled", true);

            
            LoadNutritionType();

            LoadNutrientInformation();

            LoadIngredients();
                    
            $("#myTabs").tabs();         
        });
        function getclicked() {
            console.log("mehedi hasan millat");
            $("#ContentPlaceHolder1_txtFormula").attr("disabled", false);
            $("#ContentPlaceHolder1_txtFormula").focus();
            return false;
        }
        var nutritionTypeList = new Array();
        var nutrientInfoList = new Array();

        function LoadNutritionType() {
            PageMethods.GetNutritionType(OnGetNutritionTypeSucceed, OnGetNutritionTypeFailed);
            return false;
        }
        function OnGetNutritionTypeSucceed(result) {
            nutritionTypeList = result;
            var tr = "";
            tr += "<tr>";
            var lengthOfResult = result.length;
            var widthOfNType = 80 / lengthOfResult;
            tr += "<td style='width:20%;'></td>";
            $.each(result, function (count, obj) {
                tr += "<td style='width:"+ widthOfNType +"%;'>" + obj.Name + "</td>";
            });
            tr += "</tr>";
            $("#NutritionValueTbl thead").append(tr);
            tr = "";
        }
        function OnGetNutritionTypeFailed() {
            toastr.error(error.get_message());
        }

        function LoadNutrientInformation() {
            PageMethods.GetNutrientInformation(OnGetNutrientInformationSucceed, OnGetNutrientInformationFailed);
            return false;
        }
        function OnGetNutrientInformationSucceed(result) {
            nutrientInfoList = result;
            var tr = "";
            tr += "<td style='width:20%;'>"+ "&nbsp;&nbsp;" +"Ingredient Name</td>";
            $.each(nutritionTypeList, function (ncount, nobj) {
                var td = "";
                td += "<td>";
                $.each(result, function (count, obj) {
                    if (nobj.NutritionTypeId == obj.NutritionTypeId) {
                        td += "&nbsp;&nbsp;" + obj.Name;
                    }
                });
                td += "</td>";
                tr += td;
            });
            $("#NutritionValueTbl thead").append(tr);
            td = "";
            tr = "";
        }
        function OnGetNutrientInformationFailed() {
            toastr.error(error.get_message());
        }
        function LoadIngredients() {
            PageMethods.GetIngredients(OnGetIngredientsSucceed, OnGetIngredientsFailed);
        }
        function OnGetIngredientsSucceed(result) {
            let first10 = result.slice(0, 10);
            console.log(first10);
            
            $.each(first10, function (count, obj) {
                var tr = "";
                tr += "<tr>";
                var td = "";
                td += "<td style='width:20%;'>" + obj.Name + "</td>";
                tr += td;
                
                $.each(nutritionTypeList, function (ncount, nobj) {
                    td = "<td>";
                    $.each(nutrientInfoList, function (icount, iobj) {
                        if (nobj.NutritionTypeId == iobj.NutritionTypeId) {
                            td += "&nbsp;&nbsp;" + "<input type='text' size='1' onclick='getclicked();' placeholder='" + obj.ItemId + iobj.NutrientId + nobj.NutritionTypeId + "' id='" + obj.ItemId + iobj.NutrientId + nobj.NutritionTypeId + "' \>";
                        }
                    });
                    td += "</td>";
                    tr += td;
                });
                tr += "</tr>";
                $("#NutritionValueTbl tbody").append(tr);
            });
            
        }
        function OnGetIngredientsFailed() {

        }

        function ValidationBeforeSave() {
            console.log($("#112").val());
        }
        
        function OnSearchTicketInformationSucceed(result) {
            var tr = "";
            $.each(result.GridData, function (count, gridObject) {

                tr += "<tr>";

                tr += "<td style='width:10%;'>" + gridObject.BillNumber + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.TransactionType + "</td>";
                tr += "<td style='width:30%;'>" + gridObject.CompanyName + "</td>";
                tr += "<td style='width:20%;'>" + gridObject.InvoiceAmount + "</td>";

                tr += "<td style='width:10%;'>" + gridObject.Status + "</td>";

                tr += "<td style=\"text-align: center; width:10%; cursor:pointer;\">";

                if (gridObject.IsCanEdit && IsCanEdit) {
                    tr += "&nbsp;&nbsp;<img src='../Images/edit.png' onClick= \"javascript:return TicketInfoEditWithConfirmation(" + gridObject.TicketId + ")\" alt='Edit'  title='Edit' border='0' />";
                }
                
                if (gridObject.IsCanDelete && IsCanDelete) {
                    tr += "&nbsp;&nbsp;<img src='../Images/delete.png' onClick= \"javascript:return TicketInformationDelete(" + gridObject.TicketId + ")\" alt='Delete'  title='Delete' border='0' />";
                }
                
                if (gridObject.IsCanChecked && IsCanSave) {
                    tr += "&nbsp;&nbsp;<img src='../Images/checked.png' onClick= \"javascript:return TicketInformationCheckWithConfirmation(" + gridObject.TicketId + ")\" alt='Check'  title='Check' border='0' />";
                }
                if (gridObject.IsCanApproved && IsCanSave) {
                    tr += "&nbsp;&nbsp;<img src='../Images/approved.png' onClick= \"javascript:return TicketInformationApprovalWithConfirmation(" + gridObject.TicketId + ")\" alt='Approve'  title='Approve' border='0' />";
                }
                
                tr += "&nbsp;&nbsp;<img src='../Images/ReportDocument.png'  onClick= \"javascript:return PerformBillPreviewAction(" + gridObject.TicketId + ")\" alt='Invoice' title='Invoice' border='0' />";
                
                //tr += "&nbsp;&nbsp;<img src='../Images/note.png'  onClick= \"javascript:return ShowDealDocuments('" + gridObject.ReceivedId + "')\" alt='Invoice' title='Receive Order Info' border='0' />";
                tr += "</td>";

                tr += "<td style='display:none;'>" + gridObject.TicketId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.CostCenterId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.TransactionId + "</td>";
                tr += "<td style='display:none;'>" + gridObject.ReferenceId + "</td>";

                tr += "</tr>";

                $("#TicketInformationGrid tbody").append(tr);

                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            CommonHelper.SpinnerClose();
            return false;
        }
    </script>
    
    <asp:HiddenField ID="hfSavePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Nutrition Value</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Nutrition Value</a></li>
        </ul>
        <div id="tab-1">
            <div class="panel panel-default">
                <div class="panel-heading">New Nutrition Value</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblFormula" runat="server" class="control-label" Text="Formula"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFormula" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div id="NutritionValue" style="overflow-y: scroll;">
                            <table id="NutritionValueTbl" class="table table-bordered table-condensed table-hover">
                                <thead>
                                </thead>
                                <tbody></tbody>
                                <tfoot></tfoot>
                            </table>
                        </div>
                        <div class="form-group" style="padding-top: 10px;">
                            <div class="col-md-12">
                                <input id="btnSave" type="button" value="Save" onclick="ValidationBeforeSave()"
                                    class="TransactionalButton btn btn-primary btn-sm" />
                                <input id="btnCancelTicket" type="button" value="Cancel" onclick="PerformClearActionWithConfirmation()"
                                    class="TransactionalButton btn btn-primary btn-sm" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Nutrition Value
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-12">
                                <input type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-large" value="Search" onclick="SearchTicketInformation(1, 1)" />
                                <input type="button" id="btnSearchCancel" class="TransactionalButton btn btn-primary btn-large" value="Clear" onclick="ClearSearch()" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>