import 'package:flutter/material.dart';
import 'package:get/get.dart';

class BackButtonWidget extends StatelessWidget {
  const BackButtonWidget({
    Key? key,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return IconButton(
      icon: const Icon(
        Icons.arrow_back_rounded,
        color: Colors.white,
      ),
      onPressed: () {
        // Navigator.of(context).pop();
        Get.back();
      },
    );
  }
}
