class ApiEntPoint {
  static const String appLogin = "Account/AppsLogin";
  static const String attendance = "Account/AppsAttendance";
  static const String attendanceApplication =
      "Account/AppsAttendanceApplication";
  static const String empInfoWithLocation =
      "Employee/GetPayrollEmpListWithLocation";
  static const String empLocationSave = "Employee/EmpLocationTrackingSave";
  static const String voucherListGet =
      "GeneralLedger/GetVoucherBySearchCriteria";
  static const String voucherInfo = "GeneralLedger/GetVoucherInformation";
  static const String voucherApproved = "GeneralLedger/VoucherApproval";
  static const String leaveType = "Employee/GetLeaveType";
  static const String activeEmp = "Employee/GetActiveEmployees";
  static const String leaveApplication = "Employee/LeaveApplication";
  static const String leaveList =
      "Employee/GetLeaveInformationBySearchCriteria";
  static const String leaveGetById = "Employee/GetLeaveInformation";
  static const String leaveApproval = "Employee/LeaveApproval";
  static const String getUserType = "Common/GetCustomField";
  static const String getProperty = "Common/GetPropertyInformation";
  static const String selectProperty =
      "Account/AppsLoginUserByMasterUserInfoId";

  static const String membershipSetupData = "Member/GetMembershipSetupData";
  static const String memberRegistration =
      "Member/SaveMemMemberBasicInfoForMobileAppsRegistration";
  static const String memberPaymentSave =
      "Member/SaveMemberPaymentInfoForMobileAppsRegistration";

  // static const String roomTypeInfo = "Reservation/GetRoomTypeInfo";
  static const String roomTypeInfo = "Reservation/GetRoomTypeInfoForMobileApps";
  static const String checkRoomAvailable = "Reservation/GetAvailableRoomInfo";
  static const String aboutUs = "Common/GetCompanyInfo";

  static const String roomReservationInfo =
      "Reservation/SaveRoomReservationInfoForMobileApps";
  static const String roomReservationPaymentInfoSave =
      "Reservation/SaveRoomReservationPaymentInfoForMobileApps";
  static const String reservationList =
      "Reservation/GetRoomReservationInformationForMobileApps";

  static const String guestOrMemberProfile =
      "Common/GetGuestOrMemberProfileInformation";
  static const String guestOrMemberOffer =
      "Common/GetGuestOrMemberPromotionalOffer";

  static const String guestOrMemberLogin = "Account/GuestOrMemberAppsLogin";

  static const String profileUpdate =
      "member/UpdateMemMemberBasicInfoForMobileApps";
  static const String memberRoomNightInfo =
      "Common/GetMemberRoomNightInformation";
}
