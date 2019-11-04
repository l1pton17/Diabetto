using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Diabetto.Core.Models;
using Diabetto.Core.Services.Repositories;
using Diabetto.Core.ViewModelResults;
using Diabetto.Core.ViewModels.Core;
using DynamicData;
using MvvmCross.Navigation;
using ReactiveUI;

namespace Diabetto.Core.ViewModels.Products
{
    public sealed class ProductsViewModel : MvxReactiveViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IProductService _productService;

        private string _searchName;
        public string SearchName
        {
            get => _searchName;
            set => SetProperty(ref _searchName, value);
        }

        private readonly ObservableCollection<ProductCellViewModel> _products;
        public IEnumerable<ProductCellViewModel> Products => _products;

        public ReactiveCommand<string, ImmutableArray<ProductCellViewModel>> SearchCommand { get; }

        public ReactiveCommand<ProductCellViewModel, Unit> SelectedCommand { get; }

        public ReactiveCommand<Unit, Unit> AddCommand { get; }

        public ReactiveCommand<ProductCellViewModel, Unit> DeleteCommand { get; }

        public ProductsViewModel(
            IProductService productService,
            IMvxNavigationService navigationService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _products = new ObservableCollection<ProductCellViewModel>();

            AddCommand = ReactiveCommand.CreateFromTask(Add);
            DeleteCommand = ReactiveCommand.CreateFromTask<ProductCellViewModel>(Delete);
            SelectedCommand = ReactiveCommand.CreateFromTask<ProductCellViewModel>(ProductSelected);
            SearchCommand = ReactiveCommand.CreateFromTask<string, ImmutableArray<ProductCellViewModel>>(Search);

            SearchCommand
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    items =>
                    {
                        _products.Clear();
                        _products.AddRange(items);
                    });

            this.WhenAnyValue(v => v.SearchName)
                .Throttle(TimeSpan.FromMilliseconds(300))
                .DistinctUntilChanged()
                .InvokeCommand(this, v => v.SearchCommand);

            SearchName = String.Empty;
        }

        private async Task ProductSelected(ProductCellViewModel cell)
        {
            var product = await _productService.GetAsync(cell.Id);
            var editResult = await _navigationService.Navigate<ProductViewModel, Product, EditResult<Product>>(product);

            if (editResult?.Save == true)
            {
                cell.Prepare(editResult.Entity);
            }
        }

        private Task Add()
        {
            return Task.CompletedTask;
        }

        private async Task Delete(ProductCellViewModel product)
        {
            await _productService.DeleteAsync(product.Id);
            await SearchCommand.Execute(SearchName);
        }

        private async Task<ImmutableArray<ProductCellViewModel>> Search(string query)
        {
            var items = await _productService.GetByNameAsync(query);

            return items
                .Select(v => new ProductCellViewModel(v))
                .ToImmutableArray();
        }
    }
}