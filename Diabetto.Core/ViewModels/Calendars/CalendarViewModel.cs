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

        private readonly ObservableAsPropertyHelper<MonthViewModel> _month;
        public MonthViewModel Month => _month.Value;

        private readonly ObservableAsPropertyHelper<DateTime> _monthDate;
        public DateTime MonthDate => _monthDate.Value;

        public ReactiveCommand<MonthViewModel, MonthViewModel> MonthShowed { get; }

        public ReactiveCommand<Unit, Unit> AddCommand { get; }

        public CalendarViewModel(
            IMvxNavigationService navigationService,
            ITimeProvider timeProvider
        )
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
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
                        var day = v.Days.First(c => c.Day == timeProvider.UtcNow.Day);

                        v.DaySelectedCommand.Execute(day);
                    });

            this.WhenAnyValue(v => v.Month)
                .Where(v => v != null)
                .Select(v => v.Month.Date)
                .ToProperty(this, v => v.MonthDate, out _monthDate, timeProvider.UtcNow);
        }

        private async Task Add()
        {
            var result = await _navigationService.Navigate<MeasureViewModel, Measure, EditResult<Measure>>(null);

            if (result?.Save == true)
            {
                await _navigationService.Navigate<MeasuresViewModel, MeasuresNavigationRequest, Measure>(new MeasuresNavigationRequest(result.Entity.Date.Date));
            }
        }
    }
}