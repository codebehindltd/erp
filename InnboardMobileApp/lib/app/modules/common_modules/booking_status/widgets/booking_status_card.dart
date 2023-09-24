import 'package:flutter/material.dart';
import 'package:leading_edge/app/data/models/res/reservation/reservation_list_model.dart';
import '../../../../core/values/colors.dart';

class BookingStatusCard extends StatelessWidget {
  final ReservationListModel model;
  final String? logo;
  const BookingStatusCard({super.key, required this.model, this.logo});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(8),
      child: Container(
        // height: 28,
        //color: Colors.white,
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
        child: Padding(
          padding: const EdgeInsets.all(8.0),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.start,
            children: [
              SizedBox(
                width: 110,
                child: Image.network(
                  logo??'',
                  loadingBuilder:
                      (context, Widget child, ImageChunkEvent? loadingProgress) {
                    if (loadingProgress == null) {
                      return child;
                    }
                    return SizedBox(
                      child: Image.asset(
                        "assets/images/emptyimage.png",
                        // color:Colors.grey.shade300.withOpacity(0.6),
                      ),
                    );
                  },
                  errorBuilder: (context, error, stackTrace) {
                    return SizedBox(
                      child: Image.asset(
                        "assets/images/emptyimage.png",
                        
                      ),
                    );
                  },
                ),
              ),
              const SizedBox(
                width: 14,
              ),
              Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  // Row(
                  //   children: [
                  //     Text(
                  //       "Hotel Name : ",
                  //       style: labelTextStyle(),
                  //     ),
                  //     Text(
                  //       "The Hotel westen",
                  //       style: valueTextStyle(),
                  //     ),
                  //   ],
                  // ),
                  Row(
                    children: [
                      Text(
                        "Reservation No : ",
                        style: labelTextStyle(),
                      ),
                      Text(
                        model.reservationNumber ?? '',
                        style: valueTextStyle(),
                      ),
                    ],
                  ),
                  Row(
                    children: [
                      Text(
                        "Booking Date : ",
                        style: labelTextStyle(),
                      ),
                      Text(
                        model.reservationDateDisplay ?? '',
                        style: valueTextStyle(),
                      ),
                    ],
                  ),
                  Row(
                    children: [
                      Text(
                        "Check In : ",
                        style: labelTextStyle(),
                      ),
                      Text(
                        "${model.dateInDisplay}",
                        style: valueTextStyle(),
                      ),
                    ],
                  ),

                   Row(
                    children: [
                      Text(
                        "Check Out : ",
                        style: labelTextStyle(),
                      ),
                      Text(
                        "${model.dateOutDisplay}",
                        style: valueTextStyle(),
                      ),
                    ],
                  ),
                  Row(
                    children: [
                      Text(
                        "Booking Status : ",
                        style: labelTextStyle(),
                      ),
                      Text(
                        model.reservationMode ?? '',
                        style: valueTextStyle(),
                      ),
                    ],
                  ),
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }

  TextStyle labelTextStyle() {
    return TextStyle(
        color: themeColor, fontWeight: FontWeight.w500, fontSize: 14);
  }

  TextStyle valueTextStyle() {
    return TextStyle(
        color: themeColor, fontWeight: FontWeight.w500, fontSize: 12);
  }
}
