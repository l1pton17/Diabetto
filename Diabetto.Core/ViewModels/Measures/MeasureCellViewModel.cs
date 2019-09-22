using System;
using System.Linq;
using Diabetto.Core.Models;
using Diabetto.Core.Services;
using Diabetto.Core.ViewModels.Core;

namespace Diabetto.Core.ViewModels.Measures
{
    public sealed class MeasureCellViewModel : MvxReactiveViewModel
    {
        private readonly Measure _measure;

        public int Id => _measure.Id;

        public DateTime Date => _measure.Date;

        public int? Level => _measure.Level;

        public float BreadUnits { get; }

        public int LongInsulin => _measure.LongInsulin;

        public int ShortInsulin => _measure.ShortInsulin;

        public MeasureCellViewModel(
            IBreadUnitsCalculator breadUnitsCalculator,
            Measure measure)
        {
            _measure = measure ?? throw new ArgumentNullException(nameof(measure));

            BreadUnits = measure.Products
                .Select(v => breadUnitsCalculator.Calculate(v.Amount, v.ProductMeasureUnit.Carbohydrates))
                .DefaultIfEmpty(0)
                .Sum();
        }
    }
}