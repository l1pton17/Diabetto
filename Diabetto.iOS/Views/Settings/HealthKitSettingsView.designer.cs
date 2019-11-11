// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Diabetto.iOS.Views.Settings
{
    [Register ("HealthKitSettingsView")]
    partial class HealthKitSettingsView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CloseButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch EnabledSwitch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ExportButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CloseButton != null) {
                CloseButton.Dispose ();
                CloseButton = null;
            }

            if (EnabledSwitch != null) {
                EnabledSwitch.Dispose ();
                EnabledSwitch = null;
            }

            if (ExportButton != null) {
                ExportButton.Dispose ();
                ExportButton = null;
            }
        }
    }
}