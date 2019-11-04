// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Diabetto.iOS.Views.Cells.Products
{
    [Register ("ProductTableViewCell")]
    partial class ProductTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ProductNameLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ProductNameLabel != null) {
                ProductNameLabel.Dispose ();
                ProductNameLabel = null;
            }
        }
    }
}