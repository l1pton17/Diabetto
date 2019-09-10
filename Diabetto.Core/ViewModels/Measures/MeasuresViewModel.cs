using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Diabetto.Core.Models;
using Diabetto.Core.Services;
using Diabetto.Core.ViewModelResults;
using Diabetto.Core.ViewModels.Core;
using DynamicData;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReactiveUI;
using ReactiveUI.Legacy;

namespace Diabetto.Core.ViewModels.Measures
{
    public sealed class MeasuresNavigationRequest
    {
        public DateTime Day { get; }

        public MeasuresNavigationRequest(DateTime day)
        {
            Day = day;
        }
    }

    public sealed class MeasuresViewModel : MvxReactiveViewModel<MeasuresNavigationRequest, Measure>
    {
        private readonly IMeasureService _measureService;
        private readonly IMvxNavigationService _navigationService;
        private readonly ITimeProvider _timeProvider;

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        private SourceList<Measure> _measures;
        public SourceList<Measure> Measures
        {
            get => _measures;
            set => SetProperty(ref _measures, value);
        }

        public ReactiveCommand<Unit, Unit> AddCommand { get; }

        public ReactiveCommand<Measure, Unit> SelectedCommand { get; }

        public ReactiveCommand<Measure, Unit> DeleteCommand { get; }

        public MvxNotifyTask LoadTask { get; private set; }

        public MeasuresViewModel(
            IMvxNavigationService navigationService,
            IMeasureService measureService,
            ITimeProvider timeProvider
        )
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _measureService = measureService ?? throw new ArgumentNullException(nameof(measureService));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            Measures = new SourceList<Measure>();
            SelectedCommand = ReactiveCommand.CreateFromTask<Measure>(MeasureSelected);
            AddCommand = ReactiveCommand.CreateFromTask(Add);
            DeleteCommand = ReactiveCommand.CreateFromTask<Measure>(Delete);
        }

        /// <inheritdoc />
        public override void Prepare(MeasuresNavigationRequest parameter)
        {
            Date = parameter.Day;
        }

        public override async Task Initialize()
        {
            await LoadMeasures();
            LoadTask = MvxNotifyTask.Create(LoadMeasures);
            await base.Initialize();
        }

        private async Task Delete(Measure measure)
        {
            await _measureService.DeleteAsync(measure.Id);
            await LoadMeasures();
        }

        private async Task Add()
        {
            var result = await _navigationService.Navigate<MeasureViewModel, Measure, EditResult<Measure>>(
                new Measure
                {
                    Date = _timeProvider.UtcNow,
                    Level = 55
                });

            if (result?.Save == true)
            {
                await _measureService.AddAsync(result.Entity);
                await LoadMeasures();
            }
        }

        private async Task MeasureSelected(Measure selectedMeasure)
        {
            var result = await _navigationService.Navigate<MeasureViewModel, Measure, EditResult<Measure>>(selectedMeasure);

            if (result?.Save == true)
            {
                await _measureService.EditAsync(result.Entity);
                await LoadMeasures();
            }
        }

        private async Task LoadMeasures()
        {
            var result = await _measureService.GetAsync(Date);

            Measures.Clear();
            Measures.AddRange(result.OrderBy(v => v.Date));
        }
    }
}