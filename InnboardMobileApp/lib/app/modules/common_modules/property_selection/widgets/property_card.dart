import 'package:flutter/material.dart';
import 'package:flutter_rating_bar/flutter_rating_bar.dart';
import 'package:get/get.dart';
import 'package:leading_edge/app/data/models/res/property_model.dart';
import '../../../../core/environment/environment.dart';
import '../../../../core/values/colors.dart';

class PropertyCard extends StatelessWidget {
  final PropertyModel property;
  const PropertyCard({super.key, required this.property});

  @override
  Widget build(BuildContext context) {
    return Container(
      width: double.infinity,
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 6),
      decoration: BoxDecoration(
          color: bodyColor,
          borderRadius: BorderRadius.circular(14),
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
      child: Row(
        children: [
          SizedBox(
            height: 90,
            width: 90,
            child: ClipRRect(
                child: FadeInImage.assetNetwork(
                    image: Env.rootUrl + (property.logoUrl ?? ''),
                    height: 90,
                    width: 90,
                    placeholder: 'assets/images/logo.png',
                    placeholderFit: BoxFit.scaleDown,
                    imageErrorBuilder: (context, error, stackTrace) =>
                        Image.asset(
                          'assets/images/logo.png',
                          fit: BoxFit.scaleDown,
                          height: double.infinity,
                          width: double.infinity,
                        ),
                    fadeOutDuration: const Duration(milliseconds: 30))),
          ),
          SizedBox(
            width: Get.width - 180,
            child: Column(
              mainAxisAlignment: MainAxisAlignment.start,
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  property.propertyName ?? '',
                  style: TextStyle(
                      fontSize: 20,
                      color: themeColor,
                      fontWeight: FontWeight.w600),
                ),
                RatingBar.builder(
                  initialRating: double.parse(property.rankNumber ?? '0'),
                  direction: Axis.horizontal,
                  allowHalfRating: false,
                  itemCount: 5,
                  itemPadding: const EdgeInsets.only(right: 2.0),
                  itemSize: 11,
                  ignoreGestures: true,
                  itemBuilder: (context, _) => const Icon(
                    Icons.star_outlined,
                    color: ratingColor,
                  ),
                  onRatingUpdate: (rating) {},
                ),
                const SizedBox(
                  height: 5,
                ),
                Visibility(
                  visible: property.contactNumber != null,
                  child: Text(
                     property.contactNumber ?? '',
                    style: subTitleStyle(),
                  ),
                ),
                Visibility(
                  visible: property.webAddress != null,
                  child: Text(
                    property.webAddress ?? '',
                    style: subTitleStyle(),
                  ),
                ),
                Visibility(
                  visible: property.propertyAddress != null,
                  child: Text(
                    property.propertyAddress ?? '',
                    maxLines: 2,
                    overflow: TextOverflow.ellipsis,
                    style: subTitleStyle(),
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  TextStyle subTitleStyle() {
    return TextStyle(
      fontSize: 11,
      color: themeColor.withOpacity(.65),
    );
  }
}
