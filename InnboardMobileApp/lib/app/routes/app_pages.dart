import 'package:get/get.dart';

import '../data/models/res/emp_model.dart';
import '../data/models/res/reservation/reservation_list_model.dart';
import '../modules/become_member_modules/member_registration/bindings/member_registration_binding.dart';
import '../modules/become_member_modules/member_registration/views/member_registration_choice_view.dart';
import '../modules/become_member_modules/member_registration/views/member_registration_view.dart';
import '../modules/become_member_modules/member_registration/views/payment_view.dart';
import '../modules/become_member_modules/member_registration/views/success_screen_view.dart';
import '../modules/common/middleware/auth_middleware.dart';
import '../modules/common/middleware/guest_member_middleware.dart';
import '../modules/common/middleware/user_module_middleware.dart';
import '../modules/common_modules/booking_status/bindings/booking_status_binding.dart';
import '../modules/common_modules/booking_status/views/booking_deatails_view.dart';
import '../modules/common_modules/booking_status/views/booking_status_view.dart';
import '../modules/common_modules/profile/bindings/profile_binding.dart';
import '../modules/common_modules/profile/views/profile_view.dart';
import '../modules/common_modules/profileGuestMember/bindings/profile_guest_member_binding.dart';
import '../modules/common_modules/profileGuestMember/views/profile_edit_view.dart';
import '../modules/common_modules/profileGuestMember/views/profile_guest_member_view.dart';
import '../modules/common_modules/promotional_offer/bindings/promotional_offer_binding.dart';
import '../modules/common_modules/promotional_offer/views/promotional_offer_view.dart';
import '../modules/common_modules/property_deatails/bindings/property_deatails_binding.dart';
import '../modules/common_modules/property_deatails/views/property_deatails_view.dart';
import '../modules/common_modules/property_selection/bindings/property_selection_binding.dart';
import '../modules/common_modules/property_selection/views/property_selection_view.dart';
import '../modules/common_modules/user_type_selection/bindings/user_type_selection_binding.dart';
import '../modules/common_modules/user_type_selection/views/about_us_view.dart';
import '../modules/common_modules/user_type_selection/views/user_type_selection_two_view.dart';
import '../modules/common_modules/user_type_selection/views/user_type_selection_view.dart';
import '../modules/customer_modules/customer_dashboard/bindings/customer_dashboard_binding.dart';
import '../modules/customer_modules/customer_dashboard/views/customer_dashboard_view.dart';
import '../modules/customer_modules/guest_reservation/bindings/guest_reservation_binding.dart';
import '../modules/customer_modules/guest_reservation/views/guest_reservation_view.dart';
import '../modules/customer_modules/guest_reservation/views/reservation_info_provide_modal_view.dart';
import '../modules/customer_modules/guest_reservation/views/reservation_success_screen_view.dart';
import '../modules/customer_modules/guest_reservation/views/set_customer_info_view.dart';
import '../modules/employee_module/attendance/attendance_application/bindings/attendance_application_binding.dart';
import '../modules/employee_module/attendance/attendance_application/views/attendance_application_view.dart';
import '../modules/employee_module/attendance/attendance_submit/bindings/attendance_submit_binding.dart';
import '../modules/employee_module/attendance/attendance_submit/views/attendance_submit_view.dart';
import '../modules/employee_module/dashboard/bindings/dashboard_binding.dart';
import '../modules/employee_module/dashboard/views/dashboard_view.dart';
import '../modules/employee_module/employee_details/bindings/employee_details_binding.dart';
import '../modules/employee_module/employee_details/views/employee_details_view.dart';
import '../modules/employee_module/employee_list/bindings/employee_list_binding.dart';
import '../modules/employee_module/employee_list/views/employee_list_view.dart';
import '../modules/employee_module/home/bindings/home_binding.dart';
import '../modules/employee_module/home/views/home_view.dart';
import '../modules/employee_module/leave/leave_application/bindings/leave_application_binding.dart';
import '../modules/employee_module/leave/leave_application/views/leave_application_view.dart';
import '../modules/employee_module/leave/leave_approval/bindings/leave_approval_binding.dart';
import '../modules/employee_module/leave/leave_approval/views/leave_approval_view.dart';
import '../modules/employee_module/leave/leave_status/bindings/leave_status_binding.dart';
import '../modules/employee_module/leave/leave_status/views/leave_status_view.dart';
import '../modules/employee_module/voucher/voucher_approval/bindings/voucher_approval_binding.dart';
import '../modules/employee_module/voucher/voucher_approval/views/voucher_approval_view.dart';
import '../modules/employee_module/voucher/voucher_details/bindings/voucher_details_binding.dart';
import '../modules/employee_module/voucher/voucher_details/views/voucher_details_view.dart';
import '../modules/file_picker/bindings/file_picker_binding.dart';
import '../modules/file_picker/views/file_list_view.dart';
import '../modules/file_picker/views/file_picker_view.dart';
import '../modules/login/bindings/login_binding.dart';
import '../modules/login/views/login_view.dart';
import '../modules/member_modules/member_dashboard/bindings/member_dashboard_binding.dart';
import '../modules/member_modules/member_dashboard/views/member_dashboard_view.dart';
import '../modules/member_modules/member_login/bindings/member_login_binding.dart';
import '../modules/member_modules/member_login/views/member_login_view.dart';
import '../modules/member_modules/member_reservation/bindings/member_reservation_binding.dart';
import '../modules/member_modules/member_reservation/views/member_reservation_success_view.dart';
import '../modules/member_modules/member_reservation/views/member_reservation_view.dart';
import '../modules/splash/bindings/splash_binding.dart';
import '../modules/splash/views/splash_view.dart';

