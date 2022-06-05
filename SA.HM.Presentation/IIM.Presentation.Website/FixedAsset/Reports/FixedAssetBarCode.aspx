<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true" CodeBehind="FixedAssetBarCode.aspx.cs" Inherits="HotelManagement.Presentation.Website.FixedAsset.Reports.FixedAssetBarCode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            $("#ContentPlaceHolder1_ddlCategory").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlProduct").select2({
                tags: "true",
                placeholder: "Select an option",
                allowClear: true,
                width: "99.75%"
            });
            $("#ContentPlaceHolder1_ddlCategory").change(function () {
                var categoryId = $(this).val();
                LoadProductByCategoryId(categoryId);
            });
        });
        function GenarateBarCode() {
            var itemId = $("#ContentPlaceHolder1_ddlProduct").val();
            var categoryId = $("#ContentPlaceHolder1_ddlCategory").val();
            var locationId = $("#ContentPlaceHolder1_ddlLocation").val();
            if (itemId == null) {
                itemId = 0;
            }
            if (categoryId == null) {
                categoryId = 0;
            }
            if (locationId == null) {
                locationId = 0;
            }
            PrintBarCodeById(itemId, categoryId, locationId)
            return false;
        }
        function PrintBarCodeById(itemId, categoryId, locationId) {
            PageMethods.PrintBarCodeById(itemId, categoryId, locationId, OnBarCodeSucceed, OnBarCodeFailed);
            return false;
        }

        function OnBarCodeSucceed(result) {
            //toastr.info(result);

            var baseUrl = "http://" + window.location.host + "/";
            var url = baseUrl + result;

            //$("#DetailsRequisitionGridContaiiner").dialog({
            //    autoOpen: true,
            //    modal: true,
            //    minWidth: 700,
            //    maxWidth: 800,
            //    closeOnEscape: true,
            //    resizable: false,
            //    height: 'auto',
            //    fluid: true,
            //    title: "Bar Code",
            //    show: 'slide'
            //});

            //var myframe = document.getElementById("ifrmReportViewer");
            //if (myframe !== null) {
            //    if (myframe.src) {
            //        myframe.src = url;
            //    }
            //    else if (myframe.contentWindow !== null && myframe.contentWindow.location !== null) {
            //        myframe.contentWindow.location = url;
            //    }
            //    else { myframe.setAttribute('src', url); }
            //}
            $('#ifrmReportViewer').attr("src", url + "#view=VFit" + "&toolbar=1" + "&navpanes=1");
        }

        function OnBarCodeFailed() {

        }
        function LoadProductByCategoryId(categoryId) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: './FixedAssetBarCode.aspx/LoadProductByCategoryId',
                data: "{'categoryId':'" + categoryId + "'}",
                dataType: "json",
                success: function (data) {

                    SelectedItem = data.d;
                    var list = data.d;
                    var control = $('#ContentPlaceHolder1_ddlProduct');

                    control.empty();
                    if (list != null) {
                        if (list.length > 0) {

                            control.empty().append('<option value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                            for (i = 0; i < list.length; i++) {
                                control.append('<option title="' + list[i].ItemName + '" value="' + list[i].ItemId + '">' + list[i].ItemName + '</option>');
                            }
                        }
                        else {
                            control.empty().append('<option selected="selected" value="0">' + $("#<%=CommonDropDownHiddenField.ClientID %>").val() + '</option>');
                        }
                    }

                    control.val($("#ContentPlaceHolder1_hfItemId").val());
                },
                error: function (result) {
                }
            });
        }
    </script>
    <asp:HiddenField ID="hfItemId" runat="server" Value="0"></asp:HiddenField>
    <div id="SearchPanel" class="panel panel-default">
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" class="control-label" Text="Report Type"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="ItemWiseStock" Text="Item Wise Stock"></asp:ListItem>
                            <asp:ListItem Value="CategoryWiseStock" Text="Category Wise Stock"></asp:ListItem>
                            <asp:ListItem Value="LocationWiseStock" Text="Location Wise Stock"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div id="LocationWise">
                        <asp:HiddenField ID="CommonDropDownHiddenField" runat="server"></asp:HiddenField>
                        <div class="col-md-2">
                            <asp:Label ID="lblLocation" runat="server" class="control-label" Text="Location"></asp:Label>
                        </div>
                        <div class="col-md-10">
                            <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="lblCategory" runat="server" class="control-label" Text="Category"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlCategory" CssClass="form-control" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-2">
                        <asp:Label ID="Label8" runat="server" class="control-label required-field" Text="Item"></asp:Label>
                    </div>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnGenarate" runat="server" Text="Generate" CssClass="btn btn-primary btn-sm"
                            OnClientClick="javascript: return GenarateBarCode();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="DetailsRequisitionGridContaiiner">
        <div class="row">
            <div class="col-md-12">
                <iframe id="ifrmReportViewer" name="ifrmReportViewer" frameborder="0" style="width: 100%; height: 800px" scrolling="yes"></iframe>
            </div>
        </div>
    </div>
    
</asp:Content>
