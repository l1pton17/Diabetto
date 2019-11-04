using Diabetto.Core.ViewModels.Products;
using Diabetto.iOS.Views.Cells.Products;
using Foundation;
using UIKit;

namespace Diabetto.iOS.Sources.Products
{
    public sealed class ProductTableViewSource : MvxDeleteTableViewSource<ProductCellViewModel>
    {
        public ProductTableViewSource(UITableView tableView)
            : base(tableView)
        {
            DeselectAutomatically = true;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            tableView.RegisterNibForCellReuse(
                ProductTableViewCell.Nib,
                ProductTableViewCell.Key);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            return TableView.DequeueReusableCell(ProductTableViewCell.Key);
        }
    }
}