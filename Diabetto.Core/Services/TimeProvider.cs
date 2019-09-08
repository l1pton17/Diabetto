using System;

namespace Diabetto.Core.Services
{
    public interface ITimeProvider
    {
        DateTime Now { get; }

        DateTime UtcNow { get; }
    }

    public sealed class TimeProvider : ITimeProvider
    {
        public DateTime Now => DateTime.Now;

        /// <inheritdoc />
        public DateTime UtcNow => DateTime.UtcNow;
    }
}