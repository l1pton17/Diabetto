using Cirrious.FluentLayouts.Touch;
using Diabetto.Core.ViewModels.Measures;
using Diabetto.iOS.Converters;
using Diabetto.iOS.Sources;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace Diabetto.iOS.Views.Measures
{
    [MvxChildPresentation(Animated = true)]
    public sealed class MeasuresView : MvxViewController<MeasuresViewModel>
    {
        private UITableView _tableView;
        private MeasureTableViewSource _source;
        private MvxUIRefreshControl _refreshControl;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Measures";

            EdgesForExtendedLayout = UIRectEdge.None;

            View.BackgroundColor = UIColor.Clear;

            _refreshControl = new MvxUIRefreshControl();

            _tableView = new UITableView
            {
                RowHeight = 60,
                SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine
            };

            _source = new MeasureTableViewSource(_tableView);
            _tableView.Source = _source;

            View.AddSubview(_tableView);
            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(
                _tableView.AtLeftOf(View),
                _tableView.AtTopOf(View),
                _tableView.AtBottomOf(View),
                _tableView.AtRightOf(View));

            View.BringSubviewToFront(_tableView);

            var bindingSet = this.CreateBindingSet<MeasuresView, MeasuresViewModel>();

            bindingSet
                .Bind(NavigationItem.RightBarButtonItem)
                .To(v => v.AddCommand);

            bindingSet
                .Bind(_refreshControl)
                .For(r => r.IsRefreshing)
                .To(vm => vm.LoadTask.IsNotCompleted)
                .WithFallback(false);

            bindingSet
                .Bind(_source)
                .For(v => v.ItemsSource)
                .To(vm => vm.Measures);

            bindingSet
                .Bind(_source)
                .For(v => v.SelectionChangedCommand)
                .To(vm => vm.SelectedCommand);

            bindingSet.Bind(_source)
                .For(v => v.DeleteItemCommand)
                .To(vm => vm.DeleteCommand);

            bindingSet
                .Bind(this)
                .For(v => v.Title)
                .To(v => v.Date)
                .WithConversion(
                    new DateFormatterMvxValueConverter(
                        new NSDateFormatter
                        {
                            TimeStyle = NSDateFormatterStyle.None,
                            DateFormat = "MMMM yyyy"
                        }));

            bindingSet.Apply();
        }
    }
}