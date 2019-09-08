using System;

namespace Diabetto.Core.Services
{
    public sealed class DisposableAction : IDisposable
    {
        private readonly Action _action;

        public DisposableAction(Action action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _action();
        }
    }
}