part 'app_routes.dart';

class AppPages {
  AppPages._();

  static const initial = Routes.splash;

  static final routes = [
    GetPage(
        name: _Paths.home,
        page: () => const HomeView(),
        binding: HomeBinding(),
        transition: Transition.cupertino,
        middlewares: [AuthMiddleware(), UserModuleMiddleware()]),
    GetPage(
      name: _Paths.splash,
      page: () => SplashView(),
      binding: SplashBinding(),
      transition: Transition.cupertino,
    ),
    GetPage(
        name: _Paths.dashboard,
        page: () => DashboardView(),
        binding: DashboardBinding(),
        transition: Transition.cupertino,
        middlewares: [AuthMiddleware()]),
    GetPage(
        name: _Paths.profile,
        page: () => const ProfileView(),
        binding: ProfileBinding(),
        transition: Transition.cupertino,
        middlewares: [AuthMiddleware()]),
    GetPage(
        name: _Paths.login,
        page: () => LoginView(),
        binding: LoginBinding(),
        transition: Transition.cupertino,
        arguments: String),
    GetPage(
        name: _Paths.attendanceSubmit,
        page: () => AttendanceSubmitView(),
        binding: AttendanceSubmitBinding(),
        transition: Transition.cupertino,
        middlewares: [AuthMiddleware()]),
    GetPage(
        name: _Paths.employeeList,
        page: () => const EmployeeListView(),
        binding: EmployeeListBinding(),
        transition: Transition.cupertino,
        middlewares: [AuthMiddleware()]),
    GetPage(
        name: _Paths.employeeDetails,
        page: () => const EmployeeDetailsView(),
        binding: EmployeeDetailsBinding(),
        transition: Transition.cupertino,
        arguments: EmpModel,
        middlewares: [AuthMiddleware()]),
    GetPage(
        name: _Paths.voucherApproval,
        page: () => const VoucherApprovalView(),
        binding: VoucherApprovalBinding(),
        transition: Transition.cupertino,
        arguments: int
        // children: [
        //   GetPage(
        //     name: _Paths.voucherFilter,
        //     page: () => const VoucherFilterView(),
        //     // binding: VoucherApprovalBinding(),
        //   )
        // ]
        ),
    GetPage(
      name: _Paths.voucherDetails,
      page: () => const VoucherDetailsView(),
      binding: VoucherDetailsBinding(),
      transition: Transition.cupertino,
    ),
    GetPage(
      name: _Paths.attendanceApplication,
      page: () => const AttendanceApplicationView(),
      binding: AttendanceApplicationBinding(),
      transition: Transition.cupertino,
    ),
    GetPage(
      name: _Paths.customerDashboard,
      page: () => const CustomerDashboardView(),
      binding: CustomerDashboardBinding(),
      transition: Transition.cupertino,
    ),
    GetPage(
        name: _Paths.userTypeSelection,
        page: () => const UserTypeSelectionView(),
        binding: UserTypeSelectionBinding(),
        transition: Transition.cupertino,
        children: [
          GetPage(
            name: _Paths.aboutUs,
            page: () => const AboutUsView(),
            transition: Transition.cupertino,
          ),
          GetPage(
            name: _Paths.userTypeSelectionTwo,
            page: () => const UserTypeSelectionTwoView(),
            transition: Transition.cupertino,
          ),
        ]),
    GetPage(
      name: _Paths.leaveStatus,
      page: () => const LeaveStatusView(),
      binding: LeaveStatusBinding(),
    ),
    GetPage(
      name: _Paths.leaveApplication,
      page: () => const LeaveApplicationView(),
      binding: LeaveApplicationBinding(),
    ),
    GetPage(
        name: _Paths.leaveApproval,
        page: () => const LeaveApprovalView(),
        binding: LeaveApprovalBinding(),
        arguments: int),
    GetPage(
        name: _Paths.propertySelection,
        page: () => const PropertySelectionView(),
        binding: PropertySelectionBinding(),
        transition: Transition.cupertino,
        arguments: bool),
    GetPage(
        name: _Paths.memberRegistration,
        page: () => MemberRegistrationView(),
        binding: MemberRegistrationBinding(),
        transition: Transition.cupertino,
        children: [
          GetPage(
            name: _Paths.memberRegistrationChoice,
            page: () => const MemberRegistrationChoiceView(),
            transition: Transition.cupertino,
          ),
          GetPage(
            name: _Paths.payment,
            page: () => const PaymentView(),
            transition: Transition.cupertino,
          ),
          GetPage(
            name: _Paths.successScreen,
            page: () => const SuccessScreenView(),
            transition: Transition.cupertino,
          ),
        ]),
    GetPage(
      name: _Paths.promotionalOffer,
      page: () => const PromotionalOfferView(),
      binding: PromotionalOfferBinding(),
      transition: Transition.cupertino,
    ),
    GetPage(
        name: _Paths.bookingStatus,
        page: () => const BookingStatusView(),
        binding: BookingStatusBinding(),
        transition: Transition.cupertino,
        children: [
          GetPage(
            name: _Paths.bookingDeatailsView,
            page: () => const BookingDeatailsView(),
            transition: Transition.cupertino,
            arguments: ReservationListModel,
          ),
        ]),
    GetPage(
        name: _Paths.memberDashboard,
        page: () => const MemberDashboardView(),
        binding: MemberDashboardBinding(),
        transition: Transition.cupertino,
        middlewares: [GuestOrMemberMiddleware()]),
    GetPage(
        name: _Paths.memberReservation,
        page: () => const MemberReservationView(),
        binding: MemberReservationBinding(),
        transition: Transition.cupertino,
        middlewares: [
          GuestOrMemberMiddleware()
        ],
        children: [
          GetPage(
            name: _Paths.memberReservationSuccessView,
            page: () => const MemberReservationSuccessView(),
            transition: Transition.cupertino,
          ),
        ]),
    GetPage(
        name: _Paths.guestReservation,
        page: () => GuestReservationView(),
        binding: GuestReservationBinding(),
        transition: Transition.cupertino,
        arguments: bool,
        children: [
          GetPage(
            name: _Paths.reservationInfoProvideModal,
            page: () => ReservationInfoProvideModalView(),
            transition: Transition.cupertino,
          ),
          GetPage(
              name: _Paths.setCustomerInfo,
              page: () => const SetCustomerInfoView(),
              transition: Transition.cupertino),
          GetPage(
              name: _Paths.reservationSuccessScreen,
              page: () => const ReservationSuccessScreenView(),
              transition: Transition.cupertino),
        ]),
    GetPage(
        name: _Paths.profileGuestMember,
        page: () => const ProfileGuestMemberView(),
        binding: ProfileGuestMemberBinding(),
        transition: Transition.cupertino,
        children: [
          GetPage(
              name: _Paths.profileEditView,
              page: () => const ProfileEditView(),
              transition: Transition.cupertino),
        ]),
    GetPage(
        name: _Paths.propertyDeatailsView,
        page: () => const PropertyDeatailsView(),
        binding: PropertyDeatailsBinding(),
        transition: Transition.cupertino),
    GetPage(
        name: _Paths.memberLogin,
        page: () => MemberLoginView(),
        binding: MemberLoginBinding(),
        transition: Transition.cupertino),
    GetPage(
        name: _Paths.filePicker,
        page: () => const FilePickerView(),
        binding: FilePickerBinding(),
        transition: Transition.cupertino,
        children: [
          GetPage(
            name: _Paths.fileListView,
            page: () => const FileListView(),
            transition: Transition.cupertino,
          ),
        ]),
  ];
}
