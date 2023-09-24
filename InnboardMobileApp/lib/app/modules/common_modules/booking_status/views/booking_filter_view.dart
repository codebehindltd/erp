import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../global_widgets/custom_button.dart';
import '../../../../global_widgets/input_text_field.dart';
import '../controllers/booking_status_controller.dart';

class BookingFilterView extends GetView<BookingStatusController> {
  const BookingFilterView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.only(top: 10),
      child: Column(
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              const SizedBox(),
              Padding(
                padding: const EdgeInsets.only(left: 26),
                child: Text(
                  "Select date",
                  style: Theme.of(context).textTheme.headlineLarge,
                ),
              ),
              GestureDetector(
                onTap: () {
                  Get.back();
                },
                child: const Padding(
                  padding: EdgeInsets.only(right: 7),
                  child: Icon(
                    Icons.close_outlined,
                    color: Colors.black,
                    size: 22,
                  ),
                ),
              ),
            ],
          ),
          const SizedBox(
            height: 10,
          ),
          Container(
            height: 6,
            color: Colors.grey.withOpacity(0.2),
          ),
          Padding(
            padding: const EdgeInsets.all(14.0),
            child: Column(
              children: [
                InputField(
                    controllerText: controller.vFromDateController,
                    labelText: "From Date",
                    isReadOnly: true,
                    validationText: "Please select date",
                    autovalidateMode: AutovalidateMode.onUserInteraction,
                    iconButton: IconButton(
                        onPressed: () {
                          controller.selectFromDate(context);
                        },
                        icon: const Icon(Icons.calendar_month_outlined))),
                const SizedBox(
                  height: 14,
                ),
                InputField(
                    controllerText: controller.vToDateController,
                    labelText: "To Date",
                    isReadOnly: true,
                    validationText: "Please select date",
                    autovalidateMode: AutovalidateMode.onUserInteraction,
                    iconButton: IconButton(
                        onPressed: () async {
                          controller.selectToDate(context);
                        },
                        icon: const Icon(Icons.calendar_month_outlined))),
                const SizedBox(
                  height: 50,
                ),
                Row(
                  mainAxisAlignment: MainAxisAlignment.end,
                  children: [
                    CustomButton(
                      submit: (value) {
                        controller. getDataList();
                        Get.back();
                      },
                      name: "Ok",
                      fullWidth: false,
                      horizontalPadding: 52,
                    ),
                  ],
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
