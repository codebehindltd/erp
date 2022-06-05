<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmGuestServiceInfo.aspx.cs" Inherits="HotelManagement.Presentation.Website.HotelManagement.frmGuestServiceInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Front Desk Management</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Service Information</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            CommonHelper.ApplyDecimalValidation();
            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#ContentPlaceHolder1_ddlServiceId").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%",
                tags: false
            });

            $("#ContentPlaceHolder1_ddlIncomeAccountHead").select2({
                tags: "true",
                placeholder: "--- All ---",
                allowClear: true,
                width: "99.75%",
                tags: false
            });

            $("#SearchPanel").hide();
            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });

            $("#gvPaidServiceInformation").delegate("td > img.PaidServiceDelete", "click", function () {
                var answer = confirm("Do you want to delete this record?")
                if (answer) {
                    var itemId = $.trim($(this).parent().parent().find("td:eq(4)").text());
                    var params = JSON.stringify({ sEmpId: itemId });

                    var $row = $(this).parent().parent();
                    //$(this).parent().parent().remove();
                    $.ajax({
                        type: "POST",
                        url: "/HotelManagement/frmGuestServiceInfo.aspx/DeletePaidServiceById",
                        data: params,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $row.remove();
                            CommonHelper.AlertMessage(data.d.AlertMessage);
                            $("#myTabs").tabs('load', 1);
                        },
                        error: function (error) {
                        }
                    });
                }
            });
        });

        //MessageDiv Visible True/False-------------------
        function MessagePanelShow() {
            $('#MessageBox').show("slow");
        }
        function MessagePanelHide() {
            $('#MessageBox').hide("slow");
        }
        $(function () {
            $("#myTabs").tabs();
        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#gvPaidServiceInformation tbody tr").length;

            var serviceName = $("#<%=txtSServiceName.ClientID %>").val();
            var serviceType = $("#<%=ddlSServiceType.ClientID %>").val();
            var activeStat = $("#<%=ddlSActiveStat.ClientID %>").val();

            PageMethods.SearchPaidServiceAndLoadGridInformation(serviceName, serviceType, activeStat, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {

            //$("#ltlTableWiseItemInformation").html(result);

            $("#gvPaidServiceInformation tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvPaidServiceInformation tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            var isUpdatepermission = false, isDeletePermission = false;

            if ($("#ContentPlaceHolder1_hfIsUpdatePermission").val() == "1")
                isUpdatepermission = true;
            if ($("#ContentPlaceHolder1_hfIsDeletePermission").val() == "1")
                isDeletePermission = true;

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvPaidServiceInformation tbody tr").length;
                //totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:25%; cursor:pointer;\">" + gridObject.ServiceName + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.ServiceType + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.ActiveStatus + "</td>";
                tr += "<td align='center' style=\"width:5%; cursor:pointer;\">";
                if (isUpdatepermission)
                    tr += "<img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.ServiceId + "')\" alt='Edit Information' border='0' />&nbsp;&nbsp;&nbsp;";
                if (isDeletePermission)
                    tr += "<img src='../Images/delete.png' class= 'PaidServiceDelete'  alt='Delete Information' border='0' />&nbsp;&nbsp;";
                tr += "</td>";

                tr += "<td align='right' style=\"width:5%; display:none;\">" + gridObject.ServiceId + "</td>";

                tr += "</tr>"

                $("#gvPaidServiceInformation tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);


            return false;
        }
        function OnLoadObjectFailed(error) {
            alert(error.get_message());
        }
        function PerformEditAction(itemId) {
            //alert('edit');
            var possiblePath = "frmGuestServiceInfo.aspx?editId=" + itemId;
            window.location = possiblePath;
        }
    </script>
    <asp:HiddenField ID="hfIsSavePermission" runat="server" />
    <asp:HiddenField ID="hfIsUpdatePermission" runat="server" />
    <asp:HiddenField ID="hfIsDeletePermission" runat="server" />
    <asp:HiddenField ID="hfIsFrontOfficeIntegrateWithAccounts" runat="server"></asp:HiddenField>
    <div id="MessageBox" class="alert alert-info" style="display: none">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Service Entry</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Service</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">Service Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:HiddenField ID="txtPaidServiceId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblServiceName" runat="server" class="control-label required-field" Text="Service Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtServiceName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblDescription" runat="server" class="control-label" Text="Description"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine"
                                    TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" runat="server" id="IncomeAccountHeadDiv">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblIncomeAccountHead" runat="server" class="control-label required-field" Text="Accounts Head"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlIncomeAccountHead" runat="server" CssClass="form-control"
                                    TabIndex="3">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblServiceType" runat="server" class="control-label required-field" Text="Service Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="form-control"
                                    TabIndex="3">
                                    <asp:ListItem Value="PS">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="Daily">Daily</asp:ListItem>
                                    <%--<asp:ListItem Value="PerStay">Per Stay</asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblCostCentreId" runat="server" class="control-label required-field" Text="Cost Center"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCostCentre" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSellingPriceLocal" runat="server" class="control-label" Text="Selling Price One"></asp:Label>
                                <asp:DropDownList ID="ddlSellingPriceLocal" runat="server" CssClass="customSmallDropDownSize"
                                    TabIndex="7" Visible="False">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSellingPriceLocal" runat="server" CssClass="form-control quantitydecimal" TabIndex="8"></asp:TextBox>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSellingPriceUsd" runat="server" class="control-label" Text="Selling Price Two"></asp:Label>
                                <asp:DropDownList ID="ddlSellingPriceUsd" runat="server" CssClass="customSmallDropDownSize"
                                    TabIndex="10" Visible="False">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSellingPriceUsd" runat="server" CssClass="form-control quantitydecimal" TabIndex="11"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblIsVatEnable" runat="server" class="control-label required-field" Text="Vat Enable"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIsVatEnable" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="2">No</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblIsGeneralService" runat="server" class="control-label required-field" Text="General Service"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIsGeneralService" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="2">No</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblIsServiceChargeEnable" runat="server" class="control-label required-field" Text="Svc. Charge Enable"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIsServiceChargeEnable" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="2">No</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblIsPaidService" runat="server" class="control-label required-field" Text="Paid Service"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIsPaidService" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="2">No</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSDCharge" runat="server" class="control-label required-field" Text="SD Charge Enable"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIsSDChargeEnable" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="2">No</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblIsNextDayAchievement" runat="server" class="control-label required-field" Text="Is Next Day Achieve"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIsNextDayAchievement" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="1">No</asp:ListItem>
                                    <asp:ListItem Value="2">Yes</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblAdditionalCharge" runat="server" class="control-label required-field" Text="Additional Charge"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlIsAdditionalChargeEnable" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                    <asp:ListItem Value="0">--- Please Select ---</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="2">No</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblActiveStat" runat="server" class="control-label required-field" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="form-control"
                                    TabIndex="5">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" TabIndex="6" Text="Save" CssClass="btn btn-primary btn-sm"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="7" Text="Clear" CssClass="btn btn-primary btn-sm"
                                    OnClick="btnClear_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">Search Service</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSServiceName" runat="server" class="control-label" Text="Service Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtSServiceName" runat="server" CssClass="form-control" TabIndex="8"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSServiceType" runat="server" class="control-label" Text="Service Type"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSServiceType" runat="server" CssClass="form-control"
                                    TabIndex="9">
                                    <asp:ListItem Value="0">--- All ---</asp:ListItem>
                                    <asp:ListItem Value="Daily">Daily</asp:ListItem>
                                    <asp:ListItem Value="PerStay">Per Stay</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 label-align">
                                <asp:Label ID="lblSActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlSActiveStat" runat="server" CssClass="form-control"
                                    TabIndex="10">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <button type="button" id="btnSearch" class="btn btn-primary btn-sm">
                                    Search</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">Search Information</div>
                <div class="panel-body">
                    <table class="table table-bordered table-condensed table-responsive" id='gvPaidServiceInformation' width="100%">
                        <colgroup>
                            <col style="width: 35%;" />
                            <col style="width: 20%;" />
                            <col style="width: 20%;" />
                            <col style="width: 10%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>Name
                                </td>
                                <td>Type
                                </td>
                                <td>Status
                                </td>
                                <td style="text-align: right;">Option
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
                </div>
            </div>
        </div>
    </div>
</asp:Content>
