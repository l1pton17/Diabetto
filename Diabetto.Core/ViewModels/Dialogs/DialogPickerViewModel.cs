﻿using System;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Legacy;
#pragma warning disable 618

namespace Diabetto.Core.ViewModels.Dialogs
{
    public interface IDialogPickerViewModel
    {
        string Title { get; }

        int ComponentCount { get; }

        event EventHandler ItemsChanged;

        string GetRowTitle(int component, int row);
        int GetRowsCount(int component);
        void SelectItem(int component, int row);
    }

    public abstract class DialogPickerViewModel<TItem> : ReactiveObject, IDialogPickerViewModel
    {
        public ReactiveList<TItem> Item1Values { get; }

        private TItem _selectedItem1;
        public TItem SelectedItem1
        {
            get => _selectedItem1;
            set => this.RaiseAndSetIfChanged(ref _selectedItem1, value);
        }

        public string Title { get; }

        /// <inheritdoc />
        public int ComponentCount { get; protected set; }

        /// <inheritdoc />
        public event EventHandler ItemsChanged;

        protected DialogPickerViewModel(string title)
        {
            ComponentCount = 1;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Item1Values = new ReactiveList<TItem>();

            Item1Values
                .ItemsAdded
                .Take(1)
                .Subscribe(v => SelectedItem1 = v);

            Item1Values
                .Changed
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

        protected virtual string FormatItem1(TItem item)
        {
            return item.ToString();
        }

        protected void RaiseItemsChanged()
        {
            ItemsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public abstract class DialogPickerViewModel<TItem1, TItem2> : DialogPickerViewModel<TItem1>
    {
        public ReactiveList<TItem2> Item2Values { get; }

        private TItem2 _selectedItem2;
        public TItem2 SelectedItem2
        {
            get => _selectedItem2;
            set => this.RaiseAndSetIfChanged(ref _selectedItem2, value);
        }

        /// <inheritdoc />
        protected DialogPickerViewModel(string title)
            : base(title)
        {
            ComponentCount = 2;
            Item2Values = new ReactiveList<TItem2>();

            Item2Values
                .ItemsAdded
                .Take(1)
                .Subscribe(v => SelectedItem2 = v);

            Item2Values
                .Changed
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

        protected virtual string FormatItem2(TItem2 item)
        {
            return item.ToString();
        }
    }

    public abstract class DialogPickerViewModel<TItem1, TItem2, TItem3> : DialogPickerViewModel<TItem1, TItem2>
    {
        public ReactiveList<TItem3> Item3Values { get; }

        private TItem3 _selectedItem3;
        public TItem3 SelectedItem3
        {
            get => _selectedItem3;
            set => this.RaiseAndSetIfChanged(ref _selectedItem3, value);
        }

        protected DialogPickerViewModel(string title)
            : base(title)
        {
            ComponentCount = 3;
            Item3Values = new ReactiveList<TItem3>();

            Item3Values
                .ItemsAdded
                .Take(1)
                .Subscribe(v => SelectedItem3 = v);

            Item3Values
                .Changed
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

        protected virtual string FormatItem3(TItem3 item)
        {
            return item.ToString();
        }
    }
}