using Diabetto.Core.Models;
using Diabetto.iOS.ViewModels;
using Diabetto.iOS.Views.Cells;
using Foundation;
using UIKit;

namespace Diabetto.iOS.Sources
{
    public sealed class MeasureTableViewSource : MvxDeleteTableViewSource<Measure>
    {
        private static readonly NSString MeasureCellIdentifier = new NSString("MeasureCell");

        /// <inheritdoc />
        public MeasureTableViewSource(UITableView tableView)
            : base(tableView)
        {
            DeselectAutomatically = true;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            tableView.RegisterNibForCellReuse(
                MeasureTableViewCell.Nib,
                MeasureCellIdentifier);
        }

        /// <inheritdoc />
        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            return TableView.DequeueReusableCell(MeasureCellIdentifier);
        }
    }
} 