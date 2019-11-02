using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Diabetto.Core.Models;
using Diabetto.Core.Services.Repositories;
using Diabetto.Core.ViewModelResults;
using Diabetto.Core.ViewModels.Core;
using DynamicData;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReactiveUI;
#pragma warning disable 618

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
        private readonly IMeasureCellViewModelFactory _cellViewModelFactory;

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        private readonly ObservableCollection<MeasureCellViewModel> _measures;
        public IEnumerable<MeasureCellViewModel> Measures => _measures;

        public ReactiveCommand<Unit, Unit> AddCommand { get; }

        public ReactiveCommand<MeasureCellViewModel, Unit> SelectedCommand { get; }

        public ReactiveCommand<MeasureCellViewModel, Unit> DeleteCommand { get; }

        private MvxNotifyTask _loadTask;
        public MvxNotifyTask LoadTask
        {
            get => _loadTask;
            private set => SetProperty(ref _loadTask, value);
        }

        public MeasuresViewModel(
            IMvxNavigationService navigationService,
            IMeasureService measureService,
            IMeasureCellViewModelFactory cellViewModelFactory
        )
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _measureService = measureService ?? throw new ArgumentNullException(nameof(measureService));
            _cellViewModelFactory = cellViewModelFactory ?? throw new ArgumentNullException(nameof(cellViewModelFactory));
            _measures = new ObservableCollection<MeasureCellViewModel>();

            SelectedCommand = ReactiveCommand.CreateFromTask<MeasureCellViewModel>(MeasureSelected);
            AddCommand = ReactiveCommand.CreateFromTask(Add);
            DeleteCommand = ReactiveCommand.CreateFromTask<MeasureCellViewModel>(Delete);
        }

        public override void ViewAppearing()
        {
            LoadTask = MvxNotifyTask.Create(LoadMeasures);
        }

        /// <inheritdoc />
        public override void Prepare(MeasuresNavigationRequest parameter)
        {
            Date = parameter.Day;
        }

        public override Task Initialize()
        {
            return Task.CompletedTask;
        }

        private async Task Delete(MeasureCellViewModel measureCell)
        {
            await _measureService.DeleteAsync(measureCell.Extract());

            LoadTask = MvxNotifyTask.Create(LoadMeasures);
        }

        private async Task Add()
        {
            var result = await _navigationService.Navigate<MeasureViewModel, Measure, EditResult<Measure>>(null);

            if (result?.Save == true)
            {
                LoadTask = MvxNotifyTask.Create(LoadMeasures);
            }
        }

        private async Task MeasureSelected(MeasureCellViewModel selectedMeasure)
        {
            var measure = await _measureService.GetAsync(selectedMeasure.Id);
            var result = await _navigationService.Navigate<MeasureViewModel, Measure, EditResult<Measure>>(measure);

            if (result?.Save == true)
            {
                LoadTask = MvxNotifyTask.Create(LoadMeasures);
            }
        }

        private async Task LoadMeasures()
        {
            var result = await _measureService.GetAsync(Date);



            _measures.Clear();

            _measures
                .AddRange(
                    result
                        .OrderBy(v => v.Date)
                        .Select(v => _cellViewModelFactory.Create(v)));
        }
    }
}