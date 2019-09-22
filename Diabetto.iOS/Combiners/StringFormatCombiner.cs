using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Combiners;

namespace Diabetto.iOS.Combiners
{
    public sealed class StringFormatCombiner : MvxValueCombiner
    {
        private readonly string _format;

        public StringFormatCombiner(string format)
        {
            _format = format ?? throw new ArgumentNullException(nameof(format));
        }

        public override Type SourceType(IEnumerable<IMvxSourceStep> steps)
        {
            return typeof(string);
        }

        public override bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value)
        {
            value = String.Format(
                _format,
                steps
                    .Select(v => v.GetValue())
                    .Where(v => v != null)
                    .Select(v => (object) v.ToString())
                    .ToArray());

            return true;
        }
    }
}