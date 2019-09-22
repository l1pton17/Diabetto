using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Diabetto.Core.Models;
using Diabetto.Core.MvxInteraction.ProductMeasures;
using Diabetto.Core.Services;
using Diabetto.Core.Services.Repositories;
using Diabetto.Core.ViewModelResults;
using Diabetto.Core.ViewModels.Core;
using Diabetto.Core.ViewModels.ProductMeasures;
using Diabetto.Core.ViewModels.ProductMeasures.Dialogs;
using Diabetto.Core.ViewModels.ProductMeasureUnits;
using Diabetto.Core.ViewModels.Tags.Dialogs;
using MvvmCross.Navigation;
using ReactiveUI;
using ReactiveUI.Legacy;
#pragma warning disable 618

namespace Diabetto.Core.ViewModels.Measures
{
    public class MeasureViewModel : MvxReactiveViewModel<Measure, EditResult<Measure>>
    {
        private readonly IProductMeasureViewModelFactory _productMeasureViewModelFactory;
        private readonly IProductMeasureUnitService _productMeasureUnitService;
        private readonly IMvxNavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly ITagService _tagService;

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

        private float _breadUnits;
        public float BreadUnits
        {
            get => _breadUnits;
            set => SetProperty(ref _breadUnits, value);
        }

        public ReactiveList<ProductMeasureViewModel> ProductMeasures { get; }

        public ReactiveCommand<Unit, Unit> AddProductMeasureCommand { get; }

        public ReactiveCommand<ProductMeasureViewModel, Unit> DeleteProductMeasureCommand { get; }

        public ReactiveCommand<ProductMeasureViewModel, Unit> ProductMeasureSelectedCommand { get; }

        public ReactiveCommand<Unit, Unit> EditTagCommand { get; }

        public ReactiveCommand<Unit, bool> SaveCommand { get; }

        public MeasureViewModel(
            IMvxNavigationService navigationService,
            IProductMeasureUnitService productMeasureUnitService,
            IProductMeasureViewModelFactory productMeasureViewModelFactory,
            IDialogService dialogService,
            ITagService tagService
        )
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _productMeasureUnitService = productMeasureUnitService ?? throw new ArgumentNullException(nameof(productMeasureUnitService));
            _productMeasureViewModelFactory = productMeasureViewModelFactory ?? throw new ArgumentNullException(nameof(productMeasureViewModelFactory));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService));

            this.WhenAnyValue(v => v.Level)
                .Where(v => v > 0)
                .Subscribe(_ => HasLevel = true);

            this.WhenAnyValue(v => v.Level, v => v.HasLevel)
                .Select(v => v.Item2 ? v.Item1 : (int?) null)
                .ToProperty(this, v => v.NullableLevel, out _nullableLevel);

            ProductMeasures = new ReactiveList<ProductMeasureViewModel>();

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
                                Tag = Tag,
                                TagId = Tag?.Id,
                                LongInsulin = LongInsulin,
                                ShortInsulin = ShortInsulin,
                                BreadUnits = BreadUnits,
                                Products = ProductMeasures
                                    .Select(v => v.Extract())
                                    .ToList()
                            })));

            AddProductMeasureCommand = ReactiveCommand.CreateFromTask(AddProductMeasure);

            EditTagCommand = ReactiveCommand.CreateFromTask(EditTag);

            DeleteProductMeasureCommand = ReactiveCommand
                .Create<ProductMeasureViewModel>(v => ProductMeasures.Remove(v));

            ProductMeasureSelectedCommand = ReactiveCommand
                .CreateFromTask<ProductMeasureViewModel>(ProductMeasureSelected);
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

            ProductMeasures.Add(itemViewModel);
        }

        private async Task ProductMeasureSelected(ProductMeasureViewModel productMeasure)
        {
            var units = await _productMeasureUnitService.GetByProductId(productMeasure.Unit.ProductId);
            var source = new SelectProductMeasureUnitPickerViewModel(units);

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
            Id = parameter.Id;
            Date = parameter.Date;
            Level = parameter.Level ?? 0;
            HasLevel = true;
            BreadUnits = parameter.BreadUnits;
            LongInsulin = parameter.LongInsulin;
            ShortInsulin = parameter.ShortInsulin;
            Tag = parameter.Tag;

            ProductMeasures.Clear();

            ProductMeasures
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