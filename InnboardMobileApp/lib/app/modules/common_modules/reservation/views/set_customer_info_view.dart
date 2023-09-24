import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:leading_edge/app/core/values/colors.dart';
import '../../../../global_widgets/back_widget.dart';
import '../../../../global_widgets/custom_button.dart';
import '../../../../global_widgets/input_text_field.dart';
import '../controllers/reservation_controller.dart';

class SetCustomerInfoView extends GetView<ReservationController> {
  const SetCustomerInfoView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        //title: const Text('Enter Your Infomation'),
        centerTitle: true,
        leading: const BackButtonWidget(),
 
      ),
      body: SingleChildScrollView(
        child: Container(
          padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 18),
          child: Form(
            key: controller.formKeyForCustomerInfo,
            child: Column(
              children: [
                const SizedBox(
                  height: 40,
                ),
                Text(
                  "Enter Your Infomation",
                  style: TextStyle(
                      color: themeColor,
                      fontSize: 24,
                      fontWeight: FontWeight.w700),
                ),
                const SizedBox(
                  height: 80,
                ),
                InputField(
                  controllerText: controller.nameController,
                  keyboardType: TextInputType.text,
                  validationText: "Please enter name",
                  autovalidateMode: AutovalidateMode.onUserInteraction,
                  labelText: "Name",
                ),
                const SizedBox(
                  height: 10,
                ),
                InputField(
                  controllerText: controller.phoneController,
                  keyboardType: TextInputType.number,
                  isPhoneNumber: true,
                  validationText: "Please enter phone number",
                  autovalidateMode: AutovalidateMode.onUserInteraction,
                  labelText: "Phone Number",
                  maxLength: 11,
                ),
                const SizedBox(
                  height: 110,
                ),
                CustomButton(
                    submit:  controller.saveRoomReservationInfo,
                    name: "Continue",
                    fullWidth: true)
              ],
            ),
          ),
        ),
      ),
    );
  }
}
