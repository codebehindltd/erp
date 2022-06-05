using System;
using System.Runtime.InteropServices;

namespace HotelManagement.Presentation.Website.Common.AttendanceDevice
{
    public class SFC3KPC_DLL
    {
        // Progressing Event Handler
        public delegate void ProgressingEventHandler(Progression action, short percentage, int enrollNumber);

        public event ProgressingEventHandler hProgressing;

        public enum Progression
        {
            ReadAllEnrollData = 0,
            WriteAllEnrollData,
            UpgradeFirmware,
        };

        // VoIP Message Event Handler
        public delegate void VoIPMessageEventHandler(VoIPMessage message, int wParam, int lParam);

        public event VoIPMessageEventHandler hVoIPMessage;

        public enum VoIPMessage
        {
            IncomingCallDetected = 1,
        }

        // DLL Name
        private const string DllName = "SFC3KPCDLL.dll";

        // Delegates for DLL Callback
        private ProgressingCallback progressingDelegate;

        private VoIPMessageCallback voipMessageDelegate;

        public SFC3KPC_DLL()
        {
            progressingDelegate = new ProgressingCallback(_ProgressingCallback);
            voipMessageDelegate = new VoIPMessageCallback(_VoIPMessageCallback);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        //                                  Main APIs                                      //
        /////////////////////////////////////////////////////////////////////////////////////

        private delegate void ProgressingCallback(IntPtr context, short action, short percentage, int enrollNumber);

        private void _ProgressingCallback(IntPtr context, short action, short percentage, int enrollNumber)
        {
            var eventHandler = hProgressing;
            if (eventHandler != null)
                eventHandler((Progression)action, percentage, enrollNumber);
        }

        // --------------------------- GetLastError ---------------------------
        [DllImport(DllName, EntryPoint = "?_GetLastError@@YGHPAJ@Z")]
        private extern static bool _GetLastError(out int dwErrorCode);

        public bool GetLastError(out int errorCode)
        {
            return _GetLastError(out errorCode);
        }

        // --------------------------- ConnectSerial ---------------------------
        [DllImport(DllName, EntryPoint = "?_ConnectSerial@@YGHJJJ@Z")]
        private extern static bool _ConnectSerial(int dwMachineNumber, int dwCommPort, int dwBaudRate);

        public bool ConnectSerial(int machineNumber, int commPort, int baudRate)
        {
            return _ConnectSerial(machineNumber, commPort, baudRate);
        }

        // --------------------------- ConnectTcpip ---------------------------
        [DllImport(DllName, EntryPoint = "?_ConnectTcpip@@YGHJPB_WJJ@Z")]
        private extern static bool _ConnectTcpip(int dwMachineNumber, [MarshalAs(UnmanagedType.LPWStr)] string lpszIPAddress, int dwPortNumber, int dwPassWord);

        public bool ConnectTcpip(int machineNumber, string ipAddress, int portNumber, int password)
        {
            return _ConnectTcpip(machineNumber, ipAddress, portNumber, password);
        }

        // --------------------------- Disconnect ---------------------------
        [DllImport(DllName, EntryPoint = "?_Disconnect@@YGXJ@Z")]
        private extern static void _Disconnect(int dwMachineNumber);

        public void Disconnect(int machineNumber)
        {
            _Disconnect(machineNumber);
        }

        // --------------------------- EnableDevice ---------------------------
        [DllImport(DllName, EntryPoint = "?_EnableDevice@@YGHJH@Z", CharSet = CharSet.Auto)]
        private extern static bool _EnableDevice(int dwMachineNumber, bool bFlag);

        public bool EnableDevice(int machineId, bool enable)
        {
            return _EnableDevice(machineId, enable);
        }

        // --------------------------- GetEnrollData ---------------------------
        [DllImport(DllName, EntryPoint = "?_GetEnrollData@@YGHJJPAJ@Z")]
        private extern static bool _GetEnrollData(int dwMachineNumber, int dwEnrollNumber, IntPtr dwEnrollData);

        public bool GetEnrollData(int machineId, int enrollNumber, IntPtr enrollData)
        {
            return _GetEnrollData(machineId, enrollNumber, enrollData);
        }

        // --------------------------- GetEnrollDataFromIndex ---------------------------
        [DllImport(DllName, EntryPoint = "?_GetEnrollDataFromIndex@@YGHJJPAJ0@Z")]
        private extern static bool _GetEnrollDataFromIndex(int dwMachineNumber, int dwUserInfoIndex, ref int dwEnrollNumber, IntPtr dwEnrollData);

        public bool GetEnrollDataFromIndex(int machineNumber, int userInfoIndex, ref int enrollNumber, IntPtr enrollData)
        {
            return _GetEnrollDataFromIndex(machineNumber, userInfoIndex, ref enrollNumber, enrollData);
        }

        // --------------------------- ReadAllEnrollData ---------------------------
        [DllImport(DllName, EntryPoint = "?_ReadAllEnrollData@@YGHJPAJP6GXPAXFFJ@Z1@Z")]
        private extern static bool _ReadAllEnrollData(int dwMachineNumber, ref int dwUserCount, [MarshalAs(UnmanagedType.FunctionPtr)] ProgressingCallback callback, IntPtr context);

        public bool ReadAllEnrollData(int machineNumber, ref int userCount)
        {
            return _ReadAllEnrollData(machineNumber, ref userCount, progressingDelegate, IntPtr.Zero);
        }

        // --------------------------- SetEnrollData ---------------------------
        [DllImport(DllName, EntryPoint = "?_SetEnrollData@@YGHJJPAJ@Z")]
        private extern static bool _SetEnrollData(int dwMachineNumber, int dwEnrollNumber, IntPtr dwEnrollData);

        public bool SetEnrollData(int machineNumber, int enrollNumber, IntPtr enrollData)
        {
            return _SetEnrollData(machineNumber, enrollNumber, enrollData);
        }

        // --------------------------- SetEnrollDataToIndex ---------------------------
        [DllImport(DllName, EntryPoint = "?_SetEnrollDataToIndex@@YGHJJJPAJ@Z")]
        private extern static bool _SetEnrollDataToIndex(int dwMachineNumber, int dwUserInfoIndex, int dwEnrollNumber, IntPtr dwEnrollData);

        public bool SetEnrollDataToIndex(int machineNumber, int userInfoIndex, int enrollNumber, IntPtr enrollData)
        {
            return _SetEnrollDataToIndex(machineNumber, userInfoIndex, enrollNumber, enrollData);
        }

        // --------------------------- WriteAllEnrollData ---------------------------
        [DllImport(DllName, EntryPoint = "?_WriteAllEnrollData@@YGHJJP6GXPAXFFJ@Z0@Z")]
        private extern static bool _WriteAllEnrollData(int dwMachineNumber, int dwEnrollCount, [MarshalAs(UnmanagedType.FunctionPtr)] ProgressingCallback callback, IntPtr context);

        public bool WriteAllEnrollData(int machineNumber, int enrollCount)
        {
            return _WriteAllEnrollData(machineNumber, enrollCount, progressingDelegate, IntPtr.Zero);
        }

        // --------------------------- GetUserInfo ---------------------------
        [DllImport(DllName, EntryPoint = "?_GetUserInfo@@YGHJJPAJ@Z")]
        private extern static bool _GetUserInfo(int dwMachineNumber, int dwEnrollNumber, IntPtr dwUserInfo);

        public bool GetUserInfo(int machineNumber, int enrollNumber, IntPtr userInfo)
        {
            return _GetUserInfo(machineNumber, enrollNumber, userInfo);
        }

        // --------------------------- SetUserInfo ---------------------------
        [DllImport(DllName, EntryPoint = "?_SetUserInfo@@YGHJJPAJ@Z")]
        private extern static bool _SetUserInfo(int dwMachineNumber, int dwEnrollNumber, IntPtr dwUserInfo);

        public bool SetUserInfo(int machineNumber, int enrollNumber, IntPtr userInfo)
        {
            return _SetUserInfo(machineNumber, enrollNumber, userInfo);
        }

        // --------------------------- DeleteEnrollData ---------------------------
        [DllImport(DllName, EntryPoint = "?_DeleteEnrollData@@YGHJJ@Z")]
        private extern static bool _DeleteEnrollData(int dwMachineNumber, int dwEnrollNumber);

        public bool DeleteEnrollData(int machineNumber, int enrollNumber)
        {
            return _DeleteEnrollData(machineNumber, enrollNumber);
        }

        // --------------------------- EmptyEnrollData ---------------------------
        [DllImport(DllName, EntryPoint = "?_EmptyEnrollData@@YGHJ@Z")]
        private extern static bool _EmptyEnrollData(int dwMachineNumber);

        public bool EmptyEnrollData(int machineNumber)
        {
            return _EmptyEnrollData(machineNumber);
        }

        // --------------------------- ReadAllUserID ---------------------------
        [DllImport(DllName, EntryPoint = "?_ReadAllUserID@@YGHJ@Z")]
        private extern static bool _ReadAllUserID(int machineNumber);

        public bool ReadAllUserID(int machineNumber) // not tested
        {
            return _ReadAllUserID(machineNumber);
        }

        // --------------------------- GetAllUserID ---------------------------
        [DllImport(DllName, EntryPoint = "?_GetAllUserID@@YGHJPAJ0@Z")]
        private extern static bool _GetAllUserID(int dwMachineNumber, ref int dwEnrollNumber, IntPtr dwEnrollData);

        public bool GetAllUserID(int machineNumber, ref int enrollNumber, IntPtr enrollData)
        {
            return _GetAllUserID(machineNumber, ref enrollNumber, enrollData);
        }

        // --------------------------- ModifyPrivilege ---------------------------
        [DllImport(DllName, EntryPoint = "?_ModifyPrivilege@@YGHJJJ@Z")]
        private extern static bool _ModifyPrivilege(int dwMachineNumber, int dwEnrollNumber, int dwMachinePrivilege);

        public bool ModifyPrivilege(int machineNumber, int enrollNumber, int machinePrivilege)
        {
            return _ModifyPrivilege(machineNumber, enrollNumber, machinePrivilege);
        }

        // --------------------------- ReadSuperLogData ---------------------------
        [DllImport(DllName, EntryPoint = "?_ReadSuperLogData@@YGHJ@Z")]
        private extern static bool _ReadSuperLogData(int dwMachineNumber);

        public bool ReadSuperLogData(int machineNumber)
        {
            return _ReadSuperLogData(machineNumber);
        }

        // --------------------------- GetSuperLogData ---------------------------
        [DllImport(DllName, EntryPoint = "?_GetSuperLogData@@YGHJPAJ00000000@Z")]
        private extern static bool _GetSuperLogData(int dwMachineNumber, ref int dwEnrollNumber, ref int dwDevice, ref int dwManipulation, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond);

        public bool GetSuperLogData(int machineNumber, ref int enrollNumber, ref int device, ref int manipulation, ref int year, ref int month, ref int day, ref int hour, ref int minute, ref int second)
        {
            return _GetSuperLogData(machineNumber, ref enrollNumber, ref device, ref manipulation, ref year, ref month, ref day, ref hour, ref minute, ref second);
        }

        // --------------------------- DeleteReadSuperLogData ---------------------------
        [DllImport(DllName, EntryPoint = "?_DeleteReadSuperLogData@@YGHJ@Z")]
        private extern static bool _DeleteReadSuperLogData(int dwMachineNumber);

        public bool DeleteReadSuperLogData(int machineNumber)
        {
            return _DeleteReadSuperLogData(machineNumber);
        }

        // --------------------------- StartReadSuperLogData ---------------------------
        [DllImport(DllName, EntryPoint = "?_StartReadSuperLogData@@YGHJ@Z")]
        private extern static bool _StartReadSuperLogData(int dwMachineNumber);

        public bool StartReadSuperLogData(int machineNumber)
        {
            return _StartReadSuperLogData(machineNumber);
        }

        // --------------------------- EmptySuperLogData ---------------------------
        [DllImport(DllName, EntryPoint = "?_EmptySuperLogData@@YGHJ@Z")]
        private extern static bool _EmptySuperLogData(int dwMachineNumber);

        public bool EmptySuperLogData(int machineNumber)
        {
            return _EmptySuperLogData(machineNumber);
        }

        // --------------------------- ReadGeneralLogData ---------------------------
        [DllImport(DllName, EntryPoint = "?_ReadGeneralLogData@@YGHJ@Z")]
        private extern static bool _ReadGeneralLogData(int dwMachineNumber);

        public bool ReadGeneralLogData(int machineNumber)
        {
            return _ReadGeneralLogData(machineNumber);
        }

        // --------------------------- GetGeneralLogData ---------------------------
        [DllImport(DllName, EntryPoint = "?_GetGeneralLogData@@YGHJPAJ00000000000@Z")]
        private extern static bool _GetGeneralLogData(int dwMachineNumber, ref int dwEnrollNumber, ref int dwGranted, ref int dwMethod, ref int dwDoorMode, ref int dwFunNumber, ref int dwSensor, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond);

        public bool GetGeneralLogData(int machineNumber, ref int enrollNumber, ref int granted, ref int method, ref int doorMode, ref int funNumber, ref int sensor, ref int year, ref int month, ref int day, ref int hour, ref int minute, ref int second)
        {
            return _GetGeneralLogData(machineNumber, ref enrollNumber, ref granted, ref method, ref doorMode, ref funNumber, ref sensor, ref year, ref month, ref day, ref hour, ref minute, ref second);
        }

        // --------------------------- DeleteReadGeneralLogData ---------------------------
        [DllImport(DllName, EntryPoint = "?_DeleteReadGeneralLogData@@YGHJ@Z")]
        private extern static bool _DeleteReadGeneralLogData(int dwMachineNumber);

        public bool DeleteReadGeneralLogData(int machineNumber)
        {
            return _DeleteReadGeneralLogData(machineNumber);
        }

        // --------------------------- StartReadGeneralLogData ---------------------------
        [DllImport(DllName, EntryPoint = "?_StartReadGeneralLogData@@YGHJ@Z")]
        private extern static bool _StartReadGeneralLogData(int dwMachineNumber);

        public bool StartReadGeneralLogData(int machineNumber)
        {
            return _StartReadGeneralLogData(machineNumber);
        }

        // --------------------------- EmptyGeneralLogData ---------------------------
        [DllImport(DllName, EntryPoint = "?_EmptyGeneralLogData@@YGHJ@Z")]
        private extern static bool _EmptyGeneralLogData(int dwMachineNumber);

        public bool EmptyGeneralLogData(int machineNumber)
        {
            return _EmptyGeneralLogData(machineNumber);
        }

        // --------------------------- ClearKeeperData ---------------------------
        [DllImport(DllName, EntryPoint = "?_ClearKeeperData@@YGHJ@Z")]
        private extern static bool _ClearKeeperData(int dwMachineNumber);

        public bool ClearKeeperData(int machineNumber)
        {
            return _ClearKeeperData(machineNumber);
        }

        // --------------------------- GetProductCode ---------------------------
        [DllImport(DllName, EntryPoint = "?_GetProductCode@@YGHJPAPA_W@Z")]
        private extern static bool _GetProductCode(int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszProductCode);

        public bool GetProductCode(int machineNumber, ref string productCode)
        {
            return _GetProductCode(machineNumber, ref productCode);
        }

        // --------------------------- GetSerialNumber ---------------------------
        [DllImport(DllName, EntryPoint = "?_GetSerialNumber@@YGHJPAPA_W@Z")]
        private extern static bool _GetSerialNumber(int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszSerialNumber);

        public bool GetSerialNumber(int machineNumber, ref string serialNumber)
        {
            return _GetSerialNumber(machineNumber, ref serialNumber);
        }

        // --------------------------- GetDeviceStatus ---------------------------
        [DllImport(DllName, EntryPoint = "?_GetDeviceStatus@@YGHJJPAJ@Z")]
        private extern static bool _GetDeviceStatus(int dwMachineNumber, int dwStatus, ref int dwValue);

        public bool GetDeviceStatus(int machineNumber, int status, ref int value)
        {
            return _GetDeviceStatus(machineNumber, status, ref value);
        }

        // --------------------------- GetDeviceInfo ---------------------------
        [DllImport(DllName, EntryPoint = "?_GetDeviceInfo@@YGHJJPAJ@Z")]
        private extern static bool _GetDeviceInfo(int dwMachineNumber, int dwInfo, ref int dwValue);

        public bool GetDeviceInfo(int machineNumber, int info, ref int value)
        {
            return _GetDeviceInfo(machineNumber, info, ref value);
        }

        // --------------------------- SetDeviceInfo ---------------------------
        [DllImport(DllName, EntryPoint = "?_SetDeviceInfo@@YGHJJJ@Z")]
        private extern static bool _SetDeviceInfo(int dwMachineNumber, int dwInfo, int dwValue);

        public bool SetDeviceInfo(int machineNumber, int info, int value)
        {
            return _SetDeviceInfo(machineNumber, info, value);
        }

        // --------------------------- GetDeviceLongInfo ---------------------------
        [DllImport(DllName, EntryPoint = "?_GetDeviceLongInfo@@YGHJJPAJ@Z")]
        private extern static bool _GetDeviceLongInfo(int dwMachineNumber, int dwInfo, IntPtr dwValue);

        public bool GetDeviceLongInfo(int machineNumber, int info, IntPtr value)
        {
            return _GetDeviceLongInfo(machineNumber, info, value);
        }

        // --------------------------- SetDeviceLongInfo ---------------------------
        [DllImport(DllName, EntryPoint = "?_SetDeviceLongInfo@@YGHJJPAJ@Z")]
        private extern static bool _SetDeviceLongInfo(int dwMachineNumber, int dwInfo, IntPtr dwValue);

        public bool SetDeviceLongInfo(int machineNumber, int info, IntPtr value)
        {
            return _SetDeviceLongInfo(machineNumber, info, value);
        }

        // --------------------------- GetDoorStatus ---------------------------
        [DllImport(DllName, EntryPoint = "?_GetDoorStatus@@YGHJPAJ0@Z")]
        private extern static bool _GetDoorStatus(int dwMachineNumber, ref int dwStatus, ref int dwDelay);

        public bool GetDoorStatus(int machineNumber, ref int status, ref int delay)
        {
            return _GetDoorStatus(machineNumber, ref status, ref delay);
        }

        // --------------------------- SetDoorStatus ---------------------------
        [DllImport(DllName, EntryPoint = "?_SetDoorStatus@@YGHJJJ@Z")]
        private extern static bool _SetDoorStatus(int dwMachineNumber, int dwStatus, int dwDelay);

        public bool SetDoorStatus(int machineNumber, int status, int delay)
        {
            return _SetDoorStatus(machineNumber, status, delay);
        }

        // --------------------------- GetDeviceTime ---------------------------
        [DllImport(DllName, EntryPoint = "?_GetDeviceTime@@YGHJPAJ000000@Z")]
        private extern static bool _GetDeviceTime(int dwMachineNumber, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond, ref int dwDayOfWeek);

        public bool GetDeviceTime(int machineNumber, ref int year, ref int month, ref int day, ref int hour, ref int minute, ref int second, ref int dayOfWeek)
        {
            return _GetDeviceTime(machineNumber, ref year, ref month, ref day, ref hour, ref minute, ref second, ref dayOfWeek);
        }

        // --------------------------- SetDeviceTime ---------------------------
        [DllImport(DllName, EntryPoint = "?_SetDeviceTime@@YGHJ@Z")]
        private extern static bool _SetDeviceTime(int dwMachineNumber);

        public bool SetDeviceTime(int machineNumber)
        {
            return _SetDeviceTime(machineNumber);
        }

        // --------------------------- PowerOffDevice ---------------------------
        [DllImport(DllName, EntryPoint = "?_PowerOffDevice@@YGHJ@Z")]
        private extern static bool _PowerOffDevice(int dwMachineNumber);

        public bool PowerOffDevice(int machineNumber)
        {
            return _PowerOffDevice(machineNumber);
        }

        // --------------------------- FirmwareUpgrade ---------------------------
        [DllImport(DllName, EntryPoint = "?_FirmwareUpgrade@@YGHJPB_WP6GXPAXFFJ@Z1@Z")]
        private extern static bool _FirmwareUpgrade(
            int dwMachineNumber,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszFirmwareImageFilePath,
            [MarshalAs(UnmanagedType.FunctionPtr)] ProgressingCallback callback,
            IntPtr context);

        public bool FirmwareUpgrade(int machineNumber, string fwFile)
        {
            return _FirmwareUpgrade(machineNumber, fwFile, progressingDelegate, IntPtr.Zero);
        }

        // --------------------------- GetUserName ---------------------------
        [DllImport(DllName, EntryPoint = "?_GetUserName@@YGHJJPAPA_W@Z")]
        private extern static bool _GetUserName(int dwMachineNumber, int dwEnrollNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszName);

        public bool GetUserName(int machineNumber, int enrollNumber, ref string name)
        {
            return _GetUserName(machineNumber, enrollNumber, ref name);
        }

        // --------------------------- SetUserName ---------------------------
        [DllImport(DllName, EntryPoint = "?_SetUserName@@YGHJJPAPA_W@Z")]
        private extern static bool _SetUserName(int dwMachineNumber, int dwEnrollNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszName);

        public bool SetUserName(int machineNumber, int enrollNumber, ref string name)
        {
            return _SetUserName(machineNumber, enrollNumber, ref name);
        }

        // --------------------------- GetMachineIP ---------------------------
        [DllImport(DllName, EntryPoint = "?_GetMachineIP@@YGHPB_WJPAPA_W@Z")]
        private extern static bool _GetMachineIP([MarshalAs(UnmanagedType.LPWStr)] string lpszProduct, int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszIPBuf);

        public bool GetMachineIP(string product, int machineNumber, ref string ipBuffer)
        {
            return _GetMachineIP(product, machineNumber, ref ipBuffer);
        }

        // --------------------------- GeneralOperationXML ---------------------------
        [DllImport(DllName, EntryPoint = "?_GeneralOperationXML@@YGHPAPA_W@Z")]
        private extern static bool _GeneralOperationXML([MarshalAs(UnmanagedType.BStr)] ref string lpszReqNResXML);

        public bool GeneralOperationXML(ref string xmlReqNRes)
        {
            return _GeneralOperationXML(ref xmlReqNRes);
        }

        // --------------------------- GetDeviceNetworkStatus ---------------------------
        [DllImport(DllName, EntryPoint = "?_GetDeviceNetworkStatus@@YGJPB_W@Z")]
        private extern static int _GetDeviceNetworkStatus([MarshalAs(UnmanagedType.LPWStr)] string lpszIPAddress);

        public int GetDeviceNetworkStatus(string ipAddress)
        {
            return _GetDeviceNetworkStatus(ipAddress);
        }

        // --------------------------- GetPhotoLogData ---------------------------
        [DllImport(DllName, EntryPoint = "?_GetPhotoLogData@@YGHJJJJJJJJPAJ0@Z")]
        private extern static bool _GetPhotoLogData(int dwMachineNumber, int dwEnrollNumber, int dwYear, int dwMonth, int dwDay, int dwHour, int dwMinute, int dwSecond, ref int dwSize, IntPtr dwData);

        public bool GetPhotoLogData(int machineNumber, int enrollNumber, int year, int month, int day, int hour, int minute, int second, ref int size, IntPtr data)
        {
            return _GetPhotoLogData(machineNumber, enrollNumber, year, month, day, hour, minute, second, ref size, data);
        }

        // --------------------------- GetUserPhoto ---------------------------
        [DllImport(DllName, EntryPoint = "?_GetUserPhoto@@YGHJJPAJ0@Z")]
        private extern static bool _GetUserPhoto(int dwMachineNumber, int dwEnrollNumber, ref int dwSize, IntPtr dwData);

        public bool GetUserPhoto(int machineNumber, int enrollNumber, ref int size, IntPtr data)
        {
            return _GetUserPhoto(machineNumber, enrollNumber, ref size, data);
        }

        // --------------------------- SetUserPhoto ---------------------------
        [DllImport(DllName, EntryPoint = "?_SetUserPhoto@@YGHJJJPAJ@Z")]
        private extern static bool _SetUserPhoto(int dwMachineNumber, int dwEnrollNumber, int dwSize, IntPtr dwData);

        public bool SetUserPhoto(int machineNumber, int enrollNumber, int size, IntPtr data)
        {
            return _SetUserPhoto(machineNumber, enrollNumber, size, data);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        //                                  XML Helpers                                    //
        /////////////////////////////////////////////////////////////////////////////////////

        // --------------------------- XML_ParseInt ---------------------------
        [DllImport(DllName, EntryPoint = "?_XML_ParseInt@@YGJPAPA_WPB_W@Z")]
        private extern static int _XML_ParseInt([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.LPWStr)] string lpszTagname);

        public int XML_ParseInt(ref string xml, string tagName)
        {
            return _XML_ParseInt(ref xml, tagName);
        }

        // --------------------------- XML_ParseLong ---------------------------
        [DllImport(DllName, EntryPoint = "?_XML_ParseLong@@YGJPAPA_WPB_W@Z")]
        private extern static int _XML_ParseLong([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.LPWStr)] string lpszTagname);

