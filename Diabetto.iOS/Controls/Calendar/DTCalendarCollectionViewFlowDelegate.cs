using CoreGraphics;
using Foundation;
using UIKit;

namespace Diabetto.iOS.Controls.Calendar
{
    public sealed class DTCalendarCollectionViewFlowDelegate : UICollectionViewDelegateFlowLayout
    {
        public override CGSize GetSizeForItem(
            UICollectionView collectionView,
            UICollectionViewLayout layout,
            NSIndexPath indexPath
        )
        {
            var screenWidth = collectionView.Frame.Width;
            var cellWidth = screenWidth / 7.0 - 8;
            var size = new CGSize(cellWidth, cellWidth);

            return size;
        }
    }
}