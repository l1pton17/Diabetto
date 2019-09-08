using System;
using System.Collections.Generic;

namespace Diabetto.Core.Dialogs
{
    public abstract class PickerDialogSource<TItem>
    {
        protected List<TItem> Items { get; }

        public TItem SelectedItem { get; private set; }

        protected PickerDialogSource(List<TItem> items)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }

        protected abstract string GetTitle(TItem item);
        protected abstract int GetId(TItem item);
    }
}