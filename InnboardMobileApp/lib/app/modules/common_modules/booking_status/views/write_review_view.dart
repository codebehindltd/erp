import 'package:flutter/material.dart';
import 'package:flutter_rating_bar/flutter_rating_bar.dart';
import 'package:get/get.dart';
import '../../../../global_widgets/custom_button.dart';
import '../../../../global_widgets/input_text_field.dart';
import '../controllers/booking_status_controller.dart';

class WriteReviewView extends GetView<BookingStatusController> {
  const WriteReviewView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return 
        Padding(
         padding: const EdgeInsets.only(top: 10),
          child: Column(
              children: [
                    Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              const SizedBox(),
              Padding(
                padding: const EdgeInsets.only(left: 26),
                child: Text(
                  "Write review",
                  style: Theme.of(context).textTheme.headlineLarge,
                ),
              ),
              GestureDetector(
                onTap: () {
                  Get.back();
                },
                child: const Padding(
                  padding: EdgeInsets.only(right: 7),
                  child: Icon(
                    Icons.close_outlined,
                    color: Colors.black,
                    size: 22,
                  ),
                ),
              ),
            ],
          ),
          const SizedBox(
            height: 10,
          ),
          Container(
            height: 6,
            color: Colors.grey.withOpacity(0.2),
          ),
          Padding(
             padding: const EdgeInsets.all(14.0),
            child: Column(
              children: [
                Row(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    RatingBar.builder(
                      ignoreGestures: false,
                      itemSize: 28,
                      initialRating: 1,
                      //ratingValue.value.toDouble(),
                      minRating: 1,
                      direction: Axis.horizontal,
                      allowHalfRating: false,
                      itemCount: 5,
                      itemPadding: const EdgeInsets.symmetric(horizontal: 2.0),
                      itemBuilder: (context, _) => const Icon(
                        Icons.star,
                        color: Colors.amber,
                        size: 40,
                      ),
                      onRatingUpdate: (rating) {
                        print(rating);
                        //ratingValue.value = rating.toInt();
                      },
                    ),
                  ],
                ),
                const SizedBox(
                  height: 10,
                ),
                Padding(
                  padding: const EdgeInsets.symmetric(vertical: 8.0),
                  child: InputField(
                    controllerText: controller.reviewController,
                    labelText: "Write review",
                    //hintText: "Note",
                    maxLines: 3,
                    maxLength: 200,
                  ),
                ),
                const SizedBox(
                  height: 10,
                ),
                Row(
                  mainAxisAlignment: MainAxisAlignment.end,
                  children: [
                    CustomButton(
                      submit: (value) {
                        Get.back();
                      },
                      name: "Submit",
                      fullWidth: false,
                      horizontalPadding: 52,
                    ),
                  ],
                ),
              ],
            ),
          ),
         
              ],
            ),
        );
  }
}
