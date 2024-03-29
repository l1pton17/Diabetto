﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Diabetto.Core.Models;
using Diabetto.Core.Services.Repositories;
using Diabetto.Core.ViewModels.Core;
using Diabetto.Core.ViewModels.Measures;
using MvvmCross.Navigation;
using ReactiveUI;

namespace Diabetto.Core.ViewModels.Calendars
{
    public sealed class MonthViewModelParameter
    {
        public DateTime Date { get; }

        public MonthViewModelParameter(DateTime date)
        {
            Date = date;
        }
    }

    public sealed class MonthViewModel : MvxReactiveViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public List<CalendarCellViewModel> Days { get; }
        public DateTime Month { get; }

        public ReactiveCommand<CalendarCellViewModel, Unit> DaySelectedCommand { get; }

        public MonthViewModel(
            IMeasureService measureService,
            IMvxNavigationService navigationService,
            DateTime month)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            Month = month;
            DaySelectedCommand = ReactiveCommand.CreateFromTask<CalendarCellViewModel>(DaySelected);
            Days = GenerateCells(month);
        }
        
        private async Task DaySelected(CalendarCellViewModel day)
        {
            await _navigationService.Navigate<MeasuresViewModel, MeasuresNavigationRequest, Measure>(new MeasuresNavigationRequest(day.Date));
        }

        private List<CalendarCellViewModel> GenerateCells(DateTime firstMonthDate)
        {
            var dayCount = firstMonthDate
                .AddMonths(1)
                .AddDays(-1)
                .Day;

            var firstDayOfWeek = (int) CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            var firstMonthDateDayOfWeek = (int) firstMonthDate.DayOfWeek;

            if (firstMonthDateDayOfWeek < firstDayOfWeek)
            {
                firstMonthDateDayOfWeek += 7;
            }

            var monthDays = Enumerable
                .Range(0, dayCount)
                .Select(
                    v => new CalendarCellViewModel(
                        CalendarCellType.Normal,
                        firstMonthDate.AddDays(v)));

            var previousMonthDays = Enumerable
                .Range(1, firstMonthDateDayOfWeek - firstDayOfWeek)
                .Reverse()
                .Select(
                    v => new CalendarCellViewModel(
                        CalendarCellType.PreviousMonth,
                        firstMonthDate.AddDays(-v))
                    {
                        IsHidden = true
                    });

            var days = previousMonthDays.Union(monthDays).ToList();

            return days;
        }
    }
}