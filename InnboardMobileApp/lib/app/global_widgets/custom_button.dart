import 'package:flutter/material.dart';
import 'package:leading_edge/app/core/values/colors.dart';

class CustomButton extends StatelessWidget {
  final ValueChanged<bool> submit;
  final String name;
   final bool fullWidth;
  //final double width;
  final double? borderRadius;
  final Color? bgColor;
  final double? fontSize;
  final double? horizontalPadding;
  const CustomButton(
      {Key? key,
      required this.submit,
      required this.name,
      required this.fullWidth,
      this.borderRadius,
      this.fontSize,
      this.bgColor,
      this.horizontalPadding})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    return InkWell(
      onTap: () {
        submit(true);
      },
      child: SizedBox(
        width: fullWidth ? double.infinity : null,
        //width: width ?? double.infinity,
        child: Container(
          margin: const EdgeInsets.symmetric(horizontal: 8),
          padding:  EdgeInsets.symmetric(horizontal: horizontalPadding??22, vertical: 8),
          decoration: BoxDecoration(
              color: bodyColor,
              borderRadius: BorderRadius.circular(borderRadius ?? 8),
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
          child: Center(
            child: Text(
              name,
              style: TextStyle(
                fontSize: fontSize ?? 18,
                color: themeColor,
                fontWeight: FontWeight.bold,
              ),
            ),
          ),
        ),
      ),
    );
  }

  Color getColor(Set<MaterialState> states) {
    // const Set<MaterialState> interactiveStates = <MaterialState>{
    //   MaterialState.pressed,
    //   MaterialState.hovered,
    //   MaterialState.focused,
    // };
    // if (states.any(interactiveStates.contains)) {
    //   return Colors.black12;
    // }
    return bgColor!;
  }
}
