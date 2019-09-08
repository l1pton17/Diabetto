using CoreGraphics;
using Diabetto.Core.ViewModels.Calendars;
using Diabetto.iOS.Controls.Calendar;
using Diabetto.iOS.Converters;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace Diabetto.iOS.Views.Calendars
{
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Measures")]
    public sealed class CalendarView : MvxViewController<CalendarViewModel>
    {
        private MonthHeaderView _monthView;
        private DTCalendarMonthsViewSource _source;
        private UICollectionView _yearView;
        private UICollectionViewFlowLayout _layout;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Title = "Measures";
            EdgesForExtendedLayout = UIRectEdge.None;

            NavigationItem.RightBarButtonItem = new UIBarButtonItem
            {
                Title = "Add"
            };

            _monthView = new MonthHeaderView
            {
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            _layout = new UICollectionViewFlowLayout
            {
                MinimumLineSpacing = 0,
                MinimumInteritemSpacing = 0,
                ItemSize = new CGSize(View.Frame.Width, 200),
                ScrollDirection = UICollectionViewScrollDirection.Horizontal,
                SectionInset = new UIEdgeInsets(
                    0,
                    0,
                    0,
                    0)
            };

            _yearView = new UICollectionView(CGRect.Empty, _layout)
            {
                PagingEnabled = true,
                AlwaysBounceVertical = false,
                AlwaysBounceHorizontal = false,
                TranslatesAutoresizingMaskIntoConstraints = false,
                Delegate = new DTCalendarViewDelegateFlowLayout()
            };

            _source = new DTCalendarMonthsViewSource(_yearView);

            _yearView.Source = _source;

            _monthView.RightButton.TouchUpInside += delegate
            {
                Move(1);
            };

            _monthView.LeftButton.TouchUpInside += delegate
            {
                Move(-1);
            };

            var bindingSet = this.CreateBindingSet<CalendarView, CalendarViewModel>();

            bindingSet
                .Bind(NavigationItem.RightBarButtonItem)
                .To(v => v.AddCommand);

            bindingSet
                .Bind(_monthView.NameLabel)
                .For(v => v.Text)
                .To(v => v.MonthDate)
                .WithConversion(
                    new DateFormatterMvxValueConverter(
                        new NSDateFormatter
                        {
                            TimeStyle = NSDateFormatterStyle.None,
                            DateFormat = "MMMM yyyy"
                        }));

            bindingSet
                .Bind(_source)
                .For(v => v.SelectionChangedCommand)
                .To(v => v.MonthShowed);

            bindingSet.Apply();
        }

        public override void ViewDidAppear(bool animated)
        {
            var indexPath = NSIndexPath.FromRowSection(_source.GetItemsCount(_yearView, 0) - 1, 0);

            _yearView.SelectItem(
                indexPath,
                animated: true,
                UICollectionViewScrollPosition.CenteredHorizontally);

            _yearView.Source.ItemSelected(_yearView, indexPath);
        }

        private void Move(int delta)
        {
            var selected = _source.SelectedPage;
            var indexPath = NSIndexPath.FromRowSection(selected + delta, 0);

            if (indexPath.Row < 0)
            {
                return;
            }

            if (indexPath.Row >= _yearView.Source.GetItemsCount(_yearView, 0))
            {
                return;
            }

            _yearView.SelectItem(
                indexPath,
                animated: true,
                UICollectionViewScrollPosition.CenteredHorizontally);

            _yearView.Source.ItemSelected(_yearView, indexPath);
        }

        public override void ViewWillLayoutSubviews()
        {
            View.AddSubview(_monthView);

            _monthView.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
            _monthView.LeftAnchor.ConstraintEqualTo(View.LeftAnchor).Active = true;
            _monthView.RightAnchor.ConstraintEqualTo(View.RightAnchor).Active = true;
            _monthView.HeightAnchor.ConstraintEqualTo(35).Active = true;

            View.AddSubview(_yearView);
            _yearView.TopAnchor.ConstraintEqualTo(_monthView.BottomAnchor).Active = true;
            _yearView.LeftAnchor.ConstraintEqualTo(View.LeftAnchor).Active = true;
            _yearView.RightAnchor.ConstraintEqualTo(View.RightAnchor).Active = true;
            _yearView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;

            _layout.ItemSize = _yearView.Frame.Size;
        }
    }
}