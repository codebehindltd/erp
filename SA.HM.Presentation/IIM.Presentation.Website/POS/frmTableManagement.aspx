<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Innboard.Master" AutoEventWireup="true"
    CodeBehind="frmTableManagement.aspx.cs" Inherits="HotelManagement.Presentation.Website.POS.frmTableManagement" %>

<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc1" %>
<%@ Register Assembly="FlashUpload" Namespace="ClientUploader" TagPrefix="cc2" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            /*var moduleName = "<a href='/HMCommon/frmHMHome.aspx' class='inActive'>Restaurant</a>";
            var formName = "<span class='divider'>/</span><li class='active'>Table Management</li>";
            var breadCrumbs = moduleName + formName;
            $("#ltlBreadCrumbsInformation").html(breadCrumbs);*/

            //$('.draggableClass')



            if ($("#InnboardMessageHiddenField").val() != "") {

                var alertMessage = JSON.parse($("#InnboardMessageHiddenField").val());

                CommonHelper.AlertMessage(alertMessage);
                $("#InnboardMessageHiddenField").val("");

                if (alertMessage.IsSuccess == 1) {
                    document.forms[0].reset();
                }
            }

            $("#ContentPlaceHolder1_ddlImageType").change(function () {
                var ImageType = $("#ContentPlaceHolder1_ddlImageType").val();

                if (ImageType == 'VacantImage') {
                    $('#VacantImageDiv').show();
                    $('#OccupiedImageDiv').hide();
                    $('#ContentPlaceHolder1_hfImageType').val("VacantImage");
                }
                else {
                    $('#VacantImageDiv').hide();
                    $('#OccupiedImageDiv').show();
                    $('#ContentPlaceHolder1_hfImageType').val("OccupiedImage");
                }
                return false;
            });
            $("#ContentPlaceHolder1_ddlImageType").val($("#ContentPlaceHolder1_ddlImageType option:first").val()).trigger('change');
            //debugger;
            if ($("#ContentPlaceHolder1_hfTotalRoomNumber").val() != "0") {
                var TotalRoomNumber = parseInt($("#ContentPlaceHolder1_hfTotalRoomNumber").val());
                for (var i = 0; i < TotalRoomNumber ; i++) {
                    var id = "draggable" + i + ""; //'.' + id + ''

                    const Draggables =
                    Subjx('.' + id + '').drag({
                        each: {
                            move: true,
                            resize: true
                        },
                        snap: {
                            x: 20,
                            y: 20
                        }
                    });

                }

            }

            if ($('#ContentPlaceHolder1_hfddlCostCenterId').val() != '') {
                //debugger;
                var costCenterId = $('#ContentPlaceHolder1_hfddlCostCenterId').val();
                EntryPanelVisibleTrue();
                //$("#ContentPlaceHolder1_ddlCostCenterId").val(costCenterId).trigger('change');
                $('#ContentPlaceHolder1_hfddlCostCenterId').val('');
            }

            



        });

        //const svgs =
        //    Subjx('.draggableClass').drag({
        //        each: {
        //            //move: true,
        //            resize: true,
        //            rotate: true
        //        },
        //        snap: {
        //            x: 20,
        //            y: 20,
        //            angle: 20
        //        }
        //    });



        //Subjx('.clone').clone({
        //    stack: '#draggableClass',
        //    style: 'clone',
        //    drop(e, el, clone) {

        //        const stack = Subjx('#draggableClass')[0],
        //            offset = stack.getBoundingClientRect(),
        //            div = document.createElement('div');

        //        div.style.top = `${e.pageY - offset.top}px`;
        //        div.style.left = `${e.pageX - offset.left}px`;

        //        div.classList.add('draggable');

        //        stack.appendChild(div);

        //        Draggables.push(...Subjx(div).drag());
        //    }
        //});

        //Subjx('.clone-clear').on('click', () => {

        //    Draggables.forEach(item => {
        //        item.disable();
        //        item.el.parentNode.removeChild(item.el);
        //    });

        //    Draggables.splice(0, Draggables.length);
        //});


        function GenerateCoordinates() {
            var fullStringValue = '';
            $("#<%=txtTableWiseRoomAllocationInfo.ClientID %>").val('');
            $('#FloorRoomAllocation').children().each(function () {
                if ($(this).hasClass('draggableClassWithRotate')) {
                    //debugger;
                    var id = this.id;
                    var divX = this.style.left;
                    var divY = this.style.top;
                    var divWidth = this.style.width;
                    var divHeight = this.style.height;


                    var el = document.getElementById(id);
                    var st = window.getComputedStyle(el, null);
                    var divTransition = st.getPropertyValue("-webkit-transform") ||
                             st.getPropertyValue("-moz-transform") ||
                             st.getPropertyValue("-ms-transform") ||
                             st.getPropertyValue("-o-transform") ||
                             st.getPropertyValue("transform") ||
                             "FAIL";

                    if (divTransition == "none") {
                        divTransition = "matrix(1, 0, 0, 1, 0, 0)";
                    }
                    //var strValue = 'id:' + id + ' X:' + divX + ' Y:' + divY + ' W:' + divWidth + ' H:' + divHeight + '|'; matrix(1, 0, 0, 1, 0, 0);
                    var strValue = id + ',' + divX + ',' + divY + ',' + divWidth + ',' + divHeight + ',' + divTransition + '|';

                    fullStringValue = $("#<%=txtTableWiseRoomAllocationInfo.ClientID %>").val() + strValue;
                    $("#<%=txtTableWiseRoomAllocationInfo.ClientID %>").val(fullStringValue);
                }
            });
            return true;
        }
            //MessageDiv Visible True/False-------------------
            function MessagePanelShow() {
                $('#MessageBox').show("slow");
            }
            function MessagePanelHide() {
                $('#MessageBox').hide("slow");
            }

            //AddNewButton Visible True/False-------------------
            function NewAddButtonPanelShow() {
                $('#btnNewBank').show("slow");
            }
            function NewAddButtonPanelHide() {
                $('#btnNewBank').hide("slow");
                EntryPanelVisibleTrue();
            }

            //Div Visible True/False-------------------
            function EntryPanelVisibleTrue() {
                $('#btnNewBank').hide("slow");
                $('#EntryPanel').show("slow");
                return false;
            }
            function EntryPanelVisibleFalse() {
                $('#btnNewBank').show("slow");
                $('#EntryPanel').hide("slow");
                PerformClearAction();
                return false;
            } 
            function CheckChange(rowId) {
                var Row = $(rowId).parent().parent();

                if ($(rowId).is(":checked"))
                {
                    $(Row).find("td:eq(2)").find("input").prop("disabled", true);
                }
                else
                {
                    $(Row).find("td:eq(2)").find("input").prop("disabled", true);
                }
                //return false;

            }

            function AttachFile(TableManagementId) {
                //alert(TableManagementId);
                $('#ContentPlaceHolder1_hfTableManagementId').val(TableManagementId);
                
                $("#tabledocuments").dialog({
                    autoOpen: true,
                    modal: true,
                    width: 900,
                    closeOnEscape: true,
                    resizable: false,
                    title: "Table Documents",
                    show: 'slide'
                });
                return false;
            }

            function UploadComplete() {

                var costCenterId = $('#ContentPlaceHolder1_ddlCostCenterId option:selected').val();
                $('#ContentPlaceHolder1_hfddlCostCenterId').val(costCenterId);
                $("#ContentPlaceHolder1_btnUploadComplete").trigger("click");
           
                //var randomId = +$("#ContentPlaceHolder1_RandomProductId").val();
                //var id = +$("#ContentPlaceHolder1_hfTableManagementId").val();

                
                return false;
            }

           
    </script>
    <asp:HiddenField ID="hfId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfTableManagementId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfTotalRoomNumber" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="RandomProductId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="tempId" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hfddlCostCenterId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfImageType" runat="server"></asp:HiddenField>



    <div id="tabledocuments" style="display: none;">

        <div class="form-group" id="tableImageDiv">
            <label class="control-label col-md-2">Image Type</label>
            <div class="col-sm-10">
                <asp:DropDownList ID="ddlImageType" runat="server" CssClass="form-control" TabIndex="1">

                    <asp:ListItem Value="VacantImage">Vacant Image</asp:ListItem>
                    <asp:ListItem Value="OccupiedImage">Occupied Image</asp:ListItem>

                </asp:DropDownList>
            </div>
        </div>

        <div id="VacantImageDiv" style="display: none;">
            <label for="Attachment" class="control-label col-md-2">
                Vacant Image</label>
            <div class="col-md-4">
                <asp:Panel ID="pnlUpload" runat="server" Style="text-align: center;">
                    <cc1:ClientUploader ID="flashUpload1" runat="server" UploadPage="Upload.axd"  OnUploadComplete="UploadComplete()"
                        FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
                </asp:Panel>
            </div>
        </div>
        <div id="OccupiedImageDiv" style="display: none;">
            <label for="Attachment" class="control-label col-md-2">
                Occupied Image</label>
            <div class="col-md-4">
                <asp:Panel ID="Panel1" runat="server" Style="text-align: left;">
                    <cc2:ClientUploader ID="flashUpload2" runat="server" UploadPage="Upload.axd" OnUploadComplete="UploadComplete()"
                        FileTypeDescription="Images" FileTypes="" UploadFileSizeLimit="0" TotalUploadSizeLimit="0" />
                </asp:Panel>
            </div>
        </div>
    </div>

    <div id="MessageBox" class="alert alert-info" style="display: none;">
        <button type="button" class="close" data-dismiss="alert">
            ×</button>
        <asp:Label ID='lblMessage' Font-Bold="True" runat="server"></asp:Label>
    </div>
    <div id="btnNewBank" class="btn-toolbar">
        <button onclick="javascript: return EntryPanelVisibleTrue();" class="TransactionalButton btn btn-primary btn-sm">
            <i class="icon-plus"></i>Add More Table</button>
        <div class="btn-group">
        </div>
    </div>
    <div id="EntryPanel" class="panel panel-default" style="display: none;">
        <div class="panel-heading">
            Table Management Information
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <%--<div class="col-md-2">                       
                        <asp:Label ID="lblCostCenterId" runat="server" class="control-label" Text="Cost Center"></asp:Label>
                    </div>--%>
                    <label for="CostCenter" class="control-label col-md-2">
                        Cost Center</label>
                    <div class="col-md-4">
                        <%--Right Left--%>
                        <asp:DropDownList ID="ddlCostCenterId" CssClass="form-control" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlCostCenterIdId_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <div>
                    <asp:GridView ID="gvTableManagement" Width="100%" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" AllowSorting="True"
                        ForeColor="#333333" PageSize="50" OnPageIndexChanging="gvTableManagement_PageIndexChanging"
                        OnRowCommand="gvTableManagement_RowCommand" OnRowDataBound="gvTableManagement_RowDataBound" CssClass="table table-bordered table-condensed table-responsive">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="IDNO" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblTableManagementId" runat="server" Text='<%#Eval("TableManagementId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Added Status" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkIsActiveStatus" runat="server"   onclick="return CheckChange(this)" />
                                    <asp:Label ID="lblchkIsActiveStatus" runat="server" Text='<%#Eval("ActiveStatus") %>' 
                                        Visible="False"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Table Number" ItemStyle-Width="30%">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvTableId" runat="server" Text='<%# Bind("TableId") %>' Visible="False"></asp:Label>
                                    <asp:Label ID="lblgvTableNumber" runat="server" Text='<%# Bind("TableNumber") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            

                            <asp:TemplateField HeaderText="Images Upload" ItemStyle-Width="20%">
                                
                                <ItemTemplate>
                                    <%--<input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Upload" onclick='<%#String.Format("return AttachFile({0})", Eval("TableId")) %>'" />
                                    --%>
                                    <asp:Button ID="btnAttachment" runat="server" Text="Upload" CssClass="TransactionalButton btn btn-primary btn-sm"
                                        OnClientClick='<%# String.Format("return AttachFile({0})", Eval("TableId")) %>' />

                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>


                            <%--<asp:BoundField  HeaderText="Vacant Images" DataField="VacantImagePath" ItemStyle-Width="20%"/>

                            <asp:BoundField  HeaderText="Occupied Images" DataField="OccupiedImagePath" ItemStyle-Width="20%" />--%>
                            <asp:TemplateField HeaderText="Vacant Images" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <%--<asp:Label ID="VacantImagePath" runat="server" Text='<%# Bind("VacantImagePath") %>'></asp:Label>--%>
                                      <asp:Image runat="server" ID="VacantImage"  ImageUrl='<%# Eval("VacantImagePath") %>' alt="" Width="40px" Height="40px"></asp:Image>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Occupied Images" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <%--<asp:Label ID="OccupiedImagePath" runat="server" Text='<%# Bind("OccupiedImagePath") %>'></asp:Label>--%>
                                    <asp:Image runat="server" ID="OccupiedImage"  ImageUrl='<%# Eval("OccupiedImagePath") %>' alt="" Width="40px" Height="40px"></asp:Image>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Action" ItemStyle-Width="20%">
                                
                                <ItemTemplate>
                                    <%--<input type="button" id="btnAttachment" class="TransactionalButton btn btn-primary btn-sm" value="Upload" onclick='<%#String.Format("return AttachFile({0})", Eval("TableId")) %>'" />
                                    --%>
                                    <%--<asp:Button ID="btnAttachment" runat="server" Text="Upload" CssClass="btn btn-primary"
                                        OnClientClick='<%# String.Format("return AttachFile({0})", Eval("TableManagementId")) %>' />--%>

                                    <asp:ImageButton ID="ImgDelete" OnClientClick="return confirm('Do you want to delete?');" runat="server" CausesValidation="False" CommandName="CmdDelete"
                                            CommandArgument='<%# bind("TableManagementId") %>' ImageUrl="~/Images/delete.png" Text=""
                                            AlternateText="Delete" ToolTip="Delete" />

                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
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
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnUploadComplete" runat="server" Text="Save" CssClass="btn btn-primary"
                             OnClick="btnUploadComplete_Click" style="display:none"/>
                        <asp:Button ID="btnSaveAll" runat="server" Text="Save" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClick="btnSaveAll_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Close" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClientClick="javascript: return EntryPanelVisibleFalse();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="GraphicalPanel" class="panel panel-default">
        <div class="panel-heading">
            Graphical Table Presentation
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <%--<div class="col-md-2">                                                
                        <asp:Label ID="lblSrcCostCenterId" runat="server" class="control-label" Text="Cost Center"></asp:Label>
                    </div>--%>
                    <asp:HiddenField ID="txtCostCenterWiseTableAllocationInfo" runat="server"></asp:HiddenField>
                    <label for="CostCenter" class="control-label col-md-2">
                        Cost Center</label>
                    <div class="col-md-4">
                        <%--Right Left--%>
                        <asp:DropDownList ID="ddlSrcCostCenterId" CssClass="form-control" runat="server"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlSrcCostCenterId_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:HiddenField ID="txtTableWiseRoomAllocationInfo" runat="server"></asp:HiddenField>
                        <asp:Button ID="btnSave" runat="server" Text="Save Design" CssClass="TransactionalButton btn btn-primary btn-sm"
                            OnClick="btnSave_Click" OnClientClick="javascript:return GenerateCoordinates()" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel panel-body FloorRoomAllocationBGImage" style="height: 100vh; overflow: scroll;">
            <asp:Literal ID="ltlRoomTemplate" runat="server">
            </asp:Literal>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            //$(".draggableClassWithRotate").draggable().resizable();
            $(".droppable, #droppable-inner").droppable({
                drop: function (event, ui) {
                    //alert(ui.draggable.attr('id') + ' was dropped from ' + ui.draggable.parent().attr('id'));
                    return false;
                }
            });
        });

        $(document).ready(function () {
            $(".draggableClassWithRotate").mousedown(function () {
                $(this).css('border-style', 'dashed');
            });
            $(".draggableClassWithRotate").mouseup(function () {
                $(this).parent().find('.draggableClassWithRotate').css('border-style', 'solid');
            });
        });
        var x = '<%=isMessageBoxEnable%>';
        if (x > -1) {
            MessagePanelShow();
            if (x == 2) {
                $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
            }
        }
        else {
            MessagePanelHide();
        }

        var xNewAdd = '<%=isNewAddButtonEnable%>';
        if (xNewAdd > -1) {
            NewAddButtonPanelShow();
        }
        else {
            NewAddButtonPanelHide();
        }
    </script>
</asp:Content>
