import 'package:flutter/material.dart';
import 'package:get/get.dart';
import '../../../../core/environment/environment.dart';
import '../../../../core/values/colors.dart';
import '../../../../global_widgets/custom_shimmer_loader.dart';
import '../controllers/user_type_selection_controller.dart';

class UserTypeSelectionView extends GetView<UserTypeSelectionController> {
  const UserTypeSelectionView({Key? key}) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: GetBuilder<UserTypeSelectionController>(builder: (_) {
        // if (controller.isLoading == false) {
        return SingleChildScrollView(
          child: Container(
            width: double.infinity,
            padding: const EdgeInsets.symmetric(vertical: 20, horizontal: 10),
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
                      ? Center(
                          child: CustomShimmerLoader(
                          height: 150,
                          width: 150,
                          radius: 100,
                        ))
                      : ClipRRect(
                          borderRadius: BorderRadius.circular(100),
                          child: Image.network(
                            "${Env.rootUrl}${controller.appInfo?.imagePath ?? ''}${controller.appInfo?.imageName ?? ''}",
                            //"",
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
                                    //fit: BoxFit.contain,
                                    // color:
                                    //     Colors.grey.shade300.withOpacity(0.6),
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
                                    //fit: BoxFit.contain,
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
                  child: controller.isLoading == false
                      ? ListView.separated(
                          itemCount: controller.userTypeList.length,
                          shrinkWrap: true,
                          separatorBuilder: (context, index) => const SizedBox(
                                height: 25,
                              ),
                          physics: const ScrollPhysics(),
                          itemBuilder: (context, index) {
                            return InkWell(
                              onTap: () {
                                try {
                                  controller.typeSelect(
                                      controller.userTypeList[index]);
                                } catch (e) {
                                  // Get.snackbar(
                                  //   "Fail2!",
                                  //   e.toString(),
                                  //   snackPosition: SnackPosition.BOTTOM,
                                  //   backgroundColor: warningColor,
                                  // );
                                }
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
                                  controller.userTypeList[index].description ??
                                      '',
                                  style: TextStyle(
                                      fontSize: 25,
                                      color: themeColor,
                                      fontWeight: FontWeight.w600),
                                ),
                              ),
                            );
                          })
                      : ListView.separated(
                          itemCount: 3,
                          shrinkWrap: true,
                          separatorBuilder: (context, index) => const SizedBox(
                                height: 20,
                              ),
                          physics: const ScrollPhysics(),
                          itemBuilder: (context, index) {
                            return CustomShimmerLoader(
                              height: 66,
                              // width: 150,
                              radius: 12,
                            );
                          }),
                )
              ],
            ),
          ),
        );
        // } else {
        //   return const Center(
        //     child: CircularProgressIndicator(),
        //   );
        // }
      }),
    );
  }
}
