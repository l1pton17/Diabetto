
using System;
using Diabetto.Core.ViewModels.Measures;
using Diabetto.iOS.Converters;
using Diabetto.iOS.Extensions;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace Diabetto.iOS.Views.Measures
{
    [MvxFromStoryboard]
    [MvxChildPresentation(Animated = true)]
    public partial class MeasureView : MvxTableViewController<MeasureViewModel>
    {
        private static readonly LevelMvxValueConverter _levelLabelConverter = new LevelMvxValueConverter
        {
            EmptyValue = "not measured",
            Prefix = "mmol/l"
        };

        private bool _datePickerVisible;
        private bool _levelRowVisible;

        public MeasureView(IntPtr handle) : base(handle)
        {
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var tagPicker = new UIPickerView();
            var tagPickerViewModel = new MvxPickerViewModel(tagPicker);
            tagPicker.Model = tagPickerViewModel;
            tagPicker.ShowSelectionIndicator = true;

            TagTextField.TintColor = UIColor.Clear;
            TagTextField.InputView = tagPicker;
            TagTextField.ShouldChangeCharacters += delegate { return false; };

            this.HideKeyboardWhenTappedAround();

            var set = this.CreateBindingSet<MeasureView, MeasureViewModel>();

            set.Bind(SaveButton)
                .To(v => v.SaveCommand);

            set.Bind(DateValuePicker)
                .To(v => v.Date);

            set.Bind(DateValueLabel)
                .To(v => v.Date);

            set.Bind(LongInsulinStepper)
                .For(v => v.Value)
                .To(v => v.LongInsulin);

            set.Bind(LongInsulinValueLabel)
                .To(v => v.LongInsulin);

            set.Bind(ShortInsulinStepper)
                .For(v => v.Value)
                .To(v => v.ShortInsulin);

            set.Bind(ShortInsulinValueLabel)
                .To(v => v.ShortInsulin);

            set.Bind(LevelValueLabel)
                .To(v => v.NullableLevel)
                .WithConversion(_levelLabelConverter);

            set.Bind(HasLevelSwitch)
                .To(v => v.HasLevel);

            set.Bind(LevelStepper)
                .For(v => v.Value)
                .To(v => v.Level);

            set.Bind(TagTextField)
                .OneWay()
                .To(v => v.Tag);

            set.Bind(tagPickerViewModel)
                .For(v => v.ItemsSource)
                .To(v => v.Tags);

            set.Bind(tagPickerViewModel)
                .For(v => v.SelectedItem)
                .To(v => v.Tag);

            set.Apply();
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            var height = tableView.RowHeight;

            if (indexPath.Section == 0 && indexPath.Row == 1)
            {
                height = _datePickerVisible ? 216 : 0;
            }

            if (indexPath.Section == 0 && indexPath.Row == 4)
            {
                height = _levelRowVisible ? height : 0;
            }

            return height;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Section == 0)
            {
                if (indexPath.Row == 0)
                {
                    if (_datePickerVisible)
                    {
                        HideDatePickerRow();
                    }
                    else
                    {
                        ShowDatePickerRow();
                    }
                }

                if (indexPath.Row == 3)
                {
                    if (_levelRowVisible)
                    {
                        HideLevelRow();
                    }
                    else
                    {
                        ShowLevelRow();
                    }
                }
            }

            tableView.DeselectRow(indexPath, animated: false);
        }

        private void ShowLevelRow()
        {
            _levelRowVisible = true;
            TableView.BeginUpdates();
            TableView.EndUpdates();

            UIView.Animate(
                duration: 0.25,
                () => LevelRow.Alpha = 1.0f,
                () => LevelRow.Hidden = false);
        }

        private void HideLevelRow()
        {
            _levelRowVisible = false;
            TableView.BeginUpdates();
            TableView.EndUpdates();

            UIView.Animate(
                duration: 0.25,
                () => LevelRow.Alpha = 0.0f,
                () => LevelRow.Hidden = true);
        }

        private void ShowDatePickerRow()
        {
            _datePickerVisible = true;
            TableView.BeginUpdates();
            TableView.EndUpdates();

            UIView.Animate(
                duration: 0.25,
                () => DatePickerRow.Alpha = 1.0f,
                () => DatePickerRow.Hidden = false);
        }

        private void HideDatePickerRow()
        {
            _datePickerVisible = false;
            TableView.BeginUpdates();
            TableView.EndUpdates();

            UIView.Animate(
                duration: 0.25,
                () => DatePickerRow.Alpha = 0.0f,
                () => DatePickerRow.Hidden = true);
        }

        public override void ViewWillAppear(bool animated)
        {
            DateValuePicker.TranslatesAutoresizingMaskIntoConstraints = true;
            DateValuePicker.MaximumDate = (NSDate) DateTime.UtcNow;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        #endregion
    }
}