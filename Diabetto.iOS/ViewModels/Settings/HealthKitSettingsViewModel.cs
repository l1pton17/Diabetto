using System.Reactive;
using Diabetto.Core.ViewModels.Core;
using Diabetto.Core.ViewModels.Settings;
using MvvmCross.Binding.Bindings.Source.Leaf;
using ReactiveUI;

namespace Diabetto.iOS.ViewModels.Settings
{
    public sealed class HealthKitSettingsViewModel : MvxReactiveViewModel, ISettingsViewModel
    {
        /// <inheritdoc />
        public string Name => "Health Kit";

        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public ReactiveCommand<Unit, Unit> ExportCommand { get; }

        public HealthKitSettingsViewModel()
        {

        }
    }
}