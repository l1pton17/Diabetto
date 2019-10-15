using Diabetto.Core.ViewModels.Settings;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace Diabetto.iOS.Views.Settings
{
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Settings")]
    public sealed class SettingsHolderView : MvxTableViewController<SettingsHolderViewModel>
    {
        private MvxStandardTableViewSource _source;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Settings";

            TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            _source = new MvxStandardTableViewSource(
                TableView,
                UITableViewCellStyle.Default,
                new NSString("SettingsOptionItem"),
                "TitleText Name");

            TableView.Source = _source;

            var bindingSet = this.CreateBindingSet<SettingsHolderView, SettingsHolderViewModel>();

            bindingSet
                .Bind(_source)
                .For(v => v.ItemsSource)
                .To(vm => vm.Options);

            bindingSet
                .Bind(_source)
                .For(v => v.SelectionChangedCommand)
                .To(vm => vm.OptionSelectedCommand);

            bindingSet.Apply();
        }
    }
}