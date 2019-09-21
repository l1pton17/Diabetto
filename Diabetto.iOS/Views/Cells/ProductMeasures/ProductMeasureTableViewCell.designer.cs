// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Diabetto.iOS.Views.Cells.ProductMeasures
{
    [Register ("ProductMeasureTableViewCell")]
    partial class ProductMeasureTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel AmountLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ProductBreadUnitLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ProductNameLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AmountLabel != null) {
                AmountLabel.Dispose ();
                AmountLabel = null;
            }

            if (ProductBreadUnitLabel != null) {
                ProductBreadUnitLabel.Dispose ();
                ProductBreadUnitLabel = null;
            }

            if (ProductNameLabel != null) {
                ProductNameLabel.Dispose ();
                ProductNameLabel = null;
            }
        }
    }
}