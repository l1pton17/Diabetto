using Diabetto.Core.ViewModels.Core;
using Diabetto.Core.ViewModels.Settings;

namespace Diabetto.iOS.ViewModels.Settings
{
    public sealed class HealthKitSettingsViewModel : MvxReactiveViewModel, ISettingsViewModel
    {
        /// <inheritdoc />
        public string Name => "Health Kit";
    }
}