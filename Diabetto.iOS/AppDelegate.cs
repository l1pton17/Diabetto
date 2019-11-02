using System;
using System.IO;
using System.Threading.Tasks;
using Diabetto.Core;
using Diabetto.Core.Models;
using Diabetto.Core.ViewModelResults;
using Diabetto.Core.ViewModels.Measures;
using Diabetto.iOS.Helpers;
using Diabetto.iOS.Intents.Shared;
using Foundation;
using Intents;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.Platforms.Ios.Core;
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

            INPreferences.RequestSiriAuthorization(
                status =>
                {
                    switch (status)
                    {
                        case INSiriAuthorizationStatus.Authorized:
                            break;

                        case INSiriAuthorizationStatus.Denied:
                            break;

                        case INSiriAuthorizationStatus.NotDetermined:
                            break;

                        case INSiriAuthorizationStatus.Restricted:
                            break;
                    }
                });

            return result;
        }

        public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
        {
            if (userActivity.GetInteraction()?.Intent is AddMeasureIntent adMeasureIntent)
            {
                HandleIntent(adMeasureIntent);

                return true;
            }
            else if (userActivity.GetInteraction()?.Intent is AddShortInsulinIntent addShortInsulinIntent)
            {
                HandleIntent(addShortInsulinIntent);

                return true;
            }
            else if (userActivity.ActivityType == NSUserActivityHelper.AddMeasureActivityType)
            {
                HandleAddMeasureUserActivity();

                return true;
            }

            return false;
        }

        private void HandleAddMeasureUserActivity()
        {
            var navigationService = Mvx.IoCProvider.GetSingleton<IMvxNavigationService>();

            navigationService.Navigate<MeasureViewModel, Measure, EditResult<Measure>>(null);

            //var rootViewController = Window?.RootViewController as UINavigationController;
            //var orderHistoryViewController = rootViewController?.ViewControllers?.FirstOrDefault() as OrderHistoryTableViewController;
            //if (orderHistoryViewController is null)
            //{
            //    Console.WriteLine("Failed to access OrderHistoryTableViewController.");
            //    return;
            //}
            //var segue = OrderHistoryTableViewController.SegueIdentifiers.SoupMenu;
            //orderHistoryViewController.PerformSegue(segue, null);
        }

        private static void HandleIntent(AddShortInsulinIntent intent)
        {
            var handler = new AddShortInsulinIntentHandler();

            handler.HandleAddShortInsulin(
                intent,
                response =>
                {
                    if (response.Code != AddShortInsulinIntentResponseCode.Success)
                    {
                        Console.WriteLine(response.Code);
                    }
                });
        }

        private static void HandleIntent(AddMeasureIntent intent)
        {
            var handler = new AddMeasureIntentHandler();

            handler.HandleAddMeasure(
                intent,
                response =>
                {
                    if (response.Code != AddMeasureIntentResponseCode.Success)
                    {
                        Console.WriteLine(response.Code);
                    }
                });
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

        private static void LogUnhandledException(Exception exception)
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
    }
}