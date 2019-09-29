using System;
using Diabetto.Core.Models;

namespace Diabetto.Core.Events.Measures
{
    public sealed class MeasureDeletedEvent :  IEvent
    {
        public Measure Value { get; }
        
        public MeasureDeletedEvent(Measure value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}