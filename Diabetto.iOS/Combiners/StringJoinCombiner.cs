using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Combiners;

namespace Diabetto.iOS.Combiners
{
    public sealed class StringJoinCombiner : MvxValueCombiner
    {
        private readonly string _separator;

        public StringJoinCombiner(string separator)
        {
            _separator = separator ?? throw new ArgumentNullException(nameof(separator));
        }

        public override Type SourceType(IEnumerable<IMvxSourceStep> steps)
        {
            return typeof(string);
        }

        public override bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value)
        {
            value = String.Join(
                _separator,
                steps
                    .Select(v => v.GetValue())
                    .Where(v => v != null)
                    .Select(v => v.ToString()));

            return true;
        }
    }
}