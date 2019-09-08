using Diabetto.Core.Models;
using Diabetto.Core.ViewModelResults;
using Diabetto.Core.ViewModels.Core;

namespace Diabetto.Core.ViewModels.ProductMeasureUnits
{
    public sealed class ProductMeasureUnitViewModel : MvxReactiveViewModel<ProductMeasureUnit, EmptyResult>
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private int _productId;
        public int ProductId
        {
            get => _productId;
            set => SetProperty(ref _productId, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _shortName;
        public string ShortName
        {
            get => _shortName;
            set => SetProperty(ref _shortName, value);
        }

        private float _carbohydrates;
        public float Carbohydrates
        {
            get => _carbohydrates;
            set => SetProperty(ref _carbohydrates, value);
        }

        private float? _fats;
        public float? Fats
        {
            get => _fats;
            set => SetProperty(ref _fats, value);
        }

        private float? _proteins;
        public float? Proteins
        {
            get => _proteins;
            set => SetProperty(ref _proteins, value);
        }

        private float? _calories;
        public float? Calories
        {
            get => _calories;
            set => SetProperty(ref _calories, value);
        }

        /// <inheritdoc />
        public override void Prepare(ProductMeasureUnit parameter)
        {
            Id = parameter.Id;
            ProductId = parameter.ProductId;
            Name = parameter.Name;
            ShortName = parameter.ShortName;
            Carbohydrates = parameter.Carbohydrates;
            Fats = parameter.Fats;
            Proteins = parameter.Proteins;
            Calories = parameter.Calories;
        }

        public ProductMeasureUnit Extract()
        {
            return new ProductMeasureUnit
            {
                Id = Id,
                Calories = Calories,
                Carbohydrates = Carbohydrates,
                Fats = Fats,
                Name = Name,
                ShortName = ShortName,
                ProductId = ProductId,
                Proteins = Proteins
            };
        }
    }
}