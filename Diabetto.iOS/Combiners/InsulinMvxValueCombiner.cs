using System;
using System.Collections.Generic;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Combiners;

namespace Diabetto.iOS.Combiners
{
    public sealed class InsulinMvxValueCombiner : MvxValueCombiner
    {
        public static readonly InsulinMvxValueCombiner Instance = new InsulinMvxValueCombiner();

        private InsulinMvxValueCombiner()
        {
        }

        public override Type SourceType(IEnumerable<IMvxSourceStep> steps)
        {
            return typeof(int);
        }

        public override bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value)
        {
            var (shortInsulin, longInsulin) = Extract(steps);

            value = Format(shortInsulin, longInsulin);

            return true;
        }

        private (int Short, int Long) Extract(IEnumerable<IMvxSourceStep> steps)
        {
            var shortInsulin = 0;
            var longInsulin = 0;
            var idx = 0;

            foreach (var step in steps)
            {
                if (idx == 0)
                {
                    shortInsulin = (int) step.GetValue();
                }
                else if (idx == 1)
                {
                    longInsulin = (int) step.GetValue();
                }
                else
                {
                    break;
                }

                idx++;
            }

            return (shortInsulin, longInsulin);
        }

        private static string Format(int shortInsulin, int longInsulin)
        {
            if (longInsulin == 0 && shortInsulin == 0)
            {
                return "--";
            }
            else if (shortInsulin == 0)
            {
                return String.Concat(longInsulin, "ипд");
            }
            else if (longInsulin == 0)
            {
                return shortInsulin.ToString();
            }

            return String.Concat(
                shortInsulin,
                "+",
                longInsulin,
                "ипд");
        }
    }
}