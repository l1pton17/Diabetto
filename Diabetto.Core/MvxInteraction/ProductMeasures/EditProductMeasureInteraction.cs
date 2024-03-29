﻿using System;
using System.Collections.Generic;
using Diabetto.Core.ViewModels.ProductMeasureUnits;

namespace Diabetto.Core.MvxInteraction.ProductMeasures
{
    public sealed class EditProductMeasureInteractionResult
    {
        public ProductMeasureUnitViewModel Unit { get; set; }

        public int Amount { get; set; }
    }

    public sealed class EditProductMeasureInteraction
    {
        public Action<EditProductMeasureInteractionResult> Callback { get; set; }

        public List<ProductMeasureUnitViewModel> Units { get; set; }

        public ProductMeasureUnitViewModel SelectedUnit { get; set; }

        public int SelectedAmount { get; set; }
    }
}