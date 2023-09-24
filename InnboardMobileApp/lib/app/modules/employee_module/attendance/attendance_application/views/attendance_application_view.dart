import 'package:flutter/material.dart';

import 'package:get/get.dart';

import '../../../../../core/utils/utils_function.dart';
import '../../../../../global_widgets/back_widget.dart';
import '../../../../../global_widgets/custom_button.dart';
import '../../../../../global_widgets/input_text_field.dart';
import '../controllers/attendance_application_controller.dart';

class AttendanceApplicationView
    extends GetView<AttendanceApplicationController> {
  const AttendanceApplicationView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        leading: const BackButtonWidget(),
        title: const Text("Attendance Application",
            style: TextStyle(color: Colors.white)),
        elevation: 0,
      ),
      body: GetBuilder<AttendanceApplicationController>(builder: (data) {
        return SingleChildScrollView(
          child: Container(
            padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 25),
            child: Form(
              key: controller.formKey,
              child: Column(
                children: [
                  InputField(
                      controllerText: controller.vDateController,
                      labelText: "Date",
                      isReadOnly: true,
                      validationText: "Please select attendance date",
                      iconButton: IconButton(
                          onPressed: () async {
                            DateTime? datePick = await showDatePicker(
                                useRootNavigator: false,
                                context: context,
                                initialDate:
                                    controller.selectDate ?? DateTime.now().subtract(const Duration(days: 1)),
                                firstDate: DateTime(2000),
                                lastDate:
                                    DateTime.now().subtract(const Duration(days: 1)));
                            if (datePick != null) {
                              controller.vDateController.text =
                                  dateFormat.format(datePick);
                              controller.selectDate = datePick;
                            } else {
                              controller.vDateController.text = "";
                              controller.selectDate = null;
                            }
                          },
                          icon: const Icon(Icons.calendar_month_outlined))),
                  const SizedBox(
                    height: 12,
                  ),
                  InputField(
                      controllerText: controller.entryTimeController,
                      labelText: "Entry Time",
                      isReadOnly: true,
                      // validationText: "Please select entry time",
                      iconButton: IconButton(
                          onPressed: () async {
                            var timePick = await showTimePicker(
                                useRootNavigator: false,
                                context: context,
                                initialTime: TimeOfDay.now(),
                                initialEntryMode: TimePickerEntryMode.dial);
                            if (timePick != null) {
                              controller.entryTimeController.text =
                                  timePick.format(context);
                              controller.entryTimePick = timePick;
                            } else {
                              controller.entryTimeController.text = "";
                              controller.entryTimePick = null;
                            }
                          },
                          icon: const Icon(Icons.schedule_outlined))),
                  const SizedBox(
                    height: 12,
                  ),
                  InputField(
                      controllerText: controller.exitTimeController,
                      labelText: "Exit Time",
                      isReadOnly: true,
                      // validationText: "Please select exit time",
                      iconButton: IconButton(
                          onPressed: () async {
                            var timePick = await showTimePicker(
                                useRootNavigator: false,
                                context: context,
                                initialTime: TimeOfDay.now(),
                                initialEntryMode: TimePickerEntryMode.dial);
                            if (timePick != null) {
                              controller.exitTimeController.text =
                                  timePick.format(context);
                              controller.exitTimePick = timePick;
                            } else {
                              controller.exitTimeController.text = "";
                              controller.exitTimePick = null;
                            }
                          },
                          icon: const Icon(Icons.schedule_outlined))),
                  const SizedBox(
                    height: 12,
                  ),
                  InputField(
                    controllerText: controller.remarksController,
                    keyboardType: TextInputType.text,
                    labelText: "Remarks",
                    validationText: "Please provide remarks",
                    maxLines: 5,
                  ),
                  const SizedBox(
                    height: 20,
                  ),
                  CustomButton(
                      submit: controller.submit,
                      name: "Apply",
                      fullWidth: false)
                ],
              ),
            ),
          ),
        );
      }),
    );
  }
}
