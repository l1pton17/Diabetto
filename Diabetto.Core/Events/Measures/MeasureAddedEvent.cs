using System;
using Diabetto.Core.Models;

namespace Diabetto.Core.Events.Measures
{
    public sealed class MeasureAddedEvent : IEvent
    {
        public Measure Value { get; }

        public MeasureAddedEvent(Measure value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}