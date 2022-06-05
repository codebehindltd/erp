using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace HotelManagement.Presentation.Website
{
    /// <summary>
    /// Summary description for FileUploader
    /// </summary>
    public class FileUploader : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                string dirFullPath = HttpContext.Current.Server.MapPath("~/MediaUploader/");
                string[] files;
                int numFiles;
                files = Directory.GetFiles(dirFullPath);
                numFiles = files.Length;
                numFiles = numFiles + 1;
                string str_image = "";

                var name = HttpContext.Current.Request.Params["username"];

                foreach (string s in context.Request.Files)
                {
                    HttpPostedFile file = context.Request.Files[s];
                    string fileName = file.FileName + name;
                    string fileExtension = file.ContentType;

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        fileExtension = Path.GetExtension(fileName);
                        str_image = fileName;// "MyPHOTO_" + numFiles.ToString() + fileExtension;
                        string pathToSave_100 = HttpContext.Current.Server.MapPath("~/MediaUploader/") + str_image;
                        file.SaveAs(pathToSave_100);
                    }
                }
                //  database record update logic here  ()

                context.Response.Write(str_image);
            }
            catch (Exception ac)
            {
                throw ac;
            }
        }
        //public void ProcessRequest(HttpContext context)
        //{
        //    try
        //    {
        //        if (context.Request.Files.Count > 0)
        //        {
        //            HttpFileCollection UploadedFilesCollection = context.Request.Files;
        //            for (int i = 0; i < UploadedFilesCollection.Count; i++)
        //            {
        //                System.Threading.Thread.Sleep(2000);
        //                HttpPostedFile PostedFiles = UploadedFilesCollection[i];
        //                string FilePath = context.Server.MapPath("~/MediaUploader/" + Path.GetFileName(PostedFiles.FileName));
        //                PostedFiles.SaveAs(FilePath);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
            
        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}