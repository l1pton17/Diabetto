using System;
using Diabetto.Core.Services;
using Diabetto.Core.Services.Repositories;
using MvvmCross.Logging;

namespace Diabetto.Core.ViewModels.ProductMeasures
{
    public interface IProductMeasureViewModelFactory
    {
        ProductMeasureViewModel Create();
    }
    
    internal sealed class ProductMeasureViewModelFactory : IProductMeasureViewModelFactory
    {
        private readonly IBreadUnitsCalculator _breadUnitsCalculator;
        private readonly IProductService _productService;
        private readonly IMvxLogProvider _logProvider;

        public ProductMeasureViewModelFactory(
            IBreadUnitsCalculator breadUnitsCalculator,
            IProductService productService,
            IMvxLogProvider logProvider
        )
        {
            _breadUnitsCalculator = breadUnitsCalculator ?? throw new ArgumentNullException(nameof(breadUnitsCalculator));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _logProvider = logProvider ?? throw new ArgumentNullException(nameof(logProvider));
        }

        /// <inheritdoc />
        public ProductMeasureViewModel Create()
        {
            return new ProductMeasureViewModel(_logProvider, _productService, _breadUnitsCalculator);
        }
    }
}