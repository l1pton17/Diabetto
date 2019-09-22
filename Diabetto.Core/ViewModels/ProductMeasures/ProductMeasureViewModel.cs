using System;
using System.Reactive.Linq;
using Diabetto.Core.Models;
using Diabetto.Core.Services;
using Diabetto.Core.Services.Repositories;
using Diabetto.Core.ViewModelResults;
using Diabetto.Core.ViewModels.Core;
using Diabetto.Core.ViewModels.ProductMeasureUnits;
using MvvmCross.Logging;
using ReactiveUI;

namespace Diabetto.Core.ViewModels.ProductMeasures
{
    public sealed class ProductMeasureViewModel : MvxReactiveViewModel<ProductMeasure, EmptyResult>
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private int _measureId;
        public int MeasureId
        {
            get => _measureId;
            set => SetProperty(ref _measureId, value);
        }

        private int _amount;
        public int Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        private readonly ObservableAsPropertyHelper<string> _productName;
        public string ProductName => _productName.Value;

        private readonly ObservableAsPropertyHelper<float> _breadUnits;
        public float BreadUnits => _breadUnits.Value;

        public ProductMeasureUnitViewModel Unit { get; }

        public ReactiveCommand<int, string> LoadProductNameCommand { get; }

        public ProductMeasureViewModel(
            IMvxLogProvider logProvider,
            IProductService productService,
            IBreadUnitsCalculator breadUnitsCalculator)
        {
            var logger = logProvider.GetLogFor<ProductMeasureViewModel>();

            Unit = new ProductMeasureUnitViewModel();

            LoadProductNameCommand = ReactiveCommand.CreateFromTask<int, string>(
                async id =>
                {
                    try
                    {
                        var product = await productService.GetAsync(id);

                        return product.Name;
                    }
                    catch (Exception e)
                    {
                        logger.ErrorException("While loading product name", e);

                        return null;
                    }
                });

            Unit.WhenAnyValue(v => v.ProductId)
                .Where(v => v > 0)
                .InvokeCommand(this, v => v.LoadProductNameCommand);

            LoadProductNameCommand
                .Where(v => v != null)
                .ToProperty(this, v => v.ProductName, out _productName);

            this.WhenAnyValue(v => v.Amount)
                .CombineLatest(
                    Unit.WhenAnyValue(v => v.Carbohydrates),
                    (amount, carbs) => breadUnitsCalculator.Calculate(amount, carbs))
                .ToProperty(this, v => v.BreadUnits, out _breadUnits);
        }

        /// <inheritdoc />
        public override void Prepare(ProductMeasure parameter)
        {
            Id = parameter.Id;
            Amount = parameter.Amount;
            MeasureId = parameter.MeasureId;
            Unit.Prepare(parameter.ProductMeasureUnit);
        }

        public ProductMeasure Extract()
        {
            return new ProductMeasure
            {
                Id = Id,
                Amount = Amount,
                MeasureId = MeasureId,
                ProductMeasureUnit = Unit.Extract(),
                ProductMeasureUnitId = Unit.Id
            };
        }
    }
}