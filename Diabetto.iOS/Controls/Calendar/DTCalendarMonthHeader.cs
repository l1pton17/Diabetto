using Foundation;
using UIKit;

namespace Diabetto.iOS.Controls.Calendar
{
    public sealed class DTCalendarMonthHeader : UICollectionReusableView
    {
        public static readonly NSString Key = new NSString(nameof(DTCalendarMonthHeader));

        private UILabel _monthName;

        public string MonthName
        {
            get
            {
                return _monthName.Text;
            }
            set
            {
                _monthName.Text = value;
                SetNeedsDisplay();
            }
        }

        [Export("initWithFrame:")]
        public DTCalendarMonthHeader(System.Drawing.RectangleF frame)
            : base(frame)
        {
            BackgroundColor = UIColor.FromRGBA(
                170,
                170,
                170,
                170);

            _monthName = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextColor = UIColor.FromRGB(245, 241, 230),
                TextAlignment = UITextAlignment.Center
            };

            AddSubview(_monthName);
            _monthName.LeftAnchor.ConstraintEqualTo(LeftAnchor).Active = true;
            _monthName.RightAnchor.ConstraintEqualTo(RightAnchor).Active = true;
            _monthName.CenterYAnchor.ConstraintEqualTo(CenterYAnchor).Active = true;
        }
    }
}