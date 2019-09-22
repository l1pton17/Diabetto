using System;
using Diabetto.Core.Models;
using Diabetto.Core.Services;

namespace Diabetto.Core.ViewModels.Measures
{
    public interface IMeasureCellViewModelFactory
    {
        MeasureCellViewModel Create(Measure value);
    }

    internal sealed class MeasureCellViewModelFactory : IMeasureCellViewModelFactory
    {
        private readonly IBreadUnitsCalculator _breadUnitsCalculator;

        public MeasureCellViewModelFactory(IBreadUnitsCalculator breadUnitsCalculator)
        {
            _breadUnitsCalculator = breadUnitsCalculator ?? throw new ArgumentNullException(nameof(breadUnitsCalculator));
        }

        /// <inheritdoc />
        public MeasureCellViewModel Create(Measure value)
        {
            return new MeasureCellViewModel(_breadUnitsCalculator, value);
        }
    }
}