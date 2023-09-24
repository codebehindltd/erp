import 'package:flutter/material.dart';
import 'package:leading_edge/app/core/values/colors.dart';
import 'package:shimmer/shimmer.dart';

// ignore: must_be_immutable
class CustomShimmerLoader extends StatelessWidget {
  double? height;
  double? width;
  double? radius;
  CustomShimmerLoader({super.key, this.height, this.width, this.radius});

  @override
  Widget build(BuildContext context) {
    return Shimmer.fromColors(
      // The baseColor and highlightColor creates a LinearGradient which would be painted over the child widget
      baseColor: loaderBaseColor,
      //Colors.grey[600]!,
      highlightColor: Colors.grey[300]!,
      direction: ShimmerDirection.ltr,
      child: Container(
        height: height,
        width: width,
        decoration: BoxDecoration(
            color: Colors.grey.withOpacity(0.2),
            borderRadius: BorderRadius.circular(radius!)),
      ),
    );
  }
}
