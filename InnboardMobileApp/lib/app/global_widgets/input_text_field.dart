import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

class InputField extends StatefulWidget {
  final TextEditingController? controllerText;
  final String? hintText;
  final String? labelText;
  final String? validationText;
  final bool? obscureText;
  final IconButton? iconButton;
  final ValueChanged<bool>? onchange;
  final TextInputType? keyboardType;
  final Icon? icon;
  final int? maxLines;
  final TextAlign? textAlign;
  final bool? isEmail;
  final bool? isPassword;
  final String? validLengthText;
  final int? minLength;
  final bool? isPhoneNumber;
  final bool? autofocus;
  final bool? isReadOnly;
  final bool? isBold;
  final int? maxLength;
  final AutovalidateMode? autovalidateMode;
  final bool? isDigitOnly;
  const InputField(
      {Key? key,
      this.controllerText,
      this.hintText,
      this.labelText,
      this.validationText,
      this.iconButton,
      this.obscureText,
      //this.function,
      this.keyboardType,
      this.maxLines,
      this.textAlign,
      this.isEmail,
      this.isPassword = false,
      this.icon,
      this.minLength,
      this.validLengthText,
      this.autofocus,
      this.isPhoneNumber,
      this.isReadOnly = false,
      this.isBold,
      this.maxLength,
      this.autovalidateMode,
      this.onchange,
      this.isDigitOnly = false})
      : super(key: key);

  @override
  State<InputField> createState() => _InputFieldState();
}

class _InputFieldState extends State<InputField> {
  @override
  Widget build(BuildContext context) {
    return TextFormField(
        maxLength: widget.maxLength,
        keyboardType: widget.keyboardType,
        inputFormatters: widget.isDigitOnly == false
            ? null
            : <TextInputFormatter>[FilteringTextInputFormatter.digitsOnly],
        controller: widget.controllerText,
        maxLines: widget.maxLines ?? 1,
        readOnly: widget.isReadOnly ?? false,
        textAlign: widget.textAlign ?? TextAlign.start,
        decoration: InputDecoration(
          counter: SizedBox(),

          labelText: widget.labelText,
          // floatingLabelAlignment: FloatingLabelAlignment.center,
          contentPadding: const EdgeInsets.only(
            left: 15,
            right: 15,
          ),
          hintText: widget.hintText,

          hintStyle: const TextStyle(
              // fontSize: 16.0,
              color: Color(0xff9FA3A4),
              fontWeight: FontWeight.w500),
          border: OutlineInputBorder(
            borderRadius: BorderRadius.circular(8),
            // borderSide: BorderSide(
            //   width: 1.5,
            // )
          ),
          prefixIcon: widget.icon,
          suffixIcon: widget.iconButton,
        ),
        obscureText: widget.obscureText != null ? widget.obscureText! : false,
        autofocus: widget.autofocus ?? false,
        autovalidateMode: widget.autovalidateMode,
        validator: (value) {
          if (widget.isPassword == false && value!.trim().isEmpty) {
            return widget.validationText;
          } else if (widget.isPassword == true && value!.isEmpty) {
            return widget.validationText;
          } else if (widget.isEmail == true &&
              !RegExp(r'^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}').hasMatch(value!)) {
            return "Enter your correct email";
          } else if (widget.isPhoneNumber == true &&
                  !RegExp(r"^01[0-9]\d{8}$").hasMatch(value!)
              //  !RegExp(r'^(?:(?:\+|00)88|01)?\d{11}$').hasMatch(value!)
              ) {
            return "Enter correct mobile number";
          } else if (widget.minLength != null &&
              widget.minLength! > value!.trim().length) {
            return widget.validLengthText;
          } else {
            return null;
          }
        },
        style: TextStyle(
            fontWeight: widget.isBold == true ? FontWeight.bold : null),
        onChanged: widget.onchange == null
            ? null
            : (value) {
                widget.onchange!(true);
              });
  }
}
