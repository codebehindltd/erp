using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Presentation.Website.Common.AttendanceDevice;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmDeviceEmployeeMapping : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        private string ActiveMachineId = "0";
        private long sessionId = 0;
        private Actatek.ACTAtek aCTAtek;
        public List<Device> Devices = new List<Device>();
        public List<Device> DeviceList = new List<Device>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
                LoadEmployeeDepartment();
                LoadDeviceDropdown();
            }
        }

        private void LoadEmployeeDepartment()
        {
            DepartmentDA departmentDA = new DepartmentDA();
            EmployeeMappingDA employeeDA = new EmployeeMappingDA();
            ddlEmployeeDepartment.DataSource = departmentDA.GetDepartmentInfo();
            ddlEmployeeDepartment.DataTextField = "Name";
            ddlEmployeeDepartment.DataValueField = "DepartmentId";
            ddlEmployeeDepartment.DataBind();

            ddlMappingEmployeeDepartment.DataSource = employeeDA.GetMappingDepartmentInfo();
            ddlMappingEmployeeDepartment.DataTextField = "Name";
            ddlMappingEmployeeDepartment.DataValueField = "DepartmentId";
            ddlMappingEmployeeDepartment.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlEmployeeDepartment.Items.Insert(0, item);
            ddlMappingEmployeeDepartment.Items.Insert(0, item);
        }

        private void LoadDeviceDropdown()
        {
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            ddlDevice.Items.Insert(0, item);

            AttendanceDeviceDA deviceDA = new AttendanceDeviceDA();
            List<AttendanceDeviceBO> deviceList = deviceDA.GetAllAttendanceDeviceInfo();
            ListItem item2;

            foreach (var device in deviceList)
            {
                if (device.DeviceType == "Suprima")
                {
                    item2 = new ListItem();
                    item2.Value = device.ReaderId.ToString();
                    item2.Text = device.Name;
                    ddlDevice.Items.Insert(ddlDevice.Items.Count, item2);
                    EmployeeMappingDA employeeDA = new EmployeeMappingDA();
                    var Employees = employeeDA.GetMappingEmployeeList();
                    Devices.Add(new Device
                    {
                        DeviceType = device.DeviceType,
                        DeviceId = device.ReaderId,
                        Employees = Employees
                    });

                }
                else if (device.DeviceType == "KjTech")
                {
                    item2 = new ListItem();
                    item2.Value = device.ReaderId.ToString();
                    item2.Text = device.Name;
                    ddlDevice.Items.Insert(ddlDevice.Items.Count, item2);
                    GetDeviceEmployee(device.ReaderId, device.DeviceType);
                }
                else if (device.DeviceType == "Actatek")
                {
                    item2 = new ListItem();
                    item2.Value = device.ReaderId.ToString();
                    item2.Text = device.Name;
                    ddlDevice.Items.Insert(ddlDevice.Items.Count, item2);
                    GetDeviceEmployee(device.ReaderId, device.DeviceType);
                }
            }

            Session["Devices"] = Devices;
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

            DeviceList = Devices.Select(i => new Device { DeviceId = i.DeviceId, DeviceType = i.DeviceType }).ToList();
            hfDevice.Value = javaScriptSerializer.Serialize(DeviceList);
        }

        private bool IsLoggedInKjTech(AttendanceDeviceBO device)
        {
            try
            {
                ActiveMachineId = device.ReaderId;
                int password = device.Password != "" ? Convert.ToInt32(device.Password) : 0;
                int deviceId = int.Parse(device.ReaderId);
                return Program.GetSFC3KPC_DLL().ConnectTcpip(deviceId, device.IP, device.PortNumber, password);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool IsActatekDeviceLoggedIn(AttendanceDeviceBO device)
        {
            try
            {
                aCTAtek = new Actatek.ACTAtek();
                aCTAtek.Url = "http://" + device.IP + "/cgi-bin/rpcrouter";

                if (device != null)
                    sessionId = aCTAtek.login(device.UserName, device.Password);

                return sessionId > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void LoadKjTechDeviceEmployee(AttendanceDeviceBO device)
        {
            bool bRet;
            bool vRet;
            int i;
            int EnrollNumber = 0;
            string vName = "";
            List<EmployeeBO> Employees = new List<EmployeeBO>();

            Byte[] enrollDataBinary = new Byte[Constants.ENROLL_DATA_SIZE];
            int deviceId = int.Parse(ActiveMachineId);
            if (!Program.EnableDevice(false, deviceId))
            {
                return;
            }

            
            bRet = Program.GetSFC3KPC_DLL().ReadAllUserID(deviceId);

            if (bRet)
            {
                i = 0;
                GCHandle gh = GCHandle.Alloc(enrollDataBinary, GCHandleType.Pinned);
                IntPtr addr = gh.AddrOfPinnedObject();

                while (true)
                {
                    bRet = Program.GetSFC3KPC_DLL().GetAllUserID(deviceId, ref EnrollNumber, addr);
                    if (!bRet) break;

                    vRet = Program.GetSFC3KPC_DLL().GetUserName(deviceId, EnrollNumber, ref vName);

                    Employees.Add(new EmployeeBO()
                    {
                        EmpId = EnrollNumber,
                        EmpCode = EnrollNumber.ToString(),
                        EmployeeName = vName,
                        DisplayName = string.Format("{0:D8} - {1} ", EnrollNumber, vName)
                    });
                    ++i;
                }
                bRet = Program.EnableDevice(true, deviceId);

            }
            Devices.Add(new Device
            {
                DeviceType = device.DeviceType,
                DeviceId = device.ReaderId,
                Employees = Employees
            });
        }

        private void LoadActatekDeviceEmployee(AttendanceDeviceBO device)
        {
            List<EmployeeBO> Employees = new List<EmployeeBO>();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

            Actatek.getUsersCriteria usersCriteria = new Actatek.getUsersCriteria();

            Actatek.User[] users = aCTAtek.getUsers(sessionId, usersCriteria);

            Employees = (from emp in users
                         select new EmployeeBO
                         {
                             EmpCode = emp.userID,
                             EmployeeName = emp.firstName + emp.lastName,
                             DisplayName = emp.userID + " - " + emp.firstName + " " + emp.lastName
                         }).ToList();

            Devices.Add(new Device
            {
                DeviceType = device.DeviceType,
                DeviceId = device.ReaderId,
                Employees = Employees
            });


        }

        private void GetDeviceEmployee(string deviceId, string deviceType)
        {
            List<EmployeeBO> Employees = new List<EmployeeBO>();

            CommonDA commonDA = new CommonDA();
            Employees = commonDA.GetAllTableRow<EmployeeBO>("PayrollEmpInformationForAttendanceDevice", "DeviceId", deviceId.ToString());
            Employees = Employees.Select(i => { i.DisplayName = i.EmpCode + " - " + i.EmployeeName; return i; }).ToList();

            Devices.Add(new Device
            {
                DeviceType = deviceType,
                DeviceId = deviceId,
                Employees = Employees
            });
        }

        [WebMethod(EnableSession = true)]
        public static EmployeeViewBO GetEmployeeListByDepartmentId(long departmentId, long deviceId)
        {
            EmployeeViewBO employeeMappingList = new EmployeeViewBO();
            EmployeeMappingDA employeeDA = new EmployeeMappingDA();
            employeeMappingList.Employees = employeeDA.GetEmployeeByDepartmentAndStatus(departmentId, 1, deviceId);
            employeeMappingList.Devices = HttpContext.Current.Session["Devices"] as List<Device>;

            return employeeMappingList;
        }

        [WebMethod]
        public static ReturnInfo UpdateMapping(List<EmployeeViewBO> employeeList)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo returninfo = new ReturnInfo();
            EmployeeMappingDA employeeDA = new EmployeeMappingDA();
            try
            {
                bool status = employeeDA.UpdateEmployeeMapping(employeeList);
                if (status)
                {
                    returninfo.IsSuccess = true;
                    returninfo.ObjectList = new ArrayList(employeeList);
                    returninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    foreach (var item in employeeList)
                    {
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.Employee.ToString(), item.EmployeeId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Employee));
                    }
                }
                else
                {
                    returninfo.IsSuccess = false;
                    returninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                returninfo.IsSuccess = false;
                returninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return returninfo;
        }

        protected void btnGetDeviceEmployee_Click(object sender, EventArgs e)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo returninfo = new ReturnInfo();
            AttendanceDeviceDA deviceDA = new AttendanceDeviceDA();

            try
            {
                List<AttendanceDeviceBO> deviceList = deviceDA.GetAllAttendanceDeviceInfo();

                foreach (var device in deviceList)
                {
                    if (device.DeviceType == "Suprima")
                    {
                        EmployeeMappingDA employeeDA = new EmployeeMappingDA();
                        var Employees = employeeDA.GetMappingEmployeeList();
                        Devices.Add(new Device
                        {
                            DeviceType = device.DeviceType,
                            DeviceId = device.ReaderId,
                            Employees = Employees
                        });

                    }
                    else if (device.DeviceType == "KjTech" && IsLoggedInKjTech(device))
                    {
                        LoadKjTechDeviceEmployee(device);
                    }
                    else if (device.DeviceType == "Actatek" && IsActatekDeviceLoggedIn(device))
                    {
                        LoadActatekDeviceEmployee(device);
                    }
                }

                bool status = deviceDA.SaveEmpInfoForAttendanceDevice(Devices);
                if (status)
                {
                    int devId = int.Parse(Devices[0].DeviceId);
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.AttendanceDevice.ToString(), devId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.AttendanceDevice));
                }
                else
                {
                    CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

        }
    }


}