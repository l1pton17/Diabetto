using System;
using Diabetto.Core.Services;

namespace Diabetto.Core.ViewModels.ProductMeasures
{
    public interface IProductMeasureViewModelFactory
    {
        ProductMeasureViewModel Create();
    }
    
    internal sealed class ProductMeasureViewModelFactory : IProductMeasureViewModelFactory
    {
        private readonly IBreadUnitsCalculator _breadUnitsCalculator;

        public ProductMeasureViewModelFactory(IBreadUnitsCalculator breadUnitsCalculator)
        {
            _breadUnitsCalculator = breadUnitsCalculator ?? throw new ArgumentNullException(nameof(breadUnitsCalculator));
        }

        /// <inheritdoc />
        public ProductMeasureViewModel Create()
        {
            return new ProductMeasureViewModel(_breadUnitsCalculator);
        }
    }
}