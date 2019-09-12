using System;
using Diabetto.Core.ViewModels.ProductMeasures;
using Diabetto.iOS.Combiners;
using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace Diabetto.iOS.Views.Cells.ProductMeasures
{
    public partial class ProductMeasureTableViewCell : BaseTableViewCell
    {
        public static readonly NSString Key = new NSString(nameof(ProductMeasureTableViewCell));
        public static readonly UINib Nib;

        static ProductMeasureTableViewCell()
        {
            Nib = UINib.FromName(Key, NSBundle.MainBundle);
        }

        protected ProductMeasureTableViewCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(
                () =>
                {
                    var set = this.CreateBindingSet<ProductMeasureTableViewCell, ProductMeasureViewModel>();

                    set.Bind(ProductNameLabel)
                        .For(v => v.Text)
                        .To(v => v.ProductName);

                    set.Bind(AmountLabel)
                        .For(v => v.Text)
                        .ByCombining(
                            new StringJoinCombiner(" "),
                            v => v.Amount,
                            v => v.Unit.ShortName);

                    set.Bind(ProductBreadUnitsLabel)
                        .For(v => v.Text)
                        .To(v => v.BreadUnits);

                    set.Apply();
                });
        }
    }
}
