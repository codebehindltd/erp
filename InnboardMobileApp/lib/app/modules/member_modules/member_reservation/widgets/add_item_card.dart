import 'package:flutter/material.dart';
import '../../../../core/values/colors.dart';
import '../../../../core/values/strings.dart';
import '../../../../data/models/req/reservation/hotel_room_reservation_details.dart';

class AddItemCard extends StatelessWidget {
  final RoomReservationDetail? reservationDetailsItems;
  AddItemCard({super.key, this.reservationDetailsItems});

  @override
  Widget build(BuildContext context) {
    return Container(
        width: 300,
        //padding: const EdgeInsets.all(12),
        margin: const EdgeInsets.all(15),
        // decoration: BoxDecoration(
        //   border:
        //       Border.all(color: borderColor),
        //   borderRadius:
        //       BorderRadius.circular(10),
        // ),
        decoration: BoxDecoration(
            color: bodyColor,
            borderRadius: BorderRadius.circular(8),
            boxShadow: [
              const BoxShadow(
                color: whiteColor,
                spreadRadius: 2,
                blurRadius: 6,
                offset: Offset(-5, -5),
              ),
              BoxShadow(
                color: bottonShadowColor.withOpacity(.5),
                spreadRadius: 1,
                blurRadius: 8,
                offset: const Offset(4, 4),
              )
            ]),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Padding(
                  padding: const EdgeInsets.all(4.0),
                  child: Text(reservationDetailsItems!.roomType.toString(),
                      style: Theme.of(context).textTheme.headlineLarge),
                ),
              ],
            ),
            Divider(
              height: 3,
              //  width: double.infinity,
              color: Colors.grey.shade500,
            ),
            const SizedBox(
              height: 6,
            ),
            Padding(
              padding: const EdgeInsets.all(10.0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      Text(
                          "Room quantity : ${reservationDetailsItems!.roomQuantity}",
                          style: Theme.of(context).textTheme.headlineMedium),
                      Text(
                          "Extra bed : ${reservationDetailsItems!.extraBedQuantity}",
                          style: Theme.of(context).textTheme.headlineMedium),
                    ],
                  ),
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      Text(
                          "Pax : ${reservationDetailsItems!.paxQuantity ?? ""} ",
                          style: Theme.of(context).textTheme.headlineMedium),
                      Text("Child : ${reservationDetailsItems!.childQuantity}",
                          style: Theme.of(context).textTheme.headlineMedium),
                    ],
                  ),
                  Visibility(
                    visible: reservationDetailsItems!.totalPrice!>0,
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                            "Discount: ${reservationDetailsItems!.discountPercent ?? ""}% ",
                            style: Theme.of(context).textTheme.headlineMedium),
                        Text(
                            "Discount Amount : ${reservationDetailsItems!.discountAmount?.toStringAsFixed(2) ?? ""} ",
                            style: Theme.of(context).textTheme.headlineMedium),
                        Visibility(
                          visible: reservationDetailsItems!.extraBedAmount!>0,
                          child: Text(
                              "Extra Bed Amount : ${reservationDetailsItems!.extraBedAmount?.toStringAsFixed(2) ?? ""} ",
                              style: Theme.of(context).textTheme.headlineMedium),
                        ),
                        
                        Row(
                          children: [
                            Text(
                                "Total : ${reservationDetailsItems!.totalPrice?.toStringAsFixed(2) ?? ""} $currency",
                                style:
                                    Theme.of(context).textTheme.headlineMedium),
                            const SizedBox(
                              width: 4,
                            ),
                            Visibility(
                              visible: reservationDetailsItems!.regularPrice !=
                                      null &&
                                  reservationDetailsItems!.regularPrice! > 0 &&
                                  reservationDetailsItems!.regularPrice! >
                                      reservationDetailsItems!.totalPrice!,
                              child: Text(
                                  " ${reservationDetailsItems!.regularPrice?.toStringAsFixed(2) ?? ""} $currency",
                                  style: Theme.of(context)
                                      .textTheme
                                      .headlineMedium
                                      ?.copyWith(
                                          decoration:
                                              TextDecoration.lineThrough)),
                            ),
                          ],
                        ),
                      ],
                    ),
                  ),
                  Text("Note : ${reservationDetailsItems!.guestNotes}",
                      maxLines: 1,
                      overflow: TextOverflow.ellipsis,
                      style: Theme.of(context).textTheme.headlineMedium),
                ],
              ),
            ),
          ],
        ));
  }
}
