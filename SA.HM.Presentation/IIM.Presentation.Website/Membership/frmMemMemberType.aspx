<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmMemMemberType.aspx.cs" Inherits="HotelManagement.Presentation.Website.Membership.frmMemMemberType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Membership</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Member Type</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);

            if ($("#InnboardMessageHiddenField").val() != "") {
                CommonHelper.AlertMessage(JSON.parse($("#InnboardMessageHiddenField").val()));
                $("#InnboardMessageHiddenField").val("");
            }

            $("#SearchPanel").hide();
            $("#btnSearch").click(function () {
                $("#SearchPanel").show('slow');
                GridPaging(1, 1);
            });

            $("#gvMemberType").delegate("td > img.MemberTypeDelete", "click", function () {
                var answer = confirm("Do you want to delete this record?")
                if (answer) {
                    var memberTypeId = $.trim($(this).parent().parent().find("td:eq(6)").text());
                    var params = JSON.stringify({ sMemberTypeId: memberTypeId });

                    var $row = $(this).parent().parent();
                    //$(this).parent().parent().remove();
                    $.ajax({
                        type: "POST",
                        url: "/Membership/frmMemMemberType.aspx/DeleteData",
                        data: params,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            CommonHelper.AlertMessage(data.d.AlertMessage);
                            $row.remove();
                            $("#myTabs").tabs('load', 1);
                        },
                        error: function (error) {
                        }
                    });
                }
            });

        });

        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=txtTypeName.ClientID %>").val('');
            $("#<%=txtTypeCode.ClientID %>").val('');
            $("#<%=txtSubscriptionFee.ClientID %>").val('');
            $("#<%=txtDiscountPercent.ClientID %>").val('');
            $("#<%=hfMemberTypeId.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }

        $(function () {
            $("#myTabs").tabs();
        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#gvMemberType tbody tr").length;

            var typeName = $("#<%=txtSTypeName.ClientID %>").val();
            var code = $("#<%=txtSTypeCode.ClientID %>").val();

            PageMethods.SearchMemberTypeNLoadGridInformation(typeName, code, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {

            //$("#ltlTableWiseItemInformation").html(result);

            $("#gvMemberType tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvMemberType tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvMemberType tbody tr").length;
                //totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:30%; cursor:pointer;\">" + gridObject.Name + "</td>";
                tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.Code + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.SubscriptionFee + "</td>";
                tr += "<td align='left' style=\"width:15%; cursor:pointer;\">" + gridObject.DiscountPercent + "</td>";
                tr += "<td align='right' style=\"width:8%; cursor:pointer;\"><img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.TypeId + "')\" alt='Edit Information' border='0' /></td>";
                tr += "<td align='right' style=\"width:8%; cursor:pointer;\"><img src='../Images/delete.png' class= 'MemberTypeDelete'  alt='Delete Information' border='0' /></td>";
                tr += "<td align='right' style=\"width:4%; display:none;\">" + gridObject.TypeId + "</td>";

                tr += "</tr>"

                $("#gvMemberType tbody ").append(tr);
                tr = "";
            });

            $("#GridPagingContainer ul").append(result.GridPageLinks.PreviousButton);
            $("#GridPagingContainer ul").append(result.GridPageLinks.Pagination);
            $("#GridPagingContainer ul").append(result.GridPageLinks.NextButton);


            return false;
        }
        function OnLoadObjectFailed(error) {
            toastr.error(error.get_message());
        }

        function PerformEditAction(memberTypeId) {
            PageMethods.LoadDetailInformation(memberTypeId, OnLoadDetailObjectSucceeded, OnLoadDetailObjectFailed);
            return false;
        }
        function OnLoadDetailObjectSucceeded(result) {
            $("#<%=txtTypeName.ClientID %>").val(result.Name);
            $("#<%=txtTypeCode.ClientID %>").val(result.Code);
            $("#<%=txtSubscriptionFee.ClientID %>").val(result.SubscriptionFee);
            $("#<%=txtDiscountPercent.ClientID %>").val(result.DiscountPercent);
            $("#<%=hfMemberTypeId.ClientID %>").val(result.TypeId);
            $("#<%=btnSave.ClientID %>").val("Update");

            //$("#myTabs").tabs('select', 0);
            $("#myTabs").tabs({ active: 0 });

            return false;
        }
        function OnLoadDetailObjectFailed(error) {
            toastr.error(error.get_message());
        }
    </script>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Member Type Info</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Member Type</a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">
                    Member Type</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="hfMemberTypeId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblTypeName" runat="server" class="control-label required-field" Text="Type Name"></asp:Label>                                
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTypeName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblTypeCode" runat="server" class="control-label" Text="Code"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTypeCode" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSubscriptionFee" runat="server" class="control-label" Text="Subscription Fee"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSubscriptionFee" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblDiscountPercent" runat="server" class="control-label" Text="Discount Percent"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDiscountPercent" runat="server" CssClass="form-control" TabIndex="4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" TabIndex="5" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="6" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearAction();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tab-2">
            <div id="SearchEntry" class="panel panel-default">
                <div class="panel-heading">
                    Search Member Type</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblSTypeName" runat="server" class="control-label" Text="Type Name"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSTypeName" runat="server" CssClass="form-control" TabIndex="7"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="lblSTypeCode" runat="server" class="control-label" Text="Code"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtSTypeCode" runat="server" CssClass="form-control" TabIndex="8"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <button type="button" id="btnSearch" class="TransactionalButton btn btn-primary btn-sm">
                                    Search</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="SearchPanel" class="panel panel-default">
                <div class="panel-heading">
                    Search Information</div>
                <div class="panel-body">
                    <table id='gvMemberType' class="table table-bordered table-condensed table-responsive"
                        width="100%">
                        <colgroup>
                            <col style="width: 30%;" />
                            <col style="width: 15%;" />
                            <col style="width: 15%;" />
                            <col style="width: 20%;" />
                            <col style="width: 10%;" />
                            <col style="width: 10%;" />
                        </colgroup>
                        <thead>
                            <tr style="color: White; background-color: #44545E; font-weight: bold;">
                                <td>
                                    Type Name
                                </td>
                                <td>
                                    Code
                                </td>
                                <td>
                                    Subscription Fee
                                </td>
                                <td>
                                    Discount Percent
                                </td>
                                <td style="text-align: right;">
                                    Edit
                                </td>
                                <td style="text-align: right;">
                                    Delete
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
