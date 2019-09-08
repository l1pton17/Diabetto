// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Diabetto.iOS.Views.Cells
{
    [Register ("MeasureTableViewCell")]
    partial class MeasureTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel BreadUnitsLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel InsulinLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LevelLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TimeLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BreadUnitsLabel != null) {
                BreadUnitsLabel.Dispose ();
                BreadUnitsLabel = null;
            }

            if (InsulinLabel != null) {
                InsulinLabel.Dispose ();
                InsulinLabel = null;
            }

            if (LevelLabel != null) {
                LevelLabel.Dispose ();
                LevelLabel = null;
            }

            if (TimeLabel != null) {
                TimeLabel.Dispose ();
                TimeLabel = null;
            }
        }
    }
}