using System;
using Diabetto.Core.Models;
using Diabetto.Core.Services.Repositories;
using Diabetto.Core.ViewModelResults;
using Diabetto.Core.ViewModels.Core;

namespace Diabetto.Core.ViewModels.Products
{
    public sealed class ProductViewModel : MvxReactiveViewModel<Product, EditResult<Product>>
    {
        private readonly IProductService _productService;

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

        private int _glycemicIndex;
        public int GlycemicIndex
        {
            get => _glycemicIndex;
            set => SetProperty(ref _glycemicIndex, value);
        }

        private bool _hasGlycemicIndex;
        public bool HasGlycemicIndex
        {
            get => _hasGlycemicIndex;
            set => SetProperty(ref _hasGlycemicIndex, value);
        }

        public ProductViewModel(
            IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        public override void Prepare(Product parameter)
        {
            Id = parameter?.Id ?? 0;
            Name = parameter?.Name ?? String.Empty;
            HasGlycemicIndex = parameter?.GlycemicIndex.HasValue ?? false;
            GlycemicIndex = parameter?.GlycemicIndex ?? 0;
        }
    }
}