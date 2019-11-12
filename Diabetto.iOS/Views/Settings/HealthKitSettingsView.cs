
using System;
using Cirrious.FluentLayouts.Touch;
using Diabetto.iOS.ViewModels.Settings;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace Diabetto.iOS.Views.Settings
{
    [MvxFromStoryboard(StoryboardName = "HealthKitSettingsView")]
    [MvxModalPresentation(
        Animated = true,
        ModalPresentationStyle = UIModalPresentationStyle.FormSheet)]
    public partial class HealthKitSettingsView : MvxTableViewController<HealthKitSettingsViewModel>
    {
        private UIButton _closeButton;

        public HealthKitSettingsView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _closeButton = new UIButton
            {
                VerticalAlignment = UIControlContentVerticalAlignment.Center,
                HorizontalAlignment = UIControlContentHorizontalAlignment.Center,
                Font = UIFont.SystemFontOfSize(17)
            };

            _closeButton.SetTitleColor(TableView.TintColor, UIControlState.Normal);
            _closeButton.SetTitle("Close", UIControlState.Normal);

            TableView.TableFooterView = new UIView
            {
                BackgroundColor = TableView.BackgroundColor,
                Frame = new CoreGraphics.CGRect(
                    0,
                    0,
                    View.Frame.Width,
                    30)
            };

            TableView.TableFooterView.AddSubviews(_closeButton);
            TableView.TableFooterView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            TableView.TableFooterView.AddConstraints(
                _closeButton.WithSameHeight(TableView.TableFooterView),
                _closeButton.WithSameWidth(TableView.TableFooterView)
            );

            var bindingSet = this.CreateBindingSet<HealthKitSettingsView, HealthKitSettingsViewModel>();
            
            bindingSet
                .Bind(EnabledSwitch)
                .To(v => v.IsEnabled);

            bindingSet
                .Bind(ExportButton)
                .To(v => v.ExportCommand);

            bindingSet
                .Bind(_closeButton)
                .To(v => v.CloseCommand);

            bindingSet.Apply();
        }
    }
}