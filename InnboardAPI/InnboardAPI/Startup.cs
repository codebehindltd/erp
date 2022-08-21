using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.FileSystems;
using System.IO;
using Microsoft.Owin.StaticFiles;

[assembly: OwinStartup(typeof(InnboardAPI.Startup))]

namespace InnboardAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // file path
            //string root = AppDomain.CurrentDomain.BaseDirectory;
            //var physicalFileSystem = new PhysicalFileSystem(Path.Combine(root, "wwwroot"));
            //var options = new FileServerOptions
            //{
            //    RequestPath = PathString.Empty,
            //    EnableDefaultFiles = true,
            //    FileSystem = physicalFileSystem
            //};
            //options.StaticFileOptions.FileSystem = physicalFileSystem;
            //options.StaticFileOptions.ServeUnknownFileTypes = false;
            //app.UseFileServer(options);
        }
    }
}
