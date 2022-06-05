using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Newtonsoft.Json;
using Microsoft.SqlServer.Management;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using Microsoft.SqlServer.Management.Smo.Broker;
using Microsoft.SqlServer.Management.Smo.Mail;
using Microsoft.SqlServer.Management.Smo.RegisteredServers;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Common;
using System.IO;
using HotelManagement.Data;

namespace HotelManagement.Presentation.Website.HMCommon
{
    public partial class frmDatabaseBackup : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
        }

        protected void btnDatabaseBackup_Click(object sender, EventArgs e)
        {
            try
            {
                string backupPath = string.Empty, userName = string.Empty, password = string.Empty, serverName = string.Empty, databaseName = string.Empty;
                string backFileName = string.Empty;

                backFileName = "innboard" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".bak";
                backupPath = Server.MapPath(@"~/DatabaseBackup/");
                backupPath += backFileName;

                //userName = "sa";
                //password = "123";
                //serverName = "MAMUN\\SQLEXPRESS";
                //databaseName = "Innboard";

                string encryptedConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InnboardConnectionString"].ConnectionString;
                string decryptedConnectionString = Cryptography.Decrypt(encryptedConnectionString);


                string[] wordConnectionString = decryptedConnectionString.Split(';');

                string mInitialCatalog = wordConnectionString[0];
                string mDataSource = wordConnectionString[1];
                string mUserId = wordConnectionString[2];
                string mPassword = wordConnectionString[3];


                string mInitialCatalogValue = mInitialCatalog.Split('=')[1];
                string mDataSourceValue = mDataSource.Split('=')[1];
                string mUserIdValue = mUserId.Split('=')[1];
                string mPasswordValue = mPassword.Split('=')[1];


                userName = mUserIdValue;
                password = mPasswordValue;
                serverName = mDataSourceValue;
                databaseName = mInitialCatalogValue;

                BackupDatabase(databaseName, userName, password, serverName, backupPath);
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, ex.InnerException.ToString(), "error");
            }

            //backFileName = "innboard" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".bak";
            //backupPath = Server.MapPath(@"~/DatabaseBackup/");

            //backupPath += backFileName;
            //File.Create(backupPath, null, null, System.Security.AccessControl.FileSecurity.) ;

            //string path = Server.MapPath("/");
            //string path1 = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/DatabaseBackup/" + "pp.txt");
            //string path2 = Server.MapPath("~");
            //string path3 = HttpRuntime.AppDomainAppVirtualPath;
            //string path4 = AppDomain.CurrentDomain.BaseDirectory;
            //if (!File.Exists(backupPath))
            //    return;
        }

        public void BackupDatabase(String databaseName, String userName, String password, String serverName, String backupPath)
        {
            try
            {
                Backup sqlBackup = new Backup();

                sqlBackup.Action = BackupActionType.Database;
                sqlBackup.BackupSetDescription = "InnboardDB:" + DateTime.Now.ToShortDateString();
                sqlBackup.BackupSetName = "Archive";

                sqlBackup.Database = databaseName;

                BackupDeviceItem deviceItem = new BackupDeviceItem(backupPath, DeviceType.File);
                ServerConnection connection = new ServerConnection(serverName, userName, password);
                Server sqlServer = new Server(connection);

                Database db = sqlServer.Databases[databaseName];

                sqlBackup.Initialize = true;
                sqlBackup.Checksum = true;
                sqlBackup.ContinueAfterError = true;
                
                sqlBackup.Devices.Add(deviceItem);
                sqlBackup.Incremental = false;

                sqlBackup.ExpirationDate = DateTime.Now.AddDays(3);
                sqlBackup.LogTruncation = BackupTruncateLogType.Truncate;

                sqlBackup.FormatMedia = false;

                sqlBackup.SqlBackup(sqlServer);

                CommonHelper.AlertInfo(innboardMessage, "Backup Process Succeed.", "success");
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, ex.InnerException.ToString(), "error");
            }

            //ServerConnection con = new ServerConnection(@"NAZRUL\SQLEXPRESS08");
            //Server server = new Server(con);
            //Backup source = new Backup();
            //source.Action = BackupActionType.Database;
            //source.Database = "Innboard";
            //BackupDeviceItem destination = new BackupDeviceItem(backUpFile, DeviceType.File);
            //source.Devices.Add(destination);
            //source.SqlBackup(server);
            //con.Disconnect();
        }
    }
}