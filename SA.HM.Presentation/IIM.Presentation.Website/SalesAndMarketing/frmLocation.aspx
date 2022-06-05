<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmLocation.aspx.cs" Inherits="HotelManagement.Presentation.Website.SalesAndMarketing.frmLocation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        var vv = [];
        //Bread Crumbs Information-------------
        $(document).ready(function () {
            var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Sales & Marketing </a>";
            var formName = "<span class='divider'>/</span><li class='active'>Location Information</li>";
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

            $("#ContentPlaceHolder1_ddlCity").select2({
                tags: "true",
                placeholder: "--- Please Select ---",
                allowClear: true,
                width: "99.75%"
            });

            $("#gvGustIngormation").delegate("td > img.LocationDelete", "click", function () {
                var answer = confirm("Do you want to delete this record?")
                if (answer) {
                    var bankId = $.trim($(this).parent().parent().find("td:eq(5)").text());
                    var params = JSON.stringify({ sEmpId: bankId });

                    var $row = $(this).parent().parent();
                    //$(this).parent().parent().remove();
                    $.ajax({
                        type: "POST",
                        url: "/SalesAndMarketing/frmLocation.aspx/DeleteData",
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
            $("#ContentPlaceHolder1_txtCountry").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/frmLocation.aspx/LoadCountryForAutoSearch',
                        data: JSON.stringify({ searchString: request.term }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.CountryName,
                                    value: m.CountryName,
                                    CountryId: m.CountryId
                                };
                            });
                            response(searchData);
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                },
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_txtCountry").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfCountryId").val(ui.item.CountryId);
                    $("#ContentPlaceHolder1_txtState").val("");
                    $("#ContentPlaceHolder1_hfStateId").val("0");
                    $("#ContentPlaceHolder1_txtCity").val("");
                    $("#ContentPlaceHolder1_hfCityId").val("0");
                    //$("#ContentPlaceHolder1_txtLocation").val("");
                    //$("#ContentPlaceHolder1_hfLocationId").val("0");
                }
            });
            $("#ContentPlaceHolder1_txtState").autocomplete({
                source: function (request, response) {
                    var Country = $("#ContentPlaceHolder1_hfCountryId").val();
                    if (Country == 0) {
                        toastr.warning("Please Select  Country");
                        $("#ContentPlaceHolder1_hfCountryId").focus();
                        return false;
                    }
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/frmLocation.aspx/LoadStateForAutoSearchByCountry',
                        data: JSON.stringify({ searchString: request.term, CountryId: Country }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.StateName,
                                    value: m.StateName,
                                    Id: m.Id
                                };
                            });
                            response(searchData);
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                },
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_txtState").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfStateId").val(ui.item.Id);
                    $("#ContentPlaceHolder1_txtCity").val("");
                    $("#ContentPlaceHolder1_hfCityId").val("0");
                    //$("#ContentPlaceHolder1_txtShippingLocation").val("");
                    //$("#ContentPlaceHolder1_hfShippingLocationId").val("0");
                }
            });
            $("#ContentPlaceHolder1_txtCity").autocomplete({
                source: function (request, response) {
                    var countryId = $("#ContentPlaceHolder1_hfCountryId").val();
                    var stateId = $("#ContentPlaceHolder1_hfStateId").val();
                    var stateString = $("#ContentPlaceHolder1_txtState").val();
                    if (countryId == 0) {
                        toastr.warning("Please Select  Country");
                        $("#ContentPlaceHolder1_hfCountryId").focus();
                        return false;
                    }
                    //debugger;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '../SalesAndMarketing/Company.aspx/LoadCityForAutoSearchByState',
                        data: JSON.stringify({ searchString: request.term, CountryId: countryId, StateString: stateString, StateId: stateId }),
                        dataType: "json",
                        success: function (data) {
                            var searchData = data.error ? [] : $.map(data.d, function (m) {
                                return {
                                    label: m.CityName,
                                    value: m.CityName,
                                    Id: m.CityId
                                };
                            });
                            response(searchData);
                        },
                        error: function (result) {
                            //alert("Error");
                        }
                    });
                },
                focus: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox
                    //$(this).val(ui.item.label);
                },
                select: function (event, ui) {
                    // prevent autocomplete from updating the textbox
                    event.preventDefault();
                    // manually update the textbox and hidden field
                    $(this).val(ui.item.label);
                    $("#ContentPlaceHolder1_txtCity").val(ui.item.value);
                    $("#ContentPlaceHolder1_hfCityId").val(ui.item.Id);
                }
            });
        });




        //For FillForm-------------------------   
        function PerformFillFormAction(actionId) {
            PageMethods.FillForm(actionId, OnFillFormObjectSucceeded, OnFillFormObjectFailed);
            return false;
        }
        function OnFillFormObjectSucceeded(result) {
            $("#<%=txtLocationName.ClientID %>").val(result.LocationName);
            $("#<%=ddlActiveStat.ClientID %>").val(result.ActiveStat == true ? 0 : 1);
            $("#<%=txtLocationId.ClientID %>").val(result.LocationId);
            $("#<%=btnSave.ClientID %>").val("Update");
            $('#btnNewLocation').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }


        function OnFillFormObjectFailed(error) {
            toastr.error(error.get_message());
        }
        //For Delete-------------------------        
        function PerformDeleteAction(actionId, rowIndex) {
            var answer = confirm("Do you want to delete this record?")
            if (answer) {
                PageMethods.DeleteData(actionId, OnDeleteObjectSucceeded, OnDeleteObjectFailed);
            }
            return false;
        }

        function OnDeleteObjectSucceeded(result) {
            window.location = "frmLocation.aspx?DeleteConfirmation=Deleted";
        }
        function OnDeleteObjectFailed(error) {
            toastr.error(error.get_message());
        }
        //For ClearForm-------------------------
        function PerformClearAction() {
            $("#<%=txtLocationName.ClientID %>").val('');
            $("#<%=ddlActiveStat.ClientID %>").val(0);
            $("#<%=txtLocationId.ClientID %>").val('');
            $("#<%=btnSave.ClientID %>").val("Save");
            return false;
        }

        //Div Visible True/False-------------------
        function EntryPanelVisibleTrue() {
            $('#btnNewLocation').hide("slow");
            $('#EntryPanel').show("slow");
            return false;
        }
        function EntryPanelVisibleFalse() {
            $('#btnNewLocation').show("slow");
            $('#EntryPanel').hide("slow");
            PerformClearAction();
            return false;
        }

        //AddNewButton Visible True/False-------------------
        function NewAddButtonPanelShow() {
            $('#btnNewLocation').show("slow");
        }
        function NewAddButtonPanelHide() {
            $('#btnNewLocation').hide("slow");
        }
        $(function () {
            $("#myTabs").tabs();
        });

        function GridPaging(pageNumber, IsCurrentOrPreviousPage) {

            var gridRecordsCount = $("#gvGustIngormation tbody tr").length;

            var bankName = $("#<%=txtSLocationName.ClientID %>").val();
            var activeStat = $("#<%=ddlSActiveStat.ClientID %>").val();
            if (activeStat == 0)
                activeStat = true;
            else
                activeStat = false;

            PageMethods.SearchLocationAndLoadGridInformation(bankName, activeStat, gridRecordsCount, pageNumber, IsCurrentOrPreviousPage, OnLoadObjectSucceeded, OnLoadObjectFailed);
            return false;
        }
        function OnLoadObjectSucceeded(result) {
            $("#gvGustIngormation tbody tr").remove();
            $("#GridPagingContainer ul").html("");

            if (result.GridData == "") {
                var emptyTr = "<tr style=\"background-color:#E3EAEB;\"> <td colspan=\"4\" >No Data Found</td> </tr>";
                $("#gvGustIngormation tbody ").append(emptyTr);
                return false;
            }

            var tr = "", totalRow = 0, editLink = "", deleteLink = "";

            $.each(result.GridData, function (count, gridObject) {

                totalRow = $("#gvGustIngormation tbody tr").length;
                //totalRow = totalRow < 2 ? totalRow : (totalRow - 1);

                if ((totalRow % 2) == 0) {
                    tr += "<tr style=\"background-color:#E3EAEB;\">";
                }
                else {
                    tr += "<tr style=\"background-color:#FFFFFF;\">";
                }

                tr += "<td align='left' style=\"width:25%; cursor:pointer;\">" + gridObject.LocationName + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.CityName + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.State + "</td>";
                tr += "<td align='left' style=\"width:20%; cursor:pointer;\">" + gridObject.Country + "</td>";
                tr += "<td align='right' style=\"width:15%; cursor:pointer;\">";
                tr += "<img src='../Images/edit.png' onClick= \"javascript:return PerformEditAction('" + gridObject.LocationId + "')\" alt='Edit Information' border='0' />";
                tr += "&nbsp;&nbsp;<img src='../Images/delete.png' class= 'LocationDelete'  alt='Delete Information' border='0' /></td>";
                tr += "<td align='right' style=\"width:8%; display:none;\">" + gridObject.LocationId + "</td>";

                tr += "</tr>"

                $("#gvGustIngormation tbody ").append(tr);
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

        function ReloadGrid(IsCurrentOrPreviousPage) {
            var currentPageNumber = $("#GridPagingContainer ul li[class='active']").text();

            if (currentPageNumber == "")
                currentPageNumber = 1;

            GridPaging(currentPageNumber, IsCurrentOrPreviousPage);
        }

        function PerformEditAction(bankId) {
            PageMethods.LoadDetailInformation(bankId, OnLoadDetailObjectSucceeded, OnLoadDetailObjectFailed);
            return false;
        }
        function OnLoadDetailObjectSucceeded(result) {
            if (!confirm("Do you want to edit - " + result.LocationName + "?")) {
                return false;
            }
            $("#<%=txtCity.ClientID %>").val(result.CityName);
            $("#<%=txtCountry.ClientID %>").val(result.Country);
            $("#<%=hfCountryId.ClientID %>").val(result.CountryId);
            $("#<%=txtState.ClientID %>").val(result.State);
            $("#<%=hfStateId.ClientID %>").val(result.StateId);
            $("#<%=txtLocationName.ClientID %>").val(result.LocationName);
            $("#<%=hfCityId.ClientID %>").val(result.CityId);
            $("#<%=ddlActiveStat.ClientID %>").val(result.ActiveStat == true ? 0 : 1);
            $("#<%=txtLocationId.ClientID %>").val(result.LocationId);
            $("#<%=btnSave.ClientID %>").val("Update");

            //$("#myTabs").tabs('select', 0);
            $("#myTabs").tabs({ active: 0 });

            return false;
        }
        function OnLoadDetailObjectFailed(error) {
            toastr.error(error.get_message());
        }
        function ValidateSave() {
            var cityName = $("#<%=txtLocationName.ClientID %>").val();
            var countryId = $("#<%=hfCountryId.ClientID %>").val();
            if (cityName == '') {
                toastr.warning("Enter Location Name");
                $("#<%=txtLocationName.ClientID %>").focus();
                return false;
            }
            if (countryId == '0') {
                toastr.warning("Select a country");
                $("#<%=txtCountry.ClientID %>").focus();
                return false;
            }
        }
    </script>
    <asp:HiddenField ID="hfCountryId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfStateId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfCityId" runat="server" Value="0"></asp:HiddenField>
    <div id="myTabs">
        <ul id="tabPage" class="ui-style">
            <li id="A" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-1">Location Information</a></li>
            <li id="B" runat="server" style="border: 1px solid #AAAAAA; border-bottom: none"><a
                href="#tab-2">Search Location </a></li>
        </ul>
        <div id="tab-1">
            <div id="EntryPanel" class="panel panel-default">
                <div class="panel-heading">Location Information</div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblCountry" runat="server" class="control-label required-field" Text="Country"></asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtCountry" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblState" runat="server" class="control-label" Text="State/ Province/ District"></asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtState" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblNlContactNumber" runat="server" class="control-label" Text="City"></asp:Label>
                            </div>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" TabIndex="1">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:HiddenField ID="txtFocusTabControl" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="txtLocationId" runat="server"></asp:HiddenField>
                                <asp:Label ID="lblLocationName" runat="server" class="control-label required-field" Text="Location Name"></asp:Label>
                            </div>
                            <div class="col-md-10">
                                <asp:TextBox ID="txtLocationName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <asp:Label ID="lblActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActiveStat" runat="server" CssClass="form-control"
                                    TabIndex="2">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" TabIndex="3" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" TabIndex="4" Text="Clear" CssClass="TransactionalButton btn btn-primary btn-sm"
                                    OnClientClick="javascript: return PerformClearAction();" />
                            </div>
                        </div>
                    </div>
            </div>
        </div>
    </div>
    <div id="tab-2">
        <div id="SearchEntry" class="panel panel-default">
            <div class="panel-heading">Location Information</div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblSLocationName" runat="server" class="control-label" Text="Location Name"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtSLocationName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2">
                            <asp:Label ID="lblSActiveStat" runat="server" class="control-label" Text="Status"></asp:Label>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="ddlSActiveStat" runat="server" CssClass="form-control"
                                TabIndex="2">
                                <asp:ListItem Value="0">Active</asp:ListItem>
                                <asp:ListItem Value="1">Inactive</asp:ListItem>
                            </asp:DropDownList>
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
            <div class="panel-heading">Search Information</div>
            <%--<div class="block-body collapse in">
                    <asp:GridView ID="gvGuestHouseService" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" PageSize="30" AllowSorting="True"
                        ForeColor="#333333" OnPageIndexChanging="gvGuestHouseService_PageIndexChanging"
                        OnRowCommand="gvGuestHouseService_RowCommand" OnRowDataBound="gvGuestHouseService_RowDataBound">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" Text='<%#Eval("LocationId") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="LocationName" HeaderText="Location Name" ItemStyle-Width="50%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActiveStatus" HeaderText="Status" ItemStyle-Width="35%">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgUpdate" runat="server" CausesValidation="False" CommandName="CmdEdit"
                                        CommandArgument='<%# bind("LocationId") %>' ImageUrl="~/Images/edit.png" Text=""
                                        AlternateText="Edit" ToolTip="Edit" />
                                    &nbsp;<asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                        CommandArgument='<%# bind("LocationId") %>' ImageUrl="~/Images/delete.png" Text=""
                                        AlternateText="Delete" ToolTip="Delete" OnClientClick="return confirm('Do you want to Delete?');" />
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
                    </asp:GridView>
                    <div class="divClear">
                    </div>
                    <div class="childDivSection">
                        <div class="pagination pagination-centered" id="GridPagingContainer">
                            <ul>
                            </ul>
                        </div>
                    </div>
                </div>--%>
            <div class="panel-body">
                <table id='gvGustIngormation' class="table table-bordered table-condensed table-responsive" width="100%">
                    <colgroup>
                        <col style="width: 25%;" />
                        <col style="width: 20%;" />
                        <col style="width: 20%;" />
                        <col style="width: 20%;" />
                        <col style="width: 15%;" />
                    </colgroup>
                    <thead>
                        <tr style="color: White; background-color: #44545E; font-weight: bold;">
                            <td>Location
                            </td>
                            <td>City
                            </td>
                            <td>State/ Province/ District
                            </td>
                            <td >Country
                            </td>
                            <td >Action
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
    <script type="text/javascript">
        var xNewAdd = '<%=isNewAddButtonEnable%>';

        if (xNewAdd > -1) {

            NewAddButtonPanelShow();
            if (parseInt(xNewAdd) == 2) {
                $('#btnNewLocation').hide();
                $('#EntryPanel').show();
            }
        }
        else {
            NewAddButtonPanelHide();
        }

    </script>
</asp:Content>
