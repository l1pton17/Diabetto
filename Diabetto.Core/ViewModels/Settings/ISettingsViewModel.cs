using System.ComponentModel;

namespace Diabetto.Core.ViewModels.Settings
{
    public interface ISettingsViewModel : INotifyPropertyChanged
    {
        string Name { get; }
    }
}