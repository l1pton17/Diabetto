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
            AddItem1Values(
                    Enumerable
                        .Range(1, 1000)
                        .Select(v => DialogPickerOption.Create(v * 10)));

            AddItem2Values(DialogPickerOption.Create(ProductMeasureUnitConstants.DefaultUnitName));

            AddItem3Values(
                    Enumerable
                        .Range(1, 1000)
                        .Select(v => DialogPickerOption.Create(v / 10.0f)));
        }
    }
}