using System;
using Diabetto.Core.Services;
using DynamicData;
using MvvmCross.Navigation;

namespace Diabetto.Core.ViewModels.Calendars
{
    public interface IMonthViewModelFactory
    {
        MonthViewModel Create(DateTime month);
    }

    public sealed class MonthViewModelFactory : IMonthViewModelFactory
    {
        private readonly ICache<MonthViewModel, DateTime> _cache;
        private readonly IMeasureService _measureService;
        private readonly IMvxNavigationService _navigationService;

        public MonthViewModelFactory(
            IMeasureService measureService,
            IMvxNavigationService navigationService)
        {
            _measureService = measureService ?? throw new ArgumentNullException(nameof(measureService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _cache = new ChangeAwareCache<MonthViewModel, DateTime>();
        }

        /// <inheritdoc />
        public MonthViewModel Create(DateTime month)
        {
            var value = _cache.Lookup(month);

            if (value.HasValue)
            {
                return value.Value;
            }

            var viewModel = new MonthViewModel(
                _measureService,
                _navigationService,
                month);

            _cache.AddOrUpdate(viewModel, month);

            return viewModel;
        }
    }
}