        public int XML_ParseLong(ref string xml, string tagName)
        {
            return _XML_ParseLong(ref xml, tagName);
        }

        // --------------------------- XML_ParseBoolean ---------------------------
        [DllImport(DllName, EntryPoint = "?_XML_ParseBoolean@@YGHPAPA_WPB_W@Z")]
        private extern static bool _XML_ParseBoolean([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.LPWStr)] string lpszTagname);

        public bool XML_ParseBoolean(ref string xml, string tagName)
        {
            return _XML_ParseBoolean(ref xml, tagName);
        }

        // --------------------------- XML_ParseString ---------------------------
        [DllImport(DllName, EntryPoint = "?_XML_ParseString@@YGHPAPA_WPB_W0@Z")]
        private extern static bool _XML_ParseString([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.LPWStr)] string lpszTagname, [MarshalAs(UnmanagedType.BStr)] ref string lpszValue);

        public bool XML_ParseString(ref string xml, string tagName, ref string value)
        {
            return _XML_ParseString(ref xml, tagName, ref value);
        }

        // --------------------------- XML_ParseBinaryByte ---------------------------
        [DllImport(DllName, EntryPoint = "?_XML_ParseBinaryByte@@YGHPAPA_WPB_WPAEJ@Z")]
        private extern static bool _XML_ParseBinaryByte([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.LPWStr)] string lpszTagname, IntPtr pData, int dwLen);

        public bool XML_ParseBinaryByte(ref string xml, string tagName, IntPtr data, int len)
        {
            return _XML_ParseBinaryByte(ref xml, tagName, data, len);
        }

        // --------------------------- XML_ParseBinaryWord ---------------------------
        [DllImport(DllName, EntryPoint = "?_XML_ParseBinaryWord@@YGHPAPA_WPB_WPAGJ@Z")]
        private extern static bool _XML_ParseBinaryWord([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.LPWStr)] string lpszTagname, IntPtr pData, int dwLen);

        public bool XML_ParseBinaryWord(ref string xml, string tagName, IntPtr data, int len)
        {
            return _XML_ParseBinaryWord(ref xml, tagName, data, len);
        }

        // --------------------------- XML_ParseBinaryLong ---------------------------
        [DllImport(DllName, EntryPoint = "?_XML_ParseBinaryLong@@YGHPAPA_WPB_WPAJJ@Z")]
        private extern static bool _XML_ParseBinaryLong([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.LPWStr)] string lpszTagname, IntPtr pData, int dwLen);

        public bool XML_ParseBinaryLong(ref string xml, string tagName, IntPtr data, int len)
        {
            return _XML_ParseBinaryLong(ref xml, tagName, data, len);
        }

        // --------------------------- XML_ParseBinaryUnicode ---------------------------
        [DllImport(DllName, EntryPoint = "?_XML_ParseBinaryUnicode@@YGHPAPA_WPB_W0J@Z")]
        private extern static bool _XML_ParseBinaryUnicode([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.LPWStr)] string lpszTagname, [MarshalAs(UnmanagedType.BStr)] ref string pData, int dwLen);

        public bool XML_ParseBinaryUnicode(ref string xml, string tagName, ref string data, int len)
        {
            return _XML_ParseBinaryUnicode(ref xml, tagName, ref data, len);
        }

        // --------------------------- XML_AddInt ---------------------------
        [DllImport(DllName, EntryPoint = "?_XML_AddInt@@YGHPAPA_WPB_WH@Z")]
        private extern static bool _XML_AddInt([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.LPWStr)] string lpszTagname, int nValue);

        public bool XML_AddInt(ref string xml, string tagName, int value)
        {
            return _XML_AddInt(ref xml, tagName, value);
        }

        // --------------------------- XML_AddLong ---------------------------
        [DllImport(DllName, EntryPoint = "?_XML_AddLong@@YGHPAPA_WPB_WJ@Z")]
        private extern static bool _XML_AddLong([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.LPWStr)] string lpszTagname, int dwValue);

        public bool XML_AddLong(ref string xml, string tagName, int value)
        {
            return _XML_AddLong(ref xml, tagName, value);
        }

        // --------------------------- XML_AddBoolean ---------------------------
        [DllImport(DllName, EntryPoint = "?_XML_AddBoolean@@YGHPAPA_WPB_WH@Z")]
        private extern static bool _XML_AddBoolean([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.LPWStr)] string lpszTagname, bool bValue);

        public bool XML_AddBoolean(ref string xml, string tagName, bool value)
        {
            return _XML_AddBoolean(ref xml, tagName, value);
        }

        // --------------------------- _XML_AddString ---------------------------
        [DllImport(DllName, EntryPoint = "?_XML_AddString@@YGHPAPA_WPB_W1@Z")]
        private extern static bool _XML_AddString([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.LPWStr)] string lpszTagname, [MarshalAs(UnmanagedType.LPWStr)] string lpszValue);

        public bool XML_AddString(ref string xml, string tagName, string value)
        {
            return _XML_AddString(ref xml, tagName, value);
        }

        // --------------------------- XML_AddBinaryByte ---------------------------
        [DllImport(DllName, EntryPoint = "?_XML_AddBinaryByte@@YGHPAPA_WPB_WPAEJ@Z")]
        private extern static bool _XML_AddBinaryByte([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.LPWStr)] string lpszTagname, IntPtr dwData, int dwLen);

        public bool XML_AddBinaryByte(ref string xml, string tagName, IntPtr data, int len)
        {
            return _XML_AddBinaryByte(ref xml, tagName, data, len);
        }

        // --------------------------- XML_AddBinaryWord ---------------------------
        [DllImport(DllName, EntryPoint = "?_XML_AddBinaryWord@@YGHPAPA_WPB_WPAGJ@Z")]
        private extern static bool _XML_AddBinaryWord([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.LPWStr)] string lpszTagname, IntPtr dwData, int dwLen);

        public bool XML_AddBinaryWord(ref string xml, string tagName, IntPtr data, int len)
        {
            return _XML_AddBinaryWord(ref xml, tagName, data, len);
        }

        // --------------------------- XML_AddBinaryLong ---------------------------
        [DllImport(DllName, EntryPoint = "?_XML_AddBinaryLong@@YGHPAPA_WPB_WPAJJ@Z")]
        private extern static bool _XML_AddBinaryLong([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.LPWStr)] string lpszTagname, IntPtr dwData, int dwLen);

        public bool XML_AddBinaryLong(ref string xml, string tagName, IntPtr data, int len)
        {
            return _XML_AddBinaryLong(ref xml, tagName, data, len);
        }

        // --------------------------- XML_AddBinaryUnicode ---------------------------
        [DllImport(DllName, EntryPoint = "?_XML_AddBinaryUnicode@@YGHPAPA_WPB_W0@Z")]
        private extern static bool _XML_AddBinaryUnicode([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.LPWStr)] string lpszTagname, [MarshalAs(UnmanagedType.BStr)] ref string lpszData);

        public bool XML_AddBinaryUnicode(ref string xml, string tagName, ref string data)
        {
            return _XML_AddBinaryUnicode(ref xml, tagName, ref data);
        }

        /////////////////////////////////////////////////////////////////////////////////////
        //                                VoIP Interfaces                                  //
        /////////////////////////////////////////////////////////////////////////////////////

        private delegate void VoIPMessageCallback(IntPtr context, int wParam, int lParam);

        private void _VoIPMessageCallback(IntPtr context, int wParam, int lParam)
        {
            var eventHandler = hVoIPMessage;
            if (eventHandler != null)
            {
                if (wParam == (int)VoIPMessage.IncomingCallDetected)
                    eventHandler(VoIPMessage.IncomingCallDetected, lParam, 0);
            }
        }

        // --------------------------- VoIP_Init ---------------------------
        [DllImport(DllName, EntryPoint = "?_VoIP_Init@@YGHPB_WPAPA_WJPAXP6GX2JJ@Z@Z")]
        private extern static bool _VoIP_Init(
            [MarshalAs(UnmanagedType.LPWStr)] string lpszRingWaveFile,
            [MarshalAs(UnmanagedType.BStr)] ref string lpszErrorMessage,
            int dwReserved,
            IntPtr context,
            [MarshalAs(UnmanagedType.FunctionPtr)] VoIPMessageCallback callback);

        public bool VoIP_Init(string ringWaveFile, ref string errorMessage, int reserved)
        {
            return _VoIP_Init(ringWaveFile, ref errorMessage, reserved, IntPtr.Zero, voipMessageDelegate);
        }

        // --------------------------- VoIP_SendCommand ---------------------------
        [DllImport(DllName, EntryPoint = "?_VoIP_SendCommand@@YGXJJJ@Z")]
        private extern static void _VoIP_SendCommand(int dwCommand, int dwParam, int dwReserved);

        public void VoIP_SendCommand(int command, int param, int reserved)
        {
            _VoIP_SendCommand(command, param, reserved);
        }

        // --------------------------- VoIP_GetStatus ---------------------------
        [DllImport(DllName, EntryPoint = "?_VoIP_GetStatus@@YGJXZ")]
        private extern static int _VoIP_GetStatus();

        public int VoIP_GetStatus()
        {
            return _VoIP_GetStatus();
        }

        // --------------------------- VoIP_GetClientInfo ---------------------------
        [DllImport(DllName, EntryPoint = "?_VoIP_GetClientInfo@@YGXPAJ@Z")]
        private extern static void _VoIP_GetClientInfo(IntPtr dwParams);

        public void VoIP_GetClientInfo(IntPtr dwParams)
        {
            _VoIP_GetClientInfo(dwParams);
        }

        // --------------------------- VoIP_GetImageExt ---------------------------
        [DllImport(DllName, EntryPoint = "?_VoIP_GetImageExt@@YGHPAJ00@Z")]
        private extern static bool _VoIP_GetImageExt(IntPtr dwImage, ref int dwWidth, ref int dwHeight);

        public bool VoIP_GetImageExt(IntPtr image, ref int width, ref int height)
        {
            return _VoIP_GetImageExt(image, ref width, ref height);
        }

        // --------------------------- VoIP_DeInit ---------------------------
        [DllImport(DllName, EntryPoint = "?_VoIP_DeInit@@YGHJ@Z")]
        private extern static bool _VoIP_DeInit(int dwReserved);

        public bool VoIP_DeInit(int reserved)
        {
            return _VoIP_DeInit(reserved);
        }

        // --------------------------- LumidigmEnrollStart ---------------------------
        [DllImport(DllName, EntryPoint = "?_LumidigmEnrollStart@@YGHJ@Z")]
        private extern static bool _LumidigmEnrollStart(int dwMachineNumber);

        public bool LumidigmEnrollStart(int machineNumber)
        {
            return _LumidigmEnrollStart(machineNumber);
        }

        // --------------------------- LumidigmSetImage ---------------------------
        [DllImport(DllName, EntryPoint = "?_LumidigmSetImage@@YGHJJJJPAJ@Z")]
        private extern static bool _LumidigmSetImage(int dwMachineNumber, int dwImageNumber, int dwWidth, int dwHeight, IntPtr dwData);

        public bool LumidigmSetImage(int machineNumber, int imageNumber, int width, int height, IntPtr data)
        {
            return _LumidigmSetImage(machineNumber, imageNumber, width, height, data);
        }

        // --------------------------- LumidigmEnrollEnd ---------------------------
        [DllImport(DllName, EntryPoint = "?_LumidigmEnrollEnd@@YGHJ@Z")]
        private extern static bool _LumidigmEnrollEnd(int dwMachineNumber);

        public bool LumidigmEnrollEnd(int machineNumber)
        {
            return _LumidigmEnrollEnd(machineNumber);
        }

        // --------------------------- LumidigmGetTemplate ---------------------------
        [DllImport(DllName, EntryPoint = "?_LumidigmGetTemplate@@YGHJPAJ@Z")]
        private extern static bool _LumidigmGetTemplate(int dwMachineNumber, IntPtr dwData);

        public bool LumidigmGetTemplate(int machineNumber, IntPtr data)
        {
            return _LumidigmGetTemplate(machineNumber, data);
        }

        // --------------------------- GetCompanyName ---------------------------
        [DllImport(DllName, EntryPoint = "?_GetCompanyName@@YGHJPAPA_WPAJ1@Z")]
        private extern static bool _GetCompanyName(int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszCompanyName, ref int dwTopOffset, ref int dwColor);

        public bool GetCompanyName(int machineNumber, ref string companyName, ref int topOffset, ref int color)
        {
            return _GetCompanyName(machineNumber, ref companyName, ref topOffset, ref color);
        }

        // --------------------------- SetCompanyName ---------------------------
        [DllImport(DllName, EntryPoint = "?_SetCompanyName@@YGHJPAPA_WJJJ0MJJJ@Z")]
        private extern static bool _SetCompanyName(int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszCompanyName, int dwTopOffset, int dwLeftOffset, int dwColor, [MarshalAs(UnmanagedType.BStr)] ref string lpszFontFace, float dwFontHeight, int dwDecorationFlag, int dwSecond, int dwType);

        public bool SetCompanyName(int machineNumber, ref string companyName, int topOffset, int leftOffset, int color, ref string fontFace, float fontHeight, int decorationFlag, int second, int type)
        {
            return _SetCompanyName(machineNumber, ref companyName, topOffset, leftOffset, color, ref fontFace, fontHeight, decorationFlag, second, type);
        }

        // --------------------------- SetBackgroundImage ---------------------------
        [DllImport(DllName, EntryPoint = "?_SetBackgroundImage@@YGHJPB_W@Z")]
        private extern static bool _SetBackgroundImage(int dwMachineNumber, [MarshalAs(UnmanagedType.LPWStr)] string lpszBitmapFile);

        public bool SetBackgroundImage(int machineNumber, string bitmapFile)
        {
            return _SetBackgroundImage(machineNumber, bitmapFile);
        }
    }
}