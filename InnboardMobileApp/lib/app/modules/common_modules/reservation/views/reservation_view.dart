import 'package:dropdown_search/dropdown_search.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:leading_edge/app/core/values/colors.dart';
import 'package:leading_edge/app/global_widgets/custom_button.dart';
import '../../../../data/models/res/reservation/room_type_info_model.dart';
import '../../../../global_widgets/input_text_field.dart';
import '../controllers/reservation_controller.dart';

class ReservationView extends GetView<ReservationController> {
  const ReservationView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: const Text(''),
          centerTitle: true,
        ),
        body: SingleChildScrollView(
          child: Container(
              padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 18),
              child: Form(
                key: controller.formKey,
                child: Column(
                  children: [
                    Obx(() {
                      if (ReservationController.isLoading.value) {
                        return const Center(child: CircularProgressIndicator());
                      } else {
                        return Column(
                          children: [
                            AspectRatio(
                              aspectRatio: 16 / 9,
                              child: Image.asset(
                                "assets/images/reservation.png",
                                //height: 120,
                                //width: Get.width,
                                fit: BoxFit.fitWidth,
                              ),
                            ),
                            const SizedBox(
                              height: 20,
                            ),
                            Row(
                              mainAxisAlignment: MainAxisAlignment.end,
                              children: [
                                GestureDetector(
                                  onTap: () {},
                                  child: Padding(
                                    padding: const EdgeInsets.only(
                                        top: 2, bottom: 12),
                                    child: Container(
                                      padding: const EdgeInsets.symmetric(
                                          horizontal: 12, vertical: 4),
                                      //height: 28,
                                      decoration: BoxDecoration(
                                        border: Border.all(color: themeColor),
                                        borderRadius:
                                            BorderRadius.circular(6.0),
                                      ),
                                      child: Center(
                                        child: Text(
                                          "Details",
                                          style: TextStyle(
                                              color: themeColor,
                                              fontWeight: FontWeight.w600,
                                              fontSize: 16),
                                        ),
                                      ),
                                    ),
                                  ),
                                ),
                              ],
                            ),
                            InputField(
                                controllerText: controller.vToDateController,
                                labelText: "From Date",
                                isReadOnly: true,
                                validationText: "Please select date",
                                autovalidateMode:
                                    AutovalidateMode.onUserInteraction,
                                iconButton: IconButton(
                                    onPressed: () async {
                                      DateTime? datePick = await showDatePicker(
                                          useRootNavigator: false,
                                          context: context,
                                          initialDate: controller.fromDate ??
                                              controller.toDate ??
                                              DateTime.now(),
                                          firstDate: DateTime.now(),
                                          lastDate: controller.toDate ??
                                              DateTime(2024));
                                      controller.selectFromDate(datePick);
                                    },
                                    icon: const Icon(
                                        Icons.calendar_month_outlined))),
                            const SizedBox(
                              height: 12,
                            ),
                            InputField(
                                controllerText: controller.vFromDateController,
                                labelText: "To Date",
                                isReadOnly: true,
                                validationText: "Please select date",
                                autovalidateMode:
                                    AutovalidateMode.onUserInteraction,
                                iconButton: IconButton(
                                    onPressed: () async {
                                      DateTime? datePick = await showDatePicker(
                                          useRootNavigator: false,
                                          context: context,
                                          initialDate: controller.toDate ??
                                              controller.fromDate ??
                                              DateTime.now(),
                                          firstDate: controller.fromDate ??
                                              DateTime.now(),
                                          lastDate: DateTime(2024));
                                      controller.selectToDate(datePick);
                                    },
                                    icon: const Icon(
                                        Icons.calendar_month_outlined))),
                            const SizedBox(
                              height: 12,
                            ),
                            buildRoomTypeDropdownMenu(controller.roomTypeInfo),
                            const SizedBox(
                              height: 12,
                            ),
                            Flex(
                              direction: Axis.horizontal,
                              children: [
                                Flexible(
                                  flex: 1,
                                  child: Padding(
                                    padding:
                                        EdgeInsets.symmetric(vertical: 8.0),
                                    child: InputField(
                                      controllerText: controller.paxController,
                                      labelText: "Pax",
                                      isReadOnly: true,
                                    ),
                                  ),
                                ),
                                const SizedBox(
                                  width: 5,
                                ),
                                Flexible(
                                  flex: 1,
                                  child: Padding(
                                    padding:
                                        EdgeInsets.symmetric(vertical: 8.0),
                                    child: InputField(
                                      controllerText:
                                          controller.childController,
                                      labelText: "Child",
                                      isReadOnly: true,
                                    ),
                                  ),
                                ),
                              ],
                            ),
                            const SizedBox(
                              height: 12,
                            ),
                             Padding(
                              padding: const EdgeInsets.symmetric(vertical: 8.0),
                              child: InputField(
                                 controllerText: controller.extraBedController,
                                labelText: "Extra Bed",
                              ),
                            ),
                             Padding(
                              padding: EdgeInsets.symmetric(vertical: 8.0),
                              child: InputField(
                                 controllerText: controller.noteController,
                                labelText: "Note",
                                //hintText: "Note",
                                maxLines: 3,
                              ),
                            ),
                            Card(
                                elevation: .5,
                                color: bodyColor.withOpacity(.9),
                                child: Padding(
                                  padding: EdgeInsets.all(8.0),
                                  child: Row(
                                    mainAxisAlignment:
                                        MainAxisAlignment.spaceEvenly,
                                    children: [
                                      Text("Total: ",
                                          style: Theme.of(context)
                                              .textTheme
                                              .labelLarge),
                                      Text(
                                          "${controller.selectedType.value?.roomRate ?? "0"} ${controller.selectedType.value?.localCurrencyHead ?? "BDT"}",
                                          style: Theme.of(context)
                                              .textTheme
                                              .labelLarge),
                                    ],
                                  ),
                                )),
                            const SizedBox(
                              height: 20,
                            ),
                            CustomButton(
                                submit: controller.checkAvailableRoomInfo,
                                name: "Next",
                                fullWidth: true)
                          ],
                        );
                      }
                    })
                  ],
                ),
              )),
        ));
  }

  DropdownSearch<RoomTypeInfoModel> buildRoomTypeDropdownMenu(
      List<RoomTypeInfoModel> list) {
    return DropdownSearch<RoomTypeInfoModel>(
        //compareFn: false,
        popupProps: PopupPropsMultiSelection.menu(
          showSelectedItems: false,
          itemBuilder: _customPopupItemBuilder,
          // showSearchBox: true,
        ),
        dropdownDecoratorProps: DropDownDecoratorProps(
          baseStyle: TextStyle(
              color: themeColor, fontWeight: FontWeight.w400, fontSize: 16),
          dropdownSearchDecoration: const InputDecoration(
              contentPadding: EdgeInsets.symmetric(vertical: 6, horizontal: 12),
              labelText: "Room Type",
              enabledBorder: OutlineInputBorder(
                borderSide: BorderSide(width: 1, color: gray1Color),
                borderRadius: BorderRadius.all(Radius.circular(6)),
              ),
              border: OutlineInputBorder()),
        ),
        itemAsString: (item) => item.roomType ?? '',
        items: list,
        onChanged: (value) {
          controller.selectRoomType(value);
        },
        enabled: list.isNotEmpty ? true : false,
        selectedItem: controller.selectedType.value,
        autoValidateMode: AutovalidateMode.onUserInteraction,
        validator: (value) {
          print(value);
          if (value == null) {
            return "Please select room type";
          } else {
            return null;
          }
        });
  }

  Widget _customPopupItemBuilder(
      BuildContext context, RoomTypeInfoModel item, bool isSelected) {
    return Container(
      margin: const EdgeInsets.symmetric(horizontal: 8),
      decoration: !isSelected
          ? null
          : BoxDecoration(
              border: Border.all(color: Theme.of(context).primaryColor),
              borderRadius: BorderRadius.circular(5),
              color: Colors.white,
            ),
      child: ListTile(
        selected: isSelected,
        title: Text(item.roomType.toString(),
            style: TextStyle(
                color: themeColor, fontWeight: FontWeight.w400, fontSize: 16)),
      ),
    );
  }
}
