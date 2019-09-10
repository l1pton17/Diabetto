using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Diabetto.Core.ViewModels.ProductMeasureUnits;

namespace Diabetto.Core.MvxInteraction.ProductMeasures
{
    public sealed class AddProductMeasureInteractionResult
    {
        public int ProductId { get; set; }

        public ProductMeasureUnitViewModel Unit { get; set; }

        public int Amount { get; set; }
    }

    public sealed class AddProductMeasureInteraction
    {
        public Func<AddProductMeasureInteractionResult, Task> Callback { get; set; }

        public List<ProductMeasureUnitViewModel> Units { get; set; }

        public int ProductId { get; set; }
    }
}