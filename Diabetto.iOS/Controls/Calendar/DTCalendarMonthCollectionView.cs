using CoreGraphics;
using UIKit;

namespace Diabetto.iOS.Controls.Calendar
{
    public sealed class DTCalendarMonthCollectionView : UICollectionView
    {
        public DTCalendarMonthCollectionView(UICollectionViewLayout layout)
            : base(CGRect.Empty, layout)
        {
        }
    }
}