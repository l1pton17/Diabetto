using System;
using System.Reactive;
using System.Reactive.Linq;
using Diabetto.Core.Services;
using Diabetto.Core.ViewModels.Core;
using Diabetto.Core.ViewModels.Settings;
using Diabetto.iOS.Services;
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

        public ReactiveCommand<Unit, Unit> DisableCommand { get; }

        public ReactiveCommand<Unit, bool> EnableCommand { get; }

        public ReactiveCommand<Unit, Unit> ExportCommand { get; }

        public HealthKitSettingsViewModel(
            IDialogService dialogService,
            IHealthKitService healthKitService)
        {
            _isEnabled = healthKitService.GetStatus();

            DisableCommand = ReactiveCommand.Create(() => healthKitService.Disable());

            EnableCommand = ReactiveCommand.CreateFromTask<Unit, bool>(_ => healthKitService.Enable());

            EnableCommand
                .Where(v => !v)
                .Subscribe(isEnabled => IsEnabled = isEnabled);

            EnableCommand.ThrownExceptions
                .Subscribe(e => dialogService.ShowAlert("Error", e.Message));

            DisableCommand.ThrownExceptions
                .Subscribe(e => dialogService.ShowAlert("Error", e.Message));

            var canExport = this
                .WhenAnyValue(v => v.IsEnabled);

            ExportCommand = ReactiveCommand
                .CreateFromTask(
                    healthKitService.Export,
                    canExecute: canExport);

            ExportCommand
                .Subscribe(_ => dialogService.ShowAlert("Success", "Measures have been synchronized"));

            ExportCommand.ThrownExceptions
                .Subscribe(e => dialogService.ShowAlert("Error", e.Message));

            this.WhenAnyValue(v => v.IsEnabled)
                .Where(v => v)
                .Select(_ => Unit.Default)
                .InvokeCommand(this, v => v.EnableCommand);

            this.WhenAnyValue(v => v.IsEnabled)
                .Where(v => !v)
                .Select(_ => Unit.Default)
                .InvokeCommand(this, v => v.DisableCommand);
        }
    }
}