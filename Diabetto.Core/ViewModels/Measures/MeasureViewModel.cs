using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Diabetto.Core.Models;
using Diabetto.Core.MvxInteraction.ProductMeasures;
using Diabetto.Core.Services;
using Diabetto.Core.ViewModelResults;
using Diabetto.Core.ViewModels.Core;
using Diabetto.Core.ViewModels.ProductMeasures;
using Diabetto.Core.ViewModels.ProductMeasureUnits;
using DynamicData;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReactiveUI;

namespace Diabetto.Core.ViewModels.Measures
{
    public class MeasureViewModel : MvxReactiveViewModel<Measure, EditResult<Measure>>
    {
        private readonly IProductMeasureService _productMeasureService;
        private readonly IProductMeasureUnitService _productMeasureUnitService;
        private readonly IMvxNavigationService _navigationService;

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        private Tag _tag;
        public Tag Tag
        {
            get => _tag;
            set => SetProperty(ref _tag, value);
        }

        private List<Tag> _tags;
        public List<Tag> Tags
        {
            get => _tags;
            set => SetProperty(ref _tags, value);
        }

        private int _level;
        public int Level
        {
            get => _level;
            set => SetProperty(ref _level, value);
        }

        private bool _hasLevel;
        public bool HasLevel
        {
            get => _hasLevel;
            set => SetProperty(ref _hasLevel, value);
        }

        private readonly ObservableAsPropertyHelper<int?> _nullableLevel;
        public int? NullableLevel => _nullableLevel.Value;

        private int _longInsulin;
        public int LongInsulin
        {
            get => _longInsulin;
            set => SetProperty(ref _longInsulin, value);
        }

        private int _shortInsulin;
        public int ShortInsulin
        {
            get => _shortInsulin;
            set => SetProperty(ref _shortInsulin, value);
        }

        private float _breadUnits;
        public float BreadUnits
        {
            get => _breadUnits;
            set => SetProperty(ref _breadUnits, value);
        }

        public SourceList<ProductMeasureViewModel> ProductMeasures { get; }

        public ReactiveCommand<Unit, Unit> AddProductMeasureCommand { get; }

        public ReactiveCommand<ProductMeasureViewModel, Unit> DeleteProductMeasureCommand { get; }

        public ReactiveCommand<ProductMeasureViewModel, Unit> ProductMeasureSelectedCommand { get; }

        public ICommand SaveCommand { get; }

        private readonly MvxInteraction<EditProductMeasureInteraction> _editProductMeasureInteraction;

        public IMvxInteraction<EditProductMeasureInteraction> EditProductMeasureInteraction => _editProductMeasureInteraction;

        public MeasureViewModel(
            IMvxNavigationService navigationService,
            IProductMeasureService productMeasureService,
            IProductMeasureUnitService productMeasureUnitService
        )
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _productMeasureService = productMeasureService ?? throw new ArgumentNullException(nameof(productMeasureService));
            _productMeasureUnitService = productMeasureUnitService ?? throw new ArgumentNullException(nameof(productMeasureUnitService));

            _editProductMeasureInteraction = new MvxInteraction<EditProductMeasureInteraction>();

            this.WhenAnyValue(v => v.Level)
                .Where(v => v > 0)
                .Subscribe(_ => HasLevel = true);

            this.WhenAnyValue(v => v.Level, v => v.HasLevel)
                .Select(v => v.Item2 ? v.Item1 : (int?) null)
                .ToProperty(this, v => v.NullableLevel, out _nullableLevel);

            Tags = new List<Tag>();

            ProductMeasures = new SourceList<ProductMeasureViewModel>();

            SaveCommand = ReactiveCommand
                .CreateFromTask(
                    () => navigationService.Close(
                        this,
                        EditResult.Create(
                            new Measure
                            {
                                Id = Id,
                                Date = Date,
                                Level = HasLevel ? (int?) Level : null,
                                LongInsulin = LongInsulin,
                                ShortInsulin = ShortInsulin,
                                BreadUnits = BreadUnits,
                                Products = ProductMeasures.Items
                                    .Select(v => v.Extract())
                                    .ToList()
                            })));

            AddProductMeasureCommand = ReactiveCommand.CreateFromTask(AddProductMeasure);

            DeleteProductMeasureCommand = ReactiveCommand
                .Create<ProductMeasureViewModel>(v => ProductMeasures.Remove(v));

            ProductMeasureSelectedCommand = ReactiveCommand
                .CreateFromTask<ProductMeasureViewModel>(ProductMeasureSelected);

        }

        private async Task AddProductMeasure()
        {
            var result = await _navigationService.Navigate<AddProductMeasureViewModel, AddProductMeasureParameter, ProductMeasure>(
                new AddProductMeasureParameter(Id));

            if (result == null)
            {
                return;
            }

            var itemViewModel = new ProductMeasureViewModel();

            itemViewModel.Prepare(result);

            ProductMeasures.Add(itemViewModel);
        }

        private async Task ProductMeasureSelected(ProductMeasureViewModel productMeasure)
        {
            var units = await _productMeasureUnitService.GetByProductId(productMeasure.Unit.ProductId);

            var unitViewModels = units
                .Select(
                    u =>
                    {
                        var viewModel = new ProductMeasureUnitViewModel();
                        viewModel.Prepare(u);

                        return viewModel;
                    })
                .ToList();

            var selectedUnit = unitViewModels.First(u => u.Id == productMeasure.Id);
            var selectedAmount = productMeasure.Amount;

            var interaction = new EditProductMeasureInteraction
            {
                Units = unitViewModels,
                SelectedUnit = selectedUnit,
                SelectedAmount = selectedAmount,
                Callback = r =>
                {
                    if (r == null)
                    {
                        return;
                    }

                    productMeasure.Amount = r.Amount;
                    productMeasure.Unit.Prepare(r.Unit.Extract());
                }
            };

            _editProductMeasureInteraction.Raise(interaction);
        }

        /// <inheritdoc />
        public override void Prepare(Measure parameter)
        {
            Id = parameter.Id;
            Date = parameter.Date;
            Level = parameter.Level ?? 0;
            HasLevel = true;
            BreadUnits = parameter.BreadUnits;
            LongInsulin = parameter.LongInsulin;
            ShortInsulin = parameter.ShortInsulin;

            ProductMeasures.Clear();

            ProductMeasures
                .AddRange(
                    parameter.Products
                        .Select(
                            v =>
                            {
                                var viewModel = new ProductMeasureViewModel();

                                viewModel.Prepare(v);

                                return viewModel;
                            }));
        }
    }
}