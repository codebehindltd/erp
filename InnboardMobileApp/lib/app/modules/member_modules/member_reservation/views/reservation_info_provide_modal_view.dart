import 'package:dropdown_search/dropdown_search.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../core/values/colors.dart';
import '../../../../core/values/strings.dart';
import '../../../../global_widgets/custom_button.dart';
import '../../../../global_widgets/input_text_field.dart';
import '../controllers/member_reservation_controller.dart';

class ReservationInfoProvideModalView
    extends GetView<MemberReservationController> {
  const ReservationInfoProvideModalView({Key? key}) : super(key: key);
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
                    "${controller.selectedType.value?.roomType.toString()}",
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
            Expanded(
              child: Form(
                key: controller.itemForm,
                child: Padding(
                  padding: const EdgeInsets.only(left: 8, right: 8),
                  child: ListView(
                    children: [
                      Flex(
                        direction: Axis.horizontal,
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Flexible(
                            flex: 1,
                            child: Padding(
                              padding:
                                  const EdgeInsets.symmetric(vertical: 8.0),
                              // child: InputField(
                              //   controllerText: controller.paxController,
                              //   labelText: "Pax",
                              //   isReadOnly: true,
                              // ),
                              child: buildPaxDropdownMenu(controller.paxList),
                            ),
                          ),
                          const SizedBox(
                            width: 5,
                          ),
                          Flexible(
                            flex: 1,
                            child: Padding(
                              padding:
                                  const EdgeInsets.symmetric(vertical: 8.0),
                              child:
                                  buildChildDropdownMenu(controller.childList),
                            ),
                          ),
                        ],
                      ),
                      Flex(
                        direction: Axis.horizontal,
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Flexible(
                            flex: 1,
                            child: Padding(
                              padding:
                                  const EdgeInsets.symmetric(vertical: 8.0),
                              child: InputField(
                                  controllerText: controller.qtyController,
                                  labelText: "Room Quantity",
                                  keyboardType: TextInputType.number,
                                  isDigitOnly: true,
                                  validationText: "Enter room quantity",
                                  onchange: controller.qtyOnChange),
                            ),
                          ),
                          const SizedBox(
                            width: 5,
                          ),
                          Flexible(
                            flex: 1,
                            child: Padding(
                              padding:
                                  const EdgeInsets.symmetric(vertical: 8.0),
                              child: InputField(
                                  controllerText: controller.extraBedController,
                                  labelText: "Extra Bed",
                                  keyboardType: TextInputType.number,
                                  isDigitOnly: true,
                                  onchange: controller.extraRmQtyOnChange),
                            ),
                          ),
                        ],
                      ),
                      Padding(
                        padding: EdgeInsets.symmetric(vertical: 8.0),
                        child: InputField(
                          controllerText: controller.noteController,
                          labelText: "Note",
                          //hintText: "Note",
                          maxLines: 3,
                          maxLength: 200,
                        ),
                      ),
                      Obx(() => Visibility(
                            visible: controller.totalItemAmount.value > 0,
                            child: Card(
                                elevation: .5,
                                color: bodyColor.withOpacity(.9),
                                child: Padding(
                                    padding: const EdgeInsets.all(12.0),
                                    child: Column(
                                      crossAxisAlignment:
                                          CrossAxisAlignment.start,
                                      children: [
                                        Row(
                                          mainAxisAlignment:
                                              MainAxisAlignment.spaceBetween,
                                          children: [
                                            Text("Total Room Rate: ",
                                                style: Theme.of(context)
                                                    .textTheme
                                                    .labelLarge),
                                            Obx(() => Text(
                                                "${controller.itemTotal.value.toStringAsFixed(2)} ${controller.selectedType.value?.localCurrencyHead ?? currency}",
                                                style: Theme.of(context)
                                                    .textTheme
                                                    .labelLarge)),
                                          ],
                                        ),
                                        Row(
                                          mainAxisAlignment:
                                              MainAxisAlignment.spaceBetween,
                                          children: [
                                            Text("Discount : ",
                                                style: Theme.of(context)
                                                    .textTheme
                                                    .labelLarge),
                                            Obx(() => Text(
                                                "${controller.selectedType.value?.roomDiscountPercent?.toStringAsFixed(2)}%",
                                                style: Theme.of(context)
                                                    .textTheme
                                                    .labelLarge)),
                                          ],
                                        ),
                                        Row(
                                          mainAxisAlignment:
                                              MainAxisAlignment.spaceBetween,
                                          children: [
                                            Text("Discount Amount : ",
                                                style: Theme.of(context)
                                                    .textTheme
                                                    .labelLarge),
                                            Obx(() => Text(
                                                "${controller.itemDiscountAmount.toStringAsFixed(2)} ${controller.selectedType.value?.localCurrencyHead ?? currency}",
                                                style: Theme.of(context)
                                                    .textTheme
                                                    .labelLarge)),
                                          ],
                                        ),
                                        Row(
                                          mainAxisAlignment:
                                              MainAxisAlignment.spaceBetween,
                                          children: [
                                            Text("Discounted Amount : ",
                                                style: Theme.of(context)
                                                    .textTheme
                                                    .labelLarge),
                                            Obx(() => Text(
                                                "${(controller.itemTotal.value - controller.itemDiscountAmount.value).toStringAsFixed(2)} ${controller.selectedType.value?.localCurrencyHead ?? currency}",
                                                style: Theme.of(context)
                                                    .textTheme
                                                    .labelLarge)),
                                          ],
                                        ),
                                        Visibility(
                                          visible: controller
                                                  .itemExtaBedAmount.value >
                                              0,
                                          child: Row(
                                            mainAxisAlignment:
                                                MainAxisAlignment.spaceBetween,
                                            children: [
                                              Text("Extra Bed Amount : ",
                                                  style: Theme.of(context)
                                                      .textTheme
                                                      .labelLarge),
                                              Obx(() => Text(
                                                  "${controller.itemExtaBedAmount.toStringAsFixed(2)} ${controller.selectedType.value?.localCurrencyHead ?? currency}",
                                                  style: Theme.of(context)
                                                      .textTheme
                                                      .labelLarge)),
                                            ],
                                          ),
                                        ),
                                        Row(
                                          mainAxisAlignment:
                                              MainAxisAlignment.spaceBetween,
                                          children: [
                                            Text("Grand Total : ",
                                                style: Theme.of(context)
                                                    .textTheme
                                                    .labelLarge),
                                            Obx(() => Text(
                                                "${controller.totalItemAmount.value.toStringAsFixed(2)} ${controller.selectedType.value?.localCurrencyHead ?? currency}",
                                                style: Theme.of(context)
                                                    .textTheme
                                                    .labelLarge)),
                                          ],
                                        ),
                                      ],
                                    ))),
                          )),
                      const SizedBox(
                        height: 10,
                      ),
                      Text(
                        "*Note: Your available room night ${controller.selectedType.value?.availableRoomNight}, you can take maximum ${controller.selectedType.value?.maximumNightAvailPerReservation} nights per reservation",
                        style: const TextStyle(
                          color: gray1Color,
                        ),
                      ),
                      const SizedBox(
                        height: 30,
                      ),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.end,
                        children: [
                          CustomButton(
                            submit: controller.addAndcheckAvailableRoomInfo,
                            name: "Add",
                            fullWidth: false,
                            horizontalPadding: 42,
                          ),
                        ],
                      ),
                      const SizedBox(
                        height: 30,
                      ),
                    ],
                  ),
                ),
              ),
            ),
          ],
        ));
  }

  DropdownSearch<int> buildPaxDropdownMenu(List<int> list) {
    print("call");
    return DropdownSearch<int>(
        //compareFn: false,
        // popupProps: PopupPropsMultiSelection.menu(
        //   showSelectedItems: false,
        //   itemBuilder: item,
        //   // showSearchBox: true,
        // ),
        dropdownDecoratorProps: DropDownDecoratorProps(
          baseStyle: TextStyle(
              color: themeColor, fontWeight: FontWeight.w400, fontSize: 16),
          dropdownSearchDecoration: const InputDecoration(
              contentPadding: EdgeInsets.symmetric(vertical: 6, horizontal: 12),
              labelText: "Pax (Per Room)",
              enabledBorder: OutlineInputBorder(
                borderSide: BorderSide(width: 1, color: gray1Color),
                borderRadius: BorderRadius.all(Radius.circular(6)),
              ),
              border: OutlineInputBorder()),
        ),
        itemAsString: (item) => item.toString(),
        items: list,
        onChanged: (value) {
          controller.selectPax(value);
        },
        enabled: list.isNotEmpty ? true : false,
        selectedItem: controller.selectedPax.value,
        autoValidateMode: AutovalidateMode.onUserInteraction,
        validator: (value) {
          print(value);
          if (value == null) {
            return "Please select pax";
          } else {
            return null;
          }
        });
  }

  DropdownSearch<int> buildChildDropdownMenu(List<int> list) {
    return DropdownSearch<int>(
        //compareFn: false,
        // popupProps: PopupPropsMultiSelection.menu(
        //   showSelectedItems: false,
        //   itemBuilder: item,
        //   // showSearchBox: true,
        // ),
        dropdownDecoratorProps: DropDownDecoratorProps(
          baseStyle: TextStyle(
              color: themeColor, fontWeight: FontWeight.w400, fontSize: 16),
          dropdownSearchDecoration: const InputDecoration(
              contentPadding: EdgeInsets.symmetric(vertical: 6, horizontal: 12),
              labelText: "Child (Per Room)",
              enabledBorder: OutlineInputBorder(
                borderSide: BorderSide(width: 1, color: gray1Color),
                borderRadius: BorderRadius.all(Radius.circular(6)),
              ),
              border: OutlineInputBorder()),
        ),
        itemAsString: (item) => item.toString(),
        items: list,
        onChanged: (value) {
          controller.selectChild(value);
        },
        enabled: list.isNotEmpty ? true : false,
        selectedItem: controller.selectedChild.value,
        autoValidateMode: AutovalidateMode.onUserInteraction,
        validator: (value) {
          print(value);
          if (value == null) {
            return "Please select child";
          } else {
            return null;
          }
        });
  }
}
