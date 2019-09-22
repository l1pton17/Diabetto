using System.Linq;
using Diabetto.Core.Common;
using Diabetto.Core.ViewModels.Dialogs;

namespace Diabetto.Core.ViewModels.ProductMeasures.Dialogs
{
    public sealed class AddProductMeasurePickerViewModel : DialogPickerViewModel<int, string, float>
    {
        /// <inheritdoc />
        public AddProductMeasurePickerViewModel()
            : base("Add")
        {
            Item1Values
                .AddRange(
                    Enumerable
                        .Range(1, 1000)
                        .Select(v => DialogPickerOption.Create(v * 10)));

            Item2Values
                .Add(DialogPickerOption.Create(ProductMeasureUnitConstants.DefaultUnitName));

            Item3Values
                .AddRange(
                    Enumerable
                        .Range(1, 1000)
                        .Select(v => DialogPickerOption.Create(v / 10.0f)));
        }
    }
}