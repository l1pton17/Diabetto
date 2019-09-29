using System;
using Diabetto.Core.Models;

namespace Diabetto.Core.Events.Measures
{
    public sealed class MeasureChangedEvent : IEvent
    {
        public Measure Value { get; }

        public MeasureChangedEvent(Measure value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}