import 'package:flutter/material.dart';

import '../core/values/colors.dart';
class SearchBarWidget extends StatefulWidget {
  final TextEditingController? controllerText;
  final String? hintText;
  final double? widthSize;
  final ValueChanged<String>? changed;
  final bool? autofocus;
  const SearchBarWidget(
      {Key? key,
      this.controllerText,
      this.hintText,
      this.widthSize,
      this.changed,
      this.autofocus})
      : super(key: key);

  @override
  State<SearchBarWidget> createState() => _SearchBarWidgetState();
}

class _SearchBarWidgetState extends State<SearchBarWidget> {
  @override
  Widget build(BuildContext context) {
    return Container(
      width: widget.widthSize ?? double.infinity,
      height: 40,
      decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(18),
          // color: Colors.grey.withOpacity(.2),
          color: Colors.white
          // boxShadow: [
          //   BoxShadow(
          //     color: Colors.grey.withOpacity(0.5),
          //     // spreadRadius: 5,
          //     blurRadius: 7,
          //     offset: const Offset(0, 3), // changes position of shadow
          //   ),
          // ]
          ),
      child: TextFormField(
        autofocus: widget.autofocus ?? false,
        controller: widget.controllerText,
        decoration: InputDecoration(
          border: InputBorder.none,
          hintStyle: const TextStyle(fontSize: 14, color: Colors.grey),
          contentPadding: const EdgeInsets.only(left: 15, top: 5),
          hintText: widget.hintText ?? "Search here",
          suffixIcon: InkWell(
            onTap: () {
              widget.changed!(widget.controllerText != null
                  ? widget.controllerText!.text
                  : '');
            },
            child: Container(
              margin: const EdgeInsets.all(2),
              decoration: BoxDecoration(
                  // color: themeColor,
                  gradient: LinearGradient(
                    begin: Alignment.topLeft,
                    end: Alignment.bottomRight,
                    colors: [
                      themeColor.withOpacity(.8),
                      themeColor,
                    ],
                  ),
                  borderRadius: BorderRadius.circular(18),
                  border: Border.all(color: Colors.white)),
              padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 5),
              child: const Icon(
                Icons.search,
                color: Colors.white,
              ),
            ),
          ),
        ),
      ),
    );
  }
}
