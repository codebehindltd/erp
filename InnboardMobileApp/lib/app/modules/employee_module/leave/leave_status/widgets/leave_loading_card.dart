import 'package:flutter/material.dart';
import 'package:skeletons/skeletons.dart';

import '../../../../../core/utils/size_config.dart';
import '../../../../../core/values/colors.dart';


class LoadingLeaveCard extends StatelessWidget {
  const LoadingLeaveCard({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Card(
      child: Container(
            color: themeColor.withOpacity(.03),
            padding: const EdgeInsets.all(8),
        child: Row(
          children: [
            SkeletonAvatar(
                style: SkeletonAvatarStyle(
                    width: SizeConfig.screenWidth! * .28,
                    height: SizeConfig.screenWidth! * .28,
                    borderRadius: BorderRadius.circular(100))),
            SizedBox(
              width: SizeConfig.screenWidth! * .04,
            ),
            SizedBox(
              width: SizeConfig.screenWidth! * .55,
              child: Column(
                children: [
                  SkeletonParagraph(
                    style: SkeletonParagraphStyle(
                        lines: 4,
                        spacing: 12,
                        padding: const EdgeInsets.symmetric(
                            horizontal: 0, vertical: 8),
                        lineStyle: SkeletonLineStyle(
                          randomLength: true,
                          height: 12,
                          borderRadius: BorderRadius.circular(8),
                          minLength: MediaQuery.of(context).size.width / 3,
                        )),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
