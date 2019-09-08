using Diabetto.Core.ViewModels.Calendars;
using Diabetto.iOS.Controls.Calendar;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace Diabetto.iOS.Views.Calendars
{
    [MvxChildPresentation(Animated = true)]
    public sealed class MonthView : MvxViewController<MonthViewModel>
    {
        private DTCalendarMonthsViewSource _source;
        private DTCalendarMonthCollectionView _collectionView;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var calendarFlowLayout = new UICollectionViewFlowLayout
            {
                MinimumLineSpacing = 8,
                MinimumInteritemSpacing = 8,
                SectionInset = new UIEdgeInsets(
                    0,
                    0,
                    0,
                    0)
            };

            _collectionView = new DTCalendarMonthCollectionView(calendarFlowLayout)
            {
                AlwaysBounceVertical = false,
                AlwaysBounceHorizontal = false,
                TranslatesAutoresizingMaskIntoConstraints = true
            };

            _source = new DTCalendarMonthsViewSource(_collectionView);
            _collectionView.Source = _source;

            var bindingSet = this.CreateBindingSet<MonthView, MonthViewModel>();

            bindingSet
                .Bind(_source)
                .For(v => v.ItemsSource)
                .To(v => v.Days);

            bindingSet.Apply();
        }
    }
}