import 'package:flutter/material.dart';

import '../core/values/colors.dart';

class ModalBottomSheet {
  static showBottomSheet(context, child, {screenHeight}) {
    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      isDismissible: false,
      useRootNavigator: true,
      useSafeArea: true,
      backgroundColor: bodyColor,
      shape: const RoundedRectangleBorder(
          borderRadius: BorderRadius.vertical(
        top: Radius.circular(20),
      )),
      builder: (context) => SizedBox(
        height: screenHeight + MediaQuery.of(context).viewInsets.bottom,
        child: child,
      ),

      // DraggableScrollableSheet(
      //         //initialChildSize: initialChildSize,
      //         maxChildSize: 0.96,
      //         minChildSize: 0.25,
      //         expand: false,
      //         snap: true,
      //         builder: (context, scrollController) {
      //           return child ;
      //         }),
    );
  }
}
