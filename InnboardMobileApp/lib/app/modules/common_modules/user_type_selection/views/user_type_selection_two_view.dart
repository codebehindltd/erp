import 'package:flutter/material.dart';

import 'package:get/get.dart';
import '../../../../core/environment/environment.dart';
import '../../../../core/values/colors.dart';
import '../controllers/user_type_selection_controller.dart';

class UserTypeSelectionTwoView extends GetView<UserTypeSelectionController> {
  const UserTypeSelectionTwoView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SafeArea(
        child: GetBuilder<UserTypeSelectionController>(builder: (_) {
          if (!controller.isLoading && !controller.isLoadingForAppInfo) {
            return SingleChildScrollView(
              child: Container(
                width: double.infinity,
                padding:
                    const EdgeInsets.symmetric(vertical: 20, horizontal: 10),
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    const SizedBox(
                      height: 40,
                    ),
                    Container(
                      height: 150,
                      width: 150,
                      child: controller.isLoadingForAppInfo == true
                          ? const Center(
                              child: CircularProgressIndicator(),
                            )
                          : ClipRRect(
                              borderRadius: BorderRadius.circular(100),
                              child: Image.network(
                                "${Env.rootUrl}${controller.appInfo?.imagePath??''}${controller.appInfo?.imageName??''}",
                                height: 150,
                                width: 150,
                                loadingBuilder: (context, Widget child,
                                    ImageChunkEvent? loadingProgress) {
                                  if (loadingProgress == null) {
                                    return child;
                                  }
                                  return SizedBox(
                                    height: 150,
                                    width: 150,
                                    child: Padding(
                                      padding: const EdgeInsets.all(18.0),
                                      child: Image.asset(
                                        "assets/images/empty_image.png",
                                        // color:Colors.grey.shade300.withOpacity(0.6),
                                      ),
                                    ),
                                  );
                                },
                                errorBuilder: (context, error, stackTrace) {
                                  return SizedBox(
                                    height: 150,
                                    width: 150,
                                    child: Padding(
                                      padding: const EdgeInsets.all(18.0),
                                      child: Image.asset(
                                        "assets/images/empty_image.png",
                                      ),
                                    ),
                                  );
                                },
                              )),
                    ),
                    const SizedBox(
                      height: 50,
                    ),
                    Padding(
                      padding: const EdgeInsets.symmetric(horizontal: 18),
                      child: ListView.separated(
                          itemCount: controller.userTypeListSecondRender.length,
                          shrinkWrap: true,
                          separatorBuilder: (context, index) => const SizedBox(
                                height: 25,
                              ),
                          physics: const ScrollPhysics(),
                          itemBuilder: (context, index) {
                            return InkWell(
                              onTap: () {
                                controller.typeSelect(
                                    controller.userTypeListSecondRender[index]);
                              },
                              child: Container(
                                width: double.infinity,
                                padding: const EdgeInsets.symmetric(
                                    horizontal: 12, vertical: 10),
                                decoration: BoxDecoration(
                                    color: bodyColor,
                                    borderRadius: BorderRadius.circular(14),
                                    boxShadow: [
                                      const BoxShadow(
                                        color: whiteColor,
                                        spreadRadius: 2,
                                        blurRadius: 6,
                                        offset: Offset(-5, -5),
                                      ),
                                      BoxShadow(
                                        color:
                                            bottonShadowColor.withOpacity(.5),
                                        spreadRadius: 1,
                                        blurRadius: 8,
                                        offset: const Offset(4, 4),
                                      )
                                    ]),
                                alignment: Alignment.center,
                                child: Text(
                                  controller.userTypeListSecondRender[index]
                                          .description ??
                                      '',
                                  style: TextStyle(
                                      fontSize: 25,
                                      color: themeColor,
                                      fontWeight: FontWeight.w600),
                                ),
                              ),
                            );
                          }),
                    )
                  ],
                ),
              ),
            );
          } else {
            return const Center(
              child: CircularProgressIndicator(),
            );
          }
        }),
      ),
    );
  }
}
