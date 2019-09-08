using System;
using Diabetto.Core.ViewModels.Calendars;
using Diabetto.iOS.Converters;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Diabetto.iOS.Controls.Calendar
{
    public sealed class DTCalendarCell : MvxCollectionViewCell
    {
        public static NSString Key = new NSString(nameof(DTCalendarCell));

        private UILabel _dayLabel;
        private UIView _viewHolder;

        public DTCalendarCell(IntPtr handle)
            : base(String.Empty, handle)
        {
            InitializeUi();
            InitializeBindings();
        }

        private void InitializeUi()
        {
            _viewHolder = new UIView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Layer =
                {
                    CornerRadius = 8
                }
            };

            ContentView.AddSubview(_viewHolder);
            _viewHolder.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor).Active = true;
            _viewHolder.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
            _viewHolder.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor).Active = true;
            _viewHolder.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;

            _dayLabel = new UILabel
            {

                TextColor = UIColor.Red,
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            _viewHolder.Add(_dayLabel);
            _dayLabel.CenterXAnchor.ConstraintEqualTo(_viewHolder.CenterXAnchor).Active = true;
            _dayLabel.CenterYAnchor.ConstraintEqualTo(_viewHolder.CenterYAnchor).Active = true;
        }
        
        private void InitializeBindings()
        {
            this.DelayBind(
                () =>
                {
                    var bindingSet = this.CreateBindingSet<DTCalendarCell, CalendarCellViewModel>();

                    bindingSet
                        .Bind(_dayLabel)
                        .For(v => v.Text)
                        .To(v => v.Day);

                    bindingSet
                        .Bind(_viewHolder)
                        .For(v => v.BackgroundColor)
                        .To(v => v.CellType)
                        .WithConversion(CalendarCellMvxValueConverter.Instance);

                    bindingSet
                        .Bind(this)
                        .For(v => v.Hidden)
                        .To(v => v.IsHidden);

                    bindingSet.Apply();
                });
        }
    }
}