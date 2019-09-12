// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Diabetto.iOS.Views.Measures
{
    [Register ("MeasureView")]
    partial class MeasureView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AddButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView DatePickerRow { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DateValueLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIDatePicker DateValuePicker { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch HasLevelSwitch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView LevelRow { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStepper LevelStepper { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LevelValueLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStepper LongInsulinStepper { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LongInsulinValueLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView ProductsTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SaveButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStepper ShortInsulinStepper { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ShortInsulinValueLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Diabetto.iOS.UINoCursorTextField TagTextField { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AddButton != null) {
                AddButton.Dispose ();
                AddButton = null;
            }

            if (DatePickerRow != null) {
                DatePickerRow.Dispose ();
                DatePickerRow = null;
            }

            if (DateValueLabel != null) {
                DateValueLabel.Dispose ();
                DateValueLabel = null;
            }

            if (DateValuePicker != null) {
                DateValuePicker.Dispose ();
                DateValuePicker = null;
            }

            if (HasLevelSwitch != null) {
                HasLevelSwitch.Dispose ();
                HasLevelSwitch = null;
            }

            if (LevelRow != null) {
                LevelRow.Dispose ();
                LevelRow = null;
            }

            if (LevelStepper != null) {
                LevelStepper.Dispose ();
                LevelStepper = null;
            }

            if (LevelValueLabel != null) {
                LevelValueLabel.Dispose ();
                LevelValueLabel = null;
            }

            if (LongInsulinStepper != null) {
                LongInsulinStepper.Dispose ();
                LongInsulinStepper = null;
            }

            if (LongInsulinValueLabel != null) {
                LongInsulinValueLabel.Dispose ();
                LongInsulinValueLabel = null;
            }

            if (ProductsTableView != null) {
                ProductsTableView.Dispose ();
                ProductsTableView = null;
            }

            if (SaveButton != null) {
                SaveButton.Dispose ();
                SaveButton = null;
            }

            if (ShortInsulinStepper != null) {
                ShortInsulinStepper.Dispose ();
                ShortInsulinStepper = null;
            }

            if (ShortInsulinValueLabel != null) {
                ShortInsulinValueLabel.Dispose ();
                ShortInsulinValueLabel = null;
            }

            if (TagTextField != null) {
                TagTextField.Dispose ();
                TagTextField = null;
            }
        }
    }
}