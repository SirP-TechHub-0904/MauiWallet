#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif

#if ANDROID
using AndroidX.Core.View;
using AndroidX.AppCompat.App;
using Android.Views;
using Android.OS;
using AView = Android.Views.View;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
#endif

using Microsoft.Maui.LifecycleEvents;
using SkiaSharp.Views.Maui.Controls.Hosting;
using CommunityToolkit.Maui;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;


#if ANDROID
[assembly: Android.App.UsesPermission(Android.Manifest.Permission.Camera)]
#endif

namespace MauiWallet;
public static class MauiProgram
{
    /// <summary>
    /// 3 configurable AppShellType: Main, Simple, Normal
    /// </summary>
    internal const AppShellType UsedAppShell = AppShellType.Main;

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            //.UseMauiCompatibility()
            .UseSkiaSharp()
            .UseSkiaSharp(true)
            .UseMauiMaps()
            .UseBarcodeReader()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Poppins-Regular.otf", "RegularFont");
                fonts.AddFont("Poppins-Medium.otf", "MediumFont");
                fonts.AddFont("Poppins-SemiBold.otf", "SemiBoldFont");
                fonts.AddFont("Poppins-Bold.otf", "BoldFont");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Font-Awesome-Solid.otf", "FontAwesomeSolid");

                fonts.AddFont("fa-solid-900.ttf", "FaPro");
                fonts.AddFont("fa-brands-400.ttf", "FaBrands");
                fonts.AddFont("fa-regular-400.ttf", "FaRegular");
                fonts.AddFont("line-awesome.ttf", "LineAwesome");
                fonts.AddFont("material-icons-outlined-regular.otf", "MaterialDesign");
                fonts.AddFont("ionicons.ttf", "IonIcons");
                fonts.AddFont("icon.ttf", "MauiKitIcons");
            })
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler(typeof(Video), typeof(VideoHandler));

                handlers.AddHandler(typeof(CameraBarcodeReaderView), typeof(CameraBarcodeReaderViewHandler));
                handlers.AddHandler(typeof(CameraView), typeof(CameraViewHandler));
                handlers.AddHandler(typeof(BarcodeGeneratorView), typeof(BarcodeGeneratorViewHandler));
            });

        builder.UseSimpleToolkit();

        //builder.DisplayContentBehindBars();

#if ANDROID
                    builder.SetDefaultStatusBarAppearance(color: Microsoft.Maui.Graphics.Colors.Transparent, lightElements: false);
                    builder.SetDefaultNavigationBarAppearance(color: Microsoft.Maui.Graphics.Colors.Transparent, lightElements: false);
#endif

        if (UsedAppShell is not AppShellType.Normal)
        {
            builder.UseSimpleShell();
        }

        builder.Services.AddLocalization();

#if WINDOWS
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddWindows(wndLifeCycleBuilder =>
                {
                    wndLifeCycleBuilder.OnWindowCreated(window =>
                    {
                        IntPtr nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                        WindowId win32WindowsId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
                        AppWindow winuiAppWindow = AppWindow.GetFromWindowId(win32WindowsId);

                        //https://github.com/dotnet/maui/issues/6976
                        var displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(win32WindowsId, Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);

                        int width = displayArea.WorkArea.Width * 2 / 3;
                        int height = displayArea.WorkArea.Height - 10;

                        winuiAppWindow.MoveAndResize(new RectInt32(15, 10, width, height));
                    });
                });
            });
#endif

        AppCenter.Start("windowsdesktop=78352d2a-b487-4995-9212-5a79066e6228;" +
                "android=1282d61f-e3cd-4592-8233-87956d398e62;" +
                "ios=f1eeea96-c2a6-47a1-b380-f0d0dc028d6b;" +
                "macos=15567566-c932-4d9a-897e-ce42c2bc2edd;",
                typeof(Analytics), typeof(Crashes));

        return builder.Build();
    }
}