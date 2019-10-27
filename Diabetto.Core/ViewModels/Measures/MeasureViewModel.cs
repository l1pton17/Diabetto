using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Diabetto.Core.Models;
using Diabetto.Core.Services;
using Diabetto.Core.Services.Repositories;
using Diabetto.Core.ViewModelResults;
using Diabetto.Core.ViewModels.Core;
using Diabetto.Core.ViewModels.ProductMeasures;
using Diabetto.Core.ViewModels.ProductMeasures.Dialogs;
using Diabetto.Core.ViewModels.Tags.Dialogs;
using DynamicData;
using DynamicData.Aggregation;
using MvvmCross.Navigation;
using ReactiveUI;
#pragma warning disable 618

namespace Diabetto.Core.ViewModels.Measures
{
    public class MeasureViewModel : MvxReactiveViewModel<Measure, EditResult<Measure>>
    {
        private readonly IProductMeasureViewModelFactory _productMeasureViewModelFactory;
        private readonly IProductService _productService;
        private readonly IMvxNavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly ITagService _tagService;
        private readonly ITimeProvider _timeProvider;
        private readonly IMeasureService _measureService;

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

        private readonly ObservableAsPropertyHelper<int> _productCount;
        public int ProductCount => _productCount.Value;

        private readonly ObservableAsPropertyHelper<float> _totalBreadUnits;
        public float TotalBreadUnits => _totalBreadUnits.Value;

        private readonly SourceList<ProductMeasureViewModel> _productMeasuresSource;
        private readonly ReadOnlyObservableCollection<ProductMeasureViewModel> _productMeasures;

        public ReadOnlyObservableCollection<ProductMeasureViewModel> ProductMeasures => _productMeasures;

        public ReactiveCommand<Unit, Unit> AddProductMeasureCommand { get; }

        public ReactiveCommand<ProductMeasureViewModel, Unit> DeleteProductMeasureCommand { get; }

        public ReactiveCommand<ProductMeasureViewModel, Unit> ProductMeasureSelectedCommand { get; }

        public ReactiveCommand<Unit, Unit> EditTagCommand { get; }

        public ReactiveCommand<Unit, bool> SaveCommand { get; }

        public MeasureViewModel(
            IMvxNavigationService navigationService,
            IProductMeasureViewModelFactory productMeasureViewModelFactory,
            IDialogService dialogService,
            ITagService tagService,
            IProductService productService,
            ITimeProvider timeProvider,
            IMeasureService measureService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _productMeasureViewModelFactory = productMeasureViewModelFactory ?? throw new ArgumentNullException(nameof(productMeasureViewModelFactory));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _measureService = measureService ?? throw new ArgumentNullException(nameof(measureService));

            this.WhenAnyValue(v => v.Level)
                .Where(v => v > 0)
                .Subscribe(_ => HasLevel = true);

            this.WhenAnyValue(v => v.Level, v => v.HasLevel)
                .Select(v => v.Item2 ? v.Item1 : (int?) null)
                .ToProperty(this, v => v.NullableLevel, out _nullableLevel);

            _productMeasuresSource = new SourceList<ProductMeasureViewModel>();

            _productMeasuresSource
                .Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _productMeasures)
                .DisposeMany()
                .Subscribe();

            _productMeasuresSource
                .Connect()
                .Count()
                .ToProperty(
                    this,
                    v => v.ProductCount,
                    out _productCount,
                    initialValue: 0);

            _productMeasuresSource
                .Connect()
                .WhenAnyPropertyChanged()
                .Select(_ => Unit.Default)
                .Merge(_productMeasuresSource.Connect().Select(_ => Unit.Default))
                .Select(
                    _ => ProductMeasures
                        .Select(v => v.BreadUnits)
                        .DefaultIfEmpty(0)
                        .Sum())
                .ToProperty(
                    this,
                    v => v.TotalBreadUnits,
                    out _totalBreadUnits,
                    initialValue: 0.0f);

            SaveCommand = ReactiveCommand.CreateFromTask(Save);

            AddProductMeasureCommand = ReactiveCommand.CreateFromTask(AddProductMeasure);

            EditTagCommand = ReactiveCommand.CreateFromTask(EditTag);

            DeleteProductMeasureCommand = ReactiveCommand
                .Create<ProductMeasureViewModel>(v => _productMeasuresSource.Remove(v));

            ProductMeasureSelectedCommand = ReactiveCommand
                .CreateFromTask<ProductMeasureViewModel>(ProductMeasureSelected);
        }

        private async Task<bool> Save()
        {
            var measure = Extract();

            if (measure.Id != 0)
            {
                await _measureService.EditAsync(measure);
            }
            else
            {
                await _measureService.AddAsync(measure);
            }

            var result = await _navigationService.Close(this, EditResult.Create(measure));

            return result;
        }

        private Measure Extract()
        {
            return new Measure
            {
                Id = Id,
                Date = Date,
                Level = HasLevel ? (int?)Level : null,
                Tag = Tag,
                TagId = Tag?.Id,
                LongInsulin = LongInsulin,
                ShortInsulin = ShortInsulin,
                Products = ProductMeasures
                    .Select(v => v.Extract())
                    .ToList()
            };
        }

        private async Task EditTag()
        {
            var tags = await _tagService.GetAll();
            var source = new SelectTagPickerViewModel(tags);

            source.SelectedItem1 = source.Item1Values.First(v => Tag == null ? v.IsEmpty : v.Item?.Id == Tag.Id);

            var isOk = await _dialogService.ShowPicker(source);

            if (!isOk)
            {
                return;
            }

            Tag = source.SelectedItem1.IsEmpty ? null : source.SelectedItem1.Item;
        }

        private async Task AddProductMeasure()
        {
            var result = await _navigationService.Navigate<AddProductMeasureViewModel, AddProductMeasureParameter, ProductMeasure>(new AddProductMeasureParameter(Id));

            if (result == null)
            {
                return;
            }

            var itemViewModel = _productMeasureViewModelFactory.Create();

            itemViewModel.Prepare(result);

            _productMeasuresSource.Add(itemViewModel);
        }

        private async Task ProductMeasureSelected(ProductMeasureViewModel productMeasure)
        {
            var product = await _productService.GetAsync(productMeasure.Unit.ProductId);
            var source = new SelectProductMeasureUnitPickerViewModel(product.Units);

            source.SelectedItem2 = source.Item2Values.First(v => v.Item.Id == productMeasure.Unit.Id);
            source.SelectedItem1 = source.Item1Values.First(v => v.Item == productMeasure.Amount);

            var isOk = await _dialogService.ShowPicker(source);

            if (!isOk)
            {
                return;
            }

            productMeasure.Unit.Prepare(source.SelectedItem2.Item);
            productMeasure.Amount = source.SelectedItem1.Item;
        }

        /// <inheritdoc />
        public override void Prepare(Measure parameter)
        {
            parameter ??= new Measure
            {
                Level = 55,
                Date = _timeProvider.UtcNow
            };

            Id = parameter.Id;
            Date = parameter.Date;
            Level = parameter.Level ?? 0;
            HasLevel = true;
            LongInsulin = parameter.LongInsulin;
            ShortInsulin = parameter.ShortInsulin;
            Tag = parameter.Tag;

            _productMeasuresSource.Clear();

            _productMeasuresSource
                .AddRange(
                    parameter.Products
                        .Select(
                            v =>
                            {
                                var viewModel = _productMeasureViewModelFactory.Create();

                                viewModel.Prepare(v);

                                return viewModel;
                            }));
        }
    }
}