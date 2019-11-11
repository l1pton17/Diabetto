using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Diabetto.Core.Models;
using Diabetto.Core.Services;
using Diabetto.Core.Services.Repositories;
using Diabetto.Core.ViewModels.Core;
using Diabetto.Core.ViewModels.ProductMeasures.Dialogs;
using DynamicData;
using DynamicData.Binding;
using MvvmCross.Navigation;
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

        private string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            set => SetProperty(ref _categoryName, value);
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
        private readonly IMvxNavigationService _navigationService;
        private readonly IDialogService _dialogService;

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

        public IObservableCollection<ProductSearchItemViewModel> SearchResults { get; }

        public ReactiveCommand<string, ImmutableArray<ProductSearchItemViewModel>> SearchCommand { get; }

        public ReactiveCommand<ProductSearchItemViewModel, Unit> ProductSelectedCommand { get; }

        public ReactiveCommand<Unit, bool> CloseCommand { get; }

        public AddProductMeasureViewModel(
            IMvxNavigationService navigationService,
            IProductService productService,
            IDialogService dialogService
        )
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            var searchResults = new SourceList<ProductSearchItemViewModel>();
            SearchResults = new ObservableCollectionExtended<ProductSearchItemViewModel>();

            searchResults
                .Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(SearchResults)
                .Subscribe();

            CloseCommand = ReactiveCommand.CreateFromTask(Close);

            SearchCommand = ReactiveCommand.CreateFromTask<string, ImmutableArray<ProductSearchItemViewModel>>(Search);

            SearchCommand
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    items =>
                    {
                        searchResults.Clear();
                        searchResults.AddRange(items);
                    });

            this.WhenAnyValue(v => v.SearchQuery)
                .Throttle(TimeSpan.FromMilliseconds(300))
                .DistinctUntilChanged()
                .InvokeCommand(this, v => v.SearchCommand);

            ProductSelectedCommand = ReactiveCommand.CreateFromTask<ProductSearchItemViewModel>(ProductSelected);

            SearchQuery = String.Empty;
        }

        /// <inheritdoc />
        public override void Prepare(AddProductMeasureParameter parameter)
        {
            MeasureId = parameter.MeasureId;
        }

        private async Task<bool> Close()
        {
            return await _navigationService.Close(this, null);
        }

        private async Task ProductSelected(ProductSearchItemViewModel item)
        {
            if (item.IsNew)
            {
                var source = new AddProductMeasurePickerViewModel();

                var isOk = await _dialogService.ShowPicker(source);

                var isInvalidSelection = source.SelectedItem1.Item == 0
                    || source.SelectedItem2.Item == null;

                if (!isOk || isInvalidSelection)
                {
                    return;
                }

                var product = new Product
                {
                    Name = item.Name,
                    Units = new List<ProductMeasureUnit>
                    {
                        new ProductMeasureUnit
                        {
                            IsGrams = true,
                            Name = source.SelectedItem2.Item,
                            ShortName = source.SelectedItem2.Item[0].ToString(),
                            Carbohydrates = source.SelectedItem3.Item
                        }
                    }
                };

                await _productService.AddAsync(product);

                var unit = product.Units[0];

                var productMeasure = new ProductMeasure
                {
                    Amount = source.SelectedItem1.Item,
                    ProductMeasureUnitId = unit.Id,
                    ProductMeasureUnit = unit,
                    MeasureId = MeasureId
                };

                await _navigationService.Close(this, productMeasure);
            }
            else
            {
                var product = await _productService.GetAsync(item.Id);
                var source = new SelectProductMeasureUnitPickerViewModel(product.Units);

                var isOk = await _dialogService.ShowPicker(source);

                var isInvalidSelection = source.SelectedItem1.Item == 0
                    || source.SelectedItem2.Item == null;

                if (!isOk || isInvalidSelection)
                {
                    return;
                }

                var productMeasure = new ProductMeasure
                {
                    Amount = source.SelectedItem1.Item,
                    ProductMeasureUnitId = source.SelectedItem2.Item.Id,
                    ProductMeasureUnit = source.SelectedItem2.Item,
                    MeasureId = MeasureId
                };

                await _navigationService.Close(this, productMeasure);
            }
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
                        CategoryName = v.ProductCategory?.Name ?? String.Empty,
                        IsNew = false
                    });

            if (!hasExactMatch && !String.IsNullOrWhiteSpace(query))
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