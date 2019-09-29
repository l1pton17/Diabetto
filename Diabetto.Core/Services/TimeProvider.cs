using System;

namespace Diabetto.Core.Services
{
    public interface ITimeProvider
    {
        DateTime UtcNow { get; }
    }

    public sealed class TimeProvider : ITimeProvider
    {
        /// <inheritdoc />
        public DateTime UtcNow => DateTime.UtcNow;
    }
}