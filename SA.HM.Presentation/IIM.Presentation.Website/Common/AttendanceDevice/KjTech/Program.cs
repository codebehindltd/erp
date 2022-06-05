using System;
using System.Windows.Forms;

namespace HotelManagement.Presentation.Website.Common.AttendanceDevice
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmMain());
        }

        public static int machineId = 1;
        public static int voipStatus;

        private static SFC3KPC_DLL sfc3kpc_dll = new SFC3KPC_DLL();

        public static SFC3KPC_DLL GetSFC3KPC_DLL()
        {
            return sfc3kpc_dll;
        }

        public static bool EnableDevice(bool enable, int machineId)
        {
            //int devId = int.Parse(machineId);
            return GetSFC3KPC_DLL().EnableDevice(machineId, enable);
        }

        public static string GetErrorString()
        {
            int vErrorCode = 0;
            GetSFC3KPC_DLL().GetLastError(out vErrorCode);

            switch (vErrorCode)
            {
                case 0:
                    return "No Error";

                case 1:
                    return "Can 't open com port";

                case 2:
                    return "Can 't set com port";

                case 3:
                    return "Error in creating socket";

                case 4:
                    return "Error in setting socket option";

                case 5:
                    return "Error in connecting";

                case 6:
                    return "Error in reconnecting";

                case 7:
                    return "The Password (or the machine ID) is incorrect";

                case 8:
                    return "Error in allocating memory in socket dll";

                case 101:
                    return "Can 't send data to device";

                case 102:
                    return "Can 't read data from device";

                case 103:
                    return "Error in parameter";

                case 104:
                    return "Invalid Data";

                case 105:
                    return "The scope of data is incorrect";

                case 501:
                    return "Can't operate the device properly (or none data)";

                case 502:
                    return "All data have been read";

                case 503:
                    return "Double access to FP(or CARD) data";

                case 504:
                    return "Error in allocating memory in device";

                case 505: //SFC3KPCERR_ENROLL_IMAGE_INVALID_ID:
                    return "Invalid ID";

                case 506:    //SFC3KPCERR_ENROLL_IMAGE_OVER_PER_TERMINAL
                    return "All fingerprints are enrolled at the terminal";

                case 507:    //SFC3KPCERR_ENROLL_IMAGE_ALREADY_ENROLLED
                    return "Fingerprint is already enrolled for this position";

                case 508:    //SFC3KPCERR_ENROLL_IMAGE_INVALID_NTH
                    return "Invalid step number";

                case 509:    //SFC3KPCERR_ENROLL_IMAGE_DUPLICATE
                    return "Fingerprint duplicated while enrolling image";

                case 510:    //SFC3KPCERR_ENROLL_IMAGE_NTH_ERROR
                    return "An error occurred while enrolling Nth image";

                case 511:    //SFC3KPCERR_ENROLL_IMAGE_CANT_LOAD
                    return "Could not load Fingerprint image file";

                case 512:    //SFC3KPCERR_ENROLL_IMAGE_ERROR
                    return "An error occurred while enrolling fingerprint image";

                case 513:    //SFC3KPCERR_BGIMAGE_CANT_OPEN
                    return "Cannot open background bitmap";

                case 514:    //SFC3KPCERR_BGIMAGE_INVALID_BITMAP
                    return "Invalid background bitmap";

                case 515:    //SFC3KPCERR_BGIMAGE_DIMENSION
                    return "Bitmap dimension is invalid";

                case 516:    //SFC3KPCERR_BGIMAGE_COLOR_DEPTH
                    return "Bitmap color depth is incorrect. It should be 24bit";

                case 517:    //SFC3KPCERR_BGIMAGE_COMPRESSED
                    return "This bitmap is compressed format. It should be an uncompressed bitmap";

                default:
                    return "Unknown";
            }
        }

        public static string GetXmlHeader(string cmd)
        {
            SFC3KPC_DLL sfc = GetSFC3KPC_DLL();
            string strXML = "";

            sfc.XML_AddString(ref strXML, "REQUEST", cmd);
            sfc.XML_AddString(ref strXML, "MSGTYPE", "request");
            sfc.XML_AddInt(ref strXML, "MachineID", machineId);

            return strXML;
        }
    }
}