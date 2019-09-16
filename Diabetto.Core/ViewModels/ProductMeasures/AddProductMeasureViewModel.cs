using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Diabetto.Core.Models;
using Diabetto.Core.MvxInteraction.ProductMeasures;
using Diabetto.Core.Services.Repositories;
using Diabetto.Core.ViewModels.Core;
using Diabetto.Core.ViewModels.ProductMeasureUnits;
using DynamicData;
using DynamicData.Binding;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReactiveUI;

namespace Diabetto.Core.ViewModels.ProductMeasures
{
    public sealed class AddProductMeasureParameter
    {
        public int MeasureId { get; }

        public AddProductMeasureParameter(int measureId)
        {
            MeasureId = measureId;
        }
    }

    public sealed class ProductSearchItemViewModel : MvxReactiveViewModel
    {
        private bool _isNew;
        public bool IsNew
        {
            get => _isNew;
            set => SetProperty(ref _isNew, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
    }

    public sealed class AddProductMeasureViewModel : MvxReactiveViewModel<AddProductMeasureParameter, ProductMeasure>
    {
        private readonly IProductService _productService;
        private readonly IProductMeasureUnitService _productMeasureUnitService;
        private readonly IMvxNavigationService _navigationService;

        private int _measureId;
        public int MeasureId
        {
            get => _measureId;
            set => SetProperty(ref _measureId, value);
        }

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set => SetProperty(ref _searchQuery, value);
        }

        private readonly MvxInteraction<AddNewProductMeasureInteraction> _addNewInteraction;
        public IMvxInteraction<AddNewProductMeasureInteraction> AddNewInteraction => _addNewInteraction;

        private readonly MvxInteraction<AddProductMeasureInteraction> _addInteraction;
        public IMvxInteraction<AddProductMeasureInteraction> AddInteraction => _addInteraction;

        private readonly SourceList<ProductSearchItemViewModel> _searchResults;
        public IObservableCollection<ProductSearchItemViewModel> SearchResults { get; }

        public ReactiveCommand<string, ImmutableArray<ProductSearchItemViewModel>> SearchCommand { get; }

        public ReactiveCommand<ProductSearchItemViewModel, Unit> ProductSelectedCommand { get; }

        public AddProductMeasureViewModel(
            IMvxNavigationService navigationService,
            IProductService productService,
            IProductMeasureUnitService productMeasureUnitService
        )
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _productMeasureUnitService = productMeasureUnitService ?? throw new ArgumentNullException(nameof(productMeasureUnitService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _addNewInteraction = new MvxInteraction<AddNewProductMeasureInteraction>();
            _addInteraction = new MvxInteraction<AddProductMeasureInteraction>();

            _searchResults = new SourceList<ProductSearchItemViewModel>();
            SearchResults = new ObservableCollectionExtended<ProductSearchItemViewModel>();

            _searchResults
                .Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(SearchResults)
                .Subscribe();

            SearchCommand = ReactiveCommand.CreateFromTask<string, ImmutableArray<ProductSearchItemViewModel>>(Search);

            SearchCommand
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    items =>
                    {
                        _searchResults.Clear();
                        _searchResults.AddRange(items);
                    });

            this.WhenAnyValue(v => v.SearchQuery)
                .Throttle(TimeSpan.FromMilliseconds(300))
                .DistinctUntilChanged()
                .InvokeCommand(this, v => v.SearchCommand);

            ProductSelectedCommand = ReactiveCommand.CreateFromTask<ProductSearchItemViewModel>(ProductSelected);
        }

        /// <inheritdoc />
        public override void Prepare(AddProductMeasureParameter parameter)
        {
            MeasureId = parameter.MeasureId;
        }

        private async Task ProductSelected(ProductSearchItemViewModel item)
        {
            if (item.IsNew)
            {
                var interaction = new AddNewProductMeasureInteraction
                {
                    ProductName = item.Name,
                    Callback = AddNewInteractionCompleted
                };

                _addNewInteraction.Raise(interaction);
            }
            else
            {
                var units = await _productMeasureUnitService.GetByProductId(item.Id);

                var unitViewModels = units
                    .Select(
                        u =>
                        {
                            var viewModel = new ProductMeasureUnitViewModel();
                            viewModel.Prepare(u);

                            return viewModel;
                        })
                    .ToList();

                var interaction = new AddProductMeasureInteraction
                {
                    Units = unitViewModels,
                    ProductId = item.Id,
                    Callback = AddInteractionCompleted
                };

                _addInteraction.Raise(interaction);
            }
        }

        private async Task AddInteractionCompleted(AddProductMeasureInteractionResult result)
        {
            if (result == null)
            {
                return;
            }

            var productMeasure = new ProductMeasure
            {
                Amount = result.Amount,
                ProductMeasureUnitId = result.Unit.Id,
                ProductMeasureUnit = result.Unit.Extract(),
                MeasureId = MeasureId
            };

            await _navigationService.Close(this, productMeasure);
        }

        private async Task AddNewInteractionCompleted(AddNewProductMeasureInteractionResult result)
        {
            if (result == null)
            {
                return;
            }

            var product = new Product
            {
                Name = result.ProductName,
                Units = new List<ProductMeasureUnit>
                {
                    new ProductMeasureUnit
                    {
                        Name = "grams",
                        ShortName = "gr",
                        Carbohydrates = result.Carbohydrates
                    }
                }
            };

            await _productService.AddAsync(product);

            var unit = product.Units[0];

            var productMeasure = new ProductMeasure
            {
                Amount = result.Amount,
                ProductMeasureUnitId = unit.Id,
                ProductMeasureUnit = unit,
                MeasureId = MeasureId
            };

            await _navigationService.Close(this, productMeasure);
        }

        private async Task<ImmutableArray<ProductSearchItemViewModel>> Search(string query)
        {
            var items = await _productService.GetByNameAsync(query);

            var hasExactMatch = items.Any(v => v.Name.Equals(query, StringComparison.OrdinalIgnoreCase));

            var searchItems = items
                .Select(
                    v => new ProductSearchItemViewModel
                    {
                        Id = v.Id,
                        Name = v.Name,
                        IsNew = false
                    });

            if (!hasExactMatch)
            {
                searchItems = searchItems
                    .Prepend(
                        new ProductSearchItemViewModel
                        {
                            Name = query,
                            IsNew = true
                        });
            }

            return searchItems.ToImmutableArray();
        }
    }
}