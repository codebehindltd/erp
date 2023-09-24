import 'package:flutter/cupertino.dart';
import 'package:pull_to_refresh_flutter3/pull_to_refresh_flutter3.dart';

class CustomFooterRefresher extends StatelessWidget {
  const CustomFooterRefresher({
    Key? key,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return CustomFooter(
        loadStyle: LoadStyle.ShowWhenLoading,
        builder: ((context, mode) {
          Widget? body;
          if (mode == LoadStatus.idle) {
            body = const Text("Load Completed");
          } else if (mode == LoadStatus.loading) {
            body = const CupertinoActivityIndicator();
          } else if (mode == LoadStatus.failed) {
            body = const Text("Load Failed!");
          } else if (mode == LoadStatus.canLoading) {
            body = const Text("Load More");
          } else {
            body = const Text("No More Data");
          }
          return SizedBox(
            height: 55.0,
            child: Center(child: body),
          );
        }));
  }
}
