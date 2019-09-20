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
        public static readonly int[] GramAmounts = Enumerable
            .Range(1, 500)
            .Select(v => v * 10)
            .ToArray();

        public static readonly int[] Amounts = Enumerable
            .Range(1, 100)
            .Select(v => v)
            .ToArray();

        /// <inheritdoc />
        public SelectProductMeasureUnitPickerViewModel(List<ProductMeasureUnit> units)
            : base("Add")
        {
            this.WhenAnyValue(v => v.SelectedItem2)
                .Where(v => v != null)
                .Subscribe(
                    v =>
                    {
                        Item1Values.Clear();

                        if (v.IsGrams)
                        {
                            Item1Values.AddRange(GramAmounts);
                        }
                        else
                        {
                            Item1Values.AddRange(Amounts);
                        }
                    });

            Item2Values
                .AddRange(units);
        }

        protected override string FormatItem2(ProductMeasureUnit item)
        {
            return item.Name;
        }
    }
}