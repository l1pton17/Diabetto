using System.Reactive.Linq;
using Diabetto.Core.Models;
using Diabetto.Core.Services;
using Diabetto.Core.ViewModelResults;
using Diabetto.Core.ViewModels.Core;
using Diabetto.Core.ViewModels.ProductMeasureUnits;
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

        private string _productName;
        public string ProductName
        {
            get => _productName;
            set => SetProperty(ref _productName, value);
        }

        private readonly ObservableAsPropertyHelper<float> _breadUnits;
        public float BreadUnits => _breadUnits.Value;

        public ProductMeasureUnitViewModel Unit { get; }

        public ProductMeasureViewModel(IBreadUnitsCalculator breadUnitsCalculator)
        {
            Unit = new ProductMeasureUnitViewModel();

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
            ProductName = parameter.ProductMeasureUnit.Product.Name;
        }

        public ProductMeasure Extract()
        {
            return new ProductMeasure
            {
                Id = Id,
                Amount = Amount,
                MeasureId = MeasureId,
                ProductMeasureUnitId = Unit.Id
            };
        }
    }
}