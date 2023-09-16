using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;
using Newtonsoft.Json;

namespace InnboardAPI.Models
{
    public class UtilityMethods
    {
        public static string UploadByteFile(string fileName, string path, byte[] byteArray)
        {

            try
            {
                string uniqFileName = null;
                if (byteArray != null && byteArray.Length > 0)
                {
                    //string root = AppDomain.CurrentDomain.BaseDirectory;
                    var rootPath = HttpContext.Current.Server.MapPath("~/Images");
                    //var physicalFileSystem = new PhysicalFileSystem(Path.Combine(root, "wwwroot"));
                    var folderPath = Path.Combine(rootPath, path);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    uniqFileName = Guid.NewGuid().ToString() + "_" + fileName;
                    var fileLocation = Path.Combine(folderPath, uniqFileName);
                    using (var fs = new FileStream(fileLocation, FileMode.Create, FileAccess.Write))
                    {

                        fs.Write(byteArray, 0, byteArray.Length);

                        //fs.CopyTo(fileName);

                    }
                    //uniqFileName = Path.Combine(path, uniqFileName);
                }

                return uniqFileName;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
