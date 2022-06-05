<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyProjectUserControlVertical.ascx.cs" Inherits="HotelManagement.Presentation.Website.HMCommon.UserControl.CompanyProjectUserControlVertical" %>

<script runat="server">
    public string fristDropdownValue = "all";

</script>
<script type="text/javascript">
    //var fristDropdownValue = "all";
    var fristDropdownName = "";
    var CompanyList = new Array();

    $(document).ready(function () {
        //PopulateProjects();

        if ($("#<%=hfCompanyAll.ClientID%>").val() != "") {
            //debugger;
            CompanyList = JSON.parse($("#<%=hfCompanyAll.ClientID%>").val());
        }
        var single = $("#<%=hfIsSingle.ClientID%>").val();
        if (single == "1") {
            $('#CompanyProjectPanel').hide();
        }
        else {
            $('#CompanyProjectPanel').show();
        }

        $('#<%=ddlGLCompany.ClientID%>').select2({
            tags: "true",
            placeholder: "--- Please Select ---",
            allowClear: true,
            width: "99.75%"
        });

        $('#<%=ddlGLProject.ClientID%>').select2({
            tags: "true",
            placeholder: "--- Please Select ---",
            allowClear: true,
            width: "99.75%"
        });

        $('#<%=ddlGLCompany.ClientID%>').change(function () {

            var CompanyId = $(this).val();
            $('#<%=hfGLCompanyId.ClientID%>').val(CompanyId);
            var projectControl = $('#<%=ddlGLProject.ClientID %>');
            //$(control).attr("disabled", "disabled");
            if ($(this).val() == "0") {
                $('#<%=ddlGLProject.ClientID %>').empty().append('<option selected="selected" value="0">--- Please Select ---</option>');
            }
            else {
                $('#<%=ddlGLProject.ClientID %>').empty().append('<option selected="selected" value="0">Loading...</option>');
                $.ajax({
                    type: "POST",
                    url: "/Common/WebMethodPage.aspx/PopulateProjectsWithUser",
                    data: JSON.stringify({ companyId: CompanyId }), //'{companyId: ' + CompanyId + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        // debugger;
                        OnProjectsPopulated(data, projectControl);
                    },
                    failure: function (response) {
                        //debugger;
                        alert(response.d);
                    }
                });
            }
        });

        $('#<%=ddlGLProject.ClientID%>').change(function () {

            var companyid = $('#<%=ddlGLCompany.ClientID%>').val();
            var projectid = $('#<%=ddlGLProject.ClientID%>').val();

            $('#<%=hfGLCompanyId.ClientID%>').val(companyid);
            $('#<%=hfGLProjectId.ClientID%>').val(projectid);
        });
    });

    var OnProjectsPopulated = (response, control) => {

        //var i =  fristDropdownValue;
        if ($('#<%=hfDropdownFirstValue.ClientID%>').val() == "all") {
            fristDropdownName = "--- ALL ---";
        }
        else {
            fristDropdownName = "--- Please Select ---";
        }

        $('#<%=hfGLProjectId.ClientID%>').val("0");
        $(control).attr("disabled", false);
        PopulateControl(response.d, $(control), fristDropdownName);
    }

</script>
<asp:HiddenField ID="hfCompanyAll" runat="server" />
<asp:HiddenField ID="hfIsSingle" runat="server" Value="0"></asp:HiddenField>
<asp:HiddenField ID="hfGLProjectId" runat="server" Value="0"></asp:HiddenField>
<asp:HiddenField ID="hfGLCompanyId" runat="server" Value="0"></asp:HiddenField>
<asp:HiddenField ID="hfDropdownFirstValue" runat="server" Value=""></asp:HiddenField>

<div class="form-horizontal">
    <div id="CompanyProjectPanel">
        <div class="form-group">
            <div class="col-md-4">
                <asp:Label ID="lblGLCompany" runat="server" class="control-label required-field" Text="Company"></asp:Label>
            </div>
            <div class="col-md-8">
                <asp:DropDownList ID="ddlGLCompany" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-4">
                <asp:Label ID="lblGLProject" runat="server" class="control-label required-field" Text="Project"></asp:Label>
            </div>
            <div class="col-md-8">
                <asp:DropDownList ID="ddlGLProject" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
        </div>
    </div>
</div>
