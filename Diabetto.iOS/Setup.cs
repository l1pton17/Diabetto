using Diabetto.Core;
using Diabetto.Core.Services;
using Diabetto.iOS.Intents.Shared;
using Diabetto.iOS.MvxBindings;
using Diabetto.iOS.Services;
using Diabetto.iOS.ViewModels.Settings;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.IoC;
using MvvmCross.Platforms.Ios.Binding.Target;
using MvvmCross.Platforms.Ios.Core;
using UIKit;

namespace Diabetto.iOS
{
    public sealed class Setup : MvxIosSetup<App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();
            Mvx.IoCProvider.RegisterType<IDialogService, DialogService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IDatabaseConnectionStringHolder, SharedDatabaseConnectionStringHolder>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IHealthKitService, HealthKitService>();
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();

            var registry = Mvx.IoCProvider.Resolve<IMvxTargetBindingFactoryRegistry>();
            registry.RegisterFactory(
                new MvxCustomBindingFactory<UIViewController>(
                    "NetworkIndicator",
                    viewController => new NetworkIndicatorTargetBinding(viewController)));

            var settingViewModelStorage = Mvx.IoCProvider.Resolve<ISettingViewModelsStorage>();
            var healthKitSettingsViewModel = Mvx.IoCProvider.IoCConstruct<HealthKitSettingsViewModel>();

            settingViewModelStorage.AddOption(healthKitSettingsViewModel);

            Mvx.IoCProvider.ConstructAndRegisterSingleton<IAddMeasureIntentDonationManager, AddMeasureIntentDonationManager>();
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterCustomBindingFactory<UISwitch>(
                "On",
                uiSwitch => new MvxUISwitchOnTargetBinding(uiSwitch)
            );
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