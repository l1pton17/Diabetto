using System;
using CoreGraphics;
using Diabetto.Core.ViewModels.Calendars;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Diabetto.iOS.Controls.Calendar
{
    public sealed class DTCalendarMonthCell : MvxCollectionViewCell
    {
        public static NSString Key = new NSString(nameof(DTCalendarCell));

        private UICollectionView _collectionView;
        private DTCalendarDaysViewSource _source;

        public DTCalendarMonthCell(IntPtr handle)
            : base(String.Empty, handle)
        {
            InitializeUi();
            InitializeBindings();
        }

        private void InitializeUi()
        {
            var layout = new UICollectionViewFlowLayout
            {
                MinimumLineSpacing = 8,
                MinimumInteritemSpacing = 8,
                ScrollDirection = UICollectionViewScrollDirection.Vertical,
                SectionInset = new UIEdgeInsets(
                    0,
                    0,
                    0,
                    0)
            };

            _collectionView = new UICollectionView(CGRect.Empty, layout)
            {
                AlwaysBounceVertical = false,
                TranslatesAutoresizingMaskIntoConstraints = false,
                Delegate = new DTCalendarCollectionViewFlowDelegate()
            };

            _source = new DTCalendarDaysViewSource(_collectionView);

            _collectionView.Source = _source;

            ContentView.AddSubview(_collectionView);
            _collectionView.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
            _collectionView.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor).Active = true;
            _collectionView.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor).Active = true;
            _collectionView.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;
        }

        private void InitializeBindings()
        {
            this.DelayBind(
                () =>
                {
                    var bindingSet = this.CreateBindingSet<DTCalendarMonthCell, MonthViewModel>();

                    bindingSet
                        .Bind(_source)
                        .For(v => v.ItemsSource)
                        .To(v => v.Days);

                    bindingSet
                        .Bind(_source)
                        .For(v => v.SelectionChangedCommand)
                        .To(v => v.DaySelectedCommand);

                    bindingSet.Apply();
                });
        }
    }
}