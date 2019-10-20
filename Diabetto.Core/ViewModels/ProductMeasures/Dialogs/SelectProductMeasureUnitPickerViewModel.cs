using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Diabetto.Core.Models;
using Diabetto.Core.ViewModels.Dialogs;
using ReactiveUI;

namespace Diabetto.Core.ViewModels.ProductMeasures.Dialogs
{
    public sealed class SelectProductMeasureUnitPickerViewModel : DialogPickerViewModel<int, ProductMeasureUnit>
    {
        public static readonly DialogPickerOption<int>[] GramAmounts = Enumerable
            .Range(1, 500)
            .Select(v => DialogPickerOption.Create(v * 10))
            .ToArray();

        public static readonly DialogPickerOption<int>[] Amounts = Enumerable
            .Range(1, 100)
            .Select(v => DialogPickerOption.Create(v))
            .ToArray();

        /// <inheritdoc />
        public SelectProductMeasureUnitPickerViewModel(List<ProductMeasureUnit> units)
            : base("Add")
        {
            this.WhenAnyValue(v => v.SelectedItem2)
                .Where(v => v.Item != null)
                .Select(v => v.Item)
                .Subscribe(
                    v =>
                    {
                        ClearItem1Values();

                        if (v.IsGrams)
                        {
                            AddItem1Values(GramAmounts);
                        }
                        else
                        {
                            AddItem1Values(Amounts);
                        }
                    });

            AddItem2Values(units.Select(v => DialogPickerOption.Create(v)));
        }

        protected override string FormatItem2(DialogPickerOption<ProductMeasureUnit> item)
        {
            return item.Item.Name;
        }
    }
}