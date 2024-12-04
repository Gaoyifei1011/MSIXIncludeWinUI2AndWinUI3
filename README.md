# MSIXIncludeWinUI2AndWinUI3

----------

### MSIX 打包项目同时使用 WinUI 2 和 WinUI 3

### MSIX packaging projects use both WinUI 2 and WinUI 3

----------

#### 在 MSIX 打包项目中，如果同时使用了 UWP 和 WinUI 3 桌面程序，并且 UWP 引用了 WinUI 2，编译并部署到本地设备后，由于 WinUI 2 和 WinUI 3 都使用了 Microsoft.UI.Xaml.dll，可能会出现应用程序启动时依赖查找问题。具体来说，应用程序清单中在启动时查找动态扩展库时，依赖的 PackageDependency 先后顺序可能导致 WinUI 3 桌面应用误加载 WinUI 2 的 Microsoft.UI.Xaml.dll，从而导致进程启动失败。
#### 为解决此问题，通过引入动态依赖项并强制调整依赖包的加载顺序，从而确保应用程序加载正确版本的 Microsoft.UI.Xaml.dll，避免由于版本冲突而导致的启动失败。

#### In the MSIX packaging project, if both UWP and WinUI 3 desktop applications are used, and UWP references WinUI 2, after compiling and deploying to the local device, Since both WinUI 2 and WinUI 3 use Microsoft.UI.Xaml.dll, application startup dependency lookup issues can occur. Specifically, the order of the PackageDependency dependency in the application manifest when looking for dynamic extension libraries at startup can cause the WinUI 3 desktop application to mistakenly load WinUI 2's Microsoft.UI.Xaml.dll, causing the process to fail to start.
#### To solve this problem, we ensure that the application loads the correct version of Microsoft.UI.Xaml.dll by introducing dynamic dependencies and forcibly adjusting the loading order of the dependency packages to avoid startup failures due to version conflicts.
----------

#### 其他说明

##### 1.必须保证 WinUI 2 和 WinUI 3 的依赖库出现在 AppxManifest.xml，WinUI 2 的依赖库名称是 Microsoft.UI.Xaml，WinUI 3 的依赖库名称是 Microsoft.WindowsAppSDK。

##### 2.MSIX 动态依赖项的最低系统要求是 Windows 11 22000(21H2)

----------

#### Other instructions

##### 1. You must ensure that WinUI 2 and WinUI 3 dependency libraries appear in AppxManifest.xml, WinUI 2 dependency library name is Microsoft.UI.Xaml, The dependency library name for WinUI 3 is Microsoft.WindowsAppSDK.

##### 2. The minimum system requirement for MSIX dynamic dependencies is Windows 11 22000(21H2)

----------

#### 参考资料（Reference）

> * [MSIX 动态依赖项（MSIX dynamic dependencies）](https://learn.microsoft.com/windows/apps/desktop/modernize/framework-packages/framework-packages-overview)&emsp;
