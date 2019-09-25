using System;
using Diabetto.Core.ViewModels.ProductMeasures;
using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace Diabetto.iOS.Views.Cells.Products
{
    public partial class ProductSearchResultTableViewCell : BaseTableViewCell
    {
        public static readonly NSString Key = new NSString(nameof(ProductSearchResultTableViewCell));
        public static readonly UINib Nib;

        static ProductSearchResultTableViewCell()
        {
            Nib = UINib.FromName(Key, NSBundle.MainBundle);
        }

        protected ProductSearchResultTableViewCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(
                () =>
                {
                    var bindingSet = this.CreateBindingSet<ProductSearchResultTableViewCell, ProductSearchItemViewModel>();

                    bindingSet
                        .Bind(NameLabel)
                        .For(v => v.Text)
                        .To(v => v.Name);

                    bindingSet
                        .Bind(CategoryNameLabel)
                        .For(v => v.Text)
                        .To(v => v.CategoryName);

                    bindingSet.Apply();
                });
        }
    }
}
