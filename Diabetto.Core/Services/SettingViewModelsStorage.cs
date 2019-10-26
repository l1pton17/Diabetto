using System.Collections.Generic;
using System.Collections.ObjectModel;
using Diabetto.Core.ViewModels.Settings;
#pragma warning disable 618

namespace Diabetto.Core.Services
{
    public interface ISettingViewModelsStorage
    {
        IEnumerable<ISettingsViewModel> Options { get; }

        void AddOption(ISettingsViewModel viewModel);
    }

    internal sealed class SettingViewModelsStorage : ISettingViewModelsStorage
    {
        private readonly ObservableCollection<ISettingsViewModel> _options;

        /// <inheritdoc />
        public IEnumerable<ISettingsViewModel> Options => _options;

        /// <inheritdoc />
        public void AddOption(ISettingsViewModel viewModel)
        {
            _options.Add(viewModel);
        }

        public SettingViewModelsStorage()
        {
            _options = new ObservableCollection<ISettingsViewModel>();
        }
    }
}