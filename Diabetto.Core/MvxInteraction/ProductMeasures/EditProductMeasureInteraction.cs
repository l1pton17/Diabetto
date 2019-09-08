using System;
using System.Collections.Generic;
using Diabetto.Core.ViewModels.ProductMeasureUnits;

namespace Diabetto.Core.MvxInteraction.ProductMeasures
{
    public struct EditProductMeasureInteractionResult
    {
        public ProductMeasureUnitViewModel Unit { get; }

        public int Amount { get; }

        public EditProductMeasureInteractionResult(ProductMeasureUnitViewModel unit, int amount)
        {
            Unit = unit ?? throw new ArgumentNullException(nameof(unit));
            Amount = amount;
        }
    }

    public sealed class EditProductMeasureInteraction
    {
        public Action<EditProductMeasureInteractionResult?> Callback { get; set; }

        public List<ProductMeasureUnitViewModel> Units { get; set; }

        public ProductMeasureUnitViewModel SelectedUnit { get; set; }

        public int SelectedAmount { get; set; }
    }
}