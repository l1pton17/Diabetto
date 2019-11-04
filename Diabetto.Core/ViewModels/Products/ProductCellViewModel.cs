using System;
using Diabetto.Core.Models;
using Diabetto.Core.ViewModelResults;
using Diabetto.Core.ViewModels.Core;

namespace Diabetto.Core.ViewModels.Products
{
    public sealed class ProductCellViewModel : MvxReactiveViewModel<Product, EmptyResult>
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private int? _glycemicIndex;
        public int? GlycemicIndex
        {
            get => _glycemicIndex;
            set => SetProperty(ref _glycemicIndex, value);
        }

        public ProductCellViewModel(Product product)
        {
            Prepare(product);
        }

        public override void Prepare(Product parameter)
        {
            Id = parameter.Id;
            Name = parameter.Name;
            GlycemicIndex = parameter.GlycemicIndex;
        }
    }
}