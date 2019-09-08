using Diabetto.Core.Models;
using Diabetto.Core.ViewModels.Core;

namespace Diabetto.Core.ViewModels.ProductMeasures
{
    public sealed class AddProductMeasureParameter
    {
        public int MeasureId { get; }

        public AddProductMeasureParameter(int measureId)
        {
            MeasureId = measureId;
        }
    }

    public sealed class AddProductMeasureViewModel : MvxReactiveViewModel<AddProductMeasureParameter, ProductMeasure>
    {
        /// <inheritdoc />
        public override void Prepare(AddProductMeasureParameter parameter)
        {
            throw new System.NotImplementedException();
        }
    }
}