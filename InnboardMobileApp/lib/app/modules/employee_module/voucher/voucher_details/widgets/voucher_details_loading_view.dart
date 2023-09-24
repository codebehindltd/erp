import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:skeletons/skeletons.dart';

class VoucherDLoadingView extends StatelessWidget {
  const VoucherDLoadingView({super.key});

  @override
  Widget build(BuildContext context) {
    return Container(
      child: Column(
        children: [
          Row(
            children: [
              Flexible(
                flex: 1,
                child: SkeletonParagraph(
                  style: SkeletonParagraphStyle(
                      lines: 3,
                      spacing: 12,
                      padding: const EdgeInsets.symmetric(
                          horizontal: 0, vertical: 8),
                      lineStyle: SkeletonLineStyle(
                        randomLength: true,
                        height: 12,
                        borderRadius: BorderRadius.circular(4),
                        minLength: Get.width / 3,
                        maxLength: Get.width / 2.5,
                      )),
                ),
              ),
              Flexible(
                flex: 1,
                child: SkeletonParagraph(
                  style: SkeletonParagraphStyle(
                      lines: 3,
                      spacing: 12,
                      padding: const EdgeInsets.symmetric(
                          horizontal: 0, vertical: 8),
                      lineStyle: SkeletonLineStyle(
                        randomLength: true,
                        height: 12,
                        borderRadius: BorderRadius.circular(4),
                        minLength: Get.width / 3,
                      )),
                ),
              ),
            ],
          ),
          const SizedBox(
            height: 20,
          ),
          SkeletonParagraph(
            style: SkeletonParagraphStyle(
                lines: 4,
                spacing: 8,
                padding: const EdgeInsets.symmetric(horizontal: 0, vertical: 8),
                lineStyle: SkeletonLineStyle(
                  randomLength: true,
                  height: 10,
                  borderRadius: BorderRadius.circular(4),
                  minLength: Get.width / 1.5,
                )),
          ),
          const SizedBox(
            height: 20,
          ),
          SkeletonAvatar(
            style: SkeletonAvatarStyle(width: Get.width, height: 150),
          ),
          const SizedBox(
            height: 20,
          ),
          const SkeletonAvatar(
            style: SkeletonAvatarStyle(
              width: 120,
            ),
          ),
        ],
      ),
    );
  }
}
