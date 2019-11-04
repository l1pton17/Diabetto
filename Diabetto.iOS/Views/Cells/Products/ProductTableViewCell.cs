using System;
using Diabetto.Core.ViewModels.Products;
using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace Diabetto.iOS.Views.Cells.Products
{
    public partial class ProductTableViewCell : BaseTableViewCell
    {
        public static readonly NSString Key = new NSString(nameof(ProductTableViewCell));
        public static readonly UINib Nib;

        static ProductTableViewCell()
        {
            Nib = UINib.FromName(Key, NSBundle.MainBundle);
        }

        public ProductTableViewCell(IntPtr handle)
            : base(handle)
        {
            this.DelayBind(
                () =>
                {
                    var set = this.CreateBindingSet<ProductTableViewCell, ProductCellViewModel>();

                    set.Bind(ProductNameLabel)
                        .For(v => v.Text)
                        .To(v => v.Name);

                    set.Apply();
                });
        }
    }
}
