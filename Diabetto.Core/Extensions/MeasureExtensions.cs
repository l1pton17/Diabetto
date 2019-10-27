using Diabetto.Core.Models;

namespace Diabetto.Core.Extensions
{
    public static class MeasureExtensions
    {
        public static double? GetLevel(this Measure measure)
        {
            return measure.Level / 10.0;
        }
    }
}