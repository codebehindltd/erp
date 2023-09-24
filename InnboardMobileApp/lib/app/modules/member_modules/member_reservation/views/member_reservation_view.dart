import 'package:dropdown_search/dropdown_search.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../core/values/colors.dart';
import '../../../../core/values/strings.dart';
import '../../../../data/models/res/reservation/room_type_info_model.dart';
import '../../../../global_widgets/back_widget.dart';
import '../../../../global_widgets/custom_button.dart';
import '../../../../global_widgets/empty_widget.dart';
import '../../../../global_widgets/input_text_field.dart';
import '../../../../routes/app_pages.dart';
import '../controllers/member_reservation_controller.dart';
import '../widgets/add_item_card.dart';

class MemberReservationView extends GetView<MemberReservationController> {
  const MemberReservationView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    var screenHeight = MediaQuery.of(context).size.height;
    return GetBuilder<MemberReservationController>(builder: (_) {
      return Scaffold(
          appBar: controller.isShowBackbtn
              ? AppBar(
                  automaticallyImplyLeading: false,
                  leading: const BackButtonWidget(),
                  title: const Text('Member Reservation'),
                )
              : AppBar(
                  automaticallyImplyLeading: false,
                  title: const Text('Member Reservation'),
                ),
          body: SingleChildScrollView(
            child: Form(
                key: controller.formKey,
                child: Obx(() {
                  if (MemberReservationController.isLoading.value) {
                    return SizedBox(
                        height: Get.height - Get.statusBarHeight,
                        child:
                            const Center(child: CircularProgressIndicator()));
                  } else {
                    return Container(
                      padding: const EdgeInsets.symmetric(
                          horizontal: 2, vertical: 2),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          AspectRatio(
                            aspectRatio: 16 / 9,
                            child: ClipRRect(
                              borderRadius: BorderRadius.circular(10),
                              child: Image.network(
                                (controller.propertyUrl ?? '') +
                                    (controller.property?.coverPictureURL ??
                                        ''),
                                fit: BoxFit.fitWidth,
                                loadingBuilder: (context, Widget child,
                                    ImageChunkEvent? loadingProgress) {
                                  if (loadingProgress == null) {
                                    return child;
                                  }
                                  return SizedBox(
                                    child: Image.asset(
                                      "assets/images/emptyimage.png",
                                      // color:Colors.grey.shade300.withOpacity(0.6),
                                      fit: BoxFit.fitWidth,
                                    ),
                                  );
                                },
                                errorBuilder: (context, error, stackTrace) {
                                  return SizedBox(
                                    child: Image.asset(
                                      "assets/images/emptyimage.png",
                                      fit: BoxFit.fitWidth,
                                    ),
                                  );
                                },
                              ),
                            ),
                          ),
                          Container(
                            padding: const EdgeInsets.symmetric(
                                horizontal: 10, vertical: 10),
                            child: Column(
                              children: [
                                const SizedBox(
                                  height: 20,
                                ),
                                Row(
                                  mainAxisAlignment:
                                      MainAxisAlignment.spaceBetween,
                                  children: [
                                    Text(
                                        controller.property?.propertyName ?? "",
                                        style: Theme.of(context)
                                            .textTheme
                                            .labelLarge),
                                    GestureDetector(
                                      onTap: () {
                                        Get.toNamed(
                                            Routes.propertyDeatailsView);
                                      },
                                      child: Padding(
                                        padding: const EdgeInsets.only(
                                            top: 2, bottom: 12),
                                        child: Container(
                                          padding: const EdgeInsets.symmetric(
                                              horizontal: 12, vertical: 4),
                                          //height: 28,
                                          decoration: BoxDecoration(
                                            border:
                                                Border.all(color: themeColor),
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
                                    controllerText:
                                        controller.vFromDateController,
                                    labelText: "From Date",
                                    isReadOnly: true,
                                    validationText: "Please select date",
                                    autovalidateMode:
                                        AutovalidateMode.onUserInteraction,
                                    iconButton: IconButton(
                                        onPressed: () {
                                          controller.selectFromDate(context);
                                        },
                                        icon: const Icon(
                                            Icons.calendar_month_outlined))),
                                const SizedBox(
                                  height: 12,
                                ),
                                InputField(
                                    controllerText:
                                        controller.vToDateController,
                                    labelText: "To Date",
                                    isReadOnly: true,
                                    validationText: "Please select date",
                                    autovalidateMode:
                                        AutovalidateMode.onUserInteraction,
                                    iconButton: IconButton(
                                        onPressed: () async {
                                          controller.selectToDate(context);
                                        },
                                        icon: const Icon(
                                            Icons.calendar_month_outlined))),
                                const SizedBox(
                                  height: 12,
                                ),

                                buildRoomTypeDropdownMenu(
                                    controller.roomTypeInfo,
                                    context: context,
                                    screenHeight: screenHeight),
                                const SizedBox(
                                  height: 12,
                                ),
                                // Text("Added Items",
                                //     style: Theme.of(context).textTheme.headlineLarge),
                                Stack(
                                  children: [
                                    Container(
                                      height: 296,
                                      width: double.infinity,
                                      decoration: BoxDecoration(
                                          borderRadius:
                                              BorderRadius.circular(10),
                                          border:
                                              Border.all(color: gray1Color)),
                                      margin: const EdgeInsets.only(top: 12),
                                      padding: const EdgeInsets.only(top: 8),
                                      child: controller
                                              .reservationDetailsItems.isEmpty
                                          ? const EmptyScreen(
                                              title: "Data Not Found!",
                                              topHeight: 12,
                                              height: 145,
                                              imageUrl:
                                                  "assets/images/empty.png")
                                          : ListView.separated(
                                              physics: const ScrollPhysics(),
                                              shrinkWrap: true,
                                              scrollDirection: Axis.horizontal,
                                              itemCount: controller
                                                  .reservationDetailsItems
                                                  .length,
                                              separatorBuilder:
                                                  (context, index) =>
                                                      const SizedBox(
                                                        width: 1,
                                                      ),
                                              itemBuilder: (context, index) {
                                                return Stack(
                                                  children: [
                                                    AddItemCard(
                                                      reservationDetailsItems:
                                                          controller
                                                                  .reservationDetailsItems[
                                                              index],
                                                    ),
                                                    Positioned(
                                                      right: 3,
                                                      top: 3,
                                                      child: InkWell(
                                                        onTap: () {
                                                          controller.removeItem(
                                                              index);
                                                        },
                                                        child: Card(
                                                          color: const Color
                                                                  .fromARGB(255,
                                                              253, 247, 247),
                                                          shape:
                                                              RoundedRectangleBorder(
                                                                  borderRadius:
                                                                      BorderRadius
                                                                          .circular(
                                                                              50),
                                                                  side:
                                                                      const BorderSide(
                                                                    color:
                                                                        errorColor,
                                                                    width: 1,
                                                                  )),
                                                          child: const SizedBox(
                                                            height: 24,
                                                            width: 24,
                                                            child: Center(
                                                                child: Icon(
                                                              Icons
                                                                  .close_outlined,
                                                              color: Color(
                                                                  0xffE42525),
                                                              size: 16,
                                                            )),
                                                          ),
                                                        ),
                                                      ),
                                                    )
                                                  ],
                                                );
                                              }),
                                    ),
                                    Positioned(
                                      top: -3,
                                      left: 15,
                                      child: Container(
                                        color: bodyColor,
                                        child: Text(
                                            "Added Items (${controller.reservationDetailsItems.length})",
                                            style: Theme.of(context)
                                                .textTheme
                                                .headlineLarge!
                                                .merge(const TextStyle(
                                                    color: Colors.black45))),
                                      ),
                                    ),
                                  ],
                                ),
                                Padding(
                                  padding:
                                      const EdgeInsets.symmetric(vertical: 8.0),
                                  child: InputField(
                                    controllerText:
                                        controller.reservationNoteController,
                                    labelText: "Reservation Note",
                                    //hintText: "Note",
                                    maxLines: 3,
                                    maxLength: 200,
                                  ),
                                ),
                                Visibility(
                                  visible: controller.totalAmount.value > 0,
                                  child: Card(
                                      elevation: .5,
                                      color: bodyColor.withOpacity(.9),
                                      child: Padding(
                                        padding: const EdgeInsets.all(8.0),
                                        child: Row(
                                          mainAxisAlignment:
                                              MainAxisAlignment.spaceEvenly,
                                          children: [
                                            Text("Total: ",
                                                style: Theme.of(context)
                                                    .textTheme
                                                    .labelLarge),
                                            Text(
                                                "${controller.totalAmount.value.toStringAsFixed(2)} $currency",
                                                style: Theme.of(context)
                                                    .textTheme
                                                    .labelLarge),
                                          ],
                                        ),
                                      )),
                                ),
                                const SizedBox(
                                  height: 40,
                                ),
                                CustomButton(
                                    submit: controller.next,
                                    // submit: (value) {},
                                    name: "Continue",
                                    fullWidth: true)
                              ],
                            ),
                          )
                        ],
                      ),
                    );
                  }
                })),
          ));
    });
  }

  DropdownSearch<RoomTypeInfoModel> buildRoomTypeDropdownMenu(
      List<RoomTypeInfoModel> list,
      {context,
      screenHeight}) {
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
          controller.selectRoomType(value,
              context: context, screenHeight: screenHeight);
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
