using HotelManagement.Data.DocumentManagement;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.DocumentManagement;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HotelManagement.Presentation.Website.Common.Upload
{
    /// <summary>
    /// Summary description for CommonUpload
    /// </summary>
    public class CommonUpload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {

                //string str_image = "";

                //var ReqPath = HttpContext.Current.Request.Params["ReqPath"];

                //HttpPostedFile file = context.Request.Files["file"];
                //string fileName = file.FileName;
                //string fileExtension = file.ContentType;

                //if (!string.IsNullOrEmpty(fileName))
                //{
                //    fileExtension = Path.GetExtension(fileName);
                //    str_image = fileName;// "MyPHOTO_" + numFiles.ToString() + fileExtension;
                //    string pathToSave_100 = HttpContext.Current.Server.MapPath("~/"+ ReqPath) + str_image;
                //    file.SaveAs(pathToSave_100);
                //}

                string ReqPath = HttpContext.Current.Request.Params["ReqPath"];
                string OwnerId = HttpContext.Current.Request.Params["OwnerId"];
                string DocumentCategory = HttpContext.Current.Request.Params["DocumentCategory"];

                DocumentsDA docDA = new DocumentsDA();
                DocumentsForDocManagementDA docForDocumentDA = new DocumentsForDocManagementDA();
                UserInformationBO userBO = new UserInformationBO();
                List<DocumentsBO> docList = new List<DocumentsBO>();
                List<DocumentsForDocManagementBO> docForDocumentList = new List<DocumentsForDocManagementBO>();
                HttpPostedFile uploadFile = context.Request.Files[0];

                if (uploadFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                    string extension = Path.GetExtension(uploadFile.FileName);
                    fileName = fileName + "-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss-tt") + extension;

                    DocumentsBO signatureBO = new DocumentsBO();
                    DocumentsForDocManagementBO DocumentBO = new DocumentsForDocManagementBO();
                    string uploadFolder = context.Server.MapPath(context.Request.ApplicationPath + ReqPath);

					Directory.CreateDirectory(uploadFolder);							
                    			
                    uploadFile.SaveAs(Path.Combine(uploadFolder, fileName));

                    if (DocumentCategory == "DocumentsDoc")
                    {
                        DocumentBO.OwnerId = Convert.ToInt64(OwnerId);
                        DocumentBO.Name = fileName;
                        DocumentBO.Path = @ReqPath;
                        DocumentBO.Extention = extension;
                        DocumentBO.DocumentType = "Image";
                        DocumentBO.DocumentCategory = DocumentCategory;
                        DocumentBO.CreatedBy = userBO.UserInfoId;
                        docForDocumentList.Add(DocumentBO);
                        Boolean status = docForDocumentDA.SaveDocumentsInfo(docForDocumentList);
                    }
                    else
                    {
                        signatureBO.OwnerId = Convert.ToInt64(OwnerId);
                        signatureBO.Name = fileName;
                        signatureBO.Path = @ReqPath;
                        signatureBO.Extention = extension;
                        signatureBO.DocumentType = "Image";
                        signatureBO.DocumentCategory = DocumentCategory;
                        signatureBO.CreatedBy = userBO.UserInfoId;
                        docList.Add(signatureBO);
                        Boolean status = docDA.SaveDocumentsInfo(docList);
                    }
                }

                //context.Response.Write("Hello World");
            }
            catch (Exception ac)
            {
                throw ac;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}