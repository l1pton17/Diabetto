using System.Linq;
using Diabetto.Core.ViewModels.Dialogs;
using DynamicData;

namespace Diabetto.Core.ViewModels.ProductMeasures.Dialogs
{
    public sealed class AddProductMeasurePickerViewModel : DialogPickerViewModel<int, string, float>
    {
        /// <inheritdoc />
        public AddProductMeasurePickerViewModel()
            : base("Add")
        {
            Item1Source
                .AddRange(
                    Enumerable
                        .Range(1, 1000)
                        .Select(v => v * 10));

            Item2Source.Add("grams");

            Item3Source
                .AddRange(
                    Enumerable
                        .Range(1, 1000)
                        .Select(v => v / 10.0f));
        }
    }
}