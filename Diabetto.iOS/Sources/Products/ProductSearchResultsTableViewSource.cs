using System;
using System.Linq;
using Diabetto.Core.ViewModels.ProductMeasures;
using Diabetto.iOS.Views.Cells.Products;
using Foundation;
using MvvmCross.Binding.Extensions;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Diabetto.iOS.Sources.Products
{
    public sealed class ProductSearchResultsTableViewSource : MvxTableViewSource
    {
        private bool _canAdd;

        /// <inheritdoc />
        public ProductSearchResultsTableViewSource(UITableView tableView)
            : base(tableView)
        {
            DeselectAutomatically = true;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;

            tableView.RegisterNibForCellReuse(
                ProductSearchResultTableViewCell.Nib,
                ProductSearchResultTableViewCell.Key);
        }

        /// <inheritdoc />
        protected override UITableViewCell GetOrCreateCellFor(
            UITableView tableView,
            NSIndexPath indexPath,
            object item)
        {
            return TableView.DequeueReusableCell(ProductSearchResultTableViewCell.Key);
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return _canAdd ? 2 : 1;
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return _canAdd && section == 0
                ? "Add product"
                : "Search results";
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (ItemsSource == null)
            {
                return 0;
            }
            
            if (_canAdd)
            {
                return section == 0 ? 1 : ItemsSource.Count() - 1;
            }

            return ItemsSource.Count();
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            var index = indexPath.Row + indexPath.Section;

            return ItemsSource?.ElementAt(index);
        }

        public override void ReloadTableData()
        {
            _canAdd = ItemsSource
                .OfType<ProductSearchItemViewModel>()
                .Any(v => v.IsNew);

            base.ReloadTableData();
        }
    }
}