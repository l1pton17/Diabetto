using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
#pragma warning disable 618

namespace Diabetto.Core.ViewModels.Dialogs
{
    public readonly struct DialogPickerOption<T>
    {
        public bool IsEmpty { get; }

        public T Item { get; }

        public DialogPickerOption(T item, bool isEmpty)
        {
            IsEmpty = isEmpty;
            Item = item;
        }
    }

    public static class DialogPickerOption
    {
        public static DialogPickerOption<T> Create<T>(T item)
        {
            return new DialogPickerOption<T>(item, isEmpty: false);
        }
        
        public static DialogPickerOption<T> Empty<T>()
        {
            return new DialogPickerOption<T>(default, isEmpty: true);
        }
    }

    public sealed class DialogPickerItemSelectedEventArgs : EventArgs
    {
        public int Component { get; }

        public int Row { get; }

        public bool Animated { get; }

        /// <inheritdoc />
        public DialogPickerItemSelectedEventArgs(int component, int row, bool animated = false)
        {
            Component = component;
            Row = row;
            Animated = animated;
        }
    }

    public interface IDialogPickerViewModel
    {
        string Title { get; }

        int ComponentCount { get; }

        IEnumerable<(int Component, int Row)> SelectedItems { get; }

        event EventHandler ItemsChanged;

        string GetRowTitle(int component, int row);
        int GetRowsCount(int component);
        void SelectItem(int component, int row);
    }

    public abstract class DialogPickerViewModel<TItem> : ReactiveObject, IDialogPickerViewModel
    {
        private readonly SourceList<DialogPickerOption<TItem>> _item1ValuesSource;
        private readonly ReadOnlyObservableCollection<DialogPickerOption<TItem>> _item1Values;

        public ReadOnlyObservableCollection<DialogPickerOption<TItem>> Item1Values => _item1Values;

        private DialogPickerOption<TItem> _selectedItem1;
        public DialogPickerOption<TItem> SelectedItem1
        {
            get => _selectedItem1;
            set => this.RaiseAndSetIfChanged(ref _selectedItem1, value);
        }

        public string Title { get; }

        /// <inheritdoc />
        public int ComponentCount { get; protected set; }

        /// <inheritdoc />
        public IEnumerable<(int Component, int Row)> SelectedItems => GetSelectedItems();

        /// <inheritdoc />
        public event EventHandler ItemsChanged;

        protected DialogPickerViewModel(string title)
        {
            ComponentCount = 1;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            _item1ValuesSource = new SourceList<DialogPickerOption<TItem>>();

            _item1ValuesSource
                .Connect()
                .Bind(out _item1Values)
                .DisposeMany()
                .Subscribe();

            _item1ValuesSource
                .Connect()
                .Where(v => v.Adds > 0)
                .Take(1)
                .Subscribe(v => SelectedItem1 = v.First().Item.Current);

            _item1ValuesSource
                .Connect()
                .Subscribe(_ => RaiseItemsChanged());
        }

        /// <inheritdoc />
        public virtual string GetRowTitle(int component, int row)
        {
            switch (component)
            {
                case 0:
                    return FormatItem1(Item1Values[row]);

                default:
                    return String.Empty;
            }
        }

        /// <inheritdoc />
        public virtual int GetRowsCount(int component)
        {
            switch (component)
            {
                case 0:
                    return Item1Values.Count;

                default:
                    return 0;
            }
        }

        /// <inheritdoc />
        public virtual void SelectItem(int component, int row)
        {
            switch (component)
            {
                case 0:
                    SelectedItem1 = Item1Values[row];

                    break;
            }
        }

        protected void ClearItem1Values()
        {
            _item1ValuesSource.Clear();
        }

        protected void AddItem1Values(DialogPickerOption<TItem>[] items)
        {
            _item1ValuesSource.AddRange(items);
        }

        protected void AddItem1Values(IEnumerable<DialogPickerOption<TItem>> items)
        {
            _item1ValuesSource.AddRange(items);
        }

        protected virtual IEnumerable<(int Component, int Row)> GetSelectedItems()
        {
            var idx = Item1Values.IndexOf(SelectedItem1);

            if (idx >= 0)
            {
                yield return (0, idx);
            }
        }

        protected virtual string FormatItem1(DialogPickerOption<TItem> item)
        {
            return FormatItem(item);
        }

        protected string FormatItem<T>(DialogPickerOption<T> value)
        {
            if (value.IsEmpty)
            {
                return "Not selected";
            }

            return value.Item.ToString();
        }

