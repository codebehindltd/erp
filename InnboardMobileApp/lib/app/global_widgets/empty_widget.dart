import 'package:flutter/material.dart';

class EmptyScreen extends StatelessWidget {
  final String? title;
  final String? imageUrl;
  final double? topHeight;
  final double? height;
  const EmptyScreen({Key? key, this.title, this.imageUrl, this.topHeight,this.height}) : super(key: key);

//   @override
//   State<EmptyScreen> createState() => _EmptyScreenState();
// }

// class _EmptyScreenState extends State<EmptyScreen> {
  @override
  Widget build(BuildContext context) {
    return Center(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        children: [
          SizedBox(height: topHeight??130),
          SizedBox(
            width: 200,
            child: Image.asset(
              imageUrl!,
              fit: BoxFit.contain,
              height: height,
            ),
          ),
          const SizedBox(
            height: 20,
          ),
          Text(
            title!,
            textAlign: TextAlign.center,
            style: TextStyle(
                color: const Color(0xff4F596A).withOpacity(.5),
                fontSize: 16,
                fontWeight: FontWeight.w600),
          ),
        ],
      ),
    );
  }
}
