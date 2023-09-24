import 'package:flutter/material.dart';

class CustomSnackbar {
  final String msg;
  final Color bgColor;
  final Color? color;
  const CustomSnackbar({required this.msg, required this.bgColor, this.color});

  static snackbar(
      {required BuildContext context,
      required String msg,
      required Color bgColor,
      Color? color,
      IconData? icon}) {
    return ScaffoldMessenger.of(context)
        .showSnackBar(SnackBar(
          elevation: 2,
          backgroundColor: bgColor,
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(5)),
          behavior: SnackBarBehavior.floating,

          //action: SnackBarAction(label: Icon(Icons.forward), onPressed: onPressed),
          content: Row(
            children: [
              Visibility(
                  visible: icon != null,
                  child: Icon(
                    icon,
                    color: color ?? Colors.white,
                  )),
              Expanded(
                child: Text(
                  msg,
                  style: TextStyle(color: color ?? Colors.white),
                ),
              ),
            ],
          ),
          duration: const Duration(seconds: 2),
        ))
        .closed;
  }
}
