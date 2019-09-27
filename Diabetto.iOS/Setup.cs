using Diabetto.Core;
using Diabetto.Core.Services;
using Diabetto.iOS.MvxBindings;
using Diabetto.iOS.Services;
using Diabetto.iOS.ViewModels.Settings;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.IoC;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.Plugin.Json;
using UIKit;

namespace Diabetto.iOS
{
    public sealed class Setup : MvxIosSetup<App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();
            Mvx.IoCProvider.RegisterType<IMvxJsonConverter, MvxJsonConverter>();
            Mvx.IoCProvider.RegisterType<IDialogService, DialogService>();

            var settingViewModelStorage = Mvx.IoCProvider.Resolve<ISettingViewModelsStorage>();
            var healthKitSettingsViewModel = Mvx.IoCProvider.IoCConstruct<HealthKitSettingsViewModel>();

            settingViewModelStorage.Options.Add(healthKitSettingsViewModel);
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();

            var registry = Mvx.IoCProvider.Resolve<IMvxTargetBindingFactoryRegistry>();
            registry.RegisterFactory(
                new MvxCustomBindingFactory<UIViewController>(
                    "NetworkIndicator",
                    viewController => new NetworkIndicatorTargetBinding(viewController)));
        }

        protected override IMvxIocOptions CreateIocOptions()
        {
            return new MvxIocOptions
            {
                PropertyInjectorOptions = MvxPropertyInjectorOptions.MvxInject
            };
        }
    }
}