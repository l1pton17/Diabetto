using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Diabetto.Core.Models;
using Diabetto.Core.Services;
using Diabetto.Core.Services.Repositories;
using Diabetto.Core.ViewModelResults;
using Diabetto.Core.ViewModels.Core;
using Diabetto.Core.ViewModels.Measures;
using MvvmCross.Navigation;
using ReactiveUI;

namespace Diabetto.Core.ViewModels.Calendars
{
    public sealed class CalendarViewModel : MvxReactiveViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly ITimeProvider _timeProvider;
        private readonly IMeasureService _measureService;

        private readonly ObservableAsPropertyHelper<MonthViewModel> _month;
        public MonthViewModel Month => _month.Value;

        private readonly ObservableAsPropertyHelper<DateTime> _monthDate;
        public DateTime MonthDate => _monthDate.Value;

        public ReactiveCommand<MonthViewModel, MonthViewModel> MonthShowed { get; }

        public ReactiveCommand<Unit, Unit> AddCommand { get; }

        public CalendarViewModel(
            IMvxNavigationService navigationService,
            ITimeProvider timeProvider,
            IMeasureService measureService
        )
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _measureService = measureService ?? throw new ArgumentNullException(nameof(measureService));
            AddCommand = ReactiveCommand.CreateFromTask(Add);

            MonthShowed = ReactiveCommand.Create<MonthViewModel, MonthViewModel>(v => v);

            MonthShowed
                .Where(v => v != null)
                .ToProperty(this, v => v.Month, out _month);

            this.WhenAnyValue(v => v.Month)
                .Where(v => v != null)
                .Take(1)
                .Subscribe(
                    v =>
                    {
                        var day = v.Days.First(c => c.Day == _timeProvider.UtcNow.Day);

                        v.DaySelectedCommand.Execute(day);
                    });

            this.WhenAnyValue(v => v.Month)
                .Where(v => v != null)
                .Select(v => v.Month.Date)
                .ToProperty(this, v => v.MonthDate, out _monthDate, _timeProvider.UtcNow);
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
                await _navigationService.Navigate<MeasuresViewModel, MeasuresNavigationRequest, Measure>(new MeasuresNavigationRequest(result.Entity.Date.Date));
            }
        }
    }
}