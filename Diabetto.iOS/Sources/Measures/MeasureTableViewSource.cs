﻿using Diabetto.Core.ViewModels.Measures;
using Diabetto.iOS.Views.Cells.Measures;
using Foundation;
using UIKit;

namespace Diabetto.iOS.Sources.Measures
{
    public sealed class MeasureTableViewSource : MvxDeleteTableViewSource<MeasureCellViewModel>
    {
        private static readonly NSString MeasureCellIdentifier = new NSString("MeasureCell");

        /// <inheritdoc />
        public MeasureTableViewSource(UITableView tableView)
            : base(tableView)
        {
            DeselectAutomatically = true;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;

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