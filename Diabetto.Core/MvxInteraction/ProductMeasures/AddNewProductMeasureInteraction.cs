using System;
using System.Threading.Tasks;

namespace Diabetto.Core.MvxInteraction.ProductMeasures
{
    public sealed class AddNewProductMeasureInteractionResult
    {
        public string ProductName { get; set; }

        public int Carbohydrates { get; set; }

        public int Amount { get; set; }
    }

    public sealed class AddNewProductMeasureInteraction
    {
        public Func<AddNewProductMeasureInteractionResult, Task> Callback { get; set; }

        public string ProductName { get; set; }
    }
}