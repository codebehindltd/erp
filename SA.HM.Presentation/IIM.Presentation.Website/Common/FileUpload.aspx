<%@ Page Language="C#" MasterPageFile="~/Common/InnboardEmptyDesign.Master" AutoEventWireup="true" CodeBehind="FileUpload.aspx.cs" Inherits="HotelManagement.Presentation.Website.Common.FileUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var ReqPath = "";
        var OwnerId = "";
        var DocumentCategory = "";

        $(document).ready(function () {

            ReqPath = $.trim(CommonHelper.GetParameterByName("Path"));
            OwnerId = $.trim(CommonHelper.GetParameterByName("OwnerId"));
            DocumentCategory = $.trim(CommonHelper.GetParameterByName("Category"));



            $('input[type=file]').change(function () {
                $('#btnUpload').show();
                $('#divFiles').hide();
                $('#divFiles').html('');
                $('#UploadStatus').hide();
            });

        });

        function uploadFiles() {
           // debugger;
            var file = document.getElementById("fileUploader")//All files
            //debugger;
            for (var i = 0; i < file.files.length; i++) {
                //uploadSingleFile(file.files[i], i);
                UploadFile(file.files[i], i);
            }
        }

        function UploadNClose() {
            //debugger;
            isClose = true;
            //SaveOrUpdateTask();
            $.when(uploadFiles()).done(function () {

                $('#UploadStatus').show();
                if (typeof parent.UploadComplete === "function") {
                    parent.UploadComplete();
                }

                //if (typeof parent.CloseDialog === "function") {
                //    parent.CloseDialog();
                //}

            });
            return false;
        }

        function UploadFile(file, i) {
            //debugger;
            var formData = new FormData();
            //formData.append('file', $('#NIDimage')[0].files[i]);

            //formData.append('username', 'ChrisBoss' + i);
            //formData.append('file', $('#NIDimage')[0].files[i]);

            formData.append('ReqPath', ReqPath);
            formData.append('OwnerId', OwnerId);
            formData.append('DocumentCategory', DocumentCategory);

            formData.append("file", file);

            
            $.ajax({
                type: 'post',
                url: '/Common/Upload/CommonUpload.ashx',
                data: formData,
                success: function (status) {
                    if (status != 'error') {
                        //NIdImgPath = "MediaUploader/" + status;
                        ////$("#myUploadedImg").attr("src", my_path);
                        //progressbarLabel.text('Uploaded Successfully');
                        //progressbarDiv.fadeOut(4000);
                    }
                },
                async: false,
                processData: false,
                contentType: false,
                error: function (ex) {
                    toastr.warning("Upload went wrong! " + ex);
                }
            });
        }

    </script>
    <form id="upload_form" enctype="multipart/form-data" method="post">
        <div style="padding: 10px 30px 10px 30px">
            <div class="container">
                <div class="row">
                    <div class="form-group">
                        <div class="col-md-8">
                            <input type="file" name="fileUploader" id="fileUploader" class="btn btn-default" multiple="multiple" />
                        </div>
                        <div class="col-md-4" style="text-align: right;">
                            <input type="button" id="btnUpload" style="display: none" class="btn btn-default" value="Upload File" onclick="UploadNClose()" />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div id="divFiles" class="files">
                    </div>

                </div>
                <div class="form-group" id="UploadStatus" style="display: none">
                    <div class="col-md-2">
                        <asp:Label runat="server" class="control-label" Text="Upload Complete"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </form>

</asp:Content>
