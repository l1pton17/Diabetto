using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Diabetto.iOS.Controls.Calendar
{
    public sealed class DTCalendarDaysViewSource : MvxCollectionViewSource
    {
        /// <inheritdoc />
        public DTCalendarDaysViewSource(UICollectionView collectionView)
            : base(collectionView, DTCalendarCell.Key)
        {
            CollectionView.RegisterClassForCell(
                typeof(DTCalendarCell),
                DTCalendarCell.Key);
        }

        protected override UICollectionViewCell GetOrCreateCellFor(
            UICollectionView collectionView,
            NSIndexPath indexPath,
            object item
        )
        {
            return (UICollectionViewCell) CollectionView.DequeueReusableCell(DTCalendarCell.Key, indexPath);
        }
    }
}