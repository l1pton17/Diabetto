using System;
using System.IO;
using System.Threading.Tasks;
using Diabetto.Core;
using Foundation;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.Plugin.Color.Platforms.Ios;
using UIKit;

namespace Diabetto.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public sealed class AppDelegate : MvxApplicationDelegate<Setup, App>
    {
        public override UIWindow Window { get; set; }

        // FinishedLaunching is the very first code to be executed in your app. Don't forget to call base!
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            var result = base.FinishedLaunching(application, launchOptions);

            //CustomizeAppearance();
            DisplayCrashReport();

            return result;
        }

        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            LogUnhandledException(newExc);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            LogUnhandledException(newExc);
        }

        internal static void LogUnhandledException(Exception exception)
        {
            try
            {
                const string errorFileName = "Fatal.log";
                var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
                var errorFilePath = Path.Combine(libraryPath, errorFileName);

                var errorMessage = String.Format(
                    "Time: {0}\r\nError: Unhandled Exception\r\n{1}",
                    DateTime.Now,
                    exception.ToString());

                File.WriteAllText(errorFilePath, errorMessage);
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }

        private static void DisplayCrashReport()
        {
            const string errorFilename = "Fatal.log";
            var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
            var errorFilePath = Path.Combine(libraryPath, errorFilename);

            if (!File.Exists(errorFilePath))
            {
                return;
            }

            var errorText = File.ReadAllText(errorFilePath);

            var alertView = new UIAlertView(
                "Crash Report",
                errorText,
                null,
                "Close",
                "Clear")
            {
                UserInteractionEnabled = true
            };

            alertView.Clicked += (sender, args) =>
            {
                if (args.ButtonIndex != 0)
                {
                    File.Delete(errorFilePath);
                }
            };

            alertView.Show();
        }

        private static void CustomizeAppearance()
        {
            UINavigationBar.Appearance.SetBackgroundImage(new UIImage(), UIBarPosition.Any, UIBarMetrics.Default);
            UINavigationBar.Appearance.ShadowImage = new UIImage();

            UINavigationBar.Appearance.SetTitleTextAttributes(
                new UITextAttributes()
                {
                    TextColor = UIColor.White,
                    Font = UIFont.SystemFontOfSize(17f, UIFontWeight.Semibold)
                });

            UINavigationBar.Appearance.Translucent = false;
            UINavigationBar.Appearance.BarTintColor = AppColors.PrimaryColor.ToNativeColor();
            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.BackgroundColor = AppColors.PrimaryColor.ToNativeColor();
            UINavigationBar.Appearance.BackIndicatorImage = new UIImage();
            UITabBar.Appearance.BackgroundColor = AppColors.PrimaryColor.ToNativeColor();

            UITabBarItem.Appearance.SetTitleTextAttributes(
                new UITextAttributes
                {
                    TextColor = AppColors.AccentColor.ToNativeColor()
                },
                UIControlState.Selected);

            UITextField.Appearance.TintColor = AppColors.AccentColor.ToNativeColor();
            UITextView.Appearance.TintColor = AppColors.AccentColor.ToNativeColor();
            UIButton.Appearance.SetTitleColor(AppColors.AccentColor.ToNativeColor(), UIControlState.Highlighted);
        }
    }
}