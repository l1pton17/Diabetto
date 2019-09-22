﻿using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Diabetto.Core.Models;
using Diabetto.Core.Services;
using Diabetto.Core.Services.Repositories;
using Diabetto.Core.ViewModelResults;
using Diabetto.Core.ViewModels.Core;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReactiveUI;
using ReactiveUI.Legacy;
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
        private readonly ITimeProvider _timeProvider;

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public ReactiveList<Measure> Measures { get; }

        public ReactiveCommand<Unit, Unit> AddCommand { get; }

        public ReactiveCommand<Measure, Unit> SelectedCommand { get; }

        public ReactiveCommand<Measure, Unit> DeleteCommand { get; }

        private MvxNotifyTask _loadTask;
        public MvxNotifyTask LoadTask
        {
            get => _loadTask;
            private set => SetProperty(ref _loadTask, value);
        }

        public MeasuresViewModel(
            IMvxNavigationService navigationService,
            IMeasureService measureService,
            ITimeProvider timeProvider
        )
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _measureService = measureService ?? throw new ArgumentNullException(nameof(measureService));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));

            Measures = new ReactiveList<Measure>();

            SelectedCommand = ReactiveCommand.CreateFromTask<Measure>(MeasureSelected);
            AddCommand = ReactiveCommand.CreateFromTask(Add);
            DeleteCommand = ReactiveCommand.CreateFromTask<Measure>(Delete);
        }

        /// <inheritdoc />
        public override void Prepare(MeasuresNavigationRequest parameter)
        {
            Date = parameter.Day;
        }

        public override Task Initialize()
        {
            LoadTask = MvxNotifyTask.Create(LoadMeasures);

            return Task.CompletedTask;
        }

        private async Task Delete(Measure measure)
        {
            await _measureService.DeleteAsync(measure.Id);
            LoadTask = MvxNotifyTask.Create(LoadMeasures);
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
                LoadTask = MvxNotifyTask.Create(LoadMeasures);
            }
        }

        private async Task MeasureSelected(Measure selectedMeasure)
        {
            var measure = await _measureService.GetAsync(selectedMeasure.Id);
            var result = await _navigationService.Navigate<MeasureViewModel, Measure, EditResult<Measure>>(measure);

            if (result?.Save == true)
            {
                await _measureService.EditAsync(result.Entity);
                LoadTask = MvxNotifyTask.Create(LoadMeasures);
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