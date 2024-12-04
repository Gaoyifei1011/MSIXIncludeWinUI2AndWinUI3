using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.Windows.ApplicationModel.DynamicDependency;
using System;
using System.Collections.Generic;
using System.Threading;
using Windows.ApplicationModel;
using Windows.System;
using WinRT;
using WinUI3App.WindowsAPI.PInvoke.KernelBase;

namespace WinUI3App
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            ComWrappersSupport.InitializeComWrappers();

            // 使用 MSIX 动态依赖包 API，强行修改静态包图的依赖顺序，解决 WinUI 3 桌面应用程序加载时错误加载成 WinUI 2 程序集，导致程序启动失败的问题
            IReadOnlyList<Package> dependencyPackageList = Package.Current.Dependencies;
            PackageDependencyProcessorArchitectures packageDependencyProcessorArchitectures = PackageDependencyProcessorArchitectures.None;

            switch (Package.Current.Id.Architecture)
            {
                case ProcessorArchitecture.X86:
                    {
                        packageDependencyProcessorArchitectures = PackageDependencyProcessorArchitectures.X86;
                        break;
                    }
                case ProcessorArchitecture.X64:
                    {
                        packageDependencyProcessorArchitectures = PackageDependencyProcessorArchitectures.X64;
                        break;
                    }
                case ProcessorArchitecture.Arm64:
                    {
                        packageDependencyProcessorArchitectures = PackageDependencyProcessorArchitectures.Arm64;
                        break;
                    }
                case ProcessorArchitecture.X86OnArm64:
                    {
                        packageDependencyProcessorArchitectures = PackageDependencyProcessorArchitectures.X86OnArm64;
                        break;
                    }
                case ProcessorArchitecture.Neutral:
                    {
                        packageDependencyProcessorArchitectures = PackageDependencyProcessorArchitectures.Neutral;
                        break;
                    }
                case ProcessorArchitecture.Unknown:
                    {
                        packageDependencyProcessorArchitectures = PackageDependencyProcessorArchitectures.None;
                        break;
                    }
            }

            foreach (Package dependencyPacakge in dependencyPackageList)
            {
                if (dependencyPacakge.DisplayName.Contains("WindowsAppRuntime") && KernelBaseLibrary.TryCreatePackageDependency(IntPtr.Zero, dependencyPacakge.Id.FamilyName, new Windows.ApplicationModel.PackageVersion(), packageDependencyProcessorArchitectures, PackageDependencyLifetimeArtifactKind.Process, string.Empty, WindowsAPI.PInvoke.KernelBase.CreatePackageDependencyOptions.CreatePackageDependencyOptions_None, out string packageDependencyId) is 0)
                {
                    if (KernelBaseLibrary.AddPackageDependency(packageDependencyId, 0, WindowsAPI.PInvoke.KernelBase.AddPackageDependencyOptions.AddPackageDependencyOptions_PrependIfRankCollision, out _, out _) is 0)
                    {
                        break;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            // 启动桌面程序
            Application.Start((param) =>
            {
                SynchronizationContext.SetSynchronizationContext(new DispatcherQueueSynchronizationContext(Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread()));
                new WinUIApp();
            });
        }
    }
}