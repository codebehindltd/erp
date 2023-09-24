import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:leading_edge/app/core/values/colors.dart';
import '../../../../global_widgets/custom_button.dart';
import '../../../../global_widgets/empty_widget.dart';
import '../../../../global_widgets/input_text_field.dart';
import '../../../../global_widgets/modal_bottom_sheet.dart';
import '../../../../routes/app_pages.dart';
import '../widgets/booking_status_card.dart';
import '../controllers/booking_status_controller.dart';
import 'booking_filter_view.dart';

class BookingStatusView extends GetView<BookingStatusController> {
  const BookingStatusView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    var screenHeight = MediaQuery.of(context).size.height * .48;
    return Scaffold(
      appBar: AppBar(
          title: const Text(
            'My Booking Status',
            style: TextStyle(
                color: Colors.white, fontWeight: FontWeight.w500, fontSize: 22),
          ),
          //centerTitle: true,
          automaticallyImplyLeading: false,
          actions: [
            Row(
              children: [
                GestureDetector(
                  onTap: () {
                    // Get.toNamed(Routes.propertySelection, arguments: true);
                    controller.newBooking();
                  },
                  child: Card(
                    color: bodyColor,
                    elevation: 2,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(6.0),
                    ),
                    child: Padding(
                      padding: const EdgeInsets.only(
                          left: 8, right: 8, top: 2, bottom: 2),
                      child: Container(
                        height: 28,
                        color: bodyColor,
                        child: Center(
                          child: Text(
                            "New Booking",
                            style: TextStyle(
                                color: themeColor,
                                fontWeight: FontWeight.w500,
                                fontSize: 12),
                          ),
                        ),
                      ),
                    ),
                  ),
                ),
              ],
            ),
          ]),
      body: GetBuilder<BookingStatusController>(builder: (_) {
        return Obx(() => SingleChildScrollView(
              child: Padding(
                padding: const EdgeInsets.all(12.0),
                child: Column(
                  children: [
                    Row(
                      mainAxisAlignment: MainAxisAlignment.end,
                      children: [
                        Visibility(
                          visible: controller.id != null,
                          child: InkWell(
                            onTap: () {
                              ModalBottomSheet.showBottomSheet(
                                  context, const BookingFilterView(),
                                  screenHeight: screenHeight);
                            },
                            child: Padding(
                              padding: const EdgeInsets.all(4.0),
                              child: Image.asset(
                                "assets/images/filter.png",
                                height: 20,
                                width: 20,
                              ),
                            ),
                          ),
                        ),
                      ],
                    ),
                    const SizedBox(
                      height: 6,
                    ),
                    controller.isLoading.value &&
                            controller.isLoadingForBookingData.value
                        ? SizedBox(
                            height: Get.height - Get.statusBarHeight,
                            child: const Center(
                                child: CircularProgressIndicator()))
                        : controller.id == null
                            ? Form(
                                key: controller.formKeyForGuestInfo,
                                child: buildLogin(context))
                            : controller.id != null &&
                                    controller.isLoading.value == false &&
                                    controller.isLoadingForBookingData.value ==
                                        false &&
                                    controller.dataList.isEmpty
                                ? const SingleChildScrollView(
                                    child: EmptyScreen(
                                        title: "Data Not Found!",
                                        imageUrl: "assets/images/empty.png"),
                                  )
                                : ListView.builder(
                                    physics: const ScrollPhysics(),
                                    shrinkWrap: true,
                                    itemCount: controller.dataList.length,
                                    itemBuilder: (context, index) {
                                      return InkWell(
                                        onTap: () {
controller.selecteBookingData=controller.dataList[index];
                                          Get.toNamed(
                                            Routes.bookingStatus +
                                                Routes.bookingDeatailsView
                                          );
                                        },
                                        child: BookingStatusCard(
                                            model: controller.dataList[index],
                                            logo: controller
                                                .propertyModel?.logoUrl),
                                      );
                                    })
                  ],
                ),
              ),
            ));
      }),
    );
  }

  Widget buildLogin(context) {
    return SingleChildScrollView(
      child: Padding(
        padding: const EdgeInsets.all(20),
        child: Column(
          children: [
            const SizedBox(
              height: 40,
            ),
            Text(
              "Enter Your Infomation",
              style: TextStyle(
                  color: themeColor, fontSize: 24, fontWeight: FontWeight.w700),
            ),
            const SizedBox(
              height: 80,
            ),
            InputField(
              controllerText: controller.gNameController,
              keyboardType: TextInputType.text,
              validationText: "Please enter name",
              autovalidateMode: AutovalidateMode.onUserInteraction,
              labelText: "Name",
            ),
            const SizedBox(
              height: 10,
            ),
            InputField(
              controllerText: controller.gPhoneController,
              keyboardType: TextInputType.number,
              isPhoneNumber: true,
              validationText: "Please enter phone number",
              autovalidateMode: AutovalidateMode.onUserInteraction,
              labelText: "Phone Number",
              maxLength: 11,
            ),
            const SizedBox(
              height: 90,
            ),
            CustomButton(
                submit: controller.guestLoginInfoSubmit,
                name: "Submit",
                fullWidth: true)
          ],
        ),
      ),
    );
  }
}
