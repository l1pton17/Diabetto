using System;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace Diabetto.Core.ViewModels.Dialogs
{
    public interface IDialogPickerViewModel
    {
        string Title { get; }

        int ComponentCount { get; }

        string GetRowTitle(int component, int row);
        int GetRowsCount(int component);
        void SelectItem(int component, int row);
    }

    public abstract class DialogPickerViewModel<TItem1, TItem2, TItem3> : ReactiveObject, IDialogPickerViewModel
    {
        protected SourceList<TItem1> Item1Source { get; }

        public IObservableCollection<TItem1> Item1Values { get; }

        protected SourceList<TItem2> Item2Source { get; }

        public IObservableCollection<TItem2> Item2Values { get; }

        protected SourceList<TItem3> Item3Source { get; }

        public IObservableCollection<TItem3> Item3Values { get; }

        private TItem1 _selectedItem1;
        public TItem1 SelectedItem1
        {
            get => _selectedItem1;
            set => this.RaiseAndSetIfChanged(ref _selectedItem1, value);
        }

        private TItem2 _selectedItem2;
        public TItem2 SelectedItem2
        {
            get => _selectedItem2;
            set => this.RaiseAndSetIfChanged(ref _selectedItem2, value);
        }

        private TItem3 _selectedItem3;
        public TItem3 SelectedItem3
        {
            get => _selectedItem3;
            set => this.RaiseAndSetIfChanged(ref _selectedItem3, value);
        }

        public string Title { get; }

        /// <inheritdoc />
        public int ComponentCount => 3;

        protected DialogPickerViewModel(string title)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Item1Source = new SourceList<TItem1>();
            Item2Source = new SourceList<TItem2>();
            Item3Source = new SourceList<TItem3>();
            Item1Values = new ObservableCollectionExtended<TItem1>();
            Item2Values = new ObservableCollectionExtended<TItem2>();
            Item3Values = new ObservableCollectionExtended<TItem3>();

            Item1Source
                .Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(Item1Values)
                .Subscribe();

            Item2Source
                .Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(Item2Values)
                .Subscribe();

            Item3Source
                .Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(Item3Values)
                .Subscribe();
        }

        /// <inheritdoc />
        public string GetRowTitle(int component, int row)
        {
            switch (component)
            {
                case 0:
                    return Item1Values[row].ToString();

                case 1:
                    return Item2Values[row].ToString();

                case 2:
                    return Item3Values[row].ToString();

                default:
                    return String.Empty;
            }
        }

        /// <inheritdoc />
        public int GetRowsCount(int component)
        {
            switch (component)
            {
                case 0:
                    return Item1Values.Count;

                case 1:
                    return Item2Values.Count;

                case 2:
                    return Item3Values.Count;

                default:
                    return 0;
            }
        }

        /// <inheritdoc />
        public void SelectItem(int component, int row)
        {
            switch (component)
            {
                case 0:
                    SelectedItem1 = Item1Values[row];

                    break;

                case 1:
                    SelectedItem2 = Item2Values[row];

                    break;

                case 2:
                    SelectedItem3 = Item3Values[row];

                    break;
            }
        }
    }
}