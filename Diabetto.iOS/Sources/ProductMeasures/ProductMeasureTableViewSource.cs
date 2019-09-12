using Diabetto.Core.ViewModels.ProductMeasures;
using Diabetto.iOS.ViewModels;
using Diabetto.iOS.Views.Cells.ProductMeasures;
using Foundation;
using UIKit;

namespace Diabetto.iOS.Sources.ProductMeasures
{
    public sealed class ProductMeasureTableViewSource : MvxDeleteTableViewSource<ProductMeasureViewModel>
    {
        private static readonly NSString ProductMeasureCellIdentifier = new NSString(nameof(ProductMeasureTableViewCell));

        /// <inheritdoc />
        public ProductMeasureTableViewSource(UITableView tableView)
            : base(tableView)
        {
            DeselectAutomatically = true;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;

            tableView.RegisterNibForCellReuse(
                ProductMeasureTableViewCell.Nib,
                ProductMeasureCellIdentifier);
        }

        /// <inheritdoc />
        protected override UITableViewCell GetOrCreateCellFor(
            UITableView tableView,
            NSIndexPath indexPath,
            object item
        )
        {
            return TableView.DequeueReusableCell(ProductMeasureCellIdentifier);
        }
    }
}