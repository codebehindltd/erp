import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:innboard/app/modules/common_modules/booking_status/views/write_review_view.dart';
import '../../../../core/values/colors.dart';
import '../../../../global_widgets/custom_button.dart';
import '../../../../global_widgets/modal_bottom_sheet.dart';
import '../controllers/booking_status_controller.dart';

class BookingDeatailsView extends GetView<BookingStatusController> {
  const BookingDeatailsView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    //var screenHeight = MediaQuery.of(context).size.height * .48;
    return Scaffold(
        appBar: AppBar(
          title: const Text('Booking Deatails'),
          // centerTitle: true,
        ),
        body: GetBuilder<BookingStatusController>(builder: (_) {
          return SingleChildScrollView(
            child: Padding(
              padding: const EdgeInsets.all(12.0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  AspectRatio(
                    aspectRatio: 16 / 9,
                    child: ClipRRect(
                      borderRadius: BorderRadius.circular(10),
                      child: Image.network(
                        controller.propertyModel?.logoUrl ?? '',
                        loadingBuilder: (context, Widget child,
                            ImageChunkEvent? loadingProgress) {
                          if (loadingProgress == null) {
                            return child;
                          }
                          return Image.asset(
                            "assets/images/emptyimage.png",
                            // color:Colors.grey.shade300.withOpacity(0.6),
                            fit: BoxFit.fitWidth,
                          );
                        },
                        errorBuilder: (context, error, stackTrace) {
                          return Image.asset(
                            "assets/images/emptyimage.png",
                            fit: BoxFit.fitWidth,
                          );
                        },
                      ),
                    ),
                  ),
                  Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          Text(
                            controller.propertyModel?.propertyName ?? "",
                            style: TextStyle(
                                color: themeColor,
                                fontWeight: FontWeight.w500,
                                fontSize: 22),
                          ),
                        ],
                      ),
                      Text(
                          "Reservation No: ${controller.selecteBookingData?.reservationNumber ?? ""}",
                          style: Theme.of(context).textTheme.labelSmall),
                      Text(
                          "Booking Date: ${controller.selecteBookingData?.reservationDateDisplay ?? ""}",
                          style: Theme.of(context).textTheme.labelSmall),
                      Text(
                          "Check In: ${controller.selecteBookingData?.dateInDisplay ?? ""}",
                          style: Theme.of(context).textTheme.labelSmall),
                      Text(
                          "Check Out: ${controller.selecteBookingData?.dateOutDisplay ?? ""}",
                          style: Theme.of(context).textTheme.labelSmall),
                      Text(
                          "Booking Status: ${controller.selecteBookingData?.reservationMode ?? ""}",
                          style: Theme.of(context).textTheme.labelSmall),
                    ],
                  ),
                  // const SizedBox(
                  //   height: 60,
                  // ),
                  // CustomButton(
                  //   submit: (value) {
                  //     ModalBottomSheet.showBottomSheet(
                  //         context, const WriteReviewView(),
                  //         screenHeight: screenHeight);
                  //   },
                  //   name: "Write A Review",
                  //   fullWidth: true,
                  // ),
                ],
              ),
            ),
          );
        }));
  }
}
