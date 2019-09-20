using Diabetto.Core;
using Diabetto.Core.Services;
using Diabetto.iOS.MvxBindings;
using Diabetto.iOS.Services;
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