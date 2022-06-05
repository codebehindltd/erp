<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmVatAdjustment.aspx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.frmVatAdjustment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx'>Company Information</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Vat Adjustment</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });
            $("#SearchPanel").hide('slow');

            var txtSearchDate = '<%=txtSearchDate.ClientID%>'
            $('#' + txtSearchDate).datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: innBoarDateFormat                
            });
        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#tblRstBillSearch tbody tr").length;

            var searchDate = $("#<%=txtSearchDate.ClientID %>").val();
            var costCenterId = $("#<%=ddlCostCenter.ClientID %>").val();
            PageMethods.SearchBillAndLoadGridInformation(searchDate, costCenterId, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnSuccess, OnFail);
            return false;
        }
        function OnSuccess(result) {
            // alert('success');                    
            //$("#ltlTableWiseItemInformation").html(result);

            $("#tblRstBillSearch tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#tblRstBillSearch tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {
                totalRow = $("#tblRstBillSearch tbody tr").length;
                //totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                if (gridObject.IsDeleted == true) {
                    tr += "<td align='left' style=\"width:5%; cursor:pointer;\"> <input type='checkbox' value=" + gridObject.BillId + "></td>";
                }
                else {
                    tr += "<td align='left' style=\"width:5%; cursor:pointer;\"> <input type='checkbox' checked value=" + gridObject.BillId + "></td>";
                }
                tr += "<td align='left' style=\"width:35%; cursor:pointer;\">" + gridObject.BillNumber + "</td>";
                tr += "<td align='left' style=\"width:40%; cursor:pointer;\">" + gridObject.CustomerName + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.GrandTotal + "</td>";


                tr += "</tr>"

                $("#tblRstBillSearch tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);

            return false;
        }
        function OnFail(error) {
            toastr.error(error.get_message());
        }

        function PerformProcess() {
            var debillId = '', sabillId = '';
            var deletedBillList = new Array(), savedBillList = new Array();
            $("#tblRstBillSearch tbody tr").each(function () {
                if ($(this).find("td:eq(0)").find("input").is(":checked") == false) {
                    debillId = $(this).find("td:eq(0)").find("input").val();
                    deletedBillList.push(debillId);
                }
                else {
                    sabillId = $(this).find("td:eq(0)").find("input").val();
                    savedBillList.push(sabillId);
                }
            });
            PageMethods.UpdateBill(deletedBillList, savedBillList, OnProcessSucceeded, OnProcessFailed);
            return false;
        }

        function OnProcessSucceeded(result) {
            CommonHelper.AlertMessage(result.AlertMessage);
        }
        function OnProcessFailed(error) {
            toastr.error(error);
        }        
    </script>
    <div id="EntryPanel" class="panel panel-default">
        <div class="panel-heading">
            Vat Adjustment</div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblFromDate" runat="server" class="control-label" Text="Search Date"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchDate" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblCostCenter" runat="server" class="control-label" Text="Cost Center"></asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlCostCenter" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <button type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm">
                            Search</button>
                        <%--<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="TransactionalButton btn btn-primary"
                OnClientClick="javascript: return PerformClearAction();" TabIndex="7" />--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-heading">
            Search Information</div>
        <div class="panel-body">
            <%--<asp:GridView ID="gvRestaurantSavedBill" Width="100%" runat="server" AllowPaging="True"
                    AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                    ForeColor="#333333" PageSize="100" TabIndex="9" 
                onpageindexchanging="gvRestaurantSavedBill_PageIndexChanging" 
                onrowcommand="gvRestaurantSavedBill_RowCommand">
                    <RowStyle BackColor="#E3EAEB" />
                    <Columns>
                        <asp:TemplateField HeaderText="IDNO" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblid" runat="server" Text='<%#Eval("BillId") %>'></asp:Label></ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="BillNumber" HeaderText="Bill Number" ItemStyle-Width="25%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>                        
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-Width="25%">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>                        
                        <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                            <ItemTemplate>
                                &nbsp;<asp:ImageButton ID="ImgBillPreview" runat="server" CausesValidation="False"
                                    CommandArgument='<%# bind("BillId") %>' CommandName="CmdBillPreview" ImageUrl="~/Images/ReportDocument.png"
                                    Text="" AlternateText="Bill Preview" ToolTip="Bill Preview" />
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
                </asp:GridView>--%>
            <table id='tblRstBillSearch' class="table table-bordered table-condensed table-responsive"
                width="100%">
                <colgroup>
                    <col style="width: 5%;" />
                    <col style="width: 35%;" />
                    <col style="width: 40%;" />
                    <col style="width: 20%;" />
                </colgroup>
                <thead>
                    <tr style="color: White; background-color: #44545E; font-weight: bold;">
                        <td>
                        </td>
                        <td>
                            Bill Number
                        </td>
                        <td>
                            Customer Name
                        </td>
                        <td>
                            Amount
                        </td>
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
            <asp:Button ID="btnProcess" runat="server" Text="Process" CssClass="TransactionalButton btn btn-primary btn-sm"
            OnClientClick="javascript: return PerformProcess();" TabIndex="7" />
        </div>        
    </div>
</asp:Content>
