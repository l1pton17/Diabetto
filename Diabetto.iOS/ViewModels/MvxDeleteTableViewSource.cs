using System;
using System.Reactive;
using System.Reactive.Linq;
using Foundation;
using MvvmCross.Binding.Extensions;
using MvvmCross.Platforms.Ios.Binding.Views;
using ReactiveUI;
using UIKit;

namespace Diabetto.iOS.ViewModels
{
    public abstract class MvxDeleteTableViewSource<TItem> : MvxTableViewSource
    {
        public ReactiveCommand<TItem, Unit> DeleteItemCommand { get; set; }

        /// <inheritdoc />
        protected MvxDeleteTableViewSource(UITableView tableView)
            : base(tableView)
        {
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:
                    var item = (TItem) ItemsSource.ElementAt(indexPath.Row);

                    DeleteItemCommand.Execute(item).Subscribe();

                    break;
            }
        }

        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableViewCellEditingStyle.Delete;
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true;
        }

        public override bool CanMoveRow(UITableView tableView, NSIndexPath indexPath)
        {
            return false;
        }
    }
}