        protected void RaiseItemsChanged()
        {
            ItemsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public abstract class DialogPickerViewModel<TItem1, TItem2> : DialogPickerViewModel<TItem1>
    {
        private readonly SourceList<DialogPickerOption<TItem2>> _item2ValuesSource;
        private readonly ReadOnlyObservableCollection<DialogPickerOption<TItem2>> _item2Values;
        public ReadOnlyObservableCollection<DialogPickerOption<TItem2>> Item2Values => _item2Values;

        private DialogPickerOption<TItem2> _selectedItem2;
        public DialogPickerOption<TItem2> SelectedItem2
        {
            get => _selectedItem2;
            set => this.RaiseAndSetIfChanged(ref _selectedItem2, value);
        }

        /// <inheritdoc />
        protected DialogPickerViewModel(string title)
            : base(title)
        {
            ComponentCount = 2;
            _item2ValuesSource = new SourceList<DialogPickerOption<TItem2>>();

            _item2ValuesSource
                .Connect()
                .Bind(out _item2Values)
                .DisposeMany()
                .Subscribe();

            _item2ValuesSource
                .Connect()
                .Where(v => v.Adds > 0)
                .Take(1)
                .Subscribe(v => SelectedItem2 = v.First().Item.Current);

            _item2ValuesSource
                .Connect()
                .Subscribe(_ => RaiseItemsChanged());
        }

        /// <inheritdoc />
        public override string GetRowTitle(int component, int row)
        {
            switch (component)
            {
                case 0:
                    return FormatItem1(Item1Values[row]);

                case 1:
                    return FormatItem2(Item2Values[row]);

                default:
                    return String.Empty;
            }
        }

        /// <inheritdoc />
        public override int GetRowsCount(int component)
        {
            switch (component)
            {
                case 0:
                    return Item1Values.Count;

                case 1:
                    return Item2Values.Count;

                default:
                    return 0;
            }
        }

        /// <inheritdoc />
        public override void SelectItem(int component, int row)
        {
            switch (component)
            {
                case 0:
                    SelectedItem1 = Item1Values[row];

                    break;

                case 1:
                    SelectedItem2 = Item2Values[row];

                    break;
            }
        }

        protected void ClearItem2Values()
        {
            _item2ValuesSource.Clear();
        }

        protected void AddItem2Values(params DialogPickerOption<TItem2>[] items)
        {
            _item2ValuesSource.AddRange(items);
        }


        protected void AddItem2Values(IEnumerable<DialogPickerOption<TItem2>> items)
        {
            _item2ValuesSource.AddRange(items);
        }

        protected override IEnumerable<(int Component, int Row)> GetSelectedItems()
        {
            var idx = Item2Values.IndexOf(SelectedItem2);

            if (idx >= 0)
            {
                return base
                    .GetSelectedItems()
                    .Append((1, idx));
            }

            return base.GetSelectedItems();
        }

        protected virtual string FormatItem2(DialogPickerOption<TItem2> item)
        {
            return FormatItem(item);
        }
    }

    public abstract class DialogPickerViewModel<TItem1, TItem2, TItem3> : DialogPickerViewModel<TItem1, TItem2>
    {
        private readonly SourceList<DialogPickerOption<TItem3>> _item3ValuesSource;
        private readonly ReadOnlyObservableCollection<DialogPickerOption<TItem3>> _item3Values;

        public ReadOnlyObservableCollection<DialogPickerOption<TItem3>> Item3Values => _item3Values;

        private DialogPickerOption<TItem3> _selectedItem3;
        public DialogPickerOption<TItem3> SelectedItem3
        {
            get => _selectedItem3;
            set => this.RaiseAndSetIfChanged(ref _selectedItem3, value);
        }

        protected DialogPickerViewModel(string title)
            : base(title)
        {
            ComponentCount = 3;
            _item3ValuesSource = new SourceList<DialogPickerOption<TItem3>>();

            _item3ValuesSource
                .Connect()
                .Bind(out _item3Values)
                .DisposeMany()
                .Subscribe();

            _item3ValuesSource
                .Connect()
                .Where(v => v.Adds > 0)
                .Take(1)
                .Subscribe(v => SelectedItem3 = v.First().Item.Current);

            _item3ValuesSource
                .Connect()
                .Subscribe(_ => RaiseItemsChanged());
        }

        /// <inheritdoc />
        public override string GetRowTitle(int component, int row)
        {
            switch (component)
            {
                case 0:
                    return FormatItem1(Item1Values[row]);

                case 1:
                    return FormatItem2(Item2Values[row]);

                case 2:
                    return FormatItem3(Item3Values[row]);

                default:
                    return String.Empty;
            }
        }

        /// <inheritdoc />
        public override int GetRowsCount(int component)
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
        public override void SelectItem(int component, int row)
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

        protected void ClearItem3Values()
        {
            _item3ValuesSource.Clear();
        }

        protected void AddItem3Values(DialogPickerOption<TItem3>[] items)
        {
            _item3ValuesSource.AddRange(items);
        }

        protected void AddItem3Values(IEnumerable<DialogPickerOption<TItem3>> items)
        {
            _item3ValuesSource.AddRange(items);
        }

        protected override IEnumerable<(int Component, int Row)> GetSelectedItems()
        {
            var idx = Item3Values.IndexOf(SelectedItem3);

            if (idx >= 0)
            {
                return base
                    .GetSelectedItems()
                    .Append((2, idx));
            }

            return base.GetSelectedItems();
        }

        protected virtual string FormatItem3(DialogPickerOption<TItem3> item)
        {
            return FormatItem(item);
        }
    }
}