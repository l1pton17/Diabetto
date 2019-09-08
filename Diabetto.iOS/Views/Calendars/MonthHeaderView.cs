using UIKit;

namespace Diabetto.iOS.Views.Calendars
{
    public sealed class MonthHeaderView : UIView
    {
        public UILabel NameLabel { get; } = new UILabel
        {
            Text = "Default Month Year Text",
            BackgroundColor = UIColor.White,
            TextAlignment = UITextAlignment.Center,
            Font = UIFont.BoldSystemFontOfSize(16),
            TranslatesAutoresizingMaskIntoConstraints = false
        };

        public UIButton RightButton { get; } = new UIButton
        {
            TranslatesAutoresizingMaskIntoConstraints = false
        };


        public UIButton LeftButton { get; } = new UIButton
        {
            TranslatesAutoresizingMaskIntoConstraints = false
        };

        public MonthHeaderView()
        {
            RightButton.SetTitle(">", UIControlState.Normal);
            LeftButton.SetTitle("<", UIControlState.Normal);
        }

        public override void LayoutSubviews()
        {
            AddSubview(NameLabel);
            NameLabel.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
            NameLabel.CenterXAnchor.ConstraintEqualTo(CenterXAnchor).Active = true;
            NameLabel.WidthAnchor.ConstraintEqualTo(150).Active = true;
            NameLabel.HeightAnchor.ConstraintEqualTo(HeightAnchor).Active = true;

            AddSubview(RightButton);
            RightButton.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
            RightButton.RightAnchor.ConstraintEqualTo(RightAnchor).Active =true;
            RightButton.WidthAnchor.ConstraintEqualTo(50).Active = true;
            RightButton.HeightAnchor.ConstraintEqualTo(HeightAnchor).Active = true;

            AddSubview(LeftButton);
            LeftButton.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
            LeftButton.LeftAnchor.ConstraintEqualTo(LeftAnchor).Active = true;
            LeftButton.WidthAnchor.ConstraintEqualTo(50).Active = true;
            LeftButton.HeightAnchor.ConstraintEqualTo(HeightAnchor).Active = true;
        }
    }
}