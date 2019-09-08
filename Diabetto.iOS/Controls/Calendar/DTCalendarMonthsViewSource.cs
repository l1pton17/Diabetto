using System;
using Diabetto.Core.ViewModels.Calendars;
using Foundation;
using MvvmCross;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Diabetto.iOS.Controls.Calendar
{
    public sealed class DTCalendarMonthsViewSource : MvxCollectionViewSource
    {
        public int SelectedPage { get; private set; }

        public DTCalendarMonthsViewSource(UICollectionView collectionView)
            : base(collectionView, DTCalendarCell.Key)
        {
            CollectionView.RegisterClassForCell(
                typeof(DTCalendarMonthCell),
                DTCalendarMonthCell.Key);

            ReloadOnAllItemsSourceSets = true;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return 12 * 10;
        }

        private DateTime GetMonthForOffset(nint value)
        {
            var now = DateTime.UtcNow;

            var days = new DateTime(
                    now.Year,
                    now.Month,
                    1,
                    0,
                    0,
                    0,
                    DateTimeKind.Utc)
                .AddMonths((int) value - (int) GetItemsCount(CollectionView, 0) + 1);

            return days;
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            var month = GetMonthForOffset(indexPath.Row);

            var viewModel = Mvx.IoCProvider
                .Resolve<IMonthViewModelFactory>()
                .Create(month);

            return viewModel;
        }

        protected override UICollectionViewCell GetOrCreateCellFor(
            UICollectionView collectionView,
            NSIndexPath indexPath,
            object item
        )
        {
            return (UICollectionViewCell) CollectionView.DequeueReusableCell(DTCalendarMonthCell.Key, indexPath);
        }

        public override void ReloadData()
        {
            CollectionView.Layer.RemoveAllAnimations();
            base.ReloadData();
        }

        public override void DecelerationEnded(UIScrollView scrollView)
        {
            UpdateCurrentPage(scrollView);
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            if (SelectedPage == indexPath.Row)
            {
                return;
            }

            SelectedPage = indexPath.Row;

            base.ItemSelected(collectionView, indexPath);
        }

        private void UpdateCurrentPage(UIScrollView scrollView)
        {
            var pageWidth = scrollView.Frame.Width;
            var offset = Clamp(scrollView.ContentOffset.X, 0, scrollView.ContentSize.Width - pageWidth);
            var page = (int) Math.Round(offset / pageWidth);

            ItemSelected(CollectionView, NSIndexPath.FromRowSection(page, 0));
        }

        private static double Clamp(double valueToClamp, double min, double max)
        {
            return Math.Min(Math.Max(valueToClamp, min), max);
        }
    }
}