part of 'app_pages.dart';

abstract class Routes {
  Routes._();
  static const home = _Paths.home;
  static const splash = _Paths.splash;
  static const dashboard = _Paths.dashboard;
  static const profile = _Paths.profile;
  static const login = _Paths.login;
  static const attendanceSubmit = _Paths.attendanceSubmit;
  static const employeeList = _Paths.employeeList;
  static const employeeDetails = _Paths.employeeDetails;
  static const googleMapEmp = _Paths.googleMapEmp;
  static const googleMapPathEmp = _Paths.googleMapPathEmp;
  static const voucherApproval = _Paths.voucherApproval;
  // static const voucherFilter = _Paths.voucherFilter;
  static const voucherDetails = _Paths.voucherDetails;
  static const attendanceApplication = _Paths.attendanceApplication;
  //customer
  static const customerDashboard = _Paths.customerDashboard;
  static const userTypeSelection = _Paths.userTypeSelection;
  static const leaveStatus = _Paths.leaveStatus;
  static const leaveApplication = _Paths.leaveApplication;
  static const leaveApproval = _Paths.leaveApproval;
  static const propertySelection = _Paths.propertySelection;
  static const setCustomerInfo = _Paths.setCustomerInfo;
  static const reservation = _Paths.reservation;
  static const memberRegistration = _Paths.memberRegistration;
  static const memberRegistrationChoice = _Paths.memberRegistrationChoice;
  static const payment = _Paths.payment;
  static const successScreen = _Paths.successScreen;
  static const promotionalOffer = _Paths.promotionalOffer;
  static const bookingStatus = _Paths.bookingStatus;

  static const userTypeSelectionTwo = _Paths.userTypeSelectionTwo;
  static const aboutUs = _Paths.aboutUs;
  static const memberDashboard = _Paths.memberDashboard;
  static const memberReservation = _Paths.memberReservation;

  static const reservationSuccessScreen = _Paths.reservationSuccessScreen;
  static const guestReservation = _Paths.guestReservation;
  static const reservationInfoProvideModal = _Paths.reservationInfoProvideModal;
  static const profileGuestMember = _Paths.profileGuestMember;
  static const propertyDeatailsView = _Paths.propertyDeatailsView;
  static const memberReservationSuccessView =
      _Paths.memberReservationSuccessView;
  static const memberLogin = _Paths.memberLogin;
  static const bookingDeatailsView = _Paths.bookingDeatailsView;
  static const filePicker = _Paths.filePicker;
  static const fileListView = _Paths.fileListView;
  static const profileEditView = _Paths.profileEditView;
  static const paymentMoreThenFiveLacView = _Paths.paymentMoreThenFiveLacView;
  static const paymentGraterThenFiveLacView =
      _Paths.paymentGraterThenFiveLacView;
  static const guestPaymentGraterThenFiveLacView =
      _Paths.guestPaymentGraterThenFiveLacView;
  static const paymentDetailsView = _Paths.paymentDetailsView;
  static const paymentHistoryView = _Paths.paymentHistoryView;
}

abstract class _Paths {
  _Paths._();
  static const home = '/home';
  static const splash = '/splash';
  static const dashboard = '/dashboard';
  static const profile = '/profile';
  static const login = '/login';
  static const attendanceSubmit = '/attendance-submit';
  static const employeeList = '/employee-list';
  static const employeeDetails = '/employee-details';
  static const googleMapEmp = '/google-map-emp';
  static const googleMapPathEmp = '/google-map-path-emp';
  static const voucherApproval = '/voucher-approval';
  // static const voucherFilter = '/voucher-approval/filter';
  static const voucherDetails = '/voucher-details';
  static const attendanceApplication = '/attendance-application';

  //customer
  static const customerDashboard = '/customer-dashboard';
  static const userTypeSelection = '/user-type-selection';
  static const leaveStatus = '/leave-status';
  static const leaveApplication = '/leave-application';
  static const leaveApproval = '/leave-approval';
  static const propertySelection = '/property-selection';
  static const setCustomerInfo = '/set-customer-info';
  static const reservation = '/reservation';
  static const memberRegistration = '/member-registration';
  static const memberRegistrationChoice = '/member-registration-choice';
  static const payment = '/payment';
  static const successScreen = '/successScreen';
  static const promotionalOffer = '/promotional-offer';
  static const bookingStatus = '/booking-status';

  static const userTypeSelectionTwo = '/user-type-selection-two';
  static const aboutUs = '/about-us';
  static const memberDashboard = '/member-dashboard';
  static const memberReservation = '/member-reservation';

  static const reservationSuccessScreen = '/reservationSuccessScreen';
  static const guestReservation = '/guestReservation';
  static const reservationInfoProvideModal = '/reservationInfoProvideModal';
  static const profileGuestMember = '/profile-guest-member';
  static const propertyDeatailsView = '/propertyDeatailsView';

  static const memberReservationSuccessView = '/memberReservationSuccessView';
  static const memberLogin = '/memberLogin';
  static const bookingDeatailsView = '/bookingDeatailsView';
  static const filePicker = '/filePicker';
  static const fileListView = '/FileListView';

  static const profileEditView = '/profileEditView';
  static const paymentMoreThenFiveLacView = '/paymentMoreThenFiveLacView';

  static const paymentGraterThenFiveLacView = '/paymentGraterThenFiveLacView';

  static const guestPaymentGraterThenFiveLacView =
      '/guestPaymentGraterThenFiveLacView';

  static const paymentDetailsView = '/paymentDetailsView';
  static const paymentHistoryView = '/paymentHistoryView';
}
