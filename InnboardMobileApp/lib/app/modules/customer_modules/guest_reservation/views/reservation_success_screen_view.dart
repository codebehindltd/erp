import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../core/values/colors.dart';
import '../../../../global_widgets/custom_button.dart';
import '../../../../routes/app_pages.dart';
import '../controllers/guest_reservation_controller.dart';
class ReservationSuccessScreenView extends GetView<GuestReservationController> {
  const ReservationSuccessScreenView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Padding(
        padding: const EdgeInsets.all(30.0),
        child: Column(
          children: [
            const SizedBox(
              height: 80,
            ),
            Container(
              height: 150,
              width: 150,
              padding: const EdgeInsets.all(8),
              decoration: BoxDecoration(
                borderRadius: BorderRadius.circular(100),
                gradient: LinearGradient(colors: [
                  const Color.fromARGB(255, 165, 191, 230),
                  const Color.fromARGB(255, 165, 191, 230).withOpacity(.5),
                  whiteColor,
                  whiteColor,
                ], begin: Alignment.topLeft, end: Alignment.bottomRight),
              ),
              child: Container(
                decoration: BoxDecoration(
                    // color: bodyColor,
                    borderRadius: BorderRadius.circular(100),
                    boxShadow: [
                      const BoxShadow(
                          color: Color.fromARGB(255, 165, 191, 230),
                          blurRadius: 8,
                          offset: Offset(-4, -4)),
                      BoxShadow(
                          color: bodyColor,
                          spreadRadius: 2,
                          blurRadius: 8,
                          offset: const Offset(4, 4)),
                    ]),
                child: Icon(
                  Icons.check_rounded,
                  color: themeColor,
                  size: 100,
                  grade: 500,
                ),
              ),
            ),
            const SizedBox(
              height: 50,
            ),
            Text(
              'Reservation Succussfully Done',
              style: TextStyle(
                  color: themeColor, fontWeight: FontWeight.w500, fontSize: 22),
            ),
            const SizedBox(
              height: 12,
            ),
            Text(
              // 'Please wait until your \n registration is approved, we will take \n maximum 72 hours to approve',
              "",
              textAlign: TextAlign.center,
              style: TextStyle(
                  color: themeColor, fontWeight: FontWeight.w400, fontSize: 18),
            ),
            const SizedBox(
              height: 90,
            ),
            CustomButton(
                submit: (value) {
                  Get.offAllNamed(Routes.customerDashboard);
                },
                name: "Continue",
                fullWidth: true,
                horizontalPadding: 52,
                fontSize: 24),
          ],
        ),
      ),
    );
  }